using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AcknowledgementViewModel
    {
        public AcknowledgementViewModel()
        {
        }

        public int Id { get; set; }

        public int EntityId { get; set; }

        public bool IsSent { get; set; }

        public int UserId { get; set; }

        public int UserCompanyId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
