import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Region, RegionModel,SourceRegion,SourceRegionModel, CarrierRegionModel, TfxCarrierDropdownDisplayItem } from '../model/region';
import { DropdownItem } from 'src/app/statelist.service';
import { BehaviorSubject } from 'rxjs';
import { RouteInformationModel } from 'src/app/carrier/models/location';
import { RegionScheduleViewModel } from 'src/app/driver/models/regionSchedule';
import { HandleError } from 'src/app/errors/HandleError';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
    providedIn: 'root'
})
export class RegionService extends HandleError {

    private createUrl = '/Region/Create';
    private updateUrl = '/Region/Update';
    private deleteUrl = '/Region/Delete?id=';
    private getRegionsUrl = '/Region/GetRegions';
    private getSourceRegionsUrl = '/Region/GetSourceRegions';
    private createSourceRegionUrl = '/Region/CreateSourceRegion';
    private updateSourceRegionUrl = '/Region/UpdateSourceRegion';
    private deleteSourceRegionUrl = '/Region/DeleteSourceRegion?id=';
    private getJobsUrl = '/Region/GetJobs';
    private getDriversUrl = '/Region/GetDrivers';
    private getDispatchersUrl = '/Region/GetDispatchers';
    private getTrailersUrl = '/Region/GetTrailers';
    private stateUrl = '/Settings/Profile/GetStatesEx?countryId=';
    private shiftByDriverUrl = '/Freight/GetShiftByDrivers?driverList=';
    private getRegionSchedulsbyRegionIdUrl = '/Freight/getRegionShiftSchedule?regionId=';
    private getRouteByReginId = '/ScheduleBuilder/GetRouteInfoDetails?regionId=';
    private getCompanyShiftsUrl = '/Region/GetCompanyShifts';
    private getRegionDriversUrl = '/Region/GetRegionDrivers?regionId=';
    private addDriverScheduleUrl = '/Region/AddDriverSchedule';
    private addRegionScheduleUrl = '/Region/AddRegionSchedule';
    private updateDriverScheduleUrl = '/Region/updateDriverSchedule';
    private deleteDriverScheduleUrl = '/Region/DeleteDriverSchedules';
    public getCarriersUrl = '/Region/GetCarriers';
    private getRegionShiftMapping = '/Region/GetResionShiftSchedulesDetails?regionId=';
    private getCarrierRegionsUrl = '/Carrier/Freight/GetCarrierRegions';
    private getSelectedCarriersByRegionUrl = '/Carrier/ScheduleBuilder/GetSelectedCarriersByRegion?regionId=';
    private isSourceRegionAvailableUrl = '/Validation/IsSourceRegionExist?name=';
    private getProductTypeUrl = '/Supplier/FuelGroup/GetProductTypes';
    private getFuelProductUrl = '/Region/GetMstFuelProducts';
    private isPublishedDRUrl = '/Region/IsPublishedDR?productIds=';

    onLoadingChanged: BehaviorSubject<any>;
    constructor(private httpClient: HttpClient) {
        super();
        this.onLoadingChanged = new BehaviorSubject(false);
    }

    getJobs(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getJobsUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getJobs', [])));
    }

