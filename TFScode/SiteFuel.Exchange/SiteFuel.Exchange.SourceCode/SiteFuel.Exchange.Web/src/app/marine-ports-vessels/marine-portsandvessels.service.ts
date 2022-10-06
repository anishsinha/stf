import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HandleError } from '../errors/HandleError';
import { DropdownCustomItem, DropdownItem } from '../statelist.service';
import { Observable, Subject } from 'rxjs';
import { Geocode } from './models';


@Injectable({
  providedIn: 'root'
})
export class MarinePortsandvesselsService extends HandleError {

  private urlGetAllCountries = '/Settings/Profile/GetCountriesEx';
    private urlGetMarinePorts = '/SuperAdmin/SuperAdmin/GetMarinePortsForSuperAdmin';
    private urlGetCountryList = '/SuperAdmin/SuperAdmin/GetCountryList';
    private urlGetCountryGroupList = '/SuperAdmin/SuperAdmin/GetCountryGroupList';
    private urlGetStatesOfAllCountrieslist = "/SuperAdmin/SuperAdmin/GetStatesOfAllCountries";
    private urlGetAddressByZip = "Validation/GetAddressByZip";
    private urlGetaddressbyLatLong = "/SuperAdmin/SuperAdmin/GetAddressByLongLat";
    private urlSaveMarinePort = '/SuperAdmin/SuperAdmin/SaveMarinePort'
    private urlDeletePort = 'SuperAdmin/SuperAdmin/DeletePort';

    private urlGetMarineVessels = '/SuperAdmin/SuperAdmin/GetMarineVesselsForSuperAdmin';
    private urlDeleteVessel = 'SuperAdmin/SuperAdmin/DeleteVessel';
    private urlSaveMarineVessel = '/SuperAdmin/SuperAdmin/SaveMarineVessel'

    public reloadPortMapsSubject: Subject<any>;

  constructor(private httpClient: HttpClient) {
      super();
      this.reloadPortMapsSubject = new Subject<any>();
   }

  public GetAllCountries(){
    return this.httpClient.get<DropdownItem[]>(this.urlGetAllCountries)
    .pipe(catchError(this.handleError<DropdownItem[]>('GetAllCountries', [])));
    
  }

  public getMarinePorts(countryId:number){
    return this.httpClient.get<any[]>(this.urlGetMarinePorts+'?countryId='+countryId)
    .pipe(catchError(this.handleError<any[]>('getMarinePorts', [])));
    }
    public getCountryList() {
        return this.httpClient.get<any[]>(this.urlGetCountryList)
            .pipe(catchError(this.handleError<any[]>('getCountryList', [])));
    }
    public getCountryGroupList(countryId: number) {
        return this.httpClient.get<any[]>(this.urlGetCountryGroupList + '?countryId=' + countryId)
            .pipe(catchError(this.handleError<any[]>('getCountryGroupList', [])));
    }
    public GetStatesOfAllCountries(countryId?: number): Observable<any> {
        return this.httpClient.get<any>(`${this.urlGetStatesOfAllCountrieslist}?countryId=${countryId}`)
            .pipe(catchError(this.handleError<any>('GetStatesOfAllCountries', null)));
    }

    public GetAddressByZip(zipCode: string) {
        return this.httpClient.get<any>(`${this.urlGetAddressByZip}?zipCode=${zipCode}`)
            .pipe(catchError(this.handleError<any>('GetAddressByZip', null)));
    }
    GetAddressByLongLat(latitude: string, longitude: string): Observable<Geocode> {
        return this.httpClient.get<any>(`${this.urlGetaddressbyLatLong}?latitude=${latitude}&longitude=${longitude}`)
            .pipe(catchError(this.handleError<any>('GetAddressByLongLat', null)));
    }
    saveMarinePort(port: any) {
        return this.httpClient.post<any>(this.urlSaveMarinePort, port)
            .pipe(catchError(this.handleError<any>('saveMarinePort', null)));
    }
    deletePort(id: number) {
        return this.httpClient.get<any>(`${this.urlDeletePort}?id=${id}`)
            .pipe(catchError(this.handleError<any>('deletePort', null)));
    }
    public setReloadMapSubject(countryId: any): void {
        this.reloadPortMapsSubject.next(countryId);
    }
    public getMarineVessels(countryId: number) {
        return this.httpClient.get<any[]>(this.urlGetMarineVessels + '?countryId=' + countryId)
            .pipe(catchError(this.handleError<any[]>('getMarineVessels', [])));
    }

    public deleteVessel(id: number) {
        return this.httpClient.get<any>(`${this.urlDeleteVessel}?id=${id}`)
            .pipe(catchError(this.handleError<any>('deleteVessel', null)));
    }
    saveMarineVessel(vessel: any) {
        return this.httpClient.post<any>(this.urlSaveMarineVessel, vessel)
            .pipe(catchError(this.handleError<any>('saveMarineVessel', null)));
    }

}
