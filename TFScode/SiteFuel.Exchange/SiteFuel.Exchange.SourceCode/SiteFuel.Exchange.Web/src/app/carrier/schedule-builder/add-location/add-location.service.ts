import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError } from 'src/app/errors/HandleError';
import { Geocode } from './add-location.model';

@Injectable({
    providedIn: 'root'
})
export class AddLocationService extends HandleError {

    private getaddressByZipUrl = "Validation/GetAddressByZip";
    private getOpisTerminalsUrl = "Supplier/Order/GetOpisTerminals";
    private getAllTPOCompaniesUrl = "/Carrier/Order/GetAllTPOCompanies";
    //private getAllBuyerCompaniesUrl = "/Carrier/Order/GetAllBuyerCompanies";
    private getStatesOfAllCountrieslistUrl = "/Supplier/Order/GetStatesOfAllCountries";
    private getTPOContactPersonDetailsUrl = "/Supplier/Order/GetTPOContactPersonDetails";
    private getFuelProductslistUrl = "/Supplier/Order/GetFuelProducts";
    private getMarineProductListUrl = "/Supplier/FuelRequest/GetProductList";
    private getFuelProductsByZiplistUrl = "/Supplier/Order/GetProductListByZip";
    private postCreateOrderUrl = "/Carrier/ScheduleBuilder/Create";
    private isTpoCompanyExistUrl = "/Validation/IsCompanyNameExist";
    private isJobNameExistUrl = "/Supplier/Order/ValidateJobNameByCompanyId";
    private isIsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid";
    private GetPricingCodesUrl = "/Supplier/Order/GetPricingCodes";
    private getClosedTerminalUrl = "Supplier/Order/GetClosedTerminal";
    private getCurrancylistUrl = "/Supplier/Order/GetCurrenyList";
    private getTPOCompanyContactPersonsUrl = "/Supplier/Order/GetTPOCompanyContactPersons";
    private GetaddressbyLatLongUrl = "/Supplier/Order/GetAddressByLongLat";
    private GetCityGroupTerminalsUrl = "/Supplier/Order/GetCityGroupTerminals";
    private GetTerminalsUrl = "/Carrier/ScheduleBuilder/GetTerminalsForMultipleProducts";
    private isCityGroupTerminalPriceAvailableUrl = "/Supplier/FuelRequest/IsCityGroupTerminalPriceAvailable";
    private getPreferencesSettingsUrl = "/Carrier/ScheduleBuilder/GetPreferenceSettingForOnTheFlyLocation";
    private getTimeZoneNameUrl = "https://maps.googleapis.com/maps/api/timezone/json";
    private GetAddressUrl = "/Validation/GetAddress?address=";
    private getTfxProductUrl = "Supplier/Order/GetTfxProduct";
    constructor(private httpClient: HttpClient) { super(); }

    getAllTPOCompanies(): Observable<any> {
        return this.httpClient.get<any>(this.getAllTPOCompaniesUrl)
            .pipe(catchError(this.handleError<any>('GetAllTPOCompanies', null)));
    }
    getTPOCompanyContactPersons(companyId: string): Observable<any> {
        return this.httpClient.get<any>(`${this.getTPOCompanyContactPersonsUrl}?companyId=${companyId}`)
            .pipe(catchError(this.handleError<any>('getTPOCompanyContactPersons', null)));
    }
    getTPOContactPersonDetails(userId: number): Observable<any> {
        return this.httpClient.get<any>(`${this.getTPOContactPersonDetailsUrl}?userId=${userId}`)
            .pipe(catchError(this.handleError<any>('getTPOContactPersonDetails', null)));
    }
    getCurrenyList(): Observable<any> {
        return this.httpClient.get<any>(this.getCurrancylistUrl)
            .pipe(catchError(this.handleError<any>('getCurrenyList', null)));
    }
    getStatesOfAllCountries(countryId?: number): Observable<any> {
        return this.httpClient.get<any>(`${this.getStatesOfAllCountrieslistUrl}?countryId=${countryId}`)
            .pipe(catchError(this.handleError<any>('getStatesOfAllCountries', null)));
    }
    getFuelProducts(productDisplayGroupId: any, companyId: any, jobId: any): Observable<any> {
        return this.httpClient.get<any>(`${this.getFuelProductslistUrl}?productDisplayGroupId=${productDisplayGroupId}&companyId=${companyId}&jobId=${jobId}`)
            .pipe(catchError(this.handleError<any>('GetFuelProducts', null)));
    }
    getMarineProductList(displayGroupId: number, jobId: any, zipCode: string, source: number): Observable<any> {
        return this.httpClient.get<any>(`${this.getMarineProductListUrl}?displayGroupId=${displayGroupId}&jobId=${jobId}&zipCode=${zipCode}&source=${source}`)
            .pipe(catchError(this.handleError<any>('getMarineProductList', null)));
    }
    getProductListByZip(zipCode: any, radius: number): Observable<any> {
        return this.httpClient.get<any>(`${this.getFuelProductsByZiplistUrl}?zipCode=${zipCode}&radius=${radius}`)
            .pipe(catchError(this.handleError<any>('getProductListByZip', null)));
    }
    getFuelTerminals(jobCountryId: number, pricingCodeId: number, fuelType: number, companyCountryId: number, isSupressOrderPricing: boolean, jobLatitude: number, jobLongitude: number, searchStringTeminal: string): Observable<any> {
        return this.httpClient.get<any>(this.GetTerminalsUrl + '?jobCountryId=' + jobCountryId + '&pricingCodeId=' + pricingCodeId + '&fuelType=' + fuelType + '&companyCountryId=' + companyCountryId + '&isSupressOrderPricing=' + isSupressOrderPricing + '&jobLatitude=' + jobLatitude + '&jobLongitude=' + jobLongitude + '&searchStringTeminal=' + searchStringTeminal)
            .pipe(catchError(this.handleError<any>('getFuelTerminals', null)));
    }
    getOpisTerminals(cityRackId: number, latitude: number, longitude: number, countryId: number, terminal: string, source: number): Observable<any> {
        return this.httpClient.get<any>(this.getOpisTerminalsUrl + '?cityRackId=' + cityRackId + '&countryId=' + countryId + '&latitude=' + latitude + '&longitude=' + longitude + '&terminal=' + terminal + '&source=' + source)
            .pipe(catchError(this.handleError<any>('getOpisTerminals', null)));
    }
    getTfxProduct(tfxProductId: number): Observable<any> {
        return this.httpClient.get<any>(this.getTfxProductUrl + '?tfxProductId=' + tfxProductId)
            .pipe(catchError(this.handleError<any>('getProductIdForTfxProduct', null)));
    }
    createOrder(modal: any): Observable<any> {
        return this.httpClient.post<any>(this.postCreateOrderUrl, modal)
            .pipe(catchError(this.handleError<any>('createOrder', null)));
    }

