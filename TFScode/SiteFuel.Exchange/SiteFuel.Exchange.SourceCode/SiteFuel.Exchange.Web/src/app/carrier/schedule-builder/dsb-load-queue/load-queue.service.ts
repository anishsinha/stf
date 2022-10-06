import { Injectable } from "@angular/core";
import { HandleError } from '../../../errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FormArray, FormControl, FormGroup } from "@angular/forms";
import { DSBLoadQueueModel, DSBLoadQueueNotificationModel, DSBLoadQueueNotificationResponse, TrailersDeliveryRequestViewModel } from "./dsb-load.model";
import { DeliveryRequestViewModel, DSBSaveModel, ScheduleShiftModel } from "../../models/DispatchSchedulerModels";


@Injectable()

export class LoadQueueService extends HandleError {

    private validateTrailerJobCompatibilityUrl = '/Carrier/ScheduleBuilder/ValidateTrailerJobCompatibilityForLoadQueue';
    private createLoadQueueUrl = '/Carrier/Freight/CreateLoadQueue';
    private deleteLoadQueueUrl = '/Carrier/Freight/DeleteLoadQueue';
    private saveLoadQueueUrl = '/Carrier/ScheduleBuilder/SaveDsbLoadQueue';
    private getLoadQueueNotificationsUrl = '/Carrier/Freight/GetLoadQueueNotifications';

    public loadQueueColumnMovedSubject: Subject<boolean> = new Subject<boolean>();

    constructor(private httpClient: HttpClient) { super(); }

    getTrailerJobCompatibility(models: TrailersDeliveryRequestViewModel[]): Observable<any> {
        return this.httpClient.post(this.validateTrailerJobCompatibilityUrl, models).pipe(catchError(this.handleError<any>('getTrailerJobCompatibility', null)));
    }

    createLoadQueue(data: DSBLoadQueueModel[]): Observable<any> {
        return this.httpClient.post(this.createLoadQueueUrl, data).pipe(catchError(this.handleError<any>('createLoadQueue', null)));
    }

    deleteLoadQueue(ids: string[]): Observable<any> {
        return this.httpClient.post(this.deleteLoadQueueUrl, ids).pipe(catchError(this.handleError<any>('deleteLoadQueue', null)));
    }

    saveDsbLoadQueue(data: any[]): Observable<any> {
        return this.httpClient.post(this.saveLoadQueueUrl, data).pipe(catchError(this.handleError<any>('saveDsbLoadQueue', null)));
    }

    getFilterForm() {
        return new FormGroup({ Shift: new FormControl(null), Status: new FormControl(null) });
    }

    getLoadQueueNotifications(data: DSBLoadQueueNotificationModel[]): Observable<DSBLoadQueueNotificationResponse[]> {
        return this.httpClient.post(this.getLoadQueueNotificationsUrl, data).pipe(catchError(this.handleError<any>('getLoadQueueNotifications', null)));
    }

    setLoadQueueColumnMoved(data: boolean) {
        this.loadQueueColumnMovedSubject.next(data);
    }

    validatePublishLoad(trip: FormGroup) {
        var isValid = true;
        if (trip.controls.IsCommonPickup.value && !(trip.controls.Terminal.valid || trip.controls.BulkPlant.valid))
            isValid = false;
        return isValid;
    }

    getCustomerAndJobFromDr(drs: DeliveryRequestViewModel[]) {

        let _jobs: string[] = [];
        let _customers: string[] = [];

        drs.forEach(dr => {
            if (dr.CustomerCompany && !_customers.includes(dr.CustomerCompany)) {
                _customers.push(dr.CustomerCompany);
                _jobs.push(dr.JobName || '');
            }
        });
        return { Locations: _jobs, customers: _customers };
    }

    GetAllLoadsDR(trips: FormArray) {
        var _deliveryRequests = [];
        if (trips) {
            for (var i = 0; i < trips.length; i++) {
                var trip = trips.controls[i] as FormGroup;
                var deliveryRequests = trip.controls["DeliveryRequests"].value;
                if (deliveryRequests) {
                    for (var j = 0; j < deliveryRequests.length; j++) {
                        var deliveryRequest = deliveryRequests[j];
                        if (deliveryRequest) {
                            _deliveryRequests.push(deliveryRequest);
                        }
                    }
                }
            }
        }
        return _deliveryRequests;
    }

    getDSBSaveModel(SbForm: FormGroup) {
        var sbModel = SbForm.value;
        var dataToSave = new DSBSaveModel();
        dataToSave.Id = sbModel.Id;
        dataToSave.Date = sbModel.Date;
        dataToSave.RegionId = sbModel.RegionId;
        dataToSave.ObjectFilter = sbModel.ObjectFilter;
        dataToSave.RegionFilter = sbModel.RegionFilter;
        dataToSave.DateFilter = sbModel.DateFilter;
        dataToSave.DSBFilter = sbModel.DSBFilter;
        dataToSave.TimeStamp = sbModel.TimeStamp;
        dataToSave.Status = sbModel.Status;
        dataToSave.WindowMode = sbModel.WindowMode;
        dataToSave.ToggleRequestMode = sbModel.ToggleRequestMode;
        if (sbModel.Id == null) {
            for (var i = 0; i < sbModel.Shifts.length; i++) {
                var shift = new ScheduleShiftModel();
                shift.Id = sbModel.Shifts[i].Id;
                shift.StartTime = sbModel.Shifts[i].StartTime;
                shift.EndTime = sbModel.Shifts[i].EndTime;
                shift.SlotPeriod = sbModel.Shifts[i].SlotPeriod;
                dataToSave.Shifts.push(shift);
            }
        }
        return dataToSave;
    }
}
