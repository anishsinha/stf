import { Component, OnInit, ViewChildren, QueryList, SimpleChanges, Input, OnDestroy, OnChanges, ViewChild, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { Subject, forkJoin } from 'rxjs';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { SalesDataModel, DeliveryDetailsModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DataTableDirective } from 'angular-datatables';
import { Declarations } from '../../../declarations.module';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DipTestComponent } from '../../../shared-components/dip-test/dip-test.component';
import { LocationFilterModal } from '../../dispatcher.model';
import { DeliveryReqPriority, SelectedTabEnum } from 'src/app/app.enum';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-location-view',
    templateUrl: './location-view.component.html',
    styleUrls: ['./location-view.component.css']
})
export class LocationViewComponent implements OnInit, OnChanges, OnDestroy {
    public LocationSchedules: any = [];
    IsLoading = false;
    public showDr = false;
    public IsDrExists = false;
    public DRLoader = false;
    public ExistingDeliveries: DeliveryDetailsModel[] = [];
    public DrPriority: DeliveryReqPriority = DeliveryReqPriority.MustGo;
    public RequiredQuantity: number;
    public ScheduleQuantityType: number;
    public SelectedTank: SalesDataModel;
    public dtTrigger: Subject<any> = new Subject();
    public dtOptions: any = {};
    SelectedPrioritiesId: any = [];
    public SelectedRegionId: string;
    public SelectedTankRegionId: string;
    public SelectedCustomerId: string;
    public SelectedLocationId: string;
    public dsModal = { modalDetails: { display: 'none', data: 'Modal Show', title: 'Delivery Schedule(s)', IsScheduled: false } };
    public isValid: boolean = true;
    public validateMsg: string;
    public IsDataLoaded: boolean = false;
    public ScheduleQuantityTypes: any = [];
    @Input() public IsFilterLoaded: boolean = false;
    @Input() public SelectedCustomers = [];
    @Input() public SelectedLocations = [];
    @Input() public SelectedRegions = [];
    @Input() public SelectedPriorities = [];
    @Input() public SelectedCarriers = [];
    @Input() public IsShowCarrierManaged: boolean = false;
    @Input() public SelectedStatus = [];
    @Input() public IsShowRetailJobs: boolean = false;
    @Input() selectedLocAttributeList = [];
    @Output() getJobIdsForMap: EventEmitter<any[]> = new EventEmitter<any[]>();
    public SelectedStatusId: string;
    public selectedLocAttributeId: string;
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    SelectedCarrierIds: string;
    constructor(private dispatcherService: DispatcherService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.initializeGrid();
        this.getScheduleQuantityType();
        // this.getSalesData();

    }

