import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, ObservableLike } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HandleError } from '../../errors/HandleError';
import { DropdownItem, DropdownItemExt, FeeSubType } from '../../statelist.service';


@Injectable({
  providedIn: 'root'
})
export class FeeService extends HandleError {

    private getFeeTypesUrl = '/Supplier/Invoice/GetFeeTypes?orderId=';
    private getFeeSubTypesUrl = '/Supplier/Invoice/GetFeeSubTypes?orderId=';
    private getFeeConstraintTypesUrl = '/Supplier/Invoice/GetFeeConstraintTypes';
    private getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';

    constructor(private httpClient: HttpClient) {
        super();
    }

    getFeeTypes(orderId: number, isFromAccesorialFees?: boolean): Observable<DropdownItemExt[]> {
        return this.httpClient.get<DropdownItemExt[]>(this.getFeeTypesUrl + orderId + '&isFromAccesorialFees=' + isFromAccesorialFees)
            .pipe(catchError(this.handleError<DropdownItemExt[]>('getFeeTypes', [])));
    }

    getFeeSubTypes(orderId: number): Observable<FeeSubType[]> {
        return this.httpClient.get<FeeSubType[]>(this.getFeeSubTypesUrl + orderId)
            .pipe(catchError(this.handleError<FeeSubType[]>('getFeeSubTypes', [])));
    }

    getFeeConstraintTypes(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getFeeConstraintTypesUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getFeeConstraintTypes', [])));
    }

    getEiaPrice(data: any): Observable<any> {
        return this.httpClient.post(this.getEiaPriceUrl, data)
            .pipe(catchError(this.handleError<DropdownItem[]>('getEiaPrice', [])));
    }
}
