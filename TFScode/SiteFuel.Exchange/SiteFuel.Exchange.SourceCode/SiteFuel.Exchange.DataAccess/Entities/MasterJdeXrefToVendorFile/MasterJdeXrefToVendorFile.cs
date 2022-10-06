namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   public class MasterJdeXrefToVendorFile
    {
        public int Id { get; set; }
        public int Supplier_BU_Number { get; set; }
        public string Xref_Type_supplier_Xref_short { get; set; }
        public string Xref_Type_supplier_Xref { get; set; }
        public string CustomerSupplier_Item_Number_Item_code_on_Lift_File { get; set; }
        public string Search_Text_Compressed { get; set; }
        public string Item_Number { get; set; }
        public string Description { get; set; }
        public string Date_Updated { get; set; }
        public string Expired_Date { get; set; }
        public string Eff_Date_Date { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }

        [ForeignKey("ProductCategoryId")]
        public MstProductType ProductType { get; set; }

      

    }
  
}
