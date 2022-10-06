import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HandleError } from 'src/app/errors/HandleError';
import { DropdownItemExt } from 'src/app/statelist.service';
import { RegionDetailModel, ScheduleBuilderModel, OrderPickupDetailModel, CompanyUsers, DemandModel, PreLoadDrViewModel, PreLoadDrResponseViewModel, UnAssignDriverFromShiftModel, DeliveryRequestBrokerInfoViewModel, TrailerCompartment, ShiftViewModel, OttoTripModel, OttoNotifications, OttoBuilder, OrderFuelInfo, OptionalPickupDetailModel, JobDetailsWithOrders, SubDRStatus } from '../models/DispatchSchedulerModels';
import { DeliveryRequestViewModel } from '../../buyer-wally-board/Models/BuyerWallyBoard';

@Injectable({
    providedIn: 'root'
})
export class ScheduleBuilderService extends HandleError {

    private getRegionListUrl = '/Carrier/ScheduleBuilder/GetRegions';
    private getRegionDetailsUrl = '/Carrier/ScheduleBuilder/GetRegionDetails?regionId=';
    private getGetSheduleBuilderUrl = '/Carrier/ScheduleBuilder/GetSheduleBuilder?regionId=';
    private getOrdersUrl = '/Carrier/ScheduleBuilder/GetOrderDetailsForEditDeliveryGroup?jobId=';
    private getPickupTerminalsUrl = '/Carrier/ScheduleBuilder/GetTerminalsForOrders';
    private getOrderBadgesByTerminalUrl = '/Carrier/ScheduleBuilder/GetOrderBadgesByTerminal';
    private saveDriverViewUrl = '/Carrier/ScheduleBuilder/SaveDriverView';
    private getDeliveryReqDemandsUrl = '/Carrier/ScheduleBuilder/GetDeliveryReqDemands';
    private publishDriverViewUrl = '/Carrier/ScheduleBuilder/PublishDriverView';
    private saveTrailerViewUrl = '/Carrier/ScheduleBuilder/SaveTrailerView';
    private publishTrailerViewUrl = '/Carrier/ScheduleBuilder/PublishTrailerView';
    private deleteGroupUrl = '/Carrier/ScheduleBuilder/DeleteLoadDriverView';
    private deleteGroupTrailerViewUrl = '/Carrier/ScheduleBuilder/DeleteLoadTrailerView';
    private urlGetCompanyDrivers = 'Carrier/ScheduleBuilder/GetCompanyDrivers';
    private urlAssignDriverAndTrailer = 'Carrier/ScheduleBuilder/AssignDriverAndTrailer';
    private checkDelReqStatusUrl = 'Carrier/ScheduleBuilder/CheckDelReqStatus?delReqId=';
    private getScheduleStatusUrl = 'Carrier/ScheduleBuilder/GetScheduleStatus';
    private getRegionDispatcherDetails = '/Carrier/ScheduleBuilder/GetRegionDispactherDetails?regionId=';
    private getSelectedDateDriverScheduleByDriverIdUrl = '/Carrier/ScheduleBuilder/getSelectedDateDriverScheduleByDriverId?driverId=';
    private getSelectedDateDriverScheduleByDriverIdGridViewUrl = '/Carrier/ScheduleBuilder/getSelectedDateDriverScheduleByDriverIdGridView?driverId=';
    private validateTrailerJobCompatibilityUrl = '/Carrier/ScheduleBuilder/ValidateTrailerJobCompatibility';
    private validateFilldOrderCompatibilityUrl = '/Carrier/ScheduleBuilder/IsFilldCompatibleOrder?orderId=';
    private getCustomerLocationProductUrl = '/Carrier/ScheduleBuilder/GetCustomerLocationProducts?jobId=';
    private reCreateDeliveryRequestsUrl = '/Carrier/ScheduleBuilder/ReCreateDeliveryRequests';
    private cloneDrsForPreloadUrl = '/Carrier/ScheduleBuilder/CloneDrsForPreload';
    private CreatePreloadForAcrossTheDateUrl = '/Carrier/ScheduleBuilder/CreatePreloadForAcrossTheDate';
    private postUrlUnAssignDriverFromShift = '/Carrier/ScheduleBuilder/UnAssignDriverFromShift';
    private getAssignCarrierDetailsUrl = '/Carrier/ScheduleBuilder/GetAssignCarrierDetails?regionId=';
    private getOrdersByDeliveryRequestsUrl = '/Carrier/ScheduleBuilder/GetOrdersByDeliveryRequests';
    private BrokerDeliveryRequestToCarrierUrl = '/Carrier/ScheduleBuilder/BrokerDeliveryRequestToCarrier';
    private BrokerDeliveryRequestsToCarriersUrl = '/Carrier/ScheduleBuilder/BrokerDeliveryRequestsToCarriers';
    private getGetRegionsOfCompanyUrl = '/Carrier/ScheduleBuilder/GetRegionsOfCompany?companyId=';
    private recallDrFromCarrierUrl = '/Carrier/ScheduleBuilder/RecallDrFromCarrier';
    private getChildOrdersURL = '/Carrier/ScheduleBuilder/GetSupplierChildOrders?OrderId=';
    private getTrailerComaprtmentsURL = '/Carrier/ScheduleBuilder/GetTrailerCompartmentDetails';
    private getTrailerFuelRetainURL = '/Carrier/ScheduleBuilder/GetTrailerFuelRetainDetails';
    private getOttoSettingURL = '/Carrier/ScheduleBuilder/GetOttoSetting';
    private getOttoScheduleDetailsURL = '/Carrier/ScheduleBuilder/GetOttoScheduleDetails?regionId=';
    private GetShiftsURL = '/Carrier/ScheduleBuilder/GetShifts?regionId=';
    private getOttoNotificationURL = '/Carrier/ScheduleBuilder/GetOttoNotifications?regionId=';
    private getOttoNotificationCountURL = '/Carrier/ScheduleBuilder/GetDsbNotificationCount?regionId=';
    private updateOttoNotificationStatusURL = '/Carrier/ScheduleBuilder/UpdateDsbNotificationStatus';
    private scheduleOttoDRsURL = '/Carrier/ScheduleBuilder/ScheduleOttoDRs';
    private urlGetShiftCompanyDrivers = 'Carrier/ScheduleBuilder/GetShiftCompanyDrivers';
    private urlGetDriversShiftsURL = '/Carrier/ScheduleBuilder/GetDriversShiftsURL?regionId=';
    private getOptionalPickupTerminalsUrl = '/Carrier/ScheduleBuilder/GetTerminalsForOptionalPickup';
    private getOrderFuelTypeUrl = '/Carrier/ScheduleBuilder/GetOrderFuelType';
    private postUrladdOptionalPickup = '/Carrier/ScheduleBuilder/AddOptionalPickup';
    private postUrlgetOptionalPickupDetails = '/Carrier/ScheduleBuilder/GetOptionalPickupDetails';
    private postUrlDeleteOptionalPickupDetails = '/Carrier/ScheduleBuilder/DeleteOptionalPickupDetails';
    private getTBDPickupTerminalsUrl = '/Carrier/ScheduleBuilder/GetTBDTerminals';
    private getJobWithOrdersUrl = '/Carrier/ScheduleBuilder/GetJobWithOrders?regionId='
    private cancelDriverViewUrl = '/Carrier/ScheduleBuilder/CancelDriverSchedule';
    private cancelDeliveryGroupSchedule = '/Carrier/ScheduleBuilder/CancelDeliverySchedule';
    private getSubDRInfoCancelDS = '/Carrier/ScheduleBuilder/GetSubDRInfoCancelDS';
    private getSubDRsStatus = '/Carrier/ScheduleBuilder/getSubDrsStatus';
    private deleteRemoveDeliveryScheduleUrl = '/Carrier/ScheduleBuilder/RemoveDeliverySchedule';
    private urlGetJobCoordinates = '/Carrier/ScheduleBuilder/GetJobCoordinates';
    private urlPostValidateDRMaxFill = '/Carrier/ScheduleBuilder/ValidateTankMaxFill';
    constructor(private httpClient: HttpClient) {
        super();
    }

