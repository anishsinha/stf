(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["order-lazy-loading-order-module"],{

/***/ "./src/app/order/create-blend-group/create-blend-group.component.ts":
/*!**************************************************************************!*\
  !*** ./src/app/order/create-blend-group/create-blend-group.component.ts ***!
  \**************************************************************************/
/*! exports provided: CreateBlendGroupComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateBlendGroupComponent", function() { return CreateBlendGroupComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _services_order_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../services/order.service */ "./src/app/order/services/order.service.ts");
/* harmony import */ var _services_shared_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../services/shared.service */ "./src/app/order/services/shared.service.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ng-drag-drop */ "./node_modules/ng-drag-drop/__ivy_ngcc__/index.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(ng_drag_drop__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");











function CreateBlendGroupComponent_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Please drag order here to blend fuel types");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_div_20_div_17_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_div_20_div_17_span_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_div_20_div_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateBlendGroupComponent_div_20_div_17_span_1_Template, 2, 0, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateBlendGroupComponent_div_20_div_17_span_2_Template, 2, 0, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const group_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", group_r6.get("BlendPercentage").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", group_r6.get("BlendPercentage").errors.pattern);
} }
function CreateBlendGroupComponent_div_20_div_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_div_20_Template(rf, ctx) { if (rf & 1) {
    const _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "h3", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](14, "input", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "span", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "%");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, CreateBlendGroupComponent_div_20_div_17_Template, 3, 2, "div", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "a", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateBlendGroupComponent_div_20_Template_a_click_19_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r14); const i_r7 = ctx.index; const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r13.removeBlend(i_r7); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](20, "i", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, CreateBlendGroupComponent_div_20_div_21_Template, 2, 0, "div", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const group_r6 = ctx.$implicit;
    const i_r7 = ctx.index;
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](group_r6.get("TfxPoNumber").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", group_r6.get("OrderId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](group_r6.get("FuelType").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", group_r6.get("BlendPercentage").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", group_r6.get("BlendPercentage").errors && (group_r6.get("BlendPercentage").touched || group_r6.get("BlendPercentage").dirty));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", i_r7 + 1 < ctx_r1.BlendGroupForm.get("OrderBlendedGroups")["controls"].length);
} }
function CreateBlendGroupComponent_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const order_r15 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dragData", order_r15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r15.TfxPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r15.FuelType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r15.DisplayPrice);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r15.Quantity);
} }
function CreateBlendGroupComponent_input_33_Template(rf, ctx) { if (rf & 1) {
    const _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateBlendGroupComponent_input_33_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17); const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r16.OnSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_input_34_Template(rf, ctx) { if (rf & 1) {
    const _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateBlendGroupComponent_input_34_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r18.OnSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateBlendGroupComponent_div_35_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Submitting Group...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
class CreateBlendGroupComponent {
    constructor(fb, orderService, viewGroupService) {
        this.fb = fb;
        this.orderService = orderService;
        this.viewGroupService = viewGroupService;
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.CustomerList = [];
        this.JobList = [];
        this.IsLoading = false;
        this.configSettings = {};
        this.FuelDdlSettings = {};
        this.SingleDdlSettings = {};
        this.orderList = [];
        this.finalSubmitData = {};
        this.groupId = 0;
        this.isEdit = false;
        this.onSubmitGroupData = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.isDropAllowed = (dragData) => {
            return dragData > 500;
        };
        this.BlendGroupForm = this.fb.group({
            OrderBlendedGroups: this.fb.array([]),
            CustomerPoNumber: this.fb.control('')
        });
    }
    ngOnInit() {
        this.configSettings = {
            displayKey: 'Name',
            search: true,
        };
        this.FuelDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            closeDropDownOnSelection: true
        };
        this.SingleDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        if (IsSupplierCompany) {
            this.orderService.getCustomerList().subscribe(data => this.CustomerList = data);
        }
        else {
            this.orderService.getSupplierList().subscribe(data => this.CustomerList = data);
        }
        this.viewGroupService.currentGroup.subscribe(id => {
            this.groupId = id;
            if (id > 0) {
                this.clearBlendGroupForm();
                this.GetGroupDetails();
                this.isEdit = true;
            }
        });
    }
    ngAfterViewInit() {
    }
    get OrderBlendedGroups() {
        return this.BlendGroupForm.get("OrderBlendedGroups");
    }
    get CustomerPoNumber() {
        return this.BlendGroupForm.get("CustomerPoNumber");
    }
    get Job() {
        return this.SelectedJob[0];
    }
    get Customer() {
        return this.SelectedCustomer[0];
    }
    get selectedFuelTypeList() {
        return this.SelectedFuelTypes.map(x => x.Id);
    }
    buildProduct(model) {
        return this.fb.group({
            GroupId: this.fb.control(model.GroupId),
            OrderId: this.fb.control(model.OrderId),
            TfxPoNumber: this.fb.control(model.TfxPoNumber),
            FuelType: this.fb.control(model.FuelType),
            Quantity: this.fb.control(model.Quantity),
            DisplayPrice: this.fb.control(model.DisplayPrice),
            BlendPercentage: this.fb.control(model.BlendPercentage, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^[0-9]\d*(\.\d+)?$/)]),
        });
    }
    onItemDrop(order) {
        //this.usedOrderGroupDetails.push(order.dragData);
        //this.OrderBlendedGroups.patchValue(this.usedOrderGroupDetails);
        this.setSelectedOrders(order.dragData);
        this.SetDefaultBlendPercentage();
    }
    setSelectedOrders(order) {
        this.OrderBlendedGroups.push(this.buildProduct(order));
        this.orderList = this.orderList.filter(x => x.FuelType !== order.FuelType);
    }
    onFuelSelect(item) {
        this.loadOrders(this.Customer.Id, this.Job.Id, this.selectedFuelTypeList);
    }
    onFuelDeSelect(item) {
        this.RemoveOrder(item.Name);
        this.RemoveGroups(item.Name);
    }
    removeBlend(i) {
        this.OrderBlendedGroups.removeAt(i);
        var selectedFuelTypes = this.OrderBlendedGroups.controls.map(x => { return x.get('FuelType').value; });
        var fuelTypeIds = this.FuelTypeList.map(t => {
            if (selectedFuelTypes.indexOf(t.Name) > -1)
                return t.Id;
        });
        var loadedFuelType = this.selectedFuelTypeList.filter(x => fuelTypeIds.indexOf(x) == -1);
        this.SetDefaultBlendPercentage();
        this.loadOrders(this.Customer.Id, this.Job.Id, loadedFuelType);
    }
    RemoveOrder(fuelTypeName) {
        this.orderList = this.orderList.filter(obj => obj.FuelType !== fuelTypeName);
    }
    loadOrders(_customerId, _jobId, _fuelTypeIds) {
        this.orderService.getFilteredOrdersList(_customerId, _jobId, _fuelTypeIds, this.groupId).subscribe((data) => {
            var existingOrderList = this.OrderBlendedGroups.controls.map(x => { return x.get('OrderId').value; });
            this.orderList = data.filter(ele => existingOrderList.indexOf(ele.OrderId) == -1);
        });
    }
    loadFuelType(_customerId, _jobId) {
        this.orderService.getFuelTypesList(_customerId, _jobId).subscribe((data) => {
            this.FuelTypeList = data;
        });
    }
    RemoveGroups(_FuelType) {
        //this.usedOrderGroupDetails = this.usedOrderGroupDetails.filter(x => x.FuelType !== _FuelType);
        this.OrderBlendedGroups.setValue(this.OrderBlendedGroups.controls.filter(group => group.get('FuelType').value !== _FuelType));
        this.SetDefaultBlendPercentage();
    }
    SetDefaultBlendPercentage() {
        var totalFuelTypes = this.OrderBlendedGroups.length;
        var equalPercent = (100 / totalFuelTypes).toFixed(2);
        this.OrderBlendedGroups.controls.forEach((x) => { x.get('BlendPercentage').setValue(equalPercent); });
    }
    OnCustomerSelect(customer) {
        this.SelectedJob = '';
        this.SelectedFuelTypes = [];
        this.OrderBlendedGroups.clear();
        this.orderService.getCommonJobList(customer.Id).subscribe(data => this.JobList = data);
    }
    OnCustomerDeSelect(customer) {
        this.SelectedCustomer = '';
        this.JobList = [];
        this.SelectedJob = '';
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.OrderBlendedGroups.clear();
    }
    OnJobSelect(job) {
        this.orderList = [];
        this.OrderBlendedGroups.clear();
        this.SelectedFuelTypes = [];
        this.loadFuelType(this.Customer.Id, job.Id);
    }
    OnJobDeSelect(job) {
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.OrderBlendedGroups.clear();
    }
    OnSubmit() {
        var blendValue = this.OrderBlendedGroups.controls.map(x => { return parseFloat(x.get('BlendPercentage').value); });
        var sum = 0;
        blendValue.forEach(x => sum += x);
        if (sum != 100) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror('Total percentage sum should be 100', undefined, undefined);
            this.BlendGroupForm.setErrors({ 'invalid': true });
        }
        this.finalSubmitData = {
            Id: this.isEdit ? this.groupId : 0,
            OrderList: this.OrderBlendedGroups.value,
            GroupPoNumber: this.CustomerPoNumber.value,
            BuyerCompanyId: IsSupplierCompany ? this.Customer.Id : currentUserCompanyId,
            SupplierCompanyId: IsSupplierCompany ? currentUserCompanyId : this.Customer.Id,
            JobId: this.Job.Id,
            GroupType: src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["OrderGroupType"].Blend
        };
        if (this.BlendGroupForm.valid) {
            this.IsLoading = true;
            if (this.isEdit) {
                this.orderService.postEditGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                        this.clearBlendGroupForm();
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
            else {
                this.orderService.postCreateGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
    }
    GetGroupDetails() {
        this.orderService.getBlendGroupDetails(this.groupId).subscribe(group => {
            this.JobList = group.Jobs;
            this.FuelTypeList = group.FuelTypes;
            this.orderList = group.FilteredOrders;
            this.CustomerPoNumber.setValue(group.GroupDetails.GroupPoNumber);
            this.SelectedCustomer = IsSupplierCompany ? this.getCustomerFromSupplier(group.GroupDetails.BuyerCompanyId) : this.getCustomerFromSupplier(group.GroupDetails.SupplierCompanyId);
            this.SelectedJob = this.JobList.filter(ele => ele.Id == group.GroupDetails.JobId);
            var _fuelTypes = group.GroupDetails.OrderList.map(x => x.Order.FuelType);
            this.SelectedFuelTypes = this.FuelTypeList.filter(ele => _fuelTypes.indexOf(ele.Name) > -1);
            for (var i = 0; i < group.GroupDetails.OrderList.length; i++) {
                var orderDetail = group.GroupDetails.OrderList[i];
                var orderToMove = this.orderList.filter(t => t.OrderId == orderDetail.OrderId);
                this.setSelectedOrders(orderToMove[0]);
            }
        });
    }
    getCustomerFromSupplier(id) {
        return this.CustomerList.filter(e => e.Id == id);
    }
    clearBlendGroupForm() {
        if (this.SelectedCustomer != null) {
            this.OnCustomerDeSelect(this.SelectedCustomer);
        }
        this.CustomerPoNumber.setValue("");
        this.isEdit = false;
    }
}
CreateBlendGroupComponent.ɵfac = function CreateBlendGroupComponent_Factory(t) { return new (t || CreateBlendGroupComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_shared_service__WEBPACK_IMPORTED_MODULE_5__["SharedService"])); };
CreateBlendGroupComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: CreateBlendGroupComponent, selectors: [["app-create-blend-group"]], outputs: { onSubmitGroupData: "onSubmitGroupData" }, decls: 36, vars: 16, consts: [[1, "row"], [1, "form-group", "col-sm-3"], [3, "data", "ngModel", "settings", "ngModelChange", "onSelect", "onDeSelect"], [1, "form-group", "col-sm-4"], [3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], [3, "formGroup"], [1, "col-sm-3"], [1, "group-height"], ["droppable", "", 1, "clearboth", "border-dash-dark", "radius-5", 3, "onDrop"], ["class", "pa10 ma15 text-center", 4, "ngIf"], ["formArrayName", "OrderBlendedGroups"], ["class", "radius-5 pl10 pr10 ma15", 4, "ngFor", "ngForOf"], [1, "col-sm-6"], ["class", "col-sm-6", "draggable", "", 3, "dragData", 4, "ngFor", "ngForOf"], [1, "col-md-3"], [1, "form-group"], ["for", "CustomerPoNumber"], ["type", "text", "formControlName", "CustomerPoNumber", 1, "form-control"], [1, "col-sm-12", "text-right"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", "btn-lg", 3, "click"], ["type", "button", "class", "btn btn-primary btn-lg", "value", "Create", 3, "click", 4, "ngIf"], ["type", "button", "class", "btn btn-primary btn-lg", "value", "Submit", 3, "click", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [1, "pa10", "ma15", "text-center"], [1, "far", "fa-hand-rock", "fs25"], [1, "fs16", "ma5", "pa0", "color-orange"], [1, "radius-5", "pl10", "pr10", "ma15"], [1, "row", 3, "formGroupName"], [1, "well", "mb0", "col-sm-12"], [1, "col-sm-12"], [1, "mt0", "mb0"], ["type", "hidden", "formControlName", "OrderId", 3, "value"], [1, "row", "mt10"], [1, "col-sm-4", "pt10"], [1, "input-group"], ["type", "text", "formControlName", "BlendPercentage", 1, "form-control", 3, "value"], [1, "input-group-addon"], ["class", "help-block color-maroon", 4, "ngIf"], [1, "col-sm-2", "pt10"], [1, "color-maroon", "pull-right", 3, "click"], [1, "fa", "fa-trash-alt"], ["class", "text-center col-sm-12", 4, "ngIf"], [1, "help-block", "color-maroon"], [4, "ngIf"], [1, "text-center", "col-sm-12"], [1, "fas", "fa-plus", "fs21", "mt15"], ["draggable", "", 1, "col-sm-6", 3, "dragData"], [1, "well"], ["type", "button", "value", "Create", 1, "btn", "btn-primary", "btn-lg", 3, "click"], ["type", "button", "value", "Submit", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateBlendGroupComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Customer");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { return ctx.SelectedCustomer = $event; })("onSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { return ctx.OnCustomerSelect($event); })("onDeSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { return ctx.OnCustomerDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Job");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "ng-multiselect-dropdown", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_8_listener($event) { return ctx.SelectedJob = $event; })("onSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onSelect_8_listener($event) { return ctx.OnJobSelect($event); })("onDeSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_8_listener($event) { return ctx.OnJobDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Fuel Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_12_listener($event) { return ctx.SelectedFuelTypes = $event; })("onSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onSelect_12_listener($event) { return ctx.onFuelSelect($event); })("onDeSelect", function CreateBlendGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_12_listener($event) { return ctx.onFuelDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDrop", function CreateBlendGroupComponent_Template_div_onDrop_17_listener($event) { return ctx.onItemDrop($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, CreateBlendGroupComponent_div_18_Template, 4, 0, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, CreateBlendGroupComponent_div_20_Template, 22, 7, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, CreateBlendGroupComponent_div_24_Template, 13, 5, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "Customer Po#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](29, "input", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "input", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateBlendGroupComponent_Template_input_click_32_listener() { return ctx.clearBlendGroupForm(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](33, CreateBlendGroupComponent_input_33_Template, 1, 0, "input", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](34, CreateBlendGroupComponent_input_34_Template, 1, 0, "input", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](35, CreateBlendGroupComponent_div_35_Template, 5, 0, "div", 22);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("data", ctx.CustomerList)("ngModel", ctx.SelectedCustomer)("settings", ctx.SingleDdlSettings);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("data", ctx.JobList)("ngModel", ctx.SelectedJob)("settings", ctx.SingleDdlSettings);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedFuelTypes)("settings", ctx.FuelDdlSettings)("data", ctx.FuelTypeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.BlendGroupForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.BlendGroupForm.get("OrderBlendedGroups")["controls"].length == 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.BlendGroupForm.get("OrderBlendedGroups")["controls"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.orderList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isEdit);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isEdit);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_7__["Droppable"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_7__["Draggable"]], styles: [".drag-border[_ngcontent-%COMP%] {\r\n    border: #ff525b dashed 2px;\r\n}\r\n\r\n.drag-handle[_ngcontent-%COMP%] {\r\n    cursor: move; \r\n    cursor: grab;\r\n    cursor: -webkit-grab;\r\n}\r\n\r\n.drag-handle[_ngcontent-%COMP%]:active {\r\n        cursor: grabbing;\r\n        cursor: -webkit-grabbing;\r\n    }\r\n\r\n\r\n\r\n.drag-hint-border[_ngcontent-%COMP%] {\r\n    border: #3c763d dashed 2px;\r\n}\r\n\r\n.drag-over-border[_ngcontent-%COMP%] {\r\n    border: #fbbc05 dashed 2px;\r\n}\r\n\r\n.drag-transit[_ngcontent-%COMP%] {\r\n    border: #3500FF dashed 2px;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvb3JkZXIvY3JlYXRlLWJsZW5kLWdyb3VwL2NyZWF0ZS1ibGVuZC1ncm91cC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7QUFDQSxhQUFhOztBQUViO0lBQ0ksMEJBQTBCO0FBQzlCOztBQUVBO0lBQ0ksWUFBWSxFQUFFLDJDQUEyQztJQUN6RCxZQUFZO0lBRVosb0JBQW9CO0FBQ3hCOztBQUVJO1FBQ0ksZ0JBQWdCO1FBRWhCLHdCQUF3QjtJQUM1Qjs7QUFFSixjQUFjOztBQUVkO0lBQ0ksMEJBQTBCO0FBQzlCOztBQUVBO0lBQ0ksMEJBQTBCO0FBQzlCOztBQUVBO0lBQ0ksMEJBQTBCO0FBQzlCIiwiZmlsZSI6InNyYy9hcHAvb3JkZXIvY3JlYXRlLWJsZW5kLWdyb3VwL2NyZWF0ZS1ibGVuZC1ncm91cC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiXHJcbi8qIERyYWdnYWJsZSovXHJcblxyXG4uZHJhZy1ib3JkZXIge1xyXG4gICAgYm9yZGVyOiAjZmY1MjViIGRhc2hlZCAycHg7XHJcbn1cclxuXHJcbi5kcmFnLWhhbmRsZSB7XHJcbiAgICBjdXJzb3I6IG1vdmU7IC8qIGZhbGxiYWNrIGlmIGdyYWIgY3Vyc29yIGlzIHVuc3VwcG9ydGVkICovXHJcbiAgICBjdXJzb3I6IGdyYWI7XHJcbiAgICBjdXJzb3I6IC1tb3otZ3JhYjtcclxuICAgIGN1cnNvcjogLXdlYmtpdC1ncmFiO1xyXG59XHJcblxyXG4gICAgLmRyYWctaGFuZGxlOmFjdGl2ZSB7XHJcbiAgICAgICAgY3Vyc29yOiBncmFiYmluZztcclxuICAgICAgICBjdXJzb3I6IC1tb3otZ3JhYmJpbmc7XHJcbiAgICAgICAgY3Vyc29yOiAtd2Via2l0LWdyYWJiaW5nO1xyXG4gICAgfVxyXG5cclxuLyogRHJvcHBhYmxlICovXHJcblxyXG4uZHJhZy1oaW50LWJvcmRlciB7XHJcbiAgICBib3JkZXI6ICMzYzc2M2QgZGFzaGVkIDJweDtcclxufVxyXG5cclxuLmRyYWctb3Zlci1ib3JkZXIge1xyXG4gICAgYm9yZGVyOiAjZmJiYzA1IGRhc2hlZCAycHg7XHJcbn1cclxuXHJcbi5kcmFnLXRyYW5zaXQge1xyXG4gICAgYm9yZGVyOiAjMzUwMEZGIGRhc2hlZCAycHg7XHJcbn1cclxuIl19 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateBlendGroupComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-create-blend-group',
                templateUrl: './create-blend-group.component.html',
                styleUrls: ['./create-blend-group.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"] }, { type: _services_shared_service__WEBPACK_IMPORTED_MODULE_5__["SharedService"] }]; }, { onSubmitGroupData: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/order/create-same-dest-group/create-same-dest-group.component.ts":
/*!**********************************************************************************!*\
  !*** ./src/app/order/create-same-dest-group/create-same-dest-group.component.ts ***!
  \**********************************************************************************/
/*! exports provided: CreateSameDestGroupComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CreateSameDestGroupComponent", function() { return CreateSameDestGroupComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _services_order_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../services/order.service */ "./src/app/order/services/order.service.ts");
/* harmony import */ var _services_shared_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../services/shared.service */ "./src/app/order/services/shared.service.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");










const _c0 = function (a0) { return { "order-selected": a0 }; };
function CreateSameDestGroupComponent_div_18_Template(rf, ctx) { if (rf & 1) {
    const _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSameDestGroupComponent_div_18_Template_div_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r7); const order_r5 = ctx.$implicit; const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r6.OrderClicked(order_r5); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const order_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](5, _c0, order_r5.IsOrderSelected == true));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r5.TfxPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r5.FuelType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r5.DisplayPrice);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r5.Quantity);
} }
function CreateSameDestGroupComponent_span_23_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateSameDestGroupComponent_span_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSameDestGroupComponent_span_23_span_1_Template, 2, 0, "span", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.SameDestGroupForm.get("CustomerPoNumber").errors.required);
} }
function CreateSameDestGroupComponent_input_27_Template(rf, ctx) { if (rf & 1) {
    const _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSameDestGroupComponent_input_27_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r10); const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r9.OnSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateSameDestGroupComponent_input_28_Template(rf, ctx) { if (rf & 1) {
    const _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSameDestGroupComponent_input_28_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r11.OnSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function CreateSameDestGroupComponent_div_29_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Submitting Group...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
class CreateSameDestGroupComponent {
    constructor(fb, orderService, viewGroupService) {
        this.fb = fb;
        this.orderService = orderService;
        this.viewGroupService = viewGroupService;
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.CustomerList = [];
        this.JobList = [];
        this.IsLoading = false;
        this.configSettings = {};
        this.FuelDdlSettings = {};
        this.SingleDdlSettings = {};
        this.orderList = [];
        this.orderListSelected = [];
        this.finalSubmitData = {};
        this.groupId = 0;
        this.isEdit = false;
        this.onSubmitGroupData = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.SameDestGroupForm = this.fb.group({
            CustomerPoNumber: this.fb.control('')
        });
    }
    ngOnInit() {
        this.configSettings = {
            displayKey: 'Name',
            search: true,
        };
        this.FuelDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: true,
            selectAllText: 'Select All',
            unSelectAllText: 'Unselect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            closeDropDownOnSelection: true
        };
        this.SingleDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'Unselect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        if (IsSupplierCompany) {
            this.orderService.getCustomerList().subscribe(data => this.CustomerList = data);
        }
        else {
            this.orderService.getSupplierList().subscribe(data => this.CustomerList = data);
        }
        this.viewGroupService.currentGroup.subscribe(id => {
            this.groupId = id;
            if (id > 0) {
                this.clearDestGroupForm();
                this.GetGroupDetails();
                this.isEdit = true;
            }
        });
    }
    OrderClicked(orderData) {
        let obj = this.orderList.find(o => o.OrderId == orderData.OrderId);
        obj.IsOrderSelected = !obj.IsOrderSelected;
        this.orderListSelected = this.orderList.filter(t => t.IsOrderSelected == true);
    }
    get CustomerPoNumber() {
        return this.SameDestGroupForm.get("CustomerPoNumber");
    }
    get Job() {
        return this.SelectedJob[0];
    }
    get Customer() {
        return this.SelectedCustomer[0];
    }
    get selectedFuelTypeList() {
        return this.SelectedFuelTypes.map(x => x.Id);
    }
    buildProduct(model) {
        return this.fb.group({
            GroupId: this.fb.control(model.GroupId),
            OrderId: this.fb.control(model.OrderId),
            TfxPoNumber: this.fb.control(model.TfxPoNumber),
            FuelType: this.fb.control(model.FuelType),
            Quantity: this.fb.control(model.Quantity),
            DisplayPrice: this.fb.control(model.DisplayPrice),
            BlendPercentage: this.fb.control(model.BlendPercentage, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^[0-9]\d*(\.\d+)?$/)]),
        });
    }
    onFuelSelect(item) {
        this.loadOrders(this.Customer.Id, this.Job.Id, this.selectedFuelTypeList);
    }
    onAllFuelDeSelect(items) {
        this.orderList = [];
    }
    onFuelDeSelect(item) {
        this.RemoveOrders(item.Name);
    }
    loadOrders(_customerId, _jobId, _fuelTypeIds) {
        this.orderService.getFilteredOrdersList(_customerId, _jobId, _fuelTypeIds, this.groupId).subscribe((data) => {
            this.orderList = data;
            for (var i = 0; i < this.orderListSelected.length; i++) {
                this.orderList.filter(x => x.OrderId == this.orderListSelected[i].OrderId).forEach(t => t.IsOrderSelected = this.orderListSelected[i].IsOrderSelected);
            }
        });
    }
    loadFuelType(_customerId, _jobId) {
        this.orderService.getFuelTypesList(_customerId, _jobId).subscribe((data) => {
            this.FuelTypeList = data;
        });
    }
    RemoveOrders(_FuelType) {
        this.orderList = this.orderList.filter(t => t.FuelType != _FuelType);
        this.orderListSelected = this.orderListSelected.filter(t => t.FuelType != _FuelType);
    }
    OnCustomerSelect(customer) {
        this.SelectedJob = '';
        this.SelectedFuelTypes = [];
        this.orderService.getCommonJobList(customer.Id).subscribe(data => this.JobList = data);
    }
    OnCustomerDeSelect(customer) {
        this.SelectedCustomer = '';
        this.JobList = [];
        this.SelectedJob = '';
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.orderListSelected = [];
    }
    OnJobSelect(job) {
        this.orderList = [];
        this.SelectedFuelTypes = [];
        this.orderListSelected = [];
        this.loadFuelType(this.Customer.Id, job.Id);
    }
    OnJobDeSelect(job) {
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.orderListSelected = [];
    }
    OnSubmit() {
        if (this.orderListSelected.length < 2) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror('Group must contain at least 2 orders', undefined, undefined);
            this.SameDestGroupForm.setErrors({ 'invalid': true });
        }
        this.finalSubmitData = {
            Id: this.isEdit ? this.groupId : 0,
            OrderList: this.orderListSelected,
            GroupPoNumber: this.CustomerPoNumber.value,
            BuyerCompanyId: IsSupplierCompany ? this.Customer.Id : currentUserCompanyId,
            SupplierCompanyId: IsSupplierCompany ? currentUserCompanyId : this.Customer.Id,
            JobId: this.Job.Id,
            GroupType: src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["OrderGroupType"].MultiProducts
        };
        if (this.SameDestGroupForm.valid) {
            this.IsLoading = true;
            if (this.isEdit) {
                this.orderService.postEditGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                        this.clearDestGroupForm();
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
            else {
                this.orderService.postCreateGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
    }
    GetGroupDetails() {
        this.orderService.getBlendGroupDetails(this.groupId).subscribe(group => {
            this.JobList = group.Jobs;
            this.FuelTypeList = group.FuelTypes;
            this.orderList = group.FilteredOrders;
            this.CustomerPoNumber.setValue(group.GroupDetails.GroupPoNumber);
            this.SelectedCustomer = IsSupplierCompany ? this.getCustomerFromSupplier(group.GroupDetails.BuyerCompanyId) : this.getCustomerFromSupplier(group.GroupDetails.SupplierCompanyId);
            this.SelectedJob = this.JobList.filter(ele => ele.Id == group.GroupDetails.JobId);
            var _fuelTypes = group.GroupDetails.OrderList.map(x => x.Order.FuelType);
            this.SelectedFuelTypes = this.FuelTypeList.filter(ele => _fuelTypes.indexOf(ele.Name) > -1);
            this.orderList.filter(ele => _fuelTypes.indexOf(ele.FuelType) > -1).forEach(t => t.IsOrderSelected = true);
            this.orderListSelected = this.orderList.filter(x => x.IsOrderSelected == true);
            //for (var i = 0; i < group.GroupDetails.OrderList.length; i++) {
            //    var orderDetail: OrderGroupDetails = group.GroupDetails.OrderList[i];
            //    var orderToMove: OrderGroupDetails[] = this.orderList.filter(t => t.OrderId == orderDetail.OrderId);
            //    this.setSelectedOrders(orderToMove[0]);
            //}
        });
    }
    getCustomerFromSupplier(id) {
        return this.CustomerList.filter(e => e.Id == id);
    }
    clearDestGroupForm() {
        if (this.SelectedCustomer != null) {
            this.OnCustomerDeSelect(this.SelectedCustomer);
        }
        this.CustomerPoNumber.setValue("");
        this.isEdit = false;
    }
}
CreateSameDestGroupComponent.ɵfac = function CreateSameDestGroupComponent_Factory(t) { return new (t || CreateSameDestGroupComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_shared_service__WEBPACK_IMPORTED_MODULE_5__["SharedService"])); };
CreateSameDestGroupComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: CreateSameDestGroupComponent, selectors: [["app-create-same-dest-group"]], outputs: { onSubmitGroupData: "onSubmitGroupData" }, decls: 30, vars: 15, consts: [[1, "row"], [1, "form-group", "col-sm-3"], [3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], [1, "form-group", "col-sm-4"], [3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [3, "formGroup"], [1, "group-height"], [1, "col-sm-9"], ["class", "col-sm-3", 4, "ngFor", "ngForOf"], [1, "col-sm-3"], ["for", "CustomerPoNumber"], ["type", "text", "formControlName", "CustomerPoNumber", 1, "form-control"], ["class", "help-block color-maroon", 4, "ngIf"], [1, "col-sm-12", "text-right"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", "btn-lg", 3, "click"], ["type", "button", "class", "btn btn-primary btn-lg", "value", "Create", 3, "click", 4, "ngIf"], ["type", "button", "class", "btn btn-primary btn-lg", "value", "Submit", 3, "click", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [1, "well", 2, "cursor", "pointer", 3, "ngClass", "click"], [1, "mt0", "mb0"], [1, "help-block", "color-maroon"], [4, "ngIf"], ["type", "button", "value", "Create", 1, "btn", "btn-primary", "btn-lg", 3, "click"], ["type", "button", "value", "Submit", 1, "btn", "btn-primary", "btn-lg", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CreateSameDestGroupComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Customer");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { return ctx.SelectedCustomer = $event; })("onSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { return ctx.OnCustomerSelect($event); })("onDeSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { return ctx.OnCustomerDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Job");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "ng-multiselect-dropdown", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_8_listener($event) { return ctx.SelectedJob = $event; })("onSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onSelect_8_listener($event) { return ctx.OnJobSelect($event); })("onDeSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_8_listener($event) { return ctx.OnJobDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Fuel Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_12_listener($event) { return ctx.SelectedFuelTypes = $event; })("onSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onSelect_12_listener($event) { return ctx.onFuelSelect($event); })("onDeSelect", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_12_listener($event) { return ctx.onFuelDeSelect($event); })("onSelectAll", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onSelectAll_12_listener($event) { return ctx.onFuelSelect($event); })("onDeSelectAll", function CreateSameDestGroupComponent_Template_ng_multiselect_dropdown_onDeSelectAll_12_listener($event) { return ctx.onAllFuelDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, CreateSameDestGroupComponent_div_18_Template, 12, 7, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Customer PO#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](22, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, CreateSameDestGroupComponent_span_23_Template, 2, 1, "span", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "input", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSameDestGroupComponent_Template_input_click_26_listener() { return ctx.clearDestGroupForm(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, CreateSameDestGroupComponent_input_27_Template, 1, 0, "input", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, CreateSameDestGroupComponent_input_28_Template, 1, 0, "input", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](29, CreateSameDestGroupComponent_div_29_Template, 5, 0, "div", 17);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedCustomer)("settings", ctx.SingleDdlSettings)("data", ctx.CustomerList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedJob)("settings", ctx.SingleDdlSettings)("data", ctx.JobList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedFuelTypes)("settings", ctx.FuelDdlSettings)("data", ctx.FuelTypeList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.SameDestGroupForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.orderList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.SameDestGroupForm.get("CustomerPoNumber").errors && (ctx.SameDestGroupForm.get("CustomerPoNumber").touched || ctx.SameDestGroupForm.get("CustomerPoNumber").dirty));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isEdit);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isEdit);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"]], styles: [".order-selected[_ngcontent-%COMP%] {\r\n    background-color: antiquewhite;\r\n    cursor: pointer;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvb3JkZXIvY3JlYXRlLXNhbWUtZGVzdC1ncm91cC9jcmVhdGUtc2FtZS1kZXN0LWdyb3VwLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSw4QkFBOEI7SUFDOUIsZUFBZTtBQUNuQiIsImZpbGUiOiJzcmMvYXBwL29yZGVyL2NyZWF0ZS1zYW1lLWRlc3QtZ3JvdXAvY3JlYXRlLXNhbWUtZGVzdC1ncm91cC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLm9yZGVyLXNlbGVjdGVkIHtcclxuICAgIGJhY2tncm91bmQtY29sb3I6IGFudGlxdWV3aGl0ZTtcclxuICAgIGN1cnNvcjogcG9pbnRlcjtcclxufVxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateSameDestGroupComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-create-same-dest-group',
                templateUrl: './create-same-dest-group.component.html',
                styleUrls: ['./create-same-dest-group.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"] }, { type: _services_shared_service__WEBPACK_IMPORTED_MODULE_5__["SharedService"] }]; }, { onSubmitGroupData: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/order/filter-group/filter-group.component.ts":
/*!**************************************************************!*\
  !*** ./src/app/order/filter-group/filter-group.component.ts ***!
  \**************************************************************/
/*! exports provided: FilterGroupComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FilterGroupComponent", function() { return FilterGroupComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _models_ViewOrderGroupDdlModel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../models/ViewOrderGroupDdlModel */ "./src/app/order/models/ViewOrderGroupDdlModel.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../services/viewordergroup.service */ "./src/app/order/services/viewordergroup.service.ts");
/* harmony import */ var _services_order_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../services/order.service */ "./src/app/order/services/order.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");








function FilterGroupComponent_option_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grpType_r7 = ctx.$implicit;
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngValue", ctx_r0.SelectedGroupType)("value", grpType_r7.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grpType_r7.Name);
} }
function FilterGroupComponent_label_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Select Customer");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FilterGroupComponent_label_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Select Supplier");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function FilterGroupComponent_div_15_Template(rf, ctx) { if (rf & 1) {
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Select Job/Site Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function FilterGroupComponent_div_15_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r8.SelectedJob = $event; })("onSelect", function FilterGroupComponent_div_15_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r10.OnJobSelect($event); })("onDeSelect", function FilterGroupComponent_div_15_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r11.OnJobDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r3.SelectedJob)("settings", ctx_r3.JobDdlSettings)("data", ctx_r3.JobList);
} }
function FilterGroupComponent_div_16_Template(rf, ctx) { if (rf & 1) {
    const _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Select Fuel Group");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function FilterGroupComponent_div_16_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13); const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r12.SelectedFuelGroup = $event; })("onSelect", function FilterGroupComponent_div_16_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13); const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r14.OnFuelGroupSelect($event); })("onDeSelect", function FilterGroupComponent_div_16_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r15.OnFuelGroupDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r4.SelectedFuelGroup)("settings", ctx_r4.FuelGroupDdlSettings)("data", ctx_r4.FuelGroupList);
} }
function FilterGroupComponent_div_17_Template(rf, ctx) { if (rf & 1) {
    const _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Select State");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function FilterGroupComponent_div_17_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17); const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r16.SelectedState = $event; })("onSelect", function FilterGroupComponent_div_17_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r18.OnStateSelect($event); })("onDeSelect", function FilterGroupComponent_div_17_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r19.OnStateDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r5.SelectedState)("settings", ctx_r5.StateDdlSettings)("data", ctx_r5.StateList);
} }
function FilterGroupComponent_div_26_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
const _c0 = function () { return []; };
class FilterGroupComponent {
    constructor(fb, viewOrderGroupService, orderService) {
        this.fb = fb;
        this.viewOrderGroupService = viewOrderGroupService;
        this.orderService = orderService;
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        this.IsMultiProduct = true;
        this.IsTier = false;
        this.IsBlend = false;
        this.IsLoading = true;
        this.GroupTypeList = [];
        this.CompanyList = [];
        this.JobList = [];
        this.FuelGroupList = [];
        this.StateList = [];
        this.GroupTypeDdlSettings = {};
        this.CompanyDdlSettings = {};
        this.JobDdlSettings = {};
        this.FuelGroupDdlSettings = {};
        this.StateDdlSettings = {};
        this.onViewOrderGroupResponse = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.model = new _models_ViewOrderGroupDdlModel__WEBPACK_IMPORTED_MODULE_1__["ViewOrderGroupDdlModel"]();
    }
    ngOnInit() {
        this.IsLoading = true;
        this.model.GroupTypeId = 0;
        this.filterGroupForm = this.fb.group({
            GroupType: this.fb.control(null),
            Customer: this.fb.control(null),
            Job: this.fb.control(null),
            ProductCategory: this.fb.control(null),
            State: this.fb.control(null),
            GroupTypeId: this.fb.control(0),
            CompanyId: this.fb.control(null),
            JobId: this.fb.control(null),
            FuelGroupId: this.fb.control(null),
            StateId: this.fb.control(null),
            SearchText: this.fb.control(null),
        });
        this.GroupTypeDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.CompanyDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.JobDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.FuelGroupDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.StateDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
            this.GroupTypeList = data.GroupTypes;
            this.CompanyList = data.Companies;
            this.FuelGroupList = data.ProductCategories;
            this.StateList = data.States;
            this.IsSupplierCompany = data.IsSupplierCompany;
        });
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.onOrderGroupFilterSubmit();
    }
    showHideControlsByGroupType(groupTypeId) {
        if (groupTypeId == 2) {
            this.IsTier = true;
            this.IsMultiProduct = false;
            this.IsBlend = false;
        }
        else if (groupTypeId == 3) {
            this.IsBlend = true;
            this.IsTier = false;
            this.IsMultiProduct = false;
        }
        else {
            this.IsMultiProduct = true;
            this.IsTier = false;
            this.IsBlend = false;
        }
    }
    OnGroupTypeSelect(groupType) {
        this.IsLoading = true;
        this.resetForm();
        this.model.GroupTypeId = groupType.target.value;
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
            this.CompanyList = data.Companies;
            this.FuelGroupList = data.ProductCategories;
            this.StateList = data.States;
            this.IsLoading = false;
        });
        this.onOrderGroupFilterSubmit();
    }
    onOrderGroupFilterSubmit() {
        this.IsLoading = true;
        this.model.SearchText = this.filterGroupForm.get('SearchText').value;
        this.viewOrderGroupService.getOrderGroupDetails(this.model)
            .subscribe(data => {
            this.IsLoading = false;
            this.onViewOrderGroupResponse.emit(data);
        });
    }
    resetForm() {
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        this.onViewOrderGroupResponse.emit([]);
    }
    onSearch(searchText) {
        var text = searchText.target.value;
        if (text != null && text != '' && text.length >= 3) {
            this.IsLoading = true;
            this.model.SearchText = text;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                this.IsLoading = false;
                this.onViewOrderGroupResponse.emit(data);
            });
        }
        else if (text.length == 0) {
            this.IsLoading = true;
            this.model.SearchText = null;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                this.IsLoading = false;
                this.onViewOrderGroupResponse.emit(data);
            });
        }
    }
    OnCompanySelect(company) {
        this.model.CompanyId = company.Id;
        this.JobList = [];
        this.orderService.getCommonJobList(company.Id).subscribe(data => this.JobList = data);
    }
    OnCompanyDeSelect(groupType) {
        this.JobList = [];
        this.model.CompanyId = null;
    }
    OnJobSelect(job) {
        this.model.JobId = job.Id;
    }
    OnJobDeSelect(groupType) {
        this.model.JobId = null;
    }
    OnFuelGroupSelect(fuelGroup) {
        this.model.ProductCategoryId = fuelGroup.Id;
    }
    OnFuelGroupDeSelect(fuelGroup) {
        this.model.ProductCategoryId = null;
    }
    OnStateSelect(state) {
        this.model.StateId = state.Id;
    }
    OnStateDeSelect(state) {
        this.model.StateId = null;
    }
}
FilterGroupComponent.ɵfac = function FilterGroupComponent_Factory(t) { return new (t || FilterGroupComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_3__["ViewOrderGroupService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"])); };
FilterGroupComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: FilterGroupComponent, selectors: [["app-filter-group"]], outputs: { onViewOrderGroupResponse: "onViewOrderGroupResponse" }, decls: 27, vars: 12, consts: [[1, "row"], [1, "col-sm-2"], [1, "form-group"], [1, "form-control", 3, "change"], ["value", "0"], [3, "ngValue", "value", 4, "ngFor", "ngForOf"], [4, "ngIf"], [3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], ["class", "col-sm-2", 4, "ngIf"], [3, "formGroup"], ["type", "submit", "value", "Apply", 1, "btn", "btn-primary", "btn-submit", "btn-lg", "no-disable", 3, "click"], ["type", "text", "formControlName", "SearchText", 1, "form-control", 3, "keypress"], ["class", "loader", 4, "ngIf"], [3, "ngValue", "value"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function FilterGroupComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Select Grouping Purpose");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "select", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function FilterGroupComponent_Template_select_change_6_listener($event) { return ctx.OnGroupTypeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "All");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, FilterGroupComponent_option_9_Template, 2, 3, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, FilterGroupComponent_label_12_Template, 2, 0, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, FilterGroupComponent_label_13_Template, 2, 0, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function FilterGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_14_listener($event) { return ctx.SelectedCompany = $event; })("onSelect", function FilterGroupComponent_Template_ng_multiselect_dropdown_onSelect_14_listener($event) { return ctx.OnCompanySelect($event); })("onDeSelect", function FilterGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_14_listener($event) { return ctx.OnCompanyDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, FilterGroupComponent_div_15_Template, 5, 3, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, FilterGroupComponent_div_16_Template, 5, 3, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, FilterGroupComponent_div_17_Template, 5, 3, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "input", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function FilterGroupComponent_Template_input_click_20_listener() { return ctx.onOrderGroupFilterSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Search");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("keypress", function FilterGroupComponent_Template_input_keypress_25_listener($event) { return ctx.onSearch($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, FilterGroupComponent_div_26_Template, 5, 0, "div", 12);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.GroupTypeList || _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction0"](11, _c0));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsSupplierCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsSupplierCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedCompany)("settings", ctx.CompanyDdlSettings)("data", ctx.CompanyList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsMultiProduct || ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsSupplierCompany && ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.filterGroupForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL29yZGVyL2ZpbHRlci1ncm91cC9maWx0ZXItZ3JvdXAuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FilterGroupComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-filter-group',
                templateUrl: './filter-group.component.html',
                styleUrls: ['./filter-group.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"] }, { type: _services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_3__["ViewOrderGroupService"] }, { type: _services_order_service__WEBPACK_IMPORTED_MODULE_4__["OrderService"] }]; }, { onViewOrderGroupResponse: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/order/lazy-loading/order-routing.module.ts":
/*!************************************************************!*\
  !*** ./src/app/order/lazy-loading/order-routing.module.ts ***!
  \************************************************************/
/*! exports provided: OrderRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderRoutingModule", function() { return OrderRoutingModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _view_order_group_view_order_group_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../view-order-group/view-order-group.component */ "./src/app/order/view-order-group/view-order-group.component.ts");





const routeOrder = [
    {
        path: '',
        component: _view_order_group_view_order_group_component__WEBPACK_IMPORTED_MODULE_2__["ViewOrderGroupComponent"]
    }
];
class OrderRoutingModule {
}
OrderRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: OrderRoutingModule });
OrderRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function OrderRoutingModule_Factory(t) { return new (t || OrderRoutingModule)(); }, imports: [[
            _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeOrder)
        ],
        _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](OrderRoutingModule, { imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]], exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](OrderRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                imports: [
                    _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeOrder)
                ],
                exports: [
                    _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/order/lazy-loading/order.module.ts":
/*!****************************************************!*\
  !*** ./src/app/order/lazy-loading/order.module.ts ***!
  \****************************************************/
/*! exports provided: OrderModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderModule", function() { return OrderModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _view_order_group_view_order_group_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../view-order-group/view-order-group.component */ "./src/app/order/view-order-group/view-order-group.component.ts");
/* harmony import */ var _order_routing_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./order-routing.module */ "./src/app/order/lazy-loading/order-routing.module.ts");
/* harmony import */ var _create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../create-blend-group/create-blend-group.component */ "./src/app/order/create-blend-group/create-blend-group.component.ts");
/* harmony import */ var _create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../create-same-dest-group/create-same-dest-group.component */ "./src/app/order/create-same-dest-group/create-same-dest-group.component.ts");
/* harmony import */ var _filter_group_filter_group_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../filter-group/filter-group.component */ "./src/app/order/filter-group/filter-group.component.ts");
/* harmony import */ var _term_pricing_contract_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../term-pricing-contract.component */ "./src/app/order/term-pricing-contract.component.ts");
/* harmony import */ var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! src/app/modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! src/app/modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ng-drag-drop */ "./node_modules/ng-drag-drop/__ivy_ngcc__/index.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_9___default = /*#__PURE__*/__webpack_require__.n(ng_drag_drop__WEBPACK_IMPORTED_MODULE_9__);












