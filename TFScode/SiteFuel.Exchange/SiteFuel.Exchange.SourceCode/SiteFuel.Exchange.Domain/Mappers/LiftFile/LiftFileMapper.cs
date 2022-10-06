using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class LiftFileMapper
    {
        public static LiftFileDetail ToEntity(this LiftFileViewModel viewModel, LiftFileDetail entity = null)
        {
            if (entity == null)
                entity = new LiftFileDetail();

            entity.AddedBy = viewModel.AddedByUserId;
            entity.AddedDate = viewModel.AddedDate;
            entity.CompanyId = viewModel.CompanyId;
            entity.ExternalRefId = viewModel.ExternalRefId;
            entity.IsActive = true;
            //entity.LFIDsxsfsh
            foreach (var item in viewModel.LiftFileRecords)
            {
                entity.LiftFileValidationRecords.Add(item.ToEntity());
            }

            return entity;
        }

        public static LiftFileValidationRecord ToEntity(this LiftFileRecordsViewModel viewModel, LiftFileValidationRecord entity = null)
        {
            if (entity == null)
                entity = new LiftFileValidationRecord();

            entity.BOL = viewModel.BOL?.Trim();
            entity.CarrierID = viewModel.CarrierID?.Trim();
            entity.CarrierName = viewModel.CarrierName?.Trim();
            entity.CIN = viewModel.CIN?.Trim();
            entity.CNID = viewModel.CNID?.Trim();
            entity.CorrectedQty = viewModel.CorrectedQty;
            entity.Density = viewModel.Density;
            entity.DriverBadge = viewModel.Driver_Badge;
            entity.EndTime = viewModel.EndTime?.Trim();
            entity.Filename = viewModel.Filename?.Trim();
            entity.Gross = viewModel.Gross;
            entity.IsActive = true;
            entity.LoadDate = viewModel.LoadDate?.Trim();
            entity.POFromAPI = viewModel.PO?.Trim();
            entity.Status = viewModel.Status;
            entity.Temp = viewModel.Temp;
            entity.StatusChangedDate = viewModel.StatusChangedDate;
            entity.AddedDate = viewModel.AddedDate;
            entity.TerminalCode = viewModel.Terminal_Code?.Trim();
            entity.TermItemCode = viewModel.TermItemCode?.Trim();
            entity.VendorItemCode = viewModel.VendorItemCode?.Trim();
            entity.TruckNum = viewModel.TruckNum?.Trim();
            entity.InTime = viewModel.InTime?.Trim();
            entity.StartTime = viewModel.StartTime?.Trim();
            entity.StopTime = viewModel.StopTime?.Trim();
            entity.VendorOrginalRef = viewModel.Vendor_Orginal_Ref?.Trim();
            entity.MeterRecords = viewModel.MeterRecords?.Trim();
            entity.Customer = viewModel.Customer?.Trim();
            return entity;
        }

        public static LiftFileViewModel ToViewModel(this LiftFileValidateRequest liftFileRequest, UserContext userContext, ApiResponseViewModel apiResponse, LiftFileViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new LiftFileViewModel();

            viewModel.AddedByUserId = userContext.Id;
            viewModel.AddedDate = DateTimeOffset.Now;
            viewModel.CompanyId = userContext.CompanyId;
            //viewModel.ExternalRefId = liftFileRequest.ExternalRefId;
            viewModel.IsActive = true;

            viewModel.LiftFileRecords = new List<LiftFileRecordsViewModel>();
            viewModel.LiftFileRecords.AddRange(liftFileRequest.Data.ToViewModel(apiResponse));

            return viewModel;
        }

        public static List<LiftFileRecordsViewModel> ToViewModel(this List<LiftFileItemsViewModel> liftFileRequests, ApiResponseViewModel apiResponse, List<LiftFileRecordsViewModel> viewModels = null)
        {
            if (viewModels == null)
                viewModels = new List<LiftFileRecordsViewModel>();

            List<string> hashCodeList = new List<string>();

            var groupedByParameters = liftFileRequests
                                        .GroupBy(t => new { t.BOL, t.Terminal_Code, t.TermItemCode, t.LoadDate, t.Filename })
                                        .ToList();
            try
            {
                foreach (var groupItem in groupedByParameters)
                {
                    var newRecord = new LiftFileRecordsViewModel();
                    var item = groupItem.FirstOrDefault();
                    if (item != null)
                    {
                        newRecord = new LiftFileRecordsViewModel()
                        {
                            BOL = item.BOL,
                            CarrierID = item.CarrierID,
                            CarrierName = item.CarrierName,
                            CIN = item.CIN,
                            CNID = item.CNID,
                            CorrectedQty = groupItem.Sum(t => t.CorrectedQty),
                            Density = item.Density,
                            EndTime = item.EndTime,
                            Filename = item.Filename,
                            Gross = groupItem.Sum(t => t.Gross),
                            IsActive = true,
                            LoadDate = item.LoadDate,
                            PO = item.PO,
                            Status = Utilities.LFVRecordStatus.PendingMatch,
                            StatusChangedDate = DateTimeOffset.Now,
                            AddedDate = DateTimeOffset.Now,
                            Temp = item.Temp,
                            Terminal_Code = item.Terminal_Code,
                            TermItemCode = item.TermItemCode,
                            VendorItemCode = item.VendorItemCode,
                            Driver_Badge = item.Driver_Badge,
                            InTime = item.InTime,
                            MeterRecords = item.MeterRecords,
                            StartTime = item.StartTime,
                            StopTime = item.StopTime,
                            TruckNum = item.TruckNum,
                            Vendor_Orginal_Ref = item.Vendor_Orginal_Ref,
                            Customer = item.Customer
                        };
                    }
                    var newRecodHexString = newRecord.GetHexString();

                    if (hashCodeList.Any())
                    {
                        if (hashCodeList.Contains(newRecodHexString))
                            newRecord.Status = Utilities.LFVRecordStatus.Duplicate;
                    }

                    hashCodeList.Add(newRecodHexString);
                    viewModels.Add(newRecord);
                }
            }
            catch (Exception ex)
            {
                apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF02, Message = ex.Message });
            }

            return viewModels;
        }

        public static string GetHexString(this LiftFileRecordsViewModel liftRecord)
        {
            var fields = typeof(LiftFileRecordsViewModel).GetProperties().Where(t => t.Name != "StatusChangedDate" && t.Name != "AddedDate" && t.Name != "LiftFileId" && t.Name != "Id").ToArray();
            var result = String.Join(",", fields.Select(f => f.GetValue(liftRecord)));
            var hexString = string.Join("", result.Select(c => ((int)c).ToString("X2")));
            return hexString;
        }

        public static LiftFileResponseDetails ToResponseViewModel(this LiftFileValidationRecord item, bool isSelfHauldingMatchFound = false, LiftFileResponseDetails viewModel = null)
        {
            if (item == null)
                return null;

            return new LiftFileResponseDetails()
            {
                BOL = item.BOL,
                CarrierID = item.CarrierID,
                CarrierName = item.CarrierName,
                CIN = item.CIN,
                CNID = item.CNID ?? string.Empty,
                CorrectedQty = item.CorrectedQty,
                Density = item.Density,
                EndTime = item.EndTime,
                Filename = item.Filename,
                Gross = item.Gross,
                LoadDate = item.LoadDate,
                PO = item.POFromAPI,
                Temp = item.Temp,
                Terminal_Code = item.TerminalCode,
                TermItemCode = item.TermItemCode,
                VendorItemCode = item.VendorItemCode,
                Driver_Badge = item.DriverBadge,
                InTime = item.InTime,
                MeterRecords = item.MeterRecords,
                StartTime = item.StartTime,
                StopTime = item.StopTime ?? string.Empty,
                TruckNum = item.TruckNum,
                Vendor_Orginal_Ref = item.VendorOrginalRef,
                Customer = item.Status == Utilities.LFVRecordStatus.IgnoreMatch && isSelfHauldingMatchFound ? item.POFromAPI : (string.IsNullOrWhiteSpace(item.Customer) ? string.Empty : item.Customer),
                RecordStatus = item.Status.ToString(),
                InvoiceFtlDetailId = item.InvoiceFtlDetailId ?? 0,
                LiftFileRecordId = item.Id
            };
        }

        // Set the AddedDate to DateTime today
        // Change status of newly added record as pending
        public static LiftFileValidationRecord ToEntityForStatusUpdate(this LiftFileValidationRecord entityToUpdate, UserContext userContext)
        {
            LiftFileValidationRecord entity = new LiftFileValidationRecord();
            if (entityToUpdate != null)
            {
                entity.AddedDate = DateTimeOffset.Now;
                entity.IsActive = true;
                entity.BOL = entityToUpdate.BOL;
                entity.Gross = entityToUpdate.Gross;
                entity.InTime = entityToUpdate.InTime;
                entity.LiftFileId = entityToUpdate.LiftFileId;
                entity.LoadDate = entityToUpdate.LoadDate;
                entity.MeterRecords = entityToUpdate.MeterRecords;
                entity.POFromAPI = entityToUpdate.POFromAPI;
                entity.StartTime = entityToUpdate.StartTime;
                entity.StopTime = entityToUpdate.StopTime;
                entity.Status = Utilities.LFVRecordStatus.PendingMatch;
                entity.StatusChangedDate = DateTimeOffset.Now;
                entity.Temp = entityToUpdate.Temp;
                entity.TerminalCode = entityToUpdate.TerminalCode;
                entity.TermItemCode = entityToUpdate.TermItemCode;
                entity.TruckNum = entityToUpdate.TruckNum;
                entity.VendorItemCode = entityToUpdate.VendorItemCode;
                entity.VendorOrginalRef = entityToUpdate.VendorOrginalRef;
                entity.CarrierID = entityToUpdate.CarrierID;
                entity.CarrierName = entityToUpdate.CarrierName;
                entity.CIN = entityToUpdate.CIN;
                entity.CNID = entityToUpdate.CNID;
                entity.CorrectedQty = entityToUpdate.CorrectedQty;
                entity.Density = entityToUpdate.Density;
                entity.DriverBadge = entityToUpdate.DriverBadge;
                entity.EndTime = entityToUpdate.EndTime;
                entity.Filename = entityToUpdate.Filename;
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
            }
            return entity;
        }

        public static LiftFileBadgeManagementDetail ToEntity(this BadgeApiResponseViewModel viewModel, LiftFileBadgeManagementDetail entity = null)
        {
            if (entity == null)
                entity = new LiftFileBadgeManagementDetail();

            entity.AddedByCompanyId = 1;
            entity.AddedDate = DateTimeOffset.Now;
            entity.BadgeNumber = viewModel.Badge_Number;
            entity.CustomerNumber = viewModel.Customer_Number;
            entity.StatusCode = viewModel.Status?.Trim();
            entity.IsActive = true;

            return entity;
        }

        public static LfRecordsReportscsvViewModel ToCsvViewModel(this LFRecordsGridViewModel viewModel)
        {
            var csvViewModel = new LfRecordsReportscsvViewModel();
            csvViewModel.CallID = viewModel.CallId.ToString();
            csvViewModel.Bol = viewModel.bol;
            csvViewModel.Terminal = viewModel.TerminalName; //sending Terminal Code in LF Record
            csvViewModel.CorrectedQuantity = viewModel.correctedQuantity.ToString();
            csvViewModel.TerminalItemCode = viewModel.TerminalItemCode;
            csvViewModel.ProductType = viewModel.ProductType;
            csvViewModel.LoadDate = viewModel.LoadDate;
            csvViewModel.RecordDate = viewModel.RecordDate;
            csvViewModel.CarrierName = viewModel.CarrierName;
            //csvViewModel.TerminalName = viewModel.Terminal; //Name of mapped terminal/BULKPLANT
            csvViewModel.Status = viewModel.recordStatus;
            csvViewModel.Reason = viewModel.Reason;
            csvViewModel.CarrierID = viewModel.CarrierID;
            csvViewModel.FileName = viewModel.FileName;
            csvViewModel.Username = viewModel.Username;
            csvViewModel.ModifiedDate = viewModel.ModifiedDate;
            csvViewModel.ReasonCode = viewModel.ReasonCode;
            csvViewModel.ReasonCategory = viewModel.ReasonCategory;
            csvViewModel.ResolutionTime = viewModel.LFVResolutionTime;
            csvViewModel.TimeToBOL = viewModel.TimeToBol;
            return csvViewModel;
        }
        public static LfRecordsCarrierReportsViewModel ToCarrierLFVCsvViewModel(this LFRecordsGridViewModel viewModel)
        {
            var csvViewModel = new LfRecordsCarrierReportsViewModel();
            csvViewModel.CallID = viewModel.CallId.ToString();
            csvViewModel.Bol = viewModel.bol;
            csvViewModel.Terminal = viewModel.TerminalName; //sending Terminal Code in LF Record
            csvViewModel.CorrectedQuantity = viewModel.correctedQuantity.ToString();
            csvViewModel.TerminalItemCode = viewModel.TerminalItemCode;
            csvViewModel.ProductType = viewModel.ProductType;
            csvViewModel.LoadDate = viewModel.LoadDate;
            csvViewModel.RecordDate = viewModel.RecordDate;
            csvViewModel.CarrierName = viewModel.CarrierName;
            //csvViewModel.TerminalName = viewModel.Terminal; //Name of mapped terminal/BULKPLANT
            csvViewModel.Status = viewModel.recordStatus;
            csvViewModel.Reason = viewModel.Reason;
            csvViewModel.CarrierID = viewModel.CarrierID;
            csvViewModel.FileName = viewModel.FileName;
            csvViewModel.statusChangeDate = viewModel.statusChangeDate;
            csvViewModel.IgnoredReason = viewModel.IgnoredReason;
            csvViewModel.ForcedIgnoreReason = viewModel.ForcedIgnoreReason;
            csvViewModel.ReasonCode = viewModel.ReasonCode;
            csvViewModel.ReasonCategory = viewModel.ReasonCategory;
            csvViewModel.Username = viewModel.Username;
            csvViewModel.ModifiedDate = viewModel.ModifiedDate;
            csvViewModel.LFVCarrierPerModifiedDate = viewModel.LFVCarrierPerModifiedDate;
            return csvViewModel;
        }

        public static LiftFileValidationRecord ToEntityForForcedIgnoreMatch(this LiftFileValidationRecord entityToUpdate, UserContext userContext)
        {
            LiftFileValidationRecord entity = new LiftFileValidationRecord();
            if (entityToUpdate != null)
            {
                entity.AddedDate = DateTimeOffset.Now;
                entity.IsActive = true;
                entity.BOL = entityToUpdate.BOL;
                entity.Gross = entityToUpdate.Gross;
                entity.InTime = entityToUpdate.InTime;
                entity.LiftFileId = entityToUpdate.LiftFileId;
                entity.LoadDate = entityToUpdate.LoadDate;
                entity.MeterRecords = entityToUpdate.MeterRecords;
                entity.POFromAPI = entityToUpdate.POFromAPI;
                entity.StartTime = entityToUpdate.StartTime;
                entity.StopTime = entityToUpdate.StopTime;
                entity.Status = Utilities.LFVRecordStatus.ForcedIgnore;
                entity.StatusChangedDate = DateTimeOffset.Now;
                entity.Temp = entityToUpdate.Temp;
                entity.TerminalCode = entityToUpdate.TerminalCode;
                entity.TermItemCode = entityToUpdate.TermItemCode;
                entity.TruckNum = entityToUpdate.TruckNum;
                entity.VendorItemCode = entityToUpdate.VendorItemCode;
                entity.VendorOrginalRef = entityToUpdate.VendorOrginalRef;
                entity.CarrierID = entityToUpdate.CarrierID;
                entity.CarrierName = entityToUpdate.CarrierName;
                entity.CIN = entityToUpdate.CIN;
                entity.CNID = entityToUpdate.CNID;
                entity.CorrectedQty = entityToUpdate.CorrectedQty;
                entity.Density = entityToUpdate.Density;
                entity.DriverBadge = entityToUpdate.DriverBadge;
                entity.EndTime = entityToUpdate.EndTime;
                entity.Filename = entityToUpdate.Filename;
                entity.Reason = Resource.lblIgnoreReasonForcedIgnore;
                entity.Customer = entityToUpdate.Customer;
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
            }
            return entity;
        }

        public static LiftFileHoldRecords ToLiftFileHoldRecordEntity(int liftRecId, string jsonString)
        {
            var entity = new LiftFileHoldRecords();

            entity.LiftFileRecordId = liftRecId;
            entity.JsonString = jsonString;
            entity.IsActive = true;
            entity.UpdatedDate = DateTimeOffset.Now;

            return entity;
        }

        public static LiftFileValidationRecord ToEntityForLiftFileRecordEdit(this LiftFileValidationRecord entityToUpdate, LfvParameterViewModel lfvParameters, LFRecordsGridViewModel editedValues, UserContext userContext)
        {
            LiftFileValidationRecord entity = new LiftFileValidationRecord();
            if (entityToUpdate != null && lfvParameters != null && editedValues != null)
            {
                entity.AddedDate = DateTimeOffset.Now;
                entity.IsActive = true;
                entity.BOL = lfvParameters.IsBolReq ? editedValues.bol : entityToUpdate.BOL;
                entity.Gross = (lfvParameters.IsGrossReq || lfvParameters.IsCorrectedQtyRes) ? editedValues.GrossQuantity : entityToUpdate.Gross;
                entity.InTime = entityToUpdate.InTime;
                entity.LiftFileId = entityToUpdate.LiftFileId;
                entity.LoadDate = lfvParameters.IsLoadDateReq ? editedValues.LoadDate : entityToUpdate.LoadDate;
                entity.MeterRecords = entityToUpdate.MeterRecords;
                entity.POFromAPI = entityToUpdate.POFromAPI;
                entity.StartTime = entityToUpdate.StartTime;
                entity.StopTime = entityToUpdate.StopTime;
                entity.Status = Utilities.LFVRecordStatus.PendingMatch;
                entity.StatusChangedDate = DateTimeOffset.Now;
                entity.Temp = entityToUpdate.Temp;
                entity.TerminalCode = lfvParameters.IsTerminalCodeReq ? editedValues.Terminal : entityToUpdate.TerminalCode;
                entity.TermItemCode = lfvParameters.IsTermItemCodeReq ? editedValues.TerminalItemCode : entityToUpdate.TermItemCode;
                entity.TruckNum = entityToUpdate.TruckNum;
                entity.VendorItemCode = entityToUpdate.VendorItemCode;
                entity.VendorOrginalRef = entityToUpdate.VendorOrginalRef;
                entity.CarrierID = entityToUpdate.CarrierID;
                entity.CarrierName = lfvParameters.IsCarrierNameReq ? editedValues.CarrierName : entityToUpdate.CarrierName;
                entity.CIN = lfvParameters.IsCINReq ? editedValues.CIN : entityToUpdate.CIN;
                entity.CNID = entityToUpdate.CNID;
                entity.CorrectedQty = (lfvParameters.IsCorrectedQtyRes || lfvParameters.IsGrossReq) ? editedValues.correctedQuantity : entityToUpdate.CorrectedQty;
                entity.Density = entityToUpdate.Density;
                entity.DriverBadge = entityToUpdate.DriverBadge;
                entity.EndTime = entityToUpdate.EndTime;
                entity.Filename = entityToUpdate.Filename;
                entity.Reason = null;
                entity.Customer = entityToUpdate.Customer;
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
            }
            return entity;
        }

        public static ReasonCodeDetail ToNewEntityFromExisting(this ReasonCodeDetail reasonCode, UserContext userContext)
        {
            if (reasonCode != null)
            {
                return new ReasonCodeDetail()
                {
                    CompanyId = userContext.CompanyId,
                    CreatedBy = reasonCode.CreatedBy,
                    CreatedDate = reasonCode.CreatedDate,
                    Description = reasonCode.Description,
                    IsActive = true,
                    IsDeleted = false,
                    ReasonCode = reasonCode.ReasonCode,
                    UpdatedBy = userContext.Id,
                    UpdatedDate = DateTimeOffset.Now,
                };
            }

            return null;
        }

        public static ReasonCategory ToEntity(this ReasonCategoryViewModel reasonCategoryViewModel, UserContext userContext, bool isEdited = false, ReasonCategory entity = null)
        {
            if (entity == null)
                entity = new ReasonCategory();

            entity.CompanyId = userContext.CompanyId;
            entity.Name = reasonCategoryViewModel.Name;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedBy = userContext.Id;
            entity.CreatedDate = DateTimeOffset.Now;

            if(isEdited)
            {
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
            }
            
            return entity;
        }


        public static ReasonCodeDetail ToEntity(this ReasonCodeModel reasonCodeModel, UserContext userContext, ReasonCodeDetail entity = null)
        {
            if (entity == null)
                entity = new ReasonCodeDetail();

            entity.CategoryId = reasonCodeModel.CategoryId;           
            entity.ReasonCode = reasonCodeModel.ReasonCode;
            entity.Description = reasonCodeModel.Description;
            entity.CompanyId = userContext.CompanyId;
            entity.CreatedBy = userContext.Id;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.IsDeleted = reasonCodeModel.IsDeleted;
            return entity;
        }
    }
}
