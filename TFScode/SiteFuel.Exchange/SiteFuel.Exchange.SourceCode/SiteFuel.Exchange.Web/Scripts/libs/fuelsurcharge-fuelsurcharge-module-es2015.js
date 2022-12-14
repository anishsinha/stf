(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["fuelsurcharge-fuelsurcharge-module"],{

/***/ "./src/app/company-addresses/region/service/region.service.ts":
/*!********************************************************************!*\
  !*** ./src/app/company-addresses/region/service/region.service.ts ***!
  \********************************************************************/
/*! exports provided: RegionService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RegionService", function() { return RegionService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class RegionService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.createUrl = '/Region/Create';
        this.updateUrl = '/Region/Update';
        this.deleteUrl = '/Region/Delete?id=';
        this.getRegionsUrl = '/Region/GetRegions';
        this.getSourceRegionsUrl = '/Region/GetSourceRegions';
        this.createSourceRegionUrl = '/Region/CreateSourceRegion';
        this.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        this.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        this.getJobsUrl = '/Region/GetJobs';
        this.getDriversUrl = '/Region/GetDrivers';
        this.getDispatchersUrl = '/Region/GetDispatchers';
        this.getTrailersUrl = '/Region/GetTrailers';
        this.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        this.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        this.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        this.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        this.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        this.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        this.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        this.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        this.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        this.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        this.getCarriersUrl = '/Region/GetCarriers';
        this.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        this.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        this.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        this.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        this.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        this.getFuelProductUrl = '/Region/GetMstFuelProducts';
        this.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        this.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
    }
    getJobs() {
        return this.httpClient.get(this.getJobsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobs', [])));
    }
    getDrivers() {
        return this.httpClient.get(this.getDriversUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
    }
    getRegionDrivers(regiondId) {
        return this.httpClient.get(this.getRegionDriversUrl + regiondId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
    }
    getCompanyShifts() {
        return this.httpClient.get(this.getCompanyShiftsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCompanyShifts', [])));
    }
    getRoutesByRegion(regionId) {
        return this.httpClient.get(this.getRouteByReginId + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetRouteInfoDetails', [])));
    }
    getDispatchers() {
        return this.httpClient.get(this.getDispatchersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDispatchers', [])));
    }
    getTrailers() {
        return this.httpClient.get(this.getTrailersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTrailers', [])));
    }
    getRegions() {
        return this.httpClient.get(this.getRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegions', null)));
    }
    createRegion(region) {
        return this.httpClient.post(this.createUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRegion', region)));
    }
    updateRegion(region) {
        return this.httpClient.post(this.updateUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateRegion', region)));
    }
    getSourceRegions() {
        return this.httpClient.get(this.getSourceRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSourceRegions', null)));
    }
    createSourceRegion(region) {
        return this.httpClient.post(this.createSourceRegionUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createSourceRegion', region)));
    }
    isSourceRegionAvailable(name, id) {
        return this.httpClient.get(this.isSourceRegionAvailableUrl + name + "&id=" + id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isSourceRegionAvailable', null)));
    }
    updateSourceRegion(region) {
        return this.httpClient.post(this.updateSourceRegionUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateSourceRegion', region)));
    }
    deleteRegion(id) {
        return this.httpClient.post(this.deleteUrl + id, id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRegion', id)));
    }
    deleteSourceRegion(id) {
        return this.httpClient.post(this.deleteSourceRegionUrl + id, id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteSourceRegion', id)));
    }
    getStates(countryId) {
        return this.httpClient.get(this.stateUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getStates', [])));
    }
    //for calender
    getShiftByDrivers(driverIds, scheduleType) {
        return this.httpClient.get(this.shiftByDriverUrl + driverIds + "&scheduleType=" + scheduleType)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getShiftByDrivers', [])));
    }
    getSchedulesByRegion(regionId, scheduleType) {
        return this.httpClient.get(this.getRegionSchedulsbyRegionIdUrl + regionId + "&scheduleType=" + scheduleType)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSchedulesByRegion', [])));
    }
    getRegionSchedule(regionId, routeId) {
        return this.httpClient.get(this.getRegionShiftMapping + regionId + "&routeId=" + routeId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegionSchedule', [])));
    }
    addDriverSchedule(model) {
        return this.httpClient.post(this.addDriverScheduleUrl, model, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', model)));
    }
    addRegionSchedule(model) {
        return this.httpClient.post(this.addRegionScheduleUrl, model, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRegionSchedule', model)));
    }
    updateDriverSchedule(data, date) {
        var postModel = { model: data, SelectedDate: date };
        return this.httpClient.post(this.updateDriverScheduleUrl, postModel, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', postModel)));
    }
    deleteDriverSchedule(data, date) {
        var postModel = { driverScheduleMappingViewModels: data, SelectedDate: date };
        return this.httpClient.post(this.deleteDriverScheduleUrl, postModel, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteDriverSchedule', postModel)));
    }
    getCarriers() {
        return this.httpClient.get(this.getCarriersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriers', [])));
    }
    getCarrierRegions() {
        return this.httpClient.get(this.getCarrierRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierRegions', null)));
    }
    getSelectedCarriersByRegion(regionId) {
        return this.httpClient.get(this.getSelectedCarriersByRegionUrl + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSelectedCarriersByRegion', null)));
    }
    getProductType() {
        return this.httpClient.get(this.getProductTypeUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductType', [])));
    }
    getFuelProducts() {
        return this.httpClient.get(this.getFuelProductUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelProducts', [])));
    }
    isPublishedDR(productIds, fuelTypeIds) {
        return this.httpClient.get(this.isPublishedDRUrl + productIds + "&fuelTypeIds=" + fuelTypeIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isPublishedDR', null)));
    }
}
RegionService.??fac = function RegionService_Factory(t) { return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"])); };
RegionService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({ token: RegionService, factory: RegionService.??fac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](RegionService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts":
/*!*************************************************************************!*\
  !*** ./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts ***!
  \*************************************************************************/
/*! exports provided: CreateFuelSurchargeComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateFuelSurchargeComponent", function() { return CreateFuelSurchargeComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_fuelsurcharge_models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/fuelsurcharge/models/CreateFuelSurcharge */ "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/fuelsurcharge/services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../../carrier/service/carrier.service */ "./src/app/carrier/service/carrier.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");




















function CreateFuelSurchargeComponent_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "button", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_div_2_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r21); const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r20.clearForm(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Create New");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_12_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_12_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r1.rcForm.get("TableName").errors.required);
} }
function CreateFuelSurchargeComponent_div_20_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_20_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.rcForm.get("TableTypes").errors.required);
} }
function CreateFuelSurchargeComponent_div_27_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_27_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r3.rcForm.get("Customers").errors.required);
} }
function CreateFuelSurchargeComponent_div_34_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_34_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_34_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.rcForm.get("Carriers").errors.required);
} }
function CreateFuelSurchargeComponent_div_42_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_42_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_42_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r5.rcForm.get("SourceRegions").errors.required);
} }
function CreateFuelSurchargeComponent_input_54_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "input", 29);
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "eia-period")("value", 1)("checked", ctx_r6.viewType == 1);
} }
function CreateFuelSurchargeComponent_label_55_Template(rf, ctx) { if (rf & 1) {
    const _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_label_55_Template_label_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r28); const ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r27.changeViewType(1); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "API Update");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_span_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_8_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_8_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r30.rcForm.get("FuelSurchargeProducts").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_15_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_15_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r32.rcForm.get("FuelSurchargePeriods").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_span_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_22_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_22_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r34.rcForm.get("FuelSurchargeAreas").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_input_40_Template(rf, ctx) { if (rf & 1) {
    const _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "input", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateFuelSurchargeComponent_div_61_input_40_Template_input_onDateChange_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r48); const ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r47.setApiAdjustIndexPriceDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx_r35.MinFromDate)("maxDate", ctx_r35.MaxStartDate)("mode", "WEEKLY")("daysOfWeekEnable", "MON");
} }
function CreateFuelSurchargeComponent_div_61_div_41_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_41_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_41_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r36.rcForm.get("ApiAdjustIndexPriceDate").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_42_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "ng-multiselect-dropdown", 68);
} if (rf & 2) {
    const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select month")("settings", ctx_r37.SingleSelectSettingsById)("data", ctx_r37.SourceMonthList);
} }
function CreateFuelSurchargeComponent_div_61_div_43_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_43_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_43_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r38.rcForm.get("SourceMonths").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_44_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "ng-multiselect-dropdown", 69);
} if (rf & 2) {
    const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Annually")("settings", ctx_r39.SingleSelectSettingsById)("data", ctx_r39.SourceAnnualyList);
} }
function CreateFuelSurchargeComponent_div_61_div_45_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_45_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_45_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r40.rcForm.get("SourceAnnualy").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_div_46_div_34_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_46_div_34_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_46_div_34_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r52.rcForm.get("Weeks").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_div_46_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Effective From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "input", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "label", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Mon");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "input", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "Tue");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](12, "input", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "label", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Wed");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](16, "input", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "label", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Thu");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](20, "input", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "label", 81);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Fri");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](24, "input", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "label", 83);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Sat");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](28, "input", 84);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "label", 85);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Sun");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](33, "ng-multiselect-dropdown", 87);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, CreateFuelSurchargeComponent_div_61_div_46_div_34_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](33);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select week")("settings", ctx_r41.SingleSelectSettingsById)("data", ctx_r41.WeekList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r41.rcForm.get("Weeks").invalid && ctx_r41.rcForm.get("Weeks").touched);
} }
function CreateFuelSurchargeComponent_div_61_div_47_div_6_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_47_div_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_47_div_6_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r54.rcForm.get("Months").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_div_47_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Effective From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "ng-multiselect-dropdown", 88);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, CreateFuelSurchargeComponent_div_61_div_47_div_6_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select month")("settings", ctx_r42.SingleSelectSettingsById)("data", ctx_r42.MonthList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r42.rcForm.get("Months").invalid && ctx_r42.rcForm.get("Months").touched);
} }
function CreateFuelSurchargeComponent_div_61_div_48_div_6_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_61_div_48_div_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_61_div_48_div_6_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r56.rcForm.get("Annualy").errors.required);
} }
function CreateFuelSurchargeComponent_div_61_div_48_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "label", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Effective From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "ng-multiselect-dropdown", 89);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, CreateFuelSurchargeComponent_div_61_div_48_div_6_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Annually")("settings", ctx_r43.SingleSelectSettingsById)("data", ctx_r43.AnnualyList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r43.rcForm.get("Annualy").invalid && ctx_r43.rcForm.get("Annualy").touched);
} }
function CreateFuelSurchargeComponent_div_61_Template(rf, ctx) { if (rf & 1) {
    const _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "label", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, CreateFuelSurchargeComponent_div_61_span_6_Template, 2, 0, "span", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "ng-multiselect-dropdown", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, CreateFuelSurchargeComponent_div_61_div_8_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](13, CreateFuelSurchargeComponent_div_61_span_13_Template, 2, 0, "span", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "ng-multiselect-dropdown", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateFuelSurchargeComponent_div_61_Template_ng_multiselect_dropdown_onSelect_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r59); const ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r58.onFuelSurchargePeriodsSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, CreateFuelSurchargeComponent_div_61_div_15_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, CreateFuelSurchargeComponent_div_61_span_20_Template, 2, 0, "span", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](21, "ng-multiselect-dropdown", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](22, CreateFuelSurchargeComponent_div_61_div_22_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "\u00A0");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "a", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_div_61_Template_a_click_29_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r59); const ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r60.onFetchLastIndexPrice(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Fetch Latest Index Price");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](32, "input", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "p", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](39, "Start of the Index Price");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](40, CreateFuelSurchargeComponent_div_61_input_40_Template, 1, 5, "input", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](41, CreateFuelSurchargeComponent_div_61_div_41_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](42, CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_42_Template, 1, 3, "ng-multiselect-dropdown", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](43, CreateFuelSurchargeComponent_div_61_div_43_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](44, CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_44_Template, 1, 3, "ng-multiselect-dropdown", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](45, CreateFuelSurchargeComponent_div_61_div_45_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](46, CreateFuelSurchargeComponent_div_61_div_46_Template, 35, 4, "div", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](47, CreateFuelSurchargeComponent_div_61_div_47_Template, 7, 4, "div", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](48, CreateFuelSurchargeComponent_div_61_div_48_Template, 7, 4, "div", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Product" : "NRC Product");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.SelectedCountryId == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select EIA Product")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargeProductList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.rcForm.get("FuelSurchargeProducts").invalid && ctx_r8.rcForm.get("FuelSurchargeProducts").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Period" : "NRC Period");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.SelectedCountryId == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select EIA Period")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargePeriodList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.rcForm.get("FuelSurchargePeriods").invalid && ctx_r8.rcForm.get("FuelSurchargePeriods").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Area" : "NRC Area");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.SelectedCountryId == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Area")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargeAreaList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.rcForm.get("FuelSurchargeAreas").invalid && ctx_r8.rcForm.get("FuelSurchargeAreas").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"](" ", ctx_r8.SelectedCountryId == 1 ? "U.S. Dollars per Gallon" : "Canada Cents per Litre ", " (Including Taxes) on ", ctx_r8.rcForm.get("IndexPriceDate").value, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsWeeklyVisible);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsWeeklyVisible && ctx_r8.rcForm.get("ApiAdjustIndexPriceDate").invalid && ctx_r8.rcForm.get("ApiAdjustIndexPriceDate").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsMonthlyVisible);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsMonthlyVisible && ctx_r8.rcForm.get("SourceMonths").invalid && ctx_r8.rcForm.get("SourceMonths").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsAnnualyVisible);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsAnnualyVisible && ctx_r8.rcForm.get("SourceAnnualy").invalid && ctx_r8.rcForm.get("SourceAnnualy").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsWeeklyVisible);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsMonthlyVisible);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.IsAnnualyVisible);
} }
function CreateFuelSurchargeComponent_div_62_div_8_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_62_div_8_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_62_div_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_62_div_8_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_62_div_8_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r61.rcForm.get("ManualLatestIndexPrice").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r61.rcForm.get("ManualLatestIndexPrice").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_62_div_18_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_62_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_62_div_18_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r62.rcForm.get("ManualEffectiveDate").errors.required);
} }
function CreateFuelSurchargeComponent_div_62_Template(rf, ctx) { if (rf & 1) {
    const _r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "\u00A0");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 90);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, CreateFuelSurchargeComponent_div_62_div_8_Template, 3, 2, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "p", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Effective date (from midnight UTC)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "input", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateFuelSurchargeComponent_div_62_Template_input_onDateChange_17_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r67); const ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r66.setManualEffectiveDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, CreateFuelSurchargeComponent_div_62_div_18_Template, 2, 1, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Notes");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](23, "textarea", 92);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.rcForm.get("ManualLatestIndexPrice").invalid && ctx_r9.rcForm.get("ManualLatestIndexPrice").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"](" ", ctx_r9.SelectedCountryId == 1 ? "U.S. Dollars per Gallon" : "Canada Cents per Litre ", " (Including Taxes) on ", ctx_r9.rcForm.get("IndexPriceDate").value, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx_r9.MinStartDate)("maxDate", ctx_r9.MaxStartDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.rcForm.get("ManualEffectiveDate").invalid && ctx_r9.rcForm.get("ManualEffectiveDate").touched);
} }
function CreateFuelSurchargeComponent_div_76_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_76_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_76_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r10.rcForm.controls.FuelSurchargeTable.get("StartDate").errors.required);
} }
function CreateFuelSurchargeComponent_div_95_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid range. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_96_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_96_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_96_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_96_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_96_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r12.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r12.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_106_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_106_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_106_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_106_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_106_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r13.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r13.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_111_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_111_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_111_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_111_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_111_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r14.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r14.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_126_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_126_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_126_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_126_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_126_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r15.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r15.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_132_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_132_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_132_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateFuelSurchargeComponent_div_132_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_132_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r16.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r16.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").errors.pattern);
} }
function CreateFuelSurchargeComponent_div_136_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Please click to generate surcharge table. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function CreateFuelSurchargeComponent_div_139_ng_container_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td", 97);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 98);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "span", 99);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "span", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](11, "%");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
} if (rf & 2) {
    const fst_r80 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate2"](" $", fst_r80.PriceRangeStartValue, " - $", fst_r80.PriceRangeEndValue, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fst_r80.FuelSurchargeStartPercentage);
} }
function CreateFuelSurchargeComponent_div_139_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "table", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "td", 94);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "Price Between");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "td", 95);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Fuel Surcharge %");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, CreateFuelSurchargeComponent_div_139_ng_container_8_Template, 12, 3, "ng-container", 96);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r18.rcForm.controls["GeneratedSurchargeTable"].value);
} }
function CreateFuelSurchargeComponent_div_146_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 101);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 103);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
const _c0 = function (a0) { return { "pntr-none": a0 }; };
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
class CreateFuelSurchargeComponent {
    constructor(fb, fuelsurchargeService, regionService, http, carrierService, cdr) {
        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.regionService = regionService;
        this.http = http;
        this.carrierService = carrierService;
        this.cdr = cdr;
        this.DtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        //public isLoadingSubject: BehaviorSubject<any>;
        this.SingleSelectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.IsLoading = false;
        this.SelectedCountryId = -1;
        this.TerminalsAndBulkPlantList = [];
        this.IsEditLoaded = true;
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.IsMonthlyVisible = false;
        this.IsWeeklyVisible = false;
        this.IsAnnualyVisible = false;
        this.IsGeneratedSurchargeTable = false;
        this.ShowMessage = false;
        this.viewType = 1;
        this.disableInputControls = false;
        //min max date
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.MinToDate = new Date();
        this.MinFromDate = new Date();
        this.decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
        this.SelectedTerminalsAndBulkPlants = [];
    }
    ngOnInit() {
        //this.isLoadingSubject = new BehaviorSubject(false);
        this.CurrentCompanyId = Number(currentUserCompanyId);
        this.getDefaultServingCountry();
        this.SingleSelectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.MultiSelectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.MultiSelectSettingsByGroup = {
            singleSelection: false,
            closeDropDownOnSelection: true,
            text: "Select Terminal(s) and Bulk Plant(s)",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };
        this.rcForm = this.createForm();
        this.getTableTypes();
        this.rcForm.controls['TableTypes'].patchValue([{ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master, Name: "Master" }]); // default will master
        this.IsMasterSelected = true;
        this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master.toString());
        var dt = moment__WEBPACK_IMPORTED_MODULE_7__(new Date()).toDate();
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 20);
        this.MinFromDate.setFullYear(this.MinFromDate.getFullYear() - 20);
        this.rcForm.controls.IndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__(dt).format('MM/DD/YYYY'));
        //this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment(dt).format('MM/DD/YYYY'));
        //default view type =1 so need to add required validations.
        this.AddRemoveValidations([this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate')], [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);
        this.fuelsurchargeService.onSelectedFuelSurchargeId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.fuelsurchargeId = stringify.FsurcharId;
                this.fuelsurchargeMode = stringify.Mode;
            }
        });
        // with order page integration
        let id = localStorage.getItem("FuelSurchargeTabId");
        if (id && +id > 0) {
            this.fuelsurchargeId = Number(id);
            this.fuelsurchargeMode = "VIEW";
            localStorage.removeItem("FuelSurchargeTabId");
        }
        var WeekList = [];
        var MonthList = [];
        var sourceMonthList = [];
        var AnnualyList = [];
        var sourceAnnualyList = [];
        for (let element in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"]) {
            WeekList.push({ Id: element, Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"][element], Code: "" });
        }
        this.WeekList = WeekList;
        this.rcForm.controls['Weeks'].setValue(this.WeekList.slice(0, 1));
        for (let element in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"]) {
            MonthList.push({ Id: element, Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"][element], Code: "" });
        }
        this.MonthList = MonthList;
        this.rcForm.controls['Months'].setValue(this.MonthList.slice(0, 1));
        this.IsWeeklyVisible = true;
        for (let i = 6; i >= -6; i--) {
            let m = new Date().setMonth(new Date().getMonth() + i, 1);
            sourceMonthList.push({ Id: moment__WEBPACK_IMPORTED_MODULE_7__(m).format(), Name: moment__WEBPACK_IMPORTED_MODULE_7__(m).format('MMMM YYYY'), Code: "" });
        }
        this.SourceMonthList = sourceMonthList;
        this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.slice(5, 6));
        for (let i = 1; i >= -1; i--) {
            let y = new Date().setFullYear(new Date().getFullYear() + i, 1);
            sourceAnnualyList.push({ Id: moment__WEBPACK_IMPORTED_MODULE_7__(y).format(), Name: moment__WEBPACK_IMPORTED_MODULE_7__(y).format('YYYY'), Code: "" });
        }
        this.SourceAnnualyList = sourceAnnualyList;
        this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.slice(0, 1));
        for (let element in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"]) {
            AnnualyList.push({ Id: element, Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"][element], Code: "" });
        }
        this.AnnualyList = AnnualyList;
        this.rcForm.controls['Annualy'].setValue(this.AnnualyList.slice(0, 1));
        this.rcForm.get('FuelSurchargePeriods').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('ApiAdjustIndexPriceDate').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('WeekDay').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('Weeks').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('Annualy').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('SourceAnnualy').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('Months').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        this.rcForm.get('SourceMonths').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.setValidFromDate();
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('SourceRegions').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.getTerminalsBulkPlant();
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('Carriers').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.onCarriersChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('Customers').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.onCustomersChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('SourceRegions').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded)
                this.SourceRegionChange(prev, next);
        });
        if (this.SelectedCountryId == 2) {
            this.changeViewType(this.SelectedCountryId);
        }
    }
    ngAfterViewInit() {
        if (this.fuelsurchargeId != null && this.fuelsurchargeId != undefined) {
            //this.isLoadingSubject = new BehaviorSubject(false);
            this.IsEditLoaded = false;
            this.getFuelSurchargeTable(this.fuelsurchargeId); //existing fuel charge.
        }
    }
    getDefaultServingCountry() {
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.SelectedCountryId = Number(data.DefaultCountryId);
            this.getFuelSurchargeProducts(this.SelectedCountryId);
            this.getFuelSurchargePeriods(this.SelectedCountryId);
            this.getFuelSurchargeArea(this.SelectedCountryId);
        });
    }
    modeChangeApiORmanualValidators(IsManualUpdate) {
        if (!IsManualUpdate) {
            var selectedTableType = this.rcForm.controls['TableTypes'].value;
            this.Fsmodel.TableTypeId = selectedTableType[0].Id;
            if (selectedTableType[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //, this.rcForm.get('Carriers')
            }
            else if (selectedTableType[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Customer) {
                this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
            }
            else {
                this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
            }
            this.AddRemoveValidations([this.rcForm.get('SourceRegions'), this.rcForm.get('TableTypes'), this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas')], [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);
            if (this.IsWeeklyVisible) {
                this.AddRemoveValidations([this.rcForm.get('ApiAdjustIndexPriceDate')], [this.rcForm.get('SourceAnnualy'), this.rcForm.get('SourceMonths')]);
            }
            else if (this.IsMonthlyVisible) {
                this.AddRemoveValidations([this.rcForm.get('SourceMonths')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceAnnualy')]);
            }
            else if (this.IsAnnualyVisible) {
                this.AddRemoveValidations([this.rcForm.get('SourceAnnualy')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths')]);
            }
        }
        if (IsManualUpdate) {
            this.rcForm.get('ManualLatestIndexPrice').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.get('ManualLatestIndexPrice').updateValueAndValidity();
            this.AddRemoveValidations([this.rcForm.get('ManualEffectiveDate')], [this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths'), this.rcForm.get('SourceAnnualy')]);
        }
    }
    modeChangePublishORdraftValidators(statusId) {
        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode
        if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Draft) {
            this.clearAllValidations(this.rcForm); // clear all validation
            this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for draft 
        }
        else if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published) {
            this.modeChangeApiORmanualValidators(this.rcForm.get('IsManualUpdate').value);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').updateValueAndValidity();
        }
    }
    changeViewType(value) {
        this.viewType = value;
        this.rcForm.get('IsManualUpdate').setValue(value == 1 ? false : true);
        this.modeChangeApiORmanualValidators(value == 1 ? false : true);
    }
    createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new src_app_fuelsurcharge_models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeIndexViewModel"]();
        }
        return this.fb.group({
            IsManualUpdate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.IsManualUpdate),
            ProductId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.ProductId),
            PeriodId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.PeriodId),
            TableTypeId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableTypeId),
            AreaId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.AreaId),
            FuelSurchargeIndexId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeIndexId),
            TableName: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableName, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required),
            Notes: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Notes),
            IndexPriceDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.IndexPriceDate),
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Customers),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Carriers),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.SourceRegions, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            WeekDay: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.WeekDay),
            Weeks: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Weeks),
            Months: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Months),
            SourceMonths: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.SourceMonths),
            Annualy: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Annualy),
            SourceAnnualy: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.SourceAnnualy),
            ApiEffectiveDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](''),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.SelectedTerminalsAndBulkPlants),
            FuelSurchargeProducts: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeProducts),
            FuelSurchargePeriods: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargePeriods),
            FuelSurchargeAreas: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeAreas),
            APILatestIndexPrice: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.APILatestIndexPrice),
            ApiAdjustIndexPriceDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.ApiAdjustIndexPriceDate),
            ManualLatestIndexPrice: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.ManualLatestIndexPrice),
            ManualEffectiveDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.ManualEffectiveDate),
            StatusId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.StatusId),
            FuelSurchargeTable: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroup"]({
                StartDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.StartDate, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
                EndDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.EndDate),
                PriceRangeStartValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.PriceRangeStartValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                PriceRangeEndValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.PriceRangeEndValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                PriceRangeInterval: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.PriceRangeInterval, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                FuelSurchargeStartPercentage: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.FuelSurchargeStartPercentage, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                SurchargeInterval: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelSurchargeTable.SurchargeInterval, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }),
            GeneratedSurchargeTable: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([
                new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.GeneratedSurchargeTable)
            ])
        }, { validator: RangeValidator });
    }
    //#region start : calander related functionality 
    setApiAdjustIndexPriceDate(event) {
        this.IsMonthlyVisible = false;
        this.IsWeeklyVisible = false;
        this.IsAnnualyVisible = false;
        if (event != "")
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(event);
        if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != "" && this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {
            let selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
            if (selectedPeriod != null && selectedPeriod != undefined && selectedPeriod.length > 0) {
                if (selectedPeriod[0].Name.toLowerCase() == "Weekly".toLowerCase()) {
                    this.IsWeeklyVisible = true;
                }
                else if (selectedPeriod[0].Name.toLowerCase() == "Monthly".toLowerCase()) {
                    this.IsMonthlyVisible = true;
                }
                else if (selectedPeriod[0].Name.toLowerCase() == "Annualy".toLowerCase()) {
                    this.IsAnnualyVisible = true;
                }
                this.setValidFromDate();
            }
        }
    }
    onFuelSurchargePeriodsSelect(item) {
        this.IsMonthlyVisible = false;
        this.IsWeeklyVisible = false;
        this.IsAnnualyVisible = false;
        //
        // if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {
        let selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
        if (selectedPeriod[0].Name.toLowerCase() == "Weekly".toLowerCase()) {
            this.IsWeeklyVisible = true;
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__().weekday(1).format('MM/DD/YYYY'));
        }
        if (selectedPeriod[0].Name.toLowerCase() == "Monthly".toLowerCase()) {
            this.IsMonthlyVisible = true;
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__().startOf('month').format('MM/DD/YYYY'));
        }
        if (selectedPeriod[0].Name.toLowerCase() == "Annualy".toLowerCase()) {
            this.IsAnnualyVisible = true;
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__().startOf('year').format('MM/DD/YYYY'));
        }
        //}
    }
    setManualEffectiveDate(event) {
        if (event != "") {
            this.rcForm.controls.ManualEffectiveDate.setValue(event);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(event).format('MM/DD/YYYY'));
        }
    }
    setStartDate(event) {
        this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(event);
        if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != "")
            this.MinToDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
    }
    setEndDate(event) {
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').setValue(event);
    }
    // end : calander related functionality
    onTableTypeDeSelect(item) {
        var selectedTableType = this.rcForm.get('TableTypes').value;
        if (selectedTableType.length == 0) {
            this.IsMasterSelected = true;
            this.rcForm.get('Carriers').patchValue([]);
            this.rcForm.get('Customers').patchValue([]);
            this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
            this.rcForm.get('SourceRegions').patchValue([]);
            this.IsSourceRegionSelected = false;
            this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'),
                this.rcForm.get('Carriers')]);
        }
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                break;
            case 2: // customer
                this.getCarriers();
                this.getSupplierCustomers();
                this.IsCustomerSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Customers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Carriers')]);
                break;
            case 3: //carrier
                this.getCarriers();
                this.getSupplierCustomers();
                this.IsCarrierSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Carriers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Customers')]);
                break;
        }
        this.rcForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    AddRemoveValidations(requiredControls, notRequiredControls) {
        if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {
            requiredControls.forEach(ctrl => {
                ctrl.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
                ctrl.updateValueAndValidity();
            });
        }
        if (notRequiredControls != null && notRequiredControls != undefined && notRequiredControls.length > 0) {
            notRequiredControls.forEach(ctrl => {
                ctrl.clearValidators();
                ctrl.updateValueAndValidity();
            });
        }
    }
    onCarriersChange(prev, next) {
        this.onCarriersOrCustomersChange(prev, next);
    }
    onCustomersChange(prev, next) {
        this.onCarriersOrCustomersChange(prev, next);
    }
    onCarriersOrCustomersChange(prev, next) {
        if (this.IsMasterSelected)
            return;
        this.rcForm.get('SourceRegions').patchValue([]);
        var selectedTableType = this.rcForm.get('TableTypes').value;
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    onFetchLastIndexPrice() {
        var selectedArea = this.rcForm.get('FuelSurchargeAreas').value;
        var selectedProduct = this.rcForm.get('FuelSurchargeProducts').value;
        var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
        //if (selectedProduct != undefined && selectedPeriod != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1)
        //    this.getFuelIndexPrice(selectedPeriod[0].Code, selectedProduct[0].Code, moment().format("MM-DD-YYYY"));
        if (selectedArea != undefined && selectedProduct != undefined && selectedPeriod != undefined && selectedArea != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1 && selectedArea.length == 1) {
            var selectedAreaCode = this.FuelSurchargeAreaList.filter(x => x.Id == selectedArea[0].Id)[0].Code;
            var selectedPeriodCode = this.FuelSurchargePeriodList.filter(x => x.Id == selectedPeriod[0].Id)[0].Code;
            this.getFuelIndexPrice(selectedPeriodCode, selectedProduct[0].Code, moment__WEBPACK_IMPORTED_MODULE_7__().format("MM-DD-YYYY"), selectedAreaCode);
        }
    }
    onGenerateSurchargeTable() {
        this.ShowMessage = false;
        this.IsGeneratedSurchargeTable = false;
        var gst = this.rcForm.controls['GeneratedSurchargeTable'];
        var fst = this.rcForm.controls['FuelSurchargeTable'];
        gst.clear();
        this.modeChangePublishORdraftValidators(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published);
        this.modeChangeApiORmanualValidators(this.rcForm.get("IsManualUpdate").value);
        this.markFormGroupTouched(this.rcForm);
        if (!fst.valid)
            return;
        this.IsLoading = true;
        this.fuelsurchargeService.getGenerateSurchargeTable(fst.get('PriceRangeStartValue').value, fst.get('PriceRangeEndValue').value, fst.get('PriceRangeInterval').value, fst.get('SurchargeInterval').value, fst.get('FuelSurchargeStartPercentage').value).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var dt = yield (data);
            dt.forEach(res => {
                gst.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]({
                    FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                    PriceRangeStartValue: res.PriceRangeStartValue,
                    PriceRangeEndValue: res.PriceRangeEndValue
                }));
            });
            this.DtTrigger.next();
            this.IsLoading = false;
        }));
        this.IsGeneratedSurchargeTable = true;
        //console.log(this.rcForm.controls['GeneratedSurchargeTable'].value)
    }
    markFormGroupTouched(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }
    clearAllControlValue(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.patchValue([]);
            ;
            if (control.controls) {
                this.clearAllControlValue(control);
            }
        });
    }
    clearAllValidations(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.clearValidators();
            control.updateValueAndValidity();
            control.markAsTouched();
            if (control.controls) {
                this.clearAllValidations(control);
            }
        });
    }
    onSubmit(fuelSurchageStatus) {
        this.ShowMessage = false;
        this.rcForm.get('StatusId').setValue(fuelSurchageStatus);
        this.modeChangeApiORmanualValidators(this.rcForm.get("IsManualUpdate").value);
        this.modeChangePublishORdraftValidators(fuelSurchageStatus);
        this.markFormGroupTouched(this.rcForm);
        if (!this.IsGeneratedSurchargeTable && fuelSurchageStatus == 2) {
            this.ShowMessage = true;
            return;
        }
        if (this.rcForm.valid) {
            this.Save(fuelSurchageStatus);
        }
    }
    clearForm() {
        this.rcForm.get('TableName').patchValue('');
        this.rcForm.get('SourceRegions').patchValue('');
        this.rcForm.get('TerminalsAndBulkPlants').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').patchValue('');
        this.AllowTableName = true;
        this.disableInputControls = false;
        this.IsGeneratedSurchargeTable = false;
        this.rcForm.get('TableName').clearValidators();
        this.rcForm.get('TableName').updateValueAndValidity();
        this.rcForm.get('SourceRegions').clearValidators();
        this.rcForm.get('SourceRegions').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').updateValueAndValidity();
    }
    onCancel() {
        if (this.fuelsurchargeMode != null) {
            this.changeToViewTab();
        }
        else {
            this.clearAllControlValue(this.rcForm);
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master)); // default will master
            this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.slice(0, 1));
            this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.slice(0, 1));
            this.IsGeneratedSurchargeTable = false;
            this.IsCustomerSelected = false;
            this.IsMasterSelected = true;
            this.IsCarrierSelected = false;
            this.IsSourceRegionSelected = false;
            this.IsAnnualyVisible = false;
            this.IsMonthlyVisible = false;
            this.rcForm.get('SourceMonths').setValue(null);
            this.rcForm.get('SourceAnnualy').setValue(null);
            this.rcForm.controls['WeekDay'].setValue("Mon");
            this.rcForm.controls['Weeks'].setValue(this.WeekList.slice(0, 1));
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__().weekday(1).format('MM/DD/YYYY'));
            this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.slice(0, 1));
            this.IsWeeklyVisible = true;
            this.rcForm.controls['FuelSurchargeTable'].get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__().weekday(1).format('MM/DD/YYYY'));
            this.changeViewType(1);
            this.ShowMessage = false;
        }
    }
    ngOnDestroy() {
        this.DtTrigger.unsubscribe();
    }
    //#region webserivce call
    getCarriers() {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe((carriers) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CarrierList = yield carriers;
            this.DtTrigger.next();
            this.IsLoading = false;
        }));
    }
    getTableTypes() {
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TableTypeList = yield (data);
            this.DtTrigger.next();
        }));
    }
    getSupplierCustomers() {
        this.IsLoading = true;
        this.fuelsurchargeService.getSupplierCustomers().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CustomerList = yield (data);
            this.DtTrigger.next();
            this.IsLoading = false;
        }));
    }
    //companyIds : Based on tableType we will be call API , if tableType master or customer or carrier, full source region,customers,carriers loads will populated.
    getSourceRegions(tableType) {
        let customerIds = [];
        let carrierIds = [];
        let selectedCustomers = this.rcForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }
        let selectedCarriers = this.rcForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SourceRegionInputModel"]();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.SourceRegionList = yield (data);
            this.SourceRegionList.sort((a, b) => (a.Name > b.Name) ? 1 : -1);
            this.DtTrigger.next();
        }));
    }
    getTerminalsBulkPlant() {
        this.IsLoading = true;
        var selectedSourceRegions = this.rcForm.get('SourceRegions').value;
        this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
        this.IsSourceRegionSelected = false;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null && selectedSourceRegions.length > 0) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.DtTrigger.next();
                this.IsLoading = false;
            }));
        }
        else {
            this.IsLoading = false;
        }
    }
    SourceRegionChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.IsLoading = true;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.IsSourceRegionSelected = false;
        var ids = [];
        let selectedSourceRegions = this.rcForm.get('SourceRegions').value;
        if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
                this.DtTrigger.next();
            }));
        }
        this.IsLoading = false;
    }
    getFuelSurchargeProducts(countryId) {
        this.fuelsurchargeService.getFuelSurchargeProducts(countryId).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.FuelSurchargeProductList = yield (data);
            this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.slice(0, 1));
            this.DtTrigger.next();
        }));
    }
    getFuelSurchargePeriods(countryId) {
        this.fuelsurchargeService.getFuelSurchargePeriod(countryId).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.FuelSurchargePeriodList = yield (data);
            this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.slice(0, 1));
            //var dt = moment(new Date()).toDate();
            //this.setApiAdjustIndexPriceDate(moment(dt).format('MM/DD/YYYY'));
            this.DtTrigger.next();
        }));
    }
    getFuelSurchargeArea(countryId) {
        ;
        this.fuelsurchargeService.getFuelSurchargeArea(countryId).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.FuelSurchargeAreaList = yield (data);
            this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.slice(0, 1));
            this.DtTrigger.next();
        }));
    }
    getFuelIndexPrice(periodId, productType, fetchDate, areaId) {
        this.IsLoading = true;
        if (this.SelectedCountryId == 1) {
            this.fuelsurchargeService.getEIAIndexPrice(periodId, productType, fetchDate, areaId).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.APILatestIndexPrice = yield (data);
                this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                this.DtTrigger.next();
                this.IsLoading = false;
            }));
        }
        else {
            this.fuelsurchargeService.getNRCIndexPrice(periodId, productType, fetchDate).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.APILatestIndexPrice = yield (data);
                this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                this.DtTrigger.next();
                this.IsLoading = false;
            }));
        }
    }
    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id;
            }).length == 1;
        };
    }
    //GET
    getFuelSurchargeTable(fuelSurchargeTableId) {
        //this.isLoadingSubject.next(true);;
        let sorceRegionIds = "";
        this.IsLoading = true;
        this.cdr.detectChanges();
        this.http.get(this.fuelsurchargeService.getFuelSurchargeTableUrl + fuelSurchargeTableId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["map"])(fsIndex => {
            const fsModel = fsIndex;
            return fsModel;
        }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["mergeMap"])(fsModel => {
            //this.isLoadingSubject.next(true);
            this.Fsmodel = fsModel;
            //let companyIds: number[] = [];
            if (this.fuelsurchargeId != null && this.fuelsurchargeMode.toUpperCase() == "COPY") { // on copy 
                this.Fsmodel.FuelSurchargeIndexId = null;
                this.Fsmodel.TableName = "";
            }
            const customers = this.http.get(this.fuelsurchargeService.getSupplierCustomersUrl);
            const carriers = this.http.get(this.regionService.getCarriersUrl);
            let customerIds = [];
            let carrierIds = [];
            if (this.Fsmodel.Customers.length > 0) {
                customerIds = this.Fsmodel.Customers.map(s => s.Id);
            }
            if (this.Fsmodel.Carriers.length > 0) {
                carrierIds = this.Fsmodel.Carriers.map(s => s.Id);
            }
            var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SourceRegionInputModel"]();
            sourceRegionInput.TableType = this.Fsmodel.TableTypeId.toString();
            sourceRegionInput.CustomerId = customerIds;
            sourceRegionInput.CarrierId = carrierIds;
            const sourceRegions = this.http.post(this.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);
            const tableTypes = this.http.get(this.fuelsurchargeService.getTableTypesUrl);
            if (this.Fsmodel.SourceRegions != null && this.Fsmodel.SourceRegions != undefined && this.Fsmodel.SourceRegions.length > 0) {
                sorceRegionIds = this.Fsmodel.SourceRegions.map(s => s.Id).join(',');
                this.IsSourceRegionSelected = true;
            }
            const terminalAndBulkPlans = this.http.get(this.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds);
            return Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["forkJoin"])([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
        })).subscribe(result => {
            this.IsLoading = false;
            //this.isLoadingSubject.next(true);
            this.IsMasterSelected = false;
            this.IsCustomerSelected = false;
            this.IsCarrierSelected = false;
            if (this.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
                this.IsMasterSelected = true;
            }
            else if (this.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Customer) {
                this.IsCustomerSelected = true;
            }
            else if (this.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Carrier) {
                this.IsCarrierSelected = true;
            }
            if (this.Fsmodel.TableTypeId != src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
                this.CustomerList = result[0];
                this.CarrierList = result[1];
            }
            this.SourceRegionList = result[2];
            if (this.Fsmodel.SourceRegions != null && this.Fsmodel.SourceRegions != undefined && this.Fsmodel.SourceRegions.length > 0) {
                this.TerminalsAndBulkPlantList = result[3];
            }
            this.TableTypeList = result[4];
            //this.isLoadingSubject.next(true);
            this.Edit(this.Fsmodel);
        });
    }
    //Edit
    Edit(_fs) {
        if (this.rcForm) {
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            this.IsWeeklyVisible = false;
            this.IsAnnualyVisible = false;
            this.IsMonthlyVisible = false;
            this.rcForm.controls['ProductId'].setValue(_fs.ProductId);
            this.rcForm.controls['PeriodId'].setValue(_fs.PeriodId);
            this.rcForm.controls['TableTypeId'].setValue(_fs.TableTypeId);
            this.rcForm.controls['AreaId'].setValue(_fs.AreaId);
            this.rcForm.controls['FuelSurchargeIndexId'].setValue(_fs.FuelSurchargeIndexId);
            this.rcForm.controls['TableName'].setValue(_fs.TableName);
            this.rcForm.controls['Notes'].setValue(_fs.Notes);
            this.rcForm.controls['IndexPriceDate'].setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.IndexPriceDate).format('MM/DD/YYYY'));
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == _fs.TableTypeId));
            _fs.IsManualUpdate ? this.changeViewType("2") : this.changeViewType("1");
            let stringify = JSON.parse(_fs.ApiEffectiveDate);
            if (!_fs.IsManualUpdate && stringify != null && stringify != undefined && stringify != "") {
                this.IsLoading = true;
                if (stringify.WeekDay != null && stringify.WeekDay != undefined && stringify.WeekDay != "") {
                    this.rcForm.controls['WeekDay'].setValue(stringify.WeekDay);
                    this.IsWeeklyVisible = true;
                }
                if (stringify.Weeks != null && stringify.Weeks != undefined && stringify.Weeks != "") {
                    let weeks = [];
                    weeks.push({ Id: JSON.parse(stringify.Weeks).Id, Name: JSON.parse(stringify.Weeks).Name, Code: "" });
                    this.rcForm.controls['Weeks'].setValue(weeks);
                    this.IsWeeklyVisible = true;
                }
                if (stringify.Months != null && stringify.Months != undefined && stringify.Months != "") {
                    let months = [];
                    months.push({ Id: JSON.parse(stringify.Months).Id, Name: JSON.parse(stringify.Months).Name, Code: "" });
                    this.rcForm.controls['Months'].setValue(months);
                    this.IsMonthlyVisible = true;
                }
                if (stringify.Annualy != null && stringify.Annualy != undefined && stringify.Annualy != "") {
                    let annualy = [];
                    annualy.push({ Id: JSON.parse(stringify.Annualy).Id, Name: JSON.parse(stringify.Annualy).Name, Code: "" });
                    this.rcForm.controls['Annualy'].setValue(annualy);
                    this.IsAnnualyVisible = true;
                }
            }
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            if (_fs.Customers != null && this.CustomerList != undefined && this.CustomerList != null) {
                if (this.CustomerList.length > 0 && _fs.Customers.length > 0)
                    this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_fs.Customers)));
            }
            if (_fs.Carriers != null && this.CarrierList != undefined && this.CarrierList != null) {
                if (this.CarrierList.length > 0 && _fs.Carriers.length > 0)
                    this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_fs.Carriers)));
            }
            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _fs.SourceRegions != null && _fs.SourceRegions != undefined && _fs.SourceRegions.length > 0) {
                if (this.SourceRegionList.length > 0 && _fs.SourceRegions.length > 0)
                    this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_fs.SourceRegions)));
            }
            if (this.TerminalsAndBulkPlantList != null && this.TerminalsAndBulkPlantList != undefined && _fs.TerminalsAndBulkPlants != null && _fs.TerminalsAndBulkPlants != undefined && _fs.TerminalsAndBulkPlants.length > 0) {
                if (this.TerminalsAndBulkPlantList.length > 0 && _fs.TerminalsAndBulkPlants.length > 0) {
                    this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList.filter(this.IdInComparer(_fs.TerminalsAndBulkPlants)));
                }
            }
            if (_fs.ProductId != null && this.FuelSurchargeProductList != null && this.FuelSurchargeProductList != undefined && this.FuelSurchargeProductList.length > 0) {
                this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.filter(x => x.Id == _fs.ProductId.toString()));
            }
            if (_fs.PeriodId != null && this.FuelSurchargePeriodList != null && this.FuelSurchargePeriodList != undefined && this.FuelSurchargePeriodList.length > 0) {
                this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.filter(x => x.Id == _fs.PeriodId.toString()));
            }
            if (_fs.AreaId != null && this.FuelSurchargeAreaList != null && this.FuelSurchargeAreaList != undefined && this.FuelSurchargeAreaList.length > 0) {
                this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.filter(x => x.Id == _fs.AreaId.toString()));
            }
            this.rcForm.controls['APILatestIndexPrice'].setValue(_fs.APILatestIndexPrice);
            if (_fs.ApiAdjustIndexPriceDate != null && _fs.ApiAdjustIndexPriceDate != undefined) {
                if (this.IsWeeklyVisible) {
                    this.rcForm.controls['ApiAdjustIndexPriceDate'].setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('MM/DD/YYYY'));
                }
                else if (this.IsMonthlyVisible) {
                    this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.filter(x => x.Name == moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('MMMM YYYY')));
                }
                else if (this.IsAnnualyVisible) {
                    this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.filter(x => x.Name == moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('YYYY')));
                }
            }
            this.rcForm.controls['ManualLatestIndexPrice'].setValue(_fs.ManualLatestIndexPrice);
            if (_fs.ManualEffectiveDate != null && _fs.ManualEffectiveDate != undefined) {
                this.rcForm.controls['ManualEffectiveDate'].setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ManualEffectiveDate).format('MM/DD/YYYY'));
            }
            else {
                this.rcForm.get('ManualEffectiveDate').patchValue([]);
            }
            this.rcForm.controls['StatusId'].setValue(_fs.StatusId);
            if (_fs.FuelSurchargeTable != null && _fs.FuelSurchargeTable != undefined) {
                if (_fs.FuelSurchargeTable.StartDate != null && _fs.FuelSurchargeTable.StartDate != undefined) {
                    this.rcForm.controls['FuelSurchargeTable'].get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.FuelSurchargeTable.StartDate).format('MM/DD/YYYY'));
                }
                else {
                    this.rcForm.controls['FuelSurchargeTable'].get('StartDate').patchValue([]);
                }
                if (_fs.FuelSurchargeTable.EndDate != null && _fs.FuelSurchargeTable.EndDate != undefined) {
                    this.rcForm.controls['FuelSurchargeTable'].get('EndDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.FuelSurchargeTable.EndDate).format('MM/DD/YYYY'));
                }
                else {
                    this.rcForm.controls['FuelSurchargeTable'].get('EndDate').patchValue([]);
                }
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeStartValue').setValue(_fs.FuelSurchargeTable.PriceRangeStartValue);
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeEndValue').setValue(_fs.FuelSurchargeTable.PriceRangeEndValue);
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeInterval').setValue(_fs.FuelSurchargeTable.PriceRangeInterval);
                this.rcForm.controls['FuelSurchargeTable'].get('FuelSurchargeStartPercentage').setValue(_fs.FuelSurchargeTable.FuelSurchargeStartPercentage);
                this.rcForm.controls['FuelSurchargeTable'].get('SurchargeInterval').setValue(_fs.FuelSurchargeTable.SurchargeInterval);
            }
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            var gst = this.rcForm.controls['GeneratedSurchargeTable'];
            if (_fs.GeneratedSurchargeTable != null && _fs.GeneratedSurchargeTable.length > 0) {
                gst.clear();
                _fs.GeneratedSurchargeTable.forEach(res => {
                    gst.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]({
                        FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                        PriceRangeStartValue: res.PriceRangeStartValue,
                        PriceRangeEndValue: res.PriceRangeEndValue
                    }));
                });
            }
            this.onGenerateSurchargeTable();
            this.IsEditLoaded = true;
            //this.isLoadingSubject.next(false);
            this.IsLoading = false;
        }
        if (this.fuelsurchargeMode == "VIEW") {
            this.disableInputControls = true;
        }
        else {
            this.disableInputControls = false;
        }
        this.AllowTableName = false;
    }
    nextDate(givenDate, dayIndex) {
        var today = new Date(givenDate);
        //var a = today.getDate();
        //var e = today.getDay();
        //var b = (dayIndex - 1 - today.getDay() + 7);
        //var c = b % 7;
        //var d = today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1;
        //don't find next date in case in edage case . like on same day and having fine arrangement not to crash when more than 1 week, and arrangment is to first add required days and than call current method.
        if (dayIndex != today.getDay())
            today.setDate(today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1);
        return today;
    }
    setValidFromDate() {
        this.IsLoading = true;
        var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
        var selectedSourceMonth = this.rcForm.get('SourceMonths').value;
        var selectedSourceAnnualy = this.rcForm.get('SourceAnnualy').value;
        var ApiAdjustIndexPriceDate = this.rcForm.controls.ApiAdjustIndexPriceDate.value;
        //if (!this.IsWeeklyVisible) this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(null);
        // this.AddRemoveValidations(null,[this.rcForm.get('Weeks'), this.rcForm.get('Months'), this.rcForm.get('Annualy')]);
        if (selectedPeriod != null && selectedPeriod != undefined && selectedPeriod.length > 0) {
            let effectiveDate = new Date();
            if (ApiAdjustIndexPriceDate != null && ApiAdjustIndexPriceDate != undefined && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].WEEKLY) {
                this.AddRemoveValidations([this.rcForm.get('Weeks')], [this.rcForm.get('Months'), this.rcForm.get('Annualy')]);
                effectiveDate = this.rcForm.controls.ApiAdjustIndexPriceDate.value;
                effectiveDate = new Date(effectiveDate);
                let WeekDay = this.IsWeeklyVisible && this.rcForm.controls.WeekDay.value != null && this.rcForm.controls.WeekDay.value != undefined && this.rcForm.controls.WeekDay.value != "" ? this.rcForm.controls.WeekDay.value : "";
                let Weeks = this.IsWeeklyVisible && this.rcForm.controls.Weeks.value != null && this.rcForm.controls.Weeks.value != undefined && this.rcForm.controls.Weeks.value != "" ? this.rcForm.controls.Weeks.value : "";
                var selectedWeeks = this.rcForm.get('Weeks').value;
                if (selectedWeeks != null && selectedWeeks != undefined && selectedWeeks.length > 0) {
                    if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Same_Week) {
                        // default will handle
                    }
                    else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_1) {
                        effectiveDate.setDate(effectiveDate.getDate() + 7);
                    }
                    else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_2) {
                        effectiveDate.setDate(effectiveDate.getDate() + 14);
                    }
                    else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_3) {
                        effectiveDate.setDate(effectiveDate.getDate() + 21);
                    }
                    if (WeekDay != "" && Weeks != "") {
                        switch (WeekDay.toUpperCase()) {
                            case "SUN": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Sun);
                                break;
                            }
                            case "MON": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Mon);
                                break;
                            }
                            case "TUE": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Tue);
                                break;
                            }
                            case "WED": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Wed);
                                break;
                            }
                            case "THU": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Thu);
                                break;
                            }
                            case "FRI": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Fri);
                                break;
                            }
                            case "SAT": {
                                effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Sat);
                                break;
                            }
                            default: {
                                //statements; 
                                break;
                            }
                        }
                        this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                        this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                        this.IsWeeklyVisible = true;
                    }
                }
            }
            else if (selectedSourceMonth != null && selectedSourceMonth != undefined && selectedSourceMonth.length > 0 && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].MONTHLY) {
                var selectedSourceMonth = this.rcForm.get('SourceMonths').value;
                this.AddRemoveValidations([this.rcForm.get('Months')], [this.rcForm.get('Weeks'), this.rcForm.get('Annualy')]);
                effectiveDate = new Date(selectedSourceMonth[0].Id);
                var selectedMonths = this.rcForm.get('Months').value;
                if (selectedMonths != null && selectedMonths != undefined && selectedMonths.length > 0) {
                    if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_1) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 1, 1);
                    }
                    else if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_2) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 2, 1);
                    }
                    else if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_3) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 3, 1);
                    }
                    this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                    this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                    this.IsMonthlyVisible = true;
                }
            }
            else if (selectedSourceAnnualy != null && selectedSourceAnnualy != undefined && selectedSourceAnnualy.length > 0 && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].ANNUALY) {
                effectiveDate = new Date(selectedSourceAnnualy[0].Id);
                this.AddRemoveValidations([this.rcForm.get('Annualy')], [this.rcForm.get('Weeks'), this.rcForm.get('Months')]);
                var selectedYear = this.rcForm.get('Annualy').value;
                if (selectedYear != null && selectedYear != undefined && selectedYear.length > 0) {
                    if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_1) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 1, 0, 1);
                    }
                    else if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_2) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 2, 0, 1);
                    }
                    else if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_3) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 3, 0, 1);
                    }
                    this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                    this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                    this.IsAnnualyVisible = true;
                }
            }
        }
        this.IsLoading = false;
    }
    //Save
    Save(fuelSurchageStatus) {
        this.IsLoading = true;
        this.Fsmodel = this.rcForm.value;
        if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate.value != undefined && this.rcForm.controls.ApiAdjustIndexPriceDate.value != "")
            this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(this.rcForm.controls.ApiAdjustIndexPriceDate.value).format());
        if (this.rcForm.controls.ManualEffectiveDate.value != null && this.rcForm.controls.ManualEffectiveDate.value != undefined)
            this.Fsmodel.ManualEffectiveDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(this.rcForm.controls.ManualEffectiveDate.value).format());
        if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined)
            this.Fsmodel.FuelSurchargeTable.StartDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
        if (this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != undefined)
            this.Fsmodel.FuelSurchargeTable.EndDate = this.rcForm.controls.FuelSurchargeTable.get('EndDate').value;
        if (this.Fsmodel.TerminalsAndBulkPlants != null || this.Fsmodel.TerminalsAndBulkPlants != undefined)
            this.Fsmodel.TerminalsAndBulkPlants.forEach(res => {
                res.Code = this.TerminalsAndBulkPlantList.find(c => c.Id == res.Id && c.Name == res.Name).Code;
            });
        if (this.IsMonthlyVisible) {
            var sourceMonths = this.rcForm.get('SourceMonths').value;
            if (sourceMonths != null && sourceMonths.length > 0) {
                this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(sourceMonths[0].Id).format());
            }
            else {
                this.Fsmodel.ApiAdjustIndexPriceDate = null;
            }
        }
        if (this.IsAnnualyVisible) {
            var sourceAnnualy = this.rcForm.get('SourceAnnualy').value;
            if (sourceAnnualy != null && sourceAnnualy.length > 0) {
                this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(sourceAnnualy[0].Id).format());
            }
            else {
                this.Fsmodel.ApiAdjustIndexPriceDate = null;
            }
        }
        this.Fsmodel.IndexPriceDate = this.rcForm.get('IndexPriceDate').value;
        this.Fsmodel.IsManualUpdate = this.viewType == 2 ? true : false;
        this.Fsmodel.ProductId = null;
        this.Fsmodel.PeriodId = null;
        this.Fsmodel.TableTypeId = null;
        this.Fsmodel.AreaId = null;
        this.Fsmodel.ApiEffectiveDate = "";
        if (!this.Fsmodel.IsManualUpdate) {
            var selectedProduct = this.rcForm.get('FuelSurchargeProducts').value;
            var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
            var selectedArea = this.rcForm.get('FuelSurchargeAreas').value;
            this.Fsmodel.ProductId = selectedProduct != null && selectedProduct != undefined ? selectedProduct[0].Id : null;
            this.Fsmodel.PeriodId = selectedPeriod != null && selectedPeriod != undefined ? +selectedPeriod[0].Id : null;
            this.Fsmodel.AreaId = selectedArea != null && selectedArea != undefined ? selectedArea[0].Id : null;
            let ApiEffectiveDate = {
                "WeekDay": this.IsWeeklyVisible && this.rcForm.controls.WeekDay.value != null && this.rcForm.controls.WeekDay.value != undefined && this.rcForm.controls.WeekDay.value != "" ? this.rcForm.controls.WeekDay.value : "",
                "Weeks": this.IsWeeklyVisible && this.rcForm.controls.Weeks.value != null && this.rcForm.controls.Weeks.value != undefined && this.rcForm.controls.Weeks.value != "" ? JSON.stringify(this.rcForm.controls.Weeks.value[0]) : "",
                "Months": this.IsMonthlyVisible && this.rcForm.controls.Months.value != null && this.rcForm.controls.Months.value != undefined && this.rcForm.controls.Months.value != "" ? JSON.stringify(this.rcForm.controls.Months.value[0]) : "",
                "Annualy": this.IsAnnualyVisible && this.rcForm.controls.Annualy.value != null && this.rcForm.controls.Annualy.value != undefined && this.rcForm.controls.Annualy.value != "" ? JSON.stringify(this.rcForm.controls.Annualy.value[0]) : ""
            };
            this.Fsmodel.ApiEffectiveDate = JSON.stringify(ApiEffectiveDate);
        }
        var selectedTableType = this.rcForm.get('TableTypes').value;
        this.Fsmodel.TableTypeId = selectedTableType[0].Id;
        this.Fsmodel.StatusId = fuelSurchageStatus;
        if (!this.IsGeneratedSurchargeTable)
            this.Fsmodel.GeneratedSurchargeTable = null;
        this.fuelsurchargeService.createFuelSurcharge(this.Fsmodel)
            .subscribe((response) => {
            this.ServiceResponse = response;
            if (response != null && response.StatusCode == 0) {
                let message = "";
                if (this.Fsmodel.FuelSurchargeIndexId != null) {
                    message = "Fuel Surcharge table edit successfully.";
                }
                else if (this.Fsmodel.StatusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published) {
                    message = "Fuel Surcharge table created successfully.";
                }
                else if (this.Fsmodel.StatusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Draft) {
                    message = "Fuel Surcharge saved draft successfully.";
                }
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(message, undefined, undefined);
                this.IsLoading = false;
                this.changeToViewTab();
            }
            else {
                this.IsLoading = false;
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
        });
    }
    changeToViewTab() {
        this.fuelsurchargeService.onSelectedTabChanged.next(2);
    }
}
CreateFuelSurchargeComponent.??fac = function CreateFuelSurchargeComponent_Factory(t) { return new (t || CreateFuelSurchargeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"])); };
CreateFuelSurchargeComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: CreateFuelSurchargeComponent, selectors: [["app-create-fuel-surcharge"]], decls: 147, vars: 63, consts: [[3, "formGroup", "ngSubmit"], [4, "ngIf"], [3, "ngClass", "disabled"], [1, "well", "bg-white"], [1, "row"], [1, "col-sm-3", "form-group"], [1, "color-maroon"], ["type", "text", "formControlName", "TableName", 1, "form-control", 3, "readonly"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "placeholder", "settings", "data"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "placeholder", "settings", "data"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "placeholder", "settings", "data"], [1, "col-sm-6"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], [1, "my-3"], [1, "col-sm-12"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "float-left", "mb10"], [1, "btn-group", "btn-filter"], ["class", "hide-element", "type", "radio", 3, "name", "value", "checked", 4, "ngIf"], ["class", "btn ml0", 3, "click", 4, "ngIf"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], ["formGroupName", "FuelSurchargeTable", 1, "row"], [1, "col-sm-4"], [1, "font-weight-bold", "fs14"], [1, "col"], ["type", "text", "formControlName", "StartDate", "readonly", "readonly", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "formControlName", "EndDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], [1, "input-group", "mb-3"], ["type", "text", "formControlName", "PriceRangeStartValue", 1, "form-control"], [1, "input-group-append"], [1, "input-group-text", "fs11"], ["type", "text", "formControlName", "PriceRangeEndValue", 1, "form-control"], ["type", "text", "formControlName", "PriceRangeInterval", 1, "form-control"], ["type", "text", "formControlName", "FuelSurchargeStartPercentage", 1, "form-control"], ["type", "text", "formControlName", "SurchargeInterval", 1, "form-control"], ["type", "button", "value", "Generate Surcharge Table", 1, "btn", "btn-primary", "ml-0", 3, "click"], [1, "col-sm-4", 3, "formArrayName"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "button", 1, "btn", "btn-lg", "btn-outline-primary", 3, "disabled", "click"], ["type", "submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled"], ["class", "loader", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], ["for", "FuelSurchargeProducts"], ["formControlName", "FuelSurchargeProducts", "id", "FuelSurchargeProducts", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "FuelSurchargePeriods", "id", "FuelSurchargePeriods", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect"], ["formControlName", "FuelSurchargeAreas", "id", "FuelSurchargeAreas", 1, "single-select", 3, "placeholder", "settings", "data"], [1, "form-row"], [1, "col-7"], [1, "btn", "btn-default", "ml-0", 3, "click"], ["type", "text", "readonly", "readonly", "formControlName", "APILatestIndexPrice", 1, "form-control"], [1, "text-black-50"], ["type", "text", "formControlName", "ApiAdjustIndexPriceDate", "class", "form-control datepicker", "placeholder", "Date", "CustomDatePicker", "", 3, "format", "minDate", "maxDate", "mode", "daysOfWeekEnable", "onDateChange", 4, "ngIf"], ["formControlName", "SourceMonths", "class", "single-select", "id", "SourceMonthList", 3, "placeholder", "settings", "data", 4, "ngIf"], ["formControlName", "SourceAnnualy", "class", "single-select", "id", "idSourceAnnualy", 3, "placeholder", "settings", "data", 4, "ngIf"], ["class", "col-sm-6", 4, "ngIf"], ["type", "text", "formControlName", "ApiAdjustIndexPriceDate", "placeholder", "Date", "CustomDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "mode", "daysOfWeekEnable", "onDateChange"], ["formControlName", "SourceMonths", "id", "SourceMonthList", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "SourceAnnualy", "id", "idSourceAnnualy", 1, "single-select", 3, "placeholder", "settings", "data"], [1, "d-block"], [1, "form-check", "form-check-inline"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-mon", "value", "Mon", 1, "form-check-input"], ["for", "occurance-mon", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-tue", "value", "Tue", 1, "form-check-input"], ["for", "occurance-tue", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-wed", "value", "Wed", 1, "form-check-input"], ["for", "occurance-wed", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-thu", "value", "Thu", 1, "form-check-input"], ["for", "occurance-thu", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-fri", "value", "Fri", 1, "form-check-input"], ["for", "occurance-fri", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-sat", "value", "Sat", 1, "form-check-input"], ["for", "occurance-sat", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-sun", "value", "Sun", 1, "form-check-input"], ["for", "occurance-sun", 1, "form-check-label"], [1, "row", "mt-2"], ["formControlName", "Weeks", "id", "idWeeks", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "Months", "id", "idMonths", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "Annualy", "id", "idAnnualy", 1, "single-select", 3, "placeholder", "settings", "data"], ["type", "text", "formControlName", "ManualLatestIndexPrice", 1, "form-control"], ["type", "text", "formControlName", "ManualEffectiveDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["formControlName", "Notes", 1, "form-control"], [1, "table", "table-bordered", "table-hover", "mt-3"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [4, "ngFor", "ngForOf"], [1, "text-center", "vmiddle"], [1, "input-group"], [1, "p-2", "border", "px-4"], [1, "input-group-text"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateFuelSurchargeComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngSubmit", function CreateFuelSurchargeComponent_Template_form_ngSubmit_1_listener() { return ctx.onSubmit(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateFuelSurchargeComponent_div_2_Template, 4, 0, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "fieldset", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Table Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](11, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, CreateFuelSurchargeComponent_div_12_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Table Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateFuelSurchargeComponent_Template_ng_multiselect_dropdown_onSelect_19_listener($event) { return ctx.onTableTypeSelect($event); })("onDeSelect", function CreateFuelSurchargeComponent_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) { return ctx.onTableTypeDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, CreateFuelSurchargeComponent_div_20_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](26, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, CreateFuelSurchargeComponent_div_27_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](33, "ng-multiselect-dropdown", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, CreateFuelSurchargeComponent_div_34_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](41, "ng-multiselect-dropdown", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](42, CreateFuelSurchargeComponent_div_42_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "div", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "label", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](47, "Terminal(s)/BulkPlant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](48, "angular2-multiselect", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](49, "hr", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](54, CreateFuelSurchargeComponent_input_54_Template, 1, 3, "input", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](55, CreateFuelSurchargeComponent_label_55_Template, 2, 0, "label", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](56, "input", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "label", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_Template_label_click_57_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](58, "Manual Update");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](61, CreateFuelSurchargeComponent_div_61_Template, 49, 29, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](62, CreateFuelSurchargeComponent_div_62_Template, 24, 7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](63, "hr", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "h3");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](65, "Fuel Surcharge Table");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "div", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "h5", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](69, "Valid");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](70, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](71, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](72, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](74, "From");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](75, "input", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateFuelSurchargeComponent_Template_input_onDateChange_75_listener($event) { return ctx.setStartDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](76, CreateFuelSurchargeComponent_div_76_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](77, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](78, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](79, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](80, "To");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](81, "input", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateFuelSurchargeComponent_Template_input_onDateChange_81_listener($event) { return ctx.setEndDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](82, "div", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](83, "h5", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](84, "Price Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](85, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](86, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](87, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](88, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](89, "Start Value");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](90, "div", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](91, "input", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](92, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](93, "span", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](94);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](95, CreateFuelSurchargeComponent_div_95_Template, 2, 0, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](96, CreateFuelSurchargeComponent_div_96_Template, 3, 2, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](97, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](98, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](99, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](100, "End Value");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](101, "div", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](102, "input", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](103, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](104, "span", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](105);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](106, CreateFuelSurchargeComponent_div_106_Template, 3, 2, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](107, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](108, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](109, "Interval");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](110, "input", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](111, CreateFuelSurchargeComponent_div_111_Template, 3, 2, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](112, "div", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](113, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](114, "h5", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](115, "Surcharge");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](116, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](117, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](118, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](119, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](120, "Start %");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](121, "div", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](122, "input", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](123, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](124, "span", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](125, "%");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](126, CreateFuelSurchargeComponent_div_126_Template, 3, 2, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](127, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](128, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](129, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](130, "Interval");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](131, "input", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](132, CreateFuelSurchargeComponent_div_132_Template, 3, 2, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](133, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](134, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](135, "input", 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_Template_input_click_135_listener() { return ctx.onGenerateSurchargeTable(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](136, CreateFuelSurchargeComponent_div_136_Template, 2, 0, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](137, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](138, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](139, CreateFuelSurchargeComponent_div_139_Template, 9, 1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](140, "div", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](141, "input", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_Template_input_click_141_listener() { return ctx.onCancel(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](142, "button", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateFuelSurchargeComponent_Template_button_click_142_listener() { return ctx.onSubmit(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](143, "Save Draft");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](144, "button", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](145, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](146, CreateFuelSurchargeComponent_div_146_Template, 5, 0, "div", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.rcForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.fuelsurchargeMode == "VIEW" || ctx.fuelsurchargeMode == "COPY" || ctx.fuelsurchargeMode == "EDIT");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](55, _c0, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", ctx.fuelsurchargeMode == "EDIT" && !ctx.AllowTableName ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("TableName").invalid && ctx.rcForm.get("TableName").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Table Type")("settings", ctx.SingleSelectSettingsById)("data", ctx.TableTypeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("TableTypes").invalid && ctx.rcForm.get("TableTypes").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](57, _c1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Customers(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.CustomerList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Customers").invalid && ctx.rcForm.get("Customers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](59, _c1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Carriers(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.CarrierList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsCarrierSelected && ctx.rcForm.get("Carriers").invalid && ctx.rcForm.get("Carriers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Source Regions(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.SourceRegionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("SourceRegions").invalid && ctx.rcForm.get("SourceRegions").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](61, _c1, !ctx.IsSourceRegionSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx.TerminalsAndBulkPlantList)("settings", ctx.MultiSelectSettingsByGroup);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.SelectedCountryId != 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.SelectedCountryId != 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "eia-period")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 1 && ctx.SelectedCountryId != 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 2 || ctx.SelectedCountryId == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("StartDate").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("StartDate").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.MinToDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.SelectedCountryId == 1 ? "USD" : "CAD");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx.rcForm.valid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").value > ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").value && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").touched && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.SelectedCountryId == 1 ? "USD" : "CAD");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.ShowMessage);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formArrayName", "GeneratedSurchargeTable");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsGeneratedSurchargeTable);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_14__["MultiSelectComponent"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArrayName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__["CustomDatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RadioControlValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"]], styles: [".tabs label {\r\n    background: none;\r\n    font-size: 16px;\r\n    border-bottom: 2px solid #f7f7f7;\r\n}\r\n\r\n  .tabs input[type=\"radio\"]:checked + label {\r\n    background: none;\r\n    border-bottom: 2px solid blue;\r\n    color: #0c52b1;\r\n    border-bottom: 2px solid #0c52b1;\r\n    font-size:16px;\r\n}\r\n\r\n  .custom-search {\r\n    left: 0;\r\n    top:0;\r\n}\r\n\r\n  .custom-search .card {\r\n        border:0;\r\n    }\r\n\r\n  .custom-search .card-body {\r\n        padding:0;\r\n    }\r\n\r\n  .custom-search .card-body li:hover {\r\n            background: #007bff;\r\n            cursor:pointer;\r\n            color:white;\r\n        }\r\n\r\n  .custom-search .card .card-header{\r\n        padding: 0 15px;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZnVlbHN1cmNoYXJnZS9DcmVhdGUvY3JlYXRlLWZ1ZWwtc3VyY2hhcmdlLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxnQkFBZ0I7SUFDaEIsZUFBZTtJQUNmLGdDQUFnQztBQUNwQzs7QUFFQTtJQUNJLGdCQUFnQjtJQUNoQiw2QkFBNkI7SUFDN0IsY0FBYztJQUNkLGdDQUFnQztJQUNoQyxjQUFjO0FBQ2xCOztBQUVBO0lBQ0ksT0FBTztJQUNQLEtBQUs7QUFDVDs7QUFDSTtRQUNJLFFBQVE7SUFDWjs7QUFDQTtRQUNJLFNBQVM7SUFDYjs7QUFDSTtZQUNJLG1CQUFtQjtZQUNuQixjQUFjO1lBQ2QsV0FBVztRQUNmOztBQUNKO1FBQ0ksZUFBZTtJQUNuQiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvQ3JlYXRlL2NyZWF0ZS1mdWVsLXN1cmNoYXJnZS5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC50YWJzIGxhYmVsIHtcclxuICAgIGJhY2tncm91bmQ6IG5vbmU7XHJcbiAgICBmb250LXNpemU6IDE2cHg7XHJcbiAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2Y3ZjdmNztcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJzIGlucHV0W3R5cGU9XCJyYWRpb1wiXTpjaGVja2VkICsgbGFiZWwge1xyXG4gICAgYmFja2dyb3VuZDogbm9uZTtcclxuICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCBibHVlO1xyXG4gICAgY29sb3I6ICMwYzUyYjE7XHJcbiAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgIzBjNTJiMTtcclxuICAgIGZvbnQtc2l6ZToxNnB4O1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2gge1xyXG4gICAgbGVmdDogMDtcclxuICAgIHRvcDowO1xyXG59XHJcbiAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQge1xyXG4gICAgICAgIGJvcmRlcjowO1xyXG4gICAgfVxyXG4gICAgOjpuZy1kZWVwIC5jdXN0b20tc2VhcmNoIC5jYXJkLWJvZHkge1xyXG4gICAgICAgIHBhZGRpbmc6MDtcclxuICAgIH1cclxuICAgICAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQtYm9keSBsaTpob3ZlciB7XHJcbiAgICAgICAgICAgIGJhY2tncm91bmQ6ICMwMDdiZmY7XHJcbiAgICAgICAgICAgIGN1cnNvcjpwb2ludGVyO1xyXG4gICAgICAgICAgICBjb2xvcjp3aGl0ZTtcclxuICAgICAgICB9XHJcbiAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQgLmNhcmQtaGVhZGVye1xyXG4gICAgICAgIHBhZGRpbmc6IDAgMTVweDtcclxuICAgIH1cclxuXHJcblxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CreateFuelSurchargeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-create-fuel-surcharge',
                templateUrl: './create-fuel-surcharge.component.html',
                styleUrls: ['./create-fuel-surcharge.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"] }, { type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"] }, { type: _angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"] }, { type: _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"] }, { type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"] }]; }, null); })();
const RangeValidator = (fg) => {
    const fst = fg.get('FuelSurchargeTable').value;
    const start = fst.PriceRangeStartValue;
    const end = fst.PriceRangeEndValue;
    const statusId = fg.get('StatusId').value;
    return statusId == 1 || start != null && end != null && +end > +start
        ? null
        : { range: true };
};


/***/ }),

/***/ "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts":
/*!***********************************************************************************************************************!*\
  !*** ./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts ***!
  \***********************************************************************************************************************/
/*! exports provided: ViewFuelSurchargePricingdetailsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewFuelSurchargePricingdetailsComponent", function() { return ViewFuelSurchargePricingdetailsComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");





function ViewFuelSurchargePricingdetailsComponent_tr_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "td", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "span", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "span", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "%");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const pricing_r1 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate2"](" $", pricing_r1.PriceRangeStartValue, " - $", pricing_r1.PriceRangeEndValue, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](pricing_r1.FuelSurchargeStartPercentage);
} }
class ViewFuelSurchargePricingdetailsComponent {
    constructor(fb, fuelsurchargeService) {
        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.FuelSurchargePricingList = [];
    }
    ngOnInit() {
    }
    getFuelSurchargePricingDetails(fuelSurchargeIndexId) {
        this.fuelsurchargeService.getSurchargeTableNew(fuelSurchargeIndexId).subscribe(data => {
            this.FuelSurchargePricingList = data;
        });
    }
}
ViewFuelSurchargePricingdetailsComponent.??fac = function ViewFuelSurchargePricingdetailsComponent_Factory(t) { return new (t || ViewFuelSurchargePricingdetailsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"])); };
ViewFuelSurchargePricingdetailsComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: ViewFuelSurchargePricingdetailsComponent, selectors: [["app-view-fuel-surcharge-pricingdetails"]], decls: 12, vars: 1, consts: [[1, "col-sm-12"], [1, "table"], ["width", "47%", 1, "text-center"], ["width", "48%", 1, "text-center"], ["id", "surchargeTable", 2, "max-height", "70vh", "overflow", "auto"], [1, "table", "table-bordered", "table-hover", "mb0"], [4, "ngFor", "ngForOf"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [1, "input-group"], [1, "p-2", "border", "px-4"], [1, "input-group-append"], [1, "input-group-text"]], template: function ViewFuelSurchargePricingdetailsComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "table", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "th", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Price Between");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "th", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Fuel Surcharge %");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "table", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](11, ViewFuelSurchargePricingdetailsComponent_tr_11_Template, 10, 3, "tr", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.FuelSurchargePricingList);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvVmlldy92aWV3LWZ1ZWwtc3VyY2hhcmdlLXByaWNpbmdkZXRhaWxzL3ZpZXctZnVlbC1zdXJjaGFyZ2UtcHJpY2luZ2RldGFpbHMuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](ViewFuelSurchargePricingdetailsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-view-fuel-surcharge-pricingdetails',
                templateUrl: './view-fuel-surcharge-pricingdetails.component.html',
                styleUrls: ['./view-fuel-surcharge-pricingdetails.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts":
/*!*********************************************************************!*\
  !*** ./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts ***!
  \*********************************************************************/
/*! exports provided: ViewFuelSurchargeComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewFuelSurchargeComponent", function() { return ViewFuelSurchargeComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../models/CreateFuelSurcharge */ "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/my.localstorage */ "./src/app/my.localstorage.ts");
/* harmony import */ var _view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component */ "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./view-historical-price/view-historical-price.component */ "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! angular-confirmation-popover */ "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");

























const _c0 = function (a0) { return { "d-block": a0 }; };
function ViewFuelSurchargeComponent_tr_50_td_18_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](6, _c0, !surcharge_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind3"](2, 2, surcharge_r5.Terminal, 0, 40), "...");
} }
function ViewFuelSurchargeComponent_tr_50_td_18_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, ViewFuelSurchargeComponent_tr_50_td_18_div_3_Template, 3, 8, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "a", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_td_18_Template_a_click_4_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r16); const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; return surcharge_r5.IsShowMore = !surcharge_r5.IsShowMore; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "View More/Less");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](3, _c0, surcharge_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.Terminal);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", surcharge_r5.Terminal.length > 40);
} }
function ViewFuelSurchargeComponent_tr_50_td_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.Terminal);
} }
function ViewFuelSurchargeComponent_tr_50_a_34_Template(rf, ctx) { if (rf & 1) {
    const _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("cancel", function ViewFuelSurchargeComponent_tr_50_a_34_Template_a_cancel_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r20); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r19.cancelClicked = true; })("confirm", function ViewFuelSurchargeComponent_tr_50_a_34_Template_a_confirm_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r20); const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r21.archiveFuelSurchargeTable(surcharge_r5.Id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("popoverTitle", ctx_r9.popoverTitle)("popoverMessage", ctx_r9.popoverMessage);
} }
function ViewFuelSurchargeComponent_tr_50_a_35_Template(rf, ctx) { if (rf & 1) {
    const _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_a_35_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r25); const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r23.viewFuelSurcharge(surcharge_r5.Id, "EDIT"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewFuelSurchargeComponent_tr_50_a_38_Template(rf, ctx) { if (rf & 1) {
    const _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_a_38_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r28); const surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit; const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r26.viewFuelSurcharge(surcharge_r5.Id, "COPY"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewFuelSurchargeComponent_tr_50_Template(rf, ctx) { if (rf & 1) {
    const _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "a", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_4_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const surcharge_r5 = ctx.$implicit; const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r29.getFuelSurchargePricingDetails(surcharge_r5.Id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, ViewFuelSurchargeComponent_tr_50_td_18_Template, 6, 5, "td", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, ViewFuelSurchargeComponent_tr_50_td_19_Template, 2, 1, "td", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](27);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](29);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "td", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "a", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_31_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const surcharge_r5 = ctx.$implicit; const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r31.getHistoricalPriceDetails(surcharge_r5.Id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](32, "i", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, ViewFuelSurchargeComponent_tr_50_a_34_Template, 2, 2, "a", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, ViewFuelSurchargeComponent_tr_50_a_35_Template, 2, 0, "a", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "a", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_36_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30); const surcharge_r5 = ctx.$implicit; const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r32.viewFuelSurcharge(surcharge_r5.Id, "VIEW"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](37, "i", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, ViewFuelSurchargeComponent_tr_50_a_38_Template, 2, 0, "a", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const surcharge_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", surcharge_r5.DateRange, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.TableTypeNew);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.TableName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.StatusName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.Customer);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.Carrier);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.SourceRegion);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", surcharge_r5.Terminal.length > 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", surcharge_r5.Terminal.length <= 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.BulkPlant);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.IndexProduct);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.IndexArea);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.IndexPeriod);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](surcharge_r5.IndexType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !surcharge_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !surcharge_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !surcharge_r5.IsArchived);
} }
function ViewFuelSurchargeComponent_div_69_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewFuelSurchargeComponent_ng_template_70_div_7_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Table Type is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function ViewFuelSurchargeComponent_ng_template_70_div_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 87);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, ViewFuelSurchargeComponent_ng_template_70_div_7_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r33.viewFuelSurchargeForm.get("TableTypes").errors.required);
} }
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
function ViewFuelSurchargeComponent_ng_template_70_Template(rf, ctx) { if (rf & 1) {
    const _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "label", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "Table Type");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "ng-multiselect-dropdown", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r35.onTableTypeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](7, ViewFuelSurchargeComponent_ng_template_70_div_7_Template, 2, 1, "div", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "label", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Customer(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "ng-multiselect-dropdown", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r37.onCustomersSelect($event); })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r38.onCustomersDeSelect($event); })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r39.onCustomersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "label", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Carrier(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "ng-multiselect-dropdown", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r40.onCarriersSelect($event); })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r41.onCarriersDeSelect($event); })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r42.onCarriersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "label", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Source Region(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "ng-multiselect-dropdown", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r43.onSourceRegionsSelect($event); })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r44.onSourceRegionsDeSelect($event); })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r45.onSourceRegionsDeSelectAll($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "label", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Terminal(s)/BulkPlant(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](29, "angular2-multiselect", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](33, "From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "input", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function ViewFuelSurchargeComponent_ng_template_70_Template_input_onDateChange_34_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r46.setfromDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "To");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "input", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function ViewFuelSurchargeComponent_ng_template_70_Template_input_onDateChange_39_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r47.settoDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 81);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](43, "input", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "label", 83);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](45, " Show Archived ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div", 84);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "button", 85);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_ng_template_70_Template_button_click_47_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r48.clearFilter(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](48, "Clear Filter");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "button", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_ng_template_70_Template_button_click_49_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r36); const ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4); ctx_r49.filterGrid(); return _r0.close(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](50, "Apply");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Type (Required)")("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.TableTypeList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.viewFuelSurchargeForm.get("TableTypes").invalid && ctx_r4.viewFuelSurchargeForm.get("TableTypes").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](22, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Customers")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CustomerList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](24, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Carriers")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CarrierList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Source Regions")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.SourceRegionList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r4.TerminalsAndBulkPlantList)("settings", ctx_r4.MultiSelectSettingsByGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("minDate", ctx_r4.minDate)("maxDate", ctx_r4.maxDate)("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](26, _c1, ctx_r4.IsLoading));
} }
class ViewFuelSurchargeComponent {
    constructor(fb, regionService, fuelsurchargeService, router, cdr) {
        this.fb = fb;
        this.regionService = regionService;
        this.fuelsurchargeService = fuelsurchargeService;
        this.router = router;
        this.cdr = cdr;
        this.IsLoading = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subject"]();
        this.SinlgeselectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.TerminalsAndBulkPlantList = [];
        this.SelectedTerminalsAndBulkPlants = [];
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.FuelSurchargeList = [];
        this.minDate = new Date();
        this.maxDate = new Date();
        this.popoverTitle = 'Archive Confirmation';
        this.popoverMessage = 'Do you want to archive?';
        this.cancelClicked = false;
        this.confirmClicked = false;
    }
    ngOnInit() {
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
        this.minDate.setFullYear(this.minDate.getFullYear() - 20);
        this.CounrtyId = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_5__["MyLocalStorage"].getData("countryIdForDashboard");
        this.SinlgeselectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.MultiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.MultiSelectSettingsByGroup = {
            singleSelection: false,
            text: "Select",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };
        this.getTableTypes();
        this.viewFuelSurchargeForm = this.createForm();
        this.initializeFuelSurchargeDatatableGrid();
    }
    ngOnDestroy() {
        this.rerender_destroy();
    }
    ngAfterViewInit() {
        this.getFuelSurchargeGridDetails();
    }
    createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new _models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_3__["FuelSurchargeIndexViewModel"]();
        }
        return this.fb.group({
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.Fsmodel.TableTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.Fsmodel.Customers),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.Fsmodel.Carriers),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.Fsmodel.SourceRegions),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.SelectedTerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
        });
    }
    getFuelSurchargePricingDetails(fuelSurchargeIndexId) {
        this.fuelSurchageComponent.getFuelSurchargePricingDetails(fuelSurchargeIndexId);
    }
    getHistoricalPriceDetails(fuelSurchargeIndexId) {
        this.historicalPriceComponent.getHistoricalPriceDetails(fuelSurchargeIndexId);
    }
    archiveFuelSurchargeTable(fuelSurchargeIndexId) {
        this.IsLoading = true;
        this.fuelsurchargeService.archiveFuelSurchargeTable(fuelSurchargeIndexId)
            .subscribe((response) => {
            this.IsLoading = false;
            //this.serviceResponse = response;
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__["Declarations"].msgsuccess('Selected fuel surcharge table archived successfully', undefined, undefined);
                this.filterGrid();
            }
        });
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewFuelSurchargeForm.get('Carriers').patchValue([]);
        this.viewFuelSurchargeForm.get('Customers').patchValue([]);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                break;
            case 2: // customer
                this.IsCustomerSelected = true;
                this.getSupplierCustomers();
                this.getCarriers();
                break;
            case 3: //carrier
                this.IsCarrierSelected = true;
                this.getCarriers();
                this.getSupplierCustomers();
                break;
        }
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    onCarriersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCarriersDeSelect(item) {
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCustomersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCustomersDeSelect(item) {
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCarriersOrCustomersChange() {
        var selectedTableType = this.viewFuelSurchargeForm.get('TableTypes').value;
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    getTableTypes() {
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TableTypeList = yield data;
        }));
    }
    getCarriers() {
        this.IsLoading = true;
        this.regionService.getCarriers().subscribe((carriers) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CarrierList = yield carriers;
            this.SourceRegionList = null;
            this.IsLoading = false;
        }));
    }
    getSupplierCustomers() {
        this.IsLoading = true;
        this.fuelsurchargeService.getSupplierCustomers().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CustomerList = yield data;
            this.IsLoading = false;
        }));
    }
    getSourceRegions(tableType) {
        ;
        let customerIds = [];
        let carrierIds = [];
        let selectedCustomers = this.viewFuelSurchargeForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }
        let selectedCarriers = this.viewFuelSurchargeForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.IsLoading = true;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(data => {
            this.SourceRegionList = data;
            this.IsLoading = false;
        });
    }
    getTerminalsBulkPlant() {
        this.IsLoading = true;
        var selectedSourceRegions = this.viewFuelSurchargeForm.get('SourceRegions').value;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.IsLoading = false;
            }));
        }
    }
    onSourceRegionsDeSelect(item) {
        this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
    }
    onSourceRegionsDeSelectAll(item) {
        this.IsSourceRegionSelected = false;
    }
    onSourceRegionsSelect(item) {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
    }
    filterGrid() {
        $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
        this.getFuelSurchargeGridDetails();
    }
    clearFilter() {
        this.clearForm();
        this.getFuelSurchargeGridDetails();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }
    clearForm() {
        this.viewFuelSurchargeForm.reset();
        $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
    }
    getFuelSurchargeGridDetails() {
        ;
        var input = new _models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_3__["FuelSurchargeInputModel"]();
        var selectedTableTypes = this.viewFuelSurchargeForm.get('TableTypes').value;
        var selectedCustomers = this.viewFuelSurchargeForm.get('Customers').value;
        var selectedCarriers = this.viewFuelSurchargeForm.get('Carriers').value;
        var selectedSourceRegions = this.viewFuelSurchargeForm.get('SourceRegions').value;
        var selectedTerminalAndBulkPlants = this.viewFuelSurchargeForm.get('TerminalsAndBulkPlants').value;
        input.StartDate = this.viewFuelSurchargeForm.controls.fromDate.value;
        input.EndDate = this.viewFuelSurchargeForm.controls.toDate.value;
        input.IsArchived = this.viewFuelSurchargeForm.controls.isArchived.value;
        if (selectedTableTypes != null && selectedTableTypes != undefined && selectedTableTypes.length > 0) {
            var tableTypeIds = selectedTableTypes.map(s => s.Id);
            input.TableTypeIds = tableTypeIds.join(',');
        }
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            var customerIds = selectedCustomers.map(s => s.Id);
            input.CustomerIds = customerIds.join(',');
        }
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            var carrierIds = selectedCarriers.map(s => s.Id);
            input.CarrierIds = carrierIds.join(',');
        }
        if (selectedSourceRegions != null && selectedSourceRegions != undefined && selectedSourceRegions.length > 0) {
            var sourceRegionIds = selectedSourceRegions.map(s => s.Id);
            input.SourceRegionIds = sourceRegionIds.join(',');
        }
        if (selectedTerminalAndBulkPlants != null && selectedTerminalAndBulkPlants != undefined && selectedTerminalAndBulkPlants.length > 0) {
            var selectedTerminalIds = selectedTerminalAndBulkPlants.filter(c => c.Code == "Terminals");
            var terminalIds = selectedTerminalIds.map(s => s.Id);
            input.TerminalIds = terminalIds.join(',');
            var selectedBulkPlants = selectedTerminalAndBulkPlants.filter(c => c.Code == "BulkPlants");
            var bulkPlantIds = selectedBulkPlants.map(s => s.Id);
            input.BulkPlantIds = bulkPlantIds.join(',');
        }
        this.IsLoading = true;
        this.cdr.detectChanges();
        this.fuelsurchargeService.getFuelSurchargeGridDetails(input).subscribe(data => {
            this.IsLoading = false;
            if (data && data.length > 0) {
                this.FuelSurchargeList = data;
            }
            this.dtTrigger.next();
        });
    }
    rerender_destroy() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                dtInstance.destroy();
            });
        }
    }
    rerender_trigger() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                this.dtTrigger.next();
            });
        }
    }
    setfromDate($event) {
        this.viewFuelSurchargeForm.controls.fromDate.setValue($event);
    }
    settoDate($event) {
        this.viewFuelSurchargeForm.controls.toDate.setValue($event);
    }
    viewFuelSurcharge(fsurcharId, mode) {
        let operation = { FsurcharId: fsurcharId, Mode: mode };
        this.fuelsurchargeService.onSelectedFuelSurchargeId.next(JSON.stringify(operation));
        this.fuelsurchargeService.onSelectedTabChanged.next(1);
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew/' + fsurcharId]);
    }
    initializeFuelSurchargeDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Fuel Surcharge', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Fuel Surcharge', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
}
ViewFuelSurchargeComponent.??fac = function ViewFuelSurchargeComponent_Factory(t) { return new (t || ViewFuelSurchargeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_13__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"])); };
ViewFuelSurchargeComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: ViewFuelSurchargeComponent, selectors: [["app-view-fuel-surcharge"]], viewQuery: function ViewFuelSurchargeComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.fuelSurchageComponent = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.historicalPriceComponent = _t.first);
    } }, decls: 72, vars: 7, consts: [[3, "formGroup"], [1, "row"], [1, "col-sm-12", "text-left"], ["placement", "bottom", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs16", "mr10", "filter-link", "pa", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-fuel-surcharge-grid", 1, "table-responsive"], ["id", "fuel-surcharge-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Id", 1, "hide-element"], ["data-key", "DateRange"], ["data-key", "TableTypeNew"], ["data-key", "TableName"], ["data-key", "StatusName"], ["data-key", "Customer"], ["data-key", "Carrier"], ["data-key", "SourceRegion"], ["data-key", "Terminal"], ["data-key", "BulkPlant"], ["data-key", "IndexProduct"], ["data-key", "IndexArea"], ["data-key", "IndexPeriod"], ["data-key", "IndexUpdate"], ["data-key", "HistoricalPrice"], [4, "ngFor", "ngForOf"], ["id", "historical-price-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], ["id", "pricing-panel", 1, "side-panel", "pl5", "pr5"], ["class", "loader", 4, "ngIf"], ["popContent", ""], [1, "hide-element"], [1, "text-center"], ["onclick", "slidePanel('#pricing-panel','30%')", 1, "btn", "btn-link", 3, "click"], [4, "ngIf"], ["onclick", "slidePanel('#historical-price-panel','30%')", "placement", "bottom", "ngbTooltip", "Historical Price", 1, "btn", "btn-link", "fs16", 3, "click"], [1, "fas", "fa-history"], [1, "text-center", "text-nowrap"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-street-view"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Copy", 3, "click", 4, "ngIf"], [1, "d-none", 3, "ngClass"], ["class", "d-none", 3, "ngClass", 4, "ngIf"], [3, "click"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-trash-alt", "color-maroon"], ["placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], ["placement", "bottom", "ngbTooltip", "Copy", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-copy"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "popover-details"], [1, "col-sm-6"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 3, "placeholder", "settings", "data", "onSelect"], ["class", "color-maroon", 4, "ngIf"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], [1, "col-sm-12"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "placeholder", "Select Date", "formControlName", "fromDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Select Date", "formControlName", "toDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-6", "form-group"], [1, "form-check"], ["formControlName", "isArchived", "type", "checkbox", "value", "", "id", "ckb-isArchived", 1, "form-check-input"], ["for", "ckb-isArchived", 1, "form-check-label"], [1, "col-sm-6", "text-right", "form-buttons", "mt20"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], ["id", "filter-fuel-surcharge-grid", "type", "button", 1, "btn", "btn-lg", "btn-primary", "mt3", "valid", 3, "ngClass", "click"], [1, "color-maroon"]], template: function ViewFuelSurchargeComponent_Template(rf, ctx) { if (rf & 1) {
        const _r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "a", 3, 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewFuelSurchargeComponent_Template_a_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r50); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4); return _r0.open(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Filters");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "table", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Id");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Date Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Table Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, "Table Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "th", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "Terminal(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "Bulk Plant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Index Product");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "th", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "Index Area");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](42, "Index Period");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Index Update");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "th", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](46, "Historical Price");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](48, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](50, ViewFuelSurchargeComponent_tr_50_Template, 39, 18, "tr", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "div", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "a", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](55, "i", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "h3", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](57, "Historical Price Details");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](59, "app-view-historical-price");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "div", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "a", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](64, "i", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "h3", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](66, "Fuel Surcharge Table");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](68, "app-view-fuel-surcharge-pricingdetails");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](69, ViewFuelSurchargeComponent_div_69_Template, 5, 0, "div", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](70, ViewFuelSurchargeComponent_ng_template_70_Template, 51, 28, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](71);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.viewFuelSurchargeForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngbPopover", _r3)("autoClose", "outside");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.FuelSurchargeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbPopover"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgForOf"], _view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"], _view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_16__["??c"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["CheckboxControlValueAccessor"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_15__["SlicePipe"]], styles: [".master-filter.popover {\r\n    min-width: 425px;\r\n    max-width: 450px;\r\n    background: #F9F9F9;\r\n    border: 1px solid #E9E7E7;\r\n    box-sizing: border-box;\r\n    box-shadow: 10px 10px 8px -2px rgb(0, 0, 0, 0.13);\r\n    border-radius: 10px;\r\n}\r\n      .master-filter.popover .popover-body {\r\n        padding: 0;\r\n        border-radius: 5px;\r\n        background: #ffffff;\r\n    }\r\n      .master-filter.popover .popover-details {\r\n        padding: 15px;\r\n    }\r\n      .master-filter.popover .border-bottom-2 {\r\n        border-bottom: 2px solid #e7eaec !important;\r\n    }\r\n    .filter-link[_ngcontent-%COMP%] {\r\n    top: -45px;\r\n    left: 380px\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZnVlbHN1cmNoYXJnZS9WaWV3L3ZpZXctZnVlbC1zdXJjaGFyZ2UuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGdCQUFnQjtJQUNoQixnQkFBZ0I7SUFDaEIsbUJBQW1CO0lBQ25CLHlCQUF5QjtJQUN6QixzQkFBc0I7SUFDdEIsaURBQWlEO0lBQ2pELG1CQUFtQjtBQUN2QjtJQUNJO1FBQ0ksVUFBVTtRQUNWLGtCQUFrQjtRQUNsQixtQkFBbUI7SUFDdkI7SUFFQTtRQUNJLGFBQWE7SUFDakI7SUFFQTtRQUNJLDJDQUEyQztJQUMvQztJQUNKO0lBQ0ksVUFBVTtJQUNWO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9mdWVsc3VyY2hhcmdlL1ZpZXcvdmlldy1mdWVsLXN1cmNoYXJnZS5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIge1xyXG4gICAgbWluLXdpZHRoOiA0MjVweDtcclxuICAgIG1heC13aWR0aDogNDUwcHg7XHJcbiAgICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiKDAsIDAsIDAsIDAuMTMpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxufVxyXG4gICAgOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIH1cclxuXHJcbiAgICA6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgfVxyXG5cclxuICAgIDo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICB9XHJcbi5maWx0ZXItbGluayB7XHJcbiAgICB0b3A6IC00NXB4O1xyXG4gICAgbGVmdDogMzgwcHhcclxufVxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](ViewFuelSurchargeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-view-fuel-surcharge',
                templateUrl: './view-fuel-surcharge.component.html',
                styleUrls: ['./view-fuel-surcharge.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__["RegionService"] }, { type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__["FuelSurchargeService"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_13__["Router"] }, { type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"] }]; }, { datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"]]
        }], fuelSurchageComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"]]
        }], historicalPriceComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"]]
        }] }); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts":
