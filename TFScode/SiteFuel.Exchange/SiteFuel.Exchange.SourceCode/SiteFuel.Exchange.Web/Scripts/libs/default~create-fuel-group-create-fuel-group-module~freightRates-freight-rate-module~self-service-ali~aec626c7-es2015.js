(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~create-fuel-group-create-fuel-group-module~freightRates-freight-rate-module~self-service-ali~aec626c7"],{

/***/ "./src/app/self-service-alias/service/externalmappings.service.ts":
/*!************************************************************************!*\
  !*** ./src/app/self-service-alias/service/externalmappings.service.ts ***!
  \************************************************************************/
/*! exports provided: ExternalMappingsService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ExternalMappingsService", function() { return ExternalMappingsService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class ExternalMappingsService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.urlGetExternalCustomers = '/ExternalEntityMappings/GetExternalCompanies';
        this.urlGetCustomersForExternalMapping = '/ExternalEntityMappings/GetCustomersForExternalMapping';
        this.urlSaveCustomerExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerMappings';
        this.urlBulkUploadCustomerMapping = '/ExternalEntityMappings/BulkUploadCustomerMapping';
        this.urlGetCustomerLocationsForExternalMapping = '/ExternalEntityMappings/GetCustomerLocationsForExternalMapping';
        this.urlSaveCustomerLocationExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerLocationMappings';
        this.urlBulkUploadCustomerLocationMapping = '/ExternalEntityMappings/BulkUploadCustomerLocationMapping';
        this.urlGetProductsForExternalMapping = '/ExternalEntityMappings/GetProductsForExternalMapping';
        this.urlSaveExternalProductMappings = '/ExternalEntityMappings/SaveExternalProductMappings';
        this.urlBulkUploadProductMapping = '/ExternalEntityMappings/BulkUploadProductMapping';
        this.urlGetSuppliersForExternalMapping = '/ExternalEntityMappings/GetSuppliersForExternalMapping';
        this.urlSaveExternalSupplierMappings = '/ExternalEntityMappings/SaveExternalSupplierMappings';
        this.urlBulkUploadSupplierMapping = '/ExternalEntityMappings/BulkUploadSupplierMapping';
        this.urlGetTerminalsForExternalMapping = '/ExternalEntityMappings/GetTerminalsForExternalMapping';
        this.urlSaveExternalTerminalMappings = '/ExternalEntityMappings/SaveExternalTerminalMappings';
        this.urlBulkUploadTerminalMapping = '/ExternalEntityMappings/BulkUploadTerminalMapping';
        this.urlGetBulkPlantsForExternalMapping = '/ExternalEntityMappings/GetBulkPlantsForExternalMapping';
        this.urlSaveExternalBulkPlantMappings = '/ExternalEntityMappings/SaveExternalBulkPlantMappings';
        this.urlBulkUploadBulkPlantMapping = '/ExternalEntityMappings/BulkUploadBulkPlantMapping';
        this.urlGetDriversForExternalMapping = '/ExternalEntityMappings/GetDriversForExternalMapping';
        this.urlSaveExternalDriverMappings = '/ExternalEntityMappings/SaveExternalDriverMappings';
        this.urlBulkUploadDriverMapping = '/ExternalEntityMappings/BulkUploadDriverMapping';
        this.urlGetCarriersForExternalMapping = '/ExternalEntityMappings/GetCarriersForExternalMapping';
        this.urlSaveExternalCarrierMappings = '/ExternalEntityMappings/SaveExternalCarrierMappings';
        this.urlBulkUploadCarrierMapping = '/ExternalEntityMappings/BulkUploadCarrierMapping';
        this.urlGetGetVehiclesForExternalMapping = '/ExternalEntityMappings/GetVehiclesForExternalMapping';
        this.urlSaveExternalVehicleMappings = '/ExternalEntityMappings/SaveExternalVehicleMappings';
        this.urlBulkUploadVehicleMapping = '/ExternalEntityMappings/BulkUploadVehicleMapping';
        this.urlGetFuelGroupSummary = '/FuelGroup/GetFuelGroupSummary';
        this.urlArchiveFuelGroup = '/FuelGroup/ArchiveFuelGroup';
        this.getFuelTypesUrl = '/FuelGroup/GetFuelTypes?productTypeIds=';
        this.getProductTypeUrl = '/FuelGroup/GetProductTypes';
        this.getFuelGroupDetailsUrl = '/FuelGroup/GetFuelGroupDetails?fuelGroupId=';
        this.getFuelGroupsUrl = "/FuelGroup/GetFuelGroups?fuelGroupType=";
    }
    getExternalCompanies() {
        return this.httpClient.get(this.urlGetExternalCustomers)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getExternalCompanies', null)));
    }
    getCustomersForExternalMapping() {
        return this.httpClient.get(this.urlGetCustomersForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCustomersForExternalMapping', null)));
    }
    SaveExternalCustomerMappings(customerDetails) {
        return this.httpClient.post(this.urlSaveCustomerExternalMappings, customerDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveCustomerExternalMappings', null)));
    }
    getCustomerLocationsForExternalMapping() {
        return this.httpClient.get(this.urlGetCustomerLocationsForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCustomerLocationsForExternalMapping', null)));
    }
    SaveExternalCustomerLocationMappings(locationDetails) {
        return this.httpClient.post(this.urlSaveCustomerLocationExternalMappings, locationDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalCustomerLocationMappings', null)));
    }
    getProductsForExternalMapping() {
        return this.httpClient.get(this.urlGetProductsForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductsForExternalMapping', null)));
    }
    SaveExternalProductMappings(productDetails) {
        return this.httpClient.post(this.urlSaveExternalProductMappings, productDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalProductMappings', null)));
    }
    getSuppliersForExternalMapping() {
        return this.httpClient.get(this.urlGetSuppliersForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getSuppliersForExternalMapping', null)));
    }
    SaveExternalSupplierMappings(supplierDetails) {
        return this.httpClient.post(this.urlSaveExternalSupplierMappings, supplierDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalSupplierMappings', null)));
    }
    getTerminalsForExternalMapping() {
        return this.httpClient.get(this.urlGetTerminalsForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getTerminalsForExternalMapping', null)));
    }
    SaveExternalTerminalMappings(terminalDetails) {
        return this.httpClient.post(this.urlSaveExternalTerminalMappings, terminalDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalTerminalMappings', null)));
    }
    getBulkPlantsForExternalMapping() {
        return this.httpClient.get(this.urlGetBulkPlantsForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getBulkPlantsForExternalMapping', null)));
    }
    SaveExternalBulkPlantMappings(bulkPlantDetails) {
        return this.httpClient.post(this.urlSaveExternalBulkPlantMappings, bulkPlantDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalBulkPlantMappings', null)));
    }
    getDriversForExternalMapping() {
        return this.httpClient.get(this.urlGetDriversForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('GetDriversForExternalMapping', null)));
    }
    SaveExternalDriverMappings(driverDetails) {
        return this.httpClient.post(this.urlSaveExternalDriverMappings, driverDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalDriverMappings', null)));
    }
    getCarriersForExternalMapping() {
        return this.httpClient.get(this.urlGetCarriersForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getCarriersForExternalMapping', null)));
    }
    SaveExternalCarrierMappings(carrierDetails) {
        return this.httpClient.post(this.urlSaveExternalCarrierMappings, carrierDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalCarrierMappings', null)));
    }
    getVehiclesForExternalMapping() {
        return this.httpClient.get(this.urlGetGetVehiclesForExternalMapping)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getVehiclesForExternalMapping', null)));
    }
    SaveExternalVehicleMappings(vehicleDetails) {
        return this.httpClient.post(this.urlSaveExternalVehicleMappings, vehicleDetails)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('SaveExternalVehicleMappings', null)));
    }
    BulkUploadCustomerMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCustomerMapping, formData);
    }
    BulkUploadCustomerLocationMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCustomerLocationMapping, formData);
    }
    BulkUploadProductMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadProductMapping, formData);
    }
    BulkUploadSupplierMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadSupplierMapping, formData);
    }
    BulkUploadBulkPlantMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadBulkPlantMapping, formData);
    }
    BulkUploadTerminalMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadTerminalMapping, formData);
    }
    BulkUploadCarrierMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCarrierMapping, formData);
    }
    BulkUploadDriverMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadDriverMapping, formData);
    }
    BulkUploadVehicleMapping(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadVehicleMapping, formData);
    }
    getFuelGroupSummary() {
        return this.httpClient.get(this.urlGetFuelGroupSummary)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroupSummary', null)));
    }
    archiveFuelGroup(fuelGroupId) {
        return this.httpClient.post(this.urlArchiveFuelGroup, { fuelGroupId: fuelGroupId })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('archiveFuelGroup', null)));
    }
    getFuelTypeList(productTypeIds, fuelGroupType, editingGroupId, editingcompanyId) {
        return this.httpClient.get(this.getFuelTypesUrl + productTypeIds + "&fuelGroupType=" + fuelGroupType + "&editingGroupId=" + editingGroupId + "&editingcompanyId=" + editingcompanyId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelTypeList', null)));
    }
    getProductTypeList() {
        return this.httpClient.get(this.getProductTypeUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getProductTypeList', null)));
    }
    getFuelGroup(groupId) {
        return this.httpClient.get(this.getFuelGroupDetailsUrl + groupId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroup', null)));
    }
    getFuelGroups(fuelGroupType, companyIds) {
        return this.httpClient.get(this.getFuelGroupsUrl + fuelGroupType + "&companyIds=" + companyIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(this.handleError('getFuelGroups', null)));
    }
}
ExternalMappingsService.ɵfac = function ExternalMappingsService_Factory(t) { return new (t || ExternalMappingsService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
ExternalMappingsService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: ExternalMappingsService, factory: ExternalMappingsService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ExternalMappingsService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ })

}]);
//# sourceMappingURL=default~create-fuel-group-create-fuel-group-module~freightRates-freight-rate-module~self-service-ali~aec626c7-es2015.js.map