using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.LiftFile;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.Domain
{
    public class LFVDomain : BaseDomain
    {
        public LFVDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public LFVDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<ApiResponseViewModel> SaveLiftFileRecords(LiftFileValidateRequest liftFileRequest, string token)
        {
            ApiResponseViewModel response = new ApiResponseViewModel();
            try
            {
                if (liftFileRequest != null && liftFileRequest.Data != null && liftFileRequest.Data != null && liftFileRequest.Data.Any())
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        LiftFileViewModel liftFileViewModel = liftFileRequest.ToViewModel(apiUserContext, response);
                        var liftFile = liftFileViewModel.ToEntity();
                        var lfvParameter = GetLfvRequiredParameters(liftFile.CompanyId);
                        if (lfvParameter != null)
                        {
                            ValidateAPIParameters(liftFile, lfvParameter, response, liftFileRequest.Count, liftFileRequest.Liters);
                            if (liftFileRequest.Count != liftFileRequest.Data.Count)      
                                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = Resource.errHeaderCountMismatch });

                            if (!response.Messages.Any())
                            {
                                using (var transaction = Context.DataContext.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        Context.DataContext.LiftFileDetails.Add(liftFile);
                                        await Context.CommitAsync();

                                        transaction.Commit();
                                        response.Status = Status.Success;
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRS01,
                                            Message = $"{liftFile.LiftFileValidationRecords.Count} records received successfully into TFX"
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        LogManager.Logger.WriteException("LFVDomain", "SaveLiftFileRecords", ex.Message, ex);
                                    }
                                }
                            }
                        }
                        else
                            response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.msgLiftFileValidationNotEnabled });
                        }
                    else
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ01, Message = Resource.errMsgInvalidToken });
                    }
                else
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
                }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "SaveLiftFileRecords", ex.Message, ex);
                if (!response.Messages.Any())
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF02, Message = "Database Error - Please try again later" });
            }

            return response;
        }


        private void ValidateAPIParameters(LiftFileDetail liftFile, LfvParameterViewModel lfvParameter, ApiResponseViewModel response, int count, decimal litres)
        {
            foreach (var item in liftFile.LiftFileValidationRecords)
            {
                if (lfvParameter.IsBolReq && string.IsNullOrWhiteSpace(item.BOL))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.BOL)) });

                if (lfvParameter.IsCINReq && string.IsNullOrWhiteSpace(item.CIN))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.CIN)) });

                if (lfvParameter.IsCarrierNameReq && string.IsNullOrWhiteSpace(item.CarrierName))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.CarrierName)) });

                //if (lfvParameter.IsCorrectedQtyRes && item.CorrectedQty <= 0)
                //    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.CorrectedQty)) });

                //if (lfvParameter.IsGrossReq && item.Gross <= 0)
                //    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.Gross)) });

                if (lfvParameter.IsLoadDateReq && string.IsNullOrWhiteSpace(item.LoadDate))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.LoadDate)) });

                if (lfvParameter.IsTerminalCodeReq && string.IsNullOrWhiteSpace(item.TerminalCode))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, "Terminal_Code") });

                if (lfvParameter.IsTermItemCodeReq && string.IsNullOrWhiteSpace(item.TermItemCode))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMessageRequiredParameter, nameof(item.TermItemCode)) });
                }

            if (litres != liftFile.LiftFileValidationRecords.Sum(t => t.CorrectedQty))
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = Resource.errMessageQuantityMismatch });
            }


        private List<LiftFileResponseDetails> SetLiftFileRecordStatus(List<BolDataForLFV> allbolRecords, LiftFileValidationRecord item,
                                                            LfvParameterViewModel lfvParameter, List<string> poFromAPIList,
                                                            List<BadgeApiResponseViewModel> badgeListFromAPI, List<int> cleanLiftFileRecordsIdList,
                                                            ref List<int> clearRecordsInvFtlIdList, ref bool needToSaveRecords, List<string> carrierNameList,
                                                            List<string> quebecBillingBadgeList, bool isOnlyPartialRecords,
                                                            List<DropdownDisplayItem> termItemCodes)
        {
            List<LiftFileResponseDetails> liftFileResponse = new List<LiftFileResponseDetails>();
            try
            {
                //get valid item codes
                var validItemcodeProductTypes = termItemCodes.Where(t => t.Name.ToLower().Equals(item.TermItemCode.ToLower()))
                                            .Select(t => t.Id).ToList();

                FallbackForProductTypes(validItemcodeProductTypes);

                var bolRecords = allbolRecords.ToList();
                var bolRecordsForReason = allbolRecords.ToList();

                if (isOnlyPartialRecords)
                {
                    if (item.Status != LFVRecordStatus.PartialMatch)
                        return liftFileResponse;
                }

                if (item.Status == LFVRecordStatus.IgnoreMatch && !item.IsRecordPushedToExternalApi)
                {
                    if (string.IsNullOrWhiteSpace(item.Reason))
                        item.Reason = Resource.lblIgnoreReasonForcedIgnore;
                    needToSaveRecords = true;
                    cleanLiftFileRecordsIdList.Add(item.Id);
                    liftFileResponse.Add(item.ToResponseViewModel(item.Reason.Equals(Resource.lblIgnoreReasonSelfHaul)));
                    return liftFileResponse;
                }

                if (item.Status == LFVRecordStatus.ForcedIgnore && !item.IsRecordPushedToExternalApi)
                {
                    // in case of forced ignore, we will already have reason text as while marking record = forced ignore, user needs to provide reason
                    //if (string.IsNullOrWhiteSpace(item.Reason))
                    //    item.Reason = Resource.lblIgnoreReasonForcedIgnore;
                    needToSaveRecords = true;
                    cleanLiftFileRecordsIdList.Add(item.Id);
                    liftFileResponse.Add(item.ToResponseViewModel(item.Reason.Equals(Resource.lblIgnoreReasonSelfHaul)));
                    return liftFileResponse;
                }

                if (item.Status == LFVRecordStatus.PendingMatch)
                {
                    item.Status = LFVRecordStatus.NoMatch;
                    needToSaveRecords = true;
                }

                item.Reason = null; //to avoid duplicate mismatch text

                //PoNumber match - if found then status=ignorematch and need to send record in response
                if (lfvParameter.IsIgnoreSelfHauling)
                {
                    if (poFromAPIList != null && poFromAPIList.Any() && item.POFromAPI != null)
                    {
                        var lfile = poFromAPIList.Contains(item.POFromAPI.Trim().ToLower());
                        if (lfile)
                        {
                            if (item.Status != LFVRecordStatus.IgnoreMatch)
                            {
                                item.Status = LFVRecordStatus.IgnoreMatch;
                                item.Reason = Resource.lblIgnoreReasonSelfHaul;
                                needToSaveRecords = true;
                                cleanLiftFileRecordsIdList.Add(item.Id);
                                liftFileResponse.Add(item.ToResponseViewModel(true));
                                return liftFileResponse;
                            }
                        }
                    }
                }

                //carrierNameList manually uploaded - if found then status=ignorematch and need to send record in response
                if (lfvParameter.IsIgnoreNonRegisteredCarriers && !string.IsNullOrWhiteSpace(item.CarrierID))
                {
                    if (carrierNameList != null && carrierNameList.Any())
                    {
                        var carrierName = carrierNameList.Contains(item.CarrierID.ToLower());
                        if (carrierName)
                        {
                            if (item.Status != LFVRecordStatus.IgnoreMatch)
                            {
                                item.Status = LFVRecordStatus.IgnoreMatch;
                                item.Reason = Resource.lblIgnoreReasonNonRegisteredCarrier;
                                needToSaveRecords = true;
                                cleanLiftFileRecordsIdList.Add(item.Id);
                                liftFileResponse.Add(item.ToResponseViewModel());
                                return liftFileResponse;
                            }
                        }
                    }
                }

                //badgeList FromAPI - if found then status=ignorematch and need to send record in response
                if (badgeListFromAPI != null && badgeListFromAPI.Any() && item.CIN != null)
                {
                    var apiBadgeWithCustNumber = badgeListFromAPI.Any(t => t.Badge_Number.ToLower() == item.CIN.ToLower()
                                                 && t.Customer_Number > 0);
                    if (apiBadgeWithCustNumber)
                    {
                        if (item.Status != LFVRecordStatus.IgnoreMatch)
                        {
                            item.Status = LFVRecordStatus.IgnoreMatch;
                            item.Reason = Resource.lblIgnoreReasonBadgeFromAPI;
                            needToSaveRecords = true;
                            cleanLiftFileRecordsIdList.Add(item.Id);
                            liftFileResponse.Add(item.ToResponseViewModel());
                            return liftFileResponse;
                        }
                    }

                    var apiBadgeWithBDStatus = badgeListFromAPI.Where(t => t.Badge_Number.ToLower().Equals(item.CIN.ToLower())
                                                    && !string.IsNullOrWhiteSpace(t.Status)
                                                    && t.Customer_Number == 0 && (t.Status.ToLower() == "b" || t.Status.ToLower() == "q" || t.Status.ToLower() == "d")).FirstOrDefault();
                    if (apiBadgeWithBDStatus != null)
                    {
                        if (item.Status != LFVRecordStatus.IgnoreMatch)
                        {
                            item.Status = LFVRecordStatus.IgnoreMatch;
                            if (apiBadgeWithBDStatus.Status.ToLower() == "b")
                                item.Reason = Resource.lblIgnoreReasonBStatusRecord;
                            if (apiBadgeWithBDStatus.Status.ToLower() == "q")
                                item.Reason = Resource.lblIgnoreReasonQStatusRecord;
                            if (apiBadgeWithBDStatus.Status.ToLower() == "d")
                                item.Reason = Resource.lblIgnoreReasonDStatusRecord;
                            needToSaveRecords = true;
                            cleanLiftFileRecordsIdList.Add(item.Id);
                            liftFileResponse.Add(item.ToResponseViewModel());
                            return liftFileResponse;
                        }
                    }
                }
                //Quebec billing badge 
                if (lfvParameter.IsIgnoreQuebecBillingBadges && !string.IsNullOrWhiteSpace(item.CIN))
                {
                    if (quebecBillingBadgeList != null && quebecBillingBadgeList.Any())
                    {
                        var CIN = quebecBillingBadgeList.Contains(item.CIN.ToLower());
                        if (CIN)
                        {
                            if (item.Status != LFVRecordStatus.IgnoreMatch)
                            {
                                item.Status = LFVRecordStatus.IgnoreMatch;
                                item.Reason = Resource.lblIgnoreReasonQuebecBadgeRecord;
                                needToSaveRecords = true;
                                cleanLiftFileRecordsIdList.Add(item.Id);
                                liftFileResponse.Add(item.ToResponseViewModel());
                                return liftFileResponse;
                            }
                        }
                    }
                }

                //mandetory parameters
                if (lfvParameter.IsTerminalCodeReq)
                {
                    if (bolRecords.Any() && bolRecords.Count(t => t.TerminalName.ToLower().Equals(item.TerminalCode.ToLower())
                                                    || (!string.IsNullOrWhiteSpace(t.AssignedTerminalId) && t.AssignedTerminalId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    || (!string.IsNullOrWhiteSpace(t.BulkPlantAssignedId) && t.BulkPlantAssignedId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    ) == 0)
                    {
                        bolRecordsForReason = bolRecords.ToList();
                    }

                    bolRecords = bolRecords.Where(t => t.TerminalName.ToLower().Equals(item.TerminalCode.ToLower())
                                                    || (!string.IsNullOrWhiteSpace(t.AssignedTerminalId) && t.AssignedTerminalId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    || (!string.IsNullOrWhiteSpace(t.BulkPlantAssignedId) && t.BulkPlantAssignedId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    ).ToList();

                    AddMismatchReasonForRecord(bolRecordsForReason.Any(t => t.TerminalName.ToLower().Equals(item.TerminalCode.ToLower())
                                                    || (!string.IsNullOrWhiteSpace(t.AssignedTerminalId) && t.AssignedTerminalId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    || (!string.IsNullOrWhiteSpace(t.BulkPlantAssignedId) && t.BulkPlantAssignedId.ToLower().Equals(item.TerminalCode.ToLower()))
                                                    ), item, nameof(item.TerminalCode), ref needToSaveRecords);
                }

                if (lfvParameter.IsBolReq)
                {
                    if (lfvParameter.IsNeedToTruncateLeadingZeros)
                        ValidateBolWithTrim(lfvParameter, ref needToSaveRecords, isOnlyPartialRecords, item, ref bolRecords, bolRecordsForReason);
                    else
                        ValidateBolWithoutTrim(lfvParameter, ref needToSaveRecords, isOnlyPartialRecords, item, ref bolRecords, bolRecordsForReason);

                    if (bolRecords.Any())
                    {
                        var InvFtlDetailId = GetInvoiceFtlDetailIdForProduct(bolRecords, item, validItemcodeProductTypes, isOnlyPartialRecords);
                        needToSaveRecords = SetItemStatusToPartialMatch(needToSaveRecords, item, InvFtlDetailId);
                    }
                    else
                    {
                        //termin + bol not found
                        if (lfvParameter.IsTerminalCodeReq)
                            item.Reason = Resource.errLFVBolAndTerminalDataMismatch;
                        else
                           item.Reason = string.Format(Resource.errLFVDataMismatch, "BOL");

                        item.Status = LFVRecordStatus.NoMatch;
                        needToSaveRecords = true;
                        return liftFileResponse;
                    }
                }

                if (lfvParameter.IsTermItemCodeReq)
                {
                    if (bolRecords.Any() && bolRecords.Count(t => validItemcodeProductTypes.Contains(t.ProductTypeId) || (t.ChangedProductTypeId.HasValue && validItemcodeProductTypes.Contains(t.ChangedProductTypeId.Value))) == 0)
                    {
                        bolRecordsForReason = bolRecords.ToList();
                    }

                    bolRecords = bolRecords.Where(t => validItemcodeProductTypes.Contains(t.ProductTypeId) || (t.ChangedProductTypeId.HasValue && validItemcodeProductTypes.Contains(t.ChangedProductTypeId.Value))).ToList();
                    AddMismatchReasonForRecord(bolRecordsForReason.Any(t => validItemcodeProductTypes.Contains(t.ProductTypeId) || (t.ChangedProductTypeId.HasValue && validItemcodeProductTypes.Contains(t.ChangedProductTypeId.Value))), item, "Terminal Item Code", ref needToSaveRecords);
                }

                //optiona parameters
                if (lfvParameter.IsCINReq)
                {
                    if (bolRecords.Any() && bolRecords.Count(t => !string.IsNullOrWhiteSpace(t.BolNumber) && t.BolNumber.ToLower().Equals(item.CIN.ToLower())) == 0)
                    {
                        bolRecordsForReason = bolRecords.ToList();
                    }

                    bolRecords = bolRecords.Where(t => !string.IsNullOrWhiteSpace(t.BolNumber) && t.BolNumber.ToLower().Equals(item.CIN.ToLower())).ToList();
                    AddMismatchReasonForRecord(bolRecordsForReason.Any(t => !string.IsNullOrWhiteSpace(t.BolNumber) && t.BolNumber.ToLower().Equals(item.CIN.ToLower())), item, nameof(item.CIN), ref needToSaveRecords);
                }

                if (lfvParameter.IsCarrierNameReq)
                {
                    if (bolRecords.Any() && bolRecords.Count(t => !string.IsNullOrWhiteSpace(t.Carrier) && t.Carrier.ToLower().Equals(item.CarrierName.ToLower())) == 0)
                    {
                        bolRecordsForReason = bolRecords.ToList();
                    }

                    bolRecords = bolRecords.Where(t => !string.IsNullOrWhiteSpace(t.Carrier) && t.Carrier.ToLower().Equals(item.CarrierName.ToLower())).ToList();
                    AddMismatchReasonForRecord(bolRecordsForReason.Any(t => !string.IsNullOrWhiteSpace(t.Carrier) && t.Carrier.ToLower().Equals(item.CarrierName.ToLower())), item, nameof(item.CarrierName), ref needToSaveRecords);
                }

                if (lfvParameter.IsLoadDateReq)
                {
                    if (bolRecords.Any() && bolRecords.Count(t => t.CreatedDate.ToString("MMddyyyy") == item.LoadDate) == 0)
                    {
                        //Fall back for lift date, where drop happens during midnight
                        if (bolRecords.Any(t => t.CreatedDate.Hour == 0))
                        {
                            bolRecords.Where(t => t.CreatedDate.Hour == 0).ToList()
                                    .ForEach(t => t.CreatedDate = t.CreatedDate.Date.AddDays(-1));
                            if (bolRecords.Count(t => t.CreatedDate.ToString("MMddyyyy") == item.LoadDate) == 0)
                                bolRecordsForReason = bolRecords.ToList();
                        }
                        else
                            bolRecordsForReason = bolRecords.ToList();
                    }

                    bolRecords = bolRecords.Where(t => t.CreatedDate.ToString("MMddyyyy") == item.LoadDate).ToList();
                    AddMismatchReasonForRecord(bolRecordsForReason.Any(t => t.CreatedDate.ToString("MMddyyyy") == item.LoadDate), item, nameof(item.LoadDate), ref needToSaveRecords);
                }

                if (lfvParameter.IsCorrectedQtyRes)
                {
                    var totalFromBol = bolRecords.Any() ? bolRecords.Sum(t => t.NetQuantity) : bolRecordsForReason.Sum(t => t.NetQuantity);
                    //no need to set bolRecordsForReason as its last parameter for validation

                    if (totalFromBol != item.CorrectedQty)
                    {
                        if (totalFromBol > item.CorrectedQty)
                        {
                            //check indivisual bol records net quantity
                            var qtyMatchrecord = bolRecords.Where(t => t.NetQuantity == item.CorrectedQty).FirstOrDefault();
                            if (qtyMatchrecord != null)
                            {
                                item.InvoiceFtlDetailId = qtyMatchrecord.InvoiceFtlDetailId;
                                totalFromBol = qtyMatchrecord.NetQuantity;
                                bolRecords = bolRecords.Where(t => t.NetQuantity == item.CorrectedQty).ToList();
                            }
                            else
                                bolRecords.Clear();
                        }
                        else
                            bolRecords.Clear();
                    }
                    AddMismatchReasonForRecord(totalFromBol == item.CorrectedQty, item, "Corrected Qty", ref needToSaveRecords);
                }

                if (lfvParameter.IsGrossReq)
                {
                    var totalFromBol = bolRecords.Any() ? bolRecords.Sum(t => t.GrossQuantity) : bolRecordsForReason.Sum(t => t.GrossQuantity);
                    //no need to set bolRecordsForReason as its last parameter for validation

                    if (totalFromBol != item.Gross)
                    {
                        if (totalFromBol > item.Gross)
                        {
                            //check indivisual bol records net quantity
                            var qtyMatchrecord = bolRecords.Where(t => t.GrossQuantity == item.Gross).FirstOrDefault();
                            if (qtyMatchrecord != null)
                            {
                                item.InvoiceFtlDetailId = qtyMatchrecord.InvoiceFtlDetailId;
                                totalFromBol = qtyMatchrecord.GrossQuantity;
                                bolRecords = bolRecords.Where(t => t.GrossQuantity == item.Gross).ToList();
                            }
                            else
                                bolRecords.Clear();
                        }
                        else
                            bolRecords.Clear();
                    }
                    AddMismatchReasonForRecord(totalFromBol == item.Gross, item, "Gross Qty", ref needToSaveRecords);
                }

                if (bolRecords.Any())
                {
                    if (!checkIfBolRecordsHasInvoiceWithWaitingAction(bolRecords)) //check if any ddt is in waiting
                    {
                        //add BU logic
                        if (bolRecords.Any())
                        {
                            item.Status = LFVRecordStatus.Clean;
                            item.Reason = null;
                            needToSaveRecords = true;
                            clearRecordsInvFtlIdList.AddRange(
                                        bolRecords.Where(t => validItemcodeProductTypes.Contains(t.ProductTypeId)
                                                || (t.ChangedProductTypeId.HasValue && validItemcodeProductTypes.Contains(t.ChangedProductTypeId.Value)))
                                            .Select(t => t.InvoiceFtlDetailId).Distinct().ToList());

                            //remove validated invoiceftldetail ids from bolrecords
                            foreach (var validatedBolRecord in bolRecords)
                            {
                                allbolRecords.Remove(validatedBolRecord);
                            }
                            cleanLiftFileRecordsIdList.Add(item.Id);
                            var buCounts = bolRecords.Where(t => t.AccouningCompanyId != null).Select(t => t.AccouningCompanyId).Distinct().ToList();
                            if (buCounts.Count > 1)
                            {
                                var originalCorrectedQty = item.CorrectedQty;
                                int buCountRecords = 1;
                                List<LiftFileResponseDetails> buLevelResponse = new List<LiftFileResponseDetails>();
                                foreach (var buAcount in buCounts)
                                {
                                    var totaldropForBu = bolRecords.Where(t => t.AccouningCompanyId == buAcount).Sum(t => t.NetQuantity);

                                    var newResponseRecord = item.ToResponseViewModel();

                                    if (buCountRecords == buCounts.Count)
                                        newResponseRecord.CorrectedQty = originalCorrectedQty - buLevelResponse.Sum(t => t.CorrectedQty);
                                    else
                                        newResponseRecord.CorrectedQty = totaldropForBu ?? 0;

                                    newResponseRecord.PO = buAcount;
                                    buLevelResponse.Add(newResponseRecord);
                                    buCountRecords++;
                                }
                                liftFileResponse.AddRange(buLevelResponse);
                            }
                            else
                            {
                                var cleanRecordRespViewModel = item.ToResponseViewModel();

                                if (buCounts.Any())
                                    cleanRecordRespViewModel.PO = buCounts.FirstOrDefault();

                                liftFileResponse.Add(cleanRecordRespViewModel);
                            }
                        }
                    }
                    else
                    {
                        if (bolRecords.Any())
                        {
                            item.Status = LFVRecordStatus.ActiveExceptions;
                            needToSaveRecords = true;
                            //add reason code specific to waiting action
                            SetReasonCodeAsPerWaitingStatus(item, bolRecords);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "SetLiftFileRecordStatus", ex.Message, ex);
            }
            return liftFileResponse;
        }

        private static void SetReasonCodeAsPerWaitingStatus(LiftFileValidationRecord item, List<BolDataForLFV> bolRecords)
        {
            var firstRecord = bolRecords.FirstOrDefault().WaitingFor;
            item.Reason = EnumHelperMethods.GetDisplayName((WaitingAction)firstRecord);
        }

        private void ValidateBolWithoutTrim(LfvParameterViewModel lfvParameter, ref bool needToSaveRecords, bool isOnlyPartialRecords, LiftFileValidationRecord item, ref List<BolDataForLFV> bolRecords, List<BolDataForLFV> bolRecordsForReason)
        {
            bolRecords = bolRecords.Where(t => t.BolNumber != null && t.BolNumber.ToLower().Equals(item.BOL.ToLower())).ToList();
            AddMismatchReasonForRecord(bolRecordsForReason.Any(t => t.BolNumber != null && t.BolNumber.ToLower().Equals(item.BOL.ToLower())), item, nameof(item.BOL), ref needToSaveRecords);
        }

        private void ValidateBolWithTrim(LfvParameterViewModel lfvParameter, ref bool needToSaveRecords, bool isOnlyPartialRecords, LiftFileValidationRecord item, ref List<BolDataForLFV> bolRecords, List<BolDataForLFV> bolRecordsForReason)
        {
            bolRecords = bolRecords.Where(t => t.BolNumber != null && t.BolNumber.ToLower().TrimStart('0').Equals(item.BOL.ToLower().TrimStart('0'))).ToList();
            AddMismatchReasonForRecord(bolRecordsForReason.Any(t => t.BolNumber != null && t.BolNumber.ToLower().TrimStart('0').Equals(item.BOL.ToLower().TrimStart('0'))), item, nameof(item.BOL), ref needToSaveRecords);
        }

        private static void FallbackForProductTypes(List<int> validItemcodes)
        {
            if (validItemcodes.Contains((int)ProductTypes.RedDyeDiesel2))
                validItemcodes.Add((int)ProductTypes.RedDyeDiesel);
            else if (validItemcodes.Contains((int)ProductTypes.RedDyeDiesel))
                validItemcodes.Add((int)ProductTypes.RedDyeDiesel2);

            if (validItemcodes.Contains((int)ProductTypes.ClearDiesel2))
                validItemcodes.Add((int)ProductTypes.ClearDiesel);
            else if (validItemcodes.Contains((int)ProductTypes.ClearDiesel))
                validItemcodes.Add((int)ProductTypes.ClearDiesel2);

            //new conventional - oxy types
            if (validItemcodes.Contains((int)ProductTypes.RegularGas))
            {
                validItemcodes.Add((int)ProductTypes.RegularOxygenated);
                validItemcodes.Add((int)ProductTypes.RegularConventional);
            }

            if (validItemcodes.Contains((int)ProductTypes.PremiumGas))
            {
                validItemcodes.Add((int)ProductTypes.PremiumOxygenated);
                validItemcodes.Add((int)ProductTypes.PremiumConventional);
            }

            if (validItemcodes.Contains((int)ProductTypes.MidgradeGas))
                validItemcodes.Add((int)ProductTypes.MidGradeOxygenated);

        }

        private int? GetInvoiceFtlDetailIdForProduct(List<BolDataForLFV> bolRecords, LiftFileValidationRecord item, List<int> validItemcodes, bool isOnlyPartialRecords)
        {
            var invFtlDetail = bolRecords.Where(t => validItemcodes.Contains(t.ProductTypeId) 
                                            || (t.ChangedProductTypeId.HasValue && validItemcodes.Contains(t.ChangedProductTypeId.Value)))
                                .ToList();
            if (invFtlDetail != null && invFtlDetail.Any() && invFtlDetail.Count == 1)
                return invFtlDetail.FirstOrDefault().InvoiceFtlDetailId;
            else
            {
                if (isOnlyPartialRecords && bolRecords.Any() && bolRecords.Count == 1)
                    return bolRecords.FirstOrDefault().InvoiceFtlDetailId;
                return null;
            }
        }

        private void AddMismatchReasonForRecord(bool isReasonToAdd, LiftFileValidationRecord item, string parameterName, ref bool needToSaveRecords)
        {
            if (!isReasonToAdd)
            {
                item.Reason = $"{item.Reason}{string.Format(Resource.errLFVDataMismatch, parameterName)} ";
                needToSaveRecords = true;
            }
        }

        private static bool SetItemStatusToPartialMatch(bool needToSaveRecords, LiftFileValidationRecord item, int? InvFtlDetailId)
        {
            item.Status = LFVRecordStatus.PartialMatch;
            item.InvoiceFtlDetailId = InvFtlDetailId;
            needToSaveRecords = true;

            return needToSaveRecords;
        }

        private bool checkIfBolRecordsHasInvoiceWithWaitingAction(List<BolDataForLFV> bolRecords)
        {
            if (bolRecords.Any())
            {
                if (bolRecords.Any(t => (t.WaitingFor == (int)WaitingAction.ExceptionApproval || t.WaitingFor == (int)WaitingAction.BolDetails || t.WaitingFor == (int)WaitingAction.Images
                                 || t.WaitingFor == (int)WaitingAction.InventoryVerification || t.WaitingFor == (int)WaitingAction.PrePostDipData)))
                    return true;
                }
            return false;
        }

        private LfvParameterViewModel GetLfvRequiredParameters(int liftFileParamsForCompany)
        {
            var requiredLfvParams = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == liftFileParamsForCompany
                                                && t.IsActive && t.IsLiftFileValidationEnabled)
                                        .Select(t => t.LiftFileValidationParameters)
                                        .FirstOrDefault();

            if (requiredLfvParams != null)
            {
                var inputParameters = requiredLfvParams.Where(t => t.ParameterType == LFVParameterType.Input && t.IsActive).FirstOrDefault();
                if (inputParameters != null)
                {
                    return inputParameters.ToViewModel();
                }
            }

            return null;
        }

        public async Task<bool> ProcessFailedPostApiCalls()
        {
            var response = false;
            try
            {
                //get all failed records
                string LFVResponseToCompanyId = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLFVResponseToCompanyId)
                                                .Select(t => t.Value).FirstOrDefault();

                //var failedPushObjects = Context.DataContext.ApiLogs.Where(t => !t.IsSuccessStatusCode && t.CompanyId.ToString() == 1.ToString())
                //                        .Select(t => new { t.Id, t.CompanyId, t.Request }).ToList();
                var spDomain = new StoredProcedureDomain();
               var failedPushObjects = await spDomain.GetApiLogsForCompany(Convert.ToInt32(LFVResponseToCompanyId), null, null, 0, null,false);
                int failedObjectCount = 0;
                foreach (var item in failedPushObjects)
                {
                    var connectionData = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLiftValidationStatusApiSettings
                     || t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId
                     || t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass
                     || t.Key == ApplicationConstants.KeyAppSettingParklandxApiKey
                     || t.Key == ApplicationConstants.KeyAppSettingParklandFailedPostAPICallRetry
                     || t.Key == ApplicationConstants.PostAPITimeoutInSeconds).Select(t => new { Key = t.Key, Value = t.Value }).ToList();

                    if (connectionData != null && connectionData.Count >= 5)
                    {
                        string LiftValidationStatusUrl = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingLiftValidationStatusApiSettings).FirstOrDefault().Value;
                        string ParklandBadgeMgmtApiUserId = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId).FirstOrDefault().Value;
                        string ParklandBadgeMgmtApiPass = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass).FirstOrDefault().Value;
                        string ParklandxApiKey = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandxApiKey).FirstOrDefault().Value;
                        string retryFlag = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandFailedPostAPICallRetry).FirstOrDefault().Value;
                        string PostAPITimeoutInSeconds = connectionData.Where(w => w.Key == ApplicationConstants.PostAPITimeoutInSeconds).FirstOrDefault().Value;

                        if (retryFlag.ToLower().Equals("true"))
                        {
                            if (!string.IsNullOrEmpty(LiftValidationStatusUrl))
                            {
                                using (var client = new HttpClient())
                                {
                                    try
                                    {
                                        int.TryParse(PostAPITimeoutInSeconds, out int timeoutInSec);
                                        if (timeoutInSec == 0) timeoutInSec = 300;       
                                        client.Timeout = TimeSpan.FromSeconds(timeoutInSec);

                                        if (string.IsNullOrWhiteSpace(ParklandxApiKey))
                                        {
                                            var byteArray = Encoding.ASCII.GetBytes(ParklandBadgeMgmtApiUserId + ":" + ParklandBadgeMgmtApiPass);
                                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                                        }
                                        else
                                        {
                                            client.DefaultRequestHeaders.Add("x-api-key", ParklandxApiKey);
                                        }

                                        var stringContent = new StringContent(item.Request, UnicodeEncoding.UTF8, "application/json");
                                        HttpResponseMessage apiResponse = await client.PutAsync(LiftValidationStatusUrl, stringContent);
                                        var apiResult = await apiResponse.Content.ReadAsStringAsync();

                                        if (apiResponse.IsSuccessStatusCode)
                                        {
                                            var query = $"UPDATE ApiLogs SET IsSuccessStatusCode = 1, Response = '{apiResult}' , Url = '{LiftValidationStatusUrl}' ,ExternalRefID = '{DateTimeOffset.Now}' WHERE ID = {item.Id}";
                                            Context.DataContext.Database.ExecuteSqlCommand(query);
                                            Context.Commit();
                                            response = apiResponse.IsSuccessStatusCode;
                                        }
                                        else
                                        {
                                            var query = $"UPDATE ApiLogs SET Response = '{apiResult}' , Url = '{LiftValidationStatusUrl}' ,ExternalRefID = '{DateTimeOffset.Now}' WHERE ID = {item.Id}";
                                            Context.DataContext.Database.ExecuteSqlCommand(query);
                                            Context.Commit();
                                            response = apiResponse.IsSuccessStatusCode;
                                            failedObjectCount++;
                                        }

                                        if (failedObjectCount > 2)
                                            return false;
                                        }
                                    catch (Exception ex)
                                    {
                                        LogManager.Logger.WriteException("LFVDomain", "ProcessFailedPostApiCalls", ex.Message, ex);
                                        failedObjectCount++;
                                        if (failedObjectCount > 2)
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "ProcessFailedPostApiCalls", ex.Message, ex);
            }

            return response;
        }

        public async Task<bool> ProcessMatchPendingLfvRecords()
        {
            var response = false;
            using (var tracer = new Tracer("LFVDomain", "ProcessMatchPendingLfvRecords"))
            {
                try
                {
                    
                    //get pending match records
                    var distinctCompanyIds = Context.DataContext.LiftFileValidationRecords
                                        .Where(t => t.IsActive && (t.Status == LFVRecordStatus.PendingMatch || t.Status == LFVRecordStatus.NoMatch || t.Status == LFVRecordStatus.ActiveExceptions || !t.IsRecordPushedToExternalApi))
                                        .Select(t => t.LiftFileDetails.CompanyId)
                                        .Distinct()
                                        .ToList();

                    if (distinctCompanyIds.Any())
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        foreach (var companyId in distinctCompanyIds)
                        {
                            // mark for any duplicate record in lift file
                            var duplicateRecordsCount = await spDomain.UpdateLiftFileForDuplicateRecords(companyId);

                            await ProcessLiftFileRecordsForCompany(companyId);
                        }
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVDomain", "ProcessMatchPendingLfvRecords", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<LiftFileStatusResponseViewModel> ProcessLiftFileRecordsForCompany(int companyId)
        {
            LiftFileStatusResponseViewModel liftfileresponseDtl = new LiftFileStatusResponseViewModel();
            List<int> cleanLiftFileRecordIdList = new List<int>();

            var lfvParams = GetLfvRequiredParameters(companyId);

            if (lfvParams != null)
            {
                var spDomain = new StoredProcedureDomain(this);
                var bolRecords = await spDomain.GetBolDataForLFV(companyId);

                var poFromAPIList = Context.DataContext.LiftFilePONumbers.Where(po => po.AddedByCompanyId == companyId && po.IsActive && po.SelfHaulingPoNumber != null).Select(t => t.SelfHaulingPoNumber.ToLower()).ToList();

                //get badges from LiftFileBadgeManagementDetails
                var badgeListFromAPI = Context.DataContext.LiftFileBadgeManagementDetails
                                        .Where(t => t.IsActive && t.BadgeNumber != null)
                                        .Select(t => new BadgeApiResponseViewModel
                                        {
                                            Customer_Number = t.CustomerNumber,
                                            Badge_Number = t.BadgeNumber.Trim(),
                                            Status = t.StatusCode
                                        }).ToList();

                var carrierNameList = Context.DataContext.LiftFileCarrierNames.Where(carrierName => carrierName.AddedByCompanyId == companyId && carrierName.IsActive && carrierName.CarrierName != null).Select(t => t.CarrierName.ToLower()).ToList();
                var quebecBillingBadges = Context.DataContext.LiftFileQuebecBillingBadges.Where(badge => badge.AddedByCompanyId == companyId && badge.IsActive && badge.QuebecBillingBadge != null).Select(t => t.QuebecBillingBadge.ToLower()).ToList();
                var allowedDate = DateTimeOffset.Now.AddDays(-lfvParams.DaysToContinueMatchProcess);
                //NEED TO get only active records
                var termItemCodes = Context.DataContext.TerminalItemCodeMappings.Where(t => t.CompanyId == companyId
                                                && t.IsActive && t.ExpiryDate >= DateTimeOffset.Now && t.EffectiveDate <= DateTimeOffset.Now)
                                            .Select(t => new DropdownDisplayItem()
                                            {
                                                Name =  t.ItemCode,
                                                Id = t.MstTerminalItemDescription.ProductTypeId
                                            }).ToList();
                var bufferTime = DateTimeOffset.Now.AddMinutes(-5);
                try
                {

                    List<int> cleanInvoiceFtlDetailRecords = new List<int>();
                    bool needToSaveRecords = false;
                    List<LiftFileHoldRecords> liftFileHoldRecords = new List<LiftFileHoldRecords>();
                    List<LiftFileResponseDetails> groupedLiftfileresponse = new List<LiftFileResponseDetails>();
                    List<LiftFileHoldRecords> groupedliftFileHoldRecords = new List<LiftFileHoldRecords>();

                    //take clean or ignore records which are not yet sent back
                    var cleanOrIgnoredRecords = Context.DataContext.LiftFileValidationRecords
                                                .Where(t => t.IsActive && t.LiftFileDetails.AddedDate <= bufferTime
                                                && t.LiftFileDetails.CompanyId == companyId
                                                && !t.IsRecordPushedToExternalApi
                                                && (t.Status == LFVRecordStatus.Clean || t.Status == LFVRecordStatus.IgnoreMatch || t.Status == LFVRecordStatus.ForcedIgnore))
                                                .ToList();

                    //this code is to push existing clean/ignored/forcedignore records, which havent got pushed due to some issues
                    if (cleanOrIgnoredRecords.Any())
                    {
                        var pendingbolTerminalGroup = cleanOrIgnoredRecords.GroupBy(t => new
                        {
                            Bol = lfvParams.IsNeedToTruncateLeadingZeros ? t.BOL.TrimStart('0') : t.BOL,
                            t.TerminalCode
                        }).ToList();

                        foreach (var cleanGroup in pendingbolTerminalGroup)
                        {
                            var groupedRecords = cleanGroup.ToList();
                            var firstRecord = groupedRecords.FirstOrDefault();
                            if (firstRecord != null)
                            {
                                var isOtherRecordExists = Context.DataContext.LiftFileValidationRecords
                                                        .Where(t => t.IsActive && t.LiftFileDetails.CompanyId == companyId && t.BOL == firstRecord.BOL)
                                                        .Select(t => t.Status).ToList();

                                if (!isOtherRecordExists.Any(t => t == LFVRecordStatus.ActiveExceptions
                                            || t == LFVRecordStatus.NoMatch || t == LFVRecordStatus.PartialMatch || t == LFVRecordStatus.UnMatched))
                                {
                                    foreach (var cleanorIgnoreRecords in groupedRecords)
                                    {
                                        liftfileresponseDtl.data.Add(cleanorIgnoreRecords.ToResponseViewModel());
                                        cleanLiftFileRecordIdList.Add(cleanorIgnoreRecords.Id);
                                    }
                                }
                            }
                        }
                        if (liftfileresponseDtl.data.Any())
                        {
                            var pendingPushSuccess = await PushCleanRecordsToExternalApi(companyId, liftfileresponseDtl, cleanLiftFileRecordIdList);
                            cleanLiftFileRecordIdList.Clear();
                            liftfileresponseDtl.data.Clear();
                            liftfileresponseDtl.count = 0;
                            liftfileresponseDtl.liters = 0;
                        }
                    }
                    //once existing data got pushed, start new records for processing

                    var allowedRecords = GetValidLiftRecordsForProcessing(companyId, allowedDate, bufferTime, false);

                    var bolTerminalGroup = allowedRecords.GroupBy(t => new
                                            {
                                                Bol = lfvParams.IsNeedToTruncateLeadingZeros ? t.BOL.TrimStart('0') : t.BOL,
                                                t.TerminalCode
                                            }).ToList();

                    foreach (var groupItem in bolTerminalGroup)
                    {
                        var groupedRecords = groupItem.ToList();
                        groupedLiftfileresponse.Clear();
                        groupedliftFileHoldRecords.Clear();

                        foreach (var pendingRecords in groupedRecords)
                        {
                            var processedRecords = SetLiftFileRecordStatus(bolRecords, pendingRecords, lfvParams, poFromAPIList,
                                                    badgeListFromAPI, cleanLiftFileRecordIdList, ref cleanInvoiceFtlDetailRecords, ref needToSaveRecords,
                                                    carrierNameList, quebecBillingBadges, false, termItemCodes);

                            if (groupedRecords.Count > 1)
                            {
                                if (pendingRecords.Status == LFVRecordStatus.Clean || pendingRecords.Status == LFVRecordStatus.IgnoreMatch || pendingRecords.Status == LFVRecordStatus.ForcedIgnore)
                                {
                                    groupedLiftfileresponse.AddRange(processedRecords);
                                    if (!processedRecords.Any(t => t.RecordStatus == LFVRecordStatus.Clean.ToString() && t.InvoiceFtlDetailId == 0))
                                    {
                                        var json = JsonConvert.SerializeObject(processedRecords, Formatting.Indented);
                                        if (!json.Equals("[]"))
                                        {
                                            //add bol-terminal key in json obj
                                            json = $"{pendingRecords.BOL}{pendingRecords.TerminalCode}^{json}";
                                            groupedliftFileHoldRecords.Add(LiftFileMapper.ToLiftFileHoldRecordEntity(pendingRecords.Id, json));
                                        }
                                    }
                                }
                                //removed from clean record id list to ensure its not only updating DB as pushed records =1
                                cleanLiftFileRecordIdList.Remove(pendingRecords.Id);
                            }
                            else
                                liftfileresponseDtl.data.AddRange(processedRecords);
                        }

                        if (groupedRecords.Count > 1 && groupedLiftfileresponse.Any())
                        {
                            //hold clean records if all not clean or Ignore match
                            if (groupedLiftfileresponse.Count >= groupedRecords.Count)
                            {
                                if (groupedLiftfileresponse.Select(t => t.LiftFileRecordId).Distinct().ToList().Count                                
                                >= groupedRecords.Count
                                    )
                                {
                                    liftfileresponseDtl.data.AddRange(groupedLiftfileresponse.ToList());
                                    //adding record id in case few records are from hold records
                                    cleanLiftFileRecordIdList.AddRange(groupedRecords.Select(t => t.Id).ToList());
                                    groupedliftFileHoldRecords.Clear();
                                }
                                else // debug is just to verify BU account check is working fine or not
                                    LogManager.Logger.WriteDebug("LFVDomain", "ProcessLiftFileRecordsForCompany", "Grouped record count is less than Grouped response LiftFileRecordId count");
                            }
                        }

                        if (groupedliftFileHoldRecords.Any())
                        {
                            liftFileHoldRecords.AddRange(groupedliftFileHoldRecords);
                            //remove record id from cleanrecordlist
                            liftFileHoldRecords.ForEach(t => cleanLiftFileRecordIdList.Remove(t.LiftFileRecordId));
                        }
                    }

                    if (cleanInvoiceFtlDetailRecords.Any() || needToSaveRecords || liftFileHoldRecords.Any())
                    {
                        if (liftFileHoldRecords.Any())
                        {
                            var newHoldRecrods = liftFileHoldRecords.Select(t => t.LiftFileRecordId).Distinct().ToList();
                            var existingHodlRecords = Context.DataContext.LiftFileHoldRecords
                                                        .Where(t => t.IsActive && newHoldRecrods.Contains(t.LiftFileRecordId))
                                                        .Select(t => t.LiftFileRecordId).ToList();

                            liftFileHoldRecords.RemoveAll(t => existingHodlRecords.Contains(t.LiftFileRecordId));

                            Context.DataContext.LiftFileHoldRecords.AddRange(liftFileHoldRecords);
                        }

                        if (cleanInvoiceFtlDetailRecords.Any())
                            Context.DataContext.Database.ExecuteSqlCommand($"UPDATE InvoiceFtlDetails SET IsLiftFileValidated = 1 WHERE ID IN ({string.Join<int>(",", cleanInvoiceFtlDetailRecords)})");
                        
                        Context.Commit();
                    }

                    //setting NO MATCH FOR OLD RECORDS
                    var oldRecords = Context.DataContext.LiftFileValidationRecords
                                .Where(t => t.LiftFileDetails.CompanyId == companyId
                                        && (t.Status == LFVRecordStatus.NoMatch || t.Status == LFVRecordStatus.ActiveExceptions || t.Status == LFVRecordStatus.PartialMatch)
                                        && t.LiftFileDetails.AddedDate <= allowedDate
                                        && t.AddedDate <= allowedDate)
                                .Select(t => t.LiftFileDetails.Id)
                                .Distinct()
                                .ToList();

                    if (oldRecords != null && oldRecords.Any())
                    {
                        var query = $"UPDATE LiftFileValidationRecords SET Status = {(int)LFVRecordStatus.UnMatched}, StatusChangedDate = DATEADD(day, {lfvParams.DaysToContinueMatchProcess}, AddedDate) " +
                                                    $"WHERE LiftFileId IN ({string.Join<int>(",", oldRecords)}) AND Status IN ({(int)LFVRecordStatus.NoMatch},{(int)LFVRecordStatus.ActiveExceptions},{(int)LFVRecordStatus.PartialMatch})";
                        Context.DataContext.Database.ExecuteSqlCommand(query);
                        Context.Commit();
                    }

                    if (liftfileresponseDtl.data.Any())
                    {
                        //get hold records for clean records
                        var bolTerminalGroups = liftfileresponseDtl.data.Select(t => t.BOL + t.Terminal_Code).ToList();
                        var distinctLiftFileRecordIds = liftfileresponseDtl.data.Select(t => t.LiftFileRecordId).ToList();
                        if (bolTerminalGroups.Any())
                        {
                            var recordList = Context.DataContext.LiftFileHoldRecords.Where(t => t.IsActive
                                                        && bolTerminalGroups.Any(b => t.JsonString.Contains(b))
                                                        && !distinctLiftFileRecordIds.Any(l => l == t.LiftFileRecordId))
                                                .Select(t => new { t.LiftFileRecordId, t.JsonString }).Distinct().ToList();

                            foreach (var item in recordList)
                            {
                                var splitedJsonStr = item.JsonString.Split('^').ToList();
                                if (splitedJsonStr.Any())
                                {
                                    if (splitedJsonStr.Count >= 2)
                                        liftfileresponseDtl.data.AddRange(JsonConvert.DeserializeObject<List<LiftFileResponseDetails>>(splitedJsonStr[1]));
                                    else // this is fall back for existing records
                                        liftfileresponseDtl.data.AddRange(JsonConvert.DeserializeObject<List<LiftFileResponseDetails>>(item.JsonString));

                                    cleanLiftFileRecordIdList.Add(item.LiftFileRecordId);
                                }
                            }
                        }
                    }

                    var isPushSuccess = await PushCleanRecordsToExternalApi(companyId, liftfileresponseDtl, cleanLiftFileRecordIdList);

                    if (allowedRecords.Any()) // to avoid duplicate call to bolrecords
                    {
                        //partial match processing 
                        bolRecords = await spDomain.GetBolDataForLFV(companyId);
                        List<LiftFileValidationRecord> partialRecords = GetValidLiftRecordsForProcessing(companyId, allowedDate, bufferTime, true);
                        if (partialRecords.Any())
                        {
                            foreach (var pendingRecords in partialRecords)
                            {
                                cleanInvoiceFtlDetailRecords.Clear();
                                needToSaveRecords = false;
                                var processedRecords = SetLiftFileRecordStatus(bolRecords, pendingRecords, lfvParams, poFromAPIList, badgeListFromAPI, cleanLiftFileRecordIdList, ref cleanInvoiceFtlDetailRecords, ref needToSaveRecords, carrierNameList, quebecBillingBadges, true, termItemCodes);
                            }

                            if (needToSaveRecords)
                                Context.Commit();
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVDomain", "ProcessLiftFileRecordsForCompany", ex.Message, ex);
                    throw;
                }
            }
            else
            {
                LogManager.Logger.WriteException("LFVDomain", "ProcessLiftFileRecordsForCompany", $"Lift File validation parameters not found for companyid={companyId}", new Exception());
            }
            return liftfileresponseDtl;
        }

        private List<LiftFileValidationRecord> GetValidLiftRecordsForProcessing(int companyId, DateTimeOffset allowedDate, DateTimeOffset bufferTime, bool isOnlyPartialMatch)
        {
            if (isOnlyPartialMatch)
                return Context.DataContext.LiftFileValidationRecords
                                                .Where(t => t.IsActive && t.LiftFileDetails.AddedDate <= bufferTime
                                                && t.LiftFileDetails.CompanyId == companyId
                                                && t.AddedDate >= allowedDate
                                                && t.Status == LFVRecordStatus.PartialMatch)
                                                .ToList();
            else
                return Context.DataContext.LiftFileValidationRecords
                              .Where(t => t.IsActive && t.LiftFileDetails.AddedDate <= bufferTime
                              && t.LiftFileDetails.CompanyId == companyId
                              && t.AddedDate >= allowedDate &&
                                  (t.Status == LFVRecordStatus.PendingMatch
                                                  || t.Status == LFVRecordStatus.NoMatch
                                                  || t.Status == LFVRecordStatus.ActiveExceptions
                                                  || t.Status == LFVRecordStatus.PartialMatch
                                                  || (t.Status == LFVRecordStatus.IgnoreMatch && !t.IsRecordPushedToExternalApi)
                                                  || (t.Status == LFVRecordStatus.ForcedIgnore && !t.IsRecordPushedToExternalApi)))
                              .ToList();
        }

        private async Task<bool> PushCleanRecordsToExternalApi(int companyId, LiftFileStatusResponseViewModel liftfileresponseDtl, List<int> cleanRecordIdList)
        {
            var response = true;
            string LFVResponseToCompanyId = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLFVResponseToCompanyId).Select(t => t.Value).FirstOrDefault();

            if (liftfileresponseDtl != null && liftfileresponseDtl.data != null && liftfileresponseDtl.data.Any())
            {
                liftfileresponseDtl.count = liftfileresponseDtl.data.Count();
                liftfileresponseDtl.liters = liftfileresponseDtl.data.Sum(t => t.CorrectedQty);
            }
            // post data to api for only parkland company
            if (liftfileresponseDtl != null && liftfileresponseDtl.data.Any() && !String.IsNullOrEmpty(LFVResponseToCompanyId) && LFVResponseToCompanyId == companyId.ToString())
            {
                response = false;
                response = await PostLiftValidateStatus(liftfileresponseDtl, companyId);
                UpdateLiftFileRecordsStatus(cleanRecordIdList);
            }
            //else if (String.IsNullOrEmpty(LFVResponseToCompanyId))
            //    UpdateLiftFileRecordsStatus(cleanRecordIdList);

            return response;
        }

        private void UpdateLiftFileRecordsStatus(List<int> cleanRecordIdList)
        {
            if (cleanRecordIdList.Any())
            {
                var query = $"UPDATE LiftFileValidationRecords SET IsRecordPushedToExternalApi = 1, StatusChangedDate = '{DateTimeOffset.Now}' WHERE Id IN ({string.Join<int>(",", cleanRecordIdList)})";
                Context.DataContext.Database.ExecuteSqlCommand(query);

                var updateHoldRecs = $"UPDATE LiftFileHoldRecords SET IsActive = 0, UpdatedDate = '{DateTimeOffset.Now}' WHERE LiftFileRecordId IN ({string.Join<int>(",", cleanRecordIdList)})";
                Context.DataContext.Database.ExecuteSqlCommand(updateHoldRecs);

                Context.Commit();
            }
        }

        public  List<LFValidationGridViewModel>GetLFValidationGrid(int companyId, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, bool isCarrierPerFormanceDashboard=false, string carrierIds = "",bool isMatchingWindow=false)
        {
            var response = new List<LFValidationGridViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                if (isMatchingWindow)
                {
                    var lfvParameter = GetLfvRequiredParameters(companyId);
                    var daysToContinueMatchProcess = lfvParameter != null && lfvParameter.DaysToContinueMatchProcess > 0 ? lfvParameter.DaysToContinueMatchProcess : ApplicationConstants.DefaultNoMatchRecordDays;

                     startDate = DateTimeOffset.Now.AddDays(-daysToContinueMatchProcess);
                     endDate = DateTimeOffset.Now.AddDays(-1);
                }
                var spResponse = new List<usp_LFValidationViewModel>();
                spResponse =spDomain.GetLFValidationGridData(companyId, startDate, endDate,carrierIds);
                if (spResponse != null && spResponse.Any() && !isCarrierPerFormanceDashboard)
                {
                    var grpResult = spResponse.GroupBy(t => t.CallId).ToList();
                    foreach (var item in grpResult)
                    {
                        response.Add(new LFValidationGridViewModel()
                        {
                            CallId = item.Key,
                            TotalRecordCount = item.Count(),
                            MatchedRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.Clean).Count(),
                            ActiveExceptionRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.ActiveExceptions).Count(),
                            IgnoredMatchRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.IgnoreMatch).Count(),
                            NoMatchRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.NoMatch).Count(),
                            UnmatchedRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.UnMatched).Count(),
                            RecordDate = item.Where(t => t.CallId == item.Key).FirstOrDefault().RecordDate,
                            PendingMatchCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.PendingMatch).Count(),
                            DuplicateRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.Duplicate).Count(),
                            PartialMatchRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.PartialMatch).Count(),
                            CarrierName= item.Where(t => t.CallId == item.Key).FirstOrDefault().CarrierName,
                            ForcedIgnoredMatchRecordCount = item.Where(t => t.CallId == item.Key && t.RecordStatus == (int)LFVRecordStatus.ForcedIgnore).Count()
                        });
                    }
                }
               else if (spResponse != null && spResponse.Any() && isCarrierPerFormanceDashboard)
                {
                    var grpResult = spResponse.GroupBy(t => t.CarrierID).ToList().OrderByDescending(o=>o.Count()).Take(50);
                    foreach (var item in grpResult)
                    {
                        response.Add(new LFValidationGridViewModel()
                        {
                            CarrierID = item.Key,
                            TotalRecordCount = item.Count(),
                            MatchedRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.Clean).Count(),
                            ActiveExceptionRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.ActiveExceptions).Count(),
                            IgnoredMatchRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.IgnoreMatch).Count(),
                            NoMatchRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.NoMatch).Count(),
                            UnmatchedRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.UnMatched).Count(),
                            RecordDate = item.Where(t => t.CarrierID == item.Key).FirstOrDefault().RecordDate,
                            PendingMatchCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.PendingMatch).Count(),
                            DuplicateRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.Duplicate).Count(),
                            PartialMatchRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.PartialMatch).Count(),
                            CallId = item.Where(t => t.CarrierID == item.Key).FirstOrDefault().CallId,
                            CarrierName = item.Where(t => t.CarrierID == item.Key).FirstOrDefault().CarrierName,
                            ForcedIgnoredMatchRecordCount = item.Where(t => t.CarrierID == item.Key && t.RecordStatus == (int)LFVRecordStatus.ForcedIgnore).Count()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFValidationGrid", ex.Message, ex);
            }
            return response;
        }

        public List<LFRecordsGridViewModel> GetLFRecordGrid(int companyId, bool isAdminUser, int recordStatus, int lfCallId, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, bool isMatchingWindow = false, string carrierIds = "")
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                if (isMatchingWindow)
                {
                    var lfvParameter = GetLfvRequiredParameters(companyId);
                    var daysToContinueMatchProcess = lfvParameter != null && lfvParameter.DaysToContinueMatchProcess > 0 ? lfvParameter.DaysToContinueMatchProcess : ApplicationConstants.DefaultNoMatchRecordDays;

                    startDate = DateTimeOffset.Now.AddDays(-daysToContinueMatchProcess);
                    endDate = DateTimeOffset.Now.AddDays(-1);
                }
                var spDomain = new StoredProcedureDomain();
                response = spDomain.GetLFRecordsGridData(companyId, recordStatus, lfCallId, startDate, endDate, carrierIds);
                if (response != null && response.Any() && isAdminUser)
                {
                    var lfvdomain = new LFVDomain();
                    var lfrValidationParametes = lfvdomain.GetLfvRequiredParameters(companyId);
                    if (lfrValidationParametes != null)
                    {
                        response.ForEach(t => t.IsAdminUser = isAdminUser);
                        response.ForEach(t => t.LfvValidationParameters = lfrValidationParametes);
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFRecordGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<PONumbersGridViewModel>> GetPoNumbersGrid(int CompanyId)
        {
            var response = new List<PONumbersGridViewModel>();
            try
            {
                var poNumbers = await Context.DataContext.LiftFilePONumbers.Where(t => t.AddedByCompanyId == CompanyId && t.IsActive).OrderByDescending(t=>t.Id).ToListAsync();
                if (poNumbers!=null && poNumbers.Any())
                {
                    poNumbers.ForEach(t => response.Add(new PONumbersGridViewModel { Id= t.Id,PoNumber = t.SelfHaulingPoNumber}));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFRecordGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteSelfHaulPONumber(int recordId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (recordId > 0 )
                {
                    var existingPOrecord = await Context.DataContext.LiftFilePONumbers.Where(t => t.Id == recordId && t.IsActive).SingleOrDefaultAsync();
                    if (existingPOrecord != null)
                    {
                        existingPOrecord.IsActive = false;
                        Context.DataContext.Entry(existingPOrecord).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMsgPONumberSuccess;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgPONumberUpdateFailed;
                LogManager.Logger.WriteException("LFVDomain", "DeleteSelfHaulPONumber", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> PostLiftValidateStatus(LiftFileStatusResponseViewModel liftFileresponse, int companyId)
        {
            try
            {
                var connectionData = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLiftValidationStatusApiSettings
                 || t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId
                 || t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass
                 || t.Key == ApplicationConstants.KeyAppSettingParklandxApiKey
                 || t.Key == ApplicationConstants.PostAPITimeoutInSeconds).Select(t => new { Key = t.Key, Value = t.Value }).ToList();
                if (connectionData != null && connectionData.Count >= 4)
                {
                    string LiftValidationStatusUrl = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingLiftValidationStatusApiSettings).FirstOrDefault().Value;
                    string ParklandBadgeMgmtApiUserId = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId).FirstOrDefault().Value;
                    string ParklandBadgeMgmtApiPass = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass).FirstOrDefault().Value;
                    string ParklandxApiKey = connectionData.Where(w => w.Key == ApplicationConstants.KeyAppSettingParklandxApiKey).FirstOrDefault().Value;
                    string PostAPITimeoutInSeconds = connectionData.Where(w => w.Key == ApplicationConstants.PostAPITimeoutInSeconds).FirstOrDefault().Value;

                    if (!string.IsNullOrEmpty(LiftValidationStatusUrl))
                    {
                        using (var client = new HttpClient())
                        {
                            string json = string.Empty;
                            try
                            {
                                int.TryParse(PostAPITimeoutInSeconds, out int timeoutInSec);
                                if (timeoutInSec == 0) timeoutInSec = 300;
                                client.Timeout = TimeSpan.FromSeconds(timeoutInSec);
                                if (string.IsNullOrWhiteSpace(ParklandxApiKey))
                                {
                                    var byteArray = Encoding.ASCII.GetBytes(ParklandBadgeMgmtApiUserId + ":" + ParklandBadgeMgmtApiPass);
                                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                                }
                                else
                                {
                                    client.DefaultRequestHeaders.Add("x-api-key", ParklandxApiKey);
                                }

                                json = JsonConvert.SerializeObject(liftFileresponse, Formatting.Indented);
                                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                                HttpResponseMessage apiResponse = await client.PutAsync(LiftValidationStatusUrl, stringContent);
                                var apiResult = await apiResponse.Content.ReadAsStringAsync();

                                AddApiLogEntry(companyId, LiftValidationStatusUrl, json, apiResult, apiResponse.IsSuccessStatusCode);

                                //if (apiResponse.IsSuccessStatusCode)
                                {
                                    return true;
                                }
                            }
                            catch (Exception)
                            {
                                AddApiLogEntry(companyId, LiftValidationStatusUrl, json, "PostLiftValidateStatus failed", false);
                                throw;
                            }
                        }
                    }
                }
                return false;
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "PostLiftValidateStatus", ex.Message, ex);
            }
            return false;
        }

        private static void AddApiLogEntry(int companyId, string LiftValidationStatusUrl, string json, string apiResult, bool isSucess)
        {
            var apiLog = new ApiLog();
            apiLog.Request = json;
            apiLog.Response = apiResult.ToString();
            apiLog.CreatedBy = 1;
            apiLog.Url = LiftValidationStatusUrl;
            apiLog.CompanyId = companyId;
            apiLog.ExternalRefID = DateTimeOffset.Now.ToString();
            apiLog.CreatedDate = DateTimeOffset.Now;
            apiLog.Message = Resource.PostLiftValidateStatusCalled;
            apiLog.IsSuccessStatusCode = isSucess;

            var logDomain = new ExceptionLogDomain();
            logDomain.AddApiLogs(apiLog);
        }

        //public async Task<StatusViewModel> AddUnmatchedRecordForReProcessing(int LfRecordId)
        //{
        //    var response = new StatusViewModel(Status.Failed);
        //    using (var transaction = Context.DataContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var existingliftFileRecord = await Context.DataContext.LiftFileValidationRecords.Where(t => t.Id == LfRecordId && t.IsActive).FirstOrDefaultAsync();
        //            if (existingliftFileRecord != null)
        //            {

        //                existingliftFileRecord.IsActive = false;
        //                existingliftFileRecord.Status = LFVRecordStatus.ReprocessSubmitted;
        //                existingliftFileRecord.StatusChangedDate = DateTimeOffset.Now;
        //                Context.DataContext.Entry(existingliftFileRecord).State = EntityState.Modified;
        //                await Context.CommitAsync();

        //                LiftFileValidationRecord entity = new LiftFileValidationRecord();
        //                entity = existingliftFileRecord.ToEntityForStatusUpdate();
        //                Context.DataContext.LiftFileValidationRecords.Add(entity);
        //                await Context.CommitAsync();

        //                transaction.Commit();
        //                response.StatusCode = Status.Success;
        //                response.StatusMessage = string.Format(Resource.msgSuccessReprocessRecord, LfRecordId, existingliftFileRecord.LiftFileId);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            response.StatusCode = Status.Failed;
        //            response.StatusMessage = Resource.errMessageReprocessRecord;
        //            LogManager.Logger.WriteException("LFVDomain", "AddUnmatchedRecordForReProcessing", ex.Message, ex);
        //        }
        //        return response;
        //    }
        //}


        public async Task<LiftFileStatusResponseViewModel> GetStatus(string token)
        {
            var authDomain = new AuthenticationDomain(this);
            var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
            if (apiUserContext != null)
            {
                try
                {
                    // mark for any duplicate record in lift file
                    var spDomain = new StoredProcedureDomain(this);
                    var duplicateRecordsCount = await spDomain.UpdateLiftFileForDuplicateRecords(apiUserContext.CompanyId);

                    return await ProcessLiftFileRecordsForCompany(apiUserContext.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVDomain", "GetStatus", ex.Message + $"companyid = {apiUserContext.CompanyId}", ex);
                }
            }
            return null;
        }

        public async Task<bool> RetryFailedPostCalls(string token)
        {
            var authDomain = new AuthenticationDomain(this);
            var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
            if (apiUserContext != null)
            {
                try
                {
                    var response =  await ProcessFailedPostApiCalls();
                    return response;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVDomain", "PostFailedCalls", ex.Message + $"companyid = {apiUserContext.CompanyId}", ex);
                }
            }
            return false;
        }

        public async Task<StatusViewModel> ProcessUploadedWholeSaleBadgeFile(HttpPostedFileBase wholesalebadeCsvFile, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    // read file
                    var badgeList = new List<WholesaleBadgeCsvViewModel>();
                    using (var stream = new MemoryStream())
                    {
                        wholesalebadeCsvFile.InputStream.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.Configuration.RegisterClassMap<WholesaleBadgeCsvViewModelMap>();
                                try
                                {
                                    badgeList = csv.GetRecords<WholesaleBadgeCsvViewModel>().ToList();
                                }
                                catch (Exception)
                                {
                                    badgeList = new List<WholesaleBadgeCsvViewModel>();
                                }
                            }
                        }


                        // save po details
                        int processCount = 0;
                        int totalCount = badgeList.Count;
                        badgeList = badgeList.Where(t => t.WholeSaleBadge != "" && t.WholeSaleBadge != null).ToList();
                        if (badgeList.Any())
                        {
                            var entityList = new List<LiftFileWholesaleBadges>();
                            var addedByCompanyId = userContext.CompanyId;
                            var isActive = true;
                            var addedDate = DateTimeOffset.Now;

                            foreach (var badgeModel in badgeList)
                            {
                                // check if record already exists
                                var existingBadge = await Context.DataContext.LiftFileWholesaleBadges.Where(t => t.WholesaleBadge.ToLower() == badgeModel.WholeSaleBadge.ToLower().Trim() && t.AddedByCompanyId == addedByCompanyId)
                                                                                               .OrderByDescending(t => t.AddedDate)
                                                                                               .FirstOrDefaultAsync();
                                if (existingBadge != null)
                                {
                                    if (!existingBadge.IsActive)
                                    {
                                        existingBadge.IsActive = true;
                                        Context.DataContext.Entry(existingBadge).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        processCount++;
                                    }
                                }

                                else
                                {
                                    var entity = new LiftFileWholesaleBadges ();
                                    entity.WholesaleBadge = badgeModel.WholeSaleBadge.Trim();
                                    entity.AddedByCompanyId = addedByCompanyId;
                                    entity.IsActive = isActive;
                                    entity.AddedDate = addedDate;
                                    entityList.Add(entity);

                                    processCount++;
                                }
                            }
                            if (entityList.Any())
                            {
                                Context.DataContext.LiftFileWholesaleBadges.AddRange(entityList);
                                await Context.CommitAsync();
                            }
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageLiftDataBadgeNumbersCSVUpload, processCount, totalCount);

                            // upload to blob
                            var azureBlob = new AzureBlobStorage();
                            var filePath = await azureBlob.SaveBlobAsync(wholesalebadeCsvFile.InputStream, GenerateFileName(userContext.Id, "WholesaleBadge"), BlobContainerType.LiftDataProcessingWholeSaleBadgeFile.ToString().ToLower());
                        }

                        if (processCount == 0)
                        {
                            response.StatusMessage = Resource.msgerrMessageWholesSaleBadgeFileEmpty;
                            response.StatusCode = Status.Failed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("LFVDomain", "ProcessUploadedWholeSaleBadgeFile", "Lift data processing BadgeNumber CSV upload failed", ex);
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageProcessLiftDataBadgeNumberCSVFile;
                }
            }
            return response;
        }



        private string GenerateFileName(int userId, string fileType)
        {
            if (fileType == "WholesaleBadge")
            {
                return string.Concat(values: Constants.WholeSaleBadgeFile + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
            }
            if (fileType == "CarrierNamesFilter")
            {
                return string.Concat(values: Constants.LiftFileCarrierNamesFile + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
            }
            if (fileType == "QuebecBadgeFilter")
            {
                return string.Concat(values: Constants.LiftFileQuebecBadgeFile + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
            }
            return string.Concat(values:userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }


        public async Task<List<BadgeNumberGridViewModel>> GetBadgeNumbersGrid(int CompanyId)
        {
            var response = new List<BadgeNumberGridViewModel>();
            try
            {
                var badgeNumbers = await Context.DataContext.LiftFileWholesaleBadges.Where(t => t.AddedByCompanyId == CompanyId && t.IsActive).OrderByDescending(t => t.AddedDate).ToListAsync();
                if (badgeNumbers != null && badgeNumbers.Any())
                {
                    badgeNumbers.ForEach(t => response.Add(new BadgeNumberGridViewModel { Id = t.Id, BadgeNumber = t.WholesaleBadge }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFRecordGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteBadgeNumber(int recordId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (recordId > 0)
                {
                    var existingBadge = await Context.DataContext.LiftFileWholesaleBadges.Where(t => t.Id == recordId && t.IsActive).SingleOrDefaultAsync();
                    if (existingBadge != null)
                    {
                        existingBadge.IsActive = false;
                        Context.DataContext.Entry(existingBadge).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMsgCarrierNameSuccess;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgBadgeNumberUpdateFailed;
                LogManager.Logger.WriteException("LFVDomain", "DeleteBadgeNumber", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> GetBadgelistFromParkLandBadgeManagermentApi()
        {

            var result = false;
            try
            {

                var filters = new List<string>
                {
                    ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiURL,
                    ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId,
                    ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass,
                    ApplicationConstants.KeyAppSettingParklandxApiKey
                };

                var apiConfigs = Context.DataContext.MstAppSettings.Where(t => filters.Contains(t.Key)).ToList();

                string ParklandBadgeMgmtApiURL = apiConfigs.Where(t => t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiURL).Select(t => t.Value).FirstOrDefault();
                string ParklandBadgeMgmtApiUserId = apiConfigs.Where(t => t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiUserId).Select(t => t.Value).FirstOrDefault();
                string ParklandBadgeMgmtApiPass = apiConfigs.Where(t => t.Key == ApplicationConstants.KeyAppSettingParklandBadgeMgmtApiPass).Select(t => t.Value).FirstOrDefault();
                string ParklandxApiKey = apiConfigs.Where(t => t.Key == ApplicationConstants.KeyAppSettingParklandxApiKey).Select(t => t.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(ParklandBadgeMgmtApiURL))
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(500);
                        if (string.IsNullOrEmpty(ParklandxApiKey))
                        {
                            var byteArray = Encoding.ASCII.GetBytes(ParklandBadgeMgmtApiUserId + ":" + ParklandBadgeMgmtApiPass);
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Add("x-api-key", ParklandxApiKey);
                        }

                        HttpResponseMessage apiResponse = await client.GetAsync(ParklandBadgeMgmtApiURL);
                        var resultString = await apiResponse.Content.ReadAsStringAsync();
                        var apiLog = new ApiLog();
                        apiLog.Request = ParklandBadgeMgmtApiURL.ToString();
                        apiLog.Response = resultString.ToString();
                        apiLog.CreatedBy = 1;
                        apiLog.Url = ParklandBadgeMgmtApiURL;
                        apiLog.CompanyId = 1;
                        apiLog.ExternalRefID = DateTimeOffset.Now.ToString();
                        apiLog.CreatedDate = DateTimeOffset.Now;
                        apiLog.Message = "GetBadgelistFromParkLandBadgeManagermentApi called";

                        var logDomain = new ExceptionLogDomain();
                        logDomain.AddApiLogs(apiLog);

                        if (apiResponse.IsSuccessStatusCode)
                        {
                            result = true;
                            var responseString = await apiResponse.Content.ReadAsStringAsync();
                            var response = JsonConvert.DeserializeObject<List<BadgeApiResponseViewModel>>(responseString);
                            if (response != null && response.Any())
                            {
                                var isSaved = await SaveBadgeNumbersFromAPIResponse(response);
                            }
                        }
                        else
                            LogManager.Logger.WriteException("LFVDomain", "GetBadgelistFromParkLandBadgeManagermentApi", "Get Badge API failed to get data", new Exception());
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetBadgelistFromParkLandBadgeManagermentApi", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> SaveBadgeNumbersFromAPIResponse(List<BadgeApiResponseViewModel> apiResponse)
        {
            var isSaved = false;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (apiResponse != null && apiResponse.Any())
                    {
                        var entityList = new List<LiftFileBadgeManagementDetail>();
                        foreach (var item in apiResponse)
                        {
                            if (!string.IsNullOrWhiteSpace(item.Badge_Number))
                            {
                                var entity = new LiftFileBadgeManagementDetail();
                                entity = item.ToEntity();
                                entityList.Add(entity);
                            }

                        }

                        if (entityList != null && entityList.Any())
                        {
                            var query = $"UPDATE LiftFileBadgeManagementDetails SET IsActive = {0} WHERE IsActive = {1}";

                            Context.DataContext.Database.ExecuteSqlCommand(query);
                            await Context.CommitAsync();

                            Context.DataContext.LiftFileBadgeManagementDetails.AddRange(entityList);
                            await Context.CommitAsync();
                            isSaved = true;
                            transaction.Commit();
                        }
                    }

                }
                catch (Exception ex)
                {
                    isSaved = false;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("LFVDomain", "SaveBadgeNumbersFromAPIResponse", ex.Message, ex);
                }
            }
            return isSaved;
        }

        public async Task<LFBolEditViewModel> GetLFBolEditViewModel(LFRecordsGridViewModel model,UserContext userContext)
        {
            var response = new LFBolEditViewModel();
            try
            {
                response.LiftRecord.LiftFileRecordId = model.LiftFileRecordId;
                response.LiftRecord.LoadDate = model.LoadDate;
                response.LiftRecord.ProductType = model.ProductType;
                response.LiftRecord.RecordDate = model.RecordDate;
                response.LiftRecord.Status = model.Status;
                response.LiftRecord.statusChangeDate = model.statusChangeDate;
                response.LiftRecord.TerminalItemCode = model.TerminalItemCode;
                response.LiftRecord.TerminalName = model.TerminalName;
                response.LiftRecord.correctedQuantity = model.correctedQuantity;
                response.LiftRecord.bol = model.bol;
                if (model.InvFtlDetailId.HasValue && model.InvFtlDetailId.Value > 0)
                {
                    var bolDetails = await Context.DataContext.InvoiceFtlDetails.
                                     Where(t => t.Id == model.InvFtlDetailId.Value
                                     && t.IsActive).Select(t =>
                                     new
                                     {
                                         t.Id,
                                         t.FuelTypeId,
                                         t.BadgeNumber,
                                         t.BolNumber,
                                         t.GrossQuantity,
                                         t.NetQuantity,
                                         t.LiftDate,
                                         t.TerminalId,
                                         t.MstExternalTerminal.Name,
                                         t.changedFuelTypeId,
                                         t.Notes,
                                         OrderId = t.InvoiceXBolDetails.Any() ? t.InvoiceXBolDetails.Where(t1 => t1.BolDetailId == t.Id).FirstOrDefault().Invoice.OrderId : null,
                                         t.LiftTicketNumber,
                                         t.PickupLocation,
                                         t.SiteName
                                     }).FirstOrDefaultAsync();
                    if (bolDetails != null)
                    {
                        response.InvoiceFtlDetailId = bolDetails.Id;// Id of InvFTLDetail row selected
                        response.TerminalId = bolDetails.TerminalId.HasValue ? bolDetails.TerminalId.Value : 0;
                        response.FuelTypeId = (bolDetails.changedFuelTypeId.HasValue && bolDetails.changedFuelTypeId.Value >0) ? bolDetails.changedFuelTypeId.Value : bolDetails.FuelTypeId;
                        response.PricingSourceId = Context.DataContext.MstProducts.Where(t => t.Id == response.FuelTypeId).Select(t => t.PricingSourceId).FirstOrDefault();
                        response.BadgeNumber = bolDetails.BadgeNumber;
                        if (bolDetails.PickupLocation == PickupLocationType.BulkPlant)
                        {
                            response.BolNumber = bolDetails.LiftTicketNumber;
                            response.DisplayTerminalName = bolDetails.SiteName;
                            response.IsBulkPlantLift = true;
                        }
                        else if (bolDetails.PickupLocation == PickupLocationType.Terminal)
                        {
                            response.BolNumber = bolDetails.BolNumber;
                            response.DisplayTerminalName = bolDetails.Name;
                            response.IsBulkPlantLift = false;
                        }
                        response.GrossQuantity = bolDetails.GrossQuantity.HasValue ? bolDetails.GrossQuantity.Value.GetPreciseValue(4) : 0;
                        response.NetQuantity = bolDetails.NetQuantity.HasValue ? bolDetails.NetQuantity.Value.GetPreciseValue(4) : 0;
                        response.LiftDate = bolDetails.LiftDate.Value.Date;                                               
                        response.Notes = bolDetails.Notes;
                        response.PickUpLocationType = bolDetails.PickupLocation;
                        // response.DisplayTerminalName = bolDetails.Name; //Terminal Name
                        response.OrderId = bolDetails.OrderId.HasValue ? bolDetails.OrderId.Value : 0;
                        response.InvoiceFtlDetailIdFromList = bolDetails.Id;
                        if (model.IsFromScratchReport)
                        {
                            response.DisplayLiftDate = bolDetails.LiftDate.Value.ToString(Resource.constFormatDate);
                            response.FuelTypeList = new MasterDomain(this).GetMstProductsDropDownListForLFVBol(response.PricingSourceId);
                            response.SelectedFuelType = response.FuelTypeList.Where(t => t.Id == response.FuelTypeId).FirstOrDefault();
                            response.TerminalList = await new ExternalPricingDomain(this).GetClosestTerminals(response.OrderId.Value,string.Empty);
                            if (response.TerminalList !=null && response.TerminalList.Any())
                            {
                                response.SelectedTerminal = response.TerminalList.Where(t => t.Id == response.TerminalId).FirstOrDefault();
                            }
                            else
                            {
                                response.SelectedTerminal = new DropdownDisplayItem { Id = response.TerminalId ,Name = response.DisplayTerminalName };
                            }
                        }
                    }
                    await SetInvoiceFtlDetailList(model, userContext, response);
                }
                 else
                   {
                    await SetInvoiceFtlDetailList(model, userContext, response);
                   }
                if(model.IsFromScratchReport && response.InvoiceFtlDetailsList!=null && response.InvoiceFtlDetailsList.Any())
                {
                    response.SelectedInvoiceFtlDetailId = response.InvoiceFtlDetailsList.Where(t => t.Id == response.InvoiceFtlDetailId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFBolEditViewModel", ex.Message, ex);
            }
            return response;
        }

        private async Task SetInvoiceFtlDetailList(LFRecordsGridViewModel model, UserContext userContext, LFBolEditViewModel response)
        {
            var spDomain = new StoredProcedureDomain(this);
            var bolRecords = await spDomain.GetBolDataForLFV(userContext.CompanyId, model.bol);
            var validInvoiceFtlDetailIds = bolRecords.Where(t => t.BolNumber.ToLower().Equals(model.bol.ToLower())
                                && (t.TerminalName.ToLower().Equals(model.TerminalName.ToLower())
                                                || (!string.IsNullOrWhiteSpace(t.AssignedTerminalId) && t.AssignedTerminalId.ToLower().Equals(model.TerminalName.ToLower()))
                                                || (!string.IsNullOrWhiteSpace(t.BulkPlantAssignedId) && t.BulkPlantAssignedId.ToLower().Equals(model.TerminalName.ToLower()))
                                                ))
                                .Select(t => new DropdownDisplayItem { Id = t.InvoiceFtlDetailId, Name = $"{t.BolNumber} - {t.FuelTypeName}" }).ToList();

            response.InvoiceFtlDetailsList.AddRange(validInvoiceFtlDetailIds);
        }

        public async Task<StatusViewModel> SaveLFBolEditDetails(LFBolEditViewModel model,UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.InvoiceFtlDetailId > 0 && model.LiftRecord.LiftFileRecordId > 0)
                    {
                        var bolDetails = await Context.DataContext.InvoiceFtlDetails.
                                        Where(t => t.Id == model.InvoiceFtlDetailId && t.IsActive)
                                        .FirstOrDefaultAsync();
                        if (bolDetails != null)
                        {
                            bolDetails.changedFuelTypeId = model.FuelTypeId;
                            //bolDetails.r = model.ReasonCode
                            if (model.IsBulkPlantLift)
                            {
                                bolDetails.LiftTicketNumber = model.BolNumber;
                            }
                            else if (!model.IsBulkPlantLift)
                            {
                                bolDetails.BolNumber = model.BolNumber;
                            }
                           
                            bolDetails.LiftDate = model.LiftDate;
                            bolDetails.GrossQuantity = model.GrossQuantity;
                            bolDetails.NetQuantity = model.NetQuantity;
                            bolDetails.BadgeNumber = model.BadgeNumber;

                            if (model.TerminalId > 0)
                                bolDetails.TerminalId = model.TerminalId;
                            else
                                bolDetails.TerminalId = null;

                            var newTerminalName = model.TerminalId > 0 ? Context.DataContext.MstExternalTerminals.
                                                                         Where(t => t.Id == model.TerminalId).
                                                                         Select(t => t.Name).FirstOrDefault() : bolDetails.TerminalName;
                            if (bolDetails.PickupLocation == PickupLocationType.Terminal)
                            {
                                bolDetails.SiteName = newTerminalName;
                                bolDetails.TerminalName = newTerminalName;
                            }
                            bolDetails.IsBOLEditedForLfv = true;
                            bolDetails.Notes = model.Notes;
                            Context.DataContext.Entry(bolDetails).State = EntityState.Modified;

                        }
                        var existingliftFileRecord = await Context.DataContext.LiftFileValidationRecords.
                                                     Where(t => t.Id == model.LiftRecord.LiftFileRecordId && t.IsActive)
                                                     .FirstOrDefaultAsync();
                        if (existingliftFileRecord != null)
                        {
                            existingliftFileRecord.IsActive = false;
                            existingliftFileRecord.Status = LFVRecordStatus.ReprocessSubmitted;
                            existingliftFileRecord.StatusChangedDate = DateTimeOffset.Now;
                            existingliftFileRecord.UpdatedBy = userContext.Id;
                            existingliftFileRecord.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(existingliftFileRecord).State = EntityState.Modified;

                            LiftFileValidationRecord entity = new LiftFileValidationRecord();
                            entity = existingliftFileRecord.ToEntityForStatusUpdate(userContext);
                            Context.DataContext.LiftFileValidationRecords.Add(entity);
                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            //response.StatusMessage = Resource.SuccessMsgSaveLFBolDetails;
                            response.StatusMessage = string.Format(Resource.SuccessMsgSaveLFBolDetails, model.BolNumber);

                        }
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgFailedToSaveLFBolDetails;
                    LogManager.Logger.WriteException("LFVDomain", "SaveLFBolEditDetails", ex.Message, ex);
                }
            }
            return response;


        }

        public async Task<StatusViewModel> ProcessUploadedCarrierListFile(HttpPostedFileBase carrierListCsvFile, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    // read file
                    var carrierNamesList = new List<CarrierNamesCsvViewModel>();
                    using (var stream = new MemoryStream())
                    {
                        carrierListCsvFile.InputStream.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.Configuration.RegisterClassMap<CarrierNamesCsvViewModelMap>();
                                try
                                {
                                    carrierNamesList = csv.GetRecords<CarrierNamesCsvViewModel>().ToList();
                                }
                                catch (Exception)
                                {
                                    carrierNamesList = new List<CarrierNamesCsvViewModel>();
                                }
                            }
                        }


                        // save po details
                        int processCount = 0;
                        int totalCount = carrierNamesList.Count;
                        carrierNamesList = carrierNamesList.Where(t => t.CarrierName != "" && t.CarrierName != null).ToList();
                        if (carrierNamesList.Any())
                        {
                            var entityList = new List<LiftFileCarrierNames>();
                            var addedByCompanyId = userContext.CompanyId;
                            var isActive = true;
                            var addedDate = DateTimeOffset.Now;

                            foreach (var carrier in carrierNamesList)
                            {
                                // check if record already exists
                                var existingName = await Context.DataContext.LiftFileCarrierNames.Where(t => t.CarrierName.ToLower() == carrier.CarrierName.ToLower().Trim() && t.AddedByCompanyId == addedByCompanyId)
                                                                                               .OrderByDescending(t => t.AddedDate)
                                                                                               .FirstOrDefaultAsync();
                                if (existingName != null)
                                {
                                    if (!existingName.IsActive)
                                    {
                                        existingName.IsActive = true;
                                        Context.DataContext.Entry(existingName).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        processCount++;
                                    }
                                }

                                else
                                {
                                    var entity = new LiftFileCarrierNames();
                                    entity.CarrierName = carrier.CarrierName.Trim();
                                    entity.AddedByCompanyId = addedByCompanyId;
                                    entity.IsActive = isActive;
                                    entity.AddedDate = addedDate;
                                    entityList.Add(entity);

                                    processCount++;
                                }
                            }
                            if (entityList.Any())
                            {
                                Context.DataContext.LiftFileCarrierNames.AddRange(entityList);
                                await Context.CommitAsync();
                            }
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageLiftDataCarrierNamesCSVUpload, processCount, totalCount);

                            // upload to blob
                            var azureBlob = new AzureBlobStorage();
                            var filePath = await azureBlob.SaveBlobAsync(carrierListCsvFile.InputStream, GenerateFileName(userContext.Id,"CarrierNamesFilter"), BlobContainerType.LiftDataProcessingCarrierNamesFile.ToString().ToLower());
                        }

                        if (processCount == 0)
                        {
                            response.StatusMessage = Resource.msgerrMessageCarrierNameFileEmpty;
                            response.StatusCode = Status.Failed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("LFVDomain", "ProcessUploadedCarrierListFile", "Lift data processing CarrierName CSV upload failed", ex);
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageProcessLiftDataCarrierNamesCSVFile;
                }
            }
            return response;
        }

        public async Task<List<LiftFileCarrierNamesGridViewModel>> GetLiftFileCarrierNamesGrid(int CompanyId)
        {
            var response = new List<LiftFileCarrierNamesGridViewModel>();
            try
            {
                var carrierNames = await Context.DataContext.LiftFileCarrierNames.Where(t => t.AddedByCompanyId == CompanyId && t.IsActive).OrderByDescending(t => t.AddedDate).ToListAsync();
                if (carrierNames != null && carrierNames.Any())
                {
                    carrierNames.ForEach(t => response.Add(new LiftFileCarrierNamesGridViewModel { Id = t.Id, CarrierName = t.CarrierName }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLiftFileCarrierNamesGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteLiftFileCarrierName(int recordId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (recordId > 0)
                {
                    var existingName = await Context.DataContext.LiftFileCarrierNames.Where(t => t.Id == recordId && t.IsActive).SingleOrDefaultAsync();
                    if (existingName != null)
                    {
                        existingName.IsActive = false;
                        Context.DataContext.Entry(existingName).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMsgCarrierNameSuccess;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgCarrierNameUpdateFailed;
                LogManager.Logger.WriteException("LFVDomain", "DeleteLiftFileCarrierName", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AddUnmatchedRecordForReProcessing(List<int> LfRecordIds, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (LfRecordIds != null && LfRecordIds.Any())
                    {
                        var newEntities = new List<LiftFileValidationRecord>();
                        var existingliftFileRecords = await Context.DataContext.LiftFileValidationRecords.Where(t => LfRecordIds.Contains(t.Id)).ToListAsync();
                        if (existingliftFileRecords != null && existingliftFileRecords.Any())
                        {
                            foreach (var existingliftFileRecord in existingliftFileRecords)
                            {
                                existingliftFileRecord.IsActive = false;
                                existingliftFileRecord.Status = LFVRecordStatus.ReprocessSubmitted;
                                existingliftFileRecord.StatusChangedDate = DateTimeOffset.Now;
                                Context.DataContext.Entry(existingliftFileRecord).State = EntityState.Modified;
                                newEntities.Add(existingliftFileRecord.ToEntityForStatusUpdate(userContext));
                            }
                            if (newEntities !=null && newEntities.Any())
                            {
                                Context.DataContext.LiftFileValidationRecords.AddRange(newEntities);
                                await Context.CommitAsync();
                            }
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.msgSuccessReprocessList, newEntities.Count, newEntities.FirstOrDefault().LiftFileId);
                        }
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageReprocessRecord;
                    LogManager.Logger.WriteException("LFVDomain", "AddUnmatchedRecordForReProcessing", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> ProcessUploadedQuebecBadgeListFile(HttpPostedFileBase quebecBadgeListCsvFile, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    // read file
                    var badgeList = new List<QuebecBillingBadgeCsvViewModel>();
                    using (var stream = new MemoryStream())
                    {
                        quebecBadgeListCsvFile.InputStream.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.Configuration.RegisterClassMap<QuebecBillingBadgeCsvViewModelMap>();
                                try
                                {
                                    badgeList = csv.GetRecords<QuebecBillingBadgeCsvViewModel>().ToList();
                                }
                                catch (Exception)
                                {
                                    badgeList = new List<QuebecBillingBadgeCsvViewModel>();
                                }
                            }
                        }


                        // save po details
                        int processCount = 0;
                        int totalCount = badgeList.Count;
                        badgeList = badgeList.Where(t => t.QuebecBillingBadge != "" && t.QuebecBillingBadge != null).ToList();
                        if (badgeList.Any())
                        {
                            var entityList = new List<LiftFileQuebecBillingBadges>();
                            var addedByCompanyId = userContext.CompanyId;
                            var isActive = true;
                            var addedDate = DateTimeOffset.Now;

                            foreach (var badge in badgeList)
                            {
                                // check if record already exists
                                var existingBadge = await Context.DataContext.LiftFileQuebecBillingBadges.Where(t => t.QuebecBillingBadge.ToLower() == badge.QuebecBillingBadge.ToLower().Trim() && t.AddedByCompanyId == addedByCompanyId)
                                                                                               .OrderByDescending(t => t.AddedDate)
                                                                                               .FirstOrDefaultAsync();
                                if (existingBadge != null)
                                {
                                    if (!existingBadge.IsActive)
                                    {
                                        existingBadge.IsActive = true;
                                        Context.DataContext.Entry(existingBadge).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        processCount++;
                                    }
                                }

                                else
                                {
                                    var entity = new LiftFileQuebecBillingBadges();
                                    entity.QuebecBillingBadge = badge.QuebecBillingBadge.Trim();
                                    entity.AddedByCompanyId = addedByCompanyId;
                                    entity.IsActive = isActive;
                                    entity.AddedDate = addedDate;
                                    entityList.Add(entity);

                                    processCount++;
                                }
                            }
                            if (entityList.Any())
                            {
                                Context.DataContext.LiftFileQuebecBillingBadges.AddRange(entityList);
                                await Context.CommitAsync();
                            }
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageLiftDataQuebecBadgeCSVUpload, processCount, totalCount);

                            // upload to blob
                            var azureBlob = new AzureBlobStorage();
                            var filePath = await azureBlob.SaveBlobAsync(quebecBadgeListCsvFile.InputStream, GenerateFileName(userContext.Id, "QuebecBadgeFilter"), BlobContainerType.LiftDataProcessingQuebecBadgeFile.ToString().ToLower());
                        }

                        if (processCount == 0)
                        {
                            response.StatusMessage = Resource.msgerrMessageQuebecBadgeFileEmpty;
                            response.StatusCode = Status.Failed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("LFVDomain", "ProcessUploadedQuebecBadgeListFile", "Lift data processing badge CSV upload failed", ex);
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageProcessLiftDataQuebecBadgeCSVFile;
                }
            }
            return response;
        }

        public async Task<List<LiftFileQuebecBillingBadgeGridViewModel>> GetQuebecBadgeListGridData(int CompanyId)
        {
            var response = new List<LiftFileQuebecBillingBadgeGridViewModel>();
            try
            {
                var billingBadges = await Context.DataContext.LiftFileQuebecBillingBadges.Where(t => t.AddedByCompanyId == CompanyId && t.IsActive).OrderByDescending(t => t.AddedDate).ToListAsync();
                if (billingBadges != null && billingBadges.Any())
                {
                    billingBadges.ForEach(t => response.Add(new LiftFileQuebecBillingBadgeGridViewModel { Id = t.Id, QuebecBillingBadge = t.QuebecBillingBadge }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetQuebecBadgeListGridData", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteBilingBadge(int recordId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (recordId > 0)
                {
                    var existingBadge = await Context.DataContext.LiftFileQuebecBillingBadges.Where(t => t.Id == recordId && t.IsActive).SingleOrDefaultAsync();
                    if (existingBadge != null)
                    {
                        existingBadge.IsActive = false;
                        Context.DataContext.Entry(existingBadge).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMsgBillingBadgeSuccess;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgBillingBadgeUpdateFailed;
                LogManager.Logger.WriteException("LFVDomain", "DeleteBilingBadge", ex.Message, ex);
            }
            return response;
        }

        public List<LFRecordsGridViewModel> GetLFRecordByBolfilenameGrid(string bol,string fileName,UserContext userContext)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = spDomain.GetLFRecordsByBolFilenameGridData(bol,fileName,userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFRecordByBolfilenameGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ProcessLiftFileReportCreationPerCompany()
        {
            var response = false;
            try
            {
                List<int> CompanyIds = Context.DataContext.LiftFileDetails.Where(t => t.IsActive)
                                       .Select(t => t.CompanyId)
                                       .Distinct()
                                       .ToList();
                foreach (var companyId in CompanyIds)
                {
                    // call report creation method  per company 
                    response = await CreateLiftFileReportPerCompany(companyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "ProcessLiftFileReportCreationPerCompany", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> CreateLiftFileReportPerCompany(int companyId)
        {
            var response = false;
            try
            {
                MemoryStream contentStream = null;
                List<LFRecordsGridViewModel> records = new List<LFRecordsGridViewModel>();

                //var startDate = DateTimeOffset.Now.AddDays(-1);
                //var endDate = DateTimeOffset.Now;
                var lfvParameter = GetLfvRequiredParameters(companyId);
                var daysToContinueMatchProcess = lfvParameter != null && lfvParameter.DaysToContinueMatchProcess > 0 ? lfvParameter.DaysToContinueMatchProcess : ApplicationConstants.DefaultNoMatchRecordDays;

                var startDate = DateTimeOffset.Now.AddDays(-daysToContinueMatchProcess);
                var endDate = DateTimeOffset.Now.AddDays(-1);
                records = await new StoredProcedureDomain(this).GetLfRecordsByDateTimeWindow(companyId, startDate.Date, endDate.Date);
                if (records != null && records.Any())
                {
                    var csvRecords = records.Select(t => t.ToCsvViewModel());

                    contentStream = await GetCsvAsMemoryStream(csvRecords);

                    var subject = Resource.emailLFReport_SubjectText;
                    var body = Resource.emailLFReport_BodyText;
                    var fileName = "LiftFileRecordsReport-" + DateTimeOffset.Now.ToString("MM/dd/yyyy HH:mm tt") + ".csv";
                    response = SendEmailCsvFile(contentStream, subject, body, fileName);
                }
                var debugMessage = string.Empty;
                if (records == null || !records.Any())
                {
                    debugMessage = $"No LFRecords found for timewindow {startDate.ToString()} to {endDate.ToString()} for companyId {companyId}";
                }
                else if(records != null && records.Any())
                {
                    if (response)
                    {
                        debugMessage = $"LFRecords dailyreport sent for timewindow {startDate.ToString()} to {endDate.ToString()} for companyId {companyId}";
                    }
                    else
                    {
                        debugMessage = $"Error occured when sending dailyreport for timewindow {startDate.ToString()} to {endDate.ToString()} for companyId {companyId}";
                    }
                    
                }
                LogManager.Logger.WriteDebug("LFVDomain", "CreateLiftFileReportPerCompany", debugMessage);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "CreateLiftFileReportPerCompany", ex.Message, ex);
            }
            return response;
        }

        private async Task<MemoryStream> GetCsvAsMemoryStream(IEnumerable<LfRecordsReportscsvViewModel> transactionsArray)
        {
            var memoryStream = new MemoryStream();
            var flatFileWriter = new StreamWriter(memoryStream, Encoding.ASCII);
            var fileWriterEngine = new FileHelperEngine(typeof(LfRecordsReportscsvViewModel));

            fileWriterEngine.HeaderText = Resource.liftFileRecordsReportHeaders;
            fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

            // Flush contents of fileWriterStream to underlying docStream:
            await flatFileWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        protected bool SendEmailCsvFile(Stream contentStream,string subject, string body, string fileName)
        {
            var response = false;
            try
            {

                HelperDomain helperDomain = new HelperDomain();
                var serverUrl = helperDomain.GetServerUrl();
                Attachment file = new Attachment(contentStream, fileName, Core.Utilities.MediaType.Text);

                var attachements = new List<Attachment>() { file };
                var mailingList = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLiftFileReportMailingList).Select(t => t.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(mailingList))
                {
                    var emails = mailingList.Split(';').ToList();
                    var companyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                    var _emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = emails,
                        Subject = subject,
                        CompanyLogo = companyLogo,
                        BodyText = body,
                        Attachments = attachements,
                        ShowFooterContent = false,
                        ShowHelpLineInfo = false
                    };
                    response = Email.GetClient().Send(_emailTemplate, emailModel);

                }
                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "SendEmailCsvFile", ex.Message, ex);
            }
            return response;
        }

        ////Using same method for Ignore match from nomatch and unmatch grid
        public async Task<StatusViewModel> AddUnmatchedRecordsAsIgnoreMatch(List<int> LfRecordIds, UserContext userContext, int DescriptionId = 0, string DescriptionText = "")
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (LfRecordIds != null && LfRecordIds.Any())
                    {
                        var newEntities = new List<LiftFileValidationRecord>();
                        var existingliftFileRecords = await Context.DataContext.LiftFileValidationRecords.Where(t => LfRecordIds.Contains(t.Id)).ToListAsync();
                        if (existingliftFileRecords != null && existingliftFileRecords.Any())
                        {
                            foreach (var existingliftFileRecord in existingliftFileRecords)
                            {
                                var model = existingliftFileRecord.ToEntityForForcedIgnoreMatch(userContext);
                                if (DescriptionId > 0)
                                {
                                    model.ReasonCodeId = DescriptionId;
                                    model.Reason = DescriptionText;
                                    model.Status = LFVRecordStatus.ForcedIgnore;
                                }
                                newEntities.Add(model);
                            }

                            List<int> LFrecordIdsToUpdate = new List<int>();
                            LFrecordIdsToUpdate.AddRange(existingliftFileRecords.Select(t => t.Id).Distinct().ToList());                           
                            var query = $"UPDATE LiftFileValidationRecords SET Status = {(int)LFVRecordStatus.ReprocessSubmitted}, IsActive= {0}, StatusChangedDate = '{DateTimeOffset.Now}' " +
                                                $"WHERE Id IN ({string.Join<int>(",", LFrecordIdsToUpdate)})";
                            Context.DataContext.Database.ExecuteSqlCommand(query);
                            Context.Commit();

                            if (newEntities != null && newEntities.Any())
                            {
                                Context.DataContext.LiftFileValidationRecords.AddRange(newEntities);
                                await Context.CommitAsync();
                            }
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.msgSuccessIgnoreReprocessList, newEntities.Count);
                        }
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageIgnoreReprocessRecord;
                    LogManager.Logger.WriteException("LFVDomain", "AddUnmatchedRecordsAsIgnoreMatch", ex.Message, ex);
                }
                return response;
            }
        }

        //Supplier BOL Report
        public async Task<List<SupplierBOLReportViewModel>> GetLiftFileRecordsWithMissingTFXDeliveryDetails(UserContext userContext)
        {
            var response = new List<SupplierBOLReportViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var lookBackPeriod = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingsLFVInvoiceWindow).Select(t => t.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(lookBackPeriod))
                {
                    var startDate = DateTimeOffset.Now.AddDays(-Convert.ToInt32(lookBackPeriod));
                    var endDate = DateTimeOffset.Now;
                    response = await spDomain.GetLiftFileRecordsWithMissingTFXDeliveryDetails(userContext.CompanyId, startDate, endDate);
                }               
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLiftFileRecordsWithMissingTFXDeliveryDetails", ex.Message, ex);
            }
            return response;
        }

        //Carrier BOL Report
        public async Task<List<CarrierBOLReportViewModel>> GetTFXDeliveryDetailsWithMissingLiftFileRecords(UserContext userContext,DateTimeOffset? fromDate ,DateTimeOffset? toDate)
        {
            var response = new List<CarrierBOLReportViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                if (fromDate != null && toDate !=null)
                {
                    var startDate = fromDate.Value;
                    var endDate = toDate.Value;
                    response = await spDomain.GetTFXDeliveryDetailsWithMissingLiftFileRecords(userContext.CompanyId, startDate, endDate);
                }
                else
                {
                    var lookBackPeriod = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingsLFVInvoiceWindow).Select(t => t.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(lookBackPeriod))
                    {
                        var startDate = DateTimeOffset.Now.AddDays(-Convert.ToInt32(lookBackPeriod));
                        var endDate = DateTimeOffset.Now;
                        response = await spDomain.GetTFXDeliveryDetailsWithMissingLiftFileRecords(userContext.CompanyId, startDate, endDate);
                    }
                }               
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetTFXDeliveryDetailsWithMissingLiftFileRecords", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<LFRecordsGridViewModel>>GetLiftFileRecordsScratchReport(UserContext userContext)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {   
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetLiftFileRecordsScratchReport(userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLiftFileRecordsScratchReport", ex.Message, ex);

            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetLiftFileCarrierDropDwn(UserContext userContext, DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                // response = await Context.DataContext.LiftFileValidationRecords.Where(w=>w.CarrierID !=null && w.CarrierID != "").Select(s => new DropdownDisplayExtendedItem (){Name= s.CarrierName,Code=s.CarrierID } ).Distinct().ToListAsync();
                var spDomain = new StoredProcedureDomain();
                response = await spDomain.GetLiftFileCarrierDropDownForDashboard(userContext.CompanyId, fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLiftFileCarrierDropDwn", ex.Message, ex);
            }
            return response;
        }

        public  int GetMatchingWindowDays(int companyId)
        {
            int matchingWindowDays = ApplicationConstants.DefaultNoMatchRecordDays;
            try
            {
                var lfvParameter = GetLfvRequiredParameters(companyId);
                matchingWindowDays = lfvParameter != null && lfvParameter.DaysToContinueMatchProcess > 0 ? lfvParameter.DaysToContinueMatchProcess : ApplicationConstants.DefaultNoMatchRecordDays;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetMatchingWindowDays", ex.Message, ex);
            }
            return matchingWindowDays;
        }
        public async Task<List<LFRecordsGridViewModel>> GetLFVAccrualReportGrid(UserContext userContext, AccrualReportGridInputViewModel input, DataTableSearchModel requestModel)
        {
            List<LFRecordsGridViewModel> response = new List<LFRecordsGridViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetLFVAccrualReportGrid(userContext.CompanyId, input, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFVAccrualReportGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetLFVValidationStatsAndProductTypesDDL(UserContext userContext, AccrualReportGridInputViewModel input)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetLFVValidationStatsAndProductTypesDDL(userContext.CompanyId, input);
            }
            catch ( Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetLFVValidationStatsAndProductTypesDDL", ex.Message, ex);
                
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateLiftFileRecord(LFRecordsGridViewModel editedValues,UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (editedValues != null && editedValues.LiftFileRecordId > 0)
                    {
                        var lfvValidationParameters = GetLfvRequiredParameters(userContext.CompanyId);
                        var existingliftFileRecord = await Context.DataContext.LiftFileValidationRecords.Where(t => t.Id == editedValues.LiftFileRecordId).FirstOrDefaultAsync();
                        var newEntity = new LiftFileValidationRecord();
                        if (existingliftFileRecord != null)
                        {
                            newEntity = existingliftFileRecord.ToEntityForLiftFileRecordEdit(lfvValidationParameters, editedValues, userContext);
                            if (newEntity != null)
                            {
                                Context.DataContext.LiftFileValidationRecords.Add(newEntity);

                                existingliftFileRecord.Status = LFVRecordStatus.ReprocessSubmitted;
                                existingliftFileRecord.StatusChangedDate = DateTimeOffset.Now;
                                existingliftFileRecord.IsActive = false;
                                existingliftFileRecord.UpdatedBy = userContext.Id;
                                existingliftFileRecord.UpdatedDate = DateTimeOffset.Now;

                                Context.DataContext.Entry(existingliftFileRecord).State = EntityState.Modified;

                                await Context.CommitAsync();

                                transaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.msgSuccessLFVRecordEdit;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageLFVRecordEdit;
                    LogManager.Logger.WriteException("LFVDomain", "UpdateLiftFileRecord", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<LiftFileStatusResponseViewModel> PushFailedRecords(string token, string bolList)
        {
            var authDomain = new AuthenticationDomain(this);
            var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
            if (apiUserContext != null)
            {
                try
                {
                    LiftFileStatusResponseViewModel responseViewModel = new LiftFileStatusResponseViewModel();
                    var pushedRecords = new List<int>();
                    var bols = bolList.Split(',').ToList();
                    foreach (var bol in bols)
                    {
                        var cleanButNotPushedRecords = Context.DataContext.LiftFileValidationRecords
                                                    .Where(t => t.LiftFileDetails.CompanyId == apiUserContext.CompanyId
                                                            && t.IsActive
                                                            && t.BOL == bol 
                                                            && !t.IsRecordPushedToExternalApi)
                                                   .ToList();

                        if (cleanButNotPushedRecords.All(t => t.Status == LFVRecordStatus.Clean || t.Status == LFVRecordStatus.IgnoreMatch))
                        {
                            foreach (var record in cleanButNotPushedRecords)
                            {
                                responseViewModel.data.Add(record.ToResponseViewModel());
                                pushedRecords.Add(record.Id);
                            }
                        }

                        if(responseViewModel.data.Any() && pushedRecords.Any())
                        {
                            await PushCleanRecordsToExternalApi(apiUserContext.CompanyId, responseViewModel, pushedRecords);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVDomain", "PushFailedRecords", ex.Message + $"bolList = {bolList}", ex);
                }
            }
            return null;
        }

        public async Task<List<DropdownDisplayItem>> GetReasonDescriptionList(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = await new StoredProcedureDomain(this).GetReasonDescriptionList(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "GetReasonDescriptionList", ex.Message, ex);
            }
            return response;
        }
    }
}

