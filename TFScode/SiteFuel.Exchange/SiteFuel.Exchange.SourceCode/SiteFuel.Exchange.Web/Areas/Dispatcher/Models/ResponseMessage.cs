using System.Net;

namespace SiteFuel.Exchange.Web.Areas.Dispatcher.Models
{
    public class ResponseMessage
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
    }
}