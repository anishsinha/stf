import { Component, OnInit, Input, SimpleChanges, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { OttoNotifications } from '../../models/DispatchSchedulerModels';
import { ScheduleBuilderService } from '../../service/schedule-builder.service';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DataService } from 'src/app/services/data.service';
declare function closeSlidePanel(): any;
@Component({
    selector: 'app-otto-notification',
    templateUrl: './otto-notification.component.html',
    styleUrls: ['./otto-notification.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class OttoNotificationComponent implements OnInit {
    public SelectedRegionId: string;
    public _loadingOttoNotification: boolean = false;
    public OttoNotificationsDetails: OttoNotifications[] = [];
    constructor(private sbService: ScheduleBuilderService, private dataService: DataService,
        private changeDetectorRef: ChangeDetectorRef) {
        this.SelectedRegionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
    }

    ngOnInit() {
        this.getOttoNotification();
    }
    public closeSlider(): void {
        this.dataService.setOpenDsbOttoNotificationSubject(false);
    }
    public closeSidePanel() {
        closeSlidePanel();
    }
    public getOttoNotification() {
        this._loadingOttoNotification = true;
        this.sbService.getOttoNotificationDetails(this.SelectedRegionId)
            .subscribe(detail => {
                this.OttoNotificationsDetails = detail;
                this.dataService.setDsbOttoNotificationCountSubject(this.OttoNotificationsDetails.length);
                this._loadingOttoNotification = false;
                this.changeDetectorRef.detectChanges();
            });
    }
    public updateNotificationStatus(Id:string) {
        this._loadingOttoNotification = true;
        this.sbService.updateNotificationStatus(Id)
            .subscribe(detail => {
                this._loadingOttoNotification = false;
                if (detail.StatusCode == 0) {
                    this.getOttoNotification();
                }
            });
    }
}
