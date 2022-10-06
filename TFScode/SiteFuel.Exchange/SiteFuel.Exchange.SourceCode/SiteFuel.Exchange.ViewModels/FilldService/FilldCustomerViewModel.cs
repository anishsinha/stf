using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldCustomerViewModel
    {
        public string external_customer_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string locale { get; set; }
    }
    public class CustomerDataModel
    {
        public long Id { get; set; }
        public DateTimeOffset Created_at { get; set; }
    }

    public class CustomerResponseDataModel
    {
        public CustomerDataModel user { get; set; }
    }

    public class FilldCustomerResponseModel : FilldStatusViewModel
    {
        public CustomerResponseDataModel Data { get; set; }
    }
}
