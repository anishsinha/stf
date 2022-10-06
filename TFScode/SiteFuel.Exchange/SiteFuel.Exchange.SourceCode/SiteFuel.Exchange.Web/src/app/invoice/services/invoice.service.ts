import { Injectable } from '@angular/core';
import { HandleError } from '../../errors/HandleError';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InvoiceDetailModel, AssetDropModel, DropDetailModel, FeeModel, MFNConversionRequestViewModel, BlendedSchedules } from '../models/DropDetail';
import { DropdownItem } from 'src/app/statelist.service';

@Injectable({
    providedIn: 'root'
})
export class InvoiceService extends HandleError {

    private getPoListUrl: string = '/Supplier/invoice/GetCustomerPoList?orderId=';
    private getDefaultUrl: string = '/Supplier/Invoice/GetInvoiceViewModel?orderId=';
    private getAssetstUrl: string = '/Supplier/Invoice/GetAssignedAssets?orderId=';
    private getAnotherProductUrl: string = '/Supplier/Invoice/GetInvoiceDropModel?orderId=';
    private getTerminalsUrl: string = '/Supplier/Invoice/GetTerminals?orderId=';
    private getTerminalPriceByIdUrl: string = '/Supplier/Invoice/GetTerminalPriceById';
    private postCreateInvoiceUrl: string = '/Supplier/Invoice/CreateNew';
    private postConvertToInvoiceUrl: string = '/Supplier/Invoice/ConvertToInvoice?ddtId=';
    private getInvoiceDropFeesUrl = '/Supplier/invoice/GetInvoiceDropFees?orderId=';
    private getSchedulesUrl = '/Supplier/invoice/GetDeliverySchedules?orderId=';
    private getInvoiceDetailsUrl = '/Supplier/Invoice/GetOriginalInvoiceDetails?invoiceId=';
    private getTaxePricingTypesUrl = '/Supplier/Invoice/GetTaxePricingTypes?orderId=';
    private getDriverListUrl = '/Supplier/Invoice/GetAllDrivers';
    private getsastokenurl: string = '/Supplier/Invoice/GetSasToken';
    private getAssignedurl: string = '/Supplier/Invoice/GetAssignedDriver?scheduleId=';
    private getCalculatedDropqtysUrl: string = '/Supplier/Invoice/CalculateDropQuantitiesFromPrePostForCreateInvoice';
    private validateGravityUrl: string = '/Supplier/Invoice/ValidateGravityAndConvertForMFN';
    private getEiaPriceUrl = '/Supplier/Invoice/GetEIAPrice';
    private getEiaPriceAutoFreightMethodUrl = 'Supplier/Invoice/GetEIAPriceForAutoFreightMethod';
    private getBlendedProductsUrl = '/Supplier/Invoice/GetBlendedProducts?blendGroupId=';
    private getFreightTableTypesUrl = '/FreightRate/GetFreightRateRuleTypes';
    public getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
    public getFreightTableNameUrl = '/Freight/GetFreightRateTablesForInvoice';
    public getFuelSurchargeTableNameUrl = 'FuelSurcharge/GetFuelSurchargeTablesForInvoice';
    public getAccessorialTableNameUrl = 'FuelSurcharge/GetAccessorialFeeTablesForInvoice';
    public GetAccessorialFeeTablesForConsolidatedUrl = 'FuelSurcharge/GetAccessorialFeeTablesForConsolidated';
    public GetAccessorialFeeTablesForSelectedOrderUrl = 'FuelSurcharge/GetAccessorialFeeTablesForSelectedOrder';
    public getAccessorialFeeByOrderUrl = 'Supplier/Invoice/GetAccessorialFeeByOrder';
    public getAccessorialFeeByAccessorialFeeIdUrl = 'Supplier/Invoice/GetAccessorialFeeByAccessorialFeeId';
    public GetFreightCostForAutoInvoiceUrl = 'FuelSurcharge/GetFreightCostForInvoice';
    constructor(private httpClient: HttpClient) {
        super();
    }

