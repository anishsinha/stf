using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetQuickUpdateHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OfferType { get; set; }
        public string Tiers { get; set; }
        public string Customers { get; set; }
        public string FuelType { get; set; }
        public string States { get; set; }
        public string Cities { get; set; }
        public string ZipCodes { get; set; }
        public string UpdateType { get; set; }
        public string UpdateName { get; set; }
        public int MathOperationId { get; set; }
        public decimal UpdatedAmount { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UndoBy { get; set; }
        public string UndoDate { get; set; }
        public bool? IsValid { get; set; }
        public int TotalCount { get; set; }
    }
}
