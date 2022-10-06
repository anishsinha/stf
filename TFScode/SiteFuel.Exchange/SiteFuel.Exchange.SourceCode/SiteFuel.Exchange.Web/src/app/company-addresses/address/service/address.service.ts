import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

import { Address } from '../model/address';
import { HandleError } from 'src/app/errors/HandleError';

@Injectable({
    providedIn: 'root'
})
export class AddressService extends HandleError {

    private serviceUrl = '/Profile/CompanyAddressGrid?companyId=';
    private changeStatusUrl = '/Settings/Profile/ChangeCompanyAddressStatus';

    constructor(private httpClient: HttpClient) {super(); }

    addresses: Address[];

    getAddresses(companyId: number): Observable<Address[]> {
        return this.httpClient.get<Address[]>(this.serviceUrl + companyId)
            .pipe(catchError(this.handleError<Address[]>('getAddresses', [])));
    }

    changeAddressStatus(companyAddressId: any, IsActive: boolean): Observable<any> {     
         return this.httpClient.get(this.changeStatusUrl + '?id=' + companyAddressId + '&isActive=' + IsActive);           
    }

}
