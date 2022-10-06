using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CurrencyRateViewModel
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
