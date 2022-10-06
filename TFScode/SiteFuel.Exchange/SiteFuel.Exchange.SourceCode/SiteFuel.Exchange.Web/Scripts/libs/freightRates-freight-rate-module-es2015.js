(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["freightRates-freight-rate-module"],{

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
RegionService.ɵfac = function RegionService_Factory(t) { return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"])); };
RegionService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: RegionService, factory: RegionService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/freightRates/Create/create-freight-rate-rules-component.ts":
/*!****************************************************************************!*\
  !*** ./src/app/freightRates/Create/create-freight-rate-rules-component.ts ***!
  \****************************************************************************/
/*! exports provided: CreateFreightRateRules */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateFreightRateRules", function() { return CreateFreightRateRules; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../Models/createFreightRateRules */ "./src/app/freightRates/Models/createFreightRateRules.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../shared-components/Freight/freight.component */ "./src/app/shared-components/Freight/freight.component.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../Services/freight-rate-rules.service */ "./src/app/freightRates/Services/freight-rate-rules.service.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");


















function CreateFreightRateRules_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_div_2_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r3.clearForm(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Create New");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_1_th_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r12 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r12.Name, " ");
} }
function CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r16.get("MinQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r16.get("MinQuantity").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r16 = ctx.$implicit;
    const p_r17 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r16.get("MinQuantity").invalid && groupDetail_r16.get("MinQuantity").touched);
} }
function CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](3, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_ng_container_4_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r13 = ctx.$implicit;
    const o_r14 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r13.get("group")["controls"]);
} }
function CreateFreightRateRules_div_20_div_1_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Min Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "table", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, CreateFreightRateRules_div_20_div_1_div_1_th_7_Template, 2, 1, "th", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](9, 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, CreateFreightRateRules_div_20_div_1_div_1_ng_container_10_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r8.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r8.rForm.get("FuelGroupTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_1_div_2_th_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r24 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r24.Name, " ");
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid range. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_div_3_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("StartQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("StartQuantity").errors.pattern);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("StartQuantity").errors.InvalidRange);
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid range. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_div_3_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("EndQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("EndQuantity").errors.pattern);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("EndQuantity").errors.InvalidRange);
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_Template(rf, ctx) { if (rf & 1) {
    const _r43 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "input", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_Template_input_input_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r43); const o_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index; const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r41.onEndQuantityKeyPress(o_r26); })("change", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_Template_input_change_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r43); const o_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index; const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r44.onEndQuantityLostFocus(o_r26); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_div_2_Template, 4, 3, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("EndQuantity").invalid && item_r25.get("EndQuantity").touched);
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Above ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r47.get("RateValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r47.get("RateValue").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r47 = ctx.$implicit;
    const p_r48 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r47.get("RateValue").invalid && groupDetail_r47.get("RateValue").touched);
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_13_Template(rf, ctx) { if (rf & 1) {
    const _r55 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_13_Template_a_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r55); const o_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index; const ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r53.AddRanges(o_r26); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "a", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_13_Template_a_click_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r55); const o_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index; const ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r56.RemoveRanges(o_r26); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} }
