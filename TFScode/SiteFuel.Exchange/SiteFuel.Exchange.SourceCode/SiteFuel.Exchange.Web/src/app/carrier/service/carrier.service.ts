import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { DipTestViewModel, DeliveryRequestViewModel, CustomerJobsForCarrier, SplitDeliveryRequestModel, DRReportFilterViewModel, DRReportFilterInputViewModel, DeliveryRequestReportGridModel, SplitBlendDRModel, ScheduleBuilderModel } from '../models/DispatchSchedulerModels';
import { CustomerDetailsViewModel } from '../models/CustomerDetailsViewModel';
import { LocationDetailsModel } from '../models/location';
import { AdditiveOrderViewModel, DropdownItem } from 'src/app/statelist.service';
import { TruckDetailsModel } from '../model';
import { ProductMappingGridModel } from 'src/app/self-service-alias/models/ProductMappingModel';
import { CarrierDetailsViewModel } from '../models/CarrierDetailsViewModel';
import { ShiftModel } from '../../calendar/model';



@Injectable({
    providedIn: 'root'
})
export class CarrierService extends HandleError {
    //private GetTerminalsUrl = '/Location/GetTerminals';
    private postTruckData = '/Carrier/Freight/Create';
    private urlGetAllCustomerData = '/Carrier/Order/GetAllCustomerData';
    private urlSaveAndUpdateCustomerMapping = '/Carrier/Order/SaveCarrierCustomerMapping';
    private urlCustomerIdNotTaken = '/Carrier/Order/CheckDuplicateCustomerId';
    private postCheckDuplicateTerminalId = 'Carrier/SelfServiceAlias/CheckDuplicateTerminalId';
    private urlGetBlendCompatibleProductTypes = 'Carrier/ScheduleBuilder/GetBlendProductTypeMapping';

    private urlGetHeldDrCount = '/Carrier/Freight/GetHeldDeliveryRequestCount';
    private urlCheckAndLockDr = 'Carrier/ScheduleBuilder/CheckAndLockDRs';
    private urlCheckAndReleaseDr = 'Carrier/ScheduleBuilder/CheckAndReleaseDRs';
    private urlDeleteTruckData = '/Carrier/Freight/DeleteTruck';
    private getDipTestData = '/Freight/GetDipTestData?jobId=';
    private urlIsTankNotAvailableForOrderProducts = '/Freight/IsTankNotAvailableForOrderProducts?jobId=';
    private getSiteList = '/Freight/GetSiteList?regionId=';
    private getJobListForCarrierUrl = '/Freight/GetJobListForCarrier?regionId=';
    private urlGetTrucks = '/Carrier/Freight/GetAllTruckDetails';
    private postRaiseRequestData = '/Freight/RaiseRequests';
    private postSingleRaiseRequestData = '/Freight/RaiseRequest';
    private updateDeliveryRequestData = '/Freight/UpdateDeliveryRequest';
    private addSubDeliveryRequestUrl = '/Freight/AddSubDrs';
    private changeBrokeredDrStatusUrl = '/Freight/ChangeBrokeredDrStatus?drId=';
    private getDeliveryRequestsUrl = '/Freight/GetDeliveryRequests?regionId=';
    private getBrokeredDrAssignedToMeUrl = '/Freight/GetBrokeredDrRequestedToMe?regionId=';
    private getBrokeredDrAssignedByMeUrl = '/Freight/GetBrokeredDrRequestedByMe?regionId=';
    private getDeliveryRequestByIdUrl = '/Freight/GetDeliveryRequestById?deliveryRequestId=';
    private getCalendarDeliveryRequestsUrl = '/Freight/GetCalendarDeliveryRequests';
    private urlGetCompantDrivers = '/Freight/GetCompantDrivers';
    private urlGetFuelTypes = '/Freight/GetFuelTypes';
    private getDemandCaptureChartdata = '/Driver/Dashboard/GetDemandCaptureChartdata?SiteId='; // to use in both buyer and supplier side
    private getOrdersForJobUrl = '/Carrier/ScheduleBuilder/GetOrdersForJobOfCustomerAndSupplier?jobId=';
    private urlGetStates = '/Carrier/SelfServiceAlias/GetStates?countryId=';
    public urlGetCities = '/Carrier/SelfServiceAlias/GetCities';
    public urlGetTerminals = '/Carrier/SelfServiceAlias/GetTerminals';
    private urlGetBulkPlants = '/Carrier/SelfServiceAlias/GetBulkPlants';
    private urlGetTerminalsForMapping = '/Carrier/SelfServiceAlias/GetTerminalsForMapping';
    private urlDeleteTerminalMappingById = '/Carrier/SelfServiceAlias/DeleteTerminalMappingById';
    private urlTerminalMappingGrid = '/Carrier/SelfServiceAlias/GetTerminalMappingGrid?SelectedCountryId=';

