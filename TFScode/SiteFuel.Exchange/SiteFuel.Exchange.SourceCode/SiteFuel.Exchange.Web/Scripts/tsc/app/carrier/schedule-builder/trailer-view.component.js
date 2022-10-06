import { __decorate } from "tslib";
import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { Validators } from '@angular/forms';
import { TrailerShiftModel, TripModel } from '../models/DispatchSchedulerModels';
import { Subscription, BehaviorSubject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { Declarations } from 'src/app/declarations.module';
import { CustomAbstractControls } from '../customAbstractControl';
import * as moment from 'moment';
import { DeliveryGroupStatus, DeliveryReqStatus, TripStatus } from 'src/app/app.enum';
let TrailerViewComponent = class TrailerViewComponent {
    constructor(fb, sbService, dataService, chatService, changeDetectorRef) {
        this.fb = fb;
        this.sbService = sbService;
        this.dataService = dataService;
        this.chatService = chatService;
        this.changeDetectorRef = changeDetectorRef;
        this.Trailers = [];
        this._isDriverFound = false;
        this._loadingRequests = false;
        this._savingBuilder = false;
        this._publishedRequestExists = false;
        this.SelectedDriverName = '';
        this.SelectedTrailers = [];
        this.UnassignedTrailers = [];
        this.UnassignedDrivers = [];
        this.TrailerDdlSettings = {};
        this.TrailerShifts = [];
        this.TrailerShiftsDdlSettings = {};
        this.SelectedTrailerShifts = {};
        this.Collapsed = [];
        this.CollapsedIcons = [];
        this.CompletedScheduleStatuses = [7, 8, 9, 10];
        this.OnTheWayScheduleStatuses = [1, 3, 9, 11, 12, 13, 15, 16, 17, 18, 19, 20];
        this.FormChangeSubscription = new Subscription();
        this.disableControl = false;
        this._otherRegionDriverSubject = new BehaviorSubject(false);
    }
    ngOnInit() {
        this.DriverTrailerForm = this.initDriverTrailerForm();
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
        this.TrailerShiftsDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };
        this.subscribeTrailerShiftsSubject();
        this.dataService.setShowDeliveryGroupSubject(false);
        this.subscribeDisableControlsSubject();
    }
    ngOnChanges(change) {
        if (change.Trailers && change.Trailers.currentValue) {
            this.unsubscribeFormChange();
            this.dataService.setShowDeliveryGroupSubject(false);
            this.initTrailers(this.Trailers);
            this.subscribeDisableControlsSubject();
        }
    }
    ngAfterViewInit() {
        this.dataService.setUnsavedChangesSubject(false);
    }
    ngOnDestroy() {
        this.unsubscribeFormChange();
        this.unsubscribeDeliveryGroupEvents();
        var _trailers = this.SbForm.get('Trailers');
        if (_trailers) {
            _trailers.clear(); // Clear shifts of driver view
        }
        if (this.TrailerShiftsSubjectSubscription) {
            this.TrailerShiftsSubjectSubscription.unsubscribe();
        }
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
    }
    subscribeDeliveryGroupEvents() {
        this.subscribeDraftDeliveryGroupSubject();
        this.subscribePublishDeliveryGroupSubject();
        this.subscribeCancelDeliveryGroupSubject();
        this.subscribeDeleteDeliveryGroupSubject();
    }
    unsubscribeFormChange() {
        if (this.FormChangeSubscription) {
            this.dataService.setUnsavedChangesSubject(false);
            this.FormChangeSubscription.unsubscribe();
        }
    }
    unsubscribeDeliveryGroupEvents() {
        if (this.DraftDeliveryGroupSubscription) {
            this.DraftDeliveryGroupSubscription.unsubscribe();
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
    subscribeFormChange() {
        this.unsubscribeFormChange();
        window.setTimeout(() => {
            let _trailers = this.SbForm.get('Trailers');
            let _initialValues = JSON.stringify(_trailers.value);
            this.FormChangeSubscription = _trailers.valueChanges.pipe(debounceTime(500)).subscribe(x => {
                if (x && x.length > 0) {
                    let _currentValues = JSON.stringify(x);
                    if (_initialValues == _currentValues) {
                        this.dataService.setUnsavedChangesSubject(false);
                    }
                    else {
                        this.dataService.setUnsavedChangesSubject(true);
                    }
                }
            });
        }, 200);
    }
    subscribeDraftDeliveryGroupSubject() {
        this.DraftDeliveryGroupSubscription = this.dataService.DraftDeliveryGroupSubject.subscribe(x => {
            this.draftScheduleBuilder(x.trip, x.filterChanged);
        });
    }
    subscribePublishDeliveryGroupSubject() {
        this.PublishDeliveryGroupSubscription = this.dataService.PublishDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.publishScheduleBuilder(x.shiftIndex, x.rowIndex, x.colIndex, x.schedule, x.trip);
            }
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
    subscribeTrailerShiftsSubject() {
        this.TrailerShiftsSubjectSubscription = this.dataService.TrailerShiftsSubject.subscribe(x => {
            if (x) {
                this.TrailerShifts = x.map(x => {
                    return {
                        Id: x.Id,
                        Name: 'Shift ' + x.StartTime + ' - ' + x.EndTime,
                        StartTime: x.StartTime,
                        EndTime: x.EndTime,
                        SlotPeriod: x.SlotPeriod
                    };
                });
            }
        });
    }
    subscribeDisableControlsSubject() {
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }
    initTrailers(trailers) {
        this.SelectedTrailerShifts = {};
        var _trailerArray = this.SbForm.get('Trailers');
        _trailerArray.clear();
        trailers.forEach((x, index) => {
            this.SelectedTrailerShifts[x.Id] = x.Shifts.map(y => { return { Id: y.ShiftId, Name: 'Shift ' + y.StartTime + ' - ' + y.EndTime }; });
            _trailerArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                TrailerId: this.fb.control(x.TrailerId),
                StartTime: this.fb.control(x.StartTime),
                EndTime: this.fb.control(x.EndTime),
                Compartments: this.fb.control(x.Compartments),
                TrailerType: this.fb.control(x.TrailerType),
                Shifts: this.getShiftsFormArray(x.Shifts || [], index),
                SelectedShifts: this.fb.control(this.SelectedTrailerShifts[x.Id]),
                IsCollapsed: this.fb.control(x.IsCollapsed)
            }));
            this.Collapsed.push((x.IsCollapsed ? '' : 'in'));
            this.CollapsedIcons.push((x.IsCollapsed ? 'collapsed' : ''));
        });
    }
    initDriverTrailerForm() {
        var _dtForm = this.fb.group({
            Driver: this.fb.control(null, [Validators.required]),
            Trailers: this.fb.control([], [Validators.required]),
            Index1: this.fb.control(0),
            Index2: this.fb.control(0),
            Index3: this.fb.control(0)
        });
        return _dtForm;
    }
    getShiftsFormArray(shifts, trailerIndex) {
        var _sArray = this.fb.array([]);
        shifts.forEach((x, index) => {
            _sArray.push(this.getShiftFormGroup(x, trailerIndex, index));
        });
        return _sArray;
    }
    getShiftFormGroup(x, trailerIndex, rowIdx) {
        let _sForm = this.fb.group({
            ShiftId: this.fb.control(x.ShiftId),
            StartTime: this.fb.control(x.StartTime),
            EndTime: this.fb.control(x.EndTime),
            SlotPeriod: this.fb.control(x.SlotPeriod),
            Trips: this.getTripsFormArray(x.Trips, trailerIndex, rowIdx)
        });
        return _sForm;
    }
    getTripsFormArray(trips, trailerIndex, rowIndex) {
        var _tArray = this.fb.array([]);
        trips.forEach((x, index) => {
            _tArray.push(this.getTripFormGroup(x, trailerIndex, rowIndex, index));
        });
        return _tArray;
    }
    getTripFormGroup(x, trailerIndex, rowIndex, colIndex) {
        var _tForm = this.fb.group({
            TripId: this.fb.control(x.TripId),
            GroupId: this.fb.control(x.GroupId),
            DeliveryRequests: this.getDeliveryRequestFormArray(x.DeliveryRequests, x.IsCommonPickup),
            StartDate: this.fb.control(x.StartDate),
            StartTime: this.fb.control(x.StartTime, Validators.required),
            EndTime: this.fb.control(x.EndTime, Validators.required),
            LoadCode: this.fb.control(x.LoadCode),
            RouteInfo: this.fb.control(x.RouteInfo),
            SupplierSource: this.fb.control(x.SupplierSource),
            Carrier: this.fb.control(x.Carrier),
            TripStatus: this.fb.control(x.TripStatus),
            TripPrevStatus: this.fb.control(x.TripPrevStatus),
            DeliveryGroupStatus: this.fb.control(x.DeliveryGroupStatus),
            DeliveryGroupPrevStatus: this.fb.control(x.DeliveryGroupPrevStatus),
            PickupLocationType: this.fb.control(x.PickupLocationType),
            IsCommonPickup: this.fb.control(x.IsCommonPickup),
            Terminal: this.getTerminalForm(x.Terminal, x.IsCommonPickup && x.PickupLocationType != 2),
            BulkPlant: this.getBulkPlantForm(x.BulkPlant, x.IsCommonPickup && x.PickupLocationType == 2),
            ShiftIndex: this.fb.control(trailerIndex),
            Drivers: this.getDriversFormArray(x.Drivers),
            Trailers: this.getTrailersFormArray(x.Trailers),
            DriverRowIndex: this.fb.control(x.DriverRowIndex),
            DriverColIndex: this.fb.control(x.DriverColIndex),
            TrailerRowIndex: this.fb.control(x.TrailerRowIndex),
            TrailerColIndex: this.fb.control(x.TrailerColIndex),
            IsEditable: this.fb.control(x.IsEditable),
            DriverScheduleMappingId: this.fb.control(x.DriverScheduleMappingId),
            BadgeNo1: this.fb.control(x.BadgeNo1),
            BadgeNo2: this.fb.control(x.BadgeNo2),
            BadgeNo3: this.fb.control(x.BadgeNo3),
            IsCommonBadge: this.fb.control(x.IsCommonBadge),
            IsPreloadDisable: this.fb.control(x.IsPreloadDisable),
        });
        return _tForm;
    }
    getDriversFormArray(drivers) {
        var _dArray = this.fb.array([]);
        drivers.forEach(x => {
            _dArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                Code: this.fb.control(x.Code),
                Name: this.fb.control(x.Name)
            }));
        });
        return _dArray;
    }
    getTrailersFormArray(Trailers) {
        var _tArray = this.fb.array([]);
        Trailers.forEach(x => {
            _tArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                TrailerId: this.fb.control(x.TrailerId),
                StartTime: this.fb.control(x.StartTime),
                EndTime: this.fb.control(x.EndTime),
                Compartments: this.fb.control(x.Compartments),
                TrailerType: this.fb.control(x.TrailerType)
            }));
        });
        return _tArray;
    }
    getDeliveryRequestFormArray(delReqs, isCommonPickup) {
        var _drArray = this.fb.array([], Validators.required);
        delReqs.forEach(x => {
            var _form = this.getDeliveryRequestForm(x, isCommonPickup);
            _drArray.push(_form);
        });
        return _drArray;
    }
    getDeliveryRequestForm(delReq, isCommonPickup) {
        var _drForm = this.fb.group({
            Id: this.fb.control(''),
            JobId: this.fb.control(''),
            JobAddress: this.fb.control(''),
            JobCity: this.fb.control(''),
            JobName: this.fb.control(''),
            ProductType: this.fb.control(''),
            ProductTypeId: this.fb.control(0),
            SiteId: this.fb.control(''),
            RequiredQuantity: this.fb.control('', [Validators.required,]),
            Priority: this.fb.control(''),
            TankId: this.fb.control(''),
            StorageId: this.fb.control(''),
            AssignedToCompanyId: this.fb.control(null),
            CreatedByCompanyId: this.fb.control(null),
            SupplierCompanyId: this.fb.control(null),
            Status: this.fb.control(0),
            PreviousStatus: this.fb.control(0),
            ScheduleStatus: this.fb.control(0),
            SchedulePreviousStatus: this.fb.control(0),
            OrderId: this.fb.control('', Validators.required),
            CreatedByRegionId: this.fb.control(''),
            AssignedToRegionId: this.fb.control(''),
            PickupLocationType: this.fb.control(0),
            Terminal: this.getTerminalForm(delReq.Terminal, !isCommonPickup && delReq.PickupLocationType != 2),
            BulkPlant: this.getBulkPlantForm(delReq.BulkPlant, !isCommonPickup && delReq.PickupLocationType == 2),
            UoM: this.fb.control(''),
            DeliveryGroupId: this.fb.control(null),
            DeliveryScheduleId: this.fb.control(null),
            TrackableScheduleId: this.fb.control(null),
            CustomerCompany: this.fb.control(''),
            TrackScheduleEnrouteStatus: this.fb.control(null),
            TrackScheduleStatus: this.fb.control(null),
            TrackScheduleStatusName: this.fb.control(null),
            StatusClassId: this.fb.control(null),
            IsAutoCreatedDR: this.fb.control(''),
            ParentId: this.fb.control(null),
            BadgeNo1: this.fb.control(''),
            BadgeNo2: this.fb.control(''),
            BadgeNo3: this.fb.control(''),
            IsCommonBadge: this.fb.control(true),
            DispactherNote: this.fb.control(''),
            RouteInfo: this.fb.control(null),
            isRecurringSchedule: this.fb.control(false),
            RecurringScheduleId: this.fb.control(''),
            //BLENDED REQUEST
            //IsBlendedRequest: this.fb.control(false),
            //BlendedRequests: this.fb.array([]),
            //IsCommonPickupForBlend: this.fb.control(false),
            //BlendedGroupId: this.fb.control(getUniqueNumber()),
        });
        if (delReq != null && delReq != undefined) {
            _drForm.patchValue(delReq);
        }
        return _drForm;
    }
    getTerminalForm(terminal, isTerminalPickup) {
        var _tform = this.fb.group({
            Id: this.fb.control(''),
            Name: this.fb.control('', (isTerminalPickup ? Validators.required : null))
        });
        if (isTerminalPickup && terminal) {
            _tform.patchValue(terminal);
        }
        return _tform;
    }
    getBulkPlantForm(bulkPlant, isLocationPickup) {
        var _bform = this.fb.group({
            Address: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            City: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            State: this.fb.group({ Id: this.fb.control('', (isLocationPickup ? Validators.required : null)), Code: this.fb.control('') }),
            Country: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('', (isLocationPickup ? Validators.required : null)) }),
            ZipCode: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            CountyName: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            Latitude: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            Longitude: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            SiteName: this.fb.control('', (isLocationPickup ? Validators.required : null)),
            SiteId: this.fb.control('')
        });
        if (isLocationPickup && bulkPlant) {
            _bform.patchValue(bulkPlant);
        }
        return _bform;
    }
    onItemDrop(trip, event, trailerIndex, rowIndex, colIndex, shift) {
        var drData = event.dragData;
        let drDataCopied = JSON.parse(JSON.stringify(drData.Data));
        //drData.Data = groupDrsByProduct(drData.Data);
        let isCommonPickup = trip.get('IsCommonPickup').value;
        drData.Data = this.getDeliveryRequestFormArray(drData.Data, isCommonPickup);
        if (drData.TripFrom && (drData.TripFrom.controls['TripId'].value != null || trip.controls['TripId'].value != null) && drData.TripFrom.controls['TripId'].value == trip.controls['TripId'].value) {
            return;
        }
        if (drData.TripFrom && (drData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || trip.get('DeliveryGroupPrevStatus').value == 2)) {
            this.draggedDelReqData = event.dragData;
            this.droppedTrip = { Trip: trip, TrailerIndex: trailerIndex, RowIndex: rowIndex, ColIndex: colIndex, Shift: shift };
            if (this.draggedDelReqData.Data.get('TrackableScheduleId').value != null && this.draggedDelReqData.Data.get('TrackableScheduleId').value > 0) {
                var scheduleIds = [];
                scheduleIds.push(this.draggedDelReqData.Data.get('TrackableScheduleId').value);
                this.dragDelReqToAnotherLoad(scheduleIds);
            }
            else {
                jQuery('#deleteDrHeading').html('Changes will reflect for already published load(s).');
                jQuery('#btnconfirm-dragdrop-dr').click();
            }
        }
        else {
            this.DropDelReqToLoad(trip, drData, trailerIndex, rowIndex, colIndex, shift);
            this.dataService.setUnsavedChangesSubject(true);
            //this.checkDelReqStatus(trip, drData, trailerIndex, rowIndex, colIndex, shift);
        }
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
                        jQuery('#deleteDrHeading').html('Changes will reflect for already published load(s).');
                        jQuery('#btnconfirm-dragdrop-dr').click();
                    }
                    else {
                        this.dragDelReqToAnotherLoadYes();
                    }
                }
            }
            else {
                if (this.draggedDelReqData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || this.draggedDelReqData.get('DeliveryGroupPrevStatus').value == 2) {
                    jQuery('#deleteDrHeading').html('Changes will reflect for already published load(s).');
                    jQuery('#btnconfirm-dragdrop-dr').click();
                }
                else {
                    this.dragDelReqToAnotherLoadYes();
                }
            }
        });
    }
    DropDelReqToLoad(trip, drData, trailerIndex, rowIndex, colIndex, shift) {
        let isCommonPickup = trip.get('IsCommonPickup').value;
        var _drArray = trip.get('DeliveryRequests');
        var _drFormArray = drData.Data;
        _drFormArray.controls.forEach((_drForm) => {
            if (isCommonPickup) {
                _drForm.controls['Terminal'].disable();
                _drForm.controls['BulkPlant'].disable();
            }
            _drForm.controls['ScheduleStatus'].setValue(14);
            _drForm.controls['Status'].setValue(5);
            _drArray.push(_drForm);
        });
        if (drData.TripFrom != undefined && drData.TripFrom != null) {
            var drInFromTrip = drData.TripFrom.controls['DeliveryRequests'];
            for (let idx = 0; idx < _drFormArray.controls.length; idx++) {
                let drIndex = drInFromTrip.controls.findIndex((x) => {
                    return x.controls['Id'].value == _drFormArray.controls[idx]['controls'].Id.value;
                });
                drInFromTrip.removeAt(drIndex);
            }
            this.draftScheduleBuilder(trip);
        }
        else {
            this.dataService.setRemoveDroppedRequestSubject(drData.Data.value);
            this.editDroppedGroup(trip, trailerIndex, rowIndex, colIndex, shift, shift.get('Trips')['controls'][colIndex - 1], shift.get('Trips')['controls'][colIndex + 1]);
        }
    }
    dragDelReqToAnotherLoadYes() {
        let isPublish = false;
        let isPublishDragFromTrip = false;
        let isCommonPickup = this.droppedTrip.Trip.get('IsCommonPickup').value;
        var _drArray = this.droppedTrip.Trip.get('DeliveryRequests');
        var _drForm = this.draggedDelReqData.Data;
        if (isCommonPickup) {
            _drForm.get('Terminal').disable();
            _drForm.get('BulkPlant').disable();
        }
        _drArray.push(_drForm);
        let isDraggedFromLoad = this.draggedDelReqData != null && this.draggedDelReqData.TripFrom != undefined && this.draggedDelReqData.TripFrom != null;
        if (isDraggedFromLoad) {
            if (_drForm.get('PreviousStatus').value == 3 && this.droppedTrip.Trip.get('DeliveryGroupPrevStatus').value != 2) {
                _drForm.get('Status').setValue(5);
                _drForm.get('ScheduleStatus').setValue(14);
                _drForm.get('DeliveryGroupId').setValue(null);
                _drForm.get('DeliveryScheduleId').setValue(null);
                _drForm.get('TrackableScheduleId').setValue(null);
                _drForm.get('TrackScheduleStatus').setValue(0);
                _drForm.get('TrackScheduleEnrouteStatus').setValue(0);
                _drForm.get('TrackScheduleStatusName').setValue('');
                isPublishDragFromTrip = true;
                isPublish = true;
            }
            else if (_drForm.get('PreviousStatus').value != 3 && this.droppedTrip.Trip.get('DeliveryGroupPrevStatus').value == 2) {
                _drForm.get('Status').setValue(3);
                _drForm.get('ScheduleStatus').setValue(14);
                isPublish = true;
            }
            else if (_drForm.get('PreviousStatus').value == 3 && this.droppedTrip.Trip.get('DeliveryGroupPrevStatus').value == 2) {
                _drForm.get('Status').setValue(3);
                _drForm.get('ScheduleStatus').setValue(15);
                this.draggedDelReqData.TripFrom.get('DeliveryGroupStatus').setValue(DeliveryGroupStatus.Published);
                isPublish = true;
            }
            var drInFromTrip = this.draggedDelReqData.TripFrom.get('DeliveryRequests');
            drInFromTrip.removeAt(this.draggedDelReqData.DrIndex);
            if (drInFromTrip.length > 0) {
                this.draggedDelReqData.TripFrom.get('TripStatus').setValue(TripStatus.Modified);
            }
            else {
                this.draggedDelReqData.TripFrom.get('TripStatus').setValue(TripStatus.Deleted);
                this.SbForm.get('DeletedTripId').setValue(this.draggedDelReqData.TripFrom.get('TripId').value);
                this.SbForm.get('DeletedGroupId').setValue(this.draggedDelReqData.TripFrom.get('GroupId').value);
                let reserveValue = {
                    StartDate: this.draggedDelReqData.TripFrom.get('StartDate').value,
                    StartTime: this.draggedDelReqData.TripFrom.get('StartTime').value,
                    EndTime: this.draggedDelReqData.TripFrom.get('EndTime').value,
                    Carrier: this.draggedDelReqData.TripFrom.get('Carrier').value
                };
                this.draggedDelReqData.TripFrom.reset();
                this.draggedDelReqData.TripFrom.get('DeliveryRequests').clear();
                this.draggedDelReqData.TripFrom.get('Drivers').clear();
                this.draggedDelReqData.TripFrom.reset(reserveValue);
                if (isPublishDragFromTrip) {
                    isPublish = false;
                }
                isPublishDragFromTrip = false;
            }
        }
        this.dataService.setUnsavedChangesSubject(true);
        if (isPublish) {
            if (isPublishDragFromTrip) {
                this.publishScheduleBuilder(this.draggedDelReqData.TrailerIndex, this.draggedDelReqData.RowIndex, this.draggedDelReqData.ColIndex, this.draggedDelReqData.Shift, this.draggedDelReqData.TripFrom);
            }
            else {
                this.publishScheduleBuilder(this.droppedTrip.TrailerIndex, this.droppedTrip.RowIndex, this.droppedTrip.ColIndex, this.droppedTrip.Shift, this.droppedTrip.Trip);
            }
        }
        else {
            this.draftScheduleBuilder(this.droppedTrip.Trip);
        }
        this.droppedTrip = null;
        this.draggedDelReqData = null;
    }
    dragDelReqToAnotherLoadNo() {
        this.droppedTrip = null;
        this.draggedDelReqData = null;
    }
    resetLoad(data) {
        var trip = this.SbForm.get('Trailers.' + data.shiftIndex + '.Shifts.' + data.rowIndex + '.Trips.' + data.tripIndex);
        if (trip) {
            let reserveValue = {
                StartDate: trip.get('StartDate').value,
                StartTime: trip.get('StartTime').value,
                EndTime: trip.get('EndTime').value,
                Carrier: trip.get('Carrier').value,
                DriverRowIndex: trip.get('DriverRowIndex').value,
                DriverColIndex: trip.get('DriverColIndex').value,
                TrailerRowIndex: trip.get('TrailerRowIndex').value,
                TrailerColIndex: trip.get('TrailerColIndex').value,
                IsCommonBadge: trip.get('IsCommonBadge').value,
                BadgeNo1: trip.get('BadgeNo1').value,
                BadgeNo2: trip.get('BadgeNo2').value,
                BadgeNo3: trip.get('BadgeNo3').value,
                RouteInfo: trip.get('RouteInfo').value,
                IsPreloadDisable: trip.get('IsPreloadDisable').value,
            };
            trip.reset();
            trip.get('DeliveryRequests').clear();
            trip.get('Drivers').clear();
            trip.reset(reserveValue);
        }
    }
    deleteGroup(data) {
        this.dataService.setShowDeliveryGroupSubject(false);
        if (data.GroupId != null && data.GroupId > 0) {
            this._savingBuilder = true;
            this.changeDetectorRef.detectChanges();
            var scheduleIds = data.trip.value.DeliveryRequests.map(t => t.TrackableScheduleId);
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
                this.changeDetectorRef.detectChanges();
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
    deleteLoad(data) {
        let driverScheduleMappingId = data.trip.controls.DriverScheduleMappingId.value;
        this.resetLoad(data);
        if (data.TripId != null && data.TripId != undefined && data.TripId != '') {
            var sbModel = this.SbForm.value;
            sbModel.DeletedGroupId = data.GroupId;
            sbModel.DeletedTripId = data.TripId;
            sbModel.IsLoadReset = data.isReset;
            sbModel.DeletedDriverScheduleMappingId = driverScheduleMappingId;
            this._savingBuilder = true;
            this.sbService.deleteGroupTrailerView(sbModel)
                .subscribe(response => {
                if (response != null) {
                    this.dataService.setRestoreDeletedRequestSubject(data.Deleted);
                    this.updateScheduleBuilderForm(response);
                    this.unsubscribeFormChange();
                }
                else {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                this._savingBuilder = false;
            });
        }
        else {
            this.dataService.setRestoreDeletedRequestSubject(data.Deleted);
        }
    }
    getAssignedDrivers(shiftIdx) {
        var _drivers = [];
        var _shift = this.SbForm.get('Shifts.' + shiftIdx);
        if (_shift != null && _shift != undefined) {
            var _schArray = _shift.get('Schedules');
            _schArray.controls.forEach(sc => {
                var _dArray = sc.get('Drivers');
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
    getUnassignedDrivers(drivers, currentDriverId) {
        if (drivers && drivers.length > 0) {
            let shiftIdx = this.DriverTrailerForm.get('Index1').value;
            let assigned = this.getAssignedDrivers(shiftIdx);
            assigned = assigned.filter(x => x.Id != currentDriverId);
            drivers = drivers.filter(this.IdComparer(assigned));
        }
        return drivers;
    }
    getUnassignedTrailers(shiftIdx, currentTrailers) {
        let assignedTrailers = this.getAssignedTrailers(shiftIdx);
        assignedTrailers = assignedTrailers.filter(this.IdComparer(currentTrailers));
        let _trailers = this.RegionDetail.Trailers.filter(this.IdComparer(assignedTrailers));
        return _trailers;
    }
    editDriverTrailers(index1, index2, index3, trailer, trip) {
        var driver = { Id: null, Name: '', Code: '' };
        if (trip.get('Drivers.0') != null) {
            driver = trip.get('Drivers.0').value;
        }
        this.SelectedTrailers = [trailer.value];
        this.DriverTrailerForm.patchValue({
            Index1: index1,
            Index2: index2,
            Index3: index3,
            Driver: driver.Id,
            Trailers: this.SelectedTrailers
        });
        this.UnassignedTrailers = this.getUnassignedTrailers(index1, this.SelectedTrailers);
        if (this.SelectedTrailers.length > 0) {
            this.getDriverdetails();
        }
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        this._publishedRequestExists = false;
        this.subscribeFormChange();
    }
    checkOtherRegionDriver() {
        if (this.DriverTrailerForm.get('Driver').value == "null") {
            this.DriverTrailerForm.get('Driver').setErrors({ 'incorrect': true });
        }
        this.DriverTrailerForm.markAllAsTouched();
        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var shiftId = this.SbForm.get('Trailers.' + _form.Index1 + '.Shifts.' + _form.Index2 + '.ShiftId').value;
            //newly added
            var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
            //this._savingBuilder = true;
            this.dataService.IsLoadingButtonSubject.next(true);
            this.sbService.getSelectedDateDriverScheduleByDriverId(_form.Driver, new Date(this.SbForm.controls.Date.value).toUTCString())
                .subscribe(data => {
                if (data && data.filter(f => f.ShiftId == shiftId).length > 0) {
                    var shift = this.SbForm.get('Trailers.' + _form.Index1 + '.Shifts.' + _form.Index2 + '');
                    this.checkForPublishedRequests(driverObj, shift, _form);
                    if (!this._publishedRequestExists) {
                        this.assignDriverToLoad(driverObj, shift, _form);
                    }
                }
                else {
                    this.SelectedDriverName = driverObj.Name;
                    //this._otherRegionDriver = true;
                    this._otherRegionDriverSubject.next(true);
                }
                //this._savingBuilder = false;
                this.dataService.IsLoadingButtonSubject.next(false);
            });
            //end
            //var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
            //var driverShifts = driverObj.Code.split(',');
            //if (driverShifts.indexOf(shiftId) == -1) {
            //	this.SelectedDriverName = driverObj.Name;
            //	this._otherRegionDriver = true;
            //} else {
            //	var shift = this.SbForm.get('Trailers.' + _form.Index1 + '.Shifts.' + _form.Index2 + '') as FormGroup;
            //	this.checkForPublishedRequests(driverObj, shift, _form);
            //	if (!this._publishedRequestExists) {
            //		this.assignDriverToLoad(driverObj, shift, _form);
            //	}
            //}
        }
    }
    checkForPublishedRequests(driverObj, schedule, _form) {
        //	this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        if (this.checkAnyPublishedTrip(schedule)) {
            this._publishedRequestExists = true;
        }
        else {
            this._publishedRequestExists = false;
        }
    }
    onPublishChangesNo() {
        this._publishedRequestExists = false;
    }
    onPublishChangesYes() {
        var _form = this.DriverTrailerForm.value;
        var schedule = this.SbForm.get('Trailers.' + _form.Index1 + '.Shifts.' + _form.Index2 + '');
        if (schedule != null && schedule != undefined) {
            var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
            this.assignDriverToLoad(driverObj, schedule, _form);
        }
    }
    onOtherRegionDriverNo() {
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
    }
    onOtherRegionDriverYes() {
        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var schedule = this.SbForm.get('Trailers.' + _form.Index1 + '.Shifts.' + _form.Index2);
            if (schedule != null && schedule != undefined) {
                var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
                this.checkForPublishedRequests(driverObj, schedule, _form);
                if (!this._publishedRequestExists) {
                    this.assignDriverToLoad(driverObj, schedule, _form);
                    let driverInfo = {
                        driverId: driverObj.Id,
                        regionId: this.SelectedRegionId
                    };
                    this.chatService.intializeChatDefault(driverInfo);
                }
            }
        }
    }
    assignDriverToLoad(driverObj, shift, _form) {
        Declarations.hideModal('#driverTrailerModel');
        if (driverObj != null && driverObj != undefined) {
            var _dArray = shift.get('Trips.' + _form.Index3 + '.Drivers');
            _dArray.clear();
            _dArray.push(this.fb.group({
                Id: this.fb.control(driverObj.Id),
                Name: this.fb.control(driverObj.Name),
                IsFilldCompatible: this.fb.control(driverObj.IsFilldCompatible)
            }));
        }
        var _tArray = shift.get('Trips.' + _form.Index3 + '.Trailers');
        _tArray.clear();
        _form.Trailers.forEach(x => {
            _tArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                TrailerId: this.fb.control(x.TrailerId),
                StartTime: this.fb.control(x.StartTime),
                EndTime: this.fb.control(x.EndTime),
                Compartments: this.fb.control(x.Compartments),
                TrailerType: this.fb.control(x.TrailerType)
            }));
        });
        var isPublish = false;
        var trips = shift.get('Trips');
        for (var i = 0, j = 1; i < trips.length; i++, j++) {
            if (trips[i] != null && trips[i] != undefined && trips[i].valid && trips[i].get('DeliveryGroupPrevStatus').value == 2) {
                this.setDeliveryGroupStatus(trips[i], DeliveryGroupStatus.Published);
                this.setDeliveryRequestStatus(trips[i], DeliveryReqStatus.ScheduleCreated);
                isPublish = true;
            }
            this.setTripStatus(trips[i]);
        }
        this.saveEntireRow(isPublish);
    }
    setTripStatus(trip) {
        if (trip != null && trip != undefined) {
            var tripPrevStatusId = trip.get('TripPrevStatus').value;
            var tripStatusId = TripStatus.Added;
            if (tripPrevStatusId == TripStatus.None) {
                tripStatusId = TripStatus.Added;
            }
            else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified) {
                tripStatusId = TripStatus.Modified;
            }
            trip.get('TripStatus').setValue(tripStatusId);
        }
    }
    setDeliveryGroupStatus(trip, statusId) {
        if (trip != null && trip != undefined) {
            trip.get('DeliveryGroupStatus').setValue(statusId);
        }
    }
    setDeliveryRequestStatus(trip, statusId) {
        if (trip != null && trip != undefined) {
            var deliveryRequests = trip.get('DeliveryRequests');
            for (var i = 0; i < deliveryRequests.length; i++) {
                deliveryRequests.controls[i].get('Status').setValue(statusId);
            }
        }
    }
    draftScheduleBuilder(trip, filterChanged = false) {
        this.unsubscribeDeliveryGroupEvents();
        if (trip != null && trip != undefined) {
            this.setTripStatus(trip);
            this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Draft);
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var sbModel = this.SbForm.value;
        this.dataService.setShowDeliveryGroupSubject(false);
        this.sbService.saveTrailerView(sbModel).subscribe(data => {
            this._savingBuilder = false;
            this.dataService.setUnsavedChangesSubject(false);
            if (filterChanged != true) {
                this.updateScheduleBuilderForm(data);
            }
            else {
                this.changeDetectorRef.markForCheck();
                this.changeDetectorRef.detectChanges();
            }
        });
    }
    cancelScheduleBuilder(i, j, k, trip) {
        let _thisTrip = this.SbForm.get('Trailers.' + i + '.Shifts.' + j + '.Trips.' + k);
        let _thisDrArray = _thisTrip.get('DeliveryRequests');
        _thisDrArray.clear();
        let oldTripValue = trip.value;
        trip.value.DeliveryRequests.forEach(x => {
            _thisDrArray.push(this.getDeliveryRequestForm(x, oldTripValue.IsCommonPickup));
        });
        _thisTrip.patchValue(trip.value);
        this.changeDetectorRef.detectChanges();
    }
    publishScheduleBuilder(i, j, k, shift, trip) {
        if (trip.value.GroupId != null && trip.value.GroupId > 0) {
            var deliveryRequests = trip.get('DeliveryRequests').value;
            var scheduleIds = deliveryRequests.filter(t => t.TrackableScheduleId != null && t.TrackableScheduleId > 0 && t.ScheduleStatus != 0).map(t => t.TrackableScheduleId);
            if (scheduleIds.length == 0) {
                this.publishLoad(i, j, k, shift, trip);
                return;
            }
            this.dataService.setShowDeliveryGroupSubject(false);
            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                if (response != null && response != undefined && response.length > 0) {
                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                    var isCompletedSchedule = completedSchedules.length > 0;
                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                        Declarations.msgerror("Can't edit group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                        return;
                    }
                    else if (onTheWaySchedules.length > 0) {
                        this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                        this.PublishedGroup = { shiftIndex: i, rowIndex: j, colIndex: k, schedule: shift, trip: trip };
                        jQuery('#btnconfirm-publish-delgrp').click();
                        return;
                    }
                    else {
                        this.publishLoad(i, j, k, shift, trip);
                    }
                }
                else {
                    this.publishLoad(i, j, k, shift, trip);
                }
            });
        }
        else {
            this.publishLoad(i, j, k, shift, trip);
        }
    }
    publishLoadYes() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.publishLoad(this.PublishedGroup.shiftIndex, this.PublishedGroup.rowIndex, this.PublishedGroup.colIndex, this.PublishedGroup.schedule, this.PublishedGroup.trip);
        this.PublishedGroup = null;
    }
    publishLoadNo() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.PublishedGroup = null;
    }
    publishLoad(i, j, k, shift, trip) {
        this.unsubscribeFormChange();
        this.unsubscribeDeliveryGroupEvents();
        var drivers = trip.get('Drivers').value;
        if (drivers == null || drivers == undefined || drivers.length == 0) {
            Declarations.msgwarning('Please assign a driver to publish Load ' + (k + 1), 'Driver Required', 2500);
            return;
        }
        let status = this.validateBadgeNumber(trip);
        if (status) {
            Declarations.msgerror(this._valMessageBadgeNumbers, 'Badge number error(s)', 5000);
            this.editExisingGroup(trip, i, j, k, shift, shift.get('Trips')['controls'][k - 1], shift.get('Trips')['controls'][k + 1], true);
            this.dataService.setShowOpenedDeliveryGroupSubject(true);
            return;
        }
        if (trip && trip.invalid) {
            let invalidctrls = CustomAbstractControls.findRecursiveErrors(trip);
            this.editExisingGroup(trip, i, j, k, shift, shift.get('Trips')['controls'][k - 1], shift.get('Trips')['controls'][k + 1], true);
            this.dataService.setShowOpenedDeliveryGroupSubject(true);
            return;
        }
        this.setTripStatus(trip);
        this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(trip, DeliveryReqStatus.ScheduleCreated);
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var sbModel = this.SbForm.value;
        sbModel.isCreateSchedule = true; //create schedule when load get publish
        this.sbService.publishTrailerView(sbModel)
            .subscribe(data => {
            this._savingBuilder = false;
            this.updateScheduleBuilderForm(data);
        });
    }
    publishEntireRow(shift, trailerIndex, shiftIndex) {
        this.dataService.setShowDeliveryGroupSubject(false);
        var trips = shift.get('Trips');
        var validTrips = [];
        for (var i = 0, j = 1; i < trips.length; i++, j++) {
            let thisTrip = trips.controls[i];
            let drivers = thisTrip.get('Drivers').value;
            let drs = thisTrip.get('DeliveryRequests').value;
            if (drivers.length == 0 && drs.length > 0) {
                Declarations.msgwarning('Please assign a driver to Load ' + j, 'Driver Required', 2500);
                return;
            }
            let status = this.validateBadgeNumber(thisTrip);
            if (status) {
                Declarations.msgerror(this._valMessageBadgeNumbers, 'Badge number error(s)', 5000);
                this.editExisingGroup(thisTrip, trailerIndex, shiftIndex, i, shift, shift.get('Trips')['controls'][i - 1], shift.get('Trips')['controls'][i + 1], true);
                this.dataService.setShowOpenedDeliveryGroupSubject(true);
                return;
            }
            if (thisTrip && thisTrip.invalid) {
                let invalidctrls = CustomAbstractControls.findRecursiveErrors(thisTrip);
                if (drs.length > 0) {
                    this.editExisingGroup(thisTrip, trailerIndex, shiftIndex, i, shift, shift.get('Trips')['controls'][i - 1], shift.get('Trips')['controls'][i + 1], true);
                    this.dataService.setShowOpenedDeliveryGroupSubject(true);
                    return;
                }
            }
            else {
                validTrips.push(thisTrip);
            }
        }
        for (var i = 0; i < validTrips.length; i++) {
            this.setTripStatusToPublish(validTrips[i]);
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var sbModel = this.SbForm.value;
        sbModel.isCreateSchedule = true; //create schedule when load get publish
        this.sbService.publishTrailerView(sbModel)
            .subscribe(data => {
            this._savingBuilder = false;
            this.updateScheduleBuilderForm(data);
        });
    }
    saveEntireRow(ispublish) {
        var sbModel = this.SbForm.value;
        this.dataService.setShowDeliveryGroupSubject(false);
        this._savingBuilder = true;
        if (ispublish) {
            sbModel.isCreateSchedule = true; //create schedule when load get publish
            this.sbService.publishTrailerView(sbModel)
                .subscribe(data => {
                this._savingBuilder = false;
                this.updateScheduleBuilderForm(data);
            });
        }
        else {
            this.sbService.saveTrailerView(sbModel)
                .subscribe(data => {
                this._savingBuilder = false;
                this.dataService.setUnsavedChangesSubject(false);
                this.updateScheduleBuilderForm(data);
            });
        }
    }
    setTripStatusToPublish(trip) {
        this.setTripStatus(trip);
        this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(trip, DeliveryReqStatus.ScheduleCreated);
    }
    updateScheduleBuilderForm(data) {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.unsubscribeFormChange();
            this.unsubscribeDeliveryGroupEvents();
            if (data.StatusCode == 0)
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            else
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
        }
        this.SbForm.patchValue({
            Id: data.Id,
            TimeStamp: data.TimeStamp,
            Shifts: data.Shifts,
            Trailers: data.Trailers,
            DeletedTripId: data.DeletedTripId,
            DeletedGroupId: data.DeletedGroupId
        });
        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
    }
    isTrailersSelected() {
        let _trailers = this.DriverTrailerForm.get('Trailers').value;
        return _trailers && _trailers.length > 0;
    }
    addTrip(index1, index2) {
        var shift = this.SbForm.get('Trailers.' + index1 + '.Shifts.' + index2);
        let selectedShift = this.TrailerShifts.find(x => x.Id == shift.get('ShiftId').value);
        var _tArray = shift.get('Trips');
        let _startDate = this.SbForm.get('Date').value;
        let _startTime = selectedShift.StartTime;
        if (_tArray.controls.length > 0) {
            let lastTripStartTime = _tArray.controls[_tArray.controls.length - 1].get('StartTime').value;
            let lastTripStartDate = _tArray.controls[_tArray.controls.length - 1].get('StartDate').value;
            _startTime = _tArray.controls[_tArray.controls.length - 1].get('EndTime').value;
            _startDate = this.getNewLoadStartDate(moment(lastTripStartDate + ' ' + lastTripStartTime).toDate(), moment(lastTripStartDate + ' ' + _startTime).toDate());
        }
        let startTime = moment(_startDate + ' ' + _startTime).toDate();
        let trip = this.getTrailerTrip(startTime, selectedShift.SlotPeriod, index2, _tArray.length);
        _tArray.push(this.getTripFormGroup(trip, index1, index2, _tArray.length));
    }
    getNewLoadStartDate(startDateTime, endDateTime) {
        if (startDateTime > endDateTime) {
            endDateTime.setDate(endDateTime.getDate() + 1);
        }
        return moment(endDateTime).format('MM/DD/YYYY');
    }
    toggleCollaspe(shift) {
        //var _collapse = shift.get('IsCollapsed');
        //var _isCollapsed = _collapse.value as boolean;
        //_collapse.setValue(!_isCollapsed);
    }
    onShiftSelect(item, trailer, trailerIdx) {
        let _shifts = trailer.get('Shifts');
        let selectedShift = this.TrailerShifts.find(x => x.Id == item.Id);
        if (selectedShift) {
            let shiftModel = this.getTrailerShift(selectedShift, _shifts.length);
            let _sFormGroup = this.getShiftFormGroup(shiftModel, trailerIdx, _shifts.length);
            _shifts.push(_sFormGroup);
        }
    }
    onShiftUnselect(item, trailer) {
        let _shifts = trailer.get('Shifts');
        if (_shifts) {
            let _shift = _shifts.controls.find(x => x.get('ShiftId').value == item.Id);
            if (_shift) {
                if (this.isDrsExists(_shift)) {
                    Declarations.msgerror('Please remove all delivery requests before removing shift', undefined, 3000);
                    let trailerId = trailer.get('Id').value;
                    this.SelectedTrailerShifts[trailerId].push(item);
                    trailer.get('SelectedShifts').setValue(this.SelectedTrailerShifts[trailerId]);
                }
                else {
                    _shifts.removeAt(_shifts.controls.indexOf(_shift));
                    this.dataService.setUnsavedChangesSubject(true);
                }
            }
        }
    }
    isDrsExists(shift) {
        let _result = false;
        let _trips = shift.get('Trips');
        _trips.controls.forEach(x => {
            let drs = x.get('DeliveryRequests');
            if (drs && drs.length > 0) {
                _result = true;
            }
        });
        return _result;
    }
    getTrailerShift(shift, rowIndex) {
        let model = new TrailerShiftModel();
        model.ShiftId = shift.Id;
        model.StartTime = shift.StartTime;
        model.EndTime = shift.EndTime;
        model.Trips = this.getTrailerTrips(shift, rowIndex);
        return model;
    }
    getTrailerTrips(shift, rowIndex) {
        let trips = [];
        let date = this.SbForm.get('Date').value;
        var startTime = moment(date + ' ' + shift.StartTime).toDate();
        var endTime = moment(date + ' ' + shift.EndTime).toDate();
        if (endTime <= startTime) {
            endTime = moment(endTime).add(1, 'days').toDate();
        }
        var count = 0;
        while (startTime < endTime && count < 12) {
            let trip = this.getTrailerTrip(startTime, shift.SlotPeriod, rowIndex, count);
            startTime = moment(startTime).add(shift.SlotPeriod, 'hours').toDate();
            trips.push(trip);
            count++;
        }
        return trips;
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
        trip.TrailerRowIndex = rowIndex;
        trip.TrailerColIndex = colIndex;
        return trip;
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
        if (trailers.length > 0) {
            var trailerId = [];
            trailers.forEach(obj => { trailerId.push(obj.Id); });
            let currentDrivers = this.DriverTrailerForm.get('Driver').value;
            this.sbService.getCompanyDrivers(trailerId, this.SelectedRegionId, this.SbForm.controls.Date.value).subscribe((drivers) => {
                this.UnassignedDrivers = this.getUnassignedDrivers(drivers, [currentDrivers]);
                if (this.UnassignedDrivers.length == 0) {
                    this.UnassignedDrivers = [];
                    this._isDriverFound = true;
                }
                else {
                    this._isDriverFound = false;
                }
            });
        }
        else {
            this.UnassignedDrivers = [];
            this._isDriverFound = true;
        }
    }
    IdComparer(otherArray) {
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
        if (trips) {
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
        }
        return isPublished;
    }
    editExisingGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _lastTrip = null, _nextTrip = null, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _lastTrip, _nextTrip, _isPublishLoadInvalid);
        this.dataService.setShowOpenedDeliveryGroupSubject(true);
    }
    editDroppedGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _lastTrip = null, _nextTrip = null, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _lastTrip, _nextTrip, _isPublishLoadInvalid);
        this.dataService.setShowDeliveryGroupSubject(true);
    }
    editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _lastTrip = null, _nextTrip = null, _isPublishLoadInvalid = false) {
        this.unsubscribeDeliveryGroupEvents();
        this.dataService.setEditDeliveryGroupSubject({
            trip: _trip,
            shiftIndex: _shiftIndex,
            rowIndex: _rowIndex,
            tripIndex: _tripIndex,
            schedule: _schedule,
            lastTrip: _lastTrip,
            nextTrip: _nextTrip,
            isPublishLoadInvalid: _isPublishLoadInvalid
        });
        this.subscribeFormChange();
        this.subscribeDeliveryGroupEvents();
    }
    IntializeChat(driverId) {
        this.chatService.pushDriverDetails(driverId);
    }
    trackByTrailerId(index, trailer) {
        return trailer.controls["TrailerId"].value;
    }
    trackByShiftId(index, shift) {
        return shift.controls["ShiftId"].value;
    }
    trackByTripIndex(index, shift) {
        return index;
    }
    trackByDriverId(index, driver) {
        return driver.controls["Id"].value;
    }
    trackByDrId(index, dr) {
        return dr.controls["Id"].value;
    }
    deleteScheduleDetails(tripDetails, drIndex) {
        var scheduleDetails = {
            trip: tripDetails,
            index: drIndex
        };
        this.dataService.setDeleteDRRequestSubject(scheduleDetails);
    }
    validateBadgeNumber(DelGroupForm) {
        let validateBadgeNumber = false;
        this._valMessageBadgeNumbers = '';
        let isCommon = true;
        let deliveryRequests = DelGroupForm.controls['DeliveryRequests'];
        if (deliveryRequests != null) {
            for (var i = 0; i < deliveryRequests.length; i++) {
                if (deliveryRequests.controls[i].get('IsCommonBadge').value == true) {
                    let BadgeNo1 = DelGroupForm.controls['BadgeNo1'].value;
                    let BadgeNo2 = DelGroupForm.controls['BadgeNo2'].value;
                    let BadgeNo3 = DelGroupForm.controls['BadgeNo3'].value;
                    if (BadgeNo1 == 'null' && BadgeNo2 == 'null' && BadgeNo3 == 'null') {
                        if (isCommon) {
                            this._valMessageBadgeNumbers = this._valMessageBadgeNumbers + '<br/>Atleast one badge number is required for common badge number(s).';
                            validateBadgeNumber = true;
                        }
                        isCommon = false;
                    }
                    else if (BadgeNo1.length == 0 && BadgeNo2.length == 0 && BadgeNo3.length == 0) {
                        if (isCommon) {
                            this._valMessageBadgeNumbers = this._valMessageBadgeNumbers + '<br/>Atleast one badge number is required for common badge number(s).';
                            validateBadgeNumber = true;
                        }
                        isCommon = false;
                    }
                }
                else {
                    let BadgeNo1 = deliveryRequests.controls[i].get('BadgeNo1').value;
                    let BadgeNo2 = deliveryRequests.controls[i].get('BadgeNo2').value;
                    let BadgeNo3 = deliveryRequests.controls[i].get('BadgeNo3').value;
                    let CustomerCompany = deliveryRequests.controls[i].get('CustomerCompany').value;
                    let ProductType = deliveryRequests.controls[i].get('ProductType').value;
                    if (BadgeNo1 == 'null' && BadgeNo2 == 'null' && BadgeNo3 == 'null') {
                        this._valMessageBadgeNumbers = this._valMessageBadgeNumbers + '<br/>Atleast one badge number(s) is required for particular product type:' + CustomerCompany + '-' + ProductType + '.';
                        validateBadgeNumber = true;
                    }
                    else if (BadgeNo1.length == 0 && BadgeNo2.length == 0 && BadgeNo3.length == 0) {
                        this._valMessageBadgeNumbers = this._valMessageBadgeNumbers + '<br/>Atleast one badge number(s) is required for particular product type:' + CustomerCompany + '-' + ProductType + '.';
                        validateBadgeNumber = true;
                    }
                }
            }
        }
        return validateBadgeNumber;
    }
};
__decorate([
    Input()
], TrailerViewComponent.prototype, "SbForm", void 0);
__decorate([
    Input()
], TrailerViewComponent.prototype, "Trailers", void 0);
__decorate([
    Input()
], TrailerViewComponent.prototype, "RegionDetail", void 0);
__decorate([
    Input()
], TrailerViewComponent.prototype, "SelectedRegionId", void 0);
__decorate([
    Input()
], TrailerViewComponent.prototype, "TrailerViewFilter", void 0);
__decorate([
    Input()
], TrailerViewComponent.prototype, "HeaderToggle", void 0);
TrailerViewComponent = __decorate([
    Component({
        selector: 'app-trailer-view',
        templateUrl: './trailer-view.component.html',
        styleUrls: ['./trailer-view.component.css'],
        changeDetection: ChangeDetectionStrategy.OnPush
    })
], TrailerViewComponent);
export { TrailerViewComponent };
//# sourceMappingURL=trailer-view.component.js.map