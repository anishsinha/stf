import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocationService extends HandleError{
    private GetTerminalsUrl = '/Location/GetTerminals';
    private GetBulkPlantsUrl = '/Location/GetBulkPlants?countryId=';
    private PostBulkPlantLocationUrl = '/Location/SaveBulkPlantLocation';

    constructor(private httpClient: HttpClient) { super(); }

    getTerminals(requestModel: any): Observable<any> {
        return this.httpClient.post<any>(this.GetTerminalsUrl, requestModel)
            .pipe(catchError(this.handleError<any>('getTerminals', null)));
    }

    GetBulkPlants(countryId: number): Observable<any> {
        return this.httpClient.get<any>(this.GetBulkPlantsUrl + countryId)
            .pipe(catchError(this.handleError<any>('GetBulkPlants', null)));
    }

    PostBulkPlantLocation(data: any): Observable<any> {
        return this.httpClient.post<any>(this.PostBulkPlantLocationUrl, data)
            .pipe(catchError(this.handleError<any>('PostBulkPlantLocation', null)));
    }
}
