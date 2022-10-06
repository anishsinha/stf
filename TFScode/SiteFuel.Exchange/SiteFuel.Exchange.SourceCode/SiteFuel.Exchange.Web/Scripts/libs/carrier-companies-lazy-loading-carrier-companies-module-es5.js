function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["carrier-companies-lazy-loading-carrier-companies-module"], {
  /***/
  "./src/app/carrier-companies/carrier-companies.component.ts": function srcAppCarrierCompaniesCarrierCompaniesComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierCompaniesComponent", function () {
      return CarrierCompaniesComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _service_assigncarrier_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./service/assigncarrier.service */
    "./src/app/carrier-companies/service/assigncarrier.service.ts");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var ng_sidebar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ng-sidebar */
    "./node_modules/ng-sidebar/__ivy_ngcc__/lib_esmodule/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var _search_filter__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./search-filter */
    "./src/app/carrier-companies/search-filter.ts");

    var _c0 = ["btnOpenModal"];
    var _c1 = ["btnCloseModal"];
    var _c2 = ["btnCloseBulkModal"];
    var _c3 = ["confirmationBox"];

    function CarrierCompaniesComponent_h3_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Edit Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CarrierCompaniesComponent_ng_template_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Assign Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Order(s)/DR(s) created for those locations will be sent to respective email(s)");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CarrierCompaniesComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r3.WarningMessage);
      }
    }

    function CarrierCompaniesComponent_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CarrierCompaniesComponent_label_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CarrierCompaniesComponent_ng_multiselect_dropdown_14_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "ng-multiselect-dropdown", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_ng_multiselect_dropdown_14_Template_ng_multiselect_dropdown_ngModelChange_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r16.selectedCarrierItem = $event;
        })("onSelect", function CarrierCompaniesComponent_ng_multiselect_dropdown_14_Template_ng_multiselect_dropdown_onSelect_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r18.OnCarrierSelect($event);
        })("onDeSelect", function CarrierCompaniesComponent_ng_multiselect_dropdown_14_Template_ng_multiselect_dropdown_onDeSelect_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r19.OnCarrierDeSelect($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r6.selectedCarrierItem)("placeholder", "Select Carrier")("settings", ctx_r6.dropdownSettings)("data", ctx_r6.carrierList);
      }
    }

    function CarrierCompaniesComponent_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r7.SelectedCarrier.Name);
      }
    }

    var _c4 = function _c4(a0) {
      return {
        "active": a0
      };
    };

    function CarrierCompaniesComponent_li_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_li_40_Template_li_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22);

          var availableJob_r20 = ctx.$implicit;

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r21.toggleSelect(availableJob_r20);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var availableJob_r20 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](2, _c4, availableJob_r20.Job.IsSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", availableJob_r20.Job.Name, "");
      }
    }

    function CarrierCompaniesComponent_li_65_span_6_ng_container_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var item_r26 = ctx.$implicit;
        var isLast_r27 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", item_r26.Name, "", isLast_r27 ? "" : ", ", " ");
      }
    }

    function CarrierCompaniesComponent_li_65_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CarrierCompaniesComponent_li_65_span_6_ng_container_1_Template, 2, 2, "ng-container", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assignedJob_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", assignedJob_r23.Job.Emails);
      }
    }

    function CarrierCompaniesComponent_li_65_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_li_65_Template_div_click_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r30);

          var assignedJob_r23 = ctx.$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r29.toggleSelect(assignedJob_r23);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CarrierCompaniesComponent_li_65_span_6_Template, 2, 1, "span", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_li_65_Template_a_click_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r30);

          var assignedJob_r23 = ctx.$implicit;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r31.editEmail($event, assignedJob_r23);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var assignedJob_r23 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](3, _c4, assignedJob_r23.Job.IsSelected));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](assignedJob_r23.Job.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", assignedJob_r23.Job.Emails);
      }
    }

    function CarrierCompaniesComponent_div_89_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CarrierCompaniesComponent_tr_107_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "button", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_tr_107_Template_button_click_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34);

          var carrier_r32 = ctx.$implicit;

          var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r33.editForm(carrier_r32);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "a", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("cancel", function CarrierCompaniesComponent_tr_107_Template_a_cancel_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r35.cancelClicked = true;
        })("confirm", function CarrierCompaniesComponent_tr_107_Template_a_confirm_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34);

          var carrier_r32 = ctx.$implicit;

          var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r36.removeAssignedCarrier(carrier_r32);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carrier_r32 = ctx.$implicit;

        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](carrier_r32.Carrier.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](carrier_r32.assignedLocations);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](carrier_r32.Jobs.length);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r12.popoverTitle)("popoverMessage", ctx_r12.popoverMessage);
      }
    }

    function CarrierCompaniesComponent_div_112_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var CarrierCompaniesComponent = /*#__PURE__*/function () {
      function CarrierCompaniesComponent(fb, assigncarrierService) {
        _classCallCheck(this, CarrierCompaniesComponent);

        this.fb = fb;
        this.assigncarrierService = assigncarrierService;
        this.carrierEmails = [];
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.isUpdate = false;
        this.IsLoading = false;
        this.IsEmpty = false;
        this.IsSuccess = false;
        this.assignedCarrierList = [];
        this.assignedCarriers = [];
        this.carrierList = [];
        this.jobs = [];
        this.jobs2 = [];
        this.availableJobs = [];
        this.assignedJobs = [];
        this.query = '';
        this.list1Search = '';
        this.list2Search = '';
        this.popoverTitle = 'Delete Confirmation';
        this.popoverMessage = 'Do you really want to delete? Deleting a location assignment will result in closure of all the existing orders for the carrier';
        this.confirmClicked = false;
        this.cancelClicked = false;
        this.CarrierUsers = [];
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.dtOptions = {};
        this.IsDisplayLoader = false;
        this.SelectedJobs = [];
        this.IsCreateFreightOrder = null;
        this.IsJobDeselect = false;
        this.removedJobs = [];
        this.WarningMessage = "Note: Removing a location assignment will result in closure of all the existing orders for the carrier";
        this.editCarrierId = null;
        this.existingJobs = [];
        this.selectCarrierModel = [];
        this.assginedJobSelectAll = false;
        this.availableJobSelectAll = false;
      }

      _createClass(CarrierCompaniesComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.getAssignedCarriers();
          this.getCarriers();
          this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
          };
          this.dropdownSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
          }; //this.getAssignedCarrierUsers();

          var exportColumns = {
            columns: [0, 1, 2]
          };
          this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [{
              extend: 'copy',
              exportOptions: exportColumns
            }, {
              extend: 'csv',
              title: 'Assigned Carriers',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Assigned Carriers',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }]
          };
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this = this;

          this.assigncarrierService.getCarriers().subscribe(function (carriers) {
            _this.carrierList = carriers;
            _this.carrierList.length;
          });
        }
      }, {
        key: "getAssignedCarrierUsers",
        value: function getAssignedCarrierUsers() {
          var _this2 = this;

          this.assigncarrierService.getAssignedCarrierUsers().subscribe(function (carriers) {
            _this2.CarrierUsers = carriers;

            _this2.refreshDatatable();
          });
        }
      }, {
        key: "refreshDatatable",
        value: function refreshDatatable() {
          this.dtElements.forEach(function (dtElement) {
            if (dtElement.dtInstance) {
              dtElement.dtInstance.then(function (dtInstance) {
                dtInstance.destroy();
              });
            }
          });
          this.dtTrigger.next();
        }
      }, {
        key: "removeAssignedCarrier",
        value: function removeAssignedCarrier(carrier) {
          var _this3 = this;

          this.IsSuccess = true;
          this.IsLoading = true;
          this.assigncarrierService.deleteAssignedCarrier(carrier).subscribe(function (response) {
            _this3.serviceResponse = response;

            if (response.StatusCode == 0) {
              _this3.carrierList.push(carrier.Carrier);

              _this3.closeAssignedOrdersforCarrier(carrier);
            } else {
              _this3.IsLoading = false;
            }
          });
        }
      }, {
        key: "getAssignedCarriers",
        value: function getAssignedCarriers() {
          var _this4 = this;

          this.IsLoading = true;
          this.assigncarrierService.getAssignedCarriers().subscribe(function (response) {
            if (response != null && response != undefined) {
              _this4.assignedCarrierList = response;
              var existingCarriers = response.map(function (item) {
                return item.Carrier.Id;
              });
              _this4.carrierList = _this4.carrierList.filter(function (item) {
                return existingCarriers.indexOf(item.Id) == -1;
              });

              _this4.getJobs();

              _this4.IsLoading = false;

              _this4.refreshDatatable();
            }
          });
        }
      }, {
        key: "getJobs",
        value: function getJobs() {
          var _this5 = this;

          this.assignedCarrierList.forEach(function (element) {
            if (element) {
              element.assignedLocations = _this5.getAssignedJobs(element);

              if (element.Jobs) {
                _this5.existingJobs.push(element.Jobs.map(function (item) {
                  return item.Job;
                }));
              }
            }
          }); //this.existingJobs = this.assignedCarrierList.map(function (item) { return item.Jobs;});

          this.assigncarrierService.getJobs().subscribe(function (response) {
            _this5.availableJobs = response;
            _this5.allJobs = response;
            _this5.availableJobs && _this5.availableJobs.map(function (m) {
              return m.Job.IsSelected = false;
            });

            if (_this5.existingJobs) {
              _this5.existingJobs.forEach(function (element) {
                if (element) {
                  element.forEach(function (job) {
                    var index = _this5.availableJobs.findIndex(function (t) {
                      return t.Job.Id == job.Id;
                    });

                    if (index != -1) {
                      _this5.availableJobs.splice(index, 1);
                    }

                    ;
                  });
                }
              });
            }
          });
        }
      }, {
        key: "getAssignedJobs",
        value: function getAssignedJobs(assignedJobs) {
          var locations = '-';

          if (assignedJobs) {
            var jobs = assignedJobs.Jobs.map(function (item) {
              return item.Job.Name;
            });

            if (jobs.length > 4) {
              locations = jobs.slice(0, 4).join(", ") + "...";
            } else {
              locations = jobs.join(", ");
            }
          }

          return locations;
        }
      }, {
        key: "editForm",
        value: function editForm(_carrier) {
          this.SelectedCarrier = _carrier.Carrier;
          this.editCarrierId = _carrier.Id;

          var selectedJobs = _carrier.Jobs.map(function (item) {
            return item.Job;
          });

          this.SelectedJobs = [];
          this.SelectedJobs = selectedJobs;
          this.assignedJobs = _carrier.Jobs;
          this.assignedJobs && this.assignedJobs.map(function (m) {
            return m.Job.IsSelected = false;
          });

          this._toggleOpened(true);

          this.GetCarrierUserEmails(_carrier.Carrier.Id);
          this.isUpdate = true;
        }
      }, {
        key: "getCarrierEmailsById",
        value: function getCarrierEmailsById() {
          var _this6 = this;

          if (this.SelectedCarrier && this.SelectedCarrier.Id > 0) {
            var _com = this.carrierEmails.find(function (x) {
              return x.CompanyId == _this6.SelectedCarrier.Id;
            });

            return _com ? _com.CarrierEmails : [];
          } else {
            return [];
          }
        }
      }, {
        key: "GetCarrierUserEmails",
        value: function GetCarrierUserEmails(companyId) {
          var _this7 = this;

          var _com = this.carrierEmails.find(function (x) {
            return x.CompanyId == companyId;
          });

          if (!_com) {
            this.IsLoading = true;
            this.assigncarrierService.GetCarrierUserEmails(companyId).subscribe(function (response) {
              if (response) {
                _this7.carrierEmails.push({
                  CompanyId: companyId,
                  CarrierEmails: response
                });
              }

              _this7.IsLoading = false;
            });
          }
        }
      }, {
        key: "Validate",
        value: function Validate() {
          if (!this.SelectedCarrier || this.SelectedCarrier.Id <= 0) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Atleast one carrier must be assigned", undefined, undefined);

            return false;
          }

          if (!this.assignedJobs || this.assignedJobs.length <= 0) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Atleast one job must be selected", undefined, undefined);

            return false;
          }

          return true;
        }
      }, {
        key: "SaveCarrier",
        value: function SaveCarrier() {
          if (this.Validate()) {
            this.assignedCarriers = [];
            this.assignedCarriers.push({
              Id: "",
              Carrier: null,
              Jobs: [],
              assignedLocations: null
            });

            if (this.SelectedCarrier) {
              this.assignedCarriers[0].Carrier = this.SelectedCarrier;
              this.assignedCarriers[0].Jobs = this.assignedJobs;
              this.assignedCarriers[0].Id = this.editCarrierId;

              if (this.isUpdate) {
                this.updateAssignedCarrier();
              } else {
                this.assignCarriers();
              }
            }
          }
        }
      }, {
        key: "SaveJobEmail",
        value: function SaveJobEmail() {
          var _this8 = this;

          var selectedJob = this.assignedJobs.find(function (t) {
            return t.Job.Id == _this8.editEmailJobId;
          });

          if (selectedJob) {
            selectedJob.Job.Emails = [];

            if (this.editEmailDetails) {
              this.editEmailDetails.forEach(function (element) {
                if (!selectedJob.Job.Emails) {
                  selectedJob.Job.Emails = [];
                }

                if (!selectedJob.Job.Emails.find(function (t) {
                  return t.Id == element.Id;
                })) {
                  selectedJob.Job.Emails.push({
                    Id: element.Id,
                    Name: element.Name,
                    Code: element.Code,
                    IsSelected: element.IsSelected
                  });
                }
              });
            } else {
              selectedJob.Job.Emails = [];
            }
          }
        }
      }, {
        key: "assignNewForm",
        value: function assignNewForm() {
          this._toggleOpened(true);

          this.isUpdate = false;
        }
      }, {
        key: "assignCarriers",
        value: function assignCarriers() {
          this.DisplayFreightOrderConfirmationModal();
        }
      }, {
        key: "updateAssignedCarrier",
        value: function updateAssignedCarrier() {
          var _this9 = this;

          var updatedJobIds = this.ValidateIfNewJobAdded();

          if (updatedJobIds.length > 0) {
            if (updatedJobIds[0].InsertedJobs.length > 0) {
              //show modal as new jobs are added when editing;FO will be created only for newly assigned jobs
              this.DisplayFreightOrderConfirmationModal();
            } else {
              this.IsSuccess = true;
              this.assigncarrierService.updateAssignedCarrier(this.assignedCarriers[0]) // First Update Existing carrier assignment
              .subscribe(function (response) {
                _this9.serviceResponse = response;

                if (response.StatusCode == 0) {
                  _this9.IsSuccess = false;

                  _this9.EditFreightOnlyOrders(false); // Edit FO according to new job assignment

                } else {
                  _this9.IsSuccess = false;

                  _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Carrier Assignment Failed", undefined, undefined);

                  _this9.getAssignedCarriers();

                  _this9.IsDisplayLoader = false;
                  var element = _this9.btnCloseModal.nativeElement;
                  element.click();

                  _this9._toggleOpened(false);
                }
              });
            }
          }
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          if (shouldOpen) {
            this._opened = true;
          } else {
            this._opened = !this._opened;
            this.clearForm();
            this.isUpdate = false;
          }
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.availableJobs = this.allJobs;
          this.availableJobs && this.availableJobs.map(function (m) {
            return m.Job.IsSelected = false;
          });
          this.assignedJobs = [];
          this.SelectedCarrier = null;
          this.selectCarrierModel = [];
          this.list1Search = '';
          this.list2Search = '';
          this.assginedJobSelectAll = false;
          this.availableJobSelectAll = false;
          this.assignedCarriers.push({
            Id: "",
            Carrier: null,
            Jobs: [],
            assignedLocations: null
          });
          this.editEmailDetails = [];
          this.selectedCarrierItem = null;
          this.existingJobs = [];
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(name, i) {
          var carrierControls = this.getCarriersFormArray();
          var result = carrierControls.controls[i].get(name).invalid && (carrierControls.controls[i].get(name).dirty || carrierControls.controls[i].get(name).touched);
          return result;
        }
      }, {
        key: "getCarriersFormArray",
        value: function getCarriersFormArray() {
          return this.rcForm.get('Carriers');
        }
      }, {
        key: "OnCarrierSelect",
        value: function OnCarrierSelect(carrier) {
          this.SelectedCarrier = carrier;
          this.GetCarrierUserEmails(carrier.Id);
        }
      }, {
        key: "OnCarrierDeSelect",
        value: function OnCarrierDeSelect(carrier) {
          this.SelectedCarrier = null;
        }
      }, {
        key: "OnEmailSelect",
        value: function OnEmailSelect(email, job) {
          if (!job.Job.Emails) {
            job.Job.Emails = [];
          }

          job.Job.Emails.push({
            Id: email.Id,
            Name: email.Name,
            Code: null,
            IsSelected: true
          });
        }
      }, {
        key: "OnEmailDeSelect",
        value: function OnEmailDeSelect(email, job) {
          var index = job.Job.Emails.findIndex(function (a) {
            return a.Id == email.Id;
          });

          if (index != -1) {
            job.Job.Emails.splice(index, 1);
          }
        }
      }, {
        key: "editEmail",
        value: function editEmail(email, job) {
          job.Job.IsEmailEdit = true;
          this.editEmailJobId = job.Job.Id;
          this.editEmailDetails = job.Job.Emails;
        }
      }, {
        key: "toggleSelect",
        value: function toggleSelect(availableJob) {
          if (availableJob.Job.IsSelected) {
            availableJob.Job.IsSelected = false;
          } else {
            availableJob.Job.IsSelected = true;
          }

          if (this.availableJobs.find(function (t) {
            return !t.Job.IsSelected;
          })) {
            this.availableJobSelectAll = false;
          }

          if (this.assignedJobs.find(function (t) {
            return !t.Job.IsSelected;
          })) {
            this.assginedJobSelectAll = false;
          }

          this.SelectedCount = Object.keys(this.availableJobs.filter(function (data) {
            return data.Job.IsSelected === true;
          })).length;
        }
      }, {
        key: "toogleSelectAll",
        value: function toogleSelectAll(name, isChecked) {
          var _this10 = this;

          if (isChecked) {
            name == 'availableJob' ? this.availableJobs.filter(function (t) {
              return t.Job.Name.toLowerCase().indexOf(_this10.list1Search.toLowerCase()) >= 0;
            }).map(function (m) {
              return m.Job.IsSelected = true;
            }) : this.assignedJobs.filter(function (t) {
              return t.Job.Name.toLowerCase().indexOf(_this10.list2Search.toLowerCase()) >= 0;
            }).map(function (m) {
              return m.Job.IsSelected = true;
            });
          } else {
            name == 'availableJob' ? this.availableJobs.filter(function (t) {
              return t.Job.Name.toLowerCase().indexOf(_this10.list1Search.toLowerCase()) >= 0;
            }).map(function (m) {
              return m.Job.IsSelected = false;
            }) : this.assignedJobs.filter(function (t) {
              return t.Job.Name.toLowerCase().indexOf(_this10.list2Search.toLowerCase()) >= 0;
            }).map(function (m) {
              return m.Job.IsSelected = false;
            });
          }

          this.SelectedCount = Object.keys(this.availableJobs.filter(function (data) {
            return data.Job.IsSelected === true;
          })).length;
        }
      }, {
        key: "moveToLeft",
        value: function moveToLeft() {
          this.availableJobs.map(function (m) {
            return m.Job.IsSelected = false;
          });
          var ls = this.assignedJobs.filter(function (f) {
            return f.Job.IsSelected == true;
          }); // this.availableJobs = this.availableJobs.concat(ls);

          this.availableJobs = ls.concat(this.availableJobs);
          this.assignedJobs = this.assignedJobs.filter(function (f) {
            return f.Job.IsSelected == false;
          });
          this.assignedJobs.map(function (m) {
            return m.Job.IsSelected = false;
          });
        }
      }, {
        key: "moveToRight",
        value: function moveToRight() {
          this.assignedJobs.map(function (m) {
            return m.Job.IsSelected = false;
          });
          var ls = this.availableJobs.filter(function (f) {
            return f.Job.IsSelected == true;
          }); // this.assignedJobs = this.assignedJobs.concat(ls);

          this.assignedJobs = ls.concat(this.assignedJobs);
          this.availableJobs = this.availableJobs.filter(function (f) {
            return f.Job.IsSelected == false;
          });
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.IsCreateFreightOrder = null;
          this.selectedFile = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this11 = this;

          if (!this.file) {
            _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Please select file", undefined, undefined);

            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.IsLoading = true;
          this.assigncarrierService.upload(this.file, this.IsCreateFreightOrder).subscribe(function (response) {
            if (response.StatusCode == 1) {
              _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);

              _this11.IsLoading = false;
            } else {
              _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this11.getAssignedCarriers();
            }

            _this11.file = null;
          });
        }
      }, {
        key: "DisplayFreightOrderConfirmationModal",
        value: function DisplayFreightOrderConfirmationModal() {
          this.IsDisplayLoader = false;
          var element = this.btnOpenModal.nativeElement;
          element.click();
        }
      }, {
        key: "IscreateFreightOrders",
        value: function IscreateFreightOrders(IsCreateOrder) {
          var _this12 = this;

          if (!this.isUpdate) {
            this.IsDisplayLoader = true;
            this.assigncarrierService.assignCarriers(this.assignedCarriers).subscribe(function (response) {
              if (response.StatusCode == 0) {
                _this12.IsSuccess = false;

                if (IsCreateOrder) {
                  _this12.assigncarrierService.createFreightOrder(_this12.assignedCarriers).subscribe(function (response) {
                    if (response.StatusCode == 0) {
                      _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Order(s) Assigned Successfully to Carrier", undefined, undefined);
                    } else if (response.StatusCode == 1) {
                      _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
                    }

                    _this12.IsDisplayLoader = false;
                    var element = _this12.btnCloseModal.nativeElement;
                    element.click();

                    _this12._toggleOpened(false);

                    _this12.getAssignedCarriers();
                  });
                } else {
                  var element = _this12.btnCloseModal.nativeElement;
                  element.click();

                  _this12._toggleOpened(false);

                  _this12.getAssignedCarriers();

                  _this12.IsDisplayLoader = false;
                }
              } else if (response.StatusCode == 1) {
                _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              }
            });
          } else if (this.isUpdate) {
            this.IsDisplayLoader = true;
            this.assigncarrierService.updateAssignedCarrier(this.assignedCarriers[0]) // First Update Existing carrier assignment
            .subscribe(function (response) {
              _this12.serviceResponse = response;

              if (response.StatusCode == 0) {
                _this12.IsSuccess = false;
                _this12.IsDisplayLoader = false;

                _this12.EditFreightOnlyOrders(IsCreateOrder); // Edit FO according to new job assignment

              } else {
                _this12.IsDisplayLoader = false;
                var element = _this12.btnCloseModal.nativeElement;
                element.click();

                _this12._toggleOpened(false);

                _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Carrier Assignment Failed", undefined, undefined);
              } //this.IsDisplayLoader = false;

            });
          }
        }
      }, {
        key: "ValidateIfNewJobAdded",
        value: function ValidateIfNewJobAdded() {
          this.UpdatedJobIds = [];
          var prevAssignedJobIds = this.SelectedJobs.map(function (item) {
            return item.Id;
          });
          var newlyAssignedJobIds = this.assignedCarriers[0].Jobs.map(function (item) {
            return item.Job.Id;
          });
          var keys1 = {};
          var keys2 = {};
          var inserted = [];
          var deleted = [];
          prevAssignedJobIds.forEach(function (item) {
            keys1[item] = item;
          });
          newlyAssignedJobIds.forEach(function (item) {
            keys2[item] = item;
          });
          prevAssignedJobIds.forEach(function (item) {
            if (!keys2[item]) {
              deleted.push(item);
            }
          });
          newlyAssignedJobIds.forEach(function (item) {
            if (!keys1[item]) {
              inserted.push(item);
            }
          });
          this.UpdatedJobIds.push({
            InsertedJobs: inserted,
            DeletedJobs: deleted
          });
          return this.UpdatedJobIds;
        }
      }, {
        key: "EditFreightOnlyOrders",
        value: function EditFreightOnlyOrders(IsCreateOrder) {
          var _this13 = this;

          this.IsDisplayLoader = true;
          var InsertedJobIds = [];
          var DeletedJobIds = [];
          InsertedJobIds = this.UpdatedJobIds[0].InsertedJobs;
          DeletedJobIds = this.UpdatedJobIds[0].DeletedJobs;
          var editfreightOrder = new _service_assigncarrier_service__WEBPACK_IMPORTED_MODULE_1__["EditFreightOnlyOrder"]();
          editfreightOrder.newJobsIds = InsertedJobIds;
          editfreightOrder.removedJobsIds = DeletedJobIds;
          editfreightOrder.CarrierCompanyId = this.assignedCarriers[0].Carrier.Id;
          editfreightOrder.IsCreateOrder = IsCreateOrder;
          this.assigncarrierService.editFreightOnlyOrders(editfreightOrder).subscribe(function (response) {
            if (response.StatusCode == 0) {
              _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Carrier - Location assignment made successfully", undefined, undefined);

              _this13.getAssignedCarriers();
            } else {
              _declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Failed", undefined, undefined);
            }

            _this13.IsDisplayLoader = false;
            var element = _this13.btnCloseModal.nativeElement;
            element.click();

            _this13._toggleOpened(false);
          });
        }
      }, {
        key: "closeAssignedOrdersforCarrier",
        value: function closeAssignedOrdersforCarrier(carrier) {
          var _this14 = this;

          this.IsSuccess = true;
          var removedJobsIds = carrier.Jobs.map(function (item) {
            return item.Job.Id;
          });
          var editfreightOrder = new _service_assigncarrier_service__WEBPACK_IMPORTED_MODULE_1__["EditFreightOnlyOrder"]();
          editfreightOrder.newJobsIds = [];
          editfreightOrder.removedJobsIds = removedJobsIds;
          editfreightOrder.CarrierCompanyId = carrier.Carrier.Id;
          editfreightOrder.IsCreateOrder = false;
          this.assigncarrierService.closeAssignedOrdersforCarrier(editfreightOrder).subscribe(function (response) {
            _this14.serviceResponse = response;

            if (response.StatusCode == 0) {
              _this14.IsSuccess = false;
            } else {}

            _this14.IsLoading = false;
            _this14._opened = false;

            _this14.getAssignedCarriers();
          });
        }
      }]);

      return CarrierCompaniesComponent;
    }();

    CarrierCompaniesComponent.ɵfac = function CarrierCompaniesComponent_Factory(t) {
      return new (t || CarrierCompaniesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_assigncarrier_service__WEBPACK_IMPORTED_MODULE_1__["AssigncarrierService"]));
    };

    CarrierCompaniesComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CarrierCompaniesComponent,
      selectors: [["app-carrier-companies"]],
      viewQuery: function CarrierCompaniesComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c1, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c2, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c3, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnOpenModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.confirmationBox = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      decls: 186,
      vars: 37,
      consts: [["id", "CarrierToggleId", 1, "mt10"], [2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], ["class", "dib ml10 mt10 mb0", 4, "ngIf", "ngIfElse"], ["editTitle", ""], [4, "ngIf"], ["class", "pa bg-white z-index5 loading-wrapper", 4, "ngIf"], [1, "row", "mt10"], [1, "col-sm-4", "text-left"], [1, "form-group"], ["class", "fs16 font-weight-500", "for", "formGroupExampleInput", 4, "ngIf"], ["class", "single-select", "id", "formGroupExampleInput", 3, "ngModel", "placeholder", "settings", "data", "ngModelChange", "onSelect", "onDeSelect", 4, "ngIf"], ["class", "fs16 font-weight-500", 4, "ngIf"], [1, "row"], [1, "dual-list", "list-left", "col-4"], [1, "well", "text-right"], [1, "col-sm-12", "text-left"], [1, "col-sm-9"], [1, "input-group", "mb-3"], [1, "input-group-prepend"], ["id", "basic-addon1", 1, "input-group-text"], [1, "fa", "fa-search"], ["type", "text", "name", "searchAvailableJob", "placeholder", "search", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-1"], [1, "text-center"], [1, "col-sm-2"], ["data-toggle", "tooltip", "data-placement", "top", "title", "Select All", 1, "btn-group", "float-left", "form-control", "chk-custom-all"], [1, "form-check", "form-check-inline", "mt4"], ["type", "checkbox", "name", "availableJobSelectAll", "id", "ckb-list1-SelectAll", "value", "selectAll", 1, "form-check-input", 3, "ngModel", "ngModelChange", "change"], [1, "list-group"], ["class", "list-group-item", 3, "ngClass", "click", 4, "ngFor", "ngForOf"], [1, "list-arrows", "col-sm-1", "text-center"], ["title", "Shift to Left", 1, "btn", "btn-default", "btn-sm", "move-left", 3, "click"], [1, "glyphicon", "glyphicon-chevron-left"], ["title", "Shift to Right", 1, "btn", "btn-default", "btn-sm", "move-right", 3, "click"], [1, "glyphicon", "glyphicon-chevron-right"], [1, "dual-list", "list-right", "col-7"], [1, "well"], [1, "col-sm-3"], ["type", "checkbox", "name", "assginedJobSelectAll", "id", "ckb-list2-SelectAll", "value", "selectAll", 1, "form-check-input", 3, "ngModel", "ngModelChange", "change"], ["class", "list-group-item", 3, "ngClass", 4, "ngFor", "ngForOf"], [1, "row", "mt15"], [1, "col-sm-12", "text-right"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg", 3, "click"], ["id", "openfreightOrderModel", "type", "button", "data-toggle", "modal", "data-target", "#createFreightOrderModel", 3, "hidden"], ["btnOpenModal", ""], [1, "row", "mt5", "mb10"], [1, "pt0", "pull-left"], ["id", "assignNewCarrier", 1, "fs18", "pull-left", "ml20", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt4", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "col-sm-3", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#upload-carrier-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-download", "fs18", "mt4", "pull-left"], ["id", "assignCarrierDetails", 1, "row", "pr"], ["class", "pa bg-white z-index5 loading-wrapper left0 top0", 4, "ngIf"], [1, "col-sm-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "carrierjobassignment-grid", 1, "table-responsive"], ["id", "carrier-datatable", "datatable", "", 1, "table", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngFor", "ngForOf"], ["id", "createFreightOrderModel", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-sm"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseModal", ""], [1, "fa", "fa-close", "fs21"], [1, "col-lg-12", "mt10"], [1, "col-sm-12", "text-right", "mt10"], ["type", "button", "id", "btnDismissCreateFreightOrder", 1, "btn", "btn-lg", 3, "click"], ["type", "button", "id", "btnCreateFreightOrder", 1, "btn", "btn-lg", "btn-primary", 3, "click"], ["id", "upload-carrier-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Bulk_upload_template_Carrier_Assignment.csv", 1, "mb5", "btn-download"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "alert", "alert-warning", "fs12"], ["for", "selectInputEmail1", 1, "d-block"], [1, "form-check", "form-check-inline"], ["type", "radio", "name", "inlineRadioOptions", "id", "inlineRadio1", "value", "1", 1, "form-check-input", 3, "ngModel", "ngModelChange"], ["for", "inlineRadio1", 1, "form-check-label"], ["type", "radio", "name", "inlineRadioOptions", "id", "inlineRadio2", "value", "0", 1, "form-check-input", 3, "ngModel", "ngModelChange"], ["for", "inlineRadio2", 1, "form-check-label"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "disabled", "click"], ["id", "add-edit-email", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog", "modal-dialog-centered"], [1, "modal-header", "modal-header", "pt5", "pb0"], [1, "float-left"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close"], ["aria-hidden", "true"], ["for", "selectInputEmail1"], ["id", "selectInputEmail1", 3, "placeholder", "ngModel", "settings", "data", "ngModelChange"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-primary", 3, "click"], [1, "dib", "ml10", "mt10", "mb0"], [1, "alert", "alert-warning", "fs11", "pt5", "pl10", "pr10", "pb5", "mb5"], [1, "pa", "bg-white", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], ["for", "formGroupExampleInput", 1, "fs16", "font-weight-500"], ["id", "formGroupExampleInput", 1, "single-select", 3, "ngModel", "placeholder", "settings", "data", "ngModelChange", "onSelect", "onDeSelect"], [1, "fs16", "font-weight-500"], [1, "list-group-item", 3, "ngClass", "click"], [1, "list-group-item", 3, "ngClass"], [1, "col-4", 3, "click"], [1, "col-7", "trncate-text", "text-right"], [1, "col-1", "text-right"], ["href", "javascript:void(0);", "id", "edit-email", "data-toggle", "modal", "data-target", "#add-edit-email", 3, "click"], ["data-toggle", "tooltip", "data-placement", "top", "title", "Edit Email", 1, "fa", "fa-edit", "ml10"], [1, "pa", "bg-white", "z-index5", "loading-wrapper", "left0", "top0"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fas", "fa-edit", "fs16"], ["mwlConfirmationPopover", "", "placement", "left", 1, "fa", "fa-trash-alt", "color-maroon", "ml10", "mr15", 3, "popoverTitle", "popoverMessage", "cancel", "confirm"]],
      template: function CarrierCompaniesComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "ng-sidebar", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("openedChange", function CarrierCompaniesComponent_Template_ng_sidebar_openedChange_2_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_a_click_3_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CarrierCompaniesComponent_h3_5_Template, 2, 0, "h3", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CarrierCompaniesComponent_ng_template_6_Template, 4, 0, "ng-template", null, 5, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, CarrierCompaniesComponent_div_8_Template, 3, 1, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CarrierCompaniesComponent_div_9_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, CarrierCompaniesComponent_label_13_Template, 2, 0, "label", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, CarrierCompaniesComponent_ng_multiselect_dropdown_14_Template, 1, 4, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, CarrierCompaniesComponent_div_15_Template, 2, 1, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Available Location(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "span", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](28, "i", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "input", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_29_listener($event) {
            return ctx.list1Search = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "h5");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Count");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "input", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_38_listener($event) {
            return ctx.availableJobSelectAll = $event;
          })("change", function CarrierCompaniesComponent_Template_input_change_38_listener($event) {
            return ctx.toogleSelectAll("availableJob", $event.target.checked);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "ul", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](40, CarrierCompaniesComponent_li_40_Template, 2, 4, "li", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](41, "startsWithJob");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "button", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_43_listener() {
            return ctx.moveToLeft();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "button", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_45_listener() {
            return ctx.moveToRight();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](46, "span", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](52, "Assigned Location(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](57, "span", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](58, "i", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "input", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_59_listener($event) {
            return ctx.list2Search = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "input", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_63_listener($event) {
            return ctx.assginedJobSelectAll = $event;
          })("change", function CarrierCompaniesComponent_Template_input_change_63_listener($event) {
            return ctx.toogleSelectAll("assignedJob", $event.target.checked);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "ul", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](65, CarrierCompaniesComponent_li_65_Template, 10, 5, "li", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](66, "startsWithJob");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "div", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](69, "button", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_69_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](70, "Cancel");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](71, "button", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_71_listener() {
            return ctx.SaveCarrier();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](72, "Save");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](73, "button", 46, 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](75, "div", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](77, "h4", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](78, "Assigned Carriers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](79, "a", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_a_click_79_listener() {
            return ctx.assignNewForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](80, "i", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](81, "span", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](82, "Assign New");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](83, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](84, "a", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_a_click_84_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](85, "i", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](86, "span", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](87, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](88, "div", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](89, CarrierCompaniesComponent_div_89_Template, 2, 0, "div", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](91, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](92, "div", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](93, "div", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "div", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](95, "table", 63);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](96, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](97, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](98, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](99, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](100, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](101, "Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](103, "No. of Locations Assigned");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](104, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](105, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](106, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](107, CarrierCompaniesComponent_tr_107_Template, 11, 5, "tr", 64);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](108, "div", 65);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](109, "div", 66);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](110, "div", 67);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](111, "div", 68);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](112, CarrierCompaniesComponent_div_112_Template, 2, 0, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](113, "div", 69);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](114, "h4", 70);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](115, "Pass Order");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](116, "button", 71, 72);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](118, "i", 73);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](119, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](120, "div", 74);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](121, " Do you want to pass the existing Order(s) to the assigned carrier? ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](122, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](123, "div", 75);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](124, "button", 76);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_124_listener() {
            return ctx.IscreateFreightOrders(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](125, " No ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](126, "button", 77);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_126_listener() {
            return ctx.IscreateFreightOrders(true);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](127, " Yes ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](128, "div", 78);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](129, "div", 79);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](130, "div", 67);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](131, "div", 68);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](132, "div", 69);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](133, "h4", 70);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](134, "Carrier CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](135, "button", 71, 80);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](137, "span", 81);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](138, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](139, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](140, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](141, "span", 82);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](142, "a", 83);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](143, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](144, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](145, "div", 84);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](146, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](147, "label", 85);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](148, "input", 86);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_148_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function CarrierCompaniesComponent_Template_input_change_148_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](149, "div", 87);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](150, " Note : Please use the .csv template for uploading your Carrier details. Follow the required and optional field guidelines. ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](151, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](152, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](153, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](154, "label", 88);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](155, " * Do you want to pass the existing Order(s) to the assigned carrier? ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](156, "div", 89);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](157, "input", 90);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_157_listener($event) {
            return ctx.IsCreateFreightOrder = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](158, "label", 91);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](159, "Yes");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](160, "div", 89);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](161, "input", 92);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_input_ngModelChange_161_listener($event) {
            return ctx.IsCreateFreightOrder = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](162, "label", 93);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](163, "No");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](164, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](165, "div", 94);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](166, "input", 95);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_input_click_166_listener() {
            return ctx.onFileUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](167, "div", 96);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](168, "div", 97);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](169, "div", 67);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](170, "div", 98);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](171, "h4", 99);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](172, "Add/Edit Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](173, "button", 100);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](174, "span", 101);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](175, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](176, "div", 68);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](177, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](178, "div", 84);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](179, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](180, "label", 102);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](181, "Select Email ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](182, "ng-multiselect-dropdown", 103);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CarrierCompaniesComponent_Template_ng_multiselect_dropdown_ngModelChange_182_listener($event) {
            return ctx.editEmailDetails = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](183, "div", 104);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](184, "button", 105);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CarrierCompaniesComponent_Template_button_click_184_listener() {
            return ctx.SaveJobEmail();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](185, "Save");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isUpdate)("ngIfElse", _r1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isUpdate && ctx.IsJobDeselect);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsSuccess);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isUpdate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isUpdate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isUpdate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.list1Search);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx.SelectedCount, " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.availableJobSelectAll);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](41, 31, ctx.availableJobs, ctx.list1Search));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.list2Search);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.assginedJobSelectAll);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind2"](66, 34, ctx.assignedJobs, ctx.list2Search));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("hidden", true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.assignedCarrierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsDisplayLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.IsCreateFreightOrder);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.IsCreateFreightOrder);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", ctx.IsCreateFreightOrder == null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Email")("ngModel", ctx.editEmailDetails)("settings", ctx.multiDropdownSettings)("data", ctx.getCarrierEmailsById());
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["Sidebar"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["CheckboxControlValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RadioControlValueAccessor"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__["ɵc"]],
      pipes: [_search_filter__WEBPACK_IMPORTED_MODULE_10__["startsWithJobPipe"]],
      styles: ["mwl-confirmation-popover-window .popover.bottom {\r\n    margin-top: 65px;\r\n}\r\n\r\n.carrier-assignment {\r\n    height: calc(100vh - 192px);\r\n    overflow-y: auto;\r\n    overflow-x: hidden;\r\n}\r\n\r\naside {\r\n    width: 80%;\r\n}\r\n\r\n.dual-list .list-group {\r\n    margin-top: 8px;\r\n    height: 300px;\r\n    overflow-y: auto;\r\n}\r\n\r\n.dual-list .list-group .list-group-item {\r\n        border: 0px solid #e7eaec;\r\n    }\r\n\r\n.dual-list .list-group .list-group-item:hover {\r\n        background-color: #3a8feb85;\r\n        border-color: #f2f3f485;\r\n    }\r\n\r\n.dual-list .list-group .list-group-item.active a {\r\n    color: #ffffff !important;\r\n}\r\n\r\n.dual-list .list-group .list-group-item.active i {\r\n        color: #ffffff !important;\r\n    }\r\n\r\n.list-left li, .list-right li {\r\n        cursor: pointer;\r\n        text-align: left;\r\n    }\r\n\r\n.list-arrows {\r\n    padding-top: 100px;\r\n}\r\n\r\n.list-arrows button {\r\n        margin-bottom: 20px;\r\n    }\r\n\r\n.chk-custom-all {\r\n    max-width: 40px;\r\n    padding: 6px 12px;\r\n    float: right !important;\r\n}\r\n\r\n.ng-sidebar{\r\n    z-index:3 !important;\r\n}\r\n\r\n.trncate-text {\r\n    white-space: nowrap;\r\n    overflow: hidden;\r\n    text-overflow: ellipsis;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvY2Fycmllci1jb21wYW5pZXMvY2Fycmllci1jb21wYW5pZXMuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLGdCQUFnQjtBQUNwQjs7QUFFQTtJQUNJLDJCQUEyQjtJQUMzQixnQkFBZ0I7SUFDaEIsa0JBQWtCO0FBQ3RCOztBQUVBO0lBQ0ksVUFBVTtBQUNkOztBQUVBO0lBQ0ksZUFBZTtJQUNmLGFBQWE7SUFDYixnQkFBZ0I7QUFDcEI7O0FBRUc7UUFDSyx5QkFBeUI7SUFDN0I7O0FBRUE7UUFDSSwyQkFBMkI7UUFDM0IsdUJBQXVCO0lBQzNCOztBQUdKO0lBQ0kseUJBQXlCO0FBQzdCOztBQUNJO1FBQ0kseUJBQXlCO0lBQzdCOztBQUVBO1FBQ0ksZUFBZTtRQUNmLGdCQUFnQjtJQUNwQjs7QUFFSjtJQUNJLGtCQUFrQjtBQUN0Qjs7QUFFSTtRQUNJLG1CQUFtQjtJQUN2Qjs7QUFFSjtJQUNJLGVBQWU7SUFDZixpQkFBaUI7SUFDakIsdUJBQXVCO0FBQzNCOztBQUNBO0lBQ0ksb0JBQW9CO0FBQ3hCOztBQUVBO0lBQ0ksbUJBQW1CO0lBQ25CLGdCQUFnQjtJQUNoQix1QkFBdUI7QUFDM0IiLCJmaWxlIjoic3JjL2FwcC9jYXJyaWVyLWNvbXBhbmllcy9jYXJyaWVyLWNvbXBhbmllcy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsibXdsLWNvbmZpcm1hdGlvbi1wb3BvdmVyLXdpbmRvdyAucG9wb3Zlci5ib3R0b20ge1xyXG4gICAgbWFyZ2luLXRvcDogNjVweDtcclxufVxyXG5cclxuLmNhcnJpZXItYXNzaWdubWVudCB7XHJcbiAgICBoZWlnaHQ6IGNhbGMoMTAwdmggLSAxOTJweCk7XHJcbiAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgb3ZlcmZsb3cteDogaGlkZGVuO1xyXG59XHJcblxyXG5hc2lkZSB7XHJcbiAgICB3aWR0aDogODAlO1xyXG59XHJcblxyXG4uZHVhbC1saXN0IC5saXN0LWdyb3VwIHtcclxuICAgIG1hcmdpbi10b3A6IDhweDtcclxuICAgIGhlaWdodDogMzAwcHg7XHJcbiAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG59XHJcblxyXG4gICAuZHVhbC1saXN0IC5saXN0LWdyb3VwIC5saXN0LWdyb3VwLWl0ZW0ge1xyXG4gICAgICAgIGJvcmRlcjogMHB4IHNvbGlkICNlN2VhZWM7XHJcbiAgICB9XHJcblxyXG4gICAgLmR1YWwtbGlzdCAubGlzdC1ncm91cCAubGlzdC1ncm91cC1pdGVtOmhvdmVyIHtcclxuICAgICAgICBiYWNrZ3JvdW5kLWNvbG9yOiAjM2E4ZmViODU7XHJcbiAgICAgICAgYm9yZGVyLWNvbG9yOiAjZjJmM2Y0ODU7XHJcbiAgICB9XHJcblxyXG5cclxuLmR1YWwtbGlzdCAubGlzdC1ncm91cCAubGlzdC1ncm91cC1pdGVtLmFjdGl2ZSBhIHtcclxuICAgIGNvbG9yOiAjZmZmZmZmICFpbXBvcnRhbnQ7XHJcbn1cclxuICAgIC5kdWFsLWxpc3QgLmxpc3QtZ3JvdXAgLmxpc3QtZ3JvdXAtaXRlbS5hY3RpdmUgaSB7XHJcbiAgICAgICAgY29sb3I6ICNmZmZmZmYgIWltcG9ydGFudDtcclxuICAgIH1cclxuXHJcbiAgICAubGlzdC1sZWZ0IGxpLCAubGlzdC1yaWdodCBsaSB7XHJcbiAgICAgICAgY3Vyc29yOiBwb2ludGVyO1xyXG4gICAgICAgIHRleHQtYWxpZ246IGxlZnQ7XHJcbiAgICB9XHJcblxyXG4ubGlzdC1hcnJvd3Mge1xyXG4gICAgcGFkZGluZy10b3A6IDEwMHB4O1xyXG59XHJcblxyXG4gICAgLmxpc3QtYXJyb3dzIGJ1dHRvbiB7XHJcbiAgICAgICAgbWFyZ2luLWJvdHRvbTogMjBweDtcclxuICAgIH1cclxuXHJcbi5jaGstY3VzdG9tLWFsbCB7XHJcbiAgICBtYXgtd2lkdGg6IDQwcHg7XHJcbiAgICBwYWRkaW5nOiA2cHggMTJweDtcclxuICAgIGZsb2F0OiByaWdodCAhaW1wb3J0YW50O1xyXG59XHJcbi5uZy1zaWRlYmFye1xyXG4gICAgei1pbmRleDozICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi50cm5jYXRlLXRleHQge1xyXG4gICAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcclxuICAgIG92ZXJmbG93OiBoaWRkZW47XHJcbiAgICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcclxufVxyXG4iXX0= */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CarrierCompaniesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-carrier-companies',
          templateUrl: './carrier-companies.component.html',
          styleUrls: ['./carrier-companies.component.css'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_5__["FormBuilder"]
        }, {
          type: _service_assigncarrier_service__WEBPACK_IMPORTED_MODULE_1__["AssigncarrierService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTableDirective"]]
        }],
        btnOpenModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnOpenModal']
        }],
        btnCloseModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseModal']
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }],
        confirmationBox: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['confirmationBox']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/carrier-companies/lazy-loading/carrier-companies-routing.module.ts": function srcAppCarrierCompaniesLazyLoadingCarrierCompaniesRoutingModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierCompaniesRoutingModule", function () {
      return CarrierCompaniesRoutingModule;
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


    var _carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../carrier-companies.component */
    "./src/app/carrier-companies/carrier-companies.component.ts");

    var routeCarrierCompanies = [{
      path: '',
      component: _carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__["CarrierCompaniesComponent"],
      children: [{
        path: '',
        component: _carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__["CarrierCompaniesComponent"],
        data: {
          title: 'Carrier Companies'
        }
      }]
    }];

    var CarrierCompaniesRoutingModule = function CarrierCompaniesRoutingModule() {
      _classCallCheck(this, CarrierCompaniesRoutingModule);
    };

    CarrierCompaniesRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: CarrierCompaniesRoutingModule
    });
    CarrierCompaniesRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function CarrierCompaniesRoutingModule_Factory(t) {
        return new (t || CarrierCompaniesRoutingModule)();
      },
      imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeCarrierCompanies)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](CarrierCompaniesRoutingModule, {
        imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CarrierCompaniesRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeCarrierCompanies)],
          exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/carrier-companies/lazy-loading/carrier-companies.module.ts": function srcAppCarrierCompaniesLazyLoadingCarrierCompaniesModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierCompaniesModule", function () {
      return CarrierCompaniesModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _carrier_companies_routing_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./carrier-companies-routing.module */
    "./src/app/carrier-companies/lazy-loading/carrier-companies-routing.module.ts");
    /* harmony import */


    var _carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../carrier-companies.component */
    "./src/app/carrier-companies/carrier-companies.component.ts");
    /* harmony import */


    var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");

    var CarrierCompaniesModule = function CarrierCompaniesModule() {
      _classCallCheck(this, CarrierCompaniesModule);
    };

    CarrierCompaniesModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: CarrierCompaniesModule
    });
    CarrierCompaniesModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function CarrierCompaniesModule_Factory(t) {
        return new (t || CarrierCompaniesModule)();
      },
      imports: [[_carrier_companies_routing_module__WEBPACK_IMPORTED_MODULE_1__["CarrierCompaniesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](CarrierCompaniesModule, {
        declarations: [_carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__["CarrierCompaniesComponent"]],
        imports: [_carrier_companies_routing_module__WEBPACK_IMPORTED_MODULE_1__["CarrierCompaniesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CarrierCompaniesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_carrier_companies_component__WEBPACK_IMPORTED_MODULE_2__["CarrierCompaniesComponent"]],
          imports: [_carrier_companies_routing_module__WEBPACK_IMPORTED_MODULE_1__["CarrierCompaniesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"]]
        }]
      }], null, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=carrier-companies-lazy-loading-carrier-companies-module-es5.js.map