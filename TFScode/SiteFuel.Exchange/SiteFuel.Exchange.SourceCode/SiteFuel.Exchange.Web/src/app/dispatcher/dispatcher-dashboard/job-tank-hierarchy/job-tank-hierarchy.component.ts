import { Component, OnInit, ViewChild } from '@angular/core';
import { forkJoin } from 'rxjs';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { ForecastingLocationFilter } from '../../dispatcher.model';
import { DipTestComponent } from 'src/app/shared-components/dip-test/dip-test.component';
import { SalesDataModel } from 'src/app/carrier/models/DispatchSchedulerModels';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-job-tank-hierarchy',
    templateUrl: './job-tank-hierarchy.component.html',
    styleUrls: ['./job-tank-hierarchy.component.css']
})
export class JobTankHierarchyComponent implements OnInit {

    public LocationSchedules: any = [];
    CloneLocationSchedules = [];
    public LocationDrpDwnList: LocationTankDetailsModel[] = [];
    FilterLocationDrpDwnList: LocationTankDetailsModel[] = [];
    IsLoading = false;
    IsLocDrpDwnLoading = false;

    public SelectedRegionId: string;
    public SelectedCustomerId: string;
    SelectedLocationId: string;
    SelectedTankId: string;
    SelectedSiteId: string;
    filterArgs = { key: "DaysRemaining", asc: true };
    @ViewChild(DipTestComponent) dipTestComponent: DipTestComponent;
    
    constructor(private dispatcherService: DispatcherService) { }

    ngOnInit() {
        this.initLocationDropDown();
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
            Carriers: "",
            CustomerIds: this.SelectedCustomerId? this.SelectedCustomerId: "",
            InventoryCaptureType: "",
            IsRateOfConsumption: false,
            IsShowCarrierManaged: false,
            RegionId: this.SelectedRegionId? this.SelectedRegionId: ""
        };
        

        forkJoin([this.dispatcherService.getSupplierLocationTanks(filter), this.dispatcherService.GetRaisedExceptions()])
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
            });
    }

    locationChange($event) {
        this.SelectedTankId = null;
        this.SelectedLocationId = $event.JobId;
        this.SelectedSiteId = $event.SiteId;
        this.LocationSchedules = [];
        this.CloneLocationSchedules = [];
    }

    tankChange($event) {
        if (this.CloneLocationSchedules && this.CloneLocationSchedules.length > 0) {
            this.SelectedTankId = $event.TankId + '_' + $event.StorageId;
            this.LocationSchedules = this.CloneLocationSchedules.filter(f => f.TankId == $event.TankId && f.StorageId == $event.StorageId);
        }
        else
            this.LocationSchedules = [];
    }

    Partsfiltering(inputName?) {
        this.FilterLocationDrpDwnList = [];
        if (inputName && inputName.target && inputName.target.value && inputName.target.value.trim() != '') {
            let searchWord = inputName.target.value.toUpperCase();
            this.LocationDrpDwnList.forEach(element => {
                if (element.SiteId.toUpperCase().indexOf(searchWord) !== -1) {
                    this.FilterLocationDrpDwnList.push(element);
                }
            });
        } else {
            this.FilterLocationDrpDwnList = this.LocationDrpDwnList;
        }
    }

    public showTanks(location: any) {

        let row = this.LocationSchedules[0];

        let salesDataModel = new SalesDataModel();
        salesDataModel.RegionId = row.RegionId;
        salesDataModel.SiteId = location.SiteId;
        salesDataModel.TankId = location.TankId;
        salesDataModel.StorageId = location.StorageId;
        salesDataModel.TfxJobId = parseInt(location.JobId);
        salesDataModel.LocationManagedType = location.LocationManagedType ? location.LocationManagedType: null;
        this.dipTestComponent.loadTankDR(salesDataModel);
    }
    public closeSidePanel() {
        closeSlidePanel();
    }
}


export interface LocationTankDetailsModel {
    JobId?: number;
    SiteId?: string;
    Tanks?: TankDetailModel[];
    DaysRemaining: number
    Status: string
}

export interface TankDetailModel {
    TankId?: string;
    StorageId?: string;
    Name?: string;
    IsUnknowDeliveryOrMissDelivery: boolean;
    Status: string;
    DaysRemaining: number;
    CustomerInfo?: string;
}