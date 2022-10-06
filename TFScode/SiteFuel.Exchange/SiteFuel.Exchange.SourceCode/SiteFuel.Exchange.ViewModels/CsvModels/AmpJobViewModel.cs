using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AmpJobViewModel
    {
        public AmpJobViewModel()
        {
        }
        public AmpJobViewModel(List<string> inputStrings)
        {
            Drops = new List<AmpAssetDropViewModel>();
            string jobString = string.Empty, dropString = string.Empty;
            for (int index = 0; index < inputStrings.Count; index++)
            {
                if (inputStrings[index].StartsWith("f", StringComparison.OrdinalIgnoreCase))
                {
                    jobString = inputStrings[index].Trim();
                    var jobDetails = jobString.Split(' ');
                    EndDate = jobDetails[1].ToDateTimeOffset();
                    EndTime = EndDate.TimeOfDay;
                    break;
                }
                if (inputStrings[index].StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    jobString = inputStrings[index].Trim();
                    dropString = inputStrings[++index].Trim();
                    var jobDetails = jobString.Split(' ');
                    YardNumber = jobDetails[0];
                    JobName = YardNumber.Remove(0, 1);
                }
                if (!string.IsNullOrWhiteSpace(jobString) && !string.IsNullOrWhiteSpace(dropString))
                {
                    var drop = new AmpAssetDropViewModel(jobString, dropString);
                    Drops.Add(drop);
                    jobString = string.Empty;
                    dropString = string.Empty;
                }
            }
            if (Drops.Any())
            {
                StartDate = Drops.OrderBy(t => t.StartDate).First().StartDate;
                StartTime = StartDate.TimeOfDay;
            }
        }

		public bool JobOrOrderNotExists { get; set; }
		public string JobName { get; set; }

        public string YardNumber { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public TimeSpan EndTime { get; set; }

		public InvoiceViewModel InvoiceViewModel { get; set; }

		public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }

        public string AmpProductType { get; set; }

        public int ProductTypeId { get; set; }
        public List<AmpAssetDropViewModel> Drops { get; set; }

        public int? BrokerCompanyId { get; set; }

        public AmpJobViewModel Clone()
        {
            return new AmpJobViewModel
            {
                AmpProductType = AmpProductType,
                BuyerCompanyId = BuyerCompanyId,
                EndDate = EndDate,
                EndTime = EndTime,
                JobName = JobName,
                StartDate = StartDate,
                StartTime = StartTime,
                SupplierCompanyId = SupplierCompanyId,
                YardNumber = YardNumber
            };
        }
    }
}
