import { Injectable } from '@angular/core'
import { HandleError } from '../../errors/HandleError';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ViewOrderGroupDdlModel } from '../models/ViewOrderGroupDdlModel';

@Injectable({
    providedIn: 'root'
})
export class ViewOrderGroupService extends HandleError {
    private getOrderGroupAllDdlUrl: string = '/OrderBase/FillOrderGroupDdl?groupTypeId=';
    private viewOrderGroupDetailsUrl: string = '/OrderBase/ViewOrderGroupDetails';    
    private getCommonJobListUrl = '/OrderBase/GetJobsForCustomers?companyId=';
    private getDeleteOrderGroupUrl = '/OrderBase/DeleteOrderGroup?groupId=';

    constructor(private httpClient: HttpClient) {
        super();
	}

    fillDDLByGroup(groupTypeId): Observable<any> {
        return this.httpClient.get(this.getOrderGroupAllDdlUrl + groupTypeId)
            .pipe(catchError(this.handleError<any>('fillViewOrderGroupAllDDL', null)));
    }

    getOrderGroupDetails(viewOrderGroupFilterModel: ViewOrderGroupDdlModel): Observable<any> {
        return this.httpClient.post<any>(this.viewOrderGroupDetailsUrl, viewOrderGroupFilterModel)
            .pipe(catchError(this.handleError<any>('getOrderGroupDetails', null)));
    }

    getCommonJobList(customerId: number): Observable<any> {
        return this.httpClient.get(this.getCommonJobListUrl + customerId).pipe(catchError(this.handleError<any>('getCommonJobList', null)));
    }

    deleteOrderGroup(groupId: number): Observable<any> {
        return this.httpClient.post<any>(this.getDeleteOrderGroupUrl + groupId, groupId).pipe(catchError(this.handleError<any>('deleteOrderGroup', null)));
    }
}
