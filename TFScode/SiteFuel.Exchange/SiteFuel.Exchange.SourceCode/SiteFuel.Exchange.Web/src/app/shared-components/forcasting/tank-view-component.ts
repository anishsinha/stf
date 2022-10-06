import { Component, OnInit, Input, QueryList, ViewChildren, SimpleChanges, ViewChild, OnDestroy } from '@angular/core';
import { ForecastingTankViewModel, ForecastingInventoryViewModel, ForecastingEstimatedUsageViewModel, ForecastingDeliveryViewModel, ForecastingExistingScheduleViewModel, SalesDataModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DataTableDirective } from 'angular-datatables';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { forkJoin, merge, Subject, Subscription } from 'rxjs';
import { Declarations } from 'src/app/declarations.module';
import { DipTestComponent } from '../dip-test/dip-test.component';
import { ForecastingLocationFilter } from 'src/app/dispatcher/dispatcher.model';
import { FormGroup } from '@angular/forms';
import { LocationTankDetailsModel, WallyUtilService } from 'src/app/carrier/service/wally-utility.service';
declare var IsBuyerCompany: boolean;
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-forecasting-tank-view',
    templateUrl: './tank-view-component.html',
    styleUrls: ['./tank-view-component.css']
})
export class ForcastingTankViewComponent implements OnInit, OnDestroy {

    public LocationSchedules: any = [];
    public LocationDrpDwnList: LocationTankDetailsModel[] = [];
    public FilterLocationDrpDwnList: LocationTankDetailsModel[] = [];
    public ForecastingTankDetails: ForecastingTankViewModel[] = [];
    public ForecastingTankInventoryDetails: ForecastingInventoryViewModel[] = [];
    public ForecastingTankEstimatedUsageDetails: ForecastingEstimatedUsageViewModel[] = [];
    public ForecastingTankDeliveryDetails: ForecastingDeliveryViewModel[] = [];
    public ForecastingTankScheduleDetails: ForecastingExistingScheduleViewModel[] = [];
    public dtInventoryTrigger: Subject<any> = new Subject();
    public dtInventoryOptions: any = {};
    public dtEstimatedTrigger: Subject<any> = new Subject();
    public dtEstimatedOptions: any = {};
    public dtDeliveryTrigger: Subject<any> = new Subject();
    public dtDeliveryOptions: any = {};
    public dtScheduleTrigger: Subject<any> = new Subject();
    public dtScheduleOptions: any = {};
    public exportInvitedColumns = { columns: ':visible' };
    public MinInputDate: Date = new Date();
    public MaxInputDate: Date = new Date();
    public IsInventoryLoading: boolean = false;
    public IsEstimatedLoading: boolean = false;
    public IsDeliveryLoading: boolean = false;
    public IsScheduleLoading: boolean = false;
    public IstankLoading: boolean = true;
    public SelectedTankRegionId: string = '';
    public StartDate: string = '';
    public EndDate: string = '';
    IsLocDrpDwnLoading = false;
    SelectedLocationId: string;
    SelectedTankId: string;
    SelectedTankIds: string;
    SelectedStorageId: string;
    SelectedSiteId: string;

    @Input() RequestFromBuyerWallyBoard: boolean = false;
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;

    filterArgs = { key: "DaysRemaining", asc: true };
    @Input() salesTabFilterForm: FormGroup;
    public applyFilterSubscription: Subscription[]= [];
    
    constructor(private dispatcherService: DispatcherService, private wallyUtilService: WallyUtilService) { }

