(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["common"],{

/***/ "./src/app/carrier-companies/service/assigncarrier.service.ts":
/*!********************************************************************!*\
  !*** ./src/app/carrier-companies/service/assigncarrier.service.ts ***!
  \********************************************************************/
/*! exports provided: AssigncarrierService, DropdownItem, JobWithEmails, DriverModel, Carrier, CarrierJob, EditFreightOnlyOrder, CarrierJobDetails */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AssigncarrierService", function() { return AssigncarrierService; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DropdownItem", function() { return DropdownItem; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "JobWithEmails", function() { return JobWithEmails; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DriverModel", function() { return DriverModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Carrier", function() { return Carrier; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CarrierJob", function() { return CarrierJob; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EditFreightOnlyOrder", function() { return EditFreightOnlyOrder; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CarrierJobDetails", function() { return CarrierJobDetails; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");






const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class AssigncarrierService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.carrierUrl = '/Settings/Profile/GetCarriers';
        this.carrierUsersUrl = '/Settings/Profile/GetAssignedCarriers';
        this.jobsUrl = '/Settings/Profile/GetJobsForSupplierToCarrier';
        this.assignedCarriersUrl = '/Settings/Profile/GetAssignedCarriersForSupplier';
        this.createUrl = '/Settings/Profile/AssignCarriers';
        this.updateUrl = '/Settings/Profile/UpdateAssignedCarrier';
        this.deleteUrl = '/Settings/Profile/DeleteAssignedCarrier';
        this.createFreightOrderUrl = '/Carrier/Order/createFreightOrdersForAssignedCarrier';
        this.editFreightOnlyOrderUrl = '/Carrier/Order/EditFreightOnlyOrders';
        this.closeFreightOnlyOrderUrl = '/Carrier/Order/closeAssignedOrdersforCarrier';
        this.GetCarrierUserEmailsUrl = '/Supplier/Order/GetCarrierUserEmails';
        this.BulkUploadCarrierUrl = '/Settings/Profile/BulkUploadCarrier';
    }
    GetCarrierUserEmails(assignedCarrierCompanyId) {
        return this.httpClient.get(this.GetCarrierUserEmailsUrl + '?assignedCarrierCompanyId=' + assignedCarrierCompanyId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('GetCarrierUserEmails', [])));
    }
    getCarriers() {
        return this.httpClient.get(this.carrierUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getCarriers', [])));
    }
    getAssignedCarrierUsers() {
        return this.httpClient.get(this.carrierUsersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getAssignedCarrierUsers', [])));
    }
    getJobs() {
        return this.httpClient.get(this.jobsUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getJobs', [])));
    }
    getAssignedCarriers() {
        return this.httpClient.get(this.assignedCarriersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getAssignedCarriers', [])));
    }
    assignCarriers(carriers) {
        return this.httpClient.post(this.createUrl, carriers, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('assignCarriers', carriers)));
    }
    updateAssignedCarrier(carrier) {
        return this.httpClient.post(this.updateUrl, carrier, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('updateAssignedCarrier', carrier)));
    }
    deleteAssignedCarrier(carrier) {
        return this.httpClient.post(this.deleteUrl, carrier, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('deleteAssignedCarrier', carrier)));
    }
    createFreightOrder(carriers) {
        return this.httpClient.post(this.createFreightOrderUrl, carriers)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('createFreightOrder', carriers)));
    }
    editFreightOnlyOrders(JobIdsToEdit) {
        return this.httpClient.post(this.editFreightOnlyOrderUrl, JobIdsToEdit)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('editFreightOnlyOrders', JobIdsToEdit)));
    }
    closeAssignedOrdersforCarrier(EditFreightOnlyOrder) {
        return this.httpClient.post(this.closeFreightOnlyOrderUrl, EditFreightOnlyOrder)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('closeAssignedOrdersforCarrier', EditFreightOnlyOrder)));
    }
    upload(file, IsCreateFreightOrder) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        formData.append("IsCreateFreightOrder", IsCreateFreightOrder);
        return this.httpClient.post(this.BulkUploadCarrierUrl, formData);
    }
}
AssigncarrierService.ɵfac = function AssigncarrierService_Factory(t) { return new (t || AssigncarrierService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"])); };
AssigncarrierService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AssigncarrierService, factory: AssigncarrierService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AssigncarrierService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"] }]; }, null); })();
class DropdownItem {
}
class JobWithEmails {
}
class DriverModel {
}
class Carrier {
}
class CarrierJob {
}
class EditFreightOnlyOrder {
}
class CarrierJobDetails {
}


/***/ }),

