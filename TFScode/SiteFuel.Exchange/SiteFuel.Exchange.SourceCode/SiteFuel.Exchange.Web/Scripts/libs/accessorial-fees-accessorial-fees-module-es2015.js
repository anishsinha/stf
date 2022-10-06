(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["accessorial-fees-accessorial-fees-module"],{

/***/ "./src/app/accessorial-fees/accessorial-fees.module.ts":
/*!*************************************************************!*\
  !*** ./src/app/accessorial-fees/accessorial-fees.module.ts ***!
  \*************************************************************/
/*! exports provided: AccessorialFeesModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AccessorialFeesModule", function() { return AccessorialFeesModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _master_master_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./master/master.component */ "./src/app/accessorial-fees/master/master.component.ts");
/* harmony import */ var _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./view/view-accessorial-fees.component */ "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./create/create-accessorial-fees.component */ "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts");
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./create/child-components/fee-list.component */ "./src/app/accessorial-fees/create/child-components/fee-list.component.ts");
/* harmony import */ var _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./create/child-components/fee-type.component */ "./src/app/accessorial-fees/create/child-components/fee-type.component.ts");
/* harmony import */ var _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./view/view-fees-details/view-fees-details.component */ "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
















const route = [
    { path: '', component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"] },
    { path: 'Create', component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"] }
];
class AccessorialFeesModule {
}
AccessorialFeesModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: AccessorialFeesModule });
AccessorialFeesModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function AccessorialFeesModule_Factory(t) { return new (t || AccessorialFeesModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
            angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](AccessorialFeesModule, { declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"],
        _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["ViewAccessorialFeesComponent"],
        _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__["CreateAccessorialFeesComponent"],
        _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__["FeeListComponent"],
        _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__["FeeTypeComponent"],
        _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__["ViewFeesDetailsComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
        _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AccessorialFeesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [
                    _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"],
                    _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["ViewAccessorialFeesComponent"],
                    _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_5__["CreateAccessorialFeesComponent"],
                    _create_child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_10__["FeeListComponent"],
                    _create_child_components_fee_type_component__WEBPACK_IMPORTED_MODULE_11__["FeeTypeComponent"],
                    _view_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_12__["ViewFeesDetailsComponent"]
                ],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    _modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"],
                    _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormsModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTablesModule"],
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_9__["DirectiveModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(route),
                    angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["AngularMultiSelectModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/accessorial-fees/create/child-components/fee-list.component.ts":
/*!********************************************************************************!*\
  !*** ./src/app/accessorial-fees/create/child-components/fee-list.component.ts ***!
  \********************************************************************************/
/*! exports provided: FeeListComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FeeListComponent", function() { return FeeListComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../../invoice/models/DropDetail */ "./src/app/invoice/models/DropDetail.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../../invoice/services/fee.service */ "./src/app/invoice/services/fee.service.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../../carrier/service/data.service */ "./src/app/carrier/service/data.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _fee_type_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./fee-type.component */ "./src/app/accessorial-fees/create/child-components/fee-type.component.ts");











function FeeListComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeListComponent_ng_container_8_Template(rf, ctx) { if (rf & 1) {
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_8_Template_a_click_5_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const commonFee_r5 = ctx.$implicit; const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r8.removeGeneralFee(true, commonFee_r5); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const commonFee_r5 = ctx.$implicit;
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r1.Parent)("FeeGroup", commonFee_r5)("FeeTypes", ctx_r1.FeeTypes)("Currency", ctx_r1.DisplayCurrency)("FeeConstraintTypes", ctx_r1.FeeConstraintTypes)("FeeSubTypes", ctx_r1.FeeSubTypes);
} }
function FeeListComponent_ng_container_15_Template(rf, ctx) { if (rf & 1) {
    const _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_15_Template_a_click_5_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r14); const otherFee_r10 = ctx.$implicit; const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r13.removeGeneralFee(false, otherFee_r10); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const otherFee_r10 = ctx.$implicit;
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r2.Parent)("FeeGroup", otherFee_r10)("FeeTypes", ctx_r2.FeeTypes)("Currency", ctx_r2.DisplayCurrency)("FeeConstraintTypes", ctx_r2.FeeConstraintTypes)("FeeSubTypes", ctx_r2.FeeSubTypes);
} }
function FeeListComponent_ng_container_25_Template(rf, ctx) { if (rf & 1) {
    const _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_25_Template_a_click_5_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19); const spCommonFee_r15 = ctx.$implicit; const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r18.removeSpecialFee(true, spCommonFee_r15); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const spCommonFee_r15 = ctx.$implicit;
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r3.Parent)("FeeGroup", spCommonFee_r15)("FeeTypes", ctx_r3.FeeTypes)("Currency", ctx_r3.DisplayCurrency)("FeeConstraintTypes", ctx_r3.FeeConstraintTypes)("FeeSubTypes", ctx_r3.FeeSubTypes);
} }
function FeeListComponent_ng_container_32_Template(rf, ctx) { if (rf & 1) {
    const _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "app-fee-type", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_ng_container_32_Template_a_click_5_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r24); const spOtherFee_r20 = ctx.$implicit; const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r23.removeSpecialFee(false, spOtherFee_r20); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const spOtherFee_r20 = ctx.$implicit;
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r4.Parent)("FeeGroup", spOtherFee_r20)("FeeTypes", ctx_r4.FeeTypes)("Currency", ctx_r4.DisplayCurrency)("FeeConstraintTypes", ctx_r4.FeeConstraintTypes)("FeeSubTypes", ctx_r4.FeeSubTypes);
} }
class FeeListComponent {
    //public OrderId: number;
    constructor(fb, feeService, route, dataService) {
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
    ngOnInit() {
        //this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.Parent.addControl('Fees', this.fb.array([]));
        this.IsLoading = true;
        this.feeService.getFeeTypes(0, true).subscribe(data => {
            this.IsLoading = false;
            this.FeeTypes = data;
        });
        this.feeService.getFeeConstraintTypes()
            .subscribe((data) => { this.FeeConstraintTypes = data; });
        this.feeService.getFeeSubTypes(0)
            .subscribe((data) => {
            this.FeeSubTypes = data.filter(function (elem) { return elem.FeeSubTypeId != 1; });
        });
        this.dataService.RemoveFeesSubject.subscribe(x => {
            this.removeFeesOnCreateNew();
        });
    }
    ngOnChanges(change) {
        if (change.CountryId && change.CountryId.currentValue) {
            var currency = change.CountryId.currentValue;
            if (currency == 1) {
                this.DisplayCurrency = "USD";
            }
            else if (currency == 2) {
                this.DisplayCurrency = "CAD";
            }
        }
        if (change.Fees && change.Fees.currentValue) {
            this.CommonFees = [];
            this.OtherFees = [];
            this.SpCommonFees = [];
            this.SpOtherFees = [];
            let fees = this.Parent.get('Fees');
            if (fees) {
                fees.clear();
            }
            var currValues = change.Fees.currentValue;
            currValues.forEach((x) => {
                if (x.FeeConstraintTypeId == null) {
                    this.addGeneralFee(x.CommonFee, x);
                }
                else {
                    this.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
                }
            });
        }
    }
    getForm(model) {
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
            DeliveryFeeByQuantity: this.fb.array(byQuantity),
        });
        return group;
    }
    addGeneralFee(_commonFee, feeObj) {
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
        }
        else {
            this.OtherFees.push(feeGroup);
        }
        this.Parent.get('Fees').push(feeGroup);
    }
    removeGeneralFee(_commonFee, fee) {
        var _fees = this.Parent.get('Fees');
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.CommonFees.splice(this.CommonFees.indexOf(fee), 1);
        }
        else {
            this.OtherFees.splice(this.OtherFees.indexOf(fee), 1);
        }
    }
    addSpecialFee(_commonFee, typeId, feeObj) {
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
        }
        else {
            this.SpOtherFees.push(feeGroup);
        }
        this.Parent.get('Fees').push(feeGroup);
    }
    removeSpecialFee(_commonFee, fee) {
        var _fees = this.Parent.get('Fees');
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.SpCommonFees.splice(this.SpCommonFees.indexOf(fee), 1);
        }
        else {
            this.SpOtherFees.splice(this.SpOtherFees.indexOf(fee), 1);
        }
    }
    addByQtyFee(fee) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        var lastFee = _fees.controls[_fees.controls.length - 1].get('Fee').value;
        var feeObj = new _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__["FeeModel"]();
        feeObj.Fee = lastFee;
        _fees.push(this.getForm(feeObj));
    }
    removeByQtyFee(fee, index) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        _fees.removeAt(index);
    }
    //--------------------------------------------------------------
    isInvalid(drop, name) {
        return drop.get(name).invalid &&
            (drop.get(name).dirty || drop.get(name).touched);
    }
    isRequired(drop, name) {
        return drop.get(name).errors.required;
    }
    isMin(drop, name) {
        return drop.get(name).errors.min;
    }
    requiredIfValidator(predicate) {
        return (formControl => {
            if (!formControl.parent) {
                return null;
            }
            if (predicate()) {
                return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(formControl);
            }
            return null;
        });
    }
    removeFeesOnCreateNew() {
        this.CommonFees.forEach(commonFee => {
            this.removeGeneralFee(true, commonFee);
        });
        this.OtherFees.forEach(OtherFee => {
            this.removeGeneralFee(false, OtherFee);
        });
        this.SpCommonFees.forEach(SpCommonFee => {
            this.removeSpecialFee(true, SpCommonFee);
        });
        this.SpOtherFees.forEach(SpOtherFee => {
            this.removeSpecialFee(false, SpOtherFee);
        });
    }
}
FeeListComponent.ɵfac = function FeeListComponent_Factory(t) { return new (t || FeeListComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__["FeeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_5__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__["DataService"])); };
FeeListComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: FeeListComponent, selectors: [["app-fee-list"]], inputs: { Parent: "Parent", CountryId: "CountryId", Fees: "Fees" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 36, vars: 6, consts: [["class", "loader", 4, "ngIf"], [1, "well", "box-shadow", 3, "formGroup"], ["formArrayName", "Fees"], [1, "mt10", "mb5"], [4, "ngFor", "ngForOf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "mt10"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "row"], [1, "col-sm-10"], [3, "Parent", "FeeGroup", "FeeTypes", "Currency", "FeeConstraintTypes", "FeeSubTypes"], [1, "col-sm-2"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", "mt10", 3, "click"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", 3, "click"]], template: function FeeListComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](0, FeeListComponent_div_0_Template, 5, 0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "h4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Fees");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "b");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "General");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, FeeListComponent_ng_container_8_Template, 6, 6, "ng-container", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_9_listener() { return ctx.addGeneralFee(true, null); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "i", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, " Add Fee");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "b");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Other");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, FeeListComponent_ng_container_15_Template, 6, 6, "ng-container", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_16_listener() { return ctx.addGeneralFee(false, null); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](17, "i", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, " Add Fee");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "h4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Weekend / Holiday Fee(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "b");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "General");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, FeeListComponent_ng_container_25_Template, 6, 6, "ng-container", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_26_listener() { return ctx.addSpecialFee(true, 1, null); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](27, "i", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, " Add Fee");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "b");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Other");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, FeeListComponent_ng_container_32_Template, 6, 6, "ng-container", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeListComponent_Template_button_click_33_listener() { return ctx.addSpecialFee(false, 1, null); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](34, "i", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](35, " Add Fee");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.Parent);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CommonFees);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.OtherFees);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.SpCommonFees);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.SpOtherFees);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _fee_type_component__WEBPACK_IMPORTED_MODULE_8__["FeeTypeComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NoaWxkLWNvbXBvbmVudHMvZmVlLWxpc3QuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeListComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-fee-list',
                templateUrl: './fee-list.component.html',
                styleUrls: ['./fee-list.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_4__["FeeService"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_5__["ActivatedRoute"] }, { type: _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_6__["DataService"] }]; }, { Parent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], CountryId: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], Fees: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }] }); })();


/***/ }),

