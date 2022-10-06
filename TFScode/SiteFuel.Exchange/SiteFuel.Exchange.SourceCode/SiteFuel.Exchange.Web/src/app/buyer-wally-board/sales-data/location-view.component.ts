import { Component, OnInit, Input, ViewChild, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { Declarations } from 'src/app/declarations.module';
import { SalesDataModel, DeliveryDetailsModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DataTableDirective } from 'angular-datatables';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { Subject, forkJoin, Subscription } from 'rxjs';
import { DipTestComponent } from 'src/app/shared-components/dip-test/dip-test.component';
import { FormGroup } from '@angular/forms';
import { DeliveryReqPriority, SelectedTabEnum } from 'src/app/app.enum';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-location-view',
    templateUrl: './location-view.component.html',
    styleUrls: ['./location-view.component.css']
})
export class LocationViewComponent implements OnInit {

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
    subscriptions: Subscription = new Subscription();
    public dtOptions: any = {};
    public SelectedLocations = [];
    SelectedPriorities = [];
    SelectedCarriers = [];
    IsShowCarrierManaged: boolean = false;
    public SelectedSuppliers = [];
    public SelectedStatus = [];
    IsShowRetailJobs: boolean = false;
    public SelectedPrioritiesId: any = [];
    public SelectedRegionId: string;
    public SelectedCustomerId: string;
    public SelectedLocationId: string;
    public dsModal = { modalDetails: { display: 'none', data: 'Modal Show', title: 'Delivery Schedule(s)', IsScheduled: false } };
    public isValid: boolean = true;
    public validateMsg: string;
    public isDataLoaded: boolean = false;
    public ScheduleQuantityTypes: any = [];
    @Input() FilterForm: FormGroup;
    @Input() IsFiltersLoaded: boolean = false;
    @Input() IsLoadSalesData: true;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    public SelectedTankRegionId: string = '';
    public SelectedCarrierIds: string;
    public SelectedStatusId: string;
    public SelectedSuppliersId: string;
    public SelectedLocArributeId: string;
    @Output()
    getJobIdsForMap: EventEmitter<any[]> = new EventEmitter<any[]>();
    constructor(private dispatcherService: DispatcherService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.initializeGrid();
        // this.getSalesData();
        this.getScheduleQuantityType();
        this.subscribeFormChanges();

    }

