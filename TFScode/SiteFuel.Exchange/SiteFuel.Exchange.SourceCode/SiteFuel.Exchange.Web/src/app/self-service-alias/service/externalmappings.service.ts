import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ExternalCustomerMappingViewModel, ExternalCustomerLocationMappingViewModel, ExternalSupplierMappingViewModel, ExternalTerminalMappingViewModel, ExternalProductMappingViewModel, ExternalBulkPlantMappingViewModel, ExternalCarrierMappingViewModel, ExternalDriverMappingViewModel, ExternalVehicleMappingViewModel } from '../models/ExternalMappingModel'
import { ExternalBulkPlantMappingsComponent } from '../external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component';
import { DropdownCustomItem, DropdownItem } from '../../statelist.service';
import { FuelGroupType } from '../models/FuelGroupGridViewModel';

@Injectable({
    providedIn: 'root'
})
export class ExternalMappingsService extends HandleError {
    private urlGetExternalCustomers = '/ExternalEntityMappings/GetExternalCompanies';

    private urlGetCustomersForExternalMapping = '/ExternalEntityMappings/GetCustomersForExternalMapping';
    private urlSaveCustomerExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerMappings';
    private urlBulkUploadCustomerMapping = '/ExternalEntityMappings/BulkUploadCustomerMapping';

    private urlGetCustomerLocationsForExternalMapping = '/ExternalEntityMappings/GetCustomerLocationsForExternalMapping';
    private urlSaveCustomerLocationExternalMappings = '/ExternalEntityMappings/SaveExternalCustomerLocationMappings';
    private urlBulkUploadCustomerLocationMapping = '/ExternalEntityMappings/BulkUploadCustomerLocationMapping';

    private urlGetProductsForExternalMapping = '/ExternalEntityMappings/GetProductsForExternalMapping';
    private urlSaveExternalProductMappings = '/ExternalEntityMappings/SaveExternalProductMappings';
    private urlBulkUploadProductMapping = '/ExternalEntityMappings/BulkUploadProductMapping';

    private urlGetSuppliersForExternalMapping = '/ExternalEntityMappings/GetSuppliersForExternalMapping';
    private urlSaveExternalSupplierMappings = '/ExternalEntityMappings/SaveExternalSupplierMappings';
    private urlBulkUploadSupplierMapping = '/ExternalEntityMappings/BulkUploadSupplierMapping';
    
    private urlGetTerminalsForExternalMapping = '/ExternalEntityMappings/GetTerminalsForExternalMapping';
    private urlSaveExternalTerminalMappings = '/ExternalEntityMappings/SaveExternalTerminalMappings';
    private urlBulkUploadTerminalMapping = '/ExternalEntityMappings/BulkUploadTerminalMapping';

    private urlGetBulkPlantsForExternalMapping = '/ExternalEntityMappings/GetBulkPlantsForExternalMapping';
    private urlSaveExternalBulkPlantMappings = '/ExternalEntityMappings/SaveExternalBulkPlantMappings';
    private urlBulkUploadBulkPlantMapping = '/ExternalEntityMappings/BulkUploadBulkPlantMapping';

    private urlGetDriversForExternalMapping = '/ExternalEntityMappings/GetDriversForExternalMapping';
    private urlSaveExternalDriverMappings = '/ExternalEntityMappings/SaveExternalDriverMappings';
    private urlBulkUploadDriverMapping = '/ExternalEntityMappings/BulkUploadDriverMapping';

    private urlGetCarriersForExternalMapping = '/ExternalEntityMappings/GetCarriersForExternalMapping';
    private urlSaveExternalCarrierMappings = '/ExternalEntityMappings/SaveExternalCarrierMappings';
    private urlBulkUploadCarrierMapping = '/ExternalEntityMappings/BulkUploadCarrierMapping';

    private urlGetGetVehiclesForExternalMapping = '/ExternalEntityMappings/GetVehiclesForExternalMapping';
    private urlSaveExternalVehicleMappings = '/ExternalEntityMappings/SaveExternalVehicleMappings';
    private urlBulkUploadVehicleMapping = '/ExternalEntityMappings/BulkUploadVehicleMapping';

    public urlGetFuelGroupSummary = '/FuelGroup/GetFuelGroupSummary';
    public urlArchiveFuelGroup = '/FuelGroup/ArchiveFuelGroup';
    public getFuelTypesUrl = '/FuelGroup/GetFuelTypes?productTypeIds=';
    public getProductTypeUrl = '/FuelGroup/GetProductTypes';
    public getFuelGroupDetailsUrl = '/FuelGroup/GetFuelGroupDetails?fuelGroupId=';
    public getFuelGroupsUrl = "/FuelGroup/GetFuelGroups?fuelGroupType=";

    

