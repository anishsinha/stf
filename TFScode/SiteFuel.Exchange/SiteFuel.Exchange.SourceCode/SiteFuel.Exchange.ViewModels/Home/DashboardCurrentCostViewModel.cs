using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardCurrentCostViewModel : StatusViewModel
    {
        public DashboardCurrentCostViewModel()
        {
            InstanceInitialize();
        }

        public DashboardCurrentCostViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            StatusCode = status;
            FuelTypes = new List<DropdownDisplayItem>();
            CurrentCostGridViewModels = new List<CurrentCostGridViewModel>();
        }

        public List<CurrentCostGridViewModel> CurrentCostGridViewModels { get; set; }
        
        public List<DropdownDisplayItem> FuelTypes { get; set; }

        public int FuelTypeId { get; set; }

        [Required]
        public decimal CurrentCost { get; set; }
    }

    public class CurrentCostGridViewModel
    {
        public int Id { get; set; }
        public decimal CurrentCostOfFuel { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelTypeName { get; set; }
        public string stateCodes { get; set; }
        public List<int?> stateId { get; set; }
        public UoM UoM { get; set; }
    }

    public class UpdateCurrentCostViewModel
    {
        public int FuelRequestId { get; set; }
        public int PriceRequestDetailId { get; set; }
        public int FuelTypeId { get; set; }
        public int TfxFuelTypeId { get; set; }
        public bool IsGlobalCost { get; set; }
        public decimal FuelCost { get; set; }
        public int OrderId { get; set; }
        public int JobStateId { get; set; }
        public int CountryId { get; set; }
        public int SupplierFuelCostTypeId { get; set; }
        public decimal OriginalFuelCost { get; set; }
        public Currency CurrencyType { get; set; }

        public UoM UoM { get; set; }
    }

    public class CalculateFuelCostViewModel
    {
        public decimal FuelCost { get; set; }
        public decimal CalculatedFuelCost { get; set; }
    }
}
