function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["trailer-trailer-module"], {
  /***/
  "./src/app/trailer/trailer.module.ts": function srcAppTrailerTrailerModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerModule", function () {
      return TrailerModule;
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


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _view_trailer_view_trailer_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./view-trailer/view-trailer.component */
    "./src/app/trailer/view-trailer/view-trailer.component.ts");

    var routeTrailer = [{
      path: '',
      component: _view_trailer_view_trailer_component__WEBPACK_IMPORTED_MODULE_5__["ViewTrailerComponent"]
    }];

    var TrailerModule = function TrailerModule() {
      _classCallCheck(this, TrailerModule);
    };

    TrailerModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({
      type: TrailerModule
    });
    TrailerModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({
      factory: function TrailerModule_Factory(t) {
        return new (t || TrailerModule)();
      },
      imports: [[_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeTrailer)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](TrailerModule, {
        declarations: [_view_trailer_view_trailer_component__WEBPACK_IMPORTED_MODULE_5__["ViewTrailerComponent"]],
        imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](TrailerModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_view_trailer_view_trailer_component__WEBPACK_IMPORTED_MODULE_5__["ViewTrailerComponent"]],
          imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeTrailer)]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/trailer/view-trailer/view-trailer.component.ts": function srcAppTrailerViewTrailerViewTrailerComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewTrailerComponent", function () {
      return ViewTrailerComponent;
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


    var _declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_shared_components_create_trailer_create_trailer_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/shared-components/create-trailer/create-trailer.component */
    "./src/app/shared-components/create-trailer/create-trailer.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    function ViewTrailerComponent_tr_44_span_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.Name);
      }
    }

    function ViewTrailerComponent_tr_44_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.Owner);
      }
    }

    function ViewTrailerComponent_tr_44_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.LicencePlate);
      }
    }

    function ViewTrailerComponent_tr_44_span_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_span_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.ExpirationDate);
      }
    }

    function ViewTrailerComponent_tr_44_span_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.FuelCapacity);
      }
    }

    function ViewTrailerComponent_tr_44_span_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_span_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.ContractNumber);
      }
    }

    function ViewTrailerComponent_tr_44_span_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_div_24_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var comp_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](" : " + comp_r22.Capacity);
      }
    }

    function ViewTrailerComponent_tr_44_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, ViewTrailerComponent_tr_44_div_24_span_3_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var comp_r22 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](comp_r22.CompartmentId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", comp_r22.Capacity > 0);
      }
    }

    function ViewTrailerComponent_tr_44_div_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function ViewTrailerComponent_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        var _r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, ViewTrailerComponent_tr_44_span_4_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, ViewTrailerComponent_tr_44_span_5_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](7, ViewTrailerComponent_tr_44_span_7_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, ViewTrailerComponent_tr_44_span_8_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, ViewTrailerComponent_tr_44_span_12_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, ViewTrailerComponent_tr_44_span_13_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, ViewTrailerComponent_tr_44_span_15_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, ViewTrailerComponent_tr_44_span_16_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, ViewTrailerComponent_tr_44_span_18_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, ViewTrailerComponent_tr_44_span_19_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](21, ViewTrailerComponent_tr_44_span_21_Template, 2, 1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](22, ViewTrailerComponent_tr_44_span_22_Template, 2, 0, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](24, ViewTrailerComponent_tr_44_div_24_Template, 5, 2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](25, ViewTrailerComponent_tr_44_div_25_Template, 2, 0, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "td", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "button", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function ViewTrailerComponent_tr_44_Template_button_click_31_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26);

          var truck_r1 = ctx.$implicit;

          var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          ctx_r25.createTrailer("Edit Trailer");
          return ctx_r25.editTruck(truck_r1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](32, "i", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "button", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("confirm", function ViewTrailerComponent_tr_44_Template_button_confirm_33_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26);

          var truck_r1 = ctx.$implicit;

          var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r27.deleteTruck(truck_r1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](34, "i", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var truck_r1 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](truck_r1.TruckId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.Name != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.Name == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.Owner != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.Owner == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", ctx_r0.LicenceRequirements[truck_r1.LicenceRequirement], " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.LicencePlate != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.LicencePlate == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.ExpirationDate != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.ExpirationDate == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.FuelCapacity > 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.FuelCapacity == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.ContractNumber != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.ContractNumber == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", truck_r1.Compartments);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", truck_r1.Compartments.length == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r0.TrailerType[truck_r1.TrailerType]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r0.TruckStatus[truck_r1.Status]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("popoverTitle", ctx_r0.popoverTitle)("confirmText", ctx_r0.confirmButtonText)("cancelText", ctx_r0.cancelButtonText);
      }
    }

    var ViewTrailerComponent = /*#__PURE__*/function () {
      function ViewTrailerComponent(carrierService) {
        _classCallCheck(this, ViewTrailerComponent);

        this.carrierService = carrierService;
        this.TruckStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["TruckStatus"];
        this.TrailerType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["TrailerType"];
        this.LicenceRequirements = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["LicenceRequirement"];
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
      }

      _createClass(ViewTrailerComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.ModalText = 'Create Trailer';
          var exportColumns = {
            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
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
              title: 'Trailer Details',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Trailer Details',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
          this.getAllTrucks();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtTrigger.unsubscribe();
        }
      }, {
        key: "editTruck",
        value: function editTruck(truck) {
          if (this.TrailerComponent != undefined) {
            this.TrailerComponent.loadTruckDetail(truck);
          }
        }
      }, {
        key: "deleteTruck",
        value: function deleteTruck(truck) {
          var _this = this;

          this.carrierService.postDeleteTruck(truck).subscribe(function (data) {
            if (data.StatusCode == 0) {
              _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);

              _this.loadTruckDetails();
            }
          });
        }
      }, {
        key: "createTrailer",
        value: function createTrailer(header) {
          this.ModalText = header; //this.IsCreateTruck = true;
        }
      }, {
        key: "getAllTrucks",
        value: function getAllTrucks() {
          var _this2 = this;

          this.carrierService.getAllTrucks().subscribe(function (data) {
            _this2.Trucks = data;

            _this2.dtTrigger.next();
          });
        }
      }, {
        key: "clearPanelData",
        value: function clearPanelData() {
          if (this.TrailerComponent != undefined) {
            this.TrailerComponent.clearTrailerForm();
          }
        }
      }, {
        key: "loadTruckDetails",
        value: function loadTruckDetails() {
          this.getAllTrucks();
          $("#truck-datatable").DataTable().clear().destroy();
        }
      }]);

      return ViewTrailerComponent;
    }();

    ViewTrailerComponent.??fac = function ViewTrailerComponent_Factory(t) {
      return new (t || ViewTrailerComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]));
    };

    ViewTrailerComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: ViewTrailerComponent,
      selectors: [["app-view-trailer"]],
      viewQuery: function ViewTrailerComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](src_app_shared_components_create_trailer_create_trailer_component__WEBPACK_IMPORTED_MODULE_3__["CreateTrailerComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.TrailerComponent = _t.first);
        }
      },
      decls: 54,
      vars: 4,
      consts: [[1, "row"], [1, "col-12"], ["src", "src/assets/trailer.png", "width", "40", 1, "pull-left", "mr10"], [1, "pt0", "pull-left"], ["onclick", "slidePanel('#create-trailer','50%')", 1, "fs18", "pull-left", "ml10", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt4", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "truck-details-grid", 1, "table-responsive"], ["id", "truck-datatable", "data-gridname", "15", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngFor", "ngForOf"], ["id", "create-trailer", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel", 3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [3, "onSubmitGroupData"], [4, "ngIf"], [1, "text-center"], ["type", "button", "onclick", "slidePanel('#create-trailer','30%')", 1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-edit", "fs16"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", "ml10", 3, "popoverTitle", "confirmText", "cancelText", "confirm"], [1, "fas", "fa-trash-alt", "color-maroon"]],
      template: function ViewTrailerComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "img", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "h4", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Trailer Summary");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function ViewTrailerComponent_Template_a_click_6_listener() {
            return ctx.createTrailer("Create Trailer");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "Add Trailer");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "Trailer Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "Owner");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Licence Requirement");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "Licence Plate");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Expiration Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "Fuel Capacity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "Contract Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, "Compartments");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38, "Trailer Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](40, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](42, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](43, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](44, ViewTrailerComponent_tr_44_Template, 35, 21, "tr", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "a", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function ViewTrailerComponent_Template_a_click_48_listener() {
            return ctx.clearPanelData();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](49, "i", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "h3", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](53, "app-create-trailer", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onSubmitGroupData", function ViewTrailerComponent_Template_app_create_trailer_onSubmitGroupData_53_listener() {
            return ctx.loadTruckDetails();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.Trucks);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.ModalText);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], src_app_shared_components_create_trailer_create_trailer_component__WEBPACK_IMPORTED_MODULE_3__["CreateTrailerComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__["??c"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3RyYWlsZXIvdmlldy10cmFpbGVyL3ZpZXctdHJhaWxlci5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](ViewTrailerComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-view-trailer',
          templateUrl: './view-trailer.component.html',
          styleUrls: ['./view-trailer.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]
        }];
      }, {
        TrailerComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_create_trailer_create_trailer_component__WEBPACK_IMPORTED_MODULE_3__["CreateTrailerComponent"]]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=trailer-trailer-module-es5.js.map