(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module"],{

/***/ "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts":
/*!*************************************************************!*\
  !*** ./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts ***!
  \*************************************************************/
/*! exports provided: LoadFilterModel, DipTestViewModel, TankCapacityForDR, ModifiedTripInfo, CreateDeliveryRequestViewModel, CustomerJobsForCarrier, PartialDRDetails, TfxModule, WindowModeFilter, UoM, DeliveryRequestViewModel, RegionDetailModel, ShiftModel, ScheduleBuilderModel, DSBSaveModel, DRDragDropModel, SbDriverViewModel, SbTrailerViewModel, TrailerViewModel, TrailerShiftModel, ScheduleShiftModel, ShiftDetailModel, TrailerModel, LocationFilters, TripModel, DropAddressModel, OrderPickupDetailModel, OrderPickupLocationModel, WhereIsMyDriverModel, DistatcherRegionModel, Filter, SbFilterModel, TrailerViewFilterModel, DriverViewFilterModel, CompanyUsers, TankMinMaxFill, TankChartHeight, DipTest, DriverAdditionalDetails, routesColor, DemandModel, PartialDRDetail, LoadInfo, PreLoadDrViewModel, PreLoadDrResponseViewModel, PreLoadDrModel, SalesTankFilterModal, SalesFilterModal */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LoadFilterModel", function() { return LoadFilterModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DipTestViewModel", function() { return DipTestViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TankCapacityForDR", function() { return TankCapacityForDR; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ModifiedTripInfo", function() { return ModifiedTripInfo; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateDeliveryRequestViewModel", function() { return CreateDeliveryRequestViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CustomerJobsForCarrier", function() { return CustomerJobsForCarrier; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartialDRDetails", function() { return PartialDRDetails; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TfxModule", function() { return TfxModule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "WindowModeFilter", function() { return WindowModeFilter; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "UoM", function() { return UoM; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryRequestViewModel", function() { return DeliveryRequestViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RegionDetailModel", function() { return RegionDetailModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ShiftModel", function() { return ShiftModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ScheduleBuilderModel", function() { return ScheduleBuilderModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DSBSaveModel", function() { return DSBSaveModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DRDragDropModel", function() { return DRDragDropModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SbDriverViewModel", function() { return SbDriverViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SbTrailerViewModel", function() { return SbTrailerViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerViewModel", function() { return TrailerViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerShiftModel", function() { return TrailerShiftModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ScheduleShiftModel", function() { return ScheduleShiftModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ShiftDetailModel", function() { return ShiftDetailModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerModel", function() { return TrailerModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LocationFilters", function() { return LocationFilters; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TripModel", function() { return TripModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DropAddressModel", function() { return DropAddressModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderPickupDetailModel", function() { return OrderPickupDetailModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderPickupLocationModel", function() { return OrderPickupLocationModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverModel", function() { return WhereIsMyDriverModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DistatcherRegionModel", function() { return DistatcherRegionModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Filter", function() { return Filter; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SbFilterModel", function() { return SbFilterModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TrailerViewFilterModel", function() { return TrailerViewFilterModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverViewFilterModel", function() { return DriverViewFilterModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CompanyUsers", function() { return CompanyUsers; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TankMinMaxFill", function() { return TankMinMaxFill; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TankChartHeight", function() { return TankChartHeight; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DipTest", function() { return DipTest; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverAdditionalDetails", function() { return DriverAdditionalDetails; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "routesColor", function() { return routesColor; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DemandModel", function() { return DemandModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartialDRDetail", function() { return PartialDRDetail; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LoadInfo", function() { return LoadInfo; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PreLoadDrViewModel", function() { return PreLoadDrViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PreLoadDrResponseViewModel", function() { return PreLoadDrResponseViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PreLoadDrModel", function() { return PreLoadDrModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SalesTankFilterModal", function() { return SalesTankFilterModal; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SalesFilterModal", function() { return SalesFilterModal; });
/* harmony import */ var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/statelist.service */ "./src/app/statelist.service.ts");

class LoadFilterModel {
}
class DipTestViewModel {
    constructor() {
        this.ExistingDR = [];
    }
}
class TankCapacityForDR {
}
class ModifiedTripInfo {
}
class CreateDeliveryRequestViewModel {
}
class CustomerJobsForCarrier {
}
class PartialDRDetails {
}
var TfxModule;
(function (TfxModule) {
    TfxModule[TfxModule["None"] = 0] = "None";
    TfxModule[TfxModule["SupplierWallyboardLocation"] = 1] = "SupplierWallyboardLocation";
    TfxModule[TfxModule["SupplierWallyboardLoads"] = 2] = "SupplierWallyboardLoads";
    TfxModule[TfxModule["SupplierWallyboardSales"] = 3] = "SupplierWallyboardSales";
    TfxModule[TfxModule["SupplierWallyboardSalesPriority"] = 4] = "SupplierWallyboardSalesPriority";
    TfxModule[TfxModule["SupplierWallyboardSalesTanks"] = 5] = "SupplierWallyboardSalesTanks";
    TfxModule[TfxModule["BuyerWallyboardLocation"] = 6] = "BuyerWallyboardLocation";
    TfxModule[TfxModule["BuyerWallyboardLoads"] = 7] = "BuyerWallyboardLoads";
    TfxModule[TfxModule["BuyerWallyboardSales"] = 8] = "BuyerWallyboardSales";
    TfxModule[TfxModule["BuyerWallyboardSalesPriority"] = 9] = "BuyerWallyboardSalesPriority";
    TfxModule[TfxModule["BuyerWallyboardSalesTanks"] = 10] = "BuyerWallyboardSalesTanks";
    TfxModule[TfxModule["BuyerWallyboardSalesLocation"] = 11] = "BuyerWallyboardSalesLocation";
    TfxModule[TfxModule["AssignedByMeDeliveryRequests"] = 12] = "AssignedByMeDeliveryRequests";
    TfxModule[TfxModule["DSBShift"] = 13] = "DSBShift";
})(TfxModule || (TfxModule = {}));
var WindowModeFilter;
(function (WindowModeFilter) {
    WindowModeFilter[WindowModeFilter["Single"] = 1] = "Single";
    WindowModeFilter[WindowModeFilter["Multi"] = 2] = "Multi";
})(WindowModeFilter || (WindowModeFilter = {}));
var UoM;
(function (UoM) {
    UoM[UoM["None"] = 0] = "None";
    UoM[UoM["Gallons"] = 1] = "Gallons";
    UoM[UoM["Litres"] = 2] = "Litres";
})(UoM || (UoM = {}));
class DeliveryRequestViewModel {
    constructor() {
        this.Terminal = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
        this.BulkPlant = new DropAddressModel();
        this.PickupLocationType = 1;
        this.WindowMode = 1;
        this.QueueMode = 1;
    }
}
class RegionDetailModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Shifts = [];
    }
}
class ShiftModel {
}
class ScheduleBuilderModel {
    constructor() {
        this.Shifts = [];
        this.Trailers = [];
    }
}
class DSBSaveModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.PreloadedDRs = [];
        this.PostloadedDRs = [];
        this.Trips = [];
    }
}
class DRDragDropModel extends ScheduleBuilderModel {
}
class SbDriverViewModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.Shifts = [];
    }
}
class SbTrailerViewModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.Trailers = [];
    }
}
class TrailerViewModel {
}
class TrailerShiftModel {
}
class ScheduleShiftModel {
    constructor() {
        this.Schedules = [];
        this.IsCollapsed = false;
    }
}
class ShiftDetailModel {
}
class TrailerModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Trips = [];
    }
}
class LocationFilters {
    constructor() {
        this.state = [];
        this.city = [];
        this.product = [];
        this.priority = [];
        this.customer = [];
        this.supplier = [];
        this.carrier = [];
    }
}
class TripModel {
    constructor() {
        this.DeliveryRequests = [];
        this.Terminal = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
        this.BulkPlant = new DropAddressModel();
        this.Drivers = [];
        this.Trailers = [];
    }
}
class DropAddressModel {
    constructor() {
        this.State = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
        this.Country = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
    }
}
class OrderPickupDetailModel {
    constructor() {
        this.PickupLocationType = 1;
    }
}
class OrderPickupLocationModel {
    static ToLocation(orderPickupDetail) {
        let location = new OrderPickupLocationModel();
        location.PickupLocationType = orderPickupDetail.PickupLocationType;
        location.Terminal = {
            Id: orderPickupDetail.TerminalId,
            Name: orderPickupDetail.TerminalName,
            Code: ''
        };
        location.BulkPlant = {
            Address: orderPickupDetail.Address,
            City: orderPickupDetail.City,
            State: { Id: orderPickupDetail.StateId, Code: orderPickupDetail.StateCode, Name: null },
            Country: { Id: 0, Code: orderPickupDetail.CountryCode, Name: null },
            ZipCode: orderPickupDetail.ZipCode,
            CountyName: orderPickupDetail.CountyName,
            Latitude: orderPickupDetail.Latitude,
            Longitude: orderPickupDetail.Longitude,
            SiteName: orderPickupDetail.BulkplantName,
            SiteId: null
        };
        return location;
    }
}
class WhereIsMyDriverModel {
    constructor() {
        this.routeShow = false;
    }
}
class DistatcherRegionModel {
}
class Filter {
    constructor() {
        this['state'] = [];
        this['city'] = [];
        this['product'] = [];
        this['priority'] = [];
        this['customer'] = [];
        this['supplier'] = [];
        this['carrier'] = [];
    }
}
class SbFilterModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Pickups = [];
        this.SelectedDrivers = [];
        this.SelectedPickups = [];
        this.SelectedTrailers = [];
    }
}
class TrailerViewFilterModel {
    constructor() {
        this.Shifts = {};
        this.Trailers = {};
        this.Pickups = {};
        this.Drivers = {};
    }
}
class DriverViewFilterModel {
    constructor() {
        this.Shifts = {};
    }
}
class CompanyUsers {
}
class TankMinMaxFill {
}
class TankChartHeight {
}
class DipTest {
}
class DriverAdditionalDetails {
    constructor(data) {
        this.Id = data && data['Id'] || null;
        this.Name = data && data['Name'] || null;
        this.License = data && data['License'] || null;
        this.ContactNumnber = data && data['ContactNumnber'] || null;
        this.Shifts = data && data['Shifts'] || [];
        this.Trailers = data && data['Trailers'] || [];
    }
}
const routesColor = {
    1: '#5f4aa8',
    11: '#c4c105',
    12: '#d3950f',
    18: '#19953f',
    20: '#e3584d'
};
class DemandModel {
}
class PartialDRDetail {
}
class LoadInfo {
}
class PreLoadDrViewModel {
}
class PreLoadDrResponseViewModel {
}
class PreLoadDrModel {
}
class SalesTankFilterModal {
    constructor() {
        this.selectedLocAttributeData = [];
    }
}
class SalesFilterModal {
}


/***/ }),

/***/ "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts":
/*!***********************************************************************!*\
  !*** ./src/app/buyer-wally-board/services/buyerwallyboard.service.ts ***!
  \***********************************************************************/
/*! exports provided: BuyerwallyboardService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BuyerwallyboardService", function() { return BuyerwallyboardService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../../app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");






class BuyerwallyboardService extends _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getJobLocationDetailsUrl = '/Buyer/Job/GetJobLocationDetails'; // Move this to Job Controller buyer area
        this.getSupplierCarrierDDLUrl = '/Buyer/Job/GetSupplierCarrierForjobs';
        this.getDipTestDetailsUrl = '/Buyer/Dashboard/GetDipTestDetails?';
        this.getDeliveriesForLocations = '/Buyer/WallyBoard/GetDeliveriesForLocations?';
        this.getFilterData = 'Buyer/WallyBoard/GetBuyerLoadFilterData';
        this.getDispatcherCountryUrl = '/Buyer/WallyBoard/GetUserCountry';
        this.getOnGoingLoadsUrl = '/Buyer/WallyBoard/GetOnGoingLoadsForMapView';
        this.getBuyerLoadsForGridUrl = '/Buyer/WallyBoard/GetBuyerLoads';
        this.getDriverAdditionalDetailsUrl = '/Buyer/WallyBoard/GetDriverAdditionalDetails?driverId=';
        this.getFiltersUrl = '/Buyer/WallyBoard/GetFilters?moduleId=';
        this.saveFiltersUrl = '/Buyer/WallyBoard/SaveFilters';
        this.SingleMultiWindowSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1); //singlemulti window screen 1
    }
    getBuyerLoadsForGrid(inputs) {
        return this.httpClient.post(this.getBuyerLoadsForGridUrl, inputs)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getBuyerLoadsForGrid', null)));
    }
    getFilters(moduleId) {
        return this.httpClient.get(this.getFiltersUrl + moduleId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFilters', null)));
    }
    saveFilters(moduleId, input) {
        var data = { moduleId: moduleId, filterInput: input };
        return this.httpClient.post(this.saveFiltersUrl, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('saveFilters', null)));
    }
    getDriverAdditionalDetails(driverId) {
        return this.httpClient.get(this.getDriverAdditionalDetailsUrl + driverId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDriverAdditionalDetails', null)));
    }
    getJobLocationDetails(jobIds, selectedLocAttributeId) {
        var data = { jobList: jobIds, inventoryCaptureTypeIds: selectedLocAttributeId };
        return Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["timer"])(0, 60 * 60 * 1000).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["switchMap"])(() => {
            return this.httpClient.post(this.getJobLocationDetailsUrl, data);
        })).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getJobLocationDetails', null)));
    }
    getSuppliersCarrierssDDL(jobIds) {
        return Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["timer"])(0, 60 * 60 * 1000).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["switchMap"])(() => {
            return this.httpClient.post(this.getSupplierCarrierDDLUrl, { jobIds: jobIds });
        })).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getSuppliersCarrierssDDL', null)));
    }
    getDipTestDetails(siteId, tankId, noOfDays) {
        return this.httpClient.get(this.getDipTestDetailsUrl + 'siteId=' + siteId + '&' + 'tankId=' + tankId + '&' + 'noOfDays=' + noOfDays)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDipTestDetails', null)));
    }
    GetDeliveriesForLocations(fromDate, toDate) {
        return this.httpClient.get(this.getDeliveriesForLocations + 'fromDate=' + fromDate + '&' + 'toDate=' + toDate)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetDeliveriesForLocations', null)));
    }
    GetFilterData(isShowCarrierManaged) {
        return this.httpClient.get(this.getFilterData + "?isShowCarrierManaged=" + isShowCarrierManaged)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetFilterData', null)));
    }
    getDispatcherCountry() {
        return this.httpClient.get(this.getDispatcherCountryUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDispatcherCountry', null)));
    }
    getOnGoingLoadsForMap(inputs) {
        return this.httpClient.post(this.getOnGoingLoadsUrl, inputs)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getOnGoingLoads', null)));
    }
}
BuyerwallyboardService.ɵfac = function BuyerwallyboardService_Factory(t) { return new (t || BuyerwallyboardService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"])); };
BuyerwallyboardService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: BuyerwallyboardService, factory: BuyerwallyboardService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](BuyerwallyboardService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"] }]; }, null); })();


/***/ })

}]);
//# sourceMappingURL=default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module-es2015.js.map