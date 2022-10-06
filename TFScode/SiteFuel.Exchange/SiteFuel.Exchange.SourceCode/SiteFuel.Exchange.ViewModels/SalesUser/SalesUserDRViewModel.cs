using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.SalesUser
{
    public class SalesUserDRViewModel : BaseViewModel
    {
        public SalesUserDRViewModel()
        {
            InstanceInitialize();
        }

        public SalesUserDRViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            this.Products = new List<ProductDetails>();


        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelName { get; set; }
        public decimal Quantity { get; set; }
        public UoM UoM { get; set; }
        public ProductDisplayGroups FuelType { get; set; }
        public QuantityType QuantityType { get; set; }
        public string StartDate { get; set; }
        public DateTimeOffset StartDate1 { get; set; }
        public string StartTime { get; set; }
        public TimeSpan StartTime1 { get; set; }

        public string EndTime { get; set; }
        public TimeSpan EndTime1 { get; set; }

        public string DRNotes { get; set; }
        public string RequestedBy { get; set; }
        public bool IsAdditive { get; set; }
        public List<ProductDetails> Products { get; set; }


    }

    public class ProductDetails
    {
        public int FuelTypeId { get; set; }
        public string FuelName { get; set; }
        public decimal Quantity { get; set; }
        public UoM UoM { get; set; }
        public DropdownDisplayItem FuelTypes = new DropdownDisplayItem(); 
        public ProductDisplayGroups FuelType { get; set; }
        public QuantityType QuantityType { get; set; }
        public string StartDate { get; set; }
        public DateTimeOffset StartDate1 { get; set; }
        public string StartTime { get; set; }
        public TimeSpan StartTime1 { get; set; }

        public string EndTime { get; set; }
        public TimeSpan EndTime1 { get; set; }
        public string DRPO { get; set; }

    }

    public class CustomersAndJobs
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
    }

    public class CustomersModel
    {
        public List<RegionsAndJobsModel> regionsAndJobsModels = new List<RegionsAndJobsModel>();
        public List<CustomersAndJobs> customersandJobs = new List<CustomersAndJobs>();
    }

    public class RegionsAndJobsModel
    {
        public string RegionId { get; set; }
        public int CompanyId { get; set; }
        public List<DropdownDisplayItem> Jobs = new List<DropdownDisplayItem>();
        
    }

    public class FuelProducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum SalesUserDRStatus
    {
        Success = 1,
        Error = 2,
        RegionNotFound = 3,
        FuelRequestNotFound = 4,
        OrderNotFound = 5
    }
    public class SalesUserDRStatusModel
    {
        public SalesUserDRStatus State { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class SalesUserDRProductStatus
    {
        public SalesUserDRStatusModel Status = new SalesUserDRStatusModel();
        public ProductDetails Product = new ProductDetails();
    }

    public class SalesUserDRValidationModel
    {
        public List<SalesUserDRProductStatus> ProductStatuses = new List<SalesUserDRProductStatus>();
        public List<RaiseDeliveryRequestInput> RaiseDeliveryRequestInput = new List<RaiseDeliveryRequestInput>();

    }

    public class DRValidationModel
    {
        public SalesUserDRProductStatus ProductStatuses = new SalesUserDRProductStatus();
        public RaiseDeliveryRequestInput RaiseDeliveryRequestInput = new RaiseDeliveryRequestInput();

    }


}




