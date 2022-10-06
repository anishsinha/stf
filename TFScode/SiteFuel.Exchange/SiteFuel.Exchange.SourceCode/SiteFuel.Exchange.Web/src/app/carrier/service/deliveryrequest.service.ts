import { Injectable } from '@angular/core';
import { DelRequestsByJobModel, DeliveryRequestViewModel, DelRequestByTimeModel } from '../models/DispatchSchedulerModels';
import { sortBy, sortByDesc } from 'src/app/my.functions';
import { DeliveryReqPriority, DeliveryRequestTypes } from 'src/app/app.enum';

@Injectable({
  providedIn: 'root'
})
export class DeliveryrequestService {

    constructor() { }

    public groupDrsByJob(drs: DeliveryRequestViewModel[], drType: DeliveryRequestTypes = DeliveryRequestTypes.None) {
        var response = [] as DelRequestsByJobModel[];
        for (var i = 0; i < drs.length; i++) {
            var jobIndex = -1;
            if (drs[i].IsTBD == false) {
                jobIndex = response.findIndex(t => t.JobId == drs[i].JobId);
            }
            else {
                jobIndex = response.findIndex(t => t.TBDGroupId == drs[i].TBDGroupId);
            }
            if (jobIndex == -1) {
                var job = new DelRequestsByJobModel();
                job.JobId = drs[i].JobId;
                job.JobName = drs[i].JobName;
                job.DeliveryRequestType = drType;
                job.JobCity = drs[i].JobCity;
                if (drs[i].IsTBD == false) {
                    job.CarrierStatus = this.getCarrierStatus(drs.filter(t => t.JobId == job.JobId));
                    job.Priority = this.getPriority(drs.filter(t => t.JobId == job.JobId));
                } else {
                    job.CarrierStatus = this.getCarrierStatus(drs.filter(t => t.TBDGroupId == job.TBDGroupId));
                    job.Priority = this.getPriority(drs.filter(t => t.TBDGroupId == job.TBDGroupId));
                    job.ProductType = drs[i].ProductType;
                }
                job.CustomerCompany = drs[i].CustomerCompany;
                job.JobAddress = drs[i].JobAddress;
                job.CustomerBrandId = drs[i].CustomerBrandId;
                job.TBDGroupId = drs[i].TBDGroupId;
                if (drs[i].TrailerTypes.length != 5) {
                    job.TrailerCompatibility = drs[i].TrailerTypes.map(t => t.Name).join(',');
                }
                job.IsOnlyNightDelivery = drs[i].IsAcceptNightDeliveries;
                job.HoursToCoverDistance = drs[i].HoursToCoverDistance;
                job._HoursToCoverDistance = drs[i].HoursToCoverDistance ? Number(drs[i].HoursToCoverDistance.replace(/:/g, '')) : 0;
                job.DRQueueAttributes = drs[i].DRQueueAttributes;
                job.LoadQueueAttributes = drs[i].LoadQueueAttributes;
                job.DeliveryRequests.push(drs[i]);
                job.IsTBD = drs[i].IsTBD;
                job.TBDGroupId = drs[i].TBDGroupId;
                job.ProductType = drs[i].ProductType;
                job.ProductTypeId = drs[i].ProductTypeId;
                job.UoM = drs[i].UoM;
                job.RequiredQuantity = drs[i].RequiredQuantity;
                job.ScheduleQuantityTypeText = drs[i].ScheduleQuantityTypeText;
                response.push(job);
            }
            else {
                response[jobIndex].DeliveryRequests.push(drs[i]);
            }
        }
        response = sortByDesc(response, '_HoursToCoverDistance');
        return response;
    }
    private getHourFromTime(time :string) {
        let hour: number = 0;
        hour = Number(time.split(":")[0]);
        if (time.indexOf("PM") > -1 && hour < 12) {
            hour += 12;
        }
        if (time.indexOf("AM") > -1 && hour == 12) {
            hour = 0;
        }
        return hour;
    }
    public groupDrsBySelectedTime(drs: DeliveryRequestViewModel[], drType: DeliveryRequestTypes = DeliveryRequestTypes.None) {
        var response = [] as DelRequestByTimeModel[];
        for (var i = 0; i < drs.length; i++) {
            var timeIndex = -1;
            const startHour: number = this.getHourFromTime(drs[i].ScheduleStartTime.toString());
            const endHour: number = this.getHourFromTime(drs[i].ScheduleEndTime.toString());
            if (drs[i].ScheduleStartTime && drs[i].ScheduleEndTime) { 
                timeIndex = response.findIndex(t => t.StartTime == startHour && t.EndTime == endHour);
            }
            if (timeIndex == -1) {
                var timeDrs = new DelRequestByTimeModel();
                timeDrs.StartTime = startHour;
                timeDrs.EndTime = endHour;
                timeDrs.DeliveryRequests.push(drs[i]);
                response.push(timeDrs);
            }
            else {
                response[timeIndex].DeliveryRequests.push(drs[i]);
            }
        }
        response = sortBy(response, 'StartTime');
        return response;
    }

    public getCarrierStatus(drs: DeliveryRequestViewModel[]) {
        if (drs.every(t => t.CarrierStatus == 2)) {
            return 2;
        }
        else if (drs.every(t => t.CarrierStatus == 3)) {
            return 3;
        }
        return 0;
    }

    public getMustGoRequests(drs: DelRequestsByJobModel[]) {
        var mustGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) != -1);
        mustGoRequests.forEach(t => { t.DeliveryRequestType = DeliveryRequestTypes.MustGo; t.Priority = DeliveryReqPriority.MustGo; });
        return mustGoRequests;
    }

    public getShouldGoRequests(drs: DelRequestsByJobModel[]) {
        var shouldGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 2) != -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) == -1);
        shouldGoRequests.forEach(t => { t.DeliveryRequestType = DeliveryRequestTypes.ShouldGo; t.Priority = DeliveryReqPriority.ShouldGo; });
        return shouldGoRequests;
    }

    public getCouldGoRequests(drs: DelRequestsByJobModel[]) {
        var couldGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 3) != -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) == -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 2) == -1);
        couldGoRequests.forEach(t => { t.DeliveryRequestType = DeliveryRequestTypes.CouldGo; t.Priority = DeliveryReqPriority.CouldGo; });
        return couldGoRequests;
    }

    public getPriority(drs: DeliveryRequestViewModel[]) {
        if (drs.some(t => t.Priority == 1)) {
            return DeliveryReqPriority.MustGo;
        }
        else if (drs.some(t => t.Priority == 2) && !drs.some(t => t.Priority == 1)) {
            return DeliveryReqPriority.ShouldGo;
        }
        else if (drs.some(t => t.Priority == 3) && !drs.some(t => t.Priority == 1) && !drs.some(t => t.Priority == 2)) {
            return DeliveryReqPriority.CouldGo;
        }
        return DeliveryReqPriority.None;
    }
}
