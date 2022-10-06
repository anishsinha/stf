import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable, timer, BehaviorSubject } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { DipTestViewModel, DeliveryRequestViewModel, CustomerJobsForCarrier, ForecastingTankViewModel, ForecastingInventoryViewModel, ForecastingEstimatedUsageViewModel, ForecastingDeliveryViewModel, ForecastingExistingScheduleViewModel, ForecastingTankChartViewModel } from '../models/DispatchSchedulerModels';
import { DropdownItem } from 'src/app/statelist.service';
import { TruckDetailsModel } from '../model';
import { ForecastingLocationFilter } from 'src/app/dispatcher/dispatcher.model';
import { Exception } from 'src/app/app.enum';

@Injectable({
    providedIn: 'root'
})

export class DispatcherService extends HandleError {
    private postTruckData = '/Carrier/Freight/Create';
    private urlDeleteTruckData = '/Carrier/Freight/DeleteTruck';
    private getDipTestData = '/Freight/GetDipTestData?jobId=';
    private getSiteList = '/Freight/GetSiteList?regionId=';
    private getJobListForCarrierUrl = '/Freight/GetJobListForCarrier?regionId=';
    private urlGetTrucks = '/Carrier/Freight/GetAllTruckDetails';
    private postRaiseRequestData = '/Freight/RaiseRequests';
    private postSingleRaiseRequestData = '/Freight/RaiseRequest';
    private updateDeliveryRequestData = '/Freight/UpdateDeliveryRequest';
    private getDeliveryRequestsUrl = '/Freight/GetDeliveryRequests?regionId=';
    private urlGetCompantDrivers = '/Freight/GetCompantDrivers';
    private urlGetFuelTypes = '/Freight/GetFuelTypes';
    private getDemandCaptureChartdata = '/Freight/GetDemandCaptureChartdata?SiteId=';
    private getDispatcherLoadsUrl = '/Dispatcher/Dashboard/GetDispatcherLoads';
    private getOnGoingLoadsUrl = '/Dispatcher/Dashboard/GetOnGoingLoads';
    private getDispatcherCountryUrl = '/Dispatcher/Dashboard/GetUserCountry';
    private getDispatcherRegionUrl = '/Dispatcher/Dashboard/GetRegions';
    private getStatesUrl = '/Dispatcher/Dashboard/GetStateDetails/?countryId=';
    private getCountryUrl = '/Dispatcher/Dashboard/GetCountryDetails';
    private getCityUrl = '/Dispatcher/Dashboard/GetCities/?stateId=';
    private getProductTypeUrl = '/Dispatcher/Dashboard/GetProductTypes';
    private getJobLocationDetailsUrl = '/Dispatcher/Dashboard/GetJobLocationDetails';
    private getDipTestDetailsUrl = '/Dispatcher/Dashboard/GetDipTestDetails?';
    private getDriverAdditionalDetailsUrl = '/Dispatcher/Dashboard/GetDriverAdditionalDetails?driverId=';
    private getDropTicketDetailsUrl = '/Dispatcher/Dashboard/DDTInvoiceDetails?invoiceHeaderId=';
    public SingleMultiWindowSubject: BehaviorSubject<number>;
    private getSalesDataUrl = '/Supplier/Sales/GetSalesData?regionId=';
    private getBuyerSalesDataUrl = '/Buyer/Sales/GetSalesData?jobId=';
    private getDeliveryDetailsUrl = '/Supplier/Sales/GetExistingSchedules?jobId=';
    private getScheduleQtyTypeUrl = '/Supplier/DeliveryGroup/GetScheduleQtyType';
    private getBuyerScheduleQtyTypeUrl = '/Buyer/Sales/GetBuyerScheduleQtyType';
    private postRaiseDeliveryRequestUrl = '/Supplier/Sales/RaiseDeliveryRequest';

