using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class FreightTableDomain : IFreightTableDomain
    {
        private IFreightTableRepository _freightTableRepository;
        public FreightTableDomain(IFreightTableRepository freightTableRepository)
        {
            _freightTableRepository = freightTableRepository;
        }

        public async Task<bool> DeleteAllRecords()
        {
            var response = false;
            try
            {
                response = await _freightTableRepository.DeleteAllRecords();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightTableDomain", "DeleteAllRecords", ex.Message, ex);
            }
            return response;
        }

        public async Task<FreightTableResponseModel> AddFreightTable(FreightTableModel table)
        {
            var response = new FreightTableResponseModel();
            try
            {
                var valResult = ValidateFreightTable(table);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response.Id = await _freightTableRepository.AddFreightTable(table);
                    response.StatusCode = (int)Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightTableDomain", "AddFreightTable", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> AddFreightTablePricings(List<FreightTablePriceModel> pricings)
        {
            var result = new StatusModel();
            try
            {
                result = await _freightTableRepository.AddFreightTablePrices(pricings);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightTableDomain", "AddFreightTablePricings", ex.Message, ex);
            }
            return result;
        }

        private ValidatationResult ValidateFreightTable(FreightTableModel table)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();
            if (table.CompanyId <= 0)
                messages.Add("CompanyId");

            if (table.CreatedOn == DateTimeOffset.MinValue)
                messages.Add("CreatedOn");

            if (table.EndDate == DateTimeOffset.MinValue || table.EndDate <= table.StartDate)
                messages.Add("EndDate");

            if (table.FuelType <= 0)
                messages.Add("FuelType");

            if (string.IsNullOrWhiteSpace(table.Name))
                messages.Add("Name");

            if (table.StartDate == DateTimeOffset.MinValue || table.StartDate >= table.EndDate)
                messages.Add("StartDate");

            if (table.Type <= 0)
                messages.Add("Type");

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }
    }
}
