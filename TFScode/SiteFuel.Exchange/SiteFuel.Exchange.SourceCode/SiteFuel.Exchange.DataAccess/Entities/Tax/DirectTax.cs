using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class DirectTax
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public bool IsDirectTax { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public bool IsActive { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("CountryId")]
        public virtual MstCountry MstCountry { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }
    }
}
