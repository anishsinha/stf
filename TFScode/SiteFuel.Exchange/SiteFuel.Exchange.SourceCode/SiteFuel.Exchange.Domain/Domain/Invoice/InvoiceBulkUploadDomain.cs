using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceBulkUploadDomain : InvoiceBaseDomain
    {
        public InvoiceBulkUploadDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceBulkUploadDomain(string connectionString) : base(connectionString)
        {
        }

        public InvoiceBulkUploadDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> ValidateInvoiceFile(UserContext userContext, string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("InvoiceBulkUploadDomain", "ValidateInvoiceFile"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);

                    var engine = new FileHelperEngine<InvoiceBulkCsvViewModel>();                    
                    var csvDropList = engine.ReadString(csvText).ToList();
                    var allStates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => t.Code.ToLower().Trim()).ToList();
                    int lineNumberOfCSV = 2;
                    StringBuilder errorList = new StringBuilder();
                    if (allStates.Any())
                    {
                        foreach (var record in csvDropList)
                        {
                            await ValidateBaseRow(userContext, record, lineNumberOfCSV, errorList, allStates);
                            lineNumberOfCSV++;
                        }

                        ValidateSupplierInvoiceNumbers(userContext, csvDropList, lineNumberOfCSV, errorList);
                    }
                 
                    if (lineNumberOfCSV == 2)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.Append("</br>");
                        errorList.AppendLine(Resource.errMessageCanNotProcessZeroRecords);
                    }

                    if (errorList.Length <= 0)
                    {
                        ValidateConsolidateInvoice(csvDropList, userContext.CompanyId, errorList);
                    }

                    if (errorList.Length > 0)
                    {
                        response.StatusMessage = errorList.ToString();
                        if (response.StatusMessage.Length > 1000)
                            response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                    }
                    else
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = "TPD invoices uploaded successfully. Please visit upload status page";
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ValidateInvoiceFile", ex.Message, ex);
                }

                return response;
            }
        }

        private void ValidateSupplierInvoiceNumbers(UserContext userContext, List<InvoiceBulkCsvViewModel> csvDropList, int lineNumberOfCSV, StringBuilder errorList)
        {
            var suppInvNumbers = csvDropList.Select(t => t.SupplierInvoiceNumber.Trim()).Distinct().ToList();
            if (suppInvNumbers.Any())
            {
                foreach (var number in suppInvNumbers)
                {
                    if (!string.IsNullOrWhiteSpace(number))
                    {
                        if (IsDuplicateInvoiceNumber(number))
                        {
                            errorList.Append("</br>");
                            errorList.AppendLine(string.Format("Supplier Invoice# {0} already exists", number));
                        }
                    }
                }
            }
        }

        private void ValidateConsolidateInvoice(List<InvoiceBulkCsvViewModel> csvDropList, int supplierCompanyId, System.Text.StringBuilder errorList)
        {
            var consolidatePoNumbers = GetConsolidatePoNumbers(csvDropList, supplierCompanyId);
            foreach (var item in consolidatePoNumbers)
            {
                if (item.DropDetailsViewModel.Count > 1)
                {
                    var isFuelType = item.DropDetailsViewModel.Select(t => t.FuelTypeId).Distinct().ToList();
                    if (isFuelType.Count == 1)
                    {
                        errorList.Append("</br>");
                        var poNumbers = item.DropDetailsViewModel.Select(t => t.PONumber).Distinct().ToList();
                        errorList.AppendLine(string.Format(Resource.errTPDSameFuelTypeInvalid, string.Join(",", poNumbers)));
                        break;
                    }

                    var isBolImageRequired = item.DropDetailsViewModel.Select(t => t.IsBolImageRequired).Distinct().ToList();
                    if (isBolImageRequired.Count > 1)
                    {
                        errorList.Append("</br>");
                        var poNumbers = item.DropDetailsViewModel.Select(t => t.PONumber).Distinct().ToList();
                        errorList.AppendLine(string.Format(Resource.errTPDBolImageFlagInvalid, string.Join(",", poNumbers)));
                        break;
                    }
                }
            }
        }

        private async Task ValidateBaseRow(UserContext userContext, InvoiceBulkCsvViewModel record, int lineNumberOfCSV, System.Text.StringBuilder errorList, List<string> allStates)
        {
            if (!string.IsNullOrWhiteSpace(record.LocationId) && !string.IsNullOrWhiteSpace(record.Drop1ProductId)
                && !string.IsNullOrWhiteSpace(record.CustomerID) && string.IsNullOrWhiteSpace(record.PONumber))
            {
                // get order list using location n productid
                try
                {
                    var suppliercompanyForCarrier = 0;
                    suppliercompanyForCarrier = GetSupplierCompanyIdForCarrier(record, errorList, suppliercompanyForCarrier, userContext);

                    var poFromRecord = record.PONumber;

                    var orderId = GetOrderIdFromLocAndProduct(userContext, record.Drop1ProductId.ToLower().TrimEnd().TrimStart(), record.LocationId, suppliercompanyForCarrier, ref poFromRecord);

                    if (orderId == 0)
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format(Resource.errMsgEnableToFindPOFromAtLine, record.LocationId, record.Drop1ProductId, lineNumberOfCSV));
                    }

                    record.PONumber = poFromRecord;
                }
                catch (Exception)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMsgMultipleOrdersFoundAtLine, record.LocationId, record.Drop1ProductId, lineNumberOfCSV));
                }
            }

            if (!string.IsNullOrWhiteSpace(record.PONumber))
            {
                //existing code
                //validate order for this company
                var orderList =await Context.DataContext.Orders
                                    .Where(t => t.PoNumber.ToLower().TrimEnd().TrimStart().Equals(record.PONumber.ToLower().TrimEnd().TrimStart())
                                            && t.AcceptedCompanyId == userContext.CompanyId
                                            && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                    .Select(t => new
                                    {
                                        t.Id,
                                        t.PoNumber,
                                        //t.IsFTL,
                                        t.AcceptedDate,
                                        FrStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                                        t.FuelRequest.Job.LocationType,
                                        t.FuelRequest.FreightOnBoardTypeId,
                                        t.FuelRequest.UoM,
                                        t.FuelRequest.Job.IsMarine,
                                        t.FuelRequest.Job.CountryId,
                                        IsDriverToUpdateBOL = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.IsDriverToUpdateBOL
                                    }).ToListAsync();

                bool fobVarious = false;
                if (orderList.Any())
                {
                    if (orderList.Count > 1)
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format("Multiple order exists with same PO# {0} at line {1}", record.PONumber, lineNumberOfCSV));
                        return;
                    }

                    var order = orderList.FirstOrDefault();

                    if (order != null)
                    {
                        fobVarious = order.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.LocationType == JobLocationTypes.Various;
                        var allowedDropDate = order.AcceptedDate.Date < order.FrStartDate.Date ? order.AcceptedDate : order.FrStartDate;
                        //validate mandetory feilds
                        bool isItDryRunInvoice = CheckIfDryRunInvoice(record);

                        if (isItDryRunInvoice)
                        {
                            ValidateDrop1DryRunParameters(record, lineNumberOfCSV, errorList, allowedDropDate);
                            ValidateOtherParametersForDryRun(record, lineNumberOfCSV, errorList, allowedDropDate);
                        }
                        else
                        {
                            ValidateRequiredParameters(record, lineNumberOfCSV, errorList, allowedDropDate, order.Id, userContext.CompanyId);
                            ValidateBulkPlantDetais(record, lineNumberOfCSV, errorList, allStates, userContext);
                            if (order.IsDriverToUpdateBOL)//order.IsFTL || 
                            {
                                if (string.IsNullOrWhiteSpace(record.TerminalControlNumber))
                                {
                                    ValidateLiftAddress(record, lineNumberOfCSV, errorList, allStates, userContext);
                                }
                                else
                                {
                                    ValidateLiftTicketParameterValues(record, lineNumberOfCSV, errorList, allStates);
                                }
                            }

                            if (order.IsDriverToUpdateBOL)//order.IsFTL || 
                            {
                                //validte ftl parameters                                
                                ValidateFTLParameters(record, lineNumberOfCSV, errorList);

                                //NO NEED - FTL ASSETS ARE ALLOWED
                                //ValidateAssetId(record, lineNumberOfCSV, errorList);
                            }

                            //validate other parameters
                            ValidateOptionalParameters(record, lineNumberOfCSV, errorList, allStates, fobVarious, userContext);

                            ValidateSplitLoadParameters(record, lineNumberOfCSV, errorList, allStates, allowedDropDate, fobVarious);

                            if (order.IsMarine && order.UoM == UoM.MetricTons)
                            {
                                ValidateParametersForMFN(record, lineNumberOfCSV, errorList, order.UoM, order.CountryId);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(record.PONumber))
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format("PoNumber is invalid at line {1}", record.PONumber, lineNumberOfCSV));
                    }
                    else
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format("{0} is invalid at line {1}", record.PONumber, lineNumberOfCSV));
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(record.LocationId) || string.IsNullOrWhiteSpace(record.Drop1ProductId) || string.IsNullOrWhiteSpace(record.CustomerID))
                {
                    //validation error. PO or CustomerId - LocationId - Product is required
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("PO Number OR CustomerID - LocationId - Drop1Product ID is required at line {0}", lineNumberOfCSV));

                }
                else if (string.IsNullOrWhiteSpace(record.PONumber))
                {
                    //validation error. PO or LocationId and Product is required
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("PoNumber is required at line {0}", lineNumberOfCSV));
                }

                if (!string.IsNullOrWhiteSpace(record.LocationId) && !string.IsNullOrWhiteSpace(record.Drop1ProductId))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("LocationId and Drop1ProductId is invalid at line {0}", lineNumberOfCSV));
                }
            }
        }

        private int GetSupplierCompanyIdForCarrier(InvoiceBulkCsvViewModel record, StringBuilder errorList, int suppliercompanyForCarrier, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(record.CustomerID))
            {
                try
                {
                    suppliercompanyForCarrier = Context.DataContext.CarrierCustomerMappings
                                        .Where(t => t.CarrierAssignedCustomerId.ToLower().Equals(record.CustomerID.ToLower().TrimEnd()) && t.IsActive
                                                && t.CarrierCompanyId == userContext.CompanyId) //carriercompanyId is createdby companyid
                                        .Select(t => t.CarrierCustomerId).SingleOrDefault();

                    if (suppliercompanyForCarrier == 0)
                    {
                        suppliercompanyForCarrier = Context.DataContext.Companies
                                        .Where(t => t.Name.ToLower().Equals(record.CustomerID.ToLower().TrimEnd()))
                                        .Select(t => t.Id).SingleOrDefault();

                        if (suppliercompanyForCarrier == 0)
                        {
                            errorList.Append("</br>");
                            errorList.AppendLine(string.Format(Resource.errMsgParameterIsRequired, "Customer ID"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "GetSupplierCompanyIdForCarrier", ex.Message, ex);

                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMsgMultipleCompaniesFound, record.CustomerID));
                }
            }

            return suppliercompanyForCarrier;
        }

        private void ValidateOtherParametersForDryRun(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset allowedDropDate)
        {
            //all other parameters than req should be empty for DRY RUN
            if (!string.IsNullOrWhiteSpace(record.BolCarrier) || !string.IsNullOrWhiteSpace(record.BolCreationTime) || !string.IsNullOrWhiteSpace(record.BolGross)
                || !string.IsNullOrWhiteSpace(record.BolNet) || !string.IsNullOrWhiteSpace(record.BolNumber) || !string.IsNullOrWhiteSpace(record.DriverFirstName)
                || !string.IsNullOrWhiteSpace(record.DriverLastName) || !string.IsNullOrWhiteSpace(record.Drop1AddressCity) || !string.IsNullOrWhiteSpace(record.Drop1AddressLat)
                || !string.IsNullOrWhiteSpace(record.Drop1AddressLong) || !string.IsNullOrWhiteSpace(record.Drop1AddressState) || !string.IsNullOrWhiteSpace(record.Drop1AddressStreet1)
                || !string.IsNullOrWhiteSpace(record.Drop1AddressStreet2) || !string.IsNullOrWhiteSpace(record.Drop1AddressZip)
                || !string.IsNullOrWhiteSpace(record.Drop1AssetId) || !string.IsNullOrWhiteSpace(record.Drop1DemurrageFees) || !string.IsNullOrWhiteSpace(record.Drop1DemurrageTime) || !string.IsNullOrWhiteSpace(record.Drop1EnvironmentalFees)
                || !string.IsNullOrWhiteSpace(record.Drop1FreightFees) || !string.IsNullOrWhiteSpace(record.Drop1LoadFees) || !string.IsNullOrWhiteSpace(record.Drop1Notes)
                || !string.IsNullOrWhiteSpace(record.Drop1OtherFees) || !string.IsNullOrWhiteSpace(record.Drop1OverWaterFees) || !string.IsNullOrWhiteSpace(record.Drop1Quantity)
                || !string.IsNullOrWhiteSpace(record.Drop1ServiceFees) || !string.IsNullOrWhiteSpace(record.Drop1SurchargeFees) || !string.IsNullOrWhiteSpace(record.Drop1TicketNumber)
                || !string.IsNullOrWhiteSpace(record.Drop1WethoseFees) || !string.IsNullOrWhiteSpace(record.Drop2AddressCity) || !string.IsNullOrWhiteSpace(record.Drop2AddressLat)
                || !string.IsNullOrWhiteSpace(record.Drop2AddressLong) || !string.IsNullOrWhiteSpace(record.Drop2AddressState) || !string.IsNullOrWhiteSpace(record.Drop2AddressStreet1)
                || !string.IsNullOrWhiteSpace(record.Drop2AddressStreet2) || !string.IsNullOrWhiteSpace(record.Drop2AddressZip) || !string.IsNullOrWhiteSpace(record.Drop2ArrivalDate)
                || !string.IsNullOrWhiteSpace(record.Drop2ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop2AssetId) || !string.IsNullOrWhiteSpace(record.Drop2CompleteTime)
                || !string.IsNullOrWhiteSpace(record.Drop2DemurrageFees) || !string.IsNullOrWhiteSpace(record.Drop2DemurrageTime) || !string.IsNullOrWhiteSpace(record.Drop2DryRunCount)
                || !string.IsNullOrWhiteSpace(record.Drop2DryRunFees) || !string.IsNullOrWhiteSpace(record.Drop2EnvironmentalFees) || !string.IsNullOrWhiteSpace(record.Drop2FreightFees)
                || !string.IsNullOrWhiteSpace(record.Drop2LoadFees) || !string.IsNullOrWhiteSpace(record.Drop2Notes) || !string.IsNullOrWhiteSpace(record.Drop2OtherFees)
                || !string.IsNullOrWhiteSpace(record.Drop2OverWaterFees) || !string.IsNullOrWhiteSpace(record.Drop2Quantity) || !string.IsNullOrWhiteSpace(record.Drop2ServiceFees)
                || !string.IsNullOrWhiteSpace(record.Drop2SurchargeFees) || !string.IsNullOrWhiteSpace(record.Drop2TicketNumber) || !string.IsNullOrWhiteSpace(record.Drop2WethoseFees)
               )
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format("Only ArrivalDate,ArrivalTime, DryRunCount and DryRunFee is required for Dry run invoice. All other columns must be empty at line {0}", lineNumberOfCSV));
            }
        }

        private void ValidateAssetId(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList)
        {
            if (!string.IsNullOrWhiteSpace(record.Drop1AssetId))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDAssetNotSupportsForFTL, record.Drop1AssetId, lineNumberOfCSV));
            }

            if (!string.IsNullOrWhiteSpace(record.Drop2AssetId))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDAssetNotSupportsForFTL, record.Drop2AssetId, lineNumberOfCSV));
            }

            if (!string.IsNullOrWhiteSpace(record.Drop3AssetId))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDAssetNotSupportsForFTL, record.Drop3AssetId, lineNumberOfCSV));
            }

            if (!string.IsNullOrWhiteSpace(record.Drop4AssetId))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDAssetNotSupportsForFTL, record.Drop4AssetId, lineNumberOfCSV));
            }

        }

        private void ValidateSplitLoadParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, DateTimeOffset acceptedDate, bool fobVarious)
        {
            //validate on way parameters -- NEED TO CONFIRM FROM SARAH
            if (!string.IsNullOrWhiteSpace(record.Drop2ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop2ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop2CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop2Quantity))
            {
                ValidateDrop2Parameters(record, lineNumberOfCSV, errorList, acceptedDate, fobVarious);

                if (!string.IsNullOrWhiteSpace(record.Drop2TicketNumber) && string.IsNullOrWhiteSpace(record.Drop2AssetId))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AssetId), lineNumberOfCSV));

                //validate drop1 parameters
                ValidateDrop2AddressParameters(record, lineNumberOfCSV, errorList, allStates, fobVarious);

                if (!string.IsNullOrWhiteSpace(record.Drop2DemurrageFees) && !string.IsNullOrWhiteSpace(record.Drop2DemurrageTime))
                    ValidateDecimalParameter(nameof(record.Drop2DemurrageTime), record.Drop2DemurrageTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop2DemurrageTime))
                    if (string.IsNullOrWhiteSpace(record.Drop2DemurrageFees))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2DemurrageFees), lineNumberOfCSV));
                    else
                        ValidateDecimalParameter(nameof(record.Drop2DemurrageFees), record.Drop2DemurrageFees, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop2Quantity) && (!string.IsNullOrWhiteSpace(record.Drop2DryRunCount) || !string.IsNullOrWhiteSpace(record.Drop2DryRunFees)))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileDryRunFeeWithDropInfo, lineNumberOfCSV));
                }
            }

            //validate drop3
            if (!string.IsNullOrWhiteSpace(record.Drop3ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop3ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop3CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop3Quantity))
            {
                ValidateDrop3Parameters(record, lineNumberOfCSV, errorList, acceptedDate);

                if (!string.IsNullOrWhiteSpace(record.Drop3TicketNumber) && string.IsNullOrWhiteSpace(record.Drop3AssetId))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AssetId), lineNumberOfCSV));

                //validate drop1 parameters
                ValidateDrop3AddressParameters(record, lineNumberOfCSV, errorList, allStates, fobVarious);

                if (!string.IsNullOrWhiteSpace(record.Drop3DemurrageFees) && !string.IsNullOrWhiteSpace(record.Drop3DemurrageTime))
                    ValidateDecimalParameter(nameof(record.Drop3DemurrageTime), record.Drop3DemurrageTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop3DemurrageTime))
                    if (string.IsNullOrWhiteSpace(record.Drop3DemurrageFees))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3DemurrageFees), lineNumberOfCSV));
                    else
                        ValidateDecimalParameter(nameof(record.Drop3DemurrageFees), record.Drop3DemurrageFees, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop3Quantity) &&
                     (!string.IsNullOrWhiteSpace(record.Drop3DryRunCount) || !string.IsNullOrWhiteSpace(record.Drop3DryRunFees)))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileDryRunFeeWithDropInfo, lineNumberOfCSV));
                }
            }

            //validate drop4
            if (!string.IsNullOrWhiteSpace(record.Drop4ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop4ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop4CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop4Quantity))
            {
                ValidateDrop4Parameters(record, lineNumberOfCSV, errorList, acceptedDate);

                if (!string.IsNullOrWhiteSpace(record.Drop4TicketNumber) && string.IsNullOrWhiteSpace(record.Drop4AssetId))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AssetId), lineNumberOfCSV));

                //validate drop1 parameters
                ValidateDrop4AddressParameters(record, lineNumberOfCSV, errorList, allStates, fobVarious);

                if (!string.IsNullOrWhiteSpace(record.Drop4DemurrageFees) && !string.IsNullOrWhiteSpace(record.Drop4DemurrageTime))
                    ValidateDecimalParameter(nameof(record.Drop4DemurrageTime), record.Drop4DemurrageTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop4DemurrageTime))
                    if (string.IsNullOrWhiteSpace(record.Drop4DemurrageFees))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4DemurrageFees), lineNumberOfCSV));
                    else
                        ValidateDecimalParameter(nameof(record.Drop4DemurrageFees), record.Drop4DemurrageFees, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.Drop4Quantity)
                    && (!string.IsNullOrWhiteSpace(record.Drop4DryRunCount) || !string.IsNullOrWhiteSpace(record.Drop4DryRunFees)))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileDryRunFeeWithDropInfo, lineNumberOfCSV));
                }
            }
        }

        private void ValidateOptionalParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, bool isFobVarious, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(record.DriverFirstName) && string.IsNullOrWhiteSpace(record.DriverLastName))
            {
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.DriverLastName), lineNumberOfCSV));
            }

            if (!string.IsNullOrWhiteSpace(record.DriverLastName) && string.IsNullOrWhiteSpace(record.DriverFirstName))
            {
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.DriverFirstName), lineNumberOfCSV));
            }
        
            ValidateDeliveredQuantity(record, lineNumberOfCSV, errorList);
            // validate terminal control number
            ValidateTerminalControlNumberParameter(record, lineNumberOfCSV, errorList, userContext);

            //validate on way parameters -- NEED TO CONFIRM FROM SARAH

            if (!string.IsNullOrWhiteSpace(record.Drop1TicketNumber) && string.IsNullOrWhiteSpace(record.Drop1AssetId))
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AssetId), lineNumberOfCSV));

            //validate drop1 parameters
            ValidateDrop1AddressParameters(record, lineNumberOfCSV, errorList, allStates, isFobVarious);

            if (!string.IsNullOrWhiteSpace(record.Drop1DemurrageFees) && !string.IsNullOrWhiteSpace(record.Drop1DemurrageTime))
                ValidateDecimalParameter(nameof(record.Drop1DemurrageTime), record.Drop1DemurrageTime, lineNumberOfCSV, errorList);

            if (!string.IsNullOrWhiteSpace(record.Drop1DemurrageTime))
            {
                if (string.IsNullOrWhiteSpace(record.Drop1DemurrageFees))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1DemurrageFees), lineNumberOfCSV));
                }
                else
                    ValidateDecimalParameter(nameof(record.Drop1DemurrageFees), record.Drop1DemurrageFees, lineNumberOfCSV, errorList);
            }

            if (!string.IsNullOrWhiteSpace(record.Drop1Quantity) &&
                (!string.IsNullOrWhiteSpace(record.Drop1DryRunCount) || !string.IsNullOrWhiteSpace(record.Drop1DryRunFees)))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileDryRunFeeWithDropInfo, lineNumberOfCSV));
            }

            if (!string.IsNullOrWhiteSpace(record.FuelCost))
            {
                ValidateDecimalParameter(nameof(record.FuelCost), record.FuelCost, lineNumberOfCSV, errorList);
            }

            if (!string.IsNullOrWhiteSpace(record.OrderQuantity))
            {
                ValidateDecimalParameter(nameof(record.OrderQuantity), record.OrderQuantity, lineNumberOfCSV, errorList);
            }

            if (!string.IsNullOrWhiteSpace(record.OrderDate))
                ValidateDateParameter(nameof(record.OrderDate), record.OrderDate, lineNumberOfCSV, errorList);
        }

        private void ValidateParametersForMFN(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, UoM uom, int countryId)
        {
            var invoiceDomain = new InvoiceDomain(this);
            decimal drop1ApiGravity = 0;
            if (string.IsNullOrWhiteSpace(record.Drop1ApiGravity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1ApiGravity), lineNumberOfCSV));
            }
            else
            {
                decimal.TryParse(record.Drop1ApiGravity, out drop1ApiGravity);
                if (drop1ApiGravity <= 0)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop1ApiGravity), lineNumberOfCSV));
                }
            }

            if (drop1ApiGravity > 0)
            {
                decimal.TryParse(record.Drop1Quantity, out decimal drop1Quantity);
                var conversionResponse = ValidateApiGravity(invoiceDomain, drop1Quantity, drop1ApiGravity, countryId, uom);
                if (!conversionResponse.IsValidGravity)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop1ApiGravity), lineNumberOfCSV));
                }
            }

            // split drop - check Api Gravity for mfn
            if (!string.IsNullOrWhiteSpace(record.Drop2ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop2ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop2CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop2Quantity))
            {
                ValidateDrop2ApiGravity(record, lineNumberOfCSV, errorList, uom, countryId, invoiceDomain);
            }

            if (!string.IsNullOrWhiteSpace(record.Drop3ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop3ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop3CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop3Quantity))
            {
                ValidateDrop3ApiGravity(record, lineNumberOfCSV, errorList, uom, countryId, invoiceDomain);
            }

            if (!string.IsNullOrWhiteSpace(record.Drop4ArrivalDate) || !string.IsNullOrWhiteSpace(record.Drop4ArrivalTime) || !string.IsNullOrWhiteSpace(record.Drop4CompleteTime) || !string.IsNullOrWhiteSpace(record.Drop4Quantity))
            {
                ValidateDrop4ApiGravity(record, lineNumberOfCSV, errorList, uom, countryId, invoiceDomain);
            }
        }

        private void ValidateDrop2ApiGravity(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, UoM uom, int countryId, InvoiceDomain invoiceDomain)
        {
            decimal drop2ApiGravity = 0;
            if (string.IsNullOrWhiteSpace(record.Drop2ApiGravity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2ApiGravity), lineNumberOfCSV));
            }
            else
            {
                decimal.TryParse(record.Drop2ApiGravity, out drop2ApiGravity);
                if (drop2ApiGravity <= 0)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop2ApiGravity), lineNumberOfCSV));
                }
            }

            if (drop2ApiGravity > 0)
            {
                decimal.TryParse(record.Drop2Quantity, out decimal drop2Quantity);
                var conversionResponse = ValidateApiGravity(invoiceDomain, drop2Quantity, drop2ApiGravity, countryId, uom);
                if (!conversionResponse.IsValidGravity)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop2ApiGravity), lineNumberOfCSV));
                }
            }
        }

        private void ValidateDrop3ApiGravity(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, UoM uom, int countryId, InvoiceDomain invoiceDomain)
        {
            decimal drop3ApiGravity = 0;
            if (string.IsNullOrWhiteSpace(record.Drop3ApiGravity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3ApiGravity), lineNumberOfCSV));
            }
            else
            {
                decimal.TryParse(record.Drop3ApiGravity, out drop3ApiGravity);
                if (drop3ApiGravity <= 0)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop3ApiGravity), lineNumberOfCSV));
                }
            }

            if (drop3ApiGravity > 0)
            {
                decimal.TryParse(record.Drop3Quantity, out decimal drop3Quantity);
                var conversionResponse = ValidateApiGravity(invoiceDomain, drop3Quantity, drop3ApiGravity, countryId, uom);
                if (!conversionResponse.IsValidGravity)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop3ApiGravity), lineNumberOfCSV));
                }
            }
        }

        private void ValidateDrop4ApiGravity(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, UoM uom, int countryId, InvoiceDomain invoiceDomain)
        {
            decimal drop4ApiGravity = 0;
            if (string.IsNullOrWhiteSpace(record.Drop4ApiGravity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4ApiGravity), lineNumberOfCSV));
            }
            else
            {
                decimal.TryParse(record.Drop4ApiGravity, out drop4ApiGravity);
                if (drop4ApiGravity <= 0)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop4ApiGravity), lineNumberOfCSV));
                }
            }

            if (drop4ApiGravity > 0)
            {
                decimal.TryParse(record.Drop4Quantity, out decimal Drop4Quantity);
                var conversionResponse = ValidateApiGravity(invoiceDomain, Drop4Quantity, drop4ApiGravity, countryId, uom);
                if (!conversionResponse.IsValidGravity)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMessageInValidAtLine, nameof(record.Drop4ApiGravity), lineNumberOfCSV));
                }
            }
        }

        private MFNConversionResponseViewModel ValidateApiGravity(InvoiceDomain invoiceDomain, decimal dropQuantity, decimal apiGravity, int countryId, UoM uom)
        {
            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = dropQuantity, ConversionFactor = apiGravity, JobCountryId = countryId, UoM = uom };
            var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
            return conversionResponse;
        }
        private void ValidateDeliveredQuantity(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList)
        {
               
            int BolNet = String.IsNullOrWhiteSpace(record.BolNet) ? 0 : Convert.ToInt32(record.BolNet);
            int BolGross = String.IsNullOrWhiteSpace(record.BolGross) ? 0 : Convert.ToInt32(record.BolGross);
            int LiftGross = String.IsNullOrWhiteSpace(record.LiftGross) ? 0 : Convert.ToInt32(record.LiftGross);
            int LiftNet = String.IsNullOrWhiteSpace(record.LiftNet) ? 0 : Convert.ToInt32(record.LiftNet);
            int BolDelivered = String.IsNullOrWhiteSpace(record.BolDelivered) ? 0 : Convert.ToInt32(record.BolDelivered);
            int LiftDelivered = String.IsNullOrWhiteSpace(record.LiftDelivered) ? 0 : Convert.ToInt32(record.LiftDelivered);
            int Dropped1Quantity = String.IsNullOrWhiteSpace(record.Drop1Quantity) ? 0 : Convert.ToInt32(record.Drop1Quantity);
            int Dropped2Quantity = String.IsNullOrWhiteSpace(record.Drop2Quantity) ? 0 : Convert.ToInt32(record.Drop2Quantity);
            int Dropped3Quantity = String.IsNullOrWhiteSpace(record.Drop3Quantity) ? 0 : Convert.ToInt32(record.Drop3Quantity);
            int Dropped4Quantity = String.IsNullOrWhiteSpace(record.Drop4Quantity) ? 0 : Convert.ToInt32(record.Drop4Quantity);
            int DroppedQuantity = (Dropped1Quantity + Dropped2Quantity + Dropped3Quantity + Dropped4Quantity);
            int liftQty = String.IsNullOrWhiteSpace(record.LiftQuantity) ? 0 : Convert.ToInt32(record.LiftQuantity);

            if (liftQty != 0)
            {
                if (LiftNet == 0 && LiftGross == 0)
                {
                    LiftGross = LiftNet = liftQty;
                }
            }
            if (!String.IsNullOrWhiteSpace(record.BolNet) || !String.IsNullOrWhiteSpace(record.LiftNet))
            {
                if (((BolNet + LiftNet) < DroppedQuantity) && ((BolGross + LiftGross) < DroppedQuantity))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("Total drop quantity cannot exceed the BOL/Lift Ticket volume at line {0}", lineNumberOfCSV));
                }
                if ((BolDelivered > BolGross && BolDelivered > BolNet))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("Bol Delivered quantity should not exceed the Bol Net and Bol Gross quantity at line {0}", lineNumberOfCSV));
                }
                if (LiftDelivered > LiftGross && LiftDelivered > LiftNet)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("Lift Delivered quantity should not exceed the Lift Net and Lift Gross quantity at line {0}", lineNumberOfCSV));
                }
                if (LiftDelivered != 0 || BolDelivered != 0)
                {
                    int SumOfDelivered = LiftDelivered + BolDelivered;
                    if (SumOfDelivered != DroppedQuantity)
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format("Total Drop quantity should match the Total Delivered quantity at line {0}", lineNumberOfCSV));
                    }
                }
            }
        }
        private void ValidateTerminalControlNumberParameter(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(record.TerminalControlNumber))
            {
                var terminalControlNumber = Context.DataContext.MstExternalTerminals.Where(t => t.IsActive && t.ControlNumber != null && t.ControlNumber != string.Empty
                                                        && (t.ControlNumber.ToLower() == record.TerminalControlNumber.ToLower() || t.Name.ToLower() == record.TerminalControlNumber.ToLower()))
                                                                       .Select(t => t.ControlNumber)
                                                                       .FirstOrDefault();
                if (terminalControlNumber == null)
                {
                    terminalControlNumber = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == userContext.CompanyId && t.TerminalId != null
                                                    && t.IsActive && t.TerminalSupplierId == null && t.AssignedTerminalId.ToLower().Equals(record.TerminalControlNumber.ToLower()))
                                                    .Select(t => t.MstExternalTerminal.ControlNumber).FirstOrDefault();
                }

                if (terminalControlNumber == null)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format("{0} is invalid at line {1}", nameof(record.TerminalControlNumber), lineNumberOfCSV));
                }
            }
        }

        private static void ValidateDrop1AddressParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, bool isFobVarious)
        {
            if (!string.IsNullOrWhiteSpace(record.Drop1AddressZip))
            {
                if (string.IsNullOrWhiteSpace(record.Drop1AddressStreet1) && !isFobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressStreet1), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop1AddressCity) && !isFobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressCity), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop1AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop1AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop1AddressState), lineNumberOfCSV));
                }
            }

            if (isFobVarious)
            {
                if (string.IsNullOrWhiteSpace(record.Drop1AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop1AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop1AddressState), lineNumberOfCSV));
                }
            }

            if (!string.IsNullOrWhiteSpace(record.Drop1AddressLat))
                if (string.IsNullOrWhiteSpace(record.Drop1AddressLong))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressLong), lineNumberOfCSV));

            if (!string.IsNullOrWhiteSpace(record.Drop1AddressLong))
                if (string.IsNullOrWhiteSpace(record.Drop1AddressLat))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AddressLat), lineNumberOfCSV));
        }

        private static void ValidateDrop2AddressParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, bool fobVarious)
        {
            if (!string.IsNullOrWhiteSpace(record.Drop2AddressZip))
            {
                if (string.IsNullOrWhiteSpace(record.Drop2AddressStreet1) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressStreet1), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop2AddressCity) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressCity), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop2AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop2AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop2AddressState), lineNumberOfCSV));
                }
            }

            if (fobVarious)
            {
                if (string.IsNullOrWhiteSpace(record.Drop2AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop2AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop2AddressState), lineNumberOfCSV));
                }
            }

            if (!string.IsNullOrWhiteSpace(record.Drop2AddressLat))
                if (string.IsNullOrWhiteSpace(record.Drop2AddressLong))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressLong), lineNumberOfCSV));

            if (!string.IsNullOrWhiteSpace(record.Drop2AddressLong))
                if (string.IsNullOrWhiteSpace(record.Drop2AddressLat))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2AddressLat), lineNumberOfCSV));
        }

        private static void ValidateDrop3AddressParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, bool fobVarious)
        {
            if (!string.IsNullOrWhiteSpace(record.Drop3AddressZip))
            {
                if (string.IsNullOrWhiteSpace(record.Drop3AddressStreet1) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressStreet1), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop3AddressCity) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressCity), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop3AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop3AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop3AddressState), lineNumberOfCSV));
                }
            }

            if (fobVarious)
            {
                if (string.IsNullOrWhiteSpace(record.Drop3AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop3AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop3AddressState), lineNumberOfCSV));
                }
            }

            if (!string.IsNullOrWhiteSpace(record.Drop3AddressLat))
                if (string.IsNullOrWhiteSpace(record.Drop3AddressLong))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressLong), lineNumberOfCSV));

            if (!string.IsNullOrWhiteSpace(record.Drop3AddressLong))
                if (string.IsNullOrWhiteSpace(record.Drop3AddressLat))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3AddressLat), lineNumberOfCSV));
        }

        private static void ValidateDrop4AddressParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, bool fobVarious)
        {
            if (!string.IsNullOrWhiteSpace(record.Drop4AddressZip))
            {
                if (string.IsNullOrWhiteSpace(record.Drop4AddressStreet1) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressStreet1), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop4AddressCity) && !fobVarious)
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressCity), lineNumberOfCSV));

                if (string.IsNullOrWhiteSpace(record.Drop4AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop4AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop4AddressState), lineNumberOfCSV));
                }
            }

            if (fobVarious)
            {
                if (string.IsNullOrWhiteSpace(record.Drop4AddressState))
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressState), lineNumberOfCSV));
                else
                {
                    if (!allStates.Contains(record.Drop4AddressState.ToLower()))
                        errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.Drop4AddressState), lineNumberOfCSV));
                }
            }

            if (!string.IsNullOrWhiteSpace(record.Drop4AddressLat) && string.IsNullOrWhiteSpace(record.Drop4AddressLong))
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressLong), lineNumberOfCSV));

            if (!string.IsNullOrWhiteSpace(record.Drop4AddressLong) && string.IsNullOrWhiteSpace(record.Drop4AddressLat))
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4AddressLat), lineNumberOfCSV));
        }

        private void ValidateBulkPlantDetais(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(record.LiftTicketNumber))
            {
                if (string.IsNullOrWhiteSpace(record.BulkPlantName))
                {
                    errorList.AppendLine(string.Format(Resource.errTPDBulkPlantNameRequired, lineNumberOfCSV));
                }
            }
            if (!string.IsNullOrWhiteSpace(record.BulkPlantName))
            {
                validateBulkPlant(record, lineNumberOfCSV, errorList, allStates, userContext);
            }
        }

        private void GetAdrressByZipCode(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList)
        {
            var point = GoogleApiDomain.GetGeocode($"{record.LiftAddressZip}");
            if (point == null)
            {
                //SOMETIMES FROM ZIP NOT GETTING GEOCODE
                point = GoogleApiDomain.GetGeocode($"{record.LiftAddressStreet1} {record.LiftAddressStreet2} {record.LiftAddressCity} {record.LiftAddressState} {record.LiftAddressZip}");
            }
            if (point != null)
            {             
                record.LiftAddressState = point.StateCode;              
                record.LiftAddressCity = point.City;
                record.LiftAddressLat = point.Latitude.ToString();
                record.LiftAddressLong = point.Longitude.ToString();
            }
            else
            {
                errorList.AppendLine(string.Format(Resource.errInvalidAddress, lineNumberOfCSV));
            }
        }
        private void GetAdrressByLatLong(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates)
        {
            var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(record.LiftAddressLat), Convert.ToDouble(record.LiftAddressLong));
            if (geoAddress == null || geoAddress.ZipCode == null || geoAddress.StateCode == null)
                errorList.AppendLine(string.Format(Resource.errInvalidLatLong, lineNumberOfCSV));
            else
            {               
                record.LiftAddressStreet1 = geoAddress.FormattedAddress;              
                record.LiftAddressState = geoAddress.StateCode;
                if (!allStates.Any(t => t == record.LiftAddressState.ToLower()))
                    errorList.AppendLine(string.Format(Resource.errorMsgParameterFormatIsInvalid, nameof(record.LiftAddressState), lineNumberOfCSV));
                record.LiftAddressCity = geoAddress.City;
                record.LiftAddressZip = geoAddress.ZipCode;
            }
        }
        private void validateBulkPlant(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, UserContext userContext)
        {
            bool isExistingBulkPlant = false;
            var bulkPlantdetails = ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlantDetailsByName(record.BulkPlantName.Trim(), userContext.CompanyId);
            //FALL BACK FOR MAPPING
            if (bulkPlantdetails != null && bulkPlantdetails.SiteId == 0)
            {
                var bulkplantMapping = Context.DataContext.TerminalCompanyAliases.Where(t => t.IsActive
                                            && t.CreatedByCompanyId == userContext.CompanyId
                                        && t.AssignedTerminalId.ToLower() == record.BulkPlantName.Trim().ToLower() && t.BulkPlantId != null)
                            .Select(t => new
                            {
                                t.BulkPlantLocation.City,
                                t.BulkPlantLocation.CountyName,
                                t.BulkPlantLocation.Latitude,
                                t.BulkPlantLocation.Longitude,
                                t.BulkPlantLocation.StateCode,
                                t.BulkPlantLocation.Address,
                                t.BulkPlantLocation.ZipCode,
                                t.BulkPlantLocation.CountryCode,
                                t.BulkPlantLocation.Name
                            })
                            .FirstOrDefault();
                if (bulkplantMapping != null)
                {
                    isExistingBulkPlant = true;                    
                    record.LiftAddressCity = bulkplantMapping.City;                    
                    if (bulkplantMapping.Latitude == 0 || bulkplantMapping.Longitude == 0)
                        GetAdrressByZipCode(record, lineNumberOfCSV, errorList);
                    else
                    {
                        record.LiftAddressLat = bulkplantMapping.Latitude.ToString();
                        record.LiftAddressLong = bulkplantMapping.Longitude.ToString();
                    }

                    record.LiftAddressLat = bulkplantMapping.Latitude.ToString();
                    record.LiftAddressLong = bulkplantMapping.Longitude.ToString();
                    record.LiftAddressState = bulkplantMapping.StateCode;
                    record.LiftAddressStreet1 = bulkplantMapping.Address;
                    record.LiftAddressZip = bulkplantMapping.ZipCode;
                    record.BulkPlantName = bulkplantMapping.Name;
                }
            }

            if (bulkPlantdetails != null && bulkPlantdetails.SiteId > 0 && !string.IsNullOrWhiteSpace(bulkPlantdetails.ZipCode))
            {               
                isExistingBulkPlant = true;                
                record.LiftAddressCity = bulkPlantdetails.City;                
                record.LiftAddressState = bulkPlantdetails.State.Code;
                record.LiftAddressStreet1 = bulkPlantdetails.Address;
                record.LiftAddressZip = bulkPlantdetails.ZipCode;
                if (bulkPlantdetails.Latitude == 0 || bulkPlantdetails.Longitude == 0)
                    GetAdrressByZipCode(record, lineNumberOfCSV, errorList);
                else
                {
                    record.LiftAddressLat = bulkPlantdetails.Latitude.ToString();
                    record.LiftAddressLong = bulkPlantdetails.Longitude.ToString();
                }    
            }

            if (!isExistingBulkPlant)
            {
                if (string.IsNullOrWhiteSpace(record.LiftAddressLat) && string.IsNullOrWhiteSpace(record.LiftAddressLong))
                {
                    if(!string.IsNullOrWhiteSpace(record.LiftAddressZip))
                    {
                        GetAdrressByZipCode(record, lineNumberOfCSV, errorList);
                    }
                    if (string.IsNullOrWhiteSpace(record.LiftAddressStreet1))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressStreet1), lineNumberOfCSV));                    

                    if (string.IsNullOrWhiteSpace(record.LiftAddressCity))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressCity), lineNumberOfCSV));                  

                    if (string.IsNullOrWhiteSpace(record.LiftAddressZip))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressZip), lineNumberOfCSV));                   

                    if (string.IsNullOrWhiteSpace(record.LiftAddressState))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressState), lineNumberOfCSV));
                    
                    else
                    {
                        if (!allStates.Any(t => t == record.LiftAddressState.ToLower()))
                            errorList.AppendLine(string.Format(Resource.errorMsgParameterFormatIsInvalid, nameof(record.LiftAddressState), lineNumberOfCSV));
                    }
                }
                else
                {
                    var isLatParse = Double.TryParse(record.LiftAddressLat, out double lat);
                    var isLongParse = Double.TryParse(record.LiftAddressLong, out double lang);
                    if (string.IsNullOrWhiteSpace(record.LiftAddressLat) || lat == 0)
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLat), lineNumberOfCSV));                   

                    if (string.IsNullOrWhiteSpace(record.LiftAddressLong) || lang == 0)
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLong), lineNumberOfCSV));

                    if (lat != 0 || lang != 0)
                        GetAdrressByLatLong(record, lineNumberOfCSV, errorList, allStates);
                }
                if(errorList.Length <= 0)
                {
                    PickupLocationDetailViewModel pickupLocationDetailViewModel = new PickupLocationDetailViewModel();
                    pickupLocationDetailViewModel.Name = record.BulkPlantName;
                    pickupLocationDetailViewModel.StateCode = record.LiftAddressState;
                    var stateDetails = Context.DataContext.MstStates.Where(x => x.Code == pickupLocationDetailViewModel.StateCode).FirstOrDefault();
                    if (stateDetails != null)
                    {
                        pickupLocationDetailViewModel.CountryId = stateDetails.CountryId;
                        pickupLocationDetailViewModel.StateId = stateDetails.Id;
                        pickupLocationDetailViewModel.CountryCode = stateDetails.MstCountry.Code;
                        pickupLocationDetailViewModel.County = stateDetails.MstCountry.Name;
                    }
                    pickupLocationDetailViewModel.City = record.LiftAddressCity;
                    pickupLocationDetailViewModel.ZipCode = record.LiftAddressZip;
                    pickupLocationDetailViewModel.Latitude = Convert.ToDecimal(record.LiftAddressLat);
                    pickupLocationDetailViewModel.Longitude = Convert.ToDecimal(record.LiftAddressLong);
                    pickupLocationDetailViewModel.Address = record.LiftAddressStreet1 + ' ' + record.LiftAddressStreet2;
                    var bulkPLantLocation = pickupLocationDetailViewModel.ToBulkPlantLocationEntity(userContext);
                    Context.DataContext.BulkPlantLocations.Add(bulkPLantLocation);
                    Context.Commit();
                }               
            }
        }
        private void ValidateLiftAddress(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(record.LiftTicketNumber) || !string.IsNullOrWhiteSpace(record.LiftQuantity))
            {
                if (string.IsNullOrWhiteSpace(record.LiftAddressLat) && string.IsNullOrWhiteSpace(record.LiftAddressLong) && string.IsNullOrWhiteSpace(record.LiftAddressZip))
                {
                    errorList.AppendLine(string.Format(Resource.errTPDLiftLatLongZipRequired, lineNumberOfCSV));
                }
                if (!string.IsNullOrWhiteSpace(record.LiftAddressZip))
                {
                    GetAdrressByZipCode(record, lineNumberOfCSV, errorList);

                    if (string.IsNullOrWhiteSpace(record.LiftAddressStreet1))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressStreet1), lineNumberOfCSV));

                    if (string.IsNullOrWhiteSpace(record.LiftAddressCity))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressCity), lineNumberOfCSV));

                    if (string.IsNullOrWhiteSpace(record.LiftAddressState))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressState), lineNumberOfCSV));
                    else
                    {
                        if (!allStates.Contains(record.LiftAddressState.ToLower()))
                            errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.LiftAddressState), lineNumberOfCSV));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) || !string.IsNullOrWhiteSpace(record.LiftAddressLong))
                {
                    if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) && string.IsNullOrWhiteSpace(record.LiftAddressLong))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLong), lineNumberOfCSV));

                    if (!string.IsNullOrWhiteSpace(record.LiftAddressLong) && string.IsNullOrWhiteSpace(record.LiftAddressLat))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLat), lineNumberOfCSV));
                }
                if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) && !string.IsNullOrWhiteSpace(record.LiftAddressLong))
                {
                    var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(record.LiftAddressLat), Convert.ToDouble(record.LiftAddressLong));
                    if (geoAddress == null)
                    {
                        errorList.AppendLine(string.Format(Resource.errTPDIncorrectLatLong, lineNumberOfCSV));
                    }
                }
                if (!string.IsNullOrWhiteSpace(record.TerminalControlNumber))
                {
                    var terminal = GetTerminalIdFromControlNumber(record.TerminalControlNumber, userContext);

                    if (terminal == 0)
                        errorList.AppendLine(string.Format("{0} TerminalControlNumber is invalid at line {1}", nameof(record.TerminalControlNumber), lineNumberOfCSV));
                }
            }
        }
        private void ValidateLiftTicketParameterValues(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, List<string> allStates)
        {
            if (!string.IsNullOrWhiteSpace(record.LiftTicketNumber))
            {
                if (!string.IsNullOrWhiteSpace(record.LiftQuantity))
                    ValidateDecimalParameter(nameof(record.LiftQuantity), record.LiftQuantity, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftGross))
                    ValidateDecimalParameter(nameof(record.LiftGross), record.LiftGross, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftNet))
                    ValidateDecimalParameter(nameof(record.LiftNet), record.LiftNet, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftDate))
                    ValidateDateParameter(nameof(record.LiftDate), record.LiftDate, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftArrivalTime))
                    ValidateTimeParameter(nameof(record.LiftArrivalTime), record.LiftArrivalTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftStartTime))
                    ValidateTimeParameter(nameof(record.LiftStartTime), record.LiftStartTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftEndTime))
                    ValidateTimeParameter(nameof(record.LiftEndTime), record.LiftEndTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftTicketCreationTime))
                    ValidateTimeParameter(nameof(record.LiftTicketCreationTime), record.LiftTicketCreationTime, lineNumberOfCSV, errorList);

                if (!string.IsNullOrWhiteSpace(record.LiftAddressState) && !allStates.Contains(record.LiftAddressState.ToLower()))
                {
                    errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.LiftAddressState), lineNumberOfCSV));
                }

                if (!string.IsNullOrWhiteSpace(record.LiftAddressZip))
                {
                    if (string.IsNullOrWhiteSpace(record.LiftAddressStreet1))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressStreet1), lineNumberOfCSV));

                    if (string.IsNullOrWhiteSpace(record.LiftAddressCity))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressCity), lineNumberOfCSV));

                    if (string.IsNullOrWhiteSpace(record.LiftAddressState))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressState), lineNumberOfCSV));
                    else
                    {
                        if (!allStates.Contains(record.LiftAddressState.ToLower()))
                            errorList.AppendLine(string.Format("{0} invalid state at line {1}", nameof(record.LiftAddressState), lineNumberOfCSV));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) || !string.IsNullOrWhiteSpace(record.LiftAddressLong))
                {
                    if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) && string.IsNullOrWhiteSpace(record.LiftAddressLong))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLong), lineNumberOfCSV));

                    if (!string.IsNullOrWhiteSpace(record.LiftAddressLong) && string.IsNullOrWhiteSpace(record.LiftAddressLat))
                        errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftAddressLat), lineNumberOfCSV));
                }
                if (!string.IsNullOrWhiteSpace(record.LiftAddressLat) && !string.IsNullOrWhiteSpace(record.LiftAddressLong))
                {
                    var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(record.LiftAddressLat), Convert.ToDouble(record.LiftAddressLong));
                    if (geoAddress == null)
                    {
                        errorList.AppendLine(string.Format(Resource.errTPDIncorrectLatLong, lineNumberOfCSV));
                    }
                }
            }
        }

        private void ValidateFTLParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList)
        {
            //if (!string.IsNullOrWhiteSpace(record.TerminalControlNumber))
            //{
            var isBolAvailable = false;
            var isLiftAvailable = false;

            if (!string.IsNullOrWhiteSpace(record.BolNumber) || !string.IsNullOrWhiteSpace(record.BolNet)
               || !string.IsNullOrWhiteSpace(record.BolGross))
            {
                isBolAvailable = true;

                if (string.IsNullOrWhiteSpace(record.BolNumber))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.BolNumber), lineNumberOfCSV));
                }

                if (string.IsNullOrWhiteSpace(record.BolNet))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.BolNet), lineNumberOfCSV));
                }
                else
                    ValidateDecimalParameter(nameof(record.BolNet), record.BolNet, lineNumberOfCSV, errorList);

                if (string.IsNullOrWhiteSpace(record.BolGross))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.BolGross), lineNumberOfCSV));
                }
                else
                    ValidateDecimalParameter(nameof(record.BolGross), record.BolGross, lineNumberOfCSV, errorList);

                if (string.IsNullOrWhiteSpace(record.BolCarrier))
                {
                    //errorList.Append("</br>");
                    //errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.BolCarrier), lineNumberOfCSV));
                }

                if (string.IsNullOrWhiteSpace(record.BolCreationTime))
                {
                    //errorList.Append("</br>");
                    //errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.BolCreationTime), lineNumberOfCSV));
                }
                else
                    ValidateTimeParameter(nameof(record.BolCreationTime), record.BolCreationTime, lineNumberOfCSV, errorList);

            }

            if (!string.IsNullOrWhiteSpace(record.LiftTicketNumber) || !string.IsNullOrWhiteSpace(record.LiftQuantity))
            {
                isLiftAvailable = true;
                if (string.IsNullOrWhiteSpace(record.LiftTicketNumber))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftTicketNumber), lineNumberOfCSV));
                }

                if (string.IsNullOrWhiteSpace(record.LiftNet) && string.IsNullOrWhiteSpace(record.LiftGross))
                {
                    if (string.IsNullOrWhiteSpace(record.LiftQuantity))
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format(Resource.errTPDFileLiftQuantityRequired, lineNumberOfCSV));
                    }
                    else
                        ValidateDecimalParameter(nameof(record.LiftQuantity), record.LiftQuantity, lineNumberOfCSV, errorList);
                }
                else if (string.IsNullOrWhiteSpace(record.LiftGross))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftGross), lineNumberOfCSV));
                }
                else if (string.IsNullOrWhiteSpace(record.LiftNet))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftNet), lineNumberOfCSV));
                }
                else
                {
                    ValidateDecimalParameter(nameof(record.LiftGross), record.LiftGross, lineNumberOfCSV, errorList);
                    ValidateDecimalParameter(nameof(record.LiftNet), record.LiftNet, lineNumberOfCSV, errorList);
                }

                if (string.IsNullOrWhiteSpace(record.LiftTicketCreationTime))
                {
                    //errorList.Append("</br>");
                    //errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftTicketCreationTime), lineNumberOfCSV));
                }
                else
                    ValidateTimeParameter(nameof(record.LiftTicketCreationTime), record.LiftTicketCreationTime, lineNumberOfCSV, errorList);

                if (string.IsNullOrWhiteSpace(record.LiftDate))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LiftDate), lineNumberOfCSV));
                }
                else
                    ValidateCompareDateParameters(nameof(record.LiftDate), nameof(record.Drop1ArrivalDate), record.LiftDate, record.Drop1ArrivalDate, lineNumberOfCSV, errorList);

            }

            if (!isBolAvailable && !isLiftAvailable)
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDBolLiftRequired, lineNumberOfCSV));
            }
        }
        private void ValidateCompareDateParameters(string dateparameterName1, string dateParameterName2, string date1, string date2, int lineNumberOfCSV, StringBuilder errorList)
        {
            var isDate1Parsed = true;
            var isDate2Parsed = true;
            if (!DateTime.TryParse(date1, out DateTime dateTime1))
            {
                isDate1Parsed = false;
                errorList.AppendLine(string.Format("</br>{0} invalid format at line {1}", dateparameterName1, lineNumberOfCSV));
            }
            if (!DateTime.TryParse(date2, out DateTime dateTime2))
            {
                isDate2Parsed = false;
                errorList.AppendLine(string.Format("</br>{0} invalid format at line {1}", dateParameterName2, lineNumberOfCSV));
            }
            if (isDate1Parsed && isDate2Parsed)
            {
                if (dateTime1 > dateTime2)
                {
                    errorList.AppendLine(string.Format("</br>{0} must be less than or equal to drop arrival date at line {1}", dateparameterName1, lineNumberOfCSV));
                }
            }
        }
        private void ValidateRequiredParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, System.Text.StringBuilder errorList, DateTimeOffset acceptedDate, int orderId = 0, int companyId = 0)
        {
            if (string.IsNullOrWhiteSpace(record.PONumber) && string.IsNullOrWhiteSpace(record.LocationId) && string.IsNullOrWhiteSpace(record.Drop1ProductId))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.PONumber), lineNumberOfCSV));
            }

            ValidateDrop1Parameters(record, lineNumberOfCSV, errorList, acceptedDate);
            if (!string.IsNullOrWhiteSpace(record.BolNumber) || !string.IsNullOrWhiteSpace(record.LiftTicketNumber))
            {
                var invoiceCommonDomain = new InvoiceCommonDomain(this);
                if (string.IsNullOrWhiteSpace(record.LoadingBadge) && invoiceCommonDomain.IsBadgeNumberMandatory(orderId, companyId))
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.LoadingBadge), lineNumberOfCSV));
                }
            }
        }

        private void ValidateDrop1DryRunParameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset acceptedDate)
        {
            if (string.IsNullOrWhiteSpace(record.Drop1ArrivalDate))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1ArrivalDate), lineNumberOfCSV));
            }
            else
            {
                ValidateDateParameter(nameof(record.Drop1ArrivalDate), record.Drop1ArrivalDate, lineNumberOfCSV, errorList, acceptedDate.Date);
            }

            if (string.IsNullOrWhiteSpace(record.Drop1ArrivalTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1ArrivalTime), lineNumberOfCSV));
            }
            else
                ValidateTimeParameter(nameof(record.Drop1ArrivalTime), record.Drop1ArrivalTime, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop1DryRunCount))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1DryRunCount), lineNumberOfCSV));
            }
            else
                ValidateIntParameter(nameof(record.Drop1DryRunCount), record.Drop1DryRunCount, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop1DryRunFees))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1DryRunFees), lineNumberOfCSV));
            }
            else
                ValidateDecimalParameter(nameof(record.Drop1DryRunFees), record.Drop1DryRunFees, lineNumberOfCSV, errorList);
        }

        private bool CheckIfDryRunInvoice(InvoiceBulkCsvViewModel record)
        {
            return !string.IsNullOrWhiteSpace(record.Drop1ArrivalDate) && !string.IsNullOrWhiteSpace(record.Drop1ArrivalTime)
                && !string.IsNullOrWhiteSpace(record.Drop1DryRunCount) && !string.IsNullOrWhiteSpace(record.Drop1DryRunFees)
                && string.IsNullOrWhiteSpace(record.Drop1Quantity);
        }

        private void ValidateDrop1Parameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset acceptedDate)
        {
            if (string.IsNullOrWhiteSpace(record.Drop1ArrivalDate))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1ArrivalDate), lineNumberOfCSV));
            }
            else
            {
                ValidateDateParameter(nameof(record.Drop1ArrivalDate), record.Drop1ArrivalDate, lineNumberOfCSV, errorList, acceptedDate.Date);
            }

            if (string.IsNullOrWhiteSpace(record.Drop1ArrivalTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1ArrivalTime), lineNumberOfCSV));
            }
            else
                ValidateTimeParameter(nameof(record.Drop1ArrivalTime), record.Drop1ArrivalTime, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop1CompleteTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1CompleteTime), lineNumberOfCSV));
            }
            else
            {
                ValidateTimeParameter(nameof(record.Drop1CompleteTime), record.Drop1CompleteTime, lineNumberOfCSV, errorList);
                //ValidateStartAndEndTimeParameter(record.Drop1ArrivalTime, record.Drop1CompleteTime, nameof(record.Drop1CompleteTime), lineNumberOfCSV, errorList);
            }

            if (string.IsNullOrWhiteSpace(record.Drop1Quantity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1Quantity), lineNumberOfCSV));
            }
            else
                ValidateDecimalParameter(nameof(record.Drop1Quantity), record.Drop1Quantity, lineNumberOfCSV, errorList);

            if (!string.IsNullOrWhiteSpace(record.Drop1AssetPreDip))
                ValidateDecimalParameter(nameof(record.Drop1AssetPreDip), record.Drop1AssetPreDip, lineNumberOfCSV, errorList);

            if (!string.IsNullOrWhiteSpace(record.Drop1AssetPostDip))
                ValidateDecimalParameter(nameof(record.Drop1AssetPostDip), record.Drop1AssetPostDip, lineNumberOfCSV, errorList);

            if (!string.IsNullOrWhiteSpace(record.Drop1AssetPreDip) && string.IsNullOrWhiteSpace(record.Drop1AssetPostDip))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AssetPostDip), lineNumberOfCSV));
            }

            if (string.IsNullOrWhiteSpace(record.Drop1AssetPreDip) && !string.IsNullOrWhiteSpace(record.Drop1AssetPostDip))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop1AssetPreDip), lineNumberOfCSV));
            }
        }

        private void ValidateDrop2Parameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset acceptedDate, bool fobVarious)
        {
            if (string.IsNullOrWhiteSpace(record.Drop2ArrivalDate))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2ArrivalDate), lineNumberOfCSV));
            }
            else
            {
                ValidateDateParameter(nameof(record.Drop2ArrivalDate), record.Drop2ArrivalDate, lineNumberOfCSV, errorList, acceptedDate.Date);
            }

            if (string.IsNullOrWhiteSpace(record.Drop2ArrivalTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2ArrivalTime), lineNumberOfCSV));
            }
            else
                ValidateTimeParameter(nameof(record.Drop2ArrivalTime), record.Drop2ArrivalTime, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop2CompleteTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2CompleteTime), lineNumberOfCSV));
            }
            else
            {
                ValidateTimeParameter(nameof(record.Drop2CompleteTime), record.Drop2CompleteTime, lineNumberOfCSV, errorList);
                //ValidateStartAndEndTimeParameter(record.Drop2ArrivalTime, record.Drop2CompleteTime, nameof(record.Drop2CompleteTime), lineNumberOfCSV, errorList);
            }

            if (string.IsNullOrWhiteSpace(record.Drop2Quantity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop2Quantity), lineNumberOfCSV));
            }
            else
                ValidateDecimalParameter(nameof(record.Drop2Quantity), record.Drop2Quantity, lineNumberOfCSV, errorList);
        }

        private void ValidateDrop3Parameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset acceptedDate)
        {
            if (string.IsNullOrWhiteSpace(record.Drop3ArrivalDate))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3ArrivalTime), lineNumberOfCSV));
            }
            else
            {
                ValidateDateParameter(nameof(record.Drop3ArrivalDate), record.Drop3ArrivalDate, lineNumberOfCSV, errorList, acceptedDate.Date);
            }

            if (string.IsNullOrWhiteSpace(record.Drop3ArrivalTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3ArrivalTime), lineNumberOfCSV));
            }
            else
                ValidateTimeParameter(nameof(record.Drop3ArrivalTime), record.Drop3ArrivalTime, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop3CompleteTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3CompleteTime), lineNumberOfCSV));
            }
            else
            {
                ValidateTimeParameter(nameof(record.Drop3CompleteTime), record.Drop3CompleteTime, lineNumberOfCSV, errorList);
                //ValidateStartAndEndTimeParameter(record.Drop3ArrivalTime, record.Drop3CompleteTime, nameof(record.Drop3CompleteTime), lineNumberOfCSV, errorList);
            }

            if (string.IsNullOrWhiteSpace(record.Drop3Quantity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop3Quantity), lineNumberOfCSV));
            }
            else
                ValidateDecimalParameter(nameof(record.Drop3Quantity), record.Drop3Quantity, lineNumberOfCSV, errorList);
        }

        private void ValidateDrop4Parameters(InvoiceBulkCsvViewModel record, int lineNumberOfCSV, StringBuilder errorList, DateTimeOffset acceptedDate)
        {
            if (string.IsNullOrWhiteSpace(record.Drop4ArrivalDate))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4ArrivalTime), lineNumberOfCSV));
            }
            else
            {
                ValidateDateParameter(nameof(record.Drop4ArrivalDate), record.Drop4ArrivalDate, lineNumberOfCSV, errorList, acceptedDate.Date);
            }

            if (string.IsNullOrWhiteSpace(record.Drop4ArrivalTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4ArrivalTime), lineNumberOfCSV));
            }
            else
                ValidateTimeParameter(nameof(record.Drop4ArrivalTime), record.Drop4ArrivalTime, lineNumberOfCSV, errorList);

            if (string.IsNullOrWhiteSpace(record.Drop4CompleteTime))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4CompleteTime), lineNumberOfCSV));
            }
            else
            {
                ValidateTimeParameter(nameof(record.Drop4CompleteTime), record.Drop4CompleteTime, lineNumberOfCSV, errorList);
                //ValidateStartAndEndTimeParameter(record.Drop4ArrivalTime, record.Drop4CompleteTime, nameof(record.Drop4CompleteTime), lineNumberOfCSV, errorList);
            }

            if (string.IsNullOrWhiteSpace(record.Drop4Quantity))
            {
                errorList.Append("</br>");
                errorList.AppendLine(string.Format(Resource.errTPDFileRequiredField, nameof(record.Drop4Quantity), lineNumberOfCSV));
            }
            else
                ValidateDecimalParameter(nameof(record.Drop4Quantity), record.Drop4Quantity, lineNumberOfCSV, errorList);
        }

        private void ValidateDateParameter(string parameterName, string value, int lineNumberOfCSV, StringBuilder errorList, DateTime? dateToCheck = null)
        {
            if (!DateTime.TryParse(value, out DateTime dateTime))
            {
                errorList.AppendLine(string.Format("</br>{0} invalid format at line {1}", parameterName, lineNumberOfCSV));
            }

            if (dateToCheck != null)
            {
                if (dateTime < dateToCheck.Value)
                    errorList.AppendLine(string.Format("</br>{0} can not be less than Order start date at line {1}", parameterName, lineNumberOfCSV));

                if (dateTime > DateTime.Now.Date)
                    errorList.AppendLine(string.Format("</br>{0} can not be greater than current date at line {1}", parameterName, lineNumberOfCSV));
            }
        }

        private void ValidateIntParameter(string parameterName, string value, int lineNumberOfCSV, StringBuilder errorList)
        {
            int.TryParse(value, out int parameterValue);
            if (parameterValue <= 0)
            {
                errorList.AppendLine(string.Format("</br>{0} invalid format at line {1}", parameterName, lineNumberOfCSV));
            }
        }

        private void ValidateDecimalParameter(string parameterName, string value, int lineNumberOfCSV, StringBuilder errorList)
        {
            decimal.TryParse(value, out decimal parameterValue);
            if (parameterValue <= 0)
            {
                errorList.AppendLine(string.Format("</br>{0} invalid format at line {1}", parameterName, lineNumberOfCSV));
            }
        }

        private void ValidateTimeParameter(string parameterName, string value, int lineNumberOfCSV, StringBuilder errorList)
        {
            var time = value.ToLower().Replace("am", string.Empty).Replace("pm", string.Empty);
            bool isSucess = TimeSpan.TryParse(time, out TimeSpan result);
            if (!isSucess)
            {
                errorList.AppendLine(string.Format("</br>{0} invalid time format at line {1}", parameterName, lineNumberOfCSV));
            }
        }

        private void ValidateStartAndEndTimeParameter(string startTime, string endTime, string endTimeParameterName, int lineNumberOfCSV, StringBuilder errorList)
        {
            var starttimespan = GetTimeSpan(startTime);
            var endtimespan = GetTimeSpan(endTime);

            if (starttimespan.HasValue && endtimespan.HasValue)
            {
                if (starttimespan.Value.Hours > endtimespan.Value.Hours)
                {
                    errorList.AppendLine(string.Format("</br>{0} should be greater than Arrival time at line {1}", endTimeParameterName, lineNumberOfCSV));
                }
            }
        }

        private bool IsEndTimeLessThanStartTime(string startTime, string endTime)
        {
            var starttimespan = GetTimeSpan(startTime);
            var endtimespan = GetTimeSpan(endTime);

            if (starttimespan.HasValue && endtimespan.HasValue)
            {
                if (starttimespan.Value.Hours > endtimespan.Value.Hours)
                    return true;
            }

            return false;
        }

        private string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');

            return csvText;
        }

        public async Task<StatusViewModel> UploadFileToBlob(UserContext userContext, Stream fileStream, string fileName)
        {
            using (var tracer = new Tracer("InvoiceBulkUploadDomain", "UploadFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.Id), BlobContainerType.InvoiceBulkUpload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetQueueEventForInvoiceBulkUpload(userContext, filePath);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.SFXOrderBulkUploadSuffix, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "UploadFileToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.InvoiceBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        private QueueMessageViewModel GetQueueEventForInvoiceBulkUpload(UserContext userContext, string blobStoragePath)
        {
            var jsonViewModel = new InvoiceBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.InvoiceBulkUpload,
                JsonMessage = json
            };
        }

        #region Invoice Processing
        public string ProcessBulkUploadJsonMessage(InvoiceBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessBulkUploadJsonMessage"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        var azureBlob = new AzureBlobStorage();
                        var fileStream = azureBlob.DownloadBlob(bulkRequestViewModel.FileUploadPath, BlobContainerType.InvoiceBulkUpload.ToString().ToLower());
                        if (fileStream != null)
                        {
                            string csvText = new StreamReader(fileStream).ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(csvText))
                            {
                                var filteredCsvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                                var engine = new FileHelperEngine<InvoiceBulkCsvViewModel>();
                                var csvInvoiceList = engine.ReadString(filteredCsvText).ToList();

                                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.SupplierId, CompanyType.Supplier).Result;
                                List<List<ManualInvoiceViewModel>> splitInvoiceList = new List<List<ManualInvoiceViewModel>>();
                                List<InvoiceViewModelNew> invoiceList = new List<InvoiceViewModelNew>();
                                List<DryRunInvoiceViewModel> dryRunList = new List<DryRunInvoiceViewModel>();

                                InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain(authenticationDomain);

                                foreach (var item in csvInvoiceList)
                                {
                                    if (string.IsNullOrWhiteSpace(item.PONumber))
                                    {
                                        var suppliercompanyForCarrier = 0;
                                        suppliercompanyForCarrier = GetSupplierCompanyIdForCarrier(item, new StringBuilder(), suppliercompanyForCarrier, context);

                                        string poFromLocation = item.PONumber;
                                        GetOrderIdFromLocAndProduct(context, item.Drop1ProductId, item.LocationId, suppliercompanyForCarrier, ref poFromLocation);
                                        item.PONumber = poFromLocation;
                                    }

                                    item.PONumber = item.PONumber.Trim();
                                    item.Drop1TicketNumber = item.Drop1TicketNumber.Trim();
                                }

                                //var invoicesList = GetManualInvoiceViewModelList(csvInvoiceList, bulkRequestViewModel, context, invoiceList, splitInvoiceList, dryRunList);
                                GetManualInvoiceViewModelNewList(csvInvoiceList, bulkRequestViewModel, context, invoiceList, splitInvoiceList, dryRunList);
                                processMessage.Clear();

                                ProcessDryRunInvoiceList(errorInfo, processMessage, invoiceCreateDomain, context, dryRunList);
                                //ProcessManualInvoiceList(errorInfo, processMessage, invoiceCreateDomain, context, invoiceList);
                                ProcessManualInvoiceNewList(errorInfo, processMessage, context, invoiceList);
                                ProcessSplitInvoiceList(errorInfo, processMessage, context, splitInvoiceList, invoiceCreateDomain);
                                errorInfo.Add(SetTotalNumberOfProcessedRows(csvInvoiceList.Count));
                            }
                            else
                            {
                                processMessage.Append(Resource.errMessageFailedToReadFileContent);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessBulkUploadJsonMessage", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }

        private void ProcessDryRunInvoiceList(List<string> errorInfo, StringBuilder processMessage, InvoiceCreateDomain invoiceCreateDomain, UserContext context, List<DryRunInvoiceViewModel> dryRunList)
        {
            var invoiceDomain = new InvoiceDomain(invoiceCreateDomain);
            foreach (var item in dryRunList)
            {
                try
                {
                    StatusViewModel result = invoiceDomain.CreateDryRunInvoiceAsync(item).Result;
                    if (result.StatusCode == Status.Success)
                        errorInfo.Add(SetSuccessProcessMessage(item.PoNumber));
                    else
                    {
                        SetFailedProcessMessage(processMessage, item.PoNumber, result.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                        throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        SetFailedProcessMessage(processMessage, item.PoNumber, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessDryRunInvoiceList", "TPD Invoicing Dry run processing failed", ex);
                }
            }
        }

        private void ProcessSplitInvoiceList(List<string> errorInfo, StringBuilder processMessage, UserContext context, List<List<ManualInvoiceViewModel>> splitInvoiceList, InvoiceCreateDomain invoiceCreateDomain)
        {
            foreach (var splitInvoice in splitInvoiceList)
            {
                try
                {
                    StatusViewModel result = CreateSplitInvoices(splitInvoice, invoiceCreateDomain, context, errorInfo, processMessage).Result;
                    if (result.StatusCode == Status.Success)
                    {
                        var item = splitInvoice.First();
                        errorInfo.Add(SetSuccessProcessMessage(item.PoNumber));
                    }
                    else
                    {
                        var item = splitInvoice.First();
                        SetFailedProcessMessage(processMessage, item.PoNumber, result.StatusMessage);

                        errorInfo.Add(processMessage.ToString());
                        throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        var item = splitInvoice.First();
                        SetFailedProcessMessage(processMessage, item.PoNumber, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessSplitInvoiceList", "TPD Invoicing split load processing failed", ex);
                }
            }
        }

        private async Task<StatusViewModel> CreateSplitInvoices(List<ManualInvoiceViewModel> splitInvoicesList, InvoiceCreateDomain invoiceCreateDomain, UserContext context, List<string> errorInfo, StringBuilder processMessage)
        {
            string splitLoadChainId = string.Empty;
            int ftlDetailId = 0, sequence = 0;
            foreach (var splitInvoiceManualViewModel in splitInvoicesList)
            {
                splitInvoiceManualViewModel.StatusId = (int)InvoiceStatus.Draft;
                splitInvoiceManualViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                splitInvoiceManualViewModel.BolDetails.Id = ftlDetailId;
                splitInvoiceManualViewModel.SplitLoadChainId = splitLoadChainId;
                splitInvoiceManualViewModel.SplitLoadSequence = sequence;
                var draftDdtSaveOutPut = invoiceCreateDomain.CreateSplitLoadDraftDdtAsync(context, splitInvoiceManualViewModel).Result;
                if (draftDdtSaveOutPut.StatusCode == Status.Failed)
                {
                    SetFailedProcessMessage(processMessage, splitInvoiceManualViewModel.PoNumber, draftDdtSaveOutPut.StatusMessage);
                }
                splitLoadChainId = draftDdtSaveOutPut.SplitLoadChainId;
                ftlDetailId = draftDdtSaveOutPut.BolDetailId ?? 0;
                sequence++;
            }

            var result = await ContextFactory.Current.GetDomain<SplitLoadInvoiceDomain>().CreateInvoicesFromSplitLoadDraftDdts(context, splitLoadChainId);
            return result;
        }

        private void ProcessManualInvoiceList(List<string> errorInfo, StringBuilder processMessage, InvoiceCreateDomain invoiceCreateDomain, UserContext context, List<ManualInvoiceViewModel> invoiceList)
        {
            foreach (var item in invoiceList)
            {
                try
                {
                    StatusViewModel result = CreateInvoice(item, invoiceCreateDomain, context);
                    if (result.StatusCode == Status.Success)
                        errorInfo.Add(SetSuccessProcessMessage(item.PoNumber, result));
                    else
                    {
                        SetFailedProcessMessage(processMessage, item.PoNumber, result.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                        throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        SetFailedProcessMessage(processMessage, item.PoNumber, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessManualInvoiceList", "TPD Invoice processing failed", ex);
                }
            }
        }

        private void ProcessManualInvoiceNewList(List<string> errorInfo, StringBuilder processMessage, UserContext context, List<InvoiceViewModelNew> invoiceList)
        {
            var invoiceCommonDomain = new InvoiceCommonDomain(this);
            foreach (var item in invoiceList)
            {
                string poNumbers = string.Join(", ", item.Drops.Select(t => t.PoNumber).ToList());
                try
                {
                    Thread.Sleep(300);
                    StatusViewModel result = invoiceCommonDomain.AddCreateInvioceToQueue(context, item).Result;
                    //StatusViewModel result = CreateInvoice(item, invoiceCreateDomain, context);
                    if (result.StatusCode == Status.Success)
                        errorInfo.Add(SetSuccessProcessMessage(poNumbers));
                    else
                    {
                        SetFailedProcessMessage(processMessage, poNumbers, result.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                        throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        SetFailedProcessMessage(processMessage, poNumbers, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessManualInvoiceNewList", "TPD Invoice processing failed", ex);
                }
            }
        }
        #region newSetMessages
        private static string SetSuccessProcessMessage(string poNumber)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Invoice/DDT created successfully for </b>")
                            .Append($"PO#: {poNumber} </p><br>");
            return processMessage.ToString();
        }

        private static string SetTotalNumberOfProcessedRows(int rowCount)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append($"<b>Total row processed: {rowCount} </b><br>");
            return processMessage.ToString();
        }

        #endregion

        private static void SetFailedProcessMessage(StringBuilder processMessage, string poNumber, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Invoice/DDT Info: </b>")
                            .Append($"PO#: {poNumber} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        private static string SetSuccessProcessMessage(string poNumber, StatusViewModel result)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Invoice/DDT Info: </b>")
                            .Append($"Number(s): {result.EntityNumber}, PO#: {poNumber} <br><b>Invoice/DDT created successfully</b></p><br>");
            return processMessage.ToString();
        }

        private static string SetInvoiceSuccessProcessMessage(InvoiceDetailViewModel item, StatusViewModel result)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Invoice Info: </b>")
                            .Append($"Number(s): {result.EntityNumber}, PO#: {item.PoNumber} <br><b>Invoice created successfully</b></p><br>");
            return processMessage.ToString();
        }

        private static void SetInvoiceFailedProcessMessage(StringBuilder processMessage, string displayInvNumber, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Invoice Info: </b>")
                            .Append($"DDT#: {displayInvNumber} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        private StatusViewModel CreateInvoice(ManualInvoiceViewModel viewModel, InvoiceCreateDomain invoiceCreateDomain, UserContext context)
        {

            if (viewModel.IsFTL)
            {
                return invoiceCreateDomain.CreateManualFtlInvoiceAsync(context, viewModel).Result;
            }
            else
            {
                return invoiceCreateDomain.CreateManualInvoiceAsync(context, viewModel).Result;
            }
        }

        private bool GetManualInvoiceViewModelList(List<InvoiceBulkCsvViewModel> csvInvoiceList, InvoiceBulkUploadProcessorReqViewModel bulkUploadMsg,
                                                    UserContext context, List<ManualInvoiceViewModel> manualInvoices, List<List<ManualInvoiceViewModel>> splitInvoices, List<DryRunInvoiceViewModel> dryRunList)
        {
            if (csvInvoiceList.Any())
            {
                List<InvoiceBulkViewModel> uploadedInvoices = GetInvoiceListToGenerate(csvInvoiceList, context);

                foreach (var csvRecord in uploadedInvoices)
                {
                    var order = Context.DataContext.Orders
                                .Where(t => t.PoNumber.ToLower().Equals(csvRecord.CsvViewModel.PONumber.ToLower().TrimEnd().TrimStart())
                                            && t.AcceptedCompanyId == bulkUploadMsg.SupplierCompanyId)
                                .Select(t => new { t.Id, t.IsActive })
                                .SingleOrDefault();
                    if (order != null && order.Id > 0)
                    {
                        //dry run invoice section
                        if (csvRecord.IsDryRunInvoice)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var dryRunInvoiceViewModel = invoiceDomain.GetDryRunInvoiceAsync(order.Id, context.Id).Result;
                            SetDryRunViewModel(dryRunInvoiceViewModel, csvRecord, context);
                            dryRunList.Add(dryRunInvoiceViewModel);
                        }
                        //add spilt invoice condition here
                        else if (csvRecord.IsSplitLoadInvoice)
                        {
                            List<ManualInvoiceViewModel> dropWiseSplitLoadInvoices = new List<ManualInvoiceViewModel>();
                            //get split loads from record
                            SetSplitInvoiceList(order.Id, csvRecord, context, dropWiseSplitLoadInvoices);
                            //add to splitinvoice list
                            //add list to split inovoices
                            splitInvoices.Add(dropWiseSplitLoadInvoices);
                        }
                        else
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var manualInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(order.Id).Result;

                            //set parameters from csv to manual view model
                            SetManualInvoiceViewModelFromCsvParameters(manualInvoiceViewModel, csvRecord, context);

                            manualInvoices.Add(manualInvoiceViewModel);
                        }
                    }
                }
            }
            return true;
        }

        private bool GetManualInvoiceViewModelNewList(List<InvoiceBulkCsvViewModel> csvInvoiceList, InvoiceBulkUploadProcessorReqViewModel bulkUploadMsg,
                                                   UserContext context, List<InvoiceViewModelNew> manualInvoices, List<List<ManualInvoiceViewModel>> splitInvoices, List<DryRunInvoiceViewModel> dryRunList)
        {
            if (csvInvoiceList.Any())
            {
                List<InvoiceBulkViewModel> uploadedInvoices = GetInvoiceListToGenerate(csvInvoiceList, context);

                var consolidatePoNumbers = GetConsolidatePoNumbers(csvInvoiceList, bulkUploadMsg.SupplierCompanyId);
                foreach (var jobDrop in consolidatePoNumbers)
                {
                    var csvRecord = uploadedInvoices.FirstOrDefault(t => t.CsvViewModel.PONumber == jobDrop.DropDetailsViewModel.FirstOrDefault().PONumber
                                                    && t.CsvViewModel.Drop1TicketNumber == jobDrop.DropDetailsViewModel.FirstOrDefault().DropTicketNumber);

                    var order = Context.DataContext.Orders.Where(t => t.PoNumber.ToLower().Equals(csvRecord.CsvViewModel.PONumber.ToLower().TrimEnd().TrimStart())
                                            && t.AcceptedCompanyId == bulkUploadMsg.SupplierCompanyId)
                                .Select(t => new { t.Id, t.IsActive, t.FuelRequest.FuelRequestFees, t.FuelRequest.FuelRequestDetail.OrderEnforcementId }).SingleOrDefault();
                    if (order != null && order.Id > 0)
                    {
                        //dry run invoice section
                        if (csvRecord.IsDryRunInvoice)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var dryRunInvoiceViewModel = invoiceDomain.GetDryRunInvoiceAsync(order.Id, context.Id).Result;
                            SetDryRunViewModel(dryRunInvoiceViewModel, csvRecord, context);
                            dryRunList.Add(dryRunInvoiceViewModel);
                        }
                        //add spilt invoice condition here
                        else if (csvRecord.IsSplitLoadInvoice)
                        {
                            List<ManualInvoiceViewModel> dropWiseSplitLoadInvoices = new List<ManualInvoiceViewModel>();
                            //get split loads from record
                            SetSplitInvoiceList(order.Id, csvRecord, context, dropWiseSplitLoadInvoices);
                            //add to splitinvoice list
                            //add list to split inovoices
                            splitInvoices.Add(dropWiseSplitLoadInvoices);
                        }
                        else
                        {
                            var invoiceCreate = new InvoiceCreateDomain();
                            InvoiceViewModelNew viewModel = invoiceCreate.GetPoDetailsToCreateInvoice(context, order.Id).Result;

                            //var invoiceDomain = new InvoiceDomain(this);
                            //var manualInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(order.Id).Result;
                            //set parameters from csv to manual view model
                            //SetManualInvoiceViewModelFromCsvParameters(manualInvoiceViewModel, csvRecord, context);
                            //manualInvoices.Add(manualInvoiceViewModel);

                            List<InvoiceBulkViewModel> lstConsolidatedRecord = new List<InvoiceBulkViewModel>();
                            foreach (var item in jobDrop.DropDetailsViewModel)
                            {
                                var csvConsolidatedRecord = uploadedInvoices.FirstOrDefault(t => item.PONumber.Contains(t.CsvViewModel.PONumber.Trim()) &&
                                            t.CsvViewModel.Drop1ArrivalDate == item.DropArrivalDate && t.CsvViewModel.Drop1ArrivalTime == item.DropArrivalTime);
                                lstConsolidatedRecord.Add(csvConsolidatedRecord);
                            }

                            SetInvoiceViewModelNewFromCsvParameters(viewModel, lstConsolidatedRecord, context, order.OrderEnforcementId);
                            manualInvoices.Add(viewModel);
                        }
                    }
                }
            }
            return true;
        }

        private List<ConsolidatedTpdInvoiceViewModel> GetConsolidatePoNumbers(List<InvoiceBulkCsvViewModel> csvInvoiceList, int supplierCompanyId)
        {
            var response = new List<ConsolidatedTpdInvoiceViewModel>();

            //Distinct PoNumbers
            var poNumbers = csvInvoiceList.Select(t => t.PONumber.ToLower().Trim()).Distinct().ToList();

            //JobId for above PONumber
            var orders = Context.DataContext.Orders.Where(t => poNumbers.Contains(t.PoNumber.ToLower())
                        && t.AcceptedCompanyId == supplierCompanyId)
                        .Select(t => new { t.FuelRequest.JobId, t.PoNumber }).ToList();

            var jobs = orders.Select(t => t.JobId).Distinct().ToList();

            foreach (var jobId in jobs)
            {
                var consolidatedTpdInvoiceViewModel = new ConsolidatedTpdInvoiceViewModel();
                consolidatedTpdInvoiceViewModel.JobId = jobId;

                //Jobwise Po Numbers
                var jobPoNumbers = orders.Where(t => t.JobId == jobId).Select(t => t.PoNumber).ToList();

                var sortedDropDetails = SortDropDetails(jobPoNumbers, csvInvoiceList, supplierCompanyId);

                List<ConsolidatedDropDetailsViewModel> lstConsolidatedDropDetailsViewModel = new List<ConsolidatedDropDetailsViewModel>();
                for (int i = 0; i < sortedDropDetails.Count; i++)
                {
                    if (sortedDropDetails.Count == 1 || i == sortedDropDetails.Count - 1)
                    {
                        //For Single Record and Last Record
                        var consolidatedDropDetailsViewModel = AddDropDetails(sortedDropDetails[i]);
                        lstConsolidatedDropDetailsViewModel.Add(consolidatedDropDetailsViewModel);
                        consolidatedTpdInvoiceViewModel.DropDetailsViewModel = lstConsolidatedDropDetailsViewModel;
                        response.Add(consolidatedTpdInvoiceViewModel);
                    }
                    else
                    {
                        TimeSpan diff = sortedDropDetails[i + 1].DropArrivalDateTime - sortedDropDetails[i].DropArrivalDateTime;
                        if ((diff.TotalHours < 6 && sortedDropDetails[i + 1].FuelTypeId != sortedDropDetails[i].FuelTypeId) || diff.TotalMinutes == 0)
                        {
                            var consolidatedDropDetailsViewModel = AddDropDetails(sortedDropDetails[i]);
                            lstConsolidatedDropDetailsViewModel.Add(consolidatedDropDetailsViewModel);
                        }
                        else
                        {
                            var consolidatedDropDetailsViewModel = AddDropDetails(sortedDropDetails[i]);
                            lstConsolidatedDropDetailsViewModel.Add(consolidatedDropDetailsViewModel);
                            consolidatedTpdInvoiceViewModel.DropDetailsViewModel = lstConsolidatedDropDetailsViewModel;
                            response.Add(consolidatedTpdInvoiceViewModel);

                            consolidatedTpdInvoiceViewModel = new ConsolidatedTpdInvoiceViewModel();
                            consolidatedTpdInvoiceViewModel.JobId = jobId;
                            lstConsolidatedDropDetailsViewModel = new List<ConsolidatedDropDetailsViewModel>();
                        }
                    }
                }
            }
            return response;
        }

        private List<ConsolidatedDropDetailsViewModel> SortDropDetails(List<string> jobPoNumbers, List<InvoiceBulkCsvViewModel> csvInvoiceList, int supplierCompanyId)
        {
            var orders = Context.DataContext.Orders.Where(t => jobPoNumbers.Contains(t.PoNumber.ToLower())
                        && t.AcceptedCompanyId == supplierCompanyId)
                        .Select(t => new { t.PoNumber, t.FuelRequest.FuelTypeId, t.FuelRequest.FuelRequestDetail.IsBolImageRequired }).ToList();

            var csvJobPoNumbers = csvInvoiceList.Where(t1 => jobPoNumbers.Contains(t1.PONumber.Trim())).ToList();
            var response = new List<ConsolidatedDropDetailsViewModel>();
            foreach (var item in csvJobPoNumbers)
            {
                if (!string.IsNullOrWhiteSpace(item.Drop1TicketNumber) && response.Any(t => t.DropTicketNumber.ToLower().Equals(item.Drop1TicketNumber.ToLower())))
                    continue;

                var consolidateddDropDetailsViewModel = new ConsolidatedDropDetailsViewModel();
                var dropDate = DateTime.Parse(item.Drop1ArrivalDate);

                var dropTime = DateTime.Parse(item.Drop1ArrivalTime).TimeOfDay;
                consolidateddDropDetailsViewModel.DropArrivalDateTime = dropDate.Add(dropTime);
                consolidateddDropDetailsViewModel.DropArrivalDate = item.Drop1ArrivalDate;
                consolidateddDropDetailsViewModel.DropArrivalTime = item.Drop1ArrivalTime;
                consolidateddDropDetailsViewModel.PONumber = item.PONumber.Trim();
                consolidateddDropDetailsViewModel.DropTicketNumber = item.Drop1TicketNumber;
                var fuelType = orders.FirstOrDefault(t => t.PoNumber == item.PONumber.Trim());
                if (fuelType != null)
                {
                    consolidateddDropDetailsViewModel.FuelTypeId = fuelType.FuelTypeId;
                    consolidateddDropDetailsViewModel.IsBolImageRequired = fuelType.IsBolImageRequired;
                }
                response.Add(consolidateddDropDetailsViewModel);
            }
            return response.OrderBy(t => t.DropArrivalDateTime).ToList();
        }

        private ConsolidatedDropDetailsViewModel AddDropDetails(ConsolidatedDropDetailsViewModel item)
        {
            var consolidatedDropDetailsViewModel = new ConsolidatedDropDetailsViewModel();
            consolidatedDropDetailsViewModel.PONumber = item.PONumber.Trim();
            consolidatedDropDetailsViewModel.DropTicketNumber = item.DropTicketNumber;
            consolidatedDropDetailsViewModel.DropArrivalDate = item.DropArrivalDate;
            consolidatedDropDetailsViewModel.DropArrivalTime = item.DropArrivalTime;
            consolidatedDropDetailsViewModel.DropArrivalDateTime = item.DropArrivalDateTime;
            consolidatedDropDetailsViewModel.FuelTypeId = item.FuelTypeId;
            consolidatedDropDetailsViewModel.IsBolImageRequired = item.IsBolImageRequired;
            return consolidatedDropDetailsViewModel;
        }

        private void SetDryRunViewModel(DryRunInvoiceViewModel dryRunInvoiceViewModel, InvoiceBulkViewModel csvRecord, UserContext context)
        {
            if (dryRunInvoiceViewModel != null)
            {
                dryRunInvoiceViewModel.DryRunDate = csvRecord.CsvViewModel.Drop1ArrivalDate;
                dryRunInvoiceViewModel.DeliveryTime = csvRecord.CsvViewModel.Drop1ArrivalTime;
                dryRunInvoiceViewModel.UserId = context.Id;
                dryRunInvoiceViewModel.SupplierInvoiceNumber = csvRecord.CsvViewModel.SupplierInvoiceNumber;
                dryRunInvoiceViewModel.CreationMethod = CreationMethod.BulkUploaded;
                //total dry run invoice amount =  total count * fees
                if (dryRunInvoiceViewModel.OrderEnforcement != OrderEnforcement.EnforceOrderLevelValues)
                    dryRunInvoiceViewModel.DryRunFee = csvRecord.CsvViewModel.Drop1DryRunCount.GetValue<int>() * csvRecord.CsvViewModel.Drop1DryRunFees.GetValue<decimal>();
                else
                    dryRunInvoiceViewModel.DryRunFee = csvRecord.CsvViewModel.Drop1DryRunCount.GetValue<int>() * dryRunInvoiceViewModel.DryRunFee;
            }
        }

        private void SetSplitInvoiceList(int orderId, InvoiceBulkViewModel csvRecord, UserContext context, List<ManualInvoiceViewModel> splitLoadInvoices)
        {
            var invoiceDomain = new InvoiceDomain(this);
            //for drop1 split invoice
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop1Quantity))
            {
                var drop1SplitInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(orderId).Result;
                drop1SplitInvoiceViewModel.StatusId = (int)InvoiceStatus.Draft;
                drop1SplitInvoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                //set parameters from csv to manual view model                
                SetManualInvoiceViewModelFromCsvParameters(drop1SplitInvoiceViewModel, csvRecord, context);
                splitLoadInvoices.Add(drop1SplitInvoiceViewModel);
            }

            //for drop2 split invoice
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop2Quantity))
            {
                var drop2SplitInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(orderId).Result;
                drop2SplitInvoiceViewModel.StatusId = (int)InvoiceStatus.Draft;
                drop2SplitInvoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                //set parameters from csv to manual view model
                InvoiceBulkViewModel drop2CsvRecord = GetNewCsvRecordForDrop(csvRecord, 2);
                SetSpitInvoiceViewModelFromCsvParameters(drop2SplitInvoiceViewModel, drop2CsvRecord, context);
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.FuelCost))
                    drop2SplitInvoiceViewModel.FuelCost = csvRecord.CsvViewModel.FuelCost.GetValue<decimal>();
                splitLoadInvoices.Add(drop2SplitInvoiceViewModel);
            }

            //for drop2 split invoice
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop3Quantity))
            {
                var drop3SplitInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(orderId).Result;
                drop3SplitInvoiceViewModel.StatusId = (int)InvoiceStatus.Draft;
                drop3SplitInvoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                //set parameters from csv to manual view model
                InvoiceBulkViewModel drop3CsvRecord = GetNewCsvRecordForDrop(csvRecord, 3);
                SetSpitInvoiceViewModelFromCsvParameters(drop3SplitInvoiceViewModel, drop3CsvRecord, context);
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.FuelCost))
                    drop3SplitInvoiceViewModel.FuelCost = csvRecord.CsvViewModel.FuelCost.GetValue<decimal>();
                splitLoadInvoices.Add(drop3SplitInvoiceViewModel);
            }

            //for drop2 split invoice
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop4Quantity))
            {
                var drop4SplitInvoiceViewModel = invoiceDomain.GetManualInvoiceAsync(orderId).Result;
                drop4SplitInvoiceViewModel.StatusId = (int)InvoiceStatus.Draft;
                drop4SplitInvoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                //set parameters from csv to manual view model
                InvoiceBulkViewModel drop4CsvRecord = GetNewCsvRecordForDrop(csvRecord, 4);
                SetSpitInvoiceViewModelFromCsvParameters(drop4SplitInvoiceViewModel, drop4CsvRecord, context);
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.FuelCost))
                    drop4SplitInvoiceViewModel.FuelCost = csvRecord.CsvViewModel.FuelCost.GetValue<decimal>();
                splitLoadInvoices.Add(drop4SplitInvoiceViewModel);
            }
        }

        private InvoiceBulkViewModel GetNewCsvRecordForDrop(InvoiceBulkViewModel csvRecord, int dropNumber)
        {
            var newRecord = new InvoiceBulkViewModel();

            foreach (PropertyInfo property in typeof(InvoiceBulkViewModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(newRecord, property.GetValue(csvRecord, null), null);
                }
            }

            newRecord.CsvViewModel = new InvoiceBulkCsvViewModel();
            foreach (PropertyInfo property in typeof(InvoiceBulkCsvViewModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(newRecord.CsvViewModel, property.GetValue(csvRecord.CsvViewModel, null), null);
                }
            }

            switch (dropNumber)
            {
                case 2:
                    SetDrop1DetailsFromDrop2(csvRecord, newRecord);
                    break;
                case 3:
                    SetDrop1DetailsFromDrop3(csvRecord, newRecord);
                    break;
                case 4:
                    SetDrop1DetailsFromDrop4(csvRecord, newRecord);
                    break;

                default:
                    break;
            }

            return newRecord;
        }

        private static void SetDrop1DetailsFromDrop2(InvoiceBulkViewModel csvRecord, InvoiceBulkViewModel newRecord)
        {
            newRecord.CsvViewModel.Drop1AddressCity = csvRecord.CsvViewModel.Drop2AddressCity;
            newRecord.CsvViewModel.Drop1AddressLat = csvRecord.CsvViewModel.Drop2AddressLat;
            newRecord.CsvViewModel.Drop1AddressLong = csvRecord.CsvViewModel.Drop2AddressLong;
            newRecord.CsvViewModel.Drop1AddressState = csvRecord.CsvViewModel.Drop2AddressState;
            newRecord.CsvViewModel.Drop1AddressStreet1 = csvRecord.CsvViewModel.Drop2AddressStreet1;
            newRecord.CsvViewModel.Drop1AddressStreet2 = csvRecord.CsvViewModel.Drop2AddressStreet2;
            newRecord.CsvViewModel.Drop1AddressZip = csvRecord.CsvViewModel.Drop2AddressZip;
            newRecord.CsvViewModel.Drop1ArrivalDate = csvRecord.CsvViewModel.Drop2ArrivalDate;
            newRecord.CsvViewModel.Drop1ArrivalTime = csvRecord.CsvViewModel.Drop2ArrivalTime;
            newRecord.CsvViewModel.Drop1AssetId = csvRecord.CsvViewModel.Drop1AssetId;
            newRecord.CsvViewModel.Drop1CompleteTime = csvRecord.CsvViewModel.Drop2CompleteTime;
            newRecord.CsvViewModel.Drop1DemurrageFees = csvRecord.CsvViewModel.Drop2DemurrageFees;
            newRecord.CsvViewModel.Drop1DemurrageTime = csvRecord.CsvViewModel.Drop2DemurrageTime;
            newRecord.CsvViewModel.Drop1DryRunCount = csvRecord.CsvViewModel.Drop2DryRunCount;
            newRecord.CsvViewModel.Drop1DryRunFees = csvRecord.CsvViewModel.Drop2DryRunFees;
            newRecord.CsvViewModel.Drop1EnvironmentalFees = csvRecord.CsvViewModel.Drop2EnvironmentalFees;
            newRecord.CsvViewModel.Drop1FreightFees = csvRecord.CsvViewModel.Drop2FreightFees;
            newRecord.CsvViewModel.Drop1LoadFees = csvRecord.CsvViewModel.Drop2LoadFees;
            newRecord.CsvViewModel.Drop1Notes = csvRecord.CsvViewModel.Drop2Notes;
            newRecord.CsvViewModel.Drop1OtherFees = csvRecord.CsvViewModel.Drop2OtherFees;
            newRecord.CsvViewModel.Drop1OverWaterFees = csvRecord.CsvViewModel.Drop2OverWaterFees;
            newRecord.CsvViewModel.Drop1Quantity = csvRecord.CsvViewModel.Drop2Quantity;
            newRecord.CsvViewModel.Drop1ServiceFees = csvRecord.CsvViewModel.Drop2ServiceFees;
            newRecord.CsvViewModel.Drop1SurchargeFees = csvRecord.CsvViewModel.Drop2SurchargeFees;
            newRecord.CsvViewModel.Drop1TicketNumber = csvRecord.CsvViewModel.Drop2TicketNumber;
            newRecord.CsvViewModel.Drop1WethoseFees = csvRecord.CsvViewModel.Drop2WethoseFees;
            newRecord.CsvViewModel.Drop1ApiGravity = csvRecord.CsvViewModel.Drop2ApiGravity;
        }

        private static void SetDrop1DetailsFromDrop3(InvoiceBulkViewModel csvRecord, InvoiceBulkViewModel newRecord)
        {
            newRecord.CsvViewModel.Drop1AddressCity = csvRecord.CsvViewModel.Drop3AddressCity;
            newRecord.CsvViewModel.Drop1AddressLat = csvRecord.CsvViewModel.Drop3AddressLat;
            newRecord.CsvViewModel.Drop1AddressLong = csvRecord.CsvViewModel.Drop3AddressLong;
            newRecord.CsvViewModel.Drop1AddressState = csvRecord.CsvViewModel.Drop3AddressState;
            newRecord.CsvViewModel.Drop1AddressStreet1 = csvRecord.CsvViewModel.Drop3AddressStreet1;
            newRecord.CsvViewModel.Drop1AddressStreet2 = csvRecord.CsvViewModel.Drop3AddressStreet2;
            newRecord.CsvViewModel.Drop1AddressZip = csvRecord.CsvViewModel.Drop3AddressZip;
            newRecord.CsvViewModel.Drop1ArrivalDate = csvRecord.CsvViewModel.Drop3ArrivalDate;
            newRecord.CsvViewModel.Drop1ArrivalTime = csvRecord.CsvViewModel.Drop3ArrivalTime;
            newRecord.CsvViewModel.Drop1AssetId = csvRecord.CsvViewModel.Drop1AssetId;
            newRecord.CsvViewModel.Drop1CompleteTime = csvRecord.CsvViewModel.Drop3CompleteTime;
            newRecord.CsvViewModel.Drop1DemurrageFees = csvRecord.CsvViewModel.Drop3DemurrageFees;
            newRecord.CsvViewModel.Drop1DemurrageTime = csvRecord.CsvViewModel.Drop3DemurrageTime;
            newRecord.CsvViewModel.Drop1DryRunCount = csvRecord.CsvViewModel.Drop3DryRunCount;
            newRecord.CsvViewModel.Drop1DryRunFees = csvRecord.CsvViewModel.Drop3DryRunFees;
            newRecord.CsvViewModel.Drop1EnvironmentalFees = csvRecord.CsvViewModel.Drop3EnvironmentalFees;
            newRecord.CsvViewModel.Drop1FreightFees = csvRecord.CsvViewModel.Drop3FreightFees;
            newRecord.CsvViewModel.Drop1LoadFees = csvRecord.CsvViewModel.Drop3LoadFees;
            newRecord.CsvViewModel.Drop1Notes = csvRecord.CsvViewModel.Drop3Notes;
            newRecord.CsvViewModel.Drop1OtherFees = csvRecord.CsvViewModel.Drop3OtherFees;
            newRecord.CsvViewModel.Drop1OverWaterFees = csvRecord.CsvViewModel.Drop3OverWaterFees;
            newRecord.CsvViewModel.Drop1Quantity = csvRecord.CsvViewModel.Drop3Quantity;
            newRecord.CsvViewModel.Drop1ServiceFees = csvRecord.CsvViewModel.Drop3ServiceFees;
            newRecord.CsvViewModel.Drop1SurchargeFees = csvRecord.CsvViewModel.Drop3SurchargeFees;
            newRecord.CsvViewModel.Drop1TicketNumber = csvRecord.CsvViewModel.Drop3TicketNumber;
            newRecord.CsvViewModel.Drop1WethoseFees = csvRecord.CsvViewModel.Drop3WethoseFees;
            newRecord.CsvViewModel.Drop1ApiGravity = csvRecord.CsvViewModel.Drop3ApiGravity;
        }

        private static void SetDrop1DetailsFromDrop4(InvoiceBulkViewModel csvRecord, InvoiceBulkViewModel newRecord)
        {
            newRecord.CsvViewModel.Drop1AddressCity = csvRecord.CsvViewModel.Drop4AddressCity;
            newRecord.CsvViewModel.Drop1AddressLat = csvRecord.CsvViewModel.Drop4AddressLat;
            newRecord.CsvViewModel.Drop1AddressLong = csvRecord.CsvViewModel.Drop4AddressLong;
            newRecord.CsvViewModel.Drop1AddressState = csvRecord.CsvViewModel.Drop4AddressState;
            newRecord.CsvViewModel.Drop1AddressStreet1 = csvRecord.CsvViewModel.Drop4AddressStreet1;
            newRecord.CsvViewModel.Drop1AddressStreet2 = csvRecord.CsvViewModel.Drop4AddressStreet2;
            newRecord.CsvViewModel.Drop1AddressZip = csvRecord.CsvViewModel.Drop4AddressZip;
            newRecord.CsvViewModel.Drop1ArrivalDate = csvRecord.CsvViewModel.Drop4ArrivalDate;
            newRecord.CsvViewModel.Drop1ArrivalTime = csvRecord.CsvViewModel.Drop4ArrivalTime;
            newRecord.CsvViewModel.Drop1AssetId = csvRecord.CsvViewModel.Drop1AssetId;
            newRecord.CsvViewModel.Drop1CompleteTime = csvRecord.CsvViewModel.Drop4CompleteTime;
            newRecord.CsvViewModel.Drop1DemurrageFees = csvRecord.CsvViewModel.Drop4DemurrageFees;
            newRecord.CsvViewModel.Drop1DemurrageTime = csvRecord.CsvViewModel.Drop4DemurrageTime;
            newRecord.CsvViewModel.Drop1DryRunCount = csvRecord.CsvViewModel.Drop4DryRunCount;
            newRecord.CsvViewModel.Drop1DryRunFees = csvRecord.CsvViewModel.Drop4DryRunFees;
            newRecord.CsvViewModel.Drop1EnvironmentalFees = csvRecord.CsvViewModel.Drop4EnvironmentalFees;
            newRecord.CsvViewModel.Drop1FreightFees = csvRecord.CsvViewModel.Drop4FreightFees;
            newRecord.CsvViewModel.Drop1LoadFees = csvRecord.CsvViewModel.Drop4LoadFees;
            newRecord.CsvViewModel.Drop1Notes = csvRecord.CsvViewModel.Drop4Notes;
            newRecord.CsvViewModel.Drop1OtherFees = csvRecord.CsvViewModel.Drop4OtherFees;
            newRecord.CsvViewModel.Drop1OverWaterFees = csvRecord.CsvViewModel.Drop4OverWaterFees;
            newRecord.CsvViewModel.Drop1Quantity = csvRecord.CsvViewModel.Drop4Quantity;
            newRecord.CsvViewModel.Drop1ServiceFees = csvRecord.CsvViewModel.Drop4ServiceFees;
            newRecord.CsvViewModel.Drop1SurchargeFees = csvRecord.CsvViewModel.Drop4SurchargeFees;
            newRecord.CsvViewModel.Drop1TicketNumber = csvRecord.CsvViewModel.Drop4TicketNumber;
            newRecord.CsvViewModel.Drop1WethoseFees = csvRecord.CsvViewModel.Drop4WethoseFees;
            newRecord.CsvViewModel.Drop1ApiGravity = csvRecord.CsvViewModel.Drop4ApiGravity;
        }

        private void SetManualInvoiceViewModelFromCsvParameters(ManualInvoiceViewModel manualInvoiceViewModel, InvoiceBulkViewModel csvRecord, UserContext context)
        {
            manualInvoiceViewModel.userId = context.Id;
            manualInvoiceViewModel.CreatedDate = DateTimeOffset.Now;

            //set all csv information
            DateTime.TryParse(csvRecord.CsvViewModel.Drop1ArrivalDate, out DateTime newDate);
            manualInvoiceViewModel.DeliveryDate = newDate;
            manualInvoiceViewModel.StartTime = csvRecord.CsvViewModel.Drop1ArrivalTime;
            manualInvoiceViewModel.EndTime = csvRecord.CsvViewModel.Drop1CompleteTime;
            //if completetime is less than arrival time - set dropenddate = newdate + 1 day
            if (IsEndTimeLessThanStartTime(manualInvoiceViewModel.StartTime, manualInvoiceViewModel.EndTime))
            {
                manualInvoiceViewModel.DropEndDate = newDate.AddDays(1);
            }

            manualInvoiceViewModel.FuelDropped = csvRecord.CsvViewModel.Drop1Quantity.GetValue<decimal>();
            manualInvoiceViewModel.Notes = csvRecord.CsvViewModel.Drop1Notes;
            manualInvoiceViewModel.TruckNumber = csvRecord.CsvViewModel.TruckNumber;
            manualInvoiceViewModel.DropTicketNumber = csvRecord.CsvViewModel.Drop1TicketNumber;
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.FuelCost))
                manualInvoiceViewModel.FuelCost = csvRecord.CsvViewModel.FuelCost.GetValue<decimal>();
            manualInvoiceViewModel.CreationMethod = CreationMethod.BulkUploaded;
            manualInvoiceViewModel.Gravity = csvRecord.CsvViewModel.Drop1ApiGravity.GetValue<decimal>();

            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.TerminalControlNumber))
            {
                manualInvoiceViewModel.BolDetails.BolNumber = csvRecord.CsvViewModel.BolNumber;
                manualInvoiceViewModel.BolDetails.GrossQuantity = csvRecord.CsvViewModel.BolGross.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.NetQuantity = csvRecord.CsvViewModel.BolNet.GetValue<decimal>();
            }
            else
            {
                manualInvoiceViewModel.BolDetails.LiftArrivalTime = GetTimeSpan(csvRecord.CsvViewModel.LiftArrivalTime);
                manualInvoiceViewModel.BolDetails.LiftStartTime = GetTimeSpan(csvRecord.CsvViewModel.LiftStartTime);
                manualInvoiceViewModel.BolDetails.LiftEndTime = GetTimeSpan(csvRecord.CsvViewModel.LiftEndTime);
                manualInvoiceViewModel.BolDetails.LiftTicketNumber = csvRecord.CsvViewModel.LiftTicketNumber;
                manualInvoiceViewModel.BolDetails.LiftQuantity = csvRecord.CsvViewModel.LiftQuantity.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.GrossQuantity = csvRecord.CsvViewModel.LiftGross.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.NetQuantity = csvRecord.CsvViewModel.LiftNet.GetValue<decimal>();
                SetPickupLocation(csvRecord, manualInvoiceViewModel);
            }

            //lift details
            manualInvoiceViewModel.BolDetails.Carrier = string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolCarrier) ? context.CompanyName : csvRecord.CsvViewModel.BolCarrier;
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftDate))
            {
                DateTime liftDate;
                DateTime.TryParse(csvRecord.CsvViewModel.LiftDate, out liftDate);
                manualInvoiceViewModel.BolDetails.LiftDate = liftDate;
            }
            if (manualInvoiceViewModel.IsFTL)
            {
                SetFtlParameters(manualInvoiceViewModel, csvRecord);
            }

            //set asset drop info
            SetAssetDropDetails(manualInvoiceViewModel, csvRecord);

            // set terminal details
            SetTerminalDetails(manualInvoiceViewModel, csvRecord);

            //driver details
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.DriverFirstName))
            {
                manualInvoiceViewModel.DriverId = GetDriverId(csvRecord.CsvViewModel.DriverFirstName, csvRecord.CsvViewModel.DriverLastName, csvRecord.CsvViewModel.PONumber, context);
            }

            SetFeesFromCsvFile(csvRecord, manualInvoiceViewModel, context);

            SetFuelSurchargeFee(csvRecord, manualInvoiceViewModel);
        }

        private void SetInvoiceViewModelNewFromCsvParameters(InvoiceViewModelNew invoiceViewModel, List<InvoiceBulkViewModel> csvConsolidatedRecord,
            UserContext userContext, OrderEnforcement orderEnforcement)
        {
            var invoiceCreate = new InvoiceCreateDomain(this);
            invoiceViewModel.Fees = new List<FeesViewModel>();
            var deDuplicatedFees = new List<FeesViewModel>();
            foreach (var csvRecord in csvConsolidatedRecord)
            {
                var order = Context.DataContext.Orders.
                                    Where(t => t.IsActive && t.PoNumber == csvRecord.CsvViewModel.PONumber.Trim()
                                    && t.AcceptedCompanyId == userContext.CompanyId).
                                    Select(t => new
                                    {
                                        Id = t.Id
                                    }).FirstOrDefault();

                var orderFees = invoiceCreate.GetInvoiceDropFeesAsync(order.Id).Result;
                //Remove Duplicate Fees
                deDuplicatedFees = ConsolidatedInvoiceDomain.DeDuplicateFees(orderFees, deDuplicatedFees);
            }
            invoiceViewModel.Fees = deDuplicatedFees;
            invoiceViewModel.Fees.RemoveAll(t => (t.FeeTypeId == ((int)FeeType.DemurrageFeeTerminal).ToString() && t.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                                            || (t.FeeTypeId == ((int)FeeType.DemurrageFeeDestination).ToString() && t.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                                            || (t.FeeTypeId == ((int)FeeType.DemurrageOther).ToString() && t.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                                            );

            //to enforce order level fee - we need hours so removing these types of fees


            var bolDetails = new List<InvoiceBolViewModel>();
            var liftTicketDetails = new List<InvoiceLiftTicketViewModel>();
            foreach (var csvRecord in csvConsolidatedRecord)
            {
                InvoiceDropViewModel drop = null;
                bool isAddDrop = false;
                drop = invoiceViewModel.Drops.FirstOrDefault(t => t.PoNumber == csvRecord.CsvViewModel.PONumber.Trim());
                if (drop == null)
                {
                    drop = new InvoiceDropViewModel();
                    isAddDrop = true;
                }
                drop.Assets.Clear();

                var bolDetail = new InvoiceBolViewModel();
                var liftTicketDetail = new InvoiceLiftTicketViewModel();

                var bolProduct = new BolProductViewModel();
                var liftTicketProduct = new LiftProductViewModel();

                var order = Context.DataContext.Orders.
                                    Where(t => t.IsActive && t.PoNumber == csvRecord.CsvViewModel.PONumber.Trim()
                                    && t.AcceptedCompanyId == userContext.CompanyId).
                                    Select(t => new
                                    {
                                        //IsFTL = t.IsFTL,
                                        Id = t.Id,
                                        Currency = t.FuelRequest.Currency,
                                        UoM = t.FuelRequest.UoM,
                                        PoNumber = t.PoNumber,
                                        ProductName = t.FuelRequest.MstProduct.Name,
                                        ProductId = t.FuelRequest.FuelTypeId,
                                        TypeOfFuel = t.FuelRequest.MstProduct.ProductTypeId,
                                        t.FuelRequest.FuelRequestFees,
                                        t.OrderAdditionalDetail,
                                        t.AcceptedCompanyId,
                                        t.BuyerCompanyId,
                                        t.TerminalId,
                                        TerminalName = t.MstExternalTerminal.Name,
                                    }).FirstOrDefault();

                //set drop details
                drop.PoNumber = order.PoNumber;
                drop.OrderId = order.Id;
                drop.FuelTypeId = order.ProductId;
                drop.FuelTypeName = order.ProductName;
                drop.TypeOfFuel = order.TypeOfFuel;

                //set all csv information
                DateTime.TryParse(csvRecord.CsvViewModel.Drop1ArrivalDate, out DateTime newDate);
                drop.DropDate = newDate;
                drop.StartTime = csvRecord.CsvViewModel.Drop1ArrivalTime;
                drop.EndTime = csvRecord.CsvViewModel.Drop1CompleteTime;
                if (IsEndTimeLessThanStartTime(drop.StartTime, drop.EndTime))
                {
                    drop.DropEndDate = newDate.AddDays(1);
                }

                drop.ActualDropQuantity = csvRecord.CsvViewModel.Drop1Quantity.GetValue<decimal>();
                drop.DropTicketNumber = csvRecord.CsvViewModel.Drop1TicketNumber;
                drop.CarrierOrderId = csvRecord.CsvViewModel.CarrierOrderId;
                //drop.LoadingBadge = csvRecord.CsvViewModel.LoadingBadge;
                drop.Tractor = csvRecord.CsvViewModel.Tractor;
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.OrderDate))
                {
                    DateTime ordDate;
                    DateTime.TryParse(csvRecord.CsvViewModel.OrderDate, out ordDate);
                    drop.OrderDate = ordDate;
                }
                drop.OrderQuantity = csvRecord.CsvViewModel.OrderQuantity.GetValue<decimal>();

                // set parameters for mfn
                drop.Gravity = csvRecord.CsvViewModel.Drop1ApiGravity.GetValue<decimal>();

                invoiceViewModel.InvoiceNotes = csvRecord.CsvViewModel.Drop1Notes;
                invoiceViewModel.TruckNumber = csvRecord.CsvViewModel.TruckNumber;

                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.FuelCost))
                    drop.SupplierFuelCost = csvRecord.CsvViewModel.FuelCost.GetValue<decimal>();
                invoiceViewModel.CreationMethod = CreationMethod.BulkUploaded;
                invoiceViewModel.Carrier = string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolCarrier) ? userContext.CompanyName : csvRecord.CsvViewModel.BolCarrier;

                invoiceViewModel.SupplierInvoiceNumber = csvRecord.CsvViewModel.SupplierInvoiceNumber;

                decimal totalDropQuantity = drop.ActualDropQuantity;
                //Bol details
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolNumber))
                {
                    List<BolProductViewModel> lstBolProducts = new List<BolProductViewModel>();
                    bolDetail.BolNumber = csvRecord.CsvViewModel.BolNumber;
                    bolDetail.BadgeNumber = csvRecord.CsvViewModel.LoadingBadge;
                    bolProduct.NetQuantity = csvRecord.CsvViewModel.BolNet.GetValue<decimal>();
                    bolProduct.GrossQuantity = csvRecord.CsvViewModel.BolGross.GetValue<decimal>();

                    totalDropQuantity = SetBolDeliveredQuantity(csvRecord.CsvViewModel.BolDelivered, bolProduct, totalDropQuantity);

                    bolProduct.ProductId = order.ProductId;
                    bolProduct.ProductName = order.ProductName;

                    if (string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolCreationTime))
                        liftTicketDetail.BolCreationTime = GetTimeSpan(DateTimeOffset.Now.GetTimeInHhMmFormat());
                    else
                        liftTicketDetail.BolCreationTime = GetTimeSpan(csvRecord.CsvViewModel.BolCreationTime);

                    if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftDate))
                    {
                        DateTime liftDate;
                        DateTime.TryParse(csvRecord.CsvViewModel.LiftDate, out liftDate);
                        bolDetail.LiftDate = liftDate;
                    }

                    if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.TerminalControlNumber))
                    {
                        // set terminal details
                        var terminalDetails = Context.DataContext.MstExternalTerminals.
                                                Where(t => t.IsActive && t.ControlNumber != null
                                                        && t.ControlNumber != string.Empty
                                                        && (t.ControlNumber.ToLower() == csvRecord.CsvViewModel.TerminalControlNumber.ToLower() || t.Name.ToLower() == csvRecord.CsvViewModel.TerminalControlNumber.ToLower()))
                                                .Select(t => new { TerminalId = t.Id, t.ControlNumber, TerminalName = t.Name })
                                                .FirstOrDefault();

                        if (terminalDetails == null)
                        {
                            terminalDetails = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == userContext.CompanyId && t.TerminalId != null
                                                && t.IsActive && t.TerminalSupplierId == null && t.AssignedTerminalId.ToLower().Equals(csvRecord.CsvViewModel.TerminalControlNumber.ToLower()))
                                                .Select(t => new { TerminalId = t.TerminalId.Value, t.MstExternalTerminal.ControlNumber, TerminalName = t.MstExternalTerminal.Name })
                                                .FirstOrDefault();
                        }

                        if (terminalDetails != null)
                        {
                            bolProduct.TerminalId = terminalDetails.TerminalId;
                            bolProduct.TerminalName = terminalDetails.TerminalName;
                        }
                        else
                        {
                            bolProduct.TerminalId = order.TerminalId;
                            bolProduct.TerminalName = order.TerminalName;
                        }
                    }
                    else
                    {
                        bolProduct.TerminalId = order.TerminalId;
                        bolProduct.TerminalName = order.TerminalName;
                    }

                    lstBolProducts.Add(bolProduct);
                    bolDetail.Products = lstBolProducts;
                    bolDetails.Add(bolDetail);
                }

                //Lift Details
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftTicketNumber))
                {
                    List<LiftProductViewModel> lstLiftTikcetProducts = new List<LiftProductViewModel>();
                    liftTicketDetail.LiftTicketNumber = csvRecord.CsvViewModel.LiftTicketNumber;
                    liftTicketDetail.BadgeNumber = csvRecord.CsvViewModel.LoadingBadge;
                    liftTicketDetail.BolCreationTime = GetTimeSpan(csvRecord.CsvViewModel.LiftTicketCreationTime);
                    liftTicketProduct.LiftQuantity = csvRecord.CsvViewModel.LiftQuantity.GetValue<decimal>();
                    liftTicketProduct.NetQuantity = csvRecord.CsvViewModel.LiftNet.GetValue<decimal>();
                    liftTicketProduct.GrossQuantity = csvRecord.CsvViewModel.LiftGross.GetValue<decimal>();
                    liftTicketDetail.LiftArrivalTime = GetTimeSpan(csvRecord.CsvViewModel.LiftArrivalTime);
                    liftTicketProduct.LiftStartTime = GetTimeSpan(csvRecord.CsvViewModel.LiftStartTime);
                    liftTicketProduct.LiftEndTime = GetTimeSpan(csvRecord.CsvViewModel.LiftEndTime);
                    liftTicketProduct.ProductId = order.ProductId;
                    liftTicketProduct.ProductName = order.ProductName;
                    liftTicketProduct.BulkPlantName = csvRecord.CsvViewModel.BulkPlantName;
                    if (totalDropQuantity > 0 && liftTicketProduct.NetQuantity > 0) {
                        totalDropQuantity = SetLiftDeliveredQuantity(csvRecord.CsvViewModel.LiftDelivered, liftTicketProduct, totalDropQuantity);
                    }

                    if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftDate))
                    {
                        DateTime liftDate;
                        DateTime.TryParse(csvRecord.CsvViewModel.LiftDate, out liftDate);
                        liftTicketDetail.LiftDate = liftDate;
                    }

                    //lift address
                    if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftAddressZip))
                    {
                        var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                        liftTicketProduct.Address = new DropAddressViewModel();
                        liftTicketProduct.Address.Address = csvRecord.CsvViewModel.LiftAddressStreet1 + " " + csvRecord.CsvViewModel.LiftAddressStreet2;
                        liftTicketProduct.Address.City = csvRecord.CsvViewModel.LiftAddressCity;
                        liftTicketProduct.Address.State.Code = csvRecord.CsvViewModel.LiftAddressState;
                        var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(csvRecord.CsvViewModel.LiftAddressState.ToLower()));
                        if (stateDetails != null)
                        {
                            liftTicketProduct.Address.State.Id = stateDetails.Id;
                            liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                            liftTicketProduct.Address.Country.Code = stateDetails.CountryCode;
                            liftTicketProduct.Address.Country.Name = stateDetails.Name;
                        }
                        var geoAddress = GoogleApiDomain.GetGeocode(csvRecord.CsvViewModel.LiftAddressZip);
                        if (geoAddress != null && !string.IsNullOrWhiteSpace(geoAddress.CountyName))
                        {
                            liftTicketProduct.Address.CountyName = geoAddress.CountyName;
                        }
                        liftTicketProduct.Address.ZipCode = csvRecord.CsvViewModel.LiftAddressZip;
                        liftTicketProduct.Address.Latitude = csvRecord.CsvViewModel.LiftAddressLat.GetValue<decimal>();
                        liftTicketProduct.Address.Longitude = csvRecord.CsvViewModel.LiftAddressLong.GetValue<decimal>();
                        liftTicketProduct.Address.IsAddressAvailable = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftAddressLat) && !string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftAddressLong))
                    {
                        var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(csvRecord.CsvViewModel.LiftAddressLat), Convert.ToDouble(csvRecord.CsvViewModel.LiftAddressLong));
                        if (geoAddress != null)
                        {
                            var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                            liftTicketProduct.Address = new DropAddressViewModel();
                            liftTicketProduct.Address.Address = geoAddress.Address;
                            liftTicketProduct.Address.City = geoAddress.City;
                            liftTicketProduct.Address.State.Code = geoAddress.StateCode;
                            var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(geoAddress.StateCode.ToLower()));
                            if (stateDetails != null)
                            {
                                liftTicketProduct.Address.State.Id = stateDetails.Id;
                                liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                                liftTicketProduct.Address.Country.Code = stateDetails.CountryCode;
                                liftTicketProduct.Address.Country.Name = stateDetails.Name;
                            }
                            if (!string.IsNullOrWhiteSpace(geoAddress.CountyName))
                            {
                                liftTicketProduct.Address.CountyName = geoAddress.CountyName;
                            }
                            liftTicketProduct.Address.ZipCode = geoAddress.ZipCode;
                            liftTicketProduct.Address.Latitude = csvRecord.CsvViewModel.LiftAddressLat.GetValue<decimal>();
                            liftTicketProduct.Address.Longitude = csvRecord.CsvViewModel.LiftAddressLong.GetValue<decimal>();
                            liftTicketProduct.Address.IsAddressAvailable = true;
                        }

                    }
                    lstLiftTikcetProducts.Add(liftTicketProduct);
                    liftTicketDetail.Products = lstLiftTikcetProducts;
                    liftTicketDetails.Add(liftTicketDetail);
                }

                //drop address
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop1AddressZip) ||
                        (invoiceViewModel.IsVariousOrigin && !string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop1AddressState)))
                {
                    invoiceViewModel.FuelDropLocation = new DropAddressViewModel();
                    invoiceViewModel.FuelDropLocation.Address = csvRecord.CsvViewModel.Drop1AddressStreet1;
                    invoiceViewModel.FuelDropLocation.City = csvRecord.CsvViewModel.Drop1AddressCity;
                    invoiceViewModel.FuelDropLocation.State.Code = csvRecord.CsvViewModel.Drop1AddressState;

                    var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                    var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(csvRecord.CsvViewModel.Drop1AddressState.ToLower()));
                    if (stateDetails != null)
                    {
                        invoiceViewModel.FuelDropLocation.State.Id = stateDetails.Id;
                        invoiceViewModel.FuelDropLocation.Country.Id = stateDetails.CountryId;
                        invoiceViewModel.FuelDropLocation.Country.Code = stateDetails.CountryCode;
                    }
                    invoiceViewModel.FuelDropLocation.ZipCode = csvRecord.CsvViewModel.Drop1AddressZip;
                }

                //set asset drop info
                if (csvRecord.AssetDropList.Any())
                {
                    foreach (var assetDrop in csvRecord.AssetDropList)
                    {
                        assetDrop.OrderId = order.Id;
                        assetDrop.Id = GetAssetId(assetDrop, invoiceViewModel.Customer.CompanyId);
                    }
                    drop.Assets.AddRange(csvRecord.AssetDropList);
                    drop.IsAssetTracked = true;

                    drop.ActualDropQuantity = drop.Assets.Where(t => t.DropGallons.HasValue).Sum(t => t.DropGallons.Value);
                }

                //driver details
                if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.DriverFirstName))
                {
                    invoiceViewModel.Driver = new DropdownDisplayItem();
                    invoiceViewModel.Driver.Id = GetDriverId(csvRecord.CsvViewModel.DriverFirstName, csvRecord.CsvViewModel.DriverLastName, csvRecord.CsvViewModel.PONumber, userContext) ?? 0;
                }

                //Freight Fee
                SetFuelSurchargeFeeNew(csvRecord, invoiceViewModel, order, drop);

                if (isAddDrop)
                    invoiceViewModel.Drops.Add(drop);
            }

            var firstDrop = invoiceViewModel.Drops.First();
            var isFtl = invoiceViewModel.Drops.Any(t => t.IsFTL);

            //Fees
            if (orderEnforcement != OrderEnforcement.EnforceOrderLevelValues)
            {
                if (orderEnforcement == OrderEnforcement.ManageException)
                {
                    //manage exception
                }
                else if (orderEnforcement == OrderEnforcement.NoEnforcement)
                {
                    //no enforcement is only for API
                }

                invoiceViewModel.Fees.RemoveAll(t => t.FeeTypeId != ((int)FeeType.ProcessingFee).ToString() && t.FeeTypeId != ((int)FeeType.SurchargeFreightFee).ToString() && t.FeeTypeId != ((int)FeeType.FreightCost).ToString());
                SetFeesFromCsvFileNew(csvConsolidatedRecord, invoiceViewModel, isFtl, firstDrop.Currency, firstDrop.UoM);
            }

            invoiceViewModel.TicketDetails = liftTicketDetails;
            invoiceViewModel.BolDetails = bolDetails;
        }

        private static decimal SetBolDeliveredQuantity(string deliveredQuantity, BolProductViewModel bolProduct, decimal totalDropQuantity)
        {
            if (bolProduct.NetQuantity > 0)
            {
                decimal? deliveredQty = null;
                if (!string.IsNullOrEmpty(deliveredQuantity))
                {
                    deliveredQty = deliveredQuantity.GetValue<decimal>();
                }
                else
                {
                    decimal? pickupQty = bolProduct.NetQuantity > bolProduct.GrossQuantity ? bolProduct.NetQuantity : bolProduct.GrossQuantity;
                    deliveredQty = totalDropQuantity <= pickupQty ? totalDropQuantity : pickupQty;
                    totalDropQuantity = totalDropQuantity - deliveredQty.Value;
                }
                bolProduct.DeliveredQuantity = deliveredQty;
            }

            return totalDropQuantity;
        }

        private static decimal SetLiftDeliveredQuantity(string deliveredQuantity, LiftProductViewModel bolProduct, decimal totalDropQuantity)
        {
            if (bolProduct.NetQuantity > 0)
            {
                decimal? deliveredQty = null;
                if (!string.IsNullOrEmpty(deliveredQuantity))
                {
                    deliveredQty = deliveredQuantity.GetValue<decimal>();
                }
                else
                {
                    decimal? pickupQty = bolProduct.NetQuantity > bolProduct.GrossQuantity ? bolProduct.NetQuantity : bolProduct.GrossQuantity;
                    deliveredQty = totalDropQuantity <= pickupQty ? totalDropQuantity : pickupQty;
                    totalDropQuantity = totalDropQuantity - deliveredQty.Value;
                }
                bolProduct.DeliveredQuantity = deliveredQty;
            }

            return totalDropQuantity;
        }

        private void SetSpitInvoiceViewModelFromCsvParameters(ManualInvoiceViewModel manualInvoiceViewModel, InvoiceBulkViewModel csvRecord, UserContext context)
        {
            manualInvoiceViewModel.userId = context.Id;
            manualInvoiceViewModel.CreatedDate = DateTimeOffset.Now;

            //set all csv information
            DateTime.TryParse(csvRecord.CsvViewModel.Drop1ArrivalDate, out DateTime newDate);
            manualInvoiceViewModel.DeliveryDate = newDate;
            manualInvoiceViewModel.StartTime = csvRecord.CsvViewModel.Drop1ArrivalTime;
            manualInvoiceViewModel.EndTime = csvRecord.CsvViewModel.Drop1CompleteTime;

            manualInvoiceViewModel.FuelDropped = csvRecord.CsvViewModel.Drop1Quantity.GetValue<decimal>();
            manualInvoiceViewModel.Notes = csvRecord.CsvViewModel.Drop1Notes;
            manualInvoiceViewModel.TruckNumber = csvRecord.CsvViewModel.TruckNumber;
            manualInvoiceViewModel.DropTicketNumber = csvRecord.CsvViewModel.Drop1TicketNumber;
            manualInvoiceViewModel.CreationMethod = CreationMethod.BulkUploaded;
            manualInvoiceViewModel.Gravity = csvRecord.CsvViewModel.Drop1ApiGravity.GetValue<decimal>();

            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.TerminalControlNumber))
            {
                manualInvoiceViewModel.BolDetails.BolNumber = csvRecord.CsvViewModel.BolNumber;
                manualInvoiceViewModel.BolDetails.GrossQuantity = csvRecord.CsvViewModel.BolGross.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.NetQuantity = csvRecord.CsvViewModel.BolNet.GetValue<decimal>();
            }
            else
            {
                manualInvoiceViewModel.BolDetails.LiftArrivalTime = GetTimeSpan(csvRecord.CsvViewModel.LiftArrivalTime);
                manualInvoiceViewModel.BolDetails.LiftStartTime = GetTimeSpan(csvRecord.CsvViewModel.LiftStartTime);
                manualInvoiceViewModel.BolDetails.LiftEndTime = GetTimeSpan(csvRecord.CsvViewModel.LiftEndTime);
                manualInvoiceViewModel.BolDetails.LiftTicketNumber = csvRecord.CsvViewModel.LiftTicketNumber;
                manualInvoiceViewModel.BolDetails.LiftQuantity = csvRecord.CsvViewModel.LiftQuantity.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.GrossQuantity = csvRecord.CsvViewModel.LiftGross.GetValue<decimal>();
                manualInvoiceViewModel.BolDetails.NetQuantity = csvRecord.CsvViewModel.LiftNet.GetValue<decimal>();
                SetPickupLocation(csvRecord, manualInvoiceViewModel);
            }

            //lift details
            manualInvoiceViewModel.BolDetails.Carrier = string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolCarrier) ? context.CompanyName : csvRecord.CsvViewModel.BolCarrier;
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftDate))
            {
                DateTime liftDate;
                DateTime.TryParse(csvRecord.CsvViewModel.LiftDate, out liftDate);
                manualInvoiceViewModel.BolDetails.LiftDate = liftDate;
            }
            if (manualInvoiceViewModel.IsFTL)
            {
                SetFtlParameters(manualInvoiceViewModel, csvRecord);
            }
            SetPickupLocation(csvRecord, manualInvoiceViewModel);
            SetFeesFromCsvFile(csvRecord, manualInvoiceViewModel, context);

            SetFuelSurchargeFee(csvRecord, manualInvoiceViewModel);
        }

        private void SetPickupLocation(InvoiceBulkViewModel csvRecord, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            //lift address
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.LiftAddressZip))
            {
                var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                manualInvoiceViewModel.PickUpAddress = new DropAddressViewModel();
                manualInvoiceViewModel.PickUpAddress.Address = csvRecord.CsvViewModel.LiftAddressStreet1 + " " + csvRecord.CsvViewModel.LiftAddressStreet2;
                manualInvoiceViewModel.PickUpAddress.City = csvRecord.CsvViewModel.LiftAddressCity;
                manualInvoiceViewModel.PickUpAddress.State.Code = csvRecord.CsvViewModel.LiftAddressState;
                var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(csvRecord.CsvViewModel.LiftAddressState.ToLower()));
                if (stateDetails != null)
                {
                    manualInvoiceViewModel.PickUpAddress.State.Id = stateDetails.Id;
                    manualInvoiceViewModel.PickUpAddress.Country.Id = stateDetails.CountryId;
                    manualInvoiceViewModel.PickUpAddress.Country.Code = stateDetails.CountryCode;
                    manualInvoiceViewModel.PickUpAddress.Country.Name = stateDetails.Name;
                }
                manualInvoiceViewModel.PickUpAddress.ZipCode = csvRecord.CsvViewModel.LiftAddressZip;
                manualInvoiceViewModel.PickUpAddress.Latitude = csvRecord.CsvViewModel.LiftAddressLat.GetValue<decimal>();
                manualInvoiceViewModel.PickUpAddress.Longitude = csvRecord.CsvViewModel.LiftAddressLong.GetValue<decimal>();
                manualInvoiceViewModel.PickUpAddress.IsAddressAvailable = true;
            }
        }

        private void SetFuelSurchargeFee(InvoiceBulkViewModel csvRecord, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var fscObj = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
            if (fscObj != null)
            {
                if (fscObj.IsFeeByDistance && fscObj.DeliveryFeeByQuantity.Any())
                {
                    var exactFee = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.DeliveryFeeByQuantity.FirstOrDefault();
                    if (exactFee != null)
                        manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.TotalFuelSurchargeFee =
                            GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, exactFee.Fee, manualInvoiceViewModel.FuelDropped);
                    //(fscObj.SurchargePercentage / 100) * exactFee.Fee * manualInvoiceViewModel.FuelDropped ?? 0;
                }
                else
                    manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.TotalFuelSurchargeFee =
                                                GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, fscObj.SurchargeFreightCost, manualInvoiceViewModel.FuelDropped);
                //(fscObj.SurchargePercentage / 100) * fscObj.SurchargeFreightCost * manualInvoiceViewModel.FuelDropped ?? 0;
            }
        }

        private void SetFuelSurchargeFeeNew(InvoiceBulkViewModel csvRecord, InvoiceViewModelNew invoiceViewModel, dynamic order, InvoiceDropViewModel drop)
        {
            InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain(this);
            var isDropDetails = invoiceViewModel.Drops.FirstOrDefault(t => t.PoNumber == csvRecord.CsvViewModel.PONumber.Trim());
            if (isDropDetails == null)
            {
                drop.FuelSurchargeFreightFee = invoiceCreateDomain.GetFuelSurchargeDetails(order.FuelRequestFees, order.OrderAdditionalDetail,
                                                            order.TypeOfFuel, order.AcceptedCompanyId, order.BuyerCompanyId);
            }
            var fscObj = drop.FuelSurchargeFreightFee;
            if (fscObj != null)
            {
                if (fscObj.IsFeeByDistance && fscObj.DeliveryFeeByQuantity.Any())
                {
                    var exactFee = fscObj.DeliveryFeeByQuantity.FirstOrDefault();
                    if (exactFee != null)
                        fscObj.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, exactFee.Fee, drop.ActualDropQuantity);
                }
                else
                {
                    fscObj.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, fscObj.SurchargeFreightCost, drop.ActualDropQuantity);
                }
            }
        }

        private void SetAssetDropDetails(ManualInvoiceViewModel manualInvoiceViewModel, InvoiceBulkViewModel csvRecord)
        {
            if (!manualInvoiceViewModel.IsFTL)
            {
                if (csvRecord.AssetDropList.Any())
                {
                    foreach (var assetDrop in csvRecord.AssetDropList)
                    {
                        assetDrop.OrderId = manualInvoiceViewModel.OrderId;
                        assetDrop.Id = GetAssetId(assetDrop, manualInvoiceViewModel.BuyerCompanyId);
                    }
                    manualInvoiceViewModel.SelectedAssets.AddRange(csvRecord.AssetDropList.Select(t => t.Id).ToList());
                    manualInvoiceViewModel.Assets.AddRange(csvRecord.AssetDropList);
                    manualInvoiceViewModel.AssetTracked = true;
                }
            }
        }

        private void SetTerminalDetails(ManualInvoiceViewModel manualInvoiceViewModel, InvoiceBulkViewModel csvRecord)
        {
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.TerminalControlNumber))
            {
                var terminalDetails = Context.DataContext.MstExternalTerminals
                                    .Where(t => t.IsActive && t.ControlNumber != null && t.ControlNumber != string.Empty
                                        && (t.ControlNumber.ToLower() == csvRecord.CsvViewModel.TerminalControlNumber.ToLower() || t.Name.ToLower() == csvRecord.CsvViewModel.TerminalControlNumber.ToLower()))
                                                                       .Select(t => new { TerminalId = t.Id, t.ControlNumber, TerminalName = t.Name })
                                                                       .FirstOrDefault();

                if (terminalDetails != null)
                {
                    manualInvoiceViewModel.BolDetails.TerminalId = terminalDetails.TerminalId;
                    manualInvoiceViewModel.BolDetails.TerminalName = terminalDetails.TerminalName;
                }
            }
        }

        private void SetFtlParameters(ManualInvoiceViewModel manualInvoiceViewModel, InvoiceBulkViewModel csvRecord)
        {
            if (!csvRecord.IsSplitLoadInvoice)
            {
                if (string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.TerminalControlNumber) && (manualInvoiceViewModel.BolDetails.NetQuantity <= 0 && manualInvoiceViewModel.BolDetails.GrossQuantity <= 0))
                {
                    manualInvoiceViewModel.FuelDropped = manualInvoiceViewModel.BolDetails.LiftQuantity;
                }
                else
                {
                    if (manualInvoiceViewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net)
                        manualInvoiceViewModel.FuelDropped = manualInvoiceViewModel.BolDetails.NetQuantity;
                    else
                        manualInvoiceViewModel.FuelDropped = manualInvoiceViewModel.BolDetails.GrossQuantity;
                }

            }

            if (string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.BolCreationTime))
                manualInvoiceViewModel.BolDetails.BolCreationTime = GetTimeSpan(DateTimeOffset.Now.GetTimeInHhMmFormat());
            else
                manualInvoiceViewModel.BolDetails.BolCreationTime = GetTimeSpan(csvRecord.CsvViewModel.BolCreationTime);

            var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
            //drop address
            if (!string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop1AddressZip) || (manualInvoiceViewModel.IsVariousFobOrigin && !string.IsNullOrWhiteSpace(csvRecord.CsvViewModel.Drop1AddressState)))
            {
                manualInvoiceViewModel.DropAddress = new DropAddressViewModel();
                manualInvoiceViewModel.DropAddress.Address = csvRecord.CsvViewModel.Drop1AddressStreet1;
                manualInvoiceViewModel.DropAddress.City = csvRecord.CsvViewModel.Drop1AddressCity;
                manualInvoiceViewModel.DropAddress.State.Code = csvRecord.CsvViewModel.Drop1AddressState;
                var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(csvRecord.CsvViewModel.Drop1AddressState.ToLower()));
                if (stateDetails != null)
                {
                    manualInvoiceViewModel.DropAddress.State.Id = stateDetails.Id;
                    manualInvoiceViewModel.DropAddress.Country.Id = stateDetails.CountryId;
                    manualInvoiceViewModel.DropAddress.Country.Code = stateDetails.CountryCode;
                }
                manualInvoiceViewModel.DropAddress.ZipCode = csvRecord.CsvViewModel.Drop1AddressZip;
            }
        }

        private TimeSpan? GetTimeSpan(string csvTime)
        {
            var isPmTime = csvTime.ToLower().Contains("pm");
            var time = csvTime.ToLower().Replace("am", string.Empty).Replace("pm", string.Empty);
            TimeSpan.TryParse(time, out TimeSpan result);
            if (isPmTime && result.Hours != 12)
                result = result.Add(new TimeSpan(12, 0, 0));
            if (!isPmTime && result.Hours == 12)
                result = result.Subtract(new TimeSpan(12, 0, 0));
            return result;
        }

        private void SetFeesFromCsvFileNew(List<InvoiceBulkViewModel> csvRecords, InvoiceViewModelNew manualInvoiceViewModel, bool isFtl, Currency currency, UoM uoM)
        {
            //DemurrageOther fee
            var feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1DemurrageFees.GetValue<decimal>()).Max();
            var demurrageTime = csvRecords.Where(t => t.CsvViewModel.Drop1DemurrageFees.GetValue<decimal>() == feeFromCsv)
                                .Select(t => t.CsvViewModel.Drop1DemurrageTime).FirstOrDefault().GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.DemurrageOther).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                    feeObjFromOrder.FeeSubQuantity = demurrageTime; //to save value in Hr
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.DemurrageOther, feeFromCsv, manualInvoiceViewModel,
                            isFtl, currency, uoM, demurrageTime * 60);
                }
            }

            //WetHoseFee fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1WethoseFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.WetHoseFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.WetHoseFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //FreightFee fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1FreightFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.FreightFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.FreightFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //LoadFee fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1LoadFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.LoadFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.LoadFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //Envrironmental fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1EnvironmentalFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.EnvironmentalFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.EnvironmentalFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //ServiceFee fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1ServiceFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.ServiceFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.ServiceFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //OverWaterFee fee
            feeFromCsv = csvRecords.Select(t => t.CsvViewModel.Drop1OverWaterFees.GetValue<decimal>()).Max();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.OverWaterFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.OverWaterFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            foreach (var csvRecord in csvRecords)
            {
                //SurchargeFee fee
                feeFromCsv = csvRecord.CsvViewModel.Drop1SurchargeFees.GetValue<decimal>();
                if (feeFromCsv > 0)
                {
                    var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFee).ToString()));
                    if (feeObjFromOrder != null)
                    {
                        feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                        feeObjFromOrder.Fee = feeFromCsv;
                    }
                    else
                    {
                        AddFuelReqFeeViewModelNew((int)FeeType.SurchargeFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                    }
                }

                //OtherFee fee
                feeFromCsv = csvRecord.CsvViewModel.Drop1OtherFees.GetValue<decimal>();
                if (feeFromCsv > 0)
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.OtherFee, feeFromCsv, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }
        }

        private void AddFuelReqFeeViewModelNew(int feeType, decimal totalFee, InvoiceViewModelNew manualInvoiceViewModel,
                bool isFTL, Currency currency, UoM uoM, decimal? feeSubQuantity = null)
        {
            int feeSubType = (int)FeeSubType.FlatFee;

            var feeViewModel = new FeesViewModel()
            {
                FeeTypeId = feeType.ToString(),
                FeeSubTypeId = feeSubType,
                Fee = totalFee,
                FeeSubQuantity = feeSubQuantity,
                TruckLoadType = isFTL ? (int)TruckLoadTypes.FullTruckLoad : (int)TruckLoadTypes.LessTruckLoad,
                CommonFee = true,
                Currency = currency,
                UoM = uoM,
            };

            if (feeType == (int)FeeType.OtherFee)
            {
                feeViewModel.CommonFee = false;
                feeViewModel.OtherFeeDescription = "bulk other fee";
            }

            manualInvoiceViewModel.Fees.Add(feeViewModel);
        }

        private void SetFeesFromCsvFile(InvoiceBulkViewModel csvRecord, ManualInvoiceViewModel manualInvoiceViewModel, UserContext context)
        {
            //DemurrageOther fee
            var feeFromCsv = csvRecord.CsvViewModel.Drop1DemurrageFees.GetValue<decimal>();
            if (feeFromCsv > 0 && manualInvoiceViewModel.IsFTL)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.DemurrageOther).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                    feeObjFromOrder.FeeSubQuantity = csvRecord.CsvViewModel.Drop1DemurrageTime.GetValue<decimal>(); //to save value in Hr
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.DemurrageOther, feeFromCsv, manualInvoiceViewModel, csvRecord.CsvViewModel.Drop1DemurrageTime.GetValue<decimal>() * 60);
                }
            }

            //WetHoseFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1WethoseFees.GetValue<decimal>();
            if (feeFromCsv > 0 && !manualInvoiceViewModel.IsFTL)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.WetHoseFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.WetHoseFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //FreightFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1FreightFees.GetValue<decimal>();
            if (feeFromCsv > 0 && manualInvoiceViewModel.IsFTL)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.FreightFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.FreightFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //SurchargeFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1SurchargeFees.GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.SurchargeFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //LoadFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1LoadFees.GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.LoadFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.LoadFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //Envrironmental fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1EnvironmentalFees.GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.EnvironmentalFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.EnvironmentalFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //ServiceFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1ServiceFees.GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.ServiceFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.ServiceFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //OverWaterFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1OverWaterFees.GetValue<decimal>();
            if (feeFromCsv > 0 && !manualInvoiceViewModel.IsFTL)
            {
                var feeObjFromOrder = manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId.Equals(((int)FeeType.OverWaterFee).ToString())).FirstOrDefault();
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromCsv;
                }
                else
                {
                    AddFuelReqFeeViewModel((int)FeeType.OverWaterFee, feeFromCsv, manualInvoiceViewModel);
                }
            }

            //OtherFee fee
            feeFromCsv = csvRecord.CsvViewModel.Drop1OtherFees.GetValue<decimal>();
            if (feeFromCsv > 0)
            {
                AddFuelReqFeeViewModel((int)FeeType.OtherFee, feeFromCsv, manualInvoiceViewModel);
            }
        }

        private void AddFuelReqFeeViewModel(int feeType, decimal totalFee, ManualInvoiceViewModel manualInvoiceViewModel, decimal? feeSubQuantity = null)
        {
            int feeSubType = (int)FeeSubType.FlatFee;

            var feeViewModel = new FeesViewModel()
            {
                FeeTypeId = feeType.ToString(),
                FeeSubTypeId = feeSubType,
                Fee = totalFee,
                FeeSubQuantity = feeSubQuantity,
                TruckLoadType = manualInvoiceViewModel.IsFTL ? (int)TruckLoadTypes.FullTruckLoad : (int)TruckLoadTypes.LessTruckLoad,
                CommonFee = true,
                Currency = manualInvoiceViewModel.Currency,
                UoM = manualInvoiceViewModel.UoM,
            };

            if (feeType == (int)FeeType.OtherFee)
            {
                feeViewModel.CommonFee = false;
                feeViewModel.OtherFeeDescription = "bulk other fee";
            }

            manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Add(feeViewModel);
        }

        private int GetAssetId(AssetDropViewModel assetDrop, int buyerCompanyId)
        {
            var asset = Context.DataContext.Assets
                            .Where(t => (t.AssetAdditionalDetail.VehicleId.ToLower().Equals(assetDrop.AssetName.ToLower())
                                    || t.Name.ToLower().Equals(assetDrop.AssetName.ToLower()))
                                    && t.CompanyId == buyerCompanyId && t.IsActive)
                            .Select(t => t.Id)
                            .FirstOrDefault();
            return asset;
        }



        private List<InvoiceBulkViewModel> GetInvoiceListToGenerate(List<InvoiceBulkCsvViewModel> csvInvoiceList, UserContext context)
        {
            var invoicesToCreate = new List<InvoiceBulkViewModel>();
            var originalcsvList = csvInvoiceList;
            foreach (var item in csvInvoiceList)
            {
                if (CheckIfDryRunInvoice(item))
                {
                    var invoiceToGenerate = new InvoiceBulkViewModel();
                    invoiceToGenerate.CsvViewModel = item;
                    invoiceToGenerate.IsDryRunInvoice = true;
                    invoicesToCreate.Add(invoiceToGenerate);
                }
                else
                {
                    ProcessCSVRecordForGeneralInvoices(invoicesToCreate, originalcsvList, item, context);
                }
            }

            return invoicesToCreate;
        }

        private static void ProcessCSVRecordForGeneralInvoices(List<InvoiceBulkViewModel> invoicesToCreate, List<InvoiceBulkCsvViewModel> originalcsvList, InvoiceBulkCsvViewModel item, UserContext userContext = null)
        {
            
            if (!invoicesToCreate.Exists(t => t.CsvViewModel.PONumber.Trim() == item.PONumber.Trim()
                                && t.CsvViewModel.Drop1TicketNumber.Equals(item.Drop1TicketNumber.Trim()) && t.AssetDropList.Count > 1))
            {
                var invoiceToGenerate = new InvoiceBulkViewModel();
                invoiceToGenerate.CsvViewModel = item;

                if (!string.IsNullOrWhiteSpace(invoiceToGenerate.CsvViewModel.Drop2Quantity) || !string.IsNullOrWhiteSpace(invoiceToGenerate.CsvViewModel.Drop3Quantity) || !string.IsNullOrWhiteSpace(invoiceToGenerate.CsvViewModel.Drop4Quantity))
                {
                    //split load invoice
                    invoiceToGenerate.IsSplitLoadInvoice = true;
                }

                if (!string.IsNullOrWhiteSpace(item.Drop1AssetId))
                {
                    var assetdrop = new AssetDropViewModel();
                    assetdrop.Id = 0;
                    assetdrop.AssetName = item.Drop1AssetId;
                    assetdrop.VehicleId = item.Drop1AssetId;
                    assetdrop.DropGallons = item.Drop1Quantity.GetValue<decimal>();
                    assetdrop.StartTime = item.Drop1ArrivalTime;
                    assetdrop.EndTime = item.Drop1CompleteTime;
                    assetdrop.PreDip = item.Drop1AssetPreDip.GetValue<decimal>();
                    assetdrop.PostDip = item.Drop1AssetPostDip.GetValue<decimal>();

                    invoiceToGenerate.AssetDropList.Add(assetdrop);

                    if (!string.IsNullOrWhiteSpace(item.Drop1TicketNumber))
                    {
                        var sameTicketNumbersDrop = originalcsvList.Where(t => t.Drop1TicketNumber == item.Drop1TicketNumber && t.PONumber.Trim() == item.PONumber.Trim())
                                                    .GroupBy(t => t.Drop1TicketNumber)
                                                    .SelectMany(t => t).ToList();

                        decimal assetTotalDrop = item.Drop1Quantity.GetValue<decimal>();
                        foreach (var asset in sameTicketNumbersDrop)
                        {
                            if (asset.Drop1AssetId != item.Drop1AssetId && item.PONumber.Trim() == asset.PONumber.Trim())
                            {
                                assetdrop = new AssetDropViewModel();
                                assetdrop.Id = 0;
                                assetdrop.DropGallons = asset.Drop1Quantity.GetValue<decimal>();
                                assetdrop.AssetName = asset.Drop1AssetId;
                                assetdrop.VehicleId = asset.Drop1AssetId;
                                assetdrop.StartTime = asset.Drop1ArrivalTime;
                                assetdrop.EndTime = asset.Drop1CompleteTime;
                                assetdrop.PreDip = asset.Drop1AssetPreDip.GetValue<decimal>();
                                assetdrop.PostDip = asset.Drop1AssetPostDip.GetValue<decimal>();
                                invoiceToGenerate.AssetDropList.Add(assetdrop);
                                assetTotalDrop = assetTotalDrop + assetdrop.DropGallons ?? 0;
                            }
                        }
                        invoiceToGenerate.CsvViewModel.Drop1Quantity = assetTotalDrop.GetPreciseValue(4).ToString();
                    }
                }
                else
                {
                    var domain = new InvoiceBulkUploadDomain();
                    if (!string.IsNullOrWhiteSpace(item.PONumber))
                    {
                        // domain.Context.DataContext.Orders.Where(t=>t.PoNumber == item.PONumber.TrimStart().TrimEnd())
                        var order = domain.Context.DataContext.Orders
                                     .Where(t => t.PoNumber.ToLower().TrimEnd().TrimStart().Equals(item.PONumber.ToLower().TrimEnd().TrimStart())
                                             && t.AcceptedCompanyId == userContext.CompanyId
                                             && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                     .Select(t => new
                                     {
                                         job = t.FuelRequest.Job,
                                         IsRetailJob = t.FuelRequest.Job.IsRetailJob,
                                         IsAssetTrackingEnabled = t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                         fuelTypeId = t.FuelRequest.FuelTypeId,
                                         ProductTypeId = t.FuelRequest.MstProduct.MstProductType.Id,
                                         jobAssets = t.FuelRequest.Job.JobXAssets.Where(t1 => t1.RemovedBy == null && t1.RemovedDate == null).ToList()
                                     }).FirstOrDefault();

                        if (order != null && order.jobAssets != null && order.jobAssets.Any() && order.IsAssetTrackingEnabled)
                        {
                            var assetsOnLocation = order.jobAssets;
                            var productTypeId = order.ProductTypeId;
                            List<int> assetIds = new List<int>();
                            assetsOnLocation.ForEach(t => assetIds.Add(t.AssetId));
                            if (order.IsRetailJob) // check for tanks 
                            {
                                var assets = domain.Context.DataContext.Assets.Where(t => assetIds.Contains(t.Id) && t.FuelType == productTypeId && t.Type == (int)AssetType.Tank).ToList();
                                if (assets.Any() && assets.Count == 1)
                                {
                                    var asset = assets.FirstOrDefault();
                                    var assetdrop = new AssetDropViewModel();
                                    assetdrop.Id = asset.Id;
                                    assetdrop.AssetName = asset.Name;
                                    assetdrop.VehicleId = asset.AssetAdditionalDetail.VehicleId;
                                    assetdrop.DropGallons = item.Drop1Quantity.GetValue<decimal>();
                                    assetdrop.StartTime = item.Drop1ArrivalTime;
                                    assetdrop.EndTime = item.Drop1CompleteTime;
                                    assetdrop.PreDip = item.Drop1AssetPreDip.GetValue<decimal>();
                                    assetdrop.PostDip = item.Drop1AssetPostDip.GetValue<decimal>();

                                    invoiceToGenerate.AssetDropList.Add(assetdrop);
                                }
                            }
                            else // check for asset
                            {
                                var assets = domain.Context.DataContext.Assets.Where(t => assetIds.Contains(t.Id) && t.FuelType == productTypeId && t.Type == (int)AssetType.Asset).ToList();
                                if (assets.Any() && assets.Count == 1)
                                {
                                    var asset = assets.FirstOrDefault();
                                    var assetdrop = new AssetDropViewModel();
                                    assetdrop.Id = asset.Id;
                                    assetdrop.AssetName = asset.Name;
                                    assetdrop.VehicleId = asset.AssetAdditionalDetail.VehicleId;
                                    assetdrop.DropGallons = item.Drop1Quantity.GetValue<decimal>();
                                    assetdrop.StartTime = item.Drop1ArrivalTime;
                                    assetdrop.EndTime = item.Drop1CompleteTime;
                                    assetdrop.PreDip = item.Drop1AssetPreDip.GetValue<decimal>();
                                    assetdrop.PostDip = item.Drop1AssetPostDip.GetValue<decimal>();

                                    invoiceToGenerate.AssetDropList.Add(assetdrop);
                                }
                            }
                        }
                    }


                }
                invoicesToCreate.Add(invoiceToGenerate);
            }
        }

        #endregion

        #region ImageUPload Methods

        private QueueMessageViewModel GetQueueEventForImageUpload(UserContext userContext, string blobStoragePath, InvoiceImageType imageType, int invoiceId)
        {
            var jsonViewModel = new InvoiceImageProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.ImageType = imageType;
            jsonViewModel.InvoiceId = invoiceId;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.InvoiceImageUpload,
                JsonMessage = json
            };
        }

        private string GenerateImageName(int userId, string fileName)
        {
            return string.Concat(values: userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + Resource.lblSingleHyphen + fileName);
        }

        public async Task<StatusViewModel> UploadImageToBlob(UserContext userContext, Stream fileStream, string fileName, InvoiceImageType imageType, int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceBulkUploadDomain", "UploadImageToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateImageName(userContext.Id, fileName), BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetQueueEventForImageUpload(userContext, filePath, imageType, invoiceId);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.SFXOrderBulkUploadSuffix, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "UploadImageToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        public StatusViewModel ValidateUploadedImageFiles(UserContext userContext, string fileName)
        {
            StatusViewModel response = new StatusViewModel();
            //split name and get order details
            GetDdtTicketNoFromFileName(fileName, out string ddtTicketNo, out InvoiceImageType imageType);
            if (!string.IsNullOrWhiteSpace(ddtTicketNo) && imageType != InvoiceImageType.None)
            {
                //var invoiceExists = Context.DataContext.InvoiceXAdditionalDetails
                //                    .FirstOrDefault(t => t.DropTicketNumber.ToLower().Equals(ddtTicketNo.ToLower())
                //                                && t.CreationMethod == CreationMethod.BulkUploaded
                //                                && t.Invoice.Order.AcceptedCompanyId == userContext.CompanyId
                //                                && t.Invoice.WaitingFor == (int)WaitingAction.Images);

                var invoiceExists = Context.DataContext.Invoices
                                   .FirstOrDefault(t => t.DisplayInvoiceNumber.ToLower().Equals(ddtTicketNo.ToLower())
                                               && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded ||
                                               t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload)
                                               && t.Order.AcceptedCompanyId == userContext.CompanyId
                                               && t.WaitingFor == (int)WaitingAction.Images);
                if (invoiceExists != null)
                {
                    response.StatusCode = Status.Success;
                    response.EntityId = invoiceExists.Id;
                    response.EntityNumber = imageType.ToString();
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errInvoiceNotFoundWithDropTicketNumber, ddtTicketNo);
                }
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errInvalidFileName, fileName);
            }
            return response;
        }

        private void GetDdtTicketNoFromFileName(string fileName, out string ddtTicketNo, out InvoiceImageType imageType)
        {
            ddtTicketNo = string.Empty;
            imageType = InvoiceImageType.None;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileDetails = fileName.Split(new[] { Constants.FileNameSplitCharacter }, 2);
                //add validation for lenght-  discuss with shreyoshi
                var imgType = fileDetails[0];
                switch (imgType.ToLower())
                {
                    case "bol":
                        imageType = InvoiceImageType.Bol;
                        break;
                    case "drop":
                        imageType = InvoiceImageType.Drop;
                        break;
                    case "signature":
                        imageType = InvoiceImageType.Signature;
                        break;
                    default:
                        imageType = InvoiceImageType.None;
                        break;
                }
                if (fileDetails[1].LastIndexOf("_") > 0)
                {
                    var exactPOnumber = fileDetails[1].Substring(0, fileDetails[1].LastIndexOf("_"));
                    ddtTicketNo = exactPOnumber;
                }
            }
        }

        public string ProcessBulkImages(InvoiceImageProcessorReqViewModel imageRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("InvoiceBulkUploadDomain", "ProcessBulkImages"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                  
                    //var fileStream = azureBlob.DownloadBlob(imageRequestViewModel.FileUploadPath, BlobContainerType.InvoiceBulkUpload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(imageRequestViewModel.FileUploadPath))
                    {
                        var image = GetImageViewModel(imageRequestViewModel.FileUploadPath);
                        if (image != null)
                        {
                            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                            var userContext = authenticationDomain.GetUserContextAsync(imageRequestViewModel.SupplierId).Result;
                            //update image to invoice
                            var result = Task.Run(() => UpdateImageToDdt(image, imageRequestViewModel, userContext, errorInfo)).Result;

                            // upload image for brokered invoice
                            var brokeredInvoice = Context.DataContext.Invoices.Where(t => t.Id == imageRequestViewModel.InvoiceId && (t.BrokeredChainId != "" && t.BrokeredChainId != null))
                                                                .Select(t => new { InvoiceId = t.Id, t.OrderId, t.BrokeredChainId, t.WaitingFor })
                                                                .FirstOrDefault();
                            if (brokeredInvoice != null)
                            {
                                var invoiceDomain = new InvoiceDomain();
                                var invoiceViewModel = Task.Run(() => invoiceDomain.GetOriginalInvoiceDetails(brokeredInvoice.InvoiceId)).Result;
                                var brokerResponse = Task.Run(() => UploadImagesForBrokeredInvoices(image, imageRequestViewModel, invoiceViewModel, errorInfo)).Result;
                            }
                        }
                        else
                        {
                            processMessage.Append(Resource.errMessageFailedToReadFileContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessBulkImages", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }

        private async Task<StatusViewModel> UpdateImageToDdt(ImageViewModel image, InvoiceImageProcessorReqViewModel imageRequestViewModel, UserContext userContext, List<string> errorInfo, bool isBrokeredDDT = false)
        {
            var invoiceDomain = new InvoiceDomain(this);
            var response = new StatusViewModel(Status.Failed);
            var invoice = await Context.DataContext.Invoices.Where(t => t.Id == imageRequestViewModel.InvoiceId
                                                && t.WaitingFor == (int)WaitingAction.Images
                                                && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded
                                                    || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload)
                                                && t.Order.AcceptedCompanyId == imageRequestViewModel.SupplierCompanyId)
                                            .FirstOrDefaultAsync();

            if (invoice != null)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var needToCommit = false;

                        if (imageRequestViewModel.ImageType == InvoiceImageType.Bol)
                        {
                            needToCommit = await SetBolImageToInvoice(image, invoice.InvoiceHeaderId, needToCommit, userContext);
                        }
                        else if (imageRequestViewModel.ImageType == InvoiceImageType.Signature)
                        {
                            needToCommit = await SetSignatureImageToInvoice(image, invoice.InvoiceHeaderId, needToCommit, userContext);
                        }
                        else if (imageRequestViewModel.ImageType == InvoiceImageType.Drop)
                        {
                            needToCommit = await SetDropImageToInvoice(image, invoice.InvoiceHeaderId, needToCommit, userContext);
                        }

                        if (needToCommit)
                        {
                            Context.DataContext.Entry(invoice).State = EntityState.Modified;
                            await Context.CommitAsync();

                            errorInfo.Add("<p class='color-green'><b>" + (isBrokeredDDT ? "Brokered DDT: " : "DDT: ") + "</b> " + invoice.DisplayInvoiceNumber + " updated with " + imageRequestViewModel.ImageType.ToString() + "<br><b> successfully</b></p><br>");
                        }

                        transaction.Commit();

                        if (invoice.WaitingFor == (int)WaitingAction.Images)
                        {
                            var invoiceViewModel = await invoiceDomain.GetOriginalInvoiceDetails(invoice.Id);
                            response = await CreateInvoiceFromDdtWaitingForImagesNew(imageRequestViewModel, userContext, errorInfo, invoiceViewModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "UpdateImageToDdt", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private async Task<StatusViewModel> UploadImagesForBrokeredInvoices(ImageViewModel image, InvoiceImageProcessorReqViewModel imageRequestViewModel, InvoiceViewModelNew invoiceViewModel, List<string> errorInfo)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == imageRequestViewModel.InvoiceId && (t.BrokeredChainId != "" && t.BrokeredChainId != null))
                                                                .Select(t => new { t.OrderId, t.BrokeredChainId, t.WaitingFor })
                                                                .FirstOrDefaultAsync();
                if (invoice != null && invoice.OrderId != null)
                {
                    var invoiceCommonDomain = new InvoiceCommonDomain();
                    var brokeredOrderInfo = new List<BrokeredOrdersModel>();
                    foreach (var drop in invoiceViewModel.Drops)
                    {
                        invoiceCommonDomain.GetBrokerOrderListTillOriginalOrder(drop.OrderId, brokeredOrderInfo);
                    }

                    foreach (var brokeredOrder in brokeredOrderInfo)
                    {
                        var brokeredInvoice = await Context.DataContext.Invoices.Where(t => t.OrderId == brokeredOrder.OrderId && t.BrokeredChainId == invoice.BrokeredChainId &&
                                                                                            t.WaitingFor == (int)WaitingAction.Images && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                                                                           (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded ||
                                                                                            t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload))
                                                                                   .Select(t => new
                                                                                   {
                                                                                       InvoiceId = t.Id,
                                                                                       OrderId = t.Order.Id,
                                                                                       SupplierCompanyId = t.Order.AcceptedCompanyId,
                                                                                       SupplierId = t.Order.AcceptedBy,
                                                                                       t.WaitingFor,
                                                                                       t.DisplayInvoiceNumber,
                                                                                       t.Order.PoNumber
                                                                                   })
                                                                                .FirstOrDefaultAsync();
                        if (brokeredInvoice != null)
                        {
                            imageRequestViewModel.InvoiceId = brokeredInvoice.InvoiceId;
                            imageRequestViewModel.SupplierCompanyId = brokeredInvoice.SupplierCompanyId;
                            imageRequestViewModel.SupplierId = brokeredInvoice.SupplierId;

                            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                            var userContext = authenticationDomain.GetUserContextAsync(imageRequestViewModel.SupplierId).Result;

                            response = await UpdateImageToDdt(image, imageRequestViewModel, userContext, errorInfo, true);
                        }
                        else
                        {
                            string displayInvoiceNumber = brokeredInvoice != null ? brokeredInvoice.DisplayInvoiceNumber : string.Empty;
                            errorInfo.Add("Brokered DDT not found to upload images for DisplayInvoiceNumber: " + displayInvoiceNumber + " PoNumber: " + brokeredInvoice.PoNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "UpdateImagesForBrokeredInvoices", ex.Message, ex);
            }

            return response;
        }

        private void CreateInvoiceFromDdtWaitingForImages(InvoiceImageProcessorReqViewModel imageRequestViewModel, UserContext userContext, AuthenticationDomain authenticationDomain, List<string> errorInfo)
        {

            var invoice = Context.DataContext.Invoices.Where(t => t.Id == imageRequestViewModel.InvoiceId
                                                && t.WaitingFor == (int)WaitingAction.Images
                                                && t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded
                                                && t.Order.AcceptedCompanyId == imageRequestViewModel.SupplierCompanyId)
                                                .Select(t => new
                                                {
                                                    t.Id,
                                                    t.Image,
                                                    t.ImageId,
                                                    t.IsBolImageReq,
                                                    InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).FirstOrDefault(),
                                                    t.IsSignatureReq,
                                                    t.Signaure,
                                                    t.SignatureId,
                                                    t.IsDropImageReq,
                                                    t.Order.IsFTL,
                                                    t.SupplierPreferredInvoiceTypeId,
                                                    t.InvoiceXAdditionalDetail,
                                                    t.DisplayInvoiceNumber
                                                })
                                                .FirstOrDefault();
            if (invoice != null)
            {
                var isBolImgProvided = false;
                var isDroppImgProvided = false;
                var issiggImgProvided = false;
                var canConvertToInvoice = true;
                if (invoice.IsBolImageReq)
                {
                    if (invoice.InvoiceFtlDetail != null)
                        if (invoice.InvoiceFtlDetail.Image != null && invoice.InvoiceFtlDetail.ImageId.HasValue && invoice.InvoiceFtlDetail.ImageId.Value > 0)
                            isBolImgProvided = true;
                }

                if (invoice.IsSignatureReq)
                {
                    if (invoice.Signaure.Image != null && invoice.SignatureId.HasValue && invoice.SignatureId.Value > 0)
                        issiggImgProvided = true;
                }

                if (invoice.IsDropImageReq)
                {
                    if (invoice.Image != null && invoice.ImageId.HasValue && invoice.ImageId.Value > 0)
                        isDroppImgProvided = true;
                }

                if (invoice.IsBolImageReq && !isBolImgProvided)
                    canConvertToInvoice = false;

                if (invoice.IsSignatureReq && !issiggImgProvided)
                    canConvertToInvoice = false;

                if (invoice.IsDropImageReq && !isDroppImgProvided)
                    canConvertToInvoice = false;

                //var canConvertToInvoice = (isBolImgProvided && isDroppImgProvided && issiggImgProvided) || (!isBolImgProvided && !isDroppImgProvided && !issiggImgProvided);

                if (canConvertToInvoice && invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.Manual)
                {
                    if (invoice.IsFTL)
                    {
                        if (string.IsNullOrWhiteSpace(invoice.InvoiceXAdditionalDetail.SplitLoadChainId))
                        {
                            var processMessage = new StringBuilder();
                            try
                            {
                                var invoiceDomain = new InvoiceDomain(authenticationDomain);
                                //convert ftl ddt's to invoice
                                var viewModel = invoiceDomain.GetManualInvoiceForEditAsync(imageRequestViewModel.InvoiceId).Result;
                                viewModel.IsConvertFromDDT = true;
                                viewModel.userId = userContext.Id;
                                viewModel.WaitingForAction = (int)WaitingAction.Nothing;
                                viewModel.InvoiceTypeId = (int)InvoiceType.Manual;
                                viewModel.TaxType = TaxType.Manual;

                                var response = invoiceDomain.CreateInvoiceFromDropTicketWithBol(viewModel, userContext).Result;
                                if (response.StatusCode == Status.Success)
                                    errorInfo.Add(SetSuccessProcessMessage(viewModel.PoNumber, response));
                                else
                                {
                                    SetFailedProcessMessage(processMessage, viewModel.PoNumber, response.StatusMessage);
                                    errorInfo.Add(processMessage.ToString());
                                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!errorInfo.Any())
                                {
                                    SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, Constants.ErrorWhileProcessingBulkOrder);
                                    errorInfo.Add(processMessage.ToString());
                                }
                                LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "convert ftl ddt to invoice", "TPD image processing failed", ex);
                            }
                        }
                        else
                        {
                            var processMessage = new StringBuilder();
                            try
                            {
                                var invoiceDomain = new InvoiceDomain(authenticationDomain);
                                //convert split load ddt's to invoice
                                var splitViewModel = invoiceDomain.GetSupplierInvoiceDetailAsync(invoice.Id, userContext.CompanyId, userContext).Result;
                                var response = invoiceDomain.CreateInvoiceFromSplitLoadDropTicket(userContext, splitViewModel).Result;
                                if (response.StatusCode == Status.Success)
                                    errorInfo.Add(SetInvoiceSuccessProcessMessage(splitViewModel, response));
                                else
                                {
                                    SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, response.StatusMessage);
                                    errorInfo.Add(processMessage.ToString());
                                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!errorInfo.Any())
                                {
                                    SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, Constants.ErrorWhileProcessingBulkOrder);
                                    errorInfo.Add(processMessage.ToString());
                                }
                                LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "convert split load ddt to invoice", "TPD image processing failed", ex);
                            }
                        }
                    }
                    else
                    {
                        var processMessage = new StringBuilder();
                        try
                        {
                            var invoiceDomain = new InvoiceDomain(authenticationDomain);
                            //convert ltl ddt's to invoice
                            var viewModel = invoiceDomain.GetSupplierInvoiceDetailAsync(invoice.Id, userContext.CompanyId, userContext).Result;
                            var response = invoiceDomain.CreateInvoiceFromDropTicket(userContext, viewModel.Invoice.Id, viewModel.Invoice.InvoiceHeaderId, userContext.Id).Result;
                            if (response.StatusCode == Status.Success)
                                errorInfo.Add(SetInvoiceSuccessProcessMessage(viewModel, response));
                            else
                            {
                                SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, response.StatusMessage);
                                errorInfo.Add(processMessage.ToString());
                                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (!errorInfo.Any())
                            {
                                SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, Constants.ErrorWhileProcessingBulkOrder);
                                errorInfo.Add(processMessage.ToString());
                            }
                            LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "convert ltl ddt to invoice", "TPD image processing failed", ex);
                        }
                    }
                }
            }
        }

        public async Task<StatusViewModel> CreateInvoiceFromDdtWaitingForImagesNew(InvoiceImageProcessorReqViewModel imageRequestViewModel, UserContext userContext,
                List<string> errorInfo, InvoiceViewModelNew invoiceViewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            var consolidatedDdtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
            var invoice = await Context.DataContext.Invoices.Where(t => t.Id == imageRequestViewModel.InvoiceId
                                                && t.WaitingFor == (int)WaitingAction.Images
                                                && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded
                                                    || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload)
                                                && t.Order.AcceptedCompanyId == imageRequestViewModel.SupplierCompanyId)
                                                .Select(t => new
                                                {
                                                    t.Id,
                                                    t.Image,
                                                    t.ImageId,
                                                    t.IsBolImageReq,
                                                    InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).FirstOrDefault(),
                                                    t.IsSignatureReq,
                                                    t.Signaure,
                                                    t.SignatureId,
                                                    t.IsDropImageReq,
                                                    t.Order.IsFTL,
                                                    t.SupplierPreferredInvoiceTypeId,
                                                    t.InvoiceXAdditionalDetail,
                                                    t.DisplayInvoiceNumber
                                                })
                                                .FirstOrDefaultAsync();
            if (invoice != null)
            {
                var isBolImgProvided = false;
                var isDroppImgProvided = false;
                var issiggImgProvided = false;
                var canConvertToInvoice = true;
                if (invoice.IsBolImageReq)
                {
                    if (invoice.InvoiceFtlDetail != null)
                        if (invoice.InvoiceFtlDetail.Image != null && invoice.InvoiceFtlDetail.ImageId.HasValue && invoice.InvoiceFtlDetail.ImageId.Value > 0)
                            isBolImgProvided = true;
                }

                if (invoice.IsSignatureReq)
                {
                    if (invoice.Signaure != null && invoice.Signaure.Image != null && invoice.SignatureId.HasValue && invoice.SignatureId.Value > 0)
                        issiggImgProvided = true;
                }

                if (invoice.IsDropImageReq)
                {
                    if (invoice.Image != null && invoice.ImageId.HasValue && invoice.ImageId.Value > 0)
                        isDroppImgProvided = true;
                }

                if (invoice.IsBolImageReq && !isBolImgProvided)
                    canConvertToInvoice = false;

                if (invoice.IsSignatureReq && !issiggImgProvided)
                    canConvertToInvoice = false;

                if (invoice.IsDropImageReq && !isDroppImgProvided)
                    canConvertToInvoice = false;

                //var canConvertToInvoice = (isBolImgProvided && isDroppImgProvided && issiggImgProvided) || (!isBolImgProvided && !isDroppImgProvided && !issiggImgProvided);
                //canConvertToInvoice && invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.Manual
                if (canConvertToInvoice)
                {
                    if (string.IsNullOrWhiteSpace(invoice.InvoiceXAdditionalDetail.SplitLoadChainId))
                    {
                        var processMessage = new StringBuilder();
                        try
                        {
                            var poNumbers = invoiceViewModel.Drops.Select(t => t.PoNumber).ToList();
                            response = Task.Run(() => consolidatedDdtToInvoiceDomain.ConvertDdtToInvoiceWithBolManually(userContext, invoice.Id, invoiceViewModel)).Result;
                            if (response.StatusCode == Status.Success)
                                errorInfo.Add(SetSuccessProcessMessage(string.Join(",", poNumbers), response));
                            else
                            {
                                SetFailedProcessMessage(processMessage, string.Join(",", poNumbers), response.StatusMessage);
                                errorInfo.Add(processMessage.ToString());
                                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!errorInfo.Any())
                            {
                                SetInvoiceFailedProcessMessage(processMessage, invoice.DisplayInvoiceNumber, Constants.ErrorWhileProcessingBulkOrder);
                                errorInfo.Add(processMessage.ToString());
                            }
                            LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "convert ftl ddt to invoice", "TPD image processing failed", ex);
                        }
                    }
                }
            }
            return response;
        }


        private static void AddResponseToInfo(List<string> errorInfo, StatusViewModel response)
        {
            if (response.StatusCode == Status.Success)
                errorInfo.Add(response.EntityNumber);
            else
                errorInfo.Add(response.StatusMessage);
        }

        private async Task<bool> SetDropImageToInvoice(ImageViewModel image, int invoiceHeaderId, bool needToCommit, UserContext userContext)
        {
            var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToList();
            foreach (var invoice in invoices)
            {
                if (invoice.Image == null)
                {
                    var img = Context.DataContext.Images.Add(image.ToEntity());
                    await Context.CommitAsync();

                    invoice.Image = img;
                    needToCommit = true;
                }
            }

            return needToCommit;
        }

        private async Task<bool> SetSignatureImageToInvoice(ImageViewModel image, int invoiceHeaderId, bool needToCommit, UserContext userContext)
        {
            var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToList();
            if (invoices != null && invoices.Any())
            {
                foreach (var invoice in invoices)
                {
                    //invoice.Signaure.Image == null
                    if (invoice.Signaure == null)
                    {
                        var img = Context.DataContext.Images.Add(image.ToEntity());
                        await Context.CommitAsync();

                        invoice.Signaure = new Signature();
                        invoice.Signaure.Image = new Image();

                        invoice.Signaure.IsActive = true;
                        invoice.Signaure.SignatoryAvailable = true;
                        invoice.Signaure.Image = img;
                        needToCommit = true;
                    }
                }
            }

            return needToCommit;
        }

        public async Task<bool> SetBolImageToInvoice(ImageViewModel image, int invoiceHeaderId, bool needToCommit, UserContext userContext)
        {
            var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToList();
            foreach (var invoice in invoices)
            {
                var ftlDetail = invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault();
                if (ftlDetail != null && ftlDetail.Image == null)
                {
                    var img = Context.DataContext.Images.Add(image.ToEntity());
                    await Context.CommitAsync();

                    ftlDetail.Image = img;
                    needToCommit = true;
                }
            }

            return needToCommit;
        }

        public ImageViewModel GetImageViewModel(string filePath)
        {
            return new ImageViewModel() { FilePath = filePath };
        }

        public void AddErrorListToQueue(string validationErrors, string fileName, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var queueDomain = new QueueMessageDomain();
                var queueRequest = GetQueueEventForInvoiceBulkUploadErrors(userContext, fileName, validationErrors);
                var queueId = queueDomain.EnqeueMessage(queueRequest);
            }
        }

        private QueueMessageViewModel GetQueueEventForInvoiceBulkUploadErrors(UserContext userContext, string blobStoragePath, string errors)
        {
            var jsonViewModel = new InvoiceBulkUploadErrorProcessorViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.Errors = errors;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            var lengthOfJson = json.Length;
            if (lengthOfJson > 1024)
            {
                int extraChar = lengthOfJson - 1024;
                jsonViewModel.Errors = jsonViewModel.Errors.Replace(".... Too many errors in file", "").Substring(0, jsonViewModel.Errors.Length - extraChar - 35);
                jsonViewModel.Errors = string.Concat(jsonViewModel.Errors, ".... Too many errors in file");
            }
            json = JsonConvert.SerializeObject(jsonViewModel);
            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.InvoiceUploadErrors,
                JsonMessage = json
            };
        }

        public string ProcessBulkUploadErros(InvoiceBulkUploadErrorProcessorViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessBulkUploadErros"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.Errors))
                        {
                            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                            var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.SupplierId, CompanyType.Supplier).Result;
                            processMessage.Clear();

                            ProcessErrorList(errorInfo, bulkRequestViewModel.Errors, bulkRequestViewModel.FileUploadPath);
                        }
                        else
                        {
                            processMessage.Append(Resource.errMessageFailedToReadFileContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessBulkUploadErros", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }

        private void ProcessErrorList(List<string> errorInfo, string validationErrors, string fileUploadPath)
        {
            try
            {
                errorInfo.Add("<p class='color-maroon'><br><b>" + fileUploadPath + " processing failed Reason(s):</b>" + validationErrors + "</p><br>");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessErrorList", "Process Error List", ex);
            }
        }

        /// <summary>
        /// Remove all special characters except hyphen(-), dot(.), and underscore(_)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.-]+", "", RegexOptions.Compiled);
        }

        #endregion
    }
}
