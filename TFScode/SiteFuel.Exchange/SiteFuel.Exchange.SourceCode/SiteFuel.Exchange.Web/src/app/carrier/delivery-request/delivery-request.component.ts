import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter, ChangeDetectorRef, ChangeDetectionStrategy, ViewChild, ViewEncapsulation } from '@angular/core';
import { DeliveryRequestViewModel, DropAddressModel, DelRequestsByJobModel, JobDetailsWithOrders, CustomerJobsForCarrier } from '../models/DispatchSchedulerModels';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { DataService } from 'src/app/services/data.service';
import { sortBy } from 'src/app/my.functions';
import { CarrierService } from '../service/carrier.service';
import { DeliveryRequestService } from 'src/app/delivery-request-display/services/DeliveryRequestService';
import { BrokeredDrCarrierStatus } from 'src/app/app.enum';
import { UtilService } from 'src/app/services/util.service';
declare var IsCarrierCompany: boolean;

@Component({
    selector: 'app-delivery-request',
    templateUrl: './delivery-request.component.html',
    styleUrls: ['./delivery-request.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeliveryRequestComponent implements OnInit {

    @Input() public Title: string = '';
    @Input() public TitleClass: string = 'mustgo';
    @Input() public CollapaseId: string = 'collapse1';
    @Input() public CollapaseInClass: string = '';
    @Input() public isToggleDrPanel: boolean = false;
    @Input() public Locations: DelRequestsByJobModel[] = [];
    @Input() public PriorityTabCount: number = 0;
    public tempDeliveryRequest: DeliveryRequestViewModel[] = [];
    public DrForm: FormGroup;
    public _showQueueIcon: boolean = true;
    public _dragEnabled: boolean = true;
    public IsCarrierCompany: boolean = false;

    @Output() onDeliveryRequestEdit: EventEmitter<any> = new EventEmitter<any>();

    constructor(private fb: FormBuilder,
        private deliveryRequestService: DeliveryRequestService,
        private carrierService: CarrierService,
        private dataService: DataService,
        private changeDetectorRef: ChangeDetectorRef,
        private utilService: UtilService) { }


    ngOnInit() {
        this._showQueueIcon = true;
        this.tempDeliveryRequest = [];
        this.deliveryRequestService.localQueueIcon.subscribe((data) => {
            if (data != undefined) {
                this._showQueueIcon = data as boolean;
            }
        });
        if (typeof IsCarrierCompany !== 'undefined') {
            this.IsCarrierCompany = IsCarrierCompany ? true : false;
        }

        this.DrForm = this.fb.group({
            DeliveryRequests: this.getDeliveryRequestFormArray([])
        });
    }
    bindDeliveryRequests(jobId: number) {
        let location = this.Locations.find(t => t.JobId == jobId);
        if (location) {
            //var drs = location.DeliveryRequests;
            //if (location.DeliveryRequests.some(x => x.IsBlendedRequest === true)){
            //    drs = groupDrsByBlendGroupId(location.DeliveryRequests);
            //}
            this.DrForm = this.fb.group({
                DeliveryRequests: this.getDeliveryRequestFormArray(location.DeliveryRequests)
            });
        }
    }

    bindTBDDeliveryRequests(TbdGroupId: string) {
        let location = this.Locations.find(t => t.TBDGroupId == TbdGroupId);
        if (location) {
            this.DrForm = this.fb.group({
                DeliveryRequests: this.getDeliveryRequestFormArray(location.DeliveryRequests)
            });
        }
    }
    
    getDeliveryRequestFormArray(delReqs: DeliveryRequestViewModel[]): FormArray {
        delReqs = sortBy(delReqs, 'ProductType');
        let _drArray = this.fb.array([]);
        delReqs && delReqs.forEach(x => {
            var _form = this.utilService.getDeliveryRequestFormNew(x);
            _drArray.push(_form);
        });
        return _drArray;
    }

    EditDeliveryRequest(deliveryReq: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: deliveryReq, isDeleted: false, isAssignCarrier: false });
    }

    DeleteDeliveryRequest(deliveryReq: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: deliveryReq, isDeleted: true, isAssignCarrier: false });
    }

    MakeDrop(deliveryReq: any) {
        let delReq = deliveryReq.value as DeliveryRequestViewModel;
        if (!delReq.IsTBD) {
            this.carrierService.getDeliveryRequestById(delReq.ParentId).subscribe(x => {
                if (x.OrderId != null && x.TrackableScheduleId != null) {
                    let dropUrl = '/Supplier/Invoice/CreateNew?orderId=' + x.OrderId + '&trackableScheduleId=' + x.TrackableScheduleId;
                    window.location.href = dropUrl;
                }
            })
        }
        else {
            this.AddLocationForMissedTBD(deliveryReq);
        }
    }

    MoveActiveQueueRequest(drId: string, jobId: number, blendGroupId: string) {
        let drIds: string[] = [];
        let delReqs: DeliveryRequestViewModel[] = [];
        if (blendGroupId) {
            this.DrForm.controls['DeliveryRequests'].value.filter(t => t.BlendedGroupId == blendGroupId).forEach(t => {
                drIds.push(t.Id);
            });
        }
        else {
            drIds.push(drId);
        }
        drIds.forEach(dr => {
            let index = this.tempDeliveryRequest.findIndex(top => top.Id == dr);
            if (index === -1) {
                let drLocationIndex = this.Locations.findIndex(location =>
                    location.JobId == jobId);
                if (this.Locations[drLocationIndex] && this.Locations[drLocationIndex].DeliveryRequests) {
                    var delReq = this.Locations[drLocationIndex].DeliveryRequests.find(t => t.Id == dr);
                    if (delReq != null) {
                        delReq.WindowMode = 1;
                        delReq.QueueMode = 2;
                        this.tempDeliveryRequest.push(delReq);
                        delReqs.push(delReq);
                    }
                }
            }
        });
        this.deliveryRequestService.pushItemData(delReqs);
    }

    MoveToActiveQueue(drId: string, jobId: number) {
        let index = this.tempDeliveryRequest.findIndex(top => top.Id == drId);
        if (index === -1) {
            let drLocationIndex = this.Locations.findIndex(location =>
                location.JobId == jobId);
            if (this.Locations[drLocationIndex] && this.Locations[drLocationIndex].DeliveryRequests) {
                var dr = this.Locations[drLocationIndex].DeliveryRequests.find(t => t.Id == drId);
                if (dr != null) {
                    dr.WindowMode = 1;
                    dr.QueueMode = 2;
                    this.tempDeliveryRequest.push(dr);
                    this.deliveryRequestService.pushItemData(dr);
                }
            }
        }
    }

    public trackByDrId(index: number, dr): any {
        return dr.controls['Id'].value
    }

    public onDrag() {
        this.changeDetectorRef.markForCheck();
    }

    public getDraggedDrs(item: any, isLocation: boolean): any {
        let jobId, Id, GroupParentId: string = '', isTBDRequest: boolean = false, tbdGroupId: string = '';
        let isMarineDR = false;
        if (isLocation) {
            jobId = item.JobId;
            Id = null;
            isTBDRequest = item.IsTBD;
            tbdGroupId = item.TBDGroupId;
        }
        else {
            jobId = item.controls['JobId'].value;
            Id = item.controls['Id'].value;
            isMarineDR = item.controls['IsMarine'].value;
            GroupParentId = item.controls['GroupParentDRId'].value;
            isTBDRequest = item.controls['IsTBD'].value;
            tbdGroupId = item.controls['TBDGroupId'].value;
        }
        let drs = this.dataService.AllDeliveryRequestsSubject.value;

        if (GroupParentId == '' && !isMarineDR) {
            let filtered = [];
            if (!isTBDRequest) {
                filtered = drs.filter(x => x.JobId == jobId && !x.IsMarine && !x.IsBrokered && (!x.GroupParentDRId || x.GroupParentDRId == ''));
            }
            else {
                filtered = drs.filter(x => x.TBDGroupId == tbdGroupId && !x.IsBrokered && (!x.GroupParentDRId || x.GroupParentDRId == ''));
            }
            if (filtered.length == 0 && !Id) { //if a location is dragged and it is having only one marine dr, drag it to load
                if (!isTBDRequest) {
                    filtered = drs.filter(x => x.JobId == jobId && x.IsMarine && !x.IsBrokered && (!x.GroupParentDRId || x.GroupParentDRId == ''));
                }
                else {
                    filtered = drs.filter(x => x.TBDGroupId == tbdGroupId && x.IsMarine && !x.IsBrokered && (!x.GroupParentDRId || x.GroupParentDRId == ''));
                }
                if (filtered.length > 1) {
                    filtered = [];
                }
            }
            if (filtered.length == 0 && !Id) {
                if (!isTBDRequest) {
                    filtered = drs.filter(x => x.JobId == jobId && !x.IsMarine && !x.IsBrokered);
                }
                else {
                    filtered = drs.filter(x => x.TBDGroupId == tbdGroupId && !x.IsMarine && !x.IsBrokered);
                }
                if (filtered.length > 1) {
                    filtered = [];
                }
            }
            if (filtered.length > 0) {
                filtered.forEach(x => {
                    x.PreLoadedFor = null;
                    x.PreLoadInfo = null;
                    x.PostLoadedFor = null;
                    x.PostLoadInfo = null;
                });
                let brokeredDrs = filtered.filter(x => x.CarrierStatus == 2);
                let nonbrokeredDrs = filtered.filter(x => x.CarrierStatus == 0 || x.CarrierStatus == 3 || x.CarrierStatus == 4);
                filtered = sortBy(nonbrokeredDrs, 'ProductType');
                if (brokeredDrs.length > 0) {
                    brokeredDrs.forEach(x => {
                        filtered.push(x);
                    });
                }
            }
            //if (isMultipleSplitDrs) {
            //    Declarations.msgwarning("Selected job is having multiple split delivery requests.", undefined, undefined);
            //}
            return filtered;
        }
        else {
            let filtered = drs.filter(x => x.Id == Id);
            if (filtered && filtered.length && filtered[0].BlendedGroupId) {
                filtered = drs.filter(x => x.BlendedGroupId == filtered[0].BlendedGroupId);
            }
            filtered.forEach(x => {
                x.PreLoadedFor = null;
                x.PreLoadInfo = null;
                x.PostLoadedFor = null;
                x.PostLoadInfo = null;
            });
            return filtered;
        }
    }

    public AssignCarrier(dr: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: dr, isAssignCarrier: true, });
    }

    public recallBrokeredDrRequest(dr: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: dr, isDeleted: false, isRecallDr: true });
    }
    public AddLocationForMissedTBD(dr: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: dr, isCreateDeliveryForTBD: true, isDeliveryGroup: false });
    }

    public changeBrokeredDrStatus(status: BrokeredDrCarrierStatus, dr: any) {
        this.onDeliveryRequestEdit.emit({ deliveryRequest: dr, isApproveRejectBrokeredDr: true, CarrierStatus: status });
    }
    public SpiltDeliveryRequests(inputData: any) {
        let deliveryReq = inputData.value as DeliveryRequestViewModel;
        let drs: DeliveryRequestViewModel[] = [];
        if (!deliveryReq.IsBlendedRequest) {
            drs.push(deliveryReq);
        }
        else {
            this.DrForm.controls['DeliveryRequests'].value.filter(t => t.BlendedGroupId == deliveryReq.BlendedGroupId).forEach(t => {
                drs.push(t);
            });
        }
        this.dataService.setSplitDRsInfoSubject(drs);
    }
}
