import { Component, OnInit, ChangeDetectionStrategy, Input, ChangeDetectorRef, OnDestroy, AfterViewInit, SimpleChanges, OnChanges, ViewChild } from '@angular/core';
import { FormGroup, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { chatService } from 'src/app/shared-components/sendbird//services/sendbird.service';
import { DropdownItem } from 'src/app/statelist.service';
import { UtilService } from 'src/app/services/util.service';
import { TrailerViewModel, ScheduleShiftModel, TripModel, DeliveryRequestViewModel, UnAssignDriverFromShiftModel, ProductDeliveryScheduleInfo, OptionalPickupDetailModel, BlendDRDetails } from '../../models/DispatchSchedulerModels';
import { ScheduleBuilderService } from '../../service/schedule-builder.service';
import { DataService } from 'src/app/services/data.service';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { Subscription } from 'rxjs';
import * as moment from 'moment';
import { Declarations } from 'src/app/declarations.module';
import { debounceTime } from 'rxjs/operators';
import { LoadQueueService } from '../dsb-load-queue/load-queue.service';
import { DSBLoadQueueModel } from '../dsb-load-queue/dsb-load.model';
import { SBConstants } from 'src/app/app.constants';

@Component({
    selector: 'app-driver-schedule-column-view',
    templateUrl: './driver-schedule-column-view.component.html',
    styleUrls: ['./driver-schedule-column-view.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DriverScheduleColumnViewComponent implements OnInit {
    @Input() public i: number; //shift index
    @Input() public j: number; //schedule or column index
    @Input() public SbForm: FormGroup;
    @Input() public schedule: FormGroup;
    @Input() public ScheduleLength: number;
    @Input() public ShiftId: string;
    @Input() public shiftOrderNo: number; //shift order number
    public SelectedTrailers: TrailerViewModel[] = [];
    public UnassignedDrivers: DropdownItem[] = [];

    public IsTrailerExists: boolean = false;
    //public SelectedRegionId: string;//not in use
    public SbFormDate: string;
    public draggedDelReqData: any;
    public droppedTrip: any;
    public disableControl: boolean = false;
    public preloadOption: any = { TripIndex: 0, Preloaded: false };
    private RegionDetailTrailers: TrailerViewModel[] = [];
    private UpdateDriverSubscription: Subscription;
    private GroupChangeDetectSubscription: Subscription;
    private ScheduleChangeDetectSubscription: Subscription;
    private DisableControlsSubscription: Subscription;
    private DeliveryPreloadOptionSubscription: Subscription;
    private TrailerInformationChangeDetectSubscription: Subscription;
    private GridViewSearchSubscription: Subscription;
    public _valMessageBadgeNumbers: string;
    RouteListForTrip: any[] = [];
    SelectedTrip: any = null;
    DeleteDrByRoutes: Subscription;
    IsLoadingRoute: boolean = false;
    public CompletedScheduleStatuses: number[] = [7, 8, 9, 10];
    public DoNotShowPushButtonStatuses: number[] = [5, 7, 8, 9, 11, 12, 21, 22, 25];
    public DeliveryScheduleInfo: ProductDeliveryScheduleInfo[] = [];
    public LocationName: string;
    public CustomerName: string;
    public IsTBDRequest: boolean;
    public tempRouteList: any[] = [];

    constructor(private sbService: ScheduleBuilderService, private dataService: DataService, private loadQueueService: LoadQueueService,
        private utilService: UtilService, private chatService: chatService, private changeDetectorRef: ChangeDetectorRef) {
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
        this.subscribeGridViewSearchEvents();


    }
    public ngAfterViewInit(): void {
        window.setTimeout(() => {
            this.dataService.setDeliveryPreloadOption({ ShiftIndex: this.i, ScheduleIndex: this.j, Reset: true });
        }, 2000);
    }

    private resetPreLoadedFlag(): void {
        this.preloadOption = { TripIndex: 0, Preloaded: false };
    }
    private setPreLoadedFlag(): void {
        this.schedule.controls['Trips']['controls'].forEach((x: FormGroup, index: number) => {
            let tripDrs = x.controls['DeliveryRequests']['controls'];
            if (tripDrs.length > 0) {
                let preloadedDrs = tripDrs.filter((y: FormGroup) => y.controls['PreLoadedFor']);
                if (preloadedDrs.length == 0 && this.preloadOption['Preloaded'] == false) {
                    this.preloadOption['TripIndex'] = index;
                } else if (preloadedDrs.length > 0 && this.preloadOption['Preloaded'] == false) {
                    this.preloadOption['TripIndex'] = index;
                    this.preloadOption['Preloaded'] = true;
                }
            }
        });
        this.changeDetectorRef.markForCheck();
        this.changeDetectorRef.detectChanges();
    }
    public ngOnDestroy() {
        this.unsubscribeFormChange();
        this.unsubscribeScheduleChangeDetectSubject();
        this.unsubscribeDeliveryPreloadOption();
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
        if (this.GridViewSearchSubscription) {
            this.GridViewSearchSubscription.unsubscribe();
        }
    }
    public unsubscribeUpdateDriverTrailerSubject(): void {
        if (this.UpdateDriverSubscription) {
            this.UpdateDriverSubscription.unsubscribe();
        }
    }

    private unsubscribeGroupChangeDetectSubject(): void {
        if (this.GroupChangeDetectSubscription) {
            this.GroupChangeDetectSubscription.unsubscribe();
        }
    }

    private unsubscribeScheduleChangeDetectSubject(): void {
        if (this.ScheduleChangeDetectSubscription) {
            this.ScheduleChangeDetectSubscription.unsubscribe();
        }
    }

    public unsubscribeFormChange(): void {
        this.dataService.FormChangeSubscription && this.dataService.FormChangeSubscription.forEach(t => {
            if (t) {
                t.unsubscribe();
            }
        });
    }

    private unsubscribeDeliveryPreloadOption(): void {
        if (this.DeliveryPreloadOptionSubscription) {
            this.DeliveryPreloadOptionSubscription.unsubscribe();
        }
    }
    private subscribeFormChange(): void {
        this.unsubscribeFormChange();
        let _trips = this.schedule.controls['Trips'] as FormArray;
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

    private subscribeUpdateDriverTrailerSubject(): void {
        this.unsubscribeUpdateDriverTrailerSubject();
        this.UpdateDriverSubscription = this.dataService.UpdateDriverTrailerSubject.subscribe(data => {
            if (data) {
                if (data.Driver) {
                    var _dArray = this.schedule.controls['Drivers'] as FormArray;
                    _dArray.clear();
                    _dArray.push(this.utilService.getDriverForm(data.Driver));

                    var _tArray = this.schedule.controls['Trailers'] as FormArray;
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

    private subscribeGroupChangeDetectSubject(): void {
        this.GroupChangeDetectSubscription = this.dataService.GroupChangeDetectSubject.subscribe(x => {
            if (x) {
                this.changeDetectorRef.detectChanges();
                this.unsubscribeGroupChangeDetectSubject();
                this.unsubscribeFormChange();
            }
        });
    }
    private subscribeScheduleChangeDetectSubject(): void {
        this.ScheduleChangeDetectSubscription = this.dataService.ScheduleChangeDetectSubject.subscribe(x => {
            if (x) {
                this.changeDetectorRef.detectChanges();
            }
        });
    }

    private subscribeDisableControlsSubject(): void {
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }

    private subscribeDeliveryPreloadOption(): void {
        this.DeliveryPreloadOptionSubscription = this.dataService.DeliveryPreloadOption.pipe(debounceTime(2000)).subscribe(x => {
            if (x) {//&& x.ShiftIndex == this.i && x.ScheduleIndex == this.j
                if (x.Reset) {
                    this.resetPreLoadedFlag();
                }
                this.setPreLoadedFlag();
            }
        });
    }
    addLoad(shiftIdx: number, scheduleIdx: number): void {
        var _tArray = this.schedule.controls['Trips'] as FormArray;
        let _startDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        var shift = this.SbForm.controls['Shifts']['controls'][shiftIdx] as FormGroup;
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

    getSelectedShift(shiftId: string): ScheduleShiftModel {
        let _shifts = this.dataService.AllShiftsSubject.value as ScheduleShiftModel[];
        return _shifts.find(x => x.Id == shiftId);
    }

    getNewLoadStartDate(startDateTime: Date, endDateTime: Date) {
        if (startDateTime > endDateTime) {
            endDateTime.setDate(endDateTime.getDate() + 1);
        }
        return moment(endDateTime).format('MM/DD/YYYY');
    }

    getTrailerTrip(startTime: Date, slotPeriod: number, rowIndex: number, colIndex: number): TripModel {
        if (slotPeriod <= 0) { slotPeriod = 3; }
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

    public editDriverTrailers(shiftIdx: number, scheduleIdx: number): void {
        var driver: DropdownItem = { Id: null, Name: '', Code: '' };
        if (this.schedule.controls['Drivers']['controls'].length > 0) {
            driver = this.schedule.controls['Drivers']['controls'][0].value;
        }
        let _isPublishedRequestsExists = this.IsPublishedRequestsExists();
        var trailers = this.schedule.controls['Trailers'].value;
        let data = {
            Index1: shiftIdx,
            Index2: scheduleIdx,
            Driver: driver.Id,
            Trailers: trailers,
            IsPublishedRequestsExists: _isPublishedRequestsExists,
            IsIncludeAllRegionDriver: this.schedule.controls['IsIncludeAllRegionDriver'].value
        };
        this.SelectedTrailers = trailers;
        this.dataService.setEditDriverTrailerSubject(data);
        this.subscribeUpdateDriverTrailerSubject();
    }
    public IsPublishedRequestsExists(): boolean {
        let isPublished = false;
        let trips = this.schedule.controls['Trips']['controls'] as FormArray;
        for (let i = 0; i < trips.length; i++) {
            var drs = trips[i].controls['DeliveryRequests']['controls'] as FormArray;
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
    public IsDSBGridViewPublishedRequestsExists(shiftRow: any): boolean {
        let isPublished = false;
        let trips = shiftRow.controls['Trips']['controls'] as FormArray;
        for (let i = 0; i < trips.length; i++) {
            var drs = trips[i].controls['DeliveryRequests']['controls'] as FormArray;
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
    publishEntireRow(shiftIndex: number, rowIndex: number): void {
        this.dataService.setPublishEntireRowSubject({ ShiftIndex: shiftIndex, ScheduleIndex: rowIndex, IsOptionalPickup: false, OptionalPickupInfo: null, OrderFuelInfo: null });
    }

    CheckForCancelDSButton(shiftIndex: number, rowIndex: number): boolean {
        let schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex) as FormGroup;
        let trips = schedule.controls['Trips'] as FormArray;
        let _deliveryRequests = this.utilService.GetAllLoadsDR(trips);
        if (_deliveryRequests.length > 0) {
            let DsCount = _deliveryRequests.length;
            let check = this.sbService.returnCommonTracableElements(SBConstants.CancelCompletedDraftStatus, _deliveryRequests, true);
            if (check.length > 0 && DsCount == check.length) {
                return false;
            }
        }
        return true;
    }

    draftScheduleBuilder(trip: FormGroup): void {
        this.dataService.setDraftDeliveryGroupSubject({ trip: trip, filterChanged: false });
    }

    publishScheduleBuilder(i: number, j: number, k: number, schedule: any, trip: FormGroup): void {

        this.dataService.setPublishDeliveryGroupSubject({ shiftIndex: i, rowIndex: j, colIndex: k, schedule: schedule, trip: trip, isOptionalPickup: false, OptionalPickupInfo: null, OrderFuelInfo: null });
    }
    onItemDrop(trip: FormGroup, event: any, shiftIndex: number, rowIndex: number, colIndex: number, schedule: any): void {
        if (this.preloadOption["Preloaded"] == false && this.preloadOption["TripIndex"] < colIndex) {
            this.preloadOption["TripIndex"] = colIndex
        }
        this.dataService.setDragDropItemSubject({ trip: trip, event: event, shiftIndex: shiftIndex, rowIndex: rowIndex, colIndex: colIndex, schedule: schedule });
        this.subscribeGroupChangeDetectSubject();
        this.subscribeFormChange();
        this.sorDrByProductSequence(trip);
    }
    private sorDrByProductSequence(trip: FormGroup) {
        var drForm = trip.controls['DeliveryRequests'] as FormArray;
        let isCommonPickup = trip.controls['IsCommonPickup'].value;
        let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value as boolean;
        let drList = drForm.value || [];
        //drList = sortArrayTwice(drList, 'JobId', 'ProductSequence');
        drForm.clear();
        var _drFormArray = this.utilService.getDeliveryRequestFormArray(drList, isCommonPickup, IsDispatcherDragDropSequence, 0);
        _drFormArray.controls.forEach((_drForm: FormGroup) => {
            drForm.push(_drForm);
        });
    }

    validateTrailerJobCompatibility(drData: any, schedule: any): void {
        var _deliveryRequests = [];
        _deliveryRequests.push(drData.Data.value);
        let _selectedTrailers = schedule.controls["Trailers"].value;
        var trips = schedule.get('Trips') as FormArray;
        if ((_selectedTrailers && _selectedTrailers.length > 0) && (_deliveryRequests && _deliveryRequests.length > 0)) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests).subscribe(data => {
                if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                    this.highLightDRDiv(trips, data);
                    Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                } else {
                    this.highLightDRDiv(trips, null);
                }
            });
        }
    }
    highLightDRDiv(trips: FormArray, data) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i] as FormGroup;
            var deliveryRequests = trip.controls["DeliveryRequests"] as FormArray;
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
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

    private removeClassForAllLoad(trips: FormArray) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i] as FormGroup;
            var deliveryRequests = trip.controls["DeliveryRequests"] as FormArray;
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                    deliveryRequest.controls["IsBlinkDR"].patchValue(false);
                }
            }
        }
    }

    public editExisingGroup(_trip: FormGroup, _shiftIndex: number, _rowIndex: number, _tripIndex: number, _schedule: any, _isPublishLoadInvalid = false): void {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowOpenedDeliveryGroupSubject(true);
    }

    public editGroup(_trip: FormGroup, _shiftIndex: number, _rowIndex: number, _tripIndex: number, _schedule: any, _isPublishLoadInvalid = false, route: any = null) {

        this.dataService.setEditDeliveryGroupSubject({
            trip: _trip,
            shiftIndex: _shiftIndex,
            rowIndex: _rowIndex,
            tripIndex: _tripIndex,
            schedule: _schedule,
            isPublishLoadInvalid: _isPublishLoadInvalid,
            isOptionalPickup: false,
            routeName: route,
            OptionalPickupInfo: null,
            OrderFuelInfo: null
        });
        this.subscribeFormChange();
        this.subscribeGroupChangeDetectSubject();
    }

    private setDeletePostloadedDrsSubject(drs: DeliveryRequestViewModel[]): void {
        let postloadedDrInfo = drs.filter(x => x.PostLoadInfo);
        if (postloadedDrInfo.length > 0) {
            this.dataService.setDeletePostloadSubject(postloadedDrInfo);
        }
    }
    public deleteScheduleDetails(tripDetails: FormGroup, drIndex: number) {
        var scheduleDetails = {
            trip: tripDetails,
            index: drIndex
        };
        this.setDeletePostloadedDrsSubject([tripDetails.controls['DeliveryRequests']['controls'][drIndex].value]);
        this.dataService.setDeleteDRRequestSubject(scheduleDetails);
        this.dataService.setDeliveryPreloadOption({ ShiftIndex: this.i, ScheduleIndex: this.j, Reset: true });
    }
    resetDrByRoutes(_shiftIndex: number, _rowIndex: number, _tripIndex: number, _trip: FormGroup, _schedule: any) {
        let _data = {
            i: _shiftIndex, j: _rowIndex, k: _tripIndex, trip: _trip, schedule: _schedule
        }
        this.resetRouteDetails(_data);
    }

    public deleteDrsForRoute(route: DropdownItem) {
        let _trip = this.SelectedTrip.trip;
        this.editGroup(_trip, _trip.i, _trip.j, _trip.k, _trip.schedule, false, route);
    }

    deleteDrsFromTrip(indexList: number[]) {
        let _trip = this.SelectedTrip.trip;
        let delieveryRequestForm = _trip.controls['DeliveryRequests'] as FormArray;
        let removedDrCount = 0;
        var deletedRequests: DeliveryRequestViewModel[] = [];

        for (let currentIndex = 0; currentIndex < indexList.length; currentIndex++) {

            let formIndex = indexList[currentIndex];
            deletedRequests.push(_trip.controls['DeliveryRequests']['controls'][formIndex - removedDrCount].value);
            delieveryRequestForm.removeAt((formIndex - removedDrCount));
            removedDrCount++;

            if (currentIndex == indexList.length - 1) {
                this.setDeletePostloadedDrsSubject(deletedRequests);
                this.dataService.setRestoreDeletedRequestSubject(deletedRequests);
                this.dataService.setScheduleChangeDetectSubject(true);

                this.editExisingGroup(this.SelectedTrip.trip, this.SelectedTrip.i, this.SelectedTrip.j, this.SelectedTrip.k, this.SelectedTrip.schedule)
            }
        }

        //close modal if no routes 
        if (this.RouteListForTrip.length == 0) {
            let elem = document.getElementById('closeResetDrByRouteModal'); elem.click();
        }
        //
        this.resetDrByRoutes(this.SelectedTrip.i, this.SelectedTrip.j, this.SelectedTrip.k, _trip, this.SelectedTrip.schedule);
    }

    public getDraggableDrsFromLoad(trip: FormGroup): any {
        let drs = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
        let filtered = drs.filter(x => !x.PreLoadedFor && !x.PostLoadedFor && SBConstants.DraggableScheduleStatuses.indexOf(x.TrackScheduleStatus) != -1);
        return filtered;
    }

    public getDraggedDrs(jobId: number, trip: FormGroup): any {
        let drs = trip.controls['DeliveryRequests'].value;
        let filtered = drs.filter(x => x.JobId == jobId && !x.PreLoadedFor && !x.PostLoadedFor);
        return filtered;
    }
    public preloadDelieryForThisLoc(trip: FormGroup, tripIndex: number, jobId: number): void {
        let driverTrailerInfo = this.getAssignedDriverTrailerInfo();
        if (driverTrailerInfo.Trailers.length > 0 || driverTrailerInfo.Drivers.length > 0) {
            let shift = this.SbForm.controls['Shifts']['controls'][this.i] as FormGroup;
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
                } else {
                    let schedule = shift.controls['Schedules']['controls'][this.j] as FormGroup;
                    this.editExisingGroup(trip, this.i, this.j, tripIndex, schedule, false);
                }
            }
        } else {
            Declarations.msgerror(driverTrailerInfo.ErrorMessage, undefined, undefined);
        }
    }

    public isLastTdr(trip: any, tdr: any, isLast: boolean): boolean {
        if (!tdr.controls['IsBlendedRequest'].value) {
            return isLast;
        }
        else {
            let tripDrLength = trip.value.DeliveryRequests.length;
            return trip.controls['DeliveryRequests'].controls[tripDrLength - 1].controls['BlendedGroupId'].value === tdr.controls['BlendedGroupId'].value;
        }
    }

    private validatePreloadDrs(trip: FormGroup, drs: any): boolean {
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
                } else {
                    drs.forEach(x => {
                        x['PickupLocationType'] = pickupType;
                        x['BulkPlant'] = {};
                        x['Terminal'] = trip.controls['Terminal'].value;
                    });
                }
            }
        } else {
            drs.forEach(x => {
                if (x.IsAdditive === false) {
                    if (x.PickupLocationType == 2) {
                        if (!x.BulkPlant || !x.BulkPlant.SiteName || x.BulkPlant.SiteName == '') {
                            isValid = false;
                        }
                    } else {
                        if (!x.Terminal || !x.Terminal.Id || x.Terminal.Id == 0 || x.Terminal.Id == '') {
                            isValid = false;
                        }
                    }
                }
            });
        }
        return isValid;
    }

    private getAssignedDriverTrailerInfo(): any {
        let info = { Drivers: [], Trailers: [], IsTrailerExists: this.IsTrailerExists, ErrorMessage: '' };
        if (info.IsTrailerExists) {
            info.Trailers = this.getAssignedTrailers();
            if (info.Trailers.length == 0) {
                info.ErrorMessage = 'Please assign a trailer to preload this delivery';
            }
        } else {
            info.Drivers = this.getAssignedDrivers();
            if (info.Drivers.length == 0) {
                info.ErrorMessage = 'Please assign a driver to preload this delivery';
            }
        }
        return info;
    }

    private getAssignedTrailers(): [] {
        let trailers = this.schedule.controls['Trailers'].value || [];
        return trailers;
    }

    private getAssignedDrivers(): [] {
        let drivers = this.schedule.controls['Drivers'].value || [];
        return drivers;
    }

    public IntializeChat(driverId: any) {
        this.chatService.pushDriverDetails(driverId);
    }

    public trackByTripIndex(index: number, schedule: any): any {
        return index;
    }

    public trackByTrailerId(index: number, trailer: any): any {
        return trailer.controls["TrailerId"].value;
    }

    public trackByDrId(index: number, dr: any): any {
        return dr.controls["Id"].value;
    }

    public trackByDriverId(index: number, driver: any): any {
        return driver.controls["Id"].value;
    }
    public UnAssignDriverFromShift(shiftrow: any, type: any, trailerId: string = '', sbGridView: number = 0) {
        if (shiftrow != undefined && shiftrow != null) {
            var _trips = shiftrow.controls['Trips'].value;
            var _drivers = shiftrow.controls['Drivers'].value;
            let _trailers = shiftrow.controls['Trailers'].value;

            if (_trips.length > 0 && (_drivers.length > 0 || _trailers.length > 0)) {
                let IsAnyLoadPublished = false;
                if (sbGridView == 1) {
                    IsAnyLoadPublished = this.IsDSBGridViewPublishedRequestsExists(shiftrow);
                }
                else {
                    IsAnyLoadPublished = this.IsPublishedRequestsExists();
                }
                if (IsAnyLoadPublished) {
                    this.dataService.setRemoveTrailer('ERROR');
                    Declarations.msgerror("A load has already been published for this driver. Cannot unassign this driver", undefined, undefined);
                } else {
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
                            } else {
                                this.dataService.setRemoveTrailer('ERROR');
                                Declarations.msgerror(response.StatusMessage, undefined, undefined);
                            }
                            if (response.StatusCode == 0) {
                                var tripsToUpdate = shiftrow.controls['Trips'] as FormArray;
                                var updatedTimeStamp = response.Trips[0].TimeStamp;
                                for (var i = 0; i < tripsToUpdate.length; i++) {
                                    let thisTrip = tripsToUpdate.controls[i] as FormGroup;
                                    thisTrip.patchValue({ TimeStamp: updatedTimeStamp });
                                }
                                let driverctrlarray = shiftrow.controls['Drivers'] as FormArray;
                                if (type === 'driver') {
                                    driverctrlarray.clear();// Clear driver formarray controls
                                    this.schedule.controls['IsIncludeAllRegionDriver'].patchValue(false);
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

    private unassignTrailers(shiftrow: any, type: any, trailerId: string = ''): void {
        if (type === 'trailer') {
            let trailerctrlarray = shiftrow.get('Trailers') as FormArray;
            if (trailerId != '') {
                var trailerIndex = trailerctrlarray.value.findIndex(top => top.Id.toString() == trailerId);
                if (trailerIndex != -1) {
                    this.dataService.setRemoveTrailer(trailerId);
                    trailerctrlarray.removeAt(trailerIndex);
                    var removeTrailer = [trailerId];
                    this.removeCompartmentinfo(removeTrailer, shiftrow);
                    this.dataService.setTrailerInformationSubject(shiftrow);
                }
            }
        }
    }

    public editCompartmentAssignments(): void {
        Declarations.slidePanel("#compartment-assigning", "97%");
        this.dataService.setEditCompartmentAssigmentSubject({ Schedule: this.schedule.value, ShiftIndex: this.i, RowIndex: this.j });
    }
    private updateCompartmentinfo(prevTrailer: any[], _selectedTrailers: any[]): void {
        var removedTrailer = [];
        for (var i = 0, len = prevTrailer.length; i < len; i++) {
            var missingTrailerIndex = _selectedTrailers.findIndex(x => x.Id.toString() == prevTrailer[i].Id.toString());
            if (missingTrailerIndex == -1) {
                removedTrailer.push(prevTrailer[i].Id);
            }
        }
        this.removeCompartmentinfo(removedTrailer, this.schedule);
    }
    private removeCompartmentinfo(removedTrailer: any[], shiftrow: any): void {
        if (this.IsTrailerExists && removedTrailer.length > 0) {
            var _tripsInfo = shiftrow.controls['Trips']['controls'] as FormArray;
            for (var k = 0; k < _tripsInfo.length; k++) {
                var tripsInfoControl = _tripsInfo[k].controls as FormGroup;
                tripsInfoControl['IsTrailerExists'].patchValue(this.IsTrailerExists);
                var deliveryRequest = _tripsInfo[k].controls['DeliveryRequests']['controls'] as FormArray;
                for (var j = 0; j < deliveryRequest.length; j++) {
                    var compartmentInfo = deliveryRequest[j].controls["Compartments"] as FormArray;
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
            var _tripsInfo = shiftrow.controls['Trips']['controls'] as FormArray;
            for (var k = 0; k < _tripsInfo.length; k++) {
                var tripsInfoControl = _tripsInfo[k].controls as FormGroup;
                tripsInfoControl['IsTrailerExists'].patchValue(this.IsTrailerExists);
            }
        }
    }
    truncatestring(source: any, size) {
        let trailerName = '';
        if (source) {
            trailerName = $.map(source.value, function (obj) {
                return obj.TrailerId
            }).join(',');
        }
        if (trailerName != '') {
            return trailerName.length > size ? trailerName.slice(0, size - 1) + "..." : trailerName;
        }
        return trailerName;
    }
    displayBulkPlanName(source: any) {
        var str = '';
        if (source != null && source != '') {
            var stringInfo = source.split(':');
            if (stringInfo != null && stringInfo.length > 0) {
                stringInfo = stringInfo[0].trim();
            }
            else {
                stringInfo = source;
            }
            stringInfo = stringInfo.split(/[ ,-]+/);
            if (stringInfo.length == 1) {
                str = source.substr(0, 1);
                str = str + source.substr(source.length - 1);
                return str;
            }
            else {
                var firststring = stringInfo[0];
                var lastString = stringInfo[stringInfo.length - 1];
                str = firststring.substr(0, 1);
                str = str + lastString.substr(0, 1);
                return str;
            }
        }
        return source;
    }
    GetDeliveryScheduleInfo(jobId: number, tripControl: any, tripIndex: number, drInfo: DeliveryRequestViewModel) {
        this.DeliveryScheduleInfo = [];
        var deliveryRequest = tripControl.controls['DeliveryRequests'] as FormArray;
        var tBDGroupId = drInfo.TBDGroupId;
        //var ScheduleStartTime1 = drInfo.ScheduleStartTime;
        //var ScheduleEndTime2 = drInfo.ScheduleEndTime;
        deliveryRequest.controls.forEach((_drForm: any) => {
            var productDeliveryScheduleInfo = new ProductDeliveryScheduleInfo();
            var jobInfo = _drForm.controls['JobId'].value;
            var tBDFGroupInfo = _drForm.controls['TBDGroupId'].value;
            if (jobId == jobInfo && !drInfo.IsTBD) {
                productDeliveryScheduleInfo.BlendGroupId = _drForm.controls['BlendedGroupId'].value;
                if (_drForm.controls['IsBlendedRequest'].value == false || !this.DeliveryScheduleInfo.some(t => t.BlendGroupId == productDeliveryScheduleInfo.BlendGroupId)) {
                    productDeliveryScheduleInfo.LocationName = _drForm.controls['JobName'].value;
                    this.LocationName = productDeliveryScheduleInfo.LocationName;
                    productDeliveryScheduleInfo.CustomerName = _drForm.controls['CustomerCompany'].value;
                    this.CustomerName = productDeliveryScheduleInfo.CustomerName;
                    productDeliveryScheduleInfo.ScheduleStartTime = _drForm.controls['ScheduleStartTime'].value;
                    productDeliveryScheduleInfo.ScheduleEndTime = _drForm.controls['ScheduleEndTime'].value;
                    this.IsTBDRequest = _drForm.controls['IsTBD'].value;
                    productDeliveryScheduleInfo.Status = _drForm.controls['Status'].value;
                    productDeliveryScheduleInfo.StatusClassId = _drForm.controls['StatusClassId'].value;
                    productDeliveryScheduleInfo.Priority = _drForm.controls['Priority'].value;
                    productDeliveryScheduleInfo.IsBlinkDR = _drForm.controls['IsBlinkDR'].value;
                    productDeliveryScheduleInfo.ScheduleQuantityType = _drForm.controls['ScheduleQuantityType'].value;
                    productDeliveryScheduleInfo.UoM = _drForm.controls['UoM'].value;
                    productDeliveryScheduleInfo.IsAutoCreatedDR = _drForm.controls['IsAutoCreatedDR'].value;
                    productDeliveryScheduleInfo.DelReqSource = _drForm.controls['DelReqSource'].value;
                    productDeliveryScheduleInfo.isRecurringSchedule = _drForm.controls['isRecurringSchedule'].value;
                    productDeliveryScheduleInfo.IsCommonPickup = tripControl.controls['IsCommonPickup'].value;
                    productDeliveryScheduleInfo.CommonPickupLocationType = tripControl.controls['PickupLocationType'].value;
                    productDeliveryScheduleInfo.ScheduleQuantityTypeText = _drForm.controls['ScheduleQuantityTypeText'].value;
                    this.getDRInfo(_drForm, productDeliveryScheduleInfo);
                    if (productDeliveryScheduleInfo.IsCommonPickup) {
                        if (productDeliveryScheduleInfo.CommonPickupLocationType == 2) {
                            productDeliveryScheduleInfo.Address = tripControl.controls['BulkPlant'].controls['Address'].value;
                            productDeliveryScheduleInfo.City = tripControl.controls['BulkPlant'].controls['City'].value;
                            productDeliveryScheduleInfo.Code = tripControl.controls['BulkPlant'].controls['State'].controls['Code'].value;
                            productDeliveryScheduleInfo.ZipCode = tripControl.controls['BulkPlant'].controls['ZipCode'].value;
                        }
                        else {
                            productDeliveryScheduleInfo.PickupLocationName = tripControl.controls['Terminal'].controls['Name'].value;
                        }
                    }
                    productDeliveryScheduleInfo.IsFilldInvoke = _drForm.controls['IsFilldInvoke'].value;
                    productDeliveryScheduleInfo.BadgeNo1 = _drForm.controls['BadgeNo1'].value;
                    productDeliveryScheduleInfo.BadgeNo2 = _drForm.controls['BadgeNo2'].value;
                    productDeliveryScheduleInfo.BadgeNo3 = _drForm.controls['BadgeNo3'].value;
                    if (_drForm.controls['RouteInfo'].value && _drForm.controls['RouteInfo'].value.Name) {
                        productDeliveryScheduleInfo.RouteName = _drForm.controls['RouteInfo'].value.Name;
                    }
                    var badgeInfo = [productDeliveryScheduleInfo.BadgeNo1, productDeliveryScheduleInfo.BadgeNo2, productDeliveryScheduleInfo.BadgeNo3];
                    productDeliveryScheduleInfo.BadgeNoInfo = badgeInfo.filter(function (val) { return val; }).join(", ");
                    productDeliveryScheduleInfo.TankMaxFill = _drForm.controls['TankMaxFill'].value;
                    if (_drForm.controls['PreLoadedFor']) {
                        productDeliveryScheduleInfo.isPreload = true;
                    }
                    if (_drForm.controls['PostLoadedFor']) {
                        productDeliveryScheduleInfo.isPostload = true;
                    }
                    productDeliveryScheduleInfo.TripIndex = tripIndex;
                    productDeliveryScheduleInfo.IsSelectedForAssignment = _drForm.controls['IsSelectedForAssignment'].value;
                    //Marine Nomination                
                    productDeliveryScheduleInfo.IsMarine = _drForm.controls['IsMarine'].value;
                    this.DeliveryScheduleInfo.push(productDeliveryScheduleInfo);
                }
                else {
                    productDeliveryScheduleInfo = this.DeliveryScheduleInfo.find(t => t.BlendGroupId == productDeliveryScheduleInfo.BlendGroupId);
                    this.getDRInfo(_drForm, productDeliveryScheduleInfo);
                }
            }
            else if (tBDGroupId != null && tBDGroupId == tBDFGroupInfo && drInfo.IsTBD == true) {
                productDeliveryScheduleInfo.LocationName = _drForm.controls['JobName'].value;
                this.LocationName = productDeliveryScheduleInfo.LocationName;
                productDeliveryScheduleInfo.CustomerName = _drForm.controls['CustomerCompany'].value;
                this.CustomerName = productDeliveryScheduleInfo.CustomerName;
                productDeliveryScheduleInfo.ScheduleStartTime = _drForm.controls['ScheduleStartTime'].value;
                productDeliveryScheduleInfo.ScheduleEndTime = _drForm.controls['ScheduleEndTime'].value;
                this.IsTBDRequest = _drForm.controls['IsTBD'].value;
                productDeliveryScheduleInfo.Status = _drForm.controls['Status'].value;
                productDeliveryScheduleInfo.StatusClassId = _drForm.controls['StatusClassId'].value;
                productDeliveryScheduleInfo.Priority = _drForm.controls['Priority'].value;
                productDeliveryScheduleInfo.IsBlinkDR = _drForm.controls['IsBlinkDR'].value;
                productDeliveryScheduleInfo.ScheduleQuantityType = _drForm.controls['ScheduleQuantityType'].value;
                productDeliveryScheduleInfo.UoM = _drForm.controls['UoM'].value;
                productDeliveryScheduleInfo.IsAutoCreatedDR = _drForm.controls['IsAutoCreatedDR'].value;
                productDeliveryScheduleInfo.DelReqSource = _drForm.controls['DelReqSource'].value;
                productDeliveryScheduleInfo.isRecurringSchedule = _drForm.controls['isRecurringSchedule'].value;
                productDeliveryScheduleInfo.IsCommonPickup = tripControl.controls['IsCommonPickup'].value;
                productDeliveryScheduleInfo.CommonPickupLocationType = tripControl.controls['PickupLocationType'].value;
                productDeliveryScheduleInfo.ScheduleQuantityTypeText = _drForm.controls['ScheduleQuantityTypeText'].value;
                this.getDRInfo(_drForm, productDeliveryScheduleInfo);
                if (productDeliveryScheduleInfo.IsCommonPickup) {
                    if (productDeliveryScheduleInfo.CommonPickupLocationType == 2) {
                        productDeliveryScheduleInfo.Address = tripControl.controls['BulkPlant'].controls['Address'].value;
                        productDeliveryScheduleInfo.City = tripControl.controls['BulkPlant'].controls['City'].value;
                        productDeliveryScheduleInfo.Code = tripControl.controls['BulkPlant'].controls['State'].controls['Code'].value;
                        productDeliveryScheduleInfo.ZipCode = tripControl.controls['BulkPlant'].controls['ZipCode'].value;
                    }
                    else {
                        productDeliveryScheduleInfo.PickupLocationName = tripControl.controls['Terminal'].controls['Name'].value;
                    }
                }
                productDeliveryScheduleInfo.IsFilldInvoke = _drForm.controls['IsFilldInvoke'].value;
                productDeliveryScheduleInfo.BadgeNo1 = _drForm.controls['BadgeNo1'].value;
                productDeliveryScheduleInfo.BadgeNo2 = _drForm.controls['BadgeNo2'].value;
                productDeliveryScheduleInfo.BadgeNo3 = _drForm.controls['BadgeNo3'].value;
                if (_drForm.controls['RouteInfo'].value && _drForm.controls['RouteInfo'].value.Name) {
                    productDeliveryScheduleInfo.RouteName = _drForm.controls['RouteInfo'].value.Name;
                }
                var badgeInfo = [productDeliveryScheduleInfo.BadgeNo1, productDeliveryScheduleInfo.BadgeNo2, productDeliveryScheduleInfo.BadgeNo3];
                productDeliveryScheduleInfo.BadgeNoInfo = badgeInfo.filter(function (val) { return val; }).join(", ");
                productDeliveryScheduleInfo.TankMaxFill = _drForm.controls['TankMaxFill'].value;
                if (_drForm.controls['PreLoadedFor']) {
                    productDeliveryScheduleInfo.isPreload = true;
                }
                if (_drForm.controls['PostLoadedFor']) {
                    productDeliveryScheduleInfo.isPostload = true;
                }
                productDeliveryScheduleInfo.TripIndex = tripIndex;
                productDeliveryScheduleInfo.IsSelectedForAssignment = _drForm.controls['IsSelectedForAssignment'].value;
                //Marine Nomination                
                productDeliveryScheduleInfo.IsMarine = _drForm.controls['IsMarine'].value;
                this.DeliveryScheduleInfo.push(productDeliveryScheduleInfo);
            }
        });
        this.changeDetectorRef.detectChanges();
    }

    private getDRInfo(_drForm: any, productDeliveryScheduleInfo: ProductDeliveryScheduleInfo) {
        let drInfo = new BlendDRDetails();
        if (_drForm.controls['IsAdditive'].value == true) {
            drInfo.ProductName = _drForm.controls['FuelType'].value;
        }
        else {
            drInfo.ProductName = _drForm.controls['ProductType'].value;
        }
        drInfo.IsAdditive = _drForm.controls['IsAdditive'].value;
        drInfo.RequiredQuantity = _drForm.controls['RequiredQuantity'].value;
        drInfo.PickupLocationType = _drForm.controls['PickupLocationType'].value;
        drInfo.Id = _drForm.controls['Id'].value;
        drInfo.DeliveryDateStartTime = _drForm.controls['DeliveryDateStartTime'].value;
        drInfo.TrackScheduleStatusName = _drForm.controls['TrackScheduleStatusName'].value;
        drInfo.Vessel = _drForm.controls['Vessel'].value;
        drInfo.Berth = _drForm.controls['Berth'].value;
        drInfo.IsBlendedRequest = _drForm.controls['IsBlendedRequest'].value;
        drInfo.Priority = _drForm.controls['Priority'].value;
        drInfo.StatusClassId = _drForm.controls['StatusClassId'].value;
        drInfo.IsBlinkDR = _drForm.controls['IsBlinkDR'].value;
        drInfo.ScheduleStartTime = _drForm.controls['ScheduleStartTime'].value;
        drInfo.ScheduleEndTime = _drForm.controls['ScheduleEndTime'].value;
        if (drInfo.PickupLocationType == 2) {
            drInfo.Address = _drForm.controls['BulkPlant'].controls['Address'].value;
            drInfo.City = _drForm.controls['BulkPlant'].controls['City'].value;
            drInfo.Code = _drForm.controls['BulkPlant'].controls['State'].controls['Code'].value;
            drInfo.ZipCode = _drForm.controls['BulkPlant'].controls['ZipCode'].value;
        }
        else {
            drInfo.PickupLocationName = _drForm.controls['Terminal'].controls['Name'].value;
        }
        productDeliveryScheduleInfo.DrInfo.push(drInfo);
    }

    public TransferDS(jobId: number, trip: FormGroup, DrIndex: number, ShiftIndex: number, RowIndex: number, ColIndex: number, schedule: any) {
        let data = {
            jobId: jobId,
            trip: trip,
            DrIndex: DrIndex,
            ShiftIndex: ShiftIndex,
            RowIndex: RowIndex,
            ColIndex: ColIndex,
            schedule: schedule
        };
        this.dataService.setTransferDSSubject(data);
    }

    public fnIsLastDr(trip, tdr) {
        var drInfo = tdr.value as DeliveryRequestViewModel;
        var isLastReq = false;
        if (!drInfo.IsTBD) {
            var jobId = drInfo.JobId;
            var drID = tdr.controls["Id"].value;
            var drs = trip.value.DeliveryRequests as DeliveryRequestViewModel[];
            var finalDRs = [];
            drs.forEach(x => {
                if (x.IsBlendedRequest == true && x.IsBlendedDrParent == true) {
                    finalDRs.push(x);
                }
                else {
                    finalDRs.push(x);
                }
            });
            var jobDrs = finalDRs.filter(t => t.JobId == jobId);
            isLastReq = drID == jobDrs[jobDrs.length - 1].Id;
        }
        else {
            var TBDGroupId = tdr.controls["TBDGroupId"].value;
            var drID = tdr.controls["Id"].value;
            var tbddrs = trip.value.DeliveryRequests;
            var jobtbDrs = tbddrs.filter(t => t.TBDGroupId == TBDGroupId);
            isLastReq = drID == jobtbDrs[jobtbDrs.length - 1].Id;
        }
        return isLastReq;
    }
    public fnIsLastDrSeq(trip, tdr) {
        var drInfo = tdr.value as DeliveryRequestViewModel;
        var isLastReq = false;
        if (!drInfo.IsTBD) {
            var jobId = drInfo.JobId;
            var drID = tdr.controls["Id"].value;
            var drs = trip.value.DeliveryRequests as DeliveryRequestViewModel[];
            var finalDRs = [];
            drs.forEach(x => {
                if (x.IsBlendedRequest == true && x.IsBlendedDrParent == true) {
                    finalDRs.push(x);
                }
                else {
                    finalDRs.push(x);
                }
            });
            var jobDrs = finalDRs.filter(t => t.JobId == jobId);
            isLastReq = drID == jobDrs[jobDrs.length - 1].Id;
        }
        else {
            var TBDGroupId = drInfo.TBDGroupId;
            var drTBDID = drInfo.Id;
            var drTBDs = trip.value.DeliveryRequests;
            var jobTBDDrs = drTBDs.filter(t => t.TBDGroupId == TBDGroupId);
            isLastReq = drTBDID == jobTBDDrs[jobTBDDrs.length - 1].Id;
        }
        return isLastReq;
    }
    public getTrailerInfo(trailerControl: any) {
        if (this.TrailerInformationChangeDetectSubscription) {
            this.TrailerInformationChangeDetectSubscription.unsubscribe();
        }
        this.subscribeTrailerInformationDetectSubject();
        this.dataService.setTrailerInformationSubject(trailerControl);
    }
    subscribeTrailerInformationDetectSubject(): void {
        this.TrailerInformationChangeDetectSubscription = this.dataService.UnAssignTrailerFromShift.subscribe(x => {
            if (x) {
                this.UnAssignDriverFromShift(x.schedule, x.type, x.trailerId, 1);
            }
        });
    }
    private resetRouteDetails(x: any) {
        this.RouteListForTrip = [];
        if (x) {

            this.SelectedTrip = x;
            let drForm = this.SelectedTrip.trip.controls['DeliveryRequests'] as FormArray;
            let drValue = drForm ? drForm.value as DeliveryRequestViewModel[] : [];
            drValue.forEach(dr => {
                if (dr && dr.RouteInfo && dr.PostLoadedFor == undefined) {
                    let drIndex = this.RouteListForTrip.findIndex(a => a.Id == dr.RouteInfo.Id);
                    if (drIndex === -1) { this.RouteListForTrip.push(dr.RouteInfo); }
                }
            });
            if (this.RouteListForTrip.length > 0) {
                var routeParamInfo = {
                    SelectedTrip: this.SelectedTrip,
                    RouteListForTrip: this.RouteListForTrip
                };
                let _trip = this.SelectedTrip;
                let _routeTripInfo = this.RouteListForTrip;
                this.dataService.setRouteDetailsSubject(routeParamInfo);
                this.editGroup(_trip.trip, _trip.i, _trip.j, _trip.k, _trip.schedule, false, _routeTripInfo);
            }
            else {
                var routeParamInfo = {
                    SelectedTrip: this.SelectedTrip,
                    RouteListForTrip: this.RouteListForTrip
                };
                this.dataService.setRouteDetailsSubject(routeParamInfo);
                let elem = document.getElementById('closeResetDrByRouteModal'); elem.click();
            }
            this.changeDetectorRef.detectChanges();
        }
    }
    subscribeGridViewSearchEvents(): void {
        this.GridViewSearchSubscription = this.dataService.GridViewSearchGroupSubject.subscribe(x => {
            if (x) {

                let searchLocation = x.searchLocation;
                this.filterGridView(searchLocation);

            }
        });
    }
    public filterGridView(searchLocation: string) {
        try {
            let shiftCounts = 0;
            var shifts = this.SbForm.controls["Shifts"] as FormArray;
            shiftCounts = shifts.length;
            for (var i = 0; i < shifts.controls.length; i++) {
                let shiftInfo = shifts.controls[i] as FormGroup;
                let _schedules = shiftInfo.controls['Schedules'] as FormArray;
                for (var j = 0; j < _schedules.controls.length; j++) {
                    let scheduleInfo = _schedules.controls[j] as FormGroup;
                    let trips = scheduleInfo.get('Trips') as FormArray;
                    for (var k = 0; k < trips.controls.length; k++) {
                        let tripInfo = trips.controls[k] as FormGroup;
                        let deliveryReqs = tripInfo.get('DeliveryRequests') as FormArray;
                        for (var l = 0; l < deliveryReqs.controls.length; l++) {
                            let delReq = deliveryReqs.controls[l] as FormGroup;
                            if (searchLocation != '') {
                                searchLocation = searchLocation.trim().toLowerCase();
                                let jobName = delReq.controls['JobName'].value;
                                if (jobName != '' && jobName != null) {
                                    jobName = jobName.trim().toLowerCase();
                                    if (jobName.indexOf(searchLocation) != -1) {
                                        delReq.controls['IsJobFilter'].patchValue(true);
                                    }
                                    else {
                                        delReq.controls['IsJobFilter'].patchValue(false);
                                    }
                                }
                                else {
                                    delReq.controls['IsJobFilter'].patchValue(true);
                                }
                            }
                            else {
                                delReq.controls['IsJobFilter'].patchValue(true);
                            }

                        }
                    }
                }
                if ((i + 1) == shiftCounts) {
                    this.changeDetectorRef.detectChanges();
                }
            }
        } catch (e) {
            console.error(e);
            this.changeDetectorRef.detectChanges();
        }
    }
    moveColumnToLoadQueue(): void {

        //apply client side first
        this.schedule.get('IsLoadQueueCollapsed').patchValue(true);
        this.loadQueueService.setLoadQueueColumnMoved(true);
        this.changeDetectorRef.markForCheck();
        this.dataService.setDriverColumnChangeDetect(true);

        //send collapsed columns to api
        const data: DSBLoadQueueModel = {
            Id: null,
            RegionId: this.SbForm.get('RegionId').value,
            ShiftIndex: this.i,
            Date: this.SbForm.get('Date').value,
            DriverRowIndex: this.j,
            ScheduleBuilderId: '',
            ShiftId: this.ShiftId,
        };

        this.loadQueueService.createLoadQueue([data]).subscribe((data) => {
            if (data && data.StatusCode == 0) {
                if (data.DsbLoadQueueSuccess && data.DsbLoadQueueSuccess.length > 0) {
                    this.schedule.get('LoadQueueId').patchValue(data.DsbLoadQueueSuccess[0].Id);
                }
            } else {
                Declarations.msgerror("Unable to collapse column. Please try later.", undefined, undefined);
                //if api fail, revert client side changes also
                this.schedule.get('IsLoadQueueCollapsed').patchValue(false);
                this.loadQueueService.setLoadQueueColumnMoved(true);
                this.changeDetectorRef.markForCheck();
                this.dataService.setDriverColumnChangeDetect(true);
            }
        });
    }
    //carrier assignmnet
    drSelectedForCarrierAssignment(event: any, drId: string, tripIndex: number) {
        let drs = this.schedule.controls['Trips']['controls'][tripIndex]['controls']['DeliveryRequests'] as FormArray;
        drs.controls.forEach((drForm: FormControl) => {
            if (drForm.get('Id').value == drId) {
                drForm.get('IsSelectedForAssignment').setValue(event.target.checked);
            }
        })
    }
    isAllTipsDrSelected(_drs: any) {
        let drs = _drs.value || [];
        return drs.every(dr => dr.IsSelectedForAssignment == true);
    }
    triptSelectedForCarrierAssignment(event: any, tripIndex: number) {
        let drs = this.schedule.controls['Trips']['controls'][tripIndex]['controls']['DeliveryRequests'] as FormArray;
        drs.controls.forEach((drForm: FormControl) => {
            if (drForm.get('IsSelectedForAssignment').value != event.target.checked) {
                drForm.get('IsSelectedForAssignment').setValue(event.target.checked);
            }
        })
    }
    isDrExistInColumn(_Trips: AbstractControl) {
        let TripsArray = _Trips.value || [];
        return TripsArray.some(trip => trip.DeliveryRequests.length > 0);
    }
    isAllColumnDrSelected(_Trips: AbstractControl) {
        let TripsArray = _Trips.value || [];
        return TripsArray.every(trip => trip.DeliveryRequests.every(dr => dr.IsSelectedForAssignment == true));
    }
    columnSelectedForCarrierAssignment(event, _Trips: AbstractControl) {
        _Trips['controls'].forEach((trip: FormControl) => {
            let drs = trip['controls']['DeliveryRequests'] as FormArray;
            drs.controls.forEach((drForm: FormControl) => {
                if (drForm.get('IsSelectedForAssignment').value != event.target.checked) {
                    drForm.get('IsSelectedForAssignment').setValue(event.target.checked);
                }
            })
        })
    }
    //add Optional pickup for particular column in DSB.
    addOptionalPickup(schedule: FormGroup) {
        var orderIds = [];
        let tripsControl = schedule['controls']['Trips'] as FormArray;
        tripsControl.controls.forEach((delR: FormGroup) => {
            let drControl = delR.controls['DeliveryRequests'] as FormArray;
            drControl.controls.forEach((delGroup: FormGroup) => {
                if (orderIds.findIndex(x => x == delGroup.get('OrderId').value) == -1) {
                    orderIds.push(delGroup.get('OrderId').value);
                }
            });
        });
        if (orderIds.length > 0) {
            this.sbService.getOrderFuelTypes(orderIds).subscribe(data => {
                let finalObj = { shiftIndex: this.i, shiftOrderNo: this.shiftOrderNo, ShiftId: this.ShiftId, RegionId: this.SbForm.get('RegionId').value, ScheduleBuilderId: this.SbForm.get('Id').value, ColIndex: this.j, scheduleInfo: schedule, FuelTypeInfo: data };
                this.dataService.setOptionalPickupInfo(finalObj);
            });
        }
        else {
            let finalObj = { shiftIndex: this.i, shiftOrderNo: this.shiftOrderNo, ShiftId: this.ShiftId, RegionId: this.SbForm.get('RegionId').value, ScheduleBuilderId: this.SbForm.get('Id').value, ColIndex: this.j, scheduleInfo: schedule, FuelTypeInfo: null };
            this.dataService.setOptionalPickupInfo(finalObj);

        }

    }
    cancelEntireRow(shiftIndex: number, rowIndex: number): void {
        this.dataService.setCancelEntireRowSubject({ ShiftIndex: shiftIndex, ScheduleIndex: rowIndex, IsOptionalPickup: false, OptionalPickupInfo: null, OrderFuelInfo: null });
    }

    LoadLevelCacelButtonHideShow(trip: FormGroup): boolean {
        let delReqs = trip.get('DeliveryRequests').value as DeliveryRequestViewModel[];
        if (delReqs.length > 0) {
            let loadDrLength = delReqs.length;
            let checkCommonElements = this.sbService.returnCommonTracableElements(SBConstants.CancelCompletedDraftStatus, delReqs, true);
            if (checkCommonElements.length > 0 && loadDrLength == checkCommonElements.length) {
                return false;
            }
        }
        return true;
    }

    cancelScheduleBuilder(i: number, j: number, k: number, schedule: any, trip: FormGroup): void {
        this.dataService.setCancelDSDeliveryGroupSubject({ shiftIndex: i, rowIndex: j, colIndex: k, schedule: schedule, trip: trip, isOptionalPickup: false, OptionalPickupInfo: null, OrderFuelInfo: null });
    }
    public CancelLocationDS(jobId: number, trip: FormGroup, isTBD: number = 0, TBDGroupId: string = '') {
        let data = {
            jobId: jobId,
            trip: trip,
            isTBD: isTBD,
            TBDGroupId: TBDGroupId
        };
        this.dataService.setCancelDSLocationDSSubject(data);
    }
    DoNotShowCancelDSButton(jobId: number, trip: FormGroup) {
        let buttonstatus = true;
        let EnrouteInCompleted = [3, 7, 8, 9, 22, 23];
        let EnrouteInProgress = [1, 3, 11, 12, 13, 14, 15, 16, 17, 18];
        if (this.disableControl) {
            buttonstatus = false;
        }
        let drs = trip.controls['DeliveryRequests'].value;
        let filteredDRs = drs.filter(x => x.JobId == jobId && !x.PreLoadedFor && !x.PostLoadedFor && x.TrackableScheduleId > 0) as DeliveryRequestViewModel[];
        if (filteredDRs.length == 0) {
            buttonstatus = false;
        }
        else {
            filteredDRs.forEach(x => {
                if (EnrouteInProgress.indexOf(x.TrackScheduleEnrouteStatus) == -1 || EnrouteInCompleted.indexOf(x.TrackScheduleStatus) == -1) {
                    buttonstatus = false;
                }
            });
        }
        return buttonstatus;
    }
    public CancelButtonVisible(trip, tdr) {
        var buttonShowStatus = false;
        var jobId = tdr.controls["JobId"].value;
        var drs = trip.value.DeliveryRequests;
        var isTBD = drs.filter(t => t.IsTBD == true).length > 0 ? true : false;
        var jobDrs = drs.filter(t => t.JobId == jobId) as DeliveryRequestViewModel[];
        if (jobDrs.length > 0 && !isTBD) {
            buttonShowStatus = this.CancelButtonVisibleInfo(jobDrs);
        }
        else {
            var tBDGroupId = tdr.controls["TBDGroupId"].value;
            var drs = trip.value.DeliveryRequests;
            var jobDrs = drs.filter(t => t.JobId == jobId) as DeliveryRequestViewModel[];
            var tbdDrs = drs.filter(t => t.TBDGroupId == tBDGroupId) as DeliveryRequestViewModel[];
            if (jobDrs.length > 0) {
                buttonShowStatus = this.CancelButtonVisibleInfo(jobDrs);
            }
            if (tbdDrs.length > 0) {
                buttonShowStatus = this.CancelButtonVisibleInfo(tbdDrs);
            }
        }
        return buttonShowStatus;
    }
    public CancelButtonVisibleInfo(jobDrs: DeliveryRequestViewModel[]) {
        let buttonShowStatus = true;
        if (jobDrs.filter(x1 => x1.StatusClassId != 3).length >= 0) {
            if (jobDrs.filter(x1 => x1.StatusClassId == 5).length == jobDrs.length) {
                buttonShowStatus = false;
                return buttonShowStatus;
            }
            else if (jobDrs.filter(x1 => x1.StatusClassId == 0).length == jobDrs.length) {
                buttonShowStatus = false;
                return buttonShowStatus;
            }
            else if (jobDrs.filter(x1 => x1.StatusClassId == 0).length > 0) {
                buttonShowStatus = false;
                return buttonShowStatus;
            }
            else if (jobDrs.filter(x1 => x1.TrackScheduleStatus == 5).length == jobDrs.length) {
                buttonShowStatus = false;
                return buttonShowStatus;
            }
            else if (jobDrs.filter(x1 => x1.StatusClassId == 4).length == jobDrs.length) {
                buttonShowStatus = false;
                return buttonShowStatus;
            }

            buttonShowStatus = true;
        }
        return buttonShowStatus;
    }
    public LocationSequenceTrip(direction: number, jobId: number, drInfo: DeliveryRequestViewModel, lIndex: number, trip: FormGroup) {
        var drs = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
        if (direction == 1) {
            if (!drInfo.IsTBD) {
                var fjobId = this.findLocationSeq(drs, direction, jobId);
                var jobDRs = drs.filter(x => x.JobId == jobId);
                var finalDRs = drs.filter(x => x.JobId != jobId);
                var finalDRsIndex = finalDRs.findIndex(x => x.JobId == fjobId);
                jobDRs.forEach(x => {
                    finalDRs.splice(finalDRsIndex, 0, x);
                    finalDRsIndex = finalDRsIndex + 1;
                });
                let data = {
                    DRs: finalDRs,
                    trip: trip,
                };
                this.dataService.setDispatcherLoadDragDropSubject(data);
            }
            else {
                let currentDRIndex = drs.findIndex(x => x.TBDGroupId == drInfo.TBDGroupId);
                currentDRIndex = currentDRIndex - 1;
                var finalDRJobID = drs[currentDRIndex].JobId;
                var finalDRIsTBD = drs[currentDRIndex].IsTBD;
                var finalDRTBDGroupId = drs[currentDRIndex].TBDGroupId;
                if (finalDRIsTBD) {
                    currentDRIndex = drs.findIndex(x => x.TBDGroupId == finalDRTBDGroupId)
                }
                else {
                    currentDRIndex = drs.findIndex(x => x.JobId == finalDRJobID)
                }
                var jobDRs = drs.filter(x => x.TBDGroupId == drInfo.TBDGroupId);
                var finalDRs = drs.filter(x => x.TBDGroupId != drInfo.TBDGroupId);
                jobDRs.forEach(x => {
                    finalDRs.splice(currentDRIndex, 0, x);
                    currentDRIndex = currentDRIndex + 1;
                });
                let data = {
                    DRs: finalDRs,
                    trip: trip,
                };
                this.dataService.setDispatcherLoadDragDropSubject(data);
            }
        }
        else {
            if (!drInfo.IsTBD) {
                var fjobId = this.findLocationSeq(drs, direction, jobId);
                var jobDRs = drs.filter(x => x.JobId == jobId);
                var finalDRs = drs.filter(x => x.JobId != jobId);
                var finalDRsIndex = -1;
                finalDRs.forEach((x, index: number) => {
                    if (x.JobId == fjobId) {
                        finalDRsIndex = index;
                    }
                });
                finalDRsIndex = finalDRsIndex + 1;
                jobDRs.forEach(x => {
                    finalDRs.splice(finalDRsIndex, 0, x);
                    finalDRsIndex = finalDRsIndex + 1;
                });
                let data = {
                    DRs: finalDRs,
                    trip: trip,
                };
                this.dataService.setDispatcherLoadDragDropSubject(data);
            }
            else {
                let currentDRIndex = drs.findIndex(x => x.TBDGroupId == drInfo.TBDGroupId);
                currentDRIndex = lIndex;
                var finalDRJobID = drs[currentDRIndex + 1].JobId;
                var finalDRIsTBD = drs[currentDRIndex + 1].IsTBD;
                var finalDRTBDGroupId = drs[currentDRIndex + 1].TBDGroupId;
                var jobDRs = drs.filter(x => x.TBDGroupId == drInfo.TBDGroupId);
                var finalDRs = drs.filter(x => x.TBDGroupId != drInfo.TBDGroupId);
                finalDRs.forEach((x, index: number) => {
                    if (x.JobId == finalDRJobID && !finalDRIsTBD) {
                        currentDRIndex = index;
                    }
                    else if (x.TBDGroupId == finalDRTBDGroupId && finalDRIsTBD && finalDRTBDGroupId != null) {
                        currentDRIndex = index;
                    }
                });
                currentDRIndex = currentDRIndex + 1;
                jobDRs.forEach(x => {
                    finalDRs.splice(currentDRIndex, 0, x);
                    currentDRIndex = currentDRIndex + 1;
                });
                let data = {
                    DRs: finalDRs,
                    trip: trip,
                };
                this.dataService.setDispatcherLoadDragDropSubject(data);
            }
        }
    }
    public findLocationSeq(drs: DeliveryRequestViewModel[], direction: number, jobId: number) {
        let lastDRIndex = drs.length;
        let currentJObDRIndex = drs.findIndex(x => x.JobId == jobId);
        let findex = -1;
        if (direction == 1) {
            findex = drs[currentJObDRIndex - 1].JobId;
        }
        else {
            let finaldrs = drs.filter((value, index) => index >= currentJObDRIndex && index <= lastDRIndex);
            finaldrs.forEach((value) => {
                if (value.JobId != jobId) {
                    if (findex == -1) {
                        findex = value.JobId;
                    }
                    return;
                }
            });
        }
        return findex;
    }
    public findFirstJobInfo(trip: any, tdr: DeliveryRequestViewModel, jobId: number): boolean {
        var firstRecord = true;
        let drs = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
        var finalDRs = [];
        drs.forEach(x => {
            if (x.IsBlendedRequest == true && x.IsBlendedDrParent == true) {
                finalDRs.push(x);
            }
            else {
                finalDRs.push(x);
            }
        });
        if (!tdr.IsTBD) {
            var drJobINdex = finalDRs.findIndex(x => x.JobId == jobId);
            if (drJobINdex == 0) {
                firstRecord = false;
            }
        }
        else {
            var drJobINdex = finalDRs.findIndex(x => x.TBDGroupId == tdr.TBDGroupId);
            if (drJobINdex == 0) {
                firstRecord = false;
            }
        }
        return firstRecord;
    }
    setJobSequence(trip: FormGroup) {
        this.dataService.setLoadLocationSequenceSubject(trip)
    }
    public findLastInfo(trip, tdr) {
        var drInfo = tdr.value as DeliveryRequestViewModel;
        var isLastReq = false;
        if (!drInfo.IsTBD) {
            var jobId = drInfo.JobId;
            var drID = tdr.controls["Id"].value;
            var drs = trip.value.DeliveryRequests as DeliveryRequestViewModel[];
            var finalDRs = [];
            drs.forEach(x => {
                if (x.IsBlendedRequest == true && x.IsBlendedDrParent == true) {
                    finalDRs.push(x);
                }
                else {
                    finalDRs.push(x);
                }
            });
            var jobDrs = finalDRs.filter(t => t.JobId == jobId);
            isLastReq = drID == jobDrs[jobDrs.length - 1].Id;
            if (isLastReq == true) {
                if (drID == finalDRs[finalDRs.length - 1].Id) {
                    isLastReq = false;
                }
            }
        }
        else {
            var TBDGroupId = tdr.controls["TBDGroupId"].value;
            var drID = tdr.controls["Id"].value;
            var tbddrs = trip.value.DeliveryRequests;
            var jobtbDrs = tbddrs.filter(t => t.TBDGroupId == TBDGroupId);
            isLastReq = drID == jobtbDrs[jobtbDrs.length - 1].Id;
        }
        return isLastReq;
    }

    getClasses(deliveryScheduleInfo: any, dr: any) {

        if (deliveryScheduleInfo && deliveryScheduleInfo.DrInfo && deliveryScheduleInfo.DrInfo.length > 0 && deliveryScheduleInfo.DrInfo.some(dr => dr.IsBlendedRequest)) {
            return dr ? this.getClassString(dr, true) : 'border pt-2'; //parent div
        } else {
            return this.getClassString(deliveryScheduleInfo, false);
        }
    }

    getClassString(deliveryScheduleInfo: any, isblend: boolean) {
        let classes = isblend ? 'overflow-hidden dr-round ' : ''; //child div

        //status
        if (deliveryScheduleInfo.StatusClassId == 1)
            classes += 'dr-new';
        else if (deliveryScheduleInfo.StatusClassId == 2)
            classes += 'dr-inprogress';
        else if (deliveryScheduleInfo.StatusClassId == 3)
            classes += 'dr-cancelled dr-text-white';
        else if (deliveryScheduleInfo.StatusClassId == 4)
            classes += 'dr-completed';
        else if (deliveryScheduleInfo.StatusClassId == 5)
            classes += 'diverted-status dr-text-white';
        //priority
        if (deliveryScheduleInfo.Priority == 1)
            classes += ' must-go';
        else if (deliveryScheduleInfo.Priority == 2)
            classes += ' should-go';
        else if (deliveryScheduleInfo.Priority == 3)
            classes += ' could-go';

        //blink
        if (deliveryScheduleInfo.IsBlinkDR)
            classes += ' dr-notCompatible animated infinite flash';

        return classes;
    }
}