function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_Template(rf, ctx) { if (rf & 1) {
    const _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "input", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_Template_input_input_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r59); const o_r26 = ctx.index; const ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r58.onStartQuantityKeyPress($event, o_r26); })("change", function CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_Template_input_change_4_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r59); const o_r26 = ctx.index; const ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r60.onStartQuantityLostFocus(o_r26); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_div_5_Template, 4, 3, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "To");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_8_Template, 3, 1, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_td_9_Template, 2, 0, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](10, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_11_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_ng_container_13_Template, 3, 0, "ng-container", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r25 = ctx.$implicit;
    const o_r26 = ctx.index;
    const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("StartQuantity").invalid && item_r25.get("StartQuantity").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !item_r25.get("IsLast").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r25.get("IsLast").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r25.get("group")["controls"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", o_r26 == ctx_r23.rForm.get("RangeTable")["controls"].length - 1);
} }
function CreateFreightRateRules_div_20_div_1_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "table", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "th", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Quantity (Gallons)");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateFreightRateRules_div_20_div_1_div_2_th_6_Template, 2, 1, "th", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](8, 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateFreightRateRules_div_20_div_1_div_2_ng_container_9_Template, 14, 6, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r9.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r9.rForm.get("RangeTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_div_1_Template, 11, 2, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_1_div_2_Template, 10, 2, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r5.freightComponent.ShowMessage);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r5.freightComponent.ShowMessage);
} }
function CreateFreightRateRules_div_20_div_2_div_1_th_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r68 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r68.Name, " ");
} }
function CreateFreightRateRules_div_20_div_2_div_1_th_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Mixed Load ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_th_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Calculation of Mixed fuel group load based on: ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r74.get("MinQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r74.get("MinQuantity").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r74 = ctx.$implicit;
    const p_r75 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r74.get("MinQuantity").invalid && groupDetail_r74.get("MinQuantity").touched);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("MixLoadMinValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("MixLoadMinValue").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "input", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_div_2_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("MixLoadMinValue").invalid && item_r69.get("MixLoadMinValue").touched);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_div_3_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_div_3_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("FuelGroups").errors.required);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "ng-multiselect-dropdown", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_div_3_Template, 2, 1, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    const ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select fuel group (Required)")("settings", ctx_r85.SingleSelectSettingsById)("data", ctx_r85.SelectedFuelGroups);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("FuelGroups").invalid && item_r69.get("FuelGroups").touched);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Round Up");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Proportion");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_div_12_Template, 4, 4, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FreightRateCalcPreferenceType")("value", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("disabled", item_r69.get("MixLoadMinValue").value == 0 ? true : null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FreightRateCalcPreferenceType")("value", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("disabled", item_r69.get("MixLoadMinValue").value == 0 ? true : null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r69.get("FreightRateCalcPreferenceType").value == 1 && item_r69.get("MixLoadMinValue").value > 0);
} }
function CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](3, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_ng_container_4_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_5_Template, 3, 1, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_td_6_Template, 13, 7, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r69 = ctx.$implicit;
    const o_r70 = ctx.index;
    const ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r70);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r69.get("group")["controls"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r67.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r67.freightComponent.rcForm.get("FuelGroups").value.length > 1);
} }
function CreateFreightRateRules_div_20_div_2_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Min Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "table", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, CreateFreightRateRules_div_20_div_2_div_1_th_7_Template, 2, 1, "th", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, CreateFreightRateRules_div_20_div_2_div_1_th_8_Template, 2, 0, "th", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateFreightRateRules_div_20_div_2_div_1_th_9_Template, 2, 0, "th", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](11, 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateFreightRateRules_div_20_div_2_div_1_ng_container_12_Template, 7, 4, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r61.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r61.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r61.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r61.rForm.get("FuelGroupTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_2_ng_container_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "th", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r91 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r91.Name, " ");
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r92.get("TerminalAndBulkPlantName").value, "");
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("LaneID").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("LaneID").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("AssumedMiles").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("AssumedMiles").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r105.get("RateValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r105.get("RateValue").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r105 = ctx.$implicit;
    const p_r106 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r106);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r105.get("RateValue").invalid && groupDetail_r105.get("RateValue").touched);
} }
function CreateFreightRateRules_div_20_div_2_ng_container_26_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_4_Template, 3, 1, "ng-container", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, CreateFreightRateRules_div_20_div_2_ng_container_26_div_17_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](19, "input", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, CreateFreightRateRules_div_20_div_2_ng_container_26_div_20_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](21, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, CreateFreightRateRules_div_20_div_2_ng_container_26_ng_container_22_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "td", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](24, "input", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r92 = ctx.$implicit;
    const o_r93 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r93);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("IsLast").value == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r92.get("LocationName").value, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r92.get("LocationAddress").value, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r92.get("LaneID").value, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("LaneID").invalid && item_r92.get("LaneID").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r92.get("AssumedMiles").invalid && item_r92.get("AssumedMiles").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r92.get("group")["controls"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);
} }
const _c0 = function (a0) { return { "hide-element": a0 }; };
function CreateFreightRateRules_div_20_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_2_div_1_Template, 13, 4, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "table", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "th", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, " Terminals/Bulk Plants ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "th", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, " Location Name ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "th", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "Location Address");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "th", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "LaneId");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "th", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Assumed Miles");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, CreateFreightRateRules_div_20_div_2_ng_container_21_Template, 3, 1, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "th", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "IsLane Required");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](25, 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, CreateFreightRateRules_div_20_div_2_ng_container_26_Template, 25, 9, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r6.freightComponent.ShowMessage);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](6, _c0, ctx_r6.freightComponent.ShowMessage));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx_r6.dtOptions)("dtTrigger", ctx_r6.DtTrigger);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r6.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r6.rForm.get("RangeTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_3_div_1_th_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r117 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r117.Name, " ");
} }
function CreateFreightRateRules_div_20_div_3_div_1_th_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Mixed Load ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_th_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Calculation of Mixed fuel group load based on: ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r123.get("MinQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r123.get("MinQuantity").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r123 = ctx.$implicit;
    const p_r124 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r124);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r123.get("MinQuantity").invalid && groupDetail_r123.get("MinQuantity").touched);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("MixLoadMinValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("MixLoadMinValue").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "input", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_div_2_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("MixLoadMinValue").invalid && item_r118.get("MixLoadMinValue").touched);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_div_3_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_div_3_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("FuelGroups").errors.required);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "ng-multiselect-dropdown", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_div_3_Template, 2, 1, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    const ctx_r134 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select fuel group")("settings", ctx_r134.SingleSelectSettingsById)("data", ctx_r134.SelectedFuelGroups);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("FuelGroups").invalid && item_r118.get("FuelGroups").touched);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Round Up");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Proportion");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_div_12_Template, 4, 4, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FreightRateCalcPreferenceType")("value", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("disabled", item_r118.get("MixLoadMinValue").value == 0 ? true : null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FreightRateCalcPreferenceType")("value", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("disabled", item_r118.get("MixLoadMinValue").value == 0 ? true : null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r118.get("FreightRateCalcPreferenceType").value == 1 && item_r118.get("MixLoadMinValue").value > 0);
} }
function CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](3, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_ng_container_4_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_5_Template, 3, 1, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_td_6_Template, 13, 7, "td", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r118 = ctx.$implicit;
    const o_r119 = ctx.index;
    const ctx_r116 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r119);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r118.get("group")["controls"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r116.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r116.freightComponent.rcForm.get("FuelGroups").value.length > 1);
} }
function CreateFreightRateRules_div_20_div_3_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Min Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "table", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, CreateFreightRateRules_div_20_div_3_div_1_th_7_Template, 2, 1, "th", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, CreateFreightRateRules_div_20_div_3_div_1_th_8_Template, 2, 0, "th", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateFreightRateRules_div_20_div_3_div_1_th_9_Template, 2, 0, "th", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](11, 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateFreightRateRules_div_20_div_3_div_1_ng_container_12_Template, 7, 4, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r111 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r111.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r111.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r111.freightComponent.rcForm.get("FuelGroups").value.length > 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r111.rForm.get("FuelGroupTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_3_div_2_th_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r142 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r142.Name, " ");
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_span_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Duplicate. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_span_3_Template, 2, 0, "span", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r143 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r143.get("UptoQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r143.get("UptoQuantity").errors.pattern);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r143.get("UptoQuantity").errors.DuplicateEntry);
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_div_2_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const groupDetail_r151 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r151.get("RateValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r151.get("RateValue").errors.pattern);
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_div_4_Template, 3, 2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const groupDetail_r151 = ctx.$implicit;
    const p_r152 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", p_r152);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", groupDetail_r151.get("RateValue").invalid && groupDetail_r151.get("RateValue").touched);
} }
function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_Template(rf, ctx) { if (rf & 1) {
    const _r158 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_div_5_Template, 4, 3, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](6, 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_ng_container_7_Template, 5, 2, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "a", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_Template_a_click_10_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r158); const o_r144 = ctx.index; const ctx_r157 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r157.AddRanges(o_r144); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "a", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_Template_a_click_11_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r158); const o_r144 = ctx.index; const ctx_r159 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](4); return ctx_r159.RemoveRanges(o_r144); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const item_r143 = ctx.$implicit;
    const o_r144 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", o_r144);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", item_r143.get("UptoQuantity").invalid && item_r143.get("UptoQuantity").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", item_r143.get("group")["controls"]);
} }
function CreateFreightRateRules_div_20_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Miles Ranges Table");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "table", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, " Upto ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateFreightRateRules_div_20_div_3_div_2_th_9_Template, 2, 1, "th", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, " Action ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](13, 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, CreateFreightRateRules_div_20_div_3_div_2_ng_container_14_Template, 12, 3, "ng-container", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r112 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r112.freightComponent.rcForm.get("FuelGroups").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r112.rForm.get("RangeTable")["controls"]);
} }
function CreateFreightRateRules_div_20_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_3_div_1_Template, 13, 4, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_3_div_2_Template, 15, 2, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r7.freightComponent.ShowMessage);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r7.freightComponent.ShowMessage);
} }
function CreateFreightRateRules_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateFreightRateRules_div_20_div_1_Template, 3, 2, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_20_div_2_Template, 27, 8, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateFreightRateRules_div_20_div_3_Template, 3, 2, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.rForm.get("RuleType").value == 3 && ctx_r1.freightComponent != null && ctx_r1.freightComponent != undefined && ctx_r1.freightComponent.rcForm != undefined && ctx_r1.freightComponent.rcForm.get("FuelGroups").value.length > 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.rForm.get("RuleType").value == 1 && ctx_r1.freightComponent != null && ctx_r1.freightComponent != undefined && ctx_r1.freightComponent.rcForm != undefined && ctx_r1.freightComponent.rcForm.get("FuelGroups").value.length > 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.rForm.get("RuleType").value == 2 && ctx_r1.freightComponent != null && ctx_r1.freightComponent != undefined && ctx_r1.freightComponent.rcForm != undefined && ctx_r1.freightComponent.rcForm.get("FuelGroups").value.length > 0);
} }
function CreateFreightRateRules_div_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
const _c1 = function (a0) { return { "pntr-none": a0 }; };
class CreateFreightRateRules {
    constructor(freightRateRulesService, http, cdr, _fb) {
        this.freightRateRulesService = freightRateRulesService;
        this.http = http;
        this.cdr = cdr;
        this._fb = _fb;
        this.IsEditable = true;
        this.dtOptions = {};
        this.DtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.IsLoading = false;
        this.IsGeneratedSurchargeTable = false;
        this.SingleSelectSettingsById = {};
        this.viewType = 1;
        this.RuleMode = "CREATE";
        this.freightRateRuleType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range;
        this.decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
        this.intGreaterThanZeroRegx = /^[1-9][0-9]*$/;
        this.SelectedFuelGroups = []; // for mixed fuel group.
        this.disableInputControls = false;
    }
    ngOnInit() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Freight Rate P2P', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Freight Rate P2P', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
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
        this.freightRateRulesService.onSelectedFreightRateRuleId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.RuleId = stringify.RuleId;
                this.RuleMode = stringify.Mode;
                this.freightRateRuleType = stringify.FreightRateRuleType;
            }
        });
        // with order page integration
        let id = localStorage.getItem("FreightRateRuleId");
        if (id && +id > 0) {
            this.RuleId = Number(id);
            this.RuleMode = "VIEW";
            localStorage.removeItem("FreightRateRuleId");
        }
        this.createForm(this.freightRateRuleType);
    }
    ngAfterViewInit() {
        if (this.freightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            this.DtTrigger.next();
        }
        if (this.RuleId == null || this.RuleId == undefined)
            return;
        this.freightComponent.isLoadingSubject.next(true);
        this.freightComponent.IsLoaded = false;
        this.IsLoading = true;
        this.cdr.detectChanges();
        this.http.get(this.freightRateRulesService.getFreightRateDetailUrl + this.RuleId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["map"])(fr => {
            const frModel = fr;
            return frModel;
        }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["mergeMap"])(frModel => {
            this.FrModel = frModel;
            let companyIds = [];
            if (this.FrModel.CustomerIds.length > 0) {
                this.FrModel.CustomerIds.forEach(s => companyIds.push(s));
            }
            if (this.FrModel.CarrierIds.length > 0) {
                this.FrModel.CarrierIds.forEach(s => companyIds.push(s));
            }
            var getSupplierCustomersUrl = "/FuelSurcharge/GetSupplierCustomers";
            var getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
            var getCarriersUrl = '/Region/GetCarriers';
            var getFuelGroupsUrl = "/FuelGroup/GetFuelGroups?fuelGroupType=" + this.FrModel.FuelGroupType + "&companyIds=" + companyIds.join(',');
            var getSourceRegionsUrl = "/FuelSurcharge/GetSourceRegionsAsync";
            var getTerminalsAndBulkPlantsUrl = "/FuelSurcharge/GetTerminalsAndBulkPlantsAsync?regionIds=";
            var getCustomerJobsUrl = "/FreightRate/GetCustomerJobs?customerId=" + this.FrModel.CustomerIds.join(',');
            var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
            sourceRegionInput.TableType = this.FrModel.TableType.toString();
            sourceRegionInput.CustomerId = this.FrModel.CustomerIds;
            sourceRegionInput.CarrierId = this.FrModel.CarrierIds;
            const TerminalAndBulkPlans = this.http.get(getTerminalsAndBulkPlantsUrl + frModel.SourceRegionIds.join(','));
            const Customers = this.http.get(getSupplierCustomersUrl);
            const Tabletypes = this.http.get(getTableTypesUrl);
            const Carriers = this.http.get(getCarriersUrl);
            const FuelGroups = this.http.get(getFuelGroupsUrl);
            const SourceRegion = this.http.post(getSourceRegionsUrl, sourceRegionInput);
            const Locations = this.http.get(getCustomerJobsUrl);
            let requiredCalls = [Tabletypes, Customers, Carriers, FuelGroups, SourceRegion, TerminalAndBulkPlans];
            if (this.FrModel.FreightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                requiredCalls = [Tabletypes, Customers, Carriers, FuelGroups, SourceRegion, TerminalAndBulkPlans, Locations];
            }
            return Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["forkJoin"])(requiredCalls);
        })).subscribe(result => {
            this.IsLoading = false;
            this.freightComponent.TableTypeList = result[0];
            if (this.FrModel.CustomerIds != null && this.FrModel.CustomerIds.length > 0)
                this.freightComponent.CustomerList = result[1];
            if (this.FrModel.JobIds != null && this.FrModel.JobIds.length > 0)
                this.freightComponent.LocationList = result[6];
            if (this.FrModel.CarrierIds != null && this.FrModel.CarrierIds.length > 0)
                this.freightComponent.CarrierList = result[2];
            this.freightComponent.FuelGroupsList = result[3];
            this.freightComponent.SourceRegionList = result[4];
            if (this.FrModel.TerminalsAndBulkPlants != null && this.FrModel.TerminalsAndBulkPlants.length > 0)
                this.freightComponent.TerminalsAndBulkPlantList = result[5];
            this.Edit(this.FrModel);
        });
    }
    rerender_destroy() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                dtInstance.destroy();
                //this.DtTrigger.next();
            });
        }
    }
    rerender_trigger() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                //dtInstance.destroy();
                this.DtTrigger.next();
            });
        }
    }
    Edit(_fr) {
        if (this.rForm) {
            if (this.RuleMode != "COPY") {
                this.rForm.get('RuleId').setValue(_fr.Id);
                this.freightComponent.rcForm.get('TableName').setValue(_fr.Name);
                this.IsEditable = false;
            }
            else {
                this.RuleId = null;
            }
            this.rForm.get('RuleType').setValue(_fr.FreightRateRuleType);
            this.freightComponent.rcForm.get('FuelGroupType').setValue(_fr.FuelGroupType);
            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == _fr.TableType));
            if (_fr.TableType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master)
                this.freightComponent.IsMasterSelected = false;
            if (_fr.CustomerIds != null && _fr.CustomerIds != undefined && _fr.CustomerIds.length > 0) {
                this.freightComponent.IsCustomerSelected = true;
                this.freightComponent.IsMasterSelected = false;
                if (this.freightComponent.CustomerList.length > 0 && _fr.CustomerIds.length > 0)
                    this.freightComponent.rcForm.get('Customers').setValue(this.freightComponent.CustomerList.filter(this.IdInComparer(_fr.CustomerIds)));
            }
            if (_fr.JobIds != null && _fr.JobIds != undefined && _fr.JobIds.length > 0) {
                if (this.freightComponent.LocationList.length > 0 && _fr.JobIds.length > 0)
                    this.freightComponent.rcForm.get('Locations').setValue(this.freightComponent.LocationList.filter(this.IdInComparer(_fr.JobIds)));
            }
            if (_fr.CarrierIds != null && _fr.CarrierIds != undefined && _fr.CarrierIds.length > 0) {
                this.freightComponent.IsCarrierSelected = true;
                this.freightComponent.IsMasterSelected = false;
                if (this.freightComponent.CarrierList.length > 0)
                    this.freightComponent.rcForm.get('Carriers').setValue(this.freightComponent.CarrierList.filter(this.IdInComparer(_fr.CarrierIds)));
            }
            if (this.freightComponent.SourceRegionList != null && this.freightComponent.SourceRegionList != undefined && _fr.SourceRegionIds != null && _fr.SourceRegionIds != undefined && _fr.SourceRegionIds.length > 0) {
                if (this.freightComponent.SourceRegionList.length > 0 && _fr.SourceRegionIds.length > 0)
                    this.freightComponent.rcForm.get('SourceRegions').setValue(this.freightComponent.SourceRegionList.filter(this.IdInComparer(_fr.SourceRegionIds)));
            }
            if (this.freightComponent.TerminalsAndBulkPlantList != null && this.freightComponent.TerminalsAndBulkPlantList != undefined && _fr.TerminalsAndBulkPlants != null && _fr.TerminalsAndBulkPlants != undefined && _fr.TerminalsAndBulkPlants.length > 0) {
                if (this.freightComponent.TerminalsAndBulkPlantList.length > 0 && _fr.TerminalsAndBulkPlants.length > 0) {
                    this.freightComponent.rcForm.controls.TerminalsAndBulkPlants.setValue(this.freightComponent.TerminalsAndBulkPlantList.filter(this.IdInComparers(_fr.TerminalsAndBulkPlants)));
                    this.freightComponent.IsSourceRegionSelected = true;
                }
            }
            this.freightComponent.rcForm.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fr.StartDate).format('MM/DD/YYYY'));
            if (_fr.EndDate != null && _fr.EndDate != undefined) {
                this.freightComponent.rcForm.get('EndDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fr.EndDate).format('MM/DD/YYYY'));
            }
            if (_fr.FuelGroupIds != null) {
                if (this.freightComponent.FuelGroupsList.length > 0 && _fr.FuelGroupIds.length > 0) {
                    this.freightComponent.rcForm.get('FuelGroups').setValue(this.freightComponent.FuelGroupsList.filter(this.IdInComparer(_fr.FuelGroupIds)));
                    this.SelectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value;
                }
            }
            this.rerender_destroy();
            this.freightComponent.rcForm.get('StatusId').setValue(_fr.Status);
            if (_fr.FreightRateFuelGroups != null && _fr.FreightRateFuelGroups.length > 0) {
                this.rForm.get('FuelGroupTable').clear();
                this.rForm.get('FuelGroupTable').push(this.LoadFuelGroupTable(_fr));
            }
            if (_fr.FreightRateRouteTables != null && _fr.FreightRateRouteTables.length > 0) {
                let rTable = this.rForm.get('RangeTable');
                rTable.clear();
                const ftable = _fr.FreightRateRouteTables;
                this.groupBy(ftable, ftable => ftable.StartQuantity).forEach(res => {
                    this.rForm.get('RangeTable').push(this.LoadRouteTable(false, res, _fr.Id));
                });
                rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
                this.freightComponent.IsLoaded = true;
            }
            else if (_fr.FreightRateRangeTables != null && _fr.FreightRateRangeTables.length > 0) {
                this.generateRangeTable(_fr.FreightRateRangeTables);
            }
            else if (_fr.FreightRatePtoPTables != null && _fr.FreightRatePtoPTables.length > 0) {
                this.generateP2PTable(_fr.FreightRatePtoPTables);
            }
            this.rerender_trigger();
        }
        if (this.RuleMode == "VIEW") {
            this.disableInputControls = true;
            this.freightComponent.disableInputControls = true;
        }
    }
    groupBy(list, keyGetter) {
        const map = new Map();
        list.forEach((item) => {
            const key = keyGetter(item);
            const collection = map.get(key);
            if (!collection) {
                map.set(key, [item]);
            }
            else {
                collection.push(item);
            }
        });
        return map;
    }
    groupByWithMultipleProperty(array, f) {
        let groups = {};
        array.forEach(function (o) {
            var group = JSON.stringify(f(o));
            groups[group] = groups[group] || [];
            groups[group].push(o);
        });
        return Object.keys(groups).map(function (group) {
            return groups[group];
        });
    }
    generateP2PTable(fRange, Id) {
        this.freightComponent.isLoadingSubject.next(true);
        if (fRange != null && fRange.length > 0) {
            let rTable = this.rForm.get('RangeTable');
            rTable.clear();
            let ftable = this.groupByWithMultipleProperty(fRange, function (item) {
                return [item.TerminalAndBulkPlantName, item.LocationName, item.LocationAddress, item.LaneID, item.AssumedMiles];
            });
            let sameName = "";
            ftable.forEach(res => {
                this.rForm.get('RangeTable').push(this.LoadP2PTable(sameName == "" || sameName != res[0].TerminalAndBulkPlantName ? true : false, res, Id));
                sameName = res[0].TerminalAndBulkPlantName;
            });
        }
        this.freightComponent.ShowMessage = false;
        this.freightComponent.isLoadingSubject.next(false);
        this.freightComponent.IsLoaded = true;
    }
    generateRangeTable(fRange, Id) {
        this.freightComponent.isLoadingSubject.next(true);
        if (fRange != null && fRange.length > 0) {
            let rTable = this.rForm.get('RangeTable');
            rTable.clear();
            const ftable = fRange;
            this.groupBy(ftable, ftable => ftable.UptoQuantity).forEach(res => {
                this.rForm.get('RangeTable').push(this.LoadRangeTable(false, res, Id));
            });
            rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
        }
        this.freightComponent.ShowMessage = false;
        this.freightComponent.isLoadingSubject.next(false);
        this.freightComponent.IsLoaded = true;
    }
    LoadP2PTable(IsLast, rt, Id) {
        return this._fb.group({
            Id: this._fb.control(Id),
            TerminalAndBulkPlantName: this._fb.control(rt[0].TerminalAndBulkPlantName),
            TerminalId: this._fb.control(rt[0].TerminalId),
            BulkPlantId: this._fb.control(rt[0].BulkPlantId),
            LocationName: this._fb.control(rt[0].LocationName),
            LocationAddress: this._fb.control(rt[0].LocationAddress),
            JobId: this._fb.control(rt[0].JobId),
            LaneID: this._fb.control(rt[0].LaneID),
            AssumedMiles: this._fb.control(rt[0].AssumedMiles, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
            IsLaneRequired: this._fb.control(rt[0].IsLaneRequired),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInP2PTable(rt)
        });
    }
    LoadRangeTable(IsLast, rt, Id) {
        return this._fb.group({
            Id: this._fb.control(Id),
            UptoQuantity: this._fb.control(rt[0].UptoQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInRangeTable(rt)
        });
    }
    LoadGroupInP2PTable(rt) {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.FuelGroupId),
                FuelGroupName: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.FuelGroupName),
                RateValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.RateValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    LoadGroupInRangeTable(rt) {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.FuelGroupId),
                RateValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.RateValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    LoadRouteTable(IsLast, rt, Id) {
        return this._fb.group({
            Id: this._fb.control(Id),
            StartQuantity: this._fb.control(rt[0].StartQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
            EndQuantity: this._fb.control(rt[0].EndQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInRouteTable(rt)
        });
    }
    LoadGroupInRouteTable(rt) {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.FuelGroupId),
                RateValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.RateValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    LoadFuelGroupTable(_fr) {
        if (_fr.FreightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            return this._fb.group({
                group: this.LoadGroupInFuelGroupTable(_fr)
            });
        }
        else if (_fr.FreightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range || _fr.FreightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            let selectedFuelGroup = [];
            if (_fr.FreightRateCalcPrefFuelGroupId != null) {
                selectedFuelGroup = this.freightComponent.FuelGroupsList.filter(res => res.Id == _fr.FreightRateCalcPrefFuelGroupId);
            }
            return this._fb.group({
                group: this.LoadGroupInFuelGroupTable(_fr),
                FreightRateCalcPreferenceType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](_fr.FreightRateCalcPreferenceType),
                FuelGroups: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](selectedFuelGroup, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
                MixLoadMinValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](_fr.MixLoadMinValue, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            });
        }
    }
    LoadGroupInFuelGroupTable(_fr) {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        _fr.FreightRateFuelGroups.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.FuelGroupId),
                MinQuantity: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.MinQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    createForm(ruleType) {
        if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            this.rForm = this._fb.group({
                RuleId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](''),
                RuleType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](ruleType),
                FuelGroupTable: this._fb.array([]),
                RangeTable: this._fb.array([])
            });
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            this.rForm = this._fb.group({
                RuleId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](''),
                RuleType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](ruleType),
                FuelGroupTable: this._fb.array([]),
                RangeTable: this._fb.array([])
            });
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            this.rForm = this._fb.group({
                RuleId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](''),
                RuleType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](ruleType),
                FuelGroupTable: this._fb.array([]),
                RangeTable: this._fb.array([])
            });
        }
    }
    changeViewType(value) {
        this.viewType = value;
    }
    RuleTypeTypeChange(ruleType) {
        this.freightRateRuleType = ruleType;
        this.refreshUI(ruleType);
    }
    refreshUI(ruleType) {
        this.freightComponent.IsLoaded = false;
        this.freightComponent.isLoadingSubject.next(true);
        this.RuleId = null;
        this.IsEditable = true;
        this.createForm(ruleType);
        this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master));
        this.freightComponent.SourceRegionList = [];
        this.freightComponent.TerminalsAndBulkPlantList = [];
        this.freightComponent.rcForm.get('TableName').patchValue('');
        this.freightComponent.rcForm.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(moment__WEBPACK_IMPORTED_MODULE_7__(new Date()).toDate()).format('MM/DD/YYYY'));
        this.freightComponent.FuelGroupsList = [];
        this.freightComponent.rcForm.get('FuelGroups').patchValue([]);
        if (ruleType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeStartValue]);
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeEndValue]);
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeInterval]);
        }
        else {
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeStartValue], null);
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeEndValue], null);
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeInterval], null);
        }
        if (ruleType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master));
            this.freightComponent.onTableTypeSelect({ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master });
            this.DtTrigger.next();
        }
        else {
            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer));
            this.freightComponent.onTableTypeSelect({ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer });
        }
        this.freightComponent.IsLoaded = true;
        this.freightComponent.isLoadingSubject.next(false);
        //this.rerender();
    }
    disableAllControl(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.disable();
            if (control.controls) {
                this.disableAllControl(control);
            }
        });
    }
    markFormGroupTouched(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
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
    onSubmit(freightTableStatus) {
        this.IsLoading = true;
        if (freightTableStatus == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
            if (this.rForm.get('RuleId').value != "") {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Not allowed. " + this.freightComponent.rcForm.get('TableName').value + " is in edit mode.", undefined, undefined);
                this.IsLoading = false;
                return;
            }
            this.clearAllValidations(this.rForm); // clear all validation
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.TableName], null); // minimum validation for draft
            if (this.freightComponent.rcForm.valid)
                this.Save(freightTableStatus);
            this.IsLoading = false;
            return;
        }
        this.freightComponent.ValidateOnSubmit(freightTableStatus);
        if (this.RuleMode == "COPY" && !this.freightComponent.rcForm.get('TableName').valid) {
            this.freightComponent.ShowMessage = false;
        }
        // clear validation of last row and second column of RangeTable
        let rTable = this.rForm.get('RangeTable');
        if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            rTable.at(rTable.length - 1).get('EndQuantity').clearValidators();
            rTable.at(rTable.length - 1).get('EndQuantity').updateValueAndValidity();
            rTable.at(rTable.length - 1).get('EndQuantity').markAsTouched();
        }
        else if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.get('RangeStartValue'), this.freightComponent.rcForm.get('RangeEndValue'), this.freightComponent.rcForm.get('RangeInterval')]);
            let selectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value;
            if (selectedFuelGroups.length == 1) {
                this.AddRemoveValidations(null, [this.rForm.get('FuelGroupTable').at(0).get('MixLoadMinValue'), this.rForm.get('FuelGroupTable').at(0).get('FuelGroups')]);
            }
            this.ClearDuplicateUpToQuantity();
        }
        else if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            let selectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value;
            if (selectedFuelGroups.length == 1) {
                this.AddRemoveValidations(null, [this.rForm.get('FuelGroupTable').at(0).get('MixLoadMinValue'), this.rForm.get('FuelGroupTable').at(0).get('FuelGroups')]);
            }
        }
        if (rTable.length > 0 &&
            (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P || +this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) &&
            ((this.rForm.get('FuelGroupTable').at(0).get('FreightRateCalcPreferenceType').value != 1) ||
                this.rForm.get('FuelGroupTable').at(0).get('MixLoadMinValue').value == 0)) {
            this.AddRemoveValidations(null, [this.rForm.get('FuelGroupTable').at(0).get('FuelGroups')]);
        }
        this.markFormGroupTouched(this.rForm);
        this.IsLoading = false;
        if (this.rForm.valid && this.freightComponent.rcForm.valid && !this.freightComponent.ShowMessage) {
            this.Save(freightTableStatus);
        }
    }
    IsGroupExist(table) {
        let fgTable = this.rForm.get(table);
        let fgtableArray = fgTable.value;
        return fgtableArray.findIndex((s) => s['group']);
    }
    IsDuplicateUpToQuantity() {
        let UptoQuantity = [];
        let rangeTable = this.rForm.get('RangeTable');
        let isExist = this.IsGroupExist("RangeTable");
        let flag = false;
        if (isExist != -1) {
            var rT = this.rForm.get('RangeTable').length - 1;
            for (let i = 0; i <= rT; i++) {
                if (!UptoQuantity.includes(+(rangeTable.at(i).get('UptoQuantity').value))) {
                    UptoQuantity.push(+(rangeTable.at(i).get('UptoQuantity').value));
                }
                else {
                    rangeTable.at(i).get('UptoQuantity').setErrors({ DuplicateEntry: true });
                    rangeTable.at(i).get('UptoQuantity').markAsTouched();
                    flag = true;
                }
            }
        }
        return flag;
    }
    ClearDuplicateUpToQuantity() {
        var _a, _b;
        let rangeTable = this.rForm.get('RangeTable');
        let isExist = this.IsGroupExist("RangeTable");
        if (isExist != -1) {
            var rT = this.rForm.get('RangeTable').length - 1;
            for (let i = 0; i <= rT; i++) {
                if ((_b = (_a = rangeTable.at(i).get('UptoQuantity')) === null || _a === void 0 ? void 0 : _a.errors) === null || _b === void 0 ? void 0 : _b.DuplicateEntry) {
                    rangeTable.at(i).get('UptoQuantity').markAsTouched();
                }
            }
        }
    }
    Save(freightTableStatus) {
        this.IsLoading = true;
        var saveModel = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRateViewModel"]();
        saveModel.Id = this.rForm.get('RuleId').value;
        saveModel.Name = this.freightComponent.rcForm.get('TableName').value;
        saveModel.Status = freightTableStatus;
        saveModel.FuelGroupType = this.freightComponent.rcForm.get('FuelGroupType').value;
        saveModel.FreightRateRuleType = this.rForm.get('RuleType').value;
        saveModel.StartDate = this.freightComponent.rcForm.get('StartDate').value;
        if (this.freightComponent.rcForm.get('EndDate').value != null && this.freightComponent.rcForm.get('EndDate').value != undefined) {
            saveModel.EndDate = this.freightComponent.rcForm.get('EndDate').value;
        }
        saveModel.TableType = this.freightComponent.rcForm.get('TableTypes').value[0].Id;
        this.freightComponent.rcForm.get('Customers').value.forEach(res => saveModel.CustomerIds.push(res.Id));
        this.freightComponent.rcForm.get('Carriers').value.forEach(res => saveModel.CarrierIds.push(res.Id));
        this.freightComponent.rcForm.get('SourceRegions').value.forEach(res => saveModel.SourceRegionIds.push(res.Id));
        saveModel.TerminalsAndBulkPlants = this.freightComponent.rcForm.get('TerminalsAndBulkPlants').value;
        let isExist = 0;
        isExist = this.IsGroupExist("FuelGroupTable");
        if (isExist != -1) {
            var gList = (this.rForm.get('FuelGroupTable').at(0).get('group')).length - 1;
            var fgT = this.rForm.get('FuelGroupTable').length - 1;
            for (let i = 0; i <= fgT; i++) {
                for (let j = 0; j <= gList; j++) {
                    let group = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRateFuelGroupViewModel"]();
                    group.FuelGroupId = (this.rForm.get('FuelGroupTable').at(i).get('group')).at(j).get('FuelGroupId').value;
                    group.MinQuantity = (this.rForm.get('FuelGroupTable').at(i).get('group')).at(j).get('MinQuantity').value;
                    if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range || +this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                        saveModel.MixLoadMinValue = this.rForm.get('FuelGroupTable').at(i).get('MixLoadMinValue').value;
                        saveModel.FreightRateCalcPreferenceType = this.rForm.get('FuelGroupTable').at(i).get('FreightRateCalcPreferenceType').value;
                        let selectedFuelGroup = this.rForm.get('FuelGroupTable').at(i).get('FuelGroups').value;
                        saveModel.FreightRateCalcPrefFuelGroupId = selectedFuelGroup.length == 1 ? selectedFuelGroup[0].Id : null;
                    }
                    saveModel.FreightRateFuelGroups.push(group);
                }
            }
        }
        isExist = this.IsGroupExist("RangeTable");
        if (isExist != -1) {
            gList = (this.rForm.get('RangeTable').at(0).get('group')).length - 1;
            var rT = this.rForm.get('RangeTable').length - 1;
            let rangeTable = this.rForm.get('RangeTable');
            if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
                for (let i = 0; i <= rT; i++) {
                    for (let j = 0; j <= gList; j++) {
                        let group = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRateRouteTableViewModel"]();
                        group.FuelGroupId = (rangeTable.at(i).get('group')).at(j).get('FuelGroupId').value;
                        group.StartQuantity = rangeTable.at(i).get('StartQuantity').value;
                        if (rangeTable.at(i).get('IsLast').value != true) {
                            group.EndQuantity = rangeTable.at(i).get('EndQuantity').value;
                        }
                        else {
                            group.EndQuantity = null;
                        }
                        group.RateValue = (rangeTable.at(i).get('group')).at(j).get('RateValue').value;
                        saveModel.FreightRateRouteTables.push(group);
                    }
                }
            }
            else if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
                if (this.IsDuplicateUpToQuantity()) {
                    this.IsLoading = false;
                    return;
                }
                for (var i = 0; i <= rT; i++) {
                    for (var j = 0; j <= gList; j++) {
                        let group = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRateRangeTableViewModel"]();
                        group.FuelGroupId = (rangeTable.at(i).get('group')).at(j).get('FuelGroupId').value;
                        group.UptoQuantity = rangeTable.at(i).get('UptoQuantity').value;
                        group.RateValue = (rangeTable.at(i).get('group')).at(j).get('RateValue').value;
                        saveModel.FreightRateRangeTables.push(group);
                    }
                }
            }
            else if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                for (let i = 0; i <= rT; i++) {
                    for (let j = 0; j <= gList; j++) {
                        let group = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRatePtoPTable"]();
                        group.FuelGroupId = (rangeTable.at(i).get('group')).at(j).get('FuelGroupId').value;
                        group.FuelGroupName = (rangeTable.at(i).get('group')).at(j).get('FuelGroupName').value;
                        group.RateValue = (rangeTable.at(i).get('group')).at(j).get('RateValue').value;
                        group.AssumedMiles = rangeTable.at(i).get('AssumedMiles').value;
                        group.IsLaneRequired = rangeTable.at(i).get('IsLaneRequired').value;
                        group.LaneID = rangeTable.at(i).get('LaneID').value;
                        group.LocationAddress = rangeTable.at(i).get('LocationAddress').value;
                        group.LocationName = rangeTable.at(i).get('LocationName').value;
                        group.JobId = rangeTable.at(i).get('JobId').value;
                        group.TerminalAndBulkPlantName = rangeTable.at(i).get('TerminalAndBulkPlantName').value;
                        group.BulkPlantId = rangeTable.at(i).get('BulkPlantId').value;
                        group.TerminalId = rangeTable.at(i).get('TerminalId').value;
                        saveModel.FreightRatePtoPTables.push(group);
                    }
                }
            }
        }
        if (this.rForm.get('RuleId').value != "") {
            this.freightRateRulesService.updateFreightRate(saveModel)
                .subscribe((response) => {
                this.ServiceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    let message = " edited";
                    if (saveModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                        message = " saved draft";
                    }
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(saveModel.Name + message + " successfully.", undefined, undefined);
                    this.IsLoading = false;
                    this.changeToViewTab();
                }
                else {
                    this.IsLoading = false;
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            this.freightRateRulesService.createFreightRate(saveModel)
                .subscribe((response) => {
                this.ServiceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    let message = "";
                    if (saveModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
                        message = " created";
                    }
                    else if (saveModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                        message = " saved draft";
                    }
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(saveModel.Name + message + " successfully.", undefined, undefined);
                    this.IsLoading = false;
                    this.changeToViewTab();
                }
                else {
                    this.IsLoading = false;
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        }
    }
    changeToViewTab() {
        this.freightRateRulesService.onSelectedTabChanged.next(2);
    }
    clearForm() {
        this.refreshUI(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range);
    }
    onCancel() {
        if (this.RuleMode == "VIEW") {
            this.disableInputControls = false;
            this.freightComponent.disableInputControls = false;
            this.RuleId = null;
        }
        if (this.RuleId != null) {
            this.changeToViewTab();
        }
        else {
            this.refreshUI(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range);
        }
    }
    ngOnDestroy() {
        this.DtTrigger.unsubscribe();
    }
    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                //console.log(other + " : " + current.Id);
                return other == current.Id;
            }).length == 1;
        };
    }
    IdInComparers(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id;
            }).length == 1;
        };
    }
    GetControlNames(formGroup) {
        var ctrlName = [];
        Object.keys(formGroup.controls).forEach((key) => {
            const abstractControl = formGroup.get(key);
            ctrlName.push(key);
            if (abstractControl instanceof _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroup"]) {
                this.GetControlNames(abstractControl);
            }
        });
        return ctrlName;
    }
    onStartQuantityLostFocus(index) {
        let rTable = this.rForm.get('RangeTable');
        let StartQuantity = rTable.at(index).get('StartQuantity').value;
        let UpperRowEndQuantity;
        if (index > 0) {
            UpperRowEndQuantity = rTable.at(index - 1).get('EndQuantity').value;
            let SameRowEndQuantity = rTable.at(index).get('EndQuantity').value;
            if (+StartQuantity <= +UpperRowEndQuantity) {
                rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
            else if (+StartQuantity >= SameRowEndQuantity) {
                rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
            else {
                rTable.at(index).get('StartQuantity').setErrors(null);
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
        }
        else {
            UpperRowEndQuantity = rTable.at(index).get('EndQuantity').value;
            if (UpperRowEndQuantity) {
                if (+StartQuantity >= +UpperRowEndQuantity) {
                    rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                    rTable.at(index).get('StartQuantity').markAsTouched();
                }
                else {
                    rTable.at(index).get('StartQuantity').setErrors(null);
                    rTable.at(index).get('StartQuantity').markAsTouched();
                }
            }
        }
    }
    onEndQuantityLostFocus(index) {
        let rTable = this.rForm.get('RangeTable');
        let StartQuantity = rTable.at(index).get('StartQuantity').value;
        let EndQuantity = rTable.at(index).get('EndQuantity').value;
        if (!isNaN(+EndQuantity) && !isNaN(+StartQuantity)) {
            if (+StartQuantity >= +EndQuantity) {
                rTable.at(index).get('EndQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('EndQuantity').markAsTouched();
            }
            else {
                rTable.at(index).get('EndQuantity').setErrors(null);
                rTable.at(index).get('EndQuantity').markAsTouched();
            }
        }
        this.onStartQuantityLostFocus(index);
    }
    onStartQuantityKeyPress(event, index) {
        if (index == 0 && !isNaN(event.data)) {
            var gList = (this.rForm.get('FuelGroupTable').at(0).get('group')).length - 1;
            var fgT = this.rForm.get('FuelGroupTable').length - 1;
            let StartQuantity = this.rForm.get('RangeTable').at(index).get('StartQuantity').value;
            for (let i = 0; i <= fgT; i++) {
                for (let j = 0; j <= gList; j++) {
                    (this.rForm.get('FuelGroupTable').at(i).get('group')).at(j).get('MinQuantity').patchValue(+StartQuantity);
                }
            }
        }
    }
    onEndQuantityKeyPress(index) {
        let rTable = this.rForm.get('RangeTable');
        let EndQuantity = rTable.at(index).get('EndQuantity').value;
        if (!isNaN(+EndQuantity)) {
            if (+EndQuantity) {
                rTable.at(index + 1).get('StartQuantity').patchValue((+EndQuantity + 1));
            }
        }
    }
    AddRange(srNmber) {
        this.rerender_destroy();
        if (this.rForm.get('FuelGroupTable').length == 0) {
            return;
        }
        ;
        let rTable = this.rForm.get('RangeTable');
        let EndQuantity = "";
        if (rTable.length > 0 && +this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            rTable.at(rTable.length - 1).get('IsLast').patchValue(false);
            EndQuantity = rTable.at(rTable.length - 2).get('EndQuantity').value;
            if (EndQuantity != "")
                rTable.at(rTable.length - 1).get('StartQuantity').patchValue((+EndQuantity + 1));
            rTable.push(this.CreateRangeTable(true, +this.rForm.get('RuleType').value));
        }
        else if (rTable.length > 0 && +this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            let UptoQuantity = rTable.at(srNmber).get('UptoQuantity').value;
            rTable.insert(srNmber + 1, this.CreateRangeTable(false, +this.rForm.get('RuleType').value));
            /*if (UptoQuantity != "") rTable.at(srNmber + 1).get('UptoQuantity').patchValue(0);*/
            rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
        }
        // no manaul row add for P2P
        else {
            if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
                rTable.push(this.CreateRangeTable(false, +this.rForm.get('RuleType').value));
                rTable.push(this.CreateRangeTable(true, +this.rForm.get('RuleType').value));
            }
            else if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
                this.markFormGroupTouched(this.rForm);
                if (!this.freightComponent.rcForm.get('RangeStartValue').valid
                    || !this.freightComponent.rcForm.get('RangeStartValue').valid
                    || !this.freightComponent.rcForm.get('RangeInterval').valid) {
                    return;
                }
                let start = this.freightComponent.rcForm.get('RangeStartValue').value;
                let stop = this.freightComponent.rcForm.get('RangeEndValue').value;
                let step = this.freightComponent.rcForm.get('RangeInterval').value;
                this.GenerateRange(+start, +stop, +step).forEach(res => {
                    rTable.push(this.CreateRangeTable(false, +this.rForm.get('RuleType').value, Math.round(res)));
                });
            }
            else if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                if (!this.freightComponent.rcForm.get('TerminalsAndBulkPlants').valid
                    || !this.freightComponent.rcForm.get('Locations').valid) {
                    return;
                }
                let trbls = this.freightComponent.rcForm.get('TerminalsAndBulkPlants').value;
                function sortForTerminalsAndBulkPlants(a, b) {
                    if (a.Name < b.Name) {
                        return -1;
                    }
                    if (a.Name > b.Name) {
                        return 1;
                    }
                    return 0;
                }
                trbls.sort(sortForTerminalsAndBulkPlants);
                let sLocations = this.freightComponent.rcForm.get('Locations').value;
                let JobIds = [];
                sLocations.forEach(s => JobIds.push(s.Id));
                let locs = this.freightComponent.LocationList.filter(this.IdInComparer(JobIds));
                for (var i = 0; i < trbls.length; i++) {
                    for (var j = 0; j < locs.length; j++) {
                        rTable.push(this.CreateRangeTable(false, +this.rForm.get('RuleType').value));
                        rTable.at(rTable.length - 1).get("TerminalAndBulkPlantName").patchValue(trbls[i].Name);
                        if (trbls[i].Id.startsWith('Terminals_')) {
                            rTable.at(rTable.length - 1).get("TerminalId").patchValue(trbls[i].Id.split("Terminals_")[1]);
                            rTable.at(rTable.length - 1).get("LaneID").patchValue(trbls[i].Id.split("Terminals_")[1].concat(" " + locs[j].Id.toString()));
                        }
                        else if (trbls[i].Id.startsWith('BulkPlants_')) {
                            rTable.at(rTable.length - 1).get("BulkPlantId").patchValue(trbls[i].Id.split("BulkPlants_")[1]);
                            rTable.at(rTable.length - 1).get("LaneID").patchValue(trbls[i].Id.split("BulkPlants_")[1].concat(" " + locs[j].Id.toString()));
                        }
                        rTable.at(rTable.length - 1).get("LocationName").patchValue(locs[j].Name);
                        rTable.at(rTable.length - 1).get("LocationAddress").patchValue(locs[j].Code);
                        rTable.at(rTable.length - 1).get("JobId").patchValue(locs[j].Id);
                        if (j == 0) {
                            rTable.at(rTable.length - 1).get("IsLast").patchValue(true);
                        }
                    }
                }
                //this.DtTrigger.next();
                this.rerender_trigger();
            }
        }
    }
    GenerateRange(start, end, step = 0) {
        let output = [];
        if (typeof end === 'undefined') {
            end = start;
            start = 0;
        }
        for (let i = start; i <= end; i += step) {
            output.push(i);
        }
        return output;
    }
    onEndQuantityChange(textInput, index) {
        if (this.intGreaterThanZeroRegx.test(textInput)) {
            this.rForm.get('RangeTable').at(index + 1).get('StartQuantity').patchValue((+textInput + 1));
        }
        else {
            this.rForm.get('RangeTable').at(index + 1).get('StartQuantity').patchValue('');
        }
    }
    CreateRangeTable(IsLast, ruleType, upTo) {
        if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            return this._fb.group({
                Id: this._fb.control(''),
                StartQuantity: this._fb.control(IsLast ? '' : 1, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
                EndQuantity: this._fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            return this._fb.group({
                Id: this._fb.control(''),
                UptoQuantity: this._fb.control(upTo, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            return this._fb.group({
                Id: this._fb.control(''),
                TerminalAndBulkPlantName: this._fb.control(''),
                TerminalId: this._fb.control(''),
                BulkPlantId: this._fb.control(''),
                LocationName: this._fb.control(''),
                LocationAddress: this._fb.control(''),
                JobId: this._fb.control(''),
                LaneID: this._fb.control(''),
                AssumedMiles: this._fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]),
                IsLaneRequired: this._fb.control(true),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }
    }
    addGroupInRangeTable() {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        this.freightComponent.rcForm.get('FuelGroups').value.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.Id),
                FuelGroupName: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.Name),
                RateValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    generateOutput() {
        this.freightComponent.generateTable();
    }
    getOutput($event) {
        this.rForm.get('RangeTable').clear();
        this.AddRange(0);
    }
    onFuelGroupChange($event) {
        this.rForm.get('FuelGroupTable').clear();
        if ((+this.rForm.get('RuleType').value) == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            this.freightComponent.rcForm.get('RangeStartValue').patchValue('');
            this.freightComponent.rcForm.get('RangeEndValue').patchValue('');
            this.freightComponent.rcForm.get('RangeInterval').patchValue('');
        }
        this.AddFuelGroup(+this.rForm.get('RuleType').value);
    }
    AddFuelGroup(ruleType) {
        this.rForm.get('FuelGroupTable').push(this.CreateFuelGroupTable(ruleType));
    }
    CreateFuelGroupTable(ruleType) {
        this.SelectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value;
        if (this.SelectedFuelGroups.length == 0) {
            return this._fb.group({});
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            return this._fb.group({
                group: this.addFuelGroupTable()
            });
        }
        else if (ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range || ruleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            return this._fb.group({
                group: this.addFuelGroupTable(),
                FreightRateCalcPreferenceType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](1),
                FuelGroups: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
                MixLoadMinValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](0, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            });
        }
    }
    addFuelGroupTable() {
        let fg = new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([]);
        this.freightComponent.rcForm.get('FuelGroups').value.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.Id),
                FuelGroupName: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](x.Name),
                MinQuantity: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](1, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }
    AddRanges(srNmber) {
        this.AddRange(srNmber);
    }
    RemoveRanges(srNmber) {
        let rTable = this.rForm.get('RangeTable');
        if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Route) {
            if (rTable.length > 2) {
                rTable.removeAt(rTable.length - 1);
                // rTable.removeAt(rTable.length - 1);
                if (rTable.length > 0)
                    rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
            }
        }
        else if (+this.rForm.get('RuleType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            if (rTable.length > 1) {
                rTable.removeAt(srNmber);
                if (rTable.length > 0)
                    rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
            }
        }
    }
    isNumber(n) {
        return /^-?[\d.]+(?:e-?\d+)?$/.test(n);
    }
    onImportClick($event) {
        let rows = [];
        let columns = [];
        let firstRow = [];
        rows = $event.split(/\r?\n/);
        //sainity check 1
        columns = rows[0].split(","); //first row
        firstRow = rows[0].split(","); //first row
        if (this.freightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            let frs = [];
            if (columns.length - 1 != this.SelectedFuelGroups.length || columns[0] != "UptoQuantity") {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Template mistmach with selected fuel group name.", undefined, undefined);
                return;
            }
            let i = 0;
            for (i = 0; i < this.SelectedFuelGroups.length; i++) {
                if (this.SelectedFuelGroups[i].Name != columns[i + 1]) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Template mistmach with selected fuel group name order.", undefined, undefined);
                    return;
                }
            }
            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1)
                    break;
                for (let j = 0; j < columns.length; j++) {
                    let str = columns[j].replace(/\s/g, "");
                    if (!this.isNumber(str)) {
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Invalid number " + str + ".", undefined, undefined);
                        return;
                    }
                }
            }
            rows = $event.split(/\r?\n/);
            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1)
                    break;
                //let fr = new FreightRateRangeTableViewModel();
                let frtVM = { UptoQuantity: 0, FuelGroupId: 0, RateValue: 0 };
                for (let j = 0; j < columns.length; j++) {
                    let str = columns[j].replace(/\s/g, "");
                    if (j == 0) {
                        frtVM.UptoQuantity = +str;
                    }
                    else {
                        frtVM.FuelGroupId = this.SelectedFuelGroups.filter(r => r.Name == firstRow[j])[0].Id;
                        frtVM.RateValue = +str;
                        frs.push({ UptoQuantity: frtVM.UptoQuantity, FuelGroupId: frtVM.FuelGroupId, RateValue: frtVM.RateValue });
                    }
                }
            }
            this.generateRangeTable(frs);
        }
        else if (this.freightRateRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            //TODO : validation is not yet converd.
            let frP2P = [];
            rows = $event.split(/\r?\n/);
            let i = 0;
            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1)
                    break;
                //Terminal/Bulk Plants,Location Name,Location Address,LaneId,Assumed Miles,FuelGroupStd1,FuelGroupStd2,IsLaneRequired
                //Terminals_811|Buckeye - Roseton,14038|loc 1 sep,57 wall street,,,,,
                let row = columns[0].split("|");
                let terminalInfo = { TerminalId: "", TerminalAndBulkPlantName: "" };
                let bulkPlantInfo = { BulkPlantId: "", TerminalAndBulkPlantName: "" };
                if (columns[0].startsWith('Terminals_')) {
                    terminalInfo = { TerminalId: row[0].trim(), TerminalAndBulkPlantName: row[1].trim() };
                }
                else if (columns[0].startsWith('BulkPlants_')) {
                    bulkPlantInfo = { BulkPlantId: row[0].trim(), TerminalAndBulkPlantName: row[1].trim() };
                }
                row = columns[1].split("|");
                let locationInfo = {
                    JobId: row[0].trim(), LocationName: row[1], LocationAddress: columns[2].trim()
                };
                //let lastPipline = row[2].split(","); //loc 1 sep,57 wall street,laneId,assumedMile,fg1,fg2,islaneRequired
                let LaneId = columns[3].trim();
                let AssumedMiles = columns[4].trim();
                let IsLaneRequired = columns[columns.length - 1].trim();
                for (let j = 0; j < this.SelectedFuelGroups.length; j++) {
                    let fp2p = new _Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightRatePtoPTable"]();
                    fp2p.TerminalId = terminalInfo.TerminalId == "" ? null : +terminalInfo.TerminalId.split('_')[1];
                    fp2p.BulkPlantId = bulkPlantInfo.BulkPlantId == "" ? null : +bulkPlantInfo.BulkPlantId.split('_')[1];
                    fp2p.TerminalAndBulkPlantName = terminalInfo.TerminalId == "" ? bulkPlantInfo.TerminalAndBulkPlantName : terminalInfo.TerminalAndBulkPlantName;
                    fp2p.LocationName = locationInfo.LocationName;
                    fp2p.LocationAddress = locationInfo.LocationAddress;
                    fp2p.JobId = +locationInfo.JobId;
                    fp2p.LaneID = LaneId;
                    fp2p.AssumedMiles = AssumedMiles;
                    fp2p.IsLaneRequired = (IsLaneRequired == "0") ? false : true;
                    fp2p.FuelGroupId = this.SelectedFuelGroups[0].Id;
                    fp2p.RateValue = +columns[4 + j].trim();
                    frP2P.push(fp2p);
                }
            }
            //this.freightComponent.closePopup();
            this.generateP2PTable(frP2P);
        }
        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess("Import successful.", undefined, undefined);
    }
}
CreateFreightRateRules.ɵfac = function CreateFreightRateRules_Factory(t) { return new (t || CreateFreightRateRules)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_10__["FreightRateRulesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"])); };
CreateFreightRateRules.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: CreateFreightRateRules, selectors: [["app-create-freight-rate-rules"]], viewQuery: function CreateFreightRateRules_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_8__["FreightComponent"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.freightComponent = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
    } }, decls: 28, vars: 18, consts: [[3, "formGroup", "ngSubmit"], [4, "ngIf"], [3, "ngClass", "disabled"], [1, "row", "mt-2"], [1, "col-sm-3"], [1, "form-group"], [1, "form-check", "form-check-inline"], ["type", "radio", "formControlName", "RuleType", "id", "Range", 1, "form-check-input", 3, "name", "value", "change"], ["for", "Range", 1, "form-check-label"], ["type", "radio", "formControlName", "RuleType", "id", "P2P", 1, "form-check-input", 3, "name", "value", "change"], ["for", "P2P", 1, "form-check-label"], ["type", "radio", "formControlName", "RuleType", "id", "Route", 1, "form-check-input", 3, "name", "value", "change"], ["for", "Route", 1, "form-check-label"], [3, "PricingRuleType", "IsEditable", "onGenerateTable", "onGenerateFuelGroup", "onImportClick"], ["class", "well bg-white", 4, "ngIf"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "button", 1, "btn", "btn-lg", "btn-outline-primary", 3, "disabled", "click"], ["type", "submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled"], ["class", "loader", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "well", "bg-white"], ["class", "row", 4, "ngIf"], [1, "row"], [1, "col-sm-6"], [1, "mb-1"], [1, "table", "table-bordered"], [4, "ngFor", "ngForOf"], ["formArrayName", "FuelGroupTable"], [3, "formGroupName"], ["formArrayName", "group"], ["type", "text", "formControlName", "MinQuantity", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "color-maroon"], [1, "table", "table-bordered", "table-hover", "mt-3"], ["colspan", "3", "align", "center"], ["formArrayName", "RangeTable"], ["type", "text", "formControlName", "StartQuantity", 1, "form-control", 3, "input", "change"], ["type", "text", "formControlName", "EndQuantity", 1, "form-control", 3, "input", "change"], ["type", "text", "formControlName", "RateValue", 1, "form-control"], ["type", "button", "title", "Add", 1, "fa", "fa-plus-circle", "mt4", "mr5", "mr10", 3, "click"], ["type", "button", "title", "Remove", 1, "fa", "fa-trash-alt", "mt7", "color-maroon", "remove-partial-block", "tier", 3, "click"], [1, "row", 3, "ngClass"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-p2p-rate-table", 1, "table-responsive"], ["datatable", "", 1, "table", "table-bordered", 3, "dtOptions", "dtTrigger"], ["data-key", "ptbp"], ["data-key", "pln"], ["data-key", "pla"], ["data-key", "pli"], ["data-key", "pam"], ["data-key", "plr"], ["type", "text", "formControlName", "MixLoadMinValue", 1, "form-control"], [1, "row", "mb-2"], [1, "col-sm-12"], ["type", "radio", "formControlName", "FreightRateCalcPreferenceType", "id", "RoundUp", 1, "form-check-input", 3, "name", "value"], ["for", "RoundUp", 1, "form-check-label"], ["type", "radio", "formControlName", "FreightRateCalcPreferenceType", "id", "Proportion", 1, "form-check-input", 3, "name", "value"], ["for", "Proportion", 1, "form-check-label"], ["formControlName", "FuelGroups", "id", "fuelGroup", 1, "single-select", 3, "placeholder", "settings", "data"], ["data-key", "item.Name"], [1, "break-word"], ["type", "text", "formControlName", "AssumedMiles", 1, "form-control"], [1, "text-center"], ["type", "checkbox", "formControlName", "IsLaneRequired", 1, "form-check-input", 3, "value"], ["type", "text", "formControlName", "UptoQuantity", 1, "form-control"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateFreightRateRules_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function CreateFreightRateRules_Template_form_ngSubmit_0_listener() { return ctx.onSubmit(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateFreightRateRules_div_2_Template, 4, 0, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "fieldset", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateFreightRateRules_Template_input_change_8_listener() { return ctx.RuleTypeTypeChange(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateFreightRateRules_Template_input_change_12_listener() { return ctx.RuleTypeTypeChange(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "P2P");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateFreightRateRules_Template_input_change_16_listener() { return ctx.RuleTypeTypeChange(3); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Route");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "app-freight", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onGenerateTable", function CreateFreightRateRules_Template_app_freight_onGenerateTable_19_listener($event) { return ctx.getOutput($event); })("onGenerateFuelGroup", function CreateFreightRateRules_Template_app_freight_onGenerateFuelGroup_19_listener($event) { return ctx.onFuelGroupChange($event); })("onImportClick", function CreateFreightRateRules_Template_app_freight_onImportClick_19_listener($event) { return ctx.onImportClick($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, CreateFreightRateRules_div_20_Template, 4, 3, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "input", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_Template_input_click_22_listener() { return ctx.onCancel(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "button", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateFreightRateRules_Template_button_click_23_listener() { return ctx.onSubmit(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Save Draft");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "button", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, CreateFreightRateRules_div_27_Template, 5, 0, "div", 19);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.rForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.RuleMode != "CREATE");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](16, _c1, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "RuleType")("value", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "RuleType")("value", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "RuleType")("value", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("PricingRuleType", ctx.rForm.get("RuleType").value)("IsEditable", ctx.IsEditable);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.freightComponent != null && ctx.freightComponent != undefined && ctx.freightComponent.rcForm != undefined && ctx.freightComponent.rcForm.get("FuelGroups").value.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_12__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_12__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControlName"], _shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_8__["FreightComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_12__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupName"], angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["CheckboxControlValueAccessor"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2ZyZWlnaHRSYXRlcy9DcmVhdGUvY3JlYXRlLWZyZWlnaHQtcmF0ZS1ydWxlcy1jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateFreightRateRules, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-create-freight-rate-rules',
                templateUrl: './create-freight-rate-rules-component.html',
                styleUrls: ['./create-freight-rate-rules-component.css']
            }]
    }], function () { return [{ type: _Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_10__["FreightRateRulesService"] }, { type: _angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"] }, { type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"] }]; }, { freightComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
            args: [_shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_8__["FreightComponent"]]
        }], datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"]]
        }] }); })();