    getTimeZoneName(latitude: any, longitude: any, timestamp: any, apiKey: any): Observable<any> {
        return this.httpClient.get<any>(this.getTimeZoneNameUrl + "?location=" + latitude + "," + longitude + "&timestamp=" + timestamp + "&key=" + apiKey)
            .pipe(catchError(this.handleError<any>('createOrder', null)));
    }

    isTpoCompanyExist(IsNewCompany: any, CompanyName: any): Observable<boolean> {
        return this.httpClient.get<any>(`${this.isTpoCompanyExistUrl}?IsNewCompany=${IsNewCompany}&CompanyName=${CompanyName}`)
            .pipe(catchError(this.handleError<any>('isTpoCompanyExist', null)));
    }

    isJobNameExist(jobName: string, companyId: number): Observable<boolean> {
        return this.httpClient.get<any>(`${this.isJobNameExistUrl}?jobName=${jobName}&companyId=${companyId}`)
            .pipe(catchError(this.handleError<any>('isJobNameExist', null)));
    }

    IsPhoneNumberValid(phoneNumber: string): Observable<boolean> {

        return this.httpClient.get<any>(`${this.isIsPhoneNumberValidUrl}?phoneNumber=${phoneNumber}`)
            .pipe(catchError(this.handleError<any>('IsPhoneNumberValid', null)));
    }

    getClosedTerminal(fuelTypeId,
        latitude,
        longitude,
        countryId,
        pricingCodeId,
        terminal,
        pricingSourceId,
        cityRackId): Observable<any> {
        return this.httpClient.get<any>(`${this.getClosedTerminalUrl}?fuelTypeId=${fuelTypeId}&latitude=${latitude}&longitude=${longitude}&countryId=${countryId}&pricingCodeId=${pricingCodeId}&terminal=${terminal}&pricingSourceId=${pricingSourceId}&cityRackId=${cityRackId}`)
            .pipe(catchError(this.handleError<any>('getClosedTerminal', null)));
    }
    getPricingCodes(filterModel: any) {
        return this.httpClient.get<any>(`${this.GetPricingCodesUrl}?PricingTypeId=${filterModel.PricingTypeId}&PricingSourceId=${filterModel.PricingSourceId}&feedTypeId=${filterModel.feedTypeId}&fuelClassTypeId=${filterModel.fuelClassTypeId}&tfxProdId=${filterModel.tfxProdId}&Prefix=${filterModel.Prefix}`)
            .pipe(catchError(this.handleError<any>('getPricingCodes', null)));
    }

    getAddressByZip(zipCode: string, address: string): Observable<Geocode> {
        return this.httpClient.get<any>(`${this.getaddressByZipUrl}?zipCode=${zipCode}&address=${address}`)
            .pipe(catchError(this.handleError<any>('getAddressByZip', null)));
    }

    GetAddressByLongLat(latitude: string, longitude: string): Observable<Geocode> {
        return this.httpClient.get<any>(`${this.GetaddressbyLatLongUrl}?latitude=${latitude}&longitude=${longitude}`)
            .pipe(catchError(this.handleError<any>('GetAddressByLongLat', null)));
    }

    GetCityGroupTerminals(stateId: string, allStates: boolean, sourceId: number): Observable<any> {
        return this.httpClient.get<any>(`${this.GetCityGroupTerminalsUrl}?stateId=${stateId}&allStates=${allStates}&sourceId=${sourceId}`)
            .pipe(catchError(this.handleError<any>('GetCityGroupTerminals', null)));
    }
    IsCityGroupTerminalPriceAvailable(jobid, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId): Observable<boolean> {
        return this.httpClient.get<any>(this.isCityGroupTerminalPriceAvailableUrl + '?jobid=' + jobid + '&fueltypeId=' + fueltypeId + '&selectedCityRackId=' + selectedCityRackId + '&lattitude=' + lattitude + '&longitude=' + longitude + '&countryCode=' + countryCode + '&sourceId=' + sourceId).pipe(catchError(this.handleError<any>('IsCityGroupTerminalPriceAvailable', null)));
    }

    GetPreferencesSettings(): Observable<any> {
        return this.httpClient.get<any>(this.getPreferencesSettingsUrl)
            .pipe(catchError(this.handleError<any>('GetPreferencesSettings', null)));
    }

    GetAddress(address: string): Observable<Geocode> {
        return this.httpClient.get<any>(this.GetAddressUrl + address)
            .pipe(catchError(this.handleError<any>('GetaddressbyAddress', null)));
    }
}
