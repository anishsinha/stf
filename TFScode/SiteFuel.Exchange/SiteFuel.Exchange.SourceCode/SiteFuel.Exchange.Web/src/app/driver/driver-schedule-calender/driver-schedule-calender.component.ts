import { Component, ViewChild, TemplateRef, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { startOfDay, endOfDay, subDays, addDays, endOfMonth, isSameDay, isSameMonth, addHours } from 'date-fns';
import { Subject } from 'rxjs';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarView } from 'angular-calendar';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { DropdownItem } from 'src/app/statelist.service';
import * as moment from 'moment';
import { DriverSchedule, DriverScheduleMapping, ConflictDates } from '../models/DriverSchedule';
import { Declarations } from '../../declarations.module';
import { RegionModel } from '../../company-addresses/region/model/region';
import { RouteInfoService } from 'src/app/carrier/service/route-info.service';
import { RouteInformationModel } from 'src/app/carrier/models/location';
import { RegionScheduleMappingViewModel } from '../models/regionSchedule';
import { DeleteDriverSchedule } from 'src/app/app.enum';

@Component({
    selector: 'app-driver-schedule-calender',
    templateUrl: './driver-schedule-calender.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    styleUrls: ['./driver-schedule-calender.component.css']
})
export class DriverScheduleCalenderComponent implements OnInit {

    public CalendarView = CalendarView;
    public isShowCalender = true;
    public IsLoading = false;
    public DriverList: DropdownItem[];
    public RegionList: DropdownItem[];
    public DriverRegionList: any = [];
    driverIds = '';
    public SelectedDriverList = [];
    public multiselectSettingsById: IDropdownSettings;
    public multiselectDateSettingsById: IDropdownSettings;
    public multiselectShiftById: IDropdownSettings;
    public singleselectSettingsById: IDropdownSettings;
    isShowEditPannel = false;
    isApplyAll = false; //edit schedule
    DriverShiftDetailList: any[] = [];
    RepeatList: any[] = [];
    DateList: any[] = [];
    ShiftList: any[] = [];
    regionList: any[] = [];
    TrailerList: any[] = [];
    RouteList: any[] = [];
    driverSchedule: DriverSchedule = {};
    driverScheduleMapping: DriverScheduleMapping = {};
    repeat: number = 1
    customDates: any = [];
    CurrentScheduleId: '';
    //min max date
    MinStartDate = new Date();
    MaxStartDate = new Date();
    startDateEnable: boolean;
    view: CalendarView = CalendarView.Month;
    viewDate: Date = new Date();
    //view 
    Selectedevent = [];
    SelectedDate: any;
    //end      
    IsUpdateForMultiple: any = false;
    scheduleType = 0;
    refresh: Subject<any> = new Subject();
    //evt: CalendarEvent[] = [];
    evt: any = [];
    activeDayIsOpen: boolean = false;
    RegionShiftDetailList: any[] = []

    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    //#region  Edit & Delete
    public _openedEditPanel: boolean = false;
    IsSheduleEdit = false;
    eFromDate: any;
    eToDate: any;
    public Selectedregion?: any = [];
    public SelectedDriver?: any = [];
    public IsShiftRepeted: boolean = false;
    selectedType: any = 0;
    public IsConfirmDelete: boolean = false;
    public deleteOption: any = 1;
    public eventDelete: any;
    public DriverId: string;
    public hideDeleteRange: boolean = false;
    public ConflictDateList = [];
    public conflictDates: any;
    public IsShowConflictTable: boolean = false;
    //#endregion

