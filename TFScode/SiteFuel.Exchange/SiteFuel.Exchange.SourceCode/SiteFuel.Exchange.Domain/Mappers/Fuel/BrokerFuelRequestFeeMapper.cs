using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BrokerFuelRequestFeeMapper
    {
        public static List<FuelFee> ToBrokerEntity(this FuelFeesViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            FuelFee entity;
            foreach (var fee in viewModel.FuelRequestFees)
            {
                int feeTypeId = Convert.ToInt32(fee.FeeTypeId);
                if (feeTypeId > 0)
                {
                    entity = new FuelFee();

                    entity.FeeTypeId = feeTypeId;
                    entity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee == null ? 0 : fee.Fee.Value, fee.MarginTypeId);
                    entity.FeeSubTypeId = fee.FeeSubTypeId;
                    entity.IncludeInPPG = fee.IncludeInPPG;
                    entity.MarginTypeId = fee.MarginTypeId;
                    entity.Margin = fee.Margin ?? 0;

                    if (feeTypeId == (int)FeeType.UnderGallonFee)
                        entity.MinimumGallons = fee.MinimumGallons;

                    if (entity.Fee > 0.0m)
                        entities.Add(entity);
                }
            }

            return entities;
        }

        public static List<FuelFee> ToEntity(this BrokerFuelRequestFeeViewModel viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            //Delivery Fee
            FuelFee entity = new FuelFee();
            if (viewModel.DeliveryFeeSubTypeId != (int)FeeSubType.NoFee || viewModel.DeliveryMargin.MarginTypeId != (int)MarginType.NoChange)
            {
                entity.FeeTypeId = viewModel.DeliveryFeeTypeId;
                entity.FeeSubTypeId = viewModel.DeliveryFeeSubTypeId;
                entity.MarginTypeId = viewModel.DeliveryMargin.MarginTypeId;
                entity.Margin = viewModel.DeliveryMargin.Margin;
                entity.Fee = HelperDomain.GetPriceWithMargin(entity.Margin, viewModel.DeliveryFee, viewModel.DeliveryMargin.MarginTypeId);
                entities.Add(entity);
            }

            //WetHose Fee
            if (viewModel.WetHoseFeeSubTypeId != (int)FeeSubType.NoFee || viewModel.WetHoseMargin.MarginTypeId != (int)MarginType.NoChange)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.WetHoseFeeTypeId;
                entity.Margin = viewModel.WetHoseMargin.Margin;
                entity.MarginTypeId = viewModel.WetHoseMargin.MarginTypeId;
                entity.Fee = HelperDomain.GetPriceWithMargin(entity.Margin, viewModel.WetHoseFee);
                entity.FeeSubTypeId = viewModel.WetHoseFeeSubTypeId;
                entities.Add(entity);
            }

            //OverWater Fee
            if (viewModel.OverWaterFeeSubTypeId != (int)FeeSubType.NoFee || viewModel.OverWaterMargin.MarginTypeId != (int)MarginType.NoChange)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.OverWaterFeeTypeId;
                entity.Margin = viewModel.OverWaterMargin.Margin;
                entity.MarginTypeId = viewModel.OverWaterMargin.MarginTypeId;
                entity.Fee = HelperDomain.GetPriceWithMargin(entity.Margin, viewModel.OverWaterFee);
                entity.FeeSubTypeId = viewModel.OverWaterFeeSubTypeId;
                entities.Add(entity);
            }

            //DryRun Fee
            if (viewModel.DryRunFeeSubTypeId != (int)FeeSubType.NoFee || viewModel.DryRunMargin.MarginTypeId != (int)MarginType.NoChange)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.DryRunFeeTypeId;
                entity.Margin = viewModel.DryRunMargin.Margin;
                entity.MarginTypeId = viewModel.DryRunMargin.MarginTypeId;
                entity.Fee = HelperDomain.GetPriceWithMargin(entity.Margin, viewModel.DryRunFee);
                entity.FeeSubTypeId = viewModel.DryRunFeeSubTypeId;
                entities.Add(entity);
            }

            //Under Gallon Fee
            if (viewModel.UnderGallonFeeSubTypeId != (int)FeeSubType.NoFee || viewModel.UnderGallonMargin.MarginTypeId != (int)MarginType.NoChange)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.UnderGallonFeeTypeId;
                entity.Fee = viewModel.UnderGallonFee ?? 0;
                entity.FeeSubTypeId = viewModel.UnderGallonFeeSubTypeId;
                entity.MinimumGallons = viewModel.MinimumGallons;
                entity.Margin = viewModel.UnderGallonMargin.Margin;
                entity.MarginTypeId = viewModel.UnderGallonMargin.MarginTypeId;
                entities.Add(entity);
            }

            List<AdditionalFeeViewModel> additionalFees = viewModel.AdditionalFee;
            foreach (AdditionalFeeViewModel additionalFee in additionalFees)
            {
                entity = new FuelFee();
                entity.FeeTypeId = viewModel.AdditionalFeeTypeId;
                entity.Margin = viewModel.AdditionalFeeMargin.Margin;
                entity.MarginTypeId = viewModel.AdditionalFeeMargin.MarginTypeId;
                entity.Fee = HelperDomain.GetPriceWithMargin(entity.Margin, additionalFee.Fee, viewModel.AdditionalFeeMargin.MarginTypeId);
                entity.FeeDetails = additionalFee.FeeDetails;
                entity.FeeSubTypeId = additionalFee.FeeSubTypeId;
                entities.Add(entity);
            }

            return entities;
        }

        public static BrokerFuelRequestFeeViewModel ToBrokerViewModel(this ICollection<FuelFee> entities, bool setMargin = false, BrokerFuelRequestFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BrokerFuelRequestFeeViewModel();

            foreach (FuelFee entity in entities)
            {
                switch (entity.FeeTypeId)
                {
                    case (int)FeeType.DeliveryFee:
                        {
                            viewModel.DeliveryFeeTypeId = entity.FeeTypeId;
                            viewModel.DeliveryFee = entity.Fee.GetPreciseValue(6);
                            viewModel.DeliveryFeeSubTypeId = entity.FeeSubTypeId;
                            if (setMargin)
                            {
                                viewModel.DeliveryMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.DeliveryMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }
                    case (int)FeeType.WetHoseFee:
                        {
                            viewModel.WetHoseFeeTypeId = entity.FeeTypeId;
                            if (entity.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                            {
                                viewModel.WetHoseFee = entity.Fee.GetPreciseValue(6);
                            }
                            else if (entity.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                viewModel.WetHoseFee = entity.Fee.GetPreciseValue(6);
                            }
                            viewModel.WetHoseFeeSubTypeId = entity.FeeSubTypeId;
                            if (setMargin)
                            {
                                viewModel.WetHoseMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.WetHoseMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }
                    case (int)FeeType.OverWaterFee:
                        {
                            viewModel.OverWaterFeeTypeId = entity.FeeTypeId;
                            if (entity.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                            {
                                viewModel.OverWaterFee = entity.Fee.GetPreciseValue(6);
                            }
                            else if (entity.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                viewModel.OverWaterFee = entity.Fee.GetPreciseValue(6);
                            }
                            viewModel.OverWaterFeeSubTypeId = entity.FeeSubTypeId;
                            if (setMargin)
                            {
                                viewModel.OverWaterMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.OverWaterMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }
                    case (int)FeeType.DryRunFee:
                        {
                            viewModel.DryRunFeeTypeId = entity.FeeTypeId;
                            viewModel.DryRunFee = entity.Fee.GetPreciseValue(6);
                            viewModel.DryRunFeeSubTypeId = entity.FeeSubTypeId;
                            if (setMargin)
                            {
                                viewModel.DryRunMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.DryRunMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }
                    case (int)FeeType.UnderGallonFee:
                        {
                            viewModel.UnderGallonFeeTypeId = entity.FeeTypeId;
                            viewModel.UnderGallonFee = entity.Fee.GetPreciseValue(6);
                            viewModel.UnderGallonFeeSubTypeId = entity.FeeSubTypeId;
                            viewModel.MinimumGallons = entity.MinimumGallons.Value.GetPreciseValue(6);
                            if (setMargin)
                            {
                                viewModel.UnderGallonMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.UnderGallonMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }

                    case (int)FeeType.AdditionalFee:
                        {
                            if (setMargin)
                            {
                                viewModel.AdditionalFeeMargin.Margin = entity.Margin.GetPreciseValue(6);
                                viewModel.AdditionalFeeMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
                            }
                            break;
                        }
                }
            }

            return viewModel;
        }
    }
}