/*!*********************************************************************************************!*\
  !*** ./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts ***!
  \*********************************************************************************************/
/*! exports provided: ViewHistoricalPriceComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewHistoricalPriceComponent", function() { return ViewHistoricalPriceComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");






function ViewHistoricalPriceComponent_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "label", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "ng-multiselect-dropdown", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function ViewHistoricalPriceComponent_div_2_Template_ng_multiselect_dropdown_ngModelChange_3_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r5); const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r4.SelectedPeriod = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("Select ", ctx_r0.HistoricalPrice == null ? null : ctx_r0.HistoricalPrice.PeriodName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx_r0.SelectedPeriod)("settings", ctx_r0.SinlgeselectSettingsById)("data", ctx_r0.PeriodList);
} }
function ViewHistoricalPriceComponent_div_3_Template(rf, ctx) { if (rf & 1) {
    const _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "button", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function ViewHistoricalPriceComponent_div_3_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r7); const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r6.fetchHistoricalPrice(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, "Fetch Price");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function ViewHistoricalPriceComponent_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("$", ctx_r2.HistoricalPrice == null ? null : ctx_r2.HistoricalPrice.ManualIndexPrice, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" per Gallon (Including Taxes) on ", ctx_r2.HistoricalPrice == null ? null : ctx_r2.HistoricalPrice.ManualIndexPriceDate, " ");
} }
function ViewHistoricalPriceComponent_div_5_ng_container_28_tr_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "td", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "span", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "$");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "span", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const history_r10 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", history_r10.PublishDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](history_r10.Price);
} }
function ViewHistoricalPriceComponent_div_5_ng_container_28_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, ViewHistoricalPriceComponent_div_5_ng_container_28_tr_1_Template, 10, 2, "tr", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r8.HistoricalPrice.HistoricalPriceDetails);
} }
function ViewHistoricalPriceComponent_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "table", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Index Product");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Index Area");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "Index Period");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "table", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Published Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "Price per Gallon");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "div", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "table", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](28, ViewHistoricalPriceComponent_div_5_ng_container_28_Template, 2, 1, "ng-container", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexProduct);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexArea);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexPeriod);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r3.HistoricalPrice && ctx_r3.HistoricalPrice.HistoricalPriceDetails && ctx_r3.HistoricalPrice.HistoricalPriceDetails.length > 0);
} }
class ViewHistoricalPriceComponent {
    constructor(fb, fuelsurchargeService) {
        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.SinlgeselectSettingsById = {};
        this.PeriodList = [];
        this.SelectedPeriod = [];
    }
    ngOnInit() {
        this.SinlgeselectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.getPeriod();
    }
    getPeriod() {
        for (let i = 1; i <= 12; i++) {
            this.PeriodList.push({ Id: i, Name: i });
        }
    }
    getHistoricalPriceDetails(fuelSurchargeIndexId) {
        this.SelectedPeriod = [];
        this.SelectedPeriod.push({ Id: 6, Name: 6 });
        this.fuelsurchargeService.getHistoricalPrice(fuelSurchargeIndexId, 6).subscribe(data => {
            this.HistoricalPrice = data;
            this.HistoricalPriceDetailList = this.HistoricalPrice.HistoricalPriceDetails;
            this.SelectedFuelSurchargeIndexId = fuelSurchargeIndexId;
        });
    }
    fetchHistoricalPrice() {
        this.fuelsurchargeService.getHistoricalPrice(this.SelectedFuelSurchargeIndexId, this.SelectedPeriod[0].Id).subscribe(data => {
            this.HistoricalPrice = data;
            this.HistoricalPriceDetailList = this.HistoricalPrice.HistoricalPriceDetails;
        });
    }
}
ViewHistoricalPriceComponent.??fac = function ViewHistoricalPriceComponent_Factory(t) { return new (t || ViewHistoricalPriceComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"])); };
ViewHistoricalPriceComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: ViewHistoricalPriceComponent, selectors: [["app-view-historical-price"]], decls: 6, vars: 4, consts: [[1, "mx-3"], [1, "row"], ["class", "col-sm-6", 4, "ngIf"], ["class", "col-sm-6 pt-1", 4, "ngIf"], ["class", "col-sm-12 form-group", 4, "ngIf"], ["class", "col-sm-12", 4, "ngIf"], [1, "col-sm-6"], ["for", "Periods"], ["id", "Periods", 3, "ngModel", "settings", "data", "ngModelChange"], [1, "col-sm-6", "pt-1"], ["id", "fetch-historical-price", "type", "button", 1, "btn", "btn-lg", "mt-4", "btn-primary", "mb-2", "valid", 3, "click"], [1, "col-sm-12", "form-group"], [1, "alert", "alert-info"], [1, "col-sm-12"], [1, "table", "table-bordered"], [1, "table"], ["width", "47%", 1, "text-center"], ["width", "48%", 1, "text-center"], ["id", "historyTable", 2, "max-height", "300px", "overflow", "auto"], [1, "table", "table-bordered", "table-hover", "mb0"], [4, "ngIf"], [4, "ngFor", "ngForOf"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [1, "input-group"], [1, "input-group-append"], [1, "input-group-text"], [1, "p-2", "border", "px-4"]], template: function ViewHistoricalPriceComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](2, ViewHistoricalPriceComponent_div_2_Template, 4, 4, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, ViewHistoricalPriceComponent_div_3_Template, 3, 0, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, ViewHistoricalPriceComponent_div_4_Template, 5, 2, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, ViewHistoricalPriceComponent_div_5_Template, 29, 4, "div", 5);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_4__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgModel"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvVmlldy92aWV3LWhpc3RvcmljYWwtcHJpY2Uvdmlldy1oaXN0b3JpY2FsLXByaWNlLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](ViewHistoricalPriceComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-view-historical-price',
                templateUrl: './view-historical-price.component.html',
                styleUrls: ['./view-historical-price.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/fuelsurcharge.module.ts":
/*!*******************************************************!*\
  !*** ./src/app/fuelsurcharge/fuelsurcharge.module.ts ***!
  \*******************************************************/