class OrderModule {
}
OrderModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: OrderModule });
OrderModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function OrderModule_Factory(t) { return new (t || OrderModule)(); }, imports: [[
            _order_routing_module__WEBPACK_IMPORTED_MODULE_2__["OrderRoutingModule"],
            src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_7__["SharedModule"],
            src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"],
            ng_drag_drop__WEBPACK_IMPORTED_MODULE_9__["NgDragDropModule"].forRoot(),
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](OrderModule, { declarations: [_view_order_group_view_order_group_component__WEBPACK_IMPORTED_MODULE_1__["ViewOrderGroupComponent"],
        _create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_3__["CreateBlendGroupComponent"],
        _create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__["CreateSameDestGroupComponent"],
        _filter_group_filter_group_component__WEBPACK_IMPORTED_MODULE_5__["FilterGroupComponent"],
        _term_pricing_contract_component__WEBPACK_IMPORTED_MODULE_6__["TermPricingContractComponent"]], imports: [_order_routing_module__WEBPACK_IMPORTED_MODULE_2__["OrderRoutingModule"],
        src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_7__["SharedModule"],
        src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_9__["NgDragDropModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](OrderModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [
                    _view_order_group_view_order_group_component__WEBPACK_IMPORTED_MODULE_1__["ViewOrderGroupComponent"],
                    _create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_3__["CreateBlendGroupComponent"],
                    _create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__["CreateSameDestGroupComponent"],
                    _filter_group_filter_group_component__WEBPACK_IMPORTED_MODULE_5__["FilterGroupComponent"],
                    _term_pricing_contract_component__WEBPACK_IMPORTED_MODULE_6__["TermPricingContractComponent"],
                ],
                imports: [
                    _order_routing_module__WEBPACK_IMPORTED_MODULE_2__["OrderRoutingModule"],
                    src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_7__["SharedModule"],
                    src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_8__["DirectiveModule"],
                    ng_drag_drop__WEBPACK_IMPORTED_MODULE_9__["NgDragDropModule"].forRoot(),
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/order/models/OrderDetail.ts":
/*!*********************************************!*\
  !*** ./src/app/order/models/OrderDetail.ts ***!
  \*********************************************/
/*! exports provided: OrderDetailModel, OrderList, CurrentUser, OrderGroupViewModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderDetailModel", function() { return OrderDetailModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderList", function() { return OrderList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CurrentUser", function() { return CurrentUser; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderGroupViewModel", function() { return OrderGroupViewModel; });
class OrderDetailModel {
}
class OrderList {
    constructor() {
        this.Order = new OrderDetailModel();
    }
}
class CurrentUser {
}
class OrderGroupViewModel {
}


/***/ }),

/***/ "./src/app/order/models/ViewOrderGroupDdlModel.ts":
/*!********************************************************!*\
  !*** ./src/app/order/models/ViewOrderGroupDdlModel.ts ***!
  \********************************************************/
/*! exports provided: ViewOrderGroupDdlModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewOrderGroupDdlModel", function() { return ViewOrderGroupDdlModel; });
class ViewOrderGroupDdlModel {
}


/***/ }),

/***/ "./src/app/order/services/order.service.ts":
/*!*************************************************!*\
  !*** ./src/app/order/services/order.service.ts ***!
  \*************************************************/
/*! exports provided: OrderService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderService", function() { return OrderService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class OrderService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getCustomerListUrl = '/Supplier/OrderGroup/GetCustomersForSupplier';
        this.getJobListUrl = '/Supplier/OrderGroup/GetJobsForCustomer?buyerCompanyId=';
        this.getCommonJobListUrl = '/OrderBase/GetJobsForCustomers?companyId=';
        this.postCreateGroupUrl = '/OrderBase/CreateOrderGroup';
        this.postEditGroupUrl = '/OrderBase/EditOrderGroup';
        this.getFilteredOrders = '/OrderBase/GetFilteredOrders';
        this.getFuelTypes = '/OrderBase/GetFuelTypes?';
        this.getCurrentUserUrl = '/OrderBase/GetUserContext';
        this.getSupplierListUrl = '/Buyer/OrderGroup/GetSuppliersForCustomer';
        this.getGroupDetailsUrl = '/OrderBase/GetOrderGroupDetails?groupId=';
        this.getBlendedGroupDetailstUrl = '/OrderBase/GetBlendedGroupDetails?groupId=';
    }
    getCustomerList() {
        return this.httpClient.get(this.getCustomerListUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCustomerList', null)));
    }
    getBlendGroupDetails(groupId) {
        return this.httpClient.get(this.getBlendedGroupDetailstUrl + groupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBlendGroupDetails', null)));
    }
    getSupplierList() {
        return this.httpClient.get(this.getSupplierListUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSupplierList', null)));
    }
    getJobList(customerId) {
        return this.httpClient.get(this.getJobListUrl + customerId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobList', null)));
    }
    getCommonJobList(customerId) {
        return this.httpClient.get(this.getCommonJobListUrl + customerId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCommonJobList', null)));
    }
    getFilteredOrdersList(customerId, jobId, tfxFuelTypeIds, groupId) {
        return this.httpClient.post(this.getFilteredOrders, { customerId: customerId, jobId: jobId, tfxFuelTypeIds: tfxFuelTypeIds, groupId: groupId }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFilteredOrdersList', null)));
    }
    getJobListByFuelGroupType(customerId, supplierId, fuelGroupType) {
        return this.httpClient.get(this.getJobListByFuelGroupUrl(customerId, supplierId, fuelGroupType)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobListByFuelGroupType', null)));
    }
    getOrderList(buyerCompanyId, supplierCompanyId, fuelGroupType, jobId) {
        return this.httpClient.get(this.getOrderListUrl(buyerCompanyId, supplierCompanyId, fuelGroupType, jobId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getOrderList', null)));
    }
    getCurrentUser() {
        return this.httpClient.get(this.getCurrentUserUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCurrentUser', null)));
    }
    getGroupDetails(groupId) {
        return this.httpClient.get(this.getGroupDetailsUrl + groupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getGroupDetails', null)));
    }
    getOrderListUrl(a, b, c, d) {
        return `/OrderBase/GetOrdersForTierGroup?buyerCompanyId=${a}&supplierCompanyId=${b}&fuelGroupType=${c}&jobId=${d}`;
    }
    getFuelTypesList(customerId, jobId) {
        return this.httpClient.get(this.getFuelTypes + "customerId=" + customerId + "&jobId=" + jobId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelTypesList', null)));
    }
    getJobListByFuelGroupUrl(a, b, c) {
        return `/OrderBase/GetJobsByFuelGroup?buyerCompanyId=${a}&supplierCompanyId=${b}&fuelGroupType=${c}`;
    }
    postCreateGroup(groupModel) {
        return this.httpClient.post(this.postCreateGroupUrl, groupModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postCreateGroup', null)));
    }
    postEditGroup(groupModel) {
        return this.httpClient.post(this.postEditGroupUrl, groupModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('postEditGroup', null)));
    }
}
OrderService.ɵfac = function OrderService_Factory(t) { return new (t || OrderService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
OrderService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: OrderService, factory: OrderService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](OrderService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/order/services/shared.service.ts":
/*!**************************************************!*\
  !*** ./src/app/order/services/shared.service.ts ***!
  \**************************************************/
/*! exports provided: SharedService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SharedService", function() { return SharedService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");



class SharedService {
    constructor() {
        this.selectedGroupId = new rxjs__WEBPACK_IMPORTED_MODULE_1__["BehaviorSubject"](0);
        this.currentGroup = this.selectedGroupId.asObservable();
    }
    setGroupId(groupId) {
        this.selectedGroupId.next(groupId);
    }
}
SharedService.ɵfac = function SharedService_Factory(t) { return new (t || SharedService)(); };
SharedService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: SharedService, factory: SharedService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SharedService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return []; }, null); })();


/***/ }),

