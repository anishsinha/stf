import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, timer } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { HandleError } from '../../app/errors/HandleError';
import { CustomersModel, DeliveryRequestInputModel, Geocode, ValidateDREntryResponse } from './sales-user.model';

@Injectable({
  providedIn: 'root'
})
export class SalesUserService extends HandleError{
  private getTruckLoadTypeUrl = "/SalesUser/SourcingRequest/TruckLoadType";
  private getFreightOnBoardTypesUrl = "/SalesUser/SourcingRequest/FreightOnBoardType";
  private getJoblistUrl = "/SalesUser/SourcingRequest/GetJobLists";
  private isSourcingCompanyExistUrl = "/SalesUser/SourcingRequest/IsSourcingCompanyExist";
  private getJobDetailsUrl = "/SalesUser/SourcingRequest/GetJobDetails";
  private getCountrylistUrl = "/SalesUser/SourcingRequest/GetCountryList";
  private getCurrancylistUrl = "/SalesUser/SourcingRequest/GetCurrenyList";
  private getUOMlistUrl = "/SalesUser/SourcingRequest/GetUoMList";
  private getStatesOfAllCountrieslistUrl = "/SalesUser/SourcingRequest/GetStatesOfAllCountries";
  private getFuelProductslistUrl = "/SalesUser/SourcingRequest/GetFuelProducts";
  private getFuelProductsByZiplistUrl = "/SalesUser/SourcingRequest/GetProductListByZip";
  private getQuantityIndicatorTypeslistUrl = "/SalesUser/SourcingRequest/QuantityIndicatorTypes";
  private getRackAvgPricingTypelistUrl = "/SalesUser/SourcingRequest/GetRackAvgPricingTypes";
  private getAllFeeTypeslistUrl = "/SalesUser/SourcingRequest/GetAllFeeTypes";
  private getAllFeeSubTypeslistUrl = "/SalesUser/SourcingRequest/GetAllFeeSubTypes";
  private getAllFeeConstraintTypeslistUrl = "/SalesUser/SourcingRequest/GetAllFeeConstraintTypes";
  private getPaymentMethodslistUrl = "/SalesUser/SourcingRequest/PaymentMethods";
  private getSourcingRequestsUrl = "/SalesUser/SourcingRequest/GetSourcingRequestGrid";
  private getAllBuyerCompaniesUrl = "/SalesUser/SourcingRequest/GetAllBuyerCompanies";
  private getGetSourcingCompanyContactPersonsUrl = "/SalesUser/SourcingRequest/GetSourcingCompanyContactPersons";
  private getGetSourcingContactPersonDetailsUrl = "/SalesUser/SourcingRequest/GetSourcingContactPersonDetails";
  private postSourcingRequestUrl = "/SalesUser/SourcingRequest/Create";
  private postSourcingRequestEditUrl = "/SalesUser/SourcingRequest/Edit";
  private getSourcingRequestUrl = "/SalesUser/SourcingRequest/GetRequestDetails";
  private getPreferencesSettingsUrl = "/SalesUser/SourcingRequest/GetPreferencesSettings";
  private postCreateOrderFromSourcingRequestUrl = "/SalesUser/SourcingRequest/CreateOrderFromSourcingRequest";
  private ChangesSourcingRequestStatusUrl = "/SalesUser/SourcingRequest/ChangesSourcingRequestStatus";
  private getSourcingCityGroupTerminalsUrl="SalesUser/SourcingRequest/GetSourcingCityGroupTerminals";
  private getIsCitySourcingGroupTerminalPriceAvailableUrl="SalesUser/SourcingRequest/IsCitySourcingGroupTerminalPriceAvailable";
  private getClosedTerminalUrl="SalesUser/SourcingRequest/GetClosedTerminal";
  private getOpisTerminalslUrl="SalesUser/SourcingRequest/GetOpisTerminals";
  private getaddressByZipUrl="Validation/GetAddressByZip";

