import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { TractorDetailsModel } from '../model';


@Injectable({
    providedIn: 'root'
})
export class TractorService extends HandleError {
    private postTractorData = '/Carrier/Tractor/Create';
    private urlDeleteTractorData = '/Carrier/Tractor/DeleteTractor';
    private urlGetTractors = '/Carrier/Tractor/GetAllTractorDetails';
    private urlGetCompanyDrivers = 'Carrier/Tractor/GetCompanyDrivers?trailerId=';
    private urlGetDefaultUOM = 'Carrier/Tractor/GetDefaultUOM';

    constructor(private httpClient: HttpClient) { super(); }

    getAllTractors(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetTractors)
            .pipe(catchError(this.handleError<any>('getAllTractors', null)));
    }

    getCompanyDrivers(trailerId): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCompanyDrivers + trailerId)
            .pipe(catchError(this.handleError<any>('getCompanyDrivers', null)));
    }

    postCreateTractors(inputData: any): Observable<any> {
        return this.httpClient.post<any>(this.postTractorData, inputData)
            .pipe(catchError(this.handleError<any>('postCreateTractors', null)));
    }
    postDeleteTractor(inputData: TractorDetailsModel): Observable<any> {
        return this.httpClient.post<any>(this.urlDeleteTractorData, inputData)
            .pipe(catchError(this.handleError<any>('postDeleteTractor', null)));
    }
    getDefaultUOM(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetDefaultUOM)
            .pipe(catchError(this.handleError<any>('getDefaultUOM', null)));
    }

}
