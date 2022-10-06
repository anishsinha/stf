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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module"], {
  /***/
  "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts": function srcAppBuyerWallyBoardModelsBuyerWallyBoardTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LoadFilterModel", function () {
      return LoadFilterModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DipTestViewModel", function () {
      return DipTestViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankCapacityForDR", function () {
      return TankCapacityForDR;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ModifiedTripInfo", function () {
      return ModifiedTripInfo;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateDeliveryRequestViewModel", function () {
      return CreateDeliveryRequestViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CustomerJobsForCarrier", function () {
      return CustomerJobsForCarrier;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PartialDRDetails", function () {
      return PartialDRDetails;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TfxModule", function () {
      return TfxModule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WindowModeFilter", function () {
      return WindowModeFilter;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "UoM", function () {
      return UoM;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DeliveryRequestViewModel", function () {
      return DeliveryRequestViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionDetailModel", function () {
      return RegionDetailModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ShiftModel", function () {
      return ShiftModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ScheduleBuilderModel", function () {
      return ScheduleBuilderModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DSBSaveModel", function () {
      return DSBSaveModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DRDragDropModel", function () {
      return DRDragDropModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SbDriverViewModel", function () {
      return SbDriverViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SbTrailerViewModel", function () {
      return SbTrailerViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerViewModel", function () {
      return TrailerViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerShiftModel", function () {
      return TrailerShiftModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ScheduleShiftModel", function () {
      return ScheduleShiftModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ShiftDetailModel", function () {
      return ShiftDetailModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerModel", function () {
      return TrailerModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationFilters", function () {
      return LocationFilters;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TripModel", function () {
      return TripModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DropAddressModel", function () {
      return DropAddressModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "OrderPickupDetailModel", function () {
      return OrderPickupDetailModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "OrderPickupLocationModel", function () {
      return OrderPickupLocationModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverModel", function () {
      return WhereIsMyDriverModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DistatcherRegionModel", function () {
      return DistatcherRegionModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Filter", function () {
      return Filter;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SbFilterModel", function () {
      return SbFilterModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerViewFilterModel", function () {
      return TrailerViewFilterModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverViewFilterModel", function () {
      return DriverViewFilterModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CompanyUsers", function () {
      return CompanyUsers;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankMinMaxFill", function () {
      return TankMinMaxFill;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankChartHeight", function () {
      return TankChartHeight;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DipTest", function () {
      return DipTest;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverAdditionalDetails", function () {
      return DriverAdditionalDetails;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "routesColor", function () {
      return routesColor;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DemandModel", function () {
      return DemandModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PartialDRDetail", function () {
      return PartialDRDetail;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LoadInfo", function () {
      return LoadInfo;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PreLoadDrViewModel", function () {
      return PreLoadDrViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PreLoadDrResponseViewModel", function () {
      return PreLoadDrResponseViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PreLoadDrModel", function () {
      return PreLoadDrModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesTankFilterModal", function () {
      return SalesTankFilterModal;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesFilterModal", function () {
      return SalesFilterModal;
    });
    /* harmony import */


    var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! src/app/statelist.service */
    "./src/app/statelist.service.ts");

    var LoadFilterModel = function LoadFilterModel() {
      _classCallCheck(this, LoadFilterModel);
    };

    var DipTestViewModel = function DipTestViewModel() {
      _classCallCheck(this, DipTestViewModel);

      this.ExistingDR = [];
    };

    var TankCapacityForDR = function TankCapacityForDR() {
      _classCallCheck(this, TankCapacityForDR);
    };

    var ModifiedTripInfo = function ModifiedTripInfo() {
      _classCallCheck(this, ModifiedTripInfo);
    };

    var CreateDeliveryRequestViewModel = function CreateDeliveryRequestViewModel() {
      _classCallCheck(this, CreateDeliveryRequestViewModel);
    };

    var CustomerJobsForCarrier = function CustomerJobsForCarrier() {
      _classCallCheck(this, CustomerJobsForCarrier);
    };

    var PartialDRDetails = function PartialDRDetails() {
      _classCallCheck(this, PartialDRDetails);
    };

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

    var DeliveryRequestViewModel = function DeliveryRequestViewModel() {
      _classCallCheck(this, DeliveryRequestViewModel);

      this.Terminal = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
      this.BulkPlant = new DropAddressModel();
      this.PickupLocationType = 1;
      this.WindowMode = 1;
      this.QueueMode = 1;
    };

    var RegionDetailModel = function RegionDetailModel() {
      _classCallCheck(this, RegionDetailModel);

      this.Drivers = [];
      this.Trailers = [];
      this.Shifts = [];
    };

    var ShiftModel = function ShiftModel() {
      _classCallCheck(this, ShiftModel);
    };

    var ScheduleBuilderModel = function ScheduleBuilderModel() {
      _classCallCheck(this, ScheduleBuilderModel);

      this.Shifts = [];
      this.Trailers = [];
    };

    var DSBSaveModel = /*#__PURE__*/function (_ScheduleBuilderModel) {
      _inherits(DSBSaveModel, _ScheduleBuilderModel);

      var _super = _createSuper(DSBSaveModel);

      function DSBSaveModel() {
        var _this;

        _classCallCheck(this, DSBSaveModel);

        _this = _super.call(this);
        _this.PreloadedDRs = [];
        _this.PostloadedDRs = [];
        _this.Trips = [];
        return _this;
      }

      return DSBSaveModel;
    }(ScheduleBuilderModel);

    var DRDragDropModel = /*#__PURE__*/function (_ScheduleBuilderModel2) {
      _inherits(DRDragDropModel, _ScheduleBuilderModel2);

      var _super2 = _createSuper(DRDragDropModel);

      function DRDragDropModel() {
        _classCallCheck(this, DRDragDropModel);

        return _super2.apply(this, arguments);
      }

      return DRDragDropModel;
    }(ScheduleBuilderModel);

    var SbDriverViewModel = /*#__PURE__*/function (_ScheduleBuilderModel3) {
      _inherits(SbDriverViewModel, _ScheduleBuilderModel3);

      var _super3 = _createSuper(SbDriverViewModel);

      function SbDriverViewModel() {
        var _this2;

        _classCallCheck(this, SbDriverViewModel);

        _this2 = _super3.call(this);
        _this2.Shifts = [];
        return _this2;
      }

      return SbDriverViewModel;
    }(ScheduleBuilderModel);

    var SbTrailerViewModel = /*#__PURE__*/function (_ScheduleBuilderModel4) {
      _inherits(SbTrailerViewModel, _ScheduleBuilderModel4);

      var _super4 = _createSuper(SbTrailerViewModel);

      function SbTrailerViewModel() {
        var _this3;

        _classCallCheck(this, SbTrailerViewModel);

        _this3 = _super4.call(this);
        _this3.Trailers = [];
        return _this3;
      }

      return SbTrailerViewModel;
    }(ScheduleBuilderModel);

    var TrailerViewModel = function TrailerViewModel() {
      _classCallCheck(this, TrailerViewModel);
    };

    var TrailerShiftModel = function TrailerShiftModel() {
      _classCallCheck(this, TrailerShiftModel);
    };

    var ScheduleShiftModel = function ScheduleShiftModel() {
      _classCallCheck(this, ScheduleShiftModel);

      this.Schedules = [];
      this.IsCollapsed = false;
    };

    var ShiftDetailModel = function ShiftDetailModel() {
      _classCallCheck(this, ShiftDetailModel);
    };

    var TrailerModel = function TrailerModel() {
      _classCallCheck(this, TrailerModel);

      this.Drivers = [];
      this.Trailers = [];
      this.Trips = [];
    };

    var LocationFilters = function LocationFilters() {
      _classCallCheck(this, LocationFilters);

      this.state = [];
      this.city = [];
      this.product = [];
      this.priority = [];
      this.customer = [];
      this.supplier = [];
      this.carrier = [];
    };

    var TripModel = function TripModel() {
      _classCallCheck(this, TripModel);

      this.DeliveryRequests = [];
      this.Terminal = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
      this.BulkPlant = new DropAddressModel();
      this.Drivers = [];
      this.Trailers = [];
    };

    var DropAddressModel = function DropAddressModel() {
      _classCallCheck(this, DropAddressModel);

      this.State = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
      this.Country = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
    };

    var OrderPickupDetailModel = function OrderPickupDetailModel() {
      _classCallCheck(this, OrderPickupDetailModel);

      this.PickupLocationType = 1;
    };

    var OrderPickupLocationModel = /*#__PURE__*/function () {
      function OrderPickupLocationModel() {
        _classCallCheck(this, OrderPickupLocationModel);
      }

      _createClass(OrderPickupLocationModel, null, [{
        key: "ToLocation",
        value: function ToLocation(orderPickupDetail) {
          var location = new OrderPickupLocationModel();
          location.PickupLocationType = orderPickupDetail.PickupLocationType;
          location.Terminal = {
            Id: orderPickupDetail.TerminalId,
            Name: orderPickupDetail.TerminalName,
            Code: ''
          };
          location.BulkPlant = {
            Address: orderPickupDetail.Address,
            City: orderPickupDetail.City,
            State: {
              Id: orderPickupDetail.StateId,
              Code: orderPickupDetail.StateCode,
              Name: null
            },
            Country: {
              Id: 0,
              Code: orderPickupDetail.CountryCode,
              Name: null
            },
            ZipCode: orderPickupDetail.ZipCode,
            CountyName: orderPickupDetail.CountyName,
            Latitude: orderPickupDetail.Latitude,
            Longitude: orderPickupDetail.Longitude,
            SiteName: orderPickupDetail.BulkplantName,
            SiteId: null
          };
          return location;
        }
      }]);

      return OrderPickupLocationModel;
    }();

    var WhereIsMyDriverModel = function WhereIsMyDriverModel() {
      _classCallCheck(this, WhereIsMyDriverModel);

      this.routeShow = false;
    };

    var DistatcherRegionModel = function DistatcherRegionModel() {
      _classCallCheck(this, DistatcherRegionModel);
    };

    var Filter = function Filter() {
      _classCallCheck(this, Filter);

      this['state'] = [];
      this['city'] = [];
      this['product'] = [];
      this['priority'] = [];
      this['customer'] = [];
      this['supplier'] = [];
      this['carrier'] = [];
    };

    var SbFilterModel = function SbFilterModel() {
      _classCallCheck(this, SbFilterModel);

      this.Drivers = [];
      this.Trailers = [];
      this.Pickups = [];
      this.SelectedDrivers = [];
      this.SelectedPickups = [];
      this.SelectedTrailers = [];
    };

    var TrailerViewFilterModel = function TrailerViewFilterModel() {
      _classCallCheck(this, TrailerViewFilterModel);

      this.Shifts = {};
      this.Trailers = {};
      this.Pickups = {};
      this.Drivers = {};
    };

    var DriverViewFilterModel = function DriverViewFilterModel() {
      _classCallCheck(this, DriverViewFilterModel);

      this.Shifts = {};
    };

    var CompanyUsers = function CompanyUsers() {
      _classCallCheck(this, CompanyUsers);
    };

    var TankMinMaxFill = function TankMinMaxFill() {
      _classCallCheck(this, TankMinMaxFill);
    };

    var TankChartHeight = function TankChartHeight() {
      _classCallCheck(this, TankChartHeight);
    };

    var DipTest = function DipTest() {
      _classCallCheck(this, DipTest);
    };

    var DriverAdditionalDetails = function DriverAdditionalDetails(data) {
      _classCallCheck(this, DriverAdditionalDetails);

      this.Id = data && data['Id'] || null;
      this.Name = data && data['Name'] || null;
      this.License = data && data['License'] || null;
      this.ContactNumnber = data && data['ContactNumnber'] || null;
      this.Shifts = data && data['Shifts'] || [];
      this.Trailers = data && data['Trailers'] || [];
    };

    var routesColor = {
      1: '#5f4aa8',
      11: '#c4c105',
      12: '#d3950f',
      18: '#19953f',
      20: '#e3584d'
    };

    var DemandModel = function DemandModel() {
      _classCallCheck(this, DemandModel);
    };

    var PartialDRDetail = function PartialDRDetail() {
      _classCallCheck(this, PartialDRDetail);
    };

    var LoadInfo = function LoadInfo() {
      _classCallCheck(this, LoadInfo);
    };

    var PreLoadDrViewModel = function PreLoadDrViewModel() {
      _classCallCheck(this, PreLoadDrViewModel);
    };

    var PreLoadDrResponseViewModel = function PreLoadDrResponseViewModel() {
      _classCallCheck(this, PreLoadDrResponseViewModel);
    };

    var PreLoadDrModel = function PreLoadDrModel() {
      _classCallCheck(this, PreLoadDrModel);
    };

    var SalesTankFilterModal = function SalesTankFilterModal() {
      _classCallCheck(this, SalesTankFilterModal);

      this.selectedLocAttributeData = [];
    };

    var SalesFilterModal = function SalesFilterModal() {
      _classCallCheck(this, SalesFilterModal);
    };
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts": function srcAppBuyerWallyBoardServicesBuyerwallyboardServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "BuyerwallyboardService", function () {
      return BuyerwallyboardService;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../../../app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var BuyerwallyboardService = /*#__PURE__*/function (_app_errors_HandleErr) {
      _inherits(BuyerwallyboardService, _app_errors_HandleErr);

      var _super5 = _createSuper(BuyerwallyboardService);

      function BuyerwallyboardService(httpClient) {
        var _this4;

        _classCallCheck(this, BuyerwallyboardService);

        _this4 = _super5.call(this);
        _this4.httpClient = httpClient;
        _this4.getJobLocationDetailsUrl = '/Buyer/Job/GetJobLocationDetails'; // Move this to Job Controller buyer area

        _this4.getSupplierCarrierDDLUrl = '/Buyer/Job/GetSupplierCarrierForjobs';
        _this4.getDipTestDetailsUrl = '/Buyer/Dashboard/GetDipTestDetails?';
        _this4.getDeliveriesForLocations = '/Buyer/WallyBoard/GetDeliveriesForLocations?';
        _this4.getFilterData = 'Buyer/WallyBoard/GetBuyerLoadFilterData';
        _this4.getDispatcherCountryUrl = '/Buyer/WallyBoard/GetUserCountry';
        _this4.getOnGoingLoadsUrl = '/Buyer/WallyBoard/GetOnGoingLoadsForMapView';
        _this4.getBuyerLoadsForGridUrl = '/Buyer/WallyBoard/GetBuyerLoads';
        _this4.getDriverAdditionalDetailsUrl = '/Buyer/WallyBoard/GetDriverAdditionalDetails?driverId=';
        _this4.getFiltersUrl = '/Buyer/WallyBoard/GetFilters?moduleId=';
        _this4.saveFiltersUrl = '/Buyer/WallyBoard/SaveFilters';
        _this4.SingleMultiWindowSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1); //singlemulti window screen 1

        return _this4;
      }

      _createClass(BuyerwallyboardService, [{
        key: "getBuyerLoadsForGrid",
        value: function getBuyerLoadsForGrid(inputs) {
          return this.httpClient.post(this.getBuyerLoadsForGridUrl, inputs).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getBuyerLoadsForGrid', null)));
        }
      }, {
        key: "getFilters",
        value: function getFilters(moduleId) {
          return this.httpClient.get(this.getFiltersUrl + moduleId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFilters', null)));
        }
      }, {
        key: "saveFilters",
        value: function saveFilters(moduleId, input) {
          var data = {
            moduleId: moduleId,
            filterInput: input
          };
          return this.httpClient.post(this.saveFiltersUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('saveFilters', null)));
        }
      }, {
        key: "getDriverAdditionalDetails",
        value: function getDriverAdditionalDetails(driverId) {
          return this.httpClient.get(this.getDriverAdditionalDetailsUrl + driverId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDriverAdditionalDetails', null)));
        }
      }, {
        key: "getJobLocationDetails",
        value: function getJobLocationDetails(jobIds, selectedLocAttributeId) {
          var _this5 = this;

          var data = {
            jobList: jobIds,
            inventoryCaptureTypeIds: selectedLocAttributeId
          };
          return Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["timer"])(0, 60 * 60 * 1000).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["switchMap"])(function () {
            return _this5.httpClient.post(_this5.getJobLocationDetailsUrl, data);
          })).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getJobLocationDetails', null)));
        }
      }, {
        key: "getSuppliersCarrierssDDL",
        value: function getSuppliersCarrierssDDL(jobIds) {
          var _this6 = this;

          return Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["timer"])(0, 60 * 60 * 1000).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["switchMap"])(function () {
            return _this6.httpClient.post(_this6.getSupplierCarrierDDLUrl, {
              jobIds: jobIds
            });
          })).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getSuppliersCarrierssDDL', null)));
        }
      }, {
        key: "getDipTestDetails",
        value: function getDipTestDetails(siteId, tankId, noOfDays) {
          return this.httpClient.get(this.getDipTestDetailsUrl + 'siteId=' + siteId + '&' + 'tankId=' + tankId + '&' + 'noOfDays=' + noOfDays).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDipTestDetails', null)));
        }
      }, {
        key: "GetDeliveriesForLocations",
        value: function GetDeliveriesForLocations(fromDate, toDate) {
          return this.httpClient.get(this.getDeliveriesForLocations + 'fromDate=' + fromDate + '&' + 'toDate=' + toDate).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetDeliveriesForLocations', null)));
        }
      }, {
        key: "GetFilterData",
        value: function GetFilterData(isShowCarrierManaged) {
          return this.httpClient.get(this.getFilterData + "?isShowCarrierManaged=" + isShowCarrierManaged).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetFilterData', null)));
        }
      }, {
        key: "getDispatcherCountry",
        value: function getDispatcherCountry() {
          return this.httpClient.get(this.getDispatcherCountryUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getDispatcherCountry', null)));
        }
      }, {
        key: "getOnGoingLoadsForMap",
        value: function getOnGoingLoadsForMap(inputs) {
          return this.httpClient.post(this.getOnGoingLoadsUrl, inputs).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getOnGoingLoads', null)));
        }
      }]);

      return BuyerwallyboardService;
    }(_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_3__["HandleError"]);

    BuyerwallyboardService.ɵfac = function BuyerwallyboardService_Factory(t) {
      return new (t || BuyerwallyboardService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"]));
    };

    BuyerwallyboardService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: BuyerwallyboardService,
      factory: BuyerwallyboardService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](BuyerwallyboardService, [{
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

  }
}]);
//# sourceMappingURL=default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module-es5.js.map