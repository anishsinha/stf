using FileHelpers;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class FuelFeesDomain : BaseDomain
    {
        public FuelFeesDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FuelFeesDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task SaveFuelFees(FuelDeliveryDetailsViewModel viewModel, FuelRequest fuelRequest, UserContext userContext, bool isFuelSurcharge = false, bool isFreightCost = false)
        {
            viewModel.FuelRequestFee.FuelRequestId = fuelRequest.Id;
            List<FuelFee> fuelFees = new List<FuelFee>();
            if (viewModel.FuelFees.FuelRequestFees != null && viewModel.FuelFees.FuelRequestFees.Any())
            {
                foreach (var fee in viewModel.FuelFees.FuelRequestFees)
                {
                    fee.UoM = fuelRequest.UoM;
                    fee.Currency = fuelRequest.Currency;
                    if (!fee.CommonFee || fee.FeeTypeId.Contains(Constants.OtherCommonFeeCode))
                    {
                        SetOtherFeeDetails(fee);
                    }
                }

                await SaveOtherFee(viewModel, userContext);
                fuelFees = viewModel.FuelFees.ToEntity();
                fuelRequest.FuelRequestFees = fuelFees;
            }

            if (isFuelSurcharge)
                fuelRequest.FuelRequestFees = viewModel.FuelFees.FuelSurchargeFreightFee.ToFuelSurchargeEntity(fuelFees);

            if (isFreightCost)
                fuelRequest.FuelRequestFees = viewModel.FuelFees.FreightCostFee.ToFreightCostEntity(fuelFees);
        }

        public async Task SaveOfferFuelFees(OfferViewModel viewModel, UserContext userContext)
        {
            if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees != null && viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any())
            {
                foreach (var fee in viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees)
                {
                    if (!fee.CommonFee || fee.FeeTypeId.Contains(Constants.OtherCommonFeeCode))
                    {
                        SetOtherFeeDetails(fee);
                    }
                }

                await SaveOtherFee(viewModel.FuelDeliveryDetails, userContext);
            }
        }

        public async Task SaveQuotationFuelFees(FuelDeliveryDetailsViewModel viewModel, Quotation quote, UserContext userContext)
        {
            if (viewModel.FuelFees.FuelRequestFees != null && viewModel.FuelFees.FuelRequestFees.Any())
            {
                foreach (var fee in viewModel.FuelFees.FuelRequestFees)
                {
                    fee.UoM = quote.UoM;
                    fee.Currency = quote.Currency;
                    if (!fee.CommonFee || fee.FeeTypeId.Contains(Constants.OtherCommonFeeCode))
                    {
                        SetOtherFeeDetails(fee);
                    }
                }

                await SaveOtherFee(viewModel, userContext);
                quote.QuotationFees = viewModel.FuelFees.ToEntity();
            }

            //quote.FeeByQuantities = viewModel.FuelFees.FuelRequestFees.ToEntity();
        }

        private async Task SaveOtherFee(FuelDeliveryDetailsViewModel viewModel, UserContext userContext)
        {
            foreach (var otherFee in viewModel.FuelFees.FuelRequestFees.Where(t => !t.CommonFee))
            {
                if (!otherFee.CommonFee && otherFee.AddToCommonFees)
                {
                    var mstOtherFee = Context.DataContext.MstOtherFeeTypes;
                    var isAlreadyExist = mstOtherFee.FirstOrDefault(t => t.CompanyId == userContext.CompanyId && t.Name.Equals(otherFee.OtherFeeDescription, StringComparison.OrdinalIgnoreCase));
                    if (isAlreadyExist != null)
                    {
                        otherFee.OtherFeeTypeId = isAlreadyExist.Id;
                        continue;
                    }

                    var id = mstOtherFee.Any() ? mstOtherFee.Max(t => t.Id) + 1 : 1;
                    otherFee.CompanyId = userContext.CompanyId;
                    MstOtherFeeType fee = otherFee.ToOtherFeeTypeEntity();
                    fee.Code = Constants.OtherCommonFeeCode + "-" + id;
                    fee.UserId = userContext.Id;
                    fee.UpdatedBy = userContext.Id;
                    fee.UpdatedDate = DateTimeOffset.Now;
                    fee.IsActive = true;

                    var otherFeeType = Context.DataContext.MstOtherFeeTypes.Add(fee);
                    await Context.CommitAsync();
                    otherFee.OtherFeeTypeId = otherFeeType.Id;
                }
            }

            foreach (var otherFee in viewModel.FuelFees.FuelRequestFees.Where(t => t.FeeTypeId.Contains(Constants.OtherCommonFeeCode)))
            {
                otherFee.OtherFeeTypeId = Convert.ToInt32(otherFee.FeeTypeId.Remove(0, 4));
            }
        }

        public async Task SaveFuelFees(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualViewModel, Invoice invoice, UserContext currentUser)
        {
            if ((manualViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees != null && manualViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any())
                    || manualViewModel.FuelDeliveryDetails.FuelFees.DiscountLineItems.Any())
            {
                foreach (var fee in manualViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees)
                {
                    fee.UoM = invoice.UoM;
                    fee.Currency = invoice.Currency;
                    if ((!fee.CommonFee || fee.FeeTypeId.Contains(Constants.OtherCommonFeeCode)) && !fee.FeeTypeId.Equals(((int)FeeType.TankRental).ToString()))
                    {
                        SetOtherFeeDetails(fee);
                    }
                }

                await SaveOtherFee(invoiceViewModel, manualViewModel, currentUser);
                invoice.FuelRequestFees = manualViewModel.FuelDeliveryDetails.FuelFees.ToInvoiceFeesEntity(invoiceViewModel.DropStartDate.Date);
            }

            //FeeByQuantities Entity
            //invoice.FeeByQuantities = manualViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.ToInvoiceFeesEntity(invoiceViewModel.DropStartDate.Date);
        }

        public void CalculateAndSetTotalFeeAndQuantityToFuelFees(InvoiceModel invoiceModel, int assetCount)
        {
            var droppedQuantity = invoiceModel.DroppedGallons;
            var invoiceFees = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId == null && t.FeeTypeId != (int)FeeType.DryRunFee);
            foreach (var fee in invoiceFees)
            {
                CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount);
            }
            foreach (var discount in invoiceModel.FuelFees.Where(t => t.DiscountLineItemId != null))
            {
                CalculateTotalFeeAndQuantityForDiscount(invoiceFees, droppedQuantity, discount);
            }
        }

        public void CalculateAndSetTotalFeeAndQuantityToFuelFees(InvoiceViewModelNew consolidatedViewModel, InvoiceModel invoiceModel, int assetCount, decimal? tierPricingInvQty = null)
        {
            var droppedQuantity = consolidatedViewModel.Drops.Sum(t => t.ActualDropQuantity);
            var model = GetModelToCalculateTotalDropTime(consolidatedViewModel, invoiceModel);
            var dropTimeDiff = GetDropStartDateAndEndDateDifferenceInSeconds(model);
            var invoiceFees = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId == null && t.FeeTypeId != (int)FeeType.DryRunFee);
            foreach (var fee in invoiceFees)
            {
                if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon && fee.OrderId > 0)
                {
                    var drop = consolidatedViewModel.Drops.FirstOrDefault(t => t.OrderId == fee.OrderId);
                    if (drop != null)
                    {
                        CalculateTotalFeeAndQuantityForFee(invoiceModel, drop.ActualDropQuantity, fee, assetCount);
                    }
                }
                else
                {
                    if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.OrderId == 0)
                    {
                        CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount, dropTimeDiff);
                    }
                    else
                    {
                        CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount, 0, tierPricingInvQty);
                    }
                }
            }
            foreach (var discount in invoiceModel.FuelFees.Where(t => t.DiscountLineItemId != null))
            {
                CalculateTotalFeeAndQuantityForDiscount(invoiceFees, droppedQuantity, discount);
            }
        }


        private static InvoiceModel GetModelToCalculateTotalDropTime(InvoiceViewModelNew consolidatedViewModel, InvoiceModel invoice)
        {
            var dropDates = new List<DateTimeOffset>();
            List<AssetDropModel> assets = new List<AssetDropModel>();
            consolidatedViewModel.Drops.ForEach(t => dropDates.Add(t.DropDate.Date.Add(Convert.ToDateTime(t.StartTime).TimeOfDay)));
            consolidatedViewModel.Drops.ForEach(t => dropDates.Add(t.DropDate.Date.Add(Convert.ToDateTime(t.EndTime).TimeOfDay)));
            if (consolidatedViewModel.Drops.Any(t => t.Assets.Count > 0))
            {
                consolidatedViewModel.Drops.SelectMany(t => t.Assets).ToList().Where(t => t.DropGallons != null && t.DropGallons > 0).ToList().
                    ForEach(t => assets.Add(new AssetDropModel()
                    {
                        DropStartDate = t.DropDate.Date.Add(Convert.ToDateTime(t.StartTime).TimeOfDay),
                        DropEndDate = t.DropDate.Date.Add(Convert.ToDateTime(t.EndTime).TimeOfDay)
                    }));
            }
            return new InvoiceModel() { CreatedDate = invoice.CreatedDate, DropStartDate = dropDates.OrderBy(t => t).FirstOrDefault(), DropEndDate = dropDates.OrderByDescending(t => t).FirstOrDefault(), AssetDrops = assets };
        }

        private async Task SaveOtherFee(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, UserContext currentUser)
        {
            foreach (var otherFee in manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Where(t => !t.CommonFee))
            {
                if (!otherFee.CommonFee && otherFee.AddToCommonFees)
                {
                    var mstOtherFee = Context.DataContext.MstOtherFeeTypes;
                    var isAlreadyExist = mstOtherFee.FirstOrDefault(t => t.CompanyId == currentUser.CompanyId && t.Name.ToLower() == otherFee.OtherFeeDescription);
                    if (isAlreadyExist != null)
                    {
                        otherFee.OtherFeeTypeId = isAlreadyExist.Id;
                        continue;
                    }

                    var id = mstOtherFee.Any() ? mstOtherFee.Max(t => t.Id) + 1 : 1;
                    otherFee.CompanyId = currentUser.CompanyId;
                    MstOtherFeeType fee = otherFee.ToOtherFeeTypeEntity();
                    fee.Code = Constants.OtherCommonFeeCode + "-" + id;
                    fee.UserId = viewModel.UserId;
                    fee.UpdatedBy = viewModel.UserId;
                    fee.UpdatedDate = DateTimeOffset.Now;
                    fee.IsActive = true;

                    var otherFeeType = Context.DataContext.MstOtherFeeTypes.Add(fee);
                    await Context.CommitAsync();
                    otherFee.OtherFeeTypeId = otherFeeType.Id;
                }
            }
        }

        private void SetOtherFeeDetails(FeesViewModel fee)
        {
            var mstOtherFeeType = Context.DataContext.MstOtherFeeTypes.FirstOrDefault(t => t.Code == fee.FeeTypeId);
            if (!fee.CommonFee && mstOtherFeeType != null)
            {
                fee.OtherFeeDescription = mstOtherFeeType.Name;
                fee.OtherFeeTypeId = mstOtherFeeType.Id;
            }
            else if (fee.CommonFee)
            {
                fee.OtherFeeDescription = null;
                fee.OtherFeeTypeId = mstOtherFeeType?.Id;
            }
            fee.FeeTypeId = ((int)FeeType.OtherFee).ToString();
        }

        private double GetDropStartDateAndEndDateDifferenceInSeconds(InvoiceModel invoiceModel)
        {
            double difference;
            var applicationDomain = new ApplicationDomain(this);
            var WetHoseOverWaterFeeChangedDate = applicationDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingWetHoseFeeChangedDate);
            var WetHoseOverWaterFeeChangedDateTime = DateTimeOffset.Parse(WetHoseOverWaterFeeChangedDate);
            if (invoiceModel.CreatedDate >= WetHoseOverWaterFeeChangedDateTime)
            {
                difference = invoiceModel.DropEndDate.AddMilliseconds(-invoiceModel.DropEndDate.Millisecond)
                            .Subtract(invoiceModel.DropStartDate.AddMilliseconds(-invoiceModel.DropStartDate.Millisecond)).TotalSeconds;
            }
            else
            {
                if (invoiceModel.AssetDrops.Any())
                {
                    difference = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop)
                        .Sum(t => t.DropEndDate.AddMilliseconds(-t.DropEndDate.Millisecond).Subtract(t.DropStartDate.AddMilliseconds(-t.DropStartDate.Millisecond)).TotalSeconds);
                }
                else
                {
                    difference = invoiceModel.DropEndDate.AddMilliseconds(-invoiceModel.DropEndDate.Millisecond)
                        .Subtract(invoiceModel.DropStartDate.AddMilliseconds(-invoiceModel.DropStartDate.Millisecond)).TotalSeconds;
                }
            }

            //Below is added to allow EndTime(Next Day) and StartTime(Current Day)
            if (difference < 0)
                difference = difference + 86400;

            return difference;
        }

        public void CalculateTotalFeeAndQuantityForFee(InvoiceModel invoiceModel, decimal droppedQuantity, FuelFeeViewModel fee, int assetCount, double dropTimeDiff = 0, decimal? tierPricingInvQty = null)
        {
            if (fee.FeeTypeId == (int)FeeType.DemurrageFeeDestination || fee.FeeTypeId == (int)FeeType.DemurrageFeeTerminal || fee.FeeTypeId == (int)FeeType.DemurrageOther)
            {
                CalculateAndSetDemurrageFee(fee);
            }
            else if (fee.FeeTypeId == (int)FeeType.Retain)
            {
                CalculateAndSetFuelTruckRetainFee(fee);
            }
            else if (fee.FeeTypeId == (int)FeeType.UnderGallonFee)
            {
                CalculateAndSetUnderGallonFee(invoiceModel, fee);
            }
            else if (fee.FeeTypeId == (int)FeeType.SurchargeFreightFee || fee.FeeTypeId == (int)FeeType.FreightCost)
            {
                CalculateAndSetFuelsurchargeFee(invoiceModel, droppedQuantity, fee);
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee || fee.FeeSubTypeId == (int)FeeSubType.PerRoute)
            {
                fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee, ApplicationConstants.MarineInvoiceFeeTotalAmountDecimalDisplay): Math.Round(fee.Fee, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
            {
                CalculateAndSetByQuantityFee(droppedQuantity, fee, tierPricingInvQty);
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                CalculateAndSetPerAssetFee(fee, assetCount);
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
            {
                if (dropTimeDiff == 0)
                {
                    dropTimeDiff = GetDropStartDateAndEndDateDifferenceInSeconds(invoiceModel);
                }
                CalculateAndSetPerHourFee(dropTimeDiff, fee);
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                CalculateAndSetPerGallonFee(droppedQuantity, fee);
            }
        }

        private void CalculateAndSetFuelsurchargeFee(InvoiceModel invoiceModel, decimal droppedQuantity, FuelFeeViewModel fee)
        {
            fee.TotalFee = fee.TotalFee;
            if (fee.TotalFee.HasValue)
            {
                fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.TotalFee.Value, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) :Math.Round(fee.TotalFee.Value, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
            fee.FeeSubQuantity = fee.FeeSubQuantity;
        }

        private static void CalculateAndSetDemurrageFee(FuelFeeViewModel fee)
        {
            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.EndTime.HasValue && fee.StartTime.HasValue)
            {
                var difference = fee.EndTime.Value.AddMilliseconds(-fee.EndTime.Value.Millisecond)
                                 .Subtract(fee.StartTime.Value.AddMilliseconds(-fee.StartTime.Value.Millisecond)).TotalSeconds;
                var calculatedDifference = CalcualateDifferenceAndSetTotalFee(fee, (decimal)difference);

                fee.FeeSubQuantity = calculatedDifference;
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.FeeSubQuantity.HasValue && fee.FeeSubQuantity.Value > 0)
            {
                var difference = CalcualateDifferenceAndSetTotalFee(fee, fee.FeeSubQuantity.Value);
                fee.FeeSubQuantity = difference;
            }
            else
            {
                fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) :Math.Round(fee.Fee, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
        }

        private static decimal CalcualateDifferenceAndSetTotalFee(FuelFeeViewModel fee, decimal difference)
        {
            if (fee.WaiveOffTime.HasValue)
            {
                var waiveOffInSeconds = fee.WaiveOffTime.Value * 60;
                if (difference > waiveOffInSeconds)
                    difference = difference - waiveOffInSeconds;
                else
                    difference = 0;
            }

            if (difference > 0)
                fee.TotalFee = fee.IsMarineLocation? (Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay)):Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            else
                fee.TotalFee = 0;

            return difference;
        }

        private static void CalculateAndSetFuelTruckRetainFee(FuelFeeViewModel fee)
        {
            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
            {
                decimal difference = 0;
                if (fee.EndTime.HasValue && fee.StartTime.HasValue)
                {
                    difference = (decimal)fee.EndTime.Value.AddMilliseconds(-fee.EndTime.Value.Millisecond)
                                     .Subtract(fee.StartTime.Value.AddMilliseconds(-fee.StartTime.Value.Millisecond)).TotalSeconds;
                }
                else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.FeeSubQuantity.HasValue && fee.FeeSubQuantity.Value > 0)
                {
                    difference = fee.FeeSubQuantity.Value;
                }

                if (difference > 0)
                    fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
                else
                    fee.TotalFee = 0;

                fee.FeeSubQuantity = difference;
            }
            else
            {
                fee.TotalFee = Math.Round(fee.Fee, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
        }

        private static void CalculateAndSetUnderGallonFee(InvoiceModel invoiceModel, FuelFeeViewModel fee)
        {
            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee && fee.MinimumGallons > invoiceModel.DroppedGallons)
            {
                fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) :Math.Round(fee.Fee, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
        }

        public static void CalculateAndSetByQuantityFee(decimal droppedQuantity, FuelFeeViewModel fee, decimal? tierPricingInvQty = null)
        {
            if (tierPricingInvQty != null && tierPricingInvQty.HasValue && tierPricingInvQty.Value > 0)
                droppedQuantity = tierPricingInvQty.Value;

            var byQantity = fee.FeeByQuantities.FirstOrDefault(t => droppedQuantity >= t.MinQuantity && droppedQuantity <= (t.MaxQuantity ?? droppedQuantity));
            if (byQantity != null)
            {
                fee.TotalFee = fee.IsMarineLocation ? Math.Round(byQantity.Fee, ApplicationConstants.MarineInvoiceFeeTotalAmountDecimalDisplay) :Math.Round(byQantity.Fee, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            }
        }

        private static void CalculateAndSetPerAssetFee(FuelFeeViewModel fee, int assetCount)
        {
            fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee * assetCount, ApplicationConstants.MarineInvoiceFeeTotalAmountDecimalDisplay) :Math.Round(fee.Fee * assetCount, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            fee.FeeSubQuantity = assetCount;
        }

        private void CalculateAndSetPerHourFee(double difference, FuelFeeViewModel fee)
        {
            fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.MarineInvoiceFeeTotalAmountDecimalDisplay) :Math.Round(fee.Fee * Convert.ToDecimal(difference) / 3600, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            fee.FeeSubQuantity = (decimal)difference;
        }

        private static void CalculateAndSetPerGallonFee(decimal droppedQuantity, FuelFeeViewModel fee)
        {
            fee.TotalFee = fee.IsMarineLocation ? Math.Round(fee.Fee * droppedQuantity, ApplicationConstants.MarineInvoiceFeeTotalAmountDecimalDisplay) :Math.Round(fee.Fee * droppedQuantity, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
            fee.FeeSubQuantity = droppedQuantity;
        }

        public void CalculateTotalFeeAndQuantityForDiscount(IEnumerable<FuelFeeViewModel> invoiceFees, decimal basicAmount, FuelFeeViewModel discount)
        {
            if (discount.FeeTypeId == (int)FeeType.SubTotal)
            {
                CalculateAndSetDiscountOnSubTotal(invoiceFees, basicAmount, discount);
            }
            else
            {
                CalculateAnsSetDiscountOnFee(invoiceFees, discount);
            }
        }

        private static void CalculateAndSetDiscountOnSubTotal(IEnumerable<FuelFeeViewModel> invoiceFees, decimal basicAmount, FuelFeeViewModel discount)
        {
            var subTotal = invoiceFees.Sum(t => t.TotalFee ?? 0) + basicAmount;
            if (discount.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                discount.TotalFee = Math.Round(subTotal / 100 * discount.Fee, ApplicationConstants.InvoiceDiscountTotalAmountDecimalDisplay);
            }
            else if (discount.FeeSubTypeId == (int)FeeSubType.FlatFee)
            {
                discount.TotalFee = Math.Round(discount.Fee, ApplicationConstants.InvoiceDiscountTotalAmountDecimalDisplay);
            }
        }

        private static void CalculateAnsSetDiscountOnFee(IEnumerable<FuelFeeViewModel> invoiceFees, FuelFeeViewModel discount)
        {
            var totalFee = invoiceFees.Where(t => t.FeeTypeId == discount.FeeTypeId).Sum(t1 => t1.TotalFee);
            if (totalFee.HasValue)
            {
                if (discount.FeeSubTypeId == (int)FeeSubType.Percent)
                {
                    discount.TotalFee = Math.Round(totalFee.Value / 100 * discount.Fee, ApplicationConstants.InvoiceDiscountTotalAmountDecimalDisplay);
                }
                else if (discount.FeeSubTypeId == (int)FeeSubType.FlatFee)
                {
                    discount.TotalFee = Math.Round(discount.Fee, ApplicationConstants.InvoiceDiscountTotalAmountDecimalDisplay);
                }
            }
        }
    }
}
