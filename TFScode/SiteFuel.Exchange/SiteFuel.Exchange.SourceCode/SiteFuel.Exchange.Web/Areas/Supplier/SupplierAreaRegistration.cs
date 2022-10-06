using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier
{
    public class SupplierAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Supplier";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Supplier_default",
                "Supplier/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            // Create for additional url parameter pass in eg. "Supplier/Order/Details/{id}/{isBrokeredRequest}/{isInvoiceGenerated}",
            context.MapRoute(
               "Supplier_Status",
               "Supplier/{controller}/{action}/{id}/{isBrokeredRequest}/{isInvoiceGenerated}",
               new { action = "Index", id = UrlParameter.Optional, isBrokeredRequest = UrlParameter.Optional, isInvoiceGenerated = UrlParameter.Optional }
           );
        }
    }
}