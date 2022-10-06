using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestResaleFeeViewModel : BaseViewModel
    {
        public FuelRequestResaleFeeViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestResaleFeeViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FeeTypeId = (int)FeeType.ResaleFee;
        }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public string FeeSubTypeName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Fee { get; set; }

        public Currency Currency { get; set; }
    }
}
