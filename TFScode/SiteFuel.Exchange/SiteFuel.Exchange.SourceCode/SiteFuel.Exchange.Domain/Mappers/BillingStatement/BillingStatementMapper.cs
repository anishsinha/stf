using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers.BillingStatement
{
    public static class BillingStatementMapper
    {
        public static BillingSchedule ToEntity(this BillingScheduleViewModel viewModel, BillingSchedule entity = null)
        {
            if (entity == null)
            {
                entity = new BillingSchedule();
            }

            entity.Id = viewModel.Id;
            entity.BillingStatementId = string.IsNullOrWhiteSpace(viewModel.BillingStatementId) ? $"{ApplicationConstants.BillingStatementPrefix}" : viewModel.BillingStatementId.Trim();
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedByCompanyId = viewModel.CreatedByCompanyId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.CustomerId = viewModel.CustomerId;
            entity.FrequencyTypeId = viewModel.FrequencyTypeId;
            entity.PaymentTermId = viewModel.PaymentTermId;
            entity.PaymentNetDays = viewModel.PaymentNetDays;
            entity.StartDate = viewModel.StartDate;
            entity.IsActive = true;
            entity.VersionNumber = viewModel.VersionNumber;
            entity.ScheduleChainId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + viewModel.CreatedBy;
            entity.TimeZoneName = viewModel.TimeZone;

            if (viewModel.FrequencyTypeId == (int)FrequencyTypes.Weekly || viewModel.FrequencyTypeId == (int)FrequencyTypes.Biweekly)
                entity.WeekDayId = viewModel.WeekDayId;

            entity.UpdateFrequencyType = viewModel.UpdateFrequencyTypeId;
            entity.UpdateFrequencyValue = viewModel.UpdateFrequencyValue.Value;
            entity.CountryId = viewModel.CountryId;
            entity.IsIncludePreviousStatement = viewModel.IsIncludePreviousStatement;

            foreach (var item in viewModel.Orders)
            {
                BillingScheduleXCustomerOrder billingScheduleXCustomer = new BillingScheduleXCustomerOrder();
                billingScheduleXCustomer.OrderId = item;
                billingScheduleXCustomer.IsActive = true;
                billingScheduleXCustomer.BillingScheduleId = entity.Id;
                entity.BillingScheduleXCustomerOrders.Add(billingScheduleXCustomer);
            }

            return entity;
        }

        public static BillingScheduleViewModel ToViewModel(this BillingSchedule entity, BillingScheduleViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BillingScheduleViewModel();

            viewModel.Id = entity.Id;
            viewModel.BillingStatementId = entity.BillingStatementId;
            viewModel.CompanyTimeZone = entity.TimeZoneName;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedByCompanyId = entity.CreatedByCompanyId;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.CustomerId = entity.CustomerId;
            viewModel.FrequencyTypeId = entity.FrequencyTypeId;
            viewModel.IsActive = entity.IsActive;
            viewModel.Orders = entity.BillingScheduleXCustomerOrders.Select(t => t.OrderId).ToList();
            viewModel.PaymentNetDays = entity.PaymentNetDays;
            viewModel.PaymentTermId = entity.PaymentTermId;
            viewModel.ScheduleChainId = entity.ScheduleChainId;
            viewModel.StartDate = entity.StartDate;
            viewModel.TimeZone = entity.TimeZoneName;
            viewModel.VersionNumber = entity.VersionNumber;
            viewModel.WeekDayId = entity.WeekDayId;
            viewModel.UpdateFrequencyTypeId = entity.UpdateFrequencyType;
            viewModel.UpdateFrequencyValue = entity.UpdateFrequencyValue;
            viewModel.CountryId = entity.CountryId;
            viewModel.IsIncludePreviousStatement = entity.IsIncludePreviousStatement;

            return viewModel;
        }
    }
}
