import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LFBolEditModel, LFRecordGridModel, LFRecordsGridViewModel } from '../LiftFileModels';

@Injectable({
    providedIn: 'root'
})
export class LiftfiledashboardserviceService extends HandleError {
    //urls
    public urlGetLFRecordsGrid: string = 'Supplier/LiftFile/GetLiftFileRecordsScratchReport';
    public urlGetBolDetailsForResolve: string = 'Supplier/LiftFile/GetLFBolEditDetailsForSlider';
    public urlSaveBolDetailsForResolve: string = 'Supplier/LiftFile/SaveLFBolEditDetails';
    public urlAddRecordsForForceIgnoreProcessing: string = 'Supplier/LiftFile/AddRecordsAsIgnoreMatch';
    public urlAddUnmatchedRecordForReProcessing: string = 'Supplier/Exception/AddUnmatchedRecordForReProcessing';
    //SupplierBOLReport
    public urlGetLiftFileRecordsWithMissingTFXDeliveryDetails: string = 'Supplier/LiftFile/GetLiftFileRecordsWithMissingTFXDeliveryDetails';
    //carrier bol report
    public urlGetTFXDeliveryDetailsWithMissingLiftFileRecords: string = 'Supplier/LiftFile/GetTFXDeliveryDetailsWithMissingLiftFileRecords';

    private UrlGetLFValidation: string = '/Supplier/Exception/LFValidationGridWithFilter';
    private UrlGetLFCarrier: string = '/Supplier/LiftFile/GetLFVCarrierDropDwn';
    private UrlGetLFVRecordGrid: string = '/Supplier/Exception/LFRecordsGridForDashboard';
    private urlGetLFSearchRecordsByBolFileName: string = 'Supplier/LiftFile/LFRecordsGridByBolFileName?bol=';
    private UrlGetLFVAccrualReportGrid: string = 'Supplier/LiftFile/GetLFVAccrualReportGrid'

    private UrlGetLFVValidationStatsAndProductTypesDDL: string = 'Supplier/LiftFile/GetLFVValidationStatsAndProductTypesDDL';
    private UrlUpdateLiftFileRecord: string = 'Supplier/LiftFile/UpdateLiftFileRecord';
    private urlGetReasonDescriptionList: string = 'Supplier/LiftFile/GetReasonDescriptionList';
    private urlGetPreferencesSetting: string = 'Settings/Profile/GetPreferencesSettingAsync';
    constructor(private httpClient: HttpClient) {
        super();
    }
    getLFRecords(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetLFRecordsGrid)
            .pipe(catchError(this.handleError<any>('getLFRecords', null)));
    }
    getBolDetailsForResolve(record: LFRecordGridModel): Observable<any> {
        return this.httpClient.post<LFRecordGridModel>(this.urlGetBolDetailsForResolve, record)
            .pipe(catchError(this.handleError<LFRecordGridModel>('getBolDetailsForResolve', record)));
    }
    saveBolDetailsForResolve(record: LFBolEditModel): Observable<any> {
        return this.httpClient.post<LFBolEditModel>(this.urlSaveBolDetailsForResolve, record)
            .pipe(catchError(this.handleError<LFBolEditModel>('saveBolDetailsForResolve', record)));
    }
    addRecordsForForcedIgnoreMatchProcessing(LfRecordIds: any[], descriptionId: number = 0, descriptionText: string = ''): Observable<any> {
        return this.httpClient.post<any>(this.urlAddRecordsForForceIgnoreProcessing + '?DescriptionId=' + descriptionId + '&DescriptionText=' + descriptionText, LfRecordIds)
            .pipe(catchError(this.handleError<any>('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
    }
    getLFValidationGrid(data): Observable<any> {
        return this.httpClient.post(this.UrlGetLFValidation, { startDate: data.fromDate, endDate: data.toDate, isCarrierPerFormanceDashboard: data.isCarrierPerFormanceDashboard, carrierIds: data.carrierIds, isMatchingWindow: data.isMatchingWindow })
            .pipe(catchError(this.handleError<any>('getLFValidationGrid', null)));
    }
    getLFCarrier(fromDate: any, toDate: any): Observable<any> {
        return this.httpClient.post<any>(this.UrlGetLFCarrier, { fromDate: fromDate, toDate: toDate })
            .pipe(catchError(this.handleError<any>('getLFCarrier', null)));
    }
    getSupplierBOLReport(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetLiftFileRecordsWithMissingTFXDeliveryDetails)
            .pipe(catchError(this.handleError<any>('getSupplierBOLReport', null)));
    }
    getCarrierBOLReport(fromDate: any, toDate: any): Observable<any> {
        return this.httpClient.post<any>(this.urlGetTFXDeliveryDetailsWithMissingLiftFileRecords, { fromDate: fromDate, toDate: toDate })
            .pipe(catchError(this.handleError<any>('getCarrierBOLReport', null)));
    }
    getLFVRecordGrid(data): Observable<any> {
        return this.httpClient.post<any>(this.UrlGetLFVRecordGrid, { recordStatus: data.recordStatus, startDate: data.fromDate, endDate: data.toDate, lfCallId: 0, isMatchingWindow: data.isMatchingWindow, carrierIds: data.carrierIds })
            // return this.httpClient.post(this.UrlGetLFVRecordGrid, {recordStatus:data.recordStatus, startDate: data.fromDate, endDate: data.toDate})
            .pipe(catchError(this.handleError<any>('getLFVRecordGrid', null)));
    }

    addUnmatchedRecordForReProcessing(LfRecordIds: any[]): Observable<any> {
        return this.httpClient.post<any>(this.urlAddUnmatchedRecordForReProcessing, LfRecordIds)
            .pipe(catchError(this.handleError<any>('addRecordsForForcedIgnoreMatchProcessing', LfRecordIds)));
    }
    getLFSearchRecords(bol: string, fileName: string): Observable<any> {
        return this.httpClient.get<any>(this.urlGetLFSearchRecordsByBolFileName + bol + '&fileName=' + fileName)
            .pipe(catchError(this.handleError<any>('getLFSearchRecords', null)));
    }
    getLFVAccrualReportGrid(data): Observable<any> {
        return this.httpClient.post<any>(this.UrlGetLFVAccrualReportGrid, data)
            .pipe(catchError(this.handleError<any>('getLFVAccrualReportGrid', null)));
    }
    GetLFVValidationStatsAndProductTypesDDL(data): Observable<any> {
        return this.httpClient.post<any>(this.UrlGetLFVValidationStatsAndProductTypesDDL, data)
            .pipe(catchError(this.handleError<any>('GetLFVValidationStatsAndProductTypesDDL', null)));
    }
    updateLiftFileRecord(data: LFRecordGridModel): Observable<any>{
        return this.httpClient.post<any>(this.UrlUpdateLiftFileRecord, data)
            .pipe(catchError(this.handleError<any>('updateLiftFileRecord', null)));
    }
    GetReasonDescriptionList(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetReasonDescriptionList)
            .pipe(catchError(this.handleError<any>('GetReasonDescriptionList', null)));
    }
    getPreferencesSetting(): Observable<any> {
        return this.httpClient.get<any>(this.urlGetPreferencesSetting)
            .pipe(catchError(this.handleError<any>('getPreferencesSettingAsync', null)));
    }
}
