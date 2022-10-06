namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;

    public partial class FavoriteSideMenu
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string LinkId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
