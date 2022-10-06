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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["dispatcher-dispatcher-module"], {
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
        var _this2;

        _classCallCheck(this, DSBSaveModel);

        _this2 = _super.call(this);
        _this2.PreloadedDRs = [];
        _this2.PostloadedDRs = [];
        _this2.Trips = [];
        return _this2;
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
        var _this3;

        _classCallCheck(this, SbDriverViewModel);

        _this3 = _super3.call(this);
        _this3.Shifts = [];
        return _this3;
      }

      return SbDriverViewModel;
    }(ScheduleBuilderModel);

    var SbTrailerViewModel = /*#__PURE__*/function (_ScheduleBuilderModel4) {
      _inherits(SbTrailerViewModel, _ScheduleBuilderModel4);

      var _super4 = _createSuper(SbTrailerViewModel);

      function SbTrailerViewModel() {
        var _this4;

        _classCallCheck(this, SbTrailerViewModel);

        _this4 = _super4.call(this);
        _this4.Trailers = [];
        return _this4;
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
  "./src/app/dispatcher/dispatcher-dashboard/dispatcher-dashboard.component.ts": function srcAppDispatcherDispatcherDashboardDispatcherDashboardComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DispatcherDashboardComponent", function () {
      return DispatcherDashboardComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/directives/sorting.pipe */
    "./src/app/directives/sorting.pipe.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _location_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./location.component */
    "./src/app/dispatcher/dispatcher-dashboard/location.component.ts");
    /* harmony import */


    var _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./where-is-my-driver.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver.component.ts");
    /* harmony import */


    var _sales_data_sales_data_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./sales-data/sales-data.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/sales-data.component.ts");

    function DispatcherDashboardComponent_div_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "input", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "label", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatcherDashboardComponent_div_15_Template_label_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r4.changeWindowType(1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatcherDashboardComponent_div_15_Template_label_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r6.changeWindowType(2);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "i", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1)("checked", ctx_r0.singleMulti == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2)("checked", ctx_r0.singleMulti == 2);
      }
    }

    function DispatcherDashboardComponent_app_location_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-location", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("singleMulti", ctx_r1.singleMulti);
      }
    }

    function DispatcherDashboardComponent_app_where_is_my_driver_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-where-is-my-driver", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("singleMulti", ctx_r2.singleMulti);
      }
    }

    function DispatcherDashboardComponent_app_sales_data_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-sales-data");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var DispatcherDashboardComponent = /*#__PURE__*/function () {
      function DispatcherDashboardComponent(fb, dispatcherService, carrierService, customSortingService) {
        _classCallCheck(this, DispatcherDashboardComponent);

        this.fb = fb;
        this.dispatcherService = dispatcherService;
        this.carrierService = carrierService;
        this.customSortingService = customSortingService;
        this.disableControl = false;
      }

      _createClass(DispatcherDashboardComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.checkWindowSelection();
          this.getCountries();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.customSortingService.configColumnDefsNullToBottom();
        }
      }, {
        key: "getCountries",
        value: function getCountries() {
          this.carrierService.GetCountries(currentUserCompanyId).subscribe(function (data) {
            if (data != null) {
              localStorage.setItem('countryIdForDashboard', data.DefaultCountryId);
              localStorage.setItem('currencyTypeForDashboard', data.DefaultCountryId);
            }
          });
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(type) {
          localStorage.setItem('viewType', type);

          if (this.singleMulti === 2) {
            this.dispatcherDashboard = window.open("/Dispatcher/Dashboard", "_blank");
          } else {
            this.viewType = type;
          }
        }
      }, {
        key: "changeWindowType",
        value: function changeWindowType(type) {
          var _this5 = this;

          this.singleMulti = type;
          this.dispatcherService.SingleMultiWindowSubject.next(type);

          if (type === 1 && +localStorage.getItem('singleMulti') !== 1) {
            setTimeout(function () {
              _this5.dispatcherDashboard.close();
            }, 10000);
          }

          localStorage.setItem('singleMulti', this.singleMulti);
        }
      }, {
        key: "checkWindowSelection",
        value: function checkWindowSelection() {
          this.singleMulti = localStorage.getItem('singleMulti') ? +localStorage.getItem('singleMulti') : 1;
          this.viewType = localStorage.getItem('viewType') ? +localStorage.getItem('viewType') : 1;
          var readonlyKey = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_1__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_1__["MyLocalStorage"].DSB_READONLY_KEY);

          if (readonlyKey == '') {
            this.disableControl = false;
          } else {
            this.disableControl = readonlyKey;
          }

          if (this.disableControl == true) {
            this.viewType = 2;
          }
        }
      }]);

      return DispatcherDashboardComponent;
    }();

    DispatcherDashboardComponent.ɵfac = function DispatcherDashboardComponent_Factory(t) {
      return new (t || DispatcherDashboardComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_3__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_5__["DatatableCustomSortingService"]));
    };

    DispatcherDashboardComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: DispatcherDashboardComponent,
      selectors: [["app-dispatcher-dashboard"]],
      decls: 21,
      vars: 13,
      consts: [[1, "row"], [1, "col-sm-12"], [1, "col-sm-3", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "float-left", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], ["class", "dib switch-window ml20 pull-left mt5", 4, "ngIf"], [3, "singleMulti", 4, "ngIf"], [4, "ngIf"], [1, "dib", "switch-window", "ml20", "pull-left", "mt5"], [1, "btn-group"], ["name", "single", "type", "radio", 1, "hide-element", 3, "value", "checked"], ["placement", "bottom", "ngbTooltip", "Single Window", 1, "btn", "ml0", "first-icon", 3, "click"], [1, "far", "fa-window-maximize", "fs14", "mt3"], ["name", "multiple", "type", "radio", 1, "hide-element", 3, "value", "checked"], ["placement", "bottom", "ngbTooltip", "Multi Window", 1, "btn", "last-icon", 3, "click"], [1, "far", "fa-window-restore", "fs14", "mt3"], [3, "singleMulti"]],
      template: function DispatcherDashboardComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatcherDashboardComponent_Template_label_click_7_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatcherDashboardComponent_Template_label_click_10_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Loads");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatcherDashboardComponent_Template_label_click_13_listener() {
            return ctx.changeViewType(3);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Sales");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, DispatcherDashboardComponent_div_15_Template, 8, 4, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, DispatcherDashboardComponent_app_location_18_Template, 2, 1, "app-location", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, DispatcherDashboardComponent_app_where_is_my_driver_19_Template, 2, 1, "app-where-is-my-driver", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, DispatcherDashboardComponent_app_sales_data_20_Template, 2, 0, "app-sales-data", 10);

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

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 3)("checked", ctx.viewType == 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 3);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_7__["NgbTooltip"], _location_component__WEBPACK_IMPORTED_MODULE_8__["LocationComponent"], _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverComponent"], _sales_data_sales_data_component__WEBPACK_IMPORTED_MODULE_10__["SalesDataComponent"]],
      styles: ["agm-map {\r\n    height: 60vh;\r\n}\r\n\r\n  .gmap_location {\r\n    position: relative;\r\n}\r\n\r\n  .filters_panel {\r\n    position: absolute;\r\n    z-index: 1;\r\n    top: 5px;\r\n    left: 0;\r\n    width: 100%;\r\n    padding: 10px 60px;\r\n}\r\n\r\n  .filters_panel .filter_div {\r\n    background: #ffffff;\r\n    color: #000000;\r\n    border-radius: 5px;\r\n    padding: 10px;\r\n}\r\n\r\n  .filters_panel .filter_div label {\r\n    margin-bottom: 0;\r\n}\r\n\r\n  .table .bg_must_go {\r\n    background-color: #f5d0d0;\r\n}\r\n\r\n  .table .bg_should_go {\r\n    background-color: #f7e6a9;\r\n}\r\n\r\n  .table .bg_could_go {\r\n    background-color: #e4e2e2;\r\n}\r\n\r\n  .filter-in-map {\r\n    background: lightgrey;\r\n    position: absolute;\r\n    width: 80%;\r\n    top: 20px;\r\n    left: 20px;\r\n    border-radius: 5px;\r\n}\r\n\r\n  .driver-list {\r\n    max-height: 335px;\r\n    overflow: auto;\r\n    margin-top: 10px;\r\n    padding:0 8px;\r\n}\r\n\r\n  .driver-initials {\r\n    width: 36px;\r\n    height: 36px;\r\n    text-align: center;\r\n    display: flex;\r\n    align-items: center;\r\n    justify-content: center;\r\n}\r\n\r\n  .driver-details:hover {\r\n    background: #f7f7f7;\r\n    cursor: pointer;\r\n}\r\n\r\n.sticky-header-dash[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    padding: 10px 10px;\r\n    top: 45px;\r\n    left:0;\r\n    height: 65px;\r\n    font-size: 20px;\r\n    z-index: 11;\r\n    background: #fff !important;\r\n    left:0;\r\n    float:left;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   input[type=\"radio\"][_ngcontent-%COMP%]:checked    + label[_ngcontent-%COMP%] {\r\n    background-image: none;\r\n    background-color: white;\r\n    color: #1062d1;\r\n    border: 1px solid #1062d1;\r\n    border-radius: 5px 0 0 5px !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   label.last-icon[_ngcontent-%COMP%], .switch-window[_ngcontent-%COMP%]   input[type=\"radio\"][_ngcontent-%COMP%]:checked    + label.last-icon[_ngcontent-%COMP%] {\r\n    border-radius: 0 5px 5px 0 !important;\r\n    margin-left: 0 !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   label.first-icon[_ngcontent-%COMP%] {\r\n    border-radius: 5px 0 0 5px !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\r\n    padding: 5px 10px !important;\r\n    background-image: none;\r\n    background-color: white;\r\n    color: #D1D1D1;\r\n    border: 1px solid #D1D1D1;\r\n    border-radius: 5px 0 0 5px;\r\n    cursor: pointer;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9kaXNwYXRjaGVyLWRhc2hib2FyZC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtBQUNoQjs7QUFFQTtJQUNJLGtCQUFrQjtBQUN0Qjs7QUFFQTtJQUNJLGtCQUFrQjtJQUNsQixVQUFVO0lBQ1YsUUFBUTtJQUNSLE9BQU87SUFDUCxXQUFXO0lBQ1gsa0JBQWtCO0FBQ3RCOztBQUVBO0lBQ0ksbUJBQW1CO0lBQ25CLGNBQWM7SUFDZCxrQkFBa0I7SUFDbEIsYUFBYTtBQUNqQjs7QUFFQTtJQUNJLGdCQUFnQjtBQUNwQjs7QUFFQTtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHFCQUFxQjtJQUNyQixrQkFBa0I7SUFDbEIsVUFBVTtJQUNWLFNBQVM7SUFDVCxVQUFVO0lBQ1Ysa0JBQWtCO0FBQ3RCOztBQUVBO0lBQ0ksaUJBQWlCO0lBQ2pCLGNBQWM7SUFDZCxnQkFBZ0I7SUFDaEIsYUFBYTtBQUNqQjs7QUFFQTtJQUNJLFdBQVc7SUFDWCxZQUFZO0lBQ1osa0JBQWtCO0lBQ2xCLGFBQWE7SUFDYixtQkFBbUI7SUFDbkIsdUJBQXVCO0FBQzNCOztBQUVBO0lBQ0ksbUJBQW1CO0lBQ25CLGVBQWU7QUFDbkI7O0FBRUE7SUFDSSxlQUFlO0lBQ2Ysa0JBQWtCO0lBQ2xCLFNBQVM7SUFDVCxNQUFNO0lBQ04sWUFBWTtJQUNaLGVBQWU7SUFDZixXQUFXO0lBQ1gsMkJBQTJCO0lBQzNCLE1BQU07SUFDTixVQUFVO0FBQ2Q7O0FBRUE7SUFDSSxzQkFBc0I7SUFDdEIsdUJBQXVCO0lBQ3ZCLGNBQWM7SUFDZCx5QkFBeUI7SUFDekIscUNBQXFDO0FBQ3pDOztBQUVDOztJQUVHLHFDQUFxQztJQUNyQyx5QkFBeUI7QUFDN0I7O0FBRUE7SUFDSSxxQ0FBcUM7QUFDekM7O0FBR0E7SUFDSSw0QkFBNEI7SUFDNUIsc0JBQXNCO0lBQ3RCLHVCQUF1QjtJQUN2QixjQUFjO0lBQ2QseUJBQXlCO0lBQ3pCLDBCQUEwQjtJQUMxQixlQUFlO0FBQ25CIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9kaXNwYXRjaGVyLWRhc2hib2FyZC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIGFnbS1tYXAge1xyXG4gICAgaGVpZ2h0OiA2MHZoO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmdtYXBfbG9jYXRpb24ge1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmZpbHRlcnNfcGFuZWwge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgei1pbmRleDogMTtcclxuICAgIHRvcDogNXB4O1xyXG4gICAgbGVmdDogMDtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcGFkZGluZzogMTBweCA2MHB4O1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmZpbHRlcnNfcGFuZWwgLmZpbHRlcl9kaXYge1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIGNvbG9yOiAjMDAwMDAwO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgcGFkZGluZzogMTBweDtcclxufVxyXG5cclxuOjpuZy1kZWVwIC5maWx0ZXJzX3BhbmVsIC5maWx0ZXJfZGl2IGxhYmVsIHtcclxuICAgIG1hcmdpbi1ib3R0b206IDA7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAudGFibGUgLmJnX211c3RfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2Y1ZDBkMDtcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJsZSAuYmdfc2hvdWxkX2dvIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmN2U2YTk7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAudGFibGUgLmJnX2NvdWxkX2dvIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNlNGUyZTI7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAuZmlsdGVyLWluLW1hcCB7XHJcbiAgICBiYWNrZ3JvdW5kOiBsaWdodGdyZXk7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB3aWR0aDogODAlO1xyXG4gICAgdG9wOiAyMHB4O1xyXG4gICAgbGVmdDogMjBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxufVxyXG5cclxuOjpuZy1kZWVwIC5kcml2ZXItbGlzdCB7XHJcbiAgICBtYXgtaGVpZ2h0OiAzMzVweDtcclxuICAgIG92ZXJmbG93OiBhdXRvO1xyXG4gICAgbWFyZ2luLXRvcDogMTBweDtcclxuICAgIHBhZGRpbmc6MCA4cHg7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIHdpZHRoOiAzNnB4O1xyXG4gICAgaGVpZ2h0OiAzNnB4O1xyXG4gICAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG4gICAgZGlzcGxheTogZmxleDtcclxuICAgIGFsaWduLWl0ZW1zOiBjZW50ZXI7XHJcbiAgICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcclxufVxyXG5cclxuOjpuZy1kZWVwIC5kcml2ZXItZGV0YWlsczpob3ZlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZjdmN2Y3O1xyXG4gICAgY3Vyc29yOiBwb2ludGVyO1xyXG59XHJcblxyXG4uc3RpY2t5LWhlYWRlci1kYXNoIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHBhZGRpbmc6IDEwcHggMTBweDtcclxuICAgIHRvcDogNDVweDtcclxuICAgIGxlZnQ6MDtcclxuICAgIGhlaWdodDogNjVweDtcclxuICAgIGZvbnQtc2l6ZTogMjBweDtcclxuICAgIHotaW5kZXg6IDExO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZiAhaW1wb3J0YW50O1xyXG4gICAgbGVmdDowO1xyXG4gICAgZmxvYXQ6bGVmdDtcclxufVxyXG5cclxuLnN3aXRjaC13aW5kb3cgaW5wdXRbdHlwZT1cInJhZGlvXCJdOmNoZWNrZWQgKyBsYWJlbCB7XHJcbiAgICBiYWNrZ3JvdW5kLWltYWdlOiBub25lO1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogd2hpdGU7XHJcbiAgICBjb2xvcjogIzEwNjJkMTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICMxMDYyZDE7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHggMCAwIDVweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4gLnN3aXRjaC13aW5kb3cgbGFiZWwubGFzdC1pY29uLFxyXG4gLnN3aXRjaC13aW5kb3cgaW5wdXRbdHlwZT1cInJhZGlvXCJdOmNoZWNrZWQgKyBsYWJlbC5sYXN0LWljb24ge1xyXG4gICAgYm9yZGVyLXJhZGl1czogMCA1cHggNXB4IDAgIWltcG9ydGFudDtcclxuICAgIG1hcmdpbi1sZWZ0OiAwICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5zd2l0Y2gtd2luZG93IGxhYmVsLmZpcnN0LWljb24ge1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4IDAgMCA1cHggIWltcG9ydGFudDtcclxufVxyXG5cclxuXHJcbi5zd2l0Y2gtd2luZG93IC5idG4ge1xyXG4gICAgcGFkZGluZzogNXB4IDEwcHggIWltcG9ydGFudDtcclxuICAgIGJhY2tncm91bmQtaW1hZ2U6IG5vbmU7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiB3aGl0ZTtcclxuICAgIGNvbG9yOiAjRDFEMUQxO1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgI0QxRDFEMTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweCAwIDAgNXB4O1xyXG4gICAgY3Vyc29yOiBwb2ludGVyO1xyXG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DispatcherDashboardComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-dispatcher-dashboard',
          templateUrl: './dispatcher-dashboard.component.html',
          styleUrls: ['./dispatcher-dashboard.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }, {
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_3__["DispatcherService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]
        }, {
          type: src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_5__["DatatableCustomSortingService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/job-tank-hierarchy/job-tank-hierarchy.component.ts": function srcAppDispatcherDispatcherDashboardJobTankHierarchyJobTankHierarchyComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "JobTankHierarchyComponent", function () {
      return JobTankHierarchyComponent;
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


    var src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../../directives/sorting.pipe */
    "./src/app/directives/sorting.pipe.ts");

    function JobTankHierarchyComponent_div_6_Template(rf, ctx) {
      if (rf & 1) {
        var _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "input", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function JobTankHierarchyComponent_div_6_Template_input_input_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4);

          var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r3.Partsfiltering($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function JobTankHierarchyComponent_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " No Location Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function JobTankHierarchyComponent_div_8_ng_container_12_tr_25_span_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 36);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "active": a0
      };
    };

    function JobTankHierarchyComponent_div_8_ng_container_12_tr_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "a", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function JobTankHierarchyComponent_div_8_ng_container_12_tr_25_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12);

          var tank_r9 = ctx.$implicit;

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

          return ctx_r11.tankChange(tank_r9);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, JobTankHierarchyComponent_div_8_ng_container_12_tr_25_span_4_Template, 1, 0, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function JobTankHierarchyComponent_div_8_ng_container_12_tr_25_Template_a_click_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12);

          var loc_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r13.showTanks(loc_r6);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var tank_r9 = ctx.$implicit;

        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](5, _c0, ctx_r8.SelectedTankId == (tank_r9 == null ? null : tank_r9.TankId) + "_" + (tank_r9 == null ? null : tank_r9.StorageId)));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", tank_r9.Name, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tank_r9 == null ? null : tank_r9.IsUnknowDeliveryOrMissDelivery);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", tank_r9.DaysRemaining == null ? "NA" : tank_r9.DaysRemaining + " Days", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tank_r9.Status);
      }
    }

    function JobTankHierarchyComponent_div_8_ng_container_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h2", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function JobTankHierarchyComponent_div_8_ng_container_12_Template_div_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var loc_r6 = ctx.$implicit;

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r15.locationChange(loc_r6);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](8, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](9, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "i", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "a", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function JobTankHierarchyComponent_div_8_ng_container_12_Template_a_click_16_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var loc_r6 = ctx.$implicit;

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r17.showTanks(loc_r6);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "tr", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "td", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "ul", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "table", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, JobTankHierarchyComponent_div_8_ng_container_12_tr_25_Template, 11, 7, "tr", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var loc_r6 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("id", "headingOne" + (loc_r6 == null ? null : loc_r6.SiteId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", "", loc_r6.LocationName, "", loc_r6 && loc_r6.CustomerInfo ? " - " + loc_r6.CustomerInfo : null, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("data-target", "#col" + (loc_r6 == null ? null : loc_r6.SiteId))("aria-controls", "col" + (loc_r6 == null ? null : loc_r6.SiteId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" ", loc_r6 == null ? null : loc_r6.LocationName, " ", loc_r6 && loc_r6.CustomerInfo && loc_r6.CustomerInfo.length > 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](8, 13, loc_r6.CustomerInfo, 0, 5) + "..)" : "", " ", loc_r6 && loc_r6.CustomerInfo && loc_r6.CustomerInfo.length <= 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](9, 17, loc_r6.CustomerInfo, 0, 5) + ")" : "", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](loc_r6.DaysRemaining == null ? "NA" : loc_r6.DaysRemaining + " Days");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](loc_r6.Status);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("id", "col" + (loc_r6 == null ? null : loc_r6.SiteId))("aria-labelledby", "headingOne" + (loc_r6 == null ? null : loc_r6.SiteId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", loc_r6 == null ? null : loc_r6.Tanks);
      }
    }

    function JobTankHierarchyComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "table", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "th", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function JobTankHierarchyComponent_div_8_Template_th_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r18.setSortArgs("DaysRemaining");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " Days remaining\xA0");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, " Status ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, JobTankHierarchyComponent_div_8_ng_container_12_Template, 26, 21, "ng-container", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](13, "sortingPipe");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](13, 1, ctx_r2.FilterLocationDrpDwnList, ctx_r2.filterArgs));
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "hide-element": a0
      };
    };

    var JobTankHierarchyComponent = /*#__PURE__*/function () {
      function JobTankHierarchyComponent(dispatcherService) {
        _classCallCheck(this, JobTankHierarchyComponent);

        this.dispatcherService = dispatcherService;
        this.LocationSchedules = [];
        this.CloneLocationSchedules = [];
        this.LocationDrpDwnList = [];
        this.FilterLocationDrpDwnList = [];
        this.IsLoading = false;
        this.IsLocDrpDwnLoading = false;
        this.filterArgs = {
          key: "DaysRemaining",
          asc: true
        };
      }

      _createClass(JobTankHierarchyComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initLocationDropDown();
        }
      }, {
        key: "setSortArgs",
        value: function setSortArgs(key) {
          if (this.filterArgs.key == key) {
            this.filterArgs = {
              asc: !this.filterArgs.asc,
              key: key
            };
          } else {
            this.filterArgs = {
              asc: true,
              key: key
            };
          }
        }
      }, {
        key: "initLocationDropDown",
        value: function initLocationDropDown() {
          var _this6 = this;

          this.IsLocDrpDwnLoading = true;
          this.LocationDrpDwnList = [];
          var filter = {
            Carriers: "",
            CustomerIds: this.SelectedCustomerId ? this.SelectedCustomerId : "",
            InventoryCaptureType: "",
            IsRateOfConsumption: false,
            IsShowCarrierManaged: false,
            RegionId: this.SelectedRegionId ? this.SelectedRegionId : ""
          };
          Object(rxjs__WEBPACK_IMPORTED_MODULE_1__["forkJoin"])([this.dispatcherService.getSupplierLocationTanks(filter), this.dispatcherService.GetRaisedExceptions()]).subscribe(function (result) {
            _this6.IsLocDrpDwnLoading = false;
            _this6.LocationDrpDwnList = result[0];

            _this6.Partsfiltering();

            _this6.LocationDrpDwnList && _this6.LocationDrpDwnList.length > 0 ? _this6.locationChange(_this6.LocationDrpDwnList[0]) : '';

            if (_this6.LocationDrpDwnList && _this6.LocationDrpDwnList.length > 0) {
              _this6.LocationDrpDwnList.forEach(function (loc) {
                loc && loc.Tanks && loc.Tanks.length > 0 && loc.Tanks.forEach(function (m) {
                  if (result[1] && result[1].filter(function (f) {
                    return f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == loc.SiteId && f.TankDetail.StorageId == m.StorageId;
                  }).length > 0) m.IsUnknowDeliveryOrMissDelivery = true;else m.IsUnknowDeliveryOrMissDelivery = false;
                });
              });
            }
          });
        }
      }, {
        key: "locationChange",
        value: function locationChange($event) {
          this.SelectedTankId = null;
          this.SelectedLocationId = $event.JobId;
          this.SelectedSiteId = $event.SiteId;
          this.LocationSchedules = [];
          this.CloneLocationSchedules = [];
        }
      }, {
        key: "tankChange",
        value: function tankChange($event) {
          if (this.CloneLocationSchedules && this.CloneLocationSchedules.length > 0) {
            this.SelectedTankId = $event.TankId + '_' + $event.StorageId;
            this.LocationSchedules = this.CloneLocationSchedules.filter(function (f) {
              return f.TankId == $event.TankId && f.StorageId == $event.StorageId;
            });
          } else this.LocationSchedules = [];
        }
      }, {
        key: "Partsfiltering",
        value: function Partsfiltering(inputName) {
          var _this7 = this;

          this.FilterLocationDrpDwnList = [];

          if (inputName && inputName.target && inputName.target.value && inputName.target.value.trim() != '') {
            var searchWord = inputName.target.value.toUpperCase();
            this.LocationDrpDwnList.forEach(function (element) {
              if (element.SiteId.toUpperCase().indexOf(searchWord) !== -1) {
                _this7.FilterLocationDrpDwnList.push(element);
              }
            });
          } else {
            this.FilterLocationDrpDwnList = this.LocationDrpDwnList;
          }
        }
      }, {
        key: "showTanks",
        value: function showTanks(location) {
          var row = this.LocationSchedules[0];
          var salesDataModel = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__["SalesDataModel"]();
          salesDataModel.RegionId = row.RegionId;
          salesDataModel.SiteId = location.SiteId;
          salesDataModel.TankId = location.TankId;
          salesDataModel.StorageId = location.StorageId;
          salesDataModel.TfxJobId = parseInt(location.JobId);
          salesDataModel.LocationManagedType = location.LocationManagedType ? location.LocationManagedType : null;
          this.dipTestComponent.loadTankDR(salesDataModel);
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }]);

      return JobTankHierarchyComponent;
    }();

    JobTankHierarchyComponent.ɵfac = function JobTankHierarchyComponent_Factory(t) {
      return new (t || JobTankHierarchyComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_4__["DispatcherService"]));
    };

    JobTankHierarchyComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: JobTankHierarchyComponent,
      selectors: [["app-job-tank-hierarchy"]],
      viewQuery: function JobTankHierarchyComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_2__["DipTestComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dipTestComponent = _t.first);
        }
      },
      decls: 11,
      vars: 11,
      consts: [[1, "row"], [1, "col-sm-12"], [1, "well", "bg-white", "shadow-b", "location-panel"], ["id", "accordion-location", 1, "location-accordion"], [1, "position-abs", "text-center", 3, "ngClass"], [1, "spinner-small", "ml10", "mt5"], ["class", "col-sm-12 row", 4, "ngIf"], [4, "ngIf"], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "RequestFromBuyerWallyBoard", "onRaiseDR"], [1, "col-sm-12", "row"], [1, "inner-addon", "left-addon"], [1, "glyphicon", "glyphicon-search"], ["name", "txtSearch", "placeholder", "Search Location", "type", "text", "autocomplete", "off", 1, "form-control", 3, "input"], [1, "table", "tank-view"], ["width", "49%"], ["width", "24%", 1, "cursor_pointer", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-sort"], [4, "ngFor", "ngForOf"], [1, "card-header"], [1, "mb-0"], ["data-toggle", "collapse", "aria-expanded", "true", 1, "position-relative", "pr-3", "btn", "btn-link", "collapsed", "text-left", 3, "ngbTooltip", "click"], [1, "fa-stack", "fa-sm", "icon-color-b", "position-absolute", 2, "top", "3px", "right", "-7px"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-plus", "fa-stack-1x", "fa-inverse"], ["href", "javascript:void(0)", "onclick", "slidePanel('#raisedr','60%')", 1, "", 3, "click"], [1, ""], ["data-parent", "#accordion-location", 1, "collapse"], ["colspan", "3"], [1, "card-body"], [1, "list-group", "list-group-flush"], [1, "table", "tank-view-child"], ["width", "45%"], ["href", "javascript:void(0)", 3, "ngClass", "click"], ["class", "active-dot", 4, "ngIf"], ["width", "24%"], [1, "active-dot"]],
      template: function JobTankHierarchyComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, JobTankHierarchyComponent_div_6_Template, 4, 0, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, JobTankHierarchyComponent_div_7_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, JobTankHierarchyComponent_div_8_Template, 14, 4, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "app-dip-test", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onRaiseDR", function JobTankHierarchyComponent_Template_app_dip_test_onRaiseDR_10_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](9, _c1, !ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length == 0));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("isDisableControl", true)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedRegionId)("IsThisFromDrDisplay", false)("RequestFromBuyerWallyBoard", true);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_5__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_2__["DipTestComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbTooltip"]],
      pipes: [_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_7__["SortingPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["SlicePipe"]],
      styles: [".location-panel[_ngcontent-%COMP%] {\r\nheight: calc(100vh - 130px);\r\nmax-height: calc(100vh - 120px);\r\noverflow-y: auto;\r\n}\r\n.location-chart-panel[_ngcontent-%COMP%] {\r\nmax-height: calc(100vh - 120px);\r\noverflow-y: auto;\r\nmargin-right:-20px;\r\n}\r\n.tank-view.table[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]:first-child   td[_ngcontent-%COMP%]{\r\n    border-top: 0px solid #e7eaec;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]{\r\n    margin-bottom: 5px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]   .active[_ngcontent-%COMP%]{\r\n    font-weight: 700;\r\n    color: brown;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%], .location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:last-child   .card-header[_ngcontent-%COMP%] {\r\n    border: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%] {\r\n    background: transparent;\r\n    border-bottom: 0;\r\n    padding: 0;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%]:hover {\r\n    background: #e2e0ff5e !important;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-body[_ngcontent-%COMP%] {\r\n    \r\n    padding: 0px;\r\n    margin-left: -8px;\r\n    margin-right: -8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .fa-stack[_ngcontent-%COMP%] {\r\n    font-size: 8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    border: 0;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%] {\r\n    background-color: transparent;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #616161;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\r\n    width: 100%;\r\n    \r\n    color: #004987;\r\n    padding: 0;\r\n}\r\n.icon-color-b[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:hover, .location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:focus {\r\n    text-decoration: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%] {\r\n    color: #616161 !important;\r\n}\r\n\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    padding: 3px 20px;\r\n}\r\n.bg-change[_ngcontent-%COMP%] {\r\n    background: #e2e0ff5e !important;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\r\n@keyframes blink {\r\nfrom,to {\r\nopacity: 0;\r\n}\r\n\r\n50% {\r\nopacity: 1;\r\n}\r\n}\r\n@-webkit-keyframes blink {\r\nfrom, to {\r\nopacity: 0;\r\n}\r\n\r\n50% {\r\nopacity: 1;\r\n}\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9qb2ItdGFuay1oaWVyYXJjaHkvam9iLXRhbmstaGllcmFyY2h5LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7QUFDQSwyQkFBMkI7QUFDM0IsK0JBQStCO0FBQy9CLGdCQUFnQjtBQUNoQjtBQUNBO0FBQ0EsK0JBQStCO0FBQy9CLGdCQUFnQjtBQUNoQixrQkFBa0I7QUFDbEI7QUFFQTtJQUNJLGdCQUFnQjtBQUNwQjtBQUVBO0lBQ0ksNkJBQTZCO0FBQ2pDO0FBQ0E7SUFDSSxnQkFBZ0I7QUFDcEI7QUFDQTtJQUNJLGtCQUFrQjtBQUN0QjtBQUNBO0lBQ0ksZ0JBQWdCO0lBQ2hCLFlBQVk7QUFDaEI7QUFDQTs7SUFFSSxZQUFZO0FBQ2hCO0FBRUE7SUFDSSx1QkFBdUI7SUFDdkIsZ0JBQWdCO0lBQ2hCLFVBQVU7QUFDZDtBQUVBO0lBQ0ksZ0NBQWdDO0FBQ3BDO0FBRUE7SUFDSSxrQkFBa0I7SUFDbEIsWUFBWTtJQUNaLGlCQUFpQjtJQUNqQixrQkFBa0I7QUFDdEI7QUFFQTtJQUNJLGNBQWM7QUFDbEI7QUFFQTtJQUNJLFNBQVM7QUFDYjtBQUVBO0lBQ0ksNkJBQTZCO0FBQ2pDO0FBRUE7SUFDSSxjQUFjO0lBQ2QsaUJBQWlCO0FBQ3JCO0FBRUE7SUFDSSxjQUFjO0FBQ2xCO0FBRUE7SUFDSSxXQUFXO0lBQ1gscUJBQXFCO0lBQ3JCLGNBQWM7SUFDZCxVQUFVO0FBQ2Q7QUFFQTtJQUNJLGNBQWM7QUFDbEI7QUFFQTs7SUFFSSxxQkFBcUI7QUFDekI7QUFHQTtJQUNJLHlCQUF5QjtBQUM3QjtBQUVBOztNQUVNO0FBQ047SUFDSSxpQkFBaUI7QUFDckI7QUFFQTtJQUNJLGdDQUFnQztJQUNoQyxpQkFBaUI7QUFDckI7QUFFQTtJQUNJLFlBQVk7SUFDWixXQUFXO0lBQ1gseUJBQXlCO0lBQ3pCLGtCQUFrQjtJQUNsQixxQkFBcUI7SUFDckIseUNBQWlDO1lBQWpDLGlDQUFpQztBQUNyQztBQUdBO0FBQ0E7QUFDQSxVQUFVO0FBQ1Y7O0FBRUE7QUFDQSxVQUFVO0FBQ1Y7QUFDQTtBQVlBO0FBQ0E7QUFDQSxVQUFVO0FBQ1Y7O0FBRUE7QUFDQSxVQUFVO0FBQ1Y7QUFDQSIsImZpbGUiOiJzcmMvYXBwL2Rpc3BhdGNoZXIvZGlzcGF0Y2hlci1kYXNoYm9hcmQvam9iLXRhbmstaGllcmFyY2h5L2pvYi10YW5rLWhpZXJhcmNoeS5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmxvY2F0aW9uLXBhbmVsIHtcclxuaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTMwcHgpO1xyXG5tYXgtaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTIwcHgpO1xyXG5vdmVyZmxvdy15OiBhdXRvO1xyXG59XHJcbi5sb2NhdGlvbi1jaGFydC1wYW5lbCB7XHJcbm1heC1oZWlnaHQ6IGNhbGMoMTAwdmggLSAxMjBweCk7XHJcbm92ZXJmbG93LXk6IGF1dG87XHJcbm1hcmdpbi1yaWdodDotMjBweDtcclxufVxyXG5cclxuLnRhbmstdmlldy50YWJsZSA+IHRib2R5ID4gdHIgPiB0ZHtcclxuICAgIHBhZGRpbmc6IDRweCA4cHg7XHJcbn1cclxuXHJcbi50YWJsZS50YW5rLXZpZXctY2hpbGQgPiB0Ym9keSA+IHRyOmZpcnN0LWNoaWxkIHRke1xyXG4gICAgYm9yZGVyLXRvcDogMHB4IHNvbGlkICNlN2VhZWM7XHJcbn1cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZCA+IHRib2R5ID4gdHIgPiB0ZHtcclxuICAgIHBhZGRpbmc6IDRweCA4cHg7XHJcbn1cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZHtcclxuICAgIG1hcmdpbi1ib3R0b206IDVweDtcclxufVxyXG4udGFibGUudGFuay12aWV3LWNoaWxkIC5hY3RpdmV7XHJcbiAgICBmb250LXdlaWdodDogNzAwO1xyXG4gICAgY29sb3I6IGJyb3duO1xyXG59XHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQsXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQ6bGFzdC1jaGlsZCAuY2FyZC1oZWFkZXIge1xyXG4gICAgYm9yZGVyOiBub25lO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWhlYWRlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiB0cmFuc3BhcmVudDtcclxuICAgIGJvcmRlci1ib3R0b206IDA7XHJcbiAgICBwYWRkaW5nOiAwO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWhlYWRlcjpob3ZlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZTJlMGZmNWUgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAuY2FyZC1ib2R5IHtcclxuICAgIC8qIHBhZGRpbmc6IDVweDsgKi9cclxuICAgIHBhZGRpbmc6IDBweDtcclxuICAgIG1hcmdpbi1sZWZ0OiAtOHB4O1xyXG4gICAgbWFyZ2luLXJpZ2h0OiAtOHB4O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5mYS1zdGFjayB7XHJcbiAgICBmb250LXNpemU6IDhweDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtIHtcclxuICAgIGJvcmRlcjogMDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtLmFjdGl2ZSB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtLmFjdGl2ZSBhIHtcclxuICAgIGNvbG9yOiAjMTA2MmQxO1xyXG4gICAgZm9udC13ZWlnaHQ6IGJvbGQ7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmxpc3QtZ3JvdXAtaXRlbSBhIHtcclxuICAgIGNvbG9yOiAjNjE2MTYxO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4ge1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICAvKmZvbnQtd2VpZ2h0OiBib2xkOyovXHJcbiAgICBjb2xvcjogIzAwNDk4NztcclxuICAgIHBhZGRpbmc6IDA7XHJcbn1cclxuXHJcbi5pY29uLWNvbG9yLWIge1xyXG4gICAgY29sb3I6ICMxMDYyZDE7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmJ0bi1saW5rOmhvdmVyLFxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4tbGluazpmb2N1cyB7XHJcbiAgICB0ZXh0LWRlY29yYXRpb246IG5vbmU7XHJcbn1cclxuXHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4tbGluayB7XHJcbiAgICBjb2xvcjogIzYxNjE2MSAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4vKi5sb2NhdGlvbi1hY2NvcmRpb24gbGkgKyBsaSB7XHJcbiAgICAgICAgbWFyZ2luLXRvcDogMTBweDtcclxuICAgIH0qL1xyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5saXN0LWdyb3VwLWl0ZW0ge1xyXG4gICAgcGFkZGluZzogM3B4IDIwcHg7XHJcbn1cclxuXHJcbi5iZy1jaGFuZ2Uge1xyXG4gICAgYmFja2dyb3VuZDogI2UyZTBmZjVlICFpbXBvcnRhbnQ7XHJcbiAgICBmb250LXdlaWdodDogYm9sZDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAuYWN0aXZlLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmZjU4NTg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcblxyXG5Aa2V5ZnJhbWVzIGJsaW5rIHtcclxuZnJvbSx0byB7XHJcbm9wYWNpdHk6IDA7XHJcbn1cclxuXHJcbjUwJSB7XHJcbm9wYWNpdHk6IDE7XHJcbn1cclxufVxyXG5cclxuQC1tb3ota2V5ZnJhbWVzIGJsaW5rIHtcclxuZnJvbSwgdG8ge1xyXG5vcGFjaXR5OiAwO1xyXG59XHJcblxyXG41MCUge1xyXG5vcGFjaXR5OiAxO1xyXG59XHJcbn1cclxuXHJcbkAtd2Via2l0LWtleWZyYW1lcyBibGluayB7XHJcbmZyb20sIHRvIHtcclxub3BhY2l0eTogMDtcclxufVxyXG5cclxuNTAlIHtcclxub3BhY2l0eTogMTtcclxufVxyXG59XHJcblxyXG5ALW1zLWtleWZyYW1lcyBibGluayB7XHJcbmZyb20sIHRvIHtcclxub3BhY2l0eTogMDtcclxufVxyXG5cclxuNTAlIHtcclxub3BhY2l0eTogMTtcclxufVxyXG59XHJcblxyXG5ALW8ta2V5ZnJhbWVzIGJsaW5rIHtcclxuZnJvbSwgdG8ge1xyXG5vcGFjaXR5OiAwO1xyXG59XHJcblxyXG41MCUge1xyXG5vcGFjaXR5OiAxO1xyXG59XHJcbn1cclxuXHJcblxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](JobTankHierarchyComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-job-tank-hierarchy',
          templateUrl: './job-tank-hierarchy.component.html',
          styleUrls: ['./job-tank-hierarchy.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_4__["DispatcherService"]
        }];
      }, {
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_2__["DipTestComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/location.component.ts": function srcAppDispatcherDispatcherDashboardLocationComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationComponent", function () {
      return LocationComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./sales-data/location-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/location-view.component.ts");
    /* harmony import */


    var _dispatcher_model__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../dispatcher.model */
    "./src/app/dispatcher/dispatcher.model.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var _shared_components_demand_capture_chart_demand_capture_chart_component__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ../../shared-components/demand-capture-chart/demand-capture-chart.component */
    "./src/app/shared-components/demand-capture-chart/demand-capture-chart.component.ts");
    /* harmony import */


    var ng2_charts__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ng2-charts */
    "./node_modules/ng2-charts/__ivy_ngcc__/fesm2015/ng2-charts.js");

    function LocationComponent_div_11_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "ng-multiselect-dropdown", 30, 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_div_11_ng_template_3_Template_ng_multiselect_dropdown_ngModelChange_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r15.SelectedCarrierList = $event;
        })("onSelect", function LocationComponent_div_11_ng_template_3_Template_ng_multiselect_dropdown_onSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r17.carrierChanged();
        })("onDeSelect", function LocationComponent_div_11_ng_template_3_Template_ng_multiselect_dropdown_onDeSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r18.carrierChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r13.SelectedCarrierList)("placeholder", "Select Carrier")("settings", ctx_r13.multiselectSettingsById)("data", ctx_r13.carrierList);
      }
    }

    function LocationComponent_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, LocationComponent_div_11_ng_template_3_Template, 3, 4, "ng-template", null, 26, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r12)("autoClose", "outside");
      }
    }

    function LocationComponent_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r2.count);
      }
    }

    function LocationComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c0 = function _c0() {
      return {
        "height": 24,
        "width": 24
      };
    };

    var _c1 = function _c1(a0, a1) {
      return {
        "url": a0,
        "scaledSize": a1
      };
    };

    function LocationComponent_div_24_ng_container_22_Template(rf, ctx) {
      if (rf & 1) {
        var _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "agm-marker", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("mouseOver", function LocationComponent_div_24_ng_container_22_Template_agm_marker_mouseOver_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3);

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r23.mouseHoverMarker(_r22, $event);
        })("mouseOut", function LocationComponent_div_24_ng_container_22_Template_agm_marker_mouseOut_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3);

          var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r25.mouseHoveOutMarker(_r22, $event);
        })("markerClick", function LocationComponent_div_24_ng_container_22_Template_agm_marker_markerClick_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var jobLocation_r21 = ctx.$implicit;

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r26.onInfoViewClick(jobLocation_r21);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "agm-info-window", 49, 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var jobLocation_r21 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("latitude", jobLocation_r21.Latitude)("longitude", jobLocation_r21.Longitude)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](7, _c1, jobLocation_r21.iconUrl, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction0"](6, _c0)));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](jobLocation_r21.JobName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](jobLocation_r21.CustomerName);
      }
    }

    function LocationComponent_div_24_div_23_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " No image available");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_div_24_div_23_img_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "img", 92);
      }

      if (rf & 2) {
        var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("src", ctx_r28.opendedJobDetails.SiteImageFilePath, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);
      }
    }

    function LocationComponent_div_24_div_23_span_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "OPEN");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_div_24_div_23_div_41_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "span", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "span", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "OPEN");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var day_r34 = ctx.$implicit;

        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", day_r34, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r30.opendedJobDetails.SiteAvailabilityTiming);
      }
    }

    function LocationComponent_div_24_div_23_div_42_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "No Days Available");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_div_24_div_23_div_61_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    function LocationComponent_div_24_div_23_div_61_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_div_61_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r37);

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r36.closeAssetsClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "i", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Back");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_div_61_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r37);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r38.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, LocationComponent_div_24_div_23_div_61_ng_container_9_Template, 1, 0, "ng-container", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r8)("ngTemplateOutletContext", ctx_r32.assetDetails);
      }
    }

    function LocationComponent_div_24_div_23_div_62_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_div_24_div_23_div_62_app_demand_capture_chart_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "app-demand-capture-chart", 107);
      }

      if (rf & 2) {
        var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx_r40.demandChartData);
      }
    }

    function LocationComponent_div_24_div_23_div_62_Template(rf, ctx) {
      if (rf & 1) {
        var _r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, LocationComponent_div_24_div_23_div_62_div_1_Template, 2, 0, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_div_62_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r42);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r41.closeChartsClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "i", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Back");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_div_62_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r42);

          var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r43.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, LocationComponent_div_24_div_23_div_62_app_demand_capture_chart_12_Template, 1, 1, "app-demand-capture-chart", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](13, "async");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r33.isLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](13, 2, ctx_r33.isChartDataExistSubject));
      }
    }

    function LocationComponent_div_24_div_23_Template(rf, ctx) {
      if (rf & 1) {
        var _r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, LocationComponent_div_24_div_23_div_4_Template, 3, 0, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r45);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r44.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, LocationComponent_div_24_div_23_img_9_Template, 1, 1, "img", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](15, "i", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](20, "i", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](25, "i", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "a", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "span", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "Site Availability:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, LocationComponent_div_24_div_23_span_34_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "a", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](38, "i", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](41, LocationComponent_div_24_div_23_div_41_Template, 7, 2, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](42, LocationComponent_div_24_div_23_div_42_Template, 2, 0, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "label", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, "Site Instruction: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "span", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](51, "Contact(s):");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "p", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "a", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_Template_a_click_57_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r45);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r46.onAssetsViewClick(ctx_r46.opendedJobDetails.jobAssetDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "a", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_div_24_div_23_Template_a_click_59_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r45);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r47.onChartsViewClick(ctx_r47.opendedJobDetails.jobAssetDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](60, "Demand Capture Trend ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](61, LocationComponent_div_24_div_23_div_61_Template, 10, 2, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](62, LocationComponent_div_24_div_23_div_62_Template, 14, 4, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r20.opendedJobDetails.SiteImageFilePath);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r20.opendedJobDetails.SiteImageFilePath);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r20.opendedJobDetails.JobName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r20.opendedJobDetails.CustomerName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate4"](" ", ctx_r20.opendedJobDetails.Address, ", ", ctx_r20.opendedJobDetails.City, ", ", ctx_r20.opendedJobDetails.State, ", ", ctx_r20.opendedJobDetails.ZipCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r20.opendedJobDetails.SiteAvailabilityTotalDays);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r20.opendedJobDetails.SiteAvailabilityTiming);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r20.opendedJobDetails.SiteAvailabilityArray);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r20.opendedJobDetails.SiteAvailabilityArray.length);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r20.opendedJobDetails.SiteInstructions, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r20.opendedJobDetails.ContactPersonName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", ctx_r20.opendedJobDetails.TotalCount, " Tank(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r20.clickAssetsPanel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r20.clickChartsPanel);
      }
    }

    var _c2 = function _c2(a0, a1) {
      return {
        "fadeIn": a0,
        "display_hide": a1
      };
    };

    function LocationComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        var _r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "img", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, " Must Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "img", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, " Should Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](15, "img", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, " Could Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](19, "img", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, " Unplanned ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "agm-map", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("mapReady", function LocationComponent_div_24_Template_agm_map_mapReady_21_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r48.mapReady($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, LocationComponent_div_24_ng_container_22_Template, 10, 10, "ng-container", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, LocationComponent_div_24_div_23_Template, 63, 17, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](14, _c2, ctx_r4.toogleMap, !ctx_r4.toogleMap));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", ctx_r4.clickViewActive ? "col-sm-8 mb15" : "col-sm-12 mb15");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("src", ctx_r4.mustGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("src", ctx_r4.shouldGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("src", ctx_r4.couldGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("src", ctx_r4.noDlrUrl, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("zoom", ctx_r4.zoomLevel)("maxZoom", 16)("minZoom", 2)("mapTypeControl", true)("fullscreenControl", true)("fullscreenControlOptions", ctx_r4.screenOptions);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r4.jobLocationData);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.clickViewActive);
      }
    }

    function LocationComponent_ng_template_29_div_9_ng_container_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    function LocationComponent_ng_template_29_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, LocationComponent_ng_template_29_div_9_ng_container_1_Template, 1, 0, "ng-container", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r8)("ngTemplateOutletContext", ctx_r51.assetDetails);
      }
    }

    var _c3 = function _c3(a2) {
      return {
        "modal": true,
        "fade": true,
        "show": a2
      };
    };

    var _c4 = function _c4(a0) {
      return {
        "display": a0
      };
    };

    function LocationComponent_ng_template_29_Template(rf, ctx) {
      if (rf & 1) {
        var _r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h4", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, " Tank Details ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_ng_template_29_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r54);

          var ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r53.modalClose();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "i", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, LocationComponent_ng_template_29_div_9_Template, 2, 2, "div", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r50 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](3, _c3, modalDetails_r50.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](5, _c4, modalDetails_r50.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", modalDetails_r50.display === "block");
      }
    }

    function LocationComponent_ng_container_31_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    function LocationComponent_ng_template_32_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c5 = function _c5(a0) {
      return {
        "active": a0
      };
    };

    function LocationComponent_ng_template_32_ng_container_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "li", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "a", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_ng_template_32_ng_container_3_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r66);

          var indx_r64 = ctx.index;

          var ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r65.assetTabClicked(indx_r64);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var indx_r64 = ctx.index;

        var ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](2, _c5, ctx_r57.assetDetails.assetIndex === indx_r64));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("Tank (", indx_r64 + 1, ")");
      }
    }

    function LocationComponent_ng_template_32_h2_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "h2", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().assetIndex;

        var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"](" \xA0( ", ctx_r58.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r58.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankName, "-", ctx_r58.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r58.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankNumber, " )");
      }
    }

    function LocationComponent_ng_template_32_a_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Download Dip Chart ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().assetIndex;

        var ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate"]("href", ctx_r59.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r59.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankChartPath, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);
      }
    }

    function LocationComponent_ng_template_32_div_13_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r69.FuelUnit);
      }
    }

    function LocationComponent_ng_template_32_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 130);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "table", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Product Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Tank Capacity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](17, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, LocationComponent_ng_template_32_div_13_span_18_Template, 2, 1, "span", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().assetIndex;

        var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r60.clickedAssetsDetails[assetNumber_r55].ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](17, 3, ctx_r60.clickedAssetsDetails[assetNumber_r55].FuelCapacity), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r60.clickedAssetsDetails[assetNumber_r55].FuelCapacity);
      }
    }

    function LocationComponent_ng_template_32_div_14_span_41_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r71 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r71.FuelUnit);
      }
    }

    function LocationComponent_ng_template_32_div_14_span_52_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r72.FuelUnit);
      }
    }

    function LocationComponent_ng_template_32_div_14_span_63_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r73 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r73.FuelUnit);
      }
    }

    function LocationComponent_ng_template_32_div_14_div_78_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " NA");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_ng_template_32_div_14_ng_template_79_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](1, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](1, 2, ctx_r76.latestReading == null ? null : ctx_r76.latestReading.NetVolume), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r76.FuelUnit);
      }
    }

    var _c6 = function _c6(a0) {
      return {
        "height.px": a0
      };
    };

    function LocationComponent_ng_template_32_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 133);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "div", 134);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 135);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](9, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 136);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](12, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "table", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "td", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Storage ID ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Tank Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Product Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Tank Capacity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](40, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](41, LocationComponent_ng_template_32_div_14_span_41_Template, 2, 1, "span", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, "Min Fill");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](51, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](52, LocationComponent_ng_template_32_div_14_span_52_Template, 2, 1, "span", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "td", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](56, "Max Fill");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](62, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](63, LocationComponent_ng_template_32_div_14_span_63_Template, 2, 1, "span", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "table", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](70, "Last Reading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](72, "Ullage");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](74, "Last Reading Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](75, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](76, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](77, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](78, LocationComponent_ng_template_32_div_14_div_78_Template, 2, 0, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](79, LocationComponent_ng_template_32_div_14_ng_template_79_Template, 4, 4, "ng-template", null, 140, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](82, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](84, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](86, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](88, "date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](80);

        var assetNumber_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().assetIndex;

        var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](38, _c6, ctx_r61.selectedTankHeight.ShouldBeEmptyPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](40, _c6, ctx_r61.selectedTankHeight.ShouldBeFilledPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](9, 20, ctx_r61.selectedTankHeight.sbf_percent, "1.0-2"), "%");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](42, _c6, ctx_r61.selectedTankHeight.CurrentInventoryPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](12, 23, ctx_r61.selectedTankHeight.ci_percent, "1.0-2"), "% ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r61.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r61.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].StorageId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r61.clickedAssetsDetails[assetNumber_r55].TankTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r61.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r61.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TfxProductTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](40, 26, ctx_r61.clickedAssetsDetails[assetNumber_r55].FuelCapacity), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r61.clickedAssetsDetails[assetNumber_r55].FuelCapacity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("\xA0(", ctx_r61.selectedTankMinMax.MinFillPercent, "%)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](51, 28, ctx_r61.selectedTankMinMax.MinFill), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r61.clickedAssetsDetails[assetNumber_r55].MinFill);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("\xA0(", ctx_r61.selectedTankMinMax.MaxFillPercent, "%)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](62, 30, ctx_r61.selectedTankMinMax.MaxFill), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r61.clickedAssetsDetails[assetNumber_r55].MaxFill);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r61.latestReading == null ? null : ctx_r61.latestReading.NetVolume) == 0 - 1 || (ctx_r61.latestReading == null ? null : ctx_r61.latestReading.NetVolume) === undefined)("ngIfElse", _r75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](84, 32, ctx_r61.latestReading == null ? null : ctx_r61.latestReading.Ullage) || "NA");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](88, 34, ctx_r61.latestReading == null ? null : ctx_r61.latestReading.CaptureTimeString, "MM/dd/yyyy, hh:mm a", "UTC") || "NA");
      }
    }

    function LocationComponent_ng_template_32_div_15_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "canvas", 145);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("chartType", "line")("datasets", ctx_r78.chartData)("options", ctx_r78.chartOptions)("labels", ctx_r78.chartLabels)("legend", true);
      }
    }

    function LocationComponent_ng_template_32_div_15_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " No Data Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationComponent_ng_template_32_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Dip test value trend : ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, LocationComponent_ng_template_32_div_15_div_4_Template, 2, 5, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](5, LocationComponent_ng_template_32_div_15_div_5_Template, 2, 0, "div", 143);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r62.chartData.length);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r62.chartData.length && !ctx_r62.isLoading);
      }
    }

    function LocationComponent_ng_template_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, LocationComponent_ng_template_32_div_1_Template, 2, 0, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "ul", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, LocationComponent_ng_template_32_ng_container_3_Template, 4, 4, "ng-container", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "h2", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, LocationComponent_ng_template_32_h2_11_Template, 2, 2, "h2", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, LocationComponent_ng_template_32_a_12_Template, 3, 1, "a", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](13, LocationComponent_ng_template_32_div_13_Template, 19, 5, "div", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](14, LocationComponent_ng_template_32_div_14_Template, 89, 44, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, LocationComponent_ng_template_32_div_15_Template, 6, 2, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r55 = ctx.assetIndex;

        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.isLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r9.clickedAssetsDetails);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r9.clickedAssetsDetails[assetNumber_r55].AssetName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankName) && (ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankNumber));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0] == null ? null : ctx_r9.clickedAssetsDetails[assetNumber_r55].jobTankAdditionalDetails[0].TankChartPath);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.clickedAssetsDetails[assetNumber_r55].AssetType === 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.clickedAssetsDetails[assetNumber_r55].AssetType === 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.clickedAssetsDetails[assetNumber_r55].AssetType === 2);
      }
    }

    function LocationComponent_ng_template_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r83 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 147);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 148);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "ng-multiselect-dropdown", 152);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_6_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r82.SelectedRegions = $event;
        })("onSelect", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r84.onRegionChanged();
        })("onDeSelect", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onDeSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r85.onRegionChanged();
        })("onSelectAll", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onSelectAll_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r86.onRegionChanged();
        })("onDeSelectAll", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onDeSelectAll_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r87.onRegionChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "ng-multiselect-dropdown", 154);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_11_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r88 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r88.SelectedCustomerList = $event;
        })("onSelect", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onSelect_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r89 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r89.onCustomerChanged();
        })("onDeSelect", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_onDeSelect_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r90 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r90.onCustomerChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "ng-multiselect-dropdown", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_17_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r91 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r91.SelectedlocationList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Prority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "ng-multiselect-dropdown", 157, 158);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r92.SelectedPriorityList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "ng-multiselect-dropdown", 157, 158);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_29_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r93 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r93.SelectedStatusList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "ng-multiselect-dropdown", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_ng_template_34_Template_ng_multiselect_dropdown_ngModelChange_35_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r94 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r94.selectedLocAttributeList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 159);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "button", 161);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_ng_template_34_Template_button_click_38_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r95 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r95.ResetFilters();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "button", 162);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_ng_template_34_Template_button_click_40_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r83);

          var ctx_r96 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](15);

          ctx_r96.ApplyFilters("set");
          return _r1.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.SelectedRegions)("settings", ctx_r11.multiselectSettingsById)("placeholder", "Select Region")("data", ctx_r11.Regions);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.SelectedCustomerList)("settings", ctx_r11.CustomerDdlSettings)("placeholder", "Select Customer")("data", ctx_r11.customerList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.SelectedlocationList)("settings", ctx_r11.multiselectSettingsById)("placeholder", "Select Location")("data", ctx_r11.locationList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.SelectedPriorityList)("placeholder", "Priority")("settings", ctx_r11.PriorityDdlSettings)("data", ctx_r11.priorityList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.SelectedStatusList)("placeholder", "Status")("settings", ctx_r11.multiselectSettingsById)("data", ctx_r11.statusList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r11.selectedLocAttributeList)("placeholder", "Inventory Capture Method")("settings", ctx_r11.multiselectSettingsById)("data", ctx_r11.LocationAttributeList);
      }
    }

    var LocationComponent = /*#__PURE__*/function () {
      function LocationComponent(dispatcherService, carrierService) {
        _classCallCheck(this, LocationComponent);

        this.dispatcherService = dispatcherService;
        this.carrierService = carrierService;
        this.isLoading = false;
        this.zoomLevel = 5;
        this.jobLocationData = [];
        this.clickedAssetsDetails = [];
        this.stateList = [];
        this.countryList = [];
        this.cityList = [];
        this.priorityList = [];
        this.statusList = [];
        this.fuelTypeList = [];
        this.customerList = [];
        this.latestReading = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["DipTest"]();
        this.chartData = [];
        this.chartLabels = [];
        this.IsFilterLoaded = false;
        this.selectedTankMinMax = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["TankMinMaxFill"]();
        this.selectedTankHeight = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["TankChartHeight"]();
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subscription"]();
        this.filteredJobLocationData = [];
        this.unchangedJobLocationData = [];
        this.SelectedFilter = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["Filter"]();
        this.assetDetails = {
          assetIndex: 0
        };
        this.assetsModal = {
          modalDetails: {
            display: 'none',
            data: 'Modal Show'
          }
        };
        this.locationSubscription = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subscription"]();
        this.clickViewActive = false;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
        this.toogleMap = false;
        this.toogleFilter = false;
        this.centerLocationLat = 47.1853106;
        this.centerLocationLog = -125.36955;
        this.UserCountry = "USA";
        this.CountryCentre = {
          USA: {
            lat: 39.11757961,
            lng: -103.8784
          },
          CAN: {
            lat: 57.88251631,
            lng: -98.54842922
          }
        };
        this.screenOptions = {
          position: 3
        };
        this.mustGoUrl = "src/assets/marker-mustgo.svg";
        this.shouldGoUrl = "src/assets/marker-shouldgo.svg";
        this.couldGoUrl = "src/assets/marker-couldgo.svg";
        this.noDlrUrl = "src/assets/marker-nodr.svg";
        this.noImageUrl = "Content/images/no-image.png";
        this.isShowCarrierManaged = false;
        this.carrierList = [];
        this.selectedCarrierIds = '';
        this.SelectedRegions = [];
        this.Regions = [];
        this.UnchangedCustomerList = [];
        this.SelectedRegionId = '';
        this.SelectedCustomerList = [];
        this.SelectedlocationList = [];
        this.SelectedCarrierList = [];
        this.SelectedPriorityList = [];
        this.SelectedStatusList = [];
        this.selectedPriorityIds = '';
        this.LocationAttributeList = src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__["InventoryDataCaptureList"];
        this.selectedLocAttributeList = [];
        this.locationList = [];
        this.isShowNonRetailJobs = false;
        this.jobIdsEmittedFromSalesComponent = [];
        this.locationFilterModal = new _dispatcher_model__WEBPACK_IMPORTED_MODULE_6__["LocationFilterModal"]();
        this.count = 0;
        this.isChartDataExistSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](false);

        var _this = this;

        window.addEventListener("beforeunload", function (e) {
          _this.SaveFilters(true);

          return;
        });
      }

      _createClass(LocationComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.getRegions();
          this.getCarriers();
          this.getDispatcherLocation();
          this.priorityList = [{
            Id: 1,
            Name: 'Must Go'
          }, {
            Id: 2,
            Name: 'Should Go'
          }, {
            Id: 3,
            Name: 'Could Go'
          }, {
            Id: 4,
            Name: 'Unplanned'
          }];
          this.statusList = [{
            Id: 'Scheduled',
            Name: 'Scheduled'
          }, {
            Id: 'DR Created',
            Name: 'DR Created'
          }, {
            Id: 'No DR',
            Name: 'No DR'
          }];
          this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.CustomerDdlSettings = {
            singleSelection: false,
            idField: 'CompanyId',
            textField: 'CompanyName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.PriorityDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this8 = this;

          this.dispatcherService.GetCarriersForSupplier().subscribe(function (data) {
            _this8.carrierList = data;
          });
        }
      }, {
        key: "fetchJobLocationData",
        value: function fetchJobLocationData(PageLoad) {
          var _this9 = this;

          this.isLoading = true;

          if (this.jobIdsEmittedFromSalesComponent && this.jobIdsEmittedFromSalesComponent.length) {
            var ids = [];
            this.jobIdsEmittedFromSalesComponent.forEach(function (res) {
              ids.push(res.TfxJobId);
            });
            var jobids = "";
            jobids = ids.join();
            var selectedLocAttributeId = "";

            if (this.selectedLocAttributeList && this.selectedLocAttributeList.length > 0) {
              var ids = [];
              this.selectedLocAttributeList.forEach(function (res) {
                ids.push(res.Id);
              });
              selectedLocAttributeId = ids.join();
            }

            this.locationSubscription.add(this.dispatcherService.getJobLocationDetails(jobids, selectedLocAttributeId).subscribe(function (res) {
              //   this.locationSubscription.add(this.dispatcherService.getJobLocationDetails(this.jobIdsEmittedFromSalesComponent,selectedLocAttributeId).subscribe(res => {
              if (res) {
                _this9.jobLocationData = _this9.addJobPriority(res['Data']['jobLocationDetails']);
              }

              _this9.setCountryCentre();

              _this9.isLoading = false;
            }));
          } else {
            this.unchangedJobLocationData = this.jobLocationData = [];
            this.setCountryCentre();
            this.isLoading = false;
          }
        }
      }, {
        key: "convertToObjectArray",
        value: function convertToObjectArray(data) {
          var modifiedItemArray = [];
          data.forEach(function (item, index) {
            var Item = {
              'Id': 0,
              'Name': ''
            };
            Item.Id = index;
            Item.Name = item;
            modifiedItemArray.push(Item);
          });
          return modifiedItemArray;
        }
      }, {
        key: "fetchCountryListData",
        value: function fetchCountryListData() {
          var _this10 = this;

          this.locationSubscription.add(this.dispatcherService.getCountryList().subscribe(function (res) {
            _this10.countryList = res;
          }));
        }
      }, {
        key: "fetchProductTypeListData",
        value: function fetchProductTypeListData() {
          var _this11 = this;

          this.locationSubscription.add(this.dispatcherService.getProductTypeList().subscribe(function (res) {
            _this11.fuelTypeList = res.Data;
          }));
        }
      }, {
        key: "fetchStateListData",
        value: function fetchStateListData(countryId) {
          var _this12 = this;

          this.locationSubscription.add(this.dispatcherService.getStateList(countryId).subscribe(function (res) {
            _this12.stateList = res;
          }));
        }
      }, {
        key: "fetchCityListData",
        value: function fetchCityListData(stateId) {
          var _this13 = this;

          this.locationSubscription.add(this.dispatcherService.getCityList(stateId).subscribe(function (res) {
            _this13.cityList = res;
          }));
        }
      }, {
        key: "addJobPriority",
        value: function addJobPriority(jobLocationData) {
          var _this14 = this;

          if (jobLocationData && jobLocationData.length) {
            jobLocationData.forEach(function (element) {
              var obj = _this14.jobIdsEmittedFromSalesComponent.find(function (t) {
                return t.TfxJobId == element.JobID;
              });

              if (obj) {
                if (obj.Priority == 1) {
                  element.highestPriority = 1;
                  element.iconUrl = _this14.mustGoUrl;
                } else if (obj.Priority == 2) {
                  element.highestPriority = 2;
                  element.iconUrl = _this14.shouldGoUrl;
                } else if (obj.Priority == 3) {
                  element.highestPriority = 3;
                  element.iconUrl = _this14.couldGoUrl;
                } else {
                  element.highestPriority = 4;
                  element.iconUrl = _this14.noDlrUrl;
                }
              } else {
                element.highestPriority = 4;
                element.iconUrl = _this14.noDlrUrl;
              }
            });
          }

          return jobLocationData;
        }
      }, {
        key: "checkMostPriorityJob",
        value: function checkMostPriorityJob(jobLocationData) {
          var jobLocationLength = jobLocationData.length;

          for (var i = 0; i < jobLocationLength; i++) {
            var deliveryRequests = jobLocationData[i].jobDeliveryRequests;

            if (deliveryRequests.length) {
              var filteredMustGoDRs = deliveryRequests.filter(function (data) {
                return data.Priority === 1;
              });
              var filteredShoudGoDRs = deliveryRequests.filter(function (data) {
                return data.Priority === 2;
              });
              var filteredCouldGoDRs = deliveryRequests.filter(function (data) {
                return data.Priority === 3;
              });

              if (filteredMustGoDRs.length > 0) {
                jobLocationData[i].highestPriority = 1;
                jobLocationData[i].iconUrl = this.mustGoUrl;
              } else if (filteredShoudGoDRs.length > 0) {
                jobLocationData[i].highestPriority = 2;
                jobLocationData[i].iconUrl = this.shouldGoUrl;
              } else {
                jobLocationData[i].highestPriority = 3;
                jobLocationData[i].iconUrl = this.couldGoUrl;
              }
            } else {
              jobLocationData[i].highestPriority = 4;
              jobLocationData[i].iconUrl = this.noDlrUrl;
            }
          }

          return jobLocationData;
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          if (this.locationSubscription) {
            this.locationSubscription.unsubscribe();
          }

          this.SaveFilters(true);

          if (this.setCountryCenterInterval) {
            clearInterval(this.setCountryCenterInterval);
          }
        }
      }, {
        key: "mouseHoverMarker",
        value: function mouseHoverMarker(infoWindow, event) {
          infoWindow.open();
        }
      }, {
        key: "mouseHoveOutMarker",
        value: function mouseHoveOutMarker(infoWindow, event) {
          infoWindow.close();
        }
      }, {
        key: "onInfoViewClick",
        value: function onInfoViewClick(jobLocation) {
          window.scrollTo(0, 0);
          this.opendedJobDetails = jobLocation;
          this.assetDetails.assetIndex = 0;
          this.clickViewActive = true;
          this.clickAssetsPanel = false;
          this.clickChartsPanel = false;
          this.toogleMap = true;
          this.closeAssetsClicked();
        }
      }, {
        key: "closeViewClicked",
        value: function closeViewClicked() {
          this.clickViewActive = false;
          this.clickAssetsPanel = false;
          this.clickChartsPanel = false;
        }
      }, {
        key: "onAssetsViewClick",
        value: function onAssetsViewClick(assets) {
          if (assets.length) {
            this.clickAssetsPanel = true;
            this.clickedAssetsDetails = assets;

            if (assets[0].jobTankAdditionalDetails.length) {
              this.getDipTestDetails(assets[0].jobTankAdditionalDetails[0]['SiteId'], assets[0].jobTankAdditionalDetails[0]['TankId'], 3);
            } else {
              this.chartData = [];
              this.latestReading = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["DipTest"]();
            }
          }
        }
      }, {
        key: "closeAssetsClicked",
        value: function closeAssetsClicked() {
          this.clickAssetsPanel = false;
        }
      }, {
        key: "onChartsViewClick",
        value: function onChartsViewClick(assets) {
          this.clickChartsPanel = true;
          this.isChartDataExistSubject.next(false);

          if (assets.length && assets[0].jobTankAdditionalDetails.length) {
            this.getDemandCaptureChart(assets[0].jobTankAdditionalDetails[0]['SiteId'], 3, assets[0].JobId);
          } else {
            this.isChartDataExistSubject.next(false);
            this.demandChartData = null;
          }
        }
      }, {
        key: "closeChartsClicked",
        value: function closeChartsClicked() {
          this.clickChartsPanel = false;
        }
      }, {
        key: "assetTabClicked",
        value: function assetTabClicked(indx) {
          this.assetDetails.assetIndex = indx;

          if (this.clickedAssetsDetails[indx].jobTankAdditionalDetails.length) {
            this.getDipTestDetails(this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['SiteId'], this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['TankId'], 3);
          } else {
            this.chartData = [];
            this.latestReading = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["DipTest"]();
          }
        }
      }, {
        key: "toggleMapView",
        value: function toggleMapView() {
          this.toogleMap = !this.toogleMap;

          if (this.toogleMap) {
            this.fetchJobLocationData();
          } //this.SaveFilters();

        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "tableClickFilter",
        value: function tableClickFilter(data, index) {}
      }, {
        key: "modalOpen",
        value: function modalOpen(jobLocation) {
          this.opendedJobDetails = jobLocation;
          this.clickedAssetsDetails = this.opendedJobDetails.jobAssetDetails;

          if (this.clickedAssetsDetails.length) {
            this.closeAssetsClicked();
            this.closeViewClicked();
            this.closeChartsClicked();
            this.assetDetails.assetIndex = 0;

            if (this.clickedAssetsDetails[0].jobTankAdditionalDetails.length) {
              this.getDipTestDetails(this.clickedAssetsDetails[0].jobTankAdditionalDetails[0]['SiteId'], this.clickedAssetsDetails[0].jobTankAdditionalDetails[0]['TankId'], 3);
            }

            this.assetsModal = {
              modalDetails: {
                display: 'block',
                data: 'Modal Show'
              }
            };
          }
        }
      }, {
        key: "modalClose",
        value: function modalClose() {
          this.assetsModal = {
            modalDetails: {
              display: 'none',
              data: 'Modal Show'
            }
          };
        }
      }, {
        key: "getDipTestDetails",
        value: function getDipTestDetails(siteId, tankId, noOfDays) {
          var _this15 = this;

          this.isLoading = true;
          this.chartData = [];
          this.chartOptions = {};
          this.chartOptions = this.setChartOptions(this.UserCountry);
          this.chartLabels = [];
          this.latestReading = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_4__["DipTest"]();
          this.locationSubscription.add(this.dispatcherService.getDipTestDetails(siteId, tankId, noOfDays).subscribe(function (data) {
            if (data && data.StatusCode === 302) {
              var resp = data.Data;
              _this15.latestReading = resp[0];
              var obj = {};
              var chartdata = [];
              obj['label'] = 'Tank ' + resp[0]['TankId'];
              var respLen = resp.length;

              for (var i = 0; i < respLen; i++) {
                var captureTime = moment__WEBPACK_IMPORTED_MODULE_3__(new Date(resp[i].CaptureTimeString)).format('MM/DD/YYYY hh:mm A');
                chartdata.unshift(resp[i].NetVolume);

                _this15.chartLabels.unshift(captureTime);
              }

              obj['data'] = chartdata;

              _this15.chartData.push(obj);
            }

            _this15.calculateMinMAx(_this15.clickedAssetsDetails[_this15.assetDetails.assetIndex].jobTankAdditionalDetails[0]);

            _this15.isLoading = false;
          }));
        }
      }, {
        key: "getDemandCaptureChart",
        value: function getDemandCaptureChart(siteId, noOfDays, tfxJobId) {
          // this.demandChartData = {};
          this.demandChartData = {
            siteId: siteId,
            noOfDays: noOfDays,
            tfxJobId: tfxJobId
          };
          this.isChartDataExistSubject.next(true);
        }
      }, {
        key: "mapReady",
        value: function mapReady(map) {
          this.Map = map;
          this.setCountryCentre();
        }
      }, {
        key: "setZoomLevel",
        value: function setZoomLevel() {
          if (this.jobLocationData.length == 0) {
            this.setCountryCentre();
          } else {
            this.Map.setZoom(5);
          }
        }
      }, {
        key: "setCountryCentre",
        value: function setCountryCentre() {
          var _this16 = this;

          if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(function () {
              _this16.centerLocationLat = _this16.CountryCentre[_this16.UserCountry].lat;
              _this16.centerLocationLog = _this16.CountryCentre[_this16.UserCountry].lng;

              if (_this16.Map && _this16.jobLocationData.length == 0) {
                _this16.Map.setCenter(new google.maps.LatLng(_this16.centerLocationLat, _this16.centerLocationLog));

                _this16.Map.setZoom(5);
              } else {
                var bounds = new google.maps.LatLngBounds();

                _this16.jobLocationData.forEach(function (x) {
                  bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
                });

                if (_this16.Map && bounds) {
                  _this16.Map.fitBounds(bounds);

                  _this16.Map.setCenter(bounds.getCenter());

                  _this16.Map.setZoom(5);
                }
              }
            }, 500);
          }
        }
      }, {
        key: "setChartOptions",
        value: function setChartOptions(country) {
          this.FuelUnit = country === 'USA' ? 'Gallons' : 'Litres';
          return {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              yAxes: [{
                scaleLabel: {
                  display: true,
                  labelString: "NetVolume ( Fuels Per ".concat(this.FuelUnit, ")")
                },
                ticks: {
                  callback: function callback(label) {
                    return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                  }
                }
              }],
              xAxes: [{
                type: 'time',
                time: {
                  displayFormats: {
                    'millisecond': 'MMM DD',
                    'second': 'MMM DD',
                    'minute': 'MMM DD',
                    'hour': 'MMM DD',
                    'day': 'MMM DD',
                    'week': 'MMM DD',
                    'month': 'MMM DD',
                    'quarter': 'MMM DD',
                    'year': 'MMM DD'
                  }
                }
              }]
            }
          };
        }
      }, {
        key: "getDispatcherLocation",
        value: function getDispatcherLocation() {
          var _this17 = this;

          this.dispatcherService.getDispatcherCountry().subscribe(function (data) {
            _this17.UserCountry = data;
            _this17.FuelUnit = _this17.UserCountry === 'USA' ? 'Gallons' : 'Litres';
          });
        }
      }, {
        key: "setCenterMap",
        value: function setCenterMap($event) {
          if (this.UserCountry && !this.jobLocationData.length) {
            this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
            this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;

            if (this.Map) {
              this.Map.setCenter({
                lat: this.centerLocationLat,
                lng: this.centerLocationLog
              });
              this.Map.setZoom(5);
            }
          }
        }
      }, {
        key: "downloadDipChart",
        value: function downloadDipChart(event, assetNumber) {
          var anchor = event.target;
          var orignalCanvas = document.getElementsByTagName('canvas')[0];
          var resizedCanvas = document.createElement("canvas");
          var resizedContext = resizedCanvas.getContext("2d");
          resizedCanvas.height = 500;
          resizedCanvas.width = 800;
          var context = orignalCanvas.getContext("2d");
          resizedContext.drawImage(orignalCanvas, 0, 0, 800, 500);
          anchor.href = resizedCanvas.toDataURL('image/jpeg', 1.0);
          anchor.download = "".concat(this.clickedAssetsDetails[assetNumber].AssetName, ".png");
        }
      }, {
        key: "calculateMinMAx",
        value: function calculateMinMAx(selectedTank) {
          this.selectedTankMinMax.MaxFill = selectedTank.MaxFill;
          this.selectedTankMinMax.MinFill = selectedTank.MinFill;
          this.selectedTankMinMax.MaxFillPercent = selectedTank.MaxFillPercent;
          this.selectedTankMinMax.MinFillPercent = selectedTank.MinFillPercent;
          var ci_percent = (this.latestReading.NetVolume || 0) / selectedTank.FuelCapacity * 100;
          ci_percent = ci_percent > selectedTank.MaxFillPercent ? selectedTank.MaxFillPercent : ci_percent;
          ci_percent = ci_percent < 0 ? 0 : ci_percent;
          var sbf_percent = selectedTank.MaxFillPercent - ci_percent;
          sbf_percent = sbf_percent > 100 ? 100 : sbf_percent;
          sbf_percent = sbf_percent < 0 ? 0 : sbf_percent;
          this.fillTankDiagram(sbf_percent, ci_percent);
        }
      }, {
        key: "fillTankDiagram",
        value: function fillTankDiagram(sbf_percent, ci_percent) {
          this.selectedTankHeight.sbf_percent = sbf_percent;
          this.selectedTankHeight.ci_percent = ci_percent;
          var min_ShouldBeEmptyPercent = 125 - (sbf_percent * 1.25 + ci_percent * 1.25);
          var min_ShouldBeFilledPercent = sbf_percent * 1.25;
          var min_CurrentInventoryPercent = ci_percent * 1.25; //need of cal

          if (min_ShouldBeFilledPercent < 16 || min_CurrentInventoryPercent < 16) {
            //dont remove from emtty
            if (min_ShouldBeEmptyPercent < 16) {
              if (min_ShouldBeFilledPercent < 16) {
                min_ShouldBeFilledPercent = min_ShouldBeFilledPercent + 16;
                min_CurrentInventoryPercent = min_CurrentInventoryPercent - 16;
              }

              if (min_CurrentInventoryPercent < 16) {
                min_CurrentInventoryPercent = min_CurrentInventoryPercent + 16;
                min_ShouldBeFilledPercent = min_ShouldBeFilledPercent - 16;
              }
            } //remove from empty
            else {
              if (min_ShouldBeFilledPercent < 16) {
                min_ShouldBeFilledPercent = min_ShouldBeFilledPercent + 16;
                min_ShouldBeEmptyPercent = min_ShouldBeEmptyPercent - 16;
              }

              if (min_CurrentInventoryPercent < 16) {
                min_CurrentInventoryPercent = min_CurrentInventoryPercent + 16;
                min_ShouldBeEmptyPercent = min_ShouldBeEmptyPercent - 16;
              }
            }
          }

          this.selectedTankHeight.CurrentInventoryPercent = min_CurrentInventoryPercent;
          this.selectedTankHeight.ShouldBeFilledPercent = min_ShouldBeFilledPercent;
          this.selectedTankHeight.ShouldBeEmptyPercent = min_ShouldBeEmptyPercent;
        }
      }, {
        key: "ShowCarrierMangedData",
        value: function ShowCarrierMangedData() {
          this.getAllCustomers();
        }
      }, {
        key: "carrierChanged",
        value: function carrierChanged() {
          var ids = [];
          this.selectedCarrierIds = '';
          this.SelectedCarrierList.forEach(function (res) {
            ids.push(res.Id);
          });
          this.selectedCarrierIds = ids.join();
          this.getAllCustomers();
        }
      }, {
        key: "onCustomerChanged",
        value: function onCustomerChanged() {
          var _this18 = this;

          if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
            this.locationList = [];
            var customers = this.customerList.filter(function (t) {
              return _this18.SelectedCustomerList.filter(function (el) {
                return el.CompanyId == t.CompanyId;
              }).length > 0;
            });
            customers.forEach(function (res) {
              if (!_this18.locationList.find(function (t) {
                return t.Id == res.Id;
              })) {
                _this18.locationList = _this18.locationList.concat(res.Jobs);
              }
            });
            this.locationList = this.GetUniqueItems(this.locationList.reduce(function (p, n) {
              return p.concat(n);
            }, []));

            if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
              this.SelectedlocationList = this.SelectedlocationList.filter(function (t) {
                return _this18.locationList.filter(function (el) {
                  return el.Id == t.Id;
                }).length > 0;
              });
            }
          } else {
            this.initAllLocation();
          }
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this19 = this;

          this.dispatcherService.GetDispatcherRegions().subscribe(function (data) {
            _this19.Regions = data;

            if (_this19.Regions && _this19.Regions.length > 0) {
              _this19.SelectedRegions = [];

              _this19.SelectedRegions.push(_this19.Regions[0]);

              var ids = [];

              _this19.SelectedRegions.forEach(function (res) {
                ids.push(res.Id);
              });

              _this19.SelectedRegionId = ids.join();
            }

            _this19.GetFilters();
          });
        }
      }, {
        key: "getCustomerListByRegionId",
        value: function getCustomerListByRegionId(SelectedRegionId) {
          var _this20 = this;

          this.carrierService.getJobListForCarrier(SelectedRegionId, this.isShowCarrierManaged, this.selectedCarrierIds).subscribe(function (t2) {
            _this20.customerList = t2;

            if (_this20.SelectedCustomerList && _this20.SelectedCustomerList.length > 0) {
              _this20.SelectedCustomerList = _this20.SelectedCustomerList.filter(function (t) {
                return _this20.customerList.filter(function (el) {
                  return el.CompanyId == t.CompanyId;
                }).length > 0;
              });
            }

            _this20.initAllLocation();
          });
        }
      }, {
        key: "initAllLocation",
        value: function initAllLocation() {
          var _this21 = this;

          this.locationList = [];

          if (this.SelectedRegions && this.SelectedRegions.length > 0) {
            this.customerList.forEach(function (res) {
              _this21.locationList = _this21.locationList.concat(res.Jobs.filter(function (t) {
                return _this21.SelectedRegions.some(function (el) {
                  return el.Id == t.RegionId;
                });
              }));
            });
          } else {
            this.customerList.forEach(function (res) {
              if (!_this21.locationList.find(function (t) {
                return t.Id == res.Id;
              })) {
                _this21.locationList = _this21.locationList.concat(res.Jobs);
              }
            });
          }

          this.locationList = this.GetUniqueItems(this.locationList.reduce(function (p, n) {
            return p.concat(n);
          }, []));

          if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
            this.SelectedlocationList = this.SelectedlocationList.filter(function (t) {
              return _this21.locationList.filter(function (el) {
                return el.Id == t.Id;
              }).length > 0;
            });
          }
        }
      }, {
        key: "GetUniqueItems",
        value: function GetUniqueItems(items) {
          var ids = [];
          var uniqueItems = items.filter(function (item) {
            return ids.includes(item.Id) ? false : ids.push(item.Id);
          });
          return uniqueItems.sort(function (a, b) {
            return a.Name.localeCompare(b.Name);
          });
        }
      }, {
        key: "onRegionChanged",
        value: function onRegionChanged() {
          this.setCustomerAndLocations();
        }
      }, {
        key: "setCustomerAndLocations",
        value: function setCustomerAndLocations() {
          var _this22 = this;

          if (this.SelectedRegions && this.SelectedRegions.length > 0) {
            this.customerList = this.UnchangedCustomerList.filter(function (t) {
              return _this22.SelectedRegions.filter(function (el) {
                return t.RegionIds.some(function (r) {
                  return el.Id == r;
                });
              }).length > 0;
            });
          } else {
            this.customerList = this.UnchangedCustomerList;
          }

          if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
            this.SelectedCustomerList = this.SelectedCustomerList.filter(function (t) {
              return _this22.customerList.filter(function (el) {
                return el.CompanyId == t.CompanyId;
              }).length > 0;
            });
          }

          this.initAllLocation();
        }
      }, {
        key: "getAllCustomers",
        value: function getAllCustomers() {
          var _this23 = this;

          var ids = [];
          this.Regions.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedRegionId = ids.join();
          this.carrierService.getJobListForCarrier(selectedRegionId, this.isShowCarrierManaged, this.selectedCarrierIds).subscribe(function (t2) {
            _this23.UnchangedCustomerList = t2;

            _this23.setCustomerAndLocations();
          });
        }
      }, {
        key: "onLocationTypeChange",
        value: function onLocationTypeChange($event) {}
      }, {
        key: "ResetFilters",
        value: function ResetFilters() {
          this.SelectedRegions = [];
          this.SelectedCustomerList = [];
          this.SelectedlocationList = [];
          this.SelectedPriorityList = [];
          this.SelectedStatusList = [];
          this.selectedLocAttributeList = [];
          this.ApplyFilters("reset");
        }
      }, {
        key: "ApplyFilters",
        value: function ApplyFilters(msg) {
          var _this24 = this;

          this.SaveFilters(false);
          this.count = 0;
          var Regionids = [];
          this.SelectedRegions.forEach(function (res) {
            _this24.count++;
            Regionids.push(res.Id);
          });
          this.locationFilterModal.SelectedRegionId = Regionids.join();
          var Customerids = [];
          this.SelectedCustomerList.forEach(function (res) {
            _this24.count++;
            Customerids.push(res.CompanyId);
          });
          this.locationFilterModal.SelectedCustomerId = Customerids.join();
          var Locationids = [];
          this.SelectedlocationList.forEach(function (res) {
            _this24.count++;
            Locationids.push(res.Id);
          });
          this.locationFilterModal.SelectedLocationId = Locationids.join();
          var Statusids = [];
          this.SelectedStatusList.forEach(function (res) {
            _this24.count++;
            Statusids.push(res.Id);
          });
          this.locationFilterModal.SelectedStatusId = Statusids.join();
          var Prioritiesids = [];
          this.SelectedPriorityList.forEach(function (res) {
            _this24.count++;
            Prioritiesids.push(res.Id);
          });
          this.locationFilterModal.SelectedPrioritiesId = Prioritiesids.join();
          var LocAttributeids = [];

          if (this.selectedLocAttributeList.length != 0) {
            this.selectedLocAttributeList.forEach(function (res) {
              _this24.count++;
              LocAttributeids.push(res.Id);
            });
          } else {
            // LocAttributeids.push(0);
            LocAttributeids = [0, 1, 2, 3];
          }

          this.locationFilterModal.selectedLocAttributeId = LocAttributeids.join();
          this.locationGridView.applyFilters(this.locationFilterModal);

          if (msg == "set") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__["Declarations"].msgsuccess("Filter applied successfully", undefined, undefined);
          } else if (msg == "reset") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_7__["Declarations"].msginfo("Filter reset successfully", undefined, undefined);
          }
        }
      }, {
        key: "SaveFilters",
        value: function SaveFilters(isTopFilter) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
            var _this25 = this;

            var data;
            return regeneratorRuntime.wrap(function _callee$(_context) {
              while (1) {
                switch (_context.prev = _context.next) {
                  case 0:
                    data = {};
                    this.dispatcherService.getFilters(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].SupplierWallyboardLocation).subscribe(function (res) {
                      _this25.IsFilterLoaded = true;
                      var input;

                      if (res || res == "") {
                        if (res != "") {
                          input = JSON.parse(res);
                          data = input;
                        }

                        if (isTopFilter) {
                          data['IsShowCarrierManaged'] = _this25.isShowCarrierManaged;
                          data['Carrier'] = _this25.SelectedCarrierList;
                          data['toogleMap'] = _this25.toogleMap;
                          data['isShowNonRetailJobs'] = _this25.isShowNonRetailJobs;
                          data['singleMulti'] = _this25.singleMulti;
                        } else {
                          data['Regions'] = _this25.SelectedRegions;
                          data['Customer'] = _this25.SelectedCustomerList;
                          data['Location'] = _this25.SelectedlocationList;
                          data['Priority'] = _this25.SelectedPriorityList;
                          data['Status'] = _this25.SelectedStatusList;
                          data['selectedLocAttributeList'] = _this25.selectedLocAttributeList;
                          data['IsShowCarrierManaged'] = _this25.isShowCarrierManaged;
                          data['Carrier'] = _this25.SelectedCarrierList;
                          data['toogleMap'] = _this25.toogleMap;
                          data['isShowNonRetailJobs'] = _this25.isShowNonRetailJobs;
                          data['singleMulti'] = _this25.singleMulti;
                        }

                        _this25.dispatcherService.postFiltersData(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].SupplierWallyboardLocation, JSON.stringify(data)).subscribe();
                      }
                    });

                  case 2:
                  case "end":
                    return _context.stop();
                }
              }
            }, _callee, this);
          }));
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this26 = this;

          this.isLoading = true;
          this.dispatcherService.getFilters(src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].SupplierWallyboardLocation).subscribe(function (res) {
            _this26.IsFilterLoaded = true;

            if (res) {
              _this26.SetFilters(JSON.parse(res));
            } else {
              _this26.getAllCustomers();

              _this26.toogleMap = true;
            }

            _this26.isLoading = false;
          });
        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          this.singleMulti = input.singleMulti == "" ? 1 : input.singleMulti;

          if (this.isShowCarrierManaged != input.IsShowCarrierManaged) {
            this.isShowCarrierManaged = input.IsShowCarrierManaged;
          }

          if (this.isShowNonRetailJobs != input.isShowNonRetailJobs) {
            this.isShowNonRetailJobs = input.isShowNonRetailJobs;
          }

          if (this.toogleMap != input.toogleMap) {
            this.toogleMap = input.toogleMap;
          }

          if (input.Carrier && input.Carrier.length > 0) {
            this.SelectedCarrierList = input.Carrier || [];
          }

          if (input.Regions && input.Regions.length > 0) {
            this.SelectedRegions = input.Regions || []; // var ids = [];
            // this.SelectedRegions.forEach(res => { ids.push(res.Id) });
            // this.SelectedRegionId = ids.join();
          }

          if (input.Customer && input.Customer.length > 0) {
            this.SelectedCustomerList = input.Customer || [];
          }

          if (input.Location && input.Location.length > 0) {
            this.SelectedlocationList = input.Location || [];
          }

          if (input.Priority && input.Priority.length > 0) {
            this.SelectedPriorityList = input.Priority || [];
          }

          if (input.Status && input.Status.length > 0) {
            this.SelectedStatusList = input.Status || [];
          }

          if (input.selectedLocAttributeList && input.selectedLocAttributeList.length > 0) {
            this.selectedLocAttributeList = input.selectedLocAttributeList || [];
          }

          this.ApplyFilters();
          this.getAllCustomers();
        }
      }, {
        key: "getJobIdsForMapEventHandler",
        value: function getJobIdsForMapEventHandler(valueEmitted) {
          this.jobIdsEmittedFromSalesComponent = valueEmitted;

          if (this.toogleMap) {
            this.fetchJobLocationData();
          }
        }
      }]);

      return LocationComponent;
    }();

    LocationComponent.ɵfac = function LocationComponent_Factory(t) {
      return new (t || LocationComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_11__["CarrierService"]));
    };

    LocationComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: LocationComponent,
      selectors: [["app-location"]],
      viewQuery: function LocationComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.locationGridView = _t.first);
        }
      },
      inputs: {
        singleMulti: "singleMulti"
      },
      decls: 36,
      vars: 22,
      consts: [[1, "col-sm-9", "sticky-header-loc"], [1, "row"], [1, "col"], [1, "form-check", "form-check-inline", "fs14", "mt5"], ["type", "checkbox", "id", "inlineCarrierManaged", "name", "IsShowCarrierManaged", 1, "form-check-input", 3, "ngModel", "ngModelChange", "change"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["type", "checkbox", "id", "inlineShowAsset", "name", "IsShowAssetJobs", 1, "form-check-input", 3, "ngModel", "ngModelChange"], ["for", "inlineShowAsset", 1, "form-check-label"], ["class", "mtm10", 4, "ngIf"], [1, "col-3", "pt5"], [1, "col-3", "pl0", "text-right", "pt8"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], [1, "hide_show_map", "fs14", "dib", "mr10", 3, "click"], [1, "fas", "fa-eye", "mr5"], [1, "pr"], ["class", "pa top0 bg-white left0 z-index5 loading-wrapper", 4, "ngIf"], ["class", "animated clearboth mt60 row", 3, "ngClass", 4, "ngIf"], [1, "row", 3, "ngClass"], [1, "col-sm-12"], [3, "SelectedRegions", "SelectedCustomers", "IsFilterLoaded", "SelectedLocations", "SelectedPriorities", "SelectedCarriers", "IsShowCarrierManaged", "SelectedStatus", "IsShowRetailJobs", "selectedLocAttributeList", "getJobIdsForMap"], ["assetDetailsModal", ""], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["assetsContentTemplate", ""], ["popContent", ""], [1, "mtm10"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "p-0"], [1, "fs14", 3, "ngModel", "placeholder", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], ["selectedCarrier", ""], [1, "circle-badge"], [1, "pa", "top0", "bg-white", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [1, "animated", "clearboth", "mt60", "row", 3, "ngClass"], [3, "ngClass"], ["id", "mapLegend", 2, "z-index", "1", "position", "absolute", "bottom", "0", "left", "10px", "font-size", "11px"], ["id", "status-legends", 1, "well", "pa0"], [1, "border-b"], [1, "db", "pl5", "pr5", "pt8", "pb5", "radius-10", "no-b-radius"], ["data-statusid", "11", 3, "src"], [1, "db", "pa5"], ["data-statusid", "12", 3, "src"], ["data-statusid", "1", 3, "src"], [3, "zoom", "maxZoom", "minZoom", "mapTypeControl", "fullscreenControl", "fullscreenControlOptions", "mapReady"], [4, "ngFor", "ngForOf"], ["class", "col-sm-4 pl0 right_side_panel", 4, "ngIf"], [3, "latitude", "longitude", "iconUrl", "mouseOver", "mouseOut", "markerClick"], [3, "disableAutoPan"], ["infoWindow", ""], [1, "col-sm-4", "pl0", "right_side_panel"], [1, "dib", "full-width", "pr", "well", "pa15", "pt10"], ["class", "color-maroon pull-left", 4, "ngIf"], [1, "pull-right", 3, "click"], [1, "far", "fa-times-circle", "fa-lg"], ["class", "img-responsive", 3, "src", 4, "ngIf"], [1, "col-sm-12", "driver_details"], [1, "job-location"], [1, "mb0"], [1, "address1"], [1, "fas", "fa-briefcase"], [1, "far", "fa-building"], [1, "fas", "fa-map-marker-alt"], [1, "site-status", "fs12", "mt5"], [1, "panel-group"], [1, "panel", "panel-default"], [1, "panel-heading"], ["data-toggle", "collapse", "href", "#collapse1"], [1, "f-bold"], ["class", "status  ml10", 4, "ngIf"], [1, "timing", "ml10"], ["data-toggle", "collapse", "href", "#collapse1", 1, "pull-right"], [1, "fas", "collapse1_icon", "fa-2x", "line-height_18", "fa-angle-down"], ["id", "collapse1", 1, "panel-collapse", "collapse"], [1, "panel-body"], ["class", "date_time", 4, "ngFor", "ngForOf"], [4, "ngIf"], [1, "site-instruction", "fs12", "mb5"], [1, "f-bold", "db", "mb0"], [1, "instruction", "opacity8"], [1, "site-contacts", "fs12", "row", "mb5"], [1, "col-sm-3"], [1, "col-sm-9"], [1, "mb0", "opacity8"], [1, "col-sm-12", "site-assets"], [1, "btn", "btn-default", "pull-left", "ml0", "fs12", 3, "click"], [1, "btn", "btn-default", "pull-left", "fs12", 3, "click"], ["class", "assets-panel dib full-width pr well pa15 pt10", 4, "ngIf"], ["class", "charts-panel dib full-width pr well pa15 pt10 z-index10", 4, "ngIf"], [1, "color-maroon", "pull-left"], [1, "fas", "fa-image", "mr5"], [1, "img-responsive", 3, "src"], [1, "status", "ml10"], [1, "date_time"], [1, "day", "ml10"], [1, "status", "ml10", "text-success"], [1, "assets-panel", "dib", "full-width", "pr", "well", "pa15", "pt10"], [1, "assets-header"], [1, "row", "mb5"], [1, "pull-left", 3, "click"], [1, "fas", "fa-arrow-left"], [1, "charts-panel", "dib", "full-width", "pr", "well", "pa15", "pt10", "z-index10"], [1, "charts-body"], [2, "width", "100%", "height", "50vh"], [2, "width", "100%"], [3, "data", 4, "ngIf"], [3, "data"], ["id", "assetDetailsModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "assetDetailsModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt10", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body", 2, "max-height", "80vh", "overflow-y", "scroll"], ["class", "assets-header", 4, "ngIf"], [1, "aseets-body", "assets_modal"], [1, "nav", "nav-tabs"], [1, "tab-content", "pa0"], ["id", "assets1", 1, "tab-pane", "fade", "in", "active", "animated", "fadeIn"], [1, "row", "mb10", "mt10"], [1, "mt0", "mb0", "pull-left", "fs16"], ["class", "mt0 mb0 pull-left fs16", 4, "ngIf"], ["target", "_blank", "download", "", "class", "pull-right", 3, "href", 4, "ngIf"], ["class", "border radius-5 pa15 mb20", 4, "ngIf"], ["class", "assets-id", 4, "ngIf"], [3, "click"], ["target", "_blank", "download", "", 1, "pull-right", 3, "href"], ["aria-hidden", "true", 1, "fa", "fa-download", "mr5"], [1, "border", "radius-5", "pa15", "mb20"], ["width", "100%", 1, "table", "table-condensed", "table-bordered", "table-hover", "small-table", "mb0", "mt10", "fs12"], [1, "border", "radius-5", "pa15", "tank-panel", "mb20"], [1, "tank_dip_chart", "text-center", "mt10"], ["id", "ShouldBeEmptyPercent", 1, "color-green", 3, "ngStyle"], ["id", "ShouldBeFilledPercent", 1, "color-green", 3, "ngStyle"], ["id", "CurrentInventoryPercent", 1, "red-bg", 3, "ngStyle"], ["width", "50%", 1, "f-bold"], [1, "table", "table-condensed", "table-hover", "table-bordered", "small-table"], [4, "ngIf", "ngIfElse"], ["reading", ""], [1, "assets-id"], ["style", "width: 100%;max-height:320px", 4, "ngIf"], ["class", "alert alert-danger", 4, "ngIf"], [2, "width", "100%", "max-height", "320px"], ["baseChart", "", "height", "300", 2, "margin", "auto", 3, "chartType", "datasets", "options", "labels", "legend"], [1, "alert", "alert-danger"], [1, "popover-details"], [1, "row", "border-bottom-2"], [1, "col-6", "pr-0"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "col-6"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange", "onSelect", "onDeSelect"], [1, "row", "border-bottom-2", "mt10"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange"], [3, "ngModel", "placeholder", "settings", "data", "ngModelChange"], ["selectedPriority", ""], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"]],
      template: function LocationComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r97 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_Template_input_ngModelChange_4_listener($event) {
            return ctx.isShowCarrierManaged = $event;
          })("change", function LocationComponent_Template_input_change_4_listener() {
            return ctx.ShowCarrierMangedData();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationComponent_Template_input_ngModelChange_8_listener($event) {
            return ctx.isShowNonRetailJobs = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, " Show Locations with Assets");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, LocationComponent_div_11_Template, 5, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "a", 11, 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_Template_a_click_14_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r97);

            var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](15);

            return _r1.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "i", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, LocationComponent_span_17_Template, 2, 1, "span", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "a", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationComponent_Template_a_click_19_listener() {
            return ctx.toggleMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](20, "i", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, LocationComponent_div_23_Template, 2, 0, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, LocationComponent_div_24_Template, 24, 17, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "app-location-view", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("getJobIdsForMap", function LocationComponent_Template_app_location_view_getJobIdsForMap_27_listener($event) {
            return ctx.getJobIdsForMapEventHandler($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Loading... ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, LocationComponent_ng_template_29_Template, 10, 7, "ng-template", null, 23, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](31, LocationComponent_ng_container_31_Template, 1, 0, "ng-container", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](32, LocationComponent_ng_template_32_Template, 16, 8, "ng-template", null, 25, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, LocationComponent_ng_template_34_Template, 42, 24, "ng-template", null, 26, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);
        }

        if (rf & 2) {
          var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](30);

          var _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.isShowCarrierManaged);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.isShowNonRetailJobs);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isShowCarrierManaged);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r10)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.count > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.toogleMap == true ? "Hide Map View" : "Show Map View");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.toogleMap);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", ctx.toogleMap ? "mt20" : "mt60");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("SelectedRegions", ctx.SelectedRegions)("SelectedCustomers", ctx.SelectedCustomerList)("IsFilterLoaded", ctx.IsFilterLoaded)("SelectedLocations", ctx.SelectedlocationList)("SelectedPriorities", ctx.SelectedPriorityList)("SelectedCarriers", ctx.SelectedCarrierList)("IsShowCarrierManaged", ctx.isShowCarrierManaged)("SelectedStatus", ctx.SelectedStatusList)("IsShowRetailJobs", !ctx.isShowNonRetailJobs)("selectedLocAttributeList", ctx.selectedLocAttributeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r5)("ngTemplateOutletContext", ctx.assetsModal);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_12__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgModel"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbPopover"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgTemplateOutlet"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_15__["MultiSelectComponent"], _agm_core__WEBPACK_IMPORTED_MODULE_16__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], _agm_core__WEBPACK_IMPORTED_MODULE_16__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_16__["AgmInfoWindow"], _shared_components_demand_capture_chart_demand_capture_chart_component__WEBPACK_IMPORTED_MODULE_17__["DemandCaptureChartComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgStyle"], ng2_charts__WEBPACK_IMPORTED_MODULE_18__["BaseChartDirective"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["AsyncPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["DecimalPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["DatePipe"]],
      styles: ["/*Moved map css to dispatcher-dashboard.component.css*/\n/*Scss code start here*/\n.locationfilter-in-map {\n  width: 90%;\n}\n.sticky-header-loc {\n  position: fixed;\n  right: 0;\n  padding: 15px 5px;\n  top: 45px;\n  height: 65px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n}\n.locationfilter {\n  width: 90% !important;\n  position: absolute;\n  right: 5px;\n  border-radius: 5px;\n  font-size: 14px;\n}\n.carrier-managed-filter {\n  width: 75%;\n  position: absolute;\n  left: 15px;\n  border-radius: 5px;\n  font-size: 14px;\n}\n.right_side_panel {\n  padding: 0 10px 10px;\n}\n.right_side_panel .close_btn {\n  right: 10px;\n  top: 5px;\n}\n.right_side_panel #myCarousel img {\n  max-height: 150px;\n  width: 100%;\n}\n.right_side_panel img {\n  max-height: 25vh !important;\n  margin: auto !important;\n}\n.right_side_panel .driver_details {\n  margin-top: 10px;\n}\n.right_side_panel .driver_details .site-status .panel-default {\n  background-color: white;\n  border: 0;\n}\n.right_side_panel .driver_details .site-status .panel-heading {\n  padding: 5px 0px;\n  background-color: white;\n  border: 0;\n}\n.right_side_panel .driver_details .site-status .panel-heading a {\n  color: #000000;\n}\n.right_side_panel .driver_details .site-status .panel-body {\n  padding: 5px 5px;\n}\n.right_side_panel .driver_details .site-status .panel-group {\n  margin-bottom: 0px;\n}\n.right_side_panel .driver_details .site-status .date_time {\n  padding: 5px;\n}\n.right_side_panel .assets-panel {\n  position: absolute;\n  top: 0;\n  left: 0;\n  background: #ffffff;\n  width: 100%;\n}\n.right_side_panel .aseets-body .tab-content {\n  max-height: 310px;\n  overflow-y: auto;\n  overflow-x: hidden;\n}\n.right_side_panel .charts-panel {\n  position: absolute;\n  top: 0;\n  left: 0;\n  background: #ffffff;\n  width: 100%;\n}\n.right_side_panel .charts-panel .charts-header {\n  padding: 10px 5px;\n}\n.right_side_panel .charts-panel .charts-body {\n  max-height: 340px;\n  overflow-y: auto;\n  overflow-x: hidden;\n}\n#dispatcher-datatable .bg_noDlr_go {\n  background-color: #e5e5e5;\n}\n.assets-panel .aseets-body .nav-tabs {\n  overflow-x: auto;\n  overflow-y: hidden;\n  display: -webkit-box;\n  display: -moz-box;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar-thumb {\n  background-color: #e3e3e3;\n  border-radius: 5px;\n  opacity: 0.2;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar-track {\n  background-color: transparent;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar {\n  width: 6px;\n  height: 4px;\n  overflow: scroll;\n  background-color: inherit;\n  border-radius: 5px;\n}\n.assets-panel .aseets-body .nav-tabs > li {\n  float: none;\n  padding-bottom: 5px;\n}\n.aseets-body .nav > li.active {\n  background: none !important;\n}\n.aseets-body .nav > li.active > a {\n  padding: 2px 0 !important;\n  border-width: 0 !important;\n  border-bottom: 2px solid #0c52b1 !important;\n  color: #0c52b1;\n}\n.aseets-body .nav > li > a {\n  padding: 2px 0 !important;\n  border-width: 0 !important;\n  border-bottom: 2px solid #fff !important;\n  margin-right: 8px;\n}\n.aseets-body .nav > li > a:hover {\n  border-width: 0 !important;\n  border-bottom: 2px solid #0c52b1 !important;\n  background: none !important;\n  color: #848484;\n}\n.aseets-body .nav-tabs {\n  border-bottom: 0;\n}\n.show_filter {\n  width: 100%;\n  background: 0 0;\n  position: relative;\n  top: 0px;\n  left: 0px;\n  border-radius: 5px;\n  opacity: 1;\n  margin-top: 5px;\n}\n#assetDetailsModal .modal-header {\n  padding: 5px 15px;\n  border-bottom: 1px solid #e5e5e5;\n}\n.asset-details td {\n  padding: 4px 8px;\n}\ntable.dataTable.fixedHeader-floating {\n  top: 17px !important;\n  top: 65px !important;\n}\n.display_hide {\n  display: none;\n  transition: opacity 1s ease-out;\n  opacity: 0;\n}\ntable.dataTable.fixedHeader-locked {\n  position: fixed !important;\n  top: 65px !important;\n}\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n.carrier-popover.popover .popover-body {\n  padding: 10px;\n  border-radius: 5px;\n}\n.multiselect-dropdown {\n  font-size: 12px !important;\n}\n.activediff-dot {\n  height: 10px;\n  width: 10px;\n  background-color: #585bff;\n  border-radius: 50%;\n  display: inline-block;\n  -webkit-animation: 1s blink ease infinite;\n          animation: 1s blink ease infinite;\n}\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9EOlxcVEZTY29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2VcXFNpdGVGdWVsLkV4Y2hhbmdlLlNvdXJjZUNvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlLldlYi9zcmNcXGFwcFxcZGlzcGF0Y2hlclxcZGlzcGF0Y2hlci1kYXNoYm9hcmRcXGxvY2F0aW9uLmNvbXBvbmVudC5zY3NzIiwic3JjL2FwcC9kaXNwYXRjaGVyL2Rpc3BhdGNoZXItZGFzaGJvYXJkL2xvY2F0aW9uLmNvbXBvbmVudC5zY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUNBLHNEQUFBO0FBa1BBLHVCQUFBO0FBQ0E7RUFDQyxVQUFBO0FDalBEO0FEbVBBO0VBQ0MsZUFBQTtFQUNBLFFBQUE7RUFDQSxpQkFBQTtFQUNBLFNBQUE7RUFDQSxZQUFBO0VBQ0EsZUFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtBQ2hQRDtBRGtQQTtFQUNDLHFCQUFBO0VBQ0Esa0JBQUE7RUFDQSxVQUFBO0VBQ0Esa0JBQUE7RUFDQSxlQUFBO0FDL09EO0FEaVBBO0VBQ0MsVUFBQTtFQUNBLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtBQzlPRDtBRGdQQTtFQUNDLG9CQUFBO0FDN09EO0FEOE9DO0VBQ0MsV0FBQTtFQUNBLFFBQUE7QUM1T0Y7QUQrT0U7RUFDQyxpQkFBQTtFQUNBLFdBQUE7QUM3T0g7QURnUEM7RUFDQywyQkFBQTtFQUNBLHVCQUFBO0FDOU9GO0FEZ1BDO0VBQ0MsZ0JBQUE7QUM5T0Y7QURnUEc7RUFDQyx1QkFBQTtFQUNBLFNBQUE7QUM5T0o7QURnUEc7RUFDQyxnQkFBQTtFQUNBLHVCQUFBO0VBQ0EsU0FBQTtBQzlPSjtBRCtPSTtFQUNDLGNBQUE7QUM3T0w7QURnUEc7RUFDQyxnQkFBQTtBQzlPSjtBRGdQRztFQUNDLGtCQUFBO0FDOU9KO0FEZ1BHO0VBQ0MsWUFBQTtBQzlPSjtBRGtQQztFQUNDLGtCQUFBO0VBQ0EsTUFBQTtFQUNBLE9BQUE7RUFDQSxtQkFBQTtFQUNBLFdBQUE7QUNoUEY7QURtUEU7RUFDQyxpQkFBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7QUNqUEg7QURvUEM7RUFDQyxrQkFBQTtFQUNBLE1BQUE7RUFDQSxPQUFBO0VBQ0EsbUJBQUE7RUFDQSxXQUFBO0FDbFBGO0FEbVBFO0VBQ0MsaUJBQUE7QUNqUEg7QURtUEU7RUFDQyxpQkFBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7QUNqUEg7QURzUEM7RUFDQyx5QkFBQTtBQ25QRjtBRHdQRTtFQUNDLGdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxvQkFBQTtFQUNBLGlCQUFBO0FDclBIO0FEc1BHO0VBQ0MseUJBQUE7RUFDQSxrQkFBQTtFQUNBLFlBQUE7QUNwUEo7QURzUEc7RUFDQyw2QkFBQTtBQ3BQSjtBRHNQRztFQUNDLFVBQUE7RUFDQSxXQUFBO0VBQ0EsZ0JBQUE7RUFDQSx5QkFBQTtFQUNBLGtCQUFBO0FDcFBKO0FEc1BHO0VBQ0MsV0FBQTtFQUNBLG1CQUFBO0FDcFBKO0FEMlBFO0VBQ0MsMkJBQUE7QUN4UEg7QUR5UEc7RUFDQyx5QkFBQTtFQUNBLDBCQUFBO0VBQ0EsMkNBQUE7RUFDQSxjQUFBO0FDdlBKO0FEMlBHO0VBQ0MseUJBQUE7RUFDQSwwQkFBQTtFQUNBLHdDQUFBO0VBQ0EsaUJBQUE7QUN6UEo7QUQwUEk7RUFDQywwQkFBQTtFQUNBLDJDQUFBO0VBQ0EsMkJBQUE7RUFDQSxjQUFBO0FDeFBMO0FENlBDO0VBQ0MsZ0JBQUE7QUMzUEY7QUQ4UEE7RUFDQyxXQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsUUFBQTtFQUNBLFNBQUE7RUFDQSxrQkFBQTtFQUNBLFVBQUE7RUFDQSxlQUFBO0FDM1BEO0FEOFBDO0VBQ0MsaUJBQUE7RUFDQSxnQ0FBQTtBQzNQRjtBRCtQQztFQUNDLGdCQUFBO0FDNVBGO0FEK1BBO0VBQ0Msb0JBQUE7RUFDQSxvQkFBQTtBQzVQRDtBRDhQQTtFQUNDLGFBQUE7RUFDQSwrQkFBQTtFQUNBLFVBQUE7QUMzUEQ7QUQ2UEE7RUFDQywwQkFBQTtFQUNBLG9CQUFBO0FDMVBEO0FENFBBO0VBQ0MsZ0JBQUE7RUFDQSxnQkFBQTtFQUNBLG1CQUFBO0VBQ0EseUJBQUE7RUFDQSxzQkFBQTtFQUNBLGtEQUFBO0VBQ0EsbUJBQUE7QUN6UEQ7QUQwUEM7RUFDQyxhQUFBO0VBQ0Esa0JBQUE7QUN4UEY7QUQyUEE7RUFDQywwQkFBQTtBQ3hQRDtBRDBQQTtFQUNDLFlBQUE7RUFDQSxXQUFBO0VBQ0EseUJBQUE7RUFDQSxrQkFBQTtFQUNBLHFCQUFBO0VBQ0EseUNBQUE7VUFBQSxpQ0FBQTtBQ3ZQRDtBRDRQSTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDelBSO0FEMFBRO0VBSUksVUFBQTtFQUNBLGtCQUFBO0VBQ1QsbUJBQUE7QUMzUEg7QUQ2UFE7RUFDSSxhQUFBO0FDM1BaO0FEK1BHO0VBQ0MsMkJBQUE7QUM3UEo7QURpUUU7RUFDQywyQ0FBQTtBQy9QSDtBRG9RQTtFQUNDLGtCQUFBO0VBQ0csVUFBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtFQUNBLG9CQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtFQUNILFdBQUE7RUFDRyxZQUFBO0FDalFKIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9sb2NhdGlvbi5jb21wb25lbnQuc2NzcyIsInNvdXJjZXNDb250ZW50IjpbIlxyXG4vKk1vdmVkIG1hcCBjc3MgdG8gZGlzcGF0Y2hlci1kYXNoYm9hcmQuY29tcG9uZW50LmNzcyovXHJcbi8vIC5sb2NhdGlvbmZpbHRlci1pbi1tYXAge1xyXG4vLyAgICAgd2lkdGg6IDkwJTtcclxuLy8gfVxyXG5cclxuXHJcbi8vIC5zdGlja3ktaGVhZGVyLWxvYyB7XHJcbi8vICAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbi8vICAgICByaWdodDogMDtcclxuLy8gICAgIHBhZGRpbmc6IDE1cHggNXB4O1xyXG4vLyAgICAgdG9wOiA0NXB4O1xyXG4vLyAgICAgaGVpZ2h0OiA2NXB4O1xyXG4vLyAgICAgZm9udC1zaXplOiAyMHB4O1xyXG4vLyAgICAgei1pbmRleDogMTA7XHJcbi8vICAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG4vLyB9XHJcblxyXG4vLyAubG9jYXRpb25maWx0ZXIge1xyXG4vLyAgICAgd2lkdGg6IDkwJSAhaW1wb3J0YW50O1xyXG4vLyAgICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4vLyAgICAgcmlnaHQ6IDVweDtcclxuLy8gICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuLy8gICAgIGZvbnQtc2l6ZTogMTRweDtcclxuLy8gfVxyXG4vLyAuY2Fycmllci1tYW5hZ2VkLWZpbHRlciB7XHJcbi8vICAgICB3aWR0aDogNzUlO1xyXG4vLyAgICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4vLyAgICAgbGVmdDogMTVweDtcclxuLy8gICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuLy8gICAgIGZvbnQtc2l6ZTogMTRweDtcclxuLy8gfVxyXG5cclxuLy8gLnJpZ2h0X3NpZGVfcGFuZWwge1xyXG4vLyAgICAgcGFkZGluZzogMCAxMHB4IDEwcHg7XHJcbi8vIH1cclxuXHJcbi8vICAgICAucmlnaHRfc2lkZV9wYW5lbCAuY2xvc2VfYnRuIHtcclxuLy8gICAgICAgICByaWdodDogMTBweDtcclxuLy8gICAgICAgICB0b3A6IDVweDtcclxuLy8gICAgIH1cclxuXHJcbi8vICAgICAucmlnaHRfc2lkZV9wYW5lbCAjbXlDYXJvdXNlbCBpbWcge1xyXG4vLyAgICAgICAgIG1heC1oZWlnaHQ6IDE1MHB4O1xyXG4vLyAgICAgICAgIHdpZHRoOiAxMDAlO1xyXG4vLyAgICAgfVxyXG5cclxuLy8gICAgIC5yaWdodF9zaWRlX3BhbmVsIGltZyB7XHJcbi8vICAgICAgICAgbWF4LWhlaWdodDogMjV2aCAhaW1wb3J0YW50O1xyXG4vLyAgICAgICAgIG1hcmdpbjogYXV0byAhaW1wb3J0YW50O1xyXG4vLyAgICAgfVxyXG5cclxuLy8gICAgIC5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyB7XHJcbi8vICAgICAgICAgbWFyZ2luLXRvcDogMTBweDtcclxuLy8gICAgIH1cclxuXHJcbi8vICAgICAgICAgLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtZGVmYXVsdCB7XHJcbi8vICAgICAgICAgICAgIGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xyXG4vLyAgICAgICAgICAgICBib3JkZXI6IDA7XHJcbi8vICAgICAgICAgfVxyXG5cclxuLy8gICAgICAgICAucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1oZWFkaW5nIHtcclxuLy8gICAgICAgICAgICAgcGFkZGluZzogNXB4IDBweDtcclxuLy8gICAgICAgICAgICAgYmFja2dyb3VuZC1jb2xvcjogd2hpdGU7XHJcbi8vICAgICAgICAgICAgIGJvcmRlcjogMDtcclxuLy8gICAgICAgICB9XHJcblxyXG4vLyAgICAgICAgIC5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLnBhbmVsLWJvZHkge1xyXG4vLyAgICAgICAgICAgICBwYWRkaW5nOiA1cHggNXB4O1xyXG4vLyAgICAgICAgIH1cclxuXHJcbi8vICAgICAgICAgLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtZ3JvdXAge1xyXG4vLyAgICAgICAgICAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbi8vICAgICAgICAgfVxyXG5cclxuLy8gICAgICAgICAucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1oZWFkaW5nIGEge1xyXG4vLyAgICAgICAgICAgICBjb2xvcjogIzAwMDAwMDtcclxuLy8gICAgICAgICB9XHJcblxyXG4vLyAjZGlzcGF0Y2hlci1kYXRhdGFibGUgLmJnX25vRGxyX2dvIHtcclxuLy8gICAgIGJhY2tncm91bmQtY29sb3I6ICNlNWU1ZTU7XHJcbi8vIH1cclxuXHJcbi8vIC5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLmRhdGVfdGltZSB7XHJcbi8vICAgICBwYWRkaW5nOiA1cHg7XHJcbi8vIH1cclxuXHJcbi8vIC5yaWdodF9zaWRlX3BhbmVsIC5hc3NldHMtcGFuZWwge1xyXG4vLyAgICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4vLyAgICAgdG9wOiAwO1xyXG4vLyAgICAgbGVmdDogMDtcclxuLy8gICAgIGJhY2tncm91bmQ6ICNmZmZmZmY7XHJcbi8vICAgICB3aWR0aDogMTAwJTtcclxuLy8gICAgIC8qaGVpZ2h0OiAxMDAlOyovXHJcbi8vIH1cclxuXHJcbi8vIC5hc3NldHMtcGFuZWwgIC5hc2VldHMtYm9keSAubmF2LXRhYnMge1xyXG4vLyAgICAgb3ZlcmZsb3cteDogYXV0bztcclxuLy8gICAgIG92ZXJmbG93LXk6IGhpZGRlbjtcclxuLy8gICAgIGRpc3BsYXk6IC13ZWJraXQtYm94O1xyXG4vLyAgICAgZGlzcGxheTogLW1vei1ib3g7XHJcbi8vIH1cclxuXHJcbi8vICAgICAuYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyLXRodW1iIHtcclxuLy8gICAgICAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZTNlM2UzO1xyXG4vLyAgICAgICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuLy8gICAgICAgICBvcGFjaXR5OiAuMjtcclxuLy8gICAgIH1cclxuXHJcbi8vICAgICAuYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyLXRyYWNrIHtcclxuLy8gICAgICAgICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcclxuLy8gICAgIH1cclxuXHJcbi8vICAgICAuYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyIHtcclxuLy8gICAgICAgICB3aWR0aDogNnB4O1xyXG4vLyAgICAgICAgIGhlaWdodDogNHB4O1xyXG4vLyAgICAgICAgIG92ZXJmbG93OiBzY3JvbGw7XHJcbi8vICAgICAgICAgYmFja2dyb3VuZC1jb2xvcjogaW5oZXJpdDtcclxuLy8gICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbi8vICAgICB9XHJcblxyXG4vLyAgICAgLmFzc2V0cy1wYW5lbCAuYXNlZXRzLWJvZHkgLm5hdi10YWJzID4gbGkge1xyXG4vLyAgICAgICAgIGZsb2F0OiBub25lO1xyXG4vLyAgICAgICAgIHBhZGRpbmctYm90dG9tOjVweDtcclxuLy8gICAgIH1cclxuLy8gLmFzZWV0cy1ib2R5IC5uYXYgPiBsaS5hY3RpdmUge1xyXG4vLyAgICAgYmFja2dyb3VuZDogbm9uZSAhaW1wb3J0YW50O1xyXG4vLyB9XHJcbi8vIC5hc2VldHMtYm9keSAubmF2ID4gbGkgPiBhIHtcclxuLy8gICAgIHBhZGRpbmc6IDJweCAwICFpbXBvcnRhbnQ7XHJcbi8vICAgICBib3JkZXItd2lkdGg6IDAgIWltcG9ydGFudDtcclxuLy8gICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZmZmICFpbXBvcnRhbnQ7XHJcbi8vICAgICBtYXJnaW4tcmlnaHQ6IDhweDtcclxuLy8gfVxyXG4vLyAuYXNlZXRzLWJvZHkgLm5hdiA+IGxpLmFjdGl2ZSA+IGEge1xyXG4vLyAgICAgcGFkZGluZzogMnB4IDAgIWltcG9ydGFudDtcclxuLy8gICAgIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xyXG4vLyAgICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcclxuLy8gICAgIGNvbG9yOiAjMGM1MmIxO1xyXG4vLyB9XHJcbi8vIC5hc2VldHMtYm9keSAubmF2ID4gbGkgPiBhOmhvdmVyIHtcclxuLy8gICAgIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xyXG4vLyAgICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcclxuLy8gICAgIGJhY2tncm91bmQ6IG5vbmUgIWltcG9ydGFudDtcclxuLy8gICAgIGNvbG9yOiAjODQ4NDg0O1xyXG4vLyB9XHJcblxyXG5cclxuLy8gLnJpZ2h0X3NpZGVfcGFuZWwgLmFzZWV0cy1ib2R5IC50YWItY29udGVudCB7XHJcbi8vICAgICBtYXgtaGVpZ2h0OiAzMTBweDtcclxuLy8gICAgIG92ZXJmbG93LXk6IGF1dG87XHJcbi8vICAgICBvdmVyZmxvdy14OiBoaWRkZW47XHJcbi8vIH1cclxuLy8gLnJpZ2h0X3NpZGVfcGFuZWwgLmNoYXJ0cy1wYW5lbCB7XHJcbi8vICAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbi8vICAgICB0b3A6IDA7XHJcbi8vICAgICBsZWZ0OiAwO1xyXG4vLyAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuLy8gICAgIHdpZHRoOiAxMDAlO1xyXG4vLyAgICAgLypoZWlnaHQ6IDEwMCU7Ki9cclxuLy8gfVxyXG5cclxuLy8gICAgIC5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwgLmNoYXJ0cy1oZWFkZXIge1xyXG4vLyAgICAgICAgIHBhZGRpbmc6IDEwcHggNXB4O1xyXG4vLyAgICAgfVxyXG5cclxuLy8gICAgIC5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwgLmNoYXJ0cy1ib2R5IHtcclxuLy8gICAgICAgICBtYXgtaGVpZ2h0OiAzNDBweDtcclxuLy8gICAgICAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG4vLyAgICAgICAgIG92ZXJmbG93LXg6IGhpZGRlbjtcclxuLy8gICAgIH1cclxuXHJcbi8vIC5zaG93X2ZpbHRlciB7XHJcbi8vICAgICB3aWR0aDogMTAwJTtcclxuLy8gICAgIGJhY2tncm91bmQ6IDAgMDtcclxuLy8gICAgIHBvc2l0aW9uOiByZWxhdGl2ZTtcclxuLy8gICAgIHRvcDogMHB4O1xyXG4vLyAgICAgbGVmdDogMHB4O1xyXG4vLyAgICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4vLyAgICAgb3BhY2l0eTogMTtcclxuLy8gICAgIG1hcmdpbi10b3A6IDVweDtcclxuLy8gfVxyXG5cclxuLy8gI2Fzc2V0RGV0YWlsc01vZGFsIC5tb2RhbC1oZWFkZXIge1xyXG4vLyAgICAgcGFkZGluZzogNXB4IDE1cHg7XHJcbi8vICAgICBib3JkZXItYm90dG9tOiAxcHggc29saWQgI2U1ZTVlNTtcclxuLy8gfVxyXG5cclxuLy8gLmFzc2V0LWRldGFpbHMgdGQge1xyXG4vLyAgICAgcGFkZGluZzogNHB4IDhweDtcclxuLy8gfVxyXG5cclxuLy8gLmFzZWV0cy1ib2R5IC5uYXYtdGFicyB7XHJcbi8vICAgICBib3JkZXItYm90dG9tOiAwO1xyXG4vLyB9XHJcblxyXG4vLyB0YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcge1xyXG4vLyAgICAgdG9wOiAxN3B4ICFpbXBvcnRhbnQ7XHJcbi8vIH1cclxuXHJcbi8vIC5kaXNwbGF5X2hpZGUge1xyXG4vLyAgICAgZGlzcGxheTogbm9uZTtcclxuLy8gICAgIHRyYW5zaXRpb246IG9wYWNpdHkgMXMgZWFzZS1vdXQ7XHJcbi8vICAgICBvcGFjaXR5OiAwO1xyXG4vLyB9XHJcblxyXG4vLyB0YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItbG9ja2VkIHtcclxuLy8gICAgIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xyXG4vLyB9XHJcblxyXG4vLyB0YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcsIHRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4vLyAgICAgdG9wOiA2NXB4ICFpbXBvcnRhbnQ7XHJcbi8vIH1cclxuXHJcbi8vIC5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlcntcclxuLy8gICAgIG1pbi13aWR0aDogMzAwcHg7XHJcbi8vICAgICBtYXgtd2lkdGg6IDM1MHB4O1xyXG4vLyAgICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuLy8gICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbi8vICAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4vLyAgICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuLy8gICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbi8vIH1cclxuLy8gLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyICAucG9wb3Zlci1ib2R5e1xyXG4vLyAgICAgcGFkZGluZzogMTBweDtcclxuLy8gICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuLy8gfVxyXG5cclxuLy8gLm11bHRpc2VsZWN0LWRyb3Bkb3due1xyXG4vLyBmb250LXNpemU6IDEycHggIWltcG9ydGFudDtcclxuLy8gfVxyXG5cclxuLy8gLmFjdGl2ZWRpZmYtZG90IHtcclxuLy8gICAgIGhlaWdodDogMTBweDtcclxuLy8gICAgIHdpZHRoOiAxMHB4O1xyXG4vLyAgICAgYmFja2dyb3VuZC1jb2xvcjogIzU4NWJmZjtcclxuLy8gICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuLy8gICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuLy8gICAgIGFuaW1hdGlvbjogMXMgYmxpbmsgZWFzZSBpbmZpbml0ZTtcclxuLy8gfVxyXG5cclxuXHJcblxyXG4vKlNjc3MgY29kZSBzdGFydCBoZXJlKi9cclxuLmxvY2F0aW9uZmlsdGVyLWluLW1hcCB7XHJcblx0d2lkdGg6IDkwJTtcclxufVxyXG4uc3RpY2t5LWhlYWRlci1sb2Mge1xyXG5cdHBvc2l0aW9uOiBmaXhlZDtcclxuXHRyaWdodDogMDtcclxuXHRwYWRkaW5nOiAxNXB4IDVweDtcclxuXHR0b3A6IDQ1cHg7XHJcblx0aGVpZ2h0OiA2NXB4O1xyXG5cdGZvbnQtc2l6ZTogMjBweDtcclxuXHR6LWluZGV4OiAxMDtcclxuXHRiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcbi5sb2NhdGlvbmZpbHRlciB7XHJcblx0d2lkdGg6IDkwJSAhaW1wb3J0YW50O1xyXG5cdHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuXHRyaWdodDogNXB4O1xyXG5cdGJvcmRlci1yYWRpdXM6IDVweDtcclxuXHRmb250LXNpemU6IDE0cHg7XHJcbn1cclxuLmNhcnJpZXItbWFuYWdlZC1maWx0ZXIge1xyXG5cdHdpZHRoOiA3NSU7XHJcblx0cG9zaXRpb246IGFic29sdXRlO1xyXG5cdGxlZnQ6IDE1cHg7XHJcblx0Ym9yZGVyLXJhZGl1czogNXB4O1xyXG5cdGZvbnQtc2l6ZTogMTRweDtcclxufVxyXG4ucmlnaHRfc2lkZV9wYW5lbCB7XHJcblx0cGFkZGluZzogMCAxMHB4IDEwcHg7XHJcblx0LmNsb3NlX2J0biB7XHJcblx0XHRyaWdodDogMTBweDtcclxuXHRcdHRvcDogNXB4O1xyXG5cdH1cclxuXHQjbXlDYXJvdXNlbCB7XHJcblx0XHRpbWcge1xyXG5cdFx0XHRtYXgtaGVpZ2h0OiAxNTBweDtcclxuXHRcdFx0d2lkdGg6IDEwMCU7XHJcblx0XHR9XHJcblx0fVxyXG5cdGltZyB7XHJcblx0XHRtYXgtaGVpZ2h0OiAyNXZoICFpbXBvcnRhbnQ7XHJcblx0XHRtYXJnaW46IGF1dG8gIWltcG9ydGFudDtcclxuXHR9XHJcblx0LmRyaXZlcl9kZXRhaWxzIHtcclxuXHRcdG1hcmdpbi10b3A6IDEwcHg7XHJcblx0XHQuc2l0ZS1zdGF0dXMge1xyXG5cdFx0XHQucGFuZWwtZGVmYXVsdCB7XHJcblx0XHRcdFx0YmFja2dyb3VuZC1jb2xvcjogd2hpdGU7XHJcblx0XHRcdFx0Ym9yZGVyOiAwO1xyXG5cdFx0XHR9XHJcblx0XHRcdC5wYW5lbC1oZWFkaW5nIHtcclxuXHRcdFx0XHRwYWRkaW5nOiA1cHggMHB4O1xyXG5cdFx0XHRcdGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xyXG5cdFx0XHRcdGJvcmRlcjogMDtcclxuXHRcdFx0XHRhIHtcclxuXHRcdFx0XHRcdGNvbG9yOiAjMDAwMDAwO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0XHQucGFuZWwtYm9keSB7XHJcblx0XHRcdFx0cGFkZGluZzogNXB4IDVweDtcclxuXHRcdFx0fVxyXG5cdFx0XHQucGFuZWwtZ3JvdXAge1xyXG5cdFx0XHRcdG1hcmdpbi1ib3R0b206IDBweDtcclxuXHRcdFx0fVxyXG5cdFx0XHQuZGF0ZV90aW1lIHtcclxuXHRcdFx0XHRwYWRkaW5nOiA1cHg7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9XHJcblx0LmFzc2V0cy1wYW5lbCB7XHJcblx0XHRwb3NpdGlvbjogYWJzb2x1dGU7XHJcblx0XHR0b3A6IDA7XHJcblx0XHRsZWZ0OiAwO1xyXG5cdFx0YmFja2dyb3VuZDogI2ZmZmZmZjtcclxuXHRcdHdpZHRoOiAxMDAlO1xyXG5cdH1cclxuXHQuYXNlZXRzLWJvZHkge1xyXG5cdFx0LnRhYi1jb250ZW50IHtcclxuXHRcdFx0bWF4LWhlaWdodDogMzEwcHg7XHJcblx0XHRcdG92ZXJmbG93LXk6IGF1dG87XHJcblx0XHRcdG92ZXJmbG93LXg6IGhpZGRlbjtcclxuXHRcdH1cclxuXHR9XHJcblx0LmNoYXJ0cy1wYW5lbCB7XHJcblx0XHRwb3NpdGlvbjogYWJzb2x1dGU7XHJcblx0XHR0b3A6IDA7XHJcblx0XHRsZWZ0OiAwO1xyXG5cdFx0YmFja2dyb3VuZDogI2ZmZmZmZjtcclxuXHRcdHdpZHRoOiAxMDAlO1xyXG5cdFx0LmNoYXJ0cy1oZWFkZXIge1xyXG5cdFx0XHRwYWRkaW5nOiAxMHB4IDVweDtcclxuXHRcdH1cclxuXHRcdC5jaGFydHMtYm9keSB7XHJcblx0XHRcdG1heC1oZWlnaHQ6IDM0MHB4O1xyXG5cdFx0XHRvdmVyZmxvdy15OiBhdXRvO1xyXG5cdFx0XHRvdmVyZmxvdy14OiBoaWRkZW47XHJcblx0XHR9XHJcblx0fVxyXG59XHJcbiNkaXNwYXRjaGVyLWRhdGF0YWJsZSB7XHJcblx0LmJnX25vRGxyX2dvIHtcclxuXHRcdGJhY2tncm91bmQtY29sb3I6ICNlNWU1ZTU7XHJcblx0fVxyXG59XHJcbi5hc3NldHMtcGFuZWwge1xyXG5cdC5hc2VldHMtYm9keSB7XHJcblx0XHQubmF2LXRhYnMge1xyXG5cdFx0XHRvdmVyZmxvdy14OiBhdXRvO1xyXG5cdFx0XHRvdmVyZmxvdy15OiBoaWRkZW47XHJcblx0XHRcdGRpc3BsYXk6IC13ZWJraXQtYm94O1xyXG5cdFx0XHRkaXNwbGF5OiAtbW96LWJveDtcclxuXHRcdFx0Jjo6LXdlYmtpdC1zY3JvbGxiYXItdGh1bWIge1xyXG5cdFx0XHRcdGJhY2tncm91bmQtY29sb3I6ICNlM2UzZTM7XHJcblx0XHRcdFx0Ym9yZGVyLXJhZGl1czogNXB4O1xyXG5cdFx0XHRcdG9wYWNpdHk6IC4yO1xyXG5cdFx0XHR9XHJcblx0XHRcdCY6Oi13ZWJraXQtc2Nyb2xsYmFyLXRyYWNrIHtcclxuXHRcdFx0XHRiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcclxuXHRcdFx0fVxyXG5cdFx0XHQmOjotd2Via2l0LXNjcm9sbGJhciB7XHJcblx0XHRcdFx0d2lkdGg6IDZweDtcclxuXHRcdFx0XHRoZWlnaHQ6IDRweDtcclxuXHRcdFx0XHRvdmVyZmxvdzogc2Nyb2xsO1xyXG5cdFx0XHRcdGJhY2tncm91bmQtY29sb3I6IGluaGVyaXQ7XHJcblx0XHRcdFx0Ym9yZGVyLXJhZGl1czogNXB4O1xyXG5cdFx0XHR9XHJcblx0XHRcdD5saSB7XHJcblx0XHRcdFx0ZmxvYXQ6IG5vbmU7XHJcblx0XHRcdFx0cGFkZGluZy1ib3R0b206IDVweDtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH1cclxufVxyXG4uYXNlZXRzLWJvZHkge1xyXG5cdC5uYXYge1xyXG5cdFx0PmxpLmFjdGl2ZSB7XHJcblx0XHRcdGJhY2tncm91bmQ6IG5vbmUgIWltcG9ydGFudDtcclxuXHRcdFx0PmEge1xyXG5cdFx0XHRcdHBhZGRpbmc6IDJweCAwICFpbXBvcnRhbnQ7XHJcblx0XHRcdFx0Ym9yZGVyLXdpZHRoOiAwICFpbXBvcnRhbnQ7XHJcblx0XHRcdFx0Ym9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcclxuXHRcdFx0XHRjb2xvcjogIzBjNTJiMTtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdFx0PmxpIHtcclxuXHRcdFx0PmEge1xyXG5cdFx0XHRcdHBhZGRpbmc6IDJweCAwICFpbXBvcnRhbnQ7XHJcblx0XHRcdFx0Ym9yZGVyLXdpZHRoOiAwICFpbXBvcnRhbnQ7XHJcblx0XHRcdFx0Ym9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNmZmYgIWltcG9ydGFudDtcclxuXHRcdFx0XHRtYXJnaW4tcmlnaHQ6IDhweDtcclxuXHRcdFx0XHQmOmhvdmVyIHtcclxuXHRcdFx0XHRcdGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xyXG5cdFx0XHRcdFx0Ym9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcclxuXHRcdFx0XHRcdGJhY2tncm91bmQ6IG5vbmUgIWltcG9ydGFudDtcclxuXHRcdFx0XHRcdGNvbG9yOiAjODQ4NDg0O1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH1cclxuXHQubmF2LXRhYnMge1xyXG5cdFx0Ym9yZGVyLWJvdHRvbTogMDtcclxuXHR9XHJcbn1cclxuLnNob3dfZmlsdGVyIHtcclxuXHR3aWR0aDogMTAwJTtcclxuXHRiYWNrZ3JvdW5kOiAwIDA7XHJcblx0cG9zaXRpb246IHJlbGF0aXZlO1xyXG5cdHRvcDogMHB4O1xyXG5cdGxlZnQ6IDBweDtcclxuXHRib3JkZXItcmFkaXVzOiA1cHg7XHJcblx0b3BhY2l0eTogMTtcclxuXHRtYXJnaW4tdG9wOiA1cHg7XHJcbn1cclxuI2Fzc2V0RGV0YWlsc01vZGFsIHtcclxuXHQubW9kYWwtaGVhZGVyIHtcclxuXHRcdHBhZGRpbmc6IDVweCAxNXB4O1xyXG5cdFx0Ym9yZGVyLWJvdHRvbTogMXB4IHNvbGlkICNlNWU1ZTU7XHJcblx0fVxyXG59XHJcbi5hc3NldC1kZXRhaWxzIHtcclxuXHR0ZCB7XHJcblx0XHRwYWRkaW5nOiA0cHggOHB4O1xyXG5cdH1cclxufVxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcge1xyXG5cdHRvcDogMTdweCAhaW1wb3J0YW50O1xyXG5cdHRvcDogNjVweCAhaW1wb3J0YW50O1xyXG59XHJcbi5kaXNwbGF5X2hpZGUge1xyXG5cdGRpc3BsYXk6IG5vbmU7XHJcblx0dHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcclxuXHRvcGFjaXR5OiAwO1xyXG59XHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG5cdHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xyXG5cdHRvcDogNjVweCAhaW1wb3J0YW50O1xyXG59XHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciB7XHJcblx0bWluLXdpZHRoOiAzMDBweDtcclxuXHRtYXgtd2lkdGg6IDM1MHB4O1xyXG5cdGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcblx0Ym9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuXHRib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG5cdGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcblx0Ym9yZGVyLXJhZGl1czogMTBweDtcclxuXHQucG9wb3Zlci1ib2R5IHtcclxuXHRcdHBhZGRpbmc6IDEwcHg7XHJcblx0XHRib3JkZXItcmFkaXVzOiA1cHg7XHJcblx0fVxyXG59XHJcbi5tdWx0aXNlbGVjdC1kcm9wZG93biB7XHJcblx0Zm9udC1zaXplOiAxMnB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuLmFjdGl2ZWRpZmYtZG90IHtcclxuXHRoZWlnaHQ6IDEwcHg7XHJcblx0d2lkdGg6IDEwcHg7XHJcblx0YmFja2dyb3VuZC1jb2xvcjogIzU4NWJmZjtcclxuXHRib3JkZXItcmFkaXVzOiA1MCU7XHJcblx0ZGlzcGxheTogaW5saW5lLWJsb2NrO1xyXG5cdGFuaW1hdGlvbjogMXMgYmxpbmsgZWFzZSBpbmZpbml0ZTtcclxufVxyXG5cclxuXHJcbi5tYXN0ZXItZmlsdGVyIHtcclxuICAgICYucG9wb3ZlciB7XHJcbiAgICAgICAgbWluLXdpZHRoOiA0MjVweDtcclxuICAgICAgICBtYXgtd2lkdGg6IDQ1MHB4O1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICAgICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgICAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICAgICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuICAgICAgICAucG9wb3Zlci1ib2R5IHtcclxuICAgICAgICAgICAgLy8gbWF4LWhlaWdodDogMzUwcHg7XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXg6IGhpZGRlbjtcclxuICAgICAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICAgICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG5cdFx0XHRiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgICAgIH1cclxuICAgICAgICAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICAgICAgcGFkZGluZzogMTVweDtcclxuICAgICAgICAgICAgLy8gbWF4LWhlaWdodDogMzEwcHg7XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXk6IGF1dG87XHJcblxyXG5cdFx0XHQuZm9udC1ib2xke1xyXG5cdFx0XHRcdGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcclxuXHRcdFx0fVxyXG4gICAgICAgIH1cclxuXHJcblx0XHQuYm9yZGVyLWJvdHRvbS0ye1xyXG5cdFx0XHRib3JkZXItYm90dG9tOiAycHggc29saWQgI2U3ZWFlYyAhaW1wb3J0YW50O1xyXG5cdFx0fVxyXG4gICAgfVxyXG59XHJcblxyXG4uY2lyY2xlLWJhZGdle1xyXG5cdHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogLTExcHg7XHJcbiAgICBsZWZ0OiAtMTRweDtcclxuICAgIGJhY2tncm91bmQ6IHJnYigyNTAsIDE0NywgMTQ3KTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGZvbnQtc2l6ZTogMTJweDtcclxuICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgIGNvbG9yOiB3aGl0ZTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1mbGV4O1xyXG4gICAgYWxpZ24taXRlbXM6IGNlbnRlcjtcclxuICAgIGp1c3RpZnktY29udGVudDogY2VudGVyO1xyXG5cdHdpZHRoOiAxOHB4O1xyXG4gICAgaGVpZ2h0OiAxOHB4XHJcbn0iLCIvKk1vdmVkIG1hcCBjc3MgdG8gZGlzcGF0Y2hlci1kYXNoYm9hcmQuY29tcG9uZW50LmNzcyovXG4vKlNjc3MgY29kZSBzdGFydCBoZXJlKi9cbi5sb2NhdGlvbmZpbHRlci1pbi1tYXAge1xuICB3aWR0aDogOTAlO1xufVxuXG4uc3RpY2t5LWhlYWRlci1sb2Mge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHJpZ2h0OiAwO1xuICBwYWRkaW5nOiAxNXB4IDVweDtcbiAgdG9wOiA0NXB4O1xuICBoZWlnaHQ6IDY1cHg7XG4gIGZvbnQtc2l6ZTogMjBweDtcbiAgei1pbmRleDogMTA7XG4gIGJhY2tncm91bmQ6ICNmZmY7XG59XG5cbi5sb2NhdGlvbmZpbHRlciB7XG4gIHdpZHRoOiA5MCUgIWltcG9ydGFudDtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICByaWdodDogNXB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIGZvbnQtc2l6ZTogMTRweDtcbn1cblxuLmNhcnJpZXItbWFuYWdlZC1maWx0ZXIge1xuICB3aWR0aDogNzUlO1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIGxlZnQ6IDE1cHg7XG4gIGJvcmRlci1yYWRpdXM6IDVweDtcbiAgZm9udC1zaXplOiAxNHB4O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCB7XG4gIHBhZGRpbmc6IDAgMTBweCAxMHB4O1xufVxuLnJpZ2h0X3NpZGVfcGFuZWwgLmNsb3NlX2J0biB7XG4gIHJpZ2h0OiAxMHB4O1xuICB0b3A6IDVweDtcbn1cbi5yaWdodF9zaWRlX3BhbmVsICNteUNhcm91c2VsIGltZyB7XG4gIG1heC1oZWlnaHQ6IDE1MHB4O1xuICB3aWR0aDogMTAwJTtcbn1cbi5yaWdodF9zaWRlX3BhbmVsIGltZyB7XG4gIG1heC1oZWlnaHQ6IDI1dmggIWltcG9ydGFudDtcbiAgbWFyZ2luOiBhdXRvICFpbXBvcnRhbnQ7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMge1xuICBtYXJnaW4tdG9wOiAxMHB4O1xufVxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtZGVmYXVsdCB7XG4gIGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xuICBib3JkZXI6IDA7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1oZWFkaW5nIHtcbiAgcGFkZGluZzogNXB4IDBweDtcbiAgYmFja2dyb3VuZC1jb2xvcjogd2hpdGU7XG4gIGJvcmRlcjogMDtcbn1cbi5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLnBhbmVsLWhlYWRpbmcgYSB7XG4gIGNvbG9yOiAjMDAwMDAwO1xufVxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtYm9keSB7XG4gIHBhZGRpbmc6IDVweCA1cHg7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1ncm91cCB7XG4gIG1hcmdpbi1ib3R0b206IDBweDtcbn1cbi5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLmRhdGVfdGltZSB7XG4gIHBhZGRpbmc6IDVweDtcbn1cbi5yaWdodF9zaWRlX3BhbmVsIC5hc3NldHMtcGFuZWwge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogMDtcbiAgbGVmdDogMDtcbiAgYmFja2dyb3VuZDogI2ZmZmZmZjtcbiAgd2lkdGg6IDEwMCU7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuYXNlZXRzLWJvZHkgLnRhYi1jb250ZW50IHtcbiAgbWF4LWhlaWdodDogMzEwcHg7XG4gIG92ZXJmbG93LXk6IGF1dG87XG4gIG92ZXJmbG93LXg6IGhpZGRlbjtcbn1cbi5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogMDtcbiAgbGVmdDogMDtcbiAgYmFja2dyb3VuZDogI2ZmZmZmZjtcbiAgd2lkdGg6IDEwMCU7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuY2hhcnRzLXBhbmVsIC5jaGFydHMtaGVhZGVyIHtcbiAgcGFkZGluZzogMTBweCA1cHg7XG59XG4ucmlnaHRfc2lkZV9wYW5lbCAuY2hhcnRzLXBhbmVsIC5jaGFydHMtYm9keSB7XG4gIG1heC1oZWlnaHQ6IDM0MHB4O1xuICBvdmVyZmxvdy15OiBhdXRvO1xuICBvdmVyZmxvdy14OiBoaWRkZW47XG59XG5cbiNkaXNwYXRjaGVyLWRhdGF0YWJsZSAuYmdfbm9EbHJfZ28ge1xuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZTVlNWU1O1xufVxuXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnMge1xuICBvdmVyZmxvdy14OiBhdXRvO1xuICBvdmVyZmxvdy15OiBoaWRkZW47XG4gIGRpc3BsYXk6IC13ZWJraXQtYm94O1xuICBkaXNwbGF5OiAtbW96LWJveDtcbn1cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFiczo6LXdlYmtpdC1zY3JvbGxiYXItdGh1bWIge1xuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZTNlM2UzO1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIG9wYWNpdHk6IDAuMjtcbn1cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFiczo6LXdlYmtpdC1zY3JvbGxiYXItdHJhY2sge1xuICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcbn1cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFiczo6LXdlYmtpdC1zY3JvbGxiYXIge1xuICB3aWR0aDogNnB4O1xuICBoZWlnaHQ6IDRweDtcbiAgb3ZlcmZsb3c6IHNjcm9sbDtcbiAgYmFja2dyb3VuZC1jb2xvcjogaW5oZXJpdDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xufVxuLmFzc2V0cy1wYW5lbCAuYXNlZXRzLWJvZHkgLm5hdi10YWJzID4gbGkge1xuICBmbG9hdDogbm9uZTtcbiAgcGFkZGluZy1ib3R0b206IDVweDtcbn1cblxuLmFzZWV0cy1ib2R5IC5uYXYgPiBsaS5hY3RpdmUge1xuICBiYWNrZ3JvdW5kOiBub25lICFpbXBvcnRhbnQ7XG59XG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpLmFjdGl2ZSA+IGEge1xuICBwYWRkaW5nOiAycHggMCAhaW1wb3J0YW50O1xuICBib3JkZXItd2lkdGg6IDAgIWltcG9ydGFudDtcbiAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcbiAgY29sb3I6ICMwYzUyYjE7XG59XG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpID4gYSB7XG4gIHBhZGRpbmc6IDJweCAwICFpbXBvcnRhbnQ7XG4gIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xuICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2ZmZiAhaW1wb3J0YW50O1xuICBtYXJnaW4tcmlnaHQ6IDhweDtcbn1cbi5hc2VldHMtYm9keSAubmF2ID4gbGkgPiBhOmhvdmVyIHtcbiAgYm9yZGVyLXdpZHRoOiAwICFpbXBvcnRhbnQ7XG4gIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjMGM1MmIxICFpbXBvcnRhbnQ7XG4gIGJhY2tncm91bmQ6IG5vbmUgIWltcG9ydGFudDtcbiAgY29sb3I6ICM4NDg0ODQ7XG59XG4uYXNlZXRzLWJvZHkgLm5hdi10YWJzIHtcbiAgYm9yZGVyLWJvdHRvbTogMDtcbn1cblxuLnNob3dfZmlsdGVyIHtcbiAgd2lkdGg6IDEwMCU7XG4gIGJhY2tncm91bmQ6IDAgMDtcbiAgcG9zaXRpb246IHJlbGF0aXZlO1xuICB0b3A6IDBweDtcbiAgbGVmdDogMHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIG9wYWNpdHk6IDE7XG4gIG1hcmdpbi10b3A6IDVweDtcbn1cblxuI2Fzc2V0RGV0YWlsc01vZGFsIC5tb2RhbC1oZWFkZXIge1xuICBwYWRkaW5nOiA1cHggMTVweDtcbiAgYm9yZGVyLWJvdHRvbTogMXB4IHNvbGlkICNlNWU1ZTU7XG59XG5cbi5hc3NldC1kZXRhaWxzIHRkIHtcbiAgcGFkZGluZzogNHB4IDhweDtcbn1cblxudGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWZsb2F0aW5nIHtcbiAgdG9wOiAxN3B4ICFpbXBvcnRhbnQ7XG4gIHRvcDogNjVweCAhaW1wb3J0YW50O1xufVxuXG4uZGlzcGxheV9oaWRlIHtcbiAgZGlzcGxheTogbm9uZTtcbiAgdHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcbiAgb3BhY2l0eTogMDtcbn1cblxudGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWxvY2tlZCB7XG4gIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xuICB0b3A6IDY1cHggIWltcG9ydGFudDtcbn1cblxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcbiAgbWluLXdpZHRoOiAzMDBweDtcbiAgbWF4LXdpZHRoOiAzNTBweDtcbiAgYmFja2dyb3VuZDogI0Y5RjlGOTtcbiAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcbiAgYm94LXNpemluZzogYm9yZGVyLWJveDtcbiAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYmEoMCwgMCwgMCwgMC4xMyk7XG4gIGJvcmRlci1yYWRpdXM6IDEwcHg7XG59XG4uY2Fycmllci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XG4gIHBhZGRpbmc6IDEwcHg7XG4gIGJvcmRlci1yYWRpdXM6IDVweDtcbn1cblxuLm11bHRpc2VsZWN0LWRyb3Bkb3duIHtcbiAgZm9udC1zaXplOiAxMnB4ICFpbXBvcnRhbnQ7XG59XG5cbi5hY3RpdmVkaWZmLWRvdCB7XG4gIGhlaWdodDogMTBweDtcbiAgd2lkdGg6IDEwcHg7XG4gIGJhY2tncm91bmQtY29sb3I6ICM1ODViZmY7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xuICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XG59XG5cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIge1xuICBtaW4td2lkdGg6IDQyNXB4O1xuICBtYXgtd2lkdGg6IDQ1MHB4O1xuICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xuICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xuICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiYSgwLCAwLCAwLCAwLjEzKTtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XG4gIHBhZGRpbmc6IDA7XG4gIGJvcmRlci1yYWRpdXM6IDVweDtcbiAgYmFja2dyb3VuZDogI2ZmZmZmZjtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyB7XG4gIHBhZGRpbmc6IDE1cHg7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLmZvbnQtYm9sZCB7XG4gIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLmJvcmRlci1ib3R0b20tMiB7XG4gIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XG59XG5cbi5jaXJjbGUtYmFkZ2Uge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogLTExcHg7XG4gIGxlZnQ6IC0xNHB4O1xuICBiYWNrZ3JvdW5kOiAjZmE5MzkzO1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGZvbnQtc2l6ZTogMTJweDtcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xuICBjb2xvcjogd2hpdGU7XG4gIGRpc3BsYXk6IGlubGluZS1mbGV4O1xuICBhbGlnbi1pdGVtczogY2VudGVyO1xuICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcbiAgd2lkdGg6IDE4cHg7XG4gIGhlaWdodDogMThweDtcbn0iXX0= */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](LocationComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-location',
          templateUrl: './location.component.html',
          styleUrls: ['./location.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]
        }, {
          type: _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_11__["CarrierService"]
        }];
      }, {
        locationGridView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"]]
        }],
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/sales-data/grid-view.component.ts": function srcAppDispatcherDispatcherDashboardSalesDataGridViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "GridViewComponent", function () {
      return GridViewComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../../shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/service/wally-utility.service */
    "./src/app/carrier/service/wally-utility.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../../../directives/numberWithDecimal */
    "./src/app/directives/numberWithDecimal.ts");

    function GridViewComponent_div_2_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 37);
      }
    }

    function GridViewComponent_div_2_tr_45_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 38);
      }
    }

    function GridViewComponent_div_2_tr_45_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_21_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_21_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_2_tr_45_div_21_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_2_tr_45_div_21_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r9.PrevSale == "--" ? "Not Available" : row_r9.PrevSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.PrevSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.PrevSale != "--");
      }
    }

    function GridViewComponent_div_2_tr_45_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_24_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_24_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_2_tr_45_div_24_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_2_tr_45_div_24_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r9.WeekAgoSale == "--" ? "Not Available" : row_r9.WeekAgoSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.WeekAgoSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.WeekAgoSale != "--");
      }
    }

    function GridViewComponent_div_2_tr_45_span_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_35_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_span_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_2_tr_45_a_45_Template(rf, ctx) {
      if (rf & 1) {
        var _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_2_tr_45_a_45_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r35);

          var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r33.openModal(row_r9);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.Status);
      }
    }

    function GridViewComponent_div_2_tr_45_ng_template_46_Template(rf, ctx) {
      if (rf & 1) {
        var _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_2_tr_45_ng_template_46_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r39);

          var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r37.showTanks(row_r9);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.Status);
      }
    }

    function GridViewComponent_div_2_tr_45_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, GridViewComponent_div_2_tr_45_span_11_Template, 1, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, GridViewComponent_div_2_tr_45_span_12_Template, 1, 0, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, GridViewComponent_div_2_tr_45_span_17_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, GridViewComponent_div_2_tr_45_span_18_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, GridViewComponent_div_2_tr_45_div_20_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, GridViewComponent_div_2_tr_45_div_21_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, GridViewComponent_div_2_tr_45_div_23_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, GridViewComponent_div_2_tr_45_div_24_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](27, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](28, GridViewComponent_div_2_tr_45_span_28_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, GridViewComponent_div_2_tr_45_span_29_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, GridViewComponent_div_2_tr_45_span_34_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, GridViewComponent_div_2_tr_45_span_35_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, GridViewComponent_div_2_tr_45_span_38_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, GridViewComponent_div_2_tr_45_span_39_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_2_tr_45_a_45_Template, 2, 1, "a", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, GridViewComponent_div_2_tr_45_ng_template_46_Template, 2, 1, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r9 = ctx.$implicit;

        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r9.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9 == null ? null : row_r9.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r9 == null ? null : row_r9.TankInventoryDiffinHrs) > 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.WaterLevel == "--" ? "Not Available" : row_r9.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r9.AvgSale == "--" ? "Not Available" : row_r9.AvgSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r9.Inventory == "--" ? "Not Available" : _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](27, 29, row_r9.Inventory, "1.0-2"), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.LastReadingTime == null || row_r9.LastReadingTime == "--" ? "Not Available" : row_r9.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r9.Ullage == "--" ? "Not Available" : row_r9.Ullage, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r9.LastDeliveredQuantity == "--" ? "Not Available" : row_r9.LastDeliveredQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 3 && row_r9.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r9.UOM == 4 && row_r9.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.LastDeliveryDate == "--" ? "Not Available" : row_r9.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r9.DaysRemaining == "--" ? "NA" : row_r9.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r9 == null ? null : row_r9.Status) == "Scheduled")("ngIfElse", _r25);
      }
    }

    function GridViewComponent_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Location Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Tank Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Water Level");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Trailing 7 Day Average");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Previous Day Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Week Ago Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Last Inventory Reading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Last Reading Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Ullage");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Last Delivered Qty");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Last Delivered On");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Days Remaining");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](44, GridViewComponent_div_2_tr_44_Template, 2, 0, "tr", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_2_tr_45_Template, 48, 32, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r0.dtMustGoOptions)("dtTrigger", ctx_r0.dtMustGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r0.IsMustGoLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r0.MustGoSchedules);
      }
    }

    function GridViewComponent_div_3_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 37);
      }
    }

    function GridViewComponent_div_3_tr_45_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 38);
      }
    }

    function GridViewComponent_div_3_tr_45_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_21_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_21_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_3_tr_45_div_21_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_3_tr_45_div_21_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r43.PrevSale == "--" ? "Not Available" : row_r43.PrevSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.PrevSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.PrevSale != "--");
      }
    }

    function GridViewComponent_div_3_tr_45_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_24_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_24_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_3_tr_45_div_24_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_3_tr_45_div_24_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r43.WeekAgoSale == "--" ? "Not Available" : row_r43.WeekAgoSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.WeekAgoSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.WeekAgoSale != "--");
      }
    }

    function GridViewComponent_div_3_tr_45_span_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_35_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_span_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_3_tr_45_a_45_Template(rf, ctx) {
      if (rf & 1) {
        var _r69 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_3_tr_45_a_45_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r69);

          var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r67.openModal(row_r43);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.Status);
      }
    }

    function GridViewComponent_div_3_tr_45_ng_template_46_Template(rf, ctx) {
      if (rf & 1) {
        var _r73 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_3_tr_45_ng_template_46_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r73);

          var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r71 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r71.showTanks(row_r43);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.Status);
      }
    }

    function GridViewComponent_div_3_tr_45_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, GridViewComponent_div_3_tr_45_span_11_Template, 1, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, GridViewComponent_div_3_tr_45_span_12_Template, 1, 0, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, GridViewComponent_div_3_tr_45_span_17_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, GridViewComponent_div_3_tr_45_span_18_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, GridViewComponent_div_3_tr_45_div_20_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, GridViewComponent_div_3_tr_45_div_21_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, GridViewComponent_div_3_tr_45_div_23_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, GridViewComponent_div_3_tr_45_div_24_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](27, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](28, GridViewComponent_div_3_tr_45_span_28_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, GridViewComponent_div_3_tr_45_span_29_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, GridViewComponent_div_3_tr_45_span_34_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, GridViewComponent_div_3_tr_45_span_35_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, GridViewComponent_div_3_tr_45_span_38_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, GridViewComponent_div_3_tr_45_span_39_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_3_tr_45_a_45_Template, 2, 1, "a", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, GridViewComponent_div_3_tr_45_ng_template_46_Template, 2, 1, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r43 = ctx.$implicit;

        var _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r43.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43 == null ? null : row_r43.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r43 == null ? null : row_r43.TankInventoryDiffinHrs) > 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.WaterLevel == "--" ? "Not Available" : row_r43.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r43.AvgSale == "--" ? "Not Available" : row_r43.AvgSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r43.Inventory == "--" ? "Not Available" : _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](27, 29, row_r43.Inventory, "1.0-2"), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.LastReadingTime == null || row_r43.LastReadingTime == "--" ? "Not Available" : row_r43.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r43.Ullage == "--" ? "Not Available" : row_r43.Ullage, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r43.LastDeliveredQuantity == "--" ? "Not Available" : row_r43.LastDeliveredQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 3 && row_r43.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r43.UOM == 4 && row_r43.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.LastDeliveryDate == "--" ? "Not Available" : row_r43.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r43.DaysRemaining == "--" ? "NA" : row_r43.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r43 == null ? null : row_r43.Status) == "Scheduled")("ngIfElse", _r59);
      }
    }

    function GridViewComponent_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Should Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Location Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Tank Name ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Water Level ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Trailing 7 Day Average");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Previous Day Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Week Ago Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Last Inventory Reading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Last Reading Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Ullage");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Last Delivered Qty");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Last Delivered On");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Days Remaining");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](44, GridViewComponent_div_3_tr_44_Template, 2, 0, "tr", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_3_tr_45_Template, 48, 32, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r1.dtShouldGoOptions)("dtTrigger", ctx_r1.dtMustGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.IsShouldGoLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r1.ShouldGoSchedules);
      }
    }

    function GridViewComponent_div_4_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 37);
      }
    }

    function GridViewComponent_div_4_tr_45_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 38);
      }
    }

    function GridViewComponent_div_4_tr_45_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_21_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_21_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_4_tr_45_div_21_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_4_tr_45_div_21_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r77.PrevSale == "--" ? "Not Available" : row_r77.PrevSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.PrevSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.PrevSale != "--");
      }
    }

    function GridViewComponent_div_4_tr_45_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_24_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_24_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_4_tr_45_div_24_span_2_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_4_tr_45_div_24_span_3_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r77.WeekAgoSale == "--" ? "Not Available" : row_r77.WeekAgoSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.WeekAgoSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.WeekAgoSale != "--");
      }
    }

    function GridViewComponent_div_4_tr_45_span_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_35_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_span_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_div_4_tr_45_a_45_Template(rf, ctx) {
      if (rf & 1) {
        var _r103 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_4_tr_45_a_45_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r103);

          var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r101 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r101.openModal(row_r77);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.Status);
      }
    }

    function GridViewComponent_div_4_tr_45_ng_template_46_Template(rf, ctx) {
      if (rf & 1) {
        var _r107 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_div_4_tr_45_ng_template_46_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r107);

          var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r105.showTanks(row_r77);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.Status);
      }
    }

    function GridViewComponent_div_4_tr_45_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, GridViewComponent_div_4_tr_45_span_11_Template, 1, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, GridViewComponent_div_4_tr_45_span_12_Template, 1, 0, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, GridViewComponent_div_4_tr_45_span_17_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, GridViewComponent_div_4_tr_45_span_18_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, GridViewComponent_div_4_tr_45_div_20_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, GridViewComponent_div_4_tr_45_div_21_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, GridViewComponent_div_4_tr_45_div_23_Template, 4, 0, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, GridViewComponent_div_4_tr_45_div_24_Template, 4, 3, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](27, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](28, GridViewComponent_div_4_tr_45_span_28_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, GridViewComponent_div_4_tr_45_span_29_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, GridViewComponent_div_4_tr_45_span_34_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, GridViewComponent_div_4_tr_45_span_35_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, GridViewComponent_div_4_tr_45_span_38_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, GridViewComponent_div_4_tr_45_span_39_Template, 2, 0, "span", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_4_tr_45_a_45_Template, 2, 1, "a", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, GridViewComponent_div_4_tr_45_ng_template_46_Template, 2, 1, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r77 = ctx.$implicit;

        var _r93 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r77.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77 == null ? null : row_r77.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r77 == null ? null : row_r77.TankInventoryDiffinHrs) > 2 || (row_r77 == null ? null : row_r77.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.WaterLevel == "--" ? "Not Available" : row_r77.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r77.AvgSale == "--" ? "Not Available" : row_r77.AvgSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r77.Inventory == "--" ? "Not Available" : _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](27, 29, row_r77.Inventory, "1.0-2"), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.LastReadingTime == null || row_r77.LastReadingTime == "--" ? "Not Available" : row_r77.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r77.Ullage == "--" ? "Not Available" : row_r77.Ullage, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r77.LastDeliveredQuantity == "--" ? "Not Available" : row_r77.LastDeliveredQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 3 && row_r77.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r77.UOM == 4 && row_r77.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.LastDeliveryDate == "--" ? "Not Available" : row_r77.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r77.DaysRemaining == "--" ? "NA" : row_r77.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r77 == null ? null : row_r77.Status) == "Scheduled")("ngIfElse", _r93);
      }
    }

    function GridViewComponent_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Could Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Location Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Tank Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Water Level");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Trailing 7 Day Average");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Previous Day Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Week Ago Sale");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Last Inventory Reading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Last Reading Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Ullage");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Last Delivered Qty");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Last Delivered On");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Days Remaining");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](44, GridViewComponent_div_4_tr_44_Template, 2, 0, "tr", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, GridViewComponent_div_4_tr_45_Template, 48, 32, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r2.dtCouldGoOptions)("dtTrigger", ctx_r2.dtMustGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.IsCouldGoLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r2.CouldGoSchedules);
      }
    }

    function GridViewComponent_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_ng_container_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    function GridViewComponent_ng_template_7_div_9_div_1_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "option", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var sqType_r116 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", sqType_r116.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", sqType_r116.Name, " ");
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "show-element": a0
      };
    };

    function GridViewComponent_ng_template_7_div_9_div_1_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r118 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "input", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function GridViewComponent_ng_template_7_div_9_div_1_div_12_Template_input_ngModelChange_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r118);

          var ctx_r117 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

          return ctx_r117.RequiredQuantity = $event;
        })("change", function GridViewComponent_ng_template_7_div_9_div_1_div_12_Template_input_change_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r118);

          var ctx_r119 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

          return ctx_r119.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r115 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx_r115.ScheduleQuantityType > 1 ? true : null)("ngModel", ctx_r115.RequiredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c0, !ctx_r115.isValid));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r115.validateMsg, " ");
      }
    }

    function GridViewComponent_ng_template_7_div_9_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r121 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "h3");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Create DR");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Quantity Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "select", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function GridViewComponent_ng_template_7_div_9_div_1_Template_select_ngModelChange_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r120 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r120.ScheduleQuantityType = $event;
        })("change", function GridViewComponent_ng_template_7_div_9_div_1_Template_select_change_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r122 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          ctx_r122.RequiredQuantity = null;
          return ctx_r122.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, GridViewComponent_ng_template_7_div_9_div_1_option_11_Template, 2, 2, "option", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, GridViewComponent_ng_template_7_div_9_div_1_div_12_Template, 8, 6, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "input", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function GridViewComponent_ng_template_7_div_9_div_1_Template_input_ngModelChange_18_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r123.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "label", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, " Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "input", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function GridViewComponent_ng_template_7_div_9_div_1_Template_input_ngModelChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r124 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r124.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "label", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, " Should Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "input", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function GridViewComponent_ng_template_7_div_9_div_1_Template_input_ngModelChange_26_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r125 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r125.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, " Could Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "button", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_ng_template_7_div_9_div_1_Template_button_click_30_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r121);

          var ctx_r126 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r126.onDrSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "Create");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r111 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r111.ScheduleQuantityType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r111.ScheduleQuantityTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r111.ScheduleQuantityType == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r111.DrPriority)("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r111.DrPriority)("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r111.DrPriority)("value", 3);
      }
    }

    function GridViewComponent_ng_template_7_div_9_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function GridViewComponent_ng_template_7_div_9_div_3_tr_27_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var del_r128 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", del_r128.QuantityTypeId == 0 || del_r128.QuantityTypeId == 1 ? del_r128.Quantity : del_r128.QuantityTypeName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r128.ScheduleDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r128.ScheduleTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r128.Driver);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r128.Carrier);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "show": a0
      };
    };

    function GridViewComponent_ng_template_7_div_9_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h2", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "button", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Existing Delivery Request(s) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "i", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](9, "i", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "table", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](19, "Schedule Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Schedule Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, GridViewComponent_ng_template_7_div_9_div_3_tr_27_Template, 11, 5, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r109 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).modalDetails;

        var ctx_r113 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](2, _c1, modalDetails_r109.IsScheduled));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r113.ExistingDeliveries);
      }
    }

    function GridViewComponent_ng_template_7_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, GridViewComponent_ng_template_7_div_9_div_1_Template, 32, 9, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_ng_template_7_div_9_div_2_Template, 2, 0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_ng_template_7_div_9_div_3_Template, 28, 4, "div", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r109 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().modalDetails;

        var ctx_r110 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !modalDetails_r109.IsScheduled);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r110.DRLoader);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r110.ExistingDeliveries.length);
      }
    }

    var _c2 = function _c2(a2) {
      return {
        "modal": true,
        "fade": true,
        "show": a2
      };
    };

    var _c3 = function _c3(a0) {
      return {
        "display": a0
      };
    };

    function GridViewComponent_ng_template_7_Template(rf, ctx) {
      if (rf & 1) {
        var _r132 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h3", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function GridViewComponent_ng_template_7_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r132);

          var ctx_r131 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r131.closeModal();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "i", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, GridViewComponent_ng_template_7_div_9_Template, 4, 3, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r109 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c2, modalDetails_r109.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](6, _c3, modalDetails_r109.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", modalDetails_r109.title, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", modalDetails_r109.display === "block");
      }
    }

    var GridViewComponent = /*#__PURE__*/function () {
      function GridViewComponent(dispatcherService, wallyUtilService) {
        _classCallCheck(this, GridViewComponent);

        this.dispatcherService = dispatcherService;
        this.wallyUtilService = wallyUtilService;
        this.MustGoSchedules = [];
        this.ShouldGoSchedules = [];
        this.CouldGoSchedules = [];
        this.dtMustGoOptions = {};
        this.dtShouldGoOptions = {};
        this.dtCouldGoOptions = {};
        this.dtMustGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtShouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtCouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.showDr = false;
        this.IsDrExists = false;
        this.DRLoader = false;
        this.ExistingDeliveries = [];
        this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].MustGo;
        this.dsModal = {
          modalDetails: {
            display: 'none',
            data: 'Modal Show',
            title: 'Delivery Schedule(s)',
            IsScheduled: false
          }
        };
        this.isValid = true;
        this.ScheduleQuantityTypes = [];
        this.loadingData = false;
        this.applyFilterSubscription = [];
        this.columnsDetails = [{
          data: 'Cust',
          "autoWidth": true
        }, {
          data: 'LocName',
          "autoWidth": true
        }, {
          data: 'Loc',
          "autoWidth": true
        }, {
          data: 'TName',
          "autoWidth": true
        }, {
          data: 'Avg7Day',
          "autoWidth": true
        }, {
          data: 'PDS',
          "autoWidth": true
        }, {
          data: 'CI',
          "autoWidth": true
        }, {
          data: 'Ullg',
          "autoWidth": true
        }, {
          data: 'lastDelivery',
          "autoWidth": true
        }, {
          data: 'lastDeliveryQty',
          "autoWidth": true
        }, {
          data: 'DRemg',
          "autoWidth": true
        }];
        this.SelectedPrioritiesId = [];
      }

      _createClass(GridViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this27 = this;

          this.applyFilterSubscription.push(Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["merge"])(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(function (value) {
            _this27.bindPriorityArray();

            _this27.getSalesData();
          })); //to load data - after second ngOnInit

          if (this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value) {
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
            this.bindPriorityArray();
            this.getSalesData();
          }

          this.init();
          this.getScheduleQuantityType();
        }
      }, {
        key: "init",
        value: function init() {
          this.initializeMustGo();
          this.initializeCouldGo();
          this.initializeShouldGo(); // this.getSalesData();
        }
      }, {
        key: "getScheduleQuantityType",
        value: function getScheduleQuantityType() {
          var _this28 = this;

          this.dispatcherService.GetScheduleQtyType().subscribe(function (SQT) {
            _this28.ScheduleQuantityTypes = SQT || [];
          });
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtCouldGoTrigger.unsubscribe();
          this.dtShouldGoTrigger.unsubscribe();
          this.dtMustGoTrigger.unsubscribe();

          if (this.applyFilterSubscription) {
            this.applyFilterSubscription.forEach(function (subscription) {
              subscription.unsubscribe();
            });
          }
        }
      }, {
        key: "initializeMustGo",
        value: function initializeMustGo() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtMustGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Sales Details-MustGo',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sales Details-MustGo',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            // columns: this.columnsDetails,
            pagingType: 'first_last_numbers',
            pageLength: 10,
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }]
          };
        }
      }, {
        key: "initializeCouldGo",
        value: function initializeCouldGo() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtCouldGoOptions = {
            colReorder: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Sales Details-CouldGo',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sales Details-CouldGo',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            // columns: this.columnsDetails,
            pagingType: 'first_last_numbers',
            pageLength: 10,
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }]
          };
        }
      }, {
        key: "initializeShouldGo",
        value: function initializeShouldGo() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtShouldGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Sales Details-ShouldGo',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sales Details-ShouldGo',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            // columns: this.columnsDetails,
            pagingType: 'first_last_numbers',
            pageLength: 10,
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }]
          };
        }
      }, {
        key: "getSalesDtls",
        value: function getSalesDtls() {
          var _this29 = this;

          var inputs = {
            RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value),
            Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].None,
            CustomerId: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
            LocationId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedlocationList').value),
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["SelectedTabEnum"].Priority,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
            IsShowRetailJobs: ''
          };
          this.IsShouldGoLoading = true;
          this.IsCouldGoLoading = true;
          this.IsMustGoLoading = true;
          Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["forkJoin"])([this.dispatcherService.getSalesData(inputs), this.dispatcherService.GetRaisedExceptions()]).subscribe(function (resp) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this29, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
              return regeneratorRuntime.wrap(function _callee2$(_context2) {
                while (1) {
                  switch (_context2.prev = _context2.next) {
                    case 0:
                      _context2.next = 2;
                      return resp[0];

                    case 2:
                      _context2.t0 = _context2.sent;

                      if (!_context2.t0) {
                        _context2.next = 5;
                        break;
                      }

                      resp[0].map(function (m) {
                        if (resp[1] && resp[1].filter(function (f) {
                          return f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == m.SiteId && f.TankDetail.StorageId == m.StorageId;
                        }).length > 0) {
                          m.IsUnknownOrMissing = true;
                        } else m.IsUnknownOrMissing = false;
                      });

                    case 5:
                      _context2.next = 7;
                      return resp[0];

                    case 7:
                      _context2.t1 = _context2.sent;

                      if (!_context2.t1) {
                        _context2.next = 10;
                        break;
                      }

                      _context2.t1 = resp[0].filter(function (t) {
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].ShouldGo && t.Inventory != '--';
                      });

                    case 10:
                      this.ShouldGoSchedules = _context2.t1;
                      _context2.next = 13;
                      return resp[0];

                    case 13:
                      _context2.t2 = _context2.sent;

                      if (!_context2.t2) {
                        _context2.next = 16;
                        break;
                      }

                      _context2.t2 = resp[0].filter(function (t) {
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].CouldGo && t.Inventory != '--';
                      });

                    case 16:
                      this.CouldGoSchedules = _context2.t2;
                      _context2.next = 19;
                      return resp[0];

                    case 19:
                      _context2.t3 = _context2.sent;

                      if (!_context2.t3) {
                        _context2.next = 22;
                        break;
                      }

                      _context2.t3 = resp[0].filter(function (t) {
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].MustGo && t.Inventory != '--';
                      });

                    case 22:
                      this.MustGoSchedules = _context2.t3;
                      this.destroyDatatable();
                      this.IsShouldGoLoading = false;
                      this.IsCouldGoLoading = false;
                      this.IsMustGoLoading = false;
                      this.dtCouldGoTrigger.next();
                      this.dtShouldGoTrigger.next();
                      this.dtMustGoTrigger.next();

                    case 30:
                    case "end":
                      return _context2.stop();
                  }
                }
              }, _callee2, this);
            }));
          });
        }
      }, {
        key: "filterData",
        value: function filterData() {
          this.getSalesData();
        }
      }, {
        key: "getSalesData",
        value: function getSalesData() {
          // let _priorities = []; 
          // this.SelectedPriorities.forEach(x => _priorities.push(x.Id));
          // this.SelectedPrioritiesId = _priorities;
          this.IsCouldGoLoading = true;
          this.IsShouldGoLoading = true;
          this.IsMustGoLoading = true; //this.destroyDatatable();

          this.getSalesDtls();
        }
      }, {
        key: "destroyDatatable",
        value: function destroyDatatable() {
          if (this.dtElements) {
            this.dtElements.forEach(function (dtElement) {
              if (dtElement.dtInstance) {
                dtElement.dtInstance.then(function (dtInstance) {
                  dtInstance.destroy();
                });
              }
            });
          }
        }
      }, {
        key: "openModal",
        value: function openModal(row) {
          var _this30 = this;

          this.resetModal();
          this.SelectedTank = row;
          this.DRLoader = true;
          this.dispatcherService.GetDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe(function (resp) {
            _this30.ExistingDeliveries = resp;
            _this30.DRLoader = false;
          });
          this.dsModal.modalDetails.display = 'block';
          var isSchedule = row.Status == 'Scheduled';
          this.dsModal.modalDetails.IsScheduled = isSchedule;
          this.showDr = isSchedule; //this.MaxFillQuantity = 120;
        }
      }, {
        key: "resetModal",
        value: function resetModal() {
          this.ExistingDeliveries = [];
          this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].MustGo;
          this.RequiredQuantity = null;
          this.ScheduleQuantityType = 1;
          this.validateMsg = "";
          this.isValid = true;
        }
      }, {
        key: "showTanks",
        value: function showTanks(row) {
          this.SelectedTankRegionId = row.RegionId;
          this.dipTestComponent.loadTankDR(row);
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }, {
        key: "closeModal",
        value: function closeModal() {
          this.dsModal.modalDetails.display = 'none';
          this.isValid = true;
          $(".modal-backdrop").hide();
          $('body').removeClass('modal-open');
        }
      }, {
        key: "toggleDrs",
        value: function toggleDrs() {
          this.showDr = !this.showDr;
        }
      }, {
        key: "onDrSubmit",
        value: function onDrSubmit() {
          var _this31 = this;

          this.validateMsg = "";
          this.isValid = true;
          var raiseDr = {
            SiteId: this.SelectedTank.SiteId,
            TankId: this.SelectedTank.TankId,
            StorageId: this.SelectedTank.StorageId,
            RequiredQuantity: this.ScheduleQuantityType == 1 ? this.RequiredQuantity : 0,
            ScheduleQuantityType: this.ScheduleQuantityType,
            JobId: this.SelectedTank.TfxJobId,
            FuelTypeId: this.SelectedTank.ProductTypeId,
            Priority: this.DrPriority
          };

          if (this.ScheduleQuantityType == 1 && (this.RequiredQuantity == null || this.RequiredQuantity == 0 || this.RequiredQuantity < 0.00001)) {
            this.validateMsg = "Invalid required quantity.";
            this.isValid = false;
          } else if (this.ScheduleQuantityType == 1 && this.SelectedTank.MaxFillQuantity && this.SelectedTank.MaxFillQuantity > 0 && this.RequiredQuantity > this.SelectedTank.MaxFillQuantity) {
            this.validateMsg = "Should not exceed max fill. (" + this.SelectedTank.MaxFillQuantity + ")";
            this.isValid = false;
          } else {
            this.DRLoader = true;
            this.dispatcherService.PostRaiseDeliveryRequest(raiseDr).subscribe(function (response) {
              if (response != null && response.StatusCode == 0) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

                _this31.closeModal();
              } else {
                _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
              }

              _this31.DRLoader = false;
            });
          }
        }
      }, {
        key: "bindPriorityArray",
        value: function bindPriorityArray() {
          var _this32 = this;

          this.SelectedPrioritiesId = [];
          var SelectedPriorities = this.salesTabFilterForm.get('SelectedPriorities').value;
          SelectedPriorities.forEach(function (res) {
            _this32.SelectedPrioritiesId.push(res.Id);
          });
        }
      }]);

      return GridViewComponent;
    }();

    GridViewComponent.ɵfac = function GridViewComponent_Factory(t) {
      return new (t || GridViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_8__["WallyUtilService"]));
    };

    GridViewComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: GridViewComponent,
      selectors: [["app-grid-view"]],
      viewQuery: function GridViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dipTestComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 11,
      vars: 10,
      consts: [[1, "row", "mt60"], ["id", "grid-view", 1, "col-sm-12"], [4, "ngIf"], ["class", "pa top0 bg-white left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["schedulesModal", ""], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "onRaiseDR"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-salemustgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Cust"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "TName"], ["data-key", "WL"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["data-key", "Status"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click", 4, "ngIf", "ngIfElse"], ["notSceduledBlock", ""], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click"], ["data-target", "raisedr", "onclick", "slidePanel('#raisedr','60%')", 3, "click"], [1, "shouldgo", "mb5", 2, "color", "#f3c316 !important"], ["id", "table-saleshouldgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [1, "couldgo", "mb5", 2, "color", "#a7a2a2 !important"], ["id", "table-salecouldgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [1, "pa", "top0", "bg-white", "left0", "z-index5", "loading-wrapper"], ["id", "schedulesModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "schedulesModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered", "modal-lg"], [1, "modal-content"], [1, "modal-header", "pt10", "pb5", "no-border"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt5", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body"], ["class", "assets-header", 4, "ngIf"], [1, "assets-header"], ["class", "well bg-white no-shadow border border pr", 4, "ngIf"], [1, "well", "bg-white", "no-shadow", "border", "border", "pr"], [1, "row"], [1, "col-sm-12"], [1, "row", "col-sm-12"], [1, "col-sm-3", "input-group"], [1, "form-group", "mb0"], [1, "form-control", 3, "ngModel", "ngModelChange", "change"], [3, "value", 4, "ngFor", "ngForOf"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-6", "mt5"], [1, "col-sm-12", "pa0", "mt5"], [1, "form-check", "form-check-inline"], ["id", "mustgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "mustgo-dr", 1, "form-check-label"], ["id", "shouldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "shouldgo-dr", 1, "form-check-label"], ["id", "couldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "couldgo-dr", 1, "form-check-label"], [1, "col-sm-12", "text-right", "mt10"], ["type", "button", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [3, "value"], [1, "col-sm-3"], [1, "input-group"], ["type", "text", "numberWithDecimal", "", "required", "", 1, "form-control", 3, "disabled", "ngModel", "ngModelChange", "change"], [1, "invalid-feedback", 3, "ngClass"], ["id", "accordionExitingDrReq", 1, "accordionExitingDrReq", "mt10", "mb10"], [1, "card"], ["id", "headingOne", 1, "card-header", "pt5", "pb5", "pl10", "pr10"], [1, "mb-0"], ["type", "button", "data-toggle", "collapse", "data-target", "#collapseOne", "aria-expanded", "true", "aria-controls", "collapseOne", 1, "d-flex", "align-items-center", "justify-content-between", "btn", "btn-link", "collapsed"], [1, "fa-stack", "fa-sm", "icon-color-b"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-angle-down", "fa-stack-1x", "fa-inverse"], ["id", "collapseOne", "aria-labelledby", "headingOne", "data-parent", "#accordionExitingDrReq", 1, "collapse", 3, "ngClass"], [1, "card-body", "pa5"], [1, "table", "table-hover", "margin", "bottom", "details-table"]],
      template: function GridViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, GridViewComponent_div_2_Template, 46, 4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, GridViewComponent_div_3_Template, 46, 4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, GridViewComponent_div_4_Template, 46, 4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](5, GridViewComponent_div_5_Template, 2, 0, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](6, GridViewComponent_ng_container_6_Template, 1, 0, "ng-container", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, GridViewComponent_ng_template_7_Template, 10, 8, "ng-template", null, 5, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "app-dip-test", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onRaiseDR", function GridViewComponent_Template_app_dip_test_onRaiseDR_10_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(1) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(2) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(3) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsMustGoLoading || ctx.IsShouldGoLoading || ctx.IsCouldGoLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r5)("ngTemplateOutletContext", ctx.dsModal);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("isDisableControl", false)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedTankRegionId)("IsThisFromDrDisplay", false);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgTemplateOutlet"], _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"], angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgStyle"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["ɵangular_packages_forms_forms_x"], _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_12__["NumberWithDecimal"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RequiredValidator"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["DecimalPipe"]],
      styles: [".active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\r\n\r\n@keyframes blink {\r\n    from,to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n\r\n@-webkit-keyframes blink {\r\n    from, to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n\r\ntable.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    position: fixed !important;\r\n}\r\n\r\ntable.dataTable.fixedHeader-floating[_ngcontent-%COMP%], table.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    top: 65px !important;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL2dyaWQtdmlldy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtJQUNaLFdBQVc7SUFDWCx5QkFBeUI7SUFDekIsa0JBQWtCO0lBQ2xCLHFCQUFxQjtJQUNyQix5Q0FBaUM7WUFBakMsaUNBQWlDO0FBQ3JDOztBQUVBO0lBQ0k7UUFDSSxVQUFVO0lBQ2Q7O0lBRUE7UUFDSSxVQUFVO0lBQ2Q7QUFDSjs7QUFZQTtJQUNJO1FBQ0ksVUFBVTtJQUNkOztJQUVBO1FBQ0ksVUFBVTtJQUNkO0FBQ0o7O0FBc0JBO0lBQ0ksMEJBQTBCO0FBQzlCOztBQUVBO0lBQ0ksb0JBQW9CO0FBQ3hCIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL2dyaWQtdmlldy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmFjdGl2ZS1kb3Qge1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmY1ODU4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xyXG4gICAgYW5pbWF0aW9uOiAxcyBibGluayBlYXNlIGluZmluaXRlO1xyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIGJsaW5rIHtcclxuICAgIGZyb20sdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALW1vei1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALXdlYmtpdC1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALW1zLWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtby1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItbG9ja2VkIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcsIHRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4gICAgdG9wOiA2NXB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuIl19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](GridViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-grid-view',
          templateUrl: './grid-view.component.html',
          styleUrls: ['./grid-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]
        }, {
          type: src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_8__["WallyUtilService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"]]
        }],
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/sales-data/location-view.component.ts": function srcAppDispatcherDispatcherDashboardSalesDataLocationViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationViewComponent", function () {
      return LocationViewComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../../shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../../../directives/numberWithDecimal */
    "./src/app/directives/numberWithDecimal.ts");

    function LocationViewComponent_tr_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 36);
      }
    }

    function LocationViewComponent_tr_47_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "span", 37);
      }
    }

    function LocationViewComponent_tr_47_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_21_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_21_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, LocationViewComponent_tr_47_div_21_span_2_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, LocationViewComponent_tr_47_div_21_span_3_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r5.PrevSale == "--" ? "Not Available" : row_r5.PrevSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.PrevSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.PrevSale != "--");
      }
    }

    function LocationViewComponent_tr_47_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_24_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_24_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, LocationViewComponent_tr_47_div_24_span_2_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, LocationViewComponent_tr_47_div_24_span_3_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r5.WeekAgoSale == "--" ? "Not Available" : row_r5.WeekAgoSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.WeekAgoSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.WeekAgoSale != "--");
      }
    }

    function LocationViewComponent_tr_47_span_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_35_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " G");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_span_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " L");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_tr_47_a_45_Template(rf, ctx) {
      if (rf & 1) {
        var _r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationViewComponent_tr_47_a_45_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r29.openModal(row_r5);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.Status);
      }
    }

    function LocationViewComponent_tr_47_ng_template_46_Template(rf, ctx) {
      if (rf & 1) {
        var _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationViewComponent_tr_47_ng_template_46_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r35);

          var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r33.showTanks(row_r5);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.Status);
      }
    }

    function LocationViewComponent_tr_47_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, LocationViewComponent_tr_47_span_11_Template, 1, 0, "span", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, LocationViewComponent_tr_47_span_12_Template, 1, 0, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, LocationViewComponent_tr_47_span_17_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, LocationViewComponent_tr_47_span_18_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, LocationViewComponent_tr_47_div_20_Template, 4, 0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, LocationViewComponent_tr_47_div_21_Template, 4, 3, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, LocationViewComponent_tr_47_div_23_Template, 4, 0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, LocationViewComponent_tr_47_div_24_Template, 4, 3, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](27, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](28, LocationViewComponent_tr_47_span_28_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, LocationViewComponent_tr_47_span_29_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, LocationViewComponent_tr_47_span_34_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, LocationViewComponent_tr_47_span_35_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, LocationViewComponent_tr_47_span_38_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, LocationViewComponent_tr_47_span_39_Template, 2, 0, "span", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, LocationViewComponent_tr_47_a_45_Template, 2, 1, "a", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, LocationViewComponent_tr_47_ng_template_46_Template, 2, 1, "ng-template", null, 35, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = ctx.$implicit;

        var _r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](47);

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r5.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5 == null ? null : row_r5.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r5 == null ? null : row_r5.TankInventoryDiffinHrs) > 2 || (row_r5 == null ? null : row_r5.TankInventoryDiffinHrs) == 0 && ctx_r1.IsShowRetailJobs);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.WaterLevel == "--" ? "Not Available" : row_r5.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", row_r5.AvgSale == "--" ? "Not Available" : row_r5.AvgSale, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.AvgSale != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r5.Inventory == "--" ? "Not Available" : _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](27, 29, row_r5.Inventory, "1.0-2"), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.Inventory != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.LastReadingTime == null || row_r5.LastReadingTime == "--" ? "Not Available" : row_r5.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r5.Ullage == "--" ? "Not Available" : row_r5.Ullage, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.Ullage != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", row_r5.LastDeliveredQuantity == "--" ? "Not Available" : row_r5.LastDeliveredQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 3 && row_r5.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r5.UOM == 4 && row_r5.LastDeliveredQuantity != "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.LastDeliveryDate == "--" ? "Not Available" : row_r5.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r5.DaysRemaining == "--" ? "NA" : row_r5.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (row_r5 == null ? null : row_r5.Status) == "Scheduled")("ngIfElse", _r21);
      }
    }

    function LocationViewComponent_ng_container_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    function LocationViewComponent_ng_template_49_div_9_div_1_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "option", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var sqType_r44 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", sqType_r44.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", sqType_r44.Name, " ");
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "show-element": a0
      };
    };

    function LocationViewComponent_ng_template_49_div_9_div_1_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "input", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationViewComponent_ng_template_49_div_9_div_1_div_12_Template_input_ngModelChange_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r46);

          var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

          return ctx_r45.RequiredQuantity = $event;
        })("change", function LocationViewComponent_ng_template_49_div_9_div_1_div_12_Template_input_change_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r46);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

          return ctx_r47.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx_r43.ScheduleQuantityType > 1 ? true : null)("ngModel", ctx_r43.RequiredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c0, !ctx_r43.isValid));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r43.validateMsg, " ");
      }
    }

    function LocationViewComponent_ng_template_49_div_9_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "h3", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Create DR");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Quantity Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "select", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationViewComponent_ng_template_49_div_9_div_1_Template_select_ngModelChange_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r48.ScheduleQuantityType = $event;
        })("change", function LocationViewComponent_ng_template_49_div_9_div_1_Template_select_change_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          ctx_r50.RequiredQuantity = null;
          return ctx_r50.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, LocationViewComponent_ng_template_49_div_9_div_1_option_11_Template, 2, 2, "option", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, LocationViewComponent_ng_template_49_div_9_div_1_div_12_Template, 8, 6, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "input", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationViewComponent_ng_template_49_div_9_div_1_Template_input_ngModelChange_18_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r51.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "label", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, " Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "input", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationViewComponent_ng_template_49_div_9_div_1_Template_input_ngModelChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r52.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "label", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, " Should Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "input", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LocationViewComponent_ng_template_49_div_9_div_1_Template_input_ngModelChange_26_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r53.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, " Could Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "button", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationViewComponent_ng_template_49_div_9_div_1_Template_button_click_30_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r54.onDrSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, " Create ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r39.ScheduleQuantityType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r39.ScheduleQuantityTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r39.ScheduleQuantityType == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r39.DrPriority)("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r39.DrPriority)("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r39.DrPriority)("value", 3);
      }
    }

    function LocationViewComponent_ng_template_49_div_9_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function LocationViewComponent_ng_template_49_div_9_div_3_tr_27_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var del_r56 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r56.QuantityTypeId == 0 || del_r56.QuantityTypeId == 1 ? del_r56.Quantity : del_r56.QuantityTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r56.ScheduleDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r56.ScheduleTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r56.Driver);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](del_r56.Carrier);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "show": a0
      };
    };

    function LocationViewComponent_ng_template_49_div_9_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h2", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "button", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Existing Delivery Request(s) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "i", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](9, "i", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "table", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](19, "Schedule Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Schedule Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, LocationViewComponent_ng_template_49_div_9_div_3_tr_27_Template, 11, 5, "tr", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).modalDetails;

        var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](2, _c1, modalDetails_r37.IsScheduled));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r41.ExistingDeliveries);
      }
    }

    function LocationViewComponent_ng_template_49_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, LocationViewComponent_ng_template_49_div_9_div_1_Template, 32, 9, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, LocationViewComponent_ng_template_49_div_9_div_2_Template, 2, 0, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, LocationViewComponent_ng_template_49_div_9_div_3_Template, 28, 4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().modalDetails;

        var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !modalDetails_r37.IsScheduled);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r38.DRLoader);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r38.ExistingDeliveries.length);
      }
    }

    var _c2 = function _c2(a2) {
      return {
        "modal": true,
        "fade": true,
        "show": a2
      };
    };

    var _c3 = function _c3(a0) {
      return {
        "display": a0
      };
    };

    function LocationViewComponent_ng_template_49_Template(rf, ctx) {
      if (rf & 1) {
        var _r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h3", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LocationViewComponent_ng_template_49_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r60);

          var ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r59.closeModal();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "i", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, LocationViewComponent_ng_template_49_div_9_Template, 4, 3, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r37 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c2, modalDetails_r37.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](6, _c3, modalDetails_r37.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", modalDetails_r37.title, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", modalDetails_r37.display === "block");
      }
    }

    var LocationViewComponent = /*#__PURE__*/function () {
      function LocationViewComponent(dispatcherService, carrierService) {
        _classCallCheck(this, LocationViewComponent);

        this.dispatcherService = dispatcherService;
        this.carrierService = carrierService;
        this.LocationSchedules = [];
        this.IsLoading = false;
        this.showDr = false;
        this.IsDrExists = false;
        this.DRLoader = false;
        this.ExistingDeliveries = [];
        this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].MustGo;
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtOptions = {};
        this.SelectedPrioritiesId = [];
        this.dsModal = {
          modalDetails: {
            display: 'none',
            data: 'Modal Show',
            title: 'Delivery Schedule(s)',
            IsScheduled: false
          }
        };
        this.isValid = true;
        this.IsDataLoaded = false;
        this.ScheduleQuantityTypes = [];
        this.IsFilterLoaded = false;
        this.SelectedCustomers = [];
        this.SelectedLocations = [];
        this.SelectedRegions = [];
        this.SelectedPriorities = [];
        this.SelectedCarriers = [];
        this.IsShowCarrierManaged = false;
        this.SelectedStatus = [];
        this.IsShowRetailJobs = false;
        this.selectedLocAttributeList = [];
        this.getJobIdsForMap = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
      }

      _createClass(LocationViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeGrid();
          this.getScheduleQuantityType(); // this.getSalesData();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          var isFilterData = false; // if (change.IsFilterLoaded && change.IsFilterLoaded.currentValue) {
          //     this.IsFilterLoaded = change.IsFilterLoaded.currentValue;
          // }
          // if (change.SelectedRegions && change.SelectedRegions.currentValue) {
          //     this.SelectedRegions = change.SelectedRegions.currentValue;
          //     var ids = [];
          //     this.SelectedRegionId = '';
          //     this.SelectedRegions.forEach(res => { ids.push(res.Id) });
          //     this.SelectedRegionId = ids.join();
          //     if (change.SelectedRegions.previousValue) {
          //         var previousIds = [];
          //         change.SelectedRegions.previousValue.forEach(res => { previousIds.push(res.Id) });
          //         var previousRegionIds = previousIds.join();
          //         if (this.SelectedRegionId != previousRegionIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }
          // if (change.SelectedCustomers && change.SelectedCustomers.currentValue) {
          //     this.SelectedCustomers = change.SelectedCustomers.currentValue;
          //     var ids = [];
          //     this.SelectedCustomerId = '';
          //     this.SelectedCustomers.forEach(res => { ids.push(res.CompanyId) });
          //     this.SelectedCustomerId = ids.join();
          //     if (change.SelectedCustomers.previousValue) {
          //         var previousIds = [];
          //         change.SelectedCustomers.previousValue.forEach(res => { previousIds.push(res.CompanyId) });
          //         var previousCustomerIds = previousIds.join();
          //         if (this.SelectedCustomerId != previousCustomerIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }
          // if (change.SelectedLocations && change.SelectedLocations.currentValue) {
          //     this.SelectedLocations = change.SelectedLocations.currentValue;
          //     var ids = [];
          //     this.SelectedLocationId = '';
          //     this.SelectedLocations.forEach(res => { ids.push(res.Id) });
          //     this.SelectedLocationId = ids.join();
          //     if (change.SelectedLocations.previousValue) {
          //         var previousIds = [];
          //         change.SelectedLocations.previousValue.forEach(res => { previousIds.push(res.Id) });
          //         var previousLocationIds = previousIds.join();
          //         if (this.SelectedLocationId != previousLocationIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }

          if (change.IsShowCarrierManaged && change.IsShowCarrierManaged.currentValue != change.IsShowCarrierManaged.previousValue) {
            this.IsShowCarrierManaged = change.IsShowCarrierManaged.currentValue;
            isFilterData = true;
          }

          if (change.SelectedCarriers && change.SelectedCarriers.currentValue) {
            this.SelectedCarriers = change.SelectedCarriers.currentValue;
            var ids = [];
            this.SelectedCarrierIds = '';
            this.SelectedCarriers.forEach(function (res) {
              ids.push(res.Id);
            });
            this.SelectedCarrierIds = ids.join();

            if (change.SelectedCarriers.previousValue) {
              var previousIds = [];
              change.SelectedCarriers.previousValue.forEach(function (res) {
                previousIds.push(res.Id);
              });
              var previousCarrierIds = previousIds.join();

              if (this.SelectedCarrierIds != previousCarrierIds) {
                isFilterData = true;
              }
            } else {
              isFilterData = true;
            }
          } // if (change.SelectedStatus && change.SelectedStatus.currentValue) {
          //     this.SelectedStatus = change.SelectedStatus.currentValue;
          //     var ids = [];
          //     this.SelectedStatusId = '';
          //     this.SelectedStatus.forEach(res => { ids.push(res.Id) });
          //     this.SelectedStatusId = ids.join();
          //     if (change.SelectedStatus.previousValue) {
          //         var previousIds = [];
          //         change.SelectedStatus.previousValue.forEach(res => { previousIds.push(res.Id) });
          //         var previousStatusIds = previousIds.join();
          //         if (this.SelectedStatusId != previousStatusIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }
          // if (change.SelectedPriorities && change.SelectedPriorities.currentValue) {
          //     this.SelectedPriorities = change.SelectedPriorities.currentValue;
          //     var ids = [];
          //     this.SelectedPrioritiesId = '';
          //     this.SelectedPriorities.forEach(res => { ids.push(res.Id) });
          //     this.SelectedPrioritiesId = ids.join();
          //     if (change.SelectedPriorities.previousValue) {
          //         var previousIds = [];
          //         change.SelectedPriorities.previousValue.forEach(res => { previousIds.push(res.Id) });
          //         var previousPriorityIds = previousIds.join();
          //         if (this.SelectedPrioritiesId != previousPriorityIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }
          // if (change.selectedLocAttributeList && change.selectedLocAttributeList.currentValue) {
          //     var selectedLocAttributeList = change.selectedLocAttributeList.currentValue;
          //     var ids = [];
          //     this.selectedLocAttributeId = '';
          //     selectedLocAttributeList.forEach(res => { ids.push(res.Id) });
          //     this.selectedLocAttributeId = ids.join();
          //     if (change.selectedLocAttributeList.previousValue) {
          //         var previousIds = [];
          //         change.selectedLocAttributeList.previousValue.forEach(res => { previousIds.push(res.Id) });
          //         var previousLocAttributeIds = previousIds.join();
          //         if (this.selectedLocAttributeId != previousLocAttributeIds) {
          //             isFilterData = true;
          //         }
          //     } else {
          //         isFilterData = true;
          //     }
          // }


          if (change.IsShowRetailJobs && change.IsShowRetailJobs.currentValue != change.IsShowRetailJobs.previousValue) {
            this.IsShowRetailJobs = change.IsShowRetailJobs.currentValue;
            isFilterData = true;
          }

          if ((isFilterData || !this.IsDataLoaded) && this.IsFilterLoaded) {
            this.IsDataLoaded = true;
            this.getSalesData();
          }
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          if (this.dtTrigger) this.dtTrigger.unsubscribe();
        } // regionChanged(event?: any): void {
        //     this.filterData();
        // }
        // public onRegionSelect() {
        //     var ids = [];
        //     this.SelectedRegionId = '';
        //     this.SelectedRegions.forEach(res => { ids.push(res.Id) });
        //     this.SelectedRegionId = ids.join();
        //     this.regionChanged();
        // }
        // customerChanged(): void {
        //     this.filterData();
        // }
        // onPrioritySelect(event: any): void {
        //     this.filterData();
        // }
        // onPriorityUnselect(event: any): void {
        //     this.filterData();
        // }
        // filterData(): void {
        //     this.getSalesData();
        // }

      }, {
        key: "getScheduleQuantityType",
        value: function getScheduleQuantityType() {
          var _this33 = this;

          this.dispatcherService.GetScheduleQtyType().subscribe(function (SQT) {
            _this33.ScheduleQuantityTypes = SQT || [];
          });
        }
      }, {
        key: "initializeGrid",
        value: function initializeGrid() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Sales Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sales Details',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            fixedHeader: false,
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }]
          };
        }
      }, {
        key: "getSalesData",
        value: function getSalesData() {
          var _this34 = this;

          var inputs = {
            RegionId: this.SelectedRegionId,
            Priority: this.SelectedPrioritiesId,
            CustomerId: this.SelectedCustomerId,
            LocationId: this.SelectedLocationId,
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["SelectedTabEnum"].Location,
            Carriers: this.SelectedCarrierIds,
            IsShowCarrierManaged: this.IsShowCarrierManaged,
            IsShowRetailJobs: this.IsShowRetailJobs,
            InventoryCaptureType: this.selectedLocAttributeId
          };
          this.IsLoading = true;
          Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["forkJoin"])([this.dispatcherService.getSalesData(inputs), this.dispatcherService.GetRaisedExceptions()]).subscribe(function (resp) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this34, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
              var _this35 = this;

              return regeneratorRuntime.wrap(function _callee3$(_context3) {
                while (1) {
                  switch (_context3.prev = _context3.next) {
                    case 0:
                      _context3.next = 2;
                      return resp[0];

                    case 2:
                      _context3.t0 = _context3.sent;

                      if (!_context3.t0) {
                        _context3.next = 5;
                        break;
                      }

                      resp[0].map(function (m) {
                        if (resp[1] && resp[1].filter(function (f) {
                          return f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == m.SiteId && f.TankDetail.StorageId == m.StorageId;
                        }).length > 0) {
                          m.IsUnknownOrMissing = true;
                        } else m.IsUnknownOrMissing = false;
                      });

                    case 5:
                      if (this.SelectedStatus && this.SelectedStatus.length && resp[0]) {
                        resp[0] = resp[0].filter(function (t) {
                          return _this35.SelectedStatusId.includes(t.Status);
                        });
                      }

                      this.LocationSchedules = resp[0];
                      this.passJobIdsToMapData();
                      this.IsLoading = false;
                      this.datatableRerender();

                    case 10:
                    case "end":
                      return _context3.stop();
                  }
                }
              }, _callee3, this);
            }));
          }); // this.dispatcherService.getSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
          //   this.LocationSchedules = resp;
          //   this.IsLoading = false;
          //   this.datatableRerender();
          // });
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();
            });
          }

          this.dtTrigger.next();
        }
      }, {
        key: "openModal",
        value: function openModal(row) {
          var _this36 = this;

          this.resetModal();
          this.SelectedTank = row;
          this.DRLoader = true;
          this.dispatcherService.GetDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe(function (resp) {
            _this36.ExistingDeliveries = resp;
            _this36.DRLoader = false;
          });
          this.dsModal.modalDetails.display = 'block';
          var isSchedule = row.Status == 'Scheduled';
          this.dsModal.modalDetails.IsScheduled = isSchedule;
          this.showDr = isSchedule; //this.MaxFillQuantity = 120;
        }
      }, {
        key: "resetModal",
        value: function resetModal() {
          this.ExistingDeliveries = [];
          this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].MustGo;
          this.RequiredQuantity = null;
          this.ScheduleQuantityType = 1;
          this.validateMsg = "";
          this.isValid = true;
        }
      }, {
        key: "closeModal",
        value: function closeModal() {
          this.dsModal.modalDetails.display = 'none';
          this.isValid = true;
          $(".modal-backdrop").hide();
          $('body').removeClass('modal-open');
        }
      }, {
        key: "toggleDrs",
        value: function toggleDrs() {
          this.showDr = !this.showDr;
        }
      }, {
        key: "onDrSubmit",
        value: function onDrSubmit() {
          var _this37 = this;

          this.validateMsg = "";
          this.isValid = true;
          var raiseDr = {
            SiteId: this.SelectedTank.SiteId,
            TankId: this.SelectedTank.TankId,
            StorageId: this.SelectedTank.StorageId,
            RequiredQuantity: this.ScheduleQuantityType == 1 ? this.RequiredQuantity : 0,
            ScheduleQuantityType: this.ScheduleQuantityType,
            JobId: this.SelectedTank.TfxJobId,
            FuelTypeId: this.SelectedTank.ProductTypeId,
            Priority: this.DrPriority
          };

          if (this.ScheduleQuantityType == 1 && (!(this.RequiredQuantity > 0) || this.RequiredQuantity < 0.00001)) {
            this.validateMsg = "Invalid required quantity.";
            this.isValid = false;
          } else if (this.ScheduleQuantityType == 1 && this.SelectedTank.MaxFillQuantity && this.SelectedTank.MaxFillQuantity > 0 && this.RequiredQuantity > this.SelectedTank.MaxFillQuantity) {
            this.validateMsg = "Should not exceed max fill. (" + this.SelectedTank.MaxFillQuantity + ")";
            this.isValid = false;
          } else {
            this.DRLoader = true;
            this.dispatcherService.PostRaiseDeliveryRequest(raiseDr).subscribe(function (response) {
              if (response != null && response.StatusCode == 0) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
              } else {
                _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
              }

              _this37.DRLoader = false;

              _this37.closeModal();
            });
          }
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }, {
        key: "showTanks",
        value: function showTanks(row) {
          this.SelectedTankRegionId = row.RegionId;
          this.dipTestComponent.loadTankDR(row);
        }
      }, {
        key: "passJobIdsToMapData",
        value: function passJobIdsToMapData() {
          var jobsPriority = [];

          if (this.LocationSchedules) {
            this.LocationSchedules.forEach(function (res) {
              if (!jobsPriority.find(function (t) {
                return t.TfxJobId == res.TfxJobId;
              })) {
                jobsPriority.push({
                  TfxJobId: res.TfxJobId,
                  Priority: res.Priority
                });
              }
            });
            this.getJobIdsForMap.emit(jobsPriority);
          } else {
            this.getJobIdsForMap.emit(jobsPriority);
          }
        }
      }, {
        key: "applyFilters",
        value: function applyFilters(locationFilterModal) {
          if (locationFilterModal) {
            this.SelectedRegionId = locationFilterModal.SelectedRegionId;
            this.SelectedCustomerId = locationFilterModal.SelectedCustomerId;
            this.SelectedLocationId = locationFilterModal.SelectedLocationId;
            this.SelectedStatusId = locationFilterModal.SelectedStatusId;
            this.SelectedPrioritiesId = locationFilterModal.SelectedPrioritiesId;
            this.selectedLocAttributeId = locationFilterModal.selectedLocAttributeId;
            this.getSalesData();
          }
        }
      }]);

      return LocationViewComponent;
    }();

    LocationViewComponent.ɵfac = function LocationViewComponent_Factory(t) {
      return new (t || LocationViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_8__["CarrierService"]));
    };

    LocationViewComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: LocationViewComponent,
      selectors: [["app-location-view"]],
      viewQuery: function LocationViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dipTestComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
        }
      },
      inputs: {
        IsFilterLoaded: "IsFilterLoaded",
        SelectedCustomers: "SelectedCustomers",
        SelectedLocations: "SelectedLocations",
        SelectedRegions: "SelectedRegions",
        SelectedPriorities: "SelectedPriorities",
        SelectedCarriers: "SelectedCarriers",
        IsShowCarrierManaged: "IsShowCarrierManaged",
        SelectedStatus: "SelectedStatus",
        IsShowRetailJobs: "IsShowRetailJobs",
        selectedLocAttributeList: "selectedLocAttributeList"
      },
      outputs: {
        getJobIdsForMap: "getJobIdsForMap"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 53,
      vars: 10,
      consts: [[1, "row"], ["id", "grid-view", 1, "col-sm-12"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-Locationmustgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Cust"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "TName"], ["data-key", "WaterLevel"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["data-key", "Status"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["schedulesModal", ""], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "onRaiseDR"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], [4, "ngIf"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click", 4, "ngIf", "ngIfElse"], ["notSceduledBlock", ""], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click"], ["data-target", "raisedr", "onclick", "slidePanel('#raisedr','60%')", 3, "click"], ["id", "schedulesModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "schedulesModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered", "modal-lg"], [1, "modal-content"], [1, "modal-header", "pt10", "pb5", "no-border"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt5", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body"], ["class", "assets-header", 4, "ngIf"], [1, "assets-header"], ["class", "well bg-white no-shadow border border pr", 4, "ngIf"], [1, "well", "bg-white", "no-shadow", "border", "border", "pr"], [1, "col-sm-12"], [1, "fs14", "font-bold"], [1, "row", "col-sm-12"], [1, "col-sm-3", "input-group"], [1, "form-group", "mb0"], [1, "form-control", 3, "ngModel", "ngModelChange", "change"], [3, "value", 4, "ngFor", "ngForOf"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-6", "mt5"], [1, "col-sm-12", "pa0", "mt5"], [1, "form-check", "form-check-inline"], ["id", "mustgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "mustgo-dr", 1, "form-check-label"], ["id", "shouldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "shouldgo-dr", 1, "form-check-label"], ["id", "couldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "couldgo-dr", 1, "form-check-label"], [1, "col-sm-12", "text-right", "mt10"], ["type", "button", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [3, "value"], [1, "col-sm-3"], [1, "input-group"], ["type", "text", "name", "RequiredQuantity", "numberWithDecimal", "", "required", "", 1, "form-control", 3, "disabled", "ngModel", "ngModelChange", "change"], [1, "invalid-feedback", 3, "ngClass"], ["id", "accordionExitingDrReq", 1, "accordionExitingDrReq", "mt10", "mb10"], [1, "card"], ["id", "headingOne", 1, "card-header", "pt5", "pb5", "pl10", "pr10"], [1, "mb-0"], ["type", "button", "data-toggle", "collapse", "data-target", "#collapseOne", "aria-expanded", "true", "aria-controls", "collapseOne", 1, "d-flex", "align-items-center", "justify-content-between", "btn", "btn-link", "collapsed"], [1, "fa-stack", "fa-sm", "icon-color-b"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-angle-down", "fa-stack-1x", "fa-inverse"], ["id", "collapseOne", "aria-labelledby", "headingOne", "data-parent", "#accordionExitingDrReq", 1, "collapse", 3, "ngClass"], [1, "card-body", "pa5"], [1, "table", "table-hover", "margin", "bottom", "details-table"]],
      template: function LocationViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Location View");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "table", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Customer");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Tank/Asset Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "WaterLevel");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, LocationViewComponent_tr_46_Template, 2, 0, "tr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](47, LocationViewComponent_tr_47_Template, 48, 32, "tr", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](48, LocationViewComponent_ng_container_48_Template, 1, 0, "ng-container", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](49, LocationViewComponent_ng_template_49_Template, 10, 8, "ng-template", null, 26, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "app-dip-test", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onRaiseDR", function LocationViewComponent_Template_app_dip_test_onRaiseDR_52_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.LocationSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r3)("ngTemplateOutletContext", ctx.dsModal);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("isDisableControl", false)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedTankRegionId)("IsThisFromDrDisplay", false);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgTemplateOutlet"], _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgStyle"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["ɵangular_packages_forms_forms_x"], _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_12__["NumberWithDecimal"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RequiredValidator"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["DecimalPipe"]],
      styles: [".active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\r\n\r\n@keyframes blink {\r\n    from,to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n\r\n@-webkit-keyframes blink {\r\n    from, to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL2xvY2F0aW9uLXZpZXcuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQ0FBQztJQUNHLFlBQVk7SUFDWixXQUFXO0lBQ1gseUJBQXlCO0lBQ3pCLGtCQUFrQjtJQUNsQixxQkFBcUI7SUFDckIseUNBQWlDO1lBQWpDLGlDQUFpQztBQUNyQzs7QUFFQTtJQUNJO1FBQ0ksVUFBVTtJQUNkOztJQUVBO1FBQ0ksVUFBVTtJQUNkO0FBQ0o7O0FBWUE7SUFDSTtRQUNJLFVBQVU7SUFDZDs7SUFFQTtRQUNJLFVBQVU7SUFDZDtBQUNKIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL2xvY2F0aW9uLXZpZXcuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIiAuYWN0aXZlLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmZjU4NTg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcbkBrZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSx0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtbW96LWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtd2Via2l0LWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtbXMta2V5ZnJhbWVzIGJsaW5rIHtcclxuICAgIGZyb20sIHRvIHtcclxuICAgICAgICBvcGFjaXR5OiAwO1xyXG4gICAgfVxyXG5cclxuICAgIDUwJSB7XHJcbiAgICAgICAgb3BhY2l0eTogMTtcclxuICAgIH1cclxufVxyXG5cclxuQC1vLWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcblxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](LocationViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-location-view',
          templateUrl: './location-view.component.html',
          styleUrls: ['./location-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_8__["CarrierService"]
        }];
      }, {
        IsFilterLoaded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedCustomers: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedLocations: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedRegions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedPriorities: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedCarriers: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsShowCarrierManaged: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        SelectedStatus: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsShowRetailJobs: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        selectedLocAttributeList: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        getJobIdsForMap: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_5__["DipTestComponent"]]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/sales-data/sales-data.component.ts": function srcAppDispatcherDispatcherDashboardSalesDataSalesDataComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesDataComponent", function () {
      return SalesDataComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _buyer_wally_board_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../../../buyer-wally-board/Models/BuyerWallyBoard */
    "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/service/wally-utility.service */
    "./src/app/carrier/service/wally-utility.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _grid_view_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./grid-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/grid-view.component.ts");
    /* harmony import */


    var _tank_view_master_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./tank-view-master.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view-master.component.ts");

    function SalesDataComponent_div_16_ng_template_3_ng_multiselect_dropdown_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "ng-multiselect-dropdown", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function SalesDataComponent_div_16_ng_template_3_ng_multiselect_dropdown_1_Template_ng_multiselect_dropdown_onSelect_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

          return ctx_r10.onCarrierChange();
        })("onDeSelect", function SalesDataComponent_div_16_ng_template_3_ng_multiselect_dropdown_1_Template_ng_multiselect_dropdown_onDeSelect_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

          return ctx_r12.onCarrierChange();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r9.CarrierDdlSettings)("placeholder", "Select Carrier")("data", ctx_r9.carrierList);
      }
    }

    function SalesDataComponent_div_16_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SalesDataComponent_div_16_ng_template_3_ng_multiselect_dropdown_1_Template, 1, 3, "ng-multiselect-dropdown", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r8.salesTabFilterForm.get("IsShowCarrierManaged").value);
      }
    }

    function SalesDataComponent_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SalesDataComponent_div_16_ng_template_3_Template, 2, 1, "ng-template", null, 21, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r7)("autoClose", "outside");
      }
    }

    function SalesDataComponent_span_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r2.filterCount);
      }
    }

    function SalesDataComponent_app_grid_view_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-grid-view", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("salesTabFilterForm", ctx_r3.salesTabFilterForm);
      }
    }

    function SalesDataComponent_app_tank_view_master_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-tank-view-master", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("salesTabFilterForm", ctx_r4.salesTabFilterForm);
      }
    }

    function SalesDataComponent_ng_template_28_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "ng-multiselect-dropdown", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r13.RegionDdlSettings)("placeholder", "Select Location")("data", ctx_r13.locationList);
      }
    }

    function SalesDataComponent_ng_template_28_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Prority");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "ng-multiselect-dropdown", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r14.PriorityDdlSettings)("placeholder", "Select Priority")("data", ctx_r14.LoadPriorities);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "border-bottom-2": a0
      };
    };

    function SalesDataComponent_ng_template_28_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "ng-multiselect-dropdown", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function SalesDataComponent_ng_template_28_Template_ng_multiselect_dropdown_onSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r15.regionChanged();
        })("onDeSelect", function SalesDataComponent_ng_template_28_Template_ng_multiselect_dropdown_onDeSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r17.regionChanged();
        })("onSelectAll", function SalesDataComponent_ng_template_28_Template_ng_multiselect_dropdown_onSelectAll_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r18.regionChanged();
        })("onDeSelectAll", function SalesDataComponent_ng_template_28_Template_ng_multiselect_dropdown_onDeSelectAll_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r19.regionChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "label", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "ng-multiselect-dropdown", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, SalesDataComponent_ng_template_28_div_13_Template, 5, 3, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, SalesDataComponent_ng_template_28_div_14_Template, 5, 3, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "label", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Inventory Data Capture");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](20, "ng-multiselect-dropdown", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "button", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SalesDataComponent_ng_template_28_Template_button_click_23_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          ctx_r20.ResetFilters();
          return ctx_r20.SaveFilters(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "button", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SalesDataComponent_ng_template_28_Template_button_click_25_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](20);

          ctx_r21.ApplyFilters("set");
          ctx_r21.SaveFilters(false);
          return _r1.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx_r6.salesTabFilterForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r6.RegionDdlSettings)("placeholder", "Select Region")("data", ctx_r6.Regions);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r6.CustomerDdlSettings)("placeholder", "Select Customer")("data", ctx_r6.customerList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](13, _c0, !ctx_r6.salesTabFilterForm.get("SalesViewType").value));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.salesTabFilterForm.get("SalesViewType").value != 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.salesTabFilterForm.get("SalesViewType").value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r6.PriorityDdlSettings)("placeholder", "Inventory Data Capture")("data", ctx_r6.LocationAttributeList);
      }
    }

    var SalesDataComponent = /*#__PURE__*/function () {
      function SalesDataComponent(dispatcherService, carrierService, wallyUtilService) {
        _classCallCheck(this, SalesDataComponent);

        this.dispatcherService = dispatcherService;
        this.carrierService = carrierService;
        this.wallyUtilService = wallyUtilService;
        this.toogleMap = true;
        this.toogleFilter = false;
        this.toogleDriver = false;
        this.toogleGrid = false;
        this.toogleExpandMap = false;
        this.RegionDdlSettings = {};
        this.CustomerDdlSettings = {};
        this.Regions = [];
        this.customerList = [];
        this.locationList = [];
        this.loadingData = true;
        this.PriorityDdlSettings = {};
        this.CarrierDdlSettings = {};
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_3__["LoadPriorities"];
        this.carrierList = [];
        this.LocationAttributeList = src_app_app_constants__WEBPACK_IMPORTED_MODULE_3__["InventoryDataCaptureList"];
        this.filterCount = 0;
        this.salesTabFilterForm = this.wallyUtilService.getSalesTabFilterForm();
      }

      _createClass(SalesDataComponent, [{
        key: "_selectedlocationList",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedlocationList').value;
        }
      }, {
        key: "_selectedCustomerList",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedCustomerList').value;
        }
      }, {
        key: "_selectedRegions",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedRegions').value;
        }
      }, {
        key: "_selectedPriorities",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedPriorities').value;
        }
      }, {
        key: "_selectedLocAttributeList",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedLocAttributeList').value;
        }
      }, {
        key: "_selectedCarriers",
        get: function get() {
          return this.salesTabFilterForm.get('SelectedCarriers').value;
        }
      }, {
        key: "ngOnInit",
        value: function ngOnInit() {
          this.init();
        }
      }, {
        key: "init",
        value: function init() {
          this.RegionDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.CustomerDdlSettings = {
            singleSelection: false,
            idField: 'CompanyId',
            textField: 'CompanyName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.PriorityDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.CarrierDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.getRegions();
          this.getCarriers();
          this.GetFilters();
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "regionChanged",
        value: function regionChanged() {
          var _rgnIds = this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value);

          if (_rgnIds) this.getCustomerListByRegionId(_rgnIds);else this.initAllCustomers();
        }
      }, {
        key: "getCustomerListByRegionId",
        value: function getCustomerListByRegionId(SelectedRegionId, isShowAllLoc) {
          var _this38 = this;

          this.loadingData = true;
          this.carrierService.getJobListForCarrier(SelectedRegionId, this.salesTabFilterForm.get('IsShowCarrierManaged').value, this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value)).subscribe(function (response) {
            _this38.loadingData = false;
            _this38.customerList = response;

            var SelectedCustomerList = _this38._selectedCustomerList.filter(function (t) {
              return _this38.customerList.filter(function (el) {
                return el.CompanyId == t.CompanyId;
              }).length > 0;
            });

            _this38.salesTabFilterForm.get('SelectedCustomerList').setValue(SelectedCustomerList || []);

            if (isShowAllLoc) _this38.initAllLocation();
          });
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this39 = this;

          this.dispatcherService.GetCarriersForSupplier().subscribe(function (data) {
            _this39.carrierList = data;
          });
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this40 = this;

          this.dispatcherService.GetDispatcherRegions().subscribe(function (data) {
            _this40.Regions = data;

            _this40.initAllCustomers();
          });
        }
      }, {
        key: "initAllCustomers",
        value: function initAllCustomers() {
          this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);
        }
      }, {
        key: "initAllLocation",
        value: function initAllLocation() {
          var _this41 = this;

          this.locationList = [];
          this.customerList.forEach(function (res) {
            _this41.locationList = _this41.locationList.concat(res.Jobs);
          });

          var SelectedlocationList = this._selectedlocationList.filter(function (t) {
            return _this41.locationList.filter(function (el) {
              return el.JobID == t.JobID;
            }).length > 0;
          });

          this.salesTabFilterForm.get('SelectedlocationList').setValue(SelectedlocationList || []);
        }
      }, {
        key: "onCustomerSelect",
        value: function onCustomerSelect() {
          var _this42 = this;

          if (this._selectedCustomerList && this._selectedCustomerList.length > 0) {
            this._selectedCustomerList.forEach(function (res) {
              var _cust = _this42.customerList.find(function (f) {
                return f.CompanyId == res.CompanyId;
              });

              if (_cust && _cust.Jobs) {
                _this42.locationList = _this42.locationList.concat(_cust.Jobs);
              }
            });

            var SelectedlocationList = this._selectedlocationList.filter(function (t) {
              return _this42.locationList.filter(function (el) {
                return el.JobID == t.JobID;
              }).length > 0;
            });

            this.salesTabFilterForm.get('SelectedlocationList').setValue(SelectedlocationList || []);
          } else {
            this.initAllLocation();
          }
        }
      }, {
        key: "onCarrierChange",
        value: function onCarrierChange() {
          this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);
          this.ApplyFilters();
          this.SaveFilters(true);
        }
      }, {
        key: "ShowCarrierMangedData",
        value: function ShowCarrierMangedData() {
          this.salesTabFilterForm.get('SelectedCarriers').setValue([]);
          this.salesTabFilterForm.get('SelectedRegions').setValue([]);
          this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);
          this.ApplyFilters();
          this.SaveFilters(true);
        }
      }, {
        key: "SaveFilters",
        value: function SaveFilters(isTopFilter) {
          var _this43 = this;

          var filterData = {};
          this.dispatcherService.getFilters(_buyer_wally_board_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].SupplierWallyboardSales).subscribe(function (res) {
            var input;

            if (res || res == "") {
              if (res != "") {
                input = JSON.parse(res);
                filterData = input;
              }

              if (isTopFilter) {
                filterData['isShowCarrierManaged'] = _this43.salesTabFilterForm.get('IsShowCarrierManaged').value;
                filterData['SelectedCarriers'] = _this43._selectedCarriers;
              } else {
                filterData['SelectedRegions'] = _this43._selectedRegions || [];
                filterData['SelectedCustomerList'] = _this43._selectedCustomerList || [];
                filterData['SelectedlocationList'] = _this43._selectedlocationList || [];
                filterData['SelectedPriorities'] = _this43._selectedPriorities || [];
                filterData['selectedLocAttributeList'] = _this43._selectedLocAttributeList || [];
                filterData['isShowCarrierManaged'] = _this43.salesTabFilterForm.get('IsShowCarrierManaged').value;
                filterData['SelectedCarriers'] = _this43._selectedCarriers;
              }

              _this43.dispatcherService.postFiltersData(_buyer_wally_board_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].SupplierWallyboardSales, JSON.stringify(filterData)).subscribe();
            }
          });
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this44 = this;

          this.dispatcherService.getFilters(_buyer_wally_board_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].SupplierWallyboardSales).subscribe(function (res) {
            if (res && res.length > 0) {
              _this44.SetFilters(res);
            }
          });
        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          var filterData = JSON.parse(input);
          this.salesTabFilterForm.get('SelectedlocationList').setValue(filterData.SelectedlocationList || []);
          this.salesTabFilterForm.get('SelectedCustomerList').setValue(filterData.SelectedCustomerList || []);
          this.salesTabFilterForm.get('SelectedRegions').setValue(filterData.SelectedRegions || []);
          this.salesTabFilterForm.get('SelectedPriorities').setValue(filterData.SelectedPriorities || []);
          this.salesTabFilterForm.get('SelectedLocAttributeList').setValue(filterData.selectedLocAttributeList || []);
          this.salesTabFilterForm.get('SelectedCarriers').setValue(filterData.SelectedCarriers || []);
          this.salesTabFilterForm.get('IsShowCarrierManaged').setValue(filterData.isShowCarrierManaged);
          this.ApplyFilters();

          if (this._selectedRegions && this._selectedRegions.length > 0) {
            this.regionChanged();
          } else if (this._selectedCustomerList && this._selectedCustomerList.length > 0) {
            this.onCustomerSelect();
          }
        }
      }, {
        key: "ResetFilters",
        value: function ResetFilters() {
          this.salesTabFilterForm.get('SelectedRegions').setValue([]);
          this.salesTabFilterForm.get('SelectedCustomerList').setValue([]);
          this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
          this.salesTabFilterForm.get('SelectedPriorities').setValue([]);
          this.salesTabFilterForm.get('SelectedLocAttributeList').setValue([]);
          this.ApplyFilters('reset');
        }
      }, {
        key: "ApplyFilters",
        value: function ApplyFilters(msg) {
          this.salesTabFilterForm.get('IsApplyFilter').setValue(true);

          if (msg == "set") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Filter applied successfully", undefined, undefined);
          } else if (msg == "reset") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msginfo("Filter reset successfully", undefined, undefined);
          }

          this.setFilterCount();
        }
      }, {
        key: "setFilterCount",
        value: function setFilterCount() {
          this.filterCount = 0;

          if (this.salesTabFilterForm.get('SalesViewType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["SalesTabViewType"].PriorityTab) {
            this.filterCount += this._selectedRegions.length;
            this.filterCount += this._selectedCustomerList.length;
            this.filterCount += this._selectedlocationList.length;
            this.filterCount += this._selectedPriorities.length;
            this.filterCount += this._selectedLocAttributeList.length;
          } else if (this.salesTabFilterForm.get('SalesViewType').value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["SalesTabViewType"].TanksTab) {
            this.filterCount += this._selectedRegions.length;
            this.filterCount += this._selectedCustomerList.length;
            this.filterCount += this._selectedLocAttributeList.length;
          }
        }
      }, {
        key: "salesViewTypeChanged",
        value: function salesViewTypeChanged(type) {
          this.salesTabFilterForm.get('SalesViewType').setValue(type);
          this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(true);
          this.setFilterCount();
        }
      }]);

      return SalesDataComponent;
    }();

    SalesDataComponent.ɵfac = function SalesDataComponent_Factory(t) {
      return new (t || SalesDataComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_6__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__["WallyUtilService"]));
    };

    SalesDataComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: SalesDataComponent,
      selectors: [["app-sales-data"]],
      decls: 30,
      vars: 13,
      consts: [[1, "col-sm-9", "sticky-header-sales"], [1, "row"], [1, "col"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "pull-left", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-4", 3, "formGroup"], [1, "form-check", "form-check-inline", "fs14", "mt10"], ["type", "checkbox", "id", "inlineCarrierManaged", "name", "IsShowCarrierManaged", "formControlName", "IsShowCarrierManaged", 1, "form-check-input", 3, "change"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["class", "mtm5", 4, "ngIf"], [1, "col", "pt5"], [1, "col", "pl0", "text-right", "pt8"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], [1, "col-sm-12"], [3, "salesTabFilterForm", 4, "ngIf"], ["popContent", ""], [1, "mtm5"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "mb10", "p-0"], ["formControlName", "SelectedCarriers", 3, "settings", "placeholder", "data", "onSelect", "onDeSelect", 4, "ngIf"], ["formControlName", "SelectedCarriers", 3, "settings", "placeholder", "data", "onSelect", "onDeSelect"], [1, "circle-badge"], [3, "salesTabFilterForm"], [1, "popover-details", 3, "formGroup"], [1, "row", "border-bottom-2"], [1, "col-6", "pr-0"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], ["formControlName", "SelectedRegions", 3, "settings", "placeholder", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "col-6"], ["formControlName", "SelectedCustomerList", 3, "settings", "placeholder", "data"], [1, "row", "mt10", 3, "ngClass"], ["class", "col-6 pr-0", 4, "ngIf"], ["class", "col-6", 4, "ngIf"], [1, "row", "border-bottom-2", "mt10"], ["formControlName", "SelectedLocAttributeList", 3, "settings", "placeholder", "data"], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"], ["formControlName", "SelectedlocationList", 3, "settings", "placeholder", "data"], ["formControlName", "SelectedPriorities", 3, "settings", "placeholder", "data"]],
      template: function SalesDataComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SalesDataComponent_Template_label_click_6_listener() {
            return ctx.salesViewTypeChanged(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Priority");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SalesDataComponent_Template_label_click_9_listener() {
            return ctx.salesViewTypeChanged(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Tanks");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SalesDataComponent_Template_input_change_13_listener() {
            return ctx.ShowCarrierMangedData();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, SalesDataComponent_div_16_Template, 5, 2, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](17, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "a", 15, 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SalesDataComponent_Template_a_click_19_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

            var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](20);

            return _r1.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "i", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, SalesDataComponent_span_22_Template, 2, 1, "span", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, " Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, SalesDataComponent_app_grid_view_26_Template, 2, 1, "app-grid-view", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, SalesDataComponent_app_tank_view_master_27_Template, 2, 1, "app-tank-view-master", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, SalesDataComponent_ng_template_28_Template, 27, 15, "ng-template", null, 21, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
        }

        if (rf & 2) {
          var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "saletype")("value", 1)("checked", ctx.salesTabFilterForm.get("SalesViewType").value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "saletype")("value", 2)("checked", ctx.salesTabFilterForm.get("SalesViewType").value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.salesTabFilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.salesTabFilterForm.get("IsShowCarrierManaged").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r5)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.filterCount > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.salesTabFilterForm.get("SalesViewType").value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.salesTabFilterForm.get("SalesViewType").value == 2);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbPopover"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["MultiSelectComponent"], _grid_view_component__WEBPACK_IMPORTED_MODULE_12__["GridViewComponent"], _tank_view_master_component__WEBPACK_IMPORTED_MODULE_13__["TankViewMasterComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"]],
      styles: [".sticky-header-sales {\n  position: fixed;\n  right: 0;\n  padding: 10px 20px;\n  top: 45px;\n  height: 65px;\n  /*font-size: 20px;*/\n  z-index: 10;\n  background: #fff;\n}\n\n.locationfilter {\n  width: 100%;\n  position: absolute;\n  right: 4px;\n  border-radius: 5px;\n  font-size: 14px;\n  z-index: 1010;\n}\n\n.sticky_header {\n  position: sticky;\n  top: 45px;\n  padding: 5px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n  margin-bottom: 0px;\n  margin-top: -10px;\n  /*box-shadow: 0 3px 15px 0 rgba(0,0,0,.1);*/\n  border-radius: 2px;\n}\n\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.carrier-popover.popover .popover-body {\n  /* min-height: 310px; */\n  padding: 10px;\n  border-radius: 5px;\n}\n\n.activediff-dot {\n  height: 10px;\n  width: 10px;\n  background-color: #585bff;\n  border-radius: 50%;\n  display: inline-block;\n  -webkit-animation: 1s blink ease infinite;\n          animation: 1s blink ease infinite;\n}\n\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL0Q6XFxURlNjb2RlXFxTaXRlRnVlbC5FeGNoYW5nZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuU291cmNlQ29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuV2ViL3NyY1xcYXBwXFxkaXNwYXRjaGVyXFxkaXNwYXRjaGVyLWRhc2hib2FyZFxcc2FsZXMtZGF0YVxcc2FsZXMtZGF0YS5jb21wb25lbnQuc2NzcyIsInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL3NhbGVzLWRhdGEuY29tcG9uZW50LnNjc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7RUFDSSxlQUFBO0VBQ0EsUUFBQTtFQUNBLGtCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxtQkFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtBQ0NKOztBREVBO0VBQ0ksV0FBQTtFQUNBLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGFBQUE7QUNDSjs7QURFQTtFQUVJLGdCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxpQkFBQTtFQUNBLDJDQUFBO0VBQ0Esa0JBQUE7QUNDSjs7QURFQTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDQ0o7O0FERUE7RUFDSSx1QkFBQTtFQUNBLGFBQUE7RUFDQSxrQkFBQTtBQ0NKOztBREVBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSx5QkFBQTtFQUNBLGtCQUFBO0VBQ0EscUJBQUE7RUFDQSx5Q0FBQTtVQUFBLGlDQUFBO0FDQ0o7O0FER0k7RUFDSSxnQkFBQTtFQUNBLGdCQUFBO0VBQ0EsbUJBQUE7RUFDQSx5QkFBQTtFQUNBLHNCQUFBO0VBQ0Esa0RBQUE7RUFDQSxtQkFBQTtBQ0FSOztBREVRO0VBSUksVUFBQTtFQUNBLGtCQUFBO0VBQ0EsbUJBQUE7QUNIWjs7QURNUTtFQUNJLGFBQUE7QUNKWjs7QURPWTtFQUNJLDJCQUFBO0FDTGhCOztBRFNRO0VBQ0ksMkNBQUE7QUNQWjs7QURZQTtFQUNDLGtCQUFBO0VBQ0csVUFBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtFQUNBLG9CQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtFQUNILFdBQUE7RUFDRyxZQUFBO0FDVEoiLCJmaWxlIjoic3JjL2FwcC9kaXNwYXRjaGVyL2Rpc3BhdGNoZXItZGFzaGJvYXJkL3NhbGVzLWRhdGEvc2FsZXMtZGF0YS5jb21wb25lbnQuc2NzcyIsInNvdXJjZXNDb250ZW50IjpbIi5zdGlja3ktaGVhZGVyLXNhbGVzIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgcGFkZGluZzogMTBweCAyMHB4O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgLypmb250LXNpemU6IDIwcHg7Ki9cclxuICAgIHotaW5kZXg6IDEwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxufVxyXG5cclxuLmxvY2F0aW9uZmlsdGVyIHtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgcmlnaHQ6IDRweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMTRweDtcclxuICAgIHotaW5kZXg6IDEwMTA7XHJcbn1cclxuXHJcbi5zdGlja3lfaGVhZGVyIHtcclxuICAgIHBvc2l0aW9uOiAtd2Via2l0LXN0aWNreTtcclxuICAgIHBvc2l0aW9uOiBzdGlja3k7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBwYWRkaW5nOiA1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiAxMDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbiAgICBtYXJnaW4tdG9wOiAtMTBweDtcclxuICAgIC8qYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwwLDAsLjEpOyovXHJcbiAgICBib3JkZXItcmFkaXVzOiAycHg7XHJcbn1cclxuXHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciB7XHJcbiAgICBtaW4td2lkdGg6IDMwMHB4O1xyXG4gICAgbWF4LXdpZHRoOiAzNTBweDtcclxuICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xyXG4gICAgYm94LXNpemluZzogYm9yZGVyLWJveDtcclxuICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG59XHJcblxyXG4uY2Fycmllci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XHJcbiAgICAvKiBtaW4taGVpZ2h0OiAzMTBweDsgKi9cclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuXHJcbi5hY3RpdmVkaWZmLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICM1ODViZmY7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcbi5tYXN0ZXItZmlsdGVyIHtcclxuICAgICYucG9wb3ZlciB7XHJcbiAgICAgICAgbWluLXdpZHRoOiA0MjVweDtcclxuICAgICAgICBtYXgtd2lkdGg6IDQ1MHB4O1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICAgICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgICAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICAgICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuXHJcbiAgICAgICAgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgICAgIC8vIG1heC1oZWlnaHQ6IDM1MHB4O1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy14OiBoaWRkZW47XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDA7XHJcbiAgICAgICAgICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgICAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC5wb3BvdmVyLWRldGFpbHMge1xyXG4gICAgICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgICAgICAgICAvLyBtYXgtaGVpZ2h0OiAzMTBweDtcclxuICAgICAgICAgICAgLy8gb3ZlcmZsb3cteTogYXV0bztcclxuICAgICAgICAgICAgLmZvbnQtYm9sZCB7XHJcbiAgICAgICAgICAgICAgICBmb250LXdlaWdodDogNjAwICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgICAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2U3ZWFlYyAhaW1wb3J0YW50O1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufVxyXG5cclxuLmNpcmNsZS1iYWRnZXtcclxuXHRwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IC0xMXB4O1xyXG4gICAgbGVmdDogLTE0cHg7XHJcbiAgICBiYWNrZ3JvdW5kOiByZ2IoMjUwLCAxNDcsIDE0Nyk7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBmb250LXNpemU6IDEycHg7XHJcbiAgICB0ZXh0LWFsaWduOiBjZW50ZXI7XHJcbiAgICBjb2xvcjogd2hpdGU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtZmxleDtcclxuICAgIGFsaWduLWl0ZW1zOiBjZW50ZXI7XHJcbiAgICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcclxuXHR3aWR0aDogMThweDtcclxuICAgIGhlaWdodDogMThweFxyXG59IiwiLnN0aWNreS1oZWFkZXItc2FsZXMge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHJpZ2h0OiAwO1xuICBwYWRkaW5nOiAxMHB4IDIwcHg7XG4gIHRvcDogNDVweDtcbiAgaGVpZ2h0OiA2NXB4O1xuICAvKmZvbnQtc2l6ZTogMjBweDsqL1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbn1cblxuLmxvY2F0aW9uZmlsdGVyIHtcbiAgd2lkdGg6IDEwMCU7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgcmlnaHQ6IDRweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBmb250LXNpemU6IDE0cHg7XG4gIHotaW5kZXg6IDEwMTA7XG59XG5cbi5zdGlja3lfaGVhZGVyIHtcbiAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xuICBwb3NpdGlvbjogc3RpY2t5O1xuICB0b3A6IDQ1cHg7XG4gIHBhZGRpbmc6IDVweDtcbiAgZm9udC1zaXplOiAyMHB4O1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbiAgbWFyZ2luLWJvdHRvbTogMHB4O1xuICBtYXJnaW4tdG9wOiAtMTBweDtcbiAgLypib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7Ki9cbiAgYm9yZGVyLXJhZGl1czogMnB4O1xufVxuXG4uY2Fycmllci1wb3BvdmVyLnBvcG92ZXIge1xuICBtaW4td2lkdGg6IDMwMHB4O1xuICBtYXgtd2lkdGg6IDM1MHB4O1xuICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xuICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xuICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiYSgwLCAwLCAwLCAwLjEzKTtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbn1cblxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWJvZHkge1xuICAvKiBtaW4taGVpZ2h0OiAzMTBweDsgKi9cbiAgcGFkZGluZzogMTBweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xufVxuXG4uYWN0aXZlZGlmZi1kb3Qge1xuICBoZWlnaHQ6IDEwcHg7XG4gIHdpZHRoOiAxMHB4O1xuICBiYWNrZ3JvdW5kLWNvbG9yOiAjNTg1YmZmO1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcbiAgYW5pbWF0aW9uOiAxcyBibGluayBlYXNlIGluZmluaXRlO1xufVxuXG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIHtcbiAgbWluLXdpZHRoOiA0MjVweDtcbiAgbWF4LXdpZHRoOiA0NTBweDtcbiAgYmFja2dyb3VuZDogI0Y5RjlGOTtcbiAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcbiAgYm94LXNpemluZzogYm9yZGVyLWJveDtcbiAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYmEoMCwgMCwgMCwgMC4xMyk7XG4gIGJvcmRlci1yYWRpdXM6IDEwcHg7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWJvZHkge1xuICBwYWRkaW5nOiAwO1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIGJhY2tncm91bmQ6ICNmZmZmZmY7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMge1xuICBwYWRkaW5nOiAxNXB4O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5mb250LWJvbGQge1xuICBmb250LXdlaWdodDogNjAwICFpbXBvcnRhbnQ7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xuICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2U3ZWFlYyAhaW1wb3J0YW50O1xufVxuXG4uY2lyY2xlLWJhZGdlIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IC0xMXB4O1xuICBsZWZ0OiAtMTRweDtcbiAgYmFja2dyb3VuZDogI2ZhOTM5MztcbiAgYm9yZGVyLXJhZGl1czogNTAlO1xuICBmb250LXNpemU6IDEycHg7XG4gIHRleHQtYWxpZ246IGNlbnRlcjtcbiAgY29sb3I6IHdoaXRlO1xuICBkaXNwbGF5OiBpbmxpbmUtZmxleDtcbiAgYWxpZ24taXRlbXM6IGNlbnRlcjtcbiAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XG4gIHdpZHRoOiAxOHB4O1xuICBoZWlnaHQ6IDE4cHg7XG59Il19 */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SalesDataComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-sales-data',
          templateUrl: './sales-data.component.html',
          styleUrls: ['./sales-data.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__["DispatcherService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_6__["CarrierService"]
        }, {
          type: src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__["WallyUtilService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view-master.component.ts": function srcAppDispatcherDispatcherDashboardSalesDataTankViewMasterComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankViewMasterComponent", function () {
      return TankViewMasterComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _tank_view_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./tank-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view.component.ts");
    /* harmony import */


    var _shared_components_forcasting_tank_view_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../../shared-components/forcasting/tank-view-component */
    "./src/app/shared-components/forcasting/tank-view-component.ts");

    function TankViewMasterComponent_app_tank_view_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-tank-view", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("salesTabFilterForm", ctx_r0.salesTabFilterForm);
      }
    }

    function TankViewMasterComponent_app_forecasting_tank_view_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-forecasting-tank-view", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("RequestFromBuyerWallyBoard", false)("salesTabFilterForm", ctx_r1.salesTabFilterForm);
      }
    }

    var TankViewMasterComponent = /*#__PURE__*/function () {
      function TankViewMasterComponent(dispatcherService) {
        _classCallCheck(this, TankViewMasterComponent);

        this.dispatcherService = dispatcherService;
      }

      _createClass(TankViewMasterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.verifyForcastingAccountLevelSetting();
        }
      }, {
        key: "verifyForcastingAccountLevelSetting",
        value: function verifyForcastingAccountLevelSetting() {
          var _this45 = this;

          this.dispatcherService.getForcastingSetting().subscribe(function (resp) {
            if (resp && resp.IsForecatingAccountLevel == 1) {
              _this45.salesTabFilterForm.get('RateOfConsumption').setValue(true);
            } else {
              _this45.salesTabFilterForm.get('RateOfConsumption').setValue(false);
            }
          });
        }
      }]);

      return TankViewMasterComponent;
    }();

    TankViewMasterComponent.ɵfac = function TankViewMasterComponent_Factory(t) {
      return new (t || TankViewMasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_1__["DispatcherService"]));
    };

    TankViewMasterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: TankViewMasterComponent,
      selectors: [["app-tank-view-master"]],
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 10,
      vars: 3,
      consts: [[1, "col-sm-12", 3, "formGroup"], [1, "row", "mt60"], [1, "col-sm-12", "text-center"], [1, "custom-control", "custom-switch", "mb10"], ["type", "checkbox", "id", "chk-consumptionrate", "name", "chkRateOfConsumption", "formControlName", "RateOfConsumption", 1, "custom-control-input"], ["for", "chk-consumptionrate", 1, "custom-control-label"], [3, "salesTabFilterForm", 4, "ngIf"], [3, "RequestFromBuyerWallyBoard", "salesTabFilterForm", 4, "ngIf"], [3, "salesTabFilterForm"], [3, "RequestFromBuyerWallyBoard", "salesTabFilterForm"]],
      template: function TankViewMasterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Rate Of Consumption");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, TankViewMasterComponent_app_tank_view_8_Template, 2, 1, "app-tank-view", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, TankViewMasterComponent_app_forecasting_tank_view_9_Template, 2, 2, "app-forecasting-tank-view", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.salesTabFilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.salesTabFilterForm.get("RateOfConsumption").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.salesTabFilterForm.get("RateOfConsumption").value);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _tank_view_component__WEBPACK_IMPORTED_MODULE_4__["TankViewComponent"], _shared_components_forcasting_tank_view_component__WEBPACK_IMPORTED_MODULE_5__["ForcastingTankViewComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Rpc3BhdGNoZXIvZGlzcGF0Y2hlci1kYXNoYm9hcmQvc2FsZXMtZGF0YS90YW5rLXZpZXctbWFzdGVyLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TankViewMasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-tank-view-master',
          templateUrl: './tank-view-master.component.html',
          styleUrls: ['./tank-view-master.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_1__["DispatcherService"]
        }];
      }, {
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view.component.ts": function srcAppDispatcherDispatcherDashboardSalesDataTankViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankViewComponent", function () {
      return TankViewComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/service/wally-utility.service */
    "./src/app/carrier/service/wally-utility.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _tank_chart_tank_chart_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../../../tank-chart/tank-chart.component */
    "./src/app/tank-chart/tank-chart.component.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../../directives/sorting.pipe */
    "./src/app/directives/sorting.pipe.ts");

    function TankViewComponent_div_6_Template(rf, ctx) {
      if (rf & 1) {
        var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "input", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function TankViewComponent_div_6_Template_input_input_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r5.Partsfiltering($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TankViewComponent_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "No Location Available");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TankViewComponent_div_8_ng_container_12_tr_25_span_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 60);
      }
    }

    function TankViewComponent_div_8_ng_container_12_tr_25_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 61);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "active": a0
      };
    };

    function TankViewComponent_div_8_ng_container_12_tr_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "a", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TankViewComponent_div_8_ng_container_12_tr_25_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r15);

          var tank_r11 = ctx.$implicit;

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

          return ctx_r14.tankChange(tank_r11);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, TankViewComponent_div_8_ng_container_12_tr_25_span_4_Template, 1, 0, "span", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, TankViewComponent_div_8_ng_container_12_tr_25_span_5_Template, 1, 0, "span", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "a", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TankViewComponent_div_8_ng_container_12_tr_25_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r15);

          var loc_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r16.showTanks(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var tank_r11 = ctx.$implicit;

        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](6, _c0, ctx_r10.SelectedTankId == (tank_r11 == null ? null : tank_r11.TankId) + "_" + (tank_r11 == null ? null : tank_r11.StorageId)));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", tank_r11.Name, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tank_r11 == null ? null : tank_r11.IsUnknowDeliveryOrMissDelivery);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (tank_r11 == null ? null : tank_r11.TankInventoryDiffinHrs) > 2 || (tank_r11 == null ? null : tank_r11.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", tank_r11.DaysRemaining == null ? "NA" : tank_r11.DaysRemaining + " Days", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tank_r11.Status);
      }
    }

    function TankViewComponent_div_8_ng_container_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h2", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TankViewComponent_div_8_ng_container_12_Template_div_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19);

          var loc_r8 = ctx.$implicit;

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r18.locationChange(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](8, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](9, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "i", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "a", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TankViewComponent_div_8_ng_container_12_Template_a_click_16_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19);

          var loc_r8 = ctx.$implicit;

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r20.showTanks(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "span", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "tr", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "td", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "ul", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "table", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, TankViewComponent_div_8_ng_container_12_tr_25_Template, 12, 8, "tr", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var loc_r8 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("id", "headingOne" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", "", loc_r8.LocationName, "", loc_r8 && loc_r8.CustomerInfo ? " - " + loc_r8.CustomerInfo : null, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("data-target", "#col" + (loc_r8 == null ? null : loc_r8.JobId))("aria-controls", "col" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" ", loc_r8 == null ? null : loc_r8.LocationName, " ", loc_r8 && loc_r8.CustomerInfo && loc_r8.CustomerInfo.length > 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](8, 13, loc_r8.CustomerInfo, 0, 5) + "..)" : "", " ", loc_r8 && loc_r8.CustomerInfo && loc_r8.CustomerInfo.length <= 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](9, 17, loc_r8.CustomerInfo, 0, 5) + ")" : "", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](loc_r8.DaysRemaining == null ? "NA" : loc_r8.DaysRemaining + " Days");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](loc_r8.Status);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("id", "col" + (loc_r8 == null ? null : loc_r8.JobId))("aria-labelledby", "headingOne" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", loc_r8 == null ? null : loc_r8.Tanks);
      }
    }

    function TankViewComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "table", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "th", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "th", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TankViewComponent_div_8_Template_th_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r21.setSortArgs("DaysRemaining");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " Days remaining\xA0");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, " Status ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, TankViewComponent_div_8_ng_container_12_Template, 26, 21, "ng-container", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](13, "sortingPipe");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](13, 1, ctx_r2.FilterLocationDrpDwnList, ctx_r2.filterArgs));
      }
    }

    function TankViewComponent_tr_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TankViewComponent_tr_49_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TankViewComponent_tr_49_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", row_r23.PrevSale, " ");
      }
    }

    function TankViewComponent_tr_49_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TankViewComponent_tr_49_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", row_r23.WeekAgoSale, " ");
      }
    }

    function TankViewComponent_tr_49_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, TankViewComponent_tr_49_div_14_Template, 4, 0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, TankViewComponent_tr_49_div_15_Template, 2, 1, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, TankViewComponent_tr_49_div_17_Template, 4, 0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, TankViewComponent_tr_49_div_18_Template, 2, 1, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.TankName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", row_r23.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", row_r23.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", row_r23.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", row_r23.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r23.DaysRemaining == null ? "NA" : row_r23.DaysRemaining + " Days");
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "hide-element": a0
      };
    };

    var TankViewComponent = /*#__PURE__*/function () {
      function TankViewComponent(dispatcherService, wallyUtilService) {
        _classCallCheck(this, TankViewComponent);

        this.dispatcherService = dispatcherService;
        this.wallyUtilService = wallyUtilService;
        this.LocationSchedules = [];
        this.CloneLocationSchedules = [];
        this.LocationDrpDwnList = [];
        this.FilterLocationDrpDwnList = [];
        this.IsLoading = false;
        this.IsLocDrpDwnLoading = false;
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtOptions = {};
        this.filterArgs = {
          key: "DaysRemaining",
          asc: true
        };
        this.applyFilterSubscription = [];
      }

      _createClass(TankViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this46 = this;

          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Sales Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sales Details',
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
          this.applyFilterSubscription.push(Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["merge"])(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(function (value) {
            if (_this46.salesTabFilterForm.get('SalesViewType').value == 2) {
              _this46.initLocationDropDown();
            }
          })); //to load data - after second ngOnInit

          if (this.salesTabFilterForm.get('SalesViewType').value == 2) {
            this.initLocationDropDown();
          }
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          if (this.applyFilterSubscription) {
            this.applyFilterSubscription.forEach(function (subscription) {
              subscription.unsubscribe();
            });
          }
        }
      }, {
        key: "setSortArgs",
        value: function setSortArgs(key) {
          if (this.filterArgs.key == key) {
            this.filterArgs = {
              asc: !this.filterArgs.asc,
              key: key
            };
          } else {
            this.filterArgs = {
              asc: true,
              key: key
            };
          }
        }
      }, {
        key: "initLocationDropDown",
        value: function initLocationDropDown() {
          var _this47 = this;

          this.IsLocDrpDwnLoading = true;
          this.LocationDrpDwnList = [];
          var filter = {
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            CustomerIds: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
            InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
            IsRateOfConsumption: this.salesTabFilterForm.get('RateOfConsumption').value,
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value)
          };
          Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["forkJoin"])([this.dispatcherService.getSupplierLocationTanks(filter), this.dispatcherService.GetRaisedExceptions()]).subscribe(function (result) {
            _this47.IsLocDrpDwnLoading = false;
            _this47.LocationDrpDwnList = result[0];

            _this47.Partsfiltering();

            _this47.LocationDrpDwnList && _this47.LocationDrpDwnList.length > 0 ? _this47.locationChange(_this47.LocationDrpDwnList[0]) : '';

            if (_this47.LocationDrpDwnList && _this47.LocationDrpDwnList.length > 0) {
              _this47.LocationDrpDwnList.forEach(function (loc) {
                loc && loc.Tanks && loc.Tanks.length > 0 && loc.Tanks.forEach(function (m) {
                  if (result[1] && result[1].filter(function (f) {
                    return f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == loc.SiteId && f.TankDetail.StorageId == m.StorageId;
                  }).length > 0) m.IsUnknowDeliveryOrMissDelivery = true;else m.IsUnknowDeliveryOrMissDelivery = false;
                });
              });
            } else {
              _this47.SelectedTankId = null;
              _this47.LocationSchedules = [];
              _this47.CloneLocationSchedules = [];
              _this47.SelectedLocationId = '0';
            }
          });
        }
      }, {
        key: "locationChange",
        value: function locationChange($event) {
          this.SelectedTankId = null;
          this.SelectedLocationId = $event.JobId;
          this.SelectedSiteId = $event.SiteId;
          this.LocationSchedules = [];
          this.CloneLocationSchedules = [];
          this.getSalesData();
        }
      }, {
        key: "tankChange",
        value: function tankChange($event) {
          if (this.CloneLocationSchedules && this.CloneLocationSchedules.length > 0) {
            this.SelectedTankId = $event.TankId + '_' + $event.StorageId;
            this.LocationSchedules = this.CloneLocationSchedules.filter(function (f) {
              return f.TankId == $event.TankId && f.StorageId == $event.StorageId;
            });
          } else this.LocationSchedules = []; // this.dtTrigger.next();

        }
      }, {
        key: "getSalesData",
        value: function getSalesData() {
          var _this48 = this;

          var inputs = {
            RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value),
            Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__["DeliveryReqPriority"].None,
            CustomerId: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
            LocationId: this.SelectedLocationId,
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__["SelectedTabEnum"].Tanks,
            InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
            Carriers: '',
            IsShowCarrierManaged: '',
            IsShowRetailJobs: ''
          };
          this.IsLoading = true;
          this.dispatcherService.getSalesData(inputs).subscribe(function (resp) {
            _this48.LocationSchedules = resp;
            _this48.CloneLocationSchedules = resp;
            _this48.IsLoading = false;
            _this48.LocationSchedules && _this48.LocationSchedules.map(function (m) {
              try {
                _this48.FilterLocationDrpDwnList && _this48.FilterLocationDrpDwnList.filter(function (f) {
                  return f.SiteId == m.SiteId;
                }).map(function (j) {
                  return j.Tanks.find(function (f) {
                    return f.TankId == m.TankId && f.StorageId == m.StorageId;
                  }).TankInventoryDiffinHrs = m.TankInventoryDiffinHrs;
                });
              } catch (e) {
                console.log(e);
              }
            });

            _this48.datatableRerender();
          });
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          var _this49 = this;

          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();

              _this49.dtTrigger.next();
            });
          }
        }
      }, {
        key: "Partsfiltering",
        value: function Partsfiltering(inputName) {
          var _this50 = this;

          this.FilterLocationDrpDwnList = [];

          if (inputName && inputName.target && inputName.target.value && inputName.target.value.trim() != '') {
            var searchWord = inputName.target.value.toUpperCase();
            this.LocationDrpDwnList.forEach(function (element) {
              if (element.LocationName.toUpperCase().indexOf(searchWord) !== -1) {
                _this50.FilterLocationDrpDwnList.push(element);
              }
            });
          } else {
            this.FilterLocationDrpDwnList = this.LocationDrpDwnList;
          }
        }
      }, {
        key: "showTanks",
        value: function showTanks(location) {
          var row = this.LocationSchedules[0];
          this.SelectedTankRegionId = row.RegionId;
          var salesDataModel = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__["SalesDataModel"]();
          salesDataModel.RegionId = this.SelectedTankRegionId;
          salesDataModel.SiteId = location.SiteId;
          salesDataModel.TankId = location.TankId;
          salesDataModel.StorageId = location.StorageId;
          salesDataModel.TfxJobId = parseInt(location.JobId);
          salesDataModel.LocationManagedType = location.LocationManagedType;
          this.dipTestComponent.loadTankDR(salesDataModel);
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }]);

      return TankViewComponent;
    }();

    TankViewComponent.ɵfac = function TankViewComponent_Factory(t) {
      return new (t || TankViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_6__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__["WallyUtilService"]));
    };

    TankViewComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: TankViewComponent,
      selectors: [["app-tank-view"]],
      viewQuery: function TankViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dipTestComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 52,
      vars: 18,
      consts: [[1, "row"], [1, "col-sm-4"], [1, "well", "bg-white", "shadow-b", "location-panel"], ["id", "accordion-location", 1, "location-accordion"], [1, "position-abs", "text-center", 3, "ngClass"], [1, "spinner-small", "ml10", "mt5"], ["class", "mb10", 4, "ngIf"], [4, "ngIf"], [1, "col-sm-8", "location-chart-panel"], [1, "well", "bg-white", "shadow-b"], [3, "JobId", "SiteId", "TankId", "isSupplierView"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-Location", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Cust"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "TName"], ["data-key", "WL"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "onRaiseDR"], [1, "mb10"], [1, "inner-addon", "left-addon"], [1, "glyphicon", "glyphicon-search"], ["name", "txtSearch", "placeholder", "Search Location", "type", "text", "autocomplete", "off", 1, "form-control", 3, "input"], [1, "table", "tank-view"], ["width", "49%"], ["width", "24%", 1, "cursor_pointer", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-sort"], [1, "card-header"], [1, "mb-0"], ["data-toggle", "collapse", "aria-expanded", "true", 1, "position-relative", "pr-3", "btn", "btn-link", "collapsed", "text-left", 3, "ngbTooltip", "click"], [1, "fa-stack", "fa-sm", "icon-color-b", "position-absolute", 2, "top", "3px", "right", "-7px"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-plus", "fa-stack-1x", "fa-inverse"], ["href", "javascript:void(0)", "onclick", "slidePanel('#raisedr','60%')", 1, "", 3, "click"], [1, ""], ["data-parent", "#accordion-location", 1, "collapse"], ["colspan", "3"], [1, "card-body"], [1, "list-group", "list-group-flush"], [1, "table", "tank-view-child"], ["width", "45%"], ["href", "javascript:void(0)", 3, "ngClass", "click"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], ["width", "24%"], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"]],
      template: function TankViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, TankViewComponent_div_6_Template, 4, 0, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, TankViewComponent_div_7_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, TankViewComponent_div_8_Template, 14, 4, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "app-tank-chart", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "table", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Customer");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Tank Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "Water Level");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](42, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](48, TankViewComponent_tr_48_Template, 2, 0, "tr", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](49, TankViewComponent_tr_49_Template, 31, 16, "tr", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "app-dip-test", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onRaiseDR", function TankViewComponent_Template_app_dip_test_onRaiseDR_51_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](16, _c1, !ctx.IsLocDrpDwnLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("JobId", ctx.SelectedLocationId)("SiteId", ctx.SelectedSiteId)("TankId", ctx.SelectedTankId)("isSupplierView", true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.LocationSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("isDisableControl", true)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedTankRegionId)("IsThisFromDrDisplay", false);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_8__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _tank_chart_tank_chart_component__WEBPACK_IMPORTED_MODULE_9__["TankChartComponent"], angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbTooltip"]],
      pipes: [_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_11__["SortingPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["SlicePipe"]],
      styles: [".location-panel[_ngcontent-%COMP%] {\r\n    height: calc(100vh - 130px);\r\n    max-height: calc(100vh - 120px);\r\n    overflow-y: auto;\r\n}\r\n.location-chart-panel[_ngcontent-%COMP%] {\r\n    max-height: calc(100vh - 120px);\r\n    overflow-y: auto;\r\n    margin-right:-20px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%] {\r\n    \r\n    \r\n}\r\n.tank-view.table[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]:first-child   td[_ngcontent-%COMP%]{\r\n    border-top: 0px solid #e7eaec;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]{\r\n    margin-bottom: 5px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]   .active[_ngcontent-%COMP%]{\r\n    font-weight: 700;\r\n    color: brown;\r\n}\r\n.location-accordion[_ngcontent-%COMP%] {\r\n    \r\n    \r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%], .location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:last-child   .card-header[_ngcontent-%COMP%] {\r\n    border: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%] {\r\n    border-bottom-color: #EDEFF0;\r\n    background: transparent;\r\n    border-bottom: 0;\r\n    \r\n    padding: 0px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%]:hover {\r\n    background: #e2e0ff5e !important;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-body[_ngcontent-%COMP%] {\r\n    \r\n    padding: 0px;\r\n    margin-left: -8px;\r\n    margin-right: -8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .fa-stack[_ngcontent-%COMP%] {\r\n    font-size: 8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    border: 0;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%] {\r\n    background-color: transparent;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #616161;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\r\n    width: 100%;\r\n    \r\n    color: #004987;\r\n    padding: 0;\r\n}\r\n.icon-color-b[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:hover, .location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:focus {\r\n    text-decoration: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%] {\r\n    color: #616161 !important;\r\n}\r\n\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    padding: 3px 20px;\r\n}\r\n.bg-change[_ngcontent-%COMP%] {\r\n    background: #e2e0ff5e !important;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\r\n@keyframes blink {\r\n    from,to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n@-webkit-keyframes blink {\r\n    from, to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n.tank-status[_ngcontent-%COMP%] {\r\n    width: 80px;\r\n    height: 80px;\r\n    border: 5px solid gray;\r\n    line-height: 200;\r\n    color: #ddd;\r\n    position: relative;\r\n    border-radius: 100px;\r\n    overflow: hidden;\r\n    margin: 0 auto;\r\n}\r\n.tank-status[_ngcontent-%COMP%]   .available[_ngcontent-%COMP%] {\r\n    background: #1062d1;\r\n    width: 100%;\r\n    bottom: -1px;\r\n    position: absolute;\r\n    border-radius: 0 0 90px 90px;\r\n    border: 1px solid #1062d1;\r\n}\r\n.tank-status[_ngcontent-%COMP%] {\r\n    width: 80px;\r\n    height: 80px;\r\n    border: 5px solid gray;\r\n    line-height: 200;\r\n    color: #ddd;\r\n    position: relative;\r\n    border-radius: 100px;\r\n    overflow: hidden;\r\n    margin: 0 auto;\r\n}\r\n.tank-status[_ngcontent-%COMP%]   .available[_ngcontent-%COMP%] {\r\n        background: #1062d1;\r\n        width: 100%;\r\n        bottom: -1px;\r\n        position: absolute;\r\n        border-radius: 0 0 90px 90px;\r\n        border: 1px solid #1062d1;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL3Rhbmstdmlldy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksMkJBQTJCO0lBQzNCLCtCQUErQjtJQUMvQixnQkFBZ0I7QUFDcEI7QUFDQTtJQUNJLCtCQUErQjtJQUMvQixnQkFBZ0I7SUFDaEIsa0JBQWtCO0FBQ3RCO0FBR0E7SUFDSTt1QkFDbUI7SUFDbkIsdUNBQXVDO0FBQzNDO0FBRUE7SUFDSSxnQkFBZ0I7QUFDcEI7QUFFQTtJQUNJLDZCQUE2QjtBQUNqQztBQUNBO0lBQ0ksZ0JBQWdCO0FBQ3BCO0FBQ0E7SUFDSSxrQkFBa0I7QUFDdEI7QUFFQTtJQUNJLGdCQUFnQjtJQUNoQixZQUFZO0FBQ2hCO0FBQ0E7SUFDSTt1QkFDbUI7SUFDbkIsdUNBQXVDO0FBQzNDO0FBRUE7O0lBRUksWUFBWTtBQUNoQjtBQUVBO0lBQ0ksNEJBQTRCO0lBQzVCLHVCQUF1QjtJQUN2QixnQkFBZ0I7SUFDaEIsdUJBQXVCO0lBQ3ZCLFlBQVk7QUFDaEI7QUFFQTtJQUNJLGdDQUFnQztBQUNwQztBQUVBO0lBQ0ksa0JBQWtCO0lBQ2xCLFlBQVk7SUFDWixpQkFBaUI7SUFDakIsa0JBQWtCO0FBQ3RCO0FBRUE7SUFDSSxjQUFjO0FBQ2xCO0FBRUE7SUFDSSxTQUFTO0FBQ2I7QUFFQTtJQUNJLDZCQUE2QjtBQUNqQztBQUVBO0lBQ0ksY0FBYztJQUNkLGlCQUFpQjtBQUNyQjtBQUVBO0lBQ0ksY0FBYztBQUNsQjtBQUVBO0lBQ0ksV0FBVztJQUNYLHFCQUFxQjtJQUNyQixjQUFjO0lBQ2QsVUFBVTtBQUNkO0FBRUE7SUFDSSxjQUFjO0FBQ2xCO0FBRUE7O0lBRUkscUJBQXFCO0FBQ3pCO0FBR0E7SUFDSSx5QkFBeUI7QUFDN0I7QUFFQTs7TUFFTTtBQUNOO0lBQ0ksaUJBQWlCO0FBQ3JCO0FBRUE7SUFDSSxnQ0FBZ0M7SUFDaEMsaUJBQWlCO0FBQ3JCO0FBRUE7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLHlCQUF5QjtJQUN6QixrQkFBa0I7SUFDbEIscUJBQXFCO0lBQ3JCLHlDQUFpQztZQUFqQyxpQ0FBaUM7QUFDckM7QUFHQTtJQUNJO1FBQ0ksVUFBVTtJQUNkOztJQUVBO1FBQ0ksVUFBVTtJQUNkO0FBQ0o7QUFZQTtJQUNJO1FBQ0ksVUFBVTtJQUNkOztJQUVBO1FBQ0ksVUFBVTtJQUNkO0FBQ0o7QUFxQkE7SUFDSSxXQUFXO0lBQ1gsWUFBWTtJQUNaLHNCQUFzQjtJQUN0QixnQkFBZ0I7SUFDaEIsV0FBVztJQUNYLGtCQUFrQjtJQUNsQixvQkFBb0I7SUFDcEIsZ0JBQWdCO0lBQ2hCLGNBQWM7QUFDbEI7QUFFQTtJQUNJLG1CQUFtQjtJQUNuQixXQUFXO0lBQ1gsWUFBWTtJQUNaLGtCQUFrQjtJQUNsQiw0QkFBNEI7SUFDNUIseUJBQXlCO0FBQzdCO0FBRUE7SUFDSSxXQUFXO0lBQ1gsWUFBWTtJQUNaLHNCQUFzQjtJQUN0QixnQkFBZ0I7SUFDaEIsV0FBVztJQUNYLGtCQUFrQjtJQUNsQixvQkFBb0I7SUFDcEIsZ0JBQWdCO0lBQ2hCLGNBQWM7QUFDbEI7QUFFSTtRQUNJLG1CQUFtQjtRQUNuQixXQUFXO1FBQ1gsWUFBWTtRQUNaLGtCQUFrQjtRQUNsQiw0QkFBNEI7UUFDNUIseUJBQXlCO0lBQzdCIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9zYWxlcy1kYXRhL3Rhbmstdmlldy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmxvY2F0aW9uLXBhbmVsIHtcclxuICAgIGhlaWdodDogY2FsYygxMDB2aCAtIDEzMHB4KTtcclxuICAgIG1heC1oZWlnaHQ6IGNhbGMoMTAwdmggLSAxMjBweCk7XHJcbiAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG59XHJcbi5sb2NhdGlvbi1jaGFydC1wYW5lbCB7XHJcbiAgICBtYXgtaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTIwcHgpO1xyXG4gICAgb3ZlcmZsb3cteTogYXV0bztcclxuICAgIG1hcmdpbi1yaWdodDotMjBweDtcclxufVxyXG5cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24ge1xyXG4gICAgLyptYXgtd2lkdGg6IDUwMHB4O1xyXG4gICAgbWFyZ2luOiA1MHB4IGF1dG87Ki9cclxuICAgIC8qYm94LXNoYWRvdzogMCAwIDFweCByZ2JhKDAsMCwwLDAuMSk7Ki9cclxufVxyXG5cclxuLnRhbmstdmlldy50YWJsZSA+IHRib2R5ID4gdHIgPiB0ZHtcclxuICAgIHBhZGRpbmc6IDRweCA4cHg7XHJcbn1cclxuXHJcbi50YWJsZS50YW5rLXZpZXctY2hpbGQgPiB0Ym9keSA+IHRyOmZpcnN0LWNoaWxkIHRke1xyXG4gICAgYm9yZGVyLXRvcDogMHB4IHNvbGlkICNlN2VhZWM7XHJcbn1cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZCA+IHRib2R5ID4gdHIgPiB0ZHtcclxuICAgIHBhZGRpbmc6IDRweCA4cHg7XHJcbn1cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZHtcclxuICAgIG1hcmdpbi1ib3R0b206IDVweDtcclxufVxyXG5cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZCAuYWN0aXZle1xyXG4gICAgZm9udC13ZWlnaHQ6IDcwMDtcclxuICAgIGNvbG9yOiBicm93bjtcclxufVxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIHtcclxuICAgIC8qbWF4LXdpZHRoOiA1MDBweDtcclxuICAgIG1hcmdpbjogNTBweCBhdXRvOyovXHJcbiAgICAvKmJveC1zaGFkb3c6IDAgMCAxcHggcmdiYSgwLDAsMCwwLjEpOyovXHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQsXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQ6bGFzdC1jaGlsZCAuY2FyZC1oZWFkZXIge1xyXG4gICAgYm9yZGVyOiBub25lO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWhlYWRlciB7XHJcbiAgICBib3JkZXItYm90dG9tLWNvbG9yOiAjRURFRkYwO1xyXG4gICAgYmFja2dyb3VuZDogdHJhbnNwYXJlbnQ7XHJcbiAgICBib3JkZXItYm90dG9tOiAwO1xyXG4gICAgLyogcGFkZGluZzogNXB4IDEwcHg7ICovXHJcbiAgICBwYWRkaW5nOiAwcHg7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQtaGVhZGVyOmhvdmVyIHtcclxuICAgIGJhY2tncm91bmQ6ICNlMmUwZmY1ZSAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWJvZHkge1xyXG4gICAgLyogcGFkZGluZzogNXB4OyAqL1xyXG4gICAgcGFkZGluZzogMHB4O1xyXG4gICAgbWFyZ2luLWxlZnQ6IC04cHg7XHJcbiAgICBtYXJnaW4tcmlnaHQ6IC04cHg7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmZhLXN0YWNrIHtcclxuICAgIGZvbnQtc2l6ZTogOHB4O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5saXN0LWdyb3VwLWl0ZW0ge1xyXG4gICAgYm9yZGVyOiAwO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5saXN0LWdyb3VwLWl0ZW0uYWN0aXZlIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IHRyYW5zcGFyZW50O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5saXN0LWdyb3VwLWl0ZW0uYWN0aXZlIGEge1xyXG4gICAgY29sb3I6ICMxMDYyZDE7XHJcbiAgICBmb250LXdlaWdodDogYm9sZDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtIGEge1xyXG4gICAgY29sb3I6ICM2MTYxNjE7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmJ0biB7XHJcbiAgICB3aWR0aDogMTAwJTtcclxuICAgIC8qZm9udC13ZWlnaHQ6IGJvbGQ7Ki9cclxuICAgIGNvbG9yOiAjMDA0OTg3O1xyXG4gICAgcGFkZGluZzogMDtcclxufVxyXG5cclxuLmljb24tY29sb3ItYiB7XHJcbiAgICBjb2xvcjogIzEwNjJkMTtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAuYnRuLWxpbms6aG92ZXIsXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmJ0bi1saW5rOmZvY3VzIHtcclxuICAgIHRleHQtZGVjb3JhdGlvbjogbm9uZTtcclxufVxyXG5cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmJ0bi1saW5rIHtcclxuICAgIGNvbG9yOiAjNjE2MTYxICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi8qLmxvY2F0aW9uLWFjY29yZGlvbiBsaSArIGxpIHtcclxuICAgICAgICBtYXJnaW4tdG9wOiAxMHB4O1xyXG4gICAgfSovXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmxpc3QtZ3JvdXAtaXRlbSB7XHJcbiAgICBwYWRkaW5nOiAzcHggMjBweDtcclxufVxyXG5cclxuLmJnLWNoYW5nZSB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZTJlMGZmNWUgIWltcG9ydGFudDtcclxuICAgIGZvbnQtd2VpZ2h0OiBib2xkO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5hY3RpdmUtZG90IHtcclxuICAgIGhlaWdodDogMTBweDtcclxuICAgIHdpZHRoOiAxMHB4O1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2ZmNTg1ODtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuICAgIGFuaW1hdGlvbjogMXMgYmxpbmsgZWFzZSBpbmZpbml0ZTtcclxufVxyXG5cclxuXHJcbkBrZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSx0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtbW96LWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtd2Via2l0LWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtbXMta2V5ZnJhbWVzIGJsaW5rIHtcclxuICAgIGZyb20sIHRvIHtcclxuICAgICAgICBvcGFjaXR5OiAwO1xyXG4gICAgfVxyXG5cclxuICAgIDUwJSB7XHJcbiAgICAgICAgb3BhY2l0eTogMTtcclxuICAgIH1cclxufVxyXG5cclxuQC1vLWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuLnRhbmstc3RhdHVzIHtcclxuICAgIHdpZHRoOiA4MHB4O1xyXG4gICAgaGVpZ2h0OiA4MHB4O1xyXG4gICAgYm9yZGVyOiA1cHggc29saWQgZ3JheTtcclxuICAgIGxpbmUtaGVpZ2h0OiAyMDA7XHJcbiAgICBjb2xvcjogI2RkZDtcclxuICAgIHBvc2l0aW9uOiByZWxhdGl2ZTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwMHB4O1xyXG4gICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgIG1hcmdpbjogMCBhdXRvO1xyXG59XHJcblxyXG4udGFuay1zdGF0dXMgLmF2YWlsYWJsZSB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjMTA2MmQxO1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBib3R0b206IC0xcHg7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICBib3JkZXItcmFkaXVzOiAwIDAgOTBweCA5MHB4O1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgIzEwNjJkMTtcclxufVxyXG5cclxuLnRhbmstc3RhdHVzIHtcclxuICAgIHdpZHRoOiA4MHB4O1xyXG4gICAgaGVpZ2h0OiA4MHB4O1xyXG4gICAgYm9yZGVyOiA1cHggc29saWQgZ3JheTtcclxuICAgIGxpbmUtaGVpZ2h0OiAyMDA7XHJcbiAgICBjb2xvcjogI2RkZDtcclxuICAgIHBvc2l0aW9uOiByZWxhdGl2ZTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwMHB4O1xyXG4gICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgIG1hcmdpbjogMCBhdXRvO1xyXG59XHJcblxyXG4gICAgLnRhbmstc3RhdHVzIC5hdmFpbGFibGUge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICMxMDYyZDE7XHJcbiAgICAgICAgd2lkdGg6IDEwMCU7XHJcbiAgICAgICAgYm90dG9tOiAtMXB4O1xyXG4gICAgICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiAwIDAgOTBweCA5MHB4O1xyXG4gICAgICAgIGJvcmRlcjogMXB4IHNvbGlkICMxMDYyZDE7XHJcbiAgICB9Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TankViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-tank-view',
          templateUrl: './tank-view.component.html',
          styleUrls: ['./tank-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_6__["DispatcherService"]
        }, {
          type: src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_7__["WallyUtilService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"]]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"]]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"]]
        }],
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-grid-view.component.ts": function srcAppDispatcherDispatcherDashboardWhereIsMyDriverGridViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverGridViewComponent", function () {
      return WhereIsMyDriverGridViewComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/services/sendbird.service */
    "./src/app/shared-components/sendbird/services/sendbird.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    var _c0 = ["SelectedDriverLoad"];

    function WhereIsMyDriverGridViewComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverGridViewComponent_div_21_th_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "th", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Drop Ticket");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "hide-element": a0
      };
    };

    function WhereIsMyDriverGridViewComponent_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "PO Number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Dispatcher");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Pickup");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Product Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Ordered Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, WhereIsMyDriverGridViewComponent_div_21_th_33_Template, 2, 0, "th", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c1, ctx_r1.activePriorityTab == ctx_r1.DeliveryReqPriority.ShouldGo || ctx_r1.activePriorityTab == ctx_r1.DeliveryReqPriority.CouldGo));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r1.dtMustGoOptions)("dtTrigger", ctx_r1.dtMustGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.disableControl === true);
      }
    }

    function WhereIsMyDriverGridViewComponent_div_22_th_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "th", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Drop Ticket");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverGridViewComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Should Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "PO Number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Dispatcher");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Pickup");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Product Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Ordered Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, WhereIsMyDriverGridViewComponent_div_22_th_33_Template, 2, 0, "th", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c1, ctx_r2.activePriorityTab == ctx_r2.DeliveryReqPriority.MustGo || ctx_r2.activePriorityTab == ctx_r2.DeliveryReqPriority.CouldGo));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r2.dtShouldGoOptions)("dtTrigger", ctx_r2.dtShouldGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.disableControl === true);
      }
    }

    function WhereIsMyDriverGridViewComponent_div_23_th_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "th", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Drop Ticket");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverGridViewComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Could Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "table", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "th", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "PO Number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Dispatcher");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Pickup");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Product Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Ordered Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, WhereIsMyDriverGridViewComponent_div_23_th_33_Template, 2, 0, "th", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c1, ctx_r3.activePriorityTab == ctx_r3.DeliveryReqPriority.MustGo || ctx_r3.activePriorityTab == ctx_r3.DeliveryReqPriority.ShouldGo));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx_r3.dtCouldGoOptions)("dtTrigger", ctx_r3.dtCouldGoTrigger);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.disableControl === true);
      }
    }

    function WhereIsMyDriverGridViewComponent_tr_43_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var member_r8 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r8.nickname);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r8.userId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r8.connectionStatus);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r8.lastSeenAt);
      }
    }

    var _c2 = function _c2(a0) {
      return {
        "active": a0
      };
    };

    var WhereIsMyDriverGridViewComponent = /*#__PURE__*/function () {
      function WhereIsMyDriverGridViewComponent(dispatcherService, chatService, carrierService) {
        _classCallCheck(this, WhereIsMyDriverGridViewComponent);

        this.dispatcherService = dispatcherService;
        this.chatService = chatService;
        this.carrierService = carrierService;
        this.IsFiltersLoaded = false;
        this.activePriorityTab = 1;
        this.DeliveryReqPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"];
        this.SelectedMapLabelEnum = src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SelectedMapLabelEnum"];
        this.previousInfowindow = null;
        this.previousInfowindowIndex = null;
        this.zoomLevel = 4;
        this.centerLoactionLat = 39.1175;
        this.centerLoactionLng = -103.8784;
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_4__().add(1, 'year').toDate();
        this.TodaysDate = moment__WEBPACK_IMPORTED_MODULE_4__().format('MM/DD/YYYY');
        this.AUTO_REFRESH_TIME = 300; // seconds

        this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
        this.driverModal = {
          modalDetails: {
            display: 'none',
            data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverModel"]()
          }
        };
        this.screenOptions = {
          position: 6
        };
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subscription"]();
        this.Drivers = [];
        this.OfflineDrivers = [];
        this.allLoads = [];
        this.OnGoingLoads = [];
        this.CloneOnGoingLoads = [];
        this.MustGoSchedules = [];
        this.ShouldGoSchedules = [];
        this.CouldGoSchedules = [];
        this.selectedDriverLoads = [];
        this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"]();
        this.Regions = [];
        this.RegionStates = [];
        this.RegionDispachers = [];
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.RegionDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleMap = true;
        this.toogleFilter = false;
        this.toogleDriver = false;
        this.toogleGrid = false;
        this.toogleExpandMap = false;
        this.customerList = [];
        this.IsShowCarrierManaged = false;
        this.SelectedCarrierIds = '';
        this.SelectedStateIds = '';
        this.SelectedPriorityIds = '';
        this.SelectedDispacherIds = '';
        this.SelectedCustomerId = '';
        this.selectedLocAttributeIds = '';
        this.IsDataLoaded = false;
        this.dtMustGoOptions = {};
        this.dtShouldGoOptions = {};
        this.dtCouldGoOptions = {};
        this.selectedDriverLoadsdtOptions = {};
        this.selectedDriverLoadsdtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtMustGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtShouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtCouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.loadingData = true;
        this.modalData = true; // public IsShouldGoLoading: boolean;
        // public IsCouldGoLoading: boolean;
        // public IsMustGoLoading: boolean;

        this.backgroudchatDefault = [];
        this.memberInfo = [];
        this.disableControl = false;
        this.toogleMapFromParent = true;
      }

      _createClass(WhereIsMyDriverGridViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this51 = this;

          this.readOnlyModeSelection();
          this.subscribeFormChanges();
          var exportColumns = {
            columns: ':visible'
          };
          var mustGocolumnsDetails = [];
          var shouldGocolumnsDetails = [];
          var couldGocolumnsDetails = [];

          if (this.disableControl) {
            mustGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }, {
              title: 'Drop Ticket',
              name: 'DROPTicketNum',
              data: 'DROPTicketNum',
              "autoWidth": true
            }];
          } else {
            mustGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }];
          }

          this.dtMustGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
              header: true,
              headerOffset: 200
            },
            ajax: function ajax(dataTablesParameters, callback) {
              var _states = [];

              _this51.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Code);
              });

              var selectedDispacherIds = '';

              _this51.FilterForm.get('SelectedDispachers').value.map(function (m) {
                if (selectedDispacherIds == '') selectedDispacherIds = m.Id;else selectedDispacherIds += ',' + m.Id;
              });

              var _carriers = [];

              _this51.FilterForm.get('SelectedCarriers').value.forEach(function (x) {
                return _carriers.push(x.Id);
              });

              var _locAttribute = [];

              _this51.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
                return _locAttribute.push(x.Id);
              });

              var _locAttributeIds = _locAttribute.join();

              var inputs = {
                RegionId: _this51.SelectedRegionId,
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].MustGo,
                FromDate: _this51.FilterForm.get('FromDate').value,
                ToDate: _this51.FilterForm.get('ToDate').value,
                DriverSearch: _this51.SearchedKeyword,
                DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                CustomerId: _this51.FilterForm.get('SelectedCustomerId').value,
                Carriers: _carriers,
                IsShowCarrierManaged: _this51.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _locAttributeIds
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this51.loadingData = true;

              _this51.dispatcherService.getDispatcherLoads(inputData).subscribe(function (resp) {
                _this51.MustGoSchedules = resp.data;
                _this51.loadingData = false;
                callback({
                  recordsTotal: resp.recordsTotal,
                  recordsFiltered: resp.recordsFiltered,
                  data: resp.data
                });
              });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [{
              extend: 'colvis',
              exportOptions: exportColumns
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Dispatcher Dashboard - Must Go',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Dispatcher Dashboard - Must Go',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            columns: mustGocolumnsDetails
          };

          if (this.disableControl) {
            shouldGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }, {
              title: 'Drop Ticket',
              name: 'DROPTicketNum',
              data: 'DROPTicketNum',
              "autoWidth": true
            }];
          } else {
            shouldGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }];
          }

          this.dtShouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
              header: true,
              headerOffset: 200
            },
            ajax: function ajax(dataTablesParameters, callback) {
              var _states = [];

              _this51.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Code);
              });

              var selectedDispacherIds = '';

              _this51.FilterForm.get('SelectedDispachers').value.map(function (m) {
                if (selectedDispacherIds == '') selectedDispacherIds = m.Id;else selectedDispacherIds += ',' + m.Id;
              });

              var selectedCarrierIds = '';
              var selectedCarriers = _this51.FilterForm.get('SelectedCarriers').value || [];
              selectedCarriers.map(function (m) {
                if (selectedCarrierIds == '') selectedCarrierIds = m.Id;else selectedCarrierIds += ',' + m.Id;
              });
              var _locAttribute = [];

              _this51.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
                return _locAttribute.push(x.Id);
              });

              var _locAttributeIds = _locAttribute.join();

              var inputs = {
                RegionId: _this51.SelectedRegionId,
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].ShouldGo,
                FromDate: _this51.FilterForm.get('FromDate').value,
                ToDate: _this51.FilterForm.get('ToDate').value,
                DriverSearch: _this51.SearchedKeyword,
                DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                CustomerId: _this51.FilterForm.get('SelectedCustomerId').value,
                Carriers: selectedCarrierIds,
                IsShowCarrierManaged: _this51.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _locAttributeIds
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this51.loadingData = true;

              _this51.dispatcherService.getDispatcherLoads(inputData).subscribe(function (resp) {
                _this51.ShouldGoSchedules = resp.data;
                _this51.loadingData = false;
                callback({
                  recordsTotal: resp.recordsTotal,
                  recordsFiltered: resp.recordsFiltered,
                  data: resp.data
                });
              });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [{
              extend: 'colvis',
              exportOptions: exportColumns
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Dispatcher Dashboard - Should Go',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Dispatcher Dashboard - Should Go',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            columns: shouldGocolumnsDetails
          };

          if (this.disableControl) {
            couldGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }, {
              title: 'Drop Ticket',
              name: 'DROPTicketNum',
              data: 'DROPTicketNum',
              "autoWidth": true
            }];
          } else {
            couldGocolumnsDetails = [{
              title: 'PO Number',
              name: 'PoNum',
              data: 'PoNum',
              "autoWidth": true
            }, {
              title: 'Driver',
              name: 'Name',
              data: 'Name',
              "autoWidth": true
            }, {
              title: 'Dispatcher',
              name: 'DName',
              data: 'DName',
              "autoWidth": true
            }, {
              title: 'Customer',
              name: 'CName',
              data: 'CName',
              "autoWidth": true
            }, {
              title: 'Pickup',
              name: 'Pckup',
              data: 'Pckup',
              "autoWidth": true
            }, {
              title: 'Location',
              name: 'Loc',
              data: 'Loc',
              "autoWidth": true
            }, {
              title: 'Inventory Capture Method',
              name: 'LT',
              data: 'InventoryDataCaptureTypeName',
              "autoWidth": true
            }, {
              title: 'Product Name',
              name: 'PrdtNm',
              data: 'PrdtNm',
              "autoWidth": true
            }, {
              title: 'Ordered Quantity',
              name: 'Qty',
              data: 'Qty',
              "autoWidth": true
            }, {
              title: 'Date',
              name: 'LdDate',
              data: 'LdDate',
              "autoWidth": true
            }, {
              title: 'Status',
              name: 'Status',
              data: 'Status',
              "autoWidth": true
            }];
          }

          this.dtCouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
              header: true,
              headerOffset: 200
            },
            ajax: function ajax(dataTablesParameters, callback) {
              var _states = [];

              _this51.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Code);
              });

              var selectedDispacherIds = '';

              _this51.FilterForm.get('SelectedDispachers').value.map(function (m) {
                if (selectedDispacherIds == '') selectedDispacherIds = m.Id;else selectedDispacherIds += ',' + m.Id;
              });

              var selectedCarrierIds = '';
              var selectedCarriers = _this51.FilterForm.get('SelectedCarriers').value || [];
              selectedCarriers.map(function (m) {
                if (selectedCarrierIds == '') selectedCarrierIds = m.Id;else selectedCarrierIds += ',' + m.Id;
              });
              var _locAttribute = [];

              _this51.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
                return _locAttribute.push(x.Id);
              });

              var _locAttributeIds = _locAttribute.join();

              var inputs = {
                RegionId: _this51.SelectedRegionId,
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].CouldGo,
                FromDate: _this51.FilterForm.get('FromDate').value,
                ToDate: _this51.FilterForm.get('ToDate').value,
                DriverSearch: _this51.SearchedKeyword,
                DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                CustomerId: _this51.FilterForm.get('SelectedCustomerId').value,
                Carriers: selectedCarrierIds,
                IsShowCarrierManaged: _this51.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _locAttributeIds
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this51.loadingData = true;

              _this51.dispatcherService.getDispatcherLoads(inputData).subscribe(function (resp) {
                _this51.CouldGoSchedules = resp.data;
                _this51.loadingData = false;
                callback({
                  recordsTotal: resp.recordsTotal,
                  recordsFiltered: resp.recordsFiltered,
                  data: resp.data
                });
              });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [{
              extend: 'colvis',
              exportOptions: exportColumns
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Dispatcher Dashboard - Could Go',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Dispatcher Dashboard - Could Go',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            columns: couldGocolumnsDetails
          };
          this.selectedDriverLoadsdtOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Dispatcher Dashboard - Selected Driver Loads',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Dispatcher Dashboard - Selected Driver Loads',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }]
          };
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        } // subscribeFormChanges() {
        //     this.subscriptions.add(this.FilterForm.valueChanges
        //         .subscribe(change => {
        //             var isFilterChanged = this.IsFilterChanged();
        //             if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
        //                 this.IsDataLoaded = true;
        //                 this.filterDriverData();
        //             }
        //         }))
        // }

      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this52 = this;

          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            var isFilterChanged = _this52.IsFilterChanged();

            if (_this52.IsFiltersLoaded && (isFilterChanged || !_this52.IsDataLoaded)) {
              _this52.IsDataLoaded = true;

              _this52.filterDriverData();
            }
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(function (change) {
            var isFilterChanged = _this52.IsFilterChanged();

            if (_this52.IsFiltersLoaded && (isFilterChanged || !_this52.IsDataLoaded)) {
              _this52.IsDataLoaded = true;

              _this52.filterDriverData();
            }
          }));
        }
      }, {
        key: "unSubscribeFormChanges",
        value: function unSubscribeFormChanges() {
          if (this.subscriptions) {
            this.subscriptions.unsubscribe();
          }
        }
      }, {
        key: "IsFilterChanged",
        value: function IsFilterChanged() {
          var isFilterChanged = false;
          var isShowCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;

          if (this.IsShowCarrierManaged != isShowCarrierManaged) {
            this.IsShowCarrierManaged = isShowCarrierManaged;
            isFilterChanged = true;
          }

          var ids = [];
          var selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
          selectedCarriers.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedCarrierId = ids.join();

          if (this.SelectedCarrierIds != selectedCarrierId) {
            this.SelectedCarrierIds = selectedCarrierId;
            isFilterChanged = true;
          }

          ids = [];
          var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
          selectedRegions.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedRegionId = ids.join();

          if (this.SelectedRegionId != selectedRegionId) {
            this.SelectedRegionId = selectedRegionId;
            isFilterChanged = true;
          }

          ids = [];
          var selectedStates = this.FilterForm.get('SelectedStates').value || [];
          selectedStates.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedStateIds = ids.join();

          if (this.SelectedStateIds != selectedStateIds) {
            this.SelectedStateIds = selectedStateIds;
            isFilterChanged = true;
          }

          ids = [];
          var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
          selectedPriorities.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedPriorityIds = ids.join();

          if (this.SelectedPriorityIds != selectedPriorityIds) {
            this.SelectedPriorityIds = selectedPriorityIds;
            isFilterChanged = true;
          }

          ids = [];
          var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
          selectedDispachers.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedDispacherIds = ids.join();

          if (this.SelectedDispacherIds != selectedDispacherIds) {
            this.SelectedDispacherIds = selectedDispacherIds;
            isFilterChanged = true;
          }

          var selectedCustomerId = this.FilterForm.get('SelectedCustomerId').value;

          if (this.SelectedCustomerId != selectedCustomerId) {
            this.SelectedCustomerId = selectedCustomerId;
            isFilterChanged = true;
          }

          ids = [];
          var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
          selectedLocAttributeList.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedLocAttributeIds = ids.join();

          if (this.selectedLocAttributeIds != selectedLocAttributeIds) {
            this.selectedLocAttributeIds = selectedLocAttributeIds;
            isFilterChanged = true;
          }

          var fromdate = this.FilterForm.get('FromDate').value;

          if (this.selectedStartDate != fromdate) {
            this.selectedStartDate = fromdate;
            isFilterChanged = true;
          }

          var todate = this.FilterForm.get('ToDate').value;

          if (this.selectedEndDate != todate) {
            this.selectedEndDate = todate;
            isFilterChanged = true;
          }

          return isFilterChanged;
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.IsFiltersLoaded && change.IsFiltersLoaded.currentValue) {
            this.IsFiltersLoaded = change.IsFiltersLoaded.currentValue;
          } // if (change.SelectedRegions && change.SelectedRegions.currentValue)
          //     this.onRegionSelect();
          // if (change.SelectedCustomerId && change.SelectedCustomerId.currentValue)
          //     this.customerChanged();
          // if ((change.FromDate && change.FromDate.currentValue) || (change.ToDate && change.ToDate.currentValue)) {
          //     this.filterDriverData();
          // }
          // if (change.SelectedStates && change.SelectedStates.currentValue)
          //     this.filterDriverData();
          // if (change.SelectedPriorities && change.SelectedPriorities.currentValue)
          //     this.filterDriverData();
          // if (change.SelectedDispachers && change.SelectedDispachers.currentValue)
          //     this.filterDriverData();


          if (change.toogleMapFromParent) {
            this.toogleMapFromParent = change.toogleMapFromParent.currentValue;
          }

          if (change.SelectedCarriers && change.SelectedCarriers.currentValue) this.filterDriverData(); // if (change.selectedLocAttributeList && change.selectedLocAttributeList.currentValue)
          //     this.filterDriverData();

          if (change.IsShowCarrierManaged) this.filterDriverData();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          //this.getDispatcherLoads();
          this.autoRefreshLoads();
          this.dtCouldGoTrigger.next();
          this.dtShouldGoTrigger.next();
          this.dtMustGoTrigger.next();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.clearAllIntervals();
          this.unSubscribeFormChanges();
          this.dtCouldGoTrigger.unsubscribe();
          this.dtShouldGoTrigger.unsubscribe();
          this.dtMustGoTrigger.unsubscribe();
        }
      }, {
        key: "changeActiveTab",
        value: function changeActiveTab(priority) {
          this.activePriorityTab = priority;
        }
      }, {
        key: "checkFilterValueChange",
        value: function checkFilterValueChange() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
            var frmDate, toDate, selectedRegions, selectedStates, selectedDispachers, cid;
            return regeneratorRuntime.wrap(function _callee4$(_context4) {
              while (1) {
                switch (_context4.prev = _context4.next) {
                  case 0:
                    if (this.singleMulti == 2) {
                      frmDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY);
                      toDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY);
                      selectedRegions = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_REGION_KEY);
                      selectedRegions == "" ? selectedRegions = [] : '';
                      selectedStates = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_SELECTEDSTATES_KEY);
                      selectedStates == "" ? selectedStates = [] : '';
                      selectedDispachers = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_SELECTEDDISPACHER_KEY);
                      cid = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_CUSTOMER_KEY);

                      if (frmDate != '' && toDate != '' && (!(+moment__WEBPACK_IMPORTED_MODULE_4__(frmDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('FromDate').value)) || !(+moment__WEBPACK_IMPORTED_MODULE_4__(toDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('ToDate').value)))) {
                        this.FilterForm.get('FromDate').patchValue(frmDate);
                        this.initializeFilterChange();
                      } else if (!this.isArrayEqual(selectedRegions, this.FilterForm.get('SelectedRegions').value)) {
                        this.FilterForm.get('SelectedRegions').patchValue(selectedRegions);
                        this.initializeFilterChange();
                      } else if (cid && cid != this.FilterForm.get('SelectedCustomerId').value) {
                        this.FilterForm.get('SelectedCustomerId').patchValue(cid);
                        this.initializeFilterChange();
                      }
                    }

                  case 1:
                  case "end":
                    return _context4.stop();
                }
              }
            }, _callee4, this);
          }));
        }
      }, {
        key: "initializeFilterChange",
        value: function initializeFilterChange() {
          localStorage.setItem("filterChange", '1');
          window.location.reload();
        }
      }, {
        key: "regionChanged",
        value: function regionChanged(event) {
          this.filterDriverData();
        }
      }, {
        key: "onRegionSelect",
        value: function onRegionSelect() {
          var ids = [];
          this.SelectedRegionId = '';
          this.SelectedRegionId = ids.join();
          this.regionChanged();
        }
      }, {
        key: "customerChanged",
        value: function customerChanged() {
          this.filterDriverData();
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.filterDriverData();
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.filterDriverData();
        }
      }, {
        key: "setDatatableData",
        value: function setDatatableData(data) {
          this.MustGoSchedules = data.filter(function (x) {
            return x.LdPri == 1 || x.LdPri == 0;
          }).slice();
          this.ShouldGoSchedules = data.filter(function (x) {
            return x.LdPri == 2;
          }).slice();
          this.CouldGoSchedules = data.filter(function (x) {
            return x.LdPri == 3;
          }).slice();
        }
      }, {
        key: "refreshDatatable",
        value: function refreshDatatable() {
          this.dtElements.forEach(function (dtElement) {
            if (dtElement.dtInstance) {
              dtElement.dtInstance.then(function (dtInstance) {
                dtInstance.draw();
              });
            }
          });

          if (this.driverModal.modalDetails.display === "block") {
            this.showDriverDetails(this.driverModal.modalDetails.data);
          }
        }
      }, {
        key: "filterDriverData",
        value: function filterDriverData() {
          var _this53 = this;

          this.clearAllIntervals();
          this.searchLoadInterval = window.setTimeout(function () {
            _this53.getDispatcherLoads();

            _this53.autoRefreshLoads();
          }, 2000);
        }
      }, {
        key: "clearAllIntervals",
        value: function clearAllIntervals() {
          if (this.searchLoadInterval) {
            clearInterval(this.searchLoadInterval);
          }

          if (this.autoRefreshInterval) {
            clearInterval(this.autoRefreshInterval);
          }

          if (this.setCountryCenterInterval) {
            clearInterval(this.setCountryCenterInterval);
          }

          if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
          }
        }
      }, {
        key: "getDispatcherLoads",
        value: function getDispatcherLoads(statusId) {
          if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
          }

          var _priorities = [];
          this.FilterForm.get('SelectedPriorities').value.forEach(function (x) {
            return _priorities.push(x.Id);
          });
          this.SelectedPrioritiesId = _priorities;
          this.startAutoRefreshTimer();
          this.loadingData = false;
          this.refreshDatatable();
        }
      }, {
        key: "autoRefreshLoads",
        value: function autoRefreshLoads() {
          var _this54 = this;

          this.autoRefreshInterval = window.setInterval(function () {
            if (IsUserActive()) {
              _this54.getDispatcherLoads();
            }
          }, this.AUTO_REFRESH_TIME * 1000);
        }
      }, {
        key: "startAutoRefreshTimer",
        value: function startAutoRefreshTimer() {
          var _this55 = this;

          this.stopAutoRefreshTimer();
          this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
          this.autoRefreshTimerInterval = window.setInterval(function () {
            if (IsUserActive()) {
              if (_this55.autoRefreshTicks == 0) {
                _this55.autoRefreshTicks = _this55.AUTO_REFRESH_TIME;

                _this55.stopAutoRefreshTimer();
              } else {
                _this55.autoRefreshTicks--;
              }
            }
          }, 1000);
        }
      }, {
        key: "stopAutoRefreshTimer",
        value: function stopAutoRefreshTimer() {
          if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
          }
        }
      }, {
        key: "toggleExpandMapView",
        value: function toggleExpandMapView() {
          //this.toogleExpandMap = !this.toogleExpandMap;
          var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
          this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
        }
      }, {
        key: "toggleMapView",
        value: function toggleMapView() {
          // this.toogleMap = !this.toogleMap;
          var expandMapView = this.FilterForm.get('ToggleMap').value;
          this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
        }
      }, {
        key: "toggleGrids",
        value: function toggleGrids() {
          //this.toogleGrid = !this.toogleGrid;
          var toggleGrid = this.FilterForm.get('ToggleGrids').value;
          this.FilterForm.get('ToggleGrids').patchValue(!toggleGrid);
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "toggleDriverView",
        value: function toggleDriverView() {
          //this.toogleDriver = !this.toogleDriver;
          var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
          this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
        }
      }, {
        key: "showDriverDetails",
        value: function showDriverDetails(driver) {
          var _this56 = this;

          var infoWindow = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
          window.scrollTo(0, 0);
          this.driverModal = {
            modalDetails: {
              display: 'block',
              data: driver
            }
          };

          if (infoWindow && infoWindow.isOpen) {
            infoWindow.close();
          }

          this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"]();
          this.modalData = true;
          this.dispatcherService.getDriverAdditionalDetails(driver.Id).subscribe(function (data) {
            if (data) {
              _this56.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"](data);
              _this56.modalData = false;
            } else {
              _this56.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"]();

              _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning('Please try again later.', 'Something Went Wrong', 3000);

              _this56.modalData = false;
            }
          });
        }
      }, {
        key: "modalClose",
        value: function modalClose() {
          this.driverModal = {
            modalDetails: {
              display: 'none',
              data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverModel"]()
            }
          };
        }
      }, {
        key: "readOnlyModeSelection",
        value: function readOnlyModeSelection() {
          var readonlyKey = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].DSB_READONLY_KEY);

          if (readonlyKey == '') {
            this.disableControl = false;
          } else {
            this.disableControl = readonlyKey;
          }

          if (this.disableControl === true) {
            this.FilterForm.get('ToggleMap').patchValue(false);
          }
        }
      }, {
        key: "loadDropTicketDetails",
        value: function loadDropTicketDetails(invoiceHeaderId) {
          _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].showSliderPanel();

          this.dispatcherService.GetDropTicketDetails(invoiceHeaderId).subscribe(function (data) {
            if (data != '') {
              $("#invoice").html('');
              $("#invoice").html(data);
            } else {
              $("#invoice").html('');

              _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning('No Drop ticket details found.', null, 3000);
            }

            _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].appendHTMLSliderContent("#invoice");

            _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].hideSliderLoader();
          });
        }
      }, {
        key: "filterMapByStatus",
        value: function filterMapByStatus(statusId) {
          this.selectedMaplable = statusId;
          this.getDispatcherLoads(statusId);
        }
      }, {
        key: "getCustomerListByRegionId",
        value: function getCustomerListByRegionId(SelectedRegionId) {
          var _this57 = this;

          this.loadingData = true;
          this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(function (t2) {
            _this57.loadingData = false;
            _this57.customerList = t2;
          });
        }
      }, {
        key: "arraysEqual",
        value: function arraysEqual(a, b) {
          if (a === b) return true;
          if (a == null || b == null) return false;
          if (a.length != b.length) return false;

          for (var i = 0; i < a.length; ++i) {
            if (a[i] !== b[i]) return false;
          }

          return true;
        }
      }, {
        key: "isArrayEqual",
        value: function isArrayEqual(value, other) {
          var type = Object.prototype.toString.call(value);
          if (type !== Object.prototype.toString.call(other)) return false;
          if (['[object Array]', '[object Object]'].indexOf(type) < 0) return false;
          var valueLen = type === '[object Array]' ? value.length : Object.keys(value).length;
          var otherLen = type === '[object Array]' ? other.length : Object.keys(other).length;
          if (valueLen !== otherLen) return false;

          var compare = function compare(item1, item2) {};

          var match;

          if (type === '[object Array]') {
            for (var i = 0; i < valueLen; i++) {
              compare(value[i], other[i]);
            }
          } else {
            for (var key in value) {
              if (value.hasOwnProperty(key)) {
                compare(value[key], other[key]);
              }
            }
          }

          return true;
        }
      }, {
        key: "applyLoadsFilters",
        value: function applyLoadsFilters(filterForm) {
          this.FilterForm = filterForm;
          var isFilterChanged = this.IsFilterChanged();

          if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
            this.IsDataLoaded = true;
            this.filterDriverData();
          }
        }
      }]);

      return WhereIsMyDriverGridViewComponent;
    }();

    WhereIsMyDriverGridViewComponent.ɵfac = function WhereIsMyDriverGridViewComponent_Factory(t) {
      return new (t || WhereIsMyDriverGridViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]));
    };

    WhereIsMyDriverGridViewComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: WhereIsMyDriverGridViewComponent,
      selectors: [["app-where-is-my-driver-grid-view"]],
      viewQuery: function WhereIsMyDriverGridViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true, angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti",
        FilterForm: "FilterForm",
        IsFiltersLoaded: "IsFiltersLoaded",
        toogleMapFromParent: "toogleMapFromParent"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 48,
      vars: 18,
      consts: [["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "row"], [1, "col-sm-12"], [1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-eye", "mr5"], ["id", "grid-view", 1, "col-sm-12", 2, "margin-top", "15px", 3, "ngClass"], ["id", "sticky-head", 1, "sticky-header"], [1, "col-12", "text-right", "priority-tabs"], [1, "nav", "nav-pills", "float-right"], [1, "nav-item", 3, "click"], [1, "nav-link", "mustgo", 3, "ngClass"], [1, "nav-link", "shouldgo", 3, "ngClass"], [1, "nav-link", "couldgo", 3, "ngClass"], [3, "ngClass", 4, "ngIf"], ["type", "button", "id", "btnconfirm-memberInfo", "data-toggle", "modal", "data-target", "#confirm-memberInfo", "data-backdrop", "static", "data-keyboard", "false", 1, "hide-element"], ["id", "confirm-memberInfo", "tabindex", "-1", "role", "dialog", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "fs18", "f-bold", "mt0"], ["id", "member-datatable", 1, "table", "table-striped", "table-bordered", "table-hover"], [4, "ngFor", "ngForOf"], [1, "text-right"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", "btn-lg"], ["id", "invoice", 1, "hide-element"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [3, "ngClass"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-mustgo", "data-gridname", "19", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "PoNum"], ["data-key", "Name"], ["data-key", "DName"], ["data-key", "CName"], ["data-key", "Pckup"], ["data-key", "Loc"], ["data-key", "LT"], ["data-key", "PrdtNm"], ["data-key", "Qty"], ["data-key", "LdDate"], ["data-key", "Status"], ["data-key", "DROPTicketNum", 4, "ngIf"], ["data-key", "DROPTicketNum"], [1, "shouldgo", "mb5", 2, "color", "#f3c316 !important"], ["id", "table-shouldgo", "data-gridname", "20", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [1, "couldgo", "mb5", 2, "color", "#a7a2a2 !important"], ["id", "table-couldgo", "data-gridname", "21", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"]],
      template: function WhereIsMyDriverGridViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](0, WhereIsMyDriverGridViewComponent_div_0_Template, 2, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverGridViewComponent_Template_a_click_3_listener() {
            return ctx.toggleGrids();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "ul", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_12_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.MustGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Must Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_15_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.ShouldGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "a", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Should Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_18_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.CouldGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "a", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Could Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, WhereIsMyDriverGridViewComponent_div_21_Template, 34, 6, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, WhereIsMyDriverGridViewComponent_div_22_Template, 34, 6, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, WhereIsMyDriverGridViewComponent_div_23_Template, 34, 6, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](24, "button", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "h2", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Group Member Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "table", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](41, "LastSeenAt");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](43, WhereIsMyDriverGridViewComponent_tr_43_Template, 9, 4, "tr", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "button", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](47, "div", 24);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.loadingData);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", ctx.FilterForm.get("ToggleGrids").value == true ? "Show Grids" : "Hide Grids", " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](10, _c1, ctx.FilterForm.get("ToggleGrids").value));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](12, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.MustGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](14, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.ShouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](16, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.CouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(1) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(2) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.SelectedPrioritiesId.length == 0 || ctx.SelectedPrioritiesId.indexOf(3) > 0 - 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.memberInfo);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]],
      styles: ["table.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    position: fixed !important;\r\n}\r\n\r\ntable.dataTable.fixedHeader-floating[_ngcontent-%COMP%], table.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    top: 115px !important;\r\n\r\n}\r\n\r\n.sticky-header[_ngcontent-%COMP%] {\r\n    position: sticky;\r\n    top: 110px;\r\n    background-color: #ffffff;\r\n    padding: 5px 15px;\r\n    font-size: 20px;\r\n    z-index: 9;\r\n    text-align: right;\r\n    border-radius: 10px;\r\n    \r\n    box-shadow: 0 3px -1px 0 rgba(0,0,0,.1);\r\n    margin-bottom: 5px;\r\n}\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav[_ngcontent-%COMP%]    > li[_ngcontent-%COMP%]    > a[_ngcontent-%COMP%] {\r\n    position: relative;\r\n    display: block;\r\n    padding: 10px 15px 10px 15px;\r\n    color: #616161;\r\n    font-size: 14px;\r\n    font-weight: normal;\r\n    border-radius: 5px;\r\n    margin-right:5px;\r\n}\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav[_ngcontent-%COMP%]    > li[_ngcontent-%COMP%]    > a[_ngcontent-%COMP%]:hover{\r\ncolor:#ffffff;\r\n}\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-link.active[_ngcontent-%COMP%] {\r\n    color: #ffffff;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC93aGVyZS1pcy1teS1kcml2ZXItZ3JpZC12aWV3LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSwwQkFBMEI7QUFDOUI7O0FBRUE7SUFDSSxxQkFBcUI7O0FBRXpCOztBQUVBO0lBRUksZ0JBQWdCO0lBQ2hCLFVBQVU7SUFDVix5QkFBeUI7SUFDekIsaUJBQWlCO0lBQ2pCLGVBQWU7SUFDZixVQUFVO0lBQ1YsaUJBQWlCO0lBQ2pCLG1CQUFtQjtJQUNuQixxREFBcUQ7SUFFckQsdUNBQXVDO0lBQ3ZDLGtCQUFrQjtBQUN0Qjs7QUFHQTtJQUNJLGtCQUFrQjtJQUNsQixjQUFjO0lBQ2QsNEJBQTRCO0lBQzVCLGNBQWM7SUFDZCxlQUFlO0lBQ2YsbUJBQW1CO0lBQ25CLGtCQUFrQjtJQUNsQixnQkFBZ0I7QUFDcEI7O0FBRUE7QUFDQSxhQUFhO0FBQ2I7O0FBRUE7SUFDSSxjQUFjO0FBQ2xCIiwiZmlsZSI6InNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC93aGVyZS1pcy1teS1kcml2ZXItZ3JpZC12aWV3LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyJ0YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItbG9ja2VkIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcsIHRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4gICAgdG9wOiAxMTVweCAhaW1wb3J0YW50O1xyXG5cclxufVxyXG5cclxuLnN0aWNreS1oZWFkZXIge1xyXG4gICAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xyXG4gICAgcG9zaXRpb246IHN0aWNreTtcclxuICAgIHRvcDogMTEwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZmZmZmO1xyXG4gICAgcGFkZGluZzogNXB4IDE1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiA5O1xyXG4gICAgdGV4dC1hbGlnbjogcmlnaHQ7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG4gICAgLyogLXdlYmtpdC1ib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7ICovXHJcbiAgICAtbW96LWJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIGJveC1zaGFkb3c6IDAgM3B4IC0xcHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIG1hcmdpbi1ib3R0b206IDVweDtcclxufVxyXG5cclxuXHJcbi5wcmlvcml0eS10YWJzIC5uYXYgPiBsaSA+IGEge1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG4gICAgZGlzcGxheTogYmxvY2s7XHJcbiAgICBwYWRkaW5nOiAxMHB4IDE1cHggMTBweCAxNXB4O1xyXG4gICAgY29sb3I6ICM2MTYxNjE7XHJcbiAgICBmb250LXNpemU6IDE0cHg7XHJcbiAgICBmb250LXdlaWdodDogbm9ybWFsO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgbWFyZ2luLXJpZ2h0OjVweDtcclxufVxyXG5cclxuLnByaW9yaXR5LXRhYnMgLm5hdiA+IGxpID4gYTpob3ZlcntcclxuY29sb3I6I2ZmZmZmZjtcclxufVxyXG5cclxuLnByaW9yaXR5LXRhYnMgLm5hdi1waWxscyAubmF2LWxpbmsuYWN0aXZlIHtcclxuICAgIGNvbG9yOiAjZmZmZmZmO1xyXG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](WhereIsMyDriverGridViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-where-is-my-driver-grid-view',
          templateUrl: './where-is-my-driver-grid-view.component.html',
          styleUrls: ['./where-is-my-driver-grid-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]
        }, {
          type: src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]
        }];
      }, {
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        FilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsFiltersLoaded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }],
        selectedDriverLoad: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['SelectedDriverLoad', {
            read: angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"],
            "static": false
          }]
        }],
        toogleMapFromParent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-map-view.component.ts": function srcAppDispatcherDispatcherDashboardWhereIsMyDriverMapViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverMapViewComponent", function () {
      return WhereIsMyDriverMapViewComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
    /* harmony import */


    var src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/sendbird.component */
    "./src/app/shared-components/sendbird/sendbird.component.ts");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/services/sendbird.service */
    "./src/app/shared-components/sendbird/services/sendbird.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var _directives_click_outside_directive__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ../../directives/click-outside.directive */
    "./src/app/directives/click-outside.directive.ts");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var agm_direction__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! agm-direction */
    "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js");

    var _c0 = ["SelectedDriverLoad"];

    function WhereIsMyDriverMapViewComponent_option_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "option", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var customer_r10 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", customer_r10.CompanyId)("selected", ctx_r0.SelectedCustomerId == customer_r10.CompanyId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", customer_r10.CompanyName, " ");
      }
    }

    function WhereIsMyDriverMapViewComponent_div_19_Template(rf, ctx) {
      if (rf & 1) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "ng-multiselect-dropdown", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_ngModelChange_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r11.SelectedStates = $event;
        })("onSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onSelect_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r13.onStateSelect($event);
        })("onDeSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onDeSelect_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r14.onStateUnselect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "ng-multiselect-dropdown", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_ngModelChange_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.SelectedPriorities = $event;
        })("onSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onSelect_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r16.onPrioritySelect($event);
        })("onDeSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onDeSelect_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r17.onPriorityUnselect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "ng-multiselect-dropdown", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_ngModelChange_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r18.SelectedDispachers = $event;
        })("onSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onSelect_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r19.onDispacherSelect($event);
        })("onDeSelect", function WhereIsMyDriverMapViewComponent_div_19_Template_ng_multiselect_dropdown_onDeSelect_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r20.onDispacherUnselect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r1.SelectedStates)("settings", ctx_r1.StateDdlSettings)("placeholder", "Select States")("data", ctx_r1.RegionStates);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r1.SelectedPriorities)("settings", ctx_r1.PriorityDdlSettings)("placeholder", "Select Priority")("data", ctx_r1.LoadPriorities);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r1.SelectedDispachers)("settings", ctx_r1.PriorityDdlSettings)("placeholder", "Select Dispacher")("data", ctx_r1.RegionDispachers);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_52_p_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "p", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Note:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, " Click truck to hide routes.");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_52_ng_template_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "p", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Note:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, " Click truck to view routes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c1 = function _c1(a0, a1) {
      return {
        lat: a0,
        lng: a1
      };
    };

    var _c2 = function _c2(a0) {
      return {
        strokeColor: a0
      };
    };

    var _c3 = function _c3(a1) {
      return {
        suppressMarkers: true,
        polylineOptions: a1
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_container_52_agm_direction_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "agm-direction", 74);
      }

      if (rf & 2) {
        var driver_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("origin", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](4, _c1, driver_r21.Lat, driver_r21.Lng))("destination", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](7, _c1, driver_r21.dLat, driver_r21.dLng))("visible", driver_r21.routeShow)("renderOptions", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](12, _c3, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](10, _c2, driver_r21.statusColor)));
      }
    }

    var _c4 = function _c4() {
      return {
        "height": 50,
        "width": 50
      };
    };

    var _c5 = function _c5(a0, a1) {
      return {
        "url": a0,
        "scaledSize": a1
      };
    };

    var _c6 = function _c6() {
      return {
        x: 30,
        y: 30
      };
    };

    var _c7 = function _c7(a2) {
      return {
        width: 65,
        height: 65,
        anchor: a2
      };
    };

    var _c8 = function _c8() {
      return {
        x: 20,
        y: 20
      };
    };

    var _c9 = function _c9(a10, a11, a12) {
      return {
        position: "absolute",
        top: "-11px",
        left: "-14px",
        background: "#fa9393",
        display: "inline-flex",
        width: "18px",
        height: "18px",
        color: "red",
        fontWeight: "bold",
        fontSize: "18px",
        scaledSize: a10,
        labelOrigin: a11,
        text: a12
      };
    };

    var _c10 = function _c10() {
      return {
        "height": 25,
        "width": 25
      };
    };

    var _c11 = function _c11(a1) {
      return {
        "url": "https://maps.google.com/mapfiles/ms/icons/red-dot.png",
        "scaledSize": a1
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_container_52_Template(rf, ctx) {
      if (rf & 1) {
        var _r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "agm-marker", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("mouseOver", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_agm_marker_mouseOver_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

          var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r30.mouseHoverMarker(_r23, $event);
        })("markerClick", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_agm_marker_markerClick_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var indx_r22 = ctx.index;

          var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r32.showHideRoutes(indx_r22);
        })("mouseOut", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_agm_marker_mouseOut_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var indx_r22 = ctx.index;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r33.mouseHoveOutMarker(null, $event, indx_r22);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "agm-info-window", 62, 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Driver Name: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Contact Number: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "a", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Last UpdatedAt: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "a", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_a_click_19_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var driver_r21 = ctx.$implicit;

          var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

          var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r34.showDriverDetails(driver_r21, _r23);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Show more");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "p", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "a", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_a_click_22_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var driver_r21 = ctx.$implicit;

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r35.doChat(driver_r21.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](23, "span", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, WhereIsMyDriverMapViewComponent_ng_container_52_p_24_Template, 4, 0, "p", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](25, WhereIsMyDriverMapViewComponent_ng_container_52_ng_template_25_Template, 4, 0, "ng-template", null, 68, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "agm-marker", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("mouseOver", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_agm_marker_mouseOver_27_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](29);

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r36.mouseHoverMarker(_r27, $event);
        })("mouseOut", function WhereIsMyDriverMapViewComponent_ng_container_52_Template_agm_marker_mouseOut_27_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](29);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r37.mouseHoveOutMarker(_r27, $event, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "agm-info-window", 70, 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "Engaged Driver : ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37, "Drop Location: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, WhereIsMyDriverMapViewComponent_ng_container_52_agm_direction_39_Template, 1, 14, "agm-direction", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var driver_r21 = ctx.$implicit;

        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("latitude", driver_r21.Lat)("longitude", driver_r21.Lng)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](23, _c5, "src/assets/truck-" + driver_r21.SttsId + ".svg", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction0"](22, _c4)))("label", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction3"](30, _c9, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](27, _c7, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction0"](26, _c6)), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction0"](29, _c8), driver_r21.FuelRetainCount.toString()));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", driver_r21.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("href", "tel:", driver_r21.PhNo, "", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("title", "Call ", driver_r21.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r21.PhNo);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", driver_r21.AppLastUpdatedDate, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("title", "Show ", driver_r21.Name, " more details");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("title", "Chat with ", driver_r21.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r21.routeShow)("ngIfElse", _r25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("latitude", driver_r21.dLat)("longitude", driver_r21.dLng)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](35, _c11, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction0"](34, _c10)));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disableAutoPan", false)("maxWidth", 200);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", driver_r21.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r21.Loc);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r21.dLat);
      }
    }

    var _c12 = function _c12(a0, a1) {
      return {
        "fa-arrow-circle-right": a0,
        "fa-arrow-circle-left": a1
      };
    };

    function WhereIsMyDriverMapViewComponent_div_54_Template(rf, ctx) {
      if (rf & 1) {
        var _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_div_54_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r39);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r38.toggleDriverView();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](1, _c12, !ctx_r3.FilterForm.get("ToggleDriverView").value, ctx_r3.FilterForm.get("ToggleDriverView").value));
      }
    }

    var _c13 = function _c13(a0) {
      return {
        "activeRoute": a0
      };
    };

    var _c14 = function _c14(a0) {
      return {
        "color": a0
      };
    };

    function WhereIsMyDriverMapViewComponent_div_62_Template(rf, ctx) {
      if (rf & 1) {
        var _r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "span", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_div_62_Template_div_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r43);

          var indx_r41 = ctx.index;

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r42.showHideRoutes(indx_r41);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "span", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "a", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_div_62_Template_a_click_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r43);

          var driver_r40 = ctx.$implicit;

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r44.doChat(driver_r40.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "span", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "a", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_div_62_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r43);

          var driver_r40 = ctx.$implicit;

          var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r45.showDriverDetails(driver_r40, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](13, "span", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r40 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", driver_r40.IsOnline ? "live" : "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r40.Intl);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("title", "Click to ", driver_r40.routeShow ? "hide" : "show", " routes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](8, _c13, driver_r40.routeShow))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](10, _c14, driver_r40.routeShow ? driver_r40.statusColor : "#2b2b2b"));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r40.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r40.PhNo);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("title", "Chat with ", driver_r40.Name, "");
      }
    }

    function WhereIsMyDriverMapViewComponent_div_63_Template(rf, ctx) {
      if (rf & 1) {
        var _r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "span", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "span", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "a", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_div_63_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r49);

          var driver_r46 = ctx.$implicit;

          var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r48.showDriverDetails(driver_r46, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "span", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r46 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r46.Intl);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r46.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r46.PhNo);
      }
    }

    function WhereIsMyDriverMapViewComponent_tr_83_Template(rf, ctx) {
      if (rf & 1) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var member_r50 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r50.nickname);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r50.userId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r50.connectionStatus);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](member_r50.lastSeenAt);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_container_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Shift : ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, " Not Available");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_23_ng_container_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var shift_r62 = ctx.$implicit;
        var i_r63 = ctx.index;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("Shift (", i_r63 + 1, "): ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", shift_r62, "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](0, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_23_ng_container_0_Template, 5, 2, "ng-container", 32);
      }

      if (rf & 2) {
        var ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r56.selectedDriverDetails.Shifts);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_container_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "No Data Available.");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_span_6_tbody_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var currentLoad_r71 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](currentLoad_r71.PrdtNm);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](currentLoad_r71.Qty);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Ongoing");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "table");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Fuel Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_span_6_tbody_9_Template, 6, 2, "tbody", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", row_r65.OngoingData);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_td_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Compartment_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](2, 2, Compartment_r77.Capacity, "1.0-2"), " ", row_r65.DefaultUOM == 2 ? " L" : " G", "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_ng_template_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_td_3_Template, 3, 5, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_ng_template_4_Template, 2, 0, "ng-template", null, 120, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Compartment_r77 = ctx.$implicit;

        var _r79 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](5);

        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](Compartment_r77.CompartmentId ? Compartment_r77.CompartmentId : "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", Compartment_r77.Capacity > 0)("ngIfElse", _r79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("0 ", row_r65.DefaultUOM == 2 ? " L" : " G", "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_tbody_1_Template, 10, 4, "tbody", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", row_r65.Compartments);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_td_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](2, 2, row_r65.FuelCapacity, "1.0-2"), "", row_r65.DefaultUOM == 2 ? " L" : " G", " ");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_ng_template_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_td_3_Template, 3, 5, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_ng_template_4_Template, 2, 0, "ng-template", null, 121, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r86 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](5);

        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r65.FuelCapacity > 0)("ngIfElse", _r86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" 0 ", row_r65.DefaultUOM == 2 ? " L" : " G", "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_container_1_Template, 2, 1, "ng-container", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_ng_template_3_Template, 10, 3, "ng-template", null, 118, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var _r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r65.Compartments.length > 0)("ngIfElse", _r74);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_td_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var fuelDetail_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](2, 2, fuelDetail_r92.CompartmentCapacity, "1.0-2"), "", fuelDetail_r92.UOM == 2 ? " L" : " G", "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_ng_template_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_td_3_Template, 3, 5, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_ng_template_4_Template, 2, 0, "ng-template", null, 122, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "td", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](10, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var fuelDetail_r92 = ctx.$implicit;

        var _r94 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fuelDetail_r92.CompartmentId ? fuelDetail_r92.CompartmentId : "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", fuelDetail_r92.CompartmentCapacity > 0)("ngIfElse", _r94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fuelDetail_r92.ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind2"](10, 6, fuelDetail_r92.Quantity, "1.0-2"), "", fuelDetail_r92.UOM == 2 ? " L" : " G", "");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](0, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_tbody_0_Template, 11, 9, "tbody", 32);
      }

      if (rf & 2) {
        var row_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", row_r65.TrailerFuelRetains);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](6, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_span_6_Template, 10, 1, "span", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Retain");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "table");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "th", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Compartment");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "th", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Fuel Capacity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Fuel Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Fuel Remaining");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_container_21_Template, 5, 2, "ng-container", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_ng_template_23_Template, 1, 1, "ng-template", null, 117, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r65 = ctx.$implicit;

        var _r68 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r65.TruckId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](row_r65.LicencePlate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", row_r65.OngoingData.length > 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !row_r65.TrailerFuelRetains.length)("ngIfElse", _r68);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](0, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_tr_0_Template, 25, 5, "tr", 32);
      }

      if (rf & 2) {
        var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r60.selectedDriverDetails.Trailers);
      }
    }

    var _c15 = function _c15(a3) {
      return {
        "modal": true,
        "left": true,
        "fade": true,
        "show": a3
      };
    };

    var _c16 = function _c16(a0) {
      return {
        "display": a0
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_template_89_Template(rf, ctx) {
      if (rf & 1) {
        var _r99 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, WhereIsMyDriverMapViewComponent_ng_template_89_div_2_Template, 2, 0, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "h4", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "a", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_ng_template_89_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r99);

          var modalDetails_r51 = ctx.modalDetails;

          var ctx_r98 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r98.doChat(modalDetails_r51.data.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "span", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "a", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_ng_template_89_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r99);

          var ctx_r100 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r100.modalClose();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "i", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, WhereIsMyDriverMapViewComponent_ng_template_89_div_12_Template, 2, 0, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Licence Number: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](17, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, WhereIsMyDriverMapViewComponent_ng_template_89_ng_container_18_Template, 5, 0, "ng-container", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Contact Number: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "a", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_23_Template, 1, 1, "ng-template", null, 99, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "h4", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Trailer Details");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](31, "span", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "table", 108, 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "th", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Trailer ID");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "th", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "License");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "th", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, " Details ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, WhereIsMyDriverMapViewComponent_ng_template_89_ng_container_46_Template, 3, 0, "ng-container", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](47, WhereIsMyDriverMapViewComponent_ng_template_89_ng_template_47_Template, 1, 1, "ng-template", null, 113, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r51 = ctx.modalDetails;

        var _r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](24);

        var _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](48);

        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](12, _c15, modalDetails_r51.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](14, _c16, modalDetails_r51.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.loadingData);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r8.selectedDriverDetails.Name, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.modalData);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", ctx_r8.selectedDriverDetails.License, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r8.selectedDriverDetails.Shifts.length)("ngIfElse", _r55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate1"]("href", "tel:", ctx_r8.selectedDriverDetails.ContactNumnber, "", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r8.selectedDriverDetails.ContactNumnber);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r8.selectedDriverDetails.Trailers.length)("ngIfElse", _r59);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_91_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainer"](0);
      }
    }

    var _c17 = function _c17(a0, a1, a2, a3) {
      return {
        "fadeIn": a0,
        "display_hide": a1,
        "col-sm-9": a2,
        "col-sm-12": a3
      };
    };

    var _c18 = function _c18(a0) {
      return {
        "height": a0
      };
    };

    var _c19 = function _c19(a0, a1, a2, a3) {
      return {
        "col-sm-3": a0,
        "absolute_driver": a1,
        "hide_absolute_driver": a2,
        "display_hide": a3
      };
    };

    var WhereIsMyDriverMapViewComponent = /*#__PURE__*/function () {
      function WhereIsMyDriverMapViewComponent(dispatcherService, chatService, carrierService) {
        _classCallCheck(this, WhereIsMyDriverMapViewComponent);

        this.dispatcherService = dispatcherService;
        this.chatService = chatService;
        this.carrierService = carrierService;
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subscription"]();
        this.previousInfowindow = null;
        this.previousInfowindowIndex = null;
        this.zoomLevel = 5;
        this.centerLoactionLat = 39.1175;
        this.centerLoactionLng = -103.8784;
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_4__().add(1, 'year').toDate();
        this.TodaysDate = moment__WEBPACK_IMPORTED_MODULE_4__().format('MM/DD/YYYY');
        this.AUTO_REFRESH_TIME = 300; // seconds

        this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
        this.driverModal = {
          modalDetails: {
            display: 'none',
            data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverModel"]()
          }
        };
        this.UserCountry = "";
        this.CountryCentre = {
          USA: {
            lat: 39.11757961,
            lng: -103.8784
          },
          CAN: {
            lat: 57.88251631,
            lng: -98.54842922
          }
        };
        this.screenOptions = {
          position: 6
        };
        this.Drivers = [];
        this.OfflineDrivers = [];
        this.allLoads = [];
        this.OnGoingLoads = [];
        this.CloneOnGoingLoads = [];
        this.MustGoSchedules = [];
        this.ShouldGoSchedules = [];
        this.CouldGoSchedules = [];
        this.selectedDriverLoads = [];
        this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"]();
        this.Regions = [];
        this.RegionStates = [];
        this.RegionDispachers = [];
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.RegionDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleFilter = false;
        this.toogleDriver = false;
        this.toogleGrid = false;
        this.toogleExpandMap = false;
        this.customerList = [];
        this.dtMustGoOptions = {};
        this.dtShouldGoOptions = {};
        this.dtCouldGoOptions = {};
        this.selectedDriverLoadsdtOptions = {};
        this.selectedDriverLoadsdtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtMustGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtShouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtCouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.loadingData = true;
        this.modalData = true;
        this.backgroudchatDefault = [];
        this.memberInfo = [];
        this.disableControl = false;
        this.currentOngoingLoadDetails = [];
      }

      _createClass(WhereIsMyDriverMapViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this58 = this;

          this.readOnlyModeSelection();
          this.subscribeFormChanges();
          this.dispatcherService.getDispatcherCountry().subscribe(function (data) {
            _this58.UserCountry = data;
            _this58.FuelUnit = _this58.UserCountry === 'USA' ? 'Gallons' : 'Litres';

            _this58.setMapCenter();
          });
          this.chatService.loaderDetails.subscribe(function (data) {
            if (data != undefined) {
              _this58.loadingData = data;
            }
          });
          this.chatService.memberInfoDetails.subscribe(function (data) {
            if (data != undefined) {
              _this58.memberInfo = data;
              _this58.loadingData = false;
              jQuery('#btnconfirm-memberInfo').click();
            }
          });
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.singleMulti && change.singleMulti.currentValue) {}

          if (change.SelectedRegions && change.SelectedRegions.currentValue) this.onRegionSelect(); // if (change.SelectedCustomerId && change.SelectedCustomerId.currentValue)
          //     this.customerChanged();
          // if ((change.FromDate && change.FromDate.currentValue) || (change.ToDate && change.ToDate.currentValue))
          //     this.filterDriverData();
          // if (change.SelectedStates && change.SelectedStates.currentValue)
          //     this.filterDriverData();
          // if (change.SelectedPriorities && change.SelectedPriorities.currentValue)
          //     this.filterDriverData();
          // if (change.SelectedDispachers && change.SelectedDispachers.currentValue)
          //     this.filterDriverData();
          // if (change.SelectedCarriers && change.SelectedCarriers.currentValue)
          // this.filterDriverData();
          // if (change.IsShowCarrierManaged)
          // this.filterDriverData();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getDispatcherLoads();
          this.autoRefreshLoads();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.clearAllIntervals();
          this.unSubscribeFormChanges();
          if (this.changeFilterValueIntervalForMultiWindow) clearInterval(this.changeFilterValueIntervalForMultiWindow);
          this.dtCouldGoTrigger.unsubscribe();
          this.dtShouldGoTrigger.unsubscribe();
          this.dtMustGoTrigger.unsubscribe();
        } // subscribeFormChanges() {
        //     this.subscriptions.add(this.FilterForm.valueChanges
        //         .subscribe(change => {
        //             var ids = [];
        //             var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
        //             selectedRegions.forEach(res => { ids.push(res.Id) });
        //             var selectedRegionId = ids.join();
        //             this.SelectedRegionId = selectedRegionId;
        //             this.filterDriverData()
        //             //this.SaveFilters();
        //         }))
        // }

      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this59 = this;

          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            _this59.filterDriverData();
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(function (change) {
            _this59.filterDriverData();
          }));
        }
      }, {
        key: "unSubscribeFormChanges",
        value: function unSubscribeFormChanges() {
          if (this.subscriptions) {
            this.subscriptions.unsubscribe();
          }
        }
      }, {
        key: "checkFilterValueChange",
        value: function checkFilterValueChange() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
            var frmDate, toDate, selectedRegions, selectedStates, selectedDispachers, cid;
            return regeneratorRuntime.wrap(function _callee5$(_context5) {
              while (1) {
                switch (_context5.prev = _context5.next) {
                  case 0:
                    if (this.singleMulti == 2) {
                      frmDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY);
                      toDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY);
                      selectedRegions = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_REGION_KEY);
                      selectedRegions == "" ? selectedRegions = [] : '';
                      selectedStates = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_SELECTEDSTATES_KEY);
                      selectedStates == "" ? selectedStates = [] : '';
                      selectedDispachers = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_SELECTEDDISPACHER_KEY);
                      cid = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_CUSTOMER_KEY);

                      if (frmDate != '' && toDate != '' && (!(+moment__WEBPACK_IMPORTED_MODULE_4__(frmDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('FromDate').value)) || !(+moment__WEBPACK_IMPORTED_MODULE_4__(toDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('ToDate').value)))) {
                        this.FilterForm.get('FromDate').patchValue(frmDate); // this.ToDate = this.ToDate;

                        this.initializeFilterChange();
                      } else if (!this.isArrayEqual(selectedRegions, this.FilterForm.get('selectedRegions').value)) {
                        this.FilterForm.get('selectedRegions').patchValue(selectedRegions);
                        this.initializeFilterChange();
                      } else if (cid && cid != this.FilterForm.get('SelectedCustomerId')) {
                        this.FilterForm.get('SelectedCustomerId').patchValue(cid);
                        this.initializeFilterChange();
                      }
                    }

                  case 1:
                  case "end":
                    return _context5.stop();
                }
              }
            }, _callee5, this);
          }));
        }
      }, {
        key: "initializeFilterChange",
        value: function initializeFilterChange() {
          localStorage.setItem("filterChange", '1');
          window.location.reload();
        }
      }, {
        key: "regionChanged",
        value: function regionChanged(event) {
          this.CloneOnGoingLoads = [];
          this.filterDriverData();
        }
      }, {
        key: "onRegionSelect",
        value: function onRegionSelect() {
          var ids = [];
          this.SelectedRegionId = '';
          this.FilterForm.get('SelectedRegions').value.forEach(function (res) {
            ids.push(res.Id);
          });
          this.SelectedRegionId = ids.join();
          this.regionChanged();
        }
      }, {
        key: "customerChanged",
        value: function customerChanged(event) {
          this.filterDriverData();
        }
      }, {
        key: "setRegionStates",
        value: function setRegionStates() {
          var _this60 = this;

          this.RegionStates = [];
          this.Regions.map(function (m) {
            if (_this60.FilterForm.get('SelectedRegions').value.find(function (f) {
              return f.Id == m.Id;
            })) {
              if (m && m.States && m.States.length > 0) {
                _this60.RegionStates = _this60.RegionStates.concat(m.States);
              }
            }
          });
        }
      }, {
        key: "setRegionDispachers",
        value: function setRegionDispachers() {
          var _this61 = this;

          this.Regions.map(function (m) {
            if (_this61.FilterForm.get('SelectedRegions').value.find(function (f) {
              return f.Id == m.Id;
            })) {
              if (m && m.Dispatchers && m.Dispatchers.length > 0) {
                _this61.RegionDispachers = _this61.RegionDispachers.concat(m.Dispatchers);
              }
            }
          });
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.filterDriverData();
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.filterDriverData();
        }
      }, {
        key: "onStateSelect",
        value: function onStateSelect(event) {
          this.filterDriverData();
        }
      }, {
        key: "onStateUnselect",
        value: function onStateUnselect(event) {
          this.filterDriverData();
        }
      }, {
        key: "onPrioritySelect",
        value: function onPrioritySelect(event) {
          this.filterDriverData();
        }
      }, {
        key: "onPriorityUnselect",
        value: function onPriorityUnselect(event) {
          this.filterDriverData();
        }
      }, {
        key: "onDispacherSelect",
        value: function onDispacherSelect(event) {
          this.filterDriverData();
        }
      }, {
        key: "onDispacherUnselect",
        value: function onDispacherUnselect(event) {
          this.filterDriverData();
        }
      }, {
        key: "setMapCenter",
        value: function setMapCenter() {
          var _this62 = this;

          if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(function () {
              _this62.centerLoactionLat = _this62.CountryCentre[_this62.UserCountry].lat;
              _this62.centerLoactionLng = _this62.CountryCentre[_this62.UserCountry].lng;

              if (_this62.googleMap && _this62.OnGoingLoads.length == 0) {
                var bounds = new google.maps.LatLngBounds();
                bounds.extend(new google.maps.LatLng(_this62.centerLoactionLat, _this62.centerLoactionLng));

                _this62.googleMap.fitBounds(bounds);

                _this62.googleMap.setZoom(5);
              } else {
                var _bounds = new google.maps.LatLngBounds();

                _this62.OnGoingLoads.forEach(function (x) {
                  x.statusColor = src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["routesColor"][x.SttsId];

                  _bounds.extend(new google.maps.LatLng(x.Lat, x.Lng));
                });

                _this62.googleMap.fitBounds(_bounds);

                var locationbounds = new google.maps.LatLngBounds();

                _this62.OnGoingLoads.forEach(function (x) {
                  locationbounds.extend(new google.maps.LatLng(x.dLat, x.dLng));
                });

                if (_this62.googleMap && locationbounds) {
                  _this62.googleMap.setCenter(locationbounds.getCenter());
                }

                _this62.googleMap.setZoom(5);
              }
            }, 500);
          }
        }
      }, {
        key: "searchDrivers",
        value: function searchDrivers(event) {
          this.SearchedKeyword = event.target.value;
          this.filterDriverData();
        }
      }, {
        key: "setDatatableData",
        value: function setDatatableData(data) {
          this.MustGoSchedules = data.filter(function (x) {
            return x.LdPri == 1 || x.LdPri == 0;
          }).slice();
          this.ShouldGoSchedules = data.filter(function (x) {
            return x.LdPri == 2;
          }).slice();
          this.CouldGoSchedules = data.filter(function (x) {
            return x.LdPri == 3;
          }).slice();
        }
      }, {
        key: "refreshDatatable",
        value: function refreshDatatable() {
          this.dtElements.forEach(function (dtElement) {
            if (dtElement.dtInstance) {
              dtElement.dtInstance.then(function (dtInstance) {
                dtInstance.draw();
              });
            }
          }); //this.dtMustGoTrigger.next();
          //this.dtShouldGoTrigger.next();
          //this.dtCouldGoTrigger.next();

          if (this.driverModal.modalDetails.display === "block") {
            this.showDriverDetails(this.driverModal.modalDetails.data);
          }
        }
      }, {
        key: "filterDriverData",
        value: function filterDriverData() {
          var _this63 = this;

          this.clearAllIntervals();
          this.loadingData = true;
          this.searchLoadInterval = window.setTimeout(function () {
            _this63.getDispatcherLoads();

            _this63.autoRefreshLoads();
          }, 1000);
        }
      }, {
        key: "clearAllIntervals",
        value: function clearAllIntervals() {
          if (this.searchLoadInterval) {
            clearInterval(this.searchLoadInterval);
          }

          if (this.autoRefreshInterval) {
            clearInterval(this.autoRefreshInterval);
          }

          if (this.setCountryCenterInterval) {
            clearInterval(this.setCountryCenterInterval);
          }

          if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
          }
        }
      }, {
        key: "getDispatcherLoads",
        value: function getDispatcherLoads(statusId) {
          var _this64 = this;

          if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
          }

          var _states = [];
          this.FilterForm.get('SelectedStates').value.forEach(function (x) {
            return _states.push(x.Code);
          });
          var _priorities = [];
          this.FilterForm.get('SelectedPriorities').value.forEach(function (x) {
            return _priorities.push(x.Id);
          });
          this.SelectedPrioritiesId = _priorities;
          var selectedDispacherIds = '';
          this.FilterForm.get('SelectedDispachers').value.map(function (m) {
            if (selectedDispacherIds == '') selectedDispacherIds = m.Id;else selectedDispacherIds += ',' + m.Id;
          });
          var _carriers = [];
          this.FilterForm.get('SelectedCarriers').value.forEach(function (x) {
            return _carriers.push(x.Id);
          });
          var _locAttribute = [];
          this.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
            return _locAttribute.push(x.Id);
          });

          var _locAttributeIds = _locAttribute.join();

          var _selectedRegion = [];
          this.FilterForm.get('SelectedRegions').value.forEach(function (x) {
            return _selectedRegion.push(x.Id);
          });

          var _selectedRegionIds = _selectedRegion.join();

          var inputs = {
            RegionId: _selectedRegionIds == null ? '' : _selectedRegionIds,
            States: _states,
            Priorities: _priorities,
            FromDate: this.FilterForm.get('FromDate').value,
            ToDate: this.FilterForm.get('ToDate').value,
            DriverSearch: this.SearchedKeyword,
            DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
            CustomerId: this.FilterForm.get('SelectedCustomerId').value,
            Carriers: _carriers,
            IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureType: _locAttributeIds
          };
          this.loadingData = true;
          var data = this.CloneOnGoingLoads;
          var isFilter = false;

          if (statusId && this.CloneOnGoingLoads && this.CloneOnGoingLoads.length > 0) {
            data = data.filter(function (f) {
              return f.SttsId == statusId;
            });
            isFilter = true;
          }

          if (!isFilter) {
            this.dispatcherService.getOnGoingLoads(inputs).subscribe(function (data) {
              _this64.CloneOnGoingLoads = data;

              _this64.initailizeOnGoingLoad(data);
            });
          } else this.initailizeOnGoingLoad(data); //this.refreshDatatable();
          //MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDSTATES_KEY, this.SelectedStates);
          // MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDPRIORY_KEY, this.SelectedPriorities);
          //  MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDDISPACHER_KEY, this.SelectedDispachers);

        }
      }, {
        key: "initailizeOnGoingLoad",
        value: function initailizeOnGoingLoad(data) {
          var _this65 = this;

          this.OnGoingLoads = data;
          this.currentOngoingLoadDetails = this.OnGoingLoads.filter(function (t) {
            return t.SttsId != null && (t.SttsId == 1 || t.SttsId == 11 || t.SttsId == 12 || t.SttsId == 18 || t.SttsId == 20);
          }); // this.OnGoingLoads = data.filter(x => x.Lat != null && x.Lng != null);
          //   .map(m => {
          //       if (m.AppLastUpdatedDate)
          //           var date = new Date(m.lastUpdateTimeDiff + ' UTC');
          //       m.lastUpdateTimeDiff = date.toString();
          //       return m;
          //   } 
          //);

          this.Drivers = this.OnGoingLoads.filter(function (thing, i, arr) {
            return arr.indexOf(arr.find(function (t) {
              return t.Id === thing.Id;
            })) === i;
          });
          this.Drivers = this.Drivers.filter(function (x) {
            return x.Name != null && x.Name != undefined && x.Name.trim() != '';
          }); //last location not available

          this.OfflineDrivers = [];
          var driverFilter = [];
          data && data.map(function (m) {
            if (!driverFilter.find(function (f) {
              return f && f.Name == m.Name;
            })) {
              driverFilter.push(m);
              if (m.Lat == null && m.Lng == null && m.Name != null && m.Name != undefined && m.Name.trim() != '') _this65.Drivers && _this65.Drivers.filter(function (f) {
                return f.Name == m.Name;
              }).length > 0 ? '' : _this65.OfflineDrivers.push(m);
            }
          }); //this.OfflineDrivers = data.filter(x => x.Lat == null && x.Lng == null && x.Name != null && x.Name != undefined && x.Name.trim() != '');

          this.setMapCenter();
          this.startAutoRefreshTimer();
          this.loadingData = false;
          this.addDrivertoBackground();
        } //private  getMinutesBetweenDates(startDate) {
        //      var endDate = new Date();
        //      startDate = new Date(startDate);
        //    var diff = endDate.getTime() - startDate.getTime();
        //    return (diff / 60000);
        //  }

      }, {
        key: "autoRefreshLoads",
        value: function autoRefreshLoads() {
          var _this66 = this;

          this.autoRefreshInterval = window.setInterval(function () {
            if (IsUserActive()) {
              _this66.getDispatcherLoads();
            }
          }, this.AUTO_REFRESH_TIME * 1000);
        }
      }, {
        key: "startAutoRefreshTimer",
        value: function startAutoRefreshTimer() {
          var _this67 = this;

          this.stopAutoRefreshTimer();
          this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
          this.autoRefreshTimerInterval = window.setInterval(function () {
            if (IsUserActive()) {
              if (_this67.autoRefreshTicks == 0) {
                _this67.autoRefreshTicks = _this67.AUTO_REFRESH_TIME;

                _this67.stopAutoRefreshTimer();
              } else {
                _this67.autoRefreshTicks--;
              }
            }
          }, 1000);
        }
      }, {
        key: "stopAutoRefreshTimer",
        value: function stopAutoRefreshTimer() {
          if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
          }
        }
      }, {
        key: "mapReady",
        value: function mapReady(map) {
          this.googleMap = map;
          this.setMapCenter();
        }
      }, {
        key: "setZoomLevel",
        value: function setZoomLevel() {
          if (this.OnGoingLoads.length == 0) {
            this.setMapCenter();
          } else {
            this.zoomLevel = 8; // default zoom level
          }
        }
      }, {
        key: "toggleExpandMapView",
        value: function toggleExpandMapView() {
          /// this.toogleExpandMap = !this.toogleExpandMap;
          var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
          this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
        }
      }, {
        key: "toggleMapView",
        value: function toggleMapView() {
          //this.toogleMap = !this.toogleMap;
          var expandMapView = this.FilterForm.get('ToggleMap').value;
          this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
        }
      }, {
        key: "toggleGrids",
        value: function toggleGrids() {
          //this.toogleGrid = !this.toogleGrid;
          var toggleGrid = this.FilterForm.get('ToggleGrids').value;
          this.FilterForm.get('ToggleGrids').patchValue(!toggleGrid);
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "toggleDriverView",
        value: function toggleDriverView() {
          // this.toogleDriver = !this.toogleDriver;
          var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
          this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
        }
      }, {
        key: "addDrivertoBackground",
        value: function addDrivertoBackground() {
          var _this68 = this;

          this.Drivers.forEach(function (xItem) {
            _this68.backgroudchatDefault.push(xItem.Id);
          });
          this.sendbirdComponent.IntializeChatDefault(this.backgroudchatDefault, "");
        }
      }, {
        key: "doChat",
        value: function doChat(driverId) {
          this.sendbirdComponent.IntializeDriverChat(driverId, "");
        }
      }, {
        key: "mouseHoverMarker",
        value: function mouseHoverMarker(infoWindow, event) {
          if (this.previousInfowindow !== null && this.previousInfowindow.isOpen) {
            this.previousInfowindow.close();
          }

          if (infoWindow) {
            this.previousInfowindow = infoWindow;
            this.previousInfowindow.isOpen = true;
            infoWindow.open();
          }
        }
      }, {
        key: "mouseHoveOutMarker",
        value: function mouseHoveOutMarker(infoWindow, event) {
          var index = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : null;

          if (this.previousInfowindow !== null && this.previousInfowindow.isOpen && infoWindow !== null) {
            this.previousInfowindow.close();
            this.previousInfowindow.isOpen = false;
          }

          if (infoWindow) {
            infoWindow.close();
          }
        }
      }, {
        key: "showDriverDetails",
        value: function showDriverDetails(driver) {
          var _this69 = this;

          var infoWindow = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
          window.scrollTo(0, 0);
          this.driverModal = {
            modalDetails: {
              display: 'block',
              data: driver
            }
          };

          if (infoWindow && infoWindow.isOpen) {
            infoWindow.close();
          }

          this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"]();
          this.modalData = true;
          this.dispatcherService.getDriverAdditionalDetails(driver.Id).subscribe(function (data) {
            if (data) {
              _this69.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"](data);

              if (_this69.selectedDriverDetails.Trailers.length > 0) {
                _this69.selectedDriverDetails.Trailers.forEach(function (t) {
                  t.OngoingData = _this69.currentOngoingLoadDetails.filter(function (res) {
                    return res.TrailerDisplayId.split(',').indexOf(t.TruckId);
                  });
                });
              }

              _this69.modalData = false;
            } else {
              _this69.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"]();

              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please try again later.', 'Something Went Wrong', 3000);

              _this69.modalData = false;
            }
          });
        }
      }, {
        key: "modalClose",
        value: function modalClose() {
          this.driverModal = {
            modalDetails: {
              display: 'none',
              data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverModel"]()
            }
          };
        } //private restoreFilterStates(): void {
        //    let _wbfRegionId = MyLocalStorage.getData(MyLocalStorage.WBF_REGION_KEY);
        //    if (_wbfRegionId != null && _wbfRegionId != "") {
        //        // this.SelectedRegionId = _wbfRegionId;
        //        this.SelectedRegions = _wbfRegionId;
        //        var ids = [];
        //        this.SelectedRegionId = '';
        //        this.SelectedRegions.forEach(res => { ids.push(res.Id) });
        //        this.SelectedRegionId = ids.join();
        //        this.getCustomerListByRegionId(this.SelectedRegionId);
        //        let _wbfCustomerName = MyLocalStorage.getData(MyLocalStorage.WBF_CUSTOMER_KEY);
        //        _wbfCustomerName ? this.SelectedCustomerId = _wbfCustomerName : '';
        //    } else {
        //        let _dsbRegionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
        //        if (_dsbRegionId != '') {
        //            //   this.SelectedRegionId = _dsbRegionId;
        //        }
        //    }
        //    let _searchKeyword = MyLocalStorage.getData(MyLocalStorage.WBF_SEARCHEDKEYWORD_KEY);
        //    if (_searchKeyword != '') {
        //        this.SearchedKeyword = _searchKeyword;
        //    }
        //    let _selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
        //    if (_selectedStates != '') {
        //        this.SelectedStates = _selectedStates;
        //    }
        //    let _selectedPriorities = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDPRIORY_KEY);
        //    if (_selectedPriorities != '') {
        //        this.SelectedPriorities = _selectedPriorities;
        //    }
        //    let _selectedDispacher = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDDISPACHER_KEY);
        //    if (_selectedDispacher != '') {
        //        this.SelectedDispachers = _selectedDispacher;
        //    }
        //    if (this.disableControl == true) {
        //        let _selectedDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        //        if (_selectedDate != '') {
        //            this.FromDate = _selectedDate;
        //            this.ToDate = _selectedDate;
        //        }
        //    }
        //    //let _fromDate = MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY);
        //    //if (_fromDate != '') {
        //    //    this.FromDate = _fromDate;
        //    //}
        //    //let _toDate = MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY);
        //    //if (_toDate != '') {
        //    //    this.ToDate = _toDate;
        //    //}
        //}

      }, {
        key: "closePreviousWindow",
        value: function closePreviousWindow(index) {
          if (this.previousInfowindowIndex != null && this.previousInfowindow != null) {
            this.OnGoingLoads[this.previousInfowindowIndex].routeShow = false;
            if (this.previousInfowindow && this.previousInfowindow.isOpen) this.previousInfowindow.close();
            this.setMapCenter();
          }
        }
      }, {
        key: "showHideRoutes",
        value: function showHideRoutes(index) {
          if (index == this.previousInfowindowIndex || this.previousInfowindowIndex == null) {
            this.OnGoingLoads[index].routeShow = !this.OnGoingLoads[index].routeShow;
            if (!this.OnGoingLoads[index].routeShow) this.setMapCenter();
          } else {
            this.closePreviousWindow(index);
          }

          this.previousInfowindowIndex = index;
        }
      }, {
        key: "readOnlyModeSelection",
        value: function readOnlyModeSelection() {
          var readonlyKey = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].DSB_READONLY_KEY);

          if (readonlyKey == '') {
            this.disableControl = false;
          } else {
            this.disableControl = readonlyKey;
          }

          if (this.disableControl === true) {
            this.FilterForm.get('ToggleMap').patchValue(false); //this.toogleDriver = true;
          }
        }
      }, {
        key: "loadDropTicketDetails",
        value: function loadDropTicketDetails(invoiceHeaderId) {
          _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].showSliderPanel();

          this.dispatcherService.GetDropTicketDetails(invoiceHeaderId).subscribe(function (data) {
            if (data != '') {
              $("#invoice").html('');
              $("#invoice").html(data);
            } else {
              $("#invoice").html('');

              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('No Drop ticket details found.', null, 3000);
            }

            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].appendHTMLSliderContent("#invoice");

            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].hideSliderLoader();
          });
        }
      }, {
        key: "filterMapByStatus",
        value: function filterMapByStatus(statusId) {
          this.selectedMaplable = statusId;
          this.getDispatcherLoads(statusId); //this.autoRefreshLoads(statusId);
        }
      }, {
        key: "getCustomerListByRegionId",
        value: function getCustomerListByRegionId(SelectedRegionId) {
          var _this70 = this;

          this.loadingData = true;
          this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(function (t2) {
            _this70.loadingData = false;
            _this70.customerList = t2;
          });
        }
      }, {
        key: "isArrayEqual",
        value: function isArrayEqual(value, other) {
          var type = Object.prototype.toString.call(value);
          if (type !== Object.prototype.toString.call(other)) return false;
          if (['[object Array]', '[object Object]'].indexOf(type) < 0) return false;
          var valueLen = type === '[object Array]' ? value.length : Object.keys(value).length;
          var otherLen = type === '[object Array]' ? other.length : Object.keys(other).length;
          if (valueLen !== otherLen) return false;

          var compare = function compare(item1, item2) {};

          var match;

          if (type === '[object Array]') {
            for (var i = 0; i < valueLen; i++) {
              compare(value[i], other[i]);
            }
          } else {
            for (var key in value) {
              if (value.hasOwnProperty(key)) {
                compare(value[key], other[key]);
              }
            }
          }

          return true;
        }
      }, {
        key: "applyLoadsFilters",
        value: function applyLoadsFilters(filterForm) {
          this.FilterForm = filterForm;
          this.filterDriverData();
        }
      }]);

      return WhereIsMyDriverMapViewComponent;
    }();

    WhereIsMyDriverMapViewComponent.ɵfac = function WhereIsMyDriverMapViewComponent_Factory(t) {
      return new (t || WhereIsMyDriverMapViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]));
    };

    WhereIsMyDriverMapViewComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: WhereIsMyDriverMapViewComponent,
      selectors: [["app-where-is-my-driver-map-view"]],
      viewQuery: function WhereIsMyDriverMapViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true, angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["SendbirdComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.sendbirdComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti",
        FilterForm: "FilterForm"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 93,
      vars: 58,
      consts: [[1, "row"], [1, "col-sm-5", "pa0"], [1, "col-sm-6"], [1, "form-control", 3, "change"], [3, "value"], [3, "value", "selected", 4, "ngFor", "ngForOf"], [1, "col-sm-2"], ["type", "text", "placeholder", "From Date", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "ngModel", "onDateChange", "ngModelChange"], ["type", "text", "placeholder", "To Date", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "ngModel", "onDateChange", "ngModelChange"], [1, "col-sm-3", "pl0", "text-right", 3, "clickOutside"], [1, "fs14", "mr10", "mt10", 3, "click"], [1, "fas", "fa-filter", "mr5"], [1, "hide_show_map", "fs14", "ml10", 3, "click"], [1, "fas", "fa-eye", "mr5"], ["class", "text-right mr15 mt15", 4, "ngIf"], [1, "row", "animated", "mt60"], [1, "", 3, "ngClass"], [1, "expand_map_btn"], [1, "", 3, "click"], [1, "fa", "fa-2x", 3, "ngClass"], ["id", "map-view", 1, "mb15"], ["id", "mapLegend", 2, "z-index", "1", "position", "absolute", "top", "-5px", "left", "10px", "font-size", "11px"], ["id", "status-legends", 1, "well", "pa0"], [1, "border-b", "pb5", "pt5", "pl5"], [1, "db", "pa5", 3, "ngClass", "click"], ["src", "src/assets/truck-11.svg", "data-statusid", "11"], ["src", "src/assets/truck-12.svg", "data-statusid", "12"], ["src", "src/assets/truck-1.svg", "data-statusid", "1"], ["src", "src/assets/truck-18.svg", "data-statusid", "18"], [2, "z-index", "1", "position", "absolute", "top", "0", "right", "65px", "font-size", "11px", "opacity", "0.9"], [1, "well", "pa5"], [3, "ngStyle", "zoom", "maxZoom", "minZoom", "fullscreenControl", "fullscreenControlOptions", "mapReady"], [4, "ngFor", "ngForOf"], [1, "pl0", 3, "ngClass"], ["class", "driver_btn", 4, "ngIf"], [1, "mt10"], [1, "pull-left", "mt6", "pb0", "dib"], [1, "inner-addon", "left-addon", "pull-left", "ml10"], [1, "glyphicon", "glyphicon-search"], ["name", "txtSearch", "placeholder", "Search Drivers", "type", "text", "autocomplete", "off", 1, "form-control", 3, "input"], [1, "driver-list", "dib", "full-width"], ["class", "driver-details dib full-width pa5", 4, "ngFor", "ngForOf"], ["type", "button", "id", "btnconfirm-memberInfo", "data-toggle", "modal", "data-target", "#confirm-memberInfo", "data-backdrop", "static", "data-keyboard", "false", 1, "hide-element"], ["id", "confirm-memberInfo", "tabindex", "-1", "role", "dialog", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "fs18", "f-bold", "mt0"], ["id", "member-datatable", 1, "table", "table-striped", "table-bordered", "table-hover"], [1, "text-right"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", "btn-lg"], [1, "chat-wrapper", 2, "z-index", "9999"], ["driverDetailsModal", ""], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["id", "invoice", 1, "hide-element"], [3, "value", "selected"], [1, "text-right", "mr15", "mt15"], [1, "pull-right"], [1, "locationfilter", "border", "mtm10", "bg-gray", "shadow-b", "z-index5", "pa10"], [1, "col-sm-12", "mb15"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange", "onSelect", "onDeSelect"], ["labelContent", "Label marker", "labelClass", "circle-badge", 3, "latitude", "longitude", "iconUrl", "label", "mouseOver", "markerClick", "mouseOut"], [3, "disableAutoPan"], ["infoWindow", ""], ["target", "_blank", 3, "href", "title"], [3, "title", "click"], [1, "fs21", "far", "fa-comment"], ["style", "font-size:11px;padding-top: 10px;", 4, "ngIf", "ngIfElse"], ["showRouteTemplate", ""], [3, "latitude", "longitude", "iconUrl", "mouseOver", "mouseOut"], [3, "disableAutoPan", "maxWidth"], ["infoWindow2", ""], [3, "origin", "destination", "visible", "renderOptions", 4, "ngIf"], [2, "font-size", "11px", "padding-top", "10px"], [3, "origin", "destination", "visible", "renderOptions"], [1, "driver_btn"], [1, "driver-details", "dib", "full-width", "pa5"], [1, "pull-left", "driver-initials", "radius-capsule", "mr10", "fs15", "color-white", "pr"], [3, "ngClass"], [1, "pull-left", 3, "ngClass", "ngStyle", "title", "click"], [1, "fs15"], [1, "fs12", "db", "opacity8"], [1, "pull-right", "mt10"], [1, "fs18", "far", "fa-comment"], ["title", "Show more details", 3, "click"], [1, "fs18", "fa", "fa-info-circle", "pl5"], ["title", "Last location is not available", 1, "pull-left"], ["id", "myModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-lg", "modal-dialog-centered"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "modal-header", "pb0", "pt0"], ["id", "assetDetailsModal", 1, "modal-title"], ["title", "Chat", 3, "click"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt10", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body", 2, "max-height", "80vh", "overflow-y", "scroll"], [1, "mb10"], [4, "ngIf", "ngIfElse"], ["target", "_blank", 3, "href"], ["driverShifts", ""], [1, "col-sm-12"], [1, "pt0", "mb5"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "schedule-loading-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border", "location_table"], ["id", "trailerDetails", 1, "table-responsive"], ["id", "table-selectedDriverTrailers", "data-gridname", "23", 1, "table", "table-bordered", "table-hover"], ["SelectedDriverTrailer", ""], ["data-key", "TruckId"], ["data-key", "LicencePlate"], ["data-key", "Compartment"], ["truckDetailsTable", ""], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [4, "ngIf"], [2, "width", "10px"], ["retainDetailsTable", ""], ["Trailer", ""], ["style", "width: 10px;", 4, "ngIf", "ngIfElse"], ["Campcapcity", ""], ["tcapcity", ""], ["capcity", ""]],
      template: function WhereIsMyDriverMapViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "select", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function WhereIsMyDriverMapViewComponent_Template_select_change_4_listener($event) {
            return ctx.customerChanged($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "option", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Select Customer");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, WhereIsMyDriverMapViewComponent_option_7_Template, 2, 3, "option", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function WhereIsMyDriverMapViewComponent_Template_input_onDateChange_9_listener($event) {
            return ctx.setFromDate($event);
          })("ngModelChange", function WhereIsMyDriverMapViewComponent_Template_input_ngModelChange_9_listener($event) {
            return ctx.FromDate = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function WhereIsMyDriverMapViewComponent_Template_input_onDateChange_11_listener($event) {
            return ctx.setToDate($event);
          })("ngModelChange", function WhereIsMyDriverMapViewComponent_Template_input_ngModelChange_11_listener($event) {
            return ctx.ToDate = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("clickOutside", function WhereIsMyDriverMapViewComponent_Template_div_clickOutside_12_listener() {
            return ctx.clickOutsideDropdown();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_13_listener() {
            return ctx.toggleFilterView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](14, "i", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, " Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "a", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_16_listener() {
            return ctx.toggleMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](17, "i", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, WhereIsMyDriverMapViewComponent_div_19_Template, 10, 12, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "a", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_23_listener() {
            return ctx.toggleExpandMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](24, "i", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "a", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_29_listener() {
            return ctx.filterMapByStatus(11);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](30, "img", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, " On the way to terminal ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "a", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_33_listener() {
            return ctx.filterMapByStatus(12);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](34, "img", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, " Arrived at terminal ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "a", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_37_listener() {
            return ctx.filterMapByStatus(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](38, "img", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](39, " On the way to location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "a", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_41_listener() {
            return ctx.filterMapByStatus(18);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](42, "img", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43, " Arrived at location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "Auto Refresh in: ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](49, "date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](50, " minutes");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "agm-map", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("mapReady", function WhereIsMyDriverMapViewComponent_Template_agm_map_mapReady_51_listener($event) {
            return ctx.mapReady($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](52, WhereIsMyDriverMapViewComponent_ng_container_52_Template, 40, 37, "ng-container", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](54, WhereIsMyDriverMapViewComponent_div_54_Template, 3, 4, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "h3", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](57, "Drivers");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](59, "i", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("input", function WhereIsMyDriverMapViewComponent_Template_input_input_60_listener($event) {
            return ctx.searchDrivers($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](62, WhereIsMyDriverMapViewComponent_div_62_Template, 14, 12, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](63, WhereIsMyDriverMapViewComponent_div_63_Template, 11, 3, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](64, "button", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "div", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "div", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "div", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "h2", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](70, "Group Member Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "table", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](72, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](74, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](75, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](76, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](77, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](78, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](79, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](80, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](81, "LastSeenAt");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](82, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](83, WhereIsMyDriverMapViewComponent_tr_83_Template, 9, 4, "tr", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](84, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "button", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](86, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](87, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](88, "app-sendbird");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](89, WhereIsMyDriverMapViewComponent_ng_template_89_Template, 49, 16, "ng-template", null, 52, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](91, WhereIsMyDriverMapViewComponent_ng_container_91_Template, 1, 0, "ng-container", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](92, "div", 54);
        }

        if (rf & 2) {
          var _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](90);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.customerList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx.MaxInputDate)("ngModel", ctx.FromDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx.MaxInputDate)("ngModel", ctx.ToDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.toogleMap == true ? "Hide Map" : "Show Map");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.toogleFilter);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction4"](35, _c17, ctx.FilterForm.get("ToggleMap").value, !ctx.FilterForm.get("ToggleMap").value, !ctx.FilterForm.get("ToggleExpandMapView").value, ctx.FilterForm.get("ToggleExpandMapView").value === true));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](40, _c12, !ctx.FilterForm.get("ToggleExpandMapView").value, ctx.FilterForm.get("ToggleExpandMapView").value));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](43, _c13, ctx.selectedMaplable == 11));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](45, _c13, ctx.selectedMaplable == 12));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](47, _c13, ctx.selectedMaplable == 1));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](49, _c13, ctx.selectedMaplable == 18));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](49, 31, ctx.autoRefreshTicks * 1000, "mm:ss", "UTC"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](51, _c18, ctx.FilterForm.get("singleMulti").value == 2 ? "80vh" : "60vh"))("zoom", ctx.zoomLevel)("maxZoom", 16)("minZoom", 2)("fullscreenControl", true)("fullscreenControlOptions", ctx.screenOptions);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.OnGoingLoads);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction4"](53, _c19, ctx.FilterForm.get("ToggleExpandMapView").value === false && ctx.FilterForm.get("ToggleMap").value === true, ctx.FilterForm.get("ToggleMap").value === false, ctx.FilterForm.get("ToggleDriverView").value === true && ctx.FilterForm.get("ToggleMap").value === false, ctx.FilterForm.get("ToggleExpandMapView").value === true && ctx.FilterForm.get("ToggleMap").value === true));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx.FilterForm.get("ToggleMap").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.Drivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.OfflineDrivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.memberInfo);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngTemplateOutlet", _r7)("ngTemplateOutletContext", ctx.driverModal);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgModel"], _directives_click_outside_directive__WEBPACK_IMPORTED_MODULE_16__["ClickOutsideDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgClass"], _agm_core__WEBPACK_IMPORTED_MODULE_17__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgStyle"], src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["SendbirdComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgTemplateOutlet"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__["MultiSelectComponent"], _agm_core__WEBPACK_IMPORTED_MODULE_17__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_17__["AgmInfoWindow"], agm_direction__WEBPACK_IMPORTED_MODULE_19__["ɵa"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_14__["DatePipe"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["DecimalPipe"]],
      styles: [".driver-details[_ngcontent-%COMP%]:nth-child(5n+1)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #f6af27;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+2)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #ab47bc;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+3)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #a5a5a5;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+4)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #dc4949;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+5)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #00897b;\r\n}\r\n\r\n\r\n\r\n.sticky-header-wmd[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    right: 0;\r\n    padding: 15px 20px;\r\n    top: 45px;\r\n    height: 65px;\r\n    \r\n    z-index: 10;\r\n    background: #fff;\r\n}\r\n\r\n.locationfilter[_ngcontent-%COMP%] {\r\n    width: 100%;\r\n    position: absolute;\r\n    right: 4px;\r\n    border-radius: 5px;\r\n    font-size: 14px;\r\n    z-index: 1010;\r\n}\r\n\r\n.sticky_header[_ngcontent-%COMP%] {\r\n    position: sticky;\r\n    top: 45px;\r\n    padding: 5px;\r\n    font-size: 20px;\r\n    z-index: 10;\r\n    background: #fff;\r\n    margin-bottom: 0px;\r\n    margin-top: -10px;\r\n    \r\n    border-radius: 2px;\r\n}\r\n\r\n.display_hide[_ngcontent-%COMP%] {\r\n    display: none;\r\n    transition: opacity 1s ease-out;\r\n    opacity: 0;\r\n}\r\n\r\n.expand_map_btn[_ngcontent-%COMP%] {\r\n    position: absolute;\r\n    top: 1px;\r\n    right: 15px;\r\n    background: #fff;\r\n    border-radius: 2px 2px 2px 2px;\r\n    padding: 3px;\r\n    box-shadow: -2px 2px 6px 1px #aaa;\r\n    z-index: 1;\r\n}\r\n\r\n.driver_btn[_ngcontent-%COMP%] {\r\n    position: absolute;\r\n    top: 15px;\r\n    left: -35px;\r\n    background: white;\r\n    border-radius: 2px;\r\n    border-top-left-radius: 5px;\r\n    border-bottom-left-radius: 5px;\r\n    padding: 5px;\r\n    box-shadow: -4px 0px 4px 0px #aaaaaa;\r\n}\r\n\r\n.absolute_driver[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    width: 25%;\r\n    top: 100px;\r\n    right: 0;\r\n    background: #fff;\r\n    z-index: 11;\r\n    padding: 10px;\r\n    box-shadow: 0 3px 15px 0 rgba(0,0,0,.1);\r\n    border-radius: 10px;\r\n}\r\n\r\n.hide_absolute_driver[_ngcontent-%COMP%] {\r\n    width: 0;\r\n    right: -20px;\r\n}\r\n\r\n.activeRoute[_ngcontent-%COMP%] {\r\n    font-weight: 600;\r\n    cursor: pointer;\r\n    background: #f5f5f5;\r\n}\r\n\r\n.live[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    border-radius: 50%;\r\n    background-color: green;\r\n    position: absolute;\r\n    top: -1px;\r\n    right: 1px;\r\n    transform: scale(1);\r\n    -webkit-animation: pulse 1s infinite;\r\n            animation: pulse 1s infinite;\r\n}\r\n\r\n.inactive[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    border-radius: 50%;\r\n    background-color: orange;\r\n    position: absolute;\r\n    top: -1px;\r\n    right: 1px;\r\n}\r\n\r\n@-webkit-keyframes pulse {\r\n    0% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0.4);\r\n    }\r\n\r\n    70% {\r\n        box-shadow: 0 0 0 10px rgba(204,169,44, 0);\r\n    }\r\n\r\n    100% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0);\r\n    }\r\n}\r\n\r\n@keyframes pulse {\r\n    0% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0.4);\r\n    }\r\n\r\n    70% {\r\n        box-shadow: 0 0 0 10px rgba(204,169,44, 0);\r\n    }\r\n\r\n    100% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0);\r\n    }\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC93aGVyZS1pcy1teS1kcml2ZXItbWFwLXZpZXcuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLG1CQUFtQjtBQUN2Qjs7QUFFQTtJQUNJLG1CQUFtQjtBQUN2Qjs7QUFFQTtJQUNJLG1CQUFtQjtBQUN2Qjs7QUFFQTtJQUNJLG1CQUFtQjtBQUN2Qjs7QUFFQTtJQUNJLG1CQUFtQjtBQUN2Qjs7QUFFQTs7O0NBR0M7O0FBQ0Q7SUFDSSxlQUFlO0lBQ2YsUUFBUTtJQUNSLGtCQUFrQjtJQUNsQixTQUFTO0lBQ1QsWUFBWTtJQUNaLG1CQUFtQjtJQUNuQixXQUFXO0lBQ1gsZ0JBQWdCO0FBQ3BCOztBQUdBO0lBQ0ksV0FBVztJQUNYLGtCQUFrQjtJQUNsQixVQUFVO0lBQ1Ysa0JBQWtCO0lBQ2xCLGVBQWU7SUFDZixhQUFhO0FBQ2pCOztBQUVBO0lBRUksZ0JBQWdCO0lBQ2hCLFNBQVM7SUFDVCxZQUFZO0lBQ1osZUFBZTtJQUNmLFdBQVc7SUFDWCxnQkFBZ0I7SUFDaEIsa0JBQWtCO0lBQ2xCLGlCQUFpQjtJQUNqQiwyQ0FBMkM7SUFDM0Msa0JBQWtCO0FBQ3RCOztBQUVBO0lBQ0ksYUFBYTtJQUNiLCtCQUErQjtJQUMvQixVQUFVO0FBQ2Q7O0FBRUE7SUFDSSxrQkFBa0I7SUFDbEIsUUFBUTtJQUNSLFdBQVc7SUFDWCxnQkFBZ0I7SUFDaEIsOEJBQThCO0lBQzlCLFlBQVk7SUFDWixpQ0FBaUM7SUFDakMsVUFBVTtBQUNkOztBQUdBO0lBQ0ksa0JBQWtCO0lBQ2xCLFNBQVM7SUFDVCxXQUFXO0lBQ1gsaUJBQWlCO0lBQ2pCLGtCQUFrQjtJQUNsQiwyQkFBMkI7SUFDM0IsOEJBQThCO0lBQzlCLFlBQVk7SUFDWixvQ0FBb0M7QUFDeEM7O0FBRUE7SUFDSSxlQUFlO0lBQ2YsVUFBVTtJQUNWLFVBQVU7SUFDVixRQUFRO0lBQ1IsZ0JBQWdCO0lBQ2hCLFdBQVc7SUFDWCxhQUFhO0lBQ2IsdUNBQXVDO0lBQ3ZDLG1CQUFtQjtBQUN2Qjs7QUFFQTtJQUNJLFFBQVE7SUFDUixZQUFZO0FBQ2hCOztBQUVBO0lBQ0ksZ0JBQWdCO0lBQ2hCLGVBQWU7SUFDZixtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLGtCQUFrQjtJQUNsQix1QkFBdUI7SUFDdkIsa0JBQWtCO0lBQ2xCLFNBQVM7SUFDVCxVQUFVO0lBQ1YsbUJBQW1CO0lBQ25CLG9DQUE0QjtZQUE1Qiw0QkFBNEI7QUFDaEM7O0FBRUE7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLGtCQUFrQjtJQUNsQix3QkFBd0I7SUFDeEIsa0JBQWtCO0lBQ2xCLFNBQVM7SUFDVCxVQUFVO0FBQ2Q7O0FBRUE7SUFDSTtRQUVJLHlDQUF5QztJQUM3Qzs7SUFFQTtRQUVJLDBDQUEwQztJQUM5Qzs7SUFFQTtRQUVJLHVDQUF1QztJQUMzQztBQUNKOztBQWZBO0lBQ0k7UUFFSSx5Q0FBeUM7SUFDN0M7O0lBRUE7UUFFSSwwQ0FBMEM7SUFDOUM7O0lBRUE7UUFFSSx1Q0FBdUM7SUFDM0M7QUFDSiIsImZpbGUiOiJzcmMvYXBwL2Rpc3BhdGNoZXIvZGlzcGF0Y2hlci1kYXNoYm9hcmQvd2hlcmUtaXMtbXktZHJpdmVyLW1hcC12aWV3LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzEpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogI2Y2YWYyNztcclxufVxyXG5cclxuLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1bisyKSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICNhYjQ3YmM7XHJcbn1cclxuXHJcbi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rMykgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjYTVhNWE1O1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzQpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogI2RjNDk0OTtcclxufVxyXG5cclxuLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1bis1KSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICMwMDg5N2I7XHJcbn1cclxuXHJcbi8qLnRhYmxlLmRhdGFUYWJsZSB7XHJcbiAgICBtYXJnaW4tdG9wOiAwICFpbXBvcnRhbnQ7XHJcbn1cclxuKi9cclxuLnN0aWNreS1oZWFkZXItd21kIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgcGFkZGluZzogMTVweCAyMHB4O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgLypmb250LXNpemU6IDIwcHg7Ki9cclxuICAgIHotaW5kZXg6IDEwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxufVxyXG5cclxuXHJcbi5sb2NhdGlvbmZpbHRlciB7XHJcbiAgICB3aWR0aDogMTAwJTtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHJpZ2h0OiA0cHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICBmb250LXNpemU6IDE0cHg7XHJcbiAgICB6LWluZGV4OiAxMDEwO1xyXG59XHJcblxyXG4uc3RpY2t5X2hlYWRlciB7XHJcbiAgICBwb3NpdGlvbjogLXdlYmtpdC1zdGlja3k7XHJcbiAgICBwb3NpdGlvbjogc3RpY2t5O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgcGFkZGluZzogNXB4O1xyXG4gICAgZm9udC1zaXplOiAyMHB4O1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG4gICAgbWFyZ2luLWJvdHRvbTogMHB4O1xyXG4gICAgbWFyZ2luLXRvcDogLTEwcHg7XHJcbiAgICAvKmJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTsqL1xyXG4gICAgYm9yZGVyLXJhZGl1czogMnB4O1xyXG59XHJcblxyXG4uZGlzcGxheV9oaWRlIHtcclxuICAgIGRpc3BsYXk6IG5vbmU7XHJcbiAgICB0cmFuc2l0aW9uOiBvcGFjaXR5IDFzIGVhc2Utb3V0O1xyXG4gICAgb3BhY2l0eTogMDtcclxufVxyXG5cclxuLmV4cGFuZF9tYXBfYnRuIHtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogMXB4O1xyXG4gICAgcmlnaHQ6IDE1cHg7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMnB4IDJweCAycHggMnB4O1xyXG4gICAgcGFkZGluZzogM3B4O1xyXG4gICAgYm94LXNoYWRvdzogLTJweCAycHggNnB4IDFweCAjYWFhO1xyXG4gICAgei1pbmRleDogMTtcclxufVxyXG5cclxuXHJcbi5kcml2ZXJfYnRuIHtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogMTVweDtcclxuICAgIGxlZnQ6IC0zNXB4O1xyXG4gICAgYmFja2dyb3VuZDogd2hpdGU7XHJcbiAgICBib3JkZXItcmFkaXVzOiAycHg7XHJcbiAgICBib3JkZXItdG9wLWxlZnQtcmFkaXVzOiA1cHg7XHJcbiAgICBib3JkZXItYm90dG9tLWxlZnQtcmFkaXVzOiA1cHg7XHJcbiAgICBwYWRkaW5nOiA1cHg7XHJcbiAgICBib3gtc2hhZG93OiAtNHB4IDBweCA0cHggMHB4ICNhYWFhYWE7XHJcbn1cclxuXHJcbi5hYnNvbHV0ZV9kcml2ZXIge1xyXG4gICAgcG9zaXRpb246IGZpeGVkO1xyXG4gICAgd2lkdGg6IDI1JTtcclxuICAgIHRvcDogMTAwcHg7XHJcbiAgICByaWdodDogMDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICB6LWluZGV4OiAxMTtcclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG59XHJcblxyXG4uaGlkZV9hYnNvbHV0ZV9kcml2ZXIge1xyXG4gICAgd2lkdGg6IDA7XHJcbiAgICByaWdodDogLTIwcHg7XHJcbn1cclxuXHJcbi5hY3RpdmVSb3V0ZSB7XHJcbiAgICBmb250LXdlaWdodDogNjAwO1xyXG4gICAgY3Vyc29yOiBwb2ludGVyO1xyXG4gICAgYmFja2dyb3VuZDogI2Y1ZjVmNTtcclxufVxyXG5cclxuLmxpdmUge1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiBncmVlbjtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogLTFweDtcclxuICAgIHJpZ2h0OiAxcHg7XHJcbiAgICB0cmFuc2Zvcm06IHNjYWxlKDEpO1xyXG4gICAgYW5pbWF0aW9uOiBwdWxzZSAxcyBpbmZpbml0ZTtcclxufVxyXG5cclxuLmluYWN0aXZlIHtcclxuICAgIGhlaWdodDogMTBweDtcclxuICAgIHdpZHRoOiAxMHB4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogb3JhbmdlO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAtMXB4O1xyXG4gICAgcmlnaHQ6IDFweDtcclxufVxyXG5cclxuQGtleWZyYW1lcyBwdWxzZSB7XHJcbiAgICAwJSB7XHJcbiAgICAgICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LDE2OSw0NCwgMC40KTtcclxuICAgICAgICBib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LDE2OSw0NCwgMC40KTtcclxuICAgIH1cclxuXHJcbiAgICA3MCUge1xyXG4gICAgICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMTBweCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDAgMCAwIDEwcHggcmdiYSgyMDQsMTY5LDQ0LCAwKTtcclxuICAgIH1cclxuXHJcbiAgICAxMDAlIHtcclxuICAgICAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwKTtcclxuICAgICAgICBib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LDE2OSw0NCwgMCk7XHJcbiAgICB9XHJcbn1cclxuIl19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](WhereIsMyDriverMapViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-where-is-my-driver-map-view',
          templateUrl: './where-is-my-driver-map-view.component.html',
          styleUrls: ['./where-is-my-driver-map-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_10__["DispatcherService"]
        }, {
          type: src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]
        }];
      }, {
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        FilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }],
        selectedDriverLoad: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['SelectedDriverLoad', {
            read: angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"],
            "static": false
          }]
        }],
        sendbirdComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["SendbirdComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver.component.ts": function srcAppDispatcherDispatcherDashboardWhereIsMyDriverComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverComponent", function () {
      return WhereIsMyDriverComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
    /* harmony import */


    var src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/sendbird.component */
    "./src/app/shared-components/sendbird/sendbird.component.ts");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var _where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./where-is-my-driver-map-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-map-view.component.ts");
    /* harmony import */


    var _where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./where-is-my-driver-grid-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-grid-view.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! src/app/app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/services/sendbird.service */
    "./src/app/shared-components/sendbird/services/sendbird.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    var _c0 = ["myDiv"];
    var _c1 = ["SelectedDriverLoad"];

    function WhereIsMyDriverComponent_div_9_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r9.FilterForm.controls["SelectedCarriers"])("settings", ctx_r9.CarrierDdlSettings)("placeholder", "Select Carrier")("data", ctx_r9.carrierList);
      }
    }

    function WhereIsMyDriverComponent_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, WhereIsMyDriverComponent_div_9_ng_template_3_Template, 2, 4, "ng-template", null, 18, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r8)("autoClose", "outside");
      }
    }

    function WhereIsMyDriverComponent_span_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r3.count);
      }
    }

    function WhereIsMyDriverComponent_ng_template_24_option_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "option", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var customer_r11 = ctx.$implicit;

        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", customer_r11.CompanyId)("selected", ctx_r10.SelectedCustomerId == customer_r11.CompanyId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", customer_r11.CompanyName, " ");
      }
    }

    function WhereIsMyDriverComponent_ng_template_24_Template(rf, ctx) {
      if (rf & 1) {
        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "States");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "input", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function WhereIsMyDriverComponent_ng_template_24_Template_input_onDateChange_17_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r13);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r12.setFromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "To");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "input", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function WhereIsMyDriverComponent_ng_template_24_Template_input_onDateChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r13);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r14.setToDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](28, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Dispatcher");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](33, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Inventory Data Capture");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](39, "ng-multiselect-dropdown", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43, "Select Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "select", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "option", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "Select Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](47, WhereIsMyDriverComponent_ng_template_24_option_47_Template, 2, 3, "option", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "button", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverComponent_ng_template_24_Template_button_click_50_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r13);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.ResetLoadFilters();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](51, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "button", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverComponent_ng_template_24_Template_button_click_52_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r13);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          var _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](17);

          ctx_r16.ApplyLoadFilters("set");
          return _r2.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](53, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["SelectedRegions"])("settings", ctx_r5.RegionDdlSettings)("placeholder", "Select Region")("data", ctx_r5.Regions);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["SelectedStates"])("settings", ctx_r5.StateDdlSettings)("placeholder", "Select States")("data", ctx_r5.RegionStates);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx_r5.MaxInputDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx_r5.MaxInputDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["SelectedPriorities"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Select Priority")("data", ctx_r5.LoadPriorities);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["SelectedDispachers"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Select Dispatcher")("data", ctx_r5.RegionDispachers);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["selectedLocAttributeList"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Inventory Data Capture")("data", ctx_r5.LocationAttributeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formControl", ctx_r5.FilterForm.controls["SelectedCustomerId"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r5.customerList);
      }
    }

    function WhereIsMyDriverComponent_app_where_is_my_driver_map_view_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "app-where-is-my-driver-map-view", 43);
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("singleMulti", ctx_r6.singleMulti)("FilterForm", ctx_r6.FilterForm);
      }
    }

    function WhereIsMyDriverComponent_app_where_is_my_driver_grid_view_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "app-where-is-my-driver-grid-view", 44);
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("singleMulti", ctx_r7.singleMulti)("FilterForm", ctx_r7.FilterForm)("IsFiltersLoaded", ctx_r7.IsFiltersLoaded);
      }
    }

    var WhereIsMyDriverComponent = /*#__PURE__*/function () {
      function WhereIsMyDriverComponent(fb, dispatcherService, chatService, carrierService, cdr) {
        _classCallCheck(this, WhereIsMyDriverComponent);

        this.fb = fb;
        this.dispatcherService = dispatcherService;
        this.chatService = chatService;
        this.carrierService = carrierService;
        this.cdr = cdr;
        this.previousInfowindow = null;
        this.previousInfowindowIndex = null;
        this.zoomLevel = 4;
        this.centerLoactionLat = 39.1175;
        this.centerLoactionLng = -103.8784;
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_4__().add(1, 'year').toDate();
        this.TodaysDate = moment__WEBPACK_IMPORTED_MODULE_4__().format('MM/DD/YYYY');
        this.IsFiltersLoaded = false;
        this.AUTO_REFRESH_TIME = 300; // seconds

        this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
        this.driverModal = {
          modalDetails: {
            display: 'none',
            data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverModel"]()
          }
        };
        this.screenOptions = {
          position: 6
        };
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subscription"]();
        this.Drivers = [];
        this.OfflineDrivers = [];
        this.allLoads = [];
        this.OnGoingLoads = [];
        this.CloneOnGoingLoads = [];
        this.MustGoSchedules = [];
        this.ShouldGoSchedules = [];
        this.CouldGoSchedules = [];
        this.selectedDriverLoads = [];
        this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"]();
        this.Regions = [];
        this.LocationAttributeList = src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["InventoryDataCaptureList"];
        this.selectedLocAttributeList = [];
        this.RegionStates = [];
        this.RegionDispachers = [];
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.RegionDdlSettings = {};
        this.CarrierDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleFilter = false;
        this.toogleGrid = false;
        this.customerList = [];
        this.dtMustGoOptions = {};
        this.dtShouldGoOptions = {};
        this.dtCouldGoOptions = {};
        this.selectedDriverLoadsdtOptions = {};
        this.selectedDriverLoadsdtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtMustGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtShouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtCouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.loadingData = true;
        this.modalData = true;
        this.backgroudchatDefault = [];
        this.memberInfo = [];
        this.disableControl = false;
        this.viewLoadType = 1;
        this.loadScreenType = 'All';
        this.isShowCarrierManaged = false;
        this.carrierList = [];
        this.SelectedCarriers = [];
        this.count = 0;
        this.singleMulti = localStorage.getItem('singleMulti') ? +localStorage.getItem('singleMulti') : 1;
        if (this.singleMulti == 1) this.loadScreenType = 'All';

        var _this = this;

        window.addEventListener("beforeunload", function (e) {
          _this.SaveFilters(true);

          return;
        });
      }

      _createClass(WhereIsMyDriverComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this71 = this;

          this.setFilterForm();
          this.readOnlyModeSelection();
          this.StateDdlSettings = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.PriorityDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.RegionDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: true
          };
          this.CarrierDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.subscribeFormChanges();
          this.restoreFilterStates();
          this.dispatcherService.GetDispatcherRegions().subscribe(function (data) {
            _this71.Regions = data;

            _this71.GetFilters();
          });
          this.getCarriers();
        }
      }, {
        key: "setFilterForm",
        value: function setFilterForm() {
          var toDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY) : this.TodaysDate;
          var fromDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY) : this.TodaysDate;
          this.FilterForm = this.fb.group({
            IsShowCarrierManaged: this.fb.control(false),
            ToggleMap: this.fb.control(true),
            ToggleExpandMapView: this.fb.control(false),
            ToggleGrids: this.fb.control(false),
            ToggleDriverView: this.fb.control(false),
            SelectedCarriers: this.fb.control([]),
            SelectedRegions: this.fb.control([]),
            SelectedStates: this.fb.control([]),
            SelectedPriorities: this.fb.control([]),
            SelectedDispachers: this.fb.control([]),
            SelectedCustomerId: this.fb.control(null),
            singleMulti: this.fb.control(this.singleMulti),
            FromDate: this.fb.control(fromDate),
            ToDate: this.fb.control(toDate),
            selectedLocAttributeList: this.fb.control([])
          });
        }
      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this72 = this;

          this.subscriptions.add(this.FilterForm.get('SelectedRegions').valueChanges.subscribe(function (change) {
            if (change) {
              var ids = [];
              var SelectedRegions = change;
              SelectedRegions.forEach(function (res) {
                ids.push(res.Id);
              });
              var selectedRegionId = ids.join();

              if (_this72.SelectedRegionId != selectedRegionId) {
                _this72.SelectedRegionId = selectedRegionId;

                _this72.regionChanged();
              }
            }
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(function (change) {
            _this72.FilterForm.get('SelectedRegions').patchValue([]);
          }));
          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            _this72.FilterForm.get('SelectedCarriers').patchValue([]);

            _this72.FilterForm.get('SelectedRegions').patchValue([]);

            _this72.ApplyLoadFilters();
          }));
        }
      }, {
        key: "unSubscribeFormChanges",
        value: function unSubscribeFormChanges() {
          if (this.subscriptions) {
            this.subscriptions.unsubscribe();
          }
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this73 = this;

          this.dispatcherService.GetCarriersForSupplier().subscribe(function (data) {
            _this73.carrierList = data;
          });
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          var filterChange = localStorage.getItem("filterChange") ? localStorage.getItem("filterChange") : 0;

          if (change.singleMulti && change.singleMulti.currentValue) {
            this.viewLoadType = localStorage.getItem('viewLoadType') ? +localStorage.getItem('viewLoadType') : 1;

            if (this.singleMulti == 1) {
              this.viewLoadType = 1;
              localStorage.setItem('viewLoadType', this.viewLoadType.toString());
              this.loadScreenType = "All";
              sessionStorage.setItem('loadScreenType', this.loadScreenType);
            } else if (this.singleMulti == 2 && this.viewLoadType == 0 && filterChange == 0) {
              this.viewLoadType = 2;
              localStorage.setItem('viewLoadType', this.viewLoadType.toString());
              this.loadScreenType == "Grid" ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";
              sessionStorage.setItem('loadScreenType', this.loadScreenType);
            } else if (this.viewLoadType == 2 && this.singleMulti == 2 && filterChange == 0) {
              this.loadScreenType = sessionStorage.getItem('loadScreenType');
              sessionStorage.setItem('loadScreenType', this.loadScreenType);
              this.viewLoadType = 0;
              localStorage.setItem('viewLoadType', '0');
            } else if (this.singleMulti == 2 && this.viewLoadType == 1 && filterChange == 0) {
              this.viewLoadType == 1 ? this.loadScreenType = "Map" : '';
              sessionStorage.setItem('loadScreenType', this.loadScreenType);
              this.viewLoadType = 2;
              localStorage.setItem('viewLoadType', this.viewLoadType.toString());
              this.viewLoadType = 0;
              localStorage.setItem('viewLoadType', '0');
            } else if (filterChange == 1 && this.singleMulti == 2) {
              sessionStorage.getItem('loadScreenType') ? this.loadScreenType = sessionStorage.getItem('loadScreenType') : 'All';
            }

            if (this.loadScreenType == null && this.singleMulti == 2) {
              this.loadScreenType = 'Map';
            }
          }

          filterChange = 0;
          localStorage.setItem('filterChange', filterChange.toString());
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {}
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.unSubscribeFormChanges();
          this.SaveFilters(true);
        }
      }, {
        key: "regionChanged",
        value: function regionChanged(event) {
          this.setRegionStates();
          this.setRegionDispachers();

          if (this.SelectedRegionId != "") {
            this.getCustomerListByRegionId(this.SelectedRegionId);
          }
        }
      }, {
        key: "customerChanged",
        value: function customerChanged(event) {
          this.SelectedCustomerId = event.target.value == "null" || event.target.value == null ? null : event.target.value; //MyLocalStorage.setData(MyLocalStorage.WBF_CUSTOMER_KEY, this.SelectedCustomerId);
        }
      }, {
        key: "setRegionStates",
        value: function setRegionStates() {
          var _this74 = this;

          this.RegionStates = [];
          this.Regions.map(function (m) {
            if (_this74.FilterForm.get('SelectedRegions').value.find(function (f) {
              return f.Id == m.Id;
            })) {
              if (m && m.States && m.States.length > 0) {
                _this74.RegionStates = _this74.RegionStates.concat(m.States);
              }
            }
          });
          var selectedStates = this.FilterForm.get('SelectedStates').value || [];
          selectedStates = selectedStates.filter(function (t) {
            return _this74.RegionStates.some(function (el) {
              return el.Code == t.Code;
            });
          });
          this.FilterForm.get('SelectedStates').patchValue(selectedStates);
        }
      }, {
        key: "setRegionDispachers",
        value: function setRegionDispachers() {
          var _this75 = this;

          this.RegionDispachers = [];
          this.Regions.map(function (m) {
            if (_this75.FilterForm.get('SelectedRegions').value.find(function (f) {
              return f.Id == m.Id;
            })) {
              if (m && m.Dispatchers && m.Dispatchers.length > 0) {
                _this75.RegionDispachers = _this75.RegionDispachers.concat(m.Dispatchers);
              }
            }
          });
          this.RegionDispachers = this.GetUniqueItems(this.RegionDispachers.reduce(function (p, n) {
            return p.concat(n);
          }, []));
          var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
          selectedDispachers = selectedDispachers.filter(function (t) {
            return _this75.RegionDispachers.some(function (el) {
              return el.Id == t.Id;
            });
          });
          this.FilterForm.get('SelectedDispachers').patchValue(selectedDispachers);
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          if (event != '') {
            this.FilterForm.get('FromDate').patchValue(event);
          }

          var toDate = this.FilterForm.get('ToDate').value;
          var fromDate = this.FilterForm.get('FromDate').value;

          if (fromDate != '' && toDate != '' && src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["RegExConstants"].DateFormat.test(fromDate) && src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["RegExConstants"].DateFormat.test(toDate)) {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_4__(fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_4__(toDate).toDate();

            if (_toDate < _fromDate) {
              this.FilterForm.get('ToDate').patchValue(event);
            }

            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
          }
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          if (event != '') {
            this.FilterForm.get('ToDate').patchValue(event);
          }

          var toDate = this.FilterForm.get('ToDate').value;
          var fromDate = this.FilterForm.get('FromDate').value;

          if (fromDate != '' && toDate != '' && src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["RegExConstants"].DateFormat.test(fromDate) && src_app_app_constants__WEBPACK_IMPORTED_MODULE_12__["RegExConstants"].DateFormat.test(toDate)) {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_4__(fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_4__(toDate).toDate();

            if (_fromDate > _toDate) {
              this.FilterForm.get('FromDate').patchValue(event);
            }

            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
          }
        }
      }, {
        key: "toggleExpandMapView",
        value: function toggleExpandMapView() {
          var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
          this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
        }
      }, {
        key: "toggleMapView",
        value: function toggleMapView() {
          var expandMapView = this.FilterForm.get('ToggleMap').value;
          this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
        }
      }, {
        key: "toggleGrids",
        value: function toggleGrids() {
          this.toogleGrid = !this.toogleGrid;
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "toggleDriverView",
        value: function toggleDriverView() {
          var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
          this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
        }
      }, {
        key: "modalClose",
        value: function modalClose() {
          this.driverModal = {
            modalDetails: {
              display: 'none',
              data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverModel"]()
            }
          };
        }
      }, {
        key: "restoreFilterStates",
        value: function restoreFilterStates() {
          if (this.disableControl == true) {
            var _selectedDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].DSB_DATE_KEY);

            if (_selectedDate != '') {
              this.FilterForm.get('FromDate').patchValue(_selectedDate);
              this.FilterForm.get('ToDate').patchValue(_selectedDate);
            }
          }
        }
      }, {
        key: "readOnlyModeSelection",
        value: function readOnlyModeSelection() {
          var readonlyKey = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].DSB_READONLY_KEY);

          if (readonlyKey == '') {
            this.disableControl = false;
          } else {
            this.disableControl = readonlyKey;
          }

          if (this.disableControl === true) {
            this.FilterForm.get('ToggleMap').patchValue(false);
          }
        }
      }, {
        key: "loadDropTicketDetails",
        value: function loadDropTicketDetails(invoiceHeaderId) {
          _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].showSliderPanel();

          this.dispatcherService.GetDropTicketDetails(invoiceHeaderId).subscribe(function (data) {
            if (data != '') {
              $("#invoice").html('');
              $("#invoice").html(data);
            } else {
              $("#invoice").html('');

              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('No Drop ticket details found.', null, 3000);
            }

            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].appendHTMLSliderContent("#invoice");

            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].hideSliderLoader();
          });
        }
      }, {
        key: "getCustomerListByRegionId",
        value: function getCustomerListByRegionId(SelectedRegionId) {
          var _this76 = this;

          this.loadingData = true;
          this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(function (t2) {
            _this76.loadingData = false;
            _this76.customerList = t2;

            var selectedCustomerId = _this76.FilterForm.get('SelectedCustomerId').value;

            _this76.SelectedCustomerId = _this76.customerList.filter(function (t) {
              return t.CompanyId == selectedCustomerId;
            }).length > 0 ? selectedCustomerId : null;

            if (_this76.SelectedCustomerId != selectedCustomerId) {
              _this76.FilterForm.get('SelectedCustomerId').patchValue(_this76.SelectedCustomerId);
            }
          });
        }
      }, {
        key: "SaveFilters",
        value: function SaveFilters(isTopFilter) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
            var _this77 = this;

            var filterModel;
            return regeneratorRuntime.wrap(function _callee6$(_context6) {
              while (1) {
                switch (_context6.prev = _context6.next) {
                  case 0:
                    if (isTopFilter) {
                      this.dispatcherService.getFilters(src_app_app_enum__WEBPACK_IMPORTED_MODULE_11__["TfxModule"].SupplierWallyboardLoads).subscribe(function (res) {
                        if (res && Object.keys(res).length > 0) {
                          var IsShowCarrierManaged = _this77.FilterForm.get("IsShowCarrierManaged").value;

                          var SelectedCarriers = _this77.FilterForm.get("SelectedCarriers").value || [];
                          var jsonFilterForm = null;
                          jsonFilterForm = JSON.parse(res);
                          jsonFilterForm["IsShowCarrierManaged"] = IsShowCarrierManaged;
                          jsonFilterForm["SelectedCarriers"] = SelectedCarriers;
                          var filterModel = JSON.stringify(jsonFilterForm);

                          _this77.dispatcherService.postFiltersData(src_app_app_enum__WEBPACK_IMPORTED_MODULE_11__["TfxModule"].SupplierWallyboardLoads, filterModel).subscribe();
                        }
                      });
                    } else {
                      filterModel = JSON.stringify(this.FilterForm.value);
                      this.dispatcherService.postFiltersData(src_app_app_enum__WEBPACK_IMPORTED_MODULE_11__["TfxModule"].SupplierWallyboardLoads, filterModel).subscribe();
                    }

                  case 1:
                  case "end":
                    return _context6.stop();
                }
              }
            }, _callee6, this);
          }));
        }
      }, {
        key: "getFilterData",
        value: function getFilterData() {
          var _this78 = this;

          this.dispatcherService.getFilters(this.FilterForm.get('IsShowCarrierManaged').value).subscribe(function (data) {
            _this78.Regions = data;
            _this78.RegionStates = _this78.GetUniqueItems(data.map(function (t) {
              return t.States;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
            _this78.LoadPriorities = _this78.GetUniqueItems(data.map(function (t) {
              return t.Priority;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
            _this78.RegionDispachers = _this78.GetUniqueItems(data.map(function (t) {
              return t.Dispachers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
          });
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this79 = this;

          this.dispatcherService.getFilters(src_app_app_enum__WEBPACK_IMPORTED_MODULE_11__["TfxModule"].SupplierWallyboardLoads).subscribe(function (res) {
            if (res && Object.keys(res).length > 0) {
              _this79.SetFilters(res);
            } else {
              if (_this79.Regions && _this79.Regions.length > 0) {
                var lstRegion = [_this79.Regions[0]];

                _this79.FilterForm.get('SelectedRegions').patchValue(lstRegion);
              }

              _this79.setRegionStates();

              _this79.setRegionDispachers();

              _this79.IsFiltersLoaded = true;
            }
          });
        }
      }, {
        key: "GetUniqueItems",
        value: function GetUniqueItems(items) {
          var ids = [];
          var uniqueItems = items.filter(function (item) {
            return ids.some(function (t) {
              return t == item.Id;
            }) ? false : ids.push(item.Id);
          });
          return uniqueItems.sort(function (a, b) {
            return a.Name.localeCompare(b.Name);
          });
        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          if (input && Object.keys(input).length > 0) {
            // setTimeout(() => {
            var jsonFilterForm = JSON.parse(input);
            delete jsonFilterForm["FromDate"];
            delete jsonFilterForm["ToDate"];
            this.FilterForm.patchValue(jsonFilterForm);

            if (!this.FilterForm.get('SelectedRegions').value || !this.FilterForm.get('SelectedRegions').value.length) {
              if (this.Regions && this.Regions.length) {
                var lstRegion = [this.Regions[0]];
                this.FilterForm.get('SelectedRegions').patchValue(lstRegion);
              }
            }

            if (jsonFilterForm.SelectedCustomerId == "") {
              this.FilterForm.get('SelectedCustomerId').patchValue(null);
              this.SelectedCustomerId = this.FilterForm.get('SelectedCustomerId').value;
            }

            this.IsFiltersLoaded = true;
            this.cdr.detectChanges();
            var el = this.myDiv.nativeElement;
            el.click();
            this.setRegionStates();
            this.setRegionDispachers();
            var that = this;
            setTimeout(function () {
              that.ApplyLoadFilters();
            }, 1000); // }, 1500);
          }
        }
      }, {
        key: "ApplyLoadFilters",
        value: function ApplyLoadFilters(msg) {
          var _this80 = this;

          this.SaveFilters(false);
          this.count = 0;

          if (this.FilterForm) {
            var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
            selectedRegions.forEach(function (res) {
              _this80.count++;
            });
            var selectedStates = this.FilterForm.get('SelectedStates').value || [];
            selectedStates.forEach(function (res) {
              _this80.count++;
            });
            var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
            selectedPriorities.forEach(function (res) {
              _this80.count++;
            });
            var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
            selectedDispachers.forEach(function (res) {
              _this80.count++;
            });
            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];

            if (selectedLocAttributeList != null || selectedLocAttributeList != 'undefined') {
              selectedLocAttributeList.forEach(function (res) {
                _this80.count++;
              });
            } else {
              this.FilterForm.get('selectedLocAttributeList').patchValue(0);
            }
          }

          this.loadsGridView.applyLoadsFilters(this.FilterForm);
          this.loadsMapView.applyLoadsFilters(this.FilterForm);

          if (msg == "set") {
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess("Filter applied successfully", undefined, undefined);
          } else if (msg == "reset") {
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msginfo("Filter reset successfully", undefined, undefined);
          }
        }
      }, {
        key: "ResetLoadFilters",
        value: function ResetLoadFilters() {
          var toDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY) : this.TodaysDate;
          var fromDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY) : this.TodaysDate;
          this.FilterForm.get('SelectedRegions').patchValue([]);
          this.FilterForm.get('SelectedStates').patchValue([]);
          this.FilterForm.get('SelectedPriorities').patchValue([]);
          this.FilterForm.get('SelectedDispachers').patchValue([]);
          this.FilterForm.get('SelectedCustomerId').patchValue([]);
          this.FilterForm.get('FromDate').patchValue(fromDate);
          this.FilterForm.get('ToDate').patchValue(toDate);
          this.FilterForm.get('selectedLocAttributeList').patchValue([]);
          this.ApplyLoadFilters('reset');
        }
      }]);

      return WhereIsMyDriverComponent;
    }();

    WhereIsMyDriverComponent.ɵfac = function WhereIsMyDriverComponent_Factory(t) {
      return new (t || WhereIsMyDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_13__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_14__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_15__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_16__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]));
    };

    WhereIsMyDriverComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: WhereIsMyDriverComponent,
      selectors: [["app-where-is-my-driver"]],
      viewQuery: function WhereIsMyDriverComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c1, true, angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["SendbirdComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.myDiv = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.loadsMapView = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.loadsGridView = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.sendbirdComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 28,
      vars: 8,
      consts: [[1, "col-sm-9", "sticky-header-wmd", 3, "formGroup"], [1, "row"], [1, "col-sm-5", "pa0"], [1, "col-sm-7"], [1, "form-check", "form-check-inline", "fs14", "mt5"], ["type", "checkbox", "id", "inlineCarrierManaged", "formControlName", "IsShowCarrierManaged", 1, "form-check-input"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["class", "mtm5", 4, "ngIf"], [1, "col-sm-5"], ["myDiv", ""], [1, "col-sm-2"], [1, "col-sm-3", "pl0", "text-right", "pt8"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], [1, "hide_show_map", "fs14", "ml10", 3, "click"], [1, "fas", "fa-eye", "mr5"], ["popContent", ""], [3, "singleMulti", "FilterForm", 4, "ngIf"], [3, "singleMulti", "FilterForm", "IsFiltersLoaded", 4, "ngIf"], [1, "mtm5"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "p-0"], [3, "formControl", "settings", "placeholder", "data"], [1, "circle-badge"], [1, "popover-details"], [1, "row", "border-bottom-2"], [1, "col-6", "pr-0"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], [1, "col-6"], [1, "row", "border-bottom-2", "mt10"], ["type", "text", "placeholder", "From Date", "myDatePicker", "", "formControlName", "FromDate", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], ["type", "text", "placeholder", "To Date", "myDatePicker", "", "formControlName", "ToDate", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], [1, "form-control", 3, "formControl"], [3, "value"], [3, "value", "selected", 4, "ngFor", "ngForOf"], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"], [3, "value", "selected"], [3, "singleMulti", "FilterForm"], [3, "singleMulti", "FilterForm", "IsFiltersLoaded"]],
      template: function WhereIsMyDriverComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, WhereIsMyDriverComponent_div_9_Template, 5, 2, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "div", null, 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](14, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "a", 12, 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverComponent_Template_a_click_16_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r17);

            var _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](17);

            return _r2.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "i", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, WhereIsMyDriverComponent_span_19_Template, 2, 1, "span", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "a", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function WhereIsMyDriverComponent_Template_a_click_21_listener() {
            return ctx.toggleMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](22, "i", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, WhereIsMyDriverComponent_ng_template_24_Template, 54, 27, "ng-template", null, 18, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](26, WhereIsMyDriverComponent_app_where_is_my_driver_map_view_26_Template, 1, 2, "app-where-is-my-driver-map-view", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, WhereIsMyDriverComponent_app_where_is_my_driver_grid_view_27_Template, 1, 3, "app-where-is-my-driver-grid-view", 20);
        }

        if (rf & 2) {
          var _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.FilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.FilterForm.get("IsShowCarrierManaged").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r4)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.count > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", ctx.FilterForm.get("ToggleMap").value ? "Hide Map" : "Show Map", " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.loadScreenType == "Map" || ctx.loadScreenType == "All");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.loadScreenType == "Grid" || ctx.loadScreenType == "All");
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_17__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_18__["NgbPopover"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_19__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["FormControlDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_20__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_13__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_17__["NgForOf"], _where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"], _where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"]],
      styles: [".driver-details:nth-child(5n+1) .driver-initials {\n  background: #f6af27;\n}\n\n.driver-details:nth-child(5n+2) .driver-initials {\n  background: #ab47bc;\n}\n\n.driver-details:nth-child(5n+3) .driver-initials {\n  background: #a5a5a5;\n}\n\n.driver-details:nth-child(5n+4) .driver-initials {\n  background: #dc4949;\n}\n\n.driver-details:nth-child(5n+5) .driver-initials {\n  background: #00897b;\n}\n\n/*.table.dataTable {\n    margin-top: 0 !important;\n}\n*/\n\n.sticky-header-wmd {\n  position: fixed;\n  right: 0;\n  padding: 15px 20px;\n  top: 45px;\n  height: 65px;\n  /*font-size: 20px;*/\n  z-index: 10;\n  background: #fff;\n}\n\n.locationfilter {\n  width: 100%;\n  position: absolute;\n  right: 4px;\n  border-radius: 5px;\n  font-size: 14px;\n  z-index: 1010;\n}\n\n.sticky_header {\n  position: sticky;\n  top: 45px;\n  padding: 5px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n  margin-bottom: 0px;\n  margin-top: -10px;\n  /*box-shadow: 0 3px 15px 0 rgba(0,0,0,.1);*/\n  border-radius: 2px;\n}\n\n.display_hide {\n  display: none;\n  transition: opacity 1s ease-out;\n  opacity: 0;\n}\n\n.expand_map_btn {\n  position: absolute;\n  top: 1px;\n  right: 15px;\n  background: #fff;\n  border-radius: 2px 2px 2px 2px;\n  padding: 3px;\n  box-shadow: -2px 2px 6px 1px #aaa;\n  z-index: 1;\n}\n\n.driver_btn {\n  position: absolute;\n  top: 15px;\n  left: -35px;\n  background: white;\n  border-radius: 2px;\n  border-top-left-radius: 5px;\n  border-bottom-left-radius: 5px;\n  padding: 5px;\n  box-shadow: -4px 0px 4px 0px #aaaaaa;\n}\n\n.absolute_driver {\n  position: fixed;\n  width: 25%;\n  top: 100px;\n  right: 0;\n  background: #fff;\n  z-index: 11;\n  padding: 10px;\n  box-shadow: 0 3px 15px 0 rgba(0, 0, 0, 0.1);\n  border-radius: 10px;\n}\n\n.hide_absolute_driver {\n  width: 0;\n  right: -20px;\n}\n\n.activeRoute {\n  font-weight: 600;\n  cursor: pointer;\n  background: #f5f5f5;\n}\n\n.live {\n  height: 10px;\n  width: 10px;\n  border-radius: 50%;\n  background-color: green;\n  position: absolute;\n  top: -1px;\n  right: 1px;\n  transform: scale(1);\n  -webkit-animation: pulse 1s infinite;\n          animation: pulse 1s infinite;\n}\n\n.inactive {\n  height: 10px;\n  width: 10px;\n  border-radius: 50%;\n  background-color: orange;\n  position: absolute;\n  top: -1px;\n  right: 1px;\n}\n\n@-webkit-keyframes pulse {\n  0% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0.4);\n  }\n  70% {\n    box-shadow: 0 0 0 10px rgba(204, 169, 44, 0);\n  }\n  100% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0);\n  }\n}\n\n@keyframes pulse {\n  0% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0.4);\n  }\n  70% {\n    box-shadow: 0 0 0 10px rgba(204, 169, 44, 0);\n  }\n  100% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0);\n  }\n}\n\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.carrier-popover.popover .popover-body {\n  padding: 10px;\n  border-radius: 5px;\n}\n\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC9EOlxcVEZTY29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2VcXFNpdGVGdWVsLkV4Y2hhbmdlLlNvdXJjZUNvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlLldlYi9zcmNcXGFwcFxcZGlzcGF0Y2hlclxcZGlzcGF0Y2hlci1kYXNoYm9hcmRcXHdoZXJlLWlzLW15LWRyaXZlci5jb21wb25lbnQuc2NzcyIsInNyYy9hcHAvZGlzcGF0Y2hlci9kaXNwYXRjaGVyLWRhc2hib2FyZC93aGVyZS1pcy1teS1kcml2ZXIuY29tcG9uZW50LnNjc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7RUFDSSxtQkFBQTtBQ0NKOztBREVBO0VBQ0ksbUJBQUE7QUNDSjs7QURFQTtFQUNJLG1CQUFBO0FDQ0o7O0FERUE7RUFDSSxtQkFBQTtBQ0NKOztBREVBO0VBQ0ksbUJBQUE7QUNDSjs7QURFQTs7O0NBQUE7O0FBSUE7RUFDSSxlQUFBO0VBQ0EsUUFBQTtFQUNBLGtCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxtQkFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtBQ0NKOztBREdBO0VBQ0ksV0FBQTtFQUNBLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGFBQUE7QUNBSjs7QURHQTtFQUVJLGdCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxpQkFBQTtFQUNBLDJDQUFBO0VBQ0Esa0JBQUE7QUNBSjs7QURHQTtFQUNJLGFBQUE7RUFDQSwrQkFBQTtFQUNBLFVBQUE7QUNBSjs7QURHQTtFQUNJLGtCQUFBO0VBQ0EsUUFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtFQUNBLDhCQUFBO0VBQ0EsWUFBQTtFQUNBLGlDQUFBO0VBQ0EsVUFBQTtBQ0FKOztBRElBO0VBQ0ksa0JBQUE7RUFDQSxTQUFBO0VBQ0EsV0FBQTtFQUNBLGlCQUFBO0VBQ0Esa0JBQUE7RUFDQSwyQkFBQTtFQUNBLDhCQUFBO0VBQ0EsWUFBQTtFQUNBLG9DQUFBO0FDREo7O0FESUE7RUFDSSxlQUFBO0VBQ0EsVUFBQTtFQUNBLFVBQUE7RUFDQSxRQUFBO0VBQ0EsZ0JBQUE7RUFDQSxXQUFBO0VBQ0EsYUFBQTtFQUNBLDJDQUFBO0VBQ0EsbUJBQUE7QUNESjs7QURJQTtFQUNJLFFBQUE7RUFDQSxZQUFBO0FDREo7O0FESUE7RUFDSSxnQkFBQTtFQUNBLGVBQUE7RUFDQSxtQkFBQTtBQ0RKOztBRElBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSxrQkFBQTtFQUNBLHVCQUFBO0VBQ0Esa0JBQUE7RUFDQSxTQUFBO0VBQ0EsVUFBQTtFQUNBLG1CQUFBO0VBQ0Esb0NBQUE7VUFBQSw0QkFBQTtBQ0RKOztBRElBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSxrQkFBQTtFQUNBLHdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxTQUFBO0VBQ0EsVUFBQTtBQ0RKOztBRElBO0VBQ0k7SUFFSSwyQ0FBQTtFQ0ROO0VESUU7SUFFSSw0Q0FBQTtFQ0ZOO0VES0U7SUFFSSx5Q0FBQTtFQ0hOO0FBQ0Y7O0FEWEE7RUFDSTtJQUVJLDJDQUFBO0VDRE47RURJRTtJQUVJLDRDQUFBO0VDRk47RURLRTtJQUVJLHlDQUFBO0VDSE47QUFDRjs7QURNQTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDSko7O0FET0E7RUFDSSxhQUFBO0VBQ0Esa0JBQUE7QUNKSjs7QURPSTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDSlI7O0FETVE7RUFJSSxVQUFBO0VBQ0Esa0JBQUE7RUFDQSxtQkFBQTtBQ1BaOztBRFVRO0VBQ0ksYUFBQTtBQ1JaOztBRFdZO0VBQ0ksMkJBQUE7QUNUaEI7O0FEYVE7RUFDSSwyQ0FBQTtBQ1haOztBRGdCQTtFQUNDLGtCQUFBO0VBQ0csVUFBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtFQUNBLG9CQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtFQUNILFdBQUE7RUFDRyxZQUFBO0FDYkoiLCJmaWxlIjoic3JjL2FwcC9kaXNwYXRjaGVyL2Rpc3BhdGNoZXItZGFzaGJvYXJkL3doZXJlLWlzLW15LWRyaXZlci5jb21wb25lbnQuc2NzcyIsInNvdXJjZXNDb250ZW50IjpbIi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rMSkgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZjZhZjI3O1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzIpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogI2FiNDdiYztcclxufVxyXG5cclxuLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1biszKSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICNhNWE1YTU7XHJcbn1cclxuXHJcbi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rNCkgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZGM0OTQ5O1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzUpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogIzAwODk3YjtcclxufVxyXG5cclxuLyoudGFibGUuZGF0YVRhYmxlIHtcclxuICAgIG1hcmdpbi10b3A6IDAgIWltcG9ydGFudDtcclxufVxyXG4qL1xyXG4uc3RpY2t5LWhlYWRlci13bWQge1xyXG4gICAgcG9zaXRpb246IGZpeGVkO1xyXG4gICAgcmlnaHQ6IDA7XHJcbiAgICBwYWRkaW5nOiAxNXB4IDIwcHg7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBoZWlnaHQ6IDY1cHg7XHJcbiAgICAvKmZvbnQtc2l6ZTogMjBweDsqL1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcblxyXG5cclxuLmxvY2F0aW9uZmlsdGVyIHtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgcmlnaHQ6IDRweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMTRweDtcclxuICAgIHotaW5kZXg6IDEwMTA7XHJcbn1cclxuXHJcbi5zdGlja3lfaGVhZGVyIHtcclxuICAgIHBvc2l0aW9uOiAtd2Via2l0LXN0aWNreTtcclxuICAgIHBvc2l0aW9uOiBzdGlja3k7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBwYWRkaW5nOiA1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiAxMDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbiAgICBtYXJnaW4tdG9wOiAtMTBweDtcclxuICAgIC8qYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwwLDAsLjEpOyovXHJcbiAgICBib3JkZXItcmFkaXVzOiAycHg7XHJcbn1cclxuXHJcbi5kaXNwbGF5X2hpZGUge1xyXG4gICAgZGlzcGxheTogbm9uZTtcclxuICAgIHRyYW5zaXRpb246IG9wYWNpdHkgMXMgZWFzZS1vdXQ7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG59XHJcblxyXG4uZXhwYW5kX21hcF9idG4ge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAxcHg7XHJcbiAgICByaWdodDogMTVweDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBib3JkZXItcmFkaXVzOiAycHggMnB4IDJweCAycHg7XHJcbiAgICBwYWRkaW5nOiAzcHg7XHJcbiAgICBib3gtc2hhZG93OiAtMnB4IDJweCA2cHggMXB4ICNhYWE7XHJcbiAgICB6LWluZGV4OiAxO1xyXG59XHJcblxyXG5cclxuLmRyaXZlcl9idG4ge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAxNXB4O1xyXG4gICAgbGVmdDogLTM1cHg7XHJcbiAgICBiYWNrZ3JvdW5kOiB3aGl0ZTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDJweDtcclxuICAgIGJvcmRlci10b3AtbGVmdC1yYWRpdXM6IDVweDtcclxuICAgIGJvcmRlci1ib3R0b20tbGVmdC1yYWRpdXM6IDVweDtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIGJveC1zaGFkb3c6IC00cHggMHB4IDRweCAwcHggI2FhYWFhYTtcclxufVxyXG5cclxuLmFic29sdXRlX2RyaXZlciB7XHJcbiAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbiAgICB3aWR0aDogMjUlO1xyXG4gICAgdG9wOiAxMDBweDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxuICAgIHotaW5kZXg6IDExO1xyXG4gICAgcGFkZGluZzogMTBweDtcclxuICAgIGJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5oaWRlX2Fic29sdXRlX2RyaXZlciB7XHJcbiAgICB3aWR0aDogMDtcclxuICAgIHJpZ2h0OiAtMjBweDtcclxufVxyXG5cclxuLmFjdGl2ZVJvdXRlIHtcclxuICAgIGZvbnQtd2VpZ2h0OiA2MDA7XHJcbiAgICBjdXJzb3I6IHBvaW50ZXI7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZjVmNWY1O1xyXG59XHJcblxyXG4ubGl2ZSB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IGdyZWVuO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAtMXB4O1xyXG4gICAgcmlnaHQ6IDFweDtcclxuICAgIHRyYW5zZm9ybTogc2NhbGUoMSk7XHJcbiAgICBhbmltYXRpb246IHB1bHNlIDFzIGluZmluaXRlO1xyXG59XHJcblxyXG4uaW5hY3RpdmUge1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiBvcmFuZ2U7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IC0xcHg7XHJcbiAgICByaWdodDogMXB4O1xyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIHB1bHNlIHtcclxuICAgIDAlIHtcclxuICAgICAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwLjQpO1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwLjQpO1xyXG4gICAgfVxyXG5cclxuICAgIDcwJSB7XHJcbiAgICAgICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAxMHB4IHJnYmEoMjA0LDE2OSw0NCwgMCk7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMCAwIDAgMTBweCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgfVxyXG5cclxuICAgIDEwMCUge1xyXG4gICAgICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwKTtcclxuICAgIH1cclxufVxyXG5cclxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcclxuICAgIG1pbi13aWR0aDogMzAwcHg7XHJcbiAgICBtYXgtd2lkdGg6IDM1MHB4O1xyXG4gICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuLm1hc3Rlci1maWx0ZXIge1xyXG4gICAgJi5wb3BvdmVyIHtcclxuICAgICAgICBtaW4td2lkdGg6IDQyNXB4O1xyXG4gICAgICAgIG1heC13aWR0aDogNDUwcHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgICAgICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xyXG4gICAgICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG5cclxuICAgICAgICAucG9wb3Zlci1ib2R5IHtcclxuICAgICAgICAgICAgLy8gbWF4LWhlaWdodDogMzUwcHg7XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXg6IGhpZGRlbjtcclxuICAgICAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICAgICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgICAgICAgICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLnBvcG92ZXItZGV0YWlscyB7XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDE1cHg7XHJcbiAgICAgICAgICAgIC8vIG1heC1oZWlnaHQ6IDMxMHB4O1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgICAgICAgICAuZm9udC1ib2xkIHtcclxuICAgICAgICAgICAgICAgIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLmJvcmRlci1ib3R0b20tMiB7XHJcbiAgICAgICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59XHJcblxyXG4uY2lyY2xlLWJhZGdle1xyXG5cdHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogLTExcHg7XHJcbiAgICBsZWZ0OiAtMTRweDtcclxuICAgIGJhY2tncm91bmQ6IHJnYigyNTAsIDE0NywgMTQ3KTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGZvbnQtc2l6ZTogMTJweDtcclxuICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgIGNvbG9yOiB3aGl0ZTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1mbGV4O1xyXG4gICAgYWxpZ24taXRlbXM6IGNlbnRlcjtcclxuICAgIGp1c3RpZnktY29udGVudDogY2VudGVyO1xyXG5cdHdpZHRoOiAxOHB4O1xyXG4gICAgaGVpZ2h0OiAxOHB4XHJcbn0iLCIuZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzEpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjZjZhZjI3O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzIpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjYWI0N2JjO1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzMpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjYTVhNWE1O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzQpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjZGM0OTQ5O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzUpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjMDA4OTdiO1xufVxuXG4vKi50YWJsZS5kYXRhVGFibGUge1xuICAgIG1hcmdpbi10b3A6IDAgIWltcG9ydGFudDtcbn1cbiovXG4uc3RpY2t5LWhlYWRlci13bWQge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHJpZ2h0OiAwO1xuICBwYWRkaW5nOiAxNXB4IDIwcHg7XG4gIHRvcDogNDVweDtcbiAgaGVpZ2h0OiA2NXB4O1xuICAvKmZvbnQtc2l6ZTogMjBweDsqL1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbn1cblxuLmxvY2F0aW9uZmlsdGVyIHtcbiAgd2lkdGg6IDEwMCU7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgcmlnaHQ6IDRweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBmb250LXNpemU6IDE0cHg7XG4gIHotaW5kZXg6IDEwMTA7XG59XG5cbi5zdGlja3lfaGVhZGVyIHtcbiAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xuICBwb3NpdGlvbjogc3RpY2t5O1xuICB0b3A6IDQ1cHg7XG4gIHBhZGRpbmc6IDVweDtcbiAgZm9udC1zaXplOiAyMHB4O1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbiAgbWFyZ2luLWJvdHRvbTogMHB4O1xuICBtYXJnaW4tdG9wOiAtMTBweDtcbiAgLypib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7Ki9cbiAgYm9yZGVyLXJhZGl1czogMnB4O1xufVxuXG4uZGlzcGxheV9oaWRlIHtcbiAgZGlzcGxheTogbm9uZTtcbiAgdHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcbiAgb3BhY2l0eTogMDtcbn1cblxuLmV4cGFuZF9tYXBfYnRuIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IDFweDtcbiAgcmlnaHQ6IDE1cHg7XG4gIGJhY2tncm91bmQ6ICNmZmY7XG4gIGJvcmRlci1yYWRpdXM6IDJweCAycHggMnB4IDJweDtcbiAgcGFkZGluZzogM3B4O1xuICBib3gtc2hhZG93OiAtMnB4IDJweCA2cHggMXB4ICNhYWE7XG4gIHotaW5kZXg6IDE7XG59XG5cbi5kcml2ZXJfYnRuIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IDE1cHg7XG4gIGxlZnQ6IC0zNXB4O1xuICBiYWNrZ3JvdW5kOiB3aGl0ZTtcbiAgYm9yZGVyLXJhZGl1czogMnB4O1xuICBib3JkZXItdG9wLWxlZnQtcmFkaXVzOiA1cHg7XG4gIGJvcmRlci1ib3R0b20tbGVmdC1yYWRpdXM6IDVweDtcbiAgcGFkZGluZzogNXB4O1xuICBib3gtc2hhZG93OiAtNHB4IDBweCA0cHggMHB4ICNhYWFhYWE7XG59XG5cbi5hYnNvbHV0ZV9kcml2ZXIge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHdpZHRoOiAyNSU7XG4gIHRvcDogMTAwcHg7XG4gIHJpZ2h0OiAwO1xuICBiYWNrZ3JvdW5kOiAjZmZmO1xuICB6LWluZGV4OiAxMTtcbiAgcGFkZGluZzogMTBweDtcbiAgYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwgMCwgMCwgMC4xKTtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbn1cblxuLmhpZGVfYWJzb2x1dGVfZHJpdmVyIHtcbiAgd2lkdGg6IDA7XG4gIHJpZ2h0OiAtMjBweDtcbn1cblxuLmFjdGl2ZVJvdXRlIHtcbiAgZm9udC13ZWlnaHQ6IDYwMDtcbiAgY3Vyc29yOiBwb2ludGVyO1xuICBiYWNrZ3JvdW5kOiAjZjVmNWY1O1xufVxuXG4ubGl2ZSB7XG4gIGhlaWdodDogMTBweDtcbiAgd2lkdGg6IDEwcHg7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgYmFja2dyb3VuZC1jb2xvcjogZ3JlZW47XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAtMXB4O1xuICByaWdodDogMXB4O1xuICB0cmFuc2Zvcm06IHNjYWxlKDEpO1xuICBhbmltYXRpb246IHB1bHNlIDFzIGluZmluaXRlO1xufVxuXG4uaW5hY3RpdmUge1xuICBoZWlnaHQ6IDEwcHg7XG4gIHdpZHRoOiAxMHB4O1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGJhY2tncm91bmQtY29sb3I6IG9yYW5nZTtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IC0xcHg7XG4gIHJpZ2h0OiAxcHg7XG59XG5cbkBrZXlmcmFtZXMgcHVsc2Uge1xuICAwJSB7XG4gICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LCAxNjksIDQ0LCAwLjQpO1xuICAgIGJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsIDE2OSwgNDQsIDAuNCk7XG4gIH1cbiAgNzAlIHtcbiAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDEwcHggcmdiYSgyMDQsIDE2OSwgNDQsIDApO1xuICAgIGJveC1zaGFkb3c6IDAgMCAwIDEwcHggcmdiYSgyMDQsIDE2OSwgNDQsIDApO1xuICB9XG4gIDEwMCUge1xuICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwgMTY5LCA0NCwgMCk7XG4gICAgYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwgMTY5LCA0NCwgMCk7XG4gIH1cbn1cbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciB7XG4gIG1pbi13aWR0aDogMzAwcHg7XG4gIG1heC13aWR0aDogMzUwcHg7XG4gIGJhY2tncm91bmQ6ICNGOUY5Rjk7XG4gIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2JhKDAsIDAsIDAsIDAuMTMpO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuXG4uY2Fycmllci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XG4gIHBhZGRpbmc6IDEwcHg7XG4gIGJvcmRlci1yYWRpdXM6IDVweDtcbn1cblxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciB7XG4gIG1pbi13aWR0aDogNDI1cHg7XG4gIG1heC13aWR0aDogNDUwcHg7XG4gIGJhY2tncm91bmQ6ICNGOUY5Rjk7XG4gIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2JhKDAsIDAsIDAsIDAuMTMpO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcbiAgcGFkZGluZzogMDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcbiAgcGFkZGluZzogMTVweDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAuZm9udC1ib2xkIHtcbiAgZm9udC13ZWlnaHQ6IDYwMCAhaW1wb3J0YW50O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAuYm9yZGVyLWJvdHRvbS0yIHtcbiAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNlN2VhZWMgIWltcG9ydGFudDtcbn1cblxuLmNpcmNsZS1iYWRnZSB7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAtMTFweDtcbiAgbGVmdDogLTE0cHg7XG4gIGJhY2tncm91bmQ6ICNmYTkzOTM7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgZm9udC1zaXplOiAxMnB4O1xuICB0ZXh0LWFsaWduOiBjZW50ZXI7XG4gIGNvbG9yOiB3aGl0ZTtcbiAgZGlzcGxheTogaW5saW5lLWZsZXg7XG4gIGFsaWduLWl0ZW1zOiBjZW50ZXI7XG4gIGp1c3RpZnktY29udGVudDogY2VudGVyO1xuICB3aWR0aDogMThweDtcbiAgaGVpZ2h0OiAxOHB4O1xufSJdfQ== */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](WhereIsMyDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-where-is-my-driver',
          templateUrl: './where-is-my-driver.component.html',
          styleUrls: ['./where-is-my-driver.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_13__["FormBuilder"]
        }, {
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_14__["DispatcherService"]
        }, {
          type: src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_15__["chatService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_16__["CarrierService"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]
        }];
      }, {
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        myDiv: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['myDiv']
        }],
        loadsMapView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"]]
        }],
        loadsGridView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"]]
        }],
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }],
        selectedDriverLoad: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['SelectedDriverLoad', {
            read: angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"],
            "static": false
          }]
        }],
        sendbirdComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["SendbirdComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher-routing.module.ts": function srcAppDispatcherDispatcherRoutingModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DispatcherRoutingModule", function () {
      return DispatcherRoutingModule;
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


    var _dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./dispatcher-dashboard/dispatcher-dashboard.component */
    "./src/app/dispatcher/dispatcher-dashboard/dispatcher-dashboard.component.ts");

    var routeDispatcher = [{
      path: "",
      component: _dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["DispatcherDashboardComponent"]
    }, {
      path: "Dashboard",
      component: _dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["DispatcherDashboardComponent"]
    }, {
      path: "Dashboard/Index",
      component: _dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["DispatcherDashboardComponent"]
    }];

    var DispatcherRoutingModule = function DispatcherRoutingModule() {
      _classCallCheck(this, DispatcherRoutingModule);
    };

    DispatcherRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: DispatcherRoutingModule
    });
    DispatcherRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function DispatcherRoutingModule_Factory(t) {
        return new (t || DispatcherRoutingModule)();
      },
      imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDispatcher)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DispatcherRoutingModule, {
        imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DispatcherRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDispatcher)],
          exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/dispatcher/dispatcher.module.ts": function srcAppDispatcherDispatcherModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DispatcherModule", function () {
      return DispatcherModule;
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


    var agm_direction__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! agm-direction */
    "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _dispatcher_routing_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./dispatcher-routing.module */
    "./src/app/dispatcher/dispatcher-routing.module.ts");
    /* harmony import */


    var _dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./dispatcher-dashboard/dispatcher-dashboard.component */
    "./src/app/dispatcher/dispatcher-dashboard/dispatcher-dashboard.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_location_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ./dispatcher-dashboard/location.component */
    "./src/app/dispatcher/dispatcher-dashboard/location.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./dispatcher-dashboard/where-is-my-driver.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./dispatcher-dashboard/where-is-my-driver-map-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-map-view.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./dispatcher-dashboard/where-is-my-driver-grid-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/where-is-my-driver-grid-view.component.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var ng2_charts__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ng2-charts */
    "./node_modules/ng2-charts/__ivy_ngcc__/fesm2015/ng2-charts.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _dispatcher_dashboard_sales_data_sales_data_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./dispatcher-dashboard/sales-data/sales-data.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/sales-data.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_sales_data_grid_view_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ./dispatcher-dashboard/sales-data/grid-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/grid-view.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ./dispatcher-dashboard/sales-data/tank-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ./dispatcher-dashboard/sales-data/location-view.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/location-view.component.ts");
    /* harmony import */


    var _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ../tank-chart/tank-chart.module */
    "./src/app/tank-chart/tank-chart.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _dispatcher_dashboard_job_tank_hierarchy_job_tank_hierarchy_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ./dispatcher-dashboard/job-tank-hierarchy/job-tank-hierarchy.component */
    "./src/app/dispatcher/dispatcher-dashboard/job-tank-hierarchy/job-tank-hierarchy.component.ts");
    /* harmony import */


    var _dispatcher_dashboard_sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(
    /*! ./dispatcher-dashboard/sales-data/tank-view-master.component */
    "./src/app/dispatcher/dispatcher-dashboard/sales-data/tank-view-master.component.ts");

    var DispatcherModule = function DispatcherModule() {
      _classCallCheck(this, DispatcherModule);
    };

    DispatcherModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: DispatcherModule
    });
    DispatcherModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function DispatcherModule_Factory(t) {
        return new (t || DispatcherModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], agm_direction__WEBPACK_IMPORTED_MODULE_2__["AgmDirectionModule"], _dispatcher_routing_module__WEBPACK_IMPORTED_MODULE_4__["DispatcherRoutingModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_10__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_11__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_12__["DataTablesModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_17__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_18__["FormsModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DispatcherModule, {
        declarations: [_dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_5__["DispatcherDashboardComponent"], _dispatcher_dashboard_location_component__WEBPACK_IMPORTED_MODULE_6__["LocationComponent"], _dispatcher_dashboard_where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverComponent"], _dispatcher_dashboard_where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverMapViewComponent"], _dispatcher_dashboard_where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverGridViewComponent"], _dispatcher_dashboard_sales_data_sales_data_component__WEBPACK_IMPORTED_MODULE_13__["SalesDataComponent"], _dispatcher_dashboard_sales_data_grid_view_component__WEBPACK_IMPORTED_MODULE_14__["GridViewComponent"], _dispatcher_dashboard_sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_15__["TankViewComponent"], _dispatcher_dashboard_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_16__["LocationViewComponent"], _dispatcher_dashboard_job_tank_hierarchy_job_tank_hierarchy_component__WEBPACK_IMPORTED_MODULE_19__["JobTankHierarchyComponent"], _dispatcher_dashboard_sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_20__["TankViewMasterComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], agm_direction__WEBPACK_IMPORTED_MODULE_2__["AgmDirectionModule"], _dispatcher_routing_module__WEBPACK_IMPORTED_MODULE_4__["DispatcherRoutingModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_10__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_11__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_12__["DataTablesModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_17__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_18__["FormsModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DispatcherModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_dispatcher_dashboard_dispatcher_dashboard_component__WEBPACK_IMPORTED_MODULE_5__["DispatcherDashboardComponent"], _dispatcher_dashboard_location_component__WEBPACK_IMPORTED_MODULE_6__["LocationComponent"], _dispatcher_dashboard_where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverComponent"], _dispatcher_dashboard_where_is_my_driver_map_view_component__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverMapViewComponent"], _dispatcher_dashboard_where_is_my_driver_grid_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverGridViewComponent"], _dispatcher_dashboard_sales_data_sales_data_component__WEBPACK_IMPORTED_MODULE_13__["SalesDataComponent"], _dispatcher_dashboard_sales_data_grid_view_component__WEBPACK_IMPORTED_MODULE_14__["GridViewComponent"], _dispatcher_dashboard_sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_15__["TankViewComponent"], _dispatcher_dashboard_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_16__["LocationViewComponent"], _dispatcher_dashboard_job_tank_hierarchy_job_tank_hierarchy_component__WEBPACK_IMPORTED_MODULE_19__["JobTankHierarchyComponent"], _dispatcher_dashboard_sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_20__["TankViewMasterComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], agm_direction__WEBPACK_IMPORTED_MODULE_2__["AgmDirectionModule"], _dispatcher_routing_module__WEBPACK_IMPORTED_MODULE_4__["DispatcherRoutingModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_10__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_11__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_12__["DataTablesModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_17__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_18__["FormsModule"]]
        }]
      }], null, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=dispatcher-dispatcher-module-es5.js.map