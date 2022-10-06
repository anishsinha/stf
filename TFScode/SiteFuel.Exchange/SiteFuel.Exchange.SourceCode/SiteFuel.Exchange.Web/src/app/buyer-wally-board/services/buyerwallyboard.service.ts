import { Injectable } from '@angular/core';
import { catchError, switchMap } from 'rxjs/operators';
import { timer, Observable, from, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HandleError } from '../../../app/errors/HandleError';

@Injectable({
    providedIn: 'root'
})

export class BuyerwallyboardService extends HandleError {

    private getJobLocationDetailsUrl = '/Buyer/Job/GetJobLocationDetails';// Move this to Job Controller buyer area
    private getSupplierCarrierDDLUrl = '/Buyer/Job/GetSupplierCarrierForjobs';
    private getDipTestDetailsUrl = '/Buyer/Dashboard/GetDipTestDetails?';
    private getDeliveriesForLocations = '/Buyer/WallyBoard/GetDeliveriesForLocations?';
    private getFilterData = 'Buyer/WallyBoard/GetBuyerLoadFilterData';
    private getDispatcherCountryUrl = '/Buyer/WallyBoard/GetUserCountry';
    private getOnGoingLoadsUrl = '/Buyer/WallyBoard/GetOnGoingLoadsForMapView';
    private getBuyerLoadsForGridUrl = '/Buyer/WallyBoard/GetBuyerLoads';
    private getDriverAdditionalDetailsUrl = '/Buyer/WallyBoard/GetDriverAdditionalDetails?driverId='
    private getFiltersUrl = '/Buyer/WallyBoard/GetFilters?moduleId=';
    private saveFiltersUrl = '/Buyer/WallyBoard/SaveFilters';

    public SingleMultiWindowSubject: BehaviorSubject<number>;

    constructor(public httpClient: HttpClient) {
        super();
        this.SingleMultiWindowSubject = new BehaviorSubject<any>(1); //singlemulti window screen 1
    }


    public getBuyerLoadsForGrid(inputs: any): Observable<any> {
        return this.httpClient.post<any>(this.getBuyerLoadsForGridUrl, inputs)
            .pipe(catchError(this.handleError<any>('getBuyerLoadsForGrid', null)));
    }

    public getFilters(moduleId: any): Observable<any> {
        return this.httpClient.get<any>(this.getFiltersUrl + moduleId)
            .pipe(catchError(this.handleError<any>('getFilters', null)));
    }

    public saveFilters(moduleId: any, input: any): Observable<any> {
        var data = { moduleId: moduleId, filterInput: input };
        return this.httpClient.post<any>(this.saveFiltersUrl, data)
            .pipe(catchError(this.handleError<any>('saveFilters', null)));
    }

    public getDriverAdditionalDetails(driverId: number): Observable<any> {
        return this.httpClient.get<any>(this.getDriverAdditionalDetailsUrl + driverId)
            .pipe(catchError(this.handleError<any>('getDriverAdditionalDetails', null)));
    }

    public getJobLocationDetails(jobIds,selectedLocAttributeId): Observable<any> {
        var data = { jobList: jobIds,inventoryCaptureTypeIds:selectedLocAttributeId};
        return timer(0, 60 * 60 * 1000).pipe(
            switchMap(() => {
                return this.httpClient.post<any>(this.getJobLocationDetailsUrl,data)
            })).pipe(catchError(this.handleError<any>('getJobLocationDetails', null)));
    }

    public getSuppliersCarrierssDDL(jobIds: any): Observable<any> {
        return timer(0, 60 * 60 * 1000).pipe(
            switchMap(() => {
                return this.httpClient.post<any>(this.getSupplierCarrierDDLUrl, { jobIds: jobIds })
            })).pipe(catchError(this.handleError<any>('getSuppliersCarrierssDDL', null)));
    }

    public getDipTestDetails(siteId, tankId, noOfDays): Observable<any> {
        return this.httpClient.get<any>(this.getDipTestDetailsUrl + 'siteId=' + siteId + '&' + 'tankId=' + tankId + '&' + 'noOfDays=' + noOfDays)
            .pipe(catchError(this.handleError<any>('getDipTestDetails', null)));
    }

    public GetDeliveriesForLocations(fromDate, toDate): Observable<any> {
        return this.httpClient.get<any>(this.getDeliveriesForLocations + 'fromDate=' + fromDate + '&' + 'toDate=' + toDate)
            .pipe(catchError(this.handleError<any>('GetDeliveriesForLocations', null)));
    }

    public GetFilterData(isShowCarrierManaged?: boolean): Observable<any> {
        return this.httpClient.get<any>(this.getFilterData + "?isShowCarrierManaged=" + isShowCarrierManaged)
            .pipe(catchError(this.handleError<any>('GetFilterData', null)));
    }

    public getDispatcherCountry(): Observable<any> {
        return this.httpClient.get<any>(this.getDispatcherCountryUrl)
            .pipe(catchError(this.handleError<any>('getDispatcherCountry', null)));
    }

    public getOnGoingLoadsForMap(inputs: any): Observable<any> {
        return this.httpClient.post<any>(this.getOnGoingLoadsUrl, inputs)
            .pipe(catchError(this.handleError<any>('getOnGoingLoads', null)));
    }
}
