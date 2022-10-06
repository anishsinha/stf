function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["sales-user-sales-user-module"], {
  /***/
  "./src/app/contact-person/contact-person.component.ts": function srcAppContactPersonContactPersonComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ContactPersonComponent", function () {
      return ContactPersonComponent;
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


    var _sales_user_sales_user_model__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../sales-user/sales-user.model */
    "./src/app/sales-user/sales-user.model.ts");
    /* harmony import */


    var _sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../sales-user/sales-user.service */
    "./src/app/sales-user/sales-user.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function ContactPersonComponent_div_0_ng_container_2_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Unable to verify number! You will miss Text Alerts.");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_12_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Mobile Number is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, ContactPersonComponent_div_0_ng_container_2_div_12_span_1_Template, 2, 0, "span", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var contactperson_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.controls["PhoneNumber"].errors == null ? null : contactperson_r2.controls["PhoneNumber"].errors.required);
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Please, Enter 10 digit Mobile Number.");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_17_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Email is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_17_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, ContactPersonComponent_div_0_ng_container_2_div_17_span_1_Template, 2, 0, "span", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ContactPersonComponent_div_0_ng_container_2_div_17_span_2_Template, 2, 0, "span", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var contactperson_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.get("Email").errors == null ? null : contactperson_r2.get("Email").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.get("Email").errors == null ? null : contactperson_r2.get("Email").errors.email);
      }
    }

    function ContactPersonComponent_div_0_ng_container_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "input", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function ContactPersonComponent_div_0_ng_container_2_Template_input_blur_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r14);

          var contactperson_r2 = ctx.$implicit;

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r13.onChangeMobileNumber($event, contactperson_r2);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, ContactPersonComponent_div_0_ng_container_2_div_11_Template, 2, 0, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, ContactPersonComponent_div_0_ng_container_2_div_12_Template, 2, 1, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, ContactPersonComponent_div_0_ng_container_2_div_13_Template, 2, 0, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](16, "input", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, ContactPersonComponent_div_0_ng_container_2_div_17_Template, 3, 2, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ContactPersonComponent_div_0_ng_container_2_Template_a_click_19_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r14);

          var i_r3 = ctx.index;

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r15.removeContactPerson(i_r3);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var contactperson_r2 = ctx.$implicit;
        var i_r3 = ctx.index;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "CustomerDetails_Name_", i_r3, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "CustomerDetails_PhoneNumber_", i_r3, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !contactperson_r2.controls["PhoneNumber"].errors && contactperson_r2.controls["IsPhoneNumberConfirmed"].value == false);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.controls["PhoneNumber"].errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.controls["PhoneNumber"].errors == null ? null : contactperson_r2.controls["PhoneNumber"].errors.pattern);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate1"]("id", "CustomerDetails_Email_", i_r3, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", contactperson_r2.get("Email").errors);
      }
    }

    function ContactPersonComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1, 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, ContactPersonComponent_div_0_ng_container_2_Template, 20, 8, "ng-container", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx_r0.Parent);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.Parent.get("ContactPersons")["controls"]);
      }
    }

    var ContactPersonComponent = /*#__PURE__*/function () {
      function ContactPersonComponent(fb, salesUserService) {
        _classCallCheck(this, ContactPersonComponent);

        this.fb = fb;
        this.salesUserService = salesUserService;
        this.regexPhone = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
      }

      _createClass(ContactPersonComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {//console.log(change);
        }
      }, {
        key: "onChangeMobileNumber",
        value: function onChangeMobileNumber(event, contactPerson) {
          if (contactPerson.get('PhoneNumber').value) {
            this.salesUserService.IsPhoneNumberValid(contactPerson.get('PhoneNumber').value).subscribe(function (data) {
              contactPerson.get('IsPhoneNumberConfirmed').setValue(data);
            });
          } else {
            contactPerson.get('IsPhoneNumberConfirmed').setValue(true);
          }
        }
      }, {
        key: "getNewContactPerson",
        value: function getNewContactPerson() {
          var _contactPersonForm = this.fb.group({
            Name: this.fb.control(null),
            PhoneNumber: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(this.regexPhone)]),
            Email: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].email]),
            IsPhoneNumberConfirmed: this.fb.control(true)
          });

          return _contactPersonForm;
        }
      }, {
        key: "removeContactPerson",
        value: function removeContactPerson(idx) {
          this.Parent.get('ContactPersons').removeAt(idx);
        }
      }, {
        key: "addContactPerson",
        value: function addContactPerson() {
          var contactPerson = new _sales_user_sales_user_model__WEBPACK_IMPORTED_MODULE_2__["ContactPersonModel"]();
          this.Parent.get('ContactPersons').push(this.getNewContactPerson());
        }
      }]);

      return ContactPersonComponent;
    }();

    ContactPersonComponent.ɵfac = function ContactPersonComponent_Factory(t) {
      return new (t || ContactPersonComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_3__["SalesUserService"]));
    };

    ContactPersonComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: ContactPersonComponent,
      selectors: [["app-contact-person"]],
      inputs: {
        Parent: "Parent"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 4,
      vars: 1,
      consts: [[3, "formGroup", 4, "ngIf"], ["type", "button", 1, "btn", "btn-link", 3, "click"], [1, "fa", "fa-plus-circle"], [3, "formGroup"], ["formArrayName", "ContactPersons"], [4, "ngFor", "ngForOf"], [1, "row"], [1, "col-sm-12"], [1, "row", 3, "formGroupName"], [1, "col-sm-3", "new-company"], [1, "form-group"], ["formControlName", "Name", "placeholder", "Contact Person", "type", "text", "value", "", 1, "form-control", "newContactPerson", 3, "id"], [1, "col-sm-3"], ["placeholder", "Mobile Number", "name", "PhoneNumber", "formControlName", "PhoneNumber", "type", "text", "value", "", 1, "form-control", "input-phoneformat", "phoneNumber", 3, "id", "blur"], ["class", "color-orange fs12 pt5", "id", "mobile-validation-msg", 4, "ngIf"], [4, "ngIf"], ["class", "text-danger", 4, "ngIf"], [1, "form-group", "custEmail"], ["placeholder", "Email", "name", "Email", "formControlName", "Email", "type", "email", "value", "", 1, "form-control", "email", 3, "id"], [1, "col-sm-1"], [1, "fa", "fa-trash-alt", "ml10", "color-maroon", "mt10", 3, "click"], ["id", "mobile-validation-msg", 1, "color-orange", "fs12", "pt5"], [1, "text-danger"]],
      template: function ContactPersonComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](0, ContactPersonComponent_div_0_Template, 3, 2, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ContactPersonComponent_Template_button_click_1_listener() {
            return ctx.addContactPerson();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, " Add Contact Person");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.Parent.get("ContactPersons")["controls"].length > 0);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbnRhY3QtcGVyc29uL2NvbnRhY3QtcGVyc29uLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ContactPersonComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-contact-person',
          templateUrl: './contact-person.component.html',
          styleUrls: ['./contact-person.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _sales_user_sales_user_service__WEBPACK_IMPORTED_MODULE_3__["SalesUserService"]
        }];
      }, {
        Parent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/sales-user/create-sourcing-request/create-sourcing-request.component.ts": function srcAppSalesUserCreateSourcingRequestCreateSourcingRequestComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CreateSourcingRequestComponent", function () {
      return CreateSourcingRequestComponent;
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


    var moment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_3__);
    /* harmony import */


    var src_app_shared_components_pricing_section_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/shared-components/pricing-section/pricing-section.component */
    "./src/app/shared-components/pricing-section/pricing-section.component.ts");
    /* harmony import */


    var _sales_user_model__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../sales-user.model */
    "./src/app/sales-user/sales-user.model.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _sales_user_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../sales-user.service */
    "./src/app/sales-user/sales-user.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../../shared-components/confirmation-dialog/confirmation-dialog.service */
    "./src/app/shared-components/confirmation-dialog/confirmation-dialog.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _directives_disable_control_directive__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ../../directives/disable-control.directive */
    "./src/app/directives/disable-control.directive.ts");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var _fees_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ../../fees/fee-list/fee-list.component */
    "./src/app/fees/fee-list/fee-list.component.ts");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var src_app_contact_person_contact_person_component__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(
    /*! src/app/contact-person/contact-person.component */
    "./src/app/contact-person/contact-person.component.ts");

    var _c0 = ["approveTerminalAuto"];

    function CreateSourcingRequestComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 207);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 208);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 209);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_span_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " New ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_span_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " WIP ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_span_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Sourced ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_span_14_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Lost ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_input_19_Template(rf, ctx) {
      if (rf & 1) {
        var _r54 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 210);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function CreateSourcingRequestComponent_input_19_Template_input_confirm_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r54);

          var ctx_r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r53.createPO();
        })("cancel", function CreateSourcingRequestComponent_input_19_Template_input_cancel_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r54);

          var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r55.cancelClicked = true;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r5.popoverTitle)("popoverMessage", ctx_r5.popoverMessage);
      }
    }

    function CreateSourcingRequestComponent_input_20_Template(rf, ctx) {
      if (rf & 1) {
        var _r57 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 211);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_input_20_Template_input_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r57);

          var ctx_r56 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r56.acceptRequest();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_input_21_Template(rf, ctx) {
      if (rf & 1) {
        var _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "input", 212);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("confirm", function CreateSourcingRequestComponent_input_21_Template_input_confirm_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r59);

          var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r58.loseRequest();
        })("cancel", function CreateSourcingRequestComponent_input_21_Template_input_cancel_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r59);

          var ctx_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r60.cancelClicked = true;
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("popoverTitle", ctx_r7.popoverLostTitle)("popoverMessage", ctx_r7.popoverLostMessage);
      }
    }

    function CreateSourcingRequestComponent_div_39_div_2_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Company Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_39_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_39_div_2_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r61 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r61.f.CustomerDetails.get("CompanyName").errors == null ? null : ctx_r61.f.CustomerDetails.get("CompanyName").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_39_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Company already exist");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_39_Template(rf, ctx) {
      if (rf & 1) {
        var _r65 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 213);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "input", 214);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_div_39_Template_input_change_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r65);

          var ctx_r64 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r64.isSourcingCompanyExist();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_39_div_2_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, CreateSourcingRequestComponent_div_39_div_3_Template, 3, 0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r8.formSubmited && ctx_r8.f.CustomerDetails.get("CompanyName").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r8.companyExits);
      }
    }

    function CreateSourcingRequestComponent_div_40_div_2_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Company Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_40_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_40_div_2_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r66 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r66.f.CustomerDetails.get("CompanyId").errors == null ? null : ctx_r66.f.CustomerDetails.get("CompanyId").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_40_Template(rf, ctx) {
      if (rf & 1) {
        var _r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 217);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-multiselect-dropdown", 218);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function CreateSourcingRequestComponent_div_40_Template_ng_multiselect_dropdown_onSelect_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r69);

          var ctx_r68 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r68.onCompSelect($event, true);
        })("onDeSelect", function CreateSourcingRequestComponent_div_40_Template_ng_multiselect_dropdown_onDeSelect_1_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r69);

          var ctx_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r70.onCompSelect($event, false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_40_div_2_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r9.CompDdlSetting)("data", ctx_r9.AllTPOCompaniesList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r9.formSubmited && ctx_r9.f.CustomerDetails.get("CompanyId").errors);
      }
    }

    function CreateSourcingRequestComponent_div_43_div_8_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Contact Person is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_43_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_43_div_8_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r71 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r71.f.CustomerDetails.get("Name").errors == null ? null : ctx_r71.f.CustomerDetails.get("Name").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_43_a_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 224);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_div_43_a_9_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r75);

          var ctx_r74 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r74.clickNewPerson(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 225);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Use Existing ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_43_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 219);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 220);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "label", 221);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, " Contact Person");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 222);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, CreateSourcingRequestComponent_div_43_div_8_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateSourcingRequestComponent_div_43_a_9_Template, 3, 0, "a", 223);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r10.formSubmited && ctx_r10.f.CustomerDetails.get("Name").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r10.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == false);
      }
    }

    function CreateSourcingRequestComponent_div_44_option_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r79 = ctx.$implicit;

        var ctx_r76 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r79.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", item_r79.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("selected", item_r79.Id == ctx_r76.f.CustomerDetails.get("UserId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r79.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_44_div_11_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Contact Person is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_44_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_44_div_11_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r77 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r77.f.CustomerDetails.get("UserId").errors == null ? null : ctx_r77.f.CustomerDetails.get("UserId").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_44_a_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r82 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 234);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_div_44_a_12_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r82);

          var ctx_r81 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r81.clickNewPerson(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 235);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Create New ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_44_Template(rf, ctx) {
      if (rf & 1) {
        var _r84 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 226);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 227);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "span", 228);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 229);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 221);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Contact Person");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "select", 230);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_div_44_Template_select_change_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r84);

          var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r83.getSourcingContactPersonDetails($event.target.value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "option", 231);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Select Contact Person");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, CreateSourcingRequestComponent_div_44_option_10_Template, 2, 4, "option", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, CreateSourcingRequestComponent_div_44_div_11_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateSourcingRequestComponent_div_44_a_12_Template, 3, 0, "a", 232);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r11.CompanyContactPersonsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.formSubmited && ctx_r11.f.CustomerDetails.get("UserId").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == false && !ctx_r11.f.IsRegularBuyer.value);
      }
    }

    function CreateSourcingRequestComponent_div_50_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 236);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Unable to verify number! You will miss Text Alerts.");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_51_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Mobile Number is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_51_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_51_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r13.f.CustomerDetails.get("PhoneNumber").errors == null ? null : ctx_r13.f.CustomerDetails.get("PhoneNumber").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_57_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Email is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_57_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_57_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_57_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_57_span_2_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r14.f.CustomerDetails.get("Email").errors == null ? null : ctx_r14.f.CustomerDetails.get("Email").errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r14.f.CustomerDetails.get("Email").errors == null ? null : ctx_r14.f.CustomerDetails.get("Email").errors.email);
      }
    }

    function CreateSourcingRequestComponent_div_59_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "app-contact-person", 237);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx_r15.f.CustomerDetails);
      }
    }

    function CreateSourcingRequestComponent_option_71_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r88 = ctx.$implicit;

        var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r88.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", item_r88.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("selected", item_r88.Id == ctx_r16.f.TruckLoadType.value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r88.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_72_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Truck Load Type is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_72_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_72_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r17.f.TruckLoadType.errors == null ? null : ctx_r17.f.TruckLoadType.errors.required);
      }
    }

    function CreateSourcingRequestComponent_option_78_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r90 = ctx.$implicit;

        var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r90.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", item_r90.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("selected", item_r90.Id == ctx_r18.f.FreightOnBoardType.value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r90.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_79_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Freight OnBoard Type is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_79_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_79_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r19.f.FreightOnBoardType.errors == null ? null : ctx_r19.f.FreightOnBoardType.errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_85_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Accounting CompanyId is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_85_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_85_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r20.f.AccountingCompanyId.errors == null ? null : ctx_r20.f.AccountingCompanyId.errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_91_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " SR Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_91_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_91_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r21.f.RequestName.errors == null ? null : ctx_r21.f.RequestName.errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_99_Template(rf, ctx) {
      if (rf & 1) {
        var _r95 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 164);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "input", 238);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_div_99_Template_input_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r95);

          var ctx_r94 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r94.ClearAddress();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 239);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "New");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", true);
      }
    }

    function CreateSourcingRequestComponent_div_100_Template(rf, ctx) {
      if (rf & 1) {
        var _r97 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 240);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "input", 241);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_div_100_Template_input_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r97);

          var ctx_r96 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r96.ClearAddress();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 242);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Existing");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", false);
      }
    }

    function CreateSourcingRequestComponent_div_105_div_6_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Location Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_105_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_105_div_6_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r98 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r98.f.AddressDetails.get("JobName").errors == null ? null : ctx_r98.f.AddressDetails.get("JobName").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_105_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 243);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label", 244);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Location Name ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "input", 245);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateSourcingRequestComponent_div_105_div_6_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r24.formSubmited && ctx_r24.f.AddressDetails.get("JobName").errors);
      }
    }

    function CreateSourcingRequestComponent_div_106_div_6_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Location Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_106_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_106_div_6_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r100 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r100.f.AddressDetails.get("JobName").errors == null ? null : ctx_r100.f.AddressDetails.get("JobName").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_106_Template(rf, ctx) {
      if (rf & 1) {
        var _r103 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 246);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label", 247);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Location Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "ng-multiselect-dropdown", 248);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function CreateSourcingRequestComponent_div_106_Template_ng_multiselect_dropdown_onSelect_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r103);

          var ctx_r102 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r102.onJobSelect($event, ctx_r102.sourcingRequestForm.controls["CustomerDetails"]["controls"]["CompanyName"].value, true);
        })("onDeSelect", function CreateSourcingRequestComponent_div_106_Template_ng_multiselect_dropdown_onDeSelect_5_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r103);

          var ctx_r104 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r104.onJobSelect($event, null, false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateSourcingRequestComponent_div_106_div_6_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx_r25.CompDdlSetting)("data", ctx_r25.allJobList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r25.formSubmited && ctx_r25.f.AddressDetails.get("JobName").errors);
      }
    }

    function CreateSourcingRequestComponent_option_120_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r105 = ctx.$implicit;

        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r105.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r105.Id)("selected", item_r105.Id == ctx_r26.f.AddressDetails.get("CountryId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r105.Code, " ");
      }
    }

    function CreateSourcingRequestComponent_div_121_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Country is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_121_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_121_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r27.f.AddressDetails.get("CountryId").errors == null ? null : ctx_r27.f.AddressDetails.get("CountryId").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_122_option_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 251);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r108 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r108.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r108.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r108.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_122_Template(rf, ctx) {
      if (rf & 1) {
        var _r110 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Country");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "select", 249);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_div_122_Template_select_change_4_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r110);

          var ctx_r109 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r109.countryGroupChanged($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "option", 250);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Select");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, CreateSourcingRequestComponent_div_122_option_7_Template, 2, 3, "option", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r28.countryGroupList);
      }
    }

    function CreateSourcingRequestComponent_option_130_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r111 = ctx.$implicit;

        var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r111.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r111.Id)("selected", item_r111.Id == ctx_r29.f.AddressDetails.get("Currency").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r111.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_option_137_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r112 = ctx.$implicit;

        var ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r112.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r112.Id)("selected", item_r112.Id == ctx_r30.f.AddressDetails.get("UOM").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r112.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_164_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Address is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_164_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_164_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r31.f.AddressDetails.get("Address").errors == null ? null : ctx_r31.f.AddressDetails.get("Address").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_170_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Zip is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_170_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_170_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r32.f.AddressDetails.get("ZipCode").errors == null ? null : ctx_r32.f.AddressDetails.get("ZipCode").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_179_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " City is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_179_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_179_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r33.f.AddressDetails.get("City").errors == null ? null : ctx_r33.f.AddressDetails.get("City").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_186_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " County Name is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_186_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_186_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r34.f.AddressDetails.get("CountyName").errors == null ? null : ctx_r34.f.AddressDetails.get("CountyName").errors.required);
      }
    }

    function CreateSourcingRequestComponent_option_196_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r117 = ctx.$implicit;

        var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r117.StateId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r117.StateId)("selected", item_r117.StateId == ctx_r35.f.AddressDetails.get("StateId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r117.StateName, " ");
      }
    }

    function CreateSourcingRequestComponent_div_197_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " State is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_197_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_197_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r36.f.AddressDetails.get("StateId").errors == null ? null : ctx_r36.f.AddressDetails.get("StateId").errors.required);
      }
    }

    function CreateSourcingRequestComponent_option_229_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 251);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r119 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r119.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r119.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r119.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_247_div_9_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Delivary Type is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_247_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_247_div_9_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r120 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r120.f.FuelDeliveryDetails.get("SingleDeliverySubTypes").errors == null ? null : ctx_r120.f.FuelDeliveryDetails.get("SingleDeliverySubTypes").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_247_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 252);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 253);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 254);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Type");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "select", 255);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "option", 256);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Delivery Date");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "option", 256);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Delivery Date Range");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateSourcingRequestComponent_div_247_div_9_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r38.formSubmited && ctx_r38.f.FuelDeliveryDetails.get("SingleDeliverySubTypes").errors);
      }
    }

    function CreateSourcingRequestComponent_div_253_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Start Date is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_253_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_253_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r39.f.FuelDeliveryDetails.get("StartDate").errors == null ? null : ctx_r39.f.FuelDeliveryDetails.get("StartDate").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_264_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Start Time is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_264_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_264_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r40.f.FuelDeliveryDetails.get("StartTime").errors == null ? null : ctx_r40.f.FuelDeliveryDetails.get("StartTime").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_270_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " End Time is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_270_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_270_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r41.f.FuelDeliveryDetails.get("EndTime").errors == null ? null : ctx_r41.f.FuelDeliveryDetails.get("EndTime").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_298_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 257);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 228);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_299_option_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 251);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r127 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r127.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r127.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r127.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_div_299_div_6_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Fuel type is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_299_div_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_299_div_6_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r126 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r126.f.FuelDetails.get("FuelTypeId").errors == null ? null : ctx_r126.f.FuelDetails.get("FuelTypeId").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_299_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 258);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 259);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "select", 260);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "option", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Select");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, CreateSourcingRequestComponent_div_299_option_5_Template, 2, 3, "option", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, CreateSourcingRequestComponent_div_299_div_6_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r43 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r43.FuelProductsList);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r43.formSubmited && ctx_r43.f.FuelDetails.get("FuelTypeId").errors);
      }
    }

    function CreateSourcingRequestComponent_div_300_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 261);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 33);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 262);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, " Product Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "input", 263);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 264);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label", 265);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, " Product Description ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "textarea", 266);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_322_div_9_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Quantity is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_322_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_322_div_9_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r129 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r129.f.FuelDetails.get("Quantity").errors == null ? null : ctx_r129.f.FuelDetails.get("Quantity").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_322_div_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_322_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_322_div_10_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r130 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r130.f.FuelDetails.get("Quantity").errors.pattern);
      }
    }

    function CreateSourcingRequestComponent_div_322_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 267);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 268);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 269);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 270);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "input", 271);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 272);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Gallons");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, CreateSourcingRequestComponent_div_322_div_9_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, CreateSourcingRequestComponent_div_322_div_10_Template, 2, 1, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r45.formSubmited && ctx_r45.f.FuelDetails.get("Quantity").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r45.f.FuelDetails.get("Quantity").errors);
      }
    }

    function CreateSourcingRequestComponent_div_323_div_13_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_323_div_13_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Minimum Quantity is Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_323_div_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_323_div_13_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_323_div_13_span_2_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r133 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r133.f.FuelDetails.get("MinimumQuantity").errors.pattern);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r133.f.FuelDetails.get("MinimumQuantity").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_323_div_24_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Invalid. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_323_div_24_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Maximum Quantity is Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_323_div_24_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_323_div_24_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_323_div_24_span_2_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r134 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r134.f.FuelDetails.get("MaximumQuantity").errors.pattern);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r134.f.FuelDetails.get("MaximumQuantity").errors.required);
      }
    }

    function CreateSourcingRequestComponent_div_323_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 273);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 274);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 275);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 276);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, " Min");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 277);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "input", 278);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 272);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Gallons");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, CreateSourcingRequestComponent_div_323_div_13_Template, 3, 2, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 275);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 279);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, " Max");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 277);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](21, "input", 280);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "span", 272);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "Gallons");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](24, CreateSourcingRequestComponent_div_323_div_24_Template, 3, 2, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r46 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r46.f.FuelDetails.get("MinimumQuantity").errors);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r46.f.FuelDetails.get("MaximumQuantity").errors);
      }
    }

    function CreateSourcingRequestComponent_ng_container_326_option_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r140 = ctx.$implicit;

        var ctx_r139 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r140.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", item_r140.Id)("selected", item_r140.Id == ctx_r139.f.FuelDetails.get("QuantityIndicatorTypes").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r140.Name, " ");
      }
    }

    function CreateSourcingRequestComponent_ng_container_326_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Billable Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "select", 281);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, CreateSourcingRequestComponent_ng_container_326_option_4_Template, 2, 4, "option", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r47.billableList);
      }
    }

    function CreateSourcingRequestComponent_div_359_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Value should be greater than 0 ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_359_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 216);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Net Days is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function CreateSourcingRequestComponent_div_359_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, CreateSourcingRequestComponent_div_359_span_1_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, CreateSourcingRequestComponent_div_359_span_2_Template, 2, 0, "span", 215);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r48 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r48.f.FuelDeliveryDetails.get("NetDays").errors == null ? null : ctx_r48.f.FuelDeliveryDetails.get("NetDays").errors.min);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r48.f.FuelDeliveryDetails.get("NetDays").errors == null ? null : ctx_r48.f.FuelDeliveryDetails.get("NetDays").errors.required);
      }
    }

    function CreateSourcingRequestComponent_option_367_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 233);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r143 = ctx.$implicit;

        var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", item_r143.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("value", item_r143.Id);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("selected", item_r143.Id == ctx_r49.f.FuelDeliveryDetails.get("PaymentMethods").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", item_r143.Name, " ");
      }
    }

    var _c1 = function _c1(a0) {
      return {
        "active": a0
      };
    };

    function CreateSourcingRequestComponent_div_379_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 282);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 283);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "h5", 284);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "small");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "p", 285);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r144 = ctx.$implicit;
        var i_r145 = ctx.index;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](4, _c1, i_r145 == 0));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r144.UserName);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r144.CreatedDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r144.Note);
      }
    }

    function CreateSourcingRequestComponent_div_381_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h3");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Inventory Capture Method");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 286);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 287);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Not Specified");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "input", 288);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label", 289);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Connected");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](15, "input", 290);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 291);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Manual");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](19, "input", 292);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "label", 293);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Call-in");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](23, "input", 294);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "label", 295);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Mixed");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 4);
      }
    }

    function CreateSourcingRequestComponent_div_406_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 183);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "h3");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Asset");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 296);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "input", 297);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "label", 298);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, " Enable Asset Level Tracking ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "input", 299);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label", 300);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, " Enable drop status for all assets ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 301);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 185);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](17, "input", 302);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "label", 303);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, " Require Pre-Post Dip test data for Asset/Tank ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

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

    var _c3 = function _c3(a0) {
      return {
        "hide-element": a0
      };
    };

    var CreateSourcingRequestComponent = /*#__PURE__*/function () {
      function CreateSourcingRequestComponent(fb, salesUserService, route_active, router, changeDetectorRef, confirmationDialogService) {
        _classCallCheck(this, CreateSourcingRequestComponent);

        this.fb = fb;
        this.salesUserService = salesUserService;
        this.route_active = route_active;
        this.router = router;
        this.changeDetectorRef = changeDetectorRef;
        this.confirmationDialogService = confirmationDialogService;
        this.pageloader = false;
        this.keyword = 'Code';
        this.Approved_Terminals_keyword = "Name";
        this.PrcingCodevalue = '';
        this.popoverTitle = 'Create PO';
        this.popoverMessage = 'Are you sure want to create PO?';
        this.cancelClicked = false;
        this.isPriceCodeLoading = false;
        this.editSourcingId = 0;
        this.IsLoading = false;
        this.formSubmited = false;
        this.companyExits = false;
        this.isPersonNew = true;
        this.TruckTypeLoadList = [];
        this.FreightOnBoardTypesList = [];
        this.allJobList = [];
        this.countryList = [];
        this.currucyList = [];
        this.UomList = [];
        this.statesList = [];
        this.filteredStatesList = [];
        this.FuelProductsList = [];
        this.FeeTypesList = [];
        this.FeeSubTypesList = [];
        this.FeeConstraintTypesList = [];
        this.PaymentMethodsList = [];
        this.RackAvgPricingTypesList = [];
        this.CompanyContactPersonsList = [];
        this.CitySourcingGroupTerminalPriceAvailableList = [];
        this.SourcingCityGroupTerminalsList = [];
        this.ClosedTerminalList = [];
        this.OpisTerminalList = [];
        this.AllTPOCompaniesList = [];
        this.RequestStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["SourcingRequestStatus"];
        this.GeneralNotesHistory = [];
        this.pricingCodes = [];
        this.pricingfeedTypeId = 0;
        this.pricingfuelClassTypeId = 0;
        this.UserContext = {};
        this.IsSuppressPricing = false;
        this.isValidMobile = true;
        this.billableList = [{
          Id: '1',
          Name: 'Net'
        }, {
          Id: '2',
          Name: 'Gross'
        }];
        this.MaxInputDate = moment__WEBPACK_IMPORTED_MODULE_3__().add(1, 'year').toDate();
        this.countryGroupList = [];
        this.DispatchRegionList = [];
        this.companyPreferenceSetting = false;
        this.regexPhone = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
        this.popoverLostTitle = 'Request Lost';
        this.popoverLostMessage = 'This request will be lost. Are you sure want to lose request?'; // map settings

        this.mapConstants = new _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["MapConstants"]();
        this.UoM = 0;
        this.CompDdlSetting = {};
        this.getUserContext();
        this.initailizeSourcingReqForm();
      }

      _createClass(CreateSourcingRequestComponent, [{
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (typeof CurrentUserId !== "undefined") {
            this.userId = CurrentUserId;
          }
        }
      }, {
        key: "ngOnInit",
        value: function ngOnInit() {
          this.pageloader = true;
          this.getFreightOnBoardTypes();
          this.getTruckLoadType();
          this.getUoMList();
          this.getCurrecyList();
          this.getCountryList(); // this.setCurrency(1);

          this.getPaymentMethods(); //this.getFuelProducts();

          this.getStatesOfAllCountries();
          this.getRackAvgPricingTypes();
          this.getAllTPOCompanies();

          if (this.f.AddressDetails.get('CountryId').value == 2) {
            //canada
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
          }

          this.isSalesUserType = typeof IsSalesUser !== undefined && IsSalesUser;
          this.getcountryGroupList();
          this.GetDispatchRegions();
          this.CompDdlSetting = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'id',
            textField: 'text',
            enableCheckAll: false,
            itemsShowLimit: 1,
            allowSearchFilter: true
          };
        }
      }, {
        key: "onCompSelect",
        value: function onCompSelect(company, isSelected) {
          if (isSelected) {
            this.f.CustomerDetails.get('CompanyId').setValue(company.id);
            this.getSourcingCompanyContactPersons(company.id);
            this.getJobLists(company.id, this.f.TruckLoadType.value, this.f.FreightOnBoardType.value);
            this.isRegularBuyerUpdate(company.id, false);
          } else {
            this.CompanyContactPersonsList = [];
            this.f.AddressDetails['controls']['IsNewJob'].setValue(true);
            this.allJobList = [];
            this.setRegularBuyerValidation(true);
          }

          this.ClearAddress();
          this.f.CustomerDetails.get('UserId').setValue(null);
          this.f.CustomerDetails.get('PhoneNumber').setValue(null);
          this.f.CustomerDetails.get('Email').setValue(null);
          this.f.AddressDetails.get('JobId').setValue(null);
          this.f.AddressDetails.get('JobName').setValue(null);
          this.f.AddressDetails.get('TempJob').setValue(null);
        }
      }, {
        key: "onJobSelect",
        value: function onJobSelect(job, companyName, isSelected) {
          if (isSelected) {
            this.f.AddressDetails.get('JobId').setValue(job.id);
            this.f.AddressDetails.get('JobName').setValue(job.text);
            this.getJobDetails(job.id, companyName);
          } else {
            this.f.AddressDetails.get('JobId').setValue(null);
            this.f.AddressDetails.get('JobName').setValue(null); //this.sourcingRequestForm.get('AddressDetails').reset();
            // this.f.AddressDetails.get('DisplayJobID').setValue(null);
            // this.f.AddressDetails.get('Address').setValue(null);
            // this.f.AddressDetails.get('ZipCode').setValue(null);
            // this.f.AddressDetails.get('City').setValue(null);
            // this.f.AddressDetails.get('CountyName').setValue(null);
            // this.f.AddressDetails.get('StateId').setValue(null);
            // this.f.AddressDetails.get('Latitude').setValue(null);
            // this.f.AddressDetails.get('Longitude').setValue(null);
          }
        }
      }, {
        key: "companyExistanceChanged",
        value: function companyExistanceChanged(isNew) {
          if (isNew) {
            this.f.CustomerDetails.get('CompanyName').setValue(null);
            this.f.CustomerDetails.get('UserId').setValue(null);
            this.f.CustomerDetails.get('PhoneNumber').setValue(null);
            this.f.CustomerDetails.get('Email').setValue(null);
            this.f.AddressDetails.get('JobId').setValue(null);
            this.f.AddressDetails.get('JobName').setValue(null);
            this.f.AddressDetails.get('IsNewJob').setValue(true);
            this.setRegularBuyerValidation(isNew);
            this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(this.companyPreferenceSetting);
            this.f.CustomerDetails.get('TempCompany').setValue(null);
            this.onCompSelect(null, false);
          } else {
            this.ClearAddress();
          }

          this.clickNewPerson(true);
        }
      }, {
        key: "setRegularBuyerValidation",
        value: function setRegularBuyerValidation(isNewCompany) {
          this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(this.companyPreferenceSetting);
          this.pricingModuleComponent.toggleSuppressPricing(this.companyPreferenceSetting);
          this.f.IsRegularBuyer.setValue(!isNewCompany);
        }
      }, {
        key: "initailizeSourcingReqForm",
        value: function initailizeSourcingReqForm() {
          this.sourcingRequestForm = this.fb.group({
            Id: this.fb.control(null),
            TruckLoadType: this.fb.control(null),
            FreightOnBoardType: this.fb.control(null),
            AccountingCompanyId: this.fb.control(null),
            DisplayRequestId: this.fb.control(null),
            RequestName: this.fb.control(null),
            SalesUserId: this.fb.control(null),
            GeneralNote: this.fb.control(null),
            RequestStatus: this.fb.control(0),
            IsSupressOrderPricing: this.fb.control(false),
            IsRegularBuyer: this.fb.control(false),
            CustomerDetails: this.fb.group({
              Id: this.fb.control(null),
              UserId: this.fb.control(null),
              CompanyId: this.fb.control(null),
              IsNewCompany: this.fb.control(true),
              CompanyName: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              Name: this.fb.control(null),
              PhoneNumber: this.fb.control(null),
              Email: this.fb.control(null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].email]),
              IsInvitationEnabled: this.fb.control(null),
              IsNotifyDeliveries: this.fb.control(null),
              IsNotifySchedules: this.fb.control(null),
              TempCompany: this.fb.control(null),
              ContactPersons: this.initializeContactPersons([])
            }),
            AddressDetails: this.fb.group({
              Id: this.fb.control(null),
              JobName: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              DisplayJobID: this.fb.control(null),
              JobId: this.fb.control(null),
              IsNewJob: this.fb.control(true),
              Address: this.fb.control(null),
              City: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              StateId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              CountryId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              CountyName: this.fb.control(null),
              CountryName: this.fb.control(null),
              CountryCode: this.fb.control(null),
              StateName: this.fb.control(null),
              Currency: this.fb.control(null),
              ZipCode: this.fb.control(null),
              IsProFormaPoEnabled: this.fb.control(null),
              SignatureEnabled: this.fb.control(null),
              IsGeocodeUsed: this.fb.control(true),
              Latitude: this.fb.control(this.mapConstants.CenterLat, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')),
              Longitude: this.fb.control(this.mapConstants.CenterLon, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')),
              TimeZoneName: this.fb.control(null),
              LocationManagedType: this.fb.control(null),
              IsCompanyOwned: this.fb.control(null),
              MarineUoM: this.fb.control(null),
              IsMarineLocation: this.fb.control(null),
              InventoryDataCaptureType: this.fb.control(null),
              UOM: this.fb.control(1),
              DispatchRegionId: this.fb.control(null),
              TempJob: this.fb.control(null)
            }),
            FuelDetails: this.fb.group({
              Id: this.fb.control(null),
              FuelDisplayGroupId: this.fb.control(1),
              FuelTypeId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              QuantityTypeId: this.fb.control(3),
              Quantity: this.fb.control(0, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]),
              MinimumQuantity: this.fb.control(0, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]),
              MaximumQuantity: this.fb.control(0, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(/^([0-9]\d*(\.\d+)?)$/)]),
              QuantityIndicatorTypes: this.fb.control(1),
              NonStandardFuelName: this.fb.control(null),
              NonStandardFuelDescription: this.fb.control(null),
              IsTierPricing: this.fb.control(null),
              PricingTypeId: this.fb.control(2),
              PricePerGallon: this.fb.control(null),
              Fees: this.intilizeFuelRequestFees()
            }),
            FuelDeliveryDetails: this.fb.group({
              Id: this.fb.control(null),
              DeliveryTypeId: this.fb.control(2),
              StartDate: this.fb.control(moment__WEBPACK_IMPORTED_MODULE_3__(new Date()).format('MM/DD/YYYY')),
              EndDate: this.fb.control(null),
              StartTime: this.fb.control('8:00 AM'),
              EndTime: this.fb.control('5:00 PM'),
              SingleDeliverySubTypes: this.fb.control(0),
              PaymentMethods: this.fb.control(null),
              PaymentTermId: this.fb.control(1),
              NetDays: this.fb.control(0),
              IsPrePostDipRequired: this.fb.control(null),
              OrderEnforcementId: this.fb.control(1)
            }),
            FuelPricingDetails: this.fb.group({
              Id: this.fb.control(null),
              LeadRequestId: this.fb.control(null),
              PricingTypeId: this.fb.control(2),
              PricePerGallon: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
              Code: this.fb.control(null),
              TempCode: this.fb.control(null),
              CodeId: this.fb.control(null),
              CodeDescription: this.fb.control(null),
              RackAvgTypeId: this.fb.control(1),
              RackPrice: this.fb.control(0),
              EnableCityRack: this.fb.control(null),
              TerminalName: this.fb.control(null),
              TempTerminalName: this.fb.control(null),
              TerminalId: this.fb.control(null),
              SupplierCostMarkupTypeId: this.fb.control(1),
              SupplierCostMarkupValue: this.fb.control(0),
              SupplierCost: this.fb.control(null),
              SupplierCostTypeId: this.fb.control(null),
              MarkertBasedPricingTypeId: this.fb.control(null),
              CityGroupTerminalId: this.fb.control(null),
              CityGroupTerminalName: this.fb.control(null),
              CityGroupTerminalStateId: this.fb.control(null),
              BrokerMarkUp: this.fb.control(null),
              Currency: this.fb.control(null),
              ExchangeRate: this.fb.control(null),
              IsTierPricingRequired: this.fb.control(null),
              DifferentFuelPrices: this.fb.control(null),
              FormattedPricing: this.fb.control(null),
              FuelTypeId: this.fb.control(null),
              TierPricing: this.fb.group({
                TierPricingType: this.fb.control(src_app_app_enum__WEBPACK_IMPORTED_MODULE_6__["TierPricingType"].VolumeBased),
                IsResetCumulation: this.fb.control(null),
                AboveQuantityPricing: this.fb.control(null),
                Pricings: this.fb.array([]),
                ResetCumulationSetting: this.fb.group({
                  CumulationType: this.fb.control(1),
                  Day: this.fb.control(null),
                  Date: this.fb.control(null)
                }),
                DisplayCumulationFrequency: this.fb.control(null)
              }),
              PricingSourceId: this.fb.control(1),
              PricingNote: this.fb.control(null),
              TempPricingCodeDetails: this.fb.control(null),
              FuelPricingDetails: this.fb.group({
                PricingSourceId: this.fb.control(1),
                PricingCode: this.fb.control({
                  Id: null,
                  Code: null,
                  Description: null
                })
              })
            }),
            AdditionalDetailsViewModel: this.fb.group({
              Id: this.fb.control(null),
              IsAssetTracked: this.fb.control(null),
              IsAssetDropStatusEnabled: this.fb.control(null)
            })
          });
        }
      }, {
        key: "f",
        get: function get() {
          return this.sourcingRequestForm.controls;
        }
      }, {
        key: "removeValidator",
        value: function removeValidator() {
          this.f.CustomerDetails.get('CompanyName').setValidators([]);
          this.f.CustomerDetails.get('CompanyName').updateValueAndValidity();
        }
      }, {
        key: "addValidators",
        value: function addValidators() {
          var val = [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required];
          this.f.CustomerDetails.get('CompanyName').setValidators(val);
          this.f.CustomerDetails.get('CompanyName').updateValueAndValidity();
        }
      }, {
        key: "initializeContactPersons",
        value: function initializeContactPersons(contactPersons) {
          var contactPersonsForm = this.fb.array([]);

          for (var i = 0; i < contactPersons.length; i++) {
            var _contactPersonForm = this.fb.group({
              Name: this.fb.control(contactPersons[i].Name),
              PhoneNumber: this.fb.control(contactPersons[i].PhoneNumber, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(this.regexPhone)]),
              Email: this.fb.control(contactPersons[i].Email, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].email]),
              IsValidMobileNumber: this.fb.control(contactPersons[i].IsPhoneNumberConfirmed)
            });

            contactPersonsForm.push(_contactPersonForm);
          }

          return contactPersonsForm;
        }
      }, {
        key: "initializeContactPerson",
        value: function initializeContactPerson(contactPerson) {
          return this.fb.group({
            Name: this.fb.control(contactPerson.Name),
            PhoneNumber: this.fb.control(contactPerson.PhoneNumber, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern(this.regexPhone)]),
            Email: this.fb.control(contactPerson.Email, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].email]),
            IsPhoneNumberConfirmed: this.fb.control(contactPerson.IsPhoneNumberConfirmed)
          });
        }
      }, {
        key: "intilizeFuelRequestFees",
        value: function intilizeFuelRequestFees() {
          var _FRFArray = this.fb.array([]);

          _FRFArray.push(this.fb.group({
            FeeTypeId: this.fb.control(null),
            FeeSubTypeId: this.fb.control(null),
            FeeSubTypeName: this.fb.control(null),
            Fee: this.fb.control(null),
            FeeDetails: this.fb.control(null),
            FeeConstraintTypeId: this.fb.control(null),
            IncludeInPPG: this.fb.control(null),
            OtherFeeTypeId: this.fb.control(null)
          }));

          return _FRFArray;
        }
      }, {
        key: "getDetailsPage",
        value: function getDetailsPage(Id) {
          this.router.navigate(['SourcingRequest/Details/' + Id]);
        }
      }, {
        key: "clickNewPerson",
        value: function clickNewPerson(isnew) {
          this.isPersonNew = isnew;

          if (isnew) {
            this.f.CustomerDetails.get('Name').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
            this.f.CustomerDetails.get('UserId').setValidators([]);
          } else {
            this.f.CustomerDetails.get('UserId').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
            this.f.CustomerDetails.get('Name').setValidators([]);
          }

          this.f.CustomerDetails.get('Name').updateValueAndValidity();
          this.f.CustomerDetails.get('UserId').updateValueAndValidity();
        }
      }, {
        key: "getPreferencesSettings",
        value: function getPreferencesSettings() {
          var _this2 = this;

          this.IsLoading = true;

          var _this = this;

          this.salesUserService.GetPreferencesSettings().subscribe(function (data) {
            if (data) {
              _this2.companyPreferenceSetting = data.IsSupressOrderPricing;

              var FreightOnBoardType = _this2.FreightOnBoardTypesList.find(function (t) {
                return t.Id == data.FreightOnBoardType;
              });

              var truckload = _this2.TruckTypeLoadList.find(function (t) {
                return t.Id == data.TruckLoadType;
              });

              var truckLoadType;

              if (truckload) {
                truckLoadType = truckload.Id;
              }

              if (FreightOnBoardType != null) {
                _this2.sourcingRequestForm.get('FreightOnBoardType').patchValue(FreightOnBoardType.Id);
              }

              _this2.sourcingRequestForm.get('TruckLoadType').patchValue(truckLoadType);

              _this2.sourcingRequestForm.get('CustomerDetails').patchValue(data.CustomerDetails);

              _this2.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);

              _this2.sourcingRequestForm.get('IsSupressOrderPricing').patchValue(data.IsSupressOrderPricing);

              _this2.setCurrency(data.AddressDetails.Currency);

              _this2.sourcingRequestForm.controls['FuelDetails']['controls']['FuelDisplayGroupId'].patchValue(data.FuelDetails.FuelDisplayGroupId);

              _this2.sourcingRequestForm.controls['FuelDetails']['controls']['QuantityTypeId'].patchValue(data.FuelDetails.QuantityTypeId);

              if (!_this.f.IsSupressOrderPricing.value) {
                _this.f.FuelPricingDetails.get('PricePerGallon').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
              }

              _this.f.FuelPricingDetails.get('TierPricing').get('ResetCumulationSetting').get('CumulationType').setValue(1);

              if (_this.pricingModuleComponent) {
                _this.pricingModuleComponent.setPricingCode();
              }

              if (!_this2.editSourcingId) {
                if (_this2.DispatchRegionList && _this2.DispatchRegionList.length == 1) {
                  _this2.sourcingRequestForm.get('AddressDetails').get('DispatchRegionId').setValue(_this2.DispatchRegionList[0].Id);
                }
              }

              _this2.IsLoading = false;
              _this2.pageloader = false;
            }
          });
        }
      }, {
        key: "getAllTPOCompanies",
        value: function getAllTPOCompanies() {
          var _this3 = this;

          this.IsLoading = true;
          this.salesUserService.GetAllTPOCompanies().subscribe(function (data) {
            if (data) {
              var listdata = data.map(function (item) {
                return {
                  id: item.Id,
                  text: item.Name
                };
              });
              _this3.AllTPOCompaniesList = listdata; //this.IsLoading = false;
            }

            var esourcingId = _this3.route_active.snapshot.params.Id;

            if (esourcingId && esourcingId > 0) {
              _this3.editSourcingId = esourcingId;

              _this3.getSourcingDetails();
            } else {
              _this3.getFuelProducts();

              _this3.getPreferencesSettings();
            }

            _this3.IsLoading = false;
          });
        }
      }, {
        key: "getSourcingCompanyContactPersons",
        value: function getSourcingCompanyContactPersons(event) {
          var _this4 = this;

          var companyId = event;
          this.IsLoading = true;
          this.salesUserService.GetSourcingCompanyContactPersons(companyId).subscribe(function (data) {
            if (data) {
              _this4.CompanyContactPersonsList = data;

              var companyname = _this4.AllTPOCompaniesList.filter(function (x) {
                return x.id == companyId;
              });

              _this4.sourcingRequestForm.get('CustomerDetails').get('CompanyName').setValue(companyname[0].text);

              _this4.IsLoading = false;
            } else {
              _this4.IsLoading = false;
            }
          });
        }
      }, {
        key: "getSourcingContactPersonDetails",
        value: function getSourcingContactPersonDetails(userId) {
          var _this5 = this;

          this.salesUserService.GetSourcingContactPersonDetails(userId).subscribe(function (data) {
            if (data) {
              _this5.CompanyContactPersonsDetails = data; // this.CompanyContactPersonsDetails['IsNewCompany']=false;

              _this5.sourcingRequestForm.get('CustomerDetails').get('PhoneNumber').setValue(_this5.CompanyContactPersonsDetails.PhoneNumber);

              _this5.sourcingRequestForm.get('CustomerDetails').get('Email').setValue(_this5.CompanyContactPersonsDetails.Email); // this.sourcingRequestForm.get('CustomerDetails').patchValue(this.CompanyContactPersonsDetails);

            }
          });
        }
      }, {
        key: "getTruckLoadType",
        value: function getTruckLoadType() {
          var _this6 = this;

          this.TruckTypeLoadList = [];
          this.salesUserService.GetTruckLoadType().subscribe(function (data) {
            if (data && data.length > 0) {
              _this6.TruckTypeLoadList = data;

              _this6.sourcingRequestForm.get('TruckLoadType').setValue(data[0].Id);
            }
          });
        }
      }, {
        key: "getFreightOnBoardTypes",
        value: function getFreightOnBoardTypes() {
          var _this7 = this;

          this.FreightOnBoardTypesList = [];
          this.salesUserService.GetFreightOnBoardTypes().subscribe(function (data) {
            if (data && data.length > 0) {
              _this7.FreightOnBoardTypesList = data;

              _this7.sourcingRequestForm.get('FreightOnBoardType').setValue(data[0].Id);
            }
          });
        }
      }, {
        key: "getCountryList",
        value: function getCountryList() {
          var _this8 = this;

          this.salesUserService.GetCountryList().subscribe(function (data) {
            if (data && data.length > 0) {
              _this8.countryList = data;
            }
          });
        }
      }, {
        key: "getCurrecyList",
        value: function getCurrecyList() {
          var _this9 = this;

          this.salesUserService.GetCurrenyList().subscribe(function (data) {
            if (data && data.length > 0) {
              _this9.currucyList = data;
            }
          });
        }
      }, {
        key: "getUoMList",
        value: function getUoMList() {
          var _this10 = this;

          this.salesUserService.GetUoMList().subscribe(function (data) {
            if (data && data.length > 0) {
              _this10.UomList = data;
            }
          });
        }
      }, {
        key: "getStatesOfAllCountries",
        value: function getStatesOfAllCountries(countryId) {
          var _this11 = this;

          this.salesUserService.GetStatesOfAllCountries(countryId).subscribe(function (data) {
            if (data && data.length > 0) {
              _this11.statesList = data;
              _this11.filteredStatesList = _this11.statesList;
            }
          });
        }
      }, {
        key: "getcountryGroupList",
        value: function getcountryGroupList() {
          var _this12 = this;

          this.salesUserService.GetCountryGroupList(4).subscribe(function (data) {
            if (data && data.length > 0) {
              _this12.countryGroupList = data;
            }
          });
        }
      }, {
        key: "getJobLists",
        value: function getJobLists(companyId, isFtl, foAsTerminal) {
          var _this13 = this;

          var companyName = this.AllTPOCompaniesList.find(function (t) {
            return t.id == companyId;
          }).text;
          var ftlvalue = isFtl == "FullTruckLoad" ? true : false;
          var tervalue = foAsTerminal == "Terminal" ? true : false;
          this.salesUserService.GetJobLists(companyName, ftlvalue, tervalue).subscribe(function (data) {
            if (data) {
              var joblistdata = data.map(function (item) {
                return {
                  id: item.Id,
                  text: item.Name
                };
              });
              _this13.allJobList = joblistdata;
            }
          });
        }
      }, {
        key: "getJobDetails",
        value: function getJobDetails(jobId, companyName) {
          var _this14 = this;

          var job = this.allJobList.find(function (x) {
            return x.id == jobId;
          });

          if (job != null) {
            this.salesUserService.GetJobDetails(job.text, companyName).subscribe(function (data) {
              if (data) {
                _this14.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
              }
            });
          }
        }
      }, {
        key: "getFuelProducts",
        value: function getFuelProducts() {
          var _this15 = this;

          //this.f.FuelDetails.get('FuelTypeId').setValue(null);
          var companyId = this.sourcingRequestForm.controls['CustomerDetails']['controls']['CompanyId'].value || 0;
          var jobId = this.sourcingRequestForm.controls['AddressDetails']['controls']['JobId'].value || 0;
          var productDisplayGroupId = this.f.FuelDetails.get('FuelDisplayGroupId').value ? this.f.FuelDetails.get('FuelDisplayGroupId').value : 1;
          this.IsLoading = true;
          this.salesUserService.GetFuelProducts(productDisplayGroupId, companyId, jobId).subscribe(function (data) {
            if (data) {
              _this15.FuelProductsList = data;
              _this15.IsLoading = false;
            } else {
              _this15.IsLoading = false;
            }
          });
        }
      }, {
        key: "getProductListByZip",
        value: function getProductListByZip() {
          var _this16 = this;

          //this.f.FuelDetails.get('FuelTypeId').setValue(null);
          var zipCode = this.sourcingRequestForm.controls['AddressDetails']['controls']['ZipCode'].value;
          this.IsLoading = true;
          this.salesUserService.GetProductListByZip(zipCode).subscribe(function (data) {
            if (data) {
              _this16.FuelProductsList = data;
              _this16.IsLoading = false;
            } else {
              _this16.IsLoading = false;
            }
          });
        }
      }, {
        key: "getAllFeeTypes",
        value: function getAllFeeTypes(companyId, currency, truckLoadType) {
          var _this17 = this;

          this.salesUserService.GetAllFeeTypes(companyId, currency, truckLoadType).subscribe(function (data) {
            if (data) {
              _this17.FeeTypesList = data;
            }
          });
        }
      }, {
        key: "getAllFeeSubTypes",
        value: function getAllFeeSubTypes(feeTypeId, Currency) {
          var _this18 = this;

          this.salesUserService.GetAllFeeSubTypes(feeTypeId, Currency).subscribe(function (data) {
            if (data) {
              _this18.FeeSubTypesList = data;
            }
          });
        }
      }, {
        key: "getAllFeeConstraintTypes",
        value: function getAllFeeConstraintTypes() {
          var _this19 = this;

          this.salesUserService.GetAllFeeConstraintTypes().subscribe(function (data) {
            if (data) {
              _this19.FeeConstraintTypesList = data;
            }
          });
        }
      }, {
        key: "getPaymentMethods",
        value: function getPaymentMethods() {
          var _this20 = this;

          this.salesUserService.PaymentMethods().subscribe(function (data) {
            if (data) {
              _this20.PaymentMethodsList = data;
            }
          });
        }
      }, {
        key: "getRackAvgPricingTypes",
        value: function getRackAvgPricingTypes() {
          var _this21 = this;

          this.salesUserService.GetRackAvgPricingTypes().subscribe(function (data) {
            if (data) {
              _this21.RackAvgPricingTypesList = data;
            }
          });
        }
      }, {
        key: "addFees",
        value: function addFees() {
          var fee = this.sourcingRequestForm.get('FuelDetails').get('FuelRequestFees');
          fee.push(this.fb.group({
            FeeTypeId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            FeeSubTypeId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            FeeSubTypeName: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            Fee: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            FeeDetails: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            FeeConstraintTypeId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            IncludeInPPG: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required),
            OtherFeeTypeId: this.fb.control(null, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required)
          }));
        }
      }, {
        key: "isSourcingCompanyExist",
        value: function isSourcingCompanyExist() {
          var _this22 = this;

          this.companyExits = false;

          if (this.f.CustomerDetails.get('CompanyName').value) {
            this.salesUserService.IsSourcingCompanyExist(this.f.CustomerDetails.get('IsNewCompany').value, this.f.CustomerDetails.get('CompanyName').value).subscribe(function (data) {
              if (data != null || data != undefined) {
                _this22.companyExits = data;
              }
            });
          }
        }
      }, {
        key: "GetDispatchRegions",
        value: function GetDispatchRegions() {
          var _this23 = this;

          this.DispatchRegionList = [];
          this.salesUserService.GetDispatchRegions().subscribe(function (data) {
            if (data && data.length > 0) {
              _this23.DispatchRegionList = data;

              if (data.length == 1) {
                _this23.sourcingRequestForm.get('AddressDetails').get('DispatchRegionId').setValue(data[0].Id);
              }
            }
          });
        }
      }, {
        key: "getSourcingDetails",
        value: function getSourcingDetails() {
          var _this24 = this;

          this.pageloader = true;
          this.changeDetectorRef.detectChanges();

          var _this = this; // this.initailizeSourcingReqForm();


          this.salesUserService.GetSourcingDetailsById(this.editSourcingId).subscribe(function (data) {
            if (data) {
              // this.sourcingRequestForm.patchValue(data);
              _this24.sourcingRequestForm.get('Id').patchValue(data.Id);

              _this24.sourcingRequestForm.get('FreightOnBoardType').patchValue(data.FreightOnBoardType);

              _this24.sourcingRequestForm.get('TruckLoadType').patchValue(data.TruckLoadType);

              _this24.sourcingRequestForm.get('AccountingCompanyId').patchValue(data.AccountingCompanyId);

              _this24.sourcingRequestForm.get('DisplayRequestId').patchValue(data.DisplayRequestId);

              _this24.sourcingRequestForm.get('RequestName').patchValue(data.RequestName);

              _this24.sourcingRequestForm.get('RequestStatus').patchValue(data.RequestStatus);

              _this24.sourcingRequestForm.get('SalesUserId').patchValue(data.SalesUserId);

              _this24.sourcingRequestForm.get('IsSupressOrderPricing').patchValue(data.IsSupressOrderPricing);

              if (!data.CustomerDetails.IsNewCompany) {
                _this24.getSourcingCompanyContactPersons(data.CustomerDetails.CompanyId);

                _this24.getJobLists(data.CustomerDetails.CompanyId, _this24.f.TruckLoadType.value, _this24.f.FreightOnBoardType.value);
              }

              if (data.CustomerDetails.UserId) {
                _this24.isPersonNew = false;
              }

              _this24.sourcingRequestForm.get('CustomerDetails').patchValue(data.CustomerDetails);

              var contactPersons = _this24.sourcingRequestForm.get('CustomerDetails').get('ContactPersons');

              if (data.CustomerDetails && data.CustomerDetails.ContactPersons && data.CustomerDetails.ContactPersons.length > 0) {
                for (var i = 0; i < data.CustomerDetails.ContactPersons.length; i++) {
                  contactPersons.push(_this24.initializeContactPerson(data.CustomerDetails.ContactPersons[i]));
                }
              }

              _this24.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);

              _this24.sourcingRequestForm.get('FuelDetails').patchValue(data.FuelDetails);

              _this24.LeadFees = data.FuelDetails.Fees;

              _this24.sourcingRequestForm.get('FuelDeliveryDetails').patchValue(data.FuelDeliveryDetails);

              _this24.sourcingRequestForm.get('AdditionalDetailsViewModel').patchValue(data.AdditionalDetailsViewModel); //this.getSourcingCityGroupTerminals(data.AddressDetails.StateId,(data.FuelPricingDetails.Code=="AXIS") ? 1 : 2);


              _this24.PrcingCodevalue = data.FuelPricingDetails.Code;
              _this24.GeneralNotesHistory = data.GeneralNotesHistory; // this.sourcingRequestForm.get('FuelPricingDetails').patchValue(data.FuelPricingDetails);
              // this.sourcingRequestForm.get('FuelPricingDetails').get('TempCode').patchValue(data.FuelPricingDetails);
              // this.sourcingRequestForm.get('FuelPricingDetails').get('TempTerminalName').patchValue(data.FuelPricingDetails.TerminalName);

              if (_this.pricingModuleComponent) {
                _this.pricingModuleComponent.patchExistingPricingDetails(data.FuelPricingDetails);
              }

              _this24.UpdateViewedStatus();

              _this24.setCurrency(data.AddressDetails.Currency);

              if (!data.CustomerDetails.IsNewCompany) {
                _this24.f.CustomerDetails.get('TempCompany').setValue([{
                  id: data.CustomerDetails.CompanyId,
                  text: data.CustomerDetails.CompanyName
                }]);
              }

              if (!data.AddressDetails.IsNewJob) {
                _this24.f.AddressDetails.get('TempJob').setValue([{
                  id: data.AddressDetails.JobId,
                  text: data.AddressDetails.JobName
                }]);
              }

              _this24.getFuelProducts();

              _this24.isRegularBuyerUpdate(data.CustomerDetails.CompanyId, true);

              _this24.pageloader = false;

              _this24.changeDetectorRef.detectChanges();
            }
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var _this25 = this;

          this.formSubmited = true;
          this.resetPOValidation();
          this.setSaveValidations();

          if (this.sourcingRequestForm.invalid || this.companyExits) {
            return false;
          }

          if (this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            var pricings = this.f.FuelPricingDetails.get('TierPricing').get('Pricings');
            pricings.controls.forEach(function (pricing) {
              pricing.get('UoM').setValue(_this25.UoM);
              pricing.get('Currency').setValue(_this25.UoM);
            });
          }

          if (this.editSourcingId > 0) {
            this.pageloader = true;
            this.changeDetectorRef.detectChanges();
            this.salesUserService.SaveEditSourcingDetails(this.sourcingRequestForm.getRawValue()).subscribe(function (data) {
              _this25.pageloader = false;

              _this25.changeDetectorRef.detectChanges();

              if (data && data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Request Updated successfully", undefined, undefined);

                _this25.router.navigate(['SalesUser/SourcingRequest/Index']);
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
                return;
              }
            });
          } else {
            this.pageloader = true;
            this.changeDetectorRef.detectChanges();
            this.salesUserService.CreateSourcingRequest(this.sourcingRequestForm.getRawValue()).subscribe(function (data) {
              _this25.pageloader = false;

              _this25.changeDetectorRef.detectChanges();

              if (data && data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Request created successfully.", undefined, undefined);

                _this25.router.navigate(['SalesUser/SourcingRequest/Index']);
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
                return;
              }
            });
          }
        }
      }, {
        key: "setSaveValidations",
        value: function setSaveValidations() {
          if (this.f.CustomerDetails.get('IsNewCompany').value) {
            this.f.CustomerDetails.get('Name').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
            this.f.CustomerDetails.get('Name').updateValueAndValidity();
          } else {
            if (this.isPersonNew) {
              this.f.CustomerDetails.get('Name').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
              this.f.CustomerDetails.get('Name').updateValueAndValidity();
            } else {
              this.f.CustomerDetails.get('UserId').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
              this.f.CustomerDetails.get('UserId').updateValueAndValidity();
            }
          }

          if (this.f.FuelDetails.get('QuantityTypeId').value == 1) {
            this.f.FuelDetails.get('Quantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
          } else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
          }

          this.setNetDaysValidation(this.sourcingRequestForm.get('IsSupressOrderPricing').value);
        }
      }, {
        key: "createPO",
        value: function createPO() {
          var _this26 = this;

          this.setPOValidation();
          this.formSubmited = true;
          this.pageloader = true;

          if (this.sourcingRequestForm.valid && !this.companyExits) {
            this.pageloader = true;
            this.salesUserService.CreateOrderFromSourcingRequest(this.sourcingRequestForm.value).subscribe(function (data) {
              if (data.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("PO created successfully.", undefined, undefined);

                _this26.router.navigate([]).then(function (result) {
                  window.location.href = "/Supplier/Order/View";
                });

                _this26.pageloader = false;
              } else {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
                _this26.pageloader = false;
                return;
              }
            });
          } else {
            this.pageloader = false;
          }
        }
      }, {
        key: "setPOValidation",
        value: function setPOValidation() {
          var required = [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required];
          this.f.TruckLoadType.setValidators(required);
          this.f.TruckLoadType.updateValueAndValidity();
          this.f.FreightOnBoardType.setValidators(required);
          this.f.FreightOnBoardType.updateValueAndValidity(); // this.f.AccountingCompanyId.setValidators(required);
          // this.f.AccountingCompanyId.updateValueAndValidity();
          // this.f.DisplayRequestId.setValidators(required);
          // this.f.DisplayRequestId.updateValueAndValidity();
          // this.f.RequestName.setValidators(required);
          // this.f.RequestName.updateValueAndValidity();

          if (this.f.FuelDeliveryDetails.get('DeliveryTypeId').value == 1) {
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators(required);
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();
          } else {
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators([]);
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();
          }

          this.f.FuelDeliveryDetails.get('StartDate').setValidators(required);
          this.f.FuelDeliveryDetails.get('StartDate').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('StartTime').setValidators(required);
          this.f.FuelDeliveryDetails.get('StartTime').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('EndTime').setValidators(required);
          this.f.FuelDeliveryDetails.get('EndTime').updateValueAndValidity();

          if (this.f.FuelDetails.get('QuantityTypeId').value == 1) {
            this.f.FuelDetails.get('Quantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
          } else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
          } else {
            this.f.FuelDetails.get('Quantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
            this.f.FuelDetails.get('MinimumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
          }

          this.f.AddressDetails.get('CountyName').setValidators(required);
          this.f.AddressDetails.get('CountyName').updateValueAndValidity();
          this.f.AddressDetails.get('StateId').setValidators(required);
          this.f.AddressDetails.get('StateId').updateValueAndValidity();
          this.f.AddressDetails.get('ZipCode').setValidators(required);
          this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
          this.f.AddressDetails.get('Address').setValidators(required);
          this.f.AddressDetails.get('Address').updateValueAndValidity();
          this.setNetDaysValidation(this.sourcingRequestForm.get('IsSupressOrderPricing').value);
          this.f.CustomerDetails.get('PhoneNumber').setValidators(required);
          this.f.CustomerDetails.get('PhoneNumber').updateValueAndValidity();
          this.f.CustomerDetails.get('Email').setValidators(required);
          this.f.CustomerDetails.get('Email').updateValueAndValidity();
        }
      }, {
        key: "setNetDaysValidation",
        value: function setNetDaysValidation(isSuppressPricing) {
          if (this.f.FuelDeliveryDetails.get('PaymentTermId').value == 1 && !isSuppressPricing) {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].min(1)]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
          } else {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
          }
        }
      }, {
        key: "resetPOValidation",
        value: function resetPOValidation() {
          this.f.TruckLoadType.setValidators([]);
          this.f.TruckLoadType.updateValueAndValidity();
          this.f.FreightOnBoardType.setValidators([]);
          this.f.FreightOnBoardType.updateValueAndValidity();
          this.f.AccountingCompanyId.setValidators([]);
          this.f.AccountingCompanyId.updateValueAndValidity();
          this.f.DisplayRequestId.setValidators([]);
          this.f.DisplayRequestId.updateValueAndValidity();
          this.f.RequestName.setValidators([]);
          this.f.RequestName.updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators([]);
          this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('StartDate').setValidators([]);
          this.f.FuelDeliveryDetails.get('StartDate').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('StartTime').setValidators([]);
          this.f.FuelDeliveryDetails.get('StartTime').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('EndTime').setValidators([]);
          this.f.FuelDeliveryDetails.get('EndTime').updateValueAndValidity();
          this.f.FuelDeliveryDetails.get('NetDays').setValidators([]);
          this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
          this.f.CustomerDetails.get('PhoneNumber').setValidators([]);
          this.f.CustomerDetails.get('PhoneNumber').updateValueAndValidity();
          this.f.CustomerDetails.get('Email').setValidators([]);
          this.f.CustomerDetails.get('Email').updateValueAndValidity();
          this.f.AddressDetails.get('ZipCode').setValidators([]);
          this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
          this.f.AddressDetails.get('Address').setValidators([]);
          this.f.AddressDetails.get('Address').updateValueAndValidity();
          this.f.AddressDetails.get('CountyName').setValidators([]);
          this.f.AddressDetails.get('CountyName').updateValueAndValidity();
        }
      }, {
        key: "getAddress",
        value: function getAddress() {
          var _this27 = this;

          var address = this.f.AddressDetails.get('Address').value || '';
          var state = this.f.AddressDetails.get('StateName').value || '';
          var country = this.f.AddressDetails.get('CountryCode').value || '';
          var city = this.f.AddressDetails.get('City').value || '';
          var zipcode = this.f.AddressDetails.get('ZipCode').value || '';
          if (address == '' || state == '' || country == '' || zipcode == '') return;
          address = address + " " + city + " " + state + " " + country + " " + zipcode;
          this.salesUserService.GetAddress(address).subscribe(function (data) {
            _this27.updateAddressData(data);
          });
        }
      }, {
        key: "stateChanged",
        value: function stateChanged() {
          if (this.pricingModuleComponent) {
            this.pricingModuleComponent.getCityGroupTerminals();
          }

          this.getAddress();
          this.setBillableQuantity();
        }
      }, {
        key: "setBillableQuantity",
        value: function setBillableQuantity() {
          var _this28 = this;

          var state = this.statesList.find(function (st) {
            return st.StateId == _this28.f.AddressDetails.get('StateId').value;
          });

          if (state && state.QuantityIndicatorId) {
            this.f.FuelDetails.get('FuelQuantity').get('QuantityIndicatorTypes').setValue(state.QuantityIndicatorId);
          }
        }
      }, {
        key: "acceptRequest",
        value: function acceptRequest() {
          var _this29 = this;

          this.salesUserService.ChangesSourcingRequestStatus(this.RequestStatus.Accepted, this.editSourcingId).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess("Request Accepted.", undefined, undefined);

              _this29.router.navigate(['SalesUser/SourcingRequest/Index']);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              return;
            }
          });
        }
      }, {
        key: "loseRequest",
        value: function loseRequest() {
          var _this30 = this;

          this.salesUserService.ChangesSourcingRequestStatus(this.RequestStatus.Lost, this.editSourcingId).subscribe(function (data) {
            if (data.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msginfo("Request Lost.", undefined, undefined);

              _this30.router.navigate(['SalesUser/SourcingRequest/Index']);
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              return;
            }
          });
        } // openPriceCodeModal(pricingcodeModal) {
        //   this.getPricingCodes();
        //   this.modalService.open(pricingcodeModal, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true });
        // }

      }, {
        key: "isCitySourcingGroupTerminalPriceAvailable",
        value: function isCitySourcingGroupTerminalPriceAvailable(jobId, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId) {
          var _this31 = this;

          this.salesUserService.IsCitySourcingGroupTerminalPriceAvailable(jobId, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId).subscribe(function (data) {
            if (data) {
              _this31.CitySourcingGroupTerminalPriceAvailableList = data;
            }
          });
        } // async getSourcingCityGroupTerminals(stateId: any, sourceId: any) {
        //   this.salesUserService.GetSourcingCityGroupTerminals(stateId,sourceId).subscribe(data => {
        //       if (data) {
        //               this.SourcingCityGroupTerminalsList = data;
        //           }
        //       });
        //   }
        // public getClosedTerminal(event) {
        //  var fuelTypeId=this.f.FuelDetails.get('FuelTypeId').value;
        //  var latitude=this.f.AddressDetails.get('Latitude').value;
        //  var longitude=this.f.AddressDetails.get('Longitude').value;
        //  var countryId=this.f.AddressDetails.get('CountryId').value;
        //  var pricingCodeId=this.f.FuelPricingDetails.get('CodeId').value;
        //  var CityGroupTerminalId=this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
        // //  var terminal= this.SourcingCityGroupTerminalsList.find(t => t.Id == CityGroupTerminalId).Name;
        // var terminal= "";
        // var pricingSourceId=this.f.FuelPricingDetails.get('PricingSourceId').value;
        //  if(pricingSourceId==2){
        //   //  this.getOpisTerminals(CityGroupTerminalId,latitude,longitude,countryId,terminal,pricingSourceId);
        //       this.salesUserService.GetOpisTerminals(CityGroupTerminalId, latitude, longitude, countryId, terminal, pricingSourceId).subscribe(data => {
        //           if (data) {
        //                   this.ClosedTerminalList = data;
        //                   event.stopPropagation();
        //                   this.approveTerminalAuto.open();
        //               }
        //           });
        //  }else{
        //   this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
        //     if (data) {
        //             this.ClosedTerminalList = data;
        //         }
        //     });
        //  }
        // }
        // getApprovedTerminal(event){
        //   var fuelTypeId=this.f.FuelDetails.get('FuelTypeId').value;
        //   var latitude=this.f.AddressDetails.get('Latitude').value;
        //   var longitude=this.f.AddressDetails.get('Longitude').value;
        //   var countryId=this.f.AddressDetails.get('CountryId').value;
        //   var pricingCodeId=this.f.FuelPricingDetails.get('CodeId').value;
        //   var CityGroupTerminalId=this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
        //   var terminal=event;
        //   var pricingSourceId=this.f.FuelPricingDetails.get('PricingSourceId').value;
        //   if(pricingSourceId==2){
        //     this.getOpisTerminals(CityGroupTerminalId,latitude,longitude,countryId,terminal,pricingSourceId);
        //   }else{
        //    this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
        //      if (data) {
        //              this.ClosedTerminalList = data;
        //          }
        //      });
        //   }
        // }

      }, {
        key: "getOpisTerminals",
        value: function getOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source) {
          var _this32 = this;

          this.salesUserService.GetOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source).subscribe(function (data) {
            if (data) {
              _this32.ClosedTerminalList = data;

              _this32.approveTerminalAuto.open();
            }
          });
        } // setTermialvalue(event){
        //   this.f.FuelPricingDetails.get('TerminalName').patchValue(event.Name);
        // }
        // getPricingCodes(isFromControl?: any, prefix?: any) {
        //   this.isPriceCodeLoading=true;
        //   var filterData = {};
        //   if (!prefix) {
        //     prefix = "";
        //   }
        //   if (isFromControl) {
        //     filterData = {
        //       "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
        //       "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
        //       "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
        //       "feedTypeId": 0,
        //       "fuelClassTypeId": 0,
        //       "Prefix": prefix
        //     }
        //   } else {
        //     filterData = {
        //       "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
        //       "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
        //       "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
        //       "feedTypeId": this.pricingfeedTypeId,
        //       "fuelClassTypeId": this.pricingfuelClassTypeId,
        //       "Prefix": ""
        //     }
        //   }
        //   this.salesUserService.GetPricingCodes(filterData).subscribe(data => {
        //     if (data) {
        //       this.pricingCodes = data.PricingCodes;
        //       this.isPriceCodeLoading=false;
        //     }
        //   });
        // }
        // getSelectedPricingCode(item: any) {
        //   this.modalService.dismissAll();
        //   var pricingCodeDisplayData = this.getPricingDisplayData(item);
        //   if (item) {
        //     this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
        //     this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
        //     this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
        //     this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
        //     this.f.FuelPricingDetails.get('PricingSourceId').patchValue(item.PricingSourceId);
        //   }
        //   if (item.PricingSourceId == 1) {
        //     this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
        //   }
        //   else {
        //     this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
        //      this.getSourcingCityGroupTerminals(this.f.AddressDetails.get('StateId').value,this.f.FuelPricingDetails.get('PricingSourceId').value)
        //   }
        //   this.setRackTerminalValidation();
        // }

      }, {
        key: "setRackTerminalValidation",
        value: function setRackTerminalValidation() {
          var isChecked = this.f.FuelPricingDetails.get('EnableCityRack').value;

          if (isChecked) {
            this.f.FuelPricingDetails.get('CityGroupTerminalId').setValidators([_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
            this.f.FuelPricingDetails.get('CityGroupTerminalId').updateValueAndValidity();
          } else {
            this.f.FuelPricingDetails.get('CityGroupTerminalId').setValidators([]);
            this.f.FuelPricingDetails.get('CityGroupTerminalId').updateValueAndValidity();
          }
        }
      }, {
        key: "getPricingDisplayData",
        value: function getPricingDisplayData(item) {
          var displayData = '';

          if (item != undefined || item != null) {
            if (item.PricingTypeId == 2) {
              displayData += item.PricingSource + ', ' + "Fixed";
            } else if (item.PricingTypeId == 4) {
              displayData += item.PricingSource + ', ' + "Fuel Cost";
            } else if (item.PricingTypeId == 1) {
              displayData += item.PricingSource + ', ' + item.RackAvgPricingType;

              if (item.PricingSourceId == 2 || item.PricingSourceId == 3) {
                displayData += ', ' + item.FeedType + ', ' + item.WeekendPricingDay;
              }

              if (item.PricingSourceId == 2) {
                displayData += ', ' + item.FuelClassType + ', ' + item.QuantityIndicator;
              }
            }
          }

          return displayData;
        }
      }, {
        key: "getAddressByZip",
        value: function getAddressByZip() {
          var _this33 = this;

          var zipCode = this.f.AddressDetails.get('ZipCode').value;

          if (zipCode) {
            this.salesUserService.GetAddressByZip(zipCode).subscribe(function (data) {
              if (data) {
                _this33.f.AddressDetails.get('CountryName').patchValue(data.CountryName);

                var country = _this33.countryList.find(function (t) {
                  return t.Code.includes(data.CountryCode);
                });

                if (country) {
                  var countryId = country.Id;

                  _this33.f.AddressDetails.get('CountryCode').patchValue(countryId);

                  if (countryId == 1) {
                    _this33.f.AddressDetails.get('CountryId').patchValue(countryId);

                    _this33.f.AddressDetails.get('Currency').patchValue(1);

                    _this33.f.AddressDetails.get('UOM').patchValue(1);
                  } else {
                    _this33.f.AddressDetails.get('CountryId').patchValue(countryId);

                    _this33.f.AddressDetails.get('Currency').patchValue(2);

                    _this33.f.AddressDetails.get('UOM').patchValue(2);
                  }

                  _this33.f.AddressDetails.get('Address').patchValue(data.Address);

                  _this33.f.AddressDetails.get('CountyName').patchValue(data.CountyName);

                  _this33.f.AddressDetails.get('City').patchValue(data.City);

                  var stateId = _this33.statesList.find(function (x) {
                    return x.StateCode == data.StateCode;
                  }).StateId;

                  _this33.f.AddressDetails.get('StateId').patchValue(stateId);

                  _this33.f.AddressDetails.get('StateName').patchValue(data.StateName);

                  _this33.f.AddressDetails.get('Latitude').patchValue(data.Latitude);

                  _this33.f.AddressDetails.get('Longitude').patchValue(data.Longitude);

                  _this33.mapConstants.CenterLat = data.Latitude;
                  _this33.mapConstants.CenterLon = data.Longitude;
                  _this33.filteredStatesList = _this33.statesList.filter(function (s) {
                    return s.CountryId == countryId;
                  }) || [];
                }
              }
            });
          }
        } // addValidationPrice(){
        //   let required = [Validators.required];
        //   if (this.f.FuelPricingDetails.get('PricingTypeId').value == 1) {
        //     this.f.FuelPricingDetails.get('Code').setValidators(required);
        //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('RackPrice').setValidators(required);
        //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
        //   }if(this.f.FuelPricingDetails.get('PricingTypeId').value == 4){
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators(required);
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('PricePerGallon').setValidators([]);
        //     this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('Code').setValidators([]);
        //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
        //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
        //   } else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 2){
        //     this.f.FuelPricingDetails.get('PricePerGallon').setValidators(required);
        //     this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('Code').setValidators([]);
        //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
        //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators([]);
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();
        //   }
        // }

      }, {
        key: "getUserContext",
        value: function getUserContext() {
          var _this34 = this;

          this.salesUserService.GetUserContext().subscribe(function (data) {
            _this34.UserContext = data;
          });
        }
      }, {
        key: "onChangeMobile",
        value: function onChangeMobile(event) {
          var _this35 = this;

          if (this.f.CustomerDetails.get('PhoneNumber').value) {
            this.salesUserService.IsPhoneNumberValid(this.f.CustomerDetails.get('PhoneNumber').value).subscribe(function (data) {
              _this35.isValidMobile = data;
            });
          } else {
            this.isValidMobile = true;
          }
        }
      }, {
        key: "markerDragEnd",
        value: function markerDragEnd(event) {
          var _this36 = this;

          this.confirmationDialogService.confirm('Map update', 'Geo Codes shifted to a new location!').then(function (confirmed) {
            return confirmed == true ? _this36.updateGeoCode(event.coords.lat, event.coords.lng) : _this36.previousLatLon();
          })["catch"](function () {
            return _this36.previousLatLon();
          });
        }
      }, {
        key: "updateGeoCode",
        value: function updateGeoCode(lat, lng) {
          var _this37 = this;

          this.salesUserService.GetAddressByLongLat(lat, lng).subscribe(function (data) {
            _this37.updateAddressData(data);
          });
        }
      }, {
        key: "previousLatLon",
        value: function previousLatLon() {
          this.mapConstants.CenterLat = this.f.AddressDetails.get('Latitude').value;
          this.mapConstants.CenterLon = this.f.AddressDetails.get('Longitude').value;
        }
      }, {
        key: "UpdateViewedStatus",
        value: function UpdateViewedStatus() {
          this.salesUserService.UpdateViewedStatus(true, this.editSourcingId).subscribe(function (data) {});
        }
      }, {
        key: "onCancel",
        value: function onCancel() {
          this.router.navigate(['SalesUser/SourcingRequest/Index']);
        }
      }, {
        key: "countryChanged",
        value: function countryChanged() {
          var _this38 = this;

          this.f.AddressDetails.get('StateId').setValue(null);
          this.filteredStatesList = this.statesList.filter(function (s) {
            return s.CountryId == _this38.f.AddressDetails.get('CountryId').value;
          }) || [];
          this.f.AddressDetails.get('CountryId').value == 2 ? this.UoM = 2 : this.UoM = 1;
          this.f.AddressDetails.get('UOM').setValue(this.UoM);
        }
      }, {
        key: "countryGroupChanged",
        value: function countryGroupChanged(selectedCountryGroupId) {
          var _this39 = this;

          if (selectedCountryGroupId) {
            var countryGroup = selectedCountryGroupId.target.value;

            if (countryGroup === 'Select') {
              this.filteredStatesList = this.statesList.filter(function (s) {
                return s.CountryId == _this39.f.AddressDetails.get('CountryId').value;
              }) || [];
              this.f.AddressDetails.get('StateId').setValue(null);
            } else {
              this.filteredStatesList = this.statesList.filter(function (s) {
                return s.CountryGroupId == countryGroup;
              }) || [];
              this.f.AddressDetails.get('StateId').setValue(null);
            }
          }
        }
      }, {
        key: "updateAddressData",
        value: function updateAddressData(address) {
          var countryId = address.CountryCode == 'US' ? 1 : address.CountryCode == 'CA' ? 2 : this.f.AddressDetails.get('CountryId').value;
          var state = this.statesList.find(function (st) {
            return st.StateName.toLowerCase() == address.StateName.toLowerCase();
          });
          var stateId = state && state.StateId ? state.StateId : this.f.AddressDetails.get('StateId').value;
          this.f.AddressDetails.get('Address').patchValue(address.Address);
          this.f.AddressDetails.get('City').patchValue(address.City);
          this.f.AddressDetails.get('ZipCode').patchValue(address.ZipCode);
          this.f.AddressDetails.get('CountyName').patchValue(address.CountyName);
          this.f.AddressDetails.get('CountryCode').patchValue(address.CountryCode);
          this.f.AddressDetails.get('CountryId').patchValue(countryId);
          this.f.AddressDetails.get('StateName').patchValue(address.StateName);
          this.f.AddressDetails.get('StateId').patchValue(stateId);

          if (address.Latitude) {
            this.f.AddressDetails.get('Latitude').patchValue(address.Latitude);
            this.f.AddressDetails.get('Longitude').patchValue(address.Longitude);
          }

          this.filteredStatesList = this.statesList.filter(function (s) {
            return s.CountryId == countryId;
          }) || [];
          this.UoM = countryId == 2 ? 2 : 1;
        } // changePricingCode(){
        //   if(this.f.FuelPricingDetails.get('PricingTypeId').value == 2){
        //     this.f.FuelPricingDetails.get('Code').patchValue('A-120000');
        //     this.f.FuelPricingDetails.get('Code').patchValue('');
        //     this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
        //     this.f.FuelPricingDetails.get('RackPrice').patchValue('');
        //     this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
        //     this.f.FuelPricingDetails.get('TerminalName').patchValue('');
        //   }else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 4){
        //     this.f.FuelPricingDetails.get('Code').patchValue('A-140000');
        //     this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
        //     this.f.FuelPricingDetails.get('RackPrice').patchValue('');
        //     this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
        //     this.f.FuelPricingDetails.get('TerminalName').patchValue('');
        //   }else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 1){
        //     this.f.FuelPricingDetails.get('Code').patchValue('');
        //     // this.f.FuelPricingDetails.get('PricePerGallon').patchValue('');
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').patchValue('');
        //     this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId').patchValue('');
        //   }
        // }

      }, {
        key: "setCurrency",
        value: function setCurrency(Currency) {
          this.UoM = Currency;

          if (Currency == "1") {
            this.displayCurrancy = "USD";
          } else if (Currency == "2") {
            this.displayCurrancy = "CAD";
          }
        } //ToogleNotes(){
        //  this.isShowNote= !this.isShowNote;
        //}

      }, {
        key: "deliveryTypeIdChanged",
        value: function deliveryTypeIdChanged(deliveryTypeId) {
          if (deliveryTypeId == 1) {
            this.f.FuelPricingDetails.get('TierPricing').get('TierPricingType').setValue(2);
          }
        }
      }, {
        key: "updateFormControlValidators",
        value: function updateFormControlValidators(control, validators) {
          control.setValidators(validators);
          control.updateValueAndValidity();
        }
      }, {
        key: "deliveryTypeChanged",
        value: function deliveryTypeChanged(type) {
          this.f.FuelDetails.get('FuelTypeId').setValue(null); //SET VALIDATION

          this.setFuelTypevalidation(type); //GET PRODUCTS

          if (type == 1 || type == 2) {
            this.getFuelProducts();
          } else if (type == 4) {
            this.getProductListByZip();
          }

          if (type == 3 && this.pricingModuleComponent) {
            this.f.FuelPricingDetails.get('PricingTypeId').setValue(2); //DONT UPDATE VALIDATION IF TIER PRICING ALREADY CHECKED

            if (!this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
              this.pricingModuleComponent.pricingTypeChanged(2);
            }
          }
        }
      }, {
        key: "setFuelTypevalidation",
        value: function setFuelTypevalidation(type) {
          //other
          if (type == 3) {
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelTypeId'), []);
            this.updateFormControlValidators(this.f.FuelDetails.get('NonStandardFuelName'), [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
          } //In Location Area//Common//Less common
          else {
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelTypeId'), [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]);
            this.updateFormControlValidators(this.f.FuelDetails.get('NonStandardFuelName'), []);
          }
        }
      }, {
        key: "UpdateSuppressPricing",
        value: function UpdateSuppressPricing(value) {
          this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(value);
          this.setNetDaysValidation(value);
        }
      }, {
        key: "isRegularBuyerUpdate",
        value: function isRegularBuyerUpdate(companyId, isEditRequest) {
          var _this40 = this;

          if (companyId > 0) {
            this.salesUserService.GetValidTPOCompany(companyId).subscribe(function (data) {
              _this40.f.IsRegularBuyer.setValue(!data);

              if (!isEditRequest) {
                if (data) {
                  _this40.sourcingRequestForm.get('IsSupressOrderPricing').setValue(_this40.companyPreferenceSetting);

                  _this40.pricingModuleComponent.toggleSuppressPricing(_this40.companyPreferenceSetting);
                } else {
                  _this40.sourcingRequestForm.get('IsSupressOrderPricing').setValue(false);

                  _this40.pricingModuleComponent.toggleSuppressPricing(false);

                  _this40.f.AddressDetails['controls']['IsNewJob'].setValue(false);

                  _this40.clickNewPerson(false);
                }
              }
            });
          }
        }
      }, {
        key: "ClearAddress",
        value: function ClearAddress() {
          this.f.AddressDetails['controls']['JobId'].setValue(null);
          this.f.AddressDetails['controls']['JobName'].setValue(null);
          this.f.AddressDetails['controls']['DisplayJobID'].setValue(null);
          this.f.AddressDetails['controls']['Address'].setValue(null);
          this.f.AddressDetails['controls']['ZipCode'].setValue(null);
          this.f.AddressDetails['controls']['City'].setValue(null);
          this.f.AddressDetails['controls']['CountyName'].setValue(null);
          this.f.AddressDetails['controls']['StateId'].setValue(null);
          this.f.AddressDetails['controls']['Latitude'].setValue(null);
          this.f.AddressDetails['controls']['Longitude'].setValue(null);
          this.f.AddressDetails['controls']['TimeZoneName'].setValue(null);
        }
      }]);

      return CreateSourcingRequestComponent;
    }();

    CreateSourcingRequestComponent.ɵfac = function CreateSourcingRequestComponent_Factory(t) {
      return new (t || CreateSourcingRequestComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_sales_user_service__WEBPACK_IMPORTED_MODULE_7__["SalesUserService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_8__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_8__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_9__["ConfirmationDialogService"]));
    };

    CreateSourcingRequestComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CreateSourcingRequestComponent,
      selectors: [["app-create-sourcing-request"]],
      viewQuery: function CreateSourcingRequestComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](src_app_shared_components_pricing_section_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"], true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](_c0, true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.pricingModuleComponent = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.approveTerminalAuto = _t.first);
        }
      },
      decls: 431,
      vars: 119,
      consts: [["class", "loader", 4, "ngIf"], [1, "create-sourcing-request-container"], [3, "formGroup"], [1, "row"], [1, "col-9"], ["formGroupName", "CustomerDetails", 1, "row"], [1, "col-12"], [1, "well", "col-sm-12"], [1, "col-4"], [1, "badge", "badge-pill", "badge-success", "bg-success", "shadow-sm", "animated", "flash", "infinite"], [4, "ngIf"], [1, "col-8"], [1, "float-right"], ["id", "Submit", "type", "button", "value", "Create PO", "mwlConfirmationPopover", "", "placement", "left", "class", "btn btn-lg btn-primary btnSubmit", 3, "popoverTitle", "popoverMessage", "confirm", "cancel", 4, "ngIf"], ["id", "Accept", "type", "button", "value", "Accept", "class", "btn btn-lg btn-primary btnSubmit", 3, "click", 4, "ngIf"], ["id", "lost", "type", "button", "value", "Lost", "mwlConfirmationPopover", "", "placement", "left", "class", "btn btn-lg btn-danger btnSubmit", 3, "popoverTitle", "popoverMessage", "confirm", "cancel", 4, "ngIf"], [1, "col-sm-12"], [1, "form-check", "form-check-inline"], ["formControlName", "IsNewCompany", "id", "radio-newcustomer", "type", "radio", 1, "form-check-input", 3, "value", "change"], ["for", "radio-newcustomer", 1, "form-check-label"], ["id", "radio-existingcustomer", "formControlName", "IsNewCompany", "type", "radio", 1, "form-check-input", 3, "value", "change"], ["for", "radio-existingcustomer", 1, "form-check-label"], [1, "row", "mt-2"], [1, "col-sm-4", "col-md-3"], [1, "form-group", "mb0"], ["for", "CustomerDetails_CompanyName"], ["aria-required", "true", 1, "required", "pl4"], ["class", "mtm1 new-CompanyName", 4, "ngIf"], ["class", "existing-CompanyName", 4, "ngIf"], [1, "col-sm-8", "col-md-9"], ["class", "col-sm-4 new-company", 4, "ngIf"], ["class", "col-sm-4 existing-company ", 4, "ngIf"], [1, "col-sm-4"], [1, "form-group"], ["for", "CustomerDetails_PhoneNumber"], ["id", "CustomerDetails_PhoneNumber", "name", "PhoneNumber", "formControlName", "PhoneNumber", "type", "text", "value", "", 1, "form-control", "input-phoneformat", "phoneNumber", 3, "blur"], ["class", "color-orange fs12 pt5", "id", "mobile-validation-msg", 4, "ngIf"], [1, "form-group", "custEmail"], ["for", "CustomerDetails_Email"], ["id", "CustomerDetails_Email", "name", "Email", "formControlName", "Email", "type", "email", "value", "", 1, "form-control", "email"], [1, "col-sm-3"], ["for", "TruckLoadType"], ["name", "RequestType", "formControlName", "TruckLoadType", 1, "form-control"], [3, "id", "value", "selected", 4, "ngFor", "ngForOf"], ["for", "FreightOnBoardType"], ["name", "FOB", "formControlName", "FreightOnBoardType", 1, "form-control", 3, "disableControl"], ["for", "AccountingCompanyId"], ["id", "AccountingCompanyId", "name", "AccountingCompanyId", "formControlName", "AccountingCompanyId", "type", "text", "value", "", 1, "form-control"], ["for", "RequestName"], ["id", "RequestName", "name", "Name", "formControlName", "RequestName", "type", "text", "value", "", 1, "form-control"], ["formGroupName", "AddressDetails", 1, "row"], [1, "well"], [1, "job-site-info"], ["class", "form-check form-check-inline radio", 4, "ngIf"], ["class", "form-check form-check-inline existingjobSection radio", 4, "ngIf"], [1, "col-sm-6"], ["class", "new-job", 4, "ngIf"], ["class", "existing-job defaultDisabled", 4, "ngIf"], [1, "address-controls", 3, "ngClass"], [1, "form-group", "combineDiv"], ["for", "AddressDetails_DisplayJobID", 1, "job-site-info"], ["id", "AddressDetails_DisplayJobID", "formControlName", "DisplayJobID", "type", "text", "value", "", 1, "form-control"], [1, "row", "address-controls", 3, "ngClass"], ["for", "AddressDetails_Country_Id"], ["id", "AddressDetails_Country_Id", "formControlName", "CountryId", 1, "form-control", "country", "addressInput", 3, "change"], ["class", "col-sm-4", 4, "ngIf"], [1, "col-sm-8"], [1, "col-md-6"], ["id", "ddl-currency-filter-div", 1, "form-group"], ["for", "AddressDetails_Country_Currency"], ["id", "AddressDetails_Country_Currency", "name", "AddressDetails.Country.Currency", "formControlName", "Currency", 1, "form-control", "currency", "valid", 3, "change"], [1, "col-md-6", "uom-section"], ["for", "AddressDetails_Country_UoM"], ["data-toggle", "tooltip", "data-placement", "top", "title", "Unit Of Measurement", "data-original-title", "Unit Of Measurement", 1, "fa", "fa-info-circle", "ml5"], ["id", "AddressDetails_Country_UoM", "formControlName", "UOM", "readonly", "true", 1, "form-control"], [1, "col-md-6", "mfn-uom-section", "hide-element"], ["id", "AddressDetails_MarineUoM", "formControlName", "UOM", 1, "form-control"], ["value", "1"], ["value", "2"], ["value", "3"], ["value", "4"], [1, "row", "mt20"], [1, "col-sm-7"], [1, "wrapper-location", 2, "display", "block"], ["for", "AddressDetails_Address"], ["id", "AddressDetails_Address", "name", "AddressDetails.Address", "type", "text", "formControlName", "Address", "value", "", 1, "form-control", "address", "addressInput"], ["for", "AddressDetails_ZipCode"], ["id", "AddressDetails_ZipCode_an", "name", "AddressDetails.ZipCode", "formControlName", "ZipCode", "type", "text", "value", "", 1, "form-control", 3, "change"], ["for", "AddressDetails_City"], ["id", "AddressDetails_City", "formControlName", "City", "type", "text", "value", "", 1, "form-control", "city", "addressInput"], ["for", "AddressDetails_CountyName"], ["data-toggle", "tooltip", "data-placement", "top", "title", "Correct County name is required by our Tax service to calculate taxes accurately.", 1, "fa", "fa-info-circle", "ml5"], ["id", "AddressDetails_CountyName", "formControlName", "CountyName", "type", "text", "value", "", "autocomplete", "off", 1, "form-control", "county", "addressInput"], ["for", "AddressDetails_State_Id"], ["id", "AddressDetails_State_Id", "name", "AddressDetails.State.Id", "formControlName", "StateId", 1, "form-control", "state", "addressInput", "triggerTerminalChange", 3, "change"], ["value", ""], [1, "col-md-12"], [1, "form-check", "form-group", "form-check-inline"], ["id", "checkbox-geocodes", "formControlName", "IsGeocodeUsed", "type", "checkbox", 1, "form-check-input", 3, "value", "disableControl"], ["for", "checkbox-geocodes", 1, "form-check-label"], [1, "col-xs-6", "col-md-4", "combineDiv"], ["for", "AddressDetails_Latitude"], ["id", "AddressDetails_Latitude", "name", "AddressDetails.Latitude", "formControlName", "Latitude", "type", "text", "value", "0", 1, "form-control", "datatype-decimal", "mask-decimal", "latitude", "geoInput", "defaultDisabled", 3, "readonly", "disableControl"], ["for", "AddressDetails_Longitude"], ["id", "AddressDetails_Longitude", "formControlName", "Longitude", "type", "text", "value", "0", 1, "form-control", "datatype-decimal", "mask-decimal", "longitude", "geoInput", "defaultDisabled", 3, "readonly", "disableControl"], [1, "col-md-4"], ["for", "AddressDetails_TimeZoneName"], ["id", "AddressDetails_TimeZoneName", "formControlName", "TimeZoneName", "readonly", "true", "type", "text", "value", "", 1, "form-control", "timeZoneName", "defaultDisabled", 3, "disableControl"], [1, "col-sm-5"], [3, "zoom", "latitude", "longitude"], [3, "latitude", "longitude", "markerDraggable", "iconUrl", "dragEnd"], [1, "form-group", "col-sm-2"], ["for", "DispatchRegion"], ["id", "AddressDetails_DispatchRegionId", "formControlName", "DispatchRegionId", 1, "form-control"], ["disabled", "", 3, "value"], [3, "id", "value", 4, "ngFor", "ngForOf"], ["formGroupName", "FuelDeliveryDetails", 1, "row"], [1, "col-sm-12", "mb5"], ["type", "radio", "formControlName", "DeliveryTypeId", "id", "FuelDeliveryDetails_single_DeliveryTypeId", 1, "form-check-input", "single-delivery-schedule", 3, "value", "change"], ["for", "FuelDeliveryDetails_single_DeliveryTypeId", 1, "form-check-label"], ["type", "radio", "formControlName", "DeliveryTypeId", "id", "FuelDeliveryDetails_multi_DeliveryTypeId", 1, "form-check-input", "multiple-delivery-schedule", 3, "value", "change"], ["for", "FuelDeliveryDetails_multi_DeliveryTypeId", 1, "form-check-label"], ["class", "col-sm-3 single-delivery", 4, "ngIf"], ["for", "FuelDeliveryDetails_StartDate"], ["type", "text", "formControlName", "StartDate", "myDatePicker", "", 1, "form-control", "datepicker", 3, "maxDate", "format", "onDateChange"], [1, "end-date"], ["for", "End_Date"], ["id", "FuelDeliveryDetails_EndDate", "formControlName", "EndDate", "type", "text", "value", "", "myDatePicker", "", 1, "form-control", "datepicker", 3, "format", "maxDate", "disableControl", "onDateChange"], ["for", "FuelDeliveryDetails_StartTime"], ["type", "text", "formControlName", "StartTime", "myTimePicker", "", 1, "form-control", "timepicker", 3, "format", "onTimeChange"], ["for", "FuelDeliveryDetails_EndTime"], ["type", "text", "formControlName", "EndTime", "myTimePicker", "", 1, "form-control", "timepicker", 3, "format", "onTimeChange"], ["formGroupName", "FuelDetails"], [1, "col-md-12", "disable-in-tpo-edit", "fuel-display-group"], [1, "mb0", "pb10"], [1, "form-check", "form-check-inline", "in-job-area"], ["type", "radio", "formControlName", "FuelDisplayGroupId", "id", "FuelDetails_FuelDisplayGroupId_location", 1, "form-check-input", 3, "value", "change"], ["for", "FuelDetails_FuelDisplayGroupId_location", 1, "form-check-label"], ["type", "radio", "formControlName", "FuelDisplayGroupId", "id", "FuelDetails_FuelDisplayGroupId_common", 1, "form-check-input", 3, "value", "change"], ["for", "FuelDetails_FuelDisplayGroupId_common", 1, "form-check-label"], ["type", "radio", "formControlName", "FuelDisplayGroupId", "id", "FuelDetails_FuelDisplayGroupId_less_common", 1, "form-check-input", 3, "value", "change"], ["for", "FuelDetails_FuelDisplayGroupId_less_common", 1, "form-check-label"], ["type", "radio", "formControlName", "FuelDisplayGroupId", "id", "FuelDetails_FuelDisplayGroupId_other", 1, "form-check-input", 3, "value", "change"], ["for", "FuelDetails_FuelDisplayGroupId_other", 1, "form-check-label"], [1, "col-sm-12", "disable-in-tpo-edit"], ["class", "pa bg-white top0 left0 z-index5 loading-wrapper mtm10 loader-fueltype", 4, "ngIf"], ["class", "row standard-fuels", 4, "ngIf"], ["class", "non-standard-fuels", 4, "ngIf"], [1, "container", "ml0"], ["id", "radio-quantityfixed", "formControlName", "QuantityTypeId", "type", "radio", 1, "revalidate", "quantity-type-id", "form-check-input", 3, "value"], ["for", "radio-quantityfixed", 1, "form-check-label"], ["id", "radio-quantityrange", "formControlName", "QuantityTypeId", "type", "radio", 1, "revalidate", "form-check-input", 3, "value"], ["for", "radio-quantityrange", 1, "form-check-label"], ["checked", "checked", "id", "quantitynotspecified", "formControlName", "QuantityTypeId", "type", "radio", 1, "revalidate", "form-check-input", 3, "value"], ["for", "quantitynotspecified", 1, "form-check-label"], ["class", "col-sm-3 fixed-quantity", 4, "ngIf"], ["class", "col-sm-6", 4, "ngIf"], [1, "col-sm-3", "ftl-controls", "ftl-billable-quantity", "mt5"], ["id", "quantity-warning-message", 1, "text-danger", "col-12", "fs12", "mt5"], [1, "qty-ind-err", "hide-element", "warning", 2, "display", "none"], [3, "locationForm", "formSubmited", "UoM", "IsTBD", "UpdateSuppressPricingChange"], ["formGroupName", "FuelDetails", 1, "row"], [1, "col-12", 3, "ngClass"], [3, "Parent", "Fees", "isSales", "Currency"], [1, "form-check", "form-check-inline", "radio"], ["checked", "checked", "id", "radio-netpayment", "formControlName", "PaymentTermId", "type", "radio", 1, "revalidate", "form-check-input", 3, "value"], ["for", "radio-netpayment", 1, "form-check-label"], ["id", "radio-dueonreceipt", "formControlName", "PaymentTermId", "type", "radio", 1, "revalidate", "form-check-input", 3, "value"], ["for", "radio-dueonreceipt", 1, "form-check-label"], ["id", "radio-prepaidfull", "formControlName", "PaymentTermId", "type", "radio", 1, "revalidate", "form-check-input", 3, "value"], ["for", "radio-prepaidfull", 1, "form-check-label"], [1, "row", "mt5"], [1, "col-sm-4", "netdays"], ["id", "FuelOfferDetails_NetDays", "formControlName", "NetDays", "type", "text", 1, "form-control", "always", "datatype-decimal", 3, "value", "disableControl"], [1, "col-sm-8", "pl0", "pt8", "fs12"], ["id", "FuelDeliveryDetails_PaymentMethods", "formControlName", "PaymentMethods", 1, "form-control"], [1, "row", "notes-section"], [1, "col-8", "form-group"], ["id", "GeneralNote", "formControlName", "GeneralNote", 1, "form-control", "col-12"], [1, "list-group"], [4, "ngFor", "ngForOf"], [1, "col-3"], ["class", "row", "formGroupName", "AddressDetails", 4, "ngIf"], [1, "col-sm-12", "pr-0"], ["formGroupName", "CustomerDetails"], [1, "form-check", "mb-1"], ["type", "checkbox", "formControlName", "IsNotifyDeliveries", "id", "Chk-IsNotifyDeliveries", "value", "option1", "checked", "", 1, "form-check-input"], ["for", "Chk-IsNotifyDeliveries", 1, "form-check-label"], ["type", "checkbox", "formControlName", "IsNotifySchedules", "id", "Chk-IsNotifySchedules", "value", "option1", 1, "form-check-input"], ["for", "Chk-IsNotifySchedules", 1, "form-check-label"], ["type", "checkbox", "formControlName", "IsInvitationEnabled", "id", "Chk-IsInvitationEnabled", "value", "option1", 1, "form-check-input"], ["for", "Chk-IsInvitationEnabled", 1, "form-check-label"], ["formGroupName", "AddressDetails"], ["type", "checkbox", "formControlName", "IsProFormaPoEnabled", "id", "Chk-AddressDetails_IsProFormaPoEnabled", "value", "option1", 1, "form-check-input"], ["for", "Chk-AddressDetails_IsProFormaPoEnabled", 1, "form-check-label"], ["class", "row", 4, "ngIf"], [1, "mt0"], ["type", "radio", "formControlName", "OrderEnforcementId", "id", "Radios-FuelDeliveryDetails_OrderEnforcementId", "checked", "", 1, "form-check-input", 3, "value"], ["for", "Radios-FuelDeliveryDetails_OrderEnforcementId", 1, "form-check-label"], ["type", "radio", "formControlName", "OrderEnforcementId", "id", "Radios-FuelDeliveryDetails_OrderEnforcementId_mange", 1, "form-check-input", 3, "value"], ["for", "Radios-FuelDeliveryDetails_OrderEnforcementId_mange", 1, "form-check-label"], ["type", "radio", "formControlName", "OrderEnforcementId", "id", "Radios-FuelDeliveryDetails_OrderEnforcementId_no", 1, "form-check-input", 3, "value"], ["for", "Radios-FuelDeliveryDetails_OrderEnforcementId_no", 1, "form-check-label"], [1, "fixed-footer-panel"], [1, "col-sm-9", "text-right"], ["type", "button", "value", "Cancel", 1, "btn", 3, "click"], ["id", "Submit", "type", "button", "value", "Submit", 1, "btn", "btn-lg", "btn-primary", "btnSubmit", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], ["id", "Submit", "type", "button", "value", "Create PO", "mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-lg", "btn-primary", "btnSubmit", 3, "popoverTitle", "popoverMessage", "confirm", "cancel"], ["id", "Accept", "type", "button", "value", "Accept", 1, "btn", "btn-lg", "btn-primary", "btnSubmit", 3, "click"], ["id", "lost", "type", "button", "value", "Lost", "mwlConfirmationPopover", "", "placement", "left", 1, "btn", "btn-lg", "btn-danger", "btnSubmit", 3, "popoverTitle", "popoverMessage", "confirm", "cancel"], [1, "mtm1", "new-CompanyName"], ["id", "CustomerDetails_CompanyName", "formControlName", "CompanyName", "placeholder", "Company", "type", "text", "value", "", 1, "form-control", 3, "change"], ["class", "text-danger", 4, "ngIf"], [1, "text-danger"], [1, "existing-CompanyName"], ["formControlName", "TempCompany", 1, "single-select", 2, "width", "100%", 3, "settings", "data", "onSelect", "onDeSelect"], [1, "col-sm-4", "new-company"], ["id", "newContactPersonInput"], ["for", "CustomerDetails_Name"], ["id", "CustomerDetails_Name", "formControlName", "Name", "placeholder", "Contact Person", "type", "text", "value", "", 1, "form-control", "newContactPerson"], ["id", "useExisting", "class", "", 3, "click", 4, "ngIf"], ["id", "useExisting", 1, "", 3, "click"], [1, "fa", "fa-arrow-left", "mt7"], [1, "col-sm-4", "existing-company"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "customer-contact", "hide-element"], [1, "spinner-dashboard", "pa"], ["id", "existingContactPersonDropdown"], ["id", "CustomerDetails_UserId", "formControlName", "UserId", 1, "form-control", "existingContactPerson", 3, "change"], ["value", "null", "disabled", ""], ["id", "createNew", 3, "click", 4, "ngIf"], [3, "id", "value", "selected"], ["id", "createNew", 3, "click"], [1, "fa", "fa-plus-circle", "mt7"], ["id", "mobile-validation-msg", 1, "color-orange", "fs12", "pt5"], [3, "Parent"], ["id", "radio-newjob", "formControlName", "IsNewJob", "type", "radio", 1, "jobname", "form-check-input", 3, "value", "click"], ["for", "radio-newjob", 1, "form-check-label"], [1, "form-check", "form-check-inline", "existingjobSection", "radio"], ["id", "radio-existingjob", "formControlName", "IsNewJob", "type", "radio", 1, "jobname", "existingjob", "form-check-input", 3, "value", "click"], ["for", "radio-existingjob", 1, "form-check-label"], [1, "new-job"], ["for", "AddressDetails_JobName", 1, "job-site-info"], ["id", "AddressDetails_JobName", "formControlName", "JobName", "type", "text", "value", "", 1, "form-control"], [1, "existing-job", "defaultDisabled"], ["for", "AddressDetails_JobId", 1, "job-site-info"], ["formControlName", "TempJob", 1, "single-select", 2, "width", "100%", 3, "settings", "data", "onSelect", "onDeSelect"], ["id", "AddressDetails_Country_Id", 1, "form-control", "country-group", 3, "change"], ["id", "0", "value", "Select"], [3, "id", "value"], [1, "col-sm-3", "single-delivery"], [1, "single-delivery-sub-types"], ["for", "FuelDeliveryDetails_SingleDeliverySubTypes"], ["id", "FuelDeliveryDetails_SingleDeliverySubTypes", "formControlName", "SingleDeliverySubTypes", 1, "form-control"], [3, "value"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "mtm10", "loader-fueltype"], [1, "row", "standard-fuels"], [1, "col-sm-3", "clearboth"], ["id", "FuelDetails_FuelTypeId", "formControlName", "FuelTypeId", 1, "form-control"], [1, "non-standard-fuels"], ["for", "FuelDetails_NonStandardFuelName"], ["id", "FuelDetails_NonStandardFuelName", "formControlName", "NonStandardFuelName", "type", "text", "value", "", "autocomplete", "off", 1, "form-control"], [1, "col-sm-3", "mb15"], ["for", "FuelDetails_NonStandardFuelDescription"], ["cols", "20", "id", "FuelDetails_NonStandardFuelDescription", "formControlName", "NonStandardFuelDescription", "rows", "2", 1, "form-control"], [1, "col-sm-3", "fixed-quantity"], [1, "form-group", 2, "pointer-events", "visible"], [1, "mt5"], [1, "input-group"], ["id", "FuelDetails_FuelQuantity_Quantity", "formControlName", "Quantity", "type", "text", "value", "0", 1, "form-control", "datatype-decimal", "total-gallons-required"], [1, "input-group-addon", "uom", "quantity-uom"], [1, "resetleftspace"], [1, "row", "mt5", "mb15", "form-group"], [1, "dib"], ["for", "FuelDetails_FuelQuantity_MinimumQuantity"], [1, "input-group", "pull-left"], ["id", "FuelDetails_FuelQuantity_MinimumQuantity", "formControlName", "MinimumQuantity", "type", "text", 1, "form-control", "datatype-decimal", "always", 3, "value"], ["for", "FuelDetails_FuelQuantity_MaximumQuantity"], ["id", "FuelDetails_FuelQuantity_MaximumQuantity", "formControlName", "MaximumQuantity", "type", "text", 1, "form-control", 3, "value"], ["id", "FuelDetails_FuelQuantity_QuantityIndicatorTypes", "formControlName", "QuantityIndicatorTypes", 1, "form-control", "enum-ddl", "qty-ind"], ["href", "javascript:void(0)", 1, "list-group-item", "list-group-item-action", 3, "ngClass"], [1, "d-flex", "w-100", "justify-content-between"], [1, "mb-1", "user-name"], [1, "mb-1"], ["type", "radio", "formControlName", "InventoryDataCaptureType", "id", "Radios-not-specific", "checked", "", 1, "form-check-input", 3, "value"], ["for", "Radios-not-specific", 1, "form-check-label"], ["type", "radio", "formControlName", "InventoryDataCaptureType", "id", "Radios-Connected", 1, "form-check-input", 3, "value"], ["for", "Radios-Connected", 1, "form-check-label"], ["type", "radio", "formControlName", "InventoryDataCaptureType", "id", "Radios-Manual", 1, "form-check-input", 3, "value"], ["for", "Radios-Manual", 1, "form-check-label"], ["type", "radio", "formControlName", "InventoryDataCaptureType", "id", "Radios-Call-in", 1, "form-check-input", 3, "value"], ["for", "Radios-Call-in", 1, "form-check-label"], ["type", "radio", "formControlName", "InventoryDataCaptureType", "id", "Radios-Mixed", 1, "form-check-input", 3, "value"], ["for", "Radios-Mixed", 1, "form-check-label"], ["formGroupName", "AdditionalDetailsViewModel"], ["type", "checkbox", "formControlName", "IsAssetTracked", "id", "Chk-IsAssetTracked", "value", "option1", "checked", "", 1, "form-check-input"], ["for", "Chk-IsAssetTracked", 1, "form-check-label"], ["type", "checkbox", "formControlName", "IsAssetDropStatusEnabled", "id", "Chk-IsAssetDropStatusEnabled", "value", "option1", 1, "form-check-input"], ["for", "Chk-IsAssetDropStatusEnabled", 1, "form-check-label"], ["formGroupName", "FuelDeliveryDetails", 1, "enableStatusForAllAssets"], ["type", "checkbox", "formControlName", "IsPrePostDipRequired", "id", "Chk-FuelDeliveryDetails_IsPrePostDipRequired", "value", "option1", 1, "form-check-input"], ["for", "Chk-FuelDeliveryDetails_IsPrePostDipRequired", 1, "form-check-label"]],
      template: function CreateSourcingRequestComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](0, CreateSourcingRequestComponent_div_0_Template, 3, 0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, CreateSourcingRequestComponent_span_11_Template, 2, 0, "span", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, CreateSourcingRequestComponent_span_12_Template, 2, 0, "span", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, CreateSourcingRequestComponent_span_13_Template, 2, 0, "span", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, CreateSourcingRequestComponent_span_14_Template, 2, 0, "span", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "Customer Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, CreateSourcingRequestComponent_input_19_Template, 1, 2, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, CreateSourcingRequestComponent_input_20_Template, 1, 0, "input", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, CreateSourcingRequestComponent_input_21_Template, 1, 2, "input", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "input", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_25_listener() {
            return ctx.companyExistanceChanged(true);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "label", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "New");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "input", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_29_listener() {
            return ctx.companyExistanceChanged(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "label", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "Existing");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "label", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, " Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "span", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](39, CreateSourcingRequestComponent_div_39_Template, 4, 2, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](40, CreateSourcingRequestComponent_div_40_Template, 3, 3, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](43, CreateSourcingRequestComponent_div_43_Template, 10, 2, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](44, CreateSourcingRequestComponent_div_44_Template, 13, 3, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "label", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](48, "Mobile Number");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "input", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("blur", function CreateSourcingRequestComponent_Template_input_blur_49_listener($event) {
            return ctx.onChangeMobile($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](50, CreateSourcingRequestComponent_div_50_Template, 2, 0, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](51, CreateSourcingRequestComponent_div_51_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "label", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](55, "Email");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](56, "input", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](57, CreateSourcingRequestComponent_div_57_Template, 3, 2, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](59, CreateSourcingRequestComponent_div_59_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](61, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](64, "Sourcing Request");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](66, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "label", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](69, "Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](70, "select", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](71, CreateSourcingRequestComponent_option_71_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](72, CreateSourcingRequestComponent_div_72_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](74, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](75, "label", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](76, "Freight on Board (FOB)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](77, "select", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](78, CreateSourcingRequestComponent_option_78_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](79, CreateSourcingRequestComponent_div_79_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](80, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](81, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](82, "label", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](83, "Accounting Company ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](84, "input", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](85, CreateSourcingRequestComponent_div_85_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](86, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](87, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](88, "label", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](89, "SR Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](90, "input", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](91, CreateSourcingRequestComponent_div_91_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](92, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](93, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](95, "h4", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](96, "Location Information");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](97, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](98, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](99, CreateSourcingRequestComponent_div_99_Template, 4, 1, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](100, CreateSourcingRequestComponent_div_100_Template, 4, 1, "div", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](101, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](103, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](104, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](105, CreateSourcingRequestComponent_div_105_Template, 7, 1, "div", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](106, CreateSourcingRequestComponent_div_106_Template, 7, 3, "div", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](107, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](108, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](109, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](110, "label", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](111, " Third Party Location ID ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](112, "input", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](113, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](114, "div", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](115, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](116, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](117, "label", 63);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](118, "Country/Group");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](119, "select", 64);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_select_change_119_listener() {
            return ctx.countryChanged();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](120, CreateSourcingRequestComponent_option_120_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](121, CreateSourcingRequestComponent_div_121_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](122, CreateSourcingRequestComponent_div_122_Template, 8, 1, "div", 65);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](123, "div", 66);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](124, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](125, "div", 67);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](126, "div", 68);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](127, "label", 69);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](128, "Currency");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](129, "select", 70);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_select_change_129_listener() {
            return ctx.setCurrency(ctx.f.AddressDetails.get("Currency").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](130, CreateSourcingRequestComponent_option_130_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](131, "div", 71);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](132, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](133, "label", 72);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](134, "UOM");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](135, "i", 73);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](136, "select", 74);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](137, CreateSourcingRequestComponent_option_137_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](138, "div", 75);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](139, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](140, "label", 72);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](141, "UOM");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](142, "i", 73);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](143, "select", 76);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](144, "option", 77);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](145, "Gallons");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](146, "option", 78);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](147, "Litres");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](148, "option", 79);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](149, "Barrels");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](150, "option", 80);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](151, "MetricTons");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](152, "div", 81);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](153, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](154, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](155, "div", 82);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](156, "div", 83);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](157, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](158, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](159, "div", 66);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](160, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](161, "label", 84);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](162, "Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](163, "input", 85);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](164, CreateSourcingRequestComponent_div_164_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](165, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](166, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](167, "label", 86);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](168, "Zip");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](169, "input", 87);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_169_listener() {
            return ctx.getAddressByZip();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](170, CreateSourcingRequestComponent_div_170_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](171, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](172, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](173, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](174, "label", 88);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](175, " City");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](176, "span", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](177, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](178, "input", 89);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](179, CreateSourcingRequestComponent_div_179_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](180, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](181, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](182, "label", 90);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](183, " County Name ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](184, "i", 91);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](185, "input", 92);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](186, CreateSourcingRequestComponent_div_186_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](187, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](188, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](189, "label", 93);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](190, " State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](191, "span", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](192, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](193, "select", 94);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_select_change_193_listener() {
            return ctx.stateChanged();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](194, "option", 95);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](195, "Select State");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](196, CreateSourcingRequestComponent_option_196_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](197, CreateSourcingRequestComponent_div_197_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](198, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](199, "div", 96);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](200, "div", 97);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](201, "input", 98);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](202, "label", 99);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](203, "Geo Codes");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](204, "div", 100);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](205, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](206, "label", 101);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](207, "Latitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](208, "input", 102);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](209, "div", 100);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](210, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](211, "label", 103);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](212, "Longitude");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](213, "input", 104);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](214, "div", 105);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](215, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](216, "label", 106);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](217, " Time Zone ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](218, "input", 107);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](219, "div", 108);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](220, "agm-map", 109);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](221, "agm-marker", 110);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dragEnd", function CreateSourcingRequestComponent_Template_agm_marker_dragEnd_221_listener($event) {
            return ctx.markerDragEnd($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](222, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](223, "div", 111);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](224, "label", 112);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](225, "Dispatch Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](226, "select", 113);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](227, "option", 114);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](228, "Select");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](229, CreateSourcingRequestComponent_option_229_Template, 2, 3, "option", 115);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](230, "div", 116);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](231, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](232, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](233, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](234, "Delivery");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](235, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](236, "div", 117);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](237, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](238, "input", 118);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_238_listener() {
            return ctx.deliveryTypeIdChanged(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](239, "label", 119);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](240, "Single Delivery");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](241, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](242, "input", 120);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_242_listener() {
            return ctx.deliveryTypeIdChanged(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](243, "label", 121);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](244, "Multiple Deliveries");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](245, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](246, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](247, CreateSourcingRequestComponent_div_247_Template, 10, 3, "div", 122);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](248, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](249, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](250, "label", 123);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](251, "Start Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](252, "input", 124);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function CreateSourcingRequestComponent_Template_input_onDateChange_252_listener($event) {
            return ctx.f.FuelDeliveryDetails.get("StartDate").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](253, CreateSourcingRequestComponent_div_253_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](254, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](255, "div", 125);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](256, "label", 126);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](257, "End Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](258, "input", 127);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onDateChange", function CreateSourcingRequestComponent_Template_input_onDateChange_258_listener($event) {
            return ctx.f.FuelDeliveryDetails.get("EndDate").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](259, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](260, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](261, "label", 128);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](262, "Start Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](263, "input", 129);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onTimeChange", function CreateSourcingRequestComponent_Template_input_onTimeChange_263_listener($event) {
            return ctx.f.FuelDeliveryDetails.get("StartTime").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](264, CreateSourcingRequestComponent_div_264_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](265, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](266, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](267, "label", 130);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](268, "End Time");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](269, "input", 131);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onTimeChange", function CreateSourcingRequestComponent_Template_input_onTimeChange_269_listener($event) {
            return ctx.f.FuelDeliveryDetails.get("EndTime").setValue($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](270, CreateSourcingRequestComponent_div_270_Template, 2, 1, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](271, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](272, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](273, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](274, 132);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](275, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](276, "div", 133);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](277, "h4", 134);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](278, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](279, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](280, "div", 135);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](281, "input", 136);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_281_listener() {
            return ctx.deliveryTypeChanged(4);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](282, "label", 137);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](283, " In Location Area ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](284, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](285, "input", 138);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_285_listener() {
            return ctx.deliveryTypeChanged(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](286, "label", 139);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](287, "Common");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](288, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](289, "input", 140);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_289_listener() {
            return ctx.deliveryTypeChanged(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](290, "label", 141);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](291, "Less Common");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](292, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](293, "input", 142);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CreateSourcingRequestComponent_Template_input_change_293_listener() {
            return ctx.deliveryTypeChanged(3);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](294, "label", 143);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](295, "Other");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](296, "div", 144);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](297, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](298, CreateSourcingRequestComponent_div_298_Template, 2, 0, "div", 145);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](299, CreateSourcingRequestComponent_div_299_Template, 7, 3, "div", 146);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](300, CreateSourcingRequestComponent_div_300_Template, 14, 0, "div", 147);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](301, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](302, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](303, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](304, "Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](305, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](306, "div", 148);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](307, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](308, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](309, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](310, "input", 149);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](311, "label", 150);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](312, "Fixed");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](313, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](314, "input", 151);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](315, "label", 152);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](316, "Range");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](317, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](318, "input", 153);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](319, "label", 154);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](320, " Not Specified ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](321, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](322, CreateSourcingRequestComponent_div_322_Template, 11, 2, "div", 155);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](323, CreateSourcingRequestComponent_div_323_Template, 25, 2, "div", 156);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](324, "div", 157);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](325, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](326, CreateSourcingRequestComponent_ng_container_326_Template, 5, 1, "ng-container", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](327, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](328, "div", 158);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](329, "span", 159);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](330, " Changes will be updated on BOL. Taxes will continue to be calculated as per state quantity regulations. ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](331, "app-pricing-section", 160);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("UpdateSuppressPricingChange", function CreateSourcingRequestComponent_Template_app_pricing_section_UpdateSuppressPricingChange_331_listener($event) {
            return ctx.UpdateSuppressPricing($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](332, "div", 161);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](333, "div", 162);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](334, "app-fee-list", 163);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](335, "div", 116);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](336, "div", 162);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](337, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](338, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](339, "Payment Details");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](340, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](341, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](342, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](343, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](344, "div", 164);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](345, "input", 165);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](346, "label", 166);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](347, "Net");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](348, "div", 164);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](349, "input", 167);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](350, "label", 168);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](351, " Due On Receipt ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](352, "div", 164);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](353, "input", 169);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](354, "label", 170);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](355, " Pre Paid In Full ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](356, "div", 171);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](357, "div", 172);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](358, "input", 173);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](359, CreateSourcingRequestComponent_div_359_Template, 3, 2, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](360, "div", 174);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](361, " Days of receipt - Upon Credit Approval ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](362, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](363, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](364, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](365, " Payment Method");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](366, "select", 175);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](367, CreateSourcingRequestComponent_option_367_Template, 2, 4, "option", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](368, "div", 176);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](369, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](370, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](371, "h4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](372, "General Notes");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](373, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](374, "div", 177);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](375, "textarea", 178);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](376, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](377, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](378, "div", 179);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](379, CreateSourcingRequestComponent_div_379_Template, 9, 6, "div", 180);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](380, "div", 181);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](381, CreateSourcingRequestComponent_div_381_Template, 26, 5, "div", 182);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](382, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](383, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](384, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](385, "div", 183);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](386, "h3");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](387, "Cutomer");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](388, "div", 184);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](389, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](390, "input", 186);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](391, "label", 187);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](392, " Provide Delivery Details to Customer ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](393, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](394, "input", 188);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](395, "label", 189);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](396, " Provide Schedule Details to Customer ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](397, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](398, "input", 190);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](399, "label", 191);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](400, " Send Invitation Link ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](401, "div", 192);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](402, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](403, "input", 193);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](404, "label", 194);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](405, " Enable After-the-Fact POs (Optional) ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](406, CreateSourcingRequestComponent_div_406_Template, 20, 0, "div", 195);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](407, "div", 116);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](408, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](409, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](410, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](411, "h3", 196);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](412, "Invoice Creation Preference");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](413, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](414, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](415, "input", 197);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](416, "label", 198);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](417, " Enforce Order Level Values ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](418, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](419, "input", 199);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](420, "label", 200);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](421, " Manage Exceptions ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](422, "div", 185);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](423, "input", 201);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](424, "label", 202);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](425, " No Enforcement ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](426, "div", 203);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](427, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](428, "div", 204);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](429, "input", 205);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_Template_input_click_429_listener() {
            return ctx.onCancel();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](430, "input", 206);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function CreateSourcingRequestComponent_Template_input_click_430_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.pageloader);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.sourcingRequestForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Submitted);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Modified);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Accepted || ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.AcceptedAndModified);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Lost);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Accepted || ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.AcceptedAndModified);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["SalesUserId"].value != (ctx.UserContext == null ? null : ctx.UserContext.Id) && (ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Modified || ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Submitted || ctx.sourcingRequestForm.controls["RequestStatus"].value == ctx.RequestStatus.Lost));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["RequestStatus"].value != ctx.RequestStatus.Lost && ctx.editSourcingId > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isPersonNew);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isPersonNew);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.f.CustomerDetails.get("PhoneNumber").errors && !ctx.isValidMobile);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.CustomerDetails.get("PhoneNumber").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.CustomerDetails.get("Email").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.TruckTypeLoadList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.TruckLoadType.errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disableControl", ctx.f.TruckLoadType.value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.FreightOnBoardTypesList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.FreightOnBoardType.errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AccountingCompanyId.errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.RequestName.errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.f.IsRegularBuyer.value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["CustomerDetails"]["controls"]["IsNewCompany"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](109, _c2, ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](111, _c2, ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.countryList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("CountryId").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.f.AddressDetails.get("CountryId").value == "4");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.currucyList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.UomList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](113, _c2, ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("Address").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("ZipCode").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("City").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("CountyName").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.filteredStatesList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.AddressDetails.get("StateId").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", false)("disableControl", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("readonly", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsGeocodeUsed"].value == false)("disableControl", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("readonly", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsGeocodeUsed"].value == false)("disableControl", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsNewJob"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disableControl", ctx.sourcingRequestForm.controls["AddressDetails"]["controls"]["IsGeocodeUsed"].value == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("zoom", ctx.mapConstants.ZoomArea)("latitude", ctx.mapConstants.CenterLat)("longitude", ctx.mapConstants.CenterLon);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("latitude", ctx.mapConstants.CenterLat)("longitude", ctx.mapConstants.CenterLon)("markerDraggable", true)("iconUrl", ctx.mapConstants.IconUrl);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.DispatchRegionList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["FuelDeliveryDetails"]["controls"]["DeliveryTypeId"].value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("maxDate", ctx.MaxInputDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.FuelDeliveryDetails.get("StartDate").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx.MaxInputDate)("disableControl", ctx.f.FuelDeliveryDetails.get("SingleDeliverySubTypes").value == 0 && ctx.f.FuelDeliveryDetails.get("DeliveryTypeId").value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "hh:mm:ss A");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.FuelDeliveryDetails.get("StartTime").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "hh:mm:ss A");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.FuelDeliveryDetails.get("EndTime").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["FuelDetails"]["controls"]["FuelDisplayGroupId"].value != 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["FuelDetails"]["controls"]["FuelDisplayGroupId"].value == 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["FuelDetails"]["controls"]["QuantityTypeId"].value == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.sourcingRequestForm.controls["FuelDetails"]["controls"]["QuantityTypeId"].value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.f.TruckLoadType.value == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("locationForm", ctx.sourcingRequestForm)("formSubmited", ctx.formSubmited)("UoM", ctx.f.AddressDetails.get("UOM").value)("IsTBD", false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](115, _c3, ctx.f.IsSupressOrderPricing.value));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("Parent", ctx.f.FuelDetails)("Fees", ctx.LeadFees)("isSales", true)("Currency", ctx.displayCurrancy);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](117, _c3, ctx.f.IsSupressOrderPricing.value));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 0)("disableControl", ctx.sourcingRequestForm.controls["FuelDeliveryDetails"]["controls"]["PaymentTermId"].value != 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.formSubmited && ctx.f.FuelDeliveryDetails.get("NetDays").errors);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.PaymentMethodsList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.GeneralNotesHistory);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.f.IsRegularBuyer.value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.f.IsRegularBuyer.value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", 3);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _directives_disable_control_directive__WEBPACK_IMPORTED_MODULE_11__["DisableControlDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _agm_core__WEBPACK_IMPORTED_MODULE_12__["AgmMap"], _agm_core__WEBPACK_IMPORTED_MODULE_12__["AgmMarker"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_13__["DatePicker"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_13__["TimePicker"], src_app_shared_components_pricing_section_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"], _fees_fee_list_fee_list_component__WEBPACK_IMPORTED_MODULE_14__["FeeListComponent"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_15__["ɵc"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_16__["MultiSelectComponent"], src_app_contact_person_contact_person_component__WEBPACK_IMPORTED_MODULE_17__["ContactPersonComponent"]],
      styles: [".create-sourcing-request-container .form-group {\n  margin-bottom: 10px;\n}\n.create-sourcing-request-container .fixed-footer-panel {\n  position: fixed;\n  bottom: 0;\n  left: 0;\n  z-index: 10;\n  width: 100%;\n  background: #ffffff;\n  padding: 10px 20px;\n}\n.create-sourcing-request-container .ng-autocomplete {\n  width: 100% !important;\n  display: table;\n  margin: 0 auto;\n}\n.create-sourcing-request-container .ng-autocomplete .input-container {\n  margin-bottom: 10px !important;\n}\n.create-sourcing-request-container .autocomplete-container .suggestions-container ul li a {\n  padding: 8px 15px !important;\n  display: block;\n  text-decoration: none;\n  cursor: pointer;\n  color: rgba(0, 0, 0, 0.87);\n  font-size: 12px !important;\n}\n.create-sourcing-request-container .autocomplete-container {\n  height: 35px !important;\n}\n.create-sourcing-request-container .autocomplete-container .input-container input {\n  height: 35px;\n  line-height: 35px;\n}\n.create-sourcing-request-container agm-map {\n  height: 300px;\n}\n.create-sourcing-request-container .notes-section .list-group {\n  max-height: 300px;\n  overflow-y: auto;\n}\n.create-sourcing-request-container .notes-section .list-group .user-name {\n  font-size: 18px;\n  font-weight: 500;\n}\n.pricingcode-modal .modal-header {\n  padding: 5px 15px;\n}\n.pricingcode-modal .modal-header .close {\n  margin-top: -10px;\n}\n.pricingcode-modal .modal-body {\n  min-height: 450px;\n}\n.pricingcode-modal .radius-capsule .btn {\n  line-height: 20px;\n  padding: 3px 15px !important;\n  font-size: 12px;\n}\n.pricingcode-modal .radius-capsule .btn:not(:last-child) {\n  margin-right: 5px;\n}\n.pricingcode-modal .click-card:hover {\n  cursor: pointer;\n  background: #efefef;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2FsZXMtdXNlci9jcmVhdGUtc291cmNpbmctcmVxdWVzdC9EOlxcVEZTY29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2VcXFNpdGVGdWVsLkV4Y2hhbmdlLlNvdXJjZUNvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlLldlYi9zcmNcXGFwcFxcc2FsZXMtdXNlclxcY3JlYXRlLXNvdXJjaW5nLXJlcXVlc3RcXGNyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0LmNvbXBvbmVudC5zY3NzIiwic3JjL2FwcC9zYWxlcy11c2VyL2NyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0L2NyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0LmNvbXBvbmVudC5zY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUNJO0VBQ0ksbUJBQUE7QUNBUjtBREVJO0VBQ0ksZUFBQTtFQUNBLFNBQUE7RUFDQSxPQUFBO0VBQ0EsV0FBQTtFQUNBLFdBQUE7RUFDQSxtQkFBQTtFQUNBLGtCQUFBO0FDQVI7QURHSTtFQUNJLHNCQUFBO0VBQ0EsY0FBQTtFQUNBLGNBQUE7QUNEUjtBREdNO0VBQ0ksOEJBQUE7QUNEVjtBREdLO0VBQ0csNEJBQUE7RUFDQSxjQUFBO0VBQ0EscUJBQUE7RUFDQSxlQUFBO0VBQ0EsMEJBQUE7RUFDQSwwQkFBQTtBQ0RSO0FER0k7RUFDSSx1QkFBQTtBQ0RSO0FER0k7RUFDSSxZQUFBO0VBQ0EsaUJBQUE7QUNEUjtBRElJO0VBQ0ksYUFBQTtBQ0ZSO0FETVE7RUFDSSxpQkFBQTtFQUNBLGdCQUFBO0FDSlo7QURLWTtFQUNJLGVBQUE7RUFDQSxnQkFBQTtBQ0hoQjtBRFdJO0VBQ0ksaUJBQUE7QUNSUjtBRFNRO0VBQ0ksaUJBQUE7QUNQWjtBRFVJO0VBQ0ksaUJBQUE7QUNSUjtBRFdRO0VBQ0ksaUJBQUE7RUFDQSw0QkFBQTtFQUNBLGVBQUE7QUNUWjtBRFdZO0VBQ0ksaUJBQUE7QUNUaEI7QURlUTtFQUNJLGVBQUE7RUFDQSxtQkFBQTtBQ2JaIiwiZmlsZSI6InNyYy9hcHAvc2FsZXMtdXNlci9jcmVhdGUtc291cmNpbmctcmVxdWVzdC9jcmVhdGUtc291cmNpbmctcmVxdWVzdC5jb21wb25lbnQuc2NzcyIsInNvdXJjZXNDb250ZW50IjpbIi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXJ7XHJcbiAgICAuZm9ybS1ncm91cHtcclxuICAgICAgICBtYXJnaW4tYm90dG9tOiAxMHB4O1xyXG4gICAgfVxyXG4gICAgLmZpeGVkLWZvb3Rlci1wYW5lbHtcclxuICAgICAgICBwb3NpdGlvbjogZml4ZWQ7XHJcbiAgICAgICAgYm90dG9tOiAwO1xyXG4gICAgICAgIGxlZnQ6IDA7XHJcbiAgICAgICAgei1pbmRleDogMTA7XHJcbiAgICAgICAgd2lkdGg6IDEwMCU7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcclxuICAgICAgICBwYWRkaW5nOiAxMHB4IDIwcHg7XHJcbiAgICB9XHJcblxyXG4gICAgLm5nLWF1dG9jb21wbGV0ZSB7XHJcbiAgICAgICAgd2lkdGg6MTAwJSAhaW1wb3J0YW50O1xyXG4gICAgICAgIGRpc3BsYXk6IHRhYmxlO1xyXG4gICAgICAgIG1hcmdpbjogMCBhdXRvO1xyXG4gICAgICB9XHJcbiAgICAgIC5uZy1hdXRvY29tcGxldGUgLmlucHV0LWNvbnRhaW5lcntcclxuICAgICAgICAgIG1hcmdpbi1ib3R0b206IDEwcHggIWltcG9ydGFudDtcclxuICAgICAgfVxyXG4gICAgIC5hdXRvY29tcGxldGUtY29udGFpbmVyIC5zdWdnZXN0aW9ucy1jb250YWluZXIgdWwgbGkgYSB7XHJcbiAgICAgICAgcGFkZGluZzogOHB4IDE1cHggIWltcG9ydGFudDtcclxuICAgICAgICBkaXNwbGF5OiBibG9jaztcclxuICAgICAgICB0ZXh0LWRlY29yYXRpb246IG5vbmU7XHJcbiAgICAgICAgY3Vyc29yOiBwb2ludGVyO1xyXG4gICAgICAgIGNvbG9yOiByZ2JhKDAsMCwwLC44Nyk7XHJcbiAgICAgICAgZm9udC1zaXplOiAxMnB4ICFpbXBvcnRhbnQ7XHJcbiAgICB9XHJcbiAgICAuYXV0b2NvbXBsZXRlLWNvbnRhaW5lcntcclxuICAgICAgICBoZWlnaHQ6IDM1cHggIWltcG9ydGFudDtcclxuICAgIH1cclxuICAgIC5hdXRvY29tcGxldGUtY29udGFpbmVyIC5pbnB1dC1jb250YWluZXIgaW5wdXR7XHJcbiAgICAgICAgaGVpZ2h0OiAzNXB4O1xyXG4gICAgICAgIGxpbmUtaGVpZ2h0OiAzNXB4O1xyXG4gICAgfVxyXG5cclxuICAgIGFnbS1tYXAge1xyXG4gICAgICAgIGhlaWdodDogMzAwcHg7XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIC5ub3Rlcy1zZWN0aW9ue1xyXG4gICAgICAgIC5saXN0LWdyb3Vwe1xyXG4gICAgICAgICAgICBtYXgtaGVpZ2h0OiAzMDBweDtcclxuICAgICAgICAgICAgb3ZlcmZsb3cteTogYXV0bztcclxuICAgICAgICAgICAgLnVzZXItbmFtZXtcclxuICAgICAgICAgICAgICAgIGZvbnQtc2l6ZTogMThweDtcclxuICAgICAgICAgICAgICAgIGZvbnQtd2VpZ2h0OiA1MDA7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgXHJcbiAgICAgIH1cclxufVxyXG5cclxuLnByaWNpbmdjb2RlLW1vZGFse1xyXG4gICAgLm1vZGFsLWhlYWRlcntcclxuICAgICAgICBwYWRkaW5nOiA1cHggMTVweDtcclxuICAgICAgICAuY2xvc2V7XHJcbiAgICAgICAgICAgIG1hcmdpbi10b3A6IC0xMHB4O1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIC5tb2RhbC1ib2R5IHtcclxuICAgICAgICBtaW4taGVpZ2h0OiA0NTBweDtcclxuICAgIH1cclxuICAgIC5yYWRpdXMtY2Fwc3VsZSB7XHJcbiAgICAgICAgLmJ0biB7XHJcbiAgICAgICAgICAgIGxpbmUtaGVpZ2h0OiAyMHB4O1xyXG4gICAgICAgICAgICBwYWRkaW5nOiAzcHggMTVweCAhaW1wb3J0YW50O1xyXG4gICAgICAgICAgICBmb250LXNpemU6IDEycHg7XHJcblxyXG4gICAgICAgICAgICAmOm5vdCg6bGFzdC1jaGlsZCkge1xyXG4gICAgICAgICAgICAgICAgbWFyZ2luLXJpZ2h0OiA1cHg7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgLmNsaWNrLWNhcmR7XHJcbiAgICAgICAgJjpob3ZlcntcclxuICAgICAgICAgICAgY3Vyc29yOiBwb2ludGVyO1xyXG4gICAgICAgICAgICBiYWNrZ3JvdW5kOiAjZWZlZmVmO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufSAiLCIuY3JlYXRlLXNvdXJjaW5nLXJlcXVlc3QtY29udGFpbmVyIC5mb3JtLWdyb3VwIHtcbiAgbWFyZ2luLWJvdHRvbTogMTBweDtcbn1cbi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXIgLmZpeGVkLWZvb3Rlci1wYW5lbCB7XG4gIHBvc2l0aW9uOiBmaXhlZDtcbiAgYm90dG9tOiAwO1xuICBsZWZ0OiAwO1xuICB6LWluZGV4OiAxMDtcbiAgd2lkdGg6IDEwMCU7XG4gIGJhY2tncm91bmQ6ICNmZmZmZmY7XG4gIHBhZGRpbmc6IDEwcHggMjBweDtcbn1cbi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXIgLm5nLWF1dG9jb21wbGV0ZSB7XG4gIHdpZHRoOiAxMDAlICFpbXBvcnRhbnQ7XG4gIGRpc3BsYXk6IHRhYmxlO1xuICBtYXJnaW46IDAgYXV0bztcbn1cbi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXIgLm5nLWF1dG9jb21wbGV0ZSAuaW5wdXQtY29udGFpbmVyIHtcbiAgbWFyZ2luLWJvdHRvbTogMTBweCAhaW1wb3J0YW50O1xufVxuLmNyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0LWNvbnRhaW5lciAuYXV0b2NvbXBsZXRlLWNvbnRhaW5lciAuc3VnZ2VzdGlvbnMtY29udGFpbmVyIHVsIGxpIGEge1xuICBwYWRkaW5nOiA4cHggMTVweCAhaW1wb3J0YW50O1xuICBkaXNwbGF5OiBibG9jaztcbiAgdGV4dC1kZWNvcmF0aW9uOiBub25lO1xuICBjdXJzb3I6IHBvaW50ZXI7XG4gIGNvbG9yOiByZ2JhKDAsIDAsIDAsIDAuODcpO1xuICBmb250LXNpemU6IDEycHggIWltcG9ydGFudDtcbn1cbi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXIgLmF1dG9jb21wbGV0ZS1jb250YWluZXIge1xuICBoZWlnaHQ6IDM1cHggIWltcG9ydGFudDtcbn1cbi5jcmVhdGUtc291cmNpbmctcmVxdWVzdC1jb250YWluZXIgLmF1dG9jb21wbGV0ZS1jb250YWluZXIgLmlucHV0LWNvbnRhaW5lciBpbnB1dCB7XG4gIGhlaWdodDogMzVweDtcbiAgbGluZS1oZWlnaHQ6IDM1cHg7XG59XG4uY3JlYXRlLXNvdXJjaW5nLXJlcXVlc3QtY29udGFpbmVyIGFnbS1tYXAge1xuICBoZWlnaHQ6IDMwMHB4O1xufVxuLmNyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0LWNvbnRhaW5lciAubm90ZXMtc2VjdGlvbiAubGlzdC1ncm91cCB7XG4gIG1heC1oZWlnaHQ6IDMwMHB4O1xuICBvdmVyZmxvdy15OiBhdXRvO1xufVxuLmNyZWF0ZS1zb3VyY2luZy1yZXF1ZXN0LWNvbnRhaW5lciAubm90ZXMtc2VjdGlvbiAubGlzdC1ncm91cCAudXNlci1uYW1lIHtcbiAgZm9udC1zaXplOiAxOHB4O1xuICBmb250LXdlaWdodDogNTAwO1xufVxuXG4ucHJpY2luZ2NvZGUtbW9kYWwgLm1vZGFsLWhlYWRlciB7XG4gIHBhZGRpbmc6IDVweCAxNXB4O1xufVxuLnByaWNpbmdjb2RlLW1vZGFsIC5tb2RhbC1oZWFkZXIgLmNsb3NlIHtcbiAgbWFyZ2luLXRvcDogLTEwcHg7XG59XG4ucHJpY2luZ2NvZGUtbW9kYWwgLm1vZGFsLWJvZHkge1xuICBtaW4taGVpZ2h0OiA0NTBweDtcbn1cbi5wcmljaW5nY29kZS1tb2RhbCAucmFkaXVzLWNhcHN1bGUgLmJ0biB7XG4gIGxpbmUtaGVpZ2h0OiAyMHB4O1xuICBwYWRkaW5nOiAzcHggMTVweCAhaW1wb3J0YW50O1xuICBmb250LXNpemU6IDEycHg7XG59XG4ucHJpY2luZ2NvZGUtbW9kYWwgLnJhZGl1cy1jYXBzdWxlIC5idG46bm90KDpsYXN0LWNoaWxkKSB7XG4gIG1hcmdpbi1yaWdodDogNXB4O1xufVxuLnByaWNpbmdjb2RlLW1vZGFsIC5jbGljay1jYXJkOmhvdmVyIHtcbiAgY3Vyc29yOiBwb2ludGVyO1xuICBiYWNrZ3JvdW5kOiAjZWZlZmVmO1xufSJdfQ== */"],
      encapsulation: 2
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CreateSourcingRequestComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-create-sourcing-request',
          templateUrl: './create-sourcing-request.component.html',
          styleUrls: ['./create-sourcing-request.component.scss'],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _sales_user_service__WEBPACK_IMPORTED_MODULE_7__["SalesUserService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_8__["ActivatedRoute"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_8__["Router"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]
        }, {
          type: _shared_components_confirmation_dialog_confirmation_dialog_service__WEBPACK_IMPORTED_MODULE_9__["ConfirmationDialogService"]
        }];
      }, {
        pricingModuleComponent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [src_app_shared_components_pricing_section_pricing_section_component__WEBPACK_IMPORTED_MODULE_4__["PricingSectionComponent"]]
        }],
        approveTerminalAuto: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['approveTerminalAuto']
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/sales-user/sales-user-dashboard/sales-user-dashboard.component.ts": function srcAppSalesUserSalesUserDashboardSalesUserDashboardComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesUserDashboardComponent", function () {
      return SalesUserDashboardComponent;
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


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _sales_user_model__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../sales-user.model */
    "./src/app/sales-user/sales-user.model.ts");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_6__);
    /* harmony import */


    var _sales_user_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! ../sales-user.service */
    "./src/app/sales-user/sales-user.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! src/app/fuelsurcharge/services/fuelsurcharge.service */
    "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! ng-multiselect-dropdown */
    "./node_modules/ng-multiselect-dropdown/__ivy_ngcc__/fesm2015/ng-multiselect-dropdown.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");

    function SalesUserDashboardComponent_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_11_Template(rf, ctx) {
      if (rf & 1) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "i", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_div_11_Template_i_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r16);

          var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r15.toggleArrow();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        var _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "i", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_div_12_Template_i_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r18);

          var ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r17.toggleArrow();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_22_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Company is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_div_22_span_1_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r3.SalesDRForm.controls == null ? null : ctx_r3.SalesDRForm.controls.CompanyList == null ? null : ctx_r3.SalesDRForm.controls.CompanyList.errors == null ? null : ctx_r3.SalesDRForm.controls.CompanyList.errors.required);
      }
    }

    function SalesUserDashboardComponent_div_30_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Ship-to location is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_30_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_div_30_span_1_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r4.SalesDRForm.controls == null ? null : ctx_r4.SalesDRForm.controls.SiteList == null ? null : ctx_r4.SalesDRForm.controls.SiteList.errors == null ? null : ctx_r4.SalesDRForm.controls.SiteList.errors.required);
      }
    }

    function SalesUserDashboardComponent_div_31_div_10_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Product is Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_31_div_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_div_31_div_10_span_1_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().index;

        var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r24.SalesDRForm.controls == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.FuelTypes == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.FuelTypes.errors == null ? null : ctx_r24.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.FuelTypes.errors.required) ? true : false);
      }
    }

    function SalesUserDashboardComponent_div_31_div_17_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Quantity is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_31_div_17_span_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " Quantity is Invalid ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_31_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_div_31_div_17_span_1_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](2, SalesUserDashboardComponent_div_31_div_17_span_2_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().index;

        var ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r25.SalesDRForm.controls == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity.errors == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity.errors.required) ? true : false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r25.SalesDRForm.controls == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity.errors == null ? null : ctx_r25.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity.errors.pattern) ? true : false);
      }
    }

    function SalesUserDashboardComponent_div_31_div_32_span_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "span", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1, " UoM is required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_31_div_32_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_div_31_div_32_span_1_Template, 2, 0, "span", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().index;

        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", (ctx_r26.SalesDRForm.controls == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.UoM == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.UoM.errors == null ? null : ctx_r26.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.UoM.errors.required) ? true : false);
      }
    }

    function SalesUserDashboardComponent_div_31_a_52_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "a", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "i", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_div_31_a_52_Template_i_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r37);

          var i_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]().index;

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r35.removeProduct(i_r22);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_31_Template(rf, ctx) {
      if (rf & 1) {
        var _r39 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "label", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Product");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "span", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](9, "ng-multiselect-dropdown", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](10, SalesUserDashboardComponent_div_31_div_10_Template, 2, 1, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](12, "label", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](13, "Quantity");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "span", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](15, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](16, "input", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](17, SalesUserDashboardComponent_div_31_div_17_Template, 3, 2, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "div", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "label", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](20, "UoM");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "span", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](22, "*");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "select", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "option", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Gallons");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "option", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27, "Litres");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "option", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](29, "Barrels");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "option", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "MetricTons");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](32, SalesUserDashboardComponent_div_31_div_32_Template, 2, 1, "div", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "label", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, "Delivery Level PO#");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](36, "input", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "label", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "Start Date ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](41, "input", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onDateChange", function SalesUserDashboardComponent_div_31_Template_input_onDateChange_41_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r39);

          var i_r22 = ctx.index;

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r38.SetStartDate($event, i_r22);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "label", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](44, "Start Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "input", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onTimeChange", function SalesUserDashboardComponent_div_31_Template_input_onTimeChange_45_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r39);

          var i_r22 = ctx.index;

          var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r40.SetStartTime($event, i_r22);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](46, "div", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "label", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](48, "End Time");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "input", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onTimeChange", function SalesUserDashboardComponent_div_31_Template_input_onTimeChange_49_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r39);

          var i_r22 = ctx.index;

          var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

          return ctx_r41.SetEndTime($event, i_r22);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](50, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](52, SalesUserDashboardComponent_div_31_a_52_Template, 2, 0, "a", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r22 = ctx.index;

        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroupName", i_r22);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Product")("settings", ctx_r5.SingleSelectSettingsById)("data", ctx_r5.FuelTypeList);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.formSubmitted && (ctx_r5.SalesDRForm.controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.FuelTypes == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.FuelTypes.errors) ? true : false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.formSubmitted && (ctx_r5.SalesDRForm.controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.Quantity.errors) ? true : false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx_r5.formSubmitted && (ctx_r5.SalesDRForm.controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22] == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.UoM == null ? null : ctx_r5.SalesDRForm.controls.AdditionalProducts.controls[i_r22].controls.UoM.errors) ? true : false);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "MM/DD/YYYY")("maxDate", ctx_r5.maxDate)("minDate", ctx_r5.minDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "hh:mm A");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("format", "hh:mm A");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", i_r22 > 0);
      }
    }

    function SalesUserDashboardComponent_div_50_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](2, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_div_65_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "h5", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](3, "Customer Company");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "h5", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](9, "Location Name");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r7.salesDrModel.CompanyName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx_r7.salesDrModel.JobName);
      }
    }

    var _c0 = function _c0(a0) {
      return {
        backgroundColor: a0
      };
    };

    function SalesUserDashboardComponent_tbody_80_tr_1_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "td", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](10, "span", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var prodStatus_r43 = ctx.$implicit;

        var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](prodStatus_r43.Product.FuelName);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", prodStatus_r43.Product.Quantity, " \xA0 ", ctx_r42.GetUoMString(prodStatus_r43.Product.UoM), "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](prodStatus_r43.Product.StartDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate2"]("", prodStatus_r43.Product.StartTime, " - ", prodStatus_r43.Product.EndTime, "");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate"]("ngbTooltip", ctx_r42.GetToolTip(prodStatus_r43));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngStyle", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](8, _c0, ctx_r42.getBGColor(prodStatus_r43)));
      }
    }

    function SalesUserDashboardComponent_tbody_80_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tbody");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](1, SalesUserDashboardComponent_tbody_80_tr_1_Template, 11, 10, "tr", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx_r8.ProductStatuses);
      }
    }

    function SalesUserDashboardComponent_tr_121_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r44 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r44.RequestNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", item_r44.JobName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r44.FuelType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r44.Quantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r44.Pricing);
      }
    }

    function SalesUserDashboardComponent_tr_122_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "h4");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "No Data Found");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    function SalesUserDashboardComponent_tr_151_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r45 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r45.PoNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", item_r45.JobName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r45.Customer);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r45.FuelType);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r45.Quantity);
      }
    }

    function SalesUserDashboardComponent_tr_152_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "h4");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "No Data Found");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c1 = function _c1(a0, a1, a2) {
      return {
        "badge-success": a0,
        "badge-danger": a1,
        "badge-warning": a2
      };
    };

    function SalesUserDashboardComponent_tr_190_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "span", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](11);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](12, "slice");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r46 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r46.InvoiceNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"]("", item_r46.PoNumber, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r46.SourcingRequest);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](item_r46.DropDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpropertyInterpolate"]("ngbTooltip", item_r46 == null ? null : item_r46.Status);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction3"](11, _c1, (item_r46 == null ? null : item_r46.Status) == "Received", (item_r46 == null ? null : item_r46.Status) == "Rejected", (item_r46 == null ? null : item_r46.Status.length) > 8));

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", (item_r46 == null ? null : item_r46.Status.length) > 8 ? _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind3"](12, 7, item_r46 == null ? null : item_r46.Status, 0, 8) + ".." : item_r46 == null ? null : item_r46.Status, " ");
      }
    }

    function SalesUserDashboardComponent_tr_191_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "div", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "i", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "h4");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "No Data Found");

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
      }
    }

    var _c2 = function _c2(a0) {
      return {
        "show": a0
      };
    };

    var SalesUserDashboardComponent = /*#__PURE__*/function () {
      function SalesUserDashboardComponent(salesUserService, router, fb, fuelsurchargeService) {
        _classCallCheck(this, SalesUserDashboardComponent);

        this.salesUserService = salesUserService;
        this.router = router;
        this.fb = fb;
        this.fuelsurchargeService = fuelsurchargeService;
        this.IsLoading = false;
        this.Sourcingrequests = [];
        this.orders = [];
        this.Invoices = [];
        this.activeInvoiceDDTTab = 0;
        this.minDate = new Date();
        this.maxDate = new Date();
        this.SelectedDate = new Date();
        this.formSubmitted = false;
        this.selectedCompany = {
          CompanyId: 0,
          CompanyName: ""
        };
        this.selectedSite = {
          Id: 0,
          Name: ""
        };
        this.CompanySettings = {};
        this.SiteddlSettings = {};
        this.companyList = [];
        this.SiteList = [];
        this.SingleSelectSettingsById = {};
        this.FuelTypeList = [];
        this.AllTPOCompaniesList = [];
        this.UserContext = {};
        this.postData = {};
        this.showModal = false;
        this.CustomersJobsParentList = {
          regionsAndJobsModels: [],
          customersandJobs: []
        };
        this.DRInput = [];
        this.ProductStatuses = [];
        this.isConfirmDisabled = false;
        this.QuantityRegEx = /^\d*(\.\d{1,4})?$/;
        this.uparrow = true;
        this.DisplayRequestStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_2__["SourcingRequestDisplayStatus"];
        this.getUserContext();
        this.AdditionalProducts = [];
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 1);
      }

      _createClass(SalesUserDashboardComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.multiSettings();
          this.salesDrModel = new _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesDRModel"]();
          this.SalesDRForm = this.initSalesDRForm();
          this.initFormValues = this.SalesDRForm.value;
          this.getSourcingRequests();
          this.getOrders();
          this.getInvoices(0);
          this.GetCustomersNJobs();
          this.getProducts(0, 0); //By Default Get All Products
        }
      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(change) {
          if (change.SelectedDate && change.SelectedDate.currentValue) {
            if (this.maxDate < this.SelectedDate) {
              this.maxDate = moment__WEBPACK_IMPORTED_MODULE_6__(new Date(this.SelectedDate)).add(1, 'day').toDate();
              this.minDate = moment__WEBPACK_IMPORTED_MODULE_6__(new Date(this.SelectedDate)).toDate();
            } else {
              this.minDate = moment__WEBPACK_IMPORTED_MODULE_6__(new Date(this.SelectedDate)).toDate();
              this.maxDate = moment__WEBPACK_IMPORTED_MODULE_6__(new Date(this.SelectedDate)).add(1, 'day').toDate();
            }
          }
        }
      }, {
        key: "SetStartDate",
        value: function SetStartDate(date, index) {
          var _date = this.SalesDRForm.controls.AdditionalProducts.controls[index].controls['StartDate'];

          if (_date.value != date) {
            _date.patchValue(date);
          }
        }
      }, {
        key: "SetStartTime",
        value: function SetStartTime(time, index) {
          var _startTime = this.SalesDRForm.controls.AdditionalProducts.controls[index].controls['StartTime'];

          if (_startTime.value != time) {
            _startTime.patchValue(time);
          }
        }
      }, {
        key: "SetEndTime",
        value: function SetEndTime(time, index) {
          var _endTime = this.SalesDRForm.controls.AdditionalProducts.controls[index].controls['EndTime'];

          if (_endTime.value != time) {
            _endTime.patchValue(time);
          }
        }
      }, {
        key: "GetUoMString",
        value: function GetUoMString(Uom) {
          var UoMString = src_app_app_enum__WEBPACK_IMPORTED_MODULE_2__["UoM"][Uom];
          return UoMString;
        }
      }, {
        key: "GetToolTip",
        value: function GetToolTip(prodStatus) {
          if (prodStatus.Status.State != _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesUserDRStatus"].Success) {
            return "This customer is not set up to receive\n this product at this location; please \ncontact accounting for assistance.";
          } else {
            return "";
          }
        }
      }, {
        key: "RemoveToolTip",
        value: function RemoveToolTip(event) {
          event.target.title = ""; //(<HTMLSpanElement>event.target).title = "";
        }
      }, {
        key: "getBGColor",
        value: function getBGColor(prodStatus) {
          return prodStatus.Status.State == _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesUserDRStatus"].Success ? 'green' : 'red';
        }
      }, {
        key: "GetCustomersNJobs",
        value: function GetCustomersNJobs() {
          var _this41 = this;

          this.IsLoading = true;
          this.salesUserService.GetCustomersAndLocations().subscribe(function (customersAndLocations) {
            if (customersAndLocations) {
              var _cusAndLocations = customersAndLocations;

              var _joblist = customersAndLocations.customersandJobs.map(function (item) {
                return {
                  Id: item.JobId,
                  Name: item.JobName
                };
              });

              var _custlist = customersAndLocations.customersandJobs.map(function (item) {
                return {
                  CompanyId: item.CustomerId,
                  CompanyName: item.CustomerName
                };
              });
            }

            _this41.SiteList = _joblist; //Filter Unique Customers

            _this41.AllTPOCompaniesList = _this41.FilterUniqueCustomers(_custlist); //Sort Customers by Name

            _this41.AllTPOCompaniesList.sort(function (a, b) {
              return a.CompanyName.toLowerCase() > b.CompanyName.toLowerCase() ? 1 : b.CompanyName.toLowerCase() > a.CompanyName.toLowerCase() ? -1 : 0;
            }); //Sort Job By Names


            _this41.SiteList.sort(function (a, b) {
              return a.Name.toLowerCase() > b.Name.toLowerCase() ? 1 : b.Name.toLowerCase() > a.Name.toLowerCase() ? -1 : 0;
            });

            _this41.CustomersJobsParentList = _cusAndLocations;
            _this41.IsLoading = false;
          });
        }
      }, {
        key: "FilterUniqueCustomers",
        value: function FilterUniqueCustomers(custList) {
          var res = [];
          var map = new Map();

          var _iterator = _createForOfIteratorHelper(custList),
              _step;

          try {
            for (_iterator.s(); !(_step = _iterator.n()).done;) {
              var item = _step.value;

              if (!map.has(item.CompanyId)) {
                map.set(item.CompanyId, true);
                res.push({
                  CompanyId: item.CompanyId,
                  CompanyName: item.CompanyName
                });
              }
            }
          } catch (err) {
            _iterator.e(err);
          } finally {
            _iterator.f();
          }

          return res;
        }
      }, {
        key: "getProducts",
        value: function getProducts(companyId, jobId) {
          var _this42 = this;

          this.IsLoading = true;
          this.salesUserService.GetProducts(companyId, jobId).subscribe(function (data) {
            _this42.FuelTypeList = data;
            _this42.IsLoading = false;
          });
        }
      }, {
        key: "addProducts",
        value: function addProducts() {
          this.formSubmitted = false;
          this.AdditionalProducts.push(this.initAdditionalProducts());
          this.SalesDRForm.get('AdditionalProducts').push(this.initAdditionalProducts());
          this.setProducts();
        }
      }, {
        key: "setProducts",
        value: function setProducts() {
          if (this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
            this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
          } else {
            this.getProducts(0, 0);
          }
        }
      }, {
        key: "removeProduct",
        value: function removeProduct(index) {
          this.formSubmitted = false;
          this.SalesDRForm.controls.AdditionalProducts.removeAt(index);
          this.AdditionalProducts.splice(index, 1);
        }
      }, {
        key: "initAdditionalProducts",
        value: function initAdditionalProducts() {
          var _addProd = this.fb.group({
            Quantity: this.fb.control('', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required, _angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].pattern(this.QuantityRegEx)]),
            UoM: this.fb.control(1, _angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required),
            StartDate: this.fb.control(''),
            StartTime: this.fb.control(''),
            EndTime: this.fb.control(''),
            FuelTypes: this.fb.control([], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required),
            DRPO: this.fb.control('')
          });

          return _addProd;
        }
      }, {
        key: "initSalesDRForm",
        value: function initSalesDRForm() {
          this.AdditionalProducts.push(this.initAdditionalProducts());

          var _form = this.fb.group({
            CompanyList: this.fb.control([], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required),
            SiteList: this.fb.control([], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["Validators"].required),
            DRNotes: this.fb.control(''),
            AdditionalProducts: this.fb.array(this.AdditionalProducts)
          });

          return _form;
        }
      }, {
        key: "getCompanies",
        value: function getCompanies() {
          var _this43 = this;

          //this.isLoadingSubject.next(true);
          this.fuelsurchargeService.getSupplierCustomers().subscribe(function (data) {
            return Object(tslib__WEBPACK_IMPORTED_MODULE_0__["__awaiter"])(_this43, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
              return regeneratorRuntime.wrap(function _callee$(_context) {
                while (1) {
                  switch (_context.prev = _context.next) {
                    case 0:
                      _context.next = 2;
                      return data;

                    case 2:
                      this.companyList = _context.sent;

                    case 3:
                    case "end":
                      return _context.stop();
                  }
                }
              }, _callee, this);
            }));
          });
        }
      }, {
        key: "getUserContext",
        value: function getUserContext() {
          var _this44 = this;

          this.salesUserService.GetUserContext().subscribe(function (data) {
            _this44.UserContext = data;
          });
        }
      }, {
        key: "clearProducts",
        value: function clearProducts() {
          if (this.SalesDRForm.controls.AdditionalProducts != undefined && this.SalesDRForm.controls.AdditionalProducts != null && this.SalesDRForm.controls.AdditionalProducts.length > 1) {
            for (var i = this.SalesDRForm.controls.AdditionalProducts.length - 1; i > 0; i--) {
              this.removeProduct(i);
            }
          }
        }
      }, {
        key: "clearSalesDRForm",
        value: function clearSalesDRForm() {
          this.clearProducts();
          this.SalesDRForm.reset(this.initFormValues);
          this.formSubmitted = false;
        }
      }, {
        key: "updateFormControlValidators",
        value: function updateFormControlValidators(control, validators) {
          control.setValidators(validators);
          control.updateValueAndValidity();
        }
      }, {
        key: "FindSuccessStatus",
        value: function FindSuccessStatus(status, index) {
          if (status.Status.State == _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesUserDRStatus"].Success) {
            return status.Status.State == _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesUserDRStatus"].Success;
          }
        }
      }, {
        key: "onValidate",
        value: function onValidate() {
          var _this45 = this;

          this.IsLoading = true; //Copy Form values to model

          this.formSubmitted = true;

          if (!this.SalesDRForm.valid) {
            this.IsLoading = false;
            return;
          }

          this.SalesDRformToModel(); //Post the model for validations

          this.salesUserService.ValidateDREntryForm(this.salesDrModel).subscribe(function (res) {
            if (res) {
              var _inputDR = res;
            }

            _this45.DRInput = _inputDR.RaiseDeliveryRequestInput;
            _this45.ProductStatuses = _inputDR.ProductStatuses;

            if (_this45.ProductStatuses && _this45.ProductStatuses.length > 0) {
              var successStatus = _this45.ProductStatuses.find(function (status, index) {
                return _this45.FindSuccessStatus(status, index);
              });

              _this45.isConfirmDisabled = successStatus ? false : true;
            }

            _this45.IsLoading = false;
          });
          this.showModal = true;
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          var _this46 = this;

          this.IsLoading = true;
          this.salesUserService.CreateDREntryForm(this.DRInput).subscribe(function (data) {
            if (data && data.StatusCode == _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["ConfirmDRStatus"].Success) {
              var _DRstatus = data;
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgsuccess("DR/Order Creation was Successful.", undefined, undefined);
              _this46.IsLoading = false;
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_4__["Declarations"].msgerror(data.StatusMessage, undefined, undefined);
              _this46.IsLoading = false;
              return;
            }
          });
          this.clearForm();
        }
      }, {
        key: "clearForm",
        value: function clearForm() {
          this.salesDrModel = new _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesDRModel"]();
          this.clearSalesDRForm();
          this.SalesDRForm.reset(this.initFormValues);
          this.showModal = false;
          this.formSubmitted = false;
        }
      }, {
        key: "multiSettings",
        value: function multiSettings() {
          this.CompanySettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'CompanyId',
            textField: 'CompanyName',
            enableCheckAll: false,
            itemsShowLimit: 1,
            allowSearchFilter: true
          };
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
        }
      }, {
        key: "onCompanySelect",
        value: function onCompanySelect(company) {
          var _this47 = this;

          this.SalesDRForm.controls.SiteList.setValue([]);
          this.SalesDRForm.controls.AdditionalProducts.controls.forEach(function (x) {
            return x.reset(_this47.initAdditionalProducts().value);
          });
          this.selectedCompany.CompanyId = company.CompanyId;
          this.selectedCompany.CompanyName = company.CompanyName;
          this.SalesDRForm.controls.CompanyList.setValue([{
            CompanyId: company.CompanyId,
            CompanyName: company.CompanyName
          }]);

          var _filteredSites = this.CustomersJobsParentList.customersandJobs.filter(function (x) {
            return x.CustomerId == company.CompanyId;
          }).map(function (item) {
            return {
              Id: item.JobId,
              Name: item.JobName
            };
          });

          this.SiteList = _filteredSites;
          this.SiteList.sort(function (a, b) {
            return a.Name.toLowerCase() > b.Name.toLowerCase() ? 1 : b.Name.toLowerCase() > a.Name.toLowerCase() ? -1 : 0;
          });

          if (this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
            this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
          } else {
            this.getProducts(0, 0);
          }
          /*this.selectedOrder = [];
          this.SelectedCustomerId = item.CompanyId;
          this.SiteList = this.drOrders.filter(x => x.CompanyId == item.CompanyId).map((element) => ({ Id: element.JobId, Name: element.JobName }));
          this.SiteList = this.GetUniqueLocations(this.SiteList.reduce((p, n) => p.concat(n), [])); */

        }
      }, {
        key: "onCompanyDeSelect",
        value: function onCompanyDeSelect(event) {
          this.selectedCompany.CompanyId = 0;
          this.selectedSite.Id = 0;
          this.SiteList = [];
          this.FuelTypeList = [];
          this.SalesDRForm.controls.SiteList.setValue([]);
          this.getProducts(0, 0);
          this.clearForm();
        }
      }, {
        key: "onSiteSelect",
        value: function onSiteSelect(sites) {
          var _this48 = this;

          this.SalesDRForm.controls.AdditionalProducts.controls.forEach(function (x) {
            return x.reset(_this48.initAdditionalProducts().value);
          });
          this.selectedSite.Id = sites.Id;
          this.selectedSite.Name = sites.Name;
          this.SalesDRForm.controls.SiteList.setValue([{
            Id: sites.Id,
            Name: sites.Name
          }]);

          if (this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
            this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
          } else {
            this.getProducts(0, 0);
          }
        }
      }, {
        key: "onSiteDeSelect",
        value: function onSiteDeSelect(event) {
          var _this49 = this;

          this.FuelTypeList = [];
          this.SalesDRForm.controls.AdditionalProducts.controls.forEach(function (x) {
            return x.reset(_this49.initAdditionalProducts().value);
          });
          this.getProducts(0, 0);
        }
      }, {
        key: "SalesDRformToModel",
        value: function SalesDRformToModel() {
          this.salesDrModel = new _sales_user_model__WEBPACK_IMPORTED_MODULE_5__["SalesDRModel"]();

          if (this.SalesDRForm.valid) {
            this.salesDrModel.CompanyId = this.SalesDRForm.controls.CompanyList.value[0].CompanyId;
            this.salesDrModel.CompanyName = this.SalesDRForm.controls.CompanyList.value[0].CompanyName;
            this.salesDrModel.JobId = this.SalesDRForm.controls.SiteList.value[0].Id;
            this.salesDrModel.JobName = this.SalesDRForm.controls.SiteList.value[0].Name;
            this.salesDrModel.DRNotes = this.SalesDRForm.controls.DRNotes.value;
            this.salesDrModel.Products = [];
            var addProducts = this.SalesDRForm.controls.AdditionalProducts;

            var _iterator2 = _createForOfIteratorHelper(addProducts.controls),
                _step2;

            try {
              for (_iterator2.s(); !(_step2 = _iterator2.n()).done;) {
                var control = _step2.value;

                if (control instanceof _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroup"]) {
                  this.salesDrModel.Products.push({
                    Quantity: control.value.Quantity,
                    UoM: control.value.UoM,
                    StartDate: control.value.StartDate,
                    StartTime: control.value.StartTime,
                    EndTime: control.value.EndTime,
                    DRPO: control.value.DRPO,
                    FuelTypes: {
                      Id: control.value.FuelTypes[0].Id,
                      Name: control.value.FuelTypes[0].Name
                    },
                    FuelTypeId: control.value.FuelTypes[0].Id,
                    FuelName: control.value.FuelTypes[0].Name
                  });
                }
              }
            } catch (err) {
              _iterator2.e(err);
            } finally {
              _iterator2.f();
            }
          }
        }
      }, {
        key: "getJobLists",
        value: function getJobLists(companyId, isFtl, foAsTerminal) {
          var _this50 = this;

          var companyName = this.AllTPOCompaniesList.find(function (t) {
            return t.CompanyId == companyId;
          }).CompanyName;
          var ftlvalue = isFtl == "FullTruckLoad" ? true : false;
          var tervalue = foAsTerminal == "Terminal" ? true : false;
          this.salesUserService.GetJobLists(companyName, ftlvalue, tervalue).subscribe(function (data) {
            if (data) {
              var joblistdata = data.map(function (item) {
                return {
                  Id: item.Id,
                  Name: item.Name
                };
              });
              _this50.SiteList = joblistdata;
            }
          });
        }
      }, {
        key: "toggleArrow",
        value: function toggleArrow() {
          this.uparrow = !this.uparrow;
        }
      }, {
        key: "getSourcingRequests",
        value: function getSourcingRequests() {
          var _this51 = this;

          this.IsLoading = true;
          var isFromDashboard = true;
          this.salesUserService.GetSourcingRequests(this.DisplayRequestStatus.All, isFromDashboard).subscribe(function (data) {
            _this51.IsLoading = false;
            _this51.Sourcingrequests = data;
          });
        }
      }, {
        key: "getOrders",
        value: function getOrders() {
          var _this52 = this;

          this.IsLoading = true;
          this.salesUserService.GetOrdersForDashboard().subscribe(function (data) {
            _this52.IsLoading = false;
            _this52.orders = data;
          });
        }
      }, {
        key: "getInvoices",
        value: function getInvoices(type) {
          var _this53 = this;

          this.IsLoading = true;
          this.salesUserService.GetInvoicesForDashboard(type).subscribe(function (data) {
            _this53.IsLoading = false;
            _this53.Invoices = data;
          });
        }
      }, {
        key: "changeActiveTab",
        value: function changeActiveTab(type) {
          this.activeInvoiceDDTTab = type;
          this.getInvoices(type);
        }
      }, {
        key: "navigateToSourcing",
        value: function navigateToSourcing() {
          this.router.navigate([]).then(function (result) {
            window.open('/SalesUser/SourcingRequest/Index', '_blank');
          });
        }
      }, {
        key: "navigateToOrders",
        value: function navigateToOrders() {
          this.router.navigate([]).then(function (result) {
            window.open('/Supplier/Order/View', '_blank');
          });
        }
      }, {
        key: "navigateToInvoice",
        value: function navigateToInvoice() {
          this.router.navigate([]).then(function (result) {
            window.open('Supplier/Invoice/View', '_blank');
          });
        }
      }]);

      return SalesUserDashboardComponent;
    }();

    SalesUserDashboardComponent.ɵfac = function SalesUserDashboardComponent_Factory(t) {
      return new (t || SalesUserDashboardComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_sales_user_service__WEBPACK_IMPORTED_MODULE_7__["SalesUserService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_8__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]));
    };

    SalesUserDashboardComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({
      type: SalesUserDashboardComponent,
      selectors: [["app-sales-user-dashboard"]],
      inputs: {
        SelectedDate: "SelectedDate"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵNgOnChangesFeature"]],
      decls: 192,
      vars: 36,
      consts: [[1, "sales-user-dashboard-container"], [3, "formGroup"], ["id", "entryForm", 1, "container", "well", "accordion"], ["class", "loader", 4, "ngIf"], [1, "row"], [1, "col-sm-12"], [1, "col-11"], [1, "well-header"], [1, "well-title"], ["class", "col-1 mt-2 d-flex justify-content-center", 4, "ngIf"], ["id", "collapseEntryForm", "data-parent", "#entryForm", 1, "collapse", "show"], [1, "row", "well-header"], [1, "col-sm-4"], ["for", "Quantity", 1, "control-label", "font-weight-bold"], [1, "color-maroon"], [1, "form-group"], ["formControlName", "CompanyList", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], [4, "ngIf"], ["formControlName", "SiteList", 1, "single-select", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect"], ["formArrayName", "AdditionalProducts", "class", "mx-3", 4, "ngFor", "ngForOf"], [1, "add-partial-block", 3, "click"], [1, "fas", "fa-plus-circle"], [1, "form-group", "col-sm-3"], ["for", "DRNotes", 1, "font-weight-bold"], ["name", "DRNotes", "id", "DRNotes", "cols", "30", "rows", "2", "formControlName", "DRNotes"], [1, "col-sm-12", "text-right"], [1, "container", "form-group"], ["type", "button", "value", "Clear", "data-toggle", "modal", "data-target", "#validateModal", 1, "btn", 3, "click"], ["id", "Submit", "type", "button", "value", "Submit", 1, "btn", "btn-lg", "btn-primary", "btnSubmit", 3, "click"], [1, "container", "p-0"], [1, "container", "well"], [1, "col-xs-12"], ["id", "validateModal", "tabindex", "-1", 1, "modal", "fade", 3, "ngClass"], [1, "modal-dialog"], [1, "modal-content"], [1, "modal-header"], [1, "col-sm-8", "text-left"], [1, "modal-title", "pull-left", "p-0"], [1, "col-sm-4", "text-right"], ["data-dismiss", "modal", 1, "close", "pull-right", 3, "click"], [1, "modal-body"], ["class", "row form-group", 4, "ngIf"], [1, "table"], [1, "row", "form-group"], [1, "font-weight-bold", "my-2"], [1, "modal-footer"], [1, "col-sm-12", "text-right", "pull-right", "pr-0"], ["data-dismiss", "modal", 1, "btn", "btn-clear", 3, "click"], [1, "btn", "btn-primary", 3, "disabled", "click"], [1, "col-12", "col-lg-6"], [1, "well"], [1, "col-sm-9", "form-row", "align-items-center"], [1, "d-inline-block"], [1, "col-sm-3", "form-row", "align-items-center", "flex-row-reverse", "pr0"], [1, "btn", "btn-outline", "btn-primary", "btn-rnd", "fs11", 3, "click"], [1, "well-body", "padding-8"], [1, "table-wrapper"], [1, "table", "table-hover"], [4, "ngFor", "ngForOf"], [1, "dib", "border", "radius-capsule", "shadow-b", "ml20"], [1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", 3, "click"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "col-1", "mt-2", "d-flex", "justify-content-center"], ["data-toggle", "collapse", "data-target", "#collapseEntryForm", "aria-expanded", "true", "aria-controls", "collapseEntryForm", 1, "fas", "fa-chevron-down", "fa-2x", "arrow", 3, "click"], ["data-toggle", "collapse", "data-target", "#collapseEntryForm", "aria-expanded", "true", "aria-controls", "collapseEntryForm", 1, "fas", "fa-chevron-up", "fa-2x", "arrow", 3, "click"], ["class", "text-danger", 4, "ngIf"], [1, "text-danger"], ["formArrayName", "AdditionalProducts", 1, "mx-3"], [1, "row", "well-header", 3, "formGroupName"], [1, "col-sm-12", "addProdTemplate", "shadow-sm"], [1, "row", "mt-3"], ["for", "idFuelType", 1, "control-label", "font-weight-bold"], ["formControlName", "FuelTypes", "id", "FuelTypes", 1, "single-select", 3, "placeholder", "settings", "data"], ["type", "text", "value", "", "id", "Quantity", "placeholder", "", "formControlName", "Quantity", 1, "form-control"], ["for", "idUoM", 1, "control-label", "font-weight-bold"], ["id", "idUoM", "formControlName", "UoM", 1, "form-control"], ["value", "1"], ["value", "2"], ["value", "3"], ["value", "4"], [1, "col-sm-3", "form-group"], ["for", "DR_PO", 1, "control-label", "font-weight-bold"], ["type", "text", "value", "", "id", "DR_PO", "placeholder", "", "formControlName", "DRPO", 1, "form-control"], [1, "form-group", "col-sm-3", "col-md-2"], [1, "font-weight-bold"], ["type", "text", "formControlName", "StartDate", "placeholder", "Start Date", "myDatePicker", "", "autocomplete", "off", 1, "form-control", "datepicker", 3, "format", "maxDate", "minDate", "onDateChange"], [1, "col-sm-3", "col-md-2", "form-group"], ["for", "idStartTime", 1, "font-weight-bold"], ["type", "text", "formControlName", "StartTime", "myTimePicker", "", "placeholder", "Start Time", "autocomplete", "off", 1, "form-control", "timepicker", 3, "format", "onTimeChange"], ["for", "idEndTime", 1, "font-weight-bold"], ["type", "text", "formControlName", "EndTime", "myTimePicker", "", "placeholder", "End Time", "autocomplete", "off", 1, "form-control", "timepicker", 3, "format", "onTimeChange"], [1, "col-sm-12", "form-group"], ["class", "float-right", 4, "ngIf"], [1, "float-right"], ["data-toggle", "tooltip", "data-placement", "right", "title", "Remove", 1, "fa", "fa-trash-alt", "text-danger", 3, "click"], [1, "font-800"], [1, "text-center"], ["placement", "bottom", "container", "body", 1, "dot", "tooltip-dot", 3, "ngStyle", "ngbTooltip"], ["colspan", "5"], [1, "row", "align-items-center", 2, "height", "175px"], [1, "col-12", "align-items-center", "text-center"], [1, "fab", "fa-searchengin", "fa-5x"], ["placement", "left", 1, "badge", "badge-pill", "badge-primary", 3, "ngClass", "ngbTooltip"]],
      template: function SalesUserDashboardComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "form", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](3, SalesUserDashboardComponent_div_3_Template, 3, 0, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](4, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](9, "h4", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Order Entry Form");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](11, SalesUserDashboardComponent_div_11_Template, 2, 0, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](12, SalesUserDashboardComponent_div_12_Template, 2, 0, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "div", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](14, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](16, "label", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](17, "Company Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](18, "span", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](19, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](21, "ng-multiselect-dropdown", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function SalesUserDashboardComponent_Template_ng_multiselect_dropdown_onSelect_21_listener($event) {
            return ctx.onCompanySelect($event);
          })("onDeSelect", function SalesUserDashboardComponent_Template_ng_multiselect_dropdown_onDeSelect_21_listener($event) {
            return ctx.onCompanyDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](22, SalesUserDashboardComponent_div_22_Template, 2, 1, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](23, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "label", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](25, "Ship-to Location ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](26, "span", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](27, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "ng-multiselect-dropdown", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("onSelect", function SalesUserDashboardComponent_Template_ng_multiselect_dropdown_onSelect_29_listener($event) {
            return ctx.onSiteSelect($event);
          })("onDeSelect", function SalesUserDashboardComponent_Template_ng_multiselect_dropdown_onDeSelect_29_listener($event) {
            return ctx.onSiteDeSelect($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](30, SalesUserDashboardComponent_div_30_Template, 2, 1, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](31, SalesUserDashboardComponent_div_31_Template, 53, 13, "div", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](33, "div", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "a", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_a_click_34_listener() {
            return ctx.addProducts();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](35, "i", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](36, " Add Product ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](37, "div", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](38, "div", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](39, "label", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](40, "DR Notes");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](41, "textarea", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](42, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](43, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](44, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](45, "input", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_input_click_45_listener() {
            return ctx.clearSalesDRForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](46, "\xA0\xA0 ");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](47, "input", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_input_click_47_listener() {
            return ctx.onValidate();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](48, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](49, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](50, SalesUserDashboardComponent_div_50_Template, 3, 0, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](51, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](52, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](53, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](54, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](55, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](56, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](57, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](58, "h4", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](59, "b");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](60, "Preview");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](61, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](62, "button", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_62_listener() {
            return ctx.showModal = false;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](63, "\xD7");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](64, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](65, SalesUserDashboardComponent_div_65_Template, 13, 2, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipe"](66, "json");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](67, "table", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](68, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](69, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](70, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](71, "Product");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](72, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](73, "Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](74, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](75, "Delivery Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](76, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](77, "Delivery Window");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](78, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](79, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](80, SalesUserDashboardComponent_tbody_80_Template, 2, 1, "tbody", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](81, "div", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](82, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](83, "p", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](84, "DR Notes");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](85, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](86);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](87, "div", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](88, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](89, "button", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_89_listener() {
            return ctx.showModal = false;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](90, "Back");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](91, "button", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_91_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](92, "Confirm");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](93, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](94, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](95, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](96, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](97, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](98, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](99, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](100, "h4", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](101, "Sourcing Request");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](102, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](103, "button", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_103_listener() {
            return ctx.navigateToSourcing();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](104, "View More");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](105, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](106, "div", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](107, "table", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](108, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](109, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](110, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](111, "Request #");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](112, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](113, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](114, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](115, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](116, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](117, "Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](118, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](119, "Pricing");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](120, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](121, SalesUserDashboardComponent_tr_121_Template, 11, 5, "tr", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](122, SalesUserDashboardComponent_tr_122_Template, 7, 0, "tr", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](123, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](124, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](125, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](126, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](127, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](128, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](129, "h4", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](130, "Orders");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](131, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](132, "button", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_132_listener() {
            return ctx.navigateToOrders();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](133, "View More");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](134, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](135, "div", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](136, "table", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](137, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](138, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](139, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](140, "PO #");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](141, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](142, "Location Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](143, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](144, "Customer");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](145, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](146, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](147, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](148, "Quantity");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](149, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](150);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](151, SalesUserDashboardComponent_tr_151_Template, 11, 5, "tr", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](152, SalesUserDashboardComponent_tr_152_Template, 7, 0, "tr", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](153, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](154, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](155, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](156, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](157, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](158, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](159, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](160, "h4", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](161);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](162, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](163, "div", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](164, "input", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](165, "label", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_label_click_165_listener() {
            return ctx.changeActiveTab(0);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](166, "Invoices");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](167, "input", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](168, "label", 63);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_label_click_168_listener() {
            return ctx.changeActiveTab(6);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](169, "DDTs");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](170, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](171, "button", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SalesUserDashboardComponent_Template_button_click_171_listener() {
            return ctx.navigateToInvoice();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](172, "View More");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](173, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](174, "div", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](175, "table", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](176, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](177, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](178, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](179, "InvoiceNumber #");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](180, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](181, "PoNumber");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](182, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](183, "SourcingRequest");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](184, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](185, "DropDate");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](186, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](187, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](188, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerStart"](189);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](190, SalesUserDashboardComponent_tr_190_Template, 13, 15, "tr", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](191, SalesUserDashboardComponent_tr_191_Template, 7, 0, "tr", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("formGroup", ctx.SalesDRForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", !ctx.uparrow);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.uparrow);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Company")("settings", ctx.CompanySettings)("data", ctx.AllTPOCompaniesList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.formSubmitted && (ctx.SalesDRForm.controls == null ? null : ctx.SalesDRForm.controls.CompanyList == null ? null : ctx.SalesDRForm.controls.CompanyList.errors));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("placeholder", "Select Ship-to location")("settings", ctx.SingleSelectSettingsById)("data", ctx.SiteList);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.formSubmitted && (ctx.SalesDRForm.controls == null ? null : ctx.SalesDRForm.controls.SiteList == null ? null : ctx.SalesDRForm.controls.SiteList.errors));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.AdditionalProducts);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](19);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpureFunction1"](34, _c2, ctx.showModal));

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.salesDrModel != undefined && _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵpipeBind1"](66, 32, ctx.salesDrModel) != "{}");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](15);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.ProductStatuses != undefined && ctx.ProductStatuses.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.salesDrModel.DRNotes);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", ctx.isConfirmDisabled);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](30);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.Sourcingrequests);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.Sourcingrequests && ctx.Sourcingrequests.length == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](29);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.orders);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.orders && ctx.orders.length == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.activeInvoiceDDTTab == 6 ? "DDTs" : "Invoices");

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "activeInvoiceDDTTab")("value", 0)("checked", ctx.activeInvoiceDDTTab == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", "activeInvoiceDDTTab")("value", 6)("checked", ctx.activeInvoiceDDTTab == 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](23);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.Invoices);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngIf", ctx.Invoices && ctx.Invoices.length == 0);
        }
      },
      directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgIf"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_11__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["DefaultValueAccessor"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgClass"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormArrayName"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵangular_packages_forms_forms_x"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_12__["DatePicker"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_12__["TimePicker"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["NgStyle"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_13__["NgbTooltip"]],
      pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_10__["JsonPipe"], _angular_common__WEBPACK_IMPORTED_MODULE_10__["SlicePipe"]],
      styles: [".tooltip-dot[_ngcontent-%COMP%] {\n  white-space: nowrap;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  width: 100%;\n}\n\n.modal-title[_ngcontent-%COMP%] {\n  font-size: 20px;\n}\n\n.arrow[_ngcontent-%COMP%] {\n  cursor: pointer;\n  font-size: 20px;\n}\n\n.addProdTemplate[_ngcontent-%COMP%] {\n  border: 1px solid #ebebeb;\n  border-radius: 10px;\n}\n\n#tooltip[_ngcontent-%COMP%] {\n  display: block;\n  background: black;\n  color: white;\n  padding: 5px;\n  position: absolute;\n  top: 131px;\n  width: 590px;\n  border-radius: 5px;\n}\n\n.modal[_ngcontent-%COMP%] {\n  background-color: rgba(0, 0, 0, 0.3);\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%] {\n  background: #FFFFFF;\n  padding: 0;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .well-header[_ngcontent-%COMP%] {\n  border-radius: 24px 24px 0px 0px;\n  padding: 10px 20px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .well-title[_ngcontent-%COMP%] {\n  font-weight: 600 !important;\n  font-size: 18px !important;\n  letter-spacing: 0.25px;\n  color: #12121F !important;\n  margin-bottom: 0;\n  padding: 0 !important;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .well-body[_ngcontent-%COMP%] {\n  padding: 0px;\n  min-height: 350px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .well-body.padding-8[_ngcontent-%COMP%] {\n  padding: 0 15px;\n  padding-bottom: 5px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .btn-primary.btn-outline[_ngcontent-%COMP%] {\n  color: #1062d1;\n  font-weight: 600;\n  font-size: 14px;\n  line-height: 18px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .btn-primary.btn-outline[_ngcontent-%COMP%]:hover {\n  color: #FFFFFF;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .btn-primary.btn-outline[_ngcontent-%COMP%]:active {\n  color: #FFFFFF;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .well[_ngcontent-%COMP%]   .btn-primary.btn-outline[_ngcontent-%COMP%]:focus {\n  color: #FFFFFF;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .radius-capsule[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%] {\n  line-height: 22px;\n  padding: 5px 8px !important;\n  font-size: 12px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .radius-capsule[_ngcontent-%COMP%]   .btn[_ngcontent-%COMP%]:not(:last-child) {\n  margin-right: 5px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .map-view-contanier[_ngcontent-%COMP%]   .agm-map-container-inner[_ngcontent-%COMP%] {\n  border-bottom-left-radius: 15px;\n  border-bottom-right-radius: 15px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .map-view-contanier[_ngcontent-%COMP%]   .input-search.form-group[_ngcontent-%COMP%] {\n  margin-bottom: 0px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .map-view-contanier[_ngcontent-%COMP%]   .input-search[_ngcontent-%COMP%]   .form-control[_ngcontent-%COMP%] {\n  border-radius: 15px;\n  padding-left: 2.375rem;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .map-view-contanier[_ngcontent-%COMP%]   .input-search[_ngcontent-%COMP%]   .form-control-search[_ngcontent-%COMP%] {\n  position: absolute;\n  z-index: 2;\n  display: block;\n  width: 2.375rem;\n  height: 2.375rem;\n  line-height: 2.375rem;\n  text-align: center;\n  pointer-events: none;\n  color: #aaa;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%] {\n  border-radius: 40px;\n  border: 1px solid #dee2e6 !important;\n  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link[_ngcontent-%COMP%] {\n  padding: 5px 15px;\n  font-size: 13px;\n  line-height: 22px;\n  border-radius: 50px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.mustgo[_ngcontent-%COMP%] {\n  color: #BE4242;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.mustgo[_ngcontent-%COMP%]:hover {\n  background: #BE4242;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.mustgo.active[_ngcontent-%COMP%] {\n  background: #BE4242;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.shouldgo[_ngcontent-%COMP%] {\n  color: #E89330;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.shouldgo[_ngcontent-%COMP%]:hover {\n  background: #E89330;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.shouldgo.active[_ngcontent-%COMP%] {\n  background: #E89330;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.couldgo[_ngcontent-%COMP%] {\n  color: #696969;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.couldgo[_ngcontent-%COMP%]:hover {\n  background: #696969;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]   .nav-link.couldgo.active[_ngcontent-%COMP%] {\n  background: #696969;\n  color: #ffffff;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .prority-pills[_ngcontent-%COMP%]   .nav-pills[_ngcontent-%COMP%]   .nav-item[_ngcontent-%COMP%]:not(:last-child) {\n  margin-right: 3px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .badge[_ngcontent-%COMP%] {\n  padding: 7px 10px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .badge.badge-success[_ngcontent-%COMP%] {\n  color: #fff !important;\n  background-color: #52AB34 !important;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .btn-filter[_ngcontent-%COMP%]   input[type=text].datepicker[_ngcontent-%COMP%] {\n  height: 32px !important;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .table-height-270[_ngcontent-%COMP%] {\n  min-height: 270px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .text-warapper-50[_ngcontent-%COMP%] {\n  max-width: 70px;\n  word-wrap: break-word;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .table-wrapper[_ngcontent-%COMP%] {\n  overflow-y: auto;\n  flex-grow: 1;\n  max-height: 300px;\n  min-height: 290px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .table-wrapper[_ngcontent-%COMP%]   table[_ngcontent-%COMP%] {\n  border-collapse: collapse;\n  width: 100%;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .table-wrapper[_ngcontent-%COMP%]   table[_ngcontent-%COMP%]   th[_ngcontent-%COMP%] {\n  position: sticky;\n  top: 0;\n  background: white;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .table-wrapper[_ngcontent-%COMP%]   table[_ngcontent-%COMP%]   td[_ngcontent-%COMP%], .sales-user-dashboard-container[_ngcontent-%COMP%]   .table-wrapper[_ngcontent-%COMP%]   table[_ngcontent-%COMP%]   th[_ngcontent-%COMP%] {\n  text-align: left;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-scheduled[_ngcontent-%COMP%] {\n  background: #87db87;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-inprogress[_ngcontent-%COMP%] {\n  background: #0c52b1;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-cancelled[_ngcontent-%COMP%] {\n  background: #f51616;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-completed[_ngcontent-%COMP%] {\n  background: #14ad14;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-mustgo[_ngcontent-%COMP%] {\n  background: #d95a67;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-shouldgo[_ngcontent-%COMP%] {\n  background: #ec9f5a;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .bg-couldgo[_ngcontent-%COMP%] {\n  background: #a5a5a5;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .driver-list[_ngcontent-%COMP%] {\n  max-height: 335px;\n  overflow: auto;\n  margin-top: 10px;\n  padding: 0 8px;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .driver-initials[_ngcontent-%COMP%] {\n  width: 36px;\n  height: 36px;\n  text-align: center;\n  display: flex;\n  align-items: center;\n  justify-content: center;\n}\n\n.sales-user-dashboard-container[_ngcontent-%COMP%]   .driver-details[_ngcontent-%COMP%]:hover {\n  background: #f7f7f7;\n  cursor: pointer;\n}\n\n.icon-wrapper2[_ngcontent-%COMP%] {\n  right: 5px;\n  top: 150px;\n  position: fixed;\n  z-index: 99;\n}\n\n\n\n.dot[_ngcontent-%COMP%] {\n  height: 10px;\n  width: 10px;\n  border-radius: 50%;\n  display: inline-block;\n}\n\n.modal.show[_ngcontent-%COMP%] {\n  display: block;\n}\n\n.font-800[_ngcontent-%COMP%] {\n  font-weight: 800 !important;\n}\n\n.modal-dialog[_ngcontent-%COMP%] {\n  max-width: 600px;\n}\n\n.pad-left-4[_ngcontent-%COMP%] {\n  padding-left: 4px !important;\n}\n\n.pad-top-26[_ngcontent-%COMP%] {\n  padding-top: 26px !important;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2FsZXMtdXNlci9zYWxlcy11c2VyLWRhc2hib2FyZC9EOlxcVEZTY29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2VcXFNpdGVGdWVsLkV4Y2hhbmdlLlNvdXJjZUNvZGVcXFNpdGVGdWVsLkV4Y2hhbmdlLldlYi9zcmNcXGFwcFxcc2FsZXMtdXNlclxcc2FsZXMtdXNlci1kYXNoYm9hcmRcXHNhbGVzLXVzZXItZGFzaGJvYXJkLmNvbXBvbmVudC5zY3NzIiwic3JjL2FwcC9zYWxlcy11c2VyL3NhbGVzLXVzZXItZGFzaGJvYXJkL3NhbGVzLXVzZXItZGFzaGJvYXJkLmNvbXBvbmVudC5zY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0VBQ0ksbUJBQUE7RUFDQSx1QkFBQTtFQUNBLGdCQUFBO0VBQ0EsV0FBQTtBQ0NKOztBRENBO0VBQ0ksZUFBQTtBQ0VKOztBRENBO0VBQ0ksZUFBQTtFQUNBLGVBQUE7QUNFSjs7QURDQTtFQUNJLHlCQUFBO0VBQ0EsbUJBQUE7QUNFSjs7QURDQTtFQUNJLGNBQUE7RUFDQSxpQkFBQTtFQUNBLFlBQUE7RUFDQSxZQUFBO0VBQ0Esa0JBQUE7RUFDQSxVQUFBO0VBQ0EsWUFBQTtFQUNBLGtCQUFBO0FDRUo7O0FEQ0E7RUFDRyxvQ0FBQTtBQ0VIOztBRENJO0VBQ0ksbUJBQUE7RUFDQSxVQUFBO0FDRVI7O0FEQVE7RUFDSSxnQ0FBQTtFQUNBLGtCQUFBO0FDRVo7O0FEQ1E7RUFDSSwyQkFBQTtFQUNBLDBCQUFBO0VBQ0Esc0JBQUE7RUFDQSx5QkFBQTtFQUNBLGdCQUFBO0VBQ0EscUJBQUE7QUNDWjs7QURFUTtFQUNJLFlBQUE7RUFDQSxpQkFBQTtBQ0FaOztBREVZO0VBQ0ksZUFBQTtFQUNBLG1CQUFBO0FDQWhCOztBRGNRO0VBQ0ksY0FBQTtFQUNBLGdCQUFBO0VBQ0EsZUFBQTtFQUNBLGlCQUFBO0FDWlo7O0FEY1k7RUFDSSxjQUFBO0FDWmhCOztBRGVZO0VBQ0ksY0FBQTtBQ2JoQjs7QURnQlk7RUFDSSxjQUFBO0FDZGhCOztBRG9CUTtFQUNJLGlCQUFBO0VBQ0EsMkJBQUE7RUFDQSxlQUFBO0FDbEJaOztBRG9CWTtFQUNJLGlCQUFBO0FDbEJoQjs7QUR3QlE7RUFDSSwrQkFBQTtFQUNBLGdDQUFBO0FDdEJaOztBRDJCWTtFQUNJLGtCQUFBO0FDekJoQjs7QUQ0Qlk7RUFDSSxtQkFBQTtFQUNBLHNCQUFBO0FDMUJoQjs7QUQ2Qlk7RUFDSSxrQkFBQTtFQUNBLFVBQUE7RUFDQSxjQUFBO0VBQ0EsZUFBQTtFQUNBLGdCQUFBO0VBQ0EscUJBQUE7RUFDQSxrQkFBQTtFQUNBLG9CQUFBO0VBQ0EsV0FBQTtBQzNCaEI7O0FEaUNRO0VBQ0ksbUJBQUE7RUFDQSxvQ0FBQTtFQUNBLHlDQUFBO0FDL0JaOztBRGtDZ0I7RUFDSSxpQkFBQTtFQUNBLGVBQUE7RUFDQSxpQkFBQTtFQUNBLG1CQUFBO0FDaENwQjs7QURrQ29CO0VBQ0ksY0FBQTtBQ2hDeEI7O0FEa0N3QjtFQUNJLG1CQUFBO0VBQ0EsY0FBQTtBQ2hDNUI7O0FEbUN3QjtFQUNJLG1CQUFBO0VBQ0EsY0FBQTtBQ2pDNUI7O0FEcUNvQjtFQUNJLGNBQUE7QUNuQ3hCOztBRHFDd0I7RUFDSSxtQkFBQTtFQUNBLGNBQUE7QUNuQzVCOztBRHNDd0I7RUFDSSxtQkFBQTtFQUNBLGNBQUE7QUNwQzVCOztBRHdDb0I7RUFDSSxjQUFBO0FDdEN4Qjs7QUR3Q3dCO0VBQ0ksbUJBQUE7RUFDQSxjQUFBO0FDdEM1Qjs7QUR5Q3dCO0VBQ0ksbUJBQUE7RUFDQSxjQUFBO0FDdkM1Qjs7QUQ0Q2dCO0VBQ0ksaUJBQUE7QUMxQ3BCOztBRGdESTtFQUNJLGlCQUFBO0FDOUNSOztBRGdEUTtFQUNJLHNCQUFBO0VBQ0Esb0NBQUE7QUM5Q1o7O0FEa0RJO0VBQ0ksdUJBQUE7QUNoRFI7O0FEbURJO0VBQ0ksaUJBQUE7QUNqRFI7O0FEc0RJO0VBQ0ksZUFBQTtFQUNBLHFCQUFBO0FDcERSOztBRHVESTtFQUNJLGdCQUFBO0VBQ0EsWUFBQTtFQUNBLGlCQUFBO0VBQ0EsaUJBQUE7QUNyRFI7O0FEdURRO0VBQ0kseUJBQUE7RUFDQSxXQUFBO0FDckRaOztBRHVEWTtFQUNJLGdCQUFBO0VBQ0EsTUFBQTtFQUNBLGlCQUFBO0FDckRoQjs7QUR3RFk7RUFFSSxnQkFBQTtBQ3ZEaEI7O0FENERJO0VBQ0ksbUJBQUE7QUMxRFI7O0FENkRJO0VBQ0ksbUJBQUE7QUMzRFI7O0FEOERJO0VBQ0ksbUJBQUE7QUM1RFI7O0FEK0RJO0VBQ0ksbUJBQUE7QUM3RFI7O0FEZ0VJO0VBQ0ksbUJBQUE7QUM5RFI7O0FEaUVJO0VBQ0ksbUJBQUE7QUMvRFI7O0FEa0VJO0VBQ0ksbUJBQUE7QUNoRVI7O0FEbUVJO0VBQ0ksaUJBQUE7RUFDQSxjQUFBO0VBQ0EsZ0JBQUE7RUFDQSxjQUFBO0FDakVSOztBRG9FSTtFQUNJLFdBQUE7RUFDQSxZQUFBO0VBQ0Esa0JBQUE7RUFDQSxhQUFBO0VBQ0EsbUJBQUE7RUFDQSx1QkFBQTtBQ2xFUjs7QURxRUk7RUFDSSxtQkFBQTtFQUNBLGVBQUE7QUNuRVI7O0FEdUVBO0VBQ0ksVUFBQTtFQUNBLFVBQUE7RUFDQSxlQUFBO0VBQ0EsV0FBQTtBQ3BFSjs7QUR1RUEsNkJBQUE7O0FBQ0E7RUFDSSxZQUFBO0VBQ0EsV0FBQTtFQUNBLGtCQUFBO0VBQ0EscUJBQUE7QUNwRUo7O0FEdUVBO0VBQ0ksY0FBQTtBQ3BFSjs7QUR1RUE7RUFDSSwyQkFBQTtBQ3BFSjs7QUR1RUE7RUFDSSxnQkFBQTtBQ3BFSjs7QUR1RUE7RUFDSSw0QkFBQTtBQ3BFSjs7QUR1RUE7RUFDSSw0QkFBQTtBQ3BFSiIsImZpbGUiOiJzcmMvYXBwL3NhbGVzLXVzZXIvc2FsZXMtdXNlci1kYXNoYm9hcmQvc2FsZXMtdXNlci1kYXNoYm9hcmQuY29tcG9uZW50LnNjc3MiLCJzb3VyY2VzQ29udGVudCI6WyIudG9vbHRpcC1kb3Qge1xyXG4gICAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcclxuICAgIHRleHQtb3ZlcmZsb3c6IGVsbGlwc2lzO1xyXG4gICAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICAgIHdpZHRoOiAxMDAlO1xyXG59XHJcbi5tb2RhbC10aXRsZSB7XHJcbiAgICBmb250LXNpemU6IDIwcHhcclxufVxyXG5cclxuLmFycm93IHtcclxuICAgIGN1cnNvcjogcG9pbnRlcjtcclxuICAgIGZvbnQtc2l6ZTogMjBweFxyXG59XHJcblxyXG4uYWRkUHJvZFRlbXBsYXRlIHtcclxuICAgIGJvcmRlcjogMXB4IHNvbGlkICNlYmViZWI7XHJcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xyXG59XHJcblxyXG4jdG9vbHRpcCB7XHJcbiAgICBkaXNwbGF5OiBibG9jaztcclxuICAgIGJhY2tncm91bmQ6IGJsYWNrO1xyXG4gICAgY29sb3I6IHdoaXRlO1xyXG4gICAgcGFkZGluZzogNXB4O1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgdG9wOiAxMzFweDtcclxuICAgIHdpZHRoOiA1OTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6NXB4XHJcbn1cclxuXHJcbi5tb2RhbCB7XHJcbiAgIGJhY2tncm91bmQtY29sb3I6IHJnYmEoMCwwLDAsMC4zKVxyXG59XHJcbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIge1xyXG4gICAgLndlbGwge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNGRkZGRkY7XHJcbiAgICAgICAgcGFkZGluZzogMDtcclxuXHJcbiAgICAgICAgLndlbGwtaGVhZGVyIHtcclxuICAgICAgICAgICAgYm9yZGVyLXJhZGl1czogMjRweCAyNHB4IDBweCAwcHg7XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDEwcHggMjBweDtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC53ZWxsLXRpdGxlIHtcclxuICAgICAgICAgICAgZm9udC13ZWlnaHQ6IDYwMCAhaW1wb3J0YW50O1xyXG4gICAgICAgICAgICBmb250LXNpemU6IDE4cHggIWltcG9ydGFudDtcclxuICAgICAgICAgICAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcclxuICAgICAgICAgICAgY29sb3I6ICMxMjEyMUYgIWltcG9ydGFudDtcclxuICAgICAgICAgICAgbWFyZ2luLWJvdHRvbTogMDtcclxuICAgICAgICAgICAgcGFkZGluZzogMCAhaW1wb3J0YW50O1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLndlbGwtYm9keSB7XHJcbiAgICAgICAgICAgIHBhZGRpbmc6IDBweDtcclxuICAgICAgICAgICAgbWluLWhlaWdodDogMzUwcHg7XHJcblxyXG4gICAgICAgICAgICAmLnBhZGRpbmctOCB7XHJcbiAgICAgICAgICAgICAgICBwYWRkaW5nOiAwIDE1cHg7XHJcbiAgICAgICAgICAgICAgICBwYWRkaW5nLWJvdHRvbTogNXB4O1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIC8vIHRib2R5IHtcclxuICAgICAgICAgICAgLy8gICAgIGRpc3BsYXk6YmxvY2s7XHJcbiAgICAgICAgICAgIC8vICAgICBtYXgtaGVpZ2h0OjE2MHB4O1xyXG4gICAgICAgICAgICAvLyAgICAgb3ZlcmZsb3cteTpzY3JvbGw7XHJcbiAgICAgICAgICAgIC8vIH1cclxuICAgICAgICAgICAgLy8gdGhlYWQsIHRib2R5IHRyIHtcclxuICAgICAgICAgICAgLy8gICAgIC8vIGRpc3BsYXk6dGFibGU7XHJcbiAgICAgICAgICAgIC8vICAgICAvLyB3aWR0aDoxMDAlO1xyXG4gICAgICAgICAgICAvLyAgICAgdGFibGUtbGF5b3V0OmZpeGVkO1xyXG4gICAgICAgICAgICAvLyB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAuYnRuLXByaW1hcnkuYnRuLW91dGxpbmUge1xyXG4gICAgICAgICAgICBjb2xvcjogIzEwNjJkMTtcclxuICAgICAgICAgICAgZm9udC13ZWlnaHQ6IDYwMDtcclxuICAgICAgICAgICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgICAgICAgICBsaW5lLWhlaWdodDogMThweDtcclxuXHJcbiAgICAgICAgICAgICY6aG92ZXIge1xyXG4gICAgICAgICAgICAgICAgY29sb3I6ICNGRkZGRkY7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICY6YWN0aXZlIHtcclxuICAgICAgICAgICAgICAgIGNvbG9yOiAjRkZGRkZGO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAmOmZvY3VzIHtcclxuICAgICAgICAgICAgICAgIGNvbG9yOiAjRkZGRkZGO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC5yYWRpdXMtY2Fwc3VsZSB7XHJcbiAgICAgICAgLmJ0biB7XHJcbiAgICAgICAgICAgIGxpbmUtaGVpZ2h0OiAyMnB4O1xyXG4gICAgICAgICAgICBwYWRkaW5nOiA1cHggOHB4ICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgICAgIGZvbnQtc2l6ZTogMTJweDtcclxuXHJcbiAgICAgICAgICAgICY6bm90KDpsYXN0LWNoaWxkKSB7XHJcbiAgICAgICAgICAgICAgICBtYXJnaW4tcmlnaHQ6IDVweDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICAubWFwLXZpZXctY29udGFuaWVyIHtcclxuICAgICAgICAuYWdtLW1hcC1jb250YWluZXItaW5uZXIge1xyXG4gICAgICAgICAgICBib3JkZXItYm90dG9tLWxlZnQtcmFkaXVzOiAxNXB4O1xyXG4gICAgICAgICAgICBib3JkZXItYm90dG9tLXJpZ2h0LXJhZGl1czogMTVweDtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC5pbnB1dC1zZWFyY2gge1xyXG5cclxuICAgICAgICAgICAgJi5mb3JtLWdyb3VwIHtcclxuICAgICAgICAgICAgICAgIG1hcmdpbi1ib3R0b206IDBweDtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgLmZvcm0tY29udHJvbCB7XHJcbiAgICAgICAgICAgICAgICBib3JkZXItcmFkaXVzOiAxNXB4O1xyXG4gICAgICAgICAgICAgICAgcGFkZGluZy1sZWZ0OiAyLjM3NXJlbTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgLmZvcm0tY29udHJvbC1zZWFyY2gge1xyXG4gICAgICAgICAgICAgICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgICAgICAgICAgICAgei1pbmRleDogMjtcclxuICAgICAgICAgICAgICAgIGRpc3BsYXk6IGJsb2NrO1xyXG4gICAgICAgICAgICAgICAgd2lkdGg6IDIuMzc1cmVtO1xyXG4gICAgICAgICAgICAgICAgaGVpZ2h0OiAyLjM3NXJlbTtcclxuICAgICAgICAgICAgICAgIGxpbmUtaGVpZ2h0OiAyLjM3NXJlbTtcclxuICAgICAgICAgICAgICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgICAgICAgICAgICAgIHBvaW50ZXItZXZlbnRzOiBub25lO1xyXG4gICAgICAgICAgICAgICAgY29sb3I6ICNhYWE7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgLnByb3JpdHktcGlsbHMge1xyXG4gICAgICAgIC5uYXYtcGlsbHMge1xyXG4gICAgICAgICAgICBib3JkZXItcmFkaXVzOiA0MHB4O1xyXG4gICAgICAgICAgICBib3JkZXI6IDFweCBzb2xpZCAjZGVlMmU2ICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgICAgIGJveC1zaGFkb3c6IDAgMnB4IDRweCByZ2JhKDAsIDAsIDAsIC4wOCk7XHJcblxyXG4gICAgICAgICAgICAubmF2LWl0ZW0ge1xyXG4gICAgICAgICAgICAgICAgLm5hdi1saW5rIHtcclxuICAgICAgICAgICAgICAgICAgICBwYWRkaW5nOiA1cHggMTVweDtcclxuICAgICAgICAgICAgICAgICAgICBmb250LXNpemU6IDEzcHg7XHJcbiAgICAgICAgICAgICAgICAgICAgbGluZS1oZWlnaHQ6IDIycHg7XHJcbiAgICAgICAgICAgICAgICAgICAgYm9yZGVyLXJhZGl1czogNTBweDtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgJi5tdXN0Z28ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBjb2xvcjogI0JFNDI0MjtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICY6aG92ZXIge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYmFja2dyb3VuZDogI0JFNDI0MjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGNvbG9yOiAjZmZmZmZmO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAmLmFjdGl2ZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBiYWNrZ3JvdW5kOiAjQkU0MjQyO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgY29sb3I6ICNmZmZmZmY7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICYuc2hvdWxkZ28ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBjb2xvcjogI0U4OTMzMDtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICY6aG92ZXIge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYmFja2dyb3VuZDogI0U4OTMzMDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGNvbG9yOiAjZmZmZmZmO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAmLmFjdGl2ZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBiYWNrZ3JvdW5kOiAjRTg5MzMwO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgY29sb3I6ICNmZmZmZmY7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICYuY291bGRnbyB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGNvbG9yOiAjNjk2OTY5O1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgJjpob3ZlciB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBiYWNrZ3JvdW5kOiAjNjk2OTY5O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgY29sb3I6ICNmZmZmZmY7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICYuYWN0aXZlIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJhY2tncm91bmQ6ICM2OTY5Njk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBjb2xvcjogI2ZmZmZmZjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICAmOm5vdCg6bGFzdC1jaGlsZCkge1xyXG4gICAgICAgICAgICAgICAgICAgIG1hcmdpbi1yaWdodDogM3B4O1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC5iYWRnZSB7XHJcbiAgICAgICAgcGFkZGluZzogN3B4IDEwcHg7XHJcblxyXG4gICAgICAgICYuYmFkZ2Utc3VjY2VzcyB7XHJcbiAgICAgICAgICAgIGNvbG9yOiAjZmZmICFpbXBvcnRhbnQ7XHJcbiAgICAgICAgICAgIGJhY2tncm91bmQtY29sb3I6ICM1MkFCMzQgIWltcG9ydGFudDtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgLmJ0bi1maWx0ZXIgaW5wdXRbdHlwZT0ndGV4dCddLmRhdGVwaWNrZXIge1xyXG4gICAgICAgIGhlaWdodDogMzJweCAhaW1wb3J0YW50O1xyXG4gICAgfVxyXG5cclxuICAgIC50YWJsZS1oZWlnaHQtMjcwIHtcclxuICAgICAgICBtaW4taGVpZ2h0OiAyNzBweDtcclxuICAgIH1cclxuXHJcblxyXG5cclxuICAgIC50ZXh0LXdhcmFwcGVyLTUwIHtcclxuICAgICAgICBtYXgtd2lkdGg6IDcwcHg7XHJcbiAgICAgICAgd29yZC13cmFwOiBicmVhay13b3JkO1xyXG4gICAgfVxyXG5cclxuICAgIC50YWJsZS13cmFwcGVyIHtcclxuICAgICAgICBvdmVyZmxvdy15OiBhdXRvO1xyXG4gICAgICAgIGZsZXgtZ3JvdzogMTtcclxuICAgICAgICBtYXgtaGVpZ2h0OiAzMDBweDtcclxuICAgICAgICBtaW4taGVpZ2h0OiAyOTBweDtcclxuXHJcbiAgICAgICAgdGFibGUge1xyXG4gICAgICAgICAgICBib3JkZXItY29sbGFwc2U6IGNvbGxhcHNlO1xyXG4gICAgICAgICAgICB3aWR0aDogMTAwJTtcclxuXHJcbiAgICAgICAgICAgIHRoIHtcclxuICAgICAgICAgICAgICAgIHBvc2l0aW9uOiBzdGlja3k7XHJcbiAgICAgICAgICAgICAgICB0b3A6IDA7XHJcbiAgICAgICAgICAgICAgICBiYWNrZ3JvdW5kOiB3aGl0ZTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgdGQsIHRoIHtcclxuICAgICAgICAgICAgICAgIC8vIHBhZGRpbmc6IDEwcHg7XHJcbiAgICAgICAgICAgICAgICB0ZXh0LWFsaWduOiBsZWZ0O1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC5iZy1zY2hlZHVsZWQge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICM4N2RiODc7XHJcbiAgICB9XHJcblxyXG4gICAgLmJnLWlucHJvZ3Jlc3Mge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICMwYzUyYjE7XHJcbiAgICB9XHJcblxyXG4gICAgLmJnLWNhbmNlbGxlZCB7XHJcbiAgICAgICAgYmFja2dyb3VuZDogI2Y1MTYxNjtcclxuICAgIH1cclxuXHJcbiAgICAuYmctY29tcGxldGVkIHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjMTRhZDE0O1xyXG4gICAgfVxyXG5cclxuICAgIC5iZy1tdXN0Z28ge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNkOTVhNjc7XHJcbiAgICB9XHJcblxyXG4gICAgLmJnLXNob3VsZGdvIHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjZWM5ZjVhO1xyXG4gICAgfVxyXG5cclxuICAgIC5iZy1jb3VsZGdvIHtcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjYTVhNWE1O1xyXG4gICAgfVxyXG5cclxuICAgIC5kcml2ZXItbGlzdCB7XHJcbiAgICAgICAgbWF4LWhlaWdodDogMzM1cHg7XHJcbiAgICAgICAgb3ZlcmZsb3c6IGF1dG87XHJcbiAgICAgICAgbWFyZ2luLXRvcDogMTBweDtcclxuICAgICAgICBwYWRkaW5nOiAwIDhweDtcclxuICAgIH1cclxuXHJcbiAgICAuZHJpdmVyLWluaXRpYWxzIHtcclxuICAgICAgICB3aWR0aDogMzZweDtcclxuICAgICAgICBoZWlnaHQ6IDM2cHg7XHJcbiAgICAgICAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG4gICAgICAgIGRpc3BsYXk6IGZsZXg7XHJcbiAgICAgICAgYWxpZ24taXRlbXM6IGNlbnRlcjtcclxuICAgICAgICBqdXN0aWZ5LWNvbnRlbnQ6IGNlbnRlcjtcclxuICAgIH1cclxuXHJcbiAgICAuZHJpdmVyLWRldGFpbHM6aG92ZXIge1xyXG4gICAgICAgIGJhY2tncm91bmQ6ICNmN2Y3Zjc7XHJcbiAgICAgICAgY3Vyc29yOiBwb2ludGVyO1xyXG4gICAgfVxyXG59XHJcblxyXG4uaWNvbi13cmFwcGVyMiB7XHJcbiAgICByaWdodDogNXB4O1xyXG4gICAgdG9wOiAxNTBweDtcclxuICAgIHBvc2l0aW9uOiBmaXhlZDtcclxuICAgIHotaW5kZXg6IDk5O1xyXG59XHJcblxyXG4vKiBTYWxlcyBVc2VyIERSIEVudHJ5IEZvcm0gKi9cclxuLmRvdCB7XHJcbiAgICBoZWlnaHQ6IDEwcHg7XHJcbiAgICB3aWR0aDogMTBweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxufVxyXG5cclxuLm1vZGFsLnNob3cge1xyXG4gICAgZGlzcGxheTogYmxvY2s7XHJcbn1cclxuXHJcbi5mb250LTgwMCB7XHJcbiAgICBmb250LXdlaWdodDogODAwICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5tb2RhbC1kaWFsb2cge1xyXG4gICAgbWF4LXdpZHRoOiA2MDBweDtcclxufVxyXG5cclxuLnBhZC1sZWZ0LTQge1xyXG4gICAgcGFkZGluZy1sZWZ0OiA0cHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLnBhZC10b3AtMjYge1xyXG4gICAgcGFkZGluZy10b3A6IDI2cHggIWltcG9ydGFudDtcclxufVxyXG4iLCIudG9vbHRpcC1kb3Qge1xuICB3aGl0ZS1zcGFjZTogbm93cmFwO1xuICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcbiAgb3ZlcmZsb3c6IGhpZGRlbjtcbiAgd2lkdGg6IDEwMCU7XG59XG5cbi5tb2RhbC10aXRsZSB7XG4gIGZvbnQtc2l6ZTogMjBweDtcbn1cblxuLmFycm93IHtcbiAgY3Vyc29yOiBwb2ludGVyO1xuICBmb250LXNpemU6IDIwcHg7XG59XG5cbi5hZGRQcm9kVGVtcGxhdGUge1xuICBib3JkZXI6IDFweCBzb2xpZCAjZWJlYmViO1xuICBib3JkZXItcmFkaXVzOiAxMHB4O1xufVxuXG4jdG9vbHRpcCB7XG4gIGRpc3BsYXk6IGJsb2NrO1xuICBiYWNrZ3JvdW5kOiBibGFjaztcbiAgY29sb3I6IHdoaXRlO1xuICBwYWRkaW5nOiA1cHg7XG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcbiAgdG9wOiAxMzFweDtcbiAgd2lkdGg6IDU5MHB4O1xuICBib3JkZXItcmFkaXVzOiA1cHg7XG59XG5cbi5tb2RhbCB7XG4gIGJhY2tncm91bmQtY29sb3I6IHJnYmEoMCwgMCwgMCwgMC4zKTtcbn1cblxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAud2VsbCB7XG4gIGJhY2tncm91bmQ6ICNGRkZGRkY7XG4gIHBhZGRpbmc6IDA7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC53ZWxsIC53ZWxsLWhlYWRlciB7XG4gIGJvcmRlci1yYWRpdXM6IDI0cHggMjRweCAwcHggMHB4O1xuICBwYWRkaW5nOiAxMHB4IDIwcHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC53ZWxsIC53ZWxsLXRpdGxlIHtcbiAgZm9udC13ZWlnaHQ6IDYwMCAhaW1wb3J0YW50O1xuICBmb250LXNpemU6IDE4cHggIWltcG9ydGFudDtcbiAgbGV0dGVyLXNwYWNpbmc6IDAuMjVweDtcbiAgY29sb3I6ICMxMjEyMUYgIWltcG9ydGFudDtcbiAgbWFyZ2luLWJvdHRvbTogMDtcbiAgcGFkZGluZzogMCAhaW1wb3J0YW50O1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAud2VsbCAud2VsbC1ib2R5IHtcbiAgcGFkZGluZzogMHB4O1xuICBtaW4taGVpZ2h0OiAzNTBweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLndlbGwgLndlbGwtYm9keS5wYWRkaW5nLTgge1xuICBwYWRkaW5nOiAwIDE1cHg7XG4gIHBhZGRpbmctYm90dG9tOiA1cHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC53ZWxsIC5idG4tcHJpbWFyeS5idG4tb3V0bGluZSB7XG4gIGNvbG9yOiAjMTA2MmQxO1xuICBmb250LXdlaWdodDogNjAwO1xuICBmb250LXNpemU6IDE0cHg7XG4gIGxpbmUtaGVpZ2h0OiAxOHB4O1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAud2VsbCAuYnRuLXByaW1hcnkuYnRuLW91dGxpbmU6aG92ZXIge1xuICBjb2xvcjogI0ZGRkZGRjtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLndlbGwgLmJ0bi1wcmltYXJ5LmJ0bi1vdXRsaW5lOmFjdGl2ZSB7XG4gIGNvbG9yOiAjRkZGRkZGO1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAud2VsbCAuYnRuLXByaW1hcnkuYnRuLW91dGxpbmU6Zm9jdXMge1xuICBjb2xvcjogI0ZGRkZGRjtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnJhZGl1cy1jYXBzdWxlIC5idG4ge1xuICBsaW5lLWhlaWdodDogMjJweDtcbiAgcGFkZGluZzogNXB4IDhweCAhaW1wb3J0YW50O1xuICBmb250LXNpemU6IDEycHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5yYWRpdXMtY2Fwc3VsZSAuYnRuOm5vdCg6bGFzdC1jaGlsZCkge1xuICBtYXJnaW4tcmlnaHQ6IDVweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLm1hcC12aWV3LWNvbnRhbmllciAuYWdtLW1hcC1jb250YWluZXItaW5uZXIge1xuICBib3JkZXItYm90dG9tLWxlZnQtcmFkaXVzOiAxNXB4O1xuICBib3JkZXItYm90dG9tLXJpZ2h0LXJhZGl1czogMTVweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLm1hcC12aWV3LWNvbnRhbmllciAuaW5wdXQtc2VhcmNoLmZvcm0tZ3JvdXAge1xuICBtYXJnaW4tYm90dG9tOiAwcHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5tYXAtdmlldy1jb250YW5pZXIgLmlucHV0LXNlYXJjaCAuZm9ybS1jb250cm9sIHtcbiAgYm9yZGVyLXJhZGl1czogMTVweDtcbiAgcGFkZGluZy1sZWZ0OiAyLjM3NXJlbTtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLm1hcC12aWV3LWNvbnRhbmllciAuaW5wdXQtc2VhcmNoIC5mb3JtLWNvbnRyb2wtc2VhcmNoIHtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICB6LWluZGV4OiAyO1xuICBkaXNwbGF5OiBibG9jaztcbiAgd2lkdGg6IDIuMzc1cmVtO1xuICBoZWlnaHQ6IDIuMzc1cmVtO1xuICBsaW5lLWhlaWdodDogMi4zNzVyZW07XG4gIHRleHQtYWxpZ246IGNlbnRlcjtcbiAgcG9pbnRlci1ldmVudHM6IG5vbmU7XG4gIGNvbG9yOiAjYWFhO1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAucHJvcml0eS1waWxscyAubmF2LXBpbGxzIHtcbiAgYm9yZGVyLXJhZGl1czogNDBweDtcbiAgYm9yZGVyOiAxcHggc29saWQgI2RlZTJlNiAhaW1wb3J0YW50O1xuICBib3gtc2hhZG93OiAwIDJweCA0cHggcmdiYSgwLCAwLCAwLCAwLjA4KTtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rIHtcbiAgcGFkZGluZzogNXB4IDE1cHg7XG4gIGZvbnQtc2l6ZTogMTNweDtcbiAgbGluZS1oZWlnaHQ6IDIycHg7XG4gIGJvcmRlci1yYWRpdXM6IDUwcHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5wcm9yaXR5LXBpbGxzIC5uYXYtcGlsbHMgLm5hdi1pdGVtIC5uYXYtbGluay5tdXN0Z28ge1xuICBjb2xvcjogI0JFNDI0Mjtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rLm11c3Rnbzpob3ZlciB7XG4gIGJhY2tncm91bmQ6ICNCRTQyNDI7XG4gIGNvbG9yOiAjZmZmZmZmO1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAucHJvcml0eS1waWxscyAubmF2LXBpbGxzIC5uYXYtaXRlbSAubmF2LWxpbmsubXVzdGdvLmFjdGl2ZSB7XG4gIGJhY2tncm91bmQ6ICNCRTQyNDI7XG4gIGNvbG9yOiAjZmZmZmZmO1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAucHJvcml0eS1waWxscyAubmF2LXBpbGxzIC5uYXYtaXRlbSAubmF2LWxpbmsuc2hvdWxkZ28ge1xuICBjb2xvcjogI0U4OTMzMDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rLnNob3VsZGdvOmhvdmVyIHtcbiAgYmFja2dyb3VuZDogI0U4OTMzMDtcbiAgY29sb3I6ICNmZmZmZmY7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5wcm9yaXR5LXBpbGxzIC5uYXYtcGlsbHMgLm5hdi1pdGVtIC5uYXYtbGluay5zaG91bGRnby5hY3RpdmUge1xuICBiYWNrZ3JvdW5kOiAjRTg5MzMwO1xuICBjb2xvcjogI2ZmZmZmZjtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rLmNvdWxkZ28ge1xuICBjb2xvcjogIzY5Njk2OTtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rLmNvdWxkZ286aG92ZXIge1xuICBiYWNrZ3JvdW5kOiAjNjk2OTY5O1xuICBjb2xvcjogI2ZmZmZmZjtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnByb3JpdHktcGlsbHMgLm5hdi1waWxscyAubmF2LWl0ZW0gLm5hdi1saW5rLmNvdWxkZ28uYWN0aXZlIHtcbiAgYmFja2dyb3VuZDogIzY5Njk2OTtcbiAgY29sb3I6ICNmZmZmZmY7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5wcm9yaXR5LXBpbGxzIC5uYXYtcGlsbHMgLm5hdi1pdGVtOm5vdCg6bGFzdC1jaGlsZCkge1xuICBtYXJnaW4tcmlnaHQ6IDNweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLmJhZGdlIHtcbiAgcGFkZGluZzogN3B4IDEwcHg7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5iYWRnZS5iYWRnZS1zdWNjZXNzIHtcbiAgY29sb3I6ICNmZmYgIWltcG9ydGFudDtcbiAgYmFja2dyb3VuZC1jb2xvcjogIzUyQUIzNCAhaW1wb3J0YW50O1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAuYnRuLWZpbHRlciBpbnB1dFt0eXBlPXRleHRdLmRhdGVwaWNrZXIge1xuICBoZWlnaHQ6IDMycHggIWltcG9ydGFudDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnRhYmxlLWhlaWdodC0yNzAge1xuICBtaW4taGVpZ2h0OiAyNzBweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnRleHQtd2FyYXBwZXItNTAge1xuICBtYXgtd2lkdGg6IDcwcHg7XG4gIHdvcmQtd3JhcDogYnJlYWstd29yZDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnRhYmxlLXdyYXBwZXIge1xuICBvdmVyZmxvdy15OiBhdXRvO1xuICBmbGV4LWdyb3c6IDE7XG4gIG1heC1oZWlnaHQ6IDMwMHB4O1xuICBtaW4taGVpZ2h0OiAyOTBweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnRhYmxlLXdyYXBwZXIgdGFibGUge1xuICBib3JkZXItY29sbGFwc2U6IGNvbGxhcHNlO1xuICB3aWR0aDogMTAwJTtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLnRhYmxlLXdyYXBwZXIgdGFibGUgdGgge1xuICBwb3NpdGlvbjogc3RpY2t5O1xuICB0b3A6IDA7XG4gIGJhY2tncm91bmQ6IHdoaXRlO1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAudGFibGUtd3JhcHBlciB0YWJsZSB0ZCwgLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAudGFibGUtd3JhcHBlciB0YWJsZSB0aCB7XG4gIHRleHQtYWxpZ246IGxlZnQ7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5iZy1zY2hlZHVsZWQge1xuICBiYWNrZ3JvdW5kOiAjODdkYjg3O1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAuYmctaW5wcm9ncmVzcyB7XG4gIGJhY2tncm91bmQ6ICMwYzUyYjE7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5iZy1jYW5jZWxsZWQge1xuICBiYWNrZ3JvdW5kOiAjZjUxNjE2O1xufVxuLnNhbGVzLXVzZXItZGFzaGJvYXJkLWNvbnRhaW5lciAuYmctY29tcGxldGVkIHtcbiAgYmFja2dyb3VuZDogIzE0YWQxNDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLmJnLW11c3RnbyB7XG4gIGJhY2tncm91bmQ6ICNkOTVhNjc7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5iZy1zaG91bGRnbyB7XG4gIGJhY2tncm91bmQ6ICNlYzlmNWE7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5iZy1jb3VsZGdvIHtcbiAgYmFja2dyb3VuZDogI2E1YTVhNTtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLmRyaXZlci1saXN0IHtcbiAgbWF4LWhlaWdodDogMzM1cHg7XG4gIG92ZXJmbG93OiBhdXRvO1xuICBtYXJnaW4tdG9wOiAxMHB4O1xuICBwYWRkaW5nOiAwIDhweDtcbn1cbi5zYWxlcy11c2VyLWRhc2hib2FyZC1jb250YWluZXIgLmRyaXZlci1pbml0aWFscyB7XG4gIHdpZHRoOiAzNnB4O1xuICBoZWlnaHQ6IDM2cHg7XG4gIHRleHQtYWxpZ246IGNlbnRlcjtcbiAgZGlzcGxheTogZmxleDtcbiAgYWxpZ24taXRlbXM6IGNlbnRlcjtcbiAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XG59XG4uc2FsZXMtdXNlci1kYXNoYm9hcmQtY29udGFpbmVyIC5kcml2ZXItZGV0YWlsczpob3ZlciB7XG4gIGJhY2tncm91bmQ6ICNmN2Y3Zjc7XG4gIGN1cnNvcjogcG9pbnRlcjtcbn1cblxuLmljb24td3JhcHBlcjIge1xuICByaWdodDogNXB4O1xuICB0b3A6IDE1MHB4O1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHotaW5kZXg6IDk5O1xufVxuXG4vKiBTYWxlcyBVc2VyIERSIEVudHJ5IEZvcm0gKi9cbi5kb3Qge1xuICBoZWlnaHQ6IDEwcHg7XG4gIHdpZHRoOiAxMHB4O1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcbn1cblxuLm1vZGFsLnNob3cge1xuICBkaXNwbGF5OiBibG9jaztcbn1cblxuLmZvbnQtODAwIHtcbiAgZm9udC13ZWlnaHQ6IDgwMCAhaW1wb3J0YW50O1xufVxuXG4ubW9kYWwtZGlhbG9nIHtcbiAgbWF4LXdpZHRoOiA2MDBweDtcbn1cblxuLnBhZC1sZWZ0LTQge1xuICBwYWRkaW5nLWxlZnQ6IDRweCAhaW1wb3J0YW50O1xufVxuXG4ucGFkLXRvcC0yNiB7XG4gIHBhZGRpbmctdG9wOiAyNnB4ICFpbXBvcnRhbnQ7XG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵsetClassMetadata"](SalesUserDashboardComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"],
        args: [{
          selector: 'app-sales-user-dashboard',
          templateUrl: './sales-user-dashboard.component.html',
          styleUrls: ['./sales-user-dashboard.component.scss']
        }]
      }], function () {
        return [{
          type: _sales_user_service__WEBPACK_IMPORTED_MODULE_7__["SalesUserService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_8__["Router"]
        }, {
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_3__["FormBuilder"]
        }, {
          type: src_app_fuelsurcharge_services_fuelsurcharge_service__WEBPACK_IMPORTED_MODULE_9__["FuelSurchargeService"]
        }];
      }, {
        SelectedDate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_1__["Input"]
        }]
      });
    })();
    /***/

  },

  /***/
  "./src/app/sales-user/sales-user.model.ts": function srcAppSalesUserSalesUserModelTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Geocode", function () {
      return Geocode;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapIconUrl", function () {
      return MapIconUrl;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapIconSize", function () {
      return MapIconSize;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MapConstants", function () {
      return MapConstants;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FeeModel", function () {
      return FeeModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ByQuantityModel", function () {
      return ByQuantityModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ContactPersonModel", function () {
      return ContactPersonModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "GeneralNote", function () {
      return GeneralNote;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesDRModel", function () {
      return SalesDRModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CustomersAndJobs", function () {
      return CustomersAndJobs;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DDL", function () {
      return DDL;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ProductsGroup", function () {
      return ProductsGroup;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CustomersModel", function () {
      return CustomersModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionsAndJobsModel", function () {
      return RegionsAndJobsModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesUserDRStatus", function () {
      return SalesUserDRStatus;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ConfirmDRStatus", function () {
      return ConfirmDRStatus;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesUserDRStatusModel", function () {
      return SalesUserDRStatusModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesUserDRProductStatus", function () {
      return SalesUserDRProductStatus;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ValidateDREntryResponse", function () {
      return ValidateDREntryResponse;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DeliveryRequestInputModel", function () {
      return DeliveryRequestInputModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CustomCompanyModel", function () {
      return CustomCompanyModel;
    });

    var Geocode = function Geocode() {
      _classCallCheck(this, Geocode);
    };

    var MapIconUrl = function MapIconUrl() {
      _classCallCheck(this, MapIconUrl);
    };

    var MapIconSize = function MapIconSize() {
      _classCallCheck(this, MapIconSize);
    };

    var MapConstants = function MapConstants() {
      _classCallCheck(this, MapConstants);

      this.CenterLat = 38;
      this.CenterLon = -98.35;
      this.ZoomArea = 15;
      this.IconUrl = {
        url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png',
        scaledSize: {
          width: 40,
          height: 40
        }
      };
    };

    var FeeModel = function FeeModel() {
      _classCallCheck(this, FeeModel);
    };

    var ByQuantityModel = function ByQuantityModel() {
      _classCallCheck(this, ByQuantityModel);

      this.MinQuantity = 0;
    };

    var ContactPersonModel = /*#__PURE__*/function () {
      function ContactPersonModel() {
        _classCallCheck(this, ContactPersonModel);
      }

      _createClass(ContactPersonModel, [{
        key: "ContactPersonModel",
        value: function ContactPersonModel() {
          this.IsPhoneNumberConfirmed = true;
        }
      }]);

      return ContactPersonModel;
    }();

    var GeneralNote = function GeneralNote() {
      _classCallCheck(this, GeneralNote);
    };

    var SalesDRModel = function SalesDRModel() {
      _classCallCheck(this, SalesDRModel);

      this.CompanyId = 0;
      this.CompanyId = 0;
      this.CompanyName = "";
      this.DRNotes = "";
      this.JobId = 0;
      this.JobName = "";
      this.Products = [];
    };

    var CustomersAndJobs = function CustomersAndJobs() {
      _classCallCheck(this, CustomersAndJobs);
    };

    var DDL = function DDL() {
      _classCallCheck(this, DDL);
    };

    var ProductsGroup = function ProductsGroup() {
      _classCallCheck(this, ProductsGroup);
    };

    var CustomersModel = function CustomersModel() {
      _classCallCheck(this, CustomersModel);

      this.regionsAndJobsModels = [];
      this.customersandJobs = [];
    };

    var RegionsAndJobsModel = function RegionsAndJobsModel() {
      _classCallCheck(this, RegionsAndJobsModel);
    };

    var SalesUserDRStatus;

    (function (SalesUserDRStatus) {
      SalesUserDRStatus[SalesUserDRStatus["Success"] = 1] = "Success";
      SalesUserDRStatus[SalesUserDRStatus["Error"] = 2] = "Error";
      SalesUserDRStatus[SalesUserDRStatus["RegionNotFound"] = 3] = "RegionNotFound";
      SalesUserDRStatus[SalesUserDRStatus["FuelRequestNotFound"] = 4] = "FuelRequestNotFound";
      SalesUserDRStatus[SalesUserDRStatus["OrderNotFound"] = 5] = "OrderNotFound";
    })(SalesUserDRStatus || (SalesUserDRStatus = {}));

    var ConfirmDRStatus;

    (function (ConfirmDRStatus) {
      ConfirmDRStatus[ConfirmDRStatus["Success"] = 0] = "Success";
      ConfirmDRStatus[ConfirmDRStatus["Failed"] = 1] = "Failed";
      ConfirmDRStatus[ConfirmDRStatus["Warning"] = 2] = "Warning";
    })(ConfirmDRStatus || (ConfirmDRStatus = {}));

    var SalesUserDRStatusModel = function SalesUserDRStatusModel() {
      _classCallCheck(this, SalesUserDRStatusModel);
    };

    var SalesUserDRProductStatus = function SalesUserDRProductStatus() {
      _classCallCheck(this, SalesUserDRProductStatus);
    };

    var ValidateDREntryResponse = function ValidateDREntryResponse() {
      _classCallCheck(this, ValidateDREntryResponse);
    };

    var DeliveryRequestInputModel = function DeliveryRequestInputModel() {
      _classCallCheck(this, DeliveryRequestInputModel);
    };

    var CustomCompanyModel = function CustomCompanyModel() {
      _classCallCheck(this, CustomCompanyModel);
    };
    /***/

  },

  /***/
  "./src/app/sales-user/sales-user.module.ts": function srcAppSalesUserSalesUserModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SalesUserModule", function () {
      return SalesUserModule;
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


    var _sales_user_dashboard_sales_user_dashboard_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./sales-user-dashboard/sales-user-dashboard.component */
    "./src/app/sales-user/sales-user-dashboard/sales-user-dashboard.component.ts");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _create_sourcing_request_create_sourcing_request_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ./create-sourcing-request/create-sourcing-request.component */
    "./src/app/sales-user/create-sourcing-request/create-sourcing-request.component.ts");
    /* harmony import */


    var _sourcing_request_grid_sourcing_request_grid_sourcing_request_grid_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ./sourcing-request-grid/sourcing-request-grid/sourcing-request-grid.component */
    "./src/app/sales-user/sourcing-request-grid/sourcing-request-grid/sourcing-request-grid.component.ts");
    /* harmony import */


    var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-bootstrap/ng-bootstrap */
    "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
    /* harmony import */


    var angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! angular-ng-autocomplete */
    "./node_modules/angular-ng-autocomplete/__ivy_ngcc__/fesm2015/angular-ng-autocomplete.js");
    /* harmony import */


    var angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(
    /*! angular-confirmation-popover */
    "./node_modules/angular-confirmation-popover/__ivy_ngcc__/fesm2015/angular-confirmation-popover.js");
    /* harmony import */


    var src_app_fees_fees_module__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(
    /*! src/app/fees/fees.module */
    "./src/app/fees/fees.module.ts");
    /* harmony import */


    var _shared_components_pricing_section_pricing_section_module__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(
    /*! ../shared-components/pricing-section/pricing-section.module */
    "./src/app/shared-components/pricing-section/pricing-section.module.ts");
    /* harmony import */


    var src_app_contact_person_contact_person_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(
    /*! src/app/contact-person/contact-person.component */
    "./src/app/contact-person/contact-person.component.ts");

    var route = [{
      path: 'Dashboard',
      component: _sales_user_dashboard_sales_user_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["SalesUserDashboardComponent"]
    }, {
      path: 'Dashboard/Index',
      component: _sales_user_dashboard_sales_user_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["SalesUserDashboardComponent"]
    }, {
      path: 'SourcingRequest/Create',
      component: _create_sourcing_request_create_sourcing_request_component__WEBPACK_IMPORTED_MODULE_8__["CreateSourcingRequestComponent"]
    }, {
      path: 'SourcingRequest',
      component: _sourcing_request_grid_sourcing_request_grid_sourcing_request_grid_component__WEBPACK_IMPORTED_MODULE_9__["SourcingRequestGridComponent"]
    }, {
      path: 'SourcingRequest/Index',
      component: _sourcing_request_grid_sourcing_request_grid_sourcing_request_grid_component__WEBPACK_IMPORTED_MODULE_9__["SourcingRequestGridComponent"]
    }, {
      path: 'SourcingRequest/Details/:Id',
      component: _create_sourcing_request_create_sourcing_request_component__WEBPACK_IMPORTED_MODULE_8__["CreateSourcingRequestComponent"]
    }];

    var SalesUserModule = function SalesUserModule() {
      _classCallCheck(this, SalesUserModule);
    };

    SalesUserModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: SalesUserModule
    });
    SalesUserModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function SalesUserModule_Factory(t) {
        return new (t || SalesUserModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__["AutocompleteLibModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbModule"], src_app_fees_fees_module__WEBPACK_IMPORTED_MODULE_13__["FeesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route), angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__["ConfirmationPopoverModule"].forRoot({
        confirmButtonType: 'danger'
      }), _shared_components_pricing_section_pricing_section_module__WEBPACK_IMPORTED_MODULE_14__["PricingSectionModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](SalesUserModule, {
        declarations: [_sales_user_dashboard_sales_user_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["SalesUserDashboardComponent"], _create_sourcing_request_create_sourcing_request_component__WEBPACK_IMPORTED_MODULE_8__["CreateSourcingRequestComponent"], _sourcing_request_grid_sourcing_request_grid_sourcing_request_grid_component__WEBPACK_IMPORTED_MODULE_9__["SourcingRequestGridComponent"], src_app_contact_person_contact_person_component__WEBPACK_IMPORTED_MODULE_15__["ContactPersonComponent"]],
        imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__["AutocompleteLibModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbModule"], src_app_fees_fees_module__WEBPACK_IMPORTED_MODULE_13__["FeesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"], angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__["ConfirmationPopoverModule"], _shared_components_pricing_section_pricing_section_module__WEBPACK_IMPORTED_MODULE_14__["PricingSectionModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SalesUserModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_sales_user_dashboard_sales_user_dashboard_component__WEBPACK_IMPORTED_MODULE_2__["SalesUserDashboardComponent"], _create_sourcing_request_create_sourcing_request_component__WEBPACK_IMPORTED_MODULE_8__["CreateSourcingRequestComponent"], _sourcing_request_grid_sourcing_request_grid_sourcing_request_grid_component__WEBPACK_IMPORTED_MODULE_9__["SourcingRequestGridComponent"], src_app_contact_person_contact_person_component__WEBPACK_IMPORTED_MODULE_15__["ContactPersonComponent"]],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"], _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_4__["DirectiveModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_5__["DataTablesModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"], angular_ng_autocomplete__WEBPACK_IMPORTED_MODULE_11__["AutocompleteLibModule"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_10__["NgbModule"], src_app_fees_fees_module__WEBPACK_IMPORTED_MODULE_13__["FeesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_7__["RouterModule"].forChild(route), angular_confirmation_popover__WEBPACK_IMPORTED_MODULE_12__["ConfirmationPopoverModule"].forRoot({
            confirmButtonType: 'danger'
          }), _shared_components_pricing_section_pricing_section_module__WEBPACK_IMPORTED_MODULE_14__["PricingSectionModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/sales-user/sourcing-request-grid/sourcing-request-grid/sourcing-request-grid.component.ts": function srcAppSalesUserSourcingRequestGridSourcingRequestGridSourcingRequestGridComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SourcingRequestGridComponent", function () {
      return SourcingRequestGridComponent;
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


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _sales_user_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../../sales-user.service */
    "./src/app/sales-user/sales-user.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");

    function SourcingRequestGridComponent_tr_48_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 22);
      }
    }

    var _c0 = function _c0(a0) {
      return [a0];
    };

    function SourcingRequestGridComponent_tr_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "tr");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "a", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, SourcingRequestGridComponent_tr_48_span_5_Template, 1, 0, "span", 21);

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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r1 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("routerLink", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](10, _c0, "/SalesUser/SourcingRequest/Details/" + item_r1.Id));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.RequestNumber);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.UserContext.Id != item_r1.ModifiedBy && item_r1.ModifiedBy != 0 && item_r1.Status != "Submitted" && item_r1.Status != "Lost" && !item_r1.ViewedModified);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", item_r1.JobName, " ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.FuelType);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.Quantity);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.Pricing);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.DeliveryDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.DeliveryType);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](item_r1.Status);
      }
    }

    var SourcingRequestGridComponent = /*#__PURE__*/function () {
      function SourcingRequestGridComponent(salesUserService) {
        _classCallCheck(this, SourcingRequestGridComponent);

        this.salesUserService = salesUserService;
        this.dtOptions = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.IsLoading = false;
        this.RequestStatusdata = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["SourcingRequestDisplayStatus"].All;
        this.Sourcingrequests = [];
        this.DisplayRequestStatus = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["SourcingRequestDisplayStatus"];
        this._opened = false;
        this.DispatchRegion = [];
        this.getUserContext();
      }

      _createClass(SourcingRequestGridComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.initializeGrid();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          var requestStatusdata = localStorage.getItem('SelectedSRStatus');
          var requestStatus = requestStatusdata == undefined || requestStatusdata == "" ? this.DisplayRequestStatus.All : requestStatusdata;
          this.getRequests(requestStatus);
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
        key: "getUserContext",
        value: function getUserContext() {
          var _this54 = this;

          this.salesUserService.GetUserContext().subscribe(function (data) {
            _this54.UserContext = data;
          });
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
              title: 'Sourcing Requests',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'pdf',
              title: 'Sourcing Requests',
              orientation: 'landscape',
              exportOptions: exportInvitedColumns
            }, {
              extend: 'print',
              exportOptions: exportInvitedColumns
            }],
            pagingType: 'first_last_numbers',
            order: [[0, 'desc']],
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "getRequests",
        value: function getRequests(status) {
          var _this55 = this;

          this.RequestStatusdata = status;
          localStorage.setItem('SelectedSRStatus', status);
          this.IsLoading = true;
          this.salesUserService.GetSourcingRequests(status, false).subscribe(function (data) {
            _this55.IsLoading = false;
            _this55.Sourcingrequests = data;

            _this55.refreshDatatable();
          });
        }
      }]);

      return SourcingRequestGridComponent;
    }();

    SourcingRequestGridComponent.ɵfac = function SourcingRequestGridComponent_Factory(t) {
      return new (t || SourcingRequestGridComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_sales_user_service__WEBPACK_IMPORTED_MODULE_4__["SalesUserService"]));
    };

    SourcingRequestGridComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: SourcingRequestGridComponent,
      selectors: [["app-sourcing-request-grid"]],
      viewQuery: function SourcingRequestGridComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dtElements = _t);
        }
      },
      decls: 49,
      vars: 13,
      consts: [[1, "sourcing-request-grid-container"], [1, "row"], [1, "col-lg-12"], [1, "float-right"], ["id", "daySelector", 1, "dib", "border", "pa5", "radius-capsule", "ml20"], [1, "btn-group", "btn-filter"], ["type", "radio", "id", "all", 1, "hide-element", 3, "name", "checked"], [1, "btn", "mr5", "ml0", 3, "click"], ["type", "radio", "id", "new", 1, "hide-element", 3, "name", "checked"], ["type", "radio", "id", "wip", 1, "hide-element", 3, "name", "checked"], ["type", "radio", "id", "sourced", 1, "hide-element", 3, "name", "checked"], ["type", "radio", "id", "lost", 1, "hide-element", 3, "name", "checked"], [1, "spinner-small", "fr-type", "float-left", "ml10", "mt5"], [1, "col-md-12"], [1, "well", "bg-white", "shadowb-b"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border"], ["id", "div-carrier-grid", 1, "table-responsive"], ["id", "request-grid-datatable", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], [4, "ngFor", "ngForOf"], [3, "routerLink"], ["class", "active-red-dot", 4, "ngIf"], [1, "active-red-dot"]],
      template: function SourcingRequestGridComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourcingRequestGridComponent_Template_label_click_8_listener() {
            return ctx.getRequests(ctx.DisplayRequestStatus.All);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "All");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "input", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourcingRequestGridComponent_Template_label_click_11_listener() {
            return ctx.getRequests(ctx.DisplayRequestStatus.New);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "New");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "input", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourcingRequestGridComponent_Template_label_click_14_listener() {
            return ctx.getRequests(ctx.DisplayRequestStatus.WIP);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Wip");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](16, "input", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourcingRequestGridComponent_Template_label_click_17_listener() {
            return ctx.getRequests(ctx.DisplayRequestStatus.Sourced);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](18, "Sourced");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](19, "input", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "label", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourcingRequestGridComponent_Template_label_click_20_listener() {
            return ctx.getRequests(ctx.DisplayRequestStatus.Lost);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Lost");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](22, "span", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "table", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "#Request");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Location");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Fuel Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](38, "Quantity Requested");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Pricing(USD/CAD)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](42, "Delivery Date");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Type");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "th");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](48, SourcingRequestGridComponent_tr_48_Template, 20, 12, "tr", 19);

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
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", ctx.DisplayRequestStatus)("checked", ctx.RequestStatusdata == ctx.DisplayRequestStatus.All);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", ctx.DisplayRequestStatus)("checked", ctx.RequestStatusdata == ctx.DisplayRequestStatus.New);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", ctx.DisplayRequestStatus)("checked", ctx.RequestStatusdata == ctx.DisplayRequestStatus.WIP);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", ctx.DisplayRequestStatus)("checked", ctx.RequestStatusdata == ctx.DisplayRequestStatus.Sourced);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", ctx.DisplayRequestStatus)("checked", ctx.RequestStatusdata == ctx.DisplayRequestStatus.Lost);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.Sourcingrequests);
        }
      },
      directives: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgForOf"], _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterLinkWithHref"], _angular_common__WEBPACK_IMPORTED_MODULE_5__["NgIf"]],
      styles: [".sourcing-request-grid-container[_ngcontent-%COMP%]   .active-red-dot[_ngcontent-%COMP%] {\n  height: 10px;\n  width: 10px;\n  background-color: red;\n  border-radius: 50%;\n  display: inline-block;\n  -webkit-animation: 1s infinite blink;\n  animation: 1s infinite blink;\n}\n\n  aside {\n  width: 80%;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2FsZXMtdXNlci9zb3VyY2luZy1yZXF1ZXN0LWdyaWQvc291cmNpbmctcmVxdWVzdC1ncmlkL0Q6XFxURlNjb2RlXFxTaXRlRnVlbC5FeGNoYW5nZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuU291cmNlQ29kZVxcU2l0ZUZ1ZWwuRXhjaGFuZ2UuV2ViL3NyY1xcYXBwXFxzYWxlcy11c2VyXFxzb3VyY2luZy1yZXF1ZXN0LWdyaWRcXHNvdXJjaW5nLXJlcXVlc3QtZ3JpZFxcc291cmNpbmctcmVxdWVzdC1ncmlkLmNvbXBvbmVudC5zY3NzIiwic3JjL2FwcC9zYWxlcy11c2VyL3NvdXJjaW5nLXJlcXVlc3QtZ3JpZC9zb3VyY2luZy1yZXF1ZXN0LWdyaWQvc291cmNpbmctcmVxdWVzdC1ncmlkLmNvbXBvbmVudC5zY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUNJO0VBQ0ksWUFBQTtFQUNBLFdBQUE7RUFDQSxxQkFBQTtFQUNBLGtCQUFBO0VBQ0EscUJBQUE7RUFDQSxvQ0FBQTtFQUNBLDRCQUFBO0FDQVI7O0FER0E7RUFDSSxVQUFBO0FDQUoiLCJmaWxlIjoic3JjL2FwcC9zYWxlcy11c2VyL3NvdXJjaW5nLXJlcXVlc3QtZ3JpZC9zb3VyY2luZy1yZXF1ZXN0LWdyaWQvc291cmNpbmctcmVxdWVzdC1ncmlkLmNvbXBvbmVudC5zY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLnNvdXJjaW5nLXJlcXVlc3QtZ3JpZC1jb250YWluZXJ7XHJcbiAgICAuYWN0aXZlLXJlZC1kb3Qge1xyXG4gICAgICAgIGhlaWdodDogMTBweDtcclxuICAgICAgICB3aWR0aDogMTBweDtcclxuICAgICAgICBiYWNrZ3JvdW5kLWNvbG9yOiByZWQ7XHJcbiAgICAgICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuICAgICAgICAtd2Via2l0LWFuaW1hdGlvbjogMXMgaW5maW5pdGUgYmxpbms7XHJcbiAgICAgICAgYW5pbWF0aW9uOiAxcyBpbmZpbml0ZSBibGluaztcclxuICAgIH1cclxufVxyXG46Om5nLWRlZXAgYXNpZGUge1xyXG4gICAgd2lkdGg6ODAlO1xyXG59IiwiLnNvdXJjaW5nLXJlcXVlc3QtZ3JpZC1jb250YWluZXIgLmFjdGl2ZS1yZWQtZG90IHtcbiAgaGVpZ2h0OiAxMHB4O1xuICB3aWR0aDogMTBweDtcbiAgYmFja2dyb3VuZC1jb2xvcjogcmVkO1xuICBib3JkZXItcmFkaXVzOiA1MCU7XG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcbiAgLXdlYmtpdC1hbmltYXRpb246IDFzIGluZmluaXRlIGJsaW5rO1xuICBhbmltYXRpb246IDFzIGluZmluaXRlIGJsaW5rO1xufVxuXG46Om5nLWRlZXAgYXNpZGUge1xuICB3aWR0aDogODAlO1xufSJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SourcingRequestGridComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-sourcing-request-grid',
          templateUrl: './sourcing-request-grid.component.html',
          styleUrls: ['./sourcing-request-grid.component.scss']
        }]
      }], function () {
        return [{
          type: _sales_user_service__WEBPACK_IMPORTED_MODULE_4__["SalesUserService"]
        }];
      }, {
        dtElements: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChildren"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_1__["DataTableDirective"]]
        }]
      });
    })();
    /***/

  }
}]);
//# sourceMappingURL=sales-user-sales-user-module-es5.js.map