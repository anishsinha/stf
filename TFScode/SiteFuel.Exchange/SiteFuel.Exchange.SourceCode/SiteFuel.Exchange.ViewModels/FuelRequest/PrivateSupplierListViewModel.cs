using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class PrivateSupplierListViewModel : BaseViewModel
    {
        public PrivateSupplierListViewModel()
        {
            InstanceInitialize();
        }
		
		public PrivateSupplierListViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            IsPublicRequest = true;
            IsNewSupplierList = false;
            Suppliers = new List<int>();
            //PrivateSupplierIds = new List<int>();
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPublicOrPrivate), ResourceType = typeof(Resource))]
        public bool IsPublicRequest { get; set; }

        [RequiredIfFalse("IsPublicRequest", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPrivateSupplierList), ResourceType = typeof(Resource))]
        public int? Id { get; set; }

        [Display(Name = nameof(Resource.lblPrivateSupplierList), ResourceType = typeof(Resource))]
        [RequiredIfFalse("IsPublicRequest", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public IList<int> PrivateSupplierIds { get; set; }

        [RequiredIfTrue("IsNewSupplierList", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Remote("IsValidSupplierListName", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string Name { get; set; }

        public bool IsNewSupplierList { get; set; }

        [RequiredIfTrue("IsNewSupplierList", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.gridColumnSuppliers), ResourceType = typeof(Resource))]
        public List<int> Suppliers { get; set; }

        public int AddedById { get; set; }

        public string AddedByName { get; set; }

        public int CompanyId { get; set; }

        public int SuppliersCount { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDt { get; set; }

        public string UpdatedByName { get; set; }

        public bool IsAllowDelete { get; set; }
    }
}
