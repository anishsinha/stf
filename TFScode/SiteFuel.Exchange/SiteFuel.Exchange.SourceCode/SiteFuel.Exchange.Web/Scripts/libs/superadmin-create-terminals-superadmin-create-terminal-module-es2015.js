(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["superadmin-create-terminals-superadmin-create-terminal-module"],{

/***/ "./src/app/superadmin-create-terminals/create-terminals/create-terminal.component.ts":
/*!*******************************************************************************************!*\
  !*** ./src/app/superadmin-create-terminals/create-terminals/create-terminal.component.ts ***!
  \*******************************************************************************************/
/*! exports provided: CreateTerminalComponent, MapConstants, MapIconUrl, MapIconSize */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateTerminalComponent", function() { return CreateTerminalComponent; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MapConstants", function() { return MapConstants; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MapIconUrl", function() { return MapIconUrl; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MapIconSize", function() { return MapIconSize; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _models__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./../models */ "./src/app/superadmin-create-terminals/models.ts");
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../marine-ports-vessels/marine-portsandvessels.service */ "./src/app/marine-ports-vessels/marine-portsandvessels.service.ts");
/* harmony import */ var _sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../sales-user/sales-user.service */ "./src/app/sales-user/sales-user.service.ts");
/* harmony import */ var _shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../shared-components/confirmation-dialog/confirmation-dialog.service */ "./src/app/shared-components/confirmation-dialog/confirmation-dialog.service.ts");
/* harmony import */ var _createterminals_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./../createterminals.service */ "./src/app/superadmin-create-terminals/createterminals.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _agm_core__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @agm/core */ "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");














