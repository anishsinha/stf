import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DropdownItemExt } from 'src/app/statelist.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { ScheduleBuilderModel, DeliveryRequestViewModel, DelRequestsByJobModel } from '../carrier/models/DispatchSchedulerModels';
import { CarrierService } from '../carrier/service/carrier.service';
import { ScheduleBuilderService } from '../carrier/service/schedule-builder.service';
import { DeliveryrequestService } from '../carrier/service/deliveryrequest.service';
import { sortBy } from '../my.functions';
import { DeliveryReqPriority, DeliveryRequestTypes } from 'src/app/app.enum';
declare var regionId: string;
declare var currentUserCompanyId: number;
declare function IsUserActive(): boolean;

@Component({
    selector: 'app-delivery-request-display',
    templateUrl: './delivery-request-display.component.html',
    styleUrls: ['./delivery-request-display.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class DeliveryRequestDisplayComponent implements OnInit {
    public ScheduleBuilder: ScheduleBuilderModel;
    public deliveryRequests: DeliveryRequestViewModel[] = [];
    public selectedJobRequests: DeliveryRequestViewModel[] = [];
    public mustGoRequests: DelRequestsByJobModel[] = [];
    public tempmustGoRequests: DelRequestsByJobModel[] = [];
    public localStorageMustGoRequests: DelRequestsByJobModel[] = [];
    public missedRequests: DelRequestsByJobModel[] = [];
    public tempMissedRequests: DelRequestsByJobModel[] = [];
    public localStorageMissedRequests: DelRequestsByJobModel[] = [];
    public shouldGoRequests: DelRequestsByJobModel[] = [];
    public tempshouldGoRequests: DelRequestsByJobModel[] = [];
    public localStorageShouldGoRequests: DelRequestsByJobModel[] = [];
    public couldGoRequests: DelRequestsByJobModel[] = [];
    public tempcouldGoRequests: DelRequestsByJobModel[] = [];
    public localStorageCouldGoRequests: DelRequestsByJobModel[] = [];
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public Regions: DropdownItemExt[] = [];
    public assignedByOtherRegionRequests: DeliveryRequestViewModel[] = [];
    public assignedByOtherOperatorRequests: DeliveryRequestViewModel[] = [];
    public assignedToOtherRegionRequests: DeliveryRequestViewModel[] = [];
    public assignedToOtherOperatorRequests: DeliveryRequestViewModel[] = [];
    public SbForm: FormGroup;
    public intervalTime: any;
    public updateintervalTime: any;
    public changeDeteaction: boolean = false;
    public IsThisFromDrDisplay: boolean = false;
    public regionId: string;
    public selectedDate: string;
    public _loadingDrRequests = false;
    constructor(private carrierService: CarrierService,
        private sbService: ScheduleBuilderService,
        private deliveryRequestService: DeliveryrequestService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        localStorage.setItem("deliveryRequests", JSON.stringify([]));
        this.regionId = this.route.snapshot.queryParamMap.get('regionId');
        this.selectedDate = this.route.snapshot.queryParamMap.get('selectedDate');

        if (!this.selectedDate || this.selectedDate.indexOf('null') !== -1 || this.selectedDate.indexOf('undefined') !== -1)
            this.selectedDate = ''

        this.getDeliveryRequests(regionId);
        this.SbForm = this.initForm();
        localStorage.setItem("regionId", JSON.stringify(regionId));
        this.changeDeteaction = false;
        this.IsThisFromDrDisplay = true;
        this.SbForm.get('searchField').valueChanges
            .pipe(
                debounceTime(500),
                distinctUntilChanged()
            )
            .subscribe(searchText => {

                this.filterRecords(searchText);
            });
        this.checkRegionChange();
        this.checkRecordUpdateORDelete();
    }
    initForm(): FormGroup {
        var _form = this.fb.group({
            searchField: this.fb.control('')
        });
        return _form;
    }
    checkRegionChange() {
        this.intervalTime = setInterval(() => {
            if (IsUserActive()) {
                if (localStorage.getItem("refreshRegion") != null) {
                    var changeDeteaction = JSON.parse(localStorage.getItem("refreshRegion")) as boolean;
                    if (changeDeteaction) {
                        this.changeDeteaction = changeDeteaction;
                        this.resetLocalStorage();
                        setTimeout(function () {
                            window.top.close(); // close current tab after 10 seconds
                        }, 10000);
                    }
                }
            }
        }, 3000);

    }
    checkRecordUpdateORDelete() {
        this.updateintervalTime = setInterval(() => {
            if (IsUserActive()) {
                if (localStorage.getItem("updateRequest") != null) {
                    var updateRequest = JSON.parse(localStorage.getItem("updateRequest")) as boolean;
                    if (updateRequest) {
                        localStorage.setItem("updateRequest", JSON.stringify(false));
                        window.location.reload();//refresh the current window
                    }
                }
            }


        }, 5000);
    }

    filterRecords(term: string) {
        if (term) {
            term = term.trim().toLowerCase();
            this.filterMustGoRequest(term);
            this.filterShouldGoRequest(term);
            this.filterCouldGoRequest(term);
            this.filterMissedRequest(term);
        }
        else {
            this.mustGoRequests = this.tempmustGoRequests;
            this.shouldGoRequests = this.tempshouldGoRequests;
            this.couldGoRequests = this.tempcouldGoRequests;
            this.missedRequests = this.tempMissedRequests;
        }
    }
    filterMustGoRequest(term: string) {

        let _localmustGoRequests: DeliveryRequestViewModel[] = [];

        this.tempmustGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localmustGoRequests.push(dr); });
            }
        });

        let _mustGorecords = _localmustGoRequests.filter((element: DeliveryRequestViewModel) =>
            ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
                || (element.JobName && element.JobName.toLowerCase().startsWith(term))
                || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
                || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
                || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
                || element.RequiredQuantity.toString().startsWith(term)))

        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_mustGorecords);
        this.mustGoRequests = this.deliveryRequestService.getMustGoRequests(groupedDrs);
        this.mustGoRequests.slice();
    }
    filterShouldGoRequest(term: string) {

        let _localshouldGoRequests: DeliveryRequestViewModel[] = [];

        this.tempshouldGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localshouldGoRequests.push(dr); });
            }
        });

        let _shouldGorecords = _localshouldGoRequests.filter((element: DeliveryRequestViewModel) =>
            ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
                || (element.JobName && element.JobName.toLowerCase().startsWith(term))
                || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
                || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
                || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
                || element.RequiredQuantity.toString().startsWith(term)))

        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_shouldGorecords);
        this.shouldGoRequests = this.deliveryRequestService.getShouldGoRequests(groupedDrs);
        this.shouldGoRequests.slice();
    }
    filterCouldGoRequest(term: string) {

        let _localcouldGoRequests: DeliveryRequestViewModel[] = [];

        this.tempcouldGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localcouldGoRequests.push(dr); });
            }
        });

        let _couldGorecords = _localcouldGoRequests.filter((element: DeliveryRequestViewModel) =>
            ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
                || (element.JobName && element.JobName.toLowerCase().startsWith(term))
                || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
                || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
                || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
                || element.RequiredQuantity.toString().startsWith(term)))

        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_couldGorecords);
        this.couldGoRequests = this.deliveryRequestService.getCouldGoRequests(groupedDrs);
        this.couldGoRequests.slice();
    }
    filterMissedRequest(term: string) {

        let _localMissedRequests: DeliveryRequestViewModel[] = [];

        this.tempMissedRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localMissedRequests.push(dr); });
            }
        });

        let _missedrecords = _localMissedRequests.filter((element: DeliveryRequestViewModel) =>
            ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
                || (element.JobName && element.JobName.toLowerCase().startsWith(term))
                || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
                || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
                || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
                || element.RequiredQuantity.toString().startsWith(term)));

        this.missedRequests = this.deliveryRequestService.groupDrsByJob(_missedrecords, DeliveryRequestTypes.Missed);
        this.missedRequests.slice();
    }
    ngOnDestroy(): void {
        localStorage.setItem("deliveryRequests", JSON.stringify([]));
    }
    getDeliveryRequests(regionId: string): void {
        this._loadingDrRequests = true;
        this.carrierService.getDeliveryRequests(regionId, this.selectedDate).subscribe(dr => {
            if (dr != null && dr != undefined) 
            {
                dr = dr.filter(t => !t.IsCalendarView); // hide calender Dr
                dr = this.filterDrByScheduleBuilder(dr);
                this.deliveryRequests = dr;
                var priorityRequests = this.deliveryRequests.filter(t => t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId);
                var missedRequests = this.deliveryRequests.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
                var groupedDrs = this.deliveryRequestService.groupDrsByJob(priorityRequests);
                //
                this.mustGoRequests = this.deliveryRequestService.getMustGoRequests(groupedDrs);
                this.mustGoRequests.slice();
                this.tempmustGoRequests = this.mustGoRequests;
                this.tempmustGoRequests.slice();
                //
                this.shouldGoRequests = this.deliveryRequestService.getShouldGoRequests(groupedDrs);
                this.shouldGoRequests.slice();
                this.tempshouldGoRequests = this.shouldGoRequests;
                this.tempshouldGoRequests.slice();
                //
                this.couldGoRequests = this.deliveryRequestService.getCouldGoRequests(groupedDrs);
                this.couldGoRequests.slice();
                this.tempcouldGoRequests = this.couldGoRequests;
                this.tempcouldGoRequests.slice();
                //
                this.missedRequests = this.deliveryRequestService.groupDrsByJob(missedRequests, DeliveryRequestTypes.Missed);
                this.missedRequests.slice();
                this.tempMissedRequests = this.missedRequests;
                this.missedRequests.slice();

                this.assignedByOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedByOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId != currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedToOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedToOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId != currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.getLocalStorageItems();
            }
            this._loadingDrRequests = false;
        });
    }
    filterDrByScheduleBuilder(drs: DeliveryRequestViewModel[]): DeliveryRequestViewModel[] {
        var _scheduleRequests = [];
        if (this.ScheduleBuilder != undefined && this.ScheduleBuilder != null) {
            this.ScheduleBuilder.Shifts.forEach(s => {
                s.Schedules.forEach(sc => {
                    sc.Trips.forEach(t => {
                        t.DeliveryRequests.forEach(d => {
                            _scheduleRequests.push(d.Id);
                        });
                    });
                });
            });
            drs = drs.filter(x => {
                return _scheduleRequests.find(y => y == x.Id) == undefined;
            });
        }
        return drs;
    }
    removeDraggedRequest(drData: any, deliveryRequests: DeliveryRequestViewModel[]): void {
        var index = deliveryRequests.findIndex(x => x.Priority == drData.Priority && x.Id == drData.Id);
        if (index >= 0) {
            deliveryRequests = deliveryRequests.splice(index, 1);
        }
    }

    getSelectedLocationPriority(jobId: any, isMissed: boolean, isTBD : boolean) {
        var response = null as DelRequestsByJobModel;
        if (isMissed) {
            response = this.missedRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
            return response;
        }
        response = this.mustGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
        if (!response) {
            response = this.shouldGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
            if (!response)
                response = this.couldGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
        }
        return response;
    }

    getLocalStorageItems() {
        var mustGoDeliveryRequests = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var shouldGodeliveryRequests = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var couldGodeliveryRequests = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var missedDeliveryRequests = JSON.parse(localStorage.getItem("missedDeliveryRequest")) as DelRequestsByJobModel[] || [];

        if (mustGoDeliveryRequests != null) {
            mustGoDeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    mustGoDeliveryRequests[i] = request;
                    if (request.Priority == DeliveryReqPriority.ShouldGo) {
                        shouldGodeliveryRequests.push(request);
                        mustGoDeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == DeliveryReqPriority.CouldGo) {
                        couldGodeliveryRequests.push(request);
                        mustGoDeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    mustGoDeliveryRequests.splice(i, 1);
                }
            });
        }

        if (shouldGodeliveryRequests != null) {
            shouldGodeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    shouldGodeliveryRequests[i] = request;
                    if (request.Priority == DeliveryReqPriority.MustGo) {
                        mustGoDeliveryRequests.push(request);
                        shouldGodeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == DeliveryReqPriority.CouldGo) {
                        couldGodeliveryRequests.push(request);
                        shouldGodeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    shouldGodeliveryRequests.splice(i, 1);
                }
            });
        }

        if (couldGodeliveryRequests != null) {
            couldGodeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    couldGodeliveryRequests[i] = request;
                    if (request.Priority == DeliveryReqPriority.MustGo) {
                        mustGoDeliveryRequests.push(request);
                        couldGodeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == DeliveryReqPriority.ShouldGo) {
                        shouldGodeliveryRequests.push(request);
                        couldGodeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    couldGodeliveryRequests.splice(i, 1);
                }
            });
        }
        if (missedDeliveryRequests != null) {
            missedDeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, true, t.IsTBD);
                if (request) {
                    missedDeliveryRequests[i] = request;
                }
                else {
                    missedDeliveryRequests.splice(i, 1);
                }
            });
        }
        this.localStorageMustGoRequests = mustGoDeliveryRequests;
        this.localStorageMustGoRequests.slice();
        localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(mustGoDeliveryRequests));
        this.localStorageShouldGoRequests = shouldGodeliveryRequests;
        this.localStorageShouldGoRequests.slice();
        localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(shouldGodeliveryRequests));
        this.localStorageCouldGoRequests = couldGodeliveryRequests;
        this.localStorageCouldGoRequests.slice();
        localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(couldGodeliveryRequests));
        this.localStorageMissedRequests = missedDeliveryRequests;
        this.localStorageMissedRequests.slice();
        localStorage.setItem("missedDeliveryRequest", JSON.stringify(missedDeliveryRequests));
        localStorage.setItem("refreshLocalStorage", "true");
    }

    pushItem(location: DelRequestsByJobModel, elementId: number) {
        let isTbd = location.IsTBD;
        localStorage.setItem("refreshLocalStorage", 'true');

        location.DeliveryRequests.forEach(t => { t.WindowMode = 2; t.QueueMode = 2; });
        var element = $("#" + elementId);
        if (location.DeliveryRequestType == DeliveryRequestTypes.Missed) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageMissedRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageMissedRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("missedDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
                else {
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else if (location.DeliveryRequestType == DeliveryRequestTypes.MustGo) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageMustGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageMustGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageMustGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("mustGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
                else {
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else if (location.DeliveryRequestType == DeliveryRequestTypes.ShouldGo) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageShouldGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("shouldGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
                else {
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageCouldGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("couldGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
                else {
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }

    }
    removeItem(locationId, locationPriority, isTBD) {

        localStorage.setItem("refreshLocalStorage", 'true');

        if (locationPriority == DeliveryRequestTypes.Missed) {
            if (this.localStorageMissedRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageMissedRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageMissedRequests.splice(index, 1);
                    this.localStorageMissedRequests.slice();
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
            }
        }
        else if (locationPriority == DeliveryRequestTypes.MustGo) {
            if (this.localStorageMustGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageMustGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageMustGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageMustGoRequests.splice(index, 1);
                    this.localStorageMustGoRequests.slice();
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
            }
        }
        else if (locationPriority == DeliveryRequestTypes.ShouldGo) {
            if (this.localStorageShouldGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageShouldGoRequests.splice(index, 1);
                    this.localStorageShouldGoRequests.slice();
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
            }
        }
        else {
            if (this.localStorageCouldGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageCouldGoRequests.splice(index, 1);
                    this.localStorageCouldGoRequests.slice();
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
            }
        }
    }
    resetLocalStorage() {

        localStorage.setItem("refreshLocalStorage", 'true');

        localStorage.setItem("mustGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("couldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("missedDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("refreshRegion", JSON.stringify(false));
        localStorage.setItem("regionId", JSON.stringify(''));
        localStorage.setItem("updateRequest", JSON.stringify(false));
    }

    bindDeliveryRequests(location: DelRequestsByJobModel, $event) {
        $event.stopPropagation();
        this.selectedJobRequests = sortBy(location.DeliveryRequests, 'ProductType');
    }

    checkItemsExists(jobId: any, drType: DeliveryRequestTypes, isTbd: boolean) {
        var updateRequest = null;
        if (localStorage.getItem("recordPriorityChange") != null) {
            updateRequest = JSON.parse(localStorage.getItem("recordPriorityChange")) as DeliveryRequestViewModel;
        }
        if (drType == DeliveryRequestTypes.Missed) {
            if (this.localStorageMissedRequests.length > 0) {
                let index = -1;
                if (isTbd == false)
                    index = this.localStorageMissedRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    var drPriority = this.deliveryRequestService.getPriority(this.localStorageMissedRequests[index].DeliveryRequests);
                    if (drPriority == DeliveryReqPriority.MustGo) {
                        return "radius-5 pa10 bg-lightgrey mustgo selected";
                    }
                    if (drPriority == DeliveryReqPriority.ShouldGo) {
                        return "radius-5 pa10 bg-lightgrey shouldgo selected";
                    }
                    if (drPriority == DeliveryReqPriority.CouldGo) {
                        return "radius-5 pa10 bg-lightgrey couldgo selected";
                    }
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    if (drPriority == 1) {
                        return "radius-5 pa10 bg-lightgrey mustgo selected";
                    }
                    if (drPriority == 2) {
                        return "radius-5 pa10 bg-lightgrey shouldgo selected";
                    }
                    if (drPriority == 3) {
                        return "radius-5 pa10 bg-lightgrey couldgo selected";
                    }
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == DeliveryRequestTypes.MustGo) {
            if (this.localStorageMustGoRequests.length > 0) {
                var index = -1;
                if (isTbd == false)
                    index = this.localStorageMustGoRequests.findIndex(x => x && x.JobId && x.JobId == jobId);
                else
                    index = this.localStorageMustGoRequests.findIndex(x => x && x.TBDGroupId && x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == DeliveryRequestTypes.ShouldGo) {
            if (this.localStorageShouldGoRequests.length > 0) {
                var index = -1;
                if (isTbd == false)
                    index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == DeliveryRequestTypes.CouldGo) {
            if (this.localStorageCouldGoRequests.length > 0) {
                let index = -1;
                if (isTbd == false)
                    index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
    }
    GetDrsForMultiWindow() {
        this.getDeliveryRequests(regionId);
    }
}

