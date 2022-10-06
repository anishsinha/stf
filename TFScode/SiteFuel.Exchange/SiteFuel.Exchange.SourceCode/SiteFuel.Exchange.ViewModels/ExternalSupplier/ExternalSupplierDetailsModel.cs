using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalSupplierDetailsModel
    {
        public ExternalSupplierDetailsModel()
        {
            CompanyAddress = new ExternalSupplierAddressDetail();
            OtherLocationsAndServices = new List<ExternalSupplierAddressDetail>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string CompanyType { get; set; }

        public string Website { get; set; }

        public bool InPipedrive { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonPhoneNumber { get; set; }

        public string ContactPersonEmail { get; set; }

        public string Status { get; set; }

        public ExternalSupplierAddressDetail CompanyAddress { get; set; }

        public List<ExternalSupplierAddressDetail> OtherLocationsAndServices { get; set; }
    }

    public  class ExternalSupplierAddressDetail
    {
        public ExternalSupplierAddressDetail()
        {
            SupplierDBE = new List<string>();
            SupplierTrucks = new List<int>();
        }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

         public string ZipCode { get; set; }
       
        public string PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public int? NumberOfTrucks { get; set; }

        public bool IsStateWideService { get; set; }

        public int Radius { get; set; }
       
        public string SupplierProductTypes { get; set; }

        public List<string> SupplierDBE { get; set; }

        public string SupplierServingStates { get; set; }

        public List<int> SupplierTrucks { get; set; }

    }

    public class ExternalSupplierNotesGridViewModel
    {
        public int Id { get; set; }

        public string Notes { get; set; }

        public string DateAdded { get; set; }

        public string AddedBy { get; set; }

        public bool IsCompleted { get; set; }

        public string DateCompleted { get; set; }

        public string CompletedBy { get; set; }
    }

    public class ExternalSupplierStatusesGridViewModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string DateAdded { get; set; }

        public string AddedBy { get; set; }
    }
}
