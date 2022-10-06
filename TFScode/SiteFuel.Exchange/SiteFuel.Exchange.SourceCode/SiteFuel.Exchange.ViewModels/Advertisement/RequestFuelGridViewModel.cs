using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class RequestFuelGridViewModel
    {
        public int Id { get; set; }

        public int RequestPriceId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string RequestDateTime { get; set; }

        public bool IsEmailSentToSales { get; set; }

        public string EmailSentDateTime { get; set; }

        public bool IsCustomerContacted { get; set; }

        public string CustomerContactedDateTime { get; set; }

        public bool IsBusinessDone { get; set; }
    }
}
