import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError } from '../../app/errors/HandleError';
import { CarrierDetailsModel, ThirdPartyCompanyFilter } from './third-party-network.model';

@Injectable({
  providedIn: 'root'
})
export class ThirdPartyNetworkService extends HandleError {

  private urlGetInvitationTokenByCompany = "Supplier/ThirdPartyNetwork/GetInvitationTokenByCompany";
  private urlGenerateInvitationToken = "Supplier/ThirdPartyNetwork/GenerateInvitationToken";
  private urlGetNonRegisteredInvitedCompanies = "Supplier/ThirdPartyNetwork/GetNonRegisteredInvitedCompanies";
  private urlGetRegisteredInvitedCompanies = "Supplier/ThirdPartyNetwork/GetRegisteredInvitedCompanies";
  private urlGetNonRegisteredInvitedCompany = "Supplier/ThirdPartyNetwork/GetNonRegisteredInvitedCompany";
  private urlGetRegisteredInvitedCompany = "Supplier/ThirdPartyNetwork/GetRegisteredInvitedCompany";

  constructor(private httpClient: HttpClient) { super(); }

  public GetInvitationTokenByCompany(): Observable<any> {
    return this.httpClient.get<any>(this.urlGetInvitationTokenByCompany).pipe(catchError(this.handleError<any>('GetInvitationTokenByCompany', null)));
  }

  public GenerateInvitationToken(): Observable<any> {
    return this.httpClient.get<any>(this.urlGenerateInvitationToken).pipe(catchError(this.handleError<any>('GenerateInvitationToken', null)));
  }

  public GetNonRegisteredInvitedCompanies(input?: ThirdPartyCompanyFilter): Observable<CarrierDetailsModel[]> {
    return this.httpClient.post<any>(this.urlGetNonRegisteredInvitedCompanies, (input ? input : null)).pipe(catchError(this.handleError<any>('GetNonRegisteredInvitedCompanies', null)));
  }

    public GetRegisteredInvitedCompanies(input?: ThirdPartyCompanyFilter): Observable<CarrierDetailsModel[]> {
        return this.httpClient.post<any>(this.urlGetRegisteredInvitedCompanies, (input ? input : null)).pipe(catchError(this.handleError<any>('GetRegisteredInvitedCompanies', null)));
  }

  public GetNonRegisteredInvitedCompany(entityId: number): Observable<any> {
    return this.httpClient.get<any>(this.urlGetNonRegisteredInvitedCompany + "?entityId=" + entityId).pipe(catchError(this.handleError<any>('GetNonRegisteredInvitedCompanies', null)));
  }

  public GetRegisteredInvitedCompany(companyId: number): Observable<any> {
    return this.httpClient.get<any>(this.urlGetRegisteredInvitedCompany + "?companyId=" + companyId).pipe(catchError(this.handleError<any>('GetRegisteredInvitedCompanies', null)));
  }
}
