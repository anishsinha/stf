using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FavoriteSideMenuViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string LinkId { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
