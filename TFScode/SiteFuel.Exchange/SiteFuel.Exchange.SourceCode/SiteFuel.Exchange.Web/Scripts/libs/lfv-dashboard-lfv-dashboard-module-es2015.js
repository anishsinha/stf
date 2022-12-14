(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["lfv-dashboard-lfv-dashboard-module"],{

/***/ "./node_modules/angular7-csv/dist/Angular-csv.js":
/*!*******************************************************!*\
  !*** ./node_modules/angular7-csv/dist/Angular-csv.js ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CsvConfigConsts = /** @class */ (function () {
    function CsvConfigConsts() {
    }
    CsvConfigConsts.EOL = "\r\n";
    CsvConfigConsts.BOM = "\ufeff";
    CsvConfigConsts.DEFAULT_FIELD_SEPARATOR = ',';
    CsvConfigConsts.DEFAULT_DECIMAL_SEPARATOR = '.';
    CsvConfigConsts.DEFAULT_QUOTE = '"';
    CsvConfigConsts.DEFAULT_SHOW_TITLE = false;
    CsvConfigConsts.DEFAULT_TITLE = 'My Report';
    CsvConfigConsts.DEFAULT_FILENAME = 'mycsv.csv';
    CsvConfigConsts.DEFAULT_SHOW_LABELS = false;
    CsvConfigConsts.DEFAULT_USE_BOM = true;
    CsvConfigConsts.DEFAULT_HEADER = [];
    CsvConfigConsts.DEFAULT_NO_DOWNLOAD = false;
    return CsvConfigConsts;
}());
exports.CsvConfigConsts = CsvConfigConsts;
exports.ConfigDefaults = {
    filename: CsvConfigConsts.DEFAULT_FILENAME,
    fieldSeparator: CsvConfigConsts.DEFAULT_FIELD_SEPARATOR,
    quoteStrings: CsvConfigConsts.DEFAULT_QUOTE,
    decimalseparator: CsvConfigConsts.DEFAULT_DECIMAL_SEPARATOR,
    showLabels: CsvConfigConsts.DEFAULT_SHOW_LABELS,
    showTitle: CsvConfigConsts.DEFAULT_SHOW_TITLE,
    title: CsvConfigConsts.DEFAULT_TITLE,
    useBom: CsvConfigConsts.DEFAULT_USE_BOM,
    headers: CsvConfigConsts.DEFAULT_HEADER,
    noDownload: CsvConfigConsts.DEFAULT_NO_DOWNLOAD
};
var AngularCsv = /** @class */ (function () {
    function AngularCsv(DataJSON, filename, options) {
        this.csv = "";
        var config = options || {};
        this.data = typeof DataJSON != 'object' ? JSON.parse(DataJSON) : DataJSON;
        this._options = objectAssign({}, exports.ConfigDefaults, config);
        if (this._options.filename) {
            this._options.filename = filename;
        }
        this.generateCsv();
    }
    /**
     * Generate and Download Csv
     */
    AngularCsv.prototype.generateCsv = function () {
        if (this._options.useBom) {
            this.csv += CsvConfigConsts.BOM;
        }
        if (this._options.showTitle) {
            this.csv += this._options.title + '\r\n\n';
        }
        this.getHeaders();
        this.getBody();
        if (this.csv == '') {
            console.log("Invalid data");
            return;
        }
        if (this._options.noDownload) {
            return this.csv;
        }
        var blob = new Blob([this.csv], { "type": "text/csv;charset=utf8;" });
        if (navigator.msSaveBlob) {
            var filename = this._options.filename.replace(/ /g, "_") + ".csv";
            navigator.msSaveBlob(blob, filename);
        }
        else {
            var uri = 'data:attachment/csv;charset=utf-8,' + encodeURI(this.csv);
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.setAttribute('visibility', 'hidden');
            link.download = this._options.filename.replace(/ /g, "_") + ".csv";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    };
    /**
     * Create Headers
     */
    AngularCsv.prototype.getHeaders = function () {
        var _this = this;
        if (this._options.headers.length > 0) {
            var headers = this._options.headers;
            var row = headers.reduce(function (headerRow, header) {
                return headerRow + header + _this._options.fieldSeparator;
            }, '');
            row = row.slice(0, -1);
            this.csv += row + CsvConfigConsts.EOL;
        }
    };
    /**
     * Create Body
     */
    AngularCsv.prototype.getBody = function () {
        for (var i = 0; i < this.data.length; i++) {
            var row = "";
            for (var index in this.data[i]) {
                row += this.formartData(this.data[i][index]) + this._options.fieldSeparator;
            }
            row = row.slice(0, -1);
            this.csv += row + CsvConfigConsts.EOL;
        }
    };
    /**
     * Format Data
     * @param {any} data
     */
    AngularCsv.prototype.formartData = function (data) {
        if (this._options.decimalseparator === 'locale' && AngularCsv.isFloat(data)) {
            return data.toLocaleString();
        }
        if (this._options.decimalseparator !== '.' && AngularCsv.isFloat(data)) {
            return data.toString().replace('.', this._options.decimalseparator);
        }
        if (typeof data === 'string') {
            data = data.replace(/"/g, '""');
            if (this._options.quoteStrings || data.indexOf(',') > -1 || data.indexOf('\n') > -1 || data.indexOf('\r') > -1) {
                data = this._options.quoteStrings + data + this._options.quoteStrings;
            }
            return data;
        }
        if (typeof data === 'boolean') {
            return data ? 'TRUE' : 'FALSE';
        }
        return data;
    };
    /**
     * Check if is Float
     * @param {any} input
     */
    AngularCsv.isFloat = function (input) {
        return +input === input && (!isFinite(input) || Boolean(input % 1));
    };
    return AngularCsv;
}());
exports.AngularCsv = AngularCsv;
var hasOwnProperty = Object.prototype.hasOwnProperty;
var propIsEnumerable = Object.prototype.propertyIsEnumerable;
/**
 * Convet to Object
 * @param {any} val
 */
function toObject(val) {
    if (val === null || val === undefined) {
        throw new TypeError('Object.assign cannot be called with null or undefined');
    }
    return Object(val);
}
/**
 * Assign data  to new Object
 * @param {any}   target
 * @param {any[]} ...source
 */
function objectAssign(target) {
    var source = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        source[_i - 1] = arguments[_i];
    }
    var from;
    var to = toObject(target);
    var symbols;
    for (var s = 1; s < arguments.length; s++) {
        from = Object(arguments[s]);
        for (var key in from) {
            if (hasOwnProperty.call(from, key)) {
                to[key] = from[key];
            }
        }
        if (Object.getOwnPropertySymbols) {
            symbols = Object.getOwnPropertySymbols(from);
            for (var i = 0; i < symbols.length; i++) {
                if (propIsEnumerable.call(from, symbols[i])) {
                    to[symbols[i]] = from[symbols[i]];
                }
            }
        }
    }
    return to;
}
//# sourceMappingURL=Angular-csv.js.map

/***/ }),

/***/ "./src/app/lfv-dashboard/carrier-bol-report/carrier-bol-report.component.ts":
/*!**********************************************************************************!*\
  !*** ./src/app/lfv-dashboard/carrier-bol-report/carrier-bol-report.component.ts ***!
  \**********************************************************************************/
/*! exports provided: CarrierBolReportComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CarrierBolReportComponent", function() { return CarrierBolReportComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");









function CarrierBolReportComponent_tbody_42_tr_1_Template(rf, ctx) { if (rf & 1) {
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const record_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](record_r3.BOL);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.TerminalName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.LoadDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.NetQuantity, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.GrossQuantity, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.BadgeNumber, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.CarrierName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.CarrierID, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.FuelTypeName, " ");
} }
function CarrierBolReportComponent_tbody_42_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, CarrierBolReportComponent_tbody_42_tr_1_Template, 19, 9, "tr", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r0.ReportRecords);
} }
function CarrierBolReportComponent_div_43_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class CarrierBolReportComponent {
    constructor(dashboardservice) {
        this.dashboardservice = dashboardservice;
        this.ReportRecords = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
        this.fromDate = null;
        this.toDate = null;
    }
    ngOnInit() {
        this.intializeGrid();
    }
    intializeGrid() {
        this.ShowGridLoader = true;
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Lift File Records', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Lift File Records', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getCarrierBOLReport();
    }
    setFromDate(event) {
        this.fromDate = event;
    }
    setToDate(event) {
        this.toDate = event;
    }
    ApplyFilter() {
        if ((this.fromDate == null || this.fromDate == undefined || this.fromDate == "")
            || (this.toDate == null || this.toDate == undefined || this.toDate == "")) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("From/To date not selected", undefined, undefined);
        }
        else {
            this.reloadGrid();
        }
    }
    reloadGrid() {
        $("#carrierbolreport-datatable").DataTable().clear().destroy();
        this.getCarrierBOLReport();
    }
    ClearFilter() {
        this.fromDate = null;
        this.toDate = null;
        this.reloadGrid();
    }
    getCarrierBOLReport() {
        let fromDate = this.fromDate;
        let toDate = this.toDate;
        this.ShowGridLoader = true;
        this.dashboardservice.getCarrierBOLReport(fromDate, toDate).subscribe((data) => {
            this.ShowGridLoader = false;
            this.ReportRecords = data;
            this.dtTrigger.next();
        });
    }
}
CarrierBolReportComponent.??fac = function CarrierBolReportComponent_Factory(t) { return new (t || CarrierBolReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"])); };
CarrierBolReportComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: CarrierBolReportComponent, selectors: [["app-carrier-bol-report"]], decls: 44, vars: 9, consts: [[1, "row", "mb10"], [1, "col-sm-12"], [1, "well", "pb10", "mb0"], [1, "row"], [1, "col-xs-12", "col-sm-2", "col-md-1", "pt5", "pr0"], [1, "fa", "fa-filter", "mr5", "fs16"], [1, "f-normal", "fs16"], [1, "col-md-3"], ["type", "text", "placeholder", "From", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "To", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "ngModelChange", "onDateChange"], [1, "col-12", "col-sm-4", "col-md-3", "mt5-xs"], ["type", "button", "value", "Apply", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "Clear Filter", 1, "btn", "ml5", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "carrierbolreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Bol"], ["data-key", "TerminalName"], ["data-key", "LoadDate"], ["data-key", "NetQuantity"], ["data-key", "GrossQuantity"], ["data-key", "BadgeNumber"], ["data-key", "CarrierName"], ["data-key", "CarrierID"], ["data-key", "FuelTypeName"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function CarrierBolReportComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Filter");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function CarrierBolReportComponent_Template_input_ngModelChange_9_listener($event) { return ctx.fromDate = $event; })("onDateChange", function CarrierBolReportComponent_Template_input_onDateChange_9_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function CarrierBolReportComponent_Template_input_ngModelChange_11_listener($event) { return ctx.toDate = $event; })("onDateChange", function CarrierBolReportComponent_Template_input_onDateChange_11_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CarrierBolReportComponent_Template_input_click_13_listener() { return ctx.ApplyFilter(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "input", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function CarrierBolReportComponent_Template_input_click_14_listener() { return ctx.ClearFilter(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "table", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](25, "BOL#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](27, "Terminal Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](28, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](29, "Lift Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](30, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](31, "Net Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](32, "th", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](33, "Gross Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](34, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](35, "Badge#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](36, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](37, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](38, "th", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](39, "CarrierID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "th", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](41, "Fuel Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](42, CarrierBolReportComponent_tbody_42_Template, 2, 1, "tbody", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](43, CarrierBolReportComponent_div_43_Template, 5, 0, "div", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.fromDate)("format", "MM/DD/YYYY");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.toDate)("format", "MM/DD/YYYY")("minDate", ctx.fromDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.ReportRecords == null ? null : ctx.ReportRecords.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.ShowGridLoader);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgModel"], angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvY2Fycmllci1ib2wtcmVwb3J0L2NhcnJpZXItYm9sLXJlcG9ydC5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](CarrierBolReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-carrier-bol-report',
                templateUrl: './carrier-bol-report.component.html',
                styleUrls: ['./carrier-bol-report.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/carrier/carrier.component.ts":
/*!************************************************************!*\
  !*** ./src/app/lfv-dashboard/carrier/carrier.component.ts ***!
  \************************************************************/
/*! exports provided: CarrierComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CarrierComponent", function() { return CarrierComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");




class CarrierComponent {
    constructor(_lfvService) {
        this._lfvService = _lfvService;
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
            this.createChartData();
        }
    }
    //public openLFVScratchReportGrid(): void {
    //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
    //}
    RendorChart(data, carrierList, chartHeight) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            try {
                if (this.chart)
                    this.chart.destroy();
            }
            catch (e) {
            }
            var options = {
                colors: ["#00FF00", "#ff0000", "#FF69B4", "#FFFF00", "#000080", "#00A7C6", "#800080", '#0077ff', '#A9D794'],
                series: data,
                chart: {
                    type: 'bar',
                    height: chartHeight,
                    stacked: true,
                    toolbar: {
                        show: true
                    },
                    animations: {
                        enabled: false
                    }
                },
                markers: {
                    size: 0
                },
                responsive: [{
                        breakpoint: undefined,
                        options: {}
                    }],
                plotOptions: {
                    bar: {
                        // borderRadius: 8,
                        horizontal: true,
                        dataLabels: {
                        //  position: 'bottom'
                        }
                    },
                },
                xaxis: {
                    type: 'category',
                    categories: carrierList,
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'left',
                    offsetX: 40
                    //  offsetY: 40
                },
                fill: {
                    opacity: 1
                    //  colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']
                }
            };
            this.chart = new ApexCharts(document.querySelector("#chart-timeline1"), options);
            try {
                if (this.chart)
                    this.chart.render();
            }
            catch (e) {
                this.chart.destroy();
                this.chart.render();
            }
        });
    }
    createChartData() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var mapList = [];
            var carrierList = [];
            var chartHeight = 300;
            //Match Records
            var mtchRec = this.LFValidationList.map(res => { return [res.MatchedRecordCount]; }).toString();
            var match = mtchRec && mtchRec.split(",").map(Number);
            //NoMatch Records
            var nomtchRec = this.LFValidationList.map(res => { return [res.NoMatchRecordCount]; }).toString();
            var nomatch = nomtchRec && nomtchRec.split(",").map(Number);
            //Partial Match
            var partialRec = this.LFValidationList.map(res => { return [res.PartialMatchRecordCount]; }).toString();
            var partial = partialRec && partialRec.split(",").map(Number);
            //Duplicate
            var dupRec = this.LFValidationList.map(res => { return [res.DuplicateRecordCount]; }).toString();
            var duplicate = dupRec && dupRec.split(",").map(Number);
            //PendingMatchCount
            var pendingRec = this.LFValidationList.map(res => { return [res.PendingMatchCount]; }).toString();
            var pending = pendingRec && pendingRec.split(",").map(Number);
            //activeException
            var activeExcRec = this.LFValidationList.map(res => { return [res.ActiveExceptionRecordCount]; }).toString();
            var activeExc = activeExcRec && activeExcRec.split(",").map(Number);
            //IgnoredMatchRecordCount
            var ignoreRec = this.LFValidationList.map(res => { return [res.IgnoredMatchRecordCount]; }).toString();
            var Ignore = ignoreRec && ignoreRec.split(",").map(Number);
            //UnmatchedRecordCount
            var unmstchRec = this.LFValidationList.map(res => { return [res.UnmatchedRecordCount]; }).toString();
            var unMatch = unmstchRec && unmstchRec.split(",").map(Number);
            //ForcedIgnoreRecordCount
            var forcedIgnoredRec = this.LFValidationList.map(res => { return [res.ForcedIgnoredMatchRecordCount]; }).toString();
            var forcedIgnored = forcedIgnoredRec && forcedIgnoredRec.split(",").map(Number);
            if (this.LFValidationList.length > 0) {
                yield mapList.push({ name: 'Matched ', data: match });
                yield mapList.push({ name: 'No Match ', data: nomatch });
                yield mapList.push({ name: 'Partial Match ', data: partial });
                yield mapList.push({ name: 'Pending Match ', data: pending });
                yield mapList.push({ name: 'Duplicate  ', data: duplicate });
                yield mapList.push({ name: 'Active Exception ', data: activeExc });
                yield mapList.push({ name: 'Ignored Match ', data: Ignore });
                yield mapList.push({ name: 'Forced Ignore', data: forcedIgnored });
                yield mapList.push({ name: 'Unmatched  ', data: unMatch });
                (yield this.LFValidationList) && this.LFValidationList.map(lfv => {
                    carrierList.push(lfv.CarrierID ? lfv.CarrierID : '-');
                });
                chartHeight = chartHeight + (carrierList.length * 40);
            }
            else {
                yield mapList.push({ name: 'Matched ', data: [] });
                yield mapList.push({ name: 'No Match ', data: [] });
                yield mapList.push({ name: 'Partial Match ', data: [] });
                yield mapList.push({ name: 'Pending Match ', data: [] });
                yield mapList.push({ name: 'Duplicate  ', data: [] });
                yield mapList.push({ name: 'Active Exception ', data: [] });
                yield mapList.push({ name: 'Ignored Match ', data: [] });
                yield mapList.push({ name: 'Forced Ignore  ', data: [] });
                yield mapList.push({ name: 'Unmatched  ', data: [] });
            }
            yield this.RendorChart(mapList, carrierList, chartHeight);
        });
    }
    openSupplierBOLReportGrid() {
        window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
    }
    openCarrierBOLReportGrid() {
        window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
    }
}
CarrierComponent.??fac = function CarrierComponent_Factory(t) { return new (t || CarrierComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"])); };
CarrierComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: CarrierComponent, selectors: [["app-carrier-performace"]], inputs: { LFValidationList: "LFValidationList" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["????NgOnChangesFeature"]], decls: 4, vars: 0, consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-md-10"], ["id", "chart-timeline1"]], template: function CarrierComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } }, styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvY2Fycmllci9jYXJyaWVyLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](CarrierComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-carrier-performace',
                templateUrl: './carrier.component.html',
                styleUrls: ['./carrier.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"] }]; }, { LFValidationList: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }] }); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts":
/*!******************************************************************************!*\
  !*** ./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts ***!
  \******************************************************************************/
/*! exports provided: LeftSideFilterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LeftSideFilterComponent", function() { return LeftSideFilterComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");
/* harmony import */ var moment__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");










class LeftSideFilterComponent {
    constructor(_lfvSevice) {
        this._lfvSevice = _lfvSevice;
        this.DateType = 3;
        this.CarrierDrpDwnList = [];
        //min max date
        this.search = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.export = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.matchingWindowDays = matchingWindowDays;
        this.selectedCarrierList = [];
        this.isMatchingWindow = false;
        this.selectedStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["LFVRecordStatus"].Clean;
        this.LFVRecordStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["LFVRecordStatus"];
        this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
        this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
        this.minfromdate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
    }
    ngOnInit() {
        this.getLFCarrier();
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'DisplayName',
            textField: 'DisplayName',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        };
    }
    getLFCarrier() {
        let fromDate = this.fromDate;
        let toDate = this.toDate;
        this._lfvSevice.getLFCarrier(fromDate, toDate).subscribe(res => {
            if (res) {
                this.CarrierDrpDwnList = res;
                this.CarrierDrpDwnList && this.CarrierDrpDwnList.map(m => { m.DisplayName = `${m.Name}-${m.Code}`; });
                //code=>carrierId
                //name=CarrierName
            }
            else
                this.CarrierDrpDwnList = [];
        });
    }
    changeDateType(value) {
        this.DateType = value;
        if (this.DateType == 1) {
            this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-1, 'days').format('MM/DD/YYYY');
            this.isMatchingWindow = false;
        }
        else if (this.DateType == 3) {
            this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
            this.isMatchingWindow = false;
        }
        else {
            this.isMatchingWindow = true;
            var day = this.matchingWindowDays ? this.matchingWindowDays : 3; //3 is default 
            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-day, 'days').format('MM/DD/YYYY');
        }
        this.onSearch();
    }
    setFromDate(event) {
        this.fromDate = event;
    }
    setToDate(event) {
        this.toDate = event;
        this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
    }
    onCarrierSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            yield this.selectedCarrierList.map(m => { m.Name = this.CarrierDrpDwnList.find(f => f.DisplayName == m.DisplayName).Code; });
        });
    }
    onCarrierDeSelect($event) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            yield this.selectedCarrierList.map(m => { m.Name = this.CarrierDrpDwnList.find(f => f.DisplayName == m.DisplayName).Code; });
        });
    }
    onSearch() {
        var startDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.fromDate, "MM/DD/YYYY");
        var endDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY");
        var result = endDate.diff(startDate, 'days');
        if (result > 8) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Date Difference should be less than 7 days", undefined, undefined);
        }
        else
            this.search.emit(true);
    }
    onExport() {
        this.export.emit(this.selectedStatus);
    }
}
LeftSideFilterComponent.??fac = function LeftSideFilterComponent_Factory(t) { return new (t || LeftSideFilterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"])); };
LeftSideFilterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: LeftSideFilterComponent, selectors: [["app-left-side-filter"]], outputs: { search: "search", export: "export" }, decls: 60, vars: 29, consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row", "mb10"], [1, "col-sm-12", "text-center", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-md-3"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "disabled", "ngModelChange", "onDateChange"], [1, "col-sm-3"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange", "onSelect", "onDeSelect"], [1, "col-sm-3", "text-right", "form-buttons"], ["id", "Submit", "type", "button", "value", "Search", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], ["id", "statusSelectbtn", "type", "button", "value", "Export", "data-toggle", "modal", "data-target", "#statusSelect", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid"], ["id", "statusSelect", "tabindex", "-1", "role", "dialog", "aria-labelledby", "statusSelectLabel", "aria-hidden", "true", 1, "modal", "fade"], [1, "modal-dialog", "modal-sm", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["id", "statusSelectLabel", 1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], ["aria-hidden", "true"], [1, "modal-body"], [1, "form-control", 3, "ngModel", "ngModelChange"], [3, "value"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "click"]], template: function LeftSideFilterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](5, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LeftSideFilterComponent_Template_label_click_6_listener() { return ctx.changeDateType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "Today");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](8, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LeftSideFilterComponent_Template_label_click_9_listener() { return ctx.changeDateType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "Matching Window");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](11, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LeftSideFilterComponent_Template_label_click_12_listener() { return ctx.changeDateType(3); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13, "Day Range");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LeftSideFilterComponent_Template_input_ngModelChange_16_listener($event) { return ctx.fromDate = $event; })("onDateChange", function LeftSideFilterComponent_Template_input_onDateChange_16_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LeftSideFilterComponent_Template_input_ngModelChange_18_listener($event) { return ctx.toDate = $event; })("onDateChange", function LeftSideFilterComponent_Template_input_onDateChange_18_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "ng-multiselect-dropdown", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_ngModelChange_20_listener($event) { return ctx.selectedCarrierList = $event; })("onSelect", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_onSelect_20_listener($event) { return ctx.onCarrierSelect($event); })("onDeSelect", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_onDeSelect_20_listener($event) { return ctx.onCarrierDeSelect($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "button", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LeftSideFilterComponent_Template_button_click_22_listener() { return ctx.onSearch(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](23, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "button", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](25, "Export");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "h5", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, "Select Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "button", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "span", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "\u00D7");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "select", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function LeftSideFilterComponent_Template_select_ngModelChange_36_listener($event) { return ctx.selectedStatus = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](38, "Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "No Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](42, "Partial Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Pending");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](46, "Duplicate");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](48, "Active Exception");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](50, "Ignored");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](52, "Unmatched");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](53, "option", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](54, "Forced Ignore");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "div", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "button", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](57, "Close");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "button", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function LeftSideFilterComponent_Template_button_click_58_listener() { return ctx.onExport(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](59, "Generate CSV");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "type")("value", 1)("checked", ctx.DateType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "type")("value", 2)("checked", ctx.DateType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "type")("value", 3)("checked", ctx.DateType == 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.fromDate)("format", "MM/DD/YYYY")("disabled", ctx.DateType != 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.toDate)("format", "MM/DD/YYYY")("disabled", ctx.DateType != 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Carrier(s)")("settings", ctx.multiselectSettingsById)("data", ctx.CarrierDrpDwnList)("ngModel", ctx.selectedCarrierList);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx.selectedStatus);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.Clean);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.NoMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.PartialMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.PendingMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.Duplicate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.ActiveExceptions);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.IgnoreMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.UnMatched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", ctx.LFVRecordStatus.ForcedIgnore);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["??angular_packages_forms_forms_x"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvbGVmdC1zaWRlLWZpbHRlci9sZWZ0LXNpZGUtZmlsdGVyLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](LeftSideFilterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-left-side-filter',
                templateUrl: './left-side-filter.component.html',
                styleUrls: ['./left-side-filter.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"] }]; }, { search: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }], export: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/lfv-accrual-report/lfv-accrual-report.component.ts":
/*!**********************************************************************************!*\
  !*** ./src/app/lfv-dashboard/lfv-accrual-report/lfv-accrual-report.component.ts ***!
  \**********************************************************************************/
/*! exports provided: LfvAccrualReportComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LfvAccrualReportComponent", function() { return LfvAccrualReportComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");











function LfvAccrualReportComponent_tbody_62_tr_1_Template(rf, ctx) { if (rf & 1) {
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const record_r4 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](record_r4.CallId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.RecordDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.bol, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.TerminalName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.Terminals, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.TerminalItemCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.ProductType, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.correctedQuantity, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.LoadDate, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.CarrierID, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.CarrierName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.FileName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r4.recordStatus, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", record_r4.Username, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", record_r4.ModifiedDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", record_r4.LFVResolutionTime, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"]("", record_r4.TimeToBol, " ");
} }
function LfvAccrualReportComponent_tbody_62_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvAccrualReportComponent_tbody_62_tr_1_Template, 35, 17, "tr", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r0.records);
} }
function LfvAccrualReportComponent_tbody_63_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "td", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "No Data Available");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvAccrualReportComponent_div_64_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 44);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 45);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class LfvAccrualReportComponent {
    constructor(_lfvservice) {
        this._lfvservice = _lfvservice;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.IsLoading = false;
        this.records = [];
        this.ProductTypesList = [];
        this.selectedProductTypesList = [];
        this.ProductTypeIds = "";
        this.FromDate = null;
        this.ToDate = null;
        this.ProductTypeIds = "";
    }
    ngOnInit() {
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        };
        let exportColumns = { columns: ':visible' };
        let gridcolumnsDetails = [];
        gridcolumnsDetails = [
            { title: 'CallID', name: 'CallId', data: 'CallId', "autoWidth": true },
            { title: 'Record Date', name: 'RecordDate', data: 'RecordDate', "autoWidth": true },
            { title: 'BOL#', name: 'BOL', data: 'bol', "autoWidth": true },
            { title: 'Terminal Code', name: 'TerminalName', data: 'TerminalName', "autoWidth": true },
            { title: 'Terminals', name: 'Terminal', data: 'Terminals', "autoWidth": true },
            { title: 'Terminal Item Code', name: 'TerminalItemCode', data: 'TerminalItemCode', "autoWidth": true },
            { title: 'Product Type', name: 'ProductType', data: 'ProductType', "autoWidth": true },
            { title: 'Corrected Quantity', name: 'correctedQuantity', data: 'correctedQuantity', "autoWidth": true },
            { title: 'Load Date', name: 'LoadDate', data: 'LoadDate', "autoWidth": true },
            { title: 'CarrierID', name: 'CarrierID', data: 'CarrierID', "autoWidth": true },
            { title: 'Carrier Name', name: 'CarrierName', data: 'CarrierName', "autoWidth": true },
            { title: 'FileName', name: 'FileName', data: 'FileName', "autoWidth": true },
            //{ title: 'Reason', name: 'Reason', data: 'Reason', "autowidth": true },
            { title: 'Status', name: 'RecordStatus', data: 'recordStatus', "autowidth": true },
            { title: 'User Name', name: 'Username', data: 'Username', "autowidth": true },
            { title: 'Modified Date (MST)', name: 'ModifiedDate', data: 'ModifiedDate', "autowidth": true },
            { title: 'Resolution Time', name: 'LFVResolutionTime', data: 'LFVResolutionTime', "autowidth": true },
            { title: 'Time to BOL', name: 'TimeToBol', data: 'TimeToBol', "autowidth": true }
            //{ title: 'Reason Code', name: 'ReasonCode', data: 'ReasonCode', "autowidth": true },
            //{ title: 'Reason Category', name: 'ReasonCategory', data: 'ReasonCategory', "autowidth": true }
        ];
        this.dtOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            ajax: (dataTablesParameters, callback) => {
                let inputs = {
                    FromDate: this.FromDate,
                    ToDate: this.ToDate,
                    ProductTypeIds: this.ProductTypeIds
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.IsLoading = true;
                this._lfvservice.getLFVAccrualReportGrid(inputData).subscribe((resp) => {
                    this.records = resp.data;
                    this.IsLoading = false;
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        data: resp.data
                    });
                    // this.getLFVValidationStatsAndProductTypesDDL();
                });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[0, 'desc']],
            buttons: [
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'LiftFile Accrual Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'LiftFile Accrual Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: gridcolumnsDetails
        };
    }
    ngAfterViewInit() {
        this.getLFAccrualGrid();
        this.dtTrigger.next();
        this.getLFVValidationStatsAndProductTypesDDL();
    }
    getLFAccrualGrid() {
        this.IsLoading = true;
        this.refreshDatatable();
        this.IsLoading = false;
    }
    getLFVValidationStatsAndProductTypesDDL() {
        this.IsLoading = true;
        let input = {
            FromDate: this.FromDate,
            ToDate: this.ToDate
        };
        this._lfvservice.GetLFVValidationStatsAndProductTypesDDL(input).subscribe((resp) => {
            this.ProductTypesList = resp;
            this.IsLoading = false;
        });
    }
    refreshDatatable() {
        this.dtElements.forEach((dtElement) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance) => {
                    dtInstance.draw();
                });
            }
        });
    }
    ApplyFilter() {
        if ((this.FromDate != null && this.FromDate != undefined && this.FromDate != "")
            && (this.ToDate == null || this.ToDate == undefined || this.ToDate == "")) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("To Date is required", undefined, undefined);
            return;
        }
        this.getSelectedProductTypes();
        this.refreshDatatable();
    }
    ClearFilter() {
        this.FromDate = null;
        this.ToDate = null;
        this.selectedProductTypesList = [];
        this.getSelectedProductTypes();
        this.refreshDatatable();
    }
    getSelectedProductTypes() {
        if (this.selectedProductTypesList == null || this.selectedProductTypesList.length == 0 || this.selectedProductTypesList == undefined) {
            this.ProductTypeIds = "";
        }
        else if (this.selectedProductTypesList != null || this.selectedProductTypesList.length > 0) {
            this.ProductTypeIds = this.selectedProductTypesList.map(m => m.Id).join(',');
        }
    }
    setFromDate(event) {
        this.FromDate = event;
    }
    setToDate(event) {
        this.ToDate = event;
    }
    ngOnDestroy() {
        this.dtTrigger.unsubscribe();
    }
}
LfvAccrualReportComponent.??fac = function LfvAccrualReportComponent_Factory(t) { return new (t || LfvAccrualReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__["LiftfiledashboardserviceService"])); };
LfvAccrualReportComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: LfvAccrualReportComponent, selectors: [["app-lfv-accrual-report"]], viewQuery: function LfvAccrualReportComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????loadQuery"]()) && (ctx.dtElements = _t);
    } }, decls: 65, vars: 14, consts: [[1, "row", "mb10"], [1, "col-sm-12"], [1, "well", "pb10", "mb0"], [1, "row"], [1, "col-sm-1", "pr0", "mt-1"], [1, "fa", "fa-filter", "mr5", "fs16"], [1, "f-normal", "fs16"], [1, "col-sm-2"], ["type", "text", "placeholder", "From", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "To", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "ngModelChange", "onDateChange"], [1, "col-md-3"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-sm-2", "mt5-xs"], ["type", "button", "value", "Apply", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "Clear Filter", 1, "btn", "ml5", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "accrualreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "RecordDate"], ["data-key", "Bol"], ["data-key", "TerminalCode"], ["data-key", "Terminals"], ["data-key", "TerminalItemCode"], ["data-key", "ProductType"], ["data-key", "CorrectedQuanity"], ["data-key", "LoadDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "FileName"], ["data-key", "RecordStatus"], ["data-key", "UserName"], ["data-key", "ModifiedDate"], ["data-key", "LFVResolutionTime"], ["data-key", "TimeToBol"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], ["colspan", "14", 1, "text-center"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function LfvAccrualReportComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](5, "i", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](7, "Filter");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvAccrualReportComponent_Template_input_ngModelChange_9_listener($event) { return ctx.FromDate = $event; })("onDateChange", function LfvAccrualReportComponent_Template_input_onDateChange_9_listener($event) { return ctx.setFromDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvAccrualReportComponent_Template_input_ngModelChange_11_listener($event) { return ctx.ToDate = $event; })("onDateChange", function LfvAccrualReportComponent_Template_input_onDateChange_11_listener($event) { return ctx.setToDate($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "span");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "ng-multiselect-dropdown", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvAccrualReportComponent_Template_ng_multiselect_dropdown_ngModelChange_14_listener($event) { return ctx.selectedProductTypesList = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "input", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvAccrualReportComponent_Template_input_click_16_listener() { return ctx.ApplyFilter(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "input", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvAccrualReportComponent_Template_input_click_17_listener() { return ctx.ClearFilter(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "div", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "table", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "CallID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Record Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "th", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32, "BOL#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "Terminal Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "th", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "th", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](40, "Product Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "th", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](42, "Corrected Quanity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](43, "th", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](44, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "th", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](46, "CarrierID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "th", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](48, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "th", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](50, "File Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](51, "--> ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "th", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](53, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](54, "th", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](55, "User Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](56, "th", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](57, "Modified Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](58, "th", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](59, "Resolution Time");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](60, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](61, "Time to BOL");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](62, LfvAccrualReportComponent_tbody_62_Template, 2, 1, "tbody", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](63, LfvAccrualReportComponent_tbody_63_Template, 4, 0, "tbody", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](64, LfvAccrualReportComponent_div_64_Template, 5, 0, "div", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.FromDate)("format", "MM/DD/YYYY");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngModel", ctx.ToDate)("format", "MM/DD/YYYY")("minDate", ctx.FromDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Products")("settings", ctx.multiselectSettingsById)("data", ctx.ProductTypesList)("ngModel", ctx.selectedProductTypesList);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.records == null ? null : ctx.records.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.records == null ? null : ctx.records.length) == 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.IsLoading);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgModel"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["MultiSelectComponent"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"]], styles: [".dataTables_empty[_ngcontent-%COMP%] {\r\n    display: none;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtYWNjcnVhbC1yZXBvcnQvbGZ2LWFjY3J1YWwtcmVwb3J0LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxhQUFhO0FBQ2pCIiwiZmlsZSI6InNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtYWNjcnVhbC1yZXBvcnQvbGZ2LWFjY3J1YWwtcmVwb3J0LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuZGF0YVRhYmxlc19lbXB0eSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG59XHJcbiJdfQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](LfvAccrualReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-lfv-accrual-report',
                templateUrl: './lfv-accrual-report.component.html',
                styleUrls: ['./lfv-accrual-report.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__["LiftfiledashboardserviceService"] }]; }, { dtElements: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }] }); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/lfv-dashboard.module.ts":
/*!*******************************************************!*\
  !*** ./src/app/lfv-dashboard/lfv-dashboard.module.ts ***!
  \*******************************************************/
/*! exports provided: LfvDashboardModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LfvDashboardModule", function() { return LfvDashboardModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _master_master_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./master/master.component */ "./src/app/lfv-dashboard/master/master.component.ts");
/* harmony import */ var _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./validation/validation.component */ "./src/app/lfv-dashboard/validation/validation.component.ts");
/* harmony import */ var _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./carrier/carrier.component */ "./src/app/lfv-dashboard/carrier/carrier.component.ts");
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../modules/shared.module */ "./src/app/modules/shared.module.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./left-side-filter/left-side-filter.component */ "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts");
/* harmony import */ var _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./lfv-scratch-report/lfv-scratch-report.component */ "./src/app/lfv-dashboard/lfv-scratch-report/lfv-scratch-report.component.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../modules/directive.module */ "./src/app/modules/directive.module.ts");
/* harmony import */ var _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./carrier-bol-report/carrier-bol-report.component */ "./src/app/lfv-dashboard/carrier-bol-report/carrier-bol-report.component.ts");
/* harmony import */ var _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ./supplier-bol-report/supplier-bol-report.component */ "./src/app/lfv-dashboard/supplier-bol-report/supplier-bol-report.component.ts");
/* harmony import */ var _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ./lfv-accrual-report/lfv-accrual-report.component */ "./src/app/lfv-dashboard/lfv-accrual-report/lfv-accrual-report.component.ts");

















