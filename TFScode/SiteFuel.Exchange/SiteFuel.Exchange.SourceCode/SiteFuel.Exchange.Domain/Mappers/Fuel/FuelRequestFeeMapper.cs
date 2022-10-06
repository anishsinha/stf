using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FuelRequestFeeMapper
    {
        public static bool CheckFeeApplicableConstraint(FeesViewModel currentFee, List<FeesViewModel> allFees, DateTimeOffset dateToCheck)
        {
            if (currentFee.FeeConstraintTypeId.HasValue)
            {
                if (currentFee.FeeConstraintTypeId == (int)FeeConstraintType.Weekend && (dateToCheck.DayOfWeek == DayOfWeek.Saturday || dateToCheck.DayOfWeek == DayOfWeek.Sunday))
                {
                    if ((!string.IsNullOrWhiteSpace(currentFee.FeeTypeId) && currentFee.FeeTypeId.Equals(((int)FeeType.OtherFee).ToString())) ||
                        !string.IsNullOrWhiteSpace(currentFee.OtherFeeDescription))
                    {
                        return IsOtherFeeApplicableSpecialDate(currentFee, allFees, dateToCheck);
                    }
                    // we give priority to specific date fees - so if any fees exist for this date & weekend also - we apply special date fees
                    return !allFees.Any(t => t.SpecialDate.HasValue && !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(currentFee.FeeTypeId) && t.SpecialDate.Value.Date == dateToCheck.Date);
                }
                else if (currentFee.FeeConstraintTypeId == (int)FeeConstraintType.SpecialDate && dateToCheck.Date == currentFee.SpecialDate.Value.Date)
                {
                    return true;
                }
                return false;
            }
            else
            {
                // check if any special date fee of same feetype exist - if it exists then currentFee should not be applied & that special date fee would be applied
                if (dateToCheck.DayOfWeek == DayOfWeek.Saturday || dateToCheck.DayOfWeek == DayOfWeek.Sunday)
                {
                    if ((!string.IsNullOrWhiteSpace(currentFee.FeeTypeId) && currentFee.FeeTypeId.Equals(((int)FeeType.OtherFee).ToString())) ||
                        !string.IsNullOrWhiteSpace(currentFee.OtherFeeDescription))
                    {
                        return IsOtherFeeApplicableWeekend(currentFee, allFees, dateToCheck);
                    }
                    return !allFees.Any(t => (
                                                (t.FeeConstraintTypeId.HasValue && t.FeeConstraintTypeId.Value == (int)FeeConstraintType.Weekend) ||
                                                (t.SpecialDate.HasValue && t.SpecialDate.Value.Date == dateToCheck.Date)
                                            ) &&
                                        t.FeeTypeId.Equals(currentFee.FeeTypeId));
                }
                else
                {
                    if ((!string.IsNullOrWhiteSpace(currentFee.FeeTypeId) && currentFee.FeeTypeId.Equals(((int)FeeType.OtherFee).ToString())) ||
                        !string.IsNullOrWhiteSpace(currentFee.OtherFeeDescription))
                    {
                        return IsOtherFeeApplicableSpecialDate(currentFee, allFees, dateToCheck);
                    }
                    return !allFees.Any(t => t.SpecialDate.HasValue && !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.SpecialDate.Value.Date == dateToCheck.Date
                                          && t.FeeTypeId.Equals(currentFee.FeeTypeId));
                }
            }
        }

        private static bool IsOtherFeeApplicableSpecialDate(FeesViewModel currentFee, List<FeesViewModel> allFees, DateTimeOffset dateToCheck)
        {
            if (currentFee.OtherFeeDescription != null)
            {
                return !allFees.Any(t => t.SpecialDate.HasValue &&
                        !string.IsNullOrWhiteSpace(t.OtherFeeDescription) &&
                        t.OtherFeeDescription.Equals(currentFee.OtherFeeDescription) &&
                        t.SpecialDate.Value.Date == dateToCheck.Date);
            }
            else
            {
                return !allFees.Any(t => t.SpecialDate.HasValue &&
                        t.OtherFeeTypeId.HasValue &&
                        t.OtherFeeTypeId == currentFee.OtherFeeTypeId &&
                        t.SpecialDate.Value.Date == dateToCheck.Date);
            }
        }

        private static bool IsOtherFeeApplicableWeekend(FeesViewModel currentFee, List<FeesViewModel> allFees, DateTimeOffset dateToCheck)
        {
            if (currentFee.OtherFeeDescription != null)
            {
                return !allFees.Any(t =>
                                        (
                                            (t.FeeConstraintTypeId.HasValue && t.FeeConstraintTypeId.Value == (int)FeeConstraintType.Weekend) ||
                                            (t.SpecialDate.HasValue && t.SpecialDate.Value.Date == dateToCheck.Date)
                                        ) &&
                        !string.IsNullOrWhiteSpace(t.OtherFeeDescription) &&
                        t.OtherFeeDescription.Equals(currentFee.OtherFeeDescription));
            }
            else
            {
                return !allFees.Any(t =>
                                        (
                                            (t.FeeConstraintTypeId.HasValue && t.FeeConstraintTypeId.Value == (int)FeeConstraintType.Weekend) ||
                                            (t.SpecialDate.HasValue && t.SpecialDate.Value.Date == dateToCheck.Date)
                                        ) &&
                        t.OtherFeeTypeId.HasValue &&
                        t.OtherFeeTypeId == currentFee.OtherFeeTypeId);
            }
        }

        public static List<FuelFee> ToEntity(this FuelFeesViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity;

            foreach (var fee in viewModel.FuelRequestFees)
            {
                var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);
                if (!isCommonFee)
                {
                    feeTypeId = (int)FeeType.OtherFee;
                }

                if ((feeTypeId > 0 && fee.Fee.HasValue && fee.Fee.Value > 0) || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    entity = new FuelFee();
                    entity.FeeTypeId = feeTypeId;
                    entity.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                    entity.FeeSubTypeId = fee.FeeSubTypeId;
                    entity.OtherFeeTypeId = fee.OtherFeeTypeId;
                    entity.IncludeInPPG = fee.IncludeInPPG;
                    entity.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                    entity.SpecialDate = fee.SpecialDate;
                    entity.Currency = fee.Currency;
                    entity.UoM = fee.UoM;
                    entity.WaiveOffTime = fee.WaiveOffTime;
                    if (!fee.CommonFee)
                    {
                        entity.FeeDetails = fee.OtherFeeDescription;
                    }
                    if (fee.MarginTypeId != (int)MarginType.NoChange)
                    {
                        entity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                        entity.MarginTypeId = fee.MarginTypeId;
                        entity.Margin = fee.Margin ?? 0;
                    }
                    if (feeTypeId == (int)FeeType.UnderGallonFee)
                    {
                        entity.MinimumGallons = fee.MinimumGallons;
                    }

                    if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    {
                        foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                        {
                            var entityQuantity = new FeeByQuantity();
                            entityQuantity.Id = deliveryFees.Id;
                            entityQuantity.FeeTypeId = feeTypeId;
                            entityQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                            entityQuantity.MinQuantity = deliveryFees.MinQuantity;
                            entityQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                            entityQuantity.Fee = deliveryFees.Fee;
                            entityQuantity.Currency = fee.Currency;
                            entityQuantity.UoM = fee.UoM;
                            entity.FeeByQuantities.Add(entityQuantity);
                        }
                    }

                    entities.Add(entity);
                }
            }

            if (viewModel.ResaleFee != null)
            {
                List<FuelRequestResaleFeeViewModel> resaleFees = viewModel.ResaleFee;
                foreach (FuelRequestResaleFeeViewModel resaleFee in resaleFees)
                {
                    entity = new FuelFee();
                    entity.FeeTypeId = resaleFee.FeeTypeId;
                    entity.Fee = resaleFee.Fee;
                    entity.FeeSubTypeId = resaleFee.FeeSubTypeId;
                    entity.Currency = resaleFee.Currency;
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public static List<FuelFee> ToFuelSurchargeEntity(this FuelSurchargeFreightFeeViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity;

            if (viewModel.FeeSubTypeId > 0)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.FeeTypeId;
                entity.FeeSubTypeId = viewModel.FeeSubTypeId;
                if (viewModel.FeeSubTypeId == (int)FeeSubType.FlatFee || viewModel.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    entity.Fee = viewModel.Fee;
                entity.Currency = viewModel.Currency;
                entity.UoM = viewModel.UoM;

                if (viewModel.FeeSubTypeId == (int)FeeSubType.ByDistance)
                {
                    foreach (var deliveryFees in viewModel.DeliveryFeeByQuantity)
                    {
                        var entityQuantity = new FeeByQuantity();
                        entityQuantity.Id = deliveryFees.Id;
                        entityQuantity.FeeTypeId = viewModel.FeeTypeId;
                        entityQuantity.FeeSubTypeId = (int)FeeSubType.ByDistance;
                        entityQuantity.MinQuantity = deliveryFees.MinQuantity;
                        entityQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                        entityQuantity.Fee = deliveryFees.Fee;
                        entityQuantity.Currency = viewModel.Currency;
                        entityQuantity.UoM = viewModel.UoM;
                        entity.FeeByQuantities.Add(entityQuantity);
                    }
                }

                entities.Add(entity);
            }

            return entities;
        }

        public static List<FuelFee> ToFreightCostEntity(this FreightCostFeeViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity;

            if (viewModel.FeeSubTypeId > 0)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.FeeTypeId;
                entity.FeeSubTypeId = viewModel.FeeSubTypeId;
                if (viewModel.FeeSubTypeId == (int)FeeSubType.FlatFee || viewModel.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    entity.Fee = viewModel.Fee;
                entity.Currency = viewModel.Currency;
                entity.UoM = viewModel.UoM;

                entities.Add(entity);
            }

            return entities;
        }

        public static List<FuelFee> ToInvoiceFeesEntity(this FuelFeesViewModel viewModel, DateTime dateToCompare, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity;
            bool feeConstraintCheck = viewModel.FuelRequestFees.Any(t => t.FeeConstraintTypeId.HasValue);

            foreach (var fee in viewModel.FuelRequestFees)
            {
                var isFeeApplicable = true;
                var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);
                if (!isCommonFee)
                {
                    feeTypeId = (int)FeeType.OtherFee;
                }

                if ((feeTypeId > 0 && fee.Fee.HasValue && fee.Fee.Value > 0) || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    if (feeConstraintCheck)
                        isFeeApplicable = CheckFeeApplicableConstraint(fee, viewModel.FuelRequestFees, dateToCompare);
                    if (isFeeApplicable)
                    {
                        entity = new FuelFee();
                        entity.FeeTypeId = feeTypeId;
                        entity.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                        entity.FeeSubTypeId = fee.FeeSubTypeId;
                        entity.OtherFeeTypeId = fee.OtherFeeTypeId;
                        entity.IncludeInPPG = fee.IncludeInPPG;
                        entity.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                        entity.SpecialDate = fee.SpecialDate;
                        entity.Currency = fee.Currency;
                        entity.UoM = fee.UoM;
                        entity.StartTime = fee.StartTime;
                        entity.EndTime = fee.EndTime;
                        entity.WaiveOffTime = fee.WaiveOffTime;

                        if (entity.FeeTypeId == (int)FeeType.DemurrageFeeDestination || entity.FeeTypeId == (int)FeeType.DemurrageFeeTerminal ||
                            entity.FeeTypeId == (int)FeeType.DemurrageOther || entity.FeeTypeId == (int)FeeType.Retain)
                        {
                            entity.FeeSubQuantity = fee.TimeInMinutes * 60;
                        }

                        if (entity.FeeTypeId == (int)FeeType.TankDeliveryFee || entity.FeeTypeId == (int)FeeType.TankPickupFee || entity.FeeTypeId == (int)FeeType.TankRental)
                        {
                            entity.TotalFee = fee.TotalFee;
                        }

                        if (!fee.CommonFee)
                        {
                            entity.FeeDetails = fee.OtherFeeDescription;
                        }
                        if (fee.MarginTypeId != (int)MarginType.NoChange)
                        {
                            entity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                            entity.MarginTypeId = fee.MarginTypeId;
                            entity.Margin = fee.Margin ?? 0;
                        }
                        if (feeTypeId == (int)FeeType.UnderGallonFee)
                        {
                            entity.MinimumGallons = fee.MinimumGallons;
                        }

                        if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                        {
                            foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                            {
                                var entityQuantity = new FeeByQuantity();
                                entityQuantity.Id = deliveryFees.Id;
                                entityQuantity.FeeTypeId = feeTypeId;
                                entityQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                                entityQuantity.MinQuantity = deliveryFees.MinQuantity;
                                entityQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                                entityQuantity.Fee = deliveryFees.Fee;
                                entityQuantity.Currency = fee.Currency;
                                entityQuantity.UoM = fee.UoM;
                                entity.FeeByQuantities.Add(entityQuantity);
                            }
                        }

                        entities.Add(entity);
                    }
                }
            }

            if (viewModel.ResaleFee != null)
            {
                List<FuelRequestResaleFeeViewModel> resaleFees = viewModel.ResaleFee;
                foreach (FuelRequestResaleFeeViewModel resaleFee in resaleFees)
                {
                    entity = new FuelFee();
                    entity.FeeTypeId = resaleFee.FeeTypeId;
                    entity.Fee = resaleFee.Fee;
                    entity.FeeSubTypeId = resaleFee.FeeSubTypeId;
                    entity.Currency = resaleFee.Currency;
                    entities.Add(entity);
                }
            }

            if (viewModel.DiscountLineItems.Any())
            {
                List<DiscountLineItemViewModel> discountLineItems = viewModel.DiscountLineItems;
                foreach (DiscountLineItemViewModel discountLineItem in discountLineItems)
                {
                    entity = new FuelFee();
                    entity.FeeTypeId = discountLineItem.FeeTypeId;
                    entity.Fee = discountLineItem.Amount;
                    entity.FeeSubTypeId = discountLineItem.FeeSubTypeId;
                    entity.Currency = discountLineItem.Currency;
                    entity.DiscountLineItemId = discountLineItem.Id;
                    entity.FeeDetails = discountLineItem.FeeDetails;
                    entities.Add(entity);
                }
            }

            if (viewModel.FuelSurchargeFreightFee != null && viewModel.FuelSurchargeFreightFee.IsSurchargeApplicable)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.FuelSurchargeFreightFee.FeeTypeId;
                entity.Fee = viewModel.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.FeeSubTypeId = viewModel.FuelSurchargeFreightFee.FeeSubTypeId;
                entity.Currency = viewModel.FuelSurchargeFreightFee.Currency;
                entity.TotalFee = viewModel.FuelSurchargeFreightFee.TotalFuelSurchargeFee;
                entity.FeeSubQuantity = viewModel.FuelSurchargeFreightFee.GallonsDelivered * viewModel.FuelSurchargeFreightFee.SurchargePercentage;
                entity.WaiveOffTime = viewModel.FuelSurchargeFreightFee.Distance;
                entities.Add(entity);
            }
            return entities;
        }

        public static MstOtherFeeType ToOtherFeeTypeEntity(this FeesViewModel viewModel, MstOtherFeeType entity = null)
        {
            if (entity == null)
                entity = new MstOtherFeeType();

            if (viewModel != null)
            {
                entity = new MstOtherFeeType();

                entity.Name = viewModel.OtherFeeDescription;
                entity.CompanyId = viewModel.CompanyId;
            }

            return entity;
        }

        public static List<FuelFee> ToEntity(this TPOBrokeredOrderFeeViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity = new FuelFee();
            //Freight Fee
            if (viewModel.FreightFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                entity.FeeTypeId = viewModel.FreightFeeTypeId;
                entity.Fee = viewModel.FreightFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.FreightFee;
                entity.FeeSubTypeId = viewModel.FreightFeeSubTypeId;
                entity.Currency = viewModel.Currency;
                entity.UoM = viewModel.UoM;
                entities.Add(entity);
            }

            if (viewModel.AdditionalFees != null)
            {
                List<BrokeredOrderFeeViewModel> additionalFees = viewModel.AdditionalFees;
                foreach (BrokeredOrderFeeViewModel additionalFee in additionalFees)
                {
                    entity = new FuelFee();
                    entity.FeeTypeId = (int)FeeType.OtherFee;
                    entity.FeeDetails = additionalFee.FeeDetails;
                    entity.Fee = additionalFee.Fee;
                    entity.FeeSubTypeId = additionalFee.FeeSubTypeId == 0 ? (int)FeeSubType.FlatFee : additionalFee.FeeSubTypeId;
                    entity.Currency = viewModel.Currency;
                    entity.UoM = viewModel.UoM;
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public static FuelRequestFeeViewModel ToViewModel(this ICollection<FuelFee> entities, FuelRequestFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestFeeViewModel(Status.Success);

            foreach (FuelFee entity in entities)
            {
                switch (entity.FeeTypeId)
                {
                    case (int)FeeType.DeliveryFee:
                        {
                            viewModel.DeliveryFeeTypeId = entity.FeeTypeId;
                            viewModel.DeliveryFeeType = entity.MstFeeType.Name;
                            viewModel.DeliveryFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.DeliveryFee = entity.Fee.GetPreciseValue(6);
                            viewModel.DeliveryFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.DeliveryFeeIncludeInPPG = entity.IncludeInPPG;
                            if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                            {
                                viewModel.DeliveryFeeByQuantity.AddRange(entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList());
                            }
                            break;
                        }
                    case (int)FeeType.WetHoseFee:
                        {
                            viewModel.WetHoseFeeTypeId = entity.FeeTypeId;
                            viewModel.WetHoseFeeType = entity.MstFeeType.Name;
                            viewModel.WetHoseFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.WetHoseFee = entity.Fee.GetPreciseValue(6);
                            viewModel.WetHoseFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.WetHoseFeeIncludeInPPG = entity.IncludeInPPG;
                            break;
                        }
                    case (int)FeeType.OverWaterFee:
                        {
                            viewModel.OverWaterFeeTypeId = entity.FeeTypeId;
                            viewModel.OverWaterFeeType = entity.MstFeeType.Name;
                            viewModel.OverWaterFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.OverWaterFee = entity.Fee.GetPreciseValue(6);
                            viewModel.OverWaterFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.OverWaterFeeIncludeInPPG = entity.IncludeInPPG;
                            break;
                        }
                    case (int)FeeType.DryRunFee:
                        {
                            viewModel.DryRunFeeTypeId = entity.FeeTypeId;
                            viewModel.DryRunFeeType = entity.MstFeeType.Name;
                            viewModel.DryRunFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.DryRunFee = entity.Fee.GetPreciseValue(6);
                            viewModel.DryRunFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.DryRunFeeIncludeInPPG = entity.IncludeInPPG;
                            break;
                        }
                    case (int)FeeType.UnderGallonFee:
                        {
                            viewModel.UnderGallonFeeTypeId = entity.FeeTypeId;
                            viewModel.UnderGallonFeeType = entity.MstFeeType.Name;
                            viewModel.UnderGallonFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.UnderGallonFee = entity.Fee.GetPreciseValue(6);
                            viewModel.UnderGallonFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.MinimumGallons = entity.MinimumGallons.Value.GetPreciseValue(6);
                            viewModel.UnderGallonFeeIncludeInPPG = entity.IncludeInPPG;
                            break;
                        }
                    case (int)FeeType.FreightFee:
                        {
                            viewModel.FreightFeeTypeId = entity.FeeTypeId;
                            viewModel.FreightFeeType = entity.MstFeeType.Name;
                            viewModel.FreightFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.FreightFee = entity.Fee.GetPreciseValue(6);
                            viewModel.FreightFeeSubTypeId = entity.FeeSubTypeId;
                            break;
                        }
                }
            }

            return viewModel;
        }

        public static List<FeesViewModel> ToFeesViewModel(this ICollection<FuelFee> entities, List<FeesViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<FeesViewModel>();

            foreach (FuelFee entity in entities.Where(t => (t.FeeTypeId != (int)FeeType.ResaleFee && t.FeeTypeId != (int)FeeType.SurchargeFreightFee && t.FeeTypeId != (int)FeeType.FreightCost) && t.DiscountLineItemId == null))
            {
                FeesViewModel model = new FeesViewModel();
                model.FeeTypeId = entity.FeeTypeId.ToString();
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.FeeTypeName = entity.Currency == Currency.CAD ? entity.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeType.Name;
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeSubType.Name;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.OtherFeeTypeId = entity.OtherFeeTypeId;
                model.TotalFee = entity.TotalFee ?? 0;
                model.FeeSubQuantity = entity.FeeSubQuantity ?? 0;
                model.Margin = 0;
                model.MarginTypeId = (int)MarginType.NoChange;
                model.IncludeInPPG = entity.IncludeInPPG;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                model.SpecialDate = entity.SpecialDate;
                model.MinimumGallons = entity.MinimumGallons?.GetPreciseValue(6);
                model.CommonFee = !(entity.OtherFeeTypeId.HasValue || entity.FeeDetails != null);
                model.AddToCommonFees = entity.OtherFeeTypeId.HasValue;
                model.OtherFeeDescription = entity.FeeDetails;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.TruckLoadCategoryId = entity.MstFeeType.TruckLoadCategoryId;
                if (entity.FeeTypeId == (int)FeeType.OtherFee && entity.OtherFeeTypeId.HasValue && string.IsNullOrWhiteSpace(entity.FeeDetails))
                {
                    model.FeeTypeId = Constants.OtherCommonFeeCode + "-" + entity.OtherFeeTypeId;
                    model.OtherFeeDescription = entity.MstOtherFeeType.Name;
                    var name = entity.FeeDetails ?? entity.MstOtherFeeType?.Name;
                    model.FeeTypeName = name ?? model.FeeTypeName;
                }
                else if (entity.FeeTypeId == (int)FeeType.OtherFee && !entity.OtherFeeTypeId.HasValue)
                {
                    model.OtherFeeDescription = entity.FeeDetails;
                }

                if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    model.DeliveryFeeByQuantity.AddRange(entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList());
                }
                model.UoM = entity.UoM;
                model.Currency = entity.Currency;

                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.WaiveOffTime = entity.WaiveOffTime;
                if (entity.TaxDetailId != null)
                {
                    model.FeeTaxDetails = new FeeTaxDetails
                    {
                        Amount = entity.TaxDetails.TaxAmount,
                        Description = entity.TaxDetails.RateDescription,
                        Percentage = entity.TaxDetails.TaxRate
                    };
                }

                if (IsFTLSpecificFee(entity.FeeTypeId))
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = ((int)entity.FeeSubQuantity.Value / 60) + entity.WaiveOffTime;
                    }
                }
                else if (entity.FeeTypeId == (int)FeeType.Retain)
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = (int)entity.FeeSubQuantity.Value / 60;
                    }
                }
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static FuelSurchargeFreightFeeViewModel ToSurchargeFreightFeesViewModel(this ICollection<FuelFee> entities, FuelSurchargeFreightFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelSurchargeFreightFeeViewModel();

            var entity = entities.Where(t => (t.FeeTypeId == (int)FeeType.SurchargeFreightFee) && t.DiscountLineItemId == null).FirstOrDefault();
            if (entity != null)
            {
                viewModel.IsSurchargeApplicable = true;
                viewModel.FeeTypeId = entity.FeeTypeId;
                viewModel.FeeSubTypeId = entity.FeeSubTypeId;
                viewModel.Fee = entity.Fee.GetPreciseValue(6);
                viewModel.SurchargeFreightCost = entity.Fee.GetPreciseValue(4);
                viewModel.FeeSubQuantity = entity.FeeSubQuantity;
;               viewModel.TotalFuelSurchargeFee = entity.TotalFee.HasValue ? entity.TotalFee.Value.GetPreciseValue(4) : 0;
                viewModel.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeSubType.Name;
                viewModel.FeeConstraintTypeId = entity.FeeConstraintTypeId;

                if (entity.FeeSubTypeId == (int)FeeSubType.ByDistance)
                {
                    viewModel.IsFeeByDistance = true;
                    viewModel.DeliveryFeeByQuantity.AddRange(entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList());
                    viewModel.Distance = entity.WaiveOffTime ?? 0;
                }
                viewModel.UoM = entity.UoM;
                viewModel.Currency = entity.Currency;
            }
            
            return viewModel;
        }

        public static FreightCostFeeViewModel ToFreightCostFeesViewModel(this ICollection<FuelFee> entities, FreightCostFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FreightCostFeeViewModel();

            var entity = entities.Where(t => (t.FeeTypeId == (int)FeeType.FreightCost) && t.DiscountLineItemId == null).FirstOrDefault();
            if (entity != null)
            {
                viewModel.IsFreightCostApplicable = true;
                viewModel.FeeTypeId = entity.FeeTypeId;
                viewModel.FeeSubTypeId = entity.FeeSubTypeId;
                viewModel.Fee = entity.Fee.GetPreciseValue(6);
                viewModel.UoM = entity.UoM;
                viewModel.Currency = entity.Currency;
            }

            return viewModel;
        }

        public static List<FeesViewModel> ToFeesViewModel(this ICollection<UspInvoicePdfFuelFee> entities, List<FeesViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<FeesViewModel>();

            foreach (var entity in entities.Where(t => t.FeeTypeId != (int)FeeType.ResaleFee && t.DiscountLineItemId == null))
            {
                FeesViewModel model = new FeesViewModel();
                model.FeeTypeId = entity.FeeTypeId.ToString();
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.FeeTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeTypeName;
                model.FeeSubTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeSubTypeName;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.OtherFeeTypeId = entity.OtherFeeTypeId;
                model.TotalFee = entity.TotalFee ?? 0;
                model.FeeSubQuantity = entity.FeeSubQuantity ?? 0;
                model.Margin = 0;
                model.MarginTypeId = (int)MarginType.NoChange;
                model.IncludeInPPG = entity.IncludeInPPG;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                model.SpecialDate = entity.SpecialDate;
                model.MinimumGallons = entity.MinimumGallons?.GetPreciseValue(6);
                model.CommonFee = !(entity.OtherFeeTypeId.HasValue || entity.OtherFeeName != null);
                model.AddToCommonFees = entity.OtherFeeTypeId.HasValue;
                model.OtherFeeDescription = entity.OtherFeeName;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.TruckLoadCategoryId = entity.TruckLoadCategoryId;
                if (entity.FeeTypeId == (int)FeeType.OtherFee && entity.OtherFeeTypeId.HasValue)
                {
                    model.FeeTypeId = Constants.OtherCommonFeeCode + "-" + entity.OtherFeeTypeId;
                    model.FeeTypeName = entity.OtherFeeName ?? model.FeeTypeName;
                }
                if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    var feeByQuantity = new DeliveryFeeByQuantityViewModel()
                    {
                        Id = entity.FeeByQuantityId ?? 0,
                        FeeTypeId = entity.FeeByQuantityTypeId ?? 0,
                        FeeSubTypeId = entity.FeeByQuantitySubTypeId ?? 0,
                        MinQuantity = entity.FeeByQuantityMinQuantity.HasValue ? entity.FeeByQuantityMinQuantity.Value.GetPreciseValue(6) : 0,
                        MaxQuantity = entity.FeeByQuantityMaxQuantity.HasValue ? entity.FeeByQuantityMaxQuantity.Value.GetPreciseValue(6) : 0,
                        Fee = entity.FeeByQuantityFee.HasValue ? entity.FeeByQuantityFee.Value.GetPreciseValue(6) : 0,
                        Currency = (Currency)entity.Currency
                    };
                    model.DeliveryFeeByQuantity.Add(feeByQuantity);
                }
                model.UoM = (UoM)entity.UoM;
                model.Currency = (Currency)entity.Currency;

                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.InvoiceId = entity.InvoiceId;
                model.InvoiceTypeId = entity.InvoiceTypeId;
                model.DroppedGallons = entity.DroppedGallons;
                model.IsSurchargeApplicable = entity.IsSurchargeApplicable;
                model.SurchargePricingType = entity.SurchargePricingType;
                model.SurchargePercentage = entity.SurchargePercentage;
                model.FreightRateRuleType = entity.FreightRateRuleType;
                model.Distance = entity.Distance;
                model.IsFreightCostApplicable = entity.IsFreightCostApplicable;
                model.SurchargeEIAPrice = entity.SurchargeEIAPrice;
                if (IsFTLSpecificFee(entity.FeeTypeId))
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = ((int)entity.FeeSubQuantity.Value / 60) + (entity.WaiveOffTime ?? 0);
                    }
                }
                else if (entity.FeeTypeId == (int)FeeType.Retain)
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = (int)entity.FeeSubQuantity.Value / 60;
                    }
                }
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static List<FeesViewModel> ToViewModel(this ICollection<UspInvoicePdfFuelFee> entities, List<FeesViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<FeesViewModel>();

            foreach (var entity in entities.Where(t => t.FeeTypeId != (int)FeeType.ResaleFee && t.DiscountLineItemId == null))
            {
                FeesViewModel model = new FeesViewModel();
                model.FeeTypeId = entity.FeeTypeId.ToString();
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.FeeTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeTypeName;
                model.FeeSubTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeSubTypeName;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.OtherFeeTypeId = entity.OtherFeeTypeId;
                model.TotalFee = entity.TotalFee ?? 0;
                model.FeeSubQuantity = entity.FeeSubQuantity ?? 0;
                model.Margin = 0;
                model.MarginTypeId = (int)MarginType.NoChange;
                model.IncludeInPPG = entity.IncludeInPPG;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                model.SpecialDate = entity.SpecialDate;
                model.MinimumGallons = entity.MinimumGallons?.GetPreciseValue(6);
                model.CommonFee = !(entity.OtherFeeTypeId.HasValue || entity.OtherFeeName != null);
                model.AddToCommonFees = entity.OtherFeeTypeId.HasValue;
                model.OtherFeeDescription = entity.OtherFeeName;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.TruckLoadCategoryId = entity.TruckLoadCategoryId;
                if (entity.FeeTypeId == (int)FeeType.OtherFee && entity.OtherFeeTypeId.HasValue)
                {
                    model.FeeTypeId = Constants.OtherCommonFeeCode + "-" + entity.OtherFeeTypeId;
                    model.FeeTypeName = entity.OtherFeeName ?? model.FeeTypeName;
                }
                if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    var feeByQuantity = new DeliveryFeeByQuantityViewModel()
                    {
                        Id = entity.FeeByQuantityId ?? 0,
                        FeeTypeId = entity.FeeByQuantityTypeId ?? 0,
                        FeeSubTypeId = entity.FeeByQuantitySubTypeId ?? 0,
                        MinQuantity = entity.FeeByQuantityMinQuantity.HasValue ? entity.FeeByQuantityMinQuantity.Value.GetPreciseValue(6) : 0,
                        MaxQuantity = entity.FeeByQuantityMaxQuantity.HasValue ? entity.FeeByQuantityMaxQuantity.Value.GetPreciseValue(6) : 0,
                        Fee = entity.FeeByQuantityFee.HasValue ? entity.FeeByQuantityFee.Value.GetPreciseValue(6) : 0,
                        Currency = (Currency)entity.Currency
                    };
                    model.DeliveryFeeByQuantity.Add(feeByQuantity);
                }
                model.UoM = (UoM)entity.UoM;
                model.Currency = (Currency)entity.Currency;

                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.WaiveOffTime = entity.WaiveOffTime;

                if (IsFTLSpecificFee(entity.FeeTypeId))
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = ((int)entity.FeeSubQuantity.Value / 60) + entity.WaiveOffTime;
                    }
                }
                else if (entity.FeeTypeId == (int)FeeType.Retain)
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = (int)entity.FeeSubQuantity.Value / 60;
                    }
                }
                if (model.FeeSubTypeId == (int)FeeSubType.HourlyRate && (model.FeeSubQuantity != null && model.FeeSubQuantity.Value != 0))
                {
                    model.TotalHours = new InvoiceDomain().GetHosingTimeInHours(model.FeeSubQuantity.Value.ToString());
                }
                else if (model.FeeSubTypeId == (int)FeeSubType.ByAssetCount && (model.FeeSubQuantity != null && model.FeeSubQuantity.Value != 0))
                {
                    model.TotalAssetQty = Convert.ToInt64(model.FeeSubQuantity.Value);
                }
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static List<FeesViewModel> ToFeesViewModel(this List<UspGetFuelRequestFeeDetailViewModel> entities)
        {
            List<FeesViewModel> viewModel = new List<FeesViewModel>();
            var fees = entities.Where(t => (t.FeeTypeId != (int)FeeType.ResaleFee && t.FeeTypeId != (int)FeeType.SurchargeFreightFee && t.FeeTypeId != (int)FeeType.FreightCost) && t.DiscountLineItemId == null).GroupBy(t => t.FeeId).Select(g => g.ToList()).ToList();
            foreach (var fee in fees)
            {
                var entity = fee.FirstOrDefault();
                FeesViewModel model = new FeesViewModel();
                model.FeeTypeId = entity.FeeTypeId.ToString();
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.FeeTypeName = entity.Currency == Currency.CAD ? entity.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeTypeName;
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeSubTypeName;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.OtherFeeTypeId = entity.OtherFeeTypeId;
                model.TotalFee = entity.TotalFee ?? 0;
                model.FeeSubQuantity = entity.FeeSubQuantity ?? 0;
                model.Margin = 0;
                model.MarginTypeId = (int)MarginType.NoChange;
                model.IncludeInPPG = entity.IncludeInPPG;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                model.SpecialDate = entity.SpecialDate;
                if (entity.SpecialDate.HasValue)
                    model.SpecialDateValue = entity.SpecialDate.Value.ToString(Resource.constFormatDate);
                model.MinimumGallons = entity.MinimumGallons?.GetPreciseValue(6);
                model.CommonFee = !(entity.OtherFeeTypeId.HasValue || entity.FeeDetails != null);
                model.AddToCommonFees = entity.OtherFeeTypeId.HasValue;
                model.OtherFeeDescription = entity.OtherFeeDescription;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.TruckLoadCategoryId = entity.TruckLoadCategoryId;
                if (entity.FeeTypeId == (int)FeeType.OtherFee && entity.OtherFeeTypeId.HasValue && string.IsNullOrWhiteSpace(entity.FeeDetails))
                {
                    model.FeeTypeId = Constants.OtherCommonFeeCode + "-" + entity.OtherFeeTypeId;
                }
                if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    model.DeliveryFeeByQuantity.AddRange(fee.OrderBy(t => t.FeeByQuantityMinQuantity).Select(t => t.ToViewModel()).ToList());
                }
                model.UoM = entity.UoM;
                model.Currency = entity.Currency;
                model.WaiveOffTime = entity.WaiveOffTime;

                if (IsFTLSpecificFee(entity.FeeTypeId))
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = model.FeeSubTypeId == (int)FeeSubType.FlatFee ? ((int)entity.FeeSubQuantity.Value / 60) : (((int)entity.FeeSubQuantity.Value / 60) + entity.WaiveOffTime);
                    }
                }
                else if (entity.FeeTypeId == (int)FeeType.Retain)
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = (int)entity.FeeSubQuantity.Value / 60;
                    }
                }
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static FuelSurchargeFreightFeeViewModel ToSurchargeFreightFeesViewModel(this List<UspGetFuelRequestFeeDetailViewModel> entities)
        {
            FuelSurchargeFreightFeeViewModel model = new FuelSurchargeFreightFeeViewModel();
            var fees = entities.Where(t => (t.FeeTypeId == (int)FeeType.SurchargeFreightFee) && t.DiscountLineItemId == null).GroupBy(t => t.FeeId).Select(g => g.ToList()).ToList();
            foreach (var fee in fees)
            {
                var entity = fee.FirstOrDefault();
                model.FeeTypeId = entity.FeeTypeId;
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeSubTypeName;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                if (entity.FeeSubTypeId == (int)FeeSubType.ByDistance && entity.FeeByQuantityTypeId != null)
                {
                    model.DeliveryFeeByQuantity.AddRange(fee.OrderBy(t => t.FeeByQuantityMinQuantity).Select(t => t.ToViewModel()).ToList());
                }
                model.UoM = entity.UoM;
                model.Currency = entity.Currency;
            }

            return model;
        }

        public static TPOBrokeredOrderFeeViewModel ToExternalBrokerViewModel(this List<UspGetFuelRequestFeeDetailViewModel> entities)
        {
            var viewModel = new TPOBrokeredOrderFeeViewModel();

            foreach (var entity in entities)
            {
                switch (entity.FeeTypeId)
                {
                    case (int)FeeType.FreightFee:
                        {
                            viewModel.FreightFeeTypeId = entity.FeeTypeId;
                            viewModel.FreightFeeType = entity.FeeTypeName;
                            viewModel.FreightFeeSubType = entity.FeeSubTypeName;
                            viewModel.FreightFee = entity.Fee.GetPreciseValue(6);
                            viewModel.FreightFeeSubTypeId = entity.FeeSubTypeId;
                            break;
                        }
                    case (int)FeeType.OtherFee:
                        {
                            BrokeredOrderFeeViewModel fee = new BrokeredOrderFeeViewModel();
                            fee.FeeTypeId = entity.FeeTypeId;
                            fee.FeeSubTypeId = entity.FeeSubTypeId;
                            fee.FeeDetails = entity.FeeDetails;
                            fee.FeeSubTypeName = entity.FeeSubTypeName;
                            fee.Fee = entity.Fee.GetPreciseValue(6);
                            viewModel.AdditionalFees.Add(fee);
                            break;
                        }
                }
                viewModel.Currency = entity.Currency;
                viewModel.UoM = entity.UoM;
            }

            return viewModel;
        }

        private static bool IsFTLSpecificFee(int feeTypeId)
        {
            return feeTypeId == (int)FeeType.DemurrageFeeDestination || feeTypeId == (int)FeeType.DemurrageFeeTerminal
                    || feeTypeId == (int)FeeType.DemurrageOther || feeTypeId == (int)FeeType.FreightFee || feeTypeId == (int)FeeType.SplitTank
                    || feeTypeId == (int)FeeType.PumpCharge || feeTypeId == (int)FeeType.StopOffFee;
        }

        public static List<FeesViewModel> UpdateDemurrageFees(this List<DemurrageDetailsViewModel> viewModels, List<FeesViewModel> entities)
        {
            var filteredEntities = entities.Where(t => t.FeeTypeId == ((int)FeeType.DemurrageFeeDestination).ToString()
                                                    || t.FeeTypeId == ((int)FeeType.DemurrageFeeTerminal).ToString()
                                                    || t.FeeTypeId == ((int)FeeType.DemurrageOther).ToString());

            var demurrageFees = new List<FeesViewModel>();
            foreach (FeesViewModel fuelfee in filteredEntities)
            {
                var demurrageEntities = viewModels.Where(t => t.FeeTypeId.ToString() == fuelfee.FeeTypeId);
                if (demurrageEntities.Count() == 1)
                {
                    foreach (var item in demurrageEntities)
                    {
                        var demurrageFee = fuelfee.Clone();
                        demurrageFee.StartTime = item.StartTime;
                        demurrageFee.EndTime = item.EndTime;
                        demurrageFees.Add(demurrageFee);
                    }
                }
                else if (demurrageEntities.Count() > 1)
                {
                    //If Demurrage Other is Multiple then sum of all and save only once
                    double totalMinutes = 0;
                    DateTimeOffset endTime = DateTimeOffset.Now;
                    foreach (var item in demurrageEntities)
                    {
                        endTime = item.EndTime;
                        totalMinutes = totalMinutes + item.EndTime.AddMilliseconds(-item.EndTime.Millisecond)
                                    .Subtract(item.StartTime.AddMilliseconds(-item.StartTime.Millisecond)).TotalMinutes;
                    }

                    if (totalMinutes > 0)
                    {
                        var demurrageFee = fuelfee.Clone();
                        demurrageFee.StartTime = endTime.AddMinutes(-totalMinutes);
                        demurrageFee.EndTime = endTime;
                        demurrageFees.Add(demurrageFee);
                    }
                }
            }
            entities = entities.Except(filteredEntities).ToList();
            entities.AddRange(demurrageFees);
            return entities;
        }

        public static List<FeesViewModel> UpdateFuelTruckRetainFees(this FuelTruckRetainDetailsViewModel viewModels, List<FeesViewModel> entities)
        {
            foreach (FeesViewModel fuelfee in entities)
            {
                if (fuelfee.FeeTypeId == ((int)FeeType.Retain).ToString())
                {
                    fuelfee.StartTime = viewModels.StartTime;
                    fuelfee.EndTime = viewModels.EndTime;
                }
            }
            return entities;
        }

        public static List<DiscountLineItemViewModel> ToDiscountFeesViewModel(this ICollection<FuelFee> entities, List<DiscountLineItemViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<DiscountLineItemViewModel>();

            foreach (FuelFee entity in entities)
            {
                DiscountLineItemViewModel model = new DiscountLineItemViewModel();
                model.FeeTypeId = Convert.ToInt32(entity.FeeTypeId);
                model.FeeSubTypeId = entity.FeeSubTypeId;
                if (entity.FeeTypeId != (int)FeeType.OtherFee)
                    model.FeeTypeName = entity.Currency == Currency.CAD ? entity.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeType.Name;
                else
                    model.FeeTypeName = entity.FeeDetails ?? entity.MstOtherFeeType?.Name;
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeSubType.Name;
                model.FeeSubTypeName = model.FeeSubTypeName.Replace(Resource.lblFlatFee, Resource.lblFlat);
                model.Amount = entity.Fee.GetPreciseValue(6);
                model.TotalFee = entity.TotalFee ?? 0;
                model.InvoiceId = entity.InvoiceId.HasValue ? entity.InvoiceId.Value : 0;
                model.Currency = entity.Currency;
                model.FeeDetails = entity.FeeDetails;
                model.Id = entity.DiscountLineItemId.Value;
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static List<DiscountLineItemViewModel> ToDiscountFeesViewModel(this ICollection<UspInvoicePdfFuelFee> entities, List<DiscountLineItemViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<DiscountLineItemViewModel>();

            foreach (var entity in entities.Where(t => t.DiscountLineItemId != null))
            {
                DiscountLineItemViewModel model = new DiscountLineItemViewModel();
                model.FeeTypeId = entity.FeeTypeId;
                model.FeeSubTypeId = entity.FeeSubTypeId;
                if (entity.FeeTypeId != (int)FeeType.OtherFee)
                    model.FeeTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeTypeName;
                else
                    model.FeeTypeName = entity.OtherFeeName;
                model.FeeSubTypeName = entity.Currency == (int)Currency.CAD ? entity.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : entity.FeeSubTypeName;
                model.FeeSubTypeName = model.FeeSubTypeName.Replace(Resource.lblFlatFee, Resource.lblFlat);
                model.Amount = entity.Fee.GetPreciseValue(6);
                model.TotalFee = entity.TotalFee ?? 0;
                model.Currency = (Currency)entity.Currency;
                model.FeeDetails = entity.OtherFeeName;
                model.Id = entity.DiscountLineItemId.Value;
                viewModel.Add(model);
            }

            return viewModel;
        }

        public static TPOBrokeredOrderFeeViewModel ToExternalBrokerViewModel(this ICollection<FuelFee> entities, TPOBrokeredOrderFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TPOBrokeredOrderFeeViewModel();

            foreach (FuelFee entity in entities)
            {
                switch (entity.FeeTypeId)
                {
                    case (int)FeeType.FreightFee:
                        {
                            viewModel.FreightFeeTypeId = entity.FeeTypeId;
                            viewModel.FreightFeeType = entity.MstFeeType.Name;
                            viewModel.FreightFeeSubType = entity.MstFeeSubType.Name;
                            viewModel.FreightFee = entity.Fee.GetPreciseValue(6);
                            viewModel.FreightFeeSubTypeId = entity.FeeSubTypeId;
                            break;
                        }
                    case (int)FeeType.OtherFee:
                        {
                            BrokeredOrderFeeViewModel fee = new BrokeredOrderFeeViewModel();
                            fee.FeeTypeId = entity.FeeTypeId;
                            fee.FeeSubTypeId = entity.FeeSubTypeId;
                            fee.FeeDetails = entity.FeeDetails;
                            fee.FeeSubTypeName = entity.MstFeeSubType.Name;
                            fee.Fee = entity.Fee.GetPreciseValue(6);
                            viewModel.AdditionalFees.Add(fee);
                            break;
                        }
                }
                viewModel.Currency = entity.Currency;
                viewModel.UoM = entity.UoM;
            }

            return viewModel;
        }

        public static TPOBrokeredOrderFeeViewModel ToExternalBrokerViewModel(this ICollection<UspInvoicePdfFuelFee> entities, TPOBrokeredOrderFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TPOBrokeredOrderFeeViewModel();

            foreach (var entity in entities)
            {
                switch (entity.FeeTypeId)
                {
                    case (int)FeeType.FreightFee:
                        {
                            viewModel.FreightFeeTypeId = entity.FeeTypeId;
                            viewModel.FreightFeeType = entity.FeeTypeName;
                            viewModel.FreightFeeSubType = entity.FeeSubTypeName;
                            viewModel.FreightFee = entity.Fee.GetPreciseValue(6);
                            viewModel.FreightFeeAmount = (entity.TotalFee ?? 0).GetPreciseValue(6);
                            viewModel.FreightFeeSubTypeId = entity.FeeSubTypeId;
                            break;
                        }
                    case (int)FeeType.OtherFee:
                        {
                            BrokeredOrderFeeViewModel fee = new BrokeredOrderFeeViewModel();
                            fee.FeeTypeId = entity.FeeTypeId;
                            fee.FeeSubTypeId = entity.FeeSubTypeId;
                            fee.FeeDetails = entity.OtherFeeName;
                            fee.FeeSubTypeName = entity.FeeSubTypeName;
                            fee.Fee = entity.Fee.GetPreciseValue(6);
                            viewModel.AdditionalFees.Add(fee);
                            break;
                        }
                }
                viewModel.Currency = (Currency)entity.Currency;
                viewModel.UoM = (UoM)entity.UoM;
            }

            return viewModel;
        }

        public static List<AdditionalFeeViewModel> ToAdditionalFeeViewModel(this ICollection<FuelFee> entities, List<AdditionalFeeViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<AdditionalFeeViewModel>();

            var additionalFee = entities.Where(t => t.FeeTypeId == (int)FeeType.AdditionalFee).ToList();
            foreach (FuelFee entity in additionalFee)
            {
                AdditionalFeeViewModel additionalFeeModel = new AdditionalFeeViewModel(Status.Success);
                additionalFeeModel.FeeSubTypeId = entity.FeeSubTypeId;
                additionalFeeModel.Fee = entity.Fee.GetPreciseValue(6);
                additionalFeeModel.FeeDetails = entity.FeeDetails;
                additionalFeeModel.FeeSubTypeName = entity.MstFeeSubType.Name;
                additionalFeeModel.IncludeInPPG = entity.IncludeInPPG;
                viewModel.Add(additionalFeeModel);
            }

            return viewModel;
        }

        public static FuelRequestResaleFeeViewModel ToResaleFeeViewModel(this FuelFee entity, FuelRequestResaleFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestResaleFeeViewModel(Status.Success);

            viewModel.Fee = entity.Fee;
            viewModel.FeeSubTypeId = entity.FeeSubTypeId;
            viewModel.FeeTypeId = entity.FeeTypeId;
            viewModel.FeeSubTypeName = entity.MstFeeSubType.Name;
            return viewModel;
        }

        public static List<FuelFeeViewModel> ToInvoiceModelFuelFees(this FuelFeesViewModel viewModel, DateTimeOffset dateToCompare)
        {
            var entities = viewModel.FuelRequestFees.ToInvoiceFees(dateToCompare);
            FuelFeeViewModel entity;
            if (viewModel.ResaleFee != null)
            {
                List<FuelRequestResaleFeeViewModel> resaleFees = viewModel.ResaleFee;
                foreach (FuelRequestResaleFeeViewModel resaleFee in resaleFees)
                {
                    entity = new FuelFeeViewModel();
                    entity.FeeTypeId = resaleFee.FeeTypeId;
                    entity.Fee = resaleFee.Fee;
                    entity.FeeSubTypeId = resaleFee.FeeSubTypeId;
                    entity.Currency = resaleFee.Currency;
                    entities.Add(entity);
                }
            }

            if (viewModel.DiscountLineItems.Any())
            {
                List<DiscountLineItemViewModel> discountLineItems = viewModel.DiscountLineItems;
                foreach (DiscountLineItemViewModel discountLineItem in discountLineItems)
                {
                    entity = new FuelFeeViewModel();
                    entity.FeeTypeId = discountLineItem.FeeTypeId;
                    entity.Fee = discountLineItem.Amount;
                    entity.FeeSubTypeId = discountLineItem.FeeSubTypeId;
                    entity.Currency = discountLineItem.Currency;
                    entity.DiscountLineItemId = discountLineItem.Id;
                    entity.FeeDetails = discountLineItem.FeeDetails;
                    entity.OtherFeeTypeId = discountLineItem.OtherFeeTypeId;
                    entity.TotalFee = discountLineItem.TotalFee;
                    entities.Add(entity);
                }
            }

            if (viewModel.FuelSurchargeFreightFee != null && viewModel.FuelSurchargeFreightFee.IsSurchargeApplicable)
            {
                entity = new FuelFeeViewModel();
                entity.FeeTypeId = viewModel.FuelSurchargeFreightFee.FeeTypeId;
                entity.Fee = viewModel.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.FeeSubTypeId = viewModel.FuelSurchargeFreightFee.FeeSubTypeId;
                entity.Currency = viewModel.FuelSurchargeFreightFee.Currency;
                entity.UoM = viewModel.FuelSurchargeFreightFee.UoM;
                entity.TotalFee = viewModel.FuelSurchargeFreightFee.TotalFuelSurchargeFee;
                entity.FeeSubQuantity = viewModel.FuelSurchargeFreightFee.GallonsDelivered * viewModel.FuelSurchargeFreightFee.SurchargePercentage;
                entity.WaiveOffTime = viewModel.FuelSurchargeFreightFee.Distance;
                entities.Add(entity);
            }

            if (viewModel.FuelSurchargeFreightFee != null && viewModel.FuelSurchargeFreightFee.IsFreightCostApplicable)
            {
                entity = new FuelFeeViewModel();
                entity.FeeTypeId = viewModel.FreightCostFee.FeeTypeId;
                entity.Fee = viewModel.FreightCostFee.Fee;
                entity.FeeSubTypeId = viewModel.FreightCostFee.FeeSubTypeId;
                entity.Currency = viewModel.FreightCostFee.Currency;
                entity.UoM = viewModel.FuelSurchargeFreightFee.UoM;
                var totalFreightCost = viewModel.FuelSurchargeFreightFee.GallonsDelivered * viewModel.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.TotalFee = Math.Round(totalFreightCost, ApplicationConstants.InvoiceFuelSurchargeDecimalDisplay);
                entity.FeeSubQuantity = totalFreightCost;
                //entity.WaiveOffTime = viewModel.FuelSurchargeFreightFee.Distance;
                entities.Add(entity);
            }

            return entities;
        }

        public static List<FuelFeeViewModel> ToInvoiceModelFuelFees(this TPOBrokeredOrderFeeViewModel viewModel)
        {
            var entities = new List<FuelFeeViewModel>();
            FuelFeeViewModel entity = new FuelFeeViewModel();
            //Freight Fee
            if (viewModel.FreightFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                entity.FeeTypeId = viewModel.FreightFeeTypeId;
                entity.Fee = viewModel.FreightFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.FreightFee;
                entity.FeeSubTypeId = viewModel.FreightFeeSubTypeId;
                entity.Currency = viewModel.Currency;
                entity.UoM = viewModel.UoM;
                entities.Add(entity);
            }

            if (viewModel.AdditionalFees != null)
            {
                List<BrokeredOrderFeeViewModel> additionalFees = viewModel.AdditionalFees;
                foreach (BrokeredOrderFeeViewModel additionalFee in additionalFees)
                {
                    entity = new FuelFeeViewModel();
                    entity.FeeTypeId = (int)FeeType.OtherFee;
                    entity.FeeDetails = additionalFee.FeeDetails;
                    entity.Fee = additionalFee.Fee;
                    entity.FeeSubTypeId = additionalFee.FeeSubTypeId == 0 ? (int)FeeSubType.FlatFee : additionalFee.FeeSubTypeId;
                    entity.Currency = viewModel.Currency;
                    entity.UoM = viewModel.UoM;
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public static FuelFee ToEntity(this FuelFeeViewModel viewModel)
        {
            FuelFee entity = new FuelFee();
            entity.FeeTypeId = viewModel.FeeTypeId;
            entity.FeeSubTypeId = viewModel.FeeSubTypeId;
            entity.MinimumGallons = viewModel.MinimumGallons;
            entity.Fee = viewModel.Fee;
            entity.FeeDetails = viewModel.FeeDetails;
            entity.MarginTypeId = viewModel.MarginTypeId;
            entity.Margin = viewModel.Margin;
            entity.IncludeInPPG = viewModel.IncludeInPPG;
            entity.FeeSubQuantity = viewModel.FeeSubQuantity;
            entity.TotalFee = viewModel.TotalFee != null ? (viewModel.IsMarineLocation ? Math.Round(viewModel.TotalFee.Value, 6) : Math.Round(viewModel.TotalFee.Value, 2)) : viewModel.TotalFee;
            entity.OtherFeeTypeId = viewModel.OtherFeeTypeId;
            entity.FeeConstraintTypeId = viewModel.FeeConstraintTypeId;
            entity.SpecialDate = viewModel.SpecialDate;
            entity.Currency = viewModel.Currency;
            entity.UoM = viewModel.UoM;
            entity.OfferPricingId = viewModel.OfferPricingId;
            entity.DiscountLineItemId = viewModel.DiscountLineItemId;
            entity.StartTime = viewModel.StartTime;
            entity.EndTime = viewModel.EndTime;
            entity.WaiveOffTime = viewModel.WaiveOffTime;

            if (viewModel.FeeByQuantities.Any())
            {
                entity.FeeByQuantities = viewModel.FeeByQuantities.Select(t => t.ToEntity()).ToList();
            }

            if (IsFeeTaxValid(viewModel.FeeTaxDetails))
            {
                entity.TaxDetails = viewModel.FeeTaxDetails.ToEntity();
                entity.TaxDetails.Currency = viewModel.Currency.ToString();
                entity.TaxDetails.TradingCurrency = viewModel.Currency.ToString();
                entity.TaxDetails.UnitOfMeasure = viewModel.UoM.ToString();
                entity.TaxDetails.RateDescription = viewModel.FeeTaxDetails.Description ??
                    string.Format("{0} {1}", Resource.lblTaxOn,
                    viewModel.Currency == Currency.CAD ?
                    (viewModel.FeeTypeId == (int)FeeType.OtherFee ? viewModel.FeeDetails : EnumHelperMethods.GetDisplayName((FeeType)viewModel.FeeTypeId).Replace(Resource.lblGallon, Resource.lblLitre)) :
                    (viewModel.FeeTypeId == (int)FeeType.OtherFee ? viewModel.FeeDetails : EnumHelperMethods.GetDisplayName((FeeType)viewModel.FeeTypeId)));
            }

            return entity;
        }

        public static FuelFee ToEntity(this TankDetailsViewModel viewModel, FuelFee entity = null)
        {
            if (entity == null)
                entity = new FuelFee();

            entity.Id = viewModel.TankDetailId;
            entity.Fee = viewModel.RentalFee;
            entity.FeeTypeId = (int)FeeType.TankRental; // for tank rental invoices - we make tank entries as tank rental fee type
            entity.FeeSubTypeId = (int)FeeSubType.FlatFee;
            entity.FeeDetails = viewModel.Description;
            entity.TotalFee = viewModel.RentalFee;
            entity.FeeSubQuantity = viewModel.IntervalDays;
            entity.StartTime = viewModel.IntervalStartDate ?? viewModel.StartDate;
            entity.EndTime = viewModel.IntervalEndDate ?? viewModel.EndDate;

            if (IsFeeTaxValid(viewModel.FeeTaxDetails))
            {
                entity.TaxDetails = viewModel.FeeTaxDetails.ToEntity();
                entity.TaxDetails.Currency = viewModel.Currency.ToString();
                entity.TaxDetails.TradingCurrency = viewModel.Currency.ToString();
                entity.TaxDetails.UnitOfMeasure = viewModel.UoM.ToString();
                entity.TaxDetails.RateDescription = viewModel.FeeTaxDetails.Description ??
                    string.Format("{0} {1}", Resource.lblTaxOn, viewModel.Description);
            }

            return entity;
        }

        private static bool IsFeeTaxValid(FeeTaxDetails tax)
        {
            return tax != null && tax.Amount.HasValue && tax.Percentage.HasValue && tax.Amount > 0 && tax.Percentage > 0M;
        }

        public static TaxDetail ToEntity(this FeeTaxDetails viewModel, TaxDetail entity = null)
        {
            if (entity == null)
                entity = new TaxDetail();

            entity.CalculationTypeInd = "N";
            entity.ProductCategory = 1;
            entity.RateSubType = "NONE";
            entity.RateType = "Percentage";
            entity.SalesTaxBaseAmount = 0;
            entity.TaxAmount = viewModel.Amount.Value;
            entity.TaxExemptionInd = "N";
            entity.TaxRate = viewModel.Percentage.Value;
            entity.TaxType = "FUEL";
            entity.TaxingLevel = "NONE";
            entity.LicenseNumber = "NONE";
            entity.TradingTaxAmount = viewModel.Amount.Value;
            entity.ExchangeRate = 1;
            entity.IsModified = false;
            return entity;
        }

        public static List<FuelFeeViewModel> ToInvoiceFees(this List<FeesViewModel> fuelRequestFees, DateTimeOffset dateToCompare,bool IsMarineLocation = false)
        {
            var entities = new List<FuelFeeViewModel>();
            bool feeConstraintCheck = fuelRequestFees.Any(t => t.FeeConstraintTypeId.HasValue);
            FuelFeeViewModel entity;
            foreach (var fee in fuelRequestFees)
            {
                var isFeeApplicable = true;
                
                var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);
                if (!isCommonFee)
                {
                    var otherFeeTypeId = fee.FeeTypeId == null ? string.Empty : fee.FeeTypeId.Replace(Constants.OtherCommonFeeCode + "-", string.Empty);
                    if (!fee.OtherFeeTypeId.HasValue)
                        fee.OtherFeeTypeId = string.IsNullOrWhiteSpace(otherFeeTypeId) ? (int?)null : Convert.ToInt32(otherFeeTypeId);
                    feeTypeId = (int)FeeType.OtherFee;
                }

                if ((feeTypeId > 0 && fee.Fee.HasValue && fee.Fee.Value > 0) || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    if (feeConstraintCheck)
                        isFeeApplicable = CheckFeeApplicableConstraint(fee, fuelRequestFees, dateToCompare);
                    if (isFeeApplicable)
                    {
                        entity = new FuelFeeViewModel();
                        entity.IsMarineLocation = IsMarineLocation;
                        entity.FeeTypeId = feeTypeId;                       
                        entity.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                        entity.TotalFee = fee.TotalFee;
                        entity.FeeSubTypeId = fee.FeeSubTypeId;
                        entity.OtherFeeTypeId = fee.OtherFeeTypeId;
                        entity.IncludeInPPG = fee.IncludeInPPG;
                        entity.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                        entity.SpecialDate = fee.SpecialDate;
                        entity.Currency = fee.Currency;
                        entity.UoM = fee.UoM;
                        entity.AddToCommonFees = fee.AddToCommonFees;
                        entity.StartTime = fee.StartTime;
                        entity.EndTime = fee.EndTime;
                        entity.WaiveOffTime = fee.WaiveOffTime;
                        entity.OrderId = fee.OrderId;
                        entity.FeeSubQuantity = fee.FeeSubQuantity;
                        if (fee.StartTime == null && fee.TimeInMinutes.HasValue && fee.TimeInMinutes.Value > 0)
                        {
                            entity.FeeSubQuantity = fee.TimeInMinutes * 60;
                        }

                        if (fee.FeeTaxDetails != null)
                        {
                            entity.FeeTaxDetails = new FeeTaxDetails
                            {
                                Amount = fee.FeeTaxDetails.Amount,
                                Description = fee.FeeTaxDetails.Description,
                                Percentage = fee.FeeTaxDetails.Percentage
                            };
                        }

                        if (!fee.CommonFee)
                        {
                            entity.FeeDetails = fee.OtherFeeDescription;
                        }
                        if (fee.MarginTypeId != (int)MarginType.NoChange)
                        {
                            entity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                            entity.MarginTypeId = fee.MarginTypeId;
                            entity.Margin = fee.Margin ?? 0;
                        }
                        if (feeTypeId == (int)FeeType.UnderGallonFee)
                        {
                            entity.MinimumGallons = fee.MinimumGallons;
                        }

                        if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                        {
                            foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                            {
                                var entityQuantity = new DeliveryFeeByQuantityViewModel();
                                entityQuantity.Id = deliveryFees.Id;
                                entityQuantity.FeeTypeId = feeTypeId;
                                entityQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                                entityQuantity.MinQuantity = deliveryFees.MinQuantity;
                                entityQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                                entityQuantity.Fee = IsMarineLocation ? Math.Round(deliveryFees.Fee, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : deliveryFees.Fee;
                               // entityQuantity.Fee = deliveryFees.Fee;
                                entityQuantity.Currency = fee.Currency;
                                entityQuantity.UoM = fee.UoM;
                                entity.FeeByQuantities.Add(entityQuantity);
                            }
                        }

                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }
    }
}
