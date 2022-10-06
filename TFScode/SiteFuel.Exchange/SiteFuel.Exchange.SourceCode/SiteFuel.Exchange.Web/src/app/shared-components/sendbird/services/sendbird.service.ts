import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
@Injectable({
    providedIn: 'root'
})
export class chatService extends HandleError {

    driverId: any;
    driverDetails: BehaviorSubject<any>;
    loader: any;
    loaderDetails: BehaviorSubject<any>;
    memberInfo: any;
    memberInfoDetails: BehaviorSubject<any>;
    defaultdriverChat: any;
    defaultdriverChatDetails: BehaviorSubject<any>;
    constructor(private httpClient: HttpClient) {
        super();
        this.driverDetails = new BehaviorSubject(this.driverId);
        this.loaderDetails = new BehaviorSubject(this.loader);
        this.memberInfoDetails = new BehaviorSubject(this.memberInfo);
        this.defaultdriverChatDetails = new BehaviorSubject(this.defaultdriverChat);
    }
    private getDriverDetailsURL = '/Carrier/ScheduleBuilder/GetDriverDetails?driverId=';
    private getSendBirdAPIKeyURL = '/Carrier/ScheduleBuilder/GetSendBirdAPPId';
    private sendPushNotificationTODriver = '/Carrier/ScheduleBuilder/SendPushNotificationTODriver';
    private getDriversDetailsURL = '/Carrier/ScheduleBuilder/GetDriversDetails';
    
    getDriverDetails(driverId: number): Observable<any> {
        return this.httpClient.get<any>(this.getDriverDetailsURL + driverId)
            .pipe(catchError(this.handleError<any>('getDriverDetails', null)));
    }

    getSendBirdAPIKey(): Observable<any> {
        return this.httpClient.get<any>(this.getSendBirdAPIKeyURL)
            .pipe(catchError(this.handleError<any>('getSendBirdAPIKey', null)));
    }
    pushDriverDetails(driverId: any) {
        this.driverDetails.next(driverId);
    }
    
    loaderElement(status: any) {
        this.loaderDetails.next(status);
    }
    sendMemberInfo(memberData: any) {
        this.memberInfoDetails.next(memberData);
    }
    intializeChatDefault(driverObjDetails) {
        this.defaultdriverChatDetails.next(driverObjDetails);
    }
    sendPushNotification(msgData: any, drId: any) {
        var data = { message: msgData, driverId: drId };
        return this.httpClient.post<any>(this.sendPushNotificationTODriver, data)
            .pipe(catchError(this.handleError<any>('sendPushNotification', null)));
    }
    getDriversDetails(driverIds: any): Observable<any> {
        var data = { driverIds: driverIds };
        return this.httpClient.post<any>(this.getDriversDetailsURL, data)
            .pipe(catchError(this.handleError<any>('getDriversDetails', null)));
    }
}