    getRegions(): Observable<DropdownItemExt[]> {
        return this.httpClient.get<DropdownItemExt[]>(this.getRegionListUrl)
            .pipe(catchError(this.handleError<any>('getRegions', null)));
    }

    getRegionsOfCompany(companyId: any): Observable<DropdownItemExt[]> {
        return this.httpClient.get<DropdownItemExt[]>(this.getGetRegionsOfCompanyUrl + companyId)
            .pipe(catchError(this.handleError<any>('getRegionsOfCompany', null)));
    }

    getRegionDetails(regionId: string): Observable<RegionDetailModel> {
        return this.httpClient.get<RegionDetailModel>(this.getRegionDetailsUrl + regionId)
            .pipe(catchError(this.handleError<RegionDetailModel>('getRegionDetails', null)));
    }

    getScheduleBuilder(regionId: string, date: string, sbViewId: number, sbDsbView: number): Observable<ScheduleBuilderModel> {
        return this.httpClient.get<ScheduleBuilderModel>(this.getGetSheduleBuilderUrl + regionId + '&date=' + date + '&sbView=' + sbViewId + '&sbDsbView=' + sbDsbView)
            .pipe(catchError(this.handleError<ScheduleBuilderModel>('getScheduleBuilder', null)));
    }

    getOrders(jobId: number, productTypeId: string, startDate: string, carrierStatus: number = -1, isBlendReq: boolean = false): Observable<OrderPickupDetailModel[]> {
        var url = this.getOrdersUrl + jobId + '&productTypeId=' + productTypeId + '&carrierStatus=' + carrierStatus + '&isBlendReq=' + isBlendReq;
        if (startDate != null) {
            url = url + '&startDate=' + startDate;
        }
        return this.httpClient.get<OrderPickupDetailModel[]>(url)
            .pipe(catchError(this.handleError<OrderPickupDetailModel[]>('getOrders', null)));
    }

