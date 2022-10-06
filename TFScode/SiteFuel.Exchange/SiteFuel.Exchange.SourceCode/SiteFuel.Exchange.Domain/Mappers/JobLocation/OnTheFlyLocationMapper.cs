using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public static class OnTheFlyLocationMapper
    {
        public static ThirdPartyOrderViewModel ToThirdPartyOrderViewModel(this OnTheFlyLocationModel model)
        {
            if (model == null)
            {
                return null;
            }
            var response = new ThirdPartyOrderViewModel() { PreferencesSetting = new OnboardingPreferenceViewModel() };
            model.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = TruckLoadTypes.FullTruckLoad;
            response.AddressDetails = model.AddressDetails;
            response.CityGroupTerminalId = model.FuelPricingDetails.CityGroupTerminalId;
            response.CustomerDetails = model.CustomerDetails;
            response.FuelDeliveryDetails = model.FuelDeliveryDetails;
            response.FuelOfferDetails.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
            response.TrailerType = new List<TrailerTypeStatus>() { TrailerTypeStatus.Lead, TrailerTypeStatus.Pup, TrailerTypeStatus.Quad, TrailerTypeStatus.Tandem, TrailerTypeStatus.Tridem };
            response.OrderAdditionalDetailsViewModel.BOLInvoicePreferenceTypes = InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile;
            response.FuelDeliveryDetails.PricingQuantityIndicatorTypeId = (int)model.FuelDetails.FuelQuantity.QuantityIndicatorTypes;
            response.FuelDetails = model.FuelDetails;
            response.PricingDetails = model.FuelPricingDetails;
            response.FuelDetails.IsTierPricing = model.FuelPricingDetails.IsTierPricingRequired;
            response.FuelDetails.TierPricing = model.FuelPricingDetails.TierPricing;
            response.PreferencesSetting.IsSupressOrderPricing = model.IsSupressOrderPricing;
            response.IsSupressOrderPricing = model.IsSupressOrderPricing;
            response.IsInvitationEnabled = model.SendInvitationLink;
            response.RegionId = model.RegionId;
            response.PreferencesSetting.Id = model.PreferenceSettingId;
            if(model.PreferenceSettingId > 0)
            {
                response.IsOnboardingPreferenceExists = true;
            }
            response.AddressDetails.DistanceCovered = model.AddressDetails.DistanceCovered;
            return response;
        }

        public static RaiseDeliveryRequestInput ToRaiseDeliveryRequestInput(this OnTheFlyLocationModel model)
        {
            if (model == null)
            {
                return null;
            }
            var response = new RaiseDeliveryRequestInput();
            response.BadgeNo1 = model.DeliveryRequest.BadgeNo1;
            response.BadgeNo2 = model.DeliveryRequest.BadgeNo2;
            response.BadgeNo3 = model.DeliveryRequest.BadgeNo3;
            response.BuyerCompanyId = model.CustomerDetails.CompanyId.Value;
            response.CreatedByRegionId = model.RegionId;
            response.CustomerCompany = model.CustomerDetails.CompanyName;
            response.DelReqSource = Utilities.DRSource.Manual;
            response.DeliveryRequestFor = Utilities.DeliveryRequestFor.Order;
            response.FuelTypeId = model.FuelDetails.FuelTypeId.Value;
            response.JobId = model.AddressDetails.JobId.Value;
            response.Notes = model.DeliveryRequest.DispatcherNote;
            response.PickupLocationType = model.DeliveryRequest.PickupLocationType;
            response.Terminal = model.DeliveryRequest.Terminal;
            response.Bulkplant = model.DeliveryRequest.Bulkplant;
            response.Priority = model.DeliveryRequest.Priority;
            response.RequiredQuantity = model.DeliveryRequest.RequiredQuantity ?? 0;
            response.ScheduleQuantityType = (int)model.DeliveryRequest.ScheduleQuantityType;
            response.SiteId = model.AddressDetails.JobName;
            response.DeliveryLevelPO = model.DeliveryRequest.DeliveryLevelPO;

            return response;
        }
    }
}
