using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public CustomerDetailsViewModel()
        {
            CustomerContact = new ContactPersonViewModel();
            CustomerAddress = new AddressViewModel();
            FuelSection = new List<FuelSection>();
            PrivateListSection = new List<PrivateListSection>();
            BillToInfo = new JobSpecificBillToViewModel();
        }

       public  JobSpecificBillToViewModel BillToInfo { get; set; }

        public bool IsBuyerAccount { get; set; }
        public int SelectedJobId { get; set; }

        public List<DropdownDisplayItem> JobList { get; set; }

        public int OnboardedTypeId { get; set; }

        public bool IsDirectTax { get; set; }

        public bool IsEditDirectTax { get; set; }

        public List<DirectTaxesViewModel> DirectTaxes { get; set; } = new List<DirectTaxesViewModel>();

        //supplier info section
        public int SupplierCompanyId { get; set; }
        public string CustomerCompanyName { get; set; }
        public ContactPersonViewModel CustomerContact { get; set; }
        public AddressViewModel CustomerAddress { get; set; }
        public DateTime ConnectedSince { get; set; }

        //Order section
        public decimal GallonsOrdered { get; set; }
        public decimal GallonsDelivered { get; set; }
        public decimal GallonsRemaining { get; set; }
        public decimal AvgGallonsPerDrop { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AvgPpgPerDrop { get; set; }
        public int TotalOrders { get; set; }
        public int OpenOrders { get; set; }
        public int ClosedOrders { get; set; }
        public int CanceledOrdersByYou { get; set; }
        public int CanceledOrdersByCustomer { get; set; }

        //deliveries section
        public int TotalDeliveries { get; set; }
        public int ScheduledDeleveries { get; set; }
        public int OntimeDeliveries { get; set; }
        public int LateDeliveries { get; set; }
        public int RescheduledByYou { get; set; }
        public int RescheduledByCustomer { get; set; }

        //Pricing section
        public int TotalDdtCount { get; set; }
        public int TotalInvoiceCount { get; set; }
        public int TotalDryRunCount { get; set; }
        //public decimal TotalAmount { get; set; }
        //public decimal AvgPpgPerDrop { get; set; }
        public decimal TotalFees { get; set; }

        //other section
        public List<PrivateListSection> PrivateListSection { get; set; }
        public string CreditApplication { get; set; } //for supplier side-customer details

        public string NextScheduledDelievery { get; set; }
        public bool IsTaxExemption { get; set; }
        
        //fuel section
        public List<FuelSection> FuelSection { get; set; }

        //approval section - for supplier side customer details
        public bool TaxExemptions { get; set; }
        public int TotalApprovals { get; set; }
        public int ApprovalDDTs { get; set; }
        public int ApprovalInvoices { get; set; }
        public int TotalRejected { get; set; }
        public int RejectedDDTs { get; set; }
        public int RejectedInvoices { get; set; }
        public bool IsExceptionEnabled { get; set; }
        public string AccountingCompanyId { get; set; }
        public int CustomerCompanyId { get; set; }

        public bool IsSAPCreditCheckEnabled { get; set; }

        public string GetTFCUIdForCustomer()
        {
            return ApplicationConstants.CustomerNumberPrefix + CustomerCompanyId.ToString(ApplicationConstants.SevenDigit);
        }
    }

    public class FuelSection
    {
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int TotalOrdersOfFuelType { get; set; }
        public decimal GallonsOrderedOfFuelType { get; set; }
        public decimal AvgPpgPerOrder { get; set; }
        public decimal AvgGallonsPerDrop { get; set; }
        public int InvoiceCountOfFuelType { get; set; }
    }

    public class PrivateListSection
    {
        public int ListId { get; set; }
        public string ListName { get; set; }
        public DateTime CreatedDate  { get; set; }
        public string LastUsed { get; set; }
    }
}
