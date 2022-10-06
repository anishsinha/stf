using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyBlacklistViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public int AddedBy { get; set; }

        public int AddedByCompanyId { get; set; }

        public string AddedByName { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public string Reason { get; set; }

        public int? RemovedBy { get; set; }

        public string RemovedByName { get; set; }

        public DateTimeOffset? RemovedDate { get; set; }
    }
}
