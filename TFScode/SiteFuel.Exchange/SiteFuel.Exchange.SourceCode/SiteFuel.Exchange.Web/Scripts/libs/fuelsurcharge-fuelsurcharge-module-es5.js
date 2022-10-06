function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _iterableToArrayLimit(arr, i) { var _i = arr == null ? null : typeof Symbol !== "undefined" && arr[Symbol.iterator] || arr["@@iterator"]; if (_i == null) return; var _arr = []; var _n = true; var _d = false; var _s, _e; try { for (_i = _i.call(arr); !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["fuelsurcharge-fuelsurcharge-module"], {
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

    var RegionService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(RegionService, _src_app_errors_Handl);

      var _super = _createSuper(RegionService);

      function RegionService(httpClient) {
        var _this;

        _classCallCheck(this, RegionService);

        _this = _super.call(this);
        _this.httpClient = httpClient;
        _this.createUrl = '/Region/Create';
        _this.updateUrl = '/Region/Update';
        _this.deleteUrl = '/Region/Delete?id=';
        _this.getRegionsUrl = '/Region/GetRegions';
        _this.getSourceRegionsUrl = '/Region/GetSourceRegions';
        _this.createSourceRegionUrl = '/Region/CreateSourceRegion';
        _this.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        _this.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        _this.getJobsUrl = '/Region/GetJobs';
        _this.getDriversUrl = '/Region/GetDrivers';
        _this.getDispatchersUrl = '/Region/GetDispatchers';
        _this.getTrailersUrl = '/Region/GetTrailers';
        _this.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        _this.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        _this.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        _this.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        _this.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        _this.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        _this.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        _this.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        _this.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        _this.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        _this.getCarriersUrl = '/Region/GetCarriers';
        _this.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        _this.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        _this.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        _this.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        _this.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        _this.getFuelProductUrl = '/Region/GetMstFuelProducts';
        _this.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        _this.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        return _this;
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

  },

  /***/
  "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts": function srcAppFuelsurchargeCreateCreateFuelSurchargeComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateFuelSurchargeComponent", function () {
      return CreateFuelSurchargeComponent;
    });
    /* harmony import */


    var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! tslib */
    "./node_modules/tslib/tslib.es6.js");
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_fuelsurcharge_models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/fuelsurcharge/models/CreateFuelSurcharge */
    "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_7__);
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../../carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function CreateFuelSurchargeComponent_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "button", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_div_2_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r21);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r20.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_12_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_12_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.rcForm.get("TableName").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_20_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_20_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.rcForm.get("TableTypes").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_27_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_27_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.get("Customers").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_34_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_34_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.rcForm.get("Carriers").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_42_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_42_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_42_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.rcForm.get("SourceRegions").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_input_54_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "input", 29);
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "eia-period")("value", 1)("checked", ctx_r6.viewType == 1);
      }
    }

    function CreateFuelSurchargeComponent_label_55_Template(rf, ctx) {
      if (rf & 1) {
        var _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_label_55_Template_label_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r28);

          var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r27.changeViewType(1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "API Update");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_8_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_8_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r30.rcForm.get("FuelSurchargeProducts").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_span_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_15_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_15_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r32.rcForm.get("FuelSurchargePeriods").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_span_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_22_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_22_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r34.rcForm.get("FuelSurchargeAreas").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_input_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "input", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateFuelSurchargeComponent_div_61_input_40_Template_input_onDateChange_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r48);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r47.setApiAdjustIndexPriceDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx_r35.MinFromDate)("maxDate", ctx_r35.MaxStartDate)("mode", "WEEKLY")("daysOfWeekEnable", "MON");
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_41_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_41_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_41_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r36.rcForm.get("ApiAdjustIndexPriceDate").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_42_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "ng-multiselect-dropdown", 68);
      }

      if (rf & 2) {
        var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select month")("settings", ctx_r37.SingleSelectSettingsById)("data", ctx_r37.SourceMonthList);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_43_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_43_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r38.rcForm.get("SourceMonths").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "ng-multiselect-dropdown", 69);
      }

      if (rf & 2) {
        var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Annually")("settings", ctx_r39.SingleSelectSettingsById)("data", ctx_r39.SourceAnnualyList);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_45_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_45_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r40.rcForm.get("SourceAnnualy").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_46_div_34_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_46_div_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_46_div_34_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r52.rcForm.get("Weeks").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Effective From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "input", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Mon");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "input", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Tue");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](12, "input", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "label", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Wed");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](16, "input", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Thu");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](20, "input", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "label", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Fri");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](24, "input", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "label", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Sat");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](28, "input", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "label", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Sun");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](33, "ng-multiselect-dropdown", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, CreateFuelSurchargeComponent_div_61_div_46_div_34_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select week")("settings", ctx_r41.SingleSelectSettingsById)("data", ctx_r41.WeekList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r41.rcForm.get("Weeks").invalid && ctx_r41.rcForm.get("Weeks").touched);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_47_div_6_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_47_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_47_div_6_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r54.rcForm.get("Months").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Effective From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "ng-multiselect-dropdown", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](6, CreateFuelSurchargeComponent_div_61_div_47_div_6_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select month")("settings", ctx_r42.SingleSelectSettingsById)("data", ctx_r42.MonthList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r42.rcForm.get("Months").invalid && ctx_r42.rcForm.get("Months").touched);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_48_div_6_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_48_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_61_div_48_div_6_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r56.rcForm.get("Annualy").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_61_div_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Effective From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "ng-multiselect-dropdown", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](6, CreateFuelSurchargeComponent_div_61_div_48_div_6_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Annually")("settings", ctx_r43.SingleSelectSettingsById)("data", ctx_r43.AnnualyList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r43.rcForm.get("Annualy").invalid && ctx_r43.rcForm.get("Annualy").touched);
      }
    }

    function CreateFuelSurchargeComponent_div_61_Template(rf, ctx) {
      if (rf & 1) {
        var _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](6, CreateFuelSurchargeComponent_div_61_span_6_Template, 2, 0, "span", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "ng-multiselect-dropdown", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, CreateFuelSurchargeComponent_div_61_div_8_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](13, CreateFuelSurchargeComponent_div_61_span_13_Template, 2, 0, "span", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateFuelSurchargeComponent_div_61_Template_ng_multiselect_dropdown_onSelect_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r59);

          var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r58.onFuelSurchargePeriodsSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, CreateFuelSurchargeComponent_div_61_div_15_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, CreateFuelSurchargeComponent_div_61_span_20_Template, 2, 0, "span", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](21, "ng-multiselect-dropdown", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, CreateFuelSurchargeComponent_div_61_div_22_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "\xA0");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "a", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_div_61_Template_a_click_29_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r59);

          var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r60.onFetchLastIndexPrice();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Fetch Latest Index Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](32, "input", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "p", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39, "Start of the Index Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, CreateFuelSurchargeComponent_div_61_input_40_Template, 1, 5, "input", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](41, CreateFuelSurchargeComponent_div_61_div_41_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](42, CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_42_Template, 1, 3, "ng-multiselect-dropdown", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](43, CreateFuelSurchargeComponent_div_61_div_43_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](44, CreateFuelSurchargeComponent_div_61_ng_multiselect_dropdown_44_Template, 1, 3, "ng-multiselect-dropdown", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, CreateFuelSurchargeComponent_div_61_div_45_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, CreateFuelSurchargeComponent_div_61_div_46_Template, 35, 4, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](47, CreateFuelSurchargeComponent_div_61_div_47_Template, 7, 4, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](48, CreateFuelSurchargeComponent_div_61_div_48_Template, 7, 4, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Product" : "NRC Product");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.SelectedCountryId == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select EIA Product")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargeProductList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.rcForm.get("FuelSurchargeProducts").invalid && ctx_r8.rcForm.get("FuelSurchargeProducts").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Period" : "NRC Period");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.SelectedCountryId == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select EIA Period")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargePeriodList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.rcForm.get("FuelSurchargePeriods").invalid && ctx_r8.rcForm.get("FuelSurchargePeriods").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r8.SelectedCountryId == 1 ? "EIA Area" : "NRC Area");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.SelectedCountryId == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Area")("settings", ctx_r8.SingleSelectSettingsById)("data", ctx_r8.FuelSurchargeAreaList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.rcForm.get("FuelSurchargeAreas").invalid && ctx_r8.rcForm.get("FuelSurchargeAreas").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"](" ", ctx_r8.SelectedCountryId == 1 ? "U.S. Dollars per Gallon" : "Canada Cents per Litre ", " (Including Taxes) on ", ctx_r8.rcForm.get("IndexPriceDate").value, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsWeeklyVisible);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsWeeklyVisible && ctx_r8.rcForm.get("ApiAdjustIndexPriceDate").invalid && ctx_r8.rcForm.get("ApiAdjustIndexPriceDate").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsMonthlyVisible);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsMonthlyVisible && ctx_r8.rcForm.get("SourceMonths").invalid && ctx_r8.rcForm.get("SourceMonths").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsAnnualyVisible);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsAnnualyVisible && ctx_r8.rcForm.get("SourceAnnualy").invalid && ctx_r8.rcForm.get("SourceAnnualy").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsWeeklyVisible);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsMonthlyVisible);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.IsAnnualyVisible);
      }
    }

    function CreateFuelSurchargeComponent_div_62_div_8_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_62_div_8_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_62_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_62_div_8_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_62_div_8_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r61.rcForm.get("ManualLatestIndexPrice").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r61.rcForm.get("ManualLatestIndexPrice").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_62_div_18_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_62_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_62_div_18_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r62.rcForm.get("ManualEffectiveDate").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_62_Template(rf, ctx) {
      if (rf & 1) {
        var _r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "\xA0");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "input", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, CreateFuelSurchargeComponent_div_62_div_8_Template, 3, 2, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "p", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Effective date (from midnight UTC)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "input", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateFuelSurchargeComponent_div_62_Template_input_onDateChange_17_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r67);

          var ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r66.setManualEffectiveDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, CreateFuelSurchargeComponent_div_62_div_18_Template, 2, 1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Notes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](23, "textarea", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("ManualLatestIndexPrice").invalid && ctx_r9.rcForm.get("ManualLatestIndexPrice").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"](" ", ctx_r9.SelectedCountryId == 1 ? "U.S. Dollars per Gallon" : "Canada Cents per Litre ", " (Including Taxes) on ", ctx_r9.rcForm.get("IndexPriceDate").value, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx_r9.MinStartDate)("maxDate", ctx_r9.MaxStartDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.get("ManualEffectiveDate").invalid && ctx_r9.rcForm.get("ManualEffectiveDate").touched);
      }
    }

    function CreateFuelSurchargeComponent_div_76_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_76_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_76_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r10.rcForm.controls.FuelSurchargeTable.get("StartDate").errors.required);
      }
    }

    function CreateFuelSurchargeComponent_div_95_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid range. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_96_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_96_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_96_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_96_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_96_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r12.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r12.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_106_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_106_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_106_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_106_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_106_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r13.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r13.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_111_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_111_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_111_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_111_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_111_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r14.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r14.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_126_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_126_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_126_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_126_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_126_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r15.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r15.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_132_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_132_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_132_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateFuelSurchargeComponent_div_132_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_132_div_2_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r16.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r16.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").errors.pattern);
      }
    }

    function CreateFuelSurchargeComponent_div_136_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Please click to generate surcharge table. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelSurchargeComponent_div_139_ng_container_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "span", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11, "%");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var fst_r80 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"](" $", fst_r80.PriceRangeStartValue, " - $", fst_r80.PriceRangeEndValue, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fst_r80.FuelSurchargeStartPercentage);
      }
    }

    function CreateFuelSurchargeComponent_div_139_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "table", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "td", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Price Between");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Fuel Surcharge %");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, CreateFuelSurchargeComponent_div_139_ng_container_8_Template, 12, 3, "ng-container", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r18.rcForm.controls["GeneratedSurchargeTable"].value);
      }
    }

    function CreateFuelSurchargeComponent_div_146_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "pntr-none": a0
      };
    };

    var _c1 = function _c1(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    var CreateFuelSurchargeComponent = /*#__PURE__*/function () {
      function CreateFuelSurchargeComponent(fb, fuelsurchargeService, regionService, http, carrierService, cdr) {
        _classCallCheck(this, CreateFuelSurchargeComponent);

        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.regionService = regionService;
        this.http = http;
        this.carrierService = carrierService;
        this.cdr = cdr;
        this.DtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"](); //public isLoadingSubject: BehaviorSubject<any>;

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
        this.disableInputControls = false; //min max date

        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.MinToDate = new Date();
        this.MinFromDate = new Date();
        this.decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
        this.SelectedTerminalsAndBulkPlants = [];
      }

      _createClass(CreateFuelSurchargeComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this2 = this;

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
          this.rcForm.controls['TableTypes'].patchValue([{
            Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master,
            Name: "Master"
          }]); // default will master

          this.IsMasterSelected = true;
          this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master.toString());
          var dt = moment__WEBPACK_IMPORTED_MODULE_7__(new Date()).toDate();
          this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 20);
          this.MinFromDate.setFullYear(this.MinFromDate.getFullYear() - 20);
          this.rcForm.controls.IndexPriceDate.setValue(moment__WEBPACK_IMPORTED_MODULE_7__(dt).format('MM/DD/YYYY')); //this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment(dt).format('MM/DD/YYYY'));
          //default view type =1 so need to add required validations.

          this.AddRemoveValidations([this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate')], [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);
          this.fuelsurchargeService.onSelectedFuelSurchargeId.subscribe(function (s) {
            if (s) {
              var stringify = JSON.parse(s);
              _this2.fuelsurchargeId = stringify.FsurcharId;
              _this2.fuelsurchargeMode = stringify.Mode;
            }
          }); // with order page integration

          var id = localStorage.getItem("FuelSurchargeTabId");

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

          for (var element in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"]) {
            WeekList.push({
              Id: element,
              Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"][element],
              Code: ""
            });
          }

          this.WeekList = WeekList;
          this.rcForm.controls['Weeks'].setValue(this.WeekList.slice(0, 1));

          for (var _element in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"]) {
            MonthList.push({
              Id: _element,
              Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"][_element],
              Code: ""
            });
          }

          this.MonthList = MonthList;
          this.rcForm.controls['Months'].setValue(this.MonthList.slice(0, 1));
          this.IsWeeklyVisible = true;

          for (var i = 6; i >= -6; i--) {
            var m = new Date().setMonth(new Date().getMonth() + i, 1);
            sourceMonthList.push({
              Id: moment__WEBPACK_IMPORTED_MODULE_7__(m).format(),
              Name: moment__WEBPACK_IMPORTED_MODULE_7__(m).format('MMMM YYYY'),
              Code: ""
            });
          }

          this.SourceMonthList = sourceMonthList;
          this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.slice(5, 6));

          for (var _i = 1; _i >= -1; _i--) {
            var y = new Date().setFullYear(new Date().getFullYear() + _i, 1);
            sourceAnnualyList.push({
              Id: moment__WEBPACK_IMPORTED_MODULE_7__(y).format(),
              Name: moment__WEBPACK_IMPORTED_MODULE_7__(y).format('YYYY'),
              Code: ""
            });
          }

          this.SourceAnnualyList = sourceAnnualyList;
          this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.slice(0, 1));

          for (var _element2 in src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"]) {
            AnnualyList.push({
              Id: _element2,
              Name: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"][_element2],
              Code: ""
            });
          }

          this.AnnualyList = AnnualyList;
          this.rcForm.controls['Annualy'].setValue(this.AnnualyList.slice(0, 1));
          this.rcForm.get('FuelSurchargePeriods').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref) {
            var _ref2 = _slicedToArray(_ref, 2),
                prev = _ref2[0],
                next = _ref2[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('ApiAdjustIndexPriceDate').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref3) {
            var _ref4 = _slicedToArray(_ref3, 2),
                prev = _ref4[0],
                next = _ref4[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('WeekDay').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref5) {
            var _ref6 = _slicedToArray(_ref5, 2),
                prev = _ref6[0],
                next = _ref6[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('Weeks').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref7) {
            var _ref8 = _slicedToArray(_ref7, 2),
                prev = _ref8[0],
                next = _ref8[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('Annualy').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref9) {
            var _ref10 = _slicedToArray(_ref9, 2),
                prev = _ref10[0],
                next = _ref10[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('SourceAnnualy').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref11) {
            var _ref12 = _slicedToArray(_ref11, 2),
                prev = _ref12[0],
                next = _ref12[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('Months').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref13) {
            var _ref14 = _slicedToArray(_ref13, 2),
                prev = _ref14[0],
                next = _ref14[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          this.rcForm.get('SourceMonths').valueChanges.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["distinctUntilChanged"])()).subscribe(function (_ref15) {
            var _ref16 = _slicedToArray(_ref15, 2),
                prev = _ref16[0],
                next = _ref16[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.setValidFromDate();
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('SourceRegions').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])()).subscribe(function (_ref17) {
            var _ref18 = _slicedToArray(_ref17, 2),
                prev = _ref18[0],
                next = _ref18[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.getTerminalsBulkPlant();
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('Carriers').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])()).subscribe(function (_ref19) {
            var _ref20 = _slicedToArray(_ref19, 2),
                prev = _ref20[0],
                next = _ref20[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.onCarriersChange(prev, next);
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('Customers').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])()).subscribe(function (_ref21) {
            var _ref22 = _slicedToArray(_ref21, 2),
                prev = _ref22[0],
                next = _ref22[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.onCustomersChange(prev, next);
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(this.rcForm.get('SourceRegions').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["pairwise"])()).subscribe(function (_ref23) {
            var _ref24 = _slicedToArray(_ref23, 2),
                prev = _ref24[0],
                next = _ref24[1];

            if (JSON.stringify(prev) != JSON.stringify(next) && _this2.IsEditLoaded) _this2.SourceRegionChange(prev, next);
          });

          if (this.SelectedCountryId == 2) {
            this.changeViewType(this.SelectedCountryId);
          }
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (this.fuelsurchargeId != null && this.fuelsurchargeId != undefined) {
            //this.isLoadingSubject = new BehaviorSubject(false);
            this.IsEditLoaded = false;
            this.getFuelSurchargeTable(this.fuelsurchargeId); //existing fuel charge.
          }
        }
      }, {
        key: "getDefaultServingCountry",
        value: function getDefaultServingCountry() {
          var _this3 = this;

          this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(function (data) {
            _this3.SelectedCountryId = Number(data.DefaultCountryId);

            _this3.getFuelSurchargeProducts(_this3.SelectedCountryId);

            _this3.getFuelSurchargePeriods(_this3.SelectedCountryId);

            _this3.getFuelSurchargeArea(_this3.SelectedCountryId);
          });
        }
      }, {
        key: "modeChangeApiORmanualValidators",
        value: function modeChangeApiORmanualValidators(IsManualUpdate) {
          if (!IsManualUpdate) {
            var selectedTableType = this.rcForm.controls['TableTypes'].value;
            this.Fsmodel.TableTypeId = selectedTableType[0].Id;

            if (selectedTableType[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
              this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //, this.rcForm.get('Carriers')
            } else if (selectedTableType[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Customer) {
              this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
            } else {
              this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
            }

            this.AddRemoveValidations([this.rcForm.get('SourceRegions'), this.rcForm.get('TableTypes'), this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas')], [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);

            if (this.IsWeeklyVisible) {
              this.AddRemoveValidations([this.rcForm.get('ApiAdjustIndexPriceDate')], [this.rcForm.get('SourceAnnualy'), this.rcForm.get('SourceMonths')]);
            } else if (this.IsMonthlyVisible) {
              this.AddRemoveValidations([this.rcForm.get('SourceMonths')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceAnnualy')]);
            } else if (this.IsAnnualyVisible) {
              this.AddRemoveValidations([this.rcForm.get('SourceAnnualy')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths')]);
            }
          }

          if (IsManualUpdate) {
            this.rcForm.get('ManualLatestIndexPrice').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].pattern(this.decimalSupportedRegx)]);
            this.rcForm.get('ManualLatestIndexPrice').updateValueAndValidity();
            this.AddRemoveValidations([this.rcForm.get('ManualEffectiveDate')], [this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths'), this.rcForm.get('SourceAnnualy')]);
          }
        }
      }, {
        key: "modeChangePublishORdraftValidators",
        value: function modeChangePublishORdraftValidators(statusId) {
          this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode

          if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Draft) {
            this.clearAllValidations(this.rcForm); // clear all validation

            this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for draft 
          } else if (statusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published) {
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
      }, {
        key: "changeViewType",
        value: function changeViewType(value) {
          this.viewType = value;
          this.rcForm.get('IsManualUpdate').setValue(value == 1 ? false : true);
          this.modeChangeApiORmanualValidators(value == 1 ? false : true);
        }
      }, {
        key: "createForm",
        value: function createForm() {
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
            GeneratedSurchargeTable: new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArray"]([new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"](this.Fsmodel.GeneratedSurchargeTable)])
          }, {
            validator: RangeValidator
          });
        } //#region start : calander related functionality 

      }, {
        key: "setApiAdjustIndexPriceDate",
        value: function setApiAdjustIndexPriceDate(event) {
          this.IsMonthlyVisible = false;
          this.IsWeeklyVisible = false;
          this.IsAnnualyVisible = false;
          if (event != "") this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(event);

          if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != "" && this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {
            var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;

            if (selectedPeriod != null && selectedPeriod != undefined && selectedPeriod.length > 0) {
              if (selectedPeriod[0].Name.toLowerCase() == "Weekly".toLowerCase()) {
                this.IsWeeklyVisible = true;
              } else if (selectedPeriod[0].Name.toLowerCase() == "Monthly".toLowerCase()) {
                this.IsMonthlyVisible = true;
              } else if (selectedPeriod[0].Name.toLowerCase() == "Annualy".toLowerCase()) {
                this.IsAnnualyVisible = true;
              }

              this.setValidFromDate();
            }
          }
        }
      }, {
        key: "onFuelSurchargePeriodsSelect",
        value: function onFuelSurchargePeriodsSelect(item) {
          this.IsMonthlyVisible = false;
          this.IsWeeklyVisible = false;
          this.IsAnnualyVisible = false; //
          // if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {

          var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;

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
          } //}

        }
      }, {
        key: "setManualEffectiveDate",
        value: function setManualEffectiveDate(event) {
          if (event != "") {
            this.rcForm.controls.ManualEffectiveDate.setValue(event);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(event).format('MM/DD/YYYY'));
          }
        }
      }, {
        key: "setStartDate",
        value: function setStartDate(event) {
          this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(event);
          if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != "") this.MinToDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
        }
      }, {
        key: "setEndDate",
        value: function setEndDate(event) {
          this.rcForm.controls.FuelSurchargeTable.get('EndDate').setValue(event);
        } // end : calander related functionality

      }, {
        key: "onTableTypeDeSelect",
        value: function onTableTypeDeSelect(item) {
          var selectedTableType = this.rcForm.get('TableTypes').value;

          if (selectedTableType.length == 0) {
            this.IsMasterSelected = true;
            this.rcForm.get('Carriers').patchValue([]);
            this.rcForm.get('Customers').patchValue([]);
            this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
            this.rcForm.get('SourceRegions').patchValue([]);
            this.IsSourceRegionSelected = false;
            this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]);
          }
        }
      }, {
        key: "onTableTypeSelect",
        value: function onTableTypeSelect(item) {
          this.IsMasterSelected = false;
          this.IsCustomerSelected = false;
          this.IsCarrierSelected = false;
          this.rcForm.get('Carriers').patchValue([]);
          this.rcForm.get('Customers').patchValue([]);

          switch (item.Id) {
            case 1:
              //master
              this.IsMasterSelected = true;
              this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"

              break;

            case 2:
              // customer
              this.getCarriers();
              this.getSupplierCustomers();
              this.IsCustomerSelected = true;
              this.AddRemoveValidations([this.rcForm.get('Customers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Carriers')]);
              break;

            case 3:
              //carrier
              this.getCarriers();
              this.getSupplierCustomers();
              this.IsCarrierSelected = true;
              this.AddRemoveValidations([this.rcForm.get('Carriers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Customers')]);
              break;
          }

          this.rcForm.get('SourceRegions').patchValue([]);
          this.getSourceRegions(item.Id);
        }
      }, {
        key: "AddRemoveValidations",
        value: function AddRemoveValidations(requiredControls, notRequiredControls) {
          if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {
            requiredControls.forEach(function (ctrl) {
              ctrl.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_5__["Validators"].required]);
              ctrl.updateValueAndValidity();
            });
          }

          if (notRequiredControls != null && notRequiredControls != undefined && notRequiredControls.length > 0) {
            notRequiredControls.forEach(function (ctrl) {
              ctrl.clearValidators();
              ctrl.updateValueAndValidity();
            });
          }
        }
      }, {
        key: "onCarriersChange",
        value: function onCarriersChange(prev, next) {
          this.onCarriersOrCustomersChange(prev, next);
        }
      }, {
        key: "onCustomersChange",
        value: function onCustomersChange(prev, next) {
          this.onCarriersOrCustomersChange(prev, next);
        }
      }, {
        key: "onCarriersOrCustomersChange",
        value: function onCarriersOrCustomersChange(prev, next) {
          if (this.IsMasterSelected) return;
          this.rcForm.get('SourceRegions').patchValue([]);
          var selectedTableType = this.rcForm.get('TableTypes').value;
          this.getSourceRegions(selectedTableType[0].Id.toString());
        }
      }, {
        key: "onFetchLastIndexPrice",
        value: function onFetchLastIndexPrice() {
          var selectedArea = this.rcForm.get('FuelSurchargeAreas').value;
          var selectedProduct = this.rcForm.get('FuelSurchargeProducts').value;
          var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value; //if (selectedProduct != undefined && selectedPeriod != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1)
          //    this.getFuelIndexPrice(selectedPeriod[0].Code, selectedProduct[0].Code, moment().format("MM-DD-YYYY"));

          if (selectedArea != undefined && selectedProduct != undefined && selectedPeriod != undefined && selectedArea != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1 && selectedArea.length == 1) {
            var selectedAreaCode = this.FuelSurchargeAreaList.filter(function (x) {
              return x.Id == selectedArea[0].Id;
            })[0].Code;
            var selectedPeriodCode = this.FuelSurchargePeriodList.filter(function (x) {
              return x.Id == selectedPeriod[0].Id;
            })[0].Code;
            this.getFuelIndexPrice(selectedPeriodCode, selectedProduct[0].Code, moment__WEBPACK_IMPORTED_MODULE_7__().format("MM-DD-YYYY"), selectedAreaCode);
          }
        }
      }, {
        key: "onGenerateSurchargeTable",
        value: function onGenerateSurchargeTable() {
          var _this4 = this;

          this.ShowMessage = false;
          this.IsGeneratedSurchargeTable = false;
          var gst = this.rcForm.controls['GeneratedSurchargeTable'];
          var fst = this.rcForm.controls['FuelSurchargeTable'];
          gst.clear();
          this.modeChangePublishORdraftValidators(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published);
          this.modeChangeApiORmanualValidators(this.rcForm.get("IsManualUpdate").value);
          this.markFormGroupTouched(this.rcForm);
          if (!fst.valid) return;
          this.IsLoading = true;
          this.fuelsurchargeService.getGenerateSurchargeTable(fst.get('PriceRangeStartValue').value, fst.get('PriceRangeEndValue').value, fst.get('PriceRangeInterval').value, fst.get('SurchargeInterval').value, fst.get('FuelSurchargeStartPercentage').value).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this4, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
              var dt;
              return regeneratorRuntime.wrap(function _callee$(_context) {
                while (1) {
                  switch (_context.prev = _context.next) {
                    case 0:
                      _context.next = 2;
                      return data;

                    case 2:
                      dt = _context.sent;
                      dt.forEach(function (res) {
                        gst.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]({
                          FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                          PriceRangeStartValue: res.PriceRangeStartValue,
                          PriceRangeEndValue: res.PriceRangeEndValue
                        }));
                      });
                      this.DtTrigger.next();
                      this.IsLoading = false;

                    case 6:
                    case "end":
                      return _context.stop();
                  }
                }
              }, _callee, this);
            }));
          });
          this.IsGeneratedSurchargeTable = true; //console.log(this.rcForm.controls['GeneratedSurchargeTable'].value)
        }
      }, {
        key: "markFormGroupTouched",
        value: function markFormGroupTouched(formGroup) {
          var _this5 = this;

          Object.values(formGroup.controls).forEach(function (control) {
            control.markAsTouched();

            if (control.controls) {
              _this5.markFormGroupTouched(control);
            }
          });
        }
      }, {
        key: "clearAllControlValue",
        value: function clearAllControlValue(formGroup) {
          var _this6 = this;

          Object.values(formGroup.controls).forEach(function (control) {
            control.patchValue([]);
            ;

            if (control.controls) {
              _this6.clearAllControlValue(control);
            }
          });
        }
      }, {
        key: "clearAllValidations",
        value: function clearAllValidations(formGroup) {
          var _this7 = this;

          Object.values(formGroup.controls).forEach(function (control) {
            control.clearValidators();
            control.updateValueAndValidity();
            control.markAsTouched();

            if (control.controls) {
              _this7.clearAllValidations(control);
            }
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit(fuelSurchageStatus) {
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
      }, {
        key: "clearForm",
        value: function clearForm() {
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
      }, {
        key: "onCancel",
        value: function onCancel() {
          if (this.fuelsurchargeMode != null) {
            this.changeToViewTab();
          } else {
            this.clearAllControlValue(this.rcForm);
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(function (x) {
              return x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master;
            })); // default will master

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
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.DtTrigger.unsubscribe();
        } //#region webserivce call

      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this8 = this;

          this.IsLoading = true;
          this.regionService.getCarriers().subscribe(function (carriers) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this8, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
              return regeneratorRuntime.wrap(function _callee2$(_context2) {
                while (1) {
                  switch (_context2.prev = _context2.next) {
                    case 0:
                      _context2.next = 2;
                      return carriers;

                    case 2:
                      this.CarrierList = _context2.sent;
                      this.DtTrigger.next();
                      this.IsLoading = false;

                    case 5:
                    case "end":
                      return _context2.stop();
                  }
                }
              }, _callee2, this);
            }));
          });
        }
      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          var _this9 = this;

          this.fuelsurchargeService.getTableTypes().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this9, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
              return regeneratorRuntime.wrap(function _callee3$(_context3) {
                while (1) {
                  switch (_context3.prev = _context3.next) {
                    case 0:
                      _context3.next = 2;
                      return data;

                    case 2:
                      this.TableTypeList = _context3.sent;
                      this.DtTrigger.next();

                    case 4:
                    case "end":
                      return _context3.stop();
                  }
                }
              }, _callee3, this);
            }));
          });
        }
      }, {
        key: "getSupplierCustomers",
        value: function getSupplierCustomers() {
          var _this10 = this;

          this.IsLoading = true;
          this.fuelsurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this10, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
              return regeneratorRuntime.wrap(function _callee4$(_context4) {
                while (1) {
                  switch (_context4.prev = _context4.next) {
                    case 0:
                      _context4.next = 2;
                      return data;

                    case 2:
                      this.CustomerList = _context4.sent;
                      this.DtTrigger.next();
                      this.IsLoading = false;

                    case 5:
                    case "end":
                      return _context4.stop();
                  }
                }
              }, _callee4, this);
            }));
          });
        } //companyIds : Based on tableType we will be call API , if tableType master or customer or carrier, full source region,customers,carriers loads will populated.

      }, {
        key: "getSourceRegions",
        value: function getSourceRegions(tableType) {
          var _this11 = this;

          var customerIds = [];
          var carrierIds = [];
          var selectedCustomers = this.rcForm.get('Customers').value;

          if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(function (s) {
              return s.Id;
            });
          }

          var selectedCarriers = this.rcForm.get('Carriers').value;

          if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(function (s) {
              return s.Id;
            });
          }

          var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SourceRegionInputModel"]();
          sourceRegionInput.TableType = tableType;
          sourceRegionInput.CustomerId = customerIds;
          sourceRegionInput.CarrierId = carrierIds;
          this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this11, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
              return regeneratorRuntime.wrap(function _callee5$(_context5) {
                while (1) {
                  switch (_context5.prev = _context5.next) {
                    case 0:
                      _context5.next = 2;
                      return data;

                    case 2:
                      this.SourceRegionList = _context5.sent;
                      this.SourceRegionList.sort(function (a, b) {
                        return a.Name > b.Name ? 1 : -1;
                      });
                      this.DtTrigger.next();

                    case 5:
                    case "end":
                      return _context5.stop();
                  }
                }
              }, _callee5, this);
            }));
          });
        }
      }, {
        key: "getTerminalsBulkPlant",
        value: function getTerminalsBulkPlant() {
          var _this12 = this;

          this.IsLoading = true;
          var selectedSourceRegions = this.rcForm.get('SourceRegions').value;
          this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
          this.IsSourceRegionSelected = false;

          if (selectedSourceRegions != undefined && selectedSourceRegions != null && selectedSourceRegions.length > 0) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(function (s) {
              return s.Id;
            }).join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this12, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
                return regeneratorRuntime.wrap(function _callee6$(_context6) {
                  while (1) {
                    switch (_context6.prev = _context6.next) {
                      case 0:
                        _context6.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context6.sent;
                        this.DtTrigger.next();
                        this.IsLoading = false;

                      case 5:
                      case "end":
                        return _context6.stop();
                    }
                  }
                }, _callee6, this);
              }));
            });
          } else {
            this.IsLoading = false;
          }
        }
      }, {
        key: "SourceRegionChange",
        value: function SourceRegionChange(prev, next) {
          var _this13 = this;

          if (prev == null && next.length == 0) return;
          this.IsLoading = true;
          this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
          this.IsSourceRegionSelected = false;
          var ids = [];
          var selectedSourceRegions = this.rcForm.get('SourceRegions').value;

          if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(function (s) {
              return ids.push(s.Id);
            });
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this13, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee7() {
                return regeneratorRuntime.wrap(function _callee7$(_context7) {
                  while (1) {
                    switch (_context7.prev = _context7.next) {
                      case 0:
                        _context7.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context7.sent;
                        this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                        this.IsSourceRegionSelected = true;
                        this.DtTrigger.next();

                      case 6:
                      case "end":
                        return _context7.stop();
                    }
                  }
                }, _callee7, this);
              }));
            });
          }

          this.IsLoading = false;
        }
      }, {
        key: "getFuelSurchargeProducts",
        value: function getFuelSurchargeProducts(countryId) {
          var _this14 = this;

          this.fuelsurchargeService.getFuelSurchargeProducts(countryId).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this14, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee8() {
              return regeneratorRuntime.wrap(function _callee8$(_context8) {
                while (1) {
                  switch (_context8.prev = _context8.next) {
                    case 0:
                      _context8.next = 2;
                      return data;

                    case 2:
                      this.FuelSurchargeProductList = _context8.sent;
                      this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.slice(0, 1));
                      this.DtTrigger.next();

                    case 5:
                    case "end":
                      return _context8.stop();
                  }
                }
              }, _callee8, this);
            }));
          });
        }
      }, {
        key: "getFuelSurchargePeriods",
        value: function getFuelSurchargePeriods(countryId) {
          var _this15 = this;

          this.fuelsurchargeService.getFuelSurchargePeriod(countryId).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this15, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee9() {
              return regeneratorRuntime.wrap(function _callee9$(_context9) {
                while (1) {
                  switch (_context9.prev = _context9.next) {
                    case 0:
                      _context9.next = 2;
                      return data;

                    case 2:
                      this.FuelSurchargePeriodList = _context9.sent;
                      this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.slice(0, 1)); //var dt = moment(new Date()).toDate();
                      //this.setApiAdjustIndexPriceDate(moment(dt).format('MM/DD/YYYY'));

                      this.DtTrigger.next();

                    case 5:
                    case "end":
                      return _context9.stop();
                  }
                }
              }, _callee9, this);
            }));
          });
        }
      }, {
        key: "getFuelSurchargeArea",
        value: function getFuelSurchargeArea(countryId) {
          var _this16 = this;

          ;
          this.fuelsurchargeService.getFuelSurchargeArea(countryId).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this16, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee10() {
              return regeneratorRuntime.wrap(function _callee10$(_context10) {
                while (1) {
                  switch (_context10.prev = _context10.next) {
                    case 0:
                      _context10.next = 2;
                      return data;

                    case 2:
                      this.FuelSurchargeAreaList = _context10.sent;
                      this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.slice(0, 1));
                      this.DtTrigger.next();

                    case 5:
                    case "end":
                      return _context10.stop();
                  }
                }
              }, _callee10, this);
            }));
          });
        }
      }, {
        key: "getFuelIndexPrice",
        value: function getFuelIndexPrice(periodId, productType, fetchDate, areaId) {
          var _this17 = this;

          this.IsLoading = true;

          if (this.SelectedCountryId == 1) {
            this.fuelsurchargeService.getEIAIndexPrice(periodId, productType, fetchDate, areaId).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this17, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee11() {
                return regeneratorRuntime.wrap(function _callee11$(_context11) {
                  while (1) {
                    switch (_context11.prev = _context11.next) {
                      case 0:
                        _context11.next = 2;
                        return data;

                      case 2:
                        this.APILatestIndexPrice = _context11.sent;
                        this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                        this.DtTrigger.next();
                        this.IsLoading = false;

                      case 6:
                      case "end":
                        return _context11.stop();
                    }
                  }
                }, _callee11, this);
              }));
            });
          } else {
            this.fuelsurchargeService.getNRCIndexPrice(periodId, productType, fetchDate).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this17, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee12() {
                return regeneratorRuntime.wrap(function _callee12$(_context12) {
                  while (1) {
                    switch (_context12.prev = _context12.next) {
                      case 0:
                        _context12.next = 2;
                        return data;

                      case 2:
                        this.APILatestIndexPrice = _context12.sent;
                        this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                        this.DtTrigger.next();
                        this.IsLoading = false;

                      case 6:
                      case "end":
                        return _context12.stop();
                    }
                  }
                }, _callee12, this);
              }));
            });
          }
        }
      }, {
        key: "IdInComparer",
        value: function IdInComparer(otherArray) {
          return function (current) {
            return otherArray.filter(function (other) {
              return other.Id == current.Id;
            }).length == 1;
          };
        } //GET

      }, {
        key: "getFuelSurchargeTable",
        value: function getFuelSurchargeTable(fuelSurchargeTableId) {
          var _this18 = this;

          //this.isLoadingSubject.next(true);;
          var sorceRegionIds = "";
          this.IsLoading = true;
          this.cdr.detectChanges();
          this.http.get(this.fuelsurchargeService.getFuelSurchargeTableUrl + fuelSurchargeTableId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["map"])(function (fsIndex) {
            var fsModel = fsIndex;
            return fsModel;
          }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["mergeMap"])(function (fsModel) {
            //this.isLoadingSubject.next(true);
            _this18.Fsmodel = fsModel; //let companyIds: number[] = [];

            if (_this18.fuelsurchargeId != null && _this18.fuelsurchargeMode.toUpperCase() == "COPY") {
              // on copy 
              _this18.Fsmodel.FuelSurchargeIndexId = null;
              _this18.Fsmodel.TableName = "";
            }

            var customers = _this18.http.get(_this18.fuelsurchargeService.getSupplierCustomersUrl);

            var carriers = _this18.http.get(_this18.regionService.getCarriersUrl);

            var customerIds = [];
            var carrierIds = [];

            if (_this18.Fsmodel.Customers.length > 0) {
              customerIds = _this18.Fsmodel.Customers.map(function (s) {
                return s.Id;
              });
            }

            if (_this18.Fsmodel.Carriers.length > 0) {
              carrierIds = _this18.Fsmodel.Carriers.map(function (s) {
                return s.Id;
              });
            }

            var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SourceRegionInputModel"]();
            sourceRegionInput.TableType = _this18.Fsmodel.TableTypeId.toString();
            sourceRegionInput.CustomerId = customerIds;
            sourceRegionInput.CarrierId = carrierIds;

            var sourceRegions = _this18.http.post(_this18.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);

            var tableTypes = _this18.http.get(_this18.fuelsurchargeService.getTableTypesUrl);

            if (_this18.Fsmodel.SourceRegions != null && _this18.Fsmodel.SourceRegions != undefined && _this18.Fsmodel.SourceRegions.length > 0) {
              sorceRegionIds = _this18.Fsmodel.SourceRegions.map(function (s) {
                return s.Id;
              }).join(',');
              _this18.IsSourceRegionSelected = true;
            }

            var terminalAndBulkPlans = _this18.http.get(_this18.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds);

            return Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["forkJoin"])([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
          })).subscribe(function (result) {
            _this18.IsLoading = false; //this.isLoadingSubject.next(true);

            _this18.IsMasterSelected = false;
            _this18.IsCustomerSelected = false;
            _this18.IsCarrierSelected = false;

            if (_this18.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
              _this18.IsMasterSelected = true;
            } else if (_this18.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Customer) {
              _this18.IsCustomerSelected = true;
            } else if (_this18.Fsmodel.TableTypeId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Carrier) {
              _this18.IsCarrierSelected = true;
            }

            if (_this18.Fsmodel.TableTypeId != src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TableType"].Master) {
              _this18.CustomerList = result[0];
              _this18.CarrierList = result[1];
            }

            _this18.SourceRegionList = result[2];

            if (_this18.Fsmodel.SourceRegions != null && _this18.Fsmodel.SourceRegions != undefined && _this18.Fsmodel.SourceRegions.length > 0) {
              _this18.TerminalsAndBulkPlantList = result[3];
            }

            _this18.TableTypeList = result[4]; //this.isLoadingSubject.next(true);

            _this18.Edit(_this18.Fsmodel);
          });
        } //Edit

      }, {
        key: "Edit",
        value: function Edit(_fs) {
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
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(function (x) {
              return x.Id == _fs.TableTypeId;
            }));
            _fs.IsManualUpdate ? this.changeViewType("2") : this.changeViewType("1");
            var stringify = JSON.parse(_fs.ApiEffectiveDate);

            if (!_fs.IsManualUpdate && stringify != null && stringify != undefined && stringify != "") {
              this.IsLoading = true;

              if (stringify.WeekDay != null && stringify.WeekDay != undefined && stringify.WeekDay != "") {
                this.rcForm.controls['WeekDay'].setValue(stringify.WeekDay);
                this.IsWeeklyVisible = true;
              }

              if (stringify.Weeks != null && stringify.Weeks != undefined && stringify.Weeks != "") {
                var weeks = [];
                weeks.push({
                  Id: JSON.parse(stringify.Weeks).Id,
                  Name: JSON.parse(stringify.Weeks).Name,
                  Code: ""
                });
                this.rcForm.controls['Weeks'].setValue(weeks);
                this.IsWeeklyVisible = true;
              }

              if (stringify.Months != null && stringify.Months != undefined && stringify.Months != "") {
                var months = [];
                months.push({
                  Id: JSON.parse(stringify.Months).Id,
                  Name: JSON.parse(stringify.Months).Name,
                  Code: ""
                });
                this.rcForm.controls['Months'].setValue(months);
                this.IsMonthlyVisible = true;
              }

              if (stringify.Annualy != null && stringify.Annualy != undefined && stringify.Annualy != "") {
                var annualy = [];
                annualy.push({
                  Id: JSON.parse(stringify.Annualy).Id,
                  Name: JSON.parse(stringify.Annualy).Name,
                  Code: ""
                });
                this.rcForm.controls['Annualy'].setValue(annualy);
                this.IsAnnualyVisible = true;
              }
            } //this.isLoadingSubject.next(true);


            this.IsLoading = true;

            if (_fs.Customers != null && this.CustomerList != undefined && this.CustomerList != null) {
              if (this.CustomerList.length > 0 && _fs.Customers.length > 0) this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_fs.Customers)));
            }

            if (_fs.Carriers != null && this.CarrierList != undefined && this.CarrierList != null) {
              if (this.CarrierList.length > 0 && _fs.Carriers.length > 0) this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_fs.Carriers)));
            }

            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _fs.SourceRegions != null && _fs.SourceRegions != undefined && _fs.SourceRegions.length > 0) {
              if (this.SourceRegionList.length > 0 && _fs.SourceRegions.length > 0) this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_fs.SourceRegions)));
            }

            if (this.TerminalsAndBulkPlantList != null && this.TerminalsAndBulkPlantList != undefined && _fs.TerminalsAndBulkPlants != null && _fs.TerminalsAndBulkPlants != undefined && _fs.TerminalsAndBulkPlants.length > 0) {
              if (this.TerminalsAndBulkPlantList.length > 0 && _fs.TerminalsAndBulkPlants.length > 0) {
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList.filter(this.IdInComparer(_fs.TerminalsAndBulkPlants)));
              }
            }

            if (_fs.ProductId != null && this.FuelSurchargeProductList != null && this.FuelSurchargeProductList != undefined && this.FuelSurchargeProductList.length > 0) {
              this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.filter(function (x) {
                return x.Id == _fs.ProductId.toString();
              }));
            }

            if (_fs.PeriodId != null && this.FuelSurchargePeriodList != null && this.FuelSurchargePeriodList != undefined && this.FuelSurchargePeriodList.length > 0) {
              this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.filter(function (x) {
                return x.Id == _fs.PeriodId.toString();
              }));
            }

            if (_fs.AreaId != null && this.FuelSurchargeAreaList != null && this.FuelSurchargeAreaList != undefined && this.FuelSurchargeAreaList.length > 0) {
              this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.filter(function (x) {
                return x.Id == _fs.AreaId.toString();
              }));
            }

            this.rcForm.controls['APILatestIndexPrice'].setValue(_fs.APILatestIndexPrice);

            if (_fs.ApiAdjustIndexPriceDate != null && _fs.ApiAdjustIndexPriceDate != undefined) {
              if (this.IsWeeklyVisible) {
                this.rcForm.controls['ApiAdjustIndexPriceDate'].setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('MM/DD/YYYY'));
              } else if (this.IsMonthlyVisible) {
                this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.filter(function (x) {
                  return x.Name == moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('MMMM YYYY');
                }));
              } else if (this.IsAnnualyVisible) {
                this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.filter(function (x) {
                  return x.Name == moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ApiAdjustIndexPriceDate).format('YYYY');
                }));
              }
            }

            this.rcForm.controls['ManualLatestIndexPrice'].setValue(_fs.ManualLatestIndexPrice);

            if (_fs.ManualEffectiveDate != null && _fs.ManualEffectiveDate != undefined) {
              this.rcForm.controls['ManualEffectiveDate'].setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.ManualEffectiveDate).format('MM/DD/YYYY'));
            } else {
              this.rcForm.get('ManualEffectiveDate').patchValue([]);
            }

            this.rcForm.controls['StatusId'].setValue(_fs.StatusId);

            if (_fs.FuelSurchargeTable != null && _fs.FuelSurchargeTable != undefined) {
              if (_fs.FuelSurchargeTable.StartDate != null && _fs.FuelSurchargeTable.StartDate != undefined) {
                this.rcForm.controls['FuelSurchargeTable'].get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.FuelSurchargeTable.StartDate).format('MM/DD/YYYY'));
              } else {
                this.rcForm.controls['FuelSurchargeTable'].get('StartDate').patchValue([]);
              }

              if (_fs.FuelSurchargeTable.EndDate != null && _fs.FuelSurchargeTable.EndDate != undefined) {
                this.rcForm.controls['FuelSurchargeTable'].get('EndDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(_fs.FuelSurchargeTable.EndDate).format('MM/DD/YYYY'));
              } else {
                this.rcForm.controls['FuelSurchargeTable'].get('EndDate').patchValue([]);
              }

              this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeStartValue').setValue(_fs.FuelSurchargeTable.PriceRangeStartValue);
              this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeEndValue').setValue(_fs.FuelSurchargeTable.PriceRangeEndValue);
              this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeInterval').setValue(_fs.FuelSurchargeTable.PriceRangeInterval);
              this.rcForm.controls['FuelSurchargeTable'].get('FuelSurchargeStartPercentage').setValue(_fs.FuelSurchargeTable.FuelSurchargeStartPercentage);
              this.rcForm.controls['FuelSurchargeTable'].get('SurchargeInterval').setValue(_fs.FuelSurchargeTable.SurchargeInterval);
            } //this.isLoadingSubject.next(true);


            this.IsLoading = true;
            var gst = this.rcForm.controls['GeneratedSurchargeTable'];

            if (_fs.GeneratedSurchargeTable != null && _fs.GeneratedSurchargeTable.length > 0) {
              gst.clear();

              _fs.GeneratedSurchargeTable.forEach(function (res) {
                gst.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControl"]({
                  FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                  PriceRangeStartValue: res.PriceRangeStartValue,
                  PriceRangeEndValue: res.PriceRangeEndValue
                }));
              });
            }

            this.onGenerateSurchargeTable();
            this.IsEditLoaded = true; //this.isLoadingSubject.next(false);

            this.IsLoading = false;
          }

          if (this.fuelsurchargeMode == "VIEW") {
            this.disableInputControls = true;
          } else {
            this.disableInputControls = false;
          }

          this.AllowTableName = false;
        }
      }, {
        key: "nextDate",
        value: function nextDate(givenDate, dayIndex) {
          var today = new Date(givenDate); //var a = today.getDate();
          //var e = today.getDay();
          //var b = (dayIndex - 1 - today.getDay() + 7);
          //var c = b % 7;
          //var d = today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1;
          //don't find next date in case in edage case . like on same day and having fine arrangement not to crash when more than 1 week, and arrangment is to first add required days and than call current method.

          if (dayIndex != today.getDay()) today.setDate(today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1);
          return today;
        }
      }, {
        key: "setValidFromDate",
        value: function setValidFromDate() {
          this.IsLoading = true;
          var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value;
          var selectedSourceMonth = this.rcForm.get('SourceMonths').value;
          var selectedSourceAnnualy = this.rcForm.get('SourceAnnualy').value;
          var ApiAdjustIndexPriceDate = this.rcForm.controls.ApiAdjustIndexPriceDate.value; //if (!this.IsWeeklyVisible) this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(null);
          // this.AddRemoveValidations(null,[this.rcForm.get('Weeks'), this.rcForm.get('Months'), this.rcForm.get('Annualy')]);

          if (selectedPeriod != null && selectedPeriod != undefined && selectedPeriod.length > 0) {
            var effectiveDate = new Date();

            if (ApiAdjustIndexPriceDate != null && ApiAdjustIndexPriceDate != undefined && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].WEEKLY) {
              this.AddRemoveValidations([this.rcForm.get('Weeks')], [this.rcForm.get('Months'), this.rcForm.get('Annualy')]);
              effectiveDate = this.rcForm.controls.ApiAdjustIndexPriceDate.value;
              effectiveDate = new Date(effectiveDate);
              var WeekDay = this.IsWeeklyVisible && this.rcForm.controls.WeekDay.value != null && this.rcForm.controls.WeekDay.value != undefined && this.rcForm.controls.WeekDay.value != "" ? this.rcForm.controls.WeekDay.value : "";
              var Weeks = this.IsWeeklyVisible && this.rcForm.controls.Weeks.value != null && this.rcForm.controls.Weeks.value != undefined && this.rcForm.controls.Weeks.value != "" ? this.rcForm.controls.Weeks.value : "";
              var selectedWeeks = this.rcForm.get('Weeks').value;

              if (selectedWeeks != null && selectedWeeks != undefined && selectedWeeks.length > 0) {
                if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Same_Week) {// default will handle
                } else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_1) {
                  effectiveDate.setDate(effectiveDate.getDate() + 7);
                } else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_2) {
                  effectiveDate.setDate(effectiveDate.getDate() + 14);
                } else if (selectedWeeks[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekEnum"].Week_Later_3) {
                  effectiveDate.setDate(effectiveDate.getDate() + 21);
                }

                if (WeekDay != "" && Weeks != "") {
                  switch (WeekDay.toUpperCase()) {
                    case "SUN":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Sun);
                        break;
                      }

                    case "MON":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Mon);
                        break;
                      }

                    case "TUE":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Tue);
                        break;
                      }

                    case "WED":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Wed);
                        break;
                      }

                    case "THU":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Thu);
                        break;
                      }

                    case "FRI":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Fri);
                        break;
                      }

                    case "SAT":
                      {
                        effectiveDate = this.nextDate(effectiveDate, src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["WeekDays"].Sat);
                        break;
                      }

                    default:
                      {
                        //statements; 
                        break;
                      }
                  }

                  this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                  this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                  this.IsWeeklyVisible = true;
                }
              }
            } else if (selectedSourceMonth != null && selectedSourceMonth != undefined && selectedSourceMonth.length > 0 && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].MONTHLY) {
              var selectedSourceMonth = this.rcForm.get('SourceMonths').value;
              this.AddRemoveValidations([this.rcForm.get('Months')], [this.rcForm.get('Weeks'), this.rcForm.get('Annualy')]);
              effectiveDate = new Date(selectedSourceMonth[0].Id);
              var selectedMonths = this.rcForm.get('Months').value;

              if (selectedMonths != null && selectedMonths != undefined && selectedMonths.length > 0) {
                if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_1) {
                  effectiveDate.setMonth(effectiveDate.getMonth() + 1, 1);
                } else if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_2) {
                  effectiveDate.setMonth(effectiveDate.getMonth() + 2, 1);
                } else if (selectedMonths[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["MonthEnum"].Month_Later_3) {
                  effectiveDate.setMonth(effectiveDate.getMonth() + 3, 1);
                }

                this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                this.IsMonthlyVisible = true;
              }
            } else if (selectedSourceAnnualy != null && selectedSourceAnnualy != undefined && selectedSourceAnnualy.length > 0 && selectedPeriod[0].Name.toUpperCase() == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["PeriodEnum"].ANNUALY) {
              effectiveDate = new Date(selectedSourceAnnualy[0].Id);
              this.AddRemoveValidations([this.rcForm.get('Annualy')], [this.rcForm.get('Weeks'), this.rcForm.get('Months')]);
              var selectedYear = this.rcForm.get('Annualy').value;

              if (selectedYear != null && selectedYear != undefined && selectedYear.length > 0) {
                if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_1) {
                  effectiveDate.setFullYear(effectiveDate.getFullYear() + 1, 0, 1);
                } else if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_2) {
                  effectiveDate.setFullYear(effectiveDate.getFullYear() + 2, 0, 1);
                } else if (selectedYear[0].Name == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["AnnualyEnum"].Year_Later_3) {
                  effectiveDate.setFullYear(effectiveDate.getFullYear() + 3, 0, 1);
                }

                this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).format('MM/DD/YYYY'));
                this.MinToDate = moment__WEBPACK_IMPORTED_MODULE_7__(effectiveDate).toDate();
                this.IsAnnualyVisible = true;
              }
            }
          }

          this.IsLoading = false;
        } //Save

      }, {
        key: "Save",
        value: function Save(fuelSurchageStatus) {
          var _this19 = this;

          this.IsLoading = true;
          this.Fsmodel = this.rcForm.value;
          if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate.value != undefined && this.rcForm.controls.ApiAdjustIndexPriceDate.value != "") this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(this.rcForm.controls.ApiAdjustIndexPriceDate.value).format());
          if (this.rcForm.controls.ManualEffectiveDate.value != null && this.rcForm.controls.ManualEffectiveDate.value != undefined) this.Fsmodel.ManualEffectiveDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(this.rcForm.controls.ManualEffectiveDate.value).format());
          if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined) this.Fsmodel.FuelSurchargeTable.StartDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
          if (this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != undefined) this.Fsmodel.FuelSurchargeTable.EndDate = this.rcForm.controls.FuelSurchargeTable.get('EndDate').value;
          if (this.Fsmodel.TerminalsAndBulkPlants != null || this.Fsmodel.TerminalsAndBulkPlants != undefined) this.Fsmodel.TerminalsAndBulkPlants.forEach(function (res) {
            res.Code = _this19.TerminalsAndBulkPlantList.find(function (c) {
              return c.Id == res.Id && c.Name == res.Name;
            }).Code;
          });

          if (this.IsMonthlyVisible) {
            var sourceMonths = this.rcForm.get('SourceMonths').value;

            if (sourceMonths != null && sourceMonths.length > 0) {
              this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(sourceMonths[0].Id).format());
            } else {
              this.Fsmodel.ApiAdjustIndexPriceDate = null;
            }
          }

          if (this.IsAnnualyVisible) {
            var sourceAnnualy = this.rcForm.get('SourceAnnualy').value;

            if (sourceAnnualy != null && sourceAnnualy.length > 0) {
              this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment__WEBPACK_IMPORTED_MODULE_7__(sourceAnnualy[0].Id).format());
            } else {
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
            var ApiEffectiveDate = {
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
          if (!this.IsGeneratedSurchargeTable) this.Fsmodel.GeneratedSurchargeTable = null;
          this.fuelsurchargeService.createFuelSurcharge(this.Fsmodel).subscribe(function (response) {
            _this19.ServiceResponse = response;

            if (response != null && response.StatusCode == 0) {
              var message = "";

              if (_this19.Fsmodel.FuelSurchargeIndexId != null) {
                message = "Fuel Surcharge table edit successfully.";
              } else if (_this19.Fsmodel.StatusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Published) {
                message = "Fuel Surcharge table created successfully.";
              } else if (_this19.Fsmodel.StatusId == src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["FreightTableStatus"].Draft) {
                message = "Fuel Surcharge saved draft successfully.";
              }

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(message, undefined, undefined);
              _this19.IsLoading = false;

              _this19.changeToViewTab();
            } else {
              _this19.IsLoading = false;
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "changeToViewTab",
        value: function changeToViewTab() {
          this.fuelsurchargeService.onSelectedTabChanged.next(2);
        }
      }]);

      return CreateFuelSurchargeComponent;
    }();

    CreateFuelSurchargeComponent.ɵfac = function CreateFuelSurchargeComponent_Factory(t) {
      return new (t || CreateFuelSurchargeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]));
    };

    CreateFuelSurchargeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: CreateFuelSurchargeComponent,
      selectors: [["app-create-fuel-surcharge"]],
      decls: 147,
      vars: 63,
      consts: [[3, "formGroup", "ngSubmit"], [4, "ngIf"], [3, "ngClass", "disabled"], [1, "well", "bg-white"], [1, "row"], [1, "col-sm-3", "form-group"], [1, "color-maroon"], ["type", "text", "formControlName", "TableName", 1, "form-control", 3, "readonly"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "placeholder", "settings", "data"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "placeholder", "settings", "data"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "placeholder", "settings", "data"], [1, "col-sm-6"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], [1, "my-3"], [1, "col-sm-12"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "float-left", "mb10"], [1, "btn-group", "btn-filter"], ["class", "hide-element", "type", "radio", 3, "name", "value", "checked", 4, "ngIf"], ["class", "btn ml0", 3, "click", 4, "ngIf"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], ["formGroupName", "FuelSurchargeTable", 1, "row"], [1, "col-sm-4"], [1, "font-weight-bold", "fs14"], [1, "col"], ["type", "text", "formControlName", "StartDate", "readonly", "readonly", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "formControlName", "EndDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], [1, "input-group", "mb-3"], ["type", "text", "formControlName", "PriceRangeStartValue", 1, "form-control"], [1, "input-group-append"], [1, "input-group-text", "fs11"], ["type", "text", "formControlName", "PriceRangeEndValue", 1, "form-control"], ["type", "text", "formControlName", "PriceRangeInterval", 1, "form-control"], ["type", "text", "formControlName", "FuelSurchargeStartPercentage", 1, "form-control"], ["type", "text", "formControlName", "SurchargeInterval", 1, "form-control"], ["type", "button", "value", "Generate Surcharge Table", 1, "btn", "btn-primary", "ml-0", 3, "click"], [1, "col-sm-4", 3, "formArrayName"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "button", 1, "btn", "btn-lg", "btn-outline-primary", 3, "disabled", "click"], ["type", "submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled"], ["class", "loader", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], ["for", "FuelSurchargeProducts"], ["formControlName", "FuelSurchargeProducts", "id", "FuelSurchargeProducts", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "FuelSurchargePeriods", "id", "FuelSurchargePeriods", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect"], ["formControlName", "FuelSurchargeAreas", "id", "FuelSurchargeAreas", 1, "single-select", 3, "placeholder", "settings", "data"], [1, "form-row"], [1, "col-7"], [1, "btn", "btn-default", "ml-0", 3, "click"], ["type", "text", "readonly", "readonly", "formControlName", "APILatestIndexPrice", 1, "form-control"], [1, "text-black-50"], ["type", "text", "formControlName", "ApiAdjustIndexPriceDate", "class", "form-control datepicker", "placeholder", "Date", "CustomDatePicker", "", 3, "format", "minDate", "maxDate", "mode", "daysOfWeekEnable", "onDateChange", 4, "ngIf"], ["formControlName", "SourceMonths", "class", "single-select", "id", "SourceMonthList", 3, "placeholder", "settings", "data", 4, "ngIf"], ["formControlName", "SourceAnnualy", "class", "single-select", "id", "idSourceAnnualy", 3, "placeholder", "settings", "data", 4, "ngIf"], ["class", "col-sm-6", 4, "ngIf"], ["type", "text", "formControlName", "ApiAdjustIndexPriceDate", "placeholder", "Date", "CustomDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "mode", "daysOfWeekEnable", "onDateChange"], ["formControlName", "SourceMonths", "id", "SourceMonthList", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "SourceAnnualy", "id", "idSourceAnnualy", 1, "single-select", 3, "placeholder", "settings", "data"], [1, "d-block"], [1, "form-check", "form-check-inline"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-mon", "value", "Mon", 1, "form-check-input"], ["for", "occurance-mon", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-tue", "value", "Tue", 1, "form-check-input"], ["for", "occurance-tue", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-wed", "value", "Wed", 1, "form-check-input"], ["for", "occurance-wed", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-thu", "value", "Thu", 1, "form-check-input"], ["for", "occurance-thu", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-fri", "value", "Fri", 1, "form-check-input"], ["for", "occurance-fri", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-sat", "value", "Sat", 1, "form-check-input"], ["for", "occurance-sat", 1, "form-check-label"], ["type", "radio", "formControlName", "WeekDay", "id", "occurance-sun", "value", "Sun", 1, "form-check-input"], ["for", "occurance-sun", 1, "form-check-label"], [1, "row", "mt-2"], ["formControlName", "Weeks", "id", "idWeeks", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "Months", "id", "idMonths", 1, "single-select", 3, "placeholder", "settings", "data"], ["formControlName", "Annualy", "id", "idAnnualy", 1, "single-select", 3, "placeholder", "settings", "data"], ["type", "text", "formControlName", "ManualLatestIndexPrice", 1, "form-control"], ["type", "text", "formControlName", "ManualEffectiveDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["formControlName", "Notes", 1, "form-control"], [1, "table", "table-bordered", "table-hover", "mt-3"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [4, "ngFor", "ngForOf"], [1, "text-center", "vmiddle"], [1, "input-group"], [1, "p-2", "border", "px-4"], [1, "input-group-text"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CreateFuelSurchargeComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngSubmit", function CreateFuelSurchargeComponent_Template_form_ngSubmit_1_listener() {
            return ctx.onSubmit(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateFuelSurchargeComponent_div_2_Template, 4, 0, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "fieldset", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Table Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, CreateFuelSurchargeComponent_div_12_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Table Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateFuelSurchargeComponent_Template_ng_multiselect_dropdown_onSelect_19_listener($event) {
            return ctx.onTableTypeSelect($event);
          })("onDeSelect", function CreateFuelSurchargeComponent_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) {
            return ctx.onTableTypeDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, CreateFuelSurchargeComponent_div_20_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "label", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Customer(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](26, "ng-multiselect-dropdown", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, CreateFuelSurchargeComponent_div_27_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "label", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Carrier(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](33, "ng-multiselect-dropdown", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, CreateFuelSurchargeComponent_div_34_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Source Region(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](41, "ng-multiselect-dropdown", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](42, CreateFuelSurchargeComponent_div_42_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "label", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](47, "Terminal(s)/BulkPlant(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](48, "angular2-multiselect", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](49, "hr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](54, CreateFuelSurchargeComponent_input_54_Template, 1, 3, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](55, CreateFuelSurchargeComponent_label_55_Template, 2, 0, "label", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](56, "input", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "label", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_Template_label_click_57_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58, "Manual Update");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](61, CreateFuelSurchargeComponent_div_61_Template, 49, 29, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](62, CreateFuelSurchargeComponent_div_62_Template, 24, 7, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](63, "hr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "h3");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](65, "Fuel Surcharge Table");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "h5", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](69, "Valid");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](70, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](72, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](74, "From");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](75, "input", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateFuelSurchargeComponent_Template_input_onDateChange_75_listener($event) {
            return ctx.setStartDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](76, CreateFuelSurchargeComponent_div_76_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](77, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](78, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](79, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](80, "To");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "input", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateFuelSurchargeComponent_Template_input_onDateChange_81_listener($event) {
            return ctx.setEndDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](82, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](83, "h5", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](84, "Price Range");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](86, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](87, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](88, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](89, "Start Value");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](90, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](91, "input", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](92, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](93, "span", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](94);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](95, CreateFuelSurchargeComponent_div_95_Template, 2, 0, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](96, CreateFuelSurchargeComponent_div_96_Template, 3, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](97, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](98, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](99, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](100, "End Value");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](101, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](102, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](103, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](104, "span", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](105);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](106, CreateFuelSurchargeComponent_div_106_Template, 3, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](107, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](108, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](109, "Interval");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](110, "input", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](111, CreateFuelSurchargeComponent_div_111_Template, 3, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](112, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](113, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](114, "h5", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](115, "Surcharge");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](116, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](117, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](118, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](119, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](120, "Start %");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](121, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](122, "input", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](123, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](124, "span", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](125, "%");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](126, CreateFuelSurchargeComponent_div_126_Template, 3, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](127, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](128, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](129, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](130, "Interval");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](131, "input", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](132, CreateFuelSurchargeComponent_div_132_Template, 3, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](133, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](134, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](135, "input", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_Template_input_click_135_listener() {
            return ctx.onGenerateSurchargeTable();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](136, CreateFuelSurchargeComponent_div_136_Template, 2, 0, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](137, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](138, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](139, CreateFuelSurchargeComponent_div_139_Template, 9, 1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](140, "div", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](141, "input", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_Template_input_click_141_listener() {
            return ctx.onCancel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](142, "button", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateFuelSurchargeComponent_Template_button_click_142_listener() {
            return ctx.onSubmit(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](143, "Save Draft");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](144, "button", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](145, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](146, CreateFuelSurchargeComponent_div_146_Template, 5, 0, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.rcForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.fuelsurchargeMode == "VIEW" || ctx.fuelsurchargeMode == "COPY" || ctx.fuelsurchargeMode == "EDIT");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](55, _c0, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", ctx.fuelsurchargeMode == "EDIT" && !ctx.AllowTableName ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableName").invalid && ctx.rcForm.get("TableName").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Table Type")("settings", ctx.SingleSelectSettingsById)("data", ctx.TableTypeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableTypes").invalid && ctx.rcForm.get("TableTypes").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](57, _c1, ctx.IsMasterSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Customers(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.CustomerList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Customers").invalid && ctx.rcForm.get("Customers").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](59, _c1, ctx.IsMasterSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Carriers(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.CarrierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCarrierSelected && ctx.rcForm.get("Carriers").invalid && ctx.rcForm.get("Carriers").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Source Regions(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.SourceRegionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("SourceRegions").invalid && ctx.rcForm.get("SourceRegions").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](61, _c1, !ctx.IsSourceRegionSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.TerminalsAndBulkPlantList)("settings", ctx.MultiSelectSettingsByGroup);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedCountryId != 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedCountryId != 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "eia-period")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 1 && ctx.SelectedCountryId != 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 2 || ctx.SelectedCountryId == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("StartDate").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("StartDate").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinToDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.SelectedCountryId == 1 ? "USD" : "CAD");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx.rcForm.valid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").value > ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").value && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").touched && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeStartValue").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.SelectedCountryId == 1 ? "USD" : "CAD");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeEndValue").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("PriceRangeInterval").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("FuelSurchargeStartPercentage").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").invalid && ctx.rcForm.controls.FuelSurchargeTable.get("SurchargeInterval").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.ShowMessage);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formArrayName", "GeneratedSurchargeTable");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsGeneratedSurchargeTable);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_14__["MultiSelectComponent"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormGroupName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormArrayName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_16__["CustomDatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RadioControlValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"]],
      styles: [".tabs label {\r\n    background: none;\r\n    font-size: 16px;\r\n    border-bottom: 2px solid #f7f7f7;\r\n}\r\n\r\n  .tabs input[type=\"radio\"]:checked + label {\r\n    background: none;\r\n    border-bottom: 2px solid blue;\r\n    color: #0c52b1;\r\n    border-bottom: 2px solid #0c52b1;\r\n    font-size:16px;\r\n}\r\n\r\n  .custom-search {\r\n    left: 0;\r\n    top:0;\r\n}\r\n\r\n  .custom-search .card {\r\n        border:0;\r\n    }\r\n\r\n  .custom-search .card-body {\r\n        padding:0;\r\n    }\r\n\r\n  .custom-search .card-body li:hover {\r\n            background: #007bff;\r\n            cursor:pointer;\r\n            color:white;\r\n        }\r\n\r\n  .custom-search .card .card-header{\r\n        padding: 0 15px;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZnVlbHN1cmNoYXJnZS9DcmVhdGUvY3JlYXRlLWZ1ZWwtc3VyY2hhcmdlLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxnQkFBZ0I7SUFDaEIsZUFBZTtJQUNmLGdDQUFnQztBQUNwQzs7QUFFQTtJQUNJLGdCQUFnQjtJQUNoQiw2QkFBNkI7SUFDN0IsY0FBYztJQUNkLGdDQUFnQztJQUNoQyxjQUFjO0FBQ2xCOztBQUVBO0lBQ0ksT0FBTztJQUNQLEtBQUs7QUFDVDs7QUFDSTtRQUNJLFFBQVE7SUFDWjs7QUFDQTtRQUNJLFNBQVM7SUFDYjs7QUFDSTtZQUNJLG1CQUFtQjtZQUNuQixjQUFjO1lBQ2QsV0FBVztRQUNmOztBQUNKO1FBQ0ksZUFBZTtJQUNuQiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvQ3JlYXRlL2NyZWF0ZS1mdWVsLXN1cmNoYXJnZS5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC50YWJzIGxhYmVsIHtcclxuICAgIGJhY2tncm91bmQ6IG5vbmU7XHJcbiAgICBmb250LXNpemU6IDE2cHg7XHJcbiAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2Y3ZjdmNztcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJzIGlucHV0W3R5cGU9XCJyYWRpb1wiXTpjaGVja2VkICsgbGFiZWwge1xyXG4gICAgYmFja2dyb3VuZDogbm9uZTtcclxuICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCBibHVlO1xyXG4gICAgY29sb3I6ICMwYzUyYjE7XHJcbiAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgIzBjNTJiMTtcclxuICAgIGZvbnQtc2l6ZToxNnB4O1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2gge1xyXG4gICAgbGVmdDogMDtcclxuICAgIHRvcDowO1xyXG59XHJcbiAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQge1xyXG4gICAgICAgIGJvcmRlcjowO1xyXG4gICAgfVxyXG4gICAgOjpuZy1kZWVwIC5jdXN0b20tc2VhcmNoIC5jYXJkLWJvZHkge1xyXG4gICAgICAgIHBhZGRpbmc6MDtcclxuICAgIH1cclxuICAgICAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQtYm9keSBsaTpob3ZlciB7XHJcbiAgICAgICAgICAgIGJhY2tncm91bmQ6ICMwMDdiZmY7XHJcbiAgICAgICAgICAgIGN1cnNvcjpwb2ludGVyO1xyXG4gICAgICAgICAgICBjb2xvcjp3aGl0ZTtcclxuICAgICAgICB9XHJcbiAgICA6Om5nLWRlZXAgLmN1c3RvbS1zZWFyY2ggLmNhcmQgLmNhcmQtaGVhZGVye1xyXG4gICAgICAgIHBhZGRpbmc6IDAgMTVweDtcclxuICAgIH1cclxuXHJcblxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CreateFuelSurchargeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-create-fuel-surcharge',
          templateUrl: './create-fuel-surcharge.component.html',
          styleUrls: ['./create-fuel-surcharge.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]
        }, {
          type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]
        }, {
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_11__["HttpClient"]
        }, {
          type: _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]
        }];
      }, null);
    })();

    var RangeValidator = function RangeValidator(fg) {
      var fst = fg.get('FuelSurchargeTable').value;
      var start = fst.PriceRangeStartValue;
      var end = fst.PriceRangeEndValue;
      var statusId = fg.get('StatusId').value;
      return statusId == 1 || start != null && end != null && +end > +start ? null : {
        range: true
      };
    };
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts": function srcAppFuelsurchargeViewViewFuelSurchargePricingdetailsViewFuelSurchargePricingdetailsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewFuelSurchargePricingdetailsComponent", function () {
      return ViewFuelSurchargePricingdetailsComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function ViewFuelSurchargePricingdetailsComponent_tr_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "span", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "%");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var pricing_r1 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" $", pricing_r1.PriceRangeStartValue, " - $", pricing_r1.PriceRangeEndValue, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](pricing_r1.FuelSurchargeStartPercentage);
      }
    }

    var ViewFuelSurchargePricingdetailsComponent = /*#__PURE__*/function () {
      function ViewFuelSurchargePricingdetailsComponent(fb, fuelsurchargeService) {
        _classCallCheck(this, ViewFuelSurchargePricingdetailsComponent);

        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.FuelSurchargePricingList = [];
      }

      _createClass(ViewFuelSurchargePricingdetailsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "getFuelSurchargePricingDetails",
        value: function getFuelSurchargePricingDetails(fuelSurchargeIndexId) {
          var _this20 = this;

          this.fuelsurchargeService.getSurchargeTableNew(fuelSurchargeIndexId).subscribe(function (data) {
            _this20.FuelSurchargePricingList = data;
          });
        }
      }]);

      return ViewFuelSurchargePricingdetailsComponent;
    }();

    ViewFuelSurchargePricingdetailsComponent.ɵfac = function ViewFuelSurchargePricingdetailsComponent_Factory(t) {
      return new (t || ViewFuelSurchargePricingdetailsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]));
    };

    ViewFuelSurchargePricingdetailsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ViewFuelSurchargePricingdetailsComponent,
      selectors: [["app-view-fuel-surcharge-pricingdetails"]],
      decls: 12,
      vars: 1,
      consts: [[1, "col-sm-12"], [1, "table"], ["width", "47%", 1, "text-center"], ["width", "48%", 1, "text-center"], ["id", "surchargeTable", 2, "max-height", "70vh", "overflow", "auto"], [1, "table", "table-bordered", "table-hover", "mb0"], [4, "ngFor", "ngForOf"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [1, "input-group"], [1, "p-2", "border", "px-4"], [1, "input-group-append"], [1, "input-group-text"]],
      template: function ViewFuelSurchargePricingdetailsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "table", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "th", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Price Between");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "th", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Fuel Surcharge %");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "table", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, ViewFuelSurchargePricingdetailsComponent_tr_11_Template, 10, 3, "tr", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.FuelSurchargePricingList);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvVmlldy92aWV3LWZ1ZWwtc3VyY2hhcmdlLXByaWNpbmdkZXRhaWxzL3ZpZXctZnVlbC1zdXJjaGFyZ2UtcHJpY2luZ2RldGFpbHMuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ViewFuelSurchargePricingdetailsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-view-fuel-surcharge-pricingdetails',
          templateUrl: './view-fuel-surcharge-pricingdetails.component.html',
          styleUrls: ['./view-fuel-surcharge-pricingdetails.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts": function srcAppFuelsurchargeViewViewFuelSurchargeComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewFuelSurchargeComponent", function () {
      return ViewFuelSurchargeComponent;
    });
    /* harmony import */


    var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! tslib */
    "./node_modules/tslib/tslib.es6.js");
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _models_CreateFuelSurcharge__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../models/CreateFuelSurcharge */
    "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var _view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ./view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component */
    "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./view-historical-price/view-historical-price.component */
    "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    var _c0 = function _c0(a0) {
      return {
        "d-block": a0
      };
    };

    function ViewFuelSurchargeComponent_tr_50_td_18_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](6, _c0, !surcharge_r5.IsShowMore));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](2, 2, surcharge_r5.Terminal, 0, 40), "...");
      }
    }

    function ViewFuelSurchargeComponent_tr_50_td_18_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, ViewFuelSurchargeComponent_tr_50_td_18_div_3_Template, 3, 8, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_td_18_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          return surcharge_r5.IsShowMore = !surcharge_r5.IsShowMore;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "View More/Less");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](3, _c0, surcharge_r5.IsShowMore));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.Terminal);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", surcharge_r5.Terminal.length > 40);
      }
    }

    function ViewFuelSurchargeComponent_tr_50_td_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.Terminal);
      }
    }

    function ViewFuelSurchargeComponent_tr_50_a_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("cancel", function ViewFuelSurchargeComponent_tr_50_a_34_Template_a_cancel_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r19.cancelClicked = true;
        })("confirm", function ViewFuelSurchargeComponent_tr_50_a_34_Template_a_confirm_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20);

          var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r21.archiveFuelSurchargeTable(surcharge_r5.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("popoverTitle", ctx_r9.popoverTitle)("popoverMessage", ctx_r9.popoverMessage);
      }
    }

    function ViewFuelSurchargeComponent_tr_50_a_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_a_35_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r25);

          var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r23.viewFuelSurcharge(surcharge_r5.Id, "EDIT");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewFuelSurchargeComponent_tr_50_a_38_Template(rf, ctx) {
      if (rf & 1) {
        var _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_a_38_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r28);

          var surcharge_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r26.viewFuelSurcharge(surcharge_r5.Id, "COPY");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewFuelSurchargeComponent_tr_50_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var surcharge_r5 = ctx.$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r29.getFuelSurchargePricingDetails(surcharge_r5.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, ViewFuelSurchargeComponent_tr_50_td_18_Template, 6, 5, "td", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, ViewFuelSurchargeComponent_tr_50_td_19_Template, 2, 1, "td", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_31_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var surcharge_r5 = ctx.$implicit;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r31.getHistoricalPriceDetails(surcharge_r5.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](32, "i", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "td", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, ViewFuelSurchargeComponent_tr_50_a_34_Template, 2, 2, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, ViewFuelSurchargeComponent_tr_50_a_35_Template, 2, 0, "a", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "a", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_tr_50_Template_a_click_36_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var surcharge_r5 = ctx.$implicit;

          var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r32.viewFuelSurcharge(surcharge_r5.Id, "VIEW");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](37, "i", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, ViewFuelSurchargeComponent_tr_50_a_38_Template, 2, 0, "a", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var surcharge_r5 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", surcharge_r5.DateRange, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.TableTypeNew);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.TableName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.StatusName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.Customer);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.Carrier);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.SourceRegion);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", surcharge_r5.Terminal.length > 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", surcharge_r5.Terminal.length <= 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.BulkPlant);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.IndexProduct);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.IndexArea);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.IndexPeriod);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](surcharge_r5.IndexType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !surcharge_r5.IsArchived);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !surcharge_r5.IsArchived);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !surcharge_r5.IsArchived);
      }
    }

    function ViewFuelSurchargeComponent_div_69_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewFuelSurchargeComponent_ng_template_70_div_7_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Table Type is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewFuelSurchargeComponent_ng_template_70_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, ViewFuelSurchargeComponent_ng_template_70_div_7_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r33.viewFuelSurchargeForm.get("TableTypes").errors.required);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    function ViewFuelSurchargeComponent_ng_template_70_Template(rf, ctx) {
      if (rf & 1) {
        var _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Table Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "ng-multiselect-dropdown", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_6_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r35.onTableTypeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, ViewFuelSurchargeComponent_ng_template_70_div_7_Template, 2, 1, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Customer(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "ng-multiselect-dropdown", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r37.onCustomersSelect($event);
        })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r38.onCustomersDeSelect($event);
        })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r39.onCustomersDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Carrier(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r40.onCarriersSelect($event);
        })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r41.onCarriersDeSelect($event);
        })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r42.onCarriersDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "label", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Source Region(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "ng-multiselect-dropdown", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onSelect_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r43.onSourceRegionsSelect($event);
        })("onDeSelect", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelect_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r44.onSourceRegionsDeSelect($event);
        })("onDeSelectAll", function ViewFuelSurchargeComponent_ng_template_70_Template_ng_multiselect_dropdown_onDeSelectAll_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r45.onSourceRegionsDeSelectAll($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Terminal(s)/BulkPlant(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](29, "angular2-multiselect", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "input", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewFuelSurchargeComponent_ng_template_70_Template_input_onDateChange_34_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r46.setfromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "To");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "input", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewFuelSurchargeComponent_ng_template_70_Template_input_onDateChange_39_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r47.settoDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](43, "input", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "label", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, " Show Archived ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "button", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_ng_template_70_Template_button_click_47_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r48.clearFilter();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48, "Clear Filter");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "button", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_ng_template_70_Template_button_click_49_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r36);

          var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          var _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

          ctx_r49.filterGrid();
          return _r0.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](50, "Apply");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Type (Required)")("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.TableTypeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.viewFuelSurchargeForm.get("TableTypes").invalid && ctx_r4.viewFuelSurchargeForm.get("TableTypes").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](22, _c1, ctx_r4.IsMasterSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Customers")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CustomerList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](24, _c1, ctx_r4.IsMasterSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Carriers")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CarrierList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Source Regions")("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.SourceRegionList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx_r4.TerminalsAndBulkPlantList)("settings", ctx_r4.MultiSelectSettingsByGroup);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("minDate", ctx_r4.minDate)("maxDate", ctx_r4.maxDate)("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](26, _c1, ctx_r4.IsLoading));
      }
    }

    var ViewFuelSurchargeComponent = /*#__PURE__*/function () {
      function ViewFuelSurchargeComponent(fb, regionService, fuelsurchargeService, router, cdr) {
        _classCallCheck(this, ViewFuelSurchargeComponent);

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

      _createClass(ViewFuelSurchargeComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.rerender_destroy();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getFuelSurchargeGridDetails();
        }
      }, {
        key: "createForm",
        value: function createForm() {
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
      }, {
        key: "getFuelSurchargePricingDetails",
        value: function getFuelSurchargePricingDetails(fuelSurchargeIndexId) {
          this.fuelSurchageComponent.getFuelSurchargePricingDetails(fuelSurchargeIndexId);
        }
      }, {
        key: "getHistoricalPriceDetails",
        value: function getHistoricalPriceDetails(fuelSurchargeIndexId) {
          this.historicalPriceComponent.getHistoricalPriceDetails(fuelSurchargeIndexId);
        }
      }, {
        key: "archiveFuelSurchargeTable",
        value: function archiveFuelSurchargeTable(fuelSurchargeIndexId) {
          var _this21 = this;

          this.IsLoading = true;
          this.fuelsurchargeService.archiveFuelSurchargeTable(fuelSurchargeIndexId).subscribe(function (response) {
            _this21.IsLoading = false; //this.serviceResponse = response;

            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__["Declarations"].msgsuccess('Selected fuel surcharge table archived successfully', undefined, undefined);

              _this21.filterGrid();
            }
          });
        }
      }, {
        key: "onTableTypeSelect",
        value: function onTableTypeSelect(item) {
          this.IsMasterSelected = false;
          this.IsCustomerSelected = false;
          this.IsCarrierSelected = false;
          this.viewFuelSurchargeForm.get('Carriers').patchValue([]);
          this.viewFuelSurchargeForm.get('Customers').patchValue([]);

          switch (item.Id) {
            case 1:
              //master
              this.IsMasterSelected = true;
              break;

            case 2:
              // customer
              this.IsCustomerSelected = true;
              this.getSupplierCustomers();
              this.getCarriers();
              break;

            case 3:
              //carrier
              this.IsCarrierSelected = true;
              this.getCarriers();
              this.getSupplierCustomers();
              break;
          }

          this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
          this.getSourceRegions(item.Id);
        }
      }, {
        key: "onCarriersSelect",
        value: function onCarriersSelect(item) {
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCarriersDeSelect",
        value: function onCarriersDeSelect(item) {
          this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCustomersSelect",
        value: function onCustomersSelect(item) {
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCustomersDeSelect",
        value: function onCustomersDeSelect(item) {
          this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCarriersOrCustomersChange",
        value: function onCarriersOrCustomersChange() {
          var selectedTableType = this.viewFuelSurchargeForm.get('TableTypes').value;
          this.getSourceRegions(selectedTableType[0].Id.toString());
        }
      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          var _this22 = this;

          this.fuelsurchargeService.getTableTypes().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this22, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee13() {
              return regeneratorRuntime.wrap(function _callee13$(_context13) {
                while (1) {
                  switch (_context13.prev = _context13.next) {
                    case 0:
                      _context13.next = 2;
                      return data;

                    case 2:
                      this.TableTypeList = _context13.sent;

                    case 3:
                    case "end":
                      return _context13.stop();
                  }
                }
              }, _callee13, this);
            }));
          });
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this23 = this;

          this.IsLoading = true;
          this.regionService.getCarriers().subscribe(function (carriers) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this23, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee14() {
              return regeneratorRuntime.wrap(function _callee14$(_context14) {
                while (1) {
                  switch (_context14.prev = _context14.next) {
                    case 0:
                      _context14.next = 2;
                      return carriers;

                    case 2:
                      this.CarrierList = _context14.sent;
                      this.SourceRegionList = null;
                      this.IsLoading = false;

                    case 5:
                    case "end":
                      return _context14.stop();
                  }
                }
              }, _callee14, this);
            }));
          });
        }
      }, {
        key: "getSupplierCustomers",
        value: function getSupplierCustomers() {
          var _this24 = this;

          this.IsLoading = true;
          this.fuelsurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this24, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee15() {
              return regeneratorRuntime.wrap(function _callee15$(_context15) {
                while (1) {
                  switch (_context15.prev = _context15.next) {
                    case 0:
                      _context15.next = 2;
                      return data;

                    case 2:
                      this.CustomerList = _context15.sent;
                      this.IsLoading = false;

                    case 4:
                    case "end":
                      return _context15.stop();
                  }
                }
              }, _callee15, this);
            }));
          });
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions(tableType) {
          var _this25 = this;

          ;
          var customerIds = [];
          var carrierIds = [];
          var selectedCustomers = this.viewFuelSurchargeForm.get('Customers').value;

          if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(function (s) {
              return s.Id;
            });
          }

          var selectedCarriers = this.viewFuelSurchargeForm.get('Carriers').value;

          if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(function (s) {
              return s.Id;
            });
          }

          var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
          sourceRegionInput.TableType = tableType;
          sourceRegionInput.CustomerId = customerIds;
          sourceRegionInput.CarrierId = carrierIds;
          this.IsLoading = true;
          this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(function (data) {
            _this25.SourceRegionList = data;
            _this25.IsLoading = false;
          });
        }
      }, {
        key: "getTerminalsBulkPlant",
        value: function getTerminalsBulkPlant() {
          var _this26 = this;

          this.IsLoading = true;
          var selectedSourceRegions = this.viewFuelSurchargeForm.get('SourceRegions').value;

          if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(function (s) {
              return s.Id;
            }).join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this26, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee16() {
                return regeneratorRuntime.wrap(function _callee16$(_context16) {
                  while (1) {
                    switch (_context16.prev = _context16.next) {
                      case 0:
                        _context16.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context16.sent;
                        this.IsLoading = false;

                      case 4:
                      case "end":
                        return _context16.stop();
                    }
                  }
                }, _callee16, this);
              }));
            });
          }
        }
      }, {
        key: "onSourceRegionsDeSelect",
        value: function onSourceRegionsDeSelect(item) {
          this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
        }
      }, {
        key: "onSourceRegionsDeSelectAll",
        value: function onSourceRegionsDeSelectAll(item) {
          this.IsSourceRegionSelected = false;
        }
      }, {
        key: "onSourceRegionsSelect",
        value: function onSourceRegionsSelect(item) {
          this.getTerminalsBulkPlant();
          this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
          this.getFuelSurchargeGridDetails();
        }
      }, {
        key: "clearFilter",
        value: function clearFilter() {
          this.clearForm();
          this.getFuelSurchargeGridDetails();
          this.CustomerList = [];
          this.CarrierList = [];
          this.SourceRegionList = [];
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.viewFuelSurchargeForm.reset();
          $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "getFuelSurchargeGridDetails",
        value: function getFuelSurchargeGridDetails() {
          var _this27 = this;

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
            var tableTypeIds = selectedTableTypes.map(function (s) {
              return s.Id;
            });
            input.TableTypeIds = tableTypeIds.join(',');
          }

          if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            var customerIds = selectedCustomers.map(function (s) {
              return s.Id;
            });
            input.CustomerIds = customerIds.join(',');
          }

          if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            var carrierIds = selectedCarriers.map(function (s) {
              return s.Id;
            });
            input.CarrierIds = carrierIds.join(',');
          }

          if (selectedSourceRegions != null && selectedSourceRegions != undefined && selectedSourceRegions.length > 0) {
            var sourceRegionIds = selectedSourceRegions.map(function (s) {
              return s.Id;
            });
            input.SourceRegionIds = sourceRegionIds.join(',');
          }

          if (selectedTerminalAndBulkPlants != null && selectedTerminalAndBulkPlants != undefined && selectedTerminalAndBulkPlants.length > 0) {
            var selectedTerminalIds = selectedTerminalAndBulkPlants.filter(function (c) {
              return c.Code == "Terminals";
            });
            var terminalIds = selectedTerminalIds.map(function (s) {
              return s.Id;
            });
            input.TerminalIds = terminalIds.join(',');
            var selectedBulkPlants = selectedTerminalAndBulkPlants.filter(function (c) {
              return c.Code == "BulkPlants";
            });
            var bulkPlantIds = selectedBulkPlants.map(function (s) {
              return s.Id;
            });
            input.BulkPlantIds = bulkPlantIds.join(',');
          }

          this.IsLoading = true;
          this.cdr.detectChanges();
          this.fuelsurchargeService.getFuelSurchargeGridDetails(input).subscribe(function (data) {
            _this27.IsLoading = false;

            if (data && data.length > 0) {
              _this27.FuelSurchargeList = data;
            }

            _this27.dtTrigger.next();
          });
        }
      }, {
        key: "rerender_destroy",
        value: function rerender_destroy() {
          if (this.datatableElement && this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();
            });
          }
        }
      }, {
        key: "rerender_trigger",
        value: function rerender_trigger() {
          var _this28 = this;

          if (this.datatableElement && this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              _this28.dtTrigger.next();
            });
          }
        }
      }, {
        key: "setfromDate",
        value: function setfromDate($event) {
          this.viewFuelSurchargeForm.controls.fromDate.setValue($event);
        }
      }, {
        key: "settoDate",
        value: function settoDate($event) {
          this.viewFuelSurchargeForm.controls.toDate.setValue($event);
        }
      }, {
        key: "viewFuelSurcharge",
        value: function viewFuelSurcharge(fsurcharId, mode) {
          var operation = {
            FsurcharId: fsurcharId,
            Mode: mode
          };
          this.fuelsurchargeService.onSelectedFuelSurchargeId.next(JSON.stringify(operation));
          this.fuelsurchargeService.onSelectedTabChanged.next(1); //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew/' + fsurcharId]);
        }
      }, {
        key: "initializeFuelSurchargeDatatableGrid",
        value: function initializeFuelSurchargeDatatableGrid() {
          var exportColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Fuel Surcharge',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Fuel Surcharge',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }]);

      return ViewFuelSurchargeComponent;
    }();

    ViewFuelSurchargeComponent.ɵfac = function ViewFuelSurchargeComponent_Factory(t) {
      return new (t || ViewFuelSurchargeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_13__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]));
    };

    ViewFuelSurchargeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: ViewFuelSurchargeComponent,
      selectors: [["app-view-fuel-surcharge"]],
      viewQuery: function ViewFuelSurchargeComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.fuelSurchageComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.historicalPriceComponent = _t.first);
        }
      },
      decls: 72,
      vars: 7,
      consts: [[3, "formGroup"], [1, "row"], [1, "col-sm-12", "text-left"], ["placement", "bottom", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs16", "mr10", "filter-link", "pa", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-fuel-surcharge-grid", 1, "table-responsive"], ["id", "fuel-surcharge-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Id", 1, "hide-element"], ["data-key", "DateRange"], ["data-key", "TableTypeNew"], ["data-key", "TableName"], ["data-key", "StatusName"], ["data-key", "Customer"], ["data-key", "Carrier"], ["data-key", "SourceRegion"], ["data-key", "Terminal"], ["data-key", "BulkPlant"], ["data-key", "IndexProduct"], ["data-key", "IndexArea"], ["data-key", "IndexPeriod"], ["data-key", "IndexUpdate"], ["data-key", "HistoricalPrice"], [4, "ngFor", "ngForOf"], ["id", "historical-price-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], ["id", "pricing-panel", 1, "side-panel", "pl5", "pr5"], ["class", "loader", 4, "ngIf"], ["popContent", ""], [1, "hide-element"], [1, "text-center"], ["onclick", "slidePanel('#pricing-panel','30%')", 1, "btn", "btn-link", 3, "click"], [4, "ngIf"], ["onclick", "slidePanel('#historical-price-panel','30%')", "placement", "bottom", "ngbTooltip", "Historical Price", 1, "btn", "btn-link", "fs16", 3, "click"], [1, "fas", "fa-history"], [1, "text-center", "text-nowrap"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-street-view"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Copy", 3, "click", 4, "ngIf"], [1, "d-none", 3, "ngClass"], ["class", "d-none", 3, "ngClass", 4, "ngIf"], [3, "click"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-trash-alt", "color-maroon"], ["placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], ["placement", "bottom", "ngbTooltip", "Copy", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-copy"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "popover-details"], [1, "col-sm-6"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 3, "placeholder", "settings", "data", "onSelect"], ["class", "color-maroon", 4, "ngIf"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onDeSelectAll"], [1, "col-sm-12"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "placeholder", "Select Date", "formControlName", "fromDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Select Date", "formControlName", "toDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-6", "form-group"], [1, "form-check"], ["formControlName", "isArchived", "type", "checkbox", "value", "", "id", "ckb-isArchived", 1, "form-check-input"], ["for", "ckb-isArchived", 1, "form-check-label"], [1, "col-sm-6", "text-right", "form-buttons", "mt20"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], ["id", "filter-fuel-surcharge-grid", "type", "button", 1, "btn", "btn-lg", "btn-primary", "mt3", "valid", 3, "ngClass", "click"], [1, "color-maroon"]],
      template: function ViewFuelSurchargeComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 3, 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewFuelSurchargeComponent_Template_a_click_3_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r50);

            var _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

            return _r0.open();
          });

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

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Table Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Table Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Customer(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Carrier(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Source Region(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Terminal(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Bulk Plant(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Index Product");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Index Area");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Index Period");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Index Update");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "Historical Price");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](50, ViewFuelSurchargeComponent_tr_50_Template, 39, 18, "tr", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "a", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](55, "i", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "h3", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](57, "Historical Price Details");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](59, "app-view-historical-price");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "a", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](64, "i", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "h3", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](66, "Fuel Surcharge Table");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](68, "app-view-fuel-surcharge-pricingdetails");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](69, ViewFuelSurchargeComponent_div_69_Template, 5, 0, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](70, ViewFuelSurchargeComponent_ng_template_70_Template, 51, 28, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](71);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.viewFuelSurchargeForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r3)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.FuelSurchargeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbPopover"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgForOf"], _view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"], _view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_15__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_16__["ɵc"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["CheckboxControlValueAccessor"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_15__["SlicePipe"]],
      styles: [".master-filter.popover {\r\n    min-width: 425px;\r\n    max-width: 450px;\r\n    background: #F9F9F9;\r\n    border: 1px solid #E9E7E7;\r\n    box-sizing: border-box;\r\n    box-shadow: 10px 10px 8px -2px rgb(0, 0, 0, 0.13);\r\n    border-radius: 10px;\r\n}\r\n      .master-filter.popover .popover-body {\r\n        padding: 0;\r\n        border-radius: 5px;\r\n        background: #ffffff;\r\n    }\r\n      .master-filter.popover .popover-details {\r\n        padding: 15px;\r\n    }\r\n      .master-filter.popover .border-bottom-2 {\r\n        border-bottom: 2px solid #e7eaec !important;\r\n    }\r\n    .filter-link[_ngcontent-%COMP%] {\r\n    top: -45px;\r\n    left: 380px\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZnVlbHN1cmNoYXJnZS9WaWV3L3ZpZXctZnVlbC1zdXJjaGFyZ2UuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGdCQUFnQjtJQUNoQixnQkFBZ0I7SUFDaEIsbUJBQW1CO0lBQ25CLHlCQUF5QjtJQUN6QixzQkFBc0I7SUFDdEIsaURBQWlEO0lBQ2pELG1CQUFtQjtBQUN2QjtJQUNJO1FBQ0ksVUFBVTtRQUNWLGtCQUFrQjtRQUNsQixtQkFBbUI7SUFDdkI7SUFFQTtRQUNJLGFBQWE7SUFDakI7SUFFQTtRQUNJLDJDQUEyQztJQUMvQztJQUNKO0lBQ0ksVUFBVTtJQUNWO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9mdWVsc3VyY2hhcmdlL1ZpZXcvdmlldy1mdWVsLXN1cmNoYXJnZS5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIge1xyXG4gICAgbWluLXdpZHRoOiA0MjVweDtcclxuICAgIG1heC13aWR0aDogNDUwcHg7XHJcbiAgICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiKDAsIDAsIDAsIDAuMTMpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxufVxyXG4gICAgOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIH1cclxuXHJcbiAgICA6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgfVxyXG5cclxuICAgIDo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICB9XHJcbi5maWx0ZXItbGluayB7XHJcbiAgICB0b3A6IC00NXB4O1xyXG4gICAgbGVmdDogMzgwcHhcclxufVxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](ViewFuelSurchargeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-view-fuel-surcharge',
          templateUrl: './view-fuel-surcharge.component.html',
          styleUrls: ['./view-fuel-surcharge.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_11__["RegionService"]
        }, {
          type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_12__["FuelSurchargeService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_13__["Router"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]
        }];
      }, {
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"]]
        }],
        fuelSurchageComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_6__["ViewFuelSurchargePricingdetailsComponent"]]
        }],
        historicalPriceComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_8__["ViewHistoricalPriceComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts": function srcAppFuelsurchargeViewViewHistoricalPriceViewHistoricalPriceComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewHistoricalPriceComponent", function () {
      return ViewHistoricalPriceComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");

    function ViewHistoricalPriceComponent_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "ng-multiselect-dropdown", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ViewHistoricalPriceComponent_div_2_Template_ng_multiselect_dropdown_ngModelChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r4.SelectedPeriod = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Select ", ctx_r0.HistoricalPrice == null ? null : ctx_r0.HistoricalPrice.PeriodName, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r0.SelectedPeriod)("settings", ctx_r0.SinlgeselectSettingsById)("data", ctx_r0.PeriodList);
      }
    }

    function ViewHistoricalPriceComponent_div_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewHistoricalPriceComponent_div_3_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r7);

          var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r6.fetchHistoricalPrice();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Fetch Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ViewHistoricalPriceComponent_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("$", ctx_r2.HistoricalPrice == null ? null : ctx_r2.HistoricalPrice.ManualIndexPrice, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" per Gallon (Including Taxes) on ", ctx_r2.HistoricalPrice == null ? null : ctx_r2.HistoricalPrice.ManualIndexPriceDate, " ");
      }
    }

    function ViewHistoricalPriceComponent_div_5_ng_container_28_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "$");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "span", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var history_r10 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", history_r10.PublishDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](history_r10.Price);
      }
    }

    function ViewHistoricalPriceComponent_div_5_ng_container_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, ViewHistoricalPriceComponent_div_5_ng_container_28_tr_1_Template, 10, 2, "tr", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r8.HistoricalPrice.HistoricalPriceDetails);
      }
    }

    function ViewHistoricalPriceComponent_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "table", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Index Product");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Index Area");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Index Period");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "table", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Published Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "th", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Price per Gallon");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "table", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, ViewHistoricalPriceComponent_div_5_ng_container_28_Template, 2, 1, "ng-container", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexProduct);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexArea);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r3.HistoricalPrice == null ? null : ctx_r3.HistoricalPrice.IndexPeriod);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.HistoricalPrice && ctx_r3.HistoricalPrice.HistoricalPriceDetails && ctx_r3.HistoricalPrice.HistoricalPriceDetails.length > 0);
      }
    }

    var ViewHistoricalPriceComponent = /*#__PURE__*/function () {
      function ViewHistoricalPriceComponent(fb, fuelsurchargeService) {
        _classCallCheck(this, ViewHistoricalPriceComponent);

        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.SinlgeselectSettingsById = {};
        this.PeriodList = [];
        this.SelectedPeriod = [];
      }

      _createClass(ViewHistoricalPriceComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
      }, {
        key: "getPeriod",
        value: function getPeriod() {
          for (var i = 1; i <= 12; i++) {
            this.PeriodList.push({
              Id: i,
              Name: i
            });
          }
        }
      }, {
        key: "getHistoricalPriceDetails",
        value: function getHistoricalPriceDetails(fuelSurchargeIndexId) {
          var _this29 = this;

          this.SelectedPeriod = [];
          this.SelectedPeriod.push({
            Id: 6,
            Name: 6
          });
          this.fuelsurchargeService.getHistoricalPrice(fuelSurchargeIndexId, 6).subscribe(function (data) {
            _this29.HistoricalPrice = data;
            _this29.HistoricalPriceDetailList = _this29.HistoricalPrice.HistoricalPriceDetails;
            _this29.SelectedFuelSurchargeIndexId = fuelSurchargeIndexId;
          });
        }
      }, {
        key: "fetchHistoricalPrice",
        value: function fetchHistoricalPrice() {
          var _this30 = this;

          this.fuelsurchargeService.getHistoricalPrice(this.SelectedFuelSurchargeIndexId, this.SelectedPeriod[0].Id).subscribe(function (data) {
            _this30.HistoricalPrice = data;
            _this30.HistoricalPriceDetailList = _this30.HistoricalPrice.HistoricalPriceDetails;
          });
        }
      }]);

      return ViewHistoricalPriceComponent;
    }();

    ViewHistoricalPriceComponent.ɵfac = function ViewHistoricalPriceComponent_Factory(t) {
      return new (t || ViewHistoricalPriceComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]));
    };

    ViewHistoricalPriceComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ViewHistoricalPriceComponent,
      selectors: [["app-view-historical-price"]],
      decls: 6,
      vars: 4,
      consts: [[1, "mx-3"], [1, "row"], ["class", "col-sm-6", 4, "ngIf"], ["class", "col-sm-6 pt-1", 4, "ngIf"], ["class", "col-sm-12 form-group", 4, "ngIf"], ["class", "col-sm-12", 4, "ngIf"], [1, "col-sm-6"], ["for", "Periods"], ["id", "Periods", 3, "ngModel", "settings", "data", "ngModelChange"], [1, "col-sm-6", "pt-1"], ["id", "fetch-historical-price", "type", "button", 1, "btn", "btn-lg", "mt-4", "btn-primary", "mb-2", "valid", 3, "click"], [1, "col-sm-12", "form-group"], [1, "alert", "alert-info"], [1, "col-sm-12"], [1, "table", "table-bordered"], [1, "table"], ["width", "47%", 1, "text-center"], ["width", "48%", 1, "text-center"], ["id", "historyTable", 2, "max-height", "300px", "overflow", "auto"], [1, "table", "table-bordered", "table-hover", "mb0"], [4, "ngIf"], [4, "ngFor", "ngForOf"], ["width", "50%", 1, "text-center", "vmiddle"], ["width", "50%"], [1, "input-group"], [1, "input-group-append"], [1, "input-group-text"], [1, "p-2", "border", "px-4"]],
      template: function ViewHistoricalPriceComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ViewHistoricalPriceComponent_div_2_Template, 4, 4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, ViewHistoricalPriceComponent_div_3_Template, 3, 0, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, ViewHistoricalPriceComponent_div_4_Template, 5, 2, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ViewHistoricalPriceComponent_div_5_Template, 29, 4, "div", 5);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.HistoricalPrice == null ? null : ctx.HistoricalPrice.IndexType) == 1);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_4__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgModel"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvVmlldy92aWV3LWhpc3RvcmljYWwtcHJpY2Uvdmlldy1oaXN0b3JpY2FsLXByaWNlLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ViewHistoricalPriceComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-view-historical-price',
          templateUrl: './view-historical-price.component.html',
          styleUrls: ['./view-historical-price.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/fuelsurcharge.module.ts": function srcAppFuelsurchargeFuelsurchargeModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelsurchargeModule", function () {
      return FuelsurchargeModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _master_master_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./master/master.component */
    "./src/app/fuelsurcharge/master/master.component.ts");
    /* harmony import */


    var _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ./Create/create-fuel-surcharge.component */
    "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts");
    /* harmony import */


    var _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./View/view-fuel-surcharge.component */
    "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component */
    "./src/app/fuelsurcharge/View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component.ts");
    /* harmony import */


    var _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./View/view-historical-price/view-historical-price.component */
    "./src/app/fuelsurcharge/View/view-historical-price/view-historical-price.component.ts");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");

    var route = [{
      path: '',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"]
    }, {
      path: 'CreateNew',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"]
    }, {
      path: 'CreateNew/:fuelsurchargeId',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"]
    }];

    var FuelsurchargeModule = function FuelsurchargeModule() {
      _classCallCheck(this, FuelsurchargeModule);
    };

    FuelsurchargeModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: FuelsurchargeModule
    });
    FuelsurchargeModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function FuelsurchargeModule_Factory(t) {
        return new (t || FuelsurchargeModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route), angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](FuelsurchargeModule, {
        declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"], _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__["CreateFuelSurchargeComponent"], _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__["ViewFuelSurchargeComponent"], _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__["ViewFuelSurchargePricingdetailsComponent"], _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__["ViewHistoricalPriceComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FuelsurchargeModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"], _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_6__["CreateFuelSurchargeComponent"], _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_7__["ViewFuelSurchargeComponent"], _View_view_fuel_surcharge_pricingdetails_view_fuel_surcharge_pricingdetails_component__WEBPACK_IMPORTED_MODULE_10__["ViewFuelSurchargePricingdetailsComponent"], _View_view_historical_price_view_historical_price_component__WEBPACK_IMPORTED_MODULE_11__["ViewHistoricalPriceComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route), angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["AngularMultiSelectModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/master/master.component.ts": function srcAppFuelsurchargeMasterMasterComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MasterComponent", function () {
      return MasterComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../Create/create-fuel-surcharge.component */
    "./src/app/fuelsurcharge/Create/create-fuel-surcharge.component.ts");
    /* harmony import */


    var _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../View/view-fuel-surcharge.component */
    "./src/app/fuelsurcharge/View/view-fuel-surcharge.component.ts");

    function MasterComponent_app_create_fuel_surcharge_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-create-fuel-surcharge");
      }
    }

    function MasterComponent_app_view_fuel_surcharge_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-view-fuel-surcharge");
      }
    }

    var MasterComponent = /*#__PURE__*/function () {
      function MasterComponent(router, fuelsurchargeService) {
        _classCallCheck(this, MasterComponent);

        this.router = router;
        this.fuelsurchargeService = fuelsurchargeService;
        this.viewType = 0;
      }

      _createClass(MasterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this31 = this;

          var _viewType = localStorage.getItem("fuelSurchargeTabId");

          if (_viewType && +_viewType > 0) {
            this.viewType = +_viewType;
          }

          this.fuelsurchargeService.onSelectedTabChanged.subscribe(function (s) {
            if (s == 2) {
              _this31.viewType = 2;
            } else {
              _this31.viewType = 1;
            }
          });
          this.viewType = +_viewType;
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.changeViewType(this.viewType);
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(value) {
          localStorage.setItem("fuelSurchargeTabId", value.toString());
          this.viewType = value;
          this.fuelsurchargeService.onSelectedFuelSurchargeId.next(null);
          this.fuelsurchargeService.onSelectedTabChanged.next(value); //if(this.viewType==1)
          //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew']);
        }
      }]);

      return MasterComponent;
    }();

    MasterComponent.ɵfac = function MasterComponent_Factory(t) {
      return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]));
    };

    MasterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: MasterComponent,
      selectors: [["app-master"]],
      decls: 16,
      vars: 8,
      consts: [[1, "row"], [1, "col-sm-12"], [1, "col-sm-4"], [1, "d-inline-block", "border", "bg-white", "p-1", "radius-capsule", "shadow-b", "mb-2"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [4, "ngIf"]],
      template: function MasterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_7_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Create Fuel Surcharge");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_10_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "View Fuel Surcharge");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, MasterComponent_app_create_fuel_surcharge_14_Template, 1, 0, "app-create-fuel-surcharge", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, MasterComponent_app_view_fuel_surcharge_15_Template, 1, 0, "app-view-fuel-surcharge", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 1)("checked", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 2);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _Create_create_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_4__["CreateFuelSurchargeComponent"], _View_view_fuel_surcharge_component__WEBPACK_IMPORTED_MODULE_5__["ViewFuelSurchargeComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Z1ZWxzdXJjaGFyZ2UvbWFzdGVyL21hc3Rlci5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-master',
          templateUrl: './master.component.html',
          styleUrls: ['./master.component.css']
        }]
      }], function () {
        return [{
          type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]
        }, {
          type: _services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_2__["FuelSurchargeService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/models/CreateFuelSurcharge.ts": function srcAppFuelsurchargeModelsCreateFuelSurchargeTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargeTableModel", function () {
      return FuelSurchargeTableModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargeIndexViewModel", function () {
      return FuelSurchargeIndexViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargeInputModel", function () {
      return FuelSurchargeInputModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargeGridModel", function () {
      return FuelSurchargeGridModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargePricingModel", function () {
      return FuelSurchargePricingModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "HistoricalPriceModel", function () {
      return HistoricalPriceModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "HistoricalPriceDetailsModel", function () {
      return HistoricalPriceDetailsModel;
    });

    var FuelSurchargeTableModel = function FuelSurchargeTableModel() {
      var values = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck(this, FuelSurchargeTableModel);

      Object.assign(this, values);
    };

    var FuelSurchargeIndexViewModel = function FuelSurchargeIndexViewModel() {
      _classCallCheck(this, FuelSurchargeIndexViewModel);

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
    };

    var FuelSurchargeInputModel = function FuelSurchargeInputModel() {
      _classCallCheck(this, FuelSurchargeInputModel);
    };

    var FuelSurchargeGridModel = function FuelSurchargeGridModel() {
      _classCallCheck(this, FuelSurchargeGridModel);
    };

    var FuelSurchargePricingModel = function FuelSurchargePricingModel() {
      _classCallCheck(this, FuelSurchargePricingModel);
    };

    var HistoricalPriceModel = function HistoricalPriceModel() {
      _classCallCheck(this, HistoricalPriceModel);
    };

    var HistoricalPriceDetailsModel = function HistoricalPriceDetailsModel() {
      _classCallCheck(this, HistoricalPriceDetailsModel);
    };
    /***/

  }
}]);
//# sourceMappingURL=fuelsurcharge-fuelsurcharge-module-es5.js.map