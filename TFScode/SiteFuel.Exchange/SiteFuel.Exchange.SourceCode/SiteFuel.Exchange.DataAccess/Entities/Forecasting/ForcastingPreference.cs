using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("ForcastingPreference")]
    public partial class ForcastingPreference
    {
        public int Id { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public int ForcastingServicePreference { get; set; } = 1; // buyer or supplier,, by default buyer.
        public int? ForcastingSettingId { get; set; }
        [ForeignKey("ForcastingSettingId")]
        public virtual ForcastingServiceSetting ForcastingServiceSetting { get; set; }
        public int ForcastingSettingLevel { get; set; } //Levels :1) Account,2)Job,3)Tank
        public int EntityId { get; set; } //Example : //account preference setting id when forcasting setting level is account. //Job id when forecasting setting level is job.//Asset id when forecasting setting level is Tank.
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