/***/ "./src/app/carrier/service/deliveryrequest.service.ts":
/*!************************************************************!*\
  !*** ./src/app/carrier/service/deliveryrequest.service.ts ***!
  \************************************************************/
/*! exports provided: DeliveryrequestService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeliveryrequestService", function() { return DeliveryrequestService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../models/DispatchSchedulerModels */ "./src/app/carrier/models/DispatchSchedulerModels.ts");
/* harmony import */ var src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/my.functions */ "./src/app/my.functions.ts");
/* harmony import */ var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! src/app/app.enum */ "./src/app/app.enum.ts");





class DeliveryrequestService {
    constructor() { }
    groupDrsByJob(drs, drType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].None) {
        var response = [];
        for (var i = 0; i < drs.length; i++) {
            var jobIndex = -1;
            if (drs[i].IsTBD == false) {
                jobIndex = response.findIndex(t => t.JobId == drs[i].JobId);
            }
            else {
                jobIndex = response.findIndex(t => t.TBDGroupId == drs[i].TBDGroupId);
            }
            if (jobIndex == -1) {
                var job = new _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DelRequestsByJobModel"]();
                job.JobId = drs[i].JobId;
                job.JobName = drs[i].JobName;
                job.DeliveryRequestType = drType;
                job.JobCity = drs[i].JobCity;
                if (drs[i].IsTBD == false) {
                    job.CarrierStatus = this.getCarrierStatus(drs.filter(t => t.JobId == job.JobId));
                    job.Priority = this.getPriority(drs.filter(t => t.JobId == job.JobId));
                }
                else {
                    job.CarrierStatus = this.getCarrierStatus(drs.filter(t => t.TBDGroupId == job.TBDGroupId));
                    job.Priority = this.getPriority(drs.filter(t => t.TBDGroupId == job.TBDGroupId));
                    job.ProductType = drs[i].ProductType;
                }
                job.CustomerCompany = drs[i].CustomerCompany;
                job.JobAddress = drs[i].JobAddress;
                job.CustomerBrandId = drs[i].CustomerBrandId;
                job.TBDGroupId = drs[i].TBDGroupId;
                if (drs[i].TrailerTypes.length != 5) {
                    job.TrailerCompatibility = drs[i].TrailerTypes.map(t => t.Name).join(',');
                }
                job.IsOnlyNightDelivery = drs[i].IsAcceptNightDeliveries;
                job.HoursToCoverDistance = drs[i].HoursToCoverDistance;
                job._HoursToCoverDistance = drs[i].HoursToCoverDistance ? Number(drs[i].HoursToCoverDistance.replace(/:/g, '')) : 0;
                job.DRQueueAttributes = drs[i].DRQueueAttributes;
                job.LoadQueueAttributes = drs[i].LoadQueueAttributes;
                job.DeliveryRequests.push(drs[i]);
                job.IsTBD = drs[i].IsTBD;
                job.TBDGroupId = drs[i].TBDGroupId;
                job.ProductType = drs[i].ProductType;
                job.ProductTypeId = drs[i].ProductTypeId;
                job.UoM = drs[i].UoM;
                job.RequiredQuantity = drs[i].RequiredQuantity;
                job.ScheduleQuantityTypeText = drs[i].ScheduleQuantityTypeText;
                response.push(job);
            }
            else {
                response[jobIndex].DeliveryRequests.push(drs[i]);
            }
        }
        response = Object(src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__["sortByDesc"])(response, '_HoursToCoverDistance');
        return response;
    }
    getHourFromTime(time) {
        let hour = 0;
        hour = Number(time.split(":")[0]);
        if (time.indexOf("PM") > -1 && hour < 12) {
            hour += 12;
        }
        if (time.indexOf("AM") > -1 && hour == 12) {
            hour = 0;
        }
        return hour;
    }
    groupDrsBySelectedTime(drs, drType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].None) {
        var response = [];
        for (var i = 0; i < drs.length; i++) {
            var timeIndex = -1;
            const startHour = this.getHourFromTime(drs[i].ScheduleStartTime.toString());
            const endHour = this.getHourFromTime(drs[i].ScheduleEndTime.toString());
            if (drs[i].ScheduleStartTime && drs[i].ScheduleEndTime) {
                timeIndex = response.findIndex(t => t.StartTime == startHour && t.EndTime == endHour);
            }
            if (timeIndex == -1) {
                var timeDrs = new _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DelRequestByTimeModel"]();
                timeDrs.StartTime = startHour;
                timeDrs.EndTime = endHour;
                timeDrs.DeliveryRequests.push(drs[i]);
                response.push(timeDrs);
            }
            else {
                response[timeIndex].DeliveryRequests.push(drs[i]);
            }
        }
        response = Object(src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__["sortBy"])(response, 'StartTime');
        return response;
    }
    getCarrierStatus(drs) {
        if (drs.every(t => t.CarrierStatus == 2)) {
            return 2;
        }
        else if (drs.every(t => t.CarrierStatus == 3)) {
            return 3;
        }
        return 0;
    }
    getMustGoRequests(drs) {
        var mustGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) != -1);
        mustGoRequests.forEach(t => { t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].MustGo; t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].MustGo; });
        return mustGoRequests;
    }
    getShouldGoRequests(drs) {
        var shouldGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 2) != -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) == -1);
        shouldGoRequests.forEach(t => { t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].ShouldGo; t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].ShouldGo; });
        return shouldGoRequests;
    }
    getCouldGoRequests(drs) {
        var couldGoRequests = drs.filter(t => t.DeliveryRequests.findIndex(t1 => t1.Priority == 3) != -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 1) == -1
            && t.DeliveryRequests.findIndex(t1 => t1.Priority == 2) == -1);
        couldGoRequests.forEach(t => { t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].CouldGo; t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].CouldGo; });
        return couldGoRequests;
    }
    getPriority(drs) {
        if (drs.some(t => t.Priority == 1)) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].MustGo;
        }
        else if (drs.some(t => t.Priority == 2) && !drs.some(t => t.Priority == 1)) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].ShouldGo;
        }
        else if (drs.some(t => t.Priority == 3) && !drs.some(t => t.Priority == 1) && !drs.some(t => t.Priority == 2)) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].CouldGo;
        }
        return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].None;
    }
}
DeliveryrequestService.ɵfac = function DeliveryrequestService_Factory(t) { return new (t || DeliveryrequestService)(); };
DeliveryrequestService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: DeliveryrequestService, factory: DeliveryrequestService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryrequestService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return []; }, null); })();