    getJobDetailsWithOrders(selectedRegionId: string, tfxProductId: number, productTypeId: number, terminalId: number, bulkplantId: number, startDate: string): Observable<JobDetailsWithOrders[]> {
        var url = this.getJobWithOrdersUrl + selectedRegionId + '&tfxProductId=' + tfxProductId + '&productTypeId=' + productTypeId + '&terminalId=' + terminalId + '&bulkplantId=' + bulkplantId;
        if (startDate != null) {
            url = url + '&startDate=' + startDate;
        }
        return this.httpClient.get<JobDetailsWithOrders[]>(url)
            .pipe(catchError(this.handleError<JobDetailsWithOrders[]>('getJobDetailsWithOrders', null)));
    }

    getPickupTerminals(orders: number[], terminalName = ''): Observable<any> {
        var data = { OrderList: orders, Terminal: terminalName };
        return this.httpClient.post(this.getPickupTerminalsUrl, data);
    }

    getOrderBadgesByTerminal(_orderIds, _pickupLocationType, _pickupLocationId): Observable<any> {
        var model = { OrderIds: _orderIds, PickupLocationType: _pickupLocationType, PickupLocationId: _pickupLocationId };
        return this.httpClient.post(this.getOrderBadgesByTerminalUrl, model).pipe(catchError(this.handleError<any>('getOrderBadgesByTerminal', null)));
    }

