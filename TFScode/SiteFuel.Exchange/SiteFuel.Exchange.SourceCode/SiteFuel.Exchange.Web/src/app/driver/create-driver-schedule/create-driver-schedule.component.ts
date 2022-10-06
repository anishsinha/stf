import { Component, OnInit, Input, ViewChild, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { RegionService } from '../../company-addresses/region/service/region.service';
import { DropdownItem } from '../../statelist.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { Declarations } from 'src/app/declarations.module';
import { DriverScheduleMapping } from 'src/app/driver/models/DriverSchedule';
import * as moment from 'moment';
import { RegionModel } from '../../company-addresses/region/model/region';
@Component({
    selector: 'create-driver-schedule',
    templateUrl: './create-driver-schedule.component.html',
    styleUrls: ['./create-driver-schedule.component.css']
})
export class CreateDriverScheduleComponent implements OnInit {
    //@ViewChild('DriverScheduleForm') public form: NgForm;
    @Output() OnScheduleAdded = new EventEmitter();
    DriverList = [];
    public regionList = [];
    DriverScheduleForm: FormGroup;
    isLoading = false;
    //sidebar variables
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    SelectedShiftList = [];
    ShiftList = [];
    SelectedScheduleDriverList = [];
    RepeatList = [];
    DateList = [];
    //end
    public multiselectShiftSettingsById: IDropdownSettings;
    public multiselectRepeatSettingsById: IDropdownSettings;
    public multiselectDateSettingsById: IDropdownSettings;
    public multiselectDriverSettingsById: IDropdownSettings;

    //min max date
    MinStartDate = new Date();
    MaxStartDate = new Date();

    //end
    constructor(public regionService: RegionService, private _fb: FormBuilder) { }

    ngOnInit() {
        this.init();
    }

    ngAfterViewInit(): void {
        this.isLoading = false;
    }

    

   


    init() {
        this.getRegions();
        this.createScheduleForm();
        
        this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2);
        // this.getDrivers();
        //  this.getShifts();
       
        // this.getDateList();
        this.multiselectDriverSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        }
        this.multiselectShiftSettingsById = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        }

        this.multiselectRepeatSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
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



    private createScheduleForm(): void {
        this.DriverScheduleForm = this._fb.group({
            id: [''],
            shiftId: ['', Validators.required],
            regionId: ['', Validators.required],
            type: ['1', Validators.required],
            driverId: ['', Validators.required],
            fromDate: ['', Validators.required],
            toDate: ['', Validators.required],
            repeat: [1],
            customDates: [[]],

        });
        var dt = moment(new Date()).toDate();
        //alert(moment(dt).format('MM/DD/YYYY'));
        this.DriverScheduleForm.controls.fromDate.setValue(moment(dt).format('MM/DD/YYYY'));
    }


  
    public _toggleOpened(shouldOpen: boolean): void {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }


    private getRegions(): void {
        this.regionService.getRegions()
            .subscribe((region: RegionModel) => {
                this.getRegionDropDwn(region.Regions);
            });
    }

    public onRegionSelect($event) {
        var region = this.regionList.find(f => f.Id == $event.Id);
        this.DriverScheduleForm.controls.driverId.setValue('');
        this.DriverScheduleForm.controls.shiftId.setValue('');
        let tDate = new Date()
        this.DriverScheduleForm.controls.fromDate.setValue(moment(tDate).format('MM/DD/YYYY'));
        this.setFromDate(moment(tDate).format('MM/DD/YYYY'));
        this.DriverList = region.Drivers;
        this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` } });
    }

    public onRegionDeSelect($event) {
        this.DriverList = [];
        this.DriverScheduleForm.controls.driverId.setValue('');
        this.ShiftList = [];
        this.DriverScheduleForm.controls.shiftId.setValue('');
    }

    private getRegionDropDwn(regionList): void {
        this.regionList = regionList;

    }

   


    isInvalid(name: string): boolean {
        var result = this.DriverScheduleForm.get(name).invalid && (this.DriverScheduleForm.get(name).dirty || this.DriverScheduleForm.get(name).touched)
        return result;
    }

    isRequired(name: string): boolean {
        return this.DriverScheduleForm.get(name).errors.required;
    }
    setFromDate(event: any): void {
        this.DriverScheduleForm.controls.fromDate.setValue(event);

        //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        let d = moment(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';
        if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment(this.DriverScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.DriverScheduleForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.DriverScheduleForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
         this.validateShiftForDriver(false);
    }

    async  setToDate(event: any) {
        this.DriverScheduleForm.controls.toDate.setValue(event);
        if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment(this.DriverScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.DriverScheduleForm.controls.toDate.value).toDate();
            if (_fromDate > _toDate) {
                this.DriverScheduleForm.controls.fromDate.setValue(event);
            }
        }
        this.InitializeDates();
        await this.validateShiftForDriver(false);
    }

    async InitializeDates(type?: number, repeat?: number) {
        this.DriverScheduleForm.controls.customDates.setValue([]);
        this.DateList = [];
        !repeat ? repeat = 0 : '';
        var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        // alert(this.DriverScheduleForm.controls.fromDate.value);
        // alert(this.DriverScheduleForm.controls.toDate.value);
        if (this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value) {

            for (var dt = new Date(this.DriverScheduleForm.controls.fromDate.value); dt <= new Date(this.DriverScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                if (type && type == 2) //weekend
                    (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                else
                    this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
            }

            return this.DateList;
        }
    }

    async onSubmit() {
        //this.DriverScheduleForm.controls.repeat.value &&  this.DriverScheduleForm.controls.repeat.value[0].Id == '1' ? await this.DriverScheduleForm.controls.customDates.setValue(this.DateList) : '';
        if (this.DriverScheduleForm.invalid) {
            this.DriverScheduleForm.markAllAsTouched();
            return false;
        } else if (this.DriverScheduleForm.controls.type.value == '3') {
            if (!(this.DriverScheduleForm.controls.repeat.value && this.DriverScheduleForm.controls.repeat.value > 0)) {
                Declarations.msgerror('Repeat field is greater than 0', undefined, undefined);
                return false;
            }
        } else if (this.DriverScheduleForm.controls.type.value == '4') {
            if (!(this.DriverScheduleForm.controls.customDates.value.length > 0)) {
                Declarations.msgerror('Select custom dates', undefined, undefined);
                return false;
            }
        }
        if (await this.validateShiftForDriver(true) == 1) { return false;}
        //this.DriverScheduleForm.controls.customDates.setValue([]);
        if (this.DriverScheduleForm.controls.type.value == '2') { this.DriverScheduleForm.controls.customDates.setValue(await this.InitializeDates(this.DriverScheduleForm.controls.type.value)); }
        else if (this.DriverScheduleForm.controls.type.value == '3') { this.DriverScheduleForm.controls.customDates.setValue(await this.InitializeDates(this.DriverScheduleForm.controls.type.value, this.DriverScheduleForm.controls.repeat.value)); }
        else if (this.DriverScheduleForm.controls.type.value == '1') { this.DriverScheduleForm.controls.customDates.setValue(await this.InitializeDates()) }
        let dates = [];

        //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        //!this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';

        await this.DriverScheduleForm.controls.customDates.value.map(res => { dates.push(res.Id) });
        let model = new DriverScheduleMapping();
        this.DriverScheduleForm.controls.id.value ? model.Id = this.DriverScheduleForm.controls.id.value : '';
        model.DriverId = this.DriverScheduleForm.controls.driverId.value[0].Id;
       var repeatDayListString = [];
        dates.forEach(x => {
            repeatDayListString.push(moment(x).format('MM/DD/YYYY'));
        })
        model.ScheduleList.push({ Id: `${model.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: this.DriverScheduleForm.controls.fromDate.value, EndDate: this.DriverScheduleForm.controls.toDate.value, RepeatDayList: dates, RepeatDayStringList: repeatDayListString, ShiftId: this.DriverScheduleForm.controls.shiftId.value[0].Id, Type: this.DriverScheduleForm.controls.type.value, RepeatEveryDay :this.DriverScheduleForm.controls.repeat.value ,TypeId : this.DriverScheduleForm.controls.type.value });
       
        this.addDriverSchedule(model);
    }

    private addDriverSchedule(model): void {
        this.isLoading = true;
        this.regionService.onLoadingChanged.next(true);
        this.regionService.addDriverSchedule(model)
            .subscribe((response: any) => {

                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Driver Schedule created successfully', undefined, undefined);
                    this.isLoading = false;
                    this.regionService.onLoadingChanged.next(false);
                    this._toggleOpened(false);
                    let driver = this.DriverScheduleForm.controls.driverId.value;
                    this.DriverScheduleForm.reset();
                    this.OnScheduleAdded.emit(driver);
                    this.DriverScheduleForm.controls.type.setValue('1');

                }
                else {
                    this.isLoading = false;
                    this.regionService.onLoadingChanged.next(false);
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
    }

    public closedSideBar(): void { this._opened = false; }

   
    //public getDateList() {
    //    this.DateList=  this.regionService.getDateList();
    //}

    //public onRepeatSelect($event): void {
    //    if ($event.Id == '1')
    //        this.DriverScheduleForm.controls.customDates.setValue(this.DateList);
    //    else
    //        this.DriverScheduleForm.controls.customDates.setValue([]);
    //}

    //public onRepeatDeSelect($event): void {
    //       this.DriverScheduleForm.controls.customDates.setValue([]);

    //}

    DriverShiftDetailList: any[] = [];

    async onDriverSelect($event) {
        this.regionService.onLoadingChanged.next(true);

        let driverIds = [];
        this.DriverScheduleForm.controls.driverId.value.forEach(res => { driverIds.push(res.Id) });
        let drivers = driverIds.join();
        if (driverIds.length > 0) {
            this.regionService.getShiftByDrivers(drivers,0) // schedule type
                .subscribe((data) => {
                    if (data.Result)
                        this.DriverShiftDetailList = data.Result;
                });
        }

        this.regionService.onLoadingChanged.next(false);

    }

    async onDriverDeSelect($event) {
    }

    

    async validateShiftForDriver(isSubmit:boolean) {       
        var i = 0;           
        let rpDayList = [];
        if (this.DriverScheduleForm.controls.type.value == 4)
            rpDayList = this.DriverScheduleForm.controls.customDates.value;
        else
            rpDayList = this.DateList;
        if (this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value && this.DriverScheduleForm.controls.shiftId.value && this.DriverScheduleForm.controls.shiftId.value.length > 0) {
            for await (let item of this.DriverShiftDetailList) {
                for await (let shift of item.ScheduleList) {
                    if (shift.RepeatDayList !=null &&  (moment(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment(shift.StartDate).format('MM/DD/YYYY') &&
                        moment(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment(shift.EndDate).format('MM/DD/YYYY')) ||
                        (moment(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') >= moment(shift.StartDate).format('MM/DD/YYYY') &&
                            moment(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') <= moment(shift.EndDate).format('MM/DD/YYYY'))) {
                        if (this.DriverScheduleForm.controls.shiftId.value[0].Id == shift.ShiftId) {
                            shift.RepeatDayList.forEach(ele => {
                                let idx = rpDayList.findIndex(x => moment(x.Id).format('MM/DD/YYYY') == moment(ele).format('MM/DD/YYYY'));
                                if (idx >= 0) {
                                    if (i != 1) {
                                        Declarations.msgerror("Shift is already assigned to the driver", undefined, undefined);
                                        i = 1;
                                    }
                                }
                            });

                            if (i == 1)
                                break;
                           
                        } else {
                            !isSubmit ? Declarations.msgwarning("Another shift is already assigned to the driver", undefined, undefined):'';
                            //i = 1;
                            //break;
                        }
                    }
                }
                if (i == 1)
                    break;
            }
            return i;
        }
        return i;
    }
}