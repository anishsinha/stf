using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class ProductDetailsResponseModel : BaseResponseModel
    {
        public ProductDetailsResponseModel()
        {
        }
        public ProductDetailsResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        
        public List<DropdownDisplayItem> ProductDetails { get; set; }
    }
}
