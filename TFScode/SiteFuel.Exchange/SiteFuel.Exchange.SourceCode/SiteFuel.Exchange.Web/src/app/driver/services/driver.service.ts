import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DriverViewModel } from '../models/DriverManagementModel';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class DriverService extends HandleError {
    private getShiftUrl: string = '/Settings/Profile/GetShifts';
    private getAllDriversUrl: string = '/Settings/Profile/GetAllDrivers';
    private postAddDriverUrl: string = '/Settings/Profile/AddDriver';
    private postDeleteDriverUrl: string = '/Settings/Profile/DeleteInvitedUser';
    private changeDriverStatusUrl: string = '/Settings/Profile/ChangeUserStatus?id=';
    private getRegionsUrl: string = '/Supplier/Region/GetRegionsDdl';
    private addTrailerScheduleUrl: string = '/Supplier/Dispatch/AddTrailerSchedule';
    onLoadingChanged: BehaviorSubject<any>;

    constructor(private httpClient: HttpClient) {
        super();
        this.onLoadingChanged = new BehaviorSubject(false);
    }

    getShifts(): Observable<any> {
        return this.httpClient.get(this.getShiftUrl)
            .pipe(catchError(this.handleError<any>('getShifts', null)));
    }

    getAllDrivers(): Observable<any> {
        return this.httpClient.get(this.getAllDriversUrl)
            .pipe(catchError(this.handleError<any>('getAllDrivers', null)));
    }

    postAddDriver(driverModel: any): Observable<any> {
        return this.httpClient.post<any>(this.postAddDriverUrl, driverModel)
            .pipe(catchError(this.handleError<any>('postAddDriver', null)));
    }

    postDeleteDriver(driverdelteInfo: DriverViewModel): Observable<any> {
        var data = { Id: driverdelteInfo.UserId, IsScheduleExists: driverdelteInfo.IsScheduleExists, ScheduleBuilderIdInfo: driverdelteInfo.ScheduleBuilderIdInfo };
        return this.httpClient.post<any>(this.postDeleteDriverUrl, data)
            .pipe(catchError(this.handleError<any>('postDeleteDriver', null)));
    }

    changeDriverStatus(id: number, isActive: boolean): Observable<any> {
        return this.httpClient.get(this.changeDriverStatusUrl + id + "&isActive=" + isActive)
            .pipe(catchError(this.handleError<any>('changeDriverStatus', null)));
    }

    getRegions(): Observable<any> {
        return this.httpClient.get(this.getRegionsUrl)
            .pipe(catchError(this.handleError<any>('getRegions', null)));
    }

    addTrailerSchedule(model: any): Observable<any> {
        return this.httpClient.post<any>(this.addTrailerScheduleUrl, model, httpOptions)
            .pipe(catchError(this.handleError<any>('addTrailerSchedule', model)));
    }
}