/***/ "./src/app/accessorial-fees/create/child-components/fee-type.component.ts":
/*!********************************************************************************!*\
  !*** ./src/app/accessorial-fees/create/child-components/fee-type.component.ts ***!
  \********************************************************************************/
/*! exports provided: FeeTypeComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FeeTypeComponent", function() { return FeeTypeComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _invoice_models_DropDetail__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../../invoice/models/DropDetail */ "./src/app/invoice/models/DropDetail.ts");
/* harmony import */ var _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../../invoice/services/fee.service */ "./src/app/invoice/services/fee.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");








function FeeTypeComponent_option_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ft_r12 = ctx.$implicit;
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", ft_r12.Code)("selected", ft_r12.Code == ctx_r1.FeeGroup.get("FeeTypeId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ft_r12.Name);
} }
function FeeTypeComponent_span_9_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_span_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_9_span_1_Template, 2, 0, "span", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.isRequired(ctx_r2.FeeGroup, "FeeTypeId") || ctx_r2.isFeeNameRequired(ctx_r2.FeeGroup, "OtherFeeDescription"));
} }
function FeeTypeComponent_option_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fst_r14 = ctx.$implicit;
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", fst_r14.FeeSubTypeId)("selected", fst_r14.FeeSubTypeId == ctx_r3.FeeGroup.get("FeeSubTypeId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", fst_r14.SubTypeName, " ");
} }
function FeeTypeComponent_span_16_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_span_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_16_span_1_Template, 2, 0, "span", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.isRequired(ctx_r4.FeeGroup, "FeeSubTypeId"));
} }
function FeeTypeComponent_div_17_select_2_option_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fc_r18 = ctx.$implicit;
    const ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", fc_r18.Id)("selected", fc_r18.Id == ctx_r17.FeeGroup.get("FeeConstraintTypeId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", fc_r18.Name, " ");
} }
function FeeTypeComponent_div_17_select_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "select", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_17_select_2_option_1_Template, 2, 3, "option", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r16.FeeConstraintTypes);
} }
function FeeTypeComponent_div_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, FeeTypeComponent_div_17_select_2_Template, 2, 1, "select", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.FeeGroup.get("FeeConstraintTypeId").value);
} }
function FeeTypeComponent_div_18_Template(rf, ctx) { if (rf & 1) {
    const _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "input", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function FeeTypeComponent_div_18_Template_input_onDateChange_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r20); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r19.FeeGroup.get("SpecialDate").setValue($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("maxDate", ctx_r6.maxDate)("minDate", ctx_r6.minDate)("format", "MM/DD/YYYY");
} }
function FeeTypeComponent_div_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "input", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_input_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "input", 29);
} }
function FeeTypeComponent_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r9.DisplayCurrency);
} }
function FeeTypeComponent_span_25_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_span_25_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_span_25_span_1_Template, 2, 0, "span", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r10.isRequired(ctx_r10.FeeGroup, "Fee"));
} }
function FeeTypeComponent_div_26_div_2_span_7_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_div_26_div_2_span_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_26_div_2_span_7_span_1_Template, 2, 0, "span", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const byQty_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r25.isRequired(byQty_r23, "MaxQuantity"));
} }
function FeeTypeComponent_div_26_div_2_span_10_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FeeTypeComponent_div_26_div_2_span_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, FeeTypeComponent_div_26_div_2_span_10_span_1_Template, 2, 0, "span", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const byQty_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r26.isRequired(byQty_r23, "Fee"));
} }
function FeeTypeComponent_div_26_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, FeeTypeComponent_div_26_div_2_span_7_Template, 2, 1, "span", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "input", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, FeeTypeComponent_div_26_div_2_span_10_Template, 2, 1, "span", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeTypeComponent_div_26_div_2_Template_a_click_12_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r32); const i_r24 = ctx.index; const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r31.removeByQtyFee(ctx_r31.FeeGroup, i_r24); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const byQty_r23 = ctx.$implicit;
    const i_r24 = ctx.index;
    const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r22.isInvalid(byQty_r23, "MaxQuantity"));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r22.isInvalid(byQty_r23, "Fee"));
} }
function FeeTypeComponent_div_26_Template(rf, ctx) { if (rf & 1) {
    const _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, FeeTypeComponent_div_26_div_2_Template, 14, 3, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FeeTypeComponent_div_26_Template_a_click_5_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34); const ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r33.addByQtyFee(ctx_r33.FeeGroup, null); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "i", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " Add Quantity Range");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r11.FeeGroup.get("DeliveryFeeByQuantity")["controls"]);
} }
class FeeTypeComponent {
    constructor(fb, feeService) {
        this.fb = fb;
        this.feeService = feeService;
        this.maxDate = new Date();
        this.minDate = new Date();
        this.DisplayFeeTypes = [];
    }
    ngOnInit() {
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
        this.FeeGroup.setValidators(this.feeNameRequired('FeeTypeId', 'OtherFeeDescription', 'CommonFee'));
        if (this.FeeSubTypes != null && this.FeeSubTypes != undefined)
            this.DisplayFeeTypes = this.FeeSubTypes.slice();
        this.DisplayCurrency = this.Currency;
    }
    ngOnChanges(change) {
        if (change.FeeSubTypes && change.FeeSubTypes.currentValue != null) {
            var subTypes = change.FeeSubTypes.currentValue;
            this.DisplayFeeTypes = subTypes;
        }
    }
    updateSubType(feeTypeId) {
        this.DisplayFeeTypes = this.FeeSubTypes.slice().filter(function (elem) { return elem.FeeTypeId == feeTypeId; });
    }
    getForm(_fee) {
        return this.fb.group({
            Currency: this.fb.control(_fee.Currency),
            MinQuantity: this.fb.control(_fee.MinQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]),
            MaxQuantity: this.fb.control(_fee.MaxQuantity, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/), _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Fee: this.fb.control(_fee.Fee, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)])
        });
    }
    addByQtyFee(fee, feeObj) {
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
    removeByQtyFee(fee, index) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        _fees.removeAt(index);
        if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');
            lastMax.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/), _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
        }
    }
    isInvalid(group, name) {
        return group.get(name).invalid &&
            (group.get(name).dirty || group.get(name).touched || group.get(name).invalid);
    }
    isRequired(group, name) {
        return group.get(name).errors.required;
    }
    isFeeNameRequired(group, name) {
        return group.get(name).errors.required;
    }
    handleByQuantity(group, subTypeId) {
        var fee = group.get('Fee');
        if (subTypeId == 3) {
            fee.setValue(0);
        }
        else {
            if (fee.value == 0) {
                fee.setValue('');
            }
            group.get('DeliveryFeeByQuantity').clear();
        }
    }
    feeNameRequired(field1Name, field2Name, field3Name) {
        let field1 = this.FeeGroup.controls[field1Name];
        let field2 = this.FeeGroup.controls[field2Name];
        let field3 = this.FeeGroup.controls[field3Name];
        if (field3.value && (field1.value == null || field1.value == '')) {
            return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(field1);
        }
        else if (!field3.value && (field2.value == null || field2.value.replace(/\s/g, '') == '')) {
            return _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required(field2);
        }
        else {
            return null;
        }
    }
}
FeeTypeComponent.ɵfac = function FeeTypeComponent_Factory(t) { return new (t || FeeTypeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__["FeeService"])); };
FeeTypeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: FeeTypeComponent, selectors: [["app-fee-type"]], inputs: { Parent: "Parent", FeeGroup: "FeeGroup", Currency: "Currency", FeeTypes: "FeeTypes", FeeSubTypes: "FeeSubTypes", FeeConstraintTypes: "FeeConstraintTypes" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 32, vars: 18, consts: [[1, "row", 3, "formGroup"], [1, "col-sm-3"], ["formControlName", "FeeTypeId", 1, "form-control", 3, "change"], ["feeTypeId", ""], [3, "value"], [3, "value", "selected", 4, "ngFor", "ngForOf"], [1, "mb15", "form-group"], ["type", "text", "formControlName", "OtherFeeDescription", "placeholder", "Fee Name", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "form-group"], ["formControlName", "FeeSubTypeId", 1, "form-control", 3, "focus", "change"], ["class", "color-maroon pa", 4, "ngIf"], ["class", "col-sm-3", 4, "ngIf"], [1, "input-group"], ["type", "number", "formControlName", "Fee", "class", "form-control", "placeholder", "Fee", 4, "ngIf"], ["class", "input-group-addon fs12", 4, "ngIf"], ["class", "col-sm-9", 4, "ngIf"], [1, "col-sm-2", "text-lg-right", "mt-2"], ["type", "checkbox", "formControlName", "IncludeInPPG"], [1, "ml-2"], ["type", "hidden", "formControlName", "Currency"], [3, "value", "selected"], [1, "color-maroon"], [4, "ngIf"], [1, "color-maroon", "pa"], ["formControlName", "FeeConstraintTypeId", "class", "form-control", 4, "ngIf"], ["formControlName", "FeeConstraintTypeId", 1, "form-control"], ["type", "text", "formControlName", "SpecialDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "maxDate", "minDate", "format", "onDateChange"], ["type", "text", "formControlName", "MinimumGallons", "placeholder", "Min Quantity", 1, "form-control"], ["type", "number", "formControlName", "Fee", "placeholder", "Fee", 1, "form-control"], [1, "input-group-addon", "fs12"], [1, "col-sm-9"], ["formArrayName", "DeliveryFeeByQuantity"], [4, "ngFor", "ngForOf"], [1, "row", "mb10"], [1, "col-sm-12"], [3, "click"], [1, "fa", "fa-plus-circle"], [1, "row", 3, "formGroupName"], [1, "col-sm-3", "pr0", "mb5"], ["type", "text", "formControlName", "MinQuantity", "readonly", "readonly", "placeholder", "Min Quantity", 1, "form-control"], ["type", "text", "formControlName", "MaxQuantity", "placeholder", "Max Quantity", 1, "form-control"], ["type", "text", "formControlName", "Fee", "placeholder", "Fee", 1, "form-control"], [1, "col-sm-1"], [1, "fa", "fa-trash-alt", "color-maroon", "mt10"]], template: function FeeTypeComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "select", 2, 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function FeeTypeComponent_Template_select_change_2_listener() { return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Select Fee");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, FeeTypeComponent_option_6_Template, 2, 3, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, FeeTypeComponent_span_9_Template, 2, 1, "span", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "select", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("focus", function FeeTypeComponent_Template_select_focus_12_listener() { return ctx.updateSubType(ctx.FeeGroup.get("FeeTypeId").value); })("change", function FeeTypeComponent_Template_select_change_12_listener() { return ctx.handleByQuantity(ctx.FeeGroup, ctx.FeeGroup.get("FeeSubTypeId").value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Select Fee Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, FeeTypeComponent_option_15_Template, 2, 3, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, FeeTypeComponent_span_16_Template, 2, 1, "span", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, FeeTypeComponent_div_17_Template, 3, 1, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, FeeTypeComponent_div_18_Template, 3, 3, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, FeeTypeComponent_div_19_Template, 2, 0, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, FeeTypeComponent_input_23_Template, 1, 0, "input", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, FeeTypeComponent_div_24_Template, 2, 1, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, FeeTypeComponent_span_25_Template, 2, 1, "span", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, FeeTypeComponent_div_26_Template, 8, 1, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](28, "input", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "label", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30, "Include In PPG ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](31, "input", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.FeeGroup);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstyleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "block" : "none");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.FeeTypes);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstyleProp"]("display", ctx.FeeGroup.get("CommonFee").value ? "none" : "block");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.FeeGroup, "FeeTypeId") || ctx.isInvalid(ctx.FeeGroup, "OtherFeeDescription"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
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
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NumberValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NoaWxkLWNvbXBvbmVudHMvZmVlLXR5cGUuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeTypeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-fee-type',
                templateUrl: './fee-type.component.html',
                styleUrls: ['./fee-type.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _invoice_services_fee_service__WEBPACK_IMPORTED_MODULE_3__["FeeService"] }]; }, { Parent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], FeeGroup: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], Currency: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], FeeTypes: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], FeeSubTypes: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], FeeConstraintTypes: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }] }); })();


/***/ }),

/***/ "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts":
/*!******************************************************************************!*\
  !*** ./src/app/accessorial-fees/create/create-accessorial-fees.component.ts ***!
  \******************************************************************************/
/*! exports provided: CreateAccessorialFeesComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateAccessorialFeesComponent", function() { return CreateAccessorialFeesComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../model/accessorial-fees */ "./src/app/accessorial-fees/model/accessorial-fees.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! src/app/fuelsurcharge/services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../../carrier/service/data.service */ "./src/app/carrier/service/data.service.ts");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! src/app/carrier/service/carrier.service */ "./src/app/carrier/service/carrier.service.ts");
/* harmony import */ var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ../service/accessorialfees.service */ "./src/app/accessorial-fees/service/accessorialfees.service.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");
/* harmony import */ var _child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ./child-components/fee-list.component */ "./src/app/accessorial-fees/create/child-components/fee-list.component.ts");























function CreateAccessorialFeesComponent_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "button", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateAccessorialFeesComponent_div_2_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r9); const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r8.clearForm(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Create New");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_12_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_12_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.rcForm.get("TableName").errors.required);
} }
function CreateAccessorialFeesComponent_div_20_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_20_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.rcForm.get("TableTypes").errors.required);
} }
function CreateAccessorialFeesComponent_div_27_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_27_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.get("Customers").errors.required);
} }
function CreateAccessorialFeesComponent_div_34_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_34_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_34_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.rcForm.get("Carriers").errors.required);
} }
function CreateAccessorialFeesComponent_div_42_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_42_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_42_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.rcForm.get("SourceRegions").errors.required);
} }
function CreateAccessorialFeesComponent_div_55_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function CreateAccessorialFeesComponent_div_55_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateAccessorialFeesComponent_div_55_div_1_Template, 2, 0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r6.rcForm.get("StartDate").errors.required);
} }
function CreateAccessorialFeesComponent_div_68_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
const _c0 = function (a0) { return { "pntr-none": a0 }; };
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
class CreateAccessorialFeesComponent {
    constructor(fb, fuelsurchargeService, dataService, regionService, carrierService, accesorialFeeService, http, _document) {
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
    ngOnInit() {
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
        this.rcForm.controls['TableTypes'].patchValue([{ Id: src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master, Name: "Master" }]); // default will master
        this.IsMasterSelected = true;
        this.getSourceRegions(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master.toString());
        this.accesorialFeeService.onSelectedAccessorialFeeId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.AccessorialFeeId = stringify.AccessorialFeeId;
                this.AccessorialFeeMode = stringify.Mode;
            }
        });
        let id = localStorage.getItem("AccessorialFeeId");
        if (id && +id > 0) {
            this.AccessorialFeeId = Number(id);
            this.AccessorialFeeMode = "VIEW";
            localStorage.removeItem("AccessorialFeeId");
        }
        Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["merge"])(this.rcForm.get('SourceRegions').valueChanges)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["startWith"])(null), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["pairwise"])())
            .subscribe(([prev, next]) => {
            if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next))
                this.SourceRegionChange(prev, next);
        });
    }
    getDefaultServingCountry() {
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.SelectedCountryId = Number(data.DefaultCountryId);
        });
    }
    getTableTypes() {
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TableTypeList = yield (data);
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master)); // default will master
            this.rcForm.controls['TableTypeId'].setValue(src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master);
            this.IsMasterSelected = true;
        }));
    }
    getSourceRegions(tableType) {
        let customerIds = [];
        let carrierIds = [];
        let selectedCustomers = this.rcForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }
        let selectedCarriers = this.rcForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.SourceRegionList = yield (data);
        }));
    }
    createForm() {
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
            StatusId: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
        });
    }
    AddRemoveValidations(requiredControls, notRequiredControls) {
        if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {
            requiredControls.forEach(ctrl => {
                ctrl.setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required]);
                ctrl.updateValueAndValidity();
            });
        }
        if (notRequiredControls != null && notRequiredControls != undefined && notRequiredControls.length > 0) {
            notRequiredControls.forEach(ctrl => {
                ctrl.clearValidators();
                ctrl.updateValueAndValidity();
            });
        }
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        this.rcForm.controls['TableTypeId'].setValue(item.Id);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                break;
            case 2: // customer
                this.getSupplierCustomers();
                this.getCarriers();
                this.IsCustomerSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
                break;
            case 3: //carrier
                this.getSupplierCustomers();
                this.getCarriers();
                this.IsCarrierSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
                break;
        }
        this.rcForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    onCarriersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCustomersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCustomersDeSelect(item) {
        this.rcForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCarriersDeSelect(item) {
        this.rcForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCarriersOrCustomersChange() {
        var selectedTableType = this.rcForm.get('TableTypes').value;
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    onSourceRegionsDeSelect(item) {
        var sr = this.rcForm.get('SourceRegions').value;
        this.IsSourceRegionSelected = sr.length > 0;
    }
    onSourceRegionsDeSelectAll(item) {
        this.IsSourceRegionSelected = false;
    }
    getSupplierCustomers() {
        this.fuelsurchargeService.getSupplierCustomers().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CustomerList = yield (data);
        }));
    }
    SourceRegionChange(prev, next) {
        if (prev == null && next.length == 0)
            return;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.IsSourceRegionSelected = false;
        var ids = [];
        let selectedSourceRegions = this.rcForm.get('SourceRegions').value;
        if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
            }));
        }
    }
    getCarriers() {
        this.regionService.getCarriers()
            .subscribe((carriers) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CarrierList = yield carriers;
        }));
    }
    getTerminalsBulkPlant() {
        var selectedSourceRegions = this.rcForm.get('SourceRegions').value;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
            }));
        }
    }
    onSubmit(status) {
        let accessorialFeeName = this.rcForm.get('TableName').value;
        if (accessorialFeeName == null || accessorialFeeName == undefined || accessorialFeeName == "") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(" Table Name is required", undefined, undefined);
            return;
        }
        let AccessorialDate = this.rcForm.get('StartDate').value;
        if (AccessorialDate == null || AccessorialDate == undefined || AccessorialDate == "") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(" Date is required", undefined, undefined);
            return;
        }
        let feeModel = this.createPostObject(status);
        if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft && +this.rcForm.controls['StatusId'].value == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
            if (this.rcForm.get('AccessorialFeeId').value != "") {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Not allowed. " + this.rcForm.get('TableName').value + " is in published mode.", undefined, undefined);
                this.IsLoading = false;
                return;
            }
        }
        else if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
            this.rcForm.markAllAsTouched();
            if (this.rcForm.valid) {
                let fees = this.rcForm.get('Fees').value;
                if (fees == null || fees == undefined || fees.length == 0) {
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Please add Fee(s)", undefined, undefined);
                    return;
                }
            }
        }
        if (this.rcForm.get('AccessorialFeeId').value != "") {
            this.accesorialFeeService.updateAccessorialFee(feeModel)
                .subscribe((response) => {
                this.ServiceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    let message = " edited";
                    if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                        message = " saved draft";
                    }
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                    this.IsLoading = false;
                    this.changeViewType(2);
                }
                else {
                    this.IsLoading = false;
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            this.accesorialFeeService.createAccessorialFee(feeModel)
                .subscribe((response) => {
                this.ServiceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    let message = "";
                    if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Published) {
                        message = " created";
                    }
                    else if (feeModel.Status == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["FreightTableStatus"].Draft) {
                        message = " saved draft";
                    }
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                    this.IsLoading = false;
                    this.changeViewType(2);
                }
                else {
                    this.IsLoading = false;
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        }
    }
    clearForm() {
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
    onCancel() {
        if (this.AccessorialFeeMode == "VIEW") {
            this.disableInputControls = false;
            this.AccessorialFeeId = null;
        }
        if (this.AccessorialFeeMode == "EDIT") {
            this.AccessorialFeeId = null;
        }
        if (this.AccessorialFeeId != null) {
            this.changeToViewTab();
        }
        else {
            this._document.defaultView.location.reload();
        }
    }
    changeToViewTab() {
        this.accesorialFeeService.onSelectedTabChanged.next(1);
    }
    removeValidators(form) {
        for (const key in form.controls) {
            if (key == 'TableName') {
                continue;
            }
            else {
                form.get(key).clearValidators();
                form.get(key).updateValueAndValidity();
            }
        }
    }
    changeViewType(viewType) {
        this.onPageSubmit.emit(viewType);
    }
    createPostObject(status) {
        let feeModel = new _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_3__["CreateAccessorialFeeModel"]();
        feeModel.Id = this.rcForm.get('AccessorialFeeId').value;
        feeModel.Name = this.rcForm.get('TableName').value;
        feeModel.Status = status;
        let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;
        if (selectedTerminalBulkplant != null && selectedTerminalBulkplant != undefined && selectedTerminalBulkplant.length > 0) {
            feeModel.TerminalsAndBulkPlants = this.rcForm.get('TerminalsAndBulkPlants').value;
        }
        let selectedCustomers = this.rcForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            selectedCustomers.forEach(t => feeModel.CustomerIds.push(t.Id));
        }
        let selectedCarriers = this.rcForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            selectedCarriers.forEach(t => feeModel.CarrierIds.push(t.Id));
        }
        let endDate = this.rcForm.get('EndDate').value;
        let startDate = this.rcForm.get('StartDate').value;
        if (endDate == "" || endDate == undefined || endDate == null) {
            endDate = null;
        }
        if (startDate == "" || startDate == undefined || startDate == null) {
            startDate = null;
        }
        feeModel.StartDate = startDate;
        feeModel.EndDate = endDate;
        feeModel.Fees = this.rcForm.get('Fees').value;
        let sourceRegions = this.rcForm.get('SourceRegions').value;
        if (sourceRegions != null && sourceRegions != undefined && sourceRegions.length > 0) {
            sourceRegions.forEach(t => feeModel.SourceRegionIds.push(t.Id));
        }
        let tableType = this.rcForm.get('TableTypes').value;
        if (tableType != null && tableType != undefined && tableType.length > 0) {
            feeModel.TableType = tableType[0].Id;
        }
        feeModel.CountryId = this.SelectedCountryId;
        return feeModel;
    }
    getBulkPlantTerminalIds(type) {
        let Ids = [];
        if (type === 'Terminals') {
            let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;
            let selectedTerminals = selectedTerminalBulkplant.filter(t => t.Code === 'Terminals');
            if (selectedTerminals != null && selectedTerminals != undefined && selectedTerminals.length > 0) {
                selectedTerminals.forEach(function (terminal) {
                    let terminalId = parseInt(terminal.Id.replace("Terminals_", ""));
                    if (!isNaN(terminalId)) {
                        Ids.push(terminalId);
                    }
                });
            }
        }
        else if (type === 'BulkPlants') {
            let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value;
            let selectedBulkPlants = selectedTerminalBulkplant.filter(t => t.Code === 'BulkPlants');
            if (selectedBulkPlants != null && selectedBulkPlants != undefined && selectedBulkPlants.length > 0) {
                selectedBulkPlants.forEach(function (bulkplant) {
                    let bulkplantId = parseInt(bulkplant.Id.replace("BulkPlants_", ""));
                    if (!isNaN(bulkplantId)) {
                        Ids.push(bulkplantId);
                    }
                });
            }
        }
        return Ids;
    }
    ngAfterViewInit() {
        if (this.AccessorialFeeId != null && this.AccessorialFeeId != undefined) {
            this.getAccessorialFee(this.AccessorialFeeId); //existing Accessorial Fee.
        }
    }
    //GET
    getAccessorialFee(accessorialFeeId) {
        this.IsLoading = true;
        this.IsLoaded = false;
        let sorceRegionIds = "";
        this.http.get(this.accesorialFeeService.getAccessorialFeeUrl + accessorialFeeId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["map"])(af => {
            const afModel = af;
            return afModel;
        }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["mergeMap"])(afModel => {
            this.Afmodel = afModel;
            let companyIds = [];
            if (this.AccessorialFeeId != null && this.AccessorialFeeMode.toUpperCase() == "COPY") { // on copy 
                this.Afmodel.Id = null;
                this.Afmodel.Name = "";
            }
            const customers = this.http.get(this.fuelsurchargeService.getSupplierCustomersUrl);
            const carriers = this.http.get(this.regionService.getCarriersUrl);
            if (this.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer && this.Afmodel.CustomerIds.length > 0) {
                this.Afmodel.CustomerIds.forEach(s => companyIds.push(s));
            }
            if (this.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Carrier && this.Afmodel.CarrierIds.length > 0) {
                this.Afmodel.CarrierIds.forEach(s => companyIds.push(s));
            }
            var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["SourceRegionInputModel"]();
            sourceRegionInput.TableType = this.Afmodel.TableType.toString();
            sourceRegionInput.CustomerId = this.Afmodel.CustomerIds;
            sourceRegionInput.CarrierId = this.Afmodel.CarrierIds;
            const sourceRegions = this.http.post(this.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);
            const tableTypes = this.http.get(this.fuelsurchargeService.getTableTypesUrl);
            if (this.Afmodel.SourceRegionIds != null && this.Afmodel.SourceRegionIds != undefined && this.Afmodel.SourceRegionIds.length > 0) {
                sorceRegionIds = this.Afmodel.SourceRegionIds.map(s => s).join(',');
                this.IsSourceRegionSelected = true;
            }
            const terminalAndBulkPlans = this.http.get(this.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds);
            return Object(rxjs__WEBPACK_IMPORTED_MODULE_5__["forkJoin"])([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
        })).subscribe(result => {
            this.IsLoading = false;
            this.IsMasterSelected = false;
            this.IsCustomerSelected = false;
            this.IsCarrierSelected = false;
            if (this.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master) {
                this.IsMasterSelected = true;
            }
            else if (this.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Customer) {
                this.IsCustomerSelected = true;
            }
            else if (this.Afmodel.TableType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Carrier) {
                this.IsCarrierSelected = true;
            }
            if (this.Afmodel.TableType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master) {
                this.CustomerList = result[0];
                this.CarrierList = result[1];
            }
            this.SourceRegionList = result[2];
            if (this.Afmodel.TerminalsAndBulkPlants != null && this.Afmodel.TerminalsAndBulkPlants != undefined && this.Afmodel.TerminalsAndBulkPlants.length > 0) {
                this.TerminalsAndBulkPlantList = result[3];
            }
            this.TableTypeList = result[4];
            this.Edit(this.Afmodel);
        });
    }
    //Edit
    Edit(_af) {
        if (this.rcForm) {
            if (this.AccessorialFeeMode != "COPY") {
                this.rcForm.controls['AccessorialFeeId'].setValue(_af.Id);
                this.rcForm.controls['TableTypes'].setValue(_af.TableType);
                this.rcForm.controls['TableName'].setValue(_af.Name);
                this.IsEditable = false;
            }
            else {
                this.AccessorialFeeId = null;
            }
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == _af.TableType));
            if (_af.TableType != src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["TableType"].Master)
                this.IsMasterSelected = false;
            if (_af.CustomerIds != null && _af.CustomerIds != undefined && _af.CustomerIds.length > 0) {
                if (this.CustomerList.length > 0 && _af.CustomerIds.length > 0)
                    this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_af.CustomerIds)));
            }
            if (_af.CarrierIds != null && _af.CarrierIds != undefined && _af.CarrierIds.length > 0) {
                if (this.CarrierList.length > 0 && _af.CarrierIds.length > 0)
                    this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_af.CarrierIds)));
            }
            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _af.SourceRegionIds != null && _af.SourceRegionIds != undefined && _af.SourceRegionIds.length > 0) {
                if (this.SourceRegionList.length > 0 && _af.SourceRegionIds.length > 0)
                    this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_af.SourceRegionIds)));
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
    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other == current.Id;
            }).length == 1;
        };
    }
    ComparerWithId(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                //console.log(other + " : " + current.Id);
                return other.Id == current.Id;
            }).length == 1;
        };
    }
}
CreateAccessorialFeesComponent.ɵfac = function CreateAccessorialFeesComponent_Factory(t) { return new (t || CreateAccessorialFeesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__["DataService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__["AccessorialFeesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_15__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_common__WEBPACK_IMPORTED_MODULE_7__["DOCUMENT"])); };
CreateAccessorialFeesComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: CreateAccessorialFeesComponent, selectors: [["app-create-accessorial-fees"]], outputs: { onPageSubmit: "onPageSubmit" }, decls: 69, vars: 46, consts: [[3, "formGroup", "ngSubmit"], [4, "ngIf"], [3, "ngClass", "disabled"], [1, "well", "bg-white"], [1, "row"], [1, "col-sm-3", "form-group"], [1, "color-maroon"], ["type", "text", "formControlName", "TableName", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 1, "single-select", 3, "settings", "data", "placeholder", "onSelect"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "settings", "data", "placeholder", "onDeSelect", "onDeSelectAll"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "formControlName", "StartDate", "placeholder", "Effective Start Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "maxDate", "onDateChange"], ["type", "text", "formControlName", "EndDate", "placeholder", "Effective End Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], [1, "col-sm-12"], [3, "Parent", "CountryId", "Fees"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-lg", "btn-light", 3, "click"], ["type", "button", "value", "Draft", 1, "btn", "btn-lg", "btn-light", 3, "disabled", "click"], ["type", "button", "value", "Submit", 1, "btn", "btn-lg", "btn-primary", 3, "disabled", "click"], ["class", "loader", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateAccessorialFeesComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngSubmit", function CreateAccessorialFeesComponent_Template_form_ngSubmit_0_listener() { return ctx.onSubmit(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, CreateAccessorialFeesComponent_div_2_Template, 4, 0, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "fieldset", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Table Name ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, CreateAccessorialFeesComponent_div_12_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Table Type ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_19_listener($event) { return ctx.onTableTypeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, CreateAccessorialFeesComponent_div_20_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Select Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "ng-multiselect-dropdown", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_26_listener($event) { return ctx.onCustomersSelect($event); })("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_26_listener($event) { return ctx.onCustomersDeSelect($event); })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_26_listener($event) { return ctx.onCustomersDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, CreateAccessorialFeesComponent_div_27_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Select Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "ng-multiselect-dropdown", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onSelect_33_listener($event) { return ctx.onCarriersSelect($event); })("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_33_listener($event) { return ctx.onCarriersDeSelect($event); })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_33_listener($event) { return ctx.onCarriersDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, CreateAccessorialFeesComponent_div_34_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Select Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "ng-multiselect-dropdown", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDeSelect", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelect_41_listener($event) { return ctx.onSourceRegionsDeSelect($event); })("onDeSelectAll", function CreateAccessorialFeesComponent_Template_ng_multiselect_dropdown_onDeSelectAll_41_listener($event) { return ctx.onSourceRegionsDeSelectAll($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](42, CreateAccessorialFeesComponent_div_42_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "label", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](47, "Select Terminal(s)/BulkPlant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](48, "angular2-multiselect", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](51, "Effective Start Date ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](53, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "input", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateAccessorialFeesComponent_Template_input_onDateChange_54_listener($event) { return ctx.rcForm.get("StartDate").setValue($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](55, CreateAccessorialFeesComponent_div_55_Template, 2, 1, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58, "Effective End Date ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "input", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateAccessorialFeesComponent_Template_input_onDateChange_59_listener($event) { return ctx.rcForm.get("EndDate").setValue($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](62, "app-fee-list", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "input", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateAccessorialFeesComponent_Template_input_click_65_listener() { return ctx.onCancel(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateAccessorialFeesComponent_Template_input_click_66_listener() { return ctx.onSubmit(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "input", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateAccessorialFeesComponent_Template_input_click_67_listener() { return ctx.onSubmit(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](68, CreateAccessorialFeesComponent_div_68_Template, 5, 0, "div", 30);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.rcForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.AccessorialFeeMode != "CREATE");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](38, _c0, ctx.disableInputControls))("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableName").invalid && ctx.rcForm.get("TableName").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx.SingleSelectSettingsById)("data", ctx.TableTypeList)("placeholder", "Select Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("TableTypes").invalid && ctx.rcForm.get("TableTypes").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](40, _c1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx.MultiSelectSettingsById)("data", ctx.CustomerList)("placeholder", "Select Customers");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCustomerSelected && ctx.rcForm.get("Customers").invalid && ctx.rcForm.get("Customers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](42, _c1, ctx.IsMasterSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx.MultiSelectSettingsById)("data", ctx.CarrierList)("placeholder", "Select Carriers");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsCarrierSelected && ctx.rcForm.get("Carriers").invalid && ctx.rcForm.get("Carriers").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx.MultiSelectSettingsById)("data", ctx.SourceRegionList)("placeholder", "Select Source Regions");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("SourceRegions").invalid && ctx.rcForm.get("SourceRegions").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](44, _c1, !ctx.IsSourceRegionSelected));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx.TerminalsAndBulkPlantList)("settings", ctx.MultiSelectSettingsByGroup);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx.maxDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.rcForm.get("StartDate").invalid && ctx.rcForm.get("StartDate").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.minDate)("maxDate", ctx.maxDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("Parent", ctx.rcForm)("CountryId", ctx.SelectedCountryId)("Fees", ctx.Fees);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.disableInputControls ? true : null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["MultiSelectComponent"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["AngularMultiSelect"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__["DatePicker"], _child_components_fee_list_component__WEBPACK_IMPORTED_MODULE_19__["FeeListComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvY3JlYXRlL2NyZWF0ZS1hY2Nlc3NvcmlhbC1mZWVzLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CreateAccessorialFeesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-create-accessorial-fees',
                templateUrl: './create-accessorial-fees.component.html',
                styleUrls: ['./create-accessorial-fees.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }, { type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_10__["FuelSurchargeService"] }, { type: _carrier_service_data_service__WEBPACK_IMPORTED_MODULE_11__["DataService"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_12__["RegionService"] }, { type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_13__["CarrierService"] }, { type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_14__["AccessorialFeesService"] }, { type: _angular_common_http__WEBPACK_IMPORTED_MODULE_15__["HttpClient"] }, { type: Document, decorators: [{
                type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Inject"],
                args: [_angular_common__WEBPACK_IMPORTED_MODULE_7__["DOCUMENT"]]
            }] }]; }, { onPageSubmit: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/accessorial-fees/master/master.component.ts":
/*!*************************************************************!*\
  !*** ./src/app/accessorial-fees/master/master.component.ts ***!
  \*************************************************************/
/*! exports provided: MasterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterComponent", function() { return MasterComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../service/accessorialfees.service */ "./src/app/accessorial-fees/service/accessorialfees.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../create/create-accessorial-fees.component */ "./src/app/accessorial-fees/create/create-accessorial-fees.component.ts");
/* harmony import */ var _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../view/view-accessorial-fees.component */ "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts");






function MasterComponent_app_create_accessorial_fees_12_Template(rf, ctx) { if (rf & 1) {
    const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "app-create-accessorial-fees", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onPageSubmit", function MasterComponent_app_create_accessorial_fees_12_Template_app_create_accessorial_fees_onPageSubmit_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r3); const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r2.onCreateFees($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function MasterComponent_app_view_accessorial_fees_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-view-accessorial-fees");
} }
class MasterComponent {
    constructor(accessorialFeeService) {
        this.accessorialFeeService = accessorialFeeService;
        this.viewType = 0;
    }
    ngOnInit() {
        let _viewType = localStorage.getItem("fuelSurchargeTabId");
        if (_viewType && +_viewType > 0) {
            this.viewType = +_viewType;
        }
        this.accessorialFeeService.onSelectedTabChanged.subscribe(s => {
            if (s == 2) {
                this.viewType = 2;
            }
            else {
                this.viewType = 1;
            }
        });
        this.viewType = +_viewType;
    }
    ngAfterViewInit() {
        this.changeViewType(this.viewType);
    }
    changeViewType(value) {
        localStorage.setItem("fuelSurchargeTabId", value.toString());
        this.viewType = value;
        this.accessorialFeeService.onSelectedAccessorialFeeId.next(null);
        this.accessorialFeeService.onSelectedTabChanged.next(value);
    }
    onCreateFees(viewType) {
        this.changeViewType(viewType);
    }
}
MasterComponent.ɵfac = function MasterComponent_Factory(t) { return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__["AccessorialFeesService"])); };
MasterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: MasterComponent, selectors: [["app-master"]], decls: 14, vars: 8, consts: [[1, "row"], [1, "col-sm-4"], [1, "d-inline-block", "border", "bg-white", "p-1", "radius-capsule", "shadow-b", "mb-2"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [1, "col-sm-12"], [3, "onPageSubmit", 4, "ngIf"], [4, "ngIf"], [3, "onPageSubmit"]], template: function MasterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_5_listener() { return ctx.changeViewType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Create Accessorial Fees");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_8_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "View Accessorial Fees");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, MasterComponent_app_create_accessorial_fees_12_Template, 1, 0, "app-create-accessorial-fees", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, MasterComponent_app_view_accessorial_fees_13_Template, 1, 0, "app-view-accessorial-fees", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 1)("checked", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "type")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 2);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"], _create_create_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_3__["CreateAccessorialFeesComponent"], _view_view_accessorial_fees_component__WEBPACK_IMPORTED_MODULE_4__["ViewAccessorialFeesComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvbWFzdGVyL21hc3Rlci5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-master',
                templateUrl: './master.component.html',
                styleUrls: ['./master.component.css']
            }]
    }], function () { return [{ type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_1__["AccessorialFeesService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/accessorial-fees/model/accessorial-fees.ts":
/*!************************************************************!*\
  !*** ./src/app/accessorial-fees/model/accessorial-fees.ts ***!
  \************************************************************/
/*! exports provided: ViewAccessorialFeeModel, AccessorialFeeInputModel, AccessorialFeeGridModel, CreateAccessorialFeeModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewAccessorialFeeModel", function() { return ViewAccessorialFeeModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AccessorialFeeInputModel", function() { return AccessorialFeeInputModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AccessorialFeeGridModel", function() { return AccessorialFeeGridModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateAccessorialFeeModel", function() { return CreateAccessorialFeeModel; });
class ViewAccessorialFeeModel {
    constructor() {
        this.TableTypes = [];
        this.Customers = [];
        this.Carriers = [];
        this.SourceRegions = [];
        this.TerminalsAndBulkPlants = [];
    }
}
class AccessorialFeeInputModel {
}
class AccessorialFeeGridModel {
}
class CreateAccessorialFeeModel {
    constructor() {
        this.CustomerIds = [];
        this.CarrierIds = [];
        this.SourceRegionIds = [];
        this.TerminalsAndBulkPlants = [];
        this.Fees = [];
        this.CustomerIds = [];
        this.CarrierIds = [];
        this.SourceRegionIds = [];
        this.Fees = [];
    }
}


/***/ }),

/***/ "./src/app/accessorial-fees/service/accessorialfees.service.ts":
/*!*********************************************************************!*\
  !*** ./src/app/accessorial-fees/service/accessorialfees.service.ts ***!
  \*********************************************************************/
/*! exports provided: AccessorialFeesService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AccessorialFeesService", function() { return AccessorialFeesService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class AccessorialFeesService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.archiveAccessorialFeeUrl = '/AccessorialFees/ArchiveAccessorialFee';
        this.getViewAccessorialFeeSummaryUrl = '/AccessorialFees/GetViewAccessorialFeeSummary';
        this.getAccessorialFeeUrl = '/AccessorialFees/GetAccessorialFee?accessorialFeeId=';
        this.postCreateAccesorialFeesUrl = '/AccessorialFees/CreateAccessorialFee';
        this.postUpdateAccesorialFeesUrl = '/AccessorialFees/UpdateAccessorialFee';
        this.onSelectedTabChanged = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1);
        this.onSelectedAccessorialFeeId = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](null);
    }
    getAccessorialFeeGridDetails(filter) {
        return this.httpClient.post(this.getViewAccessorialFeeSummaryUrl, filter)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAccessorialFeeGridDetails', null)));
    }
    archiveAccessorialFee(accessorialFeeId) {
        return this.httpClient.post(this.archiveAccessorialFeeUrl, { accessorialFeeId: accessorialFeeId })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('archiveAccessorialFee', null)));
    }
    createAccessorialFee(accessorialFeeModel) {
        return this.httpClient.post(this.postCreateAccesorialFeesUrl, { model: accessorialFeeModel })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('createAccessorialFee', null)));
    }
    updateAccessorialFee(accessorialFeeModel) {
        return this.httpClient.post(this.postUpdateAccesorialFeesUrl, { model: accessorialFeeModel })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('updateAccessorialFee', null)));
    }
    getAccessorialFee(accessorialFeeId) {
        return this.httpClient.get(this.getAccessorialFeeUrl + accessorialFeeId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAccessorialFee', null)));
    }
}
AccessorialFeesService.ɵfac = function AccessorialFeesService_Factory(t) { return new (t || AccessorialFeesService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
AccessorialFeesService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AccessorialFeesService, factory: AccessorialFeesService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AccessorialFeesService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/accessorial-fees/view/view-accessorial-fees.component.ts":
/*!**************************************************************************!*\
  !*** ./src/app/accessorial-fees/view/view-accessorial-fees.component.ts ***!
  \**************************************************************************/
/*! exports provided: ViewAccessorialFeesComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewAccessorialFeesComponent", function() { return ViewAccessorialFeesComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_my_localstorage__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/my.localstorage */ "./src/app/my.localstorage.ts");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _model_accessorial_fees__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../model/accessorial-fees */ "./src/app/accessorial-fees/model/accessorial-fees.ts");
/* harmony import */ var _view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./view-fees-details/view-fees-details.component */ "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! src/app/company-addresses/region/service/region.service */ "./src/app/company-addresses/region/service/region.service.ts");
/* harmony import */ var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! src/app/fuelsurcharge/services/fuelsurcharge.service */ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
/* harmony import */ var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../service/accessorialfees.service */ "./src/app/accessorial-fees/service/accessorialfees.service.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! angular-confirmation-popover */ "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! angular2-multiselect-dropdown */ "./node_modules/angular2-multiselect-dropdown/__ivy_ngcc__/fesm2015/angular2-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");























