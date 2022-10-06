using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class QuoteRequestMapper
    {
        public static QuoteRequest ToEntity(this QuoteRequestViewModel viewModel, QuoteRequest entity = null)
        {
            if (entity == null)
                entity = new QuoteRequest();

            entity.Id = viewModel.Id;
            entity.ProductDisplayGroupId = viewModel.FuelDetails.FuelDisplayGroupId;
            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                entity.FuelDescription = viewModel.FuelDetails.NonStandardFuelDescription;
            }
            entity.FuelTypeId = Convert.ToInt32(viewModel.FuelDetails.FuelTypeId);
            entity.EstimatedGallonsPerDelivery = viewModel.EstimatedGallonsPerDelivery.HasValue ? viewModel.EstimatedGallonsPerDelivery.Value : 0;
            entity.OrderTypeId = viewModel.FuelDetails.OrderTypeId;
            entity.IsPublicRequest = viewModel.PrivateSupplierList.IsPublicRequest;
            entity.Quantity = viewModel.Quantity;
            entity.QuoteDueDate = viewModel.QuoteDueDate;
            entity.QuotesNeeded = viewModel.QuotesNeeded;
            entity.IncludeFees = viewModel.IncludeFees;
            entity.Notes = viewModel.Notes;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.DeliveryTypeId = viewModel.DeliveryTypeId;
            entity.PaymentTermId = viewModel.FuelOfferDetails.PaymentTermId;
            entity.NetDays = viewModel.FuelOfferDetails.NetDays;
            if (entity.QuoteRequestStatuses.Count > 0)
            {
                entity.QuoteRequestStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
            }
            QuoteRequestStatus quoteRequestStatus = new QuoteRequestStatus();
            quoteRequestStatus.QuoteRequestId = entity.Id;
            quoteRequestStatus.StatusId = (int)QuoteRequestStatuses.Open;
            quoteRequestStatus.IsActive = true;
            quoteRequestStatus.UpdatedBy = viewModel.CreatedBy;
            quoteRequestStatus.UpdatedDate = DateTimeOffset.Now;
            entity.QuoteRequestStatuses.Add(quoteRequestStatus);
            entity.Currency = viewModel.FuelDetails.FuelPricing.Currency;
            entity.UoM = viewModel.FuelDetails.FuelQuantity.UoM;

            entity.UpdatedBy = viewModel.CreatedBy;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            return entity;
        }

        public static Quotation ToEntity(this QuoteResponseViewModel viewModel, Quotation entity = null)
        {
            if (entity == null)
                entity = new Quotation();

            entity.QuoteRequestId = viewModel.QuoteRequest.Id;
            entity.QuoteNumber = viewModel.SupplierQuoteNumber;
            entity.PricingTypeId = viewModel.FuelPricing.PricingTypeId;
            entity.PricePerGallon = 0;
            entity.RackAvgTypeId = entity.SupplierCostTypeId = null;

            if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                entity.PricePerGallon = viewModel.FuelPricing.PricePerGallon  ;
            }
            else if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.RackAverage)
            {
                entity.PricePerGallon = viewModel.FuelPricing.RackPrice ;
                entity.RackAvgTypeId = viewModel.FuelPricing.RackAvgTypeId;
                if (viewModel.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
            }
            else if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                entity.RackAvgTypeId = viewModel.FuelPricing.SupplierCostMarkupTypeId;
                entity.SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost;
                entity.SupplierCost = viewModel.FuelPricing.SupplierCost;
                entity.PricePerGallon = viewModel.FuelPricing.SupplierCostMarkupValue; 
                entity.BaseSupplierCost = MoneyConverter.GetBaseAmount(viewModel.QuoteRequest.Currency, viewModel.FuelPricing.SupplierCost ?? 0, viewModel.ExchangeRate);
            }
            entity.BasePrice = MoneyConverter.GetBaseAmount(viewModel.QuoteRequest.Currency, entity.PricePerGallon, viewModel.ExchangeRate);
            entity.QuoteNumber = viewModel.SupplierQuoteNumber;
            entity.Notes = viewModel.Notes;
            entity.UoM = viewModel.QuoteRequest.UoM;
            entity.Currency = viewModel.QuoteRequest.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;

            entity.Image = viewModel.Image == null || !string.IsNullOrWhiteSpace(viewModel.Image?.FilePath)  || viewModel.Image.IsRemoved ? null : viewModel.Image.ToEntity();
            entity.UpdatedBy = viewModel.CreatedBy;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            return entity;
        }

        public static QuoteResponseViewModel ToViewModel(this Quotation entity, QuoteResponseViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new QuoteResponseViewModel();

            viewModel.QuotationId = entity.Id;
            viewModel.QuoteRequest = entity.QuoteRequest.ToViewModel();

            viewModel.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId;
            if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            }
            else if (entity.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
            }
            else if (entity.PricingTypeId == (int)PricingType.RackHigh)
            {
                viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
            }
            else if (entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
            }
            else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                viewModel.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            }

            if (entity.PricingTypeId == (int)PricingType.RackAverage
            || entity.PricingTypeId == (int)PricingType.RackHigh
            || entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            }
            
            //TODO: add status for accepted / awarded

            viewModel.FuelTypeId = viewModel.QuoteRequest.FuelTypeId;

            viewModel.Notes = entity.Notes;
            viewModel.FuelDeliveryDetails.FuelRequestFee = entity.QuotationFees.ToViewModel();
            viewModel.FuelDeliveryDetails.FuelRequestFee.AdditionalFee = entity.QuotationFees.ToAdditionalFeeViewModel().ToList();
            viewModel.SupplierQuoteNumber = entity.QuoteNumber;
            viewModel.FuelPricing.Currency = entity.Currency;

            return viewModel;
        }

        public static QuoteRequestViewModel ToQuoteRequestViewModel(this QuoteRequest entity, QuoteRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new QuoteRequestViewModel();

            viewModel.Id = entity.Id;
            viewModel.FuelDetails.FuelDisplayGroupId = entity.ProductDisplayGroupId;
            viewModel.FuelDetails.FuelTypeId = entity.FuelTypeId;
            viewModel.FuelDetails.NonStandardFuelName = entity.FuelDescription;
            viewModel.EstimatedGallonsPerDelivery = entity.EstimatedGallonsPerDelivery;
            viewModel.FuelDetails.OrderTypeId = entity.OrderTypeId;
            viewModel.PrivateSupplierList.IsPublicRequest = entity.IsPublicRequest;
            viewModel.Quantity = entity.Quantity;
            viewModel.QuoteDueDate = entity.QuoteDueDate;
            viewModel.QuotesNeeded = entity.QuotesNeeded;
            viewModel.IncludeFees = entity.IncludeFees;
            viewModel.Notes = entity.Notes;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;

            viewModel.DeliveryTypeId = entity.DeliveryTypeId;
            viewModel.Qualifications = entity.MstSupplierQualifications.Select(t => t.Id).ToList();
            return viewModel;
        }

        public static QuoteRequestDetailsViewModel ToViewModel(this QuoteRequest entity, QuoteRequestDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new QuoteRequestDetailsViewModel();

            viewModel.Id = entity.Id;
            viewModel.JobName = entity.Job.Name;
            viewModel.JobId = entity.Job.Id;
            viewModel.JobStateId = entity.Job.StateId;
            string addCity = string.Empty;

            if (entity.Job.LocationType != JobLocationTypes.Various)
            {
                if ((!string.IsNullOrWhiteSpace(entity.Job.Address)) && (!string.IsNullOrWhiteSpace(entity.Job.City)))
                    addCity = $"{entity.Job.Address}, {entity.Job.City}";

                viewModel.Address = $"{addCity}, {entity.Job.MstState.Code}, {entity.Job.ZipCode}";
                viewModel.Address = viewModel.Address.TrimStart(',');
            }
            else
            {
                viewModel.Address = entity.Job.MstState.Code;
            }
            viewModel.QuoteNumber = entity.RequestNumber;
            viewModel.QuoteDueDate = entity.QuoteDueDate.ToString(Resource.constFormatDate);
            viewModel.QuoteDueDateUpdated = entity.QuoteDueDate;
            viewModel.RequestType = entity.IsPublicRequest ? Resource.lblPublic : Resource.lblPrivate;
            viewModel.FuelTypeId = entity.FuelTypeId;
            viewModel.FuelType = ContextFactory.Current.GetDomain<HelperDomain>().GetProductName(entity.MstProduct);
            viewModel.OrderType = entity.MstOrderType.Name;
            viewModel.Quantity = entity.Quantity;
            viewModel.DeliveryStartDate = entity.StartDate.ToString(Resource.constFormatDate);
            viewModel.EndDate = entity.EndDate != null ? entity.EndDate.Value.Date.ToString(Resource.constFormatDate) : Resource.lblHyphen;
            viewModel.DeliveryType = entity.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries ? Resource.lblMultiple : Resource.lblSingle;
            viewModel.EstimatedGallonsPerDelivery = Convert.ToString(entity.EstimatedGallonsPerDelivery);
            viewModel.IncludeFees = entity.IncludeFees;
            viewModel.SupplierDBE = entity.MstSupplierQualifications.Select(t => t.Name).ToList();
            viewModel.Notes = entity.Notes;

            // Priority - Awarded, Canceled, Completed, Expired, Open
            viewModel.Status = entity.QuoteRequestStatuses.Any(t => t.StatusId == (int)QuoteRequestStatuses.Awarded) ? Enum.GetName(typeof(QuoteRequestStatuses), (int)QuoteRequestStatuses.Awarded) :
                                entity.QuoteRequestStatuses.Any(t => t.StatusId == (int)QuoteRequestStatuses.Canceled) ? Enum.GetName(typeof(QuoteRequestStatuses), (int)QuoteRequestStatuses.Canceled) :
                                entity.Quotations.Count(t => t.IsActive) >= entity.QuotesNeeded ? Enum.GetName(typeof(QuoteRequestStatuses), (int)QuoteRequestStatuses.Completed) :
                                entity.QuoteDueDate.Date < DateTimeOffset.Now.ToTargetDateTimeOffset(entity.Job.TimeZoneName).Date ? Enum.GetName(typeof(QuoteRequestStatuses), (int)QuoteRequestStatuses.Expired) :
                                Enum.GetName(typeof(QuoteRequestStatuses), entity.QuoteRequestStatuses.First(t => t.IsActive).StatusId);

            if (entity.QuoteRequestDocuments.Count(t => t.IsActive) > 0)
            {
                viewModel.Documents = new List<DocumentViewModel>();
                foreach (var item in entity.QuoteRequestDocuments.Where(t => t.IsActive))
                {
                    var documentDetails = new DocumentViewModel();
                    documentDetails.Id = item.Id;
                    documentDetails.AddedBy = item.CreatedBy;
                    documentDetails.FileName = item.FileName;
                    documentDetails.ModifiedFileName = item.ModifiedFileName;
                    viewModel.Documents.Add(documentDetails);
                }
            }
            viewModel.QuotesNeeded = entity.QuotesNeeded;
            viewModel.QuotesNeededUpdated = entity.QuotesNeeded;
            viewModel.QuotesReceived = entity.Quotations.Count(t => t.IsActive);
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;
            viewModel.PaymentTermName = entity.MstPaymentTerm != null ? entity.MstPaymentTerm.Name : Resource.lblHyphen;
            viewModel.NetDays = entity.NetDays;
            viewModel.PaymentTermId = entity.PaymentTermId;
            return viewModel;
        }

        public static QuoteDetailsViewModel ToDetailsViewModel(this Quotation entity, QuoteDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new QuoteDetailsViewModel();

            viewModel.QuoteRequest = entity.QuoteRequest.ToViewModel();

            viewModel.Id = entity.Id;
            viewModel.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId;
            viewModel.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            }
            else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                viewModel.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            }
            else
            {
                viewModel.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            }

            if (entity.QuoteRequestDocuments.Any(t => t.IsActive))
            {
                viewModel.Documents = new List<DocumentViewModel>();
                foreach (var item in entity.QuoteRequestDocuments.Where(t => t.IsActive))
                {
                    var documentDetails = new DocumentViewModel();
                    documentDetails.Id = item.Id;
                    documentDetails.AddedBy = item.CreatedBy;
                    documentDetails.FileName = item.FileName;
                    documentDetails.ModifiedFileName = item.ModifiedFileName;
                    viewModel.Documents.Add(documentDetails);
                }
            }

            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = entity.QuotationFees.ToFeesViewModel();
            viewModel.FuelRequestFee = entity.QuotationFees.ToViewModel();
            //viewModel.FuelRequestFee.DeliveryFeeByQuantity = entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
            //viewModel.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity = entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
            //viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => { if (t.FeeSubTypeId == (int)FeeSubType.ByQuantity) t.DeliveryFeeByQuantity.AddRange(viewModel.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity); });
            viewModel.FuelRequestFee.AdditionalFee = entity.QuotationFees.ToAdditionalFeeViewModel().ToList();
            viewModel.SupplierQuoteNumber = entity.QuoteNumber;
            viewModel.CreatedDate = entity.CreatedDate.ToString(Resource.constFormatDate);
            viewModel.QuotationCreatedBy = $"{entity.User.FirstName} {entity.User.LastName}";
            viewModel.QuotationCompany = entity.User.Company.Name;
            viewModel.QuoteRequestCreatedBy = $"{entity.QuoteRequest.User.FirstName} {entity.QuoteRequest.User.LastName}";
            viewModel.QuoteRequestCompany = entity.QuoteRequest.User.Company.Name;
            viewModel.SupplierQuoteNumber = entity.QuoteNumber;
            viewModel.IsTncIncluded = entity.IsTnCIncluded;
            viewModel.QuotationStatusId = entity.QuotationStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            viewModel.QuotationStatus = entity.QuotationStatuses.FirstOrDefault(t => t.IsActive).MstQuoteRequestStatus.Name;

            viewModel.Notes = entity.Notes;
            return viewModel;
        }
    }
}