    private urlGetServingFuelTypesByCompany = '/Carrier/SelfServiceAlias/GetServingFuelTypesByCompany?companyId=';
    private postSaveProductMapping = 'Carrier/SelfServiceAlias/SaveProductMapping';
    private postSaveTerminalMapping = 'Carrier/SelfServiceAlias/SaveTerminalMapping';
    private urlGetProductMappingDetails = '/Carrier/SelfServiceAlias/GetProductMappingDetails';
    private urlDownloadProductMappingTemplate = '/Carrier/SelfServiceAlias/DownloadProductMappingTemplate?id=';
    private urlBulkUploadTemplate = '/Carrier/SelfServiceAlias/UploadProductMappingTemplate';
    private urlDefaultCountry = '/Carrier/SelfServiceAlias/GetCountries?companyId=';
    private urlDefaultServingCountry = '/Carrier/SelfServiceAlias/GetDefaultServingCountry?companyId=';
    private urlDeleteProductMappingById = '/Carrier/SelfServiceAlias/DeleteProductMappingById';
    private urlUpdateProductNames = '/Carrier/SelfServiceAlias/UpdateProductNames';
    private urlUpdateTerminalId = '/Carrier/SelfServiceAlias/UpdateTerminalId';
    private urlCheckLocationAssignedToCarrier = '/Freight/CheckLocationAssignedToCarrier?jobId=';
    //private urlGetCarrierData = '/Carrier/Order/GetCarrierData';
    private urlCheckDuplicateCarrierId = '/Carrier/Order/CheckDuplicateCarrierId';
    private urlSaveAndUpdateCarrierMapping = '/Carrier/Order/SaveAndUpdateCarrierMapping';
    private deleteRecurringSchedule = '/Freight/DeleteRecurringSchedule';
    private urlgetDefaultScheduleData = '/ScheduleBuilder/GetDefaultScheduleData';
    private getRecurringScheduleDetailsUrl = '/Carrier/ScheduleBuilder/GetRecurringScheduleDetails';
    private getCreateLoadJobListForCarrierUrl = '/Freight/GetCreateLoadJobListForCarrier?regionId=';
    private getGetBrokerJobOrderDetails = '/Freight/GetBrokerJobOrderDetails';
    private urlDownloadTerminalItemCodeMappingTemplate = '/Carrier/SelfServiceAlias/DownloadTerminalItemCodeMappingTemplate?id=';
    private urlBulkUploadTerminalItemCodeMappingFile = '/Carrier/SelfServiceAlias/BulkUploadTerminalItemCodeMappingFile';
    private urlsplitDeliveryRequestData = '/Freight/CreateSplitDeliveryRequests';
    private urlsplitBlendDeliveryRequestData = '/Freight/CreateSplitBlendDeliveryRequests';
    private urlGetDRReportDropDownFilters = '/Freight/GetDRReportDropDownFilters'; // API is in base controller
    private urlGetDRReportFilteredData = '/Freight/GetDRReportData';

    private urlGetTerminalSuppliers = 'Carrier/SelfServiceAlias/GetTerminalSuppliers?countryId=';

    private urlGetDefaultUOM = 'Carrier/Tractor/GetDefaultUOM';

    private urlgetDefaultTBDScheduleData = '/ScheduleBuilder/GetDefaultTBDScheduleData';

