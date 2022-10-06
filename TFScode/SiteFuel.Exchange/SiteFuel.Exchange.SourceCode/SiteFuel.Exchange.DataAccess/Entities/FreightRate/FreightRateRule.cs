namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRateRule
    {
        public FreightRateRule()
        {
            FreightRateFuelGroups = new HashSet<FreightRateFuelGroup>();
            FreightTableCompanies = new HashSet<FreightTableCompany>();
            FreightTablePickupLocations = new HashSet<FreightTablePickupLocation>();
            FreightTableSourceRegions = new HashSet<FreightTableSourceRegion>();
            FreightRateRouteTables = new HashSet<FreightRateRouteTable>();
            FreightRateRangeTables = new HashSet<FreightRateRangeTable>();
            FreightRatePtoPTables = new HashSet<FreightRatePtoPTable>();
        }

        [Key]
        public int Id { get; set; }
        public TableTypes TableType { get; set; }
        public FreightRateRuleType FreightRateRuleType { get; set; }
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public FuelGroupType FuelGroupType { get; set; }
        public int MixLoadMinValue { get; set; }
        public FreightRateCalcPreferenceType FreightRateCalcPreferenceType { get; set; }
        public int? FreightRateCalcPrefFuelGroupId { get; set; }
        public FreightTableStatus Status { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByCompanyId { get; set; } 
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public virtual ICollection<FreightRateFuelGroup> FreightRateFuelGroups { get; set; }
        [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }
        public virtual ICollection<FreightTableCompany> FreightTableCompanies { get; set; }
        public virtual ICollection<FreightTablePickupLocation> FreightTablePickupLocations { get; set; }
        public virtual ICollection<FreightTableSourceRegion> FreightTableSourceRegions { get; set; }
        public virtual ICollection<FreightRateRouteTable> FreightRateRouteTables { get; set; }
        public virtual ICollection<FreightRateRangeTable> FreightRateRangeTables { get; set; }
        public virtual ICollection<FreightRatePtoPTable> FreightRatePtoPTables { get; set; }
    }
}
