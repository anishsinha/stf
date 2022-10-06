using SiteFuel.Exchange.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FileGenerator
{
    public class FileGeneratorViewModel
    {
        public int Id { get; set; }
        public FileType Type { get; set; }
        public int EntityId { get; set; }
        public string ParameterJson { get; set; }
        public int CompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
