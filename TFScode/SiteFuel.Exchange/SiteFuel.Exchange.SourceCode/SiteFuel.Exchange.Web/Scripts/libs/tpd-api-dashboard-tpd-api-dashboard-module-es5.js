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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["tpd-api-dashboard-tpd-api-dashboard-module"], {
  /***/
  "./node_modules/ngx-json-viewer/__ivy_ngcc__/ngx-json-viewer.js": function node_modulesNgxJsonViewer__ivy_ngcc__NgxJsonViewerJs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "NgxJsonViewerModule", function () {
      return NgxJsonViewerModule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "NgxJsonViewerComponent", function () {
      return NgxJsonViewerComponent;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");

    function NgxJsonViewerComponent_section_1_div_2_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "div", 9);
      }
    }

    function NgxJsonViewerComponent_section_1_span_7_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "span", 10);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var segment_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](segment_r1.description);
      }
    }

    function NgxJsonViewerComponent_section_1_section_8_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "section", 11);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](1, "ngx-json-viewer", 12);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var segment_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]().$implicit;

        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("json", segment_r1.value)("expanded", ctx_r4.expanded);
      }
    }

    var _c0 = function _c0(a1) {
      return ["segment", a1];
    };

    var _c1 = function _c1(a1, a2) {
      return {
        "segment-main": true,
        "expandable": a1,
        "expanded": a2
      };
    };

    function NgxJsonViewerComponent_section_1_Template(rf, ctx) {
      if (rf & 1) {
        var _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "section", 2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "section", 3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function NgxJsonViewerComponent_section_1_Template_section_click_1_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r8);

          var segment_r1 = ctx.$implicit;

          var ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r7.toggle(segment_r1);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, NgxJsonViewerComponent_section_1_div_2_Template, 1, 0, "div", 4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "span", 5);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "span", 6);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, ": ");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, NgxJsonViewerComponent_section_1_span_7_Template, 2, 1, "span", 7);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, NgxJsonViewerComponent_section_1_section_8_Template, 2, 2, "section", 8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var segment_r1 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](6, _c0, "segment-type-" + segment_r1.type));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction2"](8, _c1, ctx_r0.isExpandable(segment_r1), segment_r1.expanded));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.isExpandable(segment_r1));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](segment_r1.key);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", !segment_r1.expanded || !ctx_r0.isExpandable(segment_r1));

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", segment_r1.expanded && ctx_r0.isExpandable(segment_r1));
      }
    }

    var NgxJsonViewerComponent = /*#__PURE__*/function () {
      function NgxJsonViewerComponent() {
        _classCallCheck(this, NgxJsonViewerComponent);

        this.expanded = true;
        /**
         * @deprecated It will be always true and deleted in version 3.0.0
         */

        this.cleanOnChange = true;
        this.segments = [];
      }
      /**
       * @return {?}
       */


      _createClass(NgxJsonViewerComponent, [{
        key: "ngOnChanges",
        value: function ngOnChanges() {
          var _this = this;

          if (this.cleanOnChange) {
            this.segments = [];
          }

          if (typeof this.json === 'object') {
            Object.keys(this.json).forEach(function (key) {
              _this.segments.push(_this.parseKeyValue(key, _this.json[key]));
            });
          } else {
            this.segments.push(this.parseKeyValue("(".concat(typeof this.json, ")"), this.json));
          }
        }
        /**
         * @param {?} segment
         * @return {?}
         */

      }, {
        key: "isExpandable",
        value: function isExpandable(segment) {
          return segment.type === 'object' || segment.type === 'array';
        }
        /**
         * @param {?} segment
         * @return {?}
         */

      }, {
        key: "toggle",
        value: function toggle(segment) {
          if (this.isExpandable(segment)) {
            segment.expanded = !segment.expanded;
          }
        }
        /**
         * @param {?} key
         * @param {?} value
         * @return {?}
         */

      }, {
        key: "parseKeyValue",
        value: function parseKeyValue(key, value) {
          var
          /** @type {?} */
          segment = {
            key: key,
            value: value,
            type: undefined,
            description: '' + value,
            expanded: this.expanded
          };

          switch (typeof segment.value) {
            case 'number':
              {
                segment.type = 'number';
                break;
              }

            case 'boolean':
              {
                segment.type = 'boolean';
                break;
              }

            case 'function':
              {
                segment.type = 'function';
                break;
              }

            case 'string':
              {
                segment.type = 'string';
                segment.description = '"' + segment.value + '"';
                break;
              }

            case 'undefined':
              {
                segment.type = 'undefined';
                segment.description = 'undefined';
                break;
              }

            case 'object':
              {
                // yea, null is object
                if (segment.value === null) {
                  segment.type = 'null';
                  segment.description = 'null';
                } else if (Array.isArray(segment.value)) {
                  segment.type = 'array';
                  segment.description = 'Array[' + segment.value.length + '] ' + JSON.stringify(segment.value);
                } else if (segment.value instanceof Date) {
                  segment.type = 'date';
                } else {
                  segment.type = 'object';
                  segment.description = 'Object ' + JSON.stringify(segment.value);
                }

                break;
              }
          }

          return segment;
        }
      }]);

      return NgxJsonViewerComponent;
    }();

    NgxJsonViewerComponent.ɵfac = function NgxJsonViewerComponent_Factory(t) {
      return new (t || NgxJsonViewerComponent)();
    };

    NgxJsonViewerComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: NgxJsonViewerComponent,
      selectors: [["ngx-json-viewer"]],
      inputs: {
        expanded: "expanded",
        cleanOnChange: "cleanOnChange",
        json: "json"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]],
      decls: 2,
      vars: 1,
      consts: [[1, "ngx-json-viewer"], [3, "ngClass", 4, "ngFor", "ngForOf"], [3, "ngClass"], [3, "ngClass", "click"], ["class", "toggler", 4, "ngIf"], [1, "segment-key"], [1, "segment-separator"], ["class", "segment-value", 4, "ngIf"], ["class", "children", 4, "ngIf"], [1, "toggler"], [1, "segment-value"], [1, "children"], [3, "json", "expanded"]],
      template: function NgxJsonViewerComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "section", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, NgxJsonViewerComponent_section_1_Template, 9, 11, "section", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.segments);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_1__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_1__["NgIf"], NgxJsonViewerComponent],
      styles: ["@charset \"UTF-8\";\n    .ngx-json-viewer[_ngcontent-%COMP%] {\n      font-family: monospace;\n      font-size: 1em;\n      width: 100%;\n      height: 100%;\n      overflow: hidden;\n      position: relative; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%] {\n        padding: 2px;\n        margin: 1px 1px 1px 12px; }\n        .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%] {\n          word-wrap: break-word; }\n          .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%]   .toggler[_ngcontent-%COMP%] {\n            position: absolute;\n            margin-left: -14px;\n            margin-top: 3px;\n            font-size: .8em;\n            line-height: 1.2em;\n            vertical-align: middle;\n            color: #787878; }\n            .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%]   .toggler[_ngcontent-%COMP%]::after {\n              display: inline-block;\n              content: \"\u25BA\";\n              -webkit-transition: -webkit-transform 0.1s ease-in;\n              transition: -webkit-transform 0.1s ease-in;\n              transition: transform 0.1s ease-in;\n              transition: transform 0.1s ease-in, -webkit-transform 0.1s ease-in; }\n          .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%]   .segment-key[_ngcontent-%COMP%] {\n            color: #4E187C; }\n          .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%]   .segment-separator[_ngcontent-%COMP%] {\n            color: #999; }\n          .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .segment-main[_ngcontent-%COMP%]   .segment-value[_ngcontent-%COMP%] {\n            color: #000; }\n        .ngx-json-viewer[_ngcontent-%COMP%]   .segment[_ngcontent-%COMP%]   .children[_ngcontent-%COMP%] {\n          margin-left: 12px; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-string[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #FF6B6B; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-number[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #009688; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-boolean[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #b938a4; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-date[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #05668D; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-array[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #999; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-object[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #999; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-function[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #999; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-null[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #fff; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-undefined[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        color: #fff; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-null[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        background-color: red; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-undefined[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-key[_ngcontent-%COMP%] {\n        color: #999; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-undefined[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%]    > .segment-value[_ngcontent-%COMP%] {\n        background-color: #999; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-object[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%], .ngx-json-viewer[_ngcontent-%COMP%]   .segment-type-array[_ngcontent-%COMP%]    > .segment-main[_ngcontent-%COMP%] {\n        white-space: nowrap; }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .expanded[_ngcontent-%COMP%]    > .toggler[_ngcontent-%COMP%]::after {\n        -webkit-transform: rotate(90deg);\n                transform: rotate(90deg); }\n      .ngx-json-viewer[_ngcontent-%COMP%]   .expandable[_ngcontent-%COMP%], .ngx-json-viewer[_ngcontent-%COMP%]   .expandable[_ngcontent-%COMP%]    > .toggler[_ngcontent-%COMP%] {\n        cursor: pointer; }"]
    });
    /**
     * @nocollapse
     */

    NgxJsonViewerComponent.ctorParameters = function () {
      return [];
    };

    NgxJsonViewerComponent.propDecorators = {
      'json': [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      'expanded': [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      'cleanOnChange': [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgxJsonViewerComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'ngx-json-viewer',
          template: "\n    <section class=\"ngx-json-viewer\">\n      <section\n        *ngFor=\"let segment of segments\"\n        [ngClass]=\"['segment', 'segment-type-' + segment.type]\">\n        <section\n          (click)=\"toggle(segment)\"\n          [ngClass]=\"{\n            'segment-main': true,\n            'expandable': isExpandable(segment),\n            'expanded': segment.expanded\n          }\">\n          <div *ngIf=\"isExpandable(segment)\" class=\"toggler\"></div>\n          <span class=\"segment-key\">{{ segment.key }}</span>\n          <span class=\"segment-separator\">: </span>\n          <span *ngIf=\"!segment.expanded || !isExpandable(segment)\" class=\"segment-value\">{{ segment.description }}</span>\n        </section>\n        <section *ngIf=\"segment.expanded && isExpandable(segment)\" class=\"children\">\n          <ngx-json-viewer [json]=\"segment.value\" [expanded]=\"expanded\"></ngx-json-viewer>\n        </section>\n      </section>\n    </section>\n  ",
          styles: ["\n    @charset \"UTF-8\";\n    .ngx-json-viewer {\n      font-family: monospace;\n      font-size: 1em;\n      width: 100%;\n      height: 100%;\n      overflow: hidden;\n      position: relative; }\n      .ngx-json-viewer .segment {\n        padding: 2px;\n        margin: 1px 1px 1px 12px; }\n        .ngx-json-viewer .segment .segment-main {\n          word-wrap: break-word; }\n          .ngx-json-viewer .segment .segment-main .toggler {\n            position: absolute;\n            margin-left: -14px;\n            margin-top: 3px;\n            font-size: .8em;\n            line-height: 1.2em;\n            vertical-align: middle;\n            color: #787878; }\n            .ngx-json-viewer .segment .segment-main .toggler::after {\n              display: inline-block;\n              content: \"\u25BA\";\n              -webkit-transition: -webkit-transform 0.1s ease-in;\n              transition: -webkit-transform 0.1s ease-in;\n              transition: transform 0.1s ease-in;\n              transition: transform 0.1s ease-in, -webkit-transform 0.1s ease-in; }\n          .ngx-json-viewer .segment .segment-main .segment-key {\n            color: #4E187C; }\n          .ngx-json-viewer .segment .segment-main .segment-separator {\n            color: #999; }\n          .ngx-json-viewer .segment .segment-main .segment-value {\n            color: #000; }\n        .ngx-json-viewer .segment .children {\n          margin-left: 12px; }\n      .ngx-json-viewer .segment-type-string > .segment-main > .segment-value {\n        color: #FF6B6B; }\n      .ngx-json-viewer .segment-type-number > .segment-main > .segment-value {\n        color: #009688; }\n      .ngx-json-viewer .segment-type-boolean > .segment-main > .segment-value {\n        color: #b938a4; }\n      .ngx-json-viewer .segment-type-date > .segment-main > .segment-value {\n        color: #05668D; }\n      .ngx-json-viewer .segment-type-array > .segment-main > .segment-value {\n        color: #999; }\n      .ngx-json-viewer .segment-type-object > .segment-main > .segment-value {\n        color: #999; }\n      .ngx-json-viewer .segment-type-function > .segment-main > .segment-value {\n        color: #999; }\n      .ngx-json-viewer .segment-type-null > .segment-main > .segment-value {\n        color: #fff; }\n      .ngx-json-viewer .segment-type-undefined > .segment-main > .segment-value {\n        color: #fff; }\n      .ngx-json-viewer .segment-type-null > .segment-main > .segment-value {\n        background-color: red; }\n      .ngx-json-viewer .segment-type-undefined > .segment-main > .segment-key {\n        color: #999; }\n      .ngx-json-viewer .segment-type-undefined > .segment-main > .segment-value {\n        background-color: #999; }\n      .ngx-json-viewer .segment-type-object > .segment-main,\n      .ngx-json-viewer .segment-type-array > .segment-main {\n        white-space: nowrap; }\n      .ngx-json-viewer .expanded > .toggler::after {\n        -webkit-transform: rotate(90deg);\n                transform: rotate(90deg); }\n      .ngx-json-viewer .expandable,\n      .ngx-json-viewer .expandable > .toggler {\n        cursor: pointer; }\n  "]
        }]
      }], function () {
        return [];
      }, {
        expanded: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        cleanOnChange: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        json: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();

    var NgxJsonViewerModule = function NgxJsonViewerModule() {
      _classCallCheck(this, NgxJsonViewerModule);
    };

    NgxJsonViewerModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: NgxJsonViewerModule
    });
    NgxJsonViewerModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function NgxJsonViewerModule_Factory(t) {
        return new (t || NgxJsonViewerModule)();
      },
      imports: [[_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"]]]
    });
    /**
     * @nocollapse
     */

    NgxJsonViewerModule.ctorParameters = function () {
      return [];
    };

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](NgxJsonViewerModule, {
        declarations: function declarations() {
          return [NgxJsonViewerComponent];
        },
        imports: function imports() {
          return [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"]];
        },
        exports: function exports() {
          return [NgxJsonViewerComponent];
        }
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](NgxJsonViewerModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"]],
          declarations: [NgxJsonViewerComponent],
          exports: [NgxJsonViewerComponent]
        }]
      }], null, null);
    })();
    /**
     * Generated bundle index. Do not edit.
     */
    //# sourceMappingURL=ngx-json-viewer.js.map

    /***/

  },

  /***/
  "./src/app/tpd-api-dashboard/dashboard/dashboard.component.ts": function srcAppTpdApiDashboardDashboardDashboardComponentTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DashboardComponent", function () {
      return DashboardComponent;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ApiDashBoardModel", function () {
      return ApiDashBoardModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ApiDetailModel", function () {
      return ApiDetailModel;
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


    var moment__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! moment */
    "./node_modules/moment/moment.js");
    /* harmony import */


    var moment__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(moment__WEBPACK_IMPORTED_MODULE_2__);
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");
    /* harmony import */


    var _service_api_dashboard_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../service/api-dashboard.service */
    "./src/app/tpd-api-dashboard/service/api-dashboard.service.ts");
    /* harmony import */


    var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! @angular/common */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
    /* harmony import */


    var _angular_forms__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(
    /*! @angular/forms */
    "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
    /* harmony import */


    var _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(
    /*! ../../directives/myDateTimePicker */
    "./src/app/directives/myDateTimePicker.ts");
    /* harmony import */


    var ngx_json_viewer__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(
    /*! ngx-json-viewer */
    "./node_modules/ngx-json-viewer/__ivy_ngcc__/ngx-json-viewer.js");

    function DashboardComponent_option_11_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 51);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var api_r6 = ctx.$implicit;

        var ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", api_r6)("selected", api_r6 == ctx_r0.selectedApi);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", api_r6, " ");
      }
    }

    function DashboardComponent_tr_62_Template(rf, ctx) {
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

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "button", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_tr_62_Template_button_click_10_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var log_r7 = ctx.$implicit;

          var ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r8.showReqRes(log_r7, ctx_r8.ReqResType.Request);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](11, "i", 53);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "td");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "button", 52);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_tr_62_Template_button_click_13_listener() {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9);

          var log_r7 = ctx.$implicit;

          var ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

          return ctx_r10.showReqRes(log_r7, ctx_r10.ReqResType.Response);
        });

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](14, "i", 54);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }

      if (rf & 2) {
        var log_r7 = ctx.$implicit;

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](log_r7.Url);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](log_r7.ExternalRefID ? log_r7.ExternalRefID : "--");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](log_r7.CreatedDate);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](log_r7.Message == 1 ? "Failed" : "Success");
      }
    }

    function DashboardComponent_ngx_json_viewer_71_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "ngx-json-viewer", 55);
      }

      if (rf & 2) {
        var ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("json", ctx_r4.selectedReqRes);
      }
    }

    function DashboardComponent_div_75_Template(rf, ctx) {
      if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 56);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 57);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "div", 58);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 59);

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Loading...");

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
      }
    }

    var _c0 = function _c0(a0) {
      return {
        "active": a0
      };
    };

    var DashboardComponent = /*#__PURE__*/function () {
      function DashboardComponent(dashbpardSer) {
        _classCallCheck(this, DashboardComponent);

        this.dashbpardSer = dashbpardSer;
        this.ApiResultType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["ApiResultType"];
        this.ReqResType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["ReqResType"];
        this.dtOptions = {};
        this.Log = {};
        this.dtTrigger = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
        this.IsLoading = false;
        this.selectedApi = '';
        this.ApiList = ['Select API', 'Invoice-Create', 'Invoice-UpdateImages', 'Schedule-Create', 'Customer-Create', 'Location-Create'];
        this.viewType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["ApiResultType"].Total; //min max date

        this.MinStartDate = moment__WEBPACK_IMPORTED_MODULE_2__(new Date(new Date().setMonth(new Date().getMonth() - 1)));
        this.MaxStartDate = new Date();
        this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__().format('MM/DD/YYYY');
        this.fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(new Date(new Date().setMonth(new Date().getMonth() - 1))).format('MM/DD/YYYY');
      }

      _createClass(DashboardComponent, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          this.currentCompanyId = currentCompanyId;
          this.initializeGrid();
          this.getAPILogs();
        }
      }, {
        key: "initializeGrid",
        value: function initializeGrid() {
          var exportInvitedColumns = {
            columns: ':visible'
          };
          this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
              /*  { extend: 'colvis' }*/
              //{ extend: 'copy', exportOptions: exportInvitedColumns },
              //{ extend: 'csv', title: 'API Details', exportOptions: exportInvitedColumns },
              //{ extend: 'pdf', title: 'API Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
              //{ extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
          };
        }
      }, {
        key: "ReloadDataTable",
        value: function ReloadDataTable() {
          this.getAPILogs();
        }
      }, {
        key: "getAPILogs",
        value: function getAPILogs() {
          var _this2 = this;

          this.IsLoading = true;

          if (this.datatableElement && this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then(function (dtInstance) {
              dtInstance.destroy();
            });
          }

          this.Log = {};
          if (this.fromDate == undefined) this.fromDate = '';
          if (this.toDate == undefined) this.toDate = '';

          if (this.selectedApi == undefined || this.selectedApi == 'Select API') {
            this.selectedApi = '';
          }

          this.dashbpardSer.getApiLogs(this.currentCompanyId, this.fromDate, this.toDate, this.selectedApi, this.viewType).subscribe(function (data) {
            _this2.IsLoading = false;
            _this2.Log = data;
            _this2.Log && _this2.Log.ApiLogList.map(function (m) {
              m.CreatedDate = moment__WEBPACK_IMPORTED_MODULE_2__(m.CreatedDate).format('MM/DD/YYYY hh:mm a');
            });

            _this2.dtTrigger.next();
          });
        }
      }, {
        key: "showReqRes",
        value: function showReqRes(log, reqResType) {
          var _this3 = this;

          this.IsLoading = true;
          this.selectedReqRes = null;
          this.dashbpardSer.getApiReqRes(log.Id, reqResType).subscribe(function (data) {
            _this3.IsLoading = false;
            reqResType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["ReqResType"].Request ? _this3.selectedReqRes = JSON.parse(data.Request) : _this3.selectedReqRes = JSON.parse(data.Response);
            reqResType == src_app_app_enum__WEBPACK_IMPORTED_MODULE_4__["ReqResType"].Request ? _this3.modelHeader = 'Request (' + log.Url + " )" : _this3.modelHeader = 'Response (' + log.Url + " )";
          }); // reqResType == 2 ? this.selectedReqRes = JSON.parse(log.Request) : this.selectedReqRes = JSON.parse(log.Response);
        }
      }, {
        key: "setFromDate",
        value: function setFromDate(event) {
          this.fromDate = event;
          var d = moment__WEBPACK_IMPORTED_MODULE_2__(new Date(new Date().setMonth(new Date().getMonth() + 1))).toDate();
          !this.toDate ? this.toDate = moment__WEBPACK_IMPORTED_MODULE_2__(d).format('MM/DD/YYYY') : '';

          if (this.fromDate != '' && this.toDate != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate).toDate();

            if (_toDate < _fromDate) {
              this.toDate = event;
            }
          }
        }
      }, {
        key: "setToDate",
        value: function setToDate(event) {
          this.toDate = event;

          if (this.fromDate != '' && this.toDate != '') {
            var _fromDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.fromDate).toDate();

            var _toDate = moment__WEBPACK_IMPORTED_MODULE_2__(this.toDate).toDate();

            if (_fromDate > _toDate) {
              this.fromDate = event;
            }
          }
        }
      }, {
        key: "apiChanged",
        value: function apiChanged($event) {
          this.selectedApi = $event.target.value;
        }
      }, {
        key: "getView",
        value: function getView(type) {
          this.viewType = type;
          this.getAPILogs();
        }
      }]);

      return DashboardComponent;
    }();

    DashboardComponent.ɵfac = function DashboardComponent_Factory(t) {
      return new (t || DashboardComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_service_api_dashboard_service__WEBPACK_IMPORTED_MODULE_5__["ApiDashboardService"]));
    };

    DashboardComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({
      type: DashboardComponent,
      selectors: [["app-dashboard"]],
      viewQuery: function DashboardComponent_Query(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵviewQuery"](angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], true);
        }

        if (rf & 2) {
          var _t;

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵloadQuery"]()) && (ctx.datatableElement = _t.first);
        }
      },
      decls: 76,
      vars: 23,
      consts: [[1, "api-dashboard-container"], [1, "row"], [1, "col-sm-12"], [1, "well", "pb10", "pt15", "mb10", "no-shadow"], [1, "col-xs-12", "col-sm-2", "col-md-1", "pt5", "pr0"], [1, "fa", "fa-filter", "mr5", "fs16"], [1, "f-normal", "fs16"], [1, "col-xs-6", "col-sm-3", "col-md-2", "mb5"], [1, "form-control", 3, "change"], [3, "value", "selected", 4, "ngFor", "ngForOf"], ["type", "text", "placeholder", "Date", "myDatePicker", "", 1, "form-control", "datepicker", 3, "ngModel", "format", "ngModelChange", "onDateChange"], ["fromDate1", ""], ["EndDate1", ""], [1, "col-xs-12", "col-sm-4", "col-md-3"], ["type", "button", "id", "btnApplyFilter", "value", "Apply", 1, "btn", "btn-primary", 3, "click"], [1, "row", "mt10"], [1, "col-sm-2", "col-xs-12", "mb10-xs"], [1, "col-xs-12", "api-cards-container"], [1, "col-sm-12", "col-xs-3", "card", 3, "ngClass", "click"], [1, "digit", "color-default"], [1, "text", "color-default"], [1, "digit", "color-lightgreen"], [1, "text", "color-lightgreen"], [1, "digit", "color-failed"], [1, "text", "color-failed"], [1, "col-sm-10", "col-xs-12"], [1, "well", "bg-white", "shadow-b", "pr"], [1, "pa", "bg-white", "top0", "left0", "z-index5", "loading-wrapper", "schedule-loading-wrapper", "hide-element"], [1, "spinner-dashboard", "pa"], [1, "ibox", "mb0"], [1, "ibox-content", "no-padding", "no-border", "location_table"], [1, "table-responsive"], ["id", "iapi-dashboard-datatable", "data-gridname", "26", "datatable", "", 1, "table", "table-striped", "table-bordered", "table-hover", 3, "dtOptions", "dtTrigger"], ["data-key", "Api_Name"], ["data-key", "External_Ref_ID"], ["data-key", "DateTime"], ["data-key", "Status"], ["data-key", "Request"], ["data-key", "Response"], [4, "ngFor", "ngForOf"], ["id", "idReqResModel", "role", "dialog", "data-backdrop", "static", 1, "modal", "fade"], [1, "modal-dialog", "modal-lg"], [1, "modal-content"], [1, "modal-header"], [1, "modal-title"], [1, "pull-right"], [1, "modal-body"], [3, "json", 4, "ngIf"], [1, "modal-footer"], ["type", "button", "id", "idCloseModel", "data-dismiss", "modal", 1, "btn", "btn-default"], ["class", "loader", 4, "ngIf"], [3, "value", "selected"], ["type", "button", "data-toggle", "modal", "data-target", "#idReqResModel", 1, "btn", "btn-link", 3, "click"], ["alt", "Request", "title", "Request", 1, "fa", "fa-arrow-up"], ["alt", "Response", "title", "Response", 1, "fa", "fa-arrow-down"], [3, "json"], [1, "loader"], [1, "loading-content", "text-center"], [1, "spinner"], [1, "font-bold"]],
      template: function DashboardComponent_Template(rf, ctx) {
        if (rf & 1) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 1);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](6, "i", 5);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "label", 6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Filter");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "select", 8);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function DashboardComponent_Template_select_change_10_listener($event) {
            return ctx.apiChanged($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, DashboardComponent_option_11_Template, 2, 3, "option", 9);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "input", 10, 11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DashboardComponent_Template_input_ngModelChange_13_listener($event) {
            return ctx.fromDate = $event;
          })("onDateChange", function DashboardComponent_Template_input_onDateChange_13_listener($event) {
            return ctx.setFromDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "input", 10, 12);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngModelChange", function DashboardComponent_Template_input_ngModelChange_16_listener($event) {
            return ctx.toDate = $event;
          })("onDateChange", function DashboardComponent_Template_input_onDateChange_16_listener($event) {
            return ctx.setToDate($event);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 13);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "input", 14);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_Template_input_click_19_listener() {
            return ctx.ReloadDataTable();
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 15);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 16);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "a", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_Template_a_click_23_listener() {
            return ctx.getView(ctx.ApiResultType.Total);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "h1", 19);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "span", 20);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Total Calls");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "a", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_Template_a_click_28_listener() {
            return ctx.getView(ctx.ApiResultType.Success);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "h1", 21);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "span", 22);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Success");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "a", 18);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardComponent_Template_a_click_33_listener() {
            return ctx.getView(ctx.ApiResultType.Exception);
          });

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "h1", 23);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "span", 24);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Failed");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div", 25);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 26);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "div", 27);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](41, "span", 28);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "div", 29);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "div", 30);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](44, "div", 31);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "table", 32);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "thead");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "tr");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](48, "th", 33);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](49, "API Name");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "th", 34);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](51, "External Ref ID");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](52, "th", 35);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](53, "DateTime");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "th", 36);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](55, "Status");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](56, "th", 37);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](57, "Request");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](58, "th", 38);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](59, "Response");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](60, "tbody");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](61);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](62, DashboardComponent_tr_62_Template, 15, 4, "tr", 39);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "div", 40);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](64, "div", 41);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](65, "div", 42);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](66, "div", 43);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](67, "h4", 44);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](68);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](69, "span", 45);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](70, "div", 46);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](71, DashboardComponent_ngx_json_viewer_71_Template, 1, 1, "ngx-json-viewer", 47);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "div", 48);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](73, "button", 49);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](74, "Close");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](75, DashboardComponent_div_75_Template, 5, 0, "div", 50);
        }

        if (rf & 2) {
          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.ApiList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.fromDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngModel", ctx.toDate)("format", "MM/DD/YYYY");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](17, _c0, ctx.viewType == ctx.ApiResultType.Total));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.Log.TotalCall);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](19, _c0, ctx.viewType == ctx.ApiResultType.Success));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.Log.SuccessCall);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](21, _c0, ctx.viewType == ctx.ApiResultType.Exception));

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx.Log.FailedCall);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](10);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("dtOptions", ctx.dtOptions)("dtTrigger", ctx.dtTrigger);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](17);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.Log == null ? null : ctx.Log.ApiLogList);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](6);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx.modelHeader, " ");

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.selectedReqRes);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);

          _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.IsLoading);
        }
      },
      directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__["NgForOf"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["DefaultValueAccessor"], _directives_myDateTimePicker__WEBPACK_IMPORTED_MODULE_8__["DatePicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgModel"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgClass"], angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgIf"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_7__["ɵangular_packages_forms_forms_x"], ngx_json_viewer__WEBPACK_IMPORTED_MODULE_9__["NgxJsonViewerComponent"]],
      styles: [".api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%] {\n    border-radius: 10px;\n    padding: 0;\n    background: #ffffff;\n    box-shadow: 0 3px 15px 0 rgba(0,0,0,.1);\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%] {\n        min-height: 20px;\n        padding: 10px 19px;\n        background: #ffffff;\n        text-align: center;\n        border: 0px solid rgba(0,0,0,.125);\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]   .digit[_ngcontent-%COMP%] {\n    font-size: 35px;\n    font-weight: normal;\n    margin: 0;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]   .text[_ngcontent-%COMP%] {\n    font-size: 12px;\n    font-weight: bold;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:first-child {\n    border-top-left-radius: 8px;\n    border-top-right-radius: 8px;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:last-child {\n    border-bottom-left-radius: 8px;\n    border-bottom-right-radius: 8px;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:hover {\n    background: #f2f2f2;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:active {\n    background: #f2f2f2;\n    border-right: 5px solid #e1e0e0;\n}\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card.active[_ngcontent-%COMP%] {\n    background: #f2f2f2;\n    border-right: 5px solid #e1e0e0;\n}\n\n    @media only screen and (max-width : 400px) {\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:active {\n        background: #f2f2f2;\n        border-right: 0;\n        border-bottom: 5px solid #e1e0e0;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card.active[_ngcontent-%COMP%] {\n        background: #f2f2f2;\n        border-right: 0;\n        border-bottom: 5px solid #e1e0e0;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]   .digit[_ngcontent-%COMP%] {\n        font-size: 30px;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%] {\n        padding: 10px;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%] {\n        padding: 0;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:first-child {\n        border-top-left-radius: 8px;\n        border-bottom-left-radius: 8px;\n    }\n\n    .api-dashboard-container[_ngcontent-%COMP%]   .api-cards-container[_ngcontent-%COMP%]   .card[_ngcontent-%COMP%]:last-child {\n        border-top-right-radius: 8px;\n        border-bottom-right-radius: 8px;\n    }\n\n   \n}\n\n    #idReqResModel[_ngcontent-%COMP%]   .modal-body[_ngcontent-%COMP%] {\n    max-height: calc(100vh - 212px);\n    overflow-y: auto;\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvdHBkLWFwaS1kYXNoYm9hcmQvZGFzaGJvYXJkL2Rhc2hib2FyZC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksbUJBQW1CO0lBQ25CLFVBQVU7SUFDVixtQkFBbUI7SUFDbkIsdUNBQXVDO0FBQzNDOztJQUVJO1FBQ0ksZ0JBQWdCO1FBQ2hCLGtCQUFrQjtRQUNsQixtQkFBbUI7UUFDbkIsa0JBQWtCO1FBQ2xCLGtDQUFrQztJQUN0Qzs7SUFFSjtJQUNJLGVBQWU7SUFDZixtQkFBbUI7SUFDbkIsU0FBUztBQUNiOztJQUNBO0lBQ0ksZUFBZTtJQUNmLGlCQUFpQjtBQUNyQjs7SUFFQTtJQUNJLDJCQUEyQjtJQUMzQiw0QkFBNEI7QUFDaEM7O0lBRUE7SUFDSSw4QkFBOEI7SUFDOUIsK0JBQStCO0FBQ25DOztJQUVBO0lBQ0ksbUJBQW1CO0FBQ3ZCOztJQUVBO0lBQ0ksbUJBQW1CO0lBQ25CLCtCQUErQjtBQUNuQzs7SUFFQTtJQUNJLG1CQUFtQjtJQUNuQiwrQkFBK0I7QUFDbkM7O0lBRUE7O0lBRUk7UUFDSSxtQkFBbUI7UUFDbkIsZUFBZTtRQUNmLGdDQUFnQztJQUNwQzs7SUFFQTtRQUNJLG1CQUFtQjtRQUNuQixlQUFlO1FBQ2YsZ0NBQWdDO0lBQ3BDOztJQUVBO1FBQ0ksZUFBZTtJQUNuQjs7SUFFQTtRQUNJLGFBQWE7SUFDakI7O0lBRUE7UUFDSSxVQUFVO0lBQ2Q7O0lBRUE7UUFDSSwyQkFBMkI7UUFDM0IsOEJBQThCO0lBQ2xDOztJQUVBO1FBQ0ksNEJBQTRCO1FBQzVCLCtCQUErQjtJQUNuQzs7O0FBR0o7O0lBQ0E7SUFDSSwrQkFBK0I7SUFDL0IsZ0JBQWdCO0FBQ3BCIiwiZmlsZSI6InNyYy9hcHAvdHBkLWFwaS1kYXNoYm9hcmQvZGFzaGJvYXJkL2Rhc2hib2FyZC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmFwaS1kYXNoYm9hcmQtY29udGFpbmVyIC5hcGktY2FyZHMtY29udGFpbmVyIHtcbiAgICBib3JkZXItcmFkaXVzOiAxMHB4O1xuICAgIHBhZGRpbmc6IDA7XG4gICAgYmFja2dyb3VuZDogI2ZmZmZmZjtcbiAgICBib3gtc2hhZG93OiAwIDNweCAxNXB4IDAgcmdiYSgwLDAsMCwuMSk7XG59XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQge1xuICAgICAgICBtaW4taGVpZ2h0OiAyMHB4O1xuICAgICAgICBwYWRkaW5nOiAxMHB4IDE5cHg7XG4gICAgICAgIGJhY2tncm91bmQ6ICNmZmZmZmY7XG4gICAgICAgIHRleHQtYWxpZ246IGNlbnRlcjtcbiAgICAgICAgYm9yZGVyOiAwcHggc29saWQgcmdiYSgwLDAsMCwuMTI1KTtcbiAgICB9XG5cbi5hcGktZGFzaGJvYXJkLWNvbnRhaW5lciAuYXBpLWNhcmRzLWNvbnRhaW5lciAuY2FyZCAuZGlnaXQge1xuICAgIGZvbnQtc2l6ZTogMzVweDtcbiAgICBmb250LXdlaWdodDogbm9ybWFsO1xuICAgIG1hcmdpbjogMDtcbn1cbi5hcGktZGFzaGJvYXJkLWNvbnRhaW5lciAuYXBpLWNhcmRzLWNvbnRhaW5lciAuY2FyZCAudGV4dCB7XG4gICAgZm9udC1zaXplOiAxMnB4O1xuICAgIGZvbnQtd2VpZ2h0OiBib2xkO1xufVxuXG4uYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQ6Zmlyc3QtY2hpbGQge1xuICAgIGJvcmRlci10b3AtbGVmdC1yYWRpdXM6IDhweDtcbiAgICBib3JkZXItdG9wLXJpZ2h0LXJhZGl1czogOHB4O1xufVxuXG4uYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQ6bGFzdC1jaGlsZCB7XG4gICAgYm9yZGVyLWJvdHRvbS1sZWZ0LXJhZGl1czogOHB4O1xuICAgIGJvcmRlci1ib3R0b20tcmlnaHQtcmFkaXVzOiA4cHg7XG59XG5cbi5hcGktZGFzaGJvYXJkLWNvbnRhaW5lciAuYXBpLWNhcmRzLWNvbnRhaW5lciAuY2FyZDpob3ZlciB7XG4gICAgYmFja2dyb3VuZDogI2YyZjJmMjtcbn1cblxuLmFwaS1kYXNoYm9hcmQtY29udGFpbmVyIC5hcGktY2FyZHMtY29udGFpbmVyIC5jYXJkOmFjdGl2ZSB7XG4gICAgYmFja2dyb3VuZDogI2YyZjJmMjtcbiAgICBib3JkZXItcmlnaHQ6IDVweCBzb2xpZCAjZTFlMGUwO1xufVxuXG4uYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQuYWN0aXZlIHtcbiAgICBiYWNrZ3JvdW5kOiAjZjJmMmYyO1xuICAgIGJvcmRlci1yaWdodDogNXB4IHNvbGlkICNlMWUwZTA7XG59XG5cbkBtZWRpYSBvbmx5IHNjcmVlbiBhbmQgKG1heC13aWR0aCA6IDQwMHB4KSB7XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQ6YWN0aXZlIHtcbiAgICAgICAgYmFja2dyb3VuZDogI2YyZjJmMjtcbiAgICAgICAgYm9yZGVyLXJpZ2h0OiAwO1xuICAgICAgICBib3JkZXItYm90dG9tOiA1cHggc29saWQgI2UxZTBlMDtcbiAgICB9XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQuYWN0aXZlIHtcbiAgICAgICAgYmFja2dyb3VuZDogI2YyZjJmMjtcbiAgICAgICAgYm9yZGVyLXJpZ2h0OiAwO1xuICAgICAgICBib3JkZXItYm90dG9tOiA1cHggc29saWQgI2UxZTBlMDtcbiAgICB9XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQgLmRpZ2l0IHtcbiAgICAgICAgZm9udC1zaXplOiAzMHB4O1xuICAgIH1cblxuICAgIC5hcGktZGFzaGJvYXJkLWNvbnRhaW5lciAuYXBpLWNhcmRzLWNvbnRhaW5lciAuY2FyZCB7XG4gICAgICAgIHBhZGRpbmc6IDEwcHg7XG4gICAgfVxuXG4gICAgLmFwaS1kYXNoYm9hcmQtY29udGFpbmVyIC5hcGktY2FyZHMtY29udGFpbmVyIHtcbiAgICAgICAgcGFkZGluZzogMDtcbiAgICB9XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQ6Zmlyc3QtY2hpbGQge1xuICAgICAgICBib3JkZXItdG9wLWxlZnQtcmFkaXVzOiA4cHg7XG4gICAgICAgIGJvcmRlci1ib3R0b20tbGVmdC1yYWRpdXM6IDhweDtcbiAgICB9XG5cbiAgICAuYXBpLWRhc2hib2FyZC1jb250YWluZXIgLmFwaS1jYXJkcy1jb250YWluZXIgLmNhcmQ6bGFzdC1jaGlsZCB7XG4gICAgICAgIGJvcmRlci10b3AtcmlnaHQtcmFkaXVzOiA4cHg7XG4gICAgICAgIGJvcmRlci1ib3R0b20tcmlnaHQtcmFkaXVzOiA4cHg7XG4gICAgfVxuXG4gICBcbn1cbiNpZFJlcVJlc01vZGVsIC5tb2RhbC1ib2R5IHtcbiAgICBtYXgtaGVpZ2h0OiBjYWxjKDEwMHZoIC0gMjEycHgpO1xuICAgIG92ZXJmbG93LXk6IGF1dG87XG59Il19 */"]
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DashboardComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
          selector: 'app-dashboard',
          templateUrl: './dashboard.component.html',
          styleUrls: ['./dashboard.component.css']
        }]
      }], function () {
        return [{
          type: _service_api_dashboard_service__WEBPACK_IMPORTED_MODULE_5__["ApiDashboardService"]
        }];
      }, {
        datatableElement: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"],
          args: [angular_datatables__WEBPACK_IMPORTED_MODULE_3__["DataTableDirective"]]
        }]
      });
    })();

    var ApiDashBoardModel = function ApiDashBoardModel() {
      _classCallCheck(this, ApiDashBoardModel);
    };

    var ApiDetailModel = function ApiDetailModel() {
      _classCallCheck(this, ApiDetailModel);

      this.ApiLogList = [];
    };
    /***/

  },

  /***/
  "./src/app/tpd-api-dashboard/service/api-dashboard.service.ts": function srcAppTpdApiDashboardServiceApiDashboardServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ApiDashboardService", function () {
      return ApiDashboardService;
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

    var ApiDashboardService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(ApiDashboardService, _src_app_errors_Handl);

      var _super = _createSuper(ApiDashboardService);

      function ApiDashboardService(httpClient) {
        var _this4;

        _classCallCheck(this, ApiDashboardService);

        _this4 = _super.call(this);
        _this4.httpClient = httpClient;
        _this4.getAllLogUrl = '/Home/GetApiLogs?companyId=';
        _this4.getReqResUrl = '/Home/GetApiLogRequestResponse?id=';
        return _this4;
      }

      _createClass(ApiDashboardService, [{
        key: "getApiLogs",
        value: function getApiLogs(currentCompanyId, fromDate, toDate, selectedApi, viewType) {
          return this.httpClient.get(this.getAllLogUrl + currentCompanyId + "&fromdate=" + fromDate + "&toDate=" + toDate + "&viewType=" + viewType + "&apiName=" + selectedApi).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getApiLogs', null)));
        }
      }, {
        key: "getApiReqRes",
        value: function getApiReqRes(id, reqResType) {
          return this.httpClient.get(this.getReqResUrl + id + "&ReqResType=" + reqResType).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetApiLogRequestResponse', null)));
        }
      }]);

      return ApiDashboardService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    ApiDashboardService.ɵfac = function ApiDashboardService_Factory(t) {
      return new (t || ApiDashboardService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    ApiDashboardService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: ApiDashboardService,
      factory: ApiDashboardService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ApiDashboardService, [{
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
  "./src/app/tpd-api-dashboard/tpd-api-dashboard.module.ts": function srcAppTpdApiDashboardTpdApiDashboardModuleTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "TpdApiDashboardModule", function () {
      return TpdApiDashboardModule;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ./dashboard/dashboard.component */
    "./src/app/tpd-api-dashboard/dashboard/dashboard.component.ts");
    /* harmony import */


    var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/router */
    "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
    /* harmony import */


    var _modules_shared_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! ../modules/shared.module */
    "./src/app/modules/shared.module.ts");
    /* harmony import */


    var ngx_json_viewer__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! ngx-json-viewer */
    "./node_modules/ngx-json-viewer/__ivy_ngcc__/ngx-json-viewer.js");
    /* harmony import */


    var _modules_directive_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(
    /*! ../modules/directive.module */
    "./src/app/modules/directive.module.ts");
    /* harmony import */


    var angular_datatables__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(
    /*! angular-datatables */
    "./node_modules/angular-datatables/__ivy_ngcc__/index.js");

    var routeTpd = [{
      path: '',
      component: _dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["DashboardComponent"]
    }];

    var TpdApiDashboardModule = function TpdApiDashboardModule() {
      _classCallCheck(this, TpdApiDashboardModule);
    };

    TpdApiDashboardModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: TpdApiDashboardModule
    });
    TpdApiDashboardModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function TpdApiDashboardModule_Factory(t) {
        return new (t || TpdApiDashboardModule)();
      },
      imports: [[_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"], ngx_json_viewer__WEBPACK_IMPORTED_MODULE_4__["NgxJsonViewerModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(routeTpd)]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](TpdApiDashboardModule, {
        declarations: [_dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["DashboardComponent"]],
        imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"], ngx_json_viewer__WEBPACK_IMPORTED_MODULE_4__["NgxJsonViewerModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](TpdApiDashboardModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          declarations: [_dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_1__["DashboardComponent"]],
          imports: [_modules_shared_module__WEBPACK_IMPORTED_MODULE_3__["SharedModule"], _modules_directive_module__WEBPACK_IMPORTED_MODULE_5__["DirectiveModule"], ngx_json_viewer__WEBPACK_IMPORTED_MODULE_4__["NgxJsonViewerModule"], angular_datatables__WEBPACK_IMPORTED_MODULE_6__["DataTablesModule"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(routeTpd)]
        }]
      }], null, null);
    })();
    /***/

  }
}]);
//# sourceMappingURL=tpd-api-dashboard-tpd-api-dashboard-module-es5.js.map