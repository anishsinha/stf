import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { RegionModel } from 'src/app/company-addresses/region/model/region';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { FormGroup, FormBuilder, Validators, FormControl, FormArray } from '@angular/forms';
import * as moment from 'moment';
import { Declarations } from 'src/app/declarations.module';
import { TrailerSchedule } from 'src/app/driver/models/TrailerSchedule';
import { DriverService } from '../services/driver.service';

@Component({
    selector: 'app-create-trailer-schedule',
    templateUrl: './create-trailer-schedule.component.html',
    styleUrls: ['./create-trailer-schedule.component.css']
})
export class CreateTrailerScheduleComponent implements OnInit {
    @Output() OnScheduleAdded = new EventEmitter();
    public regionList = [];
    ShiftList = [];
    TrailerList = [];
    DriverList = [];
    ColumnList = [];
    isLoading = false;
    TrailerScheduleForm: FormGroup;
    public SelectedRegionId: string = '';
    //sidebar variables
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public multiselectDropDownSettingsById: IDropdownSettings;
    public multiselectDateSettingsById: IDropdownSettings;
    DateList = [];

    //min max date
    MinStartDate = new Date();
    MaxStartDate = new Date();

    constructor(private regionService: RegionService,private driverService:DriverService, private _fb: FormBuilder) { }

    ngOnInit() {
        this.init();
    }

    init() {
        this.getRegions();
        this.createTrailerForm();
        this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2);

