using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class CurrencyRate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string FromCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        [Required]
        [StringLength(3)]
        public string ToCurrency { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
