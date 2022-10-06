using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    public static class EditPreLoadBolViewModelMapper
    {
        public static EditPreLoadBolViewModel ToEditPreLoadBolViewModel(this EditPreLoadBolFireBaseViewModel model)
        {
            var entity = new EditPreLoadBolViewModel();

            entity.Id = model.Id;
            entity.UserId = model.UserId;
            entity.CompanyId = model.CompanyId;
            entity.BolNumber = model.BolNumber;
            entity.LiftTicketNumber = model.LiftTicketNumber;
            entity.BadgeNumber = model.BadgeNumber;
            entity.Carrier = model.Carrier;
            entity.GrossQuantity = (decimal)model.GrossQuantity;
            entity.LiftQuantity = (decimal)model.LiftQuantity;
            entity.NetQuantity = (decimal)model.NetQuantity;

            return entity;
        }
    }
}