    //CarrierId mapping Urls 
    private urlGetAssignedTerminalIdsForMapping = '/Carrier/SelfServiceAlias/GetAssignedTerminalIdsForMapping';
    private urlSaveCarrierMapping = '/Carrier/SelfServiceAlias/SaveCarrierMapping';
    private urlGetCarrierData = '/Carrier/SelfServiceAlias/GetCarrierIDMappings';
    private urlDeleteCarrierIDMappings = '/Carrier/SelfServiceAlias/DeleteCarrierIDMapping?mappingId=';
    private urlGetCreateDrSetting = '/ScheduleBuilder/GetCreateDrSetting';
    private urlGetAdditiveOrders = '/ScheduleBuilder/GetAdditiveOrders?regionId=';
    //calendar sb data
    private getGetSheduleBuilderUrl = '/Carrier/ScheduleBuilder/GetSheduleBuilder?regionId=';
    private getSheduleCalendarDataUrl = '/Carrier/ScheduleBuilder/GetSheduleCalendarData?regionId=';
    private saveDriverViewUrl = '/Carrier/ScheduleBuilder/SaveCalendarDeliveryRequest';

    //Filter
    private urlFilterDataForDsbCalenderView = '/Carrier/Order/GetFilterDataForCalenderView';

    constructor(private httpClient: HttpClient) { super(); }

