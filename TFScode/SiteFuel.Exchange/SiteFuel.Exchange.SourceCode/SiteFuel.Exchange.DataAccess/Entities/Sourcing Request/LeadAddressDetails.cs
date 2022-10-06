using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadAddressDetail
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int JobId { get; set; }
        [StringLength(256)]
        public string JobName { get; set; }
        [StringLength(256)]
        public string DisplayJobID { get; set; }
        public int CountryId { get; set; }
        public Currency Currency { get; set; }
        public UoM UOM { get; set; }
        public string Address { get; set; }
        [StringLength(32)]
        public string ZipCode { get; set; }
        [Required, StringLength(128)]
        public string City { get; set; }
        public int StateId { get; set; }
        public bool IsGeocodeUsed { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsRetailJob { get; set; }
        public bool IsProFormaPoEnabled { get; set; }
        public bool SignatureEnabled { get; set; }
        [StringLength(64)]
        public string CountyName { get; set; }
        [StringLength(256)]
        public string TimeZoneName { get; set; }
        public LocationManagedType LocationManagedType { get; set; }
        public bool IsMarineLocation { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
        [StringLength(256)]
        public string DispatchRegionId { get; set; }
    }
}
