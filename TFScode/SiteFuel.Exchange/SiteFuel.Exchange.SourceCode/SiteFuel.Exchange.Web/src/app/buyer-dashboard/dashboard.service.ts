import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, timer } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { HandleError } from 'src/app/errors/HandleError';
import { BuyerLoadsForDashboardInputModel, DashboardTileViewModel, InventoryForDashboardInputModel, InvoiceGridBuyerDashboardInputModel } from './Model/DashboardModel';
@Injectable({
  providedIn: 'root'
})
export class DashboardService extends HandleError {
  private getJobDetailsForBuyerDashboardUrl = "/Buyer/Dashboard/GetJobDetailsForBuyerDashboard";
  private GetBuyerLoadsForDashboard = "/Buyer/Dashboard/GetBuyerLoadsForDashboard"
  private GetLocationInventoryURl = "/Buyer/Dashboard/GetLocationInventory"
  private GetInvoiceGridForBuyerDashboardURL = "/Buyer/Dashboard/GetInvoiceGridForBuyerDashboardAsync";
  private GetNewBuyerDashboardTileSettingsURL = "/Buyer/Dashboard/GetNewBuyerDashboardTileSettings";
  private SaveDBTileSettingsURL = "/Buyer/Dashboard/SaveDBTileSettings";
  private GetMessagesForBuyerDashboardURL = "/Messages/Mailbox/GetMessagesForBuyerDashboard";
  constructor(private httpClient: HttpClient) { super(); }

  public getJobDetailsForBuyerDashboard(countryId: any): Observable<any> {
    return timer(0, 60 * 60 * 1000).pipe(
      switchMap(() => {
        //return this.httpClient.get<any>(`${this.getJobDetailsForBuyerDashboardUrl}`)
        return this.httpClient.get<any>(`${this.getJobDetailsForBuyerDashboardUrl}?countryId=${countryId}`)
      })).pipe(catchError(this.handleError<any>('getJobDetailsForBuyerDashboardUrl', null)));
  }

  public getDeliveries(input: BuyerLoadsForDashboardInputModel): Observable<any> {
    return this.httpClient.post<any>(this.GetBuyerLoadsForDashboard, input)
      .pipe(catchError(this.handleError<any>('getDeliveries', null)));
  }
  public GetLocationInventory(input: InventoryForDashboardInputModel): Observable<any> {
    return this.httpClient.get<any>(this.GetLocationInventoryURl + "?CountryId=" + input.CountryId)
      .pipe(catchError(this.handleError<any>('GetLocationInventory', null)));
  }

  public GetInvoices(input: InvoiceGridBuyerDashboardInputModel): Observable<any> {
    return this.httpClient.post<any>(this.GetInvoiceGridForBuyerDashboardURL, input)
      .pipe(catchError(this.handleError<any>('GetInvoiceGridForBuyerDashboardURL', null)));
  }

  public GetNewBuyerDashboardTileSettings(): Observable<any> {
    return this.httpClient.get<any>(this.GetNewBuyerDashboardTileSettingsURL)
      .pipe(catchError(this.handleError<any>('GetNewBuyerDashboardTileSettings', null)));
  }

  public SaveDBTileSettings(pageId: string, input: DashboardTileViewModel[]): Observable<any> {
    return this.httpClient.post<any>(this.SaveDBTileSettingsURL, { pageId: pageId, isMultipleTilesUpdated: true, settingsModel: input })
      .pipe(catchError(this.handleError<any>('SaveDBTileSettings', null)));
  }
  public GetMessages(): Observable<any> {
    return this.httpClient.get<any>(this.GetMessagesForBuyerDashboardURL)
      .pipe(catchError(this.handleError<any>('GetMessagesForBuyerDashboard', null)));
  }
}
