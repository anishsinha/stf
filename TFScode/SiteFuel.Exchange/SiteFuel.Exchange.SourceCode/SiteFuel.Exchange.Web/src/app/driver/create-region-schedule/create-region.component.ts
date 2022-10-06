import { Component, OnInit, Input, ViewChild, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { RegionService } from '../../company-addresses/region/service/region.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { FormGroup, FormBuilder, Validators, NgForm, FormArray } from '@angular/forms';
import { RegionModel } from '../../company-addresses/region/model/region';
import * as moment from 'moment';
import { DropdownItem } from '../../statelist.service';
import { RouteInformationModel } from 'src/app/carrier/models/location';
import { Declarations } from 'src/app/declarations.module';
import { RegionScheduleViewModel, ShiftSchedule } from 'src/app/driver/models/regionSchedule';
import { el } from 'date-fns/locale';
import { NullVisitor } from '@angular/compiler/src/render3/r3_ast';

@Component({
    selector: 'app-create-region',
    templateUrl: './create-region.component.html',
    styleUrls: ['./create-region.component.css']
})

export class CreateRegionComponent implements OnInit {


    @Output() OnScheduleAdded = new EventEmitter();
    constructor(public regionService: RegionService, private _fb: FormBuilder) { }
    public regionList = [];
    DriverList = [];
    RouteList = [];
    ShiftList = [];
    RepeatList = [];
    DateList = [];
    ColumnsList = [];
    ShiftScheduleList: any[] = [];
    CreateRegionForm: FormGroup;
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    SelectedShiftList = [];
    isLoading = false;
    IsDuplicateShift = false;

    public multiselectRouteSettingsById: IDropdownSettings;
    public multiselectRepeatSettingsById: IDropdownSettings;
    public multiselectDateSettingsById: IDropdownSettings;
    public multiselectRegionSettingsById: IDropdownSettings;

    MinStartDate = new Date();
    MaxStartDate = new Date();
    ngOnInit() {

        this.Init();
    }
    Init() {
      
        this.createScheduleForm();
        this.clear();
        this.getRegions();
        this.multiselectRegionSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true,
            enableCheckAll: false
        }
        this.multiselectRouteSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        }
    }

    ngAfterViewInit() {
        this.isLoading = false;
    }

    //#region  Page Load Functions

    private createScheduleForm(): void {
        this.CreateRegionForm = this._fb.group({
            id: this._fb.control[''],
            RegionId:['', Validators.required],
            RouteId: ['', Validators.required],
            ShiftId: [''],
            ColumnIndex: [''],
            type: ['1', Validators.required],
            RegionShiftDetail: this._fb.array([this.getShift()]),
            fromDate: ['', Validators.required],
            toDate: ['', Validators.required],
            repeat: [1],
            customDates: [[]]            
        });
        var dt = (moment(new Date()).toDate())
    }

    private getRegions(): void {
        this.regionService.getRegions()
            .subscribe((region: RegionModel) => {
                this.getRegionDropDwn(region.Regions);
            });
    }

    getShift() {
        return this._fb.group({
            ShiftId: this._fb.control('', Validators.required),
            ColumnIndex: this._fb.control('', Validators.required)
        })
 }
    //#endregion

   
    public _toggleOpened(shouldOpen: boolean): void {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }

//#region Multiselect events 