const route = [
    { path: '', component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"] },
    { path: 'Dashboard', component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"] },
    { path: 'LFVScratchReport', component: _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"] },
    { path: 'CarrierBolReport', component: _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"] },
    { path: 'SupplierBolReport', component: _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"] },
    { path: 'LFVAccrualReport', component: _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"] }
];
class LfvDashboardModule {
}
LfvDashboardModule.??mod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineNgModule"]({ type: LfvDashboardModule });
LfvDashboardModule.??inj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjector"]({ factory: function LfvDashboardModule_Factory(t) { return new (t || LfvDashboardModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"],
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"],
            angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"],
            _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route),
            _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["????setNgModuleScope"](LfvDashboardModule, { declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__["CarrierComponent"],
        _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__["LeftSideFilterComponent"], _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"], _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"],
        _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"],
        _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"],
        _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"],
        angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](LfvDashboardModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__["CarrierComponent"],
                    _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__["LeftSideFilterComponent"], _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"], _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"],
                    _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"],
                    _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"]],
                imports: [
                    _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
                    _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"],
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"],
                    _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"],
                    angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"],
                    _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route),
                    _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]
                ]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/lfv-scratch-report/lfv-scratch-report.component.ts":
/*!**********************************************************************************!*\
  !*** ./src/app/lfv-dashboard/lfv-scratch-report/lfv-scratch-report.component.ts ***!
  \**********************************************************************************/
/*! exports provided: LfvScratchReportComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LfvScratchReportComponent", function() { return LfvScratchReportComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../LiftFileModels */ "./src/app/lfv-dashboard/LiftFileModels.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");













function LfvScratchReportComponent_div_39_div_1_ng_container_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "option", 64);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementContainerEnd"]();
} if (rf & 2) {
    const bol_r7 = ctx.$implicit;
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", bol_r7.Id)("selected", bol_r7.Id == ctx_r6.bolResolveForm.get("InvoiceFtlDetailId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", bol_r7.Name, " ");
} }
function LfvScratchReportComponent_div_39_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "select", 61);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("change", function LfvScratchReportComponent_div_39_div_1_Template_select_change_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r9); const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2); return ctx_r8.GetBolRecord($event.target.value); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "option", 62);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Select BOL-Product to edit ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, LfvScratchReportComponent_div_39_div_1_ng_container_5_Template, 3, 3, "ng-container", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("value", null);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r5.InvoiceFtlDetailIdList);
} }
function LfvScratchReportComponent_div_39_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_div_39_div_1_Template, 6, 2, "div", 58);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r0.InvoiceFtlDetailIdList != null && ctx_r0.InvoiceFtlDetailIdList.length > 0);
} }
function LfvScratchReportComponent_content_40_div_14_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " BOL/LiftTicket# is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_14_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r10.bolResolveForm.get("BolNumber").errors.required);
} }
function LfvScratchReportComponent_content_40_div_22_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Lift Date is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_22_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r11.bolResolveForm.get("LiftDate").errors.required);
} }
function LfvScratchReportComponent_content_40_div_30_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Gross quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_30_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_30_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r12.bolResolveForm.get("GrossQuantity").errors.required);
} }
function LfvScratchReportComponent_content_40_div_38_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Net quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_38_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_38_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r13.bolResolveForm.get("NetQuantity").errors.required);
} }
function LfvScratchReportComponent_content_40_div_47_div_5_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Terminal is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_47_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_47_div_5_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r20.bolResolveForm.get("SelectedTerminal").errors.required);
} }
function LfvScratchReportComponent_content_40_div_47_Template(rf, ctx) { if (rf & 1) {
    const _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 98);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "label", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](3, "Terminal Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "ng-multiselect-dropdown", 99);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvScratchReportComponent_content_40_div_47_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r23); const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2); return ctx_r22.SelectedTerminalList = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](5, LfvScratchReportComponent_content_40_div_47_div_5_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Terminal")("settings", ctx_r14.multiselectSettingsById)("data", ctx_r14.TerminalList)("ngModel", ctx_r14.SelectedTerminalList);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r14.bolResolveForm.get("SelectedTerminal").invalid && (ctx_r14.bolResolveForm.get("SelectedTerminal").dirty || ctx_r14.bolResolveForm.get("SelectedTerminal").touched));
} }
function LfvScratchReportComponent_content_40_div_53_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](1, " Fuel Type is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_content_40_div_53_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_content_40_div_53_div_1_Template, 2, 0, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r15.bolResolveForm.get("SelectedFuelType").errors.required);
} }
function LfvScratchReportComponent_content_40_Template(rf, ctx) { if (rf & 1) {
    const _r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "content", 65);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "form", 66);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngSubmit", function LfvScratchReportComponent_content_40_Template_form_ngSubmit_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26); const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r25.onSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 67);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 68);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 69);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "label", 70);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](8, "BOL/LiftTicket#");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "span", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](11, "input", 72);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](12, "input", 73);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](13, "input", 74);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](14, LfvScratchReportComponent_content_40_div_14_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "div", 76);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "label", 77);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Lift Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "span", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "input", 78);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("onDateChange", function LfvScratchReportComponent_content_40_Template_input_onDateChange_21_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26); const ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r27.bolResolveForm.get("LiftDate").setValue($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](22, LfvScratchReportComponent_content_40_div_22_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "div", 79);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "label", 80);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Gross Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "span", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](29, "input", 81);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](30, LfvScratchReportComponent_content_40_div_30_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "div", 82);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](32, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "label", 83);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34, "Net Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "span", 71);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](37, "input", 84);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](38, LfvScratchReportComponent_content_40_div_38_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](40, "div", 85);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](41, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "label", 86);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](43, "Badge#");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](44, "input", 87);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "div", 88);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](47, LfvScratchReportComponent_content_40_div_47_Template, 6, 5, "div", 89);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "div", 90);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "label", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](51, "Fuel");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](52, "ng-multiselect-dropdown", 92);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvScratchReportComponent_content_40_Template_ng_multiselect_dropdown_ngModelChange_52_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26); const ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r28.SelectedFuelTypeList = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](53, LfvScratchReportComponent_content_40_div_53_Template, 2, 1, "div", 75);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](54, "div", 59);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](55, "div", 60);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](56, "label", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](57, "Notes");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](58, "textarea", 94);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](59, "div", 95);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](60, "button", 96);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_content_40_Template_button_click_60_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r26); const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](); return ctx_r29._toggleOpened(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](61, "Cancel");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](62, "button", 97);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](63, "Save & Re-Submit");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("formGroup", ctx_r1.bolResolveForm);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.bolResolveForm.get("BolNumber").invalid && (ctx_r1.bolResolveForm.get("BolNumber").dirty || ctx_r1.bolResolveForm.get("BolNumber").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.bolResolveForm.get("LiftDate").invalid && (ctx_r1.bolResolveForm.get("LiftDate").dirty || ctx_r1.bolResolveForm.get("LiftDate").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.bolResolveForm.get("GrossQuantity").invalid && (ctx_r1.bolResolveForm.get("GrossQuantity").dirty || ctx_r1.bolResolveForm.get("GrossQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.bolResolveForm.get("NetQuantity").invalid && (ctx_r1.bolResolveForm.get("NetQuantity").dirty || ctx_r1.bolResolveForm.get("NetQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", !ctx_r1.bolResolveForm.get("IsBulkPlantLift").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Fuel")("settings", ctx_r1.multiselectSettingsById)("data", ctx_r1.FuelTypeList)("ngModel", ctx_r1.SelectedFuelTypeList);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx_r1.bolResolveForm.get("SelectedFuelType").invalid && (ctx_r1.bolResolveForm.get("SelectedFuelType").dirty || ctx_r1.bolResolveForm.get("SelectedFuelType").touched));
} }
function LfvScratchReportComponent_div_41_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 101);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](3, "div", 103);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_tbody_92_tr_1_span_40_Template(rf, ctx) { if (rf & 1) {
    const _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "button", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_tbody_92_tr_1_span_40_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r37); const record_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]().$implicit; const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2); return ctx_r35.getBolDetailsForResolve(record_r31); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "i", 107);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_tbody_92_tr_1_span_41_Template(rf, ctx) { if (rf & 1) {
    const _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "button", 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_tbody_92_tr_1_span_41_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["????restoreView"](_r39); const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](3); return ctx_r38.redirectToMyApprovalTab(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "i", 108);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
function LfvScratchReportComponent_tbody_92_tr_1_Template(rf, ctx) { if (rf & 1) {
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "td", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](38, "input", 105);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](39, "td", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](40, LfvScratchReportComponent_tbody_92_tr_1_span_40_Template, 3, 0, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](41, LfvScratchReportComponent_tbody_92_tr_1_span_41_Template, 3, 0, "span", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const record_r31 = ctx.$implicit;
    const ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.CallId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](record_r31.bol);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.TerminalName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.Terminals, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.correctedQuantity, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.TerminalItemCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.ProductType, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.LoadDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.RecordDate, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.CarrierID, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.CarrierName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.Reason, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.ReasonCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.ReasonCategory, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.recordStatus, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.Username, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.ModifiedDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r31.LFVResolutionTime, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????propertyInterpolate"]("id", record_r31.LiftFileRecordId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????propertyInterpolate"]("value", record_r31.LiftFileRecordId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("checked", ctx_r30.isChecked);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", record_r31.Status == 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", record_r31.Status == 6);
} }
function LfvScratchReportComponent_tbody_92_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, LfvScratchReportComponent_tbody_92_tr_1_Template, 42, 23, "tr", 63);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r3.LFRecords);
} }
function LfvScratchReportComponent_div_98_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 101);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 103);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class LfvScratchReportComponent {
    constructor(fb, dashboardservice) {
        this.fb = fb;
        this.dashboardservice = dashboardservice;
        //side bar related variables
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        //grid variables
        this.LFRecords = [];
        this.cancelButtonText = 'No';
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
        this.isChecked = false;
        this.LFRecordIdsForIgnoreMatch = [];
        this.ShowSideBarLoader = false;
        this.SelectedTerminalList = [];
        this.SelectedFuelTypeList = [];
        //ignore by reason
        this.preferenceSetting = null;
        this.selectedReason = [];
        this.reasonList = [];
        this.dropdownSettings = { singleSelection: true, idField: 'Id', textField: 'Name', allowSearchFilter: true };
    }
    ngOnInit() {
        this.multiselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.intializeGrid();
        this.bolResolveForm = this.buildForm();
        this.getPreferencesSetting();
    }
    intializeGrid() {
        this.ShowGridLoader = true;
        let exportColumns = { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Scratch Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Scratch Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getLFRecords();
    }
    buildForm() {
        let fg = this.fb.group({
            BolNumber: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            LiftDate: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            GrossQuantity: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            NetQuantity: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            BadgeNumber: this.fb.control(''),
            SelectedTerminal: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            SelectedFuelType: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required]),
            Notes: this.fb.control(''),
            LIftFileRecordId: this.fb.control(''),
            InvoiceFtlDetailId: this.fb.control(''),
            IsBulkPlantLift: this.fb.control('')
        });
        this.SelectedFuelTypeList = [];
        this.SelectedTerminalList = [];
        return fg;
    }
    getLFRecords() {
        this.ShowGridLoader = true;
        this.dashboardservice.getLFRecords().subscribe((data) => {
            this.ShowGridLoader = false;
            this.LFRecords = data;
            this.dtTrigger.next();
        });
    }
    reloadGrid() {
        $("#liftfilerecords-datatable").DataTable().clear().destroy();
        this.getLFRecords();
    }
    getBolDetailsForResolve(lfRecord) {
        this.ShowSideBarLoader = true;
        lfRecord.IsFromScratchReport = true;
        this.dashboardservice.getBolDetailsForResolve(lfRecord).subscribe((data) => {
            if (data) {
                this._toggleOpened(true);
                this.selectedLiftFileRecord = data.LiftRecord;
                this.TerminalList = data.TerminalList;
                this.FuelTypeList = data.FuelTypeList;
                this.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;
                this.initFormData(data);
                this.ShowSideBarLoader = false;
            }
        });
    }
    selectAllRecords(eventData) {
        if (eventData != null && eventData != undefined) {
            if (eventData.target.checked) {
                this.isChecked = true;
            }
            else {
                this.isChecked = false;
            }
        }
    }
    ValidateForIgnoreMatchProcessing() {
        let LFRecordIds = this.getLFRecordIds();
        this.selectedReason = [];
        if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {
            if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
                this.GetReasonDescriptionList();
                document.getElementById('openIgnoreModal2').click();
            }
            else {
                this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
            }
        }
        else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("No Records selected", undefined, undefined);
        }
    }
    addRecordsForForcedIgnoreMatchProcessing(LFRecordIds) {
        let descriptionId = 0;
        let descriptionText = '';
        if (this.selectedReason && this.selectedReason.length > 0) {
            descriptionId = this.selectedReason[0].Id;
            descriptionText = this.selectedReason[0].Name;
        }
        this.ShowSideBarLoader = true;
        this.dashboardservice.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe((response) => {
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                this.reloadGrid();
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
        });
        this.ShowSideBarLoader = false;
    }
    GetBolRecord(InvoiceFtlDetailId) {
        if (InvoiceFtlDetailId != null && InvoiceFtlDetailId != undefined && InvoiceFtlDetailId != '') {
            let selectedLiftFileRecordId = this.selectedLiftFileRecord.LiftFileRecordId;
            let invoiceFtlDetailId = parseInt(InvoiceFtlDetailId);
            let LFRecord = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFRecordGridModel"]();
            LFRecord.LiftFileRecordId = selectedLiftFileRecordId;
            LFRecord.InvFtlDetailId = invoiceFtlDetailId;
            LFRecord.bol = this.selectedLiftFileRecord.bol;
            LFRecord.TerminalName = this.selectedLiftFileRecord.TerminalName;
            LFRecord.TerminalItemCode = this.selectedLiftFileRecord.TerminalItemCode;
            LFRecord.LoadDate = this.selectedLiftFileRecord.LoadDate;
            LFRecord.ProductType = this.selectedLiftFileRecord.ProductType;
            LFRecord.correctedQuantity = this.selectedLiftFileRecord.correctedQuantity;
            LFRecord.IsFromScratchReport = true;
            this.getBolDetailsForResolve(LFRecord);
        }
    }
    initFormData(data) {
        var currObj = this;
        this.bolResolveForm.reset(); //clear previous values
        if ((this.bolResolveForm != null && this.bolResolveForm != undefined) && (data != null && data != undefined)) {
            this.bolResolveForm.get('BolNumber').setValue(data.BolNumber);
            this.bolResolveForm.get('LiftDate').setValue(data.DisplayLiftDate);
            this.bolResolveForm.get('GrossQuantity').setValue(data.GrossQuantity);
            this.bolResolveForm.get('NetQuantity').setValue(data.NetQuantity);
            this.bolResolveForm.get('Notes').setValue(data.Notes);
            this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
            this.bolResolveForm.get('InvoiceFtlDetailId').setValue(data.InvoiceFtlDetailId);
            this.bolResolveForm.get('IsBulkPlantLift').setValue(data.IsBulkPlantLift);
            this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
            this.bolResolveForm.get('BadgeNumber').setValue(data.BadgeNumber);
            if (data.IsBulkPlantLift == true) { // no terminal dropdown for pickup from bulk plants
                this.bolResolveForm.get('SelectedTerminal').clearValidators();
                this.bolResolveForm.get('SelectedTerminal').updateValueAndValidity();
            }
            if (data != null && data.SelectedTerminal != null) {
                this.bolResolveForm.get('SelectedTerminal').setValue(data.SelectedTerminal);
                this.SelectedTerminalList = [];
                this.SelectedTerminalList.push(data.SelectedTerminal);
            }
            if (data != null && data.SelectedFuelType != null) {
                this.bolResolveForm.get('SelectedFuelType').setValue(data.SelectedFuelType);
                this.SelectedFuelTypeList = [];
                this.SelectedFuelTypeList.push(data.SelectedFuelType);
            }
        }
    }
    createPostObject() {
        let inputPostObject = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFBolEditModel"]();
        inputPostObject.BadgeNumber = this.bolResolveForm.get('BadgeNumber').value;
        inputPostObject.BolNumber = this.bolResolveForm.get('BolNumber').value;
        inputPostObject.GrossQuantity = this.bolResolveForm.get('GrossQuantity').value;
        inputPostObject.InvoiceFtlDetailId = this.bolResolveForm.get('InvoiceFtlDetailId').value;
        inputPostObject.IsBulkPlantLift = this.bolResolveForm.get('IsBulkPlantLift').value;
        inputPostObject.LiftRecord.LiftFileRecordId = this.bolResolveForm.get('LIftFileRecordId').value;
        inputPostObject.LiftDate = this.bolResolveForm.get('LiftDate').value;
        inputPostObject.NetQuantity = this.bolResolveForm.get('NetQuantity').value;
        inputPostObject.Notes = this.bolResolveForm.get('Notes').value;
        let SelectedFuelType = this.bolResolveForm.get('SelectedFuelType').value;
        let fuelTypeId = SelectedFuelType[0].Id;
        inputPostObject.FuelTypeId = fuelTypeId;
        let selectedTerminal = this.bolResolveForm.get('SelectedTerminal').value;
        let terminalId = inputPostObject.IsBulkPlantLift ? selectedTerminal.Id : selectedTerminal[0].Id;
        inputPostObject.TerminalId = terminalId;
        return inputPostObject;
    }
    //resetSelections(isReset: boolean) {
    //    if (isReset) {
    //        this.isChecked = false;
    //    }
    //    else {
    //        this.isChecked = true;
    //    }
    //}
    redirectToMyApprovalTab() {
        window.open("Supplier/Exception/Manage", "_blank");
    }
    onSubmit() {
        this.ShowSideBarLoader = true;
        this.bolResolveForm.markAsTouched();
        if (this.bolResolveForm.valid) {
            let requestObj = this.createPostObject();
            if (requestObj != null) {
                this.dashboardservice.saveBolDetailsForResolve(requestObj).subscribe((response) => {
                    if (response.StatusCode == 0) {
                        this.ShowSideBarLoader = false;
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                        this._toggleOpened(false);
                        this.reloadGrid();
                    }
                    else {
                        this.ShowSideBarLoader = false;
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
        this.ShowSideBarLoader = false;
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.bolResolveForm.reset();
        }
    }
    ngOnDestroy() {
        this.dtTrigger.unsubscribe();
    }
    getLFRecordIds() {
        let LFRecordIds = [];
        let table = $('#liftfilerecords-datatable').DataTable();
        var rowcollection = table.$(".dt-checkbox", { "page": "all" });
        rowcollection.each(function (index, elem) {
            if ($(this).is(":checked")) {
                LFRecordIds.push(parseInt($(this).attr('id')));
            }
        });
        return LFRecordIds;
    }
    getPreferencesSetting() {
        if (!this.preferenceSetting) {
            this.dashboardservice.getPreferencesSetting().subscribe(response => {
                this.preferenceSetting = response;
            });
        }
    }
    GetReasonDescriptionList() {
        if (this.reasonList && this.reasonList.length == 0) {
            this.ShowGridLoader = true;
            this.dashboardservice.GetReasonDescriptionList().subscribe((response) => {
                if (response && response.length > 0) {
                    this.reasonList = response;
                }
                this.ShowGridLoader = false;
            });
        }
    }
    submitIgnoreDescription() {
        this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
    }
}
LfvScratchReportComponent.??fac = function LfvScratchReportComponent_Factory(t) { return new (t || LfvScratchReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"])); };
LfvScratchReportComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: LfvScratchReportComponent, selectors: [["app-lfv-scratch-report"]], decls: 116, vars: 21, consts: [[1, "Lfv-resolve-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], [1, "well", "bg-white", "shadow-b", "lfrecord-section"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border", "no-padding"], ["id", "LFrecord", 1, "table-responsive"], ["id", "table-Lfrecord", 1, "table", "table-striped", "table-bordered", "table-hover", "lfvrecord"], [1, "thead-light"], ["class", "row", 4, "ngIf"], ["class", "pr30", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "liftfilerecords-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "BolNumber"], ["data-key", "Terminal"], ["data-key", "Terminals"], ["data-key", "CorrectedQuantity"], ["data-key", "TerminalITemCode"], ["data-key", "ProductType"], ["data-key", "LoadDate"], ["data-key", "RecordDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "Reason"], ["data-key", "ReasonCode"], ["data-key", "ReasonCategory"], ["data-key", "RecordStatus"], ["data-key", "ModifiedBy"], ["data-key", "ModifiedDate"], ["data-key", "LFVResolutionTime"], ["data-key", "SelectAll"], ["type", "checkbox", "id", "select-all-records", "value", "select-all-records", 3, "click"], ["data-key", "Action"], [4, "ngIf"], [1, "col-sm-12", "text-right", "mb25", "btn-wrapper"], [1, "form-group", "col-sm-12"], ["type", "button", "id", "btnCancel", "value", "Cancel", 1, "btn", "btn-default"], ["type", "button", "value", "Ignore", "id", "btnForceIgnoreRecords", 1, "btn", "btn-primary", 3, "click"], ["type", "hidden", "id", "openIgnoreModal2", "data-toggle", "modal", "data-target", "#ignoreModal2"], ["id", "ignoreModal2", "tabindex", "-1", "role", "dialog", "aria-hidden", "true", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], ["aria-hidden", "true"], [1, "modal-body"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "disabled", "click"], ["class", "col-sm-6", 4, "ngIf"], [1, "col-sm-6"], [1, "form-group"], ["id", "select-bol", 1, "form-control", 3, "change"], [3, "value"], [4, "ngFor", "ngForOf"], [3, "value", "selected"], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "col-sm-12", "section-bol-details-edit"], [1, "mt10", "row"], [1, "col-sm-3", "bol"], ["for", "BolNumber"], [1, "color-maroon"], ["formControlName", "InvoiceFtlDetailId", "type", "hidden", 1, "hide-element"], ["formControlName", "LIftFileRecordId", "type", "hidden", 1, "hide-element"], ["formControlName", "BolNumber", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3", "lifdt"], ["for", "LiftDate"], ["name", "LiftDate", "formControlName", "LiftDate", "myDatePicker", "", 1, "form-control", 3, "format", "onDateChange"], [1, "col-sm-3", "grossQty"], ["for", "GrossQuantity"], ["formControlName", "GrossQuantity", 1, "form-control"], [1, "col-sm-3", "netQty"], ["for", "NetQuantity"], ["formControlName", "NetQuantity", 1, "form-control"], [1, "col-sm-3"], ["for", "BadgeNumber"], ["formControlName", "BadgeNumber", 1, "form-control"], [1, "col"], ["class", "col-sm-6 terminal-section", 4, "ngIf"], [1, "col-sm-6", "fuelType"], ["for", "Jobs"], ["formControlName", "SelectedFuelType", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Notes"], ["formControlName", "Notes", 1, "form-control"], [1, "col-sm-12", "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], [1, "col-sm-6", "terminal-section"], ["formControlName", "SelectedTerminal", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "text-center"], ["type", "checkbox", 1, "dt-checkbox", 3, "id", "checked", "value"], ["type", "button", 1, "btn", "btn-link", 3, "click"], ["title", "Resolve Partial Match", 1, "fas", "fa-edit", "fs16"], ["title", "Resolve Exception", 1, "fas", "fa-edit", "fs16"]], template: function LfvScratchReportComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "ng-sidebar-container");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "ng-sidebar", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("openedChange", function LfvScratchReportComponent_Template_ng_sidebar_openedChange_2_listener($event) { return ctx._opened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "a", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_Template_a_click_3_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](4, "i", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "h3", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](6, "Edit BOL Details");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](10, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "table", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](12, "thead", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](14, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](15, "BOL#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](16, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](17, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](18, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](19, "Corrected Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](20, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](21, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](22, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](23, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](24, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](25, "ProductType");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](26, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](31, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](33, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](35, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](36);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](37, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](39, LfvScratchReportComponent_div_39_Template, 2, 1, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](40, LfvScratchReportComponent_content_40_Template, 64, 12, "content", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](41, LfvScratchReportComponent_div_41_Template, 4, 0, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](42, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](43, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](44, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](45, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](46, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](47, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](48, "table", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](49, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](50, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](51, "th", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](52, "CallID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](53, "th", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](54, "Bol#");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](55, "th", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](56, "Terminal Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](57, "th", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](58, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](59, "th", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](60, "Corrected Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](61, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](62, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](63, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](64, "Product Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](65, "th", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](66, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](67, "th", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](68, "Record Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](69, "th", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](70, "CarrierID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](71, "th", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](72, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](73, "th", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](74, "Reason");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](75, "th", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](76, "Reason Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](77, "th", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](78, "Reason Category");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](79, "th", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](80, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](81, "th", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](82, "Modified By");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](83, "th", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](84, "Modified Date (MST)");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](85, "th", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](86, "Resolution Time");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](87, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](88, "SelectAll ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](89, "input", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_Template_input_click_89_listener($event) { return ctx.selectAllRecords($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](90, "th", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](91, "Action");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](92, LfvScratchReportComponent_tbody_92_Template, 2, 1, "tbody", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](93, "div", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](94, "div", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](95, "div", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](96, "input", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](97, "input", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_Template_input_click_97_listener() { return ctx.ValidateForIgnoreMatchProcessing(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](98, LfvScratchReportComponent_div_98_Template, 5, 0, "div", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](99, "div", 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](100, "div", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](101, "div", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](102, "div", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](103, "div", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](104, "h4", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](105, "Select Reason");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](106, "button", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](107, "span", 52);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](108, "\u00D7");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](109, "div", 53);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](110, "ng-multiselect-dropdown", 54);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("ngModelChange", function LfvScratchReportComponent_Template_ng_multiselect_dropdown_ngModelChange_110_listener($event) { return ctx.selectedReason = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](111, "div", 55);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](112, "button", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](113, "Cancel");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](114, "button", 57);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????listener"]("click", function LfvScratchReportComponent_Template_button_click_114_listener() { return ctx.submitIgnoreDescription(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](115, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.bol);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalName);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.correctedQuantity);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalItemCode);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.LoadDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.ProductType);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null && ctx.bolResolveForm.get("InvoiceFtlDetailId").value > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.ShowSideBarLoader);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](44);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.LFRecords == null ? null : ctx.LFRecords.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.ShowGridLoader);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("placeholder", "Select Reason")("settings", ctx.dropdownSettings)("data", ctx.reasonList)("ngModel", ctx.selectedReason);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("disabled", ctx.selectedReason && ctx.selectedReason.length == 0);
    } }, directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["Sidebar"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["??angular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"]], styles: ["aside {\r\n    width: 52% !important;\r\n    z-index: 99 !important;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtc2NyYXRjaC1yZXBvcnQvbGZ2LXNjcmF0Y2gtcmVwb3J0LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IjtBQUNBOzs7RUFHRTs7QUFFRjtJQUNJLHFCQUFxQjtJQUNyQixzQkFBc0I7QUFDMUIiLCJmaWxlIjoic3JjL2FwcC9sZnYtZGFzaGJvYXJkL2xmdi1zY3JhdGNoLXJlcG9ydC9sZnYtc2NyYXRjaC1yZXBvcnQuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIlxyXG4vKmFzaWRlLm5nLXNpZGViYXItLW9wZW5lZCB7XHJcbiAgICB3aWR0aDogODAwcHggIWltcG9ydGFudDtcclxuICAgIHotaW5kZXg6IDMgIWltcG9ydGFudDtcclxufSovXHJcblxyXG46Om5nLWRlZXAgYXNpZGUge1xyXG4gICAgd2lkdGg6IDUyJSAhaW1wb3J0YW50O1xyXG4gICAgei1pbmRleDogOTkgIWltcG9ydGFudDtcclxufSJdfQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](LfvScratchReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-lfv-scratch-report',
                templateUrl: './lfv-scratch-report.component.html',
                styleUrls: ['./lfv-scratch-report.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"] }, { type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/master/master.component.ts":
/*!**********************************************************!*\
  !*** ./src/app/lfv-dashboard/master/master.component.ts ***!
  \**********************************************************/
/*! exports provided: MasterComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterComponent", function() { return MasterComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../left-side-filter/left-side-filter.component */ "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts");
/* harmony import */ var _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../LiftFileModels */ "./src/app/lfv-dashboard/LiftFileModels.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! src/app/declarations.module */ "./src/app/declarations.module.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! angular7-csv/dist/Angular-csv */ "./node_modules/angular7-csv/dist/Angular-csv.js");
/* harmony import */ var angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__);
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var ng_sidebar__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ng-sidebar */ "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
/* harmony import */ var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ng-multiselect-dropdown */ "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _validation_validation_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ../validation/validation.component */ "./src/app/lfv-dashboard/validation/validation.component.ts");
/* harmony import */ var _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ../carrier/carrier.component */ "./src/app/lfv-dashboard/carrier/carrier.component.ts");
/* harmony import */ var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ../../directives/myDateTimePicker */ "./src/app/directives/myDateTimePicker.ts");






















const _c0 = ["btnOpenModal"];
function MasterComponent_div_14_Template(rf, ctx) { if (rf & 1) {
    const _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "a", 92);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_14_Template_a_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r14); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r13.openLFVScratchReportGrid(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 93);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_div_15_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "a", 94);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_15_Template_a_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r16); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r15.openAccrualReportGrid(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 95);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_div_16_div_5_Template(rf, ctx) { if (rf & 1) {
    const _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 101);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "a", 46, 102);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_16_div_5_Template_a_click_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r24); const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r23.toggleRecordSearchControls(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "i", 103);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "input", 105, 106);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function MasterComponent_div_16_div_5_Template_input_ngModelChange_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r24); const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r25.bolSearchQuery = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "div", 104);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "input", 107, 108);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function MasterComponent_div_16_div_5_Template_input_ngModelChange_9_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r24); const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r26.fileNameSearchQuery = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "div", 109);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "button", 110);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_16_div_5_Template_button_click_12_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r24); const ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r27.searchLiftFileRecords(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](13, "Search");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](14, "button", 111, 112);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r18.bolSearchQuery);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngModel", ctx_r18.fileNameSearchQuery);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("hidden", true);
} }
function MasterComponent_div_16_Template(rf, ctx) { if (rf & 1) {
    const _r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "button", 96, 97);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_16_Template_button_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r29); const ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r28.toggleRecordSearchControls(true); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "i", 98);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Search Records");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, MasterComponent_div_16_div_5_Template, 16, 3, "div", 99);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r2.showSearchControls);
} }
function MasterComponent_div_17_Template(rf, ctx) { if (rf & 1) {
    const _r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 91);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "a", 113);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_17_Template_a_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r31); const ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r30.openCarrierBOLReportGrid(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "i", 114);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "a", 115);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_17_Template_a_click_3_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r31); const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r32.openSupplierBOLReportGrid(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](4, "i", 116);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_app_validation_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "app-validation", 117);
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("LFValidationList", ctx_r4.LFValidationList);
} }
function MasterComponent_app_carrier_performace_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](0, "app-carrier-performace", 117);
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("LFValidationList", ctx_r5.LFValidationList);
} }
const _c1 = function (a0) { return { "highlight-record": a0 }; };
const _c2 = function (a0, a1) { return { "highlight-record": a0, "hide-element": a1 }; };
const _c3 = function (a0) { return { "hide-element": a0 }; };
const _c4 = function (a0, a1) { return { "hide-element": a0, "highlight-record": a1 }; };
function MasterComponent_tr_110_Template(rf, ctx) { if (rf & 1) {
    const _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](24);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](29, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](30);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](32);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "td", 118);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "td", 119);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](36, "input", 120);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](37, "td", 119);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](38, "button", 121);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_tr_110_Template_button_click_38_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r35); const item_r33 = ctx.$implicit; const ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r34.getBolDetailsForResolve(item_r33); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](39, "i", 122);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](40, "td", 119);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "button", 121);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_tr_110_Template_button_click_41_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r35); const item_r33 = ctx.$implicit; const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r36.editLiftFileRecord(item_r33); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](42, "i", 123);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const item_r33 = ctx.$implicit;
    const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](40, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.bol);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](42, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.TerminalName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](44, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.Terminals);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](46, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.correctedQuantity);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](48, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.TerminalItemCode);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](50, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.ProductType);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](52, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.LoadDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](54, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.RecordDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](56, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.CarrierID);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](58, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.CarrierName);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](60, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.Reason);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](62, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PendingMatch));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r33 == null ? null : item_r33.ReasonCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](65, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PendingMatch));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", item_r33 == null ? null : item_r33.ReasonCategory, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](68, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.Username);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](70, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.ModifiedDate);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](72, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.LFVResolutionTime);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](74, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean ? false : true));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](item_r33 == null ? null : item_r33.TimeToBol);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](77, _c3, !(ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.UnMatched || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate)));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate"]("id", item_r33.LiftFileRecordId);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????propertyInterpolate"]("value", item_r33.LiftFileRecordId);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("checked", ctx_r6.isChecked);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](79, _c3, ctx_r6.gridType != ctx_r6.LFVRecordStatus.PartialMatch));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction2"](81, _c4, ctx_r6.gridType == ctx_r6.LFVRecordStatus.IgnoreMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.ForcedIgnore || !item_r33.IsAdminUser, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean && !item_r33.IsRecordPushedToExternalApi));
} }
function MasterComponent_div_111_input_3_Template(rf, ctx) { if (rf & 1) {
    const _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "input", 128);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_111_input_3_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r40); const ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r39.ValidateForIgnoreMatchProcessing("ignore"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_div_111_input_4_Template(rf, ctx) { if (rf & 1) {
    const _r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "input", 129);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_div_111_input_4_Template_input_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r42); const ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r41.ValidateForIgnoreMatchProcessing("reprocess"); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_div_111_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 124);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 125);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](3, MasterComponent_div_111_input_3_Template, 1, 0, "input", 126);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](4, MasterComponent_div_111_input_4_Template, 1, 0, "input", 127);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.gridType == ctx_r7.LFVRecordStatus.NoMatch || ctx_r7.gridType == ctx_r7.LFVRecordStatus.UnMatched);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r7.gridType == ctx_r7.LFVRecordStatus.Duplicate || ctx_r7.gridType == ctx_r7.LFVRecordStatus.UnMatched);
} }
function MasterComponent_div_151_div_1_ng_container_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "option", 135);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
} if (rf & 2) {
    const bol_r45 = ctx.$implicit;
    const ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", bol_r45.Id)("selected", bol_r45.Id == ctx_r44.bolResolveForm.get("InvoiceFtlDetailId").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", bol_r45.Name, " ");
} }
function MasterComponent_div_151_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 131);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "select", 133);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("change", function MasterComponent_div_151_div_1_Template_select_change_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r47); const ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r46.GetBolRecord($event.target.value); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "option", 134);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Select BOL-Product to edit ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, MasterComponent_div_151_div_1_ng_container_5_Template, 3, 3, "ng-container", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("value", null);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r43.InvoiceFtlDetailIdList);
} }
function MasterComponent_div_151_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_div_151_div_1_Template, 6, 2, "div", 130);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r8.InvoiceFtlDetailIdList != null && ctx_r8.InvoiceFtlDetailIdList.length > 0);
} }
function MasterComponent_content_152_div_14_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " BOL/LiftTicket# is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_14_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r48.bolResolveForm.get("BolNumber").errors.required);
} }
function MasterComponent_content_152_div_22_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Lift Date is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_22_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r49.bolResolveForm.get("LiftDate").errors.required);
} }
function MasterComponent_content_152_div_30_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Gross quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_30_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_30_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r50.bolResolveForm.get("GrossQuantity").errors.required);
} }
function MasterComponent_content_152_div_38_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Net quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_38_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_38_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r51.bolResolveForm.get("NetQuantity").errors.required);
} }
function MasterComponent_content_152_div_40_div_5_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Terminal is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_40_div_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_40_div_5_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r58.bolResolveForm.get("SelectedTerminal").errors.required);
} }
function MasterComponent_content_152_div_40_Template(rf, ctx) { if (rf & 1) {
    const _r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 168);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "label", 158);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](3, "Terminal Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "ng-multiselect-dropdown", 169);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function MasterComponent_content_152_div_40_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r61); const ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2); return ctx_r60.SelectedTerminalList = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](5, MasterComponent_content_152_div_40_div_5_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Terminal")("settings", ctx_r52.multiselectSettingsById)("data", ctx_r52.TerminalList)("ngModel", ctx_r52.SelectedTerminalList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r52.bolResolveForm.get("SelectedTerminal").invalid && (ctx_r52.bolResolveForm.get("SelectedTerminal").dirty || ctx_r52.bolResolveForm.get("SelectedTerminal").touched));
} }
function MasterComponent_content_152_div_46_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Fuel Type is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_152_div_46_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_152_div_46_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r53.bolResolveForm.get("SelectedFuelType").errors.required);
} }
function MasterComponent_content_152_Template(rf, ctx) { if (rf & 1) {
    const _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "content", 136);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "form", 137);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngSubmit", function MasterComponent_content_152_Template_form_ngSubmit_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r63.onSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 138);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 139);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 140);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](7, "label", 141);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](8, "BOL/LiftTicket#");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](9, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](10, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](11, "input", 143);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](12, "input", 144);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](13, "input", 145);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, MasterComponent_content_152_div_14_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "div", 147);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](16, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "label", 148);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18, "Lift Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "input", 149);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function MasterComponent_content_152_Template_input_onDateChange_21_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r65.bolResolveForm.get("LiftDate").setValue($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](22, MasterComponent_content_152_div_22_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](23, "div", 150);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](24, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "label", 151);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](26, "Gross Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](28, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](29, "input", 152);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](30, MasterComponent_content_152_div_30_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](31, "div", 153);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](32, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "label", 154);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "Net Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](36, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](37, "input", 155);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, MasterComponent_content_152_div_38_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](40, MasterComponent_content_152_div_40_Template, 6, 5, "div", 156);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 157);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "label", 158);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Fuel");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "ng-multiselect-dropdown", 159);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function MasterComponent_content_152_Template_ng_multiselect_dropdown_ngModelChange_45_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r66.SelectedFuelTypeList = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](46, MasterComponent_content_152_div_46_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](47, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "div", 1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 160);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "label", 161);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](52, "Badge#");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](53, "input", 162);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "div", 160);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "label", 163);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](57, "Notes");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](58, "textarea", 164);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "div", 165);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](60, "button", 166);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_content_152_Template_button_click_60_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r64); const ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r67._toggleOpened(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](61, "Cancel");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "button", 167);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](63, "Save &Re-Submit");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx_r9.bolResolveForm);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.bolResolveForm.get("BolNumber").invalid && (ctx_r9.bolResolveForm.get("BolNumber").dirty || ctx_r9.bolResolveForm.get("BolNumber").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.bolResolveForm.get("LiftDate").invalid && (ctx_r9.bolResolveForm.get("LiftDate").dirty || ctx_r9.bolResolveForm.get("LiftDate").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.bolResolveForm.get("GrossQuantity").invalid && (ctx_r9.bolResolveForm.get("GrossQuantity").dirty || ctx_r9.bolResolveForm.get("GrossQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.bolResolveForm.get("NetQuantity").invalid && (ctx_r9.bolResolveForm.get("NetQuantity").dirty || ctx_r9.bolResolveForm.get("NetQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", !ctx_r9.bolResolveForm.get("IsBulkPlantLift").value);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Fuel")("settings", ctx_r9.multiselectSettingsById)("data", ctx_r9.FuelTypeList)("ngModel", ctx_r9.SelectedFuelTypeList);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r9.bolResolveForm.get("SelectedFuelType").invalid && (ctx_r9.bolResolveForm.get("SelectedFuelType").dirty || ctx_r9.bolResolveForm.get("SelectedFuelType").touched));
} }
function MasterComponent_tbody_202_tr_1_Template(rf, ctx) { if (rf & 1) {
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
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](17, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](18);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](20);
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
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const record_r69 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.CallId, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](record_r69.bol);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.TerminalName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.Terminals, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.FileName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.TerminalItemCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.LoadDate, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.RecordDate, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.CarrierID, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.CarrierName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.recordStatus, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.Reason, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.ReasonCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.ReasonCategory, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.Username, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate1"](" ", record_r69.ModifiedDate, " ");
} }
function MasterComponent_tbody_202_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_tbody_202_tr_1_Template, 33, 16, "tr", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx_r10.LiftFilesearchResults);
} }
function MasterComponent_content_210_span_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_11_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " BOL# is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_11_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r72.LFVRecordEditForm.get("BolNumber").errors.required);
} }
function MasterComponent_content_210_span_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_18_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Terminal Code is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_18_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_18_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r74.LFVRecordEditForm.get("TerminalCode").errors.required);
} }
function MasterComponent_content_210_span_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_25_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Terminal Item Code is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_25_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_25_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r76.LFVRecordEditForm.get("TerminalItemCode").errors.required);
} }
function MasterComponent_content_210_span_30_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_32_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " CIN is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_32_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_32_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r78.LFVRecordEditForm.get("CIN").errors.required);
} }
function MasterComponent_content_210_span_38_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_40_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Carrier Name is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_40_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_40_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r80 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r80.LFVRecordEditForm.get("CarrierName").errors.required);
} }
function MasterComponent_content_210_span_45_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_47_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Load Date is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_47_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_47_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r82.LFVRecordEditForm.get("LoadDate").errors.required);
} }
function MasterComponent_content_210_span_52_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_54_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Corrected quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_54_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_54_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_54_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, MasterComponent_content_210_div_54_div_2_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r84.LFVRecordEditForm.get("CorrectedQuantity").errors.required);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r84.LFVRecordEditForm.get("CorrectedQuantity").errors.pattern);
} }
function MasterComponent_content_210_span_59_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "span", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, "*");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_61_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Invalid ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_61_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](1, " Gross Quantity is required. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
function MasterComponent_content_210_div_61_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 142);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](1, MasterComponent_content_210_div_61_div_1_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](2, MasterComponent_content_210_div_61_div_2_Template, 2, 0, "div", 55);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r86.LFVRecordEditForm.get("GrossQuantity").errors.pattern);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r86.LFVRecordEditForm.get("GrossQuantity").errors.required);
} }
function MasterComponent_content_210_Template(rf, ctx) { if (rf & 1) {
    const _r98 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????getCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "content", 136);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "form", 137);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngSubmit", function MasterComponent_content_210_Template_form_ngSubmit_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r98); const ctx_r97 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r97.onRecordEditSubmit(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 170);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 139);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "label", 141);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](7, "BOL#");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](8, MasterComponent_content_210_span_8_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](9, "input", 144);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](10, "input", 172);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](11, MasterComponent_content_210_div_11_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](12, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](14, "label", 173);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](15, "Terminal Code");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, MasterComponent_content_210_span_16_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](17, "input", 174);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](18, MasterComponent_content_210_div_18_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "label", 175);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](22, "Terminal Item Code");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, MasterComponent_content_210_span_23_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](24, "input", 176);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](25, MasterComponent_content_210_div_25_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "label", 177);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](29, "CIN");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](30, MasterComponent_content_210_span_30_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](31, "input", 178);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](32, MasterComponent_content_210_div_32_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "div", 139);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](34, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](35, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "label", 179);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](37, "Carrier Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](38, MasterComponent_content_210_span_38_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](39, "input", 180);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](40, MasterComponent_content_210_div_40_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](41, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](43, "label", 181);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](44, "Load Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](45, MasterComponent_content_210_span_45_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](46, "input", 182);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("onDateChange", function MasterComponent_content_210_Template_input_onDateChange_46_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r98); const ctx_r99 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r99.LFVRecordEditForm.get("LoadDate").setValue($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](47, MasterComponent_content_210_div_47_Template, 2, 1, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](49, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](50, "label", 183);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](51, "Corrected Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](52, MasterComponent_content_210_span_52_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](53, "input", 184);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](54, MasterComponent_content_210_div_54_Template, 3, 2, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](55, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "label", 151);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](58, "Gross Quantity");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](59, MasterComponent_content_210_span_59_Template, 2, 0, "span", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](60, "input", 185);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](61, MasterComponent_content_210_div_61_Template, 3, 2, "div", 146);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "div", 139);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "label", 186);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](66, "Product Type");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](67, "input", 187);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](68, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](69, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](70, "label", 188);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](71, "Record Date");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](72, "input", 189);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "div", 171);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](74, "div", 132);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](75, "label", 190);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](76, "CarrierID");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](77, "input", 191);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](78, "div", 165);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](79, "button", 166);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_content_210_Template_button_click_79_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["????restoreView"](_r98); const ctx_r100 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"](); return ctx_r100._EditToggleOpened(false); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](80, "Cancel");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](81, "button", 167);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](82, "Save &Re-Submit");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("formGroup", ctx_r11.LFVRecordEditForm);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsBolReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsBolReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("BolNumber").invalid && (ctx_r11.LFVRecordEditForm.get("BolNumber").dirty || ctx_r11.LFVRecordEditForm.get("BolNumber").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsTerminalCodeReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsTerminalCodeReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("TerminalCode").invalid && (ctx_r11.LFVRecordEditForm.get("TerminalCode").dirty || ctx_r11.LFVRecordEditForm.get("TerminalCode").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsTermItemCodeReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsTermItemCodeReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("TerminalItemCode").invalid && (ctx_r11.LFVRecordEditForm.get("TerminalItemCode").dirty || ctx_r11.LFVRecordEditForm.get("TerminalItemCode").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsCINReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsCINReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("CIN").invalid && (ctx_r11.LFVRecordEditForm.get("CIN").dirty || ctx_r11.LFVRecordEditForm.get("CIN").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsCarrierNameReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsCarrierNameReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("CarrierName").invalid && (ctx_r11.LFVRecordEditForm.get("CarrierName").dirty || ctx_r11.LFVRecordEditForm.get("CarrierName").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsLoadDateReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("format", "MM/DD/YYYY")("readonly", !ctx_r11.LfvValidationParameters.IsLoadDateReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("LoadDate").invalid && (ctx_r11.LFVRecordEditForm.get("LoadDate").dirty || ctx_r11.LFVRecordEditForm.get("LoadDate").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsCorrectedQtyRes || ctx_r11.LfvValidationParameters.IsGrossReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsCorrectedQtyOrGrossReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").invalid && (ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").dirty || ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LfvValidationParameters.IsGrossReq || ctx_r11.LfvValidationParameters.IsCorrectedQtyRes);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", !ctx_r11.LfvValidationParameters.IsCorrectedQtyOrGrossReq);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx_r11.LFVRecordEditForm.get("GrossQuantity").invalid && (ctx_r11.LFVRecordEditForm.get("GrossQuantity").dirty || ctx_r11.LFVRecordEditForm.get("GrossQuantity").touched));
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", true);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", true);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("readonly", true);
} }
function MasterComponent_div_211_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 192);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 193);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](2, "div", 194);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 195);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](4, "Loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
} }
class MasterComponent {
    constructor(_lfvService, fb) {
        this._lfvService = _lfvService;
        this.fb = fb;
        this.viewType = 1;
        this.gridType = 0;
        this.LFValidationList = [];
        this.LFVRecordGrid = [];
        this.IsLoading = false;
        this.isValidationCarrier = false;
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subject"]();
        this.dtOptions = {};
        this.isChecked = false;
        this.LFVRecordStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["LFVRecordStatus"];
        //side bar related variables
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedTerminalList = [];
        this.SelectedFuelTypeList = [];
        //search Liftfile records variables
        this.bolSearchQuery = "";
        this.fileNameSearchQuery = "";
        this.showSearchControls = false;
        this.showSearchBtn = false;
        this.LiftFilesearchResults = [];
        this.searchGridDtOptions = {};
        this.searchGridDtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subject"]();
        this.csvOptions = {
            fieldSeparator: ',',
            quoteStrings: '"',
            decimalseparator: '.',
            showLabels: true,
            showTitle: false,
            title: 'LFV' + new Date(),
            useBom: true,
            noDownload: false,
            headers: ["BOL", "Terminal Code", "Terminal", "Corrected Quantity", "Terminal Item Code", "Product Type", "Load Date", "Record Date", "Carrier ID", "Carrier Name", "Reasons", "Reason Code", "Reason Category", "Modified By", "Modified Date", "Resolution Time", "Time to BOL"]
        };
        this.QuantityPattern = "^(\0*[1-9]*[1-9][0-9]*(\.[0-9]+)?|[0]*\.[0-9]*[1-9][0-9]*)$";
        this._EditOpened = false;
        this._EditAnimate = true;
        this._EditPositionNum = 1;
        this._EditPOSITIONS = ['left', 'right', 'top', 'bottom'];
        //ignore by reason
        this.preferenceSetting = null;
        this.selectedReason = [];
        this.reasonList = [];
        this.dropdownSettings = { singleSelection: true, idField: 'Id', textField: 'Name', allowSearchFilter: true };
    }
    ngOnInit() {
        this.getPreferencesSetting();
        this.multiselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.initializeGrid();
        this.bolResolveForm = this.buildForm();
        this.LFVRecordEditForm = this.buildLFVRecordEditForm();
    }
    buildForm() {
        let fg = this.fb.group({
            BolNumber: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            LiftDate: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            GrossQuantity: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            NetQuantity: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            BadgeNumber: this.fb.control(''),
            SelectedTerminal: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            SelectedFuelType: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]),
            Notes: this.fb.control(''),
            LIftFileRecordId: this.fb.control(''),
            InvoiceFtlDetailId: this.fb.control(''),
            IsBulkPlantLift: this.fb.control('')
        });
        this.SelectedFuelTypeList = [];
        this.SelectedTerminalList = [];
        return fg;
    }
    initFormData(data) {
        this.bolResolveForm.reset(); //clear previous values
        if ((this.bolResolveForm != null && this.bolResolveForm != undefined) && (data != null && data != undefined)) {
            this.bolResolveForm.get('BolNumber').setValue(data.BolNumber);
            this.bolResolveForm.get('LiftDate').setValue(data.DisplayLiftDate);
            this.bolResolveForm.get('GrossQuantity').setValue(data.GrossQuantity);
            this.bolResolveForm.get('NetQuantity').setValue(data.NetQuantity);
            this.bolResolveForm.get('Notes').setValue(data.Notes);
            this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
            this.bolResolveForm.get('InvoiceFtlDetailId').setValue(data.InvoiceFtlDetailId);
            this.bolResolveForm.get('IsBulkPlantLift').setValue(data.IsBulkPlantLift);
            this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
            this.bolResolveForm.get('BadgeNumber').setValue(data.BadgeNumber);
            if (data.IsBulkPlantLift == true) { // no terminal dropdown for pickup from bulk plants
                this.bolResolveForm.get('SelectedTerminal').clearValidators();
                this.bolResolveForm.get('SelectedTerminal').updateValueAndValidity();
            }
            if (data != null && data.SelectedTerminal != null) {
                this.bolResolveForm.get('SelectedTerminal').setValue(data.SelectedTerminal);
                this.SelectedTerminalList = [];
                this.SelectedTerminalList.push(data.SelectedTerminal);
            }
            if (data != null && data.SelectedFuelType != null) {
                this.bolResolveForm.get('SelectedFuelType').setValue(data.SelectedFuelType);
                this.SelectedFuelTypeList = [];
                this.SelectedFuelTypeList.push(data.SelectedFuelType);
            }
        }
    }
    createPostObject() {
        let inputPostObject = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFBolEditModel"]();
        inputPostObject.BadgeNumber = this.bolResolveForm.get('BadgeNumber').value;
        inputPostObject.BolNumber = this.bolResolveForm.get('BolNumber').value;
        inputPostObject.GrossQuantity = this.bolResolveForm.get('GrossQuantity').value;
        inputPostObject.InvoiceFtlDetailId = this.bolResolveForm.get('InvoiceFtlDetailId').value;
        inputPostObject.IsBulkPlantLift = this.bolResolveForm.get('IsBulkPlantLift').value;
        inputPostObject.LiftRecord.LiftFileRecordId = this.bolResolveForm.get('LIftFileRecordId').value;
        inputPostObject.LiftDate = this.bolResolveForm.get('LiftDate').value;
        inputPostObject.NetQuantity = this.bolResolveForm.get('NetQuantity').value;
        inputPostObject.Notes = this.bolResolveForm.get('Notes').value;
        let SelectedFuelType = this.bolResolveForm.get('SelectedFuelType').value;
        let fuelTypeId = SelectedFuelType[0].Id;
        inputPostObject.FuelTypeId = fuelTypeId;
        let selectedTerminal = this.bolResolveForm.get('SelectedTerminal').value;
        let terminalId = inputPostObject.IsBulkPlantLift ? selectedTerminal.Id : selectedTerminal[0].Id;
        inputPostObject.TerminalId = terminalId;
        return inputPostObject;
    }
    initializeGrid() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'LiftFileRecords', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'LiftFileRecords', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            fixedHeader: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    ngAfterViewInit() {
        this.getLfvData();
    }
    changeViewType(value) {
        this.viewType = value;
        if (value == 1)
            this.isValidationCarrier = false;
        else
            this.isValidationCarrier = true;
        this.getLfvData();
    }
    getLfvData() {
        this.LFValidationList = [];
        let ids = [];
        let carrierOrderIds = "";
        if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
        }
        else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name); });
            carrierOrderIds = ids.join();
        }
        this.IsLoading = true;
        this._lfvService.getLFValidationGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, isCarrierPerFormanceDashboard: this.isValidationCarrier, carrierIds: carrierOrderIds, isMatchingWindow: this.filterComponent.isMatchingWindow }).subscribe((res) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (res)
                this.LFValidationList = res;
            else
                this.LFValidationList = [];
            this.IsLoading = false;
        }));
    }
    OnSearch($event) {
        if ($event) {
            try {
                $("#liftfilerecords-datatable").DataTable().clear().destroy();
                this.gridType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["LFVRecordStatus"].None;
                this.LFVRecordGrid = [];
                // this.dtTrigger.next();
                this.getLfvData();
            }
            catch (e) {
            }
        }
    }
    changeGridType(status) {
        this.gridType = status;
        this.getLfvFilterGrid(status);
    }
    getLfvFilterGrid(status) {
        // if ((this.datatableElement && this.datatableElement.dtInstance)) {
        //   this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
        // }
        try {
            $("#liftfilerecords-datatable").DataTable().clear().destroy();
        }
        catch (e) {
        }
        this.IsLoading = true;
        this.LFVRecordGrid = [];
        let ids = [];
        let carrierOrderIds = "";
        if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
        }
        else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name); });
            carrierOrderIds = ids.join();
        }
        this._lfvService.getLFVRecordGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, recordStatus: status, isMatchingWindow: this.filterComponent.isMatchingWindow, carrierIds: carrierOrderIds }).subscribe((res) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (res) {
                this.LFVRecordGrid = yield res;
                if (this.LFVRecordGrid != null && this.LFVRecordGrid.length > 0) {
                    this.isAdminUser = this.LFVRecordGrid[0].IsAdminUser;
                    this.LfvValidationParameters = this.LFVRecordGrid[0].LfvValidationParameters;
                }
            }
            else
                this.LFVRecordGrid = [];
            this.dtTrigger.next();
            this.IsLoading = false;
        }));
    }
    openLFVScratchReportGrid() {
        window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
    }
    openAccrualReportGrid() {
        window.open("Supplier/LiftFile/LFVAccrualReport", "_blank");
    }
    selectAllRecords(eventData) {
        if (eventData != null && eventData != undefined) {
            if (eventData.target.checked) {
                this.isChecked = true;
            }
            else {
                this.isChecked = false;
            }
        }
    }
    getLFRecordIds() {
        let LFRecordIds = [];
        let table = $('#liftfilerecords-datatable').DataTable();
        var rowcollection = table.$(".dt-checkbox", { "page": "all" });
        rowcollection.each(function (index, elem) {
            if ($(this).is(":checked")) {
                LFRecordIds.push(parseInt($(this).attr('id')));
            }
        });
        return LFRecordIds;
    }
    getPreferencesSetting() {
        if (!this.preferenceSetting) {
            this._lfvService.getPreferencesSetting().subscribe(response => {
                this.preferenceSetting = response;
            });
        }
    }
    GetReasonDescriptionList() {
        if (this.reasonList && this.reasonList.length == 0) {
            this.IsLoading = true;
            this._lfvService.GetReasonDescriptionList().subscribe((response) => {
                if (response && response.length > 0) {
                    this.reasonList = response;
                }
                this.IsLoading = false;
            });
        }
    }
    submitIgnoreDescription() {
        this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
    }
    ValidateForIgnoreMatchProcessing(status) {
        let LFRecordIds = this.getLFRecordIds();
        this.selectedReason = [];
        if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {
            if (status == 'ignore') {
                if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
                    this.GetReasonDescriptionList();
                    document.getElementById('openIgnoreModal').click();
                }
                else {
                    this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
                }
            }
            else if (status == 'reprocess')
                this.addUnmatchedRecordForReProcessing(LFRecordIds);
        }
        else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("No Records selected", undefined, undefined);
        }
    }
    addRecordsForForcedIgnoreMatchProcessing(LFRecordIds) {
        let descriptionId = 0;
        let descriptionText = '';
        if (this.selectedReason && this.selectedReason.length > 0) {
            descriptionId = this.selectedReason[0].Id;
            descriptionText = this.selectedReason[0].Name;
        }
        this.IsLoading = true;
        this._lfvService.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe((response) => {
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                this.getLfvFilterGrid(this.gridType);
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
        });
        this.IsLoading = true;
    }
    addUnmatchedRecordForReProcessing(LFRecordIds) {
        this.IsLoading = true;
        this._lfvService.addUnmatchedRecordForReProcessing(LFRecordIds).subscribe((response) => {
            if (response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                this.getLfvFilterGrid(this.gridType);
            }
            else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
        });
        this.IsLoading = true;
    }
    getBolDetailsForResolve(lfRecord) {
        this.IsLoading = true;
        lfRecord.IsFromScratchReport = true;
        this._lfvService.getBolDetailsForResolve(lfRecord).subscribe((data) => {
            if (data) {
                this._toggleOpened(true);
                this.selectedLiftFileRecord = data.LiftRecord;
                this.TerminalList = data.TerminalList;
                this.FuelTypeList = data.FuelTypeList;
                this.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;
                this.initFormData(data);
                this.IsLoading = false;
            }
        });
    }
    onSubmit() {
        this.IsLoading = true;
        this.bolResolveForm.markAsTouched();
        if (this.bolResolveForm.valid) {
            let requestObj = this.createPostObject();
            if (requestObj != null) {
                this._lfvService.saveBolDetailsForResolve(requestObj).subscribe((response) => {
                    if (response.StatusCode == 0) {
                        this.IsLoading = false;
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                        this._toggleOpened(false);
                        this.getLfvFilterGrid(this.gridType);
                    }
                    else {
                        this.IsLoading = false;
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
        this.IsLoading = false;
    }
    _toggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.bolResolveForm.reset();
        }
    }
    GetBolRecord(InvoiceFtlDetailId) {
        if (InvoiceFtlDetailId != null && InvoiceFtlDetailId != undefined && InvoiceFtlDetailId != '') {
            let selectedLiftFileRecordId = this.selectedLiftFileRecord.LiftFileRecordId;
            let invoiceFtlDetailId = parseInt(InvoiceFtlDetailId);
            let LFRecord = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFRecordGridModel"]();
            LFRecord.LiftFileRecordId = selectedLiftFileRecordId;
            LFRecord.InvFtlDetailId = invoiceFtlDetailId;
            LFRecord.bol = this.selectedLiftFileRecord.bol;
            LFRecord.TerminalName = this.selectedLiftFileRecord.TerminalName;
            LFRecord.TerminalItemCode = this.selectedLiftFileRecord.TerminalItemCode;
            LFRecord.LoadDate = this.selectedLiftFileRecord.LoadDate;
            LFRecord.ProductType = this.selectedLiftFileRecord.ProductType;
            LFRecord.correctedQuantity = this.selectedLiftFileRecord.correctedQuantity;
            LFRecord.IsFromScratchReport = true;
            this.getBolDetailsForResolve(LFRecord);
        }
    }
    openSupplierBOLReportGrid() {
        window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
    }
    openCarrierBOLReportGrid() {
        window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
    }
    searchLiftFileRecords() {
        let bolQuery = this.bolSearchQuery;
        let fileNameQuery = this.fileNameSearchQuery;
        if ((bolQuery == "" || bolQuery == null || bolQuery == undefined) &&
            (fileNameQuery == "" || fileNameQuery == undefined || fileNameQuery == null)) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Please provide either Bol# or Filename", undefined, undefined);
        }
        else {
            let exportColumns = { columns: ':visible' };
            this.searchGridDtOptions = {
                dom: '<"html5buttons"B>lTfgitp',
                buttons: [
                    { extend: 'colvis' },
                    { extend: 'copy', exportOptions: exportColumns },
                    { extend: 'csv', title: 'Lift File Records', exportOptions: exportColumns },
                    { extend: 'pdf', title: 'Lift File Records', orientation: 'landscape', exportOptions: exportColumns },
                    { extend: 'print', exportOptions: exportColumns }
                ],
                pagingType: 'first_last_numbers',
                fixedHeader: false,
                pageLength: 10,
                lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            };
            let bolNumber = (bolQuery == null || bolQuery == undefined) ? "" : bolQuery.trim();
            let fileName = (fileNameQuery == null || fileNameQuery == undefined) ? "" : fileNameQuery.trim();
            this.IsLoading = true;
            this._lfvService.getLFSearchRecords(bolNumber, fileName).subscribe(data => {
                let el = this.btnOpenModal.nativeElement;
                el.click();
                $("#liftfileSearchrecords-datatable").DataTable().clear().destroy();
                this.LiftFilesearchResults = data;
                this.searchGridDtTrigger.next();
                this.IsLoading = false;
            });
        }
    }
    toggleRecordSearchControls(shouldShow) {
        this.bolSearchQuery = "";
        this.fileNameSearchQuery = "";
        this.showSearchControls = shouldShow;
        this.showSearchBtn = shouldShow;
    }
    OnExport(status) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            this.generateCSV(status);
        });
    }
    generateCSV(status) {
        this.IsLoading = true;
        var exportData = [];
        let ids = [];
        let carrierOrderIds = "";
        if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
        }
        else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name); });
            carrierOrderIds = ids.join();
        }
        this._lfvService.getLFVRecordGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, recordStatus: status, isMatchingWindow: this.filterComponent.isMatchingWindow, carrierIds: carrierOrderIds }).subscribe((res) => Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            if (res)
                exportData = res.map(m => {
                    return {
                        bol: m.bol, TerminalName: m.TerminalName, Terminal: m.Terminals, correctedQuantity: m.correctedQuantity,
                        TerminalItemCode: m.TerminalItemCode, ProductType: m.ProductType, LoadDate: m.LoadDate, RecordDate: m.RecordDate,
                        CarrierID: m.CarrierID, CarrierName: m.CarrierName, Reason: m.Reason,
                        ReasonCode: m.ReasonCode, ReasonCategory: m.ReasonCategory, Username: m.Username, ModifiedDate: m.ModifiedDate, LFVResolutionTime: m.LFVResolutionTime, TimeToBol: m.TimeToBol
                    };
                });
            else
                exportData = [];
            new angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__["AngularCsv"](exportData, 'LFV_' + new Date(), this.csvOptions);
            this.IsLoading = false;
        }));
    }
    _EditToggleOpened(shouldOpen) {
        if (shouldOpen) {
            this._EditOpened = true;
        }
        else {
            this._EditOpened = !this._EditOpened;
            this.LFVRecordEditForm.reset();
        }
    }
    editLiftFileRecord(record) {
        if (record != null) {
            this.IsLoading = true;
            this._EditToggleOpened(true);
            this.initRecordEditForm(record);
            this.IsLoading = false;
        }
    }
    buildLFVRecordEditForm() {
        let formGroup = this.fb.group({
            LIftFileRecordId: this.fb.control(''),
            BolNumber: this.fb.control(''),
            TerminalCode: this.fb.control(''),
            TerminalItemCode: this.fb.control(''),
            LoadDate: this.fb.control(''),
            CorrectedQuantity: this.fb.control(''),
            GrossQuantity: this.fb.control(''),
            ProductType: this.fb.control(''),
            RecordDate: this.fb.control(''),
            CarrierId: this.fb.control(''),
            CIN: this.fb.control(''),
            CarrierName: this.fb.control('')
        });
        return formGroup;
    }
    initRecordEditForm(record) {
        this.LFVRecordEditForm.reset();
        if (record != null && record != undefined) {
            this.LFVRecordEditForm.get('LIftFileRecordId').setValue(record.LiftFileRecordId);
            this.LFVRecordEditForm.get('BolNumber').setValue(record.bol);
            this.LFVRecordEditForm.get('TerminalCode').setValue(record.TerminalName);
            this.LFVRecordEditForm.get('TerminalItemCode').setValue(record.TerminalItemCode);
            this.LFVRecordEditForm.get('LoadDate').setValue(record.LoadDate);
            this.LFVRecordEditForm.get('CorrectedQuantity').setValue(record.correctedQuantity);
            this.LFVRecordEditForm.get('GrossQuantity').setValue(record.GrossQuantity);
            this.LFVRecordEditForm.get('ProductType').setValue(record.ProductType);
            this.LFVRecordEditForm.get('RecordDate').setValue(record.RecordDate);
            this.LFVRecordEditForm.get('CarrierId').setValue(record.CarrierID);
            this.LFVRecordEditForm.get('CIN').setValue(record.CIN);
            this.LFVRecordEditForm.get('CarrierName').setValue(record.CarrierName);
            if (this.LfvValidationParameters != null) {
                if (this.LfvValidationParameters.IsBolReq) {
                    this.LFVRecordEditForm.controls['BolNumber'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['BolNumber'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCorrectedQtyRes || this.LfvValidationParameters.IsGrossReq) {
                    this.LfvValidationParameters.IsCorrectedQtyOrGrossReq = true;
                    this.LFVRecordEditForm.controls['CorrectedQuantity'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].pattern(this.QuantityPattern)]);
                    this.LFVRecordEditForm.controls['CorrectedQuantity'].updateValueAndValidity();
                    this.LFVRecordEditForm.controls['GrossQuantity'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].pattern(this.QuantityPattern)]);
                    this.LFVRecordEditForm.controls['GrossQuantity'].updateValueAndValidity();
                }
                //if (this.LfvValidationParameters.IsGrossReq) {
                //}
                if (this.LfvValidationParameters.IsLoadDateReq) {
                    this.LFVRecordEditForm.controls['LoadDate'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['LoadDate'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsTerminalCodeReq) {
                    this.LFVRecordEditForm.controls['TerminalCode'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['TerminalCode'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsTermItemCodeReq) {
                    this.LFVRecordEditForm.controls['TerminalItemCode'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['TerminalItemCode'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCINReq) {
                    this.LFVRecordEditForm.controls['CIN'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['CIN'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCarrierNameReq) {
                    this.LFVRecordEditForm.controls['CarrierName'].setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_7__["Validators"].required]);
                    this.LFVRecordEditForm.controls['CarrierName'].updateValueAndValidity();
                }
            }
        }
    }
    onRecordEditSubmit() {
        this.LFVRecordEditForm.markAllAsTouched();
        if (this.LFVRecordEditForm.valid) {
            this.IsLoading = true;
            let values = this.LFVRecordEditForm.value;
            if (values != null) {
                let data = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFRecordGridModel"]();
                data.Terminal = this.LFVRecordEditForm.get('TerminalCode').value;
                data.bol = this.LFVRecordEditForm.get('BolNumber').value;
                data.correctedQuantity = this.LFVRecordEditForm.get('CorrectedQuantity').value;
                data.CarrierName = this.LFVRecordEditForm.get('CarrierName').value;
                data.CIN = this.LFVRecordEditForm.get('CIN').value;
                data.TerminalItemCode = this.LFVRecordEditForm.get('TerminalItemCode').value;
                data.GrossQuantity = this.LFVRecordEditForm.get('GrossQuantity').value;
                data.LoadDate = this.LFVRecordEditForm.get('LoadDate').value;
                data.LiftFileRecordId = this.LFVRecordEditForm.get('LIftFileRecordId').value;
                let requestModel = this.correctValues(data);
                this._lfvService.updateLiftFileRecord(requestModel).subscribe((response) => {
                    this.IsLoading = false;
                    if (response.StatusCode == 0) {
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                    }
                    else if (response.StatusCode == 1) {
                        src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                    }
                    this._EditToggleOpened(false);
                    this.getLfvFilterGrid(this.gridType);
                });
            }
        }
    }
    correctValues(data) {
        if (data.Terminal === '--') {
            data.Terminal = null;
        }
        if (data.bol === '--') {
            data.bol = null;
        }
        if (data.CarrierName === '--') {
            data.CarrierName = null;
        }
        if (data.CIN === '--') {
            data.CIN = null;
        }
        if (data.TerminalItemCode === '--') {
            data.TerminalItemCode = null;
        }
        if (data.LoadDate === '--') {
            data.LoadDate = null;
        }
        else {
            var loadDateWithOutSlash = data.LoadDate.replace(/\//g, '');
            data.LoadDate = loadDateWithOutSlash;
        }
        return data;
    }
}
MasterComponent.??fac = function MasterComponent_Factory(t) { return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__["LiftfiledashboardserviceService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormBuilder"])); };
MasterComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: MasterComponent, selectors: [["app-master"]], viewQuery: function MasterComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"], true);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????viewQuery"](_c0, true);
    } if (rf & 2) {
        var _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.filterComponent = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.datatableElement = _t.first);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????queryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????loadQuery"]()) && (ctx.btnOpenModal = _t.first);
    } }, decls: 229, vars: 88, consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-sm-12"], [1, "col-sm-8", "pr-5", "text-right", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-sm-4"], ["class", "float-right", 4, "ngIf"], [1, "col-md-12"], [3, "search", "export"], [3, "LFValidationList", 4, "ngIf"], [1, "col-sm-12", "text-center", "sticky-header-dash"], [1, "row", 3, "ngClass"], [1, "col-12"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "schedule-loading-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border", "location_table"], [1, "table-responsive"], ["id", "liftfilerecords-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "BOL"], ["data-key", "Terminal"], ["data-key", "Terminals"], ["data-key", "Corrected_Quantity"], ["data-key", "Terminal_Item_Code"], ["data-key", "Product_Type"], ["data-key", "Load_Date"], ["data-key", "Record_Date"], ["data-key", "Carrier_ID"], ["data-key", "Carrier_Name"], ["data-key", "Reasons"], ["data-key", "Reason_Code", 3, "ngClass"], ["data-key", "Reason_Category", 3, "ngClass"], ["data-key", "User_Name"], ["data-key", "Modified_Date"], ["data-key", "TimeToBol", 3, "ngClass"], ["data-key", "SelectAll", 3, "ngClass"], ["type", "checkbox", "id", "select-all-records", "value", "select-all-records", 3, "click"], ["data-key", "Action", 3, "ngClass"], ["data-key", "Edit", 3, "ngClass"], [4, "ngFor", "ngForOf"], ["class", "col-sm-12 text-right mb25 btn-wrapper", 4, "ngIf"], [1, "Lfv-resolve-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], [1, "scroll-auto"], [1, "well", "bg-white", "shadow-b", "lfrecord-section"], [1, "ibox-content", "no-border", "no-padding"], ["id", "LFrecord", 1, "table-responsive"], ["id", "table-Lfrecord", 1, "table", "table-striped", "table-bordered", "table-hover", "lfvrecord"], [1, "thead-light"], [4, "ngIf"], ["class", "pr30", 4, "ngIf"], ["id", "searchoutputdetailsgrid-modal", "role", "dialog", "tabindex", "-1", 1, "modal", "fade"], [1, "modal-dialog", "modal-xl", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "searchoutputdetailsgrid"], ["aria-hidden", "true"], [1, "modal-body"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox-content", "no-padding", "no-border"], ["id", "liftfileSearchrecords-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "BolNumber"], ["data-key", "FileName"], ["data-key", "TerminalITemCode"], ["data-key", "LoadDate"], ["data-key", "RecordDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "Status"], ["data-key", "Reason"], ["data-key", "Reason_Code"], ["data-key", "Reason_Category"], ["data-key", "UserName"], [1, "Lfv-edit-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], ["class", "loader", 4, "ngIf"], ["type", "hidden", "id", "openIgnoreModal", "data-toggle", "modal", "data-target", "#ignoreModal"], ["id", "ignoreModal", "tabindex", "-1", "role", "dialog", "aria-hidden", "true", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-dialog-centered"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "disabled", "click"], [1, "float-right"], ["placement", "bottom", "ngbTooltip", "Scratch Report", 1, "btn", "border", "border-secondary", "float-right", "pb-1", 3, "click"], [1, "fab", "fa-firstdraft", "fs16", "mt2"], ["placement", "bottom", "ngbTooltip", "Accrual Report", 1, "btn", "border", "border-secondary", "float-right", "pb-1", 3, "click"], [1, "fas", "fa-file-invoice", "fs16", "mt2"], [1, "btn", "btn-default", "mt0", 3, "click"], ["btnSearchRecords", ""], [1, "fas", "fa-search", "mr5"], ["class", "record-search-controls bg-white pa shadow border z-index10", 4, "ngIf"], [1, "record-search-controls", "bg-white", "pa", "shadow", "border", "z-index10"], [1, "col-auto", "text-right"], ["btnCancelSearch", ""], ["title", "Cancel", 1, "fa", "fa-close", "fs18", "my-2"], [1, "col-auto", "form-group"], ["type", "text", "placeholder", "Search BOL#", 1, "form-control", 3, "ngModel", "ngModelChange"], ["BOLSearchInput", ""], ["type", "text", "placeholder", "Search FileName", 1, "form-control", 3, "ngModel", "ngModelChange"], ["FileNameSearchInput", ""], [1, "col-auto", "form-group", "text-right"], [1, "btn", "btn-primary", "btn-sm", 3, "click"], ["id", "openModal", "data-toggle", "modal", "data-target", "#searchoutputdetailsgrid-modal", 3, "hidden"], ["btnOpenModal", ""], ["placement", "bottom", "ngbTooltip", "Carrier BOL Report", 1, "btn", "border", "border-secondary", "float-right", 3, "click"], [1, "fas", "fa-user", "fs16"], ["placement", "bottom", "ngbTooltip", "Supplier BOL Report", 1, "btn", "border", "border-secondary", "float-right", 3, "click"], [1, "fas", "fa-truck", "fs16"], [3, "LFValidationList"], [3, "ngClass"], [1, "text-center", 3, "ngClass"], ["type", "checkbox", 1, "dt-checkbox", 3, "id", "checked", "value"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-edit", "fs16"], ["title", "Edit Lf Record", 1, "fas", "fa-edit", "fs16"], [1, "col-sm-12", "text-right", "mb25", "btn-wrapper"], [1, "form-group", "col-sm-12"], ["type", "button", "class", "btn btn-primary", "value", "Ignore", "id", "btnForceIgnoreRecords", 3, "click", 4, "ngIf"], ["type", "button", "class", "btn btn-primary", "value", "ReProcess", "id", "btnForceReprocessRecords", 3, "click", 4, "ngIf"], ["type", "button", "value", "Ignore", "id", "btnForceIgnoreRecords", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "ReProcess", "id", "btnForceReprocessRecords", 1, "btn", "btn-primary", 3, "click"], ["class", "col-sm-6", 4, "ngIf"], [1, "col-sm-6"], [1, "form-group"], ["id", "select-bol", 1, "form-control", 3, "change"], [3, "value"], [3, "value", "selected"], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "col-sm-12", "section-bol-details-edit"], [1, "mt10", "row"], [1, "col-sm-3", "bol"], ["for", "BolNumber"], [1, "color-maroon"], ["formControlName", "InvoiceFtlDetailId", "type", "hidden", 1, "hide-element"], ["formControlName", "LIftFileRecordId", "type", "hidden", 1, "hide-element"], ["formControlName", "BolNumber", "required", "", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3", "lifdt"], ["for", "LiftDate"], ["name", "LiftDate", "formControlName", "LiftDate", "myDatePicker", "", 1, "form-control", 3, "format", "onDateChange"], [1, "col-sm-3", "grossQty"], ["for", "GrossQuantity"], ["formControlName", "GrossQuantity", 1, "form-control"], [1, "col-sm-3", "netQty"], ["for", "NetQuantity"], ["formControlName", "NetQuantity", 1, "form-control"], ["class", "col-sm-6 terminal-section", 4, "ngIf"], [1, "col-sm-6", "fuelType"], ["for", "Jobs"], ["formControlName", "SelectedFuelType", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-6"], ["for", "BadgeNumber"], ["formControlName", "BadgeNumber", 1, "form-control"], ["for", "Notes"], ["id", "Notes", "placeholder", "Notes", "formControlName", "Notes", 1, "form-control"], [1, "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], [1, "col-sm-6", "terminal-section"], ["formControlName", "SelectedTerminal", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-sm-12", "section-lfv-record-edit"], [1, "col-sm-3"], ["formControlName", "BolNumber", 1, "form-control", 3, "readonly"], ["for", "TerminalCode"], ["formControlName", "TerminalCode", 1, "form-control", 3, "readonly"], ["for", "TerminalItemCode"], ["formControlName", "TerminalItemCode", 1, "form-control", 3, "readonly"], ["for", "CIN"], ["formControlName", "CIN", 1, "form-control", 3, "readonly"], ["for", "CarrierName"], ["formControlName", "CarrierName", 1, "form-control", 3, "readonly"], ["for", "LoadDate"], ["name", "LoadDate", "formControlName", "LoadDate", "myDatePicker", "", 1, "form-control", 3, "format", "readonly", "onDateChange"], ["for", "CorrectedQuantity"], ["formControlName", "CorrectedQuantity", 1, "form-control", 3, "readonly"], ["formControlName", "GrossQuantity", 1, "form-control", 3, "readonly"], ["for", "ProductType"], ["name", "ProductType", "formControlName", "ProductType", 1, "form-control", 3, "readonly"], ["for", "RecordDate"], ["formControlName", "RecordDate", 1, "form-control", 3, "readonly"], ["for", "CarrierId"], ["formControlName", "CarrierId", 1, "form-control", 3, "readonly"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function MasterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](7, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](8, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_8_listener() { return ctx.changeViewType(1); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](9, "Validation Performance");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](10, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](11, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_11_listener() { return ctx.changeViewType(2); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](12, "Carrier Performance");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](13, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](14, MasterComponent_div_14_Template, 3, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](15, MasterComponent_div_15_Template, 3, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](16, MasterComponent_div_16_Template, 6, 1, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](17, MasterComponent_div_17_Template, 5, 0, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](18, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](19, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](20, "app-left-side-filter", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("search", function MasterComponent_Template_app_left_side_filter_search_20_listener($event) { return ctx.OnSearch($event); })("export", function MasterComponent_Template_app_left_side_filter_export_20_listener($event) { return ctx.OnExport($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](21, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](22, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](23, MasterComponent_app_validation_23_Template, 1, 1, "app-validation", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](24, MasterComponent_app_carrier_performace_24_Template, 1, 1, "app-carrier-performace", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](25, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](26, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](27, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](28, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](29, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](30, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_30_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.Clean); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](31, "Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](32, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](33, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_33_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.NoMatch); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](34, "No Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](35, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](36, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_36_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.PartialMatch); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](37, " Partial Match ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](38, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](39, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_39_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.PendingMatch); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](40, "Pending");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](41, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](42, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_42_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.Duplicate); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](43, "Duplicate");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](44, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](45, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_45_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.ActiveExceptions); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](46, " Active Exception ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](47, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](48, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_48_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.IgnoreMatch); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](49, "Ignored");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](50, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](51, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_51_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.ForcedIgnore); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](52, "Forced Ignore");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](53, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](54, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_label_click_54_listener() { return ctx.changeGridType(ctx.LFVRecordStatus.UnMatched); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](55, "Unmatched");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](56, "div", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](57, "div", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](58, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](59, "div", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](60, "span", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](61, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](62, "div", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](63, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](64, "table", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](65, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](66, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](67, "th", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](68, "BOL");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](69, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](70, "Terminal Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](71, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](72, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](73, "th", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](74, "Corrected Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](75, "th", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](76, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](77, "th", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](78, "Product Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](79, "th", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](80, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](81, "th", 30);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](82, "Record Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](83, "th", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](84, "Carrier ID");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](85, "th", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](86, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](87, "th", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](88, "Reasons");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](89, "th", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](90, " Reason Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](91, "th", 35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](92, " Reason Category ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](93, "th", 36);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](94, "Modified By");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](95, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](96, "Modified Date (MST)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](97, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](98, "Resolution Time");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](99, "th", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](100, " Time To BOL ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](101, "th", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](102, " SelectAll ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](103, "input", 40);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_input_click_103_listener($event) { return ctx.selectAllRecords($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](104, "th", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](105, " Action ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](106, "th", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](107, " Edit ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](108, "tbody");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerStart"](109);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](110, MasterComponent_tr_110_Template, 43, 84, "tr", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementContainerEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](111, MasterComponent_div_111_Template, 5, 2, "div", 44);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](112, "ng-sidebar-container");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](113, "ng-sidebar", 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("openedChange", function MasterComponent_Template_ng_sidebar_openedChange_113_listener($event) { return ctx._opened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](114, "a", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_a_click_114_listener() { return ctx._toggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](115, "i", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](116, "h3", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](117, "Edit BOL Details");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](118, "div", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](119, "div", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](120, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](121, "div", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](122, "div", 52);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](123, "table", 53);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](124, "thead", 54);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](125, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](126, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](127, "BOL#");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](128, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](129, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](130, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](131, "Corrected Quantity");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](132, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](133, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](134, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](135, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](136, "th");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](137, "ProductType");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](138, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](139, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](140);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](141, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](142);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](143, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](144);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](145, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](146);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](147, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](148);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](149, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](150);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](151, MasterComponent_div_151_Template, 2, 1, "div", 55);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](152, MasterComponent_content_152_Template, 64, 12, "content", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](153, "div", 57);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](154, "div", 58);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](155, "div", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](156, "div", 60);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](157, "button", 61);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](158, "span", 62);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](159, "\u00D7");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](160, "div", 63);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](161, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](162, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](163, "div", 64);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](164, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](165, "div", 65);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](166, "div", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](167, "table", 66);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](168, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](169, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](170, "th", 67);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](171, "CallID");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](172, "th", 68);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](173, "BOL#");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](174, "th", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](175, "Terminal Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](176, "th", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](177, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](178, "th", 69);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](179, "File Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](180, "th", 70);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](181, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](182, "th", 71);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](183, "Load Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](184, "th", 72);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](185, "Record Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](186, "th", 73);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](187, "CarrierID");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](188, "th", 74);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](189, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](190, "th", 75);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](191, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](192, "th", 76);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](193, "Reason");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](194, "th", 77);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](195, "Reason Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](196, "th", 78);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](197, "Reason Category");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](198, "th", 79);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](199, "Modified By");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](200, "th", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](201, " Modified Date (MST)");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](202, MasterComponent_tbody_202_Template, 2, 1, "tbody", 55);
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
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](203, "ng-sidebar-container");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](204, "ng-sidebar", 80);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("openedChange", function MasterComponent_Template_ng_sidebar_openedChange_204_listener($event) { return ctx._EditOpened = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](205, "a", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_a_click_205_listener() { return ctx._EditToggleOpened(false); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](206, "i", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](207, "h3", 48);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](208, "Edit Lift File Record ");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](209, "div", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](210, MasterComponent_content_210_Template, 83, 29, "content", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????template"](211, MasterComponent_div_211_Template, 5, 0, "div", 81);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](212, "div", 82);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](213, "div", 83);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](214, "div", 84);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](215, "div", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](216, "div", 60);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](217, "h4", 85);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](218, "Select Reason");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](219, "button", 86);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](220, "span", 62);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](221, "\u00D7");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](222, "div", 63);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](223, "ng-multiselect-dropdown", 87);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("ngModelChange", function MasterComponent_Template_ng_multiselect_dropdown_ngModelChange_223_listener($event) { return ctx.selectedReason = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](224, "div", 88);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](225, "button", 89);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](226, "Cancel");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](227, "button", 90);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????listener"]("click", function MasterComponent_Template_button_click_227_listener() { return ctx.submitIgnoreDescription(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????text"](228, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "viewType")("value", 1)("checked", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "viewType")("value", 2)("checked", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.viewType == 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.Clean)("checked", ctx.gridType == ctx.LFVRecordStatus.Clean);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.NoMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.NoMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.PartialMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.PartialMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.PendingMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.PendingMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.Duplicate)("checked", ctx.gridType == ctx.LFVRecordStatus.Duplicate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.ActiveExceptions)("checked", ctx.gridType == ctx.LFVRecordStatus.ActiveExceptions);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.IgnoreMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.IgnoreMatch);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.ForcedIgnore)("checked", ctx.gridType == ctx.LFVRecordStatus.ForcedIgnore);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("name", "gridType")("value", ctx.LFVRecordStatus.UnMatched)("checked", ctx.gridType == ctx.LFVRecordStatus.UnMatched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](74, _c3, ctx.gridType == 0));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](25);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](76, _c3, ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Duplicate || ctx.gridType == ctx.LFVRecordStatus.PendingMatch));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](78, _c3, ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Duplicate || ctx.gridType == ctx.LFVRecordStatus.PendingMatch));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](80, _c3, ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Clean ? false : true));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](82, _c3, !(ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.UnMatched || ctx.gridType == ctx.LFVRecordStatus.Duplicate)));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](84, _c3, ctx.gridType != ctx.LFVRecordStatus.PartialMatch));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["????pureFunction1"](86, _c3, ctx.gridType == ctx.LFVRecordStatus.IgnoreMatch || ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.ForcedIgnore || !ctx.isAdminUser));
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngForOf", ctx.LFVRecordGrid);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (ctx.LFVRecordGrid == null ? null : ctx.LFVRecordGrid.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](27);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.bol);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalName);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.correctedQuantity);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalItemCode);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.LoadDate);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????textInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.ProductType);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null && ctx.bolResolveForm.get("InvoiceFtlDetailId").value > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("dtOptions", ctx.searchGridDtOptions)("dtTrigger", ctx.searchGridDtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](35);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", (ctx.LiftFilesearchResults == null ? null : ctx.LiftFilesearchResults.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("opened", ctx._EditOpened)("animate", ctx._EditAnimate)("position", ctx._POSITIONS[ctx._positionNum]);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.LFVRecordEditForm != undefined && ctx.LFVRecordEditForm != null && ctx.LfvValidationParameters != undefined && ctx.LfvValidationParameters != null);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("ngIf", ctx.IsLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](12);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("placeholder", "Select Reason")("settings", ctx.dropdownSettings)("data", ctx.reasonList)("ngModel", ctx.selectedReason);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????advance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????property"]("disabled", ctx.selectedReason && ctx.selectedReason.length == 0);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_11__["NgIf"], _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_11__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_11__["NgForOf"], ng_sidebar__WEBPACK_IMPORTED_MODULE_12__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_12__["Sidebar"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgModel"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbTooltip"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["DefaultValueAccessor"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_15__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_16__["CarrierComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["??angular_packages_forms_forms_x"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["??angular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["RequiredValidator"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__["DatePicker"]], styles: ["aside {\r\n    width: 55% !important;\r\n}\r\n\r\n.scroll-auto{\r\n    height: calc(100vh - 125px);\r\n    overflow-x: hidden;\r\n    overflow-y:auto ;\r\n}\r\n\r\n.highlight-record {\r\n    background-color: #ffcccc\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9tYXN0ZXIvbWFzdGVyLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxxQkFBcUI7QUFDekI7O0FBRUE7SUFDSSwyQkFBMkI7SUFDM0Isa0JBQWtCO0lBQ2xCLGdCQUFnQjtBQUNwQjs7QUFDQTtJQUNJO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9sZnYtZGFzaGJvYXJkL21hc3Rlci9tYXN0ZXIuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbImFzaWRlIHtcclxuICAgIHdpZHRoOiA1NSUgIWltcG9ydGFudDtcclxufVxyXG5cclxuLnNjcm9sbC1hdXRve1xyXG4gICAgaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTI1cHgpO1xyXG4gICAgb3ZlcmZsb3cteDogaGlkZGVuO1xyXG4gICAgb3ZlcmZsb3cteTphdXRvIDtcclxufVxyXG4uaGlnaGxpZ2h0LXJlY29yZCB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZjY2NjXHJcbn1cclxuIl19 */"], encapsulation: 2 });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-master',
                templateUrl: './master.component.html',
                styleUrls: ['./master.component.css'],
                encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewEncapsulation"].None
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__["LiftfiledashboardserviceService"] }, { type: _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormBuilder"] }]; }, { filterComponent: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [_left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"]]
        }], datatableElement: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: [angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"]]
        }], btnOpenModal: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
            args: ['btnOpenModal']
        }] }); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts":