/***/ }),

/***/ "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts":
/*!*****************************************************************!*\
  !*** ./src/app/fuelsurcharge/services/fuelsurcharge.service.ts ***!
  \*****************************************************************/
/*! exports provided: FuelSurchargeService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FuelSurchargeService", function() { return FuelSurchargeService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");







const httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
class FuelSurchargeService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
        this.getSupplierCustomersUrl = "/FuelSurcharge/GetSupplierCustomers";
        this.getSourceRegionsUrl = "/FuelSurcharge/GetSourceRegionsAsync";
        this.getTerminalsAndBulkPlantsUrl = "/FuelSurcharge/GetTerminalsAndBulkPlantsAsync?regionIds=";
        this.getFuelSurchargeProductsUrl = '/FuelSurcharge/GetFuelSurchargeProductAsync?countryId=';
        this.getFuelSurchargePeriodUrl = '/FuelSurcharge/GetFuelSurchargePeriodAsync?countryId=';
        this.getFuelSurchargeAreaUrl = '/FuelSurcharge/GetFuelSurchargeAreaAsync?countryId=';
        this.getEIAIndexPriceUrl = '/FuelSurcharge/GetEIAIndexPrice?periodId=';
        this.getNRCIndexPriceUrl = '/FuelSurcharge/GetNRCIndexPrice?periodId=';
        this.getGenerateSurchargeTableUrl = '/FuelSurcharge/GetGenerateSurchargeTable?pRSV=';
        this.getViewFuelSurchargeSummaryUrl = '/FuelSurcharge/GetViewFuelSurchargeSummary';
        this.getSurchargeTableNewUrl = '/FuelSurcharge/GetSurchargeTableNew?fuelSurchargeIndexId=';
        this.createFuelSurchargeUrl = '/FuelSurcharge/CreateFuelSurchargeAsync';
        this.archiveFuelSurchargeTableUrl = '/FuelSurcharge/ArchiveFuelSurchargeTable';
        this.getHistoricalPriceUrl = '/FuelSurcharge/GetHistoricalPrice?fuelSurchargeIndexId=';
        this.getFuelSurchargeTableUrl = '/FuelSurcharge/GetFuelSurchargeTableAsync?fuelSurchargeTableId=';
        this.onSelectedTabChanged = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1);
        this.onSelectedFuelSurchargeId = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](null);
    }
    getTableTypes() {
        return this.httpClient.get(this.getTableTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetTableTypes', null)));
    }
    getSupplierCustomers() {
        return this.httpClient.get(this.getSupplierCustomersUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetSupplierCustomers', null)));
    }
    getSourceRegions(input) {
        return this.httpClient.post(this.getSourceRegionsUrl, input)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getSourceRegions', null)));
    }
    getTerminalsAndBulkPlants(regionIds) {
        return this.httpClient.get(this.getTerminalsAndBulkPlantsUrl + regionIds)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetTerminalsAndBulkPlants', null)));
    }
    getFuelSurchargeProducts(countryId) {
        return this.httpClient.get(this.getFuelSurchargeProductsUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetFuelSurchargeProduct', null)));
    }
    getFuelSurchargePeriod(countryId) {
        return this.httpClient.get(this.getFuelSurchargePeriodUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargePeriod', null)));
    }
    getFuelSurchargeArea(countryId) {
        return this.httpClient.get(this.getFuelSurchargeAreaUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeArea', null)));
    }
    getEIAIndexPrice(periodId, productType, fetchDate, areaId) {
        return this.httpClient.get(this.getEIAIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate + "&areaId=" + areaId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getEIAIndexPrice', null)));
    }
    getNRCIndexPrice(periodId, productType, fetchDate) {
        return this.httpClient.get(this.getNRCIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getNRCIndexPrice', null)));
    }
    createFuelSurcharge(fsm) {
        return this.httpClient.post(this.createFuelSurchargeUrl, fsm, httpOptions)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('createFuelSurcharge', fsm)));
    }
    getGenerateSurchargeTable(pRSV, pREV, pRI, sI, fSSP) {
        return this.httpClient.get(this.getGenerateSurchargeTableUrl + pRSV + "&pREV=" + pREV + "&pRI=" + pRI + "&sI=" + sI + "&fSSP=" + fSSP)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getGenerateSurchargeTable', null)));
    }
    getFuelSurchargeGridDetails(filter) {
        return this.httpClient.post(this.getViewFuelSurchargeSummaryUrl, filter)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeGridDetails', null)));
    }
    getSurchargeTableNew(fuelSurchargeIndexId) {
        return this.httpClient.get(this.getSurchargeTableNewUrl + fuelSurchargeIndexId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getSurchargeTableNew', null)));
    }
    getHistoricalPrice(fuelSurchargeIndexId, forPeriod) {
        return this.httpClient.get(this.getHistoricalPriceUrl + fuelSurchargeIndexId + "&forPeriod=" + forPeriod)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getHistoricalPrice', null)));
    }
    archiveFuelSurchargeTable(fuelSurchargeIndexId) {
        return this.httpClient.post(this.archiveFuelSurchargeTableUrl, { fuelSurchargeIndexId: fuelSurchargeIndexId })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('archiveFuelSurchargeTable', null)));
    }
    getFuelSurchargeTable(fuelSurchargeTableId) {
        return this.httpClient.get(this.getFuelSurchargeTableUrl + fuelSurchargeTableId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeTable', null)));
    }
}
FuelSurchargeService.ɵfac = function FuelSurchargeService_Factory(t) { return new (t || FuelSurchargeService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
FuelSurchargeService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: FuelSurchargeService, factory: FuelSurchargeService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FuelSurchargeService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/invitation/invitation.service.ts":
/*!**************************************************!*\
  !*** ./src/app/invitation/invitation.service.ts ***!
  \**************************************************/
