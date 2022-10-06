import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HandleError } from './errors/HandleError';


@Injectable({
    providedIn: 'root'
})
export class StatelistService extends HandleError {

    private stateUrl = '/Settings/Profile/GetStatesEx';
    private countryUrl = '/Settings/Profile/GetCountriesEx';
    private countryGroupUrl = '/Settings/Profile/GetCountriesGroupEx?countryId=';

    constructor(private httpClient: HttpClient) {
        super();
    }

    regions: DropdownItem[];

    getStates(): Observable<StateDropdownExtendedItem[]> {
        return this.httpClient.get<StateDropdownExtendedItem[]>(this.stateUrl)
            .pipe(catchError(this.handleError<StateDropdownExtendedItem[]>('getStates', [])));
    }

    getCountries(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.countryUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getCountries', [])));
    }

    getCountryGroup(countryId:any): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.countryGroupUrl + countryId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getCountries', [])));
    }
}

export class DropdownItem {
    Id: number;
    Code: string;
    Name: string;
}
export class ShiftLoadQueueDropdownItem {
    Id: string;
    Code: string;
    Name: string;
    OrderNo: number;
}
export class StateDropdownExtendedItem extends DropdownItem {
    CountryGroupId?: number;
    CountryId: number;
}

export class DropdownCustomItem {
    Id: number;
    isDisabled: boolean;
    Name: string;
}

export class DropdownItemExt {
    Id: string;
    Code: string;
    Name: string;
}

export class FeeSubType {
    public FeeTypeId: string;
    public FeeSubTypeId: number;
    public SubTypeName: string;
}
export class TBDDropdownItem {
    Id: number;
    Name: string;
    ProductTypeName: string;
    ProductTypeId: number;
    UoM: number;
}
export class LocationDropdownItem {
    Id: number;
    Code: string;
    Name: string;
    DisplayName: string;
}

export class AdditiveOrderViewModel {
    Id: number;
    Name: string;
    BuyerCompanyId: number;
    JobId: number;
    UoM: string;
}