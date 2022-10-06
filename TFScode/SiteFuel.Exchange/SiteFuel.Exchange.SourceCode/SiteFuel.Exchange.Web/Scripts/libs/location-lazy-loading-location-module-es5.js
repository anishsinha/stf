function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["location-lazy-loading-location-module"], {
  /***/
  "./src/app/location/lazy-loading/location-routing.module.ts": function srcAppLocationLazyLoadingLocationRoutingModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationRoutingModule", function () {
      return LocationRoutingModule;
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


    var src_app_location_pickup_location_pickup_location_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/location/pickup-location/pickup-location.component */
    "./src/app/location/pickup-location/pickup-location.component.ts");

    var routelocation = [{
      path: "",
      component: src_app_location_pickup_location_pickup_location_component__WEBPACK_IMPORTED_MODULE_2__["PickupLocationComponent"],
      data: {
        title: 'Supplier Location'
      }
    }];

    var LocationRoutingModule = function LocationRoutingModule() {
      _classCallCheck(this, LocationRoutingModule);
    };

    LocationRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: LocationRoutingModule
    });
    LocationRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function LocationRoutingModule_Factory(t) {
        return new (t || LocationRoutingModule)();
      },
      imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routelocation)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](LocationRoutingModule, {
        imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LocationRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routelocation)],
          exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/location/lazy-loading/location.module.ts": function srcAppLocationLazyLoadingLocationModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationModule", function () {
      return LocationModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _pickup_location_pickup_location_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../pickup-location/pickup-location.component */
    "./src/app/location/pickup-location/pickup-location.component.ts");
    /* harmony import */


    var _location_routing_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./location-routing.module */
    "./src/app/location/lazy-loading/location-routing.module.ts");
    /* harmony import */


    var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _pickup_location_bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../pickup-location/bulk-plants/bulk-plants.component */
    "./src/app/location/pickup-location/bulk-plants/bulk-plants.component.ts");
    /* harmony import */


    var _pickup_location_terminals_terminals_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../pickup-location/terminals/terminals.component */
    "./src/app/location/pickup-location/terminals/terminals.component.ts");
    /* harmony import */


    var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");

    var LocationModule = function LocationModule() {
      _classCallCheck(this, LocationModule);
    };

    LocationModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: LocationModule
    });
    LocationModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function LocationModule_Factory(t) {
        return new (t || LocationModule)();
      },
      imports: [[_location_routing_module__WEBPACK_IMPORTED_MODULE_2__["LocationRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](LocationModule, {
        declarations: [_pickup_location_pickup_location_component__WEBPACK_IMPORTED_MODULE_1__["PickupLocationComponent"], _pickup_location_bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_4__["BulkPlantsComponent"], _pickup_location_terminals_terminals_component__WEBPACK_IMPORTED_MODULE_5__["TerminalsComponent"]],
        imports: [_location_routing_module__WEBPACK_IMPORTED_MODULE_2__["LocationRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LocationModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_pickup_location_pickup_location_component__WEBPACK_IMPORTED_MODULE_1__["PickupLocationComponent"], _pickup_location_bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_4__["BulkPlantsComponent"], _pickup_location_terminals_terminals_component__WEBPACK_IMPORTED_MODULE_5__["TerminalsComponent"]],
          imports: [_location_routing_module__WEBPACK_IMPORTED_MODULE_2__["LocationRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_7__["DataTablesModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/location/models/location.ts": function srcAppLocationModelsLocationTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationDetailsModel", function () {
      return LocationDetailsModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Country", function () {
      return Country;
    });

    var LocationDetailsModel = function LocationDetailsModel() {
      _classCallCheck(this, LocationDetailsModel);
    };

    var Country;

    (function (Country) {
      Country[Country["USA"] = 1] = "USA";
      Country[Country["CAN"] = 2] = "CAN";
      Country[Country["CAR"] = 4] = "CAR";
    })(Country || (Country = {}));
    /***/

  },

  /***/
  "./src/app/location/pickup-location/bulk-plants/bulk-plants.component.ts": function srcAppLocationPickupLocationBulkPlantsBulkPlantsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "BulkPlantsComponent", function () {
      return BulkPlantsComponent;
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


    var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _models_location__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../../models/location */
    "./src/app/location/models/location.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _statelist_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../../statelist.service */
    "./src/app/statelist.service.ts");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _services_location_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../services/location.service */
    "./src/app/location/services/location.service.ts");
    /* harmony import */


    var _address_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../../../address.service */
    "./src/app/address.service.ts");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    var _c0 = ["closePickUpModal"];

    function BulkPlantsComponent_ng_container_3_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var loc_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"]("", loc_r17.Address, ", ", loc_r17.City, ", ");
      }
    }

    function BulkPlantsComponent_ng_container_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "agm-marker", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("mouseOver", function BulkPlantsComponent_ng_container_3_Template_agm_marker_mouseOver_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](3);

          return _r18.open();
        })("mouseOut", function BulkPlantsComponent_ng_container_3_Template_agm_marker_mouseOut_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](3);

          return _r18.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "agm-info-window", 48, 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, BulkPlantsComponent_ng_container_3_span_9_Template, 2, 2, "span", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var loc_r17 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("latitude", loc_r17.Latitude)("longitude", loc_r17.Longitude)("agmFitBounds", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](loc_r17.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", loc_r17.Address);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", loc_r17.StateCode, " ");
      }
    }

    function BulkPlantsComponent_tr_31_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var row_r24 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.Address);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.City);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.StateCode);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.County);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.Latitude);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](row_r24.Longitude);
      }
    }

    function BulkPlantsComponent_ng_container_43_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_43_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.PickupForm.get("Name").errors.required);
      }
    }

    function BulkPlantsComponent_ng_container_49_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_49_label_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid (alphanumeric with comma spaces only) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_49_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_49_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, BulkPlantsComponent_ng_container_49_label_2_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.PickupForm.get("Address").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.PickupForm.get("Address").errors.pattern);
      }
    }

    function BulkPlantsComponent_ng_container_55_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_55_label_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid zipcode ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_55_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_55_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, BulkPlantsComponent_ng_container_55_label_2_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.PickupForm.get("ZipCode").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.PickupForm.get("ZipCode").errors.pattern);
      }
    }

    function BulkPlantsComponent_ng_container_61_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_61_label_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid (alphanumeric with comma spaces only) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_61_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_61_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, BulkPlantsComponent_ng_container_61_label_2_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.PickupForm.get("City").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.PickupForm.get("City").errors.pattern);
      }
    }

    function BulkPlantsComponent_option_69_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var st_r32 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", st_r32.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", st_r32.Name, " ");
      }
    }

    function BulkPlantsComponent_ng_container_70_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_70_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_70_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r7.PickupForm.get("StateId").errors.required);
      }
    }

    function BulkPlantsComponent_option_79_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ct_r34 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", ct_r34.Code);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ct_r34.Code, " ");
      }
    }

    function BulkPlantsComponent_ng_container_80_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_80_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_80_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r9.PickupForm.get("CountryCode").errors.required);
      }
    }

    function BulkPlantsComponent_div_81_option_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ct_r37 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", ct_r37.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ct_r37.Name, " ");
      }
    }

    function BulkPlantsComponent_div_81_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Country");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "select", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "option", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Select Country");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, BulkPlantsComponent_div_81_option_7_Template, 2, 2, "option", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r10.CountryGroupList);
      }
    }

    function BulkPlantsComponent_ng_container_87_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainer"](0);
      }
    }

    function BulkPlantsComponent_ng_container_93_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_93_label_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_93_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_93_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, BulkPlantsComponent_ng_container_93_label_2_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r12.PickupForm.get("Latitude").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r12.PickupForm.get("Latitude").errors.pattern);
      }
    }

    function BulkPlantsComponent_ng_container_99_label_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_99_label_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_ng_container_99_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, BulkPlantsComponent_ng_container_99_label_1_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, BulkPlantsComponent_ng_container_99_label_2_Template, 2, 0, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.PickupForm.get("Longitude").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.PickupForm.get("Longitude").errors.pattern);
      }
    }

    function BulkPlantsComponent_div_100_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function BulkPlantsComponent_div_107_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var BulkPlantsComponent = /*#__PURE__*/function () {
      function BulkPlantsComponent(locationSercice, fb, stateService, addresService) {
        _classCallCheck(this, BulkPlantsComponent);

        this.locationSercice = locationSercice;
        this.fb = fb;
        this.stateService = stateService;
        this.addresService = addresService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.locations = [];
        this.StateList = [];
        this.CountryList = [];
        this.CountryGroupList = [];
        this._loadingAddress = false;
        this.zoomLevel = 4;
        this.toogleMap = true;
        this.screenOptions = {
          position: 3
        };
        this.centerLocationLat = 47.1853106;
        this.centerLocationLog = -125.36955;
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
      }

      _createClass(BulkPlantsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this = this;

          this.PickupForm = this.initPickupForm(new _models_location__WEBPACK_IMPORTED_MODULE_3__["LocationDetailsModel"]());
          var countryId = this.getCountryFilter();
          this.setAddressValidator(countryId);
          this.stateService.getStates().subscribe(function (x) {
            return _this.StateList = x;
          });
          this.stateService.getCountries().subscribe(function (x) {
            return _this.CountryList = x;
          });
          this.stateService.getCountryGroup(_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"].CAR).subscribe(function (x) {
            return _this.CountryGroupList = x;
          });
          var exportColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Terminals Detail',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Terminals Detail',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }]
          };
          this.loadDataTable();
        }
      }, {
        key: "loadDataTable",
        value: function loadDataTable() {
          var _this2 = this;

          this.IsLoading = true;
          this.locationSercice.GetBulkPlants(this.getCountryFilter()).subscribe(function (data) {
            if (data != null) {
              _this2.locations = data;

              _this2.refreshDatatable();
            }

            _this2.IsLoading = false;
          });
        }
      }, {
        key: "refreshDatatable",
        value: function refreshDatatable() {
          this.dtElements.forEach(function (dtElement) {
            if (dtElement.dtInstance) {
              dtElement.dtInstance.then(function (dtInstance) {
                dtInstance.destroy();
              });
            }
          });
          this.dtTrigger.next();
        }
      }, {
        key: "getCountryFilter",
        value: function getCountryFilter() {
          return localStorage.getItem('countryFilterType') ? localStorage.getItem('countryFilterType') : localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1;
        }
      }, {
        key: "StatesListByCountry",
        get: function get() {
          var countryCode = this.PickupForm.get('CountryCode').value;

          if (countryCode && this.CountryList && this.CountryList.length > 0) {
            countryCode = countryCode == "US" ? "USA" : countryCode;
            var countryId = 0;
            var county = this.CountryList.find(function (c) {
              return c.Code == countryCode;
            });
            if (county && county.Id) countryId = county.Id;

            if (countryId == _models_location__WEBPACK_IMPORTED_MODULE_3__["Country"].CAR) {
              var countryGroupId = this.PickupForm.get("CountryGroupId").value;
              return this.StateList.filter(function (t) {
                return t.CountryId == countryId && (countryGroupId == 0 || t.CountryGroupId == countryGroupId);
              });
            } else {
              return this.StateList.filter(function (t) {
                return t.CountryId == countryId;
              });
            }
          }
        }
      }, {
        key: "initPickupForm",
        value: function initPickupForm(loc) {
          var _pForm = this.fb.group({
            Address: this.fb.control(loc.Address),
            City: this.fb.control(loc.City),
            StateId: this.fb.control(loc.StateId, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            StateCode: this.fb.control(loc.StateCode, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            CountryCode: this.fb.control(loc.CountryCode, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            CountryId: this.fb.control(loc.CountryId, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            ZipCode: this.fb.control(loc.ZipCode),
            County: this.fb.control(loc.County),
            Latitude: this.fb.control(loc.Latitude),
            Longitude: this.fb.control(loc.Longitude),
            Name: this.fb.control(loc.Name, [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required]),
            CountryGroupId: this.fb.control(loc.CountryGroupId)
          });

          return _pForm;
        }
      }, {
        key: "setAddressValidator",
        value: function setAddressValidator(countryId) {
          if (countryId == _models_location__WEBPACK_IMPORTED_MODULE_3__["Country"].CAR) {
            this.PickupForm.get('Address').clearValidators();
            this.PickupForm.get('Address').updateValueAndValidity();
            this.PickupForm.get('City').clearValidators();
            this.PickupForm.get('City').updateValueAndValidity();
            this.PickupForm.get('ZipCode').clearValidators();
            this.PickupForm.get('ZipCode').updateValueAndValidity(); //this.PickupForm.get('County').clearValidators();
            //this.PickupForm.get('County').updateValueAndValidity();

            this.PickupForm.get('Latitude').clearValidators();
            this.PickupForm.get('Latitude').updateValueAndValidity();
            this.PickupForm.get('Longitude').clearValidators();
            this.PickupForm.get('Longitude').updateValueAndValidity();
          } else {
            var validator = [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["Validators"].required];
            this.PickupForm.get('Address').setValidators(validator);
            this.PickupForm.get('Address').updateValueAndValidity();
            this.PickupForm.get('City').setValidators(validator);
            this.PickupForm.get('City').updateValueAndValidity();
            this.PickupForm.get('ZipCode').setValidators(validator);
            this.PickupForm.get('ZipCode').updateValueAndValidity(); //this.PickupForm.get('County').setValidators(validator);
            //this.PickupForm.get('County').updateValueAndValidity();

            this.PickupForm.get('Latitude').setValidators(validator);
            this.PickupForm.get('Latitude').updateValueAndValidity();
            this.PickupForm.get('Longitude').setValidators(validator);
            this.PickupForm.get('Longitude').updateValueAndValidity();
          }
        }
      }, {
        key: "getAddressByZip",
        value: function getAddressByZip(event) {
          var _this3 = this;

          var zipCode = event.target.value; //const regexUsa = new RegExp(this.regexUsaZip);
          //const regexCan = new RegExp(this.regexCanZip);

          if (zipCode.length > 2) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode).subscribe(function (data) {
              _this3._loadingAddress = true;

              var _address = _this3.PickupForm.get('Address');

              if (data != null && data != undefined && data.CountryCode != null) {
                _this3.setCountryCode(data);

                data.Address = _address.value;
                var countryGroupId = null;

                var state = _this3.StateList.find(function (x) {
                  return x.Code == data.StateCode;
                });

                var country = _this3.CountryList.find(function (x) {
                  return x.Code == data.CountryCode;
                });

                var countrygroup = new _statelist_service__WEBPACK_IMPORTED_MODULE_5__["DropdownItem"]();

                if (country && country.Id > 0) {
                  countryGroupId = 1;
                  countrygroup.Id = 1;
                } else {
                  countrygroup = _this3.CountryGroupList.find(function (x) {
                    return x.Code == data.CountryCode;
                  });
                  country = new _statelist_service__WEBPACK_IMPORTED_MODULE_5__["DropdownItem"]();
                  country.Id = 4;
                  country.Code = "CAR";
                }

                _this3.PickupForm.patchValue({
                  City: data.City,
                  StateId: state ? state.Id : null,
                  StateCode: data.StateCode,
                  CountryId: country.Id,
                  CountryCode: country.Code,
                  CountryGroupId: countrygroup.Id,
                  ZipCode: data.ZipCode,
                  County: data.CountyName,
                  Latitude: data.Latitude,
                  Longitude: data.Longitude
                });

                _this3.PickupForm.markAllAsTouched();

                _this3.PickupForm.markAsDirty();
              }

              _this3._loadingAddress = false;
            });
          }
        }
      }, {
        key: "setCountryCode",
        value: function setCountryCode(data) {
          if (data.CountryCode == 'US') {
            data.CountryCode = 'USA';
          } else if (data.CountryCode == 'CA') {
            data.CountryCode = 'CAN';
          }
        }
      }, {
        key: "setStateCode",
        value: function setStateCode(event) {
          this.PickupForm.get('StateCode').setValue(event.target.selectedOptions[0].text);
        }
      }, {
        key: "mapReady",
        value: function mapReady(map) {
          this.googleMap = map;
          this.setMapCenter();
        }
      }, {
        key: "setCenterMap",
        value: function setCenterMap($event) {
          if (!this.locations.length) {
            var selectedCountryId = this.getCountryFilter();
            this.centerLocationLat = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lat;
            this.centerLocationLog = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lng;

            if (this.googleMap) {
              this.googleMap.setCenter({
                lat: this.centerLocationLat,
                lng: this.centerLocationLog
              });
              this.googleMap.setZoom(4);
            }
          }
        }
      }, {
        key: "setMapCenter",
        value: function setMapCenter() {
          var selectedCountryId = this.getCountryFilter();
          this.centerLocationLat = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lat;
          this.centerLocationLog = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lng;

          if (this.googleMap && this.locations.length == 0 && this.locations.length == 0) {
            var bounds = new google.maps.LatLngBounds();
            bounds.extend(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
            this.googleMap.fitBounds(bounds);
            this.googleMap.setZoom(4);
          } else {
            var _bounds = new google.maps.LatLngBounds();

            this.locations.forEach(function (x) {
              _bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
            });
            this.googleMap.fitBounds(_bounds);
          }
        }
      }, {
        key: "savePickupLocation",
        value: function savePickupLocation() {
          var _this4 = this;

          this.locationSercice.PostBulkPlantLocation(this.PickupForm.value).subscribe(function (response) {
            if (response != null && response.StatusCode == 0) {
              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this4.closePickUpModal.nativeElement.click();

              _this4.loadDataTable();
            } else {
              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "clearPickUpform",
        value: function clearPickUpform() {
          this.PickupForm.reset();
        }
      }, {
        key: "countryChanged",
        value: function countryChanged() {
          var countryId = 1;
          var countryCode = this.PickupForm.get('CountryCode').value;

          if (countryCode) {
            if (countryCode == "CAN") {
              countryId = 2;
            } else if (countryCode == "CAR") {
              countryId = 4;
            }

            this.setAddressValidator(countryId);
            this.PickupForm.get('CountryId').setValue(countryId);
          }
        }
      }]);

      return BulkPlantsComponent;
    }();

    BulkPlantsComponent.ɵfac = function BulkPlantsComponent_Factory(t) {
      return new (t || BulkPlantsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_location_service__WEBPACK_IMPORTED_MODULE_7__["LocationService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_statelist_service__WEBPACK_IMPORTED_MODULE_5__["StatelistService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_address_service__WEBPACK_IMPORTED_MODULE_8__["AddressService"]));
    };

    BulkPlantsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: BulkPlantsComponent,
      selectors: [["app-bulk-plants"]],
      viewQuery: function BulkPlantsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.closePickUpModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      decls: 108,
      vars: 30,
      consts: [[1, "row", "mb10"], [1, "col-sm-12"], [3, "maxZoom", "fitBounds", "latitude", "zoom", "longitude", "fullscreenControl", "fullscreenControlOptions", "mapTypeControl", "boundsChange", "mapReady"], [4, "ngFor", "ngForOf"], [1, "col-md-12"], ["data-toggle", "modal", "data-target", "#pickup-location", 1, "btn", "btn-default", "btn-xs", "pull-left", 3, "click"], [1, "fas", "fa-map-marker-alt"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-bulk-plants", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "tName"], ["data-key", "tAddress"], ["data-key", "tCity"], ["data-key", "tStateCode"], ["data-key", "tCounty"], ["data-key", "tLatitude"], ["data-key", "tLongitude"], ["id", "pickup-location", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "mt10", "mb10"], [1, "row", "pr", 3, "formGroup"], [1, "col-sm-6"], [1, "form-group"], ["type", "text", "formControlName", "Name", "placeholder", "Bulk Plant Name", 1, "form-control"], [4, "ngIf"], ["type", "text", "formControlName", "Address", 1, "form-control"], ["type", "text", "formControlName", "ZipCode", 1, "form-control", 3, "input"], ["type", "text", "formControlName", "City", 1, "form-control"], ["formControlName", "StateId", "placeholder", "Select State", 1, "form-control", 3, "change"], [3, "value"], [3, "value", 4, "ngFor", "ngForOf"], ["type", "hidden", "formControlName", "StateCode"], ["formControlName", "CountryCode", "placeholder", "Select Country", 1, "form-control", 3, "change"], ["class", "col-sm-6", 4, "ngIf"], ["type", "text", "formControlName", "County", 1, "form-control"], ["type", "text", "formControlName", "Latitude", 1, "form-control"], ["type", "text", "formControlName", "Longitude", 1, "form-control"], ["class", "pa bg-white z-index5 loading-wrapper", 4, "ngIf"], [1, "text-right"], ["type", "button", "data-dismiss", "modal", "id", "btnPickupClose", 1, "btn"], ["closePickUpModal", ""], ["type", "button", 1, "btn", "btn-primary", 3, "disabled", "click"], ["class", "loader", 4, "ngIf"], [3, "latitude", "longitude", "agmFitBounds", "mouseOver", "mouseOut"], [3, "disableAutoPan"], ["infoWindow", ""], ["style", "color:red", 4, "ngIf"], [2, "color", "red"], ["formControlName", "CountryGroupId", "placeholder", "Select CountryGroup", 1, "form-control"], [1, "pa", "bg-white", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function BulkPlantsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "agm-map", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("boundsChange", function BulkPlantsComponent_Template_agm_map_boundsChange_2_listener($event) {
            return ctx.setCenterMap($event);
          })("mapReady", function BulkPlantsComponent_Template_agm_map_mapReady_2_listener($event) {
            return ctx.mapReady($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, BulkPlantsComponent_ng_container_3_Template, 11, 7, "ng-container", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function BulkPlantsComponent_Template_button_click_6_listener() {
            return ctx.clearPickUpform();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "i", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, " Add New Pick-Up Location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "County");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Latitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Longitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](31, BulkPlantsComponent_tr_31_Template, 15, 7, "tr", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Bulk Plant");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](42, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](43, BulkPlantsComponent_ng_container_43_Template, 2, 1, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](47, "Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](48, "input", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](49, BulkPlantsComponent_ng_container_49_Template, 3, 2, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "Zip");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function BulkPlantsComponent_Template_input_input_54_listener($event) {
            return ctx.getAddressByZip($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](55, BulkPlantsComponent_ng_container_55_Template, 3, 2, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](57, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](59, "City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](60, "input", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](61, BulkPlantsComponent_ng_container_61_Template, 3, 2, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](65, "State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](66, "select", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function BulkPlantsComponent_Template_select_change_66_listener($event) {
            return ctx.setStateCode($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "option", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](68, "Select State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](69, BulkPlantsComponent_option_69_Template, 2, 2, "option", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](70, BulkPlantsComponent_ng_container_70_Template, 2, 1, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](71, "input", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](74, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](75, "Country/Group");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "select", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function BulkPlantsComponent_Template_select_change_76_listener() {
            return ctx.countryChanged();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](77, "option", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](78, "Select Country");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](79, BulkPlantsComponent_option_79_Template, 2, 2, "option", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](80, BulkPlantsComponent_ng_container_80_Template, 2, 1, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](81, BulkPlantsComponent_div_81_Template, 8, 2, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](82, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](83, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](84, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](85, "County");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](86, "input", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](87, BulkPlantsComponent_ng_container_87_Template, 1, 0, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](88, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](89, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](91, "Latitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](92, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](93, BulkPlantsComponent_ng_container_93_Template, 3, 2, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](95, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](96, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](97, "Longitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](98, "input", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](99, BulkPlantsComponent_ng_container_99_Template, 3, 2, "ng-container", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](100, BulkPlantsComponent_div_100_Template, 2, 0, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](101, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "button", 43, 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](104, "Cancel");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](105, "button", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function BulkPlantsComponent_Template_button_click_105_listener() {
            return ctx.savePickupLocation();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](106, "Add");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](107, BulkPlantsComponent_div_107_Template, 5, 0, "div", 46);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("maxZoom", 16)("fitBounds", true)("latitude", ctx.centerLocationLat)("zoom", ctx.zoomLevel)("longitude", ctx.centerLocationLog)("fullscreenControl", true)("fullscreenControlOptions", ctx.screenOptions)("mapTypeControl", true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.locations);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.locations);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.PickupForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("Name").errors && (ctx.PickupForm.get("Name").touched || ctx.PickupForm.get("Name").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("Address").errors && (ctx.PickupForm.get("Address").touched || ctx.PickupForm.get("Address").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("ZipCode").errors && (ctx.PickupForm.get("ZipCode").touched || ctx.PickupForm.get("ZipCode").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("City").errors && (ctx.PickupForm.get("City").touched || ctx.PickupForm.get("City").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.StatesListByCountry);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("StateId").errors && (ctx.PickupForm.get("StateId").touched || ctx.PickupForm.get("StateId").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CountryList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("CountryCode").errors && (ctx.PickupForm.get("CountryCode").touched || ctx.PickupForm.get("CountryCode").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("CountryCode").value == "CAR");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("County").errors && (ctx.PickupForm.get("County").touched || ctx.PickupForm.get("County").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("Latitude").errors && (ctx.PickupForm.get("Latitude").touched || ctx.PickupForm.get("Latitude").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.PickupForm.get("Longitude").errors && (ctx.PickupForm.get("Longitude").touched || ctx.PickupForm.get("Longitude").dirty));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx._loadingAddress);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", !ctx.PickupForm.valid);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_agm_core__WEBPACK_IMPORTED_MODULE_9__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ɵangular_packages_forms_forms_x"], _agm_core__WEBPACK_IMPORTED_MODULE_9__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_9__["AgmFitBounds"], _agm_core__WEBPACK_IMPORTED_MODULE_9__["AgmInfoWindow"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xvY2F0aW9uL3BpY2t1cC1sb2NhdGlvbi9idWxrLXBsYW50cy9idWxrLXBsYW50cy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](BulkPlantsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-bulk-plants',
          templateUrl: './bulk-plants.component.html',
          styleUrls: ['./bulk-plants.component.css']
        }]
      }], function () {
        return [{
          type: _services_location_service__WEBPACK_IMPORTED_MODULE_7__["LocationService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormBuilder"]
        }, {
          type: _statelist_service__WEBPACK_IMPORTED_MODULE_5__["StatelistService"]
        }, {
          type: _address_service__WEBPACK_IMPORTED_MODULE_8__["AddressService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        closePickUpModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['closePickUpModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/location/pickup-location/pickup-location.component.ts": function srcAppLocationPickupLocationPickupLocationComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PickupLocationComponent", function () {
      return PickupLocationComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _models_location__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../models/location */
    "./src/app/location/models/location.ts");
    /* harmony import */


    var _terminals_terminals_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./terminals/terminals.component */
    "./src/app/location/pickup-location/terminals/terminals.component.ts");
    /* harmony import */


    var _bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./bulk-plants/bulk-plants.component */
    "./src/app/location/pickup-location/bulk-plants/bulk-plants.component.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function PickupLocationComponent_option_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var key_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", key_r3.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](key_r3.Name);
      }
    }

    function PickupLocationComponent_app_terminals_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-terminals");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function PickupLocationComponent_app_bulk_plants_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-bulk-plants");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var PickupLocationComponent = /*#__PURE__*/function () {
      function PickupLocationComponent() {
        _classCallCheck(this, PickupLocationComponent);

        this.CountryEnum = _models_location__WEBPACK_IMPORTED_MODULE_1__["Country"];
        this.CountryType = [];
      }

      _createClass(PickupLocationComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this5 = this;

          this.checkWindowSelection();
          this.CountryType = Object.keys(this.CountryEnum).filter(function (k) {
            return typeof _this5.CountryEnum[k] === "number";
          }).map(function (x) {
            return {
              Id: _this5.CountryEnum[x],
              Name: x,
              Code: ""
            };
          });
        }
      }, {
        key: "onCountryChange",
        value: function onCountryChange() {
          localStorage.setItem('countryFilterType', this.CountryFilter);

          if (this.locationViewType == 2) {
            this.bulkPLantComponent.loadDataTable();
          } else {
            this.terminalComponent.loadDataTable();
          }
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(type) {
          localStorage.setItem('locationViewType', type);
          this.locationViewType = type;
        }
      }, {
        key: "checkWindowSelection",
        value: function checkWindowSelection() {
          this.locationViewType = localStorage.getItem('locationViewType') ? localStorage.getItem('locationViewType') : 1;
          this.CountryFilter = localStorage.getItem('countryFilterType') ? localStorage.getItem('countryFilterType') : localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1;
        }
      }]);

      return PickupLocationComponent;
    }();

    PickupLocationComponent.ɵfac = function PickupLocationComponent_Factory(t) {
      return new (t || PickupLocationComponent)();
    };

    PickupLocationComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: PickupLocationComponent,
      selectors: [["app-pickup-location"]],
      viewQuery: function PickupLocationComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_terminals_terminals_component__WEBPACK_IMPORTED_MODULE_2__["TerminalsComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_3__["BulkPlantsComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.terminalComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.bulkPLantComponent = _t.first);
        }
      },
      decls: 15,
      vars: 10,
      consts: [[1, "row"], [1, "col-sm-12", "pull-left"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "form-group", "pull-right"], [1, "form-control", 3, "ngModel", "ngModelChange", "change"], [3, "value", 4, "ngFor", "ngForOf"], [4, "ngIf"], [3, "value"]],
      template: function PickupLocationComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function PickupLocationComponent_Template_label_click_5_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Terminals");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function PickupLocationComponent_Template_label_click_8_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Bulk Plants");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "select", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function PickupLocationComponent_Template_select_ngModelChange_11_listener($event) {
            return ctx.CountryFilter = $event;
          })("change", function PickupLocationComponent_Template_select_change_11_listener() {
            return ctx.onCountryChange();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, PickupLocationComponent_option_12_Template, 2, 2, "option", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, PickupLocationComponent_app_terminals_13_Template, 2, 0, "app-terminals", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, PickupLocationComponent_app_bulk_plants_14_Template, 2, 0, "app-bulk-plants", 10);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 1)("checked", ctx.locationViewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 2)("checked", ctx.locationViewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CountryFilter);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CountryType);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.locationViewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.locationViewType == 2);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgModel"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ɵangular_packages_forms_forms_x"], _terminals_terminals_component__WEBPACK_IMPORTED_MODULE_2__["TerminalsComponent"], _bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_3__["BulkPlantsComponent"]],
      styles: ["agm-map {\r\n    height: 60vh;\r\n    border-radius: 10px;\r\n}\r\n      .agm-map-container-inner {\r\n        border-radius: 10px;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbG9jYXRpb24vcGlja3VwLWxvY2F0aW9uL3BpY2t1cC1sb2NhdGlvbi5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtJQUNaLG1CQUFtQjtBQUN2QjtJQUNJO1FBQ0ksbUJBQW1CO0lBQ3ZCIiwiZmlsZSI6InNyYy9hcHAvbG9jYXRpb24vcGlja3VwLWxvY2F0aW9uL3BpY2t1cC1sb2NhdGlvbi5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIGFnbS1tYXAge1xyXG4gICAgaGVpZ2h0OiA2MHZoO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxufVxyXG4gICAgOjpuZy1kZWVwIC5hZ20tbWFwLWNvbnRhaW5lci1pbm5lciB7XHJcbiAgICAgICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuICAgIH1cclxuIl19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](PickupLocationComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-pickup-location',
          templateUrl: './pickup-location.component.html',
          styleUrls: ['./pickup-location.component.css']
        }]
      }], function () {
        return [];
      }, {
        terminalComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_terminals_terminals_component__WEBPACK_IMPORTED_MODULE_2__["TerminalsComponent"]]
        }],
        bulkPLantComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_bulk_plants_bulk_plants_component__WEBPACK_IMPORTED_MODULE_3__["BulkPlantsComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/location/pickup-location/terminals/terminals.component.ts": function srcAppLocationPickupLocationTerminalsTerminalsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TerminalsComponent", function () {
      return TerminalsComponent;
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


    var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _models_location__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../../models/location */
    "./src/app/location/models/location.ts");
    /* harmony import */


    var _services_location_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../services/location.service */
    "./src/app/location/services/location.service.ts");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function TerminalsComponent_ng_container_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "agm-marker", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("mouseOver", function TerminalsComponent_ng_container_3_Template_agm_marker_mouseOver_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](3);

          return _r3.open();
        })("mouseOut", function TerminalsComponent_ng_container_3_Template_agm_marker_mouseOut_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](3);

          return _r3.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "agm-info-window", 17, 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var loc_r2 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("latitude", loc_r2.Latitude)("longitude", loc_r2.Longitude)("agmFitBounds", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", loc_r2.Name, " ");
      }
    }

    function TerminalsComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var TerminalsComponent = /*#__PURE__*/function () {
      function TerminalsComponent(locationService) {
        _classCallCheck(this, TerminalsComponent);

        this.locationService = locationService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.terminals = [];
        this.mapTerminals = [];
        this.zoomLevel = 4;
        this.toogleMap = true;
        this.screenOptions = {
          position: 3
        };
        this.centerLocationLat = 47.1853106;
        this.centerLocationLog = -125.36955;
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
      }

      _createClass(TerminalsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this6 = this;

          var exportColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 5,
            lengthMenu: [[5, 10, 25, 50], [5, 10, 25, 50]],
            serverSide: true,
            processing: true,
            ajax: function ajax(dataTablesParameters, callback) {
              var requestModel = dataTablesParameters;
              requestModel['CountryId'] = _this6.getCountryFilter();

              _this6.locationService.getTerminals(requestModel).subscribe(function (resp) {
                _this6.terminals = resp.data;

                _this6.SetMapTerminals();

                callback({
                  recordsTotal: resp.recordsTotal,
                  recordsFiltered: resp.recordsFiltered,
                  data: resp.data
                });
              });
            },
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Terminals Detail',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Terminals Detail',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            columns: [{
              title: 'Name',
              data: 'Name',
              name: 'Name'
            }, {
              title: 'Abbreviation',
              data: 'Abbreviation',
              name: 'Abbreviation'
            }, {
              title: 'Control Number',
              data: 'ControlNumber',
              name: 'ControlNumber'
            }, {
              title: 'Address',
              data: 'Address',
              name: 'Address'
            }, {
              title: 'City',
              data: 'City',
              name: 'City'
            }, {
              title: 'State',
              data: 'StateCode',
              name: 'StateCode'
            }]
          };
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.dtTrigger.next();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          // Do not forget to unsubscribe the event
          this.dtTrigger.unsubscribe();
        }
      }, {
        key: "SetMapTerminals",
        value: function SetMapTerminals() {
          this.mapTerminals = this.terminals.filter(function (t) {
            return t.Latitude != 0 && t.Longitude != 0;
          });
        }
      }, {
        key: "loadDataTable",
        value: function loadDataTable() {
          this.refreshDatatable();
        }
      }, {
        key: "refreshDatatable",
        value: function refreshDatatable() {
          if (this.dtElement.dtInstance) {
            this.dtElement.dtInstance.then(function (dtInstance) {
              dtInstance.draw();
            });
          }
        }
      }, {
        key: "getCountryFilter",
        value: function getCountryFilter() {
          return localStorage.getItem('countryFilterType') ? localStorage.getItem('countryFilterType') : localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1;
        }
      }, {
        key: "mapReady",
        value: function mapReady(map) {
          this.googleMap = map;
          this.setMapCenter();
        }
      }, {
        key: "setCenterMap",
        value: function setCenterMap($event) {
          if (!this.mapTerminals.length) {
            var selectedCountryId = this.getCountryFilter();
            this.centerLocationLat = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lat;
            this.centerLocationLog = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lng;

            if (this.googleMap) {
              this.googleMap.setCenter({
                lat: this.centerLocationLat,
                lng: this.centerLocationLog
              });
              this.googleMap.setZoom(4);
            }
          }
        }
      }, {
        key: "setMapCenter",
        value: function setMapCenter() {
          var selectedCountryId = this.getCountryFilter();
          this.centerLocationLat = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lat;
          this.centerLocationLog = this.CountryCentre[_models_location__WEBPACK_IMPORTED_MODULE_3__["Country"][selectedCountryId]].lng;

          if (this.googleMap && this.terminals.length == 0 && this.terminals.length == 0) {
            var bounds = new google.maps.LatLngBounds();
            bounds.extend(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
            this.googleMap.fitBounds(bounds);
            this.googleMap.setZoom(4);
          } else {
            var _bounds2 = new google.maps.LatLngBounds();

            this.terminals.forEach(function (x) {
              _bounds2.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
            });
            this.googleMap.fitBounds(_bounds2);
          }
        }
      }]);

      return TerminalsComponent;
    }();

    TerminalsComponent.ɵfac = function TerminalsComponent_Factory(t) {
      return new (t || TerminalsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_location_service__WEBPACK_IMPORTED_MODULE_4__["LocationService"]));
    };

    TerminalsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: TerminalsComponent,
      selectors: [["app-terminals"]],
      viewQuery: function TerminalsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElement = _t.first);
        }
      },
      decls: 24,
      vars: 12,
      consts: [[1, "row", "mb10"], [1, "col-sm-12"], [3, "maxZoom", "fitBounds", "latitude", "zoom", "longitude", "fullscreenControl", "fullscreenControlOptions", "mapTypeControl", "boundsChange", "mapReady"], [4, "ngFor", "ngForOf"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-terminals", "datatable", "", 1, "table", "table-bordered", "table-hover", "serverside-table", 3, "dtOptions", "dtTrigger"], ["data-key", "tName"], ["data-key", "tAbbreviation"], ["data-key", "tControlNumber"], ["data-key", "tAddress"], ["data-key", "tCity"], ["data-key", "tStateCode"], ["class", "loader", 4, "ngIf"], [3, "latitude", "longitude", "agmFitBounds", "mouseOver", "mouseOut"], [3, "disableAutoPan"], ["infoWindow", ""], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function TerminalsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "agm-map", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("boundsChange", function TerminalsComponent_Template_agm_map_boundsChange_2_listener($event) {
            return ctx.setCenterMap($event);
          })("mapReady", function TerminalsComponent_Template_agm_map_mapReady_2_listener($event) {
            return ctx.mapReady($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, TerminalsComponent_ng_container_3_Template, 6, 5, "ng-container", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "table", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Abbreviation");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "Control Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, TerminalsComponent_div_23_Template, 5, 0, "div", 15);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("maxZoom", 16)("fitBounds", true)("latitude", ctx.centerLocationLat)("zoom", ctx.zoomLevel)("longitude", ctx.centerLocationLog)("fullscreenControl", true)("fullscreenControlOptions", ctx.screenOptions)("mapTypeControl", true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.mapTerminals);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_agm_core__WEBPACK_IMPORTED_MODULE_5__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgForOf"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgIf"], _agm_core__WEBPACK_IMPORTED_MODULE_5__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_5__["AgmFitBounds"], _agm_core__WEBPACK_IMPORTED_MODULE_5__["AgmInfoWindow"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xvY2F0aW9uL3BpY2t1cC1sb2NhdGlvbi90ZXJtaW5hbHMvdGVybWluYWxzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TerminalsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-terminals',
          templateUrl: './terminals.component.html',
          styleUrls: ['./terminals.component.css']
        }]
      }], function () {
        return [{
          type: _services_location_service__WEBPACK_IMPORTED_MODULE_4__["LocationService"]
        }];
      }, {
        dtElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=location-lazy-loading-location-module-es5.js.map