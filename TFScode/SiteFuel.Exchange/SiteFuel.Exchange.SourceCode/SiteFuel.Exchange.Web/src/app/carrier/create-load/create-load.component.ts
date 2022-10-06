import { Component, OnInit, ChangeDetectionStrategy, ViewChild, ElementRef, OnDestroy, ChangeDetectorRef, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { DeliveryRequestViewModel, DemandModel, CustomerJobsForCarrier } from '../models/DispatchSchedulerModels';
import { DataService } from 'src/app/services/data.service';
import { ScheduleBuilderService } from '../service/schedule-builder.service';
import { UtilService } from 'src/app/services/util.service';
import { Declarations } from 'src/app/declarations.module';
import { sortBy, sum, groupDrsByProduct } from 'src/app/my.functions';
import { DropdownItem } from 'src/app/statelist.service';
import { CarrierService } from '../service/carrier.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DispatcherService } from '../service/dispatcher.service';
import { BrokeredDrCarrierStatus } from 'src/app/app.enum';
import { RegExConstants } from 'src/app/app.constants';

@Component({
    selector: 'app-create-load',
    templateUrl: './create-load.component.html',
    styleUrls: ['./create-load.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateLoadComponent implements OnInit, OnChanges, OnDestroy {

    @Input() public SelectedRegionId: string;
    @Input() selectedProduct: string;

    public LoadForm: FormGroup;
    public _loadingProducts: boolean = true;
    public showLoadOverlay: boolean = false;
    public showHidePanelDr: boolean = true;
    public DemandModels: DemandModel[] = [];
    public showTankFlag: boolean = false;
    public tankListForLocation: DemandModel[] = [];
    public DeliveryRequests: DeliveryRequestViewModel[] = [];
    public BrokeredDeliveryRequests: DeliveryRequestViewModel[] = [];
    public DeletedBrokeredDeliveryRequests: DeliveryRequestViewModel[] = [];
    public RecurringDeliveryRequests: DeliveryRequestViewModel[] = [];
    public DeletedRecurringDeliveryRequests: DeliveryRequestViewModel[] = [];
    public DeletedDeliveryRequests: DeliveryRequestViewModel[] = [];
    public GroupedDeliveryRequests: DeliveryRequestViewModel[] = [];
    public SplitDeliveryRequests: DeliveryRequestViewModel[] = [];
    public DeletedSplitDeliveryRequests: DeliveryRequestViewModel[] = [];
    public LoadInfo = { RegionId: '', Customer: '', JobId: 0, JobName: '' };
    public TotalUllage: number = 0;
    public TotalUllageUoM: string = 'Gallons';
    public draggedDrs = [];
    public IsTankExists: boolean = true;
    public OrderList: DropdownItem[] = [];
    public SelectedOrders: DropdownItem[] = [];
    public CustomerList: CustomerJobsForCarrier[] = [];
    public multiDropdownSettings: IDropdownSettings;

    private CreateLoadInputData: any = {};
    private CreateLoadFormInitData: any = '';
    private CreateLoadChangeSubscription: Subscription;
    public disabledSaveButton: boolean = false;
    public ScheduleQuantityTypes: any = [];
    @ViewChild("createloadpanel", { static: true }) loadPanel: ElementRef;

    constructor(private dataService: DataService, private utilService: UtilService,
        private sbService: ScheduleBuilderService, private carrierService: CarrierService,
        private changeDetectorRef: ChangeDetectorRef,
        private dispatcherService: DispatcherService) { }

    public ngOnInit(): void {
        this.LoadForm = this.utilService.getLoadForm();
        this.subscribeCreateLoadChangeSubject();
        this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            allowSearchFilter: true,
            itemsShowLimit: 1
        };
    }

    public ngOnChanges(change: SimpleChanges): void {
        if (change.SelectedRegionId && change.SelectedRegionId.currentValue) {
            window.setTimeout(() => {
                this.carrierService.getCreateLoadJobListForCarrier(this.SelectedRegionId).subscribe(t2 => {
                    this.CustomerList = t2 as CustomerJobsForCarrier[];
                });
            }, 2000);
        }
    }

    public ngOnDestroy(): void {
        this.unsubscribeCreateLoadChangeSubject();
    }

    public removeProduct(index: number, demand: any): void {
        this.DeletedRecurringDeliveryRequests = [];
        this.DeletedDeliveryRequests = [];
        let _array = this.LoadForm.controls['Demands'] as FormArray;
        let deleted: DeliveryRequestViewModel[] = [];
        if (_array) {
            this.removeSelectedItem(_array.controls[index]);
            _array.removeAt(index);

            if (demand.controls['isRecurringSchedule'].value == true) {
                let drId = demand.controls['DeliveryReqId'].value;
                deleted = this.RecurringDeliveryRequests.filter(x => x.Id == drId);
                deleted.forEach(x => {
                    this.DeletedRecurringDeliveryRequests.push(x);
                });

                this.RecurringDeliveryRequests = this.RecurringDeliveryRequests.filter(t => deleted.indexOf(t) == -1);
                this.CreateLoadInputData.Drs = this.CreateLoadInputData.Drs.filter(t => deleted.indexOf(t) == -1);
                this.TotalUllage = sum(_array.value, 'Ullage');

            }
            else if (demand.controls['CarrierStatus'].value == BrokeredDrCarrierStatus.Accepted) {
                let drId = demand.controls['DeliveryReqId'].value;
                deleted = this.BrokeredDeliveryRequests.filter(x => x.Id == drId);
                deleted.forEach(x => {
                    this.DeletedBrokeredDeliveryRequests.push(x);
                });

                this.RecurringDeliveryRequests = this.BrokeredDeliveryRequests.filter(t => deleted.indexOf(t) == -1);
                this.CreateLoadInputData.Drs = this.CreateLoadInputData.Drs.filter(t => deleted.indexOf(t) == -1);
                this.TotalUllage = sum(_array.value, 'Ullage');

            }
            else if (demand.controls['GroupParentDRId'].value != '') {
                let drId = demand.controls['DeliveryReqId'].value;
                deleted = this.SplitDeliveryRequests.filter(x => x.Id == drId);
                deleted.forEach(x => {
                    this.DeletedSplitDeliveryRequests.push(x);
                });

                this.SplitDeliveryRequests = this.SplitDeliveryRequests.filter(t => deleted.indexOf(t) == -1);
                this.CreateLoadInputData.Drs = this.CreateLoadInputData.Drs.filter(t => deleted.indexOf(t) == -1);
                this.TotalUllage = sum(_array.value, 'Ullage');

            }
            else {
                let productType = demand.controls['ProductType'].value;
                let ScheduleQuantityType = demand.controls['ScheduleQuantityType'].value;
                let orderId = demand.controls['OrderId'].value;
                if (orderId > 0) {
                    deleted = this.DeliveryRequests.filter(x => x.OrderId == orderId && x.ScheduleQuantityType == ScheduleQuantityType);
                }
                else {
                    deleted = this.DeliveryRequests.filter(x => x.ProductType == productType && x.ScheduleQuantityType == ScheduleQuantityType);
                }
                if (deleted != null) {
                    deleted.forEach(x => {
                        this.DeletedDeliveryRequests.push(x);
                    });
                }
                this.DeliveryRequests = this.DeliveryRequests.filter(t => deleted.indexOf(t) == -1);
                this.CreateLoadInputData.Drs = this.CreateLoadInputData.Drs.filter(t => deleted.indexOf(t) == -1);
                this.TotalUllage = sum(_array.value, 'Ullage');
            }
            this.dataService.setCreateLoadCancelSubject({
                RegionId: this.CreateLoadInputData.RegionId,
                Customer: this.CreateLoadInputData.Customer,
                JobId: this.CreateLoadInputData.JobId,
                JobName: this.CreateLoadInputData.JobName,
                Drs: deleted,
                ShiftIndex: this.CreateLoadInputData.ShiftIndex,
                RowIndex: this.CreateLoadInputData.RowIndex,
                ColIndex: this.CreateLoadInputData.ColIndex
            });


            this.disabledSaveButtonFunc(_array.value);
        }
    }

    getScheduleQuantityType() {
        this.dispatcherService.GetScheduleQtyType().subscribe((SQT: any[]) => {
            this.ScheduleQuantityTypes = SQT || [];
        });
    }
    private subscribeCreateLoadChangeSubject(): void {
        this.CreateLoadChangeSubscription = this.dataService.CreateLoadChangeSubject.subscribe(x => {
            if (x && x.Drs.length > 0) {
                this.DeletedRecurringDeliveryRequests = [];
                this.DeletedDeliveryRequests = [];
                this.DeletedSplitDeliveryRequests = [];
                this.CreateLoadInputData = x;
                let drs = x.Drs as DeliveryRequestViewModel[];
                this.draggedDrs = drs;
                this.RecurringDeliveryRequests = drs.filter(top => top.isRecurringSchedule == true);
                this.RecurringDeliveryRequests = this.RecurringDeliveryRequests.filter((el, i, a) => i === a.indexOf(el));
                this.DeliveryRequests = sortBy(drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && (top.CarrierStatus == 0 || top.CarrierStatus == 3 || top.CarrierStatus == 4)), 'ProductType');
                this.BrokeredDeliveryRequests = drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && top.CarrierStatus == 2);
                this.SplitDeliveryRequests = drs.filter(top => top.GroupParentDRId != '');
                this.GroupedDeliveryRequests = groupDrsByProduct(this.DeliveryRequests);
                this.RecurringDeliveryRequests.forEach(x => {
                    this.GroupedDeliveryRequests.push(x);
                });
                this.BrokeredDeliveryRequests.forEach(x => {
                    this.GroupedDeliveryRequests.push(x);
                });
                this.SplitDeliveryRequests.forEach(x => {
                    this.GroupedDeliveryRequests.push(x);
                });
                this.GroupedDeliveryRequests.slice();
                this.getCustomerLocationDemands(x.JobId, x.RegionId, x.Customer);
                this.getScheduleQuantityType();
                this.LoadInfo = { RegionId: x.RegionId, Customer: x.Customer, JobId: x.JobId, JobName: x.JobName };
                //Declarations.slidePanel('#create-load-panel', '35%');
                //this.showLoadOverlay = true;
                this.changeDetectorRef.detectChanges();
                //this.disabledSaveButtonFunc(drs);
            }
        });
    }

    private unsubscribeCreateLoadChangeSubject(): void {
        if (this.CreateLoadChangeSubscription) {
            this.CreateLoadChangeSubscription.unsubscribe();
        }
    }

    public closeCreateLoad(): void {
        this.showLoadOverlay = false;
        Declarations.closeSlidePanel();
        this.resetForm();
    }

    public cancelLoadCreation(): void {
        this.dataService.setCreateLoadCancelSubject(this.CreateLoadInputData);
        this.resetForm();
    }

    private resetForm(): void {
        (this.LoadForm.controls['Demands'] as FormArray).clear();
        this.LoadForm.reset();
        this.TotalUllage = 0;
        this.IsTankExists = true;
    }

    private updateRequiredQuantity(demandData: DemandModel[]): void {
        let groupedDeliveryRequests = this.GroupedDeliveryRequests.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && (top.CarrierStatus == 0 || top.CarrierStatus == 3 || top.CarrierStatus == 4));
        for (var index = 0; index < groupedDeliveryRequests.length; index++) {
            let thisDr = groupedDeliveryRequests[index];
            // ProductType check is for Drs with Tanks. OrderId check is for Drs without tanks.
            let demand = demandData.find(x => (x.ProductName == thisDr.ProductType && x.TankId != null && x.TankId.length > 0 && x.StorageId != null && x.StorageId.length > 0));
            if (!demand) {
                demand = demandData.find(x => x.OrderId > 0 && x.OrderId == thisDr.OrderId);
            }
            if (demand) {
                demand.RequiredQuantity = thisDr.RequiredQuantity;
                demand.ScheduleQuantityType = (thisDr.RequiredQuantity > 0) ? 1 : (demand.ScheduleQuantityType || 1);
                demand.ScheduleQuantityTypeText = thisDr.ScheduleQuantityTypeText;
                demand.Priority = thisDr.Priority;
                demand.DeliveryRequestFor = thisDr.DeliveryRequestFor;
                demand.IsDRExists = thisDr.IsDRExists;
                demand.IsDRMissed = thisDr.IsDRMissed;
                demand.OrderId = thisDr.OrderId;
                demand.BadgeNo1 = thisDr.BadgeNo1;
                demand.BadgeNo2 = thisDr.BadgeNo2;
                demand.BadgeNo3 = thisDr.BadgeNo3;
                demand.PickupLocationType = thisDr.PickupLocationType;
                demand.Terminal = thisDr.Terminal;
                demand.BulkPlant = thisDr.BulkPlant;
                demand.DispatcherNote = thisDr.DispactherNote;
                demand.Notes = thisDr.Notes;
                demand.IsAcceptNightDeliveries = thisDr.IsAcceptNightDeliveries;
                demand.LoadQueueAttributes = thisDr.LoadQueueAttributes;
                demand.DRQueueAttributes = thisDr.DRQueueAttributes;
                demand.TrailerTypes = thisDr.TrailerTypes;
                demand.HoursToCoverDistance = thisDr.HoursToCoverDistance;
                if (this.TotalUllageUoM != demand.UoM) {
                    this.TotalUllageUoM = demand.UoM;
                }
            }
        }
        this.TotalUllage = sum(demandData, 'Ullage');
    }

    private processDemands(data: any, regionId: string, isTankExists: boolean): void {
        if (isTankExists) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    data[i].isRecurringSchedule = false;
                    data[i].CarrierStatus = 0;
                    data[i].IsEndSupplier = true;
                    data[i].IsDispatchRetained = false;
                }
            }
            var processRecFound = this.processRecurringSchedule(data);
            if (processRecFound != null && processRecFound.length > 0) {
                if (this.DeliveryRequests.length > 0) {
                    processRecFound.forEach(x => {
                        data.push(x);
                    });
                } else {
                    data = processRecFound;
                }
            }
            else {
                var processRecFound = this.processBrokeredSchedule(data);
                if (processRecFound != null && processRecFound.length > 0) {
                    if (this.DeliveryRequests.length > 0 || this.RecurringDeliveryRequests.length > 0) {
                        processRecFound.forEach(x => {
                            data.push(x);
                        });
                    }
                    else {
                        data = processRecFound;
                    }
                }
                else {
                    var processRecFound = this.processSplitSchedule(data);
                    if (processRecFound != null && processRecFound.length > 0) {
                        if (this.DeliveryRequests.length > 0 || this.RecurringDeliveryRequests.length > 0 || this.BrokeredDeliveryRequests.length > 0) {
                            processRecFound.forEach(x => {
                                data.push(x);
                            });
                        } else {
                            data = processRecFound;
                        }
                    }
                }
            }
        }
        else {
            var processRecFound = this.processJobRecurringSchedule(data);
            if (processRecFound != null && processRecFound.length > 0) {
                if (this.DeliveryRequests.length > 0) {
                    processRecFound.forEach(x => {
                        data.push(x);
                    });
                }
                else {
                    data = processRecFound;
                }
            }
            else {
                var processRecFound = this.processJobBrokeredSchedule(data);
                if (processRecFound != null && processRecFound.length > 0) {
                    if (this.DeliveryRequests.length > 0 || this.RecurringDeliveryRequests.length > 0) {
                        processRecFound.forEach(x => {
                            data.push(x);
                        });
                    }
                    else {
                        data = processRecFound;
                    }
                }
                else {
                    var processRecFound = this.processJobSplitSchedule(data);
                    if (processRecFound != null && processRecFound.length > 0) {
                        if (this.DeliveryRequests.length > 0 || this.RecurringDeliveryRequests.length > 0 || this.BrokeredDeliveryRequests.length > 0) {
                            processRecFound.forEach(x => {
                                data.push(x);
                            });
                        }
                        else {
                            data = processRecFound;
                        }
                    }
                }
            }
        }
        this.updateRequiredQuantity(data);
        data = sortBy(data, 'ProductSequence');
        data = data.filter(t => t.ScheduleQuantityType != 1 || t.RequiredQuantity > 0);;
        this.DemandModels = data;
        let _array = this.LoadForm.controls['Demands'] as FormArray;
        //if there are no requests in create load which belongs to endsupplier then disable add button only for non retail job
        //if (data.filter(t => t.IsEndSupplier).length == 0) {
        //    this.disabledSaveButton = true;
        //}
        //else {
        //    this.disabledSaveButton = false;
        //}
        this.utilService.initCreateLoadFormArray(_array, data, regionId);
        this._loadingProducts = false;
        this.changeDetectorRef.detectChanges();
        this.CreateLoadFormInitData = JSON.stringify(_array.value);
        this.onSubmit();
    }
    private processRecurringSchedule(data: any): [] {
        var recurringDrsDetails: any = [];
        if (data != null) {
            if (this.RecurringDeliveryRequests != null) {
                this.RecurringDeliveryRequests.forEach((_dr: DeliveryRequestViewModel) => {
                    let recurringDrs = data.find(x => (x.ProductName == _dr.ProductType && x.TankId != null && x.TankId.length > 0 && x.StorageId != null && x.StorageId.length > 0));
                    if (!recurringDrs) {
                        recurringDrs = data.find(x => x.OrderId > 0 && x.OrderId == _dr.OrderId);
                    }
                    if (recurringDrs != null && _dr.JobId == recurringDrs.JobId) {
                        var demandModel = new DemandModel();
                        demandModel.Id = recurringDrs.Id;
                        demandModel.JobId = recurringDrs.JobId;
                        demandModel.JobName = recurringDrs.JobName;
                        demandModel.Level = recurringDrs.Level;
                        demandModel.OrderId = _dr.OrderId;
                        demandModel.Priority = recurringDrs.Priority;
                        demandModel.ProductName = recurringDrs.ProductName;
                        demandModel.ProductType = recurringDrs.ProductType;
                        demandModel.SiteId = recurringDrs.SiteId;
                        demandModel.StorageId = recurringDrs.StorageId;
                        demandModel.TankCapacity = recurringDrs.TankCapacity;
                        demandModel.TankId = recurringDrs.TankId;
                        demandModel.TankMaxFill = recurringDrs.TankMaxFill;
                        demandModel.TankMinFill = recurringDrs.TankMinFill;
                        demandModel.TankName = recurringDrs.TankName;
                        demandModel.Ullage = recurringDrs.Ullage;
                        demandModel.UoM = recurringDrs.UoM;
                        demandModel.IsDRExists = false;
                        demandModel.IsDRMissed = false;
                        demandModel.BuyerCompanyName = recurringDrs.BuyerCompanyName;
                        demandModel.isRecurringSchedule = true;
                        demandModel.RequiredQuantity = _dr.RequiredQuantity;
                        demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                        demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                        demandModel.DeliveryReqId = _dr.Id;
                        demandModel.IsEndSupplier = recurringDrs.IsEndSupplier;
                        demandModel.IsDispatchRetained = recurringDrs.IsDispatchRetained;
                        demandModel.DeliveryRequestFor = _dr.DeliveryRequestFor;
                        demandModel.ExistingDR = [];
                        demandModel.Notes = _dr.Notes;
                        demandModel.GroupParentDRId = _dr.GroupParentDRId;
                        demandModel.ProductSequence = recurringDrs.ProductSequence;
                        demandModel.BadgeNo1 = _dr.BadgeNo1;
                        demandModel.BadgeNo2 = _dr.BadgeNo2;
                        demandModel.BadgeNo3 = _dr.BadgeNo3;
                        demandModel.PickupLocationType = _dr.PickupLocationType;
                        demandModel.Terminal = _dr.Terminal;
                        demandModel.BulkPlant = _dr.BulkPlant;
                        demandModel.DispatcherNote = _dr.DispactherNote;
                        demandModel.TrailerTypes = _dr.TrailerTypes;
                        demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                        demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                        demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                        demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                        recurringDrsDetails.push(demandModel);
                    }

                });
            }

            return recurringDrsDetails;
        }
    }
    private processBrokeredSchedule(data: any): [] {
        var brokeredDrsDetails: any = [];
        if (data != null) {
            if (this.BrokeredDeliveryRequests != null) {
                this.BrokeredDeliveryRequests.forEach(_dr => {
                    let brokeredDrs = data.find(x => (x.ProductName == _dr.ProductType && x.TankId != null && x.TankId.length > 0 && x.StorageId != null && x.StorageId.length > 0));
                    if (!brokeredDrs) {
                        brokeredDrs = data.find(x => x.OrderId > 0 && x.OrderId == _dr.OrderId);
                    }
                    if (brokeredDrs != null && _dr.JobId == brokeredDrs.JobId) {
                        var demandModel = new DemandModel();
                        demandModel.Id = brokeredDrs.Id;
                        demandModel.JobId = brokeredDrs.JobId;
                        demandModel.JobName = brokeredDrs.JobName;
                        demandModel.Level = brokeredDrs.Level;
                        demandModel.OrderId = _dr.OrderId;
                        demandModel.Priority = brokeredDrs.Priority;
                        demandModel.ProductName = brokeredDrs.ProductName;
                        demandModel.ProductType = brokeredDrs.ProductType;
                        demandModel.SiteId = brokeredDrs.SiteId;
                        demandModel.StorageId = brokeredDrs.StorageId;
                        demandModel.TankCapacity = brokeredDrs.TankCapacity;
                        demandModel.TankId = brokeredDrs.TankId;
                        demandModel.TankMaxFill = brokeredDrs.TankMaxFill;
                        demandModel.TankMinFill = brokeredDrs.TankMinFill;
                        demandModel.TankName = brokeredDrs.TankName;
                        demandModel.Ullage = brokeredDrs.Ullage;
                        demandModel.UoM = brokeredDrs.UoM;
                        demandModel.IsDRExists = false;
                        demandModel.IsDRMissed = false;
                        demandModel.BuyerCompanyName = brokeredDrs.BuyerCompanyName;
                        demandModel.isRecurringSchedule = false;
                        demandModel.RequiredQuantity = _dr.RequiredQuantity;
                        demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                        demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                        demandModel.DeliveryReqId = _dr.Id;
                        demandModel.IsEndSupplier = brokeredDrs.IsEndSupplier;
                        demandModel.IsDispatchRetained = brokeredDrs.IsDispatchRetained;
                        demandModel.CarrierStatus = BrokeredDrCarrierStatus.Accepted;
                        demandModel.ExistingDR = [];
                        demandModel.Notes = _dr.Notes;
                        demandModel.BadgeNo1 = _dr.BadgeNo1;
                        demandModel.BadgeNo2 = _dr.BadgeNo2;
                        demandModel.BadgeNo3 = _dr.BadgeNo3;
                        demandModel.PickupLocationType = _dr.PickupLocationType;
                        demandModel.Terminal = _dr.Terminal;
                        demandModel.BulkPlant = _dr.BulkPlant;
                        demandModel.DispatcherNote = _dr.DispactherNote;
                        demandModel.GroupParentDRId = _dr.GroupParentDRId;
                        demandModel.ProductSequence = brokeredDrs.ProductSequence;
                        demandModel.TrailerTypes = _dr.TrailerTypes;
                        demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                        demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                        demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                        demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                        brokeredDrsDetails.push(demandModel);
                    }

                });
            }

            return brokeredDrsDetails;
        }
    }
    private processSplitSchedule(data: any): [] {
        var splitDrsDetails: any = [];
        if (data != null) {
            if (this.SplitDeliveryRequests != null) {
                this.SplitDeliveryRequests.forEach(_dr => {
                    let splitDrs = data.find(x => (x.ProductName == _dr.ProductType && x.TankId != null && x.TankId.length > 0 && x.StorageId != null && x.StorageId.length > 0));
                    if (!splitDrs) {
                        splitDrs = data.find(x => x.OrderId > 0 && x.OrderId == _dr.OrderId);
                    }
                    if (splitDrs != null && _dr.JobId == splitDrs.JobId) {
                        var demandModel = new DemandModel();
                        demandModel.Id = splitDrs.Id;
                        demandModel.JobId = splitDrs.JobId;
                        demandModel.JobName = splitDrs.JobName;
                        demandModel.Level = splitDrs.Level;
                        demandModel.OrderId = _dr.OrderId;
                        demandModel.Priority = splitDrs.Priority;
                        demandModel.ProductName = splitDrs.ProductName;
                        demandModel.ProductType = splitDrs.ProductType;
                        demandModel.SiteId = splitDrs.SiteId;
                        demandModel.StorageId = splitDrs.StorageId;
                        demandModel.TankCapacity = splitDrs.TankCapacity;
                        demandModel.TankId = splitDrs.TankId;
                        demandModel.TankMaxFill = splitDrs.TankMaxFill;
                        demandModel.TankMinFill = splitDrs.TankMinFill;
                        demandModel.TankName = splitDrs.TankName;
                        demandModel.Ullage = splitDrs.Ullage;
                        demandModel.UoM = splitDrs.UoM;
                        demandModel.IsDRExists = false;
                        demandModel.IsDRMissed = false;
                        demandModel.BuyerCompanyName = splitDrs.BuyerCompanyName;
                        demandModel.isRecurringSchedule = false;
                        demandModel.RequiredQuantity = _dr.RequiredQuantity;
                        demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                        demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                        demandModel.DeliveryReqId = _dr.Id;
                        demandModel.IsEndSupplier = splitDrs.IsEndSupplier;
                        demandModel.IsDispatchRetained = splitDrs.IsDispatchRetained;
                        demandModel.DeliveryRequestFor = splitDrs.DeliveryRequestFor;
                        demandModel.ExistingDR = [];
                        demandModel.Notes = _dr.Notes;
                        demandModel.BadgeNo1 = _dr.BadgeNo1;
                        demandModel.BadgeNo2 = _dr.BadgeNo2;
                        demandModel.BadgeNo3 = _dr.BadgeNo3;
                        demandModel.PickupLocationType = _dr.PickupLocationType;
                        demandModel.Terminal = _dr.Terminal;
                        demandModel.BulkPlant = _dr.BulkPlant;
                        demandModel.DispatcherNote = _dr.DispactherNote;
                        demandModel.GroupParentDRId = _dr.GroupParentDRId;
                        demandModel.ProductSequence = splitDrs.ProductSequence;
                        demandModel.TrailerTypes = _dr.TrailerTypes;
                        demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                        demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                        demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                        demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                        splitDrsDetails.push(demandModel);
                    }

                });
            }

            return splitDrsDetails;
        }
    }
    private processJobRecurringSchedule(data: any): [] {
        var recurringDrsDetails: any = [];
        if (data != null) {
            if (this.RecurringDeliveryRequests != null) {
                this.RecurringDeliveryRequests.forEach((_dr: DeliveryRequestViewModel) => {
                    var recurringDrs = data.find(top => top.OrderId == _dr.OrderId);
                    if (recurringDrs != null) {
                        if (_dr.OrderId == recurringDrs.OrderId) {
                            var demandModel = new DemandModel();
                            demandModel.Id = recurringDrs.Id;
                            demandModel.JobId = recurringDrs.JobId;
                            demandModel.JobName = recurringDrs.JobName;
                            demandModel.Level = recurringDrs.Level;
                            demandModel.OrderId = recurringDrs.OrderId;
                            demandModel.Priority = recurringDrs.Priority;
                            demandModel.ProductName = recurringDrs.ProductName;
                            demandModel.ProductType = recurringDrs.ProductType;
                            demandModel.SiteId = recurringDrs.SiteId;
                            demandModel.StorageId = recurringDrs.StorageId;
                            demandModel.TankCapacity = recurringDrs.TankCapacity;
                            demandModel.TankId = recurringDrs.TankId;
                            demandModel.TankMaxFill = recurringDrs.TankMaxFill;
                            demandModel.TankMinFill = recurringDrs.TankMinFill;
                            demandModel.TankName = recurringDrs.TankName;
                            demandModel.Ullage = recurringDrs.Ullage;
                            demandModel.UoM = recurringDrs.UoM;
                            demandModel.IsDRExists = false;
                            demandModel.IsDRMissed = false;
                            demandModel.BuyerCompanyName = recurringDrs.BuyerCompanyName;
                            demandModel.isRecurringSchedule = true;
                            demandModel.RequiredQuantity = _dr.RequiredQuantity;
                            demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                            demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                            demandModel.IsEndSupplier = recurringDrs.IsEndSupplier;
                            demandModel.IsDispatchRetained = recurringDrs.IsDispatchRetained;
                            demandModel.DeliveryReqId = _dr.Id;
                            demandModel.ExistingDR = [];
                            demandModel.Notes = _dr.Notes;
                            demandModel.BadgeNo1 = _dr.BadgeNo1;
                            demandModel.BadgeNo2 = _dr.BadgeNo2;
                            demandModel.BadgeNo3 = _dr.BadgeNo3;
                            demandModel.PickupLocationType = _dr.PickupLocationType;
                            demandModel.Terminal = _dr.Terminal;
                            demandModel.BulkPlant = _dr.BulkPlant;
                            demandModel.DispatcherNote = _dr.DispactherNote;
                            demandModel.GroupParentDRId = _dr.GroupParentDRId;
                            demandModel.ProductSequence = recurringDrs.ProductSequence;
                            demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                            demandModel.TrailerTypes = _dr.TrailerTypes;
                            demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                            demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                            demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                            recurringDrsDetails.push(demandModel);
                        }
                    }
                });
            }
            return recurringDrsDetails;
        }
    }
    private processJobBrokeredSchedule(data: any): [] {
        var brokeredDrsDetails: any = [];
        if (data != null) {
            if (this.BrokeredDeliveryRequests != null) {
                this.BrokeredDeliveryRequests.forEach(_dr => {
                    var brokeredDrs = data.find(top => top.OrderId == _dr.OrderId);
                    if (brokeredDrs != null) {
                        if (_dr.OrderId == brokeredDrs.OrderId) {
                            var demandModel = new DemandModel();
                            demandModel.Id = brokeredDrs.Id;
                            demandModel.JobId = brokeredDrs.JobId;
                            demandModel.JobName = brokeredDrs.JobName;
                            demandModel.Level = brokeredDrs.Level;
                            demandModel.OrderId = brokeredDrs.OrderId;
                            demandModel.Priority = brokeredDrs.Priority;
                            demandModel.ProductName = brokeredDrs.ProductName;
                            demandModel.ProductType = brokeredDrs.ProductType;
                            demandModel.SiteId = brokeredDrs.SiteId;
                            demandModel.StorageId = brokeredDrs.StorageId;
                            demandModel.TankCapacity = brokeredDrs.TankCapacity;
                            demandModel.TankId = brokeredDrs.TankId;
                            demandModel.TankMaxFill = brokeredDrs.TankMaxFill;
                            demandModel.TankMinFill = brokeredDrs.TankMinFill;
                            demandModel.TankName = brokeredDrs.TankName;
                            demandModel.Ullage = brokeredDrs.Ullage;
                            demandModel.UoM = brokeredDrs.UoM;
                            demandModel.IsDRExists = false;
                            demandModel.IsDRMissed = false;
                            demandModel.BuyerCompanyName = brokeredDrs.BuyerCompanyName;
                            demandModel.isRecurringSchedule = false;
                            demandModel.RequiredQuantity = _dr.RequiredQuantity;
                            demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                            demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                            demandModel.DeliveryReqId = _dr.Id;
                            demandModel.IsEndSupplier = brokeredDrs.IsEndSupplier;
                            demandModel.IsDispatchRetained = brokeredDrs.IsDispatchRetained;
                            demandModel.CarrierStatus = BrokeredDrCarrierStatus.Accepted;
                            demandModel.ExistingDR = [];
                            demandModel.Notes = _dr.Notes;
                            demandModel.BadgeNo1 = _dr.BadgeNo1;
                            demandModel.BadgeNo2 = _dr.BadgeNo2;
                            demandModel.BadgeNo3 = _dr.BadgeNo3;
                            demandModel.PickupLocationType = _dr.PickupLocationType;
                            demandModel.Terminal = _dr.Terminal;
                            demandModel.BulkPlant = _dr.BulkPlant;
                            demandModel.DispatcherNote = _dr.DispactherNote;
                            demandModel.GroupParentDRId = _dr.GroupParentDRId;
                            demandModel.ProductSequence = brokeredDrs.ProductSequence;
                            demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                            demandModel.TrailerTypes = _dr.TrailerTypes;
                            demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                            demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                            demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                            brokeredDrsDetails.push(demandModel);
                        }
                    }
                });
            }
            return brokeredDrsDetails;
        }
    }
    private processJobSplitSchedule(data: any): [] {
        var splitDrsDetails: any = [];
        if (data != null) {
            if (this.SplitDeliveryRequests != null) {
                this.SplitDeliveryRequests.forEach(_dr => {
                    var splitDrs = data.find(top => top.OrderId == _dr.OrderId);
                    if (splitDrs != null) {
                        if (_dr.OrderId == splitDrs.OrderId) {
                            var demandModel = new DemandModel();
                            demandModel.Id = splitDrs.Id;
                            demandModel.JobId = splitDrs.JobId;
                            demandModel.JobName = splitDrs.JobName;
                            demandModel.Level = splitDrs.Level;
                            demandModel.OrderId = splitDrs.OrderId;
                            demandModel.Priority = splitDrs.Priority;
                            demandModel.ProductName = splitDrs.ProductName;
                            demandModel.ProductType = splitDrs.ProductType;
                            demandModel.SiteId = splitDrs.SiteId;
                            demandModel.StorageId = splitDrs.StorageId;
                            demandModel.TankCapacity = splitDrs.TankCapacity;
                            demandModel.TankId = splitDrs.TankId;
                            demandModel.TankMaxFill = splitDrs.TankMaxFill;
                            demandModel.TankMinFill = splitDrs.TankMinFill;
                            demandModel.TankName = splitDrs.TankName;
                            demandModel.Ullage = splitDrs.Ullage;
                            demandModel.UoM = splitDrs.UoM;
                            demandModel.IsDRExists = false;
                            demandModel.IsDRMissed = false;
                            demandModel.BuyerCompanyName = splitDrs.BuyerCompanyName;
                            demandModel.isRecurringSchedule = false;
                            demandModel.RequiredQuantity = _dr.RequiredQuantity;
                            demandModel.ScheduleQuantityType = _dr.ScheduleQuantityType;
                            demandModel.ScheduleQuantityTypeText = _dr.ScheduleQuantityTypeText;
                            demandModel.DeliveryReqId = _dr.Id;
                            demandModel.IsEndSupplier = splitDrs.IsEndSupplier;
                            demandModel.IsDispatchRetained = splitDrs.IsDispatchRetained;
                            demandModel.CarrierStatus = BrokeredDrCarrierStatus.Accepted;
                            demandModel.ExistingDR = [];
                            demandModel.BadgeNo1 = _dr.BadgeNo1;
                            demandModel.BadgeNo2 = _dr.BadgeNo2;
                            demandModel.BadgeNo3 = _dr.BadgeNo3;
                            demandModel.PickupLocationType = _dr.PickupLocationType;
                            demandModel.Terminal = _dr.Terminal;
                            demandModel.BulkPlant = _dr.BulkPlant;
                            demandModel.DispatcherNote = _dr.DispactherNote;
                            demandModel.Notes = _dr.Notes;
                            demandModel.GroupParentDRId = _dr.GroupParentDRId;
                            demandModel.ProductSequence = splitDrs.ProductSequence;
                            demandModel.IsAcceptNightDeliveries = _dr.IsAcceptNightDeliveries;
                            demandModel.TrailerTypes = _dr.TrailerTypes;
                            demandModel.LoadQueueAttributes = _dr.LoadQueueAttributes;
                            demandModel.DRQueueAttributes = _dr.DRQueueAttributes;
                            demandModel.HoursToCoverDistance = _dr.HoursToCoverDistance;
                            splitDrsDetails.push(demandModel);
                        }
                    }
                });
            }
            return splitDrsDetails;
        }
    }
    private getCustomerLocationDemands(jobId: number, regionId: string, customer: string): void {
        this._loadingProducts = true;
        this.showTankFlag = false;
        this.tankListForLocation = [];
        this.sbService.getCustomerLocationDemands(jobId, regionId).subscribe(data => {
            if (data == null) {
                Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            } else {
                if (data.length > 0) {
                    this.IsTankExists = true;
                    this.tankListForLocation = data as DemandModel[];
                    data = this.updateScheduleQtyType_Tank(data);
                    this.processDemands(data, regionId, true);
                } else {
                    this.IsTankExists = false;
                    let customerObj = this.CustomerList.find(x => x.CompanyName == customer);
                    let customerId = customerObj ? customerObj.CompanyId : 0;
                    this.getOrdersForJob(jobId, customerId, regionId);
                }
            }
        });
    }

    private getOrdersForJob(_jobId: any, _customerId: any, regionId: string): void {
        this._loadingProducts = true;
        var skipMarineConversion = true;
        this.carrierService.getOrdersForJob(_jobId, _customerId, regionId, skipMarineConversion, 1).subscribe(data => {
            if (data == null) {
                Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            } else {
                this.OrderList = data.OrderList;
                this.SelectedOrders = data.OrderList;
                if (data.DeliveryReqInput != null) {
                    data.DeliveryReqInput = this.updateScheduleQtyType(data.DeliveryReqInput);
                }
                this.processDemands(data.DeliveryReqInput, regionId, false);
                this.filterRecurringDROrders(data); // This is to show selected orders of recurring DRs only.
            }
            this._loadingProducts = false;
        });
    }

    private filterRecurringDROrders(data: any): void {
        if (this.DemandModels.find(x => x.isRecurringSchedule) != undefined) {
            this.SelectedOrders = [];
            this.DemandModels.forEach(x => {
                let order = data.OrderList.find(y => y.Id == x.OrderId);
                if (order) {
                    this.SelectedOrders.push(order);
                }
            });
            this.changeDetectorRef.detectChanges();
        }
    }

    public OnOrderChange($event, isIndivisual: boolean, isItemAdded: boolean): void {
        let ids: number[];
        let data: DemandModel[] = [];
        ids = this.getSelectedOrderDetails($event, isIndivisual, isItemAdded);
        if (ids.length > 0) {
            data = this.DemandModels.filter(t => ids.indexOf(t.OrderId) != -1);
        }
        let _array = this.LoadForm.controls['Demands'] as FormArray;
        this.utilService.initCreateLoadFormArray(_array, data, this.CreateLoadInputData.RegionId);
        this.changeDetectorRef.detectChanges();
        this.CreateLoadFormInitData = JSON.stringify(_array.value);
        this.disabledSaveButtonFunc(data);
    }

    public onSubmit(): void {
        if (this.LoadForm.valid) {
            let demands = this.LoadForm.controls['Demands'].value;
            let demandsData = this.LoadForm.controls['Demands'].value;
            let modifiedData = JSON.stringify(demands);
            // Do not recreate delivery requests if there is no duplicate Drs for a product.
            if (this.CreateLoadFormInitData == modifiedData && (this.DeliveryRequests.length == demands.length || this.SplitDeliveryRequests.length == demands.length) && this.draggedDrs.length == demands.length) {
                this.setCreateLoadSuccess([]);
            } else {
                this._loadingProducts = true;
                demands = demands.filter(t => t.IsEndSupplier);
                let products = demands.map(m => m.ProductType);
                let drIds = this.DeliveryRequests.filter(t => products.indexOf(t.ProductType) >= 0 && t.isRecurringSchedule == false && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4)).map(m => m.Id);
                let recurringDrIds = this.RecurringDeliveryRequests.filter(t => products.indexOf(t.ProductType) >= 0).map(m => m.Id);
                demands = demands.filter(top => !top.isRecurringSchedule && (top.CarrierStatus == 0 || top.CarrierStatus == 3 || top.CarrierStatus == 4));
                let brokerDrIds = this.BrokeredDeliveryRequests.filter(t => products.indexOf(t.ProductType) >= 0).map(m => m.Id);
                let splitDrIds = this.SplitDeliveryRequests.filter(t => products.indexOf(t.ProductType) >= 0).map(m => m.Id);
                let inputData = { ExistingDrIds: drIds, Demands: demands, RecurringDrIds: recurringDrIds, BrokeredDRIds: brokerDrIds, SplitDRIds: splitDrIds, UpdateDemands: demandsData,skipMarineConversion:true };
                
                this.sbService.reCreateDeliveryRequests(inputData).subscribe(data => {
                    this._loadingProducts = false;
                    if (data == null) {
                        Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                    } else if (data.StatusCode == 0) {
                        this.restoreRemovedDrs(data.DeliveryRequests);
                        this.setCreateLoadSuccess(data.DeliveryRequests);
                        this.restoreRecurringRemovedDrs(this.DeletedRecurringDeliveryRequests);
                        this.restoreBrokeredRemovedDrs(this.DeletedBrokeredDeliveryRequests);
                        this.restoreSplitRemovedDrs(this.DeletedSplitDeliveryRequests);
                        if (this.RecurringDeliveryRequests.length > 0 || this.DeletedRecurringDeliveryRequests.length > 0 || this.BrokeredDeliveryRequests.length > 0 || this.DeletedBrokeredDeliveryRequests.length > 0 || this.DeletedSplitDeliveryRequests.length > 0) {
                            this.restoreRecurringRemovedDrs(this.DeletedDeliveryRequests);
                        }

                    } else {
                        Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    }
                    this.changeDetectorRef.detectChanges();
                });
            }
        }
    }

    private setCreateLoadSuccess(delReqs: any): void {
        this.closeCreateLoad();
        this.dataService.setCreateLoadSuccessSubject({
            RegionId: this.CreateLoadInputData.RegionId,
            Customer: this.CreateLoadInputData.Customer,
            JobId: this.CreateLoadInputData.JobId,
            JobName: this.CreateLoadInputData.JobName,
            Drs: delReqs,
            ShiftIndex: this.CreateLoadInputData.ShiftIndex,
            RowIndex: this.CreateLoadInputData.RowIndex,
            ColIndex: this.CreateLoadInputData.ColIndex
        });
    }

    private restoreRemovedDrs(succeededDrs: any): void {
        if (succeededDrs) {
            let removedDrs: DeliveryRequestViewModel[] = [];
            let removedRecurringDrs: DeliveryRequestViewModel[] = [];
            if (this.DeliveryRequests.length > 0) {
                this.DeliveryRequests.forEach(x => {
                    removedDrs.push(x);
                });

            }
            if (this.DeletedRecurringDeliveryRequests.length > 0) {
                this.DeletedRecurringDeliveryRequests.forEach(x => {
                    removedRecurringDrs.push(x);
                });

            }
            let sucessDRs: DeliveryRequestViewModel[] = [];
            let successRecord = succeededDrs.filter(x => x.isRecurringSchedule == false);
            successRecord.forEach(x => {
                sucessDRs.push(x);
            });
            if (sucessDRs.length > 0) {
                sucessDRs.forEach(x => {
                    removedDrs = removedDrs.filter(y => y.ProductType != x.ProductType);
                });
            }
            if (removedRecurringDrs.length > 0) {
                removedRecurringDrs.forEach(x => {
                    removedDrs.push(x);
                });
            }
            if (removedDrs.length > 0) {
                // Restore the DRs which was unselected from order dropdown
                this.dataService.setCreateLoadCancelSubject({
                    RegionId: this.CreateLoadInputData.RegionId,
                    Customer: this.CreateLoadInputData.Customer,
                    JobId: this.CreateLoadInputData.JobId,
                    JobName: this.CreateLoadInputData.JobName,
                    Drs: removedDrs,
                    ShiftIndex: this.CreateLoadInputData.ShiftIndex,
                    RowIndex: this.CreateLoadInputData.RowIndex,
                    ColIndex: this.CreateLoadInputData.ColIndex
                });
            }
        }
    }
    private restoreRecurringRemovedDrs(succeededDrs: any): void {
        if (succeededDrs) {
            if (succeededDrs.length > 0) {
                // Restore the DRs which was unselected from order dropdown
                this.dataService.setCreateLoadCancelSubject({
                    RegionId: this.CreateLoadInputData.RegionId,
                    Customer: this.CreateLoadInputData.Customer,
                    JobId: this.CreateLoadInputData.JobId,
                    JobName: this.CreateLoadInputData.JobName,
                    Drs: succeededDrs,
                    ShiftIndex: this.CreateLoadInputData.ShiftIndex,
                    RowIndex: this.CreateLoadInputData.RowIndex,
                    ColIndex: this.CreateLoadInputData.ColIndex
                });
            }
        }
    }
    private restoreBrokeredRemovedDrs(succeededDrs: any): void {
        if (succeededDrs) {
            if (succeededDrs.length > 0) {
                // Restore the DRs which was unselected from order dropdown
                this.dataService.setCreateLoadCancelSubject({
                    RegionId: this.CreateLoadInputData.RegionId,
                    Customer: this.CreateLoadInputData.Customer,
                    JobId: this.CreateLoadInputData.JobId,
                    JobName: this.CreateLoadInputData.JobName,
                    Drs: succeededDrs,
                    ShiftIndex: this.CreateLoadInputData.ShiftIndex,
                    RowIndex: this.CreateLoadInputData.RowIndex,
                    ColIndex: this.CreateLoadInputData.ColIndex
                });
            }
        }
    }
    private restoreSplitRemovedDrs(succeededDrs: any): void {
        if (succeededDrs) {
            if (succeededDrs.length > 0) {
                // Restore the DRs which was unselected from order dropdown
                this.dataService.setCreateLoadCancelSubject({
                    RegionId: this.CreateLoadInputData.RegionId,
                    Customer: this.CreateLoadInputData.Customer,
                    JobId: this.CreateLoadInputData.JobId,
                    JobName: this.CreateLoadInputData.JobName,
                    Drs: succeededDrs,
                    ShiftIndex: this.CreateLoadInputData.ShiftIndex,
                    RowIndex: this.CreateLoadInputData.RowIndex,
                    ColIndex: this.CreateLoadInputData.ColIndex
                });
            }
        }
    }
    private disabledSaveButtonFunc(array: any): void {
        if (array.length > 0 && array.filter(t => t.IsEndSupplier).length > 0) {
            this.disabledSaveButton = false;
        }
        else {
            this.disabledSaveButton = true;
        }
    }
    private getSelectedOrderDetails($event: any, isIndivisual: boolean, isItemAdded: boolean) {
        let ids: number[];
        if (isIndivisual == false) {
            if ($event.length == 0) {
                ids = [];
            }
            else {
                this.SelectedOrders = $event as DropdownItem[];
                ids = this.SelectedOrders.map(t => t.Id);
            }
        }
        else {
            if (isItemAdded) {
                this.SelectedOrders.push($event as DropdownItem);
            }
            else {
                let index = this.SelectedOrders.findIndex(top => top.Id == $event.Id);
                if (index > -1) {
                    this.SelectedOrders.splice(index, 1);
                    this.SelectedOrders.slice();//refresh the SelectedOrders array
                }
            }
            ids = this.SelectedOrders.map(t => t.Id);
        }
        return ids;
    }
    private removeSelectedItem(removedItem: any) {
        if (!this.IsTankExists) {
            let selectedProductDetails = this.SelectedOrders.filter(x => x.Id != removedItem.controls['OrderId'].value) as DropdownItem[];
            if (selectedProductDetails.length > 0) {
                this.SelectedOrders = selectedProductDetails;
                this.SelectedOrders.slice();
            }
            else {
                this.SelectedOrders = [];
            }
        }
    }
    private updateScheduleQtyType(DeliveryReqInput: any): any {
        DeliveryReqInput.forEach(x => {
            var deliveryExists = this.DeliveryRequests.find(top => top.OrderId == x.OrderId && top.JobId == x.JobId && top.SiteId == x.SiteId);
            if (deliveryExists != null) {
                x.ScheduleQuantityType = deliveryExists.ScheduleQuantityType;
                x.ScheduleQuantityTypeText = deliveryExists.ScheduleQuantityTypeText;
            }
            else {
                x.ScheduleQuantityType = 1;
                x.ScheduleQuantityTypeText = "Quantity";
            }
        });
        return DeliveryReqInput;
    }
    private updateScheduleQtyType_Tank(DeliveryReqInput: any): any {
        DeliveryReqInput.forEach(x => {
            var deliveryExists = this.DeliveryRequests.find(top => ((top.StorageId == x.StorageId && top.TankId == x.TankId) || (top.DeliveryRequestFor == 3 && top.ProductTypeId == x.ProductTypeId)) && top.JobId == x.JobId);
            if (deliveryExists != null) {
                x.ScheduleQuantityType = deliveryExists.ScheduleQuantityType;
                x.ScheduleQuantityTypeText = deliveryExists.ScheduleQuantityTypeText;
            }
            else {
                x.ScheduleQuantityType = 1;
                x.ScheduleQuantityTypeText = "Quantity";
            }
        });
        return DeliveryReqInput;
    }
    public setRequiredQuantityValidators(qtyType: number, demand: FormGroup) {
        let quantityValidators = [];
        if (qtyType == 1) {
            quantityValidators = [Validators.required, Validators.min(0.00001), Validators.pattern(RegExConstants.DecimalNumber)];
            //if (demand.controls['TankMaxFill'].value > 0) {
            //    quantityValidators.push(Validators.max(demand.controls['TankMaxFill'].value));
            //}
            if (!(demand.controls["RequiredQuantity"].value > 0)) {
                demand.controls["RequiredQuantity"].setErrors({ 'required': true });
            }
        }
        else {
            demand.controls["RequiredQuantity"].setErrors(null)
        }
        demand.controls["RequiredQuantity"].setValidators(quantityValidators);
    }
}
