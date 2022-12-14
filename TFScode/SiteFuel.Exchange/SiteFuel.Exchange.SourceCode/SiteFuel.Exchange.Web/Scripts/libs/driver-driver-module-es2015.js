(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["driver-driver-module"],{

/***/ "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts":
/*!***********************************************************************************!*\
  !*** ./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts ***!
  \***********************************************************************************/
/*! exports provided: CreateDriverScheduleComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateDriverScheduleComponent", function() { return CreateDriverScheduleComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_driver_models_DriverSchedule__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/driver/models/DriverSchedule */ "./src/app/driver/models/DriverSchedule.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");













function CreateDriverScheduleComponent_div_17_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateDriverScheduleComponent_div_17_div_1_Template, 2, 0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r0.isRequired("regionId"));
} }
function CreateDriverScheduleComponent_div_24_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateDriverScheduleComponent_div_24_div_1_Template, 2, 0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r1.isRequired("driverId"));
} }
function CreateDriverScheduleComponent_div_30_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_30_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateDriverScheduleComponent_div_30_div_1_Template, 2, 0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.isRequired("shiftId"));
} }
function CreateDriverScheduleComponent_div_38_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_38_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateDriverScheduleComponent_div_38_div_1_Template, 2, 0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.isRequired("fromDate"));
} }
function CreateDriverScheduleComponent_div_45_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_45_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateDriverScheduleComponent_div_45_div_1_Template, 2, 0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.isRequired("toDate"));
} }
function CreateDriverScheduleComponent_div_61_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "input", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function CreateDriverScheduleComponent_div_61_Template_input_change_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r16); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r15.InitializeDates(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Custom");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateDriverScheduleComponent_div_64_Template(rf, ctx) { if (rf & 1) {
    const _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, " Dates:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "ng-multiselect-dropdown", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateDriverScheduleComponent_div_64_Template_ng_multiselect_dropdown_onSelect_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r18); const ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r17.validateShiftForDriver(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Date (s)")("settings", ctx_r8.multiselectDateSettingsById)("data", ctx_r8.DateList);
} }
function CreateDriverScheduleComponent_div_65_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Days:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "input", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
class CreateDriverScheduleComponent {
    //end
    constructor(regionService, _fb) {
        this.regionService = regionService;
        this._fb = _fb;
        //@ViewChild('DriverScheduleForm') public form: NgForm;
        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.DriverList = [];
        this.regionList = [];
        this.isLoading = false;
        //sidebar variables
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedShiftList = [];
        this.ShiftList = [];
        this.SelectedScheduleDriverList = [];
        this.RepeatList = [];
        this.DateList = [];
        //min max date
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
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
        this.DriverShiftDetailList = [];
    }
    ngOnInit() {
        this.init();
    }
    ngAfterViewInit() {
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
        };
        this.multiselectShiftSettingsById = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        };
        this.multiselectRepeatSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        };
        this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
        };
    }
    createScheduleForm() {
        this.DriverScheduleForm = this._fb.group({
            id: [''],
            shiftId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            regionId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            driverId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            fromDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            toDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]],
        });
        var dt = moment__WEBPACK_IMPORTED_MODULE_5__(new Date()).toDate();
        //alert(moment(dt).format('MM/DD/YYYY'));
        this.DriverScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'));
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }
    getRegions() {
        this.regionService.getRegions()
            .subscribe((region) => {
            this.getRegionDropDwn(region.Regions);
        });
    }
    onRegionSelect($event) {
        var region = this.regionList.find(f => f.Id == $event.Id);
        this.DriverScheduleForm.controls.driverId.setValue('');
        this.DriverScheduleForm.controls.shiftId.setValue('');
        let tDate = new Date();
        this.DriverScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(tDate).format('MM/DD/YYYY'));
        this.setFromDate(moment__WEBPACK_IMPORTED_MODULE_5__(tDate).format('MM/DD/YYYY'));
        this.DriverList = region.Drivers;
        this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` }; });
    }
    onRegionDeSelect($event) {
        this.DriverList = [];
        this.DriverScheduleForm.controls.driverId.setValue('');
        this.ShiftList = [];
        this.DriverScheduleForm.controls.shiftId.setValue('');
    }
    getRegionDropDwn(regionList) {
        this.regionList = regionList;
    }
    isInvalid(name) {
        var result = this.DriverScheduleForm.get(name).invalid && (this.DriverScheduleForm.get(name).dirty || this.DriverScheduleForm.get(name).touched);
        return result;
    }
    isRequired(name) {
        return this.DriverScheduleForm.get(name).errors.required;
    }
    setFromDate(event) {
        this.DriverScheduleForm.controls.fromDate.setValue(event);
        //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        let d = moment__WEBPACK_IMPORTED_MODULE_5__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(d).format('MM/DD/YYYY')) : '';
        if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.DriverScheduleForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
        this.validateShiftForDriver(false);
    }
    setToDate(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.DriverScheduleForm.controls.toDate.setValue(event);
            if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
                let _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).toDate();
                let _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).toDate();
                if (_fromDate > _toDate) {
                    this.DriverScheduleForm.controls.fromDate.setValue(event);
                }
            }
            this.InitializeDates();
            yield this.validateShiftForDriver(false);
        });
    }
    InitializeDates(type, repeat) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.DriverScheduleForm.controls.customDates.setValue([]);
            this.DateList = [];
            !repeat ? repeat = 0 : '';
            var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            // alert(this.DriverScheduleForm.controls.fromDate.value);
            // alert(this.DriverScheduleForm.controls.toDate.value);
            if (this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value) {
                for (var dt = new Date(this.DriverScheduleForm.controls.fromDate.value); dt <= new Date(this.DriverScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                    if (type && type == 2) //weekend
                        (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                    else
                        this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
                }
                return this.DateList;
            }
        });
    }
    onSubmit() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            //this.DriverScheduleForm.controls.repeat.value &&  this.DriverScheduleForm.controls.repeat.value[0].Id == '1' ? await this.DriverScheduleForm.controls.customDates.setValue(this.DateList) : '';
            if (this.DriverScheduleForm.invalid) {
                this.DriverScheduleForm.markAllAsTouched();
                return false;
            }
            else if (this.DriverScheduleForm.controls.type.value == '3') {
                if (!(this.DriverScheduleForm.controls.repeat.value && this.DriverScheduleForm.controls.repeat.value > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return false;
                }
            }
            else if (this.DriverScheduleForm.controls.type.value == '4') {
                if (!(this.DriverScheduleForm.controls.customDates.value.length > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return false;
                }
            }
            if ((yield this.validateShiftForDriver(true)) == 1) {
                return false;
            }
            //this.DriverScheduleForm.controls.customDates.setValue([]);
            if (this.DriverScheduleForm.controls.type.value == '2') {
                this.DriverScheduleForm.controls.customDates.setValue(yield this.InitializeDates(this.DriverScheduleForm.controls.type.value));
            }
            else if (this.DriverScheduleForm.controls.type.value == '3') {
                this.DriverScheduleForm.controls.customDates.setValue(yield this.InitializeDates(this.DriverScheduleForm.controls.type.value, this.DriverScheduleForm.controls.repeat.value));
            }
            else if (this.DriverScheduleForm.controls.type.value == '1') {
                this.DriverScheduleForm.controls.customDates.setValue(yield this.InitializeDates());
            }
            let dates = [];
            //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
            //!this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';
            yield this.DriverScheduleForm.controls.customDates.value.map(res => { dates.push(res.Id); });
            let model = new src_app_driver_models_DriverSchedule__WEBPACK_IMPORTED_MODULE_4__["DriverScheduleMapping"]();
            this.DriverScheduleForm.controls.id.value ? model.Id = this.DriverScheduleForm.controls.id.value : '';
            model.DriverId = this.DriverScheduleForm.controls.driverId.value[0].Id;
            var repeatDayListString = [];
            dates.forEach(x => {
                repeatDayListString.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
            });
            model.ScheduleList.push({ Id: `${model.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: this.DriverScheduleForm.controls.fromDate.value, EndDate: this.DriverScheduleForm.controls.toDate.value, RepeatDayList: dates, RepeatDayStringList: repeatDayListString, ShiftId: this.DriverScheduleForm.controls.shiftId.value[0].Id, Type: this.DriverScheduleForm.controls.type.value, RepeatEveryDay: this.DriverScheduleForm.controls.repeat.value, TypeId: this.DriverScheduleForm.controls.type.value });
            this.addDriverSchedule(model);
        });
    }
    addDriverSchedule(model) {
        this.isLoading = true;
        this.regionService.onLoadingChanged.next(true);
        this.regionService.addDriverSchedule(model)
            .subscribe((response) => {
            if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess('Driver Schedule created successfully', undefined, undefined);
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
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
        });
    }
    closedSideBar() { this._opened = false; }
    onDriverSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(true);
            let driverIds = [];
            this.DriverScheduleForm.controls.driverId.value.forEach(res => { driverIds.push(res.Id); });
            let drivers = driverIds.join();
            if (driverIds.length > 0) {
                this.regionService.getShiftByDrivers(drivers, 0) // schedule type
                    .subscribe((data) => {
                    if (data.Result)
                        this.DriverShiftDetailList = data.Result;
                });
            }
            this.regionService.onLoadingChanged.next(false);
        });
    }
    onDriverDeSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
        });
    }
    validateShiftForDriver(isSubmit) {
        var e_1, _a, e_2, _b;
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var i = 0;
            let rpDayList = [];
            if (this.DriverScheduleForm.controls.type.value == 4)
                rpDayList = this.DriverScheduleForm.controls.customDates.value;
            else
                rpDayList = this.DateList;
            if (this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value && this.DriverScheduleForm.controls.shiftId.value && this.DriverScheduleForm.controls.shiftId.value.length > 0) {
                try {
                    for (var _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.DriverShiftDetailList), _d; _d = yield _c.next(), !_d.done;) {
                        let item = _d.value;
                        try {
                            for (var _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.ScheduleList), _f; _f = yield _e.next(), !_f.done;) {
                                let shift = _f.value;
                                if (shift.RepeatDayList != null && (moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_5__(shift.StartDate).format('MM/DD/YYYY') &&
                                    moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_5__(shift.EndDate).format('MM/DD/YYYY')) ||
                                    (moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_5__(shift.StartDate).format('MM/DD/YYYY') &&
                                        moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_5__(shift.EndDate).format('MM/DD/YYYY'))) {
                                    if (this.DriverScheduleForm.controls.shiftId.value[0].Id == shift.ShiftId) {
                                        shift.RepeatDayList.forEach(ele => {
                                            let idx = rpDayList.findIndex(x => moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(ele).format('MM/DD/YYYY'));
                                            if (idx >= 0) {
                                                if (i != 1) {
                                                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Shift is already assigned to the driver", undefined, undefined);
                                                    i = 1;
                                                }
                                            }
                                        });
                                        if (i == 1)
                                            break;
                                    }
                                    else {
                                        !isSubmit ? src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Another shift is already assigned to the driver", undefined, undefined) : '';
                                        //i = 1;
                                        //break;
                                    }
                                }
                            }
                        }
                        catch (e_2_1) { e_2 = { error: e_2_1 }; }
                        finally {
                            try {
                                if (_f && !_f.done && (_b = _e.return)) yield _b.call(_e);
                            }
                            finally { if (e_2) throw e_2.error; }
                        }
                        if (i == 1)
                            break;
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_d && !_d.done && (_a = _c.return)) yield _a.call(_c);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
                return i;
            }
            return i;
        });
    }
}
CreateDriverScheduleComponent.??fac = function CreateDriverScheduleComponent_Factory(t) { return new (t || CreateDriverScheduleComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"])); };
CreateDriverScheduleComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: CreateDriverScheduleComponent, selectors: [["create-driver-schedule"]], outputs: { OnScheduleAdded: "OnScheduleAdded" }, decls: 70, vars: 27, consts: [["id", "driverSchedule", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["name", "DriverScheduleForm", "autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "regionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Drivers"], ["formControlName", "driverId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["for", "Shift"], ["formControlName", "shiftId", 3, "placeholder", "settings", "data", "onSelect"], ["for", "fromDate"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], [1, "color-maroon"], [4, "ngIf"], ["type", "radio", "name", "type", "formControlName", "type", "value", "4", "id", "inlineRadioCustom", 1, "form-check-input", 3, "change"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], ["formControlName", "customDates", 3, "placeholder", "settings", "data", "onSelect"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]], template: function CreateDriverScheduleComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateDriverScheduleComponent_Template_button_click_0_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "Add Schedule ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "ng-sidebar-container", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "ng-sidebar", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("openedChange", function CreateDriverScheduleComponent_Template_ng_sidebar_openedChange_5_listener($event) { return ctx._opened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateDriverScheduleComponent_Template_a_click_6_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "h3", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Schedule Driver ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "content", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("keydown.enter", function CreateDriverScheduleComponent_Template_content_keydown_enter_10_listener($event) { return $event.preventDefault(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Region:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_16_listener($event) { return ctx.onRegionSelect($event); })("onDeSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_16_listener($event) { return ctx.onRegionDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, CreateDriverScheduleComponent_div_17_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Driver:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_23_listener($event) { return ctx.onDriverSelect($event); })("onDeSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_23_listener($event) { return ctx.onDriverDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, CreateDriverScheduleComponent_div_24_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Shift:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "ng-multiselect-dropdown", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_29_listener() { return ctx.validateShiftForDriver(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](30, CreateDriverScheduleComponent_div_30_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](35, "From Date:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "input", 19, 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateDriverScheduleComponent_Template_input_onDateChange_36_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, CreateDriverScheduleComponent_div_38_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](42, "To Date:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "input", 21, 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateDriverScheduleComponent_Template_input_onDateChange_43_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](45, CreateDriverScheduleComponent_div_45_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](50, "input", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "label", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](52, "Daily");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](54, "input", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "label", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](56, "WeekDays (Mon to Fri)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](58, "input", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "label", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](60, "Repeat Every");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](61, CreateDriverScheduleComponent_div_61_Template, 4, 0, "div", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](64, CreateDriverScheduleComponent_div_64_Template, 4, 3, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](65, CreateDriverScheduleComponent_div_65_Template, 4, 0, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "div", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "input", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateDriverScheduleComponent_Template_input_click_67_listener() { return ctx.closedSideBar(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "button", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateDriverScheduleComponent_Template_button_click_68_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](69, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.DriverScheduleForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Region(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.regionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("regionId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Driver(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.DriverList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("driverId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Shift(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.ShiftList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("shiftId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("fromDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("toDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.DriverScheduleForm.get("fromDate").value && ctx.DriverScheduleForm.get("toDate").value);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.DriverScheduleForm.get("type").value == "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.DriverScheduleForm.get("type").value == "3");
    } }, directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]], styles: [".hide_chart[_ngcontent-%COMP%] {\r\n    display:none;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2NyZWF0ZS1kcml2ZXItc2NoZWR1bGUvY3JlYXRlLWRyaXZlci1zY2hlZHVsZS5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtBQUNoQiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtZHJpdmVyLXNjaGVkdWxlL2NyZWF0ZS1kcml2ZXItc2NoZWR1bGUuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5oaWRlX2NoYXJ0IHtcclxuICAgIGRpc3BsYXk6bm9uZTtcclxufVxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CreateDriverScheduleComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'create-driver-schedule',
                templateUrl: './create-driver-schedule.component.html',
                styleUrls: ['./create-driver-schedule.component.css']
            }]
    }], function () { return [{ type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }]; }, { OnScheduleAdded: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/driver/create-region-schedule/create-region.component.ts":
/*!**************************************************************************!*\
  !*** ./src/app/driver/create-region-schedule/create-region.component.ts ***!
  \**************************************************************************/
/*! exports provided: CreateRegionComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateRegionComponent", function() { return CreateRegionComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/driver/models/regionSchedule */ "./src/app/driver/models/regionSchedule.ts");
/* harmony import */ var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");













function CreateRegionComponent_div_17_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_17_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r0.isRequired("RegionId"));
} }
function CreateRegionComponent_div_23_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_23_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r1.isRequired("RouteId"));
} }
function CreateRegionComponent_div_29_div_10_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_29_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_29_div_10_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r12.isRequired("ShiftId"));
} }
function CreateRegionComponent_div_29_div_18_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_29_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_29_div_18_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r13.isRequired("ColumnIndex"));
} }
function CreateRegionComponent_div_29_Template(rf, ctx) { if (rf & 1) {
    const _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "label", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Shift ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "span", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "ng-multiselect-dropdown", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateRegionComponent_div_29_Template_ng_multiselect_dropdown_onSelect_9_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r17); const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r16.onShiftSelect($event); })("onDeSelect", function CreateRegionComponent_div_29_Template_ng_multiselect_dropdown_onDeSelect_9_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r17); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r18.onShiftDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, CreateRegionComponent_div_29_div_10_Template, 2, 1, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "label", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Column");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "span", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](17, "ng-multiselect-dropdown", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, CreateRegionComponent_div_29_div_18_Template, 2, 1, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](20, "label", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "a", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_div_29_Template_a_click_21_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r17); const i_r11 = ctx.index; const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r19.removeShift(i_r11); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "i", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Remove");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const i_r11 = ctx.index;
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroupName", i_r11);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Shift(s)")("settings", ctx_r2.multiselectRegionSettingsById)("data", ctx_r2.ShiftList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.isInvalid("ShiftId"));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Columns")("settings", ctx_r2.multiselectRegionSettingsById)("data", ctx_r2.ColumnsList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.isInvalid("ColumnIndex"));
} }
function CreateRegionComponent_div_39_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_39_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_39_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.isRequired("fromDate"));
} }
function CreateRegionComponent_div_48_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateRegionComponent_div_48_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateRegionComponent_div_48_div_1_Template, 2, 0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.isRequired("toDate"));
} }
function CreateRegionComponent_div_66_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Days:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "input", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
class CreateRegionComponent {
    constructor(regionService, _fb) {
        this.regionService = regionService;
        this._fb = _fb;
        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.regionList = [];
        this.DriverList = [];
        this.RouteList = [];
        this.ShiftList = [];
        this.RepeatList = [];
        this.DateList = [];
        this.ColumnsList = [];
        this.ShiftScheduleList = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedShiftList = [];
        this.isLoading = false;
        this.IsDuplicateShift = false;
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
    }
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
        };
        this.multiselectRouteSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
        };
    }
    ngAfterViewInit() {
        this.isLoading = false;
    }
    //#region  Page Load Functions
    createScheduleForm() {
        this.CreateRegionForm = this._fb.group({
            id: this._fb.control[''],
            RegionId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            RouteId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            ShiftId: [''],
            ColumnIndex: [''],
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            RegionShiftDetail: this._fb.array([this.getShift()]),
            fromDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            toDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]]
        });
        var dt = (moment__WEBPACK_IMPORTED_MODULE_3__(new Date()).toDate());
    }
    getRegions() {
        this.regionService.getRegions()
            .subscribe((region) => {
            this.getRegionDropDwn(region.Regions);
        });
    }
    getShift() {
        return this._fb.group({
            ShiftId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            ColumnIndex: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required)
        });
    }
    //#endregion
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }
    //#region Multiselect events 
    onRegionSelect($event) {
        this.ColumnsList = [];
        var region = this.regionList.find(f => f.Id == $event.Id);
        this.getRoutes(region.Id);
        this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.Name} ( ${res.StartTime} - ${res.EndTime})` }; });
        this.createColumnList(region);
    }
    onRegionDeSelect($event) {
        this.clear();
    }
    clear() {
        this.DriverList = [];
        this.ShiftList = [];
        this.ColumnsList = [];
        this.RouteList = [];
        this.CreateRegionForm.controls.RegionShiftDetail = this._fb.array([]);
        this.CreateRegionForm.controls.RegionShiftDetail = this._fb.array([this.getShift()]),
            this.CreateRegionForm.reset();
    }
    onRouteSelect($event) {
        var regionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
        this.getShiftSchedules(regionId, $event.Id);
    }
    onShiftSelect($event) {
        var shift = this.ShiftList.find(f => f.Id == $event.Id);
        this.IsDuplicateShift = false;
        this.CheckDuplicateShits(shift);
    }
    onShiftDeSelect($event) {
    }
    //#endregion
    //#region   Date
    setFromDate(event) {
        this.CreateRegionForm.controls.fromDate.setValue(event);
        let d = moment__WEBPACK_IMPORTED_MODULE_3__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.CreateRegionForm.controls.toDate.value ? this.CreateRegionForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(d).format('MM/DD/YYYY')) : '';
        if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
            let _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).toDate();
            let _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.CreateRegionForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
        this.validateShiftForRegion(false);
    }
    setToDate(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CreateRegionForm.controls.toDate.setValue(event);
            if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
                let _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).toDate();
                let _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).toDate();
                if (_fromDate > _toDate) {
                    this.CreateRegionForm.controls.fromDate.setValue(event);
                }
            }
            this.InitializeDates();
            this.validateShiftForRegion(false);
        });
    }
    InitializeDates(type, repeat) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CreateRegionForm.controls.customDates.setValue([]);
            this.DateList = [];
            !repeat ? repeat = 0 : '';
            var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            if (this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value) {
                for (var dt = new Date(this.CreateRegionForm.controls.fromDate.value); dt <= new Date(this.CreateRegionForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                    if (type && type == 2) //weekend
                        (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY')} ` }) : ''; //(${days[new Date(dt).getDay()]})
                    else
                        this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY')}` }); //(${days[new Date(dt).getDay()]})
                }
                return this.DateList;
            }
        });
    }
    isInvalid(name) {
        var result = this.CreateRegionForm.get(name).invalid && (this.CreateRegionForm.get(name).dirty || this.CreateRegionForm.get(name).touched);
        return result;
    }
    isRequired(name) {
        return this.CreateRegionForm.get(name).errors.required;
    }
    //#endregion
    //#region  Button Events 
    closedSideBar() {
        this.clear();
        this._opened = false;
    }
    addShift() {
        let _shifts = this.CreateRegionForm.get('RegionShiftDetail');
        _shifts.push(this.getShift());
    }
    removeShift(index) {
        let _shifts = this.CreateRegionForm.get('RegionShiftDetail');
        _shifts.removeAt(index);
        this.IsDuplicateShift = false;
    }
    onSubmit() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (this.CreateRegionForm.invalid) {
                this.CreateRegionForm.markAllAsTouched();
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('All fields are mandatory', undefined, undefined);
                return false;
            }
            else if (this.CreateRegionForm.controls.type.value == '3') {
                if (!(this.CreateRegionForm.controls.repeat.value && this.CreateRegionForm.controls.repeat.value > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return false;
                }
            }
            else if (this.CreateRegionForm.controls.type.value == '4') {
                if (!(this.CreateRegionForm.controls.customDates.value.length > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return false;
                }
            }
            var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
            if (ScheduleList != null && ScheduleList.length > 0) {
                if (this.CheckDuplicateShitsOnSubmit()) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Same selection of shift will not work", undefined, undefined);
                    return false;
                }
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Atlease one schedule is required, please select Shift and column', undefined, undefined);
                return false;
            }
            if ((yield this.validateShiftForRegion(true)) == 1) {
                return false;
            }
            if (this.CreateRegionForm.controls.type.value == '2') {
                this.CreateRegionForm.controls.customDates.setValue(yield this.InitializeDates(this.CreateRegionForm.controls.type.value));
            }
            else if (this.CreateRegionForm.controls.type.value == '3') {
                this.CreateRegionForm.controls.customDates.setValue(yield this.InitializeDates(this.CreateRegionForm.controls.type.value, this.CreateRegionForm.controls.repeat.value));
            }
            else if (this.CreateRegionForm.controls.type.value == '1') {
                this.CreateRegionForm.controls.customDates.setValue(yield this.InitializeDates());
            }
            let dates = [];
            yield this.CreateRegionForm.controls.customDates.value.map(res => { dates.push(res.Name); });
            let model = new src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__["RegionScheduleViewModel"]();
            this.CreateRegionForm.controls.id.value ? model.Id = this.CreateRegionForm.controls.id.value : '';
            model.RegionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
            model.RouteId = this.CreateRegionForm.controls.RouteId.value[0].Id;
            model.StartDate = this.CreateRegionForm.controls.fromDate.value;
            model.EndDate = this.CreateRegionForm.controls.toDate.value;
            ScheduleList.forEach(element => {
                var objShiftModel = new src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__["ShiftSchedule"]();
                objShiftModel.ShiftId = element['ShiftId'][0]['Id'];
                objShiftModel.ColumnIndex = parseInt(element['ColumnIndex'][0]['Id']);
                model.RegionShiftDetail.push(objShiftModel);
            });
            model.Repeat = this.CreateRegionForm.controls.repeat.value;
            model.RepeatDayList = dates;
            model.IsActive = true;
            this.addRouteSchedule(model);
        });
    }
    //#endregion
    //#region private functions 
    validateShiftForRegion(isSubmit) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var k = 0;
            if (this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value && this.CreateRegionForm.controls.RegionId.value && this.CreateRegionForm.controls.RouteId.value) {
                var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
                for (let shift of this.ShiftScheduleList) {
                    //for await (let shift of item) {
                    if ((moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_3__(shift.StartDate).format('MM/DD/YYYY') &&
                        moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_3__(shift.EndDate).format('MM/DD/YYYY')) ||
                        (moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_3__(shift.StartDate).format('MM/DD/YYYY') &&
                            moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_3__(shift.EndDate).format('MM/DD/YYYY'))) {
                        if (ScheduleList != null && ScheduleList.length > 0) {
                            for (var i = 0; i < ScheduleList.length; i++) {
                                var iShift = ScheduleList[i];
                                for (var j = 0; j < shift.RegionShiftDetail.length; j++) {
                                    var jShift = shift.RegionShiftDetail[j];
                                    if (iShift.ShiftId != null && iShift.ShiftId[0].Id == jShift.ShiftId) {
                                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Shift is already assigned to the Region", undefined, undefined);
                                        k = 1;
                                        return false;
                                    }
                                    else {
                                        !isSubmit ? src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgwarning("Another shift is already assigned to the Region", undefined, undefined) : '';
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
        });
    }
    addRouteSchedule(model) {
        this.isLoading = true;
        this.regionService.onLoadingChanged.next(true);
        this.regionService.addRegionSchedule(model)
            .subscribe((response) => {
            if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess('Region Schedule created successfully', undefined, undefined);
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
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
        });
    }
    createColumnList(region) {
        this.DriverList = region.Drivers;
        if (region.Drivers.length > 0) {
            var num = 0;
            this.DriverList.forEach(obj => {
                var col = {
                    Id: 0,
                    Name: ""
                };
                col.Id = num;
                col.Name = "C" + num;
                this.ColumnsList.push(col);
                num++;
            });
        }
    }
    CheckDuplicateShits(shift) {
        var cnt = 1;
        var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
        if (ScheduleList.length > 1) {
            for (var i = 0; i < ScheduleList.length; i++) {
                var iShift = ScheduleList[i];
                if (iShift.ShiftId != "" && iShift.ShiftId[0].Id == shift.Id) {
                    if (cnt > 1)
                        this.IsDuplicateShift = true;
                    cnt++;
                }
            }
        }
        if (this.IsDuplicateShift) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Same selection of shift will not work", undefined, undefined);
        }
    }
    CheckDuplicateShitsOnSubmit() {
        var checkDuplicate = false;
        var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
        if (ScheduleList.length > 1) {
            for (var j = 0; j < ScheduleList.length; j++) {
                var shift = ScheduleList[j];
                var cnt = 1;
                for (var i = 0; i < ScheduleList.length; i++) {
                    var iShift = ScheduleList[i];
                    if (iShift.ShiftId != "" && shift.ShiftId != "" && iShift.ShiftId[0].Id == shift.ShiftId[0].Id) {
                        if (cnt > 1) {
                            return checkDuplicate = true;
                        }
                        cnt++;
                    }
                }
            }
        }
        return checkDuplicate;
    }
    getShiftSchedules(regionId, routeId) {
        this.regionService.getRegionSchedule(regionId, routeId)
            .subscribe((schedules) => {
            this.pushRegionShiftDetail(schedules);
        });
    }
    pushRegionShiftDetail(schedules) {
        this.ShiftScheduleList = schedules;
    }
    getRoutes(id) {
        this.regionService.getRoutesByRegion(id)
            .subscribe((routes) => {
            this.getRouteDropDown(routes);
        });
    }
    getRouteDropDown(routeList) {
        this.RouteList = routeList.ResponseData;
    }
    getRegionDropDwn(regionList) {
        this.regionList = regionList;
    }
}
CreateRegionComponent.??fac = function CreateRegionComponent_Factory(t) { return new (t || CreateRegionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"])); };
CreateRegionComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: CreateRegionComponent, selectors: [["app-create-region"]], outputs: { OnScheduleAdded: "OnScheduleAdded" }, decls: 71, vars: 23, consts: [["id", "CreateRoute", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "RegionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Route"], ["formControlName", "RouteId", 3, "placeholder", "settings", "data", "onSelect"], [1, "col-sm-6", 2, "padding-left", "90%"], ["href", "javascript:void(0)", 3, "click"], [1, "fa", "fa-plus"], ["formArrayName", "RegionShiftDetail", 4, "ngFor", "ngForOf"], ["for", "fromDate"], [1, "Required"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "disabled", "click"], [1, "color-maroon"], [4, "ngIf"], ["formArrayName", "RegionShiftDetail"], [3, "formGroupName"], ["for", "RegionShiftDetail"], ["formControlName", "ShiftId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [1, "col-sm-3"], ["for", "columns"], ["formControlName", "ColumnIndex", 3, "placeholder", "settings", "data"], [1, "col-sm-1"], ["href", "javascript:void(0)", 2, "padding-top", "50%", 3, "click"], [1, "fa", "fa-remove"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]], template: function CreateRegionComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_Template_button_click_0_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Add Route ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "ng-sidebar-container", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "ng-sidebar", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_Template_a_click_6_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "h3", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Create Route");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "content", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("keydown.enter", function CreateRegionComponent_Template_content_keydown_enter_10_listener($event) { return $event.preventDefault(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Region");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onSelect_16_listener($event) { return ctx.onRegionSelect($event); })("onDeSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onDeSelect_16_listener($event) { return ctx.onRegionDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, CreateRegionComponent_div_17_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21, "Route");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onSelect_22_listener($event) { return ctx.onRouteSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, CreateRegionComponent_div_23_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "a", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_Template_a_click_26_listener() { return ctx.addShift(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "i", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, " Add Shift");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](29, CreateRegionComponent_div_29_Template, 24, 9, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "label", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "From Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "span", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "input", 22, 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateRegionComponent_Template_input_onDateChange_37_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](39, CreateRegionComponent_div_39_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "label", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](43, "To Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "span", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](45, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "input", 24, 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateRegionComponent_Template_input_onDateChange_46_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](48, CreateRegionComponent_div_48_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](53, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "label", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](55, "Daily");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](57, "input", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "label", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](59, "WeekDays (Mon to Fri)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](61, "input", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "label", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](63, "Repeat Every");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](66, CreateRegionComponent_div_66_Template, 4, 0, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "div", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "input", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_Template_input_click_68_listener() { return ctx.closedSideBar(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](69, "button", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateRegionComponent_Template_button_click_69_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](70, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.CreateRegionForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Region(s)")("settings", ctx.multiselectRegionSettingsById)("data", ctx.regionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("RegionId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Route(s)")("settings", ctx.multiselectRegionSettingsById)("data", ctx.RouteList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("RouteId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.CreateRegionForm.get("RegionShiftDetail")["controls"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("fromDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("toDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.CreateRegionForm.get("type").value == "3");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.IsDuplicateShift);
    } }, directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]], styles: [".hide_chart[_ngcontent-%COMP%] {\r\n    display: none;\r\n}\r\n.required[_ngcontent-%COMP%] {\r\n    color: #e41813;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2NyZWF0ZS1yZWdpb24tc2NoZWR1bGUvY3JlYXRlLXJlZ2lvbi5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksYUFBYTtBQUNqQjtBQUNBO0lBQ0ksY0FBYztBQUNsQiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtcmVnaW9uLXNjaGVkdWxlL2NyZWF0ZS1yZWdpb24uY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5oaWRlX2NoYXJ0IHtcclxuICAgIGRpc3BsYXk6IG5vbmU7XHJcbn1cclxuLnJlcXVpcmVkIHtcclxuICAgIGNvbG9yOiAjZTQxODEzO1xyXG59Il19 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CreateRegionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-create-region',
                templateUrl: './create-region.component.html',
                styleUrls: ['./create-region.component.css']
            }]
    }], function () { return [{ type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }]; }, { OnScheduleAdded: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/driver/create-trailer-schedule/create-trailer-schedule.component.ts":
/*!*************************************************************************************!*\
  !*** ./src/app/driver/create-trailer-schedule/create-trailer-schedule.component.ts ***!
  \*************************************************************************************/
/*! exports provided: CreateTrailerScheduleComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateTrailerScheduleComponent", function() { return CreateTrailerScheduleComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_driver_models_TrailerSchedule__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/driver/models/TrailerSchedule */ "./src/app/driver/models/TrailerSchedule.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var _services_driver_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../services/driver.service */ "./src/app/driver/services/driver.service.ts");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");














function CreateTrailerScheduleComponent_div_16_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateTrailerScheduleComponent_div_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateTrailerScheduleComponent_div_16_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r0.isRequired("regionId"));
} }
function CreateTrailerScheduleComponent_div_23_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateTrailerScheduleComponent_div_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateTrailerScheduleComponent_div_23_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r1.isRequired("trailerId"));
} }
const _c0 = function (a0) { return { "is-invalid": a0 }; };
function CreateTrailerScheduleComponent_div_24_Template(rf, ctx) { if (rf & 1) {
    const _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "label", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Shift:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "ng-multiselect-dropdown", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "label", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](11, "Column:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](12, "ng-multiselect-dropdown", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "a", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_div_24_Template_a_click_14_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r15); const j_r13 = ctx.index; const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r14.removeShift(j_r13); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](15, "i", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const j_r13 = ctx.index;
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroupName", j_r13);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](9, _c0, ctx_r2.TrailerScheduleForm.get("shifts").touched && ctx_r2.TrailerScheduleForm.get("shifts").invalid))("placeholder", "Select Shifts")("settings", ctx_r2.multiselectDropDownSettingsById)("data", ctx_r2.ShiftList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](11, _c0, ctx_r2.TrailerScheduleForm.get("shifts").touched && ctx_r2.TrailerScheduleForm.get("shifts").invalid))("placeholder", "Select Columns")("settings", ctx_r2.multiselectDropDownSettingsById)("data", ctx_r2.ColumnList);
} }
function CreateTrailerScheduleComponent_div_43_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateTrailerScheduleComponent_div_43_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateTrailerScheduleComponent_div_43_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.isRequired("fromDate"));
} }
function CreateTrailerScheduleComponent_div_50_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateTrailerScheduleComponent_div_50_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateTrailerScheduleComponent_div_50_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.isRequired("toDate"));
} }
function CreateTrailerScheduleComponent_div_66_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "input", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Custom");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateTrailerScheduleComponent_div_69_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, " Dates:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "ng-multiselect-dropdown", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Date (s)")("settings", ctx_r8.multiselectDateSettingsById)("data", ctx_r8.DateList);
} }
function CreateTrailerScheduleComponent_div_70_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Days:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "input", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
class CreateTrailerScheduleComponent {
    constructor(regionService, driverService, _fb) {
        this.regionService = regionService;
        this.driverService = driverService;
        this._fb = _fb;
        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.regionList = [];
        this.ShiftList = [];
        this.TrailerList = [];
        this.DriverList = [];
        this.ColumnList = [];
        this.isLoading = false;
        this.SelectedRegionId = '';
        //sidebar variables
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.DateList = [];
        //min max date
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
    }
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
        };
        this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
        };
    }
    ngAfterViewInit() {
        this.isLoading = false;
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }
    isInvalid(name) {
        var result = this.TrailerScheduleForm.get(name).invalid && (this.TrailerScheduleForm.get(name).dirty || this.TrailerScheduleForm.get(name).touched);
        return result;
    }
    isRequired(name) {
        return this.TrailerScheduleForm.get(name).errors.required;
    }
    getRegions() {
        this.regionService.getRegions()
            .subscribe((region) => {
            this.getRegionDropDwn(region.Regions);
        });
    }
    closedSideBar() { this._opened = false; }
    onRegionSelect($event) {
        var region = this.regionList.find(f => f.Id == $event.Id);
        //this.TrailerScheduleForm.controls.shiftId.setValue('');
        this.ShiftList = region.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` }; });
        this.TrailerList = region.Trailers.map(res => { return { Id: res.Code, Name: `${res.Name}` }; });
        this.SelectedRegionId = region.Id;
        this.createColumnList(region);
    }
    createColumnList(region) {
        this.DriverList = region.Drivers;
        if (region.Drivers.length > 0) {
            var num = 1;
            this.DriverList.forEach(obj => {
                var col = {
                    Id: 0,
                    Name: ""
                };
                col.Id = num;
                col.Name = "C" + num;
                this.ColumnList.push(col);
                num++;
            });
        }
    }
    getRegionDropDwn(regionList) {
        this.regionList = regionList;
    }
    onRegionDeSelect($event) {
        //var region = this.regionList.find(f => f.Id == $event.Id);
        //this.ShiftList = [];
        //this.TrailerScheduleForm.controls.trailerId.setValue('');
    }
    createTrailerForm() {
        this.TrailerScheduleForm = this._fb.group({
            id: this._fb.control(null),
            regionId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            trailerId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            shifts: this._fb.array([this.getShift()]),
            fromDate: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            toDate: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            //type: this._fb.control(''),
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]],
        });
        var dt = moment__WEBPACK_IMPORTED_MODULE_3__(new Date()).toDate();
        this.TrailerScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'));
    }
    addShift() {
        let _shifts = this.TrailerScheduleForm.get('shifts');
        _shifts.push(this.getShift());
    }
    getShift() {
        return this._fb.group({
            shiftId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            columnId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required)
        });
    }
    removeShift(index) {
        let _shifts = this.TrailerScheduleForm.get('shifts');
        _shifts.removeAt(index);
    }
    setFromDate(event) {
        this.TrailerScheduleForm.controls.fromDate.setValue(event);
        //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        let d = moment__WEBPACK_IMPORTED_MODULE_3__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
        !this.TrailerScheduleForm.controls.toDate.value ? this.TrailerScheduleForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(d).format('MM/DD/YYYY')) : '';
        if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
            let _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.fromDate.value).toDate();
            let _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.toDate.value).toDate();
            if (_toDate < _fromDate) {
                this.TrailerScheduleForm.controls.toDate.setValue(event);
            }
        }
        this.InitializeDates();
    }
    setToDate(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TrailerScheduleForm.controls.toDate.setValue(event);
            if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
                let _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.fromDate.value).toDate();
                let _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.toDate.value).toDate();
                if (_fromDate > _toDate) {
                    this.TrailerScheduleForm.controls.fromDate.setValue(event);
                }
            }
            this.InitializeDates();
        });
    }
    InitializeDates(type, repeat) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TrailerScheduleForm.controls.customDates.setValue([]);
            this.DateList = [];
            !repeat ? repeat = 0 : '';
            var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            if (this.TrailerScheduleForm.controls.fromDate.value && this.TrailerScheduleForm.controls.toDate.value) {
                for (var dt = new Date(this.TrailerScheduleForm.controls.fromDate.value); dt <= new Date(this.TrailerScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                    if (type && type == 2) //weekend
                        (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                    else
                        this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
                }
                return this.DateList;
            }
        });
    }
    onSubmit() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (this.TrailerScheduleForm.invalid) {
                this.TrailerScheduleForm.markAllAsTouched();
                return false;
            }
            else if (this.TrailerScheduleForm.controls.type.value == '3') {
                if (!(this.TrailerScheduleForm.controls.repeat.value && this.TrailerScheduleForm.controls.repeat.value > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return false;
                }
            }
            else if (this.TrailerScheduleForm.controls.type.value == '4') {
                if (!(this.TrailerScheduleForm.controls.customDates.value.length > 0)) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return false;
                }
            }
            if (this.TrailerScheduleForm.controls.type.value == '2') {
                this.TrailerScheduleForm.controls.customDates.setValue(yield this.InitializeDates(this.TrailerScheduleForm.controls.type.value));
            }
            else if (this.TrailerScheduleForm.controls.type.value == '3') {
                this.TrailerScheduleForm.controls.customDates.setValue(yield this.InitializeDates(this.TrailerScheduleForm.controls.type.value, this.TrailerScheduleForm.controls.repeat.value));
            }
            else if (this.TrailerScheduleForm.controls.type.value == '1') {
                this.TrailerScheduleForm.controls.customDates.setValue(yield this.InitializeDates());
            }
            let dates = [];
            yield this.TrailerScheduleForm.controls.customDates.value.map(res => { dates.push(res.Id); });
            let model = new src_app_driver_models_TrailerSchedule__WEBPACK_IMPORTED_MODULE_5__["TrailerSchedule"]();
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
        });
    }
    addTrailerSchedule(model) {
        this.isLoading = true;
        this.driverService.onLoadingChanged.next(true);
        this.driverService.addTrailerSchedule(model)
            .subscribe((response) => {
            if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess('Trailer Schedule created successfully', undefined, undefined);
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
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
        });
    }
}
CreateTrailerScheduleComponent.??fac = function CreateTrailerScheduleComponent_Factory(t) { return new (t || CreateTrailerScheduleComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"])); };
CreateTrailerScheduleComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: CreateTrailerScheduleComponent, selectors: [["app-create-trailer-schedule"]], outputs: { OnScheduleAdded: "OnScheduleAdded" }, decls: 75, vars: 24, consts: [["id", "trailerSchedule", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus", "mr5"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["name", "TrailerScheduleForm", "autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "regionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Trailer"], ["formControlName", "trailerId", 3, "placeholder", "settings", "data"], ["class", "row", "formArrayName", "shifts", 4, "ngFor", "ngForOf"], [1, "col-sm-12"], ["width", "100%"], ["width", "15%"], ["colspan", "2"], [1, "fa", "fa-plus-circle", "fs14"], ["for", "fromDate"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], [1, "color-maroon"], [4, "ngIf"], ["formArrayName", "shifts", 1, "row"], [1, "row", 3, "formGroupName"], [1, "col-sm-5"], ["for", "Shift"], ["formControlName", "shiftId", 3, "ngClass", "placeholder", "settings", "data"], ["for", "Column"], ["formControlName", "columnId", 3, "ngClass", "placeholder", "settings", "data"], [1, "col-sm-2", "text-right"], [1, "ml20", 3, "click"], [1, "fa", "fa-trash-alt", "mt14", "color-maroon", "mt8"], ["type", "radio", "name", "type", "formControlName", "type", "value", "4", "id", "inlineRadioCustom", 1, "form-check-input"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], ["formControlName", "customDates", 3, "placeholder", "settings", "data"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]], template: function CreateTrailerScheduleComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_Template_button_click_0_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Add Trailer");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "ng-sidebar-container", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "ng-sidebar", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("openedChange", function CreateTrailerScheduleComponent_Template_ng_sidebar_openedChange_4_listener($event) { return ctx._opened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_Template_a_click_5_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](6, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "h3", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Schedule Trailer ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "content", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("keydown.enter", function CreateTrailerScheduleComponent_Template_content_keydown_enter_9_listener($event) { return $event.preventDefault(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Region:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateTrailerScheduleComponent_Template_ng_multiselect_dropdown_onSelect_15_listener($event) { return ctx.onRegionSelect($event); })("onDeSelect", function CreateTrailerScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_15_listener($event) { return ctx.onRegionDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, CreateTrailerScheduleComponent_div_16_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21, "Trailer:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](22, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, CreateTrailerScheduleComponent_div_23_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, CreateTrailerScheduleComponent_div_24_Template, 16, 13, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "table", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "\u00A0");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_Template_a_click_33_listener() { return ctx.addShift(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](34, "i", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](35, " Add Shift ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "label", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "From Date:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "input", 23, 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateTrailerScheduleComponent_Template_input_onDateChange_41_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](43, CreateTrailerScheduleComponent_div_43_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "label", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](47, "To Date:");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "input", 25, 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateTrailerScheduleComponent_Template_input_onDateChange_48_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](50, CreateTrailerScheduleComponent_div_50_Template, 2, 1, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](55, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "label", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](57, "Daily");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](59, "input", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "label", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](61, "WeekDays (Mon to Fri)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](63, "input", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "label", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](65, "Repeat Every");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](66, CreateTrailerScheduleComponent_div_66_Template, 4, 0, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](69, CreateTrailerScheduleComponent_div_69_Template, 4, 3, "div", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](70, CreateTrailerScheduleComponent_div_70_Template, 4, 0, "div", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](71, "div", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](72, "input", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_Template_input_click_72_listener() { return ctx.closedSideBar(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "button", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateTrailerScheduleComponent_Template_button_click_73_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](74, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.TrailerScheduleForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Region(s)")("settings", ctx.multiselectDropDownSettingsById)("data", ctx.regionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("regionId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Trailer(s)")("settings", ctx.multiselectDropDownSettingsById)("data", ctx.TrailerList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("trailerId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.TrailerScheduleForm.get("shifts")["controls"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("fromDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isInvalid("toDate"));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.TrailerScheduleForm.get("fromDate").value && ctx.TrailerScheduleForm.get("toDate").value);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.TrailerScheduleForm.get("type").value == "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.TrailerScheduleForm.get("type").value == "3");
    } }, directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtdHJhaWxlci1zY2hlZHVsZS9jcmVhdGUtdHJhaWxlci1zY2hlZHVsZS5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CreateTrailerScheduleComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-create-trailer-schedule',
                templateUrl: './create-trailer-schedule.component.html',
                styleUrls: ['./create-trailer-schedule.component.css']
            }]
    }], function () { return [{ type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"] }, { type: _services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }]; }, { OnScheduleAdded: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/driver/driver-management/create-driver/create-driver.component.ts":
/*!***********************************************************************************!*\
  !*** ./src/app/driver/driver-management/create-driver/create-driver.component.ts ***!
  \***********************************************************************************/
/*! exports provided: CreateDriverComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateDriverComponent", function() { return CreateDriverComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _services_driver_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../services/driver.service */ "./src/app/driver/services/driver.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");










function CreateDriverComponent_div_14_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_14_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r0.DriverForm.get("FirstName").errors.required);
} }
function CreateDriverComponent_div_22_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_22_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.DriverForm.get("LastName").errors.required);
} }
function CreateDriverComponent_div_36_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_36_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Invalid email. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_36_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_36_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](2, CreateDriverComponent_div_36_div_2_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r2.DriverForm.get("Email").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r2.DriverForm.get("Email").errors.pattern);
} }
function CreateDriverComponent_div_43_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", ctx_r13.ContactNumberValidationMessage, " ");
} }
function CreateDriverComponent_div_43_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_43_div_1_Template, 2, 1, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r3.DriverForm.get("ContactNumber").valid || (ctx_r3.DriverForm.get("ContactNumber").errors == null ? null : ctx_r3.DriverForm.get("ContactNumber").errors.pattern));
} }
function CreateDriverComponent_div_51_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_51_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_51_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r4.DriverForm.get("ExpiryDate").errors.required);
} }
function CreateDriverComponent_div_60_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_60_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_60_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.DriverForm.get("LicenseNumber").errors.required);
} }
function CreateDriverComponent_option_70_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const key_r16 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", key_r16.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](key_r16.Name);
} }
function CreateDriverComponent_div_71_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateDriverComponent_div_71_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateDriverComponent_div_71_div_1_Template, 2, 0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r7.DriverForm.get("SelectedLicenseTypes").errors.required);
} }
function CreateDriverComponent_div_95_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
const _c0 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
class CreateDriverComponent {
    constructor(fb, driverService) {
        this.fb = fb;
        this.driverService = driverService;
        this.MinDate = new Date();
        this.MaxDate = new Date();
        this.TrailerTypeEnum = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["TrailerType"];
        this.TrailerTypeList = [];
        this.LicenceTypeEnum = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["LicenceRequirement"];
        this.LicenceTypes = [];
        //public DriverStatusEnum: typeof Status = Status;
        //public Statuses: DropdownItem[] = [];
        this.RegionList = [];
        this.ContactNumberValidationMessage = "Invalid Contact number";
        this.IsOnboarded = false;
        this.IsLoading = false;
        this.trailerDdlSettings = {};
        this.regionDdlSettings = {};
        this.onSaveDriverData = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.DriverForm = this.fb.group({
            DriverId: this.fb.control(0),
            FirstName: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            LastName: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            CompanyName: this.fb.control(''),
            Email: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]),
            LicenseNumber: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            ExpiryDate: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            LicenseTypeId: this.fb.control(null),
            SelectedLicenseTypes: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            TrailerType: this.fb.control(null),
            SelectedTrailerTypes: this.fb.control(null),
            ContactNumber: this.fb.control(''),
            //DriverStatus: this.fb.control(Status.Active),
            InvitedBy: this.fb.control(''),
            UserId: this.fb.control(0),
            //IsActive: this.fb.control(true),
            Regions: this.fb.control(null),
            SelectedRegions: this.fb.control(null),
            IsFilldAuthorized: this.fb.control(false),
        });
    }
    ngOnInit() {
        this.trailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.regionDdlSettings = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        // load regions
        this.getRegions();
        this.TrailerTypeList = Object.keys(this.TrailerTypeEnum).filter(k => typeof this.TrailerTypeEnum[k] === "number").map(x => { return { Id: this.TrailerTypeEnum[x], Name: x, Code: "" }; });
        this.LicenceTypes = Object.keys(this.LicenceTypeEnum).filter(k => typeof this.LicenceTypeEnum[k] === "number").map(x => { return { Id: x, Name: x == "Class1" ? "Class 1" : x == "Class3" ? "Class 3" : x, Code: "" }; });
        //this.Statuses = (Object.keys(this.DriverStatusEnum).filter(k => typeof this.DriverStatusEnum[k] === "number") as string[]).map(x => { return { Id: this.DriverStatusEnum[x], Name: x == "InActive" ? "In-Active" : x, Code: "" } as DropdownItem });
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
    }
    onSubmit() {
        // validate contact number     
        var contactNumber = this.DriverForm.get('ContactNumber').value;
        var licenseType = this.DriverForm.get('SelectedLicenseTypes').value;
        this.formatContactNumber(contactNumber);
        this.onLicenseTypeChange(licenseType);
        if (this.DriverForm.valid) {
            var driverForm = this.DriverForm.value;
            var driverId = (driverForm.DriverId == "" || driverForm.DriverId == null) ? 0 : driverForm.DriverId;
            this.DriverForm.get("DriverId").patchValue(driverId);
            var userId = (driverForm.UserId == "" || driverForm.UserId == null) ? 0 : driverForm.UserId;
            this.DriverForm.get("UserId").patchValue(userId);
            if (driverForm.SelectedTrailerTypes != null && driverForm.SelectedTrailerTypes.length > 0) {
                var trailerTypeIds = [];
                driverForm.SelectedTrailerTypes.forEach(t => { trailerTypeIds.push(this.TrailerTypeEnum[t.Name]); });
                this.DriverForm.get("TrailerType").patchValue(trailerTypeIds);
            }
            if (driverForm.SelectedRegions != null && driverForm.SelectedRegions.length > 0) {
                var regionIds = [];
                driverForm.SelectedRegions.forEach(t => { regionIds.push(t.Code); });
                this.DriverForm.get("Regions").patchValue(regionIds);
            }
            if (driverForm.SelectedLicenseTypes != null) {
                this.DriverForm.get("LicenseTypeId").patchValue(driverForm.SelectedLicenseTypes);
            }
            //if (driverForm.IsActive == undefined || driverForm.IsActive == null) {
            //    var isActive = driverForm.DriverStatus == 1 ? true : false;
            //    this.DriverForm.get("IsActive").patchValue(isActive);
            //}
            if (!driverForm.IsFilldAuthorized) {
                this.DriverForm.get("IsFilldAuthorized").patchValue(false);
            }
            this.submitForm();
        }
        else {
            this.DriverForm.markAllAsTouched();
        }
    }
    loadDriverDetail(driver) {
        this.clearForm();
        if (driver.ContactNumber == 'NA') {
            driver.ContactNumber = null;
        }
        this.DriverForm.patchValue(driver);
        if (driver.UserId && driver.UserId > 0) {
            this.DriverForm.get("FirstName").disable();
            this.DriverForm.get("LastName").disable();
            this.DriverForm.get("Email").disable();
        }
        if (driver.TrailerType != null) {
            var trailerTypesToBind = this.TrailerTypeList.filter(t => driver.TrailerType.indexOf(t.Id) != -1);
            this.DriverForm.controls.SelectedTrailerTypes.setValue(trailerTypesToBind);
        }
        if (driver.Regions != null) {
            var regionsToBind = this.RegionList.filter(t => driver.Regions.indexOf(t.Code) != -1);
            this.DriverForm.controls.SelectedRegions.setValue(regionsToBind);
        }
        if (driver.LicenseTypeId != null)
            this.DriverForm.controls.SelectedLicenseTypes.setValue(driver.LicenseTypeId);
    }
    submitForm() {
        this.IsLoading = true;
        this.driverService.postAddDriver(this.DriverForm.getRawValue()).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                closeSlidePanel();
                this.clearForm();
                this.onSaveDriverData.emit();
            }
            else if (data.StatusCode == 2) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }
    getRegions() {
        this.IsLoading = true;
        this.driverService.getRegions().subscribe(data => {
            this.IsLoading = false;
            this.RegionList = data;
        });
    }
    clearForm() {
        this.DriverForm.reset();
        this.setDefaultValue();
        this.DriverForm.get("FirstName").enable();
        this.DriverForm.get("LastName").enable();
        this.DriverForm.get("Email").enable();
    }
    setDefaultValue() {
        // this.DriverForm.get("DriverStatus").patchValue(Status.Active);
        this.DriverForm.get("Regions").patchValue(null);
        this.DriverForm.get("TrailerType").patchValue(null);
        this.IsOnboarded = false;
    }
    setSelectedDate(date) {
        var _date = this.DriverForm.get('ExpiryDate');
        if (_date.value != date) {
            _date.setValue(date);
        }
    }
    onLicenseTypeChange(licenseType) {
        if (licenseType == null || licenseType == "null")
            this.DriverForm.controls['SelectedLicenseTypes'].setErrors({ 'required': true });
        else
            this.DriverForm.controls['SelectedLicenseTypes'].setErrors(null);
    }
    formatContactNumber(contactNumber) {
        if (contactNumber != null && contactNumber != '') {
            contactNumber = contactNumber.split('-').join("");
            if (contactNumber.length == 10) {
                contactNumber = contactNumber.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
                this.DriverForm.controls['ContactNumber'].setErrors(null);
                this.DriverForm.get("ContactNumber").patchValue(contactNumber);
            }
            else {
                this.DriverForm.controls['ContactNumber'].setErrors({ 'incorrect': true });
            }
        }
        else {
            this.DriverForm.controls['ContactNumber'].setErrors(null);
        }
    }
}
CreateDriverComponent.??fac = function CreateDriverComponent_Factory(t) { return new (t || CreateDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_4__["DriverService"])); };
CreateDriverComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: CreateDriverComponent, selectors: [["app-create-driver"]], outputs: { onSaveDriverData: "onSaveDriverData" }, decls: 96, vars: 23, consts: [[3, "formGroup", "ngSubmit"], [1, "sidePanel_overflow"], ["id", "driver-Form", 1, "col-sm-12"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["type", "hidden", "formControlName", "DriverId"], ["type", "hidden", "formControlName", "InvitedBy"], ["type", "hidden", "formControlName", "UserId"], [1, "color-maroon"], ["type", "text", "formControlName", "FirstName", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], ["type", "text", "formControlName", "LastName", 1, "form-control"], ["type", "text", "formControlName", "CompanyName", 1, "form-control"], ["type", "text", "formControlName", "Email", 1, "form-control", 3, "ngClass"], ["type", "text", "formControlName", "ContactNumber", 1, "form-control", 3, "change"], ["type", "text", "formControlName", "ExpiryDate", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "formControlName", "LicenseNumber", 1, "form-control"], ["formControlName", "SelectedLicenseTypes", 1, "form-control"], [3, "value"], [3, "value", 4, "ngFor", "ngForOf"], ["formControlName", "SelectedTrailerTypes", 3, "placeholder", "settings", "data"], ["formControlName", "SelectedRegions", 3, "placeholder", "settings", "data"], [1, "form-check", "form-check-inline"], ["type", "checkbox", "id", "IsFilldAuthorized", "formControlName", "IsFilldAuthorized", 1, "form-check-input"], ["for", "IsFilldAuthorized", 1, "form-check-label"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["id", "submit-driver-form", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateDriverComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngSubmit", function CreateDriverComponent_Template_form_ngSubmit_0_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "First Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](13, "input", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](14, CreateDriverComponent_div_14_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Last Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](21, "input", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](22, CreateDriverComponent_div_22_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](27, "Company Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](28, "input", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "E-mail");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](35, "input", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](36, CreateDriverComponent_div_36_Template, 3, 2, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](38, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](41, "Contact Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "input", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function CreateDriverComponent_Template_input_change_42_listener($event) { return ctx.formatContactNumber($event.target.value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](43, CreateDriverComponent_div_43_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](47, "Licence Expiry Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](49, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "input", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDateChange", function CreateDriverComponent_Template_input_onDateChange_50_listener($event) { return ctx.setSelectedDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](51, CreateDriverComponent_div_51_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](53, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](54, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](55, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](56, "Licence Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](57, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](58, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](59, "input", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](60, CreateDriverComponent_div_60_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](61, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](62, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](63, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](64, "Licence Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](65, "span", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](66, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](67, "select", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](68, "option", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](69, "Select");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](70, CreateDriverComponent_option_70_Template, 2, 2, "option", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](71, CreateDriverComponent_div_71_Template, 2, 1, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](72, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](73, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](74, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](75, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](76, "Trailer Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](77, "ng-multiselect-dropdown", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](78, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](79, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](80, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](81, "Region");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](82, "ng-multiselect-dropdown", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](83, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](84, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](85, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](86, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](87, "input", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](88, "label", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](89, "TrueFill Inc. Compatible");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](90, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](91, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](92, "input", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CreateDriverComponent_Template_input_click_92_listener() { return ctx.clearForm(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](93, "button", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](94, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](95, CreateDriverComponent_div_95_Template, 5, 0, "div", 29);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.DriverForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("FirstName").invalid && ctx.DriverForm.get("FirstName").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("LastName").invalid && ctx.DriverForm.get("LastName").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](21, _c0, ctx.IsOnboarded));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("Email").invalid && ctx.DriverForm.get("Email").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("ContactNumber").invalid && ctx.DriverForm.get("ContactNumber").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinDate)("maxDate", ctx.MaxDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("ExpiryDate").invalid && ctx.DriverForm.get("ExpiryDate").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("LicenseNumber").invalid && ctx.DriverForm.get("LicenseNumber").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.LicenceTypes);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.DriverForm.get("SelectedLicenseTypes").invalid && ctx.DriverForm.get("SelectedLicenseTypes").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Type")("settings", ctx.trailerDdlSettings)("data", ctx.TrailerTypeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Region")("settings", ctx.regionDdlSettings)("data", ctx.RegionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgClass"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC9jcmVhdGUtZHJpdmVyL2NyZWF0ZS1kcml2ZXIuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](CreateDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-create-driver',
                templateUrl: './create-driver.component.html',
                styleUrls: ['./create-driver.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_driver_service__WEBPACK_IMPORTED_MODULE_4__["DriverService"] }]; }, { onSaveDriverData: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/driver/driver-management/driver-management.component.ts":
/*!*************************************************************************!*\
  !*** ./src/app/driver/driver-management/driver-management.component.ts ***!
  \*************************************************************************/
/*! exports provided: DriverManagementComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverManagementComponent", function() { return DriverManagementComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./view-driver/view-driver.component */ "./src/app/driver/driver-management/view-driver/view-driver.component.ts");



class DriverManagementComponent {
    constructor() { }
    ngOnInit() {
    }
}
DriverManagementComponent.??fac = function DriverManagementComponent_Factory(t) { return new (t || DriverManagementComponent)(); };
DriverManagementComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: DriverManagementComponent, selectors: [["app-driver-management"]], decls: 3, vars: 0, consts: [["viewDriver", ""]], template: function DriverManagementComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "app-view-driver", null, 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } }, directives: [_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_1__["ViewDriverComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC9kcml2ZXItbWFuYWdlbWVudC5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DriverManagementComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-driver-management',
                templateUrl: './driver-management.component.html',
                styleUrls: ['./driver-management.component.css']
            }]
    }], function () { return []; }, null); })();


/***/ }),

/***/ "./src/app/driver/driver-management/view-driver/view-driver.component.ts":
/*!*******************************************************************************!*\
  !*** ./src/app/driver/driver-management/view-driver/view-driver.component.ts ***!
  \*******************************************************************************/
/*! exports provided: ViewDriverComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewDriverComponent", function() { return ViewDriverComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../create-driver/create-driver.component */ "./src/app/driver/driver-management/create-driver/create-driver.component.ts");
/* harmony import */ var _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../models/DriverManagementModel */ "./src/app/driver/models/DriverManagementModel.ts");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _services_driver_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../services/driver.service */ "./src/app/driver/services/driver.service.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! angular-confirmation-popover */ "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");















function ViewDriverComponent_tr_40_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.ContactNumber);
} }
function ViewDriverComponent_tr_40_span_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.CompanyName);
} }
function ViewDriverComponent_tr_40_span_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.LicenseNumber);
} }
function ViewDriverComponent_tr_40_span_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.DisplayLicenseType);
} }
function ViewDriverComponent_tr_40_span_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.ExpiryDate);
} }
function ViewDriverComponent_tr_40_span_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.DisplayTrailerTypes);
} }
function ViewDriverComponent_tr_40_span_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_span_26_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.IsFilldAuthorized ? "Yes" : "No");
} }
function ViewDriverComponent_tr_40_span_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "No");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_40_Template(rf, ctx) { if (rf & 1) {
    const _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "a", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_tr_40_Template_a_click_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const driver_r7 = ctx.$implicit; const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); ctx_r29.setPanelHeader("Edit Driver"); return ctx_r29.editDriver(driver_r7, false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, ViewDriverComponent_tr_40_span_8_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, ViewDriverComponent_tr_40_span_9_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, ViewDriverComponent_tr_40_span_11_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, ViewDriverComponent_tr_40_span_12_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, ViewDriverComponent_tr_40_span_14_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, ViewDriverComponent_tr_40_span_15_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, ViewDriverComponent_tr_40_span_17_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, ViewDriverComponent_tr_40_span_18_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, ViewDriverComponent_tr_40_span_20_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, ViewDriverComponent_tr_40_span_21_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, ViewDriverComponent_tr_40_span_23_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, ViewDriverComponent_tr_40_span_24_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](26, ViewDriverComponent_tr_40_span_26_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, ViewDriverComponent_tr_40_span_27_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "td", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "button", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_tr_40_Template_button_click_29_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const driver_r7 = ctx.$implicit; const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r31.showDriverShifts(driver_r7); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](30, "i", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "button", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_tr_40_Template_button_click_32_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const driver_r7 = ctx.$implicit; const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r32.setdeleteDriver(driver_r7); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](33, "i", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r7 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"]("", driver_r7.FirstName, " ", driver_r7.LastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r7.Email);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.ContactNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.ContactNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.CompanyName != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.CompanyName == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.LicenseNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.LicenseNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.DisplayLicenseType != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.DisplayLicenseType == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.ExpiryDate != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.ExpiryDate == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.DisplayTrailerTypes != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.DisplayTrailerTypes == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.IsFilldAuthorized != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r7.IsFilldAuthorized == null);
} }
function ViewDriverComponent_tr_76_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.ContactNumber);
} }
function ViewDriverComponent_tr_76_span_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.CompanyName);
} }
function ViewDriverComponent_tr_76_span_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.LicenseNumber);
} }
function ViewDriverComponent_tr_76_span_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.DisplayLicenseType);
} }
function ViewDriverComponent_tr_76_span_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.ExpiryDate);
} }
function ViewDriverComponent_tr_76_span_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.DisplayTrailerTypes);
} }
function ViewDriverComponent_tr_76_span_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_span_26_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.IsFilldAuthorized ? "Yes" : "No");
} }
function ViewDriverComponent_tr_76_span_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "No");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_tr_76_Template(rf, ctx) { if (rf & 1) {
    const _r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "a", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_tr_76_Template_a_click_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r56); const driver_r33 = ctx.$implicit; const ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); ctx_r55.setPanelHeader("Edit Driver"); return ctx_r55.editDriver(driver_r33, true); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, ViewDriverComponent_tr_76_span_8_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, ViewDriverComponent_tr_76_span_9_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, ViewDriverComponent_tr_76_span_11_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, ViewDriverComponent_tr_76_span_12_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, ViewDriverComponent_tr_76_span_14_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, ViewDriverComponent_tr_76_span_15_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, ViewDriverComponent_tr_76_span_17_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, ViewDriverComponent_tr_76_span_18_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, ViewDriverComponent_tr_76_span_20_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, ViewDriverComponent_tr_76_span_21_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, ViewDriverComponent_tr_76_span_23_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, ViewDriverComponent_tr_76_span_24_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](26, ViewDriverComponent_tr_76_span_26_Template, 2, 1, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, ViewDriverComponent_tr_76_span_27_Template, 2, 0, "span", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "td", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "button", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_tr_76_Template_button_click_29_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r56); const driver_r33 = ctx.$implicit; const ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r57.showDriverShifts(driver_r33); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](30, "i", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "input", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("confirm", function ViewDriverComponent_tr_76_Template_input_confirm_32_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r56); const driver_r33 = ctx.$implicit; const ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r58.changeDriverStatus(driver_r33); })("ngModelChange", function ViewDriverComponent_tr_76_Template_input_ngModelChange_32_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r56); const driver_r33 = ctx.$implicit; return driver_r33.IsActive = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const driver_r33 = ctx.$implicit;
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"]("", driver_r33.FirstName, " ", driver_r33.LastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r33.Email);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.ContactNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.ContactNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.CompanyName != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.CompanyName == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.LicenseNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.LicenseNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.DisplayLicenseType != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.DisplayLicenseType == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.ExpiryDate != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.ExpiryDate == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.DisplayTrailerTypes != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.DisplayTrailerTypes == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.IsFilldAuthorized != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r33.IsFilldAuthorized == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", driver_r33.IsActive)("popoverTitle", ctx_r1.popoverTitle)("confirmText", ctx_r1.confirmButtonText)("cancelText", ctx_r1.cancelButtonText);
} }
function ViewDriverComponent_div_87_tr_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const shift_r62 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"]("", shift_r62.ShiftFrom, " - ", shift_r62.ShiftTo, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](shift_r62.FromDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](shift_r62.ToDate);
} }
function ViewDriverComponent_div_87_tr_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "No schedule found");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_div_87_Template(rf, ctx) { if (rf & 1) {
    const _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h4", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "button", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_div_87_Template_button_click_6_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r63.closeDriverShiftsPopup(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "\u00D7");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "table", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13, "Shift");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "From Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "To Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, ViewDriverComponent_div_87_tr_19_Template, 7, 4, "tr", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, ViewDriverComponent_div_87_tr_20_Template, 3, 0, "tr", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "button", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_div_87_Template_button_click_22_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r65.closeDriverShiftsPopup(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Close");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx_r3.ShiftInfo.DriverName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r3.ShiftInfo.Shifts);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r3.ShiftInfo.Shifts == null || ctx_r3.ShiftInfo.Shifts.length == 0);
} }
function ViewDriverComponent_div_88_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_div_93_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 81);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewDriverComponent_p_98_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "p", 83);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "Driver schedule exists.");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
class ViewDriverComponent {
    constructor(driverService, regionService) {
        this.driverService = driverService;
        this.regionService = regionService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtOptionsOnboarded = {};
        this.dtTriggerOnboarded = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.InvitedDrivers = [];
        this.OnboardedDrivers = [];
        this.IsLoading = false;
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
        this.IsShowShiftInfoPopup = false;
        this.IsLoadingdriverDelete = false;
        this.IsScheduleExists = false;
        this.DriverShiftDetailList = [];
    }
    ngOnInit() {
        this.HeaderText = 'Create Driver';
        this.initializeInvitedDrivers();
        this.initializeOnboardedDrivers();
        this.getAllDrivers();
    }
    initializeInvitedDrivers() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Driver Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Driver Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    initializeOnboardedDrivers() {
        let exportOnboardedColumns = { columns: ':visible' };
        this.dtOptionsOnboarded = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportOnboardedColumns },
                { extend: 'csv', title: 'Driver Details', exportOptions: exportOnboardedColumns },
                { extend: 'pdf', title: 'Driver Details', orientation: 'landscape', exportOptions: exportOnboardedColumns },
                { extend: 'print', exportOptions: exportOnboardedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    getAllDrivers() {
        this.IsLoading = true;
        this.driverService.getAllDrivers().subscribe(data => {
            this.IsLoading = false;
            this.InvitedDrivers = data.InvitedDrivers;
            this.OnboardedDrivers = data.OnboardedDrivers;
            this.dtTrigger.next();
            this.dtTriggerOnboarded.next();
        });
    }
    editDriver(driver, isOnboarded) {
        if (this.CreateDriver != undefined) {
            this.CreateDriver.IsOnboarded = isOnboarded;
            this.CreateDriver.loadDriverDetail(driver);
        }
    }
    deleteDriver() {
        if (this.setDeleteDriverInfo != null) {
            this.IsLoadingdriverDelete = true;
            this.IsLoading = true;
            this.driverService.postDeleteDriver(this.setDeleteDriverInfo).subscribe(data => {
                this.IsLoading = false;
                this.IsLoadingdriverDelete = false;
                this.getDriverDetails();
                if (data.StatusCode == 0) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                }
                else if (data.StatusCode == 2) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
                }
                else {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
                $("#btnDriverCancel").click();
            });
        }
    }
    changeDriverStatus(driver) {
        var isActive = driver.IsActive;
        var userId = driver.UserId;
        this.IsLoading = true;
        this.driverService.changeDriverStatus(userId, isActive).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }
    showDriverShifts(driver) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var driverIds = driver.DriverId.toString();
            this.IsShowShiftInfoPopup = true;
            this.IsLoading = true;
            this.regionService.getShiftByDrivers(driverIds, 0).subscribe(data => {
                this.IsLoading = false;
                if (data != null && data.Result) {
                    this.ShiftInfo.DriverName = driver.FirstName + " " + driver.LastName;
                    this.DriverShiftDetailList = data.Result;
                    this.setShiftInfo();
                }
                else {
                    this.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
                }
            });
        });
    }
    closeDriverShiftsPopup() {
        this.IsShowShiftInfoPopup = false;
        this.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
    }
    clearPanelControls() {
        if (this.CreateDriver != undefined) {
            this.CreateDriver.clearForm();
        }
    }
    setPanelHeader(headerText) {
        this.HeaderText = headerText;
    }
    getDriverDetails() {
        this.getAllDrivers();
        $("#invited-driver-grid-datatable").DataTable().clear().destroy();
        $("#onboarded-driver-grid-datatable").DataTable().clear().destroy();
    }
    setShiftInfo() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.DriverShiftDetailList.forEach((driver) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                var e_1, _a;
                try {
                    for (var _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driver.ScheduleList), _c; _c = yield _b.next(), !_c.done;) {
                        let shift = _c.value;
                        var shiftDetail = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["ShiftDetailModel"]();
                        shiftDetail.FromDate = moment__WEBPACK_IMPORTED_MODULE_6__(shift.StartDate).format('MM/DD/YYYY');
                        shiftDetail.ToDate = moment__WEBPACK_IMPORTED_MODULE_6__(shift.EndDate).format('MM/DD/YYYY');
                        shiftDetail.ShiftFrom = shift.ShiftDetail.StartTime;
                        shiftDetail.ShiftTo = shift.ShiftDetail.EndTime;
                        this.ShiftInfo.Shifts.push(shiftDetail);
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) yield _a.call(_b);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
            }));
        });
    }
    setdeleteDriver(driverInfo) {
        this.IsScheduleExists = false;
        this.setDeleteDriverInfo = driverInfo;
        if (driverInfo.IsScheduleExists) {
            this.IsScheduleExists = true;
        }
    }
}
ViewDriverComponent.??fac = function ViewDriverComponent_Factory(t) { return new (t || ViewDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"])); };
ViewDriverComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: ViewDriverComponent, selectors: [["app-view-driver"]], viewQuery: function ViewDriverComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.CreateDriver = _t.first);
    } }, decls: 104, vars: 11, consts: [[1, "row"], ["id", "addDriver", 1, "col-12", "col-sm-12"], [1, "pt0", "pull-left", "mb5"], ["onclick", "slidePanel('#driver-panel','35%')", 1, "fs18", "pull-left", "ml15", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt2", "pull-left"], [1, "fs14", "pull-left"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-invited-driver-grid", 1, "table-responsive"], ["id", "invited-driver-grid-datatable", "data-gridname", "17", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "InvitedDriverName"], ["data-key", "InvitedEmail"], ["data-key", "InvitedContactNumber"], ["data-key", "InvitedCompanyName"], ["data-key", "InvitedLicenseNumber"], ["data-key", "InvitedLicenseType"], ["data-key", "InvitedExpiryDate"], ["data-key", "InvitedTrailerTypes"], ["data-key", "InvitedFilldAuthorized"], ["data-key", "InvitedShiftInfo"], ["data-key", "InvitedAction"], [4, "ngFor", "ngForOf"], [1, "row", "mt15"], [1, "col-md-12", "mt10"], ["id", "div-onboarded-driver-grid", 1, "table-responsive"], ["id", "onboarded-driver-grid-datatable", "data-gridname", "22", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "OnboardedDriverName"], ["data-key", "OnboardedEmail"], ["data-key", "OnboardedContactNumber"], ["data-key", "OnboardedCompanyName"], ["data-key", "OnboardedLicenseNumber"], ["data-key", "OnboardedLicenseType"], ["data-key", "OnboardedExpiryDate"], ["data-key", "OnboardedTrailerTypes"], ["data-key", "OnboardedShiftInfo"], ["data-key", "OnboardedAction"], ["id", "driver-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel", 3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [3, "onSaveDriverData"], ["createDriver", ""], ["id", "driverShiftsModal", "class", "modal fade", "role", "dialog", 4, "ngIf"], ["class", "loader", 4, "ngIf"], ["id", "confirm-delete-driver", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade", "confirm-box"], ["role", "document", 1, "modal-dialog", "modal-sm"], [1, "modal-content"], [1, "modal-body"], ["class", "pa top0 bg-white left0 z-index5 loading-wrapper", "id", "deleteDriverLoading", 4, "ngIf"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], [1, "fas", "fa-times"], [1, "fs18", "f-bold", "mt0"], ["class", "pb10", "id", "deleteDelGrpHeading", 4, "ngIf"], [1, "text-right"], ["type", "button", 1, "btn", "btn-danger", "btn-lg", "btnDriverYes", 3, "click"], ["type", "button", "id", "btnDriverCancel", "data-dismiss", "modal", 1, "btn", "btn-primary"], ["onclick", "slidePanel('#driver-panel','30%')", 1, "btn", "btn-link", 3, "click"], [4, "ngIf"], [1, "text-center"], ["type", "button", "data-toggle", "modal", "data-target", "#driverShiftsModal", 1, "btn", "btn-link", 3, "click"], ["alt", "Shifts", "title", "Shifts", 1, "far", "fa-clock-o", "color-blue", "fs16"], ["type", "button", "data-toggle", "modal", "data-target", "#confirm-delete-driver", 1, "btn", "btn-link", 3, "click"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], ["type", "checkbox", "mwlConfirmationPopover", "", "placement", "bottom", 3, "ngModel", "popoverTitle", "confirmText", "cancelText", "confirm", "ngModelChange"], ["id", "driverShiftsModal", "role", "dialog", 1, "modal", "fade"], [1, "modal-dialog"], [1, "modal-header", "pt10", "pb5"], [1, "modal-title", "pt0"], ["type", "button", "data-dismiss", "modal", 1, "close", 3, "click"], [1, "table", "table-bordered"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], ["colspan", "3", 1, "text-center", "pa5"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], ["id", "deleteDriverLoading", 1, "pa", "top0", "bg-white", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["id", "deleteDelGrpHeading", 1, "pb10"]], template: function ViewDriverComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "h4", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Invited");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "a", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_Template_a_click_4_listener() { return ctx.setPanelHeader("Add Driver"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "i", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "span", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Add Driver");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "table", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Driver Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Email");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Contact Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, "Company Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Licence Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Licence Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "th", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Expiration Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Trailer Type(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "TrueFill Inc.Compatible");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "Shift Info");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](40, ViewDriverComponent_tr_40_Template, 34, 17, "tr", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "h4", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Onboarded");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "table", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "th", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](54, "Driver Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "th", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](56, "Email");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "th", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](58, "Contact Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "th", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](60, "Company Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "th", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](62, "Licence Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "th", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](64, "Licence Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "th", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](66, "Expiration Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "th", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](68, "Trailer Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](69, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](70, "TrueFill Inc.Compatible");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](71, "th", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](72, "Shift Info");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](74, "Active");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](75, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](76, ViewDriverComponent_tr_76_Template, 33, 21, "tr", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](77, "div", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](78, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](79, "div", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](80, "a", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_Template_a_click_80_listener() { return ctx.clearPanelControls(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](81, "i", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](82, "h3", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](83);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](84, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](85, "app-create-driver", 44, 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSaveDriverData", function ViewDriverComponent_Template_app_create_driver_onSaveDriverData_85_listener() { return ctx.getDriverDetails(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](87, ViewDriverComponent_div_87_Template, 24, 3, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](88, ViewDriverComponent_div_88_Template, 5, 0, "div", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](89, "div", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](90, "div", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](91, "div", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](92, "div", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](93, ViewDriverComponent_div_93_Template, 2, 0, "div", 52);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](94, "button", 53);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](95, "i", 54);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](96, "h2", 55);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](97, "Are you sure?");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](98, ViewDriverComponent_p_98_Template, 2, 0, "p", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](99, "div", 57);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](100, "button", 58);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewDriverComponent_Template_button_click_100_listener() { return ctx.deleteDriver(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](101, "Confirm");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](102, "button", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](103, "Cancel");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.InvitedDrivers);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptionsOnboarded)("dtTrigger", ctx.dtTriggerOnboarded);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.OnboardedDrivers);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.HeaderText);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsShowShiftInfoPopup);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoadingdriverDelete);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsScheduleExists);
    } }, directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["CheckboxControlValueAccessor"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__["??c"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC92aWV3LWRyaXZlci92aWV3LWRyaXZlci5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](ViewDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-view-driver',
                templateUrl: './view-driver.component.html',
                styleUrls: ['./view-driver.component.css']
            }]
    }], function () { return [{ type: _services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"] }]; }, { CreateDriver: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"]]
        }] }); })();


/***/ }),

/***/ "./src/app/driver/driver-routing.module.ts":
/*!*************************************************!*\
  !*** ./src/app/driver/driver-routing.module.ts ***!
  \*************************************************/
/*! exports provided: DriverScheduleRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverScheduleRoutingModule", function() { return DriverScheduleRoutingModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./driver-schedule-calender/driver-schedule-calender.component */ "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");
/* harmony import */ var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./driver-management/driver-management.component */ "./src/app/driver/driver-management/driver-management.component.ts");
/* harmony import */ var _driver_driver_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./driver/driver.component */ "./src/app/driver/driver/driver.component.ts");







const routeDriver = [
    {
        path: '',
        component: _driver_driver_component__WEBPACK_IMPORTED_MODULE_4__["DriverComponent"]
    },
    {
        path: 'View',
        component: _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__["DriverManagementComponent"]
    },
    {
        path: 'schedule',
        component: _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"]
    }
];
class DriverScheduleRoutingModule {
}
DriverScheduleRoutingModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({ type: DriverScheduleRoutingModule });
DriverScheduleRoutingModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({ factory: function DriverScheduleRoutingModule_Factory(t) { return new (t || DriverScheduleRoutingModule)(); }, imports: [[
            _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDriver)
        ],
        _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](DriverScheduleRoutingModule, { imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]], exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DriverScheduleRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                imports: [
                    _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDriver)
                ],
                exports: [
                    _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts":
/*!***************************************************************************************!*\
  !*** ./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts ***!
  \***************************************************************************************/
/*! exports provided: DriverScheduleCalenderComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverScheduleCalenderComponent", function() { return DriverScheduleCalenderComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var date_fns__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! date-fns */ "./node_modules/date-fns/esm/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var angular_calendar__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! angular-calendar */ "./node_modules/angular-calendar/__ivy_ngcc__/fesm2015/angular-calendar.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/carrier/service/route-info.service */ "./src/app/carrier/service/route-info.service.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../create-driver-schedule/create-driver-schedule.component */ "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");


















function DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template(rf, ctx) { if (rf & 1) {
    const _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "mwl-calendar-month-view", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("dayClicked", function DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template_mwl_calendar_month_view_dayClicked_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r12); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r11.dayClicked($event.day); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](23);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("viewDate", ctx_r8.viewDate)("events", ctx_r8.evt)("cellTemplate", _r1)("refresh", ctx_r8.refresh)("activeDayIsOpen", ctx_r8.activeDayIsOpen);
} }
function DriverScheduleCalenderComponent_div_21_mwl_calendar_week_view_32_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "mwl-calendar-week-view", 49);
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("viewDate", ctx_r9.viewDate)("events", ctx_r9.evt)("refresh", ctx_r9.refresh);
} }
function DriverScheduleCalenderComponent_div_21_mwl_calendar_day_view_33_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "mwl-calendar-day-view", 49);
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("viewDate", ctx_r10.viewDate)("events", ctx_r10.evt)("refresh", ctx_r10.refresh);
} }
function DriverScheduleCalenderComponent_div_21_Template(rf, ctx) { if (rf & 1) {
    const _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](6, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_9_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r13.viewDate = $event; })("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_9_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r15.closeOpenMonthViewDay(); })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_9_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r16.setNextMonthEvents(ctx_r16.viewDate, "Previous"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](10, "i", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_11_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r17.viewDate = $event; })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_11_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r18.setNextMonthEvents(ctx_r18.viewDate, "Today"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](13, "calendarDate");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r19.viewDate = $event; })("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_14_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r20.closeOpenMonthViewDay(); })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_14_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r21.setNextMonthEvents(ctx_r21.viewDate, "Next"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](15, "i", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "a", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_18_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r22.setView(ctx_r22.CalendarView.Month); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "label", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Month");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "a", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_21_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r23.setView(ctx_r23.CalendarView.Week); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "label", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Week ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "a", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_24_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r24.setView(ctx_r24.CalendarView.Day); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "label", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Day");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](27, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "button", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Open Modal");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](31, DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template, 1, 5, "mwl-calendar-month-view", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](32, DriverScheduleCalenderComponent_div_21_mwl_calendar_week_view_32_Template, 1, 3, "mwl-calendar-week-view", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](33, DriverScheduleCalenderComponent_div_21_mwl_calendar_day_view_33_Template, 1, 3, "mwl-calendar-day-view", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("view", ctx_r0.view)("viewDate", ctx_r0.viewDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("viewDate", ctx_r0.viewDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind3"](13, 19, ctx_r0.viewDate, ctx_r0.view + "ViewTitle", "en"), " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("view", ctx_r0.view)("viewDate", ctx_r0.viewDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Month);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Month ? "btn-primary" : "btn-default");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Week);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Week ? "btn-primary" : "btn-default");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Day);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Day ? "btn-primary" : "btn-default");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngSwitch", ctx_r0.view);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngSwitchCase", ctx_r0.CalendarView.Month);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngSwitchCase", ctx_r0.CalendarView.Week);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngSwitchCase", ctx_r0.CalendarView.Day);
} }
const _c0 = function (a0) { return { "background-color": a0 }; };
function DriverScheduleCalenderComponent_ng_template_22_tr_3_td_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, " - ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const item_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c0, item_r30.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", item_r30.driverShortName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r30.shiftFrom);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r30.shiftTo);
} }
function DriverScheduleCalenderComponent_ng_template_22_tr_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, DriverScheduleCalenderComponent_ng_template_22_tr_3_td_1_Template, 9, 6, "td", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const i_r31 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", i_r31 < 2);
} }
function DriverScheduleCalenderComponent_ng_template_22_tr_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function DriverScheduleCalenderComponent_ng_template_22_span_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const group_r34 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classMapInterpolate1"]("badge badge-", group_r34[0], "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", group_r34[1].length, " ");
} }
function DriverScheduleCalenderComponent_ng_template_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "table", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, DriverScheduleCalenderComponent_ng_template_22_tr_3_Template, 2, 1, "tr", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](4, DriverScheduleCalenderComponent_ng_template_22_tr_4_Template, 4, 0, "tr", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](8, "calendarDate");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, DriverScheduleCalenderComponent_ng_template_22_span_10_Template, 2, 4, "span", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const day_r25 = ctx.day;
    const locale_r26 = ctx.locale;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", day_r25.events);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", day_r25.events.length > 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind3"](8, 4, day_r25.date, "monthViewDayNumber", locale_r26));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", day_r25.eventGroups);
} }
function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template(rf, ctx) { if (rf & 1) {
    const _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "i", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template_i_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r40); const event_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r38.editSchedule(event_r36); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, " \u00A0\u00A0\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "i", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template_i_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r40); const event_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r41.rmvSchedule(event_r36); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, " \u00A0\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function DriverScheduleCalenderComponent_table_33_tr_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "td", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template, 5, 0, "td", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const event_r36 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](13, _c0, event_r36.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](event_r36.driverName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](15, _c0, event_r36.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate3"](" ", event_r36.shiftFrom, " - ", event_r36.shiftTo, " ", event_r36.description, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](17, _c0, event_r36.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](event_r36.fromDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](19, _c0, event_r36.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](event_r36.toDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](21, _c0, event_r36.color.primary));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", event_r36.repeatDayList == null ? null : event_r36.repeatDayList.length, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !event_r36.isUnplanedSchedule);
} }
function DriverScheduleCalenderComponent_table_33_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "table", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Driver");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Shift");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, " From Date ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "To Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Total Days");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Action");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, DriverScheduleCalenderComponent_table_33_tr_16_Template, 12, 23, "tr", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r3.Selectedevent);
} }
function DriverScheduleCalenderComponent_div_34_div_9_Template(rf, ctx) { if (rf & 1) {
    const _r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "input", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_div_9_Template_input_ngModelChange_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r45); const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r44.deleteOption = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "label", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Delete the entire range of this schedule");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 2)("ngModel", ctx_r43.deleteOption);
} }
function DriverScheduleCalenderComponent_div_34_Template(rf, ctx) { if (rf & 1) {
    const _r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "h4", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Do you wish to :");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "input", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_Template_input_ngModelChange_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r47); const ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r46.deleteOption = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "label", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Delete only this day's schedule");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, DriverScheduleCalenderComponent_div_34_div_9_Template, 5, 2, "div", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "input", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_Template_input_ngModelChange_12_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r47); const ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r48.deleteOption = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "label", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Delete the whole schedule for this driver");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "button", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_34_Template_button_click_16_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r47); const ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r49.closeDeleteModel(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Close");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "button", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_34_Template_button_click_18_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r47); const ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r50.RemoveSchedule(ctx_r50.eventDelete); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, "Submit");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 1)("ngModel", ctx_r4.deleteOption);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx_r4.hideDeleteRange);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 3)("ngModel", ctx_r4.deleteOption);
} }
function DriverScheduleCalenderComponent_div_35_Template(rf, ctx) { if (rf & 1) {
    const _r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "button", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_35_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r52); const ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r51.closeViewDayDetailModel(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Close");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function DriverScheduleCalenderComponent_div_36_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function DriverScheduleCalenderComponent_div_38_div_27_Template(rf, ctx) { if (rf & 1) {
    const _r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "From Date:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "input", 103);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_27_Template_input_ngModelChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r60); const ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r59.eFromDate = $event; })("onDateChange", function DriverScheduleCalenderComponent_div_38_div_27_Template_input_onDateChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r60); const ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r61.setFromDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", true)("ngModel", ctx_r53.eFromDate)("format", "MM/DD/YYYY")("minDate", ctx_r53.MinStartDate)("maxDate", ctx_r53.MaxStartDate);
} }
function DriverScheduleCalenderComponent_div_38_div_28_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function DriverScheduleCalenderComponent_div_38_div_28_Template(rf, ctx) { if (rf & 1) {
    const _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "From Date:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "input", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_28_Template_input_ngModelChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r63.eFromDate = $event; })("onDateChange", function DriverScheduleCalenderComponent_div_38_div_28_Template_input_onDateChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r65.setFromDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](4, DriverScheduleCalenderComponent_div_38_div_28_div_4_Template, 2, 0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r54.eFromDate)("format", "MM/DD/YYYY")("minDate", ctx_r54.MinStartDate)("maxDate", ctx_r54.MaxStartDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r54.isRequired("eFromDate"));
} }
function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template(rf, ctx) { if (rf & 1) {
    const _r68 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "input", 114);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template_input_change_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r68); const ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3); return ctx_r67.InitializeDates(); })("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template_input_ngModelChange_1_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r68); const ctx_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3); return ctx_r69.selectedType = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label", 115);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Custom");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 4)("ngModel", ctx_r66.selectedType);
} }
function DriverScheduleCalenderComponent_div_38_div_34_Template(rf, ctx) { if (rf & 1) {
    const _r71 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 105);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "input", 107);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r71); const ctx_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r70.selectedType = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "label", 108);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Daily");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "input", 109);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_8_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r71); const ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r72.selectedType = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 110);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "WeekDays (Mon to Fri)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "input", 111);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_12_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r71); const ctx_r73 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r73.selectedType = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "label", 112);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Repeat Every");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, DriverScheduleCalenderComponent_div_38_div_34_div_15_Template, 4, 2, "div", 113);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 1)("ngModel", ctx_r55.selectedType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 2)("ngModel", ctx_r55.selectedType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 3)("ngModel", ctx_r55.selectedType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r55.eToDate && ctx_r55.eFromDate);
} }
function DriverScheduleCalenderComponent_div_38_div_37_Template(rf, ctx) { if (rf & 1) {
    const _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 116);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, " Dates:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "ng-multiselect-dropdown", 117);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_37_Template_ng_multiselect_dropdown_ngModelChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r75); const ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r74.customDates = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Date (s)")("settings", ctx_r56.multiselectDateSettingsById)("data", ctx_r56.DateList)("ngModel", ctx_r56.customDates);
} }
function DriverScheduleCalenderComponent_div_38_div_38_Template(rf, ctx) { if (rf & 1) {
    const _r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Days:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "input", 119);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_38_Template_input_ngModelChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r77); const ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r76.repeat = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r57.repeat);
} }
function DriverScheduleCalenderComponent_div_38_div_40_table_8_tr_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const event_r80 = ctx.$implicit;
    const ctx_r79 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx_r79.driverSchedule.selectedShifts[0].Name);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](event_r80);
} }
function DriverScheduleCalenderComponent_div_38_div_40_table_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "table", 123);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Shift");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, " Conflict Dates");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, DriverScheduleCalenderComponent_div_38_div_40_table_8_tr_8_Template, 5, 2, "tr", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r78.ConflictDateList);
} }
function DriverScheduleCalenderComponent_div_38_div_40_Template(rf, ctx) { if (rf & 1) {
    const _r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 105);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 120);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 121);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Warning:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, " Driver Schedule(s) exists ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "a", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_38_div_40_Template_a_click_6_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82); const ctx_r81 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r81.showDriverConflictSchedules(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Show Details");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, DriverScheduleCalenderComponent_div_38_div_40_table_8_Template, 9, 1, "table", 122);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r58.IsShowConflictTable);
} }
function DriverScheduleCalenderComponent_div_38_Template(rf, ctx) { if (rf & 1) {
    const _r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "ng-sidebar-container", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "ng-sidebar", 81);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("openedChange", function DriverScheduleCalenderComponent_div_38_Template_ng_sidebar_openedChange_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r83._openedEditPanel = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "a", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_38_Template_a_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r85._editToggleOpened(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "i", 83);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "h3", 84);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Modify Driver Schedule");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "content", 85);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("keydown.enter", function DriverScheduleCalenderComponent_div_38_Template_content_keydown_enter_7_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); return $event.preventDefault(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 87);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "label", 88);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Region:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "ng-multiselect-dropdown", 89);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r87.Selectedregion = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 87);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "label", 90);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Driver:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "ng-multiselect-dropdown", 89);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r88 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r88.SelectedDriver = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "label", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Shift:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "ng-multiselect-dropdown", 92);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r89 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r89.driverSchedule.selectedShifts = $event; })("onSelect", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_onSelect_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r90 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r90.validateShiftForDriver($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, DriverScheduleCalenderComponent_div_38_div_27_Template, 4, 5, "div", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](28, DriverScheduleCalenderComponent_div_38_div_28_Template, 5, 5, "div", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "label", 94);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "To Date:");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "input", 95);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_input_ngModelChange_33_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r91 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r91.eToDate = $event; })("onDateChange", function DriverScheduleCalenderComponent_div_38_Template_input_onDateChange_33_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r92.setToDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, DriverScheduleCalenderComponent_div_38_div_34_Template, 16, 7, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](37, DriverScheduleCalenderComponent_div_38_div_37_Template, 4, 4, "div", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, DriverScheduleCalenderComponent_div_38_div_38_Template, 4, 1, "div", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "div", 96);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](40, DriverScheduleCalenderComponent_div_38_div_40_Template, 9, 1, "div", 97);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 98);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 99);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "input", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_38_Template_input_click_43_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r93 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r93._editToggleOpened(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "button", 101);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function DriverScheduleCalenderComponent_div_38_Template_button_click_44_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r84); const ctx_r94 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r94.UpdateSchedule(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](45, "Submit");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx_r7._openedEditPanel)("animate", ctx_r7._animate)("position", ctx_r7._POSITIONS[ctx_r7._positionNum]);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Region(s)")("settings", ctx_r7.singleselectSettingsById)("data", ctx_r7.DriverRegionList)("ngModel", ctx_r7.Selectedregion)("disabled", true);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Driver(s)")("settings", ctx_r7.singleselectSettingsById)("data", ctx_r7.SelectedDriverList)("ngModel", ctx_r7.SelectedDriver)("disabled", true);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Shift(s)")("settings", ctx_r7.multiselectShiftById)("data", ctx_r7.ShiftList)("ngModel", ctx_r7.driverSchedule.selectedShifts);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx_r7.startDateEnable);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.startDateEnable);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r7.eToDate)("format", "MM/DD/YYYY")("minDate", ctx_r7.MinStartDate)("maxDate", ctx_r7.MaxStartDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.IsUpdateForMultiple);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.IsUpdateForMultiple && ctx_r7.selectedType == "4");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.IsUpdateForMultiple && ctx_r7.selectedType == "3");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.ConflictDateList.length > 0);
} }
class DriverScheduleCalenderComponent {
    //#endregion
    constructor(regionService, routeService) {
        this.regionService = regionService;
        this.routeService = routeService;
        this.CalendarView = angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"];
        this.isShowCalender = true;
        this.IsLoading = false;
        this.DriverRegionList = [];
        this.driverIds = '';
        this.SelectedDriverList = [];
        this.isShowEditPannel = false;
        this.isApplyAll = false; //edit schedule
        this.DriverShiftDetailList = [];
        this.RepeatList = [];
        this.DateList = [];
        this.ShiftList = [];
        this.regionList = [];
        this.TrailerList = [];
        this.RouteList = [];
        this.driverSchedule = {};
        this.driverScheduleMapping = {};
        this.repeat = 1;
        this.customDates = [];
        //min max date
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.view = angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month;
        this.viewDate = new Date();
        //view 
        this.Selectedevent = [];
        //end      
        this.IsUpdateForMultiple = false;
        this.scheduleType = 0;
        this.refresh = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        //evt: CalendarEvent[] = [];
        this.evt = [];
        this.activeDayIsOpen = false;
        this.RegionShiftDetailList = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        //#region  Edit & Delete
        this._openedEditPanel = false;
        this.IsSheduleEdit = false;
        this.Selectedregion = [];
        this.SelectedDriver = [];
        this.IsShiftRepeted = false;
        this.selectedType = 0;
        this.IsConfirmDelete = false;
        this.deleteOption = 1;
        this.hideDeleteRange = false;
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;
    }
    ngOnInit() {
        this.getDrivers();
        //  this.getShifts();
        this.getRegions();
        this.init();
        this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2);
        //this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 10);
    }
    ngAfterViewInit() {
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
        };
        this.singleselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false,
            disabledField: "true"
        };
        this.multiselectShiftById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: false
        };
        this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
        };
    }
    dayClicked({ date, events }) {
        if (Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["isSameMonth"])(date, this.viewDate)) {
            if ((Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["isSameDay"])(this.viewDate, date) && this.activeDayIsOpen === true) || events.length === 0) {
                this.activeDayIsOpen = false;
            }
            else {
                this.Selectedevent = events;
                this.SelectedDate = moment__WEBPACK_IMPORTED_MODULE_5__(date).format('MM/DD/YYYY');
                let element = document.getElementById('idViewDay');
                element.click();
                //this.activeDayIsOpen = true;
            }
            this.viewDate = date;
        }
    }
    setView(view) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.view = view;
            this.setNextMonthEvents(this.viewDate, this.view);
        });
    }
    closeOpenMonthViewDay() {
        this.activeDayIsOpen = false;
    }
    getDrivers() {
        this.regionService.getDrivers()
            .subscribe((drivers) => {
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
    getRegionScheduls(region) {
        this.regionService.getSchedulesByRegion(region.Id, this.scheduleType)
            .subscribe((regions) => {
            this.RegionShiftDetailList = regions;
        });
    }
    getRoutes(regionId) {
        this.routeService.getRoutesByRegion(regionId)
            .subscribe((routes) => {
            this.getRouteDropDown(routes);
        });
    }
    getRouteDropDown(routeList) {
        this.RouteList = routeList.ResponseData;
    }
    onDriverSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(true);
            this.evt = [];
            let driverIds = [];
            this.SelectedDriverList.forEach(res => { driverIds.push(res.Id); });
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
        });
    }
    onDriverDeSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.isShowCalender = false;
            this.evt = yield this.evt.filter(f => f.driverId != $event.Id);
            //  this.DriverShiftDetailList = await this.DriverShiftDetailList.filter(f => f.DriverId != $event.Id);
            this.isShowCalender = true;
        });
    }
    setEvents(mnth, year) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(true);
            this.DriverShiftDetailList.forEach((driver) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                var e_1, _a;
                let color = yield this.getRandomColor();
                let driverShortName = this.getShortDriverName(driver.DriverName);
                try {
                    for (var _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driver.ScheduleList), _c; _c = yield _b.next(), !_c.done;) {
                        let shift = _c.value;
                        let startDate = this.changeTimeFormat(shift.ShiftDetail.StartTime);
                        let endDate = this.changeTimeFormat(shift.ShiftDetail.EndTime);
                        //previous and next logic
                        if (year) {
                            startDate = new Date(new Date(new Date(new Date(startDate).setMonth(mnth))).setFullYear(year)).getTime();
                            endDate = new Date(new Date(new Date(new Date(endDate).setMonth(mnth))).setFullYear(year)).getTime();
                        }
                        //end
                        if (this.view != angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month && startDate > endDate) // if start time is greater than end time then add 1 day in end time
                         {
                            endDate = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(endDate), 1).getTime();
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
                            fromDate: moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrStartDate).format('MM/DD/YYYY'),
                            toDate: moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrEndDate).format('MM/DD/YYYY'),
                            start: new Date(startDate),
                            end: new Date(endDate),
                            title: `<table class="table "> <tr><td> Driver - <strong>${driver.DriverName}</strong></td> <td><strong>${shift.ShiftDetail.StartTime} - ${shift.ShiftDetail.EndTime}</strong></td></tr></table> `,
                            color: color,
                            resizable: {
                                beforeStart: true,
                                afterEnd: true
                            },
                            draggable: false,
                            isUnplanedSchedule: driver.IsUnplanedSchedule,
                            description: shift.Description,
                        };
                        //end
                        //this.evt.push(event)
                        let currentDate = new Date().getDate();
                        let eDate = new Date((moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrEndDate).toLocaleString())).setHours(23, 59, 59, 0);
                        let sDate = new Date((moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrStartDate).toLocaleString())).setHours(0, 0, 0, 0);
                        let date = new Date(new Date().setMonth(mnth)).setFullYear(year);
                        let currentMonthLastDate = yield this.daysInMonth(mnth + 1, year);
                        for (let i = -currentDate; i < ((currentMonthLastDate + 1) - currentDate); i++) {
                            if (new Date(sDate).getTime() <= Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(date), i).getTime() && new Date(eDate).getTime() >= Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(date), i).getTime() && shift.RepeatDayList && shift.RepeatDayStringList.filter(dt => moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.start), i)).format('MM/DD/YYYY')).length > 0)
                                yield this.addEventShift(event, i);
                        }
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) yield _a.call(_b);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
            }));
            this.regionService.onLoadingChanged.next(false);
        });
    }
    addEventShift(event, i) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            let evt = Object.assign({}, event);
            evt.start = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.start), i);
            evt.end = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.end), i);
            this.evt.push(evt);
        });
    }
    daysInMonth(month, year) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            return new Date(year, month, 0).getDate();
        });
    }
    changeTimeFormat(time) {
        //var time = '06:30 PM';
        var hours = Number(time.match(/^(\d+)/)[1]);
        var minutes = Number(time.match(/:(\d+)/)[1]);
        var AMPM = time.match(/\s(.*)$/)[1];
        if (AMPM == "PM" && hours < 12)
            hours = hours + 12;
        if (AMPM == "AM" && hours == 12)
            hours = hours - 12;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (hours < 10)
            sHours = "0" + sHours;
        if (minutes < 10)
            sMinutes = "0" + sMinutes;
        var date = (new Date(new Date().setHours(+sHours)).setMinutes(+sMinutes));
        return date;
    }
    getRandomColor() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var letters = 'BCDEF'.split('');
            var color = '#';
            for (var i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * letters.length)];
            }
            return { primary: color, secondary: color };
        });
    }
    setNextMonthEvents(date, event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.evt = [];
            yield this.setEvents(new Date(date).getMonth(), new Date(date).getFullYear());
        });
    }
    OnScheduleAdded($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(false);
            this.IsLoading = false;
            if ($event) {
                this.scheduleType = 0;
                this.SelectedDriverList.forEach(res => {
                    let cnt = $event.findIndex(x => x.Id == res.Id);
                    if (cnt < 0)
                        $event.push(res);
                });
                this.SelectedDriverList = $event.slice();
                this.onDriverSelect();
            }
        });
    }
    //edit
    rmvSchedule(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.eventDelete = event;
            this.AssignVeriables(event);
            this.IsConfirmDelete = true;
        });
    }
    updateCurrentSchedule() {
        var e_2, _a, e_3, _b;
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (this.driverSchedule.selectedShifts.length == 0) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Select shift', undefined, undefined);
                return false;
            }
            //rmv todays
            this.driverScheduleMapping = yield this.DriverShiftDetailList.find(f => f.Id == this.driverScheduleMapping.Id);
            if (this.driverScheduleMapping != null && this.driverScheduleMapping.ScheduleList.length > 0) {
                this.driverScheduleMapping.ScheduleList.forEach(f => {
                    if (f.IsActive =  true && f.Id == this.driverSchedule.Id && f.ShiftId == this.driverSchedule.ShiftId) {
                        if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                            if (f.RepeatDayList.length == 1) {
                                delete f.RepeatDayList[0];
                                f.RepeatDayList = [];
                            }
                            else {
                                let indexof = f.RepeatDayList.findIndex(x => moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).format('MM/DD/YYYY'));
                                delete f.RepeatDayList[indexof];
                                let reOrder = [];
                                f.RepeatDayList.forEach(r => { reOrder.push(r); });
                                f.RepeatDayList = reOrder;
                            }
                        }
                    }
                });
                let getCurrentSelectedShift = this.driverSchedule.selectedShifts[0].Id;
                let checkShift = this.driverScheduleMapping.ScheduleList.filter(x => x.IsActive =  true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null);
                if (checkShift != null && checkShift.length > 0) {
                    let index = this.driverScheduleMapping.ScheduleList.findIndex(x => x.IsActive =  true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null);
                    this.driverScheduleMapping.ScheduleList[index].RepeatDayList = [];
                    this.driverScheduleMapping.ScheduleList[index].RepeatDayList.push(moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate());
                    this.driverScheduleMapping.ScheduleList[index].StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                    this.driverScheduleMapping.ScheduleList[index].EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                }
                else {
                    this.driverSchedule.RepeatDayList = [];
                    this.driverSchedule.RepeatDayList.push(moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate());
                    this.driverScheduleMapping.ScheduleList.push({ Id: `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate(), EndDate: moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate(), RepeatDayList: this.driverSchedule.RepeatDayList, ShiftId: getCurrentSelectedShift });
                }
            }
            try {
                for (var _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList), _d; _d = yield _c.next(), !_d.done;) {
                    let item = _d.value;
                    if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                        item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                        item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                        var list = [];
                        try {
                            for (var _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList), _f; _f = yield _e.next(), !_f.done;) {
                                let repeat = _f.value;
                                list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());
                            }
                        }
                        catch (e_3_1) { e_3 = { error: e_3_1 }; }
                        finally {
                            try {
                                if (_f && !_f.done && (_b = _e.return)) yield _b.call(_e);
                            }
                            finally { if (e_3) throw e_3.error; }
                        }
                        item.RepeatDayList = list;
                    }
                }
            }
            catch (e_2_1) { e_2 = { error: e_2_1 }; }
            finally {
                try {
                    if (_d && !_d.done && (_a = _c.return)) yield _a.call(_c);
                }
                finally { if (e_2) throw e_2.error; }
            }
            this.update(this.driverScheduleMapping);
        });
    }
    editSchedule(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.DriverRegionList = [];
            let currentDate = new Date((moment__WEBPACK_IMPORTED_MODULE_5__().toLocaleString())).setHours(0, 0, 0, 0);
            if (new Date(event.toDate).getTime() < new Date(currentDate).getTime()) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('You cannot edit past records.', undefined, undefined);
            }
            else {
                this._editToggleOpened(true);
                this.TypeCheck(event);
                this.AssignVeriables(event);
            }
        });
    }
    AssignVeriables(event) {
        var e_4, _a;
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.deleteOption = this.deleteOption ? this.deleteOption : 1;
            this.DriverRegionList = [];
            let currentDate = new Date((moment__WEBPACK_IMPORTED_MODULE_5__().toLocaleString())).setHours(0, 0, 0, 0);
            try {
                for (var _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.regionList), _c; _c = yield _b.next(), !_c.done;) {
                    let item = _c.value;
                    if (item.Drivers.find(f => f.Id == event.driverId)) {
                        var dRegion = { Id: 0, Name: "" };
                        dRegion.Id = item.Id;
                        dRegion.Name = item.Name;
                        this.DriverRegionList.push(dRegion);
                        this.Selectedregion = this.DriverRegionList;
                        this.SelectedDriver = item.Drivers.filter(f => f.Id == event.driverId);
                        this.ShiftList = item.Shifts.map(res => { return { Id: res.Id, Name: `${res.StartTime} - ${res.EndTime}` }; });
                    }
                }
            }
            catch (e_4_1) { e_4 = { error: e_4_1 }; }
            finally {
                try {
                    if (_c && !_c.done && (_a = _b.return)) yield _a.call(_b);
                }
                finally { if (e_4) throw e_4.error; }
            }
            this.driverSchedule.selectedShifts = this.ShiftList.filter(f => f.Id == event.shiftId); // this.isShowEditPannel = true;
            this.eFromDate = moment__WEBPACK_IMPORTED_MODULE_5__(event.start).format('MM/DD/YYYY');
            this.eToDate = moment__WEBPACK_IMPORTED_MODULE_5__(event.toDate).format('MM/DD/YYYY');
            this.driverScheduleMapping.Id = event.mappingId;
            this.driverSchedule.Id = event.Id;
            this.driverSchedule.ShiftId = event.shiftId;
            this.driverScheduleMapping.DriverId = event.driverId;
            this.DriverId = event.driverId;
            this.driverScheduleMapping.DriverName = event.driverName;
            this.sdate = moment__WEBPACK_IMPORTED_MODULE_5__(event.start).format('MM/DD/YYYY'); //this.SelectedDate;
            this.edate = moment__WEBPACK_IMPORTED_MODULE_5__(event.toDate).format('MM/DD/YYYY');
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
                    let xdate = moment__WEBPACK_IMPORTED_MODULE_5__(x).toDate();
                    let dt = `${moment__WEBPACK_IMPORTED_MODULE_5__(xdate).format('MM/DD/YYYY')} (${days[new Date(xdate).getDay()]})`;
                    let sdt = this.DateList.find(x => x.Name == dt);
                    if (sdt != null)
                        chCustomDates.push({ Id: sdt.Id, Name: sdt.Name });
                });
                this.customDates = chCustomDates;
            }
        });
    }
    TypeCheck(event) {
        this.selectedType = event.typeId ? event.typeId : "1";
        this.repeat = event.repeatEvery ? event.repeatEvery : "1";
    }
    UpdateSchedule() {
        var e_5, _a, e_6, _b;
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (this.sdate && this.edate) {
                this.driverSchedule.StartDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate().getTime());
                this.driverSchedule.EndDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate().getTime());
            }
            else {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Select valid dates', undefined, undefined);
                return false;
            }
            if (moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate) > moment__WEBPACK_IMPORTED_MODULE_5__(this.edate)) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('From date cannot less than end date', undefined, undefined);
                return false;
            }
            if (this.driverSchedule.selectedShifts.length < 0) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Please select shift', undefined, undefined);
                return false;
            }
            this.InitializeDates();
            if (this.driverSchedule.selectedShifts.length == 0 || !this.driverSchedule.StartDate || !this.driverSchedule.EndDate || this.DateList.length == 0) {
                return false;
            }
            //update logic
            else {
                let selectedDates = [];
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
                    let getIndex = this.driverScheduleMapping.ScheduleList.findIndex(x => x.Id == this.driverSchedule.Id && x.ShiftId == this.driverSchedule.ShiftId && x.IsActive);
                    //Update current one
                    if (this.driverSchedule.ShiftId != ShiftId && getCurrent != null && getCurrent.RepeatDayList != null) {
                        // 
                        let rpDayList = [];
                        getCurrent.RepeatDayList.forEach(pre => {
                            let idx = selectedDates.findIndex(x => moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(pre).format('MM/DD/YYYY'));
                            if (idx < 0)
                                rpDayList.push(pre);
                        });
                        getCurrent.RepeatDayList = [];
                        getCurrent.RepeatDayList = rpDayList;
                    }
                    else {
                        if (getCurrent != null && getCurrent.RepeatDayList != null) {
                            let rpDayList = [];
                            var loop = new Date(this.driverSchedule.StartDate);
                            let cnt = 0;
                            while (loop <= this.driverSchedule.EndDate) {
                                var newDate = loop.setDate(loop.getDate() + cnt);
                                if (!(newDate > this.driverSchedule.EndDate.setDate(this.driverSchedule.EndDate.getDate()))) {
                                    getCurrent.RepeatDayList = getCurrent.RepeatDayList.filter(x => moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') != moment__WEBPACK_IMPORTED_MODULE_5__(newDate).format('MM/DD/YYYY'));
                                    if (cnt != 1) {
                                        cnt++;
                                    }
                                }
                            }
                            selectedDates.forEach(element => getCurrent.RepeatDayList.push("/Date(" + element.Id.setDate(element.Id.getDate()) + ")"));
                            let olst = [];
                            getCurrent.RepeatDayList.forEach(x => olst.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('DD/MM/YYYY')));
                            olst.sort();
                            getCurrent.RepeatDayList = [];
                            getCurrent.RepeatDayList = olst;
                            olst = [];
                            getCurrent.RepeatDayList.forEach(x => {
                                var dt = x.split('/');
                                let oDt = dt[1] + '/' + dt[0] + '/' + dt[2];
                                let eDt = moment__WEBPACK_IMPORTED_MODULE_5__(oDt).toDate();
                                olst.push(eDt.setDate(eDt.getDate()));
                            });
                            getCurrent.RepeatDayList = olst;
                            olst = [];
                            getCurrent.RepeatDayList.forEach(x => olst.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).toDate()));
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
                        getCurrent.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                    });
                    getCurrent.EndDate = getCurrent.RepeatDayList[getCurrent.RepeatDayList.length - 1];
                    delete this.driverScheduleMapping.ScheduleList[getIndex];
                    this.driverScheduleMapping.ScheduleList.splice(getIndex, 0, getCurrent);
                    //Update current done
                    //start add new logic    
                    if (this.driverSchedule.ShiftId != ShiftId) {
                        let oDateList = [];
                        let oDateListString = [];
                        if (this.selectedType == 4) {
                            this.customDates.forEach(x => {
                                if (!this.ConflictDateList.some((item) => moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY'))) {
                                    oDateList.push(new Date(x.Id).getTime());
                                }
                            });
                        }
                        else {
                            this.DateList.forEach(x => {
                                if (!this.ConflictDateList.some((item) => moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY'))) {
                                    oDateList.push(new Date(x.Id).getTime());
                                }
                            });
                        }
                        this.DateList.forEach(x => { oDateListString.push(moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY')); });
                        newScheduleId = `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`;
                        scheduleList = { Id: `${this.driverScheduleMapping.DriverId}_${new Date().getTime()}`, IsActive: true, StartDate: new Date(oDateList[0]).getTime(), EndDate: new Date(oDateList[oDateList.length - 1]).getTime(), RepeatDayList: oDateList, RepeatDayStringList: oDateListString, ShiftId: ShiftId, RepeatEveryDay: this.repeat, TypeId: this.selectedType };
                        this.driverScheduleMapping.ScheduleList.push(scheduleList);
                    }
                }
                // reset index
                let oScheduleList = [];
                this.driverScheduleMapping.ScheduleList.forEach(e => { oScheduleList.push(e); });
                this.driverScheduleMapping.ScheduleList = [];
                this.driverScheduleMapping.ScheduleList = oScheduleList;
                //Check previous records exist or not for same shift with conflict dates 
                this.driverScheduleMapping.ScheduleList.forEach(x => {
                    if (x.IsActive && x.ShiftId == ShiftId && newScheduleId != x.Id) {
                        let newList = [];
                        if (x.RepeatDayList != null && x.RepeatDayList.length > 0) {
                            x.RepeatDayList.forEach(y => {
                                if (!this.ConflictDateList.some((item) => moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(y).format('MM/DD/YYYY'))) {
                                    newList.push(moment__WEBPACK_IMPORTED_MODULE_5__(y).toDate());
                                }
                            });
                            x.RepeatDayList = newList;
                            if (x.RepeatDayList.length > 0) {
                                x.StartDate = x.RepeatDayList[0];
                                x.EndDate = x.RepeatDayList[x.RepeatDayList.length - 1];
                                x.RepeatDayStringList = [];
                                x.RepeatDayList.forEach(t => x.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(t).format('MM/DD/YYYY')));
                            }
                            else {
                                x.IsActive = false;
                            }
                        }
                    }
                });
                try {
                    for (var _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList), _d; _d = yield _c.next(), !_d.done;) {
                        let item = _d.value;
                        if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                            item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                            item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                            var list = [];
                            var stringList = [];
                            item.RepeatDayStringList = [];
                            try {
                                for (var _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList), _f; _f = yield _e.next(), !_f.done;) {
                                    let repeat = _f.value;
                                    list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());
                                    if (item.RepeatDayStringList == null || item.RepeatDayStringList.length == 0) {
                                        stringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).format('MM/DD/YYYY'));
                                    }
                                }
                            }
                            catch (e_6_1) { e_6 = { error: e_6_1 }; }
                            finally {
                                try {
                                    if (_f && !_f.done && (_b = _e.return)) yield _b.call(_e);
                                }
                                finally { if (e_6) throw e_6.error; }
                            }
                            item.RepeatDayList = list;
                            item.RepeatDayStringList = stringList;
                        }
                    }
                }
                catch (e_5_1) { e_5 = { error: e_5_1 }; }
                finally {
                    try {
                        if (_d && !_d.done && (_a = _c.return)) yield _a.call(_c);
                    }
                    finally { if (e_5) throw e_5.error; }
                }
                //reset Index of 
                delete CurrentDriverShiftDetailList[driverScheduleMappingIndex];
                CurrentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
                let oShifScheduleList = [];
                CurrentDriverShiftDetailList.forEach(e => { oShifScheduleList.push(e); });
                CurrentDriverShiftDetailList = [];
                CurrentDriverShiftDetailList = oShifScheduleList;
                //End Reset Index of 
                CurrentDriverShiftDetailList.forEach(ele => {
                    if (ele.Id != this.driverScheduleMapping.Id) {
                        ele.ScheduleList.forEach(pop => {
                            if (pop.IsActive && pop.ShiftId == ShiftId && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {
                                let rpDayList = [];
                                pop.RepeatDayList.forEach(pre => {
                                    let idx = selectedDates.findIndex(x => moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(pre).format('MM/DD/YYYY'));
                                    if (idx < 0)
                                        rpDayList.push(pre);
                                });
                                pop.RepeatDayList = [];
                                pop.RepeatDayList = rpDayList;
                                pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                                pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                                pop.RepeatDayStringList = [];
                                pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY')));
                            }
                            if (pop.RepeatDayList == null || pop.RepeatDayList.length == 0)
                                pop.IsActive = false;
                            else {
                                pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                                pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                                pop.RepeatDayStringList = [];
                                pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY')));
                            }
                        });
                    }
                });
                //end
                this.update(CurrentDriverShiftDetailList);
            }
        });
    }
    delete(model, sDate) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(true);
            this.regionService.deleteDriverSchedule(model, sDate)
                .subscribe((response) => {
                if (response != null && response.StatusCode == 0) {
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess('Driver Schedule deleted successfully', undefined, undefined);
                    setTimeout(function () {
                        let element = document.getElementById('idCloseModel');
                        element.click();
                    }, 2000);
                    this.regionService.onLoadingChanged.next(false);
                    this.refresh.next();
                    this.onDriverSelect();
                    this.setView(angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month);
                    this.deleteOption = 1;
                }
                else {
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    setTimeout(function () {
                        let element = document.getElementById('idCloseModel');
                        element.click();
                    }, 1500);
                    this.regionService.onLoadingChanged.next(false);
                    this.refresh.next();
                    this.deleteOption = 1;
                    this.onDriverSelect();
                    this.setView(angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month);
                }
                this.clearEditForm();
            });
            this.hideDeleteRange = false;
            this.IsConfirmDelete = false;
        });
    }
    update(model, Isdelete) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.regionService.onLoadingChanged.next(true);
            this.regionService.updateDriverSchedule(model, this.SelectedDate)
                .subscribe((response) => {
                if (response != null && response.StatusCode == 0) {
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess('Driver Schedule updated successfully', undefined, undefined);
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
                            let element = document.getElementById('idCloseModel');
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
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    this.refresh.next();
                }
                this.clearEditForm();
            });
        });
    }
    getRegions() {
        this.regionService.onLoadingChanged.next(true);
        this.regionService.getRegions()
            .subscribe((region) => {
            this.regionList = region.Regions;
            this.regionService.onLoadingChanged.next(false);
        });
    }
    InitializeDates(sdate, end) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.driverSchedule.RepeatDayList = [];
            this.DateList = [];
            this.repeat = this.selectedType == 3 ? this.repeat : 0;
            var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            if (this.sdate && this.edate) {
                for (var dt = new Date(this.sdate); dt <= new Date(this.edate); dt.setDate(dt.getDate() + this.repeat + 1)) {
                    if (this.selectedType && this.selectedType == 2) {
                        (new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6) ? this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` }) : '';
                    }
                    else {
                        this.DateList.push({ Id: new Date(dt), Name: `${moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY')} (${days[new Date(dt).getDay()]})` });
                    }
                }
            }
            for (var dt = new Date(sdate); dt <= new Date(end); dt.setDate(dt.getDate() + 1)) {
                this.driverSchedule.RepeatDayList.push(new Date(dt));
            }
            //this.customDates = this.DateList;
            this.validateShiftForDriver(this.driverSchedule.selectedShifts[0]);
            return this.DateList;
        });
    }
    setFromDate(event) {
        this.sdate = (event);
        let d = moment__WEBPACK_IMPORTED_MODULE_5__(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
        !this.edate ? this.edate = (moment__WEBPACK_IMPORTED_MODULE_5__(d).format('MM/DD/YYYY')) : '';
        if (this.sdate != '' && this.edate != '') {
            let _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
            let _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate();
            if (_toDate < _fromDate) {
                this.edate = (event);
            }
        }
    }
    setToDate(event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.IsUpdateForMultiple = false;
            this.edate = (event);
            if (this.sdate != '' && this.edate != '') {
                let _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                let _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate();
                if (_fromDate > _toDate) {
                    this.sdate = (event);
                }
                if (_toDate > _fromDate) {
                    this.IsUpdateForMultiple = true;
                }
            }
            this.InitializeDates();
        });
    }
    closeViewDayDetailModel() {
        this.isShowEditPannel = false;
        this.startDateEnable = false; // validate from date if it is less than current date.
        this.isApplyAll = false;
        this.driverScheduleMapping = {};
        this.driverSchedule = {};
        this._openedEditPanel = false;
        let element = document.getElementById('idCloseModel');
        element.click();
    }
    getShortDriverName(name) {
        const fullName = name.split(' ');
        const lastName = fullName.pop();
        const firstName = fullName.join(' ');
        return firstName.substring(0, 1) + " " + lastName;
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }
    _editToggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._openedEditPanel = true;
        }
        else {
            this._openedEditPanel = !this._openedEditPanel;
        }
        this.clearEditForm();
    }
    clearEditForm() {
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
        this.IsShiftRepeted = false;
        this.IsSheduleEdit = false;
        this.SelectedDate = new Date();
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;
    }
    changeIsApplyAll() {
        this.isApplyAll = !this.isApplyAll;
        this.selectedType = 1;
        this.InitializeDates();
    }
    validateShiftForDriver(event) {
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
                                                if (moment__WEBPACK_IMPORTED_MODULE_5__(slDate.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(dte).format('MM/DD/YYYY')) {
                                                    this.ConflictDateList.push(moment__WEBPACK_IMPORTED_MODULE_5__(slDate.Id).format('MM/DD/YYYY'));
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
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please select dates first', undefined, undefined);
            return true;
        }
        if (this.ConflictDateList.length > 0) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning("Following shifts are already assigned to the drive", undefined, undefined);
        }
    }
    isRequired(name) {
        if (name == "" || name == null) {
            return true;
        }
        else {
            return false;
        }
    }
    closeDeleteModel() {
        this.IsConfirmDelete = false;
    }
    RemoveSchedule(event) {
        var e_7, _a, e_8, _b, e_9, _c;
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            let currentDelete;
            this.driverScheduleMapping = yield this.DriverShiftDetailList.find(f => f.Id == event.mappingId);
            let currentDriverShiftDetailList = yield this.DriverShiftDetailList.filter(f => f.DriverId == this.DriverId);
            let driverScheduleMappingIndex = yield currentDriverShiftDetailList.findIndex(f => f.Id == event.mappingId);
            let driverShiftMapping = yield this.DriverShiftDetailList.filter(f => f.DriverId == this.DriverId);
            if (this.deleteOption != "" && this.deleteOption != null) {
                if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Single) {
                    this.driverScheduleMapping.ScheduleList.forEach(f => {
                        if (f.IsActive =  true && f.Id == this.driverSchedule.Id && f.ShiftId == this.driverSchedule.ShiftId) {
                            if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                                currentDelete = f;
                                if (f.RepeatDayList.length == 1) {
                                    delete f.RepeatDayList[0];
                                    f.RepeatDayList = null;
                                }
                                else {
                                    let indexof = f.RepeatDayList.findIndex(x => moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).format('MM/DD/YYYY'));
                                    delete f.RepeatDayList[indexof];
                                    let reOrder = [];
                                    f.RepeatDayList.forEach(r => { reOrder.push(r); });
                                    f.RepeatDayList = reOrder;
                                    f.StartDate = f.RepeatDayList[0];
                                    f.EndDate = f.RepeatDayList[f.RepeatDayList.length - 1];
                                    f.RepeatDayStringList = [];
                                    f.RepeatDayList.forEach(x => {
                                        f.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                                    });
                                }
                            }
                        }
                    });
                    this.IsConfirmDelete = false;
                }
                if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Range) {
                    if (this.driverScheduleMapping.ScheduleList.length > 0) {
                        let sList = this.driverScheduleMapping.ScheduleList.forEach(res => {
                            if (res.Id == this.driverSchedule.Id) {
                                let reOrder = [] = res.RepeatDayList.filter(x => moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') < moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).format('MM/DD/YYYY'));
                                res.RepeatDayList = reOrder;
                                res.StartDate = res.RepeatDayList[0];
                                res.EndDate = res.RepeatDayList[res.RepeatDayList.length - 1];
                                res.RepeatDayStringList = [];
                                res.RepeatDayList.forEach(x => {
                                    res.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                                });
                            }
                        });
                    }
                    this.IsConfirmDelete = false;
                }
                if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Whole) {
                    if (driverShiftMapping.length > 0) {
                        try {
                            for (var driverShiftMapping_1 = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driverShiftMapping), driverShiftMapping_1_1; driverShiftMapping_1_1 = yield driverShiftMapping_1.next(), !driverShiftMapping_1_1.done;) {
                                let oSchedule = driverShiftMapping_1_1.value;
                                if (oSchedule.ScheduleList.length > 0) {
                                    oSchedule.ScheduleList.forEach(ele => {
                                        if (ele.RepeatDayList != null) {
                                            let reOrder = [] = ele.RepeatDayList.filter(x => moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') < moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).format('MM/DD/YYYY'));
                                            ele.RepeatDayList = reOrder;
                                            ele.StartDate = ele.RepeatDayList[0];
                                            ele.EndDate = ele.RepeatDayList[ele.RepeatDayList.length - 1];
                                            ele.RepeatDayStringList = [];
                                            ele.RepeatDayList.forEach(x => {
                                                ele.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                                            });
                                        }
                                    });
                                }
                            }
                        }
                        catch (e_7_1) { e_7 = { error: e_7_1 }; }
                        finally {
                            try {
                                if (driverShiftMapping_1_1 && !driverShiftMapping_1_1.done && (_a = driverShiftMapping_1.return)) yield _a.call(driverShiftMapping_1);
                            }
                            finally { if (e_7) throw e_7.error; }
                        }
                        yield driverShiftMapping.forEach(element => {
                            for (let item of element.ScheduleList) {
                                if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                                    item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                                    item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                                    var list = [];
                                    for (let repeat of item.RepeatDayList) {
                                        list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());
                                    }
                                    item.RepeatDayList = list;
                                }
                            }
                        });
                        this.delete(driverShiftMapping, this.SelectedDate);
                    }
                }
                if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Range || this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Single) {
                    try {
                        for (var _d = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList), _e; _e = yield _d.next(), !_e.done;) {
                            let item = _e.value;
                            if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                                item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                                item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                                var list = [];
                                try {
                                    for (var _f = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList), _g; _g = yield _f.next(), !_g.done;) {
                                        let repeat = _g.value;
                                        list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());
                                    }
                                }
                                catch (e_9_1) { e_9 = { error: e_9_1 }; }
                                finally {
                                    try {
                                        if (_g && !_g.done && (_c = _f.return)) yield _c.call(_f);
                                    }
                                    finally { if (e_9) throw e_9.error; }
                                }
                                item.RepeatDayList = list;
                            }
                        }
                    }
                    catch (e_8_1) { e_8 = { error: e_8_1 }; }
                    finally {
                        try {
                            if (_e && !_e.done && (_b = _d.return)) yield _b.call(_d);
                        }
                        finally { if (e_8) throw e_8.error; }
                    }
                    delete currentDriverShiftDetailList[driverScheduleMappingIndex];
                    currentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
                    let oShifScheduleList = [];
                    currentDriverShiftDetailList.forEach(e => { oShifScheduleList.push(e); });
                    this.DriverShiftDetailList = [];
                    currentDriverShiftDetailList = oShifScheduleList;
                    //End Reset Index of 
                    currentDriverShiftDetailList.forEach(ele => {
                        if (ele.Id != this.driverScheduleMapping.Id) {
                            ele.ScheduleList.forEach(pop => {
                                if (pop.IsActive && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {
                                    pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                                    pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                                    pop.RepeatDayStringList = [];
                                    pop.RepeatDayList.forEach(ab => pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY')));
                                }
                                else
                                    pop.IsActive = false;
                            });
                        }
                    });
                    this.update(currentDriverShiftDetailList, true);
                    return true;
                }
            }
            else {
                _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please select delete option', undefined, undefined);
                return false;
            }
        });
    }
    showDriverConflictSchedules() {
        this.IsShowConflictTable = !this.IsShowConflictTable;
    }
}
DriverScheduleCalenderComponent.??fac = function DriverScheduleCalenderComponent_Factory(t) { return new (t || DriverScheduleCalenderComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__["RouteInfoService"])); };
DriverScheduleCalenderComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: DriverScheduleCalenderComponent, selectors: [["app-driver-schedule-calender"]], decls: 39, vars: 19, consts: [[1, "row", "justify-content-between"], [1, "col-sm-2"], [1, "form-group", "mb10"], [3, "placeholder", "settings", "data", "ngModel", "onSelect", "onDeSelect", "ngModelChange"], [1, "col-sm-6", 2, "display", "none"], [1, "form-group"], [1, "radio-inline"], ["type", "radio", "name", "scheduleType", "ng-control", "scheduleType", 3, "ngModel", "value", "ngModelChange", "change"], ["type", "radio", "name", "scheduleType", "ng-control", "scheduleType", "value", "2", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-3", "float-right"], [3, "OnScheduleAdded"], ["class", "row", 4, "ngIf"], ["customCellTemplate", ""], ["id", "idViewDayDetail", "role", "dialog", "data-backdrop", "static", 1, "modal", "fade"], [1, "modal-dialog", "modal-lg"], [1, "modal-content"], [1, "modal-header"], [1, "modal-title"], [1, "pull-right"], [1, "modal-body"], ["class", "table table-bordered", 4, "ngIf"], ["class", "form-group col-md-12", 4, "ngIf"], ["class", "modal-footer", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "col-md-3", "text-center"], [1, "col-md-5", "text-center"], ["mwlCalendarPreviousView", "", 1, "btn", "btn-default", "btn-xs", 3, "view", "viewDate", "viewDateChange", "click"], [1, "fas", "fa-arrow-left"], ["id", "idToday", "mwlCalendarToday", "", 1, "btn", "btn-default", "btn-xs", 3, "viewDate", "viewDateChange", "click"], ["mwlCalendarNextView", "", 1, "btn", "btn-default", "btn-xs", 3, "view", "viewDate", "viewDateChange", "click"], [1, "fas", "fa-arrow-right"], [1, "col-md-4", "text-right"], ["id", "idMonth", 1, "btn", 3, "ngClass", "click"], ["for", "idMonth"], ["id", "idWeek", 1, "btn", 3, "ngClass", "click"], ["for", "idWeek"], ["id", "idDay", 1, "btn", 3, "ngClass", "click"], ["for", "idDay"], [3, "ngSwitch"], ["type", "button", "id", "idViewDay", "hidden", "", "data-toggle", "modal", "data-target", "#idViewDayDetail"], [3, "viewDate", "events", "cellTemplate", "refresh", "activeDayIsOpen", "dayClicked", 4, "ngSwitchCase"], [3, "viewDate", "events", "refresh", 4, "ngSwitchCase"], [3, "viewDate", "events", "cellTemplate", "refresh", "activeDayIsOpen", "dayClicked"], [3, "viewDate", "events", "refresh"], [1, "cal-cell-top"], [1, "table", "table-hover"], [4, "ngFor", "ngForOf"], [1, "cal-day-number"], [1, "cell-totals"], [3, "class", 4, "ngFor", "ngForOf"], ["style", "color:black", "class", "label  calender-grid", 3, "ngStyle", 4, "ngIf"], [1, "label", "calender-grid", 2, "color", "black", 3, "ngStyle"], [1, "table", "table-bordered"], [3, "ngStyle"], [1, "fas", "fa-edit", "icon-zoom", "btn-primary", "label-font", 3, "click"], [1, "fas", "fa-trash-alt", "color-maroon", "icon-zoom", "label-font", 3, "click"], [1, "form-group", "col-md-12"], [1, "row", "col-md-12"], [1, "form-control"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsSingle", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsSingle", 1, "form-check-label"], ["class", "row col-md-12", 4, "ngIf"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsEntire", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsEntire", 1, "form-check-label"], [1, "modal-footer"], ["type", "button", "id", "closeDelete", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], ["type", "button", "id", "deleteSchedule", 1, "btn", "btn-default", 3, "click"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsRange", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsRange", 1, "form-check-label"], ["type", "button", "id", "idCloseModel", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["autocomplete", "off", 1, "pr30", 3, "keydown.enter"], [1, "col-sm-6"], [1, "form-group", "readonly"], ["for", "Region"], [3, "placeholder", "settings", "data", "ngModel", "disabled", "ngModelChange"], ["for", "Drivers"], ["for", "Shift"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange", "onSelect"], ["class", "form-group", 4, "ngIf"], ["for", "ToDate"], ["required", "", "type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], [1, "row", "form-group"], ["class", "col-sm-12", 4, "ngIf"], [1, "col-sm-12", "row-fluid", "text-right", "form-buttons"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-default", 3, "click"], ["type", "button", "id", "Submit", 1, "btn", "btn-primary", 3, "click"], ["for", "fromDate"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "readonly", "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "type", "radio", "name", "selectedTypes", "id", "inlineRadioDaily", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioWdays", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioEvery", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioCustom", 1, "form-check-input", 3, "value", "ngModel", "change", "ngModelChange"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "alert", "alert-warning", "fs12", "mt10", "mb0", "radius-10"], [1, "fas", "fa-exclamation-circle", "mr5"], ["class", "table table-striped table-bordered table-hover", 4, "ngIf"], [1, "table", "table-striped", "table-bordered", "table-hover"]], template: function DriverScheduleCalenderComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "ng-multiselect-dropdown", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { return ctx.onDriverSelect($event); })("onDeSelect", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { return ctx.onDriverDeSelect($event); })("ngModelChange", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { return ctx.SelectedDriverList = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_9_listener($event) { return ctx.scheduleType = $event; })("change", function DriverScheduleCalenderComponent_Template_input_change_9_listener() { return ctx.onDriverSelect(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "All");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_13_listener($event) { return ctx.scheduleType = $event; })("change", function DriverScheduleCalenderComponent_Template_input_change_13_listener() { return ctx.onDriverSelect(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Planned Schedule");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_17_listener($event) { return ctx.scheduleType = $event; })("change", function DriverScheduleCalenderComponent_Template_input_change_17_listener() { return ctx.onDriverSelect(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "UnPlanned Schedule ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "create-driver-schedule", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("OnScheduleAdded", function DriverScheduleCalenderComponent_Template_create_driver_schedule_OnScheduleAdded_20_listener($event) { return ctx.OnScheduleAdded($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, DriverScheduleCalenderComponent_div_21_Template, 34, 23, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](22, DriverScheduleCalenderComponent_ng_template_22_Template, 11, 8, "ng-template", null, 12, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "h4", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "span", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](33, DriverScheduleCalenderComponent_table_33_Template, 17, 1, "table", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, DriverScheduleCalenderComponent_div_34_Template, 20, 5, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, DriverScheduleCalenderComponent_div_35_Template, 3, 0, "div", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](36, DriverScheduleCalenderComponent_div_36_Template, 5, 0, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](37, "async");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, DriverScheduleCalenderComponent_div_38_Template, 46, 27, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Driver(s)")("settings", ctx.multiselectSettingsById)("data", ctx.DriverList)("ngModel", ctx.SelectedDriverList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.scheduleType)("value", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.scheduleType)("value", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.scheduleType);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.isShowCalender);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", ctx.SelectedDate, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.driverScheduleMapping.DriverName);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx.isShowEditPannel && !ctx.IsConfirmDelete);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsConfirmDelete);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx.IsConfirmDelete);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind1"](37, 17, ctx.regionService.onLoadingChanged));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsSheduleEdit);
    } }, directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["DefaultValueAccessor"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverScheduleComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["??f"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["??h"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["??g"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgSwitch"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgSwitchCase"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarMonthViewComponent"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarWeekViewComponent"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarDayViewComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgStyle"], ng_sidebar__WEBPACK_IMPORTED_MODULE_14__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_14__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RequiredValidator"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NumberValueAccessor"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["AsyncPipe"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["??i"]], styles: [".calender-grid[_ngcontent-%COMP%]{\r\n    font-size:10px;\r\n}\r\n.blueColor[_ngcontent-%COMP%] {\r\n    color: blue;\r\n    font-size: 20px;\r\n}\r\n.cal-month-view[_ngcontent-%COMP%]   .cal-day-badge[_ngcontent-%COMP%] {\r\n    background-color: #9db948;\r\n    color: #fff;\r\n    margin-bottom: 5px;\r\n}\r\n.icon-zoom[_ngcontent-%COMP%] {\r\n    padding: 5px;\r\n    transition: transform .2s; \r\n    margin: 0 auto;\r\n}\r\n.icon-zoom[_ngcontent-%COMP%]:hover {\r\n        transform: scale(1.2);\r\n    }\r\n.label-font[_ngcontent-%COMP%]{\r\n        font-size:15px;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2RyaXZlci1zY2hlZHVsZS1jYWxlbmRlci9kcml2ZXItc2NoZWR1bGUtY2FsZW5kZXIuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGNBQWM7QUFDbEI7QUFDQTtJQUNJLFdBQVc7SUFDWCxlQUFlO0FBQ25CO0FBQ0E7SUFDSSx5QkFBeUI7SUFDekIsV0FBVztJQUNYLGtCQUFrQjtBQUN0QjtBQUVBO0lBQ0ksWUFBWTtJQUNaLHlCQUF5QixFQUFFLGNBQWM7SUFDekMsY0FBYztBQUNsQjtBQUVJO1FBQ0kscUJBQXFCO0lBQ3pCO0FBRUE7UUFDSSxjQUFjO0lBQ2xCIiwiZmlsZSI6InNyYy9hcHAvZHJpdmVyL2RyaXZlci1zY2hlZHVsZS1jYWxlbmRlci9kcml2ZXItc2NoZWR1bGUtY2FsZW5kZXIuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5jYWxlbmRlci1ncmlke1xyXG4gICAgZm9udC1zaXplOjEwcHg7XHJcbn1cclxuLmJsdWVDb2xvciB7XHJcbiAgICBjb2xvcjogYmx1ZTtcclxuICAgIGZvbnQtc2l6ZTogMjBweDtcclxufVxyXG4uY2FsLW1vbnRoLXZpZXcgLmNhbC1kYXktYmFkZ2Uge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogIzlkYjk0ODtcclxuICAgIGNvbG9yOiAjZmZmO1xyXG4gICAgbWFyZ2luLWJvdHRvbTogNXB4O1xyXG59XHJcblxyXG4uaWNvbi16b29tIHtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIHRyYW5zaXRpb246IHRyYW5zZm9ybSAuMnM7IC8qIEFuaW1hdGlvbiAqL1xyXG4gICAgbWFyZ2luOiAwIGF1dG87XHJcbn1cclxuXHJcbiAgICAuaWNvbi16b29tOmhvdmVyIHtcclxuICAgICAgICB0cmFuc2Zvcm06IHNjYWxlKDEuMik7XHJcbiAgICB9XHJcblxyXG4gICAgLmxhYmVsLWZvbnR7XHJcbiAgICAgICAgZm9udC1zaXplOjE1cHg7XHJcbiAgICB9Il19 */"], changeDetection: 0 });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](DriverScheduleCalenderComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-driver-schedule-calender',
                templateUrl: './driver-schedule-calender.component.html',
                changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectionStrategy"].OnPush,
                styleUrls: ['./driver-schedule-calender.component.css']
            }]
    }], function () { return [{ type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"] }, { type: src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__["RouteInfoService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/driver/driver.module.ts":
/*!*****************************************!*\
  !*** ./src/app/driver/driver.module.ts ***!
  \*****************************************/
/*! exports provided: DriverModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverModule", function() { return DriverModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _driver_routing_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./driver-routing.module */ "./src/app/driver/driver-routing.module.ts");
/* harmony import */ var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./driver-schedule-calender/driver-schedule-calender.component */ "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");
/* harmony import */ var angular_calendar__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! angular-calendar */ "./node_modules/angular-calendar/__ivy_ngcc__/fesm2015/angular-calendar.js");
/* harmony import */ var angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! angular-calendar/date-adapters/date-fns */ "./node_modules/angular-calendar/date-adapters/date-fns/index.js");
/* harmony import */ var angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! angularx-flatpickr */ "./node_modules/angularx-flatpickr/__ivy_ngcc__/fesm2015/angularx-flatpickr.js");
/* harmony import */ var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./driver-management/driver-management.component */ "./src/app/driver/driver-management/driver-management.component.ts");
/* harmony import */ var _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./create-driver-schedule/create-driver-schedule.component */ "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts");
/* harmony import */ var _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./driver/driver.component */ "./src/app/driver/driver/driver.component.ts");
/* harmony import */ var _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./driver-management/view-driver/view-driver.component */ "./src/app/driver/driver-management/view-driver/view-driver.component.ts");
/* harmony import */ var _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./driver-management/create-driver/create-driver.component */ "./src/app/driver/driver-management/create-driver/create-driver.component.ts");
/* harmony import */ var _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ../modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ./create-trailer-schedule/create-trailer-schedule.component */ "./src/app/driver/create-trailer-schedule/create-trailer-schedule.component.ts");
/* harmony import */ var _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ./create-region-schedule/create-region.component */ "./src/app/driver/create-region-schedule/create-region.component.ts");




















