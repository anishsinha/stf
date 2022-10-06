using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TruckDetailViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string TruckId { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal OptimizedCapacity { get;set; }
        public string ContractNumber { get; set; }
        public List<Compartment> Compartments { get; set; }
        public int TfxCreatedBy { get; set; }
        public int TfxCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public TruckStatus Status { get; set; }
        public LicenceRequirementStatus LicenceRequirement { get; set; }
        public string LicencePlate { get; set; }
        public string ExpirationDate { get; set; }
        public string IsPump { get; set; }
        public TrailerTypeStatus TrailerType { get; set; }
        public bool IsDeleted { get; set; }
        public List<TrailerFuelRetainViewModel> TrailerFuelRetains { get; set; }
        public bool IsFilldCompatible { get; set; }
        public string FuelResetUserId { get; set; }
        public string FuelResetUserName { get; set; }
        public int DefaultUOM { get; set; }
        public string SmartDeviceId { get; set; }
        
    }
    public class Compartment
    {
        public string CompartmentId { get; set; }

        public int FuelType { get; set; }
        public decimal Capacity { get; set; }
        public decimal Quantity { get; set; }
        public string PumpId { get; set; }

    }
}
