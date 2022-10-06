function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _toArray(arr) { return _arrayWithHoles(arr) || _iterableToArray(arr) || _unsupportedIterableToArray(arr) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _iterableToArray(iter) { if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null) return Array.from(iter); }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e2) { throw _e2; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e3) { didErr = true; err = _e3; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["driver-driver-module"], {
  /***/
  "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts": function srcAppDriverCreateDriverScheduleCreateDriverScheduleComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateDriverScheduleComponent", function () {
      return CreateDriverScheduleComponent;
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


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_driver_models_DriverSchedule__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/driver/models/DriverSchedule */
    "./src/app/driver/models/DriverSchedule.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_5__);
    /* harmony import */


    var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function CreateDriverScheduleComponent_div_17_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateDriverScheduleComponent_div_17_div_1_Template, 2, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r0.isRequired("regionId"));
      }
    }

    function CreateDriverScheduleComponent_div_24_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateDriverScheduleComponent_div_24_div_1_Template, 2, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.isRequired("driverId"));
      }
    }

    function CreateDriverScheduleComponent_div_30_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateDriverScheduleComponent_div_30_div_1_Template, 2, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.isRequired("shiftId"));
      }
    }

    function CreateDriverScheduleComponent_div_38_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateDriverScheduleComponent_div_38_div_1_Template, 2, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.isRequired("fromDate"));
      }
    }

    function CreateDriverScheduleComponent_div_45_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateDriverScheduleComponent_div_45_div_1_Template, 2, 0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r6.isRequired("toDate"));
      }
    }

    function CreateDriverScheduleComponent_div_61_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "input", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function CreateDriverScheduleComponent_div_61_Template_input_change_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.InitializeDates();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Custom");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverScheduleComponent_div_64_Template(rf, ctx) {
      if (rf & 1) {
        var _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " Dates:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "ng-multiselect-dropdown", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateDriverScheduleComponent_div_64_Template_ng_multiselect_dropdown_onSelect_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r18);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r17.validateShiftForDriver(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Date (s)")("settings", ctx_r8.multiselectDateSettingsById)("data", ctx_r8.DateList);
      }
    }

    function CreateDriverScheduleComponent_div_65_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Days:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "input", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var CreateDriverScheduleComponent = /*#__PURE__*/function () {
      //end
      function CreateDriverScheduleComponent(regionService, _fb) {
        _classCallCheck(this, CreateDriverScheduleComponent);

        this.regionService = regionService;
        this._fb = _fb; //@ViewChild('DriverScheduleForm') public form: NgForm;

        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.DriverList = [];
        this.regionList = [];
        this.isLoading = false; //sidebar variables

        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedShiftList = [];
        this.ShiftList = [];
        this.SelectedScheduleDriverList = [];
        this.RepeatList = [];
        this.DateList = []; //min max date

        this.MinStartDate = new Date();
        this.MaxStartDate = new Date(); //public getDateList() {
        //    this.DateList=  this.regionService.getDateList();
        //}
        //public onRepeatSelect($event): void {
        //    if ($event.Id == '1')
        //        this.DriverScheduleForm.controls.customDates.setValue(this.DateList);
        //    else
        //        this.DriverScheduleForm.controls.customDates.setValue([]);
        //}
        //public onRepeatDeSelect($event): void {
        //       this.DriverScheduleForm.controls.customDates.setValue([]);
        //}

        this.DriverShiftDetailList = [];
      }

      _createClass(CreateDriverScheduleComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.init();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.isLoading = false;
        }
      }, {
        key: "init",
        value: function init() {
          this.getRegions();
          this.createScheduleForm();
          this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2); // this.getDrivers();
          //  this.getShifts();
          // this.getDateList();

          this.multiselectDriverSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.multiselectShiftSettingsById = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.multiselectRepeatSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
          };
        }
      }, {
        key: "createScheduleForm",
        value: function createScheduleForm() {
          this.DriverScheduleForm = this._fb.group({
            id: [''],
            shiftId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            regionId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            driverId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            fromDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            toDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]]
          });
          var dt = moment__WEBPACK_IMPORTED_MODULE_5__(new Date()).toDate(); //alert(moment(dt).format('MM/DD/YYYY'));

          this.DriverScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'));
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
          }
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this = this;

          this.regionService.getRegions().subscribe(function (region) {
            _this.getRegionDropDwn(region.Regions);
          });
        }
      }, {
        key: "onRegionSelect",
        value: function onRegionSelect($event) {
          var region = this.regionList.find(function (f) {
            return f.Id == $event.Id;
          });
          this.DriverScheduleForm.controls.driverId.setValue('');
          this.DriverScheduleForm.controls.shiftId.setValue('');
          var tDate = new Date();
          this.DriverScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(tDate).format('MM/DD/YYYY'));
          this.setFromDate(moment__WEBPACK_IMPORTED_MODULE_5__(tDate).format('MM/DD/YYYY'));
          this.DriverList = region.Drivers;
          this.ShiftList = region.Shifts.map(function (res) {
            return {
              Id: res.Id,
              Name: "".concat(res.StartTime, " - ").concat(res.EndTime)
            };
          });
        }
      }, {
        key: "onRegionDeSelect",
        value: function onRegionDeSelect($event) {
          this.DriverList = [];
          this.DriverScheduleForm.controls.driverId.setValue('');
          this.ShiftList = [];
          this.DriverScheduleForm.controls.shiftId.setValue('');
        }
      }, {
        key: "getRegionDropDwn",
        value: function getRegionDropDwn(regionList) {
          this.regionList = regionList;
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(name) {
          var result = this.DriverScheduleForm.get(name).invalid && (this.DriverScheduleForm.get(name).dirty || this.DriverScheduleForm.get(name).touched);
          return result;
        }
      }, {
        key: "isRequired",
        value: function isRequired(name) {
          return this.DriverScheduleForm.get(name).errors.required;
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.DriverScheduleForm.controls.fromDate.setValue(event); //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();

          var d = moment__WEBPACK_IMPORTED_MODULE_5__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
          !this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_5__(d).format('MM/DD/YYYY')) : '';

          if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).toDate();

            if (_toDate < _fromDate) {
              this.DriverScheduleForm.controls.toDate.setValue(event);
            }
          }

          this.InitializeDates();
          this.validateShiftForDriver(false);
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
            var _fromDate, _toDate;

            return regeneratorRuntime.wrap(function _callee$(_context) {
              while (1) {
                switch (_context.prev = _context.next) {
                  case 0:
                    this.DriverScheduleForm.controls.toDate.setValue(event);

                    if (this.DriverScheduleForm.controls.fromDate.value != '' && this.DriverScheduleForm.controls.toDate.value != '') {
                      _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).toDate();
                      _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).toDate();

                      if (_fromDate > _toDate) {
                        this.DriverScheduleForm.controls.fromDate.setValue(event);
                      }
                    }

                    this.InitializeDates();
                    _context.next = 5;
                    return this.validateShiftForDriver(false);

                  case 5:
                  case "end":
                    return _context.stop();
                }
              }
            }, _callee, this);
          }));
        }
      }, {
        key: "InitializeDates",
        value: function InitializeDates(type, repeat) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
            var days, dt;
            return regeneratorRuntime.wrap(function _callee2$(_context2) {
              while (1) {
                switch (_context2.prev = _context2.next) {
                  case 0:
                    this.DriverScheduleForm.controls.customDates.setValue([]);
                    this.DateList = [];
                    !repeat ? repeat = 0 : '';
                    days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]; // alert(this.DriverScheduleForm.controls.fromDate.value);
                    // alert(this.DriverScheduleForm.controls.toDate.value);

                    if (!(this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value)) {
                      _context2.next = 7;
                      break;
                    }

                    for (dt = new Date(this.DriverScheduleForm.controls.fromDate.value); dt <= new Date(this.DriverScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                      if (type && type == 2) //weekend
                        new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6 ? this.DateList.push({
                          Id: new Date(dt),
                          Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                        }) : '';else this.DateList.push({
                        Id: new Date(dt),
                        Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                      });
                    }

                    return _context2.abrupt("return", this.DateList);

                  case 7:
                  case "end":
                    return _context2.stop();
                }
              }
            }, _callee2, this);
          }));
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
            var dates, model, repeatDayListString;
            return regeneratorRuntime.wrap(function _callee3$(_context3) {
              while (1) {
                switch (_context3.prev = _context3.next) {
                  case 0:
                    if (!this.DriverScheduleForm.invalid) {
                      _context3.next = 5;
                      break;
                    }

                    this.DriverScheduleForm.markAllAsTouched();
                    return _context3.abrupt("return", false);

                  case 5:
                    if (!(this.DriverScheduleForm.controls.type.value == '3')) {
                      _context3.next = 11;
                      break;
                    }

                    if (this.DriverScheduleForm.controls.repeat.value && this.DriverScheduleForm.controls.repeat.value > 0) {
                      _context3.next = 9;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return _context3.abrupt("return", false);

                  case 9:
                    _context3.next = 15;
                    break;

                  case 11:
                    if (!(this.DriverScheduleForm.controls.type.value == '4')) {
                      _context3.next = 15;
                      break;
                    }

                    if (this.DriverScheduleForm.controls.customDates.value.length > 0) {
                      _context3.next = 15;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return _context3.abrupt("return", false);

                  case 15:
                    _context3.next = 17;
                    return this.validateShiftForDriver(true);

                  case 17:
                    _context3.t0 = _context3.sent;

                    if (!(_context3.t0 == 1)) {
                      _context3.next = 20;
                      break;
                    }

                    return _context3.abrupt("return", false);

                  case 20:
                    if (!(this.DriverScheduleForm.controls.type.value == '2')) {
                      _context3.next = 28;
                      break;
                    }

                    _context3.t1 = this.DriverScheduleForm.controls.customDates;
                    _context3.next = 24;
                    return this.InitializeDates(this.DriverScheduleForm.controls.type.value);

                  case 24:
                    _context3.t2 = _context3.sent;

                    _context3.t1.setValue.call(_context3.t1, _context3.t2);

                    _context3.next = 42;
                    break;

                  case 28:
                    if (!(this.DriverScheduleForm.controls.type.value == '3')) {
                      _context3.next = 36;
                      break;
                    }

                    _context3.t3 = this.DriverScheduleForm.controls.customDates;
                    _context3.next = 32;
                    return this.InitializeDates(this.DriverScheduleForm.controls.type.value, this.DriverScheduleForm.controls.repeat.value);

                  case 32:
                    _context3.t4 = _context3.sent;

                    _context3.t3.setValue.call(_context3.t3, _context3.t4);

                    _context3.next = 42;
                    break;

                  case 36:
                    if (!(this.DriverScheduleForm.controls.type.value == '1')) {
                      _context3.next = 42;
                      break;
                    }

                    _context3.t5 = this.DriverScheduleForm.controls.customDates;
                    _context3.next = 40;
                    return this.InitializeDates();

                  case 40:
                    _context3.t6 = _context3.sent;

                    _context3.t5.setValue.call(_context3.t5, _context3.t6);

                  case 42:
                    dates = []; //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
                    //!this.DriverScheduleForm.controls.toDate.value ? this.DriverScheduleForm.controls.toDate.setValue(moment(d).format('MM/DD/YYYY')) : '';

                    _context3.next = 45;
                    return this.DriverScheduleForm.controls.customDates.value.map(function (res) {
                      dates.push(res.Id);
                    });

                  case 45:
                    model = new src_app_driver_models_DriverSchedule__WEBPACK_IMPORTED_MODULE_4__["DriverScheduleMapping"]();
                    this.DriverScheduleForm.controls.id.value ? model.Id = this.DriverScheduleForm.controls.id.value : '';
                    model.DriverId = this.DriverScheduleForm.controls.driverId.value[0].Id;
                    repeatDayListString = [];
                    dates.forEach(function (x) {
                      repeatDayListString.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                    });
                    model.ScheduleList.push({
                      Id: "".concat(model.DriverId, "_").concat(new Date().getTime()),
                      IsActive: true,
                      StartDate: this.DriverScheduleForm.controls.fromDate.value,
                      EndDate: this.DriverScheduleForm.controls.toDate.value,
                      RepeatDayList: dates,
                      RepeatDayStringList: repeatDayListString,
                      ShiftId: this.DriverScheduleForm.controls.shiftId.value[0].Id,
                      Type: this.DriverScheduleForm.controls.type.value,
                      RepeatEveryDay: this.DriverScheduleForm.controls.repeat.value,
                      TypeId: this.DriverScheduleForm.controls.type.value
                    });
                    this.addDriverSchedule(model);

                  case 52:
                  case "end":
                    return _context3.stop();
                }
              }
            }, _callee3, this);
          }));
        }
      }, {
        key: "addDriverSchedule",
        value: function addDriverSchedule(model) {
          var _this2 = this;

          this.isLoading = true;
          this.regionService.onLoadingChanged.next(true);
          this.regionService.addDriverSchedule(model).subscribe(function (response) {
            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess('Driver Schedule created successfully', undefined, undefined);
              _this2.isLoading = false;

              _this2.regionService.onLoadingChanged.next(false);

              _this2._toggleOpened(false);

              var driver = _this2.DriverScheduleForm.controls.driverId.value;

              _this2.DriverScheduleForm.reset();

              _this2.OnScheduleAdded.emit(driver);

              _this2.DriverScheduleForm.controls.type.setValue('1');
            } else {
              _this2.isLoading = false;

              _this2.regionService.onLoadingChanged.next(false);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "closedSideBar",
        value: function closedSideBar() {
          this._opened = false;
        }
      }, {
        key: "onDriverSelect",
        value: function onDriverSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee4() {
            var _this3 = this;

            var driverIds, drivers;
            return regeneratorRuntime.wrap(function _callee4$(_context4) {
              while (1) {
                switch (_context4.prev = _context4.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(true);
                    driverIds = [];
                    this.DriverScheduleForm.controls.driverId.value.forEach(function (res) {
                      driverIds.push(res.Id);
                    });
                    drivers = driverIds.join();

                    if (driverIds.length > 0) {
                      this.regionService.getShiftByDrivers(drivers, 0) // schedule type
                      .subscribe(function (data) {
                        if (data.Result) _this3.DriverShiftDetailList = data.Result;
                      });
                    }

                    this.regionService.onLoadingChanged.next(false);

                  case 6:
                  case "end":
                    return _context4.stop();
                }
              }
            }, _callee4, this);
          }));
        }
      }, {
        key: "onDriverDeSelect",
        value: function onDriverDeSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee5() {
            return regeneratorRuntime.wrap(function _callee5$(_context5) {
              while (1) {
                switch (_context5.prev = _context5.next) {
                  case 0:
                  case "end":
                    return _context5.stop();
                }
              }
            }, _callee5);
          }));
        }
      }, {
        key: "validateShiftForDriver",
        value: function validateShiftForDriver(isSubmit) {
          var e_1, _a, e_2, _b;

          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee6() {
            var i, rpDayList, _c, _d, item, _e, _f, shift;

            return regeneratorRuntime.wrap(function _callee6$(_context6) {
              while (1) {
                switch (_context6.prev = _context6.next) {
                  case 0:
                    i = 0;
                    rpDayList = [];
                    if (this.DriverScheduleForm.controls.type.value == 4) rpDayList = this.DriverScheduleForm.controls.customDates.value;else rpDayList = this.DateList;

                    if (!(this.DriverScheduleForm.controls.fromDate.value && this.DriverScheduleForm.controls.toDate.value && this.DriverScheduleForm.controls.shiftId.value && this.DriverScheduleForm.controls.shiftId.value.length > 0)) {
                      _context6.next = 63;
                      break;
                    }

                    _context6.prev = 4;
                    _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.DriverShiftDetailList);

                  case 6:
                    _context6.next = 8;
                    return _c.next();

                  case 8:
                    _d = _context6.sent;

                    if (_d.done) {
                      _context6.next = 47;
                      break;
                    }

                    item = _d.value;
                    _context6.prev = 11;
                    _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.ScheduleList);

                  case 13:
                    _context6.next = 15;
                    return _e.next();

                  case 15:
                    _f = _context6.sent;

                    if (_f.done) {
                      _context6.next = 28;
                      break;
                    }

                    shift = _f.value;

                    if (!(shift.RepeatDayList != null && moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_5__(shift.StartDate).format('MM/DD/YYYY') && moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_5__(shift.EndDate).format('MM/DD/YYYY') || moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_5__(shift.StartDate).format('MM/DD/YYYY') && moment__WEBPACK_IMPORTED_MODULE_5__(this.DriverScheduleForm.controls.toDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_5__(shift.EndDate).format('MM/DD/YYYY'))) {
                      _context6.next = 26;
                      break;
                    }

                    if (!(this.DriverScheduleForm.controls.shiftId.value[0].Id == shift.ShiftId)) {
                      _context6.next = 25;
                      break;
                    }

                    shift.RepeatDayList.forEach(function (ele) {
                      var idx = rpDayList.findIndex(function (x) {
                        return moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(ele).format('MM/DD/YYYY');
                      });

                      if (idx >= 0) {
                        if (i != 1) {
                          src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Shift is already assigned to the driver", undefined, undefined);
                          i = 1;
                        }
                      }
                    });

                    if (!(i == 1)) {
                      _context6.next = 23;
                      break;
                    }

                    return _context6.abrupt("break", 28);

                  case 23:
                    _context6.next = 26;
                    break;

                  case 25:
                    !isSubmit ? src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Another shift is already assigned to the driver", undefined, undefined) : ''; //i = 1;
                    //break;

                  case 26:
                    _context6.next = 13;
                    break;

                  case 28:
                    _context6.next = 33;
                    break;

                  case 30:
                    _context6.prev = 30;
                    _context6.t0 = _context6["catch"](11);
                    e_2 = {
                      error: _context6.t0
                    };

                  case 33:
                    _context6.prev = 33;
                    _context6.prev = 34;

                    if (!(_f && !_f.done && (_b = _e["return"]))) {
                      _context6.next = 38;
                      break;
                    }

                    _context6.next = 38;
                    return _b.call(_e);

                  case 38:
                    _context6.prev = 38;

                    if (!e_2) {
                      _context6.next = 41;
                      break;
                    }

                    throw e_2.error;

                  case 41:
                    return _context6.finish(38);

                  case 42:
                    return _context6.finish(33);

                  case 43:
                    if (!(i == 1)) {
                      _context6.next = 45;
                      break;
                    }

                    return _context6.abrupt("break", 47);

                  case 45:
                    _context6.next = 6;
                    break;

                  case 47:
                    _context6.next = 52;
                    break;

                  case 49:
                    _context6.prev = 49;
                    _context6.t1 = _context6["catch"](4);
                    e_1 = {
                      error: _context6.t1
                    };

                  case 52:
                    _context6.prev = 52;
                    _context6.prev = 53;

                    if (!(_d && !_d.done && (_a = _c["return"]))) {
                      _context6.next = 57;
                      break;
                    }

                    _context6.next = 57;
                    return _a.call(_c);

                  case 57:
                    _context6.prev = 57;

                    if (!e_1) {
                      _context6.next = 60;
                      break;
                    }

                    throw e_1.error;

                  case 60:
                    return _context6.finish(57);

                  case 61:
                    return _context6.finish(52);

                  case 62:
                    return _context6.abrupt("return", i);

                  case 63:
                    return _context6.abrupt("return", i);

                  case 64:
                  case "end":
                    return _context6.stop();
                }
              }
            }, _callee6, this, [[4, 49, 52, 62], [11, 30, 33, 43], [34,, 38, 42], [53,, 57, 61]]);
          }));
        }
      }]);

      return CreateDriverScheduleComponent;
    }();

    CreateDriverScheduleComponent.ɵfac = function CreateDriverScheduleComponent_Factory(t) {
      return new (t || CreateDriverScheduleComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]));
    };

    CreateDriverScheduleComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: CreateDriverScheduleComponent,
      selectors: [["create-driver-schedule"]],
      outputs: {
        OnScheduleAdded: "OnScheduleAdded"
      },
      decls: 70,
      vars: 27,
      consts: [["id", "driverSchedule", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["name", "DriverScheduleForm", "autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "regionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Drivers"], ["formControlName", "driverId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["for", "Shift"], ["formControlName", "shiftId", 3, "placeholder", "settings", "data", "onSelect"], ["for", "fromDate"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], [1, "color-maroon"], [4, "ngIf"], ["type", "radio", "name", "type", "formControlName", "type", "value", "4", "id", "inlineRadioCustom", 1, "form-check-input", 3, "change"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], ["formControlName", "customDates", 3, "placeholder", "settings", "data", "onSelect"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]],
      template: function CreateDriverScheduleComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "button", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateDriverScheduleComponent_Template_button_click_0_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Add Schedule ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "i", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "ng-sidebar-container", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "ng-sidebar", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("openedChange", function CreateDriverScheduleComponent_Template_ng_sidebar_openedChange_5_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateDriverScheduleComponent_Template_a_click_6_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "h3", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Schedule Driver ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "content", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("keydown.enter", function CreateDriverScheduleComponent_Template_content_keydown_enter_10_listener($event) {
            return $event.preventDefault();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Region:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_16_listener($event) {
            return ctx.onRegionSelect($event);
          })("onDeSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_16_listener($event) {
            return ctx.onRegionDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, CreateDriverScheduleComponent_div_17_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "label", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Driver:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "ng-multiselect-dropdown", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_23_listener($event) {
            return ctx.onDriverSelect($event);
          })("onDeSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_23_listener($event) {
            return ctx.onDriverDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, CreateDriverScheduleComponent_div_24_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "label", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Shift:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "ng-multiselect-dropdown", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateDriverScheduleComponent_Template_ng_multiselect_dropdown_onSelect_29_listener() {
            return ctx.validateShiftForDriver(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](30, CreateDriverScheduleComponent_div_30_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, "From Date:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "input", 19, 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateDriverScheduleComponent_Template_input_onDateChange_36_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, CreateDriverScheduleComponent_div_38_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](42, "To Date:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "input", 21, 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateDriverScheduleComponent_Template_input_onDateChange_43_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](45, CreateDriverScheduleComponent_div_45_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](50, "input", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "label", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](52, "Daily");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](54, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "label", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](56, "WeekDays (Mon to Fri)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](58, "input", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "label", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](60, "Repeat Every");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](61, CreateDriverScheduleComponent_div_61_Template, 4, 0, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](64, CreateDriverScheduleComponent_div_64_Template, 4, 3, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](65, CreateDriverScheduleComponent_div_65_Template, 4, 0, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](66, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "input", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateDriverScheduleComponent_Template_input_click_67_listener() {
            return ctx.closedSideBar();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "button", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateDriverScheduleComponent_Template_button_click_68_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](69, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.DriverScheduleForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Region(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.regionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("regionId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Driver(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.DriverList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("driverId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Shift(s)")("settings", ctx.multiselectDriverSettingsById)("data", ctx.ShiftList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("shiftId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("fromDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("toDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.DriverScheduleForm.get("fromDate").value && ctx.DriverScheduleForm.get("toDate").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.DriverScheduleForm.get("type").value == "4");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.DriverScheduleForm.get("type").value == "3");
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]],
      styles: [".hide_chart[_ngcontent-%COMP%] {\r\n    display:none;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2NyZWF0ZS1kcml2ZXItc2NoZWR1bGUvY3JlYXRlLWRyaXZlci1zY2hlZHVsZS5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksWUFBWTtBQUNoQiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtZHJpdmVyLXNjaGVkdWxlL2NyZWF0ZS1kcml2ZXItc2NoZWR1bGUuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5oaWRlX2NoYXJ0IHtcclxuICAgIGRpc3BsYXk6bm9uZTtcclxufVxyXG4iXX0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CreateDriverScheduleComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'create-driver-schedule',
          templateUrl: './create-driver-schedule.component.html',
          styleUrls: ['./create-driver-schedule.component.css']
        }]
      }], function () {
        return [{
          type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }];
      }, {
        OnScheduleAdded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/driver/create-region-schedule/create-region.component.ts": function srcAppDriverCreateRegionScheduleCreateRegionComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateRegionComponent", function () {
      return CreateRegionComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/driver/models/regionSchedule */
    "./src/app/driver/models/regionSchedule.ts");
    /* harmony import */


    var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function CreateRegionComponent_div_17_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_17_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r0.isRequired("RegionId"));
      }
    }

    function CreateRegionComponent_div_23_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_23_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.isRequired("RouteId"));
      }
    }

    function CreateRegionComponent_div_29_div_10_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_29_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_29_div_10_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r12.isRequired("ShiftId"));
      }
    }

    function CreateRegionComponent_div_29_div_18_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_29_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_29_div_18_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r13.isRequired("ColumnIndex"));
      }
    }

    function CreateRegionComponent_div_29_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Shift ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "ng-multiselect-dropdown", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateRegionComponent_div_29_Template_ng_multiselect_dropdown_onSelect_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r17);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r16.onShiftSelect($event);
        })("onDeSelect", function CreateRegionComponent_div_29_Template_ng_multiselect_dropdown_onDeSelect_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r17);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r18.onShiftDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](10, CreateRegionComponent_div_29_div_10_Template, 2, 1, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "label", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Column");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](17, "ng-multiselect-dropdown", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, CreateRegionComponent_div_29_div_18_Template, 2, 1, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](20, "label", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "a", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_div_29_Template_a_click_21_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r17);

          var i_r11 = ctx.index;

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r19.removeShift(i_r11);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "i", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Remove");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r11 = ctx.index;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroupName", i_r11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Shift(s)")("settings", ctx_r2.multiselectRegionSettingsById)("data", ctx_r2.ShiftList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.isInvalid("ShiftId"));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Columns")("settings", ctx_r2.multiselectRegionSettingsById)("data", ctx_r2.ColumnsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r2.isInvalid("ColumnIndex"));
      }
    }

    function CreateRegionComponent_div_39_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_39_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_39_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.isRequired("fromDate"));
      }
    }

    function CreateRegionComponent_div_48_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateRegionComponent_div_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateRegionComponent_div_48_div_1_Template, 2, 0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r6.isRequired("toDate"));
      }
    }

    function CreateRegionComponent_div_66_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Days:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var CreateRegionComponent = /*#__PURE__*/function () {
      function CreateRegionComponent(regionService, _fb) {
        _classCallCheck(this, CreateRegionComponent);

        this.regionService = regionService;
        this._fb = _fb;
        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.regionList = [];
        this.DriverList = [];
        this.RouteList = [];
        this.ShiftList = [];
        this.RepeatList = [];
        this.DateList = [];
        this.ColumnsList = [];
        this.ShiftScheduleList = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.SelectedShiftList = [];
        this.isLoading = false;
        this.IsDuplicateShift = false;
        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
      }

      _createClass(CreateRegionComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.Init();
        }
      }, {
        key: "Init",
        value: function Init() {
          this.createScheduleForm();
          this.clear();
          this.getRegions();
          this.multiselectRegionSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true,
            enableCheckAll: false
          };
          this.multiselectRouteSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.isLoading = false;
        } //#region  Page Load Functions

      }, {
        key: "createScheduleForm",
        value: function createScheduleForm() {
          this.CreateRegionForm = this._fb.group({
            id: this._fb.control[''],
            RegionId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            RouteId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            ShiftId: [''],
            ColumnIndex: [''],
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            RegionShiftDetail: this._fb.array([this.getShift()]),
            fromDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            toDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]]
          });
          var dt = moment__WEBPACK_IMPORTED_MODULE_3__(new Date()).toDate();
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this4 = this;

          this.regionService.getRegions().subscribe(function (region) {
            _this4.getRegionDropDwn(region.Regions);
          });
        }
      }, {
        key: "getShift",
        value: function getShift() {
          return this._fb.group({
            ShiftId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            ColumnIndex: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required)
          });
        } //#endregion

      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
          }
        } //#region Multiselect events 

      }, {
        key: "onRegionSelect",
        value: function onRegionSelect($event) {
          this.ColumnsList = [];
          var region = this.regionList.find(function (f) {
            return f.Id == $event.Id;
          });
          this.getRoutes(region.Id);
          this.ShiftList = region.Shifts.map(function (res) {
            return {
              Id: res.Id,
              Name: "".concat(res.Name, " ( ").concat(res.StartTime, " - ").concat(res.EndTime, ")")
            };
          });
          this.createColumnList(region);
        }
      }, {
        key: "onRegionDeSelect",
        value: function onRegionDeSelect($event) {
          this.clear();
        }
      }, {
        key: "clear",
        value: function clear() {
          this.DriverList = [];
          this.ShiftList = [];
          this.ColumnsList = [];
          this.RouteList = [];
          this.CreateRegionForm.controls.RegionShiftDetail = this._fb.array([]);
          this.CreateRegionForm.controls.RegionShiftDetail = this._fb.array([this.getShift()]), this.CreateRegionForm.reset();
        }
      }, {
        key: "onRouteSelect",
        value: function onRouteSelect($event) {
          var regionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
          this.getShiftSchedules(regionId, $event.Id);
        }
      }, {
        key: "onShiftSelect",
        value: function onShiftSelect($event) {
          var shift = this.ShiftList.find(function (f) {
            return f.Id == $event.Id;
          });
          this.IsDuplicateShift = false;
          this.CheckDuplicateShits(shift);
        }
      }, {
        key: "onShiftDeSelect",
        value: function onShiftDeSelect($event) {} //#endregion
        //#region   Date

      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.CreateRegionForm.controls.fromDate.setValue(event);
          var d = moment__WEBPACK_IMPORTED_MODULE_3__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
          !this.CreateRegionForm.controls.toDate.value ? this.CreateRegionForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(d).format('MM/DD/YYYY')) : '';

          if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).toDate();

            if (_toDate < _fromDate) {
              this.CreateRegionForm.controls.toDate.setValue(event);
            }
          }

          this.InitializeDates();
          this.validateShiftForRegion(false);
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee7() {
            var _fromDate, _toDate;

            return regeneratorRuntime.wrap(function _callee7$(_context7) {
              while (1) {
                switch (_context7.prev = _context7.next) {
                  case 0:
                    this.CreateRegionForm.controls.toDate.setValue(event);

                    if (this.CreateRegionForm.controls.fromDate.value != '' && this.CreateRegionForm.controls.toDate.value != '') {
                      _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).toDate();
                      _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).toDate();

                      if (_fromDate > _toDate) {
                        this.CreateRegionForm.controls.fromDate.setValue(event);
                      }
                    }

                    this.InitializeDates();
                    this.validateShiftForRegion(false);

                  case 4:
                  case "end":
                    return _context7.stop();
                }
              }
            }, _callee7, this);
          }));
        }
      }, {
        key: "InitializeDates",
        value: function InitializeDates(type, repeat) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee8() {
            var days, dt;
            return regeneratorRuntime.wrap(function _callee8$(_context8) {
              while (1) {
                switch (_context8.prev = _context8.next) {
                  case 0:
                    this.CreateRegionForm.controls.customDates.setValue([]);
                    this.DateList = [];
                    !repeat ? repeat = 0 : '';
                    days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

                    if (!(this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value)) {
                      _context8.next = 7;
                      break;
                    }

                    for (dt = new Date(this.CreateRegionForm.controls.fromDate.value); dt <= new Date(this.CreateRegionForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                      if (type && type == 2) //weekend
                        new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6 ? this.DateList.push({
                          Id: new Date(dt),
                          Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'), " ")
                        }) : ''; //(${days[new Date(dt).getDay()]})
                      else this.DateList.push({
                        Id: new Date(dt),
                        Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'))
                      }); //(${days[new Date(dt).getDay()]})
                    }

                    return _context8.abrupt("return", this.DateList);

                  case 7:
                  case "end":
                    return _context8.stop();
                }
              }
            }, _callee8, this);
          }));
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(name) {
          var result = this.CreateRegionForm.get(name).invalid && (this.CreateRegionForm.get(name).dirty || this.CreateRegionForm.get(name).touched);
          return result;
        }
      }, {
        key: "isRequired",
        value: function isRequired(name) {
          return this.CreateRegionForm.get(name).errors.required;
        } //#endregion
        //#region  Button Events 

      }, {
        key: "closedSideBar",
        value: function closedSideBar() {
          this.clear();
          this._opened = false;
        }
      }, {
        key: "addShift",
        value: function addShift() {
          var _shifts = this.CreateRegionForm.get('RegionShiftDetail');

          _shifts.push(this.getShift());
        }
      }, {
        key: "removeShift",
        value: function removeShift(index) {
          var _shifts = this.CreateRegionForm.get('RegionShiftDetail');

          _shifts.removeAt(index);

          this.IsDuplicateShift = false;
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee9() {
            var ScheduleList, dates, model;
            return regeneratorRuntime.wrap(function _callee9$(_context9) {
              while (1) {
                switch (_context9.prev = _context9.next) {
                  case 0:
                    if (!this.CreateRegionForm.invalid) {
                      _context9.next = 6;
                      break;
                    }

                    this.CreateRegionForm.markAllAsTouched();
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('All fields are mandatory', undefined, undefined);
                    return _context9.abrupt("return", false);

                  case 6:
                    if (!(this.CreateRegionForm.controls.type.value == '3')) {
                      _context9.next = 12;
                      break;
                    }

                    if (this.CreateRegionForm.controls.repeat.value && this.CreateRegionForm.controls.repeat.value > 0) {
                      _context9.next = 10;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return _context9.abrupt("return", false);

                  case 10:
                    _context9.next = 16;
                    break;

                  case 12:
                    if (!(this.CreateRegionForm.controls.type.value == '4')) {
                      _context9.next = 16;
                      break;
                    }

                    if (this.CreateRegionForm.controls.customDates.value.length > 0) {
                      _context9.next = 16;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return _context9.abrupt("return", false);

                  case 16:
                    ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;

                    if (!(ScheduleList != null && ScheduleList.length > 0)) {
                      _context9.next = 23;
                      break;
                    }

                    if (!this.CheckDuplicateShitsOnSubmit()) {
                      _context9.next = 21;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Same selection of shift will not work", undefined, undefined);
                    return _context9.abrupt("return", false);

                  case 21:
                    _context9.next = 25;
                    break;

                  case 23:
                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Atlease one schedule is required, please select Shift and column', undefined, undefined);
                    return _context9.abrupt("return", false);

                  case 25:
                    _context9.next = 27;
                    return this.validateShiftForRegion(true);

                  case 27:
                    _context9.t0 = _context9.sent;

                    if (!(_context9.t0 == 1)) {
                      _context9.next = 30;
                      break;
                    }

                    return _context9.abrupt("return", false);

                  case 30:
                    if (!(this.CreateRegionForm.controls.type.value == '2')) {
                      _context9.next = 38;
                      break;
                    }

                    _context9.t1 = this.CreateRegionForm.controls.customDates;
                    _context9.next = 34;
                    return this.InitializeDates(this.CreateRegionForm.controls.type.value);

                  case 34:
                    _context9.t2 = _context9.sent;

                    _context9.t1.setValue.call(_context9.t1, _context9.t2);

                    _context9.next = 52;
                    break;

                  case 38:
                    if (!(this.CreateRegionForm.controls.type.value == '3')) {
                      _context9.next = 46;
                      break;
                    }

                    _context9.t3 = this.CreateRegionForm.controls.customDates;
                    _context9.next = 42;
                    return this.InitializeDates(this.CreateRegionForm.controls.type.value, this.CreateRegionForm.controls.repeat.value);

                  case 42:
                    _context9.t4 = _context9.sent;

                    _context9.t3.setValue.call(_context9.t3, _context9.t4);

                    _context9.next = 52;
                    break;

                  case 46:
                    if (!(this.CreateRegionForm.controls.type.value == '1')) {
                      _context9.next = 52;
                      break;
                    }

                    _context9.t5 = this.CreateRegionForm.controls.customDates;
                    _context9.next = 50;
                    return this.InitializeDates();

                  case 50:
                    _context9.t6 = _context9.sent;

                    _context9.t5.setValue.call(_context9.t5, _context9.t6);

                  case 52:
                    dates = [];
                    _context9.next = 55;
                    return this.CreateRegionForm.controls.customDates.value.map(function (res) {
                      dates.push(res.Name);
                    });

                  case 55:
                    model = new src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__["RegionScheduleViewModel"]();
                    this.CreateRegionForm.controls.id.value ? model.Id = this.CreateRegionForm.controls.id.value : '';
                    model.RegionId = this.CreateRegionForm.controls.RegionId.value[0].Id;
                    model.RouteId = this.CreateRegionForm.controls.RouteId.value[0].Id;
                    model.StartDate = this.CreateRegionForm.controls.fromDate.value;
                    model.EndDate = this.CreateRegionForm.controls.toDate.value;
                    ScheduleList.forEach(function (element) {
                      var objShiftModel = new src_app_driver_models_regionSchedule__WEBPACK_IMPORTED_MODULE_5__["ShiftSchedule"]();
                      objShiftModel.ShiftId = element['ShiftId'][0]['Id'];
                      objShiftModel.ColumnIndex = parseInt(element['ColumnIndex'][0]['Id']);
                      model.RegionShiftDetail.push(objShiftModel);
                    });
                    model.Repeat = this.CreateRegionForm.controls.repeat.value;
                    model.RepeatDayList = dates;
                    model.IsActive = true;
                    this.addRouteSchedule(model);

                  case 66:
                  case "end":
                    return _context9.stop();
                }
              }
            }, _callee9, this);
          }));
        } //#endregion
        //#region private functions 

      }, {
        key: "validateShiftForRegion",
        value: function validateShiftForRegion(isSubmit) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee10() {
            var k, ScheduleList, _iterator, _step, shift, i, iShift, j, jShift;

            return regeneratorRuntime.wrap(function _callee10$(_context10) {
              while (1) {
                switch (_context10.prev = _context10.next) {
                  case 0:
                    k = 0;

                    if (!(this.CreateRegionForm.controls.fromDate.value && this.CreateRegionForm.controls.toDate.value && this.CreateRegionForm.controls.RegionId.value && this.CreateRegionForm.controls.RouteId.value)) {
                      _context10.next = 42;
                      break;
                    }

                    ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;
                    _iterator = _createForOfIteratorHelper(this.ShiftScheduleList);
                    _context10.prev = 4;

                    _iterator.s();

                  case 6:
                    if ((_step = _iterator.n()).done) {
                      _context10.next = 33;
                      break;
                    }

                    shift = _step.value;

                    if (!(moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_3__(shift.StartDate).format('MM/DD/YYYY') && moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.fromDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_3__(shift.EndDate).format('MM/DD/YYYY') || moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') >= moment__WEBPACK_IMPORTED_MODULE_3__(shift.StartDate).format('MM/DD/YYYY') && moment__WEBPACK_IMPORTED_MODULE_3__(this.CreateRegionForm.controls.toDate.value).format('MM/DD/YYYY') <= moment__WEBPACK_IMPORTED_MODULE_3__(shift.EndDate).format('MM/DD/YYYY'))) {
                      _context10.next = 29;
                      break;
                    }

                    if (!(ScheduleList != null && ScheduleList.length > 0)) {
                      _context10.next = 29;
                      break;
                    }

                    i = 0;

                  case 11:
                    if (!(i < ScheduleList.length)) {
                      _context10.next = 29;
                      break;
                    }

                    iShift = ScheduleList[i];
                    j = 0;

                  case 14:
                    if (!(j < shift.RegionShiftDetail.length)) {
                      _context10.next = 26;
                      break;
                    }

                    jShift = shift.RegionShiftDetail[j];

                    if (!(iShift.ShiftId != null && iShift.ShiftId[0].Id == jShift.ShiftId)) {
                      _context10.next = 22;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Shift is already assigned to the Region", undefined, undefined);
                    k = 1;
                    return _context10.abrupt("return", false);

                  case 22:
                    !isSubmit ? src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgwarning("Another shift is already assigned to the Region", undefined, undefined) : '';

                  case 23:
                    j++;
                    _context10.next = 14;
                    break;

                  case 26:
                    i++;
                    _context10.next = 11;
                    break;

                  case 29:
                    if (!(k == 1)) {
                      _context10.next = 31;
                      break;
                    }

                    return _context10.abrupt("break", 33);

                  case 31:
                    _context10.next = 6;
                    break;

                  case 33:
                    _context10.next = 38;
                    break;

                  case 35:
                    _context10.prev = 35;
                    _context10.t0 = _context10["catch"](4);

                    _iterator.e(_context10.t0);

                  case 38:
                    _context10.prev = 38;

                    _iterator.f();

                    return _context10.finish(38);

                  case 41:
                    return _context10.abrupt("return", k);

                  case 42:
                    return _context10.abrupt("return", k);

                  case 43:
                  case "end":
                    return _context10.stop();
                }
              }
            }, _callee10, this, [[4, 35, 38, 41]]);
          }));
        }
      }, {
        key: "addRouteSchedule",
        value: function addRouteSchedule(model) {
          var _this5 = this;

          this.isLoading = true;
          this.regionService.onLoadingChanged.next(true);
          this.regionService.addRegionSchedule(model).subscribe(function (response) {
            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess('Region Schedule created successfully', undefined, undefined);
              _this5.isLoading = false;

              _this5.regionService.onLoadingChanged.next(false);

              _this5._toggleOpened(false);

              var driver = _this5.CreateRegionForm.controls.driverId.value;

              _this5.CreateRegionForm.reset();

              _this5.OnScheduleAdded.emit(driver);
            } else {
              _this5.isLoading = false;

              _this5.regionService.onLoadingChanged.next(false);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "createColumnList",
        value: function createColumnList(region) {
          var _this6 = this;

          this.DriverList = region.Drivers;

          if (region.Drivers.length > 0) {
            var num = 0;
            this.DriverList.forEach(function (obj) {
              var col = {
                Id: 0,
                Name: ""
              };
              col.Id = num;
              col.Name = "C" + num;

              _this6.ColumnsList.push(col);

              num++;
            });
          }
        }
      }, {
        key: "CheckDuplicateShits",
        value: function CheckDuplicateShits(shift) {
          var cnt = 1;
          var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;

          if (ScheduleList.length > 1) {
            for (var i = 0; i < ScheduleList.length; i++) {
              var iShift = ScheduleList[i];

              if (iShift.ShiftId != "" && iShift.ShiftId[0].Id == shift.Id) {
                if (cnt > 1) this.IsDuplicateShift = true;
                cnt++;
              }
            }
          }

          if (this.IsDuplicateShift) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror("Same selection of shift will not work", undefined, undefined);
          }
        }
      }, {
        key: "CheckDuplicateShitsOnSubmit",
        value: function CheckDuplicateShitsOnSubmit() {
          var checkDuplicate = false;
          var ScheduleList = this.CreateRegionForm.get('RegionShiftDetail').value;

          if (ScheduleList.length > 1) {
            for (var j = 0; j < ScheduleList.length; j++) {
              var shift = ScheduleList[j];
              var cnt = 1;

              for (var i = 0; i < ScheduleList.length; i++) {
                var iShift = ScheduleList[i];

                if (iShift.ShiftId != "" && shift.ShiftId != "" && iShift.ShiftId[0].Id == shift.ShiftId[0].Id) {
                  if (cnt > 1) {
                    return checkDuplicate = true;
                  }

                  cnt++;
                }
              }
            }
          }

          return checkDuplicate;
        }
      }, {
        key: "getShiftSchedules",
        value: function getShiftSchedules(regionId, routeId) {
          var _this7 = this;

          this.regionService.getRegionSchedule(regionId, routeId).subscribe(function (schedules) {
            _this7.pushRegionShiftDetail(schedules);
          });
        }
      }, {
        key: "pushRegionShiftDetail",
        value: function pushRegionShiftDetail(schedules) {
          this.ShiftScheduleList = schedules;
        }
      }, {
        key: "getRoutes",
        value: function getRoutes(id) {
          var _this8 = this;

          this.regionService.getRoutesByRegion(id).subscribe(function (routes) {
            _this8.getRouteDropDown(routes);
          });
        }
      }, {
        key: "getRouteDropDown",
        value: function getRouteDropDown(routeList) {
          this.RouteList = routeList.ResponseData;
        }
      }, {
        key: "getRegionDropDwn",
        value: function getRegionDropDwn(regionList) {
          this.regionList = regionList;
        }
      }]);

      return CreateRegionComponent;
    }();

    CreateRegionComponent.ɵfac = function CreateRegionComponent_Factory(t) {
      return new (t || CreateRegionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]));
    };

    CreateRegionComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: CreateRegionComponent,
      selectors: [["app-create-region"]],
      outputs: {
        OnScheduleAdded: "OnScheduleAdded"
      },
      decls: 71,
      vars: 23,
      consts: [["id", "CreateRoute", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "RegionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Route"], ["formControlName", "RouteId", 3, "placeholder", "settings", "data", "onSelect"], [1, "col-sm-6", 2, "padding-left", "90%"], ["href", "javascript:void(0)", 3, "click"], [1, "fa", "fa-plus"], ["formArrayName", "RegionShiftDetail", 4, "ngFor", "ngForOf"], ["for", "fromDate"], [1, "Required"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "disabled", "click"], [1, "color-maroon"], [4, "ngIf"], ["formArrayName", "RegionShiftDetail"], [3, "formGroupName"], ["for", "RegionShiftDetail"], ["formControlName", "ShiftId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [1, "col-sm-3"], ["for", "columns"], ["formControlName", "ColumnIndex", 3, "placeholder", "settings", "data"], [1, "col-sm-1"], ["href", "javascript:void(0)", 2, "padding-top", "50%", 3, "click"], [1, "fa", "fa-remove"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]],
      template: function CreateRegionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "button", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_Template_button_click_0_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Add Route ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "i", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "ng-sidebar-container", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "ng-sidebar", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_Template_a_click_6_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "h3", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Create Route");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "content", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("keydown.enter", function CreateRegionComponent_Template_content_keydown_enter_10_listener($event) {
            return $event.preventDefault();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onSelect_16_listener($event) {
            return ctx.onRegionSelect($event);
          })("onDeSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onDeSelect_16_listener($event) {
            return ctx.onRegionDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, CreateRegionComponent_div_17_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Route");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "ng-multiselect-dropdown", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateRegionComponent_Template_ng_multiselect_dropdown_onSelect_22_listener($event) {
            return ctx.onRouteSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, CreateRegionComponent_div_23_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "a", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_Template_a_click_26_listener() {
            return ctx.addShift();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "i", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, " Add Shift");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](29, CreateRegionComponent_div_29_Template, 24, 9, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "label", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "From Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "span", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "input", 22, 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateRegionComponent_Template_input_onDateChange_37_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](39, CreateRegionComponent_div_39_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](40, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "label", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](43, "To Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "span", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "input", 24, 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateRegionComponent_Template_input_onDateChange_46_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](48, CreateRegionComponent_div_48_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](53, "input", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "label", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](55, "Daily");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](57, "input", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "label", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](59, "WeekDays (Mon to Fri)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](61, "input", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "label", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](63, "Repeat Every");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](66, CreateRegionComponent_div_66_Template, 4, 0, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "input", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_Template_input_click_68_listener() {
            return ctx.closedSideBar();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "button", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateRegionComponent_Template_button_click_69_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](70, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.CreateRegionForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Region(s)")("settings", ctx.multiselectRegionSettingsById)("data", ctx.regionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("RegionId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Route(s)")("settings", ctx.multiselectRegionSettingsById)("data", ctx.RouteList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("RouteId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.CreateRegionForm.get("RegionShiftDetail")["controls"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("fromDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("toDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.CreateRegionForm.get("type").value == "3");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.IsDuplicateShift);
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_7__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_9__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_10__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]],
      styles: [".hide_chart[_ngcontent-%COMP%] {\r\n    display: none;\r\n}\r\n.required[_ngcontent-%COMP%] {\r\n    color: #e41813;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2NyZWF0ZS1yZWdpb24tc2NoZWR1bGUvY3JlYXRlLXJlZ2lvbi5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksYUFBYTtBQUNqQjtBQUNBO0lBQ0ksY0FBYztBQUNsQiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtcmVnaW9uLXNjaGVkdWxlL2NyZWF0ZS1yZWdpb24uY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5oaWRlX2NoYXJ0IHtcclxuICAgIGRpc3BsYXk6IG5vbmU7XHJcbn1cclxuLnJlcXVpcmVkIHtcclxuICAgIGNvbG9yOiAjZTQxODEzO1xyXG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CreateRegionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-create-region',
          templateUrl: './create-region.component.html',
          styleUrls: ['./create-region.component.css']
        }]
      }], function () {
        return [{
          type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }];
      }, {
        OnScheduleAdded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/driver/create-trailer-schedule/create-trailer-schedule.component.ts": function srcAppDriverCreateTrailerScheduleCreateTrailerScheduleComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateTrailerScheduleComponent", function () {
      return CreateTrailerScheduleComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_driver_models_TrailerSchedule__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/driver/models/TrailerSchedule */
    "./src/app/driver/models/TrailerSchedule.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _services_driver_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../services/driver.service */
    "./src/app/driver/services/driver.service.ts");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function CreateTrailerScheduleComponent_div_16_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateTrailerScheduleComponent_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateTrailerScheduleComponent_div_16_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r0.isRequired("regionId"));
      }
    }

    function CreateTrailerScheduleComponent_div_23_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateTrailerScheduleComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateTrailerScheduleComponent_div_23_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r1.isRequired("trailerId"));
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "is-invalid": a0
      };
    };

    function CreateTrailerScheduleComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        var _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Shift:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](7, "ng-multiselect-dropdown", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "label", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11, "Column:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](12, "ng-multiselect-dropdown", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "a", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_div_24_Template_a_click_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r15);

          var j_r13 = ctx.index;

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r14.removeShift(j_r13);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](15, "i", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var j_r13 = ctx.index;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroupName", j_r13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](9, _c0, ctx_r2.TrailerScheduleForm.get("shifts").touched && ctx_r2.TrailerScheduleForm.get("shifts").invalid))("placeholder", "Select Shifts")("settings", ctx_r2.multiselectDropDownSettingsById)("data", ctx_r2.ShiftList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](11, _c0, ctx_r2.TrailerScheduleForm.get("shifts").touched && ctx_r2.TrailerScheduleForm.get("shifts").invalid))("placeholder", "Select Columns")("settings", ctx_r2.multiselectDropDownSettingsById)("data", ctx_r2.ColumnList);
      }
    }

    function CreateTrailerScheduleComponent_div_43_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateTrailerScheduleComponent_div_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateTrailerScheduleComponent_div_43_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.isRequired("fromDate"));
      }
    }

    function CreateTrailerScheduleComponent_div_50_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateTrailerScheduleComponent_div_50_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, CreateTrailerScheduleComponent_div_50_div_1_Template, 2, 0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r6.isRequired("toDate"));
      }
    }

    function CreateTrailerScheduleComponent_div_66_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Custom");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function CreateTrailerScheduleComponent_div_69_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " Dates:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "ng-multiselect-dropdown", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Date (s)")("settings", ctx_r8.multiselectDateSettingsById)("data", ctx_r8.DateList);
      }
    }

    function CreateTrailerScheduleComponent_div_70_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Days:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](3, "input", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var CreateTrailerScheduleComponent = /*#__PURE__*/function () {
      function CreateTrailerScheduleComponent(regionService, driverService, _fb) {
        _classCallCheck(this, CreateTrailerScheduleComponent);

        this.regionService = regionService;
        this.driverService = driverService;
        this._fb = _fb;
        this.OnScheduleAdded = new _angular_core__WEBPACK_IMPORTED_MODULE_1__["EventEmitter"]();
        this.regionList = [];
        this.ShiftList = [];
        this.TrailerList = [];
        this.DriverList = [];
        this.ColumnList = [];
        this.isLoading = false;
        this.SelectedRegionId = ''; //sidebar variables

        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.DateList = []; //min max date

        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
      }

      _createClass(CreateTrailerScheduleComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.init();
        }
      }, {
        key: "init",
        value: function init() {
          this.getRegions();
          this.createTrailerForm();
          this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2);
          this.multiselectDropDownSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
          };
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.isLoading = false;
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
          }
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(name) {
          var result = this.TrailerScheduleForm.get(name).invalid && (this.TrailerScheduleForm.get(name).dirty || this.TrailerScheduleForm.get(name).touched);
          return result;
        }
      }, {
        key: "isRequired",
        value: function isRequired(name) {
          return this.TrailerScheduleForm.get(name).errors.required;
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this9 = this;

          this.regionService.getRegions().subscribe(function (region) {
            _this9.getRegionDropDwn(region.Regions);
          });
        }
      }, {
        key: "closedSideBar",
        value: function closedSideBar() {
          this._opened = false;
        }
      }, {
        key: "onRegionSelect",
        value: function onRegionSelect($event) {
          var region = this.regionList.find(function (f) {
            return f.Id == $event.Id;
          }); //this.TrailerScheduleForm.controls.shiftId.setValue('');

          this.ShiftList = region.Shifts.map(function (res) {
            return {
              Id: res.Id,
              Name: "".concat(res.StartTime, " - ").concat(res.EndTime)
            };
          });
          this.TrailerList = region.Trailers.map(function (res) {
            return {
              Id: res.Code,
              Name: "".concat(res.Name)
            };
          });
          this.SelectedRegionId = region.Id;
          this.createColumnList(region);
        }
      }, {
        key: "createColumnList",
        value: function createColumnList(region) {
          var _this10 = this;

          this.DriverList = region.Drivers;

          if (region.Drivers.length > 0) {
            var num = 1;
            this.DriverList.forEach(function (obj) {
              var col = {
                Id: 0,
                Name: ""
              };
              col.Id = num;
              col.Name = "C" + num;

              _this10.ColumnList.push(col);

              num++;
            });
          }
        }
      }, {
        key: "getRegionDropDwn",
        value: function getRegionDropDwn(regionList) {
          this.regionList = regionList;
        }
      }, {
        key: "onRegionDeSelect",
        value: function onRegionDeSelect($event) {//var region = this.regionList.find(f => f.Id == $event.Id);
          //this.ShiftList = [];
          //this.TrailerScheduleForm.controls.trailerId.setValue('');
        }
      }, {
        key: "createTrailerForm",
        value: function createTrailerForm() {
          this.TrailerScheduleForm = this._fb.group({
            id: this._fb.control(null),
            regionId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            trailerId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            shifts: this._fb.array([this.getShift()]),
            fromDate: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            toDate: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            //type: this._fb.control(''),
            type: ['1', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required],
            repeat: [1],
            customDates: [[]]
          });
          var dt = moment__WEBPACK_IMPORTED_MODULE_3__(new Date()).toDate();
          this.TrailerScheduleForm.controls.fromDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'));
        }
      }, {
        key: "addShift",
        value: function addShift() {
          var _shifts = this.TrailerScheduleForm.get('shifts');

          _shifts.push(this.getShift());
        }
      }, {
        key: "getShift",
        value: function getShift() {
          return this._fb.group({
            shiftId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            columnId: this._fb.control('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required)
          });
        }
      }, {
        key: "removeShift",
        value: function removeShift(index) {
          var _shifts = this.TrailerScheduleForm.get('shifts');

          _shifts.removeAt(index);
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.TrailerScheduleForm.controls.fromDate.setValue(event); //let d = moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();

          var d = moment__WEBPACK_IMPORTED_MODULE_3__(new Date(new Date().setMonth(new Date().getMonth() + 2))).toDate();
          !this.TrailerScheduleForm.controls.toDate.value ? this.TrailerScheduleForm.controls.toDate.setValue(moment__WEBPACK_IMPORTED_MODULE_3__(d).format('MM/DD/YYYY')) : '';

          if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.fromDate.value).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.toDate.value).toDate();

            if (_toDate < _fromDate) {
              this.TrailerScheduleForm.controls.toDate.setValue(event);
            }
          }

          this.InitializeDates();
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee11() {
            var _fromDate, _toDate;

            return regeneratorRuntime.wrap(function _callee11$(_context11) {
              while (1) {
                switch (_context11.prev = _context11.next) {
                  case 0:
                    this.TrailerScheduleForm.controls.toDate.setValue(event);

                    if (this.TrailerScheduleForm.controls.fromDate.value != '' && this.TrailerScheduleForm.controls.toDate.value != '') {
                      _fromDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.fromDate.value).toDate();
                      _toDate = moment__WEBPACK_IMPORTED_MODULE_3__(this.TrailerScheduleForm.controls.toDate.value).toDate();

                      if (_fromDate > _toDate) {
                        this.TrailerScheduleForm.controls.fromDate.setValue(event);
                      }
                    }

                    this.InitializeDates();

                  case 3:
                  case "end":
                    return _context11.stop();
                }
              }
            }, _callee11, this);
          }));
        }
      }, {
        key: "InitializeDates",
        value: function InitializeDates(type, repeat) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee12() {
            var days, dt;
            return regeneratorRuntime.wrap(function _callee12$(_context12) {
              while (1) {
                switch (_context12.prev = _context12.next) {
                  case 0:
                    this.TrailerScheduleForm.controls.customDates.setValue([]);
                    this.DateList = [];
                    !repeat ? repeat = 0 : '';
                    days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

                    if (!(this.TrailerScheduleForm.controls.fromDate.value && this.TrailerScheduleForm.controls.toDate.value)) {
                      _context12.next = 7;
                      break;
                    }

                    for (dt = new Date(this.TrailerScheduleForm.controls.fromDate.value); dt <= new Date(this.TrailerScheduleForm.controls.toDate.value); dt.setDate(dt.getDate() + repeat + 1)) {
                      if (type && type == 2) //weekend
                        new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6 ? this.DateList.push({
                          Id: new Date(dt),
                          Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                        }) : '';else this.DateList.push({
                        Id: new Date(dt),
                        Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_3__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                      });
                    }

                    return _context12.abrupt("return", this.DateList);

                  case 7:
                  case "end":
                    return _context12.stop();
                }
              }
            }, _callee12, this);
          }));
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee13() {
            var dates, model;
            return regeneratorRuntime.wrap(function _callee13$(_context13) {
              while (1) {
                switch (_context13.prev = _context13.next) {
                  case 0:
                    if (!this.TrailerScheduleForm.invalid) {
                      _context13.next = 5;
                      break;
                    }

                    this.TrailerScheduleForm.markAllAsTouched();
                    return _context13.abrupt("return", false);

                  case 5:
                    if (!(this.TrailerScheduleForm.controls.type.value == '3')) {
                      _context13.next = 11;
                      break;
                    }

                    if (this.TrailerScheduleForm.controls.repeat.value && this.TrailerScheduleForm.controls.repeat.value > 0) {
                      _context13.next = 9;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Repeat field is greater than 0', undefined, undefined);
                    return _context13.abrupt("return", false);

                  case 9:
                    _context13.next = 15;
                    break;

                  case 11:
                    if (!(this.TrailerScheduleForm.controls.type.value == '4')) {
                      _context13.next = 15;
                      break;
                    }

                    if (this.TrailerScheduleForm.controls.customDates.value.length > 0) {
                      _context13.next = 15;
                      break;
                    }

                    src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Select custom dates', undefined, undefined);
                    return _context13.abrupt("return", false);

                  case 15:
                    if (!(this.TrailerScheduleForm.controls.type.value == '2')) {
                      _context13.next = 23;
                      break;
                    }

                    _context13.t0 = this.TrailerScheduleForm.controls.customDates;
                    _context13.next = 19;
                    return this.InitializeDates(this.TrailerScheduleForm.controls.type.value);

                  case 19:
                    _context13.t1 = _context13.sent;

                    _context13.t0.setValue.call(_context13.t0, _context13.t1);

                    _context13.next = 37;
                    break;

                  case 23:
                    if (!(this.TrailerScheduleForm.controls.type.value == '3')) {
                      _context13.next = 31;
                      break;
                    }

                    _context13.t2 = this.TrailerScheduleForm.controls.customDates;
                    _context13.next = 27;
                    return this.InitializeDates(this.TrailerScheduleForm.controls.type.value, this.TrailerScheduleForm.controls.repeat.value);

                  case 27:
                    _context13.t3 = _context13.sent;

                    _context13.t2.setValue.call(_context13.t2, _context13.t3);

                    _context13.next = 37;
                    break;

                  case 31:
                    if (!(this.TrailerScheduleForm.controls.type.value == '1')) {
                      _context13.next = 37;
                      break;
                    }

                    _context13.t4 = this.TrailerScheduleForm.controls.customDates;
                    _context13.next = 35;
                    return this.InitializeDates();

                  case 35:
                    _context13.t5 = _context13.sent;

                    _context13.t4.setValue.call(_context13.t4, _context13.t5);

                  case 37:
                    dates = [];
                    _context13.next = 40;
                    return this.TrailerScheduleForm.controls.customDates.value.map(function (res) {
                      dates.push(res.Id);
                    });

                  case 40:
                    model = new src_app_driver_models_TrailerSchedule__WEBPACK_IMPORTED_MODULE_5__["TrailerSchedule"]();
                    this.TrailerScheduleForm.controls.id.value ? model.Id = this.TrailerScheduleForm.controls.id.value : '';
                    model.RegionId = this.TrailerScheduleForm.controls.regionId.value[0].Id;
                    model.TrailerId = this.TrailerScheduleForm.controls.trailerId.value[0].Id;
                    model.StartDate = this.TrailerScheduleForm.controls.fromDate.value;
                    model.EndDate = this.TrailerScheduleForm.controls.toDate.value;
                    model.IsActive = true;
                    model.RepeatDayList = dates;
                    model.Type = this.TrailerScheduleForm.controls.type.value;
                    model.TrailerShiftDetail = this.TrailerScheduleForm.controls.shifts.value.map(function (item) {
                      return {
                        ShiftId: item.shiftId[0].Id,
                        ColumnId: item.columnId[0].Id
                      };
                    });
                    this.addTrailerSchedule(model);

                  case 51:
                  case "end":
                    return _context13.stop();
                }
              }
            }, _callee13, this);
          }));
        }
      }, {
        key: "addTrailerSchedule",
        value: function addTrailerSchedule(model) {
          var _this11 = this;

          this.isLoading = true;
          this.driverService.onLoadingChanged.next(true);
          this.driverService.addTrailerSchedule(model).subscribe(function (response) {
            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess('Trailer Schedule created successfully', undefined, undefined);
              _this11.isLoading = false;

              _this11.driverService.onLoadingChanged.next(false);

              _this11._toggleOpened(false); //let driver = this.TrailerScheduleForm.controls.driverId.value;


              _this11.TrailerScheduleForm.reset(); //this.OnScheduleAdded.emit(driver);

            } else {
              _this11.isLoading = false;

              _this11.driverService.onLoadingChanged.next(false);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }]);

      return CreateTrailerScheduleComponent;
    }();

    CreateTrailerScheduleComponent.ɵfac = function CreateTrailerScheduleComponent_Factory(t) {
      return new (t || CreateTrailerScheduleComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]));
    };

    CreateTrailerScheduleComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: CreateTrailerScheduleComponent,
      selectors: [["app-create-trailer-schedule"]],
      outputs: {
        OnScheduleAdded: "OnScheduleAdded"
      },
      decls: 75,
      vars: 24,
      consts: [["id", "trailerSchedule", "type", "button", 1, "btn", "btn-default", "float-right", 3, "click"], ["aria-hidden", "true", 1, "fa", "fa-plus", "mr5"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["name", "TrailerScheduleForm", "autocomplete", "off", 1, "pr30", 3, "formGroup", "keydown.enter"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Region"], ["formControlName", "regionId", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["class", "color-maroon", 4, "ngIf"], ["for", "Trailer"], ["formControlName", "trailerId", 3, "placeholder", "settings", "data"], ["class", "row", "formArrayName", "shifts", 4, "ngFor", "ngForOf"], [1, "col-sm-12"], ["width", "100%"], ["width", "15%"], ["colspan", "2"], [1, "fa", "fa-plus-circle", "fs14"], ["for", "fromDate"], ["type", "text", "formControlName", "fromDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["fromDate", ""], ["type", "text", "formControlName", "toDate", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["EndDate", ""], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "type", "formControlName", "type", "value", "1", "id", "inlineRadioDaily", 1, "form-check-input"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "2", "id", "inlineRadioWdays", 1, "form-check-input"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "type", "formControlName", "type", "value", "3", "id", "inlineRadioEvery", 1, "form-check-input"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["class", "form-group", 4, "ngIf"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"], [1, "color-maroon"], [4, "ngIf"], ["formArrayName", "shifts", 1, "row"], [1, "row", 3, "formGroupName"], [1, "col-sm-5"], ["for", "Shift"], ["formControlName", "shiftId", 3, "ngClass", "placeholder", "settings", "data"], ["for", "Column"], ["formControlName", "columnId", 3, "ngClass", "placeholder", "settings", "data"], [1, "col-sm-2", "text-right"], [1, "ml20", 3, "click"], [1, "fa", "fa-trash-alt", "mt14", "color-maroon", "mt8"], ["type", "radio", "name", "type", "formControlName", "type", "value", "4", "id", "inlineRadioCustom", 1, "form-check-input"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], ["formControlName", "customDates", 3, "placeholder", "settings", "data"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", "formControlName", "repeat", 1, "form-control"]],
      template: function CreateTrailerScheduleComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "button", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_Template_button_click_0_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "i", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Add Trailer");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "ng-sidebar-container", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "ng-sidebar", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("openedChange", function CreateTrailerScheduleComponent_Template_ng_sidebar_openedChange_4_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_Template_a_click_5_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "h3", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Schedule Trailer ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "content", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("keydown.enter", function CreateTrailerScheduleComponent_Template_content_keydown_enter_9_listener($event) {
            return $event.preventDefault();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Region:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function CreateTrailerScheduleComponent_Template_ng_multiselect_dropdown_onSelect_15_listener($event) {
            return ctx.onRegionSelect($event);
          })("onDeSelect", function CreateTrailerScheduleComponent_Template_ng_multiselect_dropdown_onDeSelect_15_listener($event) {
            return ctx.onRegionDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, CreateTrailerScheduleComponent_div_16_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Trailer:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](22, "ng-multiselect-dropdown", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, CreateTrailerScheduleComponent_div_23_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, CreateTrailerScheduleComponent_div_24_Template, 16, 13, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "table", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "td", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "\xA0");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_Template_a_click_33_listener() {
            return ctx.addShift();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](34, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, " Add Shift ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "label", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "From Date:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "input", 23, 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateTrailerScheduleComponent_Template_input_onDateChange_41_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](43, CreateTrailerScheduleComponent_div_43_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "label", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](47, "To Date:");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "input", 25, 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function CreateTrailerScheduleComponent_Template_input_onDateChange_48_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](50, CreateTrailerScheduleComponent_div_50_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](55, "input", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "label", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](57, "Daily");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](59, "input", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](60, "label", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](61, "WeekDays (Mon to Fri)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](63, "input", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "label", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](65, "Repeat Every");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](66, CreateTrailerScheduleComponent_div_66_Template, 4, 0, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](69, CreateTrailerScheduleComponent_div_69_Template, 4, 3, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](70, CreateTrailerScheduleComponent_div_70_Template, 4, 0, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](72, "input", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_Template_input_click_72_listener() {
            return ctx.closedSideBar();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "button", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function CreateTrailerScheduleComponent_Template_button_click_73_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](74, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.TrailerScheduleForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Region(s)")("settings", ctx.multiselectDropDownSettingsById)("data", ctx.regionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("regionId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Trailer(s)")("settings", ctx.multiselectDropDownSettingsById)("data", ctx.TrailerList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("trailerId"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.TrailerScheduleForm.get("shifts")["controls"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("fromDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinStartDate)("maxDate", ctx.MaxStartDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isInvalid("toDate"));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.TrailerScheduleForm.get("fromDate").value && ctx.TrailerScheduleForm.get("toDate").value);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.TrailerScheduleForm.get("type").value == "4");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.TrailerScheduleForm.get("type").value == "3");
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_8__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_9__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_11__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NumberValueAccessor"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9jcmVhdGUtdHJhaWxlci1zY2hlZHVsZS9jcmVhdGUtdHJhaWxlci1zY2hlZHVsZS5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](CreateTrailerScheduleComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-create-trailer-schedule',
          templateUrl: './create-trailer-schedule.component.html',
          styleUrls: ['./create-trailer-schedule.component.css']
        }]
      }], function () {
        return [{
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_6__["RegionService"]
        }, {
          type: _services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }];
      }, {
        OnScheduleAdded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver-management/create-driver/create-driver.component.ts": function srcAppDriverDriverManagementCreateDriverCreateDriverComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateDriverComponent", function () {
      return CreateDriverComponent;
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


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _services_driver_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../services/driver.service */
    "./src/app/driver/services/driver.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");

    function CreateDriverComponent_div_14_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_14_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.DriverForm.get("FirstName").errors.required);
      }
    }

    function CreateDriverComponent_div_22_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_22_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.DriverForm.get("LastName").errors.required);
      }
    }

    function CreateDriverComponent_div_36_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_36_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid email. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_36_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_36_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateDriverComponent_div_36_div_2_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.DriverForm.get("Email").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.DriverForm.get("Email").errors.pattern);
      }
    }

    function CreateDriverComponent_div_43_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx_r13.ContactNumberValidationMessage, " ");
      }
    }

    function CreateDriverComponent_div_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_43_div_1_Template, 2, 1, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r3.DriverForm.get("ContactNumber").valid || (ctx_r3.DriverForm.get("ContactNumber").errors == null ? null : ctx_r3.DriverForm.get("ContactNumber").errors.pattern));
      }
    }

    function CreateDriverComponent_div_51_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_51_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_51_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.DriverForm.get("ExpiryDate").errors.required);
      }
    }

    function CreateDriverComponent_div_60_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_60_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_60_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.DriverForm.get("LicenseNumber").errors.required);
      }
    }

    function CreateDriverComponent_option_70_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var key_r16 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", key_r16.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](key_r16.Name);
      }
    }

    function CreateDriverComponent_div_71_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateDriverComponent_div_71_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateDriverComponent_div_71_div_1_Template, 2, 0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r7.DriverForm.get("SelectedLicenseTypes").errors.required);
      }
    }

    function CreateDriverComponent_div_95_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    var CreateDriverComponent = /*#__PURE__*/function () {
      function CreateDriverComponent(fb, driverService) {
        _classCallCheck(this, CreateDriverComponent);

        this.fb = fb;
        this.driverService = driverService;
        this.MinDate = new Date();
        this.MaxDate = new Date();
        this.TrailerTypeEnum = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["TrailerType"];
        this.TrailerTypeList = [];
        this.LicenceTypeEnum = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["LicenceRequirement"];
        this.LicenceTypes = []; //public DriverStatusEnum: typeof Status = Status;
        //public Statuses: DropdownItem[] = [];

        this.RegionList = [];
        this.ContactNumberValidationMessage = "Invalid Contact number";
        this.IsOnboarded = false;
        this.IsLoading = false;
        this.trailerDdlSettings = {};
        this.regionDdlSettings = {};
        this.onSaveDriverData = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.DriverForm = this.fb.group({
          DriverId: this.fb.control(0),
          FirstName: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          LastName: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          CompanyName: this.fb.control(''),
          Email: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]),
          LicenseNumber: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          ExpiryDate: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          LicenseTypeId: this.fb.control(null),
          SelectedLicenseTypes: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          TrailerType: this.fb.control(null),
          SelectedTrailerTypes: this.fb.control(null),
          ContactNumber: this.fb.control(''),
          //DriverStatus: this.fb.control(Status.Active),
          InvitedBy: this.fb.control(''),
          UserId: this.fb.control(0),
          //IsActive: this.fb.control(true),
          Regions: this.fb.control(null),
          SelectedRegions: this.fb.control(null),
          IsFilldAuthorized: this.fb.control(false)
        });
      }

      _createClass(CreateDriverComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var _this12 = this;

          this.trailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.regionDdlSettings = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
          }; // load regions

          this.getRegions();
          this.TrailerTypeList = Object.keys(this.TrailerTypeEnum).filter(function (k) {
            return typeof _this12.TrailerTypeEnum[k] === "number";
          }).map(function (x) {
            return {
              Id: _this12.TrailerTypeEnum[x],
              Name: x,
              Code: ""
            };
          });
          this.LicenceTypes = Object.keys(this.LicenceTypeEnum).filter(function (k) {
            return typeof _this12.LicenceTypeEnum[k] === "number";
          }).map(function (x) {
            return {
              Id: x,
              Name: x == "Class1" ? "Class 1" : x == "Class3" ? "Class 3" : x,
              Code: ""
            };
          }); //this.Statuses = (Object.keys(this.DriverStatusEnum).filter(k => typeof this.DriverStatusEnum[k] === "number") as string[]).map(x => { return { Id: this.DriverStatusEnum[x], Name: x == "InActive" ? "In-Active" : x, Code: "" } as DropdownItem });

          this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var _this13 = this;

          // validate contact number     
          var contactNumber = this.DriverForm.get('ContactNumber').value;
          var licenseType = this.DriverForm.get('SelectedLicenseTypes').value;
          this.formatContactNumber(contactNumber);
          this.onLicenseTypeChange(licenseType);

          if (this.DriverForm.valid) {
            var driverForm = this.DriverForm.value;
            var driverId = driverForm.DriverId == "" || driverForm.DriverId == null ? 0 : driverForm.DriverId;
            this.DriverForm.get("DriverId").patchValue(driverId);
            var userId = driverForm.UserId == "" || driverForm.UserId == null ? 0 : driverForm.UserId;
            this.DriverForm.get("UserId").patchValue(userId);

            if (driverForm.SelectedTrailerTypes != null && driverForm.SelectedTrailerTypes.length > 0) {
              var trailerTypeIds = [];
              driverForm.SelectedTrailerTypes.forEach(function (t) {
                trailerTypeIds.push(_this13.TrailerTypeEnum[t.Name]);
              });
              this.DriverForm.get("TrailerType").patchValue(trailerTypeIds);
            }

            if (driverForm.SelectedRegions != null && driverForm.SelectedRegions.length > 0) {
              var regionIds = [];
              driverForm.SelectedRegions.forEach(function (t) {
                regionIds.push(t.Code);
              });
              this.DriverForm.get("Regions").patchValue(regionIds);
            }

            if (driverForm.SelectedLicenseTypes != null) {
              this.DriverForm.get("LicenseTypeId").patchValue(driverForm.SelectedLicenseTypes);
            } //if (driverForm.IsActive == undefined || driverForm.IsActive == null) {
            //    var isActive = driverForm.DriverStatus == 1 ? true : false;
            //    this.DriverForm.get("IsActive").patchValue(isActive);
            //}


            if (!driverForm.IsFilldAuthorized) {
              this.DriverForm.get("IsFilldAuthorized").patchValue(false);
            }

            this.submitForm();
          } else {
            this.DriverForm.markAllAsTouched();
          }
        }
      }, {
        key: "loadDriverDetail",
        value: function loadDriverDetail(driver) {
          this.clearForm();

          if (driver.ContactNumber == 'NA') {
            driver.ContactNumber = null;
          }

          this.DriverForm.patchValue(driver);

          if (driver.UserId && driver.UserId > 0) {
            this.DriverForm.get("FirstName").disable();
            this.DriverForm.get("LastName").disable();
            this.DriverForm.get("Email").disable();
          }

          if (driver.TrailerType != null) {
            var trailerTypesToBind = this.TrailerTypeList.filter(function (t) {
              return driver.TrailerType.indexOf(t.Id) != -1;
            });
            this.DriverForm.controls.SelectedTrailerTypes.setValue(trailerTypesToBind);
          }

          if (driver.Regions != null) {
            var regionsToBind = this.RegionList.filter(function (t) {
              return driver.Regions.indexOf(t.Code) != -1;
            });
            this.DriverForm.controls.SelectedRegions.setValue(regionsToBind);
          }

          if (driver.LicenseTypeId != null) this.DriverForm.controls.SelectedLicenseTypes.setValue(driver.LicenseTypeId);
        }
      }, {
        key: "submitForm",
        value: function submitForm() {
          var _this14 = this;

          this.IsLoading = true;
          this.driverService.postAddDriver(this.DriverForm.getRawValue()).subscribe(function (data) {
            _this14.IsLoading = false;

            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this14.clearForm();

              _this14.onSaveDriverData.emit();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this15 = this;

          this.IsLoading = true;
          this.driverService.getRegions().subscribe(function (data) {
            _this15.IsLoading = false;
            _this15.RegionList = data;
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.DriverForm.reset();
          this.setDefaultValue();
          this.DriverForm.get("FirstName").enable();
          this.DriverForm.get("LastName").enable();
          this.DriverForm.get("Email").enable();
        }
      }, {
        key: "setDefaultValue",
        value: function setDefaultValue() {
          // this.DriverForm.get("DriverStatus").patchValue(Status.Active);
          this.DriverForm.get("Regions").patchValue(null);
          this.DriverForm.get("TrailerType").patchValue(null);
          this.IsOnboarded = false;
        }
      }, {
        key: "setSelectedDate",
        value: function setSelectedDate(date) {
          var _date = this.DriverForm.get('ExpiryDate');

          if (_date.value != date) {
            _date.setValue(date);
          }
        }
      }, {
        key: "onLicenseTypeChange",
        value: function onLicenseTypeChange(licenseType) {
          if (licenseType == null || licenseType == "null") this.DriverForm.controls['SelectedLicenseTypes'].setErrors({
            'required': true
          });else this.DriverForm.controls['SelectedLicenseTypes'].setErrors(null);
        }
      }, {
        key: "formatContactNumber",
        value: function formatContactNumber(contactNumber) {
          if (contactNumber != null && contactNumber != '') {
            contactNumber = contactNumber.split('-').join("");

            if (contactNumber.length == 10) {
              contactNumber = contactNumber.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
              this.DriverForm.controls['ContactNumber'].setErrors(null);
              this.DriverForm.get("ContactNumber").patchValue(contactNumber);
            } else {
              this.DriverForm.controls['ContactNumber'].setErrors({
                'incorrect': true
              });
            }
          } else {
            this.DriverForm.controls['ContactNumber'].setErrors(null);
          }
        }
      }]);

      return CreateDriverComponent;
    }();

    CreateDriverComponent.ɵfac = function CreateDriverComponent_Factory(t) {
      return new (t || CreateDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_4__["DriverService"]));
    };

    CreateDriverComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CreateDriverComponent,
      selectors: [["app-create-driver"]],
      outputs: {
        onSaveDriverData: "onSaveDriverData"
      },
      decls: 96,
      vars: 23,
      consts: [[3, "formGroup", "ngSubmit"], [1, "sidePanel_overflow"], ["id", "driver-Form", 1, "col-sm-12"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["type", "hidden", "formControlName", "DriverId"], ["type", "hidden", "formControlName", "InvitedBy"], ["type", "hidden", "formControlName", "UserId"], [1, "color-maroon"], ["type", "text", "formControlName", "FirstName", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], ["type", "text", "formControlName", "LastName", 1, "form-control"], ["type", "text", "formControlName", "CompanyName", 1, "form-control"], ["type", "text", "formControlName", "Email", 1, "form-control", 3, "ngClass"], ["type", "text", "formControlName", "ContactNumber", 1, "form-control", 3, "change"], ["type", "text", "formControlName", "ExpiryDate", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "minDate", "maxDate", "onDateChange"], ["type", "text", "formControlName", "LicenseNumber", 1, "form-control"], ["formControlName", "SelectedLicenseTypes", 1, "form-control"], [3, "value"], [3, "value", 4, "ngFor", "ngForOf"], ["formControlName", "SelectedTrailerTypes", 3, "placeholder", "settings", "data"], ["formControlName", "SelectedRegions", 3, "placeholder", "settings", "data"], [1, "form-check", "form-check-inline"], ["type", "checkbox", "id", "IsFilldAuthorized", "formControlName", "IsFilldAuthorized", 1, "form-check-input"], ["for", "IsFilldAuthorized", 1, "form-check-label"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["id", "submit-driver-form", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CreateDriverComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function CreateDriverComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "First Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, CreateDriverComponent_div_14_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Last Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](21, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, CreateDriverComponent_div_22_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](28, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "E-mail");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](35, "input", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](36, CreateDriverComponent_div_36_Template, 3, 2, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Contact Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "input", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateDriverComponent_Template_input_change_42_listener($event) {
            return ctx.formatContactNumber($event.target.value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](43, CreateDriverComponent_div_43_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](47, "Licence Expiry Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](49, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "input", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function CreateDriverComponent_Template_input_onDateChange_50_listener($event) {
            return ctx.setSelectedDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](51, CreateDriverComponent_div_51_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](56, "Licence Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](57, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](58, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](59, "input", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](60, CreateDriverComponent_div_60_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](64, "Licence Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](66, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "select", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "option", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](69, "Select");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](70, CreateDriverComponent_option_70_Template, 2, 2, "option", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](71, CreateDriverComponent_div_71_Template, 2, 1, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](74, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](75, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](76, "Trailer Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](77, "ng-multiselect-dropdown", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](78, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](79, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](80, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](81, "Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](82, "ng-multiselect-dropdown", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](83, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](84, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](85, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](86, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](87, "input", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](88, "label", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](89, "TrueFill Inc. Compatible");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](91, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](92, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateDriverComponent_Template_input_click_92_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](93, "button", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](94, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](95, CreateDriverComponent_div_95_Template, 5, 0, "div", 29);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.DriverForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("FirstName").invalid && ctx.DriverForm.get("FirstName").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("LastName").invalid && ctx.DriverForm.get("LastName").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](21, _c0, ctx.IsOnboarded));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("Email").invalid && ctx.DriverForm.get("Email").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("ContactNumber").invalid && ctx.DriverForm.get("ContactNumber").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY")("minDate", ctx.MinDate)("maxDate", ctx.MaxDate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("ExpiryDate").invalid && ctx.DriverForm.get("ExpiryDate").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("LicenseNumber").invalid && ctx.DriverForm.get("LicenseNumber").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.LicenceTypes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.DriverForm.get("SelectedLicenseTypes").invalid && ctx.DriverForm.get("SelectedLicenseTypes").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Type")("settings", ctx.trailerDdlSettings)("data", ctx.TrailerTypeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Region")("settings", ctx.regionDdlSettings)("data", ctx.RegionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgClass"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_6__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC9jcmVhdGUtZHJpdmVyL2NyZWF0ZS1kcml2ZXIuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-create-driver',
          templateUrl: './create-driver.component.html',
          styleUrls: ['./create-driver.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _services_driver_service__WEBPACK_IMPORTED_MODULE_4__["DriverService"]
        }];
      }, {
        onSaveDriverData: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver-management/driver-management.component.ts": function srcAppDriverDriverManagementDriverManagementComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverManagementComponent", function () {
      return DriverManagementComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./view-driver/view-driver.component */
    "./src/app/driver/driver-management/view-driver/view-driver.component.ts");

    var DriverManagementComponent = /*#__PURE__*/function () {
      function DriverManagementComponent() {
        _classCallCheck(this, DriverManagementComponent);
      }

      _createClass(DriverManagementComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }]);

      return DriverManagementComponent;
    }();

    DriverManagementComponent.ɵfac = function DriverManagementComponent_Factory(t) {
      return new (t || DriverManagementComponent)();
    };

    DriverManagementComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: DriverManagementComponent,
      selectors: [["app-driver-management"]],
      decls: 3,
      vars: 0,
      consts: [["viewDriver", ""]],
      template: function DriverManagementComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-view-driver", null, 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }
      },
      directives: [_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_1__["ViewDriverComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC9kcml2ZXItbWFuYWdlbWVudC5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DriverManagementComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-driver-management',
          templateUrl: './driver-management.component.html',
          styleUrls: ['./driver-management.component.css']
        }]
      }], function () {
        return [];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver-management/view-driver/view-driver.component.ts": function srcAppDriverDriverManagementViewDriverViewDriverComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ViewDriverComponent", function () {
      return ViewDriverComponent;
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


    var _create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../create-driver/create-driver.component */
    "./src/app/driver/driver-management/create-driver/create-driver.component.ts");
    /* harmony import */


    var _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../models/DriverManagementModel */
    "./src/app/driver/models/DriverManagementModel.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_6__);
    /* harmony import */


    var _services_driver_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../services/driver.service */
    "./src/app/driver/services/driver.service.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    function ViewDriverComponent_tr_40_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.ContactNumber);
      }
    }

    function ViewDriverComponent_tr_40_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.CompanyName);
      }
    }

    function ViewDriverComponent_tr_40_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.LicenseNumber);
      }
    }

    function ViewDriverComponent_tr_40_span_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.DisplayLicenseType);
      }
    }

    function ViewDriverComponent_tr_40_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.ExpiryDate);
      }
    }

    function ViewDriverComponent_tr_40_span_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.DisplayTrailerTypes);
      }
    }

    function ViewDriverComponent_tr_40_span_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_span_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.IsFilldAuthorized ? "Yes" : "No");
      }
    }

    function ViewDriverComponent_tr_40_span_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "No");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "a", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_tr_40_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var driver_r7 = ctx.$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          ctx_r29.setPanelHeader("Edit Driver");
          return ctx_r29.editDriver(driver_r7, false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, ViewDriverComponent_tr_40_span_8_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, ViewDriverComponent_tr_40_span_9_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, ViewDriverComponent_tr_40_span_11_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, ViewDriverComponent_tr_40_span_12_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](14, ViewDriverComponent_tr_40_span_14_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, ViewDriverComponent_tr_40_span_15_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, ViewDriverComponent_tr_40_span_17_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, ViewDriverComponent_tr_40_span_18_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, ViewDriverComponent_tr_40_span_20_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, ViewDriverComponent_tr_40_span_21_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, ViewDriverComponent_tr_40_span_23_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, ViewDriverComponent_tr_40_span_24_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](26, ViewDriverComponent_tr_40_span_26_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, ViewDriverComponent_tr_40_span_27_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "td", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "button", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_tr_40_Template_button_click_29_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var driver_r7 = ctx.$implicit;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r31.showDriverShifts(driver_r7);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](30, "i", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "button", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_tr_40_Template_button_click_32_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r30);

          var driver_r7 = ctx.$implicit;

          var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r32.setdeleteDriver(driver_r7);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](33, "i", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r7 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", driver_r7.FirstName, " ", driver_r7.LastName, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r7.Email);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.ContactNumber != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.ContactNumber == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.CompanyName != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.CompanyName == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.LicenseNumber != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.LicenseNumber == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.DisplayLicenseType != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.DisplayLicenseType == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.ExpiryDate != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.ExpiryDate == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.DisplayTrailerTypes != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.DisplayTrailerTypes == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.IsFilldAuthorized != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r7.IsFilldAuthorized == null);
      }
    }

    function ViewDriverComponent_tr_76_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.ContactNumber);
      }
    }

    function ViewDriverComponent_tr_76_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.CompanyName);
      }
    }

    function ViewDriverComponent_tr_76_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.LicenseNumber);
      }
    }

    function ViewDriverComponent_tr_76_span_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.DisplayLicenseType);
      }
    }

    function ViewDriverComponent_tr_76_span_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.ExpiryDate);
      }
    }

    function ViewDriverComponent_tr_76_span_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.DisplayTrailerTypes);
      }
    }

    function ViewDriverComponent_tr_76_span_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_span_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.IsFilldAuthorized ? "Yes" : "No");
      }
    }

    function ViewDriverComponent_tr_76_span_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "No");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_tr_76_Template(rf, ctx) {
      if (rf & 1) {
        var _r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "a", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_tr_76_Template_a_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r56);

          var driver_r33 = ctx.$implicit;

          var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          ctx_r55.setPanelHeader("Edit Driver");
          return ctx_r55.editDriver(driver_r33, true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, ViewDriverComponent_tr_76_span_8_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, ViewDriverComponent_tr_76_span_9_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, ViewDriverComponent_tr_76_span_11_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, ViewDriverComponent_tr_76_span_12_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](14, ViewDriverComponent_tr_76_span_14_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, ViewDriverComponent_tr_76_span_15_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, ViewDriverComponent_tr_76_span_17_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](18, ViewDriverComponent_tr_76_span_18_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, ViewDriverComponent_tr_76_span_20_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, ViewDriverComponent_tr_76_span_21_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](23, ViewDriverComponent_tr_76_span_23_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](24, ViewDriverComponent_tr_76_span_24_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](26, ViewDriverComponent_tr_76_span_26_Template, 2, 1, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, ViewDriverComponent_tr_76_span_27_Template, 2, 0, "span", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "td", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "button", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_tr_76_Template_button_click_29_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r56);

          var driver_r33 = ctx.$implicit;

          var ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r57.showDriverShifts(driver_r33);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](30, "i", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "td", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "input", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("confirm", function ViewDriverComponent_tr_76_Template_input_confirm_32_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r56);

          var driver_r33 = ctx.$implicit;

          var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r58.changeDriverStatus(driver_r33);
        })("ngModelChange", function ViewDriverComponent_tr_76_Template_input_ngModelChange_32_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r56);

          var driver_r33 = ctx.$implicit;
          return driver_r33.IsActive = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var driver_r33 = ctx.$implicit;

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", driver_r33.FirstName, " ", driver_r33.LastName, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](driver_r33.Email);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.ContactNumber != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.ContactNumber == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.CompanyName != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.CompanyName == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.LicenseNumber != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.LicenseNumber == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.DisplayLicenseType != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.DisplayLicenseType == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.ExpiryDate != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.ExpiryDate == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.DisplayTrailerTypes != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.DisplayTrailerTypes == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.IsFilldAuthorized != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", driver_r33.IsFilldAuthorized == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", driver_r33.IsActive)("popoverTitle", ctx_r1.popoverTitle)("confirmText", ctx_r1.confirmButtonText)("cancelText", ctx_r1.cancelButtonText);
      }
    }

    function ViewDriverComponent_div_87_tr_19_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var shift_r62 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", shift_r62.ShiftFrom, " - ", shift_r62.ShiftTo, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](shift_r62.FromDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](shift_r62.ToDate);
      }
    }

    function ViewDriverComponent_div_87_tr_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "No schedule found");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_div_87_Template(rf, ctx) {
      if (rf & 1) {
        var _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "h4", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "button", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_div_87_Template_button_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r63.closeDriverShiftsPopup();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "\xD7");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "table", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Shift");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "From Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "To Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](19, ViewDriverComponent_div_87_tr_19_Template, 7, 4, "tr", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](20, ViewDriverComponent_div_87_tr_20_Template, 3, 0, "tr", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "button", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_div_87_Template_button_click_22_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r65.closeDriverShiftsPopup();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Close");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r3.ShiftInfo.DriverName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r3.ShiftInfo.Shifts);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.ShiftInfo.Shifts == null || ctx_r3.ShiftInfo.Shifts.length == 0);
      }
    }

    function ViewDriverComponent_div_88_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_div_93_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](1, "span", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function ViewDriverComponent_p_98_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "p", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, "Driver schedule exists.");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var ViewDriverComponent = /*#__PURE__*/function () {
      function ViewDriverComponent(driverService, regionService) {
        _classCallCheck(this, ViewDriverComponent);

        this.driverService = driverService;
        this.regionService = regionService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.dtOptionsOnboarded = {};
        this.dtTriggerOnboarded = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.InvitedDrivers = [];
        this.OnboardedDrivers = [];
        this.IsLoading = false;
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
        this.IsShowShiftInfoPopup = false;
        this.IsLoadingdriverDelete = false;
        this.IsScheduleExists = false;
        this.DriverShiftDetailList = [];
      }

      _createClass(ViewDriverComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.HeaderText = 'Create Driver';
          this.initializeInvitedDrivers();
          this.initializeOnboardedDrivers();
          this.getAllDrivers();
        }
      }, {
        key: "initializeInvitedDrivers",
        value: function initializeInvitedDrivers() {
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
              title: 'Driver Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Driver Details',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "initializeOnboardedDrivers",
        value: function initializeOnboardedDrivers() {
          var exportOnboardedColumns = {
            columns: ':visible'
          };
          this.dtOptionsOnboarded = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportOnboardedColumns
            }, {
              extend: 'csv',
              title: 'Driver Details',
              exportOptions: exportOnboardedColumns
            }, {
              extend: 'pdf',
              title: 'Driver Details',
              orientation: 'landscape',
              exportOptions: exportOnboardedColumns
            }, {
              extend: 'print',
              exportOptions: exportOnboardedColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "getAllDrivers",
        value: function getAllDrivers() {
          var _this16 = this;

          this.IsLoading = true;
          this.driverService.getAllDrivers().subscribe(function (data) {
            _this16.IsLoading = false;
            _this16.InvitedDrivers = data.InvitedDrivers;
            _this16.OnboardedDrivers = data.OnboardedDrivers;

            _this16.dtTrigger.next();

            _this16.dtTriggerOnboarded.next();
          });
        }
      }, {
        key: "editDriver",
        value: function editDriver(driver, isOnboarded) {
          if (this.CreateDriver != undefined) {
            this.CreateDriver.IsOnboarded = isOnboarded;
            this.CreateDriver.loadDriverDetail(driver);
          }
        }
      }, {
        key: "deleteDriver",
        value: function deleteDriver() {
          var _this17 = this;

          if (this.setDeleteDriverInfo != null) {
            this.IsLoadingdriverDelete = true;
            this.IsLoading = true;
            this.driverService.postDeleteDriver(this.setDeleteDriverInfo).subscribe(function (data) {
              _this17.IsLoading = false;
              _this17.IsLoadingdriverDelete = false;

              _this17.getDriverDetails();

              if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              } else if (data.StatusCode == 2) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
              }

              $("#btnDriverCancel").click();
            });
          }
        }
      }, {
        key: "changeDriverStatus",
        value: function changeDriverStatus(driver) {
          var _this18 = this;

          var isActive = driver.IsActive;
          var userId = driver.UserId;
          this.IsLoading = true;
          this.driverService.changeDriverStatus(userId, isActive).subscribe(function (data) {
            _this18.IsLoading = false;

            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_5__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "showDriverShifts",
        value: function showDriverShifts(driver) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee14() {
            var _this19 = this;

            var driverIds;
            return regeneratorRuntime.wrap(function _callee14$(_context14) {
              while (1) {
                switch (_context14.prev = _context14.next) {
                  case 0:
                    driverIds = driver.DriverId.toString();
                    this.IsShowShiftInfoPopup = true;
                    this.IsLoading = true;
                    this.regionService.getShiftByDrivers(driverIds, 0).subscribe(function (data) {
                      _this19.IsLoading = false;

                      if (data != null && data.Result) {
                        _this19.ShiftInfo.DriverName = driver.FirstName + " " + driver.LastName;
                        _this19.DriverShiftDetailList = data.Result;

                        _this19.setShiftInfo();
                      } else {
                        _this19.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
                      }
                    });

                  case 4:
                  case "end":
                    return _context14.stop();
                }
              }
            }, _callee14, this);
          }));
        }
      }, {
        key: "closeDriverShiftsPopup",
        value: function closeDriverShiftsPopup() {
          this.IsShowShiftInfoPopup = false;
          this.ShiftInfo = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["DriverShiftModel"]();
        }
      }, {
        key: "clearPanelControls",
        value: function clearPanelControls() {
          if (this.CreateDriver != undefined) {
            this.CreateDriver.clearForm();
          }
        }
      }, {
        key: "setPanelHeader",
        value: function setPanelHeader(headerText) {
          this.HeaderText = headerText;
        }
      }, {
        key: "getDriverDetails",
        value: function getDriverDetails() {
          this.getAllDrivers();
          $("#invited-driver-grid-datatable").DataTable().clear().destroy();
          $("#onboarded-driver-grid-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "setShiftInfo",
        value: function setShiftInfo() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee16() {
            var _this20 = this;

            return regeneratorRuntime.wrap(function _callee16$(_context16) {
              while (1) {
                switch (_context16.prev = _context16.next) {
                  case 0:
                    this.DriverShiftDetailList.forEach(function (driver) {
                      return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this20, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee15() {
                        var e_1, _a, _b, _c, shift, shiftDetail;

                        return regeneratorRuntime.wrap(function _callee15$(_context15) {
                          while (1) {
                            switch (_context15.prev = _context15.next) {
                              case 0:
                                _context15.prev = 0;
                                _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driver.ScheduleList);

                              case 2:
                                _context15.next = 4;
                                return _b.next();

                              case 4:
                                _c = _context15.sent;

                                if (_c.done) {
                                  _context15.next = 15;
                                  break;
                                }

                                shift = _c.value;
                                shiftDetail = new _models_DriverManagementModel__WEBPACK_IMPORTED_MODULE_4__["ShiftDetailModel"]();
                                shiftDetail.FromDate = moment__WEBPACK_IMPORTED_MODULE_6__(shift.StartDate).format('MM/DD/YYYY');
                                shiftDetail.ToDate = moment__WEBPACK_IMPORTED_MODULE_6__(shift.EndDate).format('MM/DD/YYYY');
                                shiftDetail.ShiftFrom = shift.ShiftDetail.StartTime;
                                shiftDetail.ShiftTo = shift.ShiftDetail.EndTime;
                                this.ShiftInfo.Shifts.push(shiftDetail);

                              case 13:
                                _context15.next = 2;
                                break;

                              case 15:
                                _context15.next = 20;
                                break;

                              case 17:
                                _context15.prev = 17;
                                _context15.t0 = _context15["catch"](0);
                                e_1 = {
                                  error: _context15.t0
                                };

                              case 20:
                                _context15.prev = 20;
                                _context15.prev = 21;

                                if (!(_c && !_c.done && (_a = _b["return"]))) {
                                  _context15.next = 25;
                                  break;
                                }

                                _context15.next = 25;
                                return _a.call(_b);

                              case 25:
                                _context15.prev = 25;

                                if (!e_1) {
                                  _context15.next = 28;
                                  break;
                                }

                                throw e_1.error;

                              case 28:
                                return _context15.finish(25);

                              case 29:
                                return _context15.finish(20);

                              case 30:
                              case "end":
                                return _context15.stop();
                            }
                          }
                        }, _callee15, this, [[0, 17, 20, 30], [21,, 25, 29]]);
                      }));
                    });

                  case 1:
                  case "end":
                    return _context16.stop();
                }
              }
            }, _callee16, this);
          }));
        }
      }, {
        key: "setdeleteDriver",
        value: function setdeleteDriver(driverInfo) {
          this.IsScheduleExists = false;
          this.setDeleteDriverInfo = driverInfo;

          if (driverInfo.IsScheduleExists) {
            this.IsScheduleExists = true;
          }
        }
      }]);

      return ViewDriverComponent;
    }();

    ViewDriverComponent.ɵfac = function ViewDriverComponent_Factory(t) {
      return new (t || ViewDriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"]));
    };

    ViewDriverComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: ViewDriverComponent,
      selectors: [["app-view-driver"]],
      viewQuery: function ViewDriverComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.CreateDriver = _t.first);
        }
      },
      decls: 104,
      vars: 11,
      consts: [[1, "row"], ["id", "addDriver", 1, "col-12", "col-sm-12"], [1, "pt0", "pull-left", "mb5"], ["onclick", "slidePanel('#driver-panel','35%')", 1, "fs18", "pull-left", "ml15", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt2", "pull-left"], [1, "fs14", "pull-left"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-invited-driver-grid", 1, "table-responsive"], ["id", "invited-driver-grid-datatable", "data-gridname", "17", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "InvitedDriverName"], ["data-key", "InvitedEmail"], ["data-key", "InvitedContactNumber"], ["data-key", "InvitedCompanyName"], ["data-key", "InvitedLicenseNumber"], ["data-key", "InvitedLicenseType"], ["data-key", "InvitedExpiryDate"], ["data-key", "InvitedTrailerTypes"], ["data-key", "InvitedFilldAuthorized"], ["data-key", "InvitedShiftInfo"], ["data-key", "InvitedAction"], [4, "ngFor", "ngForOf"], [1, "row", "mt15"], [1, "col-md-12", "mt10"], ["id", "div-onboarded-driver-grid", 1, "table-responsive"], ["id", "onboarded-driver-grid-datatable", "data-gridname", "22", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "OnboardedDriverName"], ["data-key", "OnboardedEmail"], ["data-key", "OnboardedContactNumber"], ["data-key", "OnboardedCompanyName"], ["data-key", "OnboardedLicenseNumber"], ["data-key", "OnboardedLicenseType"], ["data-key", "OnboardedExpiryDate"], ["data-key", "OnboardedTrailerTypes"], ["data-key", "OnboardedShiftInfo"], ["data-key", "OnboardedAction"], ["id", "driver-panel", 1, "side-panel", "pl5", "pr5"], [1, "side-panel-wrapper"], [1, "pt15", "pb0", "mb10"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel", 3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [3, "onSaveDriverData"], ["createDriver", ""], ["id", "driverShiftsModal", "class", "modal fade", "role", "dialog", 4, "ngIf"], ["class", "loader", 4, "ngIf"], ["id", "confirm-delete-driver", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade", "confirm-box"], ["role", "document", 1, "modal-dialog", "modal-sm"], [1, "modal-content"], [1, "modal-body"], ["class", "pa top0 bg-white left0 z-index5 loading-wrapper", "id", "deleteDriverLoading", 4, "ngIf"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], [1, "fas", "fa-times"], [1, "fs18", "f-bold", "mt0"], ["class", "pb10", "id", "deleteDelGrpHeading", 4, "ngIf"], [1, "text-right"], ["type", "button", 1, "btn", "btn-danger", "btn-lg", "btnDriverYes", 3, "click"], ["type", "button", "id", "btnDriverCancel", "data-dismiss", "modal", 1, "btn", "btn-primary"], ["onclick", "slidePanel('#driver-panel','30%')", 1, "btn", "btn-link", 3, "click"], [4, "ngIf"], [1, "text-center"], ["type", "button", "data-toggle", "modal", "data-target", "#driverShiftsModal", 1, "btn", "btn-link", 3, "click"], ["alt", "Shifts", "title", "Shifts", 1, "far", "fa-clock-o", "color-blue", "fs16"], ["type", "button", "data-toggle", "modal", "data-target", "#confirm-delete-driver", 1, "btn", "btn-link", 3, "click"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], ["type", "checkbox", "mwlConfirmationPopover", "", "placement", "bottom", 3, "ngModel", "popoverTitle", "confirmText", "cancelText", "confirm", "ngModelChange"], ["id", "driverShiftsModal", "role", "dialog", 1, "modal", "fade"], [1, "modal-dialog"], [1, "modal-header", "pt10", "pb5"], [1, "modal-title", "pt0"], ["type", "button", "data-dismiss", "modal", 1, "close", 3, "click"], [1, "table", "table-bordered"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], ["colspan", "3", 1, "text-center", "pa5"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], ["id", "deleteDriverLoading", 1, "pa", "top0", "bg-white", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["id", "deleteDelGrpHeading", 1, "pb10"]],
      template: function ViewDriverComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Invited");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_Template_a_click_4_listener() {
            return ctx.setPanelHeader("Add Driver");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](5, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Add Driver");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Driver Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "Contact Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](24, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Licence Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](28, "Licence Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "th", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Expiration Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "Trailer Type(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](34, "TrueFill Inc.Compatible");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, "Shift Info");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](38, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, ViewDriverComponent_tr_40_Template, 34, 17, "tr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Onboarded");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "table", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](54, "Driver Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "th", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](56, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "th", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](58, "Contact Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "th", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](60, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "th", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](62, "Licence Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](63, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](64, "Licence Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](65, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](66, "Expiration Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](68, "Trailer Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](70, "TrueFill Inc.Compatible");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](71, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](72, "Shift Info");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](73, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](74, "Active");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](75, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](76, ViewDriverComponent_tr_76_Template, 33, 21, "tr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](77, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](78, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](79, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](80, "a", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_Template_a_click_80_listener() {
            return ctx.clearPanelControls();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](81, "i", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](82, "h3", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](83);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](84, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "app-create-driver", 44, 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSaveDriverData", function ViewDriverComponent_Template_app_create_driver_onSaveDriverData_85_listener() {
            return ctx.getDriverDetails();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](87, ViewDriverComponent_div_87_Template, 24, 3, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](88, ViewDriverComponent_div_88_Template, 5, 0, "div", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](89, "div", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](90, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](91, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](92, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](93, ViewDriverComponent_div_93_Template, 2, 0, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](94, "button", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](95, "i", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](96, "h2", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](97, "Are you sure?");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](98, ViewDriverComponent_p_98_Template, 2, 0, "p", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](99, "div", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](100, "button", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function ViewDriverComponent_Template_button_click_100_listener() {
            return ctx.deleteDriver();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](101, "Confirm");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](102, "button", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](103, "Cancel");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.InvitedDrivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("dtOptions", ctx.dtOptionsOnboarded)("dtTrigger", ctx.dtTriggerOnboarded);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.OnboardedDrivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.HeaderText);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsShowShiftInfoPopup);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoadingdriverDelete);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsScheduleExists);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["CheckboxControlValueAccessor"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__["ɵc"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXItbWFuYWdlbWVudC92aWV3LWRyaXZlci92aWV3LWRyaXZlci5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](ViewDriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-view-driver',
          templateUrl: './view-driver.component.html',
          styleUrls: ['./view-driver.component.css']
        }]
      }], function () {
        return [{
          type: _services_driver_service__WEBPACK_IMPORTED_MODULE_7__["DriverService"]
        }, {
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"]
        }];
      }, {
        CreateDriver: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"],
          args: [_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_3__["CreateDriverComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver-routing.module.ts": function srcAppDriverDriverRoutingModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverScheduleRoutingModule", function () {
      return DriverScheduleRoutingModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./driver-schedule-calender/driver-schedule-calender.component */
    "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");
    /* harmony import */


    var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./driver-management/driver-management.component */
    "./src/app/driver/driver-management/driver-management.component.ts");
    /* harmony import */


    var _driver_driver_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ./driver/driver.component */
    "./src/app/driver/driver/driver.component.ts");

    var routeDriver = [{
      path: '',
      component: _driver_driver_component__WEBPACK_IMPORTED_MODULE_4__["DriverComponent"]
    }, {
      path: 'View',
      component: _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__["DriverManagementComponent"]
    }, {
      path: 'schedule',
      component: _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"]
    }];

    var DriverScheduleRoutingModule = function DriverScheduleRoutingModule() {
      _classCallCheck(this, DriverScheduleRoutingModule);
    };

    DriverScheduleRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: DriverScheduleRoutingModule
    });
    DriverScheduleRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function DriverScheduleRoutingModule_Factory(t) {
        return new (t || DriverScheduleRoutingModule)();
      },
      imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDriver)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DriverScheduleRoutingModule, {
        imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DriverScheduleRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeDriver)],
          exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts": function srcAppDriverDriverScheduleCalenderDriverScheduleCalenderComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverScheduleCalenderComponent", function () {
      return DriverScheduleCalenderComponent;
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


    var date_fns__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! date-fns */
    "./node_modules/date-fns/esm/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_calendar__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-calendar */
    "./node_modules/angular-calendar/__ivy_ngcc__/fesm2015/angular-calendar.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_5__);
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! src/app/company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/carrier/service/route-info.service */
    "./src/app/carrier/service/route-info.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../create-driver-schedule/create-driver-schedule.component */
    "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template(rf, ctx) {
      if (rf & 1) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "mwl-calendar-month-view", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("dayClicked", function DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template_mwl_calendar_month_view_dayClicked_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r12);

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r11.dayClicked($event.day);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](23);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("viewDate", ctx_r8.viewDate)("events", ctx_r8.evt)("cellTemplate", _r1)("refresh", ctx_r8.refresh)("activeDayIsOpen", ctx_r8.activeDayIsOpen);
      }
    }

    function DriverScheduleCalenderComponent_div_21_mwl_calendar_week_view_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "mwl-calendar-week-view", 49);
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("viewDate", ctx_r9.viewDate)("events", ctx_r9.evt)("refresh", ctx_r9.refresh);
      }
    }

    function DriverScheduleCalenderComponent_div_21_mwl_calendar_day_view_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](0, "mwl-calendar-day-view", 49);
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("viewDate", ctx_r10.viewDate)("events", ctx_r10.evt)("refresh", ctx_r10.refresh);
      }
    }

    function DriverScheduleCalenderComponent_div_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](6, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_9_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r13.viewDate = $event;
        })("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.closeOpenMonthViewDay();
        })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r16.setNextMonthEvents(ctx_r16.viewDate, "Previous");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "i", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_11_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r17.viewDate = $event;
        })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_11_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r18.setNextMonthEvents(ctx_r18.viewDate, "Today");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](13, "calendarDate");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r19.viewDate = $event;
        })("viewDateChange", function DriverScheduleCalenderComponent_div_21_Template_div_viewDateChange_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r20.closeOpenMonthViewDay();
        })("click", function DriverScheduleCalenderComponent_div_21_Template_div_click_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r21.setNextMonthEvents(ctx_r21.viewDate, "Next");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](15, "i", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "a", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r22.setView(ctx_r22.CalendarView.Month);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "label", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "Month");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "a", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_21_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r23.setView(ctx_r23.CalendarView.Week);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "label", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Week ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_21_Template_a_click_24_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r14);

          var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r24.setView(ctx_r24.CalendarView.Day);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "label", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "Day");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](27, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "button", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](30, "Open Modal");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](31, DriverScheduleCalenderComponent_div_21_mwl_calendar_month_view_31_Template, 1, 5, "mwl-calendar-month-view", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](32, DriverScheduleCalenderComponent_div_21_mwl_calendar_week_view_32_Template, 1, 3, "mwl-calendar-week-view", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, DriverScheduleCalenderComponent_div_21_mwl_calendar_day_view_33_Template, 1, 3, "mwl-calendar-day-view", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("view", ctx_r0.view)("viewDate", ctx_r0.viewDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("viewDate", ctx_r0.viewDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](13, 19, ctx_r0.viewDate, ctx_r0.view + "ViewTitle", "en"), " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("view", ctx_r0.view)("viewDate", ctx_r0.viewDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Month);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Month ? "btn-primary" : "btn-default");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Week);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Week ? "btn-primary" : "btn-default");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("active", ctx_r0.view === ctx_r0.CalendarView.Day);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", ctx_r0.view === ctx_r0.CalendarView.Day ? "btn-primary" : "btn-default");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngSwitch", ctx_r0.view);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngSwitchCase", ctx_r0.CalendarView.Month);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngSwitchCase", ctx_r0.CalendarView.Week);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngSwitchCase", ctx_r0.CalendarView.Day);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "background-color": a0
      };
    };

    function DriverScheduleCalenderComponent_ng_template_22_tr_3_td_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " - ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](4, _c0, item_r30.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", item_r30.driverShortName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r30.shiftFrom);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r30.shiftTo);
      }
    }

    function DriverScheduleCalenderComponent_ng_template_22_tr_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, DriverScheduleCalenderComponent_ng_template_22_tr_3_td_1_Template, 9, 6, "td", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r31 = ctx.index;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", i_r31 < 2);
      }
    }

    function DriverScheduleCalenderComponent_ng_template_22_tr_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function DriverScheduleCalenderComponent_ng_template_22_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var group_r34 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassMapInterpolate1"]("badge badge-", group_r34[0], "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", group_r34[1].length, " ");
      }
    }

    function DriverScheduleCalenderComponent_ng_template_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "table", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, DriverScheduleCalenderComponent_ng_template_22_tr_3_Template, 2, 1, "tr", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, DriverScheduleCalenderComponent_ng_template_22_tr_4_Template, 4, 0, "tr", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "span", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](8, "calendarDate");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](10, DriverScheduleCalenderComponent_ng_template_22_span_10_Template, 2, 4, "span", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var day_r25 = ctx.day;
        var locale_r26 = ctx.locale;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", day_r25.events);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", day_r25.events.length > 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](8, 4, day_r25.date, "monthViewDayNumber", locale_r26));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", day_r25.eventGroups);
      }
    }

    function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template(rf, ctx) {
      if (rf & 1) {
        var _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "i", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template_i_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r40);

          var event_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r38.editSchedule(event_r36);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " \xA0\xA0\xA0 ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "i", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template_i_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r40);

          var event_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().$implicit;

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r41.rmvSchedule(event_r36);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, " \xA0\xA0 ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function DriverScheduleCalenderComponent_table_33_tr_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "td", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, DriverScheduleCalenderComponent_table_33_tr_16_td_11_Template, 5, 0, "td", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var event_r36 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](13, _c0, event_r36.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](event_r36.driverName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](15, _c0, event_r36.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate3"](" ", event_r36.shiftFrom, " - ", event_r36.shiftTo, " ", event_r36.description, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](17, _c0, event_r36.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](event_r36.fromDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](19, _c0, event_r36.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](event_r36.toDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](21, _c0, event_r36.color.primary));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", event_r36.repeatDayList == null ? null : event_r36.repeatDayList.length, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !event_r36.isUnplanedSchedule);
      }
    }

    function DriverScheduleCalenderComponent_table_33_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "table", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Shift");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, " From Date ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "To Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Total Days");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Action");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](16, DriverScheduleCalenderComponent_table_33_tr_16_Template, 12, 23, "tr", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r3.Selectedevent);
      }
    }

    function DriverScheduleCalenderComponent_div_34_div_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r45 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "input", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_div_9_Template_input_ngModelChange_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r45);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r44.deleteOption = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "label", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Delete the entire range of this schedule");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 2)("ngModel", ctx_r43.deleteOption);
      }
    }

    function DriverScheduleCalenderComponent_div_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r47 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "h4", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Do you wish to :");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "input", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_Template_input_ngModelChange_6_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r47);

          var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r46.deleteOption = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "label", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "Delete only this day's schedule");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](9, DriverScheduleCalenderComponent_div_34_div_9_Template, 5, 2, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "input", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_34_Template_input_ngModelChange_12_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r47);

          var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r48.deleteOption = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "label", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Delete the whole schedule for this driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "button", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_34_Template_button_click_16_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r47);

          var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r49.closeDeleteModel();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Close");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "button", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_34_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r47);

          var ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r50.RemoveSchedule(ctx_r50.eventDelete);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 1)("ngModel", ctx_r4.deleteOption);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r4.hideDeleteRange);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 3)("ngModel", ctx_r4.deleteOption);
      }
    }

    function DriverScheduleCalenderComponent_div_35_Template(rf, ctx) {
      if (rf & 1) {
        var _r52 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "button", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_35_Template_button_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r52);

          var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r51.closeViewDayDetailModel();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Close");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function DriverScheduleCalenderComponent_div_36_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_27_Template(rf, ctx) {
      if (rf & 1) {
        var _r60 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "From Date:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "input", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_27_Template_input_ngModelChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r60);

          var ctx_r59 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r59.eFromDate = $event;
        })("onDateChange", function DriverScheduleCalenderComponent_div_38_div_27_Template_input_onDateChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r60);

          var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r61.setFromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("readonly", true)("ngModel", ctx_r53.eFromDate)("format", "MM/DD/YYYY")("minDate", ctx_r53.MinStartDate)("maxDate", ctx_r53.MaxStartDate);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_28_div_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_28_Template(rf, ctx) {
      if (rf & 1) {
        var _r64 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "From Date:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "input", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_28_Template_input_ngModelChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r63 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r63.eFromDate = $event;
        })("onDateChange", function DriverScheduleCalenderComponent_div_38_div_28_Template_input_onDateChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r64);

          var ctx_r65 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r65.setFromDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](4, DriverScheduleCalenderComponent_div_38_div_28_div_4_Template, 2, 0, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r54 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r54.eFromDate)("format", "MM/DD/YYYY")("minDate", ctx_r54.MinStartDate)("maxDate", ctx_r54.MaxStartDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r54.isRequired("eFromDate"));
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r68 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "input", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("change", function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template_input_change_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r68);

          var ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r67.InitializeDates();
        })("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_div_15_Template_input_ngModelChange_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r68);

          var ctx_r69 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

          return ctx_r69.selectedType = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "label", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Custom");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 4)("ngModel", ctx_r66.selectedType);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r71 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "input", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_4_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r71);

          var ctx_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r70.selectedType = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Daily");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "input", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r71);

          var ctx_r72 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r72.selectedType = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "label", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "WeekDays (Mon to Fri)");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "input", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_34_Template_input_ngModelChange_12_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r71);

          var ctx_r73 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r73.selectedType = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "label", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Repeat Every");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](15, DriverScheduleCalenderComponent_div_38_div_34_div_15_Template, 4, 2, "div", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 1)("ngModel", ctx_r55.selectedType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 2)("ngModel", ctx_r55.selectedType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", 3)("ngModel", ctx_r55.selectedType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r55.eToDate && ctx_r55.eFromDate);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_37_Template(rf, ctx) {
      if (rf & 1) {
        var _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, " Dates:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "ng-multiselect-dropdown", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_37_Template_ng_multiselect_dropdown_ngModelChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r75);

          var ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r74.customDates = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Date (s)")("settings", ctx_r56.multiselectDateSettingsById)("data", ctx_r56.DateList)("ngModel", ctx_r56.customDates);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_38_Template(rf, ctx) {
      if (rf & 1) {
        var _r77 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "label", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2, "Days:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "input", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_div_38_Template_input_ngModelChange_3_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r77);

          var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r76.repeat = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r57 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r57.repeat);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_40_table_8_tr_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var event_r80 = ctx.$implicit;

        var ctx_r79 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r79.driverSchedule.selectedShifts[0].Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](event_r80);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_40_table_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "table", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "thead");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Shift");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "th");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, " Conflict Dates");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, DriverScheduleCalenderComponent_div_38_div_40_table_8_tr_8_Template, 5, 2, "tr", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r78 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r78.ConflictDateList);
      }
    }

    function DriverScheduleCalenderComponent_div_38_div_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r82 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "i", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4, "Warning:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](5, " Driver Schedule(s) exists ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "a", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_38_div_40_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r82);

          var ctx_r81 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

          return ctx_r81.showDriverConflictSchedules();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Show Details");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](8, DriverScheduleCalenderComponent_div_38_div_40_table_8_Template, 9, 1, "table", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r58.IsShowConflictTable);
      }
    }

    function DriverScheduleCalenderComponent_div_38_Template(rf, ctx) {
      if (rf & 1) {
        var _r84 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "ng-sidebar-container", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "ng-sidebar", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("openedChange", function DriverScheduleCalenderComponent_div_38_Template_ng_sidebar_openedChange_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r83._openedEditPanel = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "a", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_38_Template_a_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r85._editToggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "h3", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Modify Driver Schedule");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "content", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("keydown.enter", function DriverScheduleCalenderComponent_div_38_Template_content_keydown_enter_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          return $event.preventDefault();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Region:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "ng-multiselect-dropdown", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_13_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r87 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r87.Selectedregion = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "div", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "label", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Driver:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "ng-multiselect-dropdown", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_19_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r88 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r88.SelectedDriver = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "label", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](23, "Shift:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "ng-multiselect-dropdown", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_ngModelChange_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r89 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r89.driverSchedule.selectedShifts = $event;
        })("onSelect", function DriverScheduleCalenderComponent_div_38_Template_ng_multiselect_dropdown_onSelect_24_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r90 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r90.validateShiftForDriver($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](27, DriverScheduleCalenderComponent_div_38_div_27_Template, 4, 5, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](28, DriverScheduleCalenderComponent_div_38_div_28_Template, 5, 5, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](31, "label", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](32, "To Date:");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "input", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_div_38_Template_input_ngModelChange_33_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r91 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r91.eToDate = $event;
        })("onDateChange", function DriverScheduleCalenderComponent_div_38_Template_input_onDateChange_33_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r92 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r92.setToDate($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, DriverScheduleCalenderComponent_div_38_div_34_Template, 16, 7, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](35, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](36, "div", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](37, DriverScheduleCalenderComponent_div_38_div_37_Template, 4, 4, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, DriverScheduleCalenderComponent_div_38_div_38_Template, 4, 1, "div", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](40, DriverScheduleCalenderComponent_div_38_div_40_Template, 9, 1, "div", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "div", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "input", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_38_Template_input_click_43_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r93 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r93._editToggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "button", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DriverScheduleCalenderComponent_div_38_Template_button_click_44_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r84);

          var ctx_r94 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r94.UpdateSchedule();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](45, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("opened", ctx_r7._openedEditPanel)("animate", ctx_r7._animate)("position", ctx_r7._POSITIONS[ctx_r7._positionNum]);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Region(s)")("settings", ctx_r7.singleselectSettingsById)("data", ctx_r7.DriverRegionList)("ngModel", ctx_r7.Selectedregion)("disabled", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Driver(s)")("settings", ctx_r7.singleselectSettingsById)("data", ctx_r7.SelectedDriverList)("ngModel", ctx_r7.SelectedDriver)("disabled", true);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Shift(s)")("settings", ctx_r7.multiselectShiftById)("data", ctx_r7.ShiftList)("ngModel", ctx_r7.driverSchedule.selectedShifts);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx_r7.startDateEnable);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.startDateEnable);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx_r7.eToDate)("format", "MM/DD/YYYY")("minDate", ctx_r7.MinStartDate)("maxDate", ctx_r7.MaxStartDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.IsUpdateForMultiple);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.IsUpdateForMultiple && ctx_r7.selectedType == "4");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.IsUpdateForMultiple && ctx_r7.selectedType == "3");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r7.ConflictDateList.length > 0);
      }
    }

    var DriverScheduleCalenderComponent = /*#__PURE__*/function () {
      //#endregion
      function DriverScheduleCalenderComponent(regionService, routeService) {
        _classCallCheck(this, DriverScheduleCalenderComponent);

        this.regionService = regionService;
        this.routeService = routeService;
        this.CalendarView = angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"];
        this.isShowCalender = true;
        this.IsLoading = false;
        this.DriverRegionList = [];
        this.driverIds = '';
        this.SelectedDriverList = [];
        this.isShowEditPannel = false;
        this.isApplyAll = false; //edit schedule

        this.DriverShiftDetailList = [];
        this.RepeatList = [];
        this.DateList = [];
        this.ShiftList = [];
        this.regionList = [];
        this.TrailerList = [];
        this.RouteList = [];
        this.driverSchedule = {};
        this.driverScheduleMapping = {};
        this.repeat = 1;
        this.customDates = []; //min max date

        this.MinStartDate = new Date();
        this.MaxStartDate = new Date();
        this.view = angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month;
        this.viewDate = new Date(); //view 

        this.Selectedevent = []; //end      

        this.IsUpdateForMultiple = false;
        this.scheduleType = 0;
        this.refresh = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"](); //evt: CalendarEvent[] = [];

        this.evt = [];
        this.activeDayIsOpen = false;
        this.RegionShiftDetailList = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom']; //#region  Edit & Delete

        this._openedEditPanel = false;
        this.IsSheduleEdit = false;
        this.Selectedregion = [];
        this.SelectedDriver = [];
        this.IsShiftRepeted = false;
        this.selectedType = 0;
        this.IsConfirmDelete = false;
        this.deleteOption = 1;
        this.hideDeleteRange = false;
        this.ConflictDateList = [];
        this.IsShowConflictTable = false;
      }

      _createClass(DriverScheduleCalenderComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.getDrivers(); //  this.getShifts();

          this.getRegions();
          this.init();
          this.MaxStartDate.setMonth(this.MaxStartDate.getMonth() + 2); //this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 10);
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.IsLoading = false;
        }
      }, {
        key: "init",
        value: function init() {
          this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.singleselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: false,
            enableCheckAll: false,
            disabledField: "true"
          };
          this.multiselectShiftById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: false
          };
          this.multiselectDateSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: false,
            enableCheckAll: true
          };
        }
      }, {
        key: "dayClicked",
        value: function dayClicked(_ref) {
          var date = _ref.date,
              events = _ref.events;

          if (Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["isSameMonth"])(date, this.viewDate)) {
            if (Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["isSameDay"])(this.viewDate, date) && this.activeDayIsOpen === true || events.length === 0) {
              this.activeDayIsOpen = false;
            } else {
              this.Selectedevent = events;
              this.SelectedDate = moment__WEBPACK_IMPORTED_MODULE_5__(date).format('MM/DD/YYYY');
              var element = document.getElementById('idViewDay');
              element.click(); //this.activeDayIsOpen = true;
            }

            this.viewDate = date;
          }
        }
      }, {
        key: "setView",
        value: function setView(view) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee17() {
            return regeneratorRuntime.wrap(function _callee17$(_context17) {
              while (1) {
                switch (_context17.prev = _context17.next) {
                  case 0:
                    this.view = view;
                    this.setNextMonthEvents(this.viewDate, this.view);

                  case 2:
                  case "end":
                    return _context17.stop();
                }
              }
            }, _callee17, this);
          }));
        }
      }, {
        key: "closeOpenMonthViewDay",
        value: function closeOpenMonthViewDay() {
          this.activeDayIsOpen = false;
        }
      }, {
        key: "getDrivers",
        value: function getDrivers() {
          var _this21 = this;

          this.regionService.getDrivers().subscribe(function (drivers) {
            _this21.DriverList = drivers;
          });
        } // public onRegionSelect($event) {
        //     var region = this.regionList.find(f => f.Id == $event.Id);
        //     this.TrailerList = region.Trailers.map(res => { return { Id: res.Code, Name: `${res.Name}` } });
        //     this.DriverList = region.Drivers.map(res => { return { Id: res.Id, Name: `${res.Name}` } });
        //     this.getRoutes(region.Id);
        //     this.getRegionScheduls(region) 
        // }

      }, {
        key: "getRegionScheduls",
        value: function getRegionScheduls(region) {
          var _this22 = this;

          this.regionService.getSchedulesByRegion(region.Id, this.scheduleType).subscribe(function (regions) {
            _this22.RegionShiftDetailList = regions;
          });
        }
      }, {
        key: "getRoutes",
        value: function getRoutes(regionId) {
          var _this23 = this;

          this.routeService.getRoutesByRegion(regionId).subscribe(function (routes) {
            _this23.getRouteDropDown(routes);
          });
        }
      }, {
        key: "getRouteDropDown",
        value: function getRouteDropDown(routeList) {
          this.RouteList = routeList.ResponseData;
        }
      }, {
        key: "onDriverSelect",
        value: function onDriverSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee18() {
            var _this24 = this;

            var driverIds, drivers;
            return regeneratorRuntime.wrap(function _callee18$(_context18) {
              while (1) {
                switch (_context18.prev = _context18.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(true);
                    this.evt = [];
                    driverIds = [];
                    this.SelectedDriverList.forEach(function (res) {
                      driverIds.push(res.Id);
                    });
                    drivers = driverIds.join();

                    if (driverIds.length > 0) {
                      this.regionService.getShiftByDrivers(drivers, this.scheduleType).subscribe(function (data) {
                        if (data.Result) {
                          _this24.DriverShiftDetailList = [];
                          _this24.DriverShiftDetailList = data.Result;

                          _this24.setNextMonthEvents(_this24.viewDate, 'Today');
                        } //let element: HTMLElement = document.getElementById('idToday') as HTMLElement;
                        // element.click();


                        _this24.IsLoading = false;
                      });
                    }

                    this.regionService.onLoadingChanged.next(false);

                  case 7:
                  case "end":
                    return _context18.stop();
                }
              }
            }, _callee18, this);
          }));
        }
      }, {
        key: "onDriverDeSelect",
        value: function onDriverDeSelect($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee19() {
            return regeneratorRuntime.wrap(function _callee19$(_context19) {
              while (1) {
                switch (_context19.prev = _context19.next) {
                  case 0:
                    this.isShowCalender = false;
                    _context19.next = 3;
                    return this.evt.filter(function (f) {
                      return f.driverId != $event.Id;
                    });

                  case 3:
                    this.evt = _context19.sent;
                    //  this.DriverShiftDetailList = await this.DriverShiftDetailList.filter(f => f.DriverId != $event.Id);
                    this.isShowCalender = true;

                  case 5:
                  case "end":
                    return _context19.stop();
                }
              }
            }, _callee19, this);
          }));
        }
      }, {
        key: "setEvents",
        value: function setEvents(mnth, year) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee21() {
            var _this25 = this;

            return regeneratorRuntime.wrap(function _callee21$(_context22) {
              while (1) {
                switch (_context22.prev = _context22.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(true);
                    this.DriverShiftDetailList.forEach(function (driver) {
                      return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this25, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee20() {
                        var _this26 = this;

                        var e_1, _a, color, driverShortName, _b, _c, shift, startDate, endDate, event, currentDate, eDate, sDate, date, currentMonthLastDate, _loop, i;

                        return regeneratorRuntime.wrap(function _callee20$(_context21) {
                          while (1) {
                            switch (_context21.prev = _context21.next) {
                              case 0:
                                _context21.next = 2;
                                return this.getRandomColor();

                              case 2:
                                color = _context21.sent;
                                driverShortName = this.getShortDriverName(driver.DriverName);
                                _context21.prev = 4;
                                _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driver.ScheduleList);

                              case 6:
                                _context21.next = 8;
                                return _b.next();

                              case 8:
                                _c = _context21.sent;

                                if (_c.done) {
                                  _context21.next = 32;
                                  break;
                                }

                                shift = _c.value;
                                startDate = this.changeTimeFormat(shift.ShiftDetail.StartTime);
                                endDate = this.changeTimeFormat(shift.ShiftDetail.EndTime); //previous and next logic

                                if (year) {
                                  startDate = new Date(new Date(new Date(new Date(startDate).setMonth(mnth))).setFullYear(year)).getTime();
                                  endDate = new Date(new Date(new Date(new Date(endDate).setMonth(mnth))).setFullYear(year)).getTime();
                                } //end


                                if (this.view != angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month && startDate > endDate) // if start time is greater than end time then add 1 day in end time
                                  {
                                    endDate = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(endDate), 1).getTime();
                                  } // creating template for event schedule


                                event = {
                                  mappingId: driver.Id,
                                  repeatEvery: shift.RepeatEveryDay,
                                  typeId: shift.TypeId,
                                  id: shift.Id,
                                  repeatDayList: shift.RepeatDayList,
                                  shiftId: shift.ShiftId,
                                  regionName: shift.ShiftDetail.RegionName,
                                  driverId: driver.DriverId,
                                  driverName: driver.DriverName,
                                  driverShortName: driverShortName,
                                  shiftFrom: shift.ShiftDetail.StartTime,
                                  shiftTo: shift.ShiftDetail.EndTime,
                                  fromDate: moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrStartDate).format('MM/DD/YYYY'),
                                  toDate: moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrEndDate).format('MM/DD/YYYY'),
                                  start: new Date(startDate),
                                  end: new Date(endDate),
                                  title: "<table class=\"table \"> <tr><td> Driver - <strong>".concat(driver.DriverName, "</strong></td> <td><strong>").concat(shift.ShiftDetail.StartTime, " - ").concat(shift.ShiftDetail.EndTime, "</strong></td></tr></table> "),
                                  color: color,
                                  resizable: {
                                    beforeStart: true,
                                    afterEnd: true
                                  },
                                  draggable: false,
                                  isUnplanedSchedule: driver.IsUnplanedSchedule,
                                  description: shift.Description
                                }; //end
                                //this.evt.push(event)

                                currentDate = new Date().getDate();
                                eDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrEndDate).toLocaleString()).setHours(23, 59, 59, 0);
                                sDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(shift.StrStartDate).toLocaleString()).setHours(0, 0, 0, 0);
                                date = new Date(new Date().setMonth(mnth)).setFullYear(year);
                                _context21.next = 22;
                                return this.daysInMonth(mnth + 1, year);

                              case 22:
                                currentMonthLastDate = _context21.sent;
                                _loop = /*#__PURE__*/regeneratorRuntime.mark(function _loop(i) {
                                  return regeneratorRuntime.wrap(function _loop$(_context20) {
                                    while (1) {
                                      switch (_context20.prev = _context20.next) {
                                        case 0:
                                          if (!(new Date(sDate).getTime() <= Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(date), i).getTime() && new Date(eDate).getTime() >= Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(date), i).getTime() && shift.RepeatDayList && shift.RepeatDayStringList.filter(function (dt) {
                                            return moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.start), i)).format('MM/DD/YYYY');
                                          }).length > 0)) {
                                            _context20.next = 3;
                                            break;
                                          }

                                          _context20.next = 3;
                                          return _this26.addEventShift(event, i);

                                        case 3:
                                        case "end":
                                          return _context20.stop();
                                      }
                                    }
                                  }, _loop);
                                });
                                i = -currentDate;

                              case 25:
                                if (!(i < currentMonthLastDate + 1 - currentDate)) {
                                  _context21.next = 30;
                                  break;
                                }

                                return _context21.delegateYield(_loop(i), "t0", 27);

                              case 27:
                                i++;
                                _context21.next = 25;
                                break;

                              case 30:
                                _context21.next = 6;
                                break;

                              case 32:
                                _context21.next = 37;
                                break;

                              case 34:
                                _context21.prev = 34;
                                _context21.t1 = _context21["catch"](4);
                                e_1 = {
                                  error: _context21.t1
                                };

                              case 37:
                                _context21.prev = 37;
                                _context21.prev = 38;

                                if (!(_c && !_c.done && (_a = _b["return"]))) {
                                  _context21.next = 42;
                                  break;
                                }

                                _context21.next = 42;
                                return _a.call(_b);

                              case 42:
                                _context21.prev = 42;

                                if (!e_1) {
                                  _context21.next = 45;
                                  break;
                                }

                                throw e_1.error;

                              case 45:
                                return _context21.finish(42);

                              case 46:
                                return _context21.finish(37);

                              case 47:
                              case "end":
                                return _context21.stop();
                            }
                          }
                        }, _callee20, this, [[4, 34, 37, 47], [38,, 42, 46]]);
                      }));
                    });
                    this.regionService.onLoadingChanged.next(false);

                  case 3:
                  case "end":
                    return _context22.stop();
                }
              }
            }, _callee21, this);
          }));
        }
      }, {
        key: "addEventShift",
        value: function addEventShift(event, i) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee22() {
            var evt;
            return regeneratorRuntime.wrap(function _callee22$(_context23) {
              while (1) {
                switch (_context23.prev = _context23.next) {
                  case 0:
                    evt = Object.assign({}, event);
                    evt.start = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.start), i);
                    evt.end = Object(date_fns__WEBPACK_IMPORTED_MODULE_2__["addDays"])(new Date(event.end), i);
                    this.evt.push(evt);

                  case 4:
                  case "end":
                    return _context23.stop();
                }
              }
            }, _callee22, this);
          }));
        }
      }, {
        key: "daysInMonth",
        value: function daysInMonth(month, year) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee23() {
            return regeneratorRuntime.wrap(function _callee23$(_context24) {
              while (1) {
                switch (_context24.prev = _context24.next) {
                  case 0:
                    return _context24.abrupt("return", new Date(year, month, 0).getDate());

                  case 1:
                  case "end":
                    return _context24.stop();
                }
              }
            }, _callee23);
          }));
        }
      }, {
        key: "changeTimeFormat",
        value: function changeTimeFormat(time) {
          //var time = '06:30 PM';
          var hours = Number(time.match(/^(\d+)/)[1]);
          var minutes = Number(time.match(/:(\d+)/)[1]);
          var AMPM = time.match(/\s(.*)$/)[1];
          if (AMPM == "PM" && hours < 12) hours = hours + 12;
          if (AMPM == "AM" && hours == 12) hours = hours - 12;
          var sHours = hours.toString();
          var sMinutes = minutes.toString();
          if (hours < 10) sHours = "0" + sHours;
          if (minutes < 10) sMinutes = "0" + sMinutes;
          var date = new Date(new Date().setHours(+sHours)).setMinutes(+sMinutes);
          return date;
        }
      }, {
        key: "getRandomColor",
        value: function getRandomColor() {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee24() {
            var letters, color, i;
            return regeneratorRuntime.wrap(function _callee24$(_context25) {
              while (1) {
                switch (_context25.prev = _context25.next) {
                  case 0:
                    letters = 'BCDEF'.split('');
                    color = '#';

                    for (i = 0; i < 6; i++) {
                      color += letters[Math.floor(Math.random() * letters.length)];
                    }

                    return _context25.abrupt("return", {
                      primary: color,
                      secondary: color
                    });

                  case 4:
                  case "end":
                    return _context25.stop();
                }
              }
            }, _callee24);
          }));
        }
      }, {
        key: "setNextMonthEvents",
        value: function setNextMonthEvents(date, event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee25() {
            return regeneratorRuntime.wrap(function _callee25$(_context26) {
              while (1) {
                switch (_context26.prev = _context26.next) {
                  case 0:
                    this.evt = [];
                    _context26.next = 3;
                    return this.setEvents(new Date(date).getMonth(), new Date(date).getFullYear());

                  case 3:
                  case "end":
                    return _context26.stop();
                }
              }
            }, _callee25, this);
          }));
        }
      }, {
        key: "OnScheduleAdded",
        value: function OnScheduleAdded($event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee26() {
            return regeneratorRuntime.wrap(function _callee26$(_context27) {
              while (1) {
                switch (_context27.prev = _context27.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(false);
                    this.IsLoading = false;

                    if ($event) {
                      this.scheduleType = 0;
                      this.SelectedDriverList.forEach(function (res) {
                        var cnt = $event.findIndex(function (x) {
                          return x.Id == res.Id;
                        });
                        if (cnt < 0) $event.push(res);
                      });
                      this.SelectedDriverList = $event.slice();
                      this.onDriverSelect();
                    }

                  case 3:
                  case "end":
                    return _context27.stop();
                }
              }
            }, _callee26, this);
          }));
        } //edit

      }, {
        key: "rmvSchedule",
        value: function rmvSchedule(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee27() {
            return regeneratorRuntime.wrap(function _callee27$(_context28) {
              while (1) {
                switch (_context28.prev = _context28.next) {
                  case 0:
                    this.eventDelete = event;
                    this.AssignVeriables(event);
                    this.IsConfirmDelete = true;

                  case 3:
                  case "end":
                    return _context28.stop();
                }
              }
            }, _callee27, this);
          }));
        }
      }, {
        key: "updateCurrentSchedule",
        value: function updateCurrentSchedule() {
          var e_2, _a, e_3, _b;

          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee28() {
            var _this27 = this;

            var getCurrentSelectedShift, checkShift, index, _c, _d, item, list, _e, _f, repeat;

            return regeneratorRuntime.wrap(function _callee28$(_context29) {
              while (1) {
                switch (_context29.prev = _context29.next) {
                  case 0:
                    if (!(this.driverSchedule.selectedShifts.length == 0)) {
                      _context29.next = 3;
                      break;
                    }

                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Select shift', undefined, undefined);

                    return _context29.abrupt("return", false);

                  case 3:
                    _context29.next = 5;
                    return this.DriverShiftDetailList.find(function (f) {
                      return f.Id == _this27.driverScheduleMapping.Id;
                    });

                  case 5:
                    this.driverScheduleMapping = _context29.sent;

                    if (this.driverScheduleMapping != null && this.driverScheduleMapping.ScheduleList.length > 0) {
                      this.driverScheduleMapping.ScheduleList.forEach(function (f) {
                        if (f.IsActive = true && f.Id == _this27.driverSchedule.Id && f.ShiftId == _this27.driverSchedule.ShiftId) {
                          if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                            if (f.RepeatDayList.length == 1) {
                              delete f.RepeatDayList[0];
                              f.RepeatDayList = [];
                            } else {
                              var indexof = f.RepeatDayList.findIndex(function (x) {
                                return moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(_this27.sdate).format('MM/DD/YYYY');
                              });
                              delete f.RepeatDayList[indexof];
                              var reOrder = [];
                              f.RepeatDayList.forEach(function (r) {
                                reOrder.push(r);
                              });
                              f.RepeatDayList = reOrder;
                            }
                          }
                        }
                      });
                      getCurrentSelectedShift = this.driverSchedule.selectedShifts[0].Id;
                      checkShift = this.driverScheduleMapping.ScheduleList.filter(function (x) {
                        return x.IsActive = true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null;
                      });

                      if (checkShift != null && checkShift.length > 0) {
                        index = this.driverScheduleMapping.ScheduleList.findIndex(function (x) {
                          return x.IsActive = true && x.ShiftId == getCurrentSelectedShift && x.RepeatDayList == null;
                        });
                        this.driverScheduleMapping.ScheduleList[index].RepeatDayList = [];
                        this.driverScheduleMapping.ScheduleList[index].RepeatDayList.push(moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate());
                        this.driverScheduleMapping.ScheduleList[index].StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                        this.driverScheduleMapping.ScheduleList[index].EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                      } else {
                        this.driverSchedule.RepeatDayList = [];
                        this.driverSchedule.RepeatDayList.push(moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate());
                        this.driverScheduleMapping.ScheduleList.push({
                          Id: "".concat(this.driverScheduleMapping.DriverId, "_").concat(new Date().getTime()),
                          IsActive: true,
                          StartDate: moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate(),
                          EndDate: moment__WEBPACK_IMPORTED_MODULE_5__(this.SelectedDate).toDate(),
                          RepeatDayList: this.driverSchedule.RepeatDayList,
                          ShiftId: getCurrentSelectedShift
                        });
                      }
                    }

                    _context29.prev = 7;
                    _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList);

                  case 9:
                    _context29.next = 11;
                    return _c.next();

                  case 11:
                    _d = _context29.sent;

                    if (_d.done) {
                      _context29.next = 46;
                      break;
                    }

                    item = _d.value;

                    if (!(item.RepeatDayList != null && item.RepeatDayList.length > 0)) {
                      _context29.next = 44;
                      break;
                    }

                    item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                    item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                    list = [];
                    _context29.prev = 18;
                    _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList);

                  case 20:
                    _context29.next = 22;
                    return _e.next();

                  case 22:
                    _f = _context29.sent;

                    if (_f.done) {
                      _context29.next = 28;
                      break;
                    }

                    repeat = _f.value;
                    list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());

                  case 26:
                    _context29.next = 20;
                    break;

                  case 28:
                    _context29.next = 33;
                    break;

                  case 30:
                    _context29.prev = 30;
                    _context29.t0 = _context29["catch"](18);
                    e_3 = {
                      error: _context29.t0
                    };

                  case 33:
                    _context29.prev = 33;
                    _context29.prev = 34;

                    if (!(_f && !_f.done && (_b = _e["return"]))) {
                      _context29.next = 38;
                      break;
                    }

                    _context29.next = 38;
                    return _b.call(_e);

                  case 38:
                    _context29.prev = 38;

                    if (!e_3) {
                      _context29.next = 41;
                      break;
                    }

                    throw e_3.error;

                  case 41:
                    return _context29.finish(38);

                  case 42:
                    return _context29.finish(33);

                  case 43:
                    item.RepeatDayList = list;

                  case 44:
                    _context29.next = 9;
                    break;

                  case 46:
                    _context29.next = 51;
                    break;

                  case 48:
                    _context29.prev = 48;
                    _context29.t1 = _context29["catch"](7);
                    e_2 = {
                      error: _context29.t1
                    };

                  case 51:
                    _context29.prev = 51;
                    _context29.prev = 52;

                    if (!(_d && !_d.done && (_a = _c["return"]))) {
                      _context29.next = 56;
                      break;
                    }

                    _context29.next = 56;
                    return _a.call(_c);

                  case 56:
                    _context29.prev = 56;

                    if (!e_2) {
                      _context29.next = 59;
                      break;
                    }

                    throw e_2.error;

                  case 59:
                    return _context29.finish(56);

                  case 60:
                    return _context29.finish(51);

                  case 61:
                    this.update(this.driverScheduleMapping);

                  case 62:
                  case "end":
                    return _context29.stop();
                }
              }
            }, _callee28, this, [[7, 48, 51, 61], [18, 30, 33, 43], [34,, 38, 42], [52,, 56, 60]]);
          }));
        }
      }, {
        key: "editSchedule",
        value: function editSchedule(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee29() {
            var currentDate;
            return regeneratorRuntime.wrap(function _callee29$(_context30) {
              while (1) {
                switch (_context30.prev = _context30.next) {
                  case 0:
                    this.DriverRegionList = [];
                    currentDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__().toLocaleString()).setHours(0, 0, 0, 0);

                    if (new Date(event.toDate).getTime() < new Date(currentDate).getTime()) {
                      _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('You cannot edit past records.', undefined, undefined);
                    } else {
                      this._editToggleOpened(true);

                      this.TypeCheck(event);
                      this.AssignVeriables(event);
                    }

                  case 3:
                  case "end":
                    return _context30.stop();
                }
              }
            }, _callee29, this);
          }));
        }
      }, {
        key: "AssignVeriables",
        value: function AssignVeriables(event) {
          var e_4, _a;

          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee30() {
            var _this28 = this;

            var currentDate, _b, _c, item, dRegion, chCustomDates, days;

            return regeneratorRuntime.wrap(function _callee30$(_context31) {
              while (1) {
                switch (_context31.prev = _context31.next) {
                  case 0:
                    this.deleteOption = this.deleteOption ? this.deleteOption : 1;
                    this.DriverRegionList = [];
                    currentDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__().toLocaleString()).setHours(0, 0, 0, 0);
                    _context31.prev = 3;
                    _b = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.regionList);

                  case 5:
                    _context31.next = 7;
                    return _b.next();

                  case 7:
                    _c = _context31.sent;

                    if (_c.done) {
                      _context31.next = 13;
                      break;
                    }

                    item = _c.value;

                    if (item.Drivers.find(function (f) {
                      return f.Id == event.driverId;
                    })) {
                      dRegion = {
                        Id: 0,
                        Name: ""
                      };
                      dRegion.Id = item.Id;
                      dRegion.Name = item.Name;
                      this.DriverRegionList.push(dRegion);
                      this.Selectedregion = this.DriverRegionList;
                      this.SelectedDriver = item.Drivers.filter(function (f) {
                        return f.Id == event.driverId;
                      });
                      this.ShiftList = item.Shifts.map(function (res) {
                        return {
                          Id: res.Id,
                          Name: "".concat(res.StartTime, " - ").concat(res.EndTime)
                        };
                      });
                    }

                  case 11:
                    _context31.next = 5;
                    break;

                  case 13:
                    _context31.next = 18;
                    break;

                  case 15:
                    _context31.prev = 15;
                    _context31.t0 = _context31["catch"](3);
                    e_4 = {
                      error: _context31.t0
                    };

                  case 18:
                    _context31.prev = 18;
                    _context31.prev = 19;

                    if (!(_c && !_c.done && (_a = _b["return"]))) {
                      _context31.next = 23;
                      break;
                    }

                    _context31.next = 23;
                    return _a.call(_b);

                  case 23:
                    _context31.prev = 23;

                    if (!e_4) {
                      _context31.next = 26;
                      break;
                    }

                    throw e_4.error;

                  case 26:
                    return _context31.finish(23);

                  case 27:
                    return _context31.finish(18);

                  case 28:
                    this.driverSchedule.selectedShifts = this.ShiftList.filter(function (f) {
                      return f.Id == event.shiftId;
                    }); // this.isShowEditPannel = true;

                    this.eFromDate = moment__WEBPACK_IMPORTED_MODULE_5__(event.start).format('MM/DD/YYYY');
                    this.eToDate = moment__WEBPACK_IMPORTED_MODULE_5__(event.toDate).format('MM/DD/YYYY');
                    this.driverScheduleMapping.Id = event.mappingId;
                    this.driverSchedule.Id = event.Id;
                    this.driverSchedule.ShiftId = event.shiftId;
                    this.driverScheduleMapping.DriverId = event.driverId;
                    this.DriverId = event.driverId;
                    this.driverScheduleMapping.DriverName = event.driverName;
                    this.sdate = moment__WEBPACK_IMPORTED_MODULE_5__(event.start).format('MM/DD/YYYY'); //this.SelectedDate;

                    this.edate = moment__WEBPACK_IMPORTED_MODULE_5__(event.toDate).format('MM/DD/YYYY');
                    this.IsSheduleEdit = true; //this.SelectedDate = moment(this.sdate).format('MM/DD/YYYY');

                    this.IsUpdateForMultiple = false;

                    if (this.eFromDate < this.eToDate) {
                      this.IsUpdateForMultiple = true;
                    }

                    this.hideDeleteRange = false;

                    if (new Date(this.sdate).getTime() == new Date(this.eToDate).getTime()) {
                      this.hideDeleteRange = true;
                    }

                    if (new Date(this.sdate).getTime() < new Date(currentDate).getTime()) {
                      this.startDateEnable = true;
                    }

                    this.driverSchedule.Id = event.id;
                    this.driverSchedule.RepeatDayList = event.repeatDayList;
                    this.ConflictDateList = [];
                    this.IsShowConflictTable = false;
                    this.InitializeDates();

                    if (this.selectedType == 4 && event.repeatDayList != null && event.repeatDayList.length > 0) {
                      this.customDates = [];
                      chCustomDates = [];
                      days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
                      event.repeatDayList.forEach(function (x) {
                        var xdate = moment__WEBPACK_IMPORTED_MODULE_5__(x).toDate();
                        var dt = "".concat(moment__WEBPACK_IMPORTED_MODULE_5__(xdate).format('MM/DD/YYYY'), " (").concat(days[new Date(xdate).getDay()], ")");

                        var sdt = _this28.DateList.find(function (x) {
                          return x.Name == dt;
                        });

                        if (sdt != null) chCustomDates.push({
                          Id: sdt.Id,
                          Name: sdt.Name
                        });
                      });
                      this.customDates = chCustomDates;
                    }

                  case 51:
                  case "end":
                    return _context31.stop();
                }
              }
            }, _callee30, this, [[3, 15, 18, 28], [19,, 23, 27]]);
          }));
        }
      }, {
        key: "TypeCheck",
        value: function TypeCheck(event) {
          this.selectedType = event.typeId ? event.typeId : "1";
          this.repeat = event.repeatEvery ? event.repeatEvery : "1";
        }
      }, {
        key: "UpdateSchedule",
        value: function UpdateSchedule() {
          var e_5, _a, e_6, _b;

          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee31() {
            var _this29 = this;

            var selectedDates, ShiftId, scheduleList, newScheduleId, CurrentDriverShiftDetailList, driverScheduleMappingIndex, getCurrent, getIndex, rpDayList, _rpDayList, loop, cnt, newDate, olst, oDateList, oDateListString, oScheduleList, _c, _d, item, list, stringList, _e, _f, repeat, oShifScheduleList;

            return regeneratorRuntime.wrap(function _callee31$(_context32) {
              while (1) {
                switch (_context32.prev = _context32.next) {
                  case 0:
                    if (!(this.sdate && this.edate)) {
                      _context32.next = 5;
                      break;
                    }

                    this.driverSchedule.StartDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate().getTime());
                    this.driverSchedule.EndDate = new Date(moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate().getTime());
                    _context32.next = 7;
                    break;

                  case 5:
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Select valid dates', undefined, undefined);

                    return _context32.abrupt("return", false);

                  case 7:
                    if (!(moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate) > moment__WEBPACK_IMPORTED_MODULE_5__(this.edate))) {
                      _context32.next = 10;
                      break;
                    }

                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('From date cannot less than end date', undefined, undefined);

                    return _context32.abrupt("return", false);

                  case 10:
                    if (!(this.driverSchedule.selectedShifts.length < 0)) {
                      _context32.next = 13;
                      break;
                    }

                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror('Please select shift', undefined, undefined);

                    return _context32.abrupt("return", false);

                  case 13:
                    this.InitializeDates();

                    if (!(this.driverSchedule.selectedShifts.length == 0 || !this.driverSchedule.StartDate || !this.driverSchedule.EndDate || this.DateList.length == 0)) {
                      _context32.next = 18;
                      break;
                    }

                    return _context32.abrupt("return", false);

                  case 18:
                    selectedDates = [];
                    if (this.selectedType == 4) selectedDates = this.customDates;else selectedDates = this.DateList;
                    ShiftId = this.driverSchedule.selectedShifts[0].Id;
                    scheduleList = {};
                    newScheduleId = null;
                    this.driverScheduleMapping = this.DriverShiftDetailList.find(function (f) {
                      return f.Id == _this29.driverScheduleMapping.Id;
                    });
                    CurrentDriverShiftDetailList = this.DriverShiftDetailList.filter(function (f) {
                      return f.DriverId == _this29.SelectedDriver[0].Id;
                    });
                    driverScheduleMappingIndex = CurrentDriverShiftDetailList.findIndex(function (f) {
                      return f.Id == _this29.driverScheduleMapping.Id;
                    });

                    if (this.driverScheduleMapping != null && this.driverScheduleMapping.ScheduleList != null && this.driverScheduleMapping.ScheduleList.length > 0) {
                      getCurrent = this.driverScheduleMapping.ScheduleList.find(function (x) {
                        return x.Id == _this29.driverSchedule.Id && x.ShiftId == _this29.driverSchedule.ShiftId && x.IsActive;
                      });
                      getIndex = this.driverScheduleMapping.ScheduleList.findIndex(function (x) {
                        return x.Id == _this29.driverSchedule.Id && x.ShiftId == _this29.driverSchedule.ShiftId && x.IsActive;
                      }); //Update current one

                      if (this.driverSchedule.ShiftId != ShiftId && getCurrent != null && getCurrent.RepeatDayList != null) {
                        // 
                        rpDayList = [];
                        getCurrent.RepeatDayList.forEach(function (pre) {
                          var idx = selectedDates.findIndex(function (x) {
                            return moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(pre).format('MM/DD/YYYY');
                          });
                          if (idx < 0) rpDayList.push(pre);
                        });
                        getCurrent.RepeatDayList = [];
                        getCurrent.RepeatDayList = rpDayList;
                      } else {
                        if (getCurrent != null && getCurrent.RepeatDayList != null) {
                          _rpDayList = [];
                          loop = new Date(this.driverSchedule.StartDate);
                          cnt = 0;

                          while (loop <= this.driverSchedule.EndDate) {
                            newDate = loop.setDate(loop.getDate() + cnt);

                            if (!(newDate > this.driverSchedule.EndDate.setDate(this.driverSchedule.EndDate.getDate()))) {
                              getCurrent.RepeatDayList = getCurrent.RepeatDayList.filter(function (x) {
                                return moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') != moment__WEBPACK_IMPORTED_MODULE_5__(newDate).format('MM/DD/YYYY');
                              });

                              if (cnt != 1) {
                                cnt++;
                              }
                            }
                          }

                          selectedDates.forEach(function (element) {
                            return getCurrent.RepeatDayList.push("/Date(" + element.Id.setDate(element.Id.getDate()) + ")");
                          });
                          olst = [];
                          getCurrent.RepeatDayList.forEach(function (x) {
                            return olst.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('DD/MM/YYYY'));
                          });
                          olst.sort();
                          getCurrent.RepeatDayList = [];
                          getCurrent.RepeatDayList = olst;
                          olst = [];
                          getCurrent.RepeatDayList.forEach(function (x) {
                            var dt = x.split('/');
                            var oDt = dt[1] + '/' + dt[0] + '/' + dt[2];
                            var eDt = moment__WEBPACK_IMPORTED_MODULE_5__(oDt).toDate();
                            olst.push(eDt.setDate(eDt.getDate()));
                          });
                          getCurrent.RepeatDayList = olst;
                          olst = [];
                          getCurrent.RepeatDayList.forEach(function (x) {
                            return olst.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).toDate());
                          });
                          getCurrent.RepeatDayList = olst;
                        } else {
                          getCurrent.RepeatDayList = [];
                          selectedDates.forEach(function (element) {
                            return getCurrent.RepeatDayList.push(element.Id);
                          });
                        }
                      }

                      getCurrent.StartDate = getCurrent.RepeatDayList[0];
                      getCurrent.RepeatDayStringList = [];
                      getCurrent.RepeatDayList.forEach(function (x) {
                        getCurrent.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                      });
                      getCurrent.EndDate = getCurrent.RepeatDayList[getCurrent.RepeatDayList.length - 1];
                      delete this.driverScheduleMapping.ScheduleList[getIndex];
                      this.driverScheduleMapping.ScheduleList.splice(getIndex, 0, getCurrent); //Update current done
                      //start add new logic    

                      if (this.driverSchedule.ShiftId != ShiftId) {
                        oDateList = [];
                        oDateListString = [];

                        if (this.selectedType == 4) {
                          this.customDates.forEach(function (x) {
                            if (!_this29.ConflictDateList.some(function (item) {
                              return moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY');
                            })) {
                              oDateList.push(new Date(x.Id).getTime());
                            }
                          });
                        } else {
                          this.DateList.forEach(function (x) {
                            if (!_this29.ConflictDateList.some(function (item) {
                              return moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY');
                            })) {
                              oDateList.push(new Date(x.Id).getTime());
                            }
                          });
                        }

                        this.DateList.forEach(function (x) {
                          oDateListString.push(moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY'));
                        });
                        newScheduleId = "".concat(this.driverScheduleMapping.DriverId, "_").concat(new Date().getTime());
                        scheduleList = {
                          Id: "".concat(this.driverScheduleMapping.DriverId, "_").concat(new Date().getTime()),
                          IsActive: true,
                          StartDate: new Date(oDateList[0]).getTime(),
                          EndDate: new Date(oDateList[oDateList.length - 1]).getTime(),
                          RepeatDayList: oDateList,
                          RepeatDayStringList: oDateListString,
                          ShiftId: ShiftId,
                          RepeatEveryDay: this.repeat,
                          TypeId: this.selectedType
                        };
                        this.driverScheduleMapping.ScheduleList.push(scheduleList);
                      }
                    } // reset index


                    oScheduleList = [];
                    this.driverScheduleMapping.ScheduleList.forEach(function (e) {
                      oScheduleList.push(e);
                    });
                    this.driverScheduleMapping.ScheduleList = [];
                    this.driverScheduleMapping.ScheduleList = oScheduleList; //Check previous records exist or not for same shift with conflict dates 

                    this.driverScheduleMapping.ScheduleList.forEach(function (x) {
                      if (x.IsActive && x.ShiftId == ShiftId && newScheduleId != x.Id) {
                        var newList = [];

                        if (x.RepeatDayList != null && x.RepeatDayList.length > 0) {
                          x.RepeatDayList.forEach(function (y) {
                            if (!_this29.ConflictDateList.some(function (item) {
                              return moment__WEBPACK_IMPORTED_MODULE_5__(item).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(y).format('MM/DD/YYYY');
                            })) {
                              newList.push(moment__WEBPACK_IMPORTED_MODULE_5__(y).toDate());
                            }
                          });
                          x.RepeatDayList = newList;

                          if (x.RepeatDayList.length > 0) {
                            x.StartDate = x.RepeatDayList[0];
                            x.EndDate = x.RepeatDayList[x.RepeatDayList.length - 1];
                            x.RepeatDayStringList = [];
                            x.RepeatDayList.forEach(function (t) {
                              return x.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(t).format('MM/DD/YYYY'));
                            });
                          } else {
                            x.IsActive = false;
                          }
                        }
                      }
                    });
                    _context32.prev = 32;
                    _c = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList);

                  case 34:
                    _context32.next = 36;
                    return _c.next();

                  case 36:
                    _d = _context32.sent;

                    if (_d.done) {
                      _context32.next = 75;
                      break;
                    }

                    item = _d.value;

                    if (!(item.RepeatDayList != null && item.RepeatDayList.length > 0)) {
                      _context32.next = 73;
                      break;
                    }

                    item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                    item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                    list = [];
                    stringList = [];
                    item.RepeatDayStringList = [];
                    _context32.prev = 45;
                    _e = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList);

                  case 47:
                    _context32.next = 49;
                    return _e.next();

                  case 49:
                    _f = _context32.sent;

                    if (_f.done) {
                      _context32.next = 56;
                      break;
                    }

                    repeat = _f.value;
                    list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());

                    if (item.RepeatDayStringList == null || item.RepeatDayStringList.length == 0) {
                      stringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).format('MM/DD/YYYY'));
                    }

                  case 54:
                    _context32.next = 47;
                    break;

                  case 56:
                    _context32.next = 61;
                    break;

                  case 58:
                    _context32.prev = 58;
                    _context32.t0 = _context32["catch"](45);
                    e_6 = {
                      error: _context32.t0
                    };

                  case 61:
                    _context32.prev = 61;
                    _context32.prev = 62;

                    if (!(_f && !_f.done && (_b = _e["return"]))) {
                      _context32.next = 66;
                      break;
                    }

                    _context32.next = 66;
                    return _b.call(_e);

                  case 66:
                    _context32.prev = 66;

                    if (!e_6) {
                      _context32.next = 69;
                      break;
                    }

                    throw e_6.error;

                  case 69:
                    return _context32.finish(66);

                  case 70:
                    return _context32.finish(61);

                  case 71:
                    item.RepeatDayList = list;
                    item.RepeatDayStringList = stringList;

                  case 73:
                    _context32.next = 34;
                    break;

                  case 75:
                    _context32.next = 80;
                    break;

                  case 77:
                    _context32.prev = 77;
                    _context32.t1 = _context32["catch"](32);
                    e_5 = {
                      error: _context32.t1
                    };

                  case 80:
                    _context32.prev = 80;
                    _context32.prev = 81;

                    if (!(_d && !_d.done && (_a = _c["return"]))) {
                      _context32.next = 85;
                      break;
                    }

                    _context32.next = 85;
                    return _a.call(_c);

                  case 85:
                    _context32.prev = 85;

                    if (!e_5) {
                      _context32.next = 88;
                      break;
                    }

                    throw e_5.error;

                  case 88:
                    return _context32.finish(85);

                  case 89:
                    return _context32.finish(80);

                  case 90:
                    //reset Index of 
                    delete CurrentDriverShiftDetailList[driverScheduleMappingIndex];
                    CurrentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
                    oShifScheduleList = [];
                    CurrentDriverShiftDetailList.forEach(function (e) {
                      oShifScheduleList.push(e);
                    });
                    CurrentDriverShiftDetailList = [];
                    CurrentDriverShiftDetailList = oShifScheduleList; //End Reset Index of 

                    CurrentDriverShiftDetailList.forEach(function (ele) {
                      if (ele.Id != _this29.driverScheduleMapping.Id) {
                        ele.ScheduleList.forEach(function (pop) {
                          if (pop.IsActive && pop.ShiftId == ShiftId && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {
                            var _rpDayList2 = [];
                            pop.RepeatDayList.forEach(function (pre) {
                              var idx = selectedDates.findIndex(function (x) {
                                return moment__WEBPACK_IMPORTED_MODULE_5__(x.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(pre).format('MM/DD/YYYY');
                              });
                              if (idx < 0) _rpDayList2.push(pre);
                            });
                            pop.RepeatDayList = [];
                            pop.RepeatDayList = _rpDayList2;
                            pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                            pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                            pop.RepeatDayStringList = [];
                            pop.RepeatDayList.forEach(function (ab) {
                              return pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY'));
                            });
                          }

                          if (pop.RepeatDayList == null || pop.RepeatDayList.length == 0) pop.IsActive = false;else {
                            pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                            pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                            pop.RepeatDayStringList = [];
                            pop.RepeatDayList.forEach(function (ab) {
                              return pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY'));
                            });
                          }
                        });
                      }
                    }); //end

                    this.update(CurrentDriverShiftDetailList);

                  case 98:
                  case "end":
                    return _context32.stop();
                }
              }
            }, _callee31, this, [[32, 77, 80, 90], [45, 58, 61, 71], [62,, 66, 70], [81,, 85, 89]]);
          }));
        }
      }, {
        key: "delete",
        value: function _delete(model, sDate) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee32() {
            var _this30 = this;

            return regeneratorRuntime.wrap(function _callee32$(_context33) {
              while (1) {
                switch (_context33.prev = _context33.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(true);
                    this.regionService.deleteDriverSchedule(model, sDate).subscribe(function (response) {
                      if (response != null && response.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess('Driver Schedule deleted successfully', undefined, undefined);

                        setTimeout(function () {
                          var element = document.getElementById('idCloseModel');
                          element.click();
                        }, 2000);

                        _this30.regionService.onLoadingChanged.next(false);

                        _this30.refresh.next();

                        _this30.onDriverSelect();

                        _this30.setView(angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month);

                        _this30.deleteOption = 1;
                      } else {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);

                        setTimeout(function () {
                          var element = document.getElementById('idCloseModel');
                          element.click();
                        }, 1500);

                        _this30.regionService.onLoadingChanged.next(false);

                        _this30.refresh.next();

                        _this30.deleteOption = 1;

                        _this30.onDriverSelect();

                        _this30.setView(angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarView"].Month);
                      }

                      _this30.clearEditForm();
                    });
                    this.hideDeleteRange = false;
                    this.IsConfirmDelete = false;

                  case 4:
                  case "end":
                    return _context33.stop();
                }
              }
            }, _callee32, this);
          }));
        }
      }, {
        key: "update",
        value: function update(model, Isdelete) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee33() {
            var _this31 = this;

            return regeneratorRuntime.wrap(function _callee33$(_context34) {
              while (1) {
                switch (_context34.prev = _context34.next) {
                  case 0:
                    this.regionService.onLoadingChanged.next(true);
                    this.regionService.updateDriverSchedule(model, this.SelectedDate).subscribe(function (response) {
                      if (response != null && response.StatusCode == 0) {
                        _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgsuccess('Driver Schedule updated successfully', undefined, undefined);

                        if (!Isdelete) {
                          _this31.regionService.onLoadingChanged.next(false);

                          _this31.closeViewDayDetailModel();

                          _this31.onDriverSelect();

                          _this31.refresh.next();

                          _this31.deleteOption = 1;
                        } else {
                          _this31.IsConfirmDelete = false;
                          setTimeout(function () {
                            var element = document.getElementById('idCloseModel');
                            element.click();
                          }, 1500);

                          _this31.regionService.onLoadingChanged.next(false);

                          _this31.onDriverSelect();

                          _this31.refresh.next();

                          _this31.deleteOption = 1;
                        }
                      } else {
                        _this31.closeViewDayDetailModel();

                        _this31.regionService.onLoadingChanged.next(false);

                        _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);

                        _this31.refresh.next();
                      }

                      _this31.clearEditForm();
                    });

                  case 2:
                  case "end":
                    return _context34.stop();
                }
              }
            }, _callee33, this);
          }));
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this32 = this;

          this.regionService.onLoadingChanged.next(true);
          this.regionService.getRegions().subscribe(function (region) {
            _this32.regionList = region.Regions;

            _this32.regionService.onLoadingChanged.next(false);
          });
        }
      }, {
        key: "InitializeDates",
        value: function InitializeDates(sdate, end) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee34() {
            var days, dt;
            return regeneratorRuntime.wrap(function _callee34$(_context35) {
              while (1) {
                switch (_context35.prev = _context35.next) {
                  case 0:
                    this.driverSchedule.RepeatDayList = [];
                    this.DateList = [];
                    this.repeat = this.selectedType == 3 ? this.repeat : 0;
                    days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

                    if (this.sdate && this.edate) {
                      for (dt = new Date(this.sdate); dt <= new Date(this.edate); dt.setDate(dt.getDate() + this.repeat + 1)) {
                        if (this.selectedType && this.selectedType == 2) {
                          new Date(dt).getDay() != 0 && new Date(dt).getDay() != 6 ? this.DateList.push({
                            Id: new Date(dt),
                            Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                          }) : '';
                        } else {
                          this.DateList.push({
                            Id: new Date(dt),
                            Name: "".concat(moment__WEBPACK_IMPORTED_MODULE_5__(dt).format('MM/DD/YYYY'), " (").concat(days[new Date(dt).getDay()], ")")
                          });
                        }
                      }
                    }

                    for (dt = new Date(sdate); dt <= new Date(end); dt.setDate(dt.getDate() + 1)) {
                      this.driverSchedule.RepeatDayList.push(new Date(dt));
                    } //this.customDates = this.DateList;


                    this.validateShiftForDriver(this.driverSchedule.selectedShifts[0]);
                    return _context35.abrupt("return", this.DateList);

                  case 8:
                  case "end":
                    return _context35.stop();
                }
              }
            }, _callee34, this);
          }));
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.sdate = event;
          var d = moment__WEBPACK_IMPORTED_MODULE_5__(new Date(new Date().setFullYear(new Date().getFullYear() + 1))).toDate();
          !this.edate ? this.edate = moment__WEBPACK_IMPORTED_MODULE_5__(d).format('MM/DD/YYYY') : '';

          if (this.sdate != '' && this.edate != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate();

            if (_toDate < _fromDate) {
              this.edate = event;
            }
          }
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee35() {
            var _fromDate, _toDate;

            return regeneratorRuntime.wrap(function _callee35$(_context36) {
              while (1) {
                switch (_context36.prev = _context36.next) {
                  case 0:
                    this.IsUpdateForMultiple = false;
                    this.edate = event;

                    if (this.sdate != '' && this.edate != '') {
                      _fromDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.sdate).toDate();
                      _toDate = moment__WEBPACK_IMPORTED_MODULE_5__(this.edate).toDate();

                      if (_fromDate > _toDate) {
                        this.sdate = event;
                      }

                      if (_toDate > _fromDate) {
                        this.IsUpdateForMultiple = true;
                      }
                    }

                    this.InitializeDates();

                  case 4:
                  case "end":
                    return _context36.stop();
                }
              }
            }, _callee35, this);
          }));
        }
      }, {
        key: "closeViewDayDetailModel",
        value: function closeViewDayDetailModel() {
          this.isShowEditPannel = false;
          this.startDateEnable = false; // validate from date if it is less than current date.

          this.isApplyAll = false;
          this.driverScheduleMapping = {};
          this.driverSchedule = {};
          this._openedEditPanel = false;
          var element = document.getElementById('idCloseModel');
          element.click();
        }
      }, {
        key: "getShortDriverName",
        value: function getShortDriverName(name) {
          var fullName = name.split(' ');
          var lastName = fullName.pop();
          var firstName = fullName.join(' ');
          return firstName.substring(0, 1) + " " + lastName;
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
          }
        }
      }, {
        key: "_editToggleOpened",
        value: function _editToggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._openedEditPanel = true;
          } else {
            this._openedEditPanel = !this._openedEditPanel;
          }

          this.clearEditForm();
        }
      }, {
        key: "clearEditForm",
        value: function clearEditForm() {
          this.Selectedregion = [];
          this.SelectedDriver = [];
          this.driverSchedule.selectedShifts = [];
          this.startDateEnable = false;
          this.eToDate = null;
          this.eFromDate = null;
          this.IsUpdateForMultiple = false;
          this.selectedType = 0;
          this.customDates = [];
          this.repeat = 0;
          this.IsShiftRepeted = false;
          this.IsSheduleEdit = false;
          this.SelectedDate = new Date();
          this.ConflictDateList = [];
          this.IsShowConflictTable = false;
        }
      }, {
        key: "changeIsApplyAll",
        value: function changeIsApplyAll() {
          this.isApplyAll = !this.isApplyAll;
          this.selectedType = 1;
          this.InitializeDates();
        }
      }, {
        key: "validateShiftForDriver",
        value: function validateShiftForDriver(event) {
          var _this33 = this;

          var DaysRepeateCount = 1;
          this.ConflictDateList = [];
          this.IsShowConflictTable = false;
          this.IsShiftRepeted = false;

          if (this.sdate && this.edate) {
            var CheckConflictDays = this.DriverShiftDetailList.filter(function (f) {
              return f.DriverId == _this33.DriverId;
            });

            if (CheckConflictDays != null && CheckConflictDays.length > 0) {
              var selecteddateList = [];

              if (this.selectedType == 4) {
                selecteddateList = this.customDates;
              } else {
                selecteddateList = this.DateList;
              }

              CheckConflictDays.forEach(function (ShiftDetails) {
                if (ShiftDetails.ScheduleList != null) {
                  if (selecteddateList.length > 0) {
                    ShiftDetails.ScheduleList.forEach(function (elm) {
                      if (elm.ShiftId == event.Id && elm.ShiftId != _this33.driverSchedule.ShiftId) {
                        if (elm.RepeatDayList != null && elm.RepeatDayList.length > 0) {
                          elm.RepeatDayList.forEach(function (dte) {
                            selecteddateList.forEach(function (slDate) {
                              if (moment__WEBPACK_IMPORTED_MODULE_5__(slDate.Id).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(dte).format('MM/DD/YYYY')) {
                                _this33.ConflictDateList.push(moment__WEBPACK_IMPORTED_MODULE_5__(slDate.Id).format('MM/DD/YYYY'));

                                DaysRepeateCount++;
                              }
                            });
                          });
                        }
                      }
                    });
                  }
                }
              });
            }
          } else {
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please select dates first', undefined, undefined);

            return true;
          }

          if (this.ConflictDateList.length > 0) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning("Following shifts are already assigned to the drive", undefined, undefined);
          }
        }
      }, {
        key: "isRequired",
        value: function isRequired(name) {
          if (name == "" || name == null) {
            return true;
          } else {
            return false;
          }
        }
      }, {
        key: "closeDeleteModel",
        value: function closeDeleteModel() {
          this.IsConfirmDelete = false;
        }
      }, {
        key: "RemoveSchedule",
        value: function RemoveSchedule(event) {
          var e_7, _a, e_8, _b, e_9, _c;

          return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee36() {
            var _this34 = this;

            var currentDelete, currentDriverShiftDetailList, driverScheduleMappingIndex, driverShiftMapping, sList, driverShiftMapping_1, driverShiftMapping_1_1, oSchedule, _d, _e, item, list, _f, _g, repeat, oShifScheduleList;

            return regeneratorRuntime.wrap(function _callee36$(_context37) {
              while (1) {
                switch (_context37.prev = _context37.next) {
                  case 0:
                    _context37.next = 2;
                    return this.DriverShiftDetailList.find(function (f) {
                      return f.Id == event.mappingId;
                    });

                  case 2:
                    this.driverScheduleMapping = _context37.sent;
                    _context37.next = 5;
                    return this.DriverShiftDetailList.filter(function (f) {
                      return f.DriverId == _this34.DriverId;
                    });

                  case 5:
                    currentDriverShiftDetailList = _context37.sent;
                    _context37.next = 8;
                    return currentDriverShiftDetailList.findIndex(function (f) {
                      return f.Id == event.mappingId;
                    });

                  case 8:
                    driverScheduleMappingIndex = _context37.sent;
                    _context37.next = 11;
                    return this.DriverShiftDetailList.filter(function (f) {
                      return f.DriverId == _this34.DriverId;
                    });

                  case 11:
                    driverShiftMapping = _context37.sent;

                    if (!(this.deleteOption != "" && this.deleteOption != null)) {
                      _context37.next = 111;
                      break;
                    }

                    if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Single) {
                      this.driverScheduleMapping.ScheduleList.forEach(function (f) {
                        if (f.IsActive = true && f.Id == _this34.driverSchedule.Id && f.ShiftId == _this34.driverSchedule.ShiftId) {
                          if (f.RepeatDayList != null && f.RepeatDayList.length > 0) {
                            currentDelete = f;

                            if (f.RepeatDayList.length == 1) {
                              delete f.RepeatDayList[0];
                              f.RepeatDayList = null;
                            } else {
                              var indexof = f.RepeatDayList.findIndex(function (x) {
                                return moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') == moment__WEBPACK_IMPORTED_MODULE_5__(_this34.sdate).format('MM/DD/YYYY');
                              });
                              delete f.RepeatDayList[indexof];
                              var reOrder = [];
                              f.RepeatDayList.forEach(function (r) {
                                reOrder.push(r);
                              });
                              f.RepeatDayList = reOrder;
                              f.StartDate = f.RepeatDayList[0];
                              f.EndDate = f.RepeatDayList[f.RepeatDayList.length - 1];
                              f.RepeatDayStringList = [];
                              f.RepeatDayList.forEach(function (x) {
                                f.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                              });
                            }
                          }
                        }
                      });
                      this.IsConfirmDelete = false;
                    }

                    if (this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Range) {
                      if (this.driverScheduleMapping.ScheduleList.length > 0) {
                        sList = this.driverScheduleMapping.ScheduleList.forEach(function (res) {
                          if (res.Id == _this34.driverSchedule.Id) {
                            var _res$RepeatDayList$fi, _res$RepeatDayList$fi2;

                            var reOrder = (_res$RepeatDayList$fi = res.RepeatDayList.filter(function (x) {
                              return moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') < moment__WEBPACK_IMPORTED_MODULE_5__(_this34.sdate).format('MM/DD/YYYY');
                            }), _res$RepeatDayList$fi2 = _toArray(_res$RepeatDayList$fi), _res$RepeatDayList$fi);
                            res.RepeatDayList = reOrder;
                            res.StartDate = res.RepeatDayList[0];
                            res.EndDate = res.RepeatDayList[res.RepeatDayList.length - 1];
                            res.RepeatDayStringList = [];
                            res.RepeatDayList.forEach(function (x) {
                              res.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                            });
                          }
                        });
                      }

                      this.IsConfirmDelete = false;
                    }

                    if (!(this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Whole)) {
                      _context37.next = 45;
                      break;
                    }

                    if (!(driverShiftMapping.length > 0)) {
                      _context37.next = 45;
                      break;
                    }

                    _context37.prev = 17;
                    driverShiftMapping_1 = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(driverShiftMapping);

                  case 19:
                    _context37.next = 21;
                    return driverShiftMapping_1.next();

                  case 21:
                    driverShiftMapping_1_1 = _context37.sent;

                    if (driverShiftMapping_1_1.done) {
                      _context37.next = 27;
                      break;
                    }

                    oSchedule = driverShiftMapping_1_1.value;

                    if (oSchedule.ScheduleList.length > 0) {
                      oSchedule.ScheduleList.forEach(function (ele) {
                        if (ele.RepeatDayList != null) {
                          var _ele$RepeatDayList$fi, _ele$RepeatDayList$fi2;

                          var reOrder = (_ele$RepeatDayList$fi = ele.RepeatDayList.filter(function (x) {
                            return moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY') < moment__WEBPACK_IMPORTED_MODULE_5__(_this34.sdate).format('MM/DD/YYYY');
                          }), _ele$RepeatDayList$fi2 = _toArray(_ele$RepeatDayList$fi), _ele$RepeatDayList$fi);
                          ele.RepeatDayList = reOrder;
                          ele.StartDate = ele.RepeatDayList[0];
                          ele.EndDate = ele.RepeatDayList[ele.RepeatDayList.length - 1];
                          ele.RepeatDayStringList = [];
                          ele.RepeatDayList.forEach(function (x) {
                            ele.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(x).format('MM/DD/YYYY'));
                          });
                        }
                      });
                    }

                  case 25:
                    _context37.next = 19;
                    break;

                  case 27:
                    _context37.next = 32;
                    break;

                  case 29:
                    _context37.prev = 29;
                    _context37.t0 = _context37["catch"](17);
                    e_7 = {
                      error: _context37.t0
                    };

                  case 32:
                    _context37.prev = 32;
                    _context37.prev = 33;

                    if (!(driverShiftMapping_1_1 && !driverShiftMapping_1_1.done && (_a = driverShiftMapping_1["return"]))) {
                      _context37.next = 37;
                      break;
                    }

                    _context37.next = 37;
                    return _a.call(driverShiftMapping_1);

                  case 37:
                    _context37.prev = 37;

                    if (!e_7) {
                      _context37.next = 40;
                      break;
                    }

                    throw e_7.error;

                  case 40:
                    return _context37.finish(37);

                  case 41:
                    return _context37.finish(32);

                  case 42:
                    _context37.next = 44;
                    return driverShiftMapping.forEach(function (element) {
                      var _iterator2 = _createForOfIteratorHelper(element.ScheduleList),
                          _step2;

                      try {
                        for (_iterator2.s(); !(_step2 = _iterator2.n()).done;) {
                          var item = _step2.value;

                          if (item.RepeatDayList != null && item.RepeatDayList.length > 0) {
                            item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                            item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                            var list = [];

                            var _iterator3 = _createForOfIteratorHelper(item.RepeatDayList),
                                _step3;

                            try {
                              for (_iterator3.s(); !(_step3 = _iterator3.n()).done;) {
                                var repeat = _step3.value;
                                list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());
                              }
                            } catch (err) {
                              _iterator3.e(err);
                            } finally {
                              _iterator3.f();
                            }

                            item.RepeatDayList = list;
                          }
                        }
                      } catch (err) {
                        _iterator2.e(err);
                      } finally {
                        _iterator2.f();
                      }
                    });

                  case 44:
                    this["delete"](driverShiftMapping, this.SelectedDate);

                  case 45:
                    if (!(this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Range || this.deleteOption == src_app_app_enum__WEBPACK_IMPORTED_MODULE_7__["DeleteDriverSchedule"].Single)) {
                      _context37.next = 109;
                      break;
                    }

                    _context37.prev = 46;
                    _d = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(this.driverScheduleMapping.ScheduleList);

                  case 48:
                    _context37.next = 50;
                    return _d.next();

                  case 50:
                    _e = _context37.sent;

                    if (_e.done) {
                      _context37.next = 85;
                      break;
                    }

                    item = _e.value;

                    if (!(item.RepeatDayList != null && item.RepeatDayList.length > 0)) {
                      _context37.next = 83;
                      break;
                    }

                    item.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[0]).toDate();
                    item.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(item.RepeatDayList[item.RepeatDayList.length - 1]).toDate();
                    list = [];
                    _context37.prev = 57;
                    _f = Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__asyncValues"])(item.RepeatDayList);

                  case 59:
                    _context37.next = 61;
                    return _f.next();

                  case 61:
                    _g = _context37.sent;

                    if (_g.done) {
                      _context37.next = 67;
                      break;
                    }

                    repeat = _g.value;
                    list.push(moment__WEBPACK_IMPORTED_MODULE_5__(repeat).toDate());

                  case 65:
                    _context37.next = 59;
                    break;

                  case 67:
                    _context37.next = 72;
                    break;

                  case 69:
                    _context37.prev = 69;
                    _context37.t1 = _context37["catch"](57);
                    e_9 = {
                      error: _context37.t1
                    };

                  case 72:
                    _context37.prev = 72;
                    _context37.prev = 73;

                    if (!(_g && !_g.done && (_c = _f["return"]))) {
                      _context37.next = 77;
                      break;
                    }

                    _context37.next = 77;
                    return _c.call(_f);

                  case 77:
                    _context37.prev = 77;

                    if (!e_9) {
                      _context37.next = 80;
                      break;
                    }

                    throw e_9.error;

                  case 80:
                    return _context37.finish(77);

                  case 81:
                    return _context37.finish(72);

                  case 82:
                    item.RepeatDayList = list;

                  case 83:
                    _context37.next = 48;
                    break;

                  case 85:
                    _context37.next = 90;
                    break;

                  case 87:
                    _context37.prev = 87;
                    _context37.t2 = _context37["catch"](46);
                    e_8 = {
                      error: _context37.t2
                    };

                  case 90:
                    _context37.prev = 90;
                    _context37.prev = 91;

                    if (!(_e && !_e.done && (_b = _d["return"]))) {
                      _context37.next = 95;
                      break;
                    }

                    _context37.next = 95;
                    return _b.call(_d);

                  case 95:
                    _context37.prev = 95;

                    if (!e_8) {
                      _context37.next = 98;
                      break;
                    }

                    throw e_8.error;

                  case 98:
                    return _context37.finish(95);

                  case 99:
                    return _context37.finish(90);

                  case 100:
                    delete currentDriverShiftDetailList[driverScheduleMappingIndex];
                    currentDriverShiftDetailList.splice(driverScheduleMappingIndex, 0, this.driverScheduleMapping);
                    oShifScheduleList = [];
                    currentDriverShiftDetailList.forEach(function (e) {
                      oShifScheduleList.push(e);
                    });
                    this.DriverShiftDetailList = [];
                    currentDriverShiftDetailList = oShifScheduleList; //End Reset Index of 

                    currentDriverShiftDetailList.forEach(function (ele) {
                      if (ele.Id != _this34.driverScheduleMapping.Id) {
                        ele.ScheduleList.forEach(function (pop) {
                          if (pop.IsActive && pop.RepeatDayList != null && pop.RepeatDayList.length > 0) {
                            pop.StartDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[0]).toDate();
                            pop.EndDate = moment__WEBPACK_IMPORTED_MODULE_5__(pop.RepeatDayList[pop.RepeatDayList.length - 1]).toDate();
                            pop.RepeatDayStringList = [];
                            pop.RepeatDayList.forEach(function (ab) {
                              return pop.RepeatDayStringList.push(moment__WEBPACK_IMPORTED_MODULE_5__(ab).format('MM/DD/YYYY'));
                            });
                          } else pop.IsActive = false;
                        });
                      }
                    });
                    this.update(currentDriverShiftDetailList, true);
                    return _context37.abrupt("return", true);

                  case 109:
                    _context37.next = 113;
                    break;

                  case 111:
                    _declarations_module__WEBPACK_IMPORTED_MODULE_6__["Declarations"].msgwarning('Please select delete option', undefined, undefined);

                    return _context37.abrupt("return", false);

                  case 113:
                  case "end":
                    return _context37.stop();
                }
              }
            }, _callee36, this, [[17, 29, 32, 42], [33,, 37, 41], [46, 87, 90, 100], [57, 69, 72, 82], [73,, 77, 81], [91,, 95, 99]]);
          }));
        }
      }, {
        key: "showDriverConflictSchedules",
        value: function showDriverConflictSchedules() {
          this.IsShowConflictTable = !this.IsShowConflictTable;
        }
      }]);

      return DriverScheduleCalenderComponent;
    }();

    DriverScheduleCalenderComponent.ɵfac = function DriverScheduleCalenderComponent_Factory(t) {
      return new (t || DriverScheduleCalenderComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__["RouteInfoService"]));
    };

    DriverScheduleCalenderComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: DriverScheduleCalenderComponent,
      selectors: [["app-driver-schedule-calender"]],
      decls: 39,
      vars: 19,
      consts: [[1, "row", "justify-content-between"], [1, "col-sm-2"], [1, "form-group", "mb10"], [3, "placeholder", "settings", "data", "ngModel", "onSelect", "onDeSelect", "ngModelChange"], [1, "col-sm-6", 2, "display", "none"], [1, "form-group"], [1, "radio-inline"], ["type", "radio", "name", "scheduleType", "ng-control", "scheduleType", 3, "ngModel", "value", "ngModelChange", "change"], ["type", "radio", "name", "scheduleType", "ng-control", "scheduleType", "value", "2", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-3", "float-right"], [3, "OnScheduleAdded"], ["class", "row", 4, "ngIf"], ["customCellTemplate", ""], ["id", "idViewDayDetail", "role", "dialog", "data-backdrop", "static", 1, "modal", "fade"], [1, "modal-dialog", "modal-lg"], [1, "modal-content"], [1, "modal-header"], [1, "modal-title"], [1, "pull-right"], [1, "modal-body"], ["class", "table table-bordered", 4, "ngIf"], ["class", "form-group col-md-12", 4, "ngIf"], ["class", "modal-footer", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], [1, "col-md-3", "text-center"], [1, "col-md-5", "text-center"], ["mwlCalendarPreviousView", "", 1, "btn", "btn-default", "btn-xs", 3, "view", "viewDate", "viewDateChange", "click"], [1, "fas", "fa-arrow-left"], ["id", "idToday", "mwlCalendarToday", "", 1, "btn", "btn-default", "btn-xs", 3, "viewDate", "viewDateChange", "click"], ["mwlCalendarNextView", "", 1, "btn", "btn-default", "btn-xs", 3, "view", "viewDate", "viewDateChange", "click"], [1, "fas", "fa-arrow-right"], [1, "col-md-4", "text-right"], ["id", "idMonth", 1, "btn", 3, "ngClass", "click"], ["for", "idMonth"], ["id", "idWeek", 1, "btn", 3, "ngClass", "click"], ["for", "idWeek"], ["id", "idDay", 1, "btn", 3, "ngClass", "click"], ["for", "idDay"], [3, "ngSwitch"], ["type", "button", "id", "idViewDay", "hidden", "", "data-toggle", "modal", "data-target", "#idViewDayDetail"], [3, "viewDate", "events", "cellTemplate", "refresh", "activeDayIsOpen", "dayClicked", 4, "ngSwitchCase"], [3, "viewDate", "events", "refresh", 4, "ngSwitchCase"], [3, "viewDate", "events", "cellTemplate", "refresh", "activeDayIsOpen", "dayClicked"], [3, "viewDate", "events", "refresh"], [1, "cal-cell-top"], [1, "table", "table-hover"], [4, "ngFor", "ngForOf"], [1, "cal-day-number"], [1, "cell-totals"], [3, "class", 4, "ngFor", "ngForOf"], ["style", "color:black", "class", "label  calender-grid", 3, "ngStyle", 4, "ngIf"], [1, "label", "calender-grid", 2, "color", "black", 3, "ngStyle"], [1, "table", "table-bordered"], [3, "ngStyle"], [1, "fas", "fa-edit", "icon-zoom", "btn-primary", "label-font", 3, "click"], [1, "fas", "fa-trash-alt", "color-maroon", "icon-zoom", "label-font", 3, "click"], [1, "form-group", "col-md-12"], [1, "row", "col-md-12"], [1, "form-control"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsSingle", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsSingle", 1, "form-check-label"], ["class", "row col-md-12", 4, "ngIf"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsEntire", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsEntire", 1, "form-check-label"], [1, "modal-footer"], ["type", "button", "id", "closeDelete", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], ["type", "button", "id", "deleteSchedule", 1, "btn", "btn-default", 3, "click"], ["type", "radio", "name", "deleteOptions", "id", "deleteOptionsRange", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "deleteOptionsRange", 1, "form-check-label"], ["type", "button", "id", "idCloseModel", "data-dismiss", "modal", 1, "btn", "btn-default", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [2, "z-index", "99999"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], [1, "dib", "ml10", "mt10", "mb10"], ["autocomplete", "off", 1, "pr30", 3, "keydown.enter"], [1, "col-sm-6"], [1, "form-group", "readonly"], ["for", "Region"], [3, "placeholder", "settings", "data", "ngModel", "disabled", "ngModelChange"], ["for", "Drivers"], ["for", "Shift"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange", "onSelect"], ["class", "form-group", 4, "ngIf"], ["for", "ToDate"], ["required", "", "type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], [1, "row", "form-group"], ["class", "col-sm-12", 4, "ngIf"], [1, "col-sm-12", "row-fluid", "text-right", "form-buttons"], [1, "text-right"], ["type", "button", "value", "Cancel", 1, "btn", "btn-default", 3, "click"], ["type", "button", "id", "Submit", 1, "btn", "btn-primary", 3, "click"], ["for", "fromDate"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "readonly", "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "minDate", "maxDate", "ngModelChange", "onDateChange"], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["type", "radio", "type", "radio", "name", "selectedTypes", "id", "inlineRadioDaily", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioDaily", 1, "form-check-label"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioWdays", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioWdays", 1, "form-check-label"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioEvery", 1, "form-check-input", 3, "value", "ngModel", "ngModelChange"], ["for", "inlineRadioEvery", 1, "form-check-label"], ["class", "form-check form-check-inline", 4, "ngIf"], ["type", "radio", "name", "selectedTypes", "id", "inlineRadioCustom", 1, "form-check-input", 3, "value", "ngModel", "change", "ngModelChange"], ["for", "inlineRadioCustom", 1, "form-check-label"], ["for", "Dates"], [3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Days"], ["type", "number", "placeholder", "days", "min", "1", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "alert", "alert-warning", "fs12", "mt10", "mb0", "radius-10"], [1, "fas", "fa-exclamation-circle", "mr5"], ["class", "table table-striped table-bordered table-hover", 4, "ngIf"], [1, "table", "table-striped", "table-bordered", "table-hover"]],
      template: function DriverScheduleCalenderComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "ng-multiselect-dropdown", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_onSelect_4_listener($event) {
            return ctx.onDriverSelect($event);
          })("onDeSelect", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_onDeSelect_4_listener($event) {
            return ctx.onDriverDeSelect($event);
          })("ngModelChange", function DriverScheduleCalenderComponent_Template_ng_multiselect_dropdown_ngModelChange_4_listener($event) {
            return ctx.SelectedDriverList = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_9_listener($event) {
            return ctx.scheduleType = $event;
          })("change", function DriverScheduleCalenderComponent_Template_input_change_9_listener() {
            return ctx.onDriverSelect();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "All");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_13_listener($event) {
            return ctx.scheduleType = $event;
          })("change", function DriverScheduleCalenderComponent_Template_input_change_13_listener() {
            return ctx.onDriverSelect();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](14, "Planned Schedule");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DriverScheduleCalenderComponent_Template_input_ngModelChange_17_listener($event) {
            return ctx.scheduleType = $event;
          })("change", function DriverScheduleCalenderComponent_Template_input_change_17_listener() {
            return ctx.onDriverSelect();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "UnPlanned Schedule ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "create-driver-schedule", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("OnScheduleAdded", function DriverScheduleCalenderComponent_Template_create_driver_schedule_OnScheduleAdded_20_listener($event) {
            return ctx.OnScheduleAdded($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](21, DriverScheduleCalenderComponent_div_21_Template, 34, 23, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, DriverScheduleCalenderComponent_ng_template_22_Template, 11, 8, "ng-template", null, 12, _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "h4", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "span", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](33, DriverScheduleCalenderComponent_table_33_Template, 17, 1, "table", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](34, DriverScheduleCalenderComponent_div_34_Template, 20, 5, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](35, DriverScheduleCalenderComponent_div_35_Template, 3, 0, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](36, DriverScheduleCalenderComponent_div_36_Template, 5, 0, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](37, "async");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](38, DriverScheduleCalenderComponent_div_38_Template, 46, 27, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Driver(s)")("settings", ctx.multiselectSettingsById)("data", ctx.DriverList)("ngModel", ctx.SelectedDriverList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.scheduleType)("value", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.scheduleType)("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.scheduleType);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.isShowCalender);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", ctx.SelectedDate, " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.driverScheduleMapping.DriverName);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx.isShowEditPannel && !ctx.IsConfirmDelete);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsConfirmDelete);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx.IsConfirmDelete);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](37, 17, ctx.regionService.onLoadingChanged));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsSheduleEdit);
        }
      },
      directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_10__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["DefaultValueAccessor"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverScheduleComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgIf"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["ɵf"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["ɵh"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["ɵg"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgSwitch"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgSwitchCase"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarMonthViewComponent"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarWeekViewComponent"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["CalendarDayViewComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_13__["NgStyle"], ng_sidebar__WEBPACK_IMPORTED_MODULE_14__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_14__["Sidebar"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["RequiredValidator"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_15__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["NumberValueAccessor"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_13__["AsyncPipe"], angular_calendar__WEBPACK_IMPORTED_MODULE_4__["ɵi"]],
      styles: [".calender-grid[_ngcontent-%COMP%]{\r\n    font-size:10px;\r\n}\r\n.blueColor[_ngcontent-%COMP%] {\r\n    color: blue;\r\n    font-size: 20px;\r\n}\r\n.cal-month-view[_ngcontent-%COMP%]   .cal-day-badge[_ngcontent-%COMP%] {\r\n    background-color: #9db948;\r\n    color: #fff;\r\n    margin-bottom: 5px;\r\n}\r\n.icon-zoom[_ngcontent-%COMP%] {\r\n    padding: 5px;\r\n    transition: transform .2s; \r\n    margin: 0 auto;\r\n}\r\n.icon-zoom[_ngcontent-%COMP%]:hover {\r\n        transform: scale(1.2);\r\n    }\r\n.label-font[_ngcontent-%COMP%]{\r\n        font-size:15px;\r\n    }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvZHJpdmVyL2RyaXZlci1zY2hlZHVsZS1jYWxlbmRlci9kcml2ZXItc2NoZWR1bGUtY2FsZW5kZXIuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGNBQWM7QUFDbEI7QUFDQTtJQUNJLFdBQVc7SUFDWCxlQUFlO0FBQ25CO0FBQ0E7SUFDSSx5QkFBeUI7SUFDekIsV0FBVztJQUNYLGtCQUFrQjtBQUN0QjtBQUVBO0lBQ0ksWUFBWTtJQUNaLHlCQUF5QixFQUFFLGNBQWM7SUFDekMsY0FBYztBQUNsQjtBQUVJO1FBQ0kscUJBQXFCO0lBQ3pCO0FBRUE7UUFDSSxjQUFjO0lBQ2xCIiwiZmlsZSI6InNyYy9hcHAvZHJpdmVyL2RyaXZlci1zY2hlZHVsZS1jYWxlbmRlci9kcml2ZXItc2NoZWR1bGUtY2FsZW5kZXIuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5jYWxlbmRlci1ncmlke1xyXG4gICAgZm9udC1zaXplOjEwcHg7XHJcbn1cclxuLmJsdWVDb2xvciB7XHJcbiAgICBjb2xvcjogYmx1ZTtcclxuICAgIGZvbnQtc2l6ZTogMjBweDtcclxufVxyXG4uY2FsLW1vbnRoLXZpZXcgLmNhbC1kYXktYmFkZ2Uge1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogIzlkYjk0ODtcclxuICAgIGNvbG9yOiAjZmZmO1xyXG4gICAgbWFyZ2luLWJvdHRvbTogNXB4O1xyXG59XHJcblxyXG4uaWNvbi16b29tIHtcclxuICAgIHBhZGRpbmc6IDVweDtcclxuICAgIHRyYW5zaXRpb246IHRyYW5zZm9ybSAuMnM7IC8qIEFuaW1hdGlvbiAqL1xyXG4gICAgbWFyZ2luOiAwIGF1dG87XHJcbn1cclxuXHJcbiAgICAuaWNvbi16b29tOmhvdmVyIHtcclxuICAgICAgICB0cmFuc2Zvcm06IHNjYWxlKDEuMik7XHJcbiAgICB9XHJcblxyXG4gICAgLmxhYmVsLWZvbnR7XHJcbiAgICAgICAgZm9udC1zaXplOjE1cHg7XHJcbiAgICB9Il19 */"],
      changeDetection: 0
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](DriverScheduleCalenderComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-driver-schedule-calender',
          templateUrl: './driver-schedule-calender.component.html',
          changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_1__["ChangeDetectionStrategy"].OnPush,
          styleUrls: ['./driver-schedule-calender.component.css']
        }]
      }], function () {
        return [{
          type: src_app_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_8__["RegionService"]
        }, {
          type: src_app_carrier_service_route_info_service__WEBPACK_IMPORTED_MODULE_9__["RouteInfoService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver.module.ts": function srcAppDriverDriverModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverModule", function () {
      return DriverModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _driver_routing_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./driver-routing.module */
    "./src/app/driver/driver-routing.module.ts");
    /* harmony import */


    var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./driver-schedule-calender/driver-schedule-calender.component */
    "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");
    /* harmony import */


    var angular_calendar__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-calendar */
    "./node_modules/angular-calendar/__ivy_ngcc__/fesm2015/angular-calendar.js");
    /* harmony import */


    var angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-calendar/date-adapters/date-fns */
    "./node_modules/angular-calendar/date-adapters/date-fns/index.js");
    /* harmony import */


    var angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__);
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! angularx-flatpickr */
    "./node_modules/angularx-flatpickr/__ivy_ngcc__/fesm2015/angularx-flatpickr.js");
    /* harmony import */


    var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./driver-management/driver-management.component */
    "./src/app/driver/driver-management/driver-management.component.ts");
    /* harmony import */


    var _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./create-driver-schedule/create-driver-schedule.component */
    "./src/app/driver/create-driver-schedule/create-driver-schedule.component.ts");
    /* harmony import */


    var _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./driver/driver.component */
    "./src/app/driver/driver/driver.component.ts");
    /* harmony import */


    var _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./driver-management/view-driver/view-driver.component */
    "./src/app/driver/driver-management/view-driver/view-driver.component.ts");
    /* harmony import */


    var _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./driver-management/create-driver/create-driver.component */
    "./src/app/driver/driver-management/create-driver/create-driver.component.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ./create-trailer-schedule/create-trailer-schedule.component */
    "./src/app/driver/create-trailer-schedule/create-trailer-schedule.component.ts");
    /* harmony import */


    var _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ./create-region-schedule/create-region.component */
    "./src/app/driver/create-region-schedule/create-region.component.ts");

    var DriverModule = function DriverModule() {
      _classCallCheck(this, DriverModule);
    };

    DriverModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: DriverModule
    });
    DriverModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function DriverModule_Factory(t) {
        return new (t || DriverModule)();
      },
      imports: [[_driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"], angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"].forRoot(), angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"].forRoot({
        provide: angular_calendar__WEBPACK_IMPORTED_MODULE_3__["DateAdapter"],
        useFactory: angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__["adapterFactory"]
      })]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](DriverModule, {
        declarations: [_driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__["DriverManagementComponent"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__["CreateDriverScheduleComponent"], _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__["DriverComponent"], _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__["ViewDriverComponent"], _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverComponent"], _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__["CreateTrailerScheduleComponent"], _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__["CreateRegionComponent"]],
        imports: [_driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"], angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"], angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DriverModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_2__["DriverScheduleCalenderComponent"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_8__["DriverManagementComponent"], _create_driver_schedule_create_driver_schedule_component__WEBPACK_IMPORTED_MODULE_9__["CreateDriverScheduleComponent"], _driver_driver_component__WEBPACK_IMPORTED_MODULE_10__["DriverComponent"], _driver_management_view_driver_view_driver_component__WEBPACK_IMPORTED_MODULE_11__["ViewDriverComponent"], _driver_management_create_driver_create_driver_component__WEBPACK_IMPORTED_MODULE_12__["CreateDriverComponent"], _create_trailer_schedule_create_trailer_schedule_component__WEBPACK_IMPORTED_MODULE_15__["CreateTrailerScheduleComponent"], _create_region_schedule_create_region_component__WEBPACK_IMPORTED_MODULE_16__["CreateRegionComponent"]],
          imports: [_driver_routing_module__WEBPACK_IMPORTED_MODULE_1__["DriverScheduleRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_6__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_13__["DirectiveModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_5__["NgbModalModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_14__["DataTablesModule"], angularx_flatpickr__WEBPACK_IMPORTED_MODULE_7__["FlatpickrModule"].forRoot(), angular_calendar__WEBPACK_IMPORTED_MODULE_3__["CalendarModule"].forRoot({
            provide: angular_calendar__WEBPACK_IMPORTED_MODULE_3__["DateAdapter"],
            useFactory: angular_calendar_date_adapters_date_fns__WEBPACK_IMPORTED_MODULE_4__["adapterFactory"]
          })]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/driver/driver/driver.component.ts": function srcAppDriverDriverDriverComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverComponent", function () {
      return DriverComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../../company-addresses/region/service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../driver-management/driver-management.component */
    "./src/app/driver/driver-management/driver-management.component.ts");
    /* harmony import */


    var _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../driver-schedule-calender/driver-schedule-calender.component */
    "./src/app/driver/driver-schedule-calender/driver-schedule-calender.component.ts");

    function DriverComponent_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-driver-management");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DriverComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-driver-schedule-calender");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DriverComponent_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var DriverComponent = /*#__PURE__*/function () {
      function DriverComponent(regionService) {
        _classCallCheck(this, DriverComponent);

        this.regionService = regionService;
        this.isDriverShow = true;
        this.isProfileShow = false;
      }

      _createClass(DriverComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "changeTab",
        value: function changeTab(tabClick) {
          this.isDriverShow = false;
          this.isProfileShow = false;

          if (tabClick === "DriverShow") {
            this.isDriverShow = true;
          }

          if (tabClick == "ProfileShow") {
            this.isProfileShow = true;
          }
        }
      }]);

      return DriverComponent;
    }();

    DriverComponent.ɵfac = function DriverComponent_Factory(t) {
      return new (t || DriverComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__["RegionService"]));
    };

    DriverComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: DriverComponent,
      selectors: [["app-driver"]],
      decls: 15,
      vars: 5,
      consts: [["id", "driverManagement-Tab", 1, "small-tab"], ["role", "tablist", 1, "nav", "nav-tabs", "mb15"], [1, "nav-item"], ["id", "home-tab", "data-toggle", "tab", "href", "#driver", "role", "tab", "aria-controls", "home", "aria-selected", "true", 1, "nav-link", "active", "fs16", "mr15", 3, "click"], ["id", "profile-tab", "data-toggle", "tab", "href", "#schedule", "role", "tab", "aria-controls", "profile", "aria-selected", "false", 1, "nav-link", "fs16", "mr15", 3, "click"], [1, "tab-content"], ["id", "driver", 1, "tab-pane", "fade", "show", "active"], [4, "ngIf"], ["id", "schedule", 1, "tab-pane", "fade"], ["class", "loader", 4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function DriverComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ul", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "li", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DriverComponent_Template_a_click_3_listener() {
            return ctx.changeTab("DriverShow");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Drivers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "li", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DriverComponent_Template_a_click_6_listener() {
            return ctx.changeTab("ProfileShow");
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Profile");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DriverComponent_div_10_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, DriverComponent_div_12_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, DriverComponent_div_13_Template, 5, 0, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](14, "async");
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isDriverShow);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isProfileShow);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind1"](14, 3, ctx.regionService.onLoadingChanged));
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"], _driver_management_driver_management_component__WEBPACK_IMPORTED_MODULE_3__["DriverManagementComponent"], _driver_schedule_calender_driver_schedule_calender_component__WEBPACK_IMPORTED_MODULE_4__["DriverScheduleCalenderComponent"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["AsyncPipe"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RyaXZlci9kcml2ZXIvZHJpdmVyLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DriverComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-driver',
          templateUrl: './driver.component.html',
          styleUrls: ['./driver.component.css']
        }]
      }], function () {
        return [{
          type: _company_addresses_region_service_region_service__WEBPACK_IMPORTED_MODULE_1__["RegionService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/driver/models/DriverManagementModel.ts": function srcAppDriverModelsDriverManagementModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverManagementModel", function () {
      return DriverManagementModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverViewModel", function () {
      return DriverViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverShiftModel", function () {
      return DriverShiftModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ShiftDetailModel", function () {
      return ShiftDetailModel;
    });
    /* harmony import */


    var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! src/app/statelist.service */
    "./src/app/statelist.service.ts");

    var DriverManagementModel = function DriverManagementModel() {
      _classCallCheck(this, DriverManagementModel);

      this.Drivers = [];
      this.LicenseTypes = [];
      this.TrailerTypes = [];
    };

    var DriverViewModel = function DriverViewModel() {
      _classCallCheck(this, DriverViewModel);

      this.TrailerTypeId = [];
      this.LicenseType = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
      this.TrailerType = {};
      this.Status = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
      this.ShiftId = [];
    };

    var DriverShiftModel = function DriverShiftModel() {
      _classCallCheck(this, DriverShiftModel);

      this.Shifts = [];
    };

    var ShiftDetailModel = function ShiftDetailModel() {
      _classCallCheck(this, ShiftDetailModel);
    };
    /***/

  },

  /***/
  "./src/app/driver/models/DriverSchedule.ts": function srcAppDriverModelsDriverScheduleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverScheduleMapping", function () {
      return DriverScheduleMapping;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverSchedule", function () {
      return DriverSchedule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ConflictDates", function () {
      return ConflictDates;
    });

    var DriverScheduleMapping = function DriverScheduleMapping() {
      _classCallCheck(this, DriverScheduleMapping);

      this.ScheduleList = [];
    };

    var DriverSchedule = function DriverSchedule() {
      _classCallCheck(this, DriverSchedule);

      this.RepeatDayList = [];
      this.RepeatDayStringList = [];
      this.selectedShifts = [];
      this.selectedRepeatList = [];
    };

    var ConflictDates = function ConflictDates() {
      _classCallCheck(this, ConflictDates);
    };
    /***/

  },

  /***/
  "./src/app/driver/models/TrailerSchedule.ts": function srcAppDriverModelsTrailerScheduleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerSchedule", function () {
      return TrailerSchedule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TrailerShiftDetail", function () {
      return TrailerShiftDetail;
    });

    var TrailerSchedule = function TrailerSchedule() {
      _classCallCheck(this, TrailerSchedule);

      this.TrailerShiftDetail = [];
      this.RepeatDayList = [];
    };

    var TrailerShiftDetail = function TrailerShiftDetail() {
      _classCallCheck(this, TrailerShiftDetail);
    };
    /***/

  },

  /***/
  "./src/app/driver/models/regionSchedule.ts": function srcAppDriverModelsRegionScheduleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionScheduleViewModel", function () {
      return RegionScheduleViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ShiftSchedule", function () {
      return ShiftSchedule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionScheduleMappingViewModel", function () {
      return RegionScheduleMappingViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ShiftDetailViewModel", function () {
      return ShiftDetailViewModel;
    });

    var RegionScheduleViewModel = function RegionScheduleViewModel() {
      _classCallCheck(this, RegionScheduleViewModel);

      this.RepeatDayList = [];
      this.RegionShiftDetail = [];
    };

    var ShiftSchedule = function ShiftSchedule() {
      _classCallCheck(this, ShiftSchedule);
    };

    var RegionScheduleMappingViewModel = function RegionScheduleMappingViewModel() {
      _classCallCheck(this, RegionScheduleMappingViewModel);

      this.RepeatDayList = [];
      this.ShiftDetail = [];
    };

    var ShiftDetailViewModel = function ShiftDetailViewModel() {
      _classCallCheck(this, ShiftDetailViewModel);
    };
    /***/

  },

  /***/
  "./src/app/driver/services/driver.service.ts": function srcAppDriverServicesDriverServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverService", function () {
      return DriverService;
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


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var DriverService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(DriverService, _src_app_errors_Handl);

      var _super = _createSuper(DriverService);

      function DriverService(httpClient) {
        var _this35;

        _classCallCheck(this, DriverService);

        _this35 = _super.call(this);
        _this35.httpClient = httpClient;
        _this35.getShiftUrl = '/Settings/Profile/GetShifts';
        _this35.getAllDriversUrl = '/Settings/Profile/GetAllDrivers';
        _this35.postAddDriverUrl = '/Settings/Profile/AddDriver';
        _this35.postDeleteDriverUrl = '/Settings/Profile/DeleteInvitedUser';
        _this35.changeDriverStatusUrl = '/Settings/Profile/ChangeUserStatus?id=';
        _this35.getRegionsUrl = '/Supplier/Region/GetRegionsDdl';
        _this35.addTrailerScheduleUrl = '/Supplier/Dispatch/AddTrailerSchedule';
        _this35.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        return _this35;
      }

      _createClass(DriverService, [{
        key: "getShifts",
        value: function getShifts() {
          return this.httpClient.get(this.getShiftUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getShifts', null)));
        }
      }, {
        key: "getAllDrivers",
        value: function getAllDrivers() {
          return this.httpClient.get(this.getAllDriversUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getAllDrivers', null)));
        }
      }, {
        key: "postAddDriver",
        value: function postAddDriver(driverModel) {
          return this.httpClient.post(this.postAddDriverUrl, driverModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('postAddDriver', null)));
        }
      }, {
        key: "postDeleteDriver",
        value: function postDeleteDriver(driverdelteInfo) {
          var data = {
            Id: driverdelteInfo.UserId,
            IsScheduleExists: driverdelteInfo.IsScheduleExists,
            ScheduleBuilderIdInfo: driverdelteInfo.ScheduleBuilderIdInfo
          };
          return this.httpClient.post(this.postDeleteDriverUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('postDeleteDriver', null)));
        }
      }, {
        key: "changeDriverStatus",
        value: function changeDriverStatus(id, isActive) {
          return this.httpClient.get(this.changeDriverStatusUrl + id + "&isActive=" + isActive).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('changeDriverStatus', null)));
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          return this.httpClient.get(this.getRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getRegions', null)));
        }
      }, {
        key: "addTrailerSchedule",
        value: function addTrailerSchedule(model) {
          return this.httpClient.post(this.addTrailerScheduleUrl, model, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('addTrailerSchedule', model)));
        }
      }]);

      return DriverService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    DriverService.ɵfac = function DriverService_Factory(t) {
      return new (t || DriverService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"]));
    };

    DriverService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: DriverService,
      factory: DriverService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DriverService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"]
        }];
      }, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=driver-driver-module-es5.js.map