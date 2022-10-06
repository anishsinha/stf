using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class SpecialInstructionViewModel : StatusViewModel
    {
        public SpecialInstructionViewModel()
        {
            InstanceInitialize();
        }

        public SpecialInstructionViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Id = 0;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblSpecialInstruction), ResourceType = typeof(Resource))]
        public string Instruction { get; set; }
    }
}