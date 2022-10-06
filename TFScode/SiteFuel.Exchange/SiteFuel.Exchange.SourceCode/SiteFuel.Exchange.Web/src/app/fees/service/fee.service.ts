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
    private getSalesFeeTypesUrl = '/SalesUser/SourcingRequest/GetFeeTypes?currency=';
    private getSalesFeeSubTypesUrl = '/SalesUser/SourcingRequest/GetFeeSubTypes?currency=';
    private getFeeConstraintTypesUrl = '/Supplier/Invoice/GetFeeConstraintTypes';
    //private getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';

    constructor(private httpClient: HttpClient) {
        super();
    }

    getFeeTypes(orderId: number,currancy: string, isSale:boolean): Observable<DropdownItemExt[]> {
        if(isSale){
            return this.httpClient.get<DropdownItemExt[]>(this.getSalesFeeTypesUrl + currancy)
            .pipe(catchError(this.handleError<DropdownItemExt[]>('getSalesFeeTypesUrl', [])));
        }else{
            return this.httpClient.get<DropdownItemExt[]>(this.getFeeTypesUrl + orderId)
            .pipe(catchError(this.handleError<DropdownItemExt[]>('getFeeTypes', [])));
        }
        
    }

    getFeeSubTypes(orderId: number,currancy: string, isSale:boolean): Observable<FeeSubType[]> {
        if(isSale){
            return this.httpClient.get<FeeSubType[]>(this.getSalesFeeSubTypesUrl + currancy)
            .pipe(catchError(this.handleError<FeeSubType[]>('getSalesFeeSubTypesUrl', [])));
        }else{
            return this.httpClient.get<FeeSubType[]>(this.getFeeSubTypesUrl + orderId)
            .pipe(catchError(this.handleError<FeeSubType[]>('getFeeSubTypes', [])));
        }
       
    }

    getFeeConstraintTypes(): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getFeeConstraintTypesUrl)
            .pipe(catchError(this.handleError<DropdownItem[]>('getFeeConstraintTypes', [])));
    }

   
}
