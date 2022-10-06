function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _iterableToArrayLimit(arr, i) { var _i = arr == null ? null : typeof Symbol !== "undefined" && arr[Symbol.iterator] || arr["@@iterator"]; if (_i == null) return; var _arr = []; var _n = true; var _d = false; var _s, _e; try { for (_i = _i.call(arr); !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e2) { throw _e2; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e3) { didErr = true; err = _e3; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _unsupportedIterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _iterableToArray(iter) { if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null) return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) return _arrayLikeToArray(arr); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["company-addresses-lazy-loading-company-addresses-module"], {
  /***/
  "./node_modules/@ng-select/ng-select/__ivy_ngcc__/fesm2015/ng-select-ng-select.js": function node_modulesNgSelectNgSelect__ivy_ngcc__Fesm2015NgSelectNgSelectJs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "NgSelectComponent", function () {
      return NgSelectComponent;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "NgSelectConfig", function () {
      return NgSelectConfig;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "NgSelectModule", function () {
      return NgSelectModule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SELECTION_MODEL_FACTORY", function () {
      return SELECTION_MODEL_FACTORY;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵb", function () {
      return DefaultSelectionModelFactory;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵc", function () {
      return DefaultSelectionModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵd", function () {
      return NgDropdownPanelService;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵe", function () {
      return NgItemLabelDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵf", function () {
      return NgOptionTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵg", function () {
      return NgOptgroupTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵh", function () {
      return NgLabelTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵi", function () {
      return NgMultiLabelTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵj", function () {
      return NgHeaderTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵk", function () {
      return NgFooterTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵl", function () {
      return NgNotFoundTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵm", function () {
      return NgTypeToSearchTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵn", function () {
      return NgLoadingTextTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵo", function () {
      return NgTagTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵp", function () {
      return NgLoadingSpinnerTemplateDirective;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵq", function () {
      return NgDropdownPanelComponent;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵr", function () {
      return NgOptionComponent;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵs", function () {
      return ConsoleService;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/value-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /** @type {?} */


    var _c0 = ["content"];
    var _c1 = ["scroll"];
    var _c2 = ["padding"];

    var _c3 = function _c3(a0) {
      return {
        searchTerm: a0
      };
    };

    function NgDropdownPanelComponent_div_0_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainer"](1, 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r0.headerTemplate)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](2, _c3, ctx_r0.filterValue));
      }
    }

    function NgDropdownPanelComponent_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainer"](1, 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r4.footerTemplate)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](2, _c3, ctx_r4.filterValue));
      }
    }

    var _c4 = ["*"];
    var _c5 = ["searchInput"];

    function NgSelectComponent_ng_container_4_div_1_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function NgSelectComponent_ng_container_4_div_1_ng_template_1_Template_span_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r13);

          var item_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r11.unselect(item_r7);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "\xD7");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "span", 16);
      }

      if (rf & 2) {
        var item_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngItemLabel", item_r7.label)("escape", ctx_r9.escapeHTML);
      }
    }

    function NgSelectComponent_ng_container_4_div_1_ng_template_3_Template(rf, ctx) {}

    var _c6 = function _c6(a0, a1, a2) {
      return {
        item: a0,
        clear: a1,
        label: a2
      };
    };

    function NgSelectComponent_ng_container_4_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_container_4_div_1_ng_template_1_Template, 3, 2, "ng-template", null, 13, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_container_4_div_1_ng_template_3_Template, 0, 0, "ng-template", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r7 = ctx.$implicit;

        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-value-disabled", item_r7.disabled);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r6.labelTemplate || _r8)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction3"](4, _c6, item_r7.value, ctx_r6.clearItem, item_r7.label));
      }
    }

    function NgSelectComponent_ng_container_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_container_4_div_1_Template, 4, 8, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.selectedItems)("ngForTrackBy", ctx_r0.trackByOption);
      }
    }

    function NgSelectComponent_5_ng_template_0_Template(rf, ctx) {}

    var _c7 = function _c7(a0, a1) {
      return {
        items: a0,
        clear: a1
      };
    };

    function NgSelectComponent_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](0, NgSelectComponent_5_ng_template_0_Template, 0, 0, "ng-template", 14);
      }

      if (rf & 2) {
        var ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r1.multiLabelTemplate)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction2"](2, _c7, ctx_r1.selectedValues, ctx_r1.clearItem));
      }
    }

    function NgSelectComponent_ng_container_9_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "div", 19);
      }
    }

    function NgSelectComponent_ng_container_9_ng_template_3_Template(rf, ctx) {}

    function NgSelectComponent_ng_container_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_container_9_ng_template_1_Template, 1, 0, "ng-template", null, 17, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_container_9_ng_template_3_Template, 0, 0, "ng-template", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r3.loadingSpinnerTemplate || _r16);
      }
    }

    function NgSelectComponent_span_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 20);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "\xD7");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("title", ctx_r4.clearAllText);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_div_2_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 27);
      }

      if (rf & 2) {
        var item_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngItemLabel", item_r24.label)("escape", ctx_r26.escapeHTML);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_div_2_ng_template_3_Template(rf, ctx) {}

    var _c8 = function _c8(a0, a1, a2, a3) {
      return {
        item: a0,
        item$: a1,
        index: a2,
        searchTerm: a3
      };
    };

    function NgSelectComponent_ng_dropdown_panel_13_div_2_Template(rf, ctx) {
      if (rf & 1) {
        var _r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function NgSelectComponent_ng_dropdown_panel_13_div_2_Template_div_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r30);

          var item_r24 = ctx.$implicit;

          var ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r29.toggleItem(item_r24);
        })("mouseover", function NgSelectComponent_ng_dropdown_panel_13_div_2_Template_div_mouseover_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r30);

          var item_r24 = ctx.$implicit;

          var ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r31.onItemHover(item_r24);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_dropdown_panel_13_div_2_ng_template_1_Template, 1, 2, "ng-template", null, 26, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_div_2_ng_template_3_Template, 0, 0, "ng-template", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var item_r24 = ctx.$implicit;

        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-option-disabled", item_r24.disabled)("ng-option-selected", item_r24.selected)("ng-optgroup", item_r24.children)("ng-option", !item_r24.children)("ng-option-child", !!item_r24.parent)("ng-option-marked", item_r24 === ctx_r19.itemsList.markedItem);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("role", item_r24.children ? "group" : "option")("aria-selected", item_r24.selected)("id", item_r24 == null ? null : item_r24.htmlId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", item_r24.children ? ctx_r19.optgroupTemplate || _r25 : ctx_r19.optionTemplate || _r25)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction4"](17, _c8, item_r24.value, item_r24, item_r24.index, ctx_r19.searchTerm));
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_div_3_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r33.addTagText);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("\"", ctx_r33.searchTerm, "\"");
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_div_3_ng_template_3_Template(rf, ctx) {}

    function NgSelectComponent_ng_dropdown_panel_13_div_3_Template(rf, ctx) {
      if (rf & 1) {
        var _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("mouseover", function NgSelectComponent_ng_dropdown_panel_13_div_3_Template_div_mouseover_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r35.itemsList.unmarkItem();
        })("click", function NgSelectComponent_ng_dropdown_panel_13_div_3_Template_div_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r37.selectTag();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_dropdown_panel_13_div_3_ng_template_1_Template, 4, 2, "ng-template", null, 29, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_div_3_ng_template_3_Template, 0, 0, "ng-template", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var _r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-option-marked", !ctx_r20.itemsList.markedItem);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r20.tagTemplate || _r32)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](4, _c3, ctx_r20.searchTerm));
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_4_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r39.notFoundText);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_4_ng_template_3_Template(rf, ctx) {}

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_4_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_dropdown_panel_13_ng_container_4_ng_template_1_Template, 2, 1, "ng-template", null, 31, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_ng_container_4_ng_template_3_Template, 0, 0, "ng-template", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var _r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r21.notFoundTemplate || _r38)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](2, _c3, ctx_r21.searchTerm));
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_5_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r42 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r42.typeToSearchText);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_5_ng_template_3_Template(rf, ctx) {}

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_dropdown_panel_13_ng_container_5_ng_template_1_Template, 2, 1, "ng-template", null, 33, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_ng_container_5_ng_template_3_Template, 0, 0, "ng-template", 18);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var _r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r22.typeToSearchTemplate || _r41);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_6_ng_template_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 32);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r45 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r45.loadingText);
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_6_ng_template_3_Template(rf, ctx) {}

    function NgSelectComponent_ng_dropdown_panel_13_ng_container_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgSelectComponent_ng_dropdown_panel_13_ng_container_6_ng_template_1_Template, 2, 1, "ng-template", null, 34, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_ng_container_6_ng_template_3_Template, 0, 0, "ng-template", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var _r44 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](2);

        var ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngTemplateOutlet", ctx_r23.loadingTextTemplate || _r44)("ngTemplateOutletContext", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](2, _c3, ctx_r23.searchTerm));
      }
    }

    function NgSelectComponent_ng_dropdown_panel_13_Template(rf, ctx) {
      if (rf & 1) {
        var _r48 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "ng-dropdown-panel", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("update", function NgSelectComponent_ng_dropdown_panel_13_Template_ng_dropdown_panel_update_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r48);

          var ctx_r47 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r47.viewPortItems = $event;
        })("scroll", function NgSelectComponent_ng_dropdown_panel_13_Template_ng_dropdown_panel_scroll_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r48);

          var ctx_r49 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r49.scroll.emit($event);
        })("scrollToEnd", function NgSelectComponent_ng_dropdown_panel_13_Template_ng_dropdown_panel_scrollToEnd_0_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r48);

          var ctx_r50 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r50.scrollToEnd.emit($event);
        })("outsideClick", function NgSelectComponent_ng_dropdown_panel_13_Template_ng_dropdown_panel_outsideClick_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r48);

          var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r51.close();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, NgSelectComponent_ng_dropdown_panel_13_div_2_Template, 4, 22, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, NgSelectComponent_ng_dropdown_panel_13_div_3_Template, 4, 6, "div", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, NgSelectComponent_ng_dropdown_panel_13_ng_container_4_Template, 4, 4, "ng-container", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, NgSelectComponent_ng_dropdown_panel_13_ng_container_5_Template, 4, 1, "ng-container", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, NgSelectComponent_ng_dropdown_panel_13_ng_container_6_Template, 4, 4, "ng-container", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-select-multiple", ctx_r5.multiple);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("virtualScroll", ctx_r5.virtualScroll)("bufferAmount", ctx_r5.bufferAmount)("appendTo", ctx_r5.appendTo)("position", ctx_r5.dropdownPosition)("headerTemplate", ctx_r5.headerTemplate)("footerTemplate", ctx_r5.footerTemplate)("filterValue", ctx_r5.searchTerm)("items", ctx_r5.itemsList.filteredItems)("markedItem", ctx_r5.itemsList.markedItem)("ngClass", ctx_r5.appendTo ? ctx_r5.classes : null)("id", ctx_r5.dropdownId);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r5.viewPortItems)("ngForTrackBy", ctx_r5.trackByOption);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.showAddTag);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.showNoItemsFound());

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.showTypeToSearch());

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.loading && ctx_r5.itemsList.filteredItems.length === 0);
      }
    }

    var unescapedHTMLExp = /[&<>"']/g;
    /** @type {?} */

    var hasUnescapedHTMLExp = RegExp(unescapedHTMLExp.source);
    /** @type {?} */

    var htmlEscapes = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      '\'': '&#39;'
    };
    /**
     * @param {?} string
     * @return {?}
     */

    function escapeHTML(string) {
      return string && hasUnescapedHTMLExp.test(string) ? string.replace(unescapedHTMLExp,
      /**
      * @param {?} chr
      * @return {?}
      */
      function (chr) {
        return htmlEscapes[chr];
      }) : string;
    }
    /**
     * @param {?} value
     * @return {?}
     */


    function isDefined(value) {
      return value !== undefined && value !== null;
    }
    /**
     * @param {?} value
     * @return {?}
     */


    function isObject(value) {
      return typeof value === 'object' && isDefined(value);
    }
    /**
     * @param {?} value
     * @return {?}
     */


    function isPromise(value) {
      return value instanceof Promise;
    }
    /**
     * @param {?} value
     * @return {?}
     */


    function isFunction(value) {
      return value instanceof Function;
    }
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-templates.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var NgItemLabelDirective = /*#__PURE__*/function () {
      /**
       * @param {?} element
       */
      function NgItemLabelDirective(element) {
        _classCallCheck(this, NgItemLabelDirective);

        this.element = element;
        this.escape = true;
      }
      /**
       * @param {?} changes
       * @return {?}
       */


      _createClass(NgItemLabelDirective, [{
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          this.element.nativeElement.innerHTML = this.escape ? escapeHTML(this.ngItemLabel) : this.ngItemLabel;
        }
      }]);

      return NgItemLabelDirective;
    }();

    NgItemLabelDirective.ɵfac = function NgItemLabelDirective_Factory(t) {
      return new (t || NgItemLabelDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]));
    };

    NgItemLabelDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgItemLabelDirective,
      selectors: [["", "ngItemLabel", ""]],
      inputs: {
        escape: "escape",
        ngItemLabel: "ngItemLabel"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]]
    });
    /** @nocollapse */

    NgItemLabelDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
      }];
    };

    NgItemLabelDirective.propDecorators = {
      ngItemLabel: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      escape: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgItemLabelDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ngItemLabel]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
        }];
      }, {
        escape: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        ngItemLabel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();

    if (false) {}

    var NgOptionTemplateDirective =
    /**
     * @param {?} template
     */
    function NgOptionTemplateDirective(template) {
      _classCallCheck(this, NgOptionTemplateDirective);

      this.template = template;
    };

    NgOptionTemplateDirective.ɵfac = function NgOptionTemplateDirective_Factory(t) {
      return new (t || NgOptionTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgOptionTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgOptionTemplateDirective,
      selectors: [["", "ng-option-tmp", ""]]
    });
    /** @nocollapse */

    NgOptionTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgOptionTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-option-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgOptgroupTemplateDirective =
    /**
     * @param {?} template
     */
    function NgOptgroupTemplateDirective(template) {
      _classCallCheck(this, NgOptgroupTemplateDirective);

      this.template = template;
    };

    NgOptgroupTemplateDirective.ɵfac = function NgOptgroupTemplateDirective_Factory(t) {
      return new (t || NgOptgroupTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgOptgroupTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgOptgroupTemplateDirective,
      selectors: [["", "ng-optgroup-tmp", ""]]
    });
    /** @nocollapse */

    NgOptgroupTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgOptgroupTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-optgroup-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgLabelTemplateDirective =
    /**
     * @param {?} template
     */
    function NgLabelTemplateDirective(template) {
      _classCallCheck(this, NgLabelTemplateDirective);

      this.template = template;
    };

    NgLabelTemplateDirective.ɵfac = function NgLabelTemplateDirective_Factory(t) {
      return new (t || NgLabelTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgLabelTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgLabelTemplateDirective,
      selectors: [["", "ng-label-tmp", ""]]
    });
    /** @nocollapse */

    NgLabelTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgLabelTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-label-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgMultiLabelTemplateDirective =
    /**
     * @param {?} template
     */
    function NgMultiLabelTemplateDirective(template) {
      _classCallCheck(this, NgMultiLabelTemplateDirective);

      this.template = template;
    };

    NgMultiLabelTemplateDirective.ɵfac = function NgMultiLabelTemplateDirective_Factory(t) {
      return new (t || NgMultiLabelTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgMultiLabelTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgMultiLabelTemplateDirective,
      selectors: [["", "ng-multi-label-tmp", ""]]
    });
    /** @nocollapse */

    NgMultiLabelTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgMultiLabelTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-multi-label-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgHeaderTemplateDirective =
    /**
     * @param {?} template
     */
    function NgHeaderTemplateDirective(template) {
      _classCallCheck(this, NgHeaderTemplateDirective);

      this.template = template;
    };

    NgHeaderTemplateDirective.ɵfac = function NgHeaderTemplateDirective_Factory(t) {
      return new (t || NgHeaderTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgHeaderTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgHeaderTemplateDirective,
      selectors: [["", "ng-header-tmp", ""]]
    });
    /** @nocollapse */

    NgHeaderTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgHeaderTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-header-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgFooterTemplateDirective =
    /**
     * @param {?} template
     */
    function NgFooterTemplateDirective(template) {
      _classCallCheck(this, NgFooterTemplateDirective);

      this.template = template;
    };

    NgFooterTemplateDirective.ɵfac = function NgFooterTemplateDirective_Factory(t) {
      return new (t || NgFooterTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgFooterTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgFooterTemplateDirective,
      selectors: [["", "ng-footer-tmp", ""]]
    });
    /** @nocollapse */

    NgFooterTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgFooterTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-footer-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgNotFoundTemplateDirective =
    /**
     * @param {?} template
     */
    function NgNotFoundTemplateDirective(template) {
      _classCallCheck(this, NgNotFoundTemplateDirective);

      this.template = template;
    };

    NgNotFoundTemplateDirective.ɵfac = function NgNotFoundTemplateDirective_Factory(t) {
      return new (t || NgNotFoundTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgNotFoundTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgNotFoundTemplateDirective,
      selectors: [["", "ng-notfound-tmp", ""]]
    });
    /** @nocollapse */

    NgNotFoundTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgNotFoundTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-notfound-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgTypeToSearchTemplateDirective =
    /**
     * @param {?} template
     */
    function NgTypeToSearchTemplateDirective(template) {
      _classCallCheck(this, NgTypeToSearchTemplateDirective);

      this.template = template;
    };

    NgTypeToSearchTemplateDirective.ɵfac = function NgTypeToSearchTemplateDirective_Factory(t) {
      return new (t || NgTypeToSearchTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgTypeToSearchTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgTypeToSearchTemplateDirective,
      selectors: [["", "ng-typetosearch-tmp", ""]]
    });
    /** @nocollapse */

    NgTypeToSearchTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgTypeToSearchTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-typetosearch-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgLoadingTextTemplateDirective =
    /**
     * @param {?} template
     */
    function NgLoadingTextTemplateDirective(template) {
      _classCallCheck(this, NgLoadingTextTemplateDirective);

      this.template = template;
    };

    NgLoadingTextTemplateDirective.ɵfac = function NgLoadingTextTemplateDirective_Factory(t) {
      return new (t || NgLoadingTextTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgLoadingTextTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgLoadingTextTemplateDirective,
      selectors: [["", "ng-loadingtext-tmp", ""]]
    });
    /** @nocollapse */

    NgLoadingTextTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgLoadingTextTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-loadingtext-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgTagTemplateDirective =
    /**
     * @param {?} template
     */
    function NgTagTemplateDirective(template) {
      _classCallCheck(this, NgTagTemplateDirective);

      this.template = template;
    };

    NgTagTemplateDirective.ɵfac = function NgTagTemplateDirective_Factory(t) {
      return new (t || NgTagTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgTagTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgTagTemplateDirective,
      selectors: [["", "ng-tag-tmp", ""]]
    });
    /** @nocollapse */

    NgTagTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgTagTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-tag-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}

    var NgLoadingSpinnerTemplateDirective =
    /**
     * @param {?} template
     */
    function NgLoadingSpinnerTemplateDirective(template) {
      _classCallCheck(this, NgLoadingSpinnerTemplateDirective);

      this.template = template;
    };

    NgLoadingSpinnerTemplateDirective.ɵfac = function NgLoadingSpinnerTemplateDirective_Factory(t) {
      return new (t || NgLoadingSpinnerTemplateDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]));
    };

    NgLoadingSpinnerTemplateDirective.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: NgLoadingSpinnerTemplateDirective,
      selectors: [["", "ng-loadingspinner-tmp", ""]]
    });
    /** @nocollapse */

    NgLoadingSpinnerTemplateDirective.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
      }];
    };
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgLoadingSpinnerTemplateDirective, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: '[ng-loadingspinner-tmp]'
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }];
      }, null);
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/console.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var ConsoleService = /*#__PURE__*/function () {
      function ConsoleService() {
        _classCallCheck(this, ConsoleService);
      }

      _createClass(ConsoleService, [{
        key: "warn",
        value:
        /**
         * @param {?} message
         * @return {?}
         */
        function warn(message) {
          console.warn(message);
        }
      }]);

      return ConsoleService;
    }();

    ConsoleService.ɵfac = function ConsoleService_Factory(t) {
      return new (t || ConsoleService)();
    };
    /** @nocollapse */


    ConsoleService.ɵprov = Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"])({
      factory: function ConsoleService_Factory() {
        return new ConsoleService();
      },
      token: ConsoleService,
      providedIn: "root"
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ConsoleService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], null, null);
    })();
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/id.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @return {?}
     */


    function newId() {
      // First character is an 'a', it's good practice to tag id to begin with a letter
      return 'axxxxxxxxxxx'.replace(/[x]/g,
      /**
      * @param {?} _
      * @return {?}
      */
      function (_) {
        // tslint:disable-next-line:no-bitwise

        /** @type {?} */
        var val = Math.random() * 16 | 0;
        return val.toString(16);
      });
    }
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/search-helper.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /** @type {?} */


    var diacritics = {
      "\u24B6": 'A',
      "\uFF21": 'A',
      "\xC0": 'A',
      "\xC1": 'A',
      "\xC2": 'A',
      "\u1EA6": 'A',
      "\u1EA4": 'A',
      "\u1EAA": 'A',
      "\u1EA8": 'A',
      "\xC3": 'A',
      "\u0100": 'A',
      "\u0102": 'A',
      "\u1EB0": 'A',
      "\u1EAE": 'A',
      "\u1EB4": 'A',
      "\u1EB2": 'A',
      "\u0226": 'A',
      "\u01E0": 'A',
      "\xC4": 'A',
      "\u01DE": 'A',
      "\u1EA2": 'A',
      "\xC5": 'A',
      "\u01FA": 'A',
      "\u01CD": 'A',
      "\u0200": 'A',
      "\u0202": 'A',
      "\u1EA0": 'A',
      "\u1EAC": 'A',
      "\u1EB6": 'A',
      "\u1E00": 'A',
      "\u0104": 'A',
      "\u023A": 'A',
      "\u2C6F": 'A',
      "\uA732": 'AA',
      "\xC6": 'AE',
      "\u01FC": 'AE',
      "\u01E2": 'AE',
      "\uA734": 'AO',
      "\uA736": 'AU',
      "\uA738": 'AV',
      "\uA73A": 'AV',
      "\uA73C": 'AY',
      "\u24B7": 'B',
      "\uFF22": 'B',
      "\u1E02": 'B',
      "\u1E04": 'B',
      "\u1E06": 'B',
      "\u0243": 'B',
      "\u0182": 'B',
      "\u0181": 'B',
      "\u24B8": 'C',
      "\uFF23": 'C',
      "\u0106": 'C',
      "\u0108": 'C',
      "\u010A": 'C',
      "\u010C": 'C',
      "\xC7": 'C',
      "\u1E08": 'C',
      "\u0187": 'C',
      "\u023B": 'C',
      "\uA73E": 'C',
      "\u24B9": 'D',
      "\uFF24": 'D',
      "\u1E0A": 'D',
      "\u010E": 'D',
      "\u1E0C": 'D',
      "\u1E10": 'D',
      "\u1E12": 'D',
      "\u1E0E": 'D',
      "\u0110": 'D',
      "\u018B": 'D',
      "\u018A": 'D',
      "\u0189": 'D',
      "\uA779": 'D',
      "\u01F1": 'DZ',
      "\u01C4": 'DZ',
      "\u01F2": 'Dz',
      "\u01C5": 'Dz',
      "\u24BA": 'E',
      "\uFF25": 'E',
      "\xC8": 'E',
      "\xC9": 'E',
      "\xCA": 'E',
      "\u1EC0": 'E',
      "\u1EBE": 'E',
      "\u1EC4": 'E',
      "\u1EC2": 'E',
      "\u1EBC": 'E',
      "\u0112": 'E',
      "\u1E14": 'E',
      "\u1E16": 'E',
      "\u0114": 'E',
      "\u0116": 'E',
      "\xCB": 'E',
      "\u1EBA": 'E',
      "\u011A": 'E',
      "\u0204": 'E',
      "\u0206": 'E',
      "\u1EB8": 'E',
      "\u1EC6": 'E',
      "\u0228": 'E',
      "\u1E1C": 'E',
      "\u0118": 'E',
      "\u1E18": 'E',
      "\u1E1A": 'E',
      "\u0190": 'E',
      "\u018E": 'E',
      "\u24BB": 'F',
      "\uFF26": 'F',
      "\u1E1E": 'F',
      "\u0191": 'F',
      "\uA77B": 'F',
      "\u24BC": 'G',
      "\uFF27": 'G',
      "\u01F4": 'G',
      "\u011C": 'G',
      "\u1E20": 'G',
      "\u011E": 'G',
      "\u0120": 'G',
      "\u01E6": 'G',
      "\u0122": 'G',
      "\u01E4": 'G',
      "\u0193": 'G',
      "\uA7A0": 'G',
      "\uA77D": 'G',
      "\uA77E": 'G',
      "\u24BD": 'H',
      "\uFF28": 'H',
      "\u0124": 'H',
      "\u1E22": 'H',
      "\u1E26": 'H',
      "\u021E": 'H',
      "\u1E24": 'H',
      "\u1E28": 'H',
      "\u1E2A": 'H',
      "\u0126": 'H',
      "\u2C67": 'H',
      "\u2C75": 'H',
      "\uA78D": 'H',
      "\u24BE": 'I',
      "\uFF29": 'I',
      "\xCC": 'I',
      "\xCD": 'I',
      "\xCE": 'I',
      "\u0128": 'I',
      "\u012A": 'I',
      "\u012C": 'I',
      "\u0130": 'I',
      "\xCF": 'I',
      "\u1E2E": 'I',
      "\u1EC8": 'I',
      "\u01CF": 'I',
      "\u0208": 'I',
      "\u020A": 'I',
      "\u1ECA": 'I',
      "\u012E": 'I',
      "\u1E2C": 'I',
      "\u0197": 'I',
      "\u24BF": 'J',
      "\uFF2A": 'J',
      "\u0134": 'J',
      "\u0248": 'J',
      "\u24C0": 'K',
      "\uFF2B": 'K',
      "\u1E30": 'K',
      "\u01E8": 'K',
      "\u1E32": 'K',
      "\u0136": 'K',
      "\u1E34": 'K',
      "\u0198": 'K',
      "\u2C69": 'K',
      "\uA740": 'K',
      "\uA742": 'K',
      "\uA744": 'K',
      "\uA7A2": 'K',
      "\u24C1": 'L',
      "\uFF2C": 'L',
      "\u013F": 'L',
      "\u0139": 'L',
      "\u013D": 'L',
      "\u1E36": 'L',
      "\u1E38": 'L',
      "\u013B": 'L',
      "\u1E3C": 'L',
      "\u1E3A": 'L',
      "\u0141": 'L',
      "\u023D": 'L',
      "\u2C62": 'L',
      "\u2C60": 'L',
      "\uA748": 'L',
      "\uA746": 'L',
      "\uA780": 'L',
      "\u01C7": 'LJ',
      "\u01C8": 'Lj',
      "\u24C2": 'M',
      "\uFF2D": 'M',
      "\u1E3E": 'M',
      "\u1E40": 'M',
      "\u1E42": 'M',
      "\u2C6E": 'M',
      "\u019C": 'M',
      "\u24C3": 'N',
      "\uFF2E": 'N',
      "\u01F8": 'N',
      "\u0143": 'N',
      "\xD1": 'N',
      "\u1E44": 'N',
      "\u0147": 'N',
      "\u1E46": 'N',
      "\u0145": 'N',
      "\u1E4A": 'N',
      "\u1E48": 'N',
      "\u0220": 'N',
      "\u019D": 'N',
      "\uA790": 'N',
      "\uA7A4": 'N',
      "\u01CA": 'NJ',
      "\u01CB": 'Nj',
      "\u24C4": 'O',
      "\uFF2F": 'O',
      "\xD2": 'O',
      "\xD3": 'O',
      "\xD4": 'O',
      "\u1ED2": 'O',
      "\u1ED0": 'O',
      "\u1ED6": 'O',
      "\u1ED4": 'O',
      "\xD5": 'O',
      "\u1E4C": 'O',
      "\u022C": 'O',
      "\u1E4E": 'O',
      "\u014C": 'O',
      "\u1E50": 'O',
      "\u1E52": 'O',
      "\u014E": 'O',
      "\u022E": 'O',
      "\u0230": 'O',
      "\xD6": 'O',
      "\u022A": 'O',
      "\u1ECE": 'O',
      "\u0150": 'O',
      "\u01D1": 'O',
      "\u020C": 'O',
      "\u020E": 'O',
      "\u01A0": 'O',
      "\u1EDC": 'O',
      "\u1EDA": 'O',
      "\u1EE0": 'O',
      "\u1EDE": 'O',
      "\u1EE2": 'O',
      "\u1ECC": 'O',
      "\u1ED8": 'O',
      "\u01EA": 'O',
      "\u01EC": 'O',
      "\xD8": 'O',
      "\u01FE": 'O',
      "\u0186": 'O',
      "\u019F": 'O',
      "\uA74A": 'O',
      "\uA74C": 'O',
      "\u01A2": 'OI',
      "\uA74E": 'OO',
      "\u0222": 'OU',
      "\u24C5": 'P',
      "\uFF30": 'P',
      "\u1E54": 'P',
      "\u1E56": 'P',
      "\u01A4": 'P',
      "\u2C63": 'P',
      "\uA750": 'P',
      "\uA752": 'P',
      "\uA754": 'P',
      "\u24C6": 'Q',
      "\uFF31": 'Q',
      "\uA756": 'Q',
      "\uA758": 'Q',
      "\u024A": 'Q',
      "\u24C7": 'R',
      "\uFF32": 'R',
      "\u0154": 'R',
      "\u1E58": 'R',
      "\u0158": 'R',
      "\u0210": 'R',
      "\u0212": 'R',
      "\u1E5A": 'R',
      "\u1E5C": 'R',
      "\u0156": 'R',
      "\u1E5E": 'R',
      "\u024C": 'R',
      "\u2C64": 'R',
      "\uA75A": 'R',
      "\uA7A6": 'R',
      "\uA782": 'R',
      "\u24C8": 'S',
      "\uFF33": 'S',
      "\u1E9E": 'S',
      "\u015A": 'S',
      "\u1E64": 'S',
      "\u015C": 'S',
      "\u1E60": 'S',
      "\u0160": 'S',
      "\u1E66": 'S',
      "\u1E62": 'S',
      "\u1E68": 'S',
      "\u0218": 'S',
      "\u015E": 'S',
      "\u2C7E": 'S',
      "\uA7A8": 'S',
      "\uA784": 'S',
      "\u24C9": 'T',
      "\uFF34": 'T',
      "\u1E6A": 'T',
      "\u0164": 'T',
      "\u1E6C": 'T',
      "\u021A": 'T',
      "\u0162": 'T',
      "\u1E70": 'T',
      "\u1E6E": 'T',
      "\u0166": 'T',
      "\u01AC": 'T',
      "\u01AE": 'T',
      "\u023E": 'T',
      "\uA786": 'T',
      "\uA728": 'TZ',
      "\u24CA": 'U',
      "\uFF35": 'U',
      "\xD9": 'U',
      "\xDA": 'U',
      "\xDB": 'U',
      "\u0168": 'U',
      "\u1E78": 'U',
      "\u016A": 'U',
      "\u1E7A": 'U',
      "\u016C": 'U',
      "\xDC": 'U',
      "\u01DB": 'U',
      "\u01D7": 'U',
      "\u01D5": 'U',
      "\u01D9": 'U',
      "\u1EE6": 'U',
      "\u016E": 'U',
      "\u0170": 'U',
      "\u01D3": 'U',
      "\u0214": 'U',
      "\u0216": 'U',
      "\u01AF": 'U',
      "\u1EEA": 'U',
      "\u1EE8": 'U',
      "\u1EEE": 'U',
      "\u1EEC": 'U',
      "\u1EF0": 'U',
      "\u1EE4": 'U',
      "\u1E72": 'U',
      "\u0172": 'U',
      "\u1E76": 'U',
      "\u1E74": 'U',
      "\u0244": 'U',
      "\u24CB": 'V',
      "\uFF36": 'V',
      "\u1E7C": 'V',
      "\u1E7E": 'V',
      "\u01B2": 'V',
      "\uA75E": 'V',
      "\u0245": 'V',
      "\uA760": 'VY',
      "\u24CC": 'W',
      "\uFF37": 'W',
      "\u1E80": 'W',
      "\u1E82": 'W',
      "\u0174": 'W',
      "\u1E86": 'W',
      "\u1E84": 'W',
      "\u1E88": 'W',
      "\u2C72": 'W',
      "\u24CD": 'X',
      "\uFF38": 'X',
      "\u1E8A": 'X',
      "\u1E8C": 'X',
      "\u24CE": 'Y',
      "\uFF39": 'Y',
      "\u1EF2": 'Y',
      "\xDD": 'Y',
      "\u0176": 'Y',
      "\u1EF8": 'Y',
      "\u0232": 'Y',
      "\u1E8E": 'Y',
      "\u0178": 'Y',
      "\u1EF6": 'Y',
      "\u1EF4": 'Y',
      "\u01B3": 'Y',
      "\u024E": 'Y',
      "\u1EFE": 'Y',
      "\u24CF": 'Z',
      "\uFF3A": 'Z',
      "\u0179": 'Z',
      "\u1E90": 'Z',
      "\u017B": 'Z',
      "\u017D": 'Z',
      "\u1E92": 'Z',
      "\u1E94": 'Z',
      "\u01B5": 'Z',
      "\u0224": 'Z',
      "\u2C7F": 'Z',
      "\u2C6B": 'Z',
      "\uA762": 'Z',
      "\u24D0": 'a',
      "\uFF41": 'a',
      "\u1E9A": 'a',
      "\xE0": 'a',
      "\xE1": 'a',
      "\xE2": 'a',
      "\u1EA7": 'a',
      "\u1EA5": 'a',
      "\u1EAB": 'a',
      "\u1EA9": 'a',
      "\xE3": 'a',
      "\u0101": 'a',
      "\u0103": 'a',
      "\u1EB1": 'a',
      "\u1EAF": 'a',
      "\u1EB5": 'a',
      "\u1EB3": 'a',
      "\u0227": 'a',
      "\u01E1": 'a',
      "\xE4": 'a',
      "\u01DF": 'a',
      "\u1EA3": 'a',
      "\xE5": 'a',
      "\u01FB": 'a',
      "\u01CE": 'a',
      "\u0201": 'a',
      "\u0203": 'a',
      "\u1EA1": 'a',
      "\u1EAD": 'a',
      "\u1EB7": 'a',
      "\u1E01": 'a',
      "\u0105": 'a',
      "\u2C65": 'a',
      "\u0250": 'a',
      "\uA733": 'aa',
      "\xE6": 'ae',
      "\u01FD": 'ae',
      "\u01E3": 'ae',
      "\uA735": 'ao',
      "\uA737": 'au',
      "\uA739": 'av',
      "\uA73B": 'av',
      "\uA73D": 'ay',
      "\u24D1": 'b',
      "\uFF42": 'b',
      "\u1E03": 'b',
      "\u1E05": 'b',
      "\u1E07": 'b',
      "\u0180": 'b',
      "\u0183": 'b',
      "\u0253": 'b',
      "\u24D2": 'c',
      "\uFF43": 'c',
      "\u0107": 'c',
      "\u0109": 'c',
      "\u010B": 'c',
      "\u010D": 'c',
      "\xE7": 'c',
      "\u1E09": 'c',
      "\u0188": 'c',
      "\u023C": 'c',
      "\uA73F": 'c',
      "\u2184": 'c',
      "\u24D3": 'd',
      "\uFF44": 'd',
      "\u1E0B": 'd',
      "\u010F": 'd',
      "\u1E0D": 'd',
      "\u1E11": 'd',
      "\u1E13": 'd',
      "\u1E0F": 'd',
      "\u0111": 'd',
      "\u018C": 'd',
      "\u0256": 'd',
      "\u0257": 'd',
      "\uA77A": 'd',
      "\u01F3": 'dz',
      "\u01C6": 'dz',
      "\u24D4": 'e',
      "\uFF45": 'e',
      "\xE8": 'e',
      "\xE9": 'e',
      "\xEA": 'e',
      "\u1EC1": 'e',
      "\u1EBF": 'e',
      "\u1EC5": 'e',
      "\u1EC3": 'e',
      "\u1EBD": 'e',
      "\u0113": 'e',
      "\u1E15": 'e',
      "\u1E17": 'e',
      "\u0115": 'e',
      "\u0117": 'e',
      "\xEB": 'e',
      "\u1EBB": 'e',
      "\u011B": 'e',
      "\u0205": 'e',
      "\u0207": 'e',
      "\u1EB9": 'e',
      "\u1EC7": 'e',
      "\u0229": 'e',
      "\u1E1D": 'e',
      "\u0119": 'e',
      "\u1E19": 'e',
      "\u1E1B": 'e',
      "\u0247": 'e',
      "\u025B": 'e',
      "\u01DD": 'e',
      "\u24D5": 'f',
      "\uFF46": 'f',
      "\u1E1F": 'f',
      "\u0192": 'f',
      "\uA77C": 'f',
      "\u24D6": 'g',
      "\uFF47": 'g',
      "\u01F5": 'g',
      "\u011D": 'g',
      "\u1E21": 'g',
      "\u011F": 'g',
      "\u0121": 'g',
      "\u01E7": 'g',
      "\u0123": 'g',
      "\u01E5": 'g',
      "\u0260": 'g',
      "\uA7A1": 'g',
      "\u1D79": 'g',
      "\uA77F": 'g',
      "\u24D7": 'h',
      "\uFF48": 'h',
      "\u0125": 'h',
      "\u1E23": 'h',
      "\u1E27": 'h',
      "\u021F": 'h',
      "\u1E25": 'h',
      "\u1E29": 'h',
      "\u1E2B": 'h',
      "\u1E96": 'h',
      "\u0127": 'h',
      "\u2C68": 'h',
      "\u2C76": 'h',
      "\u0265": 'h',
      "\u0195": 'hv',
      "\u24D8": 'i',
      "\uFF49": 'i',
      "\xEC": 'i',
      "\xED": 'i',
      "\xEE": 'i',
      "\u0129": 'i',
      "\u012B": 'i',
      "\u012D": 'i',
      "\xEF": 'i',
      "\u1E2F": 'i',
      "\u1EC9": 'i',
      "\u01D0": 'i',
      "\u0209": 'i',
      "\u020B": 'i',
      "\u1ECB": 'i',
      "\u012F": 'i',
      "\u1E2D": 'i',
      "\u0268": 'i',
      "\u0131": 'i',
      "\u24D9": 'j',
      "\uFF4A": 'j',
      "\u0135": 'j',
      "\u01F0": 'j',
      "\u0249": 'j',
      "\u24DA": 'k',
      "\uFF4B": 'k',
      "\u1E31": 'k',
      "\u01E9": 'k',
      "\u1E33": 'k',
      "\u0137": 'k',
      "\u1E35": 'k',
      "\u0199": 'k',
      "\u2C6A": 'k',
      "\uA741": 'k',
      "\uA743": 'k',
      "\uA745": 'k',
      "\uA7A3": 'k',
      "\u24DB": 'l',
      "\uFF4C": 'l',
      "\u0140": 'l',
      "\u013A": 'l',
      "\u013E": 'l',
      "\u1E37": 'l',
      "\u1E39": 'l',
      "\u013C": 'l',
      "\u1E3D": 'l',
      "\u1E3B": 'l',
      "\u017F": 'l',
      "\u0142": 'l',
      "\u019A": 'l',
      "\u026B": 'l',
      "\u2C61": 'l',
      "\uA749": 'l',
      "\uA781": 'l',
      "\uA747": 'l',
      "\u01C9": 'lj',
      "\u24DC": 'm',
      "\uFF4D": 'm',
      "\u1E3F": 'm',
      "\u1E41": 'm',
      "\u1E43": 'm',
      "\u0271": 'm',
      "\u026F": 'm',
      "\u24DD": 'n',
      "\uFF4E": 'n',
      "\u01F9": 'n',
      "\u0144": 'n',
      "\xF1": 'n',
      "\u1E45": 'n',
      "\u0148": 'n',
      "\u1E47": 'n',
      "\u0146": 'n',
      "\u1E4B": 'n',
      "\u1E49": 'n',
      "\u019E": 'n',
      "\u0272": 'n',
      "\u0149": 'n',
      "\uA791": 'n',
      "\uA7A5": 'n',
      "\u01CC": 'nj',
      "\u24DE": 'o',
      "\uFF4F": 'o',
      "\xF2": 'o',
      "\xF3": 'o',
      "\xF4": 'o',
      "\u1ED3": 'o',
      "\u1ED1": 'o',
      "\u1ED7": 'o',
      "\u1ED5": 'o',
      "\xF5": 'o',
      "\u1E4D": 'o',
      "\u022D": 'o',
      "\u1E4F": 'o',
      "\u014D": 'o',
      "\u1E51": 'o',
      "\u1E53": 'o',
      "\u014F": 'o',
      "\u022F": 'o',
      "\u0231": 'o',
      "\xF6": 'o',
      "\u022B": 'o',
      "\u1ECF": 'o',
      "\u0151": 'o',
      "\u01D2": 'o',
      "\u020D": 'o',
      "\u020F": 'o',
      "\u01A1": 'o',
      "\u1EDD": 'o',
      "\u1EDB": 'o',
      "\u1EE1": 'o',
      "\u1EDF": 'o',
      "\u1EE3": 'o',
      "\u1ECD": 'o',
      "\u1ED9": 'o',
      "\u01EB": 'o',
      "\u01ED": 'o',
      "\xF8": 'o',
      "\u01FF": 'o',
      "\u0254": 'o',
      "\uA74B": 'o',
      "\uA74D": 'o',
      "\u0275": 'o',
      "\u01A3": 'oi',
      "\u0223": 'ou',
      "\uA74F": 'oo',
      "\u24DF": 'p',
      "\uFF50": 'p',
      "\u1E55": 'p',
      "\u1E57": 'p',
      "\u01A5": 'p',
      "\u1D7D": 'p',
      "\uA751": 'p',
      "\uA753": 'p',
      "\uA755": 'p',
      "\u24E0": 'q',
      "\uFF51": 'q',
      "\u024B": 'q',
      "\uA757": 'q',
      "\uA759": 'q',
      "\u24E1": 'r',
      "\uFF52": 'r',
      "\u0155": 'r',
      "\u1E59": 'r',
      "\u0159": 'r',
      "\u0211": 'r',
      "\u0213": 'r',
      "\u1E5B": 'r',
      "\u1E5D": 'r',
      "\u0157": 'r',
      "\u1E5F": 'r',
      "\u024D": 'r',
      "\u027D": 'r',
      "\uA75B": 'r',
      "\uA7A7": 'r',
      "\uA783": 'r',
      "\u24E2": 's',
      "\uFF53": 's',
      "\xDF": 's',
      "\u015B": 's',
      "\u1E65": 's',
      "\u015D": 's',
      "\u1E61": 's',
      "\u0161": 's',
      "\u1E67": 's',
      "\u1E63": 's',
      "\u1E69": 's',
      "\u0219": 's',
      "\u015F": 's',
      "\u023F": 's',
      "\uA7A9": 's',
      "\uA785": 's',
      "\u1E9B": 's',
      "\u24E3": 't',
      "\uFF54": 't',
      "\u1E6B": 't',
      "\u1E97": 't',
      "\u0165": 't',
      "\u1E6D": 't',
      "\u021B": 't',
      "\u0163": 't',
      "\u1E71": 't',
      "\u1E6F": 't',
      "\u0167": 't',
      "\u01AD": 't',
      "\u0288": 't',
      "\u2C66": 't',
      "\uA787": 't',
      "\uA729": 'tz',
      "\u24E4": 'u',
      "\uFF55": 'u',
      "\xF9": 'u',
      "\xFA": 'u',
      "\xFB": 'u',
      "\u0169": 'u',
      "\u1E79": 'u',
      "\u016B": 'u',
      "\u1E7B": 'u',
      "\u016D": 'u',
      "\xFC": 'u',
      "\u01DC": 'u',
      "\u01D8": 'u',
      "\u01D6": 'u',
      "\u01DA": 'u',
      "\u1EE7": 'u',
      "\u016F": 'u',
      "\u0171": 'u',
      "\u01D4": 'u',
      "\u0215": 'u',
      "\u0217": 'u',
      "\u01B0": 'u',
      "\u1EEB": 'u',
      "\u1EE9": 'u',
      "\u1EEF": 'u',
      "\u1EED": 'u',
      "\u1EF1": 'u',
      "\u1EE5": 'u',
      "\u1E73": 'u',
      "\u0173": 'u',
      "\u1E77": 'u',
      "\u1E75": 'u',
      "\u0289": 'u',
      "\u24E5": 'v',
      "\uFF56": 'v',
      "\u1E7D": 'v',
      "\u1E7F": 'v',
      "\u028B": 'v',
      "\uA75F": 'v',
      "\u028C": 'v',
      "\uA761": 'vy',
      "\u24E6": 'w',
      "\uFF57": 'w',
      "\u1E81": 'w',
      "\u1E83": 'w',
      "\u0175": 'w',
      "\u1E87": 'w',
      "\u1E85": 'w',
      "\u1E98": 'w',
      "\u1E89": 'w',
      "\u2C73": 'w',
      "\u24E7": 'x',
      "\uFF58": 'x',
      "\u1E8B": 'x',
      "\u1E8D": 'x',
      "\u24E8": 'y',
      "\uFF59": 'y',
      "\u1EF3": 'y',
      "\xFD": 'y',
      "\u0177": 'y',
      "\u1EF9": 'y',
      "\u0233": 'y',
      "\u1E8F": 'y',
      "\xFF": 'y',
      "\u1EF7": 'y',
      "\u1E99": 'y',
      "\u1EF5": 'y',
      "\u01B4": 'y',
      "\u024F": 'y',
      "\u1EFF": 'y',
      "\u24E9": 'z',
      "\uFF5A": 'z',
      "\u017A": 'z',
      "\u1E91": 'z',
      "\u017C": 'z',
      "\u017E": 'z',
      "\u1E93": 'z',
      "\u1E95": 'z',
      "\u01B6": 'z',
      "\u0225": 'z',
      "\u0240": 'z',
      "\u2C6C": 'z',
      "\uA763": 'z',
      "\u0386": "\u0391",
      "\u0388": "\u0395",
      "\u0389": "\u0397",
      "\u038A": "\u0399",
      "\u03AA": "\u0399",
      "\u038C": "\u039F",
      "\u038E": "\u03A5",
      "\u03AB": "\u03A5",
      "\u038F": "\u03A9",
      "\u03AC": "\u03B1",
      "\u03AD": "\u03B5",
      "\u03AE": "\u03B7",
      "\u03AF": "\u03B9",
      "\u03CA": "\u03B9",
      "\u0390": "\u03B9",
      "\u03CC": "\u03BF",
      "\u03CD": "\u03C5",
      "\u03CB": "\u03C5",
      "\u03B0": "\u03C5",
      "\u03C9": "\u03C9",
      "\u03C2": "\u03C3"
    };
    /**
     * @param {?} text
     * @return {?}
     */

    function stripSpecialChars(text) {
      /** @type {?} */
      var match =
      /**
      * @param {?} a
      * @return {?}
      */
      function match(a) {
        return diacritics[a] || a;
      };

      return text.replace(/[^\u0000-\u007E]/g, match);
    }
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/items-list.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var ItemsList = /*#__PURE__*/function () {
      /**
       * @param {?} _ngSelect
       * @param {?} _selectionModel
       */
      function ItemsList(_ngSelect, _selectionModel) {
        _classCallCheck(this, ItemsList);

        this._ngSelect = _ngSelect;
        this._selectionModel = _selectionModel;
        this._items = [];
        this._filteredItems = [];
        this._markedIndex = -1;
      }
      /**
       * @return {?}
       */


      _createClass(ItemsList, [{
        key: "items",
        get: function get() {
          return this._items;
        }
        /**
         * @return {?}
         */

      }, {
        key: "filteredItems",
        get: function get() {
          return this._filteredItems;
        }
        /**
         * @return {?}
         */

      }, {
        key: "markedIndex",
        get: function get() {
          return this._markedIndex;
        }
        /**
         * @return {?}
         */

      }, {
        key: "selectedItems",
        get: function get() {
          return this._selectionModel.value;
        }
        /**
         * @return {?}
         */

      }, {
        key: "markedItem",
        get: function get() {
          return this._filteredItems[this._markedIndex];
        }
        /**
         * @return {?}
         */

      }, {
        key: "noItemsToSelect",
        get: function get() {
          return this._ngSelect.hideSelected && this._items.length === this.selectedItems.length;
        }
        /**
         * @return {?}
         */

      }, {
        key: "maxItemsSelected",
        get: function get() {
          return this._ngSelect.multiple && this._ngSelect.maxSelectedItems <= this.selectedItems.length;
        }
        /**
         * @return {?}
         */

      }, {
        key: "lastSelectedItem",
        get: function get() {
          /** @type {?} */
          var i = this.selectedItems.length - 1;

          for (; i >= 0; i--) {
            /** @type {?} */
            var item = this.selectedItems[i];

            if (!item.disabled) {
              return item;
            }
          }

          return null;
        }
        /**
         * @param {?} items
         * @return {?}
         */

      }, {
        key: "setItems",
        value: function setItems(items) {
          var _this2 = this;

          this._items = items.map(
          /**
          * @param {?} item
          * @param {?} index
          * @return {?}
          */
          function (item, index) {
            return _this2.mapItem(item, index);
          });

          if (this._ngSelect.groupBy) {
            this._groups = this._groupBy(this._items, this._ngSelect.groupBy);
            this._items = this._flatten(this._groups);
          } else {
            this._groups = new Map();

            this._groups.set(undefined, this._items);
          }

          this._filteredItems = _toConsumableArray(this._items);
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "select",
        value: function select(item) {
          if (item.selected || this.maxItemsSelected) {
            return;
          }
          /** @type {?} */


          var multiple = this._ngSelect.multiple;

          if (!multiple) {
            this.clearSelected();
          }

          this._selectionModel.select(item, multiple, this._ngSelect.selectableGroupAsModel);

          if (this._ngSelect.hideSelected) {
            this._hideSelected(item);
          }
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "unselect",
        value: function unselect(item) {
          if (!item.selected) {
            return;
          }

          this._selectionModel.unselect(item, this._ngSelect.multiple);

          if (this._ngSelect.hideSelected && isDefined(item.index) && this._ngSelect.multiple) {
            this._showSelected(item);
          }
        }
        /**
         * @param {?} value
         * @return {?}
         */

      }, {
        key: "findItem",
        value: function findItem(value) {
          var _this3 = this;

          /** @type {?} */
          var findBy;

          if (this._ngSelect.compareWith) {
            findBy =
            /**
            * @param {?} item
            * @return {?}
            */
            function findBy(item) {
              return _this3._ngSelect.compareWith(item.value, value);
            };
          } else if (this._ngSelect.bindValue) {
            findBy =
            /**
            * @param {?} item
            * @return {?}
            */
            function findBy(item) {
              return !item.children && _this3.resolveNested(item.value, _this3._ngSelect.bindValue) === value;
            };
          } else {
            findBy =
            /**
            * @param {?} item
            * @return {?}
            */
            function findBy(item) {
              return item.value === value || !item.children && item.label && item.label === _this3.resolveNested(value, _this3._ngSelect.bindLabel);
            };
          }

          return this._items.find(
          /**
          * @param {?} item
          * @return {?}
          */
          function (item) {
            return findBy(item);
          });
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "addItem",
        value: function addItem(item) {
          /** @type {?} */
          var option = this.mapItem(item, this._items.length);

          this._items.push(option);

          this._filteredItems.push(option);

          return option;
        }
        /**
         * @param {?=} keepDisabled
         * @return {?}
         */

      }, {
        key: "clearSelected",
        value: function clearSelected() {
          var keepDisabled = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : false;

          this._selectionModel.clear(keepDisabled);

          this._items.forEach(
          /**
          * @param {?} item
          * @return {?}
          */
          function (item) {
            item.selected = keepDisabled && item.selected && item.disabled;
            item.marked = false;
          });

          if (this._ngSelect.hideSelected) {
            this.resetFilteredItems();
          }
        }
        /**
         * @param {?} term
         * @return {?}
         */

      }, {
        key: "findByLabel",
        value: function findByLabel(term) {
          term = stripSpecialChars(term).toLocaleLowerCase();
          return this.filteredItems.find(
          /**
          * @param {?} item
          * @return {?}
          */
          function (item) {
            /** @type {?} */
            var label = stripSpecialChars(item.label).toLocaleLowerCase();
            return label.substr(0, term.length) === term;
          });
        }
        /**
         * @param {?} term
         * @return {?}
         */

      }, {
        key: "filter",
        value: function filter(term) {
          var _this4 = this;

          if (!term) {
            this.resetFilteredItems();
            return;
          }

          this._filteredItems = [];
          term = this._ngSelect.searchFn ? term : stripSpecialChars(term).toLocaleLowerCase();
          /** @type {?} */

          var match = this._ngSelect.searchFn || this._defaultSearchFn;
          /** @type {?} */

          var hideSelected = this._ngSelect.hideSelected;

          for (var _i = 0, _Array$from = Array.from(this._groups.keys()); _i < _Array$from.length; _i++) {
            var key = _Array$from[_i];

            /** @type {?} */
            var matchedItems = [];

            var _iterator = _createForOfIteratorHelper(this._groups.get(key)),
                _step;

            try {
              for (_iterator.s(); !(_step = _iterator.n()).done;) {
                var item = _step.value;

                if (hideSelected && (item.parent && item.parent.selected || item.selected)) {
                  continue;
                }
                /** @type {?} */


                var searchItem = this._ngSelect.searchFn ? item.value : item;

                if (match(term, searchItem)) {
                  matchedItems.push(item);
                }
              }
            } catch (err) {
              _iterator.e(err);
            } finally {
              _iterator.f();
            }

            if (matchedItems.length > 0) {
              (function () {
                var _this4$_filteredItems;

                var _matchedItems$slice = matchedItems.slice(-1),
                    _matchedItems$slice2 = _slicedToArray(_matchedItems$slice, 1),
                    last = _matchedItems$slice2[0];

                if (last.parent) {
                  /** @type {?} */
                  var head = _this4._items.find(
                  /**
                  * @param {?} x
                  * @return {?}
                  */
                  function (x) {
                    return x === last.parent;
                  });

                  _this4._filteredItems.push(head);
                }

                (_this4$_filteredItems = _this4._filteredItems).push.apply(_this4$_filteredItems, matchedItems);
              })();
            }
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "resetFilteredItems",
        value: function resetFilteredItems() {
          if (this._filteredItems.length === this._items.length) {
            return;
          }

          if (this._ngSelect.hideSelected && this.selectedItems.length > 0) {
            this._filteredItems = this._items.filter(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return !x.selected;
            });
          } else {
            this._filteredItems = this._items;
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "unmarkItem",
        value: function unmarkItem() {
          this._markedIndex = -1;
        }
        /**
         * @return {?}
         */

      }, {
        key: "markNextItem",
        value: function markNextItem() {
          this._stepToItem(+1);
        }
        /**
         * @return {?}
         */

      }, {
        key: "markPreviousItem",
        value: function markPreviousItem() {
          this._stepToItem(-1);
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "markItem",
        value: function markItem(item) {
          this._markedIndex = this._filteredItems.indexOf(item);
        }
        /**
         * @param {?=} markDefault
         * @return {?}
         */

      }, {
        key: "markSelectedOrDefault",
        value: function markSelectedOrDefault(markDefault) {
          if (this._filteredItems.length === 0) {
            return;
          }
          /** @type {?} */


          var lastMarkedIndex = this._getLastMarkedIndex();

          if (lastMarkedIndex > -1) {
            this._markedIndex = lastMarkedIndex;
          } else {
            this._markedIndex = markDefault ? this.filteredItems.findIndex(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return !x.disabled;
            }) : -1;
          }
        }
        /**
         * @param {?} option
         * @param {?} key
         * @return {?}
         */

      }, {
        key: "resolveNested",
        value: function resolveNested(option, key) {
          if (!isObject(option)) {
            return option;
          }

          if (key.indexOf('.') === -1) {
            return option[key];
          } else {
            /** @type {?} */
            var keys = key.split('.');
            /** @type {?} */

            var value = option;

            for (var i = 0, len = keys.length; i < len; ++i) {
              if (value == null) {
                return null;
              }

              value = value[keys[i]];
            }

            return value;
          }
        }
        /**
         * @param {?} item
         * @param {?} index
         * @return {?}
         */

      }, {
        key: "mapItem",
        value: function mapItem(item, index) {
          /** @type {?} */
          var label = isDefined(item.$ngOptionLabel) ? item.$ngOptionLabel : this.resolveNested(item, this._ngSelect.bindLabel);
          /** @type {?} */

          var value = isDefined(item.$ngOptionValue) ? item.$ngOptionValue : item;
          return {
            index: index,
            label: isDefined(label) ? label.toString() : '',
            value: value,
            disabled: item.disabled,
            htmlId: "".concat(this._ngSelect.dropdownId, "-").concat(index)
          };
        }
        /**
         * @return {?}
         */

      }, {
        key: "mapSelectedItems",
        value: function mapSelectedItems() {
          var _this5 = this;

          /** @type {?} */
          var multiple = this._ngSelect.multiple;

          var _iterator2 = _createForOfIteratorHelper(this.selectedItems),
              _step2;

          try {
            for (_iterator2.s(); !(_step2 = _iterator2.n()).done;) {
              var selected = _step2.value;

              /** @type {?} */
              var value = this._ngSelect.bindValue ? this.resolveNested(selected.value, this._ngSelect.bindValue) : selected.value;
              /** @type {?} */

              var item = isDefined(value) ? this.findItem(value) : null;

              this._selectionModel.unselect(selected, multiple);

              this._selectionModel.select(item || selected, multiple, this._ngSelect.selectableGroupAsModel);
            }
          } catch (err) {
            _iterator2.e(err);
          } finally {
            _iterator2.f();
          }

          if (this._ngSelect.hideSelected) {
            this._filteredItems = this.filteredItems.filter(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return _this5.selectedItems.indexOf(x) === -1;
            });
          }
        }
        /**
         * @private
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "_showSelected",
        value: function _showSelected(item) {
          this._filteredItems.push(item);

          if (item.parent) {
            /** @type {?} */
            var parent = item.parent;
            /** @type {?} */

            var parentExists = this._filteredItems.find(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return x === parent;
            });

            if (!parentExists) {
              this._filteredItems.push(parent);
            }
          } else if (item.children) {
            var _iterator3 = _createForOfIteratorHelper(item.children),
                _step3;

            try {
              for (_iterator3.s(); !(_step3 = _iterator3.n()).done;) {
                var child = _step3.value;
                child.selected = false;

                this._filteredItems.push(child);
              }
            } catch (err) {
              _iterator3.e(err);
            } finally {
              _iterator3.f();
            }
          }

          this._filteredItems = _toConsumableArray(this._filteredItems.sort(
          /**
          * @param {?} a
          * @param {?} b
          * @return {?}
          */
          function (a, b) {
            return a.index - b.index;
          }));
        }
        /**
         * @private
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "_hideSelected",
        value: function _hideSelected(item) {
          this._filteredItems = this._filteredItems.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x !== item;
          });

          if (item.parent) {
            /** @type {?} */
            var children = item.parent.children;

            if (children.every(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return x.selected;
            })) {
              this._filteredItems = this._filteredItems.filter(
              /**
              * @param {?} x
              * @return {?}
              */
              function (x) {
                return x !== item.parent;
              });
            }
          } else if (item.children) {
            this._filteredItems = this.filteredItems.filter(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return x.parent !== item;
            });
          }
        }
        /**
         * @private
         * @param {?} search
         * @param {?} opt
         * @return {?}
         */

      }, {
        key: "_defaultSearchFn",
        value: function _defaultSearchFn(search, opt) {
          /** @type {?} */
          var label = stripSpecialChars(opt.label).toLocaleLowerCase();
          return label.indexOf(search) > -1;
        }
        /**
         * @private
         * @param {?} steps
         * @return {?}
         */

      }, {
        key: "_getNextItemIndex",
        value: function _getNextItemIndex(steps) {
          if (steps > 0) {
            return this._markedIndex === this._filteredItems.length - 1 ? 0 : this._markedIndex + 1;
          }

          return this._markedIndex <= 0 ? this._filteredItems.length - 1 : this._markedIndex - 1;
        }
        /**
         * @private
         * @param {?} steps
         * @return {?}
         */

      }, {
        key: "_stepToItem",
        value: function _stepToItem(steps) {
          if (this._filteredItems.length === 0 || this._filteredItems.every(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.disabled;
          })) {
            return;
          }

          this._markedIndex = this._getNextItemIndex(steps);

          if (this.markedItem.disabled) {
            this._stepToItem(steps);
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_getLastMarkedIndex",
        value: function _getLastMarkedIndex() {
          if (this._ngSelect.hideSelected) {
            return -1;
          }

          if (this._markedIndex > -1 && this.markedItem === undefined) {
            return -1;
          }
          /** @type {?} */


          var selectedIndex = this._filteredItems.indexOf(this.lastSelectedItem);

          if (this.lastSelectedItem && selectedIndex < 0) {
            return -1;
          }

          return Math.max(this.markedIndex, selectedIndex);
        }
        /**
         * @private
         * @param {?} items
         * @param {?} prop
         * @return {?}
         */

      }, {
        key: "_groupBy",
        value: function _groupBy(items, prop) {
          var _this6 = this;

          /** @type {?} */
          var groups = new Map();

          if (items.length === 0) {
            return groups;
          } // Check if items are already grouped by given key.


          if (Array.isArray(items[0].value[prop])) {
            var _iterator4 = _createForOfIteratorHelper(items),
                _step4;

            try {
              for (_iterator4.s(); !(_step4 = _iterator4.n()).done;) {
                var item = _step4.value;

                /** @type {?} */
                var children = (item.value[prop] || []).map(
                /**
                * @param {?} x
                * @param {?} index
                * @return {?}
                */
                function (x, index) {
                  return _this6.mapItem(x, index);
                });
                groups.set(item, children);
              }
            } catch (err) {
              _iterator4.e(err);
            } finally {
              _iterator4.f();
            }

            return groups;
          }
          /** @type {?} */


          var isFnKey = isFunction(this._ngSelect.groupBy);
          /** @type {?} */

          var keyFn =
          /**
          * @param {?} item
          * @return {?}
          */
          function keyFn(item) {
            /** @type {?} */
            var key = isFnKey ? prop(item.value) : item.value[prop];
            return isDefined(key) ? key : undefined;
          }; // Group items by key.


          var _iterator5 = _createForOfIteratorHelper(items),
              _step5;

          try {
            for (_iterator5.s(); !(_step5 = _iterator5.n()).done;) {
              var _item = _step5.value;

              /** @type {?} */
              var key = keyFn(_item);
              /** @type {?} */

              var group = groups.get(key);

              if (group) {
                group.push(_item);
              } else {
                groups.set(key, [_item]);
              }
            }
          } catch (err) {
            _iterator5.e(err);
          } finally {
            _iterator5.f();
          }

          return groups;
        }
        /**
         * @private
         * @param {?} groups
         * @return {?}
         */

      }, {
        key: "_flatten",
        value: function _flatten(groups) {
          var _this7 = this;

          /** @type {?} */
          var isGroupByFn = isFunction(this._ngSelect.groupBy);
          /** @type {?} */

          var items = [];

          var _loop = function _loop() {
            var key = _Array$from2[_i2];

            /** @type {?} */
            var i = items.length;

            if (key === undefined) {
              /** @type {?} */
              var withoutGroup = groups.get(undefined) || [];
              items.push.apply(items, _toConsumableArray(withoutGroup.map(
              /**
              * @param {?} x
              * @return {?}
              */
              function (x) {
                return Object.assign(Object.assign({}, x), {
                  index: i++
                });
              })));
              return "continue";
            }
            /** @type {?} */


            var isObjectKey = isObject(key);
            /** @type {?} */

            var parent = {
              label: isObjectKey ? '' : String(key),
              children: undefined,
              parent: null,
              index: i++,
              disabled: !_this7._ngSelect.selectableGroup,
              htmlId: newId()
            };
            /** @type {?} */

            var groupKey = isGroupByFn ? _this7._ngSelect.bindLabel : _this7._ngSelect.groupBy;
            /** @type {?} */

            var groupValue = _this7._ngSelect.groupValue ||
            /**
            * @return {?}
            */
            function () {
              if (isObjectKey) {
                return key.value;
              }

              return _defineProperty({}, groupKey, key);
            };
            /** @type {?} */


            var children = groups.get(key).map(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              x.parent = parent;
              x.children = undefined;
              x.index = i++;
              return x;
            });
            parent.children = children;
            parent.value = groupValue(key, children.map(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return x.value;
            }));
            items.push(parent);
            items.push.apply(items, _toConsumableArray(children));
          };

          for (var _i2 = 0, _Array$from2 = Array.from(groups.keys()); _i2 < _Array$from2.length; _i2++) {
            var _ret = _loop();

            if (_ret === "continue") continue;
          }

          return items;
        }
      }]);

      return ItemsList;
    }();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-select.types.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @record
     */


    function NgOption() {}

    if (false) {}
    /** @enum {number} */


    var KeyCode = {
      Tab: 9,
      Enter: 13,
      Esc: 27,
      Space: 32,
      ArrowUp: 38,
      ArrowDown: 40,
      Backspace: 8
    };
    KeyCode[KeyCode.Tab] = 'Tab';
    KeyCode[KeyCode.Enter] = 'Enter';
    KeyCode[KeyCode.Esc] = 'Esc';
    KeyCode[KeyCode.Space] = 'Space';
    KeyCode[KeyCode.ArrowUp] = 'ArrowUp';
    KeyCode[KeyCode.ArrowDown] = 'ArrowDown';
    KeyCode[KeyCode.Backspace] = 'Backspace';
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-dropdown-panel.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @record
     */

    function ItemsRangeResult() {}

    if (false) {}
    /**
     * @record
     */


    function PanelDimensions() {}

    if (false) {}

    var NgDropdownPanelService = /*#__PURE__*/function () {
      function NgDropdownPanelService() {
        _classCallCheck(this, NgDropdownPanelService);

        this._dimensions = {
          itemHeight: 0,
          panelHeight: 0,
          itemsPerViewport: 0
        };
      }
      /**
       * @return {?}
       */


      _createClass(NgDropdownPanelService, [{
        key: "dimensions",
        get: function get() {
          return this._dimensions;
        }
        /**
         * @param {?} scrollPos
         * @param {?} itemsLength
         * @param {?} buffer
         * @return {?}
         */

      }, {
        key: "calculateItems",
        value: function calculateItems(scrollPos, itemsLength, buffer) {
          /** @type {?} */
          var d = this._dimensions;
          /** @type {?} */

          var scrollHeight = d.itemHeight * itemsLength;
          /** @type {?} */

          var scrollTop = Math.max(0, scrollPos);
          /** @type {?} */

          var indexByScrollTop = scrollTop / scrollHeight * itemsLength;
          /** @type {?} */

          var end = Math.min(itemsLength, Math.ceil(indexByScrollTop) + (d.itemsPerViewport + 1));
          /** @type {?} */

          var maxStartEnd = end;
          /** @type {?} */

          var maxStart = Math.max(0, maxStartEnd - d.itemsPerViewport);
          /** @type {?} */

          var start = Math.min(maxStart, Math.floor(indexByScrollTop));
          /** @type {?} */

          var topPadding = d.itemHeight * Math.ceil(start) - d.itemHeight * Math.min(start, buffer);
          topPadding = !isNaN(topPadding) ? topPadding : 0;
          start = !isNaN(start) ? start : -1;
          end = !isNaN(end) ? end : -1;
          start -= buffer;
          start = Math.max(0, start);
          end += buffer;
          end = Math.min(itemsLength, end);
          return {
            topPadding: topPadding,
            scrollHeight: scrollHeight,
            start: start,
            end: end
          };
        }
        /**
         * @param {?} itemHeight
         * @param {?} panelHeight
         * @return {?}
         */

      }, {
        key: "setDimensions",
        value: function setDimensions(itemHeight, panelHeight) {
          /** @type {?} */
          var itemsPerViewport = Math.max(1, Math.floor(panelHeight / itemHeight));
          this._dimensions = {
            itemHeight: itemHeight,
            panelHeight: panelHeight,
            itemsPerViewport: itemsPerViewport
          };
        }
        /**
         * @param {?} itemTop
         * @param {?} itemHeight
         * @param {?} lastScroll
         * @return {?}
         */

      }, {
        key: "getScrollTo",
        value: function getScrollTo(itemTop, itemHeight, lastScroll) {
          var panelHeight = this.dimensions.panelHeight;
          /** @type {?} */

          var itemBottom = itemTop + itemHeight;
          /** @type {?} */

          var top = lastScroll;
          /** @type {?} */

          var bottom = top + panelHeight;

          if (panelHeight >= itemBottom && lastScroll === itemTop) {
            return null;
          }

          if (itemBottom > bottom) {
            return top + itemBottom - bottom;
          } else if (itemTop <= top) {
            return itemTop;
          }

          return null;
        }
      }]);

      return NgDropdownPanelService;
    }();

    NgDropdownPanelService.ɵfac = function NgDropdownPanelService_Factory(t) {
      return new (t || NgDropdownPanelService)();
    };

    NgDropdownPanelService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: NgDropdownPanelService,
      factory: NgDropdownPanelService.ɵfac
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgDropdownPanelService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"]
      }], function () {
        return [];
      }, null);
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-dropdown-panel.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /** @type {?} */


    var TOP_CSS_CLASS = 'ng-select-top';
    /** @type {?} */

    var BOTTOM_CSS_CLASS = 'ng-select-bottom';
    /** @type {?} */

    var SCROLL_SCHEDULER = typeof requestAnimationFrame !== 'undefined' ? rxjs__WEBPACK_IMPORTED_MODULE_3__["animationFrameScheduler"] : rxjs__WEBPACK_IMPORTED_MODULE_3__["asapScheduler"];

    var NgDropdownPanelComponent = /*#__PURE__*/function () {
      /**
       * @param {?} _renderer
       * @param {?} _zone
       * @param {?} _panelService
       * @param {?} _elementRef
       * @param {?} _document
       */
      function NgDropdownPanelComponent(_renderer, _zone, _panelService, _elementRef, _document) {
        _classCallCheck(this, NgDropdownPanelComponent);

        this._renderer = _renderer;
        this._zone = _zone;
        this._panelService = _panelService;
        this._document = _document;
        this.items = [];
        this.position = 'auto';
        this.virtualScroll = false;
        this.filterValue = null;
        this.update = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.scroll = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.scrollToEnd = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.outsideClick = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this._destroy$ = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this._scrollToEndFired = false;
        this._updateScrollHeight = false;
        this._lastScrollPosition = 0;
        this._dropdown = _elementRef.nativeElement;
      }
      /**
       * @return {?}
       */


      _createClass(NgDropdownPanelComponent, [{
        key: "currentPosition",
        get: function get() {
          return this._currentPosition;
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "itemsLength",
        get: function get() {
          return this._itemsLength;
        }
        /**
         * @private
         * @param {?} value
         * @return {?}
         */
        ,
        set: function set(value) {
          if (value !== this._itemsLength) {
            this._itemsLength = value;

            this._onItemsLengthChanged();
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_startOffset",
        get: function get() {
          if (this.markedItem) {
            var _this$_panelService$d = this._panelService.dimensions,
                itemHeight = _this$_panelService$d.itemHeight,
                panelHeight = _this$_panelService$d.panelHeight;
            /** @type {?} */

            var offset = this.markedItem.index * itemHeight;
            return panelHeight > offset ? 0 : offset;
          }

          return 0;
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "handleMousedown",
        value: function handleMousedown($event) {
          /** @type {?} */
          var target = $event.target;

          if (target.tagName === 'INPUT') {
            return;
          }

          $event.preventDefault();
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnInit",
        value: function ngOnInit() {
          this._select = this._dropdown.parentElement;
          this._virtualPadding = this.paddingElementRef.nativeElement;
          this._scrollablePanel = this.scrollElementRef.nativeElement;
          this._contentPanel = this.contentElementRef.nativeElement;

          this._handleScroll();

          this._handleOutsideClick();

          this._appendDropdown();
        }
        /**
         * @param {?} changes
         * @return {?}
         */

      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          if (changes.items) {
            /** @type {?} */
            var change = changes.items;

            this._onItemsChange(change.currentValue, change.firstChange);
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this._destroy$.next();

          this._destroy$.complete();

          this._destroy$.unsubscribe();

          if (this.appendTo) {
            this._renderer.removeChild(this._dropdown.parentNode, this._dropdown);
          }
        }
        /**
         * @param {?} option
         * @param {?=} startFromOption
         * @return {?}
         */

      }, {
        key: "scrollTo",
        value: function scrollTo(option) {
          var startFromOption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : false;

          if (!option) {
            return;
          }
          /** @type {?} */


          var index = this.items.indexOf(option);

          if (index < 0 || index >= this.itemsLength) {
            return;
          }
          /** @type {?} */


          var scrollTo;

          if (this.virtualScroll) {
            /** @type {?} */
            var itemHeight = this._panelService.dimensions.itemHeight;
            scrollTo = this._panelService.getScrollTo(index * itemHeight, itemHeight, this._lastScrollPosition);
          } else {
            /** @type {?} */
            var item = this._dropdown.querySelector("#".concat(option.htmlId));
            /** @type {?} */


            var lastScroll = startFromOption ? item.offsetTop : this._lastScrollPosition;
            scrollTo = this._panelService.getScrollTo(item.offsetTop, item.clientHeight, lastScroll);
          }

          if (isDefined(scrollTo)) {
            this._scrollablePanel.scrollTop = scrollTo;
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "scrollToTag",
        value: function scrollToTag() {
          /** @type {?} */
          var panel = this._scrollablePanel;
          panel.scrollTop = panel.scrollHeight - panel.clientHeight;
        }
        /**
         * @return {?}
         */

      }, {
        key: "adjustPosition",
        value: function adjustPosition() {
          /** @type {?} */
          var parent = this._parent.getBoundingClientRect();
          /** @type {?} */


          var select = this._select.getBoundingClientRect();

          this._setOffset(parent, select);
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_handleDropdownPosition",
        value: function _handleDropdownPosition() {
          this._currentPosition = this._calculateCurrentPosition(this._dropdown);

          if (this._currentPosition === 'top') {
            this._renderer.addClass(this._dropdown, TOP_CSS_CLASS);

            this._renderer.removeClass(this._dropdown, BOTTOM_CSS_CLASS);

            this._renderer.addClass(this._select, TOP_CSS_CLASS);

            this._renderer.removeClass(this._select, BOTTOM_CSS_CLASS);
          } else {
            this._renderer.addClass(this._dropdown, BOTTOM_CSS_CLASS);

            this._renderer.removeClass(this._dropdown, TOP_CSS_CLASS);

            this._renderer.addClass(this._select, BOTTOM_CSS_CLASS);

            this._renderer.removeClass(this._select, TOP_CSS_CLASS);
          }

          if (this.appendTo) {
            this._updatePosition();
          }

          this._dropdown.style.opacity = '1';
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_handleScroll",
        value: function _handleScroll() {
          var _this8 = this;

          this._zone.runOutsideAngular(
          /**
          * @return {?}
          */
          function () {
            Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["fromEvent"])(_this8.scrollElementRef.nativeElement, 'scroll').pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["takeUntil"])(_this8._destroy$), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["auditTime"])(0, SCROLL_SCHEDULER)).subscribe(
            /**
            * @param {?} e
            * @return {?}
            */
            function (e) {
              return _this8._onContentScrolled(e.target.scrollTop);
            });
          });
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_handleOutsideClick",
        value: function _handleOutsideClick() {
          var _this9 = this;

          if (!this._document) {
            return;
          }

          this._zone.runOutsideAngular(
          /**
          * @return {?}
          */
          function () {
            Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["fromEvent"])(_this9._document, 'touchstart', {
              capture: true
            }), Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["fromEvent"])(_this9._document, 'mousedown', {
              capture: true
            })).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["takeUntil"])(_this9._destroy$)).subscribe(
            /**
            * @param {?} $event
            * @return {?}
            */
            function ($event) {
              return _this9._checkToClose($event);
            });
          });
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_checkToClose",
        value: function _checkToClose($event) {
          var _this10 = this;

          if (this._select.contains($event.target) || this._dropdown.contains($event.target)) {
            return;
          }
          /** @type {?} */


          var path = $event.path || $event.composedPath && $event.composedPath();

          if ($event.target && $event.target.shadowRoot && path && path[0] && this._select.contains(path[0])) {
            return;
          }

          this._zone.run(
          /**
          * @return {?}
          */
          function () {
            return _this10.outsideClick.emit();
          });
        }
        /**
         * @private
         * @param {?} items
         * @param {?} firstChange
         * @return {?}
         */

      }, {
        key: "_onItemsChange",
        value: function _onItemsChange(items, firstChange) {
          this.items = items || [];
          this._scrollToEndFired = false;
          this.itemsLength = items.length;

          if (this.virtualScroll) {
            this._updateItemsRange(firstChange);
          } else {
            this._updateItems(firstChange);
          }
        }
        /**
         * @private
         * @param {?} firstChange
         * @return {?}
         */

      }, {
        key: "_updateItems",
        value: function _updateItems(firstChange) {
          var _this11 = this;

          this.update.emit(this.items);

          if (firstChange === false) {
            return;
          }

          this._zone.runOutsideAngular(
          /**
          * @return {?}
          */
          function () {
            Promise.resolve().then(
            /**
            * @return {?}
            */
            function () {
              /** @type {?} */
              var panelHeight = _this11._scrollablePanel.clientHeight;

              _this11._panelService.setDimensions(0, panelHeight);

              _this11._handleDropdownPosition();

              _this11.scrollTo(_this11.markedItem, firstChange);
            });
          });
        }
        /**
         * @private
         * @param {?} firstChange
         * @return {?}
         */

      }, {
        key: "_updateItemsRange",
        value: function _updateItemsRange(firstChange) {
          var _this12 = this;

          this._zone.runOutsideAngular(
          /**
          * @return {?}
          */
          function () {
            _this12._measureDimensions().then(
            /**
            * @return {?}
            */
            function () {
              if (firstChange) {
                _this12._renderItemsRange(_this12._startOffset);

                _this12._handleDropdownPosition();
              } else {
                _this12._renderItemsRange();
              }
            });
          });
        }
        /**
         * @private
         * @param {?} scrollTop
         * @return {?}
         */

      }, {
        key: "_onContentScrolled",
        value: function _onContentScrolled(scrollTop) {
          if (this.virtualScroll) {
            this._renderItemsRange(scrollTop);
          }

          this._lastScrollPosition = scrollTop;

          this._fireScrollToEnd(scrollTop);
        }
        /**
         * @private
         * @param {?} height
         * @return {?}
         */

      }, {
        key: "_updateVirtualHeight",
        value: function _updateVirtualHeight(height) {
          if (this._updateScrollHeight) {
            this._virtualPadding.style.height = "".concat(height, "px");
            this._updateScrollHeight = false;
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_onItemsLengthChanged",
        value: function _onItemsLengthChanged() {
          this._updateScrollHeight = true;
        }
        /**
         * @private
         * @param {?=} scrollTop
         * @return {?}
         */

      }, {
        key: "_renderItemsRange",
        value: function _renderItemsRange() {
          var _this13 = this;

          var scrollTop = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : null;

          if (scrollTop && this._lastScrollPosition === scrollTop) {
            return;
          }

          scrollTop = scrollTop || this._scrollablePanel.scrollTop;
          /** @type {?} */

          var range = this._panelService.calculateItems(scrollTop, this.itemsLength, this.bufferAmount);

          this._updateVirtualHeight(range.scrollHeight);

          this._contentPanel.style.transform = "translateY(".concat(range.topPadding, "px)");

          this._zone.run(
          /**
          * @return {?}
          */
          function () {
            _this13.update.emit(_this13.items.slice(range.start, range.end));

            _this13.scroll.emit({
              start: range.start,
              end: range.end
            });
          });

          if (isDefined(scrollTop) && this._lastScrollPosition === 0) {
            this._scrollablePanel.scrollTop = scrollTop;
            this._lastScrollPosition = scrollTop;
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_measureDimensions",
        value: function _measureDimensions() {
          var _this14 = this;

          if (this._panelService.dimensions.itemHeight > 0 || this.itemsLength === 0) {
            return Promise.resolve(this._panelService.dimensions);
          }

          var _this$items = _slicedToArray(this.items, 1),
              first = _this$items[0];

          this.update.emit([first]);
          return Promise.resolve().then(
          /**
          * @return {?}
          */
          function () {
            /** @type {?} */
            var option = _this14._dropdown.querySelector("#".concat(first.htmlId));
            /** @type {?} */


            var optionHeight = option.clientHeight;
            _this14._virtualPadding.style.height = "".concat(optionHeight * _this14.itemsLength, "px");
            /** @type {?} */

            var panelHeight = _this14._scrollablePanel.clientHeight;

            _this14._panelService.setDimensions(optionHeight, panelHeight);

            return _this14._panelService.dimensions;
          });
        }
        /**
         * @private
         * @param {?} scrollTop
         * @return {?}
         */

      }, {
        key: "_fireScrollToEnd",
        value: function _fireScrollToEnd(scrollTop) {
          var _this15 = this;

          if (this._scrollToEndFired || scrollTop === 0) {
            return;
          }
          /** @type {?} */


          var padding = this.virtualScroll ? this._virtualPadding : this._contentPanel;

          if (scrollTop + this._dropdown.clientHeight >= padding.clientHeight) {
            this._zone.run(
            /**
            * @return {?}
            */
            function () {
              return _this15.scrollToEnd.emit();
            });

            this._scrollToEndFired = true;
          }
        }
        /**
         * @private
         * @param {?} dropdownEl
         * @return {?}
         */

      }, {
        key: "_calculateCurrentPosition",
        value: function _calculateCurrentPosition(dropdownEl) {
          if (this.position !== 'auto') {
            return this.position;
          }
          /** @type {?} */


          var selectRect = this._select.getBoundingClientRect();
          /** @type {?} */


          var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
          /** @type {?} */

          var offsetTop = selectRect.top + window.pageYOffset;
          /** @type {?} */

          var height = selectRect.height;
          /** @type {?} */

          var dropdownHeight = dropdownEl.getBoundingClientRect().height;

          if (offsetTop + height + dropdownHeight > scrollTop + document.documentElement.clientHeight) {
            return 'top';
          } else {
            return 'bottom';
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_appendDropdown",
        value: function _appendDropdown() {
          if (!this.appendTo) {
            return;
          }

          this._parent = document.querySelector(this.appendTo);

          if (!this._parent) {
            throw new Error("appendTo selector ".concat(this.appendTo, " did not found any parent element"));
          }

          this._parent.appendChild(this._dropdown);
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_updatePosition",
        value: function _updatePosition() {
          /** @type {?} */
          var select = this._select.getBoundingClientRect();
          /** @type {?} */


          var parent = this._parent.getBoundingClientRect();
          /** @type {?} */


          var offsetLeft = select.left - parent.left;

          this._setOffset(parent, select);

          this._dropdown.style.left = offsetLeft + 'px';
          this._dropdown.style.width = select.width + 'px';
          this._dropdown.style.minWidth = select.width + 'px';
        }
        /**
         * @private
         * @param {?} parent
         * @param {?} select
         * @return {?}
         */

      }, {
        key: "_setOffset",
        value: function _setOffset(parent, select) {
          /** @type {?} */
          var delta = select.height;

          if (this._currentPosition === 'top') {
            /** @type {?} */
            var offsetBottom = parent.bottom - select.bottom;
            this._dropdown.style.bottom = offsetBottom + delta + 'px';
            this._dropdown.style.top = 'auto';
          } else if (this._currentPosition === 'bottom') {
            /** @type {?} */
            var offsetTop = select.top - parent.top;
            this._dropdown.style.top = offsetTop + delta + 'px';
            this._dropdown.style.bottom = 'auto';
          }
        }
      }]);

      return NgDropdownPanelComponent;
    }();

    NgDropdownPanelComponent.ɵfac = function NgDropdownPanelComponent_Factory(t) {
      return new (t || NgDropdownPanelComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["Renderer2"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["NgZone"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](NgDropdownPanelService), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_common__WEBPACK_IMPORTED_MODULE_4__["DOCUMENT"], 8));
    };

    NgDropdownPanelComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: NgDropdownPanelComponent,
      selectors: [["ng-dropdown-panel"]],
      viewQuery: function NgDropdownPanelComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstaticViewQuery"](_c0, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstaticViewQuery"](_c1, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstaticViewQuery"](_c2, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.contentElementRef = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.scrollElementRef = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.paddingElementRef = _t.first);
        }
      },
      hostBindings: function NgDropdownPanelComponent_HostBindings(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("mousedown", function NgDropdownPanelComponent_mousedown_HostBindingHandler($event) {
            return ctx.handleMousedown($event);
          });
        }
      },
      inputs: {
        items: "items",
        position: "position",
        virtualScroll: "virtualScroll",
        filterValue: "filterValue",
        markedItem: "markedItem",
        appendTo: "appendTo",
        bufferAmount: "bufferAmount",
        headerTemplate: "headerTemplate",
        footerTemplate: "footerTemplate"
      },
      outputs: {
        update: "update",
        scroll: "scroll",
        scrollToEnd: "scrollToEnd",
        outsideClick: "outsideClick"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      ngContentSelectors: _c4,
      decls: 9,
      vars: 6,
      consts: [["class", "ng-dropdown-header", 4, "ngIf"], [1, "ng-dropdown-panel-items", "scroll-host"], ["scroll", ""], ["padding", ""], ["content", ""], ["class", "ng-dropdown-footer", 4, "ngIf"], [1, "ng-dropdown-header"], [3, "ngTemplateOutlet", "ngTemplateOutletContext"], [1, "ng-dropdown-footer"]],
      template: function NgDropdownPanelComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵprojectionDef"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](0, NgDropdownPanelComponent_div_0_Template, 2, 4, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1, 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "div", null, 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", null, 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵprojection"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, NgDropdownPanelComponent_div_8_Template, 2, 4, "div", 5);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.headerTemplate);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("total-padding", ctx.virtualScroll);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("scrollable-content", ctx.virtualScroll && ctx.items.length);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.footerTemplate);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgTemplateOutlet"]],
      encapsulation: 2,
      changeDetection: 0
    });
    /** @nocollapse */

    NgDropdownPanelComponent.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Renderer2"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgZone"]
      }, {
        type: NgDropdownPanelService
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
      }, {
        type: undefined,
        decorators: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Optional"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Inject"],
          args: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["DOCUMENT"]]
        }]
      }];
    };

    NgDropdownPanelComponent.propDecorators = {
      items: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      markedItem: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      position: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      appendTo: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      bufferAmount: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      virtualScroll: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      headerTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      footerTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      filterValue: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      update: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      scroll: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      scrollToEnd: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      outsideClick: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      contentElementRef: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
        args: ['content', {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
          "static": true
        }]
      }],
      scrollElementRef: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
        args: ['scroll', {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
          "static": true
        }]
      }],
      paddingElementRef: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
        args: ['padding', {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
          "static": true
        }]
      }],
      handleMousedown: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostListener"],
        args: ['mousedown', ['$event']]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgDropdownPanelComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectionStrategy"].OnPush,
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None,
          selector: 'ng-dropdown-panel',
          template: "\n        <div *ngIf=\"headerTemplate\" class=\"ng-dropdown-header\">\n            <ng-container [ngTemplateOutlet]=\"headerTemplate\" [ngTemplateOutletContext]=\"{ searchTerm: filterValue }\"></ng-container>\n        </div>\n        <div #scroll class=\"ng-dropdown-panel-items scroll-host\">\n            <div #padding [class.total-padding]=\"virtualScroll\"></div>\n            <div #content [class.scrollable-content]=\"virtualScroll && items.length\">\n                <ng-content></ng-content>\n            </div>\n        </div>\n        <div *ngIf=\"footerTemplate\" class=\"ng-dropdown-footer\">\n            <ng-container [ngTemplateOutlet]=\"footerTemplate\" [ngTemplateOutletContext]=\"{ searchTerm: filterValue }\"></ng-container>\n        </div>\n    "
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Renderer2"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgZone"]
        }, {
          type: NgDropdownPanelService
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
        }, {
          type: undefined,
          decorators: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Optional"]
          }, {
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Inject"],
            args: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["DOCUMENT"]]
          }]
        }];
      }, {
        items: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        position: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        virtualScroll: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        filterValue: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        update: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        scroll: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        scrollToEnd: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        outsideClick: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        handleMousedown: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostListener"],
          args: ['mousedown', ['$event']]
        }],
        markedItem: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        appendTo: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        bufferAmount: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        headerTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        footerTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        contentElementRef: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['content', {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
            "static": true
          }]
        }],
        scrollElementRef: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['scroll', {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
            "static": true
          }]
        }],
        paddingElementRef: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['padding', {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"],
            "static": true
          }]
        }]
      });
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-option.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var NgOptionComponent = /*#__PURE__*/function () {
      /**
       * @param {?} elementRef
       */
      function NgOptionComponent(elementRef) {
        _classCallCheck(this, NgOptionComponent);

        this.elementRef = elementRef;
        this.stateChange$ = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this._disabled = false;
      }
      /**
       * @return {?}
       */


      _createClass(NgOptionComponent, [{
        key: "disabled",
        get: function get() {
          return this._disabled;
        }
        /**
         * @param {?} value
         * @return {?}
         */
        ,
        set: function set(value) {
          this._disabled = this._isDisabled(value);
        }
        /**
         * @return {?}
         */

      }, {
        key: "label",
        get: function get() {
          return (this.elementRef.nativeElement.textContent || '').trim();
        }
        /**
         * @param {?} changes
         * @return {?}
         */

      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          if (changes.disabled) {
            this.stateChange$.next({
              value: this.value,
              disabled: this._disabled
            });
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngAfterViewChecked",
        value: function ngAfterViewChecked() {
          if (this.label !== this._previousLabel) {
            this._previousLabel = this.label;
            this.stateChange$.next({
              value: this.value,
              disabled: this._disabled,
              label: this.elementRef.nativeElement.innerHTML
            });
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.stateChange$.complete();
        }
        /**
         * @private
         * @param {?} value
         * @return {?}
         */

      }, {
        key: "_isDisabled",
        value: function _isDisabled(value) {
          return value != null && "".concat(value) !== 'false';
        }
      }]);

      return NgOptionComponent;
    }();

    NgOptionComponent.ɵfac = function NgOptionComponent_Factory(t) {
      return new (t || NgOptionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]));
    };

    NgOptionComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: NgOptionComponent,
      selectors: [["ng-option"]],
      inputs: {
        disabled: "disabled",
        value: "value"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      ngContentSelectors: _c4,
      decls: 1,
      vars: 0,
      template: function NgOptionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵprojectionDef"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵprojection"](0);
        }
      },
      encapsulation: 2,
      changeDetection: 0
    });
    /** @nocollapse */

    NgOptionComponent.ctorParameters = function () {
      return [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
      }];
    };

    NgOptionComponent.propDecorators = {
      value: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      disabled: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgOptionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'ng-option',
          changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectionStrategy"].OnPush,
          template: "<ng-content></ng-content>"
        }]
      }], function () {
        return [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
        }];
      }, {
        disabled: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        value: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/config.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var NgSelectConfig = function NgSelectConfig() {
      _classCallCheck(this, NgSelectConfig);

      this.notFoundText = 'No items found';
      this.typeToSearchText = 'Type to search';
      this.addTagText = 'Add item';
      this.loadingText = 'Loading...';
      this.clearAllText = 'Clear all';
      this.disableVirtualScroll = true;
      this.openOnEnter = true;
      this.appearance = 'underline';
    };

    NgSelectConfig.ɵfac = function NgSelectConfig_Factory(t) {
      return new (t || NgSelectConfig)();
    };
    /** @nocollapse */


    NgSelectConfig.ɵprov = Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"])({
      factory: function NgSelectConfig_Factory() {
        return new NgSelectConfig();
      },
      token: NgSelectConfig,
      providedIn: "root"
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgSelectConfig, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [];
      }, null);
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-select.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /** @type {?} */


    var SELECTION_MODEL_FACTORY = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["InjectionToken"]('ng-select-selection-model');

    var NgSelectComponent = /*#__PURE__*/function () {
      /**
       * @param {?} classes
       * @param {?} autoFocus
       * @param {?} config
       * @param {?} newSelectionModel
       * @param {?} _elementRef
       * @param {?} _cd
       * @param {?} _console
       */
      function NgSelectComponent(classes, autoFocus, config, newSelectionModel, _elementRef, _cd, _console) {
        var _this16 = this;

        _classCallCheck(this, NgSelectComponent);

        this.classes = classes;
        this.autoFocus = autoFocus;
        this._cd = _cd;
        this._console = _console;
        this.markFirst = true;
        this.dropdownPosition = 'auto';
        this.loading = false;
        this.closeOnSelect = true;
        this.hideSelected = false;
        this.selectOnTab = false;
        this.bufferAmount = 4;
        this.selectableGroup = false;
        this.selectableGroupAsModel = true;
        this.searchFn = null;
        this.trackByFn = null;
        this.clearOnBackspace = true;
        this.labelForId = null;
        this.inputAttrs = {};
        this.readonly = false;
        this.searchWhileComposing = true;
        this.minTermLength = 0;
        this.editableSearchTerm = false;

        this.keyDownFn =
        /**
        * @param {?} _
        * @return {?}
        */
        function (_) {
          return true;
        };

        this.multiple = false;
        this.addTag = false;
        this.searchable = true;
        this.clearable = true;
        this.isOpen = false; // output events

        this.blurEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.focusEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.changeEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.openEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.closeEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.searchEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.clearEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.addEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.removeEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.scroll = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.scrollToEnd = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.viewPortItems = [];
        this.searchTerm = null;
        this.dropdownId = newId();
        this.escapeHTML = true;
        this.useDefaultClass = true;
        this._items = [];
        this._defaultLabel = 'label';
        this._pressedKeys = [];
        this._isComposing = false;
        this._destroy$ = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this._keyPress$ = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();

        this._onChange =
        /**
        * @param {?} _
        * @return {?}
        */
        function (_) {};

        this._onTouched =
        /**
        * @return {?}
        */
        function () {};

        this.clearItem =
        /**
        * @param {?} item
        * @return {?}
        */
        function (item) {
          /** @type {?} */
          var option = _this16.selectedItems.find(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.value === item;
          });

          _this16.unselect(option);
        };

        this.trackByOption =
        /**
        * @param {?} _
        * @param {?} item
        * @return {?}
        */
        function (_, item) {
          if (_this16.trackByFn) {
            return _this16.trackByFn(item.value);
          }

          return item;
        };

        this._mergeGlobalConfig(config);

        this.itemsList = new ItemsList(this, newSelectionModel());
        this.element = _elementRef.nativeElement;
      }
      /**
       * @return {?}
       */


      _createClass(NgSelectComponent, [{
        key: "items",
        get: function get() {
          return this._items;
        },
        set:
        /**
         * @param {?} value
         * @return {?}
         */
        function set(value) {
          this._itemsAreUsed = true;
          this._items = value;
        }
      }, {
        key: "compareWith",
        get:
        /**
         * @return {?}
         */
        function get() {
          return this._compareWith;
        }
        /**
         * @param {?} fn
         * @return {?}
         */
        ,
        set: function set(fn) {
          if (!isFunction(fn)) {
            throw Error('`compareWith` must be a function.');
          }

          this._compareWith = fn;
        }
        /**
         * @return {?}
         */

      }, {
        key: "clearSearchOnAdd",
        get: function get() {
          return isDefined(this._clearSearchOnAdd) ? this._clearSearchOnAdd : this.closeOnSelect;
        },
        set:
        /**
         * @param {?} value
         * @return {?}
         */
        function set(value) {
          this._clearSearchOnAdd = value;
        }
      }, {
        key: "disabled",
        get:
        /**
         * @return {?}
         */
        function get() {
          return this.readonly || this._disabled;
        }
      }, {
        key: "filtered",
        get:
        /**
         * @return {?}
         */
        function get() {
          return !!this.searchTerm && this.searchable || this._isComposing;
        }
      }, {
        key: "_editableSearchTerm",
        get:
        /**
         * @private
         * @return {?}
         */
        function get() {
          return this.editableSearchTerm && !this.multiple;
        }
        /**
         * @return {?}
         */

      }, {
        key: "selectedItems",
        get: function get() {
          return this.itemsList.selectedItems;
        }
        /**
         * @return {?}
         */

      }, {
        key: "selectedValues",
        get: function get() {
          return this.selectedItems.map(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.value;
          });
        }
        /**
         * @return {?}
         */

      }, {
        key: "hasValue",
        get: function get() {
          return this.selectedItems.length > 0;
        }
        /**
         * @return {?}
         */

      }, {
        key: "currentPanelPosition",
        get: function get() {
          if (this.dropdownPanel) {
            return this.dropdownPanel.currentPosition;
          }

          return undefined;
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnInit",
        value: function ngOnInit() {
          this._handleKeyPresses();

          this._setInputAttributes();
        }
        /**
         * @param {?} changes
         * @return {?}
         */

      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(changes) {
          if (changes.multiple) {
            this.itemsList.clearSelected();
          }

          if (changes.items) {
            this._setItems(changes.items.currentValue || []);
          }

          if (changes.isOpen) {
            this._manualOpen = isDefined(changes.isOpen.currentValue);
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (!this._itemsAreUsed) {
            this.escapeHTML = false;

            this._setItemsFromNgOptions();
          }

          if (isDefined(this.autoFocus)) {
            this.focus();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this._destroy$.next();

          this._destroy$.complete();
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "handleKeyDown",
        value: function handleKeyDown($event) {
          /** @type {?} */
          var keyCode = KeyCode[$event.which];

          if (keyCode) {
            if (this.keyDownFn($event) === false) {
              return;
            }

            this.handleKeyCode($event);
          } else if ($event.key && $event.key.length === 1) {
            this._keyPress$.next($event.key.toLocaleLowerCase());
          }
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "handleKeyCode",
        value: function handleKeyCode($event) {
          switch ($event.which) {
            case KeyCode.ArrowDown:
              this._handleArrowDown($event);

              break;

            case KeyCode.ArrowUp:
              this._handleArrowUp($event);

              break;

            case KeyCode.Space:
              this._handleSpace($event);

              break;

            case KeyCode.Enter:
              this._handleEnter($event);

              break;

            case KeyCode.Tab:
              this._handleTab($event);

              break;

            case KeyCode.Esc:
              this.close();
              $event.preventDefault();
              break;

            case KeyCode.Backspace:
              this._handleBackspace();

              break;
          }
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "handleMousedown",
        value: function handleMousedown($event) {
          /** @type {?} */
          var target = $event.target;

          if (target.tagName !== 'INPUT') {
            $event.preventDefault();
          }

          if (target.classList.contains('ng-clear-wrapper')) {
            this.handleClearClick();
            return;
          }

          if (target.classList.contains('ng-arrow-wrapper')) {
            this.handleArrowClick();
            return;
          }

          if (target.classList.contains('ng-value-icon')) {
            return;
          }

          if (!this.focused) {
            this.focus();
          }

          if (this.searchable) {
            this.open();
          } else {
            this.toggle();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "handleArrowClick",
        value: function handleArrowClick() {
          if (this.isOpen) {
            this.close();
          } else {
            this.open();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "handleClearClick",
        value: function handleClearClick() {
          if (this.hasValue) {
            this.itemsList.clearSelected(true);

            this._updateNgModel();
          }

          this._clearSearch();

          this.focus();
          this.clearEvent.emit();

          this._onSelectionChanged();
        }
        /**
         * @return {?}
         */

      }, {
        key: "clearModel",
        value: function clearModel() {
          if (!this.clearable) {
            return;
          }

          this.itemsList.clearSelected();

          this._updateNgModel();
        }
        /**
         * @param {?} value
         * @return {?}
         */

      }, {
        key: "writeValue",
        value: function writeValue(value) {
          this.itemsList.clearSelected();

          this._handleWriteValue(value);

          this._cd.markForCheck();
        }
        /**
         * @param {?} fn
         * @return {?}
         */

      }, {
        key: "registerOnChange",
        value: function registerOnChange(fn) {
          this._onChange = fn;
        }
        /**
         * @param {?} fn
         * @return {?}
         */

      }, {
        key: "registerOnTouched",
        value: function registerOnTouched(fn) {
          this._onTouched = fn;
        }
        /**
         * @param {?} state
         * @return {?}
         */

      }, {
        key: "setDisabledState",
        value: function setDisabledState(state) {
          this._disabled = state;

          this._cd.markForCheck();
        }
        /**
         * @return {?}
         */

      }, {
        key: "toggle",
        value: function toggle() {
          if (!this.isOpen) {
            this.open();
          } else {
            this.close();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "open",
        value: function open() {
          if (this.disabled || this.isOpen || this.itemsList.maxItemsSelected || this._manualOpen) {
            return;
          }

          if (!this._isTypeahead && !this.addTag && this.itemsList.noItemsToSelect) {
            return;
          }

          this.isOpen = true;
          this.itemsList.markSelectedOrDefault(this.markFirst);
          this.openEvent.emit();

          if (!this.searchTerm) {
            this.focus();
          }

          this.detectChanges();
        }
        /**
         * @return {?}
         */

      }, {
        key: "close",
        value: function close() {
          if (!this.isOpen || this._manualOpen) {
            return;
          }

          this.isOpen = false;

          if (!this._editableSearchTerm) {
            this._clearSearch();
          } else {
            this.itemsList.resetFilteredItems();
          }

          this.itemsList.unmarkItem();

          this._onTouched();

          this.closeEvent.emit();

          this._cd.markForCheck();
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "toggleItem",
        value: function toggleItem(item) {
          if (!item || item.disabled || this.disabled) {
            return;
          }

          if (this.multiple && item.selected) {
            this.unselect(item);
          } else {
            this.select(item);
          }

          if (this._editableSearchTerm) {
            this._setSearchTermFromItems();
          }

          this._onSelectionChanged();
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "select",
        value: function select(item) {
          if (!item.selected) {
            this.itemsList.select(item);

            if (this.clearSearchOnAdd && !this._editableSearchTerm) {
              this._clearSearch();
            }

            this._updateNgModel();

            if (this.multiple) {
              this.addEvent.emit(item.value);
            }
          }

          if (this.closeOnSelect || this.itemsList.noItemsToSelect) {
            this.close();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "focus",
        value: function focus() {
          this.searchInput.nativeElement.focus();
        }
        /**
         * @return {?}
         */

      }, {
        key: "blur",
        value: function blur() {
          this.searchInput.nativeElement.blur();
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "unselect",
        value: function unselect(item) {
          if (!item) {
            return;
          }

          this.itemsList.unselect(item);
          this.focus();

          this._updateNgModel();

          this.removeEvent.emit(item);
        }
        /**
         * @return {?}
         */

      }, {
        key: "selectTag",
        value: function selectTag() {
          var _this17 = this;

          /** @type {?} */
          var tag;

          if (isFunction(this.addTag)) {
            tag = this.addTag(this.searchTerm);
          } else {
            tag = this._primitive ? this.searchTerm : _defineProperty({}, this.bindLabel, this.searchTerm);
          }
          /** @type {?} */


          var handleTag =
          /**
          * @param {?} item
          * @return {?}
          */
          function handleTag(item) {
            return _this17._isTypeahead || !_this17.isOpen ? _this17.itemsList.mapItem(item, null) : _this17.itemsList.addItem(item);
          };

          if (isPromise(tag)) {
            tag.then(
            /**
            * @param {?} item
            * @return {?}
            */
            function (item) {
              return _this17.select(handleTag(item));
            })["catch"](
            /**
            * @return {?}
            */
            function () {});
          } else if (tag) {
            this.select(handleTag(tag));
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "showClear",
        value: function showClear() {
          return this.clearable && (this.hasValue || this.searchTerm) && !this.disabled;
        }
        /**
         * @return {?}
         */

      }, {
        key: "showAddTag",
        get: function get() {
          if (!this._validTerm) {
            return false;
          }
          /** @type {?} */


          var term = this.searchTerm.toLowerCase().trim();
          return this.addTag && !this.itemsList.filteredItems.some(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.label.toLowerCase() === term;
          }) && (!this.hideSelected && this.isOpen || !this.selectedItems.some(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.label.toLowerCase() === term;
          })) && !this.loading;
        }
        /**
         * @return {?}
         */

      }, {
        key: "showNoItemsFound",
        value: function showNoItemsFound() {
          /** @type {?} */
          var empty = this.itemsList.filteredItems.length === 0;
          return (empty && !this._isTypeahead && !this.loading || empty && this._isTypeahead && this._validTerm && !this.loading) && !this.showAddTag;
        }
        /**
         * @return {?}
         */

      }, {
        key: "showTypeToSearch",
        value: function showTypeToSearch() {
          /** @type {?} */
          var empty = this.itemsList.filteredItems.length === 0;
          return empty && this._isTypeahead && !this._validTerm && !this.loading;
        }
        /**
         * @return {?}
         */

      }, {
        key: "onCompositionStart",
        value: function onCompositionStart() {
          this._isComposing = true;
        }
        /**
         * @param {?} term
         * @return {?}
         */

      }, {
        key: "onCompositionEnd",
        value: function onCompositionEnd(term) {
          this._isComposing = false;

          if (this.searchWhileComposing) {
            return;
          }

          this.filter(term);
        }
        /**
         * @param {?} term
         * @return {?}
         */

      }, {
        key: "filter",
        value: function filter(term) {
          if (this._isComposing && !this.searchWhileComposing) {
            return;
          }

          this.searchTerm = term;

          if (this._isTypeahead && (this._validTerm || this.minTermLength === 0)) {
            this.typeahead.next(term);
          }

          if (!this._isTypeahead) {
            this.itemsList.filter(this.searchTerm);

            if (this.isOpen) {
              this.itemsList.markSelectedOrDefault(this.markFirst);
            }
          }

          this.searchEvent.emit({
            term: term,
            items: this.itemsList.filteredItems.map(
            /**
            * @param {?} x
            * @return {?}
            */
            function (x) {
              return x.value;
            })
          });
          this.open();
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "onInputFocus",
        value: function onInputFocus($event) {
          if (this.focused) {
            return;
          }

          if (this._editableSearchTerm) {
            this._setSearchTermFromItems();
          }

          this.element.classList.add('ng-select-focused');
          this.focusEvent.emit($event);
          this.focused = true;
        }
        /**
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "onInputBlur",
        value: function onInputBlur($event) {
          this.element.classList.remove('ng-select-focused');
          this.blurEvent.emit($event);

          if (!this.isOpen && !this.disabled) {
            this._onTouched();
          }

          if (this._editableSearchTerm) {
            this._setSearchTermFromItems();
          }

          this.focused = false;
        }
        /**
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "onItemHover",
        value: function onItemHover(item) {
          if (item.disabled) {
            return;
          }

          this.itemsList.markItem(item);
        }
        /**
         * @return {?}
         */

      }, {
        key: "detectChanges",
        value: function detectChanges() {
          if (!this._cd.destroyed) {
            this._cd.detectChanges();
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_setSearchTermFromItems",
        value: function _setSearchTermFromItems() {
          /** @type {?} */
          var selected = this.selectedItems && this.selectedItems[0];
          this.searchTerm = selected && selected.label || null;
        }
        /**
         * @private
         * @param {?} items
         * @return {?}
         */

      }, {
        key: "_setItems",
        value: function _setItems(items) {
          /** @type {?} */
          var firstItem = items[0];
          this.bindLabel = this.bindLabel || this._defaultLabel;
          this._primitive = isDefined(firstItem) ? !isObject(firstItem) : this._primitive || this.bindLabel === this._defaultLabel;
          this.itemsList.setItems(items);

          if (items.length > 0 && this.hasValue) {
            this.itemsList.mapSelectedItems();
          }

          if (this.isOpen && isDefined(this.searchTerm) && !this._isTypeahead) {
            this.itemsList.filter(this.searchTerm);
          }

          if (this._isTypeahead || this.isOpen) {
            this.itemsList.markSelectedOrDefault(this.markFirst);
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_setItemsFromNgOptions",
        value: function _setItemsFromNgOptions() {
          var _this18 = this;

          /** @type {?} */
          var mapNgOptions =
          /**
          * @param {?} options
          * @return {?}
          */
          function mapNgOptions(options) {
            _this18.items = options.map(
            /**
            * @param {?} option
            * @return {?}
            */
            function (option) {
              return {
                $ngOptionValue: option.value,
                $ngOptionLabel: option.elementRef.nativeElement.innerHTML,
                disabled: option.disabled
              };
            });

            _this18.itemsList.setItems(_this18.items);

            if (_this18.hasValue) {
              _this18.itemsList.mapSelectedItems();
            }

            _this18.detectChanges();
          };
          /** @type {?} */


          var handleOptionChange =
          /**
          * @return {?}
          */
          function handleOptionChange() {
            /** @type {?} */
            var changedOrDestroyed = Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"])(_this18.ngOptions.changes, _this18._destroy$);
            Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["merge"]).apply(void 0, _toConsumableArray(_this18.ngOptions.map(
            /**
            * @param {?} option
            * @return {?}
            */
            function (option) {
              return option.stateChange$;
            }))).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["takeUntil"])(changedOrDestroyed)).subscribe(
            /**
            * @param {?} option
            * @return {?}
            */
            function (option) {
              /** @type {?} */
              var item = _this18.itemsList.findItem(option.value);

              item.disabled = option.disabled;
              item.label = option.label || item.label;

              _this18._cd.detectChanges();
            });
          };

          this.ngOptions.changes.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["startWith"])(this.ngOptions), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["takeUntil"])(this._destroy$)).subscribe(
          /**
          * @param {?} options
          * @return {?}
          */
          function (options) {
            _this18.bindLabel = _this18._defaultLabel;
            mapNgOptions(options);
            handleOptionChange();
          });
        }
        /**
         * @private
         * @param {?} value
         * @return {?}
         */

      }, {
        key: "_isValidWriteValue",
        value: function _isValidWriteValue(value) {
          var _this19 = this;

          if (!isDefined(value) || this.multiple && value === '' || Array.isArray(value) && value.length === 0) {
            return false;
          }
          /** @type {?} */


          var validateBinding =
          /**
          * @param {?} item
          * @return {?}
          */
          function validateBinding(item) {
            if (!isDefined(_this19.compareWith) && isObject(item) && _this19.bindValue) {
              _this19._console.warn("Setting object(".concat(JSON.stringify(item), ") as your model with bindValue is not allowed unless [compareWith] is used."));

              return false;
            }

            return true;
          };

          if (this.multiple) {
            if (!Array.isArray(value)) {
              this._console.warn('Multiple select ngModel should be array.');

              return false;
            }

            return value.every(
            /**
            * @param {?} item
            * @return {?}
            */
            function (item) {
              return validateBinding(item);
            });
          } else {
            return validateBinding(value);
          }
        }
        /**
         * @private
         * @param {?} ngModel
         * @return {?}
         */

      }, {
        key: "_handleWriteValue",
        value: function _handleWriteValue(ngModel) {
          var _this20 = this;

          if (!this._isValidWriteValue(ngModel)) {
            return;
          }
          /** @type {?} */


          var select =
          /**
          * @param {?} val
          * @return {?}
          */
          function select(val) {
            /** @type {?} */
            var item = _this20.itemsList.findItem(val);

            if (item) {
              _this20.itemsList.select(item);
            } else {
              /** @type {?} */
              var isValObject = isObject(val);
              /** @type {?} */

              var isPrimitive = !isValObject && !_this20.bindValue;

              if (isValObject || isPrimitive) {
                _this20.itemsList.select(_this20.itemsList.mapItem(val, null));
              } else if (_this20.bindValue) {
                var _item2;

                item = (_item2 = {}, _defineProperty(_item2, _this20.bindLabel, null), _defineProperty(_item2, _this20.bindValue, val), _item2);

                _this20.itemsList.select(_this20.itemsList.mapItem(item, null));
              }
            }
          };

          if (this.multiple) {
            ngModel.forEach(
            /**
            * @param {?} item
            * @return {?}
            */
            function (item) {
              return select(item);
            });
          } else {
            select(ngModel);
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_handleKeyPresses",
        value: function _handleKeyPresses() {
          var _this21 = this;

          if (this.searchable) {
            return;
          }

          this._keyPress$.pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["takeUntil"])(this._destroy$), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["tap"])(
          /**
          * @param {?} letter
          * @return {?}
          */
          function (letter) {
            return _this21._pressedKeys.push(letter);
          }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["debounceTime"])(200), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["filter"])(
          /**
          * @return {?}
          */
          function () {
            return _this21._pressedKeys.length > 0;
          }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["map"])(
          /**
          * @return {?}
          */
          function () {
            return _this21._pressedKeys.join('');
          })).subscribe(
          /**
          * @param {?} term
          * @return {?}
          */
          function (term) {
            /** @type {?} */
            var item = _this21.itemsList.findByLabel(term);

            if (item) {
              if (_this21.isOpen) {
                _this21.itemsList.markItem(item);

                _this21._cd.markForCheck();
              } else {
                _this21.select(item);
              }
            }

            _this21._pressedKeys = [];
          });
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_setInputAttributes",
        value: function _setInputAttributes() {
          /** @type {?} */
          var input = this.searchInput.nativeElement;
          /** @type {?} */

          var attributes = Object.assign({
            type: 'text',
            autocorrect: 'off',
            autocapitalize: 'off',
            autocomplete: this.labelForId ? 'off' : this.dropdownId
          }, this.inputAttrs);

          for (var _i3 = 0, _Object$keys = Object.keys(attributes); _i3 < _Object$keys.length; _i3++) {
            var key = _Object$keys[_i3];
            input.setAttribute(key, attributes[key]);
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_updateNgModel",
        value: function _updateNgModel() {
          /** @type {?} */
          var model = [];

          var _iterator6 = _createForOfIteratorHelper(this.selectedItems),
              _step6;

          try {
            for (_iterator6.s(); !(_step6 = _iterator6.n()).done;) {
              var item = _step6.value;

              if (this.bindValue) {
                /** @type {?} */
                var value = null;

                if (item.children) {
                  /** @type {?} */
                  var groupKey = this.groupValue ? this.bindValue : this.groupBy;
                  value = item.value[groupKey || this.groupBy];
                } else {
                  value = this.itemsList.resolveNested(item.value, this.bindValue);
                }

                model.push(value);
              } else {
                model.push(item.value);
              }
            }
            /** @type {?} */

          } catch (err) {
            _iterator6.e(err);
          } finally {
            _iterator6.f();
          }

          var selected = this.selectedItems.map(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.value;
          });

          if (this.multiple) {
            this._onChange(model);

            this.changeEvent.emit(selected);
          } else {
            this._onChange(isDefined(model[0]) ? model[0] : null);

            this.changeEvent.emit(selected[0]);
          }

          this._cd.markForCheck();
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_clearSearch",
        value: function _clearSearch() {
          if (!this.searchTerm) {
            return;
          }

          this._changeSearch(null);

          this.itemsList.resetFilteredItems();
        }
        /**
         * @private
         * @param {?} searchTerm
         * @return {?}
         */

      }, {
        key: "_changeSearch",
        value: function _changeSearch(searchTerm) {
          this.searchTerm = searchTerm;

          if (this._isTypeahead) {
            this.typeahead.next(searchTerm);
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_scrollToMarked",
        value: function _scrollToMarked() {
          if (!this.isOpen || !this.dropdownPanel) {
            return;
          }

          this.dropdownPanel.scrollTo(this.itemsList.markedItem);
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_scrollToTag",
        value: function _scrollToTag() {
          if (!this.isOpen || !this.dropdownPanel) {
            return;
          }

          this.dropdownPanel.scrollToTag();
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_onSelectionChanged",
        value: function _onSelectionChanged() {
          if (this.isOpen && this.multiple && this.appendTo) {
            // Make sure items are rendered.
            this._cd.detectChanges();

            this.dropdownPanel.adjustPosition();
          }
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_handleTab",
        value: function _handleTab($event) {
          if (this.isOpen === false && !this.addTag) {
            return;
          }

          if (this.selectOnTab) {
            if (this.itemsList.markedItem) {
              this.toggleItem(this.itemsList.markedItem);
              $event.preventDefault();
            } else if (this.showAddTag) {
              this.selectTag();
              $event.preventDefault();
            } else {
              this.close();
            }
          } else {
            this.close();
          }
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_handleEnter",
        value: function _handleEnter($event) {
          if (this.isOpen || this._manualOpen) {
            if (this.itemsList.markedItem) {
              this.toggleItem(this.itemsList.markedItem);
            } else if (this.showAddTag) {
              this.selectTag();
            }
          } else if (this.openOnEnter) {
            this.open();
          } else {
            return;
          }

          $event.preventDefault();
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_handleSpace",
        value: function _handleSpace($event) {
          if (this.isOpen || this._manualOpen) {
            return;
          }

          this.open();
          $event.preventDefault();
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_handleArrowDown",
        value: function _handleArrowDown($event) {
          if (this._nextItemIsTag(+1)) {
            this.itemsList.unmarkItem();

            this._scrollToTag();
          } else {
            this.itemsList.markNextItem();

            this._scrollToMarked();
          }

          this.open();
          $event.preventDefault();
        }
        /**
         * @private
         * @param {?} $event
         * @return {?}
         */

      }, {
        key: "_handleArrowUp",
        value: function _handleArrowUp($event) {
          if (!this.isOpen) {
            return;
          }

          if (this._nextItemIsTag(-1)) {
            this.itemsList.unmarkItem();

            this._scrollToTag();
          } else {
            this.itemsList.markPreviousItem();

            this._scrollToMarked();
          }

          $event.preventDefault();
        }
        /**
         * @private
         * @param {?} nextStep
         * @return {?}
         */

      }, {
        key: "_nextItemIsTag",
        value: function _nextItemIsTag(nextStep) {
          /** @type {?} */
          var nextIndex = this.itemsList.markedIndex + nextStep;
          return this.addTag && this.searchTerm && this.itemsList.markedItem && (nextIndex < 0 || nextIndex === this.itemsList.filteredItems.length);
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_handleBackspace",
        value: function _handleBackspace() {
          if (this.searchTerm || !this.clearable || !this.clearOnBackspace || !this.hasValue) {
            return;
          }

          if (this.multiple) {
            this.unselect(this.itemsList.lastSelectedItem);
          } else {
            this.clearModel();
          }
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_isTypeahead",
        get: function get() {
          return this.typeahead && this.typeahead.observers.length > 0;
        }
        /**
         * @private
         * @return {?}
         */

      }, {
        key: "_validTerm",
        get: function get() {
          /** @type {?} */
          var term = this.searchTerm && this.searchTerm.trim();
          return term && term.length >= this.minTermLength;
        }
        /**
         * @private
         * @param {?} config
         * @return {?}
         */

      }, {
        key: "_mergeGlobalConfig",
        value: function _mergeGlobalConfig(config) {
          this.placeholder = this.placeholder || config.placeholder;
          this.notFoundText = this.notFoundText || config.notFoundText;
          this.typeToSearchText = this.typeToSearchText || config.typeToSearchText;
          this.addTagText = this.addTagText || config.addTagText;
          this.loadingText = this.loadingText || config.loadingText;
          this.clearAllText = this.clearAllText || config.clearAllText;
          this.virtualScroll = isDefined(this.virtualScroll) ? this.virtualScroll : isDefined(config.disableVirtualScroll) ? !config.disableVirtualScroll : false;
          this.openOnEnter = isDefined(this.openOnEnter) ? this.openOnEnter : config.openOnEnter;
          this.appendTo = this.appendTo || config.appendTo;
          this.bindValue = this.bindValue || config.bindValue;
          this.appearance = this.appearance || config.appearance;
        }
      }]);

      return NgSelectComponent;
    }();

    NgSelectComponent.ɵfac = function NgSelectComponent_Factory(t) {
      return new (t || NgSelectComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinjectAttribute"]('class'), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinjectAttribute"]('autofocus'), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](NgSelectConfig), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](SELECTION_MODEL_FACTORY), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](ConsoleService));
    };

    NgSelectComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: NgSelectComponent,
      selectors: [["ng-select"]],
      contentQueries: function NgSelectComponent_ContentQueries(rf, ctx, dirIndex) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgOptionTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgOptgroupTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgLabelTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgMultiLabelTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgHeaderTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgFooterTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgNotFoundTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgTypeToSearchTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgLoadingTextTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgTagTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgLoadingSpinnerTemplateDirective, true, _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵcontentQuery"](dirIndex, NgOptionComponent, true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.optionTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.optgroupTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.labelTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.multiLabelTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.headerTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.footerTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.notFoundTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.typeToSearchTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.loadingTextTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.tagTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.loadingSpinnerTemplate = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.ngOptions = _t);
        }
      },
      viewQuery: function NgSelectComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](NgDropdownPanelComponent, true);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵstaticViewQuery"](_c5, true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.dropdownPanel = _t.first);
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.searchInput = _t.first);
        }
      },
      hostAttrs: ["role", "listbox"],
      hostVars: 20,
      hostBindings: function NgSelectComponent_HostBindings(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("keydown", function NgSelectComponent_keydown_HostBindingHandler($event) {
            return ctx.handleKeyDown($event);
          });
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-select", ctx.useDefaultClass)("ng-select-single", !ctx.multiple)("ng-select-multiple", ctx.multiple)("ng-select-taggable", ctx.addTag)("ng-select-searchable", ctx.searchable)("ng-select-clearable", ctx.clearable)("ng-select-opened", ctx.isOpen)("ng-select-disabled", ctx.disabled)("ng-select-filtered", ctx.filtered)("ng-select-typeahead", ctx.typeahead);
        }
      },
      inputs: {
        markFirst: "markFirst",
        dropdownPosition: "dropdownPosition",
        loading: "loading",
        closeOnSelect: "closeOnSelect",
        hideSelected: "hideSelected",
        selectOnTab: "selectOnTab",
        bufferAmount: "bufferAmount",
        selectableGroup: "selectableGroup",
        selectableGroupAsModel: "selectableGroupAsModel",
        searchFn: "searchFn",
        trackByFn: "trackByFn",
        clearOnBackspace: "clearOnBackspace",
        labelForId: "labelForId",
        inputAttrs: "inputAttrs",
        readonly: "readonly",
        searchWhileComposing: "searchWhileComposing",
        minTermLength: "minTermLength",
        editableSearchTerm: "editableSearchTerm",
        keyDownFn: "keyDownFn",
        multiple: "multiple",
        addTag: "addTag",
        searchable: "searchable",
        clearable: "clearable",
        isOpen: "isOpen",
        items: "items",
        compareWith: "compareWith",
        clearSearchOnAdd: "clearSearchOnAdd",
        bindLabel: "bindLabel",
        placeholder: "placeholder",
        notFoundText: "notFoundText",
        typeToSearchText: "typeToSearchText",
        addTagText: "addTagText",
        loadingText: "loadingText",
        clearAllText: "clearAllText",
        virtualScroll: "virtualScroll",
        openOnEnter: "openOnEnter",
        appendTo: "appendTo",
        bindValue: "bindValue",
        appearance: "appearance",
        maxSelectedItems: "maxSelectedItems",
        groupBy: "groupBy",
        groupValue: "groupValue",
        tabIndex: "tabIndex",
        typeahead: "typeahead"
      },
      outputs: {
        blurEvent: "blur",
        focusEvent: "focus",
        changeEvent: "change",
        openEvent: "open",
        closeEvent: "close",
        searchEvent: "search",
        clearEvent: "clear",
        addEvent: "add",
        removeEvent: "remove",
        scroll: "scroll",
        scrollToEnd: "scrollToEnd"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵProvidersFeature"]([{
        provide: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NG_VALUE_ACCESSOR"],
        useExisting: Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["forwardRef"])(
        /**
        * @return {?}
        */
        function () {
          return NgSelectComponent;
        }),
        multi: true
      }, NgDropdownPanelService]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 14,
      vars: 18,
      consts: [[1, "ng-select-container", 3, "mousedown"], [1, "ng-value-container"], [1, "ng-placeholder"], [4, "ngIf"], [1, "ng-input"], ["role", "combobox", 3, "readOnly", "disabled", "value", "input", "compositionstart", "compositionend", "focus", "blur", "change"], ["searchInput", ""], ["class", "ng-clear-wrapper", 3, "title", 4, "ngIf"], [1, "ng-arrow-wrapper"], [1, "ng-arrow"], ["class", "ng-dropdown-panel", 3, "virtualScroll", "bufferAmount", "appendTo", "position", "headerTemplate", "footerTemplate", "filterValue", "items", "markedItem", "ng-select-multiple", "ngClass", "id", "update", "scroll", "scrollToEnd", "outsideClick", 4, "ngIf"], ["class", "ng-value", 3, "ng-value-disabled", 4, "ngFor", "ngForOf", "ngForTrackBy"], [1, "ng-value"], ["defaultLabelTemplate", ""], [3, "ngTemplateOutlet", "ngTemplateOutletContext"], ["aria-hidden", "true", 1, "ng-value-icon", "left", 3, "click"], [1, "ng-value-label", 3, "ngItemLabel", "escape"], ["defaultLoadingSpinnerTemplate", ""], [3, "ngTemplateOutlet"], [1, "ng-spinner-loader"], [1, "ng-clear-wrapper", 3, "title"], ["aria-hidden", "true", 1, "ng-clear"], [1, "ng-dropdown-panel", 3, "virtualScroll", "bufferAmount", "appendTo", "position", "headerTemplate", "footerTemplate", "filterValue", "items", "markedItem", "ngClass", "id", "update", "scroll", "scrollToEnd", "outsideClick"], ["class", "ng-option", 3, "ng-option-disabled", "ng-option-selected", "ng-optgroup", "ng-option", "ng-option-child", "ng-option-marked", "click", "mouseover", 4, "ngFor", "ngForOf", "ngForTrackBy"], ["class", "ng-option", "role", "option", 3, "ng-option-marked", "mouseover", "click", 4, "ngIf"], [1, "ng-option", 3, "click", "mouseover"], ["defaultOptionTemplate", ""], [1, "ng-option-label", 3, "ngItemLabel", "escape"], ["role", "option", 1, "ng-option", 3, "mouseover", "click"], ["defaultTagTemplate", ""], [1, "ng-tag-label"], ["defaultNotFoundTemplate", ""], [1, "ng-option", "ng-option-disabled"], ["defaultTypeToSearchTemplate", ""], ["defaultLoadingTextTemplate", ""]],
      template: function NgSelectComponent_Template(rf, ctx) {
        if (rf & 1) {
          var _r52 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("mousedown", function NgSelectComponent_Template_div_mousedown_0_listener($event) {
            return ctx.handleMousedown($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, NgSelectComponent_ng_container_4_Template, 2, 2, "ng-container", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, NgSelectComponent_5_Template, 1, 5, undefined, 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "input", 5, 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("input", function NgSelectComponent_Template_input_input_7_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r52);

            var _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](8);

            return ctx.filter(_r2.value);
          })("compositionstart", function NgSelectComponent_Template_input_compositionstart_7_listener() {
            return ctx.onCompositionStart();
          })("compositionend", function NgSelectComponent_Template_input_compositionend_7_listener() {
            _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r52);

            var _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](8);

            return ctx.onCompositionEnd(_r2.value);
          })("focus", function NgSelectComponent_Template_input_focus_7_listener($event) {
            return ctx.onInputFocus($event);
          })("blur", function NgSelectComponent_Template_input_blur_7_listener($event) {
            return ctx.onInputBlur($event);
          })("change", function NgSelectComponent_Template_input_change_7_listener($event) {
            return $event.stopPropagation();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, NgSelectComponent_ng_container_9_Template, 4, 1, "ng-container", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, NgSelectComponent_span_10_Template, 3, 1, "span", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "span", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](12, "span", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, NgSelectComponent_ng_dropdown_panel_13_Template, 7, 19, "ng-dropdown-panel", 10);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵclassProp"]("ng-appearance-outline", ctx.appearance === "outline")("ng-has-value", ctx.hasValue);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.placeholder);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.multiLabelTemplate && ctx.selectedItems.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.multiLabelTemplate && ctx.selectedValues.length > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("readOnly", !ctx.searchable || ctx.itemsList.maxItemsSelected)("disabled", ctx.disabled)("value", ctx.searchTerm ? ctx.searchTerm : "");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵattribute"]("id", ctx.labelForId)("tabindex", ctx.tabIndex)("aria-expanded", ctx.isOpen)("aria-owns", ctx.isOpen ? ctx.dropdownId : null)("aria-activedescendant", ctx.isOpen ? ctx.itemsList == null ? null : ctx.itemsList.markedItem == null ? null : ctx.itemsList.markedItem.htmlId : null);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.loading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.showClear());

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isOpen);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgTemplateOutlet"], NgItemLabelDirective, NgDropdownPanelComponent, _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgClass"]],
      styles: [".ng-select{position:relative;display:block;box-sizing:border-box}.ng-select div,.ng-select input,.ng-select span{box-sizing:border-box}.ng-select [hidden]{display:none}.ng-select.ng-select-searchable .ng-select-container .ng-value-container .ng-input{opacity:1}.ng-select.ng-select-opened .ng-select-container{z-index:1001}.ng-select.ng-select-disabled .ng-select-container .ng-value-container .ng-placeholder,.ng-select.ng-select-disabled .ng-select-container .ng-value-container .ng-value{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;cursor:default}.ng-select.ng-select-disabled .ng-arrow-wrapper{cursor:default}.ng-select.ng-select-filtered .ng-placeholder{display:none}.ng-select .ng-select-container{cursor:default;display:flex;outline:0;overflow:hidden;position:relative;width:100%}.ng-select .ng-select-container .ng-value-container{display:flex;flex:1}.ng-select .ng-select-container .ng-value-container .ng-input{opacity:0}.ng-select .ng-select-container .ng-value-container .ng-input>input{box-sizing:content-box;background:none;border:0;box-shadow:none;outline:0;cursor:default;width:100%}.ng-select .ng-select-container .ng-value-container .ng-input>input::-ms-clear{display:none}.ng-select .ng-select-container .ng-value-container .ng-input>input[readonly]{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;width:0;padding:0}.ng-select.ng-select-single.ng-select-filtered .ng-select-container .ng-value-container .ng-value{visibility:hidden}.ng-select.ng-select-single .ng-select-container .ng-value-container,.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-value{white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-value .ng-value-icon{display:none}.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-input{position:absolute;left:0;width:100%}.ng-select.ng-select-multiple.ng-select-disabled>.ng-select-container .ng-value-container .ng-value .ng-value-icon{display:none}.ng-select.ng-select-multiple .ng-select-container .ng-value-container{flex-wrap:wrap}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value{white-space:nowrap}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value.ng-value-disabled .ng-value-icon{display:none}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value .ng-value-icon{cursor:pointer}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-input{flex:1;z-index:2}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-placeholder{position:absolute;z-index:1}.ng-select .ng-clear-wrapper{cursor:pointer;position:relative;width:17px;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.ng-select .ng-clear-wrapper .ng-clear{display:inline-block;font-size:18px;line-height:1;pointer-events:none}.ng-select .ng-spinner-loader{border-radius:50%;width:17px;height:17px;margin-right:5px;font-size:10px;position:relative;text-indent:-9999em;border-top:2px solid rgba(66,66,66,.2);border-right:2px solid rgba(66,66,66,.2);border-bottom:2px solid rgba(66,66,66,.2);border-left:2px solid #424242;transform:translateZ(0);-webkit-animation:.8s linear infinite load8;animation:.8s linear infinite load8}.ng-select .ng-spinner-loader:after{border-radius:50%;width:17px;height:17px}@-webkit-keyframes load8{0%{transform:rotate(0)}100%{transform:rotate(360deg)}}@keyframes load8{0%{transform:rotate(0)}100%{transform:rotate(360deg)}}.ng-select .ng-arrow-wrapper{cursor:pointer;position:relative;text-align:center;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.ng-select .ng-arrow-wrapper .ng-arrow{pointer-events:none;display:inline-block;height:0;width:0;position:relative}.ng-dropdown-panel{box-sizing:border-box;position:absolute;opacity:0;width:100%;z-index:1050;-webkit-overflow-scrolling:touch}.ng-dropdown-panel .ng-dropdown-panel-items{display:block;height:auto;box-sizing:border-box;max-height:240px;overflow-y:auto}.ng-dropdown-panel .ng-dropdown-panel-items .ng-optgroup{white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option{box-sizing:border-box;cursor:pointer;display:block;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option .highlighted{font-weight:700;text-decoration:underline}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option.disabled{cursor:default}.ng-dropdown-panel .scroll-host{overflow:hidden;overflow-y:auto;position:relative;display:block;-webkit-overflow-scrolling:touch}.ng-dropdown-panel .scrollable-content{top:0;left:0;width:100%;height:100%;position:absolute}.ng-dropdown-panel .total-padding{width:1px;opacity:0}"],
      encapsulation: 2,
      changeDetection: 0
    });
    /** @nocollapse */

    NgSelectComponent.ctorParameters = function () {
      return [{
        type: String,
        decorators: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Attribute"],
          args: ['class']
        }]
      }, {
        type: undefined,
        decorators: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Attribute"],
          args: ['autofocus']
        }]
      }, {
        type: NgSelectConfig
      }, {
        type: undefined,
        decorators: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Inject"],
          args: [SELECTION_MODEL_FACTORY]
        }]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]
      }, {
        type: ConsoleService
      }];
    };

    NgSelectComponent.propDecorators = {
      bindLabel: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      bindValue: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      markFirst: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      placeholder: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      notFoundText: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      typeToSearchText: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      addTagText: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      loadingText: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      clearAllText: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      appearance: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      dropdownPosition: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      appendTo: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      loading: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      closeOnSelect: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      hideSelected: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      selectOnTab: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      openOnEnter: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      maxSelectedItems: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      groupBy: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      groupValue: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      bufferAmount: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      virtualScroll: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      selectableGroup: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      selectableGroupAsModel: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      searchFn: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      trackByFn: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      clearOnBackspace: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      labelForId: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      inputAttrs: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      tabIndex: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      readonly: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      searchWhileComposing: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      minTermLength: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      editableSearchTerm: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      keyDownFn: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      typeahead: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-typeahead']
      }],
      multiple: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-multiple']
      }],
      addTag: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-taggable']
      }],
      searchable: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-searchable']
      }],
      clearable: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-clearable']
      }],
      isOpen: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }, {
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-opened']
      }],
      items: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      compareWith: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      clearSearchOnAdd: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      blurEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['blur']
      }],
      focusEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['focus']
      }],
      changeEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['change']
      }],
      openEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['open']
      }],
      closeEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['close']
      }],
      searchEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['search']
      }],
      clearEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['clear']
      }],
      addEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['add']
      }],
      removeEvent: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['remove']
      }],
      scroll: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['scroll']
      }],
      scrollToEnd: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
        args: ['scrollToEnd']
      }],
      optionTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgOptionTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      optgroupTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgOptgroupTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      labelTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgLabelTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      multiLabelTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgMultiLabelTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      headerTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgHeaderTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      footerTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgFooterTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      notFoundTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgNotFoundTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      typeToSearchTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgTypeToSearchTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      loadingTextTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgLoadingTextTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      tagTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgTagTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      loadingSpinnerTemplate: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
        args: [NgLoadingSpinnerTemplateDirective, {
          read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
        }]
      }],
      dropdownPanel: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
        args: [Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["forwardRef"])(
        /**
        * @return {?}
        */
        function () {
          return NgDropdownPanelComponent;
        })]
      }],
      searchInput: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
        args: ['searchInput', {
          "static": true
        }]
      }],
      ngOptions: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChildren"],
        args: [NgOptionComponent, {
          descendants: true
        }]
      }],
      disabled: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-disabled']
      }],
      filtered: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
        args: ['class.ng-select-filtered']
      }],
      handleKeyDown: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostListener"],
        args: ['keydown', ['$event']]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgSelectComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'ng-select',
          template: "<div\n    (mousedown)=\"handleMousedown($event)\"\n    [class.ng-appearance-outline]=\"appearance === 'outline'\"\n    [class.ng-has-value]=\"hasValue\"\n    class=\"ng-select-container\">\n\n    <div class=\"ng-value-container\">\n        <div class=\"ng-placeholder\">{{placeholder}}</div>\n\n        <ng-container *ngIf=\"!multiLabelTemplate && selectedItems.length > 0\">\n            <div [class.ng-value-disabled]=\"item.disabled\" class=\"ng-value\" *ngFor=\"let item of selectedItems; trackBy: trackByOption\">\n                <ng-template #defaultLabelTemplate>\n                    <span class=\"ng-value-icon left\" (click)=\"unselect(item);\" aria-hidden=\"true\">\xD7</span>\n                    <span class=\"ng-value-label\" [ngItemLabel]=\"item.label\" [escape]=\"escapeHTML\"></span>\n                </ng-template>\n\n                <ng-template\n                    [ngTemplateOutlet]=\"labelTemplate || defaultLabelTemplate\"\n                    [ngTemplateOutletContext]=\"{ item: item.value, clear: clearItem, label: item.label }\">\n                </ng-template>\n            </div>\n        </ng-container>\n\n        <ng-template *ngIf=\"multiLabelTemplate && selectedValues.length > 0\"\n                [ngTemplateOutlet]=\"multiLabelTemplate\"\n                [ngTemplateOutletContext]=\"{ items: selectedValues, clear: clearItem }\">\n        </ng-template>\n\n        <div class=\"ng-input\">\n            <input #searchInput\n                   [attr.id]=\"labelForId\"\n                   [attr.tabindex]=\"tabIndex\"\n                   [readOnly]=\"!searchable || itemsList.maxItemsSelected\"\n                   [disabled]=\"disabled\"\n                   [value]=\"searchTerm ? searchTerm : ''\"\n                   (input)=\"filter(searchInput.value)\"\n                   (compositionstart)=\"onCompositionStart()\"\n                   (compositionend)=\"onCompositionEnd(searchInput.value)\"\n                   (focus)=\"onInputFocus($event)\"\n                   (blur)=\"onInputBlur($event)\"\n                   (change)=\"$event.stopPropagation()\"\n                   role=\"combobox\"\n                   [attr.aria-expanded]=\"isOpen\"\n                   [attr.aria-owns]=\"isOpen ? dropdownId : null\"\n                   [attr.aria-activedescendant]=\"isOpen ? itemsList?.markedItem?.htmlId : null\">\n        </div>\n    </div>\n\n    <ng-container *ngIf=\"loading\">\n        <ng-template #defaultLoadingSpinnerTemplate>\n            <div class=\"ng-spinner-loader\"></div>\n        </ng-template>\n\n        <ng-template\n            [ngTemplateOutlet]=\"loadingSpinnerTemplate || defaultLoadingSpinnerTemplate\">\n        </ng-template>\n    </ng-container>\n\n    <span *ngIf=\"showClear()\" class=\"ng-clear-wrapper\" title=\"{{clearAllText}}\">\n        <span class=\"ng-clear\" aria-hidden=\"true\">\xD7</span>\n    </span>\n\n    <span class=\"ng-arrow-wrapper\">\n        <span class=\"ng-arrow\"></span>\n    </span>\n</div>\n\n<ng-dropdown-panel *ngIf=\"isOpen\"\n                   class=\"ng-dropdown-panel\"\n                   [virtualScroll]=\"virtualScroll\"\n                   [bufferAmount]=\"bufferAmount\"\n                   [appendTo]=\"appendTo\"\n                   [position]=\"dropdownPosition\"\n                   [headerTemplate]=\"headerTemplate\"\n                   [footerTemplate]=\"footerTemplate\"\n                   [filterValue]=\"searchTerm\"\n                   [items]=\"itemsList.filteredItems\"\n                   [markedItem]=\"itemsList.markedItem\"\n                   (update)=\"viewPortItems = $event\"\n                   (scroll)=\"scroll.emit($event)\"\n                   (scrollToEnd)=\"scrollToEnd.emit($event)\"\n                   (outsideClick)=\"close()\"\n                   [class.ng-select-multiple]=\"multiple\"\n                   [ngClass]=\"appendTo ? classes : null\"\n                   [id]=\"dropdownId\">\n\n    <ng-container>\n        <div class=\"ng-option\" [attr.role]=\"item.children ? 'group' : 'option'\" (click)=\"toggleItem(item)\" (mouseover)=\"onItemHover(item)\"\n                *ngFor=\"let item of viewPortItems; trackBy: trackByOption\"\n                [class.ng-option-disabled]=\"item.disabled\"\n                [class.ng-option-selected]=\"item.selected\"\n                [class.ng-optgroup]=\"item.children\"\n                [class.ng-option]=\"!item.children\"\n                [class.ng-option-child]=\"!!item.parent\"\n                [class.ng-option-marked]=\"item === itemsList.markedItem\"\n                [attr.aria-selected]=\"item.selected\"\n                [attr.id]=\"item?.htmlId\">\n\n            <ng-template #defaultOptionTemplate>\n                <span class=\"ng-option-label\" [ngItemLabel]=\"item.label\" [escape]=\"escapeHTML\"></span>\n            </ng-template>\n\n            <ng-template\n                [ngTemplateOutlet]=\"item.children ? (optgroupTemplate || defaultOptionTemplate) : (optionTemplate || defaultOptionTemplate)\"\n                [ngTemplateOutletContext]=\"{ item: item.value, item$:item, index: item.index, searchTerm: searchTerm }\">\n            </ng-template>\n        </div>\n\n        <div class=\"ng-option\" [class.ng-option-marked]=\"!itemsList.markedItem\" (mouseover)=\"itemsList.unmarkItem()\" role=\"option\" (click)=\"selectTag()\" *ngIf=\"showAddTag\">\n            <ng-template #defaultTagTemplate>\n                <span><span class=\"ng-tag-label\">{{addTagText}}</span>\"{{searchTerm}}\"</span>\n            </ng-template>\n\n            <ng-template\n                [ngTemplateOutlet]=\"tagTemplate || defaultTagTemplate\"\n                [ngTemplateOutletContext]=\"{ searchTerm: searchTerm }\">\n            </ng-template>\n        </div>\n    </ng-container>\n\n    <ng-container *ngIf=\"showNoItemsFound()\">\n        <ng-template #defaultNotFoundTemplate>\n            <div class=\"ng-option ng-option-disabled\">{{notFoundText}}</div>\n        </ng-template>\n\n        <ng-template\n            [ngTemplateOutlet]=\"notFoundTemplate || defaultNotFoundTemplate\"\n            [ngTemplateOutletContext]=\"{ searchTerm: searchTerm }\">\n        </ng-template>\n    </ng-container>\n\n    <ng-container *ngIf=\"showTypeToSearch()\">\n        <ng-template #defaultTypeToSearchTemplate>\n            <div class=\"ng-option ng-option-disabled\">{{typeToSearchText}}</div>\n        </ng-template>\n\n        <ng-template\n            [ngTemplateOutlet]=\"typeToSearchTemplate || defaultTypeToSearchTemplate\">\n        </ng-template>\n    </ng-container>\n\n    <ng-container *ngIf=\"loading && itemsList.filteredItems.length === 0\">\n        <ng-template #defaultLoadingTextTemplate>\n            <div class=\"ng-option ng-option-disabled\">{{loadingText}}</div>\n        </ng-template>\n\n        <ng-template\n            [ngTemplateOutlet]=\"loadingTextTemplate || defaultLoadingTextTemplate\"\n            [ngTemplateOutletContext]=\"{ searchTerm: searchTerm }\">\n        </ng-template>\n    </ng-container>\n\n</ng-dropdown-panel>\n",
          providers: [{
            provide: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NG_VALUE_ACCESSOR"],
            useExisting: Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["forwardRef"])(
            /**
            * @return {?}
            */
            function () {
              return NgSelectComponent;
            }),
            multi: true
          }, NgDropdownPanelService],
          encapsulation: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewEncapsulation"].None,
          changeDetection: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectionStrategy"].OnPush,
          host: {
            'role': 'listbox',
            '[class.ng-select]': 'useDefaultClass',
            '[class.ng-select-single]': '!multiple'
          },
          styles: [".ng-select{position:relative;display:block;box-sizing:border-box}.ng-select div,.ng-select input,.ng-select span{box-sizing:border-box}.ng-select [hidden]{display:none}.ng-select.ng-select-searchable .ng-select-container .ng-value-container .ng-input{opacity:1}.ng-select.ng-select-opened .ng-select-container{z-index:1001}.ng-select.ng-select-disabled .ng-select-container .ng-value-container .ng-placeholder,.ng-select.ng-select-disabled .ng-select-container .ng-value-container .ng-value{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;cursor:default}.ng-select.ng-select-disabled .ng-arrow-wrapper{cursor:default}.ng-select.ng-select-filtered .ng-placeholder{display:none}.ng-select .ng-select-container{cursor:default;display:flex;outline:0;overflow:hidden;position:relative;width:100%}.ng-select .ng-select-container .ng-value-container{display:flex;flex:1}.ng-select .ng-select-container .ng-value-container .ng-input{opacity:0}.ng-select .ng-select-container .ng-value-container .ng-input>input{box-sizing:content-box;background:none;border:0;box-shadow:none;outline:0;cursor:default;width:100%}.ng-select .ng-select-container .ng-value-container .ng-input>input::-ms-clear{display:none}.ng-select .ng-select-container .ng-value-container .ng-input>input[readonly]{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;width:0;padding:0}.ng-select.ng-select-single.ng-select-filtered .ng-select-container .ng-value-container .ng-value{visibility:hidden}.ng-select.ng-select-single .ng-select-container .ng-value-container,.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-value{white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-value .ng-value-icon{display:none}.ng-select.ng-select-single .ng-select-container .ng-value-container .ng-input{position:absolute;left:0;width:100%}.ng-select.ng-select-multiple.ng-select-disabled>.ng-select-container .ng-value-container .ng-value .ng-value-icon{display:none}.ng-select.ng-select-multiple .ng-select-container .ng-value-container{flex-wrap:wrap}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value{white-space:nowrap}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value.ng-value-disabled .ng-value-icon{display:none}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-value .ng-value-icon{cursor:pointer}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-input{flex:1;z-index:2}.ng-select.ng-select-multiple .ng-select-container .ng-value-container .ng-placeholder{position:absolute;z-index:1}.ng-select .ng-clear-wrapper{cursor:pointer;position:relative;width:17px;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.ng-select .ng-clear-wrapper .ng-clear{display:inline-block;font-size:18px;line-height:1;pointer-events:none}.ng-select .ng-spinner-loader{border-radius:50%;width:17px;height:17px;margin-right:5px;font-size:10px;position:relative;text-indent:-9999em;border-top:2px solid rgba(66,66,66,.2);border-right:2px solid rgba(66,66,66,.2);border-bottom:2px solid rgba(66,66,66,.2);border-left:2px solid #424242;transform:translateZ(0);-webkit-animation:.8s linear infinite load8;animation:.8s linear infinite load8}.ng-select .ng-spinner-loader:after{border-radius:50%;width:17px;height:17px}@-webkit-keyframes load8{0%{transform:rotate(0)}100%{transform:rotate(360deg)}}@keyframes load8{0%{transform:rotate(0)}100%{transform:rotate(360deg)}}.ng-select .ng-arrow-wrapper{cursor:pointer;position:relative;text-align:center;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.ng-select .ng-arrow-wrapper .ng-arrow{pointer-events:none;display:inline-block;height:0;width:0;position:relative}.ng-dropdown-panel{box-sizing:border-box;position:absolute;opacity:0;width:100%;z-index:1050;-webkit-overflow-scrolling:touch}.ng-dropdown-panel .ng-dropdown-panel-items{display:block;height:auto;box-sizing:border-box;max-height:240px;overflow-y:auto}.ng-dropdown-panel .ng-dropdown-panel-items .ng-optgroup{white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option{box-sizing:border-box;cursor:pointer;display:block;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option .highlighted{font-weight:700;text-decoration:underline}.ng-dropdown-panel .ng-dropdown-panel-items .ng-option.disabled{cursor:default}.ng-dropdown-panel .scroll-host{overflow:hidden;overflow-y:auto;position:relative;display:block;-webkit-overflow-scrolling:touch}.ng-dropdown-panel .scrollable-content{top:0;left:0;width:100%;height:100%;position:absolute}.ng-dropdown-panel .total-padding{width:1px;opacity:0}"]
        }]
      }], function () {
        return [{
          type: String,
          decorators: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Attribute"],
            args: ['class']
          }]
        }, {
          type: undefined,
          decorators: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Attribute"],
            args: ['autofocus']
          }]
        }, {
          type: NgSelectConfig
        }, {
          type: undefined,
          decorators: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Inject"],
            args: [SELECTION_MODEL_FACTORY]
          }]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ChangeDetectorRef"]
        }, {
          type: ConsoleService
        }];
      }, {
        markFirst: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        dropdownPosition: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        loading: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        closeOnSelect: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        hideSelected: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        selectOnTab: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        bufferAmount: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        selectableGroup: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        selectableGroupAsModel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        searchFn: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        trackByFn: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        clearOnBackspace: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        labelForId: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        inputAttrs: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        readonly: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        searchWhileComposing: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        minTermLength: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        editableSearchTerm: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        keyDownFn: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        multiple: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-multiple']
        }],
        addTag: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-taggable']
        }],
        searchable: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-searchable']
        }],
        clearable: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-clearable']
        }],
        isOpen: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-opened']
        }],
        blurEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['blur']
        }],
        focusEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['focus']
        }],
        changeEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['change']
        }],
        openEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['open']
        }],
        closeEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['close']
        }],
        searchEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['search']
        }],
        clearEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['clear']
        }],
        addEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['add']
        }],
        removeEvent: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['remove']
        }],
        scroll: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['scroll']
        }],
        scrollToEnd: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"],
          args: ['scrollToEnd']
        }],
        items: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        compareWith: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        clearSearchOnAdd: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        disabled: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-disabled']
        }],
        filtered: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-filtered']
        }],
        handleKeyDown: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostListener"],
          args: ['keydown', ['$event']]
        }],
        bindLabel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        placeholder: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        notFoundText: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        typeToSearchText: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        addTagText: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        loadingText: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        clearAllText: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        virtualScroll: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        openOnEnter: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        appendTo: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        bindValue: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        appearance: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        maxSelectedItems: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        groupBy: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        groupValue: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        tabIndex: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        typeahead: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }, {
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["HostBinding"],
          args: ['class.ng-select-typeahead']
        }],
        optionTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgOptionTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        optgroupTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgOptgroupTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        labelTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgLabelTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        multiLabelTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgMultiLabelTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        headerTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgHeaderTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        footerTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgFooterTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        notFoundTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgNotFoundTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        typeToSearchTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgTypeToSearchTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        loadingTextTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgLoadingTextTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        tagTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgTagTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        loadingSpinnerTemplate: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChild"],
          args: [NgLoadingSpinnerTemplateDirective, {
            read: _angular_core__WEBPACK_IMPORTED_MODULE_0__["TemplateRef"]
          }]
        }],
        dropdownPanel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["forwardRef"])(
          /**
          * @return {?}
          */
          function () {
            return NgDropdownPanelComponent;
          })]
        }],
        searchInput: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: ['searchInput', {
            "static": true
          }]
        }],
        ngOptions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ContentChildren"],
          args: [NgOptionComponent, {
            descendants: true
          }]
        }]
      });
    })();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/selection-model.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @return {?}
     */


    function DefaultSelectionModelFactory() {
      return new DefaultSelectionModel();
    }
    /**
     * @record
     */


    function SelectionModel() {}

    if (false) {}

    var DefaultSelectionModel = /*#__PURE__*/function () {
      function DefaultSelectionModel() {
        _classCallCheck(this, DefaultSelectionModel);

        this._selected = [];
      }
      /**
       * @return {?}
       */


      _createClass(DefaultSelectionModel, [{
        key: "value",
        get: function get() {
          return this._selected;
        }
        /**
         * @param {?} item
         * @param {?} multiple
         * @param {?} groupAsModel
         * @return {?}
         */

      }, {
        key: "select",
        value: function select(item, multiple, groupAsModel) {
          item.selected = true;

          if (!item.children || !multiple && groupAsModel) {
            this._selected.push(item);
          }

          if (multiple) {
            if (item.parent) {
              /** @type {?} */
              var childrenCount = item.parent.children.length;
              /** @type {?} */

              var selectedCount = item.parent.children.filter(
              /**
              * @param {?} x
              * @return {?}
              */
              function (x) {
                return x.selected;
              }).length;
              item.parent.selected = childrenCount === selectedCount;
            } else if (item.children) {
              this._setChildrenSelectedState(item.children, true);

              this._removeChildren(item);

              if (groupAsModel && this._activeChildren(item)) {
                this._selected = [].concat(_toConsumableArray(this._selected.filter(
                /**
                * @param {?} x
                * @return {?}
                */
                function (x) {
                  return x.parent !== item;
                })), [item]);
              } else {
                this._selected = [].concat(_toConsumableArray(this._selected), _toConsumableArray(item.children.filter(
                /**
                * @param {?} x
                * @return {?}
                */
                function (x) {
                  return !x.disabled;
                })));
              }
            }
          }
        }
        /**
         * @param {?} item
         * @param {?} multiple
         * @return {?}
         */

      }, {
        key: "unselect",
        value: function unselect(item, multiple) {
          this._selected = this._selected.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x !== item;
          });
          item.selected = false;

          if (multiple) {
            if (item.parent && item.parent.selected) {
              var _this$_selected;

              /** @type {?} */
              var children = item.parent.children;

              this._removeParent(item.parent);

              this._removeChildren(item.parent);

              (_this$_selected = this._selected).push.apply(_this$_selected, _toConsumableArray(children.filter(
              /**
              * @param {?} x
              * @return {?}
              */
              function (x) {
                return x !== item && !x.disabled;
              })));

              item.parent.selected = false;
            } else if (item.children) {
              this._setChildrenSelectedState(item.children, false);

              this._removeChildren(item);
            }
          }
        }
        /**
         * @param {?} keepDisabled
         * @return {?}
         */

      }, {
        key: "clear",
        value: function clear(keepDisabled) {
          this._selected = keepDisabled ? this._selected.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.disabled;
          }) : [];
        }
        /**
         * @private
         * @param {?} children
         * @param {?} selected
         * @return {?}
         */

      }, {
        key: "_setChildrenSelectedState",
        value: function _setChildrenSelectedState(children, selected) {
          var _iterator7 = _createForOfIteratorHelper(children),
              _step7;

          try {
            for (_iterator7.s(); !(_step7 = _iterator7.n()).done;) {
              var child = _step7.value;

              if (child.disabled) {
                continue;
              }

              child.selected = selected;
            }
          } catch (err) {
            _iterator7.e(err);
          } finally {
            _iterator7.f();
          }

          ;
        }
        /**
         * @private
         * @param {?} parent
         * @return {?}
         */

      }, {
        key: "_removeChildren",
        value: function _removeChildren(parent) {
          this._selected = [].concat(_toConsumableArray(this._selected.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.parent !== parent;
          })), _toConsumableArray(parent.children.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x.parent === parent && x.disabled && x.selected;
          })));
        }
        /**
         * @private
         * @param {?} parent
         * @return {?}
         */

      }, {
        key: "_removeParent",
        value: function _removeParent(parent) {
          this._selected = this._selected.filter(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return x !== parent;
          });
        }
        /**
         * @private
         * @param {?} item
         * @return {?}
         */

      }, {
        key: "_activeChildren",
        value: function _activeChildren(item) {
          return item.children.every(
          /**
          * @param {?} x
          * @return {?}
          */
          function (x) {
            return !x.disabled || x.selected;
          });
        }
      }]);

      return DefaultSelectionModel;
    }();

    if (false) {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/ng-select.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */


    var ɵ0 = DefaultSelectionModelFactory;

    var NgSelectModule = function NgSelectModule() {
      _classCallCheck(this, NgSelectModule);
    };

    NgSelectModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: NgSelectModule
    });
    NgSelectModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function NgSelectModule_Factory(t) {
        return new (t || NgSelectModule)();
      },
      providers: [{
        provide: SELECTION_MODEL_FACTORY,
        useValue: ɵ0
      }],
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_4__["CommonModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](NgSelectModule, {
        declarations: function declarations() {
          return [NgDropdownPanelComponent, NgOptionComponent, NgSelectComponent, NgOptgroupTemplateDirective, NgOptionTemplateDirective, NgLabelTemplateDirective, NgMultiLabelTemplateDirective, NgHeaderTemplateDirective, NgFooterTemplateDirective, NgNotFoundTemplateDirective, NgTypeToSearchTemplateDirective, NgLoadingTextTemplateDirective, NgTagTemplateDirective, NgLoadingSpinnerTemplateDirective, NgItemLabelDirective];
        },
        imports: function imports() {
          return [_angular_common__WEBPACK_IMPORTED_MODULE_4__["CommonModule"]];
        },
        exports: function exports() {
          return [NgSelectComponent, NgOptionComponent, NgOptgroupTemplateDirective, NgOptionTemplateDirective, NgLabelTemplateDirective, NgMultiLabelTemplateDirective, NgHeaderTemplateDirective, NgFooterTemplateDirective, NgNotFoundTemplateDirective, NgTypeToSearchTemplateDirective, NgLoadingTextTemplateDirective, NgTagTemplateDirective, NgLoadingSpinnerTemplateDirective];
        }
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgSelectModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [NgDropdownPanelComponent, NgOptionComponent, NgSelectComponent, NgOptgroupTemplateDirective, NgOptionTemplateDirective, NgLabelTemplateDirective, NgMultiLabelTemplateDirective, NgHeaderTemplateDirective, NgFooterTemplateDirective, NgNotFoundTemplateDirective, NgTypeToSearchTemplateDirective, NgLoadingTextTemplateDirective, NgTagTemplateDirective, NgLoadingSpinnerTemplateDirective, NgItemLabelDirective],
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["CommonModule"]],
          exports: [NgSelectComponent, NgOptionComponent, NgOptgroupTemplateDirective, NgOptionTemplateDirective, NgLabelTemplateDirective, NgMultiLabelTemplateDirective, NgHeaderTemplateDirective, NgFooterTemplateDirective, NgNotFoundTemplateDirective, NgTypeToSearchTemplateDirective, NgLoadingTextTemplateDirective, NgTagTemplateDirective, NgLoadingSpinnerTemplateDirective],
          providers: [{
            provide: SELECTION_MODEL_FACTORY,
            useValue: ɵ0
          }]
        }]
      }], null, null);
    })();
    /**
     * @fileoverview added by tsickle
     * Generated from: public-api.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: ng-select-ng-select.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    //# sourceMappingURL=ng-select-ng-select.js.map

    /***/

  },

  /***/
  "./src/app/company-addresses/address/address.component.ts": function srcAppCompanyAddressesAddressAddressComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AddressComponent", function () {
      return AddressComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _declarations_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../../declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _service_address_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./service/address.service */
    "./src/app/company-addresses/address/service/address.service.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function AddressComponent_div_9_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "label");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " No Addresses available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "span", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_12_span_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 26);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Default");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_12_ng_template_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 27);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Default");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_12_span_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 28);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function AddressComponent_div_12_span_9_Template_span_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r12);

          var address_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.changeAddressStatus(address_r3.Id, !address_r3.IsActive, address_r3.IsDefault);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Active");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_12_ng_template_10_Template(rf, ctx) {
      if (rf & 1) {
        var _r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function AddressComponent_div_12_ng_template_10_Template_span_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r15);

          var address_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r13.changeAddressStatus(address_r3.Id, !address_r3.IsActive, address_r3.IsDefault);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "InActive");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function AddressComponent_div_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 13);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 15);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, AddressComponent_div_12_span_5_Template, 2, 0, "span", 17);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, AddressComponent_div_12_ng_template_6_Template, 2, 0, "ng-template", null, 18, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, " \xA0 ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, AddressComponent_div_12_span_9_Template, 2, 0, "span", 19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, AddressComponent_div_12_ng_template_10_Template, 2, 0, "ng-template", null, 20, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "a", 21);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "i", 22);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "small", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Address: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "small", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "City: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "small", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "State: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "small", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](35, "ZipCode: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 23);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "small", 24);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Country: ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 25);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var address_r3 = ctx.$implicit;

        var _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](7);

        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](11);

        var ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", address_r3.IsDefault)("ngIfElse", _r5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", address_r3.IsActive)("ngIfElse", _r8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate2"]("href", "/Settings/Profile/CompanyAddress?id=", address_r3.Id, "&companyId=", ctx_r2.CompanyId, "", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsanitizeUrl"]);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](address_r3.Address);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](address_r3.City);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](address_r3.State.Code);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](address_r3.ZipCode);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](address_r3.Country.Code);
      }
    }

    var AddressComponent = /*#__PURE__*/function () {
      function AddressComponent(addressService, route) {
        _classCallCheck(this, AddressComponent);

        this.addressService = addressService;
        this.route = route;
        this.IsLoading = true;
        this.IsEmpty = false;
      }

      _createClass(AddressComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.CompanyId = currentUserCompanyId != undefined ? currentUserCompanyId : parseInt(this.route.snapshot.queryParamMap.get('id'));
          this.getAddresses();
        }
      }, {
        key: "getAddresses",
        value: function getAddresses() {
          var _this22 = this;

          this.IsLoading = true;
          this.addressService.getAddresses(this.CompanyId).subscribe(function (Addresses) {
            _this22.Addresses = Addresses;
            _this22.IsLoading = false;
          });
        }
      }, {
        key: "showSuccess",
        value: function showSuccess() {
          _declarations_module__WEBPACK_IMPORTED_MODULE_1__["Declarations"].msgwarning("Cannot Make Default Address as InActive", undefined, undefined);
        }
      }, {
        key: "changeAddressStatus",
        value: function changeAddressStatus(CompanyAddressId, IsActive, IsDefault) {
          var _this23 = this;

          if (IsDefault) {
            this.showSuccess();
          } else {
            this.addressService.changeAddressStatus(CompanyAddressId, IsActive).subscribe(function (response) {
              _this23.getAddresses();
            });
          }

          ;
        }
      }]);

      return AddressComponent;
    }();

    AddressComponent.ɵfac = function AddressComponent_Factory(t) {
      return new (t || AddressComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_address_service__WEBPACK_IMPORTED_MODULE_2__["AddressService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__["ActivatedRoute"]));
    };

    AddressComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: AddressComponent,
      selectors: [["address-component"]],
      decls: 13,
      vars: 3,
      consts: [[1, "row"], [1, "col-sm-12", "pt20"], [1, "pt0", "pull-left"], ["id", "auto-createnewaddress", "href", "/Settings/Profile/CompanyAddress", 1, "fs18", "pull-left", "ml20"], [1, "fa", "fa-plus-circle", "fs18", "mt2", "pull-left"], [1, "fs14", "pull-left"], ["class", "col-sm-12 text-center", 4, "ngIf"], [1, "row", "pr"], ["class", "pa bg-white z-index5 loading-wrapper left0 top0", 4, "ngIf"], ["class", "col-sm-3 ", 4, "ngFor", "ngForOf"], [1, "col-sm-12", "text-center"], [1, "pa", "bg-white", "z-index5", "loading-wrapper", "left0", "top0"], [1, "spinner-dashboard", "pa"], [1, "col-sm-3"], [1, "well", "box-shadow", "tile-xs", "pr"], [1, "col-sm-12"], [1, "border-b", "pb10"], ["class", "badge badge-primary", 4, "ngIf", "ngIfElse"], ["elseDefault", ""], ["class", "badge badge-primary", "style", "cursor:pointer;", 3, "click", 4, "ngIf", "ngIfElse"], ["elseInActive", ""], [1, "pull-right", 3, "href"], [1, "fa", "fa-edit"], [1, "col-sm-4"], [1, "mb5", "color-lightgrey", "f-bold"], [1, "col-sm-8"], [1, "badge", "badge-primary"], [1, "badge", "badge-secondary"], [1, "badge", "badge-primary", 2, "cursor", "pointer", 3, "click"], [1, "badge", "badge-secondary", 2, "cursor", "pointer", 3, "click"]],
      template: function AddressComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h4", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Address");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "a", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](5, "i", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "span", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Create New");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, AddressComponent_div_9_Template, 3, 0, "div", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, AddressComponent_div_11_Template, 2, 0, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, AddressComponent_div_12_Template, 44, 11, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.Addresses || (ctx.Addresses == null ? null : ctx.Addresses.length) == 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.Addresses);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgForOf"]],
      styles: [".no-data-available[_ngcontent-%COMP%] {\r\n    text-align: center;\r\n}\r\n\r\n.dataTables_empty[_ngcontent-%COMP%] {\r\n    display: none;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvY29tcGFueS1hZGRyZXNzZXMvYWRkcmVzcy9hZGRyZXNzLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxrQkFBa0I7QUFDdEI7O0FBRUE7SUFDSSxhQUFhO0FBQ2pCIiwiZmlsZSI6InNyYy9hcHAvY29tcGFueS1hZGRyZXNzZXMvYWRkcmVzcy9hZGRyZXNzLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIubm8tZGF0YS1hdmFpbGFibGUge1xyXG4gICAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG59XHJcblxyXG4uZGF0YVRhYmxlc19lbXB0eSB7XHJcbiAgICBkaXNwbGF5OiBub25lO1xyXG59XHJcbiJdfQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AddressComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'address-component',
          templateUrl: './address.component.html',
          styleUrls: ['./address.component.css']
        }]
      }], function () {
        return [{
          type: _service_address_service__WEBPACK_IMPORTED_MODULE_2__["AddressService"]
        }, {
          type: _angular_router__WEBPACK_IMPORTED_MODULE_3__["ActivatedRoute"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/address/service/address.service.ts": function srcAppCompanyAddressesAddressServiceAddressServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AddressService", function () {
      return AddressService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var AddressService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(AddressService, _src_app_errors_Handl);

      var _super = _createSuper(AddressService);

      function AddressService(httpClient) {
        var _this24;

        _classCallCheck(this, AddressService);

        _this24 = _super.call(this);
        _this24.httpClient = httpClient;
        _this24.serviceUrl = '/Profile/CompanyAddressGrid?companyId=';
        _this24.changeStatusUrl = '/Settings/Profile/ChangeCompanyAddressStatus';
        return _this24;
      }

      _createClass(AddressService, [{
        key: "getAddresses",
        value: function getAddresses(companyId) {
          return this.httpClient.get(this.serviceUrl + companyId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getAddresses', [])));
        }
      }, {
        key: "changeAddressStatus",
        value: function changeAddressStatus(companyAddressId, IsActive) {
          return this.httpClient.get(this.changeStatusUrl + '?id=' + companyAddressId + '&isActive=' + IsActive);
        }
      }]);

      return AddressService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"]);

    AddressService.ɵfac = function AddressService_Factory(t) {
      return new (t || AddressService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    AddressService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: AddressService,
      factory: AddressService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AddressService, [{
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
  "./src/app/company-addresses/company-addresses.component.ts": function srcAppCompanyAddressesCompanyAddressesComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CompanyAddressesComponent", function () {
      return CompanyAddressesComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _address_address_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./address/address.component */
    "./src/app/company-addresses/address/address.component.ts");

    var CompanyAddressesComponent = /*#__PURE__*/function () {
      function CompanyAddressesComponent() {
        _classCallCheck(this, CompanyAddressesComponent);
      }

      _createClass(CompanyAddressesComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }]);

      return CompanyAddressesComponent;
    }();

    CompanyAddressesComponent.ɵfac = function CompanyAddressesComponent_Factory(t) {
      return new (t || CompanyAddressesComponent)();
    };

    CompanyAddressesComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: CompanyAddressesComponent,
      selectors: [["app-company-addresses"]],
      decls: 1,
      vars: 0,
      template: function CompanyAddressesComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "address-component");
        }
      },
      directives: [_address_address_component__WEBPACK_IMPORTED_MODULE_1__["AddressComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbXBhbnktYWRkcmVzc2VzL2NvbXBhbnktYWRkcmVzc2VzLmNvbXBvbmVudC5jc3MifQ== */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CompanyAddressesComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-company-addresses',
          templateUrl: './company-addresses.component.html',
          styleUrls: ['./company-addresses.component.css']
        }]
      }], function () {
        return [];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/lazy-loading/company-addresses-routing.module.ts": function srcAppCompanyAddressesLazyLoadingCompanyAddressesRoutingModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CompanyAddressesRoutingModule", function () {
      return CompanyAddressesRoutingModule;
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


    var _company_addresses_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../company-addresses.component */
    "./src/app/company-addresses/company-addresses.component.ts");
    /* harmony import */


    var _region_region_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../region/region.component */
    "./src/app/company-addresses/region/region.component.ts");

    var routeComAddress = [{
      path: 'CompanyAddresses',
      component: _company_addresses_component__WEBPACK_IMPORTED_MODULE_2__["CompanyAddressesComponent"],
      data: {
        title: 'Company Addresses'
      }
    }, {
      path: 'View',
      component: _region_region_component__WEBPACK_IMPORTED_MODULE_3__["RegionComponent"],
      data: {
        title: 'Region'
      }
    }, {
      path: 'CompanyDetails',
      component: _company_addresses_component__WEBPACK_IMPORTED_MODULE_2__["CompanyAddressesComponent"],
      data: {
        title: 'Company Addresses'
      }
    }];

    var CompanyAddressesRoutingModule = function CompanyAddressesRoutingModule() {
      _classCallCheck(this, CompanyAddressesRoutingModule);
    };

    CompanyAddressesRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: CompanyAddressesRoutingModule
    });
    CompanyAddressesRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function CompanyAddressesRoutingModule_Factory(t) {
        return new (t || CompanyAddressesRoutingModule)();
      },
      imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeComAddress)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](CompanyAddressesRoutingModule, {
        imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CompanyAddressesRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routeComAddress)],
          exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/lazy-loading/company-addresses.module.ts": function srcAppCompanyAddressesLazyLoadingCompanyAddressesModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CompanyAddressesModule", function () {
      return CompanyAddressesModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _company_addresses_routing_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./company-addresses-routing.module */
    "./src/app/company-addresses/lazy-loading/company-addresses-routing.module.ts");
    /* harmony import */


    var _company_addresses_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../company-addresses.component */
    "./src/app/company-addresses/company-addresses.component.ts");
    /* harmony import */


    var _address_address_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../address/address.component */
    "./src/app/company-addresses/address/address.component.ts");
    /* harmony import */


    var src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var _region_region_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../region/region.component */
    "./src/app/company-addresses/region/region.component.ts");
    /* harmony import */


    var _region_create_region_create_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! ../region/create/region-create.component */
    "./src/app/company-addresses/region/create/region-create.component.ts");
    /* harmony import */


    var src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! src/app/modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var _region_source_region_source_region_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../region/source-region/source-region.component */
    "./src/app/company-addresses/region/source-region/source-region.component.ts");
    /* harmony import */


    var _region_dispatch_region_dispatch_region_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../region/dispatch-region/dispatch-region.component */
    "./src/app/company-addresses/region/dispatch-region/dispatch-region.component.ts");
    /* harmony import */


    var _ng_select_ng_select__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(
    /*! @ng-select/ng-select */
    "./node_modules/@ng-select/ng-select/__ivy_ngcc__/fesm2015/ng-select-ng-select.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");

    var CompanyAddressesModule = function CompanyAddressesModule() {
      _classCallCheck(this, CompanyAddressesModule);
    };

    CompanyAddressesModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: CompanyAddressesModule
    });
    CompanyAddressesModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function CompanyAddressesModule_Factory(t) {
        return new (t || CompanyAddressesModule)();
      },
      imports: [[_company_addresses_routing_module__WEBPACK_IMPORTED_MODULE_1__["CompanyAddressesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"], _ng_select_ng_select__WEBPACK_IMPORTED_MODULE_10__["NgSelectModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["ReactiveFormsModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_7__["DirectiveModule"]]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](CompanyAddressesModule, {
        declarations: [_company_addresses_component__WEBPACK_IMPORTED_MODULE_2__["CompanyAddressesComponent"], _address_address_component__WEBPACK_IMPORTED_MODULE_3__["AddressComponent"], _region_region_component__WEBPACK_IMPORTED_MODULE_5__["RegionComponent"], _region_create_region_create_component__WEBPACK_IMPORTED_MODULE_6__["RegionCreateComponent"], _region_source_region_source_region_component__WEBPACK_IMPORTED_MODULE_8__["SourceRegionComponent"], _region_dispatch_region_dispatch_region_component__WEBPACK_IMPORTED_MODULE_9__["DispatchRegionComponent"]],
        imports: [_company_addresses_routing_module__WEBPACK_IMPORTED_MODULE_1__["CompanyAddressesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"], _ng_select_ng_select__WEBPACK_IMPORTED_MODULE_10__["NgSelectModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["ReactiveFormsModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_7__["DirectiveModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CompanyAddressesModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_company_addresses_component__WEBPACK_IMPORTED_MODULE_2__["CompanyAddressesComponent"], _address_address_component__WEBPACK_IMPORTED_MODULE_3__["AddressComponent"], _region_region_component__WEBPACK_IMPORTED_MODULE_5__["RegionComponent"], _region_create_region_create_component__WEBPACK_IMPORTED_MODULE_6__["RegionCreateComponent"], _region_source_region_source_region_component__WEBPACK_IMPORTED_MODULE_8__["SourceRegionComponent"], _region_dispatch_region_dispatch_region_component__WEBPACK_IMPORTED_MODULE_9__["DispatchRegionComponent"]],
          imports: [_company_addresses_routing_module__WEBPACK_IMPORTED_MODULE_1__["CompanyAddressesRoutingModule"], src_app_modules_shared_module__WEBPACK_IMPORTED_MODULE_4__["SharedModule"], _ng_select_ng_select__WEBPACK_IMPORTED_MODULE_10__["NgSelectModule"], _angular_forms__WEBPACK_IMPORTED_MODULE_11__["ReactiveFormsModule"], src_app_modules_directive_module__WEBPACK_IMPORTED_MODULE_7__["DirectiveModule"]]
        }]
      }], null, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/create/region-create.component.ts": function srcAppCompanyAddressesRegionCreateRegionCreateComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionCreateComponent", function () {
      return RegionCreateComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");

    var RegionCreateComponent = /*#__PURE__*/function () {
      function RegionCreateComponent() {
        _classCallCheck(this, RegionCreateComponent);
      }

      _createClass(RegionCreateComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }]);

      return RegionCreateComponent;
    }();

    RegionCreateComponent.ɵfac = function RegionCreateComponent_Factory(t) {
      return new (t || RegionCreateComponent)();
    };

    RegionCreateComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: RegionCreateComponent,
      selectors: [["app-region-create"]],
      decls: 0,
      vars: 0,
      template: function RegionCreateComponent_Template(rf, ctx) {},
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbXBhbnktYWRkcmVzc2VzL3JlZ2lvbi9jcmVhdGUvcmVnaW9uLWNyZWF0ZS5jb21wb25lbnQuY3NzIn0= */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionCreateComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-region-create',
          templateUrl: './region-create.component.html',
          styleUrls: ['./region-create.component.css']
        }]
      }], function () {
        return [];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/dispatch-region/dispatch-region.component.ts": function srcAppCompanyAddressesRegionDispatchRegionDispatchRegionComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DispatchRegionComponent", function () {
      return DispatchRegionComponent;
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


    var _model_region__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../model/region */
    "./src/app/company-addresses/region/model/region.ts");
    /* harmony import */


    var _service_region_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var src_app_statelist_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/statelist.service */
    "./src/app/statelist.service.ts");
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


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ../../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");

    function DispatchRegionComponent_h3_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Edit Dispatch Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_ng_template_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Create Dispatch Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_19_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Name is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_div_19_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.get("Name").errors.required);
      }
    }

    function DispatchRegionComponent_div_27_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Default Load Time is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_27_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_div_27_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r4.rcForm.get("SlotPeriod").errors.required);
      }
    }

    function DispatchRegionComponent_div_47_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Dispatcher is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_47_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_div_47_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.rcForm.get("Dispatchers").errors.required);
      }
    }

    function DispatchRegionComponent_div_61_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " State is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_61_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_div_61_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.rcForm.get("States").errors.required);
      }
    }

    function DispatchRegionComponent_div_81_Template(rf, ctx) {
      if (rf & 1) {
        var _r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "input", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function DispatchRegionComponent_div_81_Template_input_change_2_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r25);

          var ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r24.clearAllProducts();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "label", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Product Family/Group");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "input", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function DispatchRegionComponent_div_81_Template_input_change_6_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r25);

          var ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r26.clearAllProducts();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Specific Products");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FavProductTypeId");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "FavProductTypeId");
      }
    }

    function DispatchRegionComponent_div_82_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "ng-multiselect-dropdown", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select ProductFamily")("settings", ctx_r27.multiselectSettingsById)("data", ctx_r27.ProductTypeList);
      }
    }

    function DispatchRegionComponent_div_82_div_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 9);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "ng-multiselect-dropdown", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Product(s)")("settings", ctx_r28.multiselectSettingsById)("data", ctx_r28.FuelTypeList);
      }
    }

    function DispatchRegionComponent_div_82_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, DispatchRegionComponent_div_82_div_2_Template, 2, 3, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DispatchRegionComponent_div_82_div_3_Template, 2, 3, "div", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.controls["FavProductTypeId"].value == 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r9.rcForm.controls["FavProductTypeId"].value == 2);
      }
    }

    function DispatchRegionComponent_div_88_option_10_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 86);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var rgn_r33 = ctx.$implicit;

        var carr_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", rgn_r33.Id)("selected", rgn_r33.Id == carr_r29.get("RegionId").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", rgn_r33.Name, " ");
      }
    }

    function DispatchRegionComponent_div_88_label_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 87);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_88_Template(rf, ctx) {
      if (rf & 1) {
        var _r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 76);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 77);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "span", 78);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 79);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "select", 80);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "option", 81);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Select Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, DispatchRegionComponent_div_88_option_10_Template, 2, 3, "option", 82);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, DispatchRegionComponent_div_88_label_12_Template, 2, 0, "label", 83);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 84);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "a");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "i", 85);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_88_Template_i_click_15_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r36);

          var carr_r29 = ctx.$implicit;

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r35.removeCarrier(carr_r29.get("Id").value);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carr_r29 = ctx.$implicit;
        var j_r30 = ctx.index;

        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        var tmp_5_0 = null;
        var currVal_5 = (ctx_r10.formSubmitted || ((tmp_5_0 = carr_r29.get("RegionId")) == null ? null : tmp_5_0.touched)) && ((tmp_5_0 = carr_r29.get("RegionId")) == null ? null : tmp_5_0.errors == null ? null : tmp_5_0.errors.required);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpropertyInterpolate"]("id", carr_r29.get("Id").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", j_r30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](carr_r29.get("Name").value);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", null);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r10.getRegionsByCarrierId(carr_r29.get("Id").value));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", currVal_5);
      }
    }

    function DispatchRegionComponent_ng_container_93_div_5_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_ng_container_93_div_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_ng_container_93_div_5_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index;

        var ctx_r39 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r39.isRequired("Name", i_r38));
      }
    }

    function DispatchRegionComponent_ng_container_93_div_8_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_ng_container_93_div_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_ng_container_93_div_8_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index;

        var ctx_r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r40.isRequired("StartTime", i_r38));
      }
    }

    function DispatchRegionComponent_ng_container_93_div_11_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_ng_container_93_div_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_ng_container_93_div_11_div_1_Template, 2, 0, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var i_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().index;

        var ctx_r41 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r41.isRequired("EndTime", i_r38));
      }
    }

    function DispatchRegionComponent_ng_container_93_Template(rf, ctx) {
      if (rf & 1) {
        var _r49 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 88);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 89);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "input", 90);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 91);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DispatchRegionComponent_ng_container_93_div_5_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "input", 93);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onTimeChange", function DispatchRegionComponent_ng_container_93_Template_input_onTimeChange_7_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r49);

          var shift_r37 = ctx.$implicit;
          return shift_r37.get("StartTime").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, DispatchRegionComponent_ng_container_93_div_8_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 92);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "input", 94);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onTimeChange", function DispatchRegionComponent_ng_container_93_Template_input_onTimeChange_10_listener($event) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r49);

          var shift_r37 = ctx.$implicit;
          return shift_r37.get("EndTime").setValue($event);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, DispatchRegionComponent_ng_container_93_div_11_Template, 2, 1, "div", 14);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 95);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "a", 96);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_ng_container_93_Template_a_click_13_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r49);

          var i_r38 = ctx.index;

          var ctx_r51 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r51.removeShift(i_r38);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
      }

      if (rf & 2) {
        var i_r38 = ctx.index;

        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroupName", i_r38);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.isInvalid("Name", i_r38));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "hh:mm A");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.isInvalid("StartTime", i_r38));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("format", "hh:mm A");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.isInvalid("EndTime", i_r38));
      }
    }

    function DispatchRegionComponent_button_99_Template(rf, ctx) {
      if (rf & 1) {
        var _r53 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "button", 97);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_button_99_Template_button_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r53);

          var ctx_r52 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r52._toggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Cancel");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_button_100_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "button", 98);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Save");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_101_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r56 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 100);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 101);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_101_div_1_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r56);

          var ctx_r55 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          ctx_r55._toggleOpened(true);

          return ctx_r55._getJobsForRegion(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 102);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span", 103);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_101_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DispatchRegionComponent_div_101_div_1_Template, 5, 0, "div", 99);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r14.isSalesUser);
      }
    }

    function DispatchRegionComponent_div_103_a_4_Template(rf, ctx) {
      if (rf & 1) {
        var _r59 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 108);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_103_a_4_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r59);

          var ctx_r58 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          ctx_r58._toggleOpened(true);

          return ctx_r58._getJobsForRegion(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 109);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 110);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_103_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 104);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 105);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h1", 106);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, " No Region Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, DispatchRegionComponent_div_103_a_4_Template, 4, 0, "a", 107);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r15.isSalesUser);
      }
    }

    function DispatchRegionComponent_div_106_a_7_Template(rf, ctx) {
      if (rf & 1) {
        var _r69 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_106_a_7_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r69);

          var region_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r67 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          ctx_r67.editRegion(region_r60);
          return ctx_r67._getJobsForRegion(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 121);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_106_a_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r72 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_106_a_8_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r72);

          var region_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r70 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          ctx_r70.editRegion(region_r60);
          return ctx_r70._getJobsForRegion(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 122);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_106_div_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r75 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 123);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 124);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_106_div_9_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r75);

          var region_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r73 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r73.setRegionIdToDelete(region_r60.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 125);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_106_span_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var state_r76 = ctx.$implicit;
        var isLast_r77 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", state_r76.Name, "", isLast_r77 ? "" : ", ", " ");
      }
    }

    function DispatchRegionComponent_div_106_div_21_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carrier_r79 = ctx.$implicit;
        var isLast_r80 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", carrier_r79.Name, "", isLast_r80 ? "" : ", ", " ");
      }
    }

    function DispatchRegionComponent_div_106_div_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "small", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Carriers");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, DispatchRegionComponent_div_106_div_21_span_3_Template, 2, 2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r60 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r60.Carriers);
      }
    }

    function DispatchRegionComponent_div_106_span_25_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var shift_r82 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate3"]("", shift_r82.Name, " ", shift_r82.StartTime, " - ", shift_r82.EndTime, "");
      }
    }

    function DispatchRegionComponent_div_106_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 111);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 112);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 113);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 114);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 115);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, DispatchRegionComponent_div_106_a_7_Template, 2, 0, "a", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, DispatchRegionComponent_div_106_a_8_Template, 2, 0, "a", 116);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, DispatchRegionComponent_div_106_div_9_Template, 3, 0, "div", 117);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 30);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 118);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "small", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Description");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "small", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "States");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, DispatchRegionComponent_div_106_span_20_Template, 2, 2, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, DispatchRegionComponent_div_106_div_21_Template, 4, 1, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 120);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "small", 119);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Shifts");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](25, DispatchRegionComponent_div_106_span_25_Template, 4, 3, "span", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r60 = ctx.$implicit;

        var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](region_r60.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("title", !ctx_r16.isSalesUser ? "edit region" : "view region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r16.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r16.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r16.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](region_r60.Description);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r60.States);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", region_r60.Carriers != null && region_r60.Carriers.length > 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r60.Shifts);
      }
    }

    function DispatchRegionComponent_div_117_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 126);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 127);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 128);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 129);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Please Wait...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function DispatchRegionComponent_div_121_Template(rf, ctx) {
      if (rf & 1) {
        var _r84 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Are you sure you want to remove ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "from the list?");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "br");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " Orders and DR's for ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "strong");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Above ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "will not be visible ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "button", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_121_Template_button_click_12_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r84);

          var ctx_r83 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r83.onSubmit();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Yes");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "button", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_121_Template_button_click_14_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r84);

          var ctx_r85 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r85.resetProductType();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "No");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("", ctx_r18.removedProductNameString, " ");
      }
    }

    function DispatchRegionComponent_div_122_Template(rf, ctx) {
      if (rf & 1) {
        var _r87 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " DR's for this product are already scheduled in load queue so product can't be removed. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "button", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_div_122_Template_button_click_4_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r87);

          var ctx_r86 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r86.resetProductType();
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Ok");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var DispatchRegionComponent = /*#__PURE__*/function (_model_region__WEBPAC) {
      _inherits(DispatchRegionComponent, _model_region__WEBPAC);

      var _super2 = _createSuper(DispatchRegionComponent);

      function DispatchRegionComponent(fb, regionService, stateService) {
        var _this25;

        _classCallCheck(this, DispatchRegionComponent);

        _this25 = _super2.call(this);
        _this25.fb = fb;
        _this25.regionService = regionService;
        _this25.stateService = stateService;
        _this25._opened = false;
        _this25._animate = true;
        _this25._positionNum = 1;
        _this25._POSITIONS = ['left', 'right', 'top', 'bottom'];
        _this25.IsUpdate = false;
        _this25.SelectedRegionToDelete = null;
        _this25.IsLoading = true;
        _this25.IsEmpty = false; //public IsSuccess: boolean = false;
        //carrier with region sequencing

        _this25.CarrierRegions = [];
        _this25.carrierDdlSetting = {};
        _this25.formSubmitted = false;
        _this25.isSalesUser = false;
        _this25.ProductTypeList = [];
        _this25.FuelTypeList = [];
        _this25.selectedProdTypeList = [];
        _this25.removedProductNameString = '';
        _this25.IsPublishedDR = false;
        _this25.PastProduct = {};
        return _this25;
      }

      _createClass(DispatchRegionComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.multiselectSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.carrierDdlSetting = {
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
          };
          this.getRegions();
          this.fillDropdowns();
          this.rcForm = this.createForm();
          this.makeCarrierUIsortable();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (typeof isSalesUser !== 'undefined' && isSalesUser) {
            this.isSalesUser = isSalesUser;
          }
        }
      }, {
        key: "createForm",
        value: function createForm() {
          if (this.region == undefined || this.region == null) this.region = new _model_region__WEBPACK_IMPORTED_MODULE_3__["Region"]();
          return this.fb.group({
            Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Name, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            SlotPeriod: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.DefaultSlotPeriod, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Description: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Description),
            Jobs: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Jobs),
            Drivers: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Drivers),
            Dispatchers: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Dispatchers, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Trailers: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Trailers),
            States: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.States, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArray"]([]),
            SelectedCarrier: this.fb.control(null),
            CreatedOn: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            Shifts: this.fb.array([]),
            ProductTypeIds: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.ProductTypeIds),
            FuelTypeIds: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.FuelTypeIds),
            IsSelectAllProductTypes: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](false),
            FormProductTypeIds: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]([]),
            FavProductTypeId: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.FavProductTypeId.toString()),
            IsAddFavoriteProduct: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](false)
          });
        }
      }, {
        key: "clearShift",
        value: function clearShift() {
          var shifts = this.rcForm.controls.Shifts;
          shifts.clear();
        }
      }, {
        key: "clearCarriers",
        value: function clearCarriers() {
          var carriers = this.rcForm.controls.Carriers;
          carriers.clear();
        }
      }, {
        key: "addShift",
        value: function addShift() {
          var shifts = this.rcForm.controls.Shifts;

          if (shifts.length == 0) {
            shifts.push(this.fb.group({
              Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
              Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('Morning', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              StartTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('02:00 AM', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              EndTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('04:00 PM', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required])
            }));
            shifts.push(this.fb.group({
              Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
              Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('Evening', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              StartTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('02:00 PM', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              EndTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('04:00 AM', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required])
            }));
          } else {
            shifts.push(this.fb.group({
              Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
              Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              StartTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              EndTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"]('', [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required])
            }));
          }
        }
      }, {
        key: "editShifts",
        value: function editShifts(_shifts) {
          var formB = this.fb;
          var shifts = this.rcForm.controls.Shifts;
          shifts.clear();

          _shifts.forEach(function (shift, idx) {
            shifts.push(formB.group({
              Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](shift.Id),
              Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](shift.Name, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              StartTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](shift.StartTime, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
              EndTime: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](shift.EndTime, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required])
            }));
          });
        }
      }, {
        key: "removeShift",
        value: function removeShift(i) {
          var shifts = this.rcForm.get('Shifts');
          shifts.removeAt(i);
        }
      }, {
        key: "isInvalid",
        value: function isInvalid(name, i) {
          var shifts = this.getShifts();
          var result = shifts.controls[i].get(name).invalid && (shifts.controls[i].get(name).dirty || shifts.controls[i].get(name).touched);
          return result;
        }
      }, {
        key: "isRequired",
        value: function isRequired(name, i) {
          var shifts = this.getShifts();
          return shifts.controls[i].get(name).errors.required;
        }
      }, {
        key: "getShifts",
        value: function getShifts() {
          return this.rcForm.get('Shifts');
        }
      }, {
        key: "editRegion",
        value: function editRegion(_region) {
          var _this26 = this;

          this.region = _region;
          this.formSubmitted = false;

          this._toggleOpened(true, this.region.Id);

          this.IsUpdate = true;

          if (this.rcForm) {
            this.rcForm.controls['Id'].setValue(this.region.Id);
            this.rcForm.controls['Name'].setValue(this.region.Name);
            this.rcForm.controls['Description'].setValue(this.region.Description);
            this.rcForm.controls['SlotPeriod'].setValue(this.region.SlotPeriod);
            this.rcForm.controls['Jobs'].setValue(this.region.Jobs);
            this.rcForm.controls['Drivers'].setValue(this.region.Drivers);
            this.rcForm.controls['Dispatchers'].setValue(this.region.Dispatchers);
            this.rcForm.controls['Trailers'].setValue(this.region.Trailers);
            this.rcForm.controls['States'].setValue(this.region.States);
            this.setCarrierRegions(this.region.Carriers);

            if (this.region.ProductTypeIds.length > 0 || this.region.FuelTypeIds.length > 0) {
              this.rcForm.controls['IsAddFavoriteProduct'].setValue(true);
              this.rcForm.controls['FavProductTypeId'].setValue(this.region.FavProductTypeId.toString());

              if (this.rcForm.controls['FavProductTypeId'].value == 1) {
                this.rcForm.controls['ProductTypeIds'].setValue(this.region.ProductTypeIds);
                var formProductTypes = [];
                if (this.region.ProductTypeIds) formProductTypes = this.ProductTypeList.filter(function (x) {
                  return _this26.region.ProductTypeIds.indexOf(x.Id) > -1;
                });
                this.rcForm.controls['FormProductTypeIds'].setValue(formProductTypes);
              } else if (this.rcForm.controls['FavProductTypeId'].value == 2) {
                this.rcForm.controls['FuelTypeIds'].setValue(this.region.FuelTypeIds);
              }
            } else {
              this.rcForm.controls['IsAddFavoriteProduct'].patchValue(false);
            }

            this.editShifts(_region.Shifts);
          }
        }
      }, {
        key: "makeCarrierUIsortable",
        value: function makeCarrierUIsortable() {
          var _this = this;

          $(function () {
            var sortable = $("#sortableRegionCarriers");
            sortable.sortable({
              stop: function stop(event, ui) {
                var carrierIds = sortable.sortable("toArray");

                _this.updateSequence(carrierIds);

                sortable.click();
              }
            });
          });
        }
      }, {
        key: "updateSequence",
        value: function updateSequence(carrIds) {
          var _this27 = this;

          var _formArray = this.rcForm.controls['Carriers'];
          var carriers = _formArray.value;

          _formArray.clear();

          carrIds.forEach(function (id) {
            var carr = carriers.find(function (c) {
              return c.Id == id;
            });

            _this27.pushRowInCarrierForm(_formArray, carr);
          });
        }
      }, {
        key: "pushRowInCarrierForm",
        value: function pushRowInCarrierForm(_formArray, data) {
          _formArray.push(this.fb.group({
            Id: this.fb.control(data.Id),
            Name: this.fb.control(data.Name),
            RegionId: this.fb.control(this.CarrierRegions.some(function (f) {
              return f.Regions.some(function (r) {
                return r.Id == data.RegionId;
              });
            }) ? data.RegionId : null, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            SequenceNo: this.fb.control(data.SequenceNo)
          }));
        }
      }, {
        key: "setCarrierRegions",
        value: function setCarrierRegions(_carrRegions) {
          var _this28 = this;

          var _formArray = this.rcForm.controls['Carriers'];

          _formArray.clear();

          _carrRegions.forEach(function (carr) {
            _this28.pushRowInCarrierForm(_formArray, carr);
          });

          this.rcForm.get('SelectedCarrier').patchValue(_formArray.value);
        }
      }, {
        key: "removeCarrier",
        value: function removeCarrier(id) {
          //remove from form
          var _formArray = this.rcForm.controls['Carriers'];

          _formArray.removeAt(_formArray.value.findIndex(function (carr) {
            return carr.Id == id;
          })); //remove from dropdown


          var currentSelection = this.rcForm.get('SelectedCarrier').value || [];
          currentSelection.splice(currentSelection.findIndex(function (carr) {
            return carr.Id == id;
          }), 1);
          this.rcForm.get('SelectedCarrier').patchValue(currentSelection);
        }
      }, {
        key: "getRegionsByCarrierId",
        value: function getRegionsByCarrierId(id) {
          var response = [];

          if (this.CarrierRegions) {
            var carr = this.CarrierRegions.find(function (f) {
              return f.Id == id;
            });
            if (carr != null && carr.Regions) response = carr.Regions;
          }

          return response;
        }
      }, {
        key: "onCarrierSelect",
        value: function onCarrierSelect(item, isSelect) {
          if (isSelect) {
            var selection = this.CarrierRegions.find(function (c) {
              return c.Id == item.Id;
            });

            if (selection) {
              var _formArray = this.rcForm.controls['Carriers'];
              this.pushRowInCarrierForm(_formArray, {
                Id: selection.Id,
                Code: null,
                Name: selection.Name,
                RegionId: null,
                SequenceNo: null
              });
            }
          } else {
            var _formArray2 = this.rcForm.controls['Carriers'];

            _formArray2.removeAt(_formArray2.value.findIndex(function (carr) {
              return carr.Id == item.Id;
            }));
          }
        }
      }, {
        key: "onCarrierSelectAll",
        value: function onCarrierSelectAll(items, isSelectAll) {
          var _this29 = this;

          var _formArray = this.rcForm.controls['Carriers'];

          if (isSelectAll) {
            var existingFormCarriers = _formArray.value || [];
            this.CarrierRegions.forEach(function (carrierRegion) {
              if (!existingFormCarriers.some(function (c) {
                return c.Id == carrierRegion.Id;
              })) {
                var _formArray3 = _this29.rcForm.controls['Carriers'];

                _this29.pushRowInCarrierForm(_formArray3, {
                  Id: carrierRegion.Id,
                  Code: null,
                  Name: carrierRegion.Name,
                  RegionId: null,
                  SequenceNo: null
                });
              }
            });
          } else {
            _formArray.clear();
          }
        }
      }, {
        key: "setRegionIdToDelete",
        value: function setRegionIdToDelete(id) {
          this.SelectedRegionToDelete = id;
        }
      }, {
        key: "deleteRegion",
        value: function deleteRegion() {
          var _this30 = this;

          if (this.SelectedRegionToDelete == null) {
            return;
          }

          this.IsLoading = true;
          this.regionService.deleteRegion(this.SelectedRegionToDelete).subscribe(function (response) {
            _this30.IsLoading = false;

            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess('Deleted successfully', undefined, undefined);

              _this30.getRegions();
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
          this.SelectedRegionToDelete == null;
        }
      }, {
        key: "fillDropdowns",
        value: function fillDropdowns() {
          this.getJobs();
          this.getDrivers("");
          this.getDispatchers();
          this.getTrailers();
          this.getCarriers();
          this.getCarrierRegions();
          this.getProductType();
          this.getProducts();
        }
      }, {
        key: "getJobs",
        value: function getJobs() {
          var _this31 = this;

          this.IsLoading = true;
          this.regionService.getJobs().subscribe(function (jobs) {
            _this31.JobList = jobs;
            _this31.IsLoading = false;
          });
        }
      }, {
        key: "getDrivers",
        value: function getDrivers(regionId) {
          var _this32 = this;

          this.IsLoading = true;
          this.regionService.getRegionDrivers(regionId).subscribe(function (drivers) {
            _this32.DriverList = drivers;
            _this32.IsLoading = false;
          });
        }
      }, {
        key: "getDispatchers",
        value: function getDispatchers() {
          var _this33 = this;

          this.IsLoading = true;
          this.regionService.getDispatchers().subscribe(function (dispatchers) {
            _this33.DispatcherList = dispatchers;
            _this33.IsLoading = false;
          });
        }
      }, {
        key: "getTrailers",
        value: function getTrailers() {
          var _this34 = this;

          this.IsLoading = true;
          this.regionService.getTrailers().subscribe(function (trailers) {
            _this34.TrailerList = trailers;
            _this34.IsLoading = false;
          });
        }
      }, {
        key: "getStates",
        value: function getStates() {
          var _this35 = this;

          this.IsLoading = true;
          this.regionService.getStates(this.CountryId).subscribe(function (states) {
            _this35.StateList = states;
            _this35.IsLoading = false;
          });
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this36 = this;

          this.IsLoading = true;
          this.regionService.getRegions().subscribe(function (response) {
            _this36.CurrentUserId = response.UserId;
            _this36.CurrentCompanyId = response.CompanyId;
            _this36.CountryId = response.CountryId;
            _this36.DefaultSlotPeriod = response.DefaultSlotPeriod;

            _this36.rcForm.controls['SlotPeriod'].setValue(response.DefaultSlotPeriod);

            _this36.regions = response.Regions;

            if (_this36.regions != null && _this36.regions != undefined && _this36.regions.length == 0) {
              _this36.IsEmpty = true;
            }

            _this36.IsLoading = false;

            _this36.getStates();
          });
        }
      }, {
        key: "validateAndSubmitForm",
        value: function validateAndSubmitForm() {
          var _this37 = this;

          this.formSubmitted = true;
          var selectedProductType = this.rcForm.controls["FavProductTypeId"].value;

          if (selectedProductType == 1) {
            var newProducts = this.rcForm.get('FormProductTypeIds').value || [];

            if (newProducts && newProducts.length > 0) {
              this.rcForm.get('ProductTypeIds').patchValue(newProducts.map(function (x) {
                return x.Id;
              }));
            } else {
              this.rcForm.get('ProductTypeIds').patchValue([]);
            }
          } else if (selectedProductType == 2) {
            var _newProducts = this.rcForm.get('FuelTypeIds').value || [];

            var filterProducts = this.FuelTypeList.filter(function (t) {
              return _newProducts.some(function (t1) {
                return t1.Id == t.Id;
              });
            });
            if (filterProducts && filterProducts.length > 0) this.rcForm.get('FuelTypeIds').patchValue(filterProducts);else this.rcForm.get('FuelTypeIds').patchValue([]);
          } // pop up for removed validate product types


          var submitWithConfirmation = false;

          if (this.IsUpdate) {
            this.PastProduct = {
              FavProductTypeId: this.region.FavProductTypeId.toString(),
              ProductTypeIds: this.region.ProductTypeIds,
              FuelTypeIds: this.region.FuelTypeIds
            };
            var removedList = this.getRemovedList();

            if (removedList && removedList.length > 0) {
              this.removedProductNameString = removedList.map(function (t) {
                return t.Name;
              }).join(",");
              this.IsLoading = true;
              var productIds = +selectedProductType == 1 ? removedList.map(function (t) {
                return t.Id;
              }).join(",") : "";
              var fuelTypeIds = +selectedProductType == 2 ? removedList.map(function (t) {
                return t.Id;
              }).join(",") : "";
              this.regionService.isPublishedDR(productIds, fuelTypeIds).subscribe(function (response) {
                _this37.IsLoading = false;
                var elem = document.getElementById('openConfirmationModel01');
                elem.click();
                _this37.IsPublishedDR = response;
              });
              this.formSubmitted = false;
              submitWithConfirmation = true;
              return;
            }
          }

          if (!submitWithConfirmation) {
            this.onSubmit();
          }
        }
      }, {
        key: "getRemovedList",
        value: function getRemovedList() {
          var _this38 = this;

          var rList = [];
          var selectedProductType = this.rcForm.controls["FavProductTypeId"].value;
          var newProducts = this.rcForm.get('ProductTypeIds').value || [];
          var newFuelTypes = this.rcForm.get('FuelTypeIds').value || [];

          if (selectedProductType == 1 && newProducts && newProducts.length > 0) {
            // ProductType
            if (this.region.FavProductTypeId == 2) {
              return this.region.FuelTypeIds;
            } else {
              // filter different productTypes
              var removedProductTypeIds = this.region.ProductTypeIds.filter(function (o1) {
                return !newProducts.some(function (o2) {
                  return o1 == o2;
                });
              }) || [];

              if (removedProductTypeIds && removedProductTypeIds.length > 0) {
                return this.ProductTypeList.filter(function (x) {
                  return removedProductTypeIds.includes(x.Id);
                });
              }
            }
          } else if (selectedProductType == 2 && newFuelTypes && newFuelTypes.length > 0) {
            if (this.region.FavProductTypeId == 1) {
              return this.ProductTypeList.filter(function (t) {
                return _this38.region.ProductTypeIds.includes(t.Id);
              });
            } else {
              // filter different productTypes
              return this.region.FuelTypeIds.filter(function (o1) {
                return !newFuelTypes.some(function (o2) {
                  return o1.Id == o2.Id;
                });
              }) || [];
            }
          }

          return rList;
        }
      }, {
        key: "resetProductType",
        value: function resetProductType() {
          this.setPastValues();
          src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess('No product have been removed from the region.', undefined, undefined);
          this.removedProductNameString = '';
        }
      }, {
        key: "toggleFavProducts",
        value: function toggleFavProducts(event) {
          this.clearAllProducts();
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          this.region = this.rcForm.value;

          if (this.region.Carriers && this.region.Carriers.length > 0) {
            this.region.Carriers.forEach(function (carr, index) {
              carr.SequenceNo = index + 1;
            });
          }

          if (this.rcForm.valid) {
            if (this.IsUpdate) {
              this.updateRegion();
            } else {
              this.createRegion();
            }
          } else {
            this.rcForm.markAllAsTouched();
          }
        }
      }, {
        key: "IdInComparer",
        value: function IdInComparer(otherArray) {
          return function (current) {
            return otherArray.filter(function (other) {
              return other == current.Id;
            }).length == 1;
          };
        }
      }, {
        key: "createRegion",
        value: function createRegion() {
          var _this39 = this;

          this.IsLoading = true;
          this.regionService.createRegion(this.region).subscribe(function (response) {
            _this39.formSubmitted = false;
            _this39.serviceResponse = response;

            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess('Dispatch Region created successfully', undefined, undefined);
              _this39.IsLoading = false;

              _this39._toggleOpened(false);

              _this39.getRegions();
            } else {
              _this39.IsLoading = false;
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "updateRegion",
        value: function updateRegion() {
          var _this40 = this;

          this.IsLoading = true;
          this.regionService.updateRegion(this.region).subscribe(function (response) {
            _this40.serviceResponse = response;
            _this40.formSubmitted = false;

            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgsuccess('Dispatch Region updated successfully', undefined, undefined);
              _this40.IsLoading = false;

              _this40._toggleOpened(false);

              _this40.getRegions();
            } else {
              _this40.IsLoading = false;
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_2__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          var regionId = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";

          if (shouldOpen) {
            this.getDrivers(regionId);
            this._opened = true;
          } else {
            this._opened = !this._opened;
            this.rcForm.reset();
            this.rcForm.controls['SlotPeriod'].setValue(this.DefaultSlotPeriod);
            this.IsUpdate = false;
            this.clearShift();
            this.clearCarriers();
          }
        }
      }, {
        key: "_getJobsForRegion",
        value: function _getJobsForRegion(IsCreateRegion) {
          var _this41 = this;

          if (IsCreateRegion) {
            this.getJobs();
          } else {
            var currentJobsInRegion = new Array();
            currentJobsInRegion = this.rcForm.controls['Jobs'].value;
            this.regionService.getJobs().subscribe(function (data) {
              if (currentJobsInRegion != null && currentJobsInRegion != undefined && currentJobsInRegion.length > 0) {
                currentJobsInRegion.forEach(function (item) {
                  data.push(item);
                });
              }

              _this41.JobList = data;
            });
          }
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this42 = this;

          this.IsLoading = true;
          this.regionService.getCarriers().subscribe(function (carriers) {
            _this42.CarrierList = carriers;
            _this42.IsLoading = false;
          });
        }
      }, {
        key: "getCarrierRegions",
        value: function getCarrierRegions() {
          var _this43 = this;

          this.IsLoading = true;
          this.regionService.getCarrierRegions().subscribe(function (data) {
            _this43.CarrierRegions = data;
            _this43.IsLoading = false;
          });
        }
      }, {
        key: "getProductType",
        value: function getProductType() {
          var _this44 = this;

          this.IsLoading = true;
          this.regionService.getProductType().subscribe(function (productType) {
            _this44.ProductTypeList = productType;
          });
        }
      }, {
        key: "getProducts",
        value: function getProducts() {
          var _this45 = this;

          this.IsLoading = true;
          this.regionService.getFuelProducts().subscribe(function (products) {
            _this45.FuelTypeList = products;
          });
        }
      }, {
        key: "clearAllProducts",
        value: function clearAllProducts() {
          this.rcForm.controls['FuelTypeIds'].patchValue([]);
          this.rcForm.controls['ProductTypeIds'].patchValue([]);
          this.rcForm.controls['FormProductTypeIds'].patchValue([]);
        }
      }, {
        key: "setPastValues",
        value: function setPastValues() {
          this.rcForm.controls['FavProductTypeId'].patchValue(this.PastProduct['FavProductTypeId'] || '1');
          this.rcForm.controls['FuelTypeIds'].patchValue(this.PastProduct['FuelTypeIds'] || []);
          var selProductTypeIds = this.PastProduct['ProductTypeIds'] || [];
          this.rcForm.controls['ProductTypeIds'].patchValue(this.PastProduct['ProductTypeIds'] || []);
          this.rcForm.controls['FormProductTypeIds'].patchValue(this.ProductTypeList.filter(function (x) {
            return selProductTypeIds.indexOf(x.Id) > -1;
          }));
        }
      }]);

      return DispatchRegionComponent;
    }(_model_region__WEBPACK_IMPORTED_MODULE_3__["Region"]);

    DispatchRegionComponent.ɵfac = function DispatchRegionComponent_Factory(t) {
      return new (t || DispatchRegionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_region_service__WEBPACK_IMPORTED_MODULE_4__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_statelist_service__WEBPACK_IMPORTED_MODULE_5__["StatelistService"]));
    };

    DispatchRegionComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: DispatchRegionComponent,
      selectors: [["app-dispatch-region"]],
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵInheritDefinitionFeature"]],
      decls: 123,
      vars: 45,
      consts: [[2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], ["class", "dib ml10 mt10 mb10", 4, "ngIf", "ngIfElse"], ["editTitle", ""], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Name"], [1, "color-maroon"], ["formControlName", "Id", "type", "hidden", 1, "hide-element"], ["formControlName", "Name", "required", "", 1, "form-control"], ["class", "color-maroon", 4, "ngIf"], ["formControlName", "SlotPeriod", "placeholder", "Hours", "required", "", 1, "form-control"], ["for", "Jobs"], ["formControlName", "Jobs", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Drivers"], ["formControlName", "Drivers", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Dispatchers"], ["formControlName", "Dispatchers", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Trailers"], ["formControlName", "Trailers", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["for", "Description"], ["formControlName", "States", 3, "placeholder", "settings", "data", "ngModel", "ngModelChange"], ["formControlName", "Description", 1, "form-control"], [1, "col-6"], [3, "settings", "data", "formControl", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], ["multiSelect3", ""], [1, "col-sm-12"], [1, "form-group", "form-check"], ["type", "checkbox", "formControlName", "IsAddFavoriteProduct", "id", "ProductTypechk", 1, "form-check-input", 3, "click"], ["for", "ProductTypechk", 1, "form-check-label"], ["class", "mb-2", 4, "ngIf"], ["class", "row", 4, "ngIf"], [1, "mt-2"], ["formArrayName", "Carriers"], ["id", "sortableRegionCarriers", 1, "dragable-pane", "col-12"], ["class", "row mb-2 border-muted rounded py-1", 3, "formGroupName", "id", 4, "ngFor", "ngForOf"], ["for", "Shifts", 1, "mt-3"], ["formArrayName", "Shifts"], [4, "ngFor", "ngForOf"], ["id", "add-shift", 1, "mb15"], [1, "fa", "fa-plus-circle"], [1, "text-right"], ["class", "btn btn-lg", "type", "reset", 3, "click", 4, "ngIf"], ["class", "ml15 btn btn-primary btn-lg", "type", "submit", "data-toggle", "modal", "data-target", "#myModal1", 4, "ngIf"], ["class", "text-center wrapper-nodata", 4, "ngIf"], ["id", "regionDetails", 1, "row", "d-flex", "align-items-stretch"], ["class", "col-sm-3", 4, "ngFor", "ngForOf"], ["id", "myModal", "role", "dialog", "data-backdrop", "static", "data-keyboard", "false", 1, "modal", "fade"], [1, "modal-dialog", 2, "width", "200px"], [1, "modal-content"], [1, "modal-body"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-danger", 3, "click"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", 3, "click"], ["class", "loader", 4, "ngIf"], ["id", "openConfirmationModel01", "data-toggle", "modal", "data-target", "#ConfirmationModel01"], ["id", "ConfirmationModel01", "role", "dialog", "data-backdrop", "static", "data-keyboard", "false", 1, "modal", "fade"], [1, "modal-dialog", 2, "min-width", "180px"], ["class", "modal-content", 4, "ngIf"], [1, "dib", "ml10", "mt10", "mb10"], [4, "ngIf"], [1, "mb-2"], [1, "form-check", "form-check-inline"], ["type", "radio", "id", "radio-producttype", "formControlName", "FavProductTypeId", "value", "1", 1, "form-check-input", 3, "name", "change"], ["for", "radio-producttype", 1, "form-check-label"], ["type", "radio", "id", "radio-product", "formControlName", "FavProductTypeId", "value", "2", 1, "form-check-input", 3, "name", "change"], ["for", "radio-product", 1, "form-check-label"], ["class", "form-group", 4, "ngIf"], ["formControlName", "FormProductTypeIds", 3, "placeholder", "settings", "data"], ["formControlName", "FuelTypeIds", 3, "placeholder", "settings", "data"], [1, "row", "mb-2", "border-muted", "rounded", "py-1", 3, "formGroupName", "id"], [1, "col-sm-1", "d-flex", "align-items-center"], [1, "fas", "fa-grip-vertical", "text-muted"], [1, "col-sm-5", "list-drag", "carr-seq-sortable-cursor", "mb-0", "border-0", "d-flex", "align-items-center"], [1, "float-left"], [1, "col-sm-5"], ["formControlName", "RegionId", 1, "custom-select"], ["hidden", "", 3, "value"], [3, "value", "selected", 4, "ngFor", "ngForOf"], ["class", "fs12", "style", "color:red", 4, "ngIf"], [1, "col-sm-1", "float-left"], ["data-toggle", "tooltip", "data-placement", "right", "title", "Remove", 1, "fa", "fa-trash-alt", "color-maroon", "mt10", 3, "click"], [3, "value", "selected"], [1, "fs12", 2, "color", "red"], [1, "row", 3, "formGroupName"], [1, "col-5", "col-sm-5", "form-group"], ["type", "hidden", "placeholder", "Id", "formControlName", "Id"], ["placeholder", "Shift Name", "formControlName", "Name", 1, "form-control"], [1, "col-3", "col-sm-3", "form-group", "pl0"], ["type", "text", "placeholder", "Start Time", "formControlName", "StartTime", "myTimePicker", "", 1, "form-control", 3, "format", "onTimeChange"], ["type", "text", "placeholder", "End Time", "formControlName", "EndTime", "myTimePicker", "", 1, "form-control", 3, "format", "onTimeChange"], [1, "col-1", "col-sm-1", "form-group", "pl0"], [1, "fa", "fa-trash-alt", "color-maroon", "mt10", 3, "click"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", "data-toggle", "modal", "data-target", "#myModal1", 1, "ml15", "btn", "btn-primary", "btn-lg"], ["class", "col-sm-12 pt10 pb10", 4, "ngIf"], [1, "col-sm-12", "pt10", "pb10"], ["id", "createnewregion", 1, "fs18", "pull-left", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt3", "pull-left"], [1, "fs14", "mt1", "pull-left"], [1, "text-center", "wrapper-nodata"], [1, "fas", "fa-map-marked-alt"], [1, "mb20", "mt10", "f-normal"], ["id", "createnewregion", "class", "btn btn-primary", 3, "click", 4, "ngIf"], ["id", "createnewregion", 1, "btn", "btn-primary", 3, "click"], [1, "fa", "fa-plus", "fs14", "mt5", "pull-left", "mr5"], [1, "fs14", "mt2", "ml5", "pull-left"], [1, "col-sm-3"], [1, "well", "box-shadow", "tile-xs", "animated", "zoomIn"], [1, "col-sm-8", "fs18"], [1, "col-sm-4", "text-right"], [1, "pull-right", 3, "title"], [3, "click", 4, "ngIf"], ["class", "pull-right mr15", "title", "delete region", 4, "ngIf"], [1, "region-data"], [1, "db", "mt10", "color-lightgrey", "f-bold", "fs13"], [1, "mt10"], [1, "fa", "fa-edit", "fs14"], [1, "fa", "fa-eye", "fs14"], ["title", "delete region", 1, "pull-right", "mr15"], ["data-toggle", "modal", "data-target", "#myModal", 3, "click"], [1, "fa", "fa-trash-alt", "color-maroon"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function DispatchRegionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "ng-sidebar", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("openedChange", function DispatchRegionComponent_Template_ng_sidebar_openedChange_2_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_Template_a_click_3_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, DispatchRegionComponent_h3_5_Template, 2, 0, "h3", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, DispatchRegionComponent_ng_template_6_Template, 2, 0, "ng-template", null, 4, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "content", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "form", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function DispatchRegionComponent_Template_form_ngSubmit_9_listener() {
            return ctx.validateAndSubmitForm();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](17, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](18, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, DispatchRegionComponent_div_19_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "label", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](23, "Default Load Time (Hrs)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](26, "input", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](27, DispatchRegionComponent_div_27_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "label", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Locations");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "ng-multiselect-dropdown", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DispatchRegionComponent_Template_ng_multiselect_dropdown_ngModelChange_33_listener($event) {
            return ctx.Jobs = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Drivers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "ng-multiselect-dropdown", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DispatchRegionComponent_Template_ng_multiselect_dropdown_ngModelChange_38_listener($event) {
            return ctx.Drivers = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "label", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](43, "Dispatchers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](45, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "ng-multiselect-dropdown", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DispatchRegionComponent_Template_ng_multiselect_dropdown_ngModelChange_46_listener($event) {
            return ctx.Dispatchers = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](47, DispatchRegionComponent_div_47_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "label", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](51, "Trailers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "ng-multiselect-dropdown", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DispatchRegionComponent_Template_ng_multiselect_dropdown_ngModelChange_52_listener($event) {
            return ctx.Trailers = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "label", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "States");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](59, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "ng-multiselect-dropdown", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DispatchRegionComponent_Template_ng_multiselect_dropdown_ngModelChange_60_listener($event) {
            return ctx.States = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](61, DispatchRegionComponent_div_61_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](62, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "label", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](65, "Description");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](66, "textarea", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](69, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](70, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](71, "Select Carriers");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "ng-multiselect-dropdown", 28, 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function DispatchRegionComponent_Template_ng_multiselect_dropdown_onSelect_73_listener($event) {
            return ctx.onCarrierSelect($event, true);
          })("onDeSelect", function DispatchRegionComponent_Template_ng_multiselect_dropdown_onDeSelect_73_listener($event) {
            return ctx.onCarrierSelect($event, false);
          })("onSelectAll", function DispatchRegionComponent_Template_ng_multiselect_dropdown_onSelectAll_73_listener($event) {
            return ctx.onCarrierSelectAll($event, true);
          })("onDeSelectAll", function DispatchRegionComponent_Template_ng_multiselect_dropdown_onDeSelectAll_73_listener($event) {
            return ctx.onCarrierSelectAll($event, false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](75, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](77, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](78, "input", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_Template_input_click_78_listener($event) {
            return ctx.toggleFavProducts($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](79, "label", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](80, "Add Favorite Products");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](81, DispatchRegionComponent_div_81_Template, 9, 2, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](82, DispatchRegionComponent_div_82_Template, 4, 2, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](83, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](84, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](85, "Carrier Regions");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](86, "div", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](87, "div", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](88, DispatchRegionComponent_div_88_Template, 16, 6, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](89, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "label", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](91, "Shifts");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](92, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](93, DispatchRegionComponent_ng_container_93_Template, 14, 6, "ng-container", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "div", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](95, "a", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_Template_a_click_95_listener() {
            return ctx.addShift();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](96, "i", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](97, " Add Shift");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](98, "div", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](99, DispatchRegionComponent_button_99_Template, 2, 0, "button", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](100, DispatchRegionComponent_button_100_Template, 2, 0, "button", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](101, DispatchRegionComponent_div_101_Template, 2, 1, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](103, DispatchRegionComponent_div_103_Template, 5, 1, "div", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](104, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](105, "div", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](106, DispatchRegionComponent_div_106_Template, 26, 9, "div", 50);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](107, "div", 51);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](108, "div", 52);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](109, "div", 53);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](110, "div", 54);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](111, " Are you sure to delete? ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](112, "div", 55);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](113, "button", 56);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_Template_button_click_113_listener() {
            return ctx.deleteRegion();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](114, "Yes");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](115, "button", 57);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DispatchRegionComponent_Template_button_click_115_listener() {
            return ctx.setRegionIdToDelete(null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](116, "No");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](117, DispatchRegionComponent_div_117_Template, 5, 0, "div", 58);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](118, "div", 59);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](119, "div", 60);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](120, "div", 61);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](121, DispatchRegionComponent_div_121_Template, 16, 1, "div", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](122, DispatchRegionComponent_div_122_Template, 6, 0, "div", 62);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsUpdate)("ngIfElse", _r1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.rcForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("Name").invalid && (ctx.rcForm.get("Name").dirty || ctx.rcForm.get("Name").touched));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("SlotPeriod").invalid && ctx.rcForm.get("SlotPeriod").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Location(s)")("settings", ctx.multiselectSettingsById)("data", ctx.JobList)("ngModel", ctx.Jobs);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Driver(s)")("settings", ctx.multiselectSettingsById)("data", ctx.DriverList)("ngModel", ctx.Drivers);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Dispatcher(s)")("settings", ctx.multiselectSettingsById)("data", ctx.DispatcherList)("ngModel", ctx.Dispatchers);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("Dispatchers").invalid && ctx.rcForm.get("Dispatchers").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Trailer(s)")("settings", ctx.multiselectSettingsByCode)("data", ctx.TrailerList)("ngModel", ctx.Trailers);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select State(s)")("settings", ctx.multiselectSettingsByCode)("data", ctx.StateList)("ngModel", ctx.States);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("States").invalid && ctx.rcForm.get("States").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("settings", ctx.carrierDdlSetting)("data", ctx.CarrierRegions)("formControl", ctx.rcForm.controls["SelectedCarrier"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.controls["IsAddFavoriteProduct"].value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.controls["IsAddFavoriteProduct"].value);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.rcForm.get("Carriers")["controls"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.rcForm.get("Shifts")["controls"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isSalesUser);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isSalesUser);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.regions == null ? null : ctx.regions.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLoading && (!ctx.regions || (ctx.regions == null ? null : ctx.regions.length) == 0));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.regions);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsPublishedDR);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsPublishedDR);
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["Sidebar"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["RequiredValidator"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["CheckboxControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormArrayName"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["RadioControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_9__["TimePicker"]],
      styles: [".ng-select.custom[_ngcontent-%COMP%] {\r\n    border: 0px;\r\n    min-height: 0px;\r\n    border-radius: 0;\r\n}\r\n    .ng-select.custom[_ngcontent-%COMP%]   .ng-select-container[_ngcontent-%COMP%] {\r\n        min-height: 0px;\r\n        border-radius: 0;\r\n    }\r\n    .scroll-height[_ngcontent-%COMP%]{\r\n    height: calc(100vh - 130px);\r\n    overflow-y: auto;\r\n    overflow-x: hidden;\r\n    margin-right: -10px;\r\n    padding-right: 10px;\r\n}\r\n    aside[_ngcontent-%COMP%]{\r\n    padding-bottom: 0px !important;\r\n}\r\n    .list-drag[_ngcontent-%COMP%]{\r\n    display: inline-flex;\r\n    width: 100%;\r\n    align-items: center;\r\n    justify-items: center;\r\n    place-content: space-between;\r\n    margin: 0 0px 5px 0px;\r\n    padding: 5px;\r\n    font-size: 14px;\r\n    border: 1px solid #eae7e7;\r\n}\r\n    .custom-select[_ngcontent-%COMP%]{\r\n    padding: 0px 5px;\r\n    font-size: 14px;\r\n    line-height: 25px;\r\n}\r\n    .border-muted[_ngcontent-%COMP%]{\r\n    border: 1px solid rgba(0, 0, 0, 0.2);\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvY29tcGFueS1hZGRyZXNzZXMvcmVnaW9uL2Rpc3BhdGNoLXJlZ2lvbi9kaXNwYXRjaC1yZWdpb24uY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtJQUNJLFdBQVc7SUFDWCxlQUFlO0lBQ2YsZ0JBQWdCO0FBQ3BCO0lBQ0k7UUFDSSxlQUFlO1FBQ2YsZ0JBQWdCO0lBQ3BCO0lBQ0o7SUFDSSwyQkFBMkI7SUFDM0IsZ0JBQWdCO0lBQ2hCLGtCQUFrQjtJQUNsQixtQkFBbUI7SUFDbkIsbUJBQW1CO0FBQ3ZCO0lBQ0E7SUFDSSw4QkFBOEI7QUFDbEM7SUFDQTtJQUNJLG9CQUFvQjtJQUNwQixXQUFXO0lBQ1gsbUJBQW1CO0lBQ25CLHFCQUFxQjtJQUNyQiw0QkFBNEI7SUFDNUIscUJBQXFCO0lBQ3JCLFlBQVk7SUFDWixlQUFlO0lBQ2YseUJBQXlCO0FBQzdCO0lBQ0E7SUFDSSxnQkFBZ0I7SUFDaEIsZUFBZTtJQUNmLGlCQUFpQjtBQUNyQjtJQUNBO0lBQ0ksb0NBQW9DO0FBQ3hDIiwiZmlsZSI6InNyYy9hcHAvY29tcGFueS1hZGRyZXNzZXMvcmVnaW9uL2Rpc3BhdGNoLXJlZ2lvbi9kaXNwYXRjaC1yZWdpb24uY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5uZy1zZWxlY3QuY3VzdG9tIHtcclxuICAgIGJvcmRlcjogMHB4O1xyXG4gICAgbWluLWhlaWdodDogMHB4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogMDtcclxufVxyXG4gICAgLm5nLXNlbGVjdC5jdXN0b20gLm5nLXNlbGVjdC1jb250YWluZXIge1xyXG4gICAgICAgIG1pbi1oZWlnaHQ6IDBweDtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiAwO1xyXG4gICAgfVxyXG4uc2Nyb2xsLWhlaWdodHtcclxuICAgIGhlaWdodDogY2FsYygxMDB2aCAtIDEzMHB4KTtcclxuICAgIG92ZXJmbG93LXk6IGF1dG87XHJcbiAgICBvdmVyZmxvdy14OiBoaWRkZW47XHJcbiAgICBtYXJnaW4tcmlnaHQ6IC0xMHB4O1xyXG4gICAgcGFkZGluZy1yaWdodDogMTBweDtcclxufVxyXG5hc2lkZXtcclxuICAgIHBhZGRpbmctYm90dG9tOiAwcHggIWltcG9ydGFudDtcclxufVxyXG4ubGlzdC1kcmFne1xyXG4gICAgZGlzcGxheTogaW5saW5lLWZsZXg7XHJcbiAgICB3aWR0aDogMTAwJTtcclxuICAgIGFsaWduLWl0ZW1zOiBjZW50ZXI7XHJcbiAgICBqdXN0aWZ5LWl0ZW1zOiBjZW50ZXI7XHJcbiAgICBwbGFjZS1jb250ZW50OiBzcGFjZS1iZXR3ZWVuO1xyXG4gICAgbWFyZ2luOiAwIDBweCA1cHggMHB4O1xyXG4gICAgcGFkZGluZzogNXB4O1xyXG4gICAgZm9udC1zaXplOiAxNHB4O1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgI2VhZTdlNztcclxufVxyXG4uY3VzdG9tLXNlbGVjdHtcclxuICAgIHBhZGRpbmc6IDBweCA1cHg7XHJcbiAgICBmb250LXNpemU6IDE0cHg7XHJcbiAgICBsaW5lLWhlaWdodDogMjVweDtcclxufVxyXG4uYm9yZGVyLW11dGVke1xyXG4gICAgYm9yZGVyOiAxcHggc29saWQgcmdiYSgwLCAwLCAwLCAwLjIpO1xyXG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DispatchRegionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-dispatch-region',
          templateUrl: './dispatch-region.component.html',
          styleUrls: ['./dispatch-region.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _service_region_service__WEBPACK_IMPORTED_MODULE_4__["RegionService"]
        }, {
          type: src_app_statelist_service__WEBPACK_IMPORTED_MODULE_5__["StatelistService"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/model/region.ts": function srcAppCompanyAddressesRegionModelRegionTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionModel", function () {
      return RegionModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Region", function () {
      return Region;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TfxCarrierDropdownDisplayItem", function () {
      return TfxCarrierDropdownDisplayItem;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierRegionModel", function () {
      return CarrierRegionModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TfxCarrierRegionDetailsModel", function () {
      return TfxCarrierRegionDetailsModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SourceRegionModel", function () {
      return SourceRegionModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SourceRegion", function () {
      return SourceRegion;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FavProductType", function () {
      return FavProductType;
    });

    var RegionModel = function RegionModel() {
      _classCallCheck(this, RegionModel);
    };

    var Region = function Region() {
      _classCallCheck(this, Region);

      this.FavProductTypeId = FavProductType.ProductType;
    };

    var TfxCarrierDropdownDisplayItem = function TfxCarrierDropdownDisplayItem() {
      _classCallCheck(this, TfxCarrierDropdownDisplayItem);
    };

    var CarrierRegionModel = function CarrierRegionModel() {
      _classCallCheck(this, CarrierRegionModel);
    };

    var TfxCarrierRegionDetailsModel = function TfxCarrierRegionDetailsModel() {
      _classCallCheck(this, TfxCarrierRegionDetailsModel);
    };

    var SourceRegionModel = function SourceRegionModel() {
      _classCallCheck(this, SourceRegionModel);
    };

    var SourceRegion = function SourceRegion() {
      _classCallCheck(this, SourceRegion);
    };

    var FavProductType;

    (function (FavProductType) {
      FavProductType[FavProductType["None"] = 0] = "None";
      FavProductType[FavProductType["ProductType"] = 1] = "ProductType";
      FavProductType[FavProductType["FuelType"] = 2] = "FuelType";
    })(FavProductType || (FavProductType = {}));
    /***/

  },

  /***/
  "./src/app/company-addresses/region/region.component.ts": function srcAppCompanyAddressesRegionRegionComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionComponent", function () {
      return RegionComponent;
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


    var _dispatch_region_dispatch_region_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ./dispatch-region/dispatch-region.component */
    "./src/app/company-addresses/region/dispatch-region/dispatch-region.component.ts");
    /* harmony import */


    var _source_region_source_region_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ./source-region/source-region.component */
    "./src/app/company-addresses/region/source-region/source-region.component.ts");

    function RegionComponent_app_dispatch_region_12_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-dispatch-region");
      }
    }

    function RegionComponent_app_source_region_13_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "app-source-region");
      }
    }

    var RegionComponent = /*#__PURE__*/function () {
      function RegionComponent() {
        _classCallCheck(this, RegionComponent);

        this.viewType = 1;
      }

      _createClass(RegionComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {}
      }, {
        key: "changeViewType",
        value: function changeViewType(val) {
          this.viewType = val;
        }
      }]);

      return RegionComponent;
    }();

    RegionComponent.ɵfac = function RegionComponent_Factory(t) {
      return new (t || RegionComponent)();
    };

    RegionComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: RegionComponent,
      selectors: [["region-component"]],
      decls: 14,
      vars: 8,
      consts: [[1, "row"], [1, "col-sm-12", "sticky-header-dash"], [1, "dib", "border", "pa5", "radius-capsule", "shadow-b", "bg-white"], ["id", "RegionTab", 1, "btn-group", "btn-filter"], ["type", "radio", 1, "hide-element", 3, "name", "value", "checked"], [1, "btn", "ml0", 3, "click"], [1, "btn", "ml-1", 3, "click"], [1, "col-md-12"], [4, "ngIf"]],
      template: function RegionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "label", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function RegionComponent_Template_label_click_5_listener() {
            return ctx.changeViewType(1);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, "Dispatch Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](7, "input", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function RegionComponent_Template_label_click_8_listener() {
            return ctx.changeViewType(2);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Source Region");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, RegionComponent_app_dispatch_region_12_Template, 1, 0, "app-dispatch-region", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](13, RegionComponent_app_source_region_13_Template, 1, 0, "app-source-region", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "viewType")("value", 1)("checked", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("name", "viewType")("value", 2)("checked", ctx.viewType == 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.viewType == 2);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["NgIf"], _dispatch_region_dispatch_region_component__WEBPACK_IMPORTED_MODULE_2__["DispatchRegionComponent"], _source_region_source_region_component__WEBPACK_IMPORTED_MODULE_3__["SourceRegionComponent"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbXBhbnktYWRkcmVzc2VzL3JlZ2lvbi9yZWdpb24uY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'region-component',
          templateUrl: './region.component.html',
          styleUrls: ['./region.component.css']
        }]
      }], function () {
        return [];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/service/region.service.ts": function srcAppCompanyAddressesRegionServiceRegionServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "RegionService", function () {
      return RegionService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var RegionService = /*#__PURE__*/function (_src_app_errors_Handl2) {
      _inherits(RegionService, _src_app_errors_Handl2);

      var _super3 = _createSuper(RegionService);

      function RegionService(httpClient) {
        var _this46;

        _classCallCheck(this, RegionService);

        _this46 = _super3.call(this);
        _this46.httpClient = httpClient;
        _this46.createUrl = '/Region/Create';
        _this46.updateUrl = '/Region/Update';
        _this46.deleteUrl = '/Region/Delete?id=';
        _this46.getRegionsUrl = '/Region/GetRegions';
        _this46.getSourceRegionsUrl = '/Region/GetSourceRegions';
        _this46.createSourceRegionUrl = '/Region/CreateSourceRegion';
        _this46.updateSourceRegionUrl = '/Region/UpdateSourceRegion';
        _this46.deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
        _this46.getJobsUrl = '/Region/GetJobs';
        _this46.getDriversUrl = '/Region/GetDrivers';
        _this46.getDispatchersUrl = '/Region/GetDispatchers';
        _this46.getTrailersUrl = '/Region/GetTrailers';
        _this46.stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
        _this46.shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
        _this46.getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
        _this46.getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
        _this46.getCompanyShiftsUrl = '/Region/GetCompanyShifts';
        _this46.getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
        _this46.addDriverScheduleUrl = '/Region/AddDriverSchedule';
        _this46.addRegionScheduleUrl = '/Region/AddRegionSchedule';
        _this46.updateDriverScheduleUrl = '/Region/updateDriverSchedule';
        _this46.deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
        _this46.getCarriersUrl = '/Region/GetCarriers';
        _this46.getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
        _this46.getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
        _this46.getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
        _this46.isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
        _this46.getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
        _this46.getFuelProductUrl = '/Region/GetMstFuelProducts';
        _this46.isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';
        _this46.onLoadingChanged = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        return _this46;
      }

      _createClass(RegionService, [{
        key: "getJobs",
        value: function getJobs() {
          return this.httpClient.get(this.getJobsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getJobs', [])));
        }
      }, {
        key: "getDrivers",
        value: function getDrivers() {
          return this.httpClient.get(this.getDriversUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
        }
      }, {
        key: "getRegionDrivers",
        value: function getRegionDrivers(regiondId) {
          return this.httpClient.get(this.getRegionDriversUrl + regiondId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDrivers', [])));
        }
      }, {
        key: "getCompanyShifts",
        value: function getCompanyShifts() {
          return this.httpClient.get(this.getCompanyShiftsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCompanyShifts', [])));
        }
      }, {
        key: "getRoutesByRegion",
        value: function getRoutesByRegion(regionId) {
          return this.httpClient.get(this.getRouteByReginId + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetRouteInfoDetails', [])));
        }
      }, {
        key: "getDispatchers",
        value: function getDispatchers() {
          return this.httpClient.get(this.getDispatchersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getDispatchers', [])));
        }
      }, {
        key: "getTrailers",
        value: function getTrailers() {
          return this.httpClient.get(this.getTrailersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTrailers', [])));
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          return this.httpClient.get(this.getRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegions', null)));
        }
      }, {
        key: "createRegion",
        value: function createRegion(region) {
          return this.httpClient.post(this.createUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createRegion', region)));
        }
      }, {
        key: "updateRegion",
        value: function updateRegion(region) {
          return this.httpClient.post(this.updateUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateRegion', region)));
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions() {
          return this.httpClient.get(this.getSourceRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSourceRegions', null)));
        }
      }, {
        key: "createSourceRegion",
        value: function createSourceRegion(region) {
          return this.httpClient.post(this.createSourceRegionUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('createSourceRegion', region)));
        }
      }, {
        key: "isSourceRegionAvailable",
        value: function isSourceRegionAvailable(name, id) {
          return this.httpClient.get(this.isSourceRegionAvailableUrl + name + "&id=" + id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isSourceRegionAvailable', null)));
        }
      }, {
        key: "updateSourceRegion",
        value: function updateSourceRegion(region) {
          return this.httpClient.post(this.updateSourceRegionUrl, region, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('updateSourceRegion', region)));
        }
      }, {
        key: "deleteRegion",
        value: function deleteRegion(id) {
          return this.httpClient.post(this.deleteUrl + id, id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteRegion', id)));
        }
      }, {
        key: "deleteSourceRegion",
        value: function deleteSourceRegion(id) {
          return this.httpClient.post(this.deleteSourceRegionUrl + id, id).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteSourceRegion', id)));
        }
      }, {
        key: "getStates",
        value: function getStates(countryId) {
          return this.httpClient.get(this.stateUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getStates', [])));
        } //for calender

      }, {
        key: "getShiftByDrivers",
        value: function getShiftByDrivers(driverIds, scheduleType) {
          return this.httpClient.get(this.shiftByDriverUrl + driverIds + "&scheduleType=" + scheduleType).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getShiftByDrivers', [])));
        }
      }, {
        key: "getSchedulesByRegion",
        value: function getSchedulesByRegion(regionId, scheduleType) {
          return this.httpClient.get(this.getRegionSchedulsbyRegionIdUrl + regionId + "&scheduleType=" + scheduleType).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSchedulesByRegion', [])));
        }
      }, {
        key: "getRegionSchedule",
        value: function getRegionSchedule(regionId, routeId) {
          return this.httpClient.get(this.getRegionShiftMapping + regionId + "&routeId=" + routeId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getRegionSchedule', [])));
        }
      }, {
        key: "addDriverSchedule",
        value: function addDriverSchedule(model) {
          return this.httpClient.post(this.addDriverScheduleUrl, model, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', model)));
        }
      }, {
        key: "addRegionSchedule",
        value: function addRegionSchedule(model) {
          return this.httpClient.post(this.addRegionScheduleUrl, model, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addRegionSchedule', model)));
        }
      }, {
        key: "updateDriverSchedule",
        value: function updateDriverSchedule(data, date) {
          var postModel = {
            model: data,
            SelectedDate: date
          };
          return this.httpClient.post(this.updateDriverScheduleUrl, postModel, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('addDriverSchedule', postModel)));
        }
      }, {
        key: "deleteDriverSchedule",
        value: function deleteDriverSchedule(data, date) {
          var postModel = {
            driverScheduleMappingViewModels: data,
            SelectedDate: date
          };
          return this.httpClient.post(this.deleteDriverScheduleUrl, postModel, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('deleteDriverSchedule', postModel)));
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          return this.httpClient.get(this.getCarriersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriers', [])));
        }
      }, {
        key: "getCarrierRegions",
        value: function getCarrierRegions() {
          return this.httpClient.get(this.getCarrierRegionsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarrierRegions', null)));
        }
      }, {
        key: "getSelectedCarriersByRegion",
        value: function getSelectedCarriersByRegion(regionId) {
          return this.httpClient.get(this.getSelectedCarriersByRegionUrl + regionId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSelectedCarriersByRegion', null)));
        }
      }, {
        key: "getProductType",
        value: function getProductType() {
          return this.httpClient.get(this.getProductTypeUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductType', [])));
        }
      }, {
        key: "getFuelProducts",
        value: function getFuelProducts() {
          return this.httpClient.get(this.getFuelProductUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelProducts', [])));
        }
      }, {
        key: "isPublishedDR",
        value: function isPublishedDR(productIds, fuelTypeIds) {
          return this.httpClient.get(this.isPublishedDRUrl + productIds + "&fuelTypeIds=" + fuelTypeIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('isPublishedDR', null)));
        }
      }]);

      return RegionService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_4__["HandleError"]);

    RegionService.ɵfac = function RegionService_Factory(t) {
      return new (t || RegionService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]));
    };

    RegionService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: RegionService,
      factory: RegionService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RegionService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [{
          type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"]
        }];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/company-addresses/region/source-region/source-region.component.ts": function srcAppCompanyAddressesRegionSourceRegionSourceRegionComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SourceRegionComponent", function () {
      return SourceRegionComponent;
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


    var _model_region__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../model/region */
    "./src/app/company-addresses/region/model/region.ts");
    /* harmony import */


    var src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/declarations.module */
    "./src/app/declarations.module.ts");
    /* harmony import */


    var _service_region_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ../service/region.service */
    "./src/app/company-addresses/region/service/region.service.ts");
    /* harmony import */


    var src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! src/app/carrier/service/carrier.service */
    "./src/app/carrier/service/carrier.service.ts");
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

    function SourceRegionComponent_h3_5_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Edit Source Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_ng_template_6_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "h3", 40);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Create Source Region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_19_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Name is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_19_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SourceRegionComponent_div_19_div_1_Template, 2, 0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r3.rcForm.get("Name").errors.required);
      }
    }

    function SourceRegionComponent_div_20_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 42);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Source Region already exists. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_35_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " State is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_35_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SourceRegionComponent_div_35_div_1_Template, 2, 0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r5.rcForm.get("States").errors.required);
      }
    }

    function SourceRegionComponent_div_48_div_1_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, " Terminal is required. ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_48_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SourceRegionComponent_div_48_div_1_Template, 2, 0, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r6.rcForm.get("States").errors.required);
      }
    }

    function SourceRegionComponent_button_60_Template(rf, ctx) {
      if (rf & 1) {
        var _r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "button", 43);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_button_60_Template_button_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r17);

          var ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r16._toggleOpened(false);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Cancel");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_button_61_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "button", 44);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Save");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_62_div_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 46);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 47);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_div_62_div_1_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r20);

          var ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r19._toggleOpened(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 48);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span", 49);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_62_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, SourceRegionComponent_div_62_div_1_Template, 5, 0, "div", 45);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r9.isSalesUser);
      }
    }

    function SourceRegionComponent_div_64_a_4_Template(rf, ctx) {
      if (rf & 1) {
        var _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_div_64_a_4_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r23);

          var ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);

          return ctx_r22._toggleOpened(true);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 55);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "span", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Create New");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_64_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 50);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "h1", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, " No Region Available ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, SourceRegionComponent_div_64_a_4_Template, 4, 0, "a", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r10.isSalesUser);
      }
    }

    function SourceRegionComponent_div_67_a_7_Template(rf, ctx) {
      if (rf & 1) {
        var _r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_div_67_a_7_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r34);

          var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r32.editRegion(region_r24);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 67);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_67_a_8_Template(rf, ctx) {
      if (rf & 1) {
        var _r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "a", 1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_div_67_a_8_Template_a_click_0_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r37);

          var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r35.editRegion(region_r24);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "i", 68);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_67_div_9_Template(rf, ctx) {
      if (rf & 1) {
        var _r40 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 69);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 70);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_div_67_div_9_Template_a_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r40);

          var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

          var ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r38.setRegionIdToDelete(region_r24.Id);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "i", 71);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    function SourceRegionComponent_div_67_div_17_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var carrier_r42 = ctx.$implicit;
        var isLast_r43 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", carrier_r42.Name, "", isLast_r43 ? "" : ", ", " ");
      }
    }

    function SourceRegionComponent_div_67_div_17_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "small", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Carriers");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SourceRegionComponent_div_67_div_17_span_3_Template, 2, 2, "span", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r24.Carriers);
      }
    }

    function SourceRegionComponent_div_67_span_21_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var state_r45 = ctx.$implicit;
        var isLast_r46 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", state_r45.Name, "", isLast_r46 ? "" : ", ", " ");
      }
    }

    function SourceRegionComponent_div_67_div_22_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var terminal_r48 = ctx.$implicit;
        var isLast_r49 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", terminal_r48.Name, "", isLast_r49 ? "" : ", ", " ");
      }
    }

    function SourceRegionComponent_div_67_div_22_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "small", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Terminals");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SourceRegionComponent_div_67_div_22_span_3_Template, 2, 2, "span", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r24.Terminals);
      }
    }

    function SourceRegionComponent_div_67_div_23_span_3_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var bulkplant_r52 = ctx.$implicit;
        var isLast_r53 = ctx.last;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", bulkplant_r52.Name, "", isLast_r53 ? "" : ", ", " ");
      }
    }

    function SourceRegionComponent_div_67_div_23_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "small", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Bulk Plants");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, SourceRegionComponent_div_67_div_23_span_3_Template, 2, 2, "span", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r24.BulkPlants);
      }
    }

    function SourceRegionComponent_div_67_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 60);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 61);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, SourceRegionComponent_div_67_a_7_Template, 2, 0, "a", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, SourceRegionComponent_div_67_a_8_Template, 2, 0, "a", 62);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, SourceRegionComponent_div_67_div_9_Template, 3, 0, "div", 63);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 29);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 64);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "small", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Description");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](17, SourceRegionComponent_div_67_div_17_Template, 4, 1, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "small", 65);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "States");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, SourceRegionComponent_div_67_span_21_Template, 2, 2, "span", 66);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](22, SourceRegionComponent_div_67_div_22_Template, 4, 1, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, SourceRegionComponent_div_67_div_23_Template, 4, 1, "div", 41);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var region_r24 = ctx.$implicit;

        var ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](region_r24.Name);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("title", !ctx_r11.isSalesUser ? "edit region" : "view region");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r11.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r11.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx_r11.isSalesUser);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](region_r24.Description);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", region_r24.Carriers != null && region_r24.Carriers.length > 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", region_r24.States);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", region_r24.Terminals != null && region_r24.Terminals.length > 0);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", region_r24.BulkPlants != null && region_r24.BulkPlants.length > 0);
      }
    }

    function SourceRegionComponent_div_78_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 72);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 73);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 74);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 75);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Please Wait...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var SourceRegionComponent = /*#__PURE__*/function () {
      function SourceRegionComponent(fb, regionService, carrierService) {
        _classCallCheck(this, SourceRegionComponent);

        this.fb = fb;
        this.regionService = regionService;
        this.carrierService = carrierService;
        this._opened = false;
        this._animate = true;
        this._positionNum = 1;
        this._POSITIONS = ['left', 'right', 'top', 'bottom'];
        this.Cities = [];
        this.Terminals = [];
        this.BulkPlants = [];
        this.IsUpdate = false;
        this.SelectedRegionToDelete = null;
        this.IsLoading = true;
        this.IsEmpty = false;
        this.isSalesUser = false; //super();
      }

      _createClass(SourceRegionComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.multiselectSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
          };
          this.getRegions();
          this.getCarriers();
          this.rcForm = this.createForm();
          this.CurrentCompanyId = Number(currentCompanyId);
          this.getDefaultServingCountry();
        }
      }, {
        key: "ngAfterViewInit",
        value: function ngAfterViewInit() {
          if (typeof isSalesUser !== 'undefined' && isSalesUser) {
            this.isSalesUser = isSalesUser;
          }
        }
      }, {
        key: "fillDropdowns",
        value: function fillDropdowns() {
          this.getCarriers();
          this.getDefaultServingCountry();
          this.getCitiesByStateId();
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "createForm",
        value: function createForm() {
          if (this.region == undefined || this.region == null) this.region = new _model_region__WEBPACK_IMPORTED_MODULE_2__["SourceRegion"]();
          return this.fb.group({
            Id: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](0),
            Name: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Name, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Description: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Description),
            Carriers: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Carriers),
            States: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.States, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            Cities: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Cities),
            Terminals: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.Terminals, [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]),
            BulkPlants: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](this.region.BulkPlants)
          });
        }
      }, {
        key: "onSubmit",
        value: function onSubmit() {
          this.region = this.rcForm.value;

          if (this.rcForm.valid && !this.IsSourceRegionExist) {
            if (this.IsUpdate) {
              this.updateRegion();
            } else {
              this.createRegion();
            }
          } else {
            this.rcForm.markAllAsTouched();
          }
        }
      }, {
        key: "isSourceRegionAvailable",
        value: function isSourceRegionAvailable(name, id) {
          var _this47 = this;

          if (name != null) {
            this.regionService.isSourceRegionAvailable(name, id).subscribe(function (data) {
              _this47.IsSourceRegionExist = data;
            }, function (error) {
              _this47.IsSourceRegionExist = false;
            });
          }
        }
      }, {
        key: "createRegion",
        value: function createRegion() {
          var _this48 = this;

          if (!this.IsSourceRegionExist) {
            this.IsLoading = true;
            this.regionService.createSourceRegion(this.region).subscribe(function (response) {
              _this48.serviceResponse = response;

              if (response != null && response.StatusCode == 0) {
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess('Source Region created successfully', undefined, undefined);
                _this48.IsLoading = false;

                _this48._toggleOpened(false);

                _this48.getRegions();
              } else {
                _this48.IsLoading = false;
                src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
              }
            });
          }
        }
      }, {
        key: "editRegion",
        value: function editRegion(_region) {
          this.region = _region;

          this._toggleOpened(true);

          this.IsUpdate = true;

          if (this.rcForm) {
            this.rcForm.controls['Id'].setValue(this.region.Id);
            this.rcForm.controls['Name'].setValue(this.region.Name);
            this.rcForm.controls['Description'].setValue(this.region.Description);
            this.rcForm.controls['Carriers'].setValue(this.region.Carriers);
            this.rcForm.controls['States'].setValue(this.region.States);
            if (this.region.Cities != null) this.region.Cities.forEach(function (s) {
              return s.Code = s.Code.replace(" ", "");
            });
            this.rcForm.controls['Cities'].setValue(this.region.Cities);
            this.rcForm.controls['Terminals'].setValue(this.region.Terminals);
            this.rcForm.controls['BulkPlants'].setValue(this.region.BulkPlants);
          }

          this.fillDropdowns();
        }
      }, {
        key: "updateRegion",
        value: function updateRegion() {
          var _this49 = this;

          this.IsLoading = true;
          this.regionService.updateSourceRegion(this.region).subscribe(function (response) {
            _this49.serviceResponse = response;

            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess('Source Region updated successfully', undefined, undefined);
              _this49.IsLoading = false;

              _this49._toggleOpened(false);

              _this49.getRegions();
            } else {
              _this49.IsLoading = false;
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
        }
      }, {
        key: "_toggleOpened",
        value: function _toggleOpened(shouldOpen) {
          this.IsSourceRegionExist = false;
          this.rcForm.controls['Id'].setValue(0);

          if (shouldOpen) {
            this._opened = true;
            this.Terminals = [];
            this.BulkPlants = [];
          } else {
            this._opened = !this._opened;
            this.rcForm.reset();
            this.IsUpdate = false;
          }
        }
      }, {
        key: "getRegions",
        value: function getRegions() {
          var _this50 = this;

          this.IsLoading = true;
          this.regionService.getSourceRegions().subscribe(function (response) {
            _this50.regions = response.Regions;

            if (_this50.regions != null && _this50.regions != undefined && _this50.regions.length == 0) {
              _this50.IsEmpty = true;
            }

            _this50.IsLoading = false;
          });
        }
      }, {
        key: "setRegionIdToDelete",
        value: function setRegionIdToDelete(id) {
          this.SelectedRegionToDelete = id;
        }
      }, {
        key: "deleteRegion",
        value: function deleteRegion() {
          var _this51 = this;

          if (this.SelectedRegionToDelete == null) {
            return;
          }

          this.IsLoading = true;
          this.regionService.deleteSourceRegion(this.SelectedRegionToDelete).subscribe(function (response) {
            _this51.IsLoading = false;

            if (response != null && response.StatusCode == 0) {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgsuccess('Deleted successfully', undefined, undefined);

              _this51.getRegions();
            } else {
              src_app_declarations_module__WEBPACK_IMPORTED_MODULE_3__["Declarations"].msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
            }
          });
          this.SelectedRegionToDelete == null;
        }
      }, {
        key: "getDefaultServingCountry",
        value: function getDefaultServingCountry() {
          var _this52 = this;

          this.IsLoading = true;
          this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(function (data) {
            _this52.IsLoading = false;
            _this52.SelectedCountryId = Number(data.DefaultCountryId);

            _this52.getStates();
          });
        }
      }, {
        key: "getStates",
        value: function getStates() {
          var _this53 = this;

          this.IsLoading = true;

          if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId).subscribe(function (states) {
              _this53.States = states;
              _this53.IsLoading = false;
            });
          }
        }
      }, {
        key: "getCities",
        value: function getCities(stateIds) {
          var _this54 = this;

          this.IsLoading = true;
          this.carrierService.getCities(stateIds).subscribe(function (data) {
            _this54.IsLoading = false;
            _this54.Cities = data;

            _this54.Cities.forEach(function (s) {
              return s.Code = s.Code.replace(" ", "");
            });
          });
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          var _this55 = this;

          this.IsLoading = true;
          this.regionService.getCarriers().subscribe(function (carriers) {
            _this55.CarrierList = carriers;
            _this55.IsLoading = false;
          });
        }
      }, {
        key: "getTerminals",
        value: function getTerminals() {
          var _this56 = this;

          var selectedStates = this.rcForm.get('States').value;
          var selectedCities = this.rcForm.get('Cities').value;

          if (selectedStates.length == 0) {
            this.rcForm.get('States').patchValue([]);
            this.Terminals = [];
            return;
          }

          this.IsLoading = true;
          var input = new _model_region__WEBPACK_IMPORTED_MODULE_2__["SourceRegion"]();

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
            _this56.IsLoading = false;
            _this56.Terminals = data;
          });
        }
      }, {
        key: "getBulkPlants",
        value: function getBulkPlants() {
          var _this57 = this;

          var selectedStates = this.rcForm.get('States').value;
          var selectedCities = this.rcForm.get('Cities').value;

          if (selectedStates.length == 0) {
            this.rcForm.get('States').patchValue([]);
            this.BulkPlants = [];
            return;
          }

          this.IsLoading = true;
          var input = new _model_region__WEBPACK_IMPORTED_MODULE_2__["SourceRegion"]();

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
          this.carrierService.getBulkPlants(input).subscribe(function (data) {
            _this57.IsLoading = false;
            _this57.BulkPlants = data;
          });
        }
      }, {
        key: "onStateSelect",
        value: function onStateSelect(state) {
          this.getCitiesByStateId();
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onStateDeSelect",
        value: function onStateDeSelect(state) {
          this.rcForm.get('Cities').patchValue([]);
          this.rcForm.get('Terminals').patchValue([]);
          this.rcForm.get('BulkPlants').patchValue([]);
          this.getCitiesByStateId();
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onStateSelectAll",
        value: function onStateSelectAll(states) {
          this.rcForm.get('States').patchValue(states);
          this.getCitiesByStateId();
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onStateDeSelectAll",
        value: function onStateDeSelectAll() {
          this.rcForm.get('Cities').patchValue([]);
          this.rcForm.get('Terminals').patchValue([]);
          this.rcForm.get('BulkPlants').patchValue([]);
          this.Cities = [];
          this.Terminals = [];
          this.BulkPlants = [];
        }
      }, {
        key: "getCitiesByStateId",
        value: function getCitiesByStateId() {
          var selectedStates = this.rcForm.get('States').value;

          if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(function (m) {
              return m.Id;
            });
            this.getCities(stateIds);
          } else {
            this.Cities = [];
          }
        }
      }, {
        key: "onCitySelect",
        value: function onCitySelect(city) {
          this.rcForm.get('Terminals').patchValue([]);
          this.rcForm.get('BulkPlants').patchValue([]);
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onCityDeSelect",
        value: function onCityDeSelect(city) {
          this.rcForm.get('Terminals').patchValue([]);
          this.rcForm.get('BulkPlants').patchValue([]);
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onCitySelectAll",
        value: function onCitySelectAll(cities) {
          this.rcForm.get('Cities').patchValue(cities);
          this.getTerminals();
          this.getBulkPlants();
        }
      }, {
        key: "onCityDeSelectAll",
        value: function onCityDeSelectAll() {
          this.rcForm.get('Terminals').patchValue([]);
          this.rcForm.get('BulkPlants').patchValue([]);
          this.rcForm.get('Cities').patchValue([]);
          this.getTerminals();
          this.getBulkPlants();
        }
      }]);

      return SourceRegionComponent;
    }();

    SourceRegionComponent.ɵfac = function SourceRegionComponent_Factory(t) {
      return new (t || SourceRegionComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_region_service__WEBPACK_IMPORTED_MODULE_4__["RegionService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]));
    };

    SourceRegionComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: SourceRegionComponent,
      selectors: [["app-source-region"]],
      decls: 79,
      vars: 31,
      consts: [[2, "height", "100vh", 3, "opened", "animate", "position", "openedChange"], [3, "click"], [1, "fa", "fa-close", "fs18"], ["class", "dib ml10 mt10 mb10", 4, "ngIf", "ngIfElse"], ["editTitle", ""], [1, "pr30"], [3, "formGroup", "ngSubmit"], [1, "row"], [1, "col-sm-6"], [1, "form-group"], ["for", "Name"], [1, "color-maroon"], ["formControlName", "Id", "type", "hidden", 1, "hide-element"], ["formControlName", "Name", "name", "Name", "required", "", 1, "form-control", 3, "change"], ["class", "color-maroon", 4, "ngIf"], ["class", "error-text color-maroon", 4, "ngIf"], ["for", "Carrier"], ["formControlName", "Carriers", 3, "placeholder", "settings", "data"], ["for", "Description"], ["formControlName", "States", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], ["formControlName", "Cities", 3, "placeholder", "settings", "data", "onSelect", "onDeSelect", "onSelectAll", "onDeSelectAll"], ["formControlName", "Terminals", 3, "placeholder", "settings", "data"], ["formControlName", "BulkPlants", 3, "placeholder", "settings", "data"], ["formControlName", "Description", 1, "form-control"], [1, "text-right"], ["class", "btn btn-lg", "type", "reset", 3, "click", 4, "ngIf"], ["class", "ml15 btn btn-primary btn-lg", "type", "submit", 4, "ngIf"], ["class", "row", 4, "ngIf"], ["class", "text-center wrapper-nodata", 4, "ngIf"], [1, "col-sm-12"], ["id", "regionDetails", 1, "row", "d-flex", "align-items-stretch"], ["class", "col-sm-3", 4, "ngFor", "ngForOf"], ["id", "myModal", "role", "dialog", "data-backdrop", "static", "data-keyboard", "false", 1, "modal", "fade"], [1, "modal-dialog", 2, "width", "200px"], [1, "modal-content"], [1, "modal-body"], [1, "modal-footer"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-danger", 3, "click"], ["type", "button", "data-dismiss", "modal", 1, "btn", "btn-success", 3, "click"], ["class", "loader", 4, "ngIf"], [1, "dib", "ml10", "mt10", "mb10"], [4, "ngIf"], [1, "error-text", "color-maroon"], ["type", "reset", 1, "btn", "btn-lg", 3, "click"], ["type", "submit", 1, "ml15", "btn", "btn-primary", "btn-lg"], ["class", "col-sm-12 pt10 pb10", 4, "ngIf"], [1, "col-sm-12", "pt10", "pb10"], ["id", "createnewregion", 1, "fs18", "pull-left", 3, "click"], [1, "fa", "fa-plus-circle", "fs18", "mt3", "pull-left"], [1, "fs14", "mt1", "pull-left"], [1, "text-center", "wrapper-nodata"], [1, "fas", "fa-map-marked-alt"], [1, "mb20", "mt10", "f-normal"], ["id", "createnewregion", "class", "btn btn-primary", 3, "click", 4, "ngIf"], ["id", "createnewregion", 1, "btn", "btn-primary", 3, "click"], [1, "fa", "fa-plus", "fs14", "mt5", "pull-left", "mr5"], [1, "fs14", "mt2", "ml5", "pull-left"], [1, "col-sm-3"], [1, "well", "box-shadow", "tile-xs", "animated", "zoomIn"], [1, "col-sm-8", "fs18"], [1, "col-sm-4", "text-right"], [1, "pull-right", 3, "title"], [3, "click", 4, "ngIf"], ["class", "pull-right mr15", "title", "delete region", 4, "ngIf"], [1, "region-data"], [1, "db", "mt10", "color-lightgrey", "f-bold", "fs13"], [4, "ngFor", "ngForOf"], [1, "fa", "fa-edit", "fs14"], [1, "fa", "fa-eye", "fs14"], ["title", "delete region", 1, "pull-right", "mr15"], ["data-toggle", "modal", "data-target", "#myModal", 3, "click"], [1, "fa", "fa-trash-alt", "color-maroon"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function SourceRegionComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ng-sidebar-container");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "ng-sidebar", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("openedChange", function SourceRegionComponent_Template_ng_sidebar_openedChange_2_listener($event) {
            return ctx._opened = $event;
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_Template_a_click_3_listener() {
            return ctx._toggleOpened(false);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](4, "i", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, SourceRegionComponent_h3_5_Template, 2, 0, "h3", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, SourceRegionComponent_ng_template_6_Template, 2, 0, "ng-template", null, 4, _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplateRefExtractor"]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "content", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "form", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function SourceRegionComponent_Template_form_ngSubmit_9_listener() {
            return ctx.onSubmit();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "label", 10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](17, "input", 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "input", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SourceRegionComponent_Template_input_change_18_listener() {
            return ctx.isSourceRegionAvailable(ctx.rcForm.get("Name").value, ctx.rcForm.get("Id").value);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, SourceRegionComponent_div_19_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](20, SourceRegionComponent_div_20_Template, 2, 0, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "label", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Carrier(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](26, "ng-multiselect-dropdown", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](31, "States(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](33, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "ng-multiselect-dropdown", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function SourceRegionComponent_Template_ng_multiselect_dropdown_onSelect_34_listener($event) {
            return ctx.onStateSelect($event);
          })("onDeSelect", function SourceRegionComponent_Template_ng_multiselect_dropdown_onDeSelect_34_listener($event) {
            return ctx.onStateDeSelect($event);
          })("onSelectAll", function SourceRegionComponent_Template_ng_multiselect_dropdown_onSelectAll_34_listener($event) {
            return ctx.onStateSelectAll($event);
          })("onDeSelectAll", function SourceRegionComponent_Template_ng_multiselect_dropdown_onDeSelectAll_34_listener() {
            return ctx.onStateDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](35, SourceRegionComponent_div_35_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](39, "City(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "ng-multiselect-dropdown", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("onSelect", function SourceRegionComponent_Template_ng_multiselect_dropdown_onSelect_40_listener($event) {
            return ctx.onCitySelect($event);
          })("onDeSelect", function SourceRegionComponent_Template_ng_multiselect_dropdown_onDeSelect_40_listener($event) {
            return ctx.onCityDeSelect($event);
          })("onSelectAll", function SourceRegionComponent_Template_ng_multiselect_dropdown_onSelectAll_40_listener($event) {
            return ctx.onCitySelectAll($event);
          })("onDeSelectAll", function SourceRegionComponent_Template_ng_multiselect_dropdown_onDeSelectAll_40_listener() {
            return ctx.onCityDeSelectAll();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Terminal(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "span", 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](46, "*");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](47, "ng-multiselect-dropdown", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](48, SourceRegionComponent_div_48_Template, 2, 1, "div", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "label");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](52, "Bulk Plant(s)");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](53, "ng-multiselect-dropdown", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "div", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "div", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "label", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Description");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](58, "textarea", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "div", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](60, SourceRegionComponent_button_60_Template, 2, 0, "button", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](61, SourceRegionComponent_button_61_Template, 2, 0, "button", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](62, SourceRegionComponent_div_62_Template, 2, 1, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](64, SourceRegionComponent_div_64_Template, 5, 1, "div", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](66, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](67, SourceRegionComponent_div_67_Template, 24, 10, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "div", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](69, "div", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](70, "div", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](71, "div", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](72, " Are you sure to delete? ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "div", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](74, "button", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_Template_button_click_74_listener() {
            return ctx.deleteRegion();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](75, "Yes");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "button", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function SourceRegionComponent_Template_button_click_76_listener() {
            return ctx.setRegionIdToDelete(null);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](77, "No");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](78, SourceRegionComponent_div_78_Template, 5, 0, "div", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          var _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("opened", ctx._opened)("animate", ctx._animate)("position", ctx._POSITIONS[ctx._positionNum]);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsUpdate)("ngIfElse", _r1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.rcForm);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("Name").invalid && (ctx.rcForm.get("Name").dirty || ctx.rcForm.get("Name").touched));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsSourceRegionExist == false);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Carrier(s)")("settings", ctx.multiselectSettingsById)("data", ctx.CarrierList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select State(s)")("settings", ctx.multiselectSettingsById)("data", ctx.States);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("States").invalid && ctx.rcForm.get("States").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select City(s)")("settings", ctx.multiselectSettingsByCode)("data", ctx.Cities);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select Terminal(s)")("settings", ctx.multiselectSettingsById)("data", ctx.Terminals);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.rcForm.get("States").invalid && ctx.rcForm.get("States").touched);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("placeholder", "Select BulkPlant(s)")("settings", ctx.multiselectSettingsById)("data", ctx.BulkPlants);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isSalesUser);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.isSalesUser);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", (ctx.regions == null ? null : ctx.regions.length) > 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !ctx.IsLoading && (!ctx.regions || (ctx.regions == null ? null : ctx.regions.length) == 0));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.regions);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["SidebarContainer"], ng_sidebar__WEBPACK_IMPORTED_MODULE_6__["Sidebar"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["RequiredValidator"], ng_multiselect_dropdown__WEBPACK_IMPORTED_MODULE_8__["MultiSelectComponent"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"]],
      styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbXBhbnktYWRkcmVzc2VzL3JlZ2lvbi9zb3VyY2UtcmVnaW9uL3NvdXJjZS1yZWdpb24uY29tcG9uZW50LmNzcyJ9 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SourceRegionComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-source-region',
          templateUrl: './source-region.component.html',
          styleUrls: ['./source-region.component.css']
        }]
      }], function () {
        return [{
          type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]
        }, {
          type: _service_region_service__WEBPACK_IMPORTED_MODULE_4__["RegionService"]
        }, {
          type: src_app_carrier_service_carrier_service__WEBPACK_IMPORTED_MODULE_5__["CarrierService"]
        }];
      }, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=company-addresses-lazy-loading-company-addresses-module-es5.js.map