  /*Sales Form DR*/
  private UrlGetJobDetailsFrom = "/Order/GetJobDetails";
  private UrlGetCreateDREntryForm = "/SalesUser/Dashboard/CreateSalesDR";
  private UrlGetValidateDREntryFormUrl = "/SalesUser/Dashboard/ValidateSalesDR";
  private UrlGetAllBuyersForAllRegions = "/SalesUser/Dashboard/GetCustomersJobsForSalesDR";
  private UrlGetProductsForSalesDR = "/SalesUser/Dashboard/GetProductsForSalesDR";
  /*END Sales Form DR*/
    

  private GetPricingCodesUrl = "/SalesUser/SourcingRequest/GetPricingCodes";
  private GetUserContextUrl = "/SalesUser/SourcingRequest/GetUserContext";
  private GetSalesUserOrdersUrl = "/SalesUser/Dashboard/GetSalesUserOrders";
  private IsPhoneNumberValidUrl = "/Validation/IsPhoneNumberValid";
  private UpdateViewedStatusUrl = "/SalesUser/SourcingRequest/UpdateViewedStatus";
  // private GetaddressbyLatLongUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private GetaddressbyLatLongUrl = "/Supplier/Order/GetAddressByLongLat";

    private urlGetCountryGroupList = "/SalesUser/SourcingRequest/GetCountryGroupList"; 


  constructor(private httpClient: HttpClient) { super(); }

  public GetAllTPOCompanies(): Observable<any> {
      return this.httpClient.get<any>(this.getAllBuyerCompaniesUrl)
      .pipe(catchError(this.handleError<any>('GetAllTPOCompanies', null)));
  }
  public GetSourcingCompanyContactPersons(companyId:number): Observable<any> {
    return this.httpClient.get<any>(`${this.getGetSourcingCompanyContactPersonsUrl}?companyId=${companyId}`)
      .pipe(catchError(this.handleError<any>('GetSourcingCompanyContactPersons', null)));
  }
  public GetSourcingContactPersonDetails(userId:number): Observable<any> {
    return this.httpClient.get<any>(`${this.getGetSourcingContactPersonDetailsUrl}?userId=${userId}`)
      .pipe(catchError(this.handleError<any>('GetAllTPGetSourcingContactPersonDetailsOCompanies', null)));
  }

  public GetTruckLoadType(): Observable<any> {
    return this.httpClient.get<any>(this.getTruckLoadTypeUrl)
      .pipe(catchError(this.handleError<any>('GetTruckLoadType', null)));
  }
  public GetFreightOnBoardTypes(): Observable<any> {
    return this.httpClient.get<any>(this.getFreightOnBoardTypesUrl)
      .pipe(catchError(this.handleError<any>('GetFreightOnBoardTypes', null)));
  }
  public GetJobLists(companyName:string ,  isFtl:boolean, foAsTerminal:boolean): Observable<any> {
    return this.httpClient.get<any>( `${this.getJoblistUrl}?companyName=${companyName}&isFtl=${isFtl}&foAsTerminal=${foAsTerminal}`)
      .pipe(catchError(this.handleError<any>('GetJobLists', null)));
  }

  public GetJobDetails(jobName:string,companyName:string ): Observable<any> {
    return this.httpClient.get<any>( `${this.getJobDetailsUrl}?jobName=${jobName}&companyName=${companyName}`)
      .pipe(catchError(this.handleError<any>('GetJobDetails', null)));
  }
  public GetCountryList(): Observable<any> {
    return this.httpClient.get<any>(this.getCountrylistUrl)
      .pipe(catchError(this.handleError<any>('GetCountryList', null)));
  }
  public GetCurrenyList(): Observable<any> {
    return this.httpClient.get<any>(this.getCurrancylistUrl)
      .pipe(catchError(this.handleError<any>('getCurrancylistUrl', null)));
  }

    public GetCountryGroupList(countryId: number): Observable<any>{
        return this.httpClient.get<any>(this.urlGetCountryGroupList + '?countryId=' + countryId)
            .pipe(catchError(this.handleError<any>('GetCountryGroupList', null)));
    }
  
