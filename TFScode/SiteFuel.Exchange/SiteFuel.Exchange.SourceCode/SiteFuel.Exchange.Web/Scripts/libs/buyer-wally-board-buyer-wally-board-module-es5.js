function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["buyer-wally-board-buyer-wally-board-module"], {
  /***/
  "./src/app/buyer-wally-board/buyer-locations.component.ts": function srcAppBuyerWallyBoardBuyerLocationsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "BuyerLocationsComponent", function () {
      return BuyerLocationsComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_2__);
    /* harmony import */


    var _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./Models/BuyerWallyBoard */
    "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./sales-data/location-view.component */
    "./src/app/buyer-wally-board/sales-data/location-view.component.ts");
    /* harmony import */


    var _app_constants__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
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


    var _agm_core__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _shared_components_demand_capture_chart_demand_capture_chart_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ../shared-components/demand-capture-chart/demand-capture-chart.component */
    "./src/app/shared-components/demand-capture-chart/demand-capture-chart.component.ts");
    /* harmony import */


    var ng2_charts__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ng2-charts */
    "./node_modules/ng2-charts/__ivy_ngcc__/fesm2015/ng2-charts.js");

    function BuyerLocationsComponent_div_7_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "ng-multiselect-dropdown", 43, 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onSelect", function BuyerLocationsComponent_div_7_ng_template_3_Template_ng_multiselect_dropdown_onSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r17);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r16.SaveFilters();
        })("onDeSelect", function BuyerLocationsComponent_div_7_ng_template_3_Template_ng_multiselect_dropdown_onDeSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r17);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r18.SaveFilters();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Carriers")("formControl", ctx_r14.FilterForm.controls["SelectedCarrierList"])("settings", ctx_r14.multiDropdownSettings)("data", ctx_r14.carrierList);
      }
    }

    function BuyerLocationsComponent_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, BuyerLocationsComponent_div_7_ng_template_3_Template, 3, 4, "ng-template", null, 39, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r13)("autoClose", "outside");
      }
    }

    function BuyerLocationsComponent_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r2.filterCount);
      }
    }

    function BuyerLocationsComponent_div_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
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

    function BuyerLocationsComponent_ng_container_47_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "agm-marker", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("mouseOver", function BuyerLocationsComponent_ng_container_47_Template_agm_marker_mouseOver_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r22);

          var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](3);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r21.mouseHoverMarker(_r20, $event);
        })("mouseOut", function BuyerLocationsComponent_ng_container_47_Template_agm_marker_mouseOut_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r22);

          var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](3);

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r23.mouseHoveOutMarker(_r20, $event);
        })("markerClick", function BuyerLocationsComponent_ng_container_47_Template_agm_marker_markerClick_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r22);

          var jobLocation_r19 = ctx.$implicit;

          var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r24.onInfoViewClick(jobLocation_r19);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "agm-info-window", 48, 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var jobLocation_r19 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("latitude", jobLocation_r19.Latitude)("longitude", jobLocation_r19.Longitude)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction2"](6, _c1, jobLocation_r19.iconUrl, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction0"](5, _c0)));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](jobLocation_r19.JobName);
      }
    }

    function BuyerLocationsComponent_div_48_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, " No image available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_div_48_img_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "img", 90);
      }

      if (rf & 2) {
        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("src", ctx_r26.opendedJobDetails.SiteImageFilePath, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);
      }
    }

    function BuyerLocationsComponent_div_48_span_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "OPEN");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_div_48_div_37_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "span", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "span", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "OPEN");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var day_r32 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", day_r32, " ");
      }
    }

    function BuyerLocationsComponent_div_48_div_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " No Days Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_div_48_div_57_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainer"](0);
      }
    }

    function BuyerLocationsComponent_div_48_div_57_Template(rf, ctx) {
      if (rf & 1) {
        var _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "a", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_div_57_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r35);

          var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r34.closeAssetsClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "i", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, " Back");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_div_57_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r35);

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r36.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, BuyerLocationsComponent_div_48_div_57_ng_container_9_Template, 1, 0, "ng-container", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngTemplateOutlet", _r9)("ngTemplateOutletContext", ctx_r30.assetDetails);
      }
    }

    function BuyerLocationsComponent_div_48_div_58_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_div_48_div_58_app_demand_capture_chart_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-demand-capture-chart", 106);
      }

      if (rf & 2) {
        var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("data", ctx_r38.demandChartData);
      }
    }

    function BuyerLocationsComponent_div_48_div_58_ng_template_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " No Data Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_div_48_div_58_Template(rf, ctx) {
      if (rf & 1) {
        var _r42 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, BuyerLocationsComponent_div_48_div_58_div_1_Template, 2, 0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "a", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_div_58_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r42);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r41.closeChartsClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "i", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, " Back");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_div_58_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r42);

          var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r43.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, BuyerLocationsComponent_div_48_div_58_app_demand_capture_chart_12_Template, 1, 1, "app-demand-capture-chart", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](13, "async");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](14, BuyerLocationsComponent_div_48_div_58_ng_template_14_Template, 2, 0, "ng-template", null, 105, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](15);

        var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r31.isLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](13, 3, ctx_r31.isChartDataExistSubject))("ngIfElse", _r39);
      }
    }

    function BuyerLocationsComponent_div_48_Template(rf, ctx) {
      if (rf & 1) {
        var _r45 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, BuyerLocationsComponent_div_48_div_4_Template, 3, 0, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r45);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r44.closeViewClicked();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, BuyerLocationsComponent_div_48_img_9_Template, 1, 1, "img", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](15, "i", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](19, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "span", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](23, "i", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "a", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "span", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](31, "Site Availability:");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](32, BuyerLocationsComponent_div_48_span_32_Template, 2, 0, "span", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "a", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](34, "i", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "div", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](36, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](37, BuyerLocationsComponent_div_48_div_37_Template, 5, 1, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](38, BuyerLocationsComponent_div_48_div_38_Template, 2, 0, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "label", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](41, "Site Instruction: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "span", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "div", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "label", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](47, "Contact(s):");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "p", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](51, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](53, "a", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_Template_a_click_53_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r45);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r46.onAssetsViewClick(ctx_r46.opendedJobDetails.jobAssetDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](55, "a", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_div_48_Template_a_click_55_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r45);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r47.onChartsViewClick(ctx_r47.opendedJobDetails.jobAssetDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](56, "Demand Capture Trend ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](57, BuyerLocationsComponent_div_48_div_57_Template, 10, 2, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](58, BuyerLocationsComponent_div_48_div_58_Template, 16, 5, "div", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r5.opendedJobDetails.SiteImageFilePath);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.opendedJobDetails.SiteImageFilePath);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", ctx_r5.opendedJobDetails.JobName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate4"](" ", ctx_r5.opendedJobDetails.Address, ", ", ctx_r5.opendedJobDetails.City, ", ", ctx_r5.opendedJobDetails.State, ", ", ctx_r5.opendedJobDetails.ZipCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.opendedJobDetails.SiteAvailabilityTotalDays);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r5.opendedJobDetails.SiteAvailabilityArray);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r5.opendedJobDetails.SiteAvailabilityArray.length);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", ctx_r5.opendedJobDetails.SiteInstructions, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r5.opendedJobDetails.ContactPersonName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", ctx_r5.opendedJobDetails.TotalCount, " Assets/Tanks ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.clickAssetsPanel);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.clickChartsPanel);
      }
    }

    function BuyerLocationsComponent_ng_template_53_div_9_ng_container_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainer"](0);
      }
    }

    function BuyerLocationsComponent_ng_template_53_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, BuyerLocationsComponent_ng_template_53_div_9_ng_container_1_Template, 1, 0, "ng-container", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngTemplateOutlet", _r9)("ngTemplateOutletContext", ctx_r49.assetDetails);
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

    function BuyerLocationsComponent_ng_template_53_Template(rf, ctx) {
      if (rf & 1) {
        var _r52 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "h4", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, " Asset/Tank Details ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "a", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_ng_template_53_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r52);

          var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r51.modalClose();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "i", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, BuyerLocationsComponent_ng_template_53_div_9_Template, 2, 2, "div", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r48 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](3, _c2, modalDetails_r48.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](5, _c3, modalDetails_r48.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", modalDetails_r48.display === "block");
      }
    }

    function BuyerLocationsComponent_ng_container_55_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainer"](0);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    var _c4 = function _c4(a0) {
      return {
        "active": a0
      };
    };

    function BuyerLocationsComponent_ng_template_56_ng_container_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r66 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "li", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "a", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_ng_template_56_ng_container_3_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r66);

          var indx_r64 = ctx.index;

          var ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r65.assetTabClicked(indx_r64);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var asset_r63 = ctx.$implicit;
        var indx_r64 = ctx.index;

        var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](3, _c4, ctx_r55.assetDetails.assetIndex === indx_r64));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate2"](" ", asset_r63.AssetType === 2 ? "Tank" : "Asset", " (", indx_r64 + 1, ") ");
      }
    }

    function BuyerLocationsComponent_ng_template_56_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().assetIndex;

        var ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate2"](" \xA0( ", ctx_r56.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r56.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankName, "-", ctx_r56.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r56.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankNumber, " ) ");
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_13_button_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r70 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "button", 134);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_ng_template_56_div_13_button_1_Template_button_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r70);

          var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2).assetIndex;

          var ctx_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r69.openDRPanel(ctx_r69.clickedAssetsDetails[assetNumber_r53]);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Create DR ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, BuyerLocationsComponent_ng_template_56_div_13_button_1_Template, 2, 0, "button", 133);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().assetIndex;

        var ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r57.clickedAssetsDetails[assetNumber_r53].AssetType === 2);
      }
    }

    function BuyerLocationsComponent_ng_template_56_a_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "a", 135);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 136);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, "Download Dip Chart ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().assetIndex;

        var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????propertyInterpolate"]("href", ctx_r58.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r58.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankChartPath, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_17_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r74.UoM);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "table", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Product Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](13, "Tank Capacity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](17, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, BuyerLocationsComponent_ng_template_56_div_17_span_18_Template, 2, 1, "span", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().assetIndex;

        var ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r59.clickedAssetsDetails[assetNumber_r53].ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](17, 3, ctx_r59.clickedAssetsDetails[assetNumber_r53].FuelCapacity), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r59.clickedAssetsDetails[assetNumber_r53].FuelCapacity);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_18_span_40_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r76.UoM);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_18_span_51_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r77.UoM);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_18_span_62_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r78.UoM);
      }
    }

    var _c5 = function _c5(a0) {
      return {
        "height.px": a0
      };
    };

    function BuyerLocationsComponent_ng_template_56_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 140);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "div", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](8, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 143);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](11, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "table", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "td", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](17, "Storage ID ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](23, "Tank Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](29, "Product Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](34, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](35, "Tank Capacity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](36, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](39, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](40, BuyerLocationsComponent_ng_template_56_div_18_span_40_Template, 2, 1, "span", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](43, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](44, "Min Fill");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](50, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](51, BuyerLocationsComponent_ng_template_56_div_18_span_51_Template, 2, 1, "span", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](53, "td", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](54, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](55, "Max Fill");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](56, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](58, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](59, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](61, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](62, BuyerLocationsComponent_ng_template_56_div_18_span_62_Template, 2, 1, "span", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().assetIndex;

        var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](28, _c5, ctx_r60.selectedTankHeight.ShouldBeEmptyPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](30, _c5, ctx_r60.selectedTankHeight.ShouldBeFilledPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind2"](8, 16, ctx_r60.selectedTankHeight.sbf_percent, "1.0-2"), "% ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](32, _c5, ctx_r60.selectedTankHeight.CurrentInventoryPercent || 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind2"](11, 19, ctx_r60.selectedTankHeight.ci_percent, "1.0-2"), "% ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r60.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r60.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].StorageId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r60.clickedAssetsDetails[assetNumber_r53].TankTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r60.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r60.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TfxProductTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](39, 22, ctx_r60.clickedAssetsDetails[assetNumber_r53].FuelCapacity), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r60.clickedAssetsDetails[assetNumber_r53].FuelCapacity);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("\xA0(", ctx_r60.selectedTankMinMax.MinFillPercent, "%)");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](50, 24, ctx_r60.selectedTankMinMax.MinFill), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r60.clickedAssetsDetails[assetNumber_r53].MinFill);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("\xA0(", ctx_r60.selectedTankMinMax.MaxFillPercent, "%)");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](61, 26, ctx_r60.selectedTankMinMax.MaxFill), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r60.clickedAssetsDetails[assetNumber_r53].MaxFill);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_19_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " NA ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_19_ng_template_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](1, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r82 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](1, 2, ctx_r82.latestReading == null ? null : ctx_r82.latestReading.NetVolume), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r82.UoM);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "table", 145);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Last Reading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8, "Ullage (");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, ")");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](13, "Last Reading Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, BuyerLocationsComponent_ng_template_56_div_19_div_17_Template, 2, 0, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, BuyerLocationsComponent_ng_template_56_div_19_ng_template_18_Template, 4, 4, "ng-template", null, 147, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](23, "number");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](27, "date");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r81 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](19);

        var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r61.UoM);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx_r61.latestReading == null ? null : ctx_r61.latestReading.NetVolume) == 0 - 1 || (ctx_r61.latestReading == null ? null : ctx_r61.latestReading.NetVolume) === undefined)("ngIfElse", _r81);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind1"](23, 5, ctx_r61.latestReading == null ? null : ctx_r61.latestReading.Ullage) || "NA");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind3"](27, 7, ctx_r61.latestReading == null ? null : ctx_r61.latestReading.CaptureTimeString, "MM/dd/yyyy, hh:mm a", "UTC") || "NA", " ");
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_20_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "canvas", 152);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("chartType", "line")("datasets", ctx_r83.chartData)("options", ctx_r83.chartOptions)("labels", ctx_r83.chartLabels)("legend", true);
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_20_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " No Data Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function BuyerLocationsComponent_ng_template_56_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 148);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "p", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "label", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "Dip test value trend : ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, BuyerLocationsComponent_ng_template_56_div_20_div_4_Template, 2, 5, "div", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, BuyerLocationsComponent_ng_template_56_div_20_div_5_Template, 2, 0, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r62.chartData.length);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r62.chartData.length && !ctx_r62.isLoading);
      }
    }

    function BuyerLocationsComponent_ng_template_56_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, BuyerLocationsComponent_ng_template_56_div_1_Template, 2, 0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "ul", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, BuyerLocationsComponent_ng_template_56_ng_container_3_Template, 4, 5, "ng-container", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "div", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "span", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, BuyerLocationsComponent_ng_template_56_span_12_Template, 2, 2, "span", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, BuyerLocationsComponent_ng_template_56_div_13_Template, 2, 1, "div", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, BuyerLocationsComponent_ng_template_56_a_16_Template, 3, 1, "a", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, BuyerLocationsComponent_ng_template_56_div_17_Template, 19, 5, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, BuyerLocationsComponent_ng_template_56_div_18_Template, 63, 34, "div", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, BuyerLocationsComponent_ng_template_56_div_19_Template, 28, 11, "div", 130);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](20, BuyerLocationsComponent_ng_template_56_div_20_Template, 6, 2, "div", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var assetNumber_r53 = ctx.assetIndex;

        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.isLoading);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r10.clickedAssetsDetails);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r10.clickedAssetsDetails[assetNumber_r53].AssetName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankName) && (ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankNumber));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r10.clickAssetsPanel);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0] == null ? null : ctx_r10.clickedAssetsDetails[assetNumber_r53].jobTankAdditionalDetails[0].TankChartPath);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.clickedAssetsDetails[assetNumber_r53].AssetType === 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.clickedAssetsDetails[assetNumber_r53].AssetType === 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.clickedAssetsDetails[assetNumber_r53].AssetType === 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.clickedAssetsDetails[assetNumber_r53].AssetType === 2);
      }
    }

    function BuyerLocationsComponent_ng_template_58_Template(rf, ctx) {
      if (rf & 1) {
        var _r90 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 154);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Supplier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "ng-multiselect-dropdown", 158, 159);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDeSelect", function BuyerLocationsComponent_ng_template_58_Template_ng_multiselect_dropdown_onDeSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r90);

          var ctx_r89 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r89.onSupplierSelect();
        })("onSelect", function BuyerLocationsComponent_ng_template_58_Template_ng_multiselect_dropdown_onSelect_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r90);

          var ctx_r91 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r91.onSupplierSelect();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](12, "ng-multiselect-dropdown", 161, 162);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 163);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](19, "ng-multiselect-dropdown", 164, 165);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "div", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "Status");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](25, "ng-multiselect-dropdown", 161, 165);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "div", 163);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "div", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "label", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](31, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](32, "ng-multiselect-dropdown", 161);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "div", 166);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](34, "div", 167);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "button", 168);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_ng_template_58_Template_button_click_35_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r90);

          var ctx_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r92.ResetFilters();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "button", 169);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_ng_template_58_Template_button_click_37_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r90);

          var ctx_r93 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](15);

          ctx_r93.ApplyFilters("set");
          return _r1.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Suppliers")("formControl", ctx_r12.FilterForm.controls["SelectedSupplierList"])("settings", ctx_r12.multiDropdownSettings)("data", ctx_r12.supplierList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r12.FilterForm.controls["SelectedlocationList"])("placeholder", "Select Location")("settings", ctx_r12.jobMultiselectSettingsById)("data", ctx_r12.locationList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Priority")("formControl", ctx_r12.FilterForm.controls["SelectedPriorityList"])("settings", ctx_r12.priorityselectSettingsById)("data", ctx_r12.priorityList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r12.FilterForm.controls["SelectedStatusList"])("placeholder", "Status")("settings", ctx_r12.multiselectSettingsById)("data", ctx_r12.statusList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r12.FilterForm.controls["selectedLocAttributeList"])("placeholder", "Inventory Capture Method")("settings", ctx_r12.multiselectSettingsById)("data", ctx_r12.LocationAttributeList);
      }
    }

    var _c6 = function _c6(a0, a1) {
      return {
        "fadeIn": a0,
        "display_hide": a1
      };
    };

    var BuyerLocationsComponent = /*#__PURE__*/function () {
      function BuyerLocationsComponent(buyerwallyboardservice, fb, changeDetectorRef) {
        _classCallCheck(this, BuyerLocationsComponent);

        this.buyerwallyboardservice = buyerwallyboardservice;
        this.fb = fb;
        this.changeDetectorRef = changeDetectorRef;
        this.isLoading = false;
        this.zoomLevel = 5;
        this.jobLocationDataForMap = [];
        this.clickedAssetsDetails = [];
        this.countryList = [];
        this.priorityList = [];
        this.supplierList = [];
        this.carrierList = [];
        this.isShowCarrierManaged = false;
        this.selectedCarrierIds = '';
        this.latestReading = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["DipTest"]();
        this.chartData = [];
        this.chartLabels = [];
        this.IsFiltersLoaded = false;
        this.UoM = '';
        this.selectedTankMinMax = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["TankMinMaxFill"]();
        this.selectedTankHeight = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["TankChartHeight"]();
        this.filteredJobLocationData = [];
        this.unchangedJobLocationData = [];
        this.assetDetails = {
          assetIndex: 0
        };
        this.assetsModal = {
          modalDetails: {
            display: 'none',
            data: 'Modal Show'
          }
        };
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.locationSubscription = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subscription"]();
        this.clickViewActive = false;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
        this.toogleMap = true;
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
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subscription"]();
        this.SelectedPriorityList = [];
        this.SelectedSupplierList = [];
        this.statusList = [];
        this.SelectedStatusList = [];
        this.locationList = [];
        this.DefaultLocationList = [];
        this.SelectedlocationList = [];
        this.SelectedCarrierList = [];
        this.isShowNonRetailJobs = false;
        this.jobIdsEmittedFromSalesComponent = [];
        this.IsLoadSalesData = true;
        this.LocationAttributeList = _app_constants__WEBPACK_IMPORTED_MODULE_6__["InventoryDataCaptureList"];
        this.isChartDataExistSubject = new rxjs__WEBPACK_IMPORTED_MODULE_1__["BehaviorSubject"](false);
        this.initializeFilterForm();
      }

      _createClass(BuyerLocationsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.SelectedPriorityList.push({
            Id: 1,
            Name: 'Must Go'
          });
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
          this.jobMultiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.priorityselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.getFilterData(); // var _this = this;
          // window.addEventListener("beforeunload", function (e) {
          //     _this.SaveFilters();
          //     return;
          // });
        }
      }, {
        key: "fetchJobLocationData",
        value: function fetchJobLocationData() {
          var _this2 = this;

          this.isLoading = true;

          if (this.jobIdsEmittedFromSalesComponent && this.jobIdsEmittedFromSalesComponent.length) {
            var ids = [];
            this.jobIdsEmittedFromSalesComponent.forEach(function (res) {
              ids.push(res.TfxJobId);
            });
            var jobids = "";
            jobids = ids.join(); //Location Type FIlter

            var selectedLocAttributeId = "";

            if (this.FilterForm.controls.selectedLocAttributeList.value && this.FilterForm.controls.selectedLocAttributeList.value.length > 0) {
              var ids = [];
              this.FilterForm.controls.selectedLocAttributeList.value.forEach(function (res) {
                ids.push(res.Id);
              });
              selectedLocAttributeId = ids.join();
            }

            this.locationSubscription.add(this.buyerwallyboardservice.getJobLocationDetails(jobids, selectedLocAttributeId).subscribe(function (res) {
              if (res) {
                _this2.unchangedJobLocationData = _this2.jobLocationDataForMap = _this2.addJobPriority(res['Data']['jobLocationDetails']); // this.locationList = this.unchangedJobLocationData;

                _this2.fillSupplierCarrierDropdowns(); // this.jobLocationDataForMap = this.applyFilter();

              }

              _this2.setCountryCentre();

              _this2.isLoading = false;
            }));
          } else {
            this.unchangedJobLocationData = this.jobLocationDataForMap = [];
            this.setCountryCentre();
            this.isLoading = false;
          }
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.SaveFilters();
        }
      }, {
        key: "fillSupplierCarrierDropdowns",
        value: function fillSupplierCarrierDropdowns() {
          var _this3 = this;

          this.CurrentCompanyId = Number(currentCompanyId);
          var jobIds = [];
          this.jobLocationDataForMap.map(function (item) {
            return jobIds.push(item.JobID);
          });

          if (jobIds && jobIds.length) {
            var selectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;
            selectedSuppliers = selectedSuppliers.filter(function (t) {
              return _this3.supplierList.filter(function (el) {
                return el.Id == t.Id;
              }).length > 0;
            });
            this.FilterForm.get('SelectedSupplierList').patchValue(selectedSuppliers);
            var selectedCarriers = this.FilterForm.get('SelectedCarrierList').value;
            selectedCarriers = selectedCarriers.filter(function (t) {
              return _this3.carrierList.filter(function (el) {
                return el.Id == t.Id;
              }).length > 0;
            });
            this.FilterForm.get('SelectedCarrierList').patchValue(selectedCarriers);
            this.onSupplierSelect();
            this.changeDetectorRef.detectChanges();
          }
        }
      }, {
        key: "initializeFilterForm",
        value: function initializeFilterForm() {
          this.FilterForm = this.fb.group({
            IsShowAssetJobs: this.fb.control(false),
            IsShowCarrierManaged: this.fb.control(false),
            SelectedCarrierList: this.fb.control([]),
            SelectedSupplierList: this.fb.control([]),
            SelectedlocationList: this.fb.control([]),
            SelectedPriorityList: this.fb.control([]),
            SelectedStatusList: this.fb.control([]),
            ToggleMap: this.fb.control(true),
            singleMulti: this.fb.control(this.singleMulti),
            selectedLocAttributeList: this.fb.control([])
          });
        }
      }, {
        key: "addJobPriority",
        value: function addJobPriority(jobLocationData) {
          var _this4 = this;

          if (jobLocationData && jobLocationData.length) {
            jobLocationData.forEach(function (element) {
              var obj = _this4.jobIdsEmittedFromSalesComponent.find(function (t) {
                return t.TfxJobId == element.JobID;
              });

              if (obj) {
                if (obj.Priority == 1) {
                  element.highestPriority = 1;
                  element.iconUrl = _this4.mustGoUrl;
                } else if (obj.Priority == 2) {
                  element.highestPriority = 2;
                  element.iconUrl = _this4.shouldGoUrl;
                } else if (obj.Priority == 3) {
                  element.highestPriority = 3;
                  element.iconUrl = _this4.couldGoUrl;
                } else {
                  element.highestPriority = 4;
                  element.iconUrl = _this4.noDlrUrl;
                }
              } else {
                element.highestPriority = 4;
                element.iconUrl = _this4.noDlrUrl;
              }
            });
          }

          return jobLocationData;
        } // private checkMostPriorityJob(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
        //     const jobLocationLength = jobLocationData.length;
        //     for (let i = 0; i < jobLocationLength; i++) {
        //         let deliveryRequests = jobLocationData[i].jobDeliveryRequests;
        //         if (deliveryRequests.length) {
        //             let filteredMustGoDRs = deliveryRequests.filter((data) => data.Priority === 1);
        //             let filteredShoudGoDRs = deliveryRequests.filter((data) => data.Priority === 2);
        //             if (filteredMustGoDRs.length > 0) {
        //                 jobLocationData[i].highestPriority = 1;
        //                 jobLocationData[i].iconUrl = this.mustGoUrl;
        //             }
        //             else if (filteredShoudGoDRs.length > 0) {
        //                 jobLocationData[i].highestPriority = 2;
        //                 jobLocationData[i].iconUrl = this.shouldGoUrl;
        //             }
        //             else {
        //                 jobLocationData[i].highestPriority = 3;
        //                 jobLocationData[i].iconUrl = this.couldGoUrl;
        //             }
        //         } else {
        //             jobLocationData[i].highestPriority = 4;
        //             jobLocationData[i].iconUrl = this.noDlrUrl;
        //         }
        //     }
        //     return jobLocationData;
        // }
        // private convertToObjectArray(data: string[]): SelectedItem[] {
        //     let modifiedItemArray: SelectedItem[] = [];
        //     data.forEach((item, index) => {
        //         let Item: SelectedItem = { 'Id': 0, 'Name': '' };
        //         Item.Id = index;
        //         Item.Name = item;
        //         modifiedItemArray.push(Item);
        //     })
        //     return modifiedItemArray;
        // }

      }, {
        key: "setCountryCentre",
        value: function setCountryCentre() {
          var _this5 = this;

          if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(function () {
              _this5.centerLocationLat = _this5.CountryCentre[_this5.UserCountry].lat;
              _this5.centerLocationLog = _this5.CountryCentre[_this5.UserCountry].lng;

              if (_this5.Map && (!_this5.jobLocationDataForMap || _this5.jobLocationDataForMap.length == 0)) {
                _this5.Map.setCenter(new google.maps.LatLng(_this5.centerLocationLat, _this5.centerLocationLog));

                _this5.Map.setZoom(5);
              } else {
                var bounds = new google.maps.LatLngBounds();

                _this5.jobLocationDataForMap.forEach(function (x) {
                  bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
                });

                if (_this5.Map && bounds) {
                  _this5.Map.fitBounds(bounds);

                  _this5.Map.setCenter(bounds.getCenter());

                  _this5.Map.setZoom(5);
                }
              }
            }, 500);
          }
        }
      }, {
        key: "setZoomLevel",
        value: function setZoomLevel() {
          if (this.jobLocationDataForMap && this.jobLocationDataForMap.length == 0) {
            this.setCountryCentre();
          } else {//this.zoomLevel = 10;
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
        key: "closeAssetsClicked",
        value: function closeAssetsClicked() {
          this.clickAssetsPanel = false;
        }
      }, {
        key: "closeViewClicked",
        value: function closeViewClicked() {
          this.clickViewActive = false;
          this.clickAssetsPanel = false;
          this.clickChartsPanel = false;
        }
      }, {
        key: "modalOpen",
        value: function modalOpen(jobLocation) {
          this.opendedJobDetails = jobLocation;

          if (this.opendedJobDetails.CountryCode === 'USA' || this.opendedJobDetails.CountryCode === 'US') {
            this.UoM = 'Gallons';
          } else {
            this.UoM = 'Litres';
          }

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
        key: "toggleMapView",
        value: function toggleMapView() {
          var toggleMap = this.FilterForm.get('ToggleMap').value;
          this.FilterForm.get('ToggleMap').patchValue(!toggleMap);
          this.toogleMap = !toggleMap;
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
        } // private applySubFilter(locationData: any): JobLocationDetailsModal[] {
        //     let location = true, priority = true, status = true;
        //     let filterData = locationData.filter((data: JobLocationDetailsModal) => {
        //         if (this.SelectedlocationList.length) {
        //             var ids = [];
        //             this.SelectedLocationIds = '';
        //             this.SelectedlocationList.forEach(res => { ids.push(res.Id) });
        //             this.SelectedLocationIds = ids.join();
        //             location = this.SelectedLocationIds.includes(String(data.JobID));
        //         }
        //         if (this.SelectedPriorityList.length) {
        //             var ids = [];
        //             this.SelectedPriorityIds = '';
        //             this.SelectedPriorityList.forEach(res => { ids.push(res.Id) });
        //             this.SelectedPriorityIds = ids.join();
        //             priority = this.SelectedPriorityIds.includes(String(data.highestPriority))
        //         }
        //         if (this.SelectedStatusList.length) {
        //             var ids = [];
        //             this.SelectedStatusesId = '';
        //             this.SelectedStatusList.forEach(res => { ids.push(res.Id) });
        //             this.SelectedStatusesId = ids.join();
        //             status = this.SelectedStatusesId.includes(String(data.ScheduleStatus))
        //         }
        //         return (location && priority && status);
        //     })
        //     return filterData;
        // }
        // private applyFilter() {
        //     if (this.IsCarrierChkChanged()) {
        //         return;
        //     }
        //     if (this.IsAssetChkChanged()) {
        //         return;
        //     }
        //     this.jobLocationDataForMap = this.unchangedJobLocationData;
        //     let filteredJobData = [];
        //     let filter = this.FilterForm.value;
        //     if (this.SelectedSupplierList.length) {
        //         var objSup = this.SelectedSupplierList.reduce((a, c) => Object.assign(a, { [c.Id]: c.Id }), {});
        //         filteredJobData = this.jobLocationDataForMap.filter(f => f.supplierDetails.some(o => objSup[o.Id] === o.Id));
        //     }
        //     if (filter.SelectedCarrierList.length) {
        //         var obj = filter.SelectedCarrierList.reduce((a, c) => Object.assign(a, { [c.Id]: c.Id }), {});
        //         if (filteredJobData.length) {
        //             filteredJobData = filteredJobData.filter(f => f.carrierDetails.some(o => obj[o.Id] === o.Id));
        //         }
        //         else {
        //             filteredJobData = this.jobLocationDataForMap.filter(f => f.carrierDetails.some(o => obj[o.Id] === o.Id));
        //         }
        //     }
        //     if (filter.SelectedSupplierList.length == 0 && filter.SelectedCarrierList.length == 0) {
        //         filteredJobData = this.unchangedJobLocationData;
        //     }
        //     return this.applySubFilter(filteredJobData);
        // }

      }, {
        key: "closeChartsClicked",
        value: function closeChartsClicked() {
          this.clickChartsPanel = false;
        }
      }, {
        key: "getDipTestDetails",
        value: function getDipTestDetails(siteId, tankId, noOfDays) {
          var _this6 = this;

          this.isLoading = true;
          this.chartData = [];
          this.chartOptions = {};
          this.chartOptions = this.setChartOptions(this.UserCountry);
          this.chartLabels = [];
          this.latestReading = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["DipTest"]();
          this.calculateMinMAx(this.clickedAssetsDetails[this.assetDetails.assetIndex].jobTankAdditionalDetails[0]);
          this.locationSubscription.add(this.buyerwallyboardservice.getDipTestDetails(siteId, tankId, noOfDays).subscribe(function (data) {
            if (data && data.StatusCode === 302) {
              var resp = data.Data;
              _this6.latestReading = resp[0];
              var obj = {};
              var chartdata = [];
              obj['label'] = 'Tank ' + resp[0]['TankId'];
              var respLen = resp.length;

              for (var i = 0; i < respLen; i++) {
                var captureTime = moment__WEBPACK_IMPORTED_MODULE_2__(new Date(resp[i].CaptureTimeString)).format('MM/DD/YYYY hh:mm A');
                chartdata.unshift(resp[i].NetVolume);

                _this6.chartLabels.unshift(captureTime);
              }

              obj['data'] = chartdata;

              _this6.chartData.push(obj);
            }

            _this6.isLoading = false;
          }));
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
                  labelString: "NetVolume ( Fuels Per ".concat(this.UoM, ")")
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
        key: "onInfoViewClick",
        value: function onInfoViewClick(jobLocation) {
          window.scrollTo(0, 0);
          this.opendedJobDetails = jobLocation;

          if (this.opendedJobDetails.CountryCode === 'USA' || this.opendedJobDetails.CountryCode === 'US') {
            this.UoM = 'Gallons';
          } else {
            this.UoM = 'Litres';
          }

          this.assetDetails.assetIndex = 0;
          this.clickViewActive = true;
          this.clickAssetsPanel = false;
          this.clickChartsPanel = false;
          this.FilterForm.get('ToggleMap').patchValue(true);
          this.toogleMap = true;
          this.closeAssetsClicked();
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
              this.latestReading = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["DipTest"]();
            }
          }
        }
      }, {
        key: "onChartsViewClick",
        value: function onChartsViewClick(assets) {
          this.clickChartsPanel = true;
          this.isChartDataExistSubject.next(false);
          var tanks = [];

          if (assets.length) {
            tanks = assets.filter(function (t) {
              return t.AssetType == 2;
            });
          }

          if (tanks.length && tanks[0].jobTankAdditionalDetails.length) {
            this.getDemandCaptureChart(tanks[0].jobTankAdditionalDetails[0]['SiteId'], 3, tanks[0].JobId);
          } else {
            this.isChartDataExistSubject.next(false);
            this.demandChartData = null;
          }
        }
      }, {
        key: "assetTabClicked",
        value: function assetTabClicked(indx) {
          this.assetDetails.assetIndex = indx;

          if (this.clickedAssetsDetails[indx].jobTankAdditionalDetails.length) {
            this.getDipTestDetails(this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['SiteId'], this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['TankId'], 3);
          } else {
            this.chartData = [];
            this.latestReading = new _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["DipTest"]();
          }
        }
      }, {
        key: "mapReady",
        value: function mapReady(map) {
          this.Map = map;
          this.setCountryCentre();
        }
      }, {
        key: "getDemandCaptureChart",
        value: function getDemandCaptureChart(siteId, noOfDays, tfxJobId) {
          this.demandChartData = {
            siteId: siteId,
            noOfDays: noOfDays,
            tfxJobId: tfxJobId
          };
          this.isChartDataExistSubject.next(true);
        }
      }, {
        key: "setCenterMap",
        value: function setCenterMap($event) {
          if (this.UserCountry && (!this.jobLocationDataForMap || !this.jobLocationDataForMap.length)) {
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
        } //Will this work?

      }, {
        key: "openDRPanel",
        value: function openDRPanel(event) {
          this.modalClose();
          jQuery('#jobIdForDr').val(event.JobId); // set the html hidden field value i9n shared layout.cshtml

          jQuery('#demandCaptureButton').click(); // trigger click of demand capture button in layout.cshtml
        } // public IsCarrierChkChanged() {
        //     var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
        //     if (this.isShowCarrierManaged != isCarrierManaged) {
        //         this.selectedCarrierIds = '';
        //         this.FilterForm.get('SelectedCarrierList').patchValue([]);
        //         this.FilterForm.get('SelectedSupplierList').patchValue([]);
        //         this.isShowCarrierManaged = isCarrierManaged;
        //         // this.fetchJobLocationData();
        //         return true;
        //     }
        //     return false;
        // }
        // public IsAssetChkChanged() {
        //     var isAssetChkManaged = this.FilterForm.get('IsShowAssetJobs').value;
        //     if (this.isShowNonRetailJobs != isAssetChkManaged) {
        //         this.isShowNonRetailJobs = isAssetChkManaged;
        //         // this.fetchJobLocationData();
        //         return true;
        //     }
        //     return false;
        // }

      }, {
        key: "SaveFilters",
        value: function SaveFilters() {
          var filterData = this.FilterForm.value;
          var filterModel = JSON.stringify(filterData);
          this.buyerwallyboardservice.saveFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["TfxModule"].BuyerWallyboardLocation, filterModel).subscribe(function (res) {
            if (res) {}
          });
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this7 = this;

          this.buyerwallyboardservice.getFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_3__["TfxModule"].BuyerWallyboardLocation).subscribe(function (res) {
            if (res && res.length > 0) {
              _this7.SetFilters(res);
            } else {
              _this7.IsFiltersLoaded = true;

              _this7.changeDetectorRef.detectChanges();

              _this7.FilterForm.get('SelectedPriorityList').patchValue([{
                Id: 1,
                Name: 'Must Go'
              }]);

              _this7.locationView.getSalesData();
            }
          });
        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          this.IsFiltersLoaded = true;
          this.changeDetectorRef.detectChanges();
          var filterData = JSON.parse(input);

          if (!filterData.SelectedPriorityList || !filterData.SelectedPriorityList.length) {
            filterData.SelectedPriorityList = [{
              Id: 1,
              Name: 'Must Go'
            }];
          }

          this.FilterForm.patchValue(filterData);
          this.toogleMap = filterData.ToggleMap;
          this.locationView.getSalesData();
          this.onSupplierSelect();
          this.ApplyFilters();
        }
      }, {
        key: "getFilterData",
        value: function getFilterData() {
          var _this8 = this;

          var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
          this.buyerwallyboardservice.GetFilterData(isCarrierManaged).subscribe(function (data) {
            // this.locationList = data.map(t => {return {JobID: t.Id, JobName : t.Name}});
            _this8.locationList = data;
            _this8.DefaultLocationList = data;
            _this8.supplierList = _this8.GetUniqueItems(data.map(function (t) {
              return t.Suppliers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
            _this8.carrierList = _this8.GetUniqueItems(data.map(function (t) {
              return t.Carriers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));

            _this8.GetFilters();
          }); // this.ApplyFilters();
        }
      }, {
        key: "onSupplierSelect",
        value: function onSupplierSelect() {
          var selectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;

          if (selectedSuppliers != undefined && selectedSuppliers.length > 0) {
            // this.locationList=this.DefaultLocationList.filter(t=>t.Suppliers.filter(t1=> selectedSuppliers.some(t2=> t2.Id == t1.Id) ).length > 0 ).map(t => {return {JobID: t.Id, JobName : t.Name}});
            this.locationList = this.DefaultLocationList.filter(function (t) {
              return t.Suppliers.filter(function (t1) {
                return selectedSuppliers.some(function (t2) {
                  return t2.Id == t1.Id;
                });
              }).length > 0;
            });
          } else {
            // this.locationList= this.DefaultLocationList.map(t => {return {JobID: t.Id, JobName : t.Name}});
            this.locationList = this.DefaultLocationList;
          }

          this.locationList = this.GetUniqueItems(this.locationList.reduce(function (p, n) {
            return p.concat(n);
          }, []));
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
        key: "getJobIdsForMapEventHandler",
        value: function getJobIdsForMapEventHandler(valueEmitted) {
          this.jobIdsEmittedFromSalesComponent = valueEmitted;
          this.fetchJobLocationData();
        }
      }, {
        key: "ResetFilters",
        value: function ResetFilters() {
          this.FilterForm.get('SelectedSupplierList').patchValue([]);
          this.FilterForm.get('SelectedlocationList').patchValue([]);
          this.FilterForm.get('SelectedPriorityList').patchValue([]);
          this.FilterForm.get('SelectedStatusList').patchValue([]);
          this.FilterForm.get('selectedLocAttributeList').patchValue([]);
          this.ApplyFilters('reset');
        }
      }, {
        key: "ApplyFilters",
        value: function ApplyFilters(msg) {
          // this.SaveFilters();
          this.filterCount = 0;

          if (this.FilterForm) {
            var selectedSupplierList = this.FilterForm.get('SelectedSupplierList').value || [];
            this.filterCount += selectedSupplierList.length;
            var selectedlocationList = this.FilterForm.get('SelectedlocationList').value || [];
            this.filterCount += selectedlocationList.length;
            var selectedPriorityList = this.FilterForm.get('SelectedPriorityList').value || [];
            this.filterCount += selectedPriorityList.length;
            var selectedStatusList = this.FilterForm.get('SelectedStatusList').value || [];
            this.filterCount += selectedStatusList.length;
            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
            this.filterCount += selectedLocAttributeList.length;
          }

          if (msg == "set") {
            this.locationView.applyLoadsFilters(this.FilterForm);
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess("Filter applied successfully", undefined, undefined);
          } else if (msg == "reset") {
            this.locationView.applyLoadsFilters(this.FilterForm);
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msginfo("Filter reset successfully", undefined, undefined);
          }

          this.SaveFilters();
        }
      }]);

      return BuyerLocationsComponent;
    }();

    BuyerLocationsComponent.??fac = function BuyerLocationsComponent_Factory(t) {
      return new (t || BuyerLocationsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_7__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]));
    };

    BuyerLocationsComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: BuyerLocationsComponent,
      selectors: [["app-buyer-locations"]],
      viewQuery: function BuyerLocationsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.locationView = _t.first);
        }
      },
      inputs: {
        singleMulti: "singleMulti"
      },
      decls: 60,
      vars: 30,
      consts: [[1, "col-sm-9", "sticky-header-loc", 3, "formGroup"], [1, "row", "mr15"], [1, "col-sm-4"], [1, "form-check", "form-check-inline", "fs14", "mt5"], ["type", "checkbox", "id", "inlineCarrierManaged", "formControlName", "IsShowCarrierManaged", 1, "form-check-input", 3, "change"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["class", "mtm5", 4, "ngIf"], ["type", "checkbox", "id", "inlineShowAsset", "formControlName", "IsShowAssetJobs", 1, "form-check-input", 3, "change"], ["for", "inlineShowAsset", 1, "form-check-label"], [1, "col-4", "pl0", "text-right"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], [1, "hide_show_map", "fs14", "dib", "mr10", 3, "click"], [1, "fas", "fa-eye", "mr5"], [1, "animated", "clearboth", "mt60", "row", 3, "ngClass"], [3, "ngClass"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], [1, "pr"], ["id", "mapLegend", 2, "z-index", "1", "position", "absolute", "bottom", "0", "left", "10px", "font-size", "11px"], ["id", "status-legends", 1, "well", "pa0"], [1, "border-b"], [1, "db", "pl5", "pr5", "pt8", "pb5", "radius-10", "no-b-radius"], ["data-statusid", "11", 3, "src"], [1, "db", "pa5"], ["data-statusid", "12", 3, "src"], ["data-statusid", "1", 3, "src"], ["class", "pa top0 bg-white left0 z-index5 loading-wrapper", 4, "ngIf"], [3, "zoom", "minZoom", "maxZoom", "mapTypeControl", "fullscreenControl", "fullscreenControlOptions", "mapReady"], [4, "ngFor", "ngForOf"], ["class", "col-sm-4 pl0 right_side_panel", 4, "ngIf"], [1, "row", 3, "ngClass"], [1, "col-sm-12"], [3, "FilterForm", "IsFiltersLoaded", "IsLoadSalesData", "getJobIdsForMap"], ["assetDetailsModal", ""], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["assetsContentTemplate", ""], ["popContent", ""], [1, "mtm5"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "p-0"], [1, "fs14", 3, "placeholder", "formControl", "settings", "data", "onSelect", "onDeSelect"], ["selectedCarriers", ""], [1, "circle-badge"], [1, "pa", "top0", "bg-white", "left0", "z-index5", "loading-wrapper"], [3, "latitude", "longitude", "iconUrl", "mouseOver", "mouseOut", "markerClick"], [3, "disableAutoPan"], ["infoWindow", ""], [1, "col-sm-4", "pl0", "right_side_panel"], [1, "dib", "full-width", "pr", "well", "pa15", "pt10"], [1, "row"], ["class", "color-maroon pull-left", 4, "ngIf"], [1, "pull-right", 3, "click"], [1, "far", "fa-times-circle", "fa-lg"], ["class", "img-responsive", 3, "src", 4, "ngIf"], [1, "col-sm-12", "driver_details"], [1, "job-location"], [1, "mb0"], [1, "address1"], [1, "fas", "fa-briefcase"], [1, "fas", "fa-map-marker-alt"], [1, "site-status", "fs12", "mt5"], [1, "panel-group"], [1, "panel", "panel-default"], [1, "panel-heading"], ["data-toggle", "collapse", "href", "#collapse1"], [1, "f-bold"], ["class", "status  ml10", 4, "ngIf"], ["data-toggle", "collapse", "href", "#collapse1", 1, "pull-right"], [1, "fas", "collapse1_icon", "fa-2x", "line-height_18", "fa-angle-down"], ["id", "collapse1", 1, "panel-collapse", "collapse"], [1, "panel-body"], ["class", "date_time", 4, "ngFor", "ngForOf"], [4, "ngIf"], [1, "site-instruction", "fs12", "mb5"], [1, "f-bold", "db", "mb0"], [1, "instruction", "opacity8"], [1, "site-contacts", "fs12", "row", "mb5"], [1, "col-sm-3"], [1, "col-sm-9"], [1, "mb0", "opacity8"], [1, "col-sm-12", "site-assets"], [1, "btn", "btn-default", "pull-left", "ml0", "fs12", 3, "click"], [1, "btn", "btn-default", "pull-left", "fs12", 3, "click"], ["class", "assets-panel dib full-width pr well pa15 pt10", 4, "ngIf"], ["class", "charts-panel dib full-width pr well pa15 pt10 z-index10", 4, "ngIf"], [1, "color-maroon", "pull-left"], [1, "fas", "fa-image", "mr5"], [1, "img-responsive", 3, "src"], [1, "status", "ml10"], [1, "date_time"], [1, "day", "ml10"], [1, "status", "ml10", "text-success"], [1, "assets-panel", "dib", "full-width", "pr", "well", "pa15", "pt10"], [1, "assets-header"], [1, "row", "mb5"], [1, "pull-left", 3, "click"], [1, "fas", "fa-arrow-left"], [1, "charts-panel", "dib", "full-width", "pr", "well", "pa15", "pt10", "z-index10"], [1, "charts-body"], [2, "width", "100%", "height", "50vh"], [2, "width", "100%"], [3, "data", 4, "ngIf", "ngIfElse"], ["noDtaAvailable", ""], [3, "data"], [1, "alert", "alert-danger"], ["id", "assetDetailsModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "assetDetailsModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt15", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body", 2, "max-height", "80vh", "overflow-y", "scroll"], ["class", "assets-header", 4, "ngIf"], [1, "aseets-body", "assets_modal"], [1, "nav", "nav-tabs"], [1, "tab-content", "pa0"], ["id", "assets1", 1, "tab-pane", "fade", "in", "active", "animated", "fadeIn"], [1, "row", "mb10", "mt10"], [1, "col", "align-self-start"], [1, "mt0", "mb0", "float-left", "fs16"], ["class", "mt0 mb0 float-left fs16", 4, "ngIf"], ["class", "col align-self-end", 4, "ngIf"], [1, "col", "align-self-end"], ["target", "_blank", "download", "", "class", "float-right mt5", 3, "href", 4, "ngIf"], ["class", "border radius-5 pa15 mb20", 4, "ngIf"], ["class", "border radius-5 pa15 tank-panel mb20", 4, "ngIf"], ["class", "row", 4, "ngIf"], ["class", "assets-id", 4, "ngIf"], [3, "click"], ["class", "btn btn-primary btn-sm float-right", 3, "click", 4, "ngIf"], [1, "btn", "btn-primary", "btn-sm", "float-right", 3, "click"], ["target", "_blank", "download", "", 1, "float-right", "mt5", 3, "href"], ["aria-hidden", "true", 1, "fa", "fa-download", "mr5"], [1, "border", "radius-5", "pa15", "mb20"], ["width", "100%", 1, "table", "table-condensed", "table-bordered", "table-hover", "small-table", "mb0", "mt10", "fs12"], [1, "border", "radius-5", "pa15", "tank-panel", "mb20"], [1, "tank_dip_chart", "text-center", "mt10"], ["id", "ShouldBeEmptyPercent", 1, "color-green", 3, "ngStyle"], ["id", "ShouldBeFilledPercent", 1, "color-green", 3, "ngStyle"], ["id", "CurrentInventoryPercent", 1, "red-bg", 3, "ngStyle"], ["width", "50%", 1, "f-bold"], [1, "table", "table-condensed", "table-hover", "table-bordered", "small-table"], [4, "ngIf", "ngIfElse"], ["reading", ""], [1, "assets-id"], ["style", "width: 100%;max-height:320px", 4, "ngIf"], ["class", "alert alert-danger", 4, "ngIf"], [2, "width", "100%", "max-height", "320px"], ["baseChart", "", "height", "300", 2, "margin", "auto", 3, "chartType", "datasets", "options", "labels", "legend"], [1, "popover-details"], [1, "row", "border-bottom-2"], [1, "col-6", "pr-0"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], [3, "placeholder", "formControl", "settings", "data", "onDeSelect", "onSelect"], ["selectedSuppliers", ""], [1, "col-6"], [3, "formControl", "placeholder", "settings", "data"], ["selectedLocations", ""], [1, "row", "border-bottom-2", "mt10"], [3, "placeholder", "formControl", "settings", "data"], ["selectedPriority", ""], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"]],
      template: function BuyerLocationsComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r94 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function BuyerLocationsComponent_Template_input_change_4_listener() {
            return ctx.SaveFilters();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](7, BuyerLocationsComponent_div_7_Template, 5, 2, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function BuyerLocationsComponent_Template_input_change_10_listener() {
            return ctx.SaveFilters();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12, " Show locations with Assets");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "a", 10, 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_Template_a_click_14_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r94);

            var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](15);

            return _r1.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "i", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, BuyerLocationsComponent_span_17_Template, 2, 1, "span", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "a", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function BuyerLocationsComponent_Template_a_click_19_listener() {
            return ctx.toggleMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](20, "i", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](25, "span", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](31, "img", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, " Must Go ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](34, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](35, "img", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, " Should Go ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](38, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](39, "img", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](40, " Could Go ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](43, "img", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](44, " Unplanned ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](45, BuyerLocationsComponent_div_45_Template, 2, 0, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "agm-map", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("mapReady", function BuyerLocationsComponent_Template_agm_map_mapReady_46_listener($event) {
            return ctx.mapReady($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](47, BuyerLocationsComponent_ng_container_47_Template, 8, 9, "ng-container", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](48, BuyerLocationsComponent_div_48_Template, 59, 15, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](51, "app-location-view", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("getJobIdsForMap", function BuyerLocationsComponent_Template_app_location_view_getJobIdsForMap_51_listener($event) {
            return ctx.getJobIdsForMapEventHandler($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](52, " Loading... ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](53, BuyerLocationsComponent_ng_template_53_Template, 10, 7, "ng-template", null, 36, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](55, BuyerLocationsComponent_ng_container_55_Template, 1, 0, "ng-container", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](56, BuyerLocationsComponent_ng_template_56_Template, 21, 10, "ng-template", null, 38, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](58, BuyerLocationsComponent_ng_template_58_Template, 39, 20, "ng-template", null, 39, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);
        }

        if (rf & 2) {
          var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](54);

          var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](59);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.FilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FilterForm.get("IsShowCarrierManaged").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r11)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.filterCount > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", ctx.FilterForm.get("ToggleMap").value ? "Hide Map View" : "Show Map View", " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction2"](27, _c6, ctx.toogleMap, !ctx.toogleMap));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", ctx.clickViewActive ? "col-sm-8 mb15" : "col-sm-12 mb15");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("src", ctx.mustGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("src", ctx.shouldGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("src", ctx.couldGoUrl, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("src", ctx.noDlrUrl, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????sanitizeUrl"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("zoom", ctx.zoomLevel)("minZoom", 2)("maxZoom", 16)("mapTypeControl", true)("fullscreenControl", true)("fullscreenControlOptions", ctx.screenOptions);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.jobLocationDataForMap);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.clickViewActive);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", ctx.toogleMap ? "mt20" : "mt60");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("FilterForm", ctx.FilterForm)("IsFiltersLoaded", ctx.IsFiltersLoaded)("IsLoadSalesData", ctx.IsLoadSalesData);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngTemplateOutlet", _r6)("ngTemplateOutletContext", ctx.assetsModal);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbPopover"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"], _agm_core__WEBPACK_IMPORTED_MODULE_11__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgTemplateOutlet"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_12__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormControlDirective"], _agm_core__WEBPACK_IMPORTED_MODULE_11__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_11__["AgmInfoWindow"], _shared_components_demand_capture_chart_demand_capture_chart_component__WEBPACK_IMPORTED_MODULE_13__["DemandCaptureChartComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgStyle"], ng2_charts__WEBPACK_IMPORTED_MODULE_14__["BaseChartDirective"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["AsyncPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["DecimalPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["DatePipe"]],
      styles: ["/*Moved map css to wally-dashboard.component.css*/\n.locationfilter-in-map {\n  width: 90%;\n}\n.sticky-header-loc {\n  position: fixed;\n  right: 0;\n  padding: 15px 5px;\n  top: 45px;\n  height: 65px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n}\n.locationfilter {\n  width: 100%;\n  position: absolute;\n  right: -4px;\n  border-radius: 5px;\n  font-size: 14px;\n}\n.right_side_panel {\n  padding: 0 10px 10px;\n}\n.right_side_panel .close_btn {\n  right: 10px;\n  top: 5px;\n}\n.right_side_panel #myCarousel img {\n  max-height: 150px;\n  width: 100%;\n}\n.right_side_panel img {\n  max-height: 25vh !important;\n  margin: auto !important;\n}\n.right_side_panel .driver_details {\n  margin-top: 10px;\n}\n.right_side_panel .driver_details .site-status .panel-default {\n  background-color: white;\n  border: 0;\n}\n.right_side_panel .driver_details .site-status .panel-heading {\n  padding: 5px 0px;\n  background-color: white;\n  border: 0;\n}\n.right_side_panel .driver_details .site-status .panel-body {\n  padding: 5px 5px;\n}\n.right_side_panel .driver_details .site-status .panel-group {\n  margin-bottom: 0px;\n}\n.right_side_panel .driver_details .site-status .panel-heading a {\n  color: #000000;\n}\n#buyer-datatable .bg_noDlr_go {\n  background-color: #e5e5e5;\n}\n.right_side_panel .driver_details .site-status .date_time {\n  padding: 5px;\n}\n.right_side_panel .assets-panel {\n  position: absolute;\n  top: 0;\n  left: 0;\n  background: #ffffff;\n  width: 100%;\n  height: 100%;\n}\n.assets-panel .aseets-body .nav-tabs {\n  overflow-x: auto;\n  overflow-y: hidden;\n  display: -webkit-box;\n  display: -moz-box;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar-thumb {\n  background-color: #e3e3e3;\n  border-radius: 5px;\n  opacity: 0.2;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar-track {\n  background-color: transparent;\n}\n.assets-panel .aseets-body .nav-tabs::-webkit-scrollbar {\n  width: 6px;\n  height: 4px;\n  overflow: scroll;\n  background-color: inherit;\n  border-radius: 5px;\n}\n.assets-panel .aseets-body .nav-tabs > li {\n  float: none;\n  padding-bottom: 5px;\n}\n.aseets-body .nav > li.active {\n  background: none !important;\n}\n.aseets-body .nav > li > a {\n  padding: 2px 0 !important;\n  border-width: 0 !important;\n  border-bottom: 2px solid #fff !important;\n  margin-right: 8px;\n}\n.aseets-body .nav > li.active > a {\n  padding: 2px 0 !important;\n  border-width: 0 !important;\n  border-bottom: 2px solid #0c52b1 !important;\n  color: #0c52b1;\n}\n.aseets-body .nav > li > a:hover {\n  border-width: 0 !important;\n  border-bottom: 2px solid #0c52b1 !important;\n  background: none !important;\n  color: #848484;\n}\n.right_side_panel .aseets-body .tab-content {\n  max-height: 310px;\n  overflow-y: auto;\n  overflow-x: hidden;\n}\n.right_side_panel .charts-panel {\n  position: absolute;\n  top: 0;\n  left: 0;\n  background: #ffffff;\n  width: 100%;\n  height: 100%;\n}\n.right_side_panel .charts-panel .charts-header {\n  padding: 10px 5px;\n}\n.right_side_panel .charts-panel .charts-body {\n  max-height: 340px;\n  overflow-y: auto;\n  overflow-x: hidden;\n}\n.show_filter {\n  width: 100%;\n  background: 0 0;\n  position: relative;\n  top: 0px;\n  left: 0px;\n  border-radius: 5px;\n  opacity: 1;\n  margin-top: 5px;\n}\n#assetDetailsModal .modal-header {\n  padding: 5px 15px;\n  border-bottom: 1px solid #e5e5e5;\n}\n.asset-details td {\n  padding: 4px 8px;\n}\n.aseets-body .nav-tabs {\n  border-bottom: 0;\n}\ntable.dataTable.fixedHeader-floating {\n  top: 17px !important;\n}\n.display_hide {\n  display: none;\n  transition: opacity 1s ease-out;\n  opacity: 0;\n}\ntable.dataTable.fixedHeader-locked {\n  position: fixed !important;\n}\ntable.dataTable.fixedHeader-floating, table.dataTable.fixedHeader-locked {\n  top: 65px !important;\n}\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n.carrier-popover.popover .popover-body {\n  padding: 10px;\n  border-radius: 5px;\n}\n.activediff-dot {\n  height: 10px;\n  width: 10px;\n  background-color: #585bff;\n  border-radius: 50%;\n  display: inline-block;\n  -webkit-animation: 1s blink ease infinite;\n          animation: 1s blink ease infinite;\n}\n/* buyer-location Master-Filters starts here*/\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvRDpcXFRGU2NvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlXFxTaXRlRnVlbC5FeGNoYW5nZS5Tb3VyY2VDb2RlXFxTaXRlRnVlbC5FeGNoYW5nZS5XZWIvc3JjXFxhcHBcXGJ1eWVyLXdhbGx5LWJvYXJkXFxidXllci1sb2NhdGlvbnMuY29tcG9uZW50LnNjc3MiLCJzcmMvYXBwL2J1eWVyLXdhbGx5LWJvYXJkL2J1eWVyLWxvY2F0aW9ucy5jb21wb25lbnQuc2NzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFDQSxpREFBQTtBQUNBO0VBQ0ksVUFBQTtBQ0FKO0FESUE7RUFDSSxlQUFBO0VBQ0EsUUFBQTtFQUNBLGlCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0FDREo7QURJQTtFQUNJLFdBQUE7RUFDQSxrQkFBQTtFQUNBLFdBQUE7RUFDQSxrQkFBQTtFQUNBLGVBQUE7QUNESjtBREtBO0VBQ0ksb0JBQUE7QUNGSjtBREtBO0VBQ0ksV0FBQTtFQUNBLFFBQUE7QUNGSjtBREtBO0VBQ0ksaUJBQUE7RUFDQSxXQUFBO0FDRko7QURLQTtFQUNJLDJCQUFBO0VBQ0EsdUJBQUE7QUNGSjtBREtBO0VBQ0ksZ0JBQUE7QUNGSjtBREtBO0VBQ0ksdUJBQUE7RUFDQSxTQUFBO0FDRko7QURLQTtFQUNJLGdCQUFBO0VBQ0EsdUJBQUE7RUFDQSxTQUFBO0FDRko7QURLQTtFQUNJLGdCQUFBO0FDRko7QURLQTtFQUNJLGtCQUFBO0FDRko7QURLQTtFQUNJLGNBQUE7QUNGSjtBREtBO0VBQ0kseUJBQUE7QUNGSjtBREtBO0VBQ0ksWUFBQTtBQ0ZKO0FES0E7RUFDSSxrQkFBQTtFQUNBLE1BQUE7RUFDQSxPQUFBO0VBQ0EsbUJBQUE7RUFDQSxXQUFBO0VBQ0EsWUFBQTtBQ0ZKO0FES0E7RUFDSSxnQkFBQTtFQUNBLGtCQUFBO0VBQ0Esb0JBQUE7RUFDQSxpQkFBQTtBQ0ZKO0FES0E7RUFDSSx5QkFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtBQ0ZKO0FES0E7RUFDSSw2QkFBQTtBQ0ZKO0FES0E7RUFDSSxVQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0VBQ0EseUJBQUE7RUFDQSxrQkFBQTtBQ0ZKO0FES0E7RUFDSSxXQUFBO0VBQ0EsbUJBQUE7QUNGSjtBREtBO0VBQ0ksMkJBQUE7QUNGSjtBREtBO0VBQ0kseUJBQUE7RUFDQSwwQkFBQTtFQUNBLHdDQUFBO0VBQ0EsaUJBQUE7QUNGSjtBREtBO0VBQ0kseUJBQUE7RUFDQSwwQkFBQTtFQUNBLDJDQUFBO0VBQ0EsY0FBQTtBQ0ZKO0FES0E7RUFDSSwwQkFBQTtFQUNBLDJDQUFBO0VBQ0EsMkJBQUE7RUFDQSxjQUFBO0FDRko7QURNQTtFQUNJLGlCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxrQkFBQTtBQ0hKO0FETUE7RUFDSSxrQkFBQTtFQUNBLE1BQUE7RUFDQSxPQUFBO0VBQ0EsbUJBQUE7RUFDQSxXQUFBO0VBQ0EsWUFBQTtBQ0hKO0FETUE7RUFDSSxpQkFBQTtBQ0hKO0FETUE7RUFDSSxpQkFBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7QUNISjtBRE1BO0VBQ0ksV0FBQTtFQUNBLGVBQUE7RUFDQSxrQkFBQTtFQUNBLFFBQUE7RUFDQSxTQUFBO0VBQ0Esa0JBQUE7RUFDQSxVQUFBO0VBQ0EsZUFBQTtBQ0hKO0FETUE7RUFDSSxpQkFBQTtFQUNBLGdDQUFBO0FDSEo7QURNQTtFQUNJLGdCQUFBO0FDSEo7QURNQTtFQUNJLGdCQUFBO0FDSEo7QURNQTtFQUNJLG9CQUFBO0FDSEo7QURNQTtFQUNJLGFBQUE7RUFDQSwrQkFBQTtFQUNBLFVBQUE7QUNISjtBRE1BO0VBQ0ksMEJBQUE7QUNISjtBRE1BO0VBQ0ksb0JBQUE7QUNISjtBRE1BO0VBQ0ksZ0JBQUE7RUFDQSxnQkFBQTtFQUNBLG1CQUFBO0VBQ0EseUJBQUE7RUFDQSxzQkFBQTtFQUNBLGtEQUFBO0VBQ0EsbUJBQUE7QUNISjtBRE1BO0VBQ0ksYUFBQTtFQUNBLGtCQUFBO0FDSEo7QURNQTtFQUNJLFlBQUE7RUFDQSxXQUFBO0VBQ0EseUJBQUE7RUFDQSxrQkFBQTtFQUNBLHFCQUFBO0VBQ0EseUNBQUE7VUFBQSxpQ0FBQTtBQ0hKO0FETUEsNkNBQUE7QUFHSTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDTFI7QURPUTtFQUlJLFVBQUE7RUFDQSxrQkFBQTtFQUNBLG1CQUFBO0FDUlo7QURXUTtFQUNJLGFBQUE7QUNUWjtBRFlZO0VBQ0ksMkJBQUE7QUNWaEI7QURjUTtFQUNJLDJDQUFBO0FDWlo7QURpQkE7RUFDSSxrQkFBQTtFQUNBLFVBQUE7RUFDQSxXQUFBO0VBQ0EsbUJBQUE7RUFDQSxrQkFBQTtFQUNBLGVBQUE7RUFDQSxrQkFBQTtFQUNBLFlBQUE7RUFDQSxvQkFBQTtFQUNBLG1CQUFBO0VBQ0EsdUJBQUE7RUFDQSxXQUFBO0VBQ0EsWUFBQTtBQ2RKIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvYnV5ZXItbG9jYXRpb25zLmNvbXBvbmVudC5zY3NzIiwic291cmNlc0NvbnRlbnQiOlsiXHJcbi8qTW92ZWQgbWFwIGNzcyB0byB3YWxseS1kYXNoYm9hcmQuY29tcG9uZW50LmNzcyovXHJcbi5sb2NhdGlvbmZpbHRlci1pbi1tYXAge1xyXG4gICAgd2lkdGg6IDkwJTtcclxufVxyXG5cclxuXHJcbi5zdGlja3ktaGVhZGVyLWxvYyB7XHJcbiAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbiAgICByaWdodDogMDtcclxuICAgIHBhZGRpbmc6IDE1cHggNXB4O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgZm9udC1zaXplOiAyMHB4O1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcblxyXG4ubG9jYXRpb25maWx0ZXIge1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICByaWdodDogLTRweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMTRweDtcclxufVxyXG5cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIHtcclxuICAgIHBhZGRpbmc6IDAgMTBweCAxMHB4O1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCAuY2xvc2VfYnRuIHtcclxuICAgIHJpZ2h0OiAxMHB4O1xyXG4gICAgdG9wOiA1cHg7XHJcbn1cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsICNteUNhcm91c2VsIGltZyB7XHJcbiAgICBtYXgtaGVpZ2h0OiAxNTBweDtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCBpbWcge1xyXG4gICAgbWF4LWhlaWdodDogMjV2aCAhaW1wb3J0YW50O1xyXG4gICAgbWFyZ2luOiBhdXRvICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyB7XHJcbiAgICBtYXJnaW4tdG9wOiAxMHB4O1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1kZWZhdWx0IHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xyXG4gICAgYm9yZGVyOiAwO1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1oZWFkaW5nIHtcclxuICAgIHBhZGRpbmc6IDVweCAwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiB3aGl0ZTtcclxuICAgIGJvcmRlcjogMDtcclxufVxyXG5cclxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtYm9keSB7XHJcbiAgICBwYWRkaW5nOiA1cHggNXB4O1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1ncm91cCB7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbn1cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLnBhbmVsLWhlYWRpbmcgYSB7XHJcbiAgICBjb2xvcjogIzAwMDAwMDtcclxufVxyXG5cclxuI2J1eWVyLWRhdGF0YWJsZSAuYmdfbm9EbHJfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2U1ZTVlNTtcclxufVxyXG5cclxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAuZGF0ZV90aW1lIHtcclxuICAgIHBhZGRpbmc6IDVweDtcclxufVxyXG5cclxuLnJpZ2h0X3NpZGVfcGFuZWwgLmFzc2V0cy1wYW5lbCB7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IDA7XHJcbiAgICBsZWZ0OiAwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgaGVpZ2h0OiAxMDAlO1xyXG59XHJcblxyXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnMge1xyXG4gICAgb3ZlcmZsb3cteDogYXV0bztcclxuICAgIG92ZXJmbG93LXk6IGhpZGRlbjtcclxuICAgIGRpc3BsYXk6IC13ZWJraXQtYm94O1xyXG4gICAgZGlzcGxheTogLW1vei1ib3g7XHJcbn1cclxuXHJcbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFiczo6LXdlYmtpdC1zY3JvbGxiYXItdGh1bWIge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2UzZTNlMztcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIG9wYWNpdHk6IC4yO1xyXG59XHJcblxyXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyLXRyYWNrIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IHRyYW5zcGFyZW50O1xyXG59XHJcblxyXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyIHtcclxuICAgIHdpZHRoOiA2cHg7XHJcbiAgICBoZWlnaHQ6IDRweDtcclxuICAgIG92ZXJmbG93OiBzY3JvbGw7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiBpbmhlcml0O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG59XHJcblxyXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnMgPiBsaSB7XHJcbiAgICBmbG9hdDogbm9uZTtcclxuICAgIHBhZGRpbmctYm90dG9tOiA1cHg7XHJcbn1cclxuXHJcbi5hc2VldHMtYm9keSAubmF2ID4gbGkuYWN0aXZlIHtcclxuICAgIGJhY2tncm91bmQ6IG5vbmUgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFzZWV0cy1ib2R5IC5uYXYgPiBsaSA+IGEge1xyXG4gICAgcGFkZGluZzogMnB4IDAgIWltcG9ydGFudDtcclxuICAgIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xyXG4gICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNmZmYgIWltcG9ydGFudDtcclxuICAgIG1hcmdpbi1yaWdodDogOHB4O1xyXG59XHJcblxyXG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpLmFjdGl2ZSA+IGEge1xyXG4gICAgcGFkZGluZzogMnB4IDAgIWltcG9ydGFudDtcclxuICAgIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xyXG4gICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICMwYzUyYjEgIWltcG9ydGFudDtcclxuICAgIGNvbG9yOiAjMGM1MmIxO1xyXG59XHJcblxyXG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpID4gYTpob3ZlciB7XHJcbiAgICBib3JkZXItd2lkdGg6IDAgIWltcG9ydGFudDtcclxuICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjMGM1MmIxICFpbXBvcnRhbnQ7XHJcbiAgICBiYWNrZ3JvdW5kOiBub25lICFpbXBvcnRhbnQ7XHJcbiAgICBjb2xvcjogIzg0ODQ4NDtcclxufVxyXG5cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIC5hc2VldHMtYm9keSAudGFiLWNvbnRlbnQge1xyXG4gICAgbWF4LWhlaWdodDogMzEwcHg7XHJcbiAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgb3ZlcmZsb3cteDogaGlkZGVuO1xyXG59XHJcblxyXG4ucmlnaHRfc2lkZV9wYW5lbCAuY2hhcnRzLXBhbmVsIHtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogMDtcclxuICAgIGxlZnQ6IDA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBoZWlnaHQ6IDEwMCU7XHJcbn1cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwgLmNoYXJ0cy1oZWFkZXIge1xyXG4gICAgcGFkZGluZzogMTBweCA1cHg7XHJcbn1cclxuXHJcbi5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwgLmNoYXJ0cy1ib2R5IHtcclxuICAgIG1heC1oZWlnaHQ6IDM0MHB4O1xyXG4gICAgb3ZlcmZsb3cteTogYXV0bztcclxuICAgIG92ZXJmbG93LXg6IGhpZGRlbjtcclxufVxyXG5cclxuLnNob3dfZmlsdGVyIHtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgYmFja2dyb3VuZDogMCAwO1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG4gICAgdG9wOiAwcHg7XHJcbiAgICBsZWZ0OiAwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gICAgbWFyZ2luLXRvcDogNXB4O1xyXG59XHJcblxyXG4jYXNzZXREZXRhaWxzTW9kYWwgLm1vZGFsLWhlYWRlciB7XHJcbiAgICBwYWRkaW5nOiA1cHggMTVweDtcclxuICAgIGJvcmRlci1ib3R0b206IDFweCBzb2xpZCAjZTVlNWU1O1xyXG59XHJcblxyXG4uYXNzZXQtZGV0YWlscyB0ZCB7XHJcbiAgICBwYWRkaW5nOiA0cHggOHB4O1xyXG59XHJcblxyXG4uYXNlZXRzLWJvZHkgLm5hdi10YWJzIHtcclxuICAgIGJvcmRlci1ib3R0b206IDA7XHJcbn1cclxuXHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1mbG9hdGluZyB7XHJcbiAgICB0b3A6IDE3cHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmRpc3BsYXlfaGlkZSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG4gICAgdHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcclxuICAgIG9wYWNpdHk6IDA7XHJcbn1cclxuXHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4gICAgcG9zaXRpb246IGZpeGVkICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1mbG9hdGluZywgdGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWxvY2tlZCB7XHJcbiAgICB0b3A6IDY1cHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcclxuICAgIG1pbi13aWR0aDogMzAwcHg7XHJcbiAgICBtYXgtd2lkdGg6IDM1MHB4O1xyXG4gICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuXHJcbi5hY3RpdmVkaWZmLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICM1ODViZmY7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcbi8qIGJ1eWVyLWxvY2F0aW9uIE1hc3Rlci1GaWx0ZXJzIHN0YXJ0cyBoZXJlKi9cclxuXHJcbi5tYXN0ZXItZmlsdGVyIHtcclxuICAgICYucG9wb3ZlciB7XHJcbiAgICAgICAgbWluLXdpZHRoOiA0MjVweDtcclxuICAgICAgICBtYXgtd2lkdGg6IDQ1MHB4O1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICAgICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgICAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICAgICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuXHJcbiAgICAgICAgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgICAgIC8vIG1heC1oZWlnaHQ6IDM1MHB4O1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy14OiBoaWRkZW47XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDA7XHJcbiAgICAgICAgICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgICAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC5wb3BvdmVyLWRldGFpbHMge1xyXG4gICAgICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgICAgICAgICAvLyBtYXgtaGVpZ2h0OiAzMTBweDtcclxuICAgICAgICAgICAgLy8gb3ZlcmZsb3cteTogYXV0bztcclxuICAgICAgICAgICAgLmZvbnQtYm9sZCB7XHJcbiAgICAgICAgICAgICAgICBmb250LXdlaWdodDogNjAwICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgICAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2U3ZWFlYyAhaW1wb3J0YW50O1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufVxyXG5cclxuLmNpcmNsZS1iYWRnZSB7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IC0xMXB4O1xyXG4gICAgbGVmdDogLTE0cHg7XHJcbiAgICBiYWNrZ3JvdW5kOiByZ2IoMjUwLCAxNDcsIDE0Nyk7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBmb250LXNpemU6IDEycHg7XHJcbiAgICB0ZXh0LWFsaWduOiBjZW50ZXI7XHJcbiAgICBjb2xvcjogd2hpdGU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtZmxleDtcclxuICAgIGFsaWduLWl0ZW1zOiBjZW50ZXI7XHJcbiAgICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcclxuICAgIHdpZHRoOiAxOHB4O1xyXG4gICAgaGVpZ2h0OiAxOHB4XHJcbn1cclxuIiwiLypNb3ZlZCBtYXAgY3NzIHRvIHdhbGx5LWRhc2hib2FyZC5jb21wb25lbnQuY3NzKi9cbi5sb2NhdGlvbmZpbHRlci1pbi1tYXAge1xuICB3aWR0aDogOTAlO1xufVxuXG4uc3RpY2t5LWhlYWRlci1sb2Mge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHJpZ2h0OiAwO1xuICBwYWRkaW5nOiAxNXB4IDVweDtcbiAgdG9wOiA0NXB4O1xuICBoZWlnaHQ6IDY1cHg7XG4gIGZvbnQtc2l6ZTogMjBweDtcbiAgei1pbmRleDogMTA7XG4gIGJhY2tncm91bmQ6ICNmZmY7XG59XG5cbi5sb2NhdGlvbmZpbHRlciB7XG4gIHdpZHRoOiAxMDAlO1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHJpZ2h0OiAtNHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIGZvbnQtc2l6ZTogMTRweDtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwge1xuICBwYWRkaW5nOiAwIDEwcHggMTBweDtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgLmNsb3NlX2J0biB7XG4gIHJpZ2h0OiAxMHB4O1xuICB0b3A6IDVweDtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgI215Q2Fyb3VzZWwgaW1nIHtcbiAgbWF4LWhlaWdodDogMTUwcHg7XG4gIHdpZHRoOiAxMDAlO1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCBpbWcge1xuICBtYXgtaGVpZ2h0OiAyNXZoICFpbXBvcnRhbnQ7XG4gIG1hcmdpbjogYXV0byAhaW1wb3J0YW50O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMge1xuICBtYXJnaW4tdG9wOiAxMHB4O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1kZWZhdWx0IHtcbiAgYmFja2dyb3VuZC1jb2xvcjogd2hpdGU7XG4gIGJvcmRlcjogMDtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtaGVhZGluZyB7XG4gIHBhZGRpbmc6IDVweCAwcHg7XG4gIGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xuICBib3JkZXI6IDA7XG59XG5cbi5yaWdodF9zaWRlX3BhbmVsIC5kcml2ZXJfZGV0YWlscyAuc2l0ZS1zdGF0dXMgLnBhbmVsLWJvZHkge1xuICBwYWRkaW5nOiA1cHggNXB4O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuZHJpdmVyX2RldGFpbHMgLnNpdGUtc3RhdHVzIC5wYW5lbC1ncm91cCB7XG4gIG1hcmdpbi1ib3R0b206IDBweDtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAucGFuZWwtaGVhZGluZyBhIHtcbiAgY29sb3I6ICMwMDAwMDA7XG59XG5cbiNidXllci1kYXRhdGFibGUgLmJnX25vRGxyX2dvIHtcbiAgYmFja2dyb3VuZC1jb2xvcjogI2U1ZTVlNTtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgLmRyaXZlcl9kZXRhaWxzIC5zaXRlLXN0YXR1cyAuZGF0ZV90aW1lIHtcbiAgcGFkZGluZzogNXB4O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuYXNzZXRzLXBhbmVsIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IDA7XG4gIGxlZnQ6IDA7XG4gIGJhY2tncm91bmQ6ICNmZmZmZmY7XG4gIHdpZHRoOiAxMDAlO1xuICBoZWlnaHQ6IDEwMCU7XG59XG5cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFicyB7XG4gIG92ZXJmbG93LXg6IGF1dG87XG4gIG92ZXJmbG93LXk6IGhpZGRlbjtcbiAgZGlzcGxheTogLXdlYmtpdC1ib3g7XG4gIGRpc3BsYXk6IC1tb3otYm94O1xufVxuXG4uYXNzZXRzLXBhbmVsIC5hc2VldHMtYm9keSAubmF2LXRhYnM6Oi13ZWJraXQtc2Nyb2xsYmFyLXRodW1iIHtcbiAgYmFja2dyb3VuZC1jb2xvcjogI2UzZTNlMztcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBvcGFjaXR5OiAwLjI7XG59XG5cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFiczo6LXdlYmtpdC1zY3JvbGxiYXItdHJhY2sge1xuICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcbn1cblxuLmFzc2V0cy1wYW5lbCAuYXNlZXRzLWJvZHkgLm5hdi10YWJzOjotd2Via2l0LXNjcm9sbGJhciB7XG4gIHdpZHRoOiA2cHg7XG4gIGhlaWdodDogNHB4O1xuICBvdmVyZmxvdzogc2Nyb2xsO1xuICBiYWNrZ3JvdW5kLWNvbG9yOiBpbmhlcml0O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG59XG5cbi5hc3NldHMtcGFuZWwgLmFzZWV0cy1ib2R5IC5uYXYtdGFicyA+IGxpIHtcbiAgZmxvYXQ6IG5vbmU7XG4gIHBhZGRpbmctYm90dG9tOiA1cHg7XG59XG5cbi5hc2VldHMtYm9keSAubmF2ID4gbGkuYWN0aXZlIHtcbiAgYmFja2dyb3VuZDogbm9uZSAhaW1wb3J0YW50O1xufVxuXG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpID4gYSB7XG4gIHBhZGRpbmc6IDJweCAwICFpbXBvcnRhbnQ7XG4gIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xuICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2ZmZiAhaW1wb3J0YW50O1xuICBtYXJnaW4tcmlnaHQ6IDhweDtcbn1cblxuLmFzZWV0cy1ib2R5IC5uYXYgPiBsaS5hY3RpdmUgPiBhIHtcbiAgcGFkZGluZzogMnB4IDAgIWltcG9ydGFudDtcbiAgYm9yZGVyLXdpZHRoOiAwICFpbXBvcnRhbnQ7XG4gIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjMGM1MmIxICFpbXBvcnRhbnQ7XG4gIGNvbG9yOiAjMGM1MmIxO1xufVxuXG4uYXNlZXRzLWJvZHkgLm5hdiA+IGxpID4gYTpob3ZlciB7XG4gIGJvcmRlci13aWR0aDogMCAhaW1wb3J0YW50O1xuICBib3JkZXItYm90dG9tOiAycHggc29saWQgIzBjNTJiMSAhaW1wb3J0YW50O1xuICBiYWNrZ3JvdW5kOiBub25lICFpbXBvcnRhbnQ7XG4gIGNvbG9yOiAjODQ4NDg0O1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuYXNlZXRzLWJvZHkgLnRhYi1jb250ZW50IHtcbiAgbWF4LWhlaWdodDogMzEwcHg7XG4gIG92ZXJmbG93LXk6IGF1dG87XG4gIG92ZXJmbG93LXg6IGhpZGRlbjtcbn1cblxuLnJpZ2h0X3NpZGVfcGFuZWwgLmNoYXJ0cy1wYW5lbCB7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAwO1xuICBsZWZ0OiAwO1xuICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xuICB3aWR0aDogMTAwJTtcbiAgaGVpZ2h0OiAxMDAlO1xufVxuXG4ucmlnaHRfc2lkZV9wYW5lbCAuY2hhcnRzLXBhbmVsIC5jaGFydHMtaGVhZGVyIHtcbiAgcGFkZGluZzogMTBweCA1cHg7XG59XG5cbi5yaWdodF9zaWRlX3BhbmVsIC5jaGFydHMtcGFuZWwgLmNoYXJ0cy1ib2R5IHtcbiAgbWF4LWhlaWdodDogMzQwcHg7XG4gIG92ZXJmbG93LXk6IGF1dG87XG4gIG92ZXJmbG93LXg6IGhpZGRlbjtcbn1cblxuLnNob3dfZmlsdGVyIHtcbiAgd2lkdGg6IDEwMCU7XG4gIGJhY2tncm91bmQ6IDAgMDtcbiAgcG9zaXRpb246IHJlbGF0aXZlO1xuICB0b3A6IDBweDtcbiAgbGVmdDogMHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIG9wYWNpdHk6IDE7XG4gIG1hcmdpbi10b3A6IDVweDtcbn1cblxuI2Fzc2V0RGV0YWlsc01vZGFsIC5tb2RhbC1oZWFkZXIge1xuICBwYWRkaW5nOiA1cHggMTVweDtcbiAgYm9yZGVyLWJvdHRvbTogMXB4IHNvbGlkICNlNWU1ZTU7XG59XG5cbi5hc3NldC1kZXRhaWxzIHRkIHtcbiAgcGFkZGluZzogNHB4IDhweDtcbn1cblxuLmFzZWV0cy1ib2R5IC5uYXYtdGFicyB7XG4gIGJvcmRlci1ib3R0b206IDA7XG59XG5cbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1mbG9hdGluZyB7XG4gIHRvcDogMTdweCAhaW1wb3J0YW50O1xufVxuXG4uZGlzcGxheV9oaWRlIHtcbiAgZGlzcGxheTogbm9uZTtcbiAgdHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcbiAgb3BhY2l0eTogMDtcbn1cblxudGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWxvY2tlZCB7XG4gIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xufVxuXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcsIHRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xuICB0b3A6IDY1cHggIWltcG9ydGFudDtcbn1cblxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcbiAgbWluLXdpZHRoOiAzMDBweDtcbiAgbWF4LXdpZHRoOiAzNTBweDtcbiAgYmFja2dyb3VuZDogI0Y5RjlGOTtcbiAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcbiAgYm94LXNpemluZzogYm9yZGVyLWJveDtcbiAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYmEoMCwgMCwgMCwgMC4xMyk7XG4gIGJvcmRlci1yYWRpdXM6IDEwcHg7XG59XG5cbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcbiAgcGFkZGluZzogMTBweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xufVxuXG4uYWN0aXZlZGlmZi1kb3Qge1xuICBoZWlnaHQ6IDEwcHg7XG4gIHdpZHRoOiAxMHB4O1xuICBiYWNrZ3JvdW5kLWNvbG9yOiAjNTg1YmZmO1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcbiAgYW5pbWF0aW9uOiAxcyBibGluayBlYXNlIGluZmluaXRlO1xufVxuXG4vKiBidXllci1sb2NhdGlvbiBNYXN0ZXItRmlsdGVycyBzdGFydHMgaGVyZSovXG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIHtcbiAgbWluLXdpZHRoOiA0MjVweDtcbiAgbWF4LXdpZHRoOiA0NTBweDtcbiAgYmFja2dyb3VuZDogI0Y5RjlGOTtcbiAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcbiAgYm94LXNpemluZzogYm9yZGVyLWJveDtcbiAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYmEoMCwgMCwgMCwgMC4xMyk7XG4gIGJvcmRlci1yYWRpdXM6IDEwcHg7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWJvZHkge1xuICBwYWRkaW5nOiAwO1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIGJhY2tncm91bmQ6ICNmZmZmZmY7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMge1xuICBwYWRkaW5nOiAxNXB4O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5mb250LWJvbGQge1xuICBmb250LXdlaWdodDogNjAwICFpbXBvcnRhbnQ7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xuICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2U3ZWFlYyAhaW1wb3J0YW50O1xufVxuXG4uY2lyY2xlLWJhZGdlIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IC0xMXB4O1xuICBsZWZ0OiAtMTRweDtcbiAgYmFja2dyb3VuZDogI2ZhOTM5MztcbiAgYm9yZGVyLXJhZGl1czogNTAlO1xuICBmb250LXNpemU6IDEycHg7XG4gIHRleHQtYWxpZ246IGNlbnRlcjtcbiAgY29sb3I6IHdoaXRlO1xuICBkaXNwbGF5OiBpbmxpbmUtZmxleDtcbiAgYWxpZ24taXRlbXM6IGNlbnRlcjtcbiAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XG4gIHdpZHRoOiAxOHB4O1xuICBoZWlnaHQ6IDE4cHg7XG59Il19 */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](BuyerLocationsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-buyer-locations',
          templateUrl: './buyer-locations.component.html',
          styleUrls: ['./buyer-locations.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_7__["BuyerwallyboardService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormBuilder"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]
        }];
      }, {
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        locationView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_5__["LocationViewComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/buyer-wally-board.module.ts": function srcAppBuyerWallyBoardBuyerWallyBoardModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "BuyerWallyBoardModule", function () {
      return BuyerWallyBoardModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _wally_dashboard_wally_dashboard_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./wally-dashboard/wally-dashboard.component */
    "./src/app/buyer-wally-board/wally-dashboard/wally-dashboard.component.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _buyer_locations_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./buyer-locations.component */
    "./src/app/buyer-wally-board/buyer-locations.component.ts");
    /* harmony import */


    var _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./where-is-my-driver.component */
    "./src/app/buyer-wally-board/where-is-my-driver.component.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./wally-dashboard/map-view.component */
    "./src/app/buyer-wally-board/wally-dashboard/map-view.component.ts");
    /* harmony import */


    var _wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./wally-dashboard/grid-view.component */
    "./src/app/buyer-wally-board/wally-dashboard/grid-view.component.ts");
    /* harmony import */


    var ng2_charts__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ng2-charts */
    "./node_modules/ng2-charts/__ivy_ngcc__/fesm2015/ng2-charts.js");
    /* harmony import */


    var agm_direction__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! agm-direction */
    "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _sales_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./sales.component */
    "./src/app/buyer-wally-board/sales.component.ts");
    /* harmony import */


    var _sales_data_priority_view_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./sales-data/priority-view.component */
    "./src/app/buyer-wally-board/sales-data/priority-view.component.ts");
    /* harmony import */


    var _sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ./sales-data/tank-view.component */
    "./src/app/buyer-wally-board/sales-data/tank-view.component.ts");
    /* harmony import */


    var _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ./sales-data/location-view.component */
    "./src/app/buyer-wally-board/sales-data/location-view.component.ts");
    /* harmony import */


    var _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ../tank-chart/tank-chart.module */
    "./src/app/tank-chart/tank-chart.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ./sales-data/tank-view-master.component */
    "./src/app/buyer-wally-board/sales-data/tank-view-master.component.ts");

    var routeWallyBoard = [{
      path: '',
      component: _wally_dashboard_wally_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["WallyDashboardComponent"]
    }];

    var BuyerWallyBoardModule = function BuyerWallyBoardModule() {
      _classCallCheck(this, BuyerWallyBoardModule);
    };

    BuyerWallyBoardModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({
      type: BuyerWallyBoardModule
    });
    BuyerWallyBoardModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({
      factory: function BuyerWallyBoardModule_Factory(t) {
        return new (t || BuyerWallyBoardModule)();
      },
      imports: [[_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_9__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_11__["DataTablesModule"], agm_direction__WEBPACK_IMPORTED_MODULE_10__["AgmDirectionModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_16__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_17__["FormsModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(routeWallyBoard)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](BuyerWallyBoardModule, {
        declarations: [_wally_dashboard_wally_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["WallyDashboardComponent"], _buyer_locations_component__WEBPACK_IMPORTED_MODULE_4__["BuyerLocationsComponent"], _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_5__["WhereIsMyDriverComponent"], _wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverMapViewComponent"], _wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverGridViewComponent"], _sales_component__WEBPACK_IMPORTED_MODULE_12__["SalesComponent"], _sales_data_priority_view_component__WEBPACK_IMPORTED_MODULE_13__["PriorityViewComponent"], _sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_14__["TankViewComponent"], _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_15__["LocationViewComponent"], _sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_18__["TankViewMasterComponent"]],
        imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_9__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_11__["DataTablesModule"], agm_direction__WEBPACK_IMPORTED_MODULE_10__["AgmDirectionModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_16__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_17__["FormsModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](BuyerWallyBoardModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_wally_dashboard_wally_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["WallyDashboardComponent"], _buyer_locations_component__WEBPACK_IMPORTED_MODULE_4__["BuyerLocationsComponent"], _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_5__["WhereIsMyDriverComponent"], _wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverMapViewComponent"], _wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverGridViewComponent"], _sales_component__WEBPACK_IMPORTED_MODULE_12__["SalesComponent"], _sales_data_priority_view_component__WEBPACK_IMPORTED_MODULE_13__["PriorityViewComponent"], _sales_data_tank_view_component__WEBPACK_IMPORTED_MODULE_14__["TankViewComponent"], _sales_data_location_view_component__WEBPACK_IMPORTED_MODULE_15__["LocationViewComponent"], _sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_18__["TankViewMasterComponent"]],
          imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_6__["DirectiveModule"], ng2_charts__WEBPACK_IMPORTED_MODULE_9__["ChartsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_11__["DataTablesModule"], agm_direction__WEBPACK_IMPORTED_MODULE_10__["AgmDirectionModule"], _tank_chart_tank_chart_module__WEBPACK_IMPORTED_MODULE_16__["TankChartModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_17__["FormsModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(routeWallyBoard)]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/sales-data/location-view.component.ts": function srcAppBuyerWallyBoardSalesDataLocationViewComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ../../directives/numberWithDecimal */
    "./src/app/directives/numberWithDecimal.ts");

    function LocationViewComponent_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function LocationViewComponent_tr_45_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 36);
      }
    }

    function LocationViewComponent_tr_45_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 37);
      }
    }

    function LocationViewComponent_tr_45_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function LocationViewComponent_tr_45_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r5.PrevSale == "--" ? "Not Available" : row_r5.PrevSale, " ");
      }
    }

    function LocationViewComponent_tr_45_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function LocationViewComponent_tr_45_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r5.WeekAgoSale == "--" ? "Not Available" : row_r5.WeekAgoSale, " ");
      }
    }

    function LocationViewComponent_tr_45_a_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LocationViewComponent_tr_45_a_34_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r19);

          var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r17.openModal(row_r5);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.Status);
      }
    }

    function LocationViewComponent_tr_45_ng_template_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LocationViewComponent_tr_45_ng_template_35_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r23);

          var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r21.showTanks(row_r5);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.Status);
      }
    }

    function LocationViewComponent_tr_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, LocationViewComponent_tr_45_span_9_Template, 1, 0, "span", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, LocationViewComponent_tr_45_span_10_Template, 1, 0, "span", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, LocationViewComponent_tr_45_div_16_Template, 4, 0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, LocationViewComponent_tr_45_div_17_Template, 2, 1, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, LocationViewComponent_tr_45_div_19_Template, 4, 0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, LocationViewComponent_tr_45_div_20_Template, 2, 1, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, LocationViewComponent_tr_45_a_34_Template, 2, 1, "a", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, LocationViewComponent_tr_45_ng_template_35_Template, 2, 1, "ng-template", null, 35, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r5 = ctx.$implicit;

        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](36);

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r5.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5 == null ? null : row_r5.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5.TankInventoryDiffinHrs > 2 || row_r5.TankInventoryDiffinHrs == 0 && ctx_r1.IsShowRetailJobs);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.WaterLevel == "--" ? "Not Available" : row_r5.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.AvgSale == "--" ? "Not Available" : row_r5.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r5.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.Inventory == "--" ? "Not Available" : row_r5.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.LastReadingTime == null || row_r5.LastReadingTime == "--" ? "Not Available" : row_r5.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.Ullage == "--" ? "Not Available" : row_r5.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.LastDeliveredQuantity == "--" ? "Not Available" : row_r5.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.LastDeliveryDate == "--" ? "Not Available" : row_r5.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r5.DaysRemaining == "--" ? "NA" : row_r5.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r5 == null ? null : row_r5.Status) == "Scheduled")("ngIfElse", _r13);
      }
    }

    function LocationViewComponent_ng_container_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainer"](0);
      }
    }

    function LocationViewComponent_ng_template_47_div_9_div_1_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var sqType_r32 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", sqType_r32.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", sqType_r32.Name, " ");
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "show-element": a0
      };
    };

    function LocationViewComponent_ng_template_47_div_9_div_1_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "input", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LocationViewComponent_ng_template_47_div_9_div_1_div_12_Template_input_ngModelChange_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r33.RequiredQuantity = $event;
        })("change", function LocationViewComponent_ng_template_47_div_9_div_1_div_12_Template_input_change_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r35.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx_r31.ScheduleQuantityType > 1 ? true : null)("ngModel", ctx_r31.RequiredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c0, !ctx_r31.isValid));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r31.validateMsg, " ");
      }
    }

    function LocationViewComponent_ng_template_47_div_9_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "h3", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Create DR");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Quantity Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "select", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LocationViewComponent_ng_template_47_div_9_div_1_Template_select_ngModelChange_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r36.ScheduleQuantityType = $event;
        })("change", function LocationViewComponent_ng_template_47_div_9_div_1_Template_select_change_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          ctx_r38.RequiredQuantity = null;
          return ctx_r38.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, LocationViewComponent_ng_template_47_div_9_div_1_option_11_Template, 2, 2, "option", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, LocationViewComponent_ng_template_47_div_9_div_1_div_12_Template, 8, 6, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "input", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LocationViewComponent_ng_template_47_div_9_div_1_Template_input_ngModelChange_18_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r39.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "label", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, " Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "input", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LocationViewComponent_ng_template_47_div_9_div_1_Template_input_ngModelChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r40.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "label", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, " Should Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "input", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LocationViewComponent_ng_template_47_div_9_div_1_Template_input_ngModelChange_26_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r41.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, " Could Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "button", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LocationViewComponent_ng_template_47_div_9_div_1_Template_button_click_30_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r37);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r42.onDrSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, " Create ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r27.ScheduleQuantityType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r27.ScheduleQuantityTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r27.ScheduleQuantityType == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r27.DrPriority)("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r27.DrPriority)("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r27.DrPriority)("value", 3);
      }
    }

    function LocationViewComponent_ng_template_47_div_9_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function LocationViewComponent_ng_template_47_div_9_div_3_tr_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var del_r44 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r44.QuantityTypeId == 0 || del_r44.QuantityTypeId == 1 ? del_r44.Quantity : del_r44.QuantityTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r44.ScheduleDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r44.ScheduleTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r44.Driver);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r44.Carrier);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "show": a0
      };
    };

    function LocationViewComponent_ng_template_47_div_9_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h2", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "button", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, " Existing Delivery Request(s) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "span", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "i", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](9, "i", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "table", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, "Schedule Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21, "Schedule Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, LocationViewComponent_ng_template_47_div_9_div_3_tr_27_Template, 11, 5, "tr", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).modalDetails;

        var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](2, _c1, modalDetails_r25.IsScheduled));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r29.ExistingDeliveries);
      }
    }

    function LocationViewComponent_ng_template_47_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, LocationViewComponent_ng_template_47_div_9_div_1_Template, 32, 9, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, LocationViewComponent_ng_template_47_div_9_div_2_Template, 2, 0, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, LocationViewComponent_ng_template_47_div_9_div_3_Template, 28, 4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().modalDetails;

        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !modalDetails_r25.IsScheduled);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r26.DRLoader);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r26.ExistingDeliveries.length);
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

    function LocationViewComponent_ng_template_47_Template(rf, ctx) {
      if (rf & 1) {
        var _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h3", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "a", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LocationViewComponent_ng_template_47_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r48);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r47.closeModal();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "i", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, LocationViewComponent_ng_template_47_div_9_Template, 4, 3, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r25 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c2, modalDetails_r25.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](6, _c3, modalDetails_r25.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", modalDetails_r25.title, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", modalDetails_r25.display === "block");
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
        this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].MustGo;
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_5__["Subject"]();
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_5__["Subscription"]();
        this.dtOptions = {};
        this.SelectedLocations = [];
        this.SelectedPriorities = [];
        this.SelectedCarriers = [];
        this.IsShowCarrierManaged = false;
        this.SelectedSuppliers = [];
        this.SelectedStatus = [];
        this.IsShowRetailJobs = false;
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
        this.isDataLoaded = false;
        this.ScheduleQuantityTypes = [];
        this.IsFiltersLoaded = false;
        this.SelectedTankRegionId = '';
        this.getJobIdsForMap = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
      }

      _createClass(LocationViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeGrid(); // this.getSalesData();

          this.getScheduleQuantityType();
          this.subscribeFormChanges();
        }
      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this9 = this;

          this.subscriptions.add(this.FilterForm.valueChanges.subscribe(function (change) {
            if (_this9.IsLoadSalesData) {
              var isFilterChanged = _this9.setFilterData(); // if ((isFilterChanged || !this.isDataLoaded) && this.IsFiltersLoaded) {
              //     this.isDataLoaded = true;
              //     this.getSalesData();
              // }

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
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.IsLoadSalesData && change.IsLoadSalesData.currentValue != change.IsLoadSalesData.previousValue) {
            this.IsLoadSalesData = change.IsLoadSalesData.currentValue;
          }

          if (change.IsFiltersLoaded && change.IsFiltersLoaded.currentValue != change.IsFiltersLoaded.previousValue) {
            this.IsFiltersLoaded = change.IsFiltersLoaded.currentValue;
          }
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtTrigger.unsubscribe();
          this.unSubscribeFormChanges();
        }
      }, {
        key: "getScheduleQuantityType",
        value: function getScheduleQuantityType() {
          var _this10 = this;

          this.dispatcherService.GetBuyerScheduleQtyType().subscribe(function (SQT) {
            _this10.ScheduleQuantityTypes = SQT || [];
          });
        }
      }, {
        key: "setFilterData",
        value: function setFilterData() {
          var isFilterChanged = false;
          this.SelectedLocations = this.FilterForm.get('SelectedlocationList').value;
          var ids = [];
          this.SelectedLocations.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedLocationId = ids.join();

          if (this.SelectedLocationId != selectedLocationId) {
            this.SelectedLocationId = selectedLocationId;
            isFilterChanged = true;
          }

          if (this.IsShowCarrierManaged != this.FilterForm.get('IsShowCarrierManaged').value) {
            this.IsShowCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
            isFilterChanged = true;
            this.getSalesData();
          }

          this.SelectedCarriers = this.FilterForm.get('SelectedCarrierList').value;
          ids = [];
          this.SelectedCarriers.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedCarrierIds = ids.join();

          if (this.SelectedCarrierIds != selectedCarrierIds) {
            this.SelectedCarrierIds = selectedCarrierIds;
            isFilterChanged = true;
            this.getSalesData();
          }

          this.SelectedStatus = this.FilterForm.get('SelectedStatusList').value;
          ids = [];
          this.SelectedStatus.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedStatusId = ids.join();

          if (this.SelectedStatusId != selectedStatusId) {
            this.SelectedStatusId = selectedStatusId;
            isFilterChanged = true;
          }

          var selectedLocAttri = this.FilterForm.get('selectedLocAttributeList').value;
          ids = [];
          selectedLocAttri && selectedLocAttri.forEach(function (res) {
            ids.push(res.Id);
          });
          var SelectedLocArributeId = ids.join();

          if (this.SelectedLocArributeId != SelectedLocArributeId) {
            this.SelectedLocArributeId = SelectedLocArributeId;
            isFilterChanged = true;
          }

          this.SelectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;
          ids = [];
          this.SelectedSuppliers.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedSuppliersId = ids.join();

          if (this.SelectedSuppliersId != selectedSuppliersId) {
            this.SelectedSuppliersId = selectedSuppliersId;
            isFilterChanged = true;
          }

          var isShowRetailJobs = !this.FilterForm.get('IsShowAssetJobs').value;

          if (this.IsShowRetailJobs != isShowRetailJobs) {
            this.IsShowRetailJobs = isShowRetailJobs;
            isFilterChanged = true;
            this.getSalesData();
          }

          this.SelectedPriorities = this.FilterForm.get('SelectedPriorityList').value;
          ids = [];
          this.SelectedPriorities.forEach(function (res) {
            ids.push(res.Id);
          });
          var selectedPrioritiesId = ids.join();

          if (this.SelectedPrioritiesId != selectedPrioritiesId) {
            this.SelectedPrioritiesId = selectedPrioritiesId;
            isFilterChanged = true;
          }

          return isFilterChanged;
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
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }]
          };
        }
      }, {
        key: "getSalesData",
        value: function getSalesData() {
          var _this11 = this;

          var inputs = {
            Priority: this.SelectedPrioritiesId,
            LocationId: this.SelectedLocationId,
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["SelectedTabEnum"].Location,
            Carriers: this.SelectedCarrierIds,
            IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
            IsShowRetailJobs: !this.FilterForm.get('IsShowAssetJobs').value,
            Suppliers: this.SelectedSuppliersId,
            InventoryCaptureTypeIds: this.SelectedLocArributeId
          };
          this.IsLoading = true;
          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["forkJoin"])([this.dispatcherService.getBuyerSalesData(inputs), this.dispatcherService.GetRaisedBuyerExceptions()]).subscribe(function (resp) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this11, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
              var _this12 = this;

              return regeneratorRuntime.wrap(function _callee$(_context) {
                while (1) {
                  switch (_context.prev = _context.next) {
                    case 0:
                      _context.next = 2;
                      return resp[0];

                    case 2:
                      _context.t0 = _context.sent;

                      if (!_context.t0) {
                        _context.next = 5;
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
                          return _this12.SelectedStatusId.includes(t.Status);
                        });
                      }

                      this.LocationSchedules = resp[0];
                      this.passJobIdsToMapData();
                      this.IsLoading = false;
                      this.datatableRerender();

                    case 10:
                    case "end":
                      return _context.stop();
                  }
                }
              }, _callee, this);
            }));
          }); // this.dispatcherService.getBuyerSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
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
          var _this13 = this;

          this.resetModal();
          this.SelectedTank = row;
          this.DRLoader = true;
          this.dispatcherService.GetBuyerDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe(function (resp) {
            _this13.ExistingDeliveries = resp;
            _this13.DRLoader = false;
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
          this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].MustGo;
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
          var _this14 = this;

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
            this.isValid = true;
            this.dispatcherService.PostBuyerRaiseDeliveryRequest(raiseDr).subscribe(function (response) {
              if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

                _this14.closeModal();
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
              }

              _this14.closeModal();

              _this14.DRLoader = false;
            });
          }
        }
      }, {
        key: "showTanks",
        value: function showTanks(row) {
          this.SelectedTankRegionId = row.RegionId;
          var salesDataModel = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__["SalesDataModel"]();
          salesDataModel.RegionId = row.RegionId;
          salesDataModel.SiteId = row.SiteId;
          salesDataModel.TankId = row.TankId;
          salesDataModel.StorageId = row.StorageId;
          salesDataModel.TfxJobId = row.TfxJobId;
          salesDataModel.LocationManagedType = row.LocationManagedType ? row.LocationManagedType : null;
          this.dipTestComponent.loadTankDR(salesDataModel);
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
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
        key: "applyLoadsFilters",
        value: function applyLoadsFilters(filterForm) {
          this.FilterForm = filterForm;

          if (this.IsLoadSalesData) {
            var isFilterChanged = this.setFilterData();

            if ((isFilterChanged || !this.isDataLoaded) && this.IsFiltersLoaded) {
              this.getSalesData();
            }
          }
        }
      }]);

      return LocationViewComponent;
    }();

    LocationViewComponent.??fac = function LocationViewComponent_Factory(t) {
      return new (t || LocationViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__["CarrierService"]));
    };

    LocationViewComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: LocationViewComponent,
      selectors: [["app-location-view"]],
      viewQuery: function LocationViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.dipTestComponent = _t.first);
        }
      },
      inputs: {
        FilterForm: "FilterForm",
        IsFiltersLoaded: "IsFiltersLoaded",
        IsLoadSalesData: "IsLoadSalesData"
      },
      outputs: {
        getJobIdsForMap: "getJobIdsForMap"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["????NgOnChangesFeature"]],
      decls: 51,
      vars: 11,
      consts: [[1, "row"], ["id", "grid-view", 1, "col-sm-12"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-Locationmustgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "LT"], ["data-key", "TName"], ["data-key", "WL"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["data-key", "Status"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["schedulesModal", ""], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "RequestFromBuyerWallyBoard", "onRaiseDR"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], [4, "ngIf"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click", 4, "ngIf", "ngIfElse"], ["notSceduledBlock", ""], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click"], ["data-target", "raisedr", "onclick", "slidePanel('#raisedr','60%')", 3, "click"], ["id", "schedulesModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "schedulesModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered", "modal-lg"], [1, "modal-content"], [1, "modal-header", "pt10", "pb5", "no-border"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt5", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body"], ["class", "assets-header", 4, "ngIf"], [1, "assets-header"], ["class", "well bg-white no-shadow border border pr", 4, "ngIf"], [1, "well", "bg-white", "no-shadow", "border", "border", "pr"], [1, "col-sm-12"], [1, "fs14", "font-bold"], [1, "row", "col-sm-12"], [1, "col-sm-3", "input-group"], [1, "form-group", "mb0"], [1, "form-control", 3, "ngModel", "ngModelChange", "change"], [3, "value", 4, "ngFor", "ngForOf"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-6", "mt5"], [1, "col-sm-12", "pa0", "mt5"], [1, "form-check", "form-check-inline"], ["id", "mustgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "mustgo-dr", 1, "form-check-label"], ["id", "shouldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "shouldgo-dr", 1, "form-check-label"], ["id", "couldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "couldgo-dr", 1, "form-check-label"], [1, "col-sm-12", "text-right", "mt10"], ["type", "button", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [3, "value"], [1, "col-sm-3"], [1, "input-group"], ["type", "text", "name", "RequiredQuantity", "numberWithDecimal", "", "required", "", 1, "form-control", 3, "disabled", "ngModel", "ngModelChange", "change"], [1, "invalid-feedback", 3, "ngClass"], ["id", "accordionExitingDrReq", 1, "accordionExitingDrReq", "mt10", "mb10"], [1, "card"], ["id", "headingOne", 1, "card-header", "pt5", "pb5", "pl10", "pr10"], [1, "mb-0"], ["type", "button", "data-toggle", "collapse", "data-target", "#collapseOne", "aria-expanded", "true", "aria-controls", "collapseOne", 1, "d-flex", "align-items-center", "justify-content-between", "btn", "btn-link", "collapsed"], [1, "fa-stack", "fa-sm", "icon-color-b"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-angle-down", "fa-stack-1x", "fa-inverse"], ["id", "collapseOne", "aria-labelledby", "headingOne", "data-parent", "#accordionExitingDrReq", 1, "collapse", 3, "ngClass"], [1, "card-body", "pa5"], [1, "table", "table-hover", "margin", "bottom", "details-table"]],
      template: function LocationViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "Location View");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "table", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "th", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Tank/Asset Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Water Level");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](42, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](44, LocationViewComponent_tr_44_Template, 2, 0, "tr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](45, LocationViewComponent_tr_45_Template, 37, 20, "tr", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](46, LocationViewComponent_ng_container_46_Template, 1, 0, "ng-container", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](47, LocationViewComponent_ng_template_47_Template, 10, 8, "ng-template", null, 26, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "app-dip-test", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onRaiseDR", function LocationViewComponent_Template_app_dip_test_onRaiseDR_50_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        }

        if (rf & 2) {
          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.LocationSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngTemplateOutlet", _r3)("ngTemplateOutletContext", ctx.dsModal);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("isDisableControl", false)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedTankRegionId)("IsThisFromDrDisplay", false)("RequestFromBuyerWallyBoard", true);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgTemplateOutlet"], src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgStyle"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["??angular_packages_forms_forms_x"], _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_13__["NumberWithDecimal"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["RequiredValidator"]],
      styles: [".activediff-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #585bff;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMtZGF0YS9sb2NhdGlvbi12aWV3LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7Ozs7OztFQU1FO0FBQ0Y7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLHlCQUF5QjtJQUN6QixrQkFBa0I7SUFDbEIscUJBQXFCO0lBQ3JCLHlDQUFpQztZQUFqQyxpQ0FBaUM7QUFDckMiLCJmaWxlIjoic3JjL2FwcC9idXllci13YWxseS1ib2FyZC9zYWxlcy1kYXRhL2xvY2F0aW9uLXZpZXcuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi8qdGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWxvY2tlZCB7XHJcbiAgICBwb3NpdGlvbjogZml4ZWQgIWltcG9ydGFudDtcclxufVxyXG5cclxudGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWZsb2F0aW5nLCB0YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItbG9ja2VkIHtcclxuICAgIHRvcDogNjVweCAhaW1wb3J0YW50O1xyXG59Ki9cclxuLmFjdGl2ZWRpZmYtZG90IHtcclxuICAgIGhlaWdodDogMTBweDtcclxuICAgIHdpZHRoOiAxMHB4O1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogIzU4NWJmZjtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuICAgIGFuaW1hdGlvbjogMXMgYmxpbmsgZWFzZSBpbmZpbml0ZTtcclxufSJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](LocationViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-location-view',
          templateUrl: './location-view.component.html',
          styleUrls: ['./location-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__["DispatcherService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_9__["CarrierService"]
        }];
      }, {
        FilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsFiltersLoaded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        IsLoadSalesData: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"]]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"]]
        }],
        getJobIdsForMap: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/sales-data/priority-view.component.ts": function srcAppBuyerWallyBoardSalesDataPriorityViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "PriorityViewComponent", function () {
      return PriorityViewComponent;
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


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/carrier/service/wally-utility.service */
    "./src/app/carrier/service/wally-utility.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ../../directives/numberWithDecimal */
    "./src/app/directives/numberWithDecimal.ts");

    function PriorityViewComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_46_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 40);
      }
    }

    function PriorityViewComponent_tr_46_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 41);
      }
    }

    function PriorityViewComponent_tr_46_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_46_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r10.PrevSale == "--" ? "Not Available" : row_r10.PrevSale, " ");
      }
    }

    function PriorityViewComponent_tr_46_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_46_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r10.WeekAgoSale == "--" ? "Not Available" : row_r10.WeekAgoSale, " ");
      }
    }

    function PriorityViewComponent_tr_46_a_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_46_a_34_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r24);

          var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r22.openModal(row_r10);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.Status);
      }
    }

    function PriorityViewComponent_tr_46_ng_template_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_46_ng_template_35_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r28);

          var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r26.showTanks(row_r10);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.Status);
      }
    }

    function PriorityViewComponent_tr_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PriorityViewComponent_tr_46_span_9_Template, 1, 0, "span", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PriorityViewComponent_tr_46_span_10_Template, 1, 0, "span", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PriorityViewComponent_tr_46_div_16_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, PriorityViewComponent_tr_46_div_17_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PriorityViewComponent_tr_46_div_19_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, PriorityViewComponent_tr_46_div_20_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, PriorityViewComponent_tr_46_a_34_Template, 2, 1, "a", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, PriorityViewComponent_tr_46_ng_template_35_Template, 2, 1, "ng-template", null, 39, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r10 = ctx.$implicit;

        var _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r10.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r10 == null ? null : row_r10.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r10 == null ? null : row_r10.TankInventoryDiffinHrs) > 2 || (row_r10 == null ? null : row_r10.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.WaterLevel == "--" ? "Not Available" : row_r10.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.AvgSale == "--" ? "Not Available" : row_r10.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r10.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r10.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r10.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r10.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.Inventory == "--" ? "Not Available" : row_r10.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.LastReadingTime == null || row_r10.LastReadingTime == "--" ? "Not Available" : row_r10.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.Ullage == "--" ? "Not Available" : row_r10.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.LastDeliveredQuantity == "--" ? "Not Available" : row_r10.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.LastDeliveryDate == "--" ? "Not Available" : row_r10.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r10.DaysRemaining == "--" ? "NA" : row_r10.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r10 == null ? null : row_r10.Status) == "Scheduled")("ngIfElse", _r18);
      }
    }

    function PriorityViewComponent_tr_89_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_90_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 40);
      }
    }

    function PriorityViewComponent_tr_90_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 41);
      }
    }

    function PriorityViewComponent_tr_90_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_90_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r30.PrevSale == "--" ? "Not Available" : row_r30.PrevSale, " ");
      }
    }

    function PriorityViewComponent_tr_90_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_90_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r30.WeekAgoSale == "--" ? "Not Available" : row_r30.WeekAgoSale, " ");
      }
    }

    function PriorityViewComponent_tr_90_a_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_90_a_34_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r44);

          var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r42.openModal(row_r30);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.Status);
      }
    }

    function PriorityViewComponent_tr_90_ng_template_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_90_ng_template_35_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r48);

          var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r46.showTanks(row_r30);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.Status);
      }
    }

    function PriorityViewComponent_tr_90_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PriorityViewComponent_tr_90_span_9_Template, 1, 0, "span", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PriorityViewComponent_tr_90_span_10_Template, 1, 0, "span", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PriorityViewComponent_tr_90_div_16_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, PriorityViewComponent_tr_90_div_17_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PriorityViewComponent_tr_90_div_19_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, PriorityViewComponent_tr_90_div_20_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, PriorityViewComponent_tr_90_a_34_Template, 2, 1, "a", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, PriorityViewComponent_tr_90_ng_template_35_Template, 2, 1, "ng-template", null, 39, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r30 = ctx.$implicit;

        var _r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r30.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r30 == null ? null : row_r30.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r30 == null ? null : row_r30.TankInventoryDiffinHrs) > 2 || (row_r30 == null ? null : row_r30.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.WaterLevel == "--" ? "Not Available" : row_r30.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.AvgSale == "--" ? "Not Available" : row_r30.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r30.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r30.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r30.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r30.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.Inventory == "--" ? "Not Available" : row_r30.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.LastReadingTime == null || row_r30.LastReadingTime == "--" ? "Not Available" : row_r30.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.Ullage == "--" ? "Not Available" : row_r30.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.LastDeliveredQuantity == "--" ? "Not Available" : row_r30.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.LastDeliveryDate == "--" ? "Not Available" : row_r30.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r30.DaysRemaining == "--" ? "NA" : row_r30.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r30 == null ? null : row_r30.Status) == "Scheduled")("ngIfElse", _r38);
      }
    }

    function PriorityViewComponent_tr_133_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_134_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 40);
      }
    }

    function PriorityViewComponent_tr_134_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "span", 41);
      }
    }

    function PriorityViewComponent_tr_134_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_134_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r50.PrevSale == "--" ? "Not Available" : row_r50.PrevSale, " ");
      }
    }

    function PriorityViewComponent_tr_134_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_tr_134_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r50.WeekAgoSale == "--" ? "Not Available" : row_r50.WeekAgoSale, " ");
      }
    }

    function PriorityViewComponent_tr_134_a_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_134_a_34_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64);

          var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r62 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r62.openModal(row_r50);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.Status);
      }
    }

    function PriorityViewComponent_tr_134_ng_template_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r68 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_tr_134_ng_template_35_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r68);

          var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r66.showTanks(row_r50);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.Status);
      }
    }

    function PriorityViewComponent_tr_134_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PriorityViewComponent_tr_134_span_9_Template, 1, 0, "span", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](10, PriorityViewComponent_tr_134_span_10_Template, 1, 0, "span", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, PriorityViewComponent_tr_134_div_16_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, PriorityViewComponent_tr_134_div_17_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](19, PriorityViewComponent_tr_134_div_19_Template, 4, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, PriorityViewComponent_tr_134_div_20_Template, 2, 1, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, PriorityViewComponent_tr_134_a_34_Template, 2, 1, "a", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, PriorityViewComponent_tr_134_ng_template_35_Template, 2, 1, "ng-template", null, 39, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r50 = ctx.$implicit;

        var _r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.InventoryDataCaptureTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", row_r50.TankName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r50 == null ? null : row_r50.IsUnknownOrMissing);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r50 == null ? null : row_r50.TankInventoryDiffinHrs) > 2 || (row_r50 == null ? null : row_r50.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.WaterLevel == "--" ? "Not Available" : row_r50.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.AvgSale == "--" ? "Not Available" : row_r50.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r50.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r50.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r50.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", row_r50.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.Inventory == "--" ? "Not Available" : row_r50.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.LastReadingTime == null || row_r50.LastReadingTime == "--" ? "Not Available" : row_r50.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.Ullage == "--" ? "Not Available" : row_r50.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.LastDeliveredQuantity == "--" ? "Not Available" : row_r50.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.LastDeliveryDate == "--" ? "Not Available" : row_r50.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](row_r50.DaysRemaining == "--" ? "NA" : row_r50.DaysRemaining);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (row_r50 == null ? null : row_r50.Status) == "Scheduled")("ngIfElse", _r58);
      }
    }

    function PriorityViewComponent_ng_container_135_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainer"](0);
      }
    }

    function PriorityViewComponent_ng_template_136_div_9_div_1_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "option", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var sqType_r77 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", sqType_r77.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", sqType_r77.Name, " ");
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "show-element": a0
      };
    };

    function PriorityViewComponent_ng_template_136_div_9_div_1_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r79 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "input", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function PriorityViewComponent_ng_template_136_div_9_div_1_div_12_Template_input_ngModelChange_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r79);

          var ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r78.RequiredQuantity = $event;
        })("change", function PriorityViewComponent_ng_template_136_div_9_div_1_div_12_Template_input_change_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r79);

          var ctx_r80 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

          return ctx_r80.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx_r76.ScheduleQuantityType > 1 ? true : null)("ngModel", ctx_r76.RequiredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c0, !ctx_r76.isValid));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r76.validateMsg, " ");
      }
    }

    function PriorityViewComponent_ng_template_136_div_9_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "h3", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Create DR");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Quantity Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "select", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_select_ngModelChange_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r81 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r81.ScheduleQuantityType = $event;
        })("change", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_select_change_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          ctx_r83.RequiredQuantity = null;
          return ctx_r83.validateMsg = "";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, PriorityViewComponent_ng_template_136_div_9_div_1_option_11_Template, 2, 2, "option", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, PriorityViewComponent_ng_template_136_div_9_div_1_div_12_Template, 8, 6, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "input", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_input_ngModelChange_18_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r84.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, " Must Go");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "input", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_input_ngModelChange_22_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r85.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "label", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, " Should Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "input", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_input_ngModelChange_26_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r86.DrPriority = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "label", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, " Could Go ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "button", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_ng_template_136_div_9_div_1_Template_button_click_30_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r82);

          var ctx_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

          return ctx_r87.onDrSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, " Create ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r72.ScheduleQuantityType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r72.ScheduleQuantityTypes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r72.ScheduleQuantityType == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r72.DrPriority)("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r72.DrPriority)("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r72.DrPriority)("value", 3);
      }
    }

    function PriorityViewComponent_ng_template_136_div_9_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function PriorityViewComponent_ng_template_136_div_9_div_3_tr_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var del_r89 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r89.QuantityTypeId == 0 || del_r89.QuantityTypeId == 1 ? del_r89.Quantity : del_r89.QuantityTypeName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r89.ScheduleDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r89.ScheduleTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r89.Driver);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](del_r89.Carrier);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "show": a0
      };
    };

    function PriorityViewComponent_ng_template_136_div_9_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h2", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "button", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, " Existing Delivery Request(s) ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "span", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "i", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](9, "i", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "table", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, "Schedule Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21, "Schedule Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, PriorityViewComponent_ng_template_136_div_9_div_3_tr_27_Template, 11, 5, "tr", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).modalDetails;

        var ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](2, _c1, modalDetails_r70.IsScheduled));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r74.ExistingDeliveries);
      }
    }

    function PriorityViewComponent_ng_template_136_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, PriorityViewComponent_ng_template_136_div_9_div_1_Template, 32, 9, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, PriorityViewComponent_ng_template_136_div_9_div_2_Template, 2, 0, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, PriorityViewComponent_ng_template_136_div_9_div_3_Template, 28, 4, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().modalDetails;

        var ctx_r71 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !modalDetails_r70.IsScheduled);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r71.DRLoader);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r71.ExistingDeliveries.length);
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

    function PriorityViewComponent_ng_template_136_Template(rf, ctx) {
      if (rf & 1) {
        var _r93 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h3", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "a", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function PriorityViewComponent_ng_template_136_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r93);

          var ctx_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r92.closeModal();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "i", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](9, PriorityViewComponent_ng_template_136_div_9_Template, 4, 3, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r70 = ctx.modalDetails;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c2, modalDetails_r70.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](6, _c3, modalDetails_r70.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", modalDetails_r70.title, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", modalDetails_r70.display === "block");
      }
    }

    var PriorityViewComponent = /*#__PURE__*/function () {
      function PriorityViewComponent(dispatcherService, wallyUtilService) {
        _classCallCheck(this, PriorityViewComponent);

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
        this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].MustGo;
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
        this.SelectedTankRegionId = '';
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
      }

      _createClass(PriorityViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.init();
        }
      }, {
        key: "init",
        value: function init() {
          var _this15 = this;

          this.applyFilterSubscription.push(Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["merge"])(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(function (value) {
            if (_this15.salesTabFilterForm.get('SalesViewType').value == 1) {
              _this15.getSalesData();
            }
          })); //to load data - after second ngOnInit

          if (this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value) {
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
            this.getSalesData();
          }

          this.initializeMustGo();
          this.initializeCouldGo();
          this.initializeShouldGo();
          this.getScheduleQuantityType();
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
            pagingType: 'first_last_numbers',
            fixedHeader: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [{
              targets: 13,
              type: 'null-at-bottom'
            }],
            order: [13]
          };
        }
      }, {
        key: "getScheduleQuantityType",
        value: function getScheduleQuantityType() {
          var _this16 = this;

          this.dispatcherService.GetBuyerScheduleQtyType().subscribe(function (SQT) {
            _this16.ScheduleQuantityTypes = SQT || [];
          });
        }
      }, {
        key: "getSalesDtls",
        value: function getSalesDtls() {
          var _this17 = this;

          var inputs = {
            Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].None,
            LocationId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedlocationList').value),
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["SelectedTabEnum"].Priority,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureTypeIds: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value)
          };
          this.IsShouldGoLoading = true;
          this.IsCouldGoLoading = true;
          this.IsMustGoLoading = true;
          Object(rxjs__WEBPACK_IMPORTED_MODULE_2__["forkJoin"])([this.dispatcherService.getBuyerSalesData(inputs), this.dispatcherService.GetRaisedBuyerExceptions()]).subscribe(function (resp) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this17, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
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
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].ShouldGo;
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
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].CouldGo;
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
                        return t.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].MustGo;
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
          this.IsCouldGoLoading = true;
          this.IsShouldGoLoading = true;
          this.IsMustGoLoading = true;
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
          var _this18 = this;

          this.resetModal();
          this.SelectedTank = row;
          this.DRLoader = true;
          this.dispatcherService.GetBuyerDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe(function (resp) {
            _this18.ExistingDeliveries = resp;
            _this18.DRLoader = false;
          });
          this.dsModal.modalDetails.display = 'block';
          var isSchedule = row.Status == 'Scheduled';
          this.dsModal.modalDetails.IsScheduled = isSchedule;
          this.showDr = isSchedule;
        }
      }, {
        key: "resetModal",
        value: function resetModal() {
          this.ExistingDeliveries = [];
          this.DrPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeliveryReqPriority"].MustGo;
          this.RequiredQuantity = null;
          this.ScheduleQuantityType = 1;
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
          var _this19 = this;

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

          if (this.ScheduleQuantityType == 1 && (this.RequiredQuantity == null || this.RequiredQuantity == 0)) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror("Invalid required quantity.", undefined, undefined);
            this.isValid = false;
          } else if (this.SelectedTank.MaxFillQuantity != null && this.RequiredQuantity > this.SelectedTank.MaxFillQuantity) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror("Quantity Should be less than max fill quantity: " + this.SelectedTank.MaxFillQuantity, undefined, undefined);
            this.isValid = false;
          } else {
            this.DRLoader = true;
            this.isValid = true;
            this.dispatcherService.PostBuyerRaiseDeliveryRequest(raiseDr).subscribe(function (response) {
              if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
              }

              _this19.DRLoader = false;

              _this19.closeModal();

              $(".modal-backdrop").removeClass("show");
              $(".modal-backdrop").addClass("hide");
            });
          }
        }
      }, {
        key: "showTanks",
        value: function showTanks(row) {
          this.SelectedTankRegionId = row.RegionId;
          var salesDataModel = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__["SalesDataModel"]();
          salesDataModel.RegionId = row.RegionId;
          salesDataModel.SiteId = row.SiteId;
          salesDataModel.TankId = row.TankId;
          salesDataModel.StorageId = row.StorageId;
          salesDataModel.TfxJobId = row.TfxJobId;
          salesDataModel.LocationManagedType = row.LocationManagedType ? row.LocationManagedType : null;
          this.dipTestComponent.loadTankDR(salesDataModel);
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }]);

      return PriorityViewComponent;
    }();

    PriorityViewComponent.??fac = function PriorityViewComponent_Factory(t) {
      return new (t || PriorityViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_9__["WallyUtilService"]));
    };

    PriorityViewComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: PriorityViewComponent,
      selectors: [["app-priority-view"]],
      viewQuery: function PriorityViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.dipTestComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 140,
      vars: 20,
      consts: [["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "row", "mt60"], ["id", "grid-view", 1, "col-sm-12"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-salemustgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "LT"], ["data-key", "TName"], ["data-key", "WL"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["data-key", "Status"], [4, "ngFor", "ngForOf"], [1, "shouldgo", "mb5", 2, "color", "#f3c316 !important"], ["id", "table-saleshouldgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [1, "couldgo", "mb5", 2, "color", "#a7a2a2 !important"], ["id", "table-salecouldgo", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["schedulesModal", ""], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "RequestFromBuyerWallyBoard", "onRaiseDR"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], [4, "ngIf"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click", 4, "ngIf", "ngIfElse"], ["notSceduledBlock", ""], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"], ["data-toggle", "modal", "data-target", "#schedulesModal", 3, "click"], ["data-target", "raisedr", "onclick", "slidePanel('#raisedr','60%')", 3, "click"], ["id", "schedulesModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "schedulesModal", "aria-hidden", "true", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-dialog-scrollable", "modal-dialog-centered", "modal-lg"], [1, "modal-content"], [1, "modal-header", "pt10", "pb5", "no-border"], ["id", "assetDetailsModal", 1, "modal-title"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt5", 3, "click"], [1, "fa", "fa-close", "fa-lg"], [1, "modal-body"], ["class", "assets-header", 4, "ngIf"], [1, "assets-header"], ["class", "well bg-white no-shadow border border pr", 4, "ngIf"], [1, "well", "bg-white", "no-shadow", "border", "border", "pr"], [1, "row"], [1, "col-sm-12"], [1, "fs14", "font-bold"], [1, "row", "col-sm-12"], [1, "col-sm-3", "input-group"], [1, "form-group", "mb0"], [1, "form-control", 3, "ngModel", "ngModelChange", "change"], [3, "value", 4, "ngFor", "ngForOf"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-6", "mt5"], [1, "col-sm-12", "pa0", "mt5"], [1, "form-check", "form-check-inline"], ["id", "mustgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "mustgo-dr", 1, "form-check-label"], ["id", "shouldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "shouldgo-dr", 1, "form-check-label"], ["id", "couldgo-dr", "type", "radio", 1, "form-check-input", 3, "ngModel", "value", "ngModelChange"], ["for", "couldgo-dr", 1, "form-check-label"], [1, "col-sm-12", "text-right", "mt10"], ["type", "button", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [3, "value"], [1, "col-sm-3"], [1, "input-group"], ["type", "text", "name", "RequiredQuantity", "numberWithDecimal", "", "required", "", 1, "form-control", 3, "disabled", "ngModel", "ngModelChange", "change"], [1, "invalid-feedback", 3, "ngClass"], ["id", "accordionExitingDrReq", 1, "accordionExitingDrReq", "mt10", "mb10"], [1, "card"], ["id", "headingOne", 1, "card-header", "pt5", "pb5", "pl10", "pr10"], [1, "mb-0"], ["type", "button", "data-toggle", "collapse", "data-target", "#collapseOne", "aria-expanded", "true", "aria-controls", "collapseOne", 1, "d-flex", "align-items-center", "justify-content-between", "btn", "btn-link", "collapsed"], [1, "fa-stack", "fa-sm", "icon-color-b"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-angle-down", "fa-stack-1x", "fa-inverse"], ["id", "collapseOne", "aria-labelledby", "headingOne", "data-parent", "#accordionExitingDrReq", 1, "collapse", 3, "ngClass"], [1, "card-body", "pa5"], [1, "table", "table-hover", "margin", "bottom", "details-table"]],
      template: function PriorityViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](0, PriorityViewComponent_div_0_Template, 2, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "h4", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Must Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "table", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](19, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](21, "Tank Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Water Level");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](27, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](29, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](33, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](35, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](37, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](39, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](41, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](43, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](45, PriorityViewComponent_tr_45_Template, 2, 0, "tr", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](46, PriorityViewComponent_tr_46_Template, 37, 20, "tr", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "h4", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](50, "Should Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "table", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](59, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](61, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](63, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](65, "Tank Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](67, "Water Level");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](69, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](70, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](71, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](72, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](73, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](74, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](75, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](76, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](77, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](78, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](79, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](80, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](81, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](82, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](83, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](84, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](85, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](86, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](87, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](88, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](89, PriorityViewComponent_tr_89_Template, 2, 0, "tr", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](90, PriorityViewComponent_tr_90_Template, 37, 20, "tr", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](91, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](92, "h4", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](93, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](94, "Could Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](95, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](96, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](97, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](98, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](99, "table", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](100, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](101, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](102, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](103, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](104, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](105, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](106, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](107, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](108, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](109, "Tank Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](110, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](111, "Water Level ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](112, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](113, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](114, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](115, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](116, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](117, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](118, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](119, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](120, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](121, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](122, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](123, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](124, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](125, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](126, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](127, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](128, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](129, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](130, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](131, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](132, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](133, PriorityViewComponent_tr_133_Template, 2, 0, "tr", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](134, PriorityViewComponent_tr_134_Template, 37, 20, "tr", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](135, PriorityViewComponent_ng_container_135_Template, 1, 0, "ng-container", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](136, PriorityViewComponent_ng_template_136_Template, 10, 8, "ng-template", null, 30, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](138, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](139, "app-dip-test", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onRaiseDR", function PriorityViewComponent_Template_app_dip_test_onRaiseDR_139_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        }

        if (rf & 2) {
          var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](137);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsMustGoLoading || ctx.IsShouldGoLoading || ctx.IsCouldGoLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtMustGoOptions)("dtTrigger", ctx.dtMustGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsMustGoLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.MustGoSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtShouldGoOptions)("dtTrigger", ctx.dtShouldGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsShouldGoLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.ShouldGoSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtCouldGoOptions)("dtTrigger", ctx.dtCouldGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsCouldGoLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.CouldGoSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngTemplateOutlet", _r8)("ngTemplateOutletContext", ctx.dsModal);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("isDisableControl", false)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedTankRegionId)("IsThisFromDrDisplay", false)("RequestFromBuyerWallyBoard", true);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgTemplateOutlet"], src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgStyle"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["??angular_packages_forms_forms_x"], _directives_numberWithDecimal__WEBPACK_IMPORTED_MODULE_13__["NumberWithDecimal"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["RequiredValidator"]],
      styles: [".active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\r\n\r\n\r\n@keyframes blink {\r\n    from,to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n\r\n\r\n@-webkit-keyframes blink {\r\n    from, to {\r\n        opacity: 0;\r\n    }\r\n\r\n    50% {\r\n        opacity: 1;\r\n    }\r\n}\r\n\r\n\r\ntable.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    position: fixed !important;\r\n}\r\n\r\n\r\ntable.dataTable.fixedHeader-floating[_ngcontent-%COMP%], table.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    top: 65px !important;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMtZGF0YS9wcmlvcml0eS12aWV3LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLHlCQUF5QjtJQUN6QixrQkFBa0I7SUFDbEIscUJBQXFCO0lBQ3JCLHlDQUFpQztZQUFqQyxpQ0FBaUM7QUFDckM7OztBQUdBO0lBQ0k7UUFDSSxVQUFVO0lBQ2Q7O0lBRUE7UUFDSSxVQUFVO0lBQ2Q7QUFDSjs7O0FBWUE7SUFDSTtRQUNJLFVBQVU7SUFDZDs7SUFFQTtRQUNJLFVBQVU7SUFDZDtBQUNKOzs7QUFzQkE7SUFDSSwwQkFBMEI7QUFDOUI7OztBQUVBO0lBQ0ksb0JBQW9CO0FBQ3hCIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMtZGF0YS9wcmlvcml0eS12aWV3LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuYWN0aXZlLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmZjU4NTg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcblxyXG5Aa2V5ZnJhbWVzIGJsaW5rIHtcclxuICAgIGZyb20sdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALW1vei1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALXdlYmtpdC1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG5ALW1zLWtleWZyYW1lcyBibGluayB7XHJcbiAgICBmcm9tLCB0byB7XHJcbiAgICAgICAgb3BhY2l0eTogMDtcclxuICAgIH1cclxuXHJcbiAgICA1MCUge1xyXG4gICAgICAgIG9wYWNpdHk6IDE7XHJcbiAgICB9XHJcbn1cclxuXHJcbkAtby1rZXlmcmFtZXMgYmxpbmsge1xyXG4gICAgZnJvbSwgdG8ge1xyXG4gICAgICAgIG9wYWNpdHk6IDA7XHJcbiAgICB9XHJcblxyXG4gICAgNTAlIHtcclxuICAgICAgICBvcGFjaXR5OiAxO1xyXG4gICAgfVxyXG59XHJcblxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItbG9ja2VkIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG50YWJsZS5kYXRhVGFibGUuZml4ZWRIZWFkZXItZmxvYXRpbmcsIHRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4gICAgdG9wOiA2NXB4ICFpbXBvcnRhbnQ7XHJcbn0iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](PriorityViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-priority-view',
          templateUrl: './priority-view.component.html',
          styleUrls: ['./priority-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_8__["DispatcherService"]
        }, {
          type: src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_9__["WallyUtilService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"]]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_6__["DipTestComponent"]]
        }],
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/sales-data/tank-view-master.component.ts": function srcAppBuyerWallyBoardSalesDataTankViewMasterComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _tank_view_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./tank-view.component */
    "./src/app/buyer-wally-board/sales-data/tank-view.component.ts");
    /* harmony import */


    var _shared_components_forcasting_tank_view_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../shared-components/forcasting/tank-view-component */
    "./src/app/shared-components/forcasting/tank-view-component.ts");

    function TankViewMasterComponent_app_buyertank_view_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-buyertank-view", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("salesTabFilterForm", ctx_r0.salesTabFilterForm);
      }
    }

    function TankViewMasterComponent_app_forecasting_tank_view_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-forecasting-tank-view", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("salesTabFilterForm", ctx_r1.salesTabFilterForm)("RequestFromBuyerWallyBoard", true);
      }
    }

    var TankViewMasterComponent = /*#__PURE__*/function () {
      function TankViewMasterComponent() {
        _classCallCheck(this, TankViewMasterComponent);
      }

      _createClass(TankViewMasterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }]);

      return TankViewMasterComponent;
    }();

    TankViewMasterComponent.??fac = function TankViewMasterComponent_Factory(t) {
      return new (t || TankViewMasterComponent)();
    };

    TankViewMasterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: TankViewMasterComponent,
      selectors: [["app-tank-view-master"]],
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 10,
      vars: 3,
      consts: [[1, "col-sm-12", 3, "formGroup"], [1, "row", "mt60"], [1, "col-sm-12", "text-center"], [1, "custom-control", "custom-switch", "mb10"], ["type", "checkbox", "id", "chk-consumptionrate", "name", "chkRateOfConsumption", "formControlName", "RateOfConsumption", 1, "custom-control-input"], ["for", "chk-consumptionrate", 1, "custom-control-label"], [3, "salesTabFilterForm", 4, "ngIf"], [3, "salesTabFilterForm", "RequestFromBuyerWallyBoard", 4, "ngIf"], [3, "salesTabFilterForm"], [3, "salesTabFilterForm", "RequestFromBuyerWallyBoard"]],
      template: function TankViewMasterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Rate Of Consumption");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, TankViewMasterComponent_app_buyertank_view_8_Template, 2, 1, "app-buyertank-view", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, TankViewMasterComponent_app_forecasting_tank_view_9_Template, 2, 2, "app-forecasting-tank-view", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.salesTabFilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx.salesTabFilterForm.get("RateOfConsumption").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.salesTabFilterForm.get("RateOfConsumption").value);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"], _tank_view_component__WEBPACK_IMPORTED_MODULE_3__["TankViewComponent"], _shared_components_forcasting_tank_view_component__WEBPACK_IMPORTED_MODULE_4__["ForcastingTankViewComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2J1eWVyLXdhbGx5LWJvYXJkL3NhbGVzLWRhdGEvdGFuay12aWV3LW1hc3Rlci5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](TankViewMasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-tank-view-master',
          templateUrl: './tank-view-master.component.html',
          styleUrls: ['./tank-view-master.component.css']
        }]
      }], function () {
        return [];
      }, {
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/sales-data/tank-view.component.ts": function srcAppBuyerWallyBoardSalesDataTankViewComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankViewComponent", function () {
      return TankViewComponent;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationTankDetailsModel", function () {
      return LocationTankDetailsModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TankDetailModel", function () {
      return TankDetailModel;
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


    var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/shared-components/dip-test/dip-test.component */
    "./src/app/shared-components/dip-test/dip-test.component.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
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


    var _tank_chart_tank_chart_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../../tank-chart/tank-chart.component */
    "./src/app/tank-chart/tank-chart.component.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../../directives/sorting.pipe */
    "./src/app/directives/sorting.pipe.ts");

    function TankViewComponent_div_6_Template(rf, ctx) {
      if (rf & 1) {
        var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "i", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "input", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("input", function TankViewComponent_div_6_Template_input_input_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r6);

          var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r5.Partsfiltering($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function TankViewComponent_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "No Location Available");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function TankViewComponent_div_8_ng_container_12_tr_25_span_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "span", 59);
      }
    }

    function TankViewComponent_div_8_ng_container_12_tr_25_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "span", 60);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "active": a0
      };
    };

    function TankViewComponent_div_8_ng_container_12_tr_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "a", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TankViewComponent_div_8_ng_container_12_tr_25_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r15);

          var tank_r11 = ctx.$implicit;

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

          return ctx_r14.tankChange(tank_r11);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, TankViewComponent_div_8_ng_container_12_tr_25_span_4_Template, 1, 0, "span", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, TankViewComponent_div_8_ng_container_12_tr_25_span_5_Template, 1, 0, "span", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "td", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "a", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TankViewComponent_div_8_ng_container_12_tr_25_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r15);

          var loc_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r16.showTanks(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "span", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var tank_r11 = ctx.$implicit;

        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](6, _c0, ctx_r10.SelectedTankId == (tank_r11 == null ? null : tank_r11.TankId) + "_" + (tank_r11 == null ? null : tank_r11.StorageId)));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", tank_r11.Name, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", tank_r11 == null ? null : tank_r11.IsUnknowDeliveryOrMissDelivery);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (tank_r11 == null ? null : tank_r11.TankInventoryDiffinHrs) > 2 || (tank_r11 == null ? null : tank_r11.TankInventoryDiffinHrs) == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", tank_r11.DaysRemaining == null ? "NA" : tank_r11.DaysRemaining + " Days", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](tank_r11.Status);
      }
    }

    function TankViewComponent_div_8_ng_container_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "h2", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TankViewComponent_div_8_ng_container_12_Template_div_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r19);

          var loc_r8 = ctx.$implicit;

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r18.locationChange(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](8, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](9, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "span", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](11, "i", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](12, "i", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "a", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TankViewComponent_div_8_ng_container_12_Template_a_click_16_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r19);

          var loc_r8 = ctx.$implicit;

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r20.showTanks(loc_r8);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "span", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "tr", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "td", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "ul", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "table", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](25, TankViewComponent_div_8_ng_container_12_tr_25_Template, 12, 8, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var loc_r8 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????attribute"]("id", "headingOne" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????propertyInterpolate2"]("ngbTooltip", "", loc_r8.LocationName, "", loc_r8 && loc_r8.CustomerInfo ? " - " + loc_r8.CustomerInfo : null, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????attribute"]("data-target", "#col" + (loc_r8 == null ? null : loc_r8.JobId))("aria-controls", "col" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate3"](" ", loc_r8 == null ? null : loc_r8.LocationName, " ", loc_r8 && loc_r8.CustomerInfo && loc_r8.CustomerInfo.length > 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind3"](8, 13, loc_r8.CustomerInfo, 0, 5) + "..)" : "", " ", loc_r8 && loc_r8.CustomerInfo && loc_r8.CustomerInfo.length <= 5 ? "(" + _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind3"](9, 17, loc_r8.CustomerInfo, 0, 5) + ")" : "", " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](loc_r8.DaysRemaining == null ? "NA" : loc_r8.DaysRemaining + " Days");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](loc_r8.Status);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????attribute"]("id", "col" + (loc_r8 == null ? null : loc_r8.JobId))("aria-labelledby", "headingOne" + (loc_r8 == null ? null : loc_r8.JobId));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", loc_r8 == null ? null : loc_r8.Tanks);
      }
    }

    function TankViewComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "table", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "th", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "th", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TankViewComponent_div_8_Template_th_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r22);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r21.setSortArgs("DaysRemaining");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, " Days remaining\xA0");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "i", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, " Status ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, TankViewComponent_div_8_ng_container_12_Template, 26, 21, "ng-container", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipe"](13, "sortingPipe");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pipeBind2"](13, 1, ctx_r2.FilterLocationDrpDwnList, ctx_r2.filterArgs));
      }
    }

    function TankViewComponent_tr_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "span", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function TankViewComponent_tr_47_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "span", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function TankViewComponent_tr_47_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", row_r23.PrevSale, " ");
      }
    }

    function TankViewComponent_tr_47_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Not Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "span", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function TankViewComponent_tr_47_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", row_r23.WeekAgoSale, " ");
      }
    }

    function TankViewComponent_tr_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, TankViewComponent_tr_47_div_12_Template, 4, 0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, TankViewComponent_tr_47_div_13_Template, 2, 1, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, TankViewComponent_tr_47_div_15_Template, 4, 0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, TankViewComponent_tr_47_div_16_Template, 2, 1, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var row_r23 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.LocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.Location);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.TankName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.WaterLevel);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.AvgSale);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", row_r23.PrevSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", row_r23.PrevSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", row_r23.WeekAgoSale == "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", row_r23.WeekAgoSale != "NA*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.Inventory);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.LastReadingTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.Ullage);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.LastDeliveredQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.LastDeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](row_r23.DaysRemaining == null ? "NA" : row_r23.DaysRemaining + " Days");
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
        this.applyFilterSubscription = [];
        this.LocationSchedules = [];
        this.CloneLocationSchedules = [];
        this.LocationDrpDwnList = [];
        this.FilterLocationDrpDwnList = [];
        this.IsLoading = false;
        this.IsLocDrpDwnLoading = false;
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.dtOptions = {};
        this.filterArgs = {
          key: "DaysRemaining",
          asc: true
        };
      }

      _createClass(TankViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this20 = this;

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
          }; //  this.initLocationDropDown();

          this.applyFilterSubscription.push(Object(rxjs__WEBPACK_IMPORTED_MODULE_1__["merge"])(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(function (value) {
            _this20.initLocationDropDown();
          })); //to load data - after in ngOnInit

          if (this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value) {
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
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
          var _this21 = this;

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
          Object(rxjs__WEBPACK_IMPORTED_MODULE_1__["forkJoin"])([this.dispatcherService.getBuyerLocationTanks(filter), this.dispatcherService.GetRaisedBuyerExceptions()]).subscribe(function (result) {
            _this21.IsLocDrpDwnLoading = false;
            _this21.LocationDrpDwnList = result[0];

            _this21.Partsfiltering();

            _this21.LocationDrpDwnList && _this21.LocationDrpDwnList.length > 0 ? _this21.locationChange(_this21.LocationDrpDwnList[0]) : '';

            if (_this21.LocationDrpDwnList && _this21.LocationDrpDwnList.length > 0) {
              _this21.LocationDrpDwnList.forEach(function (loc) {
                loc && loc.Tanks && loc.Tanks.length > 0 && loc.Tanks.forEach(function (m) {
                  if (result[1] && result[1].filter(function (f) {
                    return f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == loc.SiteId && f.TankDetail.StorageId == m.StorageId;
                  }).length > 0) m.IsUnknowDeliveryOrMissDelivery = true;else m.IsUnknowDeliveryOrMissDelivery = false;
                });
              });
            } else {
              _this21.SelectedTankId = null;
              _this21.LocationSchedules = [];
              _this21.CloneLocationSchedules = [];
              _this21.SelectedLocationId = '0';
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
          } else this.LocationSchedules = [];
        }
      }, {
        key: "getSalesData",
        value: function getSalesData() {
          var _this22 = this;

          var inputs = {
            Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["DeliveryReqPriority"].None,
            LocationId: this.SelectedLocationId,
            SelectedTab: src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["SelectedTabEnum"].Tanks,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureTypeIds: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value)
          };
          this.IsLoading = true;
          this.dispatcherService.getBuyerSalesData(inputs).subscribe(function (resp) {
            _this22.LocationSchedules = resp;
            _this22.CloneLocationSchedules = resp;
            _this22.IsLoading = false;
            _this22.LocationSchedules && _this22.LocationSchedules.map(function (m) {
              try {
                _this22.FilterLocationDrpDwnList && _this22.FilterLocationDrpDwnList.filter(function (f) {
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

            _this22.datatableRerender();
          });
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          var _this23 = this;

          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();

              _this23.dtTrigger.next();
            });
          }
        }
      }, {
        key: "Partsfiltering",
        value: function Partsfiltering(inputName) {
          var _this24 = this;

          this.FilterLocationDrpDwnList = [];

          if (inputName && inputName.target && inputName.target.value && inputName.target.value.trim() != '') {
            var searchWord = inputName.target.value.toUpperCase();
            this.LocationDrpDwnList.forEach(function (element) {
              if (element.LocationName.toUpperCase().indexOf(searchWord) !== -1) {
                _this24.FilterLocationDrpDwnList.push(element);
              }
            });
          } else {
            this.FilterLocationDrpDwnList = this.LocationDrpDwnList;
          }
        }
      }, {
        key: "showTanks",
        value: function showTanks(location) {
          if (location && location.RegionId) {
            this.SelectedRegionId = location.RegionId;
            var salesDataModel = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_3__["SalesDataModel"]();
            salesDataModel.RegionId = location.RegionId;
            salesDataModel.SiteId = location.SiteId;
            salesDataModel.TankId = location.TankId;
            salesDataModel.StorageId = location.StorageId;
            salesDataModel.TfxJobId = parseInt(location.JobId);
            salesDataModel.LocationManagedType = location.LocationManagedType ? location.LocationManagedType : null;
            this.dipTestComponent.loadTankDR(salesDataModel);
          } else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror("Location not assigned to region. Contact your supplier", undefined, undefined);
            this.closeSidePanel();
          }
        }
      }, {
        key: "closeSidePanel",
        value: function closeSidePanel() {
          closeSlidePanel();
        }
      }]);

      return TankViewComponent;
    }();

    TankViewComponent.??fac = function TankViewComponent_Factory(t) {
      return new (t || TankViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_8__["WallyUtilService"]));
    };

    TankViewComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: TankViewComponent,
      selectors: [["app-buyertank-view"]],
      viewQuery: function TankViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.dipTestComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        salesTabFilterForm: "salesTabFilterForm"
      },
      decls: 50,
      vars: 19,
      consts: [[1, "row"], [1, "col-sm-4"], [1, "well", "bg-white", "shadow-b", "location-panel"], ["id", "accordion-location", 1, "location-accordion"], [1, "position-abs", "text-center", 3, "ngClass"], [1, "spinner-small", "ml10", "mt5"], ["class", "mb10", 4, "ngIf"], [4, "ngIf"], [1, "col-sm-8", "location-chart-panel"], [1, "well", "bg-white", "shadow-b"], [3, "JobId", "SiteId", "TankId", "isSupplierView"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-Location", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "LocName"], ["data-key", "Loc"], ["data-key", "TName"], ["data-key", "WL"], ["data-key", "Avg7Day"], ["data-key", "PDS"], ["data-key", "SaleWeek"], ["data-key", "CI"], ["data-key", "LastReadingTime"], ["data-key", "Ullg"], ["data-key", "lastDeliveryQty"], ["data-key", "lastDelivery"], ["data-key", "DRemg"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], ["id", "create-dip-test"], [3, "isDisableControl", "IsSalesPage", "SelectedRegionId", "IsThisFromDrDisplay", "RequestFromBuyerWallyBoard", "onRaiseDR"], [1, "mb10"], [1, "inner-addon", "left-addon"], [1, "glyphicon", "glyphicon-search"], ["name", "txtSearch", "placeholder", "Search Location", "type", "text", "autocomplete", "off", 1, "form-control", 3, "input"], [1, "table", "tank-view"], ["width", "45%"], ["width", "24%", 1, "cursor_pointer", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-sort"], [1, "card-header"], [1, "mb-0"], ["data-toggle", "collapse", "aria-expanded", "true", 1, "position-relative", "pr-3", "btn", "btn-link", "collapsed", "text-left", "position-relative", "pr-3", 3, "ngbTooltip", "click"], [1, "fa-stack", "fa-sm", "icon-color-b", "position-absolute", 2, "top", "3px", "right", "-7px"], [1, "fas", "fa-circle", "fa-stack-2x"], [1, "fas", "fa-plus", "fa-stack-1x", "fa-inverse"], ["href", "javascript:void(0)", "onclick", "slidePanel('#raisedr','60%')", 1, "", 3, "click"], [1, ""], ["data-parent", "#accordion-location", 1, "collapse"], ["colspan", "3"], [1, "card-body"], [1, "list-group", "list-group-flush"], [1, "table", "tank-view-child"], ["width", "49%"], ["href", "javascript:void(0)", 3, "ngClass", "click"], ["class", "active-dot", 4, "ngIf"], ["title", "Tank Inventory Alert", "class", "activediff-dot", 4, "ngIf"], ["width", "24%"], [1, "active-dot"], ["title", "Tank Inventory Alert", 1, "activediff-dot"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["placement", "top", "ngbTooltip", "Deliveries are missing!"]],
      template: function TankViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](6, TankViewComponent_div_6_Template, 4, 0, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](7, TankViewComponent_div_7_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, TankViewComponent_div_8_Template, 14, 4, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](11, "app-tank-chart", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "table", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "Tank Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Water Level");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "Trailing 7 Day Average");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Previous Day Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "Week Ago Sale");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "Last Inventory Reading");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, "Last Reading Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38, "Ullage");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](40, "Last Delivered Qty");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](42, "Last Delivered On");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](43, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](44, "Days Remaining");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](46, TankViewComponent_tr_46_Template, 2, 0, "tr", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](47, TankViewComponent_tr_47_Template, 29, 15, "tr", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "app-dip-test", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onRaiseDR", function TankViewComponent_Template_app_dip_test_onRaiseDR_49_listener() {
            return ctx.closeSidePanel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](17, _c1, !ctx.IsLocDrpDwnLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx.IsLocDrpDwnLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx.IsLocDrpDwnLoading && ctx.FilterLocationDrpDwnList && ctx.FilterLocationDrpDwnList.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("JobId", ctx.SelectedLocationId)("SiteId", ctx.SelectedSiteId)("TankId", ctx.SelectedTankId)("isSupplierView", false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.LocationSchedules);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("isDisableControl", true)("IsSalesPage", true)("SelectedRegionId", ctx.SelectedRegionId)("IsThisFromDrDisplay", false)("RequestFromBuyerWallyBoard", true);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _tank_chart_tank_chart_component__WEBPACK_IMPORTED_MODULE_10__["TankChartComponent"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_11__["NgbTooltip"]],
      pipes: [_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_12__["SortingPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["SlicePipe"]],
      styles: [".location-panel[_ngcontent-%COMP%] {\r\n    height: calc(100vh - 130px);\r\n    max-height: calc(100vh - 120px);\r\n    overflow-y: auto;\r\n}\r\n.location-chart-panel[_ngcontent-%COMP%] {\r\n    max-height: calc(100vh - 120px);\r\n    overflow-y: auto;\r\n    margin-right:-20px;\r\n}\r\n.tank-view.table[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]:first-child   td[_ngcontent-%COMP%]{\r\n    border-top: 0px solid #e7eaec;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]    > tbody[_ngcontent-%COMP%]    > tr[_ngcontent-%COMP%]    > td[_ngcontent-%COMP%]{\r\n    padding: 4px 8px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]{\r\n    margin-bottom: 5px;\r\n}\r\n.table.tank-view-child[_ngcontent-%COMP%]   .active[_ngcontent-%COMP%]{\r\n    font-weight: 700;\r\n    color: brown;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%], .location-accordion[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:last-child   .card-header[_ngcontent-%COMP%] {\r\n    border: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%] {\r\n    background: transparent;\r\n    border-bottom: 0;\r\n    padding: 0;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-header[_ngcontent-%COMP%]:hover {\r\n    background: #e2e0ff5e !important;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .card-body[_ngcontent-%COMP%] {\r\n    \r\n    padding: 0px;\r\n    margin-left: -8px;\r\n    margin-right: -8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .fa-stack[_ngcontent-%COMP%] {\r\n    font-size: 8px;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    border: 0;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%] {\r\n    background-color: transparent;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item.active[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\r\n    color: #616161;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\r\n    width: 100%;\r\n    color: #004987;\r\n    padding: 0;\r\n}\r\n.icon-color-b[_ngcontent-%COMP%] {\r\n    color: #1062d1;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:hover, .location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%]:focus {\r\n    text-decoration: none;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .btn-link[_ngcontent-%COMP%] {\r\n    color: #616161 !important;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .list-group-item[_ngcontent-%COMP%] {\r\n    padding: 3px 20px;\r\n}\r\n.bg-change[_ngcontent-%COMP%] {\r\n    background: #e2e0ff5e !important;\r\n    font-weight: bold;\r\n}\r\n.location-accordion[_ngcontent-%COMP%]   .active-dot[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: #ff5858;\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n    -webkit-animation: 1s blink ease infinite;\r\n            animation: 1s blink ease infinite;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMtZGF0YS90YW5rLXZpZXcuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLDJCQUEyQjtJQUMzQiwrQkFBK0I7SUFDL0IsZ0JBQWdCO0FBQ3BCO0FBQ0E7SUFDSSwrQkFBK0I7SUFDL0IsZ0JBQWdCO0lBQ2hCLGtCQUFrQjtBQUN0QjtBQUdBO0lBQ0ksZ0JBQWdCO0FBQ3BCO0FBRUE7SUFDSSw2QkFBNkI7QUFDakM7QUFDQTtJQUNJLGdCQUFnQjtBQUNwQjtBQUNBO0lBQ0ksa0JBQWtCO0FBQ3RCO0FBQ0E7SUFDSSxnQkFBZ0I7SUFDaEIsWUFBWTtBQUNoQjtBQUVBOztJQUVJLFlBQVk7QUFDaEI7QUFFQTtJQUNJLHVCQUF1QjtJQUN2QixnQkFBZ0I7SUFDaEIsVUFBVTtBQUNkO0FBRUE7SUFDSSxnQ0FBZ0M7QUFDcEM7QUFFQTtJQUNJLGtCQUFrQjtJQUNsQixZQUFZO0lBQ1osaUJBQWlCO0lBQ2pCLGtCQUFrQjtBQUN0QjtBQUVBO0lBQ0ksY0FBYztBQUNsQjtBQUVBO0lBQ0ksU0FBUztBQUNiO0FBRUE7SUFDSSw2QkFBNkI7QUFDakM7QUFFQTtJQUNJLGNBQWM7SUFDZCxpQkFBaUI7QUFDckI7QUFFQTtJQUNJLGNBQWM7QUFDbEI7QUFFQTtJQUNJLFdBQVc7SUFDWCxjQUFjO0lBQ2QsVUFBVTtBQUNkO0FBRUE7SUFDSSxjQUFjO0FBQ2xCO0FBRUE7O0lBRUkscUJBQXFCO0FBQ3pCO0FBR0E7SUFDSSx5QkFBeUI7QUFDN0I7QUFFQTtJQUNJLGlCQUFpQjtBQUNyQjtBQUVBO0lBQ0ksZ0NBQWdDO0lBQ2hDLGlCQUFpQjtBQUNyQjtBQUVBO0lBQ0ksWUFBWTtJQUNaLFdBQVc7SUFDWCx5QkFBeUI7SUFDekIsa0JBQWtCO0lBQ2xCLHFCQUFxQjtJQUNyQix5Q0FBaUM7WUFBakMsaUNBQWlDO0FBQ3JDIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMtZGF0YS90YW5rLXZpZXcuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5sb2NhdGlvbi1wYW5lbCB7XHJcbiAgICBoZWlnaHQ6IGNhbGMoMTAwdmggLSAxMzBweCk7XHJcbiAgICBtYXgtaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTIwcHgpO1xyXG4gICAgb3ZlcmZsb3cteTogYXV0bztcclxufVxyXG4ubG9jYXRpb24tY2hhcnQtcGFuZWwge1xyXG4gICAgbWF4LWhlaWdodDogY2FsYygxMDB2aCAtIDEyMHB4KTtcclxuICAgIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICBtYXJnaW4tcmlnaHQ6LTIwcHg7XHJcbn1cclxuXHJcblxyXG4udGFuay12aWV3LnRhYmxlID4gdGJvZHkgPiB0ciA+IHRke1xyXG4gICAgcGFkZGluZzogNHB4IDhweDtcclxufVxyXG5cclxuLnRhYmxlLnRhbmstdmlldy1jaGlsZCA+IHRib2R5ID4gdHI6Zmlyc3QtY2hpbGQgdGR7XHJcbiAgICBib3JkZXItdG9wOiAwcHggc29saWQgI2U3ZWFlYztcclxufVxyXG4udGFibGUudGFuay12aWV3LWNoaWxkID4gdGJvZHkgPiB0ciA+IHRke1xyXG4gICAgcGFkZGluZzogNHB4IDhweDtcclxufVxyXG4udGFibGUudGFuay12aWV3LWNoaWxke1xyXG4gICAgbWFyZ2luLWJvdHRvbTogNXB4O1xyXG59XHJcbi50YWJsZS50YW5rLXZpZXctY2hpbGQgLmFjdGl2ZXtcclxuICAgIGZvbnQtd2VpZ2h0OiA3MDA7XHJcbiAgICBjb2xvcjogYnJvd247XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQsXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmNhcmQ6bGFzdC1jaGlsZCAuY2FyZC1oZWFkZXIge1xyXG4gICAgYm9yZGVyOiBub25lO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWhlYWRlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiB0cmFuc3BhcmVudDtcclxuICAgIGJvcmRlci1ib3R0b206IDA7XHJcbiAgICBwYWRkaW5nOiAwO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5jYXJkLWhlYWRlcjpob3ZlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZTJlMGZmNWUgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAuY2FyZC1ib2R5IHtcclxuICAgIC8qIHBhZGRpbmc6IDVweDsgKi9cclxuICAgIHBhZGRpbmc6IDBweDtcclxuICAgIG1hcmdpbi1sZWZ0OiAtOHB4O1xyXG4gICAgbWFyZ2luLXJpZ2h0OiAtOHB4O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5mYS1zdGFjayB7XHJcbiAgICBmb250LXNpemU6IDhweDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtIHtcclxuICAgIGJvcmRlcjogMDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtLmFjdGl2ZSB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAubGlzdC1ncm91cC1pdGVtLmFjdGl2ZSBhIHtcclxuICAgIGNvbG9yOiAjMTA2MmQxO1xyXG4gICAgZm9udC13ZWlnaHQ6IGJvbGQ7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmxpc3QtZ3JvdXAtaXRlbSBhIHtcclxuICAgIGNvbG9yOiAjNjE2MTYxO1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4ge1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBjb2xvcjogIzAwNDk4NztcclxuICAgIHBhZGRpbmc6IDA7XHJcbn1cclxuXHJcbi5pY29uLWNvbG9yLWIge1xyXG4gICAgY29sb3I6ICMxMDYyZDE7XHJcbn1cclxuXHJcbi5sb2NhdGlvbi1hY2NvcmRpb24gLmJ0bi1saW5rOmhvdmVyLFxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4tbGluazpmb2N1cyB7XHJcbiAgICB0ZXh0LWRlY29yYXRpb246IG5vbmU7XHJcbn1cclxuXHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5idG4tbGluayB7XHJcbiAgICBjb2xvcjogIzYxNjE2MSAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4ubG9jYXRpb24tYWNjb3JkaW9uIC5saXN0LWdyb3VwLWl0ZW0ge1xyXG4gICAgcGFkZGluZzogM3B4IDIwcHg7XHJcbn1cclxuXHJcbi5iZy1jaGFuZ2Uge1xyXG4gICAgYmFja2dyb3VuZDogI2UyZTBmZjVlICFpbXBvcnRhbnQ7XHJcbiAgICBmb250LXdlaWdodDogYm9sZDtcclxufVxyXG5cclxuLmxvY2F0aW9uLWFjY29yZGlvbiAuYWN0aXZlLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmZjU4NTg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn0iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](TankViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-buyertank-view',
          templateUrl: './tank-view.component.html',
          styleUrls: ['./tank-view.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_7__["DispatcherService"]
        }, {
          type: src_app_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_8__["WallyUtilService"]
        }];
      }, {
        salesTabFilterForm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        dipTestComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_4__["DipTestComponent"]]
        }]
      });
    })();

    var LocationTankDetailsModel = function LocationTankDetailsModel() {
      _classCallCheck(this, LocationTankDetailsModel);
    };

    var TankDetailModel = function TankDetailModel() {
      _classCallCheck(this, TankDetailModel);
    };
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/sales.component.ts": function srcAppBuyerWallyBoardSalesComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesComponent", function () {
      return SalesComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./Models/BuyerWallyBoard */
    "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _app_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var _app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
    /* harmony import */


    var _carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../carrier/service/wally-utility.service */
    "./src/app/carrier/service/wally-utility.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _sales_data_priority_view_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./sales-data/priority-view.component */
    "./src/app/buyer-wally-board/sales-data/priority-view.component.ts");
    /* harmony import */


    var _sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./sales-data/tank-view-master.component */
    "./src/app/buyer-wally-board/sales-data/tank-view-master.component.ts");

    function SalesComponent_div_16_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "ng-multiselect-dropdown", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onSelect", function SalesComponent_div_16_ng_template_3_Template_ng_multiselect_dropdown_onSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r10);

          var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r9.SaveFilters(true);
        })("onDeSelect", function SalesComponent_div_16_ng_template_3_Template_ng_multiselect_dropdown_onDeSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r10);

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r11.SaveFilters(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("settings", ctx_r8.CarrierDdlSettings)("placeholder", "Select Carrier")("data", ctx_r8.Carriers);
      }
    }

    function SalesComponent_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "a", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, SalesComponent_div_16_ng_template_3_Template, 2, 3, "ng-template", null, 18, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r7)("autoClose", "outside");
      }
    }

    function SalesComponent_span_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r2.filterCount);
      }
    }

    function SalesComponent_ng_template_23_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "label", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "ng-multiselect-dropdown", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onSelect", function SalesComponent_ng_template_23_div_2_Template_ng_multiselect_dropdown_onSelect_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r14);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r13.locationChanged();
        })("onDeSelect", function SalesComponent_ng_template_23_div_2_Template_ng_multiselect_dropdown_onDeSelect_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r14);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r15.locationChanged();
        })("onSelectAll", function SalesComponent_ng_template_23_div_2_Template_ng_multiselect_dropdown_onSelectAll_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r14);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r16.locationChanged();
        })("onDeSelectAll", function SalesComponent_ng_template_23_div_2_Template_ng_multiselect_dropdown_onDeSelectAll_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r14);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r17.locationChanged();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("settings", ctx_r12.LocationDdlSettings)("placeholder", "Select Location")("data", ctx_r12.Locations);
      }
    }

    function SalesComponent_ng_template_23_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](2, SalesComponent_ng_template_23_div_2_Template, 5, 3, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "label", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "ng-multiselect-dropdown", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "button", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function SalesComponent_ng_template_23_Template_button_click_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r19);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          ctx_r18.ResetFilters();
          return ctx_r18.SaveFilters(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "button", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function SalesComponent_ng_template_23_Template_button_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r19);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](19);

          ctx_r20.ApplyFilters("set");
          ctx_r20.SaveFilters(false);
          return _r1.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](13, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx_r4.salesTabFilterForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r4.salesTabFilterForm.get("SalesViewType").value != 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", ctx_r4.salesTabFilterForm.get("SalesViewType").value != 2 ? "col-6" : "col-8");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("settings", ctx_r4.LocationDdlSettings)("placeholder", "Inventory Capture Method")("data", ctx_r4.LocationAttributeList);
      }
    }

    function SalesComponent_app_priority_view_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-priority-view", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("salesTabFilterForm", ctx_r5.salesTabFilterForm);
      }
    }

    function SalesComponent_app_tank_view_master_28_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-tank-view-master", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("TriggerFilter", function SalesComponent_app_tank_view_master_28_Template_app_tank_view_master_TriggerFilter_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r22);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r21.triggerFilter($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("salesTabFilterForm", ctx_r6.salesTabFilterForm);
      }
    }

    var SalesComponent = /*#__PURE__*/function () {
      function SalesComponent(dispatcherService, wallyUtilService) {
        _classCallCheck(this, SalesComponent);

        this.dispatcherService = dispatcherService;
        this.wallyUtilService = wallyUtilService;
        this.filterCount = 0;
        this.CarrierDdlSettings = {};
        this.LocationDdlSettings = {};
        this.toogleFilter = false;
        this.Locations = [];
        this.Carriers = [];
        this.LoadPriorities = _app_constants__WEBPACK_IMPORTED_MODULE_3__["LoadPriorities"];
        this.LocationAttributeList = _app_constants__WEBPACK_IMPORTED_MODULE_3__["InventoryDataCaptureList"];
        this.salesTabFilterForm = this.wallyUtilService.getSalesTabFilterForm();
      }

      _createClass(SalesComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
          this.GetFilters();
        }
      }, {
        key: "getCarrierLocations",
        value: function getCarrierLocations() {
          var _this25 = this;

          this.dispatcherService.GetFilterData(this.salesTabFilterForm.get('IsShowCarrierManaged').value).subscribe(function (data) {
            _this25.Locations = data;
            _this25.Carriers = _this25.GetUniqueItems(data.map(function (t) {
              return t.Carriers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
          });
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "locationChanged",
        value: function locationChanged(event) {
          this.salesTabFilterForm.get('SelectedCarriers').setValue([]);
          this.setJobSuppliers();
        }
      }, {
        key: "setJobSuppliers",
        value: function setJobSuppliers() {
          var _this26 = this;

          this.Carriers = [];
          var SelectedLocations = this.salesTabFilterForm.get('SelectedlocationList').value;
          this.Locations.map(function (m) {
            if (SelectedLocations.find(function (f) {
              return f.Id == m.Id;
            }) || SelectedLocations.length == 0) {
              if (m && m.Carriers && m.Carriers.length > 0) {
                _this26.Carriers = _this26.Carriers.concat(m.Carriers);
              }
            }
          });
          this.Carriers = this.GetUniqueItems(this.Carriers.reduce(function (p, n) {
            return p.concat(n);
          }, []));
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
        key: "ShowCarrierMangedData",
        value: function ShowCarrierMangedData() {
          this.salesTabFilterForm.get('SelectedCarriers').setValue([]);
          this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
          this.getCarrierLocations();
          this.ApplyFilters();
          this.SaveFilters(true);
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this27 = this;

          this.dispatcherService.getFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].BuyerWallyboardSales).subscribe(function (res) {
            if (res && res.length > 0) {
              _this27.SetFilters(res);
            } else {
              _this27.getCarrierLocations();

              _this27.ApplyFilters();
            }
          });
        }
      }, {
        key: "SaveFilters",
        value: function SaveFilters(isTopFilter) {
          var _this28 = this;

          this.dispatcherService.getFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].BuyerWallyboardSales).subscribe(function (res) {
            if (res) {
              var filterData = {};
              var input = JSON.parse(res);

              if (isTopFilter) {
                filterData['isShowCarrierManaged'] = _this28.salesTabFilterForm.get('IsShowCarrierManaged').value;
                filterData['SelectedCarriers'] = _this28.salesTabFilterForm.get('SelectedCarriers').value;

                if (input.SelectedLocations && input.SelectedLocations.length > 0) {
                  filterData['SelectedLocations'] = input.SelectedLocations || [];
                }

                if (input.selectedLocAttributeList && input.selectedLocAttributeList.length > 0) {
                  filterData['selectedLocAttributeList'] = input.selectedLocAttributeList || [];
                }
              } else {
                filterData['SelectedLocations'] = _this28.salesTabFilterForm.get('SelectedlocationList').value || [];
                filterData['selectedLocAttributeList'] = _this28.salesTabFilterForm.get('SelectedLocAttributeList').value || [];
                filterData['IsShowCarrierManaged'] = _this28.salesTabFilterForm.get('IsShowCarrierManaged').value;

                if (input.SelectedCarriers && input.SelectedCarriers.length > 0) {
                  filterData['SelectedCarriers'] = input.SelectedCarriers || [];
                }
              }

              _this28.dispatcherService.saveFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_1__["TfxModule"].BuyerWallyboardSales, JSON.stringify(filterData)).subscribe();
            }
          });
        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          var filter = JSON.parse(input);
          this.salesTabFilterForm.get('IsShowCarrierManaged').setValue(filter.isShowCarrierManaged);
          this.salesTabFilterForm.get('SelectedCarriers').setValue(filter.SelectedCarriers);
          this.salesTabFilterForm.get('SelectedlocationList').setValue(filter.SelectedLocations);
          this.salesTabFilterForm.get('SelectedLocAttributeList').setValue(filter.selectedLocAttributeList);
          this.getCarrierLocations();
          this.ApplyFilters();
        }
      }, {
        key: "ResetFilters",
        value: function ResetFilters() {
          this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
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
          var SelectedLocations = this.salesTabFilterForm.get('SelectedlocationList').value;
          var selectedLocAttributeList = this.salesTabFilterForm.get('SelectedLocAttributeList').value;

          if (this.salesTabFilterForm.get('SalesViewType').value == _app_enum__WEBPACK_IMPORTED_MODULE_4__["SalesTabViewType"].PriorityTab) {
            this.filterCount += SelectedLocations.length;
            this.filterCount += selectedLocAttributeList.length;
          } else if (this.salesTabFilterForm.get('SalesViewType').value == _app_enum__WEBPACK_IMPORTED_MODULE_4__["SalesTabViewType"].TanksTab) {
            this.filterCount += selectedLocAttributeList.length;
          }
        }
      }, {
        key: "triggerFilter",
        value: function triggerFilter($event) {
          if ($event) this.GetFilters();
        }
      }, {
        key: "salesViewTypeChanged",
        value: function salesViewTypeChanged(type) {
          this.salesTabFilterForm.get('SalesViewType').setValue(type);
          this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(true);
          this.setFilterCount();
        }
      }]);

      return SalesComponent;
    }();

    SalesComponent.??fac = function SalesComponent_Factory(t) {
      return new (t || SalesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_5__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_6__["WallyUtilService"]));
    };

    SalesComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: SalesComponent,
      selectors: [["app-sales"]],
      decls: 29,
      vars: 13,
      consts: [[1, "col-sm-9", "sticky-header-sales"], [1, "row"], [1, "col-2"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "pull-left", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-6", 3, "formGroup"], [1, "form-check", "form-check-inline", "fs14", "mt10"], ["type", "checkbox", "id", "inlineCarrierManaged", "name", "IsShowCarrierManaged", "formControlName", "IsShowCarrierManaged", 1, "form-check-input", 3, "change"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["class", "mtm5", 4, "ngIf"], [1, "col-sm-4", "pl0", "text-right", "pt8"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], ["popContent", ""], [1, "col-sm-12"], [3, "salesTabFilterForm", 4, "ngIf"], [3, "salesTabFilterForm", "TriggerFilter", 4, "ngIf"], [1, "mtm5"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "p-0"], ["formControlName", "SelectedCarriers", 3, "settings", "placeholder", "data", "onSelect", "onDeSelect"], [1, "circle-badge"], [1, "popover-details", 3, "formGroup"], [1, "row", "border-bottom-2"], ["class", "col-6 pr-0", 4, "ngIf"], [1, "", 3, "ngClass"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], ["formControlName", "SelectedLocAttributeList", 3, "settings", "placeholder", "data"], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"], [1, "col-6", "pr-0"], ["formControlName", "SelectedlocationList", 3, "settings", "placeholder", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [3, "salesTabFilterForm"], [3, "salesTabFilterForm", "TriggerFilter"]],
      template: function SalesComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function SalesComponent_Template_label_click_6_listener() {
            return ctx.salesViewTypeChanged(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Priority");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function SalesComponent_Template_label_click_9_listener() {
            return ctx.salesViewTypeChanged(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "Tanks");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function SalesComponent_Template_input_change_13_listener() {
            return ctx.ShowCarrierMangedData();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](15, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, SalesComponent_div_16_Template, 5, 2, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "a", 14, 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function SalesComponent_Template_a_click_18_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r23);

            var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](19);

            return _r1.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "i", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](21, SalesComponent_span_21_Template, 2, 1, "span", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](23, SalesComponent_ng_template_23_Template, 14, 6, "ng-template", null, 18, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](27, SalesComponent_app_priority_view_27_Template, 2, 1, "app-priority-view", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](28, SalesComponent_app_tank_view_master_28_Template, 2, 1, "app-tank-view-master", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "saletype")("value", 1)("checked", ctx.salesTabFilterForm.get("SalesViewType").value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "saletype")("value", 2)("checked", ctx.salesTabFilterForm.get("SalesViewType").value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.salesTabFilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.salesTabFilterForm.get("IsShowCarrierManaged").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r3)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.filterCount > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.salesTabFilterForm.get("SalesViewType").value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.salesTabFilterForm.get("SalesViewType").value == 2);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_9__["NgbPopover"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["MultiSelectComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgClass"], _sales_data_priority_view_component__WEBPACK_IMPORTED_MODULE_11__["PriorityViewComponent"], _sales_data_tank_view_master_component__WEBPACK_IMPORTED_MODULE_12__["TankViewMasterComponent"]],
      styles: [".sticky-header-sales {\n  position: fixed;\n  right: 0;\n  padding: 10px 20px;\n  top: 45px;\n  height: 65px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n}\n\n.locationfilter {\n  width: 100%;\n  position: absolute;\n  right: 4px;\n  border-radius: 5px;\n  font-size: 14px;\n  z-index: 1010;\n}\n\n.sticky_header {\n  position: sticky;\n  top: 45px;\n  padding: 5px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n  margin-bottom: 0px;\n  margin-top: -10px;\n  box-shadow: 0 3px 15px 0 rgba(0, 0, 0, 0.1);\n  border-radius: 2px;\n}\n\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.carrier-popover.popover .popover-body {\n  padding: 10px;\n  border-radius: 5px;\n}\n\n.activediff-dot {\n  height: 10px;\n  width: 10px;\n  background-color: #585bff;\n  border-radius: 50%;\n  display: inline-block;\n  -webkit-animation: 1s blink ease infinite;\n          animation: 1s blink ease infinite;\n}\n\n/*Master Filter Popper starts here*/\n\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvRDpcXFRGU2NvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlXFxTaXRlRnVlbC5FeGNoYW5nZS5Tb3VyY2VDb2RlXFxTaXRlRnVlbC5FeGNoYW5nZS5XZWIvc3JjXFxhcHBcXGJ1eWVyLXdhbGx5LWJvYXJkXFxzYWxlcy5jb21wb25lbnQuc2NzcyIsInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvc2FsZXMuY29tcG9uZW50LnNjc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7RUFDSSxlQUFBO0VBQ0EsUUFBQTtFQUNBLGtCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0FDQ0o7O0FERUE7RUFDSSxXQUFBO0VBQ0Esa0JBQUE7RUFDQSxVQUFBO0VBQ0Esa0JBQUE7RUFDQSxlQUFBO0VBQ0EsYUFBQTtBQ0NKOztBREVBO0VBRUksZ0JBQUE7RUFDQSxTQUFBO0VBQ0EsWUFBQTtFQUNBLGVBQUE7RUFDQSxXQUFBO0VBQ0EsZ0JBQUE7RUFDQSxrQkFBQTtFQUNBLGlCQUFBO0VBQ0EsMkNBQUE7RUFDQSxrQkFBQTtBQ0NKOztBREdBO0VBQ0ksZ0JBQUE7RUFDQSxnQkFBQTtFQUNBLG1CQUFBO0VBQ0EseUJBQUE7RUFDQSxzQkFBQTtFQUNBLGtEQUFBO0VBQ0EsbUJBQUE7QUNBSjs7QURHQTtFQUNJLGFBQUE7RUFDQSxrQkFBQTtBQ0FKOztBREdBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSx5QkFBQTtFQUNBLGtCQUFBO0VBQ0EscUJBQUE7RUFDQSx5Q0FBQTtVQUFBLGlDQUFBO0FDQUo7O0FER0EsbUNBQUE7O0FBR0k7RUFDSSxnQkFBQTtFQUNBLGdCQUFBO0VBQ0EsbUJBQUE7RUFDQSx5QkFBQTtFQUNBLHNCQUFBO0VBQ0Esa0RBQUE7RUFDQSxtQkFBQTtBQ0ZSOztBRElRO0VBSUksVUFBQTtFQUNBLGtCQUFBO0VBQ0EsbUJBQUE7QUNMWjs7QURRUTtFQUNJLGFBQUE7QUNOWjs7QURTWTtFQUNJLDJCQUFBO0FDUGhCOztBRFdRO0VBQ0ksMkNBQUE7QUNUWjs7QURjQTtFQUNJLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtFQUNBLG9CQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtFQUNBLFdBQUE7RUFDQSxZQUFBO0FDWEoiLCJmaWxlIjoic3JjL2FwcC9idXllci13YWxseS1ib2FyZC9zYWxlcy5jb21wb25lbnQuc2NzcyIsInNvdXJjZXNDb250ZW50IjpbIi5zdGlja3ktaGVhZGVyLXNhbGVzIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgcGFkZGluZzogMTBweCAyMHB4O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgZm9udC1zaXplOiAyMHB4O1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcblxyXG4ubG9jYXRpb25maWx0ZXIge1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICByaWdodDogNHB4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgei1pbmRleDogMTAxMDtcclxufVxyXG5cclxuLnN0aWNreV9oZWFkZXIge1xyXG4gICAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xyXG4gICAgcG9zaXRpb246IHN0aWNreTtcclxuICAgIHRvcDogNDVweDtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMjBweDtcclxuICAgIHotaW5kZXg6IDEwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxuICAgIG1hcmdpbi1ib3R0b206IDBweDtcclxuICAgIG1hcmdpbi10b3A6IC0xMHB4O1xyXG4gICAgYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwwLDAsLjEpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMnB4O1xyXG59XHJcblxyXG5cclxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcclxuICAgIG1pbi13aWR0aDogMzAwcHg7XHJcbiAgICBtYXgtd2lkdGg6IDM1MHB4O1xyXG4gICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuXHJcbi5hY3RpdmVkaWZmLWRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICM1ODViZmY7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XHJcbn1cclxuXHJcbi8qTWFzdGVyIEZpbHRlciBQb3BwZXIgc3RhcnRzIGhlcmUqL1xyXG5cclxuLm1hc3Rlci1maWx0ZXIge1xyXG4gICAgJi5wb3BvdmVyIHtcclxuICAgICAgICBtaW4td2lkdGg6IDQyNXB4O1xyXG4gICAgICAgIG1heC13aWR0aDogNDUwcHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgICAgICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xyXG4gICAgICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG5cclxuICAgICAgICAucG9wb3Zlci1ib2R5IHtcclxuICAgICAgICAgICAgLy8gbWF4LWhlaWdodDogMzUwcHg7XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXg6IGhpZGRlbjtcclxuICAgICAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICAgICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgICAgICAgICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLnBvcG92ZXItZGV0YWlscyB7XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDE1cHg7XHJcbiAgICAgICAgICAgIC8vIG1heC1oZWlnaHQ6IDMxMHB4O1xyXG4gICAgICAgICAgICAvLyBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgICAgICAgICAuZm9udC1ib2xkIHtcclxuICAgICAgICAgICAgICAgIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLmJvcmRlci1ib3R0b20tMiB7XHJcbiAgICAgICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59XHJcblxyXG4uY2lyY2xlLWJhZGdlIHtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogLTExcHg7XHJcbiAgICBsZWZ0OiAtMTRweDtcclxuICAgIGJhY2tncm91bmQ6IHJnYigyNTAsIDE0NywgMTQ3KTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGZvbnQtc2l6ZTogMTJweDtcclxuICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgIGNvbG9yOiB3aGl0ZTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1mbGV4O1xyXG4gICAgYWxpZ24taXRlbXM6IGNlbnRlcjtcclxuICAgIGp1c3RpZnktY29udGVudDogY2VudGVyO1xyXG4gICAgd2lkdGg6IDE4cHg7XHJcbiAgICBoZWlnaHQ6IDE4cHhcclxufSIsIi5zdGlja3ktaGVhZGVyLXNhbGVzIHtcbiAgcG9zaXRpb246IGZpeGVkO1xuICByaWdodDogMDtcbiAgcGFkZGluZzogMTBweCAyMHB4O1xuICB0b3A6IDQ1cHg7XG4gIGhlaWdodDogNjVweDtcbiAgZm9udC1zaXplOiAyMHB4O1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbn1cblxuLmxvY2F0aW9uZmlsdGVyIHtcbiAgd2lkdGg6IDEwMCU7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgcmlnaHQ6IDRweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBmb250LXNpemU6IDE0cHg7XG4gIHotaW5kZXg6IDEwMTA7XG59XG5cbi5zdGlja3lfaGVhZGVyIHtcbiAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xuICBwb3NpdGlvbjogc3RpY2t5O1xuICB0b3A6IDQ1cHg7XG4gIHBhZGRpbmc6IDVweDtcbiAgZm9udC1zaXplOiAyMHB4O1xuICB6LWluZGV4OiAxMDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbiAgbWFyZ2luLWJvdHRvbTogMHB4O1xuICBtYXJnaW4tdG9wOiAtMTBweDtcbiAgYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwgMCwgMCwgMC4xKTtcbiAgYm9yZGVyLXJhZGl1czogMnB4O1xufVxuXG4uY2Fycmllci1wb3BvdmVyLnBvcG92ZXIge1xuICBtaW4td2lkdGg6IDMwMHB4O1xuICBtYXgtd2lkdGg6IDM1MHB4O1xuICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xuICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xuICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiYSgwLCAwLCAwLCAwLjEzKTtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbn1cblxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWJvZHkge1xuICBwYWRkaW5nOiAxMHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG59XG5cbi5hY3RpdmVkaWZmLWRvdCB7XG4gIGhlaWdodDogMTBweDtcbiAgd2lkdGg6IDEwcHg7XG4gIGJhY2tncm91bmQtY29sb3I6ICM1ODViZmY7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xuICBhbmltYXRpb246IDFzIGJsaW5rIGVhc2UgaW5maW5pdGU7XG59XG5cbi8qTWFzdGVyIEZpbHRlciBQb3BwZXIgc3RhcnRzIGhlcmUqL1xuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciB7XG4gIG1pbi13aWR0aDogNDI1cHg7XG4gIG1heC13aWR0aDogNDUwcHg7XG4gIGJhY2tncm91bmQ6ICNGOUY5Rjk7XG4gIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2JhKDAsIDAsIDAsIDAuMTMpO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcbiAgcGFkZGluZzogMDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xuICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcbiAgcGFkZGluZzogMTVweDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAuZm9udC1ib2xkIHtcbiAgZm9udC13ZWlnaHQ6IDYwMCAhaW1wb3J0YW50O1xufVxuLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAuYm9yZGVyLWJvdHRvbS0yIHtcbiAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNlN2VhZWMgIWltcG9ydGFudDtcbn1cblxuLmNpcmNsZS1iYWRnZSB7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAtMTFweDtcbiAgbGVmdDogLTE0cHg7XG4gIGJhY2tncm91bmQ6ICNmYTkzOTM7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgZm9udC1zaXplOiAxMnB4O1xuICB0ZXh0LWFsaWduOiBjZW50ZXI7XG4gIGNvbG9yOiB3aGl0ZTtcbiAgZGlzcGxheTogaW5saW5lLWZsZXg7XG4gIGFsaWduLWl0ZW1zOiBjZW50ZXI7XG4gIGp1c3RpZnktY29udGVudDogY2VudGVyO1xuICB3aWR0aDogMThweDtcbiAgaGVpZ2h0OiAxOHB4O1xufSJdfQ== */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](SalesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-sales',
          templateUrl: './sales.component.html',
          styleUrls: ['./sales.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_5__["BuyerwallyboardService"]
        }, {
          type: _carrier_service_wally_utility_service__WEBPACK_IMPORTED_MODULE_6__["WallyUtilService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/wally-dashboard/grid-view.component.ts": function srcAppBuyerWallyBoardWallyDashboardGridViewComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
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
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function WhereIsMyDriverGridViewComponent_tr_136_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var member_r2 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r2.nickname);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r2.userId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r2.connectionStatus);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r2.lastSeenAt);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "hide-element": a0
      };
    };

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
        this.MustGoSchedules = [];
        this.ShouldGoSchedules = [];
        this.CouldGoSchedules = [];
        this.selectedDriverLoads = [];
        this.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"]();
        this.Locations = [];
        this.States = [];
        this.Suppliers = [];
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.RegionDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleFilter = false;
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
        this.activePriorityTab = 1;
        this.DeliveryReqPriority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"];
        this.showMustGo = false;
        this.showShouldGo = false;
        this.showCouldGo = false;
      }

      _createClass(WhereIsMyDriverGridViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this29 = this;

          this.readOnlyModeSelection();
          this.subscribeFormChanges();
          var exportColumns = {
            columns: ':visible'
          };
          var mustGocolumnsDetails = [];
          var shouldGocolumnsDetails = [];
          var couldGocolumnsDetails = [];
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

              _this29.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Id);
              });

              var inputs = {
                LocationIds: _this29.getLocationIds(),
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].MustGo,
                FromDate: _this29.FilterForm.get('FromDate').value,
                ToDate: _this29.FilterForm.get('ToDate').value,
                DriverSearch: _this29.SearchedKeyword,
                SupplierCompanyIds: _this29.getSupplierIds(),
                CarrierCompanyIds: _this29.getCarrierIds(),
                IsShowCarrierManaged: _this29.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _this29.getselectedLocAttributeIds()
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this29.IsMustGoLoading = true;

              _this29.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe(function (resp) {
                _this29.MustGoSchedules = resp.data;
                _this29.IsMustGoLoading = false;
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

              _this29.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Id);
              });

              var inputs = {
                LocationIds: _this29.getLocationIds(),
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].ShouldGo,
                FromDate: _this29.FilterForm.get('FromDate').value,
                ToDate: _this29.FilterForm.get('ToDate').value,
                DriverSearch: _this29.SearchedKeyword,
                SupplierCompanyIds: _this29.getSupplierIds(),
                CarrierCompanyIds: _this29.getCarrierIds(),
                IsShowCarrierManaged: _this29.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _this29.getselectedLocAttributeIds()
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this29.IsShouldGoLoading = true;

              _this29.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe(function (resp) {
                _this29.ShouldGoSchedules = resp.data;
                _this29.IsShouldGoLoading = false;
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

              _this29.FilterForm.get('SelectedStates').value.forEach(function (x) {
                return _states.push(x.Id);
              });

              var inputs = {
                LocationIds: _this29.getLocationIds(),
                States: _states,
                Priority: src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["DeliveryReqPriority"].CouldGo,
                FromDate: _this29.FilterForm.get('FromDate').value,
                ToDate: _this29.FilterForm.get('ToDate').value,
                DriverSearch: _this29.SearchedKeyword,
                SupplierCompanyIds: _this29.getSupplierIds(),
                CarrierCompanyIds: _this29.getCarrierIds(),
                IsShowCarrierManaged: _this29.FilterForm.get('IsShowCarrierManaged').value,
                InventoryCaptureType: _this29.getselectedLocAttributeIds()
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this29.IsCouldGoLoading = true;

              _this29.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe(function (resp) {
                _this29.CouldGoSchedules = resp.data;
                _this29.IsCouldGoLoading = false;
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
              extend: 'colvis',
              exportOptions: exportColumns
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
          }; // this.changeFilterValueIntervalForMultiWindow = setInterval(() => {
          //this.checkFilterValueChange();
          // }, 10000);
        }
      }, {
        key: "getLocationIds",
        value: function getLocationIds() {
          var locations = [];
          var selectedLocationIds = '';
          this.FilterForm.get('SelectedLocations').value.forEach(function (res) {
            locations.push(res.Id);
          });
          selectedLocationIds = locations.join();
          return selectedLocationIds == null ? '' : selectedLocationIds;
        }
      }, {
        key: "getSupplierIds",
        value: function getSupplierIds() {
          var selectedSupplierIds = '';
          var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value || [];
          selectedSuppliers.map(function (m) {
            if (selectedSupplierIds == '') selectedSupplierIds = m.Id;else selectedSupplierIds += ',' + m.Id;
          });
          return selectedSupplierIds;
        }
      }, {
        key: "getCarrierIds",
        value: function getCarrierIds() {
          var selectedCarrierIds = '';
          var selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
          selectedCarriers.map(function (m) {
            if (selectedCarrierIds == '') selectedCarrierIds = m.Id;else selectedCarrierIds += ',' + m.Id;
          });
          return selectedCarrierIds;
        }
      }, {
        key: "getselectedLocAttributeIds",
        value: function getselectedLocAttributeIds() {
          var _locAttribute = [];
          this.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
            return _locAttribute.push(x.Id);
          });

          var _locAttributeIds = _locAttribute.join();

          return _locAttributeIds;
        }
      }, {
        key: "changeActiveTab",
        value: function changeActiveTab(priority) {
          this.activePriorityTab = priority;
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this30 = this;

          // this.subscriptions.add(this.FilterForm.valueChanges
          //     .subscribe(change => {
          //         if (change.FromDate && change.ToDate) {
          //             this.filterDriverData();
          //         }
          //     }));
          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            _this30.loadingData = true;

            _this30.filterDriverData();
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(function (change) {
            _this30.loadingData = true;

            _this30.filterDriverData();
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
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.singleMulti && change.singleMulti.currentValue) {//alert(change.singleMulti.currentValue)
          }
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getBuyerLoads();
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
        key: "checkFilterValueChange",
        value: function checkFilterValueChange() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
            var frmDate, toDate, selectedLocations, selectedStates;
            return regeneratorRuntime.wrap(function _callee3$(_context3) {
              while (1) {
                switch (_context3.prev = _context3.next) {
                  case 0:
                    if (this.singleMulti == 2) {
                      frmDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY);
                      toDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY);
                      selectedLocations = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_LOCATION_KEY);
                      selectedLocations == "" ? selectedLocations = [] : '';
                      selectedStates = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_SELECTEDSTATES_KEY);
                      selectedStates == "" ? selectedStates = [] : '';

                      if (frmDate != '' && toDate != '' && (!(+moment__WEBPACK_IMPORTED_MODULE_4__(frmDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('FromDate').value)) || !(+moment__WEBPACK_IMPORTED_MODULE_4__(toDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('ToDate').value)))) {
                        this.FilterForm.get('FromDate').patchValue(frmDate);
                        this.initializeFilterChange();
                      } else if (!this.isArrayEqual(selectedLocations, this.FilterForm.get('SelectedLocations').value)) {
                        this.FilterForm.get('SelectedLocations').patchValue(selectedLocations);
                        this.initializeFilterChange();
                      }
                    }

                  case 1:
                  case "end":
                    return _context3.stop();
                }
              }
            }, _callee3, this);
          }));
        }
      }, {
        key: "initializeFilterChange",
        value: function initializeFilterChange() {
          localStorage.setItem("filterChange", '1');
          window.location.reload();
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
          var _this31 = this;

          this.clearAllIntervals();
          this.searchLoadInterval = window.setTimeout(function () {
            _this31.getBuyerLoads();

            _this31.autoRefreshLoads();
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
        key: "getBuyerLoads",
        value: function getBuyerLoads(statusId) {
          if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
          }

          var _priorities = [];
          this.FilterForm.get('SelectedPriorities').value.forEach(function (x) {
            return _priorities.push(x.Id);
          });
          this.SelectedPrioritiesId = _priorities;

          if (this.SelectedPrioritiesId.length > 0) {
            this.showMustGo = this.SelectedPrioritiesId.filter(function (f) {
              return f == 1;
            }).length == 1 ? true : false;
            this.showShouldGo = this.SelectedPrioritiesId.filter(function (f) {
              return f == 2;
            }).length == 1 ? true : false;
            this.showCouldGo = this.SelectedPrioritiesId.filter(function (f) {
              return f == 3;
            }).length == 1 ? true : false;
          } else {
            this.showMustGo = true;
            this.showShouldGo = true;
            this.showCouldGo = true;
          }

          this.startAutoRefreshTimer();
          this.loadingData = false;
          this.refreshDatatable();
        }
      }, {
        key: "autoRefreshLoads",
        value: function autoRefreshLoads() {
          var _this32 = this;

          this.autoRefreshInterval = window.setInterval(function () {
            if (IsUserActive()) {
              _this32.getBuyerLoads();
            }
          }, this.AUTO_REFRESH_TIME * 1000);
        }
      }, {
        key: "startAutoRefreshTimer",
        value: function startAutoRefreshTimer() {
          var _this33 = this;

          this.stopAutoRefreshTimer();
          this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
          this.autoRefreshTimerInterval = window.setInterval(function () {
            if (IsUserActive()) {
              if (_this33.autoRefreshTicks == 0) {
                _this33.autoRefreshTicks = _this33.AUTO_REFRESH_TIME;

                _this33.stopAutoRefreshTimer();
              } else {
                _this33.autoRefreshTicks--;
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
        key: "toggleGrids",
        value: function toggleGrids() {
          var toggleGrid = this.FilterForm.get('ToggleGrids').value;
          this.FilterForm.get('ToggleGrids').patchValue(!toggleGrid);
        }
      }, {
        key: "showDriverDetails",
        value: function showDriverDetails(driver) {
          var _this34 = this;

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
              _this34.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"](data);
              _this34.modalData = false;
            } else {
              _this34.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["DriverAdditionalDetails"]();

              _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning('Please try again later.', 'Something Went Wrong', 3000);

              _this34.modalData = false;
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
        key: "filterMapByStatus",
        value: function filterMapByStatus(statusId) {
          this.selectedMaplable = statusId;
          this.getBuyerLoads(statusId);
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
          this.filterDriverData();
        }
      }]);

      return WhereIsMyDriverGridViewComponent;
    }();

    WhereIsMyDriverGridViewComponent.??fac = function WhereIsMyDriverGridViewComponent_Factory(t) {
      return new (t || WhereIsMyDriverGridViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]));
    };

    WhereIsMyDriverGridViewComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: WhereIsMyDriverGridViewComponent,
      selectors: [["app-where-is-my-driver-grid-view"]],
      viewQuery: function WhereIsMyDriverGridViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_c0, true, angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti",
        FilterForm: "FilterForm"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["????NgOnChangesFeature"]],
      decls: 141,
      vars: 39,
      consts: [["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "row"], [1, "col-sm-12"], [1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-eye", "mr5"], ["id", "grid-view", 1, "col-sm-12", 2, "margin-top", "15px", 3, "ngClass"], [1, "sticky-header"], [1, "col-12", "text-right", "priority-tabs"], [1, "nav", "nav-pills", "float-right"], [1, "nav-item", 3, "click"], [1, "nav-link", "mustgo", "active", 3, "ngClass"], [1, "nav-link", "shouldgo", 3, "ngClass"], [1, "nav-link", "couldgo", 3, "ngClass"], [3, "ngClass"], [1, "mustgo", "mb5", 2, "color", "#fd7668 !important"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "table-mustgo", "data-gridname", "19", "datatable", "", 1, "table", "table-bordered", "table-hover", "serverside-table", 3, "dtOptions", "dtTrigger"], ["data-key", "PoNum"], ["data-key", "Name"], ["data-key", "DName"], ["data-key", "Pckup"], ["data-key", "Loc"], ["data-key", "LT"], ["data-key", "PrdtNm"], ["data-key", "Qty"], ["data-key", "LdDate"], ["data-key", "Status"], [1, "shouldgo", "mb5", 2, "color", "#f3c316 !important"], ["id", "table-shouldgo", "data-gridname", "20", "datatable", "", 1, "table", "table-bordered", "table-hover", "serverside-table", 3, "dtOptions", "dtTrigger"], [1, "couldgo", "mb5", 2, "color", "#a7a2a2 !important"], ["id", "table-couldgo", "data-gridname", "21", "datatable", "", 1, "table", "table-bordered", "table-hover", "serverside-table", 3, "dtOptions", "dtTrigger"], ["type", "button", "id", "btnconfirm-memberInfo", "data-toggle", "modal", "data-target", "#confirm-memberInfo", "data-backdrop", "static", "data-keyboard", "false", 1, "hide-element"], ["id", "confirm-memberInfo", "tabindex", "-1", "role", "dialog", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "fs18", "f-bold", "mt0"], ["id", "member-datatable", 1, "table", "table-striped", "table-bordered", "table-hover"], [4, "ngFor", "ngForOf"], [1, "text-right"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", "btn-lg"], ["id", "invoice", 1, "hide-element"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"]],
      template: function WhereIsMyDriverGridViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](0, WhereIsMyDriverGridViewComponent_div_0_Template, 2, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverGridViewComponent_Template_a_click_3_listener() {
            return ctx.toggleGrids();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "ul", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_12_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.MustGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14, "Must Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_15_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.ShouldGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "a", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](17, "Should Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "li", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverGridViewComponent_Template_li_click_18_listener() {
            return ctx.changeActiveTab(ctx.DeliveryReqPriority.CouldGo);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "a", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Could Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "h4", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Must Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "table", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "PO Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "Driver");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Dispatcher");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "Pickup");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](42, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](46, "Product Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](48, "Ordered Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](50, "Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](52, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "h4", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](57, "Should Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "table", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](66, "PO Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](68, "Driver");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](69, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](70, "Dispatcher");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](71, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](72, "Pickup");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](74, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](75, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](76, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](77, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](78, "Product Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](79, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](80, "Ordered Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](81, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](82, "Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](83, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](84, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](85, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](86, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](87, "h4", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](88, "strong");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](89, "Could Go");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](90, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](91, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](92, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](93, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](94, "table", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](95, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](96, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](97, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](98, "PO Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](99, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](100, "Driver");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](101, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](102, "Dispatcher");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](103, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](104, "Pickup");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](105, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](106, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](107, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](108, "Inventory Capture Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](109, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](110, "Product Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](111, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](112, "Ordered Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](113, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](114, "Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](115, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](116, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](117, "button", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](118, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](119, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](120, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](121, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](122, "h2", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](123, "Group Member Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](124, "table", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](125, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](126, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](127, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](128, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](129, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](130, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](131, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](132, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](133, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](134, "LastSeenAt");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](135, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](136, WhereIsMyDriverGridViewComponent_tr_136_Template, 9, 4, "tr", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](137, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](138, "button", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](139, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](140, "div", 44);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.loadingData);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", ctx.FilterForm.get("ToggleGrids").value == true ? "Show Grids" : "Hide Grids", " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](19, _c1, ctx.FilterForm.get("ToggleGrids").value));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](21, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.MustGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](23, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.ShouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](25, _c2, ctx.activePriorityTab == ctx.DeliveryReqPriority.CouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](27, _c1, !ctx.showMustGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](29, _c1, ctx.activePriorityTab == ctx.DeliveryReqPriority.ShouldGo || ctx.activePriorityTab == ctx.DeliveryReqPriority.CouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtMustGoOptions)("dtTrigger", ctx.dtMustGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](31, _c1, !ctx.showShouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](33, _c1, ctx.activePriorityTab == ctx.DeliveryReqPriority.MustGo || ctx.activePriorityTab == ctx.DeliveryReqPriority.CouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtShouldGoOptions)("dtTrigger", ctx.dtShouldGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](35, _c1, !ctx.showCouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](37, _c1, ctx.activePriorityTab == ctx.DeliveryReqPriority.MustGo || ctx.activePriorityTab == ctx.DeliveryReqPriority.ShouldGo));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtCouldGoOptions)("dtTrigger", ctx.dtCouldGoTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.memberInfo);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"]],
      styles: [".sticky-header[_ngcontent-%COMP%] {\r\n    position: sticky;\r\n    top: 110px;\r\n    background-color: #ffffff;\r\n    padding: 5px 15px;\r\n    font-size: 20px;\r\n    z-index: 9;\r\n    text-align: right;\r\n    border-radius: 10px;\r\n    \r\n    box-shadow: 0 3px -1px 0 rgba(0,0,0,.1);\r\n    margin-bottom: 5px;\r\n}\r\n\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav[_ngcontent-%COMP%]    > li[_ngcontent-%COMP%]    > a[_ngcontent-%COMP%] {\r\n    position: relative;\r\n    display: block;\r\n    padding: 10px 15px 10px 15px;\r\n    color: #616161;\r\n    font-size: 14px;\r\n    font-weight: normal;\r\n    border-radius: 5px;\r\n    margin-right:5px;\r\n}\r\n\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav[_ngcontent-%COMP%]    > li[_ngcontent-%COMP%]    > a[_ngcontent-%COMP%]:hover{\r\ncolor:#ffffff;\r\n}\r\n\r\n\r\n.priority-tabs[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-link.active[_ngcontent-%COMP%] {\r\n    color: #ffffff;\r\n}\r\n\r\n\r\ntable.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    position: fixed !important;\r\n}\r\n\r\n\r\ntable.dataTable.fixedHeader-floating[_ngcontent-%COMP%], table.dataTable.fixedHeader-locked[_ngcontent-%COMP%] {\r\n    top: 115px !important;\r\n    \r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2FsbHktZGFzaGJvYXJkL2dyaWQtdmlldy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBRUksZ0JBQWdCO0lBQ2hCLFVBQVU7SUFDVix5QkFBeUI7SUFDekIsaUJBQWlCO0lBQ2pCLGVBQWU7SUFDZixVQUFVO0lBQ1YsaUJBQWlCO0lBQ2pCLG1CQUFtQjtJQUNuQixxREFBcUQ7SUFFckQsdUNBQXVDO0lBQ3ZDLGtCQUFrQjtBQUN0Qjs7O0FBR0E7SUFDSSxrQkFBa0I7SUFDbEIsY0FBYztJQUNkLDRCQUE0QjtJQUM1QixjQUFjO0lBQ2QsZUFBZTtJQUNmLG1CQUFtQjtJQUNuQixrQkFBa0I7SUFDbEIsZ0JBQWdCO0FBQ3BCOzs7QUFFQTtBQUNBLGFBQWE7QUFDYjs7O0FBRUE7SUFDSSxjQUFjO0FBQ2xCOzs7QUFDQTtJQUNJLDBCQUEwQjtBQUM5Qjs7O0FBRUE7SUFDSSxxQkFBcUI7SUFDckIsa0JBQWtCO0FBQ3RCIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2FsbHktZGFzaGJvYXJkL2dyaWQtdmlldy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLnN0aWNreS1oZWFkZXIge1xyXG4gICAgcG9zaXRpb246IC13ZWJraXQtc3RpY2t5O1xyXG4gICAgcG9zaXRpb246IHN0aWNreTtcclxuICAgIHRvcDogMTEwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZmZmZmO1xyXG4gICAgcGFkZGluZzogNXB4IDE1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiA5O1xyXG4gICAgdGV4dC1hbGlnbjogcmlnaHQ7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG4gICAgLyogLXdlYmtpdC1ib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7ICovXHJcbiAgICAtbW96LWJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIGJveC1zaGFkb3c6IDAgM3B4IC0xcHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIG1hcmdpbi1ib3R0b206IDVweDtcclxufVxyXG5cclxuXHJcbi5wcmlvcml0eS10YWJzIC5uYXYgPiBsaSA+IGEge1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG4gICAgZGlzcGxheTogYmxvY2s7XHJcbiAgICBwYWRkaW5nOiAxMHB4IDE1cHggMTBweCAxNXB4O1xyXG4gICAgY29sb3I6ICM2MTYxNjE7XHJcbiAgICBmb250LXNpemU6IDE0cHg7XHJcbiAgICBmb250LXdlaWdodDogbm9ybWFsO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4O1xyXG4gICAgbWFyZ2luLXJpZ2h0OjVweDtcclxufVxyXG5cclxuLnByaW9yaXR5LXRhYnMgLm5hdiA+IGxpID4gYTpob3ZlcntcclxuY29sb3I6I2ZmZmZmZjtcclxufVxyXG5cclxuLnByaW9yaXR5LXRhYnMgLm5hdi1waWxscyAubmF2LWxpbmsuYWN0aXZlIHtcclxuICAgIGNvbG9yOiAjZmZmZmZmO1xyXG59XHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1sb2NrZWQge1xyXG4gICAgcG9zaXRpb246IGZpeGVkICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbnRhYmxlLmRhdGFUYWJsZS5maXhlZEhlYWRlci1mbG9hdGluZywgdGFibGUuZGF0YVRhYmxlLmZpeGVkSGVhZGVyLWxvY2tlZCB7XHJcbiAgICB0b3A6IDExNXB4ICFpbXBvcnRhbnQ7XHJcbiAgICAvKmJhY2tncm91bmQ6cmVkOyovXHJcbn0iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](WhereIsMyDriverGridViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-where-is-my-driver-grid-view',
          templateUrl: './grid-view.component.html',
          styleUrls: ['./grid-view.component.css']
        }]
      }], function () {
        return [{
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__["BuyerwallyboardService"]
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
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/wally-dashboard/map-view.component.ts": function srcAppBuyerWallyBoardWallyDashboardMapViewComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var src_app_shared_components_sendbird_buyer_sendbird_buyer_sendbird_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/buyer-sendbird/buyer-sendbird.component */
    "./src/app/shared-components/sendbird/buyer-sendbird/buyer-sendbird.component.ts");
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


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
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
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var agm_direction__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! agm-direction */
    "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js");

    var _c0 = ["SelectedDriverLoad"];

    function WhereIsMyDriverMapViewComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_p_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "p", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Note:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, " Click truck to hide routes.");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_ng_template_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "p", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2, "Note:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, " Click truck to view routes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    var _c1 = function _c1() {
      return {
        "height": 40,
        "width": 50
      };
    };

    var _c2 = function _c2(a0, a1) {
      return {
        "url": a0,
        "scaledSize": a1
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "agm-marker", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("mouseOver", function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_Template_agm_marker_mouseOver_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r19);

          var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](2);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r18.mouseHoverMarker(_r14, $event);
        })("markerClick", function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_Template_agm_marker_markerClick_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r19);

          var indx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().index;

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r20.showHideRoutes(indx_r10);
        })("mouseOut", function WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_Template_agm_marker_mouseOut_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r19);

          var indx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().index;

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r22.mouseHoveOutMarker(null, $event, indx_r10);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "agm-info-window", 49, 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Driver Name: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "Contact Number: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "a", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Last UpdatedAt: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_p_17_Template, 4, 0, "p", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_ng_template_18_Template, 4, 0, "ng-template", null, 53, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](19);

        var driver_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("latitude", driver_r9.Lat)("longitude", driver_r9.Lng)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](12, _c2, "src/assets/truck-" + driver_r9.SttsId + ".svg", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction0"](11, _c1)));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableAutoPan", false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", driver_r9.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate1"]("href", "tel:", driver_r9.PhNo, "", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????sanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate1"]("title", "Call ", driver_r9.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r9.PhNo);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", driver_r9.AppLastUpdatedDate, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r9.routeShow)("ngIfElse", _r16);
      }
    }

    var _c3 = function _c3(a0, a1) {
      return {
        lat: a0,
        lng: a1
      };
    };

    var _c4 = function _c4(a0) {
      return {
        strokeColor: a0
      };
    };

    var _c5 = function _c5(a1) {
      return {
        suppressMarkers: true,
        polylineOptions: a1
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_container_33_agm_direction_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "agm-direction", 55);
      }

      if (rf & 2) {
        var driver_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("origin", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](4, _c3, driver_r9.Lat, driver_r9.Lng))("destination", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](7, _c3, driver_r9.dLat, driver_r9.dLng))("visible", driver_r9.routeShow)("renderOptions", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](12, _c5, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](10, _c4, driver_r9.statusColor)));
      }
    }

    var _c6 = function _c6() {
      return {
        "height": 25,
        "width": 25
      };
    };

    var _c7 = function _c7(a1) {
      return {
        "url": "https://maps.google.com/mapfiles/ms/icons/red-dot.png",
        "scaledSize": a1
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_container_33_Template(rf, ctx) {
      if (rf & 1) {
        var _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, WhereIsMyDriverMapViewComponent_ng_container_33_agm_marker_1_Template, 20, 15, "agm-marker", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "agm-marker", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("mouseOver", function WhereIsMyDriverMapViewComponent_ng_container_33_Template_agm_marker_mouseOver_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r27);

          var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4);

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r26.mouseHoverMarker(_r12, $event);
        })("mouseOut", function WhereIsMyDriverMapViewComponent_ng_container_33_Template_agm_marker_mouseOut_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r27);

          var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4);

          var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r28.mouseHoveOutMarker(_r12, $event, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "agm-info-window", 45, 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Engaged Driver : ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "p");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "b");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Drop Location: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, WhereIsMyDriverMapViewComponent_ng_container_33_agm_direction_14_Template, 1, 14, "agm-direction", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var driver_r9 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r9.Lat != null && driver_r9.Lng != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("latitude", driver_r9.dLat)("longitude", driver_r9.dLng)("iconUrl", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](10, _c7, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction0"](9, _c6)));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disableAutoPan", false)("maxWidth", 200);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", driver_r9.Name, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r9.Loc);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", driver_r9.dLat && driver_r9.dLng && driver_r9.Lat != null && driver_r9.Lng != null);
      }
    }

    var _c8 = function _c8(a0, a1) {
      return {
        "fa-arrow-circle-right": a0,
        "fa-arrow-circle-left": a1
      };
    };

    function WhereIsMyDriverMapViewComponent_div_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "a", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_div_35_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30);

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r29.toggleDriverView();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](1, _c8, !ctx_r2.FilterForm.get("ToggleDriverView").value, ctx_r2.FilterForm.get("ToggleDriverView").value));
      }
    }

    var _c9 = function _c9(a0) {
      return {
        "activeRoute": a0
      };
    };

    var _c10 = function _c10(a0) {
      return {
        "color": a0
      };
    };

    function WhereIsMyDriverMapViewComponent_div_43_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "span", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_div_43_Template_div_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var indx_r32 = ctx.index;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r33.showHideRoutes(indx_r32);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "span", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var driver_r31 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", driver_r31.IsOnline ? "live" : "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r31.Intl);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate1"]("title", "Click to ", driver_r31.routeShow ? "hide" : "show", " routes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](7, _c9, driver_r31.routeShow))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](9, _c10, driver_r31.routeShow ? driver_r31.statusColor : "#2b2b2b"));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r31.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r31.PhNo);
      }
    }

    function WhereIsMyDriverMapViewComponent_div_44_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "span", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var driver_r35 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r35.Intl);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r35.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](driver_r35.PhNo);
      }
    }

    function WhereIsMyDriverMapViewComponent_tr_64_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var member_r37 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r37.nickname);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r37.userId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r37.connectionStatus);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](member_r37.lastSeenAt);
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_template_70_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    var _c11 = function _c11(a3) {
      return {
        "modal": true,
        "left": true,
        "fade": true,
        "show": a3
      };
    };

    var _c12 = function _c12(a0) {
      return {
        "display": a0
      };
    };

    function WhereIsMyDriverMapViewComponent_ng_template_70_Template(rf, ctx) {
      if (rf & 1) {
        var _r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, WhereIsMyDriverMapViewComponent_ng_template_70_div_2_Template, 2, 0, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "h4", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "a", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_ng_template_70_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r41);

          var modalDetails_r38 = ctx.modalDetails;

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r40.doChat(modalDetails_r38.data.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "span", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "a", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_ng_template_70_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r41);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r42.modalClose();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](10, "i", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var modalDetails_r38 = ctx.modalDetails;

        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](4, _c11, modalDetails_r38.display === "block"))("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](6, _c12, modalDetails_r38.display));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.loadingData);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", ctx_r7.selectedDriverDetails.Name, " ");
      }
    }

    function WhereIsMyDriverMapViewComponent_ng_container_72_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainer"](0);
      }
    }

    var _c13 = function _c13(a0, a1, a2, a3) {
      return {
        "fadeIn": a0,
        "display_hide": a1,
        "col-sm-9": a2,
        "col-sm-12": a3
      };
    };

    var _c14 = function _c14(a0) {
      return {
        "height": a0
      };
    };

    var _c15 = function _c15(a0, a1, a2, a3) {
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
        this.Locations = [];
        this.States = [];
        this.Suppliers = [];
        this.Carriers = [];
        this.LoadPriorities = src_app_app_constants__WEBPACK_IMPORTED_MODULE_9__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.LocationDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleFilter = false;
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
      }

      _createClass(WhereIsMyDriverMapViewComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this35 = this;

          this.readOnlyModeSelection();
          this.setSelectedLocationId();
          this.subscribeFormChanges();
          this.dispatcherService.getDispatcherCountry().subscribe(function (data) {
            _this35.UserCountry = data;
            _this35.FuelUnit = _this35.UserCountry === 'USA' ? 'Gallons' : 'Litres';

            _this35.setMapCenter();
          });
          this.chatService.loaderDetails.subscribe(function (data) {
            if (data != undefined) {
              _this35.loadingData = data;
            }
          });
          this.chatService.memberInfoDetails.subscribe(function (data) {
            if (data != undefined) {
              _this35.memberInfo = data;
              _this35.loadingData = false;
              jQuery('#btnconfirm-memberInfo').click();
            }
          });
          this.changeFilterValueIntervalForMultiWindow = setInterval(function () {
            if (IsUserActive()) {
              _this35.checkFilterValueChange();
            }
          }, 10000);
        }
      }, {
        key: "clickOutsideDropdown",
        value: function clickOutsideDropdown() {
          if (this.toogleFilter) {
            this.toogleFilter = false;
          }
        }
      }, {
        key: "setSelectedLocationId",
        value: function setSelectedLocationId() {
          var ids = [];
          var selectedLocationIds = this.FilterForm.get('SelectedLocations').value.forEach(function (res) {
            ids.push(res.Id);
          });
          selectedLocationIds = ids.join();
          this.SelectedLocationId = selectedLocationIds;
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.singleMulti && change.singleMulti.currentValue) {//alert(change.singleMulti.currentValue)
          }
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
        }
      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this36 = this;

          // this.subscriptions.add(this.FilterForm.valueChanges
          //     .subscribe(change => {
          //         if (change.FromDate && change.ToDate) {
          //             var ids = [];
          //             change.SelectedLocations.forEach(res => { ids.push(res.Id) });
          //             var selectedLocationIds = ids.join();
          //             if (selectedLocationIds != this.SelectedLocationId) {
          //                 this.CloneOnGoingLoads = [];
          //                 this.SelectedLocationId = selectedLocationIds;
          //             }
          //             this.filterDriverData();
          //         }
          //     }))
          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            // this.loadingData = true;    
            _this36.filterDriverData();
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(function (change) {
            // this.loadingData = true;       
            _this36.filterDriverData();
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
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
            var frmDate, toDate;
            return regeneratorRuntime.wrap(function _callee4$(_context4) {
              while (1) {
                switch (_context4.prev = _context4.next) {
                  case 0:
                    if (this.singleMulti == 2) {
                      frmDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_FROMDATE_KEY);
                      toDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_7__["MyLocalStorage"].WBF_TODATE_KEY); //let selectedLocations = MyLocalStorage.getData(MyLocalStorage.WBF_LOCATION_KEY);
                      //selectedLocations == "" ? selectedLocations = [] : '';
                      // let selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
                      //selectedStates == "" ? selectedStates = [] : '';

                      if (frmDate != '' && toDate != '' && (!(+moment__WEBPACK_IMPORTED_MODULE_4__(frmDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('FromDate').value)) || !(+moment__WEBPACK_IMPORTED_MODULE_4__(toDate) === +moment__WEBPACK_IMPORTED_MODULE_4__(this.FilterForm.get('ToDate').value)))) {
                        this.FilterForm.get('FromDate').patchValue(frmDate);
                        this.initializeFilterChange();
                      } //else if (!this.isArrayEqual(selectedLocations, this.FilterForm.get('SelectedLocations').value)) {
                      //    this.FilterForm.get('SelectedLocations').patchValue(selectedLocations);
                      //    this.initializeFilterChange();
                      //}

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
        key: "setMapCenter",
        value: function setMapCenter() {
          var _this37 = this;

          if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(function () {
              _this37.centerLoactionLat = _this37.CountryCentre[_this37.UserCountry].lat;
              _this37.centerLoactionLng = _this37.CountryCentre[_this37.UserCountry].lng;

              if (_this37.googleMap && _this37.OnGoingLoads.length == 0) {
                var bounds = new google.maps.LatLngBounds();
                bounds.extend(new google.maps.LatLng(_this37.centerLoactionLat, _this37.centerLoactionLng));

                _this37.googleMap.fitBounds(bounds);

                _this37.googleMap.setZoom(5);
              } else {
                var _bounds = new google.maps.LatLngBounds();

                _this37.OnGoingLoads.filter(function (t) {
                  return t.Lat != null && t.Lng != null;
                }).forEach(function (x) {
                  x.statusColor = src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["routesColor"][x.SttsId];

                  _bounds.extend(new google.maps.LatLng(x.Lat, x.Lng));
                });

                _this37.googleMap.fitBounds(_bounds);

                var locationbounds = new google.maps.LatLngBounds();

                _this37.OnGoingLoads.forEach(function (x) {
                  locationbounds.extend(new google.maps.LatLng(x.dLat, x.dLng));
                });

                if (_this37.googleMap && locationbounds) {
                  _this37.googleMap.setCenter(locationbounds.getCenter());
                }

                _this37.googleMap.setZoom(5);
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
        key: "filterDriverData",
        value: function filterDriverData() {
          var _this38 = this;

          this.clearAllIntervals();
          this.loadingData = true;
          this.searchLoadInterval = window.setTimeout(function () {
            _this38.getDispatcherLoads();

            _this38.autoRefreshLoads();
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
          var _this39 = this;

          if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
          }

          var _states = [];
          this.FilterForm.get('SelectedStates').value.forEach(function (x) {
            return _states.push(x.Id);
          });
          var _priorities = [];
          this.FilterForm.get('SelectedPriorities').value.forEach(function (x) {
            return _priorities.push(x.Id);
          });
          this.SelectedPrioritiesId = _priorities;
          var selectedSupplierIds = '';
          this.FilterForm.get('SelectedSuppliers').value.map(function (m) {
            if (selectedSupplierIds == '') selectedSupplierIds = m.Id;else selectedSupplierIds += ',' + m.Id;
          });
          var selectedCarrierIds = '';
          this.FilterForm.get('SelectedCarriers').value.map(function (m) {
            if (selectedCarrierIds == '') selectedCarrierIds = m.Id;else selectedCarrierIds += ',' + m.Id;
          });
          var ids = [];
          this.SelectedLocationId = '';
          this.FilterForm.get('SelectedLocations').value.forEach(function (res) {
            ids.push(res.Id);
          });
          this.SelectedLocationId = ids.join();
          var _locAttribute = [];
          this.FilterForm.get('selectedLocAttributeList').value.forEach(function (x) {
            return _locAttribute.push(x.Id);
          });

          var _locAttributeIds = _locAttribute.join();

          var inputs = {
            LocationIds: this.SelectedLocationId == null ? '' : this.SelectedLocationId,
            States: _states,
            Priorities: _priorities,
            FromDate: this.FilterForm.get('FromDate').value,
            ToDate: this.FilterForm.get('ToDate').value,
            DriverSearch: this.SearchedKeyword,
            SupplierCompanyIds: selectedSupplierIds,
            CarrierCompanyIds: selectedCarrierIds,
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
            this.dispatcherService.getOnGoingLoadsForMap(inputs).subscribe(function (data) {
              _this39.CloneOnGoingLoads = data;

              _this39.initailizeOnGoingLoad(data);
            });
          } else this.initailizeOnGoingLoad(data);
        }
      }, {
        key: "initailizeOnGoingLoad",
        value: function initailizeOnGoingLoad(data) {
          var _this40 = this;

          this.OnGoingLoads = data;
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
              if (m.Lat == null && m.Lng == null && m.Name != null && m.Name != undefined && m.Name.trim() != '') _this40.Drivers && _this40.Drivers.filter(function (f) {
                return f.Name == m.Name;
              }).length > 0 ? '' : _this40.OfflineDrivers.push(m);
            }
          }); //this.OfflineDrivers = data.filter(x => x.Lat == null && x.Lng == null && x.Name != null && x.Name != undefined && x.Name.trim() != '');

          this.setMapCenter();
          this.startAutoRefreshTimer();
          this.loadingData = false;
          this.addDrivertoBackground();
        }
      }, {
        key: "autoRefreshLoads",
        value: function autoRefreshLoads() {
          var _this41 = this;

          this.autoRefreshInterval = window.setInterval(function () {
            if (IsUserActive()) {
              _this41.getDispatcherLoads();
            }
          }, this.AUTO_REFRESH_TIME * 1000);
        }
      }, {
        key: "startAutoRefreshTimer",
        value: function startAutoRefreshTimer() {
          var _this42 = this;

          this.stopAutoRefreshTimer();
          this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
          this.autoRefreshTimerInterval = window.setInterval(function () {
            if (IsUserActive()) {
              if (_this42.autoRefreshTicks == 0) {
                _this42.autoRefreshTicks = _this42.AUTO_REFRESH_TIME;

                _this42.stopAutoRefreshTimer();
              } else {
                _this42.autoRefreshTicks--;
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
          var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
          this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
        }
      }, {
        key: "toggleDriverView",
        value: function toggleDriverView() {
          var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
          this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
        }
      }, {
        key: "addDrivertoBackground",
        value: function addDrivertoBackground() {
          var _this43 = this;

          this.Drivers.forEach(function (xItem) {
            _this43.backgroudchatDefault.push(xItem.Id);
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
          if (this.previousInfowindow && this.previousInfowindow.isOpen) {
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

          if (this.previousInfowindow && this.previousInfowindow.isOpen && infoWindow) {
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
          var _this44 = this;

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
              _this44.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"](data);
              _this44.modalData = false;
            } else {
              _this44.selectedDriverDetails = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_8__["DriverAdditionalDetails"]();

              _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please try again later.', 'Something Went Wrong', 3000);

              _this44.modalData = false;
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
        }
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
            this.FilterForm.get('ToggleMap').patchValue(false);
          }
        }
      }, {
        key: "filterMapByStatus",
        value: function filterMapByStatus(statusId) {
          this.selectedMaplable = statusId;
          this.getDispatcherLoads(statusId);
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

    WhereIsMyDriverMapViewComponent.??fac = function WhereIsMyDriverMapViewComponent_Factory(t) {
      return new (t || WhereIsMyDriverMapViewComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_11__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_12__["CarrierService"]));
    };

    WhereIsMyDriverMapViewComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: WhereIsMyDriverMapViewComponent,
      selectors: [["app-where-is-my-driver-map-view"]],
      viewQuery: function WhereIsMyDriverMapViewComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_c0, true, angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](src_app_shared_components_sendbird_buyer_sendbird_buyer_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["BuyerSendbirdComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.sendbirdComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti",
        FilterForm: "FilterForm"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["????NgOnChangesFeature"]],
      decls: 74,
      vars: 49,
      consts: [["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "row", "animated", "mt60"], [1, "", 3, "ngClass"], [1, "expand_map_btn"], [1, "", 3, "click"], [1, "fa", "fa-2x", 3, "ngClass"], ["id", "map-view", 1, "mb15"], ["id", "mapLegend", 2, "z-index", "1", "position", "absolute", "top", "-5px", "left", "10px", "font-size", "11px"], ["id", "status-legends", 1, "well", "pa0"], [1, "border-b", "pb5", "pt5", "pl5"], [1, "db", "pa5", 3, "ngClass", "click"], ["src", "src/assets/truck-11.svg", "data-statusid", "11"], ["src", "src/assets/truck-12.svg", "data-statusid", "12"], ["src", "src/assets/truck-1.svg", "data-statusid", "1"], ["src", "src/assets/truck-18.svg", "data-statusid", "18"], [2, "z-index", "1", "position", "absolute", "top", "0", "right", "65px", "font-size", "11px", "opacity", "0.9"], [1, "well", "pa5"], [3, "ngStyle", "zoom", "maxZoom", "minZoom", "fullscreenControl", "fullscreenControlOptions", "mapReady"], [4, "ngFor", "ngForOf"], [1, "pl0", 3, "ngClass"], ["class", "driver_btn", 4, "ngIf"], [1, "mt10"], [1, "pull-left", "mt6", "pb0", "dib"], [1, "inner-addon", "left-addon", "pull-left", "ml10"], [1, "glyphicon", "glyphicon-search"], ["name", "txtSearch", "placeholder", "Search Drivers", "type", "text", "autocomplete", "off", 1, "form-control", 3, "input"], [1, "driver-list", "dib", "full-width"], ["class", "driver-details dib full-width pa5", 4, "ngFor", "ngForOf"], ["type", "button", "id", "btnconfirm-memberInfo", "data-toggle", "modal", "data-target", "#confirm-memberInfo", "data-backdrop", "static", "data-keyboard", "false", 1, "hide-element"], ["id", "confirm-memberInfo", "tabindex", "-1", "role", "dialog", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "fs18", "f-bold", "mt0"], ["id", "member-datatable", 1, "table", "table-striped", "table-bordered", "table-hover"], [1, "text-right"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", "btn-lg"], [1, "chat-wrapper", 2, "z-index", "9999"], ["driverDetailsModal", ""], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["id", "invoice", 1, "hide-element"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [3, "latitude", "longitude", "iconUrl", "mouseOver", "markerClick", "mouseOut", 4, "ngIf"], [3, "latitude", "longitude", "iconUrl", "mouseOver", "mouseOut"], [3, "disableAutoPan", "maxWidth"], ["infoWindow2", ""], [3, "origin", "destination", "visible", "renderOptions", 4, "ngIf"], [3, "latitude", "longitude", "iconUrl", "mouseOver", "markerClick", "mouseOut"], [3, "disableAutoPan"], ["infoWindow", ""], ["target", "_blank", 3, "href", "title"], ["style", "font-size:11px;padding-top: 10px;", 4, "ngIf", "ngIfElse"], ["showRouteTemplate", ""], [2, "font-size", "11px", "padding-top", "10px"], [3, "origin", "destination", "visible", "renderOptions"], [1, "driver_btn"], [1, "driver-details", "dib", "full-width", "pa5"], [1, "pull-left", "driver-initials", "radius-capsule", "mr10", "fs15", "color-white", "pr"], [3, "ngClass"], [1, "pull-left", 3, "ngClass", "ngStyle", "title", "click"], [1, "fs15"], [1, "fs12", "db", "opacity8"], ["title", "Last location is not available", 1, "pull-left"], ["id", "myModal", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 3, "ngClass", "ngStyle"], ["role", "document", 1, "modal-dialog", "modal-lg", "modal-dialog-centered"], [1, "modal-header", "pb0", "pt0"], ["id", "assetDetailsModal", 1, "modal-title"], ["title", "Chat", 3, "click"], [1, "fs18", "far", "fa-comment"], ["data-dismiss", "modal", "aria-label", "Close", 1, "float-right", "mt10", 3, "click"], [1, "fa", "fa-close", "fa-lg"]],
      template: function WhereIsMyDriverMapViewComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](0, WhereIsMyDriverMapViewComponent_div_0_Template, 2, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_4_listener() {
            return ctx.toggleExpandMapView();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_10_listener() {
            return ctx.filterMapByStatus(11);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](11, "img", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, " On the way to terminal ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_14_listener() {
            return ctx.filterMapByStatus(12);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](15, "img", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, " Arrived at terminal ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_18_listener() {
            return ctx.filterMapByStatus(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](19, "img", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, " On the way to location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "a", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function WhereIsMyDriverMapViewComponent_Template_a_click_22_listener() {
            return ctx.filterMapByStatus(18);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](23, "img", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, " Arrived at location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](27, "Auto Refresh in: ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](30, "date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, " minutes");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "agm-map", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("mapReady", function WhereIsMyDriverMapViewComponent_Template_agm_map_mapReady_32_listener($event) {
            return ctx.mapReady($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](33, WhereIsMyDriverMapViewComponent_ng_container_33_Template, 15, 12, "ng-container", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](35, WhereIsMyDriverMapViewComponent_div_35_Template, 3, 4, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Drivers");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](40, "i", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "input", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("input", function WhereIsMyDriverMapViewComponent_Template_input_input_41_listener($event) {
            return ctx.searchDrivers($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](43, WhereIsMyDriverMapViewComponent_div_43_Template, 9, 11, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](44, WhereIsMyDriverMapViewComponent_div_44_Template, 8, 3, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](45, "button", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "h2", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](51, "Group Member Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "table", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](56, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](58, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](60, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](62, "LastSeenAt");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](64, WhereIsMyDriverMapViewComponent_tr_64_Template, 9, 4, "tr", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "button", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](67, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](69, "app-buyer-sendbird");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](70, WhereIsMyDriverMapViewComponent_ng_template_70_Template, 11, 8, "ng-template", null, 38, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](72, WhereIsMyDriverMapViewComponent_ng_container_72_Template, 1, 0, "ng-container", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](73, "div", 40);
        }

        if (rf & 2) {
          var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](71);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.loadingData);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction4"](26, _c13, ctx.FilterForm.get("ToggleMap").value, !ctx.FilterForm.get("ToggleMap").value, !ctx.FilterForm.get("ToggleExpandMapView").value, ctx.FilterForm.get("ToggleExpandMapView").value === true));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](31, _c8, !ctx.FilterForm.get("ToggleExpandMapView").value, ctx.FilterForm.get("ToggleExpandMapView").value));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](34, _c9, ctx.selectedMaplable == 11));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](36, _c9, ctx.selectedMaplable == 12));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](38, _c9, ctx.selectedMaplable == 1));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](40, _c9, ctx.selectedMaplable == 18));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind3"](30, 22, ctx.autoRefreshTicks * 1000, "mm:ss", "UTC"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](42, _c14, ctx.singleMulti == 2 ? "80vh" : "60vh"))("zoom", ctx.zoomLevel)("maxZoom", 16)("minZoom", 2)("fullscreenControl", true)("fullscreenControlOptions", ctx.screenOptions);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.OnGoingLoads);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction4"](44, _c15, ctx.FilterForm.get("ToggleExpandMapView").value === false && ctx.FilterForm.get("ToggleMap").value === true, ctx.FilterForm.get("ToggleMap").value === false, ctx.FilterForm.get("ToggleDriverView").value === true && ctx.FilterForm.get("ToggleMap").value === false, ctx.FilterForm.get("ToggleExpandMapView").value === true && ctx.FilterForm.get("ToggleMap").value === true));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx.FilterForm.get("ToggleMap").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.Drivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.OfflineDrivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.memberInfo);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngTemplateOutlet", _r6)("ngTemplateOutletContext", ctx.driverModal);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _agm_core__WEBPACK_IMPORTED_MODULE_14__["AgmMap"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgStyle"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], src_app_shared_components_sendbird_buyer_sendbird_buyer_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["BuyerSendbirdComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgTemplateOutlet"], _agm_core__WEBPACK_IMPORTED_MODULE_14__["AgmMarker"], _agm_core__WEBPACK_IMPORTED_MODULE_14__["AgmInfoWindow"], agm_direction__WEBPACK_IMPORTED_MODULE_15__["??a"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["DatePipe"]],
      styles: [".driver-details[_ngcontent-%COMP%]:nth-child(5n+1)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #f6af27;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+2)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #ab47bc;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+3)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #a5a5a5;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+4)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #dc4949;\r\n}\r\n\r\n.driver-details[_ngcontent-%COMP%]:nth-child(5n+5)   .driver-initials[_ngcontent-%COMP%] {\r\n    background: #00897b;\r\n}\r\n\r\n\r\n\r\n.sticky-header-wmd[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    right: 0;\r\n    padding: 15px 20px;\r\n    top: 45px;\r\n    height: 65px;\r\n    \r\n    z-index: 10;\r\n    background: #fff;\r\n}\r\n\r\n.locationfilter[_ngcontent-%COMP%] {\r\n    width: 100%;\r\n    position: absolute;\r\n    right: 4px;\r\n    border-radius: 5px;\r\n    font-size: 14px;\r\n    z-index: 1010;\r\n}\r\n\r\n.sticky_header[_ngcontent-%COMP%] {\r\n    position: sticky;\r\n    top: 45px;\r\n    padding: 5px;\r\n    font-size: 20px;\r\n    z-index: 10;\r\n    background: #fff;\r\n    margin-bottom: 0px;\r\n    margin-top: -10px;\r\n    \r\n    border-radius: 2px;\r\n}\r\n\r\n.display_hide[_ngcontent-%COMP%] {\r\n    display: none;\r\n    transition: opacity 1s ease-out;\r\n    opacity: 0;\r\n}\r\n\r\n.expand_map_btn[_ngcontent-%COMP%] {\r\n    position: absolute;\r\n    top: 1px;\r\n    right: 15px;\r\n    background: #fff;\r\n    border-radius: 2px 2px 2px 2px;\r\n    padding: 3px;\r\n    box-shadow: -2px 2px 6px 1px #aaa;\r\n    z-index: 1;\r\n}\r\n\r\n.driver_btn[_ngcontent-%COMP%] {\r\n    position: absolute;\r\n    top: 15px;\r\n    left: -35px;\r\n    background: white;\r\n    border-radius: 2px;\r\n    border-top-left-radius: 5px;\r\n    border-bottom-left-radius: 5px;\r\n    padding: 5px;\r\n    box-shadow: -4px 0px 4px 0px #aaaaaa;\r\n}\r\n\r\n.absolute_driver[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    width: 25%;\r\n    top: 100px;\r\n    right: 0;\r\n    background: #fff;\r\n    z-index: 11;\r\n    padding: 10px;\r\n    box-shadow: 0 3px 15px 0 rgba(0,0,0,.1);\r\n    border-radius: 10px;\r\n}\r\n\r\n.hide_absolute_driver[_ngcontent-%COMP%] {\r\n    width: 0;\r\n    right: -20px;\r\n}\r\n\r\n.activeRoute[_ngcontent-%COMP%] {\r\n    font-weight: 600;\r\n    cursor: pointer;\r\n    background: #f5f5f5;\r\n}\r\n\r\n.live[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    border-radius: 50%;\r\n    background-color: green;\r\n    position: absolute;\r\n    top: -1px;\r\n    right: 1px;\r\n    transform: scale(1);\r\n    -webkit-animation: pulse 1s infinite;\r\n            animation: pulse 1s infinite;\r\n}\r\n\r\n.inactive[_ngcontent-%COMP%] {\r\n    height: 10px;\r\n    width: 10px;\r\n    border-radius: 50%;\r\n    background-color: orange;\r\n    position: absolute;\r\n    top: -1px;\r\n    right: 1px;\r\n}\r\n\r\n@-webkit-keyframes pulse {\r\n    0% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0.4);\r\n    }\r\n\r\n    70% {\r\n        box-shadow: 0 0 0 10px rgba(204,169,44, 0);\r\n    }\r\n\r\n    100% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0);\r\n    }\r\n}\r\n\r\n@keyframes pulse {\r\n    0% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0.4);\r\n    }\r\n\r\n    70% {\r\n        box-shadow: 0 0 0 10px rgba(204,169,44, 0);\r\n    }\r\n\r\n    100% {\r\n        box-shadow: 0 0 0 0 rgba(204,169,44, 0);\r\n    }\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2FsbHktZGFzaGJvYXJkL21hcC12aWV3LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxtQkFBbUI7QUFDdkI7O0FBRUE7OztDQUdDOztBQUNEO0lBQ0ksZUFBZTtJQUNmLFFBQVE7SUFDUixrQkFBa0I7SUFDbEIsU0FBUztJQUNULFlBQVk7SUFDWixtQkFBbUI7SUFDbkIsV0FBVztJQUNYLGdCQUFnQjtBQUNwQjs7QUFHQTtJQUNJLFdBQVc7SUFDWCxrQkFBa0I7SUFDbEIsVUFBVTtJQUNWLGtCQUFrQjtJQUNsQixlQUFlO0lBQ2YsYUFBYTtBQUNqQjs7QUFFQTtJQUVJLGdCQUFnQjtJQUNoQixTQUFTO0lBQ1QsWUFBWTtJQUNaLGVBQWU7SUFDZixXQUFXO0lBQ1gsZ0JBQWdCO0lBQ2hCLGtCQUFrQjtJQUNsQixpQkFBaUI7SUFDakIsMkNBQTJDO0lBQzNDLGtCQUFrQjtBQUN0Qjs7QUFFQTtJQUNJLGFBQWE7SUFDYiwrQkFBK0I7SUFDL0IsVUFBVTtBQUNkOztBQUVBO0lBQ0ksa0JBQWtCO0lBQ2xCLFFBQVE7SUFDUixXQUFXO0lBQ1gsZ0JBQWdCO0lBQ2hCLDhCQUE4QjtJQUM5QixZQUFZO0lBQ1osaUNBQWlDO0lBQ2pDLFVBQVU7QUFDZDs7QUFHQTtJQUNJLGtCQUFrQjtJQUNsQixTQUFTO0lBQ1QsV0FBVztJQUNYLGlCQUFpQjtJQUNqQixrQkFBa0I7SUFDbEIsMkJBQTJCO0lBQzNCLDhCQUE4QjtJQUM5QixZQUFZO0lBQ1osb0NBQW9DO0FBQ3hDOztBQUVBO0lBQ0ksZUFBZTtJQUNmLFVBQVU7SUFDVixVQUFVO0lBQ1YsUUFBUTtJQUNSLGdCQUFnQjtJQUNoQixXQUFXO0lBQ1gsYUFBYTtJQUNiLHVDQUF1QztJQUN2QyxtQkFBbUI7QUFDdkI7O0FBRUE7SUFDSSxRQUFRO0lBQ1IsWUFBWTtBQUNoQjs7QUFFQTtJQUNJLGdCQUFnQjtJQUNoQixlQUFlO0lBQ2YsbUJBQW1CO0FBQ3ZCOztBQUVBO0lBQ0ksWUFBWTtJQUNaLFdBQVc7SUFDWCxrQkFBa0I7SUFDbEIsdUJBQXVCO0lBQ3ZCLGtCQUFrQjtJQUNsQixTQUFTO0lBQ1QsVUFBVTtJQUNWLG1CQUFtQjtJQUNuQixvQ0FBNEI7WUFBNUIsNEJBQTRCO0FBQ2hDOztBQUVBO0lBQ0ksWUFBWTtJQUNaLFdBQVc7SUFDWCxrQkFBa0I7SUFDbEIsd0JBQXdCO0lBQ3hCLGtCQUFrQjtJQUNsQixTQUFTO0lBQ1QsVUFBVTtBQUNkOztBQUVBO0lBQ0k7UUFFSSx5Q0FBeUM7SUFDN0M7O0lBRUE7UUFFSSwwQ0FBMEM7SUFDOUM7O0lBRUE7UUFFSSx1Q0FBdUM7SUFDM0M7QUFDSjs7QUFmQTtJQUNJO1FBRUkseUNBQXlDO0lBQzdDOztJQUVBO1FBRUksMENBQTBDO0lBQzlDOztJQUVBO1FBRUksdUNBQXVDO0lBQzNDO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9idXllci13YWxseS1ib2FyZC93YWxseS1kYXNoYm9hcmQvbWFwLXZpZXcuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rMSkgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZjZhZjI3O1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzIpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogI2FiNDdiYztcclxufVxyXG5cclxuLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1biszKSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICNhNWE1YTU7XHJcbn1cclxuXHJcbi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rNCkgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZGM0OTQ5O1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzUpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogIzAwODk3YjtcclxufVxyXG5cclxuLyoudGFibGUuZGF0YVRhYmxlIHtcclxuICAgIG1hcmdpbi10b3A6IDAgIWltcG9ydGFudDtcclxufVxyXG4qL1xyXG4uc3RpY2t5LWhlYWRlci13bWQge1xyXG4gICAgcG9zaXRpb246IGZpeGVkO1xyXG4gICAgcmlnaHQ6IDA7XHJcbiAgICBwYWRkaW5nOiAxNXB4IDIwcHg7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBoZWlnaHQ6IDY1cHg7XHJcbiAgICAvKmZvbnQtc2l6ZTogMjBweDsqL1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcblxyXG5cclxuLmxvY2F0aW9uZmlsdGVyIHtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgcmlnaHQ6IDRweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMTRweDtcclxuICAgIHotaW5kZXg6IDEwMTA7XHJcbn1cclxuXHJcbi5zdGlja3lfaGVhZGVyIHtcclxuICAgIHBvc2l0aW9uOiAtd2Via2l0LXN0aWNreTtcclxuICAgIHBvc2l0aW9uOiBzdGlja3k7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBwYWRkaW5nOiA1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiAxMDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbiAgICBtYXJnaW4tdG9wOiAtMTBweDtcclxuICAgIC8qYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwwLDAsLjEpOyovXHJcbiAgICBib3JkZXItcmFkaXVzOiAycHg7XHJcbn1cclxuXHJcbi5kaXNwbGF5X2hpZGUge1xyXG4gICAgZGlzcGxheTogbm9uZTtcclxuICAgIHRyYW5zaXRpb246IG9wYWNpdHkgMXMgZWFzZS1vdXQ7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG59XHJcblxyXG4uZXhwYW5kX21hcF9idG4ge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAxcHg7XHJcbiAgICByaWdodDogMTVweDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBib3JkZXItcmFkaXVzOiAycHggMnB4IDJweCAycHg7XHJcbiAgICBwYWRkaW5nOiAzcHg7XHJcbiAgICBib3gtc2hhZG93OiAtMnB4IDJweCA2cHggMXB4ICNhYWE7XHJcbiAgICB6LWluZGV4OiAxO1xyXG59XHJcblxyXG5cclxuLmRyaXZlcl9idG4ge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAxNXB4O1xyXG4gICAgbGVmdDogLTM1cHg7XHJcbiAgICBiYWNrZ3JvdW5kOiB3aGl0ZTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDJweDtcclxuICAgIGJvcmRlci10b3AtbGVmdC1yYWRpdXM6IDVweDtcclxuICAgIGJvcmRlci1ib3R0b20tbGVmdC1yYWRpdXM6IDVweDtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIGJveC1zaGFkb3c6IC00cHggMHB4IDRweCAwcHggI2FhYWFhYTtcclxufVxyXG5cclxuLmFic29sdXRlX2RyaXZlciB7XHJcbiAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbiAgICB3aWR0aDogMjUlO1xyXG4gICAgdG9wOiAxMDBweDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxuICAgIHotaW5kZXg6IDExO1xyXG4gICAgcGFkZGluZzogMTBweDtcclxuICAgIGJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5oaWRlX2Fic29sdXRlX2RyaXZlciB7XHJcbiAgICB3aWR0aDogMDtcclxuICAgIHJpZ2h0OiAtMjBweDtcclxufVxyXG5cclxuLmFjdGl2ZVJvdXRlIHtcclxuICAgIGZvbnQtd2VpZ2h0OiA2MDA7XHJcbiAgICBjdXJzb3I6IHBvaW50ZXI7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZjVmNWY1O1xyXG59XHJcblxyXG4ubGl2ZSB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IGdyZWVuO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAtMXB4O1xyXG4gICAgcmlnaHQ6IDFweDtcclxuICAgIHRyYW5zZm9ybTogc2NhbGUoMSk7XHJcbiAgICBhbmltYXRpb246IHB1bHNlIDFzIGluZmluaXRlO1xyXG59XHJcblxyXG4uaW5hY3RpdmUge1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiBvcmFuZ2U7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IC0xcHg7XHJcbiAgICByaWdodDogMXB4O1xyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIHB1bHNlIHtcclxuICAgIDAlIHtcclxuICAgICAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwLjQpO1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwLjQpO1xyXG4gICAgfVxyXG5cclxuICAgIDcwJSB7XHJcbiAgICAgICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAxMHB4IHJnYmEoMjA0LDE2OSw0NCwgMCk7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMCAwIDAgMTBweCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgfVxyXG5cclxuICAgIDEwMCUge1xyXG4gICAgICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgICAgIGJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsMTY5LDQ0LCAwKTtcclxuICAgIH1cclxufVxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](WhereIsMyDriverMapViewComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-where-is-my-driver-map-view',
          templateUrl: './map-view.component.html',
          styleUrls: ['./map-view.component.css']
        }]
      }], function () {
        return [{
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_10__["BuyerwallyboardService"]
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
          args: [src_app_shared_components_sendbird_buyer_sendbird_buyer_sendbird_component__WEBPACK_IMPORTED_MODULE_5__["BuyerSendbirdComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/wally-dashboard/wally-dashboard.component.ts": function srcAppBuyerWallyBoardWallyDashboardWallyDashboardComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WallyDashboardComponent", function () {
      return WallyDashboardComponent;
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


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/directives/sorting.pipe */
    "./src/app/directives/sorting.pipe.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _buyer_locations_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../buyer-locations.component */
    "./src/app/buyer-wally-board/buyer-locations.component.ts");
    /* harmony import */


    var _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../where-is-my-driver.component */
    "./src/app/buyer-wally-board/where-is-my-driver.component.ts");
    /* harmony import */


    var _sales_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../sales.component */
    "./src/app/buyer-wally-board/sales.component.ts");

    function WallyDashboardComponent_div_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "input", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "label", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WallyDashboardComponent_div_15_Template_label_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r5);

          var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r4.changeWindowType(1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "i", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "input", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "label", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WallyDashboardComponent_div_15_Template_label_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r5);

          var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r6.changeWindowType(2);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "i", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", 1)("checked", ctx_r0.singleMulti == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", 2)("checked", ctx_r0.singleMulti == 2);
      }
    }

    function WallyDashboardComponent_app_buyer_locations_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-buyer-locations", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("singleMulti", ctx_r1.singleMulti);
      }
    }

    function WallyDashboardComponent_app_where_is_my_driver_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-where-is-my-driver", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading... ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("singleMulti", ctx_r2.singleMulti);
      }
    }

    function WallyDashboardComponent_app_sales_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-sales");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    var WallyDashboardComponent = /*#__PURE__*/function () {
      function WallyDashboardComponent(wallyBoardService, _activateRoute, customSortingService) {
        _classCallCheck(this, WallyDashboardComponent);

        this.wallyBoardService = wallyBoardService;
        this._activateRoute = _activateRoute;
        this.customSortingService = customSortingService;
        this.disableControl = false;
      }

      _createClass(WallyDashboardComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.checkWindowSelection(); //this.singleMulti = (localStorage.getItem('singleMulti')) ? +(localStorage.getItem('singleMulti')) : 1;

          var params = this._activateRoute.snapshot.queryParams;
          if (params && params.viewTypeFromDashboard) this.changeViewType(params.viewTypeFromDashboard);
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.customSortingService.configColumnDefsNullToBottom();
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(type) {
          localStorage.setItem('viewType', type);

          if (this.singleMulti === 2) {
            this.dispatcherDashboard = window.open("/Buyer/Job/BuyerWallyBoard", "_blank");
          } else {
            this.viewType = type;
          }
        }
      }, {
        key: "changeWindowType",
        value: function changeWindowType(type) {
          var _this45 = this;

          this.singleMulti = type;
          this.wallyBoardService.SingleMultiWindowSubject.next(type);

          if (type === 1 && +localStorage.getItem('singleMulti') !== 1) {
            setTimeout(function () {
              _this45.dispatcherDashboard.close();
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

      return WallyDashboardComponent;
    }();

    WallyDashboardComponent.??fac = function WallyDashboardComponent_Factory(t) {
      return new (t || WallyDashboardComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_2__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_4__["DatatableCustomSortingService"]));
    };

    WallyDashboardComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: WallyDashboardComponent,
      selectors: [["app-wally-dashboard"]],
      decls: 21,
      vars: 14,
      consts: [[1, "row"], [1, "col-sm-12"], [1, "sticky-header-dash", 3, "ngClass"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "pull-left", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], ["class", "dib switch-window ml20 pull-left mt5", 4, "ngIf"], [3, "singleMulti", 4, "ngIf"], [4, "ngIf"], [1, "dib", "switch-window", "ml20", "pull-left", "mt5"], [1, "btn-group"], ["name", "single", "type", "radio", 1, "hide-element", 3, "value", "checked"], ["placement", "bottom", "ngbTooltip", "Single Window", 1, "btn", "ml0", "first-icon", 3, "click"], [1, "far", "fa-window-maximize", "fs14", "mt3"], ["name", "multiple", "type", "radio", 1, "hide-element", 3, "value", "checked"], ["placement", "bottom", "ngbTooltip", "Multi Window", 1, "btn", "last-icon", 3, "click"], [1, "far", "fa-window-restore", "fs14", "mt3"], [3, "singleMulti"]],
      template: function WallyDashboardComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WallyDashboardComponent_Template_label_click_7_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](9, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WallyDashboardComponent_Template_label_click_10_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, "Loads");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](12, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WallyDashboardComponent_Template_label_click_13_listener() {
            return ctx.changeViewType(3);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14, "Sales");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, WallyDashboardComponent_div_15_Template, 8, 4, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, WallyDashboardComponent_app_buyer_locations_18_Template, 2, 1, "app-buyer-locations", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, WallyDashboardComponent_app_where_is_my_driver_19_Template, 2, 1, "app-where-is-my-driver", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](20, WallyDashboardComponent_app_sales_20_Template, 2, 0, "app-sales", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", ctx.viewType == 3 ? "col-sm-3" : "col-sm-3");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 1)("checked", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 3)("checked", ctx.viewType == 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 3);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_5__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbTooltip"], _buyer_locations_component__WEBPACK_IMPORTED_MODULE_7__["BuyerLocationsComponent"], _where_is_my_driver_component__WEBPACK_IMPORTED_MODULE_8__["WhereIsMyDriverComponent"], _sales_component__WEBPACK_IMPORTED_MODULE_9__["SalesComponent"]],
      styles: ["agm-map {\r\n    height: 60vh;\r\n}\r\n\r\n  .gmap_location {\r\n    position: relative;\r\n}\r\n\r\n  .filters_panel {\r\n    position: absolute;\r\n    z-index: 1;\r\n    top: 5px;\r\n    left: 0;\r\n    width: 100%;\r\n    padding: 10px 60px;\r\n}\r\n\r\n  .filters_panel .filter_div {\r\n        background: #ffffff;\r\n        color: #000000;\r\n        border-radius: 5px;\r\n        padding: 10px;\r\n    }\r\n\r\n  .filters_panel .filter_div label {\r\n            margin-bottom: 0;\r\n        }\r\n\r\n  .table .bg_must_go {\r\n    background-color: #f5d0d0;\r\n}\r\n\r\n  .table .bg_should_go {\r\n    background-color: #f7e6a9;\r\n}\r\n\r\n  .table .bg_could_go {\r\n    background-color: #e4e2e2;\r\n}\r\n\r\n  .filter-in-map {\r\n    background: lightgrey;\r\n    position: absolute;\r\n    width: 80%;\r\n    top: 20px;\r\n    left: 20px;\r\n    border-radius: 5px;\r\n}\r\n\r\n  .driver-list {\r\n    max-height: 335px;\r\n    overflow: auto;\r\n    margin-top: 10px;\r\n    padding: 0 8px;\r\n}\r\n\r\n  .driver-initials {\r\n    width: 36px;\r\n    height: 36px;\r\n    text-align: center;\r\n    display: flex;\r\n    align-items: center;\r\n    justify-content: center;\r\n}\r\n\r\n  .driver-details:hover {\r\n    background: #f7f7f7;\r\n    cursor: pointer;\r\n}\r\n\r\n.sticky-header-dash[_ngcontent-%COMP%] {\r\n    position: fixed;\r\n    padding: 10px 10px;\r\n    top: 45px;\r\n    left: 0;\r\n    height: 65px;\r\n    \r\n    z-index: 11;\r\n    background: #fff !important;\r\n    left: 0;\r\n    float: left;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   input[type=\"radio\"][_ngcontent-%COMP%]:checked    + label[_ngcontent-%COMP%] {\r\n    background-image: none;\r\n    background-color: white;\r\n    color: #1062d1;\r\n    border: 1px solid #1062d1;\r\n    border-radius: 5px 0 0 5px !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   label.last-icon[_ngcontent-%COMP%], .switch-window[_ngcontent-%COMP%]   input[type=\"radio\"][_ngcontent-%COMP%]:checked    + label.last-icon[_ngcontent-%COMP%] {\r\n    border-radius: 0 5px 5px 0 !important;\r\n    margin-left: 0 !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   label.first-icon[_ngcontent-%COMP%] {\r\n    border-radius: 5px 0 0 5px !important;\r\n}\r\n\r\n.switch-window[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\r\n    padding: 5px 10px !important;\r\n    background-image: none;\r\n    background-color: white;\r\n    color: #D1D1D1;\r\n    border: 1px solid #D1D1D1;\r\n    border-radius: 5px 0 0 5px;\r\n    cursor: pointer;\r\n}\r\n\r\n.multiselect-dropdown[_ngcontent-%COMP%]   .selected-item[_ngcontent-%COMP%]{\r\n    max-width: 65%;\r\n    white-space: nowrap;\r\n    overflow: hidden;\r\n    text-overflow: ellipsis;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2FsbHktZGFzaGJvYXJkL3dhbGx5LWRhc2hib2FyZC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtBQUNoQjs7QUFFQTtJQUNJLGtCQUFrQjtBQUN0Qjs7QUFFQTtJQUNJLGtCQUFrQjtJQUNsQixVQUFVO0lBQ1YsUUFBUTtJQUNSLE9BQU87SUFDUCxXQUFXO0lBQ1gsa0JBQWtCO0FBQ3RCOztBQUVJO1FBQ0ksbUJBQW1CO1FBQ25CLGNBQWM7UUFDZCxrQkFBa0I7UUFDbEIsYUFBYTtJQUNqQjs7QUFFSTtZQUNJLGdCQUFnQjtRQUNwQjs7QUFFUjtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHlCQUF5QjtBQUM3Qjs7QUFFQTtJQUNJLHFCQUFxQjtJQUNyQixrQkFBa0I7SUFDbEIsVUFBVTtJQUNWLFNBQVM7SUFDVCxVQUFVO0lBQ1Ysa0JBQWtCO0FBQ3RCOztBQUVBO0lBQ0ksaUJBQWlCO0lBQ2pCLGNBQWM7SUFDZCxnQkFBZ0I7SUFDaEIsY0FBYztBQUNsQjs7QUFFQTtJQUNJLFdBQVc7SUFDWCxZQUFZO0lBQ1osa0JBQWtCO0lBQ2xCLGFBQWE7SUFDYixtQkFBbUI7SUFDbkIsdUJBQXVCO0FBQzNCOztBQUVBO0lBQ0ksbUJBQW1CO0lBQ25CLGVBQWU7QUFDbkI7O0FBRUE7SUFDSSxlQUFlO0lBQ2Ysa0JBQWtCO0lBQ2xCLFNBQVM7SUFDVCxPQUFPO0lBQ1AsWUFBWTtJQUNaLG1CQUFtQjtJQUNuQixXQUFXO0lBQ1gsMkJBQTJCO0lBQzNCLE9BQU87SUFDUCxXQUFXO0FBQ2Y7O0FBRUE7SUFDSSxzQkFBc0I7SUFDdEIsdUJBQXVCO0lBQ3ZCLGNBQWM7SUFDZCx5QkFBeUI7SUFDekIscUNBQXFDO0FBQ3pDOztBQUVDOztJQUVHLHFDQUFxQztJQUNyQyx5QkFBeUI7QUFDN0I7O0FBRUE7SUFDSSxxQ0FBcUM7QUFDekM7O0FBR0E7SUFDSSw0QkFBNEI7SUFDNUIsc0JBQXNCO0lBQ3RCLHVCQUF1QjtJQUN2QixjQUFjO0lBQ2QseUJBQXlCO0lBQ3pCLDBCQUEwQjtJQUMxQixlQUFlO0FBQ25COztBQUVBO0lBQ0ksY0FBYztJQUNkLG1CQUFtQjtJQUNuQixnQkFBZ0I7SUFDaEIsdUJBQXVCO0FBQzNCIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2FsbHktZGFzaGJvYXJkL3dhbGx5LWRhc2hib2FyZC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIGFnbS1tYXAge1xyXG4gICAgaGVpZ2h0OiA2MHZoO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmdtYXBfbG9jYXRpb24ge1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmZpbHRlcnNfcGFuZWwge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgei1pbmRleDogMTtcclxuICAgIHRvcDogNXB4O1xyXG4gICAgbGVmdDogMDtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcGFkZGluZzogMTBweCA2MHB4O1xyXG59XHJcblxyXG4gICAgOjpuZy1kZWVwIC5maWx0ZXJzX3BhbmVsIC5maWx0ZXJfZGl2IHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjZmZmZmZmO1xyXG4gICAgICAgIGNvbG9yOiAjMDAwMDAwO1xyXG4gICAgICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgICAgICBwYWRkaW5nOiAxMHB4O1xyXG4gICAgfVxyXG5cclxuICAgICAgICA6Om5nLWRlZXAgLmZpbHRlcnNfcGFuZWwgLmZpbHRlcl9kaXYgbGFiZWwge1xyXG4gICAgICAgICAgICBtYXJnaW4tYm90dG9tOiAwO1xyXG4gICAgICAgIH1cclxuXHJcbjo6bmctZGVlcCAudGFibGUgLmJnX211c3RfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2Y1ZDBkMDtcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJsZSAuYmdfc2hvdWxkX2dvIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNmN2U2YTk7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAudGFibGUgLmJnX2NvdWxkX2dvIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6ICNlNGUyZTI7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAuZmlsdGVyLWluLW1hcCB7XHJcbiAgICBiYWNrZ3JvdW5kOiBsaWdodGdyZXk7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB3aWR0aDogODAlO1xyXG4gICAgdG9wOiAyMHB4O1xyXG4gICAgbGVmdDogMjBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxufVxyXG5cclxuOjpuZy1kZWVwIC5kcml2ZXItbGlzdCB7XHJcbiAgICBtYXgtaGVpZ2h0OiAzMzVweDtcclxuICAgIG92ZXJmbG93OiBhdXRvO1xyXG4gICAgbWFyZ2luLXRvcDogMTBweDtcclxuICAgIHBhZGRpbmc6IDAgOHB4O1xyXG59XHJcblxyXG46Om5nLWRlZXAgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICB3aWR0aDogMzZweDtcclxuICAgIGhlaWdodDogMzZweDtcclxuICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgIGRpc3BsYXk6IGZsZXg7XHJcbiAgICBhbGlnbi1pdGVtczogY2VudGVyO1xyXG4gICAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XHJcbn1cclxuXHJcbjo6bmctZGVlcCAuZHJpdmVyLWRldGFpbHM6aG92ZXIge1xyXG4gICAgYmFja2dyb3VuZDogI2Y3ZjdmNztcclxuICAgIGN1cnNvcjogcG9pbnRlcjtcclxufVxyXG5cclxuLnN0aWNreS1oZWFkZXItZGFzaCB7XHJcbiAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbiAgICBwYWRkaW5nOiAxMHB4IDEwcHg7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBsZWZ0OiAwO1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgLypmb250LXNpemU6IDIwcHg7Ki9cclxuICAgIHotaW5kZXg6IDExO1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZiAhaW1wb3J0YW50O1xyXG4gICAgbGVmdDogMDtcclxuICAgIGZsb2F0OiBsZWZ0O1xyXG59XHJcblxyXG4uc3dpdGNoLXdpbmRvdyBpbnB1dFt0eXBlPVwicmFkaW9cIl06Y2hlY2tlZCArIGxhYmVsIHtcclxuICAgIGJhY2tncm91bmQtaW1hZ2U6IG5vbmU7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiB3aGl0ZTtcclxuICAgIGNvbG9yOiAjMTA2MmQxO1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgIzEwNjJkMTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweCAwIDAgNXB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbiAuc3dpdGNoLXdpbmRvdyBsYWJlbC5sYXN0LWljb24sXHJcbiAuc3dpdGNoLXdpbmRvdyBpbnB1dFt0eXBlPVwicmFkaW9cIl06Y2hlY2tlZCArIGxhYmVsLmxhc3QtaWNvbiB7XHJcbiAgICBib3JkZXItcmFkaXVzOiAwIDVweCA1cHggMCAhaW1wb3J0YW50O1xyXG4gICAgbWFyZ2luLWxlZnQ6IDAgIWltcG9ydGFudDtcclxufVxyXG5cclxuLnN3aXRjaC13aW5kb3cgbGFiZWwuZmlyc3QtaWNvbiB7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHggMCAwIDVweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG5cclxuLnN3aXRjaC13aW5kb3cgLmJ0biB7XHJcbiAgICBwYWRkaW5nOiA1cHggMTBweCAhaW1wb3J0YW50O1xyXG4gICAgYmFja2dyb3VuZC1pbWFnZTogbm9uZTtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IHdoaXRlO1xyXG4gICAgY29sb3I6ICNEMUQxRDE7XHJcbiAgICBib3JkZXI6IDFweCBzb2xpZCAjRDFEMUQxO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNXB4IDAgMCA1cHg7XHJcbiAgICBjdXJzb3I6IHBvaW50ZXI7XHJcbn1cclxuXHJcbi5tdWx0aXNlbGVjdC1kcm9wZG93biAuc2VsZWN0ZWQtaXRlbXtcclxuICAgIG1heC13aWR0aDogNjUlO1xyXG4gICAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcclxuICAgIG92ZXJmbG93OiBoaWRkZW47XHJcbiAgICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcclxufSJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](WallyDashboardComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-wally-dashboard',
          templateUrl: './wally-dashboard.component.html',
          styleUrls: ['./wally-dashboard.component.css']
        }]
      }], function () {
        return [{
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_2__["BuyerwallyboardService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_3__["ActivatedRoute"]
        }, {
          type: src_app_directives_sorting_pipe__WEBPACK_IMPORTED_MODULE_4__["DatatableCustomSortingService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/buyer-wally-board/where-is-my-driver.component.ts": function srcAppBuyerWallyBoardWhereIsMyDriverComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "WhereIsMyDriverComponent", function () {
      return WhereIsMyDriverComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/sendbird.component */
    "./src/app/shared-components/sendbird/sendbird.component.ts");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../declarations.module */
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


    var _Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./Models/BuyerWallyBoard */
    "./src/app/buyer-wally-board/Models/BuyerWallyBoard.ts");
    /* harmony import */


    var _wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./wally-dashboard/map-view.component */
    "./src/app/buyer-wally-board/wally-dashboard/map-view.component.ts");
    /* harmony import */


    var _wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./wally-dashboard/grid-view.component */
    "./src/app/buyer-wally-board/wally-dashboard/grid-view.component.ts");
    /* harmony import */


    var _app_constants__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../app.constants */
    "./src/app/app.constants.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./services/buyerwallyboard.service */
    "./src/app/buyer-wally-board/services/buyerwallyboard.service.ts");
    /* harmony import */


    var src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! src/app/shared-components/sendbird/services/sendbird.service */
    "./src/app/shared-components/sendbird/services/sendbird.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    var _c0 = ["SelectedDriverLoad"];

    function WhereIsMyDriverComponent_div_9_ng_template_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r9.FilterForm.controls["SelectedCarriers"])("settings", ctx_r9.PriorityDdlSettings)("placeholder", "Select Carrier")("data", ctx_r9.Carriers);
      }
    }

    function WhereIsMyDriverComponent_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2, "Select Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](3, WhereIsMyDriverComponent_div_9_ng_template_3_Template, 2, 4, "ng-template", null, 17, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r8)("autoClose", "outside");
      }
    }

    function WhereIsMyDriverComponent_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r2.filterCount);
      }
    }

    function WhereIsMyDriverComponent_a_19_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "a", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WhereIsMyDriverComponent_a_19_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r10.toggleMapView();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", ctx_r3.FilterForm.get("ToggleMap").value ? "Hide Map" : "Show Map", " ");
      }
    }

    function WhereIsMyDriverComponent_ng_template_20_Template(rf, ctx) {
      if (rf & 1) {
        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "State");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "Supplier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](11, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16, "Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](17, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "From");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "input", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDateChange", function WhereIsMyDriverComponent_ng_template_20_Template_input_onDateChange_23_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r13);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r12.setFromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](27, "To");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "input", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDateChange", function WhereIsMyDriverComponent_ng_template_20_Template_input_onDateChange_28_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r13);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r14.setToDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](32, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](33, "Priority");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](34, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](36, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "label", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](39, "ng-multiselect-dropdown", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "button", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WhereIsMyDriverComponent_ng_template_20_Template_button_click_42_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r13);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r15.ResetLoadsFilters();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](43, " Reset ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "button", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WhereIsMyDriverComponent_ng_template_20_Template_button_click_44_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r13);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](15);

          ctx_r16.ApplyLoadsFilters("set");
          return _r1.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](45, " Save ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r5.FilterForm.controls["SelectedStates"])("settings", ctx_r5.StateDdlSettings)("placeholder", "Select States")("data", ctx_r5.States);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r5.FilterForm.controls["SelectedSuppliers"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Select Supplier")("data", ctx_r5.Suppliers);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r5.FilterForm.controls["SelectedLocations"])("settings", ctx_r5.LocationDdlSettings)("placeholder", "Select Location")("data", ctx_r5.Locations);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("format", "MM/DD/YYYY")("maxDate", ctx_r5.MaxInputDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("format", "MM/DD/YYYY")("maxDate", ctx_r5.MaxInputDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r5.FilterForm.controls["SelectedPriorities"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Select Priority")("data", ctx_r5.LoadPriorities);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formControl", ctx_r5.FilterForm.controls["selectedLocAttributeList"])("settings", ctx_r5.PriorityDdlSettings)("placeholder", "Inventory Capture Method")("data", ctx_r5.LocationAttributeList);
      }
    }

    function WhereIsMyDriverComponent_app_where_is_my_driver_map_view_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-where-is-my-driver-map-view", 39);
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("singleMulti", ctx_r6.singleMulti)("FilterForm", ctx_r6.FilterForm);
      }
    }

    function WhereIsMyDriverComponent_app_where_is_my_driver_grid_view_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-where-is-my-driver-grid-view", 39);
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("singleMulti", ctx_r7.singleMulti)("FilterForm", ctx_r7.FilterForm);
      }
    }

    var WhereIsMyDriverComponent = /*#__PURE__*/function () {
      function WhereIsMyDriverComponent(fb, dispatcherService, chatService, carrierService) {
        _classCallCheck(this, WhereIsMyDriverComponent);

        this.fb = fb;
        this.dispatcherService = dispatcherService;
        this.chatService = chatService;
        this.carrierService = carrierService;
        this.previousInfowindow = null;
        this.previousInfowindowIndex = null;
        this.zoomLevel = 4;
        this.centerLoactionLat = 39.1175;
        this.centerLoactionLng = -103.8784;
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_3__().add(1, 'year').toDate();
        this.TodaysDate = moment__WEBPACK_IMPORTED_MODULE_3__().format('MM/DD/YYYY');
        this.AUTO_REFRESH_TIME = 300; // seconds

        this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
        this.LocationAttributeList = _app_constants__WEBPACK_IMPORTED_MODULE_11__["InventoryDataCaptureList"];
        this.selectedLocAttributeList = [];
        this.driverModal = {
          modalDetails: {
            display: 'none',
            data: new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_7__["WhereIsMyDriverModel"]()
          }
        };
        this.screenOptions = {
          position: 6
        };
        this.subscriptions = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subscription"]();
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
        this.Locations = [];
        this.DefaultLocations = [];
        this.States = [];
        this.Suppliers = [];
        this.Carriers = [];
        this.LoadPriorities = _app_constants__WEBPACK_IMPORTED_MODULE_11__["LoadPriorities"];
        this.StateDdlSettings = {};
        this.PriorityDdlSettings = {};
        this.LocationDdlSettings = {};
        this.SelectedPrioritiesId = [];
        this.toogleFilter = false;
        this.dtMustGoOptions = {};
        this.dtShouldGoOptions = {};
        this.dtCouldGoOptions = {};
        this.selectedDriverLoadsdtOptions = {};
        this.selectedDriverLoadsdtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.dtMustGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.dtShouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.dtCouldGoTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.loadingData = true;
        this.modalData = true;
        this.backgroudchatDefault = [];
        this.memberInfo = [];
        this.disableControl = false;
        this.viewLoadType = 1;
        this.loadScreenType = 'All';
        this.carrierList = [];
        this.IsDataLoaded = false;
        this.singleMulti = localStorage.getItem('singleMulti') ? +localStorage.getItem('singleMulti') : 1;
        if (this.singleMulti == 1) this.loadScreenType = 'All';

        var _this = this;

        window.addEventListener("beforeunload", function (e) {
          _this.SaveFilters(true);

          return;
        });
        this.setFilterForm();
      }

      _createClass(WhereIsMyDriverComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.readOnlyModeSelection();
          this.StateDdlSettings = {
            singleSelection: false,
            idField: 'Id',
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
          this.restoreFilterStates();
          this.subscribeFormChanges();
          this.GetFilters(); //this.getFilterData();
        }
      }, {
        key: "setFilterForm",
        value: function setFilterForm() {
          var toDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY) : this.TodaysDate;
          var fromDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY) : this.TodaysDate;
          this.FilterForm = this.fb.group({
            IsShowCarrierManaged: this.fb.control(false),
            ToggleMap: this.fb.control(true),
            ToggleExpandMapView: this.fb.control(false),
            ToggleGrids: this.fb.control(false),
            ToggleDriverView: this.fb.control(false),
            SelectedSuppliers: this.fb.control([]),
            SelectedCarriers: this.fb.control([]),
            SelectedLocations: this.fb.control([]),
            SelectedStates: this.fb.control([]),
            SelectedPriorities: this.fb.control([]),
            singleMulti: this.fb.control(this.singleMulti),
            FromDate: this.fb.control(fromDate),
            ToDate: this.fb.control(toDate),
            selectedLocAttributeList: this.fb.control([])
          });
        }
      }, {
        key: "subscribeFormChanges",
        value: function subscribeFormChanges() {
          var _this46 = this;

          // this.subscriptions.add(this.FilterForm.get('SelectedLocations').valueChanges
          //     .subscribe(change => {
          //         var selectedLocation = this.setSelectedLocationId();
          //         if (this.SelectedLocationId != selectedLocation) {
          //             this.locationChanged();
          //         }
          //     }))
          this.subscriptions.add(this.FilterForm.get('SelectedStates').valueChanges.subscribe(function (change) {
            var selectedStates = _this46.setSelectedStateId();

            if (_this46.SelectedStateId != selectedStates) {
              _this46.stateChanged();
            }
          }));
          this.subscriptions.add(this.FilterForm.get('SelectedSuppliers').valueChanges.subscribe(function (change) {
            var selectedSupplier = _this46.setSelectedSupplierId();

            if (_this46.SelectedSupplierId != selectedSupplier) {
              _this46.suppplierChanged();
            }
          }));
          this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(function (change) {
            _this46.ShowCarrierMangedData();
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
        key: "getFilterData",
        value: function getFilterData() {
          var _this47 = this;

          var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
          this.IsDataLoaded = false;
          this.dispatcherService.GetFilterData(isCarrierManaged).subscribe(function (data) {
            // this.IsDataLoaded = true;
            _this47.Locations = data;
            _this47.DefaultLocations = data;
            _this47.States = _this47.GetUniqueItems(data.map(function (t) {
              return t.States;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
            _this47.Suppliers = _this47.GetUniqueItems(data.map(function (t) {
              return t.Suppliers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));
            _this47.Carriers = _this47.GetUniqueItems(data.map(function (t) {
              return t.Carriers;
            }).reduce(function (p, n) {
              return p.concat(n);
            }, []));

            var selectedLocations = _this47.FilterForm.get('SelectedLocations').value;

            selectedLocations = selectedLocations.filter(function (t) {
              return _this47.Locations.filter(function (el) {
                return t.Id == el.Id;
              }).length > 0;
            });

            _this47.FilterForm.get('SelectedLocations').patchValue(selectedLocations);

            _this47.SelectedLocationId = _this47.setSelectedLocationId();

            _this47.setSelectedFilters();
          });
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
              this.loadScreenType = sessionStorage.getItem('loadScreenType'); //this.loadScreenType == "Grid" ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";

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
              sessionStorage.getItem('loadScreenType') ? this.loadScreenType = sessionStorage.getItem('loadScreenType') : 'All'; //  type == 'Grid' ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";
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
        key: "locationChanged",
        value: function locationChanged() {
          this.SelectedLocationId = this.setSelectedLocationId();
          this.setJobSuppliers(); //MyLocalStorage.setData(MyLocalStorage.WBF_LOCATION_KEY, this.SelectedLocations);
          //MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDSTATES_KEY, this.SelectedStates);
        }
      }, {
        key: "stateChanged",
        value: function stateChanged() {
          this.SelectedStateId = this.setSelectedStateId();
          this.onSelectStates();
        }
      }, {
        key: "suppplierChanged",
        value: function suppplierChanged() {
          this.SelectedStateId = this.setSelectedSupplierId();
          this.onSelectSupplier();
        }
      }, {
        key: "setSelectedLocationId",
        value: function setSelectedLocationId() {
          var ids = [];
          var selectedLocations = this.FilterForm.get('SelectedLocations').value;
          selectedLocations.forEach(function (res) {
            ids.push(res.Id);
          });
          return ids.join();
        }
      }, {
        key: "setSelectedStateId",
        value: function setSelectedStateId() {
          var ids = [];
          var selectedStates = this.FilterForm.get('SelectedStates').value;
          selectedStates.forEach(function (res) {
            ids.push(res.Id);
          });
          return ids.join();
        }
      }, {
        key: "setSelectedSupplierId",
        value: function setSelectedSupplierId() {
          var ids = [];
          var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value;
          selectedSuppliers.forEach(function (res) {
            ids.push(res.Id);
          });
          return ids.join();
        }
      }, {
        key: "setJobSuppliers",
        value: function setJobSuppliers() {
          var _this48 = this;

          this.Suppliers = [];
          this.Carriers = [];
          this.States = [];
          this.Locations.map(function (m) {
            var selectedLocations = _this48.FilterForm.get('SelectedLocations').value;

            if (selectedLocations.find(function (f) {
              return f.Id == m.Id;
            }) || selectedLocations.length == 0) {
              if (m && m.Suppliers && m.Suppliers.length > 0) {
                _this48.Suppliers = _this48.Suppliers.concat(m.Suppliers);
              }

              if (m && m.Carriers && m.Carriers.length > 0) {
                _this48.Carriers = _this48.Carriers.concat(m.Carriers);
              }

              if (m && m.States && m.States.length > 0) {
                _this48.States = _this48.States.concat(m.States);
              }
            }
          });
          this.States = this.GetUniqueItems(this.States.reduce(function (p, n) {
            return p.concat(n);
          }, []));
          this.Suppliers = this.GetUniqueItems(this.Suppliers.reduce(function (p, n) {
            return p.concat(n);
          }, []));
          this.Carriers = this.GetUniqueItems(this.Carriers.reduce(function (p, n) {
            return p.concat(n);
          }, []));

          if (this.IsDataLoaded) {
            this.setSelectedFilters();
          }
        }
      }, {
        key: "onSelectStates",
        value: function onSelectStates() {
          this.Suppliers = [];
          this.Locations = [];
          var selectedStates = this.FilterForm.get('SelectedStates').value;
          this.Locations = this.DefaultLocations.filter(function (t) {
            return selectedStates.some(function (t1) {
              return t1.Id == t.States[0].Id;
            });
          });

          if (!selectedStates || !selectedStates.length) {
            this.Locations = this.DefaultLocations;
          }

          this.Suppliers = this.Locations.map(function (t) {
            return t.Suppliers;
          });
          this.Locations = this.GetUniqueItems(this.Locations.reduce(function (p, n) {
            return p.concat(n);
          }, []));
          this.Suppliers = this.GetUniqueItems(this.Suppliers.reduce(function (p, n) {
            return p.concat(n);
          }, []));

          if (this.IsDataLoaded) {
            this.setSelectedFilters();
          }
        }
      }, {
        key: "onSelectSupplier",
        value: function onSelectSupplier() {
          var _this49 = this;

          var selectedStates = this.FilterForm.get('SelectedStates').value;
          var selectedSupplier = this.FilterForm.get('SelectedSuppliers').value;
          selectedStates.forEach(function (element) {
            _this49.Locations = _this49.DefaultLocations.filter(function (t) {
              return t.States.filter(function (t1) {
                return t1.Id == element.Id;
              }).length > 0;
            });
          });
          selectedSupplier.forEach(function (element) {
            _this49.Locations = _this49.Locations.filter(function (t) {
              return t.Suppliers.filter(function (t1) {
                return t1.Id == element.Id;
              }).length > 0;
            });
          });
          this.Locations = this.GetUniqueItems(this.Locations.reduce(function (p, n) {
            return p.concat(n);
          }, []));

          if (this.IsDataLoaded) {
            this.setSelectedFilters();
          }
        }
      }, {
        key: "setSelectedFilters",
        value: function setSelectedFilters() {
          var _this50 = this;

          var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value;
          var selectedCarriers = this.FilterForm.get('SelectedCarriers').value;
          var selectedStates = this.FilterForm.get('SelectedStates').value;
          selectedSuppliers = selectedSuppliers.filter(function (t) {
            return _this50.Suppliers.filter(function (el) {
              return t.Id == el.Id;
            }).length > 0;
          });
          selectedCarriers = selectedCarriers.filter(function (t) {
            return _this50.Carriers.filter(function (el) {
              return t.Id == el.Id;
            }).length > 0;
          });
          selectedStates = selectedStates.filter(function (t) {
            return _this50.States.filter(function (el) {
              return t.Id == el.Id;
            }).length > 0;
          });
          this.FilterForm.get('SelectedSuppliers').patchValue(selectedSuppliers);
          this.FilterForm.get('SelectedCarriers').patchValue(selectedCarriers);
          this.FilterForm.get('SelectedStates').patchValue(selectedStates);
          this.ApplyLoadsFilters();
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          if (event != '') {
            this.FilterForm.get('FromDate').patchValue(event);
          }

          var toDate = this.FilterForm.get('ToDate').value;
          var fromDate = this.FilterForm.get('FromDate').value;

          if (fromDate != '' && toDate != '' && _app_constants__WEBPACK_IMPORTED_MODULE_11__["RegExConstants"].DateFormat.test(fromDate) && _app_constants__WEBPACK_IMPORTED_MODULE_11__["RegExConstants"].DateFormat.test(toDate)) {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(toDate).toDate();

            if (_toDate < _fromDate) {
              this.FilterForm.get('ToDate').patchValue(event);
            }

            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
          }
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          if (event != '') {
            this.FilterForm.get('ToDate').patchValue(event);
          }

          this.FilterForm.get('ToDate').patchValue(event);
          var toDate = this.FilterForm.get('ToDate').value;
          var fromDate = this.FilterForm.get('FromDate').value;

          if (fromDate != '' && toDate != '' && _app_constants__WEBPACK_IMPORTED_MODULE_11__["RegExConstants"].DateFormat.test(fromDate) && _app_constants__WEBPACK_IMPORTED_MODULE_11__["RegExConstants"].DateFormat.test(toDate)) {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(toDate).toDate();

            if (_fromDate > _toDate) {
              this.FilterForm.get('FromDate').patchValue(event);
            }

            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].setData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
          }
        }
      }, {
        key: "toggleMapView",
        value: function toggleMapView() {
          var expandMapView = this.FilterForm.get('ToggleMap').value;
          this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
        }
      }, {
        key: "toggleFilterView",
        value: function toggleFilterView() {
          this.toogleFilter = !this.toogleFilter;
        }
      }, {
        key: "restoreFilterStates",
        value: function restoreFilterStates() {
          if (this.disableControl == true) {
            var _selectedDate = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].DSB_DATE_KEY);

            if (_selectedDate != '') {
              this.FilterForm.get('FromDate').patchValue(_selectedDate);
              this.FilterForm.get('ToDate').patchValue(_selectedDate);
            }
          }
        }
      }, {
        key: "GetFilters",
        value: function GetFilters() {
          var _this51 = this;

          this.dispatcherService.getFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].BuyerWallyboardLoads).subscribe(function (res) {
            if (res && res.length > 0) {
              _this51.SetFilters(res);
            } else {
              _this51.getFilterData();
            }
          });
        }
      }, {
        key: "SaveFilters",
        value: function SaveFilters(isTopFilter) {
          var _this52 = this;

          if (isTopFilter) {
            this.dispatcherService.getFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].BuyerWallyboardLoads).subscribe(function (res) {
              if (res && Object.keys(res).length > 0) {
                var IsShowCarrierManaged = _this52.FilterForm.get("IsShowCarrierManaged").value;

                var SelectedCarriers = _this52.FilterForm.get("SelectedCarriers").value || [];
                var jsonFilterForm = null;
                jsonFilterForm = JSON.parse(res);
                jsonFilterForm["IsShowCarrierManaged"] = IsShowCarrierManaged;
                jsonFilterForm["SelectedCarriers"] = SelectedCarriers;
                var filterModel = JSON.stringify(jsonFilterForm);

                _this52.dispatcherService.saveFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].BuyerWallyboardLoads, filterModel).subscribe(function (res) {
                  if (res) {}
                });
              }
            });
          } else {
            var filterModel = JSON.stringify(this.FilterForm.value);
            this.dispatcherService.saveFilters(_Models_BuyerWallyBoard__WEBPACK_IMPORTED_MODULE_8__["TfxModule"].BuyerWallyboardLoads, filterModel).subscribe(function (res) {
              if (res) {}
            });
          } // var filterData = this.FilterForm.value;
          // delete filterData["FromDate"];
          // delete filterData["ToDate"];
          // var filterModel = JSON.stringify(filterData);
          // this.dispatcherService.saveFilters(TfxModule.BuyerWallyboardLoads, filterModel).subscribe(res => {
          //     if (res) {
          //     }
          // });

        }
      }, {
        key: "SetFilters",
        value: function SetFilters(input) {
          var filterData = JSON.parse(input);
          this.FilterForm.patchValue(filterData);
          this.getFilterData();
          this.ApplyLoadsFilters();
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
            this.FilterForm.get('ToggleMap').patchValue(false); //this.toogleDriver = true;
          }
        }
      }, {
        key: "ShowCarrierMangedData",
        value: function ShowCarrierMangedData() {
          this.FilterForm.get('SelectedCarriers').patchValue([]);
          this.getFilterData();
          this.loadsGridView.applyLoadsFilters(this.FilterForm);
          this.loadsMapView.applyLoadsFilters(this.FilterForm);
        }
      }, {
        key: "ResetLoadsFilters",
        value: function ResetLoadsFilters() {
          var toDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_TODATE_KEY) : this.TodaysDate;
          var fromDate = this.singleMulti == 2 && src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY) ? src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].getData(src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_6__["MyLocalStorage"].WBF_FROMDATE_KEY) : this.TodaysDate;
          this.FilterForm.get('SelectedStates').patchValue([]);
          this.FilterForm.get('SelectedPriorities').patchValue([]);
          this.FilterForm.get('SelectedSuppliers').patchValue([]);
          this.FilterForm.get('SelectedLocations').patchValue([]);
          this.FilterForm.get('FromDate').patchValue(fromDate);
          this.FilterForm.get('ToDate').patchValue(toDate);
          this.FilterForm.get('selectedLocAttributeList').patchValue([]);
          this.ApplyLoadsFilters('reset');
        }
      }, {
        key: "ApplyLoadsFilters",
        value: function ApplyLoadsFilters(msg) {
          this.SaveFilters(false);
          this.filterCount = 0;

          if (this.FilterForm) {
            var selectedStates = this.FilterForm.get('SelectedStates').value || [];
            this.filterCount += selectedStates.length;
            var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value || [];
            this.filterCount += selectedSuppliers.length;
            var selectedLocations = this.FilterForm.get('SelectedLocations').value || [];
            this.filterCount += selectedLocations.length;
            var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
            this.filterCount += selectedPriorities.length;
            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
            this.filterCount += selectedLocAttributeList.length;
          }

          this.loadsGridView.applyLoadsFilters(this.FilterForm);
          this.loadsMapView.applyLoadsFilters(this.FilterForm);

          if (msg == "set") {
            _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess("Filter applied successfully", undefined, undefined);
          } else if (msg == "reset") {
            _declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msginfo("Filter reset successfully", undefined, undefined);
          }
        }
      }]);

      return WhereIsMyDriverComponent;
    }();

    WhereIsMyDriverComponent.??fac = function WhereIsMyDriverComponent_Factory(t) {
      return new (t || WhereIsMyDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_12__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_13__["BuyerwallyboardService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_14__["chatService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_15__["CarrierService"]));
    };

    WhereIsMyDriverComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: WhereIsMyDriverComponent,
      selectors: [["app-where-is-my-driver"]],
      viewQuery: function WhereIsMyDriverComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](_wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](_wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](_c0, true, angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_4__["SendbirdComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.loadsMapView = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.loadsGridView = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.selectedDriverLoad = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.sendbirdComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        singleMulti: "singleMulti"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["????NgOnChangesFeature"]],
      decls: 24,
      vars: 8,
      consts: [[1, "col-sm-9", "sticky-header-wmd"], [1, "row", 3, "formGroup"], [1, "col-sm-5", "pa0"], [1, "row"], [1, "col-sm-7"], [1, "form-check", "form-check-inline", "fs14", "mt5"], ["type", "checkbox", "id", "inlineCarrierManaged", "formControlName", "IsShowCarrierManaged", 1, "form-check-input"], ["for", "inlineCarrierManaged", 1, "form-check-label"], ["class", "mtm10", 4, "ngIf"], [1, "col-sm-5"], [1, "col-sm-2"], [1, "col-3", "pl0", "text-right", "pt2"], ["placement", "auto", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs14", "mr10", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], ["class", "circle-badge", 4, "ngIf"], ["class", "hide_show_map fs14 ml10", 3, "click", 4, "ngIf"], ["popContent", ""], [3, "singleMulti", "FilterForm", 4, "ngIf"], [1, "mtm10"], ["placement", "bottom", "popoverClass", "carrier-popover", 1, "fs14", "ml20", 3, "ngbPopover", "autoClose"], [1, "col-sm-12", "p-0"], [3, "formControl", "settings", "placeholder", "data"], [1, "circle-badge"], [1, "hide_show_map", "fs14", "ml10", 3, "click"], [1, "fas", "fa-eye", "mr5"], [1, "popover-details"], [1, "row", "border-bottom-2"], [1, "col-6", "pr-0"], [1, "form-group"], ["for", "exampleFormControlInput1", 1, "font-bold"], [1, "col-6"], [1, "row", "border-bottom-2", "mt10"], ["type", "text", "placeholder", "From Date", "myDatePicker", "", "formControlName", "FromDate", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], ["type", "text", "placeholder", "To Date", "myDatePicker", "", "formControlName", "ToDate", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], [1, "row", "mt10"], [1, "col-12", "text-right"], ["type", "button", 1, "btn", "btn-default", 3, "click"], ["type", "button", 1, "btn", "btn-primary", 3, "click"], [3, "singleMulti", "FilterForm"]],
      template: function WhereIsMyDriverComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8, " Carrier Managed Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, WhereIsMyDriverComponent_div_9_Template, 5, 2, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "a", 12, 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function WhereIsMyDriverComponent_Template_a_click_14_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r17);

            var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](15);

            return _r1.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "i", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, WhereIsMyDriverComponent_span_17_Template, 2, 1, "span", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, WhereIsMyDriverComponent_a_19_Template, 3, 1, "a", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](20, WhereIsMyDriverComponent_ng_template_20_Template, 46, 24, "ng-template", null, 17, _angular_core__WEBPACK_IMPORTED_MODULE_0__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](22, WhereIsMyDriverComponent_app_where_is_my_driver_map_view_22_Template, 1, 2, "app-where-is-my-driver-map-view", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](23, WhereIsMyDriverComponent_app_where_is_my_driver_grid_view_23_Template, 1, 2, "app-where-is-my-driver-grid-view", 18);
        }

        if (rf & 2) {
          var _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????reference"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.FilterForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FilterForm.get("IsShowCarrierManaged").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngbPopover", _r4)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.filterCount > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.singleMulti != 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.loadScreenType == "Map" || ctx.loadScreenType == "All");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.loadScreenType == "Grid" || ctx.loadScreenType == "All");
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_16__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_17__["NgbPopover"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_18__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["FormControlDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_12__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_19__["DatePicker"], _wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"], _wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"]],
      styles: [".driver-details:nth-child(5n+1) .driver-initials {\n  background: #f6af27;\n}\n\n.driver-details:nth-child(5n+2) .driver-initials {\n  background: #ab47bc;\n}\n\n.driver-details:nth-child(5n+3) .driver-initials {\n  background: #a5a5a5;\n}\n\n.driver-details:nth-child(5n+4) .driver-initials {\n  background: #dc4949;\n}\n\n.driver-details:nth-child(5n+5) .driver-initials {\n  background: #00897b;\n}\n\n.sticky-header-wmd {\n  position: fixed;\n  right: 0;\n  padding: 15px 20px;\n  top: 45px;\n  height: 65px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n}\n\n.locationfilter {\n  width: 100%;\n  position: absolute;\n  right: 4px;\n  border-radius: 5px;\n  font-size: 14px;\n  z-index: 1010;\n}\n\n.sticky_header {\n  position: sticky;\n  top: 45px;\n  padding: 5px;\n  font-size: 20px;\n  z-index: 10;\n  background: #fff;\n  margin-bottom: 0px;\n  margin-top: -10px;\n  box-shadow: 0 3px 15px 0 rgba(0, 0, 0, 0.1);\n  border-radius: 2px;\n}\n\n.display_hide {\n  display: none;\n  transition: opacity 1s ease-out;\n  opacity: 0;\n}\n\n.expand_map_btn {\n  position: absolute;\n  top: 1px;\n  right: 15px;\n  background: #fff;\n  border-radius: 2px 2px 2px 2px;\n  padding: 3px;\n  box-shadow: -2px 2px 6px 1px #aaa;\n  z-index: 1;\n}\n\n.driver_btn {\n  position: absolute;\n  top: 15px;\n  left: -35px;\n  background: white;\n  border-radius: 2px;\n  border-top-left-radius: 5px;\n  border-bottom-left-radius: 5px;\n  padding: 5px;\n  box-shadow: -4px 0px 4px 0px #aaaaaa;\n}\n\n.absolute_driver {\n  position: fixed;\n  width: 25%;\n  top: 100px;\n  right: 0;\n  background: #fff;\n  z-index: 11;\n  padding: 10px;\n  box-shadow: 0 3px 15px 0 rgba(0, 0, 0, 0.1);\n  border-radius: 10px;\n}\n\n.hide_absolute_driver {\n  width: 0;\n  right: -20px;\n}\n\n.activeRoute {\n  font-weight: 600;\n  cursor: pointer;\n  background: #f5f5f5;\n}\n\n.live {\n  height: 10px;\n  width: 10px;\n  border-radius: 50%;\n  background-color: green;\n  position: absolute;\n  top: -1px;\n  right: 1px;\n  transform: scale(1);\n  -webkit-animation: pulse 1s infinite;\n          animation: pulse 1s infinite;\n}\n\n.inactive {\n  height: 10px;\n  width: 10px;\n  border-radius: 50%;\n  background-color: orange;\n  position: absolute;\n  top: -1px;\n  right: 1px;\n}\n\n@-webkit-keyframes pulse {\n  0% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0.4);\n  }\n  70% {\n    box-shadow: 0 0 0 10px rgba(204, 169, 44, 0);\n  }\n  100% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0);\n  }\n}\n\n@keyframes pulse {\n  0% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0.4);\n  }\n  70% {\n    box-shadow: 0 0 0 10px rgba(204, 169, 44, 0);\n  }\n  100% {\n    box-shadow: 0 0 0 0 rgba(204, 169, 44, 0);\n  }\n}\n\n.carrier-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.carrier-popover.popover .popover-body {\n  padding: 10px;\n  border-radius: 5px;\n}\n\n.multiselect-dropdown .selected-item {\n  max-width: 65%;\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n}\n\n/* buyer-Loads Master-Filters starts here*/\n\n.master-filter.popover {\n  min-width: 425px;\n  max-width: 450px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.master-filter.popover .popover-body {\n  padding: 0;\n  border-radius: 5px;\n  background: #ffffff;\n}\n\n.master-filter.popover .popover-details {\n  padding: 15px;\n}\n\n.master-filter.popover .popover-details .font-bold {\n  font-weight: 600 !important;\n}\n\n.master-filter.popover .border-bottom-2 {\n  border-bottom: 2px solid #e7eaec !important;\n}\n\n.circle-badge {\n  position: absolute;\n  top: -11px;\n  left: -14px;\n  background: #fa9393;\n  border-radius: 50%;\n  font-size: 12px;\n  text-align: center;\n  color: white;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  width: 18px;\n  height: 18px;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvRDpcXFRGU2NvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlXFxTaXRlRnVlbC5FeGNoYW5nZS5Tb3VyY2VDb2RlXFxTaXRlRnVlbC5FeGNoYW5nZS5XZWIvc3JjXFxhcHBcXGJ1eWVyLXdhbGx5LWJvYXJkXFx3aGVyZS1pcy1teS1kcml2ZXIuY29tcG9uZW50LnNjc3MiLCJzcmMvYXBwL2J1eWVyLXdhbGx5LWJvYXJkL3doZXJlLWlzLW15LWRyaXZlci5jb21wb25lbnQuc2NzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNJLG1CQUFBO0FDQ0o7O0FERUE7RUFDSSxtQkFBQTtBQ0NKOztBREVBO0VBQ0ksbUJBQUE7QUNDSjs7QURFQTtFQUNJLG1CQUFBO0FDQ0o7O0FERUE7RUFDSSxtQkFBQTtBQ0NKOztBREdBO0VBQ0ksZUFBQTtFQUNBLFFBQUE7RUFDQSxrQkFBQTtFQUNBLFNBQUE7RUFDQSxZQUFBO0VBQ0EsZUFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtBQ0FKOztBRElBO0VBQ0ksV0FBQTtFQUNBLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGFBQUE7QUNESjs7QURJQTtFQUVJLGdCQUFBO0VBQ0EsU0FBQTtFQUNBLFlBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtFQUNBLGdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxpQkFBQTtFQUNBLDJDQUFBO0VBQ0Esa0JBQUE7QUNESjs7QURJQTtFQUNJLGFBQUE7RUFDQSwrQkFBQTtFQUNBLFVBQUE7QUNESjs7QURJQTtFQUNJLGtCQUFBO0VBQ0EsUUFBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtFQUNBLDhCQUFBO0VBQ0EsWUFBQTtFQUNBLGlDQUFBO0VBQ0EsVUFBQTtBQ0RKOztBREtBO0VBQ0ksa0JBQUE7RUFDQSxTQUFBO0VBQ0EsV0FBQTtFQUNBLGlCQUFBO0VBQ0Esa0JBQUE7RUFDQSwyQkFBQTtFQUNBLDhCQUFBO0VBQ0EsWUFBQTtFQUNBLG9DQUFBO0FDRko7O0FES0E7RUFDSSxlQUFBO0VBQ0EsVUFBQTtFQUNBLFVBQUE7RUFDQSxRQUFBO0VBQ0EsZ0JBQUE7RUFDQSxXQUFBO0VBQ0EsYUFBQTtFQUNBLDJDQUFBO0VBQ0EsbUJBQUE7QUNGSjs7QURLQTtFQUNJLFFBQUE7RUFDQSxZQUFBO0FDRko7O0FES0E7RUFDSSxnQkFBQTtFQUNBLGVBQUE7RUFDQSxtQkFBQTtBQ0ZKOztBREtBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSxrQkFBQTtFQUNBLHVCQUFBO0VBQ0Esa0JBQUE7RUFDQSxTQUFBO0VBQ0EsVUFBQTtFQUNBLG1CQUFBO0VBQ0Esb0NBQUE7VUFBQSw0QkFBQTtBQ0ZKOztBREtBO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSxrQkFBQTtFQUNBLHdCQUFBO0VBQ0Esa0JBQUE7RUFDQSxTQUFBO0VBQ0EsVUFBQTtBQ0ZKOztBREtBO0VBQ0k7SUFFSSwyQ0FBQTtFQ0ZOO0VES0U7SUFFSSw0Q0FBQTtFQ0hOO0VETUU7SUFFSSx5Q0FBQTtFQ0pOO0FBQ0Y7O0FEVkE7RUFDSTtJQUVJLDJDQUFBO0VDRk47RURLRTtJQUVJLDRDQUFBO0VDSE47RURNRTtJQUVJLHlDQUFBO0VDSk47QUFDRjs7QURRQTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDTko7O0FEU0E7RUFDSSxhQUFBO0VBQ0Esa0JBQUE7QUNOSjs7QURVQTtFQUNJLGNBQUE7RUFDQSxtQkFBQTtFQUNBLGdCQUFBO0VBQ0EsdUJBQUE7QUNQSjs7QURVQSwwQ0FBQTs7QUFHSTtFQUNJLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDVFI7O0FEV1E7RUFJSSxVQUFBO0VBQ0Esa0JBQUE7RUFDQSxtQkFBQTtBQ1paOztBRGVRO0VBQ0ksYUFBQTtBQ2JaOztBRGdCWTtFQUNJLDJCQUFBO0FDZGhCOztBRGtCUTtFQUNJLDJDQUFBO0FDaEJaOztBRHFCQTtFQUNJLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0VBQ0EsZUFBQTtFQUNBLGtCQUFBO0VBQ0EsWUFBQTtFQUNBLG9CQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtFQUNBLFdBQUE7RUFDQSxZQUFBO0FDbEJKIiwiZmlsZSI6InNyYy9hcHAvYnV5ZXItd2FsbHktYm9hcmQvd2hlcmUtaXMtbXktZHJpdmVyLmNvbXBvbmVudC5zY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1bisxKSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICNmNmFmMjc7XHJcbn1cclxuXHJcbi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rMikgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjYWI0N2JjO1xyXG59XHJcblxyXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzMpIC5kcml2ZXItaW5pdGlhbHMge1xyXG4gICAgYmFja2dyb3VuZDogI2E1YTVhNTtcclxufVxyXG5cclxuLmRyaXZlci1kZXRhaWxzOm50aC1jaGlsZCg1bis0KSAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgIGJhY2tncm91bmQ6ICNkYzQ5NDk7XHJcbn1cclxuXHJcbi5kcml2ZXItZGV0YWlsczpudGgtY2hpbGQoNW4rNSkgLmRyaXZlci1pbml0aWFscyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjMDA4OTdiO1xyXG59XHJcblxyXG5cclxuLnN0aWNreS1oZWFkZXItd21kIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHJpZ2h0OiAwO1xyXG4gICAgcGFkZGluZzogMTVweCAyMHB4O1xyXG4gICAgdG9wOiA0NXB4O1xyXG4gICAgaGVpZ2h0OiA2NXB4O1xyXG4gICAgZm9udC1zaXplOiAyMHB4O1xyXG4gICAgei1pbmRleDogMTA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG59XHJcblxyXG5cclxuLmxvY2F0aW9uZmlsdGVyIHtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgcmlnaHQ6IDRweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDVweDtcclxuICAgIGZvbnQtc2l6ZTogMTRweDtcclxuICAgIHotaW5kZXg6IDEwMTA7XHJcbn1cclxuXHJcbi5zdGlja3lfaGVhZGVyIHtcclxuICAgIHBvc2l0aW9uOiAtd2Via2l0LXN0aWNreTtcclxuICAgIHBvc2l0aW9uOiBzdGlja3k7XHJcbiAgICB0b3A6IDQ1cHg7XHJcbiAgICBwYWRkaW5nOiA1cHg7XHJcbiAgICBmb250LXNpemU6IDIwcHg7XHJcbiAgICB6LWluZGV4OiAxMDtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAwcHg7XHJcbiAgICBtYXJnaW4tdG9wOiAtMTBweDtcclxuICAgIGJveC1zaGFkb3c6IDAgM3B4IDE1cHggMCByZ2JhKDAsMCwwLC4xKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDJweDtcclxufVxyXG5cclxuLmRpc3BsYXlfaGlkZSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG4gICAgdHJhbnNpdGlvbjogb3BhY2l0eSAxcyBlYXNlLW91dDtcclxuICAgIG9wYWNpdHk6IDA7XHJcbn1cclxuXHJcbi5leHBhbmRfbWFwX2J0biB7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IDFweDtcclxuICAgIHJpZ2h0OiAxNXB4O1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxuICAgIGJvcmRlci1yYWRpdXM6IDJweCAycHggMnB4IDJweDtcclxuICAgIHBhZGRpbmc6IDNweDtcclxuICAgIGJveC1zaGFkb3c6IC0ycHggMnB4IDZweCAxcHggI2FhYTtcclxuICAgIHotaW5kZXg6IDE7XHJcbn1cclxuXHJcblxyXG4uZHJpdmVyX2J0biB7XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IDE1cHg7XHJcbiAgICBsZWZ0OiAtMzVweDtcclxuICAgIGJhY2tncm91bmQ6IHdoaXRlO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMnB4O1xyXG4gICAgYm9yZGVyLXRvcC1sZWZ0LXJhZGl1czogNXB4O1xyXG4gICAgYm9yZGVyLWJvdHRvbS1sZWZ0LXJhZGl1czogNXB4O1xyXG4gICAgcGFkZGluZzogNXB4O1xyXG4gICAgYm94LXNoYWRvdzogLTRweCAwcHggNHB4IDBweCAjYWFhYWFhO1xyXG59XHJcblxyXG4uYWJzb2x1dGVfZHJpdmVyIHtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHdpZHRoOiAyNSU7XHJcbiAgICB0b3A6IDEwMHB4O1xyXG4gICAgcmlnaHQ6IDA7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG4gICAgei1pbmRleDogMTE7XHJcbiAgICBwYWRkaW5nOiAxMHB4O1xyXG4gICAgYm94LXNoYWRvdzogMCAzcHggMTVweCAwIHJnYmEoMCwwLDAsLjEpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxufVxyXG5cclxuLmhpZGVfYWJzb2x1dGVfZHJpdmVyIHtcclxuICAgIHdpZHRoOiAwO1xyXG4gICAgcmlnaHQ6IC0yMHB4O1xyXG59XHJcblxyXG4uYWN0aXZlUm91dGUge1xyXG4gICAgZm9udC13ZWlnaHQ6IDYwMDtcclxuICAgIGN1cnNvcjogcG9pbnRlcjtcclxuICAgIGJhY2tncm91bmQ6ICNmNWY1ZjU7XHJcbn1cclxuXHJcbi5saXZlIHtcclxuICAgIGhlaWdodDogMTBweDtcclxuICAgIHdpZHRoOiAxMHB4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogZ3JlZW47XHJcbiAgICBwb3NpdGlvbjogYWJzb2x1dGU7XHJcbiAgICB0b3A6IC0xcHg7XHJcbiAgICByaWdodDogMXB4O1xyXG4gICAgdHJhbnNmb3JtOiBzY2FsZSgxKTtcclxuICAgIGFuaW1hdGlvbjogcHVsc2UgMXMgaW5maW5pdGU7XHJcbn1cclxuXHJcbi5pbmFjdGl2ZSB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IG9yYW5nZTtcclxuICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgIHRvcDogLTFweDtcclxuICAgIHJpZ2h0OiAxcHg7XHJcbn1cclxuXHJcbkBrZXlmcmFtZXMgcHVsc2Uge1xyXG4gICAgMCUge1xyXG4gICAgICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwxNjksNDQsIDAuNCk7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwxNjksNDQsIDAuNCk7XHJcbiAgICB9XHJcblxyXG4gICAgNzAlIHtcclxuICAgICAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDEwcHggcmdiYSgyMDQsMTY5LDQ0LCAwKTtcclxuICAgICAgICBib3gtc2hhZG93OiAwIDAgMCAxMHB4IHJnYmEoMjA0LDE2OSw0NCwgMCk7XHJcbiAgICB9XHJcblxyXG4gICAgMTAwJSB7XHJcbiAgICAgICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LDE2OSw0NCwgMCk7XHJcbiAgICAgICAgYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwxNjksNDQsIDApO1xyXG4gICAgfVxyXG59XHJcblxyXG5cclxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcclxuICAgIG1pbi13aWR0aDogMzAwcHg7XHJcbiAgICBtYXgtd2lkdGg6IDM1MHB4O1xyXG4gICAgYmFja2dyb3VuZDogI0Y5RjlGOTtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYigwLCAwLCAwLCAwLjEzKTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbn1cclxuXHJcbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcclxuICAgIHBhZGRpbmc6IDEwcHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuXHJcblxyXG4ubXVsdGlzZWxlY3QtZHJvcGRvd24gLnNlbGVjdGVkLWl0ZW0ge1xyXG4gICAgbWF4LXdpZHRoOiA2NSU7XHJcbiAgICB3aGl0ZS1zcGFjZTogbm93cmFwO1xyXG4gICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgIHRleHQtb3ZlcmZsb3c6IGVsbGlwc2lzO1xyXG59XHJcblxyXG4vKiBidXllci1Mb2FkcyBNYXN0ZXItRmlsdGVycyBzdGFydHMgaGVyZSovXHJcblxyXG4ubWFzdGVyLWZpbHRlciB7XHJcbiAgICAmLnBvcG92ZXIge1xyXG4gICAgICAgIG1pbi13aWR0aDogNDI1cHg7XHJcbiAgICAgICAgbWF4LXdpZHRoOiA0NTBweDtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xyXG4gICAgICAgIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XHJcbiAgICAgICAgYm94LXNpemluZzogYm9yZGVyLWJveDtcclxuICAgICAgICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiKDAsIDAsIDAsIDAuMTMpO1xyXG4gICAgICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcblxyXG4gICAgICAgIC5wb3BvdmVyLWJvZHkge1xyXG4gICAgICAgICAgICAvLyBtYXgtaGVpZ2h0OiAzNTBweDtcclxuICAgICAgICAgICAgLy8gb3ZlcmZsb3cteTogYXV0bztcclxuICAgICAgICAgICAgLy8gb3ZlcmZsb3cteDogaGlkZGVuO1xyXG4gICAgICAgICAgICBwYWRkaW5nOiAwO1xyXG4gICAgICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICAgICAgICAgIGJhY2tncm91bmQ6ICNmZmZmZmY7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICAgICAgcGFkZGluZzogMTVweDtcclxuICAgICAgICAgICAgLy8gbWF4LWhlaWdodDogMzEwcHg7XHJcbiAgICAgICAgICAgIC8vIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICAgICAgICAgIC5mb250LWJvbGQge1xyXG4gICAgICAgICAgICAgICAgZm9udC13ZWlnaHQ6IDYwMCAhaW1wb3J0YW50O1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAuYm9yZGVyLWJvdHRvbS0yIHtcclxuICAgICAgICAgICAgYm9yZGVyLWJvdHRvbTogMnB4IHNvbGlkICNlN2VhZWMgIWltcG9ydGFudDtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbn1cclxuXHJcbi5jaXJjbGUtYmFkZ2Uge1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAtMTFweDtcclxuICAgIGxlZnQ6IC0xNHB4O1xyXG4gICAgYmFja2dyb3VuZDogcmdiKDI1MCwgMTQ3LCAxNDcpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgZm9udC1zaXplOiAxMnB4O1xyXG4gICAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG4gICAgY29sb3I6IHdoaXRlO1xyXG4gICAgZGlzcGxheTogaW5saW5lLWZsZXg7XHJcbiAgICBhbGlnbi1pdGVtczogY2VudGVyO1xyXG4gICAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XHJcbiAgICB3aWR0aDogMThweDtcclxuICAgIGhlaWdodDogMThweFxyXG59XHJcblxyXG4iLCIuZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzEpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjZjZhZjI3O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzIpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjYWI0N2JjO1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzMpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjYTVhNWE1O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzQpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjZGM0OTQ5O1xufVxuXG4uZHJpdmVyLWRldGFpbHM6bnRoLWNoaWxkKDVuKzUpIC5kcml2ZXItaW5pdGlhbHMge1xuICBiYWNrZ3JvdW5kOiAjMDA4OTdiO1xufVxuXG4uc3RpY2t5LWhlYWRlci13bWQge1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHJpZ2h0OiAwO1xuICBwYWRkaW5nOiAxNXB4IDIwcHg7XG4gIHRvcDogNDVweDtcbiAgaGVpZ2h0OiA2NXB4O1xuICBmb250LXNpemU6IDIwcHg7XG4gIHotaW5kZXg6IDEwO1xuICBiYWNrZ3JvdW5kOiAjZmZmO1xufVxuXG4ubG9jYXRpb25maWx0ZXIge1xuICB3aWR0aDogMTAwJTtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICByaWdodDogNHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG4gIGZvbnQtc2l6ZTogMTRweDtcbiAgei1pbmRleDogMTAxMDtcbn1cblxuLnN0aWNreV9oZWFkZXIge1xuICBwb3NpdGlvbjogLXdlYmtpdC1zdGlja3k7XG4gIHBvc2l0aW9uOiBzdGlja3k7XG4gIHRvcDogNDVweDtcbiAgcGFkZGluZzogNXB4O1xuICBmb250LXNpemU6IDIwcHg7XG4gIHotaW5kZXg6IDEwO1xuICBiYWNrZ3JvdW5kOiAjZmZmO1xuICBtYXJnaW4tYm90dG9tOiAwcHg7XG4gIG1hcmdpbi10b3A6IC0xMHB4O1xuICBib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLCAwLCAwLCAwLjEpO1xuICBib3JkZXItcmFkaXVzOiAycHg7XG59XG5cbi5kaXNwbGF5X2hpZGUge1xuICBkaXNwbGF5OiBub25lO1xuICB0cmFuc2l0aW9uOiBvcGFjaXR5IDFzIGVhc2Utb3V0O1xuICBvcGFjaXR5OiAwO1xufVxuXG4uZXhwYW5kX21hcF9idG4ge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogMXB4O1xuICByaWdodDogMTVweDtcbiAgYmFja2dyb3VuZDogI2ZmZjtcbiAgYm9yZGVyLXJhZGl1czogMnB4IDJweCAycHggMnB4O1xuICBwYWRkaW5nOiAzcHg7XG4gIGJveC1zaGFkb3c6IC0ycHggMnB4IDZweCAxcHggI2FhYTtcbiAgei1pbmRleDogMTtcbn1cblxuLmRyaXZlcl9idG4ge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogMTVweDtcbiAgbGVmdDogLTM1cHg7XG4gIGJhY2tncm91bmQ6IHdoaXRlO1xuICBib3JkZXItcmFkaXVzOiAycHg7XG4gIGJvcmRlci10b3AtbGVmdC1yYWRpdXM6IDVweDtcbiAgYm9yZGVyLWJvdHRvbS1sZWZ0LXJhZGl1czogNXB4O1xuICBwYWRkaW5nOiA1cHg7XG4gIGJveC1zaGFkb3c6IC00cHggMHB4IDRweCAwcHggI2FhYWFhYTtcbn1cblxuLmFic29sdXRlX2RyaXZlciB7XG4gIHBvc2l0aW9uOiBmaXhlZDtcbiAgd2lkdGg6IDI1JTtcbiAgdG9wOiAxMDBweDtcbiAgcmlnaHQ6IDA7XG4gIGJhY2tncm91bmQ6ICNmZmY7XG4gIHotaW5kZXg6IDExO1xuICBwYWRkaW5nOiAxMHB4O1xuICBib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLCAwLCAwLCAwLjEpO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuXG4uaGlkZV9hYnNvbHV0ZV9kcml2ZXIge1xuICB3aWR0aDogMDtcbiAgcmlnaHQ6IC0yMHB4O1xufVxuXG4uYWN0aXZlUm91dGUge1xuICBmb250LXdlaWdodDogNjAwO1xuICBjdXJzb3I6IHBvaW50ZXI7XG4gIGJhY2tncm91bmQ6ICNmNWY1ZjU7XG59XG5cbi5saXZlIHtcbiAgaGVpZ2h0OiAxMHB4O1xuICB3aWR0aDogMTBweDtcbiAgYm9yZGVyLXJhZGl1czogNTAlO1xuICBiYWNrZ3JvdW5kLWNvbG9yOiBncmVlbjtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IC0xcHg7XG4gIHJpZ2h0OiAxcHg7XG4gIHRyYW5zZm9ybTogc2NhbGUoMSk7XG4gIGFuaW1hdGlvbjogcHVsc2UgMXMgaW5maW5pdGU7XG59XG5cbi5pbmFjdGl2ZSB7XG4gIGhlaWdodDogMTBweDtcbiAgd2lkdGg6IDEwcHg7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgYmFja2dyb3VuZC1jb2xvcjogb3JhbmdlO1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogLTFweDtcbiAgcmlnaHQ6IDFweDtcbn1cblxuQGtleWZyYW1lcyBwdWxzZSB7XG4gIDAlIHtcbiAgICAtbW96LWJveC1zaGFkb3c6IDAgMCAwIDAgcmdiYSgyMDQsIDE2OSwgNDQsIDAuNCk7XG4gICAgYm94LXNoYWRvdzogMCAwIDAgMCByZ2JhKDIwNCwgMTY5LCA0NCwgMC40KTtcbiAgfVxuICA3MCUge1xuICAgIC1tb3otYm94LXNoYWRvdzogMCAwIDAgMTBweCByZ2JhKDIwNCwgMTY5LCA0NCwgMCk7XG4gICAgYm94LXNoYWRvdzogMCAwIDAgMTBweCByZ2JhKDIwNCwgMTY5LCA0NCwgMCk7XG4gIH1cbiAgMTAwJSB7XG4gICAgLW1vei1ib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LCAxNjksIDQ0LCAwKTtcbiAgICBib3gtc2hhZG93OiAwIDAgMCAwIHJnYmEoMjA0LCAxNjksIDQ0LCAwKTtcbiAgfVxufVxuLmNhcnJpZXItcG9wb3Zlci5wb3BvdmVyIHtcbiAgbWluLXdpZHRoOiAzMDBweDtcbiAgbWF4LXdpZHRoOiAzNTBweDtcbiAgYmFja2dyb3VuZDogI0Y5RjlGOTtcbiAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcbiAgYm94LXNpemluZzogYm9yZGVyLWJveDtcbiAgYm94LXNoYWRvdzogMTBweCAxMHB4IDhweCAtMnB4IHJnYmEoMCwgMCwgMCwgMC4xMyk7XG4gIGJvcmRlci1yYWRpdXM6IDEwcHg7XG59XG5cbi5jYXJyaWVyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcbiAgcGFkZGluZzogMTBweDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xufVxuXG4ubXVsdGlzZWxlY3QtZHJvcGRvd24gLnNlbGVjdGVkLWl0ZW0ge1xuICBtYXgtd2lkdGg6IDY1JTtcbiAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcbiAgb3ZlcmZsb3c6IGhpZGRlbjtcbiAgdGV4dC1vdmVyZmxvdzogZWxsaXBzaXM7XG59XG5cbi8qIGJ1eWVyLUxvYWRzIE1hc3Rlci1GaWx0ZXJzIHN0YXJ0cyBoZXJlKi9cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIge1xuICBtaW4td2lkdGg6IDQyNXB4O1xuICBtYXgtd2lkdGg6IDQ1MHB4O1xuICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xuICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xuICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiYSgwLCAwLCAwLCAwLjEzKTtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XG4gIHBhZGRpbmc6IDA7XG4gIGJvcmRlci1yYWRpdXM6IDVweDtcbiAgYmFja2dyb3VuZDogI2ZmZmZmZjtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyB7XG4gIHBhZGRpbmc6IDE1cHg7XG59XG4ubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLmZvbnQtYm9sZCB7XG4gIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcbn1cbi5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLmJvcmRlci1ib3R0b20tMiB7XG4gIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XG59XG5cbi5jaXJjbGUtYmFkZ2Uge1xuICBwb3NpdGlvbjogYWJzb2x1dGU7XG4gIHRvcDogLTExcHg7XG4gIGxlZnQ6IC0xNHB4O1xuICBiYWNrZ3JvdW5kOiAjZmE5MzkzO1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGZvbnQtc2l6ZTogMTJweDtcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xuICBjb2xvcjogd2hpdGU7XG4gIGRpc3BsYXk6IGlubGluZS1mbGV4O1xuICBhbGlnbi1pdGVtczogY2VudGVyO1xuICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcbiAgd2lkdGg6IDE4cHg7XG4gIGhlaWdodDogMThweDtcbn0iXX0= */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](WhereIsMyDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-where-is-my-driver',
          templateUrl: './where-is-my-driver.component.html',
          styleUrls: ['./where-is-my-driver.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_12__["FormBuilder"]
        }, {
          type: _services_buyerwallyboard_service__WEBPACK_IMPORTED_MODULE_13__["BuyerwallyboardService"]
        }, {
          type: src_app_shared_components_sendbird_services_sendbird_service__WEBPACK_IMPORTED_MODULE_14__["chatService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_15__["CarrierService"]
        }];
      }, {
        singleMulti: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        loadsMapView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_wally_dashboard_map_view_component__WEBPACK_IMPORTED_MODULE_9__["WhereIsMyDriverMapViewComponent"]]
        }],
        loadsGridView: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_wally_dashboard_grid_view_component__WEBPACK_IMPORTED_MODULE_10__["WhereIsMyDriverGridViewComponent"]]
        }],
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        selectedDriverLoad: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['SelectedDriverLoad', {
            read: angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"],
            "static": false
          }]
        }],
        sendbirdComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_sendbird_sendbird_component__WEBPACK_IMPORTED_MODULE_4__["SendbirdComponent"]]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=buyer-wally-board-buyer-wally-board-module-es5.js.map