/***/ "./src/app/order/services/viewordergroup.service.ts":
/*!**********************************************************!*\
  !*** ./src/app/order/services/viewordergroup.service.ts ***!
  \**********************************************************/
/*! exports provided: ViewOrderGroupService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewOrderGroupService", function() { return ViewOrderGroupService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class ViewOrderGroupService extends _errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getOrderGroupAllDdlUrl = '/OrderBase/FillOrderGroupDdl?groupTypeId=';
        this.viewOrderGroupDetailsUrl = '/OrderBase/ViewOrderGroupDetails';
        this.getCommonJobListUrl = '/OrderBase/GetJobsForCustomers?companyId=';
        this.getDeleteOrderGroupUrl = '/OrderBase/DeleteOrderGroup?groupId=';
    }
    fillDDLByGroup(groupTypeId) {
        return this.httpClient.get(this.getOrderGroupAllDdlUrl + groupTypeId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('fillViewOrderGroupAllDDL', null)));
    }
    getOrderGroupDetails(viewOrderGroupFilterModel) {
        return this.httpClient.post(this.viewOrderGroupDetailsUrl, viewOrderGroupFilterModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getOrderGroupDetails', null)));
    }
    getCommonJobList(customerId) {
        return this.httpClient.get(this.getCommonJobListUrl + customerId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCommonJobList', null)));
    }
    deleteOrderGroup(groupId) {
        return this.httpClient.post(this.getDeleteOrderGroupUrl + groupId, groupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteOrderGroup', null)));
    }
}
ViewOrderGroupService.ɵfac = function ViewOrderGroupService_Factory(t) { return new (t || ViewOrderGroupService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
ViewOrderGroupService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: ViewOrderGroupService, factory: ViewOrderGroupService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ViewOrderGroupService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/order/term-pricing-contract.component.ts":
/*!**********************************************************!*\
  !*** ./src/app/order/term-pricing-contract.component.ts ***!
  \**********************************************************/
