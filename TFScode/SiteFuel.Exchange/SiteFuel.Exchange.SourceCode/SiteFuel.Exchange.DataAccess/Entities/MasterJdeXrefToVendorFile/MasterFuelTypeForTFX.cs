namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  

    public class MasterFuelTypeForTFX
    {

        public int Id { get; set; }
        public string Fuel_Category { get; set; }
        public string Fuel_Category_Description_ { get; set; }
    }
}
