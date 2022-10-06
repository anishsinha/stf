using System;

namespace TrueFill.SCIM2.Model
{
    public class Result
    {
        private int totalResults;
        private int itemsPerPage;

        public string[] Schemas { get; set; }
        public int TotalResults
        {
            get
            {
                if (Resources != null && totalResults < Resources.Length) totalResults = Resources.Length;
                return totalResults;
            }
            set => totalResults = value;
        }
        public int ItemsPerPage
        {
            get
            {
                if (Resources != null && itemsPerPage < Resources.Length) itemsPerPage = Resources.Length;
                return itemsPerPage;
            }
            set => itemsPerPage = value;
        }
        public int StartIndex { get; set; }
        public Object[] Resources { get; set; }


        public Result(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
            StartIndex = 1;
        }
    }

   

}
