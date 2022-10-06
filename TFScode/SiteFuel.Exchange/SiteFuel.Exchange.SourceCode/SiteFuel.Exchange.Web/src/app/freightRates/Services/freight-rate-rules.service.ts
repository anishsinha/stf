import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { FreightRateViewModel } from 'src/app/freightRates/Models/createFreightRateRules';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';


const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
    providedIn: 'root'
})
export class FreightRateRulesService extends HandleError {

    public createFreightRateUrl = '/FreightRate/CreateFreightRate';
    public updateFreightRateUrl = '/FreightRate/UpdateFreightRate';
    public getFreightRateDetailUrl = '/FreightRate/GetFreightRateDetails?freightRateId=';
    public archiveFreightRateUrl = '/FreightRate/ArchiveFreightRate';
    public getFreightRateSummaryUrl = '/FreightRate/GetFreightRateSummary';
    public getFreightRateTableForViewUrl = '/FreightRate/GetFreightRateTableForView?freightRateId=';
    public getFreightRateRuleTypesUrl = "/FreightRate/GetFreightRateRuleTypes";
    public getCustomerJobsUrl = "/FreightRate/GetCustomerJobs?customerId=";
    public onSelectedTabChanged: BehaviorSubject<any>;
    public onSelectedFreightRateRuleId: BehaviorSubject<any>;


    constructor(private httpClient: HttpClient) {
        super();
        this.onSelectedTabChanged = new BehaviorSubject(1);
        this.onSelectedFreightRateRuleId = new BehaviorSubject(null);
    }

    createFreightRate(fsm: FreightRateViewModel): Observable<FreightRateViewModel> {
        return this.httpClient.post<FreightRateViewModel>(this.createFreightRateUrl, fsm, httpOptions)
            .pipe(catchError(this.handleError<FreightRateViewModel>('createFreightRate', fsm)));
    }

    updateFreightRate(fsm: FreightRateViewModel): Observable<FreightRateViewModel> {
        return this.httpClient.post<FreightRateViewModel>(this.updateFreightRateUrl, fsm, httpOptions)
            .pipe(catchError(this.handleError<FreightRateViewModel>('updateFreightRate', fsm)));
    }

    getFreightRateRuleTypes(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getFreightRateRuleTypesUrl)
            .pipe(catchError(this.handleError<any>('getFreightRateRuleTypes', null)));
    }

    getCustomerJobs(customerId: number): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getCustomerJobsUrl + customerId)
            .pipe(catchError(this.handleError<any>('getCustomerJobs', null)));
    }

    getFreightRateDetails(freightRateId: number): Observable<any> {
        return this.httpClient.get<any>(this.getFreightRateDetailUrl + freightRateId)
            .pipe(catchError(this.handleError<any>('getFuelSurchargeTable', null)));
    }

    getFreightRateGridDetails(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getFreightRateSummaryUrl, filter)
            .pipe(catchError(this.handleError<any>('getFreightRateGridDetails', null)));
    }

    archiveFreightRate(freightRateId: number): Observable<any> {
        return this.httpClient.post<any>(this.archiveFreightRateUrl, { freightRateId: freightRateId })
            .pipe(catchError(this.handleError<any>('archiveFreightRate', null)));
    }

    getFreightRateTableForView(freightRateId: number): Observable<any> {
        return this.httpClient.get<any>(this.getFreightRateTableForViewUrl + freightRateId)
            .pipe(catchError(this.handleError<any>('getFreightRateTableForView', null)));
    }
}