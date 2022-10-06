using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ThirdPartyOrderCsvViewModelNew
    {
        [Name("Company Name*")]
        public string CompanyName { get; set; }

        [Name("Contact Person*")]
        public string ContactPerson { get; set; }

        [Name("Email*")]
        public string Email { get; set; }
        [Name("Mobile Number*")]
        public string MobileNumber { get; set; }

        [Name("Order(FTL/LTL)*")]
        public string TruckLoadType { get; set; }

        [Name("Freight On Board(Terminal/Destination)*")]
        public string FreightOnBoardType { get; set; }
        [Name("WBS#")]
        public string WBSNumber { get; set; }
        [Name("PO#")]
        public string PONumber { get; set; }

        [Name("Order Name")]
        public string OrderName { get; set; }

        [Name("Location Name*")]
        public string LocationName { get; set; }

        [Name("Third Party LocationID")]
        public string DisplayJobID { get; set; }

        [Name("Location - Address*")]
        public string Address { get; set; }
        [Name("Location - ZIP*")]
        public string ZipCode { get; set; }

        [Name("Location - City*")]
        public string LocationCity { get; set; }

        [Name("Location - County*")]
        public string CountyName { get; set; }

        [Name("Location - State*")]
        public string LocationState { get; set; }
        [Name("Location - Country*")]
        public string Country { get; set; }
        [Name("Currency(USD/CAD)*")]
        public string Currency { get; set; }
        [Name("UOM(Gallons/Litres)*")]
        public string UOM{ get; set; }

        [Name("GeoCode(Yes/No)*")]
        public string GeoCoded { get; set; }
        [Name("Latitude*")]
        public string Latitude { get; set; }
        [Name("Longitude*")]
        public string Longitude { get; set; }

        [Name("TimeZone")]
        public string TimeZoneName { get; set; }

        [Name("Retail Location(Yes/No)")]
        public string RetailLocation { get; set; }

        [Name("Automate Delivery Request(Yes/No)")]
        public string AutomateDeliveryRequest { get; set; }

        [Name("Onsite Contact Name")]
        public string OnsiteContactName { get; set; }

        [Name("Onsite Contact Email")]
        public string OnsiteContactEmail { get; set; }

        [Name("Onsite Contact Phone Number")]
        public string OnsiteContactPhoneNumber { get; set; }

        [Name("Single/Multiple Delivery(Single/Multiple)*")]
        public string DeliveryType { get; set; }
        [Name("Delivery Start Date*")]
        public string DeliveryStartDate { get; set; }

        [Name("Delivery End Date")]
        public string DeliveryEndDate { get; set; }

        [Name("Delivery Start Time*")]
        public string DeliveryStartTime { get; set; }
        [Name("Delivery End Time*")]
        public string DeliveryEndTime { get; set; }
        [Name("Fuel Type*")]
        public string FuelType { get; set; }
        [Name("Other Product Type")]
        public string NonStandardFuelType { get; set; }
        [Name("Other Product Description")]
        public string NonStandardFuelDescription { get; set; }

        [Name("Quantity Type(Fixed/Range/Not Specified)*")]
        public string QuantityType { get; set; }

        [Name("Quantity")]
        public string Quantity { get; set; }

        [Name("Min Quantity")]
        public string MinimumQuantity { get; set; }

        [Name("Max Quantity")]
        public string MaximumQuantity { get; set; }

        [Name("Billable Quantity(Net/Gross)*")]
        public string QuantityIndicatorType { get; set; }

        [Name("Pricing(Market Based/FuelCost/Fixed)*")]
        public string PricingType { get; set; }

        [Name("Price*")]
        public string Price { get; set; }
        [Name("Pricing Type(+$, -$, +%, -%)")]
        public string RackPricingType { get; set; }
        [Name("Pricing Code")]
        public string PricingCode { get; set; }

        [Name("City Rack Terminal")]
        public string CityRackTerminal { get; set; }
        [Name("Approved Terminal")]
        public string ApprovedTerminal { get; set; }
        [Name("Delivery Fee")]
        public string DeliveryFee { get; set; }

        [Name("Wet Hose Fee")]
        public string WetHoseFee { get; set; }
        [Name("Over Water Fee")]
        public string OverWaterFee { get; set; }

        [Name("Dry Run Fee")]
        public string DryRunFee { get; set; }
       
        [Name("Freight Fee")]
        public string FreightFee { get; set; }
        [Name("Minimum Gallon Fee")]
        public string MinimumGallonFee { get; set; }

        [Name("Minimum Gallons")]
        public string MinimumGallons { get; set; }

        [Name("Environmental Fee")]
        public string EnvironmentalFee { get; set; }

        [Name("Service Fee")]
        public string ServiceFee { get; set; }

        [Name("Load Fee")]
        public string LoadFee { get; set; }
        [Name("Surcharge Fee")]
        public string SurchargeFee { get; set; }

        [Name("Stop Off Fee")]
        public string StopOffFee  { get; set; }
        [Name("Demurrage Fee Terminal")]
        public string DemurrageFeeTerminal { get; set; }

        [Name("Demurrage Fee destination")]
        public string DemurrageFeeDestination { get; set; }

        [Name("Demurrage Fee Other")]
        public string DemurrageFeeOther { get; set; }

        [Name("Embedded Time")]
        public string EmbeddedTime { get; set; }

        [Name("Pump Charge Fee")]
        public string PumpChargeFee { get; set; }
        [Name("Split Tank Fee")]
        public string SplitTankFee { get; set; }

        [Name("Retain Fee")]
        public string RetainFee { get; set; }
        [Name("Additive Fee")]
        public string AdditiveFee { get; set; }
        [Name("Net Payment Details (Net/DOR/Prepaid)*")]
        public string PaymentTerm { get; set; }

        [Name("Net Days")]
        public string NetDays { get; set; }

        [Name("Payment Method")]
        public string PaymentMethod { get; set; }

        [Name("Location Inventory Management")]
        public string LocationInventoryManagementType { get; set; }

        [Name("Carrier Company Name")]
        public string CarrierCompanyName { get; set; }
        [Name("Carrier Email Address")]
        public string CarrierEmailAddress { get; set; }

        [Name("Invoice Creation Preference")]
        public string InvoiceCreationPreference { get; set; }
        [Name("Inventory Capture Method")]
        public string InventoryDataCaptureMethod { get; set; }

        [Name("Enable After the Fact PO(Yes/No)")]
        public string EnableAfterTheFactPO { get; set; }
        [Name("Send Invitation link(Yes/No)")]
        public string SendInvitationLink { get; set; }

        [Name("Provide Delivery Details to Customer(Yes/No)")]
        public string ProvideDeliveryDetailsToCustomer { get; set; }
        [Name("Enable Asset Level Tracking (Yes/No)")]
        public string EnableAssetLevelTracking { get; set; }
        [Name("Supplier Allowance")]
        public string SupplierAllowance { get; set; }
        [Name("Notes")]
        public string Notes { get; set; }
        
        [Name("Auto Freight Pricing Method(Yes/No)")]
        public string AutoFreightPricingMethod { get; set; }
        [Name("Source Region Name(s)*")]
        public string SourceRegionNames { get; set; }
        [Name("Terminal Name(s)*")]
        public string TerminalNames { get; set; }
        [Name("Bulk Plant Name(s)")]
        public string BulkPlantNames { get; set; }
        [Name("Approved Source Terminal")]
        public string ApprovedSourceTerminal { get; set; }
        [Name("Approved Bulk Plant")]
        public string ApprovedBulkPlant { get; set; }
       

    }

    public class ThirdPartyOrderCsvViewModelNewMap : ClassMap<ThirdPartyOrderCsvViewModelNew>
    {
        public ThirdPartyOrderCsvViewModelNewMap()
        {
            Map(m => m.CompanyName).Name("Company Name*");
            Map(m => m.ContactPerson).Name("Contact Person*");
            Map(m => m.Email).Name("Email*");
            Map(m => m.MobileNumber).Name("Mobile Number*");
            Map(m => m.TruckLoadType).Name("Order(FTL/LTL)*");
            Map(m => m.FreightOnBoardType).Name("Freight On Board(Terminal/Destination)*");
            Map(m => m.WBSNumber).Name("WBS#");
            Map(m => m.PONumber).Name("PO#");
            Map(m => m.OrderName).Name("Order Name");
            Map(m => m.LocationName).Name("Location Name*");
            Map(m => m.DisplayJobID).Name("Third Party LocationID");
            Map(m => m.Address).Name("Location - Address*");
            Map(m => m.ZipCode).Name("Location - ZIP*");
            Map(m => m.LocationCity).Name("Location - City*");
            Map(m => m.LocationState).Name("Location - State*");
            Map(m => m.Country).Name("Location - Country*");
            Map(m => m.Currency).Name("Currency(USD/CAD)*");
            Map(m => m.UOM).Name("UOM(Gallons/Litres)*");
            Map(m => m.GeoCoded).Name("GeoCode(Yes/No)*");
            Map(m => m.Latitude).Name("Latitude*");
            Map(m => m.Longitude).Name("Longitude*");
            Map(m => m.TimeZoneName).Name("TimeZone");
            Map(m => m.RetailLocation).Name("Retail Location(Yes/No)");
            Map(m => m.OnsiteContactName).Name("Onsite Contact Name");
            Map(m => m.OnsiteContactEmail).Name("Onsite Contact Email");
            Map(m => m.OnsiteContactPhoneNumber).Name("Onsite Contact Phone Number");
            Map(m => m.DeliveryType).Name("Single/Multiple Delivery(Single/Multiple)*");
            Map(m => m.DeliveryStartDate).Name("Delivery Start Date*");
            Map(m => m.DeliveryEndDate).Name("Delivery End Date");
            Map(m => m.DeliveryStartTime).Name("Delivery Start Time*");
            Map(m => m.DeliveryEndTime).Name("Delivery End Time*");
            Map(m => m.FuelType).Name("Fuel Type*");
            Map(m => m.NonStandardFuelType).Name("Other Product Type");
            Map(m => m.NonStandardFuelDescription).Name("Other Product Description");
            Map(m => m.QuantityType).Name("Quantity Type(Fixed/Range/Not Specified)*");
            Map(m => m.Quantity).Name("Quantity");
            Map(m => m.MinimumQuantity).Name("Min Quantity");
            Map(m => m.MaximumQuantity).Name("Max Quantity");
            Map(m => m.QuantityIndicatorType).Name("Billable Quantity(Net/Gross)*");
            Map(m => m.PricingType).Name("Pricing(Market Based/FuelCost/Fixed)*");
            Map(m => m.Price).Name("Price*");
            Map(m => m.RackPricingType).Name("Pricing Type($+, -$, +%, -%)");
            Map(m => m.PricingCode).Name("Pricing Code");
            Map(m => m.CityRackTerminal).Name("City Rack Terminal");
            Map(m => m.ApprovedTerminal).Name("Approved Terminal");
            Map(m => m.DeliveryFee).Name("Delivery Fee");
            Map(m => m.WetHoseFee).Name("Wet Hose Fee");
            Map(m => m.OverWaterFee).Name("Over Water Fee");
            Map(m => m.DryRunFee).Name("Dry Run Fee");
            Map(m => m.FreightFee).Name("Freight Fee");
            Map(m => m.MinimumGallonFee).Name("Minimum Gallon Fee");
            Map(m => m.EnvironmentalFee).Name("Environmental Fee");
            Map(m => m.ServiceFee).Name("Service Fee");
            Map(m => m.LoadFee).Name("Load Fee");
            Map(m => m.SurchargeFee).Name("Surcharge Fee");
            Map(m => m.StopOffFee).Name("Stop Off Fee");
            Map(m => m.DemurrageFeeTerminal).Name("Demurrage Fee Terminal");
            Map(m => m.DemurrageFeeDestination).Name("Demurrage Fee destination");
            Map(m => m.DemurrageFeeOther).Name("Demurrage Fee Other");
            Map(m => m.PumpChargeFee).Name("Pump Charge Fee");
            Map(m => m.SplitTankFee).Name("Split Tank Fee");
            Map(m => m.RetainFee).Name("Retain Fee");
            Map(m => m.AdditiveFee).Name("Additive Fee");
            Map(m => m.PaymentTerm).Name("Net Payment Details (Net/DOR/Prepaid)*");
            Map(m => m.NetDays).Name("Net Days");
            Map(m => m.PaymentMethod).Name("Payment Method");
            Map(m => m.LocationInventoryManagementType).Name("Location Inventory Management");
            Map(m => m.CarrierCompanyName).Name("Carrier Company Name");
            Map(m => m.CarrierEmailAddress).Name("Carrier Email Address");
            Map(m => m.InvoiceCreationPreference).Name("Invoice Creation Preference");
            Map(m => m.EnableAfterTheFactPO).Name("Enable After the Fact PO(Yes/No)");
            Map(m => m.SendInvitationLink).Name("Send Invitation link(Yes/No)");
            Map(m => m.ProvideDeliveryDetailsToCustomer).Name("Provide Delivery Details to Customer(Yes/No)");
            Map(m => m.EnableAssetLevelTracking).Name("Enable Asset Level Tracking (Yes/No)");
            Map(m => m.SupplierAllowance).Name("Supplier Allowance");
            Map(m => m.Notes).Name("Notes");
            Map(m => m.MinimumGallons).Name("Minimum Gallons");
            Map(m => m.EmbeddedTime).Name("Embedded Time");
            Map(m => m.CountyName).Name("Location - County*");

            Map(m => m.AutoFreightPricingMethod).Name("Auto Freight Pricing Method(Yes/No)");
            Map(m => m.SourceRegionNames).Name("Source Region Name(s)*");
            Map(m => m.TerminalNames).Name("Terminal Name(s)*");
            Map(m => m.BulkPlantNames).Name("Bulk Plant Name(s)");
            Map(m => m.ApprovedSourceTerminal).Name("Approved Source Terminal");
            Map(m => m.ApprovedBulkPlant).Name("Approved Bulk Plant");
        }
    }

    public class ThirdPartyCompaniesFilter 
    {
        public string CompanyName { get; set; }
        public List<string> Emails { get; set; }
        public List<string> JobNames { get; set; }
    }
}