    subscribeFormChanges() {
        this.subscriptions.add(this.FilterForm.valueChanges
            .subscribe(change => {
                if (this.IsLoadSalesData) {
                    let isFilterChanged = this.setFilterData();
                    // if ((isFilterChanged || !this.isDataLoaded) && this.IsFiltersLoaded) {
                    //     this.isDataLoaded = true;
                    //     this.getSalesData();
                    // }
                }
            }))
    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }
    ngOnChanges(change: SimpleChanges) {
        if (change.IsLoadSalesData && change.IsLoadSalesData.currentValue != change.IsLoadSalesData.previousValue) {
            this.IsLoadSalesData = change.IsLoadSalesData.currentValue;
        }
        if (change.IsFiltersLoaded && change.IsFiltersLoaded.currentValue != change.IsFiltersLoaded.previousValue) {
            this.IsFiltersLoaded = change.IsFiltersLoaded.currentValue;
        }
    }

    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
        this.unSubscribeFormChanges();
    }

    getScheduleQuantityType() {
        this.dispatcherService.GetBuyerScheduleQtyType().subscribe((SQT: any[]) => {
            this.ScheduleQuantityTypes = SQT || [];
        });
    }

    setFilterData() {
        let isFilterChanged = false;

        this.SelectedLocations = this.FilterForm.get('SelectedlocationList').value;
        var ids = [];
        this.SelectedLocations.forEach(res => { ids.push(res.Id) });
        var selectedLocationId = ids.join();
        if (this.SelectedLocationId != selectedLocationId) {
            this.SelectedLocationId = selectedLocationId;
            isFilterChanged = true;
        }

        if (this.IsShowCarrierManaged != this.FilterForm.get('IsShowCarrierManaged').value) {
            this.IsShowCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
            isFilterChanged = true;
            this.getSalesData();
        }
        this.SelectedCarriers = this.FilterForm.get('SelectedCarrierList').value;
        ids = [];
        this.SelectedCarriers.forEach(res => { ids.push(res.Id) });
        var selectedCarrierIds = ids.join();
        if (this.SelectedCarrierIds != selectedCarrierIds) {
            this.SelectedCarrierIds = selectedCarrierIds;
            isFilterChanged = true;
            this.getSalesData();
        }

        this.SelectedStatus = this.FilterForm.get('SelectedStatusList').value;
        ids = [];
        this.SelectedStatus.forEach(res => { ids.push(res.Id) });
        var selectedStatusId = ids.join();
        if (this.SelectedStatusId != selectedStatusId) {
            this.SelectedStatusId = selectedStatusId;
            isFilterChanged = true;
        }
        
        var selectedLocAttri = this.FilterForm.get('selectedLocAttributeList').value;
        ids = [];
        selectedLocAttri && selectedLocAttri.forEach(res => { ids.push(res.Id) });
        var SelectedLocArributeId = ids.join();
        if (this.SelectedLocArributeId != SelectedLocArributeId) {
            this.SelectedLocArributeId = SelectedLocArributeId;
            isFilterChanged = true;
        }

        this.SelectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;
        ids = [];
        this.SelectedSuppliers.forEach(res => { ids.push(res.Id) });
        var selectedSuppliersId = ids.join();
        if (this.SelectedSuppliersId != selectedSuppliersId) {
            this.SelectedSuppliersId = selectedSuppliersId;
            isFilterChanged = true;
        }

        var isShowRetailJobs = !this.FilterForm.get('IsShowAssetJobs').value;
        if (this.IsShowRetailJobs != isShowRetailJobs) {
            this.IsShowRetailJobs = isShowRetailJobs;
            isFilterChanged = true;
            this.getSalesData();
        }
        this.SelectedPriorities = this.FilterForm.get('SelectedPriorityList').value;
        ids = [];
        this.SelectedPriorities.forEach(res => { ids.push(res.Id) });
        var selectedPrioritiesId = ids.join();
        if (this.SelectedPrioritiesId != selectedPrioritiesId) {
            this.SelectedPrioritiesId = selectedPrioritiesId;
            isFilterChanged = true;
        }
        return isFilterChanged;
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
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
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
            Priority: this.SelectedPrioritiesId,
            LocationId: this.SelectedLocationId,
            SelectedTab: SelectedTabEnum.Location,
            Carriers: this.SelectedCarrierIds,
            IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
            IsShowRetailJobs: !this.FilterForm.get('IsShowAssetJobs').value,
            Suppliers: this.SelectedSuppliersId,
            InventoryCaptureTypeIds:this.SelectedLocArributeId
        };
        this.IsLoading = true;

        forkJoin([this.dispatcherService.getBuyerSalesData(inputs),
        this.dispatcherService.GetRaisedBuyerExceptions()])
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
        // this.dispatcherService.getBuyerSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
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
        this.dispatcherService.GetBuyerDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe((resp: DeliveryDetailsModel[]) => {
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
            this.isValid = true;
            this.dispatcherService.PostBuyerRaiseDeliveryRequest(raiseDr).subscribe((response) => {
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                    this.closeModal();
                } else {
                    Declarations.msgerror(response.StatusMessage, undefined, undefined);
                }
                this.closeModal();
                this.DRLoader = false;
            });
        }
    }

    public showTanks(row) {
        this.SelectedTankRegionId = row.RegionId;
        let salesDataModel = new SalesDataModel();
        salesDataModel.RegionId = row.RegionId;
        salesDataModel.SiteId = row.SiteId;
        salesDataModel.TankId = row.TankId;
        salesDataModel.StorageId = row.StorageId;
        salesDataModel.TfxJobId = row.TfxJobId;
        salesDataModel.LocationManagedType = row.LocationManagedType ? row.LocationManagedType: null;
        this.dipTestComponent.loadTankDR(salesDataModel);

    }
    public closeSidePanel() {
        closeSlidePanel();
    }
    public passJobIdsToMapData() {
        var jobsPriority = [];
        if (this.LocationSchedules) {        
            this.LocationSchedules.forEach(res => {
                if (!jobsPriority.find(t => t.TfxJobId == res.TfxJobId)) {
                    jobsPriority.push({TfxJobId: res.TfxJobId,Priority: res.Priority})
                }
            }
            );
            this.getJobIdsForMap.emit(jobsPriority);
        } else {
            this.getJobIdsForMap.emit(jobsPriority);
        }
    }
  
    public applyLoadsFilters(filterForm:FormGroup){
        this.FilterForm = filterForm;
        if (this.IsLoadSalesData) {
            let isFilterChanged = this.setFilterData();
            if ((isFilterChanged || !this.isDataLoaded) && this.IsFiltersLoaded) {
                this.getSalesData();
            }
        }
    }
}
