(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["delivery-request-display-delivery-request-display-module"],{

/***/ "./src/app/delivery-request-display/delivery-request-display.component.ts":
/*!********************************************************************************!*\
  !*** ./src/app/delivery-request-display/delivery-request-display.component.ts ***!
  \********************************************************************************/
/*! exports provided: DeliveryRequestDisplayComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryRequestDisplayComponent", function() { return DeliveryRequestDisplayComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../carrier/models/DispatchSchedulerModels */ "./src/app/carrier/models/DispatchSchedulerModels.ts");
/* harmony import */ var _my_functions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../my.functions */ "./src/app/my.functions.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../carrier/service/carrier.service */ "./src/app/carrier/service/carrier.service.ts");
/* harmony import */ var _carrier_service_schedule_builder_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../carrier/service/schedule-builder.service */ "./src/app/carrier/service/schedule-builder.service.ts");
/* harmony import */ var _carrier_service_deliveryrequest_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../carrier/service/deliveryrequest.service */ "./src/app/carrier/service/deliveryrequest.service.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../shared-components/dip-test/dip-test.component */ "./src/app/shared-components/dip-test/dip-test.component.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");














function DeliveryRequestDisplayComponent_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Due to region/window mode change session data removed.Close the current window and reopen it again from schedule builder.");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Otherwise, window closed after 10 seconds.");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_div_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.CustomerBrandId, " ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.CustomerCompany.length > 6 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 1, drMissed_r14.CustomerCompany, 0, 6) + ".." : drMissed_r14.CustomerCompany, " -\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_div_2_Template, 2, 1, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_ng_template_3_Template, 3, 5, "ng-template", null, 47, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.CustomerBrandId)("ngIfElse", _r22);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.TrailerCompatibility, " ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_10_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.HoursToCoverDistance, " hrs ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_10_span_1_Template, 2, 1, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.HoursToCoverDistance);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_span_13_i_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "i", 53);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_span_13_i_1_Template, 1, 0, "i", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.IsOnlyNightDelivery == true);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3); return ctx_r33.pushItem(drMissed_r14, drMissed_r14.JobId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_3_Template, 5, 2, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_9_Template, 2, 1, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_div_10_Template, 2, 1, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "span", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_span_13_Template, 2, 1, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_Template_a_click_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3); return ctx_r36.bindDeliveryRequests(drMissed_r14, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "missed-", drMissed_r14.JobId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r15.checkItemsExists(drMissed_r14.JobId, drMissed_r14.DeliveryRequestType, drMissed_r14.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.DRQueueAttributes.CustomerName == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.JobName, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drMissed_r14.JobCity, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.DRQueueAttributes.TrailerCompatibility == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.DRQueueAttributes && drMissed_r14.DRQueueAttributes.HoursToCoverDistance == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.DRQueueAttributes.DeliveryShift == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drMissed_r14.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drMissed_r14.ProductType, 0, 10) + ".." : drMissed_r14.ProductType, " ", "(" + drMissed_r14.RequiredQuantity + (drMissed_r14.UoM == 1 ? " G" : " L") + ")", " ", drMissed_r14.DeliveryRequests.length > 1 ? ": (+" + (drMissed_r14.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drMissed_r14.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drMissed_r14.ProductType, 0, 10) + ".." : drMissed_r14.ProductType, " ", "(" + drMissed_r14.ScheduleQuantityTypeText + ")", " ", drMissed_r14.DeliveryRequests.length > 1 ? ": (+" + (drMissed_r14.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r46 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r46); const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3); return ctx_r44.pushItem(drMissed_r14, drMissed_r14.TBDGroupId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_div_2_Template, 3, 7, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_ng_template_3_Template, 3, 7, "ng-template", null, 56, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_Template_a_click_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r46); const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3); return ctx_r47.bindDeliveryRequests(drMissed_r14, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);
    const drMissed_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "missed-", drMissed_r14.TBDGroupId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r16.checkItemsExists(drMissed_r14.TBDGroupId, drMissed_r14.DeliveryRequestType, drMissed_r14.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", " TBD- ", drMissed_r14.ProductType + " -", " ", drMissed_r14.RequiredQuantity == 0 ? drMissed_r14.ScheduleQuantityTypeText : drMissed_r14.RequiredQuantity + (drMissed_r14.UoM == 1 ? " G" : " L"), "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.RequiredQuantity > 0)("ngIfElse", _r40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_1_Template, 16, 9, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_16_div_4_div_1_div_2_Template, 8, 7, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drMissed_r14 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !drMissed_r14.IsTBD);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drMissed_r14.IsTBD);
} }
function DeliveryRequestDisplayComponent_div_16_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_16_div_4_div_1_Template, 3, 2, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r11.missedRequests);
} }
function DeliveryRequestDisplayComponent_div_16_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " No request available ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_div_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "h2", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Missed");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DeliveryRequestDisplayComponent_div_16_div_4_Template, 2, 1, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_div_16_div_5_Template, 3, 0, "div", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.missedRequests.length > 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.missedRequests.length == 0);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.CustomerBrandId, " ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.CustomerCompany.length > 6 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 1, drmustGo_r51.CustomerCompany, 0, 6) + ".." : drmustGo_r51.CustomerCompany, " -\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_div_2_Template, 2, 1, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_ng_template_3_Template, 3, 5, "ng-template", null, 47, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.CustomerBrandId)("ngIfElse", _r59);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.TrailerCompatibility, " ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.HoursToCoverDistance, " hrs ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_13_i_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "i", 53);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_13_i_1_Template, 1, 0, "i", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.IsOnlyNightDelivery == true);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r70 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_21_div_1_div_1_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r70); const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r68 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r68.pushItem(drmustGo_r51, drmustGo_r51.JobId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_3_Template, 5, 2, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DeliveryRequestDisplayComponent_div_21_div_1_div_1_div_9_Template, 3, 1, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_10_Template, 2, 1, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "span", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, DeliveryRequestDisplayComponent_div_21_div_1_div_1_span_13_Template, 2, 1, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_21_div_1_div_1_Template_a_click_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r70); const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r71 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r71.bindDeliveryRequests(drmustGo_r51, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "mustgo-", drmustGo_r51.JobId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r52.checkItemsExists(drmustGo_r51.JobId, drmustGo_r51.DeliveryRequestType, drmustGo_r51.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.DRQueueAttributes.CustomerName == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.JobName, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drmustGo_r51.JobCity, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.DRQueueAttributes.TrailerCompatibility == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.DRQueueAttributes.HoursToCoverDistance == true && drmustGo_r51.HoursToCoverDistance);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.DRQueueAttributes.DeliveryShift == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_2_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drmustGo_r51.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drmustGo_r51.ProductType, 0, 10) + ".." : drmustGo_r51.ProductType, " ", "(" + drmustGo_r51.RequiredQuantity + (drmustGo_r51.UoM == 1 ? " G" : " L") + ")", " ", drmustGo_r51.DeliveryRequests.length > 1 ? ": (+" + (drmustGo_r51.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_2_ng_template_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drmustGo_r51.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drmustGo_r51.ProductType, 0, 10) + ".." : drmustGo_r51.ProductType, " ", "(" + drmustGo_r51.ScheduleQuantityTypeText + ")", " ", drmustGo_r51.DeliveryRequests.length > 1 ? ": (+" + (drmustGo_r51.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_21_div_1_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r81 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_21_div_1_div_2_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r81); const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r79 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r79.pushItem(drmustGo_r51, drmustGo_r51.TBDGroupId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DeliveryRequestDisplayComponent_div_21_div_1_div_2_div_4_Template, 3, 7, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_div_21_div_1_div_2_ng_template_5_Template, 3, 7, "ng-template", null, 56, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_21_div_1_div_2_Template_a_click_8_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r81); const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r82 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r82.bindDeliveryRequests(drmustGo_r51, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](6);
    const drmustGo_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "mustgo-", drmustGo_r51.TBDGroupId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r53.checkItemsExists(drmustGo_r51.TBDGroupId, drmustGo_r51.DeliveryRequestType, drmustGo_r51.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", "TBD- ", drmustGo_r51.ProductType + " -", " ", drmustGo_r51.RequiredQuantity == 0 ? drmustGo_r51.ScheduleQuantityTypeText : drmustGo_r51.RequiredQuantity + (drmustGo_r51.UoM == 1 ? " G" : " L"), "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.RequiredQuantity > 0)("ngIfElse", _r75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_21_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_21_div_1_div_1_Template, 16, 9, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_21_div_1_div_2_Template, 10, 7, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drmustGo_r51 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !drmustGo_r51.IsTBD);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drmustGo_r51.IsTBD);
} }
function DeliveryRequestDisplayComponent_div_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_21_div_1_Template, 3, 2, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r3.mustGoRequests);
} }
function DeliveryRequestDisplayComponent_div_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " No request available ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.CustomerBrandId, " ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.CustomerCompany.length > 6 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 1, drshouldGo_r86.CustomerCompany, 0, 6) + ".." : drshouldGo_r86.CustomerCompany, " -\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_div_2_Template, 2, 1, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_ng_template_3_Template, 3, 5, "ng-template", null, 47, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r94 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.CustomerBrandId)("ngIfElse", _r94);
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.TrailerCompatibility, " ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_10_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.HoursToCoverDistance, " hrs ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_10_span_1_Template, 2, 1, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.HoursToCoverDistance && drshouldGo_r86.HoursToCoverDistance != "");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_span_13_i_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "i", 53);
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_27_div_1_div_1_span_13_i_1_Template, 1, 0, "i", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.IsOnlyNightDelivery == true);
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r107 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_27_div_1_div_1_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r107); const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r105 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r105.pushItem(drshouldGo_r86, drshouldGo_r86.JobId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_3_Template, 5, 2, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_9_Template, 2, 1, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DeliveryRequestDisplayComponent_div_27_div_1_div_1_div_10_Template, 2, 1, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "span", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, DeliveryRequestDisplayComponent_div_27_div_1_div_1_span_13_Template, 2, 1, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_27_div_1_div_1_Template_a_click_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r107); const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r108 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r108.bindDeliveryRequests(drshouldGo_r86, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "shouldgo-", drshouldGo_r86.JobId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r87.checkItemsExists(drshouldGo_r86.JobId, drshouldGo_r86.DeliveryRequestType, drshouldGo_r86.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.DRQueueAttributes.CustomerName == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.JobName, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drshouldGo_r86.JobCity, " \u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.DRQueueAttributes.TrailerCompatibility == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.DRQueueAttributes.HoursToCoverDistance == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.DRQueueAttributes.DeliveryShift == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_2_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drshouldGo_r86.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drshouldGo_r86.ProductType, 0, 10) + ".." : drshouldGo_r86.ProductType, " ", "(" + drshouldGo_r86.RequiredQuantity + (drshouldGo_r86.UoM == 1 ? " G" : " L") + ")", " ", drshouldGo_r86.DeliveryRequests.length > 1 ? ": (+" + (drshouldGo_r86.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_2_ng_template_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drshouldGo_r86.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drshouldGo_r86.ProductType, 0, 10) + ".." : drshouldGo_r86.ProductType, " ", "(" + drshouldGo_r86.ScheduleQuantityTypeText + ")", " ", drshouldGo_r86.DeliveryRequests.length > 1 ? ": (+" + (drshouldGo_r86.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_27_div_1_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r118 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_27_div_1_div_2_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r118); const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r116 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r116.pushItem(drshouldGo_r86, drshouldGo_r86.TBDGroupId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DeliveryRequestDisplayComponent_div_27_div_1_div_2_div_4_Template, 3, 7, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_div_27_div_1_div_2_ng_template_5_Template, 3, 7, "ng-template", null, 56, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_27_div_1_div_2_Template_a_click_8_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r118); const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r119 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r119.bindDeliveryRequests(drshouldGo_r86, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r112 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](6);
    const drshouldGo_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r88 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "shouldgo-", drshouldGo_r86.TBDGroupId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r88.checkItemsExists(drshouldGo_r86.TBDGroupId, drshouldGo_r86.DeliveryRequestType, drshouldGo_r86.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", " TBD- ", drshouldGo_r86.ProductType + " -", " ", drshouldGo_r86.RequiredQuantity == 0 ? drshouldGo_r86.ScheduleQuantityTypeText : drshouldGo_r86.RequiredQuantity + (drshouldGo_r86.UoM == 1 ? " G" : " L"), "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.RequiredQuantity > 0)("ngIfElse", _r112);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_27_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_27_div_1_div_1_Template, 16, 9, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_27_div_1_div_2_Template, 10, 7, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drshouldGo_r86 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !drshouldGo_r86.IsTBD);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drshouldGo_r86.IsTBD);
} }
function DeliveryRequestDisplayComponent_div_27_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_27_div_1_Template, 3, 2, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r5.shouldGoRequests);
} }
function DeliveryRequestDisplayComponent_div_28_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " No request available ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.CustomerBrandId, " ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.CustomerCompany.length > 6 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 1, drcouldGo_r123.CustomerCompany, 0, 6) + ".." : drcouldGo_r123.CustomerCompany, " -\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_div_2_Template, 2, 1, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_ng_template_3_Template, 3, 5, "ng-template", null, 47, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r131 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](4);
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.CustomerBrandId)("ngIfElse", _r131);
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.TrailerCompatibility, " ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_10_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.HoursToCoverDistance, " hrs ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_10_span_1_Template, 2, 1, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.HoursToCoverDistance && drcouldGo_r123.HoursToCoverDistance != "");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_span_13_i_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "i", 53);
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_33_div_1_div_1_span_13_i_1_Template, 1, 0, "i", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.IsOnlyNightDelivery == true);
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r144 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_33_div_1_div_1_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r144); const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r142 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r142.pushItem(drcouldGo_r123, drcouldGo_r123.JobId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_3_Template, 5, 2, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_9_Template, 2, 1, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DeliveryRequestDisplayComponent_div_33_div_1_div_1_div_10_Template, 2, 1, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "span", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, DeliveryRequestDisplayComponent_div_33_div_1_div_1_span_13_Template, 2, 1, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_33_div_1_div_1_Template_a_click_14_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r144); const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r145 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r145.bindDeliveryRequests(drcouldGo_r123, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r124 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "couldgo-", drcouldGo_r123.JobId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r124.checkItemsExists(drcouldGo_r123.JobId, drcouldGo_r123.DeliveryRequestType, drcouldGo_r123.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.DRQueueAttributes.CustomerName == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.JobName, "\u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", drcouldGo_r123.JobCity, " \u00A0 ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.DRQueueAttributes.TrailerCompatibility == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.DRQueueAttributes.HoursToCoverDistance == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.DRQueueAttributes.DeliveryShift == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_2_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drcouldGo_r123.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drcouldGo_r123.ProductType, 0, 10) + ".." : drcouldGo_r123.ProductType, " ", "(" + drcouldGo_r123.RequiredQuantity + (drcouldGo_r123.UoM == 1 ? " G" : " L") + ")", " ", drcouldGo_r123.DeliveryRequests.length > 1 ? ": (+" + (drcouldGo_r123.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_2_ng_template_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "slice");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"](" TBD - ", drcouldGo_r123.ProductType.length > 10 ? _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind3"](2, 3, drcouldGo_r123.ProductType, 0, 10) + ".." : drcouldGo_r123.ProductType, " ", "(" + drcouldGo_r123.ScheduleQuantityTypeText + ")", " ", drcouldGo_r123.DeliveryRequests.length > 1 ? ": (+" + (drcouldGo_r123.DeliveryRequests.length - 1) + ")" : "", " \u00A0\u00A0 ");
} }
function DeliveryRequestDisplayComponent_div_33_div_1_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r155 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_33_div_1_div_2_Template_div_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r155); const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r153 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r153.pushItem(drcouldGo_r123, drcouldGo_r123.TBDGroupId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DeliveryRequestDisplayComponent_div_33_div_1_div_2_div_4_Template, 3, 7, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_div_33_div_1_div_2_ng_template_5_Template, 3, 7, "ng-template", null, 56, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DeliveryRequestDisplayComponent_div_33_div_1_div_2_Template_a_click_8_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r155); const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r156 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2); return ctx_r156.bindDeliveryRequests(drcouldGo_r123, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const _r149 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](6);
    const drcouldGo_r123 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    const ctx_r125 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "couldgo-", drcouldGo_r123.TBDGroupId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", ctx_r125.checkItemsExists(drcouldGo_r123.TBDGroupId, drcouldGo_r123.DeliveryRequestType, drcouldGo_r123.IsTBD));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("ngbTooltip", " TBD- ", drcouldGo_r123.ProductType + " -", " ", drcouldGo_r123.RequiredQuantity == 0 ? drcouldGo_r123.ScheduleQuantityTypeText : drcouldGo_r123.RequiredQuantity + (drcouldGo_r123.UoM == 1 ? " G" : " L"), "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.RequiredQuantity > 0)("ngIfElse", _r149);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngbPopover", _r9);
} }
function DeliveryRequestDisplayComponent_div_33_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_33_div_1_div_1_Template, 16, 9, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_div_33_div_1_div_2_Template, 10, 7, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drcouldGo_r123 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !drcouldGo_r123.IsTBD);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drcouldGo_r123.IsTBD);
} }
function DeliveryRequestDisplayComponent_div_33_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_div_33_div_1_Template, 3, 2, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r7.couldGoRequests);
} }
function DeliveryRequestDisplayComponent_div_34_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " No request available ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const dr_r160 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r160.AdditiveProductName);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](", ", dr_r160.BlendedProductName, "");
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const dr_r160 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r160.ProductType);
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "number");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const dr_r160 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"]("", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](2, 2, dr_r160.RequiredQuantity, "1.0-2"), "", dr_r160.UoM == 1 ? "G" : "L", "");
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "number");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const dr_r160 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"]("", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](2, 2, dr_r160.TotalBlendedQuantity, "1.0-2"), "", dr_r160.UoM == 1 ? "G" : "L", "");
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_10_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const dr_r160 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r160.ScheduleQuantityTypeText);
} }
const _c0 = function (a0, a1, a2) { return { "must-go": a0, "should-go": a1, "could-go": a2 }; };
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_4_Template, 4, 2, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_5_Template, 2, 1, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_6_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_8_Template, 3, 5, "span", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_9_Template, 3, 5, "span", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_span_10_Template, 2, 1, "span", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r174 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    const i_r161 = ctx_r174.index;
    const dr_r160 = ctx_r174.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "dr_", i_r161 + 1, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction3"](8, _c0, dr_r160.Priority == 1, dr_r160.Priority == 2, dr_r160.Priority == 3));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.IsBlendedRequest === true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.IsBlendedRequest != true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.IsAutoCreatedDR);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.RequiredQuantity > 0 && !dr_r160.IsBlendedRequest);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.RequiredQuantity > 0 && dr_r160.IsBlendedRequest);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r160.RequiredQuantity == 0);
} }
function DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_div_1_Template, 11, 12, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const dr_r160 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !dr_r160.IsBlendedRequest || dr_r160.IsBlendedDrParent);
} }
function DeliveryRequestDisplayComponent_ng_template_35_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DeliveryRequestDisplayComponent_ng_template_35_ng_container_2_Template, 2, 1, "ng-container", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r10.selectedJobRequests);
} }
class DeliveryRequestDisplayComponent {
    constructor(carrierService, sbService, deliveryRequestService, fb, route) {
        this.carrierService = carrierService;
        this.sbService = sbService;
        this.deliveryRequestService = deliveryRequestService;
        this.fb = fb;
        this.route = route;
        this.deliveryRequests = [];
        this.selectedJobRequests = [];
        this.mustGoRequests = [];
        this.tempmustGoRequests = [];
        this.localStorageMustGoRequests = [];
        this.missedRequests = [];
        this.tempMissedRequests = [];
        this.localStorageMissedRequests = [];
        this.shouldGoRequests = [];
        this.tempshouldGoRequests = [];
        this.localStorageShouldGoRequests = [];
        this.couldGoRequests = [];
        this.tempcouldGoRequests = [];
        this.localStorageCouldGoRequests = [];
        this.requestToUpdate = new _carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_2__["DeliveryRequestViewModel"](false);
        this.Regions = [];
        this.assignedByOtherRegionRequests = [];
        this.assignedByOtherOperatorRequests = [];
        this.assignedToOtherRegionRequests = [];
        this.assignedToOtherOperatorRequests = [];
        this.changeDeteaction = false;
        this.IsThisFromDrDisplay = false;
        this._loadingDrRequests = false;
    }
    ngOnInit() {
        localStorage.setItem("deliveryRequests", JSON.stringify([]));
        this.regionId = this.route.snapshot.queryParamMap.get('regionId');
        this.selectedDate = this.route.snapshot.queryParamMap.get('selectedDate');
        if (!this.selectedDate || this.selectedDate.indexOf('null') !== -1 || this.selectedDate.indexOf('undefined') !== -1)
            this.selectedDate = '';
        this.getDeliveryRequests(regionId);
        this.SbForm = this.initForm();
        localStorage.setItem("regionId", JSON.stringify(regionId));
        this.changeDeteaction = false;
        this.IsThisFromDrDisplay = true;
        this.SbForm.get('searchField').valueChanges
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["debounceTime"])(500), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["distinctUntilChanged"])())
            .subscribe(searchText => {
            this.filterRecords(searchText);
        });
        this.checkRegionChange();
        this.checkRecordUpdateORDelete();
    }
    initForm() {
        var _form = this.fb.group({
            searchField: this.fb.control('')
        });
        return _form;
    }
    checkRegionChange() {
        this.intervalTime = setInterval(() => {
            if (IsUserActive()) {
                if (localStorage.getItem("refreshRegion") != null) {
                    var changeDeteaction = JSON.parse(localStorage.getItem("refreshRegion"));
                    if (changeDeteaction) {
                        this.changeDeteaction = changeDeteaction;
                        this.resetLocalStorage();
                        setTimeout(function () {
                            window.top.close(); // close current tab after 10 seconds
                        }, 10000);
                    }
                }
            }
        }, 3000);
    }
    checkRecordUpdateORDelete() {
        this.updateintervalTime = setInterval(() => {
            if (IsUserActive()) {
                if (localStorage.getItem("updateRequest") != null) {
                    var updateRequest = JSON.parse(localStorage.getItem("updateRequest"));
                    if (updateRequest) {
                        localStorage.setItem("updateRequest", JSON.stringify(false));
                        window.location.reload(); //refresh the current window
                    }
                }
            }
        }, 5000);
    }
    filterRecords(term) {
        if (term) {
            term = term.trim().toLowerCase();
            this.filterMustGoRequest(term);
            this.filterShouldGoRequest(term);
            this.filterCouldGoRequest(term);
            this.filterMissedRequest(term);
        }
        else {
            this.mustGoRequests = this.tempmustGoRequests;
            this.shouldGoRequests = this.tempshouldGoRequests;
            this.couldGoRequests = this.tempcouldGoRequests;
            this.missedRequests = this.tempMissedRequests;
        }
    }
    filterMustGoRequest(term) {
        let _localmustGoRequests = [];
        this.tempmustGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localmustGoRequests.push(dr); });
            }
        });
        let _mustGorecords = _localmustGoRequests.filter((element) => ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
            || (element.JobName && element.JobName.toLowerCase().startsWith(term))
            || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
            || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
            || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
            || element.RequiredQuantity.toString().startsWith(term)));
        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_mustGorecords);
        this.mustGoRequests = this.deliveryRequestService.getMustGoRequests(groupedDrs);
        this.mustGoRequests.slice();
    }
    filterShouldGoRequest(term) {
        let _localshouldGoRequests = [];
        this.tempshouldGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localshouldGoRequests.push(dr); });
            }
        });
        let _shouldGorecords = _localshouldGoRequests.filter((element) => ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
            || (element.JobName && element.JobName.toLowerCase().startsWith(term))
            || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
            || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
            || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
            || element.RequiredQuantity.toString().startsWith(term)));
        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_shouldGorecords);
        this.shouldGoRequests = this.deliveryRequestService.getShouldGoRequests(groupedDrs);
        this.shouldGoRequests.slice();
    }
    filterCouldGoRequest(term) {
        let _localcouldGoRequests = [];
        this.tempcouldGoRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localcouldGoRequests.push(dr); });
            }
        });
        let _couldGorecords = _localcouldGoRequests.filter((element) => ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
            || (element.JobName && element.JobName.toLowerCase().startsWith(term))
            || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
            || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
            || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
            || element.RequiredQuantity.toString().startsWith(term)));
        let groupedDrs = this.deliveryRequestService.groupDrsByJob(_couldGorecords);
        this.couldGoRequests = this.deliveryRequestService.getCouldGoRequests(groupedDrs);
        this.couldGoRequests.slice();
    }
    filterMissedRequest(term) {
        let _localMissedRequests = [];
        this.tempMissedRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _localMissedRequests.push(dr); });
            }
        });
        let _missedrecords = _localMissedRequests.filter((element) => ((element.CustomerCompany && element.CustomerCompany.toLowerCase().startsWith(term))
            || (element.JobName && element.JobName.toLowerCase().startsWith(term))
            || (element.JobAddress && element.JobAddress.toLowerCase().startsWith(term))
            || (element.RouteInfo && element.RouteInfo.Name && element.RouteInfo.Name.toLowerCase().startsWith(term))
            || (element.ProductType && element.ProductType.toLowerCase().startsWith(term))
            || element.RequiredQuantity.toString().startsWith(term)));
        this.missedRequests = this.deliveryRequestService.groupDrsByJob(_missedrecords, src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].Missed);
        this.missedRequests.slice();
    }
    ngOnDestroy() {
        localStorage.setItem("deliveryRequests", JSON.stringify([]));
    }
    getDeliveryRequests(regionId) {
        this._loadingDrRequests = true;
        this.carrierService.getDeliveryRequests(regionId, this.selectedDate).subscribe(dr => {
            if (dr != null && dr != undefined) {
                dr = dr.filter(t => !t.IsCalendarView); // hide calender Dr
                dr = this.filterDrByScheduleBuilder(dr);
                this.deliveryRequests = dr;
                var priorityRequests = this.deliveryRequests.filter(t => t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId);
                var missedRequests = this.deliveryRequests.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
                var groupedDrs = this.deliveryRequestService.groupDrsByJob(priorityRequests);
                //
                this.mustGoRequests = this.deliveryRequestService.getMustGoRequests(groupedDrs);
                this.mustGoRequests.slice();
                this.tempmustGoRequests = this.mustGoRequests;
                this.tempmustGoRequests.slice();
                //
                this.shouldGoRequests = this.deliveryRequestService.getShouldGoRequests(groupedDrs);
                this.shouldGoRequests.slice();
                this.tempshouldGoRequests = this.shouldGoRequests;
                this.tempshouldGoRequests.slice();
                //
                this.couldGoRequests = this.deliveryRequestService.getCouldGoRequests(groupedDrs);
                this.couldGoRequests.slice();
                this.tempcouldGoRequests = this.couldGoRequests;
                this.tempcouldGoRequests.slice();
                //
                this.missedRequests = this.deliveryRequestService.groupDrsByJob(missedRequests, src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].Missed);
                this.missedRequests.slice();
                this.tempMissedRequests = this.missedRequests;
                this.missedRequests.slice();
                this.assignedByOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedByOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId != currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedToOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.assignedToOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId != currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
                this.getLocalStorageItems();
            }
            this._loadingDrRequests = false;
        });
    }
    filterDrByScheduleBuilder(drs) {
        var _scheduleRequests = [];
        if (this.ScheduleBuilder != undefined && this.ScheduleBuilder != null) {
            this.ScheduleBuilder.Shifts.forEach(s => {
                s.Schedules.forEach(sc => {
                    sc.Trips.forEach(t => {
                        t.DeliveryRequests.forEach(d => {
                            _scheduleRequests.push(d.Id);
                        });
                    });
                });
            });
            drs = drs.filter(x => {
                return _scheduleRequests.find(y => y == x.Id) == undefined;
            });
        }
        return drs;
    }
    removeDraggedRequest(drData, deliveryRequests) {
        var index = deliveryRequests.findIndex(x => x.Priority == drData.Priority && x.Id == drData.Id);
        if (index >= 0) {
            deliveryRequests = deliveryRequests.splice(index, 1);
        }
    }
    getSelectedLocationPriority(jobId, isMissed, isTBD) {
        var response = null;
        if (isMissed) {
            response = this.missedRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
            return response;
        }
        response = this.mustGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
        if (!response) {
            response = this.shouldGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
            if (!response)
                response = this.couldGoRequests.find(t => ((!isTBD && t.JobId == jobId) || (isTBD && t.TBDGroupId == jobId)));
        }
        return response;
    }
    getLocalStorageItems() {
        var mustGoDeliveryRequests = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) || [];
        var shouldGodeliveryRequests = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) || [];
        var couldGodeliveryRequests = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) || [];
        var missedDeliveryRequests = JSON.parse(localStorage.getItem("missedDeliveryRequest")) || [];
        if (mustGoDeliveryRequests != null) {
            mustGoDeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    mustGoDeliveryRequests[i] = request;
                    if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].ShouldGo) {
                        shouldGodeliveryRequests.push(request);
                        mustGoDeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].CouldGo) {
                        couldGodeliveryRequests.push(request);
                        mustGoDeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    mustGoDeliveryRequests.splice(i, 1);
                }
            });
        }
        if (shouldGodeliveryRequests != null) {
            shouldGodeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    shouldGodeliveryRequests[i] = request;
                    if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].MustGo) {
                        mustGoDeliveryRequests.push(request);
                        shouldGodeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].CouldGo) {
                        couldGodeliveryRequests.push(request);
                        shouldGodeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    shouldGodeliveryRequests.splice(i, 1);
                }
            });
        }
        if (couldGodeliveryRequests != null) {
            couldGodeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, false, t.IsTBD);
                if (request) {
                    couldGodeliveryRequests[i] = request;
                    if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].MustGo) {
                        mustGoDeliveryRequests.push(request);
                        couldGodeliveryRequests.splice(i, 1);
                    }
                    else if (request.Priority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].ShouldGo) {
                        shouldGodeliveryRequests.push(request);
                        couldGodeliveryRequests.splice(i, 1);
                    }
                }
                else {
                    couldGodeliveryRequests.splice(i, 1);
                }
            });
        }
        if (missedDeliveryRequests != null) {
            missedDeliveryRequests.forEach((t, i) => {
                var request = this.getSelectedLocationPriority(t.IsTBD ? t.TBDGroupId : t.JobId, true, t.IsTBD);
                if (request) {
                    missedDeliveryRequests[i] = request;
                }
                else {
                    missedDeliveryRequests.splice(i, 1);
                }
            });
        }
        this.localStorageMustGoRequests = mustGoDeliveryRequests;
        this.localStorageMustGoRequests.slice();
        localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(mustGoDeliveryRequests));
        this.localStorageShouldGoRequests = shouldGodeliveryRequests;
        this.localStorageShouldGoRequests.slice();
        localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(shouldGodeliveryRequests));
        this.localStorageCouldGoRequests = couldGodeliveryRequests;
        this.localStorageCouldGoRequests.slice();
        localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(couldGodeliveryRequests));
        this.localStorageMissedRequests = missedDeliveryRequests;
        this.localStorageMissedRequests.slice();
        localStorage.setItem("missedDeliveryRequest", JSON.stringify(missedDeliveryRequests));
        localStorage.setItem("refreshLocalStorage", "true");
    }
    pushItem(location, elementId) {
        let isTbd = location.IsTBD;
        localStorage.setItem("refreshLocalStorage", 'true');
        location.DeliveryRequests.forEach(t => { t.WindowMode = 2; t.QueueMode = 2; });
        var element = $("#" + elementId);
        if (location.DeliveryRequestType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].Missed) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageMissedRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageMissedRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("missedDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
                else {
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else if (location.DeliveryRequestType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].MustGo) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageMustGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageMustGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageMustGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("mustGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
                else {
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else if (location.DeliveryRequestType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].ShouldGo) {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageShouldGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("shouldGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
                else {
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
        else {
            var index = -1;
            if (isTbd == false)
                index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == location.JobId);
            else
                index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == location.TBDGroupId);
            if (index == -1) {
                $(element).addClass('selected');
                this.localStorageCouldGoRequests.unshift(location);
                var deliveryRequests = localStorage.getItem("couldGoDeliveryRequest");
                if (deliveryRequests != null) {
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
                else {
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
            }
            else {
                this.removeItem(isTbd ? location.TBDGroupId : location.JobId, location.DeliveryRequestType, isTbd);
                $(element).removeClass('selected');
            }
        }
    }
    removeItem(locationId, locationPriority, isTBD) {
        localStorage.setItem("refreshLocalStorage", 'true');
        if (locationPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].Missed) {
            if (this.localStorageMissedRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageMissedRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageMissedRequests.splice(index, 1);
                    this.localStorageMissedRequests.slice();
                    localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.localStorageMissedRequests));
                }
            }
        }
        else if (locationPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].MustGo) {
            if (this.localStorageMustGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageMustGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageMustGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageMustGoRequests.splice(index, 1);
                    this.localStorageMustGoRequests.slice();
                    localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.localStorageMustGoRequests));
                }
            }
        }
        else if (locationPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].ShouldGo) {
            if (this.localStorageShouldGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageShouldGoRequests.splice(index, 1);
                    this.localStorageShouldGoRequests.slice();
                    localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.localStorageShouldGoRequests));
                }
            }
        }
        else {
            if (this.localStorageCouldGoRequests.length > 0) {
                var index = -1;
                if (isTBD == false)
                    index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == locationId);
                else
                    index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == locationId);
                if (index >= 0) {
                    this.localStorageCouldGoRequests.splice(index, 1);
                    this.localStorageCouldGoRequests.slice();
                    localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.localStorageCouldGoRequests));
                }
            }
        }
    }
    resetLocalStorage() {
        localStorage.setItem("refreshLocalStorage", 'true');
        localStorage.setItem("mustGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("couldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("missedDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("refreshRegion", JSON.stringify(false));
        localStorage.setItem("regionId", JSON.stringify(''));
        localStorage.setItem("updateRequest", JSON.stringify(false));
    }
    bindDeliveryRequests(location, $event) {
        $event.stopPropagation();
        this.selectedJobRequests = Object(_my_functions__WEBPACK_IMPORTED_MODULE_3__["sortBy"])(location.DeliveryRequests, 'ProductType');
    }
    checkItemsExists(jobId, drType, isTbd) {
        var updateRequest = null;
        if (localStorage.getItem("recordPriorityChange") != null) {
            updateRequest = JSON.parse(localStorage.getItem("recordPriorityChange"));
        }
        if (drType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].Missed) {
            if (this.localStorageMissedRequests.length > 0) {
                let index = -1;
                if (isTbd == false)
                    index = this.localStorageMissedRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageMissedRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    var drPriority = this.deliveryRequestService.getPriority(this.localStorageMissedRequests[index].DeliveryRequests);
                    if (drPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].MustGo) {
                        return "radius-5 pa10 bg-lightgrey mustgo selected";
                    }
                    if (drPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].ShouldGo) {
                        return "radius-5 pa10 bg-lightgrey shouldgo selected";
                    }
                    if (drPriority == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryReqPriority"].CouldGo) {
                        return "radius-5 pa10 bg-lightgrey couldgo selected";
                    }
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    if (drPriority == 1) {
                        return "radius-5 pa10 bg-lightgrey mustgo selected";
                    }
                    if (drPriority == 2) {
                        return "radius-5 pa10 bg-lightgrey shouldgo selected";
                    }
                    if (drPriority == 3) {
                        return "radius-5 pa10 bg-lightgrey couldgo selected";
                    }
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].MustGo) {
            if (this.localStorageMustGoRequests.length > 0) {
                var index = -1;
                if (isTbd == false)
                    index = this.localStorageMustGoRequests.findIndex(x => x && x.JobId && x.JobId == jobId);
                else
                    index = this.localStorageMustGoRequests.findIndex(x => x && x.TBDGroupId && x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].ShouldGo) {
            if (this.localStorageShouldGoRequests.length > 0) {
                var index = -1;
                if (isTbd == false)
                    index = this.localStorageShouldGoRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageShouldGoRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
        else if (drType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["DeliveryRequestTypes"].CouldGo) {
            if (this.localStorageCouldGoRequests.length > 0) {
                let index = -1;
                if (isTbd == false)
                    index = this.localStorageCouldGoRequests.findIndex(x => x.JobId == jobId);
                else
                    index = this.localStorageCouldGoRequests.findIndex(x => x.TBDGroupId == jobId);
                if (index >= 0) {
                    return "radius-5 pa10 bg-lightgrey  selected";
                }
                else if (updateRequest && ((updateRequest.IsTBD == false && updateRequest.JobId && updateRequest.JobId == jobId) || (updateRequest.IsTBD == true && updateRequest.TBDGroupId && updateRequest.TBDGroupId == jobId))) {
                    return "radius-5 pa10 bg-lightgrey selected";
                }
                else {
                    return "radius-5 pa10 bg-lightgrey";
                }
            }
            else {
                return "radius-5 pa10 bg-lightgrey";
            }
        }
    }
    GetDrsForMultiWindow() {
        this.getDeliveryRequests(regionId);
    }
}
DeliveryRequestDisplayComponent.ɵfac = function DeliveryRequestDisplayComponent_Factory(t) { return new (t || DeliveryRequestDisplayComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_carrier_service_schedule_builder_service__WEBPACK_IMPORTED_MODULE_6__["ScheduleBuilderService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_carrier_service_deliveryrequest_service__WEBPACK_IMPORTED_MODULE_7__["DeliveryrequestService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_9__["ActivatedRoute"])); };
DeliveryRequestDisplayComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: DeliveryRequestDisplayComponent, selectors: [["app-delivery-request-display"]], decls: 37, vars: 12, consts: [[1, "row"], [1, "col-sm-12", "text-right"], [1, "col-sm-12"], [1, "container-fluid", 3, "formGroup"], ["class", "row", 4, "ngIf"], [1, "row", "mb10"], [1, "col-sm-6"], [1, "dib", "fs16", "mr10", "mt0"], [1, "dib"], ["type", "text", "placeholder", "Search", "formControlName", "searchField", 1, "form-control", "radius-10"], [3, "IsThisFromDrDisplay", "SelectedRegionId", "OnRaiseDRFromMultiWindow"], [1, "pr"], ["class", "pa bg-white z-index5 loading-wrapper", 4, "ngIf"], ["class", "radius-5 border shadowb shadow-b", 4, "ngIf"], [1, "radius-5", "border", "shadowb", "shadow-b", "mb20"], [1, "fs11", "mt5", "mb5", "ml10", "dib", "mustgo-status", "pt5", "pb5", "pl10", "pr10", "radius-10"], [1, "mustgo-wrapper"], [1, "fs11", "mt5", "mb5", "ml10", "dib", "shouldgo-status", "pt5", "pb5", "pl10", "pr10", "radius-10"], [1, "shouldgo-wrapper"], [1, "radius-5", "border", "shadowb", "shadow-b"], [1, "fs11", "mt5", "mb5", "ml10", "dib", "couldgo-status", "pt5", "pb5", "pl10", "pr10", "radius-10"], [1, "couldgo-wrapper"], ["class", "row mb10", 4, "ngIf"], ["popContent", ""], [1, "alert", "alert-warning", "fs12", "radius-10"], [1, "pa", "bg-white", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [1, "fs11", "mt5", "mb5", "ml10", "dib", "missed-status", "pt5", "pb5", "pl10", "pr10", "radius-10"], [1, "missed-wrapper"], ["class", "col-sm-3 mb10", 4, "ngFor", "ngForOf"], [1, "col-sm-3", "mb10"], ["class", "dr-queue", "style", "cursor:pointer;", 3, "ngClass", "id", "click", 4, "ngIf"], [1, "dr-queue", 2, "cursor", "pointer", 3, "ngClass", "id", "click"], [1, "col-10", "dr-info"], ["class", "custom_setting", 4, "ngIf"], [1, "job-location"], [1, "job-city"], [1, "custom_setting"], ["class", "compability-type", 4, "ngIf"], ["class", "custom_settings", 4, "ngIf"], [1, "click-icon"], [1, "mr5"], [4, "ngIf"], ["type", "button", "placement", "right", "container", "body", "popoverClass", "dr-popover", 3, "ngbPopover", "click"], [1, "fas", "fa-arrow-right"], [1, "brand-name"], [4, "ngIf", "ngIfElse"], ["customerCompany", ""], [1, "compability-type"], [1, "custom_settings"], ["class", "duration", 4, "ngIf"], [1, "duration"], ["class", "fas fa-moon fs12", 4, "ngIf"], [1, "fas", "fa-moon", "fs12"], ["placement", "bottom-left", "container", "body", 1, "col-10", "dr-info", 3, "ngbTooltip"], ["class", "brand-name", 4, "ngIf", "ngIfElse"], ["ScheduleQuantityTypeText", ""], [1, "col-sm-2", "mb10"], [1, "popover-details"], [4, "ngFor", "ngForOf"], ["class", "col-12 product-details", 3, "id", "ngClass", 4, "ngIf"], [1, "col-12", "product-details", 3, "id", "ngClass"], [1, "col-8"], [1, "product-name"], [1, "col-4"], ["class", "product-qty", 4, "ngIf"], [1, "text-muted"], ["title", "Auto-Generated", 1, "fas", "fa-magic", "ml10"], [1, "product-qty"]], template: function DeliveryRequestDisplayComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DeliveryRequestDisplayComponent_div_5_Template, 7, 0, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "h1", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Delivery Request");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "app-dip-test", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("OnRaiseDRFromMultiWindow", function DeliveryRequestDisplayComponent_Template_app_dip_test_OnRaiseDRFromMultiWindow_13_listener() { return ctx.GetDrsForMultiWindow(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, DeliveryRequestDisplayComponent_div_15_Template, 2, 0, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, DeliveryRequestDisplayComponent_div_16_Template, 6, 2, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "h2", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Must Go");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, DeliveryRequestDisplayComponent_div_21_Template, 2, 1, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, DeliveryRequestDisplayComponent_div_22_Template, 3, 0, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "h2", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Should Go");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, DeliveryRequestDisplayComponent_div_27_Template, 2, 1, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, DeliveryRequestDisplayComponent_div_28_Template, 3, 0, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "h2", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Could Go");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](33, DeliveryRequestDisplayComponent_div_33_Template, 2, 1, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](34, DeliveryRequestDisplayComponent_div_34_Template, 3, 0, "div", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](35, DeliveryRequestDisplayComponent_ng_template_35_Template, 3, 1, "ng-template", null, 23, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.SbForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.changeDeteaction);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("IsThisFromDrDisplay", true)("SelectedRegionId", ctx.regionId);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx._loadingDrRequests);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.missedRequests.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.mustGoRequests.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.mustGoRequests.length == 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.shouldGoRequests.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.shouldGoRequests.length == 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.couldGoRequests.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.couldGoRequests.length == 0);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormControlName"], _shared_components_dip_test_dip_test_component__WEBPACK_IMPORTED_MODULE_11__["DipTestComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__["NgbPopover"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_12__["NgbTooltip"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_10__["SlicePipe"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["DecimalPipe"]], styles: [".icon_menu {\n  display: none;\n  position: absolute;\n  top: 0;\n  right: 5px;\n  /*background: #ffffff;*/\n  background: #7e7b7b;\n  padding: 5px 5px;\n  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);\n  border-radius: 3px;\n}\n\n.dr_cards_new:hover .icon_menu {\n  display: block;\n}\n\n.route_text {\n  color: #c95e61;\n  font-size: 10px;\n  font-weight: bold;\n}\n\n.dr-queue {\n  padding: 5px;\n  /*background: #FDD6D6;*/\n  border-radius: 10px;\n  position: relative;\n  margin-bottom: 2px;\n}\n\n.dr-queue .dr-info {\n  display: flex;\n  align-items: baseline;\n}\n\n.dr-queue .dr-info .brand-name {\n  font-weight: 600;\n  font-size: 14px;\n  line-height: 17px;\n  letter-spacing: 0.25px;\n  color: #12121F;\n  text-transform: uppercase;\n}\n\n.dr-queue .dr-info .job-location {\n  font-weight: 600;\n  font-size: 13px;\n  line-height: 16px;\n  letter-spacing: 0.25px;\n  color: #12121F;\n  text-transform: capitalize;\n  max-width: 40%;\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n}\n\n.dr-queue .dr-info .job-city {\n  font-weight: 600;\n  font-size: 11px;\n  line-height: 13px;\n  letter-spacing: 0.25px;\n  color: #12121F;\n  max-width: 25%;\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  text-transform: uppercase;\n}\n\n.dr-queue .dr-info .compability-type {\n  font-weight: 600;\n  font-size: 11px;\n  line-height: 13px;\n  letter-spacing: 0.25px;\n  color: #12121F;\n}\n\n.dr-queue .duration {\n  position: absolute;\n  bottom: 2px;\n  right: 10px;\n  font-weight: 600;\n  font-size: 11px;\n  line-height: 13px;\n  letter-spacing: 0.25px;\n  color: #D34E4E;\n}\n\n.dr-queue .click-icon {\n  position: absolute;\n  top: 0;\n  right: 10px;\n}\n\n.dr-queue.must-go {\n  background: #FDD6D6;\n}\n\n.dr-queue.should-go {\n  background: #FFDDB5;\n}\n\n.dr-queue.could-go {\n  background: #DCDCDC;\n}\n\n.dr-popover.popover {\n  min-width: 300px;\n  max-width: 350px;\n  background: #F9F9F9;\n  border: 1px solid #E9E7E7;\n  box-sizing: border-box;\n  box-shadow: 10px 10px 8px -2px rgba(0, 0, 0, 0.13);\n  border-radius: 10px;\n}\n\n.dr-popover.popover .popover-body {\n  max-height: 350px;\n  overflow-y: auto;\n  overflow-x: hidden;\n  padding: 0;\n  border-radius: 5px;\n}\n\n.dr-popover.popover .popover-details {\n  padding: 3px 20px;\n  max-height: 310px;\n  overflow-y: auto;\n}\n\n.dr-popover.popover .popover-details .product-details {\n  padding: 2px 10px;\n  border-radius: 10px;\n  margin-bottom: 5px;\n}\n\n.dr-popover.popover .popover-details .product-details .product-name {\n  font-weight: 600;\n  font-size: 14px;\n  line-height: 17px;\n  letter-spacing: 0.25px;\n  color: #12121F;\n}\n\n.dr-popover.popover .popover-details .product-details .product-qty {\n  font-weight: 600;\n  font-size: 14px;\n  line-height: 17px;\n  letter-spacing: 0.25px;\n  color: #BB4141;\n  float: right;\n  text-align: right;\n}\n\n.dr-popover.popover .popover-details .product-details .icon-tray {\n  text-align: center;\n  display: flex;\n  place-content: center;\n  transition: all 0.3s ease-out;\n  opacity: 0;\n  height: 0;\n  overflow: hidden;\n}\n\n.dr-popover.popover .popover-details .product-details .icon-tray .circle-icon {\n  background: #fff;\n  border: 0px solid #797979;\n  box-sizing: border-box;\n  border-radius: 50%;\n  margin-right: 5px;\n  width: 25px;\n  height: 25px;\n  display: flex;\n  place-content: center;\n  align-items: center;\n}\n\n.dr-popover.popover .popover-details .product-details .icon-tray .circle-icon a {\n  color: #6F6E6E !important;\n}\n\n.dr-popover.popover .popover-details .product-details .icon-tray .circle-icon i {\n  font-size: 14px;\n}\n\n.dr-popover.popover .popover-details .product-details:hover {\n  box-shadow: 0 0 3px #515151;\n  border: 0;\n  transition: all 0.6s ease-out;\n}\n\n.dr-popover.popover .popover-details .product-details:hover .icon-tray {\n  padding: 3px;\n  transition: all 0.6s ease-out;\n  opacity: 1;\n  height: auto;\n}\n\n.dr-popover.popover .popover-details .must-go {\n  background: #FDD6D6;\n}\n\n.dr-popover.popover .popover-details .must-go .product-qty {\n  color: #BB4141;\n}\n\n.dr-popover.popover .popover-details .should-go {\n  background: #FFDDB5;\n}\n\n.dr-popover.popover .popover-details .should-go .product-qty {\n  color: #E89330;\n}\n\n.dr-popover.popover .popover-details .could-go {\n  background: #DCDCDC;\n}\n\n.dr-popover.popover .popover-details .could-go .product-qty {\n  color: #696969;\n}\n\n.dr-popover.popover .popover-details .in-progress {\n  background: #3D71B8;\n}\n\n.dr-popover.popover .popover-details .in-progress .product-name {\n  color: #ffffff;\n}\n\n.dr-popover.popover .popover-details .in-progress .product-qty {\n  color: #ffffff;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZGVsaXZlcnktcmVxdWVzdC1kaXNwbGF5L0Q6XFxURlNjb2RlXFxTaXRlRnVlbC5FeGNoYW5nZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuU291cmNlQ29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuV2ViL3NyY1xcYXBwXFxkZWxpdmVyeS1yZXF1ZXN0LWRpc3BsYXlcXGRlbGl2ZXJ5LXJlcXVlc3QtZGlzcGxheS5jb21wb25lbnQuc2NzcyIsInNyYy9hcHAvZGVsaXZlcnktcmVxdWVzdC1kaXNwbGF5L2RlbGl2ZXJ5LXJlcXVlc3QtZGlzcGxheS5jb21wb25lbnQuc2NzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNJLGFBQUE7RUFDQSxrQkFBQTtFQUNBLE1BQUE7RUFDQSxVQUFBO0VBQ0EsdUJBQUE7RUFDQSxtQkFBQTtFQUNBLGdCQUFBO0VBQ0EseUNBQUE7RUFDQSxrQkFBQTtBQ0NKOztBREVBO0VBQ0ksY0FBQTtBQ0NKOztBREVBO0VBQ0ksY0FBQTtFQUNBLGVBQUE7RUFDQSxpQkFBQTtBQ0NKOztBRElBO0VBQ0ksWUFBQTtFQUNBLHVCQUFBO0VBQ0EsbUJBQUE7RUFDQSxrQkFBQTtFQUNBLGtCQUFBO0FDREo7O0FER0k7RUFDSSxhQUFBO0VBQ0EscUJBQUE7QUNEUjs7QURHUTtFQUNJLGdCQUFBO0VBQ0EsZUFBQTtFQUNBLGlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxjQUFBO0VBQ0EseUJBQUE7QUNEWjs7QURJUTtFQUNJLGdCQUFBO0VBQ0EsZUFBQTtFQUNBLGlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxjQUFBO0VBQ0EsMEJBQUE7RUFDQSxjQUFBO0VBQ0EsbUJBQUE7RUFDQSxnQkFBQTtFQUNBLHVCQUFBO0FDRlo7O0FES1E7RUFDSSxnQkFBQTtFQUNBLGVBQUE7RUFDQSxpQkFBQTtFQUNBLHNCQUFBO0VBQ0EsY0FBQTtFQUNBLGNBQUE7RUFDQSxtQkFBQTtFQUNBLGdCQUFBO0VBQ0EsdUJBQUE7RUFDQSx5QkFBQTtBQ0haOztBRE1RO0VBQ0ksZ0JBQUE7RUFDQSxlQUFBO0VBQ0EsaUJBQUE7RUFDQSxzQkFBQTtFQUNBLGNBQUE7QUNKWjs7QURTSTtFQUNJLGtCQUFBO0VBQ0EsV0FBQTtFQUNBLFdBQUE7RUFDQSxnQkFBQTtFQUNBLGVBQUE7RUFDQSxpQkFBQTtFQUNBLHNCQUFBO0VBQ0EsY0FBQTtBQ1BSOztBRFVJO0VBQ0ksa0JBQUE7RUFDQSxNQUFBO0VBQ0EsV0FBQTtBQ1JSOztBRFdJO0VBQ0ksbUJBQUE7QUNUUjs7QURZSTtFQUNJLG1CQUFBO0FDVlI7O0FEYUk7RUFDSSxtQkFBQTtBQ1hSOztBRGdCSTtFQUdBLGdCQUFBO0VBQ0EsZ0JBQUE7RUFDQSxtQkFBQTtFQUNBLHlCQUFBO0VBQ0Esc0JBQUE7RUFDQSxrREFBQTtFQUNBLG1CQUFBO0FDZko7O0FEZ0JJO0VBR0EsaUJBQUE7RUFDQSxnQkFBQTtFQUNBLGtCQUFBO0VBQ0EsVUFBQTtFQUNBLGtCQUFBO0FDaEJKOztBRG1CQTtFQUNJLGlCQUFBO0VBQ0EsaUJBQUE7RUFDQSxnQkFBQTtBQ2pCSjs7QURrQkk7RUFHQSxpQkFBQTtFQUNBLG1CQUFBO0VBQ0Esa0JBQUE7QUNsQko7O0FEbUJJO0VBR0EsZ0JBQUE7RUFDQSxlQUFBO0VBQ0EsaUJBQUE7RUFDQSxzQkFBQTtFQUNBLGNBQUE7QUNuQko7O0FEc0JBO0VBQ0ksZ0JBQUE7RUFDQSxlQUFBO0VBQ0EsaUJBQUE7RUFDQSxzQkFBQTtFQUNBLGNBQUE7RUFDQSxZQUFBO0VBQ0EsaUJBQUE7QUNwQko7O0FEdUJBO0VBQ0ksa0JBQUE7RUFDQSxhQUFBO0VBQ0EscUJBQUE7RUFDQSw2QkFBQTtFQUNBLFVBQUE7RUFDQSxTQUFBO0VBQ0EsZ0JBQUE7QUNyQko7O0FEc0JJO0VBR0EsZ0JBQUE7RUFDQSx5QkFBQTtFQUNBLHNCQUFBO0VBQ0Esa0JBQUE7RUFDQSxpQkFBQTtFQUNBLFdBQUE7RUFDQSxZQUFBO0VBQ0EsYUFBQTtFQUNBLHFCQUFBO0VBQ0EsbUJBQUE7QUN0Qko7O0FEdUJJO0VBR0EseUJBQUE7QUN2Qko7O0FEMEJBO0VBQ0ksZUFBQTtBQ3hCSjs7QUQ4QkE7RUFDSSwyQkFBQTtFQUNBLFNBQUE7RUFDQSw2QkFBQTtBQzVCSjs7QUQ2Qkk7RUFHQSxZQUFBO0VBQ0EsNkJBQUE7RUFDQSxVQUFBO0VBQ0EsWUFBQTtBQzdCSjs7QURtQ0E7RUFDSSxtQkFBQTtBQ2pDSjs7QURrQ0k7RUFDQSxjQUFBO0FDaENKOztBRHFDQTtFQUNJLG1CQUFBO0FDbkNKOztBRG9DSTtFQUdBLGNBQUE7QUNwQ0o7O0FEeUNBO0VBQ0ksbUJBQUE7QUN2Q0o7O0FEd0NJO0VBR0EsY0FBQTtBQ3hDSjs7QUQ2Q0E7RUFDSSxtQkFBQTtBQzNDSjs7QUQ0Q0k7RUFHQSxjQUFBO0FDNUNKOztBRCtDQTtFQUNJLGNBQUE7QUM3Q0oiLCJmaWxlIjoic3JjL2FwcC9kZWxpdmVyeS1yZXF1ZXN0LWRpc3BsYXkvZGVsaXZlcnktcmVxdWVzdC1kaXNwbGF5LmNvbXBvbmVudC5zY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmljb25fbWVudSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAwO1xyXG4gICAgcmlnaHQ6IDVweDtcclxuICAgIC8qYmFja2dyb3VuZDogI2ZmZmZmZjsqL1xyXG4gICAgYmFja2dyb3VuZDogIzdlN2I3YjtcclxuICAgIHBhZGRpbmc6IDVweCA1cHg7XHJcbiAgICBib3gtc2hhZG93OiAwIDJweCA0cHggcmdiYSgwLDAsMCwuMDgpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogM3B4O1xyXG59XHJcblxyXG4uZHJfY2FyZHNfbmV3OmhvdmVyIC5pY29uX21lbnUge1xyXG4gICAgZGlzcGxheTogYmxvY2s7XHJcbn1cclxuXHJcbi5yb3V0ZV90ZXh0IHtcclxuICAgIGNvbG9yOiAjYzk1ZTYxO1xyXG4gICAgZm9udC1zaXplOiAxMHB4O1xyXG4gICAgZm9udC13ZWlnaHQ6IGJvbGQ7XHJcbn1cclxuXHJcblxyXG5cclxuLmRyLXF1ZXVlIHtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIC8qYmFja2dyb3VuZDogI0ZERDZENjsqL1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuICAgIHBvc2l0aW9uOiByZWxhdGl2ZTtcclxuICAgIG1hcmdpbi1ib3R0b206IDJweDtcclxuXHJcbiAgICAuZHItaW5mbyB7XHJcbiAgICAgICAgZGlzcGxheTogZmxleDtcclxuICAgICAgICBhbGlnbi1pdGVtczogYmFzZWxpbmU7XHJcblxyXG4gICAgICAgIC5icmFuZC1uYW1lIHtcclxuICAgICAgICAgICAgZm9udC13ZWlnaHQ6IDYwMDtcclxuICAgICAgICAgICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgICAgICAgICBsaW5lLWhlaWdodDogMTdweDtcclxuICAgICAgICAgICAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcclxuICAgICAgICAgICAgY29sb3I6ICMxMjEyMUY7XHJcbiAgICAgICAgICAgIHRleHQtdHJhbnNmb3JtOiB1cHBlcmNhc2U7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAuam9iLWxvY2F0aW9uIHtcclxuICAgICAgICAgICAgZm9udC13ZWlnaHQ6IDYwMDtcclxuICAgICAgICAgICAgZm9udC1zaXplOiAxM3B4O1xyXG4gICAgICAgICAgICBsaW5lLWhlaWdodDogMTZweDtcclxuICAgICAgICAgICAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcclxuICAgICAgICAgICAgY29sb3I6ICMxMjEyMUY7XHJcbiAgICAgICAgICAgIHRleHQtdHJhbnNmb3JtOiBjYXBpdGFsaXplO1xyXG4gICAgICAgICAgICBtYXgtd2lkdGg6IDQwJTtcclxuICAgICAgICAgICAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcclxuICAgICAgICAgICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgICAgICAgICAgdGV4dC1vdmVyZmxvdzogZWxsaXBzaXM7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAuam9iLWNpdHkge1xyXG4gICAgICAgICAgICBmb250LXdlaWdodDogNjAwO1xyXG4gICAgICAgICAgICBmb250LXNpemU6IDExcHg7XHJcbiAgICAgICAgICAgIGxpbmUtaGVpZ2h0OiAxM3B4O1xyXG4gICAgICAgICAgICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xyXG4gICAgICAgICAgICBjb2xvcjogIzEyMTIxRjtcclxuICAgICAgICAgICAgbWF4LXdpZHRoOiAyNSU7XHJcbiAgICAgICAgICAgIHdoaXRlLXNwYWNlOiBub3dyYXA7XHJcbiAgICAgICAgICAgIG92ZXJmbG93OiBoaWRkZW47XHJcbiAgICAgICAgICAgIHRleHQtb3ZlcmZsb3c6IGVsbGlwc2lzO1xyXG4gICAgICAgICAgICB0ZXh0LXRyYW5zZm9ybTogdXBwZXJjYXNlO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLmNvbXBhYmlsaXR5LXR5cGUge1xyXG4gICAgICAgICAgICBmb250LXdlaWdodDogNjAwO1xyXG4gICAgICAgICAgICBmb250LXNpemU6IDExcHg7XHJcbiAgICAgICAgICAgIGxpbmUtaGVpZ2h0OiAxM3B4O1xyXG4gICAgICAgICAgICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xyXG4gICAgICAgICAgICBjb2xvcjogIzEyMTIxRjtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG5cclxuICAgIC5kdXJhdGlvbiB7XHJcbiAgICAgICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgICAgIGJvdHRvbTogMnB4O1xyXG4gICAgICAgIHJpZ2h0OiAxMHB4O1xyXG4gICAgICAgIGZvbnQtd2VpZ2h0OiA2MDA7XHJcbiAgICAgICAgZm9udC1zaXplOiAxMXB4O1xyXG4gICAgICAgIGxpbmUtaGVpZ2h0OiAxM3B4O1xyXG4gICAgICAgIGxldHRlci1zcGFjaW5nOiAwLjI1cHg7XHJcbiAgICAgICAgY29sb3I6ICNEMzRFNEU7XHJcbiAgICB9XHJcblxyXG4gICAgLmNsaWNrLWljb24ge1xyXG4gICAgICAgIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICAgICAgICB0b3A6IDA7XHJcbiAgICAgICAgcmlnaHQ6IDEwcHg7XHJcbiAgICB9XHJcblxyXG4gICAgJi5tdXN0LWdvIHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjRkRENkQ2O1xyXG4gICAgfVxyXG5cclxuICAgICYuc2hvdWxkLWdvIHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjRkZEREI1O1xyXG4gICAgfVxyXG5cclxuICAgICYuY291bGQtZ28ge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNEQ0RDREM7XHJcbiAgICB9XHJcbn1cclxuXHJcbi5kci1wb3BvdmVyIHtcclxuICAgICYucG9wb3ZlclxyXG5cclxue1xyXG4gICAgbWluLXdpZHRoOiAzMDBweDtcclxuICAgIG1heC13aWR0aDogMzUwcHg7XHJcbiAgICBiYWNrZ3JvdW5kOiAjRjlGOUY5O1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgI0U5RTdFNztcclxuICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICBib3gtc2hhZG93OiAxMHB4IDEwcHggOHB4IC0ycHggcmdiKDAsIDAsIDAsIDAuMTMpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogMTBweDtcclxuICAgIC5wb3BvdmVyLWJvZHlcclxuXHJcbntcclxuICAgIG1heC1oZWlnaHQ6IDM1MHB4O1xyXG4gICAgb3ZlcmZsb3cteTogYXV0bztcclxuICAgIG92ZXJmbG93LXg6IGhpZGRlbjtcclxuICAgIHBhZGRpbmc6IDA7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuXHJcbi5wb3BvdmVyLWRldGFpbHMge1xyXG4gICAgcGFkZGluZzogM3B4IDIwcHg7XHJcbiAgICBtYXgtaGVpZ2h0OiAzMTBweDtcclxuICAgIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICAucHJvZHVjdC1kZXRhaWxzXHJcblxyXG57XHJcbiAgICBwYWRkaW5nOiAycHggMTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDEwcHg7XHJcbiAgICBtYXJnaW4tYm90dG9tOiA1cHg7XHJcbiAgICAucHJvZHVjdC1uYW1lXHJcblxyXG57XHJcbiAgICBmb250LXdlaWdodDogNjAwO1xyXG4gICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgbGluZS1oZWlnaHQ6IDE3cHg7XHJcbiAgICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xyXG4gICAgY29sb3I6ICMxMjEyMUY7XHJcbn1cclxuXHJcbi5wcm9kdWN0LXF0eSB7XHJcbiAgICBmb250LXdlaWdodDogNjAwO1xyXG4gICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgbGluZS1oZWlnaHQ6IDE3cHg7XHJcbiAgICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xyXG4gICAgY29sb3I6ICNCQjQxNDE7XHJcbiAgICBmbG9hdDogcmlnaHQ7XHJcbiAgICB0ZXh0LWFsaWduOiByaWdodDtcclxufVxyXG5cclxuLmljb24tdHJheSB7XHJcbiAgICB0ZXh0LWFsaWduOiBjZW50ZXI7XHJcbiAgICBkaXNwbGF5OiBmbGV4O1xyXG4gICAgcGxhY2UtY29udGVudDogY2VudGVyO1xyXG4gICAgdHJhbnNpdGlvbjogYWxsIDAuM3MgZWFzZS1vdXQ7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gICAgaGVpZ2h0OiAwO1xyXG4gICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgIC5jaXJjbGUtaWNvblxyXG5cclxue1xyXG4gICAgYmFja2dyb3VuZDogI2ZmZjtcclxuICAgIGJvcmRlcjogMHB4IHNvbGlkICM3OTc5Nzk7XHJcbiAgICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgbWFyZ2luLXJpZ2h0OiA1cHg7XHJcbiAgICB3aWR0aDogMjVweDtcclxuICAgIGhlaWdodDogMjVweDtcclxuICAgIGRpc3BsYXk6IGZsZXg7XHJcbiAgICBwbGFjZS1jb250ZW50OiBjZW50ZXI7XHJcbiAgICBhbGlnbi1pdGVtczogY2VudGVyO1xyXG4gICAgYVxyXG5cclxue1xyXG4gICAgY29sb3I6ICM2RjZFNkUgIWltcG9ydGFudDtcclxufVxyXG5cclxuaSB7XHJcbiAgICBmb250LXNpemU6IDE0cHg7XHJcbn1cclxuXHJcbn1cclxufVxyXG5cclxuJjpob3ZlciB7XHJcbiAgICBib3gtc2hhZG93OiAwIDAgM3B4ICM1MTUxNTE7XHJcbiAgICBib3JkZXI6IDA7XHJcbiAgICB0cmFuc2l0aW9uOiBhbGwgLjZzIGVhc2Utb3V0O1xyXG4gICAgLmljb24tdHJheVxyXG5cclxue1xyXG4gICAgcGFkZGluZzogM3B4O1xyXG4gICAgdHJhbnNpdGlvbjogYWxsIC42cyBlYXNlLW91dDtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgICBoZWlnaHQ6IGF1dG87XHJcbn1cclxuXHJcbn1cclxufVxyXG5cclxuLm11c3QtZ28ge1xyXG4gICAgYmFja2dyb3VuZDogI0ZERDZENjtcclxuICAgIC5wcm9kdWN0LXF0eXtcclxuICAgIGNvbG9yOiAjQkI0MTQxO1xyXG59XHJcblxyXG59XHJcblxyXG4uc2hvdWxkLWdvIHtcclxuICAgIGJhY2tncm91bmQ6ICNGRkREQjU7XHJcbiAgICAucHJvZHVjdC1xdHlcclxuXHJcbntcclxuICAgIGNvbG9yOiAjRTg5MzMwO1xyXG59XHJcblxyXG59XHJcblxyXG4uY291bGQtZ28ge1xyXG4gICAgYmFja2dyb3VuZDogI0RDRENEQztcclxuICAgIC5wcm9kdWN0LXF0eVxyXG5cclxue1xyXG4gICAgY29sb3I6ICM2OTY5Njk7XHJcbn1cclxuXHJcbn1cclxuXHJcbi5pbi1wcm9ncmVzcyB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjM0Q3MUI4O1xyXG4gICAgLnByb2R1Y3QtbmFtZVxyXG5cclxue1xyXG4gICAgY29sb3I6ICNmZmZmZmY7XHJcbn1cclxuXHJcbi5wcm9kdWN0LXF0eSB7XHJcbiAgICBjb2xvcjogI2ZmZmZmZjtcclxufVxyXG5cclxufVxyXG59XHJcbn1cclxufVxyXG4iLCIuaWNvbl9tZW51IHtcbiAgZGlzcGxheTogbm9uZTtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB0b3A6IDA7XG4gIHJpZ2h0OiA1cHg7XG4gIC8qYmFja2dyb3VuZDogI2ZmZmZmZjsqL1xuICBiYWNrZ3JvdW5kOiAjN2U3YjdiO1xuICBwYWRkaW5nOiA1cHggNXB4O1xuICBib3gtc2hhZG93OiAwIDJweCA0cHggcmdiYSgwLCAwLCAwLCAwLjA4KTtcbiAgYm9yZGVyLXJhZGl1czogM3B4O1xufVxuXG4uZHJfY2FyZHNfbmV3OmhvdmVyIC5pY29uX21lbnUge1xuICBkaXNwbGF5OiBibG9jaztcbn1cblxuLnJvdXRlX3RleHQge1xuICBjb2xvcjogI2M5NWU2MTtcbiAgZm9udC1zaXplOiAxMHB4O1xuICBmb250LXdlaWdodDogYm9sZDtcbn1cblxuLmRyLXF1ZXVlIHtcbiAgcGFkZGluZzogNXB4O1xuICAvKmJhY2tncm91bmQ6ICNGREQ2RDY7Ki9cbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbiAgcG9zaXRpb246IHJlbGF0aXZlO1xuICBtYXJnaW4tYm90dG9tOiAycHg7XG59XG4uZHItcXVldWUgLmRyLWluZm8ge1xuICBkaXNwbGF5OiBmbGV4O1xuICBhbGlnbi1pdGVtczogYmFzZWxpbmU7XG59XG4uZHItcXVldWUgLmRyLWluZm8gLmJyYW5kLW5hbWUge1xuICBmb250LXdlaWdodDogNjAwO1xuICBmb250LXNpemU6IDE0cHg7XG4gIGxpbmUtaGVpZ2h0OiAxN3B4O1xuICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xuICBjb2xvcjogIzEyMTIxRjtcbiAgdGV4dC10cmFuc2Zvcm06IHVwcGVyY2FzZTtcbn1cbi5kci1xdWV1ZSAuZHItaW5mbyAuam9iLWxvY2F0aW9uIHtcbiAgZm9udC13ZWlnaHQ6IDYwMDtcbiAgZm9udC1zaXplOiAxM3B4O1xuICBsaW5lLWhlaWdodDogMTZweDtcbiAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcbiAgY29sb3I6ICMxMjEyMUY7XG4gIHRleHQtdHJhbnNmb3JtOiBjYXBpdGFsaXplO1xuICBtYXgtd2lkdGg6IDQwJTtcbiAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcbiAgb3ZlcmZsb3c6IGhpZGRlbjtcbiAgdGV4dC1vdmVyZmxvdzogZWxsaXBzaXM7XG59XG4uZHItcXVldWUgLmRyLWluZm8gLmpvYi1jaXR5IHtcbiAgZm9udC13ZWlnaHQ6IDYwMDtcbiAgZm9udC1zaXplOiAxMXB4O1xuICBsaW5lLWhlaWdodDogMTNweDtcbiAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcbiAgY29sb3I6ICMxMjEyMUY7XG4gIG1heC13aWR0aDogMjUlO1xuICB3aGl0ZS1zcGFjZTogbm93cmFwO1xuICBvdmVyZmxvdzogaGlkZGVuO1xuICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcbiAgdGV4dC10cmFuc2Zvcm06IHVwcGVyY2FzZTtcbn1cbi5kci1xdWV1ZSAuZHItaW5mbyAuY29tcGFiaWxpdHktdHlwZSB7XG4gIGZvbnQtd2VpZ2h0OiA2MDA7XG4gIGZvbnQtc2l6ZTogMTFweDtcbiAgbGluZS1oZWlnaHQ6IDEzcHg7XG4gIGxldHRlci1zcGFjaW5nOiAwLjI1cHg7XG4gIGNvbG9yOiAjMTIxMjFGO1xufVxuLmRyLXF1ZXVlIC5kdXJhdGlvbiB7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgYm90dG9tOiAycHg7XG4gIHJpZ2h0OiAxMHB4O1xuICBmb250LXdlaWdodDogNjAwO1xuICBmb250LXNpemU6IDExcHg7XG4gIGxpbmUtaGVpZ2h0OiAxM3B4O1xuICBsZXR0ZXItc3BhY2luZzogMC4yNXB4O1xuICBjb2xvcjogI0QzNEU0RTtcbn1cbi5kci1xdWV1ZSAuY2xpY2staWNvbiB7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAwO1xuICByaWdodDogMTBweDtcbn1cbi5kci1xdWV1ZS5tdXN0LWdvIHtcbiAgYmFja2dyb3VuZDogI0ZERDZENjtcbn1cbi5kci1xdWV1ZS5zaG91bGQtZ28ge1xuICBiYWNrZ3JvdW5kOiAjRkZEREI1O1xufVxuLmRyLXF1ZXVlLmNvdWxkLWdvIHtcbiAgYmFja2dyb3VuZDogI0RDRENEQztcbn1cblxuLmRyLXBvcG92ZXIucG9wb3ZlciB7XG4gIG1pbi13aWR0aDogMzAwcHg7XG4gIG1heC13aWR0aDogMzUwcHg7XG4gIGJhY2tncm91bmQ6ICNGOUY5Rjk7XG4gIGJvcmRlcjogMXB4IHNvbGlkICNFOUU3RTc7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJveC1zaGFkb3c6IDEwcHggMTBweCA4cHggLTJweCByZ2JhKDAsIDAsIDAsIDAuMTMpO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1ib2R5IHtcbiAgbWF4LWhlaWdodDogMzUwcHg7XG4gIG92ZXJmbG93LXk6IGF1dG87XG4gIG92ZXJmbG93LXg6IGhpZGRlbjtcbiAgcGFkZGluZzogMDtcbiAgYm9yZGVyLXJhZGl1czogNXB4O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIHtcbiAgcGFkZGluZzogM3B4IDIwcHg7XG4gIG1heC1oZWlnaHQ6IDMxMHB4O1xuICBvdmVyZmxvdy15OiBhdXRvO1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5wcm9kdWN0LWRldGFpbHMge1xuICBwYWRkaW5nOiAycHggMTBweDtcbiAgYm9yZGVyLXJhZGl1czogMTBweDtcbiAgbWFyZ2luLWJvdHRvbTogNXB4O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5wcm9kdWN0LWRldGFpbHMgLnByb2R1Y3QtbmFtZSB7XG4gIGZvbnQtd2VpZ2h0OiA2MDA7XG4gIGZvbnQtc2l6ZTogMTRweDtcbiAgbGluZS1oZWlnaHQ6IDE3cHg7XG4gIGxldHRlci1zcGFjaW5nOiAwLjI1cHg7XG4gIGNvbG9yOiAjMTIxMjFGO1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5wcm9kdWN0LWRldGFpbHMgLnByb2R1Y3QtcXR5IHtcbiAgZm9udC13ZWlnaHQ6IDYwMDtcbiAgZm9udC1zaXplOiAxNHB4O1xuICBsaW5lLWhlaWdodDogMTdweDtcbiAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcbiAgY29sb3I6ICNCQjQxNDE7XG4gIGZsb2F0OiByaWdodDtcbiAgdGV4dC1hbGlnbjogcmlnaHQ7XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLnByb2R1Y3QtZGV0YWlscyAuaWNvbi10cmF5IHtcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xuICBkaXNwbGF5OiBmbGV4O1xuICBwbGFjZS1jb250ZW50OiBjZW50ZXI7XG4gIHRyYW5zaXRpb246IGFsbCAwLjNzIGVhc2Utb3V0O1xuICBvcGFjaXR5OiAwO1xuICBoZWlnaHQ6IDA7XG4gIG92ZXJmbG93OiBoaWRkZW47XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLnByb2R1Y3QtZGV0YWlscyAuaWNvbi10cmF5IC5jaXJjbGUtaWNvbiB7XG4gIGJhY2tncm91bmQ6ICNmZmY7XG4gIGJvcmRlcjogMHB4IHNvbGlkICM3OTc5Nzk7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJvcmRlci1yYWRpdXM6IDUwJTtcbiAgbWFyZ2luLXJpZ2h0OiA1cHg7XG4gIHdpZHRoOiAyNXB4O1xuICBoZWlnaHQ6IDI1cHg7XG4gIGRpc3BsYXk6IGZsZXg7XG4gIHBsYWNlLWNvbnRlbnQ6IGNlbnRlcjtcbiAgYWxpZ24taXRlbXM6IGNlbnRlcjtcbn1cbi5kci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAucHJvZHVjdC1kZXRhaWxzIC5pY29uLXRyYXkgLmNpcmNsZS1pY29uIGEge1xuICBjb2xvcjogIzZGNkU2RSAhaW1wb3J0YW50O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5wcm9kdWN0LWRldGFpbHMgLmljb24tdHJheSAuY2lyY2xlLWljb24gaSB7XG4gIGZvbnQtc2l6ZTogMTRweDtcbn1cbi5kci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAucHJvZHVjdC1kZXRhaWxzOmhvdmVyIHtcbiAgYm94LXNoYWRvdzogMCAwIDNweCAjNTE1MTUxO1xuICBib3JkZXI6IDA7XG4gIHRyYW5zaXRpb246IGFsbCAwLjZzIGVhc2Utb3V0O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5wcm9kdWN0LWRldGFpbHM6aG92ZXIgLmljb24tdHJheSB7XG4gIHBhZGRpbmc6IDNweDtcbiAgdHJhbnNpdGlvbjogYWxsIDAuNnMgZWFzZS1vdXQ7XG4gIG9wYWNpdHk6IDE7XG4gIGhlaWdodDogYXV0bztcbn1cbi5kci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAubXVzdC1nbyB7XG4gIGJhY2tncm91bmQ6ICNGREQ2RDY7XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLm11c3QtZ28gLnByb2R1Y3QtcXR5IHtcbiAgY29sb3I6ICNCQjQxNDE7XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLnNob3VsZC1nbyB7XG4gIGJhY2tncm91bmQ6ICNGRkREQjU7XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLnNob3VsZC1nbyAucHJvZHVjdC1xdHkge1xuICBjb2xvcjogI0U4OTMzMDtcbn1cbi5kci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAuY291bGQtZ28ge1xuICBiYWNrZ3JvdW5kOiAjRENEQ0RDO1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5jb3VsZC1nbyAucHJvZHVjdC1xdHkge1xuICBjb2xvcjogIzY5Njk2OTtcbn1cbi5kci1wb3BvdmVyLnBvcG92ZXIgLnBvcG92ZXItZGV0YWlscyAuaW4tcHJvZ3Jlc3Mge1xuICBiYWNrZ3JvdW5kOiAjM0Q3MUI4O1xufVxuLmRyLXBvcG92ZXIucG9wb3ZlciAucG9wb3Zlci1kZXRhaWxzIC5pbi1wcm9ncmVzcyAucHJvZHVjdC1uYW1lIHtcbiAgY29sb3I6ICNmZmZmZmY7XG59XG4uZHItcG9wb3Zlci5wb3BvdmVyIC5wb3BvdmVyLWRldGFpbHMgLmluLXByb2dyZXNzIC5wcm9kdWN0LXF0eSB7XG4gIGNvbG9yOiAjZmZmZmZmO1xufSJdfQ== */"], encapsulation: 2 });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryRequestDisplayComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-delivery-request-display',
                templateUrl: './delivery-request-display.component.html',
                styleUrls: ['./delivery-request-display.component.scss'],
                encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
            }]
    }], function () { return [{ type: _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"] }, { type: _carrier_service_schedule_builder_service__WEBPACK_IMPORTED_MODULE_6__["ScheduleBuilderService"] }, { type: _carrier_service_deliveryrequest_service__WEBPACK_IMPORTED_MODULE_7__["DeliveryrequestService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormBuilder"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_9__["ActivatedRoute"] }]; }, null); })();


/***/ }),

/***/ "./src/app/delivery-request-display/delivery-request-display.module.ts":
/*!*****************************************************************************!*\
  !*** ./src/app/delivery-request-display/delivery-request-display.module.ts ***!
  \*****************************************************************************/
/*! exports provided: DeliveryRequestDisplayModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryRequestDisplayModule", function() { return DeliveryRequestDisplayModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ng-drag-drop */ "./node_modules/ng-drag-drop/__ivy_ngcc__/index.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(ng_drag_drop__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _delivery_request_display_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./delivery-request-display.component */ "./src/app/delivery-request-display/delivery-request-display.component.ts");
/* harmony import */ var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");










const routesDrDisplay = [
    {
        path: "",
        component: _delivery_request_display_component__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestDisplayComponent"]
    },
];
class DeliveryRequestDisplayModule {
}
DeliveryRequestDisplayModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: DeliveryRequestDisplayModule });
DeliveryRequestDisplayModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function DeliveryRequestDisplayModule_Factory(t) { return new (t || DeliveryRequestDisplayModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"],
            src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"],
            ng_drag_drop__WEBPACK_IMPORTED_MODULE_2__["NgDragDropModule"].forRoot(),
            _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"].forChild(routesDrDisplay)
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DeliveryRequestDisplayModule, { declarations: [_delivery_request_display_component__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestDisplayComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"],
        src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_2__["NgDragDropModule"], _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryRequestDisplayModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [
                    _delivery_request_display_component__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestDisplayComponent"],
                ],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"],
                    src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"],
                    ng_drag_drop__WEBPACK_IMPORTED_MODULE_2__["NgDragDropModule"].forRoot(),
                    _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"].forChild(routesDrDisplay)
                ]
            }]
    }], null, null); })();


/***/ })

}]);
//# sourceMappingURL=delivery-request-display-delivery-request-display-module-es2015.js.map