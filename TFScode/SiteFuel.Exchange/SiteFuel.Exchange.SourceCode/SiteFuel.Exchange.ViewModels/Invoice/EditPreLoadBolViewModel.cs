using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class EditPreLoadBolViewModel
    {
        private decimal? _netQuantity = null;
        private decimal? _grossQuantity = null;
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string BolNumber { get; set; }
        public string LiftTicketNumber { get; set; }
        public string BadgeNumber { get; set; }
        public string Carrier { get; set; }
        public decimal? GrossQuantity
        {
            get { return (_grossQuantity == null || _grossQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _grossQuantity; }
            set { _grossQuantity = value; }
        }
        public decimal? LiftQuantity { get; set; }
        public decimal? NetQuantity
        {
            get { return (_netQuantity == null || _netQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _netQuantity; }
            set { _netQuantity = value; }
        }
    }
}
