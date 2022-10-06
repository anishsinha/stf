using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
	public class InvoiceDetailViewModel : BaseCultureViewModel
	{
		public InvoiceDetailViewModel()
		{
			InstanceInitialize();
		}

		public InvoiceDetailViewModel(Status status)
			: base(status)
		{
			InstanceInitialize(status);
		}

		private void InstanceInitialize(Status status = Status.Failed)
		{
			Invoice = new InvoiceViewModel(status);
			FuelRequest = new FuelRequestViewModel(status);
			AssetDropImages = new List<ImageViewModel>();
			InvoiceImage = new ImageViewModel(status);
			BolImage = new ImageViewModel(status);
			CreditCheckApprovalImage = new ImageViewModel(status);
			FuelRequestFee = new FuelRequestFeeViewModel();
			PaymentDiscount = new PaymentDiscountViewModel();
			BrokeredOrder = new TPOBrokeredOrderViewModel();
			FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
			CustomerSignature = new CustomerSignatureViewModel();
		}

		public InvoiceViewModel Invoice { get; set; }

		public int LinkedInvoiceId { get; set; }

		public int LinkedInvoiceType { get; set; }

		public string LinkedInvoiceNumber { get; set; }

		public ImageViewModel SignatureImage { get; set; }

		public ImageViewModel InvoiceImage { get; set; }

		public List<ImageViewModel> InvoiceImages
		{
			get
			{
				var images = new List<ImageViewModel>();
				if (InvoiceImage != null && !string.IsNullOrEmpty(InvoiceImage.FilePath))
					images = BreakFilePathToMany(InvoiceImage);

				return images;
			}
		}

		

		public ImageViewModel BolImage { get; set; }

		public List<ImageViewModel> BolImages {
			get
			{
				var images = new List<ImageViewModel>();
				if (BolImage != null && !string.IsNullOrEmpty(BolImage.FilePath))
					images = BreakFilePathToMany(BolImage);

				return images;
			}
		}

		public ImageViewModel AdditionalImage { get; set; }

		public ImageViewModel CreditCheckApprovalImage { get; set; }

		public List<ImageViewModel> CreditCheckApprovalImages
		{
			get
			{
				var images = new List<ImageViewModel>();
				if (CreditCheckApprovalImage != null && !string.IsNullOrEmpty(CreditCheckApprovalImage.FilePath))
					images = BreakFilePathToMany(CreditCheckApprovalImage);

				return images;
			}
		}

		public List<ImageViewModel> AdditionalImages {
			get
			{
				var images = new List<ImageViewModel>();
				if (AdditionalImage != null && !string.IsNullOrEmpty(AdditionalImage.FilePath))
					images = BreakFilePathToMany(AdditionalImage);

				return images;
			}
		}



		public List<ImageViewModel> AssetDropImages { get; set; }

		public string PoNumber { get; set; }

		public int OrderId { get; set; }

		public string SupplierName { get; set; }

		public string SupplierEmail { get; set; }
        public string SupplierPhone { get; set; }

		public string BuyerCompanyName { get; set; }

		public string SupplierCompanyName { get; set; }

		public string PercentFuelDelivered { get; set; }

		public decimal StateTax { get; set; }

		public decimal FederalTax { get; set; }

		public decimal SalesTax { get; set; }

		public int AssetCount { get; set; }

		public decimal TotalInvoiceAmount { get; set; }

		public FuelRequestViewModel FuelRequest { get; set; }

		public string TerminalName { get; set; }
		public int? CityGroupTerminalId { get; set; }

		public double Distance { get; set; }

		public FuelRequestFeeViewModel FuelRequestFee { get; set; }

		public PaymentDiscountViewModel PaymentDiscount { get; set; }

		public int PaymentTermId { get; set; }

		public int NetDays { get; set; }

		public PaymentMethods PaymentMethod { get; set; }

		public int ActingCompanyType { get; set; }

		public int? ApprovalUserId { get; set; }

		public string ApprovalUserName { get; set; }

		public bool IsApprovalWorkflowEnabled { get; set; }

		public bool IsRejectedAndWaitingApproval { get; set; }

		public bool IsHidePricingEnabled { get; set; }

		public int? DriverId { get; set; }

		public string DriverName { get; set; }

		public string TrackableSchedule { get; set; }

		public int ExternalBrokerId { get; set; }

		public int? ParentId { get; set; }

		public int StatusId { get; set; }

		public TPOBrokeredOrderViewModel BrokeredOrder { get; set; }

		public bool IsBuyAndSellOrder { get; set; }

		public BuyAndSellPricingDetailViewModel BuyAndSellPricingDetail { get; set; }

		public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

		public bool IsEndSupplier { get; set; }

		public CustomerSignatureViewModel CustomerSignature { get; set; }

		public int SupplierCompanyId { get; set; }

		public bool IsFTL { get; set; }

		public string RefId { get; set; }
		public string Password { get; set; }
		public string SiteNumber { get; set; }
		public int BuyerCompanyId { get; set; }
		public string SplitLoadChainId { get; set; }
		public List<InvoiceNumberViewModel> SplitLoadInvoices { get; set; }
		public string DisplayPricePerGallon { get; set; }
		public string Notes { get; set; }
		public int InvoiceNumberId { get; set; }
		public decimal AmountPaid { get; set; }
		public decimal BalanceRemaining { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public int? OriginalInvoiceId { get; set; }
		public string OriginalInvoiceNumber { get; set; }
		public int? CreditInvoiceId { get; set; }
		public string CreditInvoiceDisplayNumber { get; set; }
		public bool ShowEditInvoiceMenu { get; set; }
		public bool ShowCreditRebillMenu { get; set; }
		public bool IsExceptionDdt { get; set; }

		public bool IsSingleBolInvoice { get; set; }
        public bool IsLiftFileValidated { get; set; }
        public bool IsMarineLocation { get; set; }
		public int countryId { get; set; }
		public OnboardingPreferenceViewModel PreferencesSetting { get; set; }
        public int? JobCountryId { get; set; }
        public string DeliveryLevelPO { get; set; }
        public string BDRNumber { get; set; }
        public bool CanEdit { get; set; }
        private List<ImageViewModel> BreakFilePathToMany(ImageViewModel imageViewModel)
		{
			List<ImageViewModel> images;
			{
				images = imageViewModel.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
					Select(x => new ImageViewModel { FilePath = x.Trim(), IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
			}

			return images;
		}
	}
}
