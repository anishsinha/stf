import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { addHours } from 'date-fns';
import { Subject } from 'rxjs';
import { CalendarView, CalendarViewPeriod } from 'angular-calendar';
import { CarrierService } from '../../carrier/service/carrier.service';
import { DispatcherService} from '../../carrier/service/dispatcher.service';
import { Declarations } from '../../declarations.module';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { sortBy } from '../../my.functions';
import { CalendarScheduleModel, DeliveryRequestViewModel, DelRequestsByJobModel, ScheduleBuilderModel } from '../../carrier/models/DispatchSchedulerModels';
import { DeliveryrequestService } from '../../carrier/service/deliveryrequest.service';
import { UtilService } from 'src/app/services/util.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Priority } from 'src/app/carrier/models/DispatchSchedulerModels';
import { CalendarFilterModel, IndexModel, ShiftModel } from '../model';
import * as moment from 'moment';
import { LoadPriorities } from 'src/app/app.constants';

@Component({
    selector: 'app-dsb-calendar',
    templateUrl: './dsb-calendar.component.html',
    styleUrls: ['./dsb-calendar.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DsbCalendarComponent implements OnInit {
    //filter
    public allLocationList = [];
    public allVesselList = [];
    public locationList = [];
    public customerList = [];
    public vesselList = [];
    public scheduleData: ShiftModel[] = [];
    public shifts = [];
    public columns = [];
    public loads = [];
    public priorityList: Priority[] = [];
    selectedPriorityIds: string = '';

    SelectedlocationList = [];
    SelectedCustomerList = [];
    SelectedVesselList = [];
    SelectedPriorityList = [];

    public multiselectSettingsById: IDropdownSettings;
    public CustomerDdlSettings: IDropdownSettings;
    public SingleDdlSettings: IDropdownSettings;

    public count: number = 0;
    FromDate: string = '';
    ToDate: string = '';

    public locationType: boolean = false;
    public IsFilterLoaded: boolean = false;

    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();

    public isFromDate: boolean = true;
    public isValidDate: boolean = true;

    //schedule quantity type
    public ScheduleQuantityTypes: any = [];

    //@Filter

    CalendarView = CalendarView;
    public DrForm: FormGroup;
    public MoveToDsbForm: FormGroup;
    Events: any = [];
    DayEvents: any = [];
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public blendRequestsToUpdate: DeliveryRequestViewModel[] = [];
    public blendTotalQuantity: number = 0;
    public blendAddRequestToUpdate: DeliveryRequestViewModel[] = [];
    public blendedProducts = "";
    public _loadingDrRequests = false;
    public _loadingScheduleData = false;
    public SelectedDay; // for viewMoreData
    public SelectedDayEvent; // for dayView data
    colors: any = {
        1: {
            primary: '#FDD6D6',
            secondary: '#BB4141',
            tertiary: '#fadadd'
        },
        2: {
            primary: '#FFDDB5',
            secondary: '#E89330',
        },
        3: {
            primary: '#DCDCDC',
            secondary: '#696969',
        },
    };
    period: CalendarViewPeriod;
    beforeViewRender(event) {
        if (!this.period || this.period.start.getTime() !== event.period.start.getTime() || this.period.end.getTime() !== event.period.end.getTime()) {
            this.period = event.period;
            this.setlocalFilterVals();
            this.ApplyFilters();
        }
    }
    public setlocalFilterVals() {
        this.SelectedCustomerList = JSON.parse(localStorage.getItem("calenderCustomers")) as [] || [];
        this.SelectedlocationList = JSON.parse(localStorage.getItem("calenderLocations")) as [] || [];
        this.SelectedVesselList = JSON.parse(localStorage.getItem("calenderVessels")) as [] || [];
        this.SelectedPriorityList = JSON.parse(localStorage.getItem("calenderPriority")) as [] || [];
        this.FromDate = localStorage.getItem("calenderFromDate");
        this.ToDate = localStorage.getItem("calenderToDate");
    }
    refresh: Subject<any> = new Subject();
    allDeliveryLocations: DelRequestsByJobModel[] = [];
    onShiftSelect(shift) {
        const shiftIndexes = this.scheduleData.find(x => x.Id == shift.Id);
        if (shiftIndexes) {
            let cArray = shiftIndexes.Indexes.map(x => ({ ColumnIndex: x.ColumnIndex, Driver: x.Driver }));
            this.columns = [...new Map(cArray.map(item =>
                [item['ColumnIndex'], item])).values()]
                .map(y => {
                    var cName = "C" + (y.ColumnIndex + 1);
                    if (y.Driver && y.Driver != "") {
                        cName += " : " + y.Driver;
                    }
                    return {
                        Id: y.ColumnIndex, Name: cName
                    };
                });
            this.loads = [];
            this.MoveToDsbForm.get('Load').patchValue(null);
            this.MoveToDsbForm.get('Column').patchValue(null);
        }
    }
    onShiftDeSelect() {
        this.columns = [];
        this.loads = [];
        this.MoveToDsbForm.get('Column').patchValue(null);
        this.MoveToDsbForm.get('Load').patchValue(null);
    }
    onColumnSelect(col) {
        //filter loads
        let shifts = this.MoveToDsbForm.get("Shift").value;
        if (shifts) {
            const shiftIndexes = this.scheduleData.find(x => x.Id == shifts[0].Id);
            if (shiftIndexes && col) {
                this.MoveToDsbForm.get('Load').patchValue(null);
                this.loads = shiftIndexes.Indexes.filter(x => x.ColumnIndex == col.Id)
                    .map(x => ({ LoadIndex: x.LoadIndex, LoadTime: x.LoadTime }))
                    .reduce(function (a, b) { return a.concat(b); }, []).map(y => { return { Id: y.LoadIndex, Name: "Load " + (y.LoadIndex + 1) + " : " + y.LoadTime } });
            }
        }
    }
    onColumnDeSelect() {
        this.MoveToDsbForm.get('Load').patchValue(null);
    }
    public GetEventData() {
        this.Events = [];
        if (this.view == CalendarView.Day) {
            this.setDayViewEvents();
        } else {
            this.setMonthViewEvents();
        }
        this.refresh.next();
        this._loadingDrRequests = false;
        this.cdRef.detectChanges();
    }
    public resetLoadFilter() {
        this.shifts = [];
        this.loads = [];
        this.columns = [];
        this.MoveToDsbForm.get('Shift').patchValue(null);
        this.MoveToDsbForm.get('Column').patchValue(null);
        this.MoveToDsbForm.get('Load').patchValue(null);
    }
    public getCalendarLoadData(regionId: string, drDate: string) {
        this.resetLoadFilter();
        this._loadingScheduleData = true;
        this.carrierService.getSheduleCalendarData(regionId, drDate).subscribe(data => {
            this.scheduleData = data;
            this.shifts = this.scheduleData.map(x => { return { Id: x.Id, Name: x.Name }; });
            this._loadingScheduleData = false;
            this.cdRef.detectChanges();
        });
    }
    public setMonthViewEvents() {
        this.allDeliveryLocations.forEach(z => {
            // group by date
            var seldate = [];
            z.DeliveryRequests.forEach(z1 => {
                if (!seldate.includes(z1.SelectedDate)) {
                    seldate.push(z1.SelectedDate);
                    let eventDate = new Date(z1.SelectedDate);
                    let edata = {
                        title: z.CustomerCompany + " - " + z.JobName, Customer: z.CustomerCompany, JobName: z.JobName, JobId: z.JobId, start: eventDate, end: eventDate, allDay: true, color: this.colors[z1.Priority]
                    };
                    this.Events.push(edata);
                }
            });
        });
    }

    public setDayViewEvents() {
        this.DayEvents = [];
        let allDrs = this.allDeliveryLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        if (allDrs && allDrs.length > 0) {
            let selectedDate = moment(this.viewDate).format('MM/DD/YYYY');
            var filterDrs = allDrs.filter(t => t.SelectedDate == selectedDate);
            if (filterDrs && filterDrs.length) {
                var groupByTimeDrs = this.deliveryReqService.groupDrsBySelectedTime(filterDrs);
                groupByTimeDrs.forEach(x => {
                    const eventStartDate = addHours(new Date(selectedDate), x.StartTime);
                    const eventEndDate = addHours(new Date(selectedDate), x.EndTime);
                    let jobDrs = this.deliveryReqService.groupDrsByJob(x.DeliveryRequests);
                    let timeLimit = x.EndTime - x.StartTime;
                    let edata = {
                        start: eventStartDate, end: eventEndDate, drs: jobDrs, timeLimit: timeLimit
                    };
                    this.DayEvents.push(edata);
                });
            }
        }
    }
    constructor(private carrierService: CarrierService, private dispatcherService: DispatcherService, private fb: FormBuilder, private deliveryReqService: DeliveryrequestService,
        private utilService: UtilService, private cdRef: ChangeDetectorRef) { }

    ngOnInit(): void {
        this.DrForm = this.fb.group({
            DeliveryRequests: this.getDeliveryRequestFormArray([])
        });
        this.MoveToDsbForm = this.fb.group({
            Shift: this.fb.control(null),
            Column: this.fb.control(null),
            Load: this.fb.control(null),
            ScheduleData: this.fb.control(null),
            IsScheduleForToday: this.fb.control(false)
        });
        this.getScheduleQuantityType();
        //filter
        this.getAllCustomers();
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.CustomerDdlSettings = {
            singleSelection: false,
            idField: 'BuyerCompanyId',
            textField: 'BuyerName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.SingleDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false,
            closeDropDownOnSelection: true
        };

        this.priorityList = LoadPriorities;
        this.MinDate = new Date(this.MinDate.getFullYear(), this.MinDate.getMonth(), this.MinDate.getDate(), 0, 0, 0);
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
    }
    view: CalendarView = CalendarView.Month;
    viewDate: Date = new Date();

    getFutureDrs() {
        this._loadingDrRequests = true;
        this.cdRef.detectChanges();
        this.allDeliveryLocations = [];
        var inputModel = this.getFilterData();
        this.carrierService.getCalendarDeliveryRequests(inputModel).subscribe((drs: DeliveryRequestViewModel[]) => {
            this.allDeliveryLocations = [];
            if (drs != null && drs.length > 0) {
                this.allDeliveryLocations = this.deliveryReqService.groupDrsByJob(drs);
            }
            this.GetEventData();
        })
    }
    public getFilterData(): CalendarFilterModel {
        var inputModel: CalendarFilterModel = new CalendarFilterModel();
        inputModel.Customers = this.SelectedCustomerList.map(t => t.BuyerName);
        inputModel.LocationType = this.locationType;
        inputModel.Locations = this.SelectedlocationList.map(t => t.Id);
        inputModel.Priorities = this.SelectedPriorityList.map(t => t.Id);
        inputModel.Vessels = this.SelectedVesselList.map(t => t.Name);
        inputModel.FromDate = this.FromDate && this.FromDate != "null" ? new Date(this.FromDate) : this.period.start;
        inputModel.ToDate = this.ToDate && this.ToDate != "null" ? new Date(this.ToDate) : this.period.end;
        if (this.view == CalendarView.Day) {
            inputModel.FromDate = this.period.start;
            inputModel.ToDate = this.period.end;
        }
        return inputModel;
    }

    bindDeliveryRequests(jobId: number, date) {
        let location = this.allDeliveryLocations.find(t => t.JobId == jobId);
        if (location) {
            //var drs = location.DeliveryRequests;
            //if (location.DeliveryRequests.some(x => x.IsBlendedRequest === true)){
            //    drs = groupDrsByBlendGroupId(location.DeliveryRequests);
            //}
            var drs = location.DeliveryRequests.filter(t => new Date(t.SelectedDate).toString() == date.toString());
            this.DrForm = this.fb.group({
                DeliveryRequests: this.getDeliveryRequestFormArray(drs)
            });
        }
    }
    bindDayDeliveryRequests(jobId: number, startdate: Date, enddate: Date) {
        let location = this.allDeliveryLocations.find(t => t.JobId == jobId);
        if (location) {
            let startHour = moment(startdate).format("h");
            let startEve = moment(startdate).format("A");
            let endHour = moment(enddate).format("h");
            let endEve = moment(enddate).format("A");
            var drs = location.DeliveryRequests.filter(t =>
                (t.ScheduleStartTime.toString().startsWith(startHour) && t.ScheduleStartTime.toString().indexOf(startEve) > -1)
                && (t.ScheduleEndTime.toString().startsWith(endHour) && t.ScheduleEndTime.toString().indexOf(endEve) > -1)
            );
            this.DrForm = this.fb.group({
                DeliveryRequests: this.getDeliveryRequestFormArray(drs)
            });
        }
    }

    getDeliveryRequestFormArray(delReqs: DeliveryRequestViewModel[]): FormArray {
        delReqs = sortBy(delReqs, 'ProductType');
        let _drArray = this.fb.array([]);
        delReqs && delReqs.forEach(x => {
            var _form = this.utilService.getDeliveryRequestFormNew(x);
            _drArray.push(_form);
        });
        return _drArray;
    }

    setNextMonthEvents(date, event) {
        //this.getFutureDrs();
    }

    setView(view: CalendarView) {
        this.view = view;
    };

    getTotalBlendQuantity(): number {
        return this.blendRequestsToUpdate.map(t => t.RequiredQuantity).reduce((a, b) => a + b, 0);
    }
    toggleScheduleData() {
        if (this.MoveToDsbForm.get("IsScheduleForToday").value) {
            let todaysDate = moment(new Date()).format('MM/DD/YYYY');
            this.getCalendarLoadData(this.requestToUpdate.CreatedByRegionId, todaysDate);
        } else {
            this.getCalendarLoadData(this.requestToUpdate.CreatedByRegionId, this.requestToUpdate.SelectedDate);
        }
    }
    getScheduleDate() {
        if (this.MoveToDsbForm.get("IsScheduleForToday").value) {
            let todaysDate = moment(new Date()).format('MM/DD/YYYY');
            return todaysDate;
        } else {
            return this.requestToUpdate.SelectedDate;
        }
    }
    MoveToDSB(deliveryReq) {
        this.MoveToDsbForm.get('IsScheduleForToday').patchValue(false);
        this.getCalendarLoadData(deliveryReq.value.CreatedByRegionId, deliveryReq.value.SelectedDate);
        this.LoadDeliveryRequestToUpdate(deliveryReq);
        // temp data need to remove
        if (this.requestToUpdate.ScheduleQuantityType == 0) { this.requestToUpdate.ScheduleQuantityType = 1; }

        let element = document.getElementById("switchDSBModal");
        element ? element.click() : null;
    }
    onConfirmMoveToDSB() {
        const selectedShift = this.MoveToDsbForm.get("Shift").value[0].Id;
        const selectedLoad = this.MoveToDsbForm.get("Load").value[0].Id;
        const selectedColumn = this.MoveToDsbForm.get("Column").value[0].Id;
        this._loadingScheduleData = true;
        let drs: DeliveryRequestViewModel[] = [];
        if (this.requestToUpdate.IsBlendedRequest) {
            drs.push(...this.blendRequestsToUpdate);
            if (this.blendAddRequestToUpdate && this.blendAddRequestToUpdate.length > 0) {
                drs.push(...this.blendAddRequestToUpdate);
            }
        } else {
            drs.push(this.requestToUpdate);
        }
        drs.forEach(t => t.IsCalendarView = false);
        let schedule: CalendarScheduleModel = new CalendarScheduleModel();
        schedule.DeliveryRequests = drs;
        schedule.Date = this.getScheduleDate();
        schedule.RegionId = this.requestToUpdate.CreatedByRegionId;
        schedule.ShiftId= selectedShift;
        schedule.DriverRowIndex = selectedColumn;
        schedule.DriverColIndex = selectedLoad;
        this.carrierService.saveDriverView(schedule).subscribe((data) => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess('Delivery request saved successfully.', undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
            this._loadingScheduleData = false;
            let element = document.getElementById("modal-cancel");
            element ? element.click() : null;
            this.refreshCalendar();
        });
    }
    
    LoadDeliveryRequestToUpdate(deliveryReq: any) {
        let allDrs = this.allDeliveryLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        var drToupdate = allDrs.find(t => t.Id == deliveryReq.value.Id);
        this.blendRequestsToUpdate = [];
        this.blendTotalQuantity = 0;
        this.blendAddRequestToUpdate = [];
        this.requestToUpdate = Object.assign({}, drToupdate);
        if (this.requestToUpdate.IsBlendedRequest) {
            var tempBlendDrs = allDrs.filter(t => t.BlendedGroupId == deliveryReq.value.BlendedGroupId);
            this.blendedProducts = tempBlendDrs.map(t => t.ProductType).join(", ");
            this.blendRequestsToUpdate = tempBlendDrs.filter(t => !t.IsAdditive);
            this.blendAddRequestToUpdate = tempBlendDrs.filter(t => t.IsAdditive);
            this.blendTotalQuantity = this.getTotalBlendQuantity();
        }
    }
    EditDeliveryRequest(deliveryReq: any) {
        this.LoadDeliveryRequestToUpdate(deliveryReq);
        if (this.requestToUpdate.ScheduleQuantityType == 0) { this.requestToUpdate.ScheduleQuantityType = 1; }
        let element = document.getElementById("openUpdateDrModal");
        element ? element.click() : null;
    }

    DeleteDeliveryRequest(deliveryReq: any) {
        this.LoadDeliveryRequestToUpdate(deliveryReq);
        this.requestToUpdate.IsDeleted = true;
        if (this.requestToUpdate.IsBlendedRequest) {
            $.each(this.blendRequestsToUpdate, function () { this.IsDeleted = true; });
            if (this.blendAddRequestToUpdate)
                $.each(this.blendAddRequestToUpdate, function () { this.IsDeleted = true; });
        }
        let element = document.getElementById("openDeleteDeliveryRequestModal");
        element ? element.click() : null;
    }

    IsValidBlendQuantity(): boolean {
        return this.blendRequestsToUpdate.map(t => t.QuantityInPercent).reduce((a, b) => a + b, 0) == 100;
    }
    onDeliveryReqUpdate(status: number) {
        //VALIDATION
        if (status == 1) {
            var tnkRequiredQuantity: number = this.requestToUpdate.RequiredQuantity;
            if (this.requestToUpdate.IsBlendedRequest) {
                tnkRequiredQuantity = this.getTotalBlendQuantity();
                if (this.blendAddRequestToUpdate)
                    tnkRequiredQuantity = (+tnkRequiredQuantity) + (+this.blendAddRequestToUpdate.map(t => t.RequiredQuantity).reduce((a, b) => a + b, 0));
            }
            if (this.requestToUpdate.ScheduleQuantityType == 1 && (!(tnkRequiredQuantity > 0) || tnkRequiredQuantity < 0.00001)) {
                Declarations.msgerror("Invalid required quantity.", undefined, undefined); return;
            }
            else if (this.requestToUpdate.ScheduleQuantityType == 1 && this.requestToUpdate.TankMaxFill && this.requestToUpdate.TankMaxFill > 0 && !this.requestToUpdate.IsMaxFillAllowed) {
                if (tnkRequiredQuantity > this.requestToUpdate.TankMaxFill) {
                    Declarations.msgerror("Should not exceed max fill. (" + this.requestToUpdate.TankMaxFill + ")", undefined, undefined); return;
                }
            }
        }
        this._loadingDrRequests = true;
        jQuery('#closeEditDrPanel').click();
        if (this.requestToUpdate.ScheduleQuantityType > 1) { this.requestToUpdate.RequiredQuantity = 0; }
        let updateRequests = [this.requestToUpdate];
        if (this.requestToUpdate.IsBlendedRequest) {
            if (status == 1) {
                let drNotes = this.requestToUpdate.Notes;
                let drPriority = this.requestToUpdate.Priority;
                let drSelectedDate = this.requestToUpdate.SelectedDate;
                let drScheduleStartTime = this.requestToUpdate.ScheduleStartTime;
                let drScheduleEndTime = this.requestToUpdate.ScheduleEndTime;
                let deliveryLevelPO = this.requestToUpdate.DeliveryLevelPO;
                $.each(this.blendRequestsToUpdate, function () {
                    this.Notes = drNotes; this.Priority = drPriority;
                    this.SelectedDate = drSelectedDate;
                    this.ScheduleStartTime = drScheduleStartTime;
                    this.ScheduleEndTime = drScheduleEndTime;
                    this.DeliveryLevelPO = deliveryLevelPO;
                });
            }
            updateRequests = this.blendRequestsToUpdate;
            if (this.blendAddRequestToUpdate) {
                this.blendAddRequestToUpdate.forEach(t => {
                    t.Notes = this.requestToUpdate.Notes; t.Priority = this.requestToUpdate.Priority;
                    t.SelectedDate = this.requestToUpdate.SelectedDate;
                    t.ScheduleStartTime = this.requestToUpdate.ScheduleStartTime;
                    t.ScheduleEndTime = this.requestToUpdate.ScheduleEndTime;
                    t.DeliveryLevelPO = this.requestToUpdate.DeliveryLevelPO;
                    if (t.RequiredQuantity > 0 || t.ScheduleQuantityType != 1)
                        updateRequests.push(t);
                })
            }
        }
        this.carrierService.updateDeliveryRequest(updateRequests)
            .subscribe((data: any) => {
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    /*this.refreshDeliveryRequests(status);*/

                }
                else if (data.StatusCode == 2) {
                    Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                    /*this.refreshDeliveryRequests(status);*/
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
                this.refreshCalendar();
            });
    }

    refreshCalendar() {
        this.getFutureDrs();
    }

    toggleBlendQuantity(req: DeliveryRequestViewModel, isPercent: boolean) {
        if (isPercent) {
            req.RequiredQuantity = (this.blendTotalQuantity * req.QuantityInPercent) / 100;
        } else {
            req.QuantityInPercent = (req.RequiredQuantity / this.blendTotalQuantity) * 100;
        }
    }
    toggleBlendTotalQuantity() {
        this.blendRequestsToUpdate.forEach(t => {
            this.toggleBlendQuantity(t, true);
        });
    }
    getAllCustomers() {
        this.carrierService.getFilterDataForDsbCalenderView().subscribe((data) => {
            if (data != null) {
                this.customerList = data.CustomerList;
                this.vesselList = data.Vessels;
                this.allLocationList = data.Locations;
                this.allVesselList = data.Vessels;
                this.locationList = this.allLocationList.filter(x => this.locationType == x.IsTrue);
            }
        });
    }

    public toggleLocationType(isPort) {
        this.locationList = this.allLocationList.filter(x => isPort == x.IsTrue);
    }

    public ResetFilters() {
        this.SelectedCustomerList = [];
        this.SelectedlocationList = [];
        this.SelectedPriorityList = [];
        this.SelectedVesselList = [];
        this.locationType = false;
        this.FromDate = '';
        this.ToDate = '';
        this.isValidDate = true;
        this.ApplyFilters("reset");
        this.toggleLocationType(false);
    }
    public ApplyFilters(msg?) {
        this.count = 0;
        var Customerids = [];
        this.SelectedCustomerList.forEach(res => {
            this.count++;
            Customerids.push(res.CompanyId)
        });
        var Locationids = [];
        this.SelectedlocationList.forEach(res => {
            this.count++;
            Locationids.push(res.Id)
        });
        var vesselsids = [];
        this.SelectedVesselList.forEach(res => {
            this.count++;
            vesselsids.push(res.Id)
        });
        var Prioritiesids = [];
        this.SelectedPriorityList.forEach(res => {
            this.count++;
            Prioritiesids.push(res.Id)
        });
        if ((this.FromDate && this.FromDate != 'null') || (this.ToDate && this.ToDate != 'null')) {
            this.count++;
        }
        if (msg == "set") {
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        } else if (msg == "reset") {
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }
        this.setFilterValues();
        this.getFutureDrs();
    }

    public onCustomerChanged() {
        if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
            this.locationList = this.allLocationList.filter(t => this.SelectedCustomerList.some(c => c.BuyerCompanyId == t.CodeId) && t.IsTrue == this.locationType);
            if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
                this.SelectedlocationList = this.SelectedlocationList.filter(t => this.locationList.some(el => el.Id == t.Id));
                this.onLocationChange();
            }
        }
        else {
            this.locationList = this.allLocationList.filter(x => this.locationType == x.IsTrue);
        }
    }
    public onLocationChange() {
        if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
            this.vesselList = this.allVesselList.filter(t => this.SelectedlocationList.some(c => c.Id == t.CodeId));
            if (this.SelectedVesselList && this.SelectedVesselList.length > 0) {
                this.SelectedVesselList = this.vesselList.filter(x => this.SelectedVesselList.some(t => t.Id == x.Id));
            }
        }
        else {
            this.vesselList = this.allVesselList;
        }
    }
    public setFilterValues() {
        localStorage.setItem("calenderCustomers", JSON.stringify(this.SelectedCustomerList));
        localStorage.setItem("calenderLocations", JSON.stringify(this.SelectedlocationList));
        localStorage.setItem("calenderVessels", JSON.stringify(this.SelectedVesselList));
        localStorage.setItem("calenderPriority", JSON.stringify(this.SelectedPriorityList));
        localStorage.setItem("calenderFromDate", this.FromDate);
        localStorage.setItem("calenderToDate", this.ToDate);
    }
    public validateDate(date: any, fromDate: boolean) {
        if (date != '' && this.ToDate != '' && fromDate) {
            this.isValidDate = this.ToDate >= date;
            this.validateDateMessage(this.isValidDate);
            
        }
        else if (date != '' && this.FromDate != '' && !fromDate) {
            this.isValidDate = date >= this.FromDate
            this.validateDateMessage(this.isValidDate);
        }
    }
    public validateDateMessage(validDate: boolean) {
        if (!validDate) {
            Declarations.msgerror("Select valid filter dates", undefined, undefined);
        }
    }
    getScheduleQuantityType() {
        if (this.ScheduleQuantityTypes.length == 0) {
            this.dispatcherService.GetScheduleQtyType().subscribe((SQT: any[]) => {
                this.ScheduleQuantityTypes = SQT || [];
            });
        }
    }
}