    getDefaultDetail(_orderId: number, trackableScheduleId: number): Observable<InvoiceDetailModel> {
        return this.httpClient.get<InvoiceDetailModel>(this.getDefaultUrl + _orderId + '&trackableScheduleId=' + trackableScheduleId)
            .pipe(catchError(this.handleError<InvoiceDetailModel>('getDefaultDetail', null)));
    }

    getPoList(orderId: number): Observable<any> {
        return this.httpClient.get(this.getPoListUrl + orderId)
            .pipe(catchError(this.handleError<any>('getPoList', null)));

    }

    getAssets(orderId: number): Observable<AssetDropModel[]> {
        return this.httpClient.get<AssetDropModel[]>(this.getAssetstUrl + orderId)
            .pipe(catchError(this.handleError<AssetDropModel[]>('getAssets', [])));
    }

    getAnotherProductDetail(_orderId: number): Observable<DropDetailModel> {
        return this.httpClient.get<DropDetailModel>(this.getAnotherProductUrl + _orderId)
            .pipe(catchError(this.handleError<DropDetailModel>('getAnotherProductDetail', null)));
    }

    getTerminals(_orderId: number, _terminal?:string): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getTerminalsUrl + _orderId + '&terminal=' + _terminal)
            .pipe(catchError(this.handleError<DropdownItem[]>('getTerminals', null)));
    }

    getTerminalPriceById(terminalId: number, orderId: number, deliveryDate: string): Observable<any> {
        return this.httpClient.post(this.getTerminalPriceByIdUrl, { terminalId: terminalId, orderId: orderId, deliveryDate: deliveryDate })
            .pipe(catchError(this.handleError<any>('getTerminalPriceById')));
    }
    getInvoiceDropFees(_orderId: number): Observable<FeeModel[]> {
        return this.httpClient.get<FeeModel[]>(this.getInvoiceDropFeesUrl + _orderId)
            .pipe(catchError(this.handleError<FeeModel[]>('getInvoiceDropFees', null)));
    }

    getSchedules(_orderId: number): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getSchedulesUrl + _orderId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getSchedules', null)));
    }

    getSASforblobStorage(): Observable<any> {
        return this.httpClient.get(this.getsastokenurl);
    }

    postCreateInvoice(invoiceModel: any, existingId: number): Observable<any> {
        if (existingId > 0 && !invoiceModel.IsRebillInvoice) {
            return this.httpClient.post<any>(this.postConvertToInvoiceUrl + existingId, invoiceModel)
                .pipe(catchError(this.handleError<any>('postConvertToInvoice', null)));
        } else {
            return this.httpClient.post<any>(this.postCreateInvoiceUrl, invoiceModel)
                .pipe(catchError(this.handleError<any>('postCreateInvoice', null)));
        }
    }

    postConvertToInvoice(invoiceModel: any, ddtId: number): Observable<any> {
        return this.httpClient.post<any>(this.postConvertToInvoiceUrl + ddtId, invoiceModel)
            .pipe(catchError(this.handleError<any>('postConvertToInvoice', null)));
    }

    getInvoiceDetails(_invoiceId: number): Observable<InvoiceDetailModel> {
        return this.httpClient.get<InvoiceDetailModel>(this.getInvoiceDetailsUrl + _invoiceId)
            .pipe(catchError(this.handleError<InvoiceDetailModel>('getInvoiceDetails', null)));
    }
    getBlendedProducts(_blendGroupId: string): Observable<BlendedSchedules[]> {
        return this.httpClient.get<BlendedSchedules[]>(this.getBlendedProductsUrl + _blendGroupId)
            .pipe(catchError(this.handleError<BlendedSchedules[]>('getBlendedProducts', null)));
    }
    getTaxePricingTypes(_orderId: number): Observable<DropdownItem[]> {
        return this.httpClient.get<DropdownItem[]>(this.getTaxePricingTypesUrl +_orderId)
            .pipe(catchError(this.handleError<DropdownItem[]>('getTaxePricingTypes', null)));
    }
    getDriverList(): Observable<any> {
        return this.httpClient.get(this.getDriverListUrl).pipe(catchError(this.handleError<any>('getDriverList', null)));
    }
    getAssignedDriverForSchedule(_scheduleId: number, _orderId: number): Observable<any> {
        return this.httpClient.get<any>(this.getAssignedurl + _scheduleId + '&orderId=' + _orderId)
            .pipe(catchError(this.handleError<any>('getAssignedDriverForSchedule', null)));
    }

    postPrePostAssetsInfo(assetInfo: any): Observable<any> {
        return this.httpClient.post<any>(this.getCalculatedDropqtysUrl,assetInfo)
            .pipe(catchError(this.handleError<any>('postPrePostAssetsInfo', null)));
    }
    ValidateGravityAndConvertForMFN(conversionRequest: MFNConversionRequestViewModel): Observable<any> {
        return this.httpClient.post<any>(this.validateGravityUrl, conversionRequest)
            .pipe(catchError(this.handleError<any>('ValidateGravityAndConvertForMFN', null)));
    }
    getEiaPrice(data: any): Observable<any> {
        return this.httpClient.post(this.getEiaPriceUrl, data)
            .pipe(catchError(this.handleError<DropdownItem[]>('getEiaPrice', [])));
    }
    getEiaPriceAutoFreightMethod(data: any): Observable<any> {
        return this.httpClient.post(this.getEiaPriceAutoFreightMethodUrl, data)
            .pipe(catchError(this.handleError<DropdownItem[]>('getEiaPriceAutoFreightMethod', [])));
    }
    getFreightTable(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getFreightTableTypesUrl)
            .pipe(catchError(this.handleError<any>('getFreightTable', null)));
    }
    getTableTypes(): Observable<DropdownItem[]> {
        return this.httpClient.get<any>(this.getTableTypesUrl)
            .pipe(catchError(this.handleError<any>('GetTableTypes', null)));
    }
    getFreightTableName(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getFreightTableNameUrl, filter)
            .pipe(catchError(this.handleError<any>('getFreightTableName', null)));
    }
    getFuelSurchargeTableName(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getFuelSurchargeTableNameUrl, filter)
            .pipe(catchError(this.handleError<any>('getFuelSurchargeTableName', null)));
    }
    getAccessorialTableName(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.getAccessorialTableNameUrl, filter)
            .pipe(catchError(this.handleError<any>('getAccessorialTableName', null)));
    }
    GetAccessorialFeeTablesForConsolidated(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.GetAccessorialFeeTablesForConsolidatedUrl, filter)
            .pipe(catchError(this.handleError<any>('GetAccessorialFeeTablesForConsolidated', null)));
    }
    GetAccessorialFeeTablesForSelectedOrder(orderIds: any): Observable<any> {
        return this.httpClient.post<any>(this.GetAccessorialFeeTablesForSelectedOrderUrl, { orderIds: orderIds })
            .pipe(catchError(this.handleError<any>('GetAccessorialFeeTablesForSelectedOrder', null)));
    }
   
    GetAccessorialFeeByOrder(orderId: any): Observable<any> {
        return this.httpClient.post<any>(this.getAccessorialFeeByOrderUrl, { orderId: orderId })
            .pipe(catchError(this.handleError<any>('GetAccessorialFeeByOrder', null)));
    }

    GetAccessorialFeeByAccessorialFeeId(accessorialFeeId: any): Observable<any> {
        return this.httpClient.post<any>(this.getAccessorialFeeByAccessorialFeeIdUrl, { accessorialFeeId: accessorialFeeId })
            .pipe(catchError(this.handleError<any>('GetAccessorialFeeByAccessorialFeeId', null)));
    }

    GetFreightCostForAutoInvoice(filter: any): Observable<any> {
        return this.httpClient.post<any>(this.GetFreightCostForAutoInvoiceUrl , filter)
            .pipe(catchError(this.handleError<any>('GetFreightCostForAutoInvoice', null)));
    }
}
