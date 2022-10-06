function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["self-service-alias-self-service-alias-module"], {
  /***/
  "./src/app/carrier/models/CarrierDetailsViewModel.ts": function srcAppCarrierModelsCarrierDetailsViewModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierDetailsViewModel", function () {
      return CarrierDetailsViewModel;
    });
    /* harmony import */


    var src_app_lfv_dashboard_LiftFileModels__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! src/app/lfv-dashboard/LiftFileModels */
    "./src/app/lfv-dashboard/LiftFileModels.ts");

    var CarrierDetailsViewModel = function CarrierDetailsViewModel() {
      _classCallCheck(this, CarrierDetailsViewModel);

      this.AssignedTerminalId = new src_app_lfv_dashboard_LiftFileModels__WEBPACK_IMPORTED_MODULE_0__["DropDownItem"]();
    };
    /***/

  },

  /***/
  "./src/app/self-service-alias/company-carrier-mapping/company-carrier-mapping.component.ts": function srcAppSelfServiceAliasCompanyCarrierMappingCompanyCarrierMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CompanyCarrierMappingComponent", function () {
      return CompanyCarrierMappingComponent;
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


    var src_app_carrier_models_CarrierDetailsViewModel__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/carrier/models/CarrierDetailsViewModel */
    "./src/app/carrier/models/CarrierDetailsViewModel.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/dispatcher.service */
    "./src/app/carrier/service/dispatcher.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    function CompanyCarrierMappingComponent_li_15_Template(rf, ctx) {
      if (rf & 1) {
        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CompanyCarrierMappingComponent_li_15_Template_li_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r5);

          var carrier_r3 = ctx.$implicit;

          var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r4.onCarrierSelected(carrier_r3);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carrier_r3 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("selected", carrier_r3.Name == ctx_r0.SelectedCarrierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", carrier_r3.Name, " ");
      }
    }

    function CompanyCarrierMappingComponent_tr_44_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "button", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("cancel", function CompanyCarrierMappingComponent_tr_44_Template_button_cancel_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var carrier_r6 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.cancelMapping(carrier_r6);
        })("confirm", function CompanyCarrierMappingComponent_tr_44_Template_button_confirm_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var carrier_r6 = ctx.$implicit;
          var i_r7 = ctx.index;

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.updateMapping(carrier_r6, i_r7);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](9, "i", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "button", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function CompanyCarrierMappingComponent_tr_44_Template_button_confirm_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var carrier_r6 = ctx.$implicit;

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r11.deleteMapping(carrier_r6);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carrier_r6 = ctx.$implicit;
        var i_r7 = ctx.index;

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](carrier_r6.AssignedTerminalIdName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "CarrierName_", i_r7, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", carrier_r6.CarrierName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "CarrierID_", i_r7, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", carrier_r6.AssignedCarrierId, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r1.popoverTitle)("confirmText", ctx_r1.confirmButtonText)("cancelText", ctx_r1.cancelButtonText);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r1.popoverTitle)("confirmText", ctx_r1.confirmButtonText)("cancelText", ctx_r1.cancelButtonText);
      }
    }

    function CompanyCarrierMappingComponent_div_45_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 35);

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

    var CompanyCarrierMappingComponent = /*#__PURE__*/function () {
      function CompanyCarrierMappingComponent(carrierService, dispatcherService) {
        _classCallCheck(this, CompanyCarrierMappingComponent);

        this.carrierService = carrierService;
        this.dispatcherService = dispatcherService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.isShowCountryDDL = false;
        this.CarrierDetails = [];
        this.IsLoading = false;
        this.ddlSettingsById = {}; //TerminalList = [];  

        this.carrierList = []; //Carriermapping: CarrierDetailsViewModel = {};

        this.SelectedCarrierList = [];
        this.SelectedTerminalList = [];
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.AssignedTerminalIdList = [];
        this.carriers = [];
      }

      _createClass(CompanyCarrierMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.ddlSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.initializeCarrierGridInfo(); // this.getCarrierMappingsData();

          this.isShowCountryDDL = false;
          this.getTerminalIdsForMapping();
          this.getCarriers();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          // if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
          //     this.getDefaultServingCountry();
          // }
          // else {
          if (changes.countryId && changes.countryId.currentValue) {
            this.clearForm();
          } //}

        }
      }, {
        key: "getTerminalIdsForMapping",
        value: function getTerminalIdsForMapping() {
          var _this = this;

          this.IsLoading = true;
          this.AssignedTerminalIdList = [];
          this.carrierService.getAssignedTerminalIdsForMapping().subscribe(function (response) {
            // console.log({ "response": response});
            _this.AssignedTerminalIdList = response;
            _this.IsLoading = false;
          });
        }
      }, {
        key: "onCarrierNameSearched",
        value: function onCarrierNameSearched(event) {
          var keyword = event.target.value.toLowerCase();
          this.carriers = this.carrierList.slice().filter(function (elem) {
            return elem.Name && elem.Name.toLowerCase().indexOf(keyword) >= 0;
          });
        }
      }, {
        key: "onCarrierSelected",
        value: function onCarrierSelected(event) {
          this.SelectedCarrierName = event.Name;
          this.selectedCarrierId = event.Id;
          this.carriers = this.carrierList.slice();
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this2 = this;

          this.dispatcherService.GetCarriersForSupplier().subscribe(function (data) {
            _this2.carrierList = data;
          });
        }
      }, {
        key: "getCarrierMappingsData",
        value: function getCarrierMappingsData() {
          var _this3 = this;

          this.isShowLoader = true;
          this.carrierService.getCarrierData(this.countryId).subscribe(function (data) {
            _this3.CarrierDetails = data;
            _this3.isShowLoader = false;

            _this3.dtTrigger.next();
          });
        }
      }, {
        key: "deleteMapping",
        value: function deleteMapping(carrier) {
          var _this4 = this;

          var mappingId = carrier.Id;
          this.carrierService.deleteCarrierMapping(mappingId).subscribe(function (response) {
            if (response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);
              $("#carrier-grid-datatable").DataTable().clear().destroy();

              _this4.getCarrierMappingsData();
            } else if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "setPanelHeader",
        value: function setPanelHeader(headerText) {
          this.HeaderText = headerText;
        }
      }, {
        key: "cancelMapping",
        value: function cancelMapping(carrier) {
          this.getCarrierMappingsData();
        }
      }, {
        key: "updateMapping",
        value: function updateMapping(carrier, rowIndex) {
          var _this5 = this;

          var assignedCarrierName = jQuery('#CarrierName_' + rowIndex).text();
          var assignedCarrierId = jQuery('#CarrierID_' + rowIndex).text();

          if (assignedCarrierName && assignedCarrierId) {
            if (assignedCarrierName.trim() == carrier.CarrierName && assignedCarrierId.trim() == carrier.AssignedCarrierId) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Update Assigned CarrierID/CarrierName ", undefined, undefined);
              return;
            }

            var carrierInput = new src_app_carrier_models_CarrierDetailsViewModel__WEBPACK_IMPORTED_MODULE_2__["CarrierDetailsViewModel"]();
            carrierInput.AssignedTerminalId.Id = carrier.TerminalCompanyAliasId;
            carrierInput.AssignedTerminalId.Name = carrier.AssignedTerminalIdName;
            carrierInput.Id = carrier.Id;
            carrierInput.CountryId = this.countryId;
            carrierInput.CarrierName = carrier.CarrierName;
            carrierInput.TerminalCompanyAliasId = carrier.TerminalCompanyAliasId;
            carrierInput.AssignedCarrierId = carrier.AssignedCarrierId;

            if (assignedCarrierName.trim() != carrier.CarrierName) {
              carrierInput.CarrierName = assignedCarrierName.trim();
            }

            if (assignedCarrierId.trim() != carrier.AssignedCarrierId) {
              carrierInput.AssignedCarrierId = assignedCarrierId.trim();
            }

            this.carrierService.SaveCarrierMapping(carrierInput).subscribe(function (data) {
              _this5.IsLoading = false;
              _this5.isShowLoader = false;

              if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                $("#carrier-grid-datatable").DataTable().clear().destroy();

                _this5.getCarrierMappingsData(); //this.updatedCarrierName = null;
                //this.updatedCarrierId = null;
                //this.selectedMappingId = null;

              } else if (data.StatusCode == 1) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              }
            });
          } else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning("Update Assigned CarrierID/CarrierName ", undefined, undefined);
            return;
          }
        } //editCarrier(carrier) {
        //    this.CarrierDataToBeSend = carrier;
        //}

      }, {
        key: "clearForm",
        value: function clearForm() {
          $("#carrier-grid-datatable").DataTable().clear().destroy();
          this.getTerminalIdsForMapping();
          this.getCarrierMappingsData();
        }
      }, {
        key: "initializeCarrierGridInfo",
        value: function initializeCarrierGridInfo() {
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
              title: 'Carrier Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Carrier Details',
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
        key: "onSubmit",
        value: function onSubmit() {
          var _this6 = this;

          if (this.IsValidForm()) {
            this.IsLoading = true;
            this.isShowLoader = true;
            var carrierMapping = new src_app_carrier_models_CarrierDetailsViewModel__WEBPACK_IMPORTED_MODULE_2__["CarrierDetailsViewModel"]();
            carrierMapping.AssignedTerminalId = this.SelectedTerminalList[0];
            carrierMapping.CarrierName = this.SelectedCarrierName;
            carrierMapping.AssignedCarrierId = this.AssignedCarrierId;
            carrierMapping.CountryId = this.countryId;
            this.carrierService.SaveCarrierMapping(carrierMapping).subscribe(function (data) {
              _this6.IsLoading = false;
              _this6.isShowLoader = false;

              if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
                $("#carrier-grid-datatable").DataTable().clear().destroy();

                _this6.getCarrierMappingsData();

                _this6.SelectedTerminalList = [];
                _this6.SelectedCarrierName = "";
                _this6.AssignedCarrierId = "";
              } else if (data.StatusCode == 1) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              }
            });
          } else {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Terminal ID/CarrierName/CarrierID is required", undefined, undefined);
          }
        }
      }, {
        key: "IsValidForm",
        value: function IsValidForm() {
          var isValid = false;
          var selectedTerminalId = this.IsValidValue(this.SelectedTerminalList[0]);
          var selectedCarrierName = this.IsValidValue(this.SelectedCarrierName);
          var assignedCarrierId = this.IsValidValue(this.AssignedCarrierId);

          if (selectedTerminalId && selectedCarrierName && assignedCarrierId) {
            isValid = true;
          }

          return isValid;
        }
      }, {
        key: "IsValidValue",
        value: function IsValidValue(value) {
          if (value) {
            return true;
          } else {
            return false;
          }
        }
      }]);

      return CompanyCarrierMappingComponent;
    }();

    CompanyCarrierMappingComponent.ɵfac = function CompanyCarrierMappingComponent_Factory(t) {
      return new (t || CompanyCarrierMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__["DispatcherService"]));
    };

    CompanyCarrierMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CompanyCarrierMappingComponent,
      selectors: [["app-company-carrier-mapping"]],
      inputs: {
        countryId: "countryId"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 46,
      vars: 15,
      consts: [[1, "col-sm-12"], [1, "row"], [1, "well", "col-sm-12"], [1, "col-md-3"], [1, "form-group"], [3, "placeholder", "ngModel", "settings", "data", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "input", "ngModelChange"], [1, "auto-select", "border-dash"], [3, "click", 4, "ngFor", "ngForOf"], ["type", "hidden", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "text-left", "form-buttons", "mt20"], ["id", "submit-product-mapping-form", "type", "submit", "aria-invalid", "false", 1, "mt4", "btn", "btn-lg", "btn-default", "valid", 3, "ngClass", "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-carrier-grid", 1, "table-responsive"], ["id", "carrier-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "TerminalID"], ["data-key", "CarrierName"], ["data-key", "AssignedCarrierId"], ["data-key", "Action"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], [3, "click"], ["contenteditable", "true", 1, "edit-td", 3, "id"], [1, "text-center"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "cancel", "confirm"], ["alt", "Update", "title", "Update", 1, "fs21", "fas", "fa-save", "color-green"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CompanyCarrierMappingComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Terminal ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "ng-multiselect-dropdown", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CompanyCarrierMappingComponent_Template_ng_multiselect_dropdown_ngModelChange_8_listener($event) {
            return ctx.SelectedTerminalList = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function CompanyCarrierMappingComponent_Template_input_input_13_listener($event) {
            return ctx.onCarrierNameSearched($event);
          })("ngModelChange", function CompanyCarrierMappingComponent_Template_input_ngModelChange_13_listener($event) {
            return ctx.SelectedCarrierName = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "ul", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, CompanyCarrierMappingComponent_li_15_Template, 2, 2, "li", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CompanyCarrierMappingComponent_Template_input_ngModelChange_16_listener($event) {
            return ctx.selectedCarrierId = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CompanyCarrierMappingComponent_Template_input_ngModelChange_21_listener($event) {
            return ctx.AssignedCarrierId = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "button", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CompanyCarrierMappingComponent_Template_button_click_24_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Assign");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "table", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "th", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Terminal ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "th", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "th", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "CarrierID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "th", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](42, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](44, CompanyCarrierMappingComponent_tr_44_Template, 12, 11, "tr", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](45, CompanyCarrierMappingComponent_div_45_Template, 5, 0, "div", 24);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal ID")("ngModel", ctx.SelectedTerminalList)("settings", ctx.ddlSettingsById)("data", ctx.AssignedTerminalIdList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.SelectedCarrierName);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.carriers);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedCarrierId);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.AssignedCarrierId);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](13, _c0, ctx.IsLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CarrierDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);
        }
      },
      directives: [ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["DefaultValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_10__["ɵc"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9jb21wYW55LWNhcnJpZXItbWFwcGluZy9jb21wYW55LWNhcnJpZXItbWFwcGluZy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CompanyCarrierMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-company-carrier-mapping',
          templateUrl: './company-carrier-mapping.component.html',
          styleUrls: ['./company-carrier-mapping.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]
        }, {
          type: src_app_carrier_service_dispatcher_service__WEBPACK_IMPORTED_MODULE_5__["DispatcherService"]
        }];
      }, {
        countryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/create-terminal-item-code/create-terminal-item-code.component.ts": function srcAppSelfServiceAliasCreateTerminalItemCodeCreateTerminalItemCodeComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateTerminalItemCodeComponent", function () {
      return CreateTerminalItemCodeComponent;
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


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/http-generic.service */
    "./src/app/http-generic.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function CreateTerminalItemCodeComponent_div_11_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateTerminalItemCodeComponent_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateTerminalItemCodeComponent_div_11_div_1_Template, 2, 0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.terminalmappingForm.get("terminalSupplierId").errors.required);
      }
    }

    function CreateTerminalItemCodeComponent_div_19_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateTerminalItemCodeComponent_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateTerminalItemCodeComponent_div_19_div_1_Template, 2, 0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.terminalmappingForm.get("itemDescriptionId").errors.required);
      }
    }

    function CreateTerminalItemCodeComponent_span_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Multiple Item Code separated by comma can be entered");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateTerminalItemCodeComponent_div_29_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateTerminalItemCodeComponent_div_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateTerminalItemCodeComponent_div_29_div_1_Template, 2, 0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.terminalmappingForm.get("itemCode").errors.required);
      }
    }

    function CreateTerminalItemCodeComponent_div_38_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateTerminalItemCodeComponent_div_38_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateTerminalItemCodeComponent_div_38_div_1_Template, 2, 0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.terminalmappingForm.get("effectiveDate").errors.required);
      }
    }

    function CreateTerminalItemCodeComponent_div_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 21);

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

    var CreateTerminalItemCodeComponent = /*#__PURE__*/function () {
      function CreateTerminalItemCodeComponent(httpService, _fb) {
        _classCallCheck(this, CreateTerminalItemCodeComponent);

        this.httpService = httpService;
        this._fb = _fb;
        this.result = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.TerminalSupplierList = [];
        this.TerminalSupplierDescList = [];
        this.ddlSettingsById = {};
        this.TerminalItemCodeMappingModel = {};
        this.GetTerminalSupplierUrl = '/Carrier/SelfServiceAlias/GetTerminalSupplierAndDesc?CountryId=';
        this.PostSaveTerminalMappingUrl = '/Carrier/SelfServiceAlias/SaveTerminalItemCodeMapping';
        this.PostUpdateTerminalMappingUrl = '/Carrier/SelfServiceAlias/UpdateTerminalItemCodeMapping';
        this.selectedTerminalSupplier = [];
        this.selectedItemDesc = [];
        this.minDate = new Date();
        this.maxDate = new Date();
      }

      _createClass(CreateTerminalItemCodeComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
          this.ddlSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            // selectAllText: 'Select All',
            // unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.init();
        }
      }, {
        key: "init",
        value: function init() {
          this.terminalmappingForm = this._fb.group({
            id: [''],
            terminalSupplierId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required],
            itemDescriptionId: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required],
            itemCode: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required],
            effectiveDate: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required],
            expiryDate: ['']
          });
          this.getTerminalSupplier();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.countryId && change.countryId.currentValue) {
            this.countryId = change.countryId.currentValue;

            if (change.countryId.previousValue && change.countryId.currentValue != change.countryId.previousValue) {
              this.getTerminalSupplier();
            }
          }
        }
      }, {
        key: "getTerminalSupplier",
        value: function getTerminalSupplier() {
          var _this7 = this;

          this.IsLoading = true;
          this.httpService.fetchAll("".concat(this.GetTerminalSupplierUrl).concat(this.countryId)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["first"])()).subscribe(function (result) {
            _this7.IsLoading = false;
            _this7.TerminalSupplierList = result.TerminalSupplierList;
            _this7.TerminalSupplierDescList = result.TerminalDescriptionList;
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          for (var c in this.terminalmappingForm.controls) {
            this.terminalmappingForm.controls[c].markAsTouched();
          }

          if (this.terminalmappingForm.valid) {
            if (this.terminalmappingForm && this.terminalmappingForm.controls.id.value) {
              var x = this.terminalmappingForm.controls.itemCode.value.split(",");

              if (x && x.length > 1) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Comma Seperated item code for update is not allowed', undefined, undefined);
                return false;
              }

              this.updateTerminalMapping();
            } else this.addTerminalMapping();
          }
        }
      }, {
        key: "addTerminalMapping",
        value: function addTerminalMapping() {
          var _this8 = this;

          this.TerminalItemCodeMappingModel = {};
          this.IsLoading = true;
          this.TerminalItemCodeMappingModel.TerminalSupplierId = this.terminalmappingForm.controls.terminalSupplierId.value[0].Id;
          this.TerminalItemCodeMappingModel.ItemDescriptionId = this.terminalmappingForm.controls.itemDescriptionId.value[0].Id;
          this.TerminalItemCodeMappingModel.ItemCode = this.terminalmappingForm.controls.itemCode.value;
          this.TerminalItemCodeMappingModel.EffectiveDate = this.terminalmappingForm.controls.effectiveDate.value;
          this.TerminalItemCodeMappingModel.ExpiryDate = this.terminalmappingForm.controls.expiryDate.value;
          this.httpService.postData(this.PostSaveTerminalMappingUrl, this.TerminalItemCodeMappingModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["first"])()).subscribe(function (res) {
            if (res.StatusCode == 0) {
              _this8.result.emit(true);

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(res.StatusMessage, undefined, undefined);
            } else src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(res.StatusMessage, undefined, undefined);

            _this8.IsLoading = false;
          });
        }
      }, {
        key: "updateTerminalMapping",
        value: function updateTerminalMapping() {
          var _this9 = this;

          this.TerminalItemCodeMappingModel = {};
          this.IsLoading = true;
          this.TerminalItemCodeMappingModel.Id = this.terminalmappingForm.controls.id.value;
          this.TerminalItemCodeMappingModel.TerminalSupplierId = this.terminalmappingForm.controls.terminalSupplierId.value[0].Id;
          this.TerminalItemCodeMappingModel.ItemDescriptionId = this.terminalmappingForm.controls.itemDescriptionId.value[0].Id;
          this.TerminalItemCodeMappingModel.ItemCode = this.terminalmappingForm.controls.itemCode.value;
          this.TerminalItemCodeMappingModel.EffectiveDate = this.terminalmappingForm.controls.effectiveDate.value;
          this.TerminalItemCodeMappingModel.ExpiryDate = this.terminalmappingForm.controls.expiryDate.value;
          this.httpService.postData(this.PostUpdateTerminalMappingUrl, this.TerminalItemCodeMappingModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["first"])()).subscribe(function (res) {
            if (res.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(res.StatusMessage, undefined, undefined);

              _this9.result.emit(true);
            } else src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(res.StatusMessage, undefined, undefined);

            _this9.IsLoading = false;
          });
        }
      }, {
        key: "setexpiryDate",
        value: function setexpiryDate($event) {
          this.terminalmappingForm.controls.expiryDate.setValue($event);
        }
      }, {
        key: "seteffectiveDate",
        value: function seteffectiveDate($event) {
          this.terminalmappingForm.controls.effectiveDate.setValue($event);
        }
      }]);

      return CreateTerminalItemCodeComponent;
    }();

    CreateTerminalItemCodeComponent.ɵfac = function CreateTerminalItemCodeComponent_Factory(t) {
      return new (t || CreateTerminalItemCodeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_4__["HttpGenericService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]));
    };

    CreateTerminalItemCodeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CreateTerminalItemCodeComponent,
      selectors: [["app-create-terminal-item-code"]],
      inputs: {
        countryId: "countryId"
      },
      outputs: {
        result: "result"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 49,
      vars: 23,
      consts: [["name", "terminalmappingForm", "autocomplete", "off", 3, "formGroup", "ngSubmit"], [1, "row"], [1, "col-sm-12"], [1, "col-sm-6"], [1, "form-group"], [1, "color-maroon"], ["formControlName", "terminalSupplierId", 1, "single-select", 3, "placeholder", "ngModel", "settings", "data", "ngModelChange"], ["class", "color-maroon", 4, "ngIf"], ["formControlName", "itemDescriptionId", 1, "single-select", 3, "placeholder", "ngModel", "settings", "data", "ngModelChange"], ["type", "text", "formControlName", "itemCode", 1, "form-control"], ["class", "text-info", 4, "ngIf"], ["type", "text", "placeholder", "Effective Date", "formControlName", "effectiveDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "onDateChange"], ["type", "text", "placeholder", "Expiry Date", "formControlName", "expiryDate", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "minDate", "maxDate", "format", "onDateChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["id", "submit-terminal-mapping-form", "type", "submit", "aria-invalid", "false", 1, "mt4", "btn", "btn-lg", "btn-primary", "valid", 3, "ngClass"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], [1, "text-info"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function CreateTerminalItemCodeComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function CreateTerminalItemCodeComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Terminal Supplier ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "ng-multiselect-dropdown", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateTerminalItemCodeComponent_Template_ng_multiselect_dropdown_ngModelChange_10_listener($event) {
            return ctx.selectedTerminalSupplier = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, CreateTerminalItemCodeComponent_div_11_Template, 2, 1, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Terminal Supplier Description ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "ng-multiselect-dropdown", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function CreateTerminalItemCodeComponent_Template_ng_multiselect_dropdown_ngModelChange_18_listener($event) {
            return ctx.selectedItemDesc = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, CreateTerminalItemCodeComponent_div_19_Template, 2, 1, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](27, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, CreateTerminalItemCodeComponent_span_28_Template, 2, 0, "span", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](29, CreateTerminalItemCodeComponent_div_29_Template, 2, 1, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Effective Date ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "input", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function CreateTerminalItemCodeComponent_Template_input_onDateChange_37_listener($event) {
            return ctx.seteffectiveDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](38, CreateTerminalItemCodeComponent_div_38_Template, 2, 1, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](42, "Expiry Date ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function CreateTerminalItemCodeComponent_Template_input_onDateChange_43_listener($event) {
            return ctx.setexpiryDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "button", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](47, " Assign ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](48, CreateTerminalItemCodeComponent_div_48_Template, 5, 0, "div", 15);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.terminalmappingForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal")("ngModel", ctx.selectedTerminalSupplier)("settings", ctx.ddlSettingsById)("data", ctx.TerminalSupplierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.terminalmappingForm.get("terminalSupplierId").invalid && ctx.terminalmappingForm.get("terminalSupplierId").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal Item Description")("ngModel", ctx.selectedItemDesc)("settings", ctx.ddlSettingsById)("data", ctx.TerminalSupplierDescList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.terminalmappingForm.get("itemDescriptionId").invalid && ctx.terminalmappingForm.get("itemDescriptionId").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"]((ctx.terminalmappingForm == null ? null : ctx.terminalmappingForm.get("id").value) ? "Terminal Item Code" : "Terminal Item Code (s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !(ctx.terminalmappingForm == null ? null : ctx.terminalmappingForm.get("id").value));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.terminalmappingForm.get("itemCode").invalid && ctx.terminalmappingForm.get("itemCode").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.terminalmappingForm.get("effectiveDate").invalid && ctx.terminalmappingForm.get("effectiveDate").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("minDate", ctx.minDate)("maxDate", ctx.maxDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](21, _c0, ctx.IsLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_5__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_7__["DatePicker"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgClass"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9jcmVhdGUtdGVybWluYWwtaXRlbS1jb2RlL2NyZWF0ZS10ZXJtaW5hbC1pdGVtLWNvZGUuY29tcG9uZW50LmNzcyJ9 */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateTerminalItemCodeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-create-terminal-item-code',
          templateUrl: './create-terminal-item-code.component.html',
          styleUrls: ['./create-terminal-item-code.component.css'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_4__["HttpGenericService"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }];
      }, {
        countryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        result: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/customer-mapping/customer-mapping.component.ts": function srcAppSelfServiceAliasCustomerMappingCustomerMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CustomerMappingComponent", function () {
      return CustomerMappingComponent;
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


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _edit_customer_edit_customer_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../edit-customer/edit-customer.component */
    "./src/app/self-service-alias/edit-customer/edit-customer.component.ts");

    function CustomerMappingComponent_tr_14_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Customer_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Customer_r3.CarrierAssignedCustomerId);
      }
    }

    function CustomerMappingComponent_tr_14_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CustomerMappingComponent_tr_14_Template(rf, ctx) {
      if (rf & 1) {
        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CustomerMappingComponent_tr_14_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r8);

          var Customer_r3 = ctx.$implicit;

          var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          ctx_r7.setPanelHeader("Edit Customer Id");
          return ctx_r7.editDriver(Customer_r3);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CustomerMappingComponent_tr_14_span_5_Template, 2, 1, "span", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CustomerMappingComponent_tr_14_span_6_Template, 2, 0, "span", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Customer_r3 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Customer_r3.BuyerName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Customer_r3.CarrierAssignedCustomerId != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Customer_r3.CarrierAssignedCustomerId == null);
      }
    }

    function CustomerMappingComponent_div_15_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CustomerMappingComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "app-edit-customer", 24, 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("getAllCustomerData", function CustomerMappingComponent_div_23_Template_app_edit_customer_getAllCustomerData_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.getAllCustomerData();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("CustomerDataToBeSend", ctx_r2.CustomerDataToBeSend);
      }
    }

    var CustomerMappingComponent = /*#__PURE__*/function () {
      function CustomerMappingComponent(carrierService) {
        _classCallCheck(this, CustomerMappingComponent);

        this.carrierService = carrierService;
        this.isShow = false;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.isShowCountryDDL = false;
        this.CarrierCustomers = [];
        this.IsLoading = false;
      }

      _createClass(CustomerMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getAllCustomerData();
          this.isShowCountryDDL = false;
        }
      }, {
        key: "getDriverDetails",
        value: function getDriverDetails() {
          $("#customer-grid-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "getAllCustomerData",
        value: function getAllCustomerData() {
          var _this10 = this;

          this.isShowLoader = true;
          this.carrierService.getAllCustomerData().subscribe(function (data) {
            _this10.CarrierCustomers = data;
            _this10.isShowLoader = false;
            $("#customer-grid-datatable").DataTable().clear().destroy();

            _this10.dtTrigger.next();
          });
        }
      }, {
        key: "setPanelHeader",
        value: function setPanelHeader(headerText) {
          this.HeaderText = headerText;
        }
      }, {
        key: "editDriver",
        value: function editDriver(Customer) {
          this.CustomerDataToBeSend = JSON.parse(JSON.stringify(Customer));
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          $("#customer-grid-datatable").DataTable().clear().destroy();
          this.getAllCustomerData();
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Customer Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Customer Details',
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
      }]);

      return CustomerMappingComponent;
    }();

    CustomerMappingComponent.ɵfac = function CustomerMappingComponent_Factory(t) {
      return new (t || CustomerMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_2__["CarrierService"]));
    };

    CustomerMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CustomerMappingComponent,
      selectors: [["app-customer-mapping"]],
      decls: 24,
      vars: 6,
      consts: [[1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-customer-grid", 1, "table-responsive"], ["id", "customer-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "BuyerName"], ["data-key", "CarrierAssignedCustomerId"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "driver-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["onclick", "slidePanel('#driver-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [3, "CustomerDataToBeSend", "getAllCustomerData"], ["ViewCustomer", ""]],
      template: function CustomerMappingComponent_Template(rf, ctx) {
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

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "th", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "My Customer Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, CustomerMappingComponent_tr_14_Template, 9, 3, "tr", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](15, CustomerMappingComponent_div_15_Template, 5, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "a", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](20, "i", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "h3", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, CustomerMappingComponent_div_23_Template, 3, 1, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.CarrierCustomers);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.HeaderText);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.CustomerDataToBeSend);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _edit_customer_edit_customer_component__WEBPACK_IMPORTED_MODULE_5__["EditCustomerComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9jdXN0b21lci1tYXBwaW5nL2N1c3RvbWVyLW1hcHBpbmcuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CustomerMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-customer-mapping',
          templateUrl: './customer-mapping.component.html',
          styleUrls: ['./customer-mapping.component.css']
        }]
      }], function () {
        return [{
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_2__["CarrierService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/edit-carrier-mapping/edit-carrier-mapping.component.ts": function srcAppSelfServiceAliasEditCarrierMappingEditCarrierMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "EditCarrierMappingComponent", function () {
      return EditCarrierMappingComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");

    var EditCarrierMappingComponent = /*#__PURE__*/function () {
      function EditCarrierMappingComponent() {
        _classCallCheck(this, EditCarrierMappingComponent);
      }

      _createClass(EditCarrierMappingComponent, [{
        key: "ngOnInit",
        value: //  public CarrierForm: FormGroup;
        //  public isShowLoader: boolean = false;
        //  @Input() CarrierDataToBeSend: CarrierDetailsViewModel;
        //  @Output() getCarrierData: EventEmitter<any> = new EventEmitter();
        //  constructor(public fb: FormBuilder, private carrierService: CarrierService) {
        //      this.CarrierForm = this.fb.group({
        //          Id: this.fb.control(0),          
        //          CarrierCompanyId: this.fb.control(''),
        //          CarrierName: this.fb.control(''),
        //          TotalOrders: this.fb.control(''),
        //          AssignedCarrierId: this.fb.control(''),
        //          CompanyId: this.fb.control(''),
        //      });
        //  }
        function ngOnInit() {}
      }]);

      return EditCarrierMappingComponent;
    }();

    EditCarrierMappingComponent.ɵfac = function EditCarrierMappingComponent_Factory(t) {
      return new (t || EditCarrierMappingComponent)();
    };

    EditCarrierMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: EditCarrierMappingComponent,
      selectors: [["app-edit-carrier-mapping"]],
      decls: 0,
      vars: 0,
      template: function EditCarrierMappingComponent_Template(rf, ctx) {},
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9lZGl0LWNhcnJpZXItbWFwcGluZy9lZGl0LWNhcnJpZXItbWFwcGluZy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EditCarrierMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-edit-carrier-mapping',
          templateUrl: './edit-carrier-mapping.component.html',
          styleUrls: ['./edit-carrier-mapping.component.css']
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/edit-customer/edit-customer.component.ts": function srcAppSelfServiceAliasEditCustomerEditCustomerComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "EditCustomerComponent", function () {
      return EditCustomerComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function EditCustomerComponent_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var EditCustomerComponent = /*#__PURE__*/function () {
      function EditCustomerComponent(fb, carrierService) {
        _classCallCheck(this, EditCustomerComponent);

        this.fb = fb;
        this.carrierService = carrierService;
        this.isShowLoader = false;
        this.getAllCustomerData = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.IdTaken = 5;
        this.CustomerForm = this.fb.group({
          Id: this.fb.control(0),
          TotalDDTCount: this.fb.control(''),
          TotalInvoiceCount: this.fb.control(''),
          BuyerCompanyId: this.fb.control(''),
          BuyerName: this.fb.control(''),
          TotalOrders: this.fb.control(''),
          CarrierAssignedCustomerId: this.fb.control('')
        });
      }

      _createClass(EditCustomerComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "checkMyCustomerIdDuplicate",
        value: function checkMyCustomerIdDuplicate(customerDetail) {
          var _this11 = this;

          this.carrierService.CustomerIdNotTaken(customerDetail).subscribe(function (data) {
            if (data.StatusCode == 0) {
              _this11.IdTaken = 0;

              _this11.submitForm(customerDetail);
            }

            if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_1__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              _this11.IdTaken = 2;
            }
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var CustomerDetail = {
            BuyerCompanyId: this.CustomerForm.get("BuyerCompanyId").value,
            BuyerName: this.CustomerForm.get("BuyerName").value,
            CarrierAssignedCustomerId: this.CustomerForm.get("CarrierAssignedCustomerId").value,
            Id: this.CustomerForm.get("Id").value
          };
          this.checkMyCustomerIdDuplicate(CustomerDetail);
        }
      }, {
        key: "submitForm",
        value: function submitForm(CustomerDetail) {
          var _this12 = this;

          this.isShowLoader = true;
          this.carrierService.saveAndUpdateCustomerMapping(CustomerDetail).subscribe(function (data) {
            _this12.isShowLoader = false;

            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_1__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              _this12.IdTaken = 5;
              closeSlidePanel();

              _this12.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_1__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_1__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this12.getAllCustomerData.emit();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.CustomerForm.reset();
        }
      }]);

      return EditCustomerComponent;
    }();

    EditCustomerComponent.ɵfac = function EditCustomerComponent_Factory(t) {
      return new (t || EditCustomerComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_3__["CarrierService"]));
    };

    EditCustomerComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: EditCustomerComponent,
      selectors: [["app-edit-customer"]],
      inputs: {
        CustomerDataToBeSend: "CustomerDataToBeSend"
      },
      outputs: {
        getAllCustomerData: "getAllCustomerData"
      },
      decls: 24,
      vars: 9,
      consts: [[3, "formGroup", "ngSubmit"], [1, "side-panel-body"], ["id", "Customer-Form", 1, "col-sm-12"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["type", "hidden", "formControlName", "BuyerCompanyId", 3, "ngModel", "ngModelChange"], ["type", "hidden", "formControlName", "TotalDDTCount", 3, "ngModel", "ngModelChange"], ["type", "hidden", "formControlName", "TotalInvoiceCount", 3, "ngModel", "ngModelChange"], ["type", "hidden", "formControlName", "Id", 3, "ngModel", "ngModelChange"], ["type", "text", "readonly", "", "formControlName", "BuyerName", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", "formControlName", "CarrierAssignedCustomerId", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["id", "submit-driver-form", "type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "disabled"], ["class", "loader", 4, "ngIf"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function EditCustomerComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function EditCustomerComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_6_listener($event) {
            return ctx.CustomerDataToBeSend.BuyerCompanyId = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "input", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_7_listener($event) {
            return ctx.CustomerDataToBeSend.TotalDDTCount = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_8_listener($event) {
            return ctx.CustomerDataToBeSend.TotalInvoiceCount = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_9_listener($event) {
            return ctx.CustomerDataToBeSend.Id = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_12_listener($event) {
            return ctx.CustomerDataToBeSend.BuyerName = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "My Customer Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function EditCustomerComponent_Template_input_ngModelChange_17_listener($event) {
            return ctx.CustomerDataToBeSend.CarrierAssignedCustomerId = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EditCustomerComponent_Template_input_click_20_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "button", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "Submit");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, EditCustomerComponent_div_23_Template, 5, 0, "div", 15);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.CustomerForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.BuyerCompanyId);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.TotalDDTCount);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.TotalInvoiceCount);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.Id);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.BuyerName);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.CustomerDataToBeSend.CarrierAssignedCustomerId);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", !ctx.CustomerForm.dirty || ctx.CustomerDataToBeSend.CarrierAssignedCustomerId.trim() == "" || ctx.CustomerDataToBeSend.CarrierAssignedCustomerId == null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9lZGl0LWN1c3RvbWVyL2VkaXQtY3VzdG9tZXIuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EditCustomerComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-edit-customer',
          templateUrl: './edit-customer.component.html',
          styleUrls: ['./edit-customer.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormBuilder"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_3__["CarrierService"]
        }];
      }, {
        CustomerDataToBeSend: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        getAllCustomerData: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalBulkPlantMappingsExternalBulkPlantMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalBulkPlantMappingsComponent", function () {
      return ExternalBulkPlantMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalBulkPlantMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var BulkPlant_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](BulkPlant_r4.TargetBulkPlantValue);
      }
    }

    function ExternalBulkPlantMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalBulkPlantMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var BulkPlant_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editBulkPlant(BulkPlant_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalBulkPlantMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalBulkPlantMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var BulkPlant_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](BulkPlant_r4.BulkPlantName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", BulkPlant_r4.TargetBulkPlantValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", BulkPlant_r4.TargetBulkPlantValue == null);
      }
    }

    function ExternalBulkPlantMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalBulkPlantMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Site/BulkPlant Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalBulkPlantMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editBulkPlantDetails.BulkPlantName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Site/BulkPlant Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalBulkPlantMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editBulkPlantDetails.TargetBulkPlantValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalBulkPlantMappings(ctx_r14.editBulkPlantDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editBulkPlantDetails.BulkPlantName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editBulkPlantDetails.TargetBulkPlantValue);
      }
    }

    var ExternalBulkPlantMappingsComponent = /*#__PURE__*/function () {
      function ExternalBulkPlantMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalBulkPlantMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalBulkPlantMappings = [];
      }

      _createClass(ExternalBulkPlantMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeBulkPlantCustomers();
          this.getBulkPlantsData();
        }
      }, {
        key: "getBulkPlantsData",
        value: function getBulkPlantsData() {
          var _this13 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getBulkPlantsForExternalMapping().subscribe(function (data) {
            _this13.externalBulkPlantMappings = data;
            _this13.isShowLoader = false;

            _this13.refreshDatatable();
          });
        }
      }, {
        key: "initializeBulkPlantCustomers",
        value: function initializeBulkPlantCustomers() {
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
              title: 'BulkPlant Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'BulkPlant Details',
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
        key: "editBulkPlant",
        value: function editBulkPlant(bulkPlant) {
          this.editBulkPlantDetails = JSON.parse(JSON.stringify(bulkPlant));
        }
      }, {
        key: "SaveExternalBulkPlantMappings",
        value: function SaveExternalBulkPlantMappings(bulkPlant) {
          var _this14 = this;

          bulkPlant.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalBulkPlantMappings(bulkPlant).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this14.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this14.isShowLoader = false;

            _this14.getBulkPlantsData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editBulkPlantDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this15 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadBulkPlantMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this15.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this15.getBulkPlantsData();
            }

            _this15.file = null;
          });
        }
      }]);

      return ExternalBulkPlantMappingsComponent;
    }();

    ExternalBulkPlantMappingsComponent.ɵfac = function ExternalBulkPlantMappingsComponent_Factory(t) {
      return new (t || ExternalBulkPlantMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalBulkPlantMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalBulkPlantMappingsComponent,
      selectors: [["app-external-bulk-plant-mappings"]],
      viewQuery: function ExternalBulkPlantMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb10"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-bulkPlant-grid", 1, "table-responsive"], ["id", "bulkPlant-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "BulkPlantName"], ["data-key", "TargetBulkPlantValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "bulkPlant-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Site_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkRecords", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#bulkPlant-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "BulkPlant-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalBulkPlantMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Site/BulkPlant Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Site/BulkPlant Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalBulkPlantMappingsComponent_tr_21_Template, 9, 3, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalBulkPlantMappingsComponent_div_22_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Site/BulkPlant Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalBulkPlantMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Site/BulkPlant CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalBulkPlantMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalBulkPlantMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalBulkPlantMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalBulkPlantMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editBulkPlantDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtYnVsay1wbGFudC1tYXBwaW5ncy9leHRlcm5hbC1idWxrLXBsYW50LW1hcHBpbmdzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalBulkPlantMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-bulk-plant-mappings',
          templateUrl: './external-bulk-plant-mappings.component.html',
          styleUrls: ['./external-bulk-plant-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalCarrierMappingsExternalCarrierMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalCarrierMappingsComponent", function () {
      return ExternalCarrierMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalCarrierMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Carrier_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Carrier_r4.TargetCarrierValue);
      }
    }

    function ExternalCarrierMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCarrierMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Carrier_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editCarrier(Carrier_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalCarrierMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalCarrierMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Carrier_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Carrier_r4.CarrierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Carrier_r4.TargetCarrierValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Carrier_r4.TargetCarrierValue == null);
      }
    }

    function ExternalCarrierMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCarrierMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Carrier Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCarrierMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editCarrierDetails.CarrierName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, " Carrier Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCarrierMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editCarrierDetails.TargetCarrierValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalCarrierMappings(ctx_r14.editCarrierDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCarrierDetails.CarrierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCarrierDetails.TargetCarrierValue);
      }
    }

    var ExternalCarrierMappingsComponent = /*#__PURE__*/function () {
      function ExternalCarrierMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalCarrierMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalCarrierMappings = [];
      }

      _createClass(ExternalCarrierMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getCarriersData();
        }
      }, {
        key: "getCarriersData",
        value: function getCarriersData() {
          var _this16 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getCarriersForExternalMapping().subscribe(function (data) {
            _this16.externalCarrierMappings = data;
            _this16.isShowLoader = false;

            _this16.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Carriers Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Carriers Details',
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
        key: "editCarrier",
        value: function editCarrier(carrier) {
          this.editCarrierDetails = JSON.parse(JSON.stringify(carrier));
        }
      }, {
        key: "SaveExternalCarrierMappings",
        value: function SaveExternalCarrierMappings(carrier) {
          var _this17 = this;

          carrier.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalCarrierMappings(carrier).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this17.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this17.isShowLoader = false;

            _this17.getCarriersData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editCarrierDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this18 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadCarrierMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this18.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this18.getCarriersData();
            }

            _this18.file = null;
          });
        }
      }]);

      return ExternalCarrierMappingsComponent;
    }();

    ExternalCarrierMappingsComponent.ɵfac = function ExternalCarrierMappingsComponent_Factory(t) {
      return new (t || ExternalCarrierMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalCarrierMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalCarrierMappingsComponent,
      selectors: [["app-external-carrier-mappings"]],
      viewQuery: function ExternalCarrierMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-carrier-grid", 1, "table-responsive"], ["id", "carrier-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CarrierName"], ["data-key", "TargetCarrierValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "carrier-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Carrier_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#carrier-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Carrier-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalCarrierMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Carrier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Carrier Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalCarrierMappingsComponent_tr_21_Template, 9, 3, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalCarrierMappingsComponent_div_22_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Carrier Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalCarrierMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Carrier CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCarrierMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalCarrierMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCarrierMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalCarrierMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editCarrierDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtY2Fycmllci1tYXBwaW5ncy9leHRlcm5hbC1jYXJyaWVyLW1hcHBpbmdzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalCarrierMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-carrier-mappings',
          templateUrl: './external-carrier-mappings.component.html',
          styleUrls: ['./external-carrier-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-customer-mappings/external-customer-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalCustomerMappingsExternalCustomerMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalCustomerMappingsComponent", function () {
      return ExternalCustomerMappingsComponent;
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


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalCustomerMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Customer_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Customer_r4.TargetCustomerValue);
      }
    }

    function ExternalCustomerMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCustomerMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Customer_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editCustomer(Customer_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalCustomerMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalCustomerMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Customer_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Customer_r4.CustomerName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Customer_r4.TargetCustomerValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Customer_r4.TargetCustomerValue == null);
      }
    }

    function ExternalCustomerMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCustomerMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Customer Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editCustomerDetails.CustomerName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, " Customer Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editCustomerDetails.TargetCustomerValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalCustomerMappings(ctx_r14.editCustomerDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerDetails.CustomerName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerDetails.TargetCustomerValue);
      }
    }

    var ExternalCustomerMappingsComponent = /*#__PURE__*/function () {
      function ExternalCustomerMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalCustomerMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalCustomerMappings = [];
      }

      _createClass(ExternalCustomerMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getAllCustomerData();
        }
      }, {
        key: "getAllCustomerData",
        value: function getAllCustomerData() {
          var _this19 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getCustomersForExternalMapping().subscribe(function (data) {
            _this19.externalCustomerMappings = data;
            _this19.isShowLoader = false;

            _this19.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Customer Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Customer Details',
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
        key: "editCustomer",
        value: function editCustomer(customer) {
          this.editCustomerDetails = JSON.parse(JSON.stringify(customer));
        }
      }, {
        key: "SaveExternalCustomerMappings",
        value: function SaveExternalCustomerMappings(customer) {
          var _this20 = this;

          customer.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalCustomerMappings(customer).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this20.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this20.isShowLoader = false;

            _this20.getAllCustomerData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editCustomerDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this21 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadCustomerMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this21.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this21.getAllCustomerData();
            }

            _this21.file = null;
          });
        }
      }]);

      return ExternalCustomerMappingsComponent;
    }();

    ExternalCustomerMappingsComponent.ɵfac = function ExternalCustomerMappingsComponent_Factory(t) {
      return new (t || ExternalCustomerMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalCustomerMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalCustomerMappingsComponent,
      selectors: [["app-external-customer-mappings"]],
      viewQuery: function ExternalCustomerMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-customer-grid", 1, "table-responsive"], ["id", "customer-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CustomerName"], ["data-key", "TargetCustomerValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "customer-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Customer_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#customer-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Customer-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalCustomerMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Customer Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Customer Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalCustomerMappingsComponent_tr_21_Template, 9, 3, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalCustomerMappingsComponent_div_22_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Customer Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalCustomerMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Customer CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalCustomerMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalCustomerMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editCustomerDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtY3VzdG9tZXItbWFwcGluZ3MvZXh0ZXJuYWwtY3VzdG9tZXItbWFwcGluZ3MuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalCustomerMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-customer-mappings',
          templateUrl: './external-customer-mappings.component.html',
          styleUrls: ['./external-customer-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalCustomerlocationMappingsExternalCustomerlocationMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalCustomerlocationMappingsComponent", function () {
      return ExternalCustomerlocationMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalCustomerlocationMappingsComponent_tr_25_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Location_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Location_r4.TargetCustomerLocationValue);
      }
    }

    function ExternalCustomerlocationMappingsComponent_tr_25_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCustomerlocationMappingsComponent_tr_25_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_tr_25_Template_a_click_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Location_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editCustomerLocation(Location_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, ExternalCustomerlocationMappingsComponent_tr_25_span_9_Template, 2, 1, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, ExternalCustomerlocationMappingsComponent_tr_25_span_10_Template, 2, 0, "span", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "i", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Location_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Location_r4.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Location_r4.CustomerLocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Location_r4.TargetCustomerValue);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Location_r4.TargetCustomerLocationValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Location_r4.TargetCustomerLocationValue == null);
      }
    }

    function ExternalCustomerlocationMappingsComponent_div_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalCustomerlocationMappingsComponent_ng_container_34_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Customer Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editCustomerLocationsDetails.CompanyName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Customer Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editCustomerLocationsDetails.CustomerLocationName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Customer Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "input", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_input_ngModelChange_20_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.editCustomerLocationsDetails.TargetCustomerValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "PDI Location Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_input_ngModelChange_26_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.editCustomerLocationsDetails.TargetCustomerLocationValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "input", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_input_click_29_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r15.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "button", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_ng_container_34_Template_button_click_30_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r16.SaveExternalCustomerLocationMappings(ctx_r16.editCustomerLocationsDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerLocationsDetails.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerLocationsDetails.CustomerLocationName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerLocationsDetails.TargetCustomerValue);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editCustomerLocationsDetails.TargetCustomerLocationValue);
      }
    }

    var ExternalCustomerlocationMappingsComponent = /*#__PURE__*/function () {
      function ExternalCustomerlocationMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalCustomerlocationMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalCustomerLocationMappings = [];
      }

      _createClass(ExternalCustomerlocationMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getCustomerLocationsData();
        }
      }, {
        key: "getCustomerLocationsData",
        value: function getCustomerLocationsData() {
          var _this22 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getCustomerLocationsForExternalMapping().subscribe(function (data) {
            _this22.externalCustomerLocationMappings = data;
            _this22.isShowLoader = false;

            _this22.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Location Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Location Details',
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
        key: "editCustomerLocation",
        value: function editCustomerLocation(customerLocation) {
          this.editCustomerLocationsDetails = JSON.parse(JSON.stringify(customerLocation));
        }
      }, {
        key: "SaveExternalCustomerLocationMappings",
        value: function SaveExternalCustomerLocationMappings(location) {
          var _this23 = this;

          location.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalCustomerLocationMappings(location).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this23.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this23.isShowLoader = false;

            _this23.getCustomerLocationsData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editCustomerLocationsDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this24 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadCustomerLocationMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this24.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this24.getCustomerLocationsData();
            }

            _this24.file = null;
          });
        }
      }]);

      return ExternalCustomerlocationMappingsComponent;
    }();

    ExternalCustomerlocationMappingsComponent.ɵfac = function ExternalCustomerlocationMappingsComponent_Factory(t) {
      return new (t || ExternalCustomerlocationMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalCustomerlocationMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalCustomerlocationMappingsComponent,
      selectors: [["app-external-customerlocation-mappings"]],
      viewQuery: function ExternalCustomerlocationMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 59,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb10"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-customer-grid", 1, "table-responsive"], ["id", "location-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "CompanyName"], ["data-key", "CustomerLocationName"], ["data-key", "TargetCustomerValue"], ["data-key", "TargetCustomerLocationValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "location-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_CustomerLocation_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#location-panel','40%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Location-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalCustomerlocationMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Customer Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Customer Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, " Customer Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, " Location Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, ExternalCustomerlocationMappingsComponent_tr_25_Template, 13, 5, "tr", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, ExternalCustomerlocationMappingsComponent_div_26_Template, 5, 0, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "a", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "i", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_Template_i_click_31_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "h3", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](33, "Customer Location Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](34, ExternalCustomerlocationMappingsComponent_ng_container_34_Template, 32, 4, "ng-container", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "h4", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Customer Location CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "button", 32, 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](45, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](48, "span", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "a", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](50, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "label", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalCustomerlocationMappingsComponent_Template_input_ngModelChange_55_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalCustomerlocationMappingsComponent_Template_input_change_55_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](57, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "input", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalCustomerlocationMappingsComponent_Template_input_click_58_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalCustomerLocationMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editCustomerLocationsDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtY3VzdG9tZXJsb2NhdGlvbi1tYXBwaW5ncy9leHRlcm5hbC1jdXN0b21lcmxvY2F0aW9uLW1hcHBpbmdzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalCustomerlocationMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-customerlocation-mappings',
          templateUrl: './external-customerlocation-mappings.component.html',
          styleUrls: ['./external-customerlocation-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-driver-mappings/external-driver-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalDriverMappingsExternalDriverMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalDriverMappingsComponent", function () {
      return ExternalDriverMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalDriverMappingsComponent_tr_23_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Driver_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Driver_r4.TargetDriverValue);
      }
    }

    function ExternalDriverMappingsComponent_tr_23_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalDriverMappingsComponent_tr_23_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_tr_23_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Driver_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editDriver(Driver_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, ExternalDriverMappingsComponent_tr_23_span_7_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, ExternalDriverMappingsComponent_tr_23_span_8_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Driver_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Driver_r4.FirstName + " " + Driver_r4.LastName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Driver_r4.Email);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Driver_r4.TargetDriverValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Driver_r4.TargetDriverValue == null);
      }
    }

    function ExternalDriverMappingsComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalDriverMappingsComponent_ng_container_32_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Driver Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, " Driver Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalDriverMappingsComponent_ng_container_32_Template_input_ngModelChange_15_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editDriverDetails.TargetDriverValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_ng_container_32_Template_input_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_ng_container_32_Template_button_click_19_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.SaveExternalDriverMappings(ctx_r13.editDriverDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", ctx_r2.editDriverDetails.FirstName + " " + ctx_r2.editDriverDetails.LastName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editDriverDetails.TargetDriverValue);
      }
    }

    var ExternalDriverMappingsComponent = /*#__PURE__*/function () {
      function ExternalDriverMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalDriverMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalDriverMappings = [];
      }

      _createClass(ExternalDriverMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeDriverCustomers();
          this.getDriversData();
        }
      }, {
        key: "getDriversData",
        value: function getDriversData() {
          var _this25 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getDriversForExternalMapping().subscribe(function (data) {
            _this25.externalDriverMappings = data;
            _this25.isShowLoader = false;

            _this25.refreshDatatable();
          });
        }
      }, {
        key: "initializeDriverCustomers",
        value: function initializeDriverCustomers() {
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
              title: 'Drivers Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Drivers Details',
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
        key: "editDriver",
        value: function editDriver(driver) {
          this.editDriverDetails = JSON.parse(JSON.stringify(driver));
        }
      }, {
        key: "SaveExternalDriverMappings",
        value: function SaveExternalDriverMappings(driver) {
          var _this26 = this;

          driver.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalDriverMappings(driver).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this26.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this26.isShowLoader = false;

            _this26.getDriversData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editDriverDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this27 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadDriverMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this27.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this27.getDriversData();
            }

            _this27.file = null;
          });
        }
      }]);

      return ExternalDriverMappingsComponent;
    }();

    ExternalDriverMappingsComponent.ɵfac = function ExternalDriverMappingsComponent_Factory(t) {
      return new (t || ExternalDriverMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalDriverMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalDriverMappingsComponent,
      selectors: [["app-external-driver-mappings"]],
      viewQuery: function ExternalDriverMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 57,
      vars: 6,
      consts: [[1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-driver-grid", 1, "table-responsive"], ["id", "driver-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "DriverName"], ["data-key", "Email"], ["data-key", "TargetDriverValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "driver-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Driver_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkDriver", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#driver-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Driver-Form", 1, "col-sm-12"], [1, "form-group"], [1, "form-control"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalDriverMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Driver Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Driver Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, " Driver Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, ExternalDriverMappingsComponent_tr_23_Template, 11, 4, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, ExternalDriverMappingsComponent_div_24_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_Template_i_click_29_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Driver Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, ExternalDriverMappingsComponent_ng_container_32_Template, 21, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](39, "Driver CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](43, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](46, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](48, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalDriverMappingsComponent_Template_input_ngModelChange_53_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalDriverMappingsComponent_Template_input_change_53_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalDriverMappingsComponent_Template_input_click_56_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalDriverMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editDriverDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtZHJpdmVyLW1hcHBpbmdzL2V4dGVybmFsLWRyaXZlci1tYXBwaW5ncy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalDriverMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-driver-mappings',
          templateUrl: './external-driver-mappings.component.html',
          styleUrls: ['./external-driver-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-product-mappings/external-product-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalProductMappingsExternalProductMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalProductMappingsComponent", function () {
      return ExternalProductMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalProductMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Product_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Product_r4.TargetProductValue);
      }
    }

    function ExternalProductMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalProductMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Product_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editProduct(Product_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalProductMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalProductMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Product_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Product_r4.ProductName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Product_r4.TargetProductValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Product_r4.TargetProductValue == null);
      }
    }

    function ExternalProductMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalProductMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Product Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalProductMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editProductDetails.ProductName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, " Product Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalProductMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editProductDetails.TargetProductValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalProductMappings(ctx_r14.editProductDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editProductDetails.ProductName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editProductDetails.TargetProductValue);
      }
    }

    var ExternalProductMappingsComponent = /*#__PURE__*/function () {
      function ExternalProductMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalProductMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalProductMappings = [];
      }

      _createClass(ExternalProductMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getProductsData();
        }
      }, {
        key: "getProductsData",
        value: function getProductsData() {
          var _this28 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getProductsForExternalMapping().subscribe(function (data) {
            _this28.externalProductMappings = data;
            _this28.isShowLoader = false;

            _this28.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Products Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Products Details',
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
        key: "editProduct",
        value: function editProduct(product) {
          this.editProductDetails = JSON.parse(JSON.stringify(product));
        }
      }, {
        key: "SaveExternalProductMappings",
        value: function SaveExternalProductMappings(product) {
          var _this29 = this;

          product.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalProductMappings(product).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this29.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this29.isShowLoader = false;

            _this29.getProductsData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editProductDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this30 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadProductMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this30.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this30.getProductsData();
            }

            _this30.file = null;
          });
        }
      }]);

      return ExternalProductMappingsComponent;
    }();

    ExternalProductMappingsComponent.ɵfac = function ExternalProductMappingsComponent_Factory(t) {
      return new (t || ExternalProductMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalProductMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalProductMappingsComponent,
      selectors: [["app-external-product-mappings"]],
      viewQuery: function ExternalProductMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb10"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-customer-grid", 1, "table-responsive"], ["id", "product-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "ProductName"], ["data-key", "TargetProductValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "product-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Product_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#product-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Product-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalProductMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Product Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Product Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalProductMappingsComponent_tr_21_Template, 9, 3, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalProductMappingsComponent_div_22_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Product Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalProductMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Product CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalProductMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalProductMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalProductMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalProductMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editProductDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtcHJvZHVjdC1tYXBwaW5ncy9leHRlcm5hbC1wcm9kdWN0LW1hcHBpbmdzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalProductMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-product-mappings',
          templateUrl: './external-product-mappings.component.html',
          styleUrls: ['./external-product-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalSupplierMappingsExternalSupplierMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalSupplierMappingsComponent", function () {
      return ExternalSupplierMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalSupplierMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Supplier_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Supplier_r4.TargetSupplierValue);
      }
    }

    function ExternalSupplierMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalSupplierMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Supplier_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editSupplier(Supplier_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalSupplierMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalSupplierMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Supplier_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Supplier_r4.SupplierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Supplier_r4.TargetSupplierValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Supplier_r4.TargetSupplierValue == null);
      }
    }

    function ExternalSupplierMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalSupplierMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Vendor");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalSupplierMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editSupplierDetails.SupplierName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Vendor Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalSupplierMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editSupplierDetails.TargetSupplierValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalSupplierMappings(ctx_r14.editSupplierDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editSupplierDetails.SupplierName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editSupplierDetails.TargetSupplierValue);
      }
    }

    var ExternalSupplierMappingsComponent = /*#__PURE__*/function () {
      function ExternalSupplierMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalSupplierMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalSupplierMappings = [];
      }

      _createClass(ExternalSupplierMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getSuppliersData();
        }
      }, {
        key: "getSuppliersData",
        value: function getSuppliersData() {
          var _this31 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getSuppliersForExternalMapping().subscribe(function (data) {
            _this31.externalSupplierMappings = data;
            _this31.isShowLoader = false;

            _this31.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Suppliers Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Suppliers Details',
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
        key: "editSupplier",
        value: function editSupplier(supplier) {
          this.editSupplierDetails = JSON.parse(JSON.stringify(supplier));
        }
      }, {
        key: "SaveExternalSupplierMappings",
        value: function SaveExternalSupplierMappings(supplier) {
          var _this32 = this;

          supplier.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalSupplierMappings(supplier).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this32.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this32.isShowLoader = false;

            _this32.getSuppliersData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editSupplierDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this33 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadSupplierMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this33.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this33.getSuppliersData();
            }

            _this33.file = null;
          });
        }
      }]);

      return ExternalSupplierMappingsComponent;
    }();

    ExternalSupplierMappingsComponent.ɵfac = function ExternalSupplierMappingsComponent_Factory(t) {
      return new (t || ExternalSupplierMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalSupplierMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalSupplierMappingsComponent,
      selectors: [["app-external-supplier-mappings"]],
      viewQuery: function ExternalSupplierMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-supplier-grid", 1, "table-responsive"], ["id", "supplier-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "SupplierName"], ["data-key", "TargetSupplierValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "supplier-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Vendor_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#supplier-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Supplier-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalSupplierMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Vendor");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Vendor Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalSupplierMappingsComponent_tr_21_Template, 9, 3, "tr", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalSupplierMappingsComponent_div_22_Template, 5, 0, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Vendor Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalSupplierMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Vendor CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 30, 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalSupplierMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalSupplierMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalSupplierMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalSupplierMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editSupplierDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtc3VwcGxpZXItbWFwcGluZ3MvZXh0ZXJuYWwtc3VwcGxpZXItbWFwcGluZ3MuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalSupplierMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-supplier-mappings',
          templateUrl: './external-supplier-mappings.component.html',
          styleUrls: ['./external-supplier-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalTerminalMappingsExternalTerminalMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalTerminalMappingsComponent", function () {
      return ExternalTerminalMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalTerminalMappingsComponent_tr_23_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Terminal_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Terminal_r4.TargetTerminalValue);
      }
    }

    function ExternalTerminalMappingsComponent_tr_23_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalTerminalMappingsComponent_tr_23_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_tr_23_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Terminal_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editTerminal(Terminal_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, ExternalTerminalMappingsComponent_tr_23_span_7_Template, 2, 1, "span", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, ExternalTerminalMappingsComponent_tr_23_span_8_Template, 2, 0, "span", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "i", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Terminal_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Terminal_r4.TerminalName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Terminal_r4.ControlNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Terminal_r4.TargetTerminalValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Terminal_r4.TargetTerminalValue == null);
      }
    }

    function ExternalTerminalMappingsComponent_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalTerminalMappingsComponent_ng_container_32_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Terminal Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalTerminalMappingsComponent_ng_container_32_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editTerminalDetails.TerminalName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, " Terminal Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalTerminalMappingsComponent_ng_container_32_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editTerminalDetails.TargetTerminalValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_ng_container_32_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_ng_container_32_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalTerminalMappings(ctx_r14.editTerminalDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editTerminalDetails.TerminalName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editTerminalDetails.TargetTerminalValue);
      }
    }

    var ExternalTerminalMappingsComponent = /*#__PURE__*/function () {
      function ExternalTerminalMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalTerminalMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalTerminalMappings = [];
      }

      _createClass(ExternalTerminalMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getTerminalData();
        }
      }, {
        key: "getTerminalData",
        value: function getTerminalData() {
          var _this34 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getTerminalsForExternalMapping().subscribe(function (data) {
            _this34.externalTerminalMappings = data;
            _this34.isShowLoader = false;

            _this34.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Terminals Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Terminals Details',
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
        key: "editTerminal",
        value: function editTerminal(terminal) {
          this.editTerminalDetails = JSON.parse(JSON.stringify(terminal));
        }
      }, {
        key: "SaveExternalTerminalMappings",
        value: function SaveExternalTerminalMappings(terminal) {
          var _this35 = this;

          terminal.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalTerminalMappings(terminal).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this35.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this35.isShowLoader = false;

            _this35.getTerminalData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editTerminalDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this36 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadTerminalMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this36.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this36.getTerminalData();
            }

            _this36.file = null;
          });
        }
      }]);

      return ExternalTerminalMappingsComponent;
    }();

    ExternalTerminalMappingsComponent.ɵfac = function ExternalTerminalMappingsComponent_Factory(t) {
      return new (t || ExternalTerminalMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalTerminalMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalTerminalMappingsComponent,
      selectors: [["app-external-terminal-mappings"]],
      viewQuery: function ExternalTerminalMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 57,
      vars: 6,
      consts: [[1, "well"], [1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-terminal-grid", 1, "table-responsive"], ["id", "terminal-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "TerminalName"], ["data-key", "ControlNumber"], ["data-key", "TargetTerminalValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "terminal-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Terminal_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkCarrier", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#terminal-panel','40%','60%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Terminal-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalTerminalMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Terminal Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Terminal Control Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, " Terminal Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, ExternalTerminalMappingsComponent_tr_23_Template, 11, 4, "tr", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, ExternalTerminalMappingsComponent_div_24_Template, 5, 0, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "a", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "i", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_Template_i_click_29_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "h3", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Terminal Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, ExternalTerminalMappingsComponent_ng_container_32_Template, 20, 2, "ng-container", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "h4", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](39, "Terminal CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "button", 31, 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "span", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](43, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](46, "span", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "a", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](48, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "label", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "input", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalTerminalMappingsComponent_Template_input_ngModelChange_53_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalTerminalMappingsComponent_Template_input_change_53_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "input", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalTerminalMappingsComponent_Template_input_click_56_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalTerminalMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editTerminalDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtdGVybWluYWwtbWFwcGluZ3MvZXh0ZXJuYWwtdGVybWluYWwtbWFwcGluZ3MuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalTerminalMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-terminal-mappings',
          templateUrl: './external-terminal-mappings.component.html',
          styleUrls: ['./external-terminal-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component.ts": function srcAppSelfServiceAliasExternalEntityMappingsExternalVehicleMappingsExternalVehicleMappingsComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalVehicleMappingsComponent", function () {
      return ExternalVehicleMappingsComponent;
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


    var _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var _c0 = ["btnCloseBulkModal"];

    function ExternalVehicleMappingsComponent_tr_21_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Vehicle_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Vehicle_r4.TargetVehicleValue);
      }
    }

    function ExternalVehicleMappingsComponent_tr_21_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalVehicleMappingsComponent_tr_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_tr_21_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var Vehicle_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.editVehicle(Vehicle_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ExternalVehicleMappingsComponent_tr_21_span_5_Template, 2, 1, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ExternalVehicleMappingsComponent_tr_21_span_6_Template, 2, 0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var Vehicle_r4 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](Vehicle_r4.TruckName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Vehicle_r4.TargetVehicleValue != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", Vehicle_r4.TargetVehicleValue == null);
      }
    }

    function ExternalVehicleMappingsComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ExternalVehicleMappingsComponent_ng_container_30_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Vehicle Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalVehicleMappingsComponent_ng_container_30_Template_input_ngModelChange_8_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.editVehicleDetails.TruckName = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, " Vehicle Id");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalVehicleMappingsComponent_ng_container_30_Template_input_ngModelChange_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.editVehicleDetails.TargetVehicleValue = $event;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_ng_container_30_Template_input_click_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.clearForm();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_ng_container_30_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.SaveExternalVehicleMappings(ctx_r14.editVehicleDetails);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Submit");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editVehicleDetails.TruckName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx_r2.editVehicleDetails.TargetVehicleValue);
      }
    }

    var ExternalVehicleMappingsComponent = /*#__PURE__*/function () {
      function ExternalVehicleMappingsComponent(externalMappingsService) {
        _classCallCheck(this, ExternalVehicleMappingsComponent);

        this.externalMappingsService = externalMappingsService;
        this.isShowLoader = false;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.externalVehicleMappings = [];
      }

      _createClass(ExternalVehicleMappingsComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeCarrierCustomers();
          this.getVehicleData();
        }
      }, {
        key: "getVehicleData",
        value: function getVehicleData() {
          var _this37 = this;

          this.isShowLoader = true;
          this.externalMappingsService.getVehiclesForExternalMapping().subscribe(function (data) {
            _this37.externalVehicleMappings = data;
            _this37.isShowLoader = false;

            _this37.refreshDatatable();
          });
        }
      }, {
        key: "initializeCarrierCustomers",
        value: function initializeCarrierCustomers() {
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
              title: 'Vehicles Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Vehicles Details',
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
        key: "editVehicle",
        value: function editVehicle(vehicle) {
          this.editVehicleDetails = JSON.parse(JSON.stringify(vehicle));
        }
      }, {
        key: "SaveExternalVehicleMappings",
        value: function SaveExternalVehicleMappings(vehicle) {
          var _this38 = this;

          vehicle.ThirdPartyId = this.thirdPartyCompanyId;
          this.isShowLoader = true;
          this.externalMappingsService.SaveExternalVehicleMappings(vehicle).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              closeSlidePanel();

              _this38.clearForm();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }

            _this38.isShowLoader = false;

            _this38.getVehicleData();
          });
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.editVehicleDetails = null;
        }
      }, {
        key: "onFileChange",
        value: function onFileChange(event) {
          this.file = event.target.files[0];
        }
      }, {
        key: "bulkUpload",
        value: function bulkUpload() {
          this.selectedFile = null;
          this.file = null;
        }
      }, {
        key: "onFileUpload",
        value: function onFileUpload() {
          var _this39 = this;

          if (!this.file) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror("Please select file", undefined, undefined);
            return;
          }

          var element = this.btnCloseBulkModal.nativeElement;
          element.click();
          this.isShowLoader = true;
          this.externalMappingsService.BulkUploadVehicleMapping(this.file).subscribe(function (response) {
            if (response.StatusCode == 1) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              _this39.isShowLoader = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(response.StatusMessage, undefined, undefined);

              _this39.getVehicleData();
            }

            _this39.file = null;
          });
        }
      }]);

      return ExternalVehicleMappingsComponent;
    }();

    ExternalVehicleMappingsComponent.ɵfac = function ExternalVehicleMappingsComponent_Factory(t) {
      return new (t || ExternalVehicleMappingsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]));
    };

    ExternalVehicleMappingsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ExternalVehicleMappingsComponent,
      selectors: [["app-external-vehicle-mappings"]],
      viewQuery: function ExternalVehicleMappingsComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.btnCloseBulkModal = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      inputs: {
        thirdPartyCompanyId: "thirdPartyCompanyId"
      },
      decls: 55,
      vars: 6,
      consts: [[1, "row", "mb20"], [1, "col-sm-12", "text-right"], ["id", "BulkUpload", "data-toggle", "modal", "data-target", "#bulk-upload-csv", 1, "fs18", "float-right", "ml20", 3, "click"], [1, "fa", "fa-upload", "fs18", "mt-1", "mr-2", "pull-left"], [1, "fs14", "mt2", "pull-left"], [1, "row"], [1, "col-md-12"], [1, "well", "bg-white", "no-shadow"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-vehicle-grid", 1, "table-responsive"], ["id", "vehicle-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "TruckName"], ["data-key", "TargetVehicleValue"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "vehicle-panel", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt15"], ["onclick", "closeSlidePanel();", 1, "ml15", "close-panel"], [1, "fa", "fa-close", "fs18", 3, "click"], [1, "dib", "mt0", "mb0", "ml15"], [4, "ngIf"], ["id", "bulk-upload-csv", "tabindex", "-1", "role", "dialog", "aria-labelledby", "myModalLabel", 1, "modal", "fade"], ["role", "document", 1, "modal-dialog"], [1, "modal-content"], [1, "modal-body"], [1, "overflow-h"], [1, "pull-left", "mb5", "pt0", "pb0"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", "pull-right", "pa"], ["btnCloseBulkModal", ""], ["aria-hidden", "true", 2, "font-size", "35px"], [1, "col-sm-12"], [1, "fa", "fa-download", "mr10", "mt10"], ["href", "/Content/Template_Vehicle_Mapping.csv", 1, "mb5", "btn-download"], [1, "row", "mt10"], [1, "col-md-12", "b-dashed"], ["for", "csvFile", 1, "col-sm-12", "btn", "btn-primary", "ml0"], ["id", "csvFile", "name", "csvFile", "type", "file", "accept", ".csv", 1, "bulkElements", "full-width", 3, "ngModel", "ngModelChange", "change"], [1, "col-sm-12", "text-right", "pb0", "fs12"], ["type", "submit", "value", "Upload", "id", "uploadBulkVehicle", 1, "btn", "btn-primary", "bulkElements", 3, "click"], ["onclick", "slidePanel('#vehicle-panel','40%')", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fas", "fa-edit", "pull-left", "mt7"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], [1, "side-panel-body"], ["id", "Vehicle-Form", 1, "col-sm-12"], [1, "form-group"], ["type", "text", "readonly", "", 1, "form-control", 3, "ngModel", "ngModelChange"], ["type", "text", 1, "form-control", 3, "ngModel", "ngModelChange"], [1, "col-sm-12", "text-right", "form-buttons"], ["type", "button", "value", "Cancel", "onclick", "closeSlidePanel()", 1, "btn", 3, "click"], ["type", "submit", "aria-invalid", "false", 1, "btn", "btn-lg", "btn-primary", "valid", 3, "click"]],
      template: function ExternalVehicleMappingsComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_Template_a_click_3_listener() {
            return ctx.bulkUpload();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Vehicle Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Vehicle Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, ExternalVehicleMappingsComponent_tr_21_Template, 9, 3, "tr", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, ExternalVehicleMappingsComponent_div_22_Template, 5, 0, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "a", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "i", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_Template_i_click_27_listener() {
            return ctx.clearForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "h3", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Vehicle Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, ExternalVehicleMappingsComponent_ng_container_30_Template, 20, 2, "ng-container", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "h4", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Vehicle CSV");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "button", 29, 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "span", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "span", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "a", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Download Template");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "h2");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "input", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function ExternalVehicleMappingsComponent_Template_input_ngModelChange_51_listener($event) {
            return ctx.selectedFile = $event;
          })("change", function ExternalVehicleMappingsComponent_Template_input_change_51_listener($event) {
            return ctx.onFileChange($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ExternalVehicleMappingsComponent_Template_input_click_54_listener() {
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
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalVehicleMappings);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isShowLoader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.editVehicleDetails);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.selectedFile);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["NgModel"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9leHRlcm5hbC1lbnRpdHktbWFwcGluZ3MvZXh0ZXJuYWwtdmVoaWNsZS1tYXBwaW5ncy9leHRlcm5hbC12ZWhpY2xlLW1hcHBpbmdzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalVehicleMappingsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-external-vehicle-mappings',
          templateUrl: './external-vehicle-mappings.component.html',
          styleUrls: ['./external-vehicle-mappings.component.css']
        }]
      }], function () {
        return [{
          type: _service_externalmappings_service__WEBPACK_IMPORTED_MODULE_4__["ExternalMappingsService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_2__["DataTableDirective"]]
        }],
        thirdPartyCompanyId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        btnCloseBulkModal: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkModal']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/models/ProductMappingModel.ts": function srcAppSelfServiceAliasModelsProductMappingModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ProductMappingModel", function () {
      return ProductMappingModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ProductMappingGridModel", function () {
      return ProductMappingGridModel;
    });
    /* harmony import */


    var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! src/app/statelist.service */
    "./src/app/statelist.service.ts");

    var ProductMappingModel = function ProductMappingModel() {
      _classCallCheck(this, ProductMappingModel);

      this.StateList = [];
      this.CityList = [];
      this.TerminalList = [];
      this.FuelType = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_0__["DropdownItem"]();
    };

    var ProductMappingGridModel = function ProductMappingGridModel() {
      _classCallCheck(this, ProductMappingGridModel);
    };
    /***/

  },

  /***/
  "./src/app/self-service-alias/product-mapping/product-mapping.component.ts": function srcAppSelfServiceAliasProductMappingProductMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ProductMappingComponent", function () {
      return ProductMappingComponent;
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


    var _models_ProductMappingModel__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../models/ProductMappingModel */
    "./src/app/self-service-alias/models/ProductMappingModel.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    var _c0 = ["MyProductId"];
    var _c1 = ["btnCloseBulkUploadPopup"];

    function ProductMappingComponent_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_div_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Fuel Type is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_tr_83_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var mapping_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](mapping_r6.FuelType);
      }
    }

    function ProductMappingComponent_tr_83_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_tr_83_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var mapping_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](mapping_r6.TerminalName);
      }
    }

    function ProductMappingComponent_tr_83_span_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_tr_83_span_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var mapping_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](mapping_r6.TerminalAddress);
      }
    }

    function ProductMappingComponent_tr_83_span_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_tr_83_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ProductMappingComponent_tr_83_span_2_Template, 2, 1, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, ProductMappingComponent_tr_83_span_3_Template, 2, 0, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, ProductMappingComponent_tr_83_span_5_Template, 2, 1, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, ProductMappingComponent_tr_83_span_6_Template, 2, 0, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, ProductMappingComponent_tr_83_span_8_Template, 2, 1, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, ProductMappingComponent_tr_83_span_9_Template, 2, 0, "span", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "td", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function ProductMappingComponent_tr_83_Template_td_blur_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var mapping_r6 = ctx.$implicit;

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r16.editProductNames(mapping_r6, "BackOfficeProductId", $event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "td", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function ProductMappingComponent_tr_83_Template_td_blur_12_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var mapping_r6 = ctx.$implicit;

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r18.editProductNames(mapping_r6, "DriverProductId", $event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "td", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function ProductMappingComponent_tr_83_Template_td_blur_14_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var mapping_r6 = ctx.$implicit;

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r19.editProductNames(mapping_r6, "MyProductId", $event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "td", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "button", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function ProductMappingComponent_tr_83_Template_button_confirm_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var mapping_r6 = ctx.$implicit;

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r20.updateProductNames(mapping_r6.Id);
        })("cancel", function ProductMappingComponent_tr_83_Template_button_cancel_17_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r21.cancelUpdateProductNames();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](18, "i", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "button", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function ProductMappingComponent_tr_83_Template_button_confirm_19_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var mapping_r6 = ctx.$implicit;

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r22.deleteMapping(mapping_r6.Id, mapping_r6.CompanyId);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](20, "i", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var mapping_r6 = ctx.$implicit;

        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.FuelType != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.FuelType == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.TerminalName != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.TerminalName == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.TerminalAddress != null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", mapping_r6.TerminalAddress == null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", mapping_r6.BackOfficeProductId == null ? "--" : mapping_r6.BackOfficeProductId, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", mapping_r6.DriverProductId == null ? "--" : mapping_r6.DriverProductId, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", mapping_r6.MyProductId == null ? "--" : mapping_r6.MyProductId, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r3.popoverTitle)("confirmText", ctx_r3.confirmButtonText)("cancelText", ctx_r3.cancelButtonText);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r3.popoverTitle)("confirmText", ctx_r3.confirmButtonText)("cancelText", ctx_r3.cancelButtonText);
      }
    }

    function ProductMappingComponent_div_84_div_18_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ProductMappingComponent_div_84_Template(rf, ctx) {
      if (rf & 1) {
        var _r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h4", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Product Mapping");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "button", 52, 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_div_84_Template_button_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r25.closePopup();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "span", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_div_84_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r27.downloadCsvTemplate();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Download Template");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "input", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ProductMappingComponent_div_84_Template_input_change_15_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r28.selectFiles($event.target.files);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "span", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, " Note: A new version of template is available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](18, ProductMappingComponent_div_84_div_18_Template, 2, 0, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "button", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_div_84_Template_button_click_20_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r26);

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r29.uploadProductMappingFile();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Upload");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.IsLoading);
      }
    }

    function ProductMappingComponent_div_85_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var _c2 = function _c2(a0) {
      return {
        "pntr-none subSectionOpacity": a0
      };
    };

    var ProductMappingComponent = /*#__PURE__*/function () {
      function ProductMappingComponent(fb, carrierService) {
        _classCallCheck(this, ProductMappingComponent);

        this.fb = fb;
        this.carrierService = carrierService;
        this.IsLoading = false;
        this.StateList = [];
        this.CityList = [];
        this.TerminalList = [];
        this.FuelTypeList = [];
        this.ProductMappingList = [];
        this.UpdateProductMappingList = [];
        this.IsShowBulkUploadPopup = false;
        this.ddlSettingsByCode = {};
        this.ddlSettingsById = {};
        this.ddlSettingsByIdSingleSelect = {};
        this.MaxFileUploadSize = 1048576; // 1MB

        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subject"]();
        this.popoverTitle = 'Are you sure?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.SelectedFiles = [];
        this.IsValidForm = true;
        this.ProductMappingForm = this.fb.group({
          States: this.fb.control([]),
          Cities: this.fb.control([]),
          Terminals: this.fb.control([]),
          FuelTypes: this.fb.control([], [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          MyProductId: this.fb.control(''),
          BackOfficeProductId: this.fb.control(''),
          DriverProductId: this.fb.control(''),
          //  TerminalItemCode: this.fb.control(''),
          CompanyId: this.fb.control(0)
        });
      }

      _createClass(ProductMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.ddlSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.ddlSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.ddlSettingsByIdSingleSelect = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.initializeProductMappingDatatableGrid();
          this.CurrentCompanyId = Number(currentCompanyId);
          this.getServingFuelTypesByCompany();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges() {
          this.clearForm();
          this.SelectedCountryId = Number(this.countryId);

          if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
            this.getDefaultServingCountry();
          } else {
            this.getStates();
            this.getProductMappingGridDetails();
          }
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.getProductMappingGridDetails();
        }
      }, {
        key: "getProductMappingGridDetails",
        value: function getProductMappingGridDetails() {
          var _this40 = this;

          this.IsLoading = true;
          var selectedStates = this.ProductMappingForm.get('States').value;
          var selectedCities = this.ProductMappingForm.get('Cities').value;
          var selectedTerminals = this.ProductMappingForm.get('Terminals').value;
          var selectedFuelTypes = this.ProductMappingForm.get('FuelTypes').value;
          var input = new _models_ProductMappingModel__WEBPACK_IMPORTED_MODULE_2__["ProductMappingModel"]();

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (s) {
              return s.Id;
            });
            input.StateIds = stateIds.join(',');
          }

          if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(function (s) {
              return s.Name;
            });
            input.CityIds = cityIds.join(',');
          }

          if (selectedTerminals != null && selectedTerminals != undefined && selectedTerminals.length > 0) {
            var terminalIds = selectedTerminals.map(function (s) {
              return s.Id;
            });
            input.TerminalIds = terminalIds.join(',');
          }

          if (selectedFuelTypes != null && selectedFuelTypes != undefined && selectedFuelTypes.length > 0) {
            var fuelTypeIds = selectedFuelTypes.map(function (s) {
              return s.Id;
            });
            input.FuelTypeIds = fuelTypeIds.join(',');
          }

          input.CompanyId = this.CurrentCompanyId;
          input.CountryId = this.SelectedCountryId;

          if (input.CompanyId != undefined && input.CompanyId > 0) {
            this.carrierService.getProductMappingGridDetails(input).subscribe(function (data) {
              _this40.IsLoading = false;
              _this40.ProductMappingList = data;

              _this40.dtTrigger.next();
            });
          }
        }
      }, {
        key: "getDefaultServingCountry",
        value: function getDefaultServingCountry() {
          var _this41 = this;

          this.IsLoading = true;
          this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(function (data) {
            _this41.IsLoading = false;
            _this41.SelectedCountryId = Number(data.DefaultCountryId);

            _this41.getStates();

            _this41.getProductMappingGridDetails();
          });
        }
      }, {
        key: "getStates",
        value: function getStates() {
          var _this42 = this;

          this.IsLoading = true;

          if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId).subscribe(function (data) {
              _this42.IsLoading = false;
              _this42.StateList = data;
            });
          }
        }
      }, {
        key: "getCities",
        value: function getCities(stateIds) {
          var _this43 = this;

          this.IsLoading = true;
          this.carrierService.getCities(stateIds).subscribe(function (data) {
            _this43.IsLoading = false;
            _this43.CityList = data;
          });
        }
      }, {
        key: "getTerminals",
        value: function getTerminals() {
          var _this44 = this;

          var selectedStates = this.ProductMappingForm.get('States').value;
          var selectedCities = this.ProductMappingForm.get('Cities').value;

          if (selectedStates.length == 0) {
            this.ProductMappingForm.get('States').patchValue([]);
            this.TerminalList = [];
            return;
          }

          this.IsLoading = true;
          var input = new _models_ProductMappingModel__WEBPACK_IMPORTED_MODULE_2__["ProductMappingModel"]();

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (s) {
              return s.Id;
            });
            input.StateIds = stateIds.join(',');
          }

          if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(function (s) {
              return s.Name;
            });
            input.CityIds = cityIds.join(',');
          }

          input.CompanyId = this.CurrentCompanyId;
          this.carrierService.getTerminals(input).subscribe(function (data) {
            _this44.IsLoading = false;
            _this44.TerminalList = data;
          });
        }
      }, {
        key: "getServingFuelTypesByCompany",
        value: function getServingFuelTypesByCompany() {
          var _this45 = this;

          this.IsLoading = true;
          this.carrierService.getServingFuelTypesByCompany(this.CurrentCompanyId).subscribe(function (data) {
            _this45.IsLoading = false;
            _this45.FuelTypeList = data;
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var myProductId = this.ProductMappingForm.get('MyProductId').value;
          var backOfficeProductId = this.ProductMappingForm.get('BackOfficeProductId').value;
          var driverProductId = this.ProductMappingForm.get('DriverProductId').value; //  var terminalItemCode = this.ProductMappingForm.get('TerminalItemCode').value;

          if ((myProductId == undefined || myProductId == null || myProductId == '') && (backOfficeProductId == undefined || backOfficeProductId == null || backOfficeProductId == '') && (driverProductId == undefined || driverProductId == null || driverProductId == ''))
            /*&& (terminalItemCode == undefined || terminalItemCode == null || terminalItemCode ==''))*/
            {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Please provide My Product ID or Back Office Product ID or Driver Product ID', undefined, undefined);
              return;
            }

          this.ProductMappingForm.get("CompanyId").patchValue(this.CurrentCompanyId);
          this.IsValidForm = true;

          if (!this.ProductMappingForm.valid) {
            this.IsValidForm = false;
          } else {
            this.submitForm();
          }
        }
      }, {
        key: "submitForm",
        value: function submitForm() {
          var _this46 = this;

          this.IsLoading = true;
          this.carrierService.saveProductMapping(this.ProductMappingForm.value).subscribe(function (data) {
            if (data.StatusCode == 0) {
              _this46.IsLoading = false;

              _this46.clearForm();

              _this46.MyProductId.nativeElement.click();

              _this46.getProductMappingGridDetails();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "updateProductNames",
        value: function updateProductNames(mappingId) {
          var _this47 = this;

          var rowToUpdate = this.UpdateProductMappingList.filter(function (map) {
            return map.Id === mappingId;
          });

          if (rowToUpdate.length == 0) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('No updated records found', undefined, undefined);
            return;
          }

          this.IsLoading = true;
          this.carrierService.updateProductNames(rowToUpdate).subscribe(function (data) {
            _this47.IsLoading = false;

            if (data.StatusCode == 0) {
              _this47.UpdateProductMappingList = [];

              _this47.filterGrid();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "cancelUpdateProductNames",
        value: function cancelUpdateProductNames() {//this.filterGrid();
        }
      }, {
        key: "editProductNames",
        value: function editProductNames(mapping, key, $event) {
          var nameToUpdate = $event.target.innerText;
          var existingId = '';

          switch (key) {
            case 'MyProductId':
              existingId = mapping.MyProductId;
              break;

            case 'DriverProductId':
              existingId = mapping.DriverProductId;
              break;

            case 'BackOfficeProductId':
              existingId = mapping.BackOfficeProductId;
              break;
            //case 'TerminalItemCode':
            //    existingId = mapping.TerminalItemCode;

            default:
              break;
          }

          if (nameToUpdate == undefined || nameToUpdate == null || nameToUpdate.trim() == '' || nameToUpdate == '--') nameToUpdate = '';
          if (existingId == undefined || existingId == null || existingId.trim() == '' || existingId == '--') existingId = '';
          if (nameToUpdate == existingId) return;
          mapping[key] = nameToUpdate.trim();
          var obj = this.UpdateProductMappingList.filter(function (map) {
            return map.Id === mapping.Id;
          });

          if (this.UpdateProductMappingList.length == 0) {
            this.UpdateProductMappingList.push(mapping);
          } else if (obj.length == 1) {
            obj[key] = nameToUpdate.trim();
          } else {
            this.UpdateProductMappingList.push(mapping);
          }
        }
      }, {
        key: "deleteMapping",
        value: function deleteMapping(mappingId, companyId) {
          var _this48 = this;

          if (mappingId == undefined || mappingId <= 0) return;
          var model = new _models_ProductMappingModel__WEBPACK_IMPORTED_MODULE_2__["ProductMappingGridModel"]();
          model.Id = mappingId;
          model.CompanyId = companyId;
          this.IsLoading = true;
          this.carrierService.postDeleteProductMapping(model).subscribe(function (data) {
            _this48.IsLoading = false;

            if (data.StatusCode == 0) {
              _this48.clearForm();

              _this48.getProductMappingGridDetails();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#product-mapping-grid-datatable").DataTable().clear().destroy();
          this.getProductMappingGridDetails();
        }
      }, {
        key: "onStateSelect",
        value: function onStateSelect(state) {
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "onStateDeSelect",
        value: function onStateDeSelect(state) {
          this.ProductMappingForm.get('Cities').patchValue([]);
          this.ProductMappingForm.get('Terminals').patchValue([]);
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "onStateSelectAll",
        value: function onStateSelectAll(states) {
          this.ProductMappingForm.get('States').patchValue(states);
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "onStateDeSelectAll",
        value: function onStateDeSelectAll() {
          this.ProductMappingForm.get('Cities').patchValue([]);
          this.ProductMappingForm.get('Terminals').patchValue([]);
          this.CityList = [];
          this.TerminalList = [];
        }
      }, {
        key: "getCitiesByStateId",
        value: function getCitiesByStateId() {
          var selectedStates = this.ProductMappingForm.get('States').value;

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (m) {
              return m.Id;
            });
            this.getCities(stateIds);
          } else {
            this.CityList = [];
          }
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.ProductMappingForm.reset();
          this.ProductMappingForm.get('States').setValue([]);
          this.ProductMappingForm.get('Cities').setValue([]);
          this.ProductMappingForm.get('Terminals').setValue([]);
          this.ProductMappingForm.get('FuelTypes').setValue([]);
          this.CityList = [];
          this.TerminalList = [];
          $("#product-mapping-grid-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "clearFilter",
        value: function clearFilter() {
          this.clearForm();
          this.getProductMappingGridDetails();
        }
      }, {
        key: "onCitySelect",
        value: function onCitySelect(city) {
          this.getTerminals();
        }
      }, {
        key: "onCityDeSelect",
        value: function onCityDeSelect(city) {
          this.ProductMappingForm.get('Terminals').patchValue([]);
          this.getTerminals();
        }
      }, {
        key: "onCitySelectAll",
        value: function onCitySelectAll(cities) {
          this.ProductMappingForm.get('Cities').patchValue(cities);
          this.getTerminals();
        }
      }, {
        key: "onCityDeSelectAll",
        value: function onCityDeSelectAll() {
          this.ProductMappingForm.get('Terminals').patchValue([]);
          this.ProductMappingForm.get('Cities').patchValue([]);
          this.getTerminals(); //this.CityList = [];
        }
      }, {
        key: "selectFiles",
        value: function selectFiles(files) {
          if (files != null && files != undefined && files.length > 0) this.SelectedFiles = files;
        }
      }, {
        key: "uploadProductMappingFile",
        value: function uploadProductMappingFile() {
          var _this49 = this;

          var files = this.SelectedFiles;
          if (files.length === 0) return;
          var formData = new FormData();

          var _iterator = _createForOfIteratorHelper(files),
              _step;

          try {
            for (_iterator.s(); !(_step = _iterator.n()).done;) {
              var file = _step.value;

              if (!this.isValidFile(file)) {
                return;
              }

              formData.append(file.name, file);
            }
          } catch (err) {
            _iterator.e(err);
          } finally {
            _iterator.f();
          }

          this.IsLoading = true;
          this.carrierService.postBulkUploadTemplate(formData).subscribe(function (data) {
            _this49.IsLoading = false;

            if (data.StatusCode == 0) {
              _this49.CloseBulkUploadPopup.nativeElement.click();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              _this49.SelectedFiles = []; //$("#product-mapping-grid-datatable").DataTable().clear().destroy();
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "isValidFile",
        value: function isValidFile(file) {
          var isValid = true;
          var extension = this.getExtension(file.name);

          if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
          }

          if (file.size > this.MaxFileUploadSize) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
          }

          return isValid;
        }
      }, {
        key: "downloadCsvTemplate",
        value: function downloadCsvTemplate() {
          var _this50 = this;

          this.IsLoading = true;
          var timestamp = new Date().getTime();
          this.carrierService.downloadProductMappingTemplate(timestamp).subscribe(function (blob) {
            var a = document.createElement('a');
            var objectUrl = URL.createObjectURL(blob);
            a.href = objectUrl;
            a.download = 'ProductMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);
            _this50.IsLoading = false;
          });
        }
      }, {
        key: "getExtension",
        value: function getExtension(fileName) {
          // extract file name from full path ...
          var basename = fileName.split(/[\\/]/).pop(); // (supports `\\` and `/` separators)

          var pos = basename.lastIndexOf("."); // get last position of `.`

          if (basename === "" || pos < 1) // if file name is empty or ...
            return ""; //  `.` not found (-1) or comes first (0)

          return basename.slice(pos + 1); // extract extension ignoring `.`
        }
      }, {
        key: "showBulkUploadPopup",
        value: function showBulkUploadPopup() {
          this.IsShowBulkUploadPopup = true;
        }
      }, {
        key: "closePopup",
        value: function closePopup() {
          this.IsShowBulkUploadPopup = false;
        }
      }, {
        key: "initializeProductMappingDatatableGrid",
        value: function initializeProductMappingDatatableGrid() {
          var exportColumns = {
            columns: [0, 1, 2, 3, 4, 5]
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
              title: 'Product Mapping',
              exportOptions: exportColumns
            }, {
              extend: 'pdf',
              title: 'Product Mapping',
              orientation: 'landscape',
              exportOptions: exportColumns
            }, {
              extend: 'print',
              exportOptions: exportColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //,
            //rowCallback: (row: Node, data: any[] | Object, index: number) => {
            //    const self = this;
            //    // Unbind first in order to avoid any duplicate handler
            //    // (see https://github.com/l-lin/angular-datatables/issues/87)
            //    $('td', row).unbind('click');
            //    $('td', row).bind('click', () => {
            //        self.someClickHandler(data, event);
            //    });
            //    return row;
            //}

          };
        }
      }]);

      return ProductMappingComponent;
    }();

    ProductMappingComponent.ɵfac = function ProductMappingComponent_Factory(t) {
      return new (t || ProductMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]));
    };

    ProductMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ProductMappingComponent,
      selectors: [["app-product-mapping"]],
      viewQuery: function ProductMappingComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c1, true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.MyProductId = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.CloseBulkUploadPopup = _t.first);
        }
      },
      inputs: {
        countryId: "countryId"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 86,
      vars: 26,
      consts: [[3, "formGroup", "ngSubmit"], [1, "col-sm-12"], [1, "row"], [1, "well", "col-sm-12"], [1, "col-sm-2"], [1, "form-group"], ["formControlName", "States", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], ["formControlName", "Cities", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "col-sm-3"], ["formControlName", "Terminals", 3, "placeholder", "settings", "data"], ["class", "color-maroon", 4, "ngIf"], [1, "color-maroon"], ["formControlName", "FuelTypes", 3, "placeholder", "settings", "data"], [1, "col-sm-2", "text-left", "form-buttons", "mt20", "pt-1"], ["id", "filter-product-mapping-grid", "type", "button", 1, "btn", "btn-primary", "mt3", "valid", "mx-0", 3, "ngClass", "click"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "mx-0", "px-1", "valid", 3, "click"], ["type", "text", "formControlName", "MyProductId", 1, "form-control"], ["MyProductId", ""], ["type", "text", "formControlName", "BackOfficeProductId", 1, "form-control"], ["type", "text", "formControlName", "DriverProductId", 1, "form-control"], [1, "col-sm-2", "text-left", "form-buttons", "mt20"], ["id", "submit-product-mapping-form", "type", "submit", "aria-invalid", "false", 1, "mt4", "btn", "btn-lg", "btn-default", "valid", 3, "ngClass"], [1, "col-sm-12", "text-right", "form-buttons", "mb5"], ["id", "showBulkUploadPopupBtn", "type", "button", "data-toggle", "modal", "data-target", "#bulkUploadModalPopup", 1, "btn", "btn-primary", "valid", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-border"], ["id", "div-product-mapping-grid", 1, "table-responsive"], ["id", "product-mapping-grid-datatable", "data-gridname", "14", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "FuelType"], ["data-key", "TerminalName"], ["data-key", "TerminalAddress"], ["data-key", "BackOfficeProductId"], ["data-key", "DriverProductId"], ["data-key", "MyProductId"], ["data-key", "Action"], [4, "ngFor", "ngForOf"], ["id", "bulkUploadModalPopup", "class", "modal fade", "role", "dialog", 4, "ngIf"], ["class", "loader", 4, "ngIf"], [4, "ngIf"], ["contenteditable", "true", 1, "edit-td", 3, "blur"], [1, "text-center"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm", "cancel"], ["alt", "Update", "title", "Update", 1, "fs21", "fas", "fa-save", "color-green"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], ["id", "bulkUploadModalPopup", "role", "dialog", 1, "modal", "fade"], [1, "modal-dialog"], [1, "modal-content"], [1, "modal-header", "pt0", "pb5"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", 1, "close", "color-grey", 3, "click"], ["btnCloseBulkUploadPopup", ""], [1, "fa", "fa-close", "fs21", "mt15"], [1, "modal-body"], [1, "mb10"], [1, "fa", "fa-download", "mr10", "mt10"], ["role", "button", 1, "mb5", "btn-download", 3, "click"], [1, "mb5", "mt5"], ["type", "file", "id", "bulkUploadFile", "accept", ".csv", 3, "change"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "modal-footer"], ["type", "button", 1, "btn", "btn-default", 3, "click"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function ProductMappingComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function ProductMappingComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "ng-multiselect-dropdown", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function ProductMappingComponent_Template_ng_multiselect_dropdown_onSelect_9_listener($event) {
            return ctx.onStateSelect($event);
          })("onDeSelect", function ProductMappingComponent_Template_ng_multiselect_dropdown_onDeSelect_9_listener($event) {
            return ctx.onStateDeSelect($event);
          })("onSelectAll", function ProductMappingComponent_Template_ng_multiselect_dropdown_onSelectAll_9_listener($event) {
            return ctx.onStateSelectAll($event);
          })("onDeSelectAll", function ProductMappingComponent_Template_ng_multiselect_dropdown_onDeSelectAll_9_listener() {
            return ctx.onStateDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function ProductMappingComponent_Template_ng_multiselect_dropdown_onSelect_14_listener($event) {
            return ctx.onCitySelect($event);
          })("onDeSelect", function ProductMappingComponent_Template_ng_multiselect_dropdown_onDeSelect_14_listener($event) {
            return ctx.onCityDeSelect($event);
          })("onSelectAll", function ProductMappingComponent_Template_ng_multiselect_dropdown_onSelectAll_14_listener($event) {
            return ctx.onCitySelectAll($event);
          })("onDeSelectAll", function ProductMappingComponent_Template_ng_multiselect_dropdown_onDeSelectAll_14_listener() {
            return ctx.onCityDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Terminal Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](19, "ng-multiselect-dropdown", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, ProductMappingComponent_div_20_Template, 2, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](27, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, ProductMappingComponent_div_28_Template, 2, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "button", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_Template_button_click_30_listener() {
            return ctx.filterGrid();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Apply");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "button", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_Template_button_click_32_listener() {
            return ctx.clearFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](33, "Clear Filter");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "My Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](39, "input", 16, 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Back Office Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](45, "input", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](49, "Driver Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](50, "input", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "button", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "Assign");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "button", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ProductMappingComponent_Template_button_click_56_listener() {
            return ctx.showBulkUploadPopup();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "table", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](66, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "th", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](69, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](70, "th", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](71, "Terminal");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "th", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](73, "Terminal Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](74, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](75, "Back-Office Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](77, "Driver Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](78, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](79, "My Product ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](80, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](81, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](82, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](83, ProductMappingComponent_tr_83_Template, 21, 15, "tr", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](84, ProductMappingComponent_div_84_Template, 22, 1, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](85, ProductMappingComponent_div_85_Template, 5, 0, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.ProductMappingForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select State(s)")("settings", ctx.ddlSettingsById)("data", ctx.StateList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select City(s)")("settings", ctx.ddlSettingsByCode)("data", ctx.CityList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal(s)")("settings", ctx.ddlSettingsById)("data", ctx.TerminalList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsValidForm && (ctx.ProductMappingForm.get("Terminals").errors == null ? null : ctx.ProductMappingForm.get("Terminals").errors.required));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select FuelType")("settings", ctx.ddlSettingsByIdSingleSelect)("data", ctx.FuelTypeList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsValidForm && (ctx.ProductMappingForm.get("FuelTypes").errors == null ? null : ctx.ProductMappingForm.get("FuelTypes").errors.required));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](22, _c2, ctx.IsLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](24, _c2, ctx.IsLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.ProductMappingList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsShowBulkUploadPopup);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_6__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], angular_datatables__WEBPACK_IMPORTED_MODULE_8__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__["ɵc"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy9wcm9kdWN0LW1hcHBpbmcvcHJvZHVjdC1tYXBwaW5nLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ProductMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-product-mapping',
          templateUrl: './product-mapping.component.html',
          styleUrls: ['./product-mapping.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]
        }];
      }, {
        countryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        MyProductId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['MyProductId']
        }],
        CloseBulkUploadPopup: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkUploadPopup']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/self-service-alias.component.ts": function srcAppSelfServiceAliasSelfServiceAliasComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SelfServiceAliasComponent", function () {
      return SelfServiceAliasComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! src/app/statelist.service */
    "./src/app/statelist.service.ts");
    /* harmony import */


    var _product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./product-mapping/product-mapping.component */
    "./src/app/self-service-alias/product-mapping/product-mapping.component.ts");
    /* harmony import */


    var _models_location__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./models/location */
    "./src/app/self-service-alias/models/location.ts");
    /* harmony import */


    var _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../self-service-alias/service/externalmappings.service */
    "./src/app/self-service-alias/service/externalmappings.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _company_carrier_mapping_company_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./company-carrier-mapping/company-carrier-mapping.component */
    "./src/app/self-service-alias/company-carrier-mapping/company-carrier-mapping.component.ts");
    /* harmony import */


    var _customer_mapping_customer_mapping_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./customer-mapping/customer-mapping.component */
    "./src/app/self-service-alias/customer-mapping/customer-mapping.component.ts");
    /* harmony import */


    var _terminal_mapping_terminal_mapping_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./terminal-mapping/terminal-mapping.component */
    "./src/app/self-service-alias/terminal-mapping/terminal-mapping.component.ts");
    /* harmony import */


    var _terminal_item_code_mapping_terminal_item_code_mapping_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./terminal-item-code-mapping/terminal-item-code-mapping.component */
    "./src/app/self-service-alias/terminal-item-code-mapping/terminal-item-code-mapping.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_customer_mappings_external_customer_mappings_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./external-entity-mappings/external-customer-mappings/external-customer-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-customer-mappings/external-customer-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_customerlocation_mappings_external_customerlocation_mappings_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_bulk_plant_mappings_external_bulk_plant_mappings_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ./external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_carrier_mappings_external_carrier_mappings_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ./external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_terminal_mappings_external_terminal_mappings_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ./external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_product_mappings_external_product_mappings_component__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ./external-entity-mappings/external-product-mappings/external-product-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-product-mappings/external-product-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_driver_mappings_external_driver_mappings_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ./external-entity-mappings/external-driver-mappings/external-driver-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-driver-mappings/external-driver-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_vehicle_mappings_external_vehicle_mappings_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ./external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_supplier_mappings_external_supplier_mappings_component__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(
    /*! ./external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component.ts");

    function SelfServiceAliasComponent_option_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var company_r4 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", company_r4.Id)("selected", ctx_r0.SelectedCompany.Id == company_r4.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](company_r4.Name);
      }
    }

    function SelfServiceAliasComponent_div_7_div_1_option_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var country_r7 = ctx.$implicit;

        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", country_r7.Id)("selected", ctx_r6.SelectedCountryId == country_r7.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](country_r7.Code);
      }
    }

    function SelfServiceAliasComponent_div_7_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "select", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SelfServiceAliasComponent_div_7_div_1_Template_select_change_2_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r8.countryChanged($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SelfServiceAliasComponent_div_7_div_1_option_3_Template, 2, 3, "option", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r5.CountryList);
      }
    }

    function SelfServiceAliasComponent_div_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SelfServiceAliasComponent_div_7_div_1_Template, 4, 1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.isShowCountryDDL);
      }
    }

    function SelfServiceAliasComponent_div_8_div_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ul", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "li", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_3_Template_a_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r15);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r14.changeTab("Carrier");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Carrier Mapping");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SelfServiceAliasComponent_div_8_div_4_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_4_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r16.changeTab("Product");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Product");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_4_Template_a_click_3_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r18.changeTab("Customer");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "a", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_4_Template_a_click_5_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r19.changeTab("Carrier");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Carrier");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "a", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_4_Template_a_click_7_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r20.changeTab("Terminal");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Terminal");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "a", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_8_div_4_Template_a_click_9_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r21.changeTab("TerminalCode");
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Terminal Item Code");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SelfServiceAliasComponent_div_8_div_5_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-company-carrier-mapping", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx_r22.SelectedCountryId);
      }
    }

    function SelfServiceAliasComponent_div_8_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, SelfServiceAliasComponent_div_8_div_5_div_2_Template, 2, 1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r12.isShowCarrier);
      }
    }

    function SelfServiceAliasComponent_div_8_div_6_app_product_mapping_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-product-mapping", 33);
      }

      if (rf & 2) {
        var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx_r23.SelectedCountryId);
      }
    }

    function SelfServiceAliasComponent_div_8_div_6_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-company-carrier-mapping", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx_r24.SelectedCountryId);
      }
    }

    function SelfServiceAliasComponent_div_8_div_6_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-terminal-item-code-mapping", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx_r25.SelectedCountryId);
      }
    }

    function SelfServiceAliasComponent_div_8_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SelfServiceAliasComponent_div_8_div_6_app_product_mapping_3_Template, 1, 1, "app-product-mapping", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "app-customer-mapping");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, SelfServiceAliasComponent_div_8_div_6_div_8_Template, 2, 1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "app-terminal-mapping", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, SelfServiceAliasComponent_div_8_div_6_div_13_Template, 2, 1, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.IsShowProductMappingComponent);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.isShowCarrier);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx_r13.SelectedCountryId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.isShowTerminalmappingCode);
      }
    }

    function SelfServiceAliasComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SelfServiceAliasComponent_div_8_div_3_Template, 5, 0, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, SelfServiceAliasComponent_div_8_div_4_Template, 11, 0, "div", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, SelfServiceAliasComponent_div_8_div_5_Template, 3, 1, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, SelfServiceAliasComponent_div_8_div_6_Template, 14, 4, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.IsShowCarrierMappingComponent);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r2.IsShowCarrierMappingComponent);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r2.IsShowCarrierMappingComponent);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r2.IsShowCarrierMappingComponent);
      }
    }

    function SelfServiceAliasComponent_div_9_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-customer-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r26.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-customerlocation-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r27.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_26_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-bulk-plant-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r28.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-carrier-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r29.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_28_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-terminal-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r30.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_29_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-product-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r31.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-driver-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r32.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_31_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-vehicle-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r33.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-external-supplier-mappings", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("thirdPartyCompanyId", ctx_r34.SelectedCompany.Id);
      }
    }

    function SelfServiceAliasComponent_div_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r35.pdiTabName = "CUSTOMERS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Customer");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "a", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r37.pdiTabName = "CUSTOMERLOCATIONS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Customer Location");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "a", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_8_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r38.pdiTabName = "VENDORS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Vendors");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "a", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r39.pdiTabName = "PRODUCTS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Products");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r40.pdiTabName = "TERMINALS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Terminals");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r41.pdiTabName = "SITES";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Sites");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "a", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_16_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r42.pdiTabName = "CARRIERS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Carriers");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "a", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r43.pdiTabName = "DRIVERS";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Driver");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "a", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SelfServiceAliasComponent_div_9_Template_a_click_20_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r44 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r44.pdiTabName = "VEHICLE";
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Vehicle");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, SelfServiceAliasComponent_div_9_div_24_Template, 2, 1, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, SelfServiceAliasComponent_div_9_div_25_Template, 2, 1, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, SelfServiceAliasComponent_div_9_div_26_Template, 2, 1, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, SelfServiceAliasComponent_div_9_div_27_Template, 2, 1, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](28, SelfServiceAliasComponent_div_9_div_28_Template, 2, 1, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](29, SelfServiceAliasComponent_div_9_div_29_Template, 2, 1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](30, SelfServiceAliasComponent_div_9_div_30_Template, 2, 1, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](31, SelfServiceAliasComponent_div_9_div_31_Template, 2, 1, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, SelfServiceAliasComponent_div_9_div_32_Template, 2, 1, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "CUSTOMERS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "CUSTOMERLOCATIONS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "SITES");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "CARRIERS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "TERMINALS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "PRODUCTS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "DRIVERS");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "VEHICLE");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.pdiTabName == "VENDORS");
      }
    }

    var SelfServiceAliasComponent = /*#__PURE__*/function () {
      function SelfServiceAliasComponent(carrierService, externalMappingsService) {
        _classCallCheck(this, SelfServiceAliasComponent);

        this.carrierService = carrierService;
        this.externalMappingsService = externalMappingsService;
        this.CountryEnum = _models_location__WEBPACK_IMPORTED_MODULE_3__["Country"];
        this.CountryType = [];
        this.isShow = false;
        this.isShowCarrier = false;
        this.isShowCountryDDL = true;
        this.CountryList = [];
        this.IsShowProductMappingComponent = true;
        this.IsShowCarrierMappingComponent = false;
        this.isShowTerminalmappingCode = false;
        this.isShowFuelGroup = false;
        this.SelectedCompany = new src_app_statelist_service__WEBPACK_IMPORTED_MODULE_1__["DropdownItem"]();
        this.ServingCountries = {};
        this.externalCompanies = [];
        this.pdiTabName = 'CUSTOMERS';
      }

      _createClass(SelfServiceAliasComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.CurrentCompanyId = Number(currentCompanyId);
          this.SelectedCountryId = Number(defaultCountryId);
          this.getExternalCompanies();
          this.getCountries();

          if (!carrierMappingVisible) {
            this.IsShowCarrierMappingComponent = false;
          } else {
            this.IsShowCarrierMappingComponent = true; // this.isShowCarrier = true;
          }
        }
      }, {
        key: "getExternalCompanies",
        value: function getExternalCompanies() {
          var _this51 = this;

          this.externalMappingsService.getExternalCompanies().subscribe(function (data) {
            if (data != null && data.length > 0) {
              _this51.externalCompanies = data;
              _this51.SelectedCompany = _this51.externalCompanies[0];
            }
          });
        }
      }, {
        key: "changeTab",
        value: function changeTab(currencyDdlHide) {
          if (currencyDdlHide === "Carrier") {
            this.isShowCarrier = true;
            this.isShowTerminalmappingCode = false;
          } else if (currencyDdlHide === "Customer") {
            this.isShow = true;
            this.isShowTerminalmappingCode = false;
            this.isShowCarrier = false;
          } else if (currencyDdlHide === "TerminalCode") {
            this.isShowTerminalmappingCode = true;
            this.isShowCarrier = false;
          } else if (currencyDdlHide === "FuelGroup") {
            // this.isShow = false;
            this.isShowCarrier = false;
            this.isShowTerminalmappingCode = false;
            this.isShowFuelGroup = true;
          } else {
            this.isShow = !this.isShow;
            this.isShowTerminalmappingCode = false;
            this.isShowCarrier = false;
            this.isShowFuelGroup = false;
          }
        }
      }, {
        key: "onCountryChange",
        value: function onCountryChange() {
          localStorage.setItem('countryFilterType', this.CountryFilter);
        }
      }, {
        key: "countryChanged",
        value: function countryChanged(event) {
          this.IsShowProductMappingComponent = false;
          this.SelectedCountryId = event.target.value == "null" || event.target.value == null ? 1 : Number(event.target.value);
          this.IsShowProductMappingComponent = true;
        }
      }, {
        key: "companyChanged",
        value: function companyChanged(event) {
          if (event != null) {
            this.SelectedCompany = this.externalCompanies.find(function (t) {
              return t.Id == event.target.value;
            });
          }
        }
      }, {
        key: "checkWindowSelection",
        value: function checkWindowSelection() {
          this.locationViewType = localStorage.getItem('locationViewType') ? localStorage.getItem('locationViewType') : 1;
          this.CountryFilter = localStorage.getItem('countryFilterType') ? localStorage.getItem('countryFilterType') : localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1;
        }
      }, {
        key: "getCountries",
        value: function getCountries() {
          var _this52 = this;

          this.carrierService.GetCountries(this.CurrentCompanyId).subscribe(function (data) {
            if (data != null) {
              if (data.ServingCountries != null && data.ServingCountries.length > 1) {
                _this52.ServingCountries = data.ServingCountries;
                _this52.CountryList = data.CountryList;

                if (isNaN(_this52.SelectedCountryId) || _this52.SelectedCountryId == 0) {
                  defaultCountryId = data.DefaultCountryId;
                  _this52.SelectedCountryId = Number(data.DefaultCountryId);
                }
              } else {
                _this52.isShowCountryDDL = false;
              }
            }
          });
        }
      }]);

      return SelfServiceAliasComponent;
    }();

    SelfServiceAliasComponent.ɵfac = function SelfServiceAliasComponent_Factory(t) {
      return new (t || SelfServiceAliasComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_5__["ExternalMappingsService"]));
    };

    SelfServiceAliasComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: SelfServiceAliasComponent,
      selectors: [["app-self-service-alias"]],
      viewQuery: function SelfServiceAliasComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_2__["ProductMappingComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.ProductMappingComponent = _t.first);
        }
      },
      decls: 10,
      vars: 4,
      consts: [["id", "self-service-alias-tab", 1, "small-tab"], [1, "row"], [1, "col-sm-12"], [1, "row", "mb10"], [1, "col-2", "text-right"], [1, "form-control", 3, "change"], [3, "value", "selected", 4, "ngFor", "ngForOf"], ["class", "col-10", 4, "ngIf"], ["class", "value-mapping", 4, "ngIf"], ["class", "pdi-entity-mapping mt10", 4, "ngIf"], [3, "value", "selected"], [1, "col-10"], ["class", "float-right text-right", 4, "ngIf"], [1, "float-right", "text-right"], [1, "form-group", "mb0"], [1, "value-mapping"], [1, "col-2"], ["class", "float-left text-left mb10", 4, "ngIf"], ["class", "nav flex-column nav-pills", 4, "ngIf"], ["class", "tab-content col-sm-10", 4, "ngIf"], [1, "float-left", "text-left", "mb10"], ["role", "tablist", 1, "nav", "nav-tabs", "mb5"], [1, "nav-item"], ["data-toggle", "tab", "href", "#carrier-mapping-container", "role", "tab", "aria-controls", "home", "aria-selected", "true", 1, "nav-link", "active", "fs16", 3, "click"], [1, "nav", "flex-column", "nav-pills"], ["id", "home-tab", "data-toggle", "pill", "href", "#product-mapping-container", "role", "tab", "aria-controls", "home", "aria-selected", "true", 1, "nav-link", "fs14", "active", 3, "click"], ["id", "profile-tab", "data-toggle", "pill", "href", "#customer-mapping-container", "role", "tab", "aria-controls", "profile", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "contact-tab", "data-toggle", "pill", "href", "#carrier-mapping-container", "role", "tab", "aria-controls", "contact", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "contact-tab", "data-toggle", "pill", "href", "#terminal-mapping-container", "role", "tab", "aria-controls", "contact", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "contact-tab", "data-toggle", "pill", "href", "#terminal-code-mapping-container", "role", "tab", "aria-controls", "contact", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], [1, "tab-content", "col-sm-10"], ["id", "carrier-mapping-container", 1, "tab-pane", "fade", "show", "active"], [4, "ngIf"], [3, "countryId"], ["id", "product-mapping-container", 1, "tab-pane", "fade", "show", "active"], [3, "countryId", 4, "ngIf"], ["id", "customer-mapping-container", 1, "tab-pane", "fade"], ["id", "carrier-mapping-container", 1, "tab-pane", "fade"], ["id", "terminal-mapping-container", 1, "tab-pane", "fade"], ["id", "terminal-code-mapping-container", 1, "tab-pane", "fade"], [1, "pdi-entity-mapping", "mt10"], ["role", "tablist", 1, "nav", "flex-column", "nav-pills"], ["id", "customer-tab", "data-toggle", "pill", "href", "#customer-container", "role", "tab", "aria-controls", "home", "aria-selected", "true", 1, "nav-link", "fs14", "active", 3, "click"], ["id", "cust-location-tab", "data-toggle", "pill", "href", "#cust-location-container", "role", "tab", "aria-controls", "cust-location", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "vendor-tab", "data-toggle", "pill", "href", "#vendor-container", "role", "tab", "aria-controls", "vendor", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "products-tab", "data-toggle", "pill", "href", "#products-container", "role", "tab", "aria-controls", "products", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "terminals-tab", "data-toggle", "pill", "href", "#terminals-container", "role", "tab", "aria-controls", "terminals", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "sites-tab", "data-toggle", "pill", "href", "#sites-container", "role", "tab", "aria-controls", "sites", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "carriers-tab", "data-toggle", "pill", "href", "#carriers-container", "role", "tab", "aria-controls", "carriers", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "driver-tab", "data-toggle", "pill", "href", "#driver-container", "role", "tab", "aria-controls", "driver", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], ["id", "vehical-tab", "data-toggle", "pill", "href", "#vehical-container", "role", "tab", "aria-controls", "vehical", "aria-selected", "false", 1, "nav-link", "fs14", 3, "click"], [1, "tab-content"], ["id", "customer-container", "class", "tab-pane fade show active", 4, "ngIf"], ["id", "cust-location-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "sites-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "carriers-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "terminals-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "products-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "driver-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "vehical-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "vendor-container", "class", "tab-pane fade", 4, "ngIf"], ["id", "customer-container", 1, "tab-pane", "fade", "show", "active"], [3, "thirdPartyCompanyId"], ["id", "cust-location-container", 1, "tab-pane", "fade"], ["id", "sites-container", 1, "tab-pane", "fade"], ["id", "carriers-container", 1, "tab-pane", "fade"], ["id", "terminals-container", 1, "tab-pane", "fade"], ["id", "products-container", 1, "tab-pane", "fade"], ["id", "driver-container", 1, "tab-pane", "fade"], ["id", "vehical-container", 1, "tab-pane", "fade"], ["id", "vendor-container", 1, "tab-pane", "fade"]],
      template: function SelfServiceAliasComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "select", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SelfServiceAliasComponent_Template_select_change_5_listener($event) {
            return ctx.companyChanged($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, SelfServiceAliasComponent_option_6_Template, 2, 3, "option", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, SelfServiceAliasComponent_div_7_Template, 2, 1, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, SelfServiceAliasComponent_div_8_Template, 7, 4, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, SelfServiceAliasComponent_div_9_Template, 33, 9, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.externalCompanies);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.SelectedCompany == null ? null : ctx.SelectedCompany.Id) == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.SelectedCompany == null ? null : ctx.SelectedCompany.Id) == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.SelectedCompany == null ? null : ctx.SelectedCompany.Id) == 2);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["ɵangular_packages_forms_forms_x"], _company_carrier_mapping_company_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_8__["CompanyCarrierMappingComponent"], _customer_mapping_customer_mapping_component__WEBPACK_IMPORTED_MODULE_9__["CustomerMappingComponent"], _terminal_mapping_terminal_mapping_component__WEBPACK_IMPORTED_MODULE_10__["TerminalMappingComponent"], _product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_2__["ProductMappingComponent"], _terminal_item_code_mapping_terminal_item_code_mapping_component__WEBPACK_IMPORTED_MODULE_11__["TerminalItemCodeMappingComponent"], _external_entity_mappings_external_customer_mappings_external_customer_mappings_component__WEBPACK_IMPORTED_MODULE_12__["ExternalCustomerMappingsComponent"], _external_entity_mappings_external_customerlocation_mappings_external_customerlocation_mappings_component__WEBPACK_IMPORTED_MODULE_13__["ExternalCustomerlocationMappingsComponent"], _external_entity_mappings_external_bulk_plant_mappings_external_bulk_plant_mappings_component__WEBPACK_IMPORTED_MODULE_14__["ExternalBulkPlantMappingsComponent"], _external_entity_mappings_external_carrier_mappings_external_carrier_mappings_component__WEBPACK_IMPORTED_MODULE_15__["ExternalCarrierMappingsComponent"], _external_entity_mappings_external_terminal_mappings_external_terminal_mappings_component__WEBPACK_IMPORTED_MODULE_16__["ExternalTerminalMappingsComponent"], _external_entity_mappings_external_product_mappings_external_product_mappings_component__WEBPACK_IMPORTED_MODULE_17__["ExternalProductMappingsComponent"], _external_entity_mappings_external_driver_mappings_external_driver_mappings_component__WEBPACK_IMPORTED_MODULE_18__["ExternalDriverMappingsComponent"], _external_entity_mappings_external_vehicle_mappings_external_vehicle_mappings_component__WEBPACK_IMPORTED_MODULE_19__["ExternalVehicleMappingsComponent"], _external_entity_mappings_external_supplier_mappings_external_supplier_mappings_component__WEBPACK_IMPORTED_MODULE_20__["ExternalSupplierMappingsComponent"]],
      styles: [".vertical-tab.nav-item[_ngcontent-%COMP%] {\r\n    margin-bottom: 5px;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2VsZi1zZXJ2aWNlLWFsaWFzL3NlbGYtc2VydmljZS1hbGlhcy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksa0JBQWtCO0FBQ3RCIiwiZmlsZSI6InNyYy9hcHAvc2VsZi1zZXJ2aWNlLWFsaWFzL3NlbGYtc2VydmljZS1hbGlhcy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLnZlcnRpY2FsLXRhYi5uYXYtaXRlbSB7XHJcbiAgICBtYXJnaW4tYm90dG9tOiA1cHg7XHJcbn1cclxuIl19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SelfServiceAliasComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-self-service-alias',
          templateUrl: './self-service-alias.component.html',
          styleUrls: ['./self-service-alias.component.css']
        }]
      }], function () {
        return [{
          type: _carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_4__["CarrierService"]
        }, {
          type: _self_service_alias_service_externalmappings_service__WEBPACK_IMPORTED_MODULE_5__["ExternalMappingsService"]
        }];
      }, {
        ProductMappingComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_2__["ProductMappingComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/self-service-alias.module.ts": function srcAppSelfServiceAliasSelfServiceAliasModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SelfServiceAliasModule", function () {
      return SelfServiceAliasModule;
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


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _self_service_alias_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ./self-service-alias.component */
    "./src/app/self-service-alias/self-service-alias.component.ts");
    /* harmony import */


    var _product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ./product-mapping/product-mapping.component */
    "./src/app/self-service-alias/product-mapping/product-mapping.component.ts");
    /* harmony import */


    var _customer_mapping_customer_mapping_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ./customer-mapping/customer-mapping.component */
    "./src/app/self-service-alias/customer-mapping/customer-mapping.component.ts");
    /* harmony import */


    var _edit_customer_edit_customer_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./edit-customer/edit-customer.component */
    "./src/app/self-service-alias/edit-customer/edit-customer.component.ts");
    /* harmony import */


    var _company_carrier_mapping_company_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./company-carrier-mapping/company-carrier-mapping.component */
    "./src/app/self-service-alias/company-carrier-mapping/company-carrier-mapping.component.ts");
    /* harmony import */


    var _edit_carrier_mapping_edit_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! ./edit-carrier-mapping/edit-carrier-mapping.component */
    "./src/app/self-service-alias/edit-carrier-mapping/edit-carrier-mapping.component.ts");
    /* harmony import */


    var _terminal_mapping_terminal_mapping_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ./terminal-mapping/terminal-mapping.component */
    "./src/app/self-service-alias/terminal-mapping/terminal-mapping.component.ts");
    /* harmony import */


    var _terminal_item_code_mapping_terminal_item_code_mapping_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ./terminal-item-code-mapping/terminal-item-code-mapping.component */
    "./src/app/self-service-alias/terminal-item-code-mapping/terminal-item-code-mapping.component.ts");
    /* harmony import */


    var _create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ./create-terminal-item-code/create-terminal-item-code.component */
    "./src/app/self-service-alias/create-terminal-item-code/create-terminal-item-code.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_customer_mappings_external_customer_mappings_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ./external-entity-mappings/external-customer-mappings/external-customer-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-customer-mappings/external-customer-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_customerlocation_mappings_external_customerlocation_mappings_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! ./external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_product_mappings_external_product_mappings_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ./external-entity-mappings/external-product-mappings/external-product-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-product-mappings/external-product-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_supplier_mappings_external_supplier_mappings_component__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! ./external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_terminal_mappings_external_terminal_mappings_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(
    /*! ./external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_driver_mappings_external_driver_mappings_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(
    /*! ./external-entity-mappings/external-driver-mappings/external-driver-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-driver-mappings/external-driver-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_carrier_mappings_external_carrier_mappings_component__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(
    /*! ./external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_bulk_plant_mappings_external_bulk_plant_mappings_component__WEBPACK_IMPORTED_MODULE_21__ = __webpack_require__(
    /*! ./external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component.ts");
    /* harmony import */


    var _external_entity_mappings_external_vehicle_mappings_external_vehicle_mappings_component__WEBPACK_IMPORTED_MODULE_22__ = __webpack_require__(
    /*! ./external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component */
    "./src/app/self-service-alias/external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component.ts");

    var routeSelfService = [{
      path: '',
      component: _self_service_alias_component__WEBPACK_IMPORTED_MODULE_5__["SelfServiceAliasComponent"]
    }];

    var SelfServiceAliasModule = function SelfServiceAliasModule() {
      _classCallCheck(this, SelfServiceAliasModule);
    };

    SelfServiceAliasModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: SelfServiceAliasModule
    });
    SelfServiceAliasModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function SelfServiceAliasModule_Factory(t) {
        return new (t || SelfServiceAliasModule)();
      },
      imports: [[_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeSelfService)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](SelfServiceAliasModule, {
        declarations: [_self_service_alias_component__WEBPACK_IMPORTED_MODULE_5__["SelfServiceAliasComponent"], _product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_6__["ProductMappingComponent"], _customer_mapping_customer_mapping_component__WEBPACK_IMPORTED_MODULE_7__["CustomerMappingComponent"], _edit_customer_edit_customer_component__WEBPACK_IMPORTED_MODULE_8__["EditCustomerComponent"], _company_carrier_mapping_company_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_9__["CompanyCarrierMappingComponent"], _edit_carrier_mapping_edit_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_10__["EditCarrierMappingComponent"], _terminal_mapping_terminal_mapping_component__WEBPACK_IMPORTED_MODULE_11__["TerminalMappingComponent"], _terminal_item_code_mapping_terminal_item_code_mapping_component__WEBPACK_IMPORTED_MODULE_12__["TerminalItemCodeMappingComponent"], _create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_13__["CreateTerminalItemCodeComponent"], _external_entity_mappings_external_customer_mappings_external_customer_mappings_component__WEBPACK_IMPORTED_MODULE_14__["ExternalCustomerMappingsComponent"], _external_entity_mappings_external_customerlocation_mappings_external_customerlocation_mappings_component__WEBPACK_IMPORTED_MODULE_15__["ExternalCustomerlocationMappingsComponent"], _external_entity_mappings_external_product_mappings_external_product_mappings_component__WEBPACK_IMPORTED_MODULE_16__["ExternalProductMappingsComponent"], _external_entity_mappings_external_supplier_mappings_external_supplier_mappings_component__WEBPACK_IMPORTED_MODULE_17__["ExternalSupplierMappingsComponent"], _external_entity_mappings_external_terminal_mappings_external_terminal_mappings_component__WEBPACK_IMPORTED_MODULE_18__["ExternalTerminalMappingsComponent"], _external_entity_mappings_external_driver_mappings_external_driver_mappings_component__WEBPACK_IMPORTED_MODULE_19__["ExternalDriverMappingsComponent"], _external_entity_mappings_external_carrier_mappings_external_carrier_mappings_component__WEBPACK_IMPORTED_MODULE_20__["ExternalCarrierMappingsComponent"], _external_entity_mappings_external_bulk_plant_mappings_external_bulk_plant_mappings_component__WEBPACK_IMPORTED_MODULE_21__["ExternalBulkPlantMappingsComponent"], _external_entity_mappings_external_vehicle_mappings_external_vehicle_mappings_component__WEBPACK_IMPORTED_MODULE_22__["ExternalVehicleMappingsComponent"]],
        imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SelfServiceAliasModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_self_service_alias_component__WEBPACK_IMPORTED_MODULE_5__["SelfServiceAliasComponent"], _product_mapping_product_mapping_component__WEBPACK_IMPORTED_MODULE_6__["ProductMappingComponent"], _customer_mapping_customer_mapping_component__WEBPACK_IMPORTED_MODULE_7__["CustomerMappingComponent"], _edit_customer_edit_customer_component__WEBPACK_IMPORTED_MODULE_8__["EditCustomerComponent"], _company_carrier_mapping_company_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_9__["CompanyCarrierMappingComponent"], _edit_carrier_mapping_edit_carrier_mapping_component__WEBPACK_IMPORTED_MODULE_10__["EditCarrierMappingComponent"], _terminal_mapping_terminal_mapping_component__WEBPACK_IMPORTED_MODULE_11__["TerminalMappingComponent"], _terminal_item_code_mapping_terminal_item_code_mapping_component__WEBPACK_IMPORTED_MODULE_12__["TerminalItemCodeMappingComponent"], _create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_13__["CreateTerminalItemCodeComponent"], _external_entity_mappings_external_customer_mappings_external_customer_mappings_component__WEBPACK_IMPORTED_MODULE_14__["ExternalCustomerMappingsComponent"], _external_entity_mappings_external_customerlocation_mappings_external_customerlocation_mappings_component__WEBPACK_IMPORTED_MODULE_15__["ExternalCustomerlocationMappingsComponent"], _external_entity_mappings_external_product_mappings_external_product_mappings_component__WEBPACK_IMPORTED_MODULE_16__["ExternalProductMappingsComponent"], _external_entity_mappings_external_supplier_mappings_external_supplier_mappings_component__WEBPACK_IMPORTED_MODULE_17__["ExternalSupplierMappingsComponent"], _external_entity_mappings_external_terminal_mappings_external_terminal_mappings_component__WEBPACK_IMPORTED_MODULE_18__["ExternalTerminalMappingsComponent"], _external_entity_mappings_external_driver_mappings_external_driver_mappings_component__WEBPACK_IMPORTED_MODULE_19__["ExternalDriverMappingsComponent"], _external_entity_mappings_external_carrier_mappings_external_carrier_mappings_component__WEBPACK_IMPORTED_MODULE_20__["ExternalCarrierMappingsComponent"], _external_entity_mappings_external_bulk_plant_mappings_external_bulk_plant_mappings_component__WEBPACK_IMPORTED_MODULE_21__["ExternalBulkPlantMappingsComponent"], _external_entity_mappings_external_vehicle_mappings_external_vehicle_mappings_component__WEBPACK_IMPORTED_MODULE_22__["ExternalVehicleMappingsComponent"]],
          imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_2__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_3__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_4__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeSelfService)]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/terminal-item-code-mapping/terminal-item-code-mapping.component.ts": function srcAppSelfServiceAliasTerminalItemCodeMappingTerminalItemCodeMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TerminalItemCodeMappingComponent", function () {
      return TerminalItemCodeMappingComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../create-terminal-item-code/create-terminal-item-code.component */
    "./src/app/self-service-alias/create-terminal-item-code/create-terminal-item-code.component.ts");
    /* harmony import */


    var src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/http-generic.service */
    "./src/app/http-generic.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    var _c0 = ["openSidePannel"];
    var _c1 = ["btnCloseBulkUploadPopup"];

    function TerminalItemCodeMappingComponent_tr_31_Template(rf, ctx) {
      if (rf & 1) {
        var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "td", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "button", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_tr_31_Template_button_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r5.editTerminalMapping(terminal_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "i", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "button", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function TerminalItemCodeMappingComponent_tr_31_Template_button_confirm_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r7.deleteTerminalItemCode(terminal_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "i", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var terminal_r4 = ctx.$implicit;

        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.TerminalSupplier);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.ItemDescription);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", terminal_r4.ItemCode, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.EffectiveDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.ExpiryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r1.popoverDeleteTitle)("confirmText", ctx_r1.confirmButtonText)("cancelText", ctx_r1.cancelButtonText);
      }
    }

    function TerminalItemCodeMappingComponent_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TerminalItemCodeMappingComponent_div_42_div_16_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TerminalItemCodeMappingComponent_div_42_Template(rf, ctx) {
      if (rf & 1) {
        var _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h4", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Item Code Mapping Bulk Upload");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "button", 43, 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_div_42_Template_button_click_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.closePopup();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "i", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "span", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_div_42_Template_a_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r12.downloadCsvTemplate();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Download Template");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "input", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function TerminalItemCodeMappingComponent_div_42_Template_input_change_15_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.selectFiles($event.target.files);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, TerminalItemCodeMappingComponent_div_42_div_16_Template, 2, 0, "div", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_div_42_Template_button_click_18_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r11);

          var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r14.uploadTerminalItemCodeMappingFile();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Upload");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.IsLoading);
      }
    }

    var TerminalItemCodeMappingComponent = /*#__PURE__*/function () {
      function TerminalItemCodeMappingComponent(httpService, carrierService) {
        _classCallCheck(this, TerminalItemCodeMappingComponent);

        this.httpService = httpService;
        this.carrierService = carrierService;
        this.title = 'Create';
        this.terminalMappingList = [];
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.IsValidForm = true;
        this.popoverSaveTitle = 'Save the change(s)?';
        this.popoverDeleteTitle = 'Are you sure, want to delete?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.IsShowBulkUploadPopup = false;
        this.SelectedFiles = [];
        this.MaxFileUploadSize = 1048576; // 1MB

        this.GetTerminalSupplierUrl = '/Carrier/SelfServiceAlias/GetTerminalItemCodeMappings';
        this.DeleteTerminalItemCodeMappingUrl = '/Carrier/SelfServiceAlias/DeleteTerminalItemCodeMapping';
      }

      _createClass(TerminalItemCodeMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [{
              extend: 'colvis'
            }, {
              extend: 'copy',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'csv',
              title: 'Terminal Item Code Mapping',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Terminal Item Code Mapping',
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
          this.getTerminalItemCodeMapping();
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.countryId && change.countryId.currentValue) {
            this.countryId = change.countryId.currentValue;

            if (change.countryId.previousValue && change.countryId.currentValue != change.countryId.previousValue) {
              this.getTerminalItemCodeMapping();
            }
          }
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#terminal-item-code-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          this.dtTrigger.next();
        }
      }, {
        key: "getTerminalItemCodeMapping",
        value: function getTerminalItemCodeMapping() {
          var _this53 = this;

          this.IsLoading = true;
          this.httpService.postData("".concat(this.GetTerminalSupplierUrl), {
            CountryId: this.countryId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["first"])()).subscribe(function (result) {
            _this53.IsLoading = false;
            _this53.terminalMappingList = result;

            _this53.datatableRerender();
          });
        }
      }, {
        key: "datatableRerender",
        value: function datatableRerender() {
          var _this54 = this;

          if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();

              _this54.dtTrigger.next();
            });
          }
        }
      }, {
        key: "editTerminalMapping",
        value: function editTerminalMapping(item) {
          this.title = 'Edit';
          this.createTerminalItemCodeComponent.terminalmappingForm.controls.id.setValue(item.Id);
          var obj = this.createTerminalItemCodeComponent.TerminalSupplierList.filter(function (f) {
            return f.Id == item.TerminalSupplierId;
          }).map(function (m) {
            delete m.Code;
            return m;
          });

          if (obj) {
            this.createTerminalItemCodeComponent.selectedTerminalSupplier = [];
            this.createTerminalItemCodeComponent.selectedTerminalSupplier.push(obj);
            this.createTerminalItemCodeComponent.terminalmappingForm.controls.terminalSupplierId.setValue(obj);
          }

          var obj1 = this.createTerminalItemCodeComponent.TerminalSupplierDescList.filter(function (f) {
            return f.Id == item.ItemDescriptionId;
          }).map(function (m) {
            delete m.Code;
            return m;
          });
          ;

          if (obj1) {
            this.createTerminalItemCodeComponent.selectedItemDesc = [];
            this.createTerminalItemCodeComponent.selectedItemDesc.push(obj1);
            this.createTerminalItemCodeComponent.terminalmappingForm.controls.itemDescriptionId.setValue(obj1);
          }

          this.createTerminalItemCodeComponent.terminalmappingForm.controls.effectiveDate.setValue(item.EffectiveDate);
          this.createTerminalItemCodeComponent.terminalmappingForm.controls.expiryDate.setValue(item.ExpiryDate);
          this.createTerminalItemCodeComponent.terminalmappingForm.controls.itemCode.setValue(item.ItemCode);
        }
      }, {
        key: "addTerminalItemCode",
        value: function addTerminalItemCode() {
          this.title = 'Create';
          this.createTerminalItemCodeComponent.terminalmappingForm.reset();
        }
      }, {
        key: "getOutput",
        value: function getOutput($event) {
          if ($event) {
            this.openSidePannel.nativeElement.click();
            this.getTerminalItemCodeMapping();
          }
        }
      }, {
        key: "deleteTerminalItemCode",
        value: function deleteTerminalItemCode(item) {
          var _this55 = this;

          this.IsLoading = true;
          this.httpService.postData("".concat(this.DeleteTerminalItemCodeMappingUrl), {
            id: item.Id
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["first"])()).subscribe(function (result) {
            _this55.IsLoading = false;

            if (result.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(result.StatusMessage, undefined, undefined);

              _this55.getTerminalItemCodeMapping();
            } else src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(result.StatusMessage, undefined, undefined);
          });
        }
      }, {
        key: "showBulkUploadPopup",
        value: function showBulkUploadPopup() {
          this.IsShowBulkUploadPopup = true;
        }
      }, {
        key: "closePopup",
        value: function closePopup() {
          this.IsShowBulkUploadPopup = false;
        }
      }, {
        key: "selectFiles",
        value: function selectFiles(files) {
          if (files != null && files != undefined && files.length > 0) this.SelectedFiles = files;
        }
      }, {
        key: "uploadTerminalItemCodeMappingFile",
        value: function uploadTerminalItemCodeMappingFile() {
          var _this56 = this;

          var files = this.SelectedFiles;
          if (files.length === 0) return;
          var formData = new FormData();

          var _iterator2 = _createForOfIteratorHelper(files),
              _step2;

          try {
            for (_iterator2.s(); !(_step2 = _iterator2.n()).done;) {
              var file = _step2.value;

              if (!this.isValidFile(file)) {
                return;
              }

              formData.append(file.name, file);
            }
          } catch (err) {
            _iterator2.e(err);
          } finally {
            _iterator2.f();
          }

          this.IsLoading = true;
          this.carrierService.postBulkUploadTerminalItemCodeMappingTemplate(formData).subscribe(function (data) {
            _this56.IsLoading = false;

            if (data.StatusCode == 0) {
              _this56.CloseBulkUploadPopup.nativeElement.click();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
              _this56.SelectedFiles = [];
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "isValidFile",
        value: function isValidFile(file) {
          var isValid = true;
          var extension = this.getExtension(file.name);

          if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
          }

          if (file.size > this.MaxFileUploadSize) {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
          }

          return isValid;
        }
      }, {
        key: "downloadCsvTemplate",
        value: function downloadCsvTemplate() {
          var _this57 = this;

          this.IsLoading = true;
          var timestamp = new Date().getTime();
          this.carrierService.downloadTerminalItemCodeMappingTemplate(timestamp).subscribe(function (blob) {
            var a = document.createElement('a');
            var objectUrl = URL.createObjectURL(blob);
            a.href = objectUrl;
            a.download = 'TerminalItemCodeMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);
            _this57.IsLoading = false;
          });
        }
      }, {
        key: "getExtension",
        value: function getExtension(fileName) {
          // extract file name from full path ...
          var basename = fileName.split(/[\\/]/).pop(); // (supports `\\` and `/` separators)

          var pos = basename.lastIndexOf("."); // get last position of `.`

          if (basename === "" || pos < 1) // if file name is empty or ...
            return ""; //  `.` not found (-1) or comes first (0)

          return basename.slice(pos + 1); // extract extension ignoring `.`
        }
      }]);

      return TerminalItemCodeMappingComponent;
    }();

    TerminalItemCodeMappingComponent.ɵfac = function TerminalItemCodeMappingComponent_Factory(t) {
      return new (t || TerminalItemCodeMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_6__["HttpGenericService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_7__["CarrierService"]));
    };

    TerminalItemCodeMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: TerminalItemCodeMappingComponent,
      selectors: [["app-terminal-item-code-mapping"]],
      viewQuery: function TerminalItemCodeMappingComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c1, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_5__["CreateTerminalItemCodeComponent"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.openSidePannel = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.CloseBulkUploadPopup = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.createTerminalItemCodeComponent = _t.first);
        }
      },
      inputs: {
        countryId: "countryId"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 43,
      vars: 7,
      consts: [[1, "row"], [1, "col-sm-6"], ["type", "button", "data-target", "raisedr", "onclick", "slidePanel('#terminal_i_code_map','45%')", 1, "btn", "btn-link", "float-left", "pa0", "mt10", 3, "click"], ["openSidePannel", ""], [1, "fas", "fa-plus-circle", "fs18", "mr5"], ["id", "showBulkUploadPopupBtn", "type", "button", "data-toggle", "modal", "data-target", "#bulkUploadTerminalModalPopup", 1, "btn", "btn-primary", "float-right", "mb5", "valid", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-terminal-item-code", 1, "table-responsive"], ["datatable", "", 1, "table", "table-bordered", 3, "dtOptions", "dtTrigger"], ["data-key", "tSName"], ["data-key", "tItem"], ["data-key", "tItemCode"], ["data-key", "tEffectiveDate"], ["data-key", "tExpiryDate"], ["data-key", "Action"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["id", "terminal_i_code_map", 1, "side-panel"], [1, "side-panel-wrapper"], [1, "pt10", "pb0"], ["onclick", "closeSlidePanel();", 1, "ml20", "close-panel"], [1, "fa", "fa-close", "fs18"], [1, "dib", "mt0", "mb0", "ml15"], [1, "pt10", "pb10", "pl20", "pr20"], [3, "countryId", "result"], ["id", "bulkUploadTerminalModalPopup", "class", "modal fade", "role", "dialog", 4, "ngIf"], [1, "text-center"], ["type", "button", "onclick", "slidePanel('#terminal_i_code_map','40%')", 1, "btn", "btn-link", 3, "click"], ["alt", "Update", "title", "Update", 1, "fas", "fa-pencil-square-o", "color-blue", "fs16"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"], ["id", "bulkUploadTerminalModalPopup", "role", "dialog", 1, "modal", "fade"], [1, "modal-dialog"], [1, "modal-content"], [1, "modal-header", "pt0", "pb5"], [1, "modal-title"], ["type", "button", "data-dismiss", "modal", "aria-label", "Close", 1, "close", "color-grey", 3, "click"], ["btnCloseBulkUploadPopup", ""], [1, "fa", "fa-close", "fs21", "mt15"], [1, "modal-body"], [1, "mb10"], [1, "fa", "fa-download", "mr10", "mt10"], ["role", "button", 1, "mb5", "btn-download", 3, "click"], [1, "mb5", "mt5"], ["type", "file", "id", "bulkUploadFile", "accept", ".csv", 3, "change"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper", 4, "ngIf"], [1, "modal-footer"], ["type", "button", 1, "btn", "btn-default", 3, "click"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper"], [1, "spinner-dashboard", "pa"]],
      template: function TerminalItemCodeMappingComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "button", 2, 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_Template_button_click_2_listener() {
            return ctx.addTerminalItemCode();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, " Create Terminal Item Code ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "button", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalItemCodeMappingComponent_Template_button_click_7_listener() {
            return ctx.showBulkUploadPopup();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Bulk Upload");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "table", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "th", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Terminal Supplier");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "th", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Terminal Item");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "th", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "Terminal Item Code");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "th", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Effective Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "th", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Expiry Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "th", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](31, TerminalItemCodeMappingComponent_tr_31_Template, 16, 8, "tr", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](32, TerminalItemCodeMappingComponent_div_32_Template, 5, 0, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "a", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](37, "i", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "h3", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "app-create-terminal-item-code", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("result", function TerminalItemCodeMappingComponent_Template_app_create_terminal_item_code_result_41_listener($event) {
            return ctx.getOutput($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](42, TerminalItemCodeMappingComponent_div_42_Template, 20, 1, "div", 28);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.terminalMappingList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", ctx.title, " Terminal Item Code Mapping");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("countryId", ctx.countryId);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsShowBulkUploadPopup);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_5__["CreateTerminalItemCodeComponent"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_9__["ɵc"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy90ZXJtaW5hbC1pdGVtLWNvZGUtbWFwcGluZy90ZXJtaW5hbC1pdGVtLWNvZGUtbWFwcGluZy5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TerminalItemCodeMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-terminal-item-code-mapping',
          templateUrl: './terminal-item-code-mapping.component.html',
          styleUrls: ['./terminal-item-code-mapping.component.css']
        }]
      }], function () {
        return [{
          type: src_app_http_generic_service__WEBPACK_IMPORTED_MODULE_6__["HttpGenericService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_7__["CarrierService"]
        }];
      }, {
        countryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"]]
        }],
        openSidePannel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['openSidePannel']
        }],
        CloseBulkUploadPopup: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['btnCloseBulkUploadPopup']
        }],
        createTerminalItemCodeComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [_create_terminal_item_code_create_terminal_item_code_component__WEBPACK_IMPORTED_MODULE_5__["CreateTerminalItemCodeComponent"]]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/self-service-alias/terminal-mapping/terminal-mapping.component.ts": function srcAppSelfServiceAliasTerminalMappingTerminalMappingComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TerminalMappingComponent", function () {
      return TerminalMappingComponent;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var src_app_carrier_models_location__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/carrier/models/location */
    "./src/app/carrier/models/location.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
    /* harmony import */


    var src_app_location_services_location_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/location/services/location.service */
    "./src/app/location/services/location.service.ts");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");

    function TerminalMappingComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function TerminalMappingComponent_tr_61_Template(rf, ctx) {
      if (rf & 1) {
        var _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "td", 31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function TerminalMappingComponent_tr_61_Template_td_blur_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r5.editTerminalId(terminal_r4, "AssignedTerminalId", $event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "button", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function TerminalMappingComponent_tr_61_Template_button_confirm_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r7.updateTerminalId(terminal_r4);
        })("cancel", function TerminalMappingComponent_tr_61_Template_button_cancel_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.cancelUpdateTerminalNames(terminal_r4);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 34);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "button", 35);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function TerminalMappingComponent_tr_61_Template_button_confirm_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r6);

          var terminal_r4 = ctx.$implicit;

          var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r9.deleteMapping(terminal_r4.Id, terminal_r4.CreatedByCompanyId);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "i", 36);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var terminal_r4 = ctx.$implicit;

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.TerminalName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](terminal_r4.ControlNumber == "-" ? "--" : terminal_r4.ControlNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", terminal_r4.TerminalSupplierName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", terminal_r4.AssignedTerminalId == "" ? "--" : terminal_r4.AssignedTerminalId, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r2.popoverSaveTitle)("confirmText", ctx_r2.confirmButtonText)("cancelText", ctx_r2.cancelButtonText);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r2.popoverDeleteTitle)("confirmText", ctx_r2.confirmButtonText)("cancelText", ctx_r2.cancelButtonText);
      }
    }

    function TerminalMappingComponent_div_62_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 39);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 40);

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

    var TerminalMappingComponent = /*#__PURE__*/function () {
      function TerminalMappingComponent(fb, carrierService, locationService) {
        _classCallCheck(this, TerminalMappingComponent);

        this.fb = fb;
        this.carrierService = carrierService;
        this.locationService = locationService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.selected = 0;
        this.TerminalsList = [];
        this.StateList = [];
        this.CityList = [];
        this.TerminalList = [];
        this.SelectedTerminalId = [];
        this.ddlSettingsById = {};
        this.ddlSettingsByIdSingleSelect = {};
        this.ddlSettingsByCode = {};
        this.ddlSettingsForTerminal = {};
        this.IsValidForm = true;
        this.existingAssignedTerminalId = '';
        this.nameToUpdate = '';
        this.existingId = 0;
        this.popoverSaveTitle = 'Save the change(s)?';
        this.popoverDeleteTitle = 'Are you sure, want to delete?';
        this.confirmButtonText = 'Yes';
        this.cancelButtonText = 'No';
        this.IsLiftFileValidationEnabled = false;
        this.TerminalMappingForm = this.fb.group({
          States: this.fb.control([], [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          Cities: this.fb.control([]),
          SelectedTerminal: this.fb.control([], [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          MyTerminalId: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
          CompanyId: this.fb.control(0),
          Terminals: this.fb.control(0),
          SelectedTerminalSupplier: this.fb.control([])
        });
      }

      _createClass(TerminalMappingComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.ddlSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.ddlSettingsForTerminal = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.ddlSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.ddlSettingsByIdSingleSelect = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.initializeUserTerminalData(); // this.loadData();

          this.CurrentCompanyId = Number(currentCompanyId);
          this.IsLiftFileValidationEnabled = IsLiftFileValidationEnabled;
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges() {
          this.clearForm();
        }
      }, {
        key: "loadData",
        value: function loadData() {
          this.SelectedCountryId = Number(this.countryId);

          if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
            this.getDefaultServingCountry();
          } else {
            this.getStates();
            this.getAllUserTerminalData();
          }
        }
      }, {
        key: "getDefaultServingCountry",
        value: function getDefaultServingCountry() {
          var _this58 = this;

          this.IsLoading = true;
          this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(function (data) {
            _this58.IsLoading = false;
            _this58.SelectedCountryId = Number(data.DefaultCountryId);

            _this58.getStates();

            _this58.getAllUserTerminalData();
          });
        }
      }, {
        key: "filterGrid",
        value: function filterGrid() {
          $("#terminal-grid-datatable").DataTable().clear().destroy();
        }
      }, {
        key: "onStateSelect",
        value: function onStateSelect(state) {
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "onStateDeSelect",
        value: function onStateDeSelect(state) {
          this.TerminalMappingForm.get('Cities').patchValue([]);
          this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
          this.TerminalMappingForm.get('SelectedTerminalSupplier').patchValue([]);
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "onStateSelectAll",
        value: function onStateSelectAll(states) {
          this.TerminalMappingForm.get('States').patchValue(states);
          this.getCitiesByStateId();
          this.getTerminals();
        }
      }, {
        key: "getTerminals",
        value: function getTerminals() {
          var _this59 = this;

          var selectedStates = this.TerminalMappingForm.get('States').value;
          var selectedCities = this.TerminalMappingForm.get('Cities').value;

          if (selectedStates.length == 0) {
            this.TerminalMappingForm.get('States').patchValue([]);
            this.TerminalList = [];
            this.TerminalSupplierList = [];
            return;
          }

          var input = new src_app_carrier_models_location__WEBPACK_IMPORTED_MODULE_4__["TerminalMappingModel"]();

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (s) {
              return s.Id;
            });
            input.StateIds = stateIds.join(',');
          }

          if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(function (s) {
              return s.Name;
            });
            input.CityIds = cityIds.join(',');
          }

          input.CountryId = this.SelectedCountryId;
          this.IsLoading = true;
          this.carrierService.getTerminalsForMapping(input).subscribe(function (data) {
            _this59.IsLoading = false;
            _this59.TerminalList = data;
          });
          this.getTerminalSuppliers();
        }
      }, {
        key: "onStateDeSelectAll",
        value: function onStateDeSelectAll() {
          this.TerminalMappingForm.get('Cities').patchValue([]);
          this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
          this.TerminalMappingForm.get('SelectedTerminalSupplier').patchValue([]);
          this.CityList = [];
          this.TerminalList = [];
          this.TerminalSupplierList = [];
        }
      }, {
        key: "getCitiesByStateId",
        value: function getCitiesByStateId() {
          var selectedStates = this.TerminalMappingForm.get('States').value;

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (m) {
              return m.Id;
            });
            this.getCities(stateIds);
          } else {
            this.CityList = [];
          }
        }
      }, {
        key: "getCountryFilter",
        value: function getCountryFilter() {
          return localStorage.getItem('countryFilterType') ? localStorage.getItem('countryFilterType') : localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1;
        }
      }, {
        key: "getStates",
        value: function getStates() {
          var _this60 = this;

          this.IsLoading = true;

          if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId).subscribe(function (data) {
              _this60.IsLoading = false;
              _this60.StateList = data;
            });
          }
        }
      }, {
        key: "getCities",
        value: function getCities(stateId) {
          var _this61 = this;

          this.IsLoading = true;
          this.carrierService.getCities(stateId).subscribe(function (data) {
            _this61.IsLoading = false;
            _this61.CityList = data;
          });
        }
      }, {
        key: "onCitySelect",
        value: function onCitySelect(city) {
          this.getTerminals();
        }
      }, {
        key: "onCityDeSelect",
        value: function onCityDeSelect(city) {
          this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
          this.getTerminals();
        }
      }, {
        key: "onCitySelectAll",
        value: function onCitySelectAll(cities) {
          this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
          this.TerminalMappingForm.get('Cities').patchValue(cities);
          this.getTerminals();
        }
      }, {
        key: "onCityDeSelectAll",
        value: function onCityDeSelectAll() {
          this.TerminalMappingForm.get('SelectedTerminal').setValue([]);
          this.getTerminals();
        }
      }, {
        key: "clearFilter",
        value: function clearFilter() {
          this.clearForm();
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.TerminalMappingForm.reset();
          this.TerminalMappingForm.get('States').setValue([]);
          this.TerminalMappingForm.get('Cities').setValue([]);
          this.TerminalMappingForm.get('SelectedTerminal').setValue([]);
          this.TerminalMappingForm.get('SelectedTerminalSupplier').setValue([]);
          this.existingAssignedTerminalId = '';
          this.nameToUpdate = '';
          this.TerminalList = [];
          this.CityList = [];
          this.TerminalSupplierList = [];
          $("#terminal-grid-datatable").DataTable().clear().destroy();
          this.loadData();
        }
      }, {
        key: "selectchange",
        value: function selectchange(args) {
          this.SelectedTerminalId = args.target.value;
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var TermSupplierId = 0;
          var TerminalId = 0;
          var AssignedTerminalId = this.TerminalMappingForm.get('MyTerminalId').value;
          var objTerminalId = this.TerminalMappingForm.get('SelectedTerminal').value;
          var SelectedTermSupplier = this.TerminalMappingForm.get('SelectedTerminalSupplier').value;

          if (SelectedTermSupplier != null && SelectedTermSupplier.length > 0) {
            TermSupplierId = SelectedTermSupplier[0].Id;
          }

          if (objTerminalId.length) {
            TerminalId = objTerminalId[0].Id;
          }

          if (AssignedTerminalId == undefined || AssignedTerminalId == null || AssignedTerminalId.trim() == '' || TerminalId == 0) {
            if (TerminalId == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Please provide Terminal Name', undefined, undefined);
              return;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Please provide Terminal ID', undefined, undefined);
              return;
            }
          } else {
            this.IsValidForm = true;
          }

          var TerminalMappingViewModel = {
            Id: 0,
            AssignedTerminalId: AssignedTerminalId,
            TerminalId: TerminalId,
            IsBulkPlant: this.TerminalList.find(function (f) {
              return f.Id == TerminalId;
            }).Code == '1' ? true : false,
            TerminalSupplierId: TermSupplierId
          };
          this.TerminalMappingForm.get("CompanyId").patchValue(this.CurrentCompanyId);

          if (!this.IsValidForm) {
            this.IsValidForm = false;
          } else {
            this.checkDuplicateTerminalId(TerminalMappingViewModel);
          }
        }
      }, {
        key: "submitForm",
        value: function submitForm(TerminalMappingViewModel) {
          var _this62 = this;

          this.IsLoading = true;
          this.carrierService.saveTerminalMapping(TerminalMappingViewModel).subscribe(function (data) {
            if (data.StatusCode == 0) {
              _this62.IsLoading = false;

              _this62.clearForm();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "editTerminalId",
        value: function editTerminalId(mapping, key, $event) {
          this.nameToUpdate = $event.target.innerText;
          this.existingAssignedTerminalId = mapping.AssignedTerminalId;
          this.existingId = mapping.Id;
        }
      }, {
        key: "updateTerminalId",
        value: function updateTerminalId(mapping) {
          if (mapping.Id == this.existingId) {
            mapping.AssignedTerminalId = this.nameToUpdate;
          }

          if (mapping.AssignedTerminalId.trim() == '') {
            src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror('Please provide My Terminal ID', undefined, undefined);
            return;
          }

          this.checkDuplicateTerminalId(mapping);
        }
      }, {
        key: "updateTerminal",
        value: function updateTerminal(mapping) {
          var _this63 = this;

          this.IsLoading = true;
          this.carrierService.updateTerminalId(mapping).subscribe(function (data) {
            _this63.IsLoading = false;

            if (data.StatusCode == 0) {
              _this63.clearForm();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "checkDuplicateTerminalId",
        value: function checkDuplicateTerminalId(TerminalMappingViewModel) {
          var _this64 = this;

          this.carrierService.checkDuplicateTerminalId(TerminalMappingViewModel).subscribe(function (data) {
            if (data.StatusCode == 0) {
              if (TerminalMappingViewModel.Id == 0) {
                _this64.submitForm(TerminalMappingViewModel);
              } else {
                _this64.updateTerminal(TerminalMappingViewModel);
              }
            }

            if (data.StatusCode == 2) {
              _this64.existingAssignedTerminalId = '';
              _this64.nameToUpdate = '';
              $("#terminal-grid-datatable").DataTable().clear().destroy();

              _this64.getAllUserTerminalData();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "deleteMapping",
        value: function deleteMapping(mappingId, companyId) {
          var _this65 = this;

          if (mappingId == undefined || mappingId <= 0) return;
          var model = new src_app_carrier_models_location__WEBPACK_IMPORTED_MODULE_4__["LocationDetailsModel"]();
          model.Id = mappingId;
          this.IsLoading = true;
          this.carrierService.postDeleteTerminalMappingById(model).subscribe(function (data) {
            _this65.IsLoading = false;

            if (data.StatusCode == 0) {
              _this65.clearForm();

              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess(data.StatusMessage, undefined, undefined);
            } else if (data.StatusCode == 2) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgwarning(data.StatusMessage, undefined, undefined);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "cancelUpdateTerminalNames",
        value: function cancelUpdateTerminalNames(mapping) {
          //mapping.AssignedTerminalId = this.existingAssignedTerminalId;
          this.clearForm();
        }
      }, {
        key: "initializeUserTerminalData",
        value: function initializeUserTerminalData() {
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
              title: 'Terminal Details',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Terminal Details',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            aaSorting: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "getAllUserTerminalData",
        value: function getAllUserTerminalData() {
          var _this66 = this;

          this.IsLoading = true;
          var SelectedCountryId = this.SelectedCountryId;
          this.carrierService.getTerminalMappingGrid(SelectedCountryId).subscribe(function (data) {
            _this66.IsLoading = false;
            _this66.TerminalsList = data;

            _this66.dtTrigger.next();
          });
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {//  this.dtTrigger.next();
        }
      }, {
        key: "getTerminalSuppliers",
        value: function getTerminalSuppliers() {
          var _this67 = this;

          this.IsLoading = true;
          var selectedCountryId = this.SelectedCountryId;
          this.carrierService.getTerminalSupplier(selectedCountryId).subscribe(function (data) {
            _this67.IsLoading = false;
            _this67.TerminalSupplierList = data;
          });
        }
      }]);

      return TerminalMappingComponent;
    }();

    TerminalMappingComponent.ɵfac = function TerminalMappingComponent_Factory(t) {
      return new (t || TerminalMappingComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_location_services_location_service__WEBPACK_IMPORTED_MODULE_6__["LocationService"]));
    };

    TerminalMappingComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: TerminalMappingComponent,
      selectors: [["app-terminal-mapping"]],
      inputs: {
        countryId: "countryId"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 63,
      vars: 24,
      consts: [[3, "formGroup", "ngSubmit"], [1, "col-sm-12"], [1, "row"], [1, "well", "col-sm-12"], [1, "col"], [1, "form-group"], ["formControlName", "States", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], ["formControlName", "Cities", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], [1, "color-maroon"], ["formControlName", "SelectedTerminal", 3, "placeholder", "settings", "data"], ["class", "color-maroon", 4, "ngIf"], [1, "form-group", 3, "ngClass"], ["formControlName", "SelectedTerminalSupplier", 3, "placeholder", "settings", "data"], ["type", "text", "formControlName", "MyTerminalId", 1, "form-control"], ["MyTerminalId", ""], [1, "d-flex", "flex-row-reverse"], ["id", "submit-terminal-mapping-form", "type", "submit", "aria-invalid", "false", 1, "mt4", "btn", "btn-default", "valid", 3, "ngClass"], ["id", "clear-filter", "type", "button", 1, "btn", "mt3", "valid", 3, "click"], [1, "col-md-12"], [1, "well", "bg-white", "shadow-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-terminal-grid", 1, "table-responsive"], ["id", "terminal-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "tName"], ["data-key", "tControlNumber"], ["data-key", "AssignedTermSupplierName"], ["data-key", "tAssignedTerminalId"], ["data-key", "Action"], [4, "ngFor", "ngForOf"], ["class", "loader", 4, "ngIf"], ["contenteditable", "true", 1, "edit-td", 3, "blur"], [1, "text-center"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm", "cancel"], ["alt", "Update", "title", "Update", 1, "fs21", "fas", "fa-save", "color-green"], ["type", "button", "mwlConfirmationPopover", "", "placement", "bottom", 1, "btn", "btn-link", 3, "popoverTitle", "confirmText", "cancelText", "confirm"], ["alt", "Delete", "title", "Delete", 1, "fas", "fa-trash-alt", "color-maroon"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function TerminalMappingComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function TerminalMappingComponent_Template_form_ngSubmit_0_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "ng-multiselect-dropdown", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onSelect_9_listener($event) {
            return ctx.onStateSelect($event);
          })("onDeSelect", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onDeSelect_9_listener($event) {
            return ctx.onStateDeSelect($event);
          })("onSelectAll", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onSelectAll_9_listener($event) {
            return ctx.onStateSelectAll($event);
          })("onDeSelectAll", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onDeSelectAll_9_listener() {
            return ctx.onStateDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "ng-multiselect-dropdown", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onSelect_14_listener($event) {
            return ctx.onCitySelect($event);
          })("onDeSelect", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onDeSelect_14_listener($event) {
            return ctx.onCityDeSelect($event);
          })("onSelectAll", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onSelectAll_14_listener($event) {
            return ctx.onCitySelectAll($event);
          })("onDeSelectAll", function TerminalMappingComponent_Template_ng_multiselect_dropdown_onDeSelectAll_14_listener() {
            return ctx.onCityDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Terminal/Bulk Plant Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "span", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](21, "ng-multiselect-dropdown", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, TerminalMappingComponent_div_22_Template, 2, 0, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Terminal Suppliers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](27, "ng-multiselect-dropdown", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Terminal ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "span", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](33, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](34, "input", 13, 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "button", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "Assign");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "button", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TerminalMappingComponent_Template_button_click_39_listener() {
            return ctx.clearFilter();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Clear Filter");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "table", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "th", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](51, "Terminal/Bulk Plant Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "th", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "Control Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "th", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](55, "Terminal Supplier Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "th", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Terminal Id");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "th", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](59, "Action");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](61, TerminalMappingComponent_tr_61_Template, 14, 10, "tr", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](62, TerminalMappingComponent_div_62_Template, 5, 0, "div", 30);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.TerminalMappingForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select State(s)")("settings", ctx.ddlSettingsById)("data", ctx.StateList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select City(s)")("settings", ctx.ddlSettingsByCode)("data", ctx.CityList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal(s)")("settings", ctx.ddlSettingsForTerminal)("data", ctx.TerminalList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsValidForm && (ctx.TerminalMappingForm.get("SelectedTerminal").errors == null ? null : ctx.TerminalMappingForm.get("SelectedTerminal").errors.required));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](20, _c0, !ctx.IsLiftFileValidationEnabled));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal Suppliers")("settings", ctx.ddlSettingsForTerminal)("data", ctx.TerminalSupplierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](22, _c0, ctx.IsLoading));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.TerminalsList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_7__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], angular_datatables__WEBPACK_IMPORTED_MODULE_9__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_8__["NgForOf"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_10__["ɵc"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3NlbGYtc2VydmljZS1hbGlhcy90ZXJtaW5hbC1tYXBwaW5nL3Rlcm1pbmFsLW1hcHBpbmcuY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TerminalMappingComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-terminal-mapping',
          templateUrl: './terminal-mapping.component.html',
          styleUrls: ['./terminal-mapping.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]
        }, {
          type: src_app_location_services_location_service__WEBPACK_IMPORTED_MODULE_6__["LocationService"]
        }];
      }, {
        countryId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=self-service-alias-self-service-alias-module-es5.js.map