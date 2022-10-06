using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierGeoViewModel
    {
        public SupplierGeoViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            SupplierFuelTypes = new List<int>();
            Suppliers = new List<SupplierDetailViewModel>();
            State = new StateViewModel();
        }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]        
        public IList<int> SupplierFuelTypes { get; set; }

        public bool IncludeAllLocations { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int Radius { get; set; }

        public Nullable<int> AccountTypeId { get; set; }

        [Display(Name = nameof(Resource.lblSupplierType), ResourceType = typeof(Resource))]
        public int SupplierType { get; set; }

        public bool SearchFlag { get; set; }

        public StateViewModel State { get; set; }

        public List<SupplierDetailViewModel> Suppliers { get; set; }
    }
}