    ngOnChanges(change: SimpleChanges) {
        var isFilterData = false;
        // if (change.IsFilterLoaded && change.IsFilterLoaded.currentValue) {
        //     this.IsFilterLoaded = change.IsFilterLoaded.currentValue;
        // }
        // if (change.SelectedRegions && change.SelectedRegions.currentValue) {
        //     this.SelectedRegions = change.SelectedRegions.currentValue;
        //     var ids = [];
        //     this.SelectedRegionId = '';
        //     this.SelectedRegions.forEach(res => { ids.push(res.Id) });
        //     this.SelectedRegionId = ids.join();
        //     if (change.SelectedRegions.previousValue) {
        //         var previousIds = [];
        //         change.SelectedRegions.previousValue.forEach(res => { previousIds.push(res.Id) });
        //         var previousRegionIds = previousIds.join();
        //         if (this.SelectedRegionId != previousRegionIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        // if (change.SelectedCustomers && change.SelectedCustomers.currentValue) {
        //     this.SelectedCustomers = change.SelectedCustomers.currentValue;
        //     var ids = [];
        //     this.SelectedCustomerId = '';
        //     this.SelectedCustomers.forEach(res => { ids.push(res.CompanyId) });
        //     this.SelectedCustomerId = ids.join();
        //     if (change.SelectedCustomers.previousValue) {
        //         var previousIds = [];
        //         change.SelectedCustomers.previousValue.forEach(res => { previousIds.push(res.CompanyId) });
        //         var previousCustomerIds = previousIds.join();
        //         if (this.SelectedCustomerId != previousCustomerIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        // if (change.SelectedLocations && change.SelectedLocations.currentValue) {
        //     this.SelectedLocations = change.SelectedLocations.currentValue;
        //     var ids = [];
        //     this.SelectedLocationId = '';
        //     this.SelectedLocations.forEach(res => { ids.push(res.Id) });
        //     this.SelectedLocationId = ids.join();
        //     if (change.SelectedLocations.previousValue) {
        //         var previousIds = [];
        //         change.SelectedLocations.previousValue.forEach(res => { previousIds.push(res.Id) });
        //         var previousLocationIds = previousIds.join();
        //         if (this.SelectedLocationId != previousLocationIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        if (change.IsShowCarrierManaged && change.IsShowCarrierManaged.currentValue != change.IsShowCarrierManaged.previousValue) {
            this.IsShowCarrierManaged = change.IsShowCarrierManaged.currentValue;
            isFilterData = true;
        }
        if (change.SelectedCarriers && change.SelectedCarriers.currentValue) {
            this.SelectedCarriers = change.SelectedCarriers.currentValue;
            var ids = [];
            this.SelectedCarrierIds = '';
            this.SelectedCarriers.forEach(res => { ids.push(res.Id) });
            this.SelectedCarrierIds = ids.join();
            if (change.SelectedCarriers.previousValue) {
                var previousIds = [];
                change.SelectedCarriers.previousValue.forEach(res => { previousIds.push(res.Id) });
                var previousCarrierIds = previousIds.join();
                if (this.SelectedCarrierIds != previousCarrierIds) {
                    isFilterData = true;
                }
            } else {
                isFilterData = true;
            }
        }
        // if (change.SelectedStatus && change.SelectedStatus.currentValue) {
        //     this.SelectedStatus = change.SelectedStatus.currentValue;
        //     var ids = [];
        //     this.SelectedStatusId = '';
        //     this.SelectedStatus.forEach(res => { ids.push(res.Id) });
        //     this.SelectedStatusId = ids.join();
        //     if (change.SelectedStatus.previousValue) {
        //         var previousIds = [];
        //         change.SelectedStatus.previousValue.forEach(res => { previousIds.push(res.Id) });
        //         var previousStatusIds = previousIds.join();
        //         if (this.SelectedStatusId != previousStatusIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        // if (change.SelectedPriorities && change.SelectedPriorities.currentValue) {
        //     this.SelectedPriorities = change.SelectedPriorities.currentValue;
        //     var ids = [];
        //     this.SelectedPrioritiesId = '';
        //     this.SelectedPriorities.forEach(res => { ids.push(res.Id) });
        //     this.SelectedPrioritiesId = ids.join();
        //     if (change.SelectedPriorities.previousValue) {
        //         var previousIds = [];
        //         change.SelectedPriorities.previousValue.forEach(res => { previousIds.push(res.Id) });
        //         var previousPriorityIds = previousIds.join();
        //         if (this.SelectedPrioritiesId != previousPriorityIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        // if (change.selectedLocAttributeList && change.selectedLocAttributeList.currentValue) {
        //     var selectedLocAttributeList = change.selectedLocAttributeList.currentValue;
        //     var ids = [];
        //     this.selectedLocAttributeId = '';
        //     selectedLocAttributeList.forEach(res => { ids.push(res.Id) });
        //     this.selectedLocAttributeId = ids.join();
        //     if (change.selectedLocAttributeList.previousValue) {
        //         var previousIds = [];
        //         change.selectedLocAttributeList.previousValue.forEach(res => { previousIds.push(res.Id) });
        //         var previousLocAttributeIds = previousIds.join();
        //         if (this.selectedLocAttributeId != previousLocAttributeIds) {
        //             isFilterData = true;
        //         }
        //     } else {
        //         isFilterData = true;
        //     }
        // }
        if (change.IsShowRetailJobs && change.IsShowRetailJobs.currentValue != change.IsShowRetailJobs.previousValue) {
            this.IsShowRetailJobs = change.IsShowRetailJobs.currentValue;
            isFilterData = true;
        }
        if ((isFilterData || !this.IsDataLoaded) && this.IsFilterLoaded) {
            this.IsDataLoaded = true;
            this.getSalesData();
        }
    }


    ngOnDestroy(): void {
        if(this.dtTrigger)
        this.dtTrigger.unsubscribe();
    }
    // regionChanged(event?: any): void {
    //     this.filterData();
    // }

    // public onRegionSelect() {
    //     var ids = [];
    //     this.SelectedRegionId = '';
    //     this.SelectedRegions.forEach(res => { ids.push(res.Id) });
    //     this.SelectedRegionId = ids.join();
    //     this.regionChanged();

    // }

    // customerChanged(): void {
    //     this.filterData();
    // }

    // onPrioritySelect(event: any): void {
    //     this.filterData();
    // }

    // onPriorityUnselect(event: any): void {
    //     this.filterData();
    // }

    // filterData(): void {
    //     this.getSalesData();
    // }

    getScheduleQuantityType() {
        this.dispatcherService.GetScheduleQtyType().subscribe((SQT: any[]) => {
            this.ScheduleQuantityTypes = SQT || [];
        });
    }

    initializeGrid() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            fixedHeader: false,
            columnDefs: [
                { 
                    targets: 13,
                    type:  'null-at-bottom',
                },
            ],
            //order: [13]
        };
    }


    public getSalesData() {
        let inputs = {
            RegionId: this.SelectedRegionId,
            Priority: this.SelectedPrioritiesId,
            CustomerId: this.SelectedCustomerId,
            LocationId: this.SelectedLocationId,
            SelectedTab: SelectedTabEnum.Location,
            Carriers: this.SelectedCarrierIds,
            IsShowCarrierManaged: this.IsShowCarrierManaged,
            IsShowRetailJobs: this.IsShowRetailJobs,
            InventoryCaptureType: this.selectedLocAttributeId
        };
        this.IsLoading = true;
        forkJoin([this.dispatcherService.getSalesData(inputs),
        this.dispatcherService.GetRaisedExceptions()])
            .subscribe(async (resp) => {
                await resp[0] && resp[0].map(m => {
                    if (resp[1] && resp[1].filter(f => f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == m.SiteId && f.TankDetail.StorageId == m.StorageId).length > 0) {
                        m.IsUnknownOrMissing = true;
                    }
                    else
                        m.IsUnknownOrMissing = false;
                })
                if (this.SelectedStatus && this.SelectedStatus.length && resp[0]) {
                    resp[0] = resp[0].filter(t => this.SelectedStatusId.includes(t.Status))
                }
                this.LocationSchedules = resp[0];
                this.passJobIdsToMapData();
                this.IsLoading = false;
                this.datatableRerender();
            });

        // this.dispatcherService.getSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
        //   this.LocationSchedules = resp;
        //   this.IsLoading = false;
        //   this.datatableRerender();
        // });
    }

    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
        }
        this.dtTrigger.next();
    }

    public openModal(row: SalesDataModel) {
        this.resetModal();
        this.SelectedTank = row;
        this.DRLoader = true;
        this.dispatcherService.GetDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe((resp: DeliveryDetailsModel[]) => {
            this.ExistingDeliveries = resp;
            this.DRLoader = false;
        });
        this.dsModal.modalDetails.display = 'block';
        let isSchedule = (row.Status == 'Scheduled');
        this.dsModal.modalDetails.IsScheduled = isSchedule;
        this.showDr = isSchedule;
        //this.MaxFillQuantity = 120;
    }
    public resetModal() {
        this.ExistingDeliveries = [];
        this.DrPriority = DeliveryReqPriority.MustGo;
        this.RequiredQuantity = null;
        this.ScheduleQuantityType = 1;
        this.validateMsg = "";
        this.isValid = true;
    }

    public closeModal() {
        this.dsModal.modalDetails.display = 'none';
        this.isValid = true;
        $(".modal-backdrop").hide();
        $('body').removeClass('modal-open');
    }

    public toggleDrs() {
        this.showDr = !this.showDr
    }

    public onDrSubmit() {
        this.validateMsg = "";
        this.isValid = true;
        let raiseDr = {
            SiteId: this.SelectedTank.SiteId,
            TankId: this.SelectedTank.TankId,
            StorageId: this.SelectedTank.StorageId,
            RequiredQuantity: this.ScheduleQuantityType == 1 ? this.RequiredQuantity : 0,
            ScheduleQuantityType: this.ScheduleQuantityType,
            JobId: this.SelectedTank.TfxJobId,
            FuelTypeId: this.SelectedTank.ProductTypeId,
            Priority: this.DrPriority
        };
        if (this.ScheduleQuantityType == 1 && (!(this.RequiredQuantity > 0) || this.RequiredQuantity < 0.00001)) {
            this.validateMsg = "Invalid required quantity."; this.isValid = false;
        }
        else if (this.ScheduleQuantityType == 1 && this.SelectedTank.MaxFillQuantity && this.SelectedTank.MaxFillQuantity > 0 && this.RequiredQuantity > this.SelectedTank.MaxFillQuantity) {
            this.validateMsg = "Should not exceed max fill. (" + this.SelectedTank.MaxFillQuantity + ")"; this.isValid = false;
        }
        else {
            this.DRLoader = true;
            this.dispatcherService.PostRaiseDeliveryRequest(raiseDr).subscribe((response) => {
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                } else {
                    Declarations.msgerror(response.StatusMessage, undefined, undefined);
                }
                this.DRLoader = false;
                this.closeModal();
            });
        }
    }

    public closeSidePanel() {
        closeSlidePanel();
    }

    public showTanks(row: SalesDataModel) {
        this.SelectedTankRegionId = row.RegionId;
        this.dipTestComponent.loadTankDR(row);
    }

    public passJobIdsToMapData() {
        var jobsPriority = [];
        if (this.LocationSchedules) {
            this.LocationSchedules.forEach(res => {
                if (!jobsPriority.find(t => t.TfxJobId == res.TfxJobId)) {
                    jobsPriority.push({ TfxJobId: res.TfxJobId, Priority: res.Priority })
                }
            });
            this.getJobIdsForMap.emit(jobsPriority);
        } else {
            this.getJobIdsForMap.emit(jobsPriority);
        }
    }

    public applyFilters(locationFilterModal:LocationFilterModal){
        if(locationFilterModal){
            this.SelectedRegionId = locationFilterModal.SelectedRegionId;
            this.SelectedCustomerId = locationFilterModal.SelectedCustomerId;
            this.SelectedLocationId = locationFilterModal.SelectedLocationId;
            this.SelectedStatusId = locationFilterModal.SelectedStatusId;
            this.SelectedPrioritiesId = locationFilterModal.SelectedPrioritiesId;
            this.selectedLocAttributeId = locationFilterModal.selectedLocAttributeId;
            this.getSalesData();
        }
    }
}