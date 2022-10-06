using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class Quantity
    {
        public Quantity(UoM baseUoM, decimal baseQuantity, UoM displayUoM, decimal displayQuantity)
        {
            InstanceInitialize(baseUoM, baseQuantity, displayUoM, displayQuantity);
        }

        public Quantity(UoM displayUoM, decimal displayQuantity)
        {
            var baseUoM = UoM.Gallons;
            InstanceInitialize(baseUoM, 0, displayUoM, displayQuantity);
            var isSameUoM = displayUoM == UoM.None || displayUoM == baseUoM;
            BaseQuantity = isSameUoM ? displayQuantity : Math.Round(displayQuantity / _gallonsToLitres, 8);
        }

        private void InstanceInitialize(UoM baseUoM, decimal baseQuantity, UoM displayUoM, decimal displayQuantity)
        {
            if (baseUoM == UoM.None)
                throw new ArgumentException("Base unit of measurement is required");

            if (displayUoM == UoM.None)
                throw new ArgumentException("Display unit of measurement is required");

            BaseUoM = baseUoM;
            BaseQuantity = baseQuantity;
            DisplayUoM = displayUoM;
            DisplayQuantity = displayQuantity;
        }

        private static readonly decimal _gallonsToLitres = 3.785411784M;
        public UoM BaseUoM { get; private set; }
        public decimal BaseQuantity { get; private set; }
        public UoM DisplayUoM { get; private set; }
        public decimal DisplayQuantity { get; private set; }

        public static decimal Convert(UoM fromUoM, UoM toUoM, decimal quantity)
        {
            if (fromUoM == UoM.Gallons && toUoM == UoM.Litres)
            {
                quantity = Math.Round(quantity * _gallonsToLitres, 8);
            }
            else if (fromUoM == UoM.Litres && toUoM == UoM.Gallons)
            {
                quantity = Math.Round(quantity / _gallonsToLitres, 8);
            }
            return quantity;
        }
    }
}
