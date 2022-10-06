using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverDeliveryDetailsViewModel
    {
        public DriverDeliveryDetailsViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            ContactPersons = new List<ContactPersonViewModel>();
            SpecialInstructions = new List<string>();
        }

        public string CompanyName { get; set; }

        public string JobName { get; set; }

        public string JobAddress { get; set; }

        public string JobCity { get; set; }

        public string JobState { get; set; }

        public string JobZip { get; set; }

        public decimal JobLatitude { get; set; }

        public decimal JobLongitude { get; set; }

        public List<ContactPersonViewModel> ContactPersons { get; set; }

        public List<string> SpecialInstructions { get; set; }

        public SpecialInstructionAttachmentViewModel SpecialInstructionFiles { get; set; }
    }
}
