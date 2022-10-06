using SiteFuel.BAL.Mappers;
using SiteFuel.DAL.Create;
using SiteFuel.DAL.Read;
using SiteFuel.DAL.Update;
using SiteFuel.DAL.Usp;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.CustomerException
{
    public class CustomerException
    {
        public async Task<ManageCustomerExceptionModel> GetExceptions(int ownerCompanyId, int enabledForCompanyId)
        {
            var response = new ManageCustomerExceptionModel();
            try
            {
                var spLayer = new UspLayer();
                response.OwnerCompanyId = ownerCompanyId;
                response.EnabledForCompanyId = enabledForCompanyId;
                var exceptions = await spLayer.GetCustomerExceptions(ownerCompanyId, enabledForCompanyId);
                var exceptionGroups = exceptions.GroupBy(t => t.ExceptionTypeId);
                foreach (var group in exceptionGroups)
                {
                    var groupElements = group.ToList();
                    var exception = groupElements.FirstOrDefault();
                    if (exception != null)
                    {
                        var model = exception.ToCustomerExceptionModel();
                        model.Resolutions = groupElements.Select(t => new ListItem()
                        {
                            Id = t.ResolutionId,
                            Name = t.ResolutionName
                        }).OrderBy(t => t.Id).Distinct().ToList();
                        response.Exceptions.Add(model);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<ManageCustomerExceptionModel> SaveExceptions(ManageCustomerExceptionModel model)
        {
            var response = new ManageCustomerExceptionModel();
            try
            {
                var updateLayer = new ExceptionUpdateLayer();
                var createLayer = new ExceptionCreateLayer(updateLayer);

                response.OwnerCompanyId = model.OwnerCompanyId;
                response.EnabledForCompanyId = model.EnabledForCompanyId;
                var updateRes = await updateLayer.UpdateExistingCustomerExceptions(model);
                var createRes = await createLayer.SaveNewCustomerExceptions(model, updateRes);
                //response = await GetExceptions(model.OwnerCompanyId, model.CustomerCompanyId);
                response.StatusCode = updateRes.Count == model.Exceptions.Count || createRes ? Status.Success : Status.Failed;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<bool> IsExceptionsEnabled(int ownerCompanyId, int enabledForCompanyId)
        {
            var response = false;
            try
            {
                var readLayer = new ExceptionReadLayer();
                response = await readLayer.IsCustomerExceptionsEnabled(ownerCompanyId, enabledForCompanyId);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