class DriverModule {
}
DriverModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({ type: DriverModule });
DriverModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({ factory: function DriverModule_Factory(t) { return new (t || DriverModule)(); }, imports: [[
            _driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"],
            src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"],
            _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"],
            angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"].forRoot(),
            angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"].forRoot({
                provide: angular_calendar__WEBPACK_IMPORTED_MODULE_3__["DateAdapter"],
                useFactory: angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__["adapterFactory"]
            }),
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](DriverModule, { declarations: [_driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__["DriverManagementComponent"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__["CreateDriverScheduleComponent"], _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__["DriverComponent"], _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__["ViewDriverComponent"], _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverComponent"], _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__["CreateTrailerScheduleComponent"], _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__["CreateRegionComponent"]], imports: [_driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"],
        src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
        _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"],
        _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"], angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"], angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DriverModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [_driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__["DriverManagementComponent"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__["CreateDriverScheduleComponent"], _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__["DriverComponent"], _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__["ViewDriverComponent"], _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverComponent"], _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__["CreateTrailerScheduleComponent"], _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__["CreateRegionComponent"]],
                imports: [
                    _driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"],
                    src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"],
                    _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"],
                    angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"].forRoot(),
                    angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"].forRoot({
                        provide: angular_calendar__WEBPACK_IMPORTED_MODULE_3__["DateAdapter"],
                        useFactory: angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__["adapterFactory"]
                    }),
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/driver/driver/driver.component.ts":
/*!***************************************************!*\
  !*** ./src/app/driver/driver/driver.component.ts ***!
  \***************************************************/
/*! exports provided: DriverComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverComponent", function() { return DriverComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../driver-management/driver-management.component */ "./src/app/driver/driver-management/driver-management.component.ts");
/* harmony import */ var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../driver-schedule-calender/driver-schedule-calender.component */ "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");






function DriverComponent_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "app-driver-management");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function DriverComponent_div_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "app-driver-schedule-calender");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function DriverComponent_div_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class DriverComponent {
    constructor(regionService) {
        this.regionService = regionService;
        this.isDriverShow = true;
        this.isProfileShow = false;
    }
    ngOnInit() {
    }
    changeTab(tabClick) {
        this.isDriverShow = false;
        this.isProfileShow = false;
        if (tabClick === "DriverShow") {
            this.isDriverShow = true;
        }
        if (tabClick == "ProfileShow") {
            this.isProfileShow = true;
        }
    }
}
DriverComponent.??fac = function DriverComponent_Factory(t) { return new (t || DriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__["RegionService"])); };
DriverComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: DriverComponent, selectors: [["app-driver"]], decls: 15, vars: 5, consts: [["id", "driverManagement-Tab", 1, "small-tab"], ["role", "tablist", 1, "nav", "nav-tabs", "mb15"], [1, "nav-item"], ["id", "home-tab", "data-toggle", "tab", "href", "#driver", "role", "tab", "aria-controls", "home", "aria-selected", "true", 1, "nav-link", "active", "fs16", "mr15", 3, "click"], ["id", "profile-tab", "data-toggle", "tab", "href", "#schedule", "role", "tab", "aria-controls", "profile", "aria-selected", "false", 1, "nav-link", "fs16", "mr15", 3, "click"], [1, "tab-content"], ["id", "driver", 1, "tab-pane", "fade", "show", "active"], [4, "ngIf"], ["id", "schedule", 1, "tab-pane", "fade"], ["class", "loader", 4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function DriverComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "ul", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "li", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "a", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function DriverComponent_Template_a_click_3_listener() { return ctx.changeTab("DriverShow"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Drivers");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "li", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function DriverComponent_Template_a_click_6_listener() { return ctx.changeTab("ProfileShow"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Profile");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](10, DriverComponent_div_10_Template, 2, 0, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, DriverComponent_div_12_Template, 2, 0, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, DriverComponent_div_13_Template, 5, 0, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](14, "async");
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isDriverShow);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isProfileShow);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](14, 3, ctx.regionService.onLoadingChanged));
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__["DriverManagementComponent"], _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_4__["DriverScheduleCalenderComponent"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["AsyncPipe"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXIvZHJpdmVyLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-driver',
                templateUrl: './driver.component.html',
                styleUrls: ['./driver.component.css']
            }]
    }], function () { return [{ type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__["RegionService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/driver/models/DriverManagementModel.ts":
/*!********************************************************!*\
  !*** ./src/app/driver/models/DriverManagementModel.ts ***!
  \********************************************************/
/*! exports provided: DriverManagementModel, DriverViewModel, DriverShiftModel, ShiftDetailModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverManagementModel", function() { return DriverManagementModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverViewModel", function() { return DriverViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverShiftModel", function() { return DriverShiftModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ShiftDetailModel", function() { return ShiftDetailModel; });
/* harmony import */ var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/statelist.service */ "./src/app/statelist.service.ts");

class DriverManagementModel {
    constructor() {
        this.Drivers = [];
        this.LicenseTypes = [];
        this.TrailerTypes = [];
    }
}
class DriverViewModel {
    constructor() {
        this.TrailerTypeId = [];
        this.LicenseType = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
        this.TrailerType = {};
        this.Status = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
        this.ShiftId = [];
    }
}
class DriverShiftModel {
    constructor() {
        this.Shifts = [];
    }
}
class ShiftDetailModel {
}


/***/ }),

/***/ "./src/app/driver/models/DriverSchedule.ts":
/*!*************************************************!*\
  !*** ./src/app/driver/models/DriverSchedule.ts ***!
  \*************************************************/
/*! exports provided: DriverScheduleMapping, DriverSchedule, ConflictDates */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverScheduleMapping", function() { return DriverScheduleMapping; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverSchedule", function() { return DriverSchedule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ConflictDates", function() { return ConflictDates; });
class DriverScheduleMapping {
    constructor() {
        this.ScheduleList = [];
    }
}
class DriverSchedule {
    constructor() {
        this.RepeatDayList = [];
        this.RepeatDayStringList = [];
        this.selectedShifts = [];
        this.selectedRepeatList = [];
    }
}
class ConflictDates {
}


/***/ }),

