using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.FileGenerator;
using System.IO;
using FileHelpers;
using SiteFuel.Exchange.DataAccess.Entities;
using System.Reflection;
using System.Globalization;
using SiteFuel.Exchange.Logger;
using System.Data.Entity;
using SiteFuel.Exchange.Domain.Mappers;

namespace SiteFuel.Exchange.Domain
{
    public class EBolDomain : BaseDomain
    {
        public EBolDomain()
        : base(ContextFactory.Current.ConnectionString)
        {
        }

        public EBolDomain(BaseDomain domain)
            : base(domain)
        {
        }
        public Stream ReadEBol()
        {
            Stream fileStream = new MemoryStream();
            var appSettings = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingEBolConfiguration).Select(t => t.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(appSettings))
            {
                EBolConfigurationJsonViewModel eBolConfiguration = JsonConvert.DeserializeObject<EBolConfigurationJsonViewModel>(appSettings);
                FtpFileDownloader ftpFileDownloder = new FtpFileDownloader();
                fileStream = ftpFileDownloder.DownloadEbolFile(eBolConfiguration);

            }
            return fileStream;
        }

        public StatusViewModel SaveEbol(string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            var engine = new FileHelperEngine<EBolFileModel>();
            var csvEBolList = engine.ReadString(csvText).ToList();
            List<EBolDetailsStaging> lstEBol = new List<EBolDetailsStaging>();
            foreach (var record in csvEBolList)
            {
                EBolDetailsStaging eBOLDetailStaging = new EBolDetailsStaging();
                bool isValid = ValidateAndSetRow(record, eBOLDetailStaging);
                if (isValid)
                {
                    eBOLDetailStaging.CreatedDate = DateTime.UtcNow;
                    eBOLDetailStaging.HexString = GetHexString(eBOLDetailStaging);
                    lstEBol.Add(eBOLDetailStaging);
                }
                else
                {
                    LogManager.Logger.WriteInfo("EBolDomain", "SaveEbol", "Invalid Records " + JsonConvert.SerializeObject(record));
                }
            }
            if (lstEBol.Any())
            {
                Context.DataContext.EBolDetailsStaging.AddRange(lstEBol);
                Context.Commit();
                SyncEBol();
                LogManager.Logger.WriteInfo("EBolDomain", "SaveEbol", "Valid Records: " + lstEBol.Count);
            }
            else
            {
                LogManager.Logger.WriteInfo("EBolDomain", "SaveEbol", "No record found in EBol");
            }
            response.StatusCode = Status.Success;
            return response;
        }

        public string GetHexString(EBolDetailsStaging eBOLDetailsStaging)
        {
            var fields = typeof(EBolDetailsStaging).GetProperties().Where(t => t.Name != "CreatedDate").ToArray();
            var result = String.Join(",", fields.Select(f => f.GetValue(eBOLDetailsStaging)));
            var hexString = string.Join("", result.Select(c => ((int)c).ToString("X2")));
            return hexString;
        }
        public void SyncEBol()
        {
            new StoredProcedureDomain(this).InsertEBolDetails();

        }
        public bool ValidateAndSetRow(EBolFileModel viewModel, EBolDetailsStaging eBOLDetailsStaging)
        {
            bool isValid = true;
            try
            {
                foreach (PropertyInfo prop in eBOLDetailsStaging.GetType().GetProperties())
                {
                    var field = viewModel.GetType().GetProperties().FirstOrDefault(t => t.Name == prop.Name);
                    if (field != null)
                    {
                        // Check Required Fields
                        var fieldValue = field.GetValue(viewModel).ToString();
                        var requiredAttr = (System.ComponentModel.DataAnnotations.RequiredAttribute[])prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false);
                        if (requiredAttr != null && requiredAttr.Any())
                        {
                            if (string.IsNullOrEmpty(fieldValue))
                            {
                                isValid = false;
                            }
                        }
                        // Check dataType conversion
                        if (isValid)
                        {
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                if (prop.PropertyType == typeof(DateTimeOffset) || prop.PropertyType == typeof(Nullable<DateTimeOffset>))
                                {

                                    DateTimeOffset value = new DateTimeOffset();
                                    if (fieldValue.Length == 8)
                                    {
                                        fieldValue = fieldValue.Insert(4, "/");
                                        fieldValue = fieldValue.Insert(2, "/");
                                    }
                                    isValid = DateTimeOffset.TryParse(fieldValue, out value);
                                    if (isValid)
                                    {
                                        prop.SetValue(eBOLDetailsStaging, value);
                                    }

                                }
                                else if (prop.PropertyType == typeof(Decimal))
                                {

                                    Decimal value = new Decimal();
                                    isValid = Decimal.TryParse(fieldValue, out value);
                                    if (isValid)
                                    {
                                        prop.SetValue(eBOLDetailsStaging, value);
                                    }

                                }
                                else
                                {
                                    prop.SetValue(eBOLDetailsStaging, fieldValue);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                isValid = false;
                throw ex;
            }
            return isValid;
        }

        public async Task<List<EBolAPIResponseModel>> GetEBol(EBolAPIRequestModel eBOLApiRequest)
        {
            var response = new List<EBolAPIResponseModel>();
            try
            {
                if (eBOLApiRequest.eBOLApiRequestList != null && eBOLApiRequest.eBOLApiRequestList.Any())
                {
                    foreach (var item in eBOLApiRequest.eBOLApiRequestList)
                    {
                        var ebolAPIResponse = new EBolAPIResponseModel();
                        ebolAPIResponse.BOLNumber = item.BOLNumber;
                        ebolAPIResponse.IsBOLNumberMatch = false;

                        var controlNumber = Context.DataContext.MstExternalTerminals.Where(t => t.Id == item.TerminalId).Select(t => t.ControlNumber).FirstOrDefault();
                        if (!string.IsNullOrEmpty(controlNumber))
                        {
                            var ebolDetails = await Context.DataContext.EBolDetails.Where(t => t.BOLNumber == item.BOLNumber && t.TerminalName == controlNumber).ToListAsync();
                            if (ebolDetails != null && ebolDetails.Any())
                            {
                                var entity = ebolDetails[0];
                                var eBolViewModel = new EBolViewModel();

                                eBolViewModel.TerminalName = entity.TerminalName;
                                eBolViewModel.BOLNumber = entity.BOLNumber;
                                eBolViewModel.GrossGallons = ebolDetails.Sum(t => t.GrossGallons);
                                eBolViewModel.NetGallons = ebolDetails.Sum(t => t.NetGallons);

                                eBolViewModel.TerminalId = item.TerminalId;
                                ebolAPIResponse.Details.Add(eBolViewModel);
                                ebolAPIResponse.IsBOLNumberMatch = true;
                            }
                        }
                        response.Add(ebolAPIResponse);
                    }

                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("EBolDomain", "GetEbol", ex.Message, ex);
            }

            return response;
        }
    }
}
