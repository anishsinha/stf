using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiOrderDetailsForJobViewModel
    {
        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        public int FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductType { get; set; }

        public int? TerminalId { get; set; }

        public bool IsSignatureRequired { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsBOLImageRequired { get; set; }

        public bool IsDropImageRequired { get; set; }
        public bool IsPrePostDipEnabled { get; set; }
    }
}