  public GetUoMList(): Observable<any> {
    return this.httpClient.get<any>(this.getUOMlistUrl)
      .pipe(catchError(this.handleError<any>('GetUoMList', null)));
  }
  public GetStatesOfAllCountries(countryId?:number): Observable<any> {
    return this.httpClient.get<any>(`${this.getStatesOfAllCountrieslistUrl}?countryId=${countryId}`)
      .pipe(catchError(this.handleError<any>('GetStatesOfAllCountries', null)));
  }
  public GetFuelProducts(productDisplayGroupId:any, companyId:any, jobId:any): Observable<any> {
    return this.httpClient.get<any>(`${this.getFuelProductslistUrl}?productDisplayGroupId=${productDisplayGroupId}&companyId=${companyId}&jobId=${jobId}`)
      .pipe(catchError(this.handleError<any>('GetFuelProducts', null)));
  }
  public GetProductListByZip(zipCode:any): Observable<any> {
    return this.httpClient.get<any>(`${this.getFuelProductsByZiplistUrl}?zipCode=${zipCode}`)
      .pipe(catchError(this.handleError<any>('GetProductListByZip', null)));
  }
  public QuantityIndicatorTypes(): Observable<any> {
    return this.httpClient.get<any>(this.getQuantityIndicatorTypeslistUrl)
      .pipe(catchError(this.handleError<any>('QuantityIndicatorTypes', null)));
  }
  public GetRackAvgPricingTypes(): Observable<any> {
    return this.httpClient.get<any>(this.getRackAvgPricingTypelistUrl)
      .pipe(catchError(this.handleError<any>('GetRackAvgPricingTypes', null)));
  }
  public GetAllFeeTypes( companyId:any, Currency:any , truckLoadType:any ): Observable<any> {
    return this.httpClient.get<any>(`${this.getAllFeeTypeslistUrl}?companyId=${companyId}&Currency=${Currency}&truckLoadType=${truckLoadType}`)
      .pipe(catchError(this.handleError<any>('GetAllFeeTypes', null)));
  }
  public GetAllFeeSubTypes(feeTypeId:any, Currency:any): Observable<any> {
    return this.httpClient.get<any>( `${this.getAllFeeSubTypeslistUrl}?feeTypeId=${feeTypeId}&Currency=${Currency}`)
      .pipe(catchError(this.handleError<any>('GetAllFeeSubTypes', null)));
  }
  public GetAllFeeConstraintTypes(): Observable<any> {
    return this.httpClient.get<any>(this.getAllFeeConstraintTypeslistUrl)
      .pipe(catchError(this.handleError<any>('GetAllFeeConstraintTypes', null)));
  }
  public PaymentMethods(): Observable<any> {
    return this.httpClient.get<any>(this.getPaymentMethodslistUrl)
      .pipe(catchError(this.handleError<any>('PaymentMethods', null)));
  }
    public GetSourcingRequests(RequestStatus: any, isFromDashboard:any= false): Observable<any> {
        return this.httpClient.get<any>(`${this.getSourcingRequestsUrl}?RequestStatus=${RequestStatus}&isFromDashboard=${isFromDashboard}`)
      .pipe(catchError(this.handleError<any>('GetSourcingRequestsUrl', null)));
  }

  CreateSourcingRequest(sourcingRequestModel: any): Observable<any> {
    return this.httpClient.post<any>(this.postSourcingRequestUrl, sourcingRequestModel)
        .pipe(catchError(this.handleError<any>('CreateSourcingRequest', null)));
  }
  public GetPreferencesSettings(): Observable<any> {
    return this.httpClient.get<any>(this.getPreferencesSettingsUrl)
      .pipe(catchError(this.handleError<any>('GetPreferencesSettings', null)));
  }

  public GetSourcingDetailsById(id): Observable<any> {
    return this.httpClient.get<any>(`${this.getSourcingRequestUrl}?id=${id}`)
      .pipe(catchError(this.handleError<any>('GetSourcingDetailsById', null)));
  }
  CreateOrderFromSourcingRequest(sourcingRequestModel: any): Observable<any> {
    return this.httpClient.post<any>(this.postCreateOrderFromSourcingRequestUrl, sourcingRequestModel)
        .pipe(catchError(this.handleError<any>('CreateOrderFromSourcingRequest', null)));
  }