/***/ }),

/***/ "./src/app/freightRates/Master/master.component.ts":
/*!*********************************************************!*\
  !*** ./src/app/freightRates/Master/master.component.ts ***!
  \*********************************************************/
/*! exports provided: MasterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterComponent", function() { return MasterComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../Services/freight-rate-rules.service */ "./src/app/freightRates/Services/freight-rate-rules.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _Create_create_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../Create/create-freight-rate-rules-component */ "./src/app/freightRates/Create/create-freight-rate-rules-component.ts");
/* harmony import */ var _View_view_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../View/view-freight-rate-rules.component */ "./src/app/freightRates/View/view-freight-rate-rules.component.ts");







function MasterComponent_app_create_freight_rate_rules_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-create-freight-rate-rules");
} }
function MasterComponent_app_view_freight_rate_rules_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-view-freight-rate-rules");
} }
class MasterComponent {
    constructor(router, freightRateRulesService) {
        this.router = router;
        this.freightRateRulesService = freightRateRulesService;
        this.viewType = 0;
    }
    ngOnInit() {
        this.freightRateRulesService.onSelectedTabChanged.subscribe(s => {
            if (s == 2) {
                this.viewType = 2;
            }
            else {
                this.viewType = 1;
            }
        });
    }
    changeViewType(value) {
        this.viewType = value;
        this.freightRateRulesService.onSelectedFreightRateRuleId.next(null);
        this.freightRateRulesService.onSelectedTabChanged.next(value);
        //if(this.viewType==1)
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew']);
    }
}
MasterComponent.ɵfac = function MasterComponent_Factory(t) { return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_2__["FreightRateRulesService"])); };
MasterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: MasterComponent, selectors: [["app-master"]], decls: 16, vars: 8, consts: [[1, "row"], [1, "col-sm-12"], [1, "col-sm-4"], [1, "d-inline-block", "border", "bg-white", "p-1", "radius-capsule", "shadow-b", "mb-2"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [4, "ngIf"]], template: function MasterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_7_listener() { return ctx.changeViewType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Create Freight Rate");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_10_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "View Freight Rate");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, MasterComponent_app_create_freight_rate_rules_14_Template, 1, 0, "app-create-freight-rate-rules", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, MasterComponent_app_view_freight_rate_rules_15_Template, 1, 0, "app-view-freight-rate-rules", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 1)("checked", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 2);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _Create_create_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_4__["CreateFreightRateRules"], _View_view_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_5__["ViewFreightRateRules"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2ZyZWlnaHRSYXRlcy9NYXN0ZXIvbWFzdGVyLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-master',
                templateUrl: './master.component.html',
                styleUrls: ['./master.component.css']
            }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_2__["FreightRateRulesService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/freightRates/Models/createFreightRateRules.ts":
/*!***************************************************************!*\
  !*** ./src/app/freightRates/Models/createFreightRateRules.ts ***!
  \***************************************************************/
/*! exports provided: FreightRateFuelGroupViewModel, FreightRateRouteTableViewModel, FreightRatePtoPTable, FreightRateRangeTableViewModel, FreightRateViewModel, FreightPricingRulesViewModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateFuelGroupViewModel", function() { return FreightRateFuelGroupViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateRouteTableViewModel", function() { return FreightRateRouteTableViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRatePtoPTable", function() { return FreightRatePtoPTable; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateRangeTableViewModel", function() { return FreightRateRangeTableViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateViewModel", function() { return FreightRateViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightPricingRulesViewModel", function() { return FreightPricingRulesViewModel; });
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");

class FreightRateFuelGroupViewModel {
}
class FreightRateRouteTableViewModel {
}
class FreightRatePtoPTable {
}
class FreightRateRangeTableViewModel {
}
class FreightRateViewModel {
    constructor() {
        this.CustomerIds = [];
        this.CarrierIds = [];
        this.JobIds = [];
        this.FuelGroupIds = [];
        this.SourceRegionIds = [];
        this.TerminalsAndBulkPlants = [];
        this.FreightRateFuelGroups = [];
        this.FreightRateRouteTables = [];
        this.FreightRateRangeTables = [];
        this.FreightRatePtoPTables = [];
    }
}
class FreightPricingRulesViewModel {
    constructor() {
        this.TableTypes = [];
        this.Customers = [];
        this.Locations = [];
        this.Carriers = [];
        this.FuelGroups = [];
        this.SourceRegions = [];
        this.TerminalsAndBulkPlants = [];
        this.FuelGroupType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_0__["FuelGroupType"].Standard;
        this.StatusId = 2;
    }
}


/***/ }),