const _c0 = function (a0) { return { "d-block": a0 }; };
function ViewAccessorialFeesComponent_tr_38_td_15_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](6, _c0, !fee_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](2, 2, fee_r5.Terminal, 0, 40), "...");
} }
function ViewAccessorialFeesComponent_tr_38_td_15_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, ViewAccessorialFeesComponent_tr_38_td_15_div_3_Template, 3, 8, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_tr_38_td_15_Template_a_click_4_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16); const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; return fee_r5.IsShowMore = !fee_r5.IsShowMore; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "View More/Less");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](3, _c0, fee_r5.IsShowMore));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.Terminal);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", fee_r5.Terminal.length > 40);
} }
function ViewAccessorialFeesComponent_tr_38_td_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.Terminal);
} }
function ViewAccessorialFeesComponent_tr_38_a_20_Template(rf, ctx) { if (rf & 1) {
    const _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("cancel", function ViewAccessorialFeesComponent_tr_38_a_20_Template_a_cancel_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2); return ctx_r19.cancelClicked = true; })("confirm", function ViewAccessorialFeesComponent_tr_38_a_20_Template_a_confirm_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r20); const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r21.archiveAccessorialFee(fee_r5.Id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("popoverTitle", ctx_r9.popoverTitle)("popoverMessage", ctx_r9.popoverMessage);
} }
function ViewAccessorialFeesComponent_tr_38_a_21_Template(rf, ctx) { if (rf & 1) {
    const _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_tr_38_a_21_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r25); const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r23.viewAccessorialFee(fee_r5.Id, "EDIT"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewAccessorialFeesComponent_tr_38_a_24_Template(rf, ctx) { if (rf & 1) {
    const _r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_tr_38_a_24_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r28); const fee_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit; const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r26.viewAccessorialFee(fee_r5.Id, "COPY"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewAccessorialFeesComponent_tr_38_Template(rf, ctx) { if (rf & 1) {
    const _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 30);
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
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, ViewAccessorialFeesComponent_tr_38_td_15_Template, 6, 5, "td", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, ViewAccessorialFeesComponent_tr_38_td_16_Template, 2, 1, "td", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, ViewAccessorialFeesComponent_tr_38_a_20_Template, 2, 2, "a", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, ViewAccessorialFeesComponent_tr_38_a_21_Template, 2, 0, "a", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "a", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_tr_38_Template_a_click_22_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30); const fee_r5 = ctx.$implicit; const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r29.viewAccessorialFee(fee_r5.Id, "VIEW"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](23, "i", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, ViewAccessorialFeesComponent_tr_38_a_24_Template, 2, 0, "a", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const fee_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.DateRange);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.TableType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.TableName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.StatusName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.Customer);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.Carrier);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.SourceRegion);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", fee_r5.Terminal.length > 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", fee_r5.Terminal.length <= 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](fee_r5.BulkPlant);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !fee_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !fee_r5.IsArchived);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !fee_r5.IsArchived);
} }
function ViewAccessorialFeesComponent_div_48_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewAccessorialFeesComponent_ng_template_49_div_7_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Table Type is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} }
function ViewAccessorialFeesComponent_ng_template_49_div_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, ViewAccessorialFeesComponent_ng_template_49_div_7_div_1_Template, 2, 0, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r31.viewAccessorialFeeForm.get("TableTypes").errors.required);
} }
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
function ViewAccessorialFeesComponent_ng_template_49_Template(rf, ctx) { if (rf & 1) {
    const _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "label", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, "Table Type");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "ng-multiselect-dropdown", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r33.onTableTypeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, ViewAccessorialFeesComponent_ng_template_49_div_7_Template, 2, 1, "div", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Customer(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "ng-multiselect-dropdown", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r35.onCustomersSelect($event); })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r36.onCustomersDeSelect($event); })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_13_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r37.onCustomersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Carrier(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r38.onCarriersSelect($event); })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r39.onCarriersDeSelect($event); })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r40.onCarriersDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "label", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Source Region(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "ng-multiselect-dropdown", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onSelect_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r41.onSourceRegionsSelect($event); })("onDeSelect", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelect_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r42.onSourceRegionsDeSelect($event); })("onDeSelectAll", function ViewAccessorialFeesComponent_ng_template_49_Template_ng_multiselect_dropdown_onDeSelectAll_24_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r43.onSourceRegionsDeSelectAll($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Terminal(s)/BulkPlant(s)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](29, "angular2-multiselect", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "From");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "input", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewAccessorialFeesComponent_ng_template_49_Template_input_onDateChange_34_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r44.setfromDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "To");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "input", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function ViewAccessorialFeesComponent_ng_template_49_Template_input_onDateChange_39_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r45.settoDate($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](43, "input", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "label", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, " Show Archived ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "button", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_ng_template_49_Template_button_click_47_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r46.clearFilter(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48, "Clear Filter");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "button", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_ng_template_49_Template_button_click_49_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r34); const ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4); ctx_r47.filterGrid(); return _r0.close(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](50, "Apply");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx_r4.SinlgeselectSettingsById)("data", ctx_r4.TableTypeList)("placeholder", "Select Type (Required)");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.viewAccessorialFeeForm.get("TableTypes").invalid && ctx_r4.viewAccessorialFeeForm.get("TableTypes").touched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](22, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CustomerList)("placeholder", "Select Customers");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](24, _c1, ctx_r4.IsMasterSelected));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("settings", ctx_r4.MultiselectSettingsById)("data", ctx_r4.CarrierList)("placeholder", "Select Carriers");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx_r4.SourceRegionList)("settings", ctx_r4.MultiselectSettingsById)("placeholder", "Select Source Regions");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("data", ctx_r4.TerminalsAndBulkPlantList)("settings", ctx_r4.MultiSelectSettingsByGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("minDate", ctx_r4.minDate)("maxDate", ctx_r4.maxDate)("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](26, _c1, ctx_r4.IsLoading));
} }
class ViewAccessorialFeesComponent {
    constructor(fb, regionService, fuelsurchargeService, accessorialFeeService, cdr) {
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
    ngOnInit() {
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
    ngOnDestroy() {
        this.rerender_destroy();
    }
    ngAfterViewInit() {
        this.getAccessorialFeeGridDetails();
    }
    createForm() {
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
    archiveAccessorialFee(accessorialFeeId) {
        this.IsLoading = true;
        this.accessorialFeeService.archiveAccessorialFee(accessorialFeeId)
            .subscribe((response) => {
            this.IsLoading = false;
            //this.serviceResponse = response;
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess('Selected accessorial fee archived successfully', undefined, undefined);
                this.filterGrid();
            }
        });
    }
    onTableTypeSelect(item) {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewAccessorialFeeForm.get('Carriers').patchValue([]);
        this.viewAccessorialFeeForm.get('Customers').patchValue([]);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                break;
            case 2: // customer
                this.IsCustomerSelected = true;
                this.getSupplierCustomers();
                this.getCarriers();
                break;
            case 3: //carrier
                this.IsCarrierSelected = true;
                this.getCarriers();
                this.getSupplierCustomers();
                break;
        }
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    onCarriersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCarriersDeSelect(item) {
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCustomersSelect(item) {
        this.onCarriersOrCustomersChange();
    }
    onCustomersDeSelect(item) {
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }
    onCarriersOrCustomersChange() {
        var selectedTableType = this.viewAccessorialFeeForm.get('TableTypes').value;
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    getTableTypes() {
        this.IsLoading = true;
        this.fuelsurchargeService.getTableTypes().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.TableTypeList = yield data;
        }));
    }
    getCarriers() {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe((carriers) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CarrierList = yield carriers;
            this.SourceRegionList = null;
            this.IsLoading = false;
        }));
    }
    getSupplierCustomers() {
        this.IsLoading = true;
        this.fuelsurchargeService.getSupplierCustomers().subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.CustomerList = yield data;
            this.IsLoading = false;
        }));
    }
    getSourceRegions(tableType) {
        this.IsLoading = true;
        let customerIds = [];
        let carrierIds = [];
        let selectedCustomers = this.viewAccessorialFeeForm.get('Customers').value;
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }
        let selectedCarriers = this.viewAccessorialFeeForm.get('Carriers').value;
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }
        var sourceRegionInput = new src_app_app_enum__WEBPACK_IMPORTED_MODULE_8__["SourceRegionInputModel"]();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(data => {
            this.SourceRegionList = data;
            this.IsLoading = false;
        });
    }
    getTerminalsBulkPlant() {
        this.IsLoading = true;
        var selectedSourceRegions = this.viewAccessorialFeeForm.get('SourceRegions').value;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe((data) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
                this.TerminalsAndBulkPlantList = yield (data);
                this.IsLoading = false;
            }));
        }
    }
    onSourceRegionsDeSelect(item) {
        this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
    }
    onSourceRegionsDeSelectAll(item) {
        this.IsSourceRegionSelected = false;
    }
    onSourceRegionsSelect(item) {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
    }
    filterGrid() {
        $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
        this.getAccessorialFeeGridDetails();
    }
    clearFilter() {
        this.clearForm();
        this.getAccessorialFeeGridDetails();
    }
    clearForm() {
        this.viewAccessorialFeeForm.reset();
        $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }
    getAccessorialFeeGridDetails() {
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
            var tableTypeIds = selectedTableTypes.map(s => s.Id);
            input.TableTypeIds = tableTypeIds.join(',');
        }
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            var customerIds = selectedCustomers.map(s => s.Id);
            input.CustomerIds = customerIds.join(',');
        }
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            var carrierIds = selectedCarriers.map(s => s.Id);
            input.CarrierIds = carrierIds.join(',');
        }
        if (selectedSourceRegions != null && selectedSourceRegions != undefined && selectedSourceRegions.length > 0) {
            var sourceRegionIds = selectedSourceRegions.map(s => s.Id);
            input.SourceRegionIds = sourceRegionIds.join(',');
        }
        if (selectedTerminalAndBulkPlants != null && selectedTerminalAndBulkPlants != undefined && selectedTerminalAndBulkPlants.length > 0) {
            var selectedTerminalIds = selectedTerminalAndBulkPlants.filter(c => c.Code == "Terminals");
            var terminalIds = selectedTerminalIds.map(s => s.Id);
            input.TerminalIds = terminalIds.join(',');
            var selectedBulkPlants = selectedTerminalAndBulkPlants.filter(c => c.Code == "BulkPlants");
            var bulkPlantIds = selectedBulkPlants.map(s => s.Id);
            input.BulkPlantIds = bulkPlantIds.join(',');
        }
        this.IsLoading = true;
        this.accessorialFeeService.getAccessorialFeeGridDetails(input).subscribe(data => {
            this.IsLoading = false;
            if (data && data.length > 0) {
                this.AccessorialFeeList = data;
            }
            this.dtTrigger.next();
        });
    }
    rerender_destroy() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                dtInstance.destroy();
            });
        }
    }
    rerender_trigger() {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                this.dtTrigger.next();
            });
        }
    }
    viewAccessorialFee(accessorialFeeId, mode) {
        let operation = { AccessorialFeeId: accessorialFeeId, Mode: mode };
        this.accessorialFeeService.onSelectedAccessorialFeeId.next(JSON.stringify(operation));
        this.accessorialFeeService.onSelectedTabChanged.next(1);
    }
    setfromDate($event) {
        this.viewAccessorialFeeForm.controls.fromDate.setValue($event);
    }
    settoDate($event) {
        this.viewAccessorialFeeForm.controls.toDate.setValue($event);
    }
    initializeAccessorialFeeDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Accessorial Fee', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Accessorial Fee', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
}
ViewAccessorialFeesComponent.ɵfac = function ViewAccessorialFeesComponent_Factory(t) { return new (t || ViewAccessorialFeesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__["AccessorialFeesService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"])); };
ViewAccessorialFeesComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: ViewAccessorialFeesComponent, selectors: [["app-view-accessorial-fees"]], viewQuery: function ViewAccessorialFeesComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.accessorialFeeComponent = _t.first);
    } }, decls: 51, vars: 7, consts: [[3, "formGroup"], [1, "row"], [1, "col-sm-12", "text-left"], ["placement", "bottom", "container", "body", "triggers", "manual", "popoverClass", "master-filter", 1, "fs16", "mr10", "filter-link", "pa", 3, "ngbPopover", "autoClose", "click"], ["p", "ngbPopover"], [1, "fas", "fa-filter", "mr5", "ml20", "pr"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-accessorial-fee-grid", 1, "table-responsive"], ["id", "accessorial-fee-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "DateRange"], ["data-key", "TableType"], ["data-key", "TableName"], ["data-key", "StatusName"], ["data-key", "Customer"], ["data-key", "Carrier"], ["data-key", "SourceRegion"], ["data-key", "Terminal"], ["data-key", "BulkPlant"], [4, "ngFor", "ngForOf"], ["id", "fee-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], ["class", "loader", 4, "ngIf"], ["popContent", ""], [1, "text-center"], [4, "ngIf"], [1, "text-center", "text-nowrap"], ["class", "btn btn-link fs16 mr-1", "mwlConfirmationPopover", "", "placement", "left", 3, "popoverTitle", "popoverMessage", "cancel", "confirm", 4, "ngIf"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Edit", 3, "click", 4, "ngIf"], ["placement", "bottom", "ngbTooltip", "View", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-street-view"], ["class", "btn btn-link fs16 ml-0", "placement", "bottom", "ngbTooltip", "Copy", 3, "click", 4, "ngIf"], [1, "d-none", 3, "ngClass"], ["class", "d-none", 3, "ngClass", 4, "ngIf"], [3, "click"], ["mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-link", "fs16", "mr-1", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"], ["placement", "bottom", "ngbTooltip", "Archive", 1, "fa", "fa-trash-alt", "color-maroon"], ["placement", "bottom", "ngbTooltip", "Edit", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-edit"], ["placement", "bottom", "ngbTooltip", "Copy", 1, "btn", "btn-link", "fs16", "ml-0", 3, "click"], [1, "fas", "fa-copy"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "popover-details"], [1, "col-sm-6"], [1, "form-group"], ["for", "TableTypes"], ["formControlName", "TableTypes", "id", "TableTypes", 3, "settings", "data", "placeholder", "onSelect"], ["class", "color-maroon", 4, "ngIf"], [3, "ngClass"], ["for", "Customers"], ["formControlName", "Customers", "id", "Customers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "Carriers"], ["formControlName", "Carriers", "id", "Carriers", 3, "settings", "data", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], ["for", "SourceRegions"], ["formControlName", "SourceRegions", "id", "SourceRegions", 3, "data", "settings", "placeholder", "onSelect", "onDeSelect", "onDeSelectAll"], [1, "col-sm-12"], ["for", "TerminalsAndBulkPlants"], ["id", "TerminalsAndBulkPlants", "formControlName", "TerminalsAndBulkPlants", 3, "data", "settings"], ["type", "text", "placeholder", "Select Date", "formControlName", "fromDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Select Date", "formControlName", "toDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-6", "form-group"], [1, "form-check"], ["formControlName", "isArchived", "type", "checkbox", "value", "", "id", "ckb-isArchived", 1, "form-check-input"], ["for", "ckb-isArchived", 1, "form-check-label"], [1, "col-sm-6", "text-right", "form-buttons", "mt20"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], ["id", "filter-accessorial-fee-grid", "type", "button", 1, "btn", "btn-lg", "btn-primary", "mt3", "valid", 3, "ngClass", "click"], [1, "color-maroon"]], template: function ViewAccessorialFeesComponent_Template(rf, ctx) { if (rf & 1) {
        const _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 3, 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewAccessorialFeesComponent_Template_a_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r48); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](4); return _r0.open(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Filters");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "table", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Date Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Table Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Table Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Customer(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Carrier(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Source Region(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Terminal(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Bulk Plant(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, ViewAccessorialFeesComponent_tr_38_Template, 25, 13, "tr", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "div", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "a", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](43, "i", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "h3", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, "Fee Details");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](47, "app-view-fees-details");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](48, ViewAccessorialFeesComponent_div_48_Template, 5, 0, "div", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](49, ViewAccessorialFeesComponent_ng_template_49_Template, 51, 28, "ng-template", null, 29, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    } if (rf & 2) {
        const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](50);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.viewAccessorialFeeForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngbPopover", _r3)("autoClose", "outside");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.AccessorialFeeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__["NgbPopover"], angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgForOf"], _view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgIf"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__["NgbTooltip"], _angular_common__WEBPACK_IMPORTED_MODULE_14__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__["ɵc"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], angular2_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_17__["AngularMultiSelect"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_18__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["CheckboxControlValueAccessor"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_14__["SlicePipe"]], styles: [".master-filter.popover {\r\n    min-width: 425px;\r\n    max-width: 450px;\r\n    background: #F9F9F9;\r\n    border: 1px solid #E9E7E7;\r\n    box-sizing: border-box;\r\n    box-shadow: 10px 10px 8px -2px rgb(0, 0, 0, 0.13);\r\n    border-radius: 10px;\r\n}\r\n\r\n      .master-filter.popover .popover-body {\r\n        padding: 0;\r\n        border-radius: 5px;\r\n        background: #ffffff;\r\n    }\r\n\r\n      .master-filter.popover .popover-details {\r\n        padding: 15px;\r\n    }\r\n\r\n      .master-filter.popover .border-bottom-2 {\r\n        border-bottom: 2px solid #e7eaec !important;\r\n    }\r\n\r\n    .filter-link[_ngcontent-%COMP%] {\r\n    top: -45px;\r\n    left: 380px\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYWNjZXNzb3JpYWwtZmVlcy92aWV3L3ZpZXctYWNjZXNzb3JpYWwtZmVlcy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksZ0JBQWdCO0lBQ2hCLGdCQUFnQjtJQUNoQixtQkFBbUI7SUFDbkIseUJBQXlCO0lBQ3pCLHNCQUFzQjtJQUN0QixpREFBaUQ7SUFDakQsbUJBQW1CO0FBQ3ZCOztJQUVJO1FBQ0ksVUFBVTtRQUNWLGtCQUFrQjtRQUNsQixtQkFBbUI7SUFDdkI7O0lBRUE7UUFDSSxhQUFhO0lBQ2pCOztJQUVBO1FBQ0ksMkNBQTJDO0lBQy9DOztJQUVKO0lBQ0ksVUFBVTtJQUNWO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9hY2Nlc3NvcmlhbC1mZWVzL3ZpZXcvdmlldy1hY2Nlc3NvcmlhbC1mZWVzLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyI6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciB7XHJcbiAgICBtaW4td2lkdGg6IDQyNXB4O1xyXG4gICAgbWF4LXdpZHRoOiA0NTBweDtcclxuICAgIGJhY2tncm91bmQ6ICNGOUY5Rjk7XHJcbiAgICBib3JkZXI6IDFweCBzb2xpZCAjRTlFN0U3O1xyXG4gICAgYm94LXNpemluZzogYm9yZGVyLWJveDtcclxuICAgIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2IoMCwgMCwgMCwgMC4xMyk7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG59XHJcblxyXG4gICAgOjpuZy1kZWVwIC5tYXN0ZXItZmlsdGVyLnBvcG92ZXIgLnBvcG92ZXItYm9keSB7XHJcbiAgICAgICAgcGFkZGluZzogMDtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgIH1cclxuXHJcbiAgICA6Om5nLWRlZXAgLm1hc3Rlci1maWx0ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcclxuICAgICAgICBwYWRkaW5nOiAxNXB4O1xyXG4gICAgfVxyXG5cclxuICAgIDo6bmctZGVlcCAubWFzdGVyLWZpbHRlci5wb3BvdmVyIC5ib3JkZXItYm90dG9tLTIge1xyXG4gICAgICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZTdlYWVjICFpbXBvcnRhbnQ7XHJcbiAgICB9XHJcblxyXG4uZmlsdGVyLWxpbmsge1xyXG4gICAgdG9wOiAtNDVweDtcclxuICAgIGxlZnQ6IDM4MHB4XHJcbn1cclxuIl19 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](ViewAccessorialFeesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-view-accessorial-fees',
                templateUrl: './view-accessorial-fees.component.html',
                styleUrls: ['./view-accessorial-fees.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"] }, { type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_10__["RegionService"] }, { type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_11__["FuelSurchargeService"] }, { type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_12__["AccessorialFeesService"] }, { type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectorRef"] }]; }, { datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"]]
        }], accessorialFeeComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [_view_fees_details_view_fees_details_component__WEBPACK_IMPORTED_MODULE_7__["ViewFeesDetailsComponent"]]
        }] }); })();


