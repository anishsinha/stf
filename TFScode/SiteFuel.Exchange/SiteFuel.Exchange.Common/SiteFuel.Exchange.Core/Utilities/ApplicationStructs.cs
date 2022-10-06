using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SiteFuel.Exchange.Utilities
{
    public struct ConfirmationToken
    {
        public int Id { get; set; }

        public string Token { get; set; }
    }

    public class DropdownDisplayItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class DropdownCustomItem
    {
        public int Id { get; set; }
        public bool isDisabled { get; set; }
        public string Name { get; set; }
    }

    public class DropdownDisplayStateItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsWithinState { get; set; }
    }
    //Do not add any property in DropdownDisplayExtendedItem class.
    public class DropdownDisplayExtendedItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DRNote { get; set; }
        public string DeliveryLevelPO { get; set; }
    }//Do not add any property in DropdownDisplayExtendedItem class.
    public class DeliveryScheduleDropdownExtendedItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DRNote { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
    public class DispatchOrderDetailsDropdown
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DRNote { get; set; }
    }
    public class NullableDropdownDisplayExtendedItem
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class SkybitzConfig
    {
        public int IsApi { get; set; }
        public List<DropdownDisplayExtendedItem> DropdownDisplayExtendedItems { get; set; } = new List<DropdownDisplayExtendedItem>();
    }
    public class StateDropdownExtendedItem : DropdownDisplayExtendedItem
    {
        public int? CountryGroupId { get; set; }
        public int CountryId { get; set; }
    }
    public class DropdownDisplayExtended
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class TfxJobsDetails
    {
        public int Id { get; set; }
        public int SequenceNo { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TfxCarrierRegionDetailsModel
    {
        public string Id { get; set; }
        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class DropdownDisplayExtendedId
    {
        public int Id { get; set; }
        public int CodeId { get; set; }
        public string Name { get; set; }
    }

    public class DropdownDisplayExtendedProperty
    {
        public int Id { get; set; }
        public int CodeId { get; set; }
        public string Name { get; set; }
        public bool IsTrue { get; set; }
    }
    public class DropdownDisplayWithSelectedItem
    {
        public List<DropdownDisplayItem> Items { get; set; }

        public List<int> SelectedItems { get; set; }
    }

    public class StoredProcedure
    {
        public StoredProcedure()
        {
            Params = new HashSet<SqlParameter>();
        }

        public string Query { get; set; }

        public ICollection<SqlParameter> Params { get; set; }
    }

    public struct Response
    {
        public Response(Status status)
        {
            StatusCode = status;
            StatusMessages = new List<string>();
            EntityId = 0;
        }

        public Status StatusCode { get; set; }
        public int EntityId { get; set; }
        public IList<string> StatusMessages { get; set; }
    }

    public class CountryState
    {
        public int StateId { get; set; }

        public int CountryId { get; set; }
        public int CountryGroupId { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        public string CountryCode { get; set; }

        public int QuantityIndicatorId { get; set; }
    }

    public class MobileFeeType
    {
        public MobileFeeType()
        {
            CommonFee = true;
            SubTypes = new List<MobileFeeSubType>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool CommonFee { get; set; }

        public int? TruckLoadTypeId { get; set; }

        public IEnumerable<MobileFeeSubType> SubTypes { get; set; }
    }

    public class MobileFeeSubType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class FeeSubTypeDdl
    {
        public string FeeTypeId { get; set; }
        public int FeeSubTypeId { get; set; }
        public string SubTypeName { get; set; }
    }
    public class TBDDropdownDisplayItem
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public string ProductTypeName { get; set; }
    }
    public class DropdownDisplayExtendedListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Code { get; set; }
    }
}
