import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HandleError } from '../errors/HandleError';

@Injectable({
  providedIn: 'root'
})
export class CreateterminalsService extends HandleError{
    private urlGetTerminalsForGrid = '/SuperAdmin/SuperAdmin/GetTerminals';
    private urlSaveTerminalDetails = '/SuperAdmin/SuperAdmin/SaveTerminal';
    private urlGetTerminalMappingDetails = 'SuperAdmin/SuperAdmin/GetTerminalMappingDetails';
    private urlGetAllProductsMapping = 'SuperAdmin/SuperAdmin/GetMstProductsForTerminalMapping';
    private urlSaveTerminalProductMapping = 'SuperAdmin/SuperAdmin/SaveTerminalProductMapping';

    constructor(private httpClient: HttpClient) {
        super();
    }

    public getTerminalsForGrid(countryId: number) {
        return this.httpClient.get<any[]>(this.urlGetTerminalsForGrid + '?countryId=' + countryId)
            .pipe(catchError(this.handleError<any[]>('getTerminalsForGrid', [])));
    }

    saveTerminalDetails(terminal: any) {
        return this.httpClient.post<any>(this.urlSaveTerminalDetails, terminal)
            .pipe(catchError(this.handleError<any>('saveTerminalDetails', terminal)));
    }

    public getTerminalProductMappingDetails(countryId: number) {
        return this.httpClient.get<any[]>(this.urlGetTerminalMappingDetails + '?countryId=' + countryId)
            .pipe(catchError(this.handleError<any[]>('getTerminalProductMappingDetails', [])));
    }

    public getAllProductsForMapping() {
        return this.httpClient.get<any[]>(this.urlGetAllProductsMapping)
            .pipe(catchError(this.handleError<any[]>('getAllProductsForMapping', [])));
    }
    public saveTerminalProductMapping(model: any) {
        return this.httpClient.post<any>(this.urlSaveTerminalProductMapping, model)
            .pipe(catchError(this.handleError<any>('saveTerminalProductMapping', model)));
    }
}
