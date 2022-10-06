using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Interfaces
{
    public interface IFreightTable
    {
        ObjectId Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Type { get; set; }
        int FuelType { get; set; }
        int CompanyId { get; set; }
        DateTimeOffset StartDate { get; set; }
        DateTimeOffset EndDate { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