/***/ "./src/app/freightRates/Models/viewFreightRateRules.ts":
/*!*************************************************************!*\
  !*** ./src/app/freightRates/Models/viewFreightRateRules.ts ***!
  \*************************************************************/
/*! exports provided: FreightRateModel, FreightRateInputModel, FreightRateGridModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateModel", function() { return FreightRateModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateInputModel", function() { return FreightRateInputModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateGridModel", function() { return FreightRateGridModel; });
class FreightRateModel {
    constructor() {
        this.FreightRateRuleTypes = [];
        this.TableTypes = [];
        this.Customers = [];
        this.Carriers = [];
        this.SourceRegions = [];
        this.TerminalsAndBulkPlants = [];
    }
}
class FreightRateInputModel {
}
class FreightRateGridModel {
}


/***/ }),

/***/ "./src/app/freightRates/Services/freight-rate-rules.service.ts":
/*!*********************************************************************!*\
  !*** ./src/app/freightRates/Services/freight-rate-rules.service.ts ***!
  \*********************************************************************/
/*! exports provided: FreightRateRulesService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateRulesService", function() { return FreightRateRulesService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class FreightRateRulesService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.createFreightRateUrl = '/FreightRate/CreateFreightRate';
        this.updateFreightRateUrl = '/FreightRate/UpdateFreightRate';
        this.getFreightRateDetailUrl = '/FreightRate/GetFreightRateDetails?freightRateId=';
        this.archiveFreightRateUrl = '/FreightRate/ArchiveFreightRate';
        this.getFreightRateSummaryUrl = '/FreightRate/GetFreightRateSummary';
        this.getFreightRateTableForViewUrl = '/FreightRate/GetFreightRateTableForView?freightRateId=';
        this.getFreightRateRuleTypesUrl = "/FreightRate/GetFreightRateRuleTypes";
        this.getCustomerJobsUrl = "/FreightRate/GetCustomerJobs?customerId=";
        this.onSelectedTabChanged = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1);
        this.onSelectedFreightRateRuleId = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](null);
    }
    createFreightRate(fsm) {
        return this.httpClient.post(this.createFreightRateUrl, fsm, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('createFreightRate', fsm)));
    }
    updateFreightRate(fsm) {
        return this.httpClient.post(this.updateFreightRateUrl, fsm, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('updateFreightRate', fsm)));
    }
    getFreightRateRuleTypes() {
        return this.httpClient.get(this.getFreightRateRuleTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFreightRateRuleTypes', null)));
    }
    getCustomerJobs(customerId) {
        return this.httpClient.get(this.getCustomerJobsUrl + customerId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getCustomerJobs', null)));
    }
    getFreightRateDetails(freightRateId) {
        return this.httpClient.get(this.getFreightRateDetailUrl + freightRateId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeTable', null)));
    }
    getFreightRateGridDetails(filter) {
        return this.httpClient.post(this.getFreightRateSummaryUrl, filter)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFreightRateGridDetails', null)));
    }
    archiveFreightRate(freightRateId) {
        return this.httpClient.post(this.archiveFreightRateUrl, { freightRateId: freightRateId })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('archiveFreightRate', null)));
    }
    getFreightRateTableForView(freightRateId) {
        return this.httpClient.get(this.getFreightRateTableForViewUrl + freightRateId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFreightRateTableForView', null)));
    }
}
FreightRateRulesService.ɵfac = function FreightRateRulesService_Factory(t) { return new (t || FreightRateRulesService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
FreightRateRulesService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: FreightRateRulesService, factory: FreightRateRulesService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FreightRateRulesService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/freightRates/View/view-freight-rate-rules.component.ts":
/*!************************************************************************!*\
  !*** ./src/app/freightRates/View/view-freight-rate-rules.component.ts ***!
  \************************************************************************/
