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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["create-fuel-group-create-fuel-group-module"], {
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
  "./src/app/create-fuel-group/create-fuel-group.component.ts": function srcAppCreateFuelGroupCreateFuelGroupComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateFuelGroupComponent", function () {
      return CreateFuelGroupComponent;
    });
    /* harmony import */


    var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! tslib */
    "./node_modules/tslib/tslib.es6.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _self_service_alias_models_FuelGroupGridViewModel__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../self-service-alias/models/FuelGroupGridViewModel */
    "./src/app/self-service-alias/models/FuelGroupGridViewModel.ts");
    /* harmony import */


    var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../self-service-alias/service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");

    function CreateFuelGroupComponent_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](1, "span", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_22_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_22_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r1.rcForm.get("GroupName").errors.required);
      }
    }

    function CreateFuelGroupComponent_div_23_div_7_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_23_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_23_div_7_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r6.rcForm.controls.GroupTypeCustom.get("TableTypes").errors.required);
      }
    }

    function CreateFuelGroupComponent_div_23_div_15_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_23_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_23_div_15_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r7.rcForm.controls.GroupTypeCustom.get("Customers").errors.required);
      }
    }

    function CreateFuelGroupComponent_div_23_div_23_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_23_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_23_div_23_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r8.rcForm.controls.GroupTypeCustom.get("Carriers").errors.required);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    function CreateFuelGroupComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](2, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](3, "Custom Fuel Group for");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](4, "span", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](5, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](6, "ng-multiselect-dropdown", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](7, CreateFuelGroupComponent_div_23_div_7_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](8, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](9, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](10, "label", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](11, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](12, "span", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](13, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](14, "ng-multiselect-dropdown", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](15, CreateFuelGroupComponent_div_23_div_15_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](16, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](17, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](18, "label", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](19, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](20, "span", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](21, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](22, "ng-multiselect-dropdown", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](23, CreateFuelGroupComponent_div_23_div_23_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx_r2.IsEditable)("placeholder", "Select group for")("settings", ctx_r2.SingleSelectSettingsById)("data", ctx_r2.TableTypeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r2.rcForm.controls.GroupTypeCustom.get("TableTypes").invalid && ctx_r2.rcForm.controls.GroupTypeCustom.get("TableTypes").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpureFunction1"](17, _c0, !ctx_r2.IsCustomerSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx_r2.IsEditable)("placeholder", "Select Customer")("settings", ctx_r2.SingleSelectSettingsById)("data", ctx_r2.CustomerList);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r2.IsCustomerSelected && ctx_r2.rcForm.controls.GroupTypeCustom.get("Customers").invalid && ctx_r2.rcForm.controls.GroupTypeCustom.get("Customers").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpureFunction1"](19, _c0, !ctx_r2.IsCarrierSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx_r2.IsEditable)("placeholder", "Select Carrier")("settings", ctx_r2.SingleSelectSettingsById)("data", ctx_r2.CarrierList);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r2.IsCarrierSelected && ctx_r2.rcForm.controls.GroupTypeCustom.get("Carriers").invalid && ctx_r2.rcForm.controls.GroupTypeCustom.get("Carriers").touched);
      }
    }

    function CreateFuelGroupComponent_div_32_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_32_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.controls.GroupTypeStandard.get("ProductTypes").errors.required);
      }
    }

    function CreateFuelGroupComponent_div_40_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }
    }

    function CreateFuelGroupComponent_div_40_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, CreateFuelGroupComponent_div_40_div_1_Template, 2, 0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r4.rcForm.controls.GroupTypeStandard.get("FuelTypes").errors.required);
      }
    }

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var CreateFuelGroupComponent = /*#__PURE__*/function () {
      function CreateFuelGroupComponent(httpService, _fb, fuelSurchargeService, RegionService, externalMappingsService) {
        _classCallCheck(this, CreateFuelGroupComponent);

        this.httpService = httpService;
        this._fb = _fb;
        this.fuelSurchargeService = fuelSurchargeService;
        this.RegionService = RegionService;
        this.externalMappingsService = externalMappingsService;
        this.result = new _angular_core__WEBPACK_IMPORTED_MODULE_2__["EventEmitter"]();
        this.nativeServiceCall = true;
        this.ProductTypeList = [];
        this.FuelTypeList = [];
        this.TableTypeList = [];
        this.CustomerList = [];
        this.CarrierList = [];
        this.IsCustomSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.IsEditable = true;
        this.IsProductTypeSelected = false;
        this.createFuelGroupUrl = '/FuelGroup/CreateFuelGroup';
        this.updateFuelGroupUrl = '/FuelGroup/UpdateFuelGroup';
        this.SingleSelectSettingsById = {};
      }

      _createClass(CreateFuelGroupComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this2 = this;

          this.isLoadingSubject = new rxjs__WEBPACK_IMPORTED_MODULE_5__["BehaviorSubject"](false);
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
          this.init();
          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.controls['GroupTypeCustom'].get('TableTypes').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["pairwise"])()).subscribe(function (_ref) {
            var _ref2 = _slicedToArray(_ref, 2),
                prev = _ref2[0],
                next = _ref2[1];

            if (JSON.stringify(prev) != JSON.stringify(next)) _this2.onTableTypeSelect(prev, next);
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["pairwise"])()).subscribe(function (_ref3) {
            var _ref4 = _slicedToArray(_ref3, 2),
                prev = _ref4[0],
                next = _ref4[1];

            if (JSON.stringify(prev) != JSON.stringify(next)) _this2.onProductTypeChange(prev, next);
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.controls['GroupTypeCustom'].get('Customers').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["pairwise"])()).subscribe(function (_ref5) {
            var _ref6 = _slicedToArray(_ref5, 2),
                prev = _ref6[0],
                next = _ref6[1];

            if (JSON.stringify(prev) != JSON.stringify(next)) _this2.onCustomerSelect(prev, next);
          });
          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.controls['GroupTypeCustom'].get('Carriers').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_6__["pairwise"])()).subscribe(function (_ref7) {
            var _ref8 = _slicedToArray(_ref7, 2),
                prev = _ref8[0],
                next = _ref8[1];

            if (JSON.stringify(prev) != JSON.stringify(next)) _this2.onCarrierSelect(prev, next);
          });
          this.getProductTypeList();
          this.getTableTypeList();
          this.getCustomerList();
          this.getCarrierList();
        }
      }, {
        key: "init",
        value: function init() {
          this.rcForm = this._fb.group({
            Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](''),
            GroupName: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            FuelGroupType: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](1),
            GroupTypeStandard: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroup"]({
              ProductTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](''),
              FuelTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"]('') //select top 1 * from MstTfxProducts --  fuel type

            }),
            GroupTypeCustom: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroup"]({
              TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](''),
              Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](''),
              Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"]('')
            }),
            FreightTableStatus: ['']
          });
        }
      }, {
        key: "onCustomerSelect",
        value: function onCustomerSelect(prev, item) {
          this.isLoadingSubject.next(true);
          this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
          this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
          this.isLoadingSubject.next(false);
        }
      }, {
        key: "onCarrierSelect",
        value: function onCarrierSelect(prev, item) {
          this.isLoadingSubject.next(true);
          this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
          this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
          this.isLoadingSubject.next(false);
        }
      }, {
        key: "onTableTypeSelect",
        value: function onTableTypeSelect(prev, item) {
          this.IsCarrierSelected = false;
          this.IsCustomerSelected = false;
          this.isLoadingSubject.next(true);
          this.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
          this.CustomerList = [];
          this.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
          this.CarrierList = [];
          this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
          this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);

          if (item != null && item != "" && item != undefined && item.length == 1) {
            if (item[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["TableType"].Customer) {
              this.IsCustomerSelected = true;
              this.getCustomerList();
            } else if (item[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["TableType"].Carrier) {
              this.IsCarrierSelected = true;
              this.getCarrierList();
            }
          }

          this.isLoadingSubject.next(false);
        }
      }, {
        key: "FuelGroupTypeChange",
        value: function FuelGroupTypeChange() {
          this.isLoadingSubject.next(true);
          this.IsProductTypeSelected = false;
          this.rcForm.controls['GroupTypeCustom'].get('TableTypes').patchValue([]);
          this.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
          this.CustomerList = [];
          this.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
          this.CarrierList = [];
          this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]); //this.ProductTypeList = [];

          this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
          this.FuelTypeList = [];
          this.rcForm.controls['GroupName'].patchValue(null);
          this.rcForm.controls['Id'].patchValue(null);
          this.ViewOrEdit = "Create";
          this.isLoadingSubject.next(false);
        }
      }, {
        key: "onProductTypeChange",
        value: function onProductTypeChange(prev, next) {
          var _this3 = this;

          if (!this.nativeServiceCall) {
            this.IsProductTypeSelected = true;
            return;
          }

          this.isLoadingSubject.next(true);
          var selProductTypes = this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').value;

          if (selProductTypes != null && selProductTypes != undefined && selProductTypes.length > 0) {
            this.IsProductTypeSelected = true;
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            var editingcompanyId = -1;
            var cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
            var car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;

            if (cus != null && cus.length > 0) {
              editingcompanyId = cus[0].Id;
            } else if (car != null && car.length > 0) {
              editingcompanyId = car[0].Id;
            }

            this.externalMappingsService.getFuelTypeList(selProductTypes.map(function (s) {
              return s.Id;
            }).join(','), this.rcForm.controls.FuelGroupType.value, +this.rcForm.controls.Id.value, editingcompanyId).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this3, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
                return regeneratorRuntime.wrap(function _callee$(_context) {
                  while (1) {
                    switch (_context.prev = _context.next) {
                      case 0:
                        _context.next = 2;
                        return data;

                      case 2:
                        this.FuelTypeList = _context.sent;
                        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(function (r) {
                          return !r.isDisabled;
                        }));
                        this.isLoadingSubject.next(false);

                      case 5:
                      case "end":
                        return _context.stop();
                    }
                  }
                }, _callee, this);
              }));
            });
          } else {
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            this.FuelTypeList = [];
            this.IsProductTypeSelected = false;
          }

          this.isLoadingSubject.next(false);
        }
      }, {
        key: "getProductTypeList",
        value: function getProductTypeList() {
          var _this4 = this;

          this.isLoadingSubject.next(true);
          this.externalMappingsService.getProductTypeList().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this4, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
              return regeneratorRuntime.wrap(function _callee2$(_context2) {
                while (1) {
                  switch (_context2.prev = _context2.next) {
                    case 0:
                      _context2.next = 2;
                      return data;

                    case 2:
                      this.ProductTypeList = _context2.sent;
                      this.isLoadingSubject.next(false);

                    case 4:
                    case "end":
                      return _context2.stop();
                  }
                }
              }, _callee2, this);
            }));
          });
        }
      }, {
        key: "getFuelTypeList",
        value: function getFuelTypeList(productTypeIds, fuelGroupType) {
          var _this5 = this;

          this.isLoadingSubject.next(true);
          this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
          var editingcompanyId = -1;
          var cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
          var car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;

          if (cus != null && cus.length > 0) {
            editingcompanyId = cus[0].Id;
          } else if (car != null && car.length > 0) {
            editingcompanyId = car[0].Id;
          }

          this.externalMappingsService.getFuelTypeList(productTypeIds, fuelGroupType, +this.rcForm.controls.Id.value, editingcompanyId).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this5, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
              return regeneratorRuntime.wrap(function _callee3$(_context3) {
                while (1) {
                  switch (_context3.prev = _context3.next) {
                    case 0:
                      _context3.next = 2;
                      return data;

                    case 2:
                      this.FuelTypeList = _context3.sent;
                      this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(function (r) {
                        return !r.isDisabled;
                      }));
                      this.isLoadingSubject.next(false);

                    case 5:
                    case "end":
                      return _context3.stop();
                  }
                }
              }, _callee3, this);
            }));
          });
        }
      }, {
        key: "getTableTypeList",
        value: function getTableTypeList() {
          var _this6 = this;

          if (!this.nativeServiceCall) {
            return;
          }

          this.isLoadingSubject.next(true);
          this.fuelSurchargeService.getTableTypes().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this6, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
              var tblTypeList;
              return regeneratorRuntime.wrap(function _callee4$(_context4) {
                while (1) {
                  switch (_context4.prev = _context4.next) {
                    case 0:
                      _context4.next = 2;
                      return data;

                    case 2:
                      tblTypeList = _context4.sent;
                      this.TableTypeList = tblTypeList.filter(function (x) {
                        return x.Id != 1;
                      }); // no master included.

                      this.TableTypeList.forEach(function (res) {
                        return res.Name = res.Name.replace("Specific", "");
                      }); //this.rcForm.controls.GroupTypeCustom.get('TableTypes').setValue(this.TableTypeList.slice(0, 1));

                      this.isLoadingSubject.next(false);

                    case 6:
                    case "end":
                      return _context4.stop();
                  }
                }
              }, _callee4, this);
            }));
          });
        }
      }, {
        key: "getCustomerList",
        value: function getCustomerList() {
          var _this7 = this;

          if (!this.nativeServiceCall) {
            return;
          }

          this.isLoadingSubject.next(true);
          this.fuelSurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this7, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
              return regeneratorRuntime.wrap(function _callee5$(_context5) {
                while (1) {
                  switch (_context5.prev = _context5.next) {
                    case 0:
                      _context5.next = 2;
                      return data;

                    case 2:
                      this.CustomerList = _context5.sent;
                      this.isLoadingSubject.next(false);

                    case 4:
                    case "end":
                      return _context5.stop();
                  }
                }
              }, _callee5, this);
            }));
          });
        }
      }, {
        key: "getCarrierList",
        value: function getCarrierList() {
          var _this8 = this;

          if (!this.nativeServiceCall) {
            return;
          }

          this.isLoadingSubject.next(true);
          this.RegionService.getCarriers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this8, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
              return regeneratorRuntime.wrap(function _callee6$(_context6) {
                while (1) {
                  switch (_context6.prev = _context6.next) {
                    case 0:
                      _context6.next = 2;
                      return data;

                    case 2:
                      this.CarrierList = _context6.sent;
                      this.isLoadingSubject.next(false);

                    case 4:
                    case "end":
                      return _context6.stop();
                  }
                }
              }, _callee6, this);
            }));
          });
        }
      }, {
        key: "markFormGroupTouched",
        value: function markFormGroupTouched(formGroup) {
          var _this9 = this;

          Object.values(formGroup.controls).forEach(function (control) {
            control.markAsTouched();

            if (control.controls) {
              _this9.markFormGroupTouched(control);
            }
          });
        }
      }, {
        key: "AddRemoveValidations",
        value: function AddRemoveValidations(requiredControls, notRequiredControls) {
          if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {
            requiredControls.forEach(function (ctrl) {
              ctrl.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]);
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
        key: "modeChangeCustomOrStandardValidators",
        value: function modeChangeCustomOrStandardValidators(fuelGroupType) {
          this.AddRemoveValidations([this.rcForm.controls['GroupTypeStandard'].get('ProductTypes'), this.rcForm.controls['GroupTypeStandard'].get('FuelTypes')], null);

          if (fuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["FuelGroupType"].Standard) {
            this.AddRemoveValidations(null, [this.rcForm.controls['GroupTypeCustom'].get('Customers'), this.rcForm.controls['GroupTypeCustom'].get('Carriers'), this.rcForm.controls['GroupTypeCustom'].get('TableTypes')]);
          }

          if (fuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["FuelGroupType"].Custom) {
            this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('TableTypes')], null);
            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == null || this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == undefined) return;

            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value[0].Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["TableType"].Customer) {
              this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Customers')], [this.rcForm.controls['GroupTypeCustom'].get('Carriers')]);
            } else {
              this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Carriers')], [this.rcForm.controls['GroupTypeCustom'].get('Customers')]);
            }
          }
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          this.modeChangeCustomOrStandardValidators(this.rcForm.controls.FuelGroupType.value);
          this.markFormGroupTouched(this.rcForm);
          if (this.rcForm.valid) this.Save();
        }
      }, {
        key: "onCancel",
        value: function onCancel() {
          this.result.emit(true);
        }
      }, {
        key: "Save",
        value: function Save() {
          var _this10 = this;

          this.isLoadingSubject.next(true);
          this.fuelGroupMapping = new _self_service_alias_models_FuelGroupGridViewModel__WEBPACK_IMPORTED_MODULE_8__["FuelGroupViewModel"]();
          this.fuelGroupMapping.Id = this.rcForm.controls.Id.value;
          this.fuelGroupMapping.GroupName = this.rcForm.controls.GroupName.value.trim();
          this.fuelGroupMapping.FuelGroupType = this.rcForm.controls.FuelGroupType.value;

          if (this.fuelGroupMapping.FuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["FuelGroupType"].Custom) {
            this.fuelGroupMapping.TableType = this.rcForm.controls.GroupTypeCustom.get('TableTypes').value[0].Id;

            if (this.fuelGroupMapping.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["TableType"].Customer) {
              this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Customers').value[0].Id;
            } else if (this.fuelGroupMapping.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["TableType"].Carrier) {
              this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Carriers').value[0].Id;
            }
          }

          var pIds = [];
          this.rcForm.controls.GroupTypeStandard.get('ProductTypes').value.forEach(function (res) {
            pIds.push(res.Id);
          });
          this.fuelGroupMapping.ProductTypeIds = pIds;
          var fIds = [];
          this.rcForm.controls.GroupTypeStandard.get('FuelTypes').value.forEach(function (res) {
            fIds.push(res.Id);
          });
          this.fuelGroupMapping.FuelTypeIds = fIds;
          this.fuelGroupMapping.FreightTableStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["FreightTableStatus"].Published;
          this.httpService.post(this.rcForm.controls.Id.value != null ? this.updateFuelGroupUrl : this.createFuelGroupUrl, this.fuelGroupMapping, httpOptions).pipe().subscribe(function (res) {
            if (res.StatusCode == 0) {
              _this10.result.emit(true);

              var message = "";

              if (_this10.fuelGroupMapping.Id != null) {
                message = "updated";
              } else if (_this10.fuelGroupMapping.FreightTableStatus == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["FreightTableStatus"].Published) {
                message = "created";
              }

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(_this10.fuelGroupMapping.GroupName + " fuel group " + message + " successfully.", undefined, undefined);
            } else src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(res == null || res.StatusMessage == null ? 'Failed' : res.StatusMessage, undefined, undefined);

            _this10.isLoadingSubject.next(false);
          });
        }
      }]);

      return CreateFuelGroupComponent;
    }();

    CreateFuelGroupComponent.ɵfac = function CreateFuelGroupComponent_Factory(t) {
      return new (t || CreateFuelGroupComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_11__["ExternalMappingsService"]));
    };

    CreateFuelGroupComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({
      type: CreateFuelGroupComponent,
      selectors: [["app-create-fuel-group"]],
      outputs: {
        result: "result"
      },
      decls: 46,
      vars: 27,
      consts: [[3, "formGroup", "ngSubmit"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "row"], [1, "col-sm-12"], [1, "form-group"], [1, "form-check", "form-check-inline"], ["type", "radio", "formControlName", "FuelGroupType", "id", "occurance-standard", 1, "form-check-input", 3, "name", "value", "change"], ["for", "occurance-standard", 1, "form-check-label"], ["type", "radio", "formControlName", "FuelGroupType", "id", "occurance-custom", 1, "form-check-input", 3, "name", "value", "change"], ["for", "occurance-custom", 1, "form-check-label"], [1, "col-sm-6", "form-group"], ["for", "groupName"], [1, "color-maroon"], ["id", "groupName", "type", "text", "formControlName", "GroupName", 1, "form-control", 3, "readonly"], ["class", "color-maroon", 4, "ngIf"], ["class", "row", "formGroupName", "GroupTypeCustom", 4, "ngIf"], ["formGroupName", "GroupTypeStandard", 1, "row"], [1, ""], ["for", "ProductTypes"], ["formControlName", "ProductTypes", "id", "ProductTypes", 3, "disabled", "placeholder", "settings", "data"], [3, "ngClass"], ["for", "FuelTypes"], ["formControlName", "FuelTypes", "id", "FuelTypes", 3, "disabled", "placeholder", "settings", "data"], [1, "col-sm-12", "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [4, "ngIf"], ["formGroupName", "GroupTypeCustom", 1, "row"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "disabled", "placeholder", "settings", "data"], ["for", "Customer"], ["formControlName", "Customers", "id", "Customer", 1, "single-select", 3, "disabled", "placeholder", "settings", "data"], ["for", "Carrier"], ["formControlName", "Carriers", "id", "Carrier", 1, "single-select", 3, "disabled", "placeholder", "settings", "data"]],
      template: function CreateFuelGroupComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](1, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngSubmit", function CreateFuelGroupComponent_Template_form_ngSubmit_1_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](2, CreateFuelGroupComponent_div_2_Template, 2, 0, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](3, "async");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](5, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](6, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](8, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("change", function CreateFuelGroupComponent_Template_input_change_8_listener() {
            return ctx.FuelGroupTypeChange();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](9, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](10, "Standard");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](11, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](12, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("change", function CreateFuelGroupComponent_Template_input_change_12_listener() {
            return ctx.FuelGroupTypeChange();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](13, "label", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](14, "Custom");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](15, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](16, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](17, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](18, "Group Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](19, "span", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](20, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](21, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](22, CreateFuelGroupComponent_div_22_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](23, CreateFuelGroupComponent_div_23_Template, 24, 21, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](24, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](25, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](26, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](27, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](28, "Product Types");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](29, "span", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](30, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](31, "ng-multiselect-dropdown", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](32, CreateFuelGroupComponent_div_32_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](33, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](34, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](35, "label", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](36, "Fuel Types");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](37, "span", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](38, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](39, "ng-multiselect-dropdown", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](40, CreateFuelGroupComponent_div_40_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](41, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](42, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](43, "input", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("click", function CreateFuelGroupComponent_Template_input_click_43_listener() {
            return ctx.onCancel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](44, "button", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](45, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("formGroup", ctx.rcForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](3, 23, ctx.isLoadingSubject));

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("name", "FuelGroupType")("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵattribute"]("disabled", !ctx.IsEditable || ctx.ViewOrEdit == "EDIT" ? false : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("name", "FuelGroupType")("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵattribute"]("disabled", !ctx.IsEditable || ctx.ViewOrEdit == "EDIT" ? false : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("readonly", ctx.ViewOrEdit == "EDIT" || ctx.ViewOrEdit == "VIEW" ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.rcForm.get("GroupName").invalid && ctx.rcForm.get("GroupName").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.rcForm.get("FuelGroupType").value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx.IsEditable)("placeholder", "Select Product Types(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.ProductTypeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.GroupTypeStandard.get("ProductTypes").invalid && ctx.rcForm.controls.GroupTypeStandard.get("ProductTypes").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpureFunction1"](25, _c0, !ctx.IsProductTypeSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx.IsEditable)("placeholder", "Select Fuel Type(s)")("settings", ctx.MultiSelectSettingsById)("data", ctx.FuelTypeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.rcForm.controls.GroupTypeStandard.get("FuelTypes").invalid && ctx.rcForm.controls.GroupTypeStandard.get("FuelTypes").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx.IsEditable);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_12__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["MultiSelectComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_12__["NgClass"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_12__["AsyncPipe"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NyZWF0ZS1mdWVsLWdyb3VwL2NyZWF0ZS1mdWVsLWdyb3VwLmNvbXBvbmVudC5jc3MifQ== */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵsetClassMetadata"](CreateFuelGroupComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_2__["Component"],
        args: [{
          selector: 'app-create-fuel-group',
          templateUrl: './create-fuel-group.component.html',
          styleUrls: ['./create-fuel-group.component.css'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_2__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]
        }, {
          type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]
        }, {
          type: _self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_11__["ExternalMappingsService"]
        }];
      }, {
        result: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_2__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/create-fuel-group/create-fuel-group.module.ts": function srcAppCreateFuelGroupCreateFuelGroupModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateFuelGroupModule", function () {
      return CreateFuelGroupModule;
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


    var _create_fuel_group_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./create-fuel-group.component */
    "./src/app/create-fuel-group/create-fuel-group.component.ts");
    /* harmony import */


    var _fuel_group_mapping_fuel_group_mapping_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./fuel-group-mapping/fuel-group-mapping.component */
    "./src/app/create-fuel-group/fuel-group-mapping/fuel-group-mapping.component.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");

    var routes = [{
      path: "",
      component: _fuel_group_mapping_fuel_group_mapping_component__WEBPACK_IMPORTED_MODULE_3__["FuelGroupMappingComponent"]
    }];

    var CreateFuelGroupModule = function CreateFuelGroupModule() {
      _classCallCheck(this, CreateFuelGroupModule);
    };

    CreateFuelGroupModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: CreateFuelGroupModule
    });
    CreateFuelGroupModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function CreateFuelGroupModule_Factory(t) {
        return new (t || CreateFuelGroupModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ReactiveFormsModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["NgMultiSelectDropDownModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_9__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"].forChild(routes)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](CreateFuelGroupModule, {
        declarations: [_create_fuel_group_component__WEBPACK_IMPORTED_MODULE_2__["CreateFuelGroupComponent"], _fuel_group_mapping_fuel_group_mapping_component__WEBPACK_IMPORTED_MODULE_3__["FuelGroupMappingComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ReactiveFormsModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["NgMultiSelectDropDownModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_9__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateFuelGroupModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_create_fuel_group_component__WEBPACK_IMPORTED_MODULE_2__["CreateFuelGroupComponent"], _fuel_group_mapping_fuel_group_mapping_component__WEBPACK_IMPORTED_MODULE_3__["FuelGroupMappingComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ReactiveFormsModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["NgMultiSelectDropDownModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_9__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"].forChild(routes)]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/create-fuel-group/fuel-group-mapping/fuel-group-mapping.component.ts": function srcAppCreateFuelGroupFuelGroupMappingFuelGroupMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelGroupMappingComponent", function () {
      return FuelGroupMappingComponent;
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


    var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _create_fuel_group_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../create-fuel-group.component */
    "./src/app/create-fuel-group/create-fuel-group.component.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/self-service-alias/service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    var _c0 = ["openSidePannel"];
    var _c1 = ["btnCloseBulkUploadPopup"];

    function FuelGroupMappingComponent_tr_32_a_16_Template(rf, ctx) {
      if (rf & 1) {
        var _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("cancel", function FuelGroupMappingComponent_tr_32_a_16_Template_a_cancel_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r7);

          var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r6.cancelClicked = true;
        })("confirm", function FuelGroupMappingComponent_tr_32_a_16_Template_a_confirm_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r7);

          var item_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r8.archiveFuelGroup(item_r3);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("popoverTitle", ctx_r4.popoverTitle)("popoverMessage", ctx_r4.popoverMessage);
      }
    }

    function FuelGroupMappingComponent_tr_32_a_17_Template(rf, ctx) {
      if (rf & 1) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FuelGroupMappingComponent_tr_32_a_17_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var item_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r10.editFuelGroupMapping(item_r3.Id, "EDIT");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function FuelGroupMappingComponent_tr_32_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, FuelGroupMappingComponent_tr_32_a_16_Template, 2, 2, "a", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, FuelGroupMappingComponent_tr_32_a_17_Template, 2, 0, "a", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "a", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FuelGroupMappingComponent_tr_32_Template_a_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var item_r3 = ctx.$implicit;

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r13.editFuelGroupMapping(item_r3.Id, "VIEW");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](19, "i", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.GroupName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.FuelGroupType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.TableType != 0 ? item_r3.TableType : "-");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.Customer);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.Carrier);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r3.StatusName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", item_r3.StatusName != "Archived");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", item_r3.StatusName != "Archived");
      }
    }

    function FuelGroupMappingComponent_div_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var FuelGroupMappingComponent = /*#__PURE__*/function () {
      function FuelGroupMappingComponent(httpService, carrierService, externalMappingsService, fuelSurchargeService, RegionService) {
        _classCallCheck(this, FuelGroupMappingComponent);

        this.httpService = httpService;
        this.carrierService = carrierService;
        this.externalMappingsService = externalMappingsService;
        this.fuelSurchargeService = fuelSurchargeService;
        this.RegionService = RegionService;
        this.title = 'Create';
        this.fuelGroupGridList = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.IsValidForm = true;
        this.popoverTitle = 'Archive Confirmation';
        this.popoverMessage = 'Do you want to archive?';
        this.cancelClicked = false;
        this.confirmClicked = false;
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.IsShowBulkUploadPopup = false;
        this.SelectedFiles = [];
        this.MaxFileUploadSize = 1048576; // 1MB

        this.GroupId = -1;
      }

      _createClass(FuelGroupMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Fuel Group Mapping',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Fuel Group Mapping',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
          this.getFuelGroupMapping();
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#fuel-group-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.dtTrigger.next();
        }
      }, {
        key: "getFuelGroupMapping",
        value: function getFuelGroupMapping() {
          var _this11 = this;

          this.IsLoading = true;
          this.externalMappingsService.getFuelGroupSummary().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this11, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee7() {
              return regeneratorRuntime.wrap(function _callee7$(_context7) {
                while (1) {
                  switch (_context7.prev = _context7.next) {
                    case 0:
                      this.IsLoading = false;
                      _context7.next = 3;
                      return data;

                    case 3:
                      this.fuelGroupGridList = _context7.sent;
                      this.datatableRerender();

                    case 5:
                    case "end":
                      return _context7.stop();
                  }
                }
              }, _callee7, this);
            }));
          });
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          var _this12 = this;

          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();

              _this12.dtTrigger.next();
            });
          }
        }
      }, {
        key: "IdInComparer",
        value: function IdInComparer(otherArray) {
          return function (current) {
            return otherArray.filter(function (other) {
              //console.log(other + ":" + current.Id);
              return other == current.Id;
            }).length == 1;
          };
        }
      }, {
        key: "editFuelGroupMapping",
        value: function editFuelGroupMapping(groupId, mode) {
          var _this13 = this;

          this.title = mode;
          this.createFuelGroupComponent.isLoadingSubject.next(false);
          this.createFuelGroupComponent.nativeServiceCall = false;
          this.httpService.get(this.externalMappingsService.getFuelGroupDetailsUrl + groupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["map"])(function (item) {
            _this13.IsLoading = true;
            return item;
          }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["mergeMap"])(function (item) {
            _this13.fuelGroupEdit = item;
            var getFuelTypesUrl = "";

            if (item.FuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["FuelGroupType"].Standard) {
              getFuelTypesUrl = _this13.externalMappingsService.getFuelTypesUrl + _this13.fuelGroupEdit.ProductTypeIds + "&fuelGroupType=" + _this13.fuelGroupEdit.FuelGroupType + "&editingGroupId=-1" + "&editingcompanyId=-1";
            } else {
              getFuelTypesUrl = _this13.externalMappingsService.getFuelTypesUrl + _this13.fuelGroupEdit.ProductTypeIds + "&fuelGroupType=" + _this13.fuelGroupEdit.FuelGroupType + "&editingGroupId=" + _this13.fuelGroupEdit.Id + "&editingcompanyId=" + _this13.fuelGroupEdit.AssignedCompanyId;
            }

            var getTableTypesUrl = "/FuelSurcharge/GetTableTypes";

            var productTypes = _this13.httpService.get(_this13.externalMappingsService.getProductTypeUrl);

            var fuelTypes = _this13.httpService.get(getFuelTypesUrl);

            var customers = _this13.httpService.get(_this13.fuelSurchargeService.getSupplierCustomersUrl);

            var carriers = _this13.httpService.get(_this13.RegionService.getCarriersUrl);

            var Tabletypes = _this13.httpService.get(getTableTypesUrl);

            return Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["forkJoin"])([fuelTypes, customers, carriers, productTypes, Tabletypes]);
          })).subscribe(function (result) {
            _this13.IsLoading = false; //this.createFuelGroupComponent.rcForm.reset();

            _this13.createFuelGroupComponent.isLoadingSubject.next(true);

            _this13.createFuelGroupComponent.rcForm.controls.Id.setValue(_this13.fuelGroupEdit.Id);

            _this13.createFuelGroupComponent.ViewOrEdit = mode;

            _this13.createFuelGroupComponent.rcForm.controls.GroupName.setValue(_this13.fuelGroupEdit.GroupName);

            if (_this13.fuelGroupEdit.FuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["FuelGroupType"].Custom) {
              var tblTypeList = result[4];
              tblTypeList = tblTypeList.filter(function (x) {
                return x.Id != 1;
              }); // no master included.

              tblTypeList.forEach(function (res) {
                return res.Name = res.Name.replace("Specific", "");
              });
              _this13.createFuelGroupComponent.TableTypeList = tblTypeList;

              _this13.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('TableTypes').setValue(_this13.createFuelGroupComponent.TableTypeList.filter(function (t) {
                return t.Id == _this13.fuelGroupEdit.TableType;
              })); //this.createFuelGroupComponent.onTableTypeSelect({ Id: this.fuelGroupEdit.TableType });


              _this13.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);

              _this13.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);

              _this13.createFuelGroupComponent.IsCustomerSelected = false;
              _this13.createFuelGroupComponent.IsCarrierSelected = false;

              _this13.createFuelGroupComponent.isLoadingSubject.next(true);

              if (_this13.fuelGroupEdit.TableType != null && _this13.fuelGroupEdit.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["TableType"].Customer) {
                _this13.createFuelGroupComponent.IsCustomerSelected = true;
                _this13.createFuelGroupComponent.CustomerList = result[1];

                _this13.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').setValue(_this13.createFuelGroupComponent.CustomerList.filter(function (x) {
                  return x.Id == _this13.fuelGroupEdit.AssignedCompanyId;
                }));
              } else if (_this13.fuelGroupEdit.TableType != null && _this13.fuelGroupEdit.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["TableType"].Carrier) {
                _this13.createFuelGroupComponent.IsCarrierSelected = true;
                _this13.createFuelGroupComponent.CarrierList = result[2];

                _this13.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').setValue(_this13.createFuelGroupComponent.CarrierList.filter(function (x) {
                  return x.Id == _this13.fuelGroupEdit.AssignedCompanyId;
                }));
              }
            }

            _this13.createFuelGroupComponent.rcForm.controls.FuelGroupType.setValue(_this13.fuelGroupEdit.FuelGroupType);

            _this13.createFuelGroupComponent.ProductTypeList = result[3];

            _this13.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('ProductTypes').setValue(_this13.createFuelGroupComponent.ProductTypeList.filter(_this13.IdInComparer(_this13.fuelGroupEdit.ProductTypeIds)));

            _this13.createFuelGroupComponent.IsProductTypeSelected = true;
            _this13.IsLoading = true;
            var ps = result[0];

            if (_this13.fuelGroupEdit.FuelGroupType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["FuelGroupType"].Custom) {
              ps.filter(_this13.IdInComparer(_this13.fuelGroupEdit.FuelTypeIds));
            } else {
              ps.filter(_this13.IdInComparer(_this13.fuelGroupEdit.FuelTypeIds)).forEach(function (res) {
                return res.isDisabled = false;
              }); // not all make isDisable false.
            }

            _this13.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);

            _this13.createFuelGroupComponent.FuelTypeList = ps;

            var fueltyps = _this13.createFuelGroupComponent.FuelTypeList.filter(_this13.IdInComparer(_this13.fuelGroupEdit.FuelTypeIds));

            _this13.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(fueltyps);

            _this13.createFuelGroupComponent.isLoadingSubject.next(true);

            _this13.notEditableForm(mode);

            _this13.IsLoading = false;

            _this13.createFuelGroupComponent.isLoadingSubject.next(false);

            _this13.createFuelGroupComponent.nativeServiceCall = true;
          });
        }
      }, {
        key: "notEditableForm",
        value: function notEditableForm(mode) {
          this.createFuelGroupComponent.IsEditable = true;

          if (mode == "VIEW") {
            this.createFuelGroupComponent.IsEditable = false;
          } //this.createFuelGroupComponent.rcForm.controls.GroupName.disable({ onlySelf: isEdit });
          //this.createFuelGroupComponent.rcForm.controls.FuelGroupType.disable({onlySelf: false });
          //this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('ProductTypes').disable({ onlySelf: isEdit });
          //this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').disable({ onlySelf: isEdit });
          //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('TableTypes').disable({ onlySelf: isEdit });
          //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').disable({ onlySelf: isEdit });
          //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').disable({ onlySelf: isEdit });
          //console.log(mode  + " : " + isEdit);

        }
      }, {
        key: "addFuelGroup",
        value: function addFuelGroup() {
          this.title = 'Create';
          this.createFuelGroupComponent.rcForm.reset();
          this.createFuelGroupComponent.IsEditable = true;
          this.createFuelGroupComponent.IsCarrierSelected = false;
          this.createFuelGroupComponent.IsCustomerSelected = false;
          this.createFuelGroupComponent.rcForm.controls['Id'].patchValue(null);
          this.createFuelGroupComponent.ViewOrEdit = "Create";
          this.createFuelGroupComponent.rcForm.controls.FuelGroupType.setValue(src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["FuelGroupType"].Standard);
          this.createFuelGroupComponent.isLoadingSubject.next(false);
        }
      }, {
        key: "getOutput",
        value: function getOutput($event) {
          if ($event) {
            this.openSidePannel.nativeElement.click();
            this.getFuelGroupMapping();
          }
        }
      }, {
        key: "archiveFuelGroup",
        value: function archiveFuelGroup(item) {
          var _this14 = this;

          this.IsLoading = true;
          this.externalMappingsService.archiveFuelGroup(item.Id.toString()).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this14, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee8() {
              var result;
              return regeneratorRuntime.wrap(function _callee8$(_context8) {
                while (1) {
                  switch (_context8.prev = _context8.next) {
                    case 0:
                      _context8.next = 2;
                      return data;

                    case 2:
                      result = _context8.sent;

                      if (result.StatusCode == 0) {
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(item.GroupName + " fuel group archived successfully.", undefined, undefined);
                        this.getFuelGroupMapping();
                      } else src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(result.StatusMessage, undefined, undefined);

                      this.IsLoading = false;

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
        key: "selectFiles",
        value: function selectFiles(files) {
          if (files != null && files != undefined && files.length > 0) this.SelectedFiles = files;
        }
      }, {
        key: "isValidFile",
        value: function isValidFile(file) {
          var isValid = true;
          var extension = this.getExtension(file.name);

          if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
          }

          if (file.size > this.MaxFileUploadSize) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
          }

          return isValid;
        }
      }, {
        key: "downloadCsvTemplate",
        value: function downloadCsvTemplate() {
          var _this15 = this;

          this.IsLoading = true;
          var timestamp = new Date().getTime();
          this.carrierService.downloadTerminalItemCodeMappingTemplate(timestamp).subscribe(function (blob) {
            var a = document.createElement('a');
            var objectUrl = URL.createObjectURL(blob);
            a.href = objectUrl;
            a.download = 'FuelGroupMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);
            _this15.IsLoading = false;
          });
        }
      }, {
        key: "getExtension",
        value: function getExtension(fileName) {
          // extract file name from full path ...
          var basename = fileName.split(/[\\/]/).pop(); // (supports `\\` and `/` separators)

          var pos = basename.lastIndexOf("."); // get last position of `.`

          if (basename === "" || pos < 1) // if file name is empty or ...
            return ""; //  `.` not found (-1) or comes first (0)

          return basename.slice(pos + 1); // extract extension ignoring `.`
        }
      }]);

      return FuelGroupMappingComponent;
    }();

    FuelGroupMappingComponent.ɵfac = function FuelGroupMappingComponent_Factory(t) {
      return new (t || FuelGroupMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_8__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_10__["ExternalMappingsService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]));
    };

    FuelGroupMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: FuelGroupMappingComponent,
      selectors: [["app-fuel-group-mapping"]],
      viewQuery: function FuelGroupMappingComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c1, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_create_fuel_group_component__WEBPACK_IMPORTED_MODULE_7__["CreateFuelGroupComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.openSidePannel = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.CloseBulkUploadPopup = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.createFuelGroupComponent = _t.first);
        }
      },
      decls: 44,
      vars: 7,
      consts: [[1, "row"], [1, "col-sm-12"], ["type", "button", "data-target", "raisedr", "onclick", "slidePanel('#Fuel_Group_Id','45%')", 1, "btn", "btn-link", "float-left", "pa0", "mb-2", 3, "click"], ["openSidePannel", ""], [1, "fas", "fa-plus-circle", "fs18", "mr5"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-fuel-group-code", 1, "table-responsive"], ["datatable", "", 1, "table", "table-bordered", 3, "dtOptions", "dtTrigger"], ["data-key", "fgn"], ["data-key", "fgt"], ["data-key", "fgty"], ["data-key", "fgcn"], ["data-key", "fgpt"], ["data-key", "fgsn"], ["data-key", "Action"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "Fuel_Group_Id", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt10", "pb0"], ["onclick", "closeSlidePanel();", 1, "ml20", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [1, "pt10", "pb10", "pl20", "pr20"], [3, "result"], [1, "text-center", "text-nowrap"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "onclick", "slidePanel('#Fuel_Group_Id','40%')", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["onclick", "slidePanel('#Fuel_Group_Id','40%')", "placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-eye"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-file-archive"], ["onclick", "slidePanel('#Fuel_Group_Id','40%')", "placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function FuelGroupMappingComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "button", 2, 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function FuelGroupMappingComponent_Template_button_click_2_listener() {
            return ctx.addFuelGroup();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, " Create Fuel Group ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "table", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Fuel Group");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Group Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Group for");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Customer Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Product Type(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Actions");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](32, FuelGroupMappingComponent_tr_32_Template, 20, 9, "tr", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, FuelGroupMappingComponent_div_33_Template, 5, 0, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "a", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](38, "i", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "h3", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](41, "titlecase");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "app-create-fuel-group", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("result", function FuelGroupMappingComponent_Template_app_create_fuel_group_result_43_listener($event) {
            return ctx.getOutput($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.fuelGroupGridList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](41, 5, ctx.title), " Fuel Group ");
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _create_fuel_group_component__WEBPACK_IMPORTED_MODULE_7__["CreateFuelGroupComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbTooltip"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__["ɵc"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["TitleCasePipe"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NyZWF0ZS1mdWVsLWdyb3VwL2Z1ZWwtZ3JvdXAtbWFwcGluZy9mdWVsLWdyb3VwLW1hcHBpbmcuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](FuelGroupMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-fuel-group-mapping',
          templateUrl: './fuel-group-mapping.component.html',
          styleUrls: ['./fuel-group-mapping.component.css']
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_8__["HttpClient"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__["CarrierService"]
        }, {
          type: src_app_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_10__["ExternalMappingsService"]
        }, {
          type: _fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]
        }];
      }, {
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        openSidePannel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['openSidePannel']
        }],
        CloseBulkUploadPopup: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['btnCloseBulkUploadPopup']
        }],
        createFuelGroupComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_create_fuel_group_component__WEBPACK_IMPORTED_MODULE_7__["CreateFuelGroupComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/models/FuelGroupGridViewModel.ts": function srcAppSelfServiceAliasModelsFuelGroupGridViewModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelGroupGridViewModel", function () {
      return FuelGroupGridViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelGroupViewModel", function () {
      return FuelGroupViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelGroupType", function () {
      return FuelGroupType;
    });

    var FuelGroupGridViewModel = function FuelGroupGridViewModel() {
      _classCallCheck(this, FuelGroupGridViewModel);
    };

    var FuelGroupViewModel = function FuelGroupViewModel() {
      _classCallCheck(this, FuelGroupViewModel);
    };

    var FuelGroupType;

    (function (FuelGroupType) {
      FuelGroupType[FuelGroupType["Standard"] = 1] = "Standard";
      FuelGroupType[FuelGroupType["Custom"] = 2] = "Custom";
    })(FuelGroupType || (FuelGroupType = {}));
    /***/

  }
}]);
//# sourceMappingURL=create-fuel-group-create-fuel-group-module-es5.js.map