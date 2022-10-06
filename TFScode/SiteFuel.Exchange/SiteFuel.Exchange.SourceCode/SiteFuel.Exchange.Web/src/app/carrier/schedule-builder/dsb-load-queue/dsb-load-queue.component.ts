import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { LoadQueueService } from './load-queue.service';
import { Declarations } from 'src/app/declarations.module';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownItem, ShiftLoadQueueDropdownItem } from 'src/app/statelist.service';
import { Subscription } from 'rxjs';
import { LoadQueueColumnValidations, LoadQueueStatusViewModel, TrailerJobNonCompatibleDrs, TrailersDeliveryRequestViewModel, TripError } from './dsb-load.model';
import { DSBSaveModel, RegionDetailModel, TripModel, DeliveryRequestViewModel, OrderPickupLocationModel } from '../../models/DispatchSchedulerModels';
import { DataService } from 'src/app/services/data.service';
import { ScheduleBuilderService } from '../../service/schedule-builder.service';
import { ColumnStatusEnum, DeliveryReqStatus, DSBLoadQueueStatus, TripStatus } from 'src/app/app.enum';

@Component({
    selector: 'app-dsb-load-queue',
    templateUrl: './dsb-load-queue.component.html',
    styleUrls: ['./dsb-load-queue.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class DsbLoadQueueComponent implements OnInit, OnDestroy {

    @Input() SbForm: FormGroup;
    @Input() public RegionDetail: RegionDetailModel;
    @Input() public IsTrailerExists: boolean;

    LoadQueueFilterForm: FormGroup;
    subscriptions: Subscription = new Subscription();
    columnValidations: LoadQueueColumnValidations[] = [];
    validationObj = new LoadQueueColumnValidations();
    InCompatibleDrs: TrailerJobNonCompatibleDrs[] = [];
    compatibilityModels: TrailersDeliveryRequestViewModel[] = [];
    dsbModelsToPublish: DSBSaveModel[] = [];
    public OrderList: any = {};
    public statuses: DropdownItem[] = [
        { Id: ColumnStatusEnum.Completed, Code: null, Name: 'Completed' },
        { Id: ColumnStatusEnum.Partial, Code: null, Name: 'Partially Completed' },
        { Id: ColumnStatusEnum.Empty, Code: null, Name: 'Empty' },
        { Id: ColumnStatusEnum.Published, Code: null, Name: 'Published' },
    ];
    public selectedStatuses: DropdownItem[] = []
    public settings: IDropdownSettings = {
        singleSelection: false,
        idField: 'Id',
        textField: 'Name',
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        itemsShowLimit: 1,
        allowSearchFilter: true
    };
    public shifts: ShiftLoadQueueDropdownItem[] = [];
    public selectedShifts: DropdownItem[] = [];
    public _opened: boolean = false;
    public _isQueuePanelInitiated: boolean = false;
    public _selectAll: boolean = false;
    _loading: boolean = false;
    public panels: string[] = ['panel-1', 'panel-2', 'panel-3'];
    @Input() public isDisableControl: boolean;

    constructor(
        private loadQueueService: LoadQueueService,
        private changeDetectorRef: ChangeDetectorRef,
        private dataService: DataService, private sbService: ScheduleBuilderService) { }

    ngOnInit() {
        this.LoadQueueFilterForm = this.loadQueueService.getFilterForm();
        this.subscribeFormChanges();
    }

    ngOnDestroy(): void {
        this.unSubscribeFormChanges();
    }
    subscribeFormChanges() {
        this.subscriptions.add(this.LoadQueueFilterForm.valueChanges
            .subscribe(change => {
                this.applyFilter();
            }))

        this.subscriptions.add(this.loadQueueService.loadQueueColumnMovedSubject
            .subscribe(value => {
                this.setSelectAllButtonvalue();
                this.bindShifts();
                this.bindValidation();
            }))
    }

    openPanel() {
        this._opened = true;
        if (!this._isQueuePanelInitiated) {
            this._isQueuePanelInitiated = true;
            this.loadQueueService.setLoadQueueColumnMoved(true);
        }
    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }

    setSelectAllButtonvalue() {
        let allSchedules = 0;
        let selectedSchedules = 0;

        let _shifts = this.SbForm.controls['Shifts'].value as any[] || [];
        _shifts.forEach((shift) => {
            shift && shift.Schedules && shift.Schedules.forEach((schedule) => {
                if (schedule.IsLoadQueueCollapsed) {
                    allSchedules++;
                    if (schedule.IsColumnSelected)
                        selectedSchedules++;
                }
            });
        });

        if (allSchedules && selectedSchedules == allSchedules)
            this._selectAll = true;
        else
            this._selectAll = false;
    }

    applyFilter() {
        this.selectedShifts = this.LoadQueueFilterForm.controls['Shift'].value || [];
        this.selectedStatuses = this.LoadQueueFilterForm.controls['Status'].value || [];
        this._selectAll = false;
        let _shifts = this.SbForm.controls['Shifts'] as FormArray;
        _shifts.controls.forEach((shift: FormGroup, shiftIndex: number) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            let shiftId = shift.controls['Id'].value;
            schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {
                //un-select all columns
                schedule.get('IsColumnSelected').setValue(false);
                //apply filter only for collapsed columns
                if (schedule.get('IsLoadQueueCollapsed').value) {
                    //both
                    if (this.selectedShifts.length > 0 && this.selectedStatuses.length > 0) {
                        if (this.selectedShifts.some(sh => sh.Id == shift.get('Id').value && this.getColumnStatus(shiftIndex, scheduleIndex, shiftId))) {
                            schedule.get('LoadQueueFilterVisibility').setValue(true);
                        }
                        else {
                            schedule.get('LoadQueueFilterVisibility').setValue(false);
                        }
                    }
                    //shift
                    else if ((this.selectedShifts.length > 0 && this.selectedStatuses.length == 0)) {
                        if (this.selectedShifts.some(sh => sh.Id == shift.get('Id').value)) {
                            schedule.get('LoadQueueFilterVisibility').setValue(true);
                        }
                        else {
                            schedule.get('LoadQueueFilterVisibility').setValue(false);
                        }
                    }
                    //status
                    else if ((this.selectedShifts.length == 0 && this.selectedStatuses.length > 0)) {
                        if (this.getColumnStatus(shiftIndex, scheduleIndex, shiftId)) {
                            schedule.get('LoadQueueFilterVisibility').setValue(true);
                        }
                        else {
                            schedule.get('LoadQueueFilterVisibility').setValue(false);
                        }
                    }
                    else {
                        schedule.get('LoadQueueFilterVisibility').setValue(true);
                    }
                }
            });
        });

    }

    getColumnStatus(shiftIndex: number, scheduleIndex: number, shiftId: string) {
        let obj = this.columnValidations.find(val => val.ShiftId == shiftId && val.ScheduleIndex == scheduleIndex && this.selectedStatuses.some(s => s.Id == val.ColumnStatus));
        if (obj) {
            return true;
        }
        else
            return false;
    }

    bindShifts() {
        this.shifts = [];
        let _shifts = this.SbForm.controls['Shifts'] as FormArray;
        _shifts.controls.forEach((shift: FormGroup, index: number) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            schedules.controls.forEach((schedule: FormGroup) => {
                if (schedule.get('IsLoadQueueCollapsed').value && !this.shifts.some(sh => sh.Id == shift.get('Id').value)) {
                    this.shifts.push({ Id: shift.get('Id').value, Code: shift.get('OrderNo').value.toString(), Name: 'Shift ' + (shift.get('OrderNo').value + 1), OrderNo: shift.get('OrderNo').value + 1 });
                }
            });
        });
        this.shifts = this.shifts.sort(this.SortByOrder);
    }

    checkShiftVisibility(shift: FormGroup) {
        let _shiftSelected = this.LoadQueueFilterForm.controls['Shift'].value as ShiftLoadQueueDropdownItem[];
        if (_shiftSelected == null) {
            let _schedules = shift.controls['Schedules'].value as any[];
            if (_schedules.some(x => x.IsLoadQueueCollapsed == true) && _schedules.some(x => x.LoadQueueFilterVisibility == true))//&& shift.get('LoadQueueFilterVisibility').value
                return true;
            else
                return false;
        }
        else if (_shiftSelected.length == 0) {
            let _schedules = shift.controls['Schedules'].value as any[];
            if (_schedules.some(x => x.IsLoadQueueCollapsed == true) && _schedules.some(x => x.LoadQueueFilterVisibility == true))//&& shift.get('LoadQueueFilterVisibility').value
                return true;
            else
                return false;
        }
        else {
            var shiftIndex = _shiftSelected.findIndex(x => x.Id == shift.controls["Id"].value);
            if (shiftIndex >= 0) {
                let _schedules = shift.controls['Schedules'].value as any[];
                if (_schedules.some(x => x.IsLoadQueueCollapsed == true) && _schedules.some(x => x.LoadQueueFilterVisibility == true))//&& shift.get('LoadQueueFilterVisibility').value
                    return true;
                else
                    return false;
            }
            else {
                return false;
            }
        }
    }

    getClassByStatus(i: number, j: number) {
        let obj = this.columnValidations.find(val => val.ShiftIndex == i && val.ScheduleIndex == j) || new LoadQueueColumnValidations();
        let schedule = this.SbForm.get('Shifts.' + i + '.Schedules.' + j + '') as FormGroup;
        switch (obj.ColumnStatus) {
            case ColumnStatusEnum.Completed:
                //here verify the completed column published from notifcation or not.
                if (schedule != null) {
                    let loadQueueColumnStatus = schedule.get('LoadQueueColumnStatus').value;
                    if (loadQueueColumnStatus == DSBLoadQueueStatus.Success) {
                        return "published";
                    }
                }
                return 'completed';
            case ColumnStatusEnum.Published:
                return 'published';
            case ColumnStatusEnum.Empty:
                return 'empty';
            case ColumnStatusEnum.Partial:
                return 'partial';
            case ColumnStatusEnum.None:
                return 'partial';
        }
    }
    selectColumn(event: any, schedule: FormGroup) {
        schedule.get('IsColumnSelected').setValue(event.target.checked);
        this.setSelectAllButtonvalue();
    }

    selectAllColumn() {
        this._selectAll = !this._selectAll;
        let _shifts = this.SbForm.controls['Shifts'] as FormArray;
        _shifts.controls.forEach((shift: FormGroup) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            schedules.controls.forEach((schedule: FormGroup) => {
                if (schedule.get('IsLoadQueueCollapsed').value) {
                    schedule.get('IsColumnSelected').setValue(this._selectAll);
                }
            });
        });
    }

    moveColumnsToGrid() {
        //updated client side first
        let shifts = this.SbForm.controls['Shifts'] as FormArray;
        let loadQueueIds: string[] = [];

        shifts.controls.forEach((shift: FormGroup) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            schedules.controls.forEach((schedule: FormGroup) => {
                if (schedule.get('IsColumnSelected').value) {
                    schedule.get('IsLoadQueueCollapsed').setValue(false);
                    schedule.get('IsColumnSelected').setValue(false);////unselect
                    loadQueueIds.push(schedule.get('LoadQueueId').value);////collect object ids
                }
            });
        });

        if (loadQueueIds.length) {

            this.loadQueueService.setLoadQueueColumnMoved(true);//refresh load queue panel
            this.changeDetectorRef.markForCheck();//detect form changes
            this.dataService.setDriverColumnChangeDetect(true);

            //delete expanded column data from api
            this.loadQueueService.deleteLoadQueue(loadQueueIds).subscribe((data) => {
                if (!data || data.StatusCode !== 0) {
                    Declarations.msgerror("Unable to move selected column(s). Please try later.", undefined, undefined);
                    //if api fails to update, revert changes
                    shifts.controls.forEach((shift: FormGroup) => {
                        let schedules = shift.controls['Schedules'] as FormArray;
                        schedules.controls.forEach((schedule: FormGroup) => {
                            if (schedule.get('IsColumnSelected').value) {
                                schedule.get('IsLoadQueueCollapsed').setValue(true);
                            }
                        });
                    });
                    this.loadQueueService.setLoadQueueColumnMoved(true);
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setDriverColumnChangeDetect(true);
                }
            });
        }
    }

    getValidationsAndDrsFromLoad(trips: FormArray, validation: LoadQueueColumnValidations, scheduleIndex: number, shiftIndex: number) {

        var _drList = [];

        for (let tripIndex = 0; tripIndex < trips.length; tripIndex++) {

            let tripError = new TripError();
            tripError.TripIndex = tripIndex;

            let trip = trips.controls[tripIndex] as FormGroup;
            var isCommonPickup: boolean = trip.controls['IsCommonPickup'].value;
            var _drsForm = trip.controls["DeliveryRequests"] as FormArray;

            /////LOAD LEVEL PICKUP VALIDATION/////////////
            if (_drsForm.controls.length > 0 && isCommonPickup && !(trip.controls.Terminal.get('Name').value || trip.controls.BulkPlant.get('SiteName').value)) {
                tripError.Errors.push('Please select common pickup location.');
            }

            for (let i = 0; i < _drsForm.controls.length; i++) {
                var x = _drsForm.controls[i] as FormGroup;
                let dr = x.value
                if (x) {
                    if (dr.ScheduleQuantityType == 1 && dr.TankMaxFill > 0 && dr.RequiredQuantity > dr.TankMaxFill) {
                        tripError.Errors.push('Required quantity is more than Max Fill for dr ' + (i + 1) + '.');
                    }
                    _drList.push(dr);
                    this.getOrderList(x, isCommonPickup, trip.controls['StartDate'].value, tripError, i);
                }
            }
            validation.TripErrors.push(tripError);
        }
        return _drList;
    }

    getValidationsByColumn(schedule: FormGroup, validation: LoadQueueColumnValidations, scheduleIndex: number, shiftIndex: number) {
        let completedDRStatus = [3, 7, 8, 9, 11, 12, 14, 21, 22, 23, 25];
        var trips = schedule.controls['Trips'] as FormArray;
        var _deliveryRequests: DeliveryRequestViewModel[] = this.getValidationsAndDrsFromLoad(trips, validation, scheduleIndex, shiftIndex);
        let NotpublishTripCount = 0;

        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;

        if (_deliveryRequests && _deliveryRequests.length > 0) {

            let jobAndCustomers = this.loadQueueService.getCustomerAndJobFromDr(_deliveryRequests);
            validation.Customers = jobAndCustomers.customers;
            validation.Locations = jobAndCustomers.Locations;
            validation.DrCount = _deliveryRequests.length;

            _deliveryRequests.forEach(dr => {
                if (completedDRStatus.indexOf(dr.TrackScheduleStatus) == -1) {
                    NotpublishTripCount = NotpublishTripCount + 1;
                }
                else if (dr.TrackScheduleStatus == DeliveryReqStatus.ScheduleCreated && dr.TrackableScheduleId == null) {
                    NotpublishTripCount = NotpublishTripCount + 1;
                }
            });

            if (drivers == null || drivers == undefined || drivers.length == 0 || (this.IsTrailerExists && (trailers == null || trailers == undefined || trailers.length == 0))) {
                if (this.IsTrailerExists) {
                    validation.Errors.push('Please assign a driver / trailer to publish');
                }
                else {
                    validation.Errors.push('Please assign a driver to publish');
                }
            }

            if (drivers != null && drivers.length > 0) {
                var delReqs = _deliveryRequests;
                if (delReqs.length > 0 && delReqs.some(t => t.IsFilldInvoke == true)) {
                    if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                        validation.Errors.push('Please assign a Filld compatible driver / trailer to publish', 'Driver/Trailer Required');
                    }
                    else if (drivers.some(t => t.IsFilldCompatible == false)) {
                        validation.Errors.push('Please assign a Filld compatible driver to publish', 'Driver Required');
                    }
                }
            }
        }

        //ColumnStatus
        if (validation.DrCount == 0) {
            validation.ColumnStatus = ColumnStatusEnum.Empty;
        }
        else if (validation.Errors.length > 0 || validation.TripErrors.some(tr => tr.Errors.length > 0)) {
            validation.ColumnStatus = ColumnStatusEnum.Partial;
        }
        else if (NotpublishTripCount == 0) {
            validation.ColumnStatus = ColumnStatusEnum.Published;
        }
        else {
            validation.ColumnStatus = ColumnStatusEnum.Completed;
        }
        this.columnValidations.push(validation);
    }

    setColumnStatusAfterApi() {
        this.columnValidations.forEach((validation: LoadQueueColumnValidations) => {
            if (validation.DrCount == 0) {
                validation.ColumnStatus = ColumnStatusEnum.Empty;
            }
            else if (validation.Errors.length > 0 || validation.TripErrors.some(tr => tr.Errors.length > 0)) {
                validation.ColumnStatus = ColumnStatusEnum.Partial;
            }
        });
    }

    bindValidation() {

        this.columnValidations = [];

        if (this.RegionDetail) {
            let _shifts = this.SbForm.controls['Shifts'] as FormArray;
            _shifts.controls.forEach((shift: FormGroup, shiftIndex: number) => {
                let schedules = shift.controls['Schedules'] as FormArray;
                let shiftId = shift.controls['Id'].value;
                schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {
                    if (schedule.get('IsLoadQueueCollapsed').value) {
                        let validation = new LoadQueueColumnValidations();
                        validation.ShiftIndex = shiftIndex;
                        validation.ScheduleIndex = scheduleIndex;
                        validation.ShiftId = shiftId;
                        this.getValidationsByColumn(schedule, validation, scheduleIndex, shiftIndex);
                    }
                });
            });
        }
    }

    setValidationObjectFromList(i: number, j: number) {
        this.validationObj = this.columnValidations.find(val => val.ShiftIndex == i && val.ScheduleIndex == j) || new LoadQueueColumnValidations();
    }

    setTripStatus(trip: TripModel): void {
        if (trip) {
            var tripPrevStatusId = trip.TripPrevStatus;
            var tripStatusId = TripStatus.Added;
            if (tripPrevStatusId == TripStatus.None) {
                tripStatusId = TripStatus.Added;
            } else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified) {
                tripStatusId = TripStatus.Modified;
            }
            trip.TripStatus = tripStatusId;
        }
    }

    iniatePublishColumns() {
        this.getColumnForCompatibilityCheck();
        this.getTrailerJobCompatibility();
    }

    getTrailerJobCompatibility() {
        if (this.IsTrailerExists) {
            if (this.compatibilityModels && this.compatibilityModels.length > 0) {

                this._loading = true;
                this.InCompatibleDrs = []

                this.loadQueueService.getTrailerJobCompatibility(this.compatibilityModels).subscribe(
                    (response) => {

                        this._loading = false;
                        if (response && response.deliveryRequestsNotCompatible && response.deliveryRequestsNotCompatible.length > 0) {
                            this.InCompatibleDrs = response.deliveryRequestsNotCompatible;

                            if (this.InCompatibleDrs.some(el => el.drCount > 0)) {
                                let tempString = ''
                                this.InCompatibleDrs.forEach(element => {
                                    if (element.drCount > 0) {
                                        tempString + '' + element.scheduleIndex + ', ';
                                    }
                                })
                                Declarations.msgerror("Non-compatible trailer(s) is assigned to Location(s) in columns " + tempString, 'Warning', 4000);
                            }
                            else {
                                this.publishValidatedColumns();
                            }
                        }
                    }
                );
            }
            else {
                Declarations.msgwarning('No completed columns(s) found for publish load queue.', 'Warning', 4000);
            }
        }
        else {
            this.publishValidatedColumns();
        }
    }

    publishValidatedColumns() {

        let selected_schedule = [];
        let selectedSchedules = 0;
        let selectedDSBModelsToPublish: DSBSaveModel[] = [];
        let _shifts = this.SbForm.controls['Shifts'].value as any[] || [];

        _shifts.forEach((shift, shiftIndex: number) => {
            shift && shift.Schedules && shift.Schedules.forEach((schedule, scheduleIndex: number) => {
                if (schedule.IsLoadQueueCollapsed) {
                    if (schedule.IsColumnSelected) {
                        selectedSchedules++;
                        var obj = {
                            shiftIndex: shiftIndex,
                            scheduleIndex: scheduleIndex,
                            shiftId: shift.Id
                        }
                        selected_schedule.push(obj);
                    }
                }
            });
        });
        if (selectedSchedules > 0) {
            for (var i = 0; i < selected_schedule.length; i++) {
                let obj = this.columnValidations.find(val => val.ShiftId == selected_schedule[i].shiftId && val.ScheduleIndex == selected_schedule[i].scheduleIndex);
                if (obj.ColumnStatus == ColumnStatusEnum.Completed) {
                    let _shifts = this.SbForm.controls['Shifts']["controls"][selected_schedule[i].shiftId] as FormGroup;
                    if (_shifts != null) {
                        let schedule = _shifts.controls['Schedules']["controls"][selected_schedule[i].scheduleIndex] as FormGroup;
                        var dsbModel = this.loadQueueService.getDSBSaveModel(this.SbForm);
                        var drivers = schedule.controls['Drivers'].value;
                        var trailers = schedule.controls['Trailers'].value;
                        var trips = schedule.controls['Trips'] as FormArray;
                        for (let i = 0; i < trips.length; i++) {
                            var tripValue = trips.value[i];
                            this.setTripStatus(tripValue);
                            dsbModel.Trips.push(tripValue);
                        }
                        dsbModel.Trips.forEach(t => { t.Drivers = drivers, t.Trailers = trailers });
                        dsbModel.Status = 3;
                        selectedDSBModelsToPublish.push(dsbModel);
                    }
                }
            }
            //start publish
            if (selectedDSBModelsToPublish.length == 0) {
                Declarations.msgwarning('No completed columns(s) found for publish load queue.', 'Warning', 4000);
            }
            else {
                Declarations.msgwarning('Only completed columns(s) publish from load queue.', 'Warning', 4000);
                this.intializePublish(selectedDSBModelsToPublish, 1);
            }
        }
        else {
            //start publish
            this.intializePublish(this.dsbModelsToPublish, 0);
        }
    }

    getColumnForCompatibilityCheck() {
        this._loading = true;
        this.compatibilityModels = [];
        this.dsbModelsToPublish = [];

        let _shifts = this.SbForm.controls['Shifts'] as FormArray;
        _shifts.controls.forEach((shift: FormGroup, shiftIndex: number) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {

                if (schedule.get('IsLoadQueueCollapsed').value && schedule.get('LoadQueueFilterVisibility').value && schedule.get('IsLoadQueueColumnBlocked').value != true) {

                    let isValidForCompatibilityCheck = true;
                    var drivers = schedule.controls['Drivers'].value;
                    var trailers = schedule.controls['Trailers'].value;
                    if (drivers == null || drivers == undefined || drivers.length == 0 || (this.IsTrailerExists && (trailers == null || trailers == undefined || trailers.length == 0))) {
                        isValidForCompatibilityCheck = false;
                    }
                    else {
                        var trips = schedule.controls['Trips'] as FormArray;
                        var _deliveryRequests = this.loadQueueService.GetAllLoadsDR(trips);
                        var _deliveryRequests = this.loadQueueService.GetAllLoadsDR(trips);
                        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
                            return trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
                        });

                        if (drivers != null && drivers.length > 0) {

                            if (_deliveryRequests.length > 0 && _deliveryRequests.some(t => t.IsFilldInvoke == true)) {
                                if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                                    isValidForCompatibilityCheck = false;
                                }
                                else if (drivers.some(t => t.IsFilldCompatible == false)) {
                                    isValidForCompatibilityCheck = false;
                                }
                            }
                        }

                        if (isValidForCompatibilityCheck && _deliveryRequests && _deliveryRequests.length > 0 && ((this.IsTrailerExists === true && _selectedTrailers && _selectedTrailers.length > 0) || this.IsTrailerExists === false)) {

                            var validTrips = [];

                            trips.controls.forEach((thisTrip: FormGroup, tripIndex: number) => {
                                if (thisTrip && (thisTrip.valid && this.loadQueueService.validatePublishLoad(thisTrip))) {
                                    ////////////////////////
                                    let model = new TrailersDeliveryRequestViewModel();
                                    model.deliveryRequests = _deliveryRequests;
                                    model.trailers = _selectedTrailers;
                                    model.scheduleIndex = scheduleIndex;
                                    model.shiftIndex = shiftIndex;
                                    this.compatibilityModels.push(model);
                                    ///////////////
                                    validTrips.push(tripIndex);
                                }
                            });


                            if (validTrips.length > 0) {
                                var dsbModel = this.loadQueueService.getDSBSaveModel(this.SbForm);
                                var drivers = schedule.controls['Drivers'].value;
                                var trailers = schedule.controls['Trailers'].value;

                                for (let i = 0; i < trips.length; i++) {
                                    var tripValue = trips.value[i];
                                    if (validTrips.includes(tripValue.DriverColIndex)) {
                                        this.setTripStatus(tripValue);
                                    }
                                    dsbModel.Trips.push(tripValue);
                                }
                                dsbModel.Trips.forEach(t => { t.Drivers = drivers, t.Trailers = trailers });
                                dsbModel.Status = 3;
                                let obj = this.columnValidations.find(val => val.ShiftIndex == shiftIndex && val.ScheduleIndex == scheduleIndex);
                                if (obj != null && obj.ColumnStatus == ColumnStatusEnum.Completed) {
                                    this.dsbModelsToPublish.push(dsbModel);
                                }
                            }

                        }
                    }
                }
            });
        });
        this._loading = false;
    }
    getLoadQueueStatus(status: number, shiftIndex: number, scheduleIndex: number) {
        if (status == 0) {
            return "In-Queue";
        }
        else if (status == 1) {
            return "In-Progress"
        }
        else if (status == 2) {
            return "Success"
        }
        else if (status == 3) {
            var columnValidations = this.columnValidations.find(top => top.ShiftIndex == shiftIndex && top.ScheduleIndex == scheduleIndex);
            if (columnValidations != null) {
                if (columnValidations.ColumnStatus == ColumnStatusEnum.Published) {
                    return "";
                }
            }
            return "Failed"
        }
    }
    private getOrderList(delReq: FormGroup, isCommonPickup: boolean, startDate: string, tripError: TripError, drIndex: number): void {

        var _jobId = delReq.controls['JobId'].value;
        var _productTypeId = delReq.controls['ProductTypeId'].value;
        var _orderId = delReq.controls['OrderId'];
        var _carrierStatus = delReq.controls['CarrierStatus'].value;
        let isBlendReq = delReq.controls['IsBlendedRequest'].value;
        var existing = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlendReq ? 1 : 0)];

        if (existing == undefined || existing == null || existing.length == 0) {
            this._loading = true;
            this.sbService.getOrders(_jobId, _productTypeId, startDate, _carrierStatus, isBlendReq).subscribe(data => {
                this._loading = false;
                this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlendReq ? 1 : 0)] = data;
                if (data.length > 0) {
                    var order = data[0];
                    if (_orderId && _orderId.value > 0) {
                        var orderFromData = data.filter(t => t.OrderId == _orderId.value);
                        if (orderFromData != null && orderFromData.length > 0) {
                            order = orderFromData[0];
                        }
                    }
                    if (_orderId.value == null || _orderId.value == 0) {
                        delReq.controls['OrderId'].setValue(order.OrderId);
                    }
                    if ((delReq.value.Terminal == null || delReq.value.Terminal.Id == 0) && (delReq.value.BulkPlant == null || delReq.value.BulkPlant.SiteName == '' || delReq.value.BulkPlant.SiteName == null) && !isCommonPickup) {
                        let location = OrderPickupLocationModel.ToLocation(order);
                        this.setPickupLocation(delReq, location);
                    }
                }
                else {
                    if (delReq.value.OrderId == null || delReq.value.OrderId == 0 || delReq.value.OrderId == undefined || delReq.value.OrderId == '') {
                        tripError.Errors.push('Order not available for dr number ' + (drIndex + 1) + '.');
                        this.setColumnStatusAfterApi();
                    }
                }
                /////DR LEVEL PICKUP VALIDATION/////////////
                if (!isCommonPickup && delReq.get('PickupLocationType').value != 2 && !delReq.controls.Terminal.get('Name').value) {
                    tripError.Errors.push('Please select pickup location for dr number ' + (drIndex + 1) + '.');
                    this.setColumnStatusAfterApi();
                }
                else if (!isCommonPickup && delReq.get('PickupLocationType').value == 2 && !delReq.controls.BulkPlant.get('SiteName').value) {
                    tripError.Errors.push('Please select bulk plant for dr number ' + (drIndex + 1) + '.');
                    this.setColumnStatusAfterApi();
                }
            });
        }
        else {

            if (_orderId.value == undefined || _orderId.value == null || _orderId.value == '') {
                _orderId.setValue(existing[0].OrderId);
                let location = OrderPickupLocationModel.ToLocation(existing[0]);
                this.setPickupLocation(delReq, location);
            }
            else if ((delReq.value.Terminal == null || delReq.value.Terminal.Id == 0) && (delReq.value.BulkPlant == null || delReq.value.BulkPlant.SiteName == '' || delReq.value.BulkPlant.SiteName == null) && !isCommonPickup) {
                var order = existing[0];
                if (_orderId.value != undefined && _orderId.value != null && _orderId.value > 0) {
                    var orderFromData = existing.filter(t => t.OrderId == _orderId.value);
                    if (orderFromData != null && orderFromData.length > 0) {
                        order = orderFromData[0];
                    }
                }
                let location = OrderPickupLocationModel.ToLocation(order);
                this.setPickupLocation(delReq, location);
            }
            else {
                if (delReq.value.OrderId == null || delReq.value.OrderId == 0 || delReq.value.OrderId == undefined || delReq.value.OrderId == '') {
                    tripError.Errors.push('Order not available for dr number ' + (drIndex + 1) + '.');
                    this.setColumnStatusAfterApi();
                }
            }
            /////DR LEVEL PICKUP VALIDATION/////////////
            if (!isCommonPickup && delReq.get('PickupLocationType').value != 2 && !delReq.controls.Terminal.get('Name').value) {
                tripError.Errors.push('Please select pickup location for dr number ' + (drIndex + 1) + '.');
                this.setColumnStatusAfterApi();
            }
            else if (!isCommonPickup && delReq.get('PickupLocationType').value == 2 && !delReq.controls.BulkPlant.get('SiteName').value) {
                tripError.Errors.push('Please select bulk plant for dr number ' + (drIndex + 1) + '.');
                this.setColumnStatusAfterApi();
            }
        }
    }
    setPickupLocation(form: FormGroup, order: OrderPickupLocationModel): void {
        form.controls['PickupLocationType'].patchValue(order.PickupLocationType);
        if (order.PickupLocationType != 2) {
            if (order.Terminal) {
                form.controls['Terminal'].patchValue(order.Terminal);
            }
        } else {
            if (order.BulkPlant) {
                if (order.BulkPlant.ZipCode) {
                    order.BulkPlant.ZipCode = order.BulkPlant.ZipCode.toUpperCase();
                }
                form.controls['BulkPlant'].patchValue(order.BulkPlant);
            }
        }
    }

    public intializePublish(dsbModelsToPublish: DSBSaveModel[], status: number = 0) {
        if (dsbModelsToPublish.length > 0) {
            this._loading = true;
            this.loadQueueService.saveDsbLoadQueue(dsbModelsToPublish).subscribe((responce: LoadQueueStatusViewModel) => {
                if (responce && responce.StatusCode == 0) {

                    //get all error messages if exist
                    let apiErrSring = '';
                    if (responce.LoadQueueErrorInfo.length) {
                        responce.LoadQueueErrorInfo.forEach(loadQueue => {
                            if (loadQueue.Messages.length) {
                                apiErrSring = apiErrSring + loadQueue.Messages.join(", ");
                            }
                        });
                    }
                    if (status == 0) {
                        if (responce.LoadQueueSuccessInfo.length > 0 && responce.LoadQueueErrorInfo.length > 0) {
                            Declarations.msgsuccess('Publish successfully initiated for ' + responce.LoadQueueSuccessInfo.length + ' out of ' + (responce.LoadQueueSuccessInfo.length + responce.LoadQueueErrorInfo.length) + ' columns.', 'Success', 4000);
                            Declarations.msgerror('Publish falied for ' + responce.LoadQueueErrorInfo.length + ' columns with errors -' + apiErrSring, 'Error', 4000);
                        }
                        else if (responce.LoadQueueSuccessInfo.length > 0) {
                            Declarations.msgsuccess('Publish successfully initiated for all the columns.', 'Success', 4000);
                        }
                        else if (responce.LoadQueueErrorInfo.length > 0) {
                            Declarations.msgerror('All the columns you are trying to publish have been failed.' + apiErrSring, 'Error', 4000);
                        }
                    }
                    else {
                        if (responce.LoadQueueSuccessInfo.length > 0 && responce.LoadQueueErrorInfo.length > 0) {
                            Declarations.msgsuccess('Publish successfully initiated for ' + responce.LoadQueueSuccessInfo.length + ' out of ' + (responce.LoadQueueSuccessInfo.length + responce.LoadQueueErrorInfo.length) + ' columns.', 'Success', 4000);
                            Declarations.msgerror('Publish falied for ' + responce.LoadQueueErrorInfo.length + ' columns with errors -' + apiErrSring, 'Error', 4000);
                        }
                        else if (responce.LoadQueueSuccessInfo.length > 0) {
                            Declarations.msgsuccess('Publish successfully initiated for ' + responce.LoadQueueSuccessInfo.length + ' columns.', 'Success', 4000);
                        }
                        else if (responce.LoadQueueErrorInfo.length > 0) {
                            Declarations.msgerror('Selected the columns you are trying to publish have been failed.' + apiErrSring, 'Error', 4000);
                        }
                    }
                    //bind status and block columns
                    if (responce && responce.LoadQueueSuccessInfo.length > 0) {
                        responce.LoadQueueSuccessInfo.forEach(sch => {
                            var columnValidation = this.columnValidations.find(top => top.ShiftIndex == sch.ShiftIndex && top.ScheduleIndex == sch.DriverColIndex);
                            if (columnValidation != null && columnValidation.ColumnStatus != ColumnStatusEnum.Published) {
                                let schedule = this.SbForm.get('Shifts.' + sch.ShiftIndex + '.Schedules.' + sch.DriverColIndex + '') as FormGroup;
                                schedule.get('IsLoadQueueColumnBlocked').setValue(true);
                                schedule.get('IsColumnSelected').setValue(false);
                                schedule.get('LoadQueueColumnStatus').setValue(DSBLoadQueueStatus.New);
                            }
                        });
                        this.dataService.setDsbQueueChangesForNotification(true);
                    }
                }
                else {
                    Declarations.msgerror('Unable to Publish columns. Please try again later.', 'Error', 4000);
                }
                this._loading = false;
            })
        }
        else {
            Declarations.msgwarning('No completed columns(s) found for publish load queue.', 'Warning', 4000);
        }
    }
    public SortByOrder(a: ShiftLoadQueueDropdownItem, b: ShiftLoadQueueDropdownItem) {
        var aOrderNo = a.OrderNo;
        var bOrderNo = b.OrderNo;
        return ((aOrderNo < bOrderNo) ? -1 : ((aOrderNo > bOrderNo) ? 1 : 0));
    }
}


