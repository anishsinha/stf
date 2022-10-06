using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.DataAccess.Entities
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