function CreateTerminalComponent_tr_36_span_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.Abbreviation);
} }
function CreateTerminalComponent_tr_36_span_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_span_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.ControlNumber);
} }
function CreateTerminalComponent_tr_36_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_span_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.Address);
} }
function CreateTerminalComponent_tr_36_span_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.City);
} }
function CreateTerminalComponent_tr_36_span_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_span_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.StateCode);
} }
function CreateTerminalComponent_tr_36_span_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_span_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.TerminalOwner);
} }
function CreateTerminalComponent_tr_36_span_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_tr_36_Template(rf, ctx) { if (rf & 1) {
    const _r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](4, CreateTerminalComponent_tr_36_span_4_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, CreateTerminalComponent_tr_36_span_5_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](7, CreateTerminalComponent_tr_36_span_7_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, CreateTerminalComponent_tr_36_span_8_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](10, CreateTerminalComponent_tr_36_span_10_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](11, CreateTerminalComponent_tr_36_span_11_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](13, CreateTerminalComponent_tr_36_span_13_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](14, CreateTerminalComponent_tr_36_span_14_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, CreateTerminalComponent_tr_36_span_16_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](17, CreateTerminalComponent_tr_36_span_17_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](19, CreateTerminalComponent_tr_36_span_19_Template, 2, 1, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](20, CreateTerminalComponent_tr_36_span_20_Template, 2, 0, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "td", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "button", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CreateTerminalComponent_tr_36_Template_button_click_22_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r38); const terminal_r18 = ctx.$implicit; const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); ctx_r37.createTerminal("Edit Terminal"); return ctx_r37.editTerminal(terminal_r18); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](23, "i", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const terminal_r18 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](terminal_r18.Name);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.Abbreviation != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.Abbreviation == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.ControlNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.ControlNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.Address != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.Address == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.City != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.City == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.StateCode != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.StateCode == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.TerminalOwner != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", terminal_r18.TerminalOwner == null);
} }
function CreateTerminalComponent_div_57_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_57_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_57_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.isRequired("Name"));
} }
function CreateTerminalComponent_div_63_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_63_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_63_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r2.isRequired("Abbreviation"));
} }
function CreateTerminalComponent_div_70_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_70_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_70_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r3.isRequired("ControlNumber"));
} }
function CreateTerminalComponent_div_76_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_76_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_76_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r4.isRequired("TerminalOwner"));
} }
function CreateTerminalComponent_span_89_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_91_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_91_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_91_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r6.isRequired("Address"));
} }
function CreateTerminalComponent_span_96_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_98_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_98_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_98_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r8.isRequired("ZipCode"));
} }
function CreateTerminalComponent_span_104_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_106_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_106_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_106_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.isRequired("City"));
} }
function CreateTerminalComponent_span_111_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_114_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_114_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_114_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r12.isRequired("County"));
} }
function CreateTerminalComponent_option_124_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const item_r47 = ctx.$implicit;
    const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????propertyInterpolate"]("id", item_r47.StateId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", item_r47.StateId)("selected", item_r47.StateId == ctx_r13.terminalCreateForm.get("StateId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", item_r47.StateName, " ");
} }
function CreateTerminalComponent_div_125_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_125_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_125_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r14.isRequired("StateId"));
} }
function CreateTerminalComponent_div_137_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_137_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_137_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r15.isRequired("Latitude"));
} }
function CreateTerminalComponent_div_143_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function CreateTerminalComponent_div_143_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CreateTerminalComponent_div_143_div_1_Template, 2, 0, "div", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r16.isRequired("Longitude"));
} }
function CreateTerminalComponent_div_151_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
const _c0 = function (a0) { return { "disabled": a0 }; };
const _c1 = function (a0) { return { "pntr-none subSectionOpacity": a0 }; };
class CreateTerminalComponent {
    constructor(marineService, fb, salesService, confirmationdialogueservice, terminalService) {
        this.marineService = marineService;
        this.fb = fb;
        this.salesService = salesService;
        this.confirmationdialogueservice = confirmationdialogueservice;
        this.terminalService = terminalService;
        this.TerminalDetailsData = [];
        this.TerminalDetail = new _models__WEBPACK_IMPORTED_MODULE_3__["TerminalDetailsModel"]();
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.mapConstants = new MapConstants();
        this.countryList = [];
        this.currucyList = [];
        this.statesList = [];
        this.filteredStatesList = [];
        this.countryGroupList = [];
        this.IsLoading = false;
    }
    ngOnInit() {
        this.ModalText = 'Create Terminal';
        var exportColumns = { columns: [0, 1, 2, 3, 4, 5, 6, 7] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Terminal Details', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Terminal Details', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: true,
            order: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        this.getCountryList();
        this.getcountryGroupList();
        this.getStatesOfAllCountries();
        this.TerminalDetail.CountryId = this.SelectedCountryId;
        if (this.SelectedCountryId == 2) { //canada
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        this.initializeTerminalCreationForm(this.TerminalDetail);
        jQuery("#AddressDetails_Country_Id").val("0").change();
    }
    ngOnChanges(changes) {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            // get call for grid data
            this.getTerminalDetailsData();
            this.TerminalDetail.CountryId = this.SelectedCountryId;
            this.setMapCenter(this.SelectedCountryId);
            this.setAddressValidators(this.SelectedCountryId);
            jQuery("#AddressDetails_Country_Id").val("0").change();
        }
    }
    createTerminal(header) {
        this.ModalText = header;
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.terminalCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.terminalCreateForm.get('IsGeocodeUsed').setValue(false);
        this.setLatLongValidator(false);
        this.setMapCenter(this.SelectedCountryId);
        this.terminalCreateForm.get('StateId').setValue(null);
        jQuery("#AddressDetails_Country_Id").val("0").change();
    }
    setLatLongValidator(isGeoCoded) {
        if (isGeoCoded) {
            let val = [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required];
            this.terminalCreateForm.get('Latitude').setValidators(val);
            this.terminalCreateForm.get('Longitude').setValidators(val);
            this.terminalCreateForm.get('Latitude').updateValueAndValidity();
            this.terminalCreateForm.get('Longitude').updateValueAndValidity();
        }
        else {
            this.terminalCreateForm.get('Latitude').clearValidators();
            this.terminalCreateForm.get('Latitude').updateValueAndValidity();
            this.terminalCreateForm.get('Longitude').clearValidators();
            this.terminalCreateForm.get('Longitude').updateValueAndValidity();
        }
    }
    setMapCenter(countryId) {
        if (countryId == 2) {
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        else if (countryId == 4) {
            this.mapConstants.CenterLat = 13.193887;
            this.mapConstants.CenterLon = -59.543198;
        }
        else {
            this.mapConstants.CenterLat = 38;
            this.mapConstants.CenterLon = -98.35;
        }
    }
    clearPanelData() {
        this.terminalCreateForm.reset();
        this.terminalCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.terminalCreateForm.get('StateId').setValue(null);
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.setMapCenter(this.SelectedCountryId);
        jQuery("#AddressDetails_Country_Id").val("0").change();
        this.setLatLongValidator(false);
    }
    getCountryList() {
        this.marineService.getCountryList().subscribe(data => {
            if (data && data.length > 0) {
                this.countryList = data;
            }
        });
    }
    getcountryGroupList() {
        this.marineService.getCountryGroupList(4).subscribe(data => {
            if (data && data.length > 0) {
                this.countryGroupList = data;
            }
        });
    }
    getStatesOfAllCountries(countryId) {
        this.marineService.GetStatesOfAllCountries(countryId).subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList;
            }
        });
    }
    initializeTerminalCreationForm(terminal) {
        this.terminalCreateForm = this.fb.group({
            Id: this.fb.control(terminal.Id),
            Name: this.fb.control(terminal.Name, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Abbreviation: this.fb.control(terminal.Abbreviation),
            TerminalOwner: this.fb.control(terminal.TerminalOwner),
            ControlNumber: this.fb.control(terminal.ControlNumber),
            Address: this.fb.control(terminal.Address),
            City: this.fb.control(terminal.City),
            County: this.fb.control(terminal.County),
            StateId: this.fb.control(terminal.StateId, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            CountryId: this.fb.control(terminal.CountryId, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            StateName: this.fb.control(terminal.StateCode),
            ZipCode: this.fb.control(terminal.ZipCode),
            IsGeocodeUsed: this.fb.control(terminal.IsGeoCoded),
            Latitude: this.fb.control(terminal.Latitude, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')),
            Longitude: this.fb.control(terminal.Longitude, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')),
        });
        this.setAddressValidators(terminal.CountryId);
        return this.terminalCreateForm;
    }
    setAddressValidators(countryId) {
        if (countryId && this.terminalCreateForm) {
            let validator;
            if (countryId && (countryId == 1 || countryId == 2)) {
                validator = [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required];
            }
            else {
                validator = [];
            }
            this.terminalCreateForm.get('Address').setValidators(validator);
            this.terminalCreateForm.get('Address').updateValueAndValidity();
            this.terminalCreateForm.get('City').setValidators(validator);
            this.terminalCreateForm.get('City').updateValueAndValidity();
            this.terminalCreateForm.get('County').setValidators(validator);
            this.terminalCreateForm.get('County').updateValueAndValidity();
            this.terminalCreateForm.get('ZipCode').setValidators(validator);
            this.terminalCreateForm.get('ZipCode').updateValueAndValidity();
        }
    }
    getTerminalDetailsData() {
        let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.terminalService.getTerminalsForGrid(countryId).subscribe((data) => {
            if (data) {
                jQuery("#terminal-datatable").DataTable().clear().destroy();
                this.TerminalDetailsData = data;
                this.dtTrigger.next();
                this.IsLoading = false;
            }
        });
    }
    editTerminal(terminal) {
        if (terminal) {
            this.terminalCreateForm.get('Id').setValue(terminal.Id);
            this.terminalCreateForm.get('Name').setValue(terminal.Name);
            this.terminalCreateForm.get('Abbreviation').setValue(terminal.Abbreviation);
            this.terminalCreateForm.get('ControlNumber').setValue(terminal.ControlNumber);
            this.terminalCreateForm.get('TerminalOwner').setValue(terminal.TerminalOwner);
            this.terminalCreateForm.get('Address').setValue(terminal.Address);
            this.terminalCreateForm.get('City').setValue(terminal.City);
            this.terminalCreateForm.get('County').setValue(terminal.County);
            this.terminalCreateForm.get('StateId').setValue(terminal.StateId);
            this.terminalCreateForm.get('CountryId').setValue(terminal.CountryId);
            //this.terminalCreateForm.get('StateName').setValue(terminal.State);
            this.terminalCreateForm.get('ZipCode').setValue(terminal.ZipCode);
            this.terminalCreateForm.get('IsGeocodeUsed').setValue(false); // we dont save IsGeoCode flag at terminal level so it to false always 
            this.terminalCreateForm.get('Latitude').setValue(terminal.Latitude);
            this.terminalCreateForm.get('Longitude').setValue(terminal.Longitude);
            if (terminal.Latitude) {
                this.mapConstants.CenterLat = parseFloat(terminal.Latitude);
                this.mapConstants.CenterLon = parseFloat(terminal.Longitude);
            }
            let countryId = this.terminalCreateForm.get('CountryId').value;
            this.setAddressValidators(countryId);
            jQuery("#AddressDetails_Country_Id").val("0").change();
        }
    }
    onSubmit() {
        this.terminalCreateForm.markAllAsTouched();
        this.terminalCreateForm.value;
        if (this.terminalCreateForm.valid) {
            this.IsLoading = true;
            // serverside api to save terminal
            let terminal = this.terminalCreateForm.value;
            this.terminalService.saveTerminalDetails(terminal).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                    this.terminalCreateForm.reset();
                    this.clearPanelData();
                    let dismissSlider = document.getElementById('btnCancel');
                    dismissSlider.click();
                    this.getTerminalDetailsData();
                }
                else {
                    this.IsLoading = false;
                    _declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            return;
        }
    }
    isInvalid(name) {
        var result = this.terminalCreateForm.get(name).invalid
            &&
                (this.terminalCreateForm.get(name).dirty
                    ||
                        this.terminalCreateForm.get(name).touched);
        return result;
    }
    isRequired(name) {
        return this.terminalCreateForm.get(name).errors.required;
    }
    getAddressByZip() {
        var zipCode = this.terminalCreateForm.get('ZipCode').value;
        if (zipCode) {
            this.marineService.GetAddressByZip(zipCode).subscribe(data => {
                if (data) {
                    var country = this.countryList.find(t => t.Code.includes(data.CountryCode));
                    if (country) {
                        var countryId = country.Id;
                        this.terminalCreateForm.get('CountryId').patchValue(countryId);
                        /*this.terminalCreateForm.get('Address').patchValue(data.Address);*/
                        this.terminalCreateForm.get('County').patchValue(data.CountyName);
                        this.terminalCreateForm.get('City').patchValue(data.City);
                        var stateId = this.statesList.find(x => x.StateCode == data.StateCode).StateId;
                        this.terminalCreateForm.get('StateId').patchValue(stateId);
                        this.terminalCreateForm.get('Latitude').patchValue(data.Latitude);
                        this.terminalCreateForm.get('Longitude').patchValue(data.Longitude);
                        this.mapConstants.CenterLat = data.Latitude;
                        this.mapConstants.CenterLon = data.Longitude;
                        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
                        if (!this.terminalCreateForm.get('Address').value) {
                            this.terminalCreateForm.get('Address').patchValue(data.Address);
                        }
                    }
                }
            });
        }
    }
    getAddressByLatLong(lat, long) {
        let isGeoCoded = this.terminalCreateForm.get('IsGeocodeUsed').value;
        if (isGeoCoded && lat && long) {
            this.updateGeoCode(lat, long);
        }
    }
    updateGeoCode(lat, lng) {
        this.salesService.GetAddressByLongLat(lat, lng).subscribe(data => {
            if (data) {
                data.Latitude = parseFloat(lat);
                data.Longitude = parseFloat(lng);
                this.updateAddressData(data);
            }
            else { // if no address fetched for lat-long then set only map marker on UI
                this.mapConstants.CenterLat = lat;
                this.mapConstants.CenterLon = lng;
            }
        });
    }
    updateAddressData(address) {
        let countryId = (address.CountryCode == 'US' || address.CountryCode == 'USA') ? 1 : (address.CountryCode == 'CA' || address.CountryCode == 'CAN') ? 2 : this.terminalCreateForm.get('CountryId').value;
        let stateName = (address.StateName != null && address.StateName != '' && address.StateName != undefined) ? address.StateName : address.CountryName;
        if (stateName) {
            let state = this.statesList.find(st => st.StateName.toLowerCase() == stateName.toLowerCase());
            let stateId = (state && state.StateId) ? state.StateId : this.terminalCreateForm.get('StateId').value;
            this.terminalCreateForm.get('StateId').patchValue(stateId);
        }
        else //set first state after filtering by countryID
         {
            let states = this.statesList.filter(s => s.CountryId == countryId) || [];
            if (states && states.length > 0) {
                let stateId = states[0].StateId;
                if (stateId) {
                    this.terminalCreateForm.get('StateId').patchValue(stateId);
                }
            }
        }
        this.terminalCreateForm.get('Address').patchValue(address.Address);
        this.terminalCreateForm.get('City').patchValue(address.City);
        this.terminalCreateForm.get('ZipCode').patchValue(address.ZipCode);
        this.terminalCreateForm.get('CountryId').patchValue(countryId);
        this.terminalCreateForm.get('County').patchValue(address.CountyName);
        if (address.Latitude) {
            this.terminalCreateForm.get('Latitude').patchValue(address.Latitude);
            this.terminalCreateForm.get('Longitude').patchValue(address.Longitude);
            this.mapConstants.CenterLat = address.Latitude;
            this.mapConstants.CenterLon = address.Longitude;
        }
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
    }
    markerDragEnd(event) {
        this.confirmationdialogueservice.confirm('Map update', 'Geo Codes shifted to a new location!')
            .then((confirmed) => (confirmed == true) ? this.updateGeoCode(event.coords.lat, event.coords.lng) : this.previousLatLon())
            .catch(() => this.previousLatLon());
    }
    previousLatLon() {
        this.mapConstants.CenterLat = this.terminalCreateForm.get('Latitude').value;
        this.mapConstants.CenterLon = this.terminalCreateForm.get('Longitude').value;
    }
    ngOnDestroy() {
        this.dtTrigger.unsubscribe();
    }
}
CreateTerminalComponent.??fac = function CreateTerminalComponent_Factory(t) { return new (t || CreateTerminalComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_5__["MarinePortsandvesselsService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_6__["SalesUserService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_7__["ConfirmationDialogService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_createterminals_service__WEBPACK_IMPORTED_MODULE_8__["CreateterminalsService"])); };
CreateTerminalComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: CreateTerminalComponent, selectors: [["app-create-terminal"]], inputs: { SelectedCountryId: "SelectedCountryId" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["????NgOnChangesFeature"]], decls: 152, vars: 38, consts: [[1, "section-marine-ports-grid"], [1, "row"], [1, "col-12"], [1, "pt0", "pull-left"], ["onclick", "slidePanel('#create-terminal','70%')", 1, "fs18", "pull-left", "ml10", 3, "ngClass", "click"], [1, "fa", "fa-plus-circle", "fs18", "mt4", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "terminal-details-grid", 1, "table-responsive"], ["id", "terminal-datatable", "data-gridname", "35", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngFor", "ngForOf"], ["id", "create-terminal", 1, "side-panel", "pl5", "pr5", "scroll-y"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel", 3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [1, "mx-3"], [3, "formGroup", "ngSubmit"], [1, "row", "bg-white"], [1, "create-terminal"], [1, "col-sm-4", "form-group"], [1, "new-terminal-name"], [1, "terminal-info"], ["aria-required", "true", 1, "required", "pl4"], ["formControlName", "Name", "type", "text", "value", "", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "new-terminal-abbreviation"], ["formControlName", "Abbreviation", "type", "text", "value", "", 1, "form-control"], [1, "new-terminal-control-number"], ["formControlName", "ControlNumber", "type", "text", "value", "", 1, "form-control"], [1, "new-terminal-owner"], ["formControlName", "TerminalOwner", "type", "text", "value", "", 1, "form-control"], [1, "row", "mt20"], [1, "col-sm-12"], [1, "col-sm-7"], [1, "wrapper-location", 2, "display", "block"], [1, "address-wrapper", 3, "ngClass"], [1, "address-controls"], [1, "col-sm-8"], [1, "form-group"], ["class", "required pl4", "aria-required", "true", 4, "ngIf"], ["formControlName", "Address", "value", "", 1, "form-control", "address", "addressInput"], [1, "col-sm-4"], ["for", "AddressDetails_ZipCode"], ["formControlName", "ZipCode", "type", "text", "value", "", 1, "form-control", 3, "change"], ["id", "AddressDetails_City", "formControlName", "City", "type", "text", "value", "", 1, "form-control", "city", "addressInput"], ["data-toggle", "tooltip", "data-placement", "top", "title", "Correct County name is required by our Tax service to calculate taxes accurately.", 1, "fa", "fa-info-circle", "ml5"], ["id", "AddressDetails_CountyName", "formControlName", "County", "type", "text", "value", "", "autocomplete", "off", 1, "form-control", "county", "addressInput"], ["for", "AddressDetails_State_Id"], ["id", "AddressDetails_State_Id", "name", "AddressDetails.State.Id", "formControlName", "StateId", 1, "form-control", "state", "addressInput", "triggerTerminalChange"], ["value", ""], [3, "id", "value", "selected", 4, "ngFor", "ngForOf"], [1, "form-check", "form-group", "form-check-inline"], ["id", "checkbox-geocodes", "formControlName", "IsGeocodeUsed", "type", "checkbox", 1, "form-check-input", 3, "value", "change"], ["for", "checkbox-geocodes", 1, "form-check-label"], [1, "col-xs-6", "col-md-4", "combineDiv"], ["for", "AddressDetails_Latitude"], ["id", "AddressDetails_Latitude", "name", "AddressDetails.Latitude", "formControlName", "Latitude", "type", "text", "value", "0", 1, "form-control", "latitude", "geoInput", "defaultDisabled", 3, "readonly", "change"], ["for", "AddressDetails_Longitude"], ["id", "AddressDetails_Longitude", "formControlName", "Longitude", "type", "text", "value", "0", 1, "form-control", "longitude", "geoInput", "defaultDisabled", 3, "readonly", "change"], [1, "col-sm-5"], [3, "zoom", "latitude", "longitude"], [3, "latitude", "longitude", "markerDraggable", "iconUrl", "dragEnd"], [1, "row", "mt-3"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "id", "btnCancel", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "value", "Submit", 1, "btn", "btn-primary", "btn-lg", "no-disable"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "text-center"], ["type", "button", "onclick", "slidePanel('#create-terminal','70%')", 1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-edit", "fs16"], [1, "color-maroon"], [3, "id", "value", "selected"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"]], template: function CreateTerminalComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "h4", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](5, "Terminals");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CreateTerminalComponent_Template_a_click_6_listener() { return ctx.createTerminal("Create Terminal"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "span", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "Add New");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "table", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Abbreviation");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "ControlNumber");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Address");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "City");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "State");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "Terminal Owner");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](36, CreateTerminalComponent_tr_36_Template, 24, 13, "tr", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](38, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "a", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CreateTerminalComponent_Template_a_click_40_listener() { return ctx.clearPanelData(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](41, "i", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "h3", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "div", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "form", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngSubmit", function CreateTerminalComponent_Template_form_ngSubmit_45_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "div", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](51, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "label", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](53, " Terminal Name ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](54, "span", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](55, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](56, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](57, CreateTerminalComponent_div_57_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](58, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](59, "div", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](60, "label", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](61, " Terminal Abbreviation ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](62, "input", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](63, CreateTerminalComponent_div_63_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](64, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](65, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](66, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](67, "label", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](68, " Terminal Control Number ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](69, "input", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](70, CreateTerminalComponent_div_70_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](71, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](72, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](73, "label", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](74, " Terminal Owner ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](75, "input", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](76, CreateTerminalComponent_div_76_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](77, "div", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](78, "div", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](79, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](80, "div", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](81, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](82, "div", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](83, "div", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](84, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](85, "div", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](86, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](87, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](88, "Address");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](89, CreateTerminalComponent_span_89_Template, 2, 0, "span", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](90, "input", 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](91, CreateTerminalComponent_div_91_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](92, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](93, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](94, "label", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](95, "Zip");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](96, CreateTerminalComponent_span_96_Template, 2, 0, "span", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](97, "input", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function CreateTerminalComponent_Template_input_change_97_listener() { return ctx.getAddressByZip(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](98, CreateTerminalComponent_div_98_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](99, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](100, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](101, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](102, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](103, "City");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](104, CreateTerminalComponent_span_104_Template, 2, 0, "span", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](105, "input", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](106, CreateTerminalComponent_div_106_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](107, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](108, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](109, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](110, "County Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](111, CreateTerminalComponent_span_111_Template, 2, 0, "span", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](112, "i", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](113, "input", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](114, CreateTerminalComponent_div_114_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](115, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](116, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](117, "label", 52);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](118, " State");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](119, "span", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](120, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](121, "select", 53);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](122, "option", 54);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](123, "Select State");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](124, CreateTerminalComponent_option_124_Template, 2, 4, "option", 55);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](125, CreateTerminalComponent_div_125_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](126, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](127, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](128, "div", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](129, "input", 57);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function CreateTerminalComponent_Template_input_change_129_listener() { return ctx.setLatLongValidator(ctx.terminalCreateForm.get("IsGeocodeUsed").value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](130, "label", 58);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](131, "Geo Codes");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](132, "div", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](133, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](134, "label", 60);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](135, "Latitude");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](136, "input", 61);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function CreateTerminalComponent_Template_input_change_136_listener() { return ctx.getAddressByLatLong(ctx.terminalCreateForm.get("Latitude").value, ctx.terminalCreateForm.get("Longitude").value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](137, CreateTerminalComponent_div_137_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](138, "div", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](139, "div", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](140, "label", 62);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](141, "Longitude");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](142, "input", 63);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function CreateTerminalComponent_Template_input_change_142_listener() { return ctx.getAddressByLatLong(ctx.terminalCreateForm.get("Latitude").value, ctx.terminalCreateForm.get("Longitude").value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](143, CreateTerminalComponent_div_143_Template, 2, 1, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](144, "div", 64);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](145, "agm-map", 65);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](146, "agm-marker", 66);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("dragEnd", function CreateTerminalComponent_Template_agm_marker_dragEnd_146_listener($event) { return ctx.markerDragEnd($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](147, "div", 67);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](148, "div", 68);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](149, "input", 69);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CreateTerminalComponent_Template_input_click_149_listener() { return ctx.clearPanelData(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](150, "input", 70);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](151, CreateTerminalComponent_div_151_Template, 3, 0, "div", 71);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](34, _c0, ctx.SelectedCountryId == "4"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.TerminalDetailsData);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.ModalText);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.terminalCreateForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("Name"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("Abbreviation"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("ControlNumber"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("TerminalOwner"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["????pureFunction1"](36, _c1, ctx.terminalCreateForm.controls["IsGeocodeUsed"].value == true && ctx.terminalCreateForm.get("CountryId").value != "4"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.terminalCreateForm.get("CountryId").value != "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("Address"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.terminalCreateForm.get("CountryId").value != "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("ZipCode"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.terminalCreateForm.get("CountryId").value != "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("City"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.terminalCreateForm.get("CountryId").value != "4");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("County"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.filteredStatesList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("StateId"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", false);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("readonly", ctx.terminalCreateForm.get("IsGeocodeUsed").value == false);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("Latitude"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("readonly", ctx.terminalCreateForm.get("IsGeocodeUsed").value == false);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.isInvalid("Longitude"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("zoom", ctx.mapConstants.ZoomArea)("latitude", ctx.mapConstants.CenterLat)("longitude", ctx.mapConstants.CenterLon);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("latitude", ctx.mapConstants.CenterLat)("longitude", ctx.mapConstants.CenterLon)("markerDraggable", true)("iconUrl", ctx.mapConstants.IconUrl);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_9__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_x"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _agm_core__WEBPACK_IMPORTED_MODULE_11__["AgmMap"], _agm_core__WEBPACK_IMPORTED_MODULE_11__["AgmMarker"]], styles: ["agm-map[_ngcontent-%COMP%] {\r\n    height: 300px !important;\r\n    width: 100%\r\n}\r\n\r\n.scroll-y[_ngcontent-%COMP%]{\r\n    overflow-y:scroll\r\n}\r\n\r\n.disabled[_ngcontent-%COMP%] {\r\n    pointer-events: none !important;\r\n    cursor: default;\r\n    opacity:0.5;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc3VwZXJhZG1pbi1jcmVhdGUtdGVybWluYWxzL2NyZWF0ZS10ZXJtaW5hbHMvY3JlYXRlLXRlcm1pbmFsLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSx3QkFBd0I7SUFDeEI7QUFDSjs7QUFFQTtJQUNJO0FBQ0o7O0FBRUE7SUFDSSwrQkFBK0I7SUFDL0IsZUFBZTtJQUNmLFdBQVc7QUFDZiIsImZpbGUiOiJzcmMvYXBwL3N1cGVyYWRtaW4tY3JlYXRlLXRlcm1pbmFscy9jcmVhdGUtdGVybWluYWxzL2NyZWF0ZS10ZXJtaW5hbC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiYWdtLW1hcCB7XHJcbiAgICBoZWlnaHQ6IDMwMHB4ICFpbXBvcnRhbnQ7XHJcbiAgICB3aWR0aDogMTAwJVxyXG59XHJcblxyXG4uc2Nyb2xsLXl7XHJcbiAgICBvdmVyZmxvdy15OnNjcm9sbFxyXG59XHJcblxyXG4uZGlzYWJsZWQge1xyXG4gICAgcG9pbnRlci1ldmVudHM6IG5vbmUgIWltcG9ydGFudDtcclxuICAgIGN1cnNvcjogZGVmYXVsdDtcclxuICAgIG9wYWNpdHk6MC41O1xyXG59Il19 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](CreateTerminalComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-create-terminal',
                templateUrl: './create-terminal.component.html',
                styleUrls: ['./create-terminal.component.css']
            }]
    }], function () { return [{ type: _marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_5__["MarinePortsandvesselsService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_6__["SalesUserService"] }, { type: _shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_7__["ConfirmationDialogService"] }, { type: _createterminals_service__WEBPACK_IMPORTED_MODULE_8__["CreateterminalsService"] }]; }, { SelectedCountryId: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }] }); })();
class MapConstants {
    constructor() {
        this.CenterLat = 38;
        this.CenterLon = -98.35;
        this.ZoomArea = 15;
        this.IconUrl = { url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png', scaledSize: { width: 40, height: 40 } };
    }
}
class MapIconUrl {
}
class MapIconSize {
}


/***/ }),

/***/ "./src/app/superadmin-create-terminals/createterminals.service.ts":
/*!************************************************************************!*\
  !*** ./src/app/superadmin-create-terminals/createterminals.service.ts ***!
  \************************************************************************/
/*! exports provided: CreateterminalsService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateterminalsService", function() { return CreateterminalsService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class CreateterminalsService extends _errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.urlGetTerminalsForGrid = '/SuperAdmin/SuperAdmin/GetTerminals';
        this.urlSaveTerminalDetails = '/SuperAdmin/SuperAdmin/SaveTerminal';
        this.urlGetTerminalMappingDetails = 'SuperAdmin/SuperAdmin/GetTerminalMappingDetails';
        this.urlGetAllProductsMapping = 'SuperAdmin/SuperAdmin/GetMstProductsForTerminalMapping';
        this.urlSaveTerminalProductMapping = 'SuperAdmin/SuperAdmin/SaveTerminalProductMapping';
    }
    getTerminalsForGrid(countryId) {
        return this.httpClient.get(this.urlGetTerminalsForGrid + '?countryId=' + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTerminalsForGrid', [])));
    }
    saveTerminalDetails(terminal) {
        return this.httpClient.post(this.urlSaveTerminalDetails, terminal)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('saveTerminalDetails', terminal)));
    }
    getTerminalProductMappingDetails(countryId) {
        return this.httpClient.get(this.urlGetTerminalMappingDetails + '?countryId=' + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTerminalProductMappingDetails', [])));
    }
    getAllProductsForMapping() {
        return this.httpClient.get(this.urlGetAllProductsMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getAllProductsForMapping', [])));
    }
    saveTerminalProductMapping(model) {
        return this.httpClient.post(this.urlSaveTerminalProductMapping, model)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('saveTerminalProductMapping', model)));
    }
}
CreateterminalsService.??fac = function CreateterminalsService_Factory(t) { return new (t || CreateterminalsService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
CreateterminalsService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({ token: CreateterminalsService, factory: CreateterminalsService.??fac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](CreateterminalsService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/superadmin-create-terminals/master.component.ts":
/*!*****************************************************************!*\
  !*** ./src/app/superadmin-create-terminals/master.component.ts ***!
  \*****************************************************************/
/*! exports provided: MasterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterComponent", function() { return MasterComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_self_service_alias_models_location__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/self-service-alias/models/location */ "./src/app/self-service-alias/models/location.ts");
/* harmony import */ var _marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../marine-ports-vessels/marine-portsandvessels.service */ "./src/app/marine-ports-vessels/marine-portsandvessels.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _create_terminals_create_terminal_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./create-terminals/create-terminal.component */ "./src/app/superadmin-create-terminals/create-terminals/create-terminal.component.ts");
/* harmony import */ var _terminal_product_assignment_terminal_product_assignment_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./terminal-product assignment/terminal-product-assignment.component */ "./src/app/superadmin-create-terminals/terminal-product assignment/terminal-product-assignment.component.ts");








function MasterComponent_option_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "option", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](1, "i", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const country_r3 = ctx.$implicit;
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", country_r3.Id)("selected", ctx_r0.SelectedCountryId == country_r3.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", country_r3.Code, " ");
} }
function MasterComponent_app_create_terminal_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-create-terminal", 15);
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("SelectedCountryId", ctx_r1.SelectedCountryId);
} }
function MasterComponent_app_terminal_product_assignment_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](0, "app-terminal-product-assignment", 15);
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("SelectedCountryId", ctx_r2.SelectedCountryId);
} }
class MasterComponent {
    constructor(marineService) {
        this.marineService = marineService;
        this.viewType = 1;
        //country selection related variables
        this.CountryList = [];
        this.IsLoading = false;
    }
    ngOnInit() {
        this.getCountries();
        this.SelectedCountryId = src_app_self_service_alias_models_location__WEBPACK_IMPORTED_MODULE_1__["Country"].USA;
    }
    getCountries() {
        this.marineService.GetAllCountries().subscribe((data) => {
            this.CountryList = data;
        });
    }
    onCountryChange(event) {
        this.SelectedCountryId = event.target.value;
    }
    changeViewType(value) {
        this.viewType = value;
    }
}
MasterComponent.??fac = function MasterComponent_Factory(t) { return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_2__["MarinePortsandvesselsService"])); };
MasterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: MasterComponent, selectors: [["app-master"]], decls: 17, vars: 9, consts: [[1, "row", "mb-3"], [1, "col-sm-11"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "bg-white"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", "mr-1", 3, "click"], [1, "btn", 3, "click"], [1, "col-sm-1"], [1, "form-control", "mt-1", 3, "change"], [3, "value", "selected", 4, "ngFor", "ngForOf"], [1, "row"], [1, "col-sm-12"], [3, "SelectedCountryId", 4, "ngIf"], [3, "value", "selected"], [1, "fa", "fa-airbnb", "fa-2x"], [3, "SelectedCountryId"]], template: function MasterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "label", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_5_listener() { return ctx.changeViewType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Terminals");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](7, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function MasterComponent_Template_label_click_8_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](9, "Terminals-Product Assignment");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "select", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function MasterComponent_Template_select_change_11_listener($event) { return ctx.onCountryChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](12, MasterComponent_option_12_Template, 3, 3, "option", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](15, MasterComponent_app_create_terminal_15_Template, 1, 1, "app-create-terminal", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, MasterComponent_app_terminal_product_assignment_16_Template, 1, 1, "app-terminal-product-assignment", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "viewType")("value", 1)("checked", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("name", "viewType")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.CountryList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.viewType == 2);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["??angular_packages_forms_forms_x"], _create_terminals_create_terminal_component__WEBPACK_IMPORTED_MODULE_5__["CreateTerminalComponent"], _terminal_product_assignment_terminal_product_assignment_component__WEBPACK_IMPORTED_MODULE_6__["TerminalProductAssignmentComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3N1cGVyYWRtaW4tY3JlYXRlLXRlcm1pbmFscy9tYXN0ZXIuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-master',
                templateUrl: './master.component.html',
                styleUrls: ['./master.component.css']
            }]
    }], function () { return [{ type: _marine_ports_vessels_marine_portsandvessels_service__WEBPACK_IMPORTED_MODULE_2__["MarinePortsandvesselsService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/superadmin-create-terminals/models.ts":
/*!*******************************************************!*\
  !*** ./src/app/superadmin-create-terminals/models.ts ***!
  \*******************************************************/
/*! exports provided: TerminalDetailsModel, Geocode, TerminalMappedProductsGridModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TerminalDetailsModel", function() { return TerminalDetailsModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Geocode", function() { return Geocode; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TerminalMappedProductsGridModel", function() { return TerminalMappedProductsGridModel; });
class TerminalDetailsModel {
    constructor() {
        this.IsGeoCoded = false;
    }
}
class Geocode {
}
class TerminalMappedProductsGridModel {
    constructor() {
        this.MappedProducts = [];
    }
}


/***/ }),

/***/ "./src/app/superadmin-create-terminals/superadmin-create-terminal.module.ts":
/*!**********************************************************************************!*\
  !*** ./src/app/superadmin-create-terminals/superadmin-create-terminal.module.ts ***!
  \**********************************************************************************/
/*! exports provided: SuperadminCreateTerminalModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SuperadminCreateTerminalModule", function() { return SuperadminCreateTerminalModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _master_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./master.component */ "./src/app/superadmin-create-terminals/master.component.ts");
/* harmony import */ var _create_terminals_create_terminal_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./create-terminals/create-terminal.component */ "./src/app/superadmin-create-terminals/create-terminals/create-terminal.component.ts");
/* harmony import */ var _terminal_product_assignment_terminal_product_assignment_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./terminal-product assignment/terminal-product-assignment.component */ "./src/app/superadmin-create-terminals/terminal-product assignment/terminal-product-assignment.component.ts");










