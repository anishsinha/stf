using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FavoriteFuelGridViewModel
    {
        public int Id { get; set; }

        public int FuelTypeId { get; set; }

        public string FuleName { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public string RemovedBy { get; set; }

        public string RemovedDate { get; set; }
    }
}
