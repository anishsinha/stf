using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.Common
{
    public class ListItem : IEquatable<ListItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Equals(ListItem item)
        {
            if (Id == item.Id && Name == item.Name)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hashId = Id.GetHashCode();
            int hashName = Name == null ? 0 : Name.GetHashCode();

            return hashId ^ hashName;
        }
    }
}