/*! exports provided: ViewFreightRateRules */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewFreightRateRules", function() { return ViewFreightRateRules; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/my.localstorage */ "./src/app/my.localstorage.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _Models_viewFreightRateRules__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../Models/viewFreightRateRules */ "./src/app/freightRates/Models/viewFreightRateRules.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var src_app_freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/freightRates/Services/freight-rate-rules.service */ "./src/app/freightRates/Services/freight-rate-rules.service.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! src/app/fuelsurcharge/services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! angular-confirmation-popover */ "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");





















const _c0 = function (a0) { return { "d-block": a0 }; };
function ViewFreightRateRules_tr_44_td_19_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](6, _c0, !item_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](2, 2, item_r5.Terminal, 0, 40), "...");
} }
function ViewFreightRateRules_tr_44_td_19_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, ViewFreightRateRules_tr_44_td_19_div_3_Template, 3, 8, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_tr_44_td_19_Template_a_click_4_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16); const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; return item_r5.IsShowMore = !item_r5.IsShowMore; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "View More/Less");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](3, _c0, item_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.Terminal);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", item_r5.Terminal.length > 40);
} }
function ViewFreightRateRules_tr_44_td_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.Terminal);
} }
function ViewFreightRateRules_tr_44_a_26_Template(rf, ctx) { if (rf & 1) {
    const _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("cancel", function ViewFreightRateRules_tr_44_a_26_Template_a_cancel_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2); return ctx_r19.cancelClicked = true; })("confirm", function ViewFreightRateRules_tr_44_a_26_Template_a_confirm_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20); const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r21.archiveFreightRate(item_r5.Id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("popoverTitle", ctx_r9.popoverTitle)("popoverMessage", ctx_r9.popoverMessage);
} }
function ViewFreightRateRules_tr_44_a_27_Template(rf, ctx) { if (rf & 1) {
    const _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_tr_44_a_27_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r25); const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r23.viewFreightRateRule(item_r5.Id, item_r5.FreightRateRuleTypeValue, "EDIT"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewFreightRateRules_tr_44_a_30_Template(rf, ctx) { if (rf & 1) {
    const _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_tr_44_a_30_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r28); const item_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r26.viewFreightRateRule(item_r5.Id, item_r5.FreightRateRuleTypeValue, "COPY"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewFreightRateRules_tr_44_Template(rf, ctx) { if (rf & 1) {
    const _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, ViewFreightRateRules_tr_44_td_19_Template, 6, 5, "td", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, ViewFreightRateRules_tr_44_td_20_Template, 2, 1, "td", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](26, ViewFreightRateRules_tr_44_a_26_Template, 2, 2, "a", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, ViewFreightRateRules_tr_44_a_27_Template, 2, 0, "a", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "a", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_tr_44_Template_a_click_28_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30); const item_r5 = ctx.$implicit; const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r29.viewFreightRateRule(item_r5.Id, item_r5.FreightRateRuleTypeValue, "VIEW"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](29, "i", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](30, ViewFreightRateRules_tr_44_a_30_Template, 2, 0, "a", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.DateRange);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.FreightRateRuleType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.TableType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.TableName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.StatusName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.Customer);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.Carrier);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.SourceRegion);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", item_r5.Terminal.length > 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", item_r5.Terminal.length <= 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.BulkPlant);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r5.FuelGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !item_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !item_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !item_r5.IsArchived);
} }
function ViewFreightRateRules_div_45_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewFreightRateRules_ng_template_46_div_7_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Freight Pricing Rule is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewFreightRateRules_ng_template_46_div_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, ViewFreightRateRules_ng_template_46_div_7_div_1_Template, 2, 0, "div", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r31.viewFreightRateForm.get("FreightRateRuleTypes").errors.required);
} }
function ViewFreightRateRules_ng_template_46_div_13_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Table Type is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewFreightRateRules_ng_template_46_div_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, ViewFreightRateRules_ng_template_46_div_13_div_1_Template, 2, 0, "div", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r32.viewFreightRateForm.get("TableTypes").errors.required);
} }
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
function ViewFreightRateRules_ng_template_46_Template(rf, ctx) { if (rf & 1) {
    const _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Freight Pricing Rule");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "ng-multiselect-dropdown", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, ViewFreightRateRules_ng_template_46_div_7_Template, 2, 1, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "label", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11, "Table Type");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "ng-multiselect-dropdown", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onSelect_12_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r35.onTableTypeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](13, ViewFreightRateRules_ng_template_46_div_13_Template, 2, 1, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Customer(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r37.onCustomersSelect($event); })("onDeSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r38.onCustomersDeSelect($event); })("onDeSelectAll", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelectAll_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r39.onCustomersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "label", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Carrier(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "ng-multiselect-dropdown", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onSelect_25_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r40.onCarriersSelect($event); })("onDeSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelect_25_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r41.onCarriersDeSelect($event); })("onDeSelectAll", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelectAll_25_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r42.onCarriersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "label", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](29, "Source Region(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "ng-multiselect-dropdown", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onSelect_30_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r43.onSourceRegionsSelect($event); })("onDeSelect", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelect_30_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r44.onSourceRegionsDeSelect($event); })("onDeSelectAll", function ViewFreightRateRules_ng_template_46_Template_ng_multiselect_dropdown_onDeSelectAll_30_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r45.onSourceRegionsDeSelectAll($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "label", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Terminal(s)/BulkPlant(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](35, "angular2-multiselect", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39, "From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "input", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewFreightRateRules_ng_template_46_Template_input_onDateChange_40_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r46.setfromDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "To");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "input", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewFreightRateRules_ng_template_46_Template_input_onDateChange_45_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r47.settoDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "div", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](49, "input", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "label", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](51, " Show Archived ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "button", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_ng_template_46_Template_button_click_53_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r48.clearFilter(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](54, "Clear Filter");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "button", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_ng_template_46_Template_button_click_55_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36); const ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4); ctx_r49.filterGrid(); return _r0.close(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](56, "Apply");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Type (Required)")("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.FreightRateRuleTypeList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.viewFreightRateForm.get("FreightRateRuleTypes").invalid && ctx_r4.viewFreightRateForm.get("FreightRateRuleTypes").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Type (Required)")("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.TableTypeList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.viewFreightRateForm.get("TableTypes").invalid && ctx_r4.viewFreightRateForm.get("TableTypes").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](26, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Customers")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CustomerList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](28, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CarrierList)("placeholder", "Select Carriers");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.SourceRegionList)("placeholder", "Select Source Regions");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx_r4.TerminalsAndBulkPlantList)("settings", ctx_r4.MultiSelectSettingsByGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("minDate", ctx_r4.minDate)("maxDate", ctx_r4.maxDate)("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](30, _c1, ctx_r4.IsLoading));
} }
class ViewFreightRateRules {
    constructor(fb, freightRateRulesService, regionService, fuelsurchargeService, cdr) {
        this.fb = fb;
        this.freightRateRulesService = freightRateRulesService;
        this.regionService = regionService;
        this.fuelsurchargeService = fuelsurchargeService;
        this.cdr = cdr;
        this.IsLoading = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.SinlgeselectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.TerminalsAndBulkPlantList = [];
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.FreightRateList = [];
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
        this.CounrtyId = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_2__["MyLocalStorage"].getData("countryIdForDashboard");
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
        this.getFreightRateRuleTypes();
        this.viewFreightRateForm = this.createForm();
        this.initializeFreightRateDatatableGrid();
    }
    ngOnDestroy() {
        this.datatableInventoryRerender();
    }
    ngAfterViewInit() {
        this.getFreightRateGridDetails();
    }
    viewFreightRateRule(ruleId, freightRateRuleType, mode) {
        let operation = { RuleId: ruleId, FreightRateRuleType: freightRateRuleType, Mode: mode };
        this.freightRateRulesService.onSelectedFreightRateRuleId.next(JSON.stringify(operation));
        this.freightRateRulesService.onSelectedTabChanged.next(1);
    }
    createForm() {
        if (this.Frmodel == undefined || this.Frmodel == null) {
            this.Frmodel = new _Models_viewFreightRateRules__WEBPACK_IMPORTED_MODULE_6__["FreightRateModel"]();
        }
        return this.fb.group({
            FreightRateRuleTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.FreightRateRuleTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.TableTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.Customers),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.Carriers),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.SourceRegions),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControl"](this.Frmodel.TerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
        });
    }
    archiveFreightRate(freightRateId) {
        this.IsLoading = true;
        this.freightRateRulesService.archiveFreightRate(freightRateId)
            .subscribe((response) => {
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess('Selected Freight Rate archived successfully', undefined, undefined);
                this.filterGrid();
            }
            this.IsLoading = false;
        });
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewFreightRateForm.get('Carriers').patchValue([]);
        this.viewFreightRateForm.get('Customers').patchValue([]);
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
                this.getSupplierCustomers();
                this.getCarriers();
                break;
        }
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    onCarriersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCarriersDeSelect(item) {
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCustomersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCustomersDeSelect(item) {
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCarriersOrCustomersChange() {
        var selectedTableType = this.viewFreightRateForm.get('TableTypes').value;
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    getTableTypes() {
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TableTypeList = yield data;
        }));
    }
    getFreightRateRuleTypes() {
        this.freightRateRulesService.getFreightRateRuleTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.FreightRateRuleTypeList = yield data;
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
        let customerIds = [];
        let carrierIds = [];
        let selectedCustomers = this.viewFreightRateForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }
        let selectedCarriers = this.viewFreightRateForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["SourceRegionInputModel"]();
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
        var selectedSourceRegions = this.viewFreightRateForm.get('SourceRegions').value;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.IsLoading = false;
            }));
        }
    }
    onSourceRegionsDeSelect(item) {
        this.IsSourceRegionSelected = this.Frmodel.SourceRegions.length > 0;
    }
    onSourceRegionsDeSelectAll(item) {
        this.IsSourceRegionSelected = false;
    }
    onSourceRegionsSelect(item) {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Frmodel.SourceRegions.length > 0;
    }
    filterGrid() {
        $("#freight-rate-grid-datatable").DataTable().clear().destroy();
        this.getFreightRateGridDetails();
    }
    clearFilter() {
        this.clearForm();
        this.getFreightRateGridDetails();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }
    clearForm() {
        this.viewFreightRateForm.reset();
        $("#freight-rate-grid-datatable").DataTable().clear().destroy();
    }
    getFreightRateGridDetails() {
        var input = new _Models_viewFreightRateRules__WEBPACK_IMPORTED_MODULE_6__["FreightRateInputModel"]();
        var selectedFreightRateRuleTypes = this.viewFreightRateForm.get('FreightRateRuleTypes').value;
        var selectedTableTypes = this.viewFreightRateForm.get('TableTypes').value;
        var selectedCustomers = this.viewFreightRateForm.get('Customers').value;
        var selectedCarriers = this.viewFreightRateForm.get('Carriers').value;
        var selectedSourceRegions = this.viewFreightRateForm.get('SourceRegions').value;
        var selectedTerminalAndBulkPlants = this.viewFreightRateForm.get('TerminalsAndBulkPlants').value;
        input.StartDate = this.viewFreightRateForm.controls.fromDate.value;
        input.EndDate = this.viewFreightRateForm.controls.toDate.value;
        input.IsArchived = this.viewFreightRateForm.controls.isArchived.value;
        if (selectedFreightRateRuleTypes != null && selectedFreightRateRuleTypes != undefined && selectedFreightRateRuleTypes.length > 0) {
            var freightRateRuleTypeIds = selectedFreightRateRuleTypes.map(s => s.Id);
            input.FreightRateRuleTypeIds = freightRateRuleTypeIds.join(',');
        }
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
        this.freightRateRulesService.getFreightRateGridDetails(input).subscribe(data => {
            this.IsLoading = false;
            if (data && data.length > 0) {
                this.FreightRateList = data;
            }
            this.dtTrigger.next();
        });
    }
    datatableInventoryRerender() {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                dtInstance.destroy();
            });
        }
    }
    setfromDate($event) {
        this.viewFreightRateForm.controls.fromDate.setValue($event);
    }
    settoDate($event) {
        this.viewFreightRateForm.controls.toDate.setValue($event);
    }
    initializeFreightRateDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Freight Rate', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Freight Rate', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
}
ViewFreightRateRules.ɵfac = function ViewFreightRateRules_Factory(t) { return new (t || ViewFreightRateRules)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_9__["FreightRateRulesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"])); };
ViewFreightRateRules.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: ViewFreightRateRules, selectors: [["app-view-freight-rate-rules"]], viewQuery: function ViewFreightRateRules_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
    } }, decls: 48, vars: 7, consts: [[3, "formGroup"], [1, "row"], [1, "col-sm-12", "text-left"], ["placement", "bottom", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs16", "mr10", "filter-link", "pa", "mt-2", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-freight-rate-grid", 1, "table-responsive"], ["id", "freight-rate-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Id", 1, "hide-element"], ["data-key", "DateRange"], ["data-key", "FreightRateRuleType"], ["data-key", "TableType"], ["data-key", "TableName"], ["data-key", "StatusName"], ["data-key", "Customer"], ["data-key", "Carrier"], ["data-key", "SourceRegion"], ["data-key", "Terminal"], ["data-key", "BulkPlant"], ["class", "accordion", "id", "accordionExample", 4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["popContent", ""], ["id", "accordionExample", 1, "accordion"], [1, "hide-element"], [4, "ngIf"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-street-view"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Copy", 3, "click", 4, "ngIf"], [1, "d-none", 3, "ngClass"], ["class", "d-none", 3, "ngClass", 4, "ngIf"], [3, "click"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-trash-alt", "color-maroon"], ["placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], ["placement", "bottom", "ngbTooltip", "Copy", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-copy"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "popover-details"], [1, "col-sm-6"], [1, "form-group"], ["for", "FreightRateRuleTypes"], ["formControlName", "FreightRateRuleTypes", "id", "FreightRateRuleTypes", 3, "placeholder", "settings", "data"], ["class", "color-maroon", 4, "ngIf"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 3, "placeholder", "settings", "data", "onSelect"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], [1, "col-sm-12"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "placeholder", "Select Date", "formControlName", "fromDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Select Date", "formControlName", "toDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-6", "form-group"], [1, "form-check"], ["formControlName", "isArchived", "type", "checkbox", "value", "", "id", "ckb-isArchived", 1, "form-check-input"], ["for", "ckb-isArchived", 1, "form-check-label"], [1, "col-sm-6", "text-right", "form-buttons", "mt20"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], ["id", "filter-freight-rate-grid", "type", "button", 1, "btn", "btn-lg", "btn-primary", "mt3", "valid", 3, "ngClass", "click"], [1, "color-maroon"]], template: function ViewFreightRateRules_Template(rf, ctx) { if (rf & 1) {
        const _r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 3, 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFreightRateRules_Template_a_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r50); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4); return _r0.open(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Filters");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "table", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Id");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Date Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Freight Pricing Rule");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Table Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Table Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Terminal(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Bulk Plant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Fuel Group(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](44, ViewFreightRateRules_tr_44_Template, 31, 16, "tr", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, ViewFreightRateRules_div_45_Template, 5, 0, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, ViewFreightRateRules_ng_template_46_Template, 57, 32, "ng-template", null, 25, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    } if (rf & 2) {
        const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](47);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.viewFreightRateForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r3)("autoClose", "outside");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.FreightRateList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormGroupDirective"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__["NgbPopover"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_14__["ɵc"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControlName"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["CheckboxControlValueAccessor"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["SlicePipe"]], styles: [".master-filter.popover {\r\n    min-width: 425px;\r\n    max-width: 450px;\r\n    background: #F9F9F9;\r\n    border: 1px solid #E9E7E7;\r\n    box-sizing: border-box;\r\n    box-shadow: 10px 10px 8px -2px rgb(0, 0, 0, 0.13);\r\n    border-radius: 10px;\r\n}\r\n\r\n      .master-filter.popover .popover-body {\r\n        padding: 0;\r\n        border-radius: 5px;\r\n        background: #ffffff;\r\n    }\r\n\r\n      .master-filter.popover .popover-details {\r\n        padding: 15px;\r\n    }\r\n\r\n      .master-filter.popover .border-bottom-2 {\r\n        border-bottom: 2px solid #e7eaec !important;\r\n    }\r\n\r\n    .filter-link[_ngcontent-%COMP%] {\r\n    top: -45px;\r\n    left: 380px\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZnJlaWdodFJhdGVzL1ZpZXcvdmlldy1mcmVpZ2h0LXJhdGUtcnVsZXMuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGdCQUFnQjtJQUNoQixnQkFBZ0I7SUFDaEIsbUJBQW1CO0lBQ25CLHlCQUF5QjtJQUN6QixzQkFBc0I7SUFDdEIsaURBQWlEO0lBQ2pELG1CQUFtQjtBQUN2Qjs7SUFFSTtRQUNJLFVBQVU7UUFDVixrQkFBa0I7UUFDbEIsbUJBQW1CO0lBQ3ZCOztJQUVBO1FBQ0ksYUFBYTtJQUNqQjs7SUFFQTtRQUNJLDJDQUEyQztJQUMvQzs7SUFFSjtJQUNJLFVBQVU7SUFDVjtBQUNKIiwiZmlsZSI6InNyYy9hcHAvZnJlaWdodFJhdGVzL1ZpZXcvdmlldy1mcmVpZ2h0LXJhdGUtcnVsZXMuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIjo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIHtcclxuICAgIG1pbi13aWR0aDogNDI1cHg7XHJcbiAgICBtYXgtd2lkdGg6IDQ1MHB4O1xyXG4gICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbiAgICA6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcclxuICAgICAgICBwYWRkaW5nOiAwO1xyXG4gICAgICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgfVxyXG5cclxuICAgIDo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMge1xyXG4gICAgICAgIHBhZGRpbmc6IDE1cHg7XHJcbiAgICB9XHJcblxyXG4gICAgOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLmJvcmRlci1ib3R0b20tMiB7XHJcbiAgICAgICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNlN2VhZWMgIWltcG9ydGFudDtcclxuICAgIH1cclxuXHJcbi5maWx0ZXItbGluayB7XHJcbiAgICB0b3A6IC00NXB4O1xyXG4gICAgbGVmdDogMzgwcHhcclxufVxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](ViewFreightRateRules, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-view-freight-rate-rules',
                templateUrl: './view-freight-rate-rules.component.html',
                styleUrls: ['./view-freight-rate-rules.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormBuilder"] }, { type: src_app_freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_9__["FreightRateRulesService"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"] }, { type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"] }, { type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"] }]; }, { datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"]]
        }] }); })();


