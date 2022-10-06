namespace SiteFuel.Models.PricingCode
{
    public class PricingCodeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PricingSourceId { get; set; }
        public int PricingTypeId { get; set; } //market, fixed or fuelcost
        public int RackTypeId { get; set; }
        public int FeedTypeId { get; set; }
        public int QuantityIndicatorId { get; set; }
        public int FuelClassTypeId { get; set; }        
        public int WeekendPricingTypeId { get; set; }
        public bool IsActive { get; set; }
        //public override string ToString()
        //{
        //    return $"INSERT INTO [dbo].[MstPricingCodes] ([Name],[PricingSourceId],[PricingTypeId],[RackTypeId]," +
        //        "[FeedTypeId],[QuantityIndicatorId],[FuelClassTypeId],[WeekendPricingTypeId],[IsActive])"
        //        + $" VALUES('{Name}', {PricingSourceId},{PricingTypeId}, {RackTypeId}, " +
        //        $"{FeedTypeId}, {QuantityIndicatorId}, {FuelClassTypeId}, {WeekendPricingTypeId}, {1})";
        //}
    }
}