/*! exports provided: TermPricingContractComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TermPricingContractComponent", function() { return TermPricingContractComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _order_models_OrderDetail__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../order/models/OrderDetail */ "./src/app/order/models/OrderDetail.ts");
/* harmony import */ var _declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _app_constants__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../app.constants */ "./src/app/app.constants.ts");
/* harmony import */ var _services_order_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./services/order.service */ "./src/app/order/services/order.service.ts");
/* harmony import */ var _services_shared_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./services/shared.service */ "./src/app/order/services/shared.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ng-drag-drop */ "./node_modules/ng-drag-drop/__ivy_ngcc__/index.js");
/* harmony import */ var ng_drag_drop__WEBPACK_IMPORTED_MODULE_10___default = /*#__PURE__*/__webpack_require__.n(ng_drag_drop__WEBPACK_IMPORTED_MODULE_10__);
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");














function TermPricingContractComponent_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Customer");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function TermPricingContractComponent_div_1_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r8); const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r7.SelectedCustomer = $event; })("onSelect", function TermPricingContractComponent_div_1_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r8); const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r9.OnCustomerSelect($event); })("onDeSelect", function TermPricingContractComponent_div_1_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r8); const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r10.OnCustomerDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r0.SelectedCustomer)("data", ctx_r0.CustomerList)("settings", ctx_r0.DdlSettings);
} }
function TermPricingContractComponent_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Supplier");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function TermPricingContractComponent_div_2_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r11.SelectedSupplier = $event; })("onSelect", function TermPricingContractComponent_div_2_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r13.OnSupplierSelect($event); })("onDeSelect", function TermPricingContractComponent_div_2_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12); const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r14.OnSupplierDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r1.SelectedSupplier)("settings", ctx_r1.DdlSettings)("data", ctx_r1.SupplierList);
} }
function TermPricingContractComponent_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_12_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_12_span_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_12_span_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_ng_container_20_span_12_span_1_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TermPricingContractComponent_ng_container_20_span_12_span_2_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, TermPricingContractComponent_ng_container_20_span_12_span_3_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const tier_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MinVolume").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MinVolume").errors.pattern);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !tier_r15.get("MinVolume").errors.required && !tier_r15.get("MinVolume").errors.pattern && tier_r15.get("MinVolume").errors);
} }
function TermPricingContractComponent_ng_container_20_span_16_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_16_span_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_ng_container_20_span_16_span_1_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TermPricingContractComponent_ng_container_20_span_16_span_2_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const tier_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MaxVolume").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MaxVolume").errors.pattern);
} }
function TermPricingContractComponent_ng_container_20_div_20_h3_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Please drag order here");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TermPricingContractComponent_ng_container_20_div_20_h3_2_Template, 2, 0, "h3", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const tier_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("Order").get("FuelType").value == null);
} }
function TermPricingContractComponent_ng_container_20_div_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "h3", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const tier_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tier_r15.get("Order").get("FuelType").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tier_r15.get("Order").get("TfxPoNumber").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tier_r15.get("Order").get("Quantity").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](tier_r15.get("Order").get("DisplayPrice").value);
} }
function TermPricingContractComponent_ng_container_20_span_22_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_ng_container_20_span_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_ng_container_20_span_22_span_1_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const tier_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("Order").get("OrderId").errors.required);
} }
const _c0 = function (a0) { return [a0]; };
function TermPricingContractComponent_ng_container_20_Template(rf, ctx) { if (rf & 1) {
    const _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h3", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TermPricingContractComponent_ng_container_20_Template_a_click_6_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const tier_r15 = ctx.$implicit; const i_r16 = ctx.index; const ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r34.removeTier(tier_r15, i_r16); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "i", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "input", 36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function TermPricingContractComponent_ng_container_20_Template_input_change_11_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const tier_r15 = ctx.$implicit; const i_r16 = ctx.index; const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r36.comparisonValidator(tier_r15, i_r16, "MinVolume"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, TermPricingContractComponent_ng_container_20_span_12_Template, 4, 3, "span", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "input", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function TermPricingContractComponent_ng_container_20_Template_input_change_15_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const tier_r15 = ctx.$implicit; const i_r16 = ctx.index; const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r37.comparisonValidator(tier_r15, i_r16, "MaxVolume"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, TermPricingContractComponent_ng_container_20_span_16_Template, 3, 2, "span", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDrop", function TermPricingContractComponent_ng_container_20_Template_div_onDrop_19_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r35); const i_r16 = ctx.index; const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r38.onItemDropTier(i_r16, $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, TermPricingContractComponent_ng_container_20_div_20_Template, 3, 1, "div", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, TermPricingContractComponent_ng_container_20_div_21_Template, 12, 4, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, TermPricingContractComponent_ng_container_20_span_22_Template, 2, 1, "span", 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const tier_r15 = ctx.$implicit;
    const i_r16 = ctx.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r16)("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](10, _c0, "bg" + i_r16));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Tier ", i_r16 + 1, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MinVolume").errors && (tier_r15.get("MinVolume").touched || tier_r15.get("MinVolume").dirty));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("MaxVolume").errors && (tier_r15.get("MaxVolume").touched || tier_r15.get("MaxVolume").dirty));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", tier_r15.get("Order"));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dragData", tier_r15.get("Order").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("Order").get("FuelType").value == null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("Order").get("FuelType").value != null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", tier_r15.get("Order").get("OrderId").errors && (tier_r15.get("Order").get("OrderId").touched || tier_r15.get("Order").get("OrderId").dirty));
} }
function TermPricingContractComponent_div_29_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h3", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const order_r39 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dragData", order_r39);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r39.FuelType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r39.TfxPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r39.Quantity);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](order_r39.DisplayPrice);
} }
function TermPricingContractComponent_span_42_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_span_42_span_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_span_42_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_span_42_span_1_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TermPricingContractComponent_span_42_span_2_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.isRequired(ctx_r5.TermPricingForm, "RenewalCount"));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r5.isRequired(ctx_r5.TermPricingForm, "RenewalCount") && ctx_r5.isInvalid(ctx_r5.TermPricingForm, "RenewalCount"));
} }
function TermPricingContractComponent_span_50_span_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function TermPricingContractComponent_span_50_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_span_50_span_1_Template, 2, 0, "span", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.isRequired(ctx_r6.TermPricingForm, "StartDate"));
} }
class TermPricingContractComponent {
    constructor(fb, orderService, viewGroupService) {
        this.fb = fb;
        this.orderService = orderService;
        this.viewGroupService = viewGroupService;
        this.CustomerList = [];
        this.SupplierList = [];
        this.FuelGroupList = [{ Id: 1, Name: "Gasoline" }, { Id: 2, Name: "Diesel" }];
        this.JobList = [];
        this.SelectedFuelGroup = [{ Id: 1, Name: "Gasoline" }];
        this.SelectedCustomer = [];
        this.SelectedSupplier = [];
        this.SelectedJob = [];
        this.DdlSettings = {};
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.IsLoading = true;
        this.OrderList = [];
        this.DefaultTier = { Order: new _order_models_OrderDetail__WEBPACK_IMPORTED_MODULE_2__["OrderDetailModel"](), OrderId: null, MinVolume: null, MaxVolume: null };
        this.CurrentUser = { IsBuyerCompany: !IsSupplierCompany, IsSupplierCompany: IsSupplierCompany };
        this.emptyOrder = {
            OrderId: null,
            TfxPoNumber: null,
            FuelType: null,
            Quantity: null,
            DisplayPrice: null
        };
        this.isDropAllowed = (dragData) => {
            return dragData > 500;
        };
    }
    ngOnInit() {
        this.DisplayDate = moment__WEBPACK_IMPORTED_MODULE_4__(new Date()).format('MM/DD/YYYY');
        this.NextRenewalDate = moment__WEBPACK_IMPORTED_MODULE_4__(new Date()).add(1, 'months').startOf('month').format('MM/DD/YYYY');
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 10);
        this.TermPricingForm = this.fb.group({
            GroupType: this.fb.control('2', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            BuyerCompanyId: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            SupplierCompanyId: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            ProductType: this.fb.control(1, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            JobId: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            StartDate: this.fb.control(this.DisplayDate, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            RenewalFrequency: this.fb.control('1', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            RenewalPeriod: this.fb.control('Monthly'),
            RenewalCount: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(_app_constants__WEBPACK_IMPORTED_MODULE_5__["RegExConstants"].Integer)]),
            GroupPoNumber: this.fb.control(''),
            OrderList: this.fb.array([]),
        });
        this.DdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            closeDropDownOnSelection: true,
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.viewGroupService.currentGroup.subscribe(id => this.groupId = id);
        if (IsSupplierCompany) {
            this.SupplierList = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
            this.SelectedSupplier = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
            this.TermPricingForm.get('SupplierCompanyId').patchValue(currentUserCompanyId);
            this.GetCustomerList();
        }
        else {
            this.CustomerList = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
            this.SelectedCustomer = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
            this.TermPricingForm.get('BuyerCompanyId').patchValue(currentUserCompanyId);
            this.GetSupplierList();
        }
        if (this.groupId == 0) {
            this.addNewTier(this.DefaultTier);
        }
        this.IsLoading = false;
    }
    ngAfterViewInit() {
        if (this.groupId != null && this.groupId != undefined && this.groupId > 0) {
            this.getDefaultGroupDetail(this.groupId);
        }
    }
    getDefaultGroupDetail(_groupId) {
        this.orderService.getGroupDetails(_groupId)
            .subscribe(response => {
            this.OrderGroupModel = response;
            this.initFormData(response);
        });
    }
    initFormData(model) {
        if (model != null && model != undefined && model.Id > 0) {
            this.TermPricingForm.patchValue({
                GroupType: model.GroupType,
                BuyerCompanyId: model.BuyerCompanyId,
                SupplierCompanyId: model.SupplierCompanyId,
                ProductType: model.ProductType,
                JobId: model.JobId,
                StartDate: model.StartDate,
                RenewalFrequency: model.RenewalFrequency,
                RenewalPeriod: 'Monthly',
                RenewalCount: model.RenewalCount,
                GroupPoNumber: model.GroupPoNumber,
                OrderList: model.OrderList
            });
            this.SelectedCustomer = this.CustomerList.filter(function (item) { return item.Id == model.BuyerCompanyId; });
            this.SelectedSupplier = this.SupplierList.filter(function (item) { return item.Id == model.SupplierCompanyId; });
            this.SelectedJob = this.JobList.filter(function (item) { return item.Id == model.JobId; });
            this.SelectedFuelGroup = this.FuelGroupList.filter(function (item) { return item.Id == model.ProductType; });
            //this.Tiers = model.OrderList;
            this.Orders = this.Orders.filter(function (item) {
                return model.OrderList.filter(function (t) { return t.OrderId == item.OrderId; }).length == 0;
            });
        }
    }
    GetCustomerList() {
        this.orderService.getCustomerList().subscribe(data => {
            this.CustomerList = data;
            if (data.length > 0) {
                this.SelectedCustomer = [data[0]];
                this.TermPricingForm.get('BuyerCompanyId').patchValue(data[0].Id);
                this.OnCustomerSelect(data[0]);
            }
        });
    }
    GetSupplierList() {
        this.orderService.getSupplierList().subscribe(data => {
            this.SupplierList = data;
            if (data.length > 0) {
                this.SelectedSupplier = [data[0]];
                this.TermPricingForm.get('SupplierCompanyId').patchValue(data[0].Id);
                this.OnSupplierSelect(data[0]);
            }
        });
    }
    OnCustomerSelect(customer) {
        this.TermPricingForm.get('BuyerCompanyId').patchValue(customer.Id);
        var productType = this.TermPricingForm.get('ProductType').value;
        var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
        var formOrderList = this.TermPricingForm.get('OrderList');
        if (customer != null && customer != undefined && customer.Id > 0 && productType > 0 && selectedsupplier > 0) {
            this.orderService.getJobListByFuelGroupType(customer.Id, selectedsupplier, productType).subscribe(data => {
                this.JobList = data;
                if (data.length > 0) {
                    this.SelectedJob = [data[0]];
                    this.TermPricingForm.get('JobId').patchValue(data[0].Id);
                    this.OnJobSelect(data[0]);
                }
                else {
                    this.SelectedJob = [];
                    formOrderList.clear();
                    this.OrderList = [];
                    this.addNewTier(this.DefaultTier);
                    this.TermPricingForm.get('JobId').patchValue(null);
                    this.Orders = [];
                }
            });
        }
        else {
            this.JobList = [];
            this.SelectedJob = [];
            formOrderList.clear();
            this.OrderList = [];
            this.addNewTier(this.DefaultTier);
            this.TermPricingForm.get('JobId').patchValue(null);
            this.Orders = [];
        }
    }
    OnCustomerDeSelect(customer) {
        this.TermPricingForm.get('BuyerCompanyId').patchValue(null);
        this.JobList = [];
        this.SelectedJob = [];
        this.Orders = [];
        var formOrderList = this.TermPricingForm.get('OrderList');
        formOrderList.clear();
        this.OrderList = [];
        this.addNewTier(this.DefaultTier);
        this.TermPricingForm.get('JobId').patchValue(null);
    }
    OnSupplierSelect(supplier) {
        this.TermPricingForm.get('SupplierCompanyId').patchValue(supplier.Id);
        var productType = this.TermPricingForm.get('ProductType').value;
        var customer = this.TermPricingForm.get('BuyerCompanyId').value;
        var formOrderList = this.TermPricingForm.get('OrderList');
        if (supplier != null && supplier != undefined && supplier.Id > 0 && productType > 0 && customer > 0) {
            this.orderService.getJobListByFuelGroupType(customer, supplier.Id, productType).subscribe(data => {
                this.JobList = data;
                if (data.length > 0) {
                    this.SelectedJob = [data[0]];
                    this.TermPricingForm.get('JobId').patchValue(data[0].Id);
                    this.OnJobSelect(data[0]);
                }
                else {
                    this.SelectedJob = [];
                    this.Orders = [];
                    formOrderList.clear();
                    this.OrderList = [];
                    this.addNewTier(this.DefaultTier);
                    this.TermPricingForm.get('JobId').patchValue(null);
                }
            });
        }
        else {
            this.JobList = [];
            this.SelectedJob = [];
            this.Orders = [];
            formOrderList.clear();
            this.OrderList = [];
            this.addNewTier(this.DefaultTier);
            this.TermPricingForm.get('JobId').patchValue(null);
        }
    }
    OnSupplierDeSelect(supplier) {
        this.TermPricingForm.get('SupplierCompanyId').patchValue(null);
        this.JobList = [];
        this.SelectedJob = [];
        this.Orders = [];
        var formOrderList = this.TermPricingForm.get('OrderList');
        formOrderList.clear();
        this.OrderList = [];
        this.addNewTier(this.DefaultTier);
        this.TermPricingForm.get('JobId').patchValue(null);
    }
    OnFuelGroupSelect(fuelGroup) {
        this.TermPricingForm.get('ProductType').patchValue(fuelGroup.Id);
        var customer = this.TermPricingForm.get('BuyerCompanyId').value;
        var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
        var formOrderList = this.TermPricingForm.get('OrderList');
        if (customer != null && customer != undefined && customer > 0 && fuelGroup.Id > 0 && selectedsupplier > 0) {
            this.orderService.getJobListByFuelGroupType(customer, selectedsupplier, fuelGroup.Id).subscribe(data => {
                this.JobList = data;
                if (data.length > 0) {
                    this.SelectedJob = [data[0]];
                    this.TermPricingForm.get('JobId').patchValue(data[0].Id);
                    this.OnJobSelect(data[0]);
                }
                else {
                    this.SelectedJob = [];
                    this.Orders = [];
                    formOrderList.clear();
                    this.OrderList = [];
                    this.addNewTier(this.DefaultTier);
                    this.TermPricingForm.get('JobId').patchValue(null);
                }
            });
        }
        else {
            this.JobList = [];
            this.SelectedJob = [];
            this.Orders = [];
            formOrderList.clear();
            this.OrderList = [];
            this.addNewTier(this.DefaultTier);
            this.TermPricingForm.get('JobId').patchValue(null);
        }
    }
    OnFuelGroupDeSelect(fuelGroup) {
        this.TermPricingForm.get('ProductType').patchValue(null);
        this.JobList = [];
        this.SelectedJob = [];
        this.Orders = [];
        var formOrderList = this.TermPricingForm.get('OrderList');
        formOrderList.clear();
        this.OrderList = [];
        this.addNewTier(this.DefaultTier);
        this.TermPricingForm.get('JobId').patchValue(null);
    }
    OnJobSelect(job) {
        this.TermPricingForm.get('JobId').patchValue(job.Id);
        var productType = this.TermPricingForm.get('ProductType').value;
        var selectedcustomer = this.TermPricingForm.get('BuyerCompanyId').value;
        var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
        var formOrderList = this.TermPricingForm.get('OrderList');
        formOrderList.clear();
        this.OrderList = [];
        this.addNewTier(this.DefaultTier);
        this.orderService.getOrderList(selectedcustomer, selectedsupplier, productType, job.Id).subscribe(data => { this.Orders = data; });
    }
    OnJobDeSelect(job) {
        this.TermPricingForm.get('JobId').patchValue(null);
        this.Orders = [];
        var formOrderList = this.TermPricingForm.get('OrderList');
        formOrderList.clear();
        this.OrderList = [];
        this.addNewTier(this.DefaultTier);
    }
    onSubmit() {
        if (this.TermPricingForm.valid && this.validateTierControls()) {
            this.orderService.postCreateGroup(this.TermPricingForm.value)
                .subscribe((data) => {
                if (data != null && data.StatusCode == 0) {
                    _declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                    window.location.href = this.CurrentUser.IsBuyerCompany ? "/Buyer/OrderGroup/View" : "/Supplier/OrderGroup/View";
                    //this.TermPricingForm.reset();
                }
                else {
                    this.IsLoading = false;
                    _declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            this.IsLoading = false;
            this.TermPricingForm.markAllAsTouched();
            let invalidControls = this.findInvalidControlsRecursive(this.TermPricingForm);
        }
    }
    findInvalidControlsRecursive(formToInvestigate) {
        var invalidControls = [];
        let recursiveFunc = (form) => {
            Object.keys(form.controls).forEach(field => {
                const control = form.get(field);
                if (control.invalid)
                    invalidControls.push(field);
                if (control instanceof _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]) {
                    recursiveFunc(control);
                }
                else if (control instanceof _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArray"]) {
                    recursiveFunc(control);
                }
            });
        };
        recursiveFunc(formToInvestigate);
        return invalidControls;
    }
    addNewTier(model) {
        if (model == null || model == undefined) {
            model = new _order_models_OrderDetail__WEBPACK_IMPORTED_MODULE_2__["OrderList"]();
        }
        if (model.OrderId == null || model.OrderId == undefined) {
            if (this.OrderList.length == 0) {
                model.MinVolume = 0;
            }
            else {
                var formOrderList = this.TermPricingForm.get('OrderList');
                var lastMaxValue = formOrderList.controls[formOrderList.length - 1].get('MaxVolume').value;
                if (lastMaxValue != null && lastMaxValue != undefined && parseInt(lastMaxValue) > 0) {
                    model.MinVolume = parseInt(lastMaxValue) + 1;
                }
            }
        }
        var tierForm = this.buildTier(model);
        this.OrderList.push({ Order: model.Order, OrderId: model.OrderId, MinVolume: model.MinVolume, MaxVolume: model.MaxVolume });
        var prodArray = this.TermPricingForm.get('OrderList');
        if (prodArray != undefined && prodArray != null) {
            prodArray.push(tierForm);
        }
    }
    buildTier(model) {
        return this.fb.group({
            Order: this.fb.group({
                OrderId: this.fb.control(model.Order.OrderId, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
                TfxPoNumber: this.fb.control(model.Order.TfxPoNumber),
                FuelType: this.fb.control(model.Order.FuelType),
                Quantity: this.fb.control(model.Order.Quantity),
                DisplayPrice: this.fb.control(model.Order.DisplayPrice)
            }),
            OrderId: this.fb.control(model.Order.OrderId),
            MinVolume: this.fb.control(model.MinVolume, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(_app_constants__WEBPACK_IMPORTED_MODULE_5__["RegExConstants"].Integer)]),
            MaxVolume: this.fb.control(model.MaxVolume, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(_app_constants__WEBPACK_IMPORTED_MODULE_5__["RegExConstants"].Integer)])
        });
    }
    removeTier(tier, index) {
        var deletedOrder = this.OrderList[index].Order;
        if (deletedOrder != null && deletedOrder != undefined && this.OrderList[index].OrderId > 0) {
            this.Orders.push(this.OrderList[index].Order);
        }
        var _tiers = this.TermPricingForm.get('OrderList');
        _tiers.removeAt(index);
        this.OrderList.splice(index, 1);
    }
    onItemDropTier(index, event) {
        var tierList = this.TermPricingForm.get('OrderList');
        var existingOrderinTier = this.OrderList[index].Order;
        var indexOfSameOrderinAnotherTier = this.OrderList.findIndex(t => t.OrderId == event.dragData.OrderId);
        if (indexOfSameOrderinAnotherTier >= 0) {
            this.OrderList[indexOfSameOrderinAnotherTier].Order = new _order_models_OrderDetail__WEBPACK_IMPORTED_MODULE_2__["OrderDetailModel"]();
            this.OrderList[indexOfSameOrderinAnotherTier].OrderId = null;
            tierList.controls[indexOfSameOrderinAnotherTier].get('Order').patchValue(this.emptyOrder);
            tierList.controls[indexOfSameOrderinAnotherTier].get('OrderId').patchValue(null);
        }
        this.OrderList[index].Order = event.dragData;
        this.OrderList[index].OrderId = event.dragData.OrderId;
        tierList.controls[index].get('Order').patchValue(event.dragData);
        tierList.controls[index].get('OrderId').patchValue(event.dragData.OrderId);
        this.Orders = this.Orders.filter(function (element) { return element.OrderId != event.dragData.OrderId; });
        if (existingOrderinTier != null && existingOrderinTier != undefined && existingOrderinTier.OrderId > 0) {
            this.Orders.push(existingOrderinTier);
        }
    }
    onItemDropOrder(event) {
        if (this.Orders.findIndex(t => t.OrderId == event.dragData.OrderId) < 0) {
            this.Orders.push(event.dragData);
        }
        var index = this.OrderList.findIndex(t => t.OrderId == event.dragData.OrderId);
        if (index >= 0) {
            this.OrderList[index].Order = new _order_models_OrderDetail__WEBPACK_IMPORTED_MODULE_2__["OrderDetailModel"]();
            this.OrderList[index].OrderId = null;
            var tierList = this.TermPricingForm.get('OrderList');
            tierList.controls[index].get('Order').patchValue(this.emptyOrder);
            tierList.controls[index].get('OrderId').patchValue(null);
        }
    }
    isInvalid(drop, name) {
        return drop.get(name).invalid &&
            (drop.get(name).dirty || drop.get(name).touched);
    }
    isRequired(drop, name) {
        return drop.get(name).errors.required &&
            (drop.get(name).dirty || drop.get(name).touched);
    }
    isMin(drop, name) {
        return drop.get(name).errors.min;
    }
    getNextRenewalDate(date) {
        this.NextRenewalDate = moment__WEBPACK_IMPORTED_MODULE_4__(date).add(1, 'months').startOf('month').format('MM/DD/YYYY');
    }
    comparisonValidator(group, index, controlName) {
        const minVolume = group.get('MinVolume');
        const maxVolume = group.get('MaxVolume');
        if (minVolume.value !== null && maxVolume.value !== null && parseInt(minVolume.value) >= parseInt(maxVolume.value)) {
            group.get(controlName).setErrors({ notEquivalent: true });
        }
    }
    validateTierControls() {
        var tierList = this.TermPricingForm.get('OrderList');
        var maxVolumeCtrl, minVolumeCtrl;
        var minVolume, maxVolume;
        for (var i = 0; i < tierList.length; i++) {
            maxVolumeCtrl = tierList.controls[i].get('MaxVolume');
            minVolumeCtrl = tierList.controls[i].get('MinVolume');
            if (minVolume != undefined && minVolume != null) {
                if (parseInt(maxVolume) + 1 != parseInt(minVolumeCtrl.value)) {
                    minVolumeCtrl.setErrors({ notEquivalent: true });
                }
            }
            maxVolume = maxVolumeCtrl.value;
            minVolume = minVolumeCtrl.value;
        }
        return this.TermPricingForm.valid;
    }
}
TermPricingContractComponent.ɵfac = function TermPricingContractComponent_Factory(t) { return new (t || TermPricingContractComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_order_service__WEBPACK_IMPORTED_MODULE_6__["OrderService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_shared_service__WEBPACK_IMPORTED_MODULE_7__["SharedService"])); };
TermPricingContractComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: TermPricingContractComponent, selectors: [["app-term-pricing-contract"]], decls: 59, vars: 18, consts: [[1, "row"], ["class", "col-sm-3", 4, "ngIf"], [1, "col-sm-2"], [1, "form-group"], [3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], [1, "col-sm-4"], [3, "formGroup", "ngSubmit"], ["class", "pa bg-white z-index5 loading-wrapper left0 top0", 4, "ngIf"], ["type", "hidden", "formControlName", "GroupType", "value", "2"], [1, "col-sm-3"], ["formArrayName", "OrderList", 1, "clearboth"], [1, "group-maxheight"], [4, "ngFor", "ngForOf"], [1, "col-sm-12"], [1, "mt20", 3, "click"], [1, "fa", "fa-plus"], [1, "col-sm-6"], ["droppable", "", 1, "group-height", 3, "onDrop"], ["class", "col-sm-6", 4, "ngFor", "ngForOf"], ["type", "text", "placeholder", "Renewal Frequency", "formControlName", "RenewalPeriod", "value", "Monthly", "readonly", "", 1, "form-control"], ["type", "hidden", "formControlName", "RenewalFrequency", "value", "1"], ["type", "text", "placeholder", "Maximum Renewal Count", "formControlName", "RenewalCount", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], ["type", "text", "formControlName", "StartDate", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "placeholder", "Customer PO#", "formControlName", "GroupPoNumber", 1, "form-control"], [1, "col-sm-12", "text-right"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", "btn-lg"], ["type", "submit", "value", "Create", 1, "btn", "btn-primary", "btn-lg"], [3, "ngModel", "data", "settings", "ngModelChange", "onSelect", "onDeSelect"], [1, "pa", "bg-white", "z-index5", "loading-wrapper", "left0", "top0"], [1, "spinner-dashboard", "pa"], [1, "pa10", "radius-5", "mb10", "border-dash-dark", 3, "formGroupName", "ngClass"], [1, "mt0", "mb0", "pull-left"], [1, "color-maroon", "ml10", "mt2", "pull-left", 3, "click"], [1, "fa", "fa-trash-alt", "pull-right"], [1, "form-group", "mb5"], ["type", "text", "formControlName", "MinVolume", "placeholder", "Min", 1, "form-control", 3, "change"], ["class", "help-block color-maroon", 4, "ngIf"], ["type", "text", "formControlName", "MaxVolume", "placeholder", "Max", 1, "form-control", 3, "change"], [1, "row", 3, "formGroup"], ["draggable", "", "droppable", "", 1, "well", 3, "dragData", "onDrop"], ["class", "text-center", 4, "ngIf"], [4, "ngIf"], [1, "help-block", "color-maroon"], [1, "text-center"], [1, "far", "fa-hand-rock", "fs25"], ["class", "fs16 mb0 pa0 color-orange", 4, "ngIf"], [1, "fs16", "mb0", "pa0", "color-orange"], [1, "mt0", "mb0"], ["draggable", "", 1, "well", 3, "dragData"], [1, "color-maroon"]], template: function TermPricingContractComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, TermPricingContractComponent_div_1_Template, 5, 3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TermPricingContractComponent_div_2_Template, 5, 3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Fuel Group Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function TermPricingContractComponent_Template_ng_multiselect_dropdown_ngModelChange_7_listener($event) { return ctx.SelectedFuelGroup = $event; })("onSelect", function TermPricingContractComponent_Template_ng_multiselect_dropdown_onSelect_7_listener($event) { return ctx.OnFuelGroupSelect($event); })("onDeSelect", function TermPricingContractComponent_Template_ng_multiselect_dropdown_onDeSelect_7_listener($event) { return ctx.OnFuelGroupDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Job");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function TermPricingContractComponent_Template_ng_multiselect_dropdown_ngModelChange_12_listener($event) { return ctx.SelectedJob = $event; })("onSelect", function TermPricingContractComponent_Template_ng_multiselect_dropdown_onSelect_12_listener($event) { return ctx.OnJobSelect($event); })("onDeSelect", function TermPricingContractComponent_Template_ng_multiselect_dropdown_onDeSelect_12_listener($event) { return ctx.OnJobDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "form", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function TermPricingContractComponent_Template_form_ngSubmit_13_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, TermPricingContractComponent_div_14_Template, 2, 0, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, TermPricingContractComponent_ng_container_20_Template, 23, 12, "ng-container", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "a", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TermPricingContractComponent_Template_a_click_23_listener() { return ctx.addNewTier(null); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](24, "i", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, " Add Tier");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDrop", function TermPricingContractComponent_Template_div_onDrop_27_listener($event) { return ctx.onItemDropOrder($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](29, TermPricingContractComponent_div_29_Template, 13, 5, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Renewal Period");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](36, "input", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](37, "input", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Maximum Renewal Count");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](41, "input", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](42, TermPricingContractComponent_span_42_Template, 3, 2, "span", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Start Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "input", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function TermPricingContractComponent_Template_input_onDateChange_47_listener($event) { ctx.TermPricingForm.get("StartDate").setValue($event); return ctx.getNextRenewalDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "span");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](49);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](50, TermPricingContractComponent_span_50_Template, 2, 1, "span", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "Customer PO#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](54, "input", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](57, "input", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](58, "input", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.CurrentUser.IsSupplierCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.CurrentUser.IsBuyerCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedFuelGroup)("settings", ctx.DdlSettings)("data", ctx.FuelGroupList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedJob)("settings", ctx.DdlSettings)("data", ctx.JobList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.TermPricingForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.TermPricingForm.get("OrderList")["controls"]);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.Orders);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.TermPricingForm, "RenewalCount"));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Next Renewal Date: ", ctx.NextRenewalDate, "");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isInvalid(ctx.TermPricingForm, "StartDate"));
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_10__["Droppable"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgClass"], ng_drag_drop__WEBPACK_IMPORTED_MODULE_10__["Draggable"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL29yZGVyL3Rlcm0tcHJpY2luZy1jb250cmFjdC5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TermPricingContractComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-term-pricing-contract',
                templateUrl: './term-pricing-contract.component.html',
                styleUrls: ['./term-pricing-contract.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _services_order_service__WEBPACK_IMPORTED_MODULE_6__["OrderService"] }, { type: _services_shared_service__WEBPACK_IMPORTED_MODULE_7__["SharedService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/order/view-order-group/view-order-group.component.ts":
/*!**********************************************************************!*\
  !*** ./src/app/order/view-order-group/view-order-group.component.ts ***!
  \**********************************************************************/
/*! exports provided: ViewOrderGroupComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ViewOrderGroupComponent", function() { return ViewOrderGroupComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../create-blend-group/create-blend-group.component */ "./src/app/order/create-blend-group/create-blend-group.component.ts");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _models_ViewOrderGroupDdlModel__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../models/ViewOrderGroupDdlModel */ "./src/app/order/models/ViewOrderGroupDdlModel.ts");
/* harmony import */ var _create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../create-same-dest-group/create-same-dest-group.component */ "./src/app/order/create-same-dest-group/create-same-dest-group.component.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../services/viewordergroup.service */ "./src/app/order/services/viewordergroup.service.ts");
/* harmony import */ var _services_shared_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../services/shared.service */ "./src/app/order/services/shared.service.ts");
/* harmony import */ var _services_order_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../services/order.service */ "./src/app/order/services/order.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _term_pricing_contract_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../term-pricing-contract.component */ "./src/app/order/term-pricing-contract.component.ts");
















function ViewOrderGroupComponent_div_18_Template(rf, ctx) { if (rf & 1) {
    const _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "app-create-blend-group", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSubmitGroupData", function ViewOrderGroupComponent_div_18_Template_app_create_blend_group_onSubmitGroupData_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r14); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r13.onOrderGroupFilterSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_19_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "app-create-same-dest-group", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSubmitGroupData", function ViewOrderGroupComponent_div_19_Template_app_create_same_dest_group_onSubmitGroupData_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r15.onOrderGroupFilterSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_20_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-term-pricing-contract");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_option_35_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grpType_r17 = ctx.$implicit;
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngValue", ctx_r4.SelectedGroupType)("value", grpType_r17.Id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grpType_r17.Name);
} }
function ViewOrderGroupComponent_label_38_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Customer");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_label_39_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Supplier");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_41_Template(rf, ctx) { if (rf & 1) {
    const _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Location/Site Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ViewOrderGroupComponent_div_41_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r18.SelectedJob = $event; })("onSelect", function ViewOrderGroupComponent_div_41_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19); const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r20.OnJobSelect($event); })("onDeSelect", function ViewOrderGroupComponent_div_41_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r19); const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r21.OnJobDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r7.SelectedJob)("settings", ctx_r7.JobDdlSettings)("data", ctx_r7.JobList);
} }
function ViewOrderGroupComponent_div_42_Template(rf, ctx) { if (rf & 1) {
    const _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Fuel Group");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ViewOrderGroupComponent_div_42_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r23); const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r22.SelectedFuelGroup = $event; })("onSelect", function ViewOrderGroupComponent_div_42_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r23); const ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r24.OnFuelGroupSelect($event); })("onDeSelect", function ViewOrderGroupComponent_div_42_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r23); const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r25.OnFuelGroupDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r8.SelectedFuelGroup)("settings", ctx_r8.FuelGroupDdlSettings)("data", ctx_r8.FuelGroupList);
} }
function ViewOrderGroupComponent_div_43_Template(rf, ctx) { if (rf & 1) {
    const _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "State");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ViewOrderGroupComponent_div_43_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r26.SelectedState = $event; })("onSelect", function ViewOrderGroupComponent_div_43_Template_ng_multiselect_dropdown_onSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r28.OnStateSelect($event); })("onDeSelect", function ViewOrderGroupComponent_div_43_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r29.OnStateDeSelect($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r9.SelectedState)("settings", ctx_r9.StateDdlSettings)("data", ctx_r9.StateList);
} }
function ViewOrderGroupComponent_div_57_a_12_Template(rf, ctx) { if (rf & 1) {
    const _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_div_57_a_12_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r40); const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r38.OpenEditSlider(grp_r30.OrderGroupId, grp_r30.GroupType); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_57_a_13_Template(rf, ctx) { if (rf & 1) {
    const _r43 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_div_57_a_13_Template_a_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r43); const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit; const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r41.SetOrderGroupIdToDelete(grp_r30.OrderGroupId); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_57_div_14_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grp_r30.JobName);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grp_r30.JobAddress);
} }
function ViewOrderGroupComponent_div_57_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, ViewOrderGroupComponent_div_57_div_14_div_1_Template, 7, 2, "div", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.JobAddress != null);
} }
function ViewOrderGroupComponent_div_57_div_15_span_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Renewal - ", grp_r30.RenewalFrequency, "");
} }
function ViewOrderGroupComponent_div_57_div_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, ViewOrderGroupComponent_div_57_div_15_span_4_Template, 2, 1, "span", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grp_r30.DisplayProductType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.RenewalFrequency != "" || grp_r30.RenewalFrequency != null);
} }
function ViewOrderGroupComponent_div_57_div_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grp_r30.DisplayBlendedGroupWeightedPPG);
} }
function ViewOrderGroupComponent_div_57_div_18_div_2_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"]("", drop_r51.DroppedGallons, " ", drop_r51.UoM, "");
} }
function ViewOrderGroupComponent_div_57_div_18_div_2_div_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    const j_r52 = ctx_r61.index;
    const drop_r51 = ctx_r61.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate4"]("Tier ", j_r52 + 1, " - ", drop_r51.MinVolume, " to ", drop_r51.MaxVolume, " ", drop_r51.UoM, "");
} }
function ViewOrderGroupComponent_div_57_div_18_div_2_div_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Not Specified");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_57_div_18_div_2_span_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", drop_r51.DropPercentage, "%");
} }
function ViewOrderGroupComponent_div_57_div_18_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ViewOrderGroupComponent_div_57_div_18_div_2_div_5_Template, 4, 2, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ViewOrderGroupComponent_div_57_div_18_div_2_div_6_Template, 4, 4, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, ViewOrderGroupComponent_div_57_div_18_div_2_div_7_Template, 4, 0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, ViewOrderGroupComponent_div_57_div_18_div_2_span_11_Template, 2, 1, "span", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](drop_r51.FuelType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](drop_r51.TfxPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType != 3 && drop_r51.DropPercentage != "--");
} }
function ViewOrderGroupComponent_div_57_div_18_div_3_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"]("", drop_r51.TfxPoNumber, " - ", drop_r51.DroppedGallons, " ", drop_r51.UoM, "");
} }
function ViewOrderGroupComponent_div_57_div_18_div_3_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", drop_r51.TfxPoNumber, " - Not Specified");
} }
function ViewOrderGroupComponent_div_57_div_18_div_3_span_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", drop_r51.DropPercentage, "%");
} }
function ViewOrderGroupComponent_div_57_div_18_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ViewOrderGroupComponent_div_57_div_18_div_3_div_2_Template, 4, 3, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, ViewOrderGroupComponent_div_57_div_18_div_3_div_3_Template, 4, 1, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, ViewOrderGroupComponent_div_57_div_18_div_3_span_11_Template, 2, 1, "span", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    const drop_r51 = ctx_r70.$implicit;
    const j_r52 = ctx_r70.index;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate4"]("Tier ", j_r52 + 1, " - ", drop_r51.MinVolume, " to ", drop_r51.MaxVolume, " ", drop_r51.UoM, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](drop_r51.DisplayPPG);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType != 3 && drop_r51.DropPercentage != "--");
} }
function ViewOrderGroupComponent_div_57_div_18_div_4_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"]("", drop_r51.DroppedGallons, " ", drop_r51.UoM, "");
} }
function ViewOrderGroupComponent_div_57_div_18_div_4_div_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Not Specified");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function ViewOrderGroupComponent_div_57_div_18_div_4_span_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2).$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", drop_r51.DropPercentage, "%");
} }
function ViewOrderGroupComponent_div_57_div_18_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ViewOrderGroupComponent_div_57_div_18_div_4_div_5_Template, 4, 2, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ViewOrderGroupComponent_div_57_div_18_div_4_div_6_Template, 4, 0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "br");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, ViewOrderGroupComponent_div_57_div_18_div_4_span_13_Template, 2, 1, "span", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const drop_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](drop_r51.FuelType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType == 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](drop_r51.TfxPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Blended Ratio - ", drop_r51.BlendRatioPercentage, "%");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", drop_r51.QuantityType != 3 && drop_r51.DropPercentage != "--");
} }
function ViewOrderGroupComponent_div_57_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ViewOrderGroupComponent_div_57_div_18_div_2_Template, 12, 6, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, ViewOrderGroupComponent_div_57_div_18_div_3_Template, 12, 8, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, ViewOrderGroupComponent_div_57_div_18_div_4_Template, 14, 6, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "div", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 3);
} }
function ViewOrderGroupComponent_div_57_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 46);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 47);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 48);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 49);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 50);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 51);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Customer PO#");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "h2", 52);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 53);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, ViewOrderGroupComponent_div_57_a_12_Template, 2, 0, "a", 54);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, ViewOrderGroupComponent_div_57_a_13_Template, 2, 0, "a", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, ViewOrderGroupComponent_div_57_div_14_Template, 2, 1, "div", 56);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, ViewOrderGroupComponent_div_57_div_15_Template, 6, 2, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, ViewOrderGroupComponent_div_57_div_16_Template, 4, 1, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 57);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, ViewOrderGroupComponent_div_57_div_18_Template, 6, 3, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const grp_r30 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](grp_r30.CustomerPoNumber);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.IsEditOrDeleteAllowed && grp_r30.CanCurrentUserEditOrDeleteGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.IsEditOrDeleteAllowed && grp_r30.CanCurrentUserEditOrDeleteGroup);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", grp_r30.GroupType == 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", grp_r30.OrderDrops);
} }
function ViewOrderGroupComponent_div_58_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
const _c0 = function () { return []; };
class ViewOrderGroupComponent {
    constructor(fb, viewOrderGroupService, sharedService, orderService) {
        this.fb = fb;
        this.viewOrderGroupService = viewOrderGroupService;
        this.sharedService = sharedService;
        this.orderService = orderService;
        this.SelectedButton = "";
        this.ModalText = "";
        this.ModalTextData = ['', 'Create Same Destination Group', 'Term Pricing Contract', 'Create Blend Group'];
        this.ModalEditTextData = ['', 'Edit Same Destination Group', 'Edit Pricing Contract', 'Edit Blend Group'];
        this.IsLoading = false;
        this.ShowCount = 0;
        this.TotalGroupCount = 0;
        //child 
        this.model = new _models_ViewOrderGroupDdlModel__WEBPACK_IMPORTED_MODULE_3__["ViewOrderGroupDdlModel"]();
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        this.IsMultiProduct = true;
        this.IsTier = false;
        this.IsBlend = false;
        // public SearchText: string;
        this.GroupTypeList = [];
        this.CompanyList = [];
        this.JobList = [];
        this.FuelGroupList = [];
        this.StateList = [];
        this.GroupTypeDdlSettings = {};
        this.CompanyDdlSettings = {};
        this.JobDdlSettings = {};
        this.FuelGroupDdlSettings = {};
        this.StateDdlSettings = {};
        this.SelectedOrderGroupIdToDelete = 0;
    }
    ngOnInit() {
        this.model.GroupTypeId = 0;
        this.viewOrderGroupForm = this.fb.group({
            OrderGroupDetails: this.fb.control(''),
            GroupType: this.fb.control(null),
            Customer: this.fb.control(null),
            Job: this.fb.control(null),
            ProductCategory: this.fb.control(null),
            State: this.fb.control(null),
            GroupTypeId: this.fb.control(0),
            CompanyId: this.fb.control(null),
            JobId: this.fb.control(null),
            FuelGroupId: this.fb.control(null),
            StateId: this.fb.control(null),
            SearchText: this.fb.control(null),
        });
        this.GroupTypeDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.CompanyDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.JobDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.FuelGroupDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.StateDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.fillDDLByGroup();
    }
    fillDDLByGroup() {
        this.IsLoading = true;
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
            this.GroupTypeList = data.GroupTypes;
            this.CompanyList = data.Companies;
            this.FuelGroupList = data.ProductCategories;
            this.StateList = data.States;
            this.IsSupplierCompany = data.IsSupplierCompany;
            this.onOrderGroupFilterSubmit();
        });
    }
    //onViewOrderGroupChildResponse(response: ViewOrderGroupModel) {
    //	//this.model = response;
    //	this.groups = response.OrderGroupDetails as OrderGroupDetailModel[];
    //	console.log(response);
    //}
    ButtonPressed(_SelectedButton) {
        if (this.BlendComponent != undefined) {
            this.BlendComponent.clearBlendGroupForm();
        }
        if (this.SameDestComponent != undefined) {
            this.SameDestComponent.clearDestGroupForm();
        }
        this.SelectedButton = src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__["OrderGroupType"][_SelectedButton];
        this.ModalText = this.ModalTextData[_SelectedButton];
        this.sharedService.setGroupId(0);
    }
    showHideControlsByGroupType(groupTypeId) {
        if (groupTypeId == 2) {
            this.IsTier = true;
            this.IsMultiProduct = false;
            this.IsBlend = false;
        }
        else if (groupTypeId == 3) {
            this.IsBlend = true;
            this.IsTier = false;
            this.IsMultiProduct = false;
        }
        else {
            this.IsMultiProduct = true;
            this.IsTier = false;
            this.IsBlend = false;
        }
    }
    OnGroupTypeSelect(groupType) {
        this.IsLoading = true;
        this.resetForm();
        this.model.GroupTypeId = groupType.target.value;
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
            this.CompanyList = data.Companies;
            this.FuelGroupList = data.ProductCategories;
            this.StateList = data.States;
            this.onOrderGroupFilterSubmit();
        });
    }
    onOrderGroupFilterSubmit() {
        this.IsLoading = true;
        this.viewOrderGroupService.getOrderGroupDetails(this.model)
            .subscribe(data => {
            this.groups = data.OrderGroupDetails;
            this.TotalGroupCount = data.TotalGroupCount;
            this.ShowCount = data.ShowCount;
            this.IsLoading = false;
        });
    }
    resetForm() {
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        //this.onViewOrderGroupResponse.emit([]);
        this.groups = [];
    }
    onSearch(text) {
        if (text != null && text != '' && text.length >= 3) {
            this.IsLoading = true;
            this.model.SearchText = text;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                this.IsLoading = false;
                this.groups = data.OrderGroupDetails;
                this.TotalGroupCount = data.TotalGroupCount;
                this.ShowCount = data.ShowCount;
                //this.onViewOrderGroupResponse.emit(data);
            });
        }
        else if (text.length == 0) {
            this.IsLoading = true;
            this.model.SearchText = null;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                this.IsLoading = false;
                this.groups = data.OrderGroupDetails;
                this.TotalGroupCount = data.TotalGroupCount;
                this.ShowCount = data.ShowCount;
                // this.onViewOrderGroupResponse.emit(data);
            });
        }
    }
    OnCompanySelect(company) {
        this.model.CompanyId = company.Id;
        this.JobList = [];
        this.orderService.getCommonJobList(company.Id).subscribe(data => this.JobList = data);
    }
    OnCompanyDeSelect(company) {
        this.JobList = [];
        this.model.CompanyId = null;
    }
    OnJobSelect(job) {
        this.model.JobId = job.Id;
    }
    OnJobDeSelect(groupType) {
        this.model.JobId = null;
    }
    OnFuelGroupSelect(fuelGroup) {
        this.model.ProductCategoryId = fuelGroup.Id;
    }
    OnFuelGroupDeSelect(fuelGroup) {
        this.model.ProductCategoryId = null;
    }
    OnStateSelect(state) {
        this.model.StateId = state.Id;
    }
    OnStateDeSelect(state) {
        this.model.StateId = null;
    }
    SetOrderGroupIdToDelete(orderGroupId) {
        this.SelectedOrderGroupIdToDelete = orderGroupId;
    }
    deleteOrderGroup() {
        if (this.SelectedOrderGroupIdToDelete == 0) {
            return;
        }
        this.IsLoading = true;
        this.viewOrderGroupService.deleteOrderGroup(this.SelectedOrderGroupIdToDelete)
            .subscribe(data => {
            if (data != null && data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                this.onOrderGroupFilterSubmit();
                this.IsLoading = false;
            }
            else {
                this.IsLoading = false;
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
        this.SelectedOrderGroupIdToDelete = 0;
    }
    OpenEditSlider(OrderGroupId, _SelectedButton) {
        if (this.BlendComponent != undefined) {
            this.BlendComponent.isEdit = true;
        }
        this.SelectedButton = src_app_app_enum__WEBPACK_IMPORTED_MODULE_5__["OrderGroupType"][_SelectedButton];
        this.ModalText = this.ModalEditTextData[_SelectedButton];
        this.sharedService.setGroupId(OrderGroupId);
    }
}
ViewOrderGroupComponent.ɵfac = function ViewOrderGroupComponent_Factory(t) { return new (t || ViewOrderGroupComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_7__["ViewOrderGroupService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_shared_service__WEBPACK_IMPORTED_MODULE_8__["SharedService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_order_service__WEBPACK_IMPORTED_MODULE_9__["OrderService"])); };
ViewOrderGroupComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: ViewOrderGroupComponent, selectors: [["app-view-order-group"]], viewQuery: function ViewOrderGroupComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_1__["CreateBlendGroupComponent"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__["CreateSameDestGroupComponent"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.BlendComponent = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.SameDestComponent = _t.first);
    } }, decls: 69, vars: 18, consts: [["type", "button", "onclick", "slidePanel('#create-group','90%')", "data-target", "create-group", 1, "btn", "btn-default", "mr25", 3, "click"], [1, "fas", "fa-plus", "mr5"], ["type", "button", "onclick", "slidePanel('#create-group','90%')", "data-target", "create-group", 1, "btn", "btn-default", 3, "click"], ["id", "create-group", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "fs21", "ml15"], [1, "modal-body"], [4, "ngIf"], ["viewOrderGroupForm", ""], [1, "container-fluid", "mt15"], [1, "row", "well"], [1, "col-sm-9"], [1, "row"], [1, "col-sm-2"], [1, "form-group"], [1, "form-control", 3, "change"], ["value", "0"], [3, "ngValue", "value", 4, "ngFor", "ngForOf"], [1, "col-sm-3"], ["name", "SelectedCompany", 3, "ngModel", "data", "settings", "ngModelChange", "onSelect", "onDeSelect"], ["class", "col-sm-3", 4, "ngIf"], ["class", "col-sm-2", 4, "ngIf"], ["type", "submit", "value", "Search", 1, "btn", "btn-primary", "btn-submit", "btn-lg", "no-disable", "btn-lg", "mt-4", 3, "click"], [1, "row", "mt-4"], [1, "btn", "btn-primary", 3, "click"], [1, "fas", "fa-search"], [1, "col-sm-10"], ["type", "text", "placeholder", "Search Group", 1, "form-control", "input-lg"], ["searchTxt", ""], [1, "pb15", "f-bold"], ["class", "col-sm-3", 4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "myModal", "role", "dialog", "data-backdrop", "static", "data-keyboard", "false", 1, "modal", "fade"], [1, "modal-dialog", 2, "width", "200px"], [1, "modal-content"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-danger", 3, "click"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", 3, "click"], [3, "onSubmitGroupData"], [3, "ngValue", "value"], ["name", "SelectedJob", 3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], ["name", "SelectedFuelGroup", 3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], ["name", "SelectedState", 3, "ngModel", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], [1, "well", "animated", "fadeInUp"], [1, "row", "mb10"], [1, "col-sm-12"], [1, "pa10", "mb0", "jumbotron", "group-cust", "radius-5"], [1, "col-sm-8"], [1, "fs12", "f-bold"], [1, "pt0", "mt0", "f-normal", "fs16"], [1, "col-sm-4", "text-right"], ["onclick", "slidePanel('#create-group','90%')", "data-target", "create-group", "class", "mr5 fs16", 3, "click", 4, "ngIf"], ["data-toggle", "modal", "data-target", "#myModal", 3, "click", 4, "ngIf"], ["class", "row", 4, "ngIf"], [1, "order-list", "pa10"], [4, "ngFor", "ngForOf"], ["onclick", "slidePanel('#create-group','90%')", "data-target", "create-group", 1, "mr5", "fs16", 3, "click"], [1, "fa", "fa-edit"], ["data-toggle", "modal", "data-target", "#myModal", 3, "click"], [1, "fa", "fa-trash-alt", "color-maroon"], ["class", "col-sm-12", 4, "ngIf"], [1, "fs12"], [1, "fs11"], [1, "fs14"], ["class", "fs14", 4, "ngIf"], [1, "fs13"], ["class", "row col-sm-12", 4, "ngIf"], [1, "border-b", "clearboth", "mt10", "mb10"], [1, "row", "col-sm-12"], [1, "fs11", "color-lightgrey"], [1, "col-sm-3", "text-right"], ["class", "badge", 4, "ngIf"], [1, "fs12", "color-lightgrey"], [1, "badge"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function ViewOrderGroupComponent_Template(rf, ctx) { if (rf & 1) {
        const _r78 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_1_listener() { return ctx.ButtonPressed("3"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Create Blend Group");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_4_listener() { return ctx.ButtonPressed("1"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Create Same Destination Group");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "button", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_7_listener() { return ctx.ButtonPressed("2"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Term Pricing Contract");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "a", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](14, "i", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "h2", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, ViewOrderGroupComponent_div_18_Template, 2, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, ViewOrderGroupComponent_div_19_Template, 2, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, ViewOrderGroupComponent_div_20_Template, 2, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "form", null, 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Grouping Purpose");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "select", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ViewOrderGroupComponent_Template_select_change_32_listener($event) { return ctx.OnGroupTypeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "option", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "All");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](35, ViewOrderGroupComponent_option_35_Template, 2, 3, "option", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](38, ViewOrderGroupComponent_label_38_Template, 2, 0, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](39, ViewOrderGroupComponent_label_39_Template, 2, 0, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "ng-multiselect-dropdown", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ViewOrderGroupComponent_Template_ng_multiselect_dropdown_ngModelChange_40_listener($event) { return ctx.SelectedCompany = $event; })("onSelect", function ViewOrderGroupComponent_Template_ng_multiselect_dropdown_onSelect_40_listener($event) { return ctx.OnCompanySelect($event); })("onDeSelect", function ViewOrderGroupComponent_Template_ng_multiselect_dropdown_onDeSelect_40_listener($event) { return ctx.OnCompanyDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](41, ViewOrderGroupComponent_div_41_Template, 5, 3, "div", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](42, ViewOrderGroupComponent_div_42_Template, 5, 3, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](43, ViewOrderGroupComponent_div_43_Template, 5, 3, "div", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "input", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_input_click_45_listener() { return ctx.onOrderGroupFilterSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "button", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_49_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r78); const _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](53); return ctx.onSearch(_r10.value); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](50, "i", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](52, "input", 30, 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](55);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](57, ViewOrderGroupComponent_div_57_Template, 19, 7, "div", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](58, ViewOrderGroupComponent_div_58_Template, 5, 0, "div", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "div", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "div", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "div", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](63, " Are you sure to delete? ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "div", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "button", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_65_listener() { return ctx.deleteOrderGroup(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](66, "Yes");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "button", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ViewOrderGroupComponent_Template_button_click_67_listener() { return ctx.SetOrderGroupIdToDelete(0); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](68, "No");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.ModalText);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.SelectedButton == "Blend");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.SelectedButton == "MultiProducts");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.SelectedButton == "Tier");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.GroupTypeList || _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction0"](17, _c0));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsSupplierCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsSupplierCompany);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedCompany)("data", ctx.CompanyList)("settings", ctx.CompanyDdlSettings);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsMultiProduct || ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsSupplierCompany && ctx.IsTier);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" Showing ", ctx.ShowCount, " of ", ctx.TotalGroupCount, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.groups);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgForm"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"], _create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_1__["CreateBlendGroupComponent"], _create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__["CreateSameDestGroupComponent"], _term_pricing_contract_component__WEBPACK_IMPORTED_MODULE_12__["TermPricingContractComponent"]], styles: [".no-shadow[_ngcontent-%COMP%] {\r\n    box-shadow: none;\r\n    border-radius: 5px;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvb3JkZXIvdmlldy1vcmRlci1ncm91cC92aWV3LW9yZGVyLWdyb3VwLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxnQkFBZ0I7SUFDaEIsa0JBQWtCO0FBQ3RCIiwiZmlsZSI6InNyYy9hcHAvb3JkZXIvdmlldy1vcmRlci1ncm91cC92aWV3LW9yZGVyLWdyb3VwLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIubm8tc2hhZG93IHtcclxuICAgIGJveC1zaGFkb3c6IG5vbmU7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1cHg7XHJcbn1cclxuIl19 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ViewOrderGroupComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-view-order-group',
                templateUrl: './view-order-group.component.html',
                styleUrls: ['./view-order-group.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormBuilder"] }, { type: _services_viewordergroup_service__WEBPACK_IMPORTED_MODULE_7__["ViewOrderGroupService"] }, { type: _services_shared_service__WEBPACK_IMPORTED_MODULE_8__["SharedService"] }, { type: _services_order_service__WEBPACK_IMPORTED_MODULE_9__["OrderService"] }]; }, { BlendComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
            args: [_create_blend_group_create_blend_group_component__WEBPACK_IMPORTED_MODULE_1__["CreateBlendGroupComponent"]]
        }], SameDestComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
            args: [_create_same_dest_group_create_same_dest_group_component__WEBPACK_IMPORTED_MODULE_4__["CreateSameDestGroupComponent"]]
        }] }); })();


/***/ })

}]);
//# sourceMappingURL=order-lazy-loading-order-module-es2015.js.map