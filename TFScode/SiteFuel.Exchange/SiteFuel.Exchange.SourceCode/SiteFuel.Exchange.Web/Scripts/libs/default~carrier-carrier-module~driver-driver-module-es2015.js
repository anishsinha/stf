(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~carrier-carrier-module~driver-driver-module"],{

/***/ "./src/app/carrier/service/route-info.service.ts":
/*!*******************************************************!*\
  !*** ./src/app/carrier/service/route-info.service.ts ***!
  \*******************************************************/
/*! exports provided: RouteInfoService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RouteInfoService", function() { return RouteInfoService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");






class RouteInfoService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.urlGetRouteInfoDetails = '/Carrier/ScheduleBuilder/GetRouteInfoDetails';
        this.urlGetRegionLocationDetails = '/Carrier/ScheduleBuilder/GetRegionLocationDetails';
        this.urlGetRouteLocationDetails = '/Carrier/ScheduleBuilder/GetRouteLocationDetails';
        this.urlDeleteRouteInfo = '/Carrier/ScheduleBuilder/DeleteRouteInfo';
        this.urlCreateRouteInfo = '/Carrier/ScheduleBuilder/CreateRouteInfo';
        this.urlUpdateRouteInfo = '/Carrier/ScheduleBuilder/UpdateRouteInfo';
        this.routeInfoDetails = new rxjs__WEBPACK_IMPORTED_MODULE_1__["BehaviorSubject"](this.routeInfo);
    }
    getRoutesByRegion(regionId) {
        return this.httpClient.get(this.urlGetRouteInfoDetails + '?regionId=' + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRoutesByRegion', null)));
    }
    getLocationsByRegion(regionId) {
        return this.httpClient.get(this.urlGetRegionLocationDetails + '?regionId=' + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLocationsByRegion', null)));
    }
    getLocationsByRoute(Id, regionId) {
        return this.httpClient.get(this.urlGetRouteLocationDetails + '?Id=' + Id + '&regionId=' + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLocationsByRegion', null)));
    }
    deleteRouteInfo(data) {
        return this.httpClient.post(this.urlDeleteRouteInfo, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRouteInfo', null)));
    }
    createRouteInfo(data) {
        return this.httpClient.post(this.urlCreateRouteInfo, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRouteInfo', null)));
    }
    updateRouteInfo(data) {
        return this.httpClient.post(this.urlUpdateRouteInfo, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRouteInfo', null)));
    }
    sendRouteInfo(memberData) {
        this.routeInfoDetails.next(memberData);
    }
}
RouteInfoService.ɵfac = function RouteInfoService_Factory(t) { return new (t || RouteInfoService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"])); };
RouteInfoService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: RouteInfoService, factory: RouteInfoService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RouteInfoService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"] }]; }, null); })();


/***/ }),

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


/***/ })

}]);
//# sourceMappingURL=default~carrier-carrier-module~driver-driver-module-es2015.js.map