/*! exports provided: FuelsurchargeModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelsurchargeModule", function() { return FuelsurchargeModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _master_master_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./master/master.component */ "./src/app/fuelsurcharge/master/master.component.ts");
/* harmony import */ var _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./Create/create-fuel-surcharge.component */ "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts");
/* harmony import */ var _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./View/view-fuel-surcharge.component */ "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component */ "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts");
/* harmony import */ var _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./View/view-historical-price/view-historical-price.component */ "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");















const route = [
    { path: '', component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] },
    { path: 'CreateNew', component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] },
    { path: 'CreateNew/:fuelsurchargeId', component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] }
];
class FuelsurchargeModule {
}
FuelsurchargeModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({ type: FuelsurchargeModule });
FuelsurchargeModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({ factory: function FuelsurchargeModule_Factory(t) { return new (t || FuelsurchargeModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
            angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](FuelsurchargeModule, { declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"],
        _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__["CreateFuelSurchargeComponent"],
        _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__["ViewFuelSurchargeComponent"],
        _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__["ViewFuelSurchargePricingdetailsComponent"],
        _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__["ViewHistoricalPriceComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
        _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](FuelsurchargeModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [
                    _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"],
                    _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__["CreateFuelSurchargeComponent"],
                    _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__["ViewFuelSurchargeComponent"],
                    _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__["ViewFuelSurchargePricingdetailsComponent"],
                    _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__["ViewHistoricalPriceComponent"],
                ],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
                    _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
                    angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/master/master.component.ts":
/*!**********************************************************!*\
  !*** ./src/app/fuelsurcharge/master/master.component.ts ***!
  \**********************************************************/
/*! exports provided: MasterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterComponent", function() { return MasterComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../Create/create-fuel-surcharge.component */ "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts");
/* harmony import */ var _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../View/view-fuel-surcharge.component */ "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts");







function MasterComponent_app_create_fuel_surcharge_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-create-fuel-surcharge");
} }
function MasterComponent_app_view_fuel_surcharge_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-view-fuel-surcharge");
} }
class MasterComponent {
    constructor(router, fuelsurchargeService) {
        this.router = router;
        this.fuelsurchargeService = fuelsurchargeService;
        this.viewType = 0;
    }
    ngOnInit() {
        let _viewType = localStorage.getItem("fuelSurchargeTabId");
        if (_viewType && +_viewType > 0) {
            this.viewType = +_viewType;
        }
        this.fuelsurchargeService.onSelectedTabChanged.subscribe(s => {
            if (s == 2) {
                this.viewType = 2;
            }
            else {
                this.viewType = 1;
            }
        });
        this.viewType = +_viewType;
    }
    ngAfterViewInit() {
        this.changeViewType(this.viewType);
    }
    changeViewType(value) {
        localStorage.setItem("fuelSurchargeTabId", value.toString());
        this.viewType = value;
        this.fuelsurchargeService.onSelectedFuelSurchargeId.next(null);
        this.fuelsurchargeService.onSelectedTabChanged.next(value);
        //if(this.viewType==1)
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew']);
    }
}
MasterComponent.??fac = function MasterComponent_Factory(t) { return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"])); };
MasterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: MasterComponent, selectors: [["app-master"]], decls: 16, vars: 8, consts: [[1, "row"], [1, "col-sm-12"], [1, "col-sm-4"], [1, "d-inline-block", "border", "bg-white", "p-1", "radius-capsule", "shadow-b", "mb-2"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [4, "ngIf"]], template: function MasterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_7_listener() { return ctx.changeViewType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8, "Create Fuel Surcharge");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](9, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_10_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, "View Fuel Surcharge");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](14, MasterComponent_app_create_fuel_surcharge_14_Template, 1, 0, "app-create-fuel-surcharge", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, MasterComponent_app_view_fuel_surcharge_15_Template, 1, 0, "app-view-fuel-surcharge", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 1)("checked", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 2);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_4__["CreateFuelSurchargeComponent"], _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_5__["ViewFuelSurchargeComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvbWFzdGVyL21hc3Rlci5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-master',
                templateUrl: './master.component.html',
                styleUrls: ['./master.component.css']
            }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts":
