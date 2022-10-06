using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class TruckDetailViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string TruckId { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal OptimizedCapacity { get; set; }
        public string ContractNumber { get; set; }
        public TrailerTypeStatus TrailerType { get; set; } = TrailerTypeStatus.Lead;
        public List<Compartment> Compartments { get; set; }
        public int TfxCreatedBy { get; set; }
        public int TfxCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public TruckStatus Status { get; set; } = TruckStatus.Active;
        public LicenceRequirementStatus LicenceRequirement { get; set; }
        public string LicencePlate { get; set; }
        public string ExpirationDate { get; set; }
        public string IsPump { get; set; }
        public List<TrailerFuelRetainViewModel> TrailerFuelRetains { get; set; }
        public bool IsFilldCompatible { get; set; }
        public string FuelResetUserId { get; set; }
        public string FuelResetUserName { get; set; }
        public string SmartDeviceId { get; set; }
    }
    public class Compartment
    {
        public string CompartmentId { get; set; }
        public decimal Capacity { get; set; }
        public int FuelType { get; set; }
        public string PumpId { get; set; }
    }

    public class TrailerRetainDetails
    {
        public int DriverId { get; set; }
        public string TrailerId { get; set; }
        public string Name { get; set; }
        public int RetainFuelCount { get; set; }
        public int ProductId { get; set; }
    }

    public class RetainRequets
    {
       public List<int> driverIds { get; set; }      
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}