  public SaveEditSourcingDetails(sourcingRequestModel: any): Observable<any> {
    return this.httpClient.post<any>(this.postSourcingRequestEditUrl, sourcingRequestModel)
        .pipe(catchError(this.handleError<any>('SaveEditSourcingDetails', null)));
  }
  public ChangesSourcingRequestStatus(requestStatus: any,Id: any): Observable<any> {
    var input = {sourcingRequestStatus: requestStatus,Id:Id}
    return this.httpClient.post<any>(this.ChangesSourcingRequestStatusUrl, input)
        .pipe(catchError(this.handleError<any>('ChangesSourcingRequestStatus', null)));
  }

  public IsSourcingCompanyExist(IsNewCompany:any,CompanyName:any): Observable<any> {
    return this.httpClient.get<any>(`${this.isSourcingCompanyExistUrl}?IsNewCompany=${IsNewCompany}&CompanyName=${CompanyName}`)
      .pipe(catchError(this.handleError<any>('IsSourcingCompanyExist', null)));
  }
    public GetSourcingCityGroupTerminals(stateId: any,sourceId: any): Observable<any> {
        return this.httpClient.get<any>(`${this.getSourcingCityGroupTerminalsUrl}?stateId=${stateId}&sourceId=${sourceId}`)
            .pipe(catchError(this.handleError<any>('GetSourcingCityGroupTerminals', null)));
    }
    public IsCitySourcingGroupTerminalPriceAvailable(jobId: any, fueltypeId: any, selectedCityRackId: any, lattitude?: any, longitude?: any, countryCode?: string, sourceId?: any): Observable<any> {
        return this.httpClient.get<any>(`${this.getIsCitySourcingGroupTerminalPriceAvailableUrl}?jobId=${jobId}&fueltypeId=${fueltypeId}&fueltypeId=${selectedCityRackId}&lattitude=${lattitude}&countryCode=${countryCode}&sourceId=${sourceId}`)
            .pipe(catchError(this.handleError<any>('IsCitySourcingGroupTerminalPriceAvailable', null)));
    }
    public GetClosedTerminal(fuelTypeId: any, latitude: any, longitude: any, countryId: any, pricingCodeId: any, terminal: string, pricingSourceId: any): Observable<any> {
        return this.httpClient.get<any>(`${this.getClosedTerminalUrl}?fuelTypeId=${fuelTypeId}&latitude=${latitude}&longitude=${longitude}&countryId=${countryId}&pricingCodeId=${pricingCodeId}&terminal=${terminal}&pricingSourceId=${pricingSourceId}`)
            .pipe(catchError(this.handleError<any>('GetSourcingCityGroupTerminals', null)));
    }
    public GetOpisTerminals(cityRackId: any, latitude: any, longitude: any, countryId: any, terminal: string, source: any): Observable<any> {
        return this.httpClient.get<any>(`${this.getOpisTerminalslUrl}?cityRackId=${cityRackId}&latitude=${latitude}&longitude=${longitude}&countryId=${countryId}&terminal=${terminal}&source=${source}`)
            .pipe(catchError(this.handleError<any>('GetOpisTerminals', null)));
    }
    public GetPricingCodes(filterModel:any)
    {
      return this.httpClient.get<any>(`${this.GetPricingCodesUrl}?PricingTypeId=${filterModel.PricingTypeId}&PricingSourceId=${filterModel.PricingSourceId}&feedTypeId=${filterModel.feedTypeId}&fuelClassTypeId=${filterModel.fuelClassTypeId}&tfxProdId=${filterModel.tfxProdId}&Prefix=${filterModel.Prefix}`)
      .pipe(catchError(this.handleError<any>('GetPricingCodes', null)));
    }

    public GetAddressByZip(zipCode:string){
      return this.httpClient.get<any>(`${this.getaddressByZipUrl}?zipCode=${zipCode}`)
      .pipe(catchError(this.handleError<any>('GetAddressByZip', null)));
    }
    public GetUserContext()
    {
      return this.httpClient.get<any>(`${this.GetUserContextUrl}`)
      .pipe(catchError(this.handleError<any>('GetUserContext', null)));
    }
    public GetOrdersForDashboard(): Observable<any> {
      return this.httpClient.get<any>(`${this.GetSalesUserOrdersUrl}`)
        .pipe(catchError(this.handleError<any>('GetOrdersForDashboard', null)));
    }
    public IsPhoneNumberValid(phoneNumber: string): Observable<any> {
      return this.httpClient.get<any>(`${this.IsPhoneNumberValidUrl}?phoneNumber='${phoneNumber}`)
        .pipe(catchError(this.handleError<any>('IsPhoneNumberValid', null)));
    }

