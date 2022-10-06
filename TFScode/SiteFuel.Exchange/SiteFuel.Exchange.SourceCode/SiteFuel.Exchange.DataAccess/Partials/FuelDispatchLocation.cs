namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class FuelDispatchLocation
    {
        public FuelDispatchLocation(FuelDispatchLocation originalObj)
        {
            LocationType = originalObj.LocationType;
            OrderId = originalObj.OrderId;
            DeliveryScheduleId = originalObj.DeliveryScheduleId;
            TerminalId = originalObj.TerminalId;
            IsFuturePickUp = originalObj.IsFuturePickUp;
            Address = originalObj.Address;
            AddressLine2 = originalObj.AddressLine2;
            AddressLine3 = originalObj.AddressLine3;
            City = originalObj.City;
            StateCode = originalObj.StateCode;
            StateId = originalObj.StateId;
            ZipCode = originalObj.ZipCode;
            CountryCode = originalObj.CountryCode;
            Latitude = originalObj.Latitude;
            Longitude = originalObj.Longitude;
            IsActive = originalObj.IsActive;
            CreatedBy = originalObj.CreatedBy;
            CreatedDate = originalObj.CreatedDate;
            CountyName = originalObj.CountyName;
            Currency = originalObj.Currency;
            TimeZoneName = originalObj.TimeZoneName;
            DropStatus = originalObj.DropStatus;
            IsJobLocation = originalObj.IsJobLocation;
            SiteName = originalObj.SiteName;
            ParentId = originalObj.ParentId;
            IsSkipped = originalObj.IsSkipped;
        }
    }
}
