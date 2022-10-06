import { Component, OnInit, SimpleChanges, Input, ViewChildren, QueryList, ViewChild, OnChanges, OnDestroy } from '@angular/core';
import { Subject, forkJoin, merge, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { SalesDataModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { ForecastingLocationFilter } from 'src/app/dispatcher/dispatcher.model';
import { DipTestComponent } from 'src/app/shared-components/dip-test/dip-test.component';
import { Declarations } from 'src/app/declarations.module';
import { DeliveryReqPriority, SelectedTabEnum } from 'src/app/app.enum';
import { FormGroup } from '@angular/forms';
import { WallyUtilService } from 'src/app/carrier/service/wally-utility.service';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-buyertank-view',
    templateUrl: './tank-view.component.html',
    styleUrls: ['./tank-view.component.css']
})
export class TankViewComponent implements OnInit, OnDestroy {

    @Input() salesTabFilterForm: FormGroup;
    public applyFilterSubscription: Subscription[]= [];
    public LocationSchedules: any = [];
    CloneLocationSchedules = [];
    public LocationDrpDwnList: LocationTankDetailsModel[] = [];
    public FilterLocationDrpDwnList: LocationTankDetailsModel[] = [];
    IsLoading = false;
    IsLocDrpDwnLoading = false;

    public dtTrigger: Subject<any> = new Subject();
    public dtOptions: any = {};
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public SelectedRegionId: string;
    public SelectedCustomerId: string;
    SelectedLocationId: string;
    SelectedTankId: string;
    SelectedSiteId: string;

    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    filterArgs = { key: "DaysRemaining", asc: true };
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;

    constructor(private dispatcherService: DispatcherService, private wallyUtilService: WallyUtilService) {
    }

    ngOnInit() {
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
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        //  this.initLocationDropDown();
        this.applyFilterSubscription.push(merge(this.salesTabFilterForm.get('IsApplyFilter').valueChanges).subscribe(value => {
            this.initLocationDropDown();
        }));
        //to load data - after in ngOnInit
        if(this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').value){
            this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(false);
            this.initLocationDropDown();
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

    initLocationDropDown() {
        this.IsLocDrpDwnLoading = true;
        this.LocationDrpDwnList = [];

        let filter: ForecastingLocationFilter = {
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            CustomerIds: this.wallyUtilService.getCompanyIdsByList(this.salesTabFilterForm.get('SelectedCustomerList').value),
            InventoryCaptureType: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
            IsRateOfConsumption: this.salesTabFilterForm.get('RateOfConsumption').value,
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            RegionId: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value),
        };

        forkJoin([this.dispatcherService.getBuyerLocationTanks(filter), this.dispatcherService.GetRaisedBuyerExceptions()])
            .subscribe(result => {
                this.IsLocDrpDwnLoading = false;
                this.LocationDrpDwnList = result[0];
                this.Partsfiltering();
                this.LocationDrpDwnList && this.LocationDrpDwnList.length > 0 ? this.locationChange(this.LocationDrpDwnList[0]) : '';
                if (this.LocationDrpDwnList && this.LocationDrpDwnList.length > 0) {
                    this.LocationDrpDwnList.forEach(loc => {
                        loc && loc.Tanks && loc.Tanks.length > 0 && loc.Tanks.forEach(m => {
                            if (result[1] && result[1].filter(f => f.TankDetail.TankId == m.TankId && f.TankDetail.SiteId == loc.SiteId && f.TankDetail.StorageId == m.StorageId).length > 0)
                                m.IsUnknowDeliveryOrMissDelivery = true;
                            else
                                m.IsUnknowDeliveryOrMissDelivery = false;
                        });
                    });
                }
                else {
                    this.SelectedTankId = null;
                    this.LocationSchedules = [];
                    this.CloneLocationSchedules = [];
                    this.SelectedLocationId = '0';
                }
            });
    }

    locationChange($event) {
        this.SelectedTankId = null;
        this.SelectedLocationId = $event.JobId;
        this.SelectedSiteId = $event.SiteId;
        this.LocationSchedules = [];
        this.CloneLocationSchedules = [];
        this.getSalesData();
    }

    tankChange($event) {
        if (this.CloneLocationSchedules && this.CloneLocationSchedules.length > 0) {
            this.SelectedTankId = $event.TankId + '_' + $event.StorageId;
            this.LocationSchedules = this.CloneLocationSchedules.filter(f => f.TankId == $event.TankId && f.StorageId == $event.StorageId);
        }
        else
            this.LocationSchedules = [];
    }

    public getSalesData() {
        let inputs = {
            Priority: DeliveryReqPriority.None,
            LocationId: this.SelectedLocationId,
            SelectedTab: SelectedTabEnum.Tanks,
            Carriers: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value),
            IsShowCarrierManaged: this.salesTabFilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureTypeIds: this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedLocAttributeList').value),
        };
        this.IsLoading = true;
        this.dispatcherService.getBuyerSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
            this.LocationSchedules = resp;
            this.CloneLocationSchedules = resp;
            this.IsLoading = false;
            this.LocationSchedules && this.LocationSchedules.map(m => {
                try {
                    this.FilterLocationDrpDwnList && this.FilterLocationDrpDwnList.filter(f => f.SiteId == m.SiteId).map(j => j.Tanks.find(f => f.TankId == m.TankId && f.StorageId == m.StorageId).TankInventoryDiffinHrs = m.TankInventoryDiffinHrs);
                }
                catch (e) {
                    console.log(e);
                }
            })
            this.datatableRerender();
        });
    }

    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtTrigger.next();
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


    public showTanks(location: any) {

        if (location && location.RegionId) {
            this.SelectedRegionId = location.RegionId;

            let salesDataModel = new SalesDataModel();
            salesDataModel.RegionId = location.RegionId;
            salesDataModel.SiteId = location.SiteId;
            salesDataModel.TankId = location.TankId;
            salesDataModel.StorageId = location.StorageId;
            salesDataModel.TfxJobId = parseInt(location.JobId);
            salesDataModel.LocationManagedType = location.LocationManagedType ? location.LocationManagedType: null;
            this.dipTestComponent.loadTankDR(salesDataModel);
        }
        else {
            Declarations.msgerror("Location not assigned to region. Contact your supplier", undefined, undefined);
            this.closeSidePanel();
        }
    }
    public closeSidePanel() {
        closeSlidePanel();
    }
}


export class LocationTankDetailsModel {
    JobId?: number;
    SiteId?: string;
    Tanks?: TankDetailModel[];
    LocationName: string;
    DaysRemaining: number
    Status: string
}

export class TankDetailModel {
    TankId?: string;
    StorageId?: string;
    Name?: string;
    IsUnknowDeliveryOrMissDelivery?: boolean;
    TankInventoryDiffinHrs?: number;
    Status: string;
    DaysRemaining: number;
    CustomerInfo?: string;
}




