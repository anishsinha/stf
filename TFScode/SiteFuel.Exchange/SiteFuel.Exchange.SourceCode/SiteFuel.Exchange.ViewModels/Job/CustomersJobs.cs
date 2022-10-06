using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Job
{
  public  class CustomersJobs 
    {

        public CustomersJobs()
        {
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set;}
        public List<CustomerLocations> Locations { get; set; }
    }
    public class CustomerLocations : BaseViewModel
    {
        public CustomerLocations()
        {
        }
        public int JobId { get; set;}
        public string Name { get; set; }
    }

}
