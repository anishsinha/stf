using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class Money
    {
        public Money(Currency baseCurrency, decimal baseAmount, Currency displayCurrency, decimal displayAmount, decimal xRate)
        {
            InstanceInitialize(baseCurrency, baseAmount, displayCurrency, displayAmount, xRate);
        }

        public Money(Currency displayCurrency, decimal displayAmount, decimal xRate)
        {
            var baseCurrency = Currency.USD;
            InstanceInitialize(baseCurrency, 0, displayCurrency, displayAmount, xRate);
            var isSameCurrency = displayCurrency == Currency.None || displayCurrency == baseCurrency;
            BaseAmount = isSameCurrency ? displayAmount : Math.Round(displayAmount / xRate, 8);
        }

        private void InstanceInitialize(Currency baseCurrency, decimal baseAmount, Currency displayCurrency, decimal displayAmount, decimal xRate)
        {
            if (baseCurrency == Currency.None)
                throw new ArgumentException("Base currency is required");

            if (displayCurrency == Currency.None)
                throw new ArgumentException("Display currency is required");

            if (xRate <= 0)
                throw new ArgumentException("Exchange rate should be greater than zero");

            BaseCurrency = baseCurrency;
            BaseAmount = baseAmount;
            DisplayCurrency = displayCurrency;
            DisplayAmount = displayAmount;
            ExchangeRate = xRate;
        }

        public Currency BaseCurrency { get; private set; }
        public decimal BaseAmount { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public Currency DisplayCurrency { get; private set; }
        public decimal DisplayAmount { get; private set; }
    }
}