/*! exports provided: InvitationService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InvitationService", function() { return InvitationService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class InvitationService extends _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.isCompanyNameExistUrl = "Validation/IsCompanyNameExist";
        this.getCountrylistUrl = "Invitation/GetCountryList";
        this.getStatesOfAllCountrieslistUrl = "Invitation/GetStateList";
        this.postInvitationRequestUrl = "Invitation/Save";
        this.getThirdPartyCompanyTypesUrl = "Invitation/GetThirdPartyCompanyTypes";
        this.getAllTrailerAssetTypesUrl = "Invitation/GetAllTrailerAssetTypes";
        this.getCitiesFromStatesUrl = "Invitation/GetCityAndZipsByState";
        this.getaddressByZipUrl = "Validation/GetAddressByZip";
        this.getCarrierOnboardingForBrandingUrl = "Invitation/getCarrierOnboardingForBranding";
        this.GetPhoneTypesUrl = "Invitation/GetPhoneTypes";
        this.IsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid";
        //private GetaddressbyLatLongUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        this.isEmailExistUrl = "Invitation/IsEmailExist";
        this.GetCompaniesUrl = "/Invitation/GetCompanies";
    }
    IsCompanyNameExist(IsNewCompany, CompanyName) {
        return this.httpClient.get(`${this.isCompanyNameExistUrl}?IsNewCompany=${IsNewCompany}&CompanyName=${CompanyName}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsCompanyNameExist', null)));
    }
    GetCountryList() {
        return this.httpClient.get(this.getCountrylistUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCountryList', null)));
    }
    GetStatesOfAllCountries() {
        return this.httpClient.get(this.getStatesOfAllCountrieslistUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetStatesOfAllCountries', null)));
    }
    SaveInvitedRequest(sourcingRequestModel) {
        return this.httpClient.post(this.postInvitationRequestUrl, sourcingRequestModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('SaveInvitedRequest', null)));
    }
    GetThirdPartyCompanyTypes() {
        return this.httpClient.get(this.getThirdPartyCompanyTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetThirdPartyCompanyTypes', null)));
    }
    GetAllTrailerAssetTypes() {
        return this.httpClient.get(this.getAllTrailerAssetTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAllTrailerAssetTypes', null)));
    }
    GetCitiesFromStates(stateIds) {
        return this.httpClient.get(`${this.getCitiesFromStatesUrl}?stateIds=${stateIds}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCitiesFromStates', null)));
    }
    GetAddressByZip(zipCode) {
        return this.httpClient.get(`${this.getaddressByZipUrl}?zipCode=${zipCode}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAddressByZip', null)));
    }
    GetCarrierOnboardingForBranding(token) {
        return this.httpClient.get(`${this.getCarrierOnboardingForBrandingUrl}?token=${token}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCarrierOnboardingForBranding', null)));
    }
    GetPhoneTypes() {
        return this.httpClient.get(this.GetPhoneTypesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetPhoneTypes', null)));
    }
    IsPhoneNumberValid(phoneNumber) {
        return this.httpClient.get(`${this.IsPhoneNumberValidUrl}?phoneNumber='${phoneNumber}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsPhoneNumberValid', null)));
    }
    IsEmailExist(email) {
        return this.httpClient.get(`${this.isEmailExistUrl}?email=${email}`)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsEmailExist', null)));
    }
    GetCompanies() {
        return this.httpClient.get(this.GetCompaniesUrl)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCompanies', null)));
    }
}
InvitationService.ɵfac = function InvitationService_Factory(t) { return new (t || InvitationService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
InvitationService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: InvitationService, factory: InvitationService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](InvitationService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/lfv-dashboard/LiftFileModels.ts":
/*!*************************************************!*\
  !*** ./src/app/lfv-dashboard/LiftFileModels.ts ***!
  \*************************************************/
