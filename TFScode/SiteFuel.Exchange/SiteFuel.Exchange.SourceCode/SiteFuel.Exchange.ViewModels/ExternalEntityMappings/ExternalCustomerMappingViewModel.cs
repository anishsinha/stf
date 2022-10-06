using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.ExternalEntityMappings
{
    public class ExternalCustomerMappingViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string TargetCustomerValue { get; set; }
        public int ThirdPartyId { get; set; }
    }

    public class ExternalCustomerLocationMappingViewModel
    {
        public int Id { get; set; }
        public int CustomerLocationId { get; set; }
        public string CustomerLocationName { get; set; }
        public string TargetCustomerLocationValue { get; set; }
        public int ThirdPartyId { get; set; }
        public string CompanyName { get; set; }
        public string TargetCustomerValue { get; set; }
    }

    public class ExternalProductMappingViewModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string TargetProductValue { get; set; }
        public int ThirdPartyId { get; set; }
        public int? OtherProductId { get; set; }
    }

    public class ExternalSupplierMappingViewModel
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string TargetSupplierValue { get; set; }
        public int ThirdPartyId { get; set; }
    }
    public class ExternalTerminalMappingViewModel
    {
        public int Id { get; set; }
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string ControlNumber { get; set; }
        public string TargetTerminalValue { get; set; }
        public int ThirdPartyId { get; set; }
    }
    public class ExternalBulkPlantMappingViewModel
    {
        public int Id { get; set; }
        public int BulkPlantId { get; set; }
        public string BulkPlantName { get; set; }
        public string TargetBulkPlantValue { get; set; }
        public int ThirdPartyId { get; set; }
    }

    public class ExternalCarrierMappingViewModel
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string TargetCarrierValue { get; set; }
        public int ThirdPartyId { get; set; }
    }
    public class ExternalDriverMappingViewModel
    {
        public int Id { get; set; }
        public int DriverId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TargetDriverValue { get; set; }
        public int ThirdPartyId { get; set; }
    }
    public class ExternalVehicleMappingViewModel
    {
        public string Id { get; set; }
        public string TruckId { get; set; }
        public string TruckName { get; set; }
        public string TargetVehicleValue { get; set; }
        public int ThirdPartyId { get; set; }
        public int UserId { get; set; }
    }
}
