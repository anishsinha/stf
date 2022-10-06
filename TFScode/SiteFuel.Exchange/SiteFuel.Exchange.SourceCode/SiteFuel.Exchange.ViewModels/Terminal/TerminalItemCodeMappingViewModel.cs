using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalItemCodeMappingViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public int TerminalSupplierId { get; set; }

        public string TerminalSupplier { get; set; }

        public int ItemDescriptionId { get; set; }

        public string ItemDescription{ get; set; }

        public string ProductType { get; set; }

        public string ItemCode { get; set; }

        public DateTimeOffset EffectiveDate { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        public int CompanyId { get; set; }

        public bool IsActive { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }

    public class TerminalSupplierAndDescDropDown
    {
        public List<DropdownDisplayExtendedItem> TerminalSupplierList { get; set; }
        public List<DropdownDisplayExtendedItem> TerminalDescriptionList { get; set; }
    }

    public class ItemCodeMappingGridRequestViewModel : DataTableAjaxPostModel
    {
        public int CompanyId { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }
    }

    public class TerminalItemCodeMappingGridViewModel
    {
        public int Id { get; set; }

        public int TerminalSupplierId { get; set; }

        public string TerminalSupplier { get; set; }

        public int ItemDescriptionId { get; set; }

        public string ItemDescription { get; set; }

        public string ItemCode { get; set; }

        public string EffectiveDate { get; set; }

        public string ExpiryDate { get; set; }

        public int CompanyId { get; set; }

        public Country CountryId { get; set; }
    }

    public class TerminalItemCodeMappingBulkUploadModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FileUploadPath { get; set; }
    }
}
