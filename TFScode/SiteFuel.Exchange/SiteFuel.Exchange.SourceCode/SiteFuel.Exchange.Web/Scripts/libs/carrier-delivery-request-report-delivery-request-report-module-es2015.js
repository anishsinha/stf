(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["carrier-delivery-request-report-delivery-request-report-module"],{

/***/ "./src/app/carrier/delivery-request-report/delivery-request-report.component.ts":
/*!**************************************************************************************!*\
  !*** ./src/app/carrier/delivery-request-report/delivery-request-report.component.ts ***!
  \**************************************************************************************/
/*! exports provided: DeliveryRequestReportComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryRequestReportComponent", function() { return DeliveryRequestReportComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/carrier/models/DispatchSchedulerModels */ "./src/app/carrier/models/DispatchSchedulerModels.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/carrier/service/carrier.service */ "./src/app/carrier/service/carrier.service.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");










function DeliveryRequestReportComponent_tbody_33_ng_container_1_span_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function DeliveryRequestReportComponent_tbody_33_ng_container_1_span_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
const _c0 = function (a0, a1, a2, a3) { return { "bg_must_go": a0, "bg_should_go": a1, "bg_could_go": a2, "bg_noDlr_go": a3 }; };
function DeliveryRequestReportComponent_tbody_33_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "tr", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, DeliveryRequestReportComponent_tbody_33_ng_container_1_span_18_Template, 2, 0, "span", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, DeliveryRequestReportComponent_tbody_33_ng_container_1_span_19_Template, 2, 0, "span", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const dr_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction4"](11, _c0, dr_r3.Priority === 1, dr_r3.Priority === 2, dr_r3.Priority === 3, dr_r3.Priority === 4));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r3.RegionName);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r3.Location);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", dr_r3.LocationId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", dr_r3.CustomerName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r3.CustomerBrandID == null || dr_r3.CustomerBrandID == "" ? "--" : dr_r3.CustomerBrandID);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", dr_r3.PoNumber, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](dr_r3.ProductType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", dr_r3.RequestedQuantity + dr_r3.UoM, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r3.IsRecurringSchedule == true);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", dr_r3.IsAutoDR == true);
} }
function DeliveryRequestReportComponent_tbody_33_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DeliveryRequestReportComponent_tbody_33_ng_container_1_Template, 20, 16, "ng-container", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.DRReportsData);
} }
function DeliveryRequestReportComponent_div_34_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
class DeliveryRequestReportComponent {
    constructor(carrierServ) {
        this.carrierServ = carrierServ;
        //filter and dropdown variables
        this.LocationDdlSettings = {};
        this.RegionDdlSettings = {};
        this.Regions = [];
        this.Locations = [];
        this.SelectedRegions = [];
        this.SelectedLocations = [];
        // data binding variables
        this.DRReportsData = [];
        this.unchangedDRReportsData = [];
        this.IsLoading = false;
        // using these two values in deselect as ngModel not updating after deselect event
        this.SelectedRegionsForFilter = [];
        this.SelectedLocationsForFilter = [];
        this.dtDRGridOptions = {};
        this.dtDRReportTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
    }
    ngOnInit() {
        // this.ToDate = this.TodaysDate;
        // this.FromDate = moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');
        //this.ToDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
        //this.FromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');
        let exportColumns = { columns: ':visible' };
        let DRcolumnsDetails = [];
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
        DRcolumnsDetails = [
            { title: 'Location', name: 'Location', data: 'Location', "autoWidth": true },
            { title: 'Region', name: 'RegionName', data: 'RegionName', "autoWidth": true },
            { title: 'Customer', name: 'CustomerName', data: 'CustomerName', "autoWidth": true },
            { title: 'Customer BrandID', name: 'CustomerBrandID', data: 'Customer BrandID', "autoWidth": true },
            { title: 'Product', name: 'ProductType', data: 'ProductType', "autoWidth": true },
            { title: 'Requested Qty', name: 'RequestedQty', data: 'RequestedQuantity', "autoWidth": true },
            { title: 'LocationId', name: 'LocationId', data: 'LocationId', "autoWidth": true },
            { title: 'Order', name: 'Order', data: 'PoNumber', "autoWidth": true }
        ];
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
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Delivery Request Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Delivery Request Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ]
        };
        this.IsLoading = true;
        this.carrierServ.getDRReportFilters().subscribe((data) => {
            if (data != null && data != undefined) {
                let regionsIds = [];
                let locationIds = [];
                this.Regions = data.Regions;
                this.Locations = data.Locations;
                this.SelectedRegionId = '';
                this.Regions.forEach(res => { regionsIds.push(res.Id); });
                this.SelectedRegionId = regionsIds.join();
                this.SelectedLocationId = '';
                this.Locations.forEach(res => { locationIds.push(res.Id); });
                this.SelectedLocationId = locationIds.join();
                this.getDRReportGridData();
            }
        });
        this.IsLoading = false;
    }
    ngAfterViewInit() {
        this.dtDRReportTrigger.next();
    }
    ngOnDestroy() {
        this.dtDRReportTrigger.unsubscribe();
    }
    OnFilterSelect(event, filterType) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id); });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id); });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }
    onFilterSelectAll(event, filterType) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id); });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id); });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }
    onFilterDeselect(event, filterType) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id); });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id); });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }
    onFilterDeselectAll(event, filterType) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id); });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id); });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }
    datatableRerender() {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance) => {
                dtInstance.destroy();
                this.dtDRReportTrigger.next();
            });
        }
    }
    getDRReportGridData() {
        this.IsLoading = true;
        let inputData = new src_app_carrier_models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DRReportFilterInputViewModel"];
        inputData.RegionIds = this.SelectedRegionId;
        inputData.LocationIds = this.SelectedLocationId;
        inputData.FromDate = '';
        inputData.ToDate = '';
        this.carrierServ.getDRReportGridData(inputData).subscribe((data) => {
            this.unchangedDRReportsData = data;
            this.DRReportsData = data;
            this.IsLoading = false;
            this.datatableRerender();
        });
    }
    filterDRReportData(selectedRegions, selectedLocations) {
        this.IsLoading = true;
        let filteredData = [];
        let filteredDataByRegions = [];
        let filteredDataByLocations = [];
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
        }
        else {
            this.DRReportsData = filteredDataByRegions;
        }
        this.datatableRerender();
        this.IsLoading = false;
    }
}
DeliveryRequestReportComponent.ɵfac = function DeliveryRequestReportComponent_Factory(t) { return new (t || DeliveryRequestReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"])); };
DeliveryRequestReportComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: DeliveryRequestReportComponent, selectors: [["app-delivery-request-report"]], viewQuery: function DeliveryRequestReportComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
    } }, decls: 35, vars: 12, consts: [[1, "col-sm-8", "sticky-header-wmd"], [1, "row"], [1, "col-sm-5", "pa0"], [1, "col-sm-6"], [3, "ngModel", "settings", "placeholder", "data", "ngModelChange", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-terminal-item-code", 1, "table-responsive"], ["datatable", "", 1, "table", "table-bordered", 3, "dtOptions", "dtTrigger"], ["data-key", "RegionName"], ["data-key", "Location"], ["data-key", "LocId"], ["data-key", "CustomerName"], ["data-key", "CustomerBrandID"], ["data-ke", "Order"], ["data-key", "ProductType"], ["data-key", "RequestedQty"], [4, "ngIf"], ["class", "pa bg-white z-index5 loading-wrapper", 4, "ngIf"], [4, "ngFor", "ngForOf"], [3, "ngClass"], ["title", "Recurring", 1, "fas", "fa-sync", "color-black"], ["title", "Auto-Generated", 1, "fas", "fa-magic", "ml10"], [1, "pa", "bg-white", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"]], template: function DeliveryRequestReportComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_ngModelChange_5_listener($event) { return ctx.SelectedRegions = $event; })("onSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelect_5_listener($event) { return ctx.OnFilterSelect($event, "region"); })("onDeSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelect_5_listener($event) { return ctx.onFilterDeselect($event, "region"); })("onSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelectAll_5_listener($event) { return ctx.onFilterSelectAll($event, "region"); })("onDeSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelectAll_5_listener($event) { return ctx.onFilterDeselectAll($event, "region"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "ng-multiselect-dropdown", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_ngModelChange_7_listener($event) { return ctx.SelectedLocations = $event; })("onSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelect_7_listener($event) { return ctx.OnFilterSelect($event, "location"); })("onDeSelect", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelect_7_listener($event) { return ctx.onFilterDeselect($event, "location"); })("onSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onSelectAll_7_listener($event) { return ctx.onFilterSelectAll($event, "location"); })("onDeSelectAll", function DeliveryRequestReportComponent_Template_ng_multiselect_dropdown_onDeSelectAll_7_listener($event) { return ctx.onFilterDeselectAll($event, "location"); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "table", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "th", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Region");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Location");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "LocationId");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Customer");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Customer BrandId");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "Order");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30, "Product");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "th", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Requested Qty");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](33, DeliveryRequestReportComponent_tbody_33_Template, 2, 1, "tbody", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](34, DeliveryRequestReportComponent_div_34_Template, 2, 0, "div", 20);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedRegions)("settings", ctx.RegionDdlSettings)("placeholder", "Select Region")("data", ctx.Regions);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedLocations)("settings", ctx.LocationDdlSettings)("placeholder", "Select Location")("data", ctx.Locations);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtDRGridOptions)("dtTrigger", ctx.dtDRReportTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.DRReportsData == null ? null : ctx.DRReportsData.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
    } }, directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"]], styles: [".table .bg_must_go {\r\n    background-color: #f5d0d0;\r\n}\r\n\r\n  .table .bg_should_go {\r\n    background-color: #f7e6a9;\r\n}\r\n\r\n  .table .bg_could_go {\r\n    background-color: #e4e2e2;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvY2Fycmllci9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0kseUJBQXlCO0FBQzdCOztBQUVBO0lBQ0kseUJBQXlCO0FBQzdCOztBQUVBO0lBQ0kseUJBQXlCO0FBQzdCIiwiZmlsZSI6InNyYy9hcHAvY2Fycmllci9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC9kZWxpdmVyeS1yZXF1ZXN0LXJlcG9ydC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiOjpuZy1kZWVwIC50YWJsZSAuYmdfbXVzdF9nbyB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjVkMGQwO1xyXG59XHJcblxyXG46Om5nLWRlZXAgLnRhYmxlIC5iZ19zaG91bGRfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2Y3ZTZhOTtcclxufVxyXG5cclxuOjpuZy1kZWVwIC50YWJsZSAuYmdfY291bGRfZ28ge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2U0ZTJlMjtcclxufVxyXG4iXX0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryRequestReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-delivery-request-report',
                templateUrl: './delivery-request-report.component.html',
                styleUrls: ['./delivery-request-report.component.css']
            }]
    }], function () { return [{ type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"] }]; }, { datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }] }); })();


/***/ }),

/***/ "./src/app/carrier/delivery-request-report/delivery-request-report.module.ts":
/*!***********************************************************************************!*\
  !*** ./src/app/carrier/delivery-request-report/delivery-request-report.module.ts ***!
  \***********************************************************************************/
/*! exports provided: DeliveryRequestReportModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryRequestReportModule", function() { return DeliveryRequestReportModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./delivery-request-report.component */ "./src/app/carrier/delivery-request-report/delivery-request-report.component.ts");









const routesDrReport = [
    {
        path: "",
        component: _delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]
    },
];
class DeliveryRequestReportModule {
}
DeliveryRequestReportModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: DeliveryRequestReportModule });
DeliveryRequestReportModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function DeliveryRequestReportModule_Factory(t) { return new (t || DeliveryRequestReportModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
            src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(routesDrReport)
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DeliveryRequestReportModule, { declarations: [_delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
        src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryRequestReportModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [_delivery_request_report_component__WEBPACK_IMPORTED_MODULE_6__["DeliveryRequestReportComponent"]],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"],
                    src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(routesDrReport)
                ]
            }]
    }], null, null); })();


/***/ })

}]);
//# sourceMappingURL=carrier-delivery-request-report-delivery-request-report-module-es2015.js.map