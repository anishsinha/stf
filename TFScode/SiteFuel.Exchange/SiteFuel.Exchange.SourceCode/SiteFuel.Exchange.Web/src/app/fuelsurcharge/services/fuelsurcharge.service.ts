import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { FuelSurchargeIndexViewModel, FuelSurchargeGridModel } from 'src/app/fuelsurcharge/models/CreateFuelSurcharge';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { SourceRegionInputModel } from 'src/app/app.enum';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
    providedIn: 'root'
})
export class FuelSurchargeService extends HandleError {
    public onSelectedTabChanged: BehaviorSubject<any>;
    public onSelectedFuelSurchargeId: BehaviorSubject<any>;

    public getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
    public getSupplierCustomersUrl = "/FuelSurcharge/GetSupplierCustomers";   
    public getSourceRegionsUrl = "/FuelSurcharge/GetSourceRegionsAsync";  
    public getTerminalsAndBulkPlantsUrl = "/FuelSurcharge/GetTerminalsAndBulkPlantsAsync?regionIds=";
    public getFuelSurchargeProductsUrl = '/FuelSurcharge/GetFuelSurchargeProductAsync?countryId=';
    public getFuelSurchargePeriodUrl = '/FuelSurcharge/GetFuelSurchargePeriodAsync?countryId=';
    public getFuelSurchargeAreaUrl = '/FuelSurcharge/GetFuelSurchargeAreaAsync?countryId=';
    public getEIAIndexPriceUrl = '/FuelSurcharge/GetEIAIndexPrice?periodId=';
    public getNRCIndexPriceUrl = '/FuelSurcharge/GetNRCIndexPrice?periodId=';
    public getGenerateSurchargeTableUrl = '/FuelSurcharge/GetGenerateSurchargeTable?pRSV=';
    public getViewFuelSurchargeSummaryUrl = '/FuelSurcharge/GetViewFuelSurchargeSummary';
    public getSurchargeTableNewUrl = '/FuelSurcharge/GetSurchargeTableNew?fuelSurchargeIndexId=';
    public createFuelSurchargeUrl = '/FuelSurcharge/CreateFuelSurchargeAsync';
    public archiveFuelSurchargeTableUrl = '/FuelSurcharge/ArchiveFuelSurchargeTable';
    public getHistoricalPriceUrl = '/FuelSurcharge/GetHistoricalPrice?fuelSurchargeIndexId=';
    
    public getFuelSurchargeTableUrl = '/FuelSurcharge/GetFuelSurchargeTableAsync?fuelSurchargeTableId=';
    constructor(private httpClient: HttpClient) {
        super();
        this.onSelectedTabChanged = new BehaviorSubject(1);
        this.onSelectedFuelSurchargeId = new BehaviorSubject(null);
    }


    getTableTypes(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getTableTypesUrl)
            .pipe(catchError(this.handleError<any>('GetTableTypes', null)));
    }

    getSupplierCustomers(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getSupplierCustomersUrl)
            .pipe(catchError(this.handleError<any>('GetSupplierCustomers', null)));
    }

    getSourceRegions(input: SourceRegionInputModel): Observable<any> {
        return this.httpClient.post<any>(this.getSourceRegionsUrl, input)
            .pipe(catchError(this.handleError<any>('getSourceRegions', null)));
    }

    getTerminalsAndBulkPlants(regionIds: string): Observable<DropdownItemExt[]> {       
        return this.httpClient.get<any>(this.getTerminalsAndBulkPlantsUrl + regionIds)
            .pipe(catchError(this.handleError<any>('GetTerminalsAndBulkPlants', null)));
    }

    getFuelSurchargeProducts(countryId: number): Observable<DropdownItemExt[]> {
        return this.httpClient.get<any>(this.getFuelSurchargeProductsUrl + countryId)
            .pipe(catchError(this.handleError<any>('GetFuelSurchargeProduct', null)));
    }

    getFuelSurchargePeriod(countryId: number): Observable<DropdownItemExt[]> {
        return this.httpClient.get<any>(this.getFuelSurchargePeriodUrl + countryId)
            .pipe(catchError(this.handleError<any>('getFuelSurchargePeriod', null)));
    }

    getFuelSurchargeArea(countryId: number): Observable<DropdownItemExt[]> {
        return this.httpClient.get<any>(this.getFuelSurchargeAreaUrl + countryId)
            .pipe(catchError(this.handleError<any>('getFuelSurchargeArea', null)));
    }

    getEIAIndexPrice(periodId: number, productType: number, fetchDate: string, areaId: number): Observable<any> {
        return this.httpClient.get<any>(this.getEIAIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate + "&areaId=" + areaId)
            .pipe(catchError(this.handleError<any>('getEIAIndexPrice', null)));
    }

    getNRCIndexPrice(periodId: number, productType: number, fetchDate: string): Observable<any> {
        return this.httpClient.get<any>(this.getNRCIndexPriceUrl + periodId + "&productType=" + productType + "&fetchDate=" + fetchDate)
            .pipe(catchError(this.handleError<any>('getNRCIndexPrice', null)));
    }

    createFuelSurcharge(fsm: FuelSurchargeIndexViewModel): Observable<FuelSurchargeIndexViewModel> {
        return this.httpClient.post<FuelSurchargeIndexViewModel>(this.createFuelSurchargeUrl, fsm, httpOptions)
            .pipe(catchError(this.handleError<FuelSurchargeIndexViewModel>('createFuelSurcharge', fsm)));
    }

    getGenerateSurchargeTable(pRSV: string, pREV: string, pRI: string, sI: string, fSSP: string): Observable<any> {
        return this.httpClient.get<any>(this.getGenerateSurchargeTableUrl + pRSV + "&pREV=" + pREV + "&pRI=" + pRI + "&sI=" + sI + "&fSSP=" + fSSP)
            .pipe(catchError(this.handleError<any>('getGenerateSurchargeTable', null)));
    }

    getFuelSurchargeGridDetails(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getViewFuelSurchargeSummaryUrl, filter)
            .pipe(catchError(this.handleError<any>('getFuelSurchargeGridDetails', null)));
    }

    getSurchargeTableNew(fuelSurchargeIndexId: number): Observable<any> {
        return this.httpClient.get<any>(this.getSurchargeTableNewUrl + fuelSurchargeIndexId)
            .pipe(catchError(this.handleError<any>('getSurchargeTableNew', null)));
    }

    getHistoricalPrice(fuelSurchargeIndexId: number, forPeriod: number): Observable<any> {
        return this.httpClient.get<any>(this.getHistoricalPriceUrl + fuelSurchargeIndexId + "&forPeriod=" + forPeriod)
            .pipe(catchError(this.handleError<any>('getHistoricalPrice', null)));
    }

    archiveFuelSurchargeTable(fuelSurchargeIndexId: number): Observable<any> {
        return this.httpClient.post<any>(this.archiveFuelSurchargeTableUrl, { fuelSurchargeIndexId: fuelSurchargeIndexId } )
            .pipe(catchError(this.handleError<any>('archiveFuelSurchargeTable', null)));
    }

    getFuelSurchargeTable(fuelSurchargeTableId: number): Observable<any> {
        return this.httpClient.get<any>(this.getFuelSurchargeTableUrl + fuelSurchargeTableId)
            .pipe(catchError(this.handleError<any>('getFuelSurchargeTable', null)));
    }

    
}