const route = [
    { path: '', component: _master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"] }
];
class SuperadminCreateTerminalModule {
}
SuperadminCreateTerminalModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({ type: SuperadminCreateTerminalModule });
SuperadminCreateTerminalModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({ factory: function SuperadminCreateTerminalModule_Factory(t) { return new (t || SuperadminCreateTerminalModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(route)
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](SuperadminCreateTerminalModule, { declarations: [_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"], _create_terminals_create_terminal_component__WEBPACK_IMPORTED_MODULE_6__["CreateTerminalComponent"], _terminal_product_assignment_terminal_product_assignment_component__WEBPACK_IMPORTED_MODULE_7__["TerminalProductAssignmentComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](SuperadminCreateTerminalModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [_master_component__WEBPACK_IMPORTED_MODULE_5__["MasterComponent"], _create_terminals_create_terminal_component__WEBPACK_IMPORTED_MODULE_6__["CreateTerminalComponent"], _terminal_product_assignment_terminal_product_assignment_component__WEBPACK_IMPORTED_MODULE_7__["TerminalProductAssignmentComponent"]],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(route)
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/superadmin-create-terminals/terminal-product assignment/terminal-product-assignment.component.ts":
/*!******************************************************************************************************************!*\
  !*** ./src/app/superadmin-create-terminals/terminal-product assignment/terminal-product-assignment.component.ts ***!
  \******************************************************************************************************************/
/*! exports provided: TerminalProductAssignmentComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TerminalProductAssignmentComponent", function() { return TerminalProductAssignmentComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _models__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./../models */ "./src/app/superadmin-create-terminals/models.ts");
/* harmony import */ var _createterminals_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../createterminals.service */ "./src/app/superadmin-create-terminals/createterminals.service.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");













function TerminalProductAssignmentComponent_tr_16_span_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const mapping_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](mapping_r3.TerminalControlNumber);
} }
function TerminalProductAssignmentComponent_tr_16_span_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, "--");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function TerminalProductAssignmentComponent_tr_16_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const mapping_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit;
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx_r6.formatProductsForGridDisplay(mapping_r3 == null ? null : mapping_r3.MappedProducts));
} }
function TerminalProductAssignmentComponent_tr_16_Template(rf, ctx) { if (rf & 1) {
    const _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "a", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TerminalProductAssignmentComponent_tr_16_Template_a_click_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r10); const mapping_r3 = ctx.$implicit; const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); ctx_r9._toggleOpened(true); return ctx_r9.editMapping(mapping_r3); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, TerminalProductAssignmentComponent_tr_16_span_5_Template, 2, 1, "span", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](6, TerminalProductAssignmentComponent_tr_16_span_6_Template, 2, 0, "span", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](8, TerminalProductAssignmentComponent_tr_16_span_8_Template, 4, 1, "span", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const mapping_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](mapping_r3.TerminalName);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", mapping_r3.TerminalControlNumber != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", mapping_r3.TerminalControlNumber == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (mapping_r3 == null ? null : mapping_r3.MappedProducts.length) && (mapping_r3 == null ? null : mapping_r3.MappedProducts.length) > 0);
} }
function TerminalProductAssignmentComponent_div_45_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Product(s) is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function TerminalProductAssignmentComponent_div_45_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, TerminalProductAssignmentComponent_div_45_div_1_Template, 2, 0, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.terminalMappingForm.get("MappedProducts").errors.required);
} }
function TerminalProductAssignmentComponent_div_51_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class TerminalProductAssignmentComponent {
    constructor(terminalService, fb) {
        this.terminalService = terminalService;
        this.fb = fb;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.terminalMappingDetails = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
    }
    ngOnInit() {
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: true,
            order: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: "Id",
            textField: "Name",
            selectAllText: "Select All",
            unSelectAllText: "UnSelect All",
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.getAllProductsDDL();
        this.initializeTerminalMappingCreationForm(this.terminalMappingDetail);
    }
    ngOnChanges(changes) {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            this.getTerminalMappingDetailsData();
        }
    }
    getTerminalMappingDetailsData() {
        let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.terminalService.getTerminalProductMappingDetails(countryId).subscribe((data) => {
            if (data) {
                jQuery("#terminal-products-datatable").DataTable().clear().destroy();
                this.terminalMappingDetails = data;
                this.dtTrigger.next();
                this.IsLoading = false;
            }
        });
    }
    initializeTerminalMappingCreationForm(terminalMapping) {
        if (terminalMapping == null || terminalMapping == undefined) {
            terminalMapping = new _models__WEBPACK_IMPORTED_MODULE_4__["TerminalMappedProductsGridModel"]();
        }
        this.terminalMappingForm = this.fb.group({
            TerminalId: this.fb.control(terminalMapping.TerminalId),
            TerminalControlNumber: this.fb.control(terminalMapping.TerminalControlNumber),
            MappedProducts: this.fb.control(terminalMapping.MappedProducts, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            TerminalName: this.fb.control(terminalMapping.TerminalName)
        });
    }
    formatProductsForGridDisplay(assignedProducts) {
        let formattedString = "";
        if (assignedProducts != null && assignedProducts.length > 0) {
            let displayCount = assignedProducts.length - 3;
            if (assignedProducts.length > 3) {
                assignedProducts.forEach(function (product, index) {
                    if (index <= 2) {
                        if (product.Name) {
                            formattedString = index == 2 ? formattedString.concat(product.Name, "     ", "+" + displayCount + " other") : formattedString.concat(product.Name, ", ");
                        }
                    }
                });
            }
            else {
                assignedProducts.forEach(function (product, index) {
                    if (product.Name) {
                        formattedString = index == assignedProducts.length - 1 ? formattedString.concat(product.Name, " ") : formattedString.concat(product.Name, ", ");
                    }
                });
            }
        }
        return formattedString;
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.terminalMappingForm.reset();
        }
    }
    getAllProductsDDL() {
        this.IsLoading = true;
        this.terminalService.getAllProductsForMapping().subscribe((data) => {
            this.IsLoading = false;
            this.productList = data;
        });
    }
    editMapping(terminalMapping) {
        if (terminalMapping != null && terminalMapping != undefined) {
            this.terminalMappingForm.get('TerminalId').setValue(terminalMapping.TerminalId);
            this.terminalMappingForm.get('TerminalControlNumber').setValue(terminalMapping.TerminalControlNumber);
            this.terminalMappingForm.get('MappedProducts').setValue(terminalMapping.MappedProducts);
            this.terminalMappingForm.get('TerminalName').setValue(terminalMapping.TerminalName);
        }
    }
    SubmitForm() {
        this.terminalMappingForm.markAllAsTouched();
        if (this.terminalMappingForm.valid) {
            let model = this.terminalMappingForm.value;
            if (model) {
                let input = new _models__WEBPACK_IMPORTED_MODULE_4__["TerminalMappedProductsGridModel"]();
                input.TerminalId = model.TerminalId;
                input.TerminalControlNumber = model.TerminalControlNumber;
                input.MappedProducts = model.MappedProducts;
                input.TerminalName = model.TerminalName;
                this.IsLoading = true;
                this.terminalService.saveTerminalProductMapping(input).subscribe((data) => {
                    if (data.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                        this._toggleOpened(false);
                        this.getTerminalMappingDetailsData();
                    }
                    else if (data.StatusCode == 1) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
                    }
                    this.IsLoading = false;
                });
            }
        }
    }
}
TerminalProductAssignmentComponent.??fac = function TerminalProductAssignmentComponent_Factory(t) { return new (t || TerminalProductAssignmentComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_createterminals_service__WEBPACK_IMPORTED_MODULE_5__["CreateterminalsService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"])); };
TerminalProductAssignmentComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: TerminalProductAssignmentComponent, selectors: [["app-terminal-product-assignment"]], inputs: { SelectedCountryId: "SelectedCountryId" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["????NgOnChangesFeature"]], decls: 52, vars: 15, consts: [[1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "terminal-products-grid", 1, "table-responsive"], ["id", "terminal-products-datatable", "data-gridname", "35", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngFor", "ngForOf"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "col-sm-6"], [1, "form-group"], ["for", "TerminalName"], ["formControlName", "TerminalName", 1, "form-control", 3, "readonly"], ["for", "TerminalControlNumber"], ["formControlName", "TerminalControlNumber", 1, "form-control", 3, "readonly"], ["for", "Jobs"], [1, "color-maroon"], ["formControlName", "MappedProducts", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["class", "color-maroon", 4, "ngIf"], [1, "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], ["class", "loader", 4, "ngIf"], [3, "routerLink", "click"], [4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"]], template: function TerminalProductAssignmentComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "table", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "Terminal Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12, "Terminal Control Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14, "Assigned Products");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](16, TerminalProductAssignmentComponent_tr_16_Template, 9, 4, "tr", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "ng-sidebar-container");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "ng-sidebar", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("openedChange", function TerminalProductAssignmentComponent_Template_ng_sidebar_openedChange_19_listener($event) { return ctx._opened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "a", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TerminalProductAssignmentComponent_Template_a_click_20_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](21, "i", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "h3", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](23, "Terminal-Product(s) Assignment");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "content", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "form", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngSubmit", function TerminalProductAssignmentComponent_Template_form_ngSubmit_25_listener() { return ctx.SubmitForm(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Terminal Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](31, "input", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](32, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](34, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](35, "Terminal Control Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](36, "input", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](38, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "label", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](41, "Product(s)");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "span", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](43, "*");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "ng-multiselect-dropdown", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function TerminalProductAssignmentComponent_Template_ng_multiselect_dropdown_ngModelChange_44_listener($event) { return ctx.Products = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](45, TerminalProductAssignmentComponent_div_45_Template, 2, 1, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "button", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function TerminalProductAssignmentComponent_Template_button_click_47_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](48, "Cancel");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "button", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](50, "Save");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](51, TerminalProductAssignmentComponent_div_51_Template, 3, 0, "div", 27);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx.terminalMappingDetails);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx.terminalMappingForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("readonly", true);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("readonly", true);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Product(s)")("settings", ctx.multiselectSettingsById)("data", ctx.productList)("ngModel", ctx.Products);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.terminalMappingForm.get("MappedProducts").invalid && ctx.terminalMappingForm.get("MappedProducts").touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_router__WEBPACK_IMPORTED_MODULE_10__["RouterLinkWithHref"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3N1cGVyYWRtaW4tY3JlYXRlLXRlcm1pbmFscy90ZXJtaW5hbC1wcm9kdWN0IGFzc2lnbm1lbnQvdGVybWluYWwtcHJvZHVjdC1hc3NpZ25tZW50LmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](TerminalProductAssignmentComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-terminal-product-assignment',
                templateUrl: './terminal-product-assignment.component.html',
                styleUrls: ['./terminal-product-assignment.component.css']
            }]
    }], function () { return [{ type: _createterminals_service__WEBPACK_IMPORTED_MODULE_5__["CreateterminalsService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }]; }, { SelectedCountryId: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }] }); })();


/***/ })

}]);
//# sourceMappingURL=superadmin-create-terminals-superadmin-create-terminal-module-es2015.js.map