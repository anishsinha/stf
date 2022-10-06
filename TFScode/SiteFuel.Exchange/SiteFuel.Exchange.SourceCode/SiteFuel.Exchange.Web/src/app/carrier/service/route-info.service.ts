import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HandleError } from 'src/app/errors/HandleError';
import { RouteInformationModel } from '../models/location';

@Injectable({
    providedIn: 'root'
})

export class RouteInfoService extends HandleError {
    routeInfoDetails: BehaviorSubject<any>;
    routeInfo: any;
    private urlGetRouteInfoDetails = '/Carrier/ScheduleBuilder/GetRouteInfoDetails';
    private urlGetRegionLocationDetails = '/Carrier/ScheduleBuilder/GetRegionLocationDetails';
    private urlGetRouteLocationDetails = '/Carrier/ScheduleBuilder/GetRouteLocationDetails';
    private urlDeleteRouteInfo = '/Carrier/ScheduleBuilder/DeleteRouteInfo';
    private urlCreateRouteInfo = '/Carrier/ScheduleBuilder/CreateRouteInfo';
    private urlUpdateRouteInfo = '/Carrier/ScheduleBuilder/UpdateRouteInfo';


    constructor(private httpClient: HttpClient) {
        super();
        this.routeInfoDetails = new BehaviorSubject(this.routeInfo);
    }

    getRoutesByRegion(regionId: string): Observable<any[]> {
        return this.httpClient.get<any[]>(this.urlGetRouteInfoDetails + '?regionId=' + regionId)
            .pipe(catchError(this.handleError<any>('getRoutesByRegion', null)));
    }
    getLocationsByRegion(regionId: string): Observable<any[]> {
        return this.httpClient.get<any[]>(this.urlGetRegionLocationDetails + '?regionId=' + regionId)
            .pipe(catchError(this.handleError<any>('getLocationsByRegion', null)));
    }
    getLocationsByRoute(Id: string, regionId: string): Observable<any[]> {
        return this.httpClient.get<any[]>(this.urlGetRouteLocationDetails + '?Id=' + Id + '&regionId=' + regionId)
            .pipe(catchError(this.handleError<any>('getLocationsByRegion', null)));
    }
    deleteRouteInfo(data: any): Observable<any[]> {
        return this.httpClient.post<any>(this.urlDeleteRouteInfo, data)
            .pipe(catchError(this.handleError<any>('deleteRouteInfo', null)));
    }
    createRouteInfo(data: RouteInformationModel): Observable<any[]> {
        return this.httpClient.post<any>(this.urlCreateRouteInfo, data)
            .pipe(catchError(this.handleError<any>('createRouteInfo', null)));
    }
    updateRouteInfo(data: RouteInformationModel): Observable<any[]> {
        return this.httpClient.post<any>(this.urlUpdateRouteInfo, data)
            .pipe(catchError(this.handleError<any>('createRouteInfo', null)));
    }
    sendRouteInfo(memberData: any) {
        this.routeInfoDetails.next(memberData);
    }
}
