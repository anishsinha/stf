using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class ExternalSuppliersCsvRecordViewModel
    {
        [FieldQuoted]
        public string CompanyName { get; set; }

        [FieldQuoted]
        public string CompanyType { get; set; }

        [FieldQuoted]
        public string Website { get; set; }

        [FieldQuoted]
        public string InPipedrive { get; set; }

        [FieldQuoted]
        public string ContactPersonName { get; set; }

        [FieldQuoted]
        public string ContactPersonPhoneNo { get; set; }

        [FieldQuoted]
        public string ContactPersonEmail { get; set; }

        [FieldQuoted]
        public string Address { get; set; }

        [FieldQuoted]
        public string City { get; set; }

        [FieldQuoted]
        public string State { get; set; }

        [FieldQuoted]
        public string ZipCode { get; set; }

        [FieldQuoted]
        public string PhoneNo { get; set; }

        [FieldQuoted]
        public string Address1 { get; set; }

        [FieldQuoted]
        public string City1 { get; set; }

        [FieldQuoted]
        public string State1 { get; set; }

        [FieldQuoted]
        public string ZipCode1 { get; set; }

        [FieldQuoted]
        public string PhoneNo1 { get; set; }
       
        [FieldQuoted]
        public string Address2 { get; set; }

        [FieldQuoted]
        public string City2 { get; set; }

        [FieldQuoted]
        public string State2 { get; set; }

        [FieldQuoted]
        public string ZipCode2 { get; set; }

        [FieldQuoted]
        public string PhoneNo2 { get; set; }

        [FieldQuoted]
        public string TruckType { get; set; }

        [FieldQuoted]
        public string HowManyTrucks { get; set; }       

        [FieldQuoted]
        public string FuelType { get; set; }

        [FieldQuoted]
        public string DBE { get; set; }      

        [FieldQuoted]
        public string ServingStates { get; set; }

        [FieldQuoted]
        public string SpecificRadius { get; set; }

        [FieldQuoted]
        public string Notes { get; set; }
    }
}
