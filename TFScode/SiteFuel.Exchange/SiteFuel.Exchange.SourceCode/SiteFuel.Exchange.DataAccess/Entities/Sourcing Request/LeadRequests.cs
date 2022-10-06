using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadRequest
    {
        public LeadRequest()
        {
            LeadCustomerInformations = new HashSet<LeadCustomerInformation>();
            LeadAddressDetail = new HashSet<LeadAddressDetail>();
            LeadFuelDeliveryDetails = new HashSet<LeadFuelDeliveryDetail>();
            LeadFuelDetails = new HashSet<LeadFuelDetail>();
            LeadAdditionalDetail = new HashSet<LeadAdditionalDetail>();
            LeadRequestXOrders = new HashSet<LeadRequestXOrder>();
            FuelFees = new HashSet<FuelFee>();
            LeadNotes = new HashSet<LeadNote>();
            LeadRequestPriceDetails = new HashSet<LeadRequestPriceDetails>();
        }

        [Key]
        public int Id { get; set; }
        public string DisplayRequestID { get; set; }
        public TruckLoadTypes TruckLoadType { get; set; }
        public FreightOnBoardTypes FreightOnBoardType { get; set; }

        [StringLength(256)]
        public string AccountingCompanyId { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        public SourcingRequestStatus Status { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public int SalesUserId { get; set; }
        public bool ViewedModified { get; set; }// flag to know if sales user viewed the modified changes by supplier
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public virtual ICollection<LeadCustomerInformation> LeadCustomerInformations { get; set; }
        public virtual ICollection<LeadAddressDetail> LeadAddressDetail { get; set; }
        public virtual ICollection<LeadFuelDeliveryDetail> LeadFuelDeliveryDetails { get; set; }
        public virtual ICollection<LeadFuelDetail> LeadFuelDetails { get; set; }
        public virtual ICollection<LeadAdditionalDetail> LeadAdditionalDetail { get; set; }
        public virtual ICollection<LeadRequestXOrder> LeadRequestXOrders { get; set; }
        public virtual ICollection<FuelFee> FuelFees { get; set; }
        public virtual ICollection<LeadNote> LeadNotes { get; set; }
        public virtual ICollection<LeadRequestPriceDetails> LeadRequestPriceDetails{ get; set; }
    }
}
