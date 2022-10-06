using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("HaulerPricingMatrices")]
    public class HaulerPricingMatrix
    {
        public HaulerPricingMatrix()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public decimal MinGallons { get; set; }

        public decimal? MaxGallons { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
