import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
    providedIn: 'root'
})
export class AccessorialFeesService extends HandleError {
    public onSelectedTabChanged: BehaviorSubject<any>;
    public onSelectedAccessorialFeeId: BehaviorSubject<any>;

    public archiveAccessorialFeeUrl = '/AccessorialFees/ArchiveAccessorialFee';
    public getViewAccessorialFeeSummaryUrl = '/AccessorialFees/GetViewAccessorialFeeSummary';
    public getAccessorialFeeUrl = '/AccessorialFees/GetAccessorialFee?accessorialFeeId=';
    public postCreateAccesorialFeesUrl = '/AccessorialFees/CreateAccessorialFee'
    public postUpdateAccesorialFeesUrl = '/AccessorialFees/UpdateAccessorialFee'

    constructor(private httpClient: HttpClient) {
        super();
        this.onSelectedTabChanged = new BehaviorSubject(1);
        this.onSelectedAccessorialFeeId = new BehaviorSubject(null);
    }

    getAccessorialFeeGridDetails(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getViewAccessorialFeeSummaryUrl, filter)
            .pipe(catchError(this.handleError<any>('getAccessorialFeeGridDetails', null)));
    }

    archiveAccessorialFee(accessorialFeeId: number): Observable<any> {
        return this.httpClient.post<any>(this.archiveAccessorialFeeUrl, { accessorialFeeId: accessorialFeeId })
            .pipe(catchError(this.handleError<any>('archiveAccessorialFee', null)));
    }

    createAccessorialFee(accessorialFeeModel: any): Observable<any> {
        return this.httpClient.post<any>(this.postCreateAccesorialFeesUrl, { model: accessorialFeeModel })
            .pipe(catchError(this.handleError<any>('createAccessorialFee', null)));
    }

    updateAccessorialFee(accessorialFeeModel: any): Observable<any> {
        return this.httpClient.post<any>(this.postUpdateAccesorialFeesUrl, { model: accessorialFeeModel })
            .pipe(catchError(this.handleError<any>('updateAccessorialFee', null)));
    }

    getAccessorialFee(accessorialFeeId: number): Observable<any> {
        return this.httpClient.get<any>(this.getAccessorialFeeUrl + accessorialFeeId)
            .pipe(catchError(this.handleError<any>('getAccessorialFee', null)));
    }
}