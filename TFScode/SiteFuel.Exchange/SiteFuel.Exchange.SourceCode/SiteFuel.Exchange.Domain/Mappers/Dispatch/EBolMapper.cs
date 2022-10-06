using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class EBolMapper
    {
        public static EBolViewModel ToViewModel(this EBolDetails entity, EBolViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new EBolViewModel();
            }

            viewModel.TerminalName = entity.TerminalName; 
            viewModel.BOLNumber = entity.BOLNumber;
            viewModel.GrossGallons = entity.GrossGallons;
            viewModel.NetGallons = entity.NetGallons;
        
            return viewModel;
        }
    }
}
