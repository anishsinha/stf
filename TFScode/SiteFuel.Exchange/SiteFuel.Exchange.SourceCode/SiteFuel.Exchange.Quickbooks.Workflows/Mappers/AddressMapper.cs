using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Mappers
{
    public static class AddressMapper
    {
        public static Address ToAddress(this AddressViewModel viewModel)
        {
            var address = new Address
            {
                Addr1 = viewModel.CustomerName.RemoveSpecialCharacter(),
                Addr2 = viewModel.Address,
                City = viewModel.City,
                State = viewModel.State,
                Country = viewModel.Country,
                PostalCode = viewModel.PostalCode
            };
            address = address.SplitAddressField();
            return address;
        }

        private static Address SplitAddressField(this Address address)
        {
            var tuple1 = GetAddressField(address.Addr1, address.Addr2);
            address.Addr1 = tuple1.Item1;
            address.Addr2 = tuple1.Item2;

            var tuple2 = GetAddressField(address.Addr2, address.Addr3);
            address.Addr2 = tuple2.Item1;
            address.Addr3 = tuple2.Item2;

            var tuple3 = GetAddressField(address.Addr3, address.Addr4);
            address.Addr3 = tuple3.Item1;
            address.Addr4 = tuple3.Item2;

            var tuple4 = GetAddressField(address.Addr4, address.Addr5);
            address.Addr4 = tuple4.Item1;
            address.Addr5 = tuple4.Item2;

            return address;
        }

        private static Tuple<string, string> GetAddressField(string addr1, string addr2)
        {
            if (addr1 != null && addr1.Length > QbConstants.MaxStringLength)
            {
                var tempLine = addr1;
                addr1 = tempLine.SubstringBeforeLast(" ", QbConstants.MaxStringLength);
                addr2 = tempLine.Replace(addr1, string.Empty).Trim() + (addr2 == null ? string.Empty : $" {addr2}");
            }
            return new Tuple<string, string>(addr1, addr2);
        }
    }
}