    constructor(private httpClient: HttpClient) { super(); }

    
    getExternalCompanies(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetExternalCustomers)
            .pipe(catchError(this.handleError<any>('getExternalCompanies', null)));
    }
    getCustomersForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCustomersForExternalMapping)
            .pipe(catchError(this.handleError<any>('getCustomersForExternalMapping', null)));
    }
    SaveExternalCustomerMappings(customerDetails: ExternalCustomerMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveCustomerExternalMappings, customerDetails)
            .pipe(catchError(this.handleError<any>('SaveCustomerExternalMappings', null)));
    }
    getCustomerLocationsForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCustomerLocationsForExternalMapping)
            .pipe(catchError(this.handleError<any>('getCustomerLocationsForExternalMapping', null)));
    }
    SaveExternalCustomerLocationMappings(locationDetails: ExternalCustomerLocationMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveCustomerLocationExternalMappings, locationDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalCustomerLocationMappings', null)));
    }
    getProductsForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetProductsForExternalMapping)
            .pipe(catchError(this.handleError<any>('getProductsForExternalMapping', null)));
    }
    SaveExternalProductMappings(productDetails: ExternalProductMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalProductMappings, productDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalProductMappings', null)));
    }

    getSuppliersForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetSuppliersForExternalMapping)
            .pipe(catchError(this.handleError<any>('getSuppliersForExternalMapping', null)));
    }
    SaveExternalSupplierMappings(supplierDetails: ExternalSupplierMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalSupplierMappings, supplierDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalSupplierMappings', null)));
    }

    getTerminalsForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetTerminalsForExternalMapping)
            .pipe(catchError(this.handleError<any>('getTerminalsForExternalMapping', null)));
    }
    SaveExternalTerminalMappings(terminalDetails: ExternalTerminalMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalTerminalMappings, terminalDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalTerminalMappings', null)));
    }
    getBulkPlantsForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetBulkPlantsForExternalMapping)
            .pipe(catchError(this.handleError<any>('getBulkPlantsForExternalMapping', null)));
    }
    SaveExternalBulkPlantMappings(bulkPlantDetails: ExternalBulkPlantMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalBulkPlantMappings, bulkPlantDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalBulkPlantMappings', null)));
    }
    getDriversForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetDriversForExternalMapping)
            .pipe(catchError(this.handleError<any>('GetDriversForExternalMapping', null)));
    }
    SaveExternalDriverMappings(driverDetails: ExternalDriverMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalDriverMappings, driverDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalDriverMappings', null)));
    }
    getCarriersForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetCarriersForExternalMapping)
            .pipe(catchError(this.handleError<any>('getCarriersForExternalMapping', null)));
    }
    SaveExternalCarrierMappings(carrierDetails: ExternalCarrierMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalCarrierMappings, carrierDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalCarrierMappings', null)));
    }

    getVehiclesForExternalMapping(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetGetVehiclesForExternalMapping)
            .pipe(catchError(this.handleError<any>('getVehiclesForExternalMapping', null)));
    }
    SaveExternalVehicleMappings(vehicleDetails: ExternalVehicleMappingViewModel): Observable<any> {
        return this.httpClient.post<any>(this.urlSaveExternalVehicleMappings, vehicleDetails)
            .pipe(catchError(this.handleError<any>('SaveExternalVehicleMappings', null)));
    }
    BulkUploadCustomerMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCustomerMapping, formData)
    } 
    BulkUploadCustomerLocationMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCustomerLocationMapping, formData)
    } 
  BulkUploadProductMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
      return this.httpClient.post(this.urlBulkUploadProductMapping, formData)
    } 
    BulkUploadSupplierMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadSupplierMapping, formData)
    } 
    BulkUploadBulkPlantMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadBulkPlantMapping, formData)
    } 
    BulkUploadTerminalMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadTerminalMapping, formData)
    } 
    BulkUploadCarrierMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadCarrierMapping, formData)
    } 
    BulkUploadDriverMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadDriverMapping, formData)
    } 
    BulkUploadVehicleMapping(file): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        return this.httpClient.post(this.urlBulkUploadVehicleMapping, formData)
    }

    getFuelGroupSummary(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetFuelGroupSummary)
            .pipe(catchError(this.handleError<any>('getFuelGroupSummary', null)));
    }

    archiveFuelGroup(fuelGroupId: string): Observable<any> {
        return this.httpClient.post<any>(this.urlArchiveFuelGroup, { fuelGroupId: fuelGroupId })
            .pipe(catchError(this.handleError<any>('archiveFuelGroup', null)));
    }

    getFuelTypeList(productTypeIds: string, fuelGroupType: string, editingGroupId: number, editingcompanyId: number): Observable<DropdownCustomItem[]> {
        return this.httpClient.get<any>(this.getFuelTypesUrl + productTypeIds + "&fuelGroupType=" + fuelGroupType + "&editingGroupId=" + editingGroupId + "&editingcompanyId=" + editingcompanyId)
            .pipe(catchError(this.handleError<any>('getFuelTypeList', null)));
    }

    public getProductTypeList(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getProductTypeUrl)
            .pipe(catchError(this.handleError<any>('getProductTypeList', null)));
    }

    getFuelGroup(groupId: number): Observable<any> {
        return this.httpClient.get<any>(this.getFuelGroupDetailsUrl + groupId)
            .pipe(catchError(this.handleError<any>('getFuelGroup', null)));
    }

    getFuelGroups(fuelGroupType:number,companyIds: string): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getFuelGroupsUrl + fuelGroupType + "&companyIds=" + companyIds)
            .pipe(catchError(this.handleError<any>('getFuelGroups', null)));
    }

    
}