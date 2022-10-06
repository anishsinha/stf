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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["lfv-dashboard-lfv-dashboard-module"], {
  /***/
  "./node_modules/angular7-csv/dist/Angular-csv.js": function node_modulesAngular7CsvDistAngularCsvJs(module, exports, __webpack_require__) {
    "use strict";

    Object.defineProperty(exports, "__esModule", {
      value: true
    });

    var CsvConfigConsts = function () {
      function CsvConfigConsts() {}

      CsvConfigConsts.EOL = "\r\n";
      CsvConfigConsts.BOM = "\uFEFF";
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
    }();

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

    var AngularCsv = function () {
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

        var blob = new Blob([this.csv], {
          "type": "text/csv;charset=utf8;"
        });

        if (navigator.msSaveBlob) {
          var filename = this._options.filename.replace(/ /g, "_") + ".csv";
          navigator.msSaveBlob(blob, filename);
        } else {
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
    }();

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
    } //# sourceMappingURL=Angular-csv.js.map

    /***/

  },

  /***/
  "./src/app/lfv-dashboard/carrier-bol-report/carrier-bol-report.component.ts": function srcAppLfvDashboardCarrierBolReportCarrierBolReportComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierBolReportComponent", function () {
      return CarrierBolReportComponent;
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


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function CarrierBolReportComponent_tbody_42_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var record_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](record_r3.BOL);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.TerminalName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.LoadDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.NetQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.GrossQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.BadgeNumber, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.CarrierName, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.CarrierID, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.FuelTypeName, " ");
      }
    }

    function CarrierBolReportComponent_tbody_42_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CarrierBolReportComponent_tbody_42_tr_1_Template, 19, 9, "tr", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.ReportRecords);
      }
    }

    function CarrierBolReportComponent_div_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var CarrierBolReportComponent = /*#__PURE__*/function () {
      function CarrierBolReportComponent(dashboardservice) {
        _classCallCheck(this, CarrierBolReportComponent);

        this.dashboardservice = dashboardservice;
        this.ReportRecords = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
        this.fromDate = null;
        this.toDate = null;
      }

      _createClass(CarrierBolReportComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.intializeGrid();
        }
      }, {
        key: "intializeGrid",
        value: function intializeGrid() {
          this.ShowGridLoader = true;
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
              title: 'Lift File Records',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Lift File Records',
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
          this.getCarrierBOLReport();
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.fromDate = event;
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.toDate = event;
        }
      }, {
        key: "ApplyFilter",
        value: function ApplyFilter() {
          if (this.fromDate == null || this.fromDate == undefined || this.fromDate == "" || this.toDate == null || this.toDate == undefined || this.toDate == "") {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("From/To date not selected", undefined, undefined);
          } else {
            this.reloadGrid();
          }
        }
      }, {
        key: "reloadGrid",
        value: function reloadGrid() {
          $("#carrierbolreport-datatable").DataTable().clear().destroy();
          this.getCarrierBOLReport();
        }
      }, {
        key: "ClearFilter",
        value: function ClearFilter() {
          this.fromDate = null;
          this.toDate = null;
          this.reloadGrid();
        }
      }, {
        key: "getCarrierBOLReport",
        value: function getCarrierBOLReport() {
          var _this2 = this;

          var fromDate = this.fromDate;
          var toDate = this.toDate;
          this.ShowGridLoader = true;
          this.dashboardservice.getCarrierBOLReport(fromDate, toDate).subscribe(function (data) {
            _this2.ShowGridLoader = false;
            _this2.ReportRecords = data;

            _this2.dtTrigger.next();
          });
        }
      }]);

      return CarrierBolReportComponent;
    }();

    CarrierBolReportComponent.ɵfac = function CarrierBolReportComponent_Factory(t) {
      return new (t || CarrierBolReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"]));
    };

    CarrierBolReportComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CarrierBolReportComponent,
      selectors: [["app-carrier-bol-report"]],
      decls: 44,
      vars: 9,
      consts: [[1, "row", "mb10"], [1, "col-sm-12"], [1, "well", "pb10", "mb0"], [1, "row"], [1, "col-xs-12", "col-sm-2", "col-md-1", "pt5", "pr0"], [1, "fa", "fa-filter", "mr5", "fs16"], [1, "f-normal", "fs16"], [1, "col-md-3"], ["type", "text", "placeholder", "From", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "To", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "ngModelChange", "onDateChange"], [1, "col-12", "col-sm-4", "col-md-3", "mt5-xs"], ["type", "button", "value", "Apply", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "Clear Filter", 1, "btn", "ml5", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "carrierbolreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Bol"], ["data-key", "TerminalName"], ["data-key", "LoadDate"], ["data-key", "NetQuantity"], ["data-key", "GrossQuantity"], ["data-key", "BadgeNumber"], ["data-key", "CarrierName"], ["data-key", "CarrierID"], ["data-key", "FuelTypeName"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CarrierBolReportComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Filter");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierBolReportComponent_Template_input_ngModelChange_9_listener($event) {
            return ctx.fromDate = $event;
          })("onDateChange", function CarrierBolReportComponent_Template_input_onDateChange_9_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierBolReportComponent_Template_input_ngModelChange_11_listener($event) {
            return ctx.toDate = $event;
          })("onDateChange", function CarrierBolReportComponent_Template_input_onDateChange_11_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "input", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierBolReportComponent_Template_input_click_13_listener() {
            return ctx.ApplyFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierBolReportComponent_Template_input_click_14_listener() {
            return ctx.ClearFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "table", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "BOL#");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Terminal Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Lift Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Net Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](33, "Gross Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](35, "Badge#");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](39, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](42, CarrierBolReportComponent_tbody_42_Template, 2, 1, "tbody", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](43, CarrierBolReportComponent_div_43_Template, 5, 0, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.fromDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.toDate)("format", "MM/DD/YYYY")("minDate", ctx.fromDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.ReportRecords == null ? null : ctx.ReportRecords.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.ShowGridLoader);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_4__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_5__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__["NgModel"], angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvY2Fycmllci1ib2wtcmVwb3J0L2NhcnJpZXItYm9sLXJlcG9ydC5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CarrierBolReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-carrier-bol-report',
          templateUrl: './carrier-bol-report.component.html',
          styleUrls: ['./carrier-bol-report.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/carrier/carrier.component.ts": function srcAppLfvDashboardCarrierCarrierComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierComponent", function () {
      return CarrierComponent;
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


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");

    var CarrierComponent = /*#__PURE__*/function () {
      function CarrierComponent(_lfvService) {
        _classCallCheck(this, CarrierComponent);

        this._lfvService = _lfvService;
      }

      _createClass(CarrierComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
            this.createChartData();
          }
        } //public openLFVScratchReportGrid(): void {
        //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
        //}

      }, {
        key: "RendorChart",
        value: function RendorChart(data, carrierList, chartHeight) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
            var options;
            return regeneratorRuntime.wrap(function _callee$(_context) {
              while (1) {
                switch (_context.prev = _context.next) {
                  case 0:
                    try {
                      if (this.chart) this.chart.destroy();
                    } catch (e) {}

                    options = {
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
                          dataLabels: {//  position: 'bottom'
                          }
                        }
                      },
                      xaxis: {
                        type: 'category',
                        categories: carrierList
                      },
                      legend: {
                        position: 'top',
                        horizontalAlign: 'left',
                        offsetX: 40 //  offsetY: 40

                      },
                      fill: {
                        opacity: 1 //  colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']

                      }
                    };
                    this.chart = new ApexCharts(document.querySelector("#chart-timeline1"), options);

                    try {
                      if (this.chart) this.chart.render();
                    } catch (e) {
                      this.chart.destroy();
                      this.chart.render();
                    }

                  case 4:
                  case "end":
                    return _context.stop();
                }
              }
            }, _callee, this);
          }));
        }
      }, {
        key: "createChartData",
        value: function createChartData() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
            var mapList, carrierList, chartHeight, mtchRec, match, nomtchRec, nomatch, partialRec, partial, dupRec, duplicate, pendingRec, pending, activeExcRec, activeExc, ignoreRec, Ignore, unmstchRec, unMatch, forcedIgnoredRec, forcedIgnored;
            return regeneratorRuntime.wrap(function _callee2$(_context2) {
              while (1) {
                switch (_context2.prev = _context2.next) {
                  case 0:
                    mapList = [];
                    carrierList = [];
                    chartHeight = 300; //Match Records

                    mtchRec = this.LFValidationList.map(function (res) {
                      return [res.MatchedRecordCount];
                    }).toString();
                    match = mtchRec && mtchRec.split(",").map(Number); //NoMatch Records

                    nomtchRec = this.LFValidationList.map(function (res) {
                      return [res.NoMatchRecordCount];
                    }).toString();
                    nomatch = nomtchRec && nomtchRec.split(",").map(Number); //Partial Match

                    partialRec = this.LFValidationList.map(function (res) {
                      return [res.PartialMatchRecordCount];
                    }).toString();
                    partial = partialRec && partialRec.split(",").map(Number); //Duplicate

                    dupRec = this.LFValidationList.map(function (res) {
                      return [res.DuplicateRecordCount];
                    }).toString();
                    duplicate = dupRec && dupRec.split(",").map(Number); //PendingMatchCount

                    pendingRec = this.LFValidationList.map(function (res) {
                      return [res.PendingMatchCount];
                    }).toString();
                    pending = pendingRec && pendingRec.split(",").map(Number); //activeException

                    activeExcRec = this.LFValidationList.map(function (res) {
                      return [res.ActiveExceptionRecordCount];
                    }).toString();
                    activeExc = activeExcRec && activeExcRec.split(",").map(Number); //IgnoredMatchRecordCount

                    ignoreRec = this.LFValidationList.map(function (res) {
                      return [res.IgnoredMatchRecordCount];
                    }).toString();
                    Ignore = ignoreRec && ignoreRec.split(",").map(Number); //UnmatchedRecordCount

                    unmstchRec = this.LFValidationList.map(function (res) {
                      return [res.UnmatchedRecordCount];
                    }).toString();
                    unMatch = unmstchRec && unmstchRec.split(",").map(Number); //ForcedIgnoreRecordCount

                    forcedIgnoredRec = this.LFValidationList.map(function (res) {
                      return [res.ForcedIgnoredMatchRecordCount];
                    }).toString();
                    forcedIgnored = forcedIgnoredRec && forcedIgnoredRec.split(",").map(Number);

                    if (!(this.LFValidationList.length > 0)) {
                      _context2.next = 48;
                      break;
                    }

                    _context2.next = 24;
                    return mapList.push({
                      name: 'Matched ',
                      data: match
                    });

                  case 24:
                    _context2.next = 26;
                    return mapList.push({
                      name: 'No Match ',
                      data: nomatch
                    });

                  case 26:
                    _context2.next = 28;
                    return mapList.push({
                      name: 'Partial Match ',
                      data: partial
                    });

                  case 28:
                    _context2.next = 30;
                    return mapList.push({
                      name: 'Pending Match ',
                      data: pending
                    });

                  case 30:
                    _context2.next = 32;
                    return mapList.push({
                      name: 'Duplicate  ',
                      data: duplicate
                    });

                  case 32:
                    _context2.next = 34;
                    return mapList.push({
                      name: 'Active Exception ',
                      data: activeExc
                    });

                  case 34:
                    _context2.next = 36;
                    return mapList.push({
                      name: 'Ignored Match ',
                      data: Ignore
                    });

                  case 36:
                    _context2.next = 38;
                    return mapList.push({
                      name: 'Forced Ignore',
                      data: forcedIgnored
                    });

                  case 38:
                    _context2.next = 40;
                    return mapList.push({
                      name: 'Unmatched  ',
                      data: unMatch
                    });

                  case 40:
                    _context2.next = 42;
                    return this.LFValidationList;

                  case 42:
                    _context2.t0 = _context2.sent;

                    if (!_context2.t0) {
                      _context2.next = 45;
                      break;
                    }

                    this.LFValidationList.map(function (lfv) {
                      carrierList.push(lfv.CarrierID ? lfv.CarrierID : '-');
                    });

                  case 45:
                    chartHeight = chartHeight + carrierList.length * 40;
                    _context2.next = 66;
                    break;

                  case 48:
                    _context2.next = 50;
                    return mapList.push({
                      name: 'Matched ',
                      data: []
                    });

                  case 50:
                    _context2.next = 52;
                    return mapList.push({
                      name: 'No Match ',
                      data: []
                    });

                  case 52:
                    _context2.next = 54;
                    return mapList.push({
                      name: 'Partial Match ',
                      data: []
                    });

                  case 54:
                    _context2.next = 56;
                    return mapList.push({
                      name: 'Pending Match ',
                      data: []
                    });

                  case 56:
                    _context2.next = 58;
                    return mapList.push({
                      name: 'Duplicate  ',
                      data: []
                    });

                  case 58:
                    _context2.next = 60;
                    return mapList.push({
                      name: 'Active Exception ',
                      data: []
                    });

                  case 60:
                    _context2.next = 62;
                    return mapList.push({
                      name: 'Ignored Match ',
                      data: []
                    });

                  case 62:
                    _context2.next = 64;
                    return mapList.push({
                      name: 'Forced Ignore  ',
                      data: []
                    });

                  case 64:
                    _context2.next = 66;
                    return mapList.push({
                      name: 'Unmatched  ',
                      data: []
                    });

                  case 66:
                    _context2.next = 68;
                    return this.RendorChart(mapList, carrierList, chartHeight);

                  case 68:
                  case "end":
                    return _context2.stop();
                }
              }
            }, _callee2, this);
          }));
        }
      }, {
        key: "openSupplierBOLReportGrid",
        value: function openSupplierBOLReportGrid() {
          window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
        }
      }, {
        key: "openCarrierBOLReportGrid",
        value: function openCarrierBOLReportGrid() {
          window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
        }
      }]);

      return CarrierComponent;
    }();

    CarrierComponent.ɵfac = function CarrierComponent_Factory(t) {
      return new (t || CarrierComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"]));
    };

    CarrierComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: CarrierComponent,
      selectors: [["app-carrier-performace"]],
      inputs: {
        LFValidationList: "LFValidationList"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 4,
      vars: 0,
      consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-md-10"], ["id", "chart-timeline1"]],
      template: function CarrierComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }
      },
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvY2Fycmllci9jYXJyaWVyLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CarrierComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-carrier-performace',
          templateUrl: './carrier.component.html',
          styleUrls: ['./carrier.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"]
        }];
      }, {
        LFValidationList: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts": function srcAppLfvDashboardLeftSideFilterLeftSideFilterComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LeftSideFilterComponent", function () {
      return LeftSideFilterComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_2__);
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");

    var LeftSideFilterComponent = /*#__PURE__*/function () {
      function LeftSideFilterComponent(_lfvSevice) {
        _classCallCheck(this, LeftSideFilterComponent);

        this._lfvSevice = _lfvSevice;
        this.DateType = 3;
        this.CarrierDrpDwnList = []; //min max date

        this.search = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this["export"] = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
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

      _createClass(LeftSideFilterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
      }, {
        key: "getLFCarrier",
        value: function getLFCarrier() {
          var _this3 = this;

          var fromDate = this.fromDate;
          var toDate = this.toDate;

          this._lfvSevice.getLFCarrier(fromDate, toDate).subscribe(function (res) {
            if (res) {
              _this3.CarrierDrpDwnList = res;
              _this3.CarrierDrpDwnList && _this3.CarrierDrpDwnList.map(function (m) {
                m.DisplayName = "".concat(m.Name, "-").concat(m.Code);
              }); //code=>carrierId
              //name=CarrierName
            } else _this3.CarrierDrpDwnList = [];
          });
        }
      }, {
        key: "changeDateType",
        value: function changeDateType(value) {
          this.DateType = value;

          if (this.DateType == 1) {
            this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-1, 'days').format('MM/DD/YYYY');
            this.isMatchingWindow = false;
          } else if (this.DateType == 3) {
            this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
            this.isMatchingWindow = false;
          } else {
            this.isMatchingWindow = true;
            var day = this.matchingWindowDays ? this.matchingWindowDays : 3; //3 is default 

            this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-day, 'days').format('MM/DD/YYYY');
          }

          this.onSearch();
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.fromDate = event;
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.toDate = event;
          this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
        }
      }, {
        key: "onCarrierSelect",
        value: function onCarrierSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
            var _this4 = this;

            return regeneratorRuntime.wrap(function _callee3$(_context3) {
              while (1) {
                switch (_context3.prev = _context3.next) {
                  case 0:
                    _context3.next = 2;
                    return this.selectedCarrierList.map(function (m) {
                      m.Name = _this4.CarrierDrpDwnList.find(function (f) {
                        return f.DisplayName == m.DisplayName;
                      }).Code;
                    });

                  case 2:
                  case "end":
                    return _context3.stop();
                }
              }
            }, _callee3, this);
          }));
        }
      }, {
        key: "onCarrierDeSelect",
        value: function onCarrierDeSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
            var _this5 = this;

            return regeneratorRuntime.wrap(function _callee4$(_context4) {
              while (1) {
                switch (_context4.prev = _context4.next) {
                  case 0:
                    _context4.next = 2;
                    return this.selectedCarrierList.map(function (m) {
                      m.Name = _this5.CarrierDrpDwnList.find(function (f) {
                        return f.DisplayName == m.DisplayName;
                      }).Code;
                    });

                  case 2:
                  case "end":
                    return _context4.stop();
                }
              }
            }, _callee4, this);
          }));
        }
      }, {
        key: "onSearch",
        value: function onSearch() {
          var startDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.fromDate, "MM/DD/YYYY");
          var endDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate, "MM/DD/YYYY");
          var result = endDate.diff(startDate, 'days');

          if (result > 8) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Date Difference should be less than 7 days", undefined, undefined);
          } else this.search.emit(true);
        }
      }, {
        key: "onExport",
        value: function onExport() {
          this["export"].emit(this.selectedStatus);
        }
      }]);

      return LeftSideFilterComponent;
    }();

    LeftSideFilterComponent.ɵfac = function LeftSideFilterComponent_Factory(t) {
      return new (t || LeftSideFilterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"]));
    };

    LeftSideFilterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: LeftSideFilterComponent,
      selectors: [["app-left-side-filter"]],
      outputs: {
        search: "search",
        "export": "export"
      },
      decls: 60,
      vars: 29,
      consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row", "mb10"], [1, "col-sm-12", "text-center", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-md-3"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "disabled", "ngModelChange", "onDateChange"], [1, "col-sm-3"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange", "onSelect", "onDeSelect"], [1, "col-sm-3", "text-right", "form-buttons"], ["id", "Submit", "type", "button", "value", "Search", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], ["id", "statusSelectbtn", "type", "button", "value", "Export", "data-toggle", "modal", "data-target", "#statusSelect", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid"], ["id", "statusSelect", "tabindex", "-1", "role", "dialog", "aria-labelledby", "statusSelectLabel", "aria-hidden", "true", 1, "modal", "fade"], [1, "modal-dialog", "modal-sm", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["id", "statusSelectLabel", 1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], ["aria-hidden", "true"], [1, "modal-body"], [1, "form-control", 3, "ngModel", "ngModelChange"], [3, "value"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "click"]],
      template: function LeftSideFilterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LeftSideFilterComponent_Template_label_click_6_listener() {
            return ctx.changeDateType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Today");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](8, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LeftSideFilterComponent_Template_label_click_9_listener() {
            return ctx.changeDateType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Matching Window");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "input", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LeftSideFilterComponent_Template_label_click_12_listener() {
            return ctx.changeDateType(3);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Day Range");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LeftSideFilterComponent_Template_input_ngModelChange_16_listener($event) {
            return ctx.fromDate = $event;
          })("onDateChange", function LeftSideFilterComponent_Template_input_onDateChange_16_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LeftSideFilterComponent_Template_input_ngModelChange_18_listener($event) {
            return ctx.toDate = $event;
          })("onDateChange", function LeftSideFilterComponent_Template_input_onDateChange_18_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "ng-multiselect-dropdown", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_ngModelChange_20_listener($event) {
            return ctx.selectedCarrierList = $event;
          })("onSelect", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_onSelect_20_listener($event) {
            return ctx.onCarrierSelect($event);
          })("onDeSelect", function LeftSideFilterComponent_Template_ng_multiselect_dropdown_onDeSelect_20_listener($event) {
            return ctx.onCarrierDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "button", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LeftSideFilterComponent_Template_button_click_22_listener() {
            return ctx.onSearch();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "button", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Export");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "h5", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "Select Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "button", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "span", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "select", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function LeftSideFilterComponent_Template_select_ngModelChange_36_listener($event) {
            return ctx.selectedStatus = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Match");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "No Match");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "Partial Match");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Pending");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "Duplicate");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48, "Active Exception");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](50, "Ignored");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](52, "Unmatched");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "option", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](54, "Forced Ignore");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "button", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](57, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "button", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function LeftSideFilterComponent_Template_button_click_58_listener() {
            return ctx.onExport();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](59, "Generate CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "type")("value", 1)("checked", ctx.DateType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "type")("value", 2)("checked", ctx.DateType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "type")("value", 3)("checked", ctx.DateType == 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.fromDate)("format", "MM/DD/YYYY")("disabled", ctx.DateType != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.toDate)("format", "MM/DD/YYYY")("disabled", ctx.DateType != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Carrier(s)")("settings", ctx.multiselectSettingsById)("data", ctx.CarrierDrpDwnList)("ngModel", ctx.selectedCarrierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.selectedStatus);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.Clean);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.NoMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.PartialMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.PendingMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.Duplicate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.ActiveExceptions);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.IgnoreMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.UnMatched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.LFVRecordStatus.ForcedIgnore);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["ɵangular_packages_forms_forms_x"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvbGVmdC1zaWRlLWZpbHRlci9sZWZ0LXNpZGUtZmlsdGVyLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](LeftSideFilterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-left-side-filter',
          templateUrl: './left-side-filter.component.html',
          styleUrls: ['./left-side-filter.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"]
        }];
      }, {
        search: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }],
        "export": [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/lfv-accrual-report/lfv-accrual-report.component.ts": function srcAppLfvDashboardLfvAccrualReportLfvAccrualReportComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LfvAccrualReportComponent", function () {
      return LfvAccrualReportComponent;
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


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function LfvAccrualReportComponent_tbody_62_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var record_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](record_r4.CallId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.RecordDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.bol, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.TerminalName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.Terminals, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.TerminalItemCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.ProductType, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.correctedQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.LoadDate, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.CarrierID, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.CarrierName, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.FileName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r4.recordStatus, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", record_r4.Username, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", record_r4.ModifiedDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", record_r4.LFVResolutionTime, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", record_r4.TimeToBol, " ");
      }
    }

    function LfvAccrualReportComponent_tbody_62_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvAccrualReportComponent_tbody_62_tr_1_Template, 35, 17, "tr", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.records);
      }
    }

    function LfvAccrualReportComponent_tbody_63_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "td", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "No Data Available");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvAccrualReportComponent_div_64_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var LfvAccrualReportComponent = /*#__PURE__*/function () {
      function LfvAccrualReportComponent(_lfvservice) {
        _classCallCheck(this, LfvAccrualReportComponent);

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

      _createClass(LfvAccrualReportComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this6 = this;

          this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
          };
          var exportColumns = {
            columns: ':visible'
          };
          var gridcolumnsDetails = [];
          gridcolumnsDetails = [{
            title: 'CallID',
            name: 'CallId',
            data: 'CallId',
            "autoWidth": true
          }, {
            title: 'Record Date',
            name: 'RecordDate',
            data: 'RecordDate',
            "autoWidth": true
          }, {
            title: 'BOL#',
            name: 'BOL',
            data: 'bol',
            "autoWidth": true
          }, {
            title: 'Terminal Code',
            name: 'TerminalName',
            data: 'TerminalName',
            "autoWidth": true
          }, {
            title: 'Terminals',
            name: 'Terminal',
            data: 'Terminals',
            "autoWidth": true
          }, {
            title: 'Terminal Item Code',
            name: 'TerminalItemCode',
            data: 'TerminalItemCode',
            "autoWidth": true
          }, {
            title: 'Product Type',
            name: 'ProductType',
            data: 'ProductType',
            "autoWidth": true
          }, {
            title: 'Corrected Quantity',
            name: 'correctedQuantity',
            data: 'correctedQuantity',
            "autoWidth": true
          }, {
            title: 'Load Date',
            name: 'LoadDate',
            data: 'LoadDate',
            "autoWidth": true
          }, {
            title: 'CarrierID',
            name: 'CarrierID',
            data: 'CarrierID',
            "autoWidth": true
          }, {
            title: 'Carrier Name',
            name: 'CarrierName',
            data: 'CarrierName',
            "autoWidth": true
          }, {
            title: 'FileName',
            name: 'FileName',
            data: 'FileName',
            "autoWidth": true
          }, //{ title: 'Reason', name: 'Reason', data: 'Reason', "autowidth": true },
          {
            title: 'Status',
            name: 'RecordStatus',
            data: 'recordStatus',
            "autowidth": true
          }, {
            title: 'User Name',
            name: 'Username',
            data: 'Username',
            "autowidth": true
          }, {
            title: 'Modified Date (MST)',
            name: 'ModifiedDate',
            data: 'ModifiedDate',
            "autowidth": true
          }, {
            title: 'Resolution Time',
            name: 'LFVResolutionTime',
            data: 'LFVResolutionTime',
            "autowidth": true
          }, {
            title: 'Time to BOL',
            name: 'TimeToBol',
            data: 'TimeToBol',
            "autowidth": true
          } //{ title: 'Reason Code', name: 'ReasonCode', data: 'ReasonCode', "autowidth": true },
          //{ title: 'Reason Category', name: 'ReasonCategory', data: 'ReasonCategory', "autowidth": true }
          ];
          this.dtOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            ajax: function ajax(dataTablesParameters, callback) {
              var inputs = {
                FromDate: _this6.FromDate,
                ToDate: _this6.ToDate,
                ProductTypeIds: _this6.ProductTypeIds
              };
              var inputData = Object.assign(dataTablesParameters, inputs);
              _this6.IsLoading = true;

              _this6._lfvservice.getLFVAccrualReportGrid(inputData).subscribe(function (resp) {
                _this6.records = resp.data;
                _this6.IsLoading = false;
                callback({
                  recordsTotal: resp.recordsTotal,
                  recordsFiltered: resp.recordsFiltered,
                  data: resp.data
                }); // this.getLFVValidationStatsAndProductTypesDDL();
              });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[0, 'desc']],
            buttons: [{
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'LiftFile Accrual Report',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'LiftFile Accrual Report',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            columns: gridcolumnsDetails
          };
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getLFAccrualGrid();
          this.dtTrigger.next();
          this.getLFVValidationStatsAndProductTypesDDL();
        }
      }, {
        key: "getLFAccrualGrid",
        value: function getLFAccrualGrid() {
          this.IsLoading = true;
          this.refreshDatatable();
          this.IsLoading = false;
        }
      }, {
        key: "getLFVValidationStatsAndProductTypesDDL",
        value: function getLFVValidationStatsAndProductTypesDDL() {
          var _this7 = this;

          this.IsLoading = true;
          var input = {
            FromDate: this.FromDate,
            ToDate: this.ToDate
          };

          this._lfvservice.GetLFVValidationStatsAndProductTypesDDL(input).subscribe(function (resp) {
            _this7.ProductTypesList = resp;
            _this7.IsLoading = false;
          });
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
        }
      }, {
        key: "ApplyFilter",
        value: function ApplyFilter() {
          if (this.FromDate != null && this.FromDate != undefined && this.FromDate != "" && (this.ToDate == null || this.ToDate == undefined || this.ToDate == "")) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("To Date is required", undefined, undefined);
            return;
          }

          this.getSelectedProductTypes();
          this.refreshDatatable();
        }
      }, {
        key: "ClearFilter",
        value: function ClearFilter() {
          this.FromDate = null;
          this.ToDate = null;
          this.selectedProductTypesList = [];
          this.getSelectedProductTypes();
          this.refreshDatatable();
        }
      }, {
        key: "getSelectedProductTypes",
        value: function getSelectedProductTypes() {
          if (this.selectedProductTypesList == null || this.selectedProductTypesList.length == 0 || this.selectedProductTypesList == undefined) {
            this.ProductTypeIds = "";
          } else if (this.selectedProductTypesList != null || this.selectedProductTypesList.length > 0) {
            this.ProductTypeIds = this.selectedProductTypesList.map(function (m) {
              return m.Id;
            }).join(',');
          }
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.FromDate = event;
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.ToDate = event;
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtTrigger.unsubscribe();
        }
      }]);

      return LfvAccrualReportComponent;
    }();

    LfvAccrualReportComponent.ɵfac = function LfvAccrualReportComponent_Factory(t) {
      return new (t || LfvAccrualReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__["LiftfiledashboardserviceService"]));
    };

    LfvAccrualReportComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: LfvAccrualReportComponent,
      selectors: [["app-lfv-accrual-report"]],
      viewQuery: function LfvAccrualReportComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      decls: 65,
      vars: 14,
      consts: [[1, "row", "mb10"], [1, "col-sm-12"], [1, "well", "pb10", "mb0"], [1, "row"], [1, "col-sm-1", "pr0", "mt-1"], [1, "fa", "fa-filter", "mr5", "fs16"], [1, "f-normal", "fs16"], [1, "col-sm-2"], ["type", "text", "placeholder", "From", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "To", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "ngModelChange", "onDateChange"], [1, "col-md-3"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-sm-2", "mt5-xs"], ["type", "button", "value", "Apply", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "Clear Filter", 1, "btn", "ml5", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "accrualreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "RecordDate"], ["data-key", "Bol"], ["data-key", "TerminalCode"], ["data-key", "Terminals"], ["data-key", "TerminalItemCode"], ["data-key", "ProductType"], ["data-key", "CorrectedQuanity"], ["data-key", "LoadDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "FileName"], ["data-key", "RecordStatus"], ["data-key", "UserName"], ["data-key", "ModifiedDate"], ["data-key", "LFVResolutionTime"], ["data-key", "TimeToBol"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], ["colspan", "14", 1, "text-center"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function LfvAccrualReportComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Filter");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvAccrualReportComponent_Template_input_ngModelChange_9_listener($event) {
            return ctx.FromDate = $event;
          })("onDateChange", function LfvAccrualReportComponent_Template_input_onDateChange_9_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvAccrualReportComponent_Template_input_ngModelChange_11_listener($event) {
            return ctx.ToDate = $event;
          })("onDateChange", function LfvAccrualReportComponent_Template_input_onDateChange_11_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "span");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvAccrualReportComponent_Template_ng_multiselect_dropdown_ngModelChange_14_listener($event) {
            return ctx.selectedProductTypesList = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvAccrualReportComponent_Template_input_click_16_listener() {
            return ctx.ApplyFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvAccrualReportComponent_Template_input_click_17_listener() {
            return ctx.ClearFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "table", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "CallID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30, "Record Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "BOL#");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Terminal Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Product Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](42, "Corrected Quanity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "th", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "th", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](48, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "th", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](50, "File Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](51, "--> ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](55, "User Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Modified Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](59, "Resolution Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](61, "Time to BOL");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](62, LfvAccrualReportComponent_tbody_62_Template, 2, 1, "tbody", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](63, LfvAccrualReportComponent_tbody_63_Template, 4, 0, "tbody", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](64, LfvAccrualReportComponent_div_64_Template, 5, 0, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.FromDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.ToDate)("format", "MM/DD/YYYY")("minDate", ctx.FromDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Products")("settings", ctx.multiselectSettingsById)("data", ctx.ProductTypesList)("ngModel", ctx.selectedProductTypesList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.records == null ? null : ctx.records.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.records == null ? null : ctx.records.length) == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgModel"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["MultiSelectComponent"], angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"]],
      styles: [".dataTables_empty[_ngcontent-%COMP%] {\r\n    display: none;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtYWNjcnVhbC1yZXBvcnQvbGZ2LWFjY3J1YWwtcmVwb3J0LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxhQUFhO0FBQ2pCIiwiZmlsZSI6InNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtYWNjcnVhbC1yZXBvcnQvbGZ2LWFjY3J1YWwtcmVwb3J0LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuZGF0YVRhYmxlc19lbXB0eSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG59XHJcbiJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LfvAccrualReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-lfv-accrual-report',
          templateUrl: './lfv-accrual-report.component.html',
          styleUrls: ['./lfv-accrual-report.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_4__["LiftfiledashboardserviceService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/lfv-dashboard.module.ts": function srcAppLfvDashboardLfvDashboardModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LfvDashboardModule", function () {
      return LfvDashboardModule;
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
    "./src/app/lfv-dashboard/master/master.component.ts");
    /* harmony import */


    var _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./validation/validation.component */
    "./src/app/lfv-dashboard/validation/validation.component.ts");
    /* harmony import */


    var _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./carrier/carrier.component */
    "./src/app/lfv-dashboard/carrier/carrier.component.ts");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./left-side-filter/left-side-filter.component */
    "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts");
    /* harmony import */


    var _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./lfv-scratch-report/lfv-scratch-report.component */
    "./src/app/lfv-dashboard/lfv-scratch-report/lfv-scratch-report.component.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./carrier-bol-report/carrier-bol-report.component */
    "./src/app/lfv-dashboard/carrier-bol-report/carrier-bol-report.component.ts");
    /* harmony import */


    var _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./supplier-bol-report/supplier-bol-report.component */
    "./src/app/lfv-dashboard/supplier-bol-report/supplier-bol-report.component.ts");
    /* harmony import */


    var _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ./lfv-accrual-report/lfv-accrual-report.component */
    "./src/app/lfv-dashboard/lfv-accrual-report/lfv-accrual-report.component.ts");

    var route = [{
      path: '',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"]
    }, {
      path: 'Dashboard',
      component: _master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"]
    }, {
      path: 'LFVScratchReport',
      component: _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"]
    }, {
      path: 'CarrierBolReport',
      component: _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"]
    }, {
      path: 'SupplierBolReport',
      component: _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"]
    }, {
      path: 'LFVAccrualReport',
      component: _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"]
    }];

    var LfvDashboardModule = function LfvDashboardModule() {
      _classCallCheck(this, LfvDashboardModule);
    };

    LfvDashboardModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: LfvDashboardModule
    });
    LfvDashboardModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function LfvDashboardModule_Factory(t) {
        return new (t || LfvDashboardModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route), _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](LfvDashboardModule, {
        declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__["CarrierComponent"], _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__["LeftSideFilterComponent"], _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"], _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"], _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"], _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LfvDashboardModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_master_master_component__WEBPACK_IMPORTED_MODULE_2__["MasterComponent"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_3__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_4__["CarrierComponent"], _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_8__["LeftSideFilterComponent"], _lfv_scratch_report_lfv_scratch_report_component__WEBPACK_IMPORTED_MODULE_9__["LfvScratchReportComponent"], _carrier_bol_report_carrier_bol_report_component__WEBPACK_IMPORTED_MODULE_12__["CarrierBolReportComponent"], _supplier_bol_report_supplier_bol_report_component__WEBPACK_IMPORTED_MODULE_13__["SupplierBolReportComponent"], _lfv_accrual_report_lfv_accrual_report_component__WEBPACK_IMPORTED_MODULE_14__["LfvAccrualReportComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_5__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_10__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route), _modules_directive_module__WEBPACK_IMPORTED_MODULE_11__["DirectiveModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/lfv-scratch-report/lfv-scratch-report.component.ts": function srcAppLfvDashboardLfvScratchReportLfvScratchReportComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LfvScratchReportComponent", function () {
      return LfvScratchReportComponent;
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


    var _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../LiftFileModels */
    "./src/app/lfv-dashboard/LiftFileModels.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function LfvScratchReportComponent_div_39_div_1_ng_container_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "option", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var bol_r7 = ctx.$implicit;

        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", bol_r7.Id)("selected", bol_r7.Id == ctx_r6.bolResolveForm.get("InvoiceFtlDetailId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", bol_r7.Name, " ");
      }
    }

    function LfvScratchReportComponent_div_39_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "select", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function LfvScratchReportComponent_div_39_div_1_Template_select_change_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r8.GetBolRecord($event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "option", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Select BOL-Product to edit ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, LfvScratchReportComponent_div_39_div_1_ng_container_5_Template, 3, 3, "ng-container", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r5.InvoiceFtlDetailIdList);
      }
    }

    function LfvScratchReportComponent_div_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_div_39_div_1_Template, 6, 2, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.InvoiceFtlDetailIdList != null && ctx_r0.InvoiceFtlDetailIdList.length > 0);
      }
    }

    function LfvScratchReportComponent_content_40_div_14_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " BOL/LiftTicket# is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_14_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r10.bolResolveForm.get("BolNumber").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_div_22_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Lift Date is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_22_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.bolResolveForm.get("LiftDate").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_div_30_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Gross quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_30_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r12.bolResolveForm.get("GrossQuantity").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_div_38_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Net quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_38_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.bolResolveForm.get("NetQuantity").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_div_47_div_5_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_47_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_47_div_5_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r20.bolResolveForm.get("SelectedTerminal").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_div_47_Template(rf, ctx) {
      if (rf & 1) {
        var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Terminal Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvScratchReportComponent_content_40_div_47_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r23);

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r22.SelectedTerminalList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, LfvScratchReportComponent_content_40_div_47_div_5_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal")("settings", ctx_r14.multiselectSettingsById)("data", ctx_r14.TerminalList)("ngModel", ctx_r14.SelectedTerminalList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r14.bolResolveForm.get("SelectedTerminal").invalid && (ctx_r14.bolResolveForm.get("SelectedTerminal").dirty || ctx_r14.bolResolveForm.get("SelectedTerminal").touched));
      }
    }

    function LfvScratchReportComponent_content_40_div_53_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Fuel Type is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_content_40_div_53_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_content_40_div_53_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r15.bolResolveForm.get("SelectedFuelType").errors.required);
      }
    }

    function LfvScratchReportComponent_content_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "content", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "form", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function LfvScratchReportComponent_content_40_Template_form_ngSubmit_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r25.onSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "BOL/LiftTicket#");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "input", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "input", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "input", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, LfvScratchReportComponent_content_40_div_14_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Lift Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "input", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function LfvScratchReportComponent_content_40_Template_input_onDateChange_21_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r27.bolResolveForm.get("LiftDate").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, LfvScratchReportComponent_content_40_div_22_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "label", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Gross Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](29, "input", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, LfvScratchReportComponent_content_40_div_30_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "label", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Net Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](37, "input", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](38, LfvScratchReportComponent_content_40_div_38_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "label", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](43, "Badge#");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "input", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](47, LfvScratchReportComponent_content_40_div_47_Template, 6, 5, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](51, "Fuel");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "ng-multiselect-dropdown", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvScratchReportComponent_content_40_Template_ng_multiselect_dropdown_ngModelChange_52_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r28.SelectedFuelTypeList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](53, LfvScratchReportComponent_content_40_div_53_Template, 2, 1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "label", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Notes");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](58, "textarea", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "button", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_content_40_Template_button_click_60_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r29._toggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](61, "Cancel");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "button", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](63, "Save & Re-Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx_r1.bolResolveForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.bolResolveForm.get("BolNumber").invalid && (ctx_r1.bolResolveForm.get("BolNumber").dirty || ctx_r1.bolResolveForm.get("BolNumber").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.bolResolveForm.get("LiftDate").invalid && (ctx_r1.bolResolveForm.get("LiftDate").dirty || ctx_r1.bolResolveForm.get("LiftDate").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.bolResolveForm.get("GrossQuantity").invalid && (ctx_r1.bolResolveForm.get("GrossQuantity").dirty || ctx_r1.bolResolveForm.get("GrossQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.bolResolveForm.get("NetQuantity").invalid && (ctx_r1.bolResolveForm.get("NetQuantity").dirty || ctx_r1.bolResolveForm.get("NetQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r1.bolResolveForm.get("IsBulkPlantLift").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Fuel")("settings", ctx_r1.multiselectSettingsById)("data", ctx_r1.FuelTypeList)("ngModel", ctx_r1.SelectedFuelTypeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.bolResolveForm.get("SelectedFuelType").invalid && (ctx_r1.bolResolveForm.get("SelectedFuelType").dirty || ctx_r1.bolResolveForm.get("SelectedFuelType").touched));
      }
    }

    function LfvScratchReportComponent_div_41_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_tbody_92_tr_1_span_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_tbody_92_tr_1_span_40_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r37);

          var record_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r35.getBolDetailsForResolve(record_r31);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_tbody_92_tr_1_span_41_Template(rf, ctx) {
      if (rf & 1) {
        var _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_tbody_92_tr_1_span_41_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r39);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

          return ctx_r38.redirectToMyApprovalTab();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function LfvScratchReportComponent_tbody_92_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "td", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](38, "input", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "td", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](40, LfvScratchReportComponent_tbody_92_tr_1_span_40_Template, 3, 0, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](41, LfvScratchReportComponent_tbody_92_tr_1_span_41_Template, 3, 0, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var record_r31 = ctx.$implicit;

        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.CallId, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](record_r31.bol);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.TerminalName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.Terminals, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.correctedQuantity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.TerminalItemCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.ProductType, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.LoadDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.RecordDate, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.CarrierID, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.CarrierName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.Reason, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.ReasonCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.ReasonCategory, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.recordStatus, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.Username, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.ModifiedDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r31.LFVResolutionTime, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", record_r31.LiftFileRecordId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", record_r31.LiftFileRecordId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("checked", ctx_r30.isChecked);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", record_r31.Status == 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", record_r31.Status == 6);
      }
    }

    function LfvScratchReportComponent_tbody_92_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LfvScratchReportComponent_tbody_92_tr_1_Template, 42, 23, "tr", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r3.LFRecords);
      }
    }

    function LfvScratchReportComponent_div_98_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var LfvScratchReportComponent = /*#__PURE__*/function () {
      function LfvScratchReportComponent(fb, dashboardservice) {
        _classCallCheck(this, LfvScratchReportComponent);

        this.fb = fb;
        this.dashboardservice = dashboardservice; //side bar related variables

        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom']; //grid variables

        this.LFRecords = [];
        this.cancelButtonText = 'No';
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
        this.isChecked = false;
        this.LFRecordIdsForIgnoreMatch = [];
        this.ShowSideBarLoader = false;
        this.SelectedTerminalList = [];
        this.SelectedFuelTypeList = []; //ignore by reason

        this.preferenceSetting = null;
        this.selectedReason = [];
        this.reasonList = [];
        this.dropdownSettings = {
          singleSelection: true,
          idField: 'Id',
          textField: 'Name',
          allowSearchFilter: true
        };
      }

      _createClass(LfvScratchReportComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
      }, {
        key: "intializeGrid",
        value: function intializeGrid() {
          this.ShowGridLoader = true;
          var exportColumns = {
            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
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
              title: 'Scratch Report',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Scratch Report',
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
          this.getLFRecords();
        }
      }, {
        key: "buildForm",
        value: function buildForm() {
          var fg = this.fb.group({
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
      }, {
        key: "getLFRecords",
        value: function getLFRecords() {
          var _this8 = this;

          this.ShowGridLoader = true;
          this.dashboardservice.getLFRecords().subscribe(function (data) {
            _this8.ShowGridLoader = false;
            _this8.LFRecords = data;

            _this8.dtTrigger.next();
          });
        }
      }, {
        key: "reloadGrid",
        value: function reloadGrid() {
          $("#liftfilerecords-datatable").DataTable().clear().destroy();
          this.getLFRecords();
        }
      }, {
        key: "getBolDetailsForResolve",
        value: function getBolDetailsForResolve(lfRecord) {
          var _this9 = this;

          this.ShowSideBarLoader = true;
          lfRecord.IsFromScratchReport = true;
          this.dashboardservice.getBolDetailsForResolve(lfRecord).subscribe(function (data) {
            if (data) {
              _this9._toggleOpened(true);

              _this9.selectedLiftFileRecord = data.LiftRecord;
              _this9.TerminalList = data.TerminalList;
              _this9.FuelTypeList = data.FuelTypeList;
              _this9.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;

              _this9.initFormData(data);

              _this9.ShowSideBarLoader = false;
            }
          });
        }
      }, {
        key: "selectAllRecords",
        value: function selectAllRecords(eventData) {
          if (eventData != null && eventData != undefined) {
            if (eventData.target.checked) {
              this.isChecked = true;
            } else {
              this.isChecked = false;
            }
          }
        }
      }, {
        key: "ValidateForIgnoreMatchProcessing",
        value: function ValidateForIgnoreMatchProcessing() {
          var LFRecordIds = this.getLFRecordIds();
          this.selectedReason = [];

          if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {
            if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
              this.GetReasonDescriptionList();
              document.getElementById('openIgnoreModal2').click();
            } else {
              this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
            }
          } else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("No Records selected", undefined, undefined);
          }
        }
      }, {
        key: "addRecordsForForcedIgnoreMatchProcessing",
        value: function addRecordsForForcedIgnoreMatchProcessing(LFRecordIds) {
          var _this10 = this;

          var descriptionId = 0;
          var descriptionText = '';

          if (this.selectedReason && this.selectedReason.length > 0) {
            descriptionId = this.selectedReason[0].Id;
            descriptionText = this.selectedReason[0].Name;
          }

          this.ShowSideBarLoader = true;
          this.dashboardservice.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe(function (response) {
            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this10.reloadGrid();
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
          });
          this.ShowSideBarLoader = false;
        }
      }, {
        key: "GetBolRecord",
        value: function GetBolRecord(InvoiceFtlDetailId) {
          if (InvoiceFtlDetailId != null && InvoiceFtlDetailId != undefined && InvoiceFtlDetailId != '') {
            var selectedLiftFileRecordId = this.selectedLiftFileRecord.LiftFileRecordId;
            var invoiceFtlDetailId = parseInt(InvoiceFtlDetailId);
            var LFRecord = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFRecordGridModel"]();
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
      }, {
        key: "initFormData",
        value: function initFormData(data) {
          var currObj = this;
          this.bolResolveForm.reset(); //clear previous values

          if (this.bolResolveForm != null && this.bolResolveForm != undefined && data != null && data != undefined) {
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

            if (data.IsBulkPlantLift == true) {
              // no terminal dropdown for pickup from bulk plants
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
      }, {
        key: "createPostObject",
        value: function createPostObject() {
          var inputPostObject = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFBolEditModel"]();
          inputPostObject.BadgeNumber = this.bolResolveForm.get('BadgeNumber').value;
          inputPostObject.BolNumber = this.bolResolveForm.get('BolNumber').value;
          inputPostObject.GrossQuantity = this.bolResolveForm.get('GrossQuantity').value;
          inputPostObject.InvoiceFtlDetailId = this.bolResolveForm.get('InvoiceFtlDetailId').value;
          inputPostObject.IsBulkPlantLift = this.bolResolveForm.get('IsBulkPlantLift').value;
          inputPostObject.LiftRecord.LiftFileRecordId = this.bolResolveForm.get('LIftFileRecordId').value;
          inputPostObject.LiftDate = this.bolResolveForm.get('LiftDate').value;
          inputPostObject.NetQuantity = this.bolResolveForm.get('NetQuantity').value;
          inputPostObject.Notes = this.bolResolveForm.get('Notes').value;
          var SelectedFuelType = this.bolResolveForm.get('SelectedFuelType').value;
          var fuelTypeId = SelectedFuelType[0].Id;
          inputPostObject.FuelTypeId = fuelTypeId;
          var selectedTerminal = this.bolResolveForm.get('SelectedTerminal').value;
          var terminalId = inputPostObject.IsBulkPlantLift ? selectedTerminal.Id : selectedTerminal[0].Id;
          inputPostObject.TerminalId = terminalId;
          return inputPostObject;
        } //resetSelections(isReset: boolean) {
        //    if (isReset) {
        //        this.isChecked = false;
        //    }
        //    else {
        //        this.isChecked = true;
        //    }
        //}

      }, {
        key: "redirectToMyApprovalTab",
        value: function redirectToMyApprovalTab() {
          window.open("Supplier/Exception/Manage", "_blank");
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var _this11 = this;

          this.ShowSideBarLoader = true;
          this.bolResolveForm.markAsTouched();

          if (this.bolResolveForm.valid) {
            var requestObj = this.createPostObject();

            if (requestObj != null) {
              this.dashboardservice.saveBolDetailsForResolve(requestObj).subscribe(function (response) {
                if (response.StatusCode == 0) {
                  _this11.ShowSideBarLoader = false;
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

                  _this11._toggleOpened(false);

                  _this11.reloadGrid();
                } else {
                  _this11.ShowSideBarLoader = false;
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                }
              });
            }
          }

          this.ShowSideBarLoader = false;
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
            this.bolResolveForm.reset();
          }
        }
      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.dtTrigger.unsubscribe();
        }
      }, {
        key: "getLFRecordIds",
        value: function getLFRecordIds() {
          var LFRecordIds = [];
          var table = $('#liftfilerecords-datatable').DataTable();
          var rowcollection = table.$(".dt-checkbox", {
            "page": "all"
          });
          rowcollection.each(function (index, elem) {
            if ($(this).is(":checked")) {
              LFRecordIds.push(parseInt($(this).attr('id')));
            }
          });
          return LFRecordIds;
        }
      }, {
        key: "getPreferencesSetting",
        value: function getPreferencesSetting() {
          var _this12 = this;

          if (!this.preferenceSetting) {
            this.dashboardservice.getPreferencesSetting().subscribe(function (response) {
              _this12.preferenceSetting = response;
            });
          }
        }
      }, {
        key: "GetReasonDescriptionList",
        value: function GetReasonDescriptionList() {
          var _this13 = this;

          if (this.reasonList && this.reasonList.length == 0) {
            this.ShowGridLoader = true;
            this.dashboardservice.GetReasonDescriptionList().subscribe(function (response) {
              if (response && response.length > 0) {
                _this13.reasonList = response;
              }

              _this13.ShowGridLoader = false;
            });
          }
        }
      }, {
        key: "submitIgnoreDescription",
        value: function submitIgnoreDescription() {
          this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
        }
      }]);

      return LfvScratchReportComponent;
    }();

    LfvScratchReportComponent.ɵfac = function LfvScratchReportComponent_Factory(t) {
      return new (t || LfvScratchReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"]));
    };

    LfvScratchReportComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: LfvScratchReportComponent,
      selectors: [["app-lfv-scratch-report"]],
      decls: 116,
      vars: 21,
      consts: [[1, "Lfv-resolve-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], [1, "well", "bg-white", "shadow-b", "lfrecord-section"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border", "no-padding"], ["id", "LFrecord", 1, "table-responsive"], ["id", "table-Lfrecord", 1, "table", "table-striped", "table-bordered", "table-hover", "lfvrecord"], [1, "thead-light"], ["class", "row", 4, "ngIf"], ["class", "pr30", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "liftfilerecords-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "BolNumber"], ["data-key", "Terminal"], ["data-key", "Terminals"], ["data-key", "CorrectedQuantity"], ["data-key", "TerminalITemCode"], ["data-key", "ProductType"], ["data-key", "LoadDate"], ["data-key", "RecordDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "Reason"], ["data-key", "ReasonCode"], ["data-key", "ReasonCategory"], ["data-key", "RecordStatus"], ["data-key", "ModifiedBy"], ["data-key", "ModifiedDate"], ["data-key", "LFVResolutionTime"], ["data-key", "SelectAll"], ["type", "checkbox", "id", "select-all-records", "value", "select-all-records", 3, "click"], ["data-key", "Action"], [4, "ngIf"], [1, "col-sm-12", "text-right", "mb25", "btn-wrapper"], [1, "form-group", "col-sm-12"], ["type", "button", "id", "btnCancel", "value", "Cancel", 1, "btn", "btn-default"], ["type", "button", "value", "Ignore", "id", "btnForceIgnoreRecords", 1, "btn", "btn-primary", 3, "click"], ["type", "hidden", "id", "openIgnoreModal2", "data-toggle", "modal", "data-target", "#ignoreModal2"], ["id", "ignoreModal2", "tabindex", "-1", "role", "dialog", "aria-hidden", "true", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], ["aria-hidden", "true"], [1, "modal-body"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "disabled", "click"], ["class", "col-sm-6", 4, "ngIf"], [1, "col-sm-6"], [1, "form-group"], ["id", "select-bol", 1, "form-control", 3, "change"], [3, "value"], [4, "ngFor", "ngForOf"], [3, "value", "selected"], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "col-sm-12", "section-bol-details-edit"], [1, "mt10", "row"], [1, "col-sm-3", "bol"], ["for", "BolNumber"], [1, "color-maroon"], ["formControlName", "InvoiceFtlDetailId", "type", "hidden", 1, "hide-element"], ["formControlName", "LIftFileRecordId", "type", "hidden", 1, "hide-element"], ["formControlName", "BolNumber", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3", "lifdt"], ["for", "LiftDate"], ["name", "LiftDate", "formControlName", "LiftDate", "myDatePicker", "", 1, "form-control", 3, "format", "onDateChange"], [1, "col-sm-3", "grossQty"], ["for", "GrossQuantity"], ["formControlName", "GrossQuantity", 1, "form-control"], [1, "col-sm-3", "netQty"], ["for", "NetQuantity"], ["formControlName", "NetQuantity", 1, "form-control"], [1, "col-sm-3"], ["for", "BadgeNumber"], ["formControlName", "BadgeNumber", 1, "form-control"], [1, "col"], ["class", "col-sm-6 terminal-section", 4, "ngIf"], [1, "col-sm-6", "fuelType"], ["for", "Jobs"], ["formControlName", "SelectedFuelType", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Notes"], ["formControlName", "Notes", 1, "form-control"], [1, "col-sm-12", "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], [1, "col-sm-6", "terminal-section"], ["formControlName", "SelectedTerminal", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "text-center"], ["type", "checkbox", 1, "dt-checkbox", 3, "id", "checked", "value"], ["type", "button", 1, "btn", "btn-link", 3, "click"], ["title", "Resolve Partial Match", 1, "fas", "fa-edit", "fs16"], ["title", "Resolve Exception", 1, "fas", "fa-edit", "fs16"]],
      template: function LfvScratchReportComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "ng-sidebar", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("openedChange", function LfvScratchReportComponent_Template_ng_sidebar_openedChange_2_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_Template_a_click_3_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "h3", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Edit BOL Details");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "table", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "thead", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "BOL#");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Corrected Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "ProductType");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](39, LfvScratchReportComponent_div_39_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](40, LfvScratchReportComponent_content_40_Template, 64, 12, "content", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](41, LfvScratchReportComponent_div_41_Template, 4, 0, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "table", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](52, "CallID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](54, "Bol#");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](56, "Terminal Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](57, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](58, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](60, "Corrected Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](62, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](64, "Product Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](66, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](68, "Record Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](69, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](70, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](71, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](72, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "th", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](74, "Reason");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](75, "th", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](76, "Reason Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](77, "th", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](78, "Reason Category");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](79, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](80, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](81, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](82, "Modified By");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](83, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](84, "Modified Date (MST)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](85, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](86, "Resolution Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](87, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](88, "SelectAll ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](89, "input", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_Template_input_click_89_listener($event) {
            return ctx.selectAllRecords($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "th", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](91, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](92, LfvScratchReportComponent_tbody_92_Template, 2, 1, "tbody", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](93, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](95, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](96, "input", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](97, "input", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_Template_input_click_97_listener() {
            return ctx.ValidateForIgnoreMatchProcessing();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](98, LfvScratchReportComponent_div_98_Template, 5, 0, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](99, "div", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](100, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](101, "div", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "div", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](103, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](104, "h4", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](105, "Select Reason");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](106, "button", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](107, "span", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](108, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](109, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](110, "ng-multiselect-dropdown", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function LfvScratchReportComponent_Template_ng_multiselect_dropdown_ngModelChange_110_listener($event) {
            return ctx.selectedReason = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](111, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](112, "button", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](113, "Cancel");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](114, "button", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function LfvScratchReportComponent_Template_button_click_114_listener() {
            return ctx.submitIgnoreDescription();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](115, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.bol);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalName);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.correctedQuantity);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalItemCode);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.LoadDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.ProductType);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null && ctx.bolResolveForm.get("InvoiceFtlDetailId").value > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.ShowSideBarLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.LFRecords == null ? null : ctx.LFRecords.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.ShowGridLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Reason")("settings", ctx.dropdownSettings)("data", ctx.reasonList)("ngModel", ctx.selectedReason);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", ctx.selectedReason && ctx.selectedReason.length == 0);
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["Sidebar"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"]],
      styles: ["aside {\r\n    width: 52% !important;\r\n    z-index: 99 !important;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9sZnYtc2NyYXRjaC1yZXBvcnQvbGZ2LXNjcmF0Y2gtcmVwb3J0LmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IjtBQUNBOzs7RUFHRTs7QUFFRjtJQUNJLHFCQUFxQjtJQUNyQixzQkFBc0I7QUFDMUIiLCJmaWxlIjoic3JjL2FwcC9sZnYtZGFzaGJvYXJkL2xmdi1zY3JhdGNoLXJlcG9ydC9sZnYtc2NyYXRjaC1yZXBvcnQuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIlxyXG4vKmFzaWRlLm5nLXNpZGViYXItLW9wZW5lZCB7XHJcbiAgICB3aWR0aDogODAwcHggIWltcG9ydGFudDtcclxuICAgIHotaW5kZXg6IDMgIWltcG9ydGFudDtcclxufSovXHJcblxyXG46Om5nLWRlZXAgYXNpZGUge1xyXG4gICAgd2lkdGg6IDUyJSAhaW1wb3J0YW50O1xyXG4gICAgei1pbmRleDogOTkgIWltcG9ydGFudDtcclxufSJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LfvScratchReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-lfv-scratch-report',
          templateUrl: './lfv-scratch-report.component.html',
          styleUrls: ['./lfv-scratch-report.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]
        }, {
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_5__["LiftfiledashboardserviceService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/master/master.component.ts": function srcAppLfvDashboardMasterMasterComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MasterComponent", function () {
      return MasterComponent;
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


    var _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../left-side-filter/left-side-filter.component */
    "./src/app/lfv-dashboard/left-side-filter/left-side-filter.component.ts");
    /* harmony import */


    var _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../LiftFileModels */
    "./src/app/lfv-dashboard/LiftFileModels.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular7-csv/dist/Angular-csv */
    "./node_modules/angular7-csv/dist/Angular-csv.js");
    /* harmony import */


    var angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__);
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var _validation_validation_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ../validation/validation.component */
    "./src/app/lfv-dashboard/validation/validation.component.ts");
    /* harmony import */


    var _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ../carrier/carrier.component */
    "./src/app/lfv-dashboard/carrier/carrier.component.ts");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    var _c0 = ["btnOpenModal"];

    function MasterComponent_div_14_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_14_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r13.openLFVScratchReportGrid();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_div_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_15_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.openAccrualReportGrid();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_div_16_div_5_Template(rf, ctx) {
      if (rf & 1) {
        var _r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "a", 46, 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_16_div_5_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r23.toggleRecordSearchControls(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "input", 105, 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function MasterComponent_div_16_div_5_Template_input_ngModelChange_6_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r25.bolSearchQuery = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "input", 107, 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function MasterComponent_div_16_div_5_Template_input_ngModelChange_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r26.fileNameSearchQuery = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "button", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_16_div_5_Template_button_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r24);

          var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r27.searchLiftFileRecords();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Search");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](14, "button", 111, 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r18.bolSearchQuery);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r18.fileNameSearchQuery);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("hidden", true);
      }
    }

    function MasterComponent_div_16_Template(rf, ctx) {
      if (rf & 1) {
        var _r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "button", 96, 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_16_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r29);

          var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r28.toggleRecordSearchControls(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "i", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Search Records");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](5, MasterComponent_div_16_div_5_Template, 16, 3, "div", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.showSearchControls);
      }
    }

    function MasterComponent_div_17_Template(rf, ctx) {
      if (rf & 1) {
        var _r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "a", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_17_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r30.openCarrierBOLReportGrid();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_17_Template_a_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r31);

          var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r32.openSupplierBOLReportGrid();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_app_validation_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "app-validation", 117);
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("LFValidationList", ctx_r4.LFValidationList);
      }
    }

    function MasterComponent_app_carrier_performace_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "app-carrier-performace", 117);
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("LFValidationList", ctx_r5.LFValidationList);
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "highlight-record": a0
      };
    };

    var _c2 = function _c2(a0, a1) {
      return {
        "highlight-record": a0,
        "hide-element": a1
      };
    };

    var _c3 = function _c3(a0) {
      return {
        "hide-element": a0
      };
    };

    var _c4 = function _c4(a0, a1) {
      return {
        "hide-element": a0,
        "highlight-record": a1
      };
    };

    function MasterComponent_tr_110_Template(rf, ctx) {
      if (rf & 1) {
        var _r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "td", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](36, "input", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "button", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_tr_110_Template_button_click_38_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r35);

          var item_r33 = ctx.$implicit;

          var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r34.getBolDetailsForResolve(item_r33);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](39, "i", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "td", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "button", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_tr_110_Template_button_click_41_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r35);

          var item_r33 = ctx.$implicit;

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r36.editLiftFileRecord(item_r33);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](42, "i", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r33 = ctx.$implicit;

        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](40, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.bol);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](42, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.TerminalName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](44, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.Terminals);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](46, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.correctedQuantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](48, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.TerminalItemCode);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](50, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.ProductType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](52, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.LoadDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](54, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.RecordDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](56, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.CarrierID);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](58, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.CarrierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](60, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.Reason);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](62, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PendingMatch));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", item_r33 == null ? null : item_r33.ReasonCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](65, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate || ctx_r6.gridType == ctx_r6.LFVRecordStatus.PendingMatch));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", item_r33 == null ? null : item_r33.ReasonCategory, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](68, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.Username);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](70, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.ModifiedDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](72, _c1, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.LFVResolutionTime);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](74, _c2, (item_r33.Status == ctx_r6.LFVRecordStatus.Clean || item_r33.Status == ctx_r6.LFVRecordStatus.IgnoreMatch || item_r33.Status == ctx_r6.LFVRecordStatus.ForcedIgnore) && !item_r33.IsRecordPushedToExternalApi, ctx_r6.gridType == ctx_r6.LFVRecordStatus.PartialMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean ? false : true));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r33 == null ? null : item_r33.TimeToBol);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](77, _c3, !(ctx_r6.gridType == ctx_r6.LFVRecordStatus.NoMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.UnMatched || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Duplicate)));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate"]("id", item_r33.LiftFileRecordId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate"]("value", item_r33.LiftFileRecordId);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("checked", ctx_r6.isChecked);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](79, _c3, ctx_r6.gridType != ctx_r6.LFVRecordStatus.PartialMatch));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction2"](81, _c4, ctx_r6.gridType == ctx_r6.LFVRecordStatus.IgnoreMatch || ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean || ctx_r6.gridType == ctx_r6.LFVRecordStatus.ForcedIgnore || !item_r33.IsAdminUser, ctx_r6.gridType == ctx_r6.LFVRecordStatus.Clean && !item_r33.IsRecordPushedToExternalApi));
      }
    }

    function MasterComponent_div_111_input_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "input", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_111_input_3_Template_input_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r40);

          var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r39.ValidateForIgnoreMatchProcessing("ignore");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_div_111_input_4_Template(rf, ctx) {
      if (rf & 1) {
        var _r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "input", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_div_111_input_4_Template_input_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r42);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r41.ValidateForIgnoreMatchProcessing("reprocess");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_div_111_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, MasterComponent_div_111_input_3_Template, 1, 0, "input", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, MasterComponent_div_111_input_4_Template, 1, 0, "input", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.gridType == ctx_r7.LFVRecordStatus.NoMatch || ctx_r7.gridType == ctx_r7.LFVRecordStatus.UnMatched);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.gridType == ctx_r7.LFVRecordStatus.Duplicate || ctx_r7.gridType == ctx_r7.LFVRecordStatus.UnMatched);
      }
    }

    function MasterComponent_div_151_div_1_ng_container_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "option", 135);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var bol_r45 = ctx.$implicit;

        var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", bol_r45.Id)("selected", bol_r45.Id == ctx_r44.bolResolveForm.get("InvoiceFtlDetailId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", bol_r45.Name, " ");
      }
    }

    function MasterComponent_div_151_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 131);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "select", 133);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function MasterComponent_div_151_div_1_Template_select_change_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r47);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r46.GetBolRecord($event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "option", 134);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Select BOL-Product to edit ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](5, MasterComponent_div_151_div_1_ng_container_5_Template, 3, 3, "ng-container", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r43.InvoiceFtlDetailIdList);
      }
    }

    function MasterComponent_div_151_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_div_151_div_1_Template, 6, 2, "div", 130);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r8.InvoiceFtlDetailIdList != null && ctx_r8.InvoiceFtlDetailIdList.length > 0);
      }
    }

    function MasterComponent_content_152_div_14_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " BOL/LiftTicket# is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_14_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r48.bolResolveForm.get("BolNumber").errors.required);
      }
    }

    function MasterComponent_content_152_div_22_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Lift Date is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_22_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r49.bolResolveForm.get("LiftDate").errors.required);
      }
    }

    function MasterComponent_content_152_div_30_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Gross quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_30_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r50.bolResolveForm.get("GrossQuantity").errors.required);
      }
    }

    function MasterComponent_content_152_div_38_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Net quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_38_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r51.bolResolveForm.get("NetQuantity").errors.required);
      }
    }

    function MasterComponent_content_152_div_40_div_5_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_40_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_40_div_5_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r58.bolResolveForm.get("SelectedTerminal").errors.required);
      }
    }

    function MasterComponent_content_152_div_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 168);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label", 158);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Terminal Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 169);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function MasterComponent_content_152_div_40_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r61);

          var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r60.SelectedTerminalList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](5, MasterComponent_content_152_div_40_div_5_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Terminal")("settings", ctx_r52.multiselectSettingsById)("data", ctx_r52.TerminalList)("ngModel", ctx_r52.SelectedTerminalList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r52.bolResolveForm.get("SelectedTerminal").invalid && (ctx_r52.bolResolveForm.get("SelectedTerminal").dirty || ctx_r52.bolResolveForm.get("SelectedTerminal").touched));
      }
    }

    function MasterComponent_content_152_div_46_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Fuel Type is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_152_div_46_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_152_div_46_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r53.bolResolveForm.get("SelectedFuelType").errors.required);
      }
    }

    function MasterComponent_content_152_Template(rf, ctx) {
      if (rf & 1) {
        var _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "content", 136);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "form", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngSubmit", function MasterComponent_content_152_Template_form_ngSubmit_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r63.onSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 138);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 140);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "BOL/LiftTicket#");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](11, "input", 143);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](12, "input", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](13, "input", 145);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](14, MasterComponent_content_152_div_14_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 147);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 148);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Lift Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "input", 149);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function MasterComponent_content_152_Template_input_onDateChange_21_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r65.bolResolveForm.get("LiftDate").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, MasterComponent_content_152_div_22_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 150);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Gross Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](29, "input", 152);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](30, MasterComponent_content_152_div_30_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 153);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "label", 154);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "Net Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](37, "input", 155);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, MasterComponent_content_152_div_38_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, MasterComponent_content_152_div_40_Template, 6, 5, "div", 156);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 157);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "label", 158);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Fuel");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "ng-multiselect-dropdown", 159);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function MasterComponent_content_152_Template_ng_multiselect_dropdown_ngModelChange_45_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r66.SelectedFuelTypeList = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](46, MasterComponent_content_152_div_46_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "label", 161);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](52, "Badge#");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](53, "input", 162);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "div", 160);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "label", 163);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](57, "Notes");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](58, "textarea", 164);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "div", 165);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "button", 166);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_content_152_Template_button_click_60_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r67._toggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](61, "Cancel");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "button", 167);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](63, "Save &Re-Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx_r9.bolResolveForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.bolResolveForm.get("BolNumber").invalid && (ctx_r9.bolResolveForm.get("BolNumber").dirty || ctx_r9.bolResolveForm.get("BolNumber").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.bolResolveForm.get("LiftDate").invalid && (ctx_r9.bolResolveForm.get("LiftDate").dirty || ctx_r9.bolResolveForm.get("LiftDate").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.bolResolveForm.get("GrossQuantity").invalid && (ctx_r9.bolResolveForm.get("GrossQuantity").dirty || ctx_r9.bolResolveForm.get("GrossQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.bolResolveForm.get("NetQuantity").invalid && (ctx_r9.bolResolveForm.get("NetQuantity").dirty || ctx_r9.bolResolveForm.get("NetQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r9.bolResolveForm.get("IsBulkPlantLift").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Fuel")("settings", ctx_r9.multiselectSettingsById)("data", ctx_r9.FuelTypeList)("ngModel", ctx_r9.SelectedFuelTypeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r9.bolResolveForm.get("SelectedFuelType").invalid && (ctx_r9.bolResolveForm.get("SelectedFuelType").dirty || ctx_r9.bolResolveForm.get("SelectedFuelType").touched));
      }
    }

    function MasterComponent_tbody_202_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var record_r69 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.CallId, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](record_r69.bol);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.TerminalName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.Terminals, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.FileName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.TerminalItemCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.LoadDate, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.RecordDate, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.CarrierID, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.CarrierName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.recordStatus, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.Reason, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.ReasonCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.ReasonCategory, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.Username, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", record_r69.ModifiedDate, " ");
      }
    }

    function MasterComponent_tbody_202_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_tbody_202_tr_1_Template, 33, 16, "tr", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r10.LiftFilesearchResults);
      }
    }

    function MasterComponent_content_210_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_11_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " BOL# is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_11_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r72.LFVRecordEditForm.get("BolNumber").errors.required);
      }
    }

    function MasterComponent_content_210_span_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_18_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Terminal Code is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_18_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r74.LFVRecordEditForm.get("TerminalCode").errors.required);
      }
    }

    function MasterComponent_content_210_span_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_25_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Terminal Item Code is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_25_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r76.LFVRecordEditForm.get("TerminalItemCode").errors.required);
      }
    }

    function MasterComponent_content_210_span_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_32_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " CIN is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_32_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r78.LFVRecordEditForm.get("CIN").errors.required);
      }
    }

    function MasterComponent_content_210_span_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_40_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Carrier Name is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_40_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_40_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r80 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r80.LFVRecordEditForm.get("CarrierName").errors.required);
      }
    }

    function MasterComponent_content_210_span_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_47_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Load Date is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_47_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r82.LFVRecordEditForm.get("LoadDate").errors.required);
      }
    }

    function MasterComponent_content_210_span_52_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_54_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Corrected quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_54_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_54_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_54_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, MasterComponent_content_210_div_54_div_2_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r84.LFVRecordEditForm.get("CorrectedQuantity").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r84.LFVRecordEditForm.get("CorrectedQuantity").errors.pattern);
      }
    }

    function MasterComponent_content_210_span_59_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_61_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_61_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Gross Quantity is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function MasterComponent_content_210_div_61_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 142);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, MasterComponent_content_210_div_61_div_1_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, MasterComponent_content_210_div_61_div_2_Template, 2, 0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r86.LFVRecordEditForm.get("GrossQuantity").errors.pattern);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r86.LFVRecordEditForm.get("GrossQuantity").errors.required);
      }
    }

    function MasterComponent_content_210_Template(rf, ctx) {
      if (rf & 1) {
        var _r98 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "content", 136);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "form", 137);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngSubmit", function MasterComponent_content_210_Template_form_ngSubmit_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r98);

          var ctx_r97 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r97.onRecordEditSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 170);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "label", 141);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "BOL#");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, MasterComponent_content_210_span_8_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](9, "input", 144);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "input", 172);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, MasterComponent_content_210_div_11_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "label", 173);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Terminal Code");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, MasterComponent_content_210_span_16_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](17, "input", 174);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, MasterComponent_content_210_div_18_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "label", 175);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Terminal Item Code");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, MasterComponent_content_210_span_23_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](24, "input", 176);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](25, MasterComponent_content_210_div_25_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "label", 177);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](29, "CIN");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](30, MasterComponent_content_210_span_30_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](31, "input", 178);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](32, MasterComponent_content_210_div_32_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "label", 179);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37, "Carrier Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, MasterComponent_content_210_span_38_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](39, "input", 180);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, MasterComponent_content_210_div_40_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "label", 181);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Load Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, MasterComponent_content_210_span_45_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "input", 182);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function MasterComponent_content_210_Template_input_onDateChange_46_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r98);

          var ctx_r99 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r99.LFVRecordEditForm.get("LoadDate").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](47, MasterComponent_content_210_div_47_Template, 2, 1, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "label", 183);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](51, "Corrected Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](52, MasterComponent_content_210_span_52_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](53, "input", 184);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](54, MasterComponent_content_210_div_54_Template, 3, 2, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "label", 151);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58, "Gross Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](59, MasterComponent_content_210_span_59_Template, 2, 0, "span", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](60, "input", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](61, MasterComponent_content_210_div_61_Template, 3, 2, "div", 146);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 139);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "label", 186);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](66, "Product Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](67, "input", 187);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](70, "label", 188);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](71, "Record Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](72, "input", 189);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "div", 171);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](74, "div", 132);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](75, "label", 190);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](76, "CarrierID");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](77, "input", 191);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](78, "div", 165);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](79, "button", 166);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_content_210_Template_button_click_79_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r98);

          var ctx_r100 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r100._EditToggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](80, "Cancel");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "button", 167);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](82, "Save &Re-Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx_r11.LFVRecordEditForm);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsBolReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsBolReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("BolNumber").invalid && (ctx_r11.LFVRecordEditForm.get("BolNumber").dirty || ctx_r11.LFVRecordEditForm.get("BolNumber").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsTerminalCodeReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsTerminalCodeReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("TerminalCode").invalid && (ctx_r11.LFVRecordEditForm.get("TerminalCode").dirty || ctx_r11.LFVRecordEditForm.get("TerminalCode").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsTermItemCodeReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsTermItemCodeReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("TerminalItemCode").invalid && (ctx_r11.LFVRecordEditForm.get("TerminalItemCode").dirty || ctx_r11.LFVRecordEditForm.get("TerminalItemCode").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsCINReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsCINReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("CIN").invalid && (ctx_r11.LFVRecordEditForm.get("CIN").dirty || ctx_r11.LFVRecordEditForm.get("CIN").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsCarrierNameReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsCarrierNameReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("CarrierName").invalid && (ctx_r11.LFVRecordEditForm.get("CarrierName").dirty || ctx_r11.LFVRecordEditForm.get("CarrierName").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsLoadDateReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("readonly", !ctx_r11.LfvValidationParameters.IsLoadDateReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("LoadDate").invalid && (ctx_r11.LFVRecordEditForm.get("LoadDate").dirty || ctx_r11.LFVRecordEditForm.get("LoadDate").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsCorrectedQtyRes || ctx_r11.LfvValidationParameters.IsGrossReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsCorrectedQtyOrGrossReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").invalid && (ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").dirty || ctx_r11.LFVRecordEditForm.get("CorrectedQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LfvValidationParameters.IsGrossReq || ctx_r11.LfvValidationParameters.IsCorrectedQtyRes);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", !ctx_r11.LfvValidationParameters.IsCorrectedQtyOrGrossReq);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r11.LFVRecordEditForm.get("GrossQuantity").invalid && (ctx_r11.LFVRecordEditForm.get("GrossQuantity").dirty || ctx_r11.LFVRecordEditForm.get("GrossQuantity").touched));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", true);
      }
    }

    function MasterComponent_div_211_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 192);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 193);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 194);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 195);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var MasterComponent = /*#__PURE__*/function () {
      function MasterComponent(_lfvService, fb) {
        _classCallCheck(this, MasterComponent);

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
        this.LFVRecordStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["LFVRecordStatus"]; //side bar related variables

        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedTerminalList = [];
        this.SelectedFuelTypeList = []; //search Liftfile records variables

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
        this._EditPOSITIONS = ['left', 'right', 'top', 'bottom']; //ignore by reason

        this.preferenceSetting = null;
        this.selectedReason = [];
        this.reasonList = [];
        this.dropdownSettings = {
          singleSelection: true,
          idField: 'Id',
          textField: 'Name',
          allowSearchFilter: true
        };
      }

      _createClass(MasterComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
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
      }, {
        key: "buildForm",
        value: function buildForm() {
          var fg = this.fb.group({
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
      }, {
        key: "initFormData",
        value: function initFormData(data) {
          this.bolResolveForm.reset(); //clear previous values

          if (this.bolResolveForm != null && this.bolResolveForm != undefined && data != null && data != undefined) {
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

            if (data.IsBulkPlantLift == true) {
              // no terminal dropdown for pickup from bulk plants
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
      }, {
        key: "createPostObject",
        value: function createPostObject() {
          var inputPostObject = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFBolEditModel"]();
          inputPostObject.BadgeNumber = this.bolResolveForm.get('BadgeNumber').value;
          inputPostObject.BolNumber = this.bolResolveForm.get('BolNumber').value;
          inputPostObject.GrossQuantity = this.bolResolveForm.get('GrossQuantity').value;
          inputPostObject.InvoiceFtlDetailId = this.bolResolveForm.get('InvoiceFtlDetailId').value;
          inputPostObject.IsBulkPlantLift = this.bolResolveForm.get('IsBulkPlantLift').value;
          inputPostObject.LiftRecord.LiftFileRecordId = this.bolResolveForm.get('LIftFileRecordId').value;
          inputPostObject.LiftDate = this.bolResolveForm.get('LiftDate').value;
          inputPostObject.NetQuantity = this.bolResolveForm.get('NetQuantity').value;
          inputPostObject.Notes = this.bolResolveForm.get('Notes').value;
          var SelectedFuelType = this.bolResolveForm.get('SelectedFuelType').value;
          var fuelTypeId = SelectedFuelType[0].Id;
          inputPostObject.FuelTypeId = fuelTypeId;
          var selectedTerminal = this.bolResolveForm.get('SelectedTerminal').value;
          var terminalId = inputPostObject.IsBulkPlantLift ? selectedTerminal.Id : selectedTerminal[0].Id;
          inputPostObject.TerminalId = terminalId;
          return inputPostObject;
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
              title: 'LiftFileRecords',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'LiftFileRecords',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            fixedHeader: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getLfvData();
        }
      }, {
        key: "changeViewType",
        value: function changeViewType(value) {
          this.viewType = value;
          if (value == 1) this.isValidationCarrier = false;else this.isValidationCarrier = true;
          this.getLfvData();
        }
      }, {
        key: "getLfvData",
        value: function getLfvData() {
          var _this14 = this;

          this.LFValidationList = [];
          var ids = [];
          var carrierOrderIds = "";

          if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
          } else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(function (res) {
              ids.push(res.Name);
            });
            carrierOrderIds = ids.join();
          }

          this.IsLoading = true;

          this._lfvService.getLFValidationGrid({
            fromDate: this.filterComponent.fromDate,
            toDate: this.filterComponent.toDate,
            isCarrierPerFormanceDashboard: this.isValidationCarrier,
            carrierIds: carrierOrderIds,
            isMatchingWindow: this.filterComponent.isMatchingWindow
          }).subscribe(function (res) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this14, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
              return regeneratorRuntime.wrap(function _callee5$(_context5) {
                while (1) {
                  switch (_context5.prev = _context5.next) {
                    case 0:
                      if (res) this.LFValidationList = res;else this.LFValidationList = [];
                      this.IsLoading = false;

                    case 2:
                    case "end":
                      return _context5.stop();
                  }
                }
              }, _callee5, this);
            }));
          });
        }
      }, {
        key: "OnSearch",
        value: function OnSearch($event) {
          if ($event) {
            try {
              $("#liftfilerecords-datatable").DataTable().clear().destroy();
              this.gridType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_9__["LFVRecordStatus"].None;
              this.LFVRecordGrid = []; // this.dtTrigger.next();

              this.getLfvData();
            } catch (e) {}
          }
        }
      }, {
        key: "changeGridType",
        value: function changeGridType(status) {
          this.gridType = status;
          this.getLfvFilterGrid(status);
        }
      }, {
        key: "getLfvFilterGrid",
        value: function getLfvFilterGrid(status) {
          var _this15 = this;

          // if ((this.datatableElement && this.datatableElement.dtInstance)) {
          //   this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
          // }
          try {
            $("#liftfilerecords-datatable").DataTable().clear().destroy();
          } catch (e) {}

          this.IsLoading = true;
          this.LFVRecordGrid = [];
          var ids = [];
          var carrierOrderIds = "";

          if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
          } else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(function (res) {
              ids.push(res.Name);
            });
            carrierOrderIds = ids.join();
          }

          this._lfvService.getLFVRecordGrid({
            fromDate: this.filterComponent.fromDate,
            toDate: this.filterComponent.toDate,
            recordStatus: status,
            isMatchingWindow: this.filterComponent.isMatchingWindow,
            carrierIds: carrierOrderIds
          }).subscribe(function (res) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this15, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
              return regeneratorRuntime.wrap(function _callee6$(_context6) {
                while (1) {
                  switch (_context6.prev = _context6.next) {
                    case 0:
                      if (!res) {
                        _context6.next = 7;
                        break;
                      }

                      _context6.next = 3;
                      return res;

                    case 3:
                      this.LFVRecordGrid = _context6.sent;

                      if (this.LFVRecordGrid != null && this.LFVRecordGrid.length > 0) {
                        this.isAdminUser = this.LFVRecordGrid[0].IsAdminUser;
                        this.LfvValidationParameters = this.LFVRecordGrid[0].LfvValidationParameters;
                      }

                      _context6.next = 8;
                      break;

                    case 7:
                      this.LFVRecordGrid = [];

                    case 8:
                      this.dtTrigger.next();
                      this.IsLoading = false;

                    case 10:
                    case "end":
                      return _context6.stop();
                  }
                }
              }, _callee6, this);
            }));
          });
        }
      }, {
        key: "openLFVScratchReportGrid",
        value: function openLFVScratchReportGrid() {
          window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
        }
      }, {
        key: "openAccrualReportGrid",
        value: function openAccrualReportGrid() {
          window.open("Supplier/LiftFile/LFVAccrualReport", "_blank");
        }
      }, {
        key: "selectAllRecords",
        value: function selectAllRecords(eventData) {
          if (eventData != null && eventData != undefined) {
            if (eventData.target.checked) {
              this.isChecked = true;
            } else {
              this.isChecked = false;
            }
          }
        }
      }, {
        key: "getLFRecordIds",
        value: function getLFRecordIds() {
          var LFRecordIds = [];
          var table = $('#liftfilerecords-datatable').DataTable();
          var rowcollection = table.$(".dt-checkbox", {
            "page": "all"
          });
          rowcollection.each(function (index, elem) {
            if ($(this).is(":checked")) {
              LFRecordIds.push(parseInt($(this).attr('id')));
            }
          });
          return LFRecordIds;
        }
      }, {
        key: "getPreferencesSetting",
        value: function getPreferencesSetting() {
          var _this16 = this;

          if (!this.preferenceSetting) {
            this._lfvService.getPreferencesSetting().subscribe(function (response) {
              _this16.preferenceSetting = response;
            });
          }
        }
      }, {
        key: "GetReasonDescriptionList",
        value: function GetReasonDescriptionList() {
          var _this17 = this;

          if (this.reasonList && this.reasonList.length == 0) {
            this.IsLoading = true;

            this._lfvService.GetReasonDescriptionList().subscribe(function (response) {
              if (response && response.length > 0) {
                _this17.reasonList = response;
              }

              _this17.IsLoading = false;
            });
          }
        }
      }, {
        key: "submitIgnoreDescription",
        value: function submitIgnoreDescription() {
          this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
        }
      }, {
        key: "ValidateForIgnoreMatchProcessing",
        value: function ValidateForIgnoreMatchProcessing(status) {
          var LFRecordIds = this.getLFRecordIds();
          this.selectedReason = [];

          if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {
            if (status == 'ignore') {
              if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
                this.GetReasonDescriptionList();
                document.getElementById('openIgnoreModal').click();
              } else {
                this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
              }
            } else if (status == 'reprocess') this.addUnmatchedRecordForReProcessing(LFRecordIds);
          } else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("No Records selected", undefined, undefined);
          }
        }
      }, {
        key: "addRecordsForForcedIgnoreMatchProcessing",
        value: function addRecordsForForcedIgnoreMatchProcessing(LFRecordIds) {
          var _this18 = this;

          var descriptionId = 0;
          var descriptionText = '';

          if (this.selectedReason && this.selectedReason.length > 0) {
            descriptionId = this.selectedReason[0].Id;
            descriptionText = this.selectedReason[0].Name;
          }

          this.IsLoading = true;

          this._lfvService.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe(function (response) {
            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this18.getLfvFilterGrid(_this18.gridType);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
          });

          this.IsLoading = true;
        }
      }, {
        key: "addUnmatchedRecordForReProcessing",
        value: function addUnmatchedRecordForReProcessing(LFRecordIds) {
          var _this19 = this;

          this.IsLoading = true;

          this._lfvService.addUnmatchedRecordForReProcessing(LFRecordIds).subscribe(function (response) {
            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this19.getLfvFilterGrid(_this19.gridType);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
          });

          this.IsLoading = true;
        }
      }, {
        key: "getBolDetailsForResolve",
        value: function getBolDetailsForResolve(lfRecord) {
          var _this20 = this;

          this.IsLoading = true;
          lfRecord.IsFromScratchReport = true;

          this._lfvService.getBolDetailsForResolve(lfRecord).subscribe(function (data) {
            if (data) {
              _this20._toggleOpened(true);

              _this20.selectedLiftFileRecord = data.LiftRecord;
              _this20.TerminalList = data.TerminalList;
              _this20.FuelTypeList = data.FuelTypeList;
              _this20.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;

              _this20.initFormData(data);

              _this20.IsLoading = false;
            }
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var _this21 = this;

          this.IsLoading = true;
          this.bolResolveForm.markAsTouched();

          if (this.bolResolveForm.valid) {
            var requestObj = this.createPostObject();

            if (requestObj != null) {
              this._lfvService.saveBolDetailsForResolve(requestObj).subscribe(function (response) {
                if (response.StatusCode == 0) {
                  _this21.IsLoading = false;
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

                  _this21._toggleOpened(false);

                  _this21.getLfvFilterGrid(_this21.gridType);
                } else {
                  _this21.IsLoading = false;
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                }
              });
            }
          }

          this.IsLoading = false;
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
            this.bolResolveForm.reset();
          }
        }
      }, {
        key: "GetBolRecord",
        value: function GetBolRecord(InvoiceFtlDetailId) {
          if (InvoiceFtlDetailId != null && InvoiceFtlDetailId != undefined && InvoiceFtlDetailId != '') {
            var selectedLiftFileRecordId = this.selectedLiftFileRecord.LiftFileRecordId;
            var invoiceFtlDetailId = parseInt(InvoiceFtlDetailId);
            var LFRecord = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFRecordGridModel"]();
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
      }, {
        key: "openSupplierBOLReportGrid",
        value: function openSupplierBOLReportGrid() {
          window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
        }
      }, {
        key: "openCarrierBOLReportGrid",
        value: function openCarrierBOLReportGrid() {
          window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
        }
      }, {
        key: "searchLiftFileRecords",
        value: function searchLiftFileRecords() {
          var _this22 = this;

          var bolQuery = this.bolSearchQuery;
          var fileNameQuery = this.fileNameSearchQuery;

          if ((bolQuery == "" || bolQuery == null || bolQuery == undefined) && (fileNameQuery == "" || fileNameQuery == undefined || fileNameQuery == null)) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror("Please provide either Bol# or Filename", undefined, undefined);
          } else {
            var exportColumns = {
              columns: ':visible'
            };
            this.searchGridDtOptions = {
              dom: '<"html5buttons"B>lTfgitp',
              buttons: [{
                extend: 'colvis'
              }, {
                extend: 'copy',
                exportOptions: exportColumns
              }, {
                extend: 'csv',
                title: 'Lift File Records',
                exportOptions: exportColumns
              }, {
                extend: 'pdf',
                title: 'Lift File Records',
                orientation: 'landscape',
                exportOptions: exportColumns
              }, {
                extend: 'print',
                exportOptions: exportColumns
              }],
              pagingType: 'first_last_numbers',
              fixedHeader: false,
              pageLength: 10,
              lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
            };
            var bolNumber = bolQuery == null || bolQuery == undefined ? "" : bolQuery.trim();
            var fileName = fileNameQuery == null || fileNameQuery == undefined ? "" : fileNameQuery.trim();
            this.IsLoading = true;

            this._lfvService.getLFSearchRecords(bolNumber, fileName).subscribe(function (data) {
              var el = _this22.btnOpenModal.nativeElement;
              el.click();
              $("#liftfileSearchrecords-datatable").DataTable().clear().destroy();
              _this22.LiftFilesearchResults = data;

              _this22.searchGridDtTrigger.next();

              _this22.IsLoading = false;
            });
          }
        }
      }, {
        key: "toggleRecordSearchControls",
        value: function toggleRecordSearchControls(shouldShow) {
          this.bolSearchQuery = "";
          this.fileNameSearchQuery = "";
          this.showSearchControls = shouldShow;
          this.showSearchBtn = shouldShow;
        }
      }, {
        key: "OnExport",
        value: function OnExport(status) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee7() {
            return regeneratorRuntime.wrap(function _callee7$(_context7) {
              while (1) {
                switch (_context7.prev = _context7.next) {
                  case 0:
                    this.generateCSV(status);

                  case 1:
                  case "end":
                    return _context7.stop();
                }
              }
            }, _callee7, this);
          }));
        }
      }, {
        key: "generateCSV",
        value: function generateCSV(status) {
          var _this23 = this;

          this.IsLoading = true;
          var exportData = [];
          var ids = [];
          var carrierOrderIds = "";

          if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
            carrierOrderIds = "";
          } else {
            this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(function (res) {
              ids.push(res.Name);
            });
            carrierOrderIds = ids.join();
          }

          this._lfvService.getLFVRecordGrid({
            fromDate: this.filterComponent.fromDate,
            toDate: this.filterComponent.toDate,
            recordStatus: status,
            isMatchingWindow: this.filterComponent.isMatchingWindow,
            carrierIds: carrierOrderIds
          }).subscribe(function (res) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this23, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee8() {
              return regeneratorRuntime.wrap(function _callee8$(_context8) {
                while (1) {
                  switch (_context8.prev = _context8.next) {
                    case 0:
                      if (res) exportData = res.map(function (m) {
                        return {
                          bol: m.bol,
                          TerminalName: m.TerminalName,
                          Terminal: m.Terminals,
                          correctedQuantity: m.correctedQuantity,
                          TerminalItemCode: m.TerminalItemCode,
                          ProductType: m.ProductType,
                          LoadDate: m.LoadDate,
                          RecordDate: m.RecordDate,
                          CarrierID: m.CarrierID,
                          CarrierName: m.CarrierName,
                          Reason: m.Reason,
                          ReasonCode: m.ReasonCode,
                          ReasonCategory: m.ReasonCategory,
                          Username: m.Username,
                          ModifiedDate: m.ModifiedDate,
                          LFVResolutionTime: m.LFVResolutionTime,
                          TimeToBol: m.TimeToBol
                        };
                      });else exportData = [];
                      new angular7_csv_dist_Angular_csv__WEBPACK_IMPORTED_MODULE_8__["AngularCsv"](exportData, 'LFV_' + new Date(), this.csvOptions);
                      this.IsLoading = false;

                    case 3:
                    case "end":
                      return _context8.stop();
                  }
                }
              }, _callee8, this);
            }));
          });
        }
      }, {
        key: "_EditToggleOpened",
        value: function _EditToggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._EditOpened = true;
          } else {
            this._EditOpened = !this._EditOpened;
            this.LFVRecordEditForm.reset();
          }
        }
      }, {
        key: "editLiftFileRecord",
        value: function editLiftFileRecord(record) {
          if (record != null) {
            this.IsLoading = true;

            this._EditToggleOpened(true);

            this.initRecordEditForm(record);
            this.IsLoading = false;
          }
        }
      }, {
        key: "buildLFVRecordEditForm",
        value: function buildLFVRecordEditForm() {
          var formGroup = this.fb.group({
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
      }, {
        key: "initRecordEditForm",
        value: function initRecordEditForm(record) {
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
              } //if (this.LfvValidationParameters.IsGrossReq) {
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
      }, {
        key: "onRecordEditSubmit",
        value: function onRecordEditSubmit() {
          var _this24 = this;

          this.LFVRecordEditForm.markAllAsTouched();

          if (this.LFVRecordEditForm.valid) {
            this.IsLoading = true;
            var values = this.LFVRecordEditForm.value;

            if (values != null) {
              var data = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_3__["LFRecordGridModel"]();
              data.Terminal = this.LFVRecordEditForm.get('TerminalCode').value;
              data.bol = this.LFVRecordEditForm.get('BolNumber').value;
              data.correctedQuantity = this.LFVRecordEditForm.get('CorrectedQuantity').value;
              data.CarrierName = this.LFVRecordEditForm.get('CarrierName').value;
              data.CIN = this.LFVRecordEditForm.get('CIN').value;
              data.TerminalItemCode = this.LFVRecordEditForm.get('TerminalItemCode').value;
              data.GrossQuantity = this.LFVRecordEditForm.get('GrossQuantity').value;
              data.LoadDate = this.LFVRecordEditForm.get('LoadDate').value;
              data.LiftFileRecordId = this.LFVRecordEditForm.get('LIftFileRecordId').value;
              var requestModel = this.correctValues(data);

              this._lfvService.updateLiftFileRecord(requestModel).subscribe(function (response) {
                _this24.IsLoading = false;

                if (response.StatusCode == 0) {
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
                } else if (response.StatusCode == 1) {
                  src_app_declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                }

                _this24._EditToggleOpened(false);

                _this24.getLfvFilterGrid(_this24.gridType);
              });
            }
          }
        }
      }, {
        key: "correctValues",
        value: function correctValues(data) {
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
          } else {
            var loadDateWithOutSlash = data.LoadDate.replace(/\//g, '');
            data.LoadDate = loadDateWithOutSlash;
          }

          return data;
        }
      }]);

      return MasterComponent;
    }();

    MasterComponent.ɵfac = function MasterComponent_Factory(t) {
      return new (t || MasterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__["LiftfiledashboardserviceService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormBuilder"]));
    };

    MasterComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: MasterComponent,
      selectors: [["app-master"]],
      viewQuery: function MasterComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.filterComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.btnOpenModal = _t.first);
        }
      },
      decls: 229,
      vars: 88,
      consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-sm-12"], [1, "col-sm-8", "pr-5", "text-right", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "mb10"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "col-sm-4"], ["class", "float-right", 4, "ngIf"], [1, "col-md-12"], [3, "search", "export"], [3, "LFValidationList", 4, "ngIf"], [1, "col-sm-12", "text-center", "sticky-header-dash"], [1, "row", 3, "ngClass"], [1, "col-12"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "schedule-loading-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border", "location_table"], [1, "table-responsive"], ["id", "liftfilerecords-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "BOL"], ["data-key", "Terminal"], ["data-key", "Terminals"], ["data-key", "Corrected_Quantity"], ["data-key", "Terminal_Item_Code"], ["data-key", "Product_Type"], ["data-key", "Load_Date"], ["data-key", "Record_Date"], ["data-key", "Carrier_ID"], ["data-key", "Carrier_Name"], ["data-key", "Reasons"], ["data-key", "Reason_Code", 3, "ngClass"], ["data-key", "Reason_Category", 3, "ngClass"], ["data-key", "User_Name"], ["data-key", "Modified_Date"], ["data-key", "TimeToBol", 3, "ngClass"], ["data-key", "SelectAll", 3, "ngClass"], ["type", "checkbox", "id", "select-all-records", "value", "select-all-records", 3, "click"], ["data-key", "Action", 3, "ngClass"], ["data-key", "Edit", 3, "ngClass"], [4, "ngFor", "ngForOf"], ["class", "col-sm-12 text-right mb25 btn-wrapper", 4, "ngIf"], [1, "Lfv-resolve-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], [1, "scroll-auto"], [1, "well", "bg-white", "shadow-b", "lfrecord-section"], [1, "ibox-content", "no-border", "no-padding"], ["id", "LFrecord", 1, "table-responsive"], ["id", "table-Lfrecord", 1, "table", "table-striped", "table-bordered", "table-hover", "lfvrecord"], [1, "thead-light"], [4, "ngIf"], ["class", "pr30", 4, "ngIf"], ["id", "searchoutputdetailsgrid-modal", "role", "dialog", "tabindex", "-1", 1, "modal", "fade"], [1, "modal-dialog", "modal-xl", "modal-dialog-scrollable", "modal-dialog-centered"], [1, "modal-content"], [1, "modal-header"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "searchoutputdetailsgrid"], ["aria-hidden", "true"], [1, "modal-body"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox-content", "no-padding", "no-border"], ["id", "liftfileSearchrecords-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "BolNumber"], ["data-key", "FileName"], ["data-key", "TerminalITemCode"], ["data-key", "LoadDate"], ["data-key", "RecordDate"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "Status"], ["data-key", "Reason"], ["data-key", "Reason_Code"], ["data-key", "Reason_Category"], ["data-key", "UserName"], [1, "Lfv-edit-sidebar", 2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], ["class", "loader", 4, "ngIf"], ["type", "hidden", "id", "openIgnoreModal", "data-toggle", "modal", "data-target", "#ignoreModal"], ["id", "ignoreModal", "tabindex", "-1", "role", "dialog", "aria-hidden", "true", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-dialog-centered"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-secondary"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "disabled", "click"], [1, "float-right"], ["placement", "bottom", "ngbTooltip", "Scratch Report", 1, "btn", "border", "border-secondary", "float-right", "pb-1", 3, "click"], [1, "fab", "fa-firstdraft", "fs16", "mt2"], ["placement", "bottom", "ngbTooltip", "Accrual Report", 1, "btn", "border", "border-secondary", "float-right", "pb-1", 3, "click"], [1, "fas", "fa-file-invoice", "fs16", "mt2"], [1, "btn", "btn-default", "mt0", 3, "click"], ["btnSearchRecords", ""], [1, "fas", "fa-search", "mr5"], ["class", "record-search-controls bg-white pa shadow border z-index10", 4, "ngIf"], [1, "record-search-controls", "bg-white", "pa", "shadow", "border", "z-index10"], [1, "col-auto", "text-right"], ["btnCancelSearch", ""], ["title", "Cancel", 1, "fa", "fa-close", "fs18", "my-2"], [1, "col-auto", "form-group"], ["type", "text", "placeholder", "Search BOL#", 1, "form-control", 3, "ngModel", "ngModelChange"], ["BOLSearchInput", ""], ["type", "text", "placeholder", "Search FileName", 1, "form-control", 3, "ngModel", "ngModelChange"], ["FileNameSearchInput", ""], [1, "col-auto", "form-group", "text-right"], [1, "btn", "btn-primary", "btn-sm", 3, "click"], ["id", "openModal", "data-toggle", "modal", "data-target", "#searchoutputdetailsgrid-modal", 3, "hidden"], ["btnOpenModal", ""], ["placement", "bottom", "ngbTooltip", "Carrier BOL Report", 1, "btn", "border", "border-secondary", "float-right", 3, "click"], [1, "fas", "fa-user", "fs16"], ["placement", "bottom", "ngbTooltip", "Supplier BOL Report", 1, "btn", "border", "border-secondary", "float-right", 3, "click"], [1, "fas", "fa-truck", "fs16"], [3, "LFValidationList"], [3, "ngClass"], [1, "text-center", 3, "ngClass"], ["type", "checkbox", 1, "dt-checkbox", 3, "id", "checked", "value"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-edit", "fs16"], ["title", "Edit Lf Record", 1, "fas", "fa-edit", "fs16"], [1, "col-sm-12", "text-right", "mb25", "btn-wrapper"], [1, "form-group", "col-sm-12"], ["type", "button", "class", "btn btn-primary", "value", "Ignore", "id", "btnForceIgnoreRecords", 3, "click", 4, "ngIf"], ["type", "button", "class", "btn btn-primary", "value", "ReProcess", "id", "btnForceReprocessRecords", 3, "click", 4, "ngIf"], ["type", "button", "value", "Ignore", "id", "btnForceIgnoreRecords", 1, "btn", "btn-primary", 3, "click"], ["type", "button", "value", "ReProcess", "id", "btnForceReprocessRecords", 1, "btn", "btn-primary", 3, "click"], ["class", "col-sm-6", 4, "ngIf"], [1, "col-sm-6"], [1, "form-group"], ["id", "select-bol", 1, "form-control", 3, "change"], [3, "value"], [3, "value", "selected"], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "col-sm-12", "section-bol-details-edit"], [1, "mt10", "row"], [1, "col-sm-3", "bol"], ["for", "BolNumber"], [1, "color-maroon"], ["formControlName", "InvoiceFtlDetailId", "type", "hidden", 1, "hide-element"], ["formControlName", "LIftFileRecordId", "type", "hidden", 1, "hide-element"], ["formControlName", "BolNumber", "required", "", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], [1, "col-sm-3", "lifdt"], ["for", "LiftDate"], ["name", "LiftDate", "formControlName", "LiftDate", "myDatePicker", "", 1, "form-control", 3, "format", "onDateChange"], [1, "col-sm-3", "grossQty"], ["for", "GrossQuantity"], ["formControlName", "GrossQuantity", 1, "form-control"], [1, "col-sm-3", "netQty"], ["for", "NetQuantity"], ["formControlName", "NetQuantity", 1, "form-control"], ["class", "col-sm-6 terminal-section", 4, "ngIf"], [1, "col-sm-6", "fuelType"], ["for", "Jobs"], ["formControlName", "SelectedFuelType", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-6"], ["for", "BadgeNumber"], ["formControlName", "BadgeNumber", 1, "form-control"], ["for", "Notes"], ["id", "Notes", "placeholder", "Notes", "formControlName", "Notes", 1, "form-control"], [1, "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], [1, "col-sm-6", "terminal-section"], ["formControlName", "SelectedTerminal", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], [1, "col-sm-12", "section-lfv-record-edit"], [1, "col-sm-3"], ["formControlName", "BolNumber", 1, "form-control", 3, "readonly"], ["for", "TerminalCode"], ["formControlName", "TerminalCode", 1, "form-control", 3, "readonly"], ["for", "TerminalItemCode"], ["formControlName", "TerminalItemCode", 1, "form-control", 3, "readonly"], ["for", "CIN"], ["formControlName", "CIN", 1, "form-control", 3, "readonly"], ["for", "CarrierName"], ["formControlName", "CarrierName", 1, "form-control", 3, "readonly"], ["for", "LoadDate"], ["name", "LoadDate", "formControlName", "LoadDate", "myDatePicker", "", 1, "form-control", 3, "format", "readonly", "onDateChange"], ["for", "CorrectedQuantity"], ["formControlName", "CorrectedQuantity", 1, "form-control", 3, "readonly"], ["formControlName", "GrossQuantity", 1, "form-control", 3, "readonly"], ["for", "ProductType"], ["name", "ProductType", "formControlName", "ProductType", 1, "form-control", 3, "readonly"], ["for", "RecordDate"], ["formControlName", "RecordDate", 1, "form-control", 3, "readonly"], ["for", "CarrierId"], ["formControlName", "CarrierId", 1, "form-control", 3, "readonly"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function MasterComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_8_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Validation Performance");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_11_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Carrier Performance");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](14, MasterComponent_div_14_Template, 3, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, MasterComponent_div_15_Template, 3, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, MasterComponent_div_16_Template, 6, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, MasterComponent_div_17_Template, 5, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "app-left-side-filter", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("search", function MasterComponent_Template_app_left_side_filter_search_20_listener($event) {
            return ctx.OnSearch($event);
          })("export", function MasterComponent_Template_app_left_side_filter_export_20_listener($event) {
            return ctx.OnExport($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, MasterComponent_app_validation_23_Template, 1, 1, "app-validation", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, MasterComponent_app_carrier_performace_24_Template, 1, 1, "app-carrier-performace", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](29, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_30_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.Clean);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "Match");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](32, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_33_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.NoMatch);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "No Match");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](35, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_36_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.PartialMatch);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](37, " Partial Match ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](38, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_39_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.PendingMatch);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Pending");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](41, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_42_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.Duplicate);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43, "Duplicate");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](44, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_45_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.ActiveExceptions);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, " Active Exception ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](47, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_48_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.IgnoreMatch);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](49, "Ignored");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](50, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_51_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.ForcedIgnore);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](52, "Forced Ignore");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](53, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "label", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_label_click_54_listener() {
            return ctx.changeGridType(ctx.LFVRecordStatus.UnMatched);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](55, "Unmatched");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](60, "span", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "table", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "th", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](68, "BOL");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](70, "Terminal Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](72, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](74, "Corrected Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](75, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](76, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](77, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](78, "Product Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](79, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](80, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "th", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](82, "Record Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](83, "th", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](84, "Carrier ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "th", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](86, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](87, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](88, "Reasons");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](89, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](90, " Reason Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](91, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](92, " Reason Category ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](93, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](94, "Modified By");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](95, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](96, "Modified Date (MST)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](97, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](98, "Resolution Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](99, "th", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](100, " Time To BOL ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](101, "th", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](102, " SelectAll ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](103, "input", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_input_click_103_listener($event) {
            return ctx.selectAllRecords($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](104, "th", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](105, " Action ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](106, "th", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](107, " Edit ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](108, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](109);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](110, MasterComponent_tr_110_Template, 43, 84, "tr", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](111, MasterComponent_div_111_Template, 5, 2, "div", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](112, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](113, "ng-sidebar", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("openedChange", function MasterComponent_Template_ng_sidebar_openedChange_113_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](114, "a", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_a_click_114_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](115, "i", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](116, "h3", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](117, "Edit BOL Details");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](118, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](119, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](120, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](121, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](122, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](123, "table", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](124, "thead", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](125, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](126, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](127, "BOL#");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](128, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](129, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](130, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](131, "Corrected Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](132, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](133, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](134, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](135, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](136, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](137, "ProductType");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](138, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](139, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](140);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](141, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](142);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](143, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](144);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](145, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](146);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](147, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](148);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](149, "td");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](150);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](151, MasterComponent_div_151_Template, 2, 1, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](152, MasterComponent_content_152_Template, 64, 12, "content", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](153, "div", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](154, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](155, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](156, "div", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](157, "button", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](158, "span", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](159, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](160, "div", 63);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](161, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](162, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](163, "div", 64);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](164, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](165, "div", 65);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](166, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](167, "table", 66);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](168, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](169, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](170, "th", 67);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](171, "CallID");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](172, "th", 68);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](173, "BOL#");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](174, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](175, "Terminal Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](176, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](177, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](178, "th", 69);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](179, "File Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](180, "th", 70);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](181, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](182, "th", 71);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](183, "Load Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](184, "th", 72);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](185, "Record Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](186, "th", 73);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](187, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](188, "th", 74);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](189, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](190, "th", 75);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](191, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](192, "th", 76);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](193, "Reason");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](194, "th", 77);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](195, "Reason Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](196, "th", 78);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](197, "Reason Category");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](198, "th", 79);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](199, "Modified By");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](200, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](201, " Modified Date (MST)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](202, MasterComponent_tbody_202_Template, 2, 1, "tbody", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](203, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](204, "ng-sidebar", 80);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("openedChange", function MasterComponent_Template_ng_sidebar_openedChange_204_listener($event) {
            return ctx._EditOpened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](205, "a", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_a_click_205_listener() {
            return ctx._EditToggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](206, "i", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](207, "h3", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](208, "Edit Lift File Record ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](209, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](210, MasterComponent_content_210_Template, 83, 29, "content", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](211, MasterComponent_div_211_Template, 5, 0, "div", 81);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](212, "div", 82);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](213, "div", 83);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](214, "div", 84);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](215, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](216, "div", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](217, "h4", 85);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](218, "Select Reason");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](219, "button", 86);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](220, "span", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](221, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](222, "div", 63);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](223, "ng-multiselect-dropdown", 87);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function MasterComponent_Template_ng_multiselect_dropdown_ngModelChange_223_listener($event) {
            return ctx.selectedReason = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](224, "div", 88);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](225, "button", 89);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](226, "Cancel");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](227, "button", 90);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function MasterComponent_Template_button_click_227_listener() {
            return ctx.submitIgnoreDescription();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](228, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "viewType")("value", 1)("checked", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "viewType")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.Clean)("checked", ctx.gridType == ctx.LFVRecordStatus.Clean);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.NoMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.NoMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.PartialMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.PartialMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.PendingMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.PendingMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.Duplicate)("checked", ctx.gridType == ctx.LFVRecordStatus.Duplicate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.ActiveExceptions)("checked", ctx.gridType == ctx.LFVRecordStatus.ActiveExceptions);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.IgnoreMatch)("checked", ctx.gridType == ctx.LFVRecordStatus.IgnoreMatch);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.ForcedIgnore)("checked", ctx.gridType == ctx.LFVRecordStatus.ForcedIgnore);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "gridType")("value", ctx.LFVRecordStatus.UnMatched)("checked", ctx.gridType == ctx.LFVRecordStatus.UnMatched);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](74, _c3, ctx.gridType == 0));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](76, _c3, ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Duplicate || ctx.gridType == ctx.LFVRecordStatus.PendingMatch));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](78, _c3, ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Duplicate || ctx.gridType == ctx.LFVRecordStatus.PendingMatch));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](80, _c3, ctx.gridType == ctx.LFVRecordStatus.PartialMatch || ctx.gridType == ctx.LFVRecordStatus.Clean ? false : true));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](82, _c3, !(ctx.gridType == ctx.LFVRecordStatus.NoMatch || ctx.gridType == ctx.LFVRecordStatus.UnMatched || ctx.gridType == ctx.LFVRecordStatus.Duplicate)));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](84, _c3, ctx.gridType != ctx.LFVRecordStatus.PartialMatch));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](86, _c3, ctx.gridType == ctx.LFVRecordStatus.IgnoreMatch || ctx.gridType == ctx.LFVRecordStatus.Clean || ctx.gridType == ctx.LFVRecordStatus.ForcedIgnore || !ctx.isAdminUser));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.LFVRecordGrid);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx.LFVRecordGrid == null ? null : ctx.LFVRecordGrid.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.bol);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalName);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.correctedQuantity);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.TerminalItemCode);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.LoadDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.selectedLiftFileRecord == null ? null : ctx.selectedLiftFileRecord.ProductType);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.bolResolveForm != undefined && ctx.bolResolveForm != null && ctx.bolResolveForm.get("InvoiceFtlDetailId").value > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.searchGridDtOptions)("dtTrigger", ctx.searchGridDtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx.LiftFilesearchResults == null ? null : ctx.LiftFilesearchResults.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx._EditOpened)("animate", ctx._EditAnimate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.LFVRecordEditForm != undefined && ctx.LFVRecordEditForm != null && ctx.LfvValidationParameters != undefined && ctx.LfvValidationParameters != null);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Reason")("settings", ctx.dropdownSettings)("data", ctx.reasonList)("ngModel", ctx.selectedReason);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.selectedReason && ctx.selectedReason.length == 0);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_11__["NgIf"], _left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_11__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_11__["NgForOf"], ng_sidebar__WEBPACK_IMPORTED_MODULE_12__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_12__["Sidebar"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_13__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgModel"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_14__["NgbTooltip"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["DefaultValueAccessor"], _validation_validation_component__WEBPACK_IMPORTED_MODULE_15__["ValidationComponent"], _carrier_carrier_component__WEBPACK_IMPORTED_MODULE_16__["CarrierComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["ɵangular_packages_forms_forms_x"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["RequiredValidator"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_17__["DatePicker"]],
      styles: ["aside {\r\n    width: 55% !important;\r\n}\r\n\r\n.scroll-auto{\r\n    height: calc(100vh - 125px);\r\n    overflow-x: hidden;\r\n    overflow-y:auto ;\r\n}\r\n\r\n.highlight-record {\r\n    background-color: #ffcccc\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbGZ2LWRhc2hib2FyZC9tYXN0ZXIvbWFzdGVyLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxxQkFBcUI7QUFDekI7O0FBRUE7SUFDSSwyQkFBMkI7SUFDM0Isa0JBQWtCO0lBQ2xCLGdCQUFnQjtBQUNwQjs7QUFDQTtJQUNJO0FBQ0oiLCJmaWxlIjoic3JjL2FwcC9sZnYtZGFzaGJvYXJkL21hc3Rlci9tYXN0ZXIuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbImFzaWRlIHtcclxuICAgIHdpZHRoOiA1NSUgIWltcG9ydGFudDtcclxufVxyXG5cclxuLnNjcm9sbC1hdXRve1xyXG4gICAgaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMTI1cHgpO1xyXG4gICAgb3ZlcmZsb3cteDogaGlkZGVuO1xyXG4gICAgb3ZlcmZsb3cteTphdXRvIDtcclxufVxyXG4uaGlnaGxpZ2h0LXJlY29yZCB7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZjY2NjXHJcbn1cclxuIl19 */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](MasterComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-master',
          templateUrl: './master.component.html',
          styleUrls: ['./master.component.css'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_10__["LiftfiledashboardserviceService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_7__["FormBuilder"]
        }];
      }, {
        filterComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_left_side_filter_left_side_filter_component__WEBPACK_IMPORTED_MODULE_2__["LeftSideFilterComponent"]]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTableDirective"]]
        }],
        btnOpenModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: ['btnOpenModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts": function srcAppLfvDashboardServiceLiftfiledashboardserviceServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LiftfiledashboardserviceService", function () {
      return LiftfiledashboardserviceService;
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


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var LiftfiledashboardserviceService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(LiftfiledashboardserviceService, _src_app_errors_Handl);

      var _super = _createSuper(LiftfiledashboardserviceService);

      function LiftfiledashboardserviceService(httpClient) {
        var _this25;

        _classCallCheck(this, LiftfiledashboardserviceService);

        _this25 = _super.call(this);
        _this25.httpClient = httpClient; //urls

        _this25.urlGetLFRecordsGrid = 'Supplier/LiftFile/GetLiftFileRecordsScratchReport';
        _this25.urlGetBolDetailsForResolve = 'Supplier/LiftFile/GetLFBolEditDetailsForSlider';
        _this25.urlSaveBolDetailsForResolve = 'Supplier/LiftFile/SaveLFBolEditDetails';
        _this25.urlAddRecordsForForceIgnoreProcessing = 'Supplier/LiftFile/AddRecordsAsIgnoreMatch';
        _this25.urlAddUnmatchedRecordForReProcessing = 'Supplier/Exception/AddUnmatchedRecordForReProcessing'; //SupplierBOLReport

        _this25.urlGetLiftFileRecordsWithMissingTFXDeliveryDetails = 'Supplier/LiftFile/GetLiftFileRecordsWithMissingTFXDeliveryDetails'; //carrier bol report

        _this25.urlGetTFXDeliveryDetailsWithMissingLiftFileRecords = 'Supplier/LiftFile/GetTFXDeliveryDetailsWithMissingLiftFileRecords';
        _this25.UrlGetLFValidation = '/Supplier/Exception/LFValidationGridWithFilter';
        _this25.UrlGetLFCarrier = '/Supplier/LiftFile/GetLFVCarrierDropDwn';
        _this25.UrlGetLFVRecordGrid = '/Supplier/Exception/LFRecordsGridForDashboard';
        _this25.urlGetLFSearchRecordsByBolFileName = 'Supplier/LiftFile/LFRecordsGridByBolFileName?bol=';
        _this25.UrlGetLFVAccrualReportGrid = 'Supplier/LiftFile/GetLFVAccrualReportGrid';
        _this25.UrlGetLFVValidationStatsAndProductTypesDDL = 'Supplier/LiftFile/GetLFVValidationStatsAndProductTypesDDL';
        _this25.UrlUpdateLiftFileRecord = 'Supplier/LiftFile/UpdateLiftFileRecord';
        _this25.urlGetReasonDescriptionList = 'Supplier/LiftFile/GetReasonDescriptionList';
        _this25.urlGetPreferencesSetting = 'Settings/Profile/GetPreferencesSettingAsync';
        return _this25;
      }

      _createClass(LiftfiledashboardserviceService, [{
        key: "getLFRecords",
        value: function getLFRecords() {
          return this.httpClient.get(this.urlGetLFRecordsGrid).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFRecords', null)));
        }
      }, {
        key: "getBolDetailsForResolve",
        value: function getBolDetailsForResolve(record) {
          return this.httpClient.post(this.urlGetBolDetailsForResolve, record).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBolDetailsForResolve', record)));
        }
      }, {
        key: "saveBolDetailsForResolve",
        value: function saveBolDetailsForResolve(record) {
          return this.httpClient.post(this.urlSaveBolDetailsForResolve, record).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('saveBolDetailsForResolve', record)));
        }
      }, {
        key: "addRecordsForForcedIgnoreMatchProcessing",
        value: function addRecordsForForcedIgnoreMatchProcessing(LfRecordIds) {
          var descriptionId = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 0;
          var descriptionText = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
          return this.httpClient.post(this.urlAddRecordsForForceIgnoreProcessing + '?DescriptionId=' + descriptionId + '&DescriptionText=' + descriptionText, LfRecordIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
        }
      }, {
        key: "getLFValidationGrid",
        value: function getLFValidationGrid(data) {
          return this.httpClient.post(this.UrlGetLFValidation, {
            startDate: data.fromDate,
            endDate: data.toDate,
            isCarrierPerFormanceDashboard: data.isCarrierPerFormanceDashboard,
            carrierIds: data.carrierIds,
            isMatchingWindow: data.isMatchingWindow
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFValidationGrid', null)));
        }
      }, {
        key: "getLFCarrier",
        value: function getLFCarrier(fromDate, toDate) {
          return this.httpClient.post(this.UrlGetLFCarrier, {
            fromDate: fromDate,
            toDate: toDate
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFCarrier', null)));
        }
      }, {
        key: "getSupplierBOLReport",
        value: function getSupplierBOLReport() {
          return this.httpClient.get(this.urlGetLiftFileRecordsWithMissingTFXDeliveryDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSupplierBOLReport', null)));
        }
      }, {
        key: "getCarrierBOLReport",
        value: function getCarrierBOLReport(fromDate, toDate) {
          return this.httpClient.post(this.urlGetTFXDeliveryDetailsWithMissingLiftFileRecords, {
            fromDate: fromDate,
            toDate: toDate
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierBOLReport', null)));
        }
      }, {
        key: "getLFVRecordGrid",
        value: function getLFVRecordGrid(data) {
          return this.httpClient.post(this.UrlGetLFVRecordGrid, {
            recordStatus: data.recordStatus,
            startDate: data.fromDate,
            endDate: data.toDate,
            lfCallId: 0,
            isMatchingWindow: data.isMatchingWindow,
            carrierIds: data.carrierIds
          }) // return this.httpClient.post(this.UrlGetLFVRecordGrid, {recordStatus:data.recordStatus, startDate: data.fromDate, endDate: data.toDate})
          .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFVRecordGrid', null)));
        }
      }, {
        key: "addUnmatchedRecordForReProcessing",
        value: function addUnmatchedRecordForReProcessing(LfRecordIds) {
          return this.httpClient.post(this.urlAddUnmatchedRecordForReProcessing, LfRecordIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
        }
      }, {
        key: "getLFSearchRecords",
        value: function getLFSearchRecords(bol, fileName) {
          return this.httpClient.get(this.urlGetLFSearchRecordsByBolFileName + bol + '&fileName=' + fileName).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFSearchRecords', null)));
        }
      }, {
        key: "getLFVAccrualReportGrid",
        value: function getLFVAccrualReportGrid(data) {
          return this.httpClient.post(this.UrlGetLFVAccrualReportGrid, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getLFVAccrualReportGrid', null)));
        }
      }, {
        key: "GetLFVValidationStatsAndProductTypesDDL",
        value: function GetLFVValidationStatsAndProductTypesDDL(data) {
          return this.httpClient.post(this.UrlGetLFVValidationStatsAndProductTypesDDL, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetLFVValidationStatsAndProductTypesDDL', null)));
        }
      }, {
        key: "updateLiftFileRecord",
        value: function updateLiftFileRecord(data) {
          return this.httpClient.post(this.UrlUpdateLiftFileRecord, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateLiftFileRecord', null)));
        }
      }, {
        key: "GetReasonDescriptionList",
        value: function GetReasonDescriptionList() {
          return this.httpClient.get(this.urlGetReasonDescriptionList).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetReasonDescriptionList', null)));
        }
      }, {
        key: "getPreferencesSetting",
        value: function getPreferencesSetting() {
          return this.httpClient.get(this.urlGetPreferencesSetting).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getPreferencesSettingAsync', null)));
        }
      }]);

      return LiftfiledashboardserviceService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    LiftfiledashboardserviceService.ɵfac = function LiftfiledashboardserviceService_Factory(t) {
      return new (t || LiftfiledashboardserviceService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    LiftfiledashboardserviceService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: LiftfiledashboardserviceService,
      factory: LiftfiledashboardserviceService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LiftfiledashboardserviceService, [{
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
  "./src/app/lfv-dashboard/supplier-bol-report/supplier-bol-report.component.ts": function srcAppLfvDashboardSupplierBolReportSupplierBolReportComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SupplierBolReportComponent", function () {
      return SupplierBolReportComponent;
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


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function SupplierBolReportComponent_tbody_31_tr_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var record_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](record_r3.CallId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.BOL, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.TerminalCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.Terminal, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.CorrectedQuanity, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.TerminalItemCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.ProductType, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.CarrierID, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.CarrierName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.FileName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", record_r3.RecordDate, " ");
      }
    }

    function SupplierBolReportComponent_tbody_31_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SupplierBolReportComponent_tbody_31_tr_1_Template, 23, 11, "tr", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.ReportRecords);
      }
    }

    function SupplierBolReportComponent_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var SupplierBolReportComponent = /*#__PURE__*/function () {
      function SupplierBolReportComponent(dashboardservice) {
        _classCallCheck(this, SupplierBolReportComponent);

        this.dashboardservice = dashboardservice;
        this.ReportRecords = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.ShowGridLoader = false;
      }

      _createClass(SupplierBolReportComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.intializeGrid();
        }
      }, {
        key: "intializeGrid",
        value: function intializeGrid() {
          this.ShowGridLoader = true;
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
              title: 'Lift File Records',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Lift File Records',
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
          this.getSupplierBOLReport();
        }
      }, {
        key: "getSupplierBOLReport",
        value: function getSupplierBOLReport() {
          var _this26 = this;

          this.ShowGridLoader = true;
          this.dashboardservice.getSupplierBOLReport().subscribe(function (data) {
            _this26.ShowGridLoader = false;
            _this26.ReportRecords = data;

            _this26.dtTrigger.next();
          });
        }
      }, {
        key: "reloadGrid",
        value: function reloadGrid() {
          $("#supplierbolreport-datatable").DataTable().clear().destroy();
          this.getSupplierBOLReport();
        }
      }]);

      return SupplierBolReportComponent;
    }();

    SupplierBolReportComponent.ɵfac = function SupplierBolReportComponent_Factory(t) {
      return new (t || SupplierBolReportComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"]));
    };

    SupplierBolReportComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: SupplierBolReportComponent,
      selectors: [["app-supplier-bol-report"]],
      decls: 33,
      vars: 4,
      consts: [[1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "table-responsive"], ["id", "supplierbolreport-datatable", "data-gridname", "16", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CallId"], ["data-key", "Bol"], ["data-key", "TerminalCode"], ["data-key", "Terminal"], ["data-key", "CorrectedQuanity"], ["data-key", "TerminalItemCode"], ["data-key", "ProductType"], ["data-key", "CarrierID"], ["data-key", "CarrierName"], ["data-key", "FileName"], ["data-key", "RecordDate"], [4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngFor", "ngForOf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function SupplierBolReportComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "table", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "th", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "CallId");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "th", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "BOL");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "th", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Terminal Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "th", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "th", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Corrected Quanity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Product Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "File Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30, "Record Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](31, SupplierBolReportComponent_tbody_31_Template, 2, 1, "tbody", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, SupplierBolReportComponent_div_32_Template, 5, 0, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.ReportRecords == null ? null : ctx.ReportRecords.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.ShowGridLoader);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvc3VwcGxpZXItYm9sLXJlcG9ydC9zdXBwbGllci1ib2wtcmVwb3J0LmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SupplierBolReportComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-supplier-bol-report',
          templateUrl: './supplier-bol-report.component.html',
          styleUrls: ['./supplier-bol-report.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_2__["LiftfiledashboardserviceService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/lfv-dashboard/validation/validation.component.ts": function srcAppLfvDashboardValidationValidationComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ValidationComponent", function () {
      return ValidationComponent;
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


    var _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../LiftFileModels */
    "./src/app/lfv-dashboard/LiftFileModels.ts");
    /* harmony import */


    var _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../service/liftfiledashboardservice.service */
    "./src/app/lfv-dashboard/service/liftfiledashboardservice.service.ts");

    var ValidationComponent = /*#__PURE__*/function () {
      function ValidationComponent(_lfvService) {
        _classCallCheck(this, ValidationComponent);

        this._lfvService = _lfvService;
      }

      _createClass(ValidationComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.init();
        }
      }, {
        key: "init",
        value: function init() {}
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
            this.createChartData();
          }
        } //public openLFVScratchReportGrid(): void {
        //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
        //}

      }, {
        key: "RendorChart",
        value: function RendorChart(data) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee9() {
            var options;
            return regeneratorRuntime.wrap(function _callee9$(_context9) {
              while (1) {
                switch (_context9.prev = _context9.next) {
                  case 0:
                    try {
                      if (this.chart) this.chart.destroy();
                    } catch (e) {}

                    options = {
                      colors: ["#00FF00", "#ff0000", "#FF69B4", "#FFFF00", "#000080", "#00A7C6", "#800080", '#0077ff', '#A9D794'],
                      series: [{
                        data: data
                      }],
                      chart: {
                        height: 350,
                        type: 'bar',
                        events: {
                          click: function click(chart, w, e) {
                            console.log(chart, w, e);
                          }
                        }
                      },
                      // colors: colors,
                      plotOptions: {
                        bar: {
                          columnWidth: '45%',
                          distributed: true
                        }
                      },
                      dataLabels: {
                        enabled: true
                      },
                      legend: {
                        show: true
                      },
                      xaxis: {
                        categories: ['Match', 'No Match', 'Partial Match', 'Pending', 'Duplicate', 'Active Exception', 'Ignored', 'Forced Ignore', 'UnMatched'],
                        labels: {
                          style: {
                            //colors: colors,
                            fontSize: '12px'
                          }
                        }
                      },
                      fill: {
                        opacity: 1 //colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']

                      }
                    };
                    this.chart = new ApexCharts(document.querySelector("#chart-timeline"), options);

                    try {
                      if (this.chart) this.chart.render();
                    } catch (e) {
                      this.chart.destroy();
                      this.chart.render();
                    }

                  case 4:
                  case "end":
                    return _context9.stop();
                }
              }
            }, _callee9, this);
          }));
        }
      }, {
        key: "createChartData",
        value: function createChartData() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee10() {
            var lfv, data;
            return regeneratorRuntime.wrap(function _callee10$(_context10) {
              while (1) {
                switch (_context10.prev = _context10.next) {
                  case 0:
                    lfv = new _LiftFileModels__WEBPACK_IMPORTED_MODULE_2__["LFValidationGridViewModel"]();
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
                    _context10.next = 13;
                    return this.LFValidationList;

                  case 13:
                    _context10.t0 = _context10.sent;

                    if (!_context10.t0) {
                      _context10.next = 16;
                      break;
                    }

                    this.LFValidationList.map(function (m) {
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

                  case 16:
                    data = [lfv.MatchedRecordCount, lfv.NoMatchRecordCount, lfv.PartialMatchRecordCount, lfv.PendingMatchCount, lfv.DuplicateRecordCount, lfv.ActiveExceptionRecordCount, lfv.IgnoredMatchRecordCount, lfv.ForcedIgnoredMatchRecordCount, lfv.UnmatchedRecordCount];
                    this.RendorChart(data);

                  case 18:
                  case "end":
                    return _context10.stop();
                }
              }
            }, _callee10, this);
          }));
        }
      }]);

      return ValidationComponent;
    }();

    ValidationComponent.ɵfac = function ValidationComponent_Factory(t) {
      return new (t || ValidationComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"]));
    };

    ValidationComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: ValidationComponent,
      selectors: [["app-validation"]],
      inputs: {
        LFValidationList: "LFValidationList"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 4,
      vars: 0,
      consts: [[1, "well", "bg-white", "shadow-b", "pr"], [1, "row"], [1, "col-md-10"], ["id", "chart-timeline"]],
      template: function ValidationComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }
      },
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2xmdi1kYXNoYm9hcmQvdmFsaWRhdGlvbi92YWxpZGF0aW9uLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](ValidationComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-validation',
          templateUrl: './validation.component.html',
          styleUrls: ['./validation.component.css']
        }]
      }], function () {
        return [{
          type: _service_liftfiledashboardservice_service__WEBPACK_IMPORTED_MODULE_3__["LiftfiledashboardserviceService"]
        }];
      }, {
        LFValidationList: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=lfv-dashboard-lfv-dashboard-module-es5.js.map