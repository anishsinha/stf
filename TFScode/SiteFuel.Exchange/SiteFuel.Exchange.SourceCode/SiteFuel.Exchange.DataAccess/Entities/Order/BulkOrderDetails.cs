using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SiteFuel.Exchange.DataAccess.Entities
{

    public class BulkOrderDetails
    {
        public int Id { get; set; }
        public int FileID { get; set; }
        public int Status { get; set; }
        public bool IsOrderProcessed { get; set; }
        public string FileData { get; set; }
        public int? CsvLineNumber { get; set; }
        [ForeignKey("FileID")]
        public virtual QueueMessage QueueMessage { get; set; }
    }

}
