using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFuel.Exchange.Core
{

    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<ColumnOrder> order { get; set; } = new List<ColumnOrder>();


		public t ParseSearchablesByName<t>()
		{
			var obj = Activator.CreateInstance<t>();
			var type = obj.GetType();
			foreach (var item in columns)
			{
				foreach (var prop in type.GetProperties())
				{
					if(item.search != null && !string.IsNullOrEmpty(item.search.value) && prop.Name.Equals(item.name, StringComparison.OrdinalIgnoreCase))
					{
						if (!prop.PropertyType.IsEnum)
						{
                            var SearchVals = (item.search.value.Replace(",", "").Replace("miles", "").Replace("\\", "")).Trim();
                            prop.SetValue(obj, SearchVals);
						}
					}
				}
			}

			return obj;
		}

    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class ColumnOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
    /// End- JSon class sent from Datatables
    /// 
    public class DatatableResponse
    {
        public int draw { get; set; }
        public object data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
}