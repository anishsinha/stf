using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class NewsfeedMessage
    {
        public NewsfeedMessage(int eventId, string buyerMessage, string supplierMessage, string targetUrl)
        {
            EventId = (NewsfeedEvent)eventId;
            BuyerMessage = buyerMessage;
            SupplierMessage = supplierMessage;
            TargetUrl = targetUrl;
        }

        public NewsfeedEvent EventId { get; }

        public string BuyerMessage { get; }

        public string SupplierMessage { get; }

        public string TargetUrl { get; }

        public string GetBuyerMessage(IDictionary<string, string> parameters)
        {
            return GetFormatedMessage(BuyerMessage, parameters);
        }

        public string GetSupplierMessage(IDictionary<string, string> parameters)
        {
            return GetFormatedMessage(SupplierMessage, parameters);
        }

        private string GetFormatedMessage(string message, IDictionary<string, string> parameters)
        {
            if (!string.IsNullOrWhiteSpace(message) && parameters != null)
            {
                var messageParameters = GetMessageParameters();
                if (parameters.Count < messageParameters.Count)
                    throw new ArgumentException("NewsfeedMessage: Number of parameters passed does not match " + EventId);

                foreach (var mParameter in messageParameters)
                {
                    if (parameters.Any(t => t.Key.Equals(mParameter, StringComparison.OrdinalIgnoreCase)))
                    {
                        message = message.Replace(mParameter, parameters[mParameter]);
                    }
                    else
                    {
                        throw new ArgumentException("NewsfeedMessage: expected parameter is does not found", mParameter);
                    }
                }
            }
            return message;
        }

        private List<string> GetMessageParameters()
        {
            var response = new List<string>();
            if (!string.IsNullOrWhiteSpace(BuyerMessage))
            {
                var buyerParams = BuyerMessage.Replace(".", "").Replace("{", "").Replace("}", "").Replace("%", "").Replace(",", "").Replace("'", "").Replace("\"", "").Split(' ')
                    .Where(t => t.StartsWith("[") && t.EndsWith("]")).ToList();
                response.AddRange(buyerParams);
            }
            if (!string.IsNullOrWhiteSpace(SupplierMessage))
            {
                var SupplierParams = SupplierMessage.Replace(".", "").Replace("{", "").Replace("}", "").Replace("%", "").Replace(",", "").Replace("'", "").Replace("\"", "").Split(' ')
                    .Where(t => t.StartsWith("[") && t.EndsWith("]")).ToList();
                response.AddRange(SupplierParams);
            }
            response = response.Distinct().ToList();

            return response;
        }
    }
}
