function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["carrier-delivery-request-report-delivery-request-report-module"], {
  /***/
  "./src/app/carrier/delivery-request-report/delivery-request-report.component.ts": function srcAppCarrierDeliveryRequestReportDeliveryRequestReportComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DeliveryRequestReportComponent", function () {
      return DeliveryRequestReportComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function DeliveryRequestReportComponent_tbody_33_ng_container_1_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function DeliveryRequestReportComponent_tbody_33_ng_container_1_span_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    var _c0 = function _c0(a0, a1, a2, a3) {
      return {
        "bg_must_go": a0,
        "bg_should_go": a1,
        "bg_could_go": a2,
        "bg_noDlr_go": a3
      };
    };

    function DeliveryRequestReportComponent_tbody_33_ng_container_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "tr", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, DeliveryRequestReportComponent_tbody_33_ng_container_1_span_18_Template, 2, 0, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, DeliveryRequestReportComponent_tbody_33_ng_container_1_span_19_Template, 2, 0, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var dr_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction4"](11, _c0, dr_r3.Priority === 1, dr_r3.Priority === 2, dr_r3.Priority === 3, dr_r3.Priority === 4));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](dr_r3.RegionName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](dr_r3.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", dr_r3.LocationId, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", dr_r3.CustomerName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](dr_r3.CustomerBrandID == null || dr_r3.CustomerBrandID == "" ? "--" : dr_r3.CustomerBrandID);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", dr_r3.PoNumber, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](dr_r3.ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", dr_r3.RequestedQuantity + dr_r3.UoM, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", dr_r3.IsRecurringSchedule == true);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", dr_r3.IsAutoDR == true);
      }
    }

    function DeliveryRequestReportComponent_tbody_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, DeliveryRequestReportComponent_tbody_33_ng_container_1_Template, 20, 16, "ng-container", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r0.DRReportsData);
      }
    }

    function DeliveryRequestReportComponent_div_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    var DeliveryRequestReportComponent = /*#__PURE__*/function () {
      function DeliveryRequestReportComponent(carrierServ) {
        _classCallCheck(this, DeliveryRequestReportComponent);

        this.carrierServ = carrierServ; //filter and dropdown variables

        this.LocationDdlSettings = {};
        this.RegionDdlSettings = {};
        this.Regions = [];
        this.Locations = [];
        this.SelectedRegions = [];
        this.SelectedLocations = []; // data binding variables

        this.DRReportsData = [];
        this.unchangedDRReportsData = [];
        this.IsLoading = false; // using these two values in deselect as ngModel not updating after deselect event

        this.SelectedRegionsForFilter = [];
        this.SelectedLocationsForFilter = [];
        this.dtDRGridOptions = {};
        this.dtDRReportTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
      }

      _createClass(DeliveryRequestReportComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this = this;

          // this.ToDate = this.TodaysDate;
          // this.FromDate = moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');
          //this.ToDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
          //this.FromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');
          var exportColumns = {
            columns: ':visible'
          };
          var DRcolumnsDetails = [];
          this.LocationDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: true
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
          DRcolumnsDetails = [{
            title: 'Location',
            name: 'Location',
            data: 'Location',
            "autoWidth": true
          }, {
            title: 'Region',
            name: 'RegionName',
            data: 'RegionName',
            "autoWidth": true
          }, {
            title: 'Customer',
            name: 'CustomerName',
            data: 'CustomerName',
            "autoWidth": true
          }, {
            title: 'Customer BrandID',
            name: 'CustomerBrandID',
            data: 'Customer BrandID',
            "autoWidth": true
          }, {
            title: 'Product',
            name: 'ProductType',
            data: 'ProductType',
            "autoWidth": true
          }, {
            title: 'Requested Qty',
            name: 'RequestedQty',
            data: 'RequestedQuantity',
            "autoWidth": true
          }, {
            title: 'LocationId',
            name: 'LocationId',
            data: 'LocationId',
            "autoWidth": true
          }, {
            title: 'Order',
            name: 'Order',
            data: 'PoNumber',
            "autoWidth": true
          }];
          this.dtDRGridOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            //searching: true,
            dom: '<"html5buttons"B>lTfgitp',
            autoWidth: true,
            fixedHeader: true,
            //ordering: false,
            search: true,
            destroy: true,
            order: [],
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Delivery Request Report',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Delivery Request Report',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }]
          };
          this.IsLoading = true;
          this.carrierServ.getDRReportFilters().subscribe(function (data) {
            if (data != null && data != undefined) {
              var regionsIds = [];
              var locationIds = [];
              _this.Regions = data.Regions;
              _this.Locations = data.Locations;
              _this.SelectedRegionId = '';

              _this.Regions.forEach(function (res) {
                regionsIds.push(res.Id);
              });

              _this.SelectedRegionId = regionsIds.join();
              _this.SelectedLocationId = '';

              _this.Locations.forEach(function (res) {
                locationIds.push(res.Id);
              });

              _this.SelectedLocationId = locationIds.join();

              _this.getDRReportGridData();
            }
          });
          this.IsLoading = false;
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.dtDRReportTrigger.next();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtDRReportTrigger.unsubscribe();
        }
      }, {
        key: "OnFilterSelect",
        value: function OnFilterSelect(event, filterType) {
          var regionsIds = [];
          var locationIds = [];

          if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(function (res) {
              regionsIds.push(res.Id);
            });
            this.SelectedRegionId = regionsIds.join();
          }

          if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(function (res) {
              locationIds.push(res.Id);
            });
            this.SelectedLocationId = locationIds.join();
          }

          this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
        }
      }, {
        key: "onFilterSelectAll",
        value: function onFilterSelectAll(event, filterType) {
          var regionsIds = [];
          var locationIds = [];

          if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(function (res) {
              regionsIds.push(res.Id);
            });
            this.SelectedRegionId = regionsIds.join();
          }

          if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(function (res) {
              locationIds.push(res.Id);
            });
            this.SelectedLocationId = locationIds.join();
          }

          this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
        }
      }, {
        key: "onFilterDeselect",
        value: function onFilterDeselect(event, filterType) {
          var regionsIds = [];
          var locationIds = [];

          if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(function (res) {
              regionsIds.push(res.Id);
            });
            this.SelectedRegionId = regionsIds.join();
          }

          if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(function (res) {
              locationIds.push(res.Id);
            });
            this.SelectedLocationId = locationIds.join();
          }

          this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
        }
      }, {
        key: "onFilterDeselectAll",
        value: function onFilterDeselectAll(event, filterType) {
          var regionsIds = [];
          var locationIds = [];

          if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(function (res) {
              regionsIds.push(res.Id);
            });
            this.SelectedRegionId = regionsIds.join();
          }

          if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(function (res) {
              locationIds.push(res.Id);
            });
            this.SelectedLocationId = locationIds.join();
          }

          this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          var _this2 = this;

          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();

              _this2.dtDRReportTrigger.next();
            });
          }
        }
      }, {
        key: "getDRReportGridData",
        value: function getDRReportGridData() {
          var _this3 = this;

          this.IsLoading = true;
          var inputData = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DRReportFilterInputViewModel"]();
          inputData.RegionIds = this.SelectedRegionId;
          inputData.LocationIds = this.SelectedLocationId;
          inputData.FromDate = '';
          inputData.ToDate = '';
          this.carrierServ.getDRReportGridData(inputData).subscribe(function (data) {
            _this3.unchangedDRReportsData = data;
            _this3.DRReportsData = data;
            _this3.IsLoading = false;

            _this3.datatableRerender();
          });
        }
      }, {
        key: "filterDRReportData",
        value: function filterDRReportData(selectedRegions, selectedLocations) {
          this.IsLoading = true;
          var filteredData = [];
          var filteredDataByRegions = [];
          var filteredDataByLocations = [];

          if (selectedRegions == null || selectedRegions == undefined || selectedRegions.length == 0) {
            selectedRegions = this.Regions;
          }

          if (selectedLocations == null || selectedLocations == undefined || selectedLocations.length == 0) {
            selectedLocations = this.Locations;
          }

          this.unchangedDRReportsData.forEach(function (data) {
            if (selectedRegions != null && selectedRegions != undefined && selectedRegions.length > 0) {
              for (var i = 0; i < selectedRegions.length; i++) {
                if (selectedRegions[i].Id == data.RegionId) {
                  filteredDataByRegions.push(data);
                }
              }
            }
          });

          if (filteredDataByRegions != null && filteredDataByRegions != undefined && filteredDataByRegions.length > 0) {
            filteredDataByRegions.forEach(function (data) {
              if (selectedLocations != null && selectedLocations != undefined && selectedLocations.length > 0) {
                for (var i = 0; i < selectedLocations.length; i++) {
                  if (selectedLocations[i].Id == data.TfxJobId) {
                    filteredDataByLocations.push(data);
                  }
                }
              }
            });
            this.DRReportsData = filteredDataByLocations;
          } else {
            this.DRReportsData = filteredDataByRegions;
          }

          this.datatableRerender();
          this.IsLoading = false;
        }
      }]);

      return DeliveryRequestReportComponent;
    }();

    DeliveryRequestReportComponent.??fac = function DeliveryRequestReportComponent_Factory(t) {
      return new (t || DeliveryRequestReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]));
    };

    DeliveryRequestReportComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: DeliveryRequestReportComponent,
      selectors: [["app-delivery-request-report"]],
      viewQuery: function DeliveryRequestReportComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
        }
      },
      decls: 35,
      vars: 12,
      consts: [[1, "col-sm-8", "sticky-header-wmd"], [1, "row"], [1, "col-sm-5", "pa0"], [1, "col-sm-6"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-terminal-item-code", 1, "table-responsive"], ["datatable", "", 1, "table", "table-bordered", 3, "dtOptions", "dtTrigger"], ["data-key", "RegionName"], ["data-key", "Location"], ["data-key", "LocId"], ["data-key", "CustomerName"], ["data-key", "CustomerBrandID"], ["data-ke", "Order"], ["data-key", "ProductType"], ["data-key", "RequestedQty"], [4, "ngIf"], ["class", "pa bg-white z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], [3, "ngClass"], ["title", "Recurring", 1, "fas", "fa-sync", "color-black"], ["title", "Auto-Generated", 1, "fas", "fa-magic", "ml10"], [1, "pa", "bg-white", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"]],
      template: function DeliveryRequestReportComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "ng-multiselect-dropdown", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_ngModelChange_5_listener($event) {
            return ctx.SelectedRegions = $event;
          })("onSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelect_5_listener($event) {
            return ctx.OnFilterSelect($event, "region");
          })("onDeSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelect_5_listener($event) {
            return ctx.onFilterDeselect($event, "region");
          })("onSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelectAll_5_listener($event) {
            return ctx.onFilterSelectAll($event, "region");
          })("onDeSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelectAll_5_listener($event) {
            return ctx.onFilterDeselectAll($event, "region");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "ng-multiselect-dropdown", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_ngModelChange_7_listener($event) {
            return ctx.SelectedLocations = $event;
          })("onSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelect_7_listener($event) {
            return ctx.OnFilterSelect($event, "location");
          })("onDeSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelect_7_listener($event) {
            return ctx.onFilterDeselect($event, "location");
          })("onSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelectAll_7_listener($event) {
            return ctx.onFilterSelectAll($event, "location");
          })("onDeSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelectAll_7_listener($event) {
            return ctx.onFilterDeselectAll($event, "location");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "table", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "LocationId");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "Customer");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Customer BrandId");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "Order");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Product");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "Requested Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](33, DeliveryRequestReportComponent_tbody_33_Template, 2, 1, "tbody", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](34, DeliveryRequestReportComponent_div_34_Template, 2, 0, "div", 20);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.SelectedRegions)("settings", ctx.RegionDdlSettings)("placeholder", "Select Region")("data", ctx.Regions);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.SelectedLocations)("settings", ctx.LocationDdlSettings)("placeholder", "Select Location")("data", ctx.Locations);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtDRGridOptions)("dtTrigger", ctx.dtDRReportTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.DRReportsData == null ? null : ctx.DRReportsData.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"]],
      styles: [".table .bg_must_go {\r\n    background-color: #f5d0d0;\r\n}\r\n\r\n  .table .bg_should_go {\r\n    background-color: #f7e6a9;\r\n}\r\n\r\n  .table .bg_could_go {\r\n    background-color: #e4e2e2;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvY2Fycmllci9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0kseUJBQXlCO0FBQzdCOztBQUVBO0lBQ0kseUJBQXlCO0FBQzdCOztBQUVBO0lBQ0kseUJBQXlCO0FBQzdCIiwiZmlsZSI6InNyYy9hcHAvY2Fycmllci9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC50YWJsZSAuYmdfbXVzdF9nbyB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjVkMGQwO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLnRhYmxlIC5iZ19zaG91bGRfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2Y3ZTZhOTtcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJsZSAuYmdfY291bGRfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2U0ZTJlMjtcclxufVxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DeliveryRequestReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-delivery-request-report',
          templateUrl: './delivery-request-report.component.html',
          styleUrls: ['./delivery-request-report.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]
        }];
      }, {
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/carrier/delivery-request-report/delivery-request-report.module.ts": function srcAppCarrierDeliveryRequestReportDeliveryRequestReportModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DeliveryRequestReportModule", function () {
      return DeliveryRequestReportModule;
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


    var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ./delivery-request-report.component */
    "./src/app/carrier/delivery-request-report/delivery-request-report.component.ts");

    var routesDrReport = [{
      path: "",
      component: _delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]
    }];

    var DeliveryRequestReportModule = function DeliveryRequestReportModule() {
      _classCallCheck(this, DeliveryRequestReportModule);
    };

    DeliveryRequestReportModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({
      type: DeliveryRequestReportModule
    });
    DeliveryRequestReportModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({
      factory: function DeliveryRequestReportModule_Factory(t) {
        return new (t || DeliveryRequestReportModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(routesDrReport)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](DeliveryRequestReportModule, {
        declarations: [_delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](DeliveryRequestReportModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(routesDrReport)]
        }]
      }], null, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=carrier-delivery-request-report-delivery-request-report-module-es5.js.map