/***/ }),

/***/ "./src/app/freightRates/freight.rate.module.ts":
/*!*****************************************************!*\
  !*** ./src/app/freightRates/freight.rate.module.ts ***!
  \*****************************************************/
/*! exports provided: FreightRateRulesModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightRateRulesModule", function() { return FreightRateRulesModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _Master_master_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./Master/master.component */ "./src/app/freightRates/Master/master.component.ts");
/* harmony import */ var _Create_create_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./Create/create-freight-rate-rules-component */ "./src/app/freightRates/Create/create-freight-rate-rules-component.ts");
/* harmony import */ var _View_view_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./View/view-freight-rate-rules.component */ "./src/app/freightRates/View/view-freight-rate-rules.component.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../shared-components/Freight/freight.component */ "./src/app/shared-components/Freight/freight.component.ts");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");














const route = [
    { path: '', component: _Master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] },
    { path: 'Create', component: _Master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] },
    { path: 'Create/:freightrateruleId', component: _Master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] }
];
class FreightRateRulesModule {
}
FreightRateRulesModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: FreightRateRulesModule });
FreightRateRulesModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function FreightRateRulesModule_Factory(t) { return new (t || FreightRateRulesModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
            angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["AngularMultiSelectModule"]
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](FreightRateRulesModule, { declarations: [_Master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"],
        _Create_create_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_6__["CreateFreightRateRules"],
        _View_view_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_7__["ViewFreightRateRules"],
        _shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_10__["FreightComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
        _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["AngularMultiSelectModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FreightRateRulesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [
                    _Master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"],
                    _Create_create_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_6__["CreateFreightRateRules"],
                    _View_view_freight_rate_rules_component__WEBPACK_IMPORTED_MODULE_7__["ViewFreightRateRules"],
                    _shared_components_Freight_freight_component__WEBPACK_IMPORTED_MODULE_10__["FreightComponent"]
                ],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
                    _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
                    angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["AngularMultiSelectModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/shared-components/Freight/freight.component.ts":
/*!****************************************************************!*\
  !*** ./src/app/shared-components/Freight/freight.component.ts ***!
  \****************************************************************/
/*! exports provided: FreightComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FreightComponent", function() { return FreightComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_freightRates_Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/freightRates/Models/createFreightRateRules */ "./src/app/freightRates/Models/createFreightRateRules.ts");
/* harmony import */ var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/my.localstorage */ "./src/app/my.localstorage.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! src/app/fuelsurcharge/services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../../freightRates/Services/freight-rate-rules.service */ "./src/app/freightRates/Services/freight-rate-rules.service.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! src/app/self-service-alias/service/externalmappings.service */ "./src/app/self-service-alias/service/externalmappings.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");




















const _c0 = ["btnCloseBulkUploadPopup"];
function FreightComponent_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_10_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_10_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.rcForm.get("TableName").errors.required);
} }
function FreightComponent_div_15_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_15_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.rcForm.get("TableTypes").errors.required);
} }
function FreightComponent_div_24_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_24_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.get("Customers").errors.required);
} }
function FreightComponent_div_29_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_29_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_29_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.rcForm.get("Locations").errors.required);
} }
function FreightComponent_div_35_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_35_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_35_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.rcForm.get("Carriers").errors.required);
} }
function FreightComponent_div_40_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_40_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_40_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r6.rcForm.get("SourceRegions").errors.required);
} }
function FreightComponent_div_55_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_55_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_55_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.rcForm.get("StartDate").errors.required);
} }
function FreightComponent_div_77_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_77_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_77_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.rcForm.get("FuelGroups").errors.required);
} }
function FreightComponent_div_78_div_18_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_18_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_78_div_18_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, FreightComponent_div_78_div_18_div_2_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r21.rcForm.get("RangeStartValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r21.rcForm.get("RangeStartValue").errors.pattern);
} }
function FreightComponent_div_78_div_21_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_21_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_78_div_21_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, FreightComponent_div_78_div_21_div_2_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r22.rcForm.get("RangeEndValue").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r22.rcForm.get("RangeEndValue").errors.pattern);
} }
function FreightComponent_div_78_div_24_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_24_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid, must be greater than zero. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, FreightComponent_div_78_div_24_div_1_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, FreightComponent_div_78_div_24_div_2_Template, 2, 0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r23.rcForm.get("RangeInterval").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r23.rcForm.get("RangeInterval").errors.pattern);
} }
function FreightComponent_div_78_div_25_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid range. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_78_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "table", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "thead");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "th", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Range Value");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Start Value");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11, "End Value");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "th");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Interval");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](17, "input", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, FreightComponent_div_78_div_18_Template, 3, 2, "div", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](20, "input", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, FreightComponent_div_78_div_21_Template, 3, 2, "div", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](23, "input", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, FreightComponent_div_78_div_24_Template, 3, 2, "div", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](25, FreightComponent_div_78_div_25_Template, 2, 0, "div", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("RangeStartValue").invalid && ctx_r9.rcForm.get("RangeStartValue").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("RangeEndValue").invalid && ctx_r9.rcForm.get("RangeEndValue").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("RangeInterval").invalid && ctx_r9.rcForm.get("RangeInterval").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("RangeStartValue").valid && ctx_r9.rcForm.get("RangeEndValue").valid && ctx_r9.rcForm.get("RangeStartValue").value - 0 > ctx_r9.rcForm.get("RangeEndValue").value - 0);
} }
function FreightComponent_div_82_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Please click to generate table. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_83_span_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "P2P");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_83_span_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Range");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_83_Template(rf, ctx) { if (rf & 1) {
    const _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "button", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FreightComponent_div_83_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r33.importClick(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " Import ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, FreightComponent_div_83_span_3_Template, 2, 0, "span", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, FreightComponent_div_83_span_4_Template, 2, 0, "span", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, " Table ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", !ctx_r11.IsRangePopupOpen);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.PricingRuleType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.PricingRuleType == 2);
} }
function FreightComponent_div_84_div_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function FreightComponent_div_84_Template(rf, ctx) { if (rf & 1) {
    const _r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h4", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Upload Range Table");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "button", 57, 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FreightComponent_div_84_Template_button_click_6_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r38); const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r37.closePopup(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "i", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "span", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "a", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FreightComponent_div_84_Template_a_click_12_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r38); const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r39.downloadCsvTemplate(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Download Dynamic Template");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](14, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Note: Generate Dynamic template based on selected fule group order.");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "input", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function FreightComponent_div_84_Template_input_change_18_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r38); const ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r40.selectFiles($event.target.files); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, FreightComponent_div_84_div_19_Template, 2, 0, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](20, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "button", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FreightComponent_div_84_Template_button_click_22_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r38); const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r41.uploadRangeTableFile(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, " Upload ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](19);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](20, 1, ctx_r12.isLoadingSubject));
} }
const _c1 = function (a0) { return { "pntr-none": a0 }; };
const _c2 = function (a0, a1) { return { "hide-element": a0, "pntr-none subSectionOpacity": a1 }; };
const _c3 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
class FreightComponent {
    constructor(fb, fuelsurchargeService, freightRateRulesService, regionService, externalMappingsService) {
        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.freightRateRulesService = freightRateRulesService;
        this.regionService = regionService;
        this.externalMappingsService = externalMappingsService;
        this.onGenerateTable = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.onGenerateFuelGroup = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.onImportClick = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.DtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subject"]();
        this.SingleSelectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.IsLoaded = true;
        this.TerminalsAndBulkPlantList = [];
        this.SelectedTerminalsAndBulkPlants = [];
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.SelectedFiles = [];
        this.IsShowBulkUploadPopup = false;
        this.MaxFileUploadSize = 1048576; // 1MB
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.disableInputControls = false;
        this.ShowMessage = false;
        this.decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
        this.intGreaterThanZeroRegx = /^[1-9][0-9]*$/;
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.MinToDate = new Date();
        this.MinFromDate = new Date();
    }
    ngOnInit() {
        this.isLoadingSubject = new rxjs__WEBPACK_IMPORTED_MODULE_4__["BehaviorSubject"](false);
        this.CountryId = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_3__["MyLocalStorage"].getData("countryIdForDashboard");
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
            text: "Select Terminals or Bulk Plants",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };
        var dt = moment__WEBPACK_IMPORTED_MODULE_7__(new Date()).toDate();
        this.rcForm = this.createForm();
        this.getTableTypes();
        this.rcForm.get('FuelGroupType').setValue(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FuelGroupType"].Standard);
        this.getFuelGroups(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FuelGroupType"].Standard, "");
        if (this.PricingRuleType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            this.rcForm.controls['TableTypes'].patchValue([{ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master, Name: "Master" }]); // default will master
            this.IsMasterSelected = true;
        }
        else {
            this.rcForm.controls['TableTypes'].patchValue([{ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer, Name: "Customer Specific" }]);
            this.IsCustomerSelected = true;
            //this.IsMasterSelected = false;
        }
        this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master.toString());
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('Customers').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.CustomerChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('Locations').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.LocationChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('Carriers').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.CarrierChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('SourceRegions').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.SourceRegionChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('TerminalsAndBulkPlants').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.TerminalsAndBulkPlantsChange(prev, next);
        });
        Object(rxjs__WEBPACK_IMPORTED_MODULE_4__["merge"])(this.rcForm.get('FuelGroups').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.FuelGroupsChange(prev, next);
        });
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 20);
        this.MinStartDate.setFullYear(this.MinStartDate.getFullYear() - 20);
        this.rcForm.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(dt).format('MM/DD/YYYY'));
    }
    ngOnChanges(change) {
        if (change.PricingRuleType && change.PricingRuleType.currentValue) {
            this.PricingRuleType = change.PricingRuleType.currentValue;
            if (change.PricingRuleType.previousValue && change.PricingRuleType.currentValue != change.PricingRuleType.previousValue) {
            }
        }
        if (change.IsEditable && change.IsEditable.currentValue) {
            this.IsEditable = change.IsEditable.currentValue;
            if (change.IsEditable.previousValue && change.IsEditable.currentValue != change.IsEditable.previousValue) {
                // todo : will progress
            }
        }
    }
    createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new src_app_freightRates_Models_createFreightRateRules__WEBPACK_IMPORTED_MODULE_2__["FreightPricingRulesViewModel"]();
        }
        return this.fb.group({
            FuelGroupType: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelGroupType),
            TableTypeId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableTypeId),
            TableName: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableName, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required),
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.TableTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Customers),
            Locations: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Locations),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.Carriers),
            FuelGroups: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.FuelGroups, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.SourceRegions, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.SelectedTerminalsAndBulkPlants),
            StatusId: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.StatusId),
            StartDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.StartDate, [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]),
            EndDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.EndDate),
            RangeStartValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
            RangeEndValue: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)]),
            RangeInterval: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.intGreaterThanZeroRegx)])
        });
    }
    setStartDate(event) {
        this.rcForm.get('StartDate').setValue(event);
        if (this.rcForm.get('StartDate').value != null && this.rcForm.get('StartDate').value != undefined && this.rcForm.get('StartDate').value != "") {
            this.MinToDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(this.rcForm.get('StartDate').value).format());
        }
    }
    setEndDate(event) {
        this.rcForm.get('EndDate').setValue(event);
    }
    FuelGroupTypeChange(fgt) {
        this.isLoadingSubject.next(true);
        //this.ViewOrEdit = "VIEW";
        this.rcForm.get('FuelGroups').patchValue([]);
        let selectedCustomers = [];
        let selectedCarriers = [];
        selectedCustomers = this.rcForm.get('Customers').value;
        selectedCarriers = this.rcForm.get('Carriers').value;
        if (selectedCustomers.length > 1 || selectedCarriers.length > 1 || (selectedCustomers.length > 0 && selectedCarriers.length > 0)) {
            this.getFuelGroups(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FuelGroupType"].Standard, "");
        }
        else {
            this.getFuelGroups(fgt, selectedCustomers.length > 0 ? selectedCustomers.map(s => s.Id).join(',') : selectedCarriers.map(s => s.Id).join(','));
        }
        this.isLoadingSubject.next(false);
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                //this.IsCarrierSelected = true;
                //this.getCarriers();
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                this.getSourceRegions(item.Id);
                this.rcForm.get('FuelGroupType').setValue(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FuelGroupType"].Standard);
                break;
            case 2: // customer
                this.getSupplierCustomers();
                this.getCarriers();
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
        this.getFuelGroups(+this.rcForm.get('FuelGroupType').value, "");
    }
    LocationChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.ShowMessage = true;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);
        //this.rcForm.controls.FuelGroups.patchValue([]);
    }
    CustomerChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.rcForm.controls.Locations.patchValue([]);
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);
        //this.rcForm.controls.FuelGroups.patchValue([]);
        //this.FuelGroupsList = [];
        let selectedCustomers = this.rcForm.get('Customers').value;
        let selectedCarriers = this.rcForm.get('Carriers').value;
        if (this.IsCustomerSelected) {
            this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer.toString());
        }
        if (this.IsCarrierSelected) {
            this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Carrier.toString());
        }
        if (selectedCustomers.length > 1 || selectedCarriers.length > 1 || (selectedCustomers.length > 0 && selectedCarriers.length > 0)) {
            this.getFuelGroups(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FuelGroupType"].Standard, "");
        }
        else {
            this.getFuelGroups(+this.rcForm.get('FuelGroupType').value, selectedCustomers.length > 0 ? selectedCustomers.map(s => s.Id).join(',') : selectedCarriers.map(s => s.Id).join(','));
        }
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            if (selectedCustomers.length > 0) {
                this.freightRateRulesService.getCustomerJobs(selectedCustomers[0].Id).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                    this.LocationList = yield (data);
                    this.DtTrigger.next();
                    this.isLoadingSubject.next(false);
                }));
            }
            this.AddRemoveValidations([this.rcForm.get('Locations')], null);
        }
        else {
            this.AddRemoveValidations(null, [this.rcForm.get('Locations')]);
        }
    }
    CarrierChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.ShowMessage = true;
        this.CustomerChange(prev, next);
    }
    FuelGroupsChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.ShowMessage = true;
        this.onGenerateFuelGroup.emit(true);
    }
    TerminalsAndBulkPlantsChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        //this.rcForm.controls.FuelGroups.patchValue([]);
    }
    SourceRegionChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        //this.FuelGroupsList = [];
        this.IsSourceRegionSelected = false;
        var ids = [];
        this.ShowMessage = true;
        let selectedSourceRegions = this.rcForm.get('SourceRegions').value;
        if (selectedSourceRegions.length > 0) {
            this.isLoadingSubject.next(true);
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
                this.DtTrigger.next();
                this.isLoadingSubject.next(false);
            }));
        }
    }
    ValidateOnSubmit(freightTableStatus) {
        this.isLoadingSubject.next(true);
        this.ShowMessage = false;
        // publish or draft
        this.modeChangePublishORdraftValidators(freightTableStatus);
        this.markFormGroupTouched(this.rcForm);
        this.isLoadingSubject.next(false);
        if (!this.rcForm.valid) {
            //let selectedSourceRegion = this.rcForm.get('FuelGroups').value as DropdownItem[];
            this.ShowMessage = true;
        }
        return this.ShowMessage;
    }
    ValidateOnGenearteRow() {
        this.isLoadingSubject.next(true);
        this.ShowMessage = false;
        // publish or draft
        this.clearAllValidations(this.rcForm); // clear all validation
        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            this.AddRemoveValidations([this.rcForm.controls.RangeStartValue], null);
            this.AddRemoveValidations([this.rcForm.controls.RangeEndValue], null);
            this.AddRemoveValidations([this.rcForm.controls.RangeInterval], null);
        }
        this.AddRemoveValidations([this.rcForm.controls.FuelGroups], null);
        this.AddRemoveValidations([this.rcForm.controls.TerminalsAndBulkPlants], null);
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            this.AddRemoveValidations([this.rcForm.controls.Locations], null);
            this.AddRemoveValidations([this.rcForm.controls.Customers], null);
        }
        this.rcForm.get('StartDate').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
        this.rcForm.get('StartDate').updateValueAndValidity();
        this.rcForm.get('StartDate').markAsTouched();
        this.markFormGroupTouched(this.rcForm);
        this.isLoadingSubject.next(false);
        if (!this.rcForm.valid) {
            this.ShowMessage = true;
        }
        return this.ShowMessage;
    }
    generateTable() {
        this.ValidateOnGenearteRow();
        this.rcForm.get('SourceRegions').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
        this.rcForm.get('SourceRegions').updateValueAndValidity();
        this.onGenerateTable.emit(true);
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
    modeChangePublishORdraftValidators(statusId) {
        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode
        if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
            this.clearAllValidations(this.rcForm); // clear all validation
            this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for draft 
        }
        else if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
            this.AddRemoveValidations([this.rcForm.controls.FuelGroups], null);
            this.AddRemoveValidations([this.rcForm.controls.TerminalsAndBulkPlants], null);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeStartValue]);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeEndValue]);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeInterval]);
            if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                this.AddRemoveValidations([this.rcForm.controls.Locations], null);
                this.AddRemoveValidations([this.rcForm.controls.Customers], null);
            }
            this.rcForm.get('StartDate').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
            this.rcForm.get('StartDate').updateValueAndValidity();
            this.rcForm.get('StartDate').markAsTouched();
        }
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
    markFormGroupTouched(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }
    ngOnDestroy() {
        this.DtTrigger.unsubscribe();
    }
    getCarriers() {
        this.isLoadingSubject.next(true);
        this.regionService.getCarriers()
            .subscribe((carriers) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CarrierList = yield carriers;
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        }));
    }
    getTableTypes() {
        this.isLoadingSubject.next(true);
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
                let tableType = yield (data);
                this.TableTypeList = tableType.filter(s => s.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P);
            }
            else {
                this.TableTypeList = yield (data);
            }
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        }));
    }
    getSupplierCustomers() {
        this.isLoadingSubject.next(true);
        this.fuelsurchargeService.getSupplierCustomers().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CustomerList = yield (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        }));
    }
    getFuelGroups(fuelGroupType, companyIds) {
        this.isLoadingSubject.next(true);
        this.rcForm.get('FuelGroupType').setValue(fuelGroupType);
        this.externalMappingsService.getFuelGroups(fuelGroupType, companyIds).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.FuelGroupsList = yield (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        }));
    }
    //companyIds : Based on tableType we will be call API , if tableType master or customer or carrier, full source region,customers,carriers loads will populated.
    getSourceRegions(tableType) {
        this.isLoadingSubject.next(true);
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
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.SourceRegionList = yield (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        }));
    }
    importClick() {
        this.generateTable();
        if (!this.ShowMessage)
            this.IsShowBulkUploadPopup = true;
    }
    closePopup() {
        this.IsShowBulkUploadPopup = false;
    }
    get IsRangePopupOpen() {
        let selectedFuelGroups = this.rcForm.get('FuelGroups').value;
        return selectedFuelGroups.length > 0 && !this.ShowMessage && (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P || (this.rcForm.get('RangeStartValue').value > 0 && this.rcForm.get('RangeEndValue').value > 0 && this.rcForm.get('RangeInterval').value > 0));
    }
    selectFiles(files) {
        if (files != null && files != undefined && files.length > 0)
            this.SelectedFiles = files;
    }
    GenerateRange(start, end, step = 0) {
        let output = [];
        if (typeof end === 'undefined') {
            end = start;
            start = 0;
        }
        for (let i = start; i <= end; i += step) {
            output.push(i);
        }
        return output;
    }
    downloadCsvTemplate() {
        this.isLoadingSubject.next(true);
        const a = document.createElement('a');
        var columnName = [];
        var template;
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            columnName.push("Upto");
        }
        else if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            columnName.push("Terminal/Bulk Plants");
            columnName.push("Location Name");
            columnName.push("Location Address");
            columnName.push("LaneId");
            columnName.push("Assumed Miles");
        }
        let selectedFuelGroups = this.rcForm.get('FuelGroups').value;
        let rep = ",";
        selectedFuelGroups.forEach(s => columnName.push(s.Name));
        template = columnName.join(',');
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            template = template.concat(",IsLaneRequired") + "\n";
            var cols = [];
            let trbls = this.rcForm.get('TerminalsAndBulkPlants').value;
            let sLocations = this.rcForm.get('Locations').value;
            let JobIds = [];
            sLocations.forEach(s => JobIds.push(s.Id));
            let locs = this.LocationList.filter(this.IdInComparer(JobIds));
            for (var i = 0; i < trbls.length; i++) {
                for (var j = 0; j < locs.length; j++) {
                    template = template.concat(trbls[i].Id + "|" + trbls[i].Name + "," + locs[j].Id + "|" + locs[j].Name + "," + locs[j].Code + ',' + trbls[i].Id.split("_")[1] + " " + locs[j].Id) + rep.repeat(selectedFuelGroups.length + 2) + "1" + "\n";
                }
            }
        }
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            let start = this.rcForm.get('RangeStartValue').value;
            let stop = this.rcForm.get('RangeEndValue').value;
            let step = this.rcForm.get('RangeInterval').value;
            template = template + "\n";
            this.GenerateRange(+start, +stop, +step).forEach(res => {
                template = template + ",".repeat(selectedFuelGroups.length) + "\n";
            });
        }
        var blob = new Blob(["\ufeff", template]);
        const objectUrl = URL.createObjectURL(blob);
        a.href = objectUrl;
        if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].Range) {
            a.download = 'FreightRate_Range_Table_Template.csv';
        }
        else if (this.PricingRuleType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightRateRuleType"].P2P) {
            a.download = 'FreightRate_P2P_Table_Template.csv';
        }
        a.click();
        URL.revokeObjectURL(objectUrl);
        this.isLoadingSubject.next(false);
    }
    getExtension(fileName) {
        // extract file name from full path ...
        var basename = fileName.split(/[\\/]/).pop();
        // (supports `\\` and `/` separators)
        var pos = basename.lastIndexOf("."); // get last position of `.`
        if (basename === "" || pos < 1) // if file name is empty or ...
            return ""; //  `.` not found (-1) or comes first (0)
        return basename.slice(pos + 1); // extract extension ignoring `.`
    }
    uploadRangeTableFile() {
        var files = this.SelectedFiles;
        if (files.length === 0)
            return;
        const formData = new FormData();
        for (var file of files) {
            if (!this.isValidFile(file)) {
                return;
            }
            formData.append(file.name, file);
        }
        this.isLoadingSubject.next(true);
        let reader = new FileReader();
        reader.readAsText(file);
        reader.onload = (e) => {
            this.CloseBulkUploadPopup.nativeElement.click();
            this.onImportClick.emit(reader.result);
            this.SelectedFiles = [];
            this.isLoadingSubject.next(false);
        };
    }
    isValidFile(file) {
        var isValid = true;
        var extension = this.getExtension(file.name);
        if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
        }
        if (file.size > this.MaxFileUploadSize) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
        }
        return isValid;
    }
    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                //console.log(other + " : " + current.Id);
                return other == current.Id;
            }).length == 1;
        };
    }
}
FreightComponent.ɵfac = function FreightComponent_Factory(t) { return new (t || FreightComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_11__["FreightRateRulesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_13__["ExternalMappingsService"])); };
FreightComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: FreightComponent, selectors: [["app-freight"]], viewQuery: function FreightComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.CloseBulkUploadPopup = _t.first);
    } }, inputs: { PricingRuleType: "PricingRuleType", IsEditable: "IsEditable" }, outputs: { onGenerateTable: "onGenerateTable", onGenerateFuelGroup: "onGenerateFuelGroup", onImportClick: "onImportClick" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]], decls: 85, vars: 74, consts: [[3, "formGroup"], [3, "ngClass", "disabled"], [1, "well", "bg-white"], [1, "row"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "col-sm-3", "form-group"], ["type", "text", "formControlName", "TableName", 1, "form-control", 3, "readonly"], ["class", "color-maroon", 4, "ngIf"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "disabled", "placeholder", "settings", "data", "onSelect"], [1, "col-sm-3", 3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "data", "placeholder", "settings"], [1, "col-sm-3", "form-group", 3, "ngClass"], ["for", "Customer"], ["formControlName", "Customers", "id", "Customer", 1, "single-select", 3, "data", "placeholder", "settings"], ["for", "Locations"], ["formControlName", "Locations", "id", "Locations", 3, "data", "placeholder", "settings"], [3, "ngClass"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "placeholder", "settings", "data"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "placeholder", "settings", "data"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], [1, "col-sm-4"], [1, "col"], [1, "form-group"], ["type", "text", "formControlName", "StartDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "formControlName", "EndDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], [1, "col-sm-3"], [1, "form-check", "form-check-inline"], ["type", "radio", "formControlName", "FuelGroupType", "id", "occurance-standard", 1, "form-check-input", 3, "name", "value", "change"], ["for", "occurance-standard", 1, "form-check-label"], ["type", "radio", "formControlName", "FuelGroupType", "id", "occurance-custom", 1, "form-check-input", 3, "name", "value", "change"], ["for", "occurance-custom", 1, "form-check-label"], ["for", "selectfuelGroups", 1, "form-check-label"], ["formControlName", "FuelGroups", "id", "selectfuelGroups", 3, "placeholder", "settings", "data"], ["class", "row", 4, "ngIf"], ["type", "button", "value", "Generate Table", 1, "btn", "btn-primary", "ml-0", 3, "click"], ["class", "col-sm-3", 4, "ngIf"], ["id", "RangeFreightRateModalPopup", "class", "modal fade", "role", "dialog", 4, "ngIf"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [1, "color-maroon"], [4, "ngIf"], [1, "table", "table-bordered", "table-hover", "mt-3"], ["colspan", "4"], ["type", "text", "formControlName", "RangeStartValue", 1, "form-control"], ["type", "text", "formControlName", "RangeEndValue", 1, "form-control"], ["type", "text", "formControlName", "RangeInterval", 1, "form-control"], ["type", "button", "data-toggle", "modal", "data-target", "#RangeFreightRateModalPopup", 1, "btn", "btn-primary", "float-right", "mb5", "valid", 3, "disabled", "click"], ["id", "RangeFreightRateModalPopup", "role", "dialog", 1, "modal", "fade"], [1, "modal-dialog"], [1, "modal-content"], [1, "modal-header", "pt0", "pb5"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", 3, "click"], ["btnCloseBulkUploadPopup", ""], [1, "fa", "fa-close", "fs21", "mt15"], [1, "modal-body"], [1, "mb10"], [1, "fa", "fa-download", "mr10", "mt10"], ["role", "button", 1, "mb5", "btn-download", 3, "click"], [1, "mb5", "mt5"], ["type", "file", "id", "bulkUploadFile", "accept", ".csv", 3, "change"], [1, "modal-footer"], ["type", "button", 1, "btn", "btn-default", 3, "click"]], template: function FreightComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "fieldset", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, FreightComponent_div_4_Template, 2, 0, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](5, "async");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Table Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](9, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](10, FreightComponent_div_10_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Table Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function FreightComponent_Template_ng_multiselect_dropdown_onSelect_14_listener($event) { return ctx.onTableTypeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, FreightComponent_div_15_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Select Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](19, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Select Customer");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](23, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, FreightComponent_div_24_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27, "Select Location(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](28, "ng-multiselect-dropdown", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, FreightComponent_div_29_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "label", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "Select Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](34, "ng-multiselect-dropdown", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, FreightComponent_div_35_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Select Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](39, "ng-multiselect-dropdown", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, FreightComponent_div_40_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "label", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Select Terminal(s)/BulkPlant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](45, "angular2-multiselect", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](46, "hr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](53, "Effective From");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function FreightComponent_Template_input_onDateChange_54_listener($event) { return ctx.setStartDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](55, FreightComponent_div_55_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](59, "End Date (Optional) ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "input", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function FreightComponent_Template_input_onDateChange_60_listener($event) { return ctx.setEndDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "div", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "input", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function FreightComponent_Template_input_change_65_listener() { return ctx.FuelGroupTypeChange(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "label", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](67, "Standard");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "div", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "input", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function FreightComponent_Template_input_change_69_listener() { return ctx.FuelGroupTypeChange(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](70, "label", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](71, "Custom");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](72, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](74, "label", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](75, "Fuel Groups");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](76, "ng-multiselect-dropdown", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](77, FreightComponent_div_77_Template, 2, 1, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](78, FreightComponent_div_78_Template, 26, 4, "div", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](79, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](80, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "input", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FreightComponent_Template_input_click_81_listener() { return ctx.generateTable(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](82, FreightComponent_div_82_Template, 2, 0, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](83, FreightComponent_div_83_Template, 6, 3, "div", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](84, FreightComponent_div_84_Template, 24, 3, "div", 41);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.rcForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](59, _c1, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](5, 57, ctx.isLoadingSubject));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx.IsEditable ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableName").invalid && ctx.rcForm.get("TableName").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.PricingRuleType == 1)("placeholder", "Select Type")("settings", ctx.SingleSelectSettingsById)("data", ctx.TableTypeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableTypes").invalid && ctx.rcForm.get("TableTypes").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](61, _c2, ctx.PricingRuleType == 1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.CustomerList)("placeholder", "Select Customers")("settings", ctx.MultiSelectSettingsById);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](64, _c2, ctx.PricingRuleType != 1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.CustomerList)("placeholder", "Select Customer")("settings", ctx.SingleSelectSettingsById);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Customers").invalid && ctx.rcForm.get("Customers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](67, _c2, ctx.PricingRuleType != 1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.LocationList)("placeholder", "Select Locations")("settings", ctx.MultiSelectSettingsById);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Locations").invalid && ctx.rcForm.get("Locations").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](70, _c3, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Carriers")("settings", ctx.MultiSelectSettingsById)("data", ctx.CarrierList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCarrierSelected && ctx.rcForm.get("Carriers").invalid && ctx.rcForm.get("Carriers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Source Regions")("settings", ctx.MultiSelectSettingsById)("data", ctx.SourceRegionList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("SourceRegions").invalid && ctx.rcForm.get("SourceRegions").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](72, _c3, !ctx.IsSourceRegionSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.TerminalsAndBulkPlantList)("settings", ctx.MultiSelectSettingsByGroup);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("StartDate").invalid && ctx.rcForm.get("StartDate").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinToDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "FuelGroupType")("value", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "FuelGroupType")("value", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵattribute"]("disabled", ctx.IsMasterSelected || ctx.rcForm.get("Customers").value.length > 1 || ctx.rcForm.get("Carriers").value.length > 1 || ctx.rcForm.get("Customers").value.length > 0 && ctx.rcForm.get("Carriers").value.length > 0 ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Fuel Group")("settings", ctx.MultiSelectSettingsById)("data", ctx.FuelGroupsList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("FuelGroups").invalid && ctx.rcForm.get("FuelGroups").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.PricingRuleType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.ShowMessage);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.PricingRuleType != 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsShowBulkUploadPopup && ctx.IsRangePopupOpen);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__["MultiSelectComponent"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["AngularMultiSelect"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RadioControlValueAccessor"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_14__["AsyncPipe"]], styles: ["body {\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2hhcmVkLWNvbXBvbmVudHMvRnJlaWdodC9mcmVpZ2h0LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7QUFDQSIsImZpbGUiOiJzcmMvYXBwL3NoYXJlZC1jb21wb25lbnRzL0ZyZWlnaHQvZnJlaWdodC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiYm9keSB7XHJcbn1cclxuIl19 */"], encapsulation: 2, changeDetection: 0 });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](FreightComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-freight',
                templateUrl: './freight.component.html',
                styleUrls: ['./freight.component.css'],
                changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectionStrategy"].OnPush,
                encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewEncapsulation"].None
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"] }, { type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"] }, { type: _freightRates_Services_freight_rate_rules_service__WEBPACK_IMPORTED_MODULE_11__["FreightRateRulesService"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"] }, { type: src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_13__["ExternalMappingsService"] }]; }, { PricingRuleType: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }], IsEditable: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }], onGenerateTable: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }], onGenerateFuelGroup: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }], onImportClick: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }], CloseBulkUploadPopup: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: ['btnCloseBulkUploadPopup']
        }] }); })();


/***/ })

}]);
//# sourceMappingURL=freightRates-freight-rate-module-es2015.js.map