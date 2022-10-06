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

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["common"], {
  /***/
  "./src/app/carrier-companies/service/assigncarrier.service.ts": function srcAppCarrierCompaniesServiceAssigncarrierServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AssigncarrierService", function () {
      return AssigncarrierService;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DropdownItem", function () {
      return DropdownItem;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "JobWithEmails", function () {
      return JobWithEmails;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DriverModel", function () {
      return DriverModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "Carrier", function () {
      return Carrier;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierJob", function () {
      return CarrierJob;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "EditFreightOnlyOrder", function () {
      return EditFreightOnlyOrder;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierJobDetails", function () {
      return CarrierJobDetails;
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


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var AssigncarrierService = /*#__PURE__*/function (_src_app_errors_Handl) {
      _inherits(AssigncarrierService, _src_app_errors_Handl);

      var _super = _createSuper(AssigncarrierService);

      function AssigncarrierService(httpClient) {
        var _this;

        _classCallCheck(this, AssigncarrierService);

        _this = _super.call(this);
        _this.httpClient = httpClient;
        _this.carrierUrl = '/Settings/Profile/GetCarriers';
        _this.carrierUsersUrl = '/Settings/Profile/GetAssignedCarriers';
        _this.jobsUrl = '/Settings/Profile/GetJobsForSupplierToCarrier';
        _this.assignedCarriersUrl = '/Settings/Profile/GetAssignedCarriersForSupplier';
        _this.createUrl = '/Settings/Profile/AssignCarriers';
        _this.updateUrl = '/Settings/Profile/UpdateAssignedCarrier';
        _this.deleteUrl = '/Settings/Profile/DeleteAssignedCarrier';
        _this.createFreightOrderUrl = '/Carrier/Order/createFreightOrdersForAssignedCarrier';
        _this.editFreightOnlyOrderUrl = '/Carrier/Order/EditFreightOnlyOrders';
        _this.closeFreightOnlyOrderUrl = '/Carrier/Order/closeAssignedOrdersforCarrier';
        _this.GetCarrierUserEmailsUrl = '/Supplier/Order/GetCarrierUserEmails';
        _this.BulkUploadCarrierUrl = '/Settings/Profile/BulkUploadCarrier';
        return _this;
      }

      _createClass(AssigncarrierService, [{
        key: "GetCarrierUserEmails",
        value: function GetCarrierUserEmails(assignedCarrierCompanyId) {
          return this.httpClient.get(this.GetCarrierUserEmailsUrl + '?assignedCarrierCompanyId=' + assignedCarrierCompanyId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('GetCarrierUserEmails', [])));
        }
      }, {
        key: "getCarriers",
        value: function getCarriers() {
          return this.httpClient.get(this.carrierUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getCarriers', [])));
        }
      }, {
        key: "getAssignedCarrierUsers",
        value: function getAssignedCarrierUsers() {
          return this.httpClient.get(this.carrierUsersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getAssignedCarrierUsers', [])));
        }
      }, {
        key: "getJobs",
        value: function getJobs() {
          return this.httpClient.get(this.jobsUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getJobs', [])));
        }
      }, {
        key: "getAssignedCarriers",
        value: function getAssignedCarriers() {
          return this.httpClient.get(this.assignedCarriersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('getAssignedCarriers', [])));
        }
      }, {
        key: "assignCarriers",
        value: function assignCarriers(carriers) {
          return this.httpClient.post(this.createUrl, carriers, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('assignCarriers', carriers)));
        }
      }, {
        key: "updateAssignedCarrier",
        value: function updateAssignedCarrier(carrier) {
          return this.httpClient.post(this.updateUrl, carrier, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('updateAssignedCarrier', carrier)));
        }
      }, {
        key: "deleteAssignedCarrier",
        value: function deleteAssignedCarrier(carrier) {
          return this.httpClient.post(this.deleteUrl, carrier, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('deleteAssignedCarrier', carrier)));
        }
      }, {
        key: "createFreightOrder",
        value: function createFreightOrder(carriers) {
          return this.httpClient.post(this.createFreightOrderUrl, carriers).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('createFreightOrder', carriers)));
        }
      }, {
        key: "editFreightOnlyOrders",
        value: function editFreightOnlyOrders(JobIdsToEdit) {
          return this.httpClient.post(this.editFreightOnlyOrderUrl, JobIdsToEdit).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('editFreightOnlyOrders', JobIdsToEdit)));
        }
      }, {
        key: "closeAssignedOrdersforCarrier",
        value: function closeAssignedOrdersforCarrier(EditFreightOnlyOrder) {
          return this.httpClient.post(this.closeFreightOnlyOrderUrl, EditFreightOnlyOrder).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_3__["catchError"])(this.handleError('closeAssignedOrdersforCarrier', EditFreightOnlyOrder)));
        }
      }, {
        key: "upload",
        value: function upload(file, IsCreateFreightOrder) {
          var formData = new FormData();
          formData.append("file", file, file.name);
          formData.append("IsCreateFreightOrder", IsCreateFreightOrder);
          return this.httpClient.post(this.BulkUploadCarrierUrl, formData);
        }
      }]);

      return AssigncarrierService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    AssigncarrierService.ɵfac = function AssigncarrierService_Factory(t) {
      return new (t || AssigncarrierService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"]));
    };

    AssigncarrierService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: AssigncarrierService,
      factory: AssigncarrierService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AssigncarrierService, [{
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

    var DropdownItem = function DropdownItem() {
      _classCallCheck(this, DropdownItem);
    };

    var JobWithEmails = function JobWithEmails() {
      _classCallCheck(this, JobWithEmails);
    };

    var DriverModel = function DriverModel() {
      _classCallCheck(this, DriverModel);
    };

    var Carrier = function Carrier() {
      _classCallCheck(this, Carrier);
    };

    var CarrierJob = function CarrierJob() {
      _classCallCheck(this, CarrierJob);
    };

    var EditFreightOnlyOrder = function EditFreightOnlyOrder() {
      _classCallCheck(this, EditFreightOnlyOrder);
    };

    var CarrierJobDetails = function CarrierJobDetails() {
      _classCallCheck(this, CarrierJobDetails);
    };
    /***/

  },

  /***/
  "./src/app/carrier/service/deliveryrequest.service.ts": function srcAppCarrierServiceDeliveryrequestServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DeliveryrequestService", function () {
      return DeliveryrequestService;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! ../models/DispatchSchedulerModels */
    "./src/app/carrier/models/DispatchSchedulerModels.ts");
    /* harmony import */


    var src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! src/app/my.functions */
    "./src/app/my.functions.ts");
    /* harmony import */


    var src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! src/app/app.enum */
    "./src/app/app.enum.ts");

    var DeliveryrequestService = /*#__PURE__*/function () {
      function DeliveryrequestService() {
        _classCallCheck(this, DeliveryrequestService);
      }

      _createClass(DeliveryrequestService, [{
        key: "groupDrsByJob",
        value: function groupDrsByJob(drs) {
          var drType = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].None;
          var response = [];

          for (var i = 0; i < drs.length; i++) {
            var jobIndex = -1;

            if (drs[i].IsTBD == false) {
              jobIndex = response.findIndex(function (t) {
                return t.JobId == drs[i].JobId;
              });
            } else {
              jobIndex = response.findIndex(function (t) {
                return t.TBDGroupId == drs[i].TBDGroupId;
              });
            }

            if (jobIndex == -1) {
              var job = new _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DelRequestsByJobModel"]();
              job.JobId = drs[i].JobId;
              job.JobName = drs[i].JobName;
              job.DeliveryRequestType = drType;
              job.JobCity = drs[i].JobCity;

              if (drs[i].IsTBD == false) {
                job.CarrierStatus = this.getCarrierStatus(drs.filter(function (t) {
                  return t.JobId == job.JobId;
                }));
                job.Priority = this.getPriority(drs.filter(function (t) {
                  return t.JobId == job.JobId;
                }));
              } else {
                job.CarrierStatus = this.getCarrierStatus(drs.filter(function (t) {
                  return t.TBDGroupId == job.TBDGroupId;
                }));
                job.Priority = this.getPriority(drs.filter(function (t) {
                  return t.TBDGroupId == job.TBDGroupId;
                }));
                job.ProductType = drs[i].ProductType;
              }

              job.CustomerCompany = drs[i].CustomerCompany;
              job.JobAddress = drs[i].JobAddress;
              job.CustomerBrandId = drs[i].CustomerBrandId;
              job.TBDGroupId = drs[i].TBDGroupId;

              if (drs[i].TrailerTypes.length != 5) {
                job.TrailerCompatibility = drs[i].TrailerTypes.map(function (t) {
                  return t.Name;
                }).join(',');
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
            } else {
              response[jobIndex].DeliveryRequests.push(drs[i]);
            }
          }

          response = Object(src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__["sortByDesc"])(response, '_HoursToCoverDistance');
          return response;
        }
      }, {
        key: "getHourFromTime",
        value: function getHourFromTime(time) {
          var hour = 0;
          hour = Number(time.split(":")[0]);

          if (time.indexOf("PM") > -1 && hour < 12) {
            hour += 12;
          }

          if (time.indexOf("AM") > -1 && hour == 12) {
            hour = 0;
          }

          return hour;
        }
      }, {
        key: "groupDrsBySelectedTime",
        value: function groupDrsBySelectedTime(drs) {
          var _this2 = this;

          var drType = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].None;
          var response = [];

          var _loop = function _loop() {
            timeIndex = -1;

            var startHour = _this2.getHourFromTime(drs[i].ScheduleStartTime.toString());

            var endHour = _this2.getHourFromTime(drs[i].ScheduleEndTime.toString());

            if (drs[i].ScheduleStartTime && drs[i].ScheduleEndTime) {
              timeIndex = response.findIndex(function (t) {
                return t.StartTime == startHour && t.EndTime == endHour;
              });
            }

            if (timeIndex == -1) {
              timeDrs = new _models_DispatchSchedulerModels__WEBPACK_IMPORTED_MODULE_1__["DelRequestByTimeModel"]();
              timeDrs.StartTime = startHour;
              timeDrs.EndTime = endHour;
              timeDrs.DeliveryRequests.push(drs[i]);
              response.push(timeDrs);
            } else {
              response[timeIndex].DeliveryRequests.push(drs[i]);
            }
          };

          for (var i = 0; i < drs.length; i++) {
            var timeIndex;
            var timeDrs;

            _loop();
          }

          response = Object(src_app_my_functions__WEBPACK_IMPORTED_MODULE_2__["sortBy"])(response, 'StartTime');
          return response;
        }
      }, {
        key: "getCarrierStatus",
        value: function getCarrierStatus(drs) {
          if (drs.every(function (t) {
            return t.CarrierStatus == 2;
          })) {
            return 2;
          } else if (drs.every(function (t) {
            return t.CarrierStatus == 3;
          })) {
            return 3;
          }

          return 0;
        }
      }, {
        key: "getMustGoRequests",
        value: function getMustGoRequests(drs) {
          var mustGoRequests = drs.filter(function (t) {
            return t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 1;
            }) != -1;
          });
          mustGoRequests.forEach(function (t) {
            t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].MustGo;
            t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].MustGo;
          });
          return mustGoRequests;
        }
      }, {
        key: "getShouldGoRequests",
        value: function getShouldGoRequests(drs) {
          var shouldGoRequests = drs.filter(function (t) {
            return t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 2;
            }) != -1 && t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 1;
            }) == -1;
          });
          shouldGoRequests.forEach(function (t) {
            t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].ShouldGo;
            t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].ShouldGo;
          });
          return shouldGoRequests;
        }
      }, {
        key: "getCouldGoRequests",
        value: function getCouldGoRequests(drs) {
          var couldGoRequests = drs.filter(function (t) {
            return t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 3;
            }) != -1 && t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 1;
            }) == -1 && t.DeliveryRequests.findIndex(function (t1) {
              return t1.Priority == 2;
            }) == -1;
          });
          couldGoRequests.forEach(function (t) {
            t.DeliveryRequestType = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryRequestTypes"].CouldGo;
            t.Priority = src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].CouldGo;
          });
          return couldGoRequests;
        }
      }, {
        key: "getPriority",
        value: function getPriority(drs) {
          if (drs.some(function (t) {
            return t.Priority == 1;
          })) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].MustGo;
          } else if (drs.some(function (t) {
            return t.Priority == 2;
          }) && !drs.some(function (t) {
            return t.Priority == 1;
          })) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].ShouldGo;
          } else if (drs.some(function (t) {
            return t.Priority == 3;
          }) && !drs.some(function (t) {
            return t.Priority == 1;
          }) && !drs.some(function (t) {
            return t.Priority == 2;
          })) {
            return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].CouldGo;
          }

          return src_app_app_enum__WEBPACK_IMPORTED_MODULE_3__["DeliveryReqPriority"].None;
        }
      }]);

      return DeliveryrequestService;
    }();

    DeliveryrequestService.ɵfac = function DeliveryrequestService_Factory(t) {
      return new (t || DeliveryrequestService)();
    };

    DeliveryrequestService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: DeliveryrequestService,
      factory: DeliveryrequestService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DeliveryrequestService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
          providedIn: 'root'
        }]
      }], function () {
        return [];
      }, null);
    })();
    /***/

  },

  /***/
  "./src/app/fuelsurcharge/services/fuelsurcharge.service.ts": function srcAppFuelsurchargeServicesFuelsurchargeServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "FuelSurchargeService", function () {
      return FuelSurchargeService;
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


    var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! rxjs */
    "./node_modules/rxjs/_esm2015/index.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");

    var httpOptions = {
      headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpHeaders"]({
        'Content-Type': 'application/json'
      })
    };

    var FuelSurchargeService = /*#__PURE__*/function (_src_app_errors_Handl2) {
      _inherits(FuelSurchargeService, _src_app_errors_Handl2);

      var _super2 = _createSuper(FuelSurchargeService);

      function FuelSurchargeService(httpClient) {
        var _this3;

        _classCallCheck(this, FuelSurchargeService);

        _this3 = _super2.call(this);
        _this3.httpClient = httpClient;
        _this3.getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
        _this3.getSupplierCustomersUrl = "/FuelSurcharge/GetSupplierCustomers";
        _this3.getSourceRegionsUrl = "/FuelSurcharge/GetSourceRegionsAsync";
        _this3.getTerminalsAndBulkPlantsUrl = "/FuelSurcharge/GetTerminalsAndBulkPlantsAsync?regionIds=";
        _this3.getFuelSurchargeProductsUrl = '/FuelSurcharge/GetFuelSurchargeProductAsync?countryId=';
        _this3.getFuelSurchargePeriodUrl = '/FuelSurcharge/GetFuelSurchargePeriodAsync?countryId=';
        _this3.getFuelSurchargeAreaUrl = '/FuelSurcharge/GetFuelSurchargeAreaAsync?countryId=';
        _this3.getEIAIndexPriceUrl = '/FuelSurcharge/GetEIAIndexPrice?periodId=';
        _this3.getNRCIndexPriceUrl = '/FuelSurcharge/GetNRCIndexPrice?periodId=';
        _this3.getGenerateSurchargeTableUrl = '/FuelSurcharge/GetGenerateSurchargeTable?pRSV=';
        _this3.getViewFuelSurchargeSummaryUrl = '/FuelSurcharge/GetViewFuelSurchargeSummary';
        _this3.getSurchargeTableNewUrl = '/FuelSurcharge/GetSurchargeTableNew?fuelSurchargeIndexId=';
        _this3.createFuelSurchargeUrl = '/FuelSurcharge/CreateFuelSurchargeAsync';
        _this3.archiveFuelSurchargeTableUrl = '/FuelSurcharge/ArchiveFuelSurchargeTable';
        _this3.getHistoricalPriceUrl = '/FuelSurcharge/GetHistoricalPrice?fuelSurchargeIndexId=';
        _this3.getFuelSurchargeTableUrl = '/FuelSurcharge/GetFuelSurchargeTableAsync?fuelSurchargeTableId=';
        _this3.onSelectedTabChanged = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](1);
        _this3.onSelectedFuelSurchargeId = new rxjs__WEBPACK_IMPORTED_MODULE_2__["BehaviorSubject"](null);
        return _this3;
      }

      _createClass(FuelSurchargeService, [{
        key: "getTableTypes",
        value: function getTableTypes() {
          return this.httpClient.get(this.getTableTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetTableTypes', null)));
        }
      }, {
        key: "getSupplierCustomers",
        value: function getSupplierCustomers() {
          return this.httpClient.get(this.getSupplierCustomersUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetSupplierCustomers', null)));
        }
      }, {
        key: "getSourceRegions",
        value: function getSourceRegions(input) {
          return this.httpClient.post(this.getSourceRegionsUrl, input).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getSourceRegions', null)));
        }
      }, {
        key: "getTerminalsAndBulkPlants",
        value: function getTerminalsAndBulkPlants(regionIds) {
          return this.httpClient.get(this.getTerminalsAndBulkPlantsUrl + regionIds).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetTerminalsAndBulkPlants', null)));
        }
      }, {
        key: "getFuelSurchargeProducts",
        value: function getFuelSurchargeProducts(countryId) {
          return this.httpClient.get(this.getFuelSurchargeProductsUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('GetFuelSurchargeProduct', null)));
        }
      }, {
        key: "getFuelSurchargePeriod",
        value: function getFuelSurchargePeriod(countryId) {
          return this.httpClient.get(this.getFuelSurchargePeriodUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargePeriod', null)));
        }
      }, {
        key: "getFuelSurchargeArea",
        value: function getFuelSurchargeArea(countryId) {
          return this.httpClient.get(this.getFuelSurchargeAreaUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeArea', null)));
        }
      }, {
        key: "getEIAIndexPrice",
        value: function getEIAIndexPrice(periodId, productType, fetchDate, areaId) {
          return this.httpClient.get(this.getEIAIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate + "&areaId=" + areaId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getEIAIndexPrice', null)));
        }
      }, {
        key: "getNRCIndexPrice",
        value: function getNRCIndexPrice(periodId, productType, fetchDate) {
          return this.httpClient.get(this.getNRCIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getNRCIndexPrice', null)));
        }
      }, {
        key: "createFuelSurcharge",
        value: function createFuelSurcharge(fsm) {
          return this.httpClient.post(this.createFuelSurchargeUrl, fsm, httpOptions).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('createFuelSurcharge', fsm)));
        }
      }, {
        key: "getGenerateSurchargeTable",
        value: function getGenerateSurchargeTable(pRSV, pREV, pRI, sI, fSSP) {
          return this.httpClient.get(this.getGenerateSurchargeTableUrl + pRSV + "&pREV=" + pREV + "&pRI=" + pRI + "&sI=" + sI + "&fSSP=" + fSSP).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getGenerateSurchargeTable', null)));
        }
      }, {
        key: "getFuelSurchargeGridDetails",
        value: function getFuelSurchargeGridDetails(filter) {
          return this.httpClient.post(this.getViewFuelSurchargeSummaryUrl, filter).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeGridDetails', null)));
        }
      }, {
        key: "getSurchargeTableNew",
        value: function getSurchargeTableNew(fuelSurchargeIndexId) {
          return this.httpClient.get(this.getSurchargeTableNewUrl + fuelSurchargeIndexId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getSurchargeTableNew', null)));
        }
      }, {
        key: "getHistoricalPrice",
        value: function getHistoricalPrice(fuelSurchargeIndexId, forPeriod) {
          return this.httpClient.get(this.getHistoricalPriceUrl + fuelSurchargeIndexId + "&forPeriod=" + forPeriod).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getHistoricalPrice', null)));
        }
      }, {
        key: "archiveFuelSurchargeTable",
        value: function archiveFuelSurchargeTable(fuelSurchargeIndexId) {
          return this.httpClient.post(this.archiveFuelSurchargeTableUrl, {
            fuelSurchargeIndexId: fuelSurchargeIndexId
          }).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('archiveFuelSurchargeTable', null)));
        }
      }, {
        key: "getFuelSurchargeTable",
        value: function getFuelSurchargeTable(fuelSurchargeTableId) {
          return this.httpClient.get(this.getFuelSurchargeTableUrl + fuelSurchargeTableId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_4__["catchError"])(this.handleError('getFuelSurchargeTable', null)));
        }
      }]);

      return FuelSurchargeService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_1__["HandleError"]);

    FuelSurchargeService.ɵfac = function FuelSurchargeService_Factory(t) {
      return new (t || FuelSurchargeService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    FuelSurchargeService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: FuelSurchargeService,
      factory: FuelSurchargeService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](FuelSurchargeService, [{
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
  "./src/app/invitation/invitation.service.ts": function srcAppInvitationInvitationServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "InvitationService", function () {
      return InvitationService;
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


    var _app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! ../../app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var InvitationService = /*#__PURE__*/function (_app_errors_HandleErr) {
      _inherits(InvitationService, _app_errors_HandleErr);

      var _super3 = _createSuper(InvitationService);

      function InvitationService(httpClient) {
        var _this4;

        _classCallCheck(this, InvitationService);

        _this4 = _super3.call(this);
        _this4.httpClient = httpClient;
        _this4.isCompanyNameExistUrl = "Validation/IsCompanyNameExist";
        _this4.getCountrylistUrl = "Invitation/GetCountryList";
        _this4.getStatesOfAllCountrieslistUrl = "Invitation/GetStateList";
        _this4.postInvitationRequestUrl = "Invitation/Save";
        _this4.getThirdPartyCompanyTypesUrl = "Invitation/GetThirdPartyCompanyTypes";
        _this4.getAllTrailerAssetTypesUrl = "Invitation/GetAllTrailerAssetTypes";
        _this4.getCitiesFromStatesUrl = "Invitation/GetCityAndZipsByState";
        _this4.getaddressByZipUrl = "Validation/GetAddressByZip";
        _this4.getCarrierOnboardingForBrandingUrl = "Invitation/getCarrierOnboardingForBranding";
        _this4.GetPhoneTypesUrl = "Invitation/GetPhoneTypes";
        _this4.IsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid"; //private GetaddressbyLatLongUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        _this4.isEmailExistUrl = "Invitation/IsEmailExist";
        _this4.GetCompaniesUrl = "/Invitation/GetCompanies";
        return _this4;
      }

      _createClass(InvitationService, [{
        key: "IsCompanyNameExist",
        value: function IsCompanyNameExist(IsNewCompany, CompanyName) {
          return this.httpClient.get("".concat(this.isCompanyNameExistUrl, "?IsNewCompany=").concat(IsNewCompany, "&CompanyName=").concat(CompanyName)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsCompanyNameExist', null)));
        }
      }, {
        key: "GetCountryList",
        value: function GetCountryList() {
          return this.httpClient.get(this.getCountrylistUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCountryList', null)));
        }
      }, {
        key: "GetStatesOfAllCountries",
        value: function GetStatesOfAllCountries() {
          return this.httpClient.get(this.getStatesOfAllCountrieslistUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetStatesOfAllCountries', null)));
        }
      }, {
        key: "SaveInvitedRequest",
        value: function SaveInvitedRequest(sourcingRequestModel) {
          return this.httpClient.post(this.postInvitationRequestUrl, sourcingRequestModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('SaveInvitedRequest', null)));
        }
      }, {
        key: "GetThirdPartyCompanyTypes",
        value: function GetThirdPartyCompanyTypes() {
          return this.httpClient.get(this.getThirdPartyCompanyTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetThirdPartyCompanyTypes', null)));
        }
      }, {
        key: "GetAllTrailerAssetTypes",
        value: function GetAllTrailerAssetTypes() {
          return this.httpClient.get(this.getAllTrailerAssetTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAllTrailerAssetTypes', null)));
        }
      }, {
        key: "GetCitiesFromStates",
        value: function GetCitiesFromStates(stateIds) {
          return this.httpClient.get("".concat(this.getCitiesFromStatesUrl, "?stateIds=").concat(stateIds)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCitiesFromStates', null)));
        }
      }, {
        key: "GetAddressByZip",
        value: function GetAddressByZip(zipCode) {
          return this.httpClient.get("".concat(this.getaddressByZipUrl, "?zipCode=").concat(zipCode)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetAddressByZip', null)));
        }
      }, {
        key: "GetCarrierOnboardingForBranding",
        value: function GetCarrierOnboardingForBranding(token) {
          return this.httpClient.get("".concat(this.getCarrierOnboardingForBrandingUrl, "?token=").concat(token)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCarrierOnboardingForBranding', null)));
        }
      }, {
        key: "GetPhoneTypes",
        value: function GetPhoneTypes() {
          return this.httpClient.get(this.GetPhoneTypesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetPhoneTypes', null)));
        }
      }, {
        key: "IsPhoneNumberValid",
        value: function IsPhoneNumberValid(phoneNumber) {
          return this.httpClient.get("".concat(this.IsPhoneNumberValidUrl, "?phoneNumber='").concat(phoneNumber)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsPhoneNumberValid', null)));
        }
      }, {
        key: "IsEmailExist",
        value: function IsEmailExist(email) {
          return this.httpClient.get("".concat(this.isEmailExistUrl, "?email=").concat(email)).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('IsEmailExist', null)));
        }
      }, {
        key: "GetCompanies",
        value: function GetCompanies() {
          return this.httpClient.get(this.GetCompaniesUrl).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetCompanies', null)));
        }
      }]);

      return InvitationService;
    }(_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_2__["HandleError"]);

    InvitationService.ɵfac = function InvitationService_Factory(t) {
      return new (t || InvitationService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    InvitationService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({
      token: InvitationService,
      factory: InvitationService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](InvitationService, [{
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
  "./src/app/lfv-dashboard/LiftFileModels.ts": function srcAppLfvDashboardLiftFileModelsTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LFRecordGridModel", function () {
      return LFRecordGridModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LFBolEditModel", function () {
      return LFBolEditModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "DropDownItem", function () {
      return DropDownItem;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LFValidationGridViewModel", function () {
      return LFValidationGridViewModel;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "SupplierBOLReport", function () {
      return SupplierBOLReport;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "CarrierBOLReport", function () {
      return CarrierBOLReport;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LFRecordsGridExport", function () {
      return LFRecordsGridExport;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LFVValidationParameters", function () {
      return LFVValidationParameters;
    });

    var LFRecordGridModel = function LFRecordGridModel() {
      _classCallCheck(this, LFRecordGridModel);

      this.statusChangeDate = "";
    };

    var LFBolEditModel = function LFBolEditModel() {
      _classCallCheck(this, LFBolEditModel);

      this.LiftRecord = new LFRecordGridModel();
    };

    var DropDownItem = function DropDownItem() {
      _classCallCheck(this, DropDownItem);
    };

    var LFValidationGridViewModel = function LFValidationGridViewModel() {
      _classCallCheck(this, LFValidationGridViewModel);
    }; //LiftFileRecordsWithMissingTFXDeliveryDetails


    var SupplierBOLReport = function SupplierBOLReport() {
      _classCallCheck(this, SupplierBOLReport);
    }; //TFXDeliveryDetailsWithMissingLiftFileRecords


    var CarrierBOLReport = function CarrierBOLReport() {
      _classCallCheck(this, CarrierBOLReport);
    };

    var LFRecordsGridExport = function LFRecordsGridExport() {
      _classCallCheck(this, LFRecordsGridExport);
    };

    var LFVValidationParameters = function LFVValidationParameters() {
      _classCallCheck(this, LFVValidationParameters);

      this.IsTerminalCodeReq = true;
      this.IsBolReq = true;
    };
    /***/

  },

  /***/
  "./src/app/location/services/location.service.ts": function srcAppLocationServicesLocationServiceTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "LocationService", function () {
      return LocationService;
    });
    /* harmony import */


    var src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! src/app/errors/HandleError */
    "./src/app/errors/HandleError.ts");
    /* harmony import */


    var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! rxjs/operators */
    "./node_modules/rxjs/_esm2015/operators/index.js");
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(
    /*! @angular/common/http */
    "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");

    var LocationService = /*#__PURE__*/function (_src_app_errors_Handl3) {
      _inherits(LocationService, _src_app_errors_Handl3);

      var _super4 = _createSuper(LocationService);

      function LocationService(httpClient) {
        var _this5;

        _classCallCheck(this, LocationService);

        _this5 = _super4.call(this);
        _this5.httpClient = httpClient;
        _this5.GetTerminalsUrl = '/Location/GetTerminals';
        _this5.GetBulkPlantsUrl = '/Location/GetBulkPlants?countryId=';
        _this5.PostBulkPlantLocationUrl = '/Location/SaveBulkPlantLocation';
        return _this5;
      }

      _createClass(LocationService, [{
        key: "getTerminals",
        value: function getTerminals(requestModel) {
          return this.httpClient.post(this.GetTerminalsUrl, requestModel).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('getTerminals', null)));
        }
      }, {
        key: "GetBulkPlants",
        value: function GetBulkPlants(countryId) {
          return this.httpClient.get(this.GetBulkPlantsUrl + countryId).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('GetBulkPlants', null)));
        }
      }, {
        key: "PostBulkPlantLocation",
        value: function PostBulkPlantLocation(data) {
          return this.httpClient.post(this.PostBulkPlantLocationUrl, data).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["catchError"])(this.handleError('PostBulkPlantLocation', null)));
        }
      }]);

      return LocationService;
    }(src_app_errors_HandleError__WEBPACK_IMPORTED_MODULE_0__["HandleError"]);

    LocationService.ɵfac = function LocationService_Factory(t) {
      return new (t || LocationService)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"]));
    };

    LocationService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({
      token: LocationService,
      factory: LocationService.ɵfac,
      providedIn: 'root'
    });
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵsetClassMetadata"](LocationService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_2__["Injectable"],
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
  "./src/app/my.localstorage.ts": function srcAppMyLocalstorageTs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "MyLocalStorage", function () {
      return MyLocalStorage;
    });

    var MyLocalStorage = /*#__PURE__*/function () {
      function MyLocalStorage() {
        _classCallCheck(this, MyLocalStorage);
      }

      _createClass(MyLocalStorage, null, [{
        key: "setData",
        value: function setData(key, value) {
          if (!value) {
            localStorage.removeItem(key);
          } else {
            localStorage.setItem(key, JSON.stringify(value));
          }
        }
      }, {
        key: "getData",
        value: function getData(key) {
          var value = localStorage.getItem(key);

          if (value) {
            value = JSON.parse(value);
          }

          if (value == 'null') {
            value = null;
          }

          return value || '';
        }
      }]);

      return MyLocalStorage;
    }(); // Schedule Builder filter Keys---------------------------------


    MyLocalStorage.DSB_DATE_KEY = "date";
    MyLocalStorage.DSB_REGION_KEY = "regionId";
    MyLocalStorage.DSB_OBJECTFILTER_KEY = "objectFilter";
    MyLocalStorage.DSB_DATEFILTER_KEY = "dateFilter";
    MyLocalStorage.DSB_WINDOWMODE_KEY = "windowMode";
    MyLocalStorage.DSB_TOGGLEREQUESTMODE_KEY = "toggleRequestMode";
    MyLocalStorage.DSB_READONLY_KEY = "readOnlyMode";
    MyLocalStorage.DSB_FILTER_KEY = "dsbviewFilter"; // Wally Boards Filter Keys-------------------------------------

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
    /***/
  }
}]);
//# sourceMappingURL=common-es5.js.map