/*!*************************************************************!*\
  !*** ./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts ***!
  \*************************************************************/
/*! exports provided: FuelSurchargeTableModel, FuelSurchargeIndexViewModel, FuelSurchargeInputModel, FuelSurchargeGridModel, FuelSurchargePricingModel, HistoricalPriceModel, HistoricalPriceDetailsModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargeTableModel", function() { return FuelSurchargeTableModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargeIndexViewModel", function() { return FuelSurchargeIndexViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargeInputModel", function() { return FuelSurchargeInputModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargeGridModel", function() { return FuelSurchargeGridModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargePricingModel", function() { return FuelSurchargePricingModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HistoricalPriceModel", function() { return HistoricalPriceModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HistoricalPriceDetailsModel", function() { return HistoricalPriceDetailsModel; });
class FuelSurchargeTableModel {
    constructor(values = {}) {
        Object.assign(this, values);
    }
}
class FuelSurchargeIndexViewModel {
    constructor() {
        this.TableTypes = [];
        this.Customers = [];
        this.Carriers = [];
        this.SourceRegions = [];
        this.TerminalsAndBulkPlants = [];
        this.FuelSurchargeProducts = [];
        this.FuelSurchargePeriods = [];
        this.FuelSurchargeAreas = [];
        this.WeekDay = "Mon";
        this.Weeks = [];
        this.Months = [];
        this.SourceMonths = [];
        this.Annualy = [];
        this.SourceAnnualy = [];
        this.IsManualUpdate = false;
        this.FuelSurchargeTable = new FuelSurchargeTableModel(); //input model
        this.StatusId = 2;
    }
}
class FuelSurchargeInputModel {
}
class FuelSurchargeGridModel {
}
class FuelSurchargePricingModel {
}
class HistoricalPriceModel {
}
class HistoricalPriceDetailsModel {
}


/***/ })

}]);
//# sourceMappingURL=fuelsurcharge-fuelsurcharge-module-es2015.js.map