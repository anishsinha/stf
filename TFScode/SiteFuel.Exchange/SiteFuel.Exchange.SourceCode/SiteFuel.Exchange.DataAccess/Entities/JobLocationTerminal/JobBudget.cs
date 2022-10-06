namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JobBudget
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobBudget()
        {
            MstBudgetAllocationTypes = new HashSet<MstBudgetAllocationType>();
            Jobs = new HashSet<Job>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            ExchangeRate = 1;
        }

        public int Id { get; set; }

        public int BudgetCalculationTypeId { get; set; }

        public Nullable<int> BudgetTypeId { get; set; }

        public decimal Budget { get; set; }

        public bool IsExceededBudget { get; set; }

        public decimal Gallons { get; set; }

        public decimal PricePerGallon { get; set; }

        public bool IsBudgetTracked { get; set; }

        public bool IsHedgeAmountTracked { get; set; }

        public decimal HedgeAmount { get; set; }

        public decimal SpotAmount { get; set; }

        public bool IsTaxExempted { get; set; }

        public bool IsAssetTracked { get; set; }

        public bool IsAssetDropStatusEnabled { get; set; }

        public bool IsDropPictureRequired { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public decimal BaseBudget { get; set; }
        public decimal BasePrice{ get; set; }
        public decimal BaseHedgeAmount{ get; set; }
        public decimal BaseSpotAmount{ get; set; }
        public Currency Currency { get; set; }
        public decimal ExchangeRate{ get; set; }
        public UoM UoM { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual MstBudgetCalculationType MstBudgetCalculationType { get; set; }

        public virtual MstBudgetType MstBudgetType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstBudgetAllocationType> MstBudgetAllocationTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        public bool IsTankAvailable { get; set; }
    }
}