/***/ }),

/***/ "./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts":
/*!****************************************************************************************!*\
  !*** ./src/app/accessorial-fees/view/view-fees-details/view-fees-details.component.ts ***!
  \****************************************************************************************/
/*! exports provided: ViewFeesDetailsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewFeesDetailsComponent", function() { return ViewFeesDetailsComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../service/accessorialfees.service */ "./src/app/accessorial-fees/service/accessorialfees.service.ts");




//import { FeesDetailModel, AccessorialFuelFeeModel } from '../../model/accessorial-fees';
class ViewFeesDetailsComponent {
    constructor(fb, accessorialFeeService) {
        this.fb = fb;
        this.accessorialFeeService = accessorialFeeService;
    }
    //public AccessorialFuelFee: AccessorialFuelFeeModel;
    //public FeesDetailList: FeesDetailModel[]
    ngOnInit() {
    }
    getAccessorialFeesDetails(accessorialFeeId) {
        //this.accessorialFeeService.getAccessorialFee(accessorialFeeId).subscribe(data => {
        //    this.AccessorialFuelFee = data as AccessorialFuelFeeModel;
        //    this.FeesDetailList = this.AccessorialFuelFee.FuelFees;
        //});
    }
}
ViewFeesDetailsComponent.ɵfac = function ViewFeesDetailsComponent_Factory(t) { return new (t || ViewFeesDetailsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__["AccessorialFeesService"])); };
ViewFeesDetailsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: ViewFeesDetailsComponent, selectors: [["app-view-fees-details"]], decls: 0, vars: 0, template: function ViewFeesDetailsComponent_Template(rf, ctx) { }, styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FjY2Vzc29yaWFsLWZlZXMvdmlldy92aWV3LWZlZXMtZGV0YWlscy92aWV3LWZlZXMtZGV0YWlscy5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ViewFeesDetailsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-view-fees-details',
                templateUrl: './view-fees-details.component.html',
                styleUrls: ['./view-fees-details.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _service_accessorialfees_service__WEBPACK_IMPORTED_MODULE_2__["AccessorialFeesService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/company-addresses/region/service/region.service.ts":
/*!********************************************************************!*\
  !*** ./src/app/company-addresses/region/service/region.service.ts ***!
  \********************************************************************/
/*! exports provided: RegionService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RegionService", function() { return RegionService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class RegionService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.createUrl = '/Region/Create';
        this.updateUrl = '/Region/Update';
        this.deleteUrl = '/Region/Delete?id=';
        this.getRegionsUrl = '/Region/GetRegions';
        this.getSourceRegionsUrl = '/Region/GetSourceRegions';
        this.createSourceRegionUrl = '/Region/CreateSourceRegion';
        this.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        this.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        this.getJobsUrl = '/Region/GetJobs';
        this.getDriversUrl = '/Region/GetDrivers';
        this.getDispatchersUrl = '/Region/GetDispatchers';
        this.getTrailersUrl = '/Region/GetTrailers';
        this.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        this.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        this.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        this.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        this.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        this.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        this.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        this.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        this.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        this.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        this.getCarriersUrl = '/Region/GetCarriers';
        this.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        this.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        this.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        this.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        this.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        this.getFuelProductUrl = '/Region/GetMstFuelProducts';
        this.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        this.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
    }
    getJobs() {
        return this.httpClient.get(this.getJobsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobs', [])));
    }
    getDrivers() {
        return this.httpClient.get(this.getDriversUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
    }
    getRegionDrivers(regiondId) {
        return this.httpClient.get(this.getRegionDriversUrl + regiondId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
    }
    getCompanyShifts() {
        return this.httpClient.get(this.getCompanyShiftsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCompanyShifts', [])));
    }
    getRoutesByRegion(regionId) {
        return this.httpClient.get(this.getRouteByReginId + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetRouteInfoDetails', [])));
    }
    getDispatchers() {
        return this.httpClient.get(this.getDispatchersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDispatchers', [])));
    }
    getTrailers() {
        return this.httpClient.get(this.getTrailersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTrailers', [])));
    }
    getRegions() {
        return this.httpClient.get(this.getRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegions', null)));
    }
    createRegion(region) {
        return this.httpClient.post(this.createUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRegion', region)));
    }
    updateRegion(region) {
        return this.httpClient.post(this.updateUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateRegion', region)));
    }
    getSourceRegions() {
        return this.httpClient.get(this.getSourceRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSourceRegions', null)));
    }
    createSourceRegion(region) {
        return this.httpClient.post(this.createSourceRegionUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createSourceRegion', region)));
    }
    isSourceRegionAvailable(name, id) {
        return this.httpClient.get(this.isSourceRegionAvailableUrl + name + "&id=" + id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isSourceRegionAvailable', null)));
    }
    updateSourceRegion(region) {
        return this.httpClient.post(this.updateSourceRegionUrl, region, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateSourceRegion', region)));
    }
    deleteRegion(id) {
        return this.httpClient.post(this.deleteUrl + id, id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRegion', id)));
    }
    deleteSourceRegion(id) {
        return this.httpClient.post(this.deleteSourceRegionUrl + id, id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteSourceRegion', id)));
    }
    getStates(countryId) {
        return this.httpClient.get(this.stateUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getStates', [])));
    }
    //for calender
    getShiftByDrivers(driverIds, scheduleType) {
        return this.httpClient.get(this.shiftByDriverUrl + driverIds + "&scheduleType=" + scheduleType)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getShiftByDrivers', [])));
    }
    getSchedulesByRegion(regionId, scheduleType) {
        return this.httpClient.get(this.getRegionSchedulsbyRegionIdUrl + regionId + "&scheduleType=" + scheduleType)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSchedulesByRegion', [])));
    }
    getRegionSchedule(regionId, routeId) {
        return this.httpClient.get(this.getRegionShiftMapping + regionId + "&routeId=" + routeId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegionSchedule', [])));
    }
    addDriverSchedule(model) {
        return this.httpClient.post(this.addDriverScheduleUrl, model, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', model)));
    }
    addRegionSchedule(model) {
        return this.httpClient.post(this.addRegionScheduleUrl, model, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRegionSchedule', model)));
    }
    updateDriverSchedule(data, date) {
        var postModel = { model: data, SelectedDate: date };
        return this.httpClient.post(this.updateDriverScheduleUrl, postModel, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', postModel)));
    }
    deleteDriverSchedule(data, date) {
        var postModel = { driverScheduleMappingViewModels: data, SelectedDate: date };
        return this.httpClient.post(this.deleteDriverScheduleUrl, postModel, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteDriverSchedule', postModel)));
    }
    getCarriers() {
        return this.httpClient.get(this.getCarriersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriers', [])));
    }
    getCarrierRegions() {
        return this.httpClient.get(this.getCarrierRegionsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierRegions', null)));
    }
    getSelectedCarriersByRegion(regionId) {
        return this.httpClient.get(this.getSelectedCarriersByRegionUrl + regionId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSelectedCarriersByRegion', null)));
    }
    getProductType() {
        return this.httpClient.get(this.getProductTypeUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductType', [])));
    }
    getFuelProducts() {
        return this.httpClient.get(this.getFuelProductUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelProducts', [])));
    }
    isPublishedDR(productIds, fuelTypeIds) {
        return this.httpClient.get(this.isPublishedDRUrl + productIds + "&fuelTypeIds=" + fuelTypeIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isPublishedDR', null)));
    }
}
RegionService.ɵfac = function RegionService_Factory(t) { return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"])); };
RegionService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: RegionService, factory: RegionService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/invoice/services/fee.service.ts":
/*!*************************************************!*\
  !*** ./src/app/invoice/services/fee.service.ts ***!
  \*************************************************/
/*! exports provided: FeeService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FeeService", function() { return FeeService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class FeeService extends _errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getFeeTypesUrl = '/Supplier/Invoice/GetFeeTypes?orderId=';
        this.getFeeSubTypesUrl = '/Supplier/Invoice/GetFeeSubTypes?orderId=';
        this.getFeeConstraintTypesUrl = '/Supplier/Invoice/GetFeeConstraintTypes';
        this.getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';
    }
    getFeeTypes(orderId, isFromAccesorialFees) {
        return this.httpClient.get(this.getFeeTypesUrl + orderId + '&isFromAccesorialFees=' + isFromAccesorialFees)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeTypes', [])));
    }
    getFeeSubTypes(orderId) {
        return this.httpClient.get(this.getFeeSubTypesUrl + orderId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeSubTypes', [])));
    }
    getFeeConstraintTypes() {
        return this.httpClient.get(this.getFeeConstraintTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getFeeConstraintTypes', [])));
    }
    getEiaPrice(data) {
        return this.httpClient.post(this.getEiaPriceUrl, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getEiaPrice', [])));
    }
}
FeeService.ɵfac = function FeeService_Factory(t) { return new (t || FeeService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
FeeService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: FeeService, factory: FeeService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FeeService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ })

}]);
//# sourceMappingURL=accessorial-fees-accessorial-fees-module-es2015.js.map