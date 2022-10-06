import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, timer } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { HandleError } from '../../app/errors/HandleError';
import { DropdownItem } from '../statelist.service';
import { ServiceArea, StateListViewModel } from './third-party-invitation.model';

@Injectable({
  providedIn: 'root'
})
export class InvitationService extends HandleError {
    private isCompanyNameExistUrl = "Validation/IsCompanyNameExist";
    private getCountrylistUrl = "Invitation/GetCountryList";
    private getStatesOfAllCountrieslistUrl = "Invitation/GetStateList";
    private postInvitationRequestUrl = "Invitation/Save";
    private getThirdPartyCompanyTypesUrl = "Invitation/GetThirdPartyCompanyTypes";
    private getAllTrailerAssetTypesUrl = "Invitation/GetAllTrailerAssetTypes";
    private getCitiesFromStatesUrl = "Invitation/GetCityAndZipsByState";
    private getaddressByZipUrl = "Validation/GetAddressByZip";
    private getCarrierOnboardingForBrandingUrl = "Invitation/getCarrierOnboardingForBranding";
    
    private GetPhoneTypesUrl = "Invitation/GetPhoneTypes";
    private IsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid";
    //private GetaddressbyLatLongUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private isEmailExistUrl = "Invitation/IsEmailExist";
    private GetCompaniesUrl = "/Invitation/GetCompanies";

    constructor(private httpClient: HttpClient) { super(); }

    public IsCompanyNameExist(IsNewCompany:any,CompanyName:any): Observable<any>{
        return this.httpClient.get<any>(`${this.isCompanyNameExistUrl}?IsNewCompany=${IsNewCompany}&CompanyName=${CompanyName}`)
            .pipe(catchError(this.handleError<any>('IsCompanyNameExist', null))); 
    }
    public GetCountryList(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getCountrylistUrl)
            .pipe(catchError(this.handleError<any>('GetCountryList', null)));
    }
    public GetStatesOfAllCountries(): Observable<StateListViewModel[]> {
        return this.httpClient.get<any>(this.getStatesOfAllCountrieslistUrl)
            .pipe(catchError(this.handleError<any>('GetStatesOfAllCountries', null)));
    }
    SaveInvitedRequest(sourcingRequestModel: any): Observable<any> {
        return this.httpClient.post<any>(this.postInvitationRequestUrl, sourcingRequestModel)
            .pipe(catchError(this.handleError<any>('SaveInvitedRequest', null)));
    }
    public GetThirdPartyCompanyTypes(): Observable<any> {
        return this.httpClient.get<any>(this.getThirdPartyCompanyTypesUrl)
            .pipe(catchError(this.handleError<any>('GetThirdPartyCompanyTypes', null)));
    }
    public GetAllTrailerAssetTypes(): Observable<any> {
        return this.httpClient.get<any>(this.getAllTrailerAssetTypesUrl)
            .pipe(catchError(this.handleError<any>('GetAllTrailerAssetTypes', null)));
    }
    public GetCitiesFromStates(stateIds: string): Observable<ServiceArea[]> {
        return this.httpClient.get<any>(`${this.getCitiesFromStatesUrl}?stateIds=${stateIds}`)
            .pipe(catchError(this.handleError<any>('GetCitiesFromStates', null)));
    }
    public GetAddressByZip(zipCode: string) {
        return this.httpClient.get<any>(`${this.getaddressByZipUrl}?zipCode=${zipCode}`)
            .pipe(catchError(this.handleError<any>('GetAddressByZip', null)));
    }
    public GetCarrierOnboardingForBranding(token: string) {
        return this.httpClient.get<any>(`${this.getCarrierOnboardingForBrandingUrl}?token=${token}`)
            .pipe(catchError(this.handleError<any>('GetCarrierOnboardingForBranding', null)));
    }
    public GetPhoneTypes(): Observable<any> {
        return this.httpClient.get<any>(this.GetPhoneTypesUrl)
            .pipe(catchError(this.handleError<any>('GetPhoneTypes', null)));
    }
    public IsPhoneNumberValid(phoneNumber: string): Observable<boolean> {
        return this.httpClient.get<any>(`${this.IsPhoneNumberValidUrl}?phoneNumber='${phoneNumber}`)
            .pipe(catchError(this.handleError<any>('IsPhoneNumberValid', null)));
    }
    public IsEmailExist(email: string): Observable<any> {
        return this.httpClient.get<any>(`${this.isEmailExistUrl}?email=${email}`)
            .pipe(catchError(this.handleError<any>('IsEmailExist', null)));
    }
    public GetCompanies(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.GetCompaniesUrl)
            .pipe(catchError(this.handleError<any>('GetCompanies', null)));
    }
}
