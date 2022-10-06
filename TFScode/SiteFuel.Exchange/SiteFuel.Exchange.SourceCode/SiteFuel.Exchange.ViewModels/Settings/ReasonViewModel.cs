using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ReasonViewModel
    {
        public ReasonCategoryViewModel ReasonCategory { get; set; }
        public ReasonCodeModel ReasonCode { get; set; }
    }

    public class ReasonCategoryViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CompanyId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class ReasonCodeModel : StatusViewModel
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public string ReasonCode { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
