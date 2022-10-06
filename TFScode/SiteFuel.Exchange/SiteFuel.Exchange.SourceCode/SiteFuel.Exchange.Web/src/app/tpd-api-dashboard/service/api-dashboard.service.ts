import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiDashboardService extends HandleError {
    private getAllLogUrl: string = '/Home/GetApiLogs?companyId=';
    private getReqResUrl: string = '/Home/GetApiLogRequestResponse?id=';
    constructor(private httpClient: HttpClient) {
        super();
    }

    getApiLogs(currentCompanyId:number,fromDate,toDate,selectedApi,viewType): Observable<any> {
        return this.httpClient.get(this.getAllLogUrl + currentCompanyId + "&fromdate=" + fromDate + "&toDate=" + toDate + "&viewType=" + viewType+"&apiName=" + selectedApi)
            .pipe(catchError(this.handleError<any>('getApiLogs', null)));
    }
    getApiReqRes(id, reqResType): Observable<any> {
        return this.httpClient.get(this.getReqResUrl + id + "&ReqResType=" + reqResType)
            .pipe(catchError(this.handleError<any>('GetApiLogRequestResponse', null)));
    }
    
}
