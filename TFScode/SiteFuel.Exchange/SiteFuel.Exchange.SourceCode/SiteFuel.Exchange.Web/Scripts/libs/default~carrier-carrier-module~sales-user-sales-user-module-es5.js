function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~carrier-carrier-module~sales-user-sales-user-module"], {
  /***/
  "./src/app/carrier/schedule-builder/add-location/add-location.model.ts": function srcAppCarrierScheduleBuilderAddLocationAddLocationModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Geocode", function () {
      return Geocode;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "StateModel", function () {
      return StateModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapIconUrl", function () {
      return MapIconUrl;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapIconSize", function () {
      return MapIconSize;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapConstants", function () {
      return MapConstants;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RackAvgTypes", function () {
      return RackAvgTypes;
    });

    var Geocode = function Geocode() {
      _classCallCheck(this, Geocode);
    };

    var StateModel = function StateModel() {
      _classCallCheck(this, StateModel);
    };

    var MapIconUrl = function MapIconUrl() {
      _classCallCheck(this, MapIconUrl);
    };

    var MapIconSize = function MapIconSize() {
      _classCallCheck(this, MapIconSize);
    };

    var MapConstants = function MapConstants(countryId) {
      _classCallCheck(this, MapConstants);

      this.ZoomArea = 15;
      this.IconUrl = {
        url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png',
        scaledSize: {
          width: 40,
          height: 40
        }
      };

      if (countryId == 2) {
        this.CenterLat = 56.14;
        this.CenterLon = -106.34;
      } else if (countryId == 3) {
        this.CenterLat = 28.61;
        this.CenterLon = 77.23;
      } else {
        this.CenterLat = 38;
        this.CenterLon = -98.35;
      }
    };

    var RackAvgTypes = [{
      Id: 1,
      Name: '+$',
      Code: ''
    }, {
      Id: 2,
      Name: '-$',
      Code: ''
    }, {
      Id: 3,
      Name: '+%',
      Code: ''
    }, {
      Id: 4,
      Name: '-%',
      Code: ''
    }];
    /***/
  },

  /***/
  "./src/app/carrier/schedule-builder/add-location/add-location.service.ts": function srcAppCarrierScheduleBuilderAddLocationAddLocationServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AddLocationService", function () {
      return AddLocationService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var AddLocationService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(AddLocationService, _src_app_errors_Handl);

      var _super = _createSuper(AddLocationService);

      function AddLocationService(httpClient) {
        var _this;

        _classCallCheck(this, AddLocationService);

        _this = _super.call(this);
        _this.httpClient = httpClient;
        _this.getaddressByZipUrl = "Validation/GetAddressByZip";
        _this.getOpisTerminalsUrl = "Supplier/Order/GetOpisTerminals";
        _this.getAllTPOCompaniesUrl = "/Carrier/Order/GetAllTPOCompanies"; //private getAllBuyerCompaniesUrl = "/Carrier/Order/GetAllBuyerCompanies";

        _this.getStatesOfAllCountrieslistUrl = "/Supplier/Order/GetStatesOfAllCountries";
        _this.getTPOContactPersonDetailsUrl = "/Supplier/Order/GetTPOContactPersonDetails";
        _this.getFuelProductslistUrl = "/Supplier/Order/GetFuelProducts";
        _this.getMarineProductListUrl = "/Supplier/FuelRequest/GetProductList";
        _this.getFuelProductsByZiplistUrl = "/Supplier/Order/GetProductListByZip";
        _this.postCreateOrderUrl = "/Carrier/ScheduleBuilder/Create";
        _this.isTpoCompanyExistUrl = "/Validation/IsCompanyNameExist";
        _this.isJobNameExistUrl = "/Supplier/Order/ValidateJobNameByCompanyId";
        _this.isIsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid";
        _this.GetPricingCodesUrl = "/Supplier/Order/GetPricingCodes";
        _this.getClosedTerminalUrl = "Supplier/Order/GetClosedTerminal";
        _this.getCurrancylistUrl = "/Supplier/Order/GetCurrenyList";
        _this.getTPOCompanyContactPersonsUrl = "/Supplier/Order/GetTPOCompanyContactPersons";
        _this.GetaddressbyLatLongUrl = "/Supplier/Order/GetAddressByLongLat";
        _this.GetCityGroupTerminalsUrl = "/Supplier/Order/GetCityGroupTerminals";
        _this.GetTerminalsUrl = "/Carrier/ScheduleBuilder/GetTerminalsForMultipleProducts";
        _this.isCityGroupTerminalPriceAvailableUrl = "/Supplier/FuelRequest/IsCityGroupTerminalPriceAvailable";
        _this.getPreferencesSettingsUrl = "/Carrier/ScheduleBuilder/GetPreferenceSettingForOnTheFlyLocation";
        _this.getTimeZoneNameUrl = "https://maps.googleapis.com/maps/api/timezone/json";
        _this.GetAddressUrl = "/Validation/GetAddress?address=";
        _this.getTfxProductUrl = "Supplier/Order/GetTfxProduct";
        return _this;
      }

      _createClass(AddLocationService, [{
        key: "getAllTPOCompanies",
        value: function getAllTPOCompanies() {
          return this.httpClient.get(this.getAllTPOCompaniesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAllTPOCompanies', null)));
        }
      }, {
        key: "getTPOCompanyContactPersons",
        value: function getTPOCompanyContactPersons(companyId) {
          return this.httpClient.get("".concat(this.getTPOCompanyContactPersonsUrl, "?companyId=").concat(companyId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTPOCompanyContactPersons', null)));
        }
      }, {
        key: "getTPOContactPersonDetails",
        value: function getTPOContactPersonDetails(userId) {
          return this.httpClient.get("".concat(this.getTPOContactPersonDetailsUrl, "?userId=").concat(userId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTPOContactPersonDetails', null)));
        }
      }, {
        key: "getCurrenyList",
        value: function getCurrenyList() {
          return this.httpClient.get(this.getCurrancylistUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getCurrenyList', null)));
        }
      }, {
        key: "getStatesOfAllCountries",
        value: function getStatesOfAllCountries(countryId) {
          return this.httpClient.get("".concat(this.getStatesOfAllCountrieslistUrl, "?countryId=").concat(countryId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getStatesOfAllCountries', null)));
        }
      }, {
        key: "getFuelProducts",
        value: function getFuelProducts(productDisplayGroupId, companyId, jobId) {
          return this.httpClient.get("".concat(this.getFuelProductslistUrl, "?productDisplayGroupId=").concat(productDisplayGroupId, "&companyId=").concat(companyId, "&jobId=").concat(jobId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetFuelProducts', null)));
        }
      }, {
        key: "getMarineProductList",
        value: function getMarineProductList(displayGroupId, jobId, zipCode, source) {
          return this.httpClient.get("".concat(this.getMarineProductListUrl, "?displayGroupId=").concat(displayGroupId, "&jobId=").concat(jobId, "&zipCode=").concat(zipCode, "&source=").concat(source)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getMarineProductList', null)));
        }
      }, {
        key: "getProductListByZip",
        value: function getProductListByZip(zipCode, radius) {
          return this.httpClient.get("".concat(this.getFuelProductsByZiplistUrl, "?zipCode=").concat(zipCode, "&radius=").concat(radius)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getProductListByZip', null)));
        }
      }, {
        key: "getFuelTerminals",
        value: function getFuelTerminals(jobCountryId, pricingCodeId, fuelType, companyCountryId, isSupressOrderPricing, jobLatitude, jobLongitude, searchStringTeminal) {
          return this.httpClient.get(this.GetTerminalsUrl + '?jobCountryId=' + jobCountryId + '&pricingCodeId=' + pricingCodeId + '&fuelType=' + fuelType + '&companyCountryId=' + companyCountryId + '&isSupressOrderPricing=' + isSupressOrderPricing + '&jobLatitude=' + jobLatitude + '&jobLongitude=' + jobLongitude + '&searchStringTeminal=' + searchStringTeminal).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFuelTerminals', null)));
        }
      }, {
        key: "getOpisTerminals",
        value: function getOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source) {
          return this.httpClient.get(this.getOpisTerminalsUrl + '?cityRackId=' + cityRackId + '&countryId=' + countryId + '&latitude=' + latitude + '&longitude=' + longitude + '&terminal=' + terminal + '&source=' + source).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getOpisTerminals', null)));
        }
      }, {
        key: "getTfxProduct",
        value: function getTfxProduct(tfxProductId) {
          return this.httpClient.get(this.getTfxProductUrl + '?tfxProductId=' + tfxProductId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getProductIdForTfxProduct', null)));
        }
      }, {
        key: "createOrder",
        value: function createOrder(modal) {
          return this.httpClient.post(this.postCreateOrderUrl, modal).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('createOrder', null)));
        }
      }, {
        key: "getTimeZoneName",
        value: function getTimeZoneName(latitude, longitude, timestamp, apiKey) {
          return this.httpClient.get(this.getTimeZoneNameUrl + "?location=" + latitude + "," + longitude + "&timestamp=" + timestamp + "&key=" + apiKey).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('createOrder', null)));
        }
      }, {
        key: "isTpoCompanyExist",
        value: function isTpoCompanyExist(IsNewCompany, CompanyName) {
          return this.httpClient.get("".concat(this.isTpoCompanyExistUrl, "?IsNewCompany=").concat(IsNewCompany, "&CompanyName=").concat(CompanyName)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('isTpoCompanyExist', null)));
        }
      }, {
        key: "isJobNameExist",
        value: function isJobNameExist(jobName, companyId) {
          return this.httpClient.get("".concat(this.isJobNameExistUrl, "?jobName=").concat(jobName, "&companyId=").concat(companyId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('isJobNameExist', null)));
        }
      }, {
        key: "IsPhoneNumberValid",
        value: function IsPhoneNumberValid(phoneNumber) {
          return this.httpClient.get("".concat(this.isIsPhoneNumberValidUrl, "?phoneNumber=").concat(phoneNumber)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsPhoneNumberValid', null)));
        }
      }, {
        key: "getClosedTerminal",
        value: function getClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId, cityRackId) {
          return this.httpClient.get("".concat(this.getClosedTerminalUrl, "?fuelTypeId=").concat(fuelTypeId, "&latitude=").concat(latitude, "&longitude=").concat(longitude, "&countryId=").concat(countryId, "&pricingCodeId=").concat(pricingCodeId, "&terminal=").concat(terminal, "&pricingSourceId=").concat(pricingSourceId, "&cityRackId=").concat(cityRackId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getClosedTerminal', null)));
        }
      }, {
        key: "getPricingCodes",
        value: function getPricingCodes(filterModel) {
          return this.httpClient.get("".concat(this.GetPricingCodesUrl, "?PricingTypeId=").concat(filterModel.PricingTypeId, "&PricingSourceId=").concat(filterModel.PricingSourceId, "&feedTypeId=").concat(filterModel.feedTypeId, "&fuelClassTypeId=").concat(filterModel.fuelClassTypeId, "&tfxProdId=").concat(filterModel.tfxProdId, "&Prefix=").concat(filterModel.Prefix)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getPricingCodes', null)));
        }
      }, {
        key: "getAddressByZip",
        value: function getAddressByZip(zipCode, address) {
          return this.httpClient.get("".concat(this.getaddressByZipUrl, "?zipCode=").concat(zipCode, "&address=").concat(address)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getAddressByZip', null)));
        }
      }, {
        key: "GetAddressByLongLat",
        value: function GetAddressByLongLat(latitude, longitude) {
          return this.httpClient.get("".concat(this.GetaddressbyLatLongUrl, "?latitude=").concat(latitude, "&longitude=").concat(longitude)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAddressByLongLat', null)));
        }
      }, {
        key: "GetCityGroupTerminals",
        value: function GetCityGroupTerminals(stateId, allStates, sourceId) {
          return this.httpClient.get("".concat(this.GetCityGroupTerminalsUrl, "?stateId=").concat(stateId, "&allStates=").concat(allStates, "&sourceId=").concat(sourceId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCityGroupTerminals', null)));
        }
      }, {
        key: "IsCityGroupTerminalPriceAvailable",
        value: function IsCityGroupTerminalPriceAvailable(jobid, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId) {
          return this.httpClient.get(this.isCityGroupTerminalPriceAvailableUrl + '?jobid=' + jobid + '&fueltypeId=' + fueltypeId + '&selectedCityRackId=' + selectedCityRackId + '&lattitude=' + lattitude + '&longitude=' + longitude + '&countryCode=' + countryCode + '&sourceId=' + sourceId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsCityGroupTerminalPriceAvailable', null)));
        }
      }, {
        key: "GetPreferencesSettings",
        value: function GetPreferencesSettings() {
          return this.httpClient.get(this.getPreferencesSettingsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetPreferencesSettings', null)));
        }
      }, {
        key: "GetAddress",
        value: function GetAddress(address) {
          return this.httpClient.get(this.GetAddressUrl + address).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetaddressbyAddress', null)));
        }
      }]);

      return AddLocationService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"]);

    AddLocationService.??fac = function AddLocationService_Factory(t) {
      return new (t || AddLocationService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    AddLocationService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({
      token: AddLocationService,
      factory: AddLocationService.??fac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](AddLocationService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/shared-components/pricing-section/pricing-section.component.ts": function srcAppSharedComponentsPricingSectionPricingSectionComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PricingSectionComponent", function () {
      return PricingSectionComponent;
    });
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _carrier_schedule_builder_add_location_add_location_model__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../carrier/schedule-builder/add-location/add-location.model */
    "./src/app/carrier/schedule-builder/add-location/add-location.model.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _carrier_schedule_builder_add_location_add_location_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../carrier/schedule-builder/add-location/add-location.service */
    "./src/app/carrier/schedule-builder/add-location/add-location.service.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _directives_disable_control_directive__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../../directives/disable-control.directive */
    "./src/app/directives/disable-control.directive.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! angular-ng-autocomplete */
    "./node_modules/angular-ng-autocomplete/__ivy_ngcc__/fesm2015/angular-ng-autocomplete.js");

    function PricingSectionComponent_div_12_div_5_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " For Cumulative Delivered Quantity ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Delivery Quantity Ranging ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_input_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "input", 54);
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", true);
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_input_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "input", 55);
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", true);
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_label_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "To");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_label_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "Above");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_input_18_Template(rf, ctx) {
      if (rf & 1) {
        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "input", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_5_ng_container_19_input_18_Template_input_change_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r25);

          var i_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().index;

          var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r24.toQuantityChanged($event.target.value, i_r12);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_span_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " To Quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_label_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_label_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var priceControl_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](priceControl_r11.get("DisplayPrice").value);
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_ng_container_29_Template(rf, ctx) {
      if (rf & 1) {
        var _r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "a", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_div_5_ng_container_19_ng_container_29_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r29);

          var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r28.addRemoveTierFee(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "a", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_div_5_ng_container_19_ng_container_29_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r29);

          var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r30.addRemoveTierFee(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_label_31_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "label", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Pricing is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_ng_container_19_Template(rf, ctx) {
      if (rf & 1) {
        var _r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PricingSectionComponent_div_12_div_5_ng_container_19_input_10_Template, 1, 1, "input", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PricingSectionComponent_div_12_div_5_ng_container_19_input_11_Template, 1, 1, "input", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](12, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, PricingSectionComponent_div_12_div_5_ng_container_19_label_14_Template, 2, 0, "label", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, PricingSectionComponent_div_12_div_5_ng_container_19_label_15_Template, 2, 0, "label", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, PricingSectionComponent_div_12_div_5_ng_container_19_input_18_Template, 1, 0, "input", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PricingSectionComponent_div_12_div_5_ng_container_19_span_19_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](22, PricingSectionComponent_div_12_div_5_ng_container_19_label_22_Template, 2, 0, "label", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, PricingSectionComponent_div_12_div_5_ng_container_19_label_23_Template, 2, 1, "label", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "a", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_div_5_ng_container_19_Template_a_click_26_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r32);

          var i_r12 = ctx.index;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](16);

          return ctx_r31.setPricingSourceClicked_tpt(i_r12, _r3);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](27, " Set Pricing ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](29, PricingSectionComponent_div_12_div_5_ng_container_19_ng_container_29_Template, 3, 0, "ng-container", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](31, PricingSectionComponent_div_12_div_5_ng_container_19_label_31_Template, 2, 0, "label", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var priceControl_r11 = ctx.$implicit;
        var i_r12 = ctx.index;
        var isLast_r13 = ctx.last;

        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroupName", i_r12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !isLast_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", isLast_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !isLast_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", isLast_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !isLast_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.formSubmited && priceControl_r11.get("ToQuantity").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", priceControl_r11.get("DisplayPrice").value == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", priceControl_r11.get("DisplayPrice").value != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", i_r12 == ctx_r9.tierPricingForm.Pricings.controls.length - 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.formSubmited && priceControl_r11.get("DisplayPrice").errors);
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_div_6_div_15_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Date is required field. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_div_6_div_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "input", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function PricingSectionComponent_div_12_div_5_div_20_div_6_div_15_Template_input_onDateChange_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r38);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](5);

          return ctx_r37.tierPricingForm.ResetCumulationSetting.get("Date").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, PricingSectionComponent_div_12_div_5_div_20_div_6_div_15_span_3_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("maxDate", ctx_r34.MaxInputDate)("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r34.formSubmited && ctx_r34.tierPricingForm.ResetCumulationSetting.get("Date").errors);
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_div_6_div_16_span_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Day is required field. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_div_6_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "select", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "option", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Select Day");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "option", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Mon");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "option", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Tue");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "option", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "Wed");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "option", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Thu");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "option", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Fri");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "option", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Sat");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "option", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Sun");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PricingSectionComponent_div_12_div_5_div_20_div_6_div_16_span_19_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r35.formSubmited && ctx_r35.tierPricingForm.ResetCumulationSetting.get("Day").errors);
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_div_6_Template(rf, ctx) {
      if (rf & 1) {
        var _r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "select", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_5_div_20_div_6_Template_select_change_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r41);

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r40.cumulationTypeChanged(true, $event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "option", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "Select");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "option", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Weekly");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "option", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "BiWeekly");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "option", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](11, "Monthly");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "option", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13, "Specific Dates");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](14, "span", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, PricingSectionComponent_div_12_div_5_div_20_div_6_div_15_Template, 4, 3, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PricingSectionComponent_div_12_div_5_div_20_div_6_div_16_Template, 20, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r33.tierPricingForm.ResetCumulationSetting.get("CumulationType").value == 3 || ctx_r33.tierPricingForm.ResetCumulationSetting.get("CumulationType").value == 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r33.tierPricingForm.ResetCumulationSetting.get("CumulationType").value == 1 || ctx_r33.tierPricingForm.ResetCumulationSetting.get("CumulationType").value == 2);
      }
    }

    function PricingSectionComponent_div_12_div_5_div_20_Template(rf, ctx) {
      if (rf & 1) {
        var _r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "input", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_5_div_20_Template_input_change_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r43);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r42.cumulationTypeChanged($event.target.checked, ctx_r42.tierPricingForm.ResetCumulationSetting.get("CumulationType").value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "label", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, " Reset Frequency");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, PricingSectionComponent_div_12_div_5_div_20_div_6_Template, 17, 2, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r10.tierPricingForm.IsResetCumulation.value);
      }
    }

    function PricingSectionComponent_div_12_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "input", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "label", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Volume Based");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "input", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, " Delivery Quantity Based ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, PricingSectionComponent_div_12_div_5_div_14_Template, 2, 0, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, PricingSectionComponent_div_12_div_5_div_15_Template, 2, 0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PricingSectionComponent_div_12_div_5_ng_container_19_Template, 32, 11, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, PricingSectionComponent_div_12_div_5_div_20_Template, 7, 1, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classMap"](ctx_r5.f.FuelDeliveryDetails.get("DeliveryTypeId").value == 1 ? "pntr-none subSectionOpacity" : "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx_r5.f.FuelDeliveryDetails.get("DeliveryTypeId").value == 1)("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r5.tierPricingForm.TierPricingType.value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r5.tierPricingForm.TierPricingType.value == 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r5.tierPricingForm.Pricings["controls"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r5.tierPricingForm.TierPricingType.value == 1);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_ng_template_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "a", 109);
      }

      if (rf & 2) {
        var item_r56 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", item_r56.Code, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_ng_template_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "div", 109);
      }

      if (rf & 2) {
        var notFound_r57 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", notFound_r57, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_div_16_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Select Pricing Code. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_17_div_16_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r54.f.FuelPricingDetails.get("Code").errors.required);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r55.f.FuelPricingDetails.get("CodeDescription").value, " ");
      }
    }

    function PricingSectionComponent_div_12_div_6_div_17_Template(rf, ctx) {
      if (rf & 1) {
        var _r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "label", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, " Pricing Code ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PricingSectionComponent_div_12_div_6_div_17_div_9_Template, 2, 0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "ng-autocomplete", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("selected", function PricingSectionComponent_div_12_div_6_div_17_Template_ng_autocomplete_selected_11_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r60);

          var ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r59.onPricingCodeSelected($event);
        })("click", function PricingSectionComponent_div_12_div_6_div_17_Template_ng_autocomplete_click_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r60);

          var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r61.getPricingCodes();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PricingSectionComponent_div_12_div_6_div_17_ng_template_12_Template, 1, 1, "ng-template", null, 103, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, PricingSectionComponent_div_12_div_6_div_17_ng_template_14_Template, 1, 1, "ng-template", null, 104, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PricingSectionComponent_div_12_div_6_div_17_div_16_Template, 2, 1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "button", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_div_6_div_17_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r60);

          var ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](14);

          return ctx_r62.openPriceCodeModal(_r1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, " Select Pricing Code ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, PricingSectionComponent_div_12_div_6_div_17_div_21_Template, 2, 1, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](13);

        var _r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](15);

        var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r44._loadingPricingCodes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r44.pricingCodes)("searchKeyword", "Code")("itemTemplate", _r50)("notFoundTemplate", _r52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r44.formSubmited && ctx_r44.f.FuelPricingDetails.get("Code").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx_r44._loadingPricingCodes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r44.f.FuelPricingDetails.get("TempPricingCodeDetails").value);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_18_div_8_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Price is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_18_div_8_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid Pricing. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_18_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_18_div_8_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_div_12_div_6_div_18_div_8_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r63.f.FuelPricingDetails.get("PricePerGallon").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r63.f.FuelPricingDetails.get("PricePerGallon").errors.pattern || (ctx_r63.f.FuelPricingDetails.get("PricePerGallon").errors == null ? null : ctx_r63.f.FuelPricingDetails.get("PricePerGallon").errors.min));
      }
    }

    function PricingSectionComponent_div_12_div_6_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, PricingSectionComponent_div_12_div_6_div_18_div_8_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r45.f.AddressDetails.get("CountryId").value == 1 ? "USD" : "CAD", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r45.formSubmited && ctx_r45.f.FuelPricingDetails.get("PricePerGallon").errors);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_22_option_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var rackAvgType_r68 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", rackAvgType_r68.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", rackAvgType_r68.Name, " ");
      }
    }

    function PricingSectionComponent_div_12_div_6_div_22_div_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Rack Price field is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_22_div_10_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid Rack Price. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_22_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_22_div_10_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_div_12_div_6_div_22_div_10_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r67.f.FuelPricingDetails.get("RackPrice").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r67.f.FuelPricingDetails.get("RackPrice").errors.pattern || ctx_r67.f.FuelPricingDetails.get("RackPrice").errors.min);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "select", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, PricingSectionComponent_div_12_div_6_div_22_option_5_Template, 2, 2, "option", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](9, "input", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PricingSectionComponent_div_12_div_6_div_22_div_10_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r46.RackAvgTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r46.formSubmited && ctx_r46.f.FuelPricingDetails.get("RackPrice").errors);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_23_option_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var rackAvgType_r73 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", rackAvgType_r73.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", rackAvgType_r73.Name, " ");
      }
    }

    function PricingSectionComponent_div_12_div_6_div_23_div_8_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " The Fuel Cost field is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_23_div_8_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid mark up value. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_23_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_23_div_8_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_div_12_div_6_div_23_div_8_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r72.f.FuelPricingDetails.get("SupplierCostMarkupValue").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r72.f.FuelPricingDetails.get("SupplierCostMarkupValue").errors.pattern || ctx_r72.f.FuelPricingDetails.get("SupplierCostMarkupValue").errors.min);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "select", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, PricingSectionComponent_div_12_div_6_div_23_option_3_Template, 2, 2, "option", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, PricingSectionComponent_div_12_div_6_div_23_div_8_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r47.RackAvgTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r47.formSubmited && ctx_r47.f.FuelPricingDetails.get("SupplierCostMarkupValue").errors);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_9_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", item_r84.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r84.Name, " ");
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_9_option_1_Template, 2, 2, "option", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r84 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", item_r84.IsWithinState);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_11_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", item_r87.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r87.Name, " ");
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_11_option_1_Template, 2, 2, "option", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r87 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !item_r87.IsWithinState);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_div_12_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_div_12_div_6_div_24_div_11_div_12_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r83.f.FuelPricingDetails.get("CityGroupTerminalId").errors.required);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_div_11_Template(rf, ctx) {
      if (rf & 1) {
        var _r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "select", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_6_div_24_div_11_Template_select_change_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r92);

          var ctx_r91 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r91.cityRackTerminalChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "option", 140);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, " Select ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "option", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, " City Rack/Terminal ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "optgroup", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_9_Template, 2, 1, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "optgroup", 143);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PricingSectionComponent_div_12_div_6_div_24_div_11_ng_container_11_Template, 2, 1, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PricingSectionComponent_div_12_div_6_div_24_div_11_div_12_Template, 2, 1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r76.CityGroupTerminalsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r76.CityGroupTerminalsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r76.formSubmited && ctx_r76.f.FuelPricingDetails.get("CityGroupTerminalId").errors);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_ng_template_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "a", 109);
      }

      if (rf & 2) {
        var item_r93 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", item_r93.Name, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_ng_template_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "div", 109);
      }

      if (rf & 2) {
        var notFound_r94 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", notFound_r94, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_div_12_div_6_div_24_Template(rf, ctx) {
      if (rf & 1) {
        var _r96 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "input", 130);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_6_div_24_Template_input_change_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r96);

          var ctx_r95 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          ctx_r95.setRackTerminalValidation();
          return ctx_r95.getCityGroupTerminals();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, " City Rack/Terminal ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PricingSectionComponent_div_12_div_6_div_24_div_11_Template, 13, 3, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 133);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "ng-autocomplete", 134);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_div_6_div_24_Template_ng_autocomplete_click_15_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r96);

          var ctx_r97 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r97.getApprovedTerminal();
        })("selected", function PricingSectionComponent_div_12_div_6_div_24_Template_ng_autocomplete_selected_15_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r96);

          var ctx_r98 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r98.onApprovedTerminalSelected($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PricingSectionComponent_div_12_div_6_div_24_ng_template_16_Template, 1, 1, "ng-template", null, 135, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, PricingSectionComponent_div_12_div_6_div_24_ng_template_18_Template, 1, 1, "ng-template", null, 136, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](17);

        var _r79 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](19);

        var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classMap"](ctx_r48.f.FuelPricingDetails.get("FuelPricingDetails").get("PricingSourceId").value == 2 ? "pntr-none subSectionOpacity" : "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx_r48.f.FuelPricingDetails.get("FuelPricingDetails").get("PricingSourceId").value == 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r48.f.FuelPricingDetails.get("EnableCityRack").value == true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r48.approvedTerminalList)("searchKeyword", "Name")("itemTemplate", _r77)("notFoundTemplate", _r79);
      }
    }

    function PricingSectionComponent_div_12_div_6_Template(rf, ctx) {
      if (rf & 1) {
        var _r100 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "select", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_div_12_div_6_Template_select_change_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r100);

          var ctx_r99 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r99.pricingTypeChanged($event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Market Based");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Fuel Cost");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Fixed");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, PricingSectionComponent_div_12_div_6_div_17_Template, 22, 8, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, PricingSectionComponent_div_12_div_6_div_18_Template, 9, 2, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](21, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](22, PricingSectionComponent_div_12_div_6_div_22_Template, 11, 2, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, PricingSectionComponent_div_12_div_6_div_23_Template, 9, 2, "div", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, PricingSectionComponent_div_12_div_6_div_24_Template, 20, 8, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx_r6.f.FuelDetails["controls"]["FuelDisplayGroupId"].value == 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.f.FuelPricingDetails.get("PricingTypeId").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.f.FuelPricingDetails.get("PricingTypeId").value == 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.f.FuelPricingDetails.get("PricingTypeId").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.f.FuelPricingDetails.get("PricingTypeId").value == 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.f.FuelPricingDetails.get("PricingTypeId").value == 1);
      }
    }

    function PricingSectionComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r102 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "input", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_div_12_Template_input_click_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r102);

          var ctx_r101 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r101.tierPricingEnabled($event.target.checked);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "label", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, " Create Tier Pricing");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, PricingSectionComponent_div_12_div_5_Template, 21, 9, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, PricingSectionComponent_div_12_div_6_Template, 25, 9, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx_r0.f.AddressDetails.get("IsMarineLocation").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r0.fuelPricingForm.IsTierPricingRequired.value);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx_r0.fuelPricingForm.IsTierPricingRequired.value);
      }
    }

    function PricingSectionComponent_ng_template_13_div_42_div_4_ng_container_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r105.FeedType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r105.WeekendPricingDay);
      }
    }

    function PricingSectionComponent_ng_template_13_div_42_div_4_ng_container_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r105.FuelClassType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r105.QuantityIndicator);
      }
    }

    function PricingSectionComponent_ng_template_13_div_42_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, PricingSectionComponent_ng_template_13_div_42_div_4_ng_container_5_Template, 5, 2, "ng-container", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](6, PricingSectionComponent_ng_template_13_div_42_div_4_ng_container_6_Template, 5, 2, "ng-container", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r105.PricingSource, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r105.RackAvgPricingType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", item_r105.PricingSourceId == 2 || item_r105.PricingSourceId == 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", item_r105.PricingSourceId == 2);
      }
    }

    function PricingSectionComponent_ng_template_13_div_42_Template(rf, ctx) {
      if (rf & 1) {
        var _r113 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 172);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_13_div_42_Template_div_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r113);

          var item_r105 = ctx.$implicit;

          var ctx_r112 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r112.getSelectedPricingCode(item_r105);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label", 173);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](4, PricingSectionComponent_ng_template_13_div_42_div_4_Template, 7, 4, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r105 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate"]("id", item_r105.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r105.Code, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", item_r105.PricingTypeId == 1);
      }
    }

    function PricingSectionComponent_ng_template_13_Template(rf, ctx) {
      if (rf & 1) {
        var _r116 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 145);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "h4", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Pricing Code");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "button", 147);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_13_Template_button_click_3_listener() {
          var modal_r103 = ctx.$implicit;
          return modal_r103.dismiss("Cross click");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "span", 148);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "\xD7");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "input", 152);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r115 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r115.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_11_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r117 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r117.pricingfeedTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "label", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13, "All");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "input", 154);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r118 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r118.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r119 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r119.pricingfeedTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "label", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Contract (10AM EST)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "input", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r120 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r120.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_17_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r121 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r121.pricingfeedTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, "End of Day (5PM EST)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "input", 158);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_20_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r122 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r122.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_20_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r123.pricingfeedTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "label", 159);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Previous Day (10AM EST)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "input", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_23_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r124 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r124.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_23_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r125 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r125.pricingfeedTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "label", 161);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Previous Day (5PM EST)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "input", 162);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_29_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r126 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r126.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_29_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r127 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r127.pricingfuelClassTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "label", 163);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, "All");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "input", 164);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_32_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r128 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r128.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_32_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r129 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r129.pricingfuelClassTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "label", 165);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "Branded");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "input", 166);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_35_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r130 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r130.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_35_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r131 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r131.pricingfuelClassTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "label", 167);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](37, "Unbranded");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "input", 168);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_13_Template_input_change_38_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r132 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r132.getPricingCodes();
        })("ngModelChange", function PricingSectionComponent_ng_template_13_Template_input_ngModelChange_38_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r116);

          var ctx_r133 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r133.pricingfuelClassTypeId = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "label", 169);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "Both");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](42, PricingSectionComponent_ng_template_13_div_42_Template, 5, 3, "div", 170);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfeedTypeId)("value", 0)("checked", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfeedTypeId)("value", 1)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfeedTypeId)("value", 3)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfeedTypeId)("value", 5)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfeedTypeId)("value", 4)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfuelClassTypeId)("value", 0)("checked", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfuelClassTypeId)("value", 1)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfuelClassTypeId)("value", 2)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r2.pricingfuelClassTypeId)("value", 3)("checked", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r2.pricingCodes);
      }
    }

    function PricingSectionComponent_ng_template_15_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_ng_template_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "a", 109);
      }

      if (rf & 2) {
        var item_r148 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", item_r148.Code, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_ng_template_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "div", 109);
      }

      if (rf & 2) {
        var notFound_r149 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", notFound_r149, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_div_16_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Select Pricing Code. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_25_div_16_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r146 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r146.TierPricingTypeForm.get("Code").errors.required);
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r147 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r147.TierPricingTypeForm.get("CodeDescription").value, " ");
      }
    }

    function PricingSectionComponent_ng_template_15_div_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r152 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "label", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, " Pricing Code ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PricingSectionComponent_ng_template_15_div_25_div_9_Template, 2, 0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "ng-autocomplete", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("selected", function PricingSectionComponent_ng_template_15_div_25_Template_ng_autocomplete_selected_11_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r152);

          var ctx_r151 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r151.onPricingCodeSelected_tpt($event);
        })("click", function PricingSectionComponent_ng_template_15_div_25_Template_ng_autocomplete_click_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r152);

          var ctx_r153 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r153.getPricingCodes();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PricingSectionComponent_ng_template_15_div_25_ng_template_12_Template, 1, 1, "ng-template", null, 103, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, PricingSectionComponent_ng_template_15_div_25_ng_template_14_Template, 1, 1, "ng-template", null, 104, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PricingSectionComponent_ng_template_15_div_25_div_16_Template, 2, 1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "button", 180);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_15_div_25_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r152);

          var ctx_r154 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](14);

          return ctx_r154.openPriceCodeModal_tpt(_r1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, " Select Pricing Code ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, PricingSectionComponent_ng_template_15_div_25_div_21_Template, 2, 1, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r142 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](13);

        var _r144 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](15);

        var ctx_r136 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r136._loadingPricingCodes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r136.pricingCodes)("searchKeyword", "Code")("itemTemplate", _r142)("notFoundTemplate", _r144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r136.TierPricingTypeForm.get("TempPricingCodeDetails").touched && ctx_r136.TierPricingTypeForm.get("Code").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx_r136._loadingPricingCodes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r136.TierPricingTypeForm.get("TempPricingCodeDetails").value);
      }
    }

    function PricingSectionComponent_ng_template_15_div_26_div_8_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Price is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_26_div_8_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid Pricing. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_26_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_26_div_8_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_ng_template_15_div_26_div_8_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r155 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r155.TierPricingTypeForm.get("PricePerGallon").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r155.TierPricingTypeForm.get("PricePerGallon").errors.pattern || (ctx_r155.TierPricingTypeForm.get("PricePerGallon").errors == null ? null : ctx_r155.TierPricingTypeForm.get("PricePerGallon").errors.min));
      }
    }

    function PricingSectionComponent_ng_template_15_div_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, PricingSectionComponent_ng_template_15_div_26_div_8_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r137 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r137.f.AddressDetails.get("CountryId").value == 1 ? "USD" : "CAD", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r137.TierPricingTypeForm.get("PricePerGallon").touched && ctx_r137.TierPricingTypeForm.get("PricePerGallon").errors);
      }
    }

    function PricingSectionComponent_ng_template_15_div_30_option_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var rackAvgType_r160 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", rackAvgType_r160.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", rackAvgType_r160.Name, " ");
      }
    }

    function PricingSectionComponent_ng_template_15_div_30_div_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Rack Price field is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_30_div_10_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid Rack Price. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_30_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_30_div_10_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_ng_template_15_div_30_div_10_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r159 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r159.TierPricingTypeForm.get("RackPrice").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r159.TierPricingTypeForm.get("RackPrice").errors.pattern || ctx_r159.TierPricingTypeForm.get("RackPrice").errors.min);
      }
    }

    function PricingSectionComponent_ng_template_15_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "select", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, PricingSectionComponent_ng_template_15_div_30_option_5_Template, 2, 2, "option", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](9, "input", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PricingSectionComponent_ng_template_15_div_30_div_10_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r138 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r138.RackAvgTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r138.TierPricingTypeForm.get("RackPrice").touched && ctx_r138.TierPricingTypeForm.get("RackPrice").errors);
      }
    }

    function PricingSectionComponent_ng_template_15_div_31_option_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var rackAvgType_r165 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", rackAvgType_r165.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", rackAvgType_r165.Name, " ");
      }
    }

    function PricingSectionComponent_ng_template_15_div_31_div_8_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " The Fuel Cost field is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_31_div_8_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid mark up value. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_31_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_31_div_8_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PricingSectionComponent_ng_template_15_div_31_div_8_span_2_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r164 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r164.TierPricingTypeForm.get("SupplierCostMarkupValue").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r164.TierPricingTypeForm.get("SupplierCostMarkupValue").errors.pattern || ctx_r164.TierPricingTypeForm.get("SupplierCostMarkupValue").errors.min);
      }
    }

    function PricingSectionComponent_ng_template_15_div_31_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "select", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, PricingSectionComponent_ng_template_15_div_31_option_3_Template, 2, 2, "option", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, PricingSectionComponent_ng_template_15_div_31_div_8_Template, 3, 2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r139 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r139.RackAvgTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r139.TierPricingTypeForm.get("SupplierCostMarkupValue").touched && ctx_r139.TierPricingTypeForm.get("SupplierCostMarkupValue").errors);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_9_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r176 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", item_r176.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r176.Name, " ");
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_9_option_1_Template, 2, 2, "option", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r176 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", item_r176.IsWithinState);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_11_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var item_r179 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", item_r179.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r179.Name, " ");
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_11_option_1_Template, 2, 2, "option", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r179 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !item_r179.IsWithinState);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_div_12_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PricingSectionComponent_ng_template_15_div_32_div_11_div_12_span_1_Template, 2, 0, "span", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r175 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r175.TierPricingTypeForm.get("CityGroupTerminalId").errors.required);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_div_11_Template(rf, ctx) {
      if (rf & 1) {
        var _r184 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "select", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_15_div_32_div_11_Template_select_change_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r184);

          var ctx_r183 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r183.cityRackTerminalChanged_tpt();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "option", 140);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, " Select ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "option", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, " City Rack/Terminal ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "optgroup", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_9_Template, 2, 1, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "optgroup", 143);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PricingSectionComponent_ng_template_15_div_32_div_11_ng_container_11_Template, 2, 1, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PricingSectionComponent_ng_template_15_div_32_div_11_div_12_Template, 2, 1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r168 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r168.tierCityGroupTerminalsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r168.tierCityGroupTerminalsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r168.TierPricingTypeForm.get("CityGroupTerminalId").touched && ctx_r168.TierPricingTypeForm.get("CityGroupTerminalId").errors);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_ng_template_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "a", 109);
      }

      if (rf & 2) {
        var item_r185 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", item_r185.Name, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_ng_template_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "div", 109);
      }

      if (rf & 2) {
        var notFound_r186 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("innerHTML", notFound_r186, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeHtml"]);
      }
    }

    function PricingSectionComponent_ng_template_15_div_32_Template(rf, ctx) {
      if (rf & 1) {
        var _r188 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "input", 130);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_15_div_32_Template_input_change_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r188);

          var ctx_r187 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          ctx_r187.setRackTerminalValidation_tpt();
          return ctx_r187.getCityGroupTerminals_tpt();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, " City Rack/Terminal ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PricingSectionComponent_ng_template_15_div_32_div_11_Template, 13, 3, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 181);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "ng-autocomplete", 182);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_15_div_32_Template_ng_autocomplete_click_15_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r188);

          var ctx_r189 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r189.getApprovedTerminal_tpt();
        })("selected", function PricingSectionComponent_ng_template_15_div_32_Template_ng_autocomplete_selected_15_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r188);

          var ctx_r190 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r190.onApprovedTerminalSelected_tpt($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PricingSectionComponent_ng_template_15_div_32_ng_template_16_Template, 1, 1, "ng-template", null, 135, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, PricingSectionComponent_ng_template_15_div_32_ng_template_18_Template, 1, 1, "ng-template", null, 136, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r169 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](17);

        var _r171 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](19);

        var ctx_r140 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????classMap"](ctx_r140.TierPricingTypeForm.get("FuelPricingDetails").get("PricingSourceId").value == 2 ? "pntr-none subSectionOpacity" : "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx_r140.TierPricingTypeForm.get("FuelPricingDetails").get("PricingSourceId").value == 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r140.TierPricingTypeForm.get("EnableCityRack").value == true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r140.tierApprovedTerminalList)("searchKeyword", "Name")("itemTemplate", _r169)("notFoundTemplate", _r171);
      }
    }

    function PricingSectionComponent_ng_template_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r193 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 145);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "h4", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Set Tier Price");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "button", 174);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_15_Template_button_click_3_listener() {
          var modal_r134 = ctx.$implicit;
          return modal_r134.dismiss("Cross click");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "span", 148);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "\xD7");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "form", 175);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, PricingSectionComponent_ng_template_15_div_8_Template, 2, 0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "select", 176);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function PricingSectionComponent_ng_template_15_Template_select_change_18_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r193);

          var ctx_r192 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r192.pricingTypeChanged_tpt($event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Market Based");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Fuel Cost");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "option", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, "Fixed");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](25, PricingSectionComponent_ng_template_15_div_25_Template, 22, 8, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](26, PricingSectionComponent_ng_template_15_div_26_Template, 9, 2, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](29, "span", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](30, PricingSectionComponent_ng_template_15_div_30_Template, 11, 2, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](31, PricingSectionComponent_ng_template_15_div_31_Template, 9, 2, "div", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](32, PricingSectionComponent_ng_template_15_div_32_Template, 20, 8, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 177);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "input", 178);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_15_Template_input_click_37_listener() {
          var modal_r134 = ctx.$implicit;
          return modal_r134.dismiss("Cross click");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "input", 179);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_ng_template_15_Template_input_click_38_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r193);

          var ctx_r195 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r195.setPricing();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx_r4.TierPricingTypeForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4._loaderTierPricingType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.TierPricingTypeForm.get("PricingTypeId").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.TierPricingTypeForm.get("PricingTypeId").value == 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.TierPricingTypeForm.get("PricingTypeId").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.TierPricingTypeForm.get("PricingTypeId").value == 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.TierPricingTypeForm.get("PricingTypeId").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx_r4.TierPricingTypeForm.invalid);
      }
    }

    var PricingSectionComponent = /*#__PURE__*/function () {
      function PricingSectionComponent(fb, addLocationService, modalService, changeDetectorRef) {
        _classCallCheck(this, PricingSectionComponent);

        this.fb = fb;
        this.addLocationService = addLocationService;
        this.modalService = modalService;
        this.changeDetectorRef = changeDetectorRef;
        this.formSubmited = false;
        this.UoM = 0;
        this.IsTBD = false;
        this.DetectFormChange = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.UpdateSuppressPricingChange = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this._loadingPricingCodes = false;
        this.CityGroupTerminalsList = [];
        this.approvedTerminalList = [];
        this._minimumConst = 0.00001;
        this._zeroConst = 0;
        this.CountryBasedZipcodeLabel = "Zip";
        this._loadingAddress = false;
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_3__().add(1, 'year').toDate(); //pricing

        this.pricingCodes = [];
        this.pricingfeedTypeId = 0;
        this.pricingfuelClassTypeId = 0;
        this.pricingCodesArr = [{
          "PricingTypes": {
            "Fixed": {
              "Id": 1,
              "Code": "A-120000"
            },
            "FuelCost": {
              "Id": 4,
              "Code": "A-140000"
            }
          }
        }];
        this.RackAvgTypes = _carrier_schedule_builder_add_location_add_location_model__WEBPACK_IMPORTED_MODULE_2__["RackAvgTypes"]; //TIER PRICING TYPE

        this.tierApprovedTerminalList = [];
        this.tierCityGroupTerminalsList = [];
        this.selectedPricingIndex = null;
        this._loaderTierPricingType = false;
      }

      _createClass(PricingSectionComponent, [{
        key: "f",
        get: function get() {
          return this.locationForm['controls'];
        }
      }, {
        key: "fuelPricingForm",
        get: function get() {
          return this.f.FuelPricingDetails['controls'];
        }
      }, {
        key: "tierPricingForm",
        get: function get() {
          return this.fuelPricingForm.TierPricing['controls'];
        }
      }, {
        key: "ngOnInit",
        value: function ngOnInit() {
          this.TierPricingTypeForm = this.initailizeTierPricingTypeForm();
        }
      }, {
        key: "pricingTypeChanged",
        value: function pricingTypeChanged(type) {
          this.setPricingValidation(type);
          this.initilizeMarketBasedPrice();

          if (type == 1) {
            this.getPricingCodes();
          } else {
            this.setPricingCode();
          }
        }
      }, {
        key: "updateFormControlValidators",
        value: function updateFormControlValidators(control, validators) {
          control.setValidators(validators);
          control.updateValueAndValidity();
        }
      }, {
        key: "openPriceCodeModal",
        value: function openPriceCodeModal(pricingcodeModal) {
          this.getPricingCodes();
          this.modalService.open(pricingcodeModal, {
            windowClass: 'pricingcode-modal',
            size: 'lg',
            scrollable: true,
            backdrop: 'static',
            keyboard: false
          });
        }
      }, {
        key: "updateFuelType",
        value: function updateFuelType(pricingSourceId) {
          var fuelTypeId = this.tbdDrProductId;
          var fuelDetails = this.productDetails.find(function (t) {
            return t.PricingSourceId == pricingSourceId && t.TfxProductId == fuelTypeId;
          });
          this.f.FuelDetails.get('FuelTypeId').setValue(fuelDetails.Id);
          this.f.FuelDetails.get('FuelDisplayGroupId').setValue(fuelDetails.DisplayGroupId);
        }
      }, {
        key: "getSelectedPricingCode",
        value: function getSelectedPricingCode(item) {
          document.getElementById('pricingcodeModal').click();

          if (!this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            //this.modalService.dismissAll();
            var pricingCodeDisplayData = this.getPricingDisplayData(item);

            if (item) {
              this.f.FuelPricingDetails.get('TempPricingCodeDetails').patchValue(item);
              this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
              this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id); //this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);

              this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
              var existingPricingSource = this.f.FuelPricingDetails.get('FuelPricingDetails.PricingSourceId').value;
              this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
              this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

              if (this.IsTBD && this.tbdDrProductTypeId != 10) {
                this.updateFuelType(item.PricingSourceId);
              }

              if (existingPricingSource != item.PricingSourceId) {
                this.getCityGroupTerminals();
              }

              this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({
                Id: item.Id,
                Code: item.Code,
                Description: pricingCodeDisplayData
              });
            }

            if (item.PricingSourceId == 1) {
              this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
            } else {
              this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
            }

            this.setRackTerminalValidation();
          } else {
            var _pricingCodeDisplayData = this.getPricingDisplayData(item);

            if (item) {
              this.TierPricingTypeForm.get('TempPricingCodeDetails').patchValue(item);
              this.TierPricingTypeForm.get('Code').patchValue(item.Code);
              this.TierPricingTypeForm.get('CodeId').patchValue(item.Id); //this.TierPricingTypeForm.get('PricingTypeId').patchValue(item.PricingTypeId);

              this.TierPricingTypeForm.get('CodeDescription').patchValue(_pricingCodeDisplayData);
              this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
              this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);
              this.getCityGroupTerminals_tpt();
              this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({
                Id: item.Id,
                Code: item.Code,
                Description: _pricingCodeDisplayData
              });
            }

            if (item.PricingSourceId == 1) {
              this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
            } else {
              this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
            }

            this.setRackTerminalValidation_tpt();
          }
        }
      }, {
        key: "onPricingCodeSelected",
        value: function onPricingCodeSelected(item) {
          var pricingCodeDisplayData = this.getPricingDisplayData(item);
          this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
          this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
          this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
          this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
          this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
          this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({
            Id: item.Id,
            Code: item.Code,
            Description: pricingCodeDisplayData
          });
          this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

          if (item.PricingSourceId == 1) {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
          } else {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
          }

          this.setRackTerminalValidation();
          this.getCityGroupTerminals();
        }
      }, {
        key: "onApprovedTerminalSelected",
        value: function onApprovedTerminalSelected(event) {
          this.f.FuelPricingDetails.get('TerminalName').patchValue(event.Name);
          this.f.FuelPricingDetails.get('TerminalId').patchValue(event.Id);
        }
      }, {
        key: "getCityGroupTerminals",
        value: function getCityGroupTerminals() {
          var _this2 = this;

          this.CityGroupTerminalsList = [];
          var selectedState = this.f.AddressDetails.get('StateId').value;

          if (selectedState > 0) {
            this.addLocationService.GetCityGroupTerminals(selectedState, false, this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value).subscribe(function (data) {
              if (data && data.length > 0) {
                _this2.CityGroupTerminalsList = data;
              }
            });
          }
        }
      }, {
        key: "setPricingCode",
        value: function setPricingCode() {
          if (this.locationForm.get('FuelPricingDetails.PricingTypeId').value != 1) {
            var pricingCode = this.getPricingCode(this.locationForm.get('FuelPricingDetails.PricingTypeId').value, this.locationForm.get('FuelPricingDetails.FuelPricingDetails.PricingSourceId').value);

            if (pricingCode != null) {
              this.locationForm.get('FuelPricingDetails.FuelPricingDetails.PricingCode').patchValue(pricingCode);
            }
          }
        }
      }, {
        key: "getPricingCode",
        value: function getPricingCode(pricingTypeId, pricingSourceId) {
          var pricingType = pricingTypeId == 2 ? 'Fixed' : pricingTypeId == 4 ? 'FuelCost' : '';
          if (pricingType == '') return null;
          var pricing = this.pricingCodesArr.map(function (prc) {
            return prc.PricingTypes[pricingType];
          });
          if (pricing.length > 0) pricing = pricing[0];else pricing = null;
          return pricing;
        }
      }, {
        key: "getPricingDisplayData",
        value: function getPricingDisplayData(item) {
          var displayData = '';

          if (item != undefined || item != null) {
            if (item.PricingTypeId == 2) {
              displayData += item.PricingSource + ', ' + "Fixed";
            } else if (item.PricingTypeId == 4) {
              displayData += item.PricingSource + ', ' + "Fuel Cost";
            } else if (item.PricingTypeId == 1) {
              displayData += item.PricingSource + ', ' + item.RackAvgPricingType;

              if (item.PricingSourceId == 2 || item.PricingSourceId == 3) {
                displayData += ', ' + item.FeedType + ', ' + item.WeekendPricingDay;
              }

              if (item.PricingSourceId == 2) {
                displayData += ', ' + item.FuelClassType + ', ' + item.QuantityIndicator;
              }
            }
          }

          return displayData;
        }
      }, {
        key: "removePricingValidation",
        value: function removePricingValidation() {
          this.updateFormControlValidators(this.f.FuelPricingDetails.get('PricePerGallon'), []); //this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId'), []);

          this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupValue'), []);
          this.updateFormControlValidators(this.f.FuelPricingDetails.get('RackPrice'), []);
          this.updateFormControlValidators(this.f.FuelPricingDetails.get('Code'), []);
        }
      }, {
        key: "setPricingValidation",
        value: function setPricingValidation(type) {
          this.removePricingValidation();

          if (!this.f.IsSupressOrderPricing.value) {
            //Market Based
            if (type == 1) {
              this.updateFormControlValidators(this.f.FuelPricingDetails.get('RackPrice'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._zeroConst)]);
              this.updateFormControlValidators(this.f.FuelPricingDetails.get('Code'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
              this.f.FuelPricingDetails.get('RackAvgTypeId').setValue(1);
            } //Fuel cost
            else if (type == 4) {
              //this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId'), [Validators.required]);
              this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupValue'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._zeroConst)]);
              this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId').setValue(1);
            } //Fixed
            else if (type == 2) {
              this.updateFormControlValidators(this.f.FuelPricingDetails.get('PricePerGallon'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._minimumConst)]);
            }
          }
        }
      }, {
        key: "initilizeMarketBasedPrice",
        value: function initilizeMarketBasedPrice() {
          this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
          this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), []);
          this.f.FuelPricingDetails.get('Code').patchValue(null);
          this.f.FuelPricingDetails.get('CodeId').patchValue(null);
          this.f.FuelPricingDetails.get('CodeDescription').patchValue(null);
          this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({
            Id: null,
            Code: null,
            Description: null
          });
          this.f.FuelPricingDetails.get('TempPricingCodeDetails').patchValue(null);
        }
      }, {
        key: "setRackTerminalValidation",
        value: function setRackTerminalValidation() {
          var isChecked = this.f.FuelPricingDetails.get('EnableCityRack').value;

          if (isChecked) {
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
          } else {
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), []);
          }
        }
      }, {
        key: "cityRackTerminalChanged",
        value: function cityRackTerminalChanged() {
          var _this3 = this;

          var jobid = this.f.AddressDetails.get('JobId').value,
              fueltypeId = this.f.FuelDetails.get('FuelTypeId').value,
              selectedCityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value,
              lattitude = this.f.AddressDetails.get('Latitude').value,
              longitude = this.f.AddressDetails.get('Longitude').value,
              countryCode = this.f.AddressDetails.get('CountryCode').value,
              _countryCode = countryCode == "1" ? "USA" : "CAN",
              sourceId = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value;

          this.addLocationService.IsCityGroupTerminalPriceAvailable(jobid ? jobid : 0, fueltypeId, selectedCityRackId, lattitude, longitude, _countryCode, sourceId).subscribe(function (response) {
            if (response == false) {
              _this3.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgwarning("Pricing not available for this City Rack/Terminal. Try to assign another City Rack/Terminal.", null, null);
            }
          });

          if (sourceId != 1) {
            this.getOpisTerminals();
          }
        }
      }, {
        key: "getOpisTerminals",
        value: function getOpisTerminals() {
          var _this4 = this;

          var cityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value || 0,
              latitude = this.f.AddressDetails.get('Latitude').value,
              longitude = this.f.AddressDetails.get('Longitude').value,
              countryId = this.f.AddressDetails.get('CountryId').value,
              source = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value;
          this.addLocationService.getOpisTerminals(cityRackId, latitude, longitude, countryId, '', source).subscribe(function (response) {
            if (response) {
              _this4.approvedTerminalList = response;
            }
          });
        }
      }, {
        key: "getApprovedTerminal",
        value: function getApprovedTerminal() {
          var _this5 = this;

          var pricingSourceId = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value || 0;

          if (pricingSourceId == 1) {
            var pricingCodeId = this.f.FuelPricingDetails.get('CodeId').value || 0;
            var fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value || 0;
            var latitude = this.f.AddressDetails['controls']['Latitude'].value || 0;
            var longitude = this.f.AddressDetails['controls']['Longitude'].value || 0;
            var countryId = this.f.AddressDetails.get('CountryId').value || 0;
            var terminal = this.f.FuelPricingDetails.get('TerminalId').value || '';
            var cityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value || '';
            this.addLocationService.getClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId, cityRackId).subscribe(function (data) {
              if (data) {
                _this5.approvedTerminalList = data;
              }
            });
          }
        }
      }, {
        key: "addRemoveTierFee",
        value: function addRemoveTierFee(isAdd) {
          var tierPricingArray = this.tierPricingForm.Pricings;
          var arrayLength = tierPricingArray.controls.length;
          var lastIndex = arrayLength - 1;
          var lastRow = tierPricingArray.controls[lastIndex];
          var secondLastRow = tierPricingArray.controls[lastIndex - 1];
          var thirdLastRow = tierPricingArray.controls[lastIndex - 2];

          if (isAdd) {
            var _tierPricingArray = this.tierPricingForm.Pricings;
            var newRow = this.getTierPricingForm(false);
            newRow.get('FromQuantity').setValue(secondLastRow.get('ToQuantity').value);
            lastRow.get('FromQuantity').setValue(0);
            lastRow.get('Quantity').setValue(0);

            _tierPricingArray.insert(lastIndex, newRow);
          } else if (arrayLength > 2) {
            lastRow.get('FromQuantity').setValue(thirdLastRow.get('ToQuantity').value);
            lastRow.get('Quantity').setValue(thirdLastRow.get('ToQuantity').value);
            tierPricingArray.removeAt(lastIndex - 1);
          }
        }
      }, {
        key: "toQuantityChanged",
        value: function toQuantityChanged(value, index) {
          var nextPricingRow = this.tierPricingForm.Pricings.controls[index + 1];

          if (nextPricingRow) {
            nextPricingRow.get('FromQuantity').setValue(value);
            nextPricingRow.get('Quantity').setValue(value);
          }
        }
      }, {
        key: "getTierPricingForm",
        value: function getTierPricingForm(isAboveQuantityRow) {
          var validators = [];
          isAboveQuantityRow ? null : validators.push(_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required);
          return this.fb.group({
            RequestPriceDetailId: this.fb.control(null),
            FromQuantity: this.fb.control(0, validators),
            ToQuantity: this.fb.control(0, validators),
            Quantity: this.fb.control(null),
            TerminalId: this.fb.control(null),
            TerminalName: this.fb.control(null),
            TempTerminalName: this.fb.control(null),
            CityGroupTerminalId: this.fb.control(null),
            CityGroupTerminalStateId: this.fb.control(null),
            DisplayPrice: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]),
            RackAvgTypeId: this.fb.control(null),
            PricePerGallon: this.fb.control(0),
            SupplierCostMarkupTypeId: this.fb.control(null),
            SupplierCostMarkupValue: this.fb.control(0),
            MarginTypeId: this.fb.control(null),
            Margin: this.fb.control(null),
            BasePrice: this.fb.control(null),
            RackPrice: this.fb.control(0),
            BaseSupplierCost: this.fb.control(null),
            Currency: this.fb.control(0),
            ExchangeRate: this.fb.control(null),
            UoM: this.fb.control(0),
            FuelTypeId: this.fb.control(null),
            JobId: this.fb.control(0),
            PricingSourceId: this.fb.control(1),
            PricingTypeId: this.fb.control(src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__["PricingType"].PricePerGallon),
            //RackTypeId : this.fb.control(null),
            IncludeTaxes: this.fb.control(false),
            IsActive: this.fb.control(false),
            IsAboveQuantity: this.fb.control(isAboveQuantityRow),
            RowIndex: this.fb.control(1),
            TotalRows: this.fb.control(null),
            EstimatedPPG: this.fb.control(null),
            //PricingCode: this.fb.control({ Id: null, Code: null, Description: null }),
            PricingCode: this.fb.control({
              Id: this.pricingCodesArr[0].PricingTypes.Fixed.Id,
              Code: this.pricingCodesArr[0].PricingTypes.Fixed.Code,
              Description: null
            }),
            TempPricingCodeDetails: this.fb.control(null),
            SupplierCost: this.fb.control(null),
            SupplierCostTypeId: this.fb.control(null),
            CreationTimeRackPPG: this.fb.control(null),
            MarkertBasedPricingTypeId: this.fb.control(null),
            IsEdit: this.fb.control(false),
            CityGroupTerminalName: this.fb.control(null),
            FuelDisplayGroupId: this.fb.control(0),
            IsTerminalPrice: this.fb.control(null),
            IsSupplierCostPrice: this.fb.control(null),
            IsFixedPrice: this.fb.control(null)
          });
        } //TIER PRICING TYPE 

      }, {
        key: "initailizeTierPricingTypeForm",
        value: function initailizeTierPricingTypeForm(selectedRow) {
          var _form = this.fb.group({
            Id: this.fb.control(null),
            PricingTypeId: this.fb.control(2, [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]),
            PricePerGallon: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._minimumConst)]),
            Code: this.fb.control(null),
            CodeId: this.fb.control(null),
            CodeDescription: this.fb.control(null),
            TempPricingCodeDetails: this.fb.control(null),
            RackAvgTypeId: this.fb.control(1),
            RackPrice: this.fb.control(0),
            EnableCityRack: this.fb.control(null),
            TerminalName: this.fb.control(null),
            TerminalId: this.fb.control(null),
            TempTerminalName: this.fb.control(null),
            SupplierCostMarkupTypeId: this.fb.control(1),
            SupplierCostMarkupValue: this.fb.control(0),
            SupplierCost: this.fb.control(null),
            CityGroupTerminalId: this.fb.control(null),
            FuelPricingDetails: this.fb.group({
              PricingSourceId: this.fb.control(1),
              PricingCode: this.fb.control({
                Id: null,
                Code: null,
                Description: null
              })
            })
          });

          if (selectedRow) {
            _form.get('TerminalName').setValue(selectedRow.TerminalName);

            _form.get('TerminalId').setValue(selectedRow.TerminalId);

            _form.get('CityGroupTerminalId').setValue(selectedRow.CityGroupTerminalId);

            _form.get('RackAvgTypeId').setValue(selectedRow.RackAvgTypeId);

            _form.get('PricePerGallon').setValue(selectedRow.PricePerGallon);

            _form.get('SupplierCostMarkupTypeId').setValue(selectedRow.SupplierCostMarkupTypeId);

            _form.get('SupplierCostMarkupValue').setValue(selectedRow.SupplierCostMarkupValue);

            _form.get('SupplierCost').setValue(selectedRow.SupplierCost);

            _form.get('RackPrice').setValue(selectedRow.RackPrice);

            _form.get('PricingTypeId').setValue(selectedRow.PricingTypeId);

            _form.get('TempPricingCodeDetails').setValue(selectedRow.TempPricingCodeDetails); //_form.get('PricingSourceId').setValue(data.FuelPricingDetails.PricingSourceId);


            _form.get('FuelPricingDetails').get('PricingSourceId').setValue(selectedRow.PricingSourceId); //_form.get('PricingCode').setValue(data.FuelPricingDetails.PricingCode);


            _form.get('FuelPricingDetails').get('PricingCode').setValue(selectedRow.PricingCode);
          }

          return _form;
        }
      }, {
        key: "setPricingSourceClicked_tpt",
        value: function setPricingSourceClicked_tpt(index, SetTierPriceType) {
          this.selectedPricingIndex = index; //set form

          var selectedRow = this.tierPricingForm.Pricings.controls[this.selectedPricingIndex].getRawValue(); //selectedRow.PricingSourceId = 1;

          this.TierPricingTypeForm = this.initailizeTierPricingTypeForm(selectedRow);
          var control = this.TierPricingTypeForm.get('PricingTypeId');
          this.setPricingValidation_tpt(this.TierPricingTypeForm.get('PricingTypeId').value); //CITY RACK FOR SOURCING REQ

          if (selectedRow.PricingCode && selectedRow.PricingCode.Code && selectedRow.PricingCode.Code.startsWith("O")) {
            this.getCityGroupTerminals_tpt();
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
            this.TierPricingTypeForm.get('CityGroupTerminalId').setValue(null);
            this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
            this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').setValue(2);
          } //this.TierPricingTypeForm.get('TempTerminalName').setValue(selectedRow.TempTerminalName || selectedRow.TempTerminalName)
          //open modal


          this.modalService.open(SetTierPriceType, {
            windowClass: 'pricingcode-modal',
            size: 'lg',
            scrollable: true
          });
          this.getPricingCodes();
        }
      }, {
        key: "pricingTypeChanged_tpt",
        value: function pricingTypeChanged_tpt(type) {
          this.setPricingValidation_tpt(type);
          this.initilizeMarketBasedPrice_tpt();

          if (type == 1) {
            this.getPricingCodes();
          } else {
            this.setPricingCode_tpt();
          }
        }
      }, {
        key: "onPricingCodeSelected_tpt",
        value: function onPricingCodeSelected_tpt(item) {
          var pricingCodeDisplayData = this.getPricingDisplayData(item);
          this.TierPricingTypeForm.get('Code').patchValue(item.Code);
          this.TierPricingTypeForm.get('CodeId').patchValue(item.Id);
          this.TierPricingTypeForm.get('PricingTypeId').patchValue(item.PricingTypeId);
          this.TierPricingTypeForm.get('CodeDescription').patchValue(pricingCodeDisplayData);
          this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
          this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({
            Id: item.Id,
            Code: item.Code,
            Description: pricingCodeDisplayData
          });
          this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

          if (item.PricingSourceId == 1) {
            this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
          } else {
            this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
          }

          this.setRackTerminalValidation_tpt();
          this.getCityGroupTerminals_tpt();
        }
      }, {
        key: "openPriceCodeModal_tpt",
        value: function openPriceCodeModal_tpt(pricingcodeModal) {
          this.getPricingCodes();
          this.modalService.open(pricingcodeModal, {
            windowClass: 'pricingcode-modal',
            size: 'lg',
            scrollable: true,
            backdrop: 'static',
            keyboard: false
          });
        }
      }, {
        key: "setRackTerminalValidation_tpt",
        value: function setRackTerminalValidation_tpt() {
          var isChecked = this.TierPricingTypeForm.get('EnableCityRack').value;

          if (isChecked) {
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
          } else {
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), []);
          }
        }
      }, {
        key: "cityRackTerminalChanged_tpt",
        value: function cityRackTerminalChanged_tpt() {
          var _this6 = this;

          var jobid = this.f.AddressDetails.get('JobId').value,
              fueltypeId = this.f.FuelDetails.get('FuelTypeId').value,
              selectedCityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value,
              lattitude = this.f.AddressDetails.get('Latitude').value,
              longitude = this.f.AddressDetails.get('Longitude').value,
              countryCode = this.f.AddressDetails.get('CountryCode').value,
              _countryCode = countryCode == "1" ? "USA" : "CAN",
              sourceId = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value;

          this._loaderTierPricingType = true;
          this.addLocationService.IsCityGroupTerminalPriceAvailable(jobid ? jobid : 0, fueltypeId, selectedCityRackId, lattitude, longitude, _countryCode, sourceId).subscribe(function (response) {
            _this6._loaderTierPricingType = false;

            if (response == false) {
              _this6.TierPricingTypeForm.get('CityGroupTerminalId').setValue(null);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgwarning("Pricing not available for this City Rack/Terminal. Try to assign another City Rack/Terminal.", null, null);
            }
          });

          if (sourceId != 1) {
            this.getOpisTerminals_tpt();
          }
        }
      }, {
        key: "getApprovedTerminal_tpt",
        value: function getApprovedTerminal_tpt() {
          var _this7 = this;

          var pricingSourceId = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value || 0;

          if (pricingSourceId == 1) {
            var pricingCodeId = this.TierPricingTypeForm.get('CodeId').value || 0;
            var fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value || 0;
            var latitude = this.f.AddressDetails['controls']['Latitude'].value || 0;
            var longitude = this.f.AddressDetails['controls']['Longitude'].value || 0;
            var countryId = this.f.AddressDetails.get('CountryId').value || 0;
            var terminal = this.TierPricingTypeForm.get('TerminalId').value || '';
            var cityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value || '';
            this.addLocationService.getClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId, cityRackId).subscribe(function (data) {
              if (data) {
                _this7.tierApprovedTerminalList = data;
              }
            });
          }
        }
      }, {
        key: "onApprovedTerminalSelected_tpt",
        value: function onApprovedTerminalSelected_tpt(event) {
          this.TierPricingTypeForm.get('TerminalName').patchValue(event.Name);
          this.TierPricingTypeForm.get('TerminalId').patchValue(event.Id);
        }
      }, {
        key: "removePricingValidation_tpt",
        value: function removePricingValidation_tpt() {
          this.updateFormControlValidators(this.TierPricingTypeForm.get('PricePerGallon'), []);
          this.updateFormControlValidators(this.TierPricingTypeForm.get('SupplierCostMarkupValue'), []);
          this.updateFormControlValidators(this.TierPricingTypeForm.get('RackPrice'), []);
          this.updateFormControlValidators(this.TierPricingTypeForm.get('Code'), []);
          this.updateFormControlValidators(this.TierPricingTypeForm.get('TempPricingCodeDetails'), []);
        }
      }, {
        key: "setPricingValidation_tpt",
        value: function setPricingValidation_tpt(type) {
          this.removePricingValidation_tpt();

          if (!this.f.IsSupressOrderPricing.value) {
            //Market Based
            if (type == 1) {
              this.updateFormControlValidators(this.TierPricingTypeForm.get('RackPrice'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._zeroConst)]); //REMOVED FOR SOURCING
              //this.updateFormControlValidators(this.TierPricingTypeForm.get('Code'), [Validators.required]);

              this.updateFormControlValidators(this.TierPricingTypeForm.get('TempPricingCodeDetails'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
              this.TierPricingTypeForm.get('RackAvgTypeId').setValue(1);
            } //Fuel cost
            else if (type == 4) {
              this.updateFormControlValidators(this.TierPricingTypeForm.get('SupplierCostMarkupValue'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._zeroConst)]);
              this.TierPricingTypeForm.get('SupplierCostMarkupTypeId').setValue(1);
            } //Fixed
            else if (type == 2) {
              this.updateFormControlValidators(this.TierPricingTypeForm.get('PricePerGallon'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].min(this._minimumConst)]);
            }
          }
        }
      }, {
        key: "getOpisTerminals_tpt",
        value: function getOpisTerminals_tpt() {
          var _this8 = this;

          var cityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value || 0,
              latitude = this.f.AddressDetails.get('Latitude').value,
              longitude = this.f.AddressDetails.get('Longitude').value,
              countryId = this.f.AddressDetails.get('CountryId').value,
              source = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value;
          this.addLocationService.getOpisTerminals(cityRackId, latitude, longitude, countryId, '', source).subscribe(function (response) {
            if (response) {
              _this8.tierApprovedTerminalList = response;
            }
          });
        }
      }, {
        key: "getCityGroupTerminals_tpt",
        value: function getCityGroupTerminals_tpt() {
          var _this9 = this;

          this.tierCityGroupTerminalsList = [];
          var selectedState = this.f.AddressDetails.get('StateId').value;

          if (selectedState > 0) {
            this.addLocationService.GetCityGroupTerminals(selectedState, false, this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value).subscribe(function (data) {
              if (data && data.length > 0) {
                _this9.tierCityGroupTerminalsList = data;
              }
            });
          }
        }
      }, {
        key: "initilizeMarketBasedPrice_tpt",
        value: function initilizeMarketBasedPrice_tpt() {
          this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
          this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), []);
          this.TierPricingTypeForm.get('Code').patchValue(null);
          this.TierPricingTypeForm.get('CodeId').patchValue(null);
          this.TierPricingTypeForm.get('CodeDescription').patchValue(null);
          this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({
            Id: null,
            Code: null,
            Description: null
          });
          this.TierPricingTypeForm.get('TempPricingCodeDetails').patchValue(null);
        }
      }, {
        key: "setPricingCode_tpt",
        value: function setPricingCode_tpt() {
          if (this.TierPricingTypeForm.get('PricingTypeId').value != 1) {
            var pricingCode = this.getPricingCode(this.TierPricingTypeForm.get('PricingTypeId').value, this.TierPricingTypeForm.get('FuelPricingDetails.PricingSourceId').value);

            if (pricingCode != null) {
              this.TierPricingTypeForm.get('FuelPricingDetails.PricingCode').patchValue(pricingCode);
            }
          }
        }
      }, {
        key: "getRackAvgTypeNameById",
        value: function getRackAvgTypeNameById(id, price) {
          var response = '';
          var rack = this.RackAvgTypes.find(function (r) {
            return r.Id == id;
          });

          if (rack) {
            //let name = rack.Name;
            if (id == 1) {
              response = '+ $' + price;
            } else if (id == 2) {
              response = '- $' + price;
            } else if (id == 3) {
              response = '+ ' + price + '%';
            } else if (id == 4) {
              response = '- ' + price + '%';
            }
          }

          return response;
        }
      }, {
        key: "cumulationTypeChanged",
        value: function cumulationTypeChanged(checked, cumulationType) {
          this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Day'), []);
          this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Date'), []);

          if (checked) {
            //let cumulationType = this.tierPricingForm.ResetCumulationSetting.get('CumulationType').value
            if (cumulationType == 1 || cumulationType == 2) {
              this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Day'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
            } else if (cumulationType == 3 || cumulationType == 4) {
              this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Date'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
            }
          }
        }
      }, {
        key: "setPricing",
        value: function setPricing() {
          if (this.TierPricingTypeForm.invalid) {
            return;
          }

          this.modalService.dismissAll();
          var control = this.tierPricingForm.Pricings.controls[this.selectedPricingIndex];
          var sourceObj = this.TierPricingTypeForm.getRawValue();
          control.get('TerminalName').setValue(sourceObj.TerminalName);
          control.get('TerminalId').setValue(sourceObj.TerminalId);
          control.get('CityGroupTerminalId').setValue(sourceObj.CityGroupTerminalId);
          control.get('RackAvgTypeId').setValue(sourceObj.RackAvgTypeId);
          control.get('PricePerGallon').setValue(sourceObj.PricePerGallon);
          control.get('SupplierCostMarkupTypeId').setValue(sourceObj.SupplierCostMarkupTypeId);
          control.get('SupplierCostMarkupValue').setValue(sourceObj.SupplierCostMarkupValue);
          control.get('SupplierCost').setValue(sourceObj.SupplierCost);
          control.get('RackPrice').setValue(sourceObj.RackPrice);
          control.get('PricingSourceId').setValue(sourceObj.FuelPricingDetails.PricingSourceId);
          control.get('PricingCode').setValue(sourceObj.FuelPricingDetails.PricingCode);
          control.get('PricingTypeId').setValue(sourceObj.PricingTypeId);
          control.get('TempPricingCodeDetails').setValue(sourceObj.TempPricingCodeDetails); // control.patchValue(this.TierPricingTypeForm.value);
          // control.get('TempPricingCodeDetails').setValue(sourceObj.TempPricingCodeDetails);
          // control.get('Code').setValue(sourceObj.Code);
          // control.get('CodeId').setValue(sourceObj.CodeId);
          // control.get('CodeDescription').setValue(sourceObj.CodeDescription);
          // control.get('EnableCityRack').setValue(sourceObj.EnableCityRack);
          // control.get('FuelPricingDetails').patchValue(sourceObj.FuelPricingDetails);

          if (sourceObj.PricingTypeId == 1) {
            control.get('DisplayPrice').setValue('Rack Avg ' + this.getRackAvgTypeNameById(sourceObj.RackAvgTypeId, sourceObj.RackPrice));
          } else if (sourceObj.PricingTypeId == 4) {
            control.get('DisplayPrice').setValue('Fuel Cost ' + this.getRackAvgTypeNameById(sourceObj.SupplierCostMarkupTypeId, sourceObj.SupplierCostMarkupValue));
          } else {
            control.get('DisplayPrice').setValue('$' + sourceObj.PricePerGallon);
          }
        }
      }, {
        key: "getPricingCodes",
        value: function getPricingCodes() {
          var _this10 = this;

          var filterData = {};

          if (this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            filterData = {
              "PricingSourceId": this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value,
              "PricingTypeId": this.TierPricingTypeForm.get('PricingTypeId').value,
              "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
              "feedTypeId": this.pricingfeedTypeId,
              "fuelClassTypeId": this.pricingfuelClassTypeId,
              "Prefix": ""
            };
          } else {
            filterData = {
              "PricingSourceId": this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value,
              "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
              "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
              "feedTypeId": this.pricingfeedTypeId,
              "fuelClassTypeId": this.pricingfuelClassTypeId,
              "Prefix": ""
            };
          }

          this._loadingPricingCodes = true;
          this.addLocationService.getPricingCodes(filterData).subscribe(function (data) {
            _this10._loadingPricingCodes = false;

            if (data) {
              _this10.pricingCodes = data.PricingCodes;
            }
          });
        }
      }, {
        key: "toggleSuppressPricing",
        value: function toggleSuppressPricing(isChecked) {
          var isTierPriced = this.fuelPricingForm.IsTierPricingRequired.value;

          if (isChecked) {
            if (isTierPriced) {
              var pricing = this.tierPricingForm.Pricings;
              pricing.clear();
              this.removePricingValidation_tpt();
            } else {
              this.removePricingValidation();
            }
          } else {
            if (isTierPriced) {
              this.tierPricingEnabled(isTierPriced);
            } else {
              this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
            }
          }

          this.UpdateSuppressPricingChange.emit(isChecked);
        }
      }, {
        key: "tierPricingEnabled",
        value: function tierPricingEnabled(isChecked) {
          var pricing = this.tierPricingForm.Pricings;
          pricing.clear();

          if (isChecked) {
            pricing.push(this.getTierPricingForm(false));
            pricing.push(this.getTierPricingForm(true));
            this.removePricingValidation();
          } else {
            this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
          }

          this.changeDetectorRef.detectChanges();
        }
      }, {
        key: "patchExistingPricingDetails",
        value: function patchExistingPricingDetails(fuelPricingDetails) {
          var _this11 = this;

          var _a;

          fuelPricingDetails['TempPricingCodeDetails'] = fuelPricingDetails.Code;
          fuelPricingDetails['TempTerminalName'] = fuelPricingDetails.TerminalName;
          var code = {
            Code: fuelPricingDetails.Code,
            CodeDescription: fuelPricingDetails.CodeDescription,
            CodeId: fuelPricingDetails.CodeId
          };
          this.locationForm.get('FuelPricingDetails').patchValue(fuelPricingDetails);
          this.locationForm.get('FuelPricingDetails').get('FuelPricingDetails').get('PricingCode').patchValue(code); //CITY RACK

          if (fuelPricingDetails.EnableCityRack) {
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["Validators"].required]);
            this.getCityGroupTerminals();
          }

          if (fuelPricingDetails.Code && fuelPricingDetails.Code.startsWith("O")) {
            this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').setValue(2);
          } else {
            this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').setValue(1);
          } //TERMINAL DETAILS


          if (fuelPricingDetails.IsTierPricingRequired) {
            var pricing = this.tierPricingForm.Pricings;
            ((_a = fuelPricingDetails.TierPricing) === null || _a === void 0 ? void 0 : _a.Pricings) && fuelPricingDetails.TierPricing.Pricings.forEach(function (row) {
              //SET QUANTITY
              if (row.IsAboveQuantity) {
                row['Quantity'] = row['FromQuantity'];
              } //SET TEMP


              row['TempPricingCodeDetails'] = row['PricingCode'];
              row['TempTerminalName'] = row.TerminalName;

              var form = _this11.getTierPricingForm(row.IsAboveQuantity);

              form.patchValue(row); // //CITY RACK
              // if(row.EnableCityRack){
              //     this.updateFormControlValidators(form.get('CityGroupTerminalId'), [Validators.required]);
              //     form.get('CityGroupTerminalId').setValue(2)
              // }

              pricing.push(form);
            });
            this.removePricingValidation();
            this.changeDetectorRef.detectChanges();
          } else {
            this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
          }
        }
      }]);

      return PricingSectionComponent;
    }();

    PricingSectionComponent.??fac = function PricingSectionComponent_Factory(t) {
      return new (t || PricingSectionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_carrier_schedule_builder_add_location_add_location_service__WEBPACK_IMPORTED_MODULE_6__["AddLocationService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_7__["NgbModal"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]));
    };

    PricingSectionComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: PricingSectionComponent,
      selectors: [["app-pricing-section"]],
      inputs: {
        locationForm: "locationForm",
        formSubmited: "formSubmited",
        UoM: "UoM",
        IsTBD: "IsTBD",
        tbdDrProductTypeId: "tbdDrProductTypeId",
        tbdDrProductId: "tbdDrProductId",
        productDetails: "productDetails"
      },
      outputs: {
        DetectFormChange: "DetectFormChange",
        UpdateSuppressPricingChange: "UpdateSuppressPricingChange"
      },
      decls: 17,
      vars: 3,
      consts: [[1, "row", 3, "formGroup"], [1, "col-12"], [1, "well"], [1, "row"], [1, "col-sm-12"], [1, "col-sm-12", "form-check", "form-check-inline", "checkbox"], ["id", "IsSupressOrderPricing", "formControlName", "IsSupressOrderPricing", "type", "checkbox", 1, "form-check-input", 3, "disableControl", "click"], ["for", "IsSupressOrderPricing", 1, "form-check-label"], ["formGroupName", "FuelPricingDetails", 4, "ngIf"], ["pricingcodeModal", ""], ["SetTierPriceType", ""], ["formGroupName", "FuelPricingDetails"], ["id", "IsTierPricingRequired", "formControlName", "IsTierPricingRequired", "type", "checkbox", 1, "form-check-input", 3, "disableControl", "click"], ["for", "IsTierPricingRequired", 1, "form-check-label"], ["id", "tier-pricing-qty-section", "formGroupName", "TierPricing", 4, "ngIf"], ["class", "section-pricing", 4, "ngIf"], ["id", "tier-pricing-qty-section", "formGroupName", "TierPricing"], [1, "row", "mt5"], [1, "form-check", "form-check-inline", "radio", "volume-based-tier"], ["id", "FuelDetails_TierPricing_TierPricingType", "formControlName", "TierPricingType", "name", "TierPricingType", "type", "radio", 1, "form-check-input", 3, "disableControl", "value"], ["for", "radio-volumeBased", 1, "form-check-label"], [1, "form-check", "form-check-inline", "radio", "delivery-quantity-based-tier"], ["id", "FuelDetails_TierPricing_TierPricingType", "formControlName", "TierPricingType", "name", "TierPricingType", "type", "radio", 1, "form-check-input", 3, "value"], ["for", "radio-deliveredquantityrange", 1, "form-check-label"], [1, "tier-pricing-fuel-quantity"], [1, "col-sm-3"], ["class", "cumulative-delivered-quantity", 4, "ngIf"], ["class", "delivery-quantity-ranging", 4, "ngIf"], ["id", "tier-fuel-qty-section", 1, "partial-section", "quantity-range"], ["formArrayName", "Pricings", 1, "partial-block-collection-section", "mt5"], [4, "ngFor", "ngForOf"], ["class", "row reset-cumulation", 4, "ngIf"], [1, "cumulative-delivered-quantity"], [1, "delivery-quantity-ranging"], [3, "formGroupName"], [1, "partial-block"], [1, "pricing-row", "row"], [1, "tier-fuel-quantity-row", "col-sm-12", "idx-1"], [1, "col-sm-2", "subSectionOpacity", "pntr-none"], [1, "form-group"], ["class", "form-control", "formControlName", "FromQuantity", "placeholder", "From", "type", "text", 3, "disableControl", 4, "ngIf"], ["class", "form-control", "formControlName", "Quantity", "placeholder", "Above", "type", "text", 3, "disableControl", 4, "ngIf"], [1, "text-danger"], [1, "col-sm-1"], ["class", "mt7", 4, "ngIf"], [1, "col-sm-2"], ["class", "form-control to-quantity valid", "formControlName", "ToQuantity", "placeholder", "Up to", "type", "number", 3, "change", 4, "ngIf"], ["class", "text-danger", 4, "ngIf"], [1, "form-group", "mt7"], ["class", "lblDisplayPrice", 4, "ngIf"], ["type", "button", "data-target", "#set-tier-price", "data-backdrop", "static", "data-keyboard", "false", 1, "btn", "btn-primary", "btn-sm", 3, "click"], [1, "qty-add-btns", "fs18", "pl0", "col-sm-1"], [4, "ngIf"], [1, "ml10", "pricing-error-section", "col"], ["formControlName", "FromQuantity", "placeholder", "From", "type", "text", 1, "form-control", 3, "disableControl"], ["formControlName", "Quantity", "placeholder", "Above", "type", "text", 1, "form-control", 3, "disableControl"], [1, "mt7"], ["formControlName", "ToQuantity", "placeholder", "Up to", "type", "number", 1, "form-control", "to-quantity", "valid", 3, "change"], [1, "lblDisplayPrice"], ["type", "button", "title", "Add", 1, "fa", "fa-plus-circle", "mt4", "mr5", "mr10", 3, "click"], ["type", "button", "title", "Remove", 1, "fa", "fa-trash-alt", "mt7", "color-maroon", "remove-partial-block", "tier", 3, "click"], [1, "row", "reset-cumulation"], [1, "form-check", "form-check-inline", "checkbox"], ["id", "FuelDetails_TierPricing_IsResetCumulation", "formControlName", "IsResetCumulation", "type", "checkbox", "value", "true", 1, "form-check-input", 3, "change"], ["for", "IsResetCumulation", 1, "form-check-label"], ["id", "reset-cumulation-section", "formGroupName", "ResetCumulationSetting", 4, "ngIf"], ["id", "reset-cumulation-section", "formGroupName", "ResetCumulationSetting"], ["id", "FuelDetails_TierPricing_ResetCumulationSetting_CumulationType", "formControlName", "CumulationType", 1, "form-control", 3, "change"], ["value", "null", "disabled", ""], ["value", "1"], ["value", "2"], ["value", "3"], ["value", "4"], ["data-valmsg-for", "FuelDetails.TierPricing.ResetCumulationSetting.CumulationType", 1, "text-danger"], ["class", "col-sm-3 reset-cumulation-date", 4, "ngIf"], ["class", "col-sm-3 reset-cumulation-day", 4, "ngIf"], [1, "col-sm-3", "reset-cumulation-date"], ["type", "text", "formControlName", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "maxDate", "format", "onDateChange"], [1, "col-sm-3", "reset-cumulation-day"], ["formControlName", "Day", 1, "form-control", "reset-cumulation-day"], ["value", "5"], ["value", "6"], ["value", "7"], [1, "section-pricing"], [1, "row", "mt10"], ["id", "divPricingSection", 1, "col-sm-12"], [1, "single-delivery-sub-types"], ["formControlName", "PricingTypeId", 1, "form-control", 3, "disableControl", "change"], [3, "value"], ["class", "col-sm-6", 4, "ngIf"], ["class", "col-sm-3", 4, "ngIf"], [1, "pa", "bg-white", "subSectionOpacity", "mt10", "top0", "left0", "z-index5", "loading-wrapper", "calculate-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], ["class", "market-control", 4, "ngIf"], ["class", "row mt-2", 4, "ngIf"], ["class", "row cityrack-wrapper", 4, "ngIf"], [1, "col-sm-6"], [1, "col-sm-12", "pricing-codes"], ["for", "FuelPricingDetails_FuelPricingDetails_PricingCode_Pricing_Code"], ["aria-required", "true", 1, "required", "pl4"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper cityrack-loader", 4, "ngIf"], [1, "ng-autocomplete"], ["formControlName", "TempPricingCodeDetails", "placeholder", "Pricing code", 3, "data", "searchKeyword", "itemTemplate", "notFoundTemplate", "selected", "click"], ["itemTemplate", ""], ["notFoundTemplate", ""], [1, "col-sm-6", "div-selectpricingcode"], ["type", "button", 1, "btn", "btn-primary", "btn-sm", 3, "disabled", "click"], ["class", "col-sm-12", 4, "ngIf"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "cityrack-loader"], [3, "innerHTML"], [1, "input-group", "mtm1"], [1, "input-group-addon", "currency-symbol"], ["id", "FuelPricingDetails_PricePerGallon", "formControlName", "PricePerGallon", "type", "number", 1, "form-control", "datatype-decimal", "always"], [1, "market-control"], [1, "col-sm-3", "terminal-price", "market-control"], ["id", "FuelPricingDetails_RackAvgTypeId", "formControlName", "RackAvgTypeId", 1, "form-control"], [3, "value", 4, "ngFor", "ngForOf"], [1, "mtm1"], ["id", "FuelPricingDetails_RackPrice", "formControlName", "RackPrice", "placeholder", "Rack Price", "type", "number", 1, "form-control", "datatype-decimal", "always"], [1, "row", "mt-2"], [1, "col-sm-3", "supplier-cost", "cost-control"], ["id", "FuelPricingDetails_SupplierCostMarkupTypeId", "formControlName", "SupplierCostMarkupTypeId", 1, "form-control", "valid"], [1, "col-sm-3", "supplier-cost", "defaultDisabled", "cost-control"], [1, "mtm1", "defaultDisabled"], ["id", "FuelPricingDetails_SupplierCostMarkupValue", "formControlName", "SupplierCostMarkupValue", "type", "number", "autocomplete", "off", 1, "form-control", "datatype-decimal", "always"], [1, "row", "cityrack-wrapper"], [1, "col-sm-3", "terminal-price-check"], [1, "form-group", "mb5"], [1, ""], [1, "form-check", "form-check-inline", "mb10"], ["type", "checkbox", "formControlName", "EnableCityRack", "id", "chk-enable-cityrack", 1, "enablecityrack", "form-check-input", 3, "disableControl", "change"], ["for", "chk-enable-cityrack", 1, "form-check-label"], ["class", "pr", "id", "cityrack-div", 4, "ngIf"], ["id", "terminalContainer1", 1, "col-sm-4", "mb25"], ["placeholder", "Approved Terminals", 3, "data", "searchKeyword", "itemTemplate", "notFoundTemplate", "click", "selected"], ["TerminalItemTemplate", ""], ["TerminalnotFoundTemplate", ""], ["id", "cityrack-div", 1, "pr"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "cityrack-loader", "hide-element"], ["id", "FuelPricingDetails_CityGroupTerminalId", "formControlName", "CityGroupTerminalId", 1, "form-control", "enum-ddl", "qty-ind", 3, "change"], ["disabled", "", "value", "null"], ["selected", "", "disabled", "", "value", "noneselected", 1, "hidden"], ["label", "Within State"], ["label", "Other States"], [3, "value", 4, "ngIf"], [1, "modal-header"], ["id", "modal-basic-title", 1, "modal-title"], ["type", "button", "aria-label", "Close", "id", "pricingcodeModal", 1, "close", 3, "click"], ["aria-hidden", "true"], [1, "modal-body"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "float-left", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", "name", "pricing-feedfilter", "id", "filter_all", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_all", 1, "btn", "ml0"], ["type", "radio", "name", "pricing-feedfilter", "id", "filter_10am", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_10am", 1, "btn"], ["type", "radio", "name", "pricing-feedfilter", "id", "filter_5pm", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_5pm", 1, "btn"], ["type", "radio", "name", "pricing-feedfilter", "id", "filter_10am_pre", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_10am_pre", 1, "btn"], ["type", "radio", "name", "pricing-feedfilter", "id", "filter_5pm_pre", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_5pm_pre", 1, "btn"], ["type", "radio", "name", "pricing-fuelClassfilter", "id", "filter_brand_all", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_brand_all", 1, "btn", "ml0"], ["type", "radio", "name", "pricing-fuelClassfilter", "id", "filter_branded", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_branded", 1, "btn"], ["type", "radio", "name", "pricing-fuelClassfilter", "id", "filter_unbranded", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_unbranded", 1, "btn"], ["type", "radio", "name", "pricing-fuelClassfilter", "id", "filter_both", 1, "hide-element", 3, "ngModel", "value", "checked", "change", "ngModelChange"], ["for", "filter_both", 1, "btn"], ["class", "col-sm-4", 4, "ngFor", "ngForOf"], [1, "col-sm-4"], [1, "well", "click-card", 3, "click"], [1, "text-center", 3, "id"], ["type", "button", "aria-label", "Close", 1, "close", 3, "click"], [3, "formGroup"], ["formControlName", "PricingTypeId", 1, "form-control", 3, "change"], [1, "col-sm-12", "text-right"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["type", "button", "id", "SelectPricing", "value", "Select", 1, "btn", "btn-lg", "btn-primary", 3, "disabled", "click"], ["type", "button", 1, "btn", "btn-primary", "btn-sm", "mt-2", 3, "disabled", "click"], ["id", "terminalContainer2", 1, "col-sm-4", "mb25"], ["placeholder", "Approved Terminals", "formControlName", "TempTerminalName", 3, "data", "searchKeyword", "itemTemplate", "notFoundTemplate", "click", "selected"]],
      template: function PricingSectionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Pricing");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PricingSectionComponent_Template_input_click_9_listener($event) {
            return ctx.toggleSuppressPricing($event.target.checked);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](11, " Suppress Pricing");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PricingSectionComponent_div_12_Template, 7, 3, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](13, PricingSectionComponent_ng_template_13_Template, 43, 28, "ng-template", null, 9, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, PricingSectionComponent_ng_template_15_Template, 39, 11, "ng-template", null, 10, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.locationForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableControl", ctx.f.IsRegularBuyer.value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx.f.IsSupressOrderPricing.value);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_0__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormControlName"], _directives_disable_control_directive__WEBPACK_IMPORTED_MODULE_8__["DisableControlDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["NumberValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["??angular_packages_forms_forms_x"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__["AutocompleteComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_0__["??angular_packages_forms_forms_y"]],
      styles: [".ng-autocomplete[_ngcontent-%COMP%]{\r\n    width:100% !important;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2hhcmVkLWNvbXBvbmVudHMvcHJpY2luZy1zZWN0aW9uL3ByaWNpbmctc2VjdGlvbi5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0kscUJBQXFCO0FBQ3pCIiwiZmlsZSI6InNyYy9hcHAvc2hhcmVkLWNvbXBvbmVudHMvcHJpY2luZy1zZWN0aW9uL3ByaWNpbmctc2VjdGlvbi5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLm5nLWF1dG9jb21wbGV0ZXtcclxuICAgIHdpZHRoOjEwMCUgIWltcG9ydGFudDtcclxufVxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](PricingSectionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-pricing-section',
          templateUrl: './pricing-section.component.html',
          styleUrls: ['./pricing-section.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormBuilder"]
        }, {
          type: _carrier_schedule_builder_add_location_add_location_service__WEBPACK_IMPORTED_MODULE_6__["AddLocationService"]
        }, {
          type: _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_7__["NgbModal"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]
        }];
      }, {
        locationForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        formSubmited: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        UoM: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsTBD: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        tbdDrProductTypeId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        tbdDrProductId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        productDetails: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        DetectFormChange: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }],
        UpdateSuppressPricingChange: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/shared-components/pricing-section/pricing-section.module.ts": function srcAppSharedComponentsPricingSectionPricingSectionModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PricingSectionModule", function () {
      return PricingSectionModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var agm_direction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! agm-direction */
    "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js");
    /* harmony import */


    var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _pricing_section_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./pricing-section.component */
    "./src/app/shared-components/pricing-section/pricing-section.component.ts");
    /* harmony import */


    var angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! angular-ng-autocomplete */
    "./node_modules/angular-ng-autocomplete/__ivy_ngcc__/fesm2015/angular-ng-autocomplete.js");
    /* harmony import */


    var src_app_modules_global_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/modules/global.module */
    "./src/app/modules/global.module.ts");

    var PricingSectionModule = function PricingSectionModule() {
      _classCallCheck(this, PricingSectionModule);
    };

    PricingSectionModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({
      type: PricingSectionModule
    });
    PricingSectionModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({
      factory: function PricingSectionModule_Factory(t) {
        return new (t || PricingSectionModule)();
      },
      imports: [[src_app_modules_global_module__WEBPACK_IMPORTED_MODULE_6__["GlobalModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_5__["AutocompleteLibModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_2__["DirectiveModule"], agm_direction__WEBPACK_IMPORTED_MODULE_1__["AgmDirectionModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](PricingSectionModule, {
        declarations: [_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"]],
        imports: [src_app_modules_global_module__WEBPACK_IMPORTED_MODULE_6__["GlobalModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_5__["AutocompleteLibModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_2__["DirectiveModule"], agm_direction__WEBPACK_IMPORTED_MODULE_1__["AgmDirectionModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"]],
        exports: [_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](PricingSectionModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"]],
          imports: [src_app_modules_global_module__WEBPACK_IMPORTED_MODULE_6__["GlobalModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_5__["AutocompleteLibModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_2__["DirectiveModule"], agm_direction__WEBPACK_IMPORTED_MODULE_1__["AgmDirectionModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormsModule"]],
          exports: [_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"]]
        }]
      }], null, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=default~carrier-carrier-module~sales-user-sales-user-module-es5.js.map