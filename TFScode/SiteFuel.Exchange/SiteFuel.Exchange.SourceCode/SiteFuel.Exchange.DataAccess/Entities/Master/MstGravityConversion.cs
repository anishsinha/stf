namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;

    public partial class MstGravityConversion
    {
        public int Id { get; set; }

        public decimal Gravity { get; set; }

        public decimal GallonsPerMetricTon { get; set; }
    }
}
