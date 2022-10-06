using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core
{
    [DataContractAttribute]
    public class DataTableSearchModel
    {
        public DataTableSearchModel(DataTableAjaxPostModel requestModel)
        {
            DataTableSearchValues = new List<DataTableSearchValues>();
            if (requestModel.columns != null)
            {
                int idx = 0;
                requestModel.columns.Where(x => !string.IsNullOrWhiteSpace(x.name)).ToList().ForEach(x =>
                {
                    DataTableSearchValues.Add(
                       string.IsNullOrWhiteSpace(x.search.value) ? new DataTableSearchValues
                       {
                           SearchVals = new List<string>(),
                           Name = x.name
                       }
                       :
                       new DataTableSearchValues
                       {
                           SearchVals = (x.search.value.Replace(",", "").Replace("miles", "").Replace("\\", "")).Trim().Split('|').ToList(),
                           Name = x.name
                       });

                    if (x.search != null && x.search.value != null && x.search.value.Contains(",") && DataTableSearchValues.Count > 0)
                    {
                        DataTableSearchValues[idx].SearchVals.Add(x.search.value);
                    }
                    idx += 1;
                });
            }

            if (requestModel.length != 0)
            {
                PageSize = requestModel.length;
                PageNumber = requestModel.start / requestModel.length + 1;
            }
            else
            {
                PageNumber = 1;
                PageSize = 9999999;
            }

            if (requestModel.order != null && requestModel.order.Count > 0)
            {
                SortId = requestModel.order[0].column;
                SortDirection = requestModel.order[0].dir;
            }
            else
            {
                SortId = 0;
                SortDirection = "desc";
            }
            if (requestModel.search != null && requestModel.search.value != null)
                GlobalSearchText = (requestModel.search.value.Replace(",", "").Replace("miles", "").Replace("\\", "")).Trim();
        }

        [DataMember]
        public string GlobalSearchText { get; set; }

        [DataMember]
        public List<DataTableSearchValues> DataTableSearchValues { get; set; }

        [DataMember]
        public string SortDirection { get; set; }

        [DataMember]
        public int SortId { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int PageNumber { get; set; }
    }
}
