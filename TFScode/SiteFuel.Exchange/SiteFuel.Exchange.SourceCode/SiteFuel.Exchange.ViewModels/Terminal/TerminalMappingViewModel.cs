using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalMappingViewModel : StatusViewModel
    {
        public TerminalMappingViewModel()
        {
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string AssignedTerminalId { get; set; }
        public List<DropdownDisplayItem> States { get; set; }
        public List<DropdownDisplayItem> Cities { get; set; }
        public List<DropdownDisplayItem> Terminals { get; set; }
        public string StateIds { get; set; }
        public string CityIds { get; set; }
        public string TerminalIds { get; set; }
        public string FuelTypeIds { get; set; }
        public List<DropdownDisplayItem> FuelTypes { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsBulkUploadRequest { get; set; }
        public int RowNumber { get; set; }
        public int TerminalId { get; set; }
        public List<DropdownDisplayItem> TerminalSuppliers { get; set; }
    }

    public class TerminalMappingGridViewModel
    {
        public int Id { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string ControlNumber { get; set; }
        public string AssignedTerminalId { get; set; }
        public int CreatedByCompanyId { get; set; }
       
        public bool IsBulkPlant { get; set; }

        public int?TerminalSupplierId { get; set; }

        public string TerminalSupplierName { get; set; }

    }
}
