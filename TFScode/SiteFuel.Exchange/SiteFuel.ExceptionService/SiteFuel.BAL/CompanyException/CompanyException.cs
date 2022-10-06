using SiteFuel.BAL.Mappers;
using SiteFuel.DAL.Create;
using SiteFuel.DAL.Read;
using SiteFuel.DAL.Update;
using SiteFuel.DAL.Usp;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common;
using SiteFuel.Models.CompanyException;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.CompanyException
{
    public class CompanyException
    {
        public async Task<ManageExceptionModel> GetExceptions(int ownerCompayId)
        {
            var response = new ManageExceptionModel();
            try
            {
                var spLayer = new UspLayer();
                response.OwnerCompanyId = ownerCompayId;
                var exceptions = await spLayer.GetCompanyExceptions(ownerCompayId);
                var exceptionGroups = exceptions.GroupBy(t => t.TypeId);
                foreach (var group in exceptionGroups)
                {
                    var groupElements = group.ToList();
                    var exception = groupElements.FirstOrDefault();
                    if (exception != null)
                    {
                        var model = exception.ToCompanyExceptionModel();
                        model.Approvers = groupElements.Select(t => new ListItem()
                        {
                            Id = t.ApproverId,
                            Name = t.ApproverName
                        }).OrderBy(t => t.Id).Distinct().ToList();
                        model.Resolutions = groupElements.Select(t => new ListItem()
                        {
                            Id = t.ResolutionId,
                            Name = t.ResolutionName
                        }).OrderBy(t => t.Id).Distinct().ToList();
                        response.Exceptions.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyException", "GetExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManageExceptionModel> SaveExceptions(ManageExceptionModel model)
        {
            var response = new ManageExceptionModel();
            try
            {
                var updateLayer = new ExceptionUpdateLayer();
                var createLayer = new ExceptionCreateLayer(updateLayer);

                response.OwnerCompanyId = model.OwnerCompanyId;
                var updateRes = await updateLayer.UpdateExistingCompanyExceptions(model);
                var createRes = await createLayer.SaveNewCompanyExceptions(model, updateRes);
                //response = await GetExceptions(model.OwnerCompanyId);
                response.StatusCode = updateRes.Count == model.Exceptions.Count || createRes ? Status.Success : Status.Failed;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<bool> IsExceptionsEnabled(int ownerCompanyId)
        {
            var response = false;
            try
            {
                var readLayer = new ExceptionReadLayer();
                response = await readLayer.IsCompanyExceptionsEnabled(ownerCompanyId);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<bool> IsExceptionsEnabledByType(int ownerCompanyId, int exceptionType)
        {
            var response = false;
            try
            {
                var readLayer = new ExceptionReadLayer();
                response = await readLayer.IsCompanyExceptionsEnabledByType(ownerCompanyId, exceptionType);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<List<EnabledExceptionModel>> GetCompaniesForEnabledException(int exceptionTypeId)
        {
            var response = new List<EnabledExceptionModel>();
            try
            {
                var readLayer = new ExceptionReadLayer();
                response = await readLayer.GetCompaniesForEnabledException(exceptionTypeId);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedId>> GetDelayInvoiceCreationTimeByCompany(List<int> companyIds)
        {
            var response = new List<DropdownDisplayExtendedId>();
            try
            {
                var readLayer = new ExceptionReadLayer();
                response = await readLayer.GetDelayInvoiceCreationTimeByCompany(companyIds);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<List<RaisedExceptionModel>> GetRaisedException(string exceptionTypeIds, int companyId, bool isBuyerCompany)
        {
            var response = new List<RaisedExceptionModel>();
            try
            {
                var spLayer = new UspLayer();
                response = await spLayer.GetRaisedExceptions(exceptionTypeIds, companyId,isBuyerCompany);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyException", "GetRaisedException", ex.Message, ex);
            }
            return response;
        }
    }
}
