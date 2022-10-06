namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferLocationExclusion
    {
        public int Id { get; set; }

        public int ExclusionTypeId { get; set; } //1 for State 2 for City/Zip

        public Nullable<int> StateId { get; set; }

        public Nullable<int> CityId { get; set; }

        public Nullable<int> TierId { get; set; }

        public Nullable<int> CustomerId { get; set; }

        public string ZipCode { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        [ForeignKey("CityId")]
        public virtual MstCity MstCity { get; set; }

        [ForeignKey("TierId")]
        public virtual MstTierType MstTierType { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Company Company { get; set; }
    }
}