/*!***************************************************************************!*\
  !*** ./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts ***!
  \***************************************************************************/
/*! exports provided: LiftfiledashboardserviceService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LiftfiledashboardserviceService", function() { return LiftfiledashboardserviceService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class LiftfiledashboardserviceService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        //urls
        this.urlGetLFRecordsGrid = 'Supplier/LiftFile/GetLiftFileRecordsScratchReport';
        this.urlGetBolDetailsForResolve = 'Supplier/LiftFile/GetLFBolEditDetailsForSlider';
        this.urlSaveBolDetailsForResolve = 'Supplier/LiftFile/SaveLFBolEditDetails';
        this.urlAddRecordsForForceIgnoreProcessing = 'Supplier/LiftFile/AddRecordsAsIgnoreMatch';
        this.urlAddUnmatchedRecordForReProcessing = 'Supplier/Exception/AddUnmatchedRecordForReProcessing';
        //SupplierBOLReport
        this.urlGetLiftFileRecordsWithMissingTFXDeliveryDetails = 'Supplier/LiftFile/GetLiftFileRecordsWithMissingTFXDeliveryDetails';
        //carrier bol report
        this.urlGetTFXDeliveryDetailsWithMissingLiftFileRecords = 'Supplier/LiftFile/GetTFXDeliveryDetailsWithMissingLiftFileRecords';
        this.UrlGetLFValidation = '/Supplier/Exception/LFValidationGridWithFilter';
        this.UrlGetLFCarrier = '/Supplier/LiftFile/GetLFVCarrierDropDwn';
        this.UrlGetLFVRecordGrid = '/Supplier/Exception/LFRecordsGridForDashboard';
        this.urlGetLFSearchRecordsByBolFileName = 'Supplier/LiftFile/LFRecordsGridByBolFileName?bol=';
        this.UrlGetLFVAccrualReportGrid = 'Supplier/LiftFile/GetLFVAccrualReportGrid';
        this.UrlGetLFVValidationStatsAndProductTypesDDL = 'Supplier/LiftFile/GetLFVValidationStatsAndProductTypesDDL';
        this.UrlUpdateLiftFileRecord = 'Supplier/LiftFile/UpdateLiftFileRecord';
        this.urlGetReasonDescriptionList = 'Supplier/LiftFile/GetReasonDescriptionList';
        this.urlGetPreferencesSetting = 'Settings/Profile/GetPreferencesSettingAsync';
    }
    getLFRecords() {
        return this.httpClient.get(this.urlGetLFRecordsGrid)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFRecords', null)));
    }
    getBolDetailsForResolve(record) {
        return this.httpClient.post(this.urlGetBolDetailsForResolve, record)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBolDetailsForResolve', record)));
    }
    saveBolDetailsForResolve(record) {
        return this.httpClient.post(this.urlSaveBolDetailsForResolve, record)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('saveBolDetailsForResolve', record)));
    }
    addRecordsForForcedIgnoreMatchProcessing(LfRecordIds, descriptionId = 0, descriptionText = '') {
        return this.httpClient.post(this.urlAddRecordsForForceIgnoreProcessing + '?DescriptionId=' + descriptionId + '&DescriptionText=' + descriptionText, LfRecordIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
    }
    getLFValidationGrid(data) {
        return this.httpClient.post(this.UrlGetLFValidation, { startDate: data.fromDate, endDate: data.toDate, isCarrierPerFormanceDashboard: data.isCarrierPerFormanceDashboard, carrierIds: data.carrierIds, isMatchingWindow: data.isMatchingWindow })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFValidationGrid', null)));
    }
    getLFCarrier(fromDate, toDate) {
        return this.httpClient.post(this.UrlGetLFCarrier, { fromDate: fromDate, toDate: toDate })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFCarrier', null)));
    }
    getSupplierBOLReport() {
        return this.httpClient.get(this.urlGetLiftFileRecordsWithMissingTFXDeliveryDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSupplierBOLReport', null)));
    }
    getCarrierBOLReport(fromDate, toDate) {
        return this.httpClient.post(this.urlGetTFXDeliveryDetailsWithMissingLiftFileRecords, { fromDate: fromDate, toDate: toDate })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierBOLReport', null)));
    }
    getLFVRecordGrid(data) {
        return this.httpClient.post(this.UrlGetLFVRecordGrid, { recordStatus: data.recordStatus, startDate: data.fromDate, endDate: data.toDate, lfCallId: 0, isMatchingWindow: data.isMatchingWindow, carrierIds: data.carrierIds })
            // return this.httpClient.post(this.UrlGetLFVRecordGrid, {recordStatus:data.recordStatus, startDate: data.fromDate, endDate: data.toDate})
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFVRecordGrid', null)));
    }
    addUnmatchedRecordForReProcessing(LfRecordIds) {
        return this.httpClient.post(this.urlAddUnmatchedRecordForReProcessing, LfRecordIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
    }
    getLFSearchRecords(bol, fileName) {
        return this.httpClient.get(this.urlGetLFSearchRecordsByBolFileName + bol + '&fileName=' + fileName)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFSearchRecords', null)));
    }
    getLFVAccrualReportGrid(data) {
        return this.httpClient.post(this.UrlGetLFVAccrualReportGrid, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFVAccrualReportGrid', null)));
    }
    GetLFVValidationStatsAndProductTypesDDL(data) {
        return this.httpClient.post(this.UrlGetLFVValidationStatsAndProductTypesDDL, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetLFVValidationStatsAndProductTypesDDL', null)));
    }
    updateLiftFileRecord(data) {
        return this.httpClient.post(this.UrlUpdateLiftFileRecord, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateLiftFileRecord', null)));
    }
    GetReasonDescriptionList() {
        return this.httpClient.get(this.urlGetReasonDescriptionList)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetReasonDescriptionList', null)));
    }
    getPreferencesSetting() {
        return this.httpClient.get(this.urlGetPreferencesSetting)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getPreferencesSettingAsync', null)));
    }
}
LiftfiledashboardserviceService.??fac = function LiftfiledashboardserviceService_Factory(t) { return new (t || LiftfiledashboardserviceService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????inject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
LiftfiledashboardserviceService.??prov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineInjectable"]({ token: LiftfiledashboardserviceService, factory: LiftfiledashboardserviceService.??fac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](LiftfiledashboardserviceService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/supplier-bol-report/supplier-bol-report.component.ts":
/*!************************************************************************************!*\
  !*** ./src/app/lfv-dashboard/supplier-bol-report/supplier-bol-report.component.ts ***!
  \************************************************************************************/
