using MongoDB.Driver;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class DeliveryReqJobInfoRepository
    {
        private readonly MdbContext mdbContext;
        public DeliveryReqJobInfoRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        protected void GetJobCompatibilityInfo(List<int> jobIds, List<DeliveryRequestViewModel> drs, string selectedDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(selectedDate) && drs != null && drs.Any() && jobIds != null && jobIds.Any())
                {
                    var date = DateTime.Parse(selectedDate);
                    var deliveryDay = (int)date.DayOfWeek;
                    deliveryDay = deliveryDay == 0 ? 7 : deliveryDay; // sunday is saved as 7 in our code but 0 in DayOfWeek enum
                    var compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                    bool IsAcceptNightDeliveries = false;
                    var jobsDetails = mdbContext.JobAdditionalDetails.Find(t => jobIds.Contains(t.TfxJobId)).Project(t => new { t.TfxJobId, t.TrailerType, t.DeliveryDaysList, t.DistanceCovered }).ToList();
                    foreach (var jobId in jobIds)
                    {
                        IsAcceptNightDeliveries = false;
                        compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                        var jobInfoModel = jobsDetails.Where(t => t.TfxJobId == jobId).Select(t => new
                        {
                            TrailerTypes = t.TrailerType,
                            t.DeliveryDaysList,
                            t.DistanceCovered
                        }).FirstOrDefault();
                        if (jobInfoModel != null)
                        {
                            if (jobInfoModel.TrailerTypes != null && jobInfoModel.TrailerTypes.Any())
                            {
                                //compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                                foreach (var trailer in jobInfoModel.TrailerTypes)
                                {
                                    var item = new FreightModels.DropdownDisplayItem();
                                    item.Id = (int)trailer;
                                    item.Name = trailer.GetDisplayName();
                                    compatibleTrailerTypes.Add(item);
                                }
                            }
                            if (jobInfoModel.DeliveryDaysList != null && jobInfoModel.DeliveryDaysList.Any())
                            {
                                var deliveryday = jobInfoModel.DeliveryDaysList.Where(t => t.DeliveryDays.HasValue && t.DeliveryDays == deliveryDay).FirstOrDefault();
                                if (deliveryday != null)
                                {
                                    IsAcceptNightDeliveries = deliveryday.IsAcceptNightDeliveries;
                                }
                            }
                            var setCompatibilityInfo = drs.FindAll(t => t.JobId == jobId).ToList();
                            if (setCompatibilityInfo != null && setCompatibilityInfo.Any())
                            {
                                setCompatibilityInfo.ForEach(t => t.TrailerTypes.AddRange(compatibleTrailerTypes));
                                setCompatibilityInfo.ForEach(t => t.IsAcceptNightDeliveries = IsAcceptNightDeliveries);
                                setCompatibilityInfo.ForEach(t => t.HoursToCoverDistance = jobInfoModel.DistanceCovered);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryReqJobInfoRepository", "GetJobCompatibilityInfo", ex.Message, ex);
            }
        }
    }
}