    private getBuyerDeliveryDetailsUrl = '/Buyer/Sales/GetExistingSchedules?jobId=';
    private postBuyerRaiseDeliveryRequestUrl = '/Buyer/Sales/RaiseDeliveryRequest';
    private getRaisedExceptionsUrl = 'Supplier/Sales/getRaisedExceptions?ExceptionTypes='
    private getRaisedExceptionsBuyerUrl = 'Buyer/Sales/getRaisedExceptions?ExceptionTypes='
    private getBuyerForcastingSettingUrl = '/Buyer/Sales/GetForecastingSetting';
    private getForcastingSettingUrl = '/Supplier/Sales/GetForecastingSetting';
    //private getBuyerForcastingLocationDetailsUrl = '/Buyer/Sales/GetFilteredLocations?regionId=';
    //private getForcastingLocationDetailsUrl = '/Supplier/Sales/GetFilteredLocations?regionId=';
    private getBuyerForcastingTankDetailsUrl = '/Buyer/Sales/GetForecastingTankDetails';
    private getForcastingTankDetailsUrl = '/Supplier/Sales/GetForecastingTankDetails';
    private getBuyerForcastingTankInventoryDetailsUrl = '/Buyer/Sales/GetForecastingTankInventoryDetails';
    private getForcastingTankInventoryDetailsUrl = '/Supplier/Sales/GetForecastingTankInventoryDetails';
    private getBuyerForcastingTankEstimatedUsageDetailsUrl = '/Buyer/Sales/GetForecastingTankEstimatedUsageDetails';
    private getForcastingTankEstimatedUsageDetailsUrl = '/Supplier/Sales/GetForecastingTankEstimatedUsageDetails';
    private getBuyerForcastingTankDeliveryDetailsUrl = '/Buyer/Sales/GetForecastingTankDeliveryDetails';
    private getForcastingTankDeliveryDetailsUrl = '/Supplier/Sales/GetForecastingTankDeliveryDetails';
    private getBuyerForcastingTankScheduleUrl = '/Buyer/Sales/GetForecastingTankScheduleDetails';
    private getForcastingTankScheduleUrl = '/Supplier/Sales/GetForecastingTankScheduleDetails';
    private getBuyerForcastingTankChartDetailsUrl = '/Buyer/Sales/GetForecastingTankDataForChart';
    private getForcastingTankChartDetailsUrl = '/Supplier/Sales/GetForecastingTankDataForChart';
    private postForcastingCalculateTankRetainWindowInfoUrl = '/Supplier/Sales/CalculateTankRetainWindowInfo';
    private postForcastingCalculateTankDetailsRetainWindowInfoUrl = '/Supplier/Sales/CalculateTankDetailsRetainWindowInfo';
    private postForcastingBuyerCalculateTankRetainWindowInfoUrl = '/Buyer/Dashboard/CalculateTankRetainWindowInfo';
    private postForcastingBuyerCalculateTankDetailsRetainWindowInfoUrl = '/Buyer/Dashboard/CalculateTankDetailsRetainWindowInfo';
    private getCarriersForSupplierUrl = '/Dispatcher/Dashboard/GetCarriersForSupplierDashboard';
    private postSupplierFilters = '/Dispatcher/Dashboard/SaveFilters';
    private getFiltersUrl = '/Dispatcher/Dashboard/GetFilters?moduleId=';
    private getDSBShiftFiltersUrl = '/Dispatcher/Dashboard/GetDSBShiftFilters?moduleId=';
    private getSupplierLocationTanksInfoUrl = "/Supplier/Sales/GetLocationTanksInfo";
    private getBuyerLocationTanksInfoUrl = "/Buyer/Sales/GetLocationTanksInfo";
    private postForcastingCalculateProductRetainWindowInfoUrl = '/Supplier/Sales/CalculateProductRetainWindowInfo';

    constructor(private httpClient: HttpClient) {
        super();
        this.SingleMultiWindowSubject = new BehaviorSubject<any>(1); //singlemulti window screen 1
    }

    getAllTrucks(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetTrucks)
            .pipe(catchError(this.handleError<any>('getAllTrucks', null)));
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

    getDipTests(_jobId: string, regionId: string): Observable<DipTestViewModel[]> {
        return this.httpClient.get<DipTestViewModel[]>(this.getDipTestData + _jobId + "&regionId=" + regionId)
            .pipe(catchError(this.handleError<DipTestViewModel[]>('getDipTests', null)));
    }

