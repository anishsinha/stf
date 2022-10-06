import { Component, OnInit, ViewChildren, QueryList, SimpleChanges, Input, OnDestroy, OnChanges, ViewChild, AfterViewInit } from '@angular/core';
import { Subject, forkJoin, Subscription, merge } from 'rxjs';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { SalesDataModel, DeliveryDetailsModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DataTableDirective } from 'angular-datatables';
import { Declarations } from '../../../declarations.module';
import { DipTestComponent } from '../../../shared-components/dip-test/dip-test.component';
import { DeliveryReqPriority, SelectedTabEnum } from 'src/app/app.enum';
import { WallyUtilService } from 'src/app/carrier/service/wally-utility.service';
import { FormGroup } from '@angular/forms';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-grid-view',
    templateUrl: './grid-view.component.html',
    styleUrls: ['./grid-view.component.css']
})
export class GridViewComponent implements OnInit, OnDestroy {

    public MustGoSchedules: any = [];
    public ShouldGoSchedules: any = [];
    public CouldGoSchedules: any = [];
    public dtMustGoOptions: any = {};
    public dtShouldGoOptions: any = {};
    public dtCouldGoOptions: any = {};
    public dtMustGoTrigger: Subject<any> = new Subject();
    public dtShouldGoTrigger: Subject<any> = new Subject();
    public dtCouldGoTrigger: Subject<any> = new Subject();
    public IsShouldGoLoading: boolean;
    public IsCouldGoLoading: boolean;
    public IsMustGoLoading: boolean;
    public showDr = false;
    public IsDrExists = false;
    public DRLoader = false;
    public ExistingDeliveries: DeliveryDetailsModel[] = [];
    public DrPriority: DeliveryReqPriority = DeliveryReqPriority.MustGo;
    public RequiredQuantity: number;
    public ScheduleQuantityType: number;
    public SelectedTankRegionId: string;

    public SelectedTank: SalesDataModel;
    public dsModal = { modalDetails: { display: 'none', data: 'Modal Show', title: 'Delivery Schedule(s)', IsScheduled: false } };
    public isValid: boolean = true;
    public validateMsg: string;
    public ScheduleQuantityTypes: any = [];

    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;

    loadingData = false;
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    @Input() salesTabFilterForm: FormGroup;
    public applyFilterSubscription: Subscription[] = [];


    constructor(private dispatcherService: DispatcherService, private wallyUtilService: WallyUtilService) {
    }

    ngOnInit() {
        this.applyFilterSubscription.push(merge(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(value => {
            this.bindPriorityArray();
            this.getSalesData();
        }));

        //to load data - after second ngOnInit
        if(this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value){
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
            this.bindPriorityArray();
            this.getSalesData();
        }

        this.init();
        this.getScheduleQuantityType();
    }

    init() {
        this.initializeMustGo();
        this.initializeCouldGo();
        this.initializeShouldGo();
        // this.getSalesData();
    }
    getScheduleQuantityType() {
        this.dispatcherService.GetScheduleQtyType().subscribe((SQT: any[]) => {
            this.ScheduleQuantityTypes = SQT || [];
        });
    }



    ngOnDestroy(): void {
        this.dtCouldGoTrigger.unsubscribe();
        this.dtShouldGoTrigger.unsubscribe();
        this.dtMustGoTrigger.unsubscribe();
        if (this.applyFilterSubscription) {
            this.applyFilterSubscription.forEach(subscription => {
                subscription.unsubscribe()
            });
        }
    }

    columnsDetails = [
        { data: 'Cust', "autoWidth": true },
        { data: 'LocName', "autoWidth": true },
        { data: 'Loc', "autoWidth": true },
        { data: 'TName', "autoWidth": true },
        { data: 'Avg7Day', "autoWidth": true },
        { data: 'PDS', "autoWidth": true },
        { data: 'CI', "autoWidth": true },
        { data: 'Ullg', "autoWidth": true },
        { data: 'lastDelivery', "autoWidth": true },
        { data: 'lastDeliveryQty', "autoWidth": true },
        { data: 'DRemg', "autoWidth": true }
    ];

    initializeMustGo() {

        let exportInvitedColumns = { columns: ':visible' };
        this.dtMustGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details-MustGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details-MustGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            // columns: this.columnsDetails,
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

    initializeCouldGo() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtCouldGoOptions = {
            colReorder: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details-CouldGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details-CouldGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            // columns: this.columnsDetails,
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

    initializeShouldGo() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtShouldGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details-ShouldGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details-ShouldGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            // columns: this.columnsDetails,
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

    getSalesDtls() {
        let inputs = {
            RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value),
            Priority: DeliveryReqPriority.None,
            CustomerId: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
            LocationId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedlocationList').value),
            SelectedTab: SelectedTabEnum.Priority,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
            IsShowRetailJobs: ''
        };
        this.IsShouldGoLoading = true;
        this.IsCouldGoLoading = true;
        this.IsMustGoLoading = true;

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
                this.ShouldGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.ShouldGo && t.Inventory != '--') as SalesDataModel[];
                this.CouldGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.CouldGo && t.Inventory != '--') as SalesDataModel[];
                this.MustGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.MustGo && t.Inventory != '--') as SalesDataModel[];
                this.destroyDatatable();
                this.IsShouldGoLoading = false;
                this.IsCouldGoLoading = false;
                this.IsMustGoLoading = false;
                this.dtCouldGoTrigger.next();
                this.dtShouldGoTrigger.next();
                this.dtMustGoTrigger.next();

            });
    }

    filterData(): void {
        this.getSalesData();
    }

    getSalesData(): void {
        // let _priorities = []; 
        // this.SelectedPriorities.forEach(x => _priorities.push(x.Id));
        // this.SelectedPrioritiesId = _priorities;
        this.IsCouldGoLoading = true;
        this.IsShouldGoLoading = true;
        this.IsMustGoLoading = true;
        //this.destroyDatatable();

        this.getSalesDtls();
    }

    destroyDatatable(): void {
        if (this.dtElements) {
            this.dtElements.forEach((dtElement: DataTableDirective) => {
                if (dtElement.dtInstance) {
                    dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                        dtInstance.destroy();

                    });
                }
            });
        }
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

    public showTanks(row: SalesDataModel) {
        this.SelectedTankRegionId = row.RegionId;
        this.dipTestComponent.loadTankDR(row);
    }
    public closeSidePanel() {
        closeSlidePanel();
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
        if (this.ScheduleQuantityType == 1 && (this.RequiredQuantity == null || this.RequiredQuantity == 0 || this.RequiredQuantity < 0.00001)) {
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
                    this.closeModal();

                } else {
                    Declarations.msgerror(response.StatusMessage, undefined, undefined);
                }
                this.DRLoader = false;
            });
        }
    }

    SelectedPrioritiesId: number[] = []
    bindPriorityArray() {
        this.SelectedPrioritiesId = [];
        let SelectedPriorities = this.salesTabFilterForm.get('SelectedPriorities').value as any[];
        SelectedPriorities.forEach(res => { this.SelectedPrioritiesId.push(res.Id) });
    }
}

