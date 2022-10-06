import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
    providedIn: 'root'
})
export class DeliveryRequestService {
    localStroageData: any;
    localStroage: BehaviorSubject<any>;
    QueueIcon: any;
    localQueueIcon: BehaviorSubject<any>;


    smallLoader: any;
    smallLoaderBeh: BehaviorSubject<any>;

    constructor() {
        this.localStroage = new BehaviorSubject(this.localStroageData);
        this.localQueueIcon = new BehaviorSubject(this.QueueIcon);
        this.smallLoaderBeh = new BehaviorSubject(this.smallLoader);
    }

    pushItemData(dr: any) {
        this.localStroage.next(dr);
    }
    ToggleQueueIcon(status: any) {
        this.localQueueIcon.next(status);
    }
    displayLoader(status: any) {
        this.smallLoaderBeh.next(status);
    }
}