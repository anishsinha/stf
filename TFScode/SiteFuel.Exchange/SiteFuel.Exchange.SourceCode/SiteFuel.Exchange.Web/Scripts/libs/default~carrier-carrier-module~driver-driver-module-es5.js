function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~carrier-carrier-module~driver-driver-module"], {
  /***/
  "./src/app/carrier/service/route-info.service.ts": function srcAppCarrierServiceRouteInfoServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RouteInfoService", function () {
      return RouteInfoService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var RouteInfoService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(RouteInfoService, _src_app_errors_Handl);

      var _super = _createSuper(RouteInfoService);

      function RouteInfoService(httpClient) {
        var _this;

        _classCallCheck(this, RouteInfoService);

        _this = _super.call(this);
        _this.httpClient = httpClient;
        _this.urlGetRouteInfoDetails = '/Carrier/ScheduleBuilder/GetRouteInfoDetails';
        _this.urlGetRegionLocationDetails = '/Carrier/ScheduleBuilder/GetRegionLocationDetails';
        _this.urlGetRouteLocationDetails = '/Carrier/ScheduleBuilder/GetRouteLocationDetails';
        _this.urlDeleteRouteInfo = '/Carrier/ScheduleBuilder/DeleteRouteInfo';
        _this.urlCreateRouteInfo = '/Carrier/ScheduleBuilder/CreateRouteInfo';
        _this.urlUpdateRouteInfo = '/Carrier/ScheduleBuilder/UpdateRouteInfo';
        _this.routeInfoDetails = new rxjs__WEBPACK_IMPORTED_MODULE_1__["BehaviorSubject"](_this.routeInfo);
        return _this;
      }

      _createClass(RouteInfoService, [{
        key: "getRoutesByRegion",
        value: function getRoutesByRegion(regionId) {
          return this.httpClient.get(this.urlGetRouteInfoDetails + '?regionId=' + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRoutesByRegion', null)));
        }
      }, {
        key: "getLocationsByRegion",
        value: function getLocationsByRegion(regionId) {
          return this.httpClient.get(this.urlGetRegionLocationDetails + '?regionId=' + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLocationsByRegion', null)));
        }
      }, {
        key: "getLocationsByRoute",
        value: function getLocationsByRoute(Id, regionId) {
          return this.httpClient.get(this.urlGetRouteLocationDetails + '?Id=' + Id + '&regionId=' + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLocationsByRegion', null)));
        }
      }, {
        key: "deleteRouteInfo",
        value: function deleteRouteInfo(data) {
          return this.httpClient.post(this.urlDeleteRouteInfo, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRouteInfo', null)));
        }
      }, {
        key: "createRouteInfo",
        value: function createRouteInfo(data) {
          return this.httpClient.post(this.urlCreateRouteInfo, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRouteInfo', null)));
        }
      }, {
        key: "updateRouteInfo",
        value: function updateRouteInfo(data) {
          return this.httpClient.post(this.urlUpdateRouteInfo, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRouteInfo', null)));
        }
      }, {
        key: "sendRouteInfo",
        value: function sendRouteInfo(memberData) {
          this.routeInfoDetails.next(memberData);
        }
      }]);

      return RouteInfoService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__["HandleError"]);

    RouteInfoService.ɵfac = function RouteInfoService_Factory(t) {
      return new (t || RouteInfoService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"]));
    };

    RouteInfoService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: RouteInfoService,
      factory: RouteInfoService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RouteInfoService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/service/region.service.ts": function srcAppCompanyAddressesRegionServiceRegionServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionService", function () {
      return RegionService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var RegionService = /*#__PURE__*/function (_src_app_errors_Handl2) {
      _inherits(RegionService, _src_app_errors_Handl2);

      var _super2 = _createSuper(RegionService);

      function RegionService(httpClient) {
        var _this2;

        _classCallCheck(this, RegionService);

        _this2 = _super2.call(this);
        _this2.httpClient = httpClient;
        _this2.createUrl = '/Region/Create';
        _this2.updateUrl = '/Region/Update';
        _this2.deleteUrl = '/Region/Delete?id=';
        _this2.getRegionsUrl = '/Region/GetRegions';
        _this2.getSourceRegionsUrl = '/Region/GetSourceRegions';
        _this2.createSourceRegionUrl = '/Region/CreateSourceRegion';
        _this2.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        _this2.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        _this2.getJobsUrl = '/Region/GetJobs';
        _this2.getDriversUrl = '/Region/GetDrivers';
        _this2.getDispatchersUrl = '/Region/GetDispatchers';
        _this2.getTrailersUrl = '/Region/GetTrailers';
        _this2.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        _this2.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        _this2.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        _this2.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        _this2.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        _this2.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        _this2.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        _this2.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        _this2.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        _this2.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        _this2.getCarriersUrl = '/Region/GetCarriers';
        _this2.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        _this2.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        _this2.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        _this2.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        _this2.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        _this2.getFuelProductUrl = '/Region/GetMstFuelProducts';
        _this2.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        _this2.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        return _this2;
      }

      _createClass(RegionService, [{
        key: "getJobs",
        value: function getJobs() {
          return this.httpClient.get(this.getJobsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobs', [])));
        }
      }, {
        key: "getDrivers",
        value: function getDrivers() {
          return this.httpClient.get(this.getDriversUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
        }
      }, {
        key: "getRegionDrivers",
        value: function getRegionDrivers(regiondId) {
          return this.httpClient.get(this.getRegionDriversUrl + regiondId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
        }
      }, {
        key: "getCompanyShifts",
        value: function getCompanyShifts() {
          return this.httpClient.get(this.getCompanyShiftsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCompanyShifts', [])));
        }
      }, {
        key: "getRoutesByRegion",
        value: function getRoutesByRegion(regionId) {
          return this.httpClient.get(this.getRouteByReginId + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetRouteInfoDetails', [])));
        }
      }, {
        key: "getDispatchers",
        value: function getDispatchers() {
          return this.httpClient.get(this.getDispatchersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDispatchers', [])));
        }
      }, {
        key: "getTrailers",
        value: function getTrailers() {
          return this.httpClient.get(this.getTrailersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTrailers', [])));
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          return this.httpClient.get(this.getRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegions', null)));
        }
      }, {
        key: "createRegion",
        value: function createRegion(region) {
          return this.httpClient.post(this.createUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRegion', region)));
        }
      }, {
        key: "updateRegion",
        value: function updateRegion(region) {
          return this.httpClient.post(this.updateUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateRegion', region)));
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions() {
          return this.httpClient.get(this.getSourceRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSourceRegions', null)));
        }
      }, {
        key: "createSourceRegion",
        value: function createSourceRegion(region) {
          return this.httpClient.post(this.createSourceRegionUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createSourceRegion', region)));
        }
      }, {
        key: "isSourceRegionAvailable",
        value: function isSourceRegionAvailable(name, id) {
          return this.httpClient.get(this.isSourceRegionAvailableUrl + name + "&id=" + id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isSourceRegionAvailable', null)));
        }
      }, {
        key: "updateSourceRegion",
        value: function updateSourceRegion(region) {
          return this.httpClient.post(this.updateSourceRegionUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateSourceRegion', region)));
        }
      }, {
        key: "deleteRegion",
        value: function deleteRegion(id) {
          return this.httpClient.post(this.deleteUrl + id, id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRegion', id)));
        }
      }, {
        key: "deleteSourceRegion",
        value: function deleteSourceRegion(id) {
          return this.httpClient.post(this.deleteSourceRegionUrl + id, id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteSourceRegion', id)));
        }
      }, {
        key: "getStates",
        value: function getStates(countryId) {
          return this.httpClient.get(this.stateUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getStates', [])));
        } //for calender

      }, {
        key: "getShiftByDrivers",
        value: function getShiftByDrivers(driverIds, scheduleType) {
          return this.httpClient.get(this.shiftByDriverUrl + driverIds + "&scheduleType=" + scheduleType).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getShiftByDrivers', [])));
        }
      }, {
        key: "getSchedulesByRegion",
        value: function getSchedulesByRegion(regionId, scheduleType) {
          return this.httpClient.get(this.getRegionSchedulsbyRegionIdUrl + regionId + "&scheduleType=" + scheduleType).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSchedulesByRegion', [])));
        }
      }, {
        key: "getRegionSchedule",
        value: function getRegionSchedule(regionId, routeId) {
          return this.httpClient.get(this.getRegionShiftMapping + regionId + "&routeId=" + routeId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegionSchedule', [])));
        }
      }, {
        key: "addDriverSchedule",
        value: function addDriverSchedule(model) {
          return this.httpClient.post(this.addDriverScheduleUrl, model, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', model)));
        }
      }, {
        key: "addRegionSchedule",
        value: function addRegionSchedule(model) {
          return this.httpClient.post(this.addRegionScheduleUrl, model, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRegionSchedule', model)));
        }
      }, {
        key: "updateDriverSchedule",
        value: function updateDriverSchedule(data, date) {
          var postModel = {
            model: data,
            SelectedDate: date
          };
          return this.httpClient.post(this.updateDriverScheduleUrl, postModel, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', postModel)));
        }
      }, {
        key: "deleteDriverSchedule",
        value: function deleteDriverSchedule(data, date) {
          var postModel = {
            driverScheduleMappingViewModels: data,
            SelectedDate: date
          };
          return this.httpClient.post(this.deleteDriverScheduleUrl, postModel, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteDriverSchedule', postModel)));
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          return this.httpClient.get(this.getCarriersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriers', [])));
        }
      }, {
        key: "getCarrierRegions",
        value: function getCarrierRegions() {
          return this.httpClient.get(this.getCarrierRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierRegions', null)));
        }
      }, {
        key: "getSelectedCarriersByRegion",
        value: function getSelectedCarriersByRegion(regionId) {
          return this.httpClient.get(this.getSelectedCarriersByRegionUrl + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSelectedCarriersByRegion', null)));
        }
      }, {
        key: "getProductType",
        value: function getProductType() {
          return this.httpClient.get(this.getProductTypeUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductType', [])));
        }
      }, {
        key: "getFuelProducts",
        value: function getFuelProducts() {
          return this.httpClient.get(this.getFuelProductUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelProducts', [])));
        }
      }, {
        key: "isPublishedDR",
        value: function isPublishedDR(productIds, fuelTypeIds) {
          return this.httpClient.get(this.isPublishedDRUrl + productIds + "&fuelTypeIds=" + fuelTypeIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isPublishedDR', null)));
        }
      }]);

      return RegionService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__["HandleError"]);

    RegionService.ɵfac = function RegionService_Factory(t) {
      return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]));
    };

    RegionService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: RegionService,
      factory: RegionService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]
        }];
      }, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=default~carrier-carrier-module~driver-driver-module-es5.js.map