    getAllTrucks(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetTrucks)
            .pipe(catchError(this.handleError<any>('getAllTrucks', null)));
    }

    updateHeldDrCount(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetHeldDrCount)
            .pipe(catchError(this.handleError<any>('updateHeldDrCount', null)));
    }


    CustomerIdNotTaken(customerDetail: CustomerDetailsViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlCustomerIdNotTaken, customerDetail)
            .pipe(catchError(this.handleError<any>('CustomerIdNotTaken', null)));
    }

    checkDuplicateTerminalId(TerminalMappingViewModel: LocationDetailsModel): Observable<any> {
        return this.httpClient.post<any>(this.postCheckDuplicateTerminalId, TerminalMappingViewModel)
            .pipe(catchError(this.handleError<any>('checkDuplicateTerminalId', null)));
    }
    getSheduleCalendarData(regionId: string, date: string): Observable<ShiftModel[]> {
        return this.httpClient.get<ShiftModel[]>(this.getSheduleCalendarDataUrl + regionId + '&date=' + date)
            .pipe(catchError(this.handleError<ShiftModel[]>('getSheduleCalendarData', null)));
    }
    getScheduleBuilder(regionId: string, date: string, sbViewId: number, sbDsbView: number): Observable<ScheduleBuilderModel> {
        return this.httpClient.get<ScheduleBuilderModel>(this.getGetSheduleBuilderUrl + regionId + '&date=' + date + '&sbView=' + sbViewId + '&sbDsbView=' + sbDsbView)
            .pipe(catchError(this.handleError<ScheduleBuilderModel>('getScheduleBuilder', null)));
    }
    saveDriverView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.saveDriverViewUrl, sbModel).pipe(catchError(this.handleError<any>('saveDriverView', null)));
    }

    getCompatibleProductTypes(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetBlendCompatibleProductTypes)
            .pipe(catchError(this.handleError<any>('getCompatibleProductTypes', null)));
    }

    getAllCustomerData(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetAllCustomerData)
            .pipe(catchError(this.handleError<any>('getAllCustomerData', null)));
    }

    saveAndUpdateCustomerMapping(customerMapping: CustomerDetailsViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveAndUpdateCustomerMapping, customerMapping)
            .pipe(catchError(this.handleError<any>('saveAndUpdateCustomerMapping', null)));
    }

    checkAndLockDr(drIds: string[]): Observable<any> {
        return this.httpClient.post<any>(this.urlCheckAndLockDr, drIds)
            .pipe(catchError(this.handleError<any>('checkAndLockDr', null)));
    }

    checkAndReleaseDr(drIds: string[]): Observable<any> {
        return this.httpClient.post<any>(this.urlCheckAndReleaseDr, drIds)
            .pipe(catchError(this.handleError<any>('checkAndReleaseDr', null)));
    }

    getCompanyDrivers(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCompantDrivers)
            .pipe(catchError(this.handleError<any>('getCompanyDrivers', null)));
    }

    postCreateTruck(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postTruckData, inputData)
            .pipe(catchError(this.handleError<any>('postCreateTruck', null)));
    }
    postDeleteTruck(inputData: TruckDetailsModel): Observable<any> {
        return this.httpClient.post<any>(this.urlDeleteTruckData, inputData)
            .pipe(catchError(this.handleError<any>('postDeleteTruck', null)));
    }

    getDipTests(_jobId: string, regionId: string, buyerCompanyId: number, requestFromBuyerWallyBoard: boolean, isShowForm: boolean): Observable<DipTestViewModel[]> {
        return this.httpClient.get<DipTestViewModel[]>(this.getDipTestData + _jobId + "&regionId=" + regionId + "&buyerCompanyId=" + buyerCompanyId + "&requestFromBuyerWallyBoard=" + requestFromBuyerWallyBoard + "&isCreateDR=" + isShowForm)
            .pipe(catchError(this.handleError<DipTestViewModel[]>('getDipTests', null)));
    }

    isTankNotAvailableForOrderProducts(jobId: number, customerId: number, regionId: any, productTypeIds: number[], endSupplier: number = 0): Observable<any> {
        if (endSupplier == 1) {
            return this.httpClient.get<any>(this.urlIsTankNotAvailableForOrderProducts + jobId + "&customerId=" + customerId + "&regionId=" + regionId + "&endSupplier=" + endSupplier + "&skipMarineConversion=" + productTypeIds)
                .pipe(catchError(this.handleError<any>('getOrdersForJob', null)));
        }
        else {
            return this.httpClient.get<any>(this.urlIsTankNotAvailableForOrderProducts + jobId + "&customerId=" + customerId + "&regionId=" + regionId + "&productTypeIds=" + productTypeIds)
                .pipe(catchError(this.handleError<any>('getOrdersForJob', null)));
        }
    }

    getSites(regionId: string): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getSiteList + regionId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getSites', null)));
    }

    getJobListForCarrier(regionId: string, isShowCarrierManaged?: boolean, SelectedCarrierId?: string): Observable<CustomerJobsForCarrier[]> {
        if (isShowCarrierManaged == undefined || isShowCarrierManaged == null) {
            isShowCarrierManaged = false;
        }
        if (SelectedCarrierId == undefined) {
            SelectedCarrierId = "";
        }
        return this.httpClient.get<CustomerJobsForCarrier[]>(this.getJobListForCarrierUrl + regionId + "&isShowCarrierManaged=" + isShowCarrierManaged + "&carriers=" + SelectedCarrierId)
            .pipe(catchError(this.handleError<CustomerJobsForCarrier[]>('getJobListForCarrier', null)));
    }
    getCreateLoadJobListForCarrier(regionId: string): Observable<CustomerJobsForCarrier[]> {
        return this.httpClient.get<CustomerJobsForCarrier[]>(this.getCreateLoadJobListForCarrierUrl + regionId)
            .pipe(catchError(this.handleError<CustomerJobsForCarrier[]>('getJobListForCarrier', null)));
    }
    getOrdersForJob(jobId: number, customerId: number, regionId: any, skipMarineConversion: boolean, endSupplier: number = 0, productsToExclude: number[] = []): Observable<any> {
        let products = '';
        if (productsToExclude && productsToExclude.length > 0) {
            products = productsToExclude.join(',');
        }

        if (endSupplier == 1) {
            return this.httpClient.get<any>(this.getOrdersForJobUrl + jobId + "&customerId=" + customerId + "&regionId=" + regionId + "&endSupplier=" + endSupplier + "&skipMarineConversion=" + skipMarineConversion + "&productsToExclude=" + products)
                .pipe(catchError(this.handleError<any>('getOrdersForJob', null)));
        }
        else {
            return this.httpClient.get<any>(this.getOrdersForJobUrl + jobId + "&customerId=" + customerId + "&regionId=" + regionId + "&endSupplier=" + 0 + "&skipMarineConversion=" + skipMarineConversion + "&productsToExclude=" + products)
                .pipe(catchError(this.handleError<any>('getOrdersForJob', null)));
        }
    }

    postRaiseRequests(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postRaiseRequestData, inputData)
            .pipe(catchError(this.handleError<any>('postRaiseRequests', null)));
    }

    postRaiseRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postSingleRaiseRequestData, inputData)
            .pipe(catchError(this.handleError<any>('postRaiseRequest', null)));
    }

    getDeliveryRequests(regionId: string, selectedDate?: string): Observable<DeliveryRequestViewModel[]> {
        return this.httpClient.get<DeliveryRequestViewModel[]>(this.getDeliveryRequestsUrl + regionId + "&selectedDate=" + selectedDate)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel[]>('getDeliveryRequests', null)));
    }

    getBrokeredDrAssignedToMe(regionId: string, selectedDate?: string): Observable<DeliveryRequestViewModel[]> {
        if (regionId != null && !regionId.match(/^[0-9a-fA-F]{24}$/)) { regionId = null; }
        return this.httpClient.get<DeliveryRequestViewModel[]>(this.getBrokeredDrAssignedToMeUrl + regionId + "&selectedDate=" + selectedDate)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel[]>('getBrokeredDrAssignedToMe', null)));
    }

    getBrokeredDrAssignedByMe(regionId: string, selectedDate?: string): Observable<DeliveryRequestViewModel[]> {
        if (regionId != null && !regionId.match(/^[0-9a-fA-F]{24}$/)) { regionId = null; }
        return this.httpClient.get<DeliveryRequestViewModel[]>(this.getBrokeredDrAssignedByMeUrl + regionId + "&selectedDate=" + selectedDate)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel[]>('getBrokeredDrAssignedByMe', null)));
    }
    getDeliveryRequestById(deliveryRequestId: string): Observable<DeliveryRequestViewModel> {
        return this.httpClient.get<DeliveryRequestViewModel>(this.getDeliveryRequestByIdUrl + deliveryRequestId)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel>('getDeliveryRequestById', null)));
    }
    getCalendarDeliveryRequests(inputModel): Observable<DeliveryRequestViewModel[]> {
        return this.httpClient.post<DeliveryRequestViewModel[]>(this.getCalendarDeliveryRequestsUrl, inputModel)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel[]>('getCalendarDeliveryRequests', null)));
    }
    updateDeliveryRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.updateDeliveryRequestData, inputData)
            .pipe(catchError(this.handleError<any>('updateDeliveryRequest', null)));
    }
    addSubDrs(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.addSubDeliveryRequestUrl, inputData)
            .pipe(catchError(this.handleError<any>('addSubDrs', null)));
    }

    getFuelTypes(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetFuelTypes)
            .pipe(catchError(this.handleError<any>('getFuelTypes', null)));
    }

    public getDemandCapChartData(SiteId, days, tfxJobId): Observable<any> {
        return this.httpClient.get<any>(`${this.getDemandCaptureChartdata}${SiteId}&noOfDays=${days}&tfxJobId=${tfxJobId}`)
            .pipe(catchError(this.handleError<any>('getDemandCapChartData', null)));
    }

    GetCountries(companyId: number): Observable<any> {
        return this.httpClient.get<any>(this.urlDefaultCountry + companyId)
            .pipe(catchError(this.handleError<any>('GetCountries', null)));
    }

    getDefaultServingCountry(companyId: number): Observable<any> {
        return this.httpClient.get<any>(this.urlDefaultServingCountry + companyId)
            .pipe(catchError(this.handleError<any>('getDefaultServingCountry', null)));
    }

    getStates(countryId: number): Observable<any> {
        return this.httpClient.get<any>(this.urlGetStates + countryId)
            .pipe(catchError(this.handleError<any>('getStates', null)));
    }

    getCities(stateIds: number[]): Observable<any> {
        return this.httpClient.post<any>(this.urlGetCities, stateIds)
            .pipe(catchError(this.handleError<any>('getCities', null)));
    }

    getTerminals(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.urlGetTerminals, filter)
            .pipe(catchError(this.handleError<any>('getTerminals', null)));
    }

    getBulkPlants(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.urlGetBulkPlants, filter)
            .pipe(catchError(this.handleError<any>('getBulkPlants', null)));
    }

    getTerminalMappingGrid(SelectedCountryId: number): Observable<any> {
        return this.httpClient.get(this.urlTerminalMappingGrid + SelectedCountryId)
            .pipe(catchError(this.handleError<any>('getTerminalsForMapping', null)));
    }
    getTerminalsForMapping(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.urlGetTerminalsForMapping, filter)
            .pipe(catchError(this.handleError<any>('getTerminalsForMapping', null)));
    }

    getServingFuelTypesByCompany(companyId: number): Observable<any> {
        return this.httpClient.get<any>(this.urlGetServingFuelTypesByCompany + companyId)
            .pipe(catchError(this.handleError<any>('getServingFuelTypesByCompany', null)));
    }

    saveProductMapping(productMapping: any): Observable<any> {
        return this.httpClient.post<any>(this.postSaveProductMapping, productMapping)
            .pipe(catchError(this.handleError<any>('saveProductMapping', null)));
    }
    saveTerminalMapping(productMapping: any): Observable<any> {
        return this.httpClient.post<any>(this.postSaveTerminalMapping, productMapping)
            .pipe(catchError(this.handleError<any>('saveTerminalMapping', null)));
    }

    getProductMappingGridDetails(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.urlGetProductMappingDetails, filter)
            .pipe(catchError(this.handleError<any>('getProductMappingGridDetails', null)));
    }

    downloadProductMappingTemplate(timestamp: number): Observable<Blob> {
        return this.httpClient.get(this.urlDownloadProductMappingTemplate + timestamp, {
            responseType: 'blob'
        }).pipe(catchError(this.handleError<any>('downloadProductMappingTemplate', null)));
    }

    postBulkUploadTemplate(files: any): Observable<any> {
        return this.httpClient.post<any>(this.urlBulkUploadTemplate, files)
            .pipe(catchError(this.handleError<any>('postBulkUploadTemplate', null)));
    }

    postDeleteProductMapping(model: ProductMappingGridModel): Observable<any> {
        return this.httpClient.post<any>(this.urlDeleteProductMappingById, model)
            .pipe(catchError(this.handleError<any>('postDeleteProductMapping', null)));
    }

    postDeleteTerminalMappingById(model: LocationDetailsModel): Observable<any> {
        return this.httpClient.post<any>(this.urlDeleteTerminalMappingById, model)
            .pipe(catchError(this.handleError<any>('postDeleteProductMapping', null)));
    }


    updateProductNames(model: ProductMappingGridModel[]): Observable<any> {
        return this.httpClient.post<any>(this.urlUpdateProductNames, model)
            .pipe(catchError(this.handleError<any>('updateProductNames', null)));
    }

    updateTerminalId(model: LocationDetailsModel): Observable<any> {
        return this.httpClient.post<any>(this.urlUpdateTerminalId, model)
            .pipe(catchError(this.handleError<any>('updateProductNames', null)));
    }


    checkLocationAssignedToCarrier(jobId: number): Observable<any> {
        return this.httpClient.get(this.urlCheckLocationAssignedToCarrier + jobId)
            .pipe(catchError(this.handleError<any>('checkLocationAssignedToCarrier', null)));
    }

    getCarrierData(countryId): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCarrierData + "?countryId=" + countryId)
            .pipe(catchError(this.handleError<any>('getCarrierData', null)));
    }

    checkDuplicateCarrierId(carrierDetail: CarrierDetailsViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlCheckDuplicateCarrierId, carrierDetail)
            .pipe(catchError(this.handleError<any>('CheckDuplicateCarrierId', null)));
    }

    saveAndUpdateCarrierMapping(carrierMapping: CarrierDetailsViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveAndUpdateCarrierMapping, carrierMapping)
            .pipe(catchError(this.handleError<any>('saveAndUpdateCarrierMapping', null)));
    }

    deleteRecurringScheduleDetails(recurringId: string): Observable<any> {
        var data = { Id: recurringId };
        return this.httpClient.post<any>(this.deleteRecurringSchedule, data)
            .pipe(catchError(this.handleError<any>('deleteRecurringScheduleDetails', null)));
    }

    getDefaultScheduleData(): Observable<any> {
        return this.httpClient.get<any>(this.urlgetDefaultScheduleData)
            .pipe(catchError(this.handleError<any>('getDefaultScheduleData', null)));
    }

    getRecurringScheduleDetails(JobId: number, PoNumb: string, JOBSiteId: string, productTypeId: number): Observable<any> {
        var data = { jobId: JobId, PoNumber: PoNumb, JobSiteId: JOBSiteId, productTypeId: productTypeId };
        return this.httpClient.post<any>(this.getRecurringScheduleDetailsUrl, data)
            .pipe(catchError(this.handleError<any>('getRecurringScheduleDetails', null)));
    }

    changeBrokeredDrStatus(drId: string, blendedGroupId: string, status: any): Observable<any> {
        return this.httpClient.post<any>(this.changeBrokeredDrStatusUrl + drId + '&blendedGroupId=' + blendedGroupId + '&status=' + status, {})
            .pipe(catchError(this.handleError<any>('changeBrokeredDrStatus', null)));
    }

    downloadTerminalItemCodeMappingTemplate(timestamp: number): Observable<Blob> {
        return this.httpClient.get(this.urlDownloadTerminalItemCodeMappingTemplate + timestamp, {
            responseType: 'blob'
        }).pipe(catchError(this.handleError<any>('downloadTerminalItemCodeMappingTemplate', null)));
    }

    postBulkUploadTerminalItemCodeMappingTemplate(files: any): Observable<any> {
        return this.httpClient.post<any>(this.urlBulkUploadTerminalItemCodeMappingFile, files)
            .pipe(catchError(this.handleError<any>('postBulkUploadTerminalItemCodeMappingTemplate', null)));
    }
    splitDeliveryRequests(inputData: SplitDeliveryRequestModel): Observable<any> {
        return this.httpClient.post<any>(this.urlsplitDeliveryRequestData, inputData)
            .pipe(catchError(this.handleError<any>('splitDeliveryRequests', null)));
    }
    splitBlendDeliveryRequests(inputData: SplitBlendDRModel[]): Observable<any> {
        return this.httpClient.post<any>(this.urlsplitBlendDeliveryRequestData, inputData)
            .pipe(catchError(this.handleError<any>('splitBlendDeliveryRequests', null)));
    }
    getDRReportFilters(): Observable<DRReportFilterViewModel> {
        return this.httpClient.get<DRReportFilterViewModel>(this.urlGetDRReportDropDownFilters)
            .pipe(catchError(this.handleError<DRReportFilterViewModel>('getDRReportFilterData', null)));
    }

    getDRReportGridData(inputData: DRReportFilterInputViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlGetDRReportFilteredData, inputData)
            .pipe(catchError(this.handleError<DeliveryRequestReportGridModel>('getDRReportGridData', null)));
    }
    getTerminalSupplier(selectedCountryId: number): Observable<any> {
        return this.httpClient.get<any>(this.urlGetTerminalSuppliers + selectedCountryId)
            .pipe(catchError(this.handleError<DropdownItem>('getTerminalSupplier', null)));
    }
    getDefaultUOM(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetDefaultUOM)
            .pipe(catchError(this.handleError<any>('getDefaultUOM', null)));
    }
    getDefaultTBDScheduleData(): Observable<any> {
        return this.httpClient.get<any>(this.urlgetDefaultTBDScheduleData)
            .pipe(catchError(this.handleError<any>('getDefaultTBDScheduleData', null)));
    }
    getAssignedTerminalIdsForMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetAssignedTerminalIdsForMapping)
            .pipe(catchError(this.handleError<any>('getAssignedTerminalIdsForMapping', null)));
    }
    SaveCarrierMapping(carrierMapping: CarrierDetailsViewModel) {
        return this.httpClient.post(this.urlSaveCarrierMapping, carrierMapping).
            pipe(catchError(this.handleError<any>('SaveCarrierMapping', null)))
    }
    deleteCarrierMapping(mappingId: number) {
        return this.httpClient.get<any>(this.urlDeleteCarrierIDMappings + mappingId)
            .pipe(catchError(this.handleError<any>('deleteCarrierMapping', null)));
    }
    getCreateDrSetting(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCreateDrSetting)
            .pipe(catchError(this.handleError<any>('getCreateDrSetting', null)));
    }
    getAdditiveOrders(regionId?: string): Observable<AdditiveOrderViewModel[]> {
        return this.httpClient.get<any>(this.urlGetAdditiveOrders + regionId)
            .pipe(catchError(this.handleError<AdditiveOrderViewModel[]>('getAdditiveOrders', null)));
    }
    getFilterDataForDsbCalenderView(): Observable<any> {
        return this.httpClient.get<any>(this.urlFilterDataForDsbCalenderView)
            .pipe(catchError(this.handleError<any>('getFilterDataForDsbCalenderView', null)));
    }
}
