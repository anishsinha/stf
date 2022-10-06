import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HandleError } from 'src/app/errors/HandleError';
import { DropdownItem } from 'src/app/statelist.service';

@Injectable({
    providedIn: 'root'
})
export class AddressService extends HandleError {

    private getAddressUrl = '/Validation/GetAddressByZip?zipCode=';
    private getBulkPlantsUrl = '/Supplier/SupplierBase/GetBulkPlants?Prefix=';
    private getBulkPlantsForAutoFreightMethodUrl: string = 'Supplier/Invoice/GetBulkPlantsForAutoFreightMethod?orderId=';
    private getBulkPlantDetailsUrl = '/Supplier/SupplierBase/GetBulkPlantDetails?name=';
    private getBulkplantAddressUrl = '/Supplier/Invoice/GetBulkplantAddress?trackableScheduleId=';

    constructor(private httpClient: HttpClient) {
        super();
    }

    getAddress(zipCode: string): Observable<any> {
        return this.httpClient.get<any>(this.getAddressUrl + zipCode)
            .pipe(catchError(this.handleError<any>('getAddress', null)));
    }

    getBulkPlants(prfix: string, orderId: number = 0): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getBulkPlantsUrl + prfix + '&orderId=' + orderId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getBulkPlants', null)));
    }
    getBulkPlantsForAutoFreightMethod(_orderId: number, _bulkPlant?: string): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getBulkPlantsForAutoFreightMethodUrl + _orderId + '&bulkPlant=' + _bulkPlant)
            .pipe(catchError(this.handleError<DropdownItem[]>('getBulkPlantsForAutoInvoice', null)));
    }
    GetBulkPlantDetails(name: string): Observable<any> {
        return this.httpClient.get<any>(this.getBulkPlantDetailsUrl + name)
            .pipe(catchError(this.handleError<any>('GetBulkPlantDetails', null)));
    }

    getBulkplantAddress(scheduleId: number, orderId: number): Observable<any> {
        return this.httpClient.get<any>(this.getBulkplantAddressUrl + scheduleId + '&orderId=' + orderId)
            .pipe(catchError(this.handleError<any>('getBulkplantAddress', null)));
    }
}