    ngOnInit() {
        this.ForecastingTankDetails = [];
        this.MinInputDate.setFullYear(this.MaxInputDate.getFullYear() - 1);
        this.MaxInputDate.setFullYear(this.MaxInputDate.getFullYear() + 10);
        this.intializeTableDetails();

        this.applyFilterSubscription.push(merge(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(value => {
            if(this.salesTabFilterForm.get('RateOfConsumption').value){
                this.initLocationDropDown(1);
            }
        }));

        if(this.salesTabFilterForm.get('RateOfConsumption').value){
            this.initLocationDropDown(1);
        }
    }
    ngOnDestroy(){
        if (this.applyFilterSubscription) {
            this.applyFilterSubscription.forEach(subscription => {
                subscription.unsubscribe()
            });
        }
    }
    setSortArgs(key: string) {
        if (this.filterArgs.key == key) {
            this.filterArgs = { asc: !this.filterArgs.asc, key: key }
        }
        else {
            this.filterArgs = { asc: true, key: key }
        }
    }
    initLocationDropDown(isLocationLoad: number = 0) {
        this.IsLocDrpDwnLoading = true;
        this.LocationDrpDwnList = [];

        let filter: ForecastingLocationFilter = new ForecastingLocationFilter();

        if (this.RequestFromBuyerWallyBoard) {
            filter = {
                Carriers: '',
                CustomerIds: '',
                InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
                IsRateOfConsumption: this.salesTabFilterForm.get('RateOfConsumption').value,
                IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
                RegionId: '',
            };
        } else {
            filter = {
                Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
                CustomerIds: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
                InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
                IsRateOfConsumption: this.salesTabFilterForm.get('RateOfConsumption').value,
                IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
                RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value),
            };
        }

        if (IsBuyerCompany == false) {
            this.dispatcherService.getSupplierLocationTanks(filter).subscribe((result: any) => {
                if (result != null) {
                    this.intializeLocationDetails(result, isLocationLoad);
                }
            });
        }
        else {
            this.dispatcherService.getBuyerLocationTanks(filter).subscribe((result: any) => {
                if (result != null) {
                    this.intializeLocationDetails(result, isLocationLoad);
                }
            });
        }
    }
    Partsfiltering(inputName?) {       
        this.FilterLocationDrpDwnList = [];
        if (inputName && inputName.target && inputName.target.value && inputName.target.value.trim() != '') {
            let searchWord = inputName.target.value.toUpperCase();
            this.LocationDrpDwnList.forEach(element => {
                if (element.LocationName.toUpperCase().indexOf(searchWord) !== -1) {
                    this.FilterLocationDrpDwnList.push(element);
                }
            });
        } else {
            this.FilterLocationDrpDwnList = this.LocationDrpDwnList;           
        }       
    }
    locationChange($event) {
        this.SelectedTankId = null;
        this.SelectedLocationId = $event.JobId;
        this.SelectedSiteId = $event.SiteId;
        this.SelectedTankIds = 'NONE';
        this.SelectedStorageId = 'NONE';
        this.StartDate = '';
        this.EndDate = '';
        this.ForecastingTankDetails = [];
        this.ForecastingTankEstimatedUsageDetails = [];
        this.getTankDetails(parseInt(this.SelectedLocationId));
        this.getTankInventoryDetails(parseInt(this.SelectedLocationId));
        this.getTankDeliveryDetails(parseInt(this.SelectedLocationId));
        this.getTankScheduleDetails(parseInt(this.SelectedLocationId));
    }
    tankChange($event) {
        this.StartDate = '';
        this.EndDate = '';
        this.SelectedTankId = $event.TankId + '_' + $event.StorageId;
        this.SelectedTankIds = $event.TankId;
        this.SelectedStorageId = $event.StorageId;
        this.ForecastingTankDetails = [];
        this.ForecastingTankEstimatedUsageDetails = [];
        this.getTankDetails(parseInt(this.SelectedLocationId), $event.TankId, $event.StorageId);
        this.getTankInventoryDetails(parseInt(this.SelectedLocationId), $event.TankId, $event.StorageId);
        this.getTankDeliveryDetails(parseInt(this.SelectedLocationId), $event.TankId, $event.StorageId);
        this.getTankScheduleDetails(parseInt(this.SelectedLocationId), $event.TankId, $event.StorageId);
    }
    getTankDetails(JobId: number, TankId: string = '', StorageId = '') {
        this.IstankLoading = true;
        this.IsInventoryLoading = true;
        this.IsDeliveryLoading = true;
        this.IsScheduleLoading = true;
        if (IsBuyerCompany == false) {
            this.dispatcherService.getForcastingTankDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingTankViewModel[]) => {
                this.ForecastingTankDetails = resp;             
                this.ForecastingTankDetails && this.ForecastingTankDetails.map(m => {
                    try {
                        this.FilterLocationDrpDwnList && this.FilterLocationDrpDwnList.filter(f => f.SiteId == m.SiteId).map(j => j.Tanks.find(f => f.TankId == m.TankId && f.StorageId == m.StorageId).TankInventoryDiffinHrs = m.TankInventoryDiffinHrs);
                    } catch (e) {
                        console.log(e);
                    }

                })
                this.IstankLoading = false;
            });
        }
        else {
            this.dispatcherService.getBuyerForcastingTankDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingTankViewModel[]) => {
                this.ForecastingTankDetails = resp;
                this.ForecastingTankDetails && this.ForecastingTankDetails.map(m => {
                    try {
                        this.FilterLocationDrpDwnList && this.FilterLocationDrpDwnList.filter(f => f.SiteId == m.SiteId).map(j => j.Tanks.find(f => f.TankId == m.TankId && f.StorageId == m.StorageId).TankInventoryDiffinHrs = m.TankInventoryDiffinHrs);
                    } catch (e) {
                        console.log(e);
                    }

                })
                this.IstankLoading = false;
            });
        }
    }
    getTankInventoryDetails(JobId: number, TankId: string = '', StorageId = '') {
        this.IsInventoryLoading = true;
        if (IsBuyerCompany == false) {
            this.dispatcherService.getForcastingTankInventoryDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingInventoryViewModel[]) => {
                this.ForecastingTankInventoryDetails = resp;
                this.IsInventoryLoading = false;
                this.datatableInventoryRerender();
            });
        }
        else {
            this.dispatcherService.getBuyerForcastingTankInventoryDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingInventoryViewModel[]) => {
                this.ForecastingTankInventoryDetails = resp;
                this.IsInventoryLoading = false;
                this.datatableInventoryRerender();
            });
        }
    }
    getTankEstimatedUsageDetails(JobId: number, TankId: string = '', StorageId = '') {
        if (this.StartDate && this.StartDate != '' && this.EndDate && this.EndDate != '') {
            this.IsEstimatedLoading = true;
            if (IsBuyerCompany == false) {
                this.dispatcherService.getForcastingTankEstimatedUsageDetails(JobId, TankId, StorageId, this.StartDate, this.EndDate).subscribe((resp: ForecastingEstimatedUsageViewModel[]) => {
                    this.ForecastingTankEstimatedUsageDetails = resp;
                    this.IsEstimatedLoading = false;
                    this.datatableEstimatedRerender();
                });
            }
            else {
                this.dispatcherService.getBuyerForcastingTankEstimatedUsageDetails(JobId, TankId, StorageId, this.StartDate, this.EndDate).subscribe((resp: ForecastingEstimatedUsageViewModel[]) => {
                    this.ForecastingTankEstimatedUsageDetails = resp;
                    this.IsEstimatedLoading = false;
                    this.datatableEstimatedRerender();
                });
            }
        }
    }
    getTankDeliveryDetails(JobId: number, TankId: string = '', StorageId = '') {
        this.IsDeliveryLoading = true;
        if (IsBuyerCompany == false) {
            this.dispatcherService.getForcastingTankDeliveryDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingDeliveryViewModel[]) => {
                this.ForecastingTankDeliveryDetails = resp;
                this.IsDeliveryLoading = false;
                this.datatableDeliveryRerender();
            });
        }
        else {
            this.dispatcherService.getBuyerForcastingTankDeliveryDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingDeliveryViewModel[]) => {
                this.ForecastingTankDeliveryDetails = resp;
                this.IsDeliveryLoading = false;
                this.datatableDeliveryRerender();
            });
        }
    }
    getTankScheduleDetails(JobId: number, TankId: string = '', StorageId = '') {
        this.IsScheduleLoading = true;
        if (IsBuyerCompany == false) {
            this.dispatcherService.getForcastingTankScheduleDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingExistingScheduleViewModel[]) => {
                this.ForecastingTankScheduleDetails = resp;
                this.IsScheduleLoading = false;
                this.datatableScheduleRerender();
            });
        }
        else {
            this.dispatcherService.getBuyerForcastingTankScheduleDetails(JobId, TankId, StorageId).subscribe((resp: ForecastingExistingScheduleViewModel[]) => {
                this.ForecastingTankScheduleDetails = resp;
                this.IsScheduleLoading = false;
                this.datatableScheduleRerender();
            });
        }
    }
    intializeInventoryTable() {
        this.dtInventoryOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: this.exportInvitedColumns },
                { extend: 'csv', title: 'Inventory Information', exportOptions: this.exportInvitedColumns },
                { extend: 'pdf', title: 'Inventory Information', orientation: 'landscape', exportOptions: this.exportInvitedColumns },
                { extend: 'print', exportOptions: this.exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    intializeEstimatedUsageTable() {
        this.dtEstimatedOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: this.exportInvitedColumns },
                { extend: 'csv', title: 'Estimated Usage Information', exportOptions: this.exportInvitedColumns },
                { extend: 'pdf', title: 'Estimated Usage Information', orientation: 'landscape', exportOptions: this.exportInvitedColumns },
                { extend: 'print', exportOptions: this.exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    intializeDeliveryTable() {
        this.dtDeliveryOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: this.exportInvitedColumns },
                { extend: 'csv', title: 'Delivery Information', exportOptions: this.exportInvitedColumns },
                { extend: 'pdf', title: 'Delivery Information', orientation: 'landscape', exportOptions: this.exportInvitedColumns },
                { extend: 'print', exportOptions: this.exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    intializeScheduleTable() {
        this.dtScheduleOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: this.exportInvitedColumns },
                { extend: 'csv', title: 'Existing Schedule', exportOptions: this.exportInvitedColumns },
                { extend: 'pdf', title: 'Existing Schedule', orientation: 'landscape', exportOptions: this.exportInvitedColumns },
                { extend: 'print', exportOptions: this.exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    intializeTableDetails() {
        this.intializeInventoryTable();
        this.intializeEstimatedUsageTable();
        this.intializeDeliveryTable();
        this.intializeScheduleTable();
    }
    private datatableInventoryRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtInventoryTrigger.next();
            });
        }
    }
    private datatableEstimatedRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtEstimatedTrigger.next();
            });
        }
    }
    private datatableDeliveryRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtDeliveryTrigger.next();
            });
        }
    }
    private datatableScheduleRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtScheduleTrigger.next();
            });
        }
    }
    public searchEstimatedData() {
        if (this.StartDate && this.StartDate != '' && this.EndDate && this.EndDate != '') {
            this.IsEstimatedLoading = true;
            if (IsBuyerCompany == false) {
                this.dispatcherService.getForcastingTankEstimatedUsageDetails(this.SelectedLocationId, this.SelectedTankIds, this.SelectedStorageId, this.StartDate, this.EndDate).subscribe((resp: ForecastingEstimatedUsageViewModel[]) => {
                    this.ForecastingTankEstimatedUsageDetails = resp;
                    this.IsEstimatedLoading = false;
                    this.datatableEstimatedRerender();
                });
            }
            else {
                this.dispatcherService.getBuyerForcastingTankEstimatedUsageDetails(this.SelectedLocationId, this.SelectedTankIds, this.SelectedStorageId, this.StartDate, this.EndDate).subscribe((resp: ForecastingEstimatedUsageViewModel[]) => {
                    this.ForecastingTankEstimatedUsageDetails = resp;
                    this.IsEstimatedLoading = false;
                    this.datatableEstimatedRerender();
                });
            }
        }
        else {
            Declarations.msgerror('Provide valid input details.', undefined, undefined);
        }
    }
    public intializeLocationDetails(result: any, isLocationLoad: number = 0) {
        this.IsLocDrpDwnLoading = false;
        this.LocationDrpDwnList = result;
        this.Partsfiltering();
        this.LocationDrpDwnList && this.LocationDrpDwnList.length > 0 ? this.locationChange(this.LocationDrpDwnList[0]) : '';
        if (this.LocationDrpDwnList && this.LocationDrpDwnList.length > 0) {
            this.LocationDrpDwnList.forEach(loc => {
                loc && loc.Tanks && loc.Tanks.length > 0 && loc.Tanks.forEach(m => {
                    if (result && result.filter(f => f.Tanks.TankId == m.TankId && f.TankDetail.SiteId == loc.SiteId && f.TankDetail.StorageId == m.StorageId).length > 0)
                        m.IsUnknowDeliveryOrMissDelivery = true;
                    else
                        m.IsUnknowDeliveryOrMissDelivery = false;
                });
            });
            this.SelectedLocationId = this.LocationDrpDwnList[0].JobId.toString();
            if (isLocationLoad == 0) {
                this.getTankDetails(this.LocationDrpDwnList[0].JobId);
                this.getTankInventoryDetails(this.LocationDrpDwnList[0].JobId);
                this.getTankEstimatedUsageDetails(this.LocationDrpDwnList[0].JobId);
                this.getTankDeliveryDetails(this.LocationDrpDwnList[0].JobId);
                this.getTankScheduleDetails(this.LocationDrpDwnList[0].JobId);
            }
        }
    }
    setFromDate(event: any): void {
        this.StartDate = event;
    }

    setToDate(event: any): void {
        this.EndDate = event;
    }
    public showTanks(location?: any) {
        if(location){
            let row = this.ForecastingTankDetails[0];
            this.SelectedTankRegionId = row.RegionId;

            var salesDataModel = new SalesDataModel();
            salesDataModel.RegionId = row.RegionId;
            salesDataModel.SiteId = location.SiteId;
            salesDataModel.TankId = location.TankId;
            salesDataModel.StorageId = location.StorageId;
            salesDataModel.TfxJobId = parseInt(location.JobId);
            salesDataModel.LocationManagedType = location.LocationManagedType;
            this.dipTestComponent.loadTankDR(salesDataModel);
        }
        else if (this.ForecastingTankDetails.length > 0) {

            let row = this.ForecastingTankDetails[0];
            this.SelectedTankRegionId = row.RegionId;
            var salesDataModel = new SalesDataModel();
            salesDataModel.RegionId = row.RegionId;
            salesDataModel.SiteId = row.SiteId;
            salesDataModel.TankId = row.TankId;
            salesDataModel.StorageId = row.StorageId;
            salesDataModel.TfxJobId = parseInt(this.SelectedLocationId);
            salesDataModel.LocationManagedType = row.LocationManagedType;
            this.dipTestComponent.loadTankDR(salesDataModel);
        }
    }
    public closeSidePanel() {
        closeSlidePanel();
    }
}
