using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductMappingViewModel : StatusViewModel
    {
        public ProductMappingViewModel()
        {
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string MyProductId { get; set; }
        public string BackOfficeProductId { get; set; }
        public string DriverProductId { get; set; }
        public List<DropdownDisplayItem> States { get; set; }
        public List<DropdownDisplayExtendedItem> Cities { get; set; }
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
       // public string TerminalItemCode { get; set; }
    }

    public class ProductMappingGridViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string MyProductId { get; set; }
        public string BackOfficeProductId { get; set; }
        public string DriverProductId { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string CountryCode { get; set; }
        public string TerminalAddress { get; set; }
      //  public string TerminalItemCode { get; set; }
    }

    public class ProductMappingBulkUploadModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FileUploadPath { get; set; }
    }
}