/***/ "./src/app/driver/models/TrailerSchedule.ts":
/*!**************************************************!*\
  !*** ./src/app/driver/models/TrailerSchedule.ts ***!
  \**************************************************/
/*! exports provided: TrailerSchedule, TrailerShiftDetail */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerSchedule", function() { return TrailerSchedule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerShiftDetail", function() { return TrailerShiftDetail; });
class TrailerSchedule {
    constructor() {
        this.TrailerShiftDetail = [];
        this.RepeatDayList = [];
    }
}
class TrailerShiftDetail {
}


/***/ }),

/***/ "./src/app/driver/models/regionSchedule.ts":
/*!*************************************************!*\
  !*** ./src/app/driver/models/regionSchedule.ts ***!
  \*************************************************/
/*! exports provided: RegionScheduleViewModel, ShiftSchedule, RegionScheduleMappingViewModel, ShiftDetailViewModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RegionScheduleViewModel", function() { return RegionScheduleViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ShiftSchedule", function() { return ShiftSchedule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RegionScheduleMappingViewModel", function() { return RegionScheduleMappingViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ShiftDetailViewModel", function() { return ShiftDetailViewModel; });
class RegionScheduleViewModel {
    constructor() {
        this.RepeatDayList = [];
        this.RegionShiftDetail = [];
    }
}
class ShiftSchedule {
}
class RegionScheduleMappingViewModel {
    constructor() {
        this.RepeatDayList = [];
        this.ShiftDetail = [];
    }
}
class ShiftDetailViewModel {
}


/***/ }),