    getSites(regionId: string): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getSiteList + regionId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getSites', null)));
    }

    getJobListForCarrier(regionId: string): Observable<CustomerJobsForCarrier[]> {
        return this.httpClient.get<CustomerJobsForCarrier[]>(this.getJobListForCarrierUrl + regionId)
            .pipe(catchError(this.handleError<CustomerJobsForCarrier[]>('getJobListForCarrier', null)));
    }

    postRaiseRequests(inputData: any[]): Observable<any> {
        return this.httpClient.post<any>(this.postRaiseRequestData, inputData)
            .pipe(catchError(this.handleError<any>('postRaiseRequests', null)));
    }

    postRaiseRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postSingleRaiseRequestData, inputData)
            .pipe(catchError(this.handleError<any>('postRaiseRequest', null)));
    }

    getDeliveryRequests(regionId: string): Observable<DeliveryRequestViewModel[]> {
        return this.httpClient.get<DeliveryRequestViewModel[]>(this.getDeliveryRequestsUrl + regionId)
            .pipe(catchError(this.handleError<DeliveryRequestViewModel[]>('getDeliveryRequests', null)));
    }

    updateDeliveryRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.updateDeliveryRequestData, inputData)
            .pipe(catchError(this.handleError<any>('updateDeliveryRequest', null)));
    }

    getFuelTypes(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetFuelTypes)
            .pipe(catchError(this.handleError<any>('getFuelTypes', null)));
    }

    public getDemandCapChartData(SiteId, days): Observable<any> {
        return this.httpClient.get<any>(`${this.getDemandCaptureChartdata}${SiteId}&noOfDays=${days}`)
            .pipe(catchError(this.handleError<any>('getDemandCapChartData', null)));
    }

    public GetDispatcherRegions(): Observable<any> {
        return this.httpClient.get<any>(this.getDispatcherRegionUrl)
            .pipe(catchError(this.handleError<any>('GetDispatcherRegions', null)));
    }

    public getDispatcherLoads(inputs: any): Observable<any> {
        return this.httpClient.post<any>(this.getDispatcherLoadsUrl, inputs)
            .pipe(catchError(this.handleError<any>('getDispatcherLoads', null)));
    }

    public getOnGoingLoads(inputs: any): Observable<any> {
        return this.httpClient.post<any>(this.getOnGoingLoadsUrl, inputs)
            .pipe(catchError(this.handleError<any>('getOnGoingLoads', null)));
    }

    public getDispatcherCountry(): Observable<any> {
        return this.httpClient.get<any>(this.getDispatcherCountryUrl)
            .pipe(catchError(this.handleError<any>('getDispatcherCountry', null)));
    }

    public getJobLocationDetails(jobIds, selectedLocAttributeId): Observable<any> {
        var data = { jobList: jobIds, inventoryCaptureTypeIds: selectedLocAttributeId };
        return timer(0, 60 * 60 * 1000).pipe(
            switchMap(() => {
                return this.httpClient.post<any>(this.getJobLocationDetailsUrl, data)
            })).pipe(catchError(this.handleError<any>('getJobLocationDetails', null)));
    }

    public getStateList(countryId = '1,2,3'): Observable<any> {
        return this.httpClient.get<any>(this.getStatesUrl + countryId)
            .pipe(catchError(this.handleError<any>('getStateList', null)));
    }

    public getCountryList(): Observable<any> {
        return this.httpClient.get<any>(this.getCountryUrl)
            .pipe(catchError(this.handleError<any>('getCountryList', null)));
    }

    public getCityList(stateId = '1'): Observable<any> {
        return this.httpClient.get<any>(this.getCityUrl + stateId)
            .pipe(catchError(this.handleError<any>('getCityList', null)));
    }

    public getProductTypeList(): Observable<any> {
        return this.httpClient.get<any>(this.getProductTypeUrl)
            .pipe(catchError(this.handleError<any>('getProductTypeList', null)));
    }
    public getDipTestDetails(siteId, tankId, noOfDays): Observable<any> {
        return this.httpClient.get<any>(this.getDipTestDetailsUrl + 'siteId=' + siteId + '&' + 'tankId=' + tankId + '&' + 'noOfDays=' + noOfDays)
            .pipe(catchError(this.handleError<any>('getDipTestDetails', null)));
    }
    public getDriverAdditionalDetails(driverId: number): Observable<any> {
        return this.httpClient.get<any>(this.getDriverAdditionalDetailsUrl + driverId)
            .pipe(catchError(this.handleError<any>('getDriverAdditionalDetails', null)));
    }
    public GetDropTicketDetails(invoiceHeaderId: number): Observable<any> {
        return this.httpClient.get<any>(this.getDropTicketDetailsUrl + invoiceHeaderId)
            .pipe(catchError(this.handleError<any>('GetDropTicketDetails', null)));
    }

    public getSalesData(inputs: any): Observable<any> {
        !inputs.RegionId ? inputs.RegionId = '' : '';
        !inputs.CustomerId ? inputs.CustomerId = '' : '';
        !inputs.LocationId ? inputs.LocationId = '' : '';
        !inputs.SelectedTab ? inputs.SelectedTab = '' : 0;
        !inputs.Carriers ? inputs.Carriers = '' : '';
        !inputs.InventoryCaptureType ? inputs.InventoryCaptureType = '' : '';
        return this.httpClient.get<any>(`${this.getSalesDataUrl}${inputs.RegionId}&customerId=${inputs.CustomerId}&jobId=${inputs.LocationId}&priority=${inputs.Priority}&SelectedTab=${inputs.SelectedTab}&isShowCarrierManaged=${inputs.IsShowCarrierManaged}&carriers=${inputs.Carriers}&isRetailJob=${inputs.IsShowRetailJobs}&inventoryCaptureType=${inputs.InventoryCaptureType}`)
            .pipe(catchError(this.handleError<any>('getSalesData', null)));
    }

    public getBuyerSalesData(inputs: any): Observable<any> {
        !inputs.LocationId ? inputs.LocationId = '' : '';
        !inputs.Carriers ? inputs.Carriers = '' : '';
        !inputs.Suppliers ? inputs.Suppliers = '' : '';
        return this.httpClient.get<any>(`${this.getBuyerSalesDataUrl}${inputs.LocationId}&priority=${inputs.Priority}&SelectedTab=${inputs.SelectedTab}&isShowCarrierManaged=${inputs.IsShowCarrierManaged}&carriers=${inputs.Carriers}&isRetailJob=${inputs.IsShowRetailJobs}&supplierIds=${inputs.Suppliers}&InventoryCaptureTypeIds=${inputs.InventoryCaptureTypeIds}`)
            .pipe(catchError(this.handleError<any>('getBuyerSalesDataUrl', null)));
    }
    public GetDeliveryDetails(jobId: number, productTypeId: number): Observable<any> {
        return this.httpClient.get<any>(this.getDeliveryDetailsUrl + jobId + "&productTypeId=" + productTypeId)
            .pipe(catchError(this.handleError<any>('GetDeliveryDetails', null)));
    }

    public GetScheduleQtyType(): Observable<any> {
        return this.httpClient.get<any>(this.getScheduleQtyTypeUrl)
            .pipe(catchError(this.handleError<any>('GetScheduleQtyType', null)));
    }
    public PostRaiseDeliveryRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postRaiseDeliveryRequestUrl, inputData)
            .pipe(catchError(this.handleError<any>('PostRaiseDeliveryRequest', null)));
    }
    public GetBuyerDeliveryDetails(jobId: number, productTypeId: number): Observable<any> {
        return this.httpClient.get<any>(this.getBuyerDeliveryDetailsUrl + jobId + "&productTypeId=" + productTypeId)
            .pipe(catchError(this.handleError<any>('GetBuyerDeliveryDetails', null)));
    }

    public PostBuyerRaiseDeliveryRequest(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postBuyerRaiseDeliveryRequestUrl, inputData)
            .pipe(catchError(this.handleError<any>('PostBuyerRaiseDeliveryRequest', null)));
    }


    public GetRaisedExceptions(): Observable<any> {

        return this.httpClient.get<any>(`${this.getRaisedExceptionsUrl}${Exception.UnKnowDelivery},${Exception.MissingDelivery}`)
            .pipe(catchError(this.handleError<any>('getRaisedExceptionsUrl', null)));
    }

    public GetRaisedBuyerExceptions(): Observable<any> {
        return this.httpClient.get<any>(`${this.getRaisedExceptionsBuyerUrl}${Exception.UnKnowDelivery},${Exception.MissingDelivery}`)
            .pipe(catchError(this.handleError<any>('getRaisedExceptionsBuyerUrl', null)));
    }
    public GetBuyerScheduleQtyType(): Observable<any> {
        return this.httpClient.get<any>(this.getBuyerScheduleQtyTypeUrl)
            .pipe(catchError(this.handleError<any>('GetScheduleQtyType', null)));
    }
    public getForcastingSetting(): Observable<any> {
        return this.httpClient.get<any>(this.getForcastingSettingUrl)
            .pipe(catchError(this.handleError<any>('getForcastingSetting', null)));
    }
    public getBuyerForcastingSetting(): Observable<any> {
        return this.httpClient.get<any>(this.getBuyerForcastingSettingUrl)
            .pipe(catchError(this.handleError<any>('getBuyerForcastingSetting', null)));
    }
    // public getForcastingLocationDetails(regionId, customerIds, IsShowCarrierManaged?, Carriers?,selectedLocAttributeIds?): Observable<any> {
    //     return this.httpClient.get<any>(this.getForcastingLocationDetailsUrl + regionId + "&customerIds=" + customerIds + "&isShowCarrierManaged=" + IsShowCarrierManaged + "&carriers=" + Carriers+"&inventoryCaptureType="+selectedLocAttributeIds)
    //         .pipe(catchError(this.handleError<any>('getForcastingLocationDetails', null)));
    // }
    // public getBuyerForcastingLocationDetails(regionId, customerIds,selectedLocAttributeIds): Observable<ForecastingTankViewModel[]> {
    //     return this.httpClient.get<any>(this.getBuyerForcastingLocationDetailsUrl + regionId + "&customerIds=" + customerIds+"&inventoryCaptureType="+selectedLocAttributeIds)
    //         .pipe(catchError(this.handleError<any>('getBuyerForcastingLocationDetails', null)));
    // }
    public getForcastingTankDetails(JobIds, TankIds, StorageIds): Observable<ForecastingTankViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingTankViewModel[]>(this.getForcastingTankDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingTankViewModel[]>('getForcastingTankDetails', null)));
    }
    public getBuyerForcastingTankDetails(JobIds, TankIds, StorageIds): Observable<ForecastingTankViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingTankViewModel[]>(this.getBuyerForcastingTankDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingTankViewModel[]>('getBuyerForcastingTankDetails', null)));
    }
    public getForcastingTankInventoryDetails(JobIds, TankIds, StorageIds): Observable<ForecastingInventoryViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingInventoryViewModel[]>(this.getForcastingTankInventoryDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingInventoryViewModel[]>('getForcastingTankInventoryDetails', null)));
    }
    public getBuyerForcastingTankInventoryDetails(JobIds, TankIds, StorageIds): Observable<ForecastingInventoryViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingInventoryViewModel[]>(this.getBuyerForcastingTankInventoryDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingInventoryViewModel[]>('getBuyerForcastingTankInventoryDetails', null)));
    }
    public getForcastingTankEstimatedUsageDetails(JobIds, TankIds, StorageIds, StartDate, EndDate): Observable<ForecastingEstimatedUsageViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds, startDate: StartDate, endDate: EndDate, };
        return this.httpClient.post<ForecastingEstimatedUsageViewModel[]>(this.getForcastingTankEstimatedUsageDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingEstimatedUsageViewModel[]>('getForcastingTankEstimatedUsageDetails', null)));
    }
    public getBuyerForcastingTankEstimatedUsageDetails(JobIds, TankIds, StorageIds, StartDate, EndDate): Observable<ForecastingEstimatedUsageViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds, startDate: StartDate, endDate: EndDate };
        return this.httpClient.post<ForecastingEstimatedUsageViewModel[]>(this.getBuyerForcastingTankEstimatedUsageDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingEstimatedUsageViewModel[]>('getBuyerForcastingTankEstimatedUsageDetails', null)));
    }
    public getForcastingTankDeliveryDetails(JobIds, TankIds, StorageIds): Observable<ForecastingDeliveryViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingDeliveryViewModel[]>(this.getForcastingTankDeliveryDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingDeliveryViewModel[]>('getForcastingTankDeliveryDetails', null)));
    }
    public getBuyerForcastingTankDeliveryDetails(JobIds, TankIds, StorageIds): Observable<ForecastingDeliveryViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingDeliveryViewModel[]>(this.getBuyerForcastingTankDeliveryDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingDeliveryViewModel[]>('getBuyerForcastingTankDeliveryDetails', null)));
    }
    public getForcastingTankScheduleDetails(JobIds, TankIds, StorageIds): Observable<ForecastingExistingScheduleViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingExistingScheduleViewModel[]>(this.getForcastingTankScheduleUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingExistingScheduleViewModel[]>('getForcastingTankScheduleDetails', null)));
    }
    public getBuyerForcastingTankScheduleDetails(JobIds, TankIds, StorageIds): Observable<ForecastingExistingScheduleViewModel[]> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds };
        return this.httpClient.post<ForecastingExistingScheduleViewModel[]>(this.getBuyerForcastingTankScheduleUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingExistingScheduleViewModel[]>('getBuyerForcastingTankScheduleDetails', null)));
    }
    public getForcastingTankChartDetails(JobIds, TankIds, StorageIds, currentDateTime): Observable<ForecastingTankChartViewModel> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds, currentDate: currentDateTime };
        return this.httpClient.post<ForecastingTankChartViewModel>(this.getForcastingTankChartDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingTankChartViewModel>('getForcastingTankChartDetails', null)));
    }
    public getBuyerForcastingTankChartDetails(JobIds, TankIds, StorageIds, currentDateTime): Observable<ForecastingTankChartViewModel> {
        var inputData = { JobId: JobIds, TankId: TankIds, StorageId: StorageIds, currentDate: currentDateTime };
        return this.httpClient.post<ForecastingTankChartViewModel>(this.getBuyerForcastingTankChartDetailsUrl, inputData)
            .pipe(catchError(this.handleError<ForecastingTankChartViewModel>('getBuyerForcastingTankChartDetails', null)));
    }
    public calculateTankRetainWindowInfo(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postForcastingCalculateTankRetainWindowInfoUrl, inputData)
            .pipe(catchError(this.handleError<any>('CalculateTankRetainWindowInfo', null)));
    }
    public calculateBuyerTankRetainWindowInfo(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postForcastingBuyerCalculateTankRetainWindowInfoUrl, inputData)
            .pipe(catchError(this.handleError<any>('calculateBuyerTankRetainWindowInfo', null)));
    }
    public calculateBuyerTankDetailsRetainWindowInfo(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postForcastingBuyerCalculateTankDetailsRetainWindowInfoUrl, inputData)
            .pipe(catchError(this.handleError<any>('calculateBuyerTankRetainWindowInfo', null)));
    }
    public calculateTankDetailsRetainWindowInfo(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postForcastingCalculateTankDetailsRetainWindowInfoUrl, inputData)
            .pipe(catchError(this.handleError<any>('calculateTankDetailsRetainWindowInfo', null)));
    }
    public GetCarriersForSupplier(): Observable<any> {
        return this.httpClient.get<any>(this.getCarriersForSupplierUrl)
            .pipe(catchError(this.handleError<any>('getCarriersForSupplier', null)));
    }

    public postFiltersData(moduleId: any, inputData: any): Observable<any> {
        var data = { moduleId: moduleId, filterInput: inputData };
        return this.httpClient.post<any>(this.postSupplierFilters, data)
            .pipe(catchError(this.handleError<any>('postFiltersData', null)));
    }
    public getFilters(moduleId: any): Observable<any> {
        return this.httpClient.get<any>(this.getFiltersUrl + moduleId)
            .pipe(catchError(this.handleError<any>('getFilters', null)));
    }
    public getDSBShiftFilters(moduleId: any, regionId): Observable<any> {
        return this.httpClient.get<any>(this.getDSBShiftFiltersUrl + moduleId + '&' + 'regionId=' + regionId)
            .pipe(catchError(this.handleError<any>('getFilters', null)));
    }
    public getSupplierLocationTanks(filter: ForecastingLocationFilter): Observable<any> {
        return this.httpClient.post<any>(this.getSupplierLocationTanksInfoUrl, filter)
            .pipe(catchError(this.handleError<any>('getSupplierLocationTanks', null)));
    }
    public getBuyerLocationTanks(filter: ForecastingLocationFilter): Observable<any> {
        return this.httpClient.post<any>(this.getBuyerLocationTanksInfoUrl, filter)
            .pipe(catchError(this.handleError<any>('getBuyerLocationTanks', null)));
    }
    public getInventoryDataCaptureList() {
        return [{ Id: 0, Name: 'Not specified' }, { Id: 1, Name: 'Connected' }, { Id: 2, Name: 'Manual' }, { Id: 3, Name: 'Call-In' }, { Id: 4, Name: 'Mixed' }];
    }

    public calculateProductRetainWindowInfo(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postForcastingCalculateProductRetainWindowInfoUrl, inputData)
            .pipe(catchError(this.handleError<any>('CalculateProductRetainWindowInfo', null)));
    }

}
