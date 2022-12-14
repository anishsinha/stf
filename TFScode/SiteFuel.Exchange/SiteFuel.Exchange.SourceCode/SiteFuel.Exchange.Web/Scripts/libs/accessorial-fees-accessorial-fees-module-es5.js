function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _iterableToArrayLimit(arr, i) { var _i = arr == null ? null : typeof Symbol !== "undefined" && arr[Symbol.iterator] || arr["@@iterator"]; if (_i == null) return; var _arr = []; var _n = true; var _d = false; var _s, _e; try { for (_i = _i.call(arr); !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["accessorial-fees-accessorial-fees-module"], {
  /***/
  "./src/app/accessorial-fees/accessorial-fees.module.ts": function srcAppAccessorialFeesAccessorialFeesModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AccessorialFeesModule", function () {
      return AccessorialFeesModule;
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


    var _master_master_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./master/master.component */
    "./src/app/accessorial-fees/master/master.component.ts");
    /* harmony import */


    var _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./view/view-accessorial-fees.component */
    "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./create/create-accessorial-fees.component */
    "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./create/child-components/fee-list.component */
    "./src/app/accessorial-fees/create/child-components/fee-list.component.ts");
    /* harmony import */


    var _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./create/child-components/fee-type.component */
    "./src/app/accessorial-fees/create/child-components/fee-type.component.ts");
    /* harmony import */


    var _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./view/view-fees-details/view-fees-details.component */
    "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");

    var route = [{
      path: '',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"]
    }, {
      path: 'Create',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"]
    }];

    var AccessorialFeesModule = function AccessorialFeesModule() {
      _classCallCheck(this, AccessorialFeesModule);
    };

    AccessorialFeesModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({
      type: AccessorialFeesModule
    });
    AccessorialFeesModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({
      factory: function AccessorialFeesModule_Factory(t) {
        return new (t || AccessorialFeesModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route), angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](AccessorialFeesModule, {
        declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["ViewAccessorialFeesComponent"], _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__["CreateAccessorialFeesComponent"], _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__["FeeListComponent"], _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__["FeeTypeComponent"], _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__["ViewFeesDetailsComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](AccessorialFeesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["ViewAccessorialFeesComponent"], _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__["CreateAccessorialFeesComponent"], _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__["FeeListComponent"], _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__["FeeTypeComponent"], _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__["ViewFeesDetailsComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route), angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/accessorial-fees/create/child-components/fee-list.component.ts": function srcAppAccessorialFeesCreateChildComponentsFeeListComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../../invoice/models/DropDetail */
    "./src/app/invoice/models/DropDetail.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../../invoice/services/fee.service */
    "./src/app/invoice/services/fee.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../../carrier/service/data.service */
    "./src/app/carrier/service/data.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _fee_type_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./fee-type.component */
    "./src/app/accessorial-fees/create/child-components/fee-type.component.ts");

    function FeeListComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeListComponent_ng_container_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "app-fee-type", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_ng_container_8_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r9);

          var commonFee_r5 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r8.removeGeneralFee(true, commonFee_r5);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var commonFee_r5 = ctx.$implicit;

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("Parent", ctx_r1.Parent)("FeeGroup", commonFee_r5)("FeeTypes", ctx_r1.FeeTypes)("Currency", ctx_r1.DisplayCurrency)("FeeConstraintTypes", ctx_r1.FeeConstraintTypes)("FeeSubTypes", ctx_r1.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "app-fee-type", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_ng_container_15_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r14);

          var otherFee_r10 = ctx.$implicit;

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r13.removeGeneralFee(false, otherFee_r10);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var otherFee_r10 = ctx.$implicit;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("Parent", ctx_r2.Parent)("FeeGroup", otherFee_r10)("FeeTypes", ctx_r2.FeeTypes)("Currency", ctx_r2.DisplayCurrency)("FeeConstraintTypes", ctx_r2.FeeConstraintTypes)("FeeSubTypes", ctx_r2.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "app-fee-type", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_ng_container_25_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r19);

          var spCommonFee_r15 = ctx.$implicit;

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r18.removeSpecialFee(true, spCommonFee_r15);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var spCommonFee_r15 = ctx.$implicit;

        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("Parent", ctx_r3.Parent)("FeeGroup", spCommonFee_r15)("FeeTypes", ctx_r3.FeeTypes)("Currency", ctx_r3.DisplayCurrency)("FeeConstraintTypes", ctx_r3.FeeConstraintTypes)("FeeSubTypes", ctx_r3.FeeSubTypes);
      }
    }

    function FeeListComponent_ng_container_32_Template(rf, ctx) {
      if (rf & 1) {
        var _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "app-fee-type", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_ng_container_32_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r24);

          var spOtherFee_r20 = ctx.$implicit;

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r23.removeSpecialFee(false, spOtherFee_r20);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
      }

      if (rf & 2) {
        var spOtherFee_r20 = ctx.$implicit;

        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("Parent", ctx_r4.Parent)("FeeGroup", spOtherFee_r20)("FeeTypes", ctx_r4.FeeTypes)("Currency", ctx_r4.DisplayCurrency)("FeeConstraintTypes", ctx_r4.FeeConstraintTypes)("FeeSubTypes", ctx_r4.FeeSubTypes);
      }
    }

    var FeeListComponent = /*#__PURE__*/function () {
      //public OrderId: number;
      function FeeListComponent(fb, feeService, route, dataService) {
        _classCallCheck(this, FeeListComponent);

        this.fb = fb;
        this.feeService = feeService;
        this.route = route;
        this.dataService = dataService;
        this.IsLoading = false;
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];
      }

      _createClass(FeeListComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this = this;

          //this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
          this.Parent.addControl('Fees', this.fb.array([]));
          this.IsLoading = true;
          this.feeService.getFeeTypes(0, true).subscribe(function (data) {
            _this.IsLoading = false;
            _this.FeeTypes = data;
          });
          this.feeService.getFeeConstraintTypes().subscribe(function (data) {
            _this.FeeConstraintTypes = data;
          });
          this.feeService.getFeeSubTypes(0).subscribe(function (data) {
            _this.FeeSubTypes = data.filter(function (elem) {
              return elem.FeeSubTypeId != 1;
            });
          });
          this.dataService.RemoveFeesSubject.subscribe(function (x) {
            _this.removeFeesOnCreateNew();
          });
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          var _this2 = this;

          if (change.CountryId && change.CountryId.currentValue) {
            var currency = change.CountryId.currentValue;

            if (currency == 1) {
              this.DisplayCurrency = "USD";
            } else if (currency == 2) {
              this.DisplayCurrency = "CAD";
            }
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

            var currValues = change.Fees.currentValue;
            currValues.forEach(function (x) {
              if (x.FeeConstraintTypeId == null) {
                _this2.addGeneralFee(x.CommonFee, x);
              } else {
                _this2.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
              }
            });
          }
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

          var group = this.fb.group({
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
            OtherFeeDescription: this.fb.control(model.OtherFeeDescription),
            MinimumGallons: this.fb.control(model.MinimumGallons),
            Fee: this.fb.control(model.Fee, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            DeliveryFeeByQuantity: this.fb.array(byQuantity)
          });
          return group;
        }
      }, {
        key: "addGeneralFee",
        value: function addGeneralFee(_commonFee, feeObj) {
          if (feeObj == null) {
            feeObj = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
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
            feeObj = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
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

          var feeObj = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
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
        }
      }, {
        key: "requiredIfValidator",
        value: function requiredIfValidator(predicate) {
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
      }, {
        key: "removeFeesOnCreateNew",
        value: function removeFeesOnCreateNew() {
          var _this3 = this;

          this.CommonFees.forEach(function (commonFee) {
            _this3.removeGeneralFee(true, commonFee);
          });
          this.OtherFees.forEach(function (OtherFee) {
            _this3.removeGeneralFee(false, OtherFee);
          });
          this.SpCommonFees.forEach(function (SpCommonFee) {
            _this3.removeSpecialFee(true, SpCommonFee);
          });
          this.SpOtherFees.forEach(function (SpOtherFee) {
            _this3.removeSpecialFee(false, SpOtherFee);
          });
        }
      }]);

      return FeeListComponent;
    }();

    FeeListComponent.??fac = function FeeListComponent_Factory(t) {
      return new (t || FeeListComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__["FeeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_5__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__["DataService"]));
    };

    FeeListComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: FeeListComponent,
      selectors: [["app-fee-list"]],
      inputs: {
        Parent: "Parent",
        CountryId: "CountryId",
        Fees: "Fees"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["????NgOnChangesFeature"]],
      decls: 36,
      vars: 6,
      consts: [["class", "loader", 4, "ngIf"], [1, "well", "box-shadow", 3, "formGroup"], ["formArrayName", "Fees"], [1, "mt10", "mb5"], [4, "ngFor", "ngForOf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "mt10"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "row"], [1, "col-sm-10"], [3, "Parent", "FeeGroup", "FeeTypes", "Currency", "FeeConstraintTypes", "FeeSubTypes"], [1, "col-sm-2"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", "mt10", 3, "click"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", 3, "click"]],
      template: function FeeListComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](0, FeeListComponent_div_0_Template, 5, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Fees");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "General");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, FeeListComponent_ng_container_8_Template, 6, 6, "ng-container", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_Template_button_click_9_listener() {
            return ctx.addGeneralFee(true, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](10, "i", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](11, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14, "Other");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, FeeListComponent_ng_container_15_Template, 6, 6, "ng-container", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_Template_button_click_16_listener() {
            return ctx.addGeneralFee(false, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](17, "i", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](21, "Weekend / Holiday Fee(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "General");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](25, FeeListComponent_ng_container_25_Template, 6, 6, "ng-container", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_Template_button_click_26_listener() {
            return ctx.addSpecialFee(true, 1, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](27, "i", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](31, "Other");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](32, FeeListComponent_ng_container_32_Template, 6, 6, "ng-container", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeListComponent_Template_button_click_33_listener() {
            return ctx.addSpecialFee(false, 1, null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](34, "i", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](35, " Add Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.Parent);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.CommonFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.OtherFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.SpCommonFees);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.SpOtherFees);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _fee_type_component__WEBPACK_IMPORTED_MODULE_8__["FeeTypeComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NoaWxkLWNvbXBvbmVudHMvZmVlLWxpc3QuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](FeeListComponent, [{
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
          type: _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__["FeeService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_5__["ActivatedRoute"]
        }, {
          type: _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__["DataService"]
        }];
      }, {
        Parent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        CountryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        Fees: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/accessorial-fees/create/child-components/fee-type.component.ts": function srcAppAccessorialFeesCreateChildComponentsFeeTypeComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../../invoice/models/DropDetail */
    "./src/app/invoice/models/DropDetail.ts");
    /* harmony import */


    var _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../../../invoice/services/fee.service */
    "./src/app/invoice/services/fee.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function FeeTypeComponent_option_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ft_r12 = ctx.$implicit;

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", ft_r12.Code)("selected", ft_r12.Code == ctx_r1.FeeGroup.get("FeeTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ft_r12.Name);
      }
    }

    function FeeTypeComponent_span_9_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_span_9_span_1_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r2.isRequired(ctx_r2.FeeGroup, "FeeTypeId") || ctx_r2.isFeeNameRequired(ctx_r2.FeeGroup, "OtherFeeDescription"));
      }
    }

    function FeeTypeComponent_option_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var fst_r14 = ctx.$implicit;

        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", fst_r14.FeeSubTypeId)("selected", fst_r14.FeeSubTypeId == ctx_r3.FeeGroup.get("FeeSubTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", fst_r14.SubTypeName, " ");
      }
    }

    function FeeTypeComponent_span_16_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_span_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_span_16_span_1_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r4.isRequired(ctx_r4.FeeGroup, "FeeSubTypeId"));
      }
    }

    function FeeTypeComponent_div_17_select_2_option_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var fc_r18 = ctx.$implicit;

        var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", fc_r18.Id)("selected", fc_r18.Id == ctx_r17.FeeGroup.get("FeeConstraintTypeId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", fc_r18.Name, " ");
      }
    }

    function FeeTypeComponent_div_17_select_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "select", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_div_17_select_2_option_1_Template, 2, 3, "option", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r16.FeeConstraintTypes);
      }
    }

    function FeeTypeComponent_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](2, FeeTypeComponent_div_17_select_2_Template, 2, 1, "select", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r5.FeeGroup.get("FeeConstraintTypeId").value);
      }
    }

    function FeeTypeComponent_div_18_Template(rf, ctx) {
      if (rf & 1) {
        var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "input", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDateChange", function FeeTypeComponent_div_18_Template_input_onDateChange_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r20);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r19.FeeGroup.get("SpecialDate").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("maxDate", ctx_r6.maxDate)("minDate", ctx_r6.minDate)("format", "MM/DD/YYYY");
      }
    }

    function FeeTypeComponent_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "input", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_input_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "input", 29);
      }
    }

    function FeeTypeComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r9.DisplayCurrency);
      }
    }

    function FeeTypeComponent_span_25_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_span_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_span_25_span_1_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.isRequired(ctx_r10.FeeGroup, "Fee"));
      }
    }

    function FeeTypeComponent_div_26_div_2_span_7_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_div_26_div_2_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_div_26_div_2_span_7_span_1_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var byQty_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r25.isRequired(byQty_r23, "MaxQuantity"));
      }
    }

    function FeeTypeComponent_div_26_div_2_span_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function FeeTypeComponent_div_26_div_2_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, FeeTypeComponent_div_26_div_2_span_10_span_1_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var byQty_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;

        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r26.isRequired(byQty_r23, "Fee"));
      }
    }

    function FeeTypeComponent_div_26_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "input", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "input", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "input", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](7, FeeTypeComponent_div_26_div_2_span_7_Template, 2, 1, "span", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](9, "input", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](10, FeeTypeComponent_div_26_div_2_span_10_Template, 2, 1, "span", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "a", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeTypeComponent_div_26_div_2_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r32);

          var i_r24 = ctx.index;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

          return ctx_r31.removeByQtyFee(ctx_r31.FeeGroup, i_r24);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](13, "i", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var byQty_r23 = ctx.$implicit;
        var i_r24 = ctx.index;

        var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroupName", i_r24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r22.isInvalid(byQty_r23, "MaxQuantity"));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r22.isInvalid(byQty_r23, "Fee"));
      }
    }

    function FeeTypeComponent_div_26_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](2, FeeTypeComponent_div_26_div_2_Template, 14, 3, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "a", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function FeeTypeComponent_div_26_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r34);

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r33.addByQtyFee(ctx_r33.FeeGroup, null);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](6, "i", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, " Add Quantity Range");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r11.FeeGroup.get("DeliveryFeeByQuantity")["controls"]);
      }
    }

    var FeeTypeComponent = /*#__PURE__*/function () {
      function FeeTypeComponent(fb, feeService) {
        _classCallCheck(this, FeeTypeComponent);

        this.fb = fb;
        this.feeService = feeService;
        this.maxDate = new Date();
        this.minDate = new Date();
        this.DisplayFeeTypes = [];
      }

      _createClass(FeeTypeComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
          this.FeeGroup.setValidators(this.feeNameRequired('FeeTypeId', 'OtherFeeDescription', 'CommonFee'));
          if (this.FeeSubTypes != null && this.FeeSubTypes != undefined) this.DisplayFeeTypes = this.FeeSubTypes.slice();
          this.DisplayCurrency = this.Currency;
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.FeeSubTypes && change.FeeSubTypes.currentValue != null) {
            var subTypes = change.FeeSubTypes.currentValue;
            this.DisplayFeeTypes = subTypes;
          }
        }
      }, {
        key: "updateSubType",
        value: function updateSubType(feeTypeId) {
          this.DisplayFeeTypes = this.FeeSubTypes.slice().filter(function (elem) {
            return elem.FeeTypeId == feeTypeId;
          });
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
            feeObj = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__["ByQuantityModel"]();
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

    FeeTypeComponent.??fac = function FeeTypeComponent_Factory(t) {
      return new (t || FeeTypeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__["FeeService"]));
    };

    FeeTypeComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
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
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["????NgOnChangesFeature"]],
      decls: 32,
      vars: 18,
      consts: [[1, "row", 3, "formGroup"], [1, "col-sm-3"], ["formControlName", "FeeTypeId", 1, "form-control", 3, "change"], ["feeTypeId", ""], [3, "value"], [3, "value", "selected", 4, "ngFor", "ngForOf"], [1, "mb15", "form-group"], ["type", "text", "formControlName", "OtherFeeDescription", "placeholder", "Fee Name", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "form-group"], ["formControlName", "FeeSubTypeId", 1, "form-control", 3, "focus", "change"], ["class", "color-maroon pa", 4, "ngIf"], ["class", "col-sm-3", 4, "ngIf"], [1, "input-group"], ["type", "number", "formControlName", "Fee", "class", "form-control", "placeholder", "Fee", 4, "ngIf"], ["class", "input-group-addon fs12", 4, "ngIf"], ["class", "col-sm-9", 4, "ngIf"], [1, "col-sm-2", "text-lg-right", "mt-2"], ["type", "checkbox", "formControlName", "IncludeInPPG"], [1, "ml-2"], ["type", "hidden", "formControlName", "Currency"], [3, "value", "selected"], [1, "color-maroon"], [4, "ngIf"], [1, "color-maroon", "pa"], ["formControlName", "FeeConstraintTypeId", "class", "form-control", 4, "ngIf"], ["formControlName", "FeeConstraintTypeId", 1, "form-control"], ["type", "text", "formControlName", "SpecialDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "maxDate", "minDate", "format", "onDateChange"], ["type", "text", "formControlName", "MinimumGallons", "placeholder", "Min Quantity", 1, "form-control"], ["type", "number", "formControlName", "Fee", "placeholder", "Fee", 1, "form-control"], [1, "input-group-addon", "fs12"], [1, "col-sm-9"], ["formArrayName", "DeliveryFeeByQuantity"], [4, "ngFor", "ngForOf"], [1, "row", "mb10"], [1, "col-sm-12"], [3, "click"], [1, "fa", "fa-plus-circle"], [1, "row", 3, "formGroupName"], [1, "col-sm-3", "pr0", "mb5"], ["type", "text", "formControlName", "MinQuantity", "readonly", "readonly", "placeholder", "Min Quantity", 1, "form-control"], ["type", "text", "formControlName", "MaxQuantity", "placeholder", "Max Quantity", 1, "form-control"], ["type", "text", "formControlName", "Fee", "placeholder", "Fee", 1, "form-control"], [1, "col-sm-1"], [1, "fa", "fa-trash-alt", "color-maroon", "mt10"]],
      template: function FeeTypeComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "select", 2, 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function FeeTypeComponent_Template_select_change_2_listener() {
            return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "option", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Select Fee");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](6, FeeTypeComponent_option_6_Template, 2, 3, "option", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](8, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](9, FeeTypeComponent_span_9_Template, 2, 1, "span", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "select", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("focus", function FeeTypeComponent_Template_select_focus_12_listener() {
            return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value);
          })("change", function FeeTypeComponent_Template_select_change_12_listener() {
            return ctx.handleByQuantity(ctx.FeeGroup, ctx.FeeGroup.get("FeeSubTypeId").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "option", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14, "Select Fee Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, FeeTypeComponent_option_15_Template, 2, 3, "option", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, FeeTypeComponent_span_16_Template, 2, 1, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, FeeTypeComponent_div_17_Template, 3, 1, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](18, FeeTypeComponent_div_18_Template, 3, 3, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, FeeTypeComponent_div_19_Template, 2, 0, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](23, FeeTypeComponent_input_23_Template, 1, 0, "input", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](24, FeeTypeComponent_div_24_Template, 2, 1, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](25, FeeTypeComponent_span_25_Template, 2, 1, "span", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](26, FeeTypeComponent_div_26_Template, 8, 1, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](28, "input", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "label", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Include In PPG ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](31, "input", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.FeeGroup);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????styleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "block" : "none");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.FeeTypes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????styleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "none" : "block");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "FeeTypeId") || ctx.isInvalid(ctx.FeeGroup, "OtherFeeDescription"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.DisplayFeeTypes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "FeeSubTypeId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeConstraintTypeId").value != null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeConstraintTypeId").value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeTypeId").value == "8");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "Fee"));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.FeeGroup.get("FeeSubTypeId").value == 3);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NumberValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NoaWxkLWNvbXBvbmVudHMvZmVlLXR5cGUuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](FeeTypeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-fee-type',
          templateUrl: './fee-type.component.html',
          styleUrls: ['./fee-type.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__["FeeService"]
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
  "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts": function srcAppAccessorialFeesCreateCreateAccessorialFeesComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateAccessorialFeesComponent", function () {
      return CreateAccessorialFeesComponent;
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


    var _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../model/accessorial-fees */
    "./src/app/accessorial-fees/model/accessorial-fees.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../carrier/service/data.service */
    "./src/app/carrier/service/data.service.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ../service/accessorialfees.service */
    "./src/app/accessorial-fees/service/accessorialfees.service.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var _child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ./child-components/fee-list.component */
    "./src/app/accessorial-fees/create/child-components/fee-list.component.ts");

    function CreateAccessorialFeesComponent_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "button", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateAccessorialFeesComponent_div_2_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r9);

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r8.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_12_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_12_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r1.rcForm.get("TableName").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_20_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_20_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.rcForm.get("TableTypes").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_27_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_27_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r3.rcForm.get("Customers").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_34_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_34_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_34_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.rcForm.get("Carriers").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_42_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_42_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_42_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r5.rcForm.get("SourceRegions").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_55_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function CreateAccessorialFeesComponent_div_55_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, CreateAccessorialFeesComponent_div_55_div_1_Template, 2, 0, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r6.rcForm.get("StartDate").errors.required);
      }
    }

    function CreateAccessorialFeesComponent_div_68_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
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

    var CreateAccessorialFeesComponent = /*#__PURE__*/function () {
      function CreateAccessorialFeesComponent(fb, fuelsurchargeService, dataService, regionService, carrierService, accesorialFeeService, http, _document) {
        _classCallCheck(this, CreateAccessorialFeesComponent);

        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.dataService = dataService;
        this.regionService = regionService;
        this.carrierService = carrierService;
        this.accesorialFeeService = accesorialFeeService;
        this.http = http;
        this._document = _document;
        this.minDate = new Date();
        this.maxDate = new Date();
        this.SingleSelectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.IsLoading = false;
        this.SelectedCountryId = -1;
        this.AccessorialFeeMode = "CREATE";
        this.TerminalsAndBulkPlantList = [];
        this.Fees = [];
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.decimalSupportedRegx = /^[0-9]\d{0,9}(\.\d{0,5})?%?$/;
        this.SelectedTerminalsAndBulkPlants = [];
        this.disableInputControls = false;
        this.IsEditable = true;
        this.IsLoaded = true;
        this.onPageSubmit = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
      }

      _createClass(CreateAccessorialFeesComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this4 = this;

          this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
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
            text: "Select Terminals or Bulk Plants",
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
            Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master,
            Name: "Master"
          }]); // default will master

          this.IsMasterSelected = true;
          this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master.toString());
          this.accesorialFeeService.onSelectedAccessorialFeeId.subscribe(function (s) {
            if (s) {
              var stringify = JSON.parse(s);
              _this4.AccessorialFeeId = stringify.AccessorialFeeId;
              _this4.AccessorialFeeMode = stringify.Mode;
            }
          });
          var id = localStorage.getItem("AccessorialFeeId");

          if (id && +id > 0) {
            this.AccessorialFeeId = Number(id);
            this.AccessorialFeeMode = "VIEW";
            localStorage.removeItem("AccessorialFeeId");
          }

          Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.get('SourceRegions').valueChanges).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])()).subscribe(function (_ref) {
            var _ref2 = _slicedToArray(_ref, 2),
                prev = _ref2[0],
                next = _ref2[1];

            if (_this4.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) _this4.SourceRegionChange(prev, next);
          });
        }
      }, {
        key: "getDefaultServingCountry",
        value: function getDefaultServingCountry() {
          var _this5 = this;

          this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(function (data) {
            _this5.SelectedCountryId = Number(data.DefaultCountryId);
          });
        }
      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          var _this6 = this;

          this.fuelsurchargeService.getTableTypes().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this6, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
              return regeneratorRuntime.wrap(function _callee$(_context) {
                while (1) {
                  switch (_context.prev = _context.next) {
                    case 0:
                      _context.next = 2;
                      return data;

                    case 2:
                      this.TableTypeList = _context.sent;
                      this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(function (x) {
                        return x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master;
                      })); // default will master

                      this.rcForm.controls['TableTypeId'].setValue(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master);
                      this.IsMasterSelected = true;

                    case 6:
                    case "end":
                      return _context.stop();
                  }
                }
              }, _callee, this);
            }));
          });
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions(tableType) {
          var _this7 = this;

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

          var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
          sourceRegionInput.TableType = tableType;
          sourceRegionInput.CustomerId = customerIds;
          sourceRegionInput.CarrierId = carrierIds;
          this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this7, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
              return regeneratorRuntime.wrap(function _callee2$(_context2) {
                while (1) {
                  switch (_context2.prev = _context2.next) {
                    case 0:
                      _context2.next = 2;
                      return data;

                    case 2:
                      this.SourceRegionList = _context2.sent;

                    case 3:
                    case "end":
                      return _context2.stop();
                  }
                }
              }, _callee2, this);
            }));
          });
        }
      }, {
        key: "createForm",
        value: function createForm() {
          return this.fb.group({
            AccessorialFeeId: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            TableTypeId: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](),
            TableName: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]),
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.SelectedTerminalsAndBulkPlants),
            StartDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]("", [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]),
            EndDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](""),
            StatusId: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('')
          });
        }
      }, {
        key: "AddRemoveValidations",
        value: function AddRemoveValidations(requiredControls, notRequiredControls) {
          if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {
            requiredControls.forEach(function (ctrl) {
              ctrl.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]);
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
        key: "onTableTypeSelect",
        value: function onTableTypeSelect(item) {
          this.IsMasterSelected = false;
          this.IsCustomerSelected = false;
          this.IsCarrierSelected = false;
          this.rcForm.get('Carriers').patchValue([]);
          this.rcForm.get('Customers').patchValue([]);
          this.rcForm.controls['TableTypeId'].setValue(item.Id);

          switch (item.Id) {
            case 1:
              //master
              this.IsMasterSelected = true;
              this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"

              break;

            case 2:
              // customer
              this.getSupplierCustomers();
              this.getCarriers();
              this.IsCustomerSelected = true;
              this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
              break;

            case 3:
              //carrier
              this.getSupplierCustomers();
              this.getCarriers();
              this.IsCarrierSelected = true;
              this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
              break;
          }

          this.rcForm.get('SourceRegions').patchValue([]);
          this.getSourceRegions(item.Id);
        }
      }, {
        key: "onCarriersSelect",
        value: function onCarriersSelect(item) {
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
          this.rcForm.get('SourceRegions').patchValue([]);
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCarriersDeSelect",
        value: function onCarriersDeSelect(item) {
          this.rcForm.get('SourceRegions').patchValue([]);
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCarriersOrCustomersChange",
        value: function onCarriersOrCustomersChange() {
          var selectedTableType = this.rcForm.get('TableTypes').value;
          this.getSourceRegions(selectedTableType[0].Id.toString());
        }
      }, {
        key: "onSourceRegionsDeSelect",
        value: function onSourceRegionsDeSelect(item) {
          var sr = this.rcForm.get('SourceRegions').value;
          this.IsSourceRegionSelected = sr.length > 0;
        }
      }, {
        key: "onSourceRegionsDeSelectAll",
        value: function onSourceRegionsDeSelectAll(item) {
          this.IsSourceRegionSelected = false;
        }
      }, {
        key: "getSupplierCustomers",
        value: function getSupplierCustomers() {
          var _this8 = this;

          this.fuelsurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this8, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
              return regeneratorRuntime.wrap(function _callee3$(_context3) {
                while (1) {
                  switch (_context3.prev = _context3.next) {
                    case 0:
                      _context3.next = 2;
                      return data;

                    case 2:
                      this.CustomerList = _context3.sent;

                    case 3:
                    case "end":
                      return _context3.stop();
                  }
                }
              }, _callee3, this);
            }));
          });
        }
      }, {
        key: "SourceRegionChange",
        value: function SourceRegionChange(prev, next) {
          var _this9 = this;

          if (prev == null && next.length == 0) return;
          this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
          this.IsSourceRegionSelected = false;
          var ids = [];
          var selectedSourceRegions = this.rcForm.get('SourceRegions').value;

          if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(function (s) {
              return ids.push(s.Id);
            });
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this9, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
                return regeneratorRuntime.wrap(function _callee4$(_context4) {
                  while (1) {
                    switch (_context4.prev = _context4.next) {
                      case 0:
                        _context4.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context4.sent;
                        this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                        this.IsSourceRegionSelected = true;

                      case 5:
                      case "end":
                        return _context4.stop();
                    }
                  }
                }, _callee4, this);
              }));
            });
          }
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this10 = this;

          this.regionService.getCarriers().subscribe(function (carriers) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this10, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
              return regeneratorRuntime.wrap(function _callee5$(_context5) {
                while (1) {
                  switch (_context5.prev = _context5.next) {
                    case 0:
                      _context5.next = 2;
                      return carriers;

                    case 2:
                      this.CarrierList = _context5.sent;

                    case 3:
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
          var _this11 = this;

          var selectedSourceRegions = this.rcForm.get('SourceRegions').value;

          if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(function (s) {
              return s.Id;
            }).join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this11, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
                return regeneratorRuntime.wrap(function _callee6$(_context6) {
                  while (1) {
                    switch (_context6.prev = _context6.next) {
                      case 0:
                        _context6.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context6.sent;

                      case 3:
                      case "end":
                        return _context6.stop();
                    }
                  }
                }, _callee6, this);
              }));
            });
          }
        }
      }, {
        key: "onSubmit",
        value: function onSubmit(status) {
          var _this12 = this;

          var accessorialFeeName = this.rcForm.get('TableName').value;

          if (accessorialFeeName == null || accessorialFeeName == undefined || accessorialFeeName == "") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(" Table Name is required", undefined, undefined);
            return;
          }

          var AccessorialDate = this.rcForm.get('StartDate').value;

          if (AccessorialDate == null || AccessorialDate == undefined || AccessorialDate == "") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(" Date is required", undefined, undefined);
            return;
          }

          var feeModel = this.createPostObject(status);

          if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft && +this.rcForm.controls['StatusId'].value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
            if (this.rcForm.get('AccessorialFeeId').value != "") {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Not allowed. " + this.rcForm.get('TableName').value + " is in published mode.", undefined, undefined);
              this.IsLoading = false;
              return;
            }
          } else if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
            this.rcForm.markAllAsTouched();

            if (this.rcForm.valid) {
              var fees = this.rcForm.get('Fees').value;

              if (fees == null || fees == undefined || fees.length == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Please add Fee(s)", undefined, undefined);
                return;
              }
            }
          }

          if (this.rcForm.get('AccessorialFeeId').value != "") {
            this.accesorialFeeService.updateAccessorialFee(feeModel).subscribe(function (response) {
              _this12.ServiceResponse = response;

              if (response != null && response.StatusCode == 0) {
                var message = " edited";

                if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                  message = " saved draft";
                }

                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                _this12.IsLoading = false;

                _this12.changeViewType(2);
              } else {
                _this12.IsLoading = false;
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              }
            });
          } else {
            this.accesorialFeeService.createAccessorialFee(feeModel).subscribe(function (response) {
              _this12.ServiceResponse = response;

              if (response != null && response.StatusCode == 0) {
                var message = "";

                if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
                  message = " created";
                } else if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                  message = " saved draft";
                }

                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                _this12.IsLoading = false;

                _this12.changeViewType(2);
              } else {
                _this12.IsLoading = false;
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              }
            });
          }
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.rcForm.get('TableName').patchValue([]);
          this.rcForm.get('TableTypes').patchValue([]);
          this.rcForm.get('SourceRegions').patchValue([]);
          this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
          this.rcForm.get('StartDate').patchValue([]);
          this.rcForm.get('EndDate').patchValue([]);
          this.rcForm.controls['Fees'].reset();
          this.disableInputControls = false;
          this.dataService.removeFeesOnCreateNewSubject();
        }
      }, {
        key: "onCancel",
        value: function onCancel() {
          if (this.AccessorialFeeMode == "VIEW") {
            this.disableInputControls = false;
            this.AccessorialFeeId = null;
          }

          if (this.AccessorialFeeMode == "EDIT") {
            this.AccessorialFeeId = null;
          }

          if (this.AccessorialFeeId != null) {
            this.changeToViewTab();
          } else {
            this._document.defaultView.location.reload();
          }
        }
      }, {
        key: "changeToViewTab",
        value: function changeToViewTab() {
          this.accesorialFeeService.onSelectedTabChanged.next(1);
        }
      }, {
        key: "removeValidators",
        value: function removeValidators(form) {
          for (var key in form.controls) {
            if (key == 'TableName') {
              continue;
            } else {
              form.get(key).clearValidators();
              form.get(key).updateValueAndValidity();
            }
          }
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(viewType) {
          this.onPageSubmit.emit(viewType);
        }
      }, {
        key: "createPostObject",
        value: function createPostObject(status) {
          var feeModel = new _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_3__["CreateAccessorialFeeModel"]();
          feeModel.Id = this.rcForm.get('AccessorialFeeId').value;
          feeModel.Name = this.rcForm.get('TableName').value;
          feeModel.Status = status;
          var selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;

          if (selectedTerminalBulkplant != null && selectedTerminalBulkplant != undefined && selectedTerminalBulkplant.length > 0) {
            feeModel.TerminalsAndBulkPlants = this.rcForm.get('TerminalsAndBulkPlants').value;
          }

          var selectedCustomers = this.rcForm.get('Customers').value;

          if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            selectedCustomers.forEach(function (t) {
              return feeModel.CustomerIds.push(t.Id);
            });
          }

          var selectedCarriers = this.rcForm.get('Carriers').value;

          if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            selectedCarriers.forEach(function (t) {
              return feeModel.CarrierIds.push(t.Id);
            });
          }

          var endDate = this.rcForm.get('EndDate').value;
          var startDate = this.rcForm.get('StartDate').value;

          if (endDate == "" || endDate == undefined || endDate == null) {
            endDate = null;
          }

          if (startDate == "" || startDate == undefined || startDate == null) {
            startDate = null;
          }

          feeModel.StartDate = startDate;
          feeModel.EndDate = endDate;
          feeModel.Fees = this.rcForm.get('Fees').value;
          var sourceRegions = this.rcForm.get('SourceRegions').value;

          if (sourceRegions != null && sourceRegions != undefined && sourceRegions.length > 0) {
            sourceRegions.forEach(function (t) {
              return feeModel.SourceRegionIds.push(t.Id);
            });
          }

          var tableType = this.rcForm.get('TableTypes').value;

          if (tableType != null && tableType != undefined && tableType.length > 0) {
            feeModel.TableType = tableType[0].Id;
          }

          feeModel.CountryId = this.SelectedCountryId;
          return feeModel;
        }
      }, {
        key: "getBulkPlantTerminalIds",
        value: function getBulkPlantTerminalIds(type) {
          var Ids = [];

          if (type === 'Terminals') {
            var selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;
            var selectedTerminals = selectedTerminalBulkplant.filter(function (t) {
              return t.Code === 'Terminals';
            });

            if (selectedTerminals != null && selectedTerminals != undefined && selectedTerminals.length > 0) {
              selectedTerminals.forEach(function (terminal) {
                var terminalId = parseInt(terminal.Id.replace("Terminals_", ""));

                if (!isNaN(terminalId)) {
                  Ids.push(terminalId);
                }
              });
            }
          } else if (type === 'BulkPlants') {
            var _selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;

            var selectedBulkPlants = _selectedTerminalBulkplant.filter(function (t) {
              return t.Code === 'BulkPlants';
            });

            if (selectedBulkPlants != null && selectedBulkPlants != undefined && selectedBulkPlants.length > 0) {
              selectedBulkPlants.forEach(function (bulkplant) {
                var bulkplantId = parseInt(bulkplant.Id.replace("BulkPlants_", ""));

                if (!isNaN(bulkplantId)) {
                  Ids.push(bulkplantId);
                }
              });
            }
          }

          return Ids;
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (this.AccessorialFeeId != null && this.AccessorialFeeId != undefined) {
            this.getAccessorialFee(this.AccessorialFeeId); //existing Accessorial Fee.
          }
        } //GET

      }, {
        key: "getAccessorialFee",
        value: function getAccessorialFee(accessorialFeeId) {
          var _this13 = this;

          this.IsLoading = true;
          this.IsLoaded = false;
          var sorceRegionIds = "";
          this.http.get(this.accesorialFeeService.getAccessorialFeeUrl + accessorialFeeId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["map"])(function (af) {
            var afModel = af;
            return afModel;
          }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["mergeMap"])(function (afModel) {
            _this13.Afmodel = afModel;
            var companyIds = [];

            if (_this13.AccessorialFeeId != null && _this13.AccessorialFeeMode.toUpperCase() == "COPY") {
              // on copy 
              _this13.Afmodel.Id = null;
              _this13.Afmodel.Name = "";
            }

            var customers = _this13.http.get(_this13.fuelsurchargeService.getSupplierCustomersUrl);

            var carriers = _this13.http.get(_this13.regionService.getCarriersUrl);

            if (_this13.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer && _this13.Afmodel.CustomerIds.length > 0) {
              _this13.Afmodel.CustomerIds.forEach(function (s) {
                return companyIds.push(s);
              });
            }

            if (_this13.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Carrier && _this13.Afmodel.CarrierIds.length > 0) {
              _this13.Afmodel.CarrierIds.forEach(function (s) {
                return companyIds.push(s);
              });
            }

            var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
            sourceRegionInput.TableType = _this13.Afmodel.TableType.toString();
            sourceRegionInput.CustomerId = _this13.Afmodel.CustomerIds;
            sourceRegionInput.CarrierId = _this13.Afmodel.CarrierIds;

            var sourceRegions = _this13.http.post(_this13.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);

            var tableTypes = _this13.http.get(_this13.fuelsurchargeService.getTableTypesUrl);

            if (_this13.Afmodel.SourceRegionIds != null && _this13.Afmodel.SourceRegionIds != undefined && _this13.Afmodel.SourceRegionIds.length > 0) {
              sorceRegionIds = _this13.Afmodel.SourceRegionIds.map(function (s) {
                return s;
              }).join(',');
              _this13.IsSourceRegionSelected = true;
            }

            var terminalAndBulkPlans = _this13.http.get(_this13.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds);

            return Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["forkJoin"])([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
          })).subscribe(function (result) {
            _this13.IsLoading = false;
            _this13.IsMasterSelected = false;
            _this13.IsCustomerSelected = false;
            _this13.IsCarrierSelected = false;

            if (_this13.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master) {
              _this13.IsMasterSelected = true;
            } else if (_this13.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer) {
              _this13.IsCustomerSelected = true;
            } else if (_this13.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Carrier) {
              _this13.IsCarrierSelected = true;
            }

            if (_this13.Afmodel.TableType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master) {
              _this13.CustomerList = result[0];
              _this13.CarrierList = result[1];
            }

            _this13.SourceRegionList = result[2];

            if (_this13.Afmodel.TerminalsAndBulkPlants != null && _this13.Afmodel.TerminalsAndBulkPlants != undefined && _this13.Afmodel.TerminalsAndBulkPlants.length > 0) {
              _this13.TerminalsAndBulkPlantList = result[3];
            }

            _this13.TableTypeList = result[4];

            _this13.Edit(_this13.Afmodel);
          });
        } //Edit

      }, {
        key: "Edit",
        value: function Edit(_af) {
          if (this.rcForm) {
            if (this.AccessorialFeeMode != "COPY") {
              this.rcForm.controls['AccessorialFeeId'].setValue(_af.Id);
              this.rcForm.controls['TableTypes'].setValue(_af.TableType);
              this.rcForm.controls['TableName'].setValue(_af.Name);
              this.IsEditable = false;
            } else {
              this.AccessorialFeeId = null;
            }

            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(function (x) {
              return x.Id == _af.TableType;
            }));
            if (_af.TableType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master) this.IsMasterSelected = false;

            if (_af.CustomerIds != null && _af.CustomerIds != undefined && _af.CustomerIds.length > 0) {
              if (this.CustomerList.length > 0 && _af.CustomerIds.length > 0) this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_af.CustomerIds)));
            }

            if (_af.CarrierIds != null && _af.CarrierIds != undefined && _af.CarrierIds.length > 0) {
              if (this.CarrierList.length > 0 && _af.CarrierIds.length > 0) this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_af.CarrierIds)));
            }

            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _af.SourceRegionIds != null && _af.SourceRegionIds != undefined && _af.SourceRegionIds.length > 0) {
              if (this.SourceRegionList.length > 0 && _af.SourceRegionIds.length > 0) this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_af.SourceRegionIds)));
            }

            if (this.TerminalsAndBulkPlantList != null && this.TerminalsAndBulkPlantList != undefined && _af.TerminalsAndBulkPlants != null && _af.TerminalsAndBulkPlants != undefined && _af.TerminalsAndBulkPlants.length > 0) {
              if (this.TerminalsAndBulkPlantList.length > 0 && _af.TerminalsAndBulkPlants.length > 0) {
                this.rcForm.controls['TerminalsAndBulkPlants'].setValue(this.TerminalsAndBulkPlantList.filter(this.ComparerWithId(_af.TerminalsAndBulkPlants)));
              }
            }

            this.rcForm.get('StartDate').setValue(moment__WEBPACK_IMPORTED_MODULE_4__(_af.StartDate).format('MM/DD/YYYY'));

            if (_af.EndDate != null && _af.EndDate != undefined) {
              this.rcForm.get('EndDate').setValue(moment__WEBPACK_IMPORTED_MODULE_4__(_af.EndDate).format('MM/DD/YYYY'));
            }

            this.Fees = _af.Fees;
            this.rcForm.controls['StatusId'].setValue(_af.Status);
            this.IsLoading = false;
            this.IsLoaded = true;
          }

          if (this.AccessorialFeeMode == "VIEW") {
            this.disableInputControls = true;
          }
        }
      }, {
        key: "IdInComparer",
        value: function IdInComparer(otherArray) {
          return function (current) {
            return otherArray.filter(function (other) {
              return other == current.Id;
            }).length == 1;
          };
        }
      }, {
        key: "ComparerWithId",
        value: function ComparerWithId(otherArray) {
          return function (current) {
            return otherArray.filter(function (other) {
              //console.log(other + " : " + current.Id);
              return other.Id == current.Id;
            }).length == 1;
          };
        }
      }]);

      return CreateAccessorialFeesComponent;
    }();

    CreateAccessorialFeesComponent.??fac = function CreateAccessorialFeesComponent_Factory(t) {
      return new (t || CreateAccessorialFeesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__["DataService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__["AccessorialFeesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_15__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_common__WEBPACK_IMPORTED_MODULE_7__["DOCUMENT"]));
    };

    CreateAccessorialFeesComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: CreateAccessorialFeesComponent,
      selectors: [["app-create-accessorial-fees"]],
      outputs: {
        onPageSubmit: "onPageSubmit"
      },
      decls: 69,
      vars: 46,
      consts: [[3, "formGroup", "ngSubmit"], [4, "ngIf"], [3, "ngClass", "disabled"], [1, "well", "bg-white"], [1, "row"], [1, "col-sm-3", "form-group"], [1, "color-maroon"], ["type", "text", "formControlName", "TableName", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "settings", "data", "placeholder", "onSelect"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "settings", "data", "placeholder", "onDeSelect", "onDeSelectAll"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "formControlName", "StartDate", "placeholder", "Effective Start Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], ["type", "text", "formControlName", "EndDate", "placeholder", "Effective End Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], [1, "col-sm-12"], [3, "Parent", "CountryId", "Fees"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "button", "value", "Draft", 1, "btn", "btn-lg", "btn-light", 3, "disabled", "click"], ["type", "button", "value", "Submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled", "click"], ["class", "loader", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CreateAccessorialFeesComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngSubmit", function CreateAccessorialFeesComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, CreateAccessorialFeesComponent_div_2_Template, 4, 0, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "fieldset", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "Table Name ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](11, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](12, CreateAccessorialFeesComponent_div_12_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16, "Table Type ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_19_listener($event) {
            return ctx.onTableTypeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, CreateAccessorialFeesComponent_div_20_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "label", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Select Customer(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "ng-multiselect-dropdown", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_26_listener($event) {
            return ctx.onCustomersSelect($event);
          })("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_26_listener($event) {
            return ctx.onCustomersDeSelect($event);
          })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_26_listener($event) {
            return ctx.onCustomersDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](27, CreateAccessorialFeesComponent_div_27_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "label", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Select Carrier(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "ng-multiselect-dropdown", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_33_listener($event) {
            return ctx.onCarriersSelect($event);
          })("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_33_listener($event) {
            return ctx.onCarriersDeSelect($event);
          })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_33_listener($event) {
            return ctx.onCarriersDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](34, CreateAccessorialFeesComponent_div_34_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Select Source Region(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "ng-multiselect-dropdown", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_41_listener($event) {
            return ctx.onSourceRegionsDeSelect($event);
          })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_41_listener($event) {
            return ctx.onSourceRegionsDeSelectAll($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](42, CreateAccessorialFeesComponent_div_42_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "label", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](47, "Select Terminal(s)/BulkPlant(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](48, "angular2-multiselect", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](51, "Effective Start Date ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](52, "span", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](53, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "input", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateAccessorialFeesComponent_Template_input_onDateChange_54_listener($event) {
            return ctx.rcForm.get("StartDate").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](55, CreateAccessorialFeesComponent_div_55_Template, 2, 1, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](58, "Effective End Date ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "input", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function CreateAccessorialFeesComponent_Template_input_onDateChange_59_listener($event) {
            return ctx.rcForm.get("EndDate").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](62, "app-fee-list", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateAccessorialFeesComponent_Template_input_click_65_listener() {
            return ctx.onCancel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "input", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateAccessorialFeesComponent_Template_input_click_66_listener() {
            return ctx.onSubmit(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "input", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function CreateAccessorialFeesComponent_Template_input_click_67_listener() {
            return ctx.onSubmit(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](68, CreateAccessorialFeesComponent_div_68_Template, 5, 0, "div", 30);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.rcForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.AccessorialFeeMode != "CREATE");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](38, _c0, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("TableName").invalid && ctx.rcForm.get("TableName").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx.SingleSelectSettingsById)("data", ctx.TableTypeList)("placeholder", "Select Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("TableTypes").invalid && ctx.rcForm.get("TableTypes").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](40, _c1, ctx.IsMasterSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx.MultiSelectSettingsById)("data", ctx.CustomerList)("placeholder", "Select Customers");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Customers").invalid && ctx.rcForm.get("Customers").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](42, _c1, ctx.IsMasterSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx.MultiSelectSettingsById)("data", ctx.CarrierList)("placeholder", "Select Carriers");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsCarrierSelected && ctx.rcForm.get("Carriers").invalid && ctx.rcForm.get("Carriers").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx.MultiSelectSettingsById)("data", ctx.SourceRegionList)("placeholder", "Select Source Regions");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("SourceRegions").invalid && ctx.rcForm.get("SourceRegions").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](44, _c1, !ctx.IsSourceRegionSelected));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx.TerminalsAndBulkPlantList)("settings", ctx.MultiSelectSettingsByGroup);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("maxDate", ctx.maxDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.rcForm.get("StartDate").invalid && ctx.rcForm.get("StartDate").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("minDate", ctx.minDate)("maxDate", ctx.maxDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("Parent", ctx.rcForm)("CountryId", ctx.SelectedCountryId)("Fees", ctx.Fees);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.disableInputControls ? true : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["MultiSelectComponent"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["AngularMultiSelect"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__["DatePicker"], _child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_19__["FeeListComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NyZWF0ZS1hY2Nlc3NvcmlhbC1mZWVzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CreateAccessorialFeesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-create-accessorial-fees',
          templateUrl: './create-accessorial-fees.component.html',
          styleUrls: ['./create-accessorial-fees.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }, {
          type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"]
        }, {
          type: _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__["DataService"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__["CarrierService"]
        }, {
          type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__["AccessorialFeesService"]
        }, {
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_15__["HttpClient"]
        }, {
          type: Document,
          decorators: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Inject"],
            args: [_angular_common__WEBPACK_IMPORTED_MODULE_7__["DOCUMENT"]]
          }]
        }];
      }, {
        onPageSubmit: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/accessorial-fees/master/master.component.ts": function srcAppAccessorialFeesMasterMasterComponentTs(module, __webpack_exports__, __webpack_require__) {
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


    var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../service/accessorialfees.service */
    "./src/app/accessorial-fees/service/accessorialfees.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../create/create-accessorial-fees.component */
    "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts");
    /* harmony import */


    var _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../view/view-accessorial-fees.component */
    "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts");

    function MasterComponent_app_create_accessorial_fees_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "app-create-accessorial-fees", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onPageSubmit", function MasterComponent_app_create_accessorial_fees_12_Template_app_create_accessorial_fees_onPageSubmit_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r3);

          var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();

          return ctx_r2.onCreateFees($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
      }
    }

    function MasterComponent_app_view_accessorial_fees_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-view-accessorial-fees");
      }
    }

    var MasterComponent = /*#__PURE__*/function () {
      function MasterComponent(accessorialFeeService) {
        _classCallCheck(this, MasterComponent);

        this.accessorialFeeService = accessorialFeeService;
        this.viewType = 0;
      }

      _createClass(MasterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this14 = this;

          var _viewType = localStorage.getItem("fuelSurchargeTabId");

          if (_viewType && +_viewType > 0) {
            this.viewType = +_viewType;
          }

          this.accessorialFeeService.onSelectedTabChanged.subscribe(function (s) {
            if (s == 2) {
              _this14.viewType = 2;
            } else {
              _this14.viewType = 1;
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
          this.accessorialFeeService.onSelectedAccessorialFeeId.next(null);
          this.accessorialFeeService.onSelectedTabChanged.next(value);
        }
      }, {
        key: "onCreateFees",
        value: function onCreateFees(viewType) {
          this.changeViewType(viewType);
        }
      }]);

      return MasterComponent;
    }();

    MasterComponent.??fac = function MasterComponent_Factory(t) {
      return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__["AccessorialFeesService"]));
    };

    MasterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: MasterComponent,
      selectors: [["app-master"]],
      decls: 14,
      vars: 8,
      consts: [[1, "row"], [1, "col-sm-4"], [1, "d-inline-block", "border", "bg-white", "p-1", "radius-capsule", "shadow-b", "mb-2"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [1, "col-sm-12"], [3, "onPageSubmit", 4, "ngIf"], [4, "ngIf"], [3, "onPageSubmit"]],
      template: function MasterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_5_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Create Accessorial Fees");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_8_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "View Accessorial Fees");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, MasterComponent_app_create_accessorial_fees_12_Template, 1, 0, "app-create-accessorial-fees", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, MasterComponent_app_view_accessorial_fees_13_Template, 1, 0, "app-view-accessorial-fees", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 1)("checked", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "type")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 2);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"], _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["CreateAccessorialFeesComponent"], _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_4__["ViewAccessorialFeesComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvbWFzdGVyL21hc3Rlci5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-master',
          templateUrl: './master.component.html',
          styleUrls: ['./master.component.css']
        }]
      }], function () {
        return [{
          type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__["AccessorialFeesService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/accessorial-fees/model/accessorial-fees.ts": function srcAppAccessorialFeesModelAccessorialFeesTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewAccessorialFeeModel", function () {
      return ViewAccessorialFeeModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AccessorialFeeInputModel", function () {
      return AccessorialFeeInputModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AccessorialFeeGridModel", function () {
      return AccessorialFeeGridModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateAccessorialFeeModel", function () {
      return CreateAccessorialFeeModel;
    });

    var ViewAccessorialFeeModel = function ViewAccessorialFeeModel() {
      _classCallCheck(this, ViewAccessorialFeeModel);

      this.TableTypes = [];
      this.Customers = [];
      this.Carriers = [];
      this.SourceRegions = [];
      this.TerminalsAndBulkPlants = [];
    };

    var AccessorialFeeInputModel = function AccessorialFeeInputModel() {
      _classCallCheck(this, AccessorialFeeInputModel);
    };

    var AccessorialFeeGridModel = function AccessorialFeeGridModel() {
      _classCallCheck(this, AccessorialFeeGridModel);
    };

    var CreateAccessorialFeeModel = function CreateAccessorialFeeModel() {
      _classCallCheck(this, CreateAccessorialFeeModel);

      this.CustomerIds = [];
      this.CarrierIds = [];
      this.SourceRegionIds = [];
      this.TerminalsAndBulkPlants = [];
      this.Fees = [];
      this.CustomerIds = [];
      this.CarrierIds = [];
      this.SourceRegionIds = [];
      this.Fees = [];
    };
    /***/

  },

  /***/
  "./src/app/accessorial-fees/service/accessorialfees.service.ts": function srcAppAccessorialFeesServiceAccessorialfeesServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AccessorialFeesService", function () {
      return AccessorialFeesService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var AccessorialFeesService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(AccessorialFeesService, _src_app_errors_Handl);

      var _super = _createSuper(AccessorialFeesService);

      function AccessorialFeesService(httpClient) {
        var _this15;

        _classCallCheck(this, AccessorialFeesService);

        _this15 = _super.call(this);
        _this15.httpClient = httpClient;
        _this15.archiveAccessorialFeeUrl = '/AccessorialFees/ArchiveAccessorialFee';
        _this15.getViewAccessorialFeeSummaryUrl = '/AccessorialFees/GetViewAccessorialFeeSummary';
        _this15.getAccessorialFeeUrl = '/AccessorialFees/GetAccessorialFee?accessorialFeeId=';
        _this15.postCreateAccesorialFeesUrl = '/AccessorialFees/CreateAccessorialFee';
        _this15.postUpdateAccesorialFeesUrl = '/AccessorialFees/UpdateAccessorialFee';
        _this15.onSelectedTabChanged = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1);
        _this15.onSelectedAccessorialFeeId = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](null);
        return _this15;
      }

      _createClass(AccessorialFeesService, [{
        key: "getAccessorialFeeGridDetails",
        value: function getAccessorialFeeGridDetails(filter) {
          return this.httpClient.post(this.getViewAccessorialFeeSummaryUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAccessorialFeeGridDetails', null)));
        }
      }, {
        key: "archiveAccessorialFee",
        value: function archiveAccessorialFee(accessorialFeeId) {
          return this.httpClient.post(this.archiveAccessorialFeeUrl, {
            accessorialFeeId: accessorialFeeId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('archiveAccessorialFee', null)));
        }
      }, {
        key: "createAccessorialFee",
        value: function createAccessorialFee(accessorialFeeModel) {
          return this.httpClient.post(this.postCreateAccesorialFeesUrl, {
            model: accessorialFeeModel
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('createAccessorialFee', null)));
        }
      }, {
        key: "updateAccessorialFee",
        value: function updateAccessorialFee(accessorialFeeModel) {
          return this.httpClient.post(this.postUpdateAccesorialFeesUrl, {
            model: accessorialFeeModel
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('updateAccessorialFee', null)));
        }
      }, {
        key: "getAccessorialFee",
        value: function getAccessorialFee(accessorialFeeId) {
          return this.httpClient.get(this.getAccessorialFeeUrl + accessorialFeeId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAccessorialFee', null)));
        }
      }]);

      return AccessorialFeesService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    AccessorialFeesService.??fac = function AccessorialFeesService_Factory(t) {
      return new (t || AccessorialFeesService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    AccessorialFeesService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({
      token: AccessorialFeesService,
      factory: AccessorialFeesService.??fac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](AccessorialFeesService, [{
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
  "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts": function srcAppAccessorialFeesViewViewAccessorialFeesComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewAccessorialFeesComponent", function () {
      return ViewAccessorialFeesComponent;
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


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/my.localstorage */
    "./src/app/my.localstorage.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../model/accessorial-fees */
    "./src/app/accessorial-fees/model/accessorial-fees.ts");
    /* harmony import */


    var _view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./view-fees-details/view-fees-details.component */
    "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! src/app/fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../service/accessorialfees.service */
    "./src/app/accessorial-fees/service/accessorialfees.service.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! angular2-multiselect-dropdown */
    "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    var _c0 = function _c0(a0) {
      return {
        "d-block": a0
      };
    };

    function ViewAccessorialFeesComponent_tr_38_td_15_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipe"](2, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2).$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](6, _c0, !fee_r5.IsShowMore));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pipeBind3"](2, 2, fee_r5.Terminal, 0, 40), "...");
      }
    }

    function ViewAccessorialFeesComponent_tr_38_td_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, ViewAccessorialFeesComponent_tr_38_td_15_div_3_Template, 3, 8, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_tr_38_td_15_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r16);

          var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          return fee_r5.IsShowMore = !fee_r5.IsShowMore;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "View More/Less");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](3, _c0, fee_r5.IsShowMore));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.Terminal);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", fee_r5.Terminal.length > 40);
      }
    }

    function ViewAccessorialFeesComponent_tr_38_td_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.Terminal);
      }
    }

    function ViewAccessorialFeesComponent_tr_38_a_20_Template(rf, ctx) {
      if (rf & 1) {
        var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("cancel", function ViewAccessorialFeesComponent_tr_38_a_20_Template_a_cancel_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r20);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

          return ctx_r19.cancelClicked = true;
        })("confirm", function ViewAccessorialFeesComponent_tr_38_a_20_Template_a_confirm_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r20);

          var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r21.archiveAccessorialFee(fee_r5.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("popoverTitle", ctx_r9.popoverTitle)("popoverMessage", ctx_r9.popoverMessage);
      }
    }

    function ViewAccessorialFeesComponent_tr_38_a_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_tr_38_a_21_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r25);

          var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r23.viewAccessorialFee(fee_r5.Id, "EDIT");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function ViewAccessorialFeesComponent_tr_38_a_24_Template(rf, ctx) {
      if (rf & 1) {
        var _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "a", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_tr_38_a_24_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r28);

          var fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]().$implicit;

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r26.viewAccessorialFee(fee_r5.Id, "COPY");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](1, "i", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function ViewAccessorialFeesComponent_tr_38_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td", 30);

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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, ViewAccessorialFeesComponent_tr_38_td_15_Template, 6, 5, "td", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, ViewAccessorialFeesComponent_tr_38_td_16_Template, 2, 1, "td", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "td", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](20, ViewAccessorialFeesComponent_tr_38_a_20_Template, 2, 2, "a", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](21, ViewAccessorialFeesComponent_tr_38_a_21_Template, 2, 0, "a", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "a", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_tr_38_Template_a_click_22_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r30);

          var fee_r5 = ctx.$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r29.viewAccessorialFee(fee_r5.Id, "VIEW");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](23, "i", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, ViewAccessorialFeesComponent_tr_38_a_24_Template, 2, 0, "a", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var fee_r5 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.DateRange);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.TableType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.TableName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.StatusName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.Customer);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.Carrier);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.SourceRegion);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", fee_r5.Terminal.length > 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", fee_r5.Terminal.length <= 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](fee_r5.BulkPlant);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !fee_r5.IsArchived);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !fee_r5.IsArchived);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !fee_r5.IsArchived);
      }
    }

    function ViewAccessorialFeesComponent_div_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function ViewAccessorialFeesComponent_ng_template_49_div_7_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Table Type is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }
    }

    function ViewAccessorialFeesComponent_ng_template_49_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, ViewAccessorialFeesComponent_ng_template_49_div_7_div_1_Template, 2, 0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r31.viewAccessorialFeeForm.get("TableTypes").errors.required);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    function ViewAccessorialFeesComponent_ng_template_49_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "label", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](5, "Table Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "ng-multiselect-dropdown", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_6_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r33.onTableTypeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](7, ViewAccessorialFeesComponent_ng_template_49_div_7_Template, 2, 1, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "label", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Customer(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "ng-multiselect-dropdown", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r35.onCustomersSelect($event);
        })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r36.onCustomersDeSelect($event);
        })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r37.onCustomersDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "label", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Carrier(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "ng-multiselect-dropdown", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r38.onCarriersSelect($event);
        })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r39.onCarriersDeSelect($event);
        })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r40.onCarriersDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "label", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Source Region(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "ng-multiselect-dropdown", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r41.onSourceRegionsSelect($event);
        })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r42.onSourceRegionsDeSelect($event);
        })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r43.onSourceRegionsDeSelectAll($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "label", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Terminal(s)/BulkPlant(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](29, "angular2-multiselect", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](33, "From");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "input", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function ViewAccessorialFeesComponent_ng_template_49_Template_input_onDateChange_34_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r44.setfromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "To");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "input", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function ViewAccessorialFeesComponent_ng_template_49_Template_input_onDateChange_39_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r45.settoDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](43, "input", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "label", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](45, " Show Archived ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "button", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_ng_template_49_Template_button_click_47_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          return ctx_r46.clearFilter();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](48, "Clear Filter");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "button", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_ng_template_49_Template_button_click_49_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r34);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

          var _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4);

          ctx_r47.filterGrid();
          return _r0.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](50, "Apply");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.TableTypeList)("placeholder", "Select Type (Required)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r4.viewAccessorialFeeForm.get("TableTypes").invalid && ctx_r4.viewAccessorialFeeForm.get("TableTypes").touched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](22, _c1, ctx_r4.IsMasterSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CustomerList)("placeholder", "Select Customers");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](24, _c1, ctx_r4.IsMasterSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CarrierList)("placeholder", "Select Carriers");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r4.SourceRegionList)("settings", ctx_r4.MultiselectSettingsById)("placeholder", "Select Source Regions");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("data", ctx_r4.TerminalsAndBulkPlantList)("settings", ctx_r4.MultiSelectSettingsByGroup);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("minDate", ctx_r4.minDate)("maxDate", ctx_r4.maxDate)("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](26, _c1, ctx_r4.IsLoading));
      }
    }

    var ViewAccessorialFeesComponent = /*#__PURE__*/function () {
      function ViewAccessorialFeesComponent(fb, regionService, fuelsurchargeService, accessorialFeeService, cdr) {
        _classCallCheck(this, ViewAccessorialFeesComponent);

        this.fb = fb;
        this.regionService = regionService;
        this.fuelsurchargeService = fuelsurchargeService;
        this.accessorialFeeService = accessorialFeeService;
        this.cdr = cdr;
        this.IsLoading = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.SinlgeselectSettingsById = {};
        this.MultiSelectSettingsByGroup = {};
        this.TerminalsAndBulkPlantList = [];
        this.SelectedTerminalsAndBulkPlants = [];
        this.IsCustomerSelected = false;
        this.IsMasterSelected = false;
        this.IsCarrierSelected = false;
        this.IsSourceRegionSelected = false;
        this.AccessorialFeeList = [];
        this.minDate = new Date();
        this.maxDate = new Date();
        this.popoverTitle = 'Archive Confirmation';
        this.popoverMessage = 'Do you want to archive?';
        this.cancelClicked = false;
        this.confirmClicked = false;
      }

      _createClass(ViewAccessorialFeesComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
          this.minDate.setFullYear(this.minDate.getFullYear() - 20);
          this.CounrtyId = src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_4__["MyLocalStorage"].getData("countryIdForDashboard");
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
          this.viewAccessorialFeeForm = this.createForm();
          this.initializeAccessorialFeeDatatableGrid();
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.rerender_destroy();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getAccessorialFeeGridDetails();
        }
      }, {
        key: "createForm",
        value: function createForm() {
          if (this.Afmodel == undefined || this.Afmodel == null) {
            this.Afmodel = new _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_6__["ViewAccessorialFeeModel"]();
          }

          return this.fb.group({
            TableTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](this.Afmodel.TableTypes, [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            Customers: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](this.Afmodel.Customers),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](this.Afmodel.Carriers),
            SourceRegions: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](this.Afmodel.SourceRegions),
            TerminalsAndBulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControl"](this.SelectedTerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
          });
        }
      }, {
        key: "archiveAccessorialFee",
        value: function archiveAccessorialFee(accessorialFeeId) {
          var _this16 = this;

          this.IsLoading = true;
          this.accessorialFeeService.archiveAccessorialFee(accessorialFeeId).subscribe(function (response) {
            _this16.IsLoading = false; //this.serviceResponse = response;

            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess('Selected accessorial fee archived successfully', undefined, undefined);

              _this16.filterGrid();
            }
          });
        }
      }, {
        key: "onTableTypeSelect",
        value: function onTableTypeSelect(item) {
          this.IsMasterSelected = false;
          this.IsCustomerSelected = false;
          this.IsCarrierSelected = false;
          this.viewAccessorialFeeForm.get('Carriers').patchValue([]);
          this.viewAccessorialFeeForm.get('Customers').patchValue([]);

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

          this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
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
          this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
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
          this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
          this.onCarriersOrCustomersChange();
        }
      }, {
        key: "onCarriersOrCustomersChange",
        value: function onCarriersOrCustomersChange() {
          var selectedTableType = this.viewAccessorialFeeForm.get('TableTypes').value;
          this.getSourceRegions(selectedTableType[0].Id.toString());
        }
      }, {
        key: "getTableTypes",
        value: function getTableTypes() {
          var _this17 = this;

          this.IsLoading = true;
          this.fuelsurchargeService.getTableTypes().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this17, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee7() {
              return regeneratorRuntime.wrap(function _callee7$(_context7) {
                while (1) {
                  switch (_context7.prev = _context7.next) {
                    case 0:
                      _context7.next = 2;
                      return data;

                    case 2:
                      this.TableTypeList = _context7.sent;

                    case 3:
                    case "end":
                      return _context7.stop();
                  }
                }
              }, _callee7, this);
            }));
          });
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this18 = this;

          this.IsLoading = true;
          this.regionService.getCarriers().subscribe(function (carriers) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this18, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee8() {
              return regeneratorRuntime.wrap(function _callee8$(_context8) {
                while (1) {
                  switch (_context8.prev = _context8.next) {
                    case 0:
                      _context8.next = 2;
                      return carriers;

                    case 2:
                      this.CarrierList = _context8.sent;
                      this.SourceRegionList = null;
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
        key: "getSupplierCustomers",
        value: function getSupplierCustomers() {
          var _this19 = this;

          this.IsLoading = true;
          this.fuelsurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this19, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee9() {
              return regeneratorRuntime.wrap(function _callee9$(_context9) {
                while (1) {
                  switch (_context9.prev = _context9.next) {
                    case 0:
                      _context9.next = 2;
                      return data;

                    case 2:
                      this.CustomerList = _context9.sent;
                      this.IsLoading = false;

                    case 4:
                    case "end":
                      return _context9.stop();
                  }
                }
              }, _callee9, this);
            }));
          });
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions(tableType) {
          var _this20 = this;

          this.IsLoading = true;
          var customerIds = [];
          var carrierIds = [];
          var selectedCustomers = this.viewAccessorialFeeForm.get('Customers').value;

          if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(function (s) {
              return s.Id;
            });
          }

          var selectedCarriers = this.viewAccessorialFeeForm.get('Carriers').value;

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
            _this20.SourceRegionList = data;
            _this20.IsLoading = false;
          });
        }
      }, {
        key: "getTerminalsBulkPlant",
        value: function getTerminalsBulkPlant() {
          var _this21 = this;

          this.IsLoading = true;
          var selectedSourceRegions = this.viewAccessorialFeeForm.get('SourceRegions').value;

          if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(function (s) {
              return s.Id;
            }).join(',')).subscribe(function (data) {
              return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this21, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee10() {
                return regeneratorRuntime.wrap(function _callee10$(_context10) {
                  while (1) {
                    switch (_context10.prev = _context10.next) {
                      case 0:
                        _context10.next = 2;
                        return data;

                      case 2:
                        this.TerminalsAndBulkPlantList = _context10.sent;
                        this.IsLoading = false;

                      case 4:
                      case "end":
                        return _context10.stop();
                    }
                  }
                }, _callee10, this);
              }));
            });
          }
        }
      }, {
        key: "onSourceRegionsDeSelect",
        value: function onSourceRegionsDeSelect(item) {
          this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
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
          this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
          this.getAccessorialFeeGridDetails();
        }
      }, {
        key: "clearFilter",
        value: function clearFilter() {
          this.clearForm();
          this.getAccessorialFeeGridDetails();
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.viewAccessorialFeeForm.reset();
          $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
          this.CustomerList = [];
          this.CarrierList = [];
          this.SourceRegionList = [];
        }
      }, {
        key: "getAccessorialFeeGridDetails",
        value: function getAccessorialFeeGridDetails() {
          var _this22 = this;

          this.cdr.detectChanges();
          var input = new _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_6__["AccessorialFeeInputModel"]();
          var selectedTableTypes = this.viewAccessorialFeeForm.get('TableTypes').value;
          var selectedCustomers = this.viewAccessorialFeeForm.get('Customers').value;
          var selectedCarriers = this.viewAccessorialFeeForm.get('Carriers').value;
          var selectedSourceRegions = this.viewAccessorialFeeForm.get('SourceRegions').value;
          var selectedTerminalAndBulkPlants = this.viewAccessorialFeeForm.get('TerminalsAndBulkPlants').value;
          input.StartDate = this.viewAccessorialFeeForm.controls.fromDate.value;
          input.EndDate = this.viewAccessorialFeeForm.controls.toDate.value;
          input.IsArchived = this.viewAccessorialFeeForm.controls.isArchived.value;

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
          this.accessorialFeeService.getAccessorialFeeGridDetails(input).subscribe(function (data) {
            _this22.IsLoading = false;

            if (data && data.length > 0) {
              _this22.AccessorialFeeList = data;
            }

            _this22.dtTrigger.next();
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
          var _this23 = this;

          if (this.datatableElement && this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              _this23.dtTrigger.next();
            });
          }
        }
      }, {
        key: "viewAccessorialFee",
        value: function viewAccessorialFee(accessorialFeeId, mode) {
          var operation = {
            AccessorialFeeId: accessorialFeeId,
            Mode: mode
          };
          this.accessorialFeeService.onSelectedAccessorialFeeId.next(JSON.stringify(operation));
          this.accessorialFeeService.onSelectedTabChanged.next(1);
        }
      }, {
        key: "setfromDate",
        value: function setfromDate($event) {
          this.viewAccessorialFeeForm.controls.fromDate.setValue($event);
        }
      }, {
        key: "settoDate",
        value: function settoDate($event) {
          this.viewAccessorialFeeForm.controls.toDate.setValue($event);
        }
      }, {
        key: "initializeAccessorialFeeDatatableGrid",
        value: function initializeAccessorialFeeDatatableGrid() {
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
              title: 'Accessorial Fee',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Accessorial Fee',
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

      return ViewAccessorialFeesComponent;
    }();

    ViewAccessorialFeesComponent.??fac = function ViewAccessorialFeesComponent_Factory(t) {
      return new (t || ViewAccessorialFeesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__["AccessorialFeesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]));
    };

    ViewAccessorialFeesComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({
      type: ViewAccessorialFeesComponent,
      selectors: [["app-view-accessorial-fees"]],
      viewQuery: function ViewAccessorialFeesComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.accessorialFeeComponent = _t.first);
        }
      },
      decls: 51,
      vars: 7,
      consts: [[3, "formGroup"], [1, "row"], [1, "col-sm-12", "text-left"], ["placement", "bottom", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs16", "mr10", "filter-link", "pa", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-accessorial-fee-grid", 1, "table-responsive"], ["id", "accessorial-fee-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "DateRange"], ["data-key", "TableType"], ["data-key", "TableName"], ["data-key", "StatusName"], ["data-key", "Customer"], ["data-key", "Carrier"], ["data-key", "SourceRegion"], ["data-key", "Terminal"], ["data-key", "BulkPlant"], [4, "ngFor", "ngForOf"], ["id", "fee-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], ["class", "loader", 4, "ngIf"], ["popContent", ""], [1, "text-center"], [4, "ngIf"], [1, "text-center", "text-nowrap"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-street-view"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Copy", 3, "click", 4, "ngIf"], [1, "d-none", 3, "ngClass"], ["class", "d-none", 3, "ngClass", 4, "ngIf"], [3, "click"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-trash-alt", "color-maroon"], ["placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], ["placement", "bottom", "ngbTooltip", "Copy", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-copy"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "popover-details"], [1, "col-sm-6"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 3, "settings", "data", "placeholder", "onSelect"], ["class", "color-maroon", 4, "ngIf"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "data", "settings", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], [1, "col-sm-12"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "placeholder", "Select Date", "formControlName", "fromDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Select Date", "formControlName", "toDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-6", "form-group"], [1, "form-check"], ["formControlName", "isArchived", "type", "checkbox", "value", "", "id", "ckb-isArchived", 1, "form-check-input"], ["for", "ckb-isArchived", 1, "form-check-label"], [1, "col-sm-6", "text-right", "form-buttons", "mt20"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], ["id", "filter-accessorial-fee-grid", "type", "button", 1, "btn", "btn-lg", "btn-primary", "mt3", "valid", 3, "ngClass", "click"], [1, "color-maroon"]],
      template: function ViewAccessorialFeesComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "a", 3, 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function ViewAccessorialFeesComponent_Template_a_click_3_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r48);

            var _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](4);

            return _r0.open();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6, "Filters");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Date Range");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "Table Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Table Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Customer(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "Carrier(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30, "Source Region(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32, "Terminal(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "Bulk Plant(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, ViewAccessorialFeesComponent_tr_38_Template, 25, 13, "tr", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "a", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](43, "i", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](44, "h3", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](45, "Fee Details");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](47, "app-view-fees-details");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](48, ViewAccessorialFeesComponent_div_48_Template, 5, 0, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](49, ViewAccessorialFeesComponent_ng_template_49_Template, 51, 28, "ng-template", null, 29, _angular_core__WEBPACK_IMPORTED_MODULE_1__["????templateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        }

        if (rf & 2) {
          var _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????reference"](50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx.viewAccessorialFeeForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngbPopover", _r3)("autoClose", "outside");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.AccessorialFeeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__["NgbPopover"], angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgForOf"], _view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__["??c"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["CheckboxControlValueAccessor"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_14__["SlicePipe"]],
      styles: [".master-filter.popover {\r\n    min-width: 425px;\r\n    max-width: 450px;\r\n    background: #F9F9F9;\r\n    border: 1px solid #E9E7E7;\r\n    box-sizing: border-box;\r\n    box-shadow: 10px 10px 8px -2px rgb(0, 0, 0, 0.13);\r\n    border-radius: 10px;\r\n}\r\n\r\n      .master-filter.popover .popover-body {\r\n        padding: 0;\r\n        border-radius: 5px;\r\n        background: #ffffff;\r\n    }\r\n\r\n      .master-filter.popover .popover-details {\r\n        padding: 15px;\r\n    }\r\n\r\n      .master-filter.popover .border-bottom-2 {\r\n        border-bottom: 2px solid #e7eaec !important;\r\n    }\r\n\r\n    .filter-link[_ngcontent-%COMP%] {\r\n    top: -45px;\r\n    left: 380px\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYWNjZXNzb3JpYWwtZmVlcy92aWV3L3ZpZXctYWNjZXNzb3JpYWwtZmVlcy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksZ0JBQWdCO0lBQ2hCLGdCQUFnQjtJQUNoQixtQkFBbUI7SUFDbkIseUJBQXlCO0lBQ3pCLHNCQUFzQjtJQUN0QixpREFBaUQ7SUFDakQsbUJBQW1CO0FBQ3ZCOztJQUVJO1FBQ0ksVUFBVTtRQUNWLGtCQUFrQjtRQUNsQixtQkFBbUI7SUFDdkI7O0lBRUE7UUFDSSxhQUFhO0lBQ2pCOztJQUVBO1FBQ0ksMkNBQTJDO0lBQy9DOztJQUVKO0lBQ0ksVUFBVTtJQUNWO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9hY2Nlc3NvcmlhbC1mZWVzL3ZpZXcvdmlldy1hY2Nlc3NvcmlhbC1mZWVzLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyI6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciB7XHJcbiAgICBtaW4td2lkdGg6IDQyNXB4O1xyXG4gICAgbWF4LXdpZHRoOiA0NTBweDtcclxuICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xyXG4gICAgYm94LXNpemluZzogYm9yZGVyLWJveDtcclxuICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG59XHJcblxyXG4gICAgOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIH1cclxuXHJcbiAgICA6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgfVxyXG5cclxuICAgIDo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICB9XHJcblxyXG4uZmlsdGVyLWxpbmsge1xyXG4gICAgdG9wOiAtNDVweDtcclxuICAgIGxlZnQ6IDM4MHB4XHJcbn1cclxuIl19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](ViewAccessorialFeesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-view-accessorial-fees',
          templateUrl: './view-accessorial-fees.component.html',
          styleUrls: ['./view-accessorial-fees.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]
        }, {
          type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]
        }, {
          type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__["AccessorialFeesService"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"]
        }];
      }, {
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"]]
        }],
        accessorialFeeComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts": function srcAppAccessorialFeesViewViewFeesDetailsViewFeesDetailsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewFeesDetailsComponent", function () {
      return ViewFeesDetailsComponent;
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


    var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../service/accessorialfees.service */
    "./src/app/accessorial-fees/service/accessorialfees.service.ts"); //import { FeesDetailModel, AccessorialFuelFeeModel } from '../../model/accessorial-fees';


    var ViewFeesDetailsComponent = /*#__PURE__*/function () {
      function ViewFeesDetailsComponent(fb, accessorialFeeService) {
        _classCallCheck(this, ViewFeesDetailsComponent);

        this.fb = fb;
        this.accessorialFeeService = accessorialFeeService;
      } //public AccessorialFuelFee: AccessorialFuelFeeModel;
      //public FeesDetailList: FeesDetailModel[]


      _createClass(ViewFeesDetailsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "getAccessorialFeesDetails",
        value: function getAccessorialFeesDetails(accessorialFeeId) {//this.accessorialFeeService.getAccessorialFee(accessorialFeeId).subscribe(data => {
          //    this.AccessorialFuelFee = data as AccessorialFuelFeeModel;
          //    this.FeesDetailList = this.AccessorialFuelFee.FuelFees;
          //});
        }
      }]);

      return ViewFeesDetailsComponent;
    }();

    ViewFeesDetailsComponent.??fac = function ViewFeesDetailsComponent_Factory(t) {
      return new (t || ViewFeesDetailsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__["AccessorialFeesService"]));
    };

    ViewFeesDetailsComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({
      type: ViewFeesDetailsComponent,
      selectors: [["app-view-fees-details"]],
      decls: 0,
      vars: 0,
      template: function ViewFeesDetailsComponent_Template(rf, ctx) {},
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvdmlldy92aWV3LWZlZXMtZGV0YWlscy92aWV3LWZlZXMtZGV0YWlscy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](ViewFeesDetailsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-view-fees-details',
          templateUrl: './view-fees-details.component.html',
          styleUrls: ['./view-fees-details.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__["AccessorialFeesService"]
        }];
      }, null);
    })();
    /***/

  },

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

    var RegionService = /*#__PURE__*/function (_src_app_errors_Handl2) {
      _inherits(RegionService, _src_app_errors_Handl2);

      var _super2 = _createSuper(RegionService);

      function RegionService(httpClient) {
        var _this24;

        _classCallCheck(this, RegionService);

        _this24 = _super2.call(this);
        _this24.httpClient = httpClient;
        _this24.createUrl = '/Region/Create';
        _this24.updateUrl = '/Region/Update';
        _this24.deleteUrl = '/Region/Delete?id=';
        _this24.getRegionsUrl = '/Region/GetRegions';
        _this24.getSourceRegionsUrl = '/Region/GetSourceRegions';
        _this24.createSourceRegionUrl = '/Region/CreateSourceRegion';
        _this24.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        _this24.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        _this24.getJobsUrl = '/Region/GetJobs';
        _this24.getDriversUrl = '/Region/GetDrivers';
        _this24.getDispatchersUrl = '/Region/GetDispatchers';
        _this24.getTrailersUrl = '/Region/GetTrailers';
        _this24.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        _this24.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        _this24.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        _this24.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        _this24.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        _this24.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        _this24.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        _this24.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        _this24.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        _this24.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        _this24.getCarriersUrl = '/Region/GetCarriers';
        _this24.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        _this24.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        _this24.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        _this24.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        _this24.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        _this24.getFuelProductUrl = '/Region/GetMstFuelProducts';
        _this24.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        _this24.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        return _this24;
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

    RegionService.??fac = function RegionService_Factory(t) {
      return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]));
    };

    RegionService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({
      token: RegionService,
      factory: RegionService.??fac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](RegionService, [{
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
  "./src/app/invoice/services/fee.service.ts": function srcAppInvoiceServicesFeeServiceTs(module, __webpack_exports__, __webpack_require__) {
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

      var _super3 = _createSuper(FeeService);

      function FeeService(httpClient) {
        var _this25;

        _classCallCheck(this, FeeService);

        _this25 = _super3.call(this);
        _this25.httpClient = httpClient;
        _this25.getFeeTypesUrl = '/Supplier/Invoice/GetFeeTypes?orderId=';
        _this25.getFeeSubTypesUrl = '/Supplier/Invoice/GetFeeSubTypes?orderId=';
        _this25.getFeeConstraintTypesUrl = '/Supplier/Invoice/GetFeeConstraintTypes';
        _this25.getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';
        return _this25;
      }

      _createClass(FeeService, [{
        key: "getFeeTypes",
        value: function getFeeTypes(orderId, isFromAccesorialFees) {
          return this.httpClient.get(this.getFeeTypesUrl + orderId + '&isFromAccesorialFees=' + isFromAccesorialFees).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeTypes', [])));
        }
      }, {
        key: "getFeeSubTypes",
        value: function getFeeSubTypes(orderId) {
          return this.httpClient.get(this.getFeeSubTypesUrl + orderId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeSubTypes', [])));
        }
      }, {
        key: "getFeeConstraintTypes",
        value: function getFeeConstraintTypes() {
          return this.httpClient.get(this.getFeeConstraintTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeConstraintTypes', [])));
        }
      }, {
        key: "getEiaPrice",
        value: function getEiaPrice(data) {
          return this.httpClient.post(this.getEiaPriceUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getEiaPrice', [])));
        }
      }]);

      return FeeService;
    }(_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"]);

    FeeService.??fac = function FeeService_Factory(t) {
      return new (t || FeeService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    FeeService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({
      token: FeeService,
      factory: FeeService.??fac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](FeeService, [{
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
//# sourceMappingURL=accessorial-fees-accessorial-fees-module-es5.js.map