    public UpdateViewedStatus(isViewed: any,Id: any): Observable<any> {
      var input = {Id:Id,IsViewed:isViewed}
    return this.httpClient.post<any>(this.UpdateViewedStatusUrl, input)
        .pipe(catchError(this.handleError<any>('UpdateViewedStatus', null)));
    }

    GetAddressByLongLat(latitude: string, longitude: string): Observable<Geocode> {
      return this.httpClient.get<any>(`${this.GetaddressbyLatLongUrl}?latitude=${latitude}&longitude=${longitude}`)
          .pipe(catchError(this.handleError<any>('GetAddressByLongLat', null)));
    }

    private GetAddressUrl = "/Validation/GetAddress?address=";
    GetAddress(address: string): Observable<Geocode> {
      return this.httpClient.get<any>(this.GetAddressUrl + address)
          .pipe(catchError(this.handleError<any>('GetaddressbyAddress', null)));
    }

    private getDispatchRegionsUrl = "/SalesUser/SourcingRequest/GetDispatchRegions";
    public GetDispatchRegions(): Observable<any> {
        return this.httpClient.get<any>(this.getDispatchRegionsUrl)
            .pipe(catchError(this.handleError<any>('GetDispatchRegions', null)));
    }
    private getSourcingDetailUrl = "/SalesUser/SourcingRequest/GetSourcingDetails";
    public GetSourcingrequestDetailsById(id): Observable<any> {
        return this.httpClient.get<any>(`${this.getSourcingDetailUrl}?id=${id}`)
            .pipe(catchError(this.handleError<any>('GetSourcingrequestDetailsById', null)));
    }
    private GetSalesUserInvoiceUrl = "/SalesUser/Dashboard/GetSalesInvoiceDashboard";
    public GetInvoicesForDashboard(type: number): Observable<any> {
        return this.httpClient.get<any>(`${this.GetSalesUserInvoiceUrl}?InvoiceTypeId=${type}`)
            .pipe(catchError(this.handleError<any>('GetInvoicesForDashboard', null)));
    }
    private GetValidTPOCompanyUrl = "Validation/IsValidTpoCompany";
    public GetValidTPOCompany(companyId : number): Observable<any> {
        return this.httpClient.get<any>(`${this.GetValidTPOCompanyUrl}?companyId=${companyId}`)
            .pipe(catchError(this.handleError<any>('GetValidTPOCompany', null)));
    }

    
    //Sales ORder DR
    public ValidateDREntryForm(data: any): Observable<ValidateDREntryResponse> {
      return this.httpClient.post<any>(this.UrlGetValidateDREntryFormUrl, data)
        .pipe(catchError(this.handleError<any>('ValidateDREntryForm', null)));
    }

    public CreateDREntryForm(data: DeliveryRequestInputModel[]): Observable<any> {
      return this.httpClient.post<any>(this.UrlGetCreateDREntryForm, data)
        .pipe(catchError(this.handleError<any>('CreateDREntryForm', null)));
    }

    public GetJobDetails2(jobName:string,companyName:string ): Observable<any> {
      return this.httpClient.get<any>( `${this.UrlGetJobDetailsFrom}?jobName=${jobName}&companyName=${companyName}`)
        .pipe(catchError(this.handleError<any>('GetJobDetails2', null)));
    }

    public GetCustomersAndLocations(): Observable<CustomersModel> {
      return this.httpClient.get<CustomersModel>( `${this.UrlGetAllBuyersForAllRegions}`)
      .pipe(catchError(this.handleError<CustomersModel>('GetCustomersAndLocations', null)));
    }

    
  public GetProducts(CompanyId:number,jobId:number): Observable<any> {
    return this.httpClient.get<any>( `${this.UrlGetProductsForSalesDR}?CompanyId=${CompanyId}&JobId=${jobId}`)
      .pipe(catchError(this.handleError<any>('GetJobDetails', null)));
  }
    //Sales ORder DR End
    
}
