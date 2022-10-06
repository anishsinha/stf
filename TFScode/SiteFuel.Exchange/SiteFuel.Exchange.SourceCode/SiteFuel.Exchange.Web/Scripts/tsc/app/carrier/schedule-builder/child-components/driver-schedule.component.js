import { __decorate } from "tslib";
import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { TripModel, UnAssignDriverFromShiftModel } from '../../models/DispatchSchedulerModels';
import { MyLocalStorage } from 'src/app/my.localstorage';
import * as moment from 'moment';
import { Declarations } from 'src/app/declarations.module';
import { debounceTime } from 'rxjs/operators';
let DriverScheduleComponent = class DriverScheduleComponent {
    constructor(sbService, dataService, utilService, chatService, changeDetectorRef) {
        this.sbService = sbService;
        this.dataService = dataService;
        this.utilService = utilService;
        this.chatService = chatService;
        this.changeDetectorRef = changeDetectorRef;
        this.SelectedTrailers = [];
        this.UnassignedDrivers = [];
        this.IsTrailerExists = false;
        this.disableControl = false;
        this.preloadOption = { TripIndex: 0, Preloaded: false };
        this.RegionDetailTrailers = [];
        this.RouteListForTrip = [];
        this.SelectedTrip = null;
        this.IsLoadingRoute = false;
        this.CompletedScheduleStatuses = [7, 8, 9, 10];
        this.DoNotShowPushButtonStatuses = [7, 8, 9, 11, 12, 21, 22, 25];
    }
    ngOnInit() {
        this.dataService.AllTrailersSubject.pipe(debounceTime(1000)).subscribe(x => {
            this.RegionDetailTrailers = x;
            this.IsTrailerExists = x.length > 0;
            if (!this.changeDetectorRef['destroyed']) {
                this.changeDetectorRef.detectChanges();
            }
        });
        this.subscribeScheduleChangeDetectSubject();
        this.subscribeDisableControlsSubject();
        this.subscribeDeliveryPreloadOption();
        this.subscribeDeleteRoutesInfoSubjectSubject();
    }
    ngAfterViewInit() {
        window.setTimeout(() => {
            this.dataService.setDeliveryPreloadOption({ ShiftIndex: this.i, ScheduleIndex: this.j, Reset: true });
        }, 2000);
    }
    resetPreLoadedFlag() {
        this.preloadOption = { TripIndex: 0, Preloaded: false };
    }
    setPreLoadedFlag() {
        this.schedule.controls['Trips']['controls'].forEach((x, index) => {
            let tripDrs = x.controls['DeliveryRequests']['controls'];
            if (tripDrs.length > 0) {
                let preloadedDrs = tripDrs.filter((y) => y.controls['PreLoadedFor']);
                if (preloadedDrs.length == 0 && this.preloadOption['Preloaded'] == false) {
                    this.preloadOption['TripIndex'] = index;
                }
                else if (preloadedDrs.length > 0 && this.preloadOption['Preloaded'] == false) {
                    this.preloadOption['TripIndex'] = index;
                    this.preloadOption['Preloaded'] = true;
                }
            }
        });
        this.changeDetectorRef.markForCheck();
        this.changeDetectorRef.detectChanges();
    }
    ngOnDestroy() {
        this.unsubscribeFormChange();
        this.unsubscribeScheduleChangeDetectSubject();
        this.unsubscribeDeliveryPreloadOption();
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
        if (this.DeleteRouteInfoSubscription) {
            this.DeleteRouteInfoSubscription.unsubscribe();
        }
    }
    unsubscribeUpdateDriverTrailerSubject() {
        if (this.UpdateDriverSubscription) {
            this.UpdateDriverSubscription.unsubscribe();
        }
    }
    unsubscribeGroupChangeDetectSubject() {
        if (this.GroupChangeDetectSubscription) {
            this.GroupChangeDetectSubscription.unsubscribe();
        }
    }
    unsubscribeScheduleChangeDetectSubject() {
        if (this.ScheduleChangeDetectSubscription) {
            this.ScheduleChangeDetectSubscription.unsubscribe();
        }
    }
    unsubscribeFormChange() {
        this.dataService.FormChangeSubscription && this.dataService.FormChangeSubscription.forEach(t => {
            if (t) {
                t.unsubscribe();
            }
        });
    }
    unsubscribeDeliveryPreloadOption() {
        if (this.DeliveryPreloadOptionSubscription) {
            this.DeliveryPreloadOptionSubscription.unsubscribe();
        }
    }
    subscribeFormChange() {
        this.unsubscribeFormChange();
        let _trips = this.schedule.controls['Trips'];
        for (var k = 0; k < _trips.length; k++) {
            let _initialValues = JSON.stringify(_trips.controls[k].value);
            var formChanges = _trips.controls[k].valueChanges.pipe().subscribe(x => {
                if (x) {
                    let _currentValues = JSON.stringify(x);
                    if (_initialValues != _currentValues) {
                        this.dataService.setUnsavedChangesSubject(x);
                    }
                }
            });
            this.dataService.FormChangeSubscription && this.dataService.FormChangeSubscription.push(formChanges);
        }
    }
    subscribeUpdateDriverTrailerSubject() {
        this.unsubscribeUpdateDriverTrailerSubject();
        this.UpdateDriverSubscription = this.dataService.UpdateDriverTrailerSubject.subscribe(data => {
            if (data) {
                if (data.Driver) {
                    var _dArray = this.schedule.controls['Drivers'];
                    _dArray.clear();
                    _dArray.push(this.utilService.getDriverForm(data.Driver));
                    var _tArray = this.schedule.controls['Trailers'];
                    var prevTrailer = _tArray.value;
                    _tArray.clear();
                    let _selectedTrailers = this.RegionDetailTrailers.filter(x => {
                        return data.Trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
                    });
                    _selectedTrailers.forEach(x => {
                        _tArray.push(this.utilService.getAssignedTrailerForm(x));
                    });
                    this.updateCompartmentinfo(prevTrailer, _selectedTrailers);
                    this.changeDetectorRef.detectChanges();
                    this.dataService.setSaveDriverAssignmentSubject({ Index1: data.Index1, Index2: data.Index2 });
                }
            }
            this.unsubscribeUpdateDriverTrailerSubject();
        });
    }
    subscribeGroupChangeDetectSubject() {
        this.GroupChangeDetectSubscription = this.dataService.GroupChangeDetectSubject.subscribe(x => {
            if (x) {
                this.changeDetectorRef.detectChanges();
                this.unsubscribeGroupChangeDetectSubject();
                this.unsubscribeFormChange();
            }
        });
    }
    subscribeScheduleChangeDetectSubject() {
        this.ScheduleChangeDetectSubscription = this.dataService.ScheduleChangeDetectSubject.subscribe(x => {
            if (x) {
                this.changeDetectorRef.detectChanges();
            }
        });
    }
    subscribeDisableControlsSubject() {
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }
    subscribeDeliveryPreloadOption() {
        this.DeliveryPreloadOptionSubscription = this.dataService.DeliveryPreloadOption.pipe(debounceTime(2000)).subscribe(x => {
            if (x) { //&& x.ShiftIndex == this.i && x.ScheduleIndex == this.j
                if (x.Reset) {
                    this.resetPreLoadedFlag();
                }
                this.setPreLoadedFlag();
            }
        });
    }
    addLoad(shiftIdx, scheduleIdx) {
        var _tArray = this.schedule.controls['Trips'];
        let _startDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        var shift = this.SbForm.controls['Shifts']['controls'][shiftIdx];
        let shiftId = shift.controls['Id'].value;
        let selectedShift = this.getSelectedShift(shiftId);
        let _startTime = selectedShift.StartTime;
        if (_tArray.controls.length > 0) {
            let lastTripStartTime = _tArray.controls[_tArray.controls.length - 1].get('StartTime').value;
            let lastTripStartDate = _tArray.controls[_tArray.controls.length - 1].get('StartDate').value;
            if (lastTripStartDate == '') {
                lastTripStartDate = _startDate;
            }
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
    getSelectedShift(shiftId) {
        let _shifts = this.dataService.AllShiftsSubject.value;
        return _shifts.find(x => x.Id == shiftId);
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
        trip.DriverRowIndex = rowIndex;
        trip.DriverColIndex = colIndex;
        return trip;
    }
    editDriverTrailers(shiftIdx, scheduleIdx) {
        var driver = { Id: null, Name: '', Code: '' };
        if (this.schedule.controls['Drivers']['controls'].length > 0) {
            driver = this.schedule.controls['Drivers']['controls'][0].value;
        }
        let _isPublishedRequestsExists = this.IsPublishedRequestsExists();
        var trailers = this.schedule.controls['Trailers'].value;
        //if (trailers.length > 0 && trailers != undefined && trailers != null) {
        //    if (trailers[0].Id == '') {
        //        trailers = [];
        //    } 
        //}
        let data = {
            Index1: shiftIdx,
            Index2: scheduleIdx,
            Driver: driver.Id,
            Trailers: trailers,
            IsPublishedRequestsExists: _isPublishedRequestsExists
        };
        this.SelectedTrailers = trailers;
        this.dataService.setEditDriverTrailerSubject(data);
        this.subscribeUpdateDriverTrailerSubject();
    }
    IsPublishedRequestsExists() {
        let isPublished = false;
        let trips = this.schedule.controls['Trips']['controls'];
        for (let i = 0; i < trips.length; i++) {
            var drs = trips[i].controls['DeliveryRequests']['controls'];
            for (let k = 0; k < drs.length; k++) {
                if (drs[k]['controls'].PreviousStatus.value === 3) {
                    isPublished = true;
                    break;
                }
            }
            if (isPublished) {
                break;
            }
        }
        return isPublished;
    }
    publishEntireRow(shiftIndex, rowIndex) {
        this.dataService.setPublishEntireRowSubject({ ShiftIndex: shiftIndex, ScheduleIndex: rowIndex });
    }
    draftScheduleBuilder(trip) {
        this.dataService.setDraftDeliveryGroupSubject({ trip: trip, filterChanged: false });
    }
    publishScheduleBuilder(i, j, k, schedule, trip) {
        this.dataService.setPublishDeliveryGroupSubject({ shiftIndex: i, rowIndex: j, colIndex: k, schedule: schedule, trip: trip });
    }
    onItemDrop(trip, event, shiftIndex, rowIndex, colIndex, schedule) {
        if (this.preloadOption["Preloaded"] == false && this.preloadOption["TripIndex"] < colIndex) {
            this.preloadOption["TripIndex"] = colIndex;
        }
        this.dataService.setDragDropItemSubject({ trip: trip, event: event, shiftIndex: shiftIndex, rowIndex: rowIndex, colIndex: colIndex, schedule: schedule });
        this.subscribeGroupChangeDetectSubject();
        this.subscribeFormChange();
        this.sorDrByProductSequence(trip);
    }
    sorDrByProductSequence(trip) {
        var drForm = trip.controls['DeliveryRequests'];
        let isCommonPickup = trip.controls['IsCommonPickup'].value;
        let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value;
        let drList = drForm.value || [];
        //drList = sortArrayTwice(drList, 'JobId', 'ProductSequence');
        drForm.clear();
        var _drFormArray = this.utilService.getDeliveryRequestFormArray(drList, isCommonPickup, IsDispatcherDragDropSequence, 0);
        _drFormArray.controls.forEach((_drForm) => {
            drForm.push(_drForm);
        });
    }
    validateTrailerJobCompatibility(drData, schedule) {
        var _deliveryRequests = [];
        _deliveryRequests.push(drData.Data.value);
        let _selectedTrailers = schedule.controls["Trailers"].value;
        var trips = schedule.get('Trips');
        if ((_selectedTrailers && _selectedTrailers.length > 0) && (_deliveryRequests && _deliveryRequests.length > 0)) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests).subscribe(data => {
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
    removeClassForAllLoad(trips) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i];
            var deliveryRequests = trip.controls["DeliveryRequests"];
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j];
                    deliveryRequest.controls["IsBlinkDR"].patchValue(false);
                }
            }
        }
    }
    editExisingGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowOpenedDeliveryGroupSubject(true);
    }
    editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid = false) {
        this.dataService.setEditDeliveryGroupSubject({
            trip: _trip,
            shiftIndex: _shiftIndex,
            rowIndex: _rowIndex,
            tripIndex: _tripIndex,
            schedule: _schedule,
            isPublishLoadInvalid: _isPublishLoadInvalid
        });
        this.subscribeFormChange();
        this.subscribeGroupChangeDetectSubject();
    }
    setDeletePostloadedDrsSubject(drs) {
        let postloadedDrInfo = drs.filter(x => x.PostLoadInfo);
        if (postloadedDrInfo.length > 0) {
            this.dataService.setDeletePostloadSubject(postloadedDrInfo);
        }
    }
    deleteScheduleDetails(tripDetails, drIndex) {
        var scheduleDetails = {
            trip: tripDetails,
            index: drIndex
        };
        this.setDeletePostloadedDrsSubject([tripDetails.controls['DeliveryRequests']['controls'][drIndex].value]);
        this.dataService.setDeleteDRRequestSubject(scheduleDetails);
        this.dataService.setDeliveryPreloadOption({ ShiftIndex: this.i, ScheduleIndex: this.j, Reset: true });
    }
    resetDrByRoutes(_shiftIndex, _rowIndex, _tripIndex, _trip, _schedule) {
        let _data = {
            i: _shiftIndex, j: _rowIndex, k: _tripIndex, trip: _trip, schedule: _schedule
        };
        this.resetRouteDetails(_data);
    }
    subscribeDeleteRoutesInfoSubjectSubject() {
        this.DeleteRouteInfoSubscription = this.dataService.DeleteRouteDetailsSubject.subscribe(x => {
            this.SelectedTrip = x.SelectedTrip;
            this.deleteDrsForRoute(x.route);
        });
    }
    deleteDrsForRoute(route) {
        let _trip = this.SelectedTrip.trip;
        let delieveryRequestForm = _trip.controls['DeliveryRequests'];
        let delieveryRequests = delieveryRequestForm ? delieveryRequestForm.value : [];
        let drIndex = delieveryRequests.findIndex(_dr => _dr && _dr.RouteInfo && _dr.RouteInfo.Id && _dr.RouteInfo.Id == route.Id.toString());
        if (drIndex == -1)
            return;
        //this.IsLoadingRoute = true;
        this.RouteListForTrip.splice(drIndex, 1);
        var indexList = [];
        var scheduleIds = [];
        delieveryRequests.forEach((dr, currentIndex) => {
            if (dr && dr.RouteInfo && (dr.PostLoadedFor == null || dr.PostLoadedFor == undefined || dr.PostLoadedFor == '')) {
                if (dr.RouteInfo.Id == route.Id.toString()) {
                    indexList.push(currentIndex);
                    if (dr.TrackableScheduleId != null && dr.TrackableScheduleId > 0) {
                        scheduleIds.push(dr.TrackableScheduleId);
                    }
                }
            }
        });
        if (scheduleIds.length > 0) {
            this.IsLoadingRoute = true;
            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                this.IsLoadingRoute = false;
                if (response != null && response != undefined && response.length > 0) {
                    var data = response[0];
                    if (this.CompletedScheduleStatuses.indexOf(data.ScheduleStatusId) !== -1 || data.ScheduleEnrouteStatusId == 4) {
                        Declarations.msgerror("Can't delete as drop has been made already", 'Warning', 2500);
                        return;
                    }
                    else {
                        this.deleteDrsFromTrip(indexList);
                    }
                }
                else {
                    this.deleteDrsFromTrip(indexList);
                }
            });
        }
        else {
            this.deleteDrsFromTrip(indexList);
        }
        var routeParamInfo = {
            SelectedTrip: this.SelectedTrip,
            RouteListForTrip: this.RouteListForTrip
        };
        this.dataService.setRouteDetailsSubject(routeParamInfo);
    }
    deleteDrsFromTrip(indexList) {
        let _trip = this.SelectedTrip.trip;
        let delieveryRequestForm = _trip.controls['DeliveryRequests'];
        let removedDrCount = 0;
        var deletedRequests = [];
        for (let currentIndex = 0; currentIndex < indexList.length; currentIndex++) {
            let formIndex = indexList[currentIndex];
            deletedRequests.push(_trip.controls['DeliveryRequests']['controls'][formIndex - removedDrCount].value);
            delieveryRequestForm.removeAt((formIndex - removedDrCount));
            removedDrCount++;
            if (currentIndex == indexList.length - 1) {
                this.setDeletePostloadedDrsSubject(deletedRequests);
                this.dataService.setRestoreDeletedRequestSubject(deletedRequests);
                this.dataService.setScheduleChangeDetectSubject(true);
                this.editExisingGroup(this.SelectedTrip.trip, this.SelectedTrip.i, this.SelectedTrip.j, this.SelectedTrip.k, this.SelectedTrip.schedule);
            }
        }
        //close modal if no routes 
        if (this.RouteListForTrip.length == 0) {
            let elem = document.getElementById('closeResetDrByRouteModal');
            elem.click();
        }
        //
        this.resetDrByRoutes(this.SelectedTrip.i, this.SelectedTrip.j, this.SelectedTrip.k, _trip, this.SelectedTrip.schedule);
    }
    getDraggedDrs(jobId, trip) {
        let drs = trip.controls['DeliveryRequests'].value;
        let filtered = drs.filter(x => x.JobId == jobId && !x.PreLoadedFor && !x.PostLoadedFor);
        return filtered;
    }
    preloadDelieryForThisLoc(trip, tripIndex, jobId) {
        let driverTrailerInfo = this.getAssignedDriverTrailerInfo();
        if (driverTrailerInfo.Trailers.length > 0 || driverTrailerInfo.Drivers.length > 0) {
            let shift = this.SbForm.controls['Shifts']['controls'][this.i];
            let _drArray = trip.controls['DeliveryRequests'].value;
            let drs = _drArray.filter(x => x.JobId == jobId);
            if (drs && drs.length > 0) {
                let isValidDrs = this.validatePreloadDrs(trip, drs);
                if (isValidDrs) {
                    this.dataService.setCreatePreloadSubject({
                        ShiftId: shift.controls['Id'].value,
                        ShiftIndex: this.i,
                        ScheduleIndex: this.j,
                        TripIndex: tripIndex,
                        PreloadTrailers: driverTrailerInfo.Trailers,
                        PreloadDrivers: driverTrailerInfo.Drivers,
                        IsTrailerExists: this.IsTrailerExists,
                        PreloadDrs: drs
                    });
                }
                else {
                    let schedule = shift.controls['Schedules']['controls'][this.j];
                    this.editExisingGroup(trip, this.i, this.j, tripIndex, schedule, false);
                }
            }
        }
        else {
            Declarations.msgerror(driverTrailerInfo.ErrorMessage, undefined, undefined);
        }
    }
    validatePreloadDrs(trip, drs) {
        let isValid = true;
        if (trip.controls['IsCommonPickup'].value == true) {
            isValid = trip.valid;
            if (isValid) {
                let pickupType = trip.controls['PickupLocationType'].value;
                if (pickupType == 2) {
                    drs.forEach(x => {
                        x['PickupLocationType'] = pickupType;
                        x['BulkPlant'] = trip.controls['BulkPlant'].value;
                        x['Terminal'] = {};
                    });
                }
                else {
                    drs.forEach(x => {
                        x['PickupLocationType'] = pickupType;
                        x['BulkPlant'] = {};
                        x['Terminal'] = trip.controls['Terminal'].value;
                    });
                }
            }
        }
        else {
            drs.forEach(x => {
                if (x.PickupLocationType == 2) {
                    if (!x.BulkPlant || !x.BulkPlant.SiteName || x.BulkPlant.SiteName == '') {
                        isValid = false;
                    }
                }
                else {
                    if (!x.Terminal || !x.Terminal.Id || x.Terminal.Id == 0 || x.Terminal.Id == '') {
                        isValid = false;
                    }
                }
            });
        }
        return isValid;
    }
    getAssignedDriverTrailerInfo() {
        let info = { Drivers: [], Trailers: [], IsTrailerExists: this.IsTrailerExists, ErrorMessage: '' };
        if (info.IsTrailerExists) {
            info.Trailers = this.getAssignedTrailers();
            if (info.Trailers.length == 0) {
                info.ErrorMessage = 'Please assign a trailer to preload this delivery';
            }
        }
        else {
            info.Drivers = this.getAssignedDrivers();
            if (info.Drivers.length == 0) {
                info.ErrorMessage = 'Please assign a driver to preload this delivery';
            }
        }
        return info;
    }
    getAssignedTrailers() {
        let trailers = this.schedule.controls['Trailers'].value || [];
        return trailers;
    }
    getAssignedDrivers() {
        let drivers = this.schedule.controls['Drivers'].value || [];
        return drivers;
    }
    IntializeChat(driverId) {
        this.chatService.pushDriverDetails(driverId);
    }
    trackByTripIndex(index, schedule) {
        return index;
    }
    trackByTrailerId(index, trailer) {
        return trailer.controls["TrailerId"].value;
    }
    trackByDrId(index, dr) {
        return dr.controls["Id"].value;
    }
    trackByDriverId(index, driver) {
        return driver.controls["Id"].value;
    }
    UnAssignDriverFromShift(shiftrow, type, trailerId = '') {
        if (shiftrow != undefined && shiftrow != null) {
            var _trips = shiftrow.controls['Trips'].value;
            var _drivers = shiftrow.controls['Drivers'].value;
            let _trailers = shiftrow.controls['Trailers'].value;
            if (_trips.length > 0 && (_drivers.length > 0 || _trailers.length > 0)) {
                let IsAnyLoadPublished = this.IsPublishedRequestsExists();
                if (IsAnyLoadPublished) {
                    Declarations.msgerror("A load has already been published for this driver. Cannot unassign this driver", undefined, undefined);
                }
                else {
                    let UnAssignDriverFromShift = new UnAssignDriverFromShiftModel();
                    let _sbId = this.SbForm.controls['Id'].value;
                    let timeStamp = this.SbForm.controls['TimeStamp'].value;
                    if (_trips.length > 0 && (_drivers.length || _trailers.length)) {
                        let _rowIdx = _trips[0].DriverRowIndex;
                        let _shiftIdx = _trips[0].ShiftIndex;
                        UnAssignDriverFromShift.DriverRowIdx = _rowIdx;
                        if (type === 'driver') {
                            UnAssignDriverFromShift.Drivers = _drivers;
                        }
                        UnAssignDriverFromShift.sbId = _sbId;
                        if (type === 'trailer' && trailerId && trailerId != '') {
                            var data = _trailers.filter(top => top.Id.toString() == trailerId);
                            if (data.length > 0) {
                                _trailers = data;
                            }
                            UnAssignDriverFromShift.Trailers = _trailers;
                        }
                        UnAssignDriverFromShift.Trips = _trips;
                        UnAssignDriverFromShift.TimeStamp = timeStamp;
                        UnAssignDriverFromShift.shiftIdx = _shiftIdx;
                    }
                    this.sbService.UnAssignDriverTrailerFromShift(UnAssignDriverFromShift).subscribe((response) => {
                        if (response) {
                            if (response.StatusCode == 0) {
                                Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                            }
                            else {
                                Declarations.msgerror(response.StatusMessage, undefined, undefined);
                            }
                            if (response.StatusCode == 0) {
                                var tripsToUpdate = shiftrow.controls['Trips'];
                                var updatedTimeStamp = response.Trips[0].TimeStamp;
                                for (var i = 0; i < tripsToUpdate.length; i++) {
                                    let thisTrip = tripsToUpdate.controls[i];
                                    thisTrip.patchValue({ TimeStamp: updatedTimeStamp });
                                }
                                let driverctrlarray = shiftrow.controls['Drivers'];
                                if (type === 'driver') {
                                    driverctrlarray.clear(); // Clear driver formarray controls
                                }
                                if (type === 'trailer') {
                                    this.unassignTrailers(shiftrow, type, trailerId);
                                }
                                this.changeDetectorRef.detectChanges();
                            }
                        }
                    });
                }
            }
        }
    }
    unassignTrailers(shiftrow, type, trailerId = '') {
        if (type === 'trailer') {
            let trailerctrlarray = shiftrow.get('Trailers');
            if (trailerId != '') {
                var trailerIndex = trailerctrlarray.value.findIndex(top => top.Id.toString() == trailerId);
                if (trailerIndex != -1) {
                    trailerctrlarray.removeAt(trailerIndex);
                    var removeTrailer = [trailerId];
                    this.removeCompartmentinfo(removeTrailer, shiftrow);
                }
            }
        }
    }
    editCompartmentAssignments() {
        Declarations.slidePanel("#compartment-assigning", "97%");
        this.dataService.setEditCompartmentAssigmentSubject({ Schedule: this.schedule.value, ShiftIndex: this.i, RowIndex: this.j });
    }
    updateCompartmentinfo(prevTrailer, _selectedTrailers) {
        var removedTrailer = [];
        for (var i = 0, len = prevTrailer.length; i < len; i++) {
            var missingTrailerIndex = _selectedTrailers.findIndex(x => x.Id.toString() == prevTrailer[i].Id.toString());
            if (missingTrailerIndex == -1) {
                removedTrailer.push(prevTrailer[i].Id);
            }
        }
        this.removeCompartmentinfo(removedTrailer, this.schedule);
    }
    removeCompartmentinfo(removedTrailer, shiftrow) {
        if (this.IsTrailerExists && removedTrailer.length > 0) {
            var _tripsInfo = shiftrow.controls['Trips']['controls'];
            for (var k = 0; k < _tripsInfo.length; k++) {
                var tripsInfoControl = _tripsInfo[k].controls;
                tripsInfoControl['IsTrailerExists'].patchValue(this.IsTrailerExists);
                var deliveryRequest = _tripsInfo[k].controls['DeliveryRequests']['controls'];
                for (var j = 0; j < deliveryRequest.length; j++) {
                    var compartmentInfo = deliveryRequest[j].controls["Compartments"];
                    for (var i = 0; i < removedTrailer.length; i++) {
                        var compartmentIndex = compartmentInfo.value.findIndex(x => x.TrailerId.toString() == removedTrailer[i].toString());
                        if (compartmentIndex != -1) {
                            compartmentInfo.removeAt(compartmentIndex);
                        }
                    }
                }
            }
        }
        else {
            var _tripsInfo = shiftrow.controls['Trips']['controls'];
            for (var k = 0; k < _tripsInfo.length; k++) {
                var tripsInfoControl = _tripsInfo[k].controls;
                tripsInfoControl['IsTrailerExists'].patchValue(this.IsTrailerExists);
            }
        }
    }
    resetRouteDetails(x) {
        this.RouteListForTrip = [];
        if (x) {
            this.SelectedTrip = x;
            let drForm = this.SelectedTrip.trip.controls['DeliveryRequests'];
            let drValue = drForm ? drForm.value : [];
            drValue.forEach(dr => {
                if (dr && dr.RouteInfo && dr.PostLoadedFor == undefined) {
                    let drIndex = this.RouteListForTrip.findIndex(a => a.Id == dr.RouteInfo.Id);
                    if (drIndex === -1) {
                        this.RouteListForTrip.push(dr.RouteInfo);
                    }
                }
            });
            if (this.RouteListForTrip.length > 0) {
                var routeParamInfo = {
                    SelectedTrip: this.SelectedTrip,
                    RouteListForTrip: this.RouteListForTrip
                };
                this.dataService.setRouteDetailsSubject(routeParamInfo);
                this.editExisingGroup(this.SelectedTrip.trip, this.SelectedTrip.i, this.SelectedTrip.j, this.SelectedTrip.k, this.SelectedTrip.schedule);
            }
            else {
                let elem = document.getElementById('closeResetDrByRouteModal');
                elem.click();
            }
            this.changeDetectorRef.detectChanges();
        }
    }
};
__decorate([
    Input()
], DriverScheduleComponent.prototype, "i", void 0);
__decorate([
    Input()
], DriverScheduleComponent.prototype, "j", void 0);
__decorate([
    Input()
], DriverScheduleComponent.prototype, "SbForm", void 0);
__decorate([
    Input()
], DriverScheduleComponent.prototype, "schedule", void 0);
DriverScheduleComponent = __decorate([
    Component({
        selector: 'app-driver-schedule',
        templateUrl: './driver-schedule.component.html',
        styleUrls: ['./driver-schedule.component.css'],
        changeDetection: ChangeDetectionStrategy.OnPush
    })
], DriverScheduleComponent);
export { DriverScheduleComponent };
//# sourceMappingURL=driver-schedule.component.js.map