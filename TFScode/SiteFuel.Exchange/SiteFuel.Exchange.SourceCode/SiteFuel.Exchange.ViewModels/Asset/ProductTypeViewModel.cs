using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductTypeViewModel : BaseViewModel
    {
        public ProductTypeViewModel()
        {
        }

        public ProductTypeViewModel(Status status)
            : base(status)
        {
        }

        public Nullable<int> Id { get; set; }

        public string Name { get; set; }
    }
}
