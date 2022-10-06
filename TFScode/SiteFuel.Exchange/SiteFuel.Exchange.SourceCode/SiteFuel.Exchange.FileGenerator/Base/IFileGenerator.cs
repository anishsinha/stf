using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.ViewModels.FileGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.Base
{
    public interface IFileGenerator
    {
        FileType FileType { get; set; }

        string Generate(FileGeneratorViewModel viewModel, IFileViewModel file);
    }
}