/***/ "./src/app/driver/services/driver.service.ts":
/*!***************************************************!*\
  !*** ./src/app/driver/services/driver.service.ts ***!
  \***************************************************/
/*! exports provided: DriverService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverService", function() { return DriverService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class DriverService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getShiftUrl = '/Settings/Profile/GetShifts';
        this.getAllDriversUrl = '/Settings/Profile/GetAllDrivers';
        this.postAddDriverUrl = '/Settings/Profile/AddDriver';
        this.postDeleteDriverUrl = '/Settings/Profile/DeleteInvitedUser';
        this.changeDriverStatusUrl = '/Settings/Profile/ChangeUserStatus?id=';
        this.getRegionsUrl = '/Supplier/Region/GetRegionsDdl';
        this.addTrailerScheduleUrl = '/Supplier/Dispatch/AddTrailerSchedule';
        this.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
    }
    getShifts() {
        return this.httpClient.get(this.getShiftUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getShifts', null)));
    }
    getAllDrivers() {
        return this.httpClient.get(this.getAllDriversUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAllDrivers', null)));
    }
    postAddDriver(driverModel) {
        return this.httpClient.post(this.postAddDriverUrl, driverModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('postAddDriver', null)));
    }
    postDeleteDriver(driverdelteInfo) {
        var data = { Id: driverdelteInfo.UserId, IsScheduleExists: driverdelteInfo.IsScheduleExists, ScheduleBuilderIdInfo: driverdelteInfo.ScheduleBuilderIdInfo };
        return this.httpClient.post(this.postDeleteDriverUrl, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('postDeleteDriver', null)));
    }
    changeDriverStatus(id, isActive) {
        return this.httpClient.get(this.changeDriverStatusUrl + id + "&isActive=" + isActive)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('changeDriverStatus', null)));
    }
    getRegions() {
        return this.httpClient.get(this.getRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getRegions', null)));
    }
    addTrailerSchedule(model) {
        return this.httpClient.post(this.addTrailerScheduleUrl, model, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('addTrailerSchedule', model)));
    }
}
DriverService.??fac = function DriverService_Factory(t) { return new (t || DriverService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"])); };
DriverService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({ token: DriverService, factory: DriverService.??fac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DriverService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"] }]; }, null); })();


/***/ })

}]);
//# sourceMappingURL=driver-driver-module-es2015.js.map