using SiteFuel.Exchange.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.Base
{
    public interface IFileViewModel
    {
        FileType FileType { get; set; }
    }
}
