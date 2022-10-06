using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProgressReportCountViewModel
    {
        public int ActiveAccounts { get; set; }

        public int NewAccounts { get; set; }

        public int SFXOwnedAccounts { get; set; }

        public int ActiveUsers { get; set; }

        public int NewUsers { get; set; }

        public int NewUsersBuyer { get; set; }

        public int NewUsersSupplier { get; set; }

        public int NewUsersBuyerAndSupplier { get; set; }

        public int FRCreated { get; set; }

        public int RegularFRCreated { get; set; }

        public int TPOFRCreated { get; set; }

        public int FRCreatedForRealAccount { get; set; }

        public int RegularFRCreatedForRealAccount { get; set; }

        public int TPOFRCreatedForRealAccount { get; set; }

        public int FRCreatedForSFXOwnedAccount { get; set; }

        public int RegularFRCreatedForSFXOwnedAccount { get; set; }

        public int TPOFRCreatedForSFXOwnedAccount { get; set; }

        public int FRAccepted { get; set; }

        public int RegularFRAccepted { get; set; }

        public int TPOFRAccepted { get; set; }

        public int FRAcceptedForRealAccount { get; set; }

        public int RegularFRAcceptedForRealAccount { get; set; }

        public int TPOFRAcceptedForRealAccount { get; set; }

        public int FRAcceptedForSFXOwnedAccount { get; set; }

        public int RegularFRAcceptedForSFXOwnedAccount { get; set; }

        public int TPOFRAcceptedForSFXOwnedAccount { get; set; }

        public decimal GallonsRequested { get; set; }

        public decimal RegularGallonsRequested { get; set; }

        public decimal TPOGallonsRequested { get; set; }

        public decimal GallonsRequestedForRealAccount { get; set; }

        public decimal RegularGallonsRequestedForRealAccount { get; set; }

        public decimal TPOGallonsRequestedForRealAccount { get; set; }

        public decimal GallonsRequestedForSFXOwnedAccount { get; set; }

        public decimal RegularGallonsRequestedForSFXOwnedAccount { get; set; }

        public decimal TPOGallonsRequestedForSFXOwnedAccount { get; set; }

        public int TotalDrops { get; set; }

        public int TotalDropsForRealAccount { get; set; }

        public int TotalDropsForSFXOwnedAccount { get; set; }

        public decimal GallonsDropped { get; set; }

        public decimal GallonsDroppedForRealAccount { get; set; }

        public decimal GallonsDroppedForSFXOwnedAccount { get; set; }

        public int DDTCreated { get; set; }

        public int DDTCreatedForRealAccount { get; set; }

        public int DDTCreatedForSFXOwnedAccount { get; set; }

        public int InvoiceCreated { get; set; }

        public int InvoiceCreatedForRealAccount { get; set; }

        public int InvoiceCreatedForSFXOwnedAccount { get; set; }

        public int CreditInvoiceCreated { get; set; }

        public int RebillInvoiceCreated { get; set; }

        public decimal TotalInvoiceAmount { get; set; }

        public decimal TotalInvoiceAmountForRealAccount { get; set; }

        public decimal TotalInvoiceAmountForSFXOwnedAccount { get; set; }

        public decimal TotalMonthGallonsRequested { get; set; }

        public decimal TotalMonthGallonsDropped { get; set; }

        public decimal TotalMonthInvoiceAmount { get; set; }

        public string AccountOwnerName { get; set; }
    }
}
