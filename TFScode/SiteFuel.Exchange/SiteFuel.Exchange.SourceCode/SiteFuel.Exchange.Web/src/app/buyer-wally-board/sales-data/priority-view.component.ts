import { Component, OnInit, ViewChildren, QueryList, Input, SimpleChanges, OnDestroy, OnChanges, ViewChild } from '@angular/core';
import { Subject, forkJoin, Subscription, merge } from 'rxjs';
import { DeliveryDetailsModel, SalesDataModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { SalesFilterModal } from '../Models/BuyerWallyBoard';
import { DataTableDirective } from 'angular-datatables';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { Declarations } from 'src/app/declarations.module';
import { DipTestComponent } from 'src/app/shared-components/dip-test/dip-test.component';
import { DeliveryReqPriority, SelectedTabEnum } from 'src/app/app.enum';
import { FormGroup } from '@angular/forms';
import { WallyUtilService } from 'src/app/carrier/service/wally-utility.service';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-priority-view',
    templateUrl: './priority-view.component.html',
    styleUrls: ['./priority-view.component.css']
})
export class PriorityViewComponent implements OnInit, OnDestroy {
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
    public SelectedTank: SalesDataModel;
    public dsModal = { modalDetails: { display: 'none', data: 'Modal Show', title: 'Delivery Schedule(s)', IsScheduled: false } };
    public isValid: boolean = true;
    public validateMsg: string;
    public ScheduleQuantityTypes: any = [];
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;


    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    public SelectedTankRegionId: string = '';

    @Input() salesTabFilterForm: FormGroup;
    public applyFilterSubscription: Subscription[]= [];


    constructor(private dispatcherService: DispatcherService, private wallyUtilService: WallyUtilService) {
    }

    ngOnInit() {
        this.init();
    }

    init() {

        this.applyFilterSubscription.push(merge(
            this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(value => {
                if(this.salesTabFilterForm.get('SalesViewType').value == 1){
                    this.getSalesData();
                }
         }));
        //to load data - after second ngOnInit
        if (this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value) {
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
            this.getSalesData();
        }

        this.initializeMustGo();
        this.initializeCouldGo();
        this.initializeShouldGo();
        this.getScheduleQuantityType();
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
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details-CouldGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details-CouldGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
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
            pagingType: 'first_last_numbers',
            fixedHeader: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            columnDefs: [
                { 
                    targets: 13,
                    type:  'null-at-bottom',
                },
            ],
            order: [13]
        };

    }
    getScheduleQuantityType() {
        this.dispatcherService.GetBuyerScheduleQtyType().subscribe((SQT: any[]) => {
            this.ScheduleQuantityTypes = SQT || [];
        });
    }
    getSalesDtls() {
        let inputs = {
            Priority: DeliveryReqPriority.None,
            LocationId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedlocationList').value),
            SelectedTab: SelectedTabEnum.Priority,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureTypeIds: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
        };
        this.IsShouldGoLoading = true;
        this.IsCouldGoLoading = true;
        this.IsMustGoLoading = true;
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
                this.ShouldGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.ShouldGo) as SalesDataModel[];
                this.CouldGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.CouldGo) as SalesDataModel[];
                this.MustGoSchedules = await resp[0] && resp[0].filter(t => t.Priority == DeliveryReqPriority.MustGo) as SalesDataModel[];
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
        this.IsCouldGoLoading = true;
        this.IsShouldGoLoading = true;
        this.IsMustGoLoading = true;
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
        this.dispatcherService.GetBuyerDeliveryDetails(row.TfxJobId, row.ProductTypeId).subscribe((resp: DeliveryDetailsModel[]) => {
            this.ExistingDeliveries = resp;
            this.DRLoader = false;
        });
        this.dsModal.modalDetails.display = 'block';
        let isSchedule = (row.Status == 'Scheduled');
        this.dsModal.modalDetails.IsScheduled = isSchedule;
        this.showDr = isSchedule;
    }
    public resetModal() {
        this.ExistingDeliveries = [];
        this.DrPriority = DeliveryReqPriority.MustGo;
        this.RequiredQuantity = null;
        this.ScheduleQuantityType = 1;
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
        if (this.ScheduleQuantityType == 1 && (this.RequiredQuantity == null || this.RequiredQuantity == 0)) {
            Declarations.msgerror("Invalid required quantity.", undefined, undefined);
            this.isValid = false;
        }
        else if ((this.SelectedTank.MaxFillQuantity != null && this.RequiredQuantity > this.SelectedTank.MaxFillQuantity)) {
            Declarations.msgerror("Quantity Should be less than max fill quantity: " + this.SelectedTank.MaxFillQuantity, undefined, undefined);
            this.isValid = false;
        }
        else {
            this.DRLoader = true;
            this.isValid = true;
            this.dispatcherService.PostBuyerRaiseDeliveryRequest(raiseDr).subscribe((response) => {
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                } else {
                    Declarations.msgerror(response.StatusMessage, undefined, undefined);
                }
                this.DRLoader = false;
                this.closeModal();
                $(".modal-backdrop").removeClass("show");
                $(".modal-backdrop").addClass("hide");

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
}
