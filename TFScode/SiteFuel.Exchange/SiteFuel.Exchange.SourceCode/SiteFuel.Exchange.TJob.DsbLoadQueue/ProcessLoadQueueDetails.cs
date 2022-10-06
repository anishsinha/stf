using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DsbLoadQueue
{
    public class ProcessLoadQueueDetails
    {
        public async Task ProcessLoadQueue(DsbLoadQueueDomain dsbLoadQueueDomain, ScheduleBuilderDomain scheduleBuilderDomain)
        {

            List<DsbLoadQueueSuccessModel> dsbLoadQueueSuccess = new List<DsbLoadQueueSuccessModel>();
            var dsbLoadQueueDetails = await dsbLoadQueueDomain.GetLoadQueueDetails();
            if (dsbLoadQueueDetails.Any())
            {
                var dsbLoadQueueUserdetails = await dsbLoadQueueDomain.GetLoadUserDetails(dsbLoadQueueDetails.Select(top => top.TfxUserId).ToList());
                var loadQueueUpdateStatus = await dsbLoadQueueDomain.UpdateLoadQueueStatus(dsbLoadQueueDetails.Select(top => top.Id).ToList(), DsbLoadQueueStatus.InProgress);
                if (dsbLoadQueueDetails.Any() && loadQueueUpdateStatus.StatusCode == Status.Success)
                {
                    foreach (var item in dsbLoadQueueDetails)
                    {
                        var userContext = new UserContext();
                        userContext.Id = item.TfxUserId;
                        userContext.CompanyId = item.TfxCompanyId;
                        var userName = dsbLoadQueueUserdetails.FirstOrDefault(top => top.Id == item.Id) != null ? dsbLoadQueueUserdetails.FirstOrDefault(top => top.Id == item.Id).UserName : string.Empty;
                        var sbDriverViewModel = new DSBSaveModel();
                        sbDriverViewModel = JsonConvert.DeserializeObject<DSBSaveModel>(item.DriverColJsonInfo);
                        sbDriverViewModel.CompanyId = item.TfxCompanyId;
                        sbDriverViewModel.UserId = item.TfxUserId;
                        sbDriverViewModel.Trips.ForEach(t => t.UpdatedByName = userName);
                        foreach (var tripitem in sbDriverViewModel.Trips)
                        {
                            tripitem.TripStatus = TripStatus.Modified;
                            tripitem.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                            //find cancel DS and exclude Canceled DS.
                            if (tripitem.DeliveryRequests.Where(x => x.TrackScheduleStatus == (int)TrackableDeliveryScheduleStatus.Canceled).Any())
                            {
                                tripitem.DeliveryRequests.Where(x => x.TrackScheduleStatus != (int)TrackableDeliveryScheduleStatus.Canceled).ToList().ForEach(t => t.Status = (int)DeliveryReqStatus.ScheduleCreated);
                            }
                            else
                            {
                                tripitem.DeliveryRequests.ForEach(t => t.Status = (int)DeliveryReqStatus.ScheduleCreated);
                            }
                        }
                        var apiResponse = await scheduleBuilderDomain.PublishScheduleBuilder(sbDriverViewModel, userContext, item.UserLanguage);
                        if (apiResponse != null)
                        {
                            if (apiResponse.StatusCode != Status.Failed)
                            {
                                dsbLoadQueueSuccess = new List<DsbLoadQueueSuccessModel>();
                                dsbLoadQueueSuccess.Add(new DsbLoadQueueSuccessModel { Id = item.Id, DSBSaveModel = JsonConvert.SerializeObject(apiResponse) });
                                if (dsbLoadQueueSuccess.Any())
                                {
                                    await dsbLoadQueueDomain.UpdateLoadQueueResponse(dsbLoadQueueSuccess);
                                }
                            }
                            else
                            {
                                await dsbLoadQueueDomain.UpdateLoadQueueFailedStatus(item.Id, DsbLoadQueueStatus.Failed, apiResponse.StatusMessage);
                            }
                        }
                    }
                }
                else
                {
                    LogManager.Logger.WriteDebug("ProcessLoadQueue", "UpdateLoadQueueStatus", "Error-UpdateLoadQueueStatus");
                }
            }

        }
    }
}