/*! exports provided: LFRecordGridModel, LFBolEditModel, DropDownItem, LFValidationGridViewModel, SupplierBOLReport, CarrierBOLReport, LFRecordsGridExport, LFVValidationParameters */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LFRecordGridModel", function() { return LFRecordGridModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LFBolEditModel", function() { return LFBolEditModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DropDownItem", function() { return DropDownItem; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LFValidationGridViewModel", function() { return LFValidationGridViewModel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SupplierBOLReport", function() { return SupplierBOLReport; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CarrierBOLReport", function() { return CarrierBOLReport; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LFRecordsGridExport", function() { return LFRecordsGridExport; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LFVValidationParameters", function() { return LFVValidationParameters; });
class LFRecordGridModel {
    constructor() {
        this.statusChangeDate = "";
    }
}
class LFBolEditModel {
    constructor() {
        this.LiftRecord = new LFRecordGridModel();
    }
}
class DropDownItem {
}
class LFValidationGridViewModel {
}
//LiftFileRecordsWithMissingTFXDeliveryDetails
class SupplierBOLReport {
}
//TFXDeliveryDetailsWithMissingLiftFileRecords
class CarrierBOLReport {
}
class LFRecordsGridExport {
}
class LFVValidationParameters {
    constructor() {
        this.IsTerminalCodeReq = true;
        this.IsBolReq = true;
    }
}


/***/ }),