        this.multiselectDropDownSettingsById = {
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

    ngAfterViewInit(): void {
        this.isLoading = false;
    }

    public _toggleOpened(shouldOpen: boolean): void {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }

    isInvalid(name: string): boolean {
        var result = this.TrailerScheduleForm.get(name).invalid && (this.TrailerScheduleForm.get(name).dirty || this.TrailerScheduleForm.get(name).touched)
        return result;
    }

    isRequired(name: string): boolean {
        return this.TrailerScheduleForm.get(name).errors.required;
    }

    private getRegions(): void {
        this.regionService.getRegions()
            .subscribe((region: RegionModel) => {
                this.getRegionDropDwn(region.Regions);
            });
    }

    public closedSideBar(): void { this._opened = false; }

    public onRegionSelect($event) {
        var region = this.regionList.find(f => f.Id == $event.Id);
        //this.TrailerScheduleForm.controls.shiftId.setValue('');
        this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` } });
        this.TrailerList = region.Trailers.map(res => { return { Id: res.Code, Name: `${res.Name}` } });
        this.SelectedRegionId = region.Id;
        this.createColumnList(region);
    }

    private createColumnList(region) {
        this.DriverList = region.Drivers;
        if (region.Drivers.length > 0) {
            var num = 1;
            this.DriverList.forEach(obj => {
                var col = {
                    Id: 0,
                    Name: ""
                }
                col.Id = num;
                col.Name = "C" + num;
                this.ColumnList.push(col);
                num++;
            })
        }
    }

    private getRegionDropDwn(regionList): void {
        this.regionList = regionList;
    }

    public onRegionDeSelect($event) {
        //var region = this.regionList.find(f => f.Id == $event.Id);
        //this.ShiftList = [];
        //this.TrailerScheduleForm.controls.trailerId.setValue('');
    }

    public createTrailerForm(): void {
        this.TrailerScheduleForm = this._fb.group({
            id: this._fb.control(null),
            regionId: this._fb.control('', Validators.required),
            trailerId: this._fb.control('', Validators.required),
            shifts: this._fb.array([this.getShift()]),
            fromDate: this._fb.control('', Validators.required),
            toDate: this._fb.control('', Validators.required),
            //type: this._fb.control(''),
            type: ['1', Validators.required],
            repeat: [1],
            customDates: [[]],
        });
        var dt = moment(new Date()).toDate();
        this.TrailerScheduleForm.controls.fromDate.setValue(moment(dt).format('MM/DD/YYYY'));
    }

    addShift() {
        let _shifts = this.TrailerScheduleForm.get('shifts') as FormArray;
        _shifts.push(this.getShift());
    }

    getShift() {
        return this._fb.group({
            shiftId: this._fb.control('', Validators.required),
            columnId: this._fb.control('', Validators.required)
        })
    }

    removeShift(index: number) {
        let _shifts = this.TrailerScheduleForm.get('shifts') as FormArray;
        _shifts.removeAt(index);
    }

    setFromDate(event: any): void {
        this.TrailerScheduleForm.controls.fromDate.setValue(event);

        //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        let d = moment(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.TrailerScheduleForm.controls.toDate.value ? this.TrailerScheduleForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';
        if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment(this.TrailerScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.TrailerScheduleForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.TrailerScheduleForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
    }

    async  setToDate(event: any) {
        this.TrailerScheduleForm.controls.toDate.setValue(event);
        if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment(this.TrailerScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.TrailerScheduleForm.controls.toDate.value).toDate();
            if (_fromDate > _toDate) {
                this.TrailerScheduleForm.controls.fromDate.setValue(event);
            }
        }
        this.InitializeDates();
    }

    async InitializeDates(type?: number, repeat?: number) {
        this.TrailerScheduleForm.controls.customDates.setValue([]);
        this.DateList = [];
        !repeat ? repeat = 0 : '';
        var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        if (this.TrailerScheduleForm.controls.fromDate.value && this.TrailerScheduleForm.controls.toDate.value) {

            for (var dt = new Date(this.TrailerScheduleForm.controls.fromDate.value); dt <= new Date(this.TrailerScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                if (type && type == 2) //weekend
                    (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                else
                    this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
            }

            return this.DateList;
        }
    }

    async onSubmit() {
        if (this.TrailerScheduleForm.invalid) {
            this.TrailerScheduleForm.markAllAsTouched();
            return false;
        } else if (this.TrailerScheduleForm.controls.type.value == '3') {
            if (!(this.TrailerScheduleForm.controls.repeat.value && this.TrailerScheduleForm.controls.repeat.value > 0)) {
                Declarations.msgerror('Repeat field is greater than 0', undefined, undefined);
                return false;
            }
        } else if (this.TrailerScheduleForm.controls.type.value == '4') {
            if (!(this.TrailerScheduleForm.controls.customDates.value.length > 0)) {
                Declarations.msgerror('Select custom dates', undefined, undefined);
                return false;
            }
        }
       
        if (this.TrailerScheduleForm.controls.type.value == '2') { this.TrailerScheduleForm.controls.customDates.setValue(await this.InitializeDates(this.TrailerScheduleForm.controls.type.value)); }
        else if (this.TrailerScheduleForm.controls.type.value == '3') { this.TrailerScheduleForm.controls.customDates.setValue(await this.InitializeDates(this.TrailerScheduleForm.controls.type.value, this.TrailerScheduleForm.controls.repeat.value)); }
        else if (this.TrailerScheduleForm.controls.type.value == '1') { this.TrailerScheduleForm.controls.customDates.setValue(await this.InitializeDates()) }
        let dates = [];

        await this.TrailerScheduleForm.controls.customDates.value.map(res => { dates.push(res.Id) });
        let model = new TrailerSchedule();
        this.TrailerScheduleForm.controls.id.value ? model.Id = this.TrailerScheduleForm.controls.id.value : '';
        model.RegionId = this.TrailerScheduleForm.controls.regionId.value[0].Id;
        model.TrailerId = this.TrailerScheduleForm.controls.trailerId.value[0].Id;
        model.StartDate = this.TrailerScheduleForm.controls.fromDate.value;
        model.EndDate = this.TrailerScheduleForm.controls.toDate.value;
        model.IsActive = true;
        model.RepeatDayList = dates;
        model.Type = this.TrailerScheduleForm.controls.type.value;
        model.TrailerShiftDetail = this.TrailerScheduleForm.controls.shifts.value.map(item => ({ ShiftId: item.shiftId[0].Id, ColumnId: item.columnId[0].Id }));
        this.addTrailerSchedule(model);
    }

    private addTrailerSchedule(model): void {
        this.isLoading = true;
        this.driverService.onLoadingChanged.next(true);
        this.driverService.addTrailerSchedule(model)
            .subscribe((response: any) => {

                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Trailer Schedule created successfully', undefined, undefined);
                    this.isLoading = false;
                    this.driverService.onLoadingChanged.next(false);
                    this._toggleOpened(false);
                    //let driver = this.TrailerScheduleForm.controls.driverId.value;
                    this.TrailerScheduleForm.reset();
                    //this.OnScheduleAdded.emit(driver);
                }
                else {
                    this.isLoading = false;
                    this.driverService.onLoadingChanged.next(false);
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
    }
}
