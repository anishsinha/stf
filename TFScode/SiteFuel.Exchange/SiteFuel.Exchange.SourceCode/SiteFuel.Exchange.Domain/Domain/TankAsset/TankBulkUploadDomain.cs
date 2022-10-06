using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class TankBulkUploadDomain : BaseDomain
    {
        public TankBulkUploadDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TankBulkUploadDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> ValidateTankBulkFile(string csvText, string csvFilePath, UserContext context, CompanyType companyType, int jobId = 0)
        {
            using (var tracer = new Tracer("TankBulkUploadDomain", "ValidateTankBulkFile"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    response = ValidateCsvHeader(csvText, csvFilePath);
                    if (response.StatusCode == Status.Failed)
                        return response;

                    response = ValidateTankUploadFields(csvText, context, companyType);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TankBulkUploadDomain", "ValidateTankBulkFile", ex.Message, ex);
                }

                return response;
            }
        }
        public StatusViewModel ValidateCsvHeader(string csvText, string csvFilePath)
        {
            StatusViewModel response = new StatusViewModel(Status.Success);
            try
            {
                var csvHeaderLine = Regex.Matches(csvText.Trim(), @"\A.*").Cast<Match>().FirstOrDefault();
                string[] lines = File.ReadAllLines(csvFilePath);
                //header validations
                string headerLine = lines.FirstOrDefault();

                if (csvHeaderLine.Value.Trim() != headerLine)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                }
                var csvFileData = RemoveHeaderAndGuidelinesFromFile(csvText);
                if (string.IsNullOrEmpty(csvFileData))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFileEmpty;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankBulkUploadDomain", "ValidateCsvHeader", ex.Message, ex);
            }
            return response;

        }

        private StatusViewModel ValidateTankUploadFields(string csvText, UserContext context, CompanyType companyType)
        {
            StatusViewModel response = new StatusViewModel();

            csvText = RemoveHeaderAndGuidelinesFromFile(csvText);

            var engine = new FileHelperEngine<TankBulkCsvViewModel>();
            var csvTankList = engine.ReadString(csvText).ToList();
            if (csvTankList == null || !csvTankList.Any())
                return response;

            var allJobs = GetJobs(csvTankList, context, companyType);
            if (allJobs == null || !allJobs.Any())
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgJobDetailsNotfound;
                return response;
            }
            var tankModelTypes = GetTankModelType(context.CompanyId, companyType, allJobs);

            // validate tanks if multiple rows has same Pedigree Asset DB ID
            var duplicatePedigreeAssetDBID = csvTankList.Where(t => t.DipTestMethod != null && t.DipTestMethod != "" && t.DipTestMethod.ToLower().Trim() == DipTestMethod.Pedigree.GetDisplayName().ToLower() &&
                                                                    (t.PedigreeAssetDBID != null && t.PedigreeAssetDBID != ""))
                                                        .GroupBy(t => t.PedigreeAssetDBID)
                                                        .Where(grp => grp.Count() > 1)
                                                        .Select(grp => grp.Key)
                                                        .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(duplicatePedigreeAssetDBID))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMessageTankBulkUploadDuplicatePedigreeAssetIds, duplicatePedigreeAssetDBID);
                return response;
            }

            var duplicateSkyBitzTankId = csvTankList.Where(t => t.DipTestMethod != null && t.DipTestMethod != "" && t.DipTestMethod.ToLower().Trim() == DipTestMethod.Skybitz.GetDisplayName().ToLower() &&
                                                                   (t.SkyBitzRTUID != null && t.SkyBitzRTUID != ""))
                                                       .GroupBy(t => t.SkyBitzRTUID)
                                                       .Where(grp => grp.Count() > 1)
                                                       .Select(grp => grp.Key)
                                                       .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(duplicateSkyBitzTankId))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMessageTankBulkUploadDuplicateSkybitzAssetDBID, duplicateSkyBitzTankId);
                return response;
            }

            var duplicateInsightTankId = csvTankList.Where(t => t.DipTestMethod != null && t.DipTestMethod != "" && t.DipTestMethod.ToLower().Trim() == DipTestMethod.Insight360.GetDisplayName().ToLower() &&
                                                                   (t.ExternalTankID != null && t.ExternalTankID != ""))
                                                       .GroupBy(t => t.ExternalTankID)
                                                       .Where(grp => grp.Count() > 1)
                                                       .Select(grp => grp.Key)
                                                       .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(duplicateInsightTankId))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMessageTankBulkUploadDuplicateInsightTankId, duplicateInsightTankId);
                return response;
            }

            var duplicateVeederRootTank = csvTankList.Where(t => t.DipTestMethod != null && t.DipTestMethod != "" &&
                                                                   t.DipTestMethod.ToLower().Trim() == DipTestMethod.VeederRoot.GetDisplayName().ToLower() &&
                                                                   (t.ExternalTankID != null && t.ExternalTankID != "") &&
                                                                   (t.IPAddress != null && t.IPAddress != "") && (t.Port != null && t.Port != "")
                                                             )
                                                       .GroupBy(t => new { t.ExternalTankID, t.IPAddress, t.Port })
                                                       .Where(grp => grp.Count() > 1)
                                                       .Select(grp => grp.Key)
                                                       .FirstOrDefault();
            if (duplicateVeederRootTank != null)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMessageTankBulkUploadDuplicateVeederRootTankId, duplicateVeederRootTank.ExternalTankID, duplicateVeederRootTank.IPAddress, duplicateVeederRootTank.Port);
                return response;
            }

            int lineNumberOfCSV = 1;
            foreach (var record in csvTankList)
            {
                if (CheckIfItsEmptyLine(record))
                    break;

                lineNumberOfCSV++;

                //Required field validation
                if (IsRequiredFieldMissing(record))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageBulkUploadRequiredFieldsAreMissing, lineNumberOfCSV);
                    return response;
                }


                var errorList = ValidateInputParams(response, allJobs, lineNumberOfCSV, record, tankModelTypes, context);
                if (!IsJobExistsForCompany(record, context))
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMsgInvalidLocation, record.SiteName));
                    //return response;
                }
                if (errorList.Length > 0)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = errorList.ToString();
                    return response;
                }
            }

            if (lineNumberOfCSV - 1 > 0)
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = string.Format(Resource.successMessageForBulkUpload, (lineNumberOfCSV - 1));
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
            }
            return response;
        }

        public async Task<List<DipChartDetailsViewModel>> ValidateTankTypesBulkFile(string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("TankBulkUploadDomain", "ValidateTankTypesBulkFile"))
            {
                var response = new List<DipChartDetailsViewModel>();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"\A.*").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    //header validations
                    string headerLine = lines.FirstOrDefault();
                    var csvHeaderLine_ = csvHeaderLine.Value.Trim();
                    if (csvHeaderLine_ != headerLine)
                    {
                        return response;
                    }

                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);

                    var engine = new FileHelperEngine<DipChartDetailsViewModel>();
                    response = engine.ReadString(csvText).ToList();
                    //remove invalid lines
                    if (response.Any())
                    {
                        response.RemoveAll(re => (re.Dip == null || re.Dip == 0) && (re.Volume == null || re.Volume == 0) && (re.Ullage == null || re.Ullage == 0));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TankBulkUploadDomain", "ValidateTankTypesBulkFile", ex.Message, ex);
                }

                return response;
            }
        }

        private StringBuilder ValidateInputParams(StatusViewModel response, List<DropdownDisplayExtendedId> allJobs, int lineNumberOfCSV, TankBulkCsvViewModel record, List<DropdownDisplayExtended> tankModelTypes , UserContext userContext)
        {
            StringBuilder errorList = new StringBuilder();
            var job = allJobs.FirstOrDefault(t => t.Name.Trim() == record.SiteName.Trim());
            if (job == null)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidSiteName, record.SiteName));
            }
            //commented after Impediment 31805
            //if (!string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod == DipTestMethod.Manual.ToString() && string.IsNullOrWhiteSpace(record.TankMake) && string.IsNullOrWhiteSpace(record.TankModel))
            //{
            //    response.StatusCode = Status.Failed;
            //    errorList.Append(string.Format(Resource.errorMessageMakeAndModelRequiredForManualDip, record.TankName));
            //}

            if (!string.IsNullOrWhiteSpace(record.TankMake) && !string.IsNullOrWhiteSpace(record.TankModel))
            {
                var combineTankModel = tankModelTypes.FirstOrDefault(t => t.Name == record.TankMake.Trim() && t.Code == record.TankModel.Trim());
                if (combineTankModel == null)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidCombinationTankMakeAndModel, record.TankMake, record.TankModel));
                }
            }
            if (!string.IsNullOrEmpty(record.StorageTypeID) && !string.IsNullOrEmpty(record.StorageID))
            {
                bool tankAlreadyExists = Context.DataContext.AssetAdditionalDetails.Any(t => t.VehicleId.ToLower() == record.StorageTypeID.ToLower() && t.Vendor.ToLower() == record.StorageID.ToLower() && t.IsActive && t.Asset.IsActive);
                if (tankAlreadyExists)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageTankIdAlreadyExists, record.StorageTypeID, record.StorageID));
                }
            }
            bool validFuelType = Context.DataContext.MstProductTypes.Any(t => t.Name.ToLower() == record.ProductType.ToLower());
            if (!validFuelType)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidProductType, record.ProductType));
            }

            if (!string.IsNullOrEmpty(record.FuelType))
            {
                var isFuelTypeValid = Context.DataContext.MstProducts.Any(t => t.DisplayName.Equals(record.FuelType.Trim(), StringComparison.InvariantCultureIgnoreCase) && t.MstProductType.Name.Equals(record.ProductType, StringComparison.InvariantCultureIgnoreCase) && t.IsActive);
                if (!isFuelTypeValid)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidFuelType, record.FuelType));
                }
            }

            bool tankNameAlreadyExists = ValidateTankNameExists(record, job);
            if (!tankNameAlreadyExists)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageTankNameAlreadyExists, record.TankName));
            }
                        
            if (record.FillType.ToLower() == FillType.Percent.GetDisplayName().ToLower())
            {
                if (Convert.ToDecimal(record.MaxFill) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValueForPercentFillType, Resource.lblMaxFill));
                }
                if (Convert.ToDecimal(record.MinFill) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValueForPercentFillType, Resource.lblMinFill));
                }
                if (!string.IsNullOrEmpty(record.PhysicalPumpStop) && Convert.ToDecimal(record.PhysicalPumpStop) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblPhysicalPumpStop));
                }
            }
            else
            {
                if (Convert.ToDecimal(record.MaxFill) > Convert.ToDecimal(record.FuelCapacity))
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidFillGreaterThan, Resource.lblMaxFill, Resource.lblFuelCapacity));
                }
            }

            if (Convert.ToDecimal(record.MinFill) > Convert.ToDecimal(record.MaxFill))
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidFillGreaterThan, Resource.lblMinFill, Resource.lblMaxFill));
            }

            if (Convert.ToDecimal(record.RunOutLevel) > 100)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblRunOutLevel));
            }
            if (Convert.ToDecimal(record.ReOrderLevel) > 100)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblThresholdDeliveryRequest));
            }

            ValidateDecimalParameter(nameof(record.MinFill), record.MinFill, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.MaxFill), record.MaxFill, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.ReOrderLevel), record.ReOrderLevel, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.FuelCapacity), record.ReOrderLevel, lineNumberOfCSV, errorList);

            // validate water level threshold
            ValidateWaterLevelThresholdParameter(record.WaterLevelThreshold, record.MaxFill, lineNumberOfCSV, errorList);

            // validate Pedigree asset DB ID
            ValidatePedigreeAssetDBId(record, errorList, response, job);
            ValidateSkyBitzRTUID(record, errorList, response, job,userContext);
            ValidateInsightTankId(record, errorList, response, job, userContext);
            ValidateVeederRootTankId(record, errorList, response, job, userContext);

            return errorList;
        }

        private bool ValidateTankNameExists(TankBulkCsvViewModel record, DropdownDisplayExtendedId job)
        {
            var response = false;
            if (record != null && job?.CodeId > 0 && job?.Id > 0)
            {
                var asset = Context.DataContext.Assets.Where(t => t.Name.Equals(record.TankName, StringComparison.InvariantCultureIgnoreCase) && t.CompanyId == job.CodeId && t.IsActive && t.Type == (int)AssetType.Tank).FirstOrDefault();
                if (asset != null)
                {
                    asset = Context.DataContext.JobXAssets.Where(x => x.AssetId == asset.Id && x.JobId == job.Id && x.RemovedBy == null && x.Asset.Name.Equals(record.TankName, StringComparison.InvariantCultureIgnoreCase) && x.Asset.IsActive && x.Asset.CompanyId == job.CodeId).Select(x => x.Asset).FirstOrDefault();
                }
                response = (asset == null);
            }

            return response;
        }

        private void ValidatePedigreeAssetDBId(TankBulkCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.PedigreeAssetDBID))
            {
                var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Pedigree &&
                                                                  (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && 
                                                                   t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower().Trim() == record.PedigreeAssetDBID.ToLower().Trim())
                                                            ).FirstOrDefault();
                if (asset != null)
                {
                    response.StatusCode = Status.Failed;
                    errorList.AppendLine(string.Format("</br>Pedigree Asset DB ID {0} already exists for tank {1}", record.PedigreeAssetDBID, asset.Name));
                }
            }

            if(record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Pedigree.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.PedigreeAssetDBID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>Pedigree Asset DB ID is required for tank {0}", record.TankName));
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

        private void ValidateWaterLevelThresholdParameter(string waterLevel, string maxFill, int lineNumberOfCSV, StringBuilder errorList)
        {
            decimal.TryParse(waterLevel, out decimal waterLevelValue);
            decimal.TryParse(maxFill, out decimal maxFillValue);
            if (waterLevelValue < 0)
            {
                errorList.AppendLine(string.Format("</br>Water Level Threshold is invalid at line {0}", lineNumberOfCSV));
            }
            else if (waterLevelValue > maxFillValue)
            {
                errorList.AppendLine(string.Format("</br>Water Level Threshold should not be greater than Max Fill at line {0}", lineNumberOfCSV));
            }
        }

        private bool CheckIfItsEmptyLine(TankBulkCsvViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.SiteName) && string.IsNullOrWhiteSpace(record.FuelCapacity) && string.IsNullOrWhiteSpace(record.TankName) && string.IsNullOrWhiteSpace(record.ProductType) && string.IsNullOrWhiteSpace(record.StorageTypeID) && string.IsNullOrWhiteSpace(record.StorageID)
                && string.IsNullOrWhiteSpace(record.FillType) && string.IsNullOrWhiteSpace(record.MinFill) && string.IsNullOrWhiteSpace(record.MaxFill) && string.IsNullOrWhiteSpace(record.ReOrderLevel) && string.IsNullOrWhiteSpace(record.RunOutLevel);
        }

        private bool IsRequiredFieldMissing(TankBulkCsvViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.SiteName) || string.IsNullOrWhiteSpace(record.FuelCapacity) || string.IsNullOrWhiteSpace(record.TankName) || string.IsNullOrWhiteSpace(record.ProductType)
                || string.IsNullOrWhiteSpace(record.FillType) || string.IsNullOrWhiteSpace(record.MinFill) || string.IsNullOrWhiteSpace(record.MaxFill) || string.IsNullOrWhiteSpace(record.ReOrderLevel) || string.IsNullOrWhiteSpace(record.RunOutLevel);
        }

        public string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');
            return csvText;
        }
        private bool IsRequiredFieldMissing(TankBulkUploadCsvViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.Customer) || string.IsNullOrWhiteSpace(record.SiteName) || string.IsNullOrWhiteSpace(record.FuelCapacity) || string.IsNullOrWhiteSpace(record.TankName) || string.IsNullOrWhiteSpace(record.ProductType)
                || string.IsNullOrWhiteSpace(record.FillType) || string.IsNullOrWhiteSpace(record.MinFill) || string.IsNullOrWhiteSpace(record.MaxFill) || string.IsNullOrWhiteSpace(record.ReOrderLevel) || string.IsNullOrWhiteSpace(record.RunOutLevel);
        }

        private StringBuilder ValidateTPOTankInputParams(StatusViewModel response, List<DropdownDisplayExtendedId> allJobs, int lineNumberOfCSV, TankBulkUploadCsvViewModel record, List<DropdownDisplayExtended> tankModelTypes, UserContext userContext)
        {
            StringBuilder errorList = new StringBuilder();
            var job = allJobs.FirstOrDefault(t => t.Name.Trim() == record.SiteName.Trim());
            if (job == null)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidSiteName, record.SiteName));
            }

            //commented after Impediment 31805
            //if (!string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod == DipTestMethod.Manual.ToString() && string.IsNullOrWhiteSpace(record.TankMake) && string.IsNullOrWhiteSpace(record.TankModel))
            //{
            //    response.StatusCode = Status.Failed;
            //    errorList.Append(string.Format(Resource.errorMessageMakeAndModelRequiredForManualDip, record.TankName));
            //}

            if (!string.IsNullOrWhiteSpace(record.TankMake) && !string.IsNullOrWhiteSpace(record.TankModel))
            {
                var combineTankModel = tankModelTypes.FirstOrDefault(t => t.Name == record.TankMake.Trim() && t.Code == record.TankModel.Trim());
                if (combineTankModel == null)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidCombinationTankMakeAndModel, record.TankMake, record.TankModel));
                }
            }

            if (!string.IsNullOrEmpty(record.StorageTypeID) && !string.IsNullOrEmpty(record.StorageID))
            {
                bool tankAlreadyExists = Context.DataContext.AssetAdditionalDetails.Any(t => t.VehicleId.ToLower() == record.StorageTypeID.ToLower() && t.Vendor.ToLower() == record.StorageID.ToLower() && t.IsActive && t.Asset.IsActive);
                if (tankAlreadyExists)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageTankIdAlreadyExists, record.StorageTypeID, record.StorageID));
                }
            }
            bool validFuelType = Context.DataContext.MstProductTypes.Any(t => t.Name.ToLower() == record.ProductType.ToLower());
            if (!validFuelType)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidProductType, record.ProductType));
            }

            if (!string.IsNullOrEmpty(record.FuelType))
            {
                var isFuelTypeValid = Context.DataContext.MstProducts.Any(t => t.DisplayName.Equals(record.FuelType.Trim(), StringComparison.InvariantCultureIgnoreCase) && t.MstProductType.Name.Equals(record.ProductType, StringComparison.InvariantCultureIgnoreCase) && t.IsActive);
                if (!isFuelTypeValid)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidFuelType, record.FuelType));
                }
            }

            bool tankNameAlreadyExists = ValidateTankNameExists(record, job);
            if (!tankNameAlreadyExists)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageTankNameAlreadyExists, record.TankName));
            }

            if (record.FillType.ToLower() == FillType.Percent.GetDisplayName().ToLower())
            {
                if (Convert.ToDecimal(record.MaxFill) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValueForPercentFillType, Resource.lblMaxFill));
                }
                if (Convert.ToDecimal(record.MinFill) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValueForPercentFillType, Resource.lblMinFill));
                }
                if (!string.IsNullOrEmpty(record.PhysicalPumpStop) && Convert.ToDecimal(record.PhysicalPumpStop) > 100)
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblPhysicalPumpStop));
                }
            }
            else
            {
                if (Convert.ToDecimal(record.MaxFill) > Convert.ToDecimal(record.FuelCapacity))
                {
                    response.StatusCode = Status.Failed;
                    errorList.Append(string.Format(Resource.errMessageInvalidFillGreaterThan, Resource.lblMaxFill, Resource.lblFuelCapacity));
                }
            }

            if (Convert.ToDecimal(record.MinFill) > Convert.ToDecimal(record.MaxFill))
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInvalidFillGreaterThan, Resource.lblMinFill, Resource.lblMaxFill));
            }

            if (Convert.ToDecimal(record.RunOutLevel) > 100)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblRunOutLevel));
            }
            if (Convert.ToDecimal(record.ReOrderLevel) > 100)
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMessageInValidPercentValue, Resource.lblThresholdDeliveryRequest));
            }

            ValidateDecimalParameter(nameof(record.MinFill), record.MinFill, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.MaxFill), record.MaxFill, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.ReOrderLevel), record.ReOrderLevel, lineNumberOfCSV, errorList);
            ValidateDecimalParameter(nameof(record.FuelCapacity), record.ReOrderLevel, lineNumberOfCSV, errorList);

            // validate water level threshold
            ValidateWaterLevelThresholdParameter(record.WaterLevelThreshold, record.MaxFill, lineNumberOfCSV, errorList);

            // validate Pedigree asset DB ID
            ValidatePedigreeAssetDBId(record, errorList, response, job);
            ValidateSkyBitzRTUID(record, errorList, response, job, userContext);
            ValidateInsightTankId(record, errorList, response, job, userContext);
            ValidateVeederRootTankId(record, errorList, response, job, userContext);
            
            return errorList;
        }

        private bool ValidateTankNameExists(TankBulkUploadCsvViewModel record, DropdownDisplayExtendedId job)
        {
            var response = false;
            if (record != null && job?.CodeId > 0 && job?.Id > 0)
            {
                var asset = Context.DataContext.Assets.Where(t => t.Name.Equals(record.TankName, StringComparison.InvariantCultureIgnoreCase) && t.CompanyId == job.CodeId && t.IsActive && t.Type == (int)AssetType.Tank).FirstOrDefault();
                if (asset != null)
                {
                    asset = Context.DataContext.JobXAssets.Where(x => x.AssetId == asset.Id && x.JobId == job.Id && x.RemovedBy == null && x.Asset.Name.Equals(record.TankName, StringComparison.InvariantCultureIgnoreCase) && x.Asset.IsActive && x.Asset.CompanyId == job.CodeId).Select(x => x.Asset).FirstOrDefault();
                }
                response = (asset == null);
            }

            return response;
        }

        private void ValidatePedigreeAssetDBId(TankBulkUploadCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.PedigreeAssetDBID))
            {
                var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Pedigree &&
                                                                  (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" &&
                                                                   t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower().Trim() == record.PedigreeAssetDBID.ToLower().Trim())
                                                            ).FirstOrDefault();
                if (asset != null)
                {
                    response.StatusCode = Status.Failed;
                    errorList.AppendLine(string.Format("</br>Pedigree Asset DB ID {0} already exists for tank {1}", record.PedigreeAssetDBID, asset.Name));
                }
            }

            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Pedigree.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.PedigreeAssetDBID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>Pedigree Asset DB ID is required for tank {0}", record.TankName));
            }
        }


        public async Task<StatusViewModel> UploadTankFileToBlob(UserContext userContext, Stream fileStream, string fileName, CompanyType companyType, bool isTpoTank = false)
        {
            using (var tracer = new Tracer("TankBulkUploadDomain", "UploadTankFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.Id), BlobContainerType.TankBulkUpload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath, companyType, isTpoTank);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.TankBulk, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("UploadTankFileToBlob", "UploadFileToStorage", ex.Message, ex);
                }
                return response;
            }
        }

        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath, CompanyType companyType, bool isTpoTank)
        {
            var jsonViewModel = new TankBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.CompanyType = companyType;
            jsonViewModel.IsTpoTank = isTpoTank;
            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.TankBulkUpload,
                JsonMessage = json
            };
        }

        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.TankBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        public string ProcessTankBulkUploadJsonMessage(TankBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("TankBulkUploadDomain", "ProcessTankBulkUploadJsonMessage"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        var azureBlob = new AzureBlobStorage();
                        var fileStream = azureBlob.DownloadBlob(bulkRequestViewModel.FileUploadPath, BlobContainerType.TankBulkUpload.ToString().ToLower());
                        if (fileStream != null)
                        {
                            if (bulkRequestViewModel.IsTpoTank)
                            {
                                ProcessTpoTankBulkFile(bulkRequestViewModel, errorInfo, processMessage, fileStream);
                            }
                            else
                            {
                                ProcessTankBulkFile(bulkRequestViewModel, errorInfo, processMessage, fileStream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("TankBulkUploadDomain", "ProcessTankBulkUploadJsonMessage", ex.Message, ex);
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
        private void ProcessTpoTankBulkFile(TankBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo, StringBuilder processMessage, Stream fileStream)
        {
            var csvList = ReadCSVFile<TankBulkUploadCsvViewModel>(fileStream, true);
            if (csvList != null)
            {
                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.UserId, bulkRequestViewModel.CompanyType).Result;
                ProcessTPOTankList(errorInfo, csvList, context, processMessage, bulkRequestViewModel.CompanyType);
            }
            else
            {
                processMessage.Append(Resource.errMessageFailedToReadFileContent);
            }
        }

        private void ProcessTankBulkFile(TankBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo, StringBuilder processMessage, Stream fileStream)
        {
            string csvText = new StreamReader(fileStream).ReadToEnd();
            if (!string.IsNullOrWhiteSpace(csvText))
            {
                var filteredCsvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                var engine = new FileHelperEngine<TankBulkCsvViewModel>();
                var csvList = engine.ReadString(filteredCsvText).ToList();

                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.UserId, bulkRequestViewModel.CompanyType).Result;
                ProcessTankList(errorInfo, csvList, context, processMessage, bulkRequestViewModel.CompanyType);
            }
            else
            {
                processMessage.Append(Resource.errMessageFailedToReadFileContent);
            }
        }
        
        private List<DropdownDisplayExtendedId> GetJobs(List<TankBulkCsvViewModel> csvTankList, UserContext context, CompanyType companyType)
        {
            List<string> siteNames = csvTankList.Select(t => t.SiteName.Trim()).ToList();
            List<DropdownDisplayExtendedId> allJobs;
            if (companyType == CompanyType.Buyer)
            {
                allJobs = Context.DataContext.Jobs.Where(t => t.IsActive && siteNames.Contains(t.Name) && t.CompanyId == context.CompanyId).
                                Select(t => new DropdownDisplayExtendedId
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    CodeId = t.CompanyId
                                }).Distinct().ToList();
            }
            else
            {
                allJobs = Context.DataContext.Jobs.Where(t => t.IsActive && siteNames.Contains(t.Name) &&
                                t.FuelRequests.Any(t1 => t1.Orders.Any(t2 => t2.AcceptedCompanyId == context.CompanyId)))
                                .Select(t => new DropdownDisplayExtendedId
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                   // CodeId = t.CompanyId // buyercompany Id was going hence Freight service response was coming empty
                                   CodeId = context.CompanyId // changed to current user companyId (supplier)
                                }).Distinct().ToList();
            }
            return allJobs;
        }

        private List<DropdownDisplayExtended> GetTankModelType(int companyId, CompanyType companyType, List<DropdownDisplayExtendedId> alljobs)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            if (companyType == CompanyType.Buyer)
            {
                var companyIds = new List<int>();
                companyIds.Add(companyId);
                response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetTankModelType(companyIds)).Result;
            }
            else
            {
                var companyIds = alljobs.Select(t => t.CodeId).ToList();
                response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetTankModelType(companyIds)).Result;
            }
            return response;
        }

        private List<AssetViewModel> GetTankViewModel(List<TankBulkCsvViewModel> csvTankList, UserContext context, CompanyType companyType)
        {
            var response = new List<AssetViewModel>();
            if (csvTankList != null && csvTankList.Count > 0)
            {
                var allJobs = GetJobs(csvTankList, context, companyType);
                var tankModelTypes = GetTankModelType(context.CompanyId, companyType, allJobs);
                var OriginalListProducts = csvTankList.Select(t => t.ProductType.ToLower()).ToList();
                var productTypes = Context.DataContext.MstProductTypes.Where(t => OriginalListProducts.Contains(t.Name.ToLower()) && t.IsActive)
                                                                      .Select(t => new { t.Id, t.Name, Products= t.MstProducts.Where(t1 => t1.IsActive)
                                                                                     .Select(t1 => new { t1.DisplayName, t1.TfxProductId }).ToList() })
                                                                      .ToList();

                var originalList = csvTankList;
                foreach (var item in originalList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TankName))
                    {
                        var viewModel = new AssetViewModel();

                        viewModel.Name = item.TankName;
                        viewModel.AssetAdditionalDetail.TankId = item.StorageTypeID;
                        viewModel.AssetAdditionalDetail.StorageId = item.StorageID;
                        viewModel.AssetAdditionalDetail.TankNumber = item.TankNumber;
                        viewModel.AssetAdditionalDetail.Manufacturer = item.Manufacture;
                        viewModel.JobName = item.SiteName;
                        viewModel.UserId = context.Id;
                        viewModel.Type = (int)AssetType.Tank;
                        viewModel.CreatedDate = DateTimeOffset.Now;
                       
                        var productType = productTypes.FirstOrDefault(t => t.Name.ToLower() == item.ProductType.ToLower());
                        if (productType != null)
                        {
                            viewModel.FuelType.Id = productType.Id;
                            viewModel.FuelType.Name = productType.Name;
                            if (!string.IsNullOrEmpty(item.FuelType))
                            {
                                var fuelType = productType.Products.FirstOrDefault(t => !string.IsNullOrEmpty(t.DisplayName) && t.DisplayName.Equals(item.FuelType, StringComparison.InvariantCultureIgnoreCase));
                                if (fuelType != null)
                                    viewModel.AssetTankFuelTypeId = fuelType.TfxProductId;
                            }
                        }

                        var job = allJobs.FirstOrDefault(t => t.Name.Trim() == item.SiteName.Trim());
                        if (job != null)
                        {
                            viewModel.JobId = job.Id;
                        }

                        if (tankModelTypes != null && tankModelTypes.Any() && !string.IsNullOrWhiteSpace(item.TankMake))
                        {
                            var tankModelType = tankModelTypes.FirstOrDefault(t => t.Name == item.TankMake.Trim());
                            if (tankModelType != null)
                            {
                                viewModel.AssetAdditionalDetail.TankModelTypeId = tankModelType.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.ReOrderLevel))
                            viewModel.AssetAdditionalDetail.ThresholdDeliveryRequest = Convert.ToDecimal(item.ReOrderLevel);

                        if (!string.IsNullOrEmpty(item.DipTestMethod))
                        {
                            if (item.DipTestMethod.ToLower() == DipTestMethod.eFax.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.eFax;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.FranklinFuelSystem.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.FranklinFuelSystem;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Incon.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Incon;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Manual.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Manual;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Sentinal.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Sentinal;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.VeederRoot.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.VeederRoot;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Pedigree.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Pedigree;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Skybitz.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Skybitz;
                            else if (item.DipTestMethod.ToLower() == DipTestMethod.Insight360.GetDisplayName().ToLower())
                                viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Insight360;
                        }

                        if (!string.IsNullOrEmpty(item.FillType))
                            viewModel.AssetAdditionalDetail.FillType = item.FillType.ToLower() == FillType.Percent.GetDisplayName().ToLower() ? FillType.Percent : FillType.UoM;

                        if (!string.IsNullOrEmpty(item.MaxFill))
                            viewModel.AssetAdditionalDetail.MaxFill = Convert.ToDecimal(item.MaxFill);

                        if (!string.IsNullOrEmpty(item.MinFill))
                            viewModel.AssetAdditionalDetail.MinFill = Convert.ToDecimal(item.MinFill);

                        if (!string.IsNullOrEmpty(item.RunOutLevel))
                            viewModel.AssetAdditionalDetail.RunOutLevel = Convert.ToDecimal(item.RunOutLevel);

                        if (!string.IsNullOrEmpty(item.FuelCapacity))
                            viewModel.AssetAdditionalDetail.FuelCapacity = Convert.ToDecimal(item.FuelCapacity);

                        if (!string.IsNullOrEmpty(item.TankType))
                            viewModel.AssetAdditionalDetail.TankType = item.TankType.ToLower() == TankType.AboveGround.GetDisplayName() ? TankType.AboveGround : TankType.BelowGround;

                        if (!string.IsNullOrEmpty(item.PhysicalPumpStop))
                            viewModel.AssetAdditionalDetail.PhysicalPumpStop = Convert.ToDecimal(item.PhysicalPumpStop);

                        if (!string.IsNullOrEmpty(item.WaterLevelThreshold))
                            viewModel.AssetAdditionalDetail.WaterLevel = Convert.ToDecimal(item.WaterLevelThreshold);

                        if (!string.IsNullOrEmpty(item.Manifolded))
                            viewModel.AssetAdditionalDetail.ManiFolded = item.Manifolded.ToLower() == ManiFolded.Yes.GetDisplayName() ? ManiFolded.Yes : ManiFolded.No;

                        if (!string.IsNullOrEmpty(item.Construction))
                            viewModel.AssetAdditionalDetail.TankConstruction = item.Construction.ToLower() == TankConstruction.SingleWall.GetDisplayName() ? TankConstruction.SingleWall : TankConstruction.DoubleWall;

                        if (!string.IsNullOrEmpty(item.NotificationUponUsageSwing))
                            viewModel.AssetAdditionalDetail.NotificationUponUsageSwing = Convert.ToDecimal(item.NotificationUponUsageSwing);

                        if (!string.IsNullOrEmpty(item.NotificationUponUsageSwingValue))
                            viewModel.AssetAdditionalDetail.NotificationUponUsageSwingValue = Convert.ToDecimal(item.NotificationUponUsageSwingValue);

                        if (!string.IsNullOrEmpty(item.NotificationUponInventorySwingValue))
                            viewModel.AssetAdditionalDetail.NotificationUponInventorySwingValue = Convert.ToDecimal(item.NotificationUponInventorySwingValue);

                        if (!string.IsNullOrEmpty(item.NotificationUponInventorySwing))
                            viewModel.AssetAdditionalDetail.NotificationUponInventorySwing = Convert.ToDecimal(item.NotificationUponInventorySwing);

                        if (!string.IsNullOrEmpty(item.PedigreeAssetDBID))
                            viewModel.AssetAdditionalDetail.PedigreeAssetDBID = item.PedigreeAssetDBID.Trim();
                        if (!string.IsNullOrEmpty(item.SkyBitzRTUID))
                            viewModel.AssetAdditionalDetail.SkyBitzRTUID = item.SkyBitzRTUID.Trim();
                        if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.Insight360 && !string.IsNullOrEmpty(item.ExternalTankID))
                        {
                            viewModel.AssetAdditionalDetail.Insight360TankId = item.ExternalTankID.Trim();
                        }
                        if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                        {
                            if (!string.IsNullOrEmpty(item.ExternalTankID))
                                viewModel.AssetAdditionalDetail.VeederRootTankID = item.ExternalTankID.Trim();
                            if (!string.IsNullOrEmpty(item.IPAddress))
                                viewModel.AssetAdditionalDetail.VeederRootIPAddress = item.IPAddress.Trim();
                            if (!string.IsNullOrEmpty(item.Port))
                                viewModel.AssetAdditionalDetail.Port = item.Port.Trim();
                        }

                        List<int> days = new List<int>();
                        if (!string.IsNullOrEmpty(item.TankAcceptsDeliveryDays))
                        {
                            string[] tankAcceptsDelivery = item.TankAcceptsDeliveryDays.Split(',');
                            foreach (var tankAcceptitem in tankAcceptsDelivery)
                            {
                                string day = tankAcceptitem.ToLower();
                                if (day == "mon" || day == WeekDay.Monday.ToString().ToLower())
                                    days.Add((int)WeekDay.Monday);
                                else if (day == "tue" || day == WeekDay.Tuesday.ToString().ToLower())
                                    days.Add((int)WeekDay.Tuesday);
                                else if (day == "wed" || day == WeekDay.Wednesday.ToString().ToLower())
                                    days.Add((int)WeekDay.Wednesday);
                                else if (day == "thu" || day == WeekDay.Thursday.ToString().ToLower())
                                    days.Add((int)WeekDay.Thursday);
                                else if (day == "fri" || day == WeekDay.Friday.ToString().ToLower())
                                    days.Add((int)WeekDay.Friday);
                                else if (day == "sat" || day == WeekDay.Saturday.ToString().ToLower())
                                    days.Add((int)WeekDay.Saturday);
                                else if (day == "sun" || day == WeekDay.Sunday.ToString().ToLower())
                                    days.Add((int)WeekDay.Sunday);
                            }
                        }

                        viewModel.AssetAdditionalDetail.TankAcceptDelivery = days;
                        response.Add(viewModel);
                    }
                }
            }
            return response;
        }
        private void ProcessTPOTankList(List<string> errorInfo, List<TankBulkUploadCsvViewModel> csvList, UserContext context, StringBuilder processMessage, CompanyType companyType)
        {
            var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
            var thirdPartyOrderDomain = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>();
            var allJobs = GetTPOJobs(csvList, context, companyType);
            var tankModelTypes = GetTankModelType(context.CompanyId, companyType, allJobs);
            int lineNumberOfCSV = 0;
            foreach (var tank in csvList)
            {
                lineNumberOfCSV++;
                var tankName = tank.TankName.Trim();
                processMessage.Clear();
                try
                {
                    StatusViewModel result = new StatusViewModel();
                    result = ValidateTPOTankFields(tank, context, allJobs, tankModelTypes, lineNumberOfCSV);
                    if(result.StatusCode == Status.Failed)
                    {
                        SetFailedProcessMessage(processMessage, tankName, result.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                        continue;
                    }

                    var item = GetTPOTankViewModel(tank, context, tankModelTypes, allJobs);
                    if (companyType == CompanyType.Buyer)
                        result = assetDomain.SavejobXAssetAsync(context, item, item.JobId.Value).Result;
                    else
                        result = thirdPartyOrderDomain.CreateAssetsAsync(context, item, item.JobId.Value).Result;

                    if (result.StatusCode == Status.Success)
                        errorInfo.Add(SetSuccessProcessMessage(tankName));
                    else
                    {
                        SetFailedProcessMessage(processMessage, tankName, result.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                        throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        SetFailedProcessMessage(processMessage, tankName, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("TankBulkUploadDomain", "ProcessTPOTankList", "Tank bulkupload failed", ex);
                }
            }
        }
        private AssetViewModel GetTPOTankViewModel(TankBulkUploadCsvViewModel item, UserContext context, List<DropdownDisplayExtended> tankModelTypes, List<DropdownDisplayExtendedId> allJobs)
        {
            var viewModel = new AssetViewModel();
            if (item != null)
            {
                if (!string.IsNullOrWhiteSpace(item.TankName))
                {
                    viewModel.Name = item.TankName;
                    viewModel.AssetAdditionalDetail.TankId = string.IsNullOrEmpty(item.StorageTypeID) ? ApplicationConstants.TankId : item.StorageTypeID;
                    viewModel.AssetAdditionalDetail.StorageId = string.IsNullOrEmpty(item.StorageID) ? item.TankName : item.StorageID;
                    viewModel.AssetAdditionalDetail.TankNumber = item.TankNumber;
                    viewModel.AssetAdditionalDetail.Manufacturer = item.Manufacture;
                    viewModel.JobName = item.SiteName;
                    viewModel.UserId = context.Id;
                    viewModel.Type = (int)AssetType.Tank;
                    viewModel.CreatedDate = DateTimeOffset.Now;
                    var productType = Context.DataContext.MstProductTypes.Where(t => t.Name.ToLower() == item.ProductType.ToLower())
                                                                         .Select(t => new { t.Id, t.Name, FueltypeId = 
                                                                                             t.MstProducts.Where( t1 => t1.DisplayName.Equals(item.FuelType, StringComparison.InvariantCultureIgnoreCase))
                                                                                            .Select(t1 => t1.TfxProductId).FirstOrDefault() })
                                                                         .FirstOrDefault();
                    if (productType != null)
                    {
                        viewModel.FuelType.Id = productType.Id;
                        viewModel.FuelType.Name = productType.Name;
                        viewModel.AssetTankFuelTypeId = productType.FueltypeId > 0 ? productType.FueltypeId : (int?)null;
                    }
                    var job = allJobs.FirstOrDefault(t => t.Name.Trim() == item.SiteName.Trim());
                    if (job != null)
                    {
                        viewModel.JobId = job.Id;
                    }

                    if (tankModelTypes != null && tankModelTypes.Any() && !string.IsNullOrWhiteSpace(item.TankMake))
                    {
                        var tankModelType = tankModelTypes.FirstOrDefault(t => t.Name == item.TankMake.Trim());
                        if (tankModelType != null)
                        {
                            viewModel.AssetAdditionalDetail.TankModelTypeId = tankModelType.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.ReOrderLevel))
                        viewModel.AssetAdditionalDetail.ThresholdDeliveryRequest = Convert.ToDecimal(item.ReOrderLevel);

                    if (!string.IsNullOrEmpty(item.DipTestMethod))
                    {
                        if (item.DipTestMethod.ToLower() == DipTestMethod.eFax.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.eFax;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.FranklinFuelSystem.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.FranklinFuelSystem;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Incon.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Incon;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Manual.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Manual;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Sentinal.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Sentinal;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.VeederRoot.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.VeederRoot;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Pedigree.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Pedigree;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Skybitz.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Skybitz;
                        else if (item.DipTestMethod.ToLower() == DipTestMethod.Insight360.GetDisplayName().ToLower())
                            viewModel.AssetAdditionalDetail.DipTestMethod = DipTestMethod.Insight360;
                    }

                    if (!string.IsNullOrEmpty(item.FillType))
                        viewModel.AssetAdditionalDetail.FillType = item.FillType.ToLower() == FillType.Percent.GetDisplayName().ToLower() ? FillType.Percent : FillType.UoM;

                    if (!string.IsNullOrEmpty(item.MaxFill))
                        viewModel.AssetAdditionalDetail.MaxFill = Convert.ToDecimal(item.MaxFill);

                    if (!string.IsNullOrEmpty(item.MinFill))
                        viewModel.AssetAdditionalDetail.MinFill = Convert.ToDecimal(item.MinFill);

                    if (!string.IsNullOrEmpty(item.RunOutLevel))
                        viewModel.AssetAdditionalDetail.RunOutLevel = Convert.ToDecimal(item.RunOutLevel);

                    if (!string.IsNullOrEmpty(item.FuelCapacity))
                        viewModel.AssetAdditionalDetail.FuelCapacity = Convert.ToDecimal(item.FuelCapacity);

                    if (!string.IsNullOrEmpty(item.TankType))
                        viewModel.AssetAdditionalDetail.TankType = item.TankType.ToLower() == TankType.AboveGround.GetDisplayName() ? TankType.AboveGround : TankType.BelowGround;

                    if (!string.IsNullOrEmpty(item.PhysicalPumpStop))
                        viewModel.AssetAdditionalDetail.PhysicalPumpStop = Convert.ToDecimal(item.PhysicalPumpStop);

                    if (!string.IsNullOrEmpty(item.WaterLevelThreshold))
                        viewModel.AssetAdditionalDetail.WaterLevel = Convert.ToDecimal(item.WaterLevelThreshold);

                    if (!string.IsNullOrEmpty(item.Manifolded))
                        viewModel.AssetAdditionalDetail.ManiFolded = item.Manifolded.ToLower() == ManiFolded.Yes.GetDisplayName() ? ManiFolded.Yes : ManiFolded.No;

                    if (!string.IsNullOrEmpty(item.Construction))
                        viewModel.AssetAdditionalDetail.TankConstruction = item.Construction.ToLower() == TankConstruction.SingleWall.GetDisplayName() ? TankConstruction.SingleWall : TankConstruction.DoubleWall;

                    if (!string.IsNullOrEmpty(item.NotificationUponUsageSwing))
                        viewModel.AssetAdditionalDetail.NotificationUponUsageSwing = Convert.ToDecimal(item.NotificationUponUsageSwing);

                    if (!string.IsNullOrEmpty(item.NotificationUponUsageSwingValue))
                        viewModel.AssetAdditionalDetail.NotificationUponUsageSwingValue = Convert.ToDecimal(item.NotificationUponUsageSwingValue);

                    if (!string.IsNullOrEmpty(item.NotificationUponInventorySwingValue))
                        viewModel.AssetAdditionalDetail.NotificationUponInventorySwingValue = Convert.ToDecimal(item.NotificationUponInventorySwingValue);

                    if (!string.IsNullOrEmpty(item.NotificationUponInventorySwing))
                        viewModel.AssetAdditionalDetail.NotificationUponInventorySwing = Convert.ToDecimal(item.NotificationUponInventorySwing);

                    if (!string.IsNullOrEmpty(item.PedigreeAssetDBID))
                        viewModel.AssetAdditionalDetail.PedigreeAssetDBID = item.PedigreeAssetDBID.Trim();
                    if (!string.IsNullOrEmpty(item.SkyBitzRTUID))
                        viewModel.AssetAdditionalDetail.SkyBitzRTUID = item.SkyBitzRTUID.Trim();
                    if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.Insight360 && !string.IsNullOrEmpty(item.ExternalTankID))
                    {
                        viewModel.AssetAdditionalDetail.Insight360TankId = item.ExternalTankID.Trim();
                    }
                    if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                    {
                        if (!string.IsNullOrEmpty(item.ExternalTankID))
                            viewModel.AssetAdditionalDetail.VeederRootTankID = item.ExternalTankID.Trim();
                        if (!string.IsNullOrEmpty(item.IPAddress))
                            viewModel.AssetAdditionalDetail.VeederRootIPAddress = item.IPAddress.Trim();
                        if (!string.IsNullOrEmpty(item.Port))
                            viewModel.AssetAdditionalDetail.Port = item.Port.Trim();
                    }

                    List<int> days = new List<int>();
                    if (!string.IsNullOrEmpty(item.TankAcceptsDeliveryDays))
                    {
                        string[] tankAcceptsDelivery = item.TankAcceptsDeliveryDays.Split(',');
                        foreach (var tankAcceptitem in tankAcceptsDelivery)
                        {
                            string day = tankAcceptitem.ToLower();
                            if (day == "mon" || day == WeekDay.Monday.ToString().ToLower())
                                days.Add((int)WeekDay.Monday);
                            else if (day == "tue" || day == WeekDay.Tuesday.ToString().ToLower())
                                days.Add((int)WeekDay.Tuesday);
                            else if (day == "wed" || day == WeekDay.Wednesday.ToString().ToLower())
                                days.Add((int)WeekDay.Wednesday);
                            else if (day == "thu" || day == WeekDay.Thursday.ToString().ToLower())
                                days.Add((int)WeekDay.Thursday);
                            else if (day == "fri" || day == WeekDay.Friday.ToString().ToLower())
                                days.Add((int)WeekDay.Friday);
                            else if (day == "sat" || day == WeekDay.Saturday.ToString().ToLower())
                                days.Add((int)WeekDay.Saturday);
                            else if (day == "sun" || day == WeekDay.Sunday.ToString().ToLower())
                                days.Add((int)WeekDay.Sunday);
                        }
                    }
                    viewModel.AssetAdditionalDetail.TankAcceptDelivery = days;
                }
            }
            return viewModel;
        }
        private List<DropdownDisplayExtendedId> GetTPOJobs(List<TankBulkUploadCsvViewModel> csvTankList, UserContext context, CompanyType companyType)
        {
            List<string> siteNames = csvTankList.Select(t => t.SiteName.Trim().ToLower()).ToList();
            List<string> customers = csvTankList.Select(t => t.Customer.Trim().ToLower()).ToList();

            List<DropdownDisplayExtendedId> allJobs = (from J in Context.DataContext.Jobs
                                                       where siteNames.Contains(J.Name.ToLower()) && customers.Contains(J.Company.Name) &&
                                                       J.IsActive == true && J.FuelRequests.Any(t1 => t1.Orders.Any(t2 => t2.AcceptedCompanyId == context.CompanyId))
                                                       select (new DropdownDisplayExtendedId
                                                       {
                                                           Id = J.Id,
                                                           Name = J.Name,
                                                           CodeId = context.CompanyId // changed to current user companyId (supplier)
                                                       })).Distinct().ToList();

            return allJobs;
        }

        private StatusViewModel ValidateTPOTankFields(TankBulkUploadCsvViewModel csvTank, UserContext context, List<DropdownDisplayExtendedId> allJobs, List<DropdownDisplayExtended> tankModelTypes, int lineNumberOfCSV)
        {
            StatusViewModel response = new StatusViewModel(Status.Success);

            //Required field validation
            if (IsRequiredFieldMissing(csvTank))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMsgRequiredFieldAreMissing, lineNumberOfCSV);
                return response;
            }

            var errorList = ValidateTPOTankInputParams(response, allJobs, lineNumberOfCSV, csvTank, tankModelTypes, context);
            if (!IsJobExistsForCompany(csvTank, context))
            {
                response.StatusCode = Status.Failed;
                errorList.Append(string.Format(Resource.errMsgInvalidLocation, csvTank.SiteName));
            }
            if (errorList.Length > 0)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = errorList.ToString();
                return response;
            }
            return response;
        }

        private void ProcessTankList(List<string> errorInfo, List<TankBulkCsvViewModel> csvList, UserContext context, StringBuilder processMessage, CompanyType companyType)
        {
            try
            {
                var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                var thirdPartyOrderDomain = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>();
                List<AssetViewModel> viewModel = GetTankViewModel(csvList, context, companyType);

                foreach (var item in viewModel)
                {
                    var tankName = item.Name.Trim();
                    processMessage.Clear();

                    try
                    {
                        StatusViewModel result = new StatusViewModel();
                        if (companyType == CompanyType.Buyer)
                            result = assetDomain.SavejobXAssetAsync(context, item, item.JobId.Value).Result;
                        else
                            result = thirdPartyOrderDomain.CreateAssetsAsync(context, item, item.JobId.Value).Result;

                        if (result.StatusCode == Status.Success)
                            errorInfo.Add(SetSuccessProcessMessage(tankName));
                        else
                        {
                            SetFailedProcessMessage(processMessage, tankName, result.StatusMessage);
                            errorInfo.Add(processMessage.ToString());
                            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!errorInfo.Any())
                        {
                            SetFailedProcessMessage(processMessage, tankName, Constants.ErrorWhileProcessingBulkOrder);
                            errorInfo.Add(processMessage.ToString());
                        }
                        LogManager.Logger.WriteException("TankBulkUploadDomain", "ProcessTankList", "Tank bulkupload failed", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankBulkUploadDomain", "ProcessTankList", "Tank bulkupload failed", ex);
            }
        }

        private static void SetFailedProcessMessage(StringBuilder processMessage, string tankName, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Info: </b>")
                        .Append($"Tank Name: {tankName} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        private static string SetSuccessProcessMessage(string tankName)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Info: </b>")
                        .Append($"Tank Name: {tankName} <br><b>processed successfully</b></p><br>");
            return processMessage.ToString();
        }

        public bool IsValidTankId(int jobId, int id, string storageTypeId, string storageId)
        {
            var response = false;

            try
            {
                var tank = Context.DataContext.AssetAdditionalDetails.FirstOrDefault(t => t.Asset.JobXAssets.Any(t1 => t1.Asset.Id != id && t1.JobId == jobId && t1.RemovedBy == null && t1.Asset.IsActive
                            && t1.Asset.AssetAdditionalDetail.VehicleId.ToLower() == storageTypeId.ToLower() && t1.Asset.AssetAdditionalDetail.Vendor.ToLower() == storageId.ToLower() && t1.Asset.Type == (int)AssetType.Tank));
                response = tank == null;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankBulkUploadDomain", "IsValidTankId", ex.Message, ex);
            }

            return response;
        }

        private bool IsJobExistsForCompany(TankBulkCsvViewModel record,UserContext context)
        {

            if (record != null && !string.IsNullOrWhiteSpace(record.SiteName))
            {
                var siteName = record.SiteName.Trim();
                var job = Context.DataContext.Jobs.Where(t => t.Name.Trim() == siteName && t.CreatedByCompanyId == context.CompanyId && t.IsActive).FirstOrDefault();
                if (job == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void ValidateSkyBitzRTUID(TankBulkCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job,UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.SkyBitzRTUID))
            {
               
                if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var  asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Skybitz &&
                                                                (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" &&
                                                                 t.AssetAdditionalDetail.SkyBitzRTUID.ToLower().Trim() == record.SkyBitzRTUID.ToLower().Trim())
                                                          ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>SkybitzRTUID {0} already exists for tank {1}", record.SkyBitzRTUID, asset.Name));
                    }
                }
                else 
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                   var  asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Skybitz &&
                                                              (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" &&
                                                               t.AssetAdditionalDetail.SkyBitzRTUID.ToLower().Trim() == record.SkyBitzRTUID.ToLower().Trim())
                                                        ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>SkybitzRTUID {0} already exists for tank {1}", record.SkyBitzRTUID, asset.Name));
                    }

                }                             
            }

            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Skybitz.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.SkyBitzRTUID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>SkybitzRTUID is required for tank {0}", record.TankName));
            }
        }

        private void ValidateInsightTankId(TankBulkCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job, UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.ExternalTankID) 
                && !string.IsNullOrWhiteSpace(record.DipTestMethod) 
                && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Insight360.GetDisplayName().ToLower())
            {

                if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Insight360 &&
                                                               (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim())
                                                          ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID {0} already exists for tank {1}", record.ExternalTankID, asset.Name));
                    }
                }
                else
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Insight360 &&
                                                              (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                               t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim())
                                                         ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID {0} already exists for tank {1}", record.ExternalTankID, asset.Name));
                    }

                }
            }

            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Insight360.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.ExternalTankID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>ExternalTankID is required for tank {0}", record.TankName));
            }
        }

        private void ValidateVeederRootTankId(TankBulkCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job, UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod)
                && record.DipTestMethod.ToLower().Trim() == DipTestMethod.VeederRoot.GetDisplayName().ToLower())
            {
                if((record.ExternalTankID == null || record.ExternalTankID == "") ||
                   (record.IPAddress == null || record.IPAddress == "") ||
                   (record.Port == null || record.Port == ""))
                {
                    response.StatusCode = Status.Failed;
                    errorList.AppendLine(string.Format("</br>ExternalTankID, IPAddress and Port are required for tank {0}", record.TankName));
                }
                else if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && 
                                                                      t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.VeederRoot &&
                                                                      (
                                                                       t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                       t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress != null && t.AssetAdditionalDetail.VeederRootIPAddress != "" &&
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress.Trim() == record.IPAddress.Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.Port != null && t.AssetAdditionalDetail.Port != "" &&
                                                                       t.AssetAdditionalDetail.Port.Trim() == record.Port.Trim()
                                                                      )
                                                                )
                                                           .Select(t => new { t.Name })
                                                           .FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID - {0}, IPAddress - {1} and Port - {2} already exists for tank {3}", record.ExternalTankID, record.IPAddress, record.Port, asset.Name));
                    }
                }
                else
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId &&
                                                                       t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.VeederRoot &&
                                                                      (
                                                                       t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                       t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress != null && t.AssetAdditionalDetail.VeederRootIPAddress != "" &&
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress.Trim() == record.IPAddress.Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.Port != null && t.AssetAdditionalDetail.Port != "" &&
                                                                       t.AssetAdditionalDetail.Port.Trim() == record.Port.Trim()
                                                                      )
                                                                )
                                                           .Select(t => new { t.Name })
                                                           .FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID - {0}, IPAddress - {1} and Port - {2} already exists for tank {3}", record.ExternalTankID, record.IPAddress, record.Port, asset.Name));
                    }
                }
            }
        }


        private bool IsJobExistsForCompany(TankBulkUploadCsvViewModel record, UserContext context)
        {

            if (record != null && !string.IsNullOrWhiteSpace(record.SiteName))
            {
                var siteName = record.SiteName.Trim();
                var job = Context.DataContext.Jobs.Where(t => t.Name.Trim() == siteName && t.CreatedByCompanyId == context.CompanyId && t.IsActive).FirstOrDefault();
                if (job == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void ValidateSkyBitzRTUID(TankBulkUploadCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job, UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.SkyBitzRTUID))
            {

                if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Skybitz &&
                                                               (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" &&
                                                                t.AssetAdditionalDetail.SkyBitzRTUID.ToLower().Trim() == record.SkyBitzRTUID.ToLower().Trim())
                                                          ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>SkybitzRTUID {0} already exists for tank {1}", record.SkyBitzRTUID, asset.Name));
                    }
                }
                else
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Skybitz &&
                                                              (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" &&
                                                               t.AssetAdditionalDetail.SkyBitzRTUID.ToLower().Trim() == record.SkyBitzRTUID.ToLower().Trim())
                                                         ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>SkybitzRTUID {0} already exists for tank {1}", record.SkyBitzRTUID, asset.Name));
                    }

                }
            }

            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Skybitz.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.SkyBitzRTUID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>SkybitzRTUID is required for tank {0}", record.TankName));
            }
        }

        private void ValidateInsightTankId(TankBulkUploadCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job, UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.ExternalTankID)
                && !string.IsNullOrWhiteSpace(record.DipTestMethod)
                && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Insight360.GetDisplayName().ToLower())
            {

                if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Insight360 &&
                                                               (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim())
                                                          ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID {0} already exists for tank {1}", record.ExternalTankID, asset.Name));
                    }
                }
                else
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId && t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.Insight360 &&
                                                              (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                               t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim())
                                                         ).FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID {0} already exists for tank {1}", record.ExternalTankID, asset.Name));
                    }

                }
            }

            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod) && record.DipTestMethod.ToLower().Trim() == DipTestMethod.Insight360.GetDisplayName().ToLower() && string.IsNullOrWhiteSpace(record.ExternalTankID))
            {
                response.StatusCode = Status.Failed;
                errorList.AppendLine(string.Format("</br>ExternalTankID is required for tank {0}", record.TankName));
            }
        }

        private void ValidateVeederRootTankId(TankBulkUploadCsvViewModel record, StringBuilder errorList, StatusViewModel response, DropdownDisplayExtendedId job, UserContext context)
        {
            if (record != null && !string.IsNullOrWhiteSpace(record.DipTestMethod)
                && record.DipTestMethod.ToLower().Trim() == DipTestMethod.VeederRoot.GetDisplayName().ToLower())
            {
                if ((record.ExternalTankID == null || record.ExternalTankID == "") ||
                   (record.IPAddress == null || record.IPAddress == "") ||
                   (record.Port == null || record.Port == ""))
                {
                    response.StatusCode = Status.Failed;
                    errorList.AppendLine(string.Format("</br>ExternalTankID, IPAddress and Port are required for tank {0}", record.TankName));
                }
                else if (context.CompanyTypeId == CompanyType.Buyer)
                {
                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == job.CodeId &&
                                                                      t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.VeederRoot &&
                                                                      (
                                                                       t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                       t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress != null && t.AssetAdditionalDetail.VeederRootIPAddress != "" &&
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress.Trim() == record.IPAddress.Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.Port != null && t.AssetAdditionalDetail.Port != "" &&
                                                                       t.AssetAdditionalDetail.Port.Trim() == record.Port.Trim()
                                                                      )
                                                                )
                                                           .Select(t => new { t.Name })
                                                           .FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID - {0}, IPAddress - {1} and Port - {2} already exists for tank {3}", record.ExternalTankID, record.IPAddress, record.Port, asset.Name));
                    }
                }
                else
                {
                    var companyId = Context.DataContext.Jobs.Where(t => t.Id == job.Id).Select(t => t.CompanyId).FirstOrDefault();

                    var asset = Context.DataContext.Assets.Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.CompanyId == companyId &&
                                                                       t.AssetAdditionalDetail.DipTestMethod == (int)DipTestMethod.VeederRoot &&
                                                                      (
                                                                       t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                       t.AssetAdditionalDetail.ExternalTankId.ToLower().Trim() == record.ExternalTankID.ToLower().Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress != null && t.AssetAdditionalDetail.VeederRootIPAddress != "" &&
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress.Trim() == record.IPAddress.Trim()
                                                                      ) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.Port != null && t.AssetAdditionalDetail.Port != "" &&
                                                                       t.AssetAdditionalDetail.Port.Trim() == record.Port.Trim()
                                                                      )
                                                                )
                                                           .Select(t => new { t.Name })
                                                           .FirstOrDefault();
                    if (asset != null)
                    {
                        response.StatusCode = Status.Failed;
                        errorList.AppendLine(string.Format("</br>ExternalTankID - {0}, IPAddress - {1} and Port - {2} already exists for tank {3}", record.ExternalTankID, record.IPAddress, record.Port, asset.Name));
                    }
                }
            }
        }
    }
}