    constructor(public regionService: RegionService, private routeService: RouteInfoService) {

    }
    ngOnInit() {
        this.getDrivers();
        //  this.getShifts();
        this.getRegions();
        this.init();
        this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2);
        //this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 10);
    }

    ngAfterViewInit(): void {
        this.IsLoading = false;
    }

    init() {
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        }

        this.singleselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false,
            disabledField: "true"
        }

        this.multiselectShiftById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: false
        }
        this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
        }
    }

    public dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {

        if (isSameMonth(date, this.viewDate)) {
            if ((isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) || events.length === 0) {
                this.activeDayIsOpen = false;
            } else {

                this.Selectedevent = events;
                this.SelectedDate = moment(date).format('MM/DD/YYYY');
                let element: HTMLElement = document.getElementById('idViewDay') as HTMLElement;
                element.click();
                //this.activeDayIsOpen = true;

            }
            this.viewDate = date;
        }
    }

    async setView(view: CalendarView) {
        this.view = view;
        this.setNextMonthEvents(this.viewDate, this.view);
    }

    public closeOpenMonthViewDay() {
        this.activeDayIsOpen = false;
    }
    private getDrivers(): void {
        this.regionService.getDrivers()
            .subscribe((drivers: DropdownItem[]) => {
                this.DriverList = drivers;
            });
    }

    // public onRegionSelect($event) {

    //     var region = this.regionList.find(f => f.Id == $event.Id);
    //     this.TrailerList = region.Trailers.map(res => { return { Id: res.Code, Name: `${res.Name}` } });
    //     this.DriverList = region.Drivers.map(res => { return { Id: res.Id, Name: `${res.Name}` } });
    //     this.getRoutes(region.Id);

    //     this.getRegionScheduls(region) 

    // }

    private getRegionScheduls(region) {
        this.regionService.getSchedulesByRegion(region.Id, this.scheduleType)
            .subscribe((regions: RegionScheduleMappingViewModel[]) => {
                this.RegionShiftDetailList = regions;
            });
    }


    private getRoutes(regionId): void {
        this.routeService.getRoutesByRegion(regionId)
            .subscribe((routes: RouteInformationModel[]) => {
                this.getRouteDropDown(routes);
            });
    }

    private getRouteDropDown(routeList): void {
        this.RouteList = routeList.ResponseData
    }

    async onDriverSelect($event?) {
        this.regionService.onLoadingChanged.next(true);
        this.evt = [];
        let driverIds = [];
        this.SelectedDriverList.forEach(res => { driverIds.push(res.Id) });
        let drivers = driverIds.join();
        if (driverIds.length > 0) {
            this.regionService.getShiftByDrivers(drivers, this.scheduleType)
                .subscribe((data) => {
                    if (data.Result) {
                        this.DriverShiftDetailList = [];
                        this.DriverShiftDetailList = data.Result;
                        this.setNextMonthEvents(this.viewDate, 'Today');
                    }
                    //let element: HTMLElement = document.getElementById('idToday') as HTMLElement;
                    // element.click();
                    this.IsLoading = false;
                });
        }
        this.regionService.onLoadingChanged.next(false);
    }



    async onDriverDeSelect($event) {
        this.isShowCalender = false;
        this.evt = await this.evt.filter(f => f.driverId != $event.Id);
        //  this.DriverShiftDetailList = await this.DriverShiftDetailList.filter(f => f.DriverId != $event.Id);
        this.isShowCalender = true;
    }

    async setEvents(mnth?, year?) {
        this.regionService.onLoadingChanged.next(true);
        this.DriverShiftDetailList.forEach(async (driver) => {
            let color = await this.getRandomColor();
            let driverShortName = this.getShortDriverName(driver.DriverName);

            for await (let shift of driver.ScheduleList) {
                let startDate = this.changeTimeFormat(shift.ShiftDetail.StartTime);
                let endDate = this.changeTimeFormat(shift.ShiftDetail.EndTime);
                //previous and next logic
                if (year) {
                    startDate = new Date(new Date(new Date(new Date(startDate).setMonth(mnth))).setFullYear(year)).getTime();
                    endDate = new Date(new Date(new Date(new Date(endDate).setMonth(mnth))).setFullYear(year)).getTime();
                }
                //end

                if (this.view != CalendarView.Month && startDate > endDate) // if start time is greater than end time then add 1 day in end time
                {
                    endDate = addDays(new Date(endDate), 1).getTime();
                }

                // creating template for event schedule
                var event = {
                    mappingId: driver.Id,
                    repeatEvery: shift.RepeatEveryDay,
                    typeId: shift.TypeId,
                    id: shift.Id,
                    repeatDayList: shift.RepeatDayList,
                    shiftId: shift.ShiftId,
                    regionName: shift.ShiftDetail.RegionName,
                    driverId: driver.DriverId,
                    driverName: driver.DriverName,
                    driverShortName: driverShortName,
                    shiftFrom: shift.ShiftDetail.StartTime,
                    shiftTo: shift.ShiftDetail.EndTime,
                    fromDate: moment(shift.StrStartDate).format('MM/DD/YYYY'),
                    toDate: moment(shift.StrEndDate).format('MM/DD/YYYY'),
                    start: new Date(startDate),  // day wise start date time
                    end: new Date(endDate),       // day wise end date time
                    title: `<table class="table "> <tr><td> Driver - <strong>${driver.DriverName}</strong></td> <td><strong>${shift.ShiftDetail.StartTime} - ${shift.ShiftDetail.EndTime}</strong></td></tr></table> `,
                    color: color,
                    resizable: {
                        beforeStart: true,
                        afterEnd: true
                    },
                    draggable: false,
                    isUnplanedSchedule: driver.IsUnplanedSchedule,
                    description: shift.Description,
                }
                //end
                //this.evt.push(event)
                let currentDate = new Date().getDate();
                let eDate = new Date((moment(shift.StrEndDate).toLocaleString())).setHours(23, 59, 59, 0);
                let sDate = new Date((moment(shift.StrStartDate).toLocaleString())).setHours(0, 0, 0, 0);
                let date = new Date(new Date().setMonth(mnth)).setFullYear(year);
                let currentMonthLastDate = await this.daysInMonth(mnth + 1, year);

                for (let i = -currentDate; i < ((currentMonthLastDate + 1) - currentDate); i++) {
                    if (new Date(sDate).getTime() <= addDays(new Date(date), i).getTime() && new Date(eDate).getTime() >= addDays(new Date(date), i).getTime() && shift.RepeatDayList && shift.RepeatDayStringList.filter(dt => moment(dt).format('MM/DD/YYYY') == moment(addDays(new Date(event.start), i)).format('MM/DD/YYYY')).length > 0)
                        await this.addEventShift(event, i);
                }
            }

        })
        this.regionService.onLoadingChanged.next(false);
    }




    async addEventShift(event, i) {
        let evt = { ...event };
        evt.start = addDays(new Date(event.start), i);
        evt.end = addDays(new Date(event.end), i);
        this.evt.push(evt);
    }

    async daysInMonth(month, year) {
        return new Date(year, month, 0).getDate();
    }
    private changeTimeFormat(time) {
        //var time = '06:30 PM';
        var hours = Number(time.match(/^(\d+)/)[1]);
        var minutes = Number(time.match(/:(\d+)/)[1]);
        var AMPM = time.match(/\s(.*)$/)[1];
        if (AMPM == "PM" && hours < 12) hours = hours + 12;
        if (AMPM == "AM" && hours == 12) hours = hours - 12;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (hours < 10) sHours = "0" + sHours;
        if (minutes < 10) sMinutes = "0" + sMinutes;
        var date = (new Date(new Date().setHours(+sHours)).setMinutes(+sMinutes));
        return date;
    }

    async getRandomColor() {
        var letters = 'BCDEF'.split('');
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * letters.length)];
        }
        return { primary: color, secondary: color };
    }

    async setNextMonthEvents(date, event) {
        this.evt = [];
        await this.setEvents(new Date(date).getMonth(), new Date(date).getFullYear());
    }

    async OnScheduleAdded($event) {
        this.regionService.onLoadingChanged.next(false);
        this.IsLoading = false;
        if ($event) {
            this.scheduleType = 0;
            this.SelectedDriverList.forEach(res => {
                let cnt = $event.findIndex(x => x.Id == res.Id)
                if (cnt < 0)
                    $event.push(res)
            });
            this.SelectedDriverList = $event.slice();
            this.onDriverSelect();
        }
    }
    //edit

    async rmvSchedule(event) {
        this.eventDelete = event;
        this.AssignVeriables(event);
        this.IsConfirmDelete = true;
    }

    async updateCurrentSchedule() {
        if (this.driverSchedule.selectedShifts.length == 0) {
            Declarations.msgerror('Select shift', undefined, undefined);
            return false;
        }
        //rmv todays

        this.driverScheduleMapping = await this.DriverShiftDetailList.find(f => f.Id == this.driverScheduleMapping.Id);
        if (this.driverScheduleMapping != null && this.driverScheduleMapping.ScheduleList.length > 0) {
            this.driverScheduleMapping.ScheduleList.forEach(f => {
                if (f.IsActive = true && f.Id == this.driverSchedule.Id && f.ShiftId == this.driverSchedule.ShiftId) {
                    if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                        if (f.RepeatDayList.length == 1) {
                            delete f.RepeatDayList[0];
                            f.RepeatDayList = [];
                        } else {
                            let indexof = f.RepeatDayList.findIndex(x => moment(x).format('MM/DD/YYYY') == moment(this.sdate).format('MM/DD/YYYY'));
                            delete f.RepeatDayList[indexof];
                            let reOrder = []
                            f.RepeatDayList.forEach(r => { reOrder.push(r) });
                            f.RepeatDayList = reOrder;
                        }
                    }
                }
            });

            let getCurrentSelectedShift = this.driverSchedule.selectedShifts[0].Id;
            let checkShift = this.driverScheduleMapping.ScheduleList.filter(x => x.IsActive = true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null);
            if (checkShift != null && checkShift.length > 0) {
                let index = this.driverScheduleMapping.ScheduleList.findIndex(x => x.IsActive = true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null);
                this.driverScheduleMapping.ScheduleList[index].RepeatDayList = [];
                this.driverScheduleMapping.ScheduleList[index].RepeatDayList.push(moment(this.sdate).toDate());
                this.driverScheduleMapping.ScheduleList[index].StartDate = moment(this.sdate).toDate();
                this.driverScheduleMapping.ScheduleList[index].EndDate = moment(this.sdate).toDate();
            }
            else {
                this.driverSchedule.RepeatDayList = [];
                this.driverSchedule.RepeatDayList.push(moment(this.SelectedDate).toDate());
                this.driverScheduleMapping.ScheduleList.push({ Id: `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: moment(this.SelectedDate).toDate(), EndDate: moment(this.SelectedDate).toDate(), RepeatDayList: this.driverSchedule.RepeatDayList, ShiftId: getCurrentSelectedShift })
            }
        }
        for await (let item of this.driverScheduleMapping.ScheduleList) {
            if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                item.StartDate = moment(item.RepeatDayList[0]).toDate();
                item.EndDate = moment(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                var list = [];
                for await (let repeat of item.RepeatDayList) {
                    list.push(moment(repeat).toDate());
                }
                item.RepeatDayList = list;
            }

        }
        this.update(this.driverScheduleMapping);
    }

    sdate: any;
    edate: any;
    async editSchedule(event) {
        this.DriverRegionList = [];
        let currentDate = new Date((moment().toLocaleString())).setHours(0, 0, 0, 0);
        if (new Date(event.toDate).getTime() < new Date(currentDate).getTime()) {
            Declarations.msgerror('You cannot edit past records.', undefined, undefined);
        } else {
            this._editToggleOpened(true);
            this.TypeCheck(event);
            this.AssignVeriables(event);
        }
    }

    async AssignVeriables(event) {
        this.deleteOption = this.deleteOption ? this.deleteOption : 1;
        this.DriverRegionList = [];
        let currentDate = new Date((moment().toLocaleString())).setHours(0, 0, 0, 0);
        for await (let item of this.regionList) {
            if (item.Drivers.find(f => f.Id == event.driverId)) {
                var dRegion = { Id: 0, Name: "" };
                dRegion.Id = item.Id;
                dRegion.Name = item.Name;
                this.DriverRegionList.push(dRegion)
                this.Selectedregion = this.DriverRegionList;
                this.SelectedDriver = item.Drivers.filter(f => f.Id == event.driverId);
                this.ShiftList = item.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` } });
            }
        }
        this.driverSchedule.selectedShifts = this.ShiftList.filter(f => f.Id == event.shiftId);           // this.isShowEditPannel = true;
        this.eFromDate = moment(event.start).format('MM/DD/YYYY');
        this.eToDate = moment(event.toDate).format('MM/DD/YYYY');
        this.driverScheduleMapping.Id = event.mappingId;
        this.driverSchedule.Id = event.Id;
        this.driverSchedule.ShiftId = event.shiftId;
        this.driverScheduleMapping.DriverId = event.driverId;
        this.DriverId = event.driverId;
        this.driverScheduleMapping.DriverName = event.driverName;
        this.sdate = moment(event.start).format('MM/DD/YYYY');//this.SelectedDate;
        this.edate = moment(event.toDate).format('MM/DD/YYYY');
        this.IsSheduleEdit = true;
        //this.SelectedDate = moment(this.sdate).format('MM/DD/YYYY');
        this.IsUpdateForMultiple = false;
        if (this.eFromDate < this.eToDate) {
            this.IsUpdateForMultiple = true;
        }
        this.hideDeleteRange = false;
        if (new Date(this.sdate).getTime() == new Date(this.eToDate).getTime()) {

            this.hideDeleteRange = true;
        }
        if (new Date(this.sdate).getTime() < new Date(currentDate).getTime()) {

            this.startDateEnable = true;
        }
        this.driverSchedule.Id = event.id;
        this.driverSchedule.RepeatDayList = event.repeatDayList;
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;

        this.InitializeDates();
        if (this.selectedType == 4
            && event.repeatDayList != null
            && event.repeatDayList.length > 0) {

            this.customDates = [];
            let chCustomDates = [];
            let days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            event.repeatDayList.forEach(x => {
                let xdate = moment(x).toDate();
                let dt = `${moment(xdate).format('MM/DD/YYYY')} (${days[new Date(xdate).getDay()]})`;
                let sdt = this.DateList.find(x => x.Name == dt);
                if (sdt != null)
                    chCustomDates.push({ Id: sdt.Id, Name: sdt.Name });
            })
            this.customDates = chCustomDates;
        }
    }
    private TypeCheck(event: any) {
        this.selectedType = event.typeId ? event.typeId : "1";
        this.repeat = event.repeatEvery ? event.repeatEvery : "1";
    }
    async UpdateSchedule() {

        if (this.sdate && this.edate) {
            this.driverSchedule.StartDate = new Date(moment(this.sdate).toDate().getTime());
            this.driverSchedule.EndDate = new Date(moment(this.edate).toDate().getTime());
        } else {
            Declarations.msgerror('Select valid dates', undefined, undefined);
            return false;
        }
        if (moment(this.sdate) > moment(this.edate)) {
            Declarations.msgerror('From date cannot less than end date', undefined, undefined);
            return false;
        }
        if (this.driverSchedule.selectedShifts.length < 0) {
            Declarations.msgerror('Please select shift', undefined, undefined);
            return false;
        }
        this.InitializeDates();
        if (this.driverSchedule.selectedShifts.length == 0 || !this.driverSchedule.StartDate || !this.driverSchedule.EndDate || this.DateList.length == 0) { return false; }
        //update logic
        else {
            let selectedDates = []
            if (this.selectedType == 4)
                selectedDates = this.customDates;
            else
                selectedDates = this.DateList;

            let ShiftId = this.driverSchedule.selectedShifts[0].Id;
            let scheduleList = {};
            let newScheduleId = null;
            this.driverScheduleMapping = this.DriverShiftDetailList.find(f => f.Id == this.driverScheduleMapping.Id);

            let CurrentDriverShiftDetailList = this.DriverShiftDetailList.filter(f => f.DriverId == this.SelectedDriver[0].Id);
            let driverScheduleMappingIndex = CurrentDriverShiftDetailList.findIndex(f => f.Id == this.driverScheduleMapping.Id);

            if (this.driverScheduleMapping != null && this.driverScheduleMapping.ScheduleList != null && this.driverScheduleMapping.ScheduleList.length > 0) {

                let getCurrent = this.driverScheduleMapping.ScheduleList.find(x => x.Id == this.driverSchedule.Id && x.ShiftId == this.driverSchedule.ShiftId && x.IsActive);
                let getIndex = this.driverScheduleMapping.ScheduleList.findIndex(x => x.Id == this.driverSchedule.Id && x.ShiftId == this.driverSchedule.ShiftId && x.IsActive)
                //Update current one

                if (this.driverSchedule.ShiftId != ShiftId && getCurrent != null && getCurrent.RepeatDayList != null) {

                    // 

                    let rpDayList = [];
                    getCurrent.RepeatDayList.forEach(pre => {
                        let idx = selectedDates.findIndex(x => moment(x.Id).format('MM/DD/YYYY') == moment(pre).format('MM/DD/YYYY'));
                        if (idx < 0)
                            rpDayList.push(pre);
                    })
                    getCurrent.RepeatDayList = [];
                    getCurrent.RepeatDayList = rpDayList;
                }
                else {
                    if (getCurrent != null && getCurrent.RepeatDayList != null) {
                        let rpDayList = [];
                        var loop = new Date(this.driverSchedule.StartDate);
                        let cnt = 0
                        while (loop <= this.driverSchedule.EndDate) {
                            var newDate = loop.setDate(loop.getDate() + cnt);
                            if (!(newDate > this.driverSchedule.EndDate.setDate(this.driverSchedule.EndDate.getDate()))) {
                                getCurrent.RepeatDayList = getCurrent.RepeatDayList.filter(x => moment(x).format('MM/DD/YYYY') != moment(newDate).format('MM/DD/YYYY'));
                                if (cnt != 1) {
                                    cnt++
                                }
                            }
                        }
                        selectedDates.forEach(element => getCurrent.RepeatDayList.push("/Date(" + element.Id.setDate(element.Id.getDate()) + ")"));
                        let olst = [];
                        getCurrent.RepeatDayList.forEach(x => olst.push(moment(x).format('DD/MM/YYYY')));
                        olst.sort(); getCurrent.RepeatDayList = []; getCurrent.RepeatDayList = olst;
                        olst = []; getCurrent.RepeatDayList.forEach(x => {
                            var dt = x.split('/');
                            let oDt = dt[1] + '/' + dt[0] + '/' + dt[2];
                            let eDt = moment(oDt).toDate()
                            olst.push(eDt.setDate(eDt.getDate()));
                        });
                        getCurrent.RepeatDayList = olst;
                        olst = []; getCurrent.RepeatDayList.forEach(x => olst.push(moment(x).toDate()));
                        getCurrent.RepeatDayList = olst;
                    }
                    else {
                        getCurrent.RepeatDayList = [];
                        selectedDates.forEach(element => getCurrent.RepeatDayList.push(element.Id));
                    }
                }
                getCurrent.StartDate = getCurrent.RepeatDayList[0];
                getCurrent.RepeatDayStringList = [];
                getCurrent.RepeatDayList.forEach(x => {
                    getCurrent.RepeatDayStringList.push(moment(x).format('MM/DD/YYYY'))
                });
                getCurrent.EndDate = getCurrent.RepeatDayList[getCurrent.RepeatDayList.length - 1];
                delete this.driverScheduleMapping.ScheduleList[getIndex];
                this.driverScheduleMapping.ScheduleList.splice(getIndex, 0, getCurrent);
                //Update current done
                //start add new logic    

                if (this.driverSchedule.ShiftId != ShiftId) {
                    let oDateList = []
                    let oDateListString = []
                    if (this.selectedType == 4) {
                        this.customDates.forEach(x => {
                            if (!this.ConflictDateList.some((item) => moment(item).format('MM/DD/YYYY') == moment(x.Id).format('MM/DD/YYYY'))) {
                                oDateList.push(new Date(x.Id).getTime())
                            }
                        });
                    }
                    else {
                        this.DateList.forEach(x => {
                            if (!this.ConflictDateList.some((item) => moment(item).format('MM/DD/YYYY') == moment(x.Id).format('MM/DD/YYYY'))) {
                                oDateList.push(new Date(x.Id).getTime());
                            }
                        });
                    }
                    this.DateList.forEach(x => { oDateListString.push(moment(x.Id).format('MM/DD/YYYY')) });
                    newScheduleId = `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`;
                    scheduleList = { Id: `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: new Date(oDateList[0]).getTime(), EndDate: new Date(oDateList[oDateList.length - 1]).getTime(), RepeatDayList: oDateList, RepeatDayStringList: oDateListString, ShiftId: ShiftId, RepeatEveryDay: this.repeat, TypeId: this.selectedType };
                    this.driverScheduleMapping.ScheduleList.push(scheduleList)
                }
            }

            // reset index

            let oScheduleList = [];
            this.driverScheduleMapping.ScheduleList.forEach(e => { oScheduleList.push(e) });
            this.driverScheduleMapping.ScheduleList = [];
            this.driverScheduleMapping.ScheduleList = oScheduleList;

            //Check previous records exist or not for same shift with conflict dates 
            this.driverScheduleMapping.ScheduleList.forEach(x => {
                if (x.IsActive && x.ShiftId == ShiftId && newScheduleId != x.Id) {
                    let newList = [];
                    if (x.RepeatDayList != null && x.RepeatDayList.length > 0) {
                        x.RepeatDayList.forEach(y => {
                            if (!this.ConflictDateList.some((item) => moment(item).format('MM/DD/YYYY') == moment(y).format('MM/DD/YYYY'))) {
                                newList.push(moment(y).toDate())
                            }
                        });
                        x.RepeatDayList = newList;
                        if (x.RepeatDayList.length > 0) {
                            x.StartDate = x.RepeatDayList[0];
                            x.EndDate = x.RepeatDayList[x.RepeatDayList.length - 1];
                            x.RepeatDayStringList = [];
                            x.RepeatDayList.forEach(t => x.RepeatDayStringList.push(moment(t).format('MM/DD/YYYY')));
                        }
                        else {
                            x.IsActive = false;
                        }
                    }
                }
            });

            for await (let item of this.driverScheduleMapping.ScheduleList) {
                if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                    item.StartDate = moment(item.RepeatDayList[0]).toDate();
                    item.EndDate = moment(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                    var list = [];
                    var stringList = [];
                    item.RepeatDayStringList = [];
                    for await (let repeat of item.RepeatDayList) {
                        list.push(moment(repeat).toDate());
                        if (item.RepeatDayStringList == null || item.RepeatDayStringList.length == 0) {
                            stringList.push(moment(repeat).format('MM/DD/YYYY'))
                        }
                    }
                    item.RepeatDayList = list;
                    item.RepeatDayStringList = stringList;
                }
            }

            //reset Index of 
            delete CurrentDriverShiftDetailList[driverScheduleMappingIndex];
            CurrentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
            let oShifScheduleList = [];
            CurrentDriverShiftDetailList.forEach(e => { oShifScheduleList.push(e) });
            CurrentDriverShiftDetailList = [];
            CurrentDriverShiftDetailList = oShifScheduleList;
            //End Reset Index of 

            CurrentDriverShiftDetailList.forEach(ele => {
                if (ele.Id != this.driverScheduleMapping.Id) {
                    ele.ScheduleList.forEach(pop => {
                        if (pop.IsActive && pop.ShiftId == ShiftId && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {

                            let rpDayList = [];
                            pop.RepeatDayList.forEach(pre => {
                                let idx = selectedDates.findIndex(x => moment(x.Id).format('MM/DD/YYYY') == moment(pre).format('MM/DD/YYYY'));
                                if (idx < 0)
                                    rpDayList.push(pre);
                            })
                            pop.RepeatDayList = [];
                            pop.RepeatDayList = rpDayList;


                            pop.StartDate = moment(pop.RepeatDayList[0]).toDate();
                            pop.EndDate = moment(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                            pop.RepeatDayStringList = [];
                            pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment(ab).format('MM/DD/YYYY')));
                        }
                        if (pop.RepeatDayList == null || pop.RepeatDayList.length == 0)
                            pop.IsActive = false;
                        else {
                            pop.StartDate = moment(pop.RepeatDayList[0]).toDate();
                            pop.EndDate = moment(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                            pop.RepeatDayStringList = [];
                            pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment(ab).format('MM/DD/YYYY')));
                        }

                    });
                }
            });
            //end
            this.update(CurrentDriverShiftDetailList);
        }

    }
    async delete(model, sDate) {
        this.regionService.onLoadingChanged.next(true);
        this.regionService.deleteDriverSchedule(model, sDate)
            .subscribe((response: any) => {
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Driver Schedule deleted successfully', undefined, undefined);
                    setTimeout(function () {
                        let element: HTMLElement = document.getElementById('idCloseModel') as HTMLElement;
                        element.click();
                    }, 2000);
                    this.regionService.onLoadingChanged.next(false);
                    this.refresh.next();
                    this.onDriverSelect();
                    this.setView(CalendarView.Month);
                    this.deleteOption = 1;
                }
                else {
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    setTimeout(function () {
                        let element: HTMLElement = document.getElementById('idCloseModel') as HTMLElement;
                        element.click();
                    }, 1500);
                    this.regionService.onLoadingChanged.next(false);
                    this.refresh.next();
                    this.deleteOption = 1;
                    this.onDriverSelect();
                    this.setView(CalendarView.Month);
                }
                this.clearEditForm();
            });

        this.hideDeleteRange = false;
        this.IsConfirmDelete = false;
    }
    async update(model, Isdelete?: boolean) {
        this.regionService.onLoadingChanged.next(true);
        this.regionService.updateDriverSchedule(model, this.SelectedDate)
            .subscribe((response: any) => {
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Driver Schedule updated successfully', undefined, undefined);
                    if (!Isdelete) {
                        this.regionService.onLoadingChanged.next(false);
                        this.closeViewDayDetailModel();
                        this.onDriverSelect();
                        this.refresh.next();
                        this.deleteOption = 1;
                    }
                    else {
                        this.IsConfirmDelete = false;
                        setTimeout(function () {
                            let element: HTMLElement = document.getElementById('idCloseModel') as HTMLElement;
                            element.click();
                        }, 1500);
                        this.regionService.onLoadingChanged.next(false);
                        this.onDriverSelect();
                        this.refresh.next();
                        this.deleteOption = 1;
                    }
                }
                else {
                    this.closeViewDayDetailModel();
                    this.regionService.onLoadingChanged.next(false);
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    this.refresh.next();
                }
                this.clearEditForm();
            });
    }

    private getRegions(): void {
        this.regionService.onLoadingChanged.next(true);
        this.regionService.getRegions()
            .subscribe((region: RegionModel) => {
                this.regionList = region.Regions;
                this.regionService.onLoadingChanged.next(false);
            });
    }

    async InitializeDates(sdate?, end?) {
        this.driverSchedule.RepeatDayList = [];
        this.DateList = [];
        this.repeat = this.selectedType == 3 ? this.repeat : 0;
        var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        if (this.sdate && this.edate) {
            for (var dt = new Date(this.sdate); dt <= new Date(this.edate); dt.setDate(dt.getDate() + this.repeat + 1)) {
                if (this.selectedType && this.selectedType == 2) {
                    (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                }
                else {
                    this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
                }
            }
        }
        for (var dt = new Date(sdate); dt <= new Date(end); dt.setDate(dt.getDate() + 1)) {
            this.driverSchedule.RepeatDayList.push(new Date(dt));
        }
        //this.customDates = this.DateList;
        this.validateShiftForDriver(this.driverSchedule.selectedShifts[0]);
        return this.DateList;
    }

    setFromDate(event: any): void {
        this.sdate = (event);
        let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        !this.edate ? this.edate = (moment(d).format('MM/DD/YYYY')) : '';
        if (this.sdate != '' && this.edate != '') {
            let _fromDate = moment(this.sdate).toDate();
            let _toDate = moment(this.edate).toDate();
            if (_toDate < _fromDate) {
                this.edate = (event);
            }
        }
    }

    async setToDate(event: any) {

        this.IsUpdateForMultiple = false;
        this.edate = (event);
        if (this.sdate != '' && this.edate != '') {
            let _fromDate = moment(this.sdate).toDate();
            let _toDate = moment(this.edate).toDate();
            if (_fromDate > _toDate) {
                this.sdate = (event);
            }
            if (_toDate > _fromDate) {
                this.IsUpdateForMultiple = true;
            }
        }
        this.InitializeDates();
    }

    public closeViewDayDetailModel(): void {
        this.isShowEditPannel = false;
        this.startDateEnable = false;  // validate from date if it is less than current date.
        this.isApplyAll = false;
        this.driverScheduleMapping = {};
        this.driverSchedule = {};
        this._openedEditPanel = false;
        let element: HTMLElement = document.getElementById('idCloseModel') as HTMLElement;
        element.click();
    }

    private getShortDriverName(name): any {
        const fullName = name.split(' ');
        const lastName = fullName.pop();
        const firstName = fullName.join(' ');
        return firstName.substring(0, 1) + " " + lastName;

    }

    public _toggleOpened(shouldOpen: boolean): void {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }
    public _editToggleOpened(shouldOpen: boolean): void {

        if (shouldOpen) {
            this._openedEditPanel = true;
        }
        else {
            this._openedEditPanel = !this._openedEditPanel;
        }
        this.clearEditForm();
    }

    public clearEditForm() {
        this.Selectedregion = [];
        this.SelectedDriver = [];
        this.driverSchedule.selectedShifts = [];
        this.startDateEnable = false;
        this.eToDate = null;
        this.eFromDate = null;
        this.IsUpdateForMultiple = false;
        this.selectedType = 0;
        this.customDates = [];
        this.repeat = 0;
        this.IsShiftRepeted = false
        this.IsSheduleEdit = false;
        this.SelectedDate = new Date();
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;
    }

    public changeIsApplyAll() {
        this.isApplyAll = !this.isApplyAll;
        this.selectedType = 1;
        this.InitializeDates();
    }

    public validateShiftForDriver(event) {
        let DaysRepeateCount = 1;
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;
        this.IsShiftRepeted = false;
        if (this.sdate && this.edate) {

            let CheckConflictDays = this.DriverShiftDetailList.filter(f => f.DriverId == this.DriverId);
            if (CheckConflictDays != null && CheckConflictDays.length > 0) {
                let selecteddateList = [];
                if (this.selectedType == 4) {
                    selecteddateList = this.customDates;
                }
                else {
                    selecteddateList = this.DateList;
                }
                CheckConflictDays.forEach(ShiftDetails => {
                    if (ShiftDetails.ScheduleList != null) {
                        if (selecteddateList.length > 0) {
                            ShiftDetails.ScheduleList.forEach(elm => {
                                if (elm.ShiftId == event.Id && elm.ShiftId != this.driverSchedule.ShiftId) {
                                    if (elm.RepeatDayList != null && elm.RepeatDayList.length > 0) {
                                        elm.RepeatDayList.forEach(dte => {
                                            selecteddateList.forEach(slDate => {
                                                if (moment(slDate.Id).format('MM/DD/YYYY') == moment(dte).format('MM/DD/YYYY')) {
                                                    this.ConflictDateList.push(moment(slDate.Id).format('MM/DD/YYYY'));
                                                    DaysRepeateCount++;
                                                }
                                            });
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
        else {
            Declarations.msgwarning('Please select dates first', undefined, undefined);
            return true;
        }
        if (this.ConflictDateList.length > 0) {
            Declarations.msgwarning("Following shifts are already assigned to the drive", undefined, undefined);
        }


    }

    public isRequired(name) {
        if (name == "" || name == null) {
            return true;
        }
        else {
            return false;
        }
    }

    public closeDeleteModel() {
        this.IsConfirmDelete = false;
    }
    async RemoveSchedule(event) {
        let currentDelete: DriverSchedule;
        this.driverScheduleMapping = await this.DriverShiftDetailList.find(f => f.Id == event.mappingId);

        let currentDriverShiftDetailList = await this.DriverShiftDetailList.filter(f => f.DriverId == this.DriverId);
        let driverScheduleMappingIndex = await currentDriverShiftDetailList.findIndex(f => f.Id == event.mappingId);

        let driverShiftMapping = await this.DriverShiftDetailList.filter(f => f.DriverId == this.DriverId);
        if (this.deleteOption != "" && this.deleteOption != null) {
            if (this.deleteOption == DeleteDriverSchedule.Single) {
                this.driverScheduleMapping.ScheduleList.forEach(f => {
                    if (f.IsActive = true && f.Id == this.driverSchedule.Id && f.ShiftId == this.driverSchedule.ShiftId) {
                        if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                            currentDelete = f;
                            if (f.RepeatDayList.length == 1) {
                                delete f.RepeatDayList[0];
                                f.RepeatDayList = null;
                            } else {
                                let indexof = f.RepeatDayList.findIndex(x => moment(x).format('MM/DD/YYYY') == moment(this.sdate).format('MM/DD/YYYY'));
                                delete f.RepeatDayList[indexof];
                                let reOrder = []
                                f.RepeatDayList.forEach(r => { reOrder.push(r) });
                                f.RepeatDayList = reOrder;
                                f.StartDate = f.RepeatDayList[0];
                                f.EndDate = f.RepeatDayList[f.RepeatDayList.length - 1];
                                f.RepeatDayStringList = [];
                                f.RepeatDayList.forEach(x => {
                                    f.RepeatDayStringList.push(moment(x).format('MM/DD/YYYY'))
                                });
                            }
                        }
                    }
                });
                this.IsConfirmDelete = false;
            }
            if (this.deleteOption == DeleteDriverSchedule.Range) {
                if (this.driverScheduleMapping.ScheduleList.length > 0) {
                    let sList = this.driverScheduleMapping.ScheduleList.forEach(res => {
                        if (res.Id == this.driverSchedule.Id) {
                            let reOrder = [] = res.RepeatDayList.filter(x => moment(x).format('MM/DD/YYYY') < moment(this.sdate).format('MM/DD/YYYY'));
                            res.RepeatDayList = reOrder;
                            res.StartDate = res.RepeatDayList[0];
                            res.EndDate = res.RepeatDayList[res.RepeatDayList.length - 1];
                            res.RepeatDayStringList = [];
                            res.RepeatDayList.forEach(x => {
                                res.RepeatDayStringList.push(moment(x).format('MM/DD/YYYY'))
                            });
                        }
                    });
                }
                this.IsConfirmDelete = false;
            }

            if (this.deleteOption == DeleteDriverSchedule.Whole) {
                if (driverShiftMapping.length > 0) {
                    for await (let oSchedule of driverShiftMapping) {
                        if (oSchedule.ScheduleList.length > 0) {
                            oSchedule.ScheduleList.forEach(ele => {
                                if (ele.RepeatDayList != null) {
                                    let reOrder = [] = ele.RepeatDayList.filter(x => moment(x).format('MM/DD/YYYY') < moment(this.sdate).format('MM/DD/YYYY'));
                                    ele.RepeatDayList = reOrder;
                                    ele.StartDate = ele.RepeatDayList[0];
                                    ele.EndDate = ele.RepeatDayList[ele.RepeatDayList.length - 1];
                                    ele.RepeatDayStringList = [];
                                    ele.RepeatDayList.forEach(x => {
                                        ele.RepeatDayStringList.push(moment(x).format('MM/DD/YYYY'))
                                    });
                                }
                            });
                        }
                    }

                    await driverShiftMapping.forEach(element => {
                        for (let item of element.ScheduleList) {
                            if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                                item.StartDate = moment(item.RepeatDayList[0]).toDate();
                                item.EndDate = moment(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                                var list = [];
                                for (let repeat of item.RepeatDayList) {
                                    list.push(moment(repeat).toDate());
                                }
                                item.RepeatDayList = list;
                            }
                        }
                    });
                    this.delete(driverShiftMapping, this.SelectedDate);
                }
            }

            if (this.deleteOption == DeleteDriverSchedule.Range || this.deleteOption == DeleteDriverSchedule.Single) {

                for await (let item of this.driverScheduleMapping.ScheduleList) {
                    if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                        item.StartDate = moment(item.RepeatDayList[0]).toDate();
                        item.EndDate = moment(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                        var list = [];
                        for await (let repeat of item.RepeatDayList) {
                            list.push(moment(repeat).toDate());
                        }
                        item.RepeatDayList = list;
                    }
                }

                delete currentDriverShiftDetailList[driverScheduleMappingIndex];
                currentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
                let oShifScheduleList = [];
                currentDriverShiftDetailList.forEach(e => { oShifScheduleList.push(e) });
                this.DriverShiftDetailList = [];
                currentDriverShiftDetailList = oShifScheduleList;
                //End Reset Index of 

                currentDriverShiftDetailList.forEach(ele => {
                    if (ele.Id != this.driverScheduleMapping.Id) {
                        ele.ScheduleList.forEach(pop => {
                            if (pop.IsActive && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {
                                pop.StartDate = moment(pop.RepeatDayList[0]).toDate();
                                pop.EndDate = moment(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                                pop.RepeatDayStringList = [];
                                pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment(ab).format('MM/DD/YYYY')));
                            }
                            else
                                pop.IsActive = false;
                        });
                    }
                });
                this.update(currentDriverShiftDetailList, true);
                return true;
            }

        } else {
            Declarations.msgwarning('Please select delete option', undefined, undefined);
            return false;
        }
    }

    public showDriverConflictSchedules() {
        this.IsShowConflictTable = !this.IsShowConflictTable;
    }
}
