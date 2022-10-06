using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CommonMapper
    {
        public static OrderDetailsViewModel CorrectValues(this OrderDetailsViewModel srcViewModel)
        {
            srcViewModel.AvgGallonsPercentagePerDelivery = srcViewModel.AvgGallonsPercentagePerDelivery.GetPreciseValue(2);
            srcViewModel.TotalGallonsDelivered = srcViewModel.TotalGallonsDelivered.GetPreciseValue(2);
            srcViewModel.AvgPricePerGallon = srcViewModel.AvgPricePerGallon.GetPreciseValue(6);
            srcViewModel.InvoicedAmount = srcViewModel.InvoicedAmount.GetPreciseValue(6);
            srcViewModel.DropTicketAmount = srcViewModel.DropTicketAmount.GetPreciseValue(6);
            srcViewModel.GallonsDelivered = srcViewModel.GallonsDelivered.GetPreciseValue(6);
            srcViewModel.GallonsOrdered = srcViewModel.GallonsOrdered.GetPreciseValue(2);
            if (srcViewModel.OrderAdditionalDetails != null && srcViewModel.OrderAdditionalDetails.Allowance.HasValue)
                srcViewModel.OrderAdditionalDetails.Allowance = srcViewModel.OrderAdditionalDetails.Allowance.Value.GetPreciseValue(6);

            return srcViewModel;
        }

        public static OrderDetailsOutPutViewModel CorrectValues(this OrderDetailsOutPutViewModel srcViewModel)
        {
            srcViewModel.GallonsOrdered = srcViewModel.GallonsOrdered.GetPreciseValue(6);
            srcViewModel.GallonsDelivered = srcViewModel.GallonsDelivered.GetPreciseValue(6);
            srcViewModel.GallonsRemaining = srcViewModel.GallonsOrdered - srcViewModel.GallonsDelivered < 0 ? 0 : srcViewModel.GallonsOrdered - srcViewModel.GallonsDelivered;

            return srcViewModel;
        }

        public static List<InvoiceGridViewModel> CorrectValues(this List<InvoiceGridViewModel> srcViewModel)
        {
            srcViewModel.ForEach(t =>
            {
                t.InvoiceAmount = t.InvoiceAmount.GetPreciseValue(6);
                t.Quantity = t.Quantity.GetPreciseValue(6);
                //t.PricePerGallon = t.PricePerGallon.GetPreciseValue(6);
                t.PricePerGallon = t.PricePerGallon;
            });

            return srcViewModel;
        }

        public static List<InvoiceHistoryGridViewModel> CorrectValues(this List<InvoiceHistoryGridViewModel> srcViewModel)
        {
            srcViewModel.ForEach(t =>
            {
                t.InvoiceAmount = t.InvoiceAmount.GetPreciseValue(6);
            });

            return srcViewModel;
        }

        public static InvoiceDetailViewModel CorrectValues(this InvoiceDetailViewModel srcViewModel)
        {
            srcViewModel.TotalInvoiceAmount = srcViewModel.TotalInvoiceAmount.GetPreciseValue(6);
            srcViewModel.Invoice.DroppedGallons = srcViewModel.Invoice.DroppedGallons.GetPreciseValue();
            return srcViewModel;
        }

        public static ManualInvoiceViewModel CorrectValues(this ManualInvoiceViewModel srcViewModel)
        {
            srcViewModel.FuelRemaining = srcViewModel.FuelRemaining.GetPreciseValue(6);
            srcViewModel.FuelDropped = srcViewModel.FuelDropped.HasValue ? srcViewModel.FuelDropped.Value.GetPreciseValue(6) : srcViewModel.FuelDropped;
            srcViewModel.Assets.ForEach(t =>
            {
                t.DropGallons = t.DropGallons.HasValue ? t.DropGallons.Value.GetPreciseValue(6) : t.DropGallons;
            });

            srcViewModel.StateTax = srcViewModel.StateTax.GetPreciseValue(2);
            srcViewModel.FederalTax = srcViewModel.FederalTax.GetPreciseValue(2);
            srcViewModel.SalesTax = srcViewModel.SalesTax.GetPreciseValue(2);
            if (srcViewModel.Gravity.HasValue && srcViewModel.Gravity.Value > 0)
            {
                srcViewModel.Gravity = srcViewModel.Gravity.Value.GetPreciseValue(1);
            }

            return srcViewModel;
        }

        public static DryRunInvoiceViewModel CorrectValues(this DryRunInvoiceViewModel srcViewModel)
        {
            srcViewModel.FuelRemaining = srcViewModel.FuelRemaining.GetPreciseValue();
            srcViewModel.DryRunFee = srcViewModel.DryRunFee.GetPreciseValue();
            return srcViewModel;
        }

        public static InvoiceGridViewModel CorrectValues(this InvoiceGridViewModel srcViewModel)
        {
            srcViewModel.InvoiceAmount = srcViewModel.InvoiceAmount.GetPreciseValue(6);
            srcViewModel.Quantity = srcViewModel.Quantity.GetPreciseValue(6);
            //srcViewModel.PricePerGallon = srcViewModel.PricePerGallon.GetPreciseValue(6);
            srcViewModel.PricePerGallon = srcViewModel.PricePerGallon;
            return srcViewModel;
        }
    }
}
