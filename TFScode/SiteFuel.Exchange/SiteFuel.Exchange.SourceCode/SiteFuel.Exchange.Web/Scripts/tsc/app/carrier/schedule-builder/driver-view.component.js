import { __decorate } from "tslib";
import { Component, Input, ChangeDetectionStrategy, ViewChildren } from '@angular/core';
import { FormGroup, FormArray, Validators } from '@angular/forms';
import { ScheduleShiftModel, TripModel, DriverViewFilterModel, DSBSaveModel, PreLoadDrViewModel, ModifiedTripInfo, CompartmentModel } from '../models/DispatchSchedulerModels';
import { Declarations } from 'src/app/declarations.module';
import { BehaviorSubject } from 'rxjs';
import { CustomAbstractControls } from '../customAbstractControl';
import * as moment from 'moment';
import { DriverScheduleComponent } from './child-components/driver-schedule.component';
import { sortArrayTwice, groupDrsByProduct, sortBy } from 'src/app/my.functions';
import { DeliveryGroupStatus, DeliveryReqStatus, TripStatus } from 'src/app/app.enum';
let DriverViewComponent = class DriverViewComponent {
    constructor(fb, sbService, dataService, chatService, utilService, changeDetectorRef, zone) {
        this.fb = fb;
        this.sbService = sbService;
        this.dataService = dataService;
        this.chatService = chatService;
        this.utilService = utilService;
        this.changeDetectorRef = changeDetectorRef;
        this.zone = zone;
        this.DriverViewFilter = new DriverViewFilterModel;
        this._isDriverFound = false;
        this._loadingRequests = false;
        this._loadingDrivers = false;
        this._savingBuilder = false;
        this._loadingCmprts = false;
        this._publishedRequestExists = false;
        this.PreLoadInfo = null;
        this.SelectedDriverName = '';
        this.SelectedTrailers = [];
        this.UnassignedTrailers = [];
        this.UnassignedDrivers = [];
        this.AllUnassignedDrivers = [];
        this.chatDriverDetails = [];
        this.TrailerDdlSettings = {};
        this.Collapsed = [];
        this.CollapsedIcons = [];
        this.CompletedScheduleStatuses = [7, 8, 9, 10, 24];
        this.OnTheWayScheduleStatuses = [1, 3, 9, 11, 12, 13, 15, 16, 17, 18, 19, 20];
        this.CompartmentEditModel = {};
        this.TrailerCompartments = {};
        this.TrailerCompartmentRetains = [];
        this.ShiftPaginationInfo = [];
        this.disableControl = false;
        this.SelectedTrip = null;
        this.RouteListForTrip = [];
        this._otherRegionDriverSubject = new BehaviorSubject(false);
    }
    ngOnInit() {
        this.DriverTrailerForm = this.utilService.getDriverTrailerForm();
        this._isDriverFound = false;
        this.TrailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'TrailerId',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 2,
            allowSearchFilter: true,
        };
        this.CompartmentViewFormGroup = this.utilService.getCompartmentViewForm([], null);
        this.dataService.setShowDeliveryGroupSubject(false);
        this.subscribeDeliveryGroupEvents();
        this.subscribeSaveModifiedLoadsSubject();
        this.subscribeEditDriverTrailerEvents();
        this.subscribeDraftAndPublishEvents();
        this.subscribeDragDropItemSubject();
        this.subscribeDisableControlsSubject();
        this.subscriberCreateLoadCancelSubject();
        this.subscribeCreateLoadSuccessSubject();
        this.subscribeCreatePreloadSubject();
        this.subscribeUpdatePostloadSubject();
        this.subscribeDeletePostloadSubject();
        this.setRowVisibiliryObserver();
        this.subscribeEditCompartmentAssigmentSubject();
        this.subscribeRouteInfoDSEvents();
        this.CompartmentDetails = [];
        this.TrailerCompartmentRetains = [];
    }
    ngOnChanges(change) {
        if (change.Shifts && change.Shifts.currentValue) {
            this.dataService.setShowDeliveryGroupSubject(false);
            this.subscribeDisableControlsSubject();
            this.initShiftPageInfo(this.Shifts);
            this.initShifts(this.Shifts);
            this.dataService.setAllShiftsSubject(this.Shifts);
            this.resetDriverTrailerForm();
            this.setRowVisibiliryObserver();
        }
        if (change.RegionDetail && change.RegionDetail.currentValue) {
            this.dataService.setAllTrailersSubject(this.RegionDetail.Trailers);
        }
    }
    ngAfterViewInit() {
        this.dataService.setUnsavedChangesAsEmptySubject();
        this.setUnsavedChanges();
        //this.setRowVisibiliryObserver();
    }
    ngOnDestroy() {
        this.unsubscribeDeliveryGroupEvents();
        this.unsubscribeEditDriverTrailerEvents();
        this.unsubscribeDraftAndPublishEvents();
        this.unsubscribeDragDropItemSubject();
        this.unsubscriberCreateLoadCancelSubject();
        this.unsubscribeCreateLoadSuccessSubject();
        this.unsubscribeCreatePreloadSubject();
        this.unsubscribeUpdatePostloadSubject();
        this.unsubscribeDeletePostloadSubject();
        var _shifts = this.SbForm.get('Shifts');
        if (_shifts) {
            _shifts.clear(); // Clear shifts of driver view
        }
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
        if (this.RouteInfoDSSubscription) {
            this.RouteInfoDSSubscription.unsubscribe();
        }
    }
    subscribeDeliveryGroupEvents() {
        this.subscribeDraftDeliveryGroupSubject();
        this.subscribePublishDeliveryGroupSubject();
        this.subscribeCancelDeliveryGroupSubject();
        this.subscribeDeleteDeliveryGroupSubject();
    }
    unsubscribeDeliveryGroupEvents() {
        if (this.DraftDeliveryGroupSubscription) {
            this.DraftDeliveryGroupSubscription.unsubscribe();
        }
        if (this.SaveModifiedLoadsSubscription) {
            this.SaveModifiedLoadsSubscription.unsubscribe();
        }
        if (this.PublishDeliveryGroupSubscription) {
            this.PublishDeliveryGroupSubscription.unsubscribe();
        }
        if (this.DeleteDeliveryGroupSubscription) {
            this.DeleteDeliveryGroupSubscription.unsubscribe();
        }
        if (this.CancelDeliveryGroupSubscription) {
            this.CancelDeliveryGroupSubscription.unsubscribe();
        }
    }
    unsubscribeEditDriverTrailerEvents() {
        if (this.EditDriverTrailerSubscription) {
            this.EditDriverTrailerSubscription.unsubscribe();
        }
        if (this.SaveDriverAssignmentSubscription) {
            this.SaveDriverAssignmentSubscription.unsubscribe();
        }
    }
    unsubscribeDraftAndPublishEvents() {
        if (this.PublishEntireRowSubscription) {
            this.PublishEntireRowSubscription.unsubscribe();
        }
    }
    unsubscribeDragDropItemSubject() {
        if (this.DragDropItemSubscription) {
            this.DragDropItemSubscription.unsubscribe();
        }
    }
    unsubscriberCreateLoadCancelSubject() {
        if (this.CreateLoadCancelSubscription) {
            this.CreateLoadCancelSubscription.unsubscribe();
        }
    }
    unsubscribeCreateLoadSuccessSubject() {
        if (this.CreateLoadSuccessSubscription) {
            this.CreateLoadSuccessSubscription.unsubscribe();
        }
    }
    unsubscribeCreatePreloadSubject() {
        if (this.CreatePreloadSubscription) {
            this.CreatePreloadSubscription.unsubscribe();
        }
    }
    unsubscribeUpdatePostloadSubject() {
        if (this.UpdatePostloadSubscription) {
            this.UpdatePostloadSubscription.unsubscribe();
        }
    }
    unsubscribeDeletePostloadSubject() {
        if (this.DeletePostloadSubscription) {
            this.DeletePostloadSubscription.unsubscribe();
        }
    }
    subscribeDraftDeliveryGroupSubject() {
        this.DraftDeliveryGroupSubscription = this.dataService.DraftDeliveryGroupSubject.subscribe(x => {
            this.draftScheduleBuilder(x.trip, x.filterChanged);
        });
    }
    subscribeSaveModifiedLoadsSubject() {
        this.SaveModifiedLoadsSubscription = this.dataService.SaveModifiedLoadsSubject.subscribe(x => {
            if (x && x.length > 0) {
                this.saveScheduleBuilderData(x, true);
            }
        });
    }
    subscribePublishDeliveryGroupSubject() {
        this.PublishDeliveryGroupSubscription = this.dataService.PublishDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.publishScheduleBuilder(x.shiftIndex, x.rowIndex, x.colIndex, x.schedule, x.trip);
            }
        });
    }
    subscribeDisableControlsSubject() {
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }
    subscribeCancelDeliveryGroupSubject() {
        this.CancelDeliveryGroupSubscription = this.dataService.CancelDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.cancelScheduleBuilder(x.shiftIndex, x.rowIndex, x.tripIndex, x.trip);
            }
        });
    }
    subscribeDeleteDeliveryGroupSubject() {
        this.DeleteDeliveryGroupSubscription = this.dataService.DeleteDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.deleteGroup(x);
            }
        });
    }
    subscribeEditDriverTrailerEvents() {
        this.EditDriverTrailerSubscription = this.dataService.EditDriverTrailerSubject.subscribe(x => {
            if (x) {
                this.editDriverTrailers(x);
            }
        });
        this.SaveDriverAssignmentSubscription = this.dataService.SaveDriverAssignmentSubject.subscribe(x => {
            if (x) {
                this.saveDriverAssignment(x.Index1, x.Index2);
            }
        });
    }
    subscribeDraftAndPublishEvents() {
        this.PublishEntireRowSubscription = this.dataService.PublishEntireRowSubject.subscribe(x => {
            if (x) {
                this.validateRowPublish(x.ShiftIndex, x.ScheduleIndex);
            }
        });
    }
    subscribeDragDropItemSubject() {
        this.DragDropItemSubscription = this.dataService.DragDropItemSubject.subscribe(x => {
            if (x) {
                this.onItemDrop(x.trip, x.event, x.shiftIndex, x.rowIndex, x.colIndex, x.schedule);
            }
        });
    }
    subscriberCreateLoadCancelSubject() {
        this.CreateLoadCancelSubscription = this.dataService.CreateLoadCancelSubject.subscribe(x => {
            if (x) {
                this.cancelCreateLoad(x);
            }
        });
    }
    subscribeCreateLoadSuccessSubject() {
        this.CreateLoadSuccessSubscription = this.dataService.CreateLoadSuccessSubject.subscribe(x => {
            if (x) {
                this.updateTripOnCreateLoadSucess(x);
            }
        });
    }
    subscribeCreatePreloadSubject() {
        this.CreatePreloadSubscription = this.dataService.CreatePreloadSubject.subscribe(x => {
            if (x) {
                this.processPreloadDeliveryCreation(x);
            }
        });
    }
    subscribeUpdatePostloadSubject() {
        this.UpdatePostloadSubscription = this.dataService.UpdatePostloadSubject.subscribe(x => {
            if (x) {
                this.updateEditedPostloadDrs(x);
            }
        });
    }
    subscribeDeletePostloadSubject() {
        this.DeletePostloadSubscription = this.dataService.DeletePostloadSubject.subscribe(x => {
            if (x) {
                this.deletePreAndPostloadDrs(x);
            }
        });
    }
    subscribeEditCompartmentAssigmentSubject() {
        this.dataService.EditCompartmentAssigmentSubject.subscribe(x => {
            if (x) {
                this.editCompartmentAssignments(x);
            }
        });
    }
    subscribeRouteInfoDSEvents() {
        this.RouteInfoDSSubscription = this.dataService.RouteDetailsSubject.subscribe(x => {
            if (x) {
                this.SelectedTrip = x.SelectedTrip;
                this.RouteListForTrip = x.RouteListForTrip;
            }
        });
    }
    initShifts(shifts) {
        var _shiftArray = this.SbForm.controls['Shifts'];
        _shiftArray.clear();
        shifts.forEach((x, i) => {
            _shiftArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                Schedules: this.fb.array([]),
                StartTime: this.fb.control(x.StartTime),
                EndTime: this.fb.control(x.EndTime),
                SlotPeriod: this.fb.control(x.SlotPeriod),
                IsCollapsed: this.fb.control(x.IsCollapsed),
                IsVisible: this.fb.control(true),
            }));
            this.Collapsed.push((x.IsCollapsed ? '' : 'show'));
            this.CollapsedIcons.push((x.IsCollapsed ? 'collapsed' : ''));
        });
        for (var idx = 0; idx < shifts.length; idx++) {
            if (shifts[idx].Schedules.length <= Declarations.ShiftVisibleRows) {
                this.setShiftRows(idx, shifts[idx].Schedules);
            }
            else {
                let firstNrows = shifts[idx].Schedules.slice(0, Declarations.ShiftVisibleRows);
                this.setShiftRows(idx, firstNrows);
            }
            if (idx == shifts.length - 1) {
                break;
            }
        }
        //this.renderNextAllRows();
    }
    renderNextAllRows() {
        this.zone.runOutsideAngular(() => {
            window.setTimeout(() => {
                for (var idx = 0; idx < this.Shifts.length; idx++) {
                    let shiftPageInfo = this.ShiftPaginationInfo[idx];
                    let endNRowIndex = this.Shifts[idx].Schedules.length - 1;
                    let nextNrows = this.Shifts[idx].Schedules.slice(shiftPageInfo.NextRowIndex, endNRowIndex);
                    if (nextNrows.length > 0) {
                        this.setShiftRows(idx, nextNrows);
                    }
                }
            }, 10);
        });
    }
    renderNextNRows(shiftIndex) {
        this.zone.runOutsideAngular(() => {
            let shiftPageInfo = this.ShiftPaginationInfo[shiftIndex];
            let endNRowIndex = shiftPageInfo.NextRowIndex + Declarations.ShiftVisibleRows;
            let nextNrows = this.Shifts[shiftIndex].Schedules.slice(shiftPageInfo.NextRowIndex, endNRowIndex);
            if (nextNrows.length > 0) {
                this.setShiftRows(shiftIndex, nextNrows);
            }
        });
    }
    setRowVisibiliryObserver() {
        this.visibilityObserver = new IntersectionObserver((entries) => {
            if (entries[0].isIntersecting === true) {
                let shiftIndex = entries[0].target.getAttribute('data-shiftIndex');
                // console.log('Observer become visible in screen.');
                // console.log('Shift Index: ' + shiftIndex);
                let pageInfo = this.ShiftPaginationInfo[shiftIndex];
                if (pageInfo.NextRowIndex < pageInfo.RowCount - 1) {
                    let shiftPageInfo = this.ShiftPaginationInfo[pageInfo.ShiftIndex];
                    let endNRowIndex = shiftPageInfo.NextRowIndex + Declarations.ShiftVisibleRows;
                    let nextNrows = this.Shifts[pageInfo.ShiftIndex].Schedules.slice(shiftPageInfo.NextRowIndex, endNRowIndex);
                    if (nextNrows.length > 0) {
                        this.setShiftRows(pageInfo.ShiftIndex, nextNrows);
                    }
                }
            }
            else {
                //console.log('Observer become invisible in screen.');
            }
        }, { threshold: [0, 0.5, 1] });
        window.setTimeout(() => {
            for (var idx = 0; idx < this.ShiftPaginationInfo.length; idx++) {
                this.visibilityObserver.unobserve(document.querySelector('#visibilityObserver' + idx));
            }
            for (var idx = 0; idx < this.ShiftPaginationInfo.length; idx++) {
                this.visibilityObserver.observe(document.querySelector('#visibilityObserver' + idx));
            }
        }, 500);
    }
    getSchedulesFormArray(schedules) {
        var _sArray = this.fb.array([]);
        schedules.forEach(x => {
            _sArray.push(this.fb.group({
                AllowDriverChange: this.fb.control(x.AllowDriverChange),
                Drivers: this.utilService.getDriversFormArray(x.Drivers),
                Trailers: this.utilService.getTrailersFormArray(x.Trailers),
                Trips: this.utilService.getTripsFormArray(x.Trips),
                LoadQueueId: this.fb.control(x.LoadQueueId),
                IsLoadQueueCollapsed: this.fb.control(x.IsLoadQueueCollapsed),
                IsColumnSelected: this.fb.control(x.IsColumnSelected),
                IsLoadQueueColumnBlocked: this.fb.control(x.IsLoadQueueColumnBlocked),
                IsIncludeAllRegionDriver: this.fb.control(x.IsIncludeAllRegionDriver),
                LoadQueueColumnStatus: this.fb.control(x.LoadQueueColumnStatus),
                LoadQueueFilterVisibility: this.fb.control(true),
                IsDriverScheduleExists: this.fb.control(x.IsDriverScheduleExists),
            }));
        });
        return _sArray;
    }
    setShiftRows(shiftIndex, schedules) {
        var _sArray = this.getSchedulesFormArray(schedules);
        var _shift = this.SbForm.controls['Shifts']['controls'][shiftIndex];
        let _schedules = _shift.controls['Schedules'];
        _sArray.controls.forEach(x => _schedules.push(x));
        let shiftPageInfo = this.ShiftPaginationInfo[shiftIndex];
        shiftPageInfo.NextRowIndex += schedules.length;
        this.changeDetectorRef.markForCheck();
    }
    initShiftPageInfo(shifts) {
        this.ShiftPaginationInfo = [];
        for (var idx = 0; idx < shifts.length; idx++) {
            var shiftPageInfo = {
                ShiftIndex: idx,
                RowCount: shifts[idx].Schedules.length,
                NextRowIndex: 0
            };
            this.ShiftPaginationInfo.push(shiftPageInfo);
        }
    }
    resetDriverTrailerForm() {
        this.SelectedTrailers = [];
        this.UnassignedTrailers = [];
        if (this.DriverTrailerForm) {
            this.DriverTrailerForm.reset();
        }
    }
    validateTrailerJobCompatibility(drData, schedule) {
        var _deliveryRequests = drData.Data.value;
        let _selectedTrailers = schedule.controls["Trailers"].value;
        var trips = schedule.controls['Trips'];
        if ((_selectedTrailers && _selectedTrailers.length > 0) && (_deliveryRequests && _deliveryRequests.length > 0)) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                    this.highLightDRDiv(trips, data);
                    Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                }
                else {
                    this.highLightDRDiv(trips, null);
                }
            });
        }
    }
    highLightDRDiv(trips, data) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i];
            var deliveryRequests = trip.controls["DeliveryRequests"];
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j];
                    deliveryRequest.controls["IsNotCompatible"].patchValue(false);
                    deliveryRequest.controls["IsBlinkDR"].patchValue(false);
                    if (data) {
                        if (data.deliveryRequestsNotCompatible.find(t => t.Id == deliveryRequest.controls["Id"].value)) {
                            deliveryRequest.controls["IsNotCompatible"].patchValue(true);
                            deliveryRequest.controls["IsBlinkDR"].patchValue(true);
                            setTimeout(() => {
                                this.removeClassForAllLoad(trips);
                            }, 10000);
                        }
                    }
                }
            }
        }
        this.changeDetectorRef.detectChanges();
    }
    highLightDRDivOneLoad(trip, data) {
        var deliveryRequests = trip.get("DeliveryRequests");
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j];
                deliveryRequest.get("IsNotCompatible").patchValue(false);
                deliveryRequest.get("IsBlinkDR").patchValue(false);
                if (data && data.deliveryRequestsNotCompatible) {
                    if (data.deliveryRequestsNotCompatible.find(t => t.Id == deliveryRequest.get("Id").value)) {
                        deliveryRequest.get("IsNotCompatible").patchValue(true);
                        deliveryRequest.get("IsBlinkDR").patchValue(true);
                        setTimeout(() => {
                            this.removeClassForLoad(trip);
                        }, 10000);
                    }
                }
            }
        }
        this.changeDetectorRef.detectChanges();
    }
    removeClassForAllLoad(trips) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i];
            var deliveryRequests = trip.get("DeliveryRequests");
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j];
                    deliveryRequest.get("IsBlinkDR").patchValue(false);
                }
            }
        }
    }
    removeClassForLoad(trip) {
        var deliveryRequests = trip.get("DeliveryRequests");
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j];
                deliveryRequest.get("IsBlinkDR").patchValue(false);
            }
        }
    }
    IsDRCompatible(trip) {
        var isDRCompatible = true;
        var deliveryRequests = trip.get("DeliveryRequests");
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j];
                if (deliveryRequest && deliveryRequest.get("IsNotCompatible").value) {
                    isDRCompatible = false;
                }
            }
        }
        return isDRCompatible;
    }
    onItemDrop(trip, event, shiftIndex, rowIndex, colIndex, schedule) {
        let drData = event.dragData;
        let dragToLoad = false;
        if (drData && drData.Data && drData.Data.length == 0) {
            return; //There is no dr in dragged data, so don't take any action.
        }
        if (drData.TripFrom) {
            if (drData.TripFrom.controls['DriverRowIndex'].value == trip.controls['DriverRowIndex'].value && drData.TripFrom.controls['DriverColIndex'].value == trip.controls['DriverColIndex'].value && drData.TripFrom.controls['ShiftIndex'].value == trip.controls['ShiftIndex'].value)
                return;
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        let drDataCopied = JSON.parse(JSON.stringify(drData.Data));
        if (drData.TripFrom) {
            let isCommonPickup = trip.get('IsCommonPickup').value;
            let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value;
            if ((drData.TripFrom.controls['DriverRowIndex'].value != trip.controls['DriverRowIndex'].value) || (drData.TripFrom.controls['ShiftIndex'].value == trip.controls['ShiftIndex'].value)) {
                drData.Data = this.utilService.getDeliveryRequestFormArray(drData.Data, isCommonPickup, IsDispatcherDragDropSequence, 1, drData.TripFrom);
            }
            else {
                drData.Data = this.utilService.getDeliveryRequestFormArray(drData.Data, isCommonPickup, IsDispatcherDragDropSequence, 0, drData.TripFrom);
            }
            this.draggedDelReqData = event.dragData;
            this.droppedTrip = { Trip: trip, ShiftIndex: shiftIndex, RowIndex: rowIndex, ColIndex: colIndex, Schedule: schedule };
            if (this.isDraggedDRsPublished(drData.Data.value) || trip.controls['DeliveryGroupPrevStatus'].value == 2) {
                var scheduleIds = this.getPublishedDRsTrackableScheduleIds(drData.Data.value);
                if (scheduleIds.length > 0) {
                    this.dragDelReqToAnotherLoad(scheduleIds);
                }
                else {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    jQuery('#btnconfirm-dragdrop-dr').click();
                }
            }
            else {
                dragToLoad = true;
            }
        }
        else {
            dragToLoad = true;
        }
        if (dragToLoad == true) {
            this._savingBuilder = true;
            this.createLoad({
                RegionId: this.SelectedRegionId,
                Customer: drDataCopied[0].CustomerCompany,
                JobId: drDataCopied[0].JobId,
                JobName: drDataCopied[0].JobName,
                Drs: drDataCopied,
                ShiftIndex: shiftIndex,
                RowIndex: rowIndex,
                ColIndex: colIndex,
                Schedule: schedule,
                Trip: trip,
                DrData: drData,
                IsDragFromLoad: drData.TripFrom != null
            });
        }
        this.dataService.setDeliveryPreloadOption({ ShiftIndex: shiftIndex, ScheduleIndex: colIndex, Reset: true });
    }
    isDraggedDRsPublished(delRequests) {
        return delRequests.filter(t => t.PreviousStatus == 3).length > 0;
    }
    getPublishedDRsTrackableScheduleIds(delRequests) {
        return delRequests.filter(t => t.PreviousStatus == 3).map(t => t.TrackableScheduleId);
    }
    dragDelReqToAnotherLoad(scheduleIds) {
        this._savingBuilder = true;
        this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
            this._savingBuilder = false;
            if (response != null && response != undefined && response.length > 0) {
                var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                var isCompletedSchedule = completedSchedules.length > 0;
                var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                    Declarations.msgerror("Can't delete from this load as drop has been made already", 'Warning', 2500);
                    this.droppedTrip = null;
                    this.draggedDelReqData = null;
                    return;
                }
                else if (onTheWaySchedules.length > 0) {
                    this.sbService.setConfirmationHeadingForDR(onTheWaySchedules[0]);
                    jQuery('#btnconfirm-dragdrop-dr').click();
                    return;
                }
                else {
                    if (this.draggedDelReqData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || this.draggedDelReqData.get('DeliveryGroupPrevStatus').value == 2) {
                        jQuery('#btnconfirm-dragdrop-dr').click();
                    }
                    else {
                        this.dragDelReqToAnotherLoadYes();
                    }
                }
            }
            else {
                if (this.draggedDelReqData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || this.draggedDelReqData.get('DeliveryGroupPrevStatus').value == 2) {
                    jQuery('#btnconfirm-dragdrop-dr').click();
                }
                else {
                    this.dragDelReqToAnotherLoadYes();
                }
            }
            this.changeDetectorRef.detectChanges();
        });
    }
    DropDelReqToLoad(droppedToSchedule, droppedToTrip, drData, drsToRemove) {
        var isDraggedFromLoad = drData.TripFrom != undefined && drData.TripFrom != null;
        let isCommonPickup = droppedToTrip.controls['IsCommonPickup'].value;
        var locationType = droppedToTrip.controls['PickupLocationType'].value;
        var pickupLocation = null;
        if (isCommonPickup) {
            if (locationType != 2)
                pickupLocation = droppedToTrip.controls['Terminal'].value;
            else
                pickupLocation = droppedToTrip.controls['BulkPlant'].value;
        }
        var _drArray = droppedToTrip.controls['DeliveryRequests'];
        var _drFormArray = drData.Data;
        _drFormArray.controls.forEach((_drForm) => {
            if (isCommonPickup) {
                if (isDraggedFromLoad) {
                    if (!(locationType == 2 && _drForm.value.BulkPlant && _drForm.value.BulkPlant.SiteName == droppedToTrip.value.BulkPlant.SiteName)) {
                        isCommonPickup = false;
                    }
                    if (!(locationType != 2 && _drForm.value.Terminal && _drForm.value.Terminal.Id == droppedToTrip.value.Terminal.Id)) {
                        isCommonPickup = false;
                    }
                    if (!isCommonPickup) {
                        droppedToTrip.controls['IsCommonPickup'].setValue(false);
                        droppedToTrip.controls['Terminal'].disable();
                        droppedToTrip.controls['BulkPlant'].disable();
                        _drArray.controls.forEach((_toDrForm) => {
                            if (locationType != 2) {
                                _toDrForm.controls['Terminal'].patchValue(pickupLocation);
                                _toDrForm.controls['PickupLocationType'].patchValue(1);
                            }
                            else {
                                _toDrForm.controls['BulkPlant'].patchValue(pickupLocation);
                                _toDrForm.controls['PickupLocationType'].patchValue(2);
                            }
                        });
                    }
                }
            }
        });
        _drFormArray.controls.forEach((_drForm) => {
            if (isCommonPickup) {
                _drForm.controls['Terminal'].disable();
                _drForm.controls['BulkPlant'].disable();
            }
            else {
                var drValue = _drForm.value;
                var isDRPickupExists = (drValue.PickupLocationType != 2 && drValue.Terminal && drValue.Terminal.Id > 0) || (drValue.PickupLocationType == 2 && drValue.BulkPlant && drValue.BulkPlant.SiteName && drValue.BulkPlant.SiteName.length > 0);
                if (pickupLocation) {
                    if (!isDRPickupExists) {
                        if (locationType != 2) {
                            _drForm.controls['Terminal'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(1);
                        }
                        else {
                            _drForm.controls['BulkPlant'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(2);
                        }
                    }
                }
            }
            if (!isDraggedFromLoad) {
                _drForm.controls['ScheduleStatus'].setValue(14);
                _drForm.controls['Status'].setValue(5);
            }
            _drArray.push(_drForm);
        });
        if (isDraggedFromLoad) {
            var drInFromTrip = drData.TripFrom.controls['DeliveryRequests'];
            for (let idx = 0; idx < _drFormArray.controls.length; idx++) {
                let drIndex = drInFromTrip.controls.findIndex((x) => {
                    return x.controls['Id'].value == _drFormArray.controls[idx]['controls'].Id.value;
                });
                _drFormArray.controls[idx].get('SourceTripId').setValue(drData.TripFrom.controls['TripId'].value);
                drInFromTrip.removeAt(drIndex);
            }
            this.dragDropDeliveryRequestsSave(drData.TripFrom, droppedToTrip, drData.Schedule, droppedToSchedule);
        }
        else {
            this.dataService.setRemoveDroppedRequestSubject(drsToRemove);
        }
        this.validateTrailerJobCompatibility(drData, droppedToSchedule);
        this._savingBuilder = false;
        this.changeDetectorRef.markForCheck();
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    dragDelReqToAnotherLoadYes() {
        this._savingBuilder = true;
        let isCommonPickup = this.droppedTrip.Trip.controls['IsCommonPickup'].value;
        var locationType = this.droppedTrip.Trip.controls['PickupLocationType'].value;
        var pickupLocation = null;
        if (isCommonPickup) {
            if (locationType != 2)
                pickupLocation = this.droppedTrip.Trip.controls['Terminal'].value;
            else
                pickupLocation = this.droppedTrip.Trip.controls['BulkPlant'].value;
        }
        var _drArray = this.droppedTrip.Trip.controls['DeliveryRequests'];
        let isDraggedFromLoad = this.draggedDelReqData != null && this.draggedDelReqData.TripFrom != undefined && this.draggedDelReqData.TripFrom != null;
        var _drFormArray = this.draggedDelReqData.Data;
        _drFormArray.controls.forEach((_drForm) => {
            if (isCommonPickup) {
                if (isDraggedFromLoad) {
                    if (!(locationType == 2 && _drForm.value.BulkPlant && _drForm.value.BulkPlant.SiteName == this.droppedTrip.Trip.value.BulkPlant.SiteName)) {
                        isCommonPickup = false;
                    }
                    if (!(locationType != 2 && _drForm.value.Terminal && _drForm.value.Terminal.Id == this.droppedTrip.Trip.value.Terminal.Id)) {
                        isCommonPickup = false;
                    }
                    if (!isCommonPickup) {
                        this.droppedTrip.Trip.controls['IsCommonPickup'].setValue(false);
                        this.droppedTrip.Trip.controls['Terminal'].disable();
                        this.droppedTrip.Trip.controls['BulkPlant'].disable();
                        _drArray.controls.forEach((_toDrForm) => {
                            if (locationType != 2) {
                                _toDrForm.controls['Terminal'].patchValue(pickupLocation);
                                _toDrForm.controls['PickupLocationType'].patchValue(1);
                            }
                            else {
                                _toDrForm.controls['BulkPlant'].patchValue(pickupLocation);
                                _toDrForm.controls['PickupLocationType'].patchValue(2);
                            }
                        });
                    }
                }
            }
        });
        _drFormArray.controls.forEach((_drForm) => {
            if (isCommonPickup) {
                _drForm.controls['Terminal'].disable();
                _drForm.controls['BulkPlant'].disable();
            }
            else {
                var drValue = _drForm.value;
                var isDRPickupExists = (drValue.PickupLocationType != 2 && drValue.Terminal && drValue.Terminal.Id > 0) || (drValue.PickupLocationType == 2 && drValue.BulkPlant && drValue.BulkPlant.SiteName && drValue.BulkPlant.SiteName.length > 0);
                if (pickupLocation) {
                    if (!isDRPickupExists) {
                        if (locationType != 2) {
                            _drForm.controls['Terminal'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(1);
                        }
                        else {
                            _drForm.controls['BulkPlant'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(2);
                        }
                    }
                }
            }
            _drArray.push(_drForm);
        });
        if (isDraggedFromLoad) {
            var drInFromTrip = this.draggedDelReqData.TripFrom.controls['DeliveryRequests'];
            for (let idx = 0; idx < _drFormArray.controls.length; idx++) {
                let drIndex = drInFromTrip.controls.findIndex((x) => {
                    return x.controls['Id'].value == _drFormArray.controls[idx]['controls'].Id.value;
                });
                _drFormArray.controls[idx].get('SourceTripId').setValue(this.draggedDelReqData.TripFrom.controls['TripId'].value);
                drInFromTrip.removeAt(drIndex);
            }
        }
        this._savingBuilder = false;
        this.validateTrailerJobCompatibility(this.draggedDelReqData, this.draggedDelReqData.Schedule);
        this.draggedDelReqData.TripFrom.controls['ShiftIndex'].setValue(this.draggedDelReqData.ShiftIndex);
        this.droppedTrip.Trip.controls['ShiftIndex'].setValue(this.droppedTrip.ShiftIndex);
        this.dragDropDeliveryRequestsPublish(this.draggedDelReqData.TripFrom, this.droppedTrip.Trip, this.draggedDelReqData.Schedule, this.droppedTrip.Schedule);
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    dragDropDeliveryRequestsSave(tripFrom, tripTo, tripFromSchedule, tripToSchedule) {
        this._savingBuilder = true;
        if (tripFrom.controls['DeliveryRequests'].value.length == 0) {
            tripFrom.controls['IsCommonPickup'].setValue(false);
            tripFrom.controls['Terminal'].disable();
            tripFrom.controls['BulkPlant'].disable();
        }
        this.changeDetectorRef.detectChanges();
        var sourceTrip = tripFrom.value;
        var destTrip = tripTo.value;
        this.setTripStatus(sourceTrip);
        this.setDeliveryGroupStatus(sourceTrip, DeliveryGroupStatus.Draft);
        this.setTripStatus(destTrip);
        this.setDeliveryGroupStatus(destTrip, DeliveryGroupStatus.Draft);
        var dataToSave = this.getDSBSaveModel();
        sourceTrip.Drivers = tripFromSchedule.get('Drivers').value;
        sourceTrip.Trailers = tripFromSchedule.get('Trailers').value;
        this.setDeliveryRequestStatusAsDraft(sourceTrip.DeliveryRequests);
        dataToSave.Trips.push(sourceTrip);
        destTrip.Drivers = tripToSchedule.get('Drivers').value;
        destTrip.Trailers = tripToSchedule.get('Trailers').value;
        this.setDeliveryRequestStatusAsDraft(destTrip.DeliveryRequests);
        dataToSave.Trips.push(destTrip);
        dataToSave.Status = 5;
        this.dataService.setShowDeliveryGroupSubject(false);
        this.sbService.saveDriverView(dataToSave).subscribe(data => {
            this._savingBuilder = false;
            var trips = [];
            trips.push(tripFrom);
            trips.push(tripTo);
            this.updateLoadForm(data, trips);
        });
    }
    dragDropDeliveryRequestsPublish(tripFrom, tripTo, tripFromSchedule, tripToSchedule) {
        var isDraggedDRsPublished = this.isDraggedDRsPublished(this.draggedDelReqData.Data.value), isDestTripPublished = false;
        if (tripFrom != null && tripFrom != undefined) {
            if (tripFrom.get('GroupId').value > 0) {
                if (tripFrom && tripFrom.invalid) {
                    let invalidctrls = CustomAbstractControls.findRecursiveErrors(tripFrom);
                    var i = tripFrom.get('ShiftIndex').value;
                    var j = tripFrom.get('DriverRowIndex').value;
                    var k = tripFrom.get('DriverColIndex').value;
                    if (tripFrom.get('DeliveryRequests').value.length > 0) {
                        this.editExisingGroup(tripFrom, i, j, k, tripFromSchedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        return;
                    }
                    else {
                        tripFrom.controls['IsCommonPickup'].setValue(false);
                        tripFrom.controls['Terminal'].disable();
                        tripFrom.controls['BulkPlant'].disable();
                    }
                }
                else {
                    this.dataService.setShowDeliveryGroupSubject(false);
                }
            }
        }
        if (tripTo != null && tripTo != undefined) {
            if (tripTo.get('GroupId').value > 0) {
                isDestTripPublished = true;
                if (tripTo.invalid) {
                    var i = tripTo.get('ShiftIndex').value;
                    var j = tripTo.get('DriverRowIndex').value;
                    var k = tripTo.get('DriverColIndex').value;
                    let invalidctrls = CustomAbstractControls.findRecursiveErrors(tripTo);
                    if (tripTo.get('DeliveryRequests').value.length > 0) {
                        this.editExisingGroup(tripTo, i, j, k, tripToSchedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        return;
                    }
                }
                else {
                    this.dataService.setShowDeliveryGroupSubject(false);
                }
            }
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var sourceTripValue = tripFrom.value;
        var destTripValue = tripTo.value;
        if (sourceTripValue.GroupId > 0) {
            this.setTripStatus(sourceTripValue);
            this.setDeliveryGroupStatus(sourceTripValue, DeliveryGroupStatus.Published);
        }
        else {
            this.setTripStatus(sourceTripValue);
            this.setDeliveryGroupStatus(sourceTripValue, DeliveryGroupStatus.Draft);
        }
        sourceTripValue.Drivers = tripFromSchedule.get('Drivers').value;
        sourceTripValue.Trailers = tripFromSchedule.get('Trailers').value;
        if (destTripValue.GroupId > 0) {
            this.setTripStatus(destTripValue);
            this.setDeliveryGroupStatus(destTripValue, DeliveryGroupStatus.Published);
        }
        else {
            this.setTripStatus(destTripValue);
            this.setDeliveryGroupStatus(destTripValue, DeliveryGroupStatus.Draft);
        }
        destTripValue.Drivers = tripToSchedule.get('Drivers').value;
        destTripValue.Trailers = tripToSchedule.get('Trailers').value;
        if (isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus != 2) {
            this.setDeliveryRequestStatusAsDraft(destTripValue.DeliveryRequests);
        }
        else if (!isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus == 2) {
            this.setDeliveryRequestStatusAsPublished(this.draggedDelReqData.Data.value, destTripValue.DeliveryRequests);
        }
        else if (isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus == 2) {
            this.setDeliveryRequestStatusAsPublished(this.draggedDelReqData.Data.value, destTripValue.DeliveryRequests);
        }
        dsbModel.Trips.push(sourceTripValue);
        dsbModel.Trips.push(destTripValue);
        dsbModel.Status = 5;
        var tripArray = [];
        tripArray.push(tripFrom);
        tripArray.push(tripTo);
        if (isDraggedDRsPublished || isDestTripPublished) {
            this.sbService.publishDriverView(dsbModel).subscribe(data => {
                this._savingBuilder = false;
                this.updateLoadForm(data, tripArray);
            });
        }
        else {
            this.sbService.saveDriverView(dsbModel).subscribe(data => {
                this._savingBuilder = false;
                this.updateLoadForm(data, tripArray);
            });
        }
    }
    setDeliveryRequestStatusAsDraft(deliveryRequests) {
        deliveryRequests.forEach(t => {
            t.Status = 5;
            t.ScheduleStatus = 14;
            t.DeliveryGroupId = null;
            t.DeliveryScheduleId = null;
            t.TrackableScheduleId = null;
            t.TrackScheduleStatus = 0;
            t.TrackScheduleEnrouteStatus = 0;
            t.TrackScheduleStatusName = '';
        });
    }
    setDeliveryRequestStatusAsPublished(draggedDeliveryRequests, destTripDeliveryRequests) {
        for (var i = 0; i < draggedDeliveryRequests.length; i++) {
            var destLoadDR = destTripDeliveryRequests.filter(t => t.Id == draggedDeliveryRequests[i].Id);
            destLoadDR.forEach(t => {
                t.Status = 3;
                t.ScheduleStatus = 14;
            });
        }
    }
    dragDelReqToAnotherLoadNo() {
        this.droppedTrip = null;
        this.draggedDelReqData = null;
    }
    resetLoad(trip) {
        if (trip) {
            let reserveValue = {
                TripId: trip.controls['TripId'].value,
                ShiftId: trip.controls['ShiftId'].value,
                ShiftStartTime: trip.controls['ShiftStartTime'].value,
                ShiftEndTime: trip.controls['ShiftEndTime'].value,
                SlotPeriod: trip.controls['SlotPeriod'].value,
                StartDate: trip.controls['StartDate'].value,
                StartTime: trip.controls['StartTime'].value,
                EndTime: trip.controls['EndTime'].value,
                Carrier: trip.controls['Carrier'].value,
                ShiftIndex: trip.controls['ShiftIndex'].value,
                DriverRowIndex: trip.controls['DriverRowIndex'].value,
                DriverColIndex: trip.controls['DriverColIndex'].value,
                TrailerRowIndex: trip.controls['TrailerRowIndex'].value,
                TrailerColIndex: trip.controls['TrailerColIndex'].value,
                IsEditable: true,
                IsPreloadDisable: false
            };
            trip.reset();
            trip.controls['DeliveryRequests'].clear();
            trip.reset(reserveValue);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }
    deleteGroup(data) {
        this.dataService.setShowDeliveryGroupSubject(false);
        if (data.get('GroupId').value != null && data.get('GroupId').value > 0) {
            this._savingBuilder = true;
            this.changeDetectorRef.detectChanges();
            var delRequests = data.get('DeliveryRequests');
            var scheduleIds = delRequests.value.map(t => t.TrackableScheduleId);
            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                this._savingBuilder = false;
                if (response != null && response != undefined && response.length > 0) {
                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                    var isCompletedSchedule = completedSchedules.length > 0;
                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                        Declarations.msgerror("Can't delete/reset group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                        return;
                    }
                    else if (onTheWaySchedules.length > 0) {
                        this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                        this.DeletedGroup = data;
                        jQuery('#btnconfirm-delete-delgrp').click();
                        return;
                    }
                    else {
                        this.deleteLoad(data);
                    }
                }
                else {
                    this.deleteLoad(data);
                }
                //this.changeDetectorRef.detectChanges();
            });
        }
        else {
            this.deleteLoad(data);
        }
    }
    deleteLoadYes() {
        Declarations.hideModal('#confirm-delete-delgrp');
        this.deleteLoad(this.DeletedGroup);
        this.DeletedGroup = null;
    }
    deleteLoadNo() {
        Declarations.hideModal('#confirm-delete-delgrp');
        this.DeletedGroup = null;
    }
    deleteLoad(trip) {
        var delRequests = trip.controls['DeliveryRequests'];
        delRequests.value.forEach(t => { t.WindowMode = 1; t.QueueMode = 1; t.Compartments = []; });
        var tripId = trip.controls['TripId'].value;
        if (tripId != null && tripId != undefined && tripId != '') {
            var deliveryRequestsValue = delRequests.value;
            var dsbModel = this.getDSBSaveModel();
            dsbModel.DeletedGroupId = trip.controls['GroupId'].value;
            dsbModel.DeletedTripId = tripId;
            let preloadInfo = this.getPreloadedInfoFromLoad(trip);
            let preloadedTrips = this.getPrePostLoadedTrips(preloadInfo);
            let postloadInfo = this.getPostloadedInfoFromLoad(trip);
            let postloadedTrips = this.getPrePostLoadedTrips(postloadInfo);
            dsbModel.PreloadedDRs = this.getPreloadAcrossTheDateInfoFromLoad(trip);
            dsbModel.PostloadedDRs = this.getPostloadAcrossTheDateInfoFromLoad(trip);
            delRequests.clear();
            var schedule = this.SbForm.get('Shifts.' + trip.controls['ShiftIndex'].value + '.Schedules.' + trip.controls['DriverRowIndex'].value);
            trip.value.Drivers = schedule.controls['Drivers'].value;
            trip.value.Trailers = schedule.controls['Trailers'].value;
            dsbModel.Trips.push(trip.value);
            dsbModel.Status = 4;
            let tripArray = [];
            tripArray.push(trip);
            preloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });
            postloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });
            this._savingBuilder = true;
            this.sbService.deleteGroup(dsbModel).subscribe(response => {
                if (response != null) {
                    if (response.StatusCode == 0 || response.StatusCode == 2) {
                        let drsToRestore = deliveryRequestsValue.filter(t => !t.PostLoadedFor);
                        this.dataService.setRestoreDeletedRequestSubject(drsToRestore);
                        this.resetLoad(trip);
                    }
                    this.updateLoadForm(response, tripArray);
                }
                else {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                this._savingBuilder = false;
            });
        }
        else {
            this.dataService.setRestoreDeletedRequestSubject(delRequests.value);
            this.resetLoad(trip);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }
    getPreloadedInfoFromLoad(trip) {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => x.PreLoadInfo).map(m => m.PreLoadInfo);
        return preloadInfo;
    }
    getPostloadedInfoFromLoad(trip) {
        let _drs = trip.controls['DeliveryRequests'].value;
        let postloadInfo = _drs.filter(x => x.PostLoadInfo).map(m => m.PostLoadInfo);
        return postloadInfo;
    }
    getPreloadAcrossTheDateInfoFromLoad(trip) {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => !x.PostLoadInfo && x.PostLoadedFor && !x.IsRetainFuelLoaded).map(m => m.PostLoadedFor);
        preloadInfo = preloadInfo.filter((value, index, self) => self.indexOf(value) === index);
        return preloadInfo;
    }
    getPostloadAcrossTheDateInfoFromLoad(trip) {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => !x.PreLoadInfo && x.PreLoadedFor).map(m => m.PreLoadedFor);
        return preloadInfo;
    }
    getPrePostLoadedTrips(loadInfo) {
        let loadedTrips = [];
        loadInfo.forEach(x => {
            let thisShift = this.SbForm.controls['Shifts']['controls'].find((f) => f.controls['Id'].value == x.ShiftId);
            if (thisShift) {
                let thisSchedule = thisShift.get('Schedules.' + x.ScheduleIndex);
                let thisTrip = thisSchedule.get("Trips." + x.TripIndex);
                if (thisTrip) {
                    thisTrip.value.Drivers = thisSchedule.controls['Drivers'].value;
                    thisTrip.value.Trailers = thisSchedule.controls['Trailers'].value;
                    loadedTrips.push(thisTrip);
                }
            }
        });
        loadedTrips = loadedTrips.filter((value, index, self) => self.indexOf(value) === index);
        return loadedTrips;
    }
    getAssignedDrivers(shiftIdx) {
        var _drivers = [];
        var _shift = this.SbForm.controls['Shifts']['controls'][shiftIdx];
        if (_shift != null && _shift != undefined) {
            var _schArray = _shift.controls['Schedules'];
            _schArray.controls.forEach((sc) => {
                var _dArray = sc.controls['Drivers'];
                if (_dArray && _dArray.length > 0) {
                    _drivers = _drivers.concat(_dArray.value);
                }
            });
        }
        return _drivers;
    }
    getAssignedTrailers(shiftIdx) {
        var _trailers = [];
        var _shift = this.SbForm.get('Shifts.' + shiftIdx);
        if (_shift != null && _shift != undefined) {
            var _schArray = _shift.get('Schedules');
            _schArray.controls.forEach(sc => {
                var _tArray = sc.get('Trailers');
                if (_tArray && _tArray.length > 0) {
                    _trailers = _trailers.concat(_tArray.value);
                }
            });
        }
        return _trailers;
    }
    getUnassignedDrivers(isFilldToggle = false) {
        let currentDriverId = this.DriverTrailerForm.controls['Driver'].value;
        let drivers = this.AllUnassignedDrivers;
        if (drivers && drivers.length > 0) {
            this._loadingDrivers = true;
            let shiftIdx = this.DriverTrailerForm.controls['Index1'].value;
            let assigned = this.getAssignedDrivers(shiftIdx);
            if (!isFilldToggle) {
                assigned = assigned.filter(x => x.Id != currentDriverId);
                drivers = drivers.filter(this.IdNotInComparer(assigned));
            }
            let isFilldCompatible = this.DriverTrailerForm.get('IsFilldCompatible').value;
            if (isFilldCompatible != undefined) {
                drivers = drivers.filter(t => t.IsFilldCompatible == isFilldCompatible);
            }
            this._loadingDrivers = false;
        }
        return drivers;
    }
    getUnassignedTrailers(shiftIdx, currentTrailers, IsFilldToggle = false) {
        let _trailers = this.RegionDetail.Trailers;
        if (!IsFilldToggle) {
            let assignedTrailers = this.getAssignedTrailers(shiftIdx);
            assignedTrailers = assignedTrailers.filter(this.IdNotInComparer(currentTrailers));
            _trailers = _trailers.filter(this.IdNotInComparer(assignedTrailers));
        }
        let isFilldCompatible = this.DriverTrailerForm.get('IsFilldCompatible').value;
        if (isFilldCompatible != undefined) {
            _trailers = _trailers.filter(t => t.IsFilldCompatible == isFilldCompatible);
        }
        return _trailers;
    }
    editDriverTrailers(data) {
        //this.changeDetectorRef.detach();
        this.EditDriverData = data;
        var driverId = data.Driver;
        var trailers = data.Trailers;
        this.DriverTrailerForm.patchValue({
            Index1: data.Index1,
            Index2: data.Index2,
            Driver: driverId,
            Trailers: trailers
        });
        this.SelectedTrailers = trailers;
        if (this.SelectedTrailers.length > 0) {
            let trailer1 = trailers[0];
            this.DriverTrailerForm.get('IsFilldCompatible').patchValue(trailer1.IsFilldCompatible);
        }
        this.selTrailerIndex = data.Index1;
        this.selTrailerlist = trailers;
        this.UnassignedTrailers = this.getUnassignedTrailers(data.Index1, trailers);
        if (trailers.length > 0 || !this.IsTrailerExists) {
            this._loadingDrivers = true;
            this.getDriverdetails();
            this.changeDetectorRef.detectChanges();
        }
        //if (this.RegionDetail.Trailers && this.RegionDetail.Trailers.length > 0) {
        //    this.changeDetectorRef.reattach();
        //}
        if (!this.IsTrailerExists) {
            this.DriverTrailerForm.controls['Trailers'].clearValidators();
            this.DriverTrailerForm.controls['Trailers'].updateValueAndValidity();
        }
        else {
            this.DriverTrailerForm.controls['Trailers'].setValidators([Validators.required]);
        }
        // this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        this._publishedRequestExists = false;
        //this.subscribeFormChange();
    }
    checkOtherRegionDriver() {
        if (this.DriverTrailerForm.get('Driver').value == "null") {
            this.DriverTrailerForm.get('Driver').setErrors({ 'incorrect': true });
        }
        this.DriverTrailerForm.markAllAsTouched();
        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var status = this.validatePrePostLoadTrailer(_form);
            if (status) {
                var shiftId = this.SbForm.get('Shifts.' + _form.Index1 + '.Id').value;
                //newly added
                var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
                this.dataService.IsLoadingButtonSubject.next(true);
                this.sbService.getSelectedDateDriverScheduleByDriverId(_form.Driver, new Date(this.SbForm.controls.Date.value).toUTCString()).subscribe(data => {
                    if (data && data.filter(f => f.ShiftId == shiftId).length > 0) {
                        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '');
                        //this.checkForPublishedRequests(schedule);
                        if (!this.EditDriverData.IsPublishedRequestsExists) {
                            this.assignDriverToSchedule(driverObj, schedule, _form);
                        }
                    }
                    else {
                        this.SelectedDriverName = driverObj.Name;
                        this._otherRegionDriverSubject.next(true);
                    }
                    this.dataService.IsLoadingButtonSubject.next(false);
                });
            }
        }
    }
    checkForPublishedRequests(schedule) {
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        if (this.checkAnyPublishedTrip(schedule)) {
            this._publishedRequestExists = true;
        }
        else {
            this._publishedRequestExists = false;
        }
    }
    editDriverCancel() {
        if (this.driverSchedules) {
            this.driverSchedules.forEach(t => t.unsubscribeUpdateDriverTrailerSubject());
        }
    }
    onPublishChangesNo() {
        this._publishedRequestExists = false;
    }
    onPublishChangesYes() {
        var _form = this.DriverTrailerForm.value;
        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '');
        if (schedule != null && schedule != undefined) {
            this.EditDriverData.IsPublishedRequestsExists = false;
            var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
            this.assignDriverToSchedule(driverObj, schedule, _form);
        }
    }
    onOtherRegionDriverNo() {
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
    }
    onOtherRegionDriverYes() {
        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '');
            if (schedule != null && schedule != undefined) {
                var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
                //this.checkForPublishedRequests(schedule);
                this._otherRegionDriverSubject.next(false);
                this._publishedRequestExists = this.EditDriverData.IsPublishedRequestsExists;
                if (!this.EditDriverData.IsPublishedRequestsExists) {
                    this.assignDriverToSchedule(driverObj, schedule, _form);
                    let driverInfo = {
                        driverId: driverObj.Id,
                        regionId: this.SelectedRegionId
                    };
                    this.chatService.intializeChatDefault(driverInfo);
                }
            }
        }
    }
    GetAllLoadsDR(trips) {
        var _deliveryRequests = [];
        if (trips) {
            for (var i = 0; i < trips.length; i++) {
                var trip = trips.controls[i];
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
    allowDriverChange(data, driverObj) {
        var isAllow = true;
        if (data && data.deliverySchedules && data.deliverySchedules.length > 0) {
            if (driverObj) {
                if (data.deliverySchedules.find(driverId => driverObj.Id != driverId)) {
                    isAllow = false;
                }
            }
        }
        return isAllow;
    }
    assignDriverToSchedule(driverObj, schedule, _form) {
        var trips = schedule.controls['Trips'];
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return _form.Trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        // Validate Delivery Requests with trailer type
        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                    this.highLightDRDiv(trips, data);
                    Declarations.hideModal('#driverTrailerModel');
                    Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                    this.editDriverCancel();
                }
                else if (!this.allowDriverChange(data, driverObj)) {
                    Declarations.msgerror("The user cannot change the driver once drop is done", undefined, undefined);
                    this.editDriverCancel();
                }
                else {
                    this.assignDriverToScheduleSave(driverObj, schedule, _form);
                    this.highLightDRDiv(trips, null);
                }
                if (_selectedTrailers.length > 0) {
                    var trailerIds = [];
                    _selectedTrailers.forEach(x => {
                        trailerIds.push(x.Id);
                    });
                    this.GetfuelRetainDetails(trailerIds);
                }
            });
        }
        else {
            this.assignDriverToScheduleSave(driverObj, schedule, _form);
            this.highLightDRDiv(trips, null);
        }
    }
    assignDriverToScheduleSave(driverObj, schedule, _form) {
        Declarations.hideModal('#driverTrailerModel');
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return _form.Trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        let data = {
            Index1: _form.Index1,
            Index2: _form.Index2,
            Driver: driverObj,
            Trailers: _selectedTrailers
        };
        this.dataService.setUpdateDriverTrailerSubject(data);
    }
    saveDriverAssignment(shiftIndex, scheduleIndex) {
        var schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + scheduleIndex);
        var drivers = schedule.get('Drivers').value;
        var trailers = schedule.get('Trailers').value;
        var trips = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + scheduleIndex + '.Trips');
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dataToSave = this.getDSBSaveModel();
        for (var i = 0; i < trips.length; i++) {
            trips.value[i].Drivers = drivers;
            trips.value[i].Trailers = trailers;
            dataToSave.Trips.push(trips.value[i]);
        }
        dataToSave.Status = 2;
        this.dataService.setShowDeliveryGroupSubject(false);
        this._savingBuilder = true;
        this.sbService.assignDriverAndTrailer(dataToSave).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadsFromRow(data, trips);
        });
    }
    getDSBSaveModel() {
        var sbModel = this.SbForm.value;
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
    setTripStatus(trip) {
        if (trip) {
            var tripPrevStatusId = trip.TripPrevStatus;
            var tripStatusId = TripStatus.Added;
            if (tripPrevStatusId == TripStatus.None) {
                tripStatusId = TripStatus.Added;
            }
            else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified) {
                tripStatusId = TripStatus.Modified;
            }
            trip.TripStatus = tripStatusId;
        }
    }
    setDeliveryGroupStatus(trip, statusId) {
        if (trip) {
            trip.DeliveryGroupStatus = statusId;
        }
    }
    setDeliveryRequestStatus(trip, statusId, updateScheduleStatus = false) {
        if (trip) {
            var deliveryRequests = trip.DeliveryRequests;
            for (var i = 0; i < deliveryRequests.length; i++) {
                deliveryRequests[i].Status = statusId;
                //let isCompletedDrop = deliveryRequests.controls[i].get('StatusClassId').value == 4;
                if (updateScheduleStatus) {
                    var scheduleStatus = deliveryRequests[i].ScheduleStatus;
                    if (scheduleStatus == 14) {
                        deliveryRequests[i].ScheduleStatus = 15;
                    }
                    else {
                        deliveryRequests[i].ScheduleStatus = 14;
                    }
                }
            }
        }
    }
    processPostloadedDrToSaveEditedData(postloadInfo, modifiedTrips) {
        let shifts = this.SbForm.controls['Shifts'].value;
        let postloadInfoClone = JSON.parse(JSON.stringify(postloadInfo));
        postloadInfoClone.forEach(f => { f.ShiftIndex = shifts.indexOf(shifts.find(t => t.Id == f.ShiftId)), f.DrId = ''; f.ShiftId = ''; });
        let postloadInfoArray = [];
        postloadInfoClone.forEach(t => {
            if (!postloadInfoArray.find(x => x.ShiftIndex == t.ShiftIndex && x.ScheduleIndex == t.ScheduleIndex && x.TripIndex == t.TripIndex)) {
                postloadInfoArray.push({ ShiftIndex: t.ShiftIndex, ScheduleIndex: t.ScheduleIndex, TripIndex: t.TripIndex, DrId: '', ShiftId: t.ShiftId });
            }
        });
        for (var index = 0; index < postloadInfoArray.length; index++) {
            let modifiedPreloadedTripInfo = {
                ShiftIndex: postloadInfoArray[index].ShiftIndex,
                DriverRowIndex: postloadInfoArray[index].ScheduleIndex,
                DriverColIndex: postloadInfoArray[index].TripIndex,
                ShiftId: postloadInfoArray[index].ShiftId,
            };
            modifiedTrips.push(modifiedPreloadedTripInfo);
        }
    }
    draftScheduleBuilder(trip, filterChanged = false) {
        let shiftIndex = trip.controls['ShiftIndex'].value;
        let rowIndex = trip.controls['DriverRowIndex'].value;
        let loadIndex = trip.controls['DriverColIndex'].value;
        let modifiedTrips = [{ ShiftIndex: shiftIndex, DriverRowIndex: rowIndex, DriverColIndex: loadIndex }];
        let postloadInfo = this.getPostloadedInfoFromLoad(trip);
        if (postloadInfo && postloadInfo.length > 0) {
            this.processPostloadedDrToSaveEditedData(postloadInfo, modifiedTrips);
        }
        this.draftScheduleBuilderData(modifiedTrips, filterChanged);
    }
    draftScheduleBuilderData(_unsavedChanges, isDateChange) {
        if (_unsavedChanges.length > 0) {
            var isValidTrips = this.validateTrips(_unsavedChanges);
            if (!isValidTrips) {
                return;
            }
            this._savingBuilder = true;
            var trips = [];
            var dataToSave = this.getDSBSaveModel();
            _unsavedChanges.forEach(t => {
                let schedule = this.SbForm.get('Shifts.' + t.ShiftIndex + '.Schedules.' + t.DriverRowIndex);
                var trip = schedule.get('Trips.' + t.DriverColIndex);
                trips.push(trip);
                var tripModel = trip.value;
                this.setTripStatus(tripModel);
                this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Draft);
                tripModel.Drivers = schedule.controls['Drivers'].value;
                tripModel.Trailers = schedule.controls['Trailers'].value;
                dataToSave.Trips.push(tripModel);
            });
            this.changeDetectorRef.detectChanges();
            this.dataService.setShowDeliveryGroupSubject(false);
            if (dataToSave.Trips.length > 0) {
                dataToSave.Status = 1;
                this.sbService.saveDriverView(dataToSave).subscribe(data => {
                    this._savingBuilder = false;
                    this.updateLoadForm(data, trips, isDateChange);
                });
            }
        }
    }
    setAllowDriverChange(i, j) {
        let _thisRow = this.SbForm.get('Shifts.' + i + '.Schedules.' + j);
        var schedule = _thisRow.value;
        var allowDriverchange = true;
        for (var k = 0; k < schedule.Trips.length; k++) {
            let thisTrip = schedule.Trips[k];
            if (thisTrip.DeliveryGroupStatus == DeliveryGroupStatus.Published || thisTrip.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published) {
                allowDriverchange = false;
                this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
                return;
            }
            var deliveryReqs = thisTrip.DeliveryRequests;
            if (deliveryReqs.findIndex(t => (t.PreLoadedFor != null && t.PreLoadedFor.trim() != '') || (t.PostLoadedFor != null && t.PostLoadedFor.trim() != '')) != -1) {
                allowDriverchange = false;
                this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
                return;
            }
        }
        this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
    }
    cancelScheduleBuilder(i, j, k, trip) {
        let _thisTrip = this.SbForm.get('Shifts.' + i + '.Schedules.' + j + '.Trips.' + k);
        let _thisDrArray = _thisTrip.get('DeliveryRequests');
        _thisDrArray.clear();
        let oldTripValue = trip.value;
        trip.value.DeliveryRequests.forEach(x => {
            _thisDrArray.push(this.utilService.getDeliveryRequestForm(x, oldTripValue.IsCommonPickup));
        });
        _thisTrip.patchValue(trip.value);
        this.changeDetectorRef.detectChanges();
    }
    publishScheduleBuilder(i, j, k, schedule, trip) {
        this._savingBuilder = true;
        var drivers = schedule.get('Drivers').value;
        var trailers = schedule.get('Trailers').value;
        if (drivers == null || drivers == undefined || drivers.length == 0 || ((this.IsTrailerExists) && (trailers == null || trailers == undefined || trailers.length == 0))) {
            if (this.IsTrailerExists) {
                Declarations.msgwarning('Please assign a driver / trailer to publish Load ' + (k + 1), 'Driver/Trailer Required', 2500);
            }
            else {
                Declarations.msgwarning('Please assign a driver to publish Load ' + (k + 1), 'Driver Required', 2500);
            }
            this._savingBuilder = false;
            return;
        }
        var delReqs = trip.get('DeliveryRequests').value;
        if (drivers != null && drivers.length > 0) {
            if (delReqs.length > 0 && delReqs.some(t => t.IsFilldInvoke == true)) {
                if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver / trailer to publish Load ' + (k + 1), 'Driver/Trailer Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
                else if (drivers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver to publish Load ' + (k + 1), 'Driver Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
            }
        }
        if (trip != null && trip != undefined) {
            if (trip.invalid || !this.validatePublishLoad(trip)) {
                let invalidctrls = CustomAbstractControls.findRecursiveErrors(trip);
                this.editExisingGroup(trip, i, j, k, schedule, true);
                this.dataService.setShowOpenedDeliveryGroupSubject(true);
                this._savingBuilder = false;
                return;
            }
            else {
                this.dataService.setShowDeliveryGroupSubject(false);
            }
        }
        //Check Job and Trailer Compatibility
        var _deliveryRequests = trip.get('DeliveryRequests').value;
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                if (data) {
                    if (data.trackableScheduleStatuses && data.trackableScheduleStatuses.length > 0) {
                        var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, data.trackableScheduleStatuses, true);
                        var isCompletedSchedule = completedSchedules.length > 0;
                        if (isCompletedSchedule || data.trackableScheduleStatuses.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                            this._savingBuilder = false;
                            Declarations.msgerror("Can't edit group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                            return;
                        }
                    }
                    if (data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                        this.highLightDRDivOneLoad(trip, data);
                        this._savingBuilder = false;
                        Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                        return;
                    }
                    if (data.trackableScheduleStatuses && data.trackableScheduleStatuses.length > 0) {
                        var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, data.trackableScheduleStatuses, false);
                        if (onTheWaySchedules.length > 0) {
                            this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                            this.PublishedGroup = { shiftIndex: i, rowIndex: j, colIndex: k, schedule: schedule, trip: trip };
                            this._savingBuilder = false;
                            jQuery('#btnconfirm-publish-delgrp').click();
                            return;
                        }
                    }
                    if (delReqs.some(t => t.IsFilldInvoke == true)) {
                        var delReqOrderIds = delReqs.map(t => { if (t.IsFilldInvoke == true)
                            return t.OrderId; });
                        this.sbService.validateFilldOrderCompatibility(delReqOrderIds)
                            .subscribe(data => {
                            if (data && data.Result == false) {
                                this._savingBuilder = false;
                                Declarations.msgerror(data.Message, undefined, undefined);
                                this.changeDetectorRef.detectChanges();
                                return;
                            }
                            this.highLightDRDivOneLoad(trip, data);
                            this.publishLoadSave(i, j, k, schedule, trip);
                        });
                    }
                    else {
                        this.highLightDRDivOneLoad(trip, data);
                        this.publishLoadSave(i, j, k, schedule, trip);
                    }
                }
            });
        }
    }
    validateDraftLoad(trip) {
        var isValid = true;
        if (trip.controls['StartTime'].invalid || trip.controls['EndTime'].invalid || trip.controls['StartDate'].invalid) {
            isValid = false;
            trip.controls['StartDate'].touched;
            !trip.controls['StartDate'].value ? Declarations.msgerror('', 'Invalid Date', 5000) : Declarations.msgerror('', 'Please fill required field', 5000);
        }
        return isValid;
    }
    validatePublishLoad(trip) {
        var isValid = true;
        if (trip.controls.IsCommonPickup.value && !(trip.controls.Terminal.valid || trip.controls.BulkPlant.valid)) {
            isValid = false;
            Declarations.msgerror('', 'Please select common pickup location', 5000);
        }
        return isValid;
    }
    publishLoadYes() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.publishLoadSave(this.PublishedGroup.shiftIndex, this.PublishedGroup.rowIndex, this.PublishedGroup.colIndex, this.PublishedGroup.schedule, this.PublishedGroup.trip);
        this.PublishedGroup = null;
    }
    publishLoadNo() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.PublishedGroup = null;
    }
    publishLoadSave(i, j, k, schedule, trip) {
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var tripValue = trip.value;
        this.setTripStatus(tripValue);
        this.setDeliveryGroupStatus(tripValue, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(tripValue, DeliveryReqStatus.ScheduleCreated);
        tripValue.Drivers = schedule.get('Drivers').value;
        tripValue.Trailers = schedule.get('Trailers').value;
        dsbModel.Trips.push(tripValue);
        dsbModel.Status = 3;
        this.sbService.publishDriverView(dsbModel).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadForm(data, trip);
        });
    }
    validateRowPublish(shiftIndex, rowIndex) {
        this.dataService.setShowDeliveryGroupSubject(false);
        let schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex);
        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;
        if (drivers == null || drivers == undefined || drivers.length == 0 || (this.IsTrailerExists && (trailers == null || trailers == undefined || trailers.length == 0))) {
            if (this.IsTrailerExists)
                Declarations.msgwarning('Please assign a driver / trailer to publish', 'Driver/Trailer Required', 2500);
            else
                Declarations.msgwarning('Please assign a driver to publish', 'Driver Required', 2500);
            return;
        }
        //Check Job and Trailer Compatibility
        var trips = schedule.controls['Trips'];
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        if (drivers != null && drivers.length > 0) {
            var delReqs = _deliveryRequests;
            if (delReqs.length > 0 && delReqs.some(t => t.IsFilldInvoke == true)) {
                if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver / trailer to publish', 'Driver/Trailer Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
                else if (drivers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver to publish', 'Driver Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
            }
        }
        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                    this.highLightDRDiv(trips, data);
                    Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                }
                else if (_deliveryRequests.some(t => t.IsFilldInvoke == true)) {
                    var delReqOrderIds = _deliveryRequests.map(t => { if (t.IsFilldInvoke == true)
                        return t.OrderId; });
                    this.sbService.validateFilldOrderCompatibility(delReqOrderIds)
                        .subscribe(data => {
                        if (data && data.Result == false) {
                            Declarations.msgerror(data.Message, undefined, undefined);
                        }
                        else {
                            this.publishEntireRow(schedule, shiftIndex, rowIndex, trips);
                            this.highLightDRDiv(trips, null);
                        }
                    });
                }
                else {
                    this.publishEntireRow(schedule, shiftIndex, rowIndex, trips);
                    this.highLightDRDiv(trips, null);
                }
            });
        }
        else {
            this.publishEntireRow(schedule, shiftIndex, rowIndex, trips);
            this.highLightDRDiv(trips, null);
        }
        //End Check Job and Trailer Compatibility
    }
    publishEntireRow(schedule, shiftIndex, rowIndex, trips) {
        var validTrips = [];
        for (var i = 0, j = 1; i < trips.length; i++, j++) {
            let thisTrip = trips.controls[i];
            if (thisTrip && (thisTrip.invalid || !this.validatePublishLoad(thisTrip))) {
                let invalidctrls = CustomAbstractControls.findRecursiveErrors(thisTrip);
                if (thisTrip.get('DeliveryRequests').value.length > 0) {
                    this.editExisingGroup(thisTrip, shiftIndex, rowIndex, i, schedule, true);
                    this.dataService.setShowOpenedDeliveryGroupSubject(true);
                    return;
                }
            }
            else {
                validTrips.push(i);
            }
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;
        for (var i = 0; i < trips.length; i++) {
            var tripValue = trips.value[i];
            if (validTrips.includes(tripValue.DriverColIndex)) {
                this.setTripStatusToPublish(tripValue);
            }
            dsbModel.Trips.push(tripValue);
        }
        dsbModel.Trips.forEach(t => { t.Drivers = drivers, t.Trailers = trailers; });
        dsbModel.Status = 3;
        this.sbService.publishDriverView(dsbModel).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadForm(data, trips);
        });
    }
    setTripStatusToPublish(trip) {
        this.setTripStatus(trip);
        this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(trip, DeliveryReqStatus.ScheduleCreated);
    }
    updateLoadForm(data, trip, isDateChange = false) {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.droppedTrip = null;
            this.draggedDelReqData = null;
            this.setUnsavedChanges();
            if (!isDateChange) {
                this.SbForm.patchValue({
                    Id: data.Id,
                    TimeStamp: data.TimeStamp,
                    Status: 0
                });
                if (data && data.Trips && data.Trips.length > 0) {
                    if (trip instanceof FormGroup) {
                        trip.patchValue(data.Trips[0]);
                        var sIndex = trip.controls['ShiftIndex'].value;
                        var rIndex = trip.controls['DriverRowIndex'].value;
                        this.setAllowDriverChange(sIndex, rIndex);
                        this.dataService.setSavedChangesSubject(trip.value);
                    }
                    else if (trip instanceof FormArray) {
                        for (var i = 0, j = 1; i < trip.length; i++, j++) {
                            let thisTrip = trip.controls[i];
                            var shiftId = thisTrip.controls['ShiftId'].value;
                            var shiftIndex = thisTrip.controls['ShiftIndex'].value;
                            var rowIndex = thisTrip.controls['DriverRowIndex'].value;
                            var colIndex = thisTrip.controls['DriverColIndex'].value;
                            var savedTrip = data.Trips.find(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                            if (savedTrip) {
                                trip.controls[i].patchValue(savedTrip);
                                this.setAllowDriverChange(shiftIndex, rowIndex);
                                this.dataService.setSavedChangesSubject(trip.controls[i].value);
                            }
                        }
                    }
                    else {
                        for (var i = 0, j = 1; i < trip.length; i++, j++) {
                            let thisTrip = trip[i];
                            var shiftId = thisTrip.controls['ShiftId'].value;
                            var shiftIndex = thisTrip.controls['ShiftIndex'].value;
                            var rowIndex = thisTrip.controls['DriverRowIndex'].value;
                            var colIndex = thisTrip.controls['DriverColIndex'].value;
                            var savedTrip = data.Trips.find(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                            if (savedTrip) {
                                trip[i].patchValue(savedTrip);
                                this.setAllowDriverChange(shiftIndex, rowIndex);
                                this.dataService.setSavedChangesSubject(trip[i].value);
                            }
                        }
                    }
                }
            }
            if (data.StatusCode == 0)
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            else
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            if (isDateChange) {
                this.dataService.setUnsavedChangesAsEmptySubject();
            }
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
        }
        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
        //this.dataService.FormUnsavedChangesSubject.unsubscribe();
    }
    updateLoadsFromRow(data, trips) {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            this.editDriverCancel();
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.setUnsavedChanges();
            this.SbForm.patchValue({
                Id: data.Id,
                TimeStamp: data.TimeStamp,
                Status: 0
            });
            for (var i = 0, j = 1; i < trips.length; i++, j++) {
                let thisTrip = trips.controls[i];
                var shiftId = thisTrip.get('ShiftId').value;
                var rowIndex = thisTrip.get('DriverRowIndex').value;
                var colIndex = thisTrip.get('DriverColIndex').value;
                var savedTrip = data.Trips.filter(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                if (savedTrip && savedTrip != null && savedTrip.length > 0) {
                    thisTrip.patchValue({
                        TripId: savedTrip[0].TripId,
                        TimeStamp: savedTrip[0].TimeStamp
                    });
                }
            }
            if (data.StatusCode == 0)
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            else
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
            this.editDriverCancel();
        }
        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
        //this.dataService.FormUnsavedChangesSubject.unsubscribe();
    }
    isTrailersSelected() {
        let _trailers = this.DriverTrailerForm.get('Trailers').value;
        return _trailers && _trailers.length > 0;
    }
    addShift(shiftIdx, scheduleIdx) {
        var _tArray = this.SbForm.get('Shifts.' + shiftIdx + '.Schedules.' + scheduleIdx + '.Trips');
        let _startDate = this.SbForm.get('Date').value;
        var shift = this.SbForm.get('Shifts.' + shiftIdx);
        let shiftId = shift.get('Id').value;
        let selectedShift = this.Shifts.find(x => x.Id == shiftId);
        let _startTime = selectedShift.StartTime;
        if (_tArray.controls.length > 0) {
            let lastTripStartTime = _tArray.controls[_tArray.controls.length - 1].get('StartTime').value;
            let lastTripStartDate = _tArray.controls[_tArray.controls.length - 1].get('StartDate').value;
            _startTime = _tArray.controls[_tArray.controls.length - 1].get('EndTime').value;
            _startDate = this.getNewLoadStartDate(moment(lastTripStartDate + ' ' + lastTripStartTime).toDate(), moment(lastTripStartDate + ' ' + _startTime).toDate());
        }
        let startTime = moment(_startDate + ' ' + _startTime).toDate();
        let trip = this.getTrailerTrip(startTime, selectedShift.SlotPeriod, scheduleIdx, _tArray.controls.length);
        trip.ShiftStartTime = shift.get('StartTime').value;
        trip.ShiftEndTime = shift.get('EndTime').value;
        trip.SlotPeriod = shift.get('SlotPeriod').value;
        trip.ShiftId = shiftId;
        trip.ShiftIndex = shiftIdx;
        _tArray.push(this.utilService.getTripFormGroup(trip));
    }
    getNewLoadStartDate(startDateTime, endDateTime) {
        if (startDateTime > endDateTime) {
            endDateTime.setDate(endDateTime.getDate() + 1);
        }
        return moment(endDateTime).format('MM/DD/YYYY');
    }
    getTrailerTrip(startTime, slotPeriod, rowIndex, colIndex) {
        if (slotPeriod <= 0) {
            slotPeriod = 3;
        }
        let trip = new TripModel();
        trip.StartDate = moment(startTime).format('MM/DD/YYYY');
        trip.StartTime = moment(startTime).format('hh:mm A');
        let endTime = moment(startTime).add(slotPeriod, 'hours').toDate();
        trip.EndTime = moment(endTime).format('hh:mm A');
        trip.IsEditable = true;
        trip.IsPreloadDisable = false;
        trip.DriverRowIndex = rowIndex;
        trip.DriverColIndex = colIndex;
        return trip;
    }
    toggleCollaspe(idx) {
        this.renderNextNRows(idx);
    }
    onTrailerSelect() {
        this.getDriverdetails();
    }
    onTrailerSelectAll(items) {
        this.DriverTrailerForm.get('Trailers').setValue(items);
        this.getDriverdetails();
    }
    onTrailerDeSelectAll() {
        this.DriverTrailerForm.get('Trailers').setValue([]);
        this.getDriverdetails();
        this._isDriverFound = false;
    }
    onTrailerDeSelect() {
        this.getDriverdetails();
    }
    getDriverdetails() {
        var trailers = this.DriverTrailerForm.get('Trailers').value;
        if (trailers.length > 0 || !this.IsTrailerExists) {
            var trailerId = [];
            trailers.forEach(obj => { trailerId.push(obj.Id); });
            this.sbService.getCompanyDrivers(trailerId, this.SelectedRegionId, this.SbForm.controls.Date.value).subscribe((drivers) => {
                this.AllUnassignedDrivers = drivers;
                this.UnassignedDrivers = this.getUnassignedDrivers();
                if (this.UnassignedDrivers.length == 0) {
                    this.UnassignedDrivers = [];
                    this._isDriverFound = true;
                }
                else {
                    this._isDriverFound = false;
                }
                this._loadingDrivers = false;
                this.changeDetectorRef.detectChanges();
            });
        }
        else {
            this.UnassignedDrivers = [];
            this._isDriverFound = true;
            this._loadingDrivers = false;
            this.changeDetectorRef.detectChanges();
        }
    }
    IdNotInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id;
            }).length == 0;
        };
    }
    CodeComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Code == current.Code;
            }).length == 0;
        };
    }
    checkAnyPublishedTrip(schedule) {
        let isPublished = false;
        var trips = schedule.get('Trips');
        for (var i = 0; i < trips.length; i++) {
            if (trips[i] != null && trips[i] != undefined && !isPublished) {
                var drs = trips[i].get('DeliveryRequests');
                for (let k = 0; k < drs.length; k++) {
                    if (drs.controls[k].get('PreviousStatus').value === 3) {
                        isPublished = true;
                        break;
                    }
                }
                if (isPublished)
                    break;
            }
        }
        return isPublished;
    }
    editExisingGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowOpenedDeliveryGroupSubject(true);
    }
    editDroppedGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowDeliveryGroupSubject(true);
    }
    editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid = false) {
        const drsFormArray = _trip.controls['DeliveryRequests'];
        let drs = drsFormArray.value || [];
        drs = sortArrayTwice(drs, 'JobId', 'ProductSequence');
        drsFormArray.patchValue(drs);
        this.dataService.setEditDeliveryGroupSubject({
            trip: _trip,
            shiftIndex: _shiftIndex,
            rowIndex: _rowIndex,
            tripIndex: _tripIndex,
            schedule: _schedule,
            isPublishLoadInvalid: _isPublishLoadInvalid
        });
    }
    cancelCreateLoad(data) {
        let trip = this.SbForm.get('Shifts.' + data.ShiftIndex + '.Schedules.' + data.RowIndex + '.Trips.' + data.ColIndex);
        if (trip) {
            let _drArray = trip.controls['DeliveryRequests'];
            if (_drArray) {
                data.Drs.forEach(x => {
                    let drIndex = _drArray.controls.findIndex((y) => y.controls['Id'].value == x.Id);
                    if (drIndex >= 0) {
                        _drArray.removeAt(drIndex);
                    }
                });
            }
            this.changeDetectorRef.markForCheck();
            this.dataService.setRestoreDeletedRequestSubject(data.Drs);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }
    updateTripOnCreateLoadSucess(data) {
        let schedule = this.SbForm.get('Shifts.' + data.ShiftIndex + '.Schedules.' + data.RowIndex);
        let trip = schedule.get('Trips.' + data.ColIndex);
        if (trip && data.Drs.length > 0) {
            data.Drs.forEach(t => t.Status = 5);
            let groupParentDrs = data.Drs.filter(x => x.GroupParentDRId != '').length;
            let jobId = data.Drs[0].JobId;
            let _drArray = trip.controls['DeliveryRequests'];
            if (_drArray) {
                let existingDrsIndexes = [];
                let existingDrs = _drArray.controls.filter((x, index) => {
                    let groupParentDRId = x.controls['GroupParentDRId'].value;
                    let drId = x.controls['Id'].value;
                    if (groupParentDrs > 0) {
                        if (data.Drs.find(x => x.GroupParentDRId == groupParentDRId && x.Id == drId)) {
                            existingDrsIndexes.push(index);
                        }
                    }
                    else if (x.controls['JobId'].value == jobId && (x.controls['Status'].value == 0 || x.controls['Status'].value == 1 || x.controls['Status'].value == 2 || x.controls['Status'].value == 5)) {
                        existingDrsIndexes.push(index);
                    }
                    return x.controls['JobId'].value == jobId;
                });
                for (let index = existingDrsIndexes.length - 1; index >= 0; index--) {
                    _drArray.removeAt(existingDrsIndexes[index]);
                }
                let isCommonPickup = trip.controls['IsCommonPickup'].value;
                if (existingDrsIndexes.length > 0 && _drArray.controls.length > 0) {
                    for (let index = data.Drs.length - 1; index >= 0; index--) {
                        let _drForm = this.utilService.getDeliveryRequestForm(data.Drs[index], isCommonPickup);
                        _drArray.insert(existingDrsIndexes[0], _drForm);
                    }
                }
                else {
                    for (let index = 0; index < data.Drs.length; index++) {
                        let _drForm = this.utilService.getDeliveryRequestForm(data.Drs[index], isCommonPickup);
                        _drArray.push(_drForm);
                    }
                }
            }
            this.changeDetectorRef.markForCheck();
            this.dataService.setScheduleChangeDetectSubject(true);
            this.addNewDrsInDataService(jobId, data);
        }
        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        // if (trip && data.Drs) {
        //    this.editDroppedGroup(trip, data.ShiftIndex, data.RowIndex, data.ColIndex, schedule, false);
        //}
    }
    addNewDrsInDataService(jobId, data) {
        let AllDrs = this.dataService.AllDeliveryRequestsSubject.value;
        //let drsToRemove = AllDrs.filter(t => t.JobId == jobId);
        let subDRs = AllDrs.filter(t => t.GroupParentDRId && t.GroupParentDRId != '' && t.JobId == jobId);
        let subJobDRs = AllDrs.filter(t => t.GroupParentDRId == '' && t.JobId == jobId);
        AllDrs = AllDrs.filter(t => t.JobId != jobId);
        AllDrs = AllDrs.concat(data.Drs);
        AllDrs = AllDrs.concat(subDRs);
        if (subDRs.length > 0) {
            AllDrs = AllDrs.concat(subJobDRs);
        }
        this.dataService.setAllDeliveryRequestsSubject(AllDrs);
        //this.dataService.setRemoveDroppedRequestSubject(drsToRemove);
    }
    setUnsavedChanges() {
        if (this.driverSchedules) {
            this.driverSchedules.forEach(x => x.unsubscribeFormChange());
        }
    }
    getPreloadDrViewModel(preLoadInfo, drIds, shiftEndDate) {
        var model = new PreLoadDrViewModel();
        model.PreloadDrs = drIds;
        model.PreloadTrailers = preLoadInfo.PreloadTrailers;
        model.PreloadDrivers = preLoadInfo.PreloadDrivers;
        model.IsTrailerExists = preLoadInfo.IsTrailerExists;
        model.RegionId = this.SbForm.controls['RegionId'].value;
        model.SbView = this.SbForm.controls['ObjectFilter'].value;
        model.ShiftEndDate = shiftEndDate;
        model.ShiftId = preLoadInfo.ShiftId;
        return model;
    }
    getModifiedLoadTripInfo(loadInfo) {
        let _modifiedTripInfo = new ModifiedTripInfo();
        _modifiedTripInfo.ShiftIndex = loadInfo.ShiftIndex;
        _modifiedTripInfo.DriverRowIndex = loadInfo.ScheduleIndex;
        _modifiedTripInfo.DriverColIndex = loadInfo.TripIndex;
        return _modifiedTripInfo;
    }
    createPreloadForAcrossTheDate(preLoadInfo, shiftEndDate) {
        // shift cross the date, need to create a draft schedule builder (or update existing)
        var model = this.getPreloadDrViewModel(preLoadInfo, preLoadInfo.PreloadDrs, shiftEndDate);
        this._savingBuilder = true;
        this.sbService.createPreloadForAcrossTheDate(model).subscribe(data => {
            if (data == null) {
                Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            }
            else if (data.StatusCode == 0) {
                this.updateAcrossTheDateDrsPreloadInfo(preLoadInfo, data);
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
            }
            if (data.StatusCode == 0) {
                let _modifiedTripInfo = this.getModifiedLoadTripInfo(preLoadInfo);
                this.saveScheduleBuilderData([_modifiedTripInfo], false);
            }
            this.changeDetectorRef.detectChanges();
        });
    }
    createPreloadForSameDate(preLoadInfo, shifts, shiftEndDate) {
        let postLoadInfo = this.getShiftInfoWithDriverTrailerInOtherShift(preLoadInfo, shifts);
        if (postLoadInfo == undefined) {
            if (preLoadInfo.IsTrailerExists) {
                let trailerNames = preLoadInfo.PreloadTrailers.map(t => t.TrailerId).join(", ");
                trailerNames = preLoadInfo.PreloadTrailers.length > 1 ? 'trailers ' + trailerNames + ' are' : 'trailer ' + trailerNames + ' is';
                Declarations.msgerror('Preload can not be done as ' + trailerNames + ' not assigned to any other shift', undefined, undefined);
            }
            else {
                Declarations.msgerror('Preload can not be done as driver is not assigned to any other shift', undefined, undefined);
            }
        }
        else {
            this._savingBuilder = true;
            let drIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            this.sbService.cloneDrsForPreload(drIds).subscribe(data => {
                if (data == null) {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                else if (data.length > 0) {
                    postLoadInfo["PostloadDrs"] = data;
                    this.updatePreloadDrsOnSuccess(preLoadInfo, postLoadInfo);
                    Declarations.msgsuccess('Preload created successfully for the date ' + shiftEndDate, undefined, undefined);
                    let _modifiedPreloadTripInfo = this.getModifiedLoadTripInfo(preLoadInfo);
                    let _modifiedPostloadTripInfo = this.getModifiedLoadTripInfo(postLoadInfo);
                    this.saveScheduleBuilderData([_modifiedPreloadTripInfo, _modifiedPostloadTripInfo], false);
                    this.dataService.setDeliveryPreloadOption({ ShiftIndex: preLoadInfo.ShiftIndex, ScheduleIndex: preLoadInfo.ScheduleIndex, Reset: false });
                }
                else {
                    Declarations.msgsuccess('Failed to create preload. Please contact the administrator.', undefined, undefined);
                }
                this.changeDetectorRef.detectChanges();
            });
        }
    }
    onPreloadTrailerChange(event) {
        let trailerId = event.target.value;
        if (trailerId) {
            this.preloadSelectedTrailerId = trailerId;
        }
    }
    onPreloadTrailerSubmit() {
        if (this.preloadSelectedTrailerId) {
            Declarations.hideModal("#select-preload-trailer");
            let selectedTrailer = this.PreLoadInfo.PreloadTrailers.find(x => x.Id == this.preloadSelectedTrailerId);
            this.PreLoadInfo.PreloadTrailers = [selectedTrailer];
            this.processPreloadDeliveryCreation(this.PreLoadInfo);
            this.preloadSelectedTrailerId = null;
        }
    }
    processPreloadDeliveryCreation(preLoadInfo) {
        let shifts = this.SbForm.controls['Shifts'].value;
        let preloadShift = shifts.find(x => x.Id == preLoadInfo.ShiftId);
        let result = this.isAcrossTheDateShift(preloadShift);
        if (result.IsAcrossTheDateShift) {
            // shift cross the date, need to create a draft schedule builder (or update existing)
            this.createPreloadForAcrossTheDate(preLoadInfo, result.ShiftEndDate);
        }
        else {
            // shift is ending in same date, so search for same trailers
            this.createPreloadForSameDate(preLoadInfo, shifts, result.ShiftEndDate);
        }
    }
    isAcrossTheDateShift(preloadShift) {
        let shiftStartTime = preloadShift.StartTime;
        let shifEndTime = preloadShift.EndTime;
        let dsbDate = this.SbForm.controls['Date'].value;
        let startDate = moment(dsbDate + ' ' + shiftStartTime).toDate();
        let endDate = moment(dsbDate + ' ' + shifEndTime).toDate();
        if (startDate > endDate) {
            endDate.setDate(endDate.getDate() + 1);
        }
        let shiftEndDate = moment(endDate).format('MM/DD/YYYY');
        let _isAcrossTheDateShift = dsbDate != shiftEndDate;
        return {
            IsAcrossTheDateShift: _isAcrossTheDateShift,
            ShiftEndDate: shiftEndDate
        };
    }
    validateTrips(_unsavedChanges) {
        var isValidTrips = true;
        _unsavedChanges.forEach(t => {
            let schedule = this.SbForm.get('Shifts.' + t.ShiftIndex + '.Schedules.' + t.DriverRowIndex);
            var trip = schedule.get('Trips.' + t.DriverColIndex);
            if (trip.get('GroupId').value > 0) {
                if (!trip.valid) {
                    let invalidctrls = CustomAbstractControls.findRecursiveErrors(trip);
                    if (trip.get('DeliveryRequests').value.length > 0) {
                        this.editExisingGroup(trip, t.ShiftIndex, t.DriverRowIndex, t.DriverColIndex, schedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        isValidTrips = false;
                        return isValidTrips;
                    }
                }
            }
        });
        return isValidTrips;
    }
    saveScheduleBuilderData(_unsavedChanges, isDateChange) {
        if (_unsavedChanges.length > 0) {
            var isValidTrips = this.validateTrips(_unsavedChanges);
            if (!isValidTrips) {
                return;
            }
            this._savingBuilder = true;
            var isPublish = false;
            var trips = [];
            var dataToSave = this.getDSBSaveModel();
            _unsavedChanges.forEach(t => {
                let schedule = this.SbForm.get('Shifts.' + t.ShiftIndex + '.Schedules.' + t.DriverRowIndex);
                var trip = schedule.get('Trips.' + t.DriverColIndex);
                trips.push(trip);
                var tripModel = trip.value;
                if (trip.controls['GroupId'].value > 0) {
                    this.setTripStatus(tripModel);
                    this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Published);
                    this.setDeliveryRequestStatus(tripModel, DeliveryReqStatus.ScheduleCreated);
                    isPublish = true;
                }
                else {
                    this.setTripStatus(tripModel);
                    this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Draft);
                }
                tripModel.Drivers = schedule.controls['Drivers'].value;
                tripModel.Trailers = schedule.controls['Trailers'].value;
                dataToSave.Trips.push(tripModel);
            });
            this.changeDetectorRef.detectChanges();
            this.dataService.setShowDeliveryGroupSubject(false);
            if (dataToSave.Trips.length > 0) {
                if (isPublish) {
                    dataToSave.Status = 3;
                    this.sbService.publishDriverView(dataToSave).subscribe(data => {
                        this._savingBuilder = false;
                        this.updateLoadForm(data, trips, isDateChange);
                    });
                }
                else {
                    dataToSave.Status = 1;
                    this.sbService.saveDriverView(dataToSave).subscribe(data => {
                        this._savingBuilder = false;
                        this.updateLoadForm(data, trips, isDateChange);
                    });
                }
            }
        }
    }
    getShiftInfoWithDriverTrailerInOtherShift(data, allShifts) {
        let shiftWithSameDriverTrailer = undefined;
        for (var shiftIdx = data.ShiftIndex + 1; shiftIdx < allShifts.length; shiftIdx++) {
            let schedules = allShifts[shiftIdx].Schedules;
            for (var scheduleIdx = 0; scheduleIdx < schedules.length; scheduleIdx++) {
                shiftWithSameDriverTrailer = this.getMatchedDriverTrailerInfo(data, allShifts, shiftIdx, scheduleIdx);
                if (shiftWithSameDriverTrailer != undefined) {
                    break;
                }
            }
            if (shiftWithSameDriverTrailer != undefined) {
                break;
            }
        }
        return shiftWithSameDriverTrailer;
    }
    getMatchedDriverTrailerInfo(data, allShifts, shiftIdx, scheduleIdx) {
        let shiftWithSameTrailer = undefined;
        let schedules = allShifts[shiftIdx].Schedules;
        let matchedDriverTrailers = [];
        if (data.IsTrailerExists) {
            matchedDriverTrailers = schedules[scheduleIdx].Trailers.filter(this.IdInComparer(data.PreloadTrailers));
        }
        else {
            matchedDriverTrailers = schedules[scheduleIdx].Drivers.filter(this.IdInComparer(data.PreloadDrivers));
        }
        if (matchedDriverTrailers.length > 0) {
            shiftWithSameTrailer = {
                Shift: allShifts[shiftIdx],
                ShiftIndex: shiftIdx,
                ShiftId: allShifts[shiftIdx].Id,
                Schedule: schedules[scheduleIdx],
                ScheduleIndex: scheduleIdx,
                TripIndex: 0
            };
        }
        return shiftWithSameTrailer;
    }
    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id;
            }).length > 0;
        };
    }
    updatePreloadDrsOnSuccess(preLoadInfo, postLoadInfo) {
        this.updatePostLoadedForIds(preLoadInfo, postLoadInfo);
        this.updatePreloadDrs(preLoadInfo, postLoadInfo);
        this.updatePostloadDrs(preLoadInfo, postLoadInfo);
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    updatePostLoadedForIds(preLoadInfo, postLoadInfo) {
        preLoadInfo.PreloadDrs.forEach(x => {
            var postLoadedDr = postLoadInfo.PostloadDrs.find(y => y.PostLoadedFor == x.Id);
            if (postLoadedDr) {
                x['PreLoadedFor'] = postLoadedDr.Id;
            }
        });
    }
    updatePreloadDrs(preLoadInfo, postLoadInfo) {
        let trip = this.SbForm.get('Shifts.' + preLoadInfo.ShiftIndex + '.Schedules.' + preLoadInfo.ScheduleIndex + '.Trips.' + preLoadInfo.TripIndex);
        if (trip) {
            let preloadDrIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            let preloadDrs = trip.controls['DeliveryRequests'];
            let drsToUpdate = preloadDrs.controls.filter((x) => preloadDrIds.indexOf(x.controls['Id'].value) >= 0);
            drsToUpdate.forEach((x) => {
                let postLoadDr = postLoadInfo.PostloadDrs.find(y => y.PostLoadedFor == x.controls['Id'].value);
                if (postLoadDr) { //Add post load refernce to preload DRs
                    x.addControl('PreLoadedFor', this.fb.control(postLoadDr.Id));
                    x.addControl('PostLoadInfo', this.utilService.getLoadInfoForm({
                        ShiftId: postLoadInfo.ShiftId,
                        ScheduleIndex: postLoadInfo.ScheduleIndex,
                        TripIndex: postLoadInfo.TripIndex,
                        DrId: postLoadDr.Id
                    }));
                    x.controls['ScheduleStatus'].patchValue(15);
                }
            });
        }
    }
    updatePostloadDrs(preLoadInfo, postLoadInfo) {
        let trip = this.SbForm.get('Shifts.' + postLoadInfo.ShiftIndex + '.Schedules.' + postLoadInfo.ScheduleIndex + '.Trips.' + postLoadInfo.TripIndex);
        if (trip) {
            let postloadDrs = trip.controls['DeliveryRequests'];
            for (var index = postLoadInfo.PostloadDrs.length - 1; index >= 0; index--) {
                postLoadInfo.PostloadDrs[index].Status = 5; // New DR
                postLoadInfo.PostloadDrs[index].ScheduleStatus = 14; // New DR
                let preLoadDr = preLoadInfo.PreloadDrs.find(y => y.PreLoadedFor == postLoadInfo.PostloadDrs[index].Id);
                let postLoadDr = this.utilService.getDeliveryRequestForm(postLoadInfo.PostloadDrs[index], false);
                postLoadDr.patchValue({
                    Terminal: preLoadDr.Terminal,
                    BulkPlant: preLoadDr.BulkPlant,
                    OrderId: preLoadDr.OrderId,
                    PickupLocationType: preLoadDr.PickupLocationType
                });
                //Add pre load refernce to postload DRs
                postLoadDr.addControl('PreLoadInfo', this.utilService.getLoadInfoForm({
                    ShiftId: preLoadInfo.ShiftId,
                    ScheduleIndex: preLoadInfo.ScheduleIndex,
                    TripIndex: preLoadInfo.TripIndex,
                    DrId: preLoadDr.Id
                }));
                postloadDrs.insert(0, postLoadDr);
            }
        }
    }
    updateAcrossTheDateDrsPreloadInfo(preLoadInfo, data) {
        let trip = this.SbForm.get('Shifts.' + preLoadInfo.ShiftIndex + '.Schedules.' + preLoadInfo.ScheduleIndex + '.Trips.' + preLoadInfo.TripIndex);
        if (trip) {
            let preloadDrIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            let preloadDrs = trip.controls['DeliveryRequests'];
            let drsToUpdate = preloadDrs.controls.filter((x) => preloadDrIds.indexOf(x.controls['Id'].value) >= 0);
            drsToUpdate.forEach((x) => {
                let preLoadDrModel = data.PreloadDrs.find(y => y.Id == x.controls['Id'].value);
                if (preLoadDrModel) { //Add post load refernce to preload DRs
                    x.addControl('PreLoadedFor', this.fb.control(preLoadDrModel.PreLoadedForId));
                    x.controls['ScheduleStatus'].patchValue(15);
                    trip.controls['TripStatus'].patchValue(TripStatus.Modified);
                }
            });
        }
    }
    updateEditedPostloadDrs(drs) {
        let _shiftArray = this.SbForm.controls['Shifts'];
        drs.forEach(x => {
            let postLoadInfo = x.PostLoadInfo;
            if (postLoadInfo) {
                let thisShift = _shiftArray.controls.find((s) => s.controls['Id'].value == postLoadInfo.ShiftId);
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + postLoadInfo.ScheduleIndex + ".Trips." + postLoadInfo.TripIndex);
                    if (thisTrip) {
                        let thisDr = thisTrip.controls['DeliveryRequests']['controls'].find((d) => d.controls['Id'].value == postLoadInfo.DrId);
                        if (thisDr) {
                            let updatedValues = {
                                RequiredQuantity: x.RequiredQuantity,
                                ScheduleStatus: 15,
                                Terminal: x.Terminal,
                                BulkPlant: x.BulkPlant,
                                OrderId: x.OrderId
                            };
                            if (!thisDr.controls['IsCommonBadge'].value && !x.IsCommonBadge) {
                                updatedValues['BadgeNo1'] = x.BadgeNo1;
                                updatedValues['BadgeNo2'] = x.BadgeNo2;
                                updatedValues['BadgeNo3'] = x.BadgeNo3;
                                updatedValues['DispactherNote'] = x.DispactherNote;
                            }
                            thisDr.patchValue(updatedValues);
                        }
                    }
                }
            }
        });
        this.changeDetectorRef.detectChanges();
    }
    updateCompartmentPostloadDrs(drs, prepostloadStatus) {
        var posttripInfo = null;
        let _shiftArray = this.SbForm.controls['Shifts'];
        drs.forEach(x => {
            let postLoadInfo = null;
            if (prepostloadStatus == 1) {
                postLoadInfo = x.PostLoadInfo;
            }
            else {
                postLoadInfo = x.PreLoadInfo;
            }
            if (postLoadInfo) {
                let thisShift = _shiftArray.controls.find((s) => s.controls['Id'].value == postLoadInfo.ShiftId);
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + postLoadInfo.ScheduleIndex + ".Trips." + postLoadInfo.TripIndex);
                    if (thisTrip) {
                        posttripInfo = new ModifiedTripInfo();
                        posttripInfo.ShiftIndex = thisTrip['controls']['ShiftIndex'].value;
                        posttripInfo.DriverRowIndex = thisTrip['controls']['DriverRowIndex'].value;
                        posttripInfo.DriverColIndex = thisTrip['controls']['DriverColIndex'].value;
                        let thisDr = thisTrip.controls['DeliveryRequests']['controls'].find((d) => d.controls['Id'].value == postLoadInfo.DrId);
                        if (thisDr) {
                            let compartmentInfo = x.Compartments;
                            let compartmentArray = thisDr.get('Compartments');
                            let prevCompartment = compartmentArray.getRawValue();
                            compartmentArray.clear();
                            let disabledControl = false;
                            if (x.TrackScheduleEnrouteStatus == 16 || x.TrackScheduleStatus == 7 || x.TrackScheduleStatus == 8 || x.TrackScheduleStatus == 9) {
                                disabledControl = true;
                            }
                            for (var i = 0; i < compartmentInfo.length; i++) {
                                if (prevCompartment.length > 0) {
                                    var trailerExists = prevCompartment.find(top => top.TrailerId == compartmentInfo[i].TrailerId);
                                    if (trailerExists) {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                    else {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                }
                                else {
                                    if (compartmentInfo[i].TrailerId != null && compartmentInfo[i].CompartmentId != null) {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                }
                            }
                            for (var i = 0; i < compartmentInfo.length; i++) {
                            }
                            window.setTimeout(() => { this.validateDrQuantity(thisDr.controls['RequiredQuantity']); }, 1);
                            thisDr.controls['ScheduleStatus'].patchValue(15);
                        }
                    }
                }
            }
        });
        this.changeDetectorRef.detectChanges();
        return posttripInfo;
    }
    deletePreAndPostloadDrs(drs) {
        let drsToRestore = [];
        let _shiftArray = this.SbForm.controls['Shifts'];
        let postloadInfo = drs.filter(t => t.PostLoadInfo).map(m => m.PostLoadInfo);
        let preloadInfo = drs.filter(t => t.PreLoadInfo).map(m => m.PreLoadInfo);
        let prepostloadInfo = postloadInfo.concat(preloadInfo);
        prepostloadInfo = prepostloadInfo.filter((value, index, self) => self.indexOf(value) === index);
        prepostloadInfo.forEach(x => {
            if (x) {
                let thisShift = _shiftArray.controls.find((s) => s.controls['Id'].value == x.ShiftId);
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + x.ScheduleIndex + ".Trips." + x.TripIndex);
                    if (thisTrip) {
                        let _drArray = thisTrip.controls['DeliveryRequests'];
                        let thisDr = _drArray.controls.find((d) => d.controls['Id'].value == x.DrId);
                        if (thisDr) {
                            _drArray.removeAt(_drArray.controls.indexOf(thisDr));
                            if (!thisDr.controls['PostLoadedFor'] || !thisDr.controls['PostLoadedFor'].value) {
                                thisDr.value.Compartments = [];
                                drsToRestore.push(thisDr.value);
                            }
                        }
                    }
                }
            }
        });
        this.dataService.setScheduleChangeDetectSubject(true);
        if (drsToRestore && drsToRestore.length > 0) {
            this.dataService.setRestoreDeletedRequestSubject(drsToRestore);
        }
    }
    editCompartmentAssignments(model) {
        this.CompartmentDetails = [];
        this.CompartmentEditModel = model;
        let trailerIds = this.CompartmentEditModel.Schedule.Trailers.map(t => t.Id);
        let selectedTrailerId = 'null';
        if (trailerIds.length == 1) {
            selectedTrailerId = trailerIds[0];
        }
        this.CompartmentViewFormGroup = this.utilService.getCompartmentViewForm(this.CompartmentEditModel.Schedule.Trips, selectedTrailerId);
        this._loadingCmprts = true;
        this.sbService.getTrailerCompartments(trailerIds).subscribe(data => {
            this._loadingCmprts = false;
            if (data) {
                this.TrailerCompartmentRetains = data;
                for (var idx = 0; idx < data.length; idx++) {
                    this.TrailerCompartments[data[idx].TrailerId] = data[idx].Compartments;
                }
            }
            this.changeDetectorRef.detectChanges();
        });
    }
    addCompartment(dr, trailerId) {
        let compartments = dr.controls['Compartments'];
        if (!trailerId) {
            let trailerIds = this.CompartmentEditModel.Schedule.Trailers.map(t => t.Id);
            trailerId = trailerIds[0];
        }
        let model = new CompartmentModel();
        model.TrailerId = trailerId;
        let newComp = this.utilService.getCompartmentsFormGroup(dr.value, model, false);
        compartments.push(newComp);
    }
    removeCompartment(dr, index) {
        let compartments = dr.controls['Compartments'];
        compartments.removeAt(index);
        window.setTimeout(() => { this.validateDrQuantity(dr.controls['RequiredQuantity']); }, 1);
    }
    saveCompartmentAssignment() {
        this.CompartmentViewFormGroup.markAllAsTouched();
        if (this.CompartmentViewFormGroup.invalid) {
            Declarations.msgerror('Please resolve highlighted errors', undefined, undefined);
            return;
        }
        else {
            let modifiedTrips = this.setCompartmentInfoToRow();
            if (modifiedTrips.length > 0) {
                this.saveScheduleBuilderData(modifiedTrips, false);
            }
        }
    }
    setCompartmentInfoToRow() {
        let modifiedTrips = [];
        let shiftIndex = this.CompartmentEditModel.ShiftIndex;
        let rowIndex = this.CompartmentEditModel.RowIndex;
        let targetRow = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex);
        if (targetRow) {
            let sourceLoads = this.CompartmentViewFormGroup.controls['Trips']['controls'];
            let targetLoads = targetRow.controls['Trips']['controls'];
            for (var loadIdx = 0; loadIdx < targetLoads.length; loadIdx++) {
                let sourceLoad = sourceLoads[loadIdx];
                let targetLoad = targetLoads[loadIdx];
                let sourceDrs = sourceLoad['controls']['DeliveryRequests']['controls'];
                let targetDrs = targetLoad['controls']['DeliveryRequests']['controls'];
                let tripInfo = null;
                for (var drIdx = 0; drIdx < targetDrs.length; drIdx++) {
                    let sourceDr = sourceDrs[drIdx];
                    let compartments = sourceDr['controls'].Compartments.getRawValue();
                    let targetDr = targetDrs[drIdx];
                    let targetDrCompArray = targetDr.controls['Compartments'];
                    let delRequestUpdate = this.checkDeliveryRequestStatus(targetDr.controls['TrackScheduleStatus'].value, targetDr.controls['TrackScheduleEnrouteStatus'].value);
                    if (delRequestUpdate == false) {
                        targetDrCompArray.clear();
                        for (var cIdx = 0; cIdx < compartments.length; cIdx++) {
                            let compartment = compartments[cIdx];
                            targetDrCompArray.push(this.utilService.getCompartmentsFormGroupForDR(compartment));
                            if (sourceDr['controls'].IsRetainFuelLoaded.value && sourceDr['controls'].PostLoadedFor) {
                                targetDr.controls["IsRetainFuelLoaded"].patchValue(sourceDr['controls'].IsRetainFuelLoaded.value);
                                this.setDrValues(targetDr, sourceDr['controls'].RequiredQuantity.value, 1);
                                this.setPostLoadedFor(targetDr, sourceDr['controls'].PostLoadedFor.value);
                                this.setDrPickupLocation(targetDr, sourceDr.value);
                            }
                            targetDr.controls['ScheduleStatus'].patchValue(15);
                            tripInfo = new ModifiedTripInfo();
                            tripInfo.ShiftIndex = targetLoad['controls']['ShiftIndex'].value;
                            tripInfo.DriverRowIndex = targetLoad['controls']['DriverRowIndex'].value;
                            tripInfo.DriverColIndex = targetLoad['controls']['DriverColIndex'].value;
                        }
                    }
                }
                if (tripInfo) {
                    modifiedTrips.push(tripInfo);
                }
                var pretripInfo = this.intializePrePostLoadCompartmentInfo(targetLoad, 1);
                if (pretripInfo) {
                    modifiedTrips.push(pretripInfo);
                }
                //var posttripInfo = this.intializePreLoadCompartmentInfo(targetLoad); // not required - due to not provide edit value on postload DR.
                //if (posttripInfo) {
                //    modifiedTrips.push(posttripInfo);
                //}
            }
            Declarations.closeSlidePanel();
        }
        return modifiedTrips;
    }
    validateDrQuantity(drQty) {
        drQty.updateValueAndValidity();
        this.changeDetectorRef.detectChanges();
    }
    trackByShiftId(index, shift) {
        return shift.controls["Id"].value;
    }
    trackByScheduleIndex(index, schedule) {
        return index;
    }
    trackByTripIndex(index, schedule) {
        return index;
    }
    trackByDrId(index, dr) {
        return dr.controls["Id"].value;
    }
    trackByDriverId(index, driver) {
        return driver.controls["Id"].value;
    }
    trackByTrailerId(index, trailer) {
        return trailer.controls["TrailerId"].value;
    }
    intializePrePostLoadCompartmentInfo(targetLoad, prepostDRStatus) {
        var tripInfo = null;
        let updatedPrePostloadedDrs = targetLoad['controls']['DeliveryRequests'].value;
        if (updatedPrePostloadedDrs.length > 0) {
            var prepostloadedDrsStatus = null;
            if (prepostDRStatus) {
                prepostloadedDrsStatus = updatedPrePostloadedDrs.filter(x => x.PostLoadInfo);
            }
            else {
                prepostloadedDrsStatus = updatedPrePostloadedDrs.filter(x => x.PreLoadInfo);
            }
            if (prepostloadedDrsStatus.length > 0) {
                tripInfo = this.updateCompartmentPostloadDrs(prepostloadedDrsStatus, prepostDRStatus);
            }
        }
        return tripInfo;
    }
    TrailerRetainDetails(TrailerId) {
        var Id = [TrailerId];
        this.CompartmentDetails = [];
        this._loadingCmprts = true;
        this.sbService.getTrailerFuelRetain(Id).subscribe(data => {
            this.CompartmentDetails = data;
            this._loadingCmprts = false;
            this.changeDetectorRef.detectChanges();
        });
    }
    closeRetainInfo() {
        this.CompartmentDetails = [];
    }
    GetfuelRetainDetails(trailerIds) {
        this.sbService.getTrailerFuelRetain(trailerIds).subscribe(data => {
            if (data != null && data.length > 0) {
                var trailerFuelRetainInfo = data;
                var trailerNames = '';
                trailerFuelRetainInfo.forEach(x => {
                    if (trailerNames == '') {
                        trailerNames = x.TrailerId;
                    }
                    else {
                        trailerNames = trailerNames + ", " + x.TrailerId;
                    }
                });
                Declarations.msgwarning("There is some fuel retained in the " + trailerNames + ".", undefined, undefined);
            }
        });
    }
    checkIfTrailerFuelRetainExists(trailerId) {
        let status = false;
        var trailerInfo = this.TrailerCompartmentRetains.find(x => x.TrailerId.toString() == trailerId);
        if (trailerInfo != null) {
            status = trailerInfo.IsFuelRetain;
        }
        return status;
    }
    checkDeliveryRequestStatus(delStatus, delEncStatus) {
        if (delEncStatus == 16 || this.CompletedScheduleStatuses.filter(x => x == delStatus).length > 0) {
            return true;
        }
        return false;
    }
    toggleFilldCompatible() {
        this.SelectedTrailers = [];
        this.UnassignedDrivers = [];
        this.DriverTrailerForm.get('Driver').patchValue(null);
        this.UnassignedTrailers = this.getUnassignedTrailers(this.selTrailerIndex, this.selTrailerlist);
        this.UnassignedDrivers = this.getUnassignedDrivers(true);
    }
    getRetainCompartmentInfo(comp, dr) {
        let compartmentCtrl = comp.value;
        let delRequestCtrl = dr.value;
        var fuelRetain = this.TrailerCompartmentRetains.find(x => x.IsFuelRetain == true && x.TrailerId == compartmentCtrl.TrailerId);
        if (fuelRetain) {
            var compartment = fuelRetain.Compartments.find(x => x.CompartmentId == compartmentCtrl.CompartmentId);
            if (compartment.RetainInfo) {
                comp.controls["Quantity"].patchValue(compartment.RetainInfo.Quantity);
                dr.controls["IsRetainFuelLoaded"].patchValue(true);
                let drQty = this.getDRTotalQuantity(dr);
                this.setDrValues(dr, drQty, 1);
                this.setPostLoadedFor(dr, compartment.RetainInfo.DeliveryReqId);
                this.setDrPickupLocation(dr, {
                    PickupLocationType: compartment.RetainInfo.PickupLocationType,
                    Terminal: compartment.RetainInfo.TfxTerminal,
                    BulkPlant: compartment.RetainInfo.TfxBulkPlant
                });
            }
            else {
                this.resetQuantity(dr, delRequestCtrl);
            }
        }
        else {
            this.resetQuantity(dr, delRequestCtrl);
        }
        delRequestCtrl = dr.value;
        if (delRequestCtrl.ScheduleQuantityType <= 1) {
            let quantityValidators = [Validators.required, Validators.pattern(/^([0-9]\d*(\.\d+)?)$/), Validators.min(0.00000001), Validators.max(delRequestCtrl.RequiredQuantity)];
            comp.controls['Quantity'].setValidators(quantityValidators);
            comp.updateValueAndValidity();
        }
        this.changeDetectorRef.detectChanges();
    }
    getDRTotalQuantity(dr) {
        let drValue = dr.value;
        let quantity = 0;
        let assignedComps = drValue.Compartments.map(x => { return { TrailerId: x.TrailerId, CompartmentId: x.CompartmentId }; });
        for (var i = 0; i < this.TrailerCompartmentRetains.length; i++) {
            let trailer = this.TrailerCompartmentRetains[i];
            for (var j = 0; j < trailer.Compartments.length; j++) {
                let comp = trailer.Compartments[j];
                let assigned = assignedComps.find(x => x.TrailerId == trailer.TrailerId && x.CompartmentId == comp.CompartmentId);
                if (assigned && comp.RetainInfo) {
                    quantity += comp.RetainInfo.Quantity;
                }
            }
        }
        return quantity;
    }
    resetQuantity(dr, delRequestCtrl) {
        if (delRequestCtrl.ScheduleQuantityTypeText == 'Not Specified') {
            this.setDrValues(dr, 0, 5);
        }
        else if (delRequestCtrl.ScheduleQuantityTypeText == 'Small Compartment') {
            this.setDrValues(dr, 0, 4);
        }
        else if (delRequestCtrl.ScheduleQuantityTypeText == 'Full Load') {
            this.setDrValues(dr, 0, 3);
        }
        else if (delRequestCtrl.ScheduleQuantityTypeText == 'Balance') {
            this.setDrValues(dr, 0, 2);
        }
        else {
            this.setDrValues(dr, delRequestCtrl.RequiredQuantity, 1);
        }
        dr.controls["IsRetainFuelLoaded"].patchValue(false);
        let drQuantity = dr.controls["DrQuantity"].value;
        dr.patchValue({ IsRetainFuelLoaded: false, RequiredQuantity: drQuantity });
    }
    setDrValues(dr, quantity, type) {
        dr.patchValue({
            ScheduleQuantityType: type,
            RequiredQuantity: quantity
        });
    }
    setDrPickupLocation(dr, source) {
        if (source.PickupLocationType == 2 && source.BulkPlant) {
            dr.patchValue({
                BulkPlant: source.BulkPlant,
                PickupLocationType: source.PickupLocationType,
            });
        }
        else if (source.Terminal) {
            dr.patchValue({
                Terminal: source.Terminal,
                PickupLocationType: source.PickupLocationType,
            });
        }
    }
    setPostLoadedFor(dr, drId) {
        if (dr.controls['PostLoadedFor']) {
            dr.controls['PostLoadedFor'].patchValue(drId);
        }
        else {
            dr.addControl("PostLoadedFor", this.fb.control(drId));
        }
    }
    validatePrePostLoadTrailer(_form) {
        var status = true;
        var trailerDetails = this.DriverTrailerForm.controls['Trailers'].value;
        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '');
        var assignedTrailer = schedule.get('Trailers').value;
        var trips = schedule.controls['Trips'];
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        var PreLoadInfoLength = _deliveryRequests.filter(x => x.PreLoadedFor).length;
        var PostLoadInfoLength = _deliveryRequests.filter(x => x.PostLoadedFor).length;
        if (PostLoadInfoLength > 0 || PreLoadInfoLength > 0) {
            for (var i = 0; i < assignedTrailer.length; i++) {
                if (trailerDetails.findIndex(x => x.Id == assignedTrailer[i].Id) == -1) {
                    Declarations.msgerror('There is Preload/Postload DS in the shift. Please remove the Preload/Postload in order to change the assignment.', undefined, undefined);
                    status = false;
                    return status;
                }
            }
        }
        return status;
    }
    deleteDrsForRoute(route) {
        let deleteRouteInfo = {
            route: route,
            SelectedTrip: this.SelectedTrip
        };
        this.dataService.setDeleteRouteDetailsSubject(deleteRouteInfo);
    }
    hideDeliveryGroup() {
        this.dataService.setHideDeliveryGroupSubject(true);
    }
    createLoad(x) {
        if (x.IsDragFromLoad == true) {
            this.DropDelReqToLoad(x.Schedule, x.Trip, x.DrData, x.Drs);
            return;
        }
        if (x && x.Drs.length > 0) {
            let drs = x.Drs;
            var recurringDeliveryRequests = drs.filter(top => top.isRecurringSchedule == true);
            recurringDeliveryRequests = recurringDeliveryRequests.filter((el, i, a) => i === a.indexOf(el));
            var deliveryRequests = sortBy(drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && (top.CarrierStatus == 0 || top.CarrierStatus == 3 || top.CarrierStatus == 4)), 'ProductType');
            var brokeredDeliveryRequests = drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && top.CarrierStatus == 2);
            var splitDeliveryRequests = drs.filter(top => top.GroupParentDRId != '');
            var groupedDeliveryRequests = groupDrsByProduct(deliveryRequests);
            recurringDeliveryRequests.forEach(x => {
                groupedDeliveryRequests.push(x);
            });
            if (groupedDeliveryRequests.length == 0) {
                brokeredDeliveryRequests.forEach(x => {
                    groupedDeliveryRequests.push(x);
                });
            }
            if (groupedDeliveryRequests.length == 0) {
                splitDeliveryRequests.forEach(x => {
                    groupedDeliveryRequests.push(x);
                });
            }
            groupedDeliveryRequests.slice();
            let drIds = deliveryRequests.filter(t => t.isRecurringSchedule == false && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4)).map(m => m.Id);
            var input = { jobId: x.JobId, regionId: x.RegionId, customer: x.Customer, deliveryReqs: groupedDeliveryRequests, existingDrIds: drIds };
            this.sbService.getDeliveryReqDemands(input).subscribe(response => {
                if (response == null) {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                else if (response.Status && response.Status.StatusCode != 0) {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    Declarations.msgerror(response.Status.StatusMessage, undefined, undefined);
                }
                else {
                    if (response.Data && response.Data.length > 0) {
                        let isCommonPickup = x.Trip.get('IsCommonPickup').value;
                        let IsDispatcherDragDropSequence = x.Trip.get('IsDispatcherDragDropSequence').value;
                        x.DrData.Data = new FormArray([]);
                        x.DrData.Data = this.utilService.getDeliveryRequestFormArray(response.Data, isCommonPickup, IsDispatcherDragDropSequence, 0);
                        x.Drs.forEach((req, i) => {
                            if (response.Data.findIndex(t => t.Id == req.Id || (t.ProductTypeId == req.ProductTypeId
                                && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4))) == -1) {
                                x.Drs.splice(i, 1);
                            }
                        });
                        this.DropDelReqToLoad(x.Schedule, x.Trip, x.DrData, x.Drs);
                    }
                    else {
                        this._savingBuilder = false;
                        this.changeDetectorRef.detectChanges();
                    }
                }
            });
        }
    }
};
__decorate([
    Input()
], DriverViewComponent.prototype, "SbForm", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "Shifts", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "RegionDetail", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "SelectedRegionId", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "IsTrailerExists", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "DriverViewFilter", void 0);
__decorate([
    Input()
], DriverViewComponent.prototype, "HeaderToggle", void 0);
__decorate([
    ViewChildren(DriverScheduleComponent)
], DriverViewComponent.prototype, "driverSchedules", void 0);
DriverViewComponent = __decorate([
    Component({
        selector: 'app-driver-view',
        templateUrl: './driver-view.component.html',
        styleUrls: ['./driver-view.component.css'],
        changeDetection: ChangeDetectionStrategy.OnPush
    })
], DriverViewComponent);
export { DriverViewComponent };
//# sourceMappingURL=driver-view.component.js.map