/***/ "./src/app/location/services/location.service.ts":
/*!*******************************************************!*\
  !*** ./src/app/location/services/location.service.ts ***!
  \*******************************************************/
/*! exports provided: LocationService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LocationService", function() { return LocationService; });
/* harmony import */ var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/errors/HandleError */ "./src/app/errors/HandleError.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");





class LocationService extends src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_0__["HandleError"] {
    constructor(httpClient) {
        super();
        this.httpClient = httpClient;
        this.GetTerminalsUrl = '/Location/GetTerminals';
        this.GetBulkPlantsUrl = '/Location/GetBulkPlants?countryId=';
        this.PostBulkPlantLocationUrl = '/Location/SaveBulkPlantLocation';
    }
    getTerminals(requestModel) {
        return this.httpClient.post(this.GetTerminalsUrl, requestModel)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTerminals', null)));
    }
    GetBulkPlants(countryId) {
        return this.httpClient.get(this.GetBulkPlantsUrl + countryId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetBulkPlants', null)));
    }
    PostBulkPlantLocation(data) {
        return this.httpClient.post(this.PostBulkPlantLocationUrl, data)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('PostBulkPlantLocation', null)));
    }
}
LocationService.ɵfac = function LocationService_Factory(t) { return new (t || LocationService)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"])); };
LocationService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({ token: LocationService, factory: LocationService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵsetClassMetadata"](LocationService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_2__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/my.localstorage.ts":
/*!************************************!*\
  !*** ./src/app/my.localstorage.ts ***!
  \************************************/
/*! exports provided: MyLocalStorage */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MyLocalStorage", function() { return MyLocalStorage; });
class MyLocalStorage {
    static setData(key, value) {
        if (!value) {
            localStorage.removeItem(key);
        }
        else {
            localStorage.setItem(key, JSON.stringify(value));
        }
    }
    static getData(key) {
        let value = localStorage.getItem(key);
        if (value) {
            value = JSON.parse(value);
        }
        if (value == 'null') {
            value = null;
        }
        return value || '';
    }
}
// Schedule Builder filter Keys---------------------------------
MyLocalStorage.DSB_DATE_KEY = "date";
MyLocalStorage.DSB_REGION_KEY = "regionId";
MyLocalStorage.DSB_OBJECTFILTER_KEY = "objectFilter";
MyLocalStorage.DSB_DATEFILTER_KEY = "dateFilter";
MyLocalStorage.DSB_WINDOWMODE_KEY = "windowMode";
MyLocalStorage.DSB_TOGGLEREQUESTMODE_KEY = "toggleRequestMode";
MyLocalStorage.DSB_READONLY_KEY = "readOnlyMode";
MyLocalStorage.DSB_FILTER_KEY = "dsbviewFilter";
// Wally Boards Filter Keys-------------------------------------
MyLocalStorage.WBF_REGION_KEY = "wbf_regionId";
MyLocalStorage.WBF_CUSTOMER_KEY = "wbf_customerId";
MyLocalStorage.WBF_SEARCHEDKEYWORD_KEY = "wbf_searchedKeyword";
MyLocalStorage.WBF_SELECTEDSTATES_KEY = "wbf_selectedStates";
MyLocalStorage.WBF_LOCATION_KEY = "wbf_selectedLocations";
MyLocalStorage.WBF_SELECTEDPRIORY_KEY = "wbf_selectedPriority";
MyLocalStorage.WBF_SELECTEDSUPPLIER_KEY = "wbf_selectedSupplier";
MyLocalStorage.WBF_SELECTEDCARRIER_KEY = "wbf_selectedCarrier";
MyLocalStorage.WBF_SELECTEDDISPACHER_KEY = "wbf_selectedDispacher";
MyLocalStorage.WBF_FROMDATE_KEY = "wbf_fromDate";
MyLocalStorage.WBF_TODATE_KEY = "wbf_toDate";


/***/ })

}]);
//# sourceMappingURL=common-es2015.js.map