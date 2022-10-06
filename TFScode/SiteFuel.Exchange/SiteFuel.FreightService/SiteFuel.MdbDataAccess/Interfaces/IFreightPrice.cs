using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Interfaces
{
    public interface IFreightPrice
    {
        ObjectId Id { get; set; }
        int Type { get; set; }
        int CompanyId { get; set; }
        decimal MinValue { get; set; }
        decimal MaxValue { get; set; }
        decimal Price { get; set; }
        int Currency { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