    getDrivers(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getDriversUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getDrivers', [])));
    }
    getRegionDrivers(regiondId): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getRegionDriversUrl + regiondId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getDrivers', [])));
    }
    getCompanyShifts(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getCompanyShiftsUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getCompanyShifts', [])));
    }
    getRoutesByRegion(regionId: number): Observable<RouteInformationModel[]> {
        return this.httpClient.get<RouteInformationModel[]>(this.getRouteByReginId + regionId)
            .pipe(catchError(this.handleError<RouteInformationModel[]>('GetRouteInfoDetails', [])));
    }

    getDispatchers(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getDispatchersUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getDispatchers', [])));
    }

    getTrailers(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getTrailersUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getTrailers', [])));
    }

    getRegions(): Observable<RegionModel> {
        return this.httpClient.get<RegionModel>(this.getRegionsUrl)
            .pipe(catchError(this.handleError<RegionModel>('getRegions', null)));
    }

    createRegion(region: Region): Observable<Region> {
        return this.httpClient.post<Region>(this.createUrl, region, httpOptions)
            .pipe(catchError(this.handleError<Region>('createRegion', region)));
    }

    updateRegion(region: Region): any {
        return this.httpClient.post<Region>(this.updateUrl, region, httpOptions)
            .pipe(catchError(this.handleError<Region>('updateRegion', region)));
    }

    getSourceRegions(): Observable<SourceRegionModel> {
        return this.httpClient.get<SourceRegionModel>(this.getSourceRegionsUrl)
            .pipe(catchError(this.handleError<SourceRegionModel>('getSourceRegions', null)));
    }

    createSourceRegion(region: SourceRegion): Observable<SourceRegion> {
        return this.httpClient.post<SourceRegion>(this.createSourceRegionUrl, region, httpOptions)
            .pipe(catchError(this.handleError<SourceRegion>('createSourceRegion', region)));
    }

    isSourceRegionAvailable(name: string, id: string): Observable<any> {
        return this.httpClient.get<any>(this.isSourceRegionAvailableUrl + name + "&id=" + id)
            .pipe(catchError(this.handleError<SourceRegionModel>('isSourceRegionAvailable', null)));
    }

    updateSourceRegion(region: SourceRegion): any {
        return this.httpClient.post<SourceRegion>(this.updateSourceRegionUrl, region, httpOptions)
            .pipe(catchError(this.handleError<SourceRegion>('updateSourceRegion', region)));
    }

    deleteRegion(id: string): any {
        return this.httpClient.post<any>(this.deleteUrl + id, id)
            .pipe(catchError(this.handleError<any>('deleteRegion', id)));
    }

    deleteSourceRegion(id: string): any {
        return this.httpClient.post<any>(this.deleteSourceRegionUrl + id, id)
            .pipe(catchError(this.handleError<any>('deleteSourceRegion', id)));
    }

    getStates(countryId: number): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.stateUrl + countryId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getStates', [])));
    }

    //for calender
    getShiftByDrivers(driverIds: any, scheduleType: any): Observable<any> {
        return this.httpClient.get<any>(this.shiftByDriverUrl + driverIds + "&scheduleType=" + scheduleType)
            .pipe(catchError(this.handleError<any>('getShiftByDrivers', [])));
    }

    getSchedulesByRegion(regionId: any, scheduleType: any): Observable<any> {
        return this.httpClient.get<any>(this.getRegionSchedulsbyRegionIdUrl + regionId + "&scheduleType=" + scheduleType)
            .pipe(catchError(this.handleError<any>('getSchedulesByRegion', [])));
    }

    getRegionSchedule(regionId: string, routeId: string): Observable<RegionScheduleViewModel[]> {
        return this.httpClient.get<RegionScheduleViewModel[]>(this.getRegionShiftMapping + regionId + "&routeId=" + routeId)
            .pipe(catchError(this.handleError<any>('getRegionSchedule', [])));
    }

    addDriverSchedule(model: any): Observable<any> {
        return this.httpClient.post<any>(this.addDriverScheduleUrl, model, httpOptions)
            .pipe(catchError(this.handleError<any>('addDriverSchedule', model)));
    }

    addRegionSchedule(model: any): Observable<any> {
        return this.httpClient.post<any>(this.addRegionScheduleUrl, model, httpOptions)
            .pipe(catchError(this.handleError<any>('addRegionSchedule', model)));
    }

    updateDriverSchedule(data: any, date: any): Observable<any> {
        var postModel = { model: data, SelectedDate: date};
        return this.httpClient.post<any>(this.updateDriverScheduleUrl, postModel, httpOptions)
            .pipe(catchError(this.handleError<any>('addDriverSchedule', postModel)));
    }

    deleteDriverSchedule(data: any, date: any): Observable<any> {
        var postModel = { driverScheduleMappingViewModels: data, SelectedDate: date};
        return this.httpClient.post<any>(this.deleteDriverScheduleUrl, postModel, httpOptions)
            .pipe(catchError(this.handleError<any>('deleteDriverSchedule', postModel)));
    }

    getCarriers(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getCarriersUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getCarriers', [])));
    }
    getCarrierRegions(): Observable<CarrierRegionModel[]> {
        return this.httpClient.get<any>(this.getCarrierRegionsUrl)
            .pipe(catchError(this.handleError<any>('getCarrierRegions', null)));
    }

    getSelectedCarriersByRegion(regionId: string): Observable<TfxCarrierDropdownDisplayItem[]> {
        return this.httpClient.get<any>(this.getSelectedCarriersByRegionUrl + regionId)
            .pipe(catchError(this.handleError<any>('getSelectedCarriersByRegion', null)));
    }

    getProductType(): Observable<any> {
        return this.httpClient.get<any>(this.getProductTypeUrl)
            .pipe(catchError(this.handleError<any>('getProductType', [])));
    }
    getFuelProducts(): Observable<any> {
        return this.httpClient.get<any>(this.getFuelProductUrl)
            .pipe(catchError(this.handleError<any>('getFuelProducts',[])));
    }

    isPublishedDR(productIds: any, fuelTypeIds: string): Observable<any> {
        return this.httpClient.get<any>(this.isPublishedDRUrl + productIds + "&fuelTypeIds=" + fuelTypeIds)
            .pipe(catchError(this.handleError<any>('isPublishedDR', null)));
    }
}
