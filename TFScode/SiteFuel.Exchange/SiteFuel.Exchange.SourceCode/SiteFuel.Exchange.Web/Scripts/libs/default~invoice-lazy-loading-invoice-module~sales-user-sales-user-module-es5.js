function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~invoice-lazy-loading-invoice-module~sales-user-sales-user-module"], {
  /***/
  "./src/app/fees/fee-list/fee-list.component.ts": function srcAppFeesFeeListFeeListComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeeListComponent", function () {
      return FeeListComponent;
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


    var _model__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../model */
    "./src/app/fees/model.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../invoice/models/DropDetail */
    "./src/app/invoice/models/DropDetail.ts");
    /* harmony import */


    var _app_enum__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _service_fee_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../service/fee.service */
    "./src/app/fees/service/fee.service.ts");
    /* harmony import */


    var _invoice_services_invoice_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../invoice/services/invoice.service */
    "./src/app/invoice/services/invoice.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _fee_type_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./fee-type.component */
    "./src/app/fees/fee-list/fee-type.component.ts");

    function FeeListComponent_h4_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h4");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Freight Cost / Fees");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeListComponent_div_4_div_4_Template(rf, ctx) {
      if (rf & 1) {
        var _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "h5");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Table Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function FeeListComponent_div_4_div_4_Template_ng_multiselect_dropdown_onSelect_4_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r10);

          var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r9.OnAccessorialFeeTableTypeSelect($event);
        })("onDeSelect", function FeeListComponent_div_4_div_4_Template_ng_multiselect_dropdown_onDeSelect_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r10);

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r11.OnAccessorialFeeTableTypeDeSelect();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Table Type")("settings", ctx_r6.SingleSelectSettingsById)("data", ctx_r6.AccessorialFeeTableTypeList);
      }
    }

    function FeeListComponent_div_4_div_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-multiselect-dropdown", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function FeeListComponent_div_4_div_8_Template_ng_multiselect_dropdown_onSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r12.OnAccessorialFeeIdSelect();
        })("onDeSelect", function FeeListComponent_div_4_div_8_Template_ng_multiselect_dropdown_onDeSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r14.OnAccessorialFeeTableTypeDeSelectForSingle();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Type")("settings", ctx_r7.SingleSelectSettingsById)("data", ctx_r7.AccessorialFeeIdList);
      }
    }

    function FeeListComponent_div_4_div_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-multiselect-dropdown", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function FeeListComponent_div_4_div_9_Template_ng_multiselect_dropdown_onSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r15.OnAccessorialFeeIdSelect();
        })("onDeSelect", function FeeListComponent_div_4_div_9_Template_ng_multiselect_dropdown_onDeSelect_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r17.OnAccessorialFeeTableTypeDeSelect();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Type")("settings", ctx_r8.MultiSelectSettingsById)("data", ctx_r8.AccessorialFeeIdList);
      }
    }

    function FeeListComponent_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "h3");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Accessorial Fees");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, FeeListComponent_div_4_div_4_Template, 5, 3, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "h5");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Accessorial Fees Table Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, FeeListComponent_div_4_div_8_Template, 2, 3, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, FeeListComponent_div_4_div_9_Template, 2, 3, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.Parent.get("SelectedOrders").value.length == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.Parent.get("SelectedOrders").value.length == 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.Parent.get("SelectedOrders").value.length >= 1);
      }
    }

    function FeeListComponent_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_9_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var commonFee_r18 = ctx.$implicit;

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r21.removeGeneralFee(true, commonFee_r18);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var commonFee_r18 = ctx.$implicit;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r2.Parent)("FeeGroup", commonFee_r18)("FeeTypes", ctx_r2.FeeTypes)("Currency", ctx_r2.DisplayCurrency)("FeeConstraintTypes", ctx_r2.FeeConstraintTypes)("FeeSubTypes", ctx_r2.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_16_Template(rf, ctx) {
      if (rf & 1) {
        var _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_16_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27);

          var otherFee_r23 = ctx.$implicit;

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r26.removeGeneralFee(false, otherFee_r23);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var otherFee_r23 = ctx.$implicit;

        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r3.Parent)("FeeGroup", otherFee_r23)("FeeTypes", ctx_r3.FeeTypes)("Currency", ctx_r3.DisplayCurrency)("FeeConstraintTypes", ctx_r3.FeeConstraintTypes)("FeeSubTypes", ctx_r3.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_26_Template(rf, ctx) {
      if (rf & 1) {
        var _r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_26_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r32);

          var spCommonFee_r28 = ctx.$implicit;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r31.removeSpecialFee(true, spCommonFee_r28);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var spCommonFee_r28 = ctx.$implicit;

        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r4.Parent)("FeeGroup", spCommonFee_r28)("FeeTypes", ctx_r4.FeeTypes)("Currency", ctx_r4.DisplayCurrency)("FeeConstraintTypes", ctx_r4.FeeConstraintTypes)("FeeSubTypes", ctx_r4.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_33_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_33_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r37);

          var spOtherFee_r33 = ctx.$implicit;

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r36.removeSpecialFee(false, spOtherFee_r33);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var spOtherFee_r33 = ctx.$implicit;

        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r5.Parent)("FeeGroup", spOtherFee_r33)("FeeTypes", ctx_r5.FeeTypes)("Currency", ctx_r5.DisplayCurrency)("FeeConstraintTypes", ctx_r5.FeeConstraintTypes)("FeeSubTypes", ctx_r5.FeeSubTypes);
      }
    }

    var FeeListComponent = /*#__PURE__*/function () {
      function FeeListComponent(fb, feeService, invoiceService, route) {
        _classCallCheck(this, FeeListComponent);

        this.fb = fb;
        this.feeService = feeService;
        this.invoiceService = invoiceService;
        this.route = route;
        this.isSales = false;
        this.SingleSelectSettingsById = {};
        this.MultiSelectSettingsById = {};
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];
      }

      _createClass(FeeListComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this = this;

          this.AccessorialForm = this.buildForm();
          this.Parent.addControl('AccessorialFeeDetails', this.AccessorialForm);

          if (this.InputAccessorialFeeDetails != undefined && this.InputAccessorialFeeDetails != null) {
            this.AccessorialForm.patchValue(this.InputAccessorialFeeDetails);
          }

          this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
          this.Parent.addControl('Fees', this.fb.array([]));
          this.feeService.getFeeTypes(this.OrderId, this.Currency, this.isSales).subscribe(function (data) {
            _this.FeeTypes = data;
          });
          this.feeService.getFeeConstraintTypes().subscribe(function (data) {
            _this.FeeConstraintTypes = data;
          });
          this.feeService.getFeeSubTypes(this.OrderId, this.Currency, this.isSales).subscribe(function (data) {
            _this.FeeSubTypes = data.filter(function (elem) {
              return elem.FeeSubTypeId != 1;
            });
          });
          this.SingleSelectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1
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
          this.getTableTypes();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          var _this2 = this;

          if (this.NoOrders) {
            this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([]);
            this.AccessorialForm.controls['AccessorialFeeId'].patchValue([]);
          }

          if (change.Fees && change.Fees.currentValue) {
            this.CommonFees = [];
            this.OtherFees = [];
            this.SpCommonFees = [];
            this.SpOtherFees = [];
            var fees = this.Parent.get('Fees');

            if (fees) {
              fees.clear();
            }

            this.currValues = change.Fees.currentValue;
            this.currValues.forEach(function (x) {
              if (x.FeeConstraintTypeId == null) {
                _this2.addGeneralFee(x.CommonFee, x);
              } else {
                _this2.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
              }
            });
          }

          if (change.InputAccessorialFeeDetails && change.InputAccessorialFeeDetails.currentValue) {
            this.getDefaultDetail(change.InputAccessorialFeeDetails.currentValue);
          }

          if (change.Currency && change.Currency.currentValue) {
            var currency = change.Currency.currentValue;
            this.DisplayCurrency = currency;
            this.feeService.getFeeTypes(0, this.DisplayCurrency, this.isSales).subscribe(function (data) {
              _this2.FeeTypes = data;
            });
            this.feeService.getFeeConstraintTypes().subscribe(function (data) {
              _this2.FeeConstraintTypes = data;
            });
            this.feeService.getFeeSubTypes(0, this.DisplayCurrency, this.isSales).subscribe(function (data) {
              _this2.FeeSubTypes = data.filter(function (elem) {
                return elem.FeeSubTypeId != 1;
              });
            });
          }
        }
      }, {
        key: "buildForm",
        value: function buildForm() {
          return this.fb.group({
            AccessorialFeeTableType: this.fb.control(''),
            AccessorialFeeId: this.fb.control([])
          });
        }
      }, {
        key: "getForm",
        value: function getForm(model) {
          var byQtyModel = model.DeliveryFeeByQuantity;
          var byQuantity = [];
          var _fb = this.fb;

          if (byQtyModel != undefined && byQtyModel != null) {
            byQtyModel.forEach(function (elem, idx) {
              byQuantity.push(_fb.group({
                Currency: _fb.control(elem.Currency),
                MinQuantity: _fb.control(elem.MinQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
                MaxQuantity: _fb.control(elem.MaxQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
                Fee: _fb.control(elem.Fee, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required])
              }));
            });
          }

          var _specialDate = '';

          if (model.SpecialDate != null && model.SpecialDate != undefined) {
            _specialDate = moment__WEBPACK_IMPORTED_MODULE_3__(model.SpecialDate).format('MM/DD/YYYY');
            _specialDate = _specialDate == '01/01/0001' ? '' : _specialDate;
          }

          this.group = this.fb.group({
            OrderId: this.fb.control(model.OrderId),
            Currency: this.fb.control(this.DisplayCurrency),
            TruckLoadType: this.fb.control(model.TruckLoadType),
            TruckLoadCategoryId: this.fb.control(model.TruckLoadCategoryId),
            IncludeInPPG: this.fb.control(model.IncludeInPPG),
            CommonFee: this.fb.control(model.CommonFee),
            FeeConstraintTypeId: this.fb.control(model.FeeConstraintTypeId),
            SpecialDate: this.fb.control(_specialDate),
            FeeTypeId: this.fb.control(model.FeeTypeId, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            FeeSubTypeId: this.fb.control(model.FeeSubTypeId, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Fee: this.fb.control(model.Fee, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            OtherFeeDescription: this.fb.control(model.OtherFeeDescription),
            MinimumGallons: this.fb.control(model.MinimumGallons),
            DeliveryFeeByQuantity: this.fb.array(byQuantity)
          });
          return this.group;
        }
      }, {
        key: "addGeneralFee",
        value: function addGeneralFee(_commonFee, feeObj) {
          if (feeObj == null) {
            feeObj = new _model__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
            feeObj.CommonFee = _commonFee;
          }

          if (!_commonFee && (feeObj.FeeTypeId == undefined || feeObj.FeeTypeId == null || feeObj.FeeTypeId.indexOf('14') < 0)) {
            feeObj.FeeTypeId = '14';
          }

          var feeGroup = this.getForm(feeObj);

          if (_commonFee) {
            this.CommonFees.push(feeGroup);
          } else {
            this.OtherFees.push(feeGroup);
          }

          this.Parent.get('Fees').push(feeGroup);
        }
      }, {
        key: "removeGeneralFee",
        value: function removeGeneralFee(_commonFee, fee) {
          var _fees = this.Parent.get('Fees');

          _fees.removeAt(_fees.controls.indexOf(fee));

          if (_commonFee) {
            this.CommonFees.splice(this.CommonFees.indexOf(fee), 1);
          } else {
            this.OtherFees.splice(this.OtherFees.indexOf(fee), 1);
          }
        }
      }, {
        key: "addSpecialFee",
        value: function addSpecialFee(_commonFee, typeId, feeObj) {
          if (feeObj == null) {
            feeObj = new _model__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
            feeObj.CommonFee = _commonFee;
          }

          if (!_commonFee && (feeObj.FeeTypeId == undefined || feeObj.FeeTypeId == null || feeObj.FeeTypeId.indexOf('14') < 0)) {
            feeObj.FeeTypeId = '14';
          }

          feeObj.FeeConstraintTypeId = typeId;
          var feeGroup = this.getForm(feeObj);

          if (_commonFee) {
            this.SpCommonFees.push(feeGroup);
          } else {
            this.SpOtherFees.push(feeGroup);
          }

          this.Parent.get('Fees').push(feeGroup);
        }
      }, {
        key: "removeSpecialFee",
        value: function removeSpecialFee(_commonFee, fee) {
          var _fees = this.Parent.get('Fees');

          _fees.removeAt(_fees.controls.indexOf(fee));

          if (_commonFee) {
            this.SpCommonFees.splice(this.SpCommonFees.indexOf(fee), 1);
          } else {
            this.SpOtherFees.splice(this.SpOtherFees.indexOf(fee), 1);
          }
        }
      }, {
        key: "addByQtyFee",
        value: function addByQtyFee(fee) {
          var _fees = fee.get('DeliveryFeeByQuantity');

          var lastFee = _fees.controls[_fees.controls.length - 1].get('Fee').value;

          var feeObj = new _model__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
          feeObj.Fee = lastFee;

          _fees.push(this.getForm(feeObj));
        }
      }, {
        key: "removeByQtyFee",
        value: function removeByQtyFee(fee, index) {
          var _fees = fee.get('DeliveryFeeByQuantity');

          _fees.removeAt(index);
        } //--------------------------------------------------------------

      }, {
        key: "isInvalid",
        value: function isInvalid(drop, name) {
          return drop.get(name).invalid && (drop.get(name).dirty || drop.get(name).touched);
        }
      }, {
        key: "isRequired",
        value: function isRequired(drop, name) {
          return drop.get(name).errors.required;
        }
      }, {
        key: "isMin",
        value: function isMin(drop, name) {
          return drop.get(name).errors.min;
        } // Accessorial

      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          var _this3 = this;

          this.invoiceService.getTableTypes().subscribe(function (data) {
            _this3.AccessorialFeeTableTypeList = data;
            _this3.AccessorialFeeTableTypeList = _this3.AccessorialFeeTableTypeList.filter(function (x) {
              return x.Id != _app_enum__WEBPACK_IMPORTED_MODULE_5__["TableType"].Carrier;
            });
          });
        }
      }, {
        key: "OnAccessorialFeeTableTypeSelect",
        value: function OnAccessorialFeeTableTypeSelect(item) {
          this.AccessorialForm.get('AccessorialFeeId').patchValue([]);
          this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([{
            Id: item.Id,
            Name: item.Id == _app_enum__WEBPACK_IMPORTED_MODULE_5__["TableType"].Master ? "Master" : "CustomerSpecific"
          }]);
          this.GetAccessorialFeeId(null);
        }
      }, {
        key: "GetAccessorialFeeId",
        value: function GetAccessorialFeeId(selectedAccessorialId) {
          var _this4 = this;

          var input = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_4__["FreightTableNameModel"]();
          var selectedAccessorialFeeTableType = this.AccessorialForm.get('AccessorialFeeTableType').value;

          if (selectedAccessorialFeeTableType && selectedAccessorialFeeTableType.length > 0) {
            var AccessorialFeeTableTypeIds = selectedAccessorialFeeTableType.map(function (s) {
              return s.Id;
            });
            input.TableType = AccessorialFeeTableTypeIds.join(',');
            input.OrderId = this.OrderId;
            this.invoiceService.getAccessorialTableName(input).subscribe(function (data) {
              if (data && data.length > 0) {
                _this4.AccessorialFeeIdList = data;

                if (selectedAccessorialId) {
                  _this4.AccessorialForm.controls['AccessorialFeeId'].patchValue(_this4.AccessorialFeeIdList.filter(function (t) {
                    return t.Id == selectedAccessorialId;
                  }));
                }
              }
            });
          }
        }
      }, {
        key: "getDefaultDetail",
        value: function getDefaultDetail(AccessorialFees) {
          var _this5 = this;

          if (AccessorialFees && AccessorialFees.length > 0) {
            if (AccessorialFees.length == 1) {
              var type = AccessorialFees[0].AccessorialFeeTableType;
              this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([{
                Id: type,
                Name: type == _app_enum__WEBPACK_IMPORTED_MODULE_5__["TableType"].Master ? "Master" : "CustomerSpecific"
              }]);
              this.GetAccessorialFeeId(AccessorialFees[0].AccessorialFeeId);
            } else {
              this.invoiceService.GetAccessorialFeeTablesForConsolidated(null).subscribe(function (allTableNames) {
                _this5.AccessorialFeeIdList = allTableNames;

                var AccessorialFeeIds = _this5.AccessorialFeeIdList.filter(_this5.IdInComparer(AccessorialFees));

                _this5.AccessorialForm.controls['AccessorialFeeId'].setValue(AccessorialFeeIds);
              });
            }
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
        }
      }, {
        key: "AllFeeRemove",
        value: function AllFeeRemove() {
          var _this6 = this;

          if (this.CommonFees && this.CommonFees.length > 0) {
            this.CommonFees.forEach(function (generalFees) {
              _this6.removeGeneralFee(true, generalFees);
            });
            this.CommonFees = [];
          }

          if (this.OtherFees && this.OtherFees.length > 0) {
            this.OtherFees.forEach(function (otherFees) {
              _this6.removeGeneralFee(false, otherFees);
            });
            this.OtherFees = [];
          }

          if (this.SpCommonFees && this.SpCommonFees.length > 0) {
            this.SpCommonFees.forEach(function (spFee) {
              _this6.removeSpecialFee(true, spFee);
            });
            this.SpCommonFees = [];
          }

          if (this.SpOtherFees && this.SpOtherFees.length > 0) {
            this.SpOtherFees.forEach(function (spOtherFee) {
              _this6.removeSpecialFee(false, spOtherFee);
            });
            this.SpOtherFees = [];
          }
        }
      }, {
        key: "OnAccessorialFeeTableTypeDeSelect",
        value: function OnAccessorialFeeTableTypeDeSelect() {
          var _this7 = this;

          var selectedAccessorialFeeTableType = this.AccessorialForm.get('AccessorialFeeTableType').value;

          if (selectedAccessorialFeeTableType.length == 0) {
            this.AccessorialForm.get('AccessorialFeeId').patchValue([]);
            this.AccessorialFeeIdList = [];
          }

          this.AllFeeRemove();
          var selectedItems = this.AccessorialForm.get("AccessorialFeeId").value;

          if (selectedItems != null && selectedItems != undefined) {
            var feeIds = selectedItems.map(function (s) {
              return s.Id;
            });
            this.invoiceService.GetAccessorialFeeByAccessorialFeeId(feeIds.join(',')).subscribe(function (data) {
              if (data && data.length > 0) {
                data.forEach(function (obj) {
                  if (obj.FeeConstraintTypeId == null) {
                    _this7.addGeneralFee(obj.CommonFee, obj);
                  } else {
                    _this7.addSpecialFee(obj.CommonFee, obj.FeeConstraintTypeId, obj);
                  }
                });
              }
            });
          }
        }
      }, {
        key: "OnAccessorialFeeTableTypeDeSelectForSingle",
        value: function OnAccessorialFeeTableTypeDeSelectForSingle() {
          this.AllFeeRemove();
        }
      }, {
        key: "OnAccessorialFeeIdSelect",
        value: function OnAccessorialFeeIdSelect() {
          var _this8 = this;

          this.CommonFees = [];
          this.OtherFees = [];
          this.SpCommonFees = [];
          this.SpOtherFees = [];
          var fees = this.Parent.get('Fees');

          if (fees) {
            fees.clear();
          }

          var selectedItems = this.AccessorialForm.get("AccessorialFeeId").value;

          if (selectedItems != null && selectedItems != undefined) {
            var feeIds = selectedItems.map(function (s) {
              return s.Id;
            });

            if (feeIds.length != 0) {
              this.invoiceService.GetAccessorialFeeByAccessorialFeeId(feeIds.join(',')).subscribe(function (data) {
                if (feeIds.length != 1) {
                  data = Object(_invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_4__["DeDuplicateFees"])(_this8.currValues, data);
                }

                if (data && data.length > 0) {
                  data.forEach(function (obj) {
                    if (obj.FeeConstraintTypeId == null) {
                      _this8.addGeneralFee(obj.CommonFee, obj);
                    } else {
                      _this8.addSpecialFee(obj.CommonFee, obj.FeeConstraintTypeId, obj);
                    }
                  });
                }
              });
            }
          } else {
            this.OnAccessorialFeeTableTypeDeSelect();
          }
        }
      }]);

      return FeeListComponent;
    }();

    FeeListComponent.ɵfac = function FeeListComponent_Factory(t) {
      return new (t || FeeListComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_fee_service__WEBPACK_IMPORTED_MODULE_6__["FeeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_invoice_services_invoice_service__WEBPACK_IMPORTED_MODULE_7__["InvoiceService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_8__["ActivatedRoute"]));
    };

    FeeListComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: FeeListComponent,
      selectors: [["app-fee-list"]],
      inputs: {
        Parent: "Parent",
        Fees: "Fees",
        InputAccessorialFeeDetails: "InputAccessorialFeeDetails",
        isWaitingForBol: "isWaitingForBol",
        isSales: "isSales",
        IsFrieghtPricingMethodAuto: "IsFrieghtPricingMethodAuto",
        Currency: "Currency",
        NoOrders: "NoOrders"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 37,
      vars: 8,
      consts: [[3, "formGroup"], [1, "well", "box-shadow"], [4, "ngIf"], ["formArrayName", "Fees"], [1, "mt10", "mb5"], [4, "ngFor", "ngForOf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "mt10"], [1, "row"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-3"], ["class", "form-group", 4, "ngIf"], [1, "form-group"], ["formControlName", "AccessorialFeeTableType", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["formControlName", "AccessorialFeeId", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["formControlName", "AccessorialFeeId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [1, "col-sm-10"], [3, "Parent", "FeeGroup", "FeeTypes", "Currency", "FeeConstraintTypes", "FeeSubTypes"], [1, "col-sm-2"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", "mt10", 3, "click"]],
      template: function FeeListComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, FeeListComponent_h4_3_Template, 2, 0, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, FeeListComponent_div_4_Template, 10, 3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "General");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, FeeListComponent_ng_container_9_Template, 6, 6, "ng-container", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "button", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_10_listener() {
            return ctx.addGeneralFee(true, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Other");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, FeeListComponent_ng_container_16_Template, 6, 6, "ng-container", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "button", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_17_listener() {
            return ctx.addGeneralFee(false, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](18, "i", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Weekend / Holiday Fee(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "General ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, FeeListComponent_ng_container_26_Template, 6, 6, "ng-container", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "button", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_27_listener() {
            return ctx.addSpecialFee(true, 1, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](28, "i", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Other");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](33, FeeListComponent_ng_container_33_Template, 6, 6, "ng-container", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "button", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_34_listener() {
            return ctx.addSpecialFee(false, 1, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](35, "i", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.Parent);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.AccessorialForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsFrieghtPricingMethodAuto);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsFrieghtPricingMethodAuto);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CommonFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.OtherFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.SpCommonFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.SpOtherFees);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _fee_type_component__WEBPACK_IMPORTED_MODULE_11__["FeeTypeComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2ZlZXMvZmVlLWxpc3QvZmVlLWxpc3QuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeListComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-fee-list',
          templateUrl: './fee-list.component.html',
          styleUrls: ['./fee-list.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _service_fee_service__WEBPACK_IMPORTED_MODULE_6__["FeeService"]
        }, {
          type: _invoice_services_invoice_service__WEBPACK_IMPORTED_MODULE_7__["InvoiceService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_8__["ActivatedRoute"]
        }];
      }, {
        Parent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        Fees: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        InputAccessorialFeeDetails: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        isWaitingForBol: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        isSales: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        IsFrieghtPricingMethodAuto: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        Currency: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        NoOrders: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();

    function requiredIfValidator(predicate) {
      return function (formControl) {
        if (!formControl.parent) {
          return null;
        }

        if (predicate()) {
          return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(formControl);
        }

        return null;
      };
    }
    /***/

  },

  /***/
  "./src/app/fees/fee-list/fee-type.component.ts": function srcAppFeesFeeListFeeTypeComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeeTypeComponent", function () {
      return FeeTypeComponent;
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


    var _model__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../model */
    "./src/app/fees/model.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function FeeTypeComponent_option_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Select Fee");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);
      }
    }

    function FeeTypeComponent_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ft_r14 = ctx.$implicit;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngValue", ft_r14.Code)("selected", ft_r14.Code == ctx_r2.FeeGroup.get("FeeTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ft_r14.Name);
      }
    }

    function FeeTypeComponent_span_14_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_span_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_14_span_1_Template, 2, 0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.isRequired(ctx_r3.FeeGroup, "FeeTypeId") || ctx_r3.isFeeNameRequired(ctx_r3.FeeGroup, "OtherFeeDescription"));
      }
    }

    function FeeTypeComponent_option_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Select Fee Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);
      }
    }

    function FeeTypeComponent_option_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var fst_r16 = ctx.$implicit;

        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", fst_r16.FeeSubTypeId)("selected", fst_r16.FeeSubTypeId == ctx_r5.FeeGroup.get("FeeSubTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", fst_r16.SubTypeName, " ");
      }
    }

    function FeeTypeComponent_span_20_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_span_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_20_span_1_Template, 2, 0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.isRequired(ctx_r6.FeeGroup, "FeeSubTypeId"));
      }
    }

    function FeeTypeComponent_div_21_select_2_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var fc_r20 = ctx.$implicit;

        var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", fc_r20.Id)("selected", fc_r20.Id == ctx_r19.FeeGroup.get("FeeConstraintTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", fc_r20.Name, " ");
      }
    }

    function FeeTypeComponent_div_21_select_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "select", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_21_select_2_option_1_Template, 2, 3, "option", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r18.FeeConstraintTypes);
      }
    }

    function FeeTypeComponent_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, FeeTypeComponent_div_21_select_2_Template, 2, 1, "select", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r7.FeeGroup.get("FeeConstraintTypeId").value);
      }
    }

    function FeeTypeComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "input", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function FeeTypeComponent_div_22_Template_input_onDateChange_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r21.FeeGroup.get("SpecialDate").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY");
      }
    }

    function FeeTypeComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "input", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_input_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "input", 33);
      }
    }

    function FeeTypeComponent_div_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r11.DisplayCurrency);
      }
    }

    function FeeTypeComponent_span_29_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_span_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_29_span_1_Template, 2, 0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r12.isRequired(ctx_r12.FeeGroup, "Fee"));
      }
    }

    function FeeTypeComponent_div_30_div_2_span_7_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_div_30_div_2_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_30_div_2_span_7_span_1_Template, 2, 0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var byQty_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r27.isRequired(byQty_r25, "MaxQuantity"));
      }
    }

    function FeeTypeComponent_div_30_div_2_span_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function FeeTypeComponent_div_30_div_2_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_30_div_2_span_10_span_1_Template, 2, 0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var byQty_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r28.isRequired(byQty_r25, "Fee"));
      }
    }

    function FeeTypeComponent_div_30_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, FeeTypeComponent_div_30_div_2_span_7_Template, 2, 1, "span", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, FeeTypeComponent_div_30_div_2_span_10_Template, 2, 1, "span", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeTypeComponent_div_30_div_2_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34);

          var i_r26 = ctx.index;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r33.removeByQtyFee(ctx_r33.FeeGroup, i_r26);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "i", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var byQty_r25 = ctx.$implicit;
        var i_r26 = ctx.index;

        var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r24.isInvalid(byQty_r25, "MaxQuantity"));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r24.isInvalid(byQty_r25, "Fee"));
      }
    }

    function FeeTypeComponent_div_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, FeeTypeComponent_div_30_div_2_Template, 14, 3, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeTypeComponent_div_30_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r35.addByQtyFee(ctx_r35.FeeGroup, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "i", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " Add Quantity Range");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r13.FeeGroup.get("DeliveryFeeByQuantity")["controls"]);
      }
    }

    var FeeTypeComponent = /*#__PURE__*/function () {
      function FeeTypeComponent(fb) {
        _classCallCheck(this, FeeTypeComponent);

        this.fb = fb;
        this.DisplayFeeTypes = [];
      }

      _createClass(FeeTypeComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.FeeGroup.setValidators(this.feeNameRequired('FeeTypeId', 'OtherFeeDescription', 'CommonFee'));
          if (this.FeeSubTypes != null && this.FeeSubTypes != undefined) this.DisplayFeeTypes = this.FeeSubTypes.slice();
          this.DisplayCurrency = this.Currency;
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.FeeSubTypes && change.FeeSubTypes.currentValue != null) {
            // var subTypes = change.FeeSubTypes.currentValue as FeeSubType[];
            // this.DisplayFeeTypes = subTypes;
            this.updateSubType(this.FeeGroup.get('FeeTypeId').value);
          }

          if (change.Currency && change.Currency.currentValue) {
            var currency = change.Currency.currentValue;
            this.DisplayCurrency = currency;
          }
        }
      }, {
        key: "updateSubType",
        value: function updateSubType(feeTypeId) {
          var feeSubTypes = this.FeeSubTypes.slice().filter(function (elem) {
            return elem.FeeTypeId == feeTypeId;
          });

          if (feeSubTypes.length == 0) {
            var otherFeeSubTypes = [17, 5, 2];
            this.DisplayFeeTypes = this.FeeSubTypes.slice().filter(function (elem) {
              return otherFeeSubTypes.includes(elem.FeeSubTypeId) && elem.FeeTypeId == "14";
            });
          } else {
            this.DisplayFeeTypes = feeSubTypes;
          }
        }
      }, {
        key: "getForm",
        value: function getForm(_fee) {
          return this.fb.group({
            Currency: this.fb.control(_fee.Currency),
            MinQuantity: this.fb.control(_fee.MinQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]),
            MaxQuantity: this.fb.control(_fee.MaxQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/), _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Fee: this.fb.control(_fee.Fee, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)])
          });
        }
      }, {
        key: "addByQtyFee",
        value: function addByQtyFee(fee, feeObj) {
          if (feeObj == null) {
            feeObj = new _model__WEBPACK_IMPORTED_MODULE_2__["ByQuantityModel"]();
          }

          var _fees = fee.get('DeliveryFeeByQuantity');

          if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');

            lastMax.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]);
            feeObj.MinQuantity = lastMax.value;
          }

          var _form = this.getForm(feeObj);

          _fees.push(_form);
        }
      }, {
        key: "removeByQtyFee",
        value: function removeByQtyFee(fee, index) {
          var _fees = fee.get('DeliveryFeeByQuantity');

          _fees.removeAt(index);

          if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');

            lastMax.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/), _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
          }
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(group, name) {
          return group.get(name).invalid && (group.get(name).dirty || group.get(name).touched || group.get(name).invalid);
        }
      }, {
        key: "isRequired",
        value: function isRequired(group, name) {
          return group.get(name).errors.required;
        }
      }, {
        key: "isFeeNameRequired",
        value: function isFeeNameRequired(group, name) {
          return group.get(name).errors.required;
        }
      }, {
        key: "handleByQuantity",
        value: function handleByQuantity(group, subTypeId) {
          var fee = group.get('Fee');

          if (subTypeId == 3) {
            fee.setValue(0);
          } else {
            if (fee.value == 0) {
              fee.setValue('');
            }

            group.get('DeliveryFeeByQuantity').clear();
          }
        }
      }, {
        key: "feeNameRequired",
        value: function feeNameRequired(field1Name, field2Name, field3Name) {
          var field1 = this.FeeGroup.controls[field1Name];
          var field2 = this.FeeGroup.controls[field2Name];
          var field3 = this.FeeGroup.controls[field3Name];

          if (field3.value && (field1.value == null || field1.value == '')) {
            return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(field1);
          } else if (!field3.value && (field2.value == null || field2.value.replace(/\s/g, '') == '')) {
            return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(field2);
          } else {
            return null;
          }
        }
      }]);

      return FeeTypeComponent;
    }();

    FeeTypeComponent.ɵfac = function FeeTypeComponent_Factory(t) {
      return new (t || FeeTypeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]));
    };

    FeeTypeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: FeeTypeComponent,
      selectors: [["app-fee-type"]],
      inputs: {
        Parent: "Parent",
        FeeGroup: "FeeGroup",
        Currency: "Currency",
        FeeTypes: "FeeTypes",
        FeeSubTypes: "FeeSubTypes",
        FeeConstraintTypes: "FeeConstraintTypes"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 31,
      vars: 18,
      consts: [[1, "row", 3, "formGroup"], [1, "col-sm-3"], ["type", "hidden", "formControlName", "OrderId"], ["type", "hidden", "formControlName", "CommonFee"], ["type", "hidden", "formControlName", "TruckLoadType"], ["type", "hidden", "formControlName", "TruckLoadCategoryId"], ["type", "hidden", "formControlName", "IncludeInPPG"], ["formControlName", "FeeTypeId", 1, "form-control", 3, "change"], ["feeTypeId", ""], [3, "value", 4, "ngIf"], [3, "ngValue", "selected", 4, "ngFor", "ngForOf"], [1, "mb15", "form-group"], ["type", "text", "formControlName", "OtherFeeDescription", "placeholder", "Fee Name", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "form-group"], ["formControlName", "FeeSubTypeId", 1, "form-control", 3, "focus", "change"], [3, "value", "selected", 4, "ngFor", "ngForOf"], ["class", "color-maroon pa", 4, "ngIf"], ["class", "col-sm-3", 4, "ngIf"], [1, "input-group"], ["type", "text", "formControlName", "Fee", "class", "form-control", "placeholder", "Fee", 4, "ngIf"], ["class", "input-group-addon fs12", 4, "ngIf"], ["class", "col-sm-9", 4, "ngIf"], [3, "value"], [3, "ngValue", "selected"], [1, "color-maroon"], [4, "ngIf"], [3, "value", "selected"], [1, "color-maroon", "pa"], ["formControlName", "FeeConstraintTypeId", "class", "form-control", 4, "ngIf"], ["formControlName", "FeeConstraintTypeId", 1, "form-control"], ["type", "text", "formControlName", "SpecialDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "formControlName", "MinimumGallons", "placeholder", "Min Quantity", 1, "form-control"], ["type", "text", "formControlName", "Fee", "placeholder", "Fee", 1, "form-control"], [1, "input-group-addon", "fs12"], [1, "col-sm-9"], ["formArrayName", "DeliveryFeeByQuantity"], [4, "ngFor", "ngForOf"], [1, "row", "mb10"], [1, "col-sm-12"], [3, "click"], [1, "fa", "fa-plus-circle"], [1, "row", 3, "formGroupName"], [1, "col-sm-3", "pr0", "mb5"], ["type", "hidden", "formControlName", "Currency"], ["type", "text", "formControlName", "MinQuantity", "readonly", "readonly", "placeholder", "Min Quantity", 1, "form-control"], ["type", "text", "formControlName", "MaxQuantity", "placeholder", "Max Quantity", 1, "form-control"], [1, "col-sm-1"], [1, "fa", "fa-trash-alt", "color-maroon", "mt10"]],
      template: function FeeTypeComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "input", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "select", 7, 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function FeeTypeComponent_Template_select_change_8_listener() {
            return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, FeeTypeComponent_option_10_Template, 2, 1, "option", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, FeeTypeComponent_option_11_Template, 2, 3, "option", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, FeeTypeComponent_span_14_Template, 2, 1, "span", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "select", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("focus", function FeeTypeComponent_Template_select_focus_17_listener() {
            return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value);
          })("change", function FeeTypeComponent_Template_select_change_17_listener() {
            return ctx.handleByQuantity(ctx.FeeGroup, ctx.FeeGroup.get("FeeSubTypeId").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, FeeTypeComponent_option_18_Template, 2, 1, "option", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, FeeTypeComponent_option_19_Template, 2, 3, "option", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, FeeTypeComponent_span_20_Template, 2, 1, "span", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, FeeTypeComponent_div_21_Template, 3, 1, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, FeeTypeComponent_div_22_Template, 3, 1, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, FeeTypeComponent_div_23_Template, 2, 0, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, FeeTypeComponent_input_27_Template, 1, 0, "input", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, FeeTypeComponent_div_28_Template, 2, 1, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](29, FeeTypeComponent_span_29_Template, 2, 1, "span", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, FeeTypeComponent_div_30_Template, 8, 1, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.FeeGroup);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstyleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "block" : "none");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.FeeGroup.get("FeeTypeId").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.FeeTypes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstyleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "none" : "block");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "FeeTypeId") || ctx.isInvalid(ctx.FeeGroup, "OtherFeeDescription"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.FeeGroup.get("FeeSubTypeId").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.DisplayFeeTypes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "FeeSubTypeId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeConstraintTypeId").value != null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeConstraintTypeId").value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeTypeId").value == "8");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "Fee"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value == 3);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_4__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2ZlZXMvZmVlLWxpc3QvZmVlLXR5cGUuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeTypeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-fee-type',
          templateUrl: './fee-type.component.html',
          styleUrls: ['./fee-type.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }];
      }, {
        Parent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        FeeGroup: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        Currency: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        FeeTypes: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        FeeSubTypes: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        FeeConstraintTypes: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/fees/fee-list/filter.pipe.ts": function srcAppFeesFeeListFilterPipeTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FilterPipe", function () {
      return FilterPipe;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");

    var FilterPipe = /*#__PURE__*/function () {
      function FilterPipe() {
        _classCallCheck(this, FilterPipe);
      }

      _createClass(FilterPipe, [{
        key: "transform",
        value: function transform(items, field, value) {
          if (!items) {
            return [];
          }

          if (!field || !value) {
            return items;
          }

          var filtered = items.filter(function (singleItem) {
            return singleItem[field].value == value;
          });
          return filtered;
        }
      }]);

      return FilterPipe;
    }();

    FilterPipe.ɵfac = function FilterPipe_Factory(t) {
      return new (t || FilterPipe)();
    };

    FilterPipe.ɵpipe = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefinePipe"]({
      name: "filter",
      type: FilterPipe,
      pure: true
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FilterPipe, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Pipe"],
        args: [{
          name: 'filter'
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/fees/fees.module.ts": function srcAppFeesFeesModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeesModule", function () {
      return FeesModule;
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


    var _fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./fee-list/fee-list.component */
    "./src/app/fees/fee-list/fee-list.component.ts");
    /* harmony import */


    var _fee_list_fee_type_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./fee-list/fee-type.component */
    "./src/app/fees/fee-list/fee-type.component.ts");
    /* harmony import */


    var _fee_list_filter_pipe__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./fee-list/filter.pipe */
    "./src/app/fees/fee-list/filter.pipe.ts");
    /* harmony import */


    var _modules_global_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../modules/global.module */
    "./src/app/modules/global.module.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");

    var FeesModule = function FeesModule() {
      _classCallCheck(this, FeesModule);
    };

    FeesModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: FeesModule
    });
    FeesModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function FeesModule_Factory(t) {
        return new (t || FeesModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_global_module__WEBPACK_IMPORTED_MODULE_5__["GlobalModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["NgMultiSelectDropDownModule"].forRoot(), angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__["ConfirmationPopoverModule"].forRoot({
        confirmButtonType: 'danger' // set defaults here

      }), _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["AngularMultiSelectModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](FeesModule, {
        declarations: [_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_2__["FeeListComponent"], _fee_list_fee_type_component__WEBPACK_IMPORTED_MODULE_3__["FeeTypeComponent"], _fee_list_filter_pipe__WEBPACK_IMPORTED_MODULE_4__["FilterPipe"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_global_module__WEBPACK_IMPORTED_MODULE_5__["GlobalModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["NgMultiSelectDropDownModule"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__["ConfirmationPopoverModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["AngularMultiSelectModule"]],
        exports: [_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_2__["FeeListComponent"], _fee_list_fee_type_component__WEBPACK_IMPORTED_MODULE_3__["FeeTypeComponent"], _fee_list_filter_pipe__WEBPACK_IMPORTED_MODULE_4__["FilterPipe"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_2__["FeeListComponent"], _fee_list_fee_type_component__WEBPACK_IMPORTED_MODULE_3__["FeeTypeComponent"], _fee_list_filter_pipe__WEBPACK_IMPORTED_MODULE_4__["FilterPipe"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_global_module__WEBPACK_IMPORTED_MODULE_5__["GlobalModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbModule"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["NgMultiSelectDropDownModule"].forRoot(), angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_8__["ConfirmationPopoverModule"].forRoot({
            confirmButtonType: 'danger' // set defaults here

          }), _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["AngularMultiSelectModule"]],
          exports: [_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_2__["FeeListComponent"], _fee_list_fee_type_component__WEBPACK_IMPORTED_MODULE_3__["FeeTypeComponent"], _fee_list_filter_pipe__WEBPACK_IMPORTED_MODULE_4__["FilterPipe"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/fees/model.ts": function srcAppFeesModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeeModel", function () {
      return FeeModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ByQuantityModel", function () {
      return ByQuantityModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "IsMatched", function () {
      return IsMatched;
    });

    var FeeModel = function FeeModel() {
      _classCallCheck(this, FeeModel);
    };

    var ByQuantityModel = function ByQuantityModel() {
      _classCallCheck(this, ByQuantityModel);

      this.MinQuantity = 0;
    };

    function IsMatched(x, y) {
      return y.FeeTypeId == x.FeeTypeId && y.FeeSubTypeId == x.FeeSubTypeId && y.CommonFee == x.CommonFee && y.FeeConstraintTypeId == x.FeeConstraintTypeId;
    }

    ;
    /***/
  },

  /***/
  "./src/app/fees/service/fee.service.ts": function srcAppFeesServiceFeeServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeeService", function () {
      return FeeService;
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


    var _errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var FeeService = /*#__PURE__*/function (_errors_HandleError__) {
      _inherits(FeeService, _errors_HandleError__);

      var _super = _createSuper(FeeService);

      //private getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';
      function FeeService(httpClient) {
        var _this9;

        _classCallCheck(this, FeeService);

        _this9 = _super.call(this);
        _this9.httpClient = httpClient;
        _this9.getFeeTypesUrl = '/Supplier/Invoice/GetFeeTypes?orderId=';
        _this9.getFeeSubTypesUrl = '/Supplier/Invoice/GetFeeSubTypes?orderId=';
        _this9.getSalesFeeTypesUrl = '/SalesUser/SourcingRequest/GetFeeTypes?currency=';
        _this9.getSalesFeeSubTypesUrl = '/SalesUser/SourcingRequest/GetFeeSubTypes?currency=';
        _this9.getFeeConstraintTypesUrl = '/Supplier/Invoice/GetFeeConstraintTypes';
        return _this9;
      }

      _createClass(FeeService, [{
        key: "getFeeTypes",
        value: function getFeeTypes(orderId, currancy, isSale) {
          if (isSale) {
            return this.httpClient.get(this.getSalesFeeTypesUrl + currancy).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getSalesFeeTypesUrl', [])));
          } else {
            return this.httpClient.get(this.getFeeTypesUrl + orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeTypes', [])));
          }
        }
      }, {
        key: "getFeeSubTypes",
        value: function getFeeSubTypes(orderId, currancy, isSale) {
          if (isSale) {
            return this.httpClient.get(this.getSalesFeeSubTypesUrl + currancy).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getSalesFeeSubTypesUrl', [])));
          } else {
            return this.httpClient.get(this.getFeeSubTypesUrl + orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeSubTypes', [])));
          }
        }
      }, {
        key: "getFeeConstraintTypes",
        value: function getFeeConstraintTypes() {
          return this.httpClient.get(this.getFeeConstraintTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeConstraintTypes', [])));
        }
      }]);

      return FeeService;
    }(_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"]);

    FeeService.ɵfac = function FeeService_Factory(t) {
      return new (t || FeeService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    FeeService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: FeeService,
      factory: FeeService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeService, [{
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
  "./src/app/invoice/services/invoice.service.ts": function srcAppInvoiceServicesInvoiceServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "InvoiceService", function () {
      return InvoiceService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../../errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var InvoiceService = /*#__PURE__*/function (_errors_HandleError__2) {
      _inherits(InvoiceService, _errors_HandleError__2);

      var _super2 = _createSuper(InvoiceService);

      function InvoiceService(httpClient) {
        var _this10;

        _classCallCheck(this, InvoiceService);

        _this10 = _super2.call(this);
        _this10.httpClient = httpClient;
        _this10.getPoListUrl = '/Supplier/invoice/GetCustomerPoList?orderId=';
        _this10.getDefaultUrl = '/Supplier/Invoice/GetInvoiceViewModel?orderId=';
        _this10.getAssetstUrl = '/Supplier/Invoice/GetAssignedAssets?orderId=';
        _this10.getAnotherProductUrl = '/Supplier/Invoice/GetInvoiceDropModel?orderId=';
        _this10.getTerminalsUrl = '/Supplier/Invoice/GetTerminals?orderId=';
        _this10.getTerminalPriceByIdUrl = '/Supplier/Invoice/GetTerminalPriceById';
        _this10.postCreateInvoiceUrl = '/Supplier/Invoice/CreateNew';
        _this10.postConvertToInvoiceUrl = '/Supplier/Invoice/ConvertToInvoice?ddtId=';
        _this10.getInvoiceDropFeesUrl = '/Supplier/invoice/GetInvoiceDropFees?orderId=';
        _this10.getSchedulesUrl = '/Supplier/invoice/GetDeliverySchedules?orderId=';
        _this10.getInvoiceDetailsUrl = '/Supplier/Invoice/GetOriginalInvoiceDetails?invoiceId=';
        _this10.getTaxePricingTypesUrl = '/Supplier/Invoice/GetTaxePricingTypes?orderId=';
        _this10.getDriverListUrl = '/Supplier/Invoice/GetAllDrivers';
        _this10.getsastokenurl = '/Supplier/Invoice/GetSasToken';
        _this10.getAssignedurl = '/Supplier/Invoice/GetAssignedDriver?scheduleId=';
        _this10.getCalculatedDropqtysUrl = '/Supplier/Invoice/CalculateDropQuantitiesFromPrePostForCreateInvoice';
        _this10.validateGravityUrl = '/Supplier/Invoice/ValidateGravityAndConvertForMFN';
        _this10.getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';
        _this10.getEiaPriceAutoFreightMethodUrl = 'Supplier/Invoice/GetEIAPriceForAutoFreightMethod';
        _this10.getBlendedProductsUrl = '/Supplier/Invoice/GetBlendedProducts?blendGroupId=';
        _this10.getFreightTableTypesUrl = '/FreightRate/GetFreightRateRuleTypes';
        _this10.getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
        _this10.getFreightTableNameUrl = '/Freight/GetFreightRateTablesForInvoice';
        _this10.getFuelSurchargeTableNameUrl = 'FuelSurcharge/GetFuelSurchargeTablesForInvoice';
        _this10.getAccessorialTableNameUrl = 'FuelSurcharge/GetAccessorialFeeTablesForInvoice';
        _this10.GetAccessorialFeeTablesForConsolidatedUrl = 'FuelSurcharge/GetAccessorialFeeTablesForConsolidated';
        _this10.GetAccessorialFeeTablesForSelectedOrderUrl = 'FuelSurcharge/GetAccessorialFeeTablesForSelectedOrder';
        _this10.getAccessorialFeeByOrderUrl = 'Supplier/Invoice/GetAccessorialFeeByOrder';
        _this10.getAccessorialFeeByAccessorialFeeIdUrl = 'Supplier/Invoice/GetAccessorialFeeByAccessorialFeeId';
        _this10.GetFreightCostForAutoInvoiceUrl = 'FuelSurcharge/GetFreightCostForInvoice';
        return _this10;
      }

      _createClass(InvoiceService, [{
        key: "getDefaultDetail",
        value: function getDefaultDetail(_orderId, trackableScheduleId) {
          return this.httpClient.get(this.getDefaultUrl + _orderId + '&trackableScheduleId=' + trackableScheduleId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDefaultDetail', null)));
        }
      }, {
        key: "getPoList",
        value: function getPoList(orderId) {
          return this.httpClient.get(this.getPoListUrl + orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getPoList', null)));
        }
      }, {
        key: "getAssets",
        value: function getAssets(orderId) {
          return this.httpClient.get(this.getAssetstUrl + orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getAssets', [])));
        }
      }, {
        key: "getAnotherProductDetail",
        value: function getAnotherProductDetail(_orderId) {
          return this.httpClient.get(this.getAnotherProductUrl + _orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getAnotherProductDetail', null)));
        }
      }, {
        key: "getTerminals",
        value: function getTerminals(_orderId, _terminal) {
          return this.httpClient.get(this.getTerminalsUrl + _orderId + '&terminal=' + _terminal).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTerminals', null)));
        }
      }, {
        key: "getTerminalPriceById",
        value: function getTerminalPriceById(terminalId, orderId, deliveryDate) {
          return this.httpClient.post(this.getTerminalPriceByIdUrl, {
            terminalId: terminalId,
            orderId: orderId,
            deliveryDate: deliveryDate
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTerminalPriceById')));
        }
      }, {
        key: "getInvoiceDropFees",
        value: function getInvoiceDropFees(_orderId) {
          return this.httpClient.get(this.getInvoiceDropFeesUrl + _orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getInvoiceDropFees', null)));
        }
      }, {
        key: "getSchedules",
        value: function getSchedules(_orderId) {
          return this.httpClient.get(this.getSchedulesUrl + _orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSchedules', null)));
        }
      }, {
        key: "getSASforblobStorage",
        value: function getSASforblobStorage() {
          return this.httpClient.get(this.getsastokenurl);
        }
      }, {
        key: "postCreateInvoice",
        value: function postCreateInvoice(invoiceModel, existingId) {
          if (existingId > 0 && !invoiceModel.IsRebillInvoice) {
            return this.httpClient.post(this.postConvertToInvoiceUrl + existingId, invoiceModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postConvertToInvoice', null)));
          } else {
            return this.httpClient.post(this.postCreateInvoiceUrl, invoiceModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postCreateInvoice', null)));
          }
        }
      }, {
        key: "postConvertToInvoice",
        value: function postConvertToInvoice(invoiceModel, ddtId) {
          return this.httpClient.post(this.postConvertToInvoiceUrl + ddtId, invoiceModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postConvertToInvoice', null)));
        }
      }, {
        key: "getInvoiceDetails",
        value: function getInvoiceDetails(_invoiceId) {
          return this.httpClient.get(this.getInvoiceDetailsUrl + _invoiceId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getInvoiceDetails', null)));
        }
      }, {
        key: "getBlendedProducts",
        value: function getBlendedProducts(_blendGroupId) {
          return this.httpClient.get(this.getBlendedProductsUrl + _blendGroupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBlendedProducts', null)));
        }
      }, {
        key: "getTaxePricingTypes",
        value: function getTaxePricingTypes(_orderId) {
          return this.httpClient.get(this.getTaxePricingTypesUrl + _orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTaxePricingTypes', null)));
        }
      }, {
        key: "getDriverList",
        value: function getDriverList() {
          return this.httpClient.get(this.getDriverListUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDriverList', null)));
        }
      }, {
        key: "getAssignedDriverForSchedule",
        value: function getAssignedDriverForSchedule(_scheduleId, _orderId) {
          return this.httpClient.get(this.getAssignedurl + _scheduleId + '&orderId=' + _orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getAssignedDriverForSchedule', null)));
        }
      }, {
        key: "postPrePostAssetsInfo",
        value: function postPrePostAssetsInfo(assetInfo) {
          return this.httpClient.post(this.getCalculatedDropqtysUrl, assetInfo).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postPrePostAssetsInfo', null)));
        }
      }, {
        key: "ValidateGravityAndConvertForMFN",
        value: function ValidateGravityAndConvertForMFN(conversionRequest) {
          return this.httpClient.post(this.validateGravityUrl, conversionRequest).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('ValidateGravityAndConvertForMFN', null)));
        }
      }, {
        key: "getEiaPrice",
        value: function getEiaPrice(data) {
          return this.httpClient.post(this.getEiaPriceUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getEiaPrice', [])));
        }
      }, {
        key: "getEiaPriceAutoFreightMethod",
        value: function getEiaPriceAutoFreightMethod(data) {
          return this.httpClient.post(this.getEiaPriceAutoFreightMethodUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getEiaPriceAutoFreightMethod', [])));
        }
      }, {
        key: "getFreightTable",
        value: function getFreightTable() {
          return this.httpClient.get(this.getFreightTableTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFreightTable', null)));
        }
      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          return this.httpClient.get(this.getTableTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetTableTypes', null)));
        }
      }, {
        key: "getFreightTableName",
        value: function getFreightTableName(filter) {
          return this.httpClient.post(this.getFreightTableNameUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFreightTableName', null)));
        }
      }, {
        key: "getFuelSurchargeTableName",
        value: function getFuelSurchargeTableName(filter) {
          return this.httpClient.post(this.getFuelSurchargeTableNameUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelSurchargeTableName', null)));
        }
      }, {
        key: "getAccessorialTableName",
        value: function getAccessorialTableName(filter) {
          return this.httpClient.post(this.getAccessorialTableNameUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getAccessorialTableName', null)));
        }
      }, {
        key: "GetAccessorialFeeTablesForConsolidated",
        value: function GetAccessorialFeeTablesForConsolidated(filter) {
          return this.httpClient.post(this.GetAccessorialFeeTablesForConsolidatedUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetAccessorialFeeTablesForConsolidated', null)));
        }
      }, {
        key: "GetAccessorialFeeTablesForSelectedOrder",
        value: function GetAccessorialFeeTablesForSelectedOrder(orderIds) {
          return this.httpClient.post(this.GetAccessorialFeeTablesForSelectedOrderUrl, {
            orderIds: orderIds
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetAccessorialFeeTablesForSelectedOrder', null)));
        }
      }, {
        key: "GetAccessorialFeeByOrder",
        value: function GetAccessorialFeeByOrder(orderId) {
          return this.httpClient.post(this.getAccessorialFeeByOrderUrl, {
            orderId: orderId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetAccessorialFeeByOrder', null)));
        }
      }, {
        key: "GetAccessorialFeeByAccessorialFeeId",
        value: function GetAccessorialFeeByAccessorialFeeId(accessorialFeeId) {
          return this.httpClient.post(this.getAccessorialFeeByAccessorialFeeIdUrl, {
            accessorialFeeId: accessorialFeeId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetAccessorialFeeByAccessorialFeeId', null)));
        }
      }, {
        key: "GetFreightCostForAutoInvoice",
        value: function GetFreightCostForAutoInvoice(filter) {
          return this.httpClient.post(this.GetFreightCostForAutoInvoiceUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetFreightCostForAutoInvoice', null)));
        }
      }]);

      return InvoiceService;
    }(_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    InvoiceService.ɵfac = function InvoiceService_Factory(t) {
      return new (t || InvoiceService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    InvoiceService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: InvoiceService,
      factory: InvoiceService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](InvoiceService, [{
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

  }
}]);
//# sourceMappingURL=default~invoice-lazy-loading-invoice-module~sales-user-sales-user-module-es5.js.map