    saveDriverView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.saveDriverViewUrl, sbModel).pipe(catchError(this.handleError<any>('saveDriverView', null)));
    }

    getDeliveryReqDemands(input: any): Observable<any> {
        return this.httpClient.post(this.getDeliveryReqDemandsUrl, input).pipe(catchError(this.handleError<any>('getDeliveryReqDemands', null)));
    }

    publishDriverView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.publishDriverViewUrl, sbModel).pipe(catchError(this.handleError<any>('publishDriverView', null)));
    }

    saveTrailerView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.saveTrailerViewUrl, sbModel).pipe(catchError(this.handleError<any>('saveTrailerView', null)));
    }

    publishTrailerView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.publishTrailerViewUrl, sbModel).pipe(catchError(this.handleError<any>('publishTrailerView', null)));
    }

    assignDriverAndTrailer(sbModel: any): Observable<any> {
        return this.httpClient.post(this.urlAssignDriverAndTrailer, sbModel);
    }

    deleteGroup(sbModel: any): Observable<any> {
        return this.httpClient.post(this.deleteGroupUrl, sbModel).pipe(catchError(this.handleError<any>('deleteGroup', null)));
    }

    deleteGroupTrailerView(sbModel: any): Observable<any> {
        return this.httpClient.post(this.deleteGroupTrailerViewUrl, sbModel).pipe(catchError(this.handleError<any>('deleteGroupTrailerView', null)));
    }

    checkDelReqStatus(selectedDate: string, selectedRegionId: string, currentTimeStamp: any, delReqId: string): Observable<any> {
        return this.httpClient.get<any>(this.checkDelReqStatusUrl + delReqId + '&selectedDate=' + selectedDate + '&selectedRegionId=' + selectedRegionId + '&currentTimeStamp=' + currentTimeStamp)
            .pipe(catchError(this.handleError<any>('checkDelReqStatus', null)));
    }

    getSubDRStatus(groupDRParentIds: SubDRStatus[]): Observable<any> {
        return this.httpClient.post<any>(this.getSubDRsStatus, groupDRParentIds).pipe(catchError(this.handleError<any>('getSubDRStatus', null)));
    }

    getScheduleStatus(trackableScheduleIds: number[]): Observable<any> {
        return this.httpClient.post<any>(this.getScheduleStatusUrl, trackableScheduleIds).pipe(catchError(this.handleError<any>('getScheduleStatus', null)));
    }

    getCompanyDrivers(trailerIds, regionIds, selectedDate): Observable<any> {
        var data = { trailerId: trailerIds, regionId: regionIds, selectedDate: selectedDate };
        return this.httpClient.post<any>(this.urlGetCompanyDrivers, data)
            .pipe(catchError(this.handleError<any>('getCompanyDrivers', null)));
    }

    setConfirmationHeadingForDeleteGroup(enrouteStatus: number) {
        switch (enrouteStatus) {
            case 1:
            case 3:
            case 9:
                jQuery('#deleteDrHeading').html('Driver has started to fuel drop location for one or more schedule(s).');
                break;
            case 11:
                jQuery('#deleteDrHeading').html('Driver is on the way to fuel pickup location for one or more schedule(s).');
                break;
            case 12:
                jQuery('#deleteDrHeading').html('Driver has arrived at fuel pickup location for one or more schedule(s).');
                break;
            case 17:
                jQuery('#deleteDrHeading').html('Driver is waiting before fuel pickup location for one or more schedule(s).');
                break;
            case 15:
                jQuery('#deleteDrHeading').html('Driver has started picking up fuel for one or more schedule(s).');
                break;
            case 16:
                jQuery('#deleteDrHeading').html('Driver has already picked fuel for one or more schedule(s).');
                break;
            case 13:
                jQuery('#deleteDrHeading').html('Driver is waiting before fuel drop location for one or more schedule(s).');
                break;
            case 18:
                jQuery('#deleteDrHeading').html('Driver has arrived at fuel drop location for one or more schedule(s).');
                break;
            default:
                jQuery('#deleteDrHeading').html('Driver is on the way to fuel drop location for one or more schedule(s).');
                break;
        }
    }

    setConfirmationHeadingForDR(enrouteStatus: number) {
        switch (enrouteStatus) {
            case 1:
            case 3:
            case 9:
                jQuery('#deleteDrHeading').html('Driver has started to fuel drop location.');
                break;
            case 11:
                jQuery('#deleteDrHeading').html('Driver is on the way to fuel pickup location.');
                break;
            case 12:
                jQuery('#deleteDrHeading').html('Driver has arrived at fuel pickup location.');
                break;
            case 17:
                jQuery('#deleteDrHeading').html('Driver is waiting before fuel pickup location.');
                break;
            case 15:
                jQuery('#deleteDrHeading').html('Driver has started picking up fuel.');
                break;
            case 16:
                jQuery('#deleteDrHeading').html('Driver has already picked fuel.');
                break;
            case 13:
                jQuery('#deleteDrHeading').html('Driver is waiting before fuel drop location.');
                break;
            case 18:
                jQuery('#deleteDrHeading').html('Driver has arrived at fuel drop location.');
                break;
            default:
                jQuery('#deleteDrHeading').html('Driver is on the way to fuel drop location.');
                break;
        }
    }

    returnCommonElements(array1, list: any, isScheduleStatus: boolean) {
        if (isScheduleStatus) {
            return list.filter(i =>
                array1.indexOf(i.ScheduleStatusId) != -1
            ).map(t => t.ScheduleStatusId);
        }
        else {
            return list.filter(i =>
                array1.indexOf(i.ScheduleEnrouteStatusId) != -1).map(t => t.ScheduleEnrouteStatusId);
        }
    }

    returnCommonTracableElements(StatusArray, list: any, isTrackableStatus: boolean) {
        if (isTrackableStatus) {
            return list.filter(i =>
                StatusArray.indexOf(i.TrackScheduleStatus) != -1
            ).map(t => t.TrackScheduleStatus);
        }
        else {
            return list.filter(i =>
                StatusArray.indexOf(i.TrackScheduleStatus) != -1).map(t => t.TrackScheduleStatus);
        }
    }

    returnSubDrStatusOtherthaCancel(StatusArray: any, list: any) {
        return list.filter(i =>
            StatusArray.indexOf(i.DeliveryScheduleStatusId) != -1
        ).map(t => t.DeliveryScheduleStatusId);

    }

    getRegionDispacther(regionId, driverId): Observable<CompanyUsers[]> {
        return this.httpClient.get<CompanyUsers[]>(this.getRegionDispatcherDetails + regionId + '&driverId=' + driverId)
            .pipe(catchError(this.handleError<any>('getCompanyUsersDetails', null)));
    }

    getSelectedDateDriverScheduleByDriverId(driverId, selectedDate): Observable<any> {
        return this.httpClient.get<any>(this.getSelectedDateDriverScheduleByDriverIdUrl + driverId + "&selectedDate=" + selectedDate)
            .pipe(catchError(this.handleError<any>('getSelectedDateDriverScheduleByDriverId', null)));
    }

    validateTrailerJobCompatibility(trailers: any, deliveryRequests: any): Observable<any> {
        var data = { trailers: trailers, deliveryRequests: deliveryRequests };
        return this.httpClient.post(this.validateTrailerJobCompatibilityUrl, data).pipe(catchError(this.handleError<any>('validateTrailerJobCompatibility', null)));
    }

    validateFilldOrderCompatibility(orderIds: any): Observable<any> {
        var data = { orderIds: orderIds };
        return this.httpClient.post(this.validateFilldOrderCompatibilityUrl, data).pipe(catchError(this.handleError<any>('validateFilldOrderCompatibility', null)));
    }

    getCustomerLocationDemands(jobId: number, regionId: string): Observable<DemandModel[]> {
        return this.httpClient.get<any>(this.getCustomerLocationProductUrl + jobId + "&regionId=" + regionId)
            .pipe(catchError(this.handleError<any>('getCustomerLocationProducts', null)));
    }

    reCreateDeliveryRequests(inputData: any): Observable<any> {
        return this.httpClient.post(this.reCreateDeliveryRequestsUrl, inputData)
            .pipe(catchError(this.handleError<any>('reCreateDeliveryRequests', null)));
    }

    cloneDrsForPreload(drIds: any): Observable<any> {
        return this.httpClient.post(this.cloneDrsForPreloadUrl, drIds)
            .pipe(catchError(this.handleError<any>('cloneDrsForPreload', null)));
    }

    createPreloadForAcrossTheDate(data: PreLoadDrViewModel): Observable<PreLoadDrResponseViewModel> {
        return this.httpClient.post(this.CreatePreloadForAcrossTheDateUrl, data)
            .pipe(catchError(this.handleError<any>('CreatePreloadForAcrossTheDate', null)));
    }

    UnAssignDriverTrailerFromShift(_data: UnAssignDriverFromShiftModel): Observable<any> {
        return this.httpClient.post(this.postUrlUnAssignDriverFromShift, _data).pipe(catchError(this.handleError<any>('UnAssignDriverFromShift', null)));
    }
    getAssignCarrierDetails(regionId: string, JobId: number, FuelTypeId: number): Observable<any> {
        return this.httpClient.get<any>(this.getAssignCarrierDetailsUrl + regionId + "&jobId=" + JobId + "&fuelTypeId=" + FuelTypeId)
            .pipe(catchError(this.handleError<any>('getAssignCarrierDetails', null)));
    }

    getOrdersByDeliveryRequests(data: any[]): Observable<any> {
        return this.httpClient.post<any>(this.getOrdersByDeliveryRequestsUrl, data)
            .pipe(catchError(this.handleError<any>('getOrdersByDeliveryRequests', null)));
    }

    BrokerDeliveryRequestToCarrier(_data: DeliveryRequestBrokerInfoViewModel): Observable<any> {
        return this.httpClient.post(this.BrokerDeliveryRequestToCarrierUrl, _data)
            .pipe(catchError(this.handleError<any>('BrokerDeliveryRequestToCarrier', null)));
    }

    BrokerDeliveryRequestsToCarriers(_data: DeliveryRequestBrokerInfoViewModel): Observable<any> {
        return this.httpClient.post(this.BrokerDeliveryRequestsToCarriersUrl, _data)
            .pipe(catchError(this.handleError<any>('BrokerDeliveryRequestsToCarriers', null)));
    }

    recallDrFromCarrier(_data: any): Observable<any> {
        return this.httpClient.post(this.recallDrFromCarrierUrl, _data)
            .pipe(catchError(this.handleError<any>('recallDrFromCarrier', null)));
    }

    getChildOrderDetails(OrderId: any): Observable<DropdownItemExt[]> {
        return this.httpClient.get<DropdownItemExt[]>(this.getChildOrdersURL + OrderId)
            .pipe(catchError(this.handleError<any>('getChildOrderDetails', null)));
    }

    getTrailerCompartments(data: any[]): Observable<TrailerCompartment[]> {
        return this.httpClient.post(this.getTrailerComaprtmentsURL, data)
            .pipe(catchError(this.handleError<any>('getTrailerCompartments', null)));
    }
    getTrailerFuelRetain(data: any[]): Observable<any> {
        return this.httpClient.post(this.getTrailerFuelRetainURL, data)
            .pipe(catchError(this.handleError<any>('getTrailerFuelRetain', null)));
    }
    getOttoSetting(): Observable<any> {
        return this.httpClient.get<any>(this.getOttoSettingURL)
            .pipe(catchError(this.handleError<any>('getOttoSetting', null)));
    }
    getOttoScheduleDetails(regionId: string, shiftStartTime: string, shiftEndTime: string, date: string): Observable<OttoTripModel[]> {
        return this.httpClient.get<OttoTripModel[]>(this.getOttoScheduleDetailsURL + regionId + '&shiftStartTime=' + shiftStartTime + '&shiftEndTime=' + shiftEndTime + '&date=' + date)
            .pipe(catchError(this.handleError<OttoTripModel[]>('getOttoScheduleDetails', null)));
    }
    getShifts(regionid): Observable<ShiftViewModel[]> {
        return this.httpClient.get<ShiftViewModel[]>(this.GetShiftsURL + regionid)
            .pipe(catchError(this.handleError<ShiftViewModel[]>('getShifts', null)));
    }
    getOttoNotificationDetails(regionId: string): Observable<OttoNotifications[]> {
        return this.httpClient.get<OttoNotifications[]>(this.getOttoNotificationURL + regionId)
            .pipe(catchError(this.handleError<OttoNotifications[]>('getOttoNotificationDetails', null)));
    }
    getOttoNotificationCount(regionId: string): Observable<any> {
        return this.httpClient.get<any>(this.getOttoNotificationCountURL + regionId)
            .pipe(catchError(this.handleError<any>('getOttoNotificationCount', null)));
    }
    updateNotificationStatus(data: any): Observable<any> {
        var response = { Id: data };
        return this.httpClient.post(this.updateOttoNotificationStatusURL, response)
            .pipe(catchError(this.handleError<any>('updateNotificationStatus', null)));
    }
    scheduleOttoDRs(ottoBuilder: OttoBuilder): Observable<any> {
        return this.httpClient.post(this.scheduleOttoDRsURL, ottoBuilder)
            .pipe(catchError(this.handleError<any>('scheduleOttoDRs', null)));
    }
    getShiftCompanyDrivers(regionIds, otherRegion, selectedDate, shiftId): Observable<any> {
        var data = { regionId: regionIds, otherRegion: otherRegion, selectedDate: selectedDate, shiftId: shiftId };
        return this.httpClient.post<any>(this.urlGetShiftCompanyDrivers, data)
            .pipe(catchError(this.handleError<any>('getShiftCompanyDrivers', null)));
    }
    getDriversShifts(regionid, date: string): Observable<ShiftViewModel[]> {
        return this.httpClient.get<ShiftViewModel[]>(this.urlGetDriversShiftsURL + regionid + '&SelectedDate=' + date)
            .pipe(catchError(this.handleError<ShiftViewModel[]>('getDriversShifts', null)));
    }
    getSelectedDateDriverScheduleByDriverIdGridView(driverId, selectedDate, shiftId): Observable<any> {
        return this.httpClient.get<any>(this.getSelectedDateDriverScheduleByDriverIdGridViewUrl + driverId + "&selectedDate=" + selectedDate + "&shiftId=" + shiftId)
            .pipe(catchError(this.handleError<any>('getSelectedDateDriverScheduleByDriverId', null)));
    }
    getOptioanlPickupTerminals(orders: number[], fuelType: number[], terminalName = ''): Observable<any> {
        var data = { OrderList: orders, FuelTypeId: fuelType, Terminal: terminalName };
        return this.httpClient.post(this.getOptionalPickupTerminalsUrl, data);
    }
    getOrderFuelTypes(orders: number[]): Observable<OrderFuelInfo> {
        var data = { OrderList: orders };
        return this.httpClient.post<OrderFuelInfo>(this.getOrderFuelTypeUrl, data);
    }
    addOptionalPickup(OptionalPickupDetails: OptionalPickupDetailModel[]) {
        var data = { OptionalPickupDetails };
        return this.httpClient.post<any>(this.postUrladdOptionalPickup, data);
    }

    getOptionalPickup(OptionalPickupDetails: OptionalPickupDetailModel) {
        var data = { OptionalPickupDetails };
        return this.httpClient.post<OptionalPickupDetailModel[]>(this.postUrlgetOptionalPickupDetails, data);
    }
    deleteOptionalPickup(opId: string, DriverId: number) {
        var data = { Id: opId, driverId: DriverId };
        return this.httpClient.post<any>(this.postUrlDeleteOptionalPickupDetails, data);
    }
    getTBDPickupTerminals(terminalName = ''): Observable<any> {
        var data = { Terminal: terminalName };
        return this.httpClient.post(this.getTBDPickupTerminalsUrl, data);
    }
    CancelDriverSchedule(sbModel: any): Observable<any> {
        return this.httpClient.post(this.cancelDriverViewUrl, sbModel).pipe(catchError(this.handleError<any>('cancelDriverSchedule', null)));
    }
    CancelDeliveryGroupSchedule(CancelDeliverySchedule: any): Observable<any> {
        return this.httpClient.post(this.cancelDeliveryGroupSchedule, CancelDeliverySchedule).pipe(catchError(this.handleError<any>('CancelDeliveryGroupSchedule', null)));
    }
    GetSubDRInfoCancelDS(inputModel: any): Observable<any> {
        return this.httpClient.post(this.getSubDRInfoCancelDS, inputModel).pipe(catchError(this.handleError<any>('GetSubDRInfoCancelDS', null)));
    }
    deleteDeliverySchedule(sbModel: any): Observable<any> {
        return this.httpClient.post(this.deleteRemoveDeliveryScheduleUrl, sbModel).pipe(catchError(this.handleError<any>('deleteDeliverySchedule', null)));
    }
    getJobCoordinates(jobIds: number[]): Observable<any[]> {
        return this.httpClient.post<any[]>(this.urlGetJobCoordinates, { jobIds: jobIds }).pipe(catchError(this.handleError<any>('getJobCoordinates', null)));
    }
    postValidateDRMaxFill(sbModel: DeliveryRequestViewModel[]): Observable<any> {
        return this.httpClient.post(this.urlPostValidateDRMaxFill, sbModel).pipe(catchError(this.handleError<any>('postValidateDRMaxFill', null)));
    }
}
