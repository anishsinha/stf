using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Interfaces
{
    interface ITankModalType
    {
        ObjectId Id { get; set; }
        string Name { get; set; }
        string Modal { get; set; }
        int ScaleMeasurement { get; set; }
        int CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        int BuyerCompanyId { get; set; }
        int SupplierCompanyId { get; set; }
        int CreatedByCompanyId { get; set; }
        bool IsActive { get; set; }
        string PdfFilePath { get; set; }
        List<DipChartDetails> DipChartDetails { get; set; }
    }
    interface IDipChartDetails
    {
        decimal Dip { get; set; }
        decimal Volume { get; set; }
        decimal Ullage { get; set; }
    }
}