public onRegionSelect($event) {

    this.ColumnsList = [];
    var region = this.regionList.find(f => f.Id == $event.Id);       
    this.getRoutes(region.Id);
    this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.Name} ( ${res.StartTime} - ${res.EndTime})` } });
    this.createColumnList(region);
}

public onRegionDeSelect($event) {
    this.clear();
}


public clear(){
    this.DriverList = [];
    this.ShiftList = [];
    this.ColumnsList = [];
    this.RouteList = [];
    this.CreateRegionForm.controls.RegionShiftDetail =  this._fb.array([]);
    this.CreateRegionForm.controls.RegionShiftDetail =  this._fb.array([this.getShift()]),
    this.CreateRegionForm.reset();
}

public onRouteSelect($event) {
    var regionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
    this.getShiftSchedules(regionId, $event.Id)
}  

public onShiftSelect($event) {
   
    var shift = this.ShiftList.find(f => f.Id == $event.Id);
    this.IsDuplicateShift = false;
    this.CheckDuplicateShits(shift);
}
public onShiftDeSelect($event) {

}

//#endregion
      
    //#region   Date
    setFromDate(event: any): void {
        this.CreateRegionForm.controls.fromDate.setValue(event);
        let d = moment(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.CreateRegionForm.controls.toDate.value ? this.CreateRegionForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';
        if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
            let _fromDate = moment(this.CreateRegionForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.CreateRegionForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.CreateRegionForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
        this.validateShiftForRegion(false);

    }

    async setToDate(event: any) {
        this.CreateRegionForm.controls.toDate.setValue(event);
        if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
            let _fromDate = moment(this.CreateRegionForm.controls.fromDate.value).toDate();
            let _toDate = moment(this.CreateRegionForm.controls.toDate.value).toDate();
            if (_fromDate > _toDate) {
                this.CreateRegionForm.controls.fromDate.setValue(event);
            }
        }
        this.InitializeDates();
       this.validateShiftForRegion(false);

    }

    async InitializeDates(type?: number, repeat?: number) {
        this.CreateRegionForm.controls.customDates.setValue([]);
        this.DateList = [];
        !repeat ? repeat = 0 : '';
        var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        if (this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value) {

            for (var dt = new Date(this.CreateRegionForm.controls.fromDate.value); dt <= new Date(this.CreateRegionForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                if (type && type == 2) //weekend
                    (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')} ` }) : '';//(${days[new Date(dt).getDay()]})
                else
                    this.DateList.push({ Id: new Date(dt), Name: `${moment(dt).format('MM/DD/YYYY')}` }); //(${days[new Date(dt).getDay()]})
            }

            return this.DateList;
        }
    }

    isInvalid(name: string): boolean {
        var result = this.CreateRegionForm.get(name).invalid && (this.CreateRegionForm.get(name).dirty || this.CreateRegionForm.get(name).touched)
        return result;
    }

    isRequired(name: string): boolean {
        return this.CreateRegionForm.get(name).errors.required;
    }
 //#endregion

    
   //#region  Button Events 
   public closedSideBar(): void { 
       this.clear();
       this._opened = false; 
    }

   addShift() {
    let _shifts = this.CreateRegionForm.get('RegionShiftDetail') as FormArray;
            _shifts.push(this.getShift());
}