/*! exports provided: SupplierBolReportComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SupplierBolReportComponent", function() { return SupplierBolReportComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
/* harmony import */ var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! angular-datatables */ "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");






function SupplierBolReportComponent_tbody_31_tr_1_Template(rf, ctx) { if (rf & 1) {
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16);
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
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const record_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate"](record_r3.CallId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.BOL, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.TerminalCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.Terminal, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.CorrectedQuanity, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.TerminalItemCode, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.ProductType, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.CarrierID, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.CarrierName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.FileName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????textInterpolate1"](" ", record_r3.RecordDate, " ");
} }
function SupplierBolReportComponent_tbody_31_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "tbody");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](1, SupplierBolReportComponent_tbody_31_tr_1_Template, 23, 11, "tr", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????nextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngForOf", ctx_r0.ReportRecords);
} }
function SupplierBolReportComponent_div_32_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????element"](2, "div", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](4, "Loading");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
} }
class SupplierBolReportComponent {
    constructor(dashboardservice) {
        this.dashboardservice = dashboardservice;
        this.ReportRecords = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
    }
    ngOnInit() {
        this.intializeGrid();
    }
    intializeGrid() {
        this.ShowGridLoader = true;
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Lift File Records', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Lift File Records', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getSupplierBOLReport();
    }
    getSupplierBOLReport() {
        this.ShowGridLoader = true;
        this.dashboardservice.getSupplierBOLReport().subscribe((data) => {
            this.ShowGridLoader = false;
            this.ReportRecords = data;
            this.dtTrigger.next();
        });
    }
    reloadGrid() {
        $("#supplierbolreport-datatable").DataTable().clear().destroy();
        this.getSupplierBOLReport();
    }
}
SupplierBolReportComponent.??fac = function SupplierBolReportComponent_Factory(t) { return new (t || SupplierBolReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"])); };
SupplierBolReportComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["????defineComponent"]({ type: SupplierBolReportComponent, selectors: [["app-supplier-bol-report"]], decls: 33, vars: 4, consts: [[1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "supplierbolreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "Bol"], ["data-key", "TerminalCode"], ["data-key", "Terminal"], ["data-key", "CorrectedQuanity"], ["data-key", "TerminalItemCode"], ["data-key", "ProductType"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "FileName"], ["data-key", "RecordDate"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]], template: function SupplierBolReportComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](4, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](5, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](6, "table", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](7, "thead");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](8, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](9, "th", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](10, "CallId");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](11, "th", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](12, "BOL");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](13, "th", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](14, "Terminal Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](15, "th", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](16, "Terminal");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](17, "th", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](18, "Corrected Quanity");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](19, "th", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](20, "Terminal Item Code");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](21, "th", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](22, "Product Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](23, "th", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](24, "CarrierID");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](25, "th", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](26, "Carrier Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](27, "th", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](28, "File Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementStart"](29, "th", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????text"](30, "Record Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](31, SupplierBolReportComponent_tbody_31_Template, 2, 1, "tbody", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????template"](32, SupplierBolReportComponent_div_32_Template, 5, 0, "div", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????elementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", (ctx.ReportRecords == null ? null : ctx.ReportRecords.length) > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????advance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["????property"]("ngIf", ctx.ShowGridLoader);
    } }, directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvc3VwcGxpZXItYm9sLXJlcG9ydC9zdXBwbGllci1ib2wtcmVwb3J0LmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["??setClassMetadata"](SupplierBolReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-supplier-bol-report',
                templateUrl: './supplier-bol-report.component.html',
                styleUrls: ['./supplier-bol-report.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/validation/validation.component.ts":
/*!******************************************************************!*\
  !*** ./src/app/lfv-dashboard/validation/validation.component.ts ***!
  \******************************************************************/
/*! exports provided: ValidationComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ValidationComponent", function() { return ValidationComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../LiftFileModels */ "./src/app/lfv-dashboard/LiftFileModels.ts");
/* harmony import */ var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../service/liftfiledashboardservice.service */ "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");





class ValidationComponent {
    constructor(_lfvService) {
        this._lfvService = _lfvService;
    }
    ngOnInit() {
        this.init();
    }
    init() {
    }
    ngOnChanges(changes) {
        if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
            this.createChartData();
        }
    }
    //public openLFVScratchReportGrid(): void {
    //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
    //}
    RendorChart(data) {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            try {
                if (this.chart)
                    this.chart.destroy();
            }
            catch (e) {
            }
            var options = {
                colors: ["#00FF00", "#ff0000", "#FF69B4", "#FFFF00", "#000080", "#00A7C6", "#800080", '#0077ff', '#A9D794'],
                series: [{
                        data: data
                    }],
                chart: {
                    height: 350,
                    type: 'bar',
                    events: {
                        click: function (chart, w, e) {
                            console.log(chart, w, e);
                        }
                    }
                },
                // colors: colors,
                plotOptions: {
                    bar: {
                        columnWidth: '45%',
                        distributed: true,
                    }
                },
                dataLabels: {
                    enabled: true
                },
                legend: {
                    show: true
                },
                xaxis: {
                    categories: [
                        'Match',
                        'No Match',
                        'Partial Match',
                        'Pending',
                        'Duplicate',
                        'Active Exception',
                        'Ignored',
                        'Forced Ignore',
                        'UnMatched'
                    ],
                    labels: {
                        style: {
                            //colors: colors,
                            fontSize: '12px'
                        }
                    }
                },
                fill: {
                    opacity: 1
                    //colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']
                }
            };
            this.chart = new ApexCharts(document.querySelector("#chart-timeline"), options);
            try {
                if (this.chart)
                    this.chart.render();
            }
            catch (e) {
                this.chart.destroy();
                this.chart.render();
            }
        });
    }
    createChartData() {
        return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, function* () {
            var lfv = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFValidationGridViewModel"]();
            lfv.ActiveExceptionRecordCount = 0;
            lfv.DuplicateRecordCount = 0;
            lfv.IgnoredMatchRecordCount = 0;
            lfv.MatchedRecordCount = 0;
            lfv.NoMatchRecordCount = 0;
            lfv.PartialMatchRecordCount = 0;
            lfv.PendingMatchCount = 0;
            lfv.TotalRecordCount = 0;
            lfv.UnmatchedRecordCount = 0;
            lfv.ForcedIgnoredMatchRecordCount = 0;
            (yield this.LFValidationList) && this.LFValidationList.map(m => {
                lfv.ActiveExceptionRecordCount += m.ActiveExceptionRecordCount;
                lfv.DuplicateRecordCount += m.DuplicateRecordCount;
                lfv.IgnoredMatchRecordCount += m.IgnoredMatchRecordCount;
                lfv.MatchedRecordCount += m.MatchedRecordCount;
                lfv.NoMatchRecordCount += m.NoMatchRecordCount;
                lfv.PartialMatchRecordCount += m.PartialMatchRecordCount;
                lfv.PendingMatchCount += m.PendingMatchCount;
                lfv.TotalRecordCount += m.TotalRecordCount;
                lfv.UnmatchedRecordCount += m.UnmatchedRecordCount;
                lfv.ForcedIgnoredMatchRecordCount += m.ForcedIgnoredMatchRecordCount;
            });
            let data = [lfv.MatchedRecordCount, lfv.NoMatchRecordCount, lfv.PartialMatchRecordCount,
                lfv.PendingMatchCount, lfv.DuplicateRecordCount, lfv.ActiveExceptionRecordCount,
                lfv.IgnoredMatchRecordCount, lfv.ForcedIgnoredMatchRecordCount, lfv.UnmatchedRecordCount];
            this.RendorChart(data);
        });
    }
}
ValidationComponent.??fac = function ValidationComponent_Factory(t) { return new (t || ValidationComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["????directiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"])); };
ValidationComponent.??cmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["????defineComponent"]({ type: ValidationComponent, selectors: [["app-validation"]], inputs: { LFValidationList: "LFValidationList" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["????NgOnChangesFeature"]], decls: 4, vars: 0, consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-md-10"], ["id", "chart-timeline"]], template: function ValidationComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????element"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["????elementEnd"]();
    } }, styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvdmFsaWRhdGlvbi92YWxpZGF0aW9uLmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_1__["??setClassMetadata"](ValidationComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
                selector: 'app-validation',
                templateUrl: './validation.component.html',
                styleUrls: ['./validation.component.css']
            }]
    }], function () { return [{ type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"] }]; }, { LFValidationList: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }] }); })();


/***/ })

}]);
//# sourceMappingURL=lfv-dashboard-lfv-dashboard-module-es2015.js.map