function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (typeof call === "object" || typeof call === "function")) { return call; } else if (call !== void 0) { throw new TypeError("Derived constructors may only return object or undefined"); } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~create-fuel-group-create-fuel-group-module~freightRates-freight-rate-module~self-service-ali~aec626c7"], {
  /***/
  "./src/app/self-service-alias/service/externalmappings.service.ts": function srcAppSelfServiceAliasServiceExternalmappingsServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ExternalMappingsService", function () {
      return ExternalMappingsService;
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

    var ExternalMappingsService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(ExternalMappingsService, _src_app_errors_Handl);

      var _super = _createSuper(ExternalMappingsService);

      function ExternalMappingsService(httpClient) {
        var _this;

        _classCallCheck(this, ExternalMappingsService);

        _this = _super.call(this);
        _this.httpClient = httpClient;
        _this.urlGetExternalCustomers = '/ExternalEntityMappings/GetExternalCompanies';
        _this.urlGetCustomersForExternalMapping = '/ExternalEntityMappings/GetCustomersForExternalMapping';
        _this.urlSaveCustomerExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerMappings';
        _this.urlBulkUploadCustomerMapping = '/ExternalEntityMappings/BulkUploadCustomerMapping';
        _this.urlGetCustomerLocationsForExternalMapping = '/ExternalEntityMappings/GetCustomerLocationsForExternalMapping';
        _this.urlSaveCustomerLocationExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerLocationMappings';
        _this.urlBulkUploadCustomerLocationMapping = '/ExternalEntityMappings/BulkUploadCustomerLocationMapping';
        _this.urlGetProductsForExternalMapping = '/ExternalEntityMappings/GetProductsForExternalMapping';
        _this.urlSaveExternalProductMappings = '/ExternalEntityMappings/SaveExternalProductMappings';
        _this.urlBulkUploadProductMapping = '/ExternalEntityMappings/BulkUploadProductMapping';
        _this.urlGetSuppliersForExternalMapping = '/ExternalEntityMappings/GetSuppliersForExternalMapping';
        _this.urlSaveExternalSupplierMappings = '/ExternalEntityMappings/SaveExternalSupplierMappings';
        _this.urlBulkUploadSupplierMapping = '/ExternalEntityMappings/BulkUploadSupplierMapping';
        _this.urlGetTerminalsForExternalMapping = '/ExternalEntityMappings/GetTerminalsForExternalMapping';
        _this.urlSaveExternalTerminalMappings = '/ExternalEntityMappings/SaveExternalTerminalMappings';
        _this.urlBulkUploadTerminalMapping = '/ExternalEntityMappings/BulkUploadTerminalMapping';
        _this.urlGetBulkPlantsForExternalMapping = '/ExternalEntityMappings/GetBulkPlantsForExternalMapping';
        _this.urlSaveExternalBulkPlantMappings = '/ExternalEntityMappings/SaveExternalBulkPlantMappings';
        _this.urlBulkUploadBulkPlantMapping = '/ExternalEntityMappings/BulkUploadBulkPlantMapping';
        _this.urlGetDriversForExternalMapping = '/ExternalEntityMappings/GetDriversForExternalMapping';
        _this.urlSaveExternalDriverMappings = '/ExternalEntityMappings/SaveExternalDriverMappings';
        _this.urlBulkUploadDriverMapping = '/ExternalEntityMappings/BulkUploadDriverMapping';
        _this.urlGetCarriersForExternalMapping = '/ExternalEntityMappings/GetCarriersForExternalMapping';
        _this.urlSaveExternalCarrierMappings = '/ExternalEntityMappings/SaveExternalCarrierMappings';
        _this.urlBulkUploadCarrierMapping = '/ExternalEntityMappings/BulkUploadCarrierMapping';
        _this.urlGetGetVehiclesForExternalMapping = '/ExternalEntityMappings/GetVehiclesForExternalMapping';
        _this.urlSaveExternalVehicleMappings = '/ExternalEntityMappings/SaveExternalVehicleMappings';
        _this.urlBulkUploadVehicleMapping = '/ExternalEntityMappings/BulkUploadVehicleMapping';
        _this.urlGetFuelGroupSummary = '/FuelGroup/GetFuelGroupSummary';
        _this.urlArchiveFuelGroup = '/FuelGroup/ArchiveFuelGroup';
        _this.getFuelTypesUrl = '/FuelGroup/GetFuelTypes?productTypeIds=';
        _this.getProductTypeUrl = '/FuelGroup/GetProductTypes';
        _this.getFuelGroupDetailsUrl = '/FuelGroup/GetFuelGroupDetails?fuelGroupId=';
        _this.getFuelGroupsUrl = "/FuelGroup/GetFuelGroups?fuelGroupType=";
        return _this;
      }

      _createClass(ExternalMappingsService, [{
        key: "getExternalCompanies",
        value: function getExternalCompanies() {
          return this.httpClient.get(this.urlGetExternalCustomers).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getExternalCompanies', null)));
        }
      }, {
        key: "getCustomersForExternalMapping",
        value: function getCustomersForExternalMapping() {
          return this.httpClient.get(this.urlGetCustomersForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCustomersForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalCustomerMappings",
        value: function SaveExternalCustomerMappings(customerDetails) {
          return this.httpClient.post(this.urlSaveCustomerExternalMappings, customerDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveCustomerExternalMappings', null)));
        }
      }, {
        key: "getCustomerLocationsForExternalMapping",
        value: function getCustomerLocationsForExternalMapping() {
          return this.httpClient.get(this.urlGetCustomerLocationsForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCustomerLocationsForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalCustomerLocationMappings",
        value: function SaveExternalCustomerLocationMappings(locationDetails) {
          return this.httpClient.post(this.urlSaveCustomerLocationExternalMappings, locationDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalCustomerLocationMappings', null)));
        }
      }, {
        key: "getProductsForExternalMapping",
        value: function getProductsForExternalMapping() {
          return this.httpClient.get(this.urlGetProductsForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductsForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalProductMappings",
        value: function SaveExternalProductMappings(productDetails) {
          return this.httpClient.post(this.urlSaveExternalProductMappings, productDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalProductMappings', null)));
        }
      }, {
        key: "getSuppliersForExternalMapping",
        value: function getSuppliersForExternalMapping() {
          return this.httpClient.get(this.urlGetSuppliersForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSuppliersForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalSupplierMappings",
        value: function SaveExternalSupplierMappings(supplierDetails) {
          return this.httpClient.post(this.urlSaveExternalSupplierMappings, supplierDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalSupplierMappings', null)));
        }
      }, {
        key: "getTerminalsForExternalMapping",
        value: function getTerminalsForExternalMapping() {
          return this.httpClient.get(this.urlGetTerminalsForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTerminalsForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalTerminalMappings",
        value: function SaveExternalTerminalMappings(terminalDetails) {
          return this.httpClient.post(this.urlSaveExternalTerminalMappings, terminalDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalTerminalMappings', null)));
        }
      }, {
        key: "getBulkPlantsForExternalMapping",
        value: function getBulkPlantsForExternalMapping() {
          return this.httpClient.get(this.urlGetBulkPlantsForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBulkPlantsForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalBulkPlantMappings",
        value: function SaveExternalBulkPlantMappings(bulkPlantDetails) {
          return this.httpClient.post(this.urlSaveExternalBulkPlantMappings, bulkPlantDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalBulkPlantMappings', null)));
        }
      }, {
        key: "getDriversForExternalMapping",
        value: function getDriversForExternalMapping() {
          return this.httpClient.get(this.urlGetDriversForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetDriversForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalDriverMappings",
        value: function SaveExternalDriverMappings(driverDetails) {
          return this.httpClient.post(this.urlSaveExternalDriverMappings, driverDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalDriverMappings', null)));
        }
      }, {
        key: "getCarriersForExternalMapping",
        value: function getCarriersForExternalMapping() {
          return this.httpClient.get(this.urlGetCarriersForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriersForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalCarrierMappings",
        value: function SaveExternalCarrierMappings(carrierDetails) {
          return this.httpClient.post(this.urlSaveExternalCarrierMappings, carrierDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalCarrierMappings', null)));
        }
      }, {
        key: "getVehiclesForExternalMapping",
        value: function getVehiclesForExternalMapping() {
          return this.httpClient.get(this.urlGetGetVehiclesForExternalMapping).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getVehiclesForExternalMapping', null)));
        }
      }, {
        key: "SaveExternalVehicleMappings",
        value: function SaveExternalVehicleMappings(vehicleDetails) {
          return this.httpClient.post(this.urlSaveExternalVehicleMappings, vehicleDetails).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalVehicleMappings', null)));
        }
      }, {
        key: "BulkUploadCustomerMapping",
        value: function BulkUploadCustomerMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadCustomerMapping, formData);
        }
      }, {
        key: "BulkUploadCustomerLocationMapping",
        value: function BulkUploadCustomerLocationMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadCustomerLocationMapping, formData);
        }
      }, {
        key: "BulkUploadProductMapping",
        value: function BulkUploadProductMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadProductMapping, formData);
        }
      }, {
        key: "BulkUploadSupplierMapping",
        value: function BulkUploadSupplierMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadSupplierMapping, formData);
        }
      }, {
        key: "BulkUploadBulkPlantMapping",
        value: function BulkUploadBulkPlantMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadBulkPlantMapping, formData);
        }
      }, {
        key: "BulkUploadTerminalMapping",
        value: function BulkUploadTerminalMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadTerminalMapping, formData);
        }
      }, {
        key: "BulkUploadCarrierMapping",
        value: function BulkUploadCarrierMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadCarrierMapping, formData);
        }
      }, {
        key: "BulkUploadDriverMapping",
        value: function BulkUploadDriverMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadDriverMapping, formData);
        }
      }, {
        key: "BulkUploadVehicleMapping",
        value: function BulkUploadVehicleMapping(file) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          return this.httpClient.post(this.urlBulkUploadVehicleMapping, formData);
        }
      }, {
        key: "getFuelGroupSummary",
        value: function getFuelGroupSummary() {
          return this.httpClient.get(this.urlGetFuelGroupSummary).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroupSummary', null)));
        }
      }, {
        key: "archiveFuelGroup",
        value: function archiveFuelGroup(fuelGroupId) {
          return this.httpClient.post(this.urlArchiveFuelGroup, {
            fuelGroupId: fuelGroupId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('archiveFuelGroup', null)));
        }
      }, {
        key: "getFuelTypeList",
        value: function getFuelTypeList(productTypeIds, fuelGroupType, editingGroupId, editingcompanyId) {
          return this.httpClient.get(this.getFuelTypesUrl + productTypeIds + "&fuelGroupType=" + fuelGroupType + "&editingGroupId=" + editingGroupId + "&editingcompanyId=" + editingcompanyId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelTypeList', null)));
        }
      }, {
        key: "getProductTypeList",
        value: function getProductTypeList() {
          return this.httpClient.get(this.getProductTypeUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductTypeList', null)));
        }
      }, {
        key: "getFuelGroup",
        value: function getFuelGroup(groupId) {
          return this.httpClient.get(this.getFuelGroupDetailsUrl + groupId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroup', null)));
        }
      }, {
        key: "getFuelGroups",
        value: function getFuelGroups(fuelGroupType, companyIds) {
          return this.httpClient.get(this.getFuelGroupsUrl + fuelGroupType + "&companyIds=" + companyIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroups', null)));
        }
      }]);

      return ExternalMappingsService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    ExternalMappingsService.ɵfac = function ExternalMappingsService_Factory(t) {
      return new (t || ExternalMappingsService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    ExternalMappingsService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: ExternalMappingsService,
      factory: ExternalMappingsService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalMappingsService, [{
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

  }
}]);
//# sourceMappingURL=default~create-fuel-group-create-fuel-group-module~freightRates-freight-rate-module~self-service-ali~aec626c7-es5.js.map