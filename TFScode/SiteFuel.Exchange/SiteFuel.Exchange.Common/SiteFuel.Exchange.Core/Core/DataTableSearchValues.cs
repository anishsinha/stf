using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core
{
    public class DataTableSearchValues
    {
        public string Name { get; set; }

        public List<string> SearchVals { get; set; }



       public DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("SearchVar", typeof(string));
            SearchVals.ForEach(x => dataTable.Rows.Add(x));
            return dataTable;

        }

    }
}