removeShift(index: number) {
    let _shifts = this.CreateRegionForm.get('RegionShiftDetail') as FormArray;
    _shifts.removeAt(index);
    this.IsDuplicateShift = false;
}
async onSubmit() {
      
    if (this.CreateRegionForm.invalid) {
        this.CreateRegionForm.markAllAsTouched();
        Declarations.msgerror('All fields are mandatory', undefined, undefined);
        return false;
    } else if (this.CreateRegionForm.controls.type.value == '3') {
        if (!(this.CreateRegionForm.controls.repeat.value && this.CreateRegionForm.controls.repeat.value > 0)) {
            Declarations.msgerror('Repeat field is greater than 0', undefined, undefined);
            return false;
        }
    } else if (this.CreateRegionForm.controls.type.value == '4') {
        if (!(this.CreateRegionForm.controls.customDates.value.length > 0)) {
            Declarations.msgerror('Select custom dates', undefined, undefined);
            return false;
        }
    }
    var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
    if(ScheduleList !=null && ScheduleList.length > 0){
       
       if(this.CheckDuplicateShitsOnSubmit()) {
        Declarations.msgerror("Same selection of shift will not work", undefined, undefined);
        return false;
       }
    }
    else{
        Declarations.msgerror('Atlease one schedule is required, please select Shift and column', undefined, undefined);
        return false;
    }

    if (await this.validateShiftForRegion(true) == 1) { return false;}

    if (this.CreateRegionForm.controls.type.value == '2') { this.CreateRegionForm.controls.customDates.setValue(await this.InitializeDates(this.CreateRegionForm.controls.type.value)); }
    else if (this.CreateRegionForm.controls.type.value == '3') { this.CreateRegionForm.controls.customDates.setValue(await this.InitializeDates(this.CreateRegionForm.controls.type.value, this.CreateRegionForm.controls.repeat.value)); }
    else if (this.CreateRegionForm.controls.type.value == '1') { this.CreateRegionForm.controls.customDates.setValue(await this.InitializeDates()) }
    let dates = [];
    await this.CreateRegionForm.controls.customDates.value.map(res => { dates.push(res.Name) });
    let model = new RegionScheduleViewModel();
    this.CreateRegionForm.controls.id.value ? model.Id = this.CreateRegionForm.controls.id.value : '';
    model.RegionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
    model.RouteId = this.CreateRegionForm.controls.RouteId.value[0].Id;
    model.StartDate = this.CreateRegionForm.controls.fromDate.value;
    model.EndDate = this.CreateRegionForm.controls.toDate.value;

    ScheduleList.forEach(element => {
        var objShiftModel = new ShiftSchedule();
        objShiftModel.ShiftId = element['ShiftId'][0]['Id'];
        objShiftModel.ColumnIndex = parseInt(element['ColumnIndex'][0]['Id']);
        model.RegionShiftDetail.push(objShiftModel);
    });
    
    model.Repeat = this.CreateRegionForm.controls.repeat.value
    model.RepeatDayList = dates;
    model.IsActive = true;
    this.addRouteSchedule(model);
}
   //#endregion

    //#region private functions 

    async validateShiftForRegion(isSubmit:boolean) {
        var k = 0;
        if (this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value && this.CreateRegionForm.controls.RegionId.value && this.CreateRegionForm.controls.RouteId.value) {
            var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
            for (let shift of this.ShiftScheduleList) {
                //for await (let shift of item) {
                    if ((moment(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment(shift.StartDate).format('MM/DD/YYYY') &&
                        moment(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment(shift.EndDate).format('MM/DD/YYYY')) ||
                        (moment(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') >= moment(shift.StartDate).format('MM/DD/YYYY') &&
                            moment(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') <= moment(shift.EndDate).format('MM/DD/YYYY'))) {                            
                               
                        if (ScheduleList != null && ScheduleList.length > 0) {
                            for (var i = 0; i < ScheduleList.length; i++) {
                                var iShift = ScheduleList[i];
                                for (var j = 0; j < shift.RegionShiftDetail.length; j++) {
                                    var jShift = shift.RegionShiftDetail[j]
                                    if(iShift.ShiftId != null && iShift.ShiftId[0].Id==  jShift.ShiftId)
                                    {
                                        Declarations.msgerror("Shift is already assigned to the Region", undefined, undefined);
                                        k = 1;
                                        return false;                                        
                                    }
                                    else
                                    {
                                        !isSubmit ? Declarations.msgwarning("Another shift is already assigned to the Region", undefined, undefined):'';
                                    }
                                }
                            }

                        }
                    }
              //  }
                if (k == 1)
                    break;
            }
            return k;
        }
        return k;
    }

    private addRouteSchedule(model): void {
        this.isLoading = true;
        this.regionService.onLoadingChanged.next(true);
        this.regionService.addRegionSchedule(model)
            .subscribe((response: any) => {

                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Region Schedule created successfully', undefined, undefined);
                    this.isLoading = false;
                    this.regionService.onLoadingChanged.next(false);
                    this._toggleOpened(false);
                    let driver = this.CreateRegionForm.controls.driverId.value;
                    this.CreateRegionForm.reset();
                    this.OnScheduleAdded.emit(driver);

                }
                else {
                    this.isLoading = false;
                    this.regionService.onLoadingChanged.next(false);
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
    }

    private createColumnList(region) {
        this.DriverList = region.Drivers;
        if (region.Drivers.length > 0) {
            var num = 0;
            this.DriverList.forEach(obj => {
                var col = {
                    Id: 0,
                    Name: ""
                }
                col.Id = num;
                col.Name = "C" + num;
                this.ColumnsList.push(col)
                num++
            })
        }
    }

    private CheckDuplicateShits(shift) {
        var cnt = 1;
        var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
        if (ScheduleList.length > 1) {
            for (var i = 0; i < ScheduleList.length; i++) {
                var iShift = ScheduleList[i];
                if (iShift.ShiftId !=""  && iShift.ShiftId[0].Id == shift.Id) {
                    if (cnt > 1)
                        this.IsDuplicateShift = true;
                    cnt++
                }
            }
        }
        if (this.IsDuplicateShift) {
            Declarations.msgerror("Same selection of shift will not work", undefined, undefined);
        }
    }

    private CheckDuplicateShitsOnSubmit() {        
        var checkDuplicate= false;
        var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
        if (ScheduleList.length > 1) {
            for (var j = 0; j < ScheduleList.length; j++) {
             var   shift = ScheduleList[j];
             var cnt = 1;
            for (var i = 0; i < ScheduleList.length; i++) {
                var iShift = ScheduleList[i];
                if (iShift.ShiftId !="" && shift.ShiftId !="" && iShift.ShiftId[0].Id == shift.ShiftId[0].Id) {
                    if (cnt > 1){                                        
                        return checkDuplicate = true;
                    }  
                    cnt++
                }
            }
        }
        }
        
        return checkDuplicate;
    }


    private getShiftSchedules(regionId, routeId): void {
        this.regionService.getRegionSchedule(regionId, routeId)
            .subscribe((schedules: RegionScheduleViewModel[]) => {
                this.pushRegionShiftDetail(schedules);
            });
    }

    public pushRegionShiftDetail(schedules) {   

        this.ShiftScheduleList = schedules;
    }

    private getRoutes(id): void {
        this.regionService.getRoutesByRegion(id)
            .subscribe((routes: RouteInformationModel[]) => {
                this.getRouteDropDown(routes);
            });
    }

    private getRouteDropDown(routeList): void {
        this.RouteList = routeList.ResponseData
    }
    private getRegionDropDwn(regionList): void {
        this.regionList = regionList;
    }

    //#endregion
}