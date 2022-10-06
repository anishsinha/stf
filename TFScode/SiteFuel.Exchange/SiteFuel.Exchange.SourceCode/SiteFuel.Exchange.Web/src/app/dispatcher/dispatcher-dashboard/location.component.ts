import { Component, OnInit, ViewChild, ElementRef, TemplateRef, ViewEncapsulation, ɵbypassSanitizationTrustStyle, Input } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Subject, Subscription, BehaviorSubject } from 'rxjs';
import * as moment from 'moment';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { JobLocationDetailsModal, JobAssetDetail, State, Country, City, Priority, SelectedItem, Customer, DipTest, TankMinMaxFill, TankChartHeight, Filter, JobTankAdditionalDetails } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { DistatcherRegionModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { CarrierService } from '../../carrier/service/carrier.service';
import { LocationViewComponent } from './sales-data/location-view.component';
import { LocationFilterModal } from '../dispatcher.model';
import { Declarations } from 'src/app/declarations.module';
import { TfxModule } from 'src/app/app.enum';
import { InventoryDataCaptureList } from 'src/app/app.constants';
export declare var google: any;

@Component({
    selector: 'app-location',
    templateUrl: './location.component.html',
    styleUrls: ['./location.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class LocationComponent implements OnInit {
    @ViewChild(LocationViewComponent) locationGridView: LocationViewComponent;
    @Input() singleMulti: number;
    public Map: any;
    public isLoading = false;
    public zoomLevel = 5;
    public jobLocationData: JobLocationDetailsModal[] = [];
    public opendedJobDetails: JobLocationDetailsModal;
    public clickedAssetsDetails: JobAssetDetail[] = [];
    public stateList: State[] = [];
    public countryList: Country[] = [];
    public cityList: City[] = [];
    public priorityList: Priority[] = [];
    public statusList = [];
    public fuelTypeList: SelectedItem[] = [];
    public customerList = [];
    public latestReading: DipTest = new DipTest();
    public chartData = [];
    public demandChartData: any;
    public chartLabels = [];
    private setCountryCenterInterval: any;
    public FuelUnit: string;
    public IsFilterLoaded: boolean = false;
    public chartOptions: ChartOptions;
    public selectedTankMinMax: TankMinMaxFill = new TankMinMaxFill();
    public selectedTankHeight: TankChartHeight = new TankChartHeight();

    subscriptions: Subscription = new Subscription();
    public multiselectSettingsById: IDropdownSettings;
    public CustomerDdlSettings: IDropdownSettings;
    public PriorityDdlSettings: IDropdownSettings;
    public filteredJobLocationData: JobLocationDetailsModal[] = [];
    public unchangedJobLocationData: JobLocationDetailsModal[] = [];
    public SelectedFilter: Filter = new Filter();

    public assetDetails = { assetIndex: 0 };
    public assetsModal = { modalDetails: { display: 'none', data: 'Modal Show' } };

    private locationSubscription = new Subscription();

    public clickViewActive: Boolean = false;
    public clickAssetsPanel: Boolean = false;
    public clickChartsPanel: Boolean = false;

    public toogleMap: Boolean = false;
    public toogleFilter: Boolean = false;

    public centerLocationLat = 47.1853106;
    public centerLocationLog = -125.36955;
    private UserCountry = "USA";
    private CountryCentre = {
        USA: { lat: 39.11757961, lng: -103.8784 },
        CAN: { lat: 57.88251631, lng: -98.54842922 }
    };
    public screenOptions = {
        position: 3
    };
    public mustGoUrl = "src/assets/marker-mustgo.svg";
    public shouldGoUrl = "src/assets/marker-shouldgo.svg";
    public couldGoUrl = "src/assets/marker-couldgo.svg";
    public noDlrUrl = "src/assets/marker-nodr.svg";
    public noImageUrl = "Content/images/no-image.png";
    public isChartDataExistSubject: BehaviorSubject<any>;
    public isShowCarrierManaged: boolean = false;
    public carrierList: any[] = [];
    selectedCarrierIds: string = '';
    SelectedRegions = [];
    public Regions: DistatcherRegionModel[] = [];
    public UnchangedCustomerList = [];
    public SelectedRegionId: string = '';
    public SelectedCustomerIds: string;
    public SelectedPriorityIds: string;
    public SelectedLocationIds: string;
    public SelectedStatusesId: string;
    SelectedCustomerList = [];
    SelectedlocationList = [];
    SelectedCarrierList = [];
    SelectedPriorityList = [];
    SelectedStatusList = [];
    selectedPriorityIds: string = '';
    LocationAttributeList = InventoryDataCaptureList;
    selectedLocAttributeList = [];

    public locationList = [];
    public isShowNonRetailJobs: boolean = false;
    public jobIdsEmittedFromSalesComponent: any = [];

    public locationFilterModal: LocationFilterModal = new LocationFilterModal();
    public count: number = 0;

    constructor(private readonly dispatcherService: DispatcherService,
        private carrierService: CarrierService,) {
        this.isChartDataExistSubject = new BehaviorSubject(false);

        var _this = this;
        window.addEventListener("beforeunload", function (e) {
            _this.SaveFilters(true);
            return;
        });
    }



    public ngOnInit(): void {
        this.getRegions();
        this.getCarriers();
        this.getDispatcherLocation();
        this.priorityList = [{
            Id: 1,
            Name: 'Must Go'
        }, {
            Id: 2,
            Name: 'Should Go'
        },
        {
            Id: 3,
            Name: 'Could Go'
        },
        {
            Id: 4,
            Name: 'Unplanned'
        }];
        this.statusList = [{
            Id: 'Scheduled',
            Name: 'Scheduled'
        }, {
            Id: 'DR Created',
            Name: 'DR Created'
        },
        {
            Id: 'No DR',
            Name: 'No DR'
        }];
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.CustomerDdlSettings = {
            singleSelection: false,
            idField: 'CompanyId',
            textField: 'CompanyName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        }
        this.PriorityDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        }
    }



    getCarriers() {
        this.dispatcherService.GetCarriersForSupplier().subscribe(data => {
            this.carrierList = data;
        });
    }

    private fetchJobLocationData(PageLoad?: Boolean): void {
        this.isLoading = true;
        if (this.jobIdsEmittedFromSalesComponent && this.jobIdsEmittedFromSalesComponent.length) {
            var ids = []
            this.jobIdsEmittedFromSalesComponent.forEach(res => { ids.push(res.TfxJobId) });
            var jobids = "";
            jobids = ids.join();

            var selectedLocAttributeId = "";
            if (this.selectedLocAttributeList && this.selectedLocAttributeList.length > 0) {
                var ids = [];
                this.selectedLocAttributeList.forEach(res => { ids.push(res.Id) });
                selectedLocAttributeId = ids.join();
            }
            this.locationSubscription.add(this.dispatcherService.getJobLocationDetails(jobids, selectedLocAttributeId).subscribe(res => {
                //   this.locationSubscription.add(this.dispatcherService.getJobLocationDetails(this.jobIdsEmittedFromSalesComponent,selectedLocAttributeId).subscribe(res => {
                if (res) {
                    this.jobLocationData = this.addJobPriority(res['Data']['jobLocationDetails']);
                }
                this.setCountryCentre();
                this.isLoading = false;
            }));
        } else {
            this.unchangedJobLocationData = this.jobLocationData = [];
            this.setCountryCentre();
            this.isLoading = false;
        }

    }

    private convertToObjectArray(data: string[]): SelectedItem[] {
        let modifiedItemArray: SelectedItem[] = [];
        data.forEach((item, index) => {
            let Item: SelectedItem = { 'Id': 0, 'Name': '' };
            Item.Id = index;
            Item.Name = item;
            modifiedItemArray.push(Item);
        })
        return modifiedItemArray;
    }

    private fetchCountryListData(): void {
        this.locationSubscription.add(this.dispatcherService.getCountryList().subscribe(res => {
            this.countryList = res;
        }));
    }

    private fetchProductTypeListData(): void {
        this.locationSubscription.add(this.dispatcherService.getProductTypeList().subscribe(res => {
            this.fuelTypeList = res.Data;
        }));
    }

    private fetchStateListData(countryId?: string): void {
        this.locationSubscription.add(this.dispatcherService.getStateList(countryId).subscribe(res => {
            this.stateList = res;
        }));
    }

    private fetchCityListData(stateId?: string): void {
        this.locationSubscription.add(this.dispatcherService.getCityList(stateId).subscribe(res => {
            this.cityList = res;
        }));
    }
    private addJobPriority(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
        if (jobLocationData && jobLocationData.length) {
            jobLocationData.forEach(element => {
                var obj = this.jobIdsEmittedFromSalesComponent.find(t => t.TfxJobId == element.JobID);
                if (obj) {
                    if (obj.Priority == 1) {
                        element.highestPriority = 1;
                        element.iconUrl = this.mustGoUrl;
                    }
                    else if (obj.Priority == 2) {
                        element.highestPriority = 2;
                        element.iconUrl = this.shouldGoUrl;
                    }
                    else if (obj.Priority == 3) {
                        element.highestPriority = 3;
                        element.iconUrl = this.couldGoUrl;
                    }
                    else {
                        element.highestPriority = 4;
                        element.iconUrl = this.noDlrUrl;
                    }
                } else {
                    element.highestPriority = 4;
                    element.iconUrl = this.noDlrUrl;
                }
            });
        }
        return jobLocationData;
    }
    private checkMostPriorityJob(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
        const jobLocationLength = jobLocationData.length;
        for (let i = 0; i < jobLocationLength; i++) {
            let deliveryRequests = jobLocationData[i].jobDeliveryRequests;
            if (deliveryRequests.length) {
                let filteredMustGoDRs = deliveryRequests.filter((data) => data.Priority === 1);
                let filteredShoudGoDRs = deliveryRequests.filter((data) => data.Priority === 2);
                let filteredCouldGoDRs = deliveryRequests.filter((data) => data.Priority === 3);
                if (filteredMustGoDRs.length > 0) {
                    jobLocationData[i].highestPriority = 1;
                    jobLocationData[i].iconUrl = this.mustGoUrl;
                }
                else if (filteredShoudGoDRs.length > 0) {
                    jobLocationData[i].highestPriority = 2;
                    jobLocationData[i].iconUrl = this.shouldGoUrl;
                }
                else {
                    jobLocationData[i].highestPriority = 3;
                    jobLocationData[i].iconUrl = this.couldGoUrl;
                }
            } else {
                jobLocationData[i].highestPriority = 4;
                jobLocationData[i].iconUrl = this.noDlrUrl;
            }
        }
        return jobLocationData;
    }

    public ngOnDestroy(): void {
        if (this.locationSubscription) {
            this.locationSubscription.unsubscribe();
        }
        this.SaveFilters(true);
        if (this.setCountryCenterInterval) {
            clearInterval(this.setCountryCenterInterval);
        }
    }

    public mouseHoverMarker(infoWindow, event: MouseEvent): void {
        infoWindow.open();
    }

    public mouseHoveOutMarker(infoWindow, event: MouseEvent): void {
        infoWindow.close();
    }

    public onInfoViewClick(jobLocation: JobLocationDetailsModal): void {
        window.scrollTo(0, 0);
        this.opendedJobDetails = jobLocation;
        this.assetDetails.assetIndex = 0;
        this.clickViewActive = true;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
        this.toogleMap = true;
        this.closeAssetsClicked();
    }

    public closeViewClicked(): void {
        this.clickViewActive = false;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
    }

    public onAssetsViewClick(assets: JobAssetDetail[]): void {
        if (assets.length) {
            this.clickAssetsPanel = true;
            this.clickedAssetsDetails = assets;
            if (assets[0].jobTankAdditionalDetails.length) {
                this.getDipTestDetails(assets[0].jobTankAdditionalDetails[0]['SiteId'], assets[0].jobTankAdditionalDetails[0]['TankId'], 3);
            }
            else {
                this.chartData = [];
                this.latestReading = new DipTest();
            }
        }
    }

    public closeAssetsClicked(): void {
        this.clickAssetsPanel = false;

    }

    public onChartsViewClick(assets: JobAssetDetail[]): void {
        this.clickChartsPanel = true;
        this.isChartDataExistSubject.next(false);
        if (assets.length && assets[0].jobTankAdditionalDetails.length) {
            this.getDemandCaptureChart(assets[0].jobTankAdditionalDetails[0]['SiteId'], 3, assets[0].JobId);
        }
        else {
            this.isChartDataExistSubject.next(false);
            this.demandChartData = null;
        }
    }

    public closeChartsClicked(): void {
        this.clickChartsPanel = false;
    }

    public assetTabClicked(indx: number): void {
        this.assetDetails.assetIndex = indx;
        if (this.clickedAssetsDetails[indx].jobTankAdditionalDetails.length) {
            this.getDipTestDetails(this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['SiteId'], this.clickedAssetsDetails[indx].jobTankAdditionalDetails[0]['TankId'], 3);
        } else {
            this.chartData = [];
            this.latestReading = new DipTest();
        }
    }
    public toggleMapView(): void {
        this.toogleMap = !this.toogleMap;
        if (this.toogleMap) {
            this.fetchJobLocationData();
        }
        //this.SaveFilters();
    }

    public toggleFilterView(): void {
        this.toogleFilter = !this.toogleFilter;
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }

    private tableClickFilter(data, index): void {
    }

    public modalOpen(jobLocation: JobLocationDetailsModal): void {
        this.opendedJobDetails = jobLocation;
        this.clickedAssetsDetails = this.opendedJobDetails.jobAssetDetails;
        if (this.clickedAssetsDetails.length) {
            this.closeAssetsClicked();
            this.closeViewClicked();
            this.closeChartsClicked();
            this.assetDetails.assetIndex = 0;
            if (this.clickedAssetsDetails[0].jobTankAdditionalDetails.length) {
                this.getDipTestDetails(this.clickedAssetsDetails[0].jobTankAdditionalDetails[0]['SiteId'], this.clickedAssetsDetails[0].jobTankAdditionalDetails[0]['TankId'], 3);
            }
            this.assetsModal = { modalDetails: { display: 'block', data: 'Modal Show' } };
        }
    }

    public modalClose(): void {
        this.assetsModal = { modalDetails: { display: 'none', data: 'Modal Show' } };
    }

    private getDipTestDetails(siteId: string, tankId: string, noOfDays: number): void {
        this.isLoading = true;
        this.chartData = [];
        this.chartOptions = {};
        this.chartOptions = this.setChartOptions(this.UserCountry);
        this.chartLabels = [];
        this.latestReading = new DipTest();
        this.locationSubscription.add(this.dispatcherService.getDipTestDetails(siteId, tankId, noOfDays).subscribe((data) => {
            if (data && data.StatusCode === 302) {
                let resp = data.Data;
                this.latestReading = resp[0];
                let obj = {};
                let chartdata = [];
                obj['label'] = 'Tank ' + resp[0]['TankId'];
                let respLen = resp.length;
                for (let i = 0; (i < respLen); i++) {
                    let captureTime = moment(new Date(resp[i].CaptureTimeString)).format('MM/DD/YYYY hh:mm A');
                    chartdata.unshift(resp[i].NetVolume);
                    this.chartLabels.unshift(captureTime);
                }
                obj['data'] = chartdata;
                this.chartData.push(obj);
            }
            this.calculateMinMAx(this.clickedAssetsDetails[this.assetDetails.assetIndex].jobTankAdditionalDetails[0]);
            this.isLoading = false;
        }));
    }

    public getDemandCaptureChart(siteId: string, noOfDays: number, tfxJobId: number): void {
        // this.demandChartData = {};
        this.demandChartData = { siteId: siteId, noOfDays: noOfDays, tfxJobId: tfxJobId };
        this.isChartDataExistSubject.next(true);
    }

    public mapReady(map: any): void {
        this.Map = map;
        this.setCountryCentre();
    }

    private setZoomLevel(): void {
        if (this.jobLocationData.length == 0) {
            this.setCountryCentre();
        } else {
            this.Map.setZoom(5);
        }
    }

    private setCountryCentre(): void {
        if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(() => {
                this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
                this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
                if (this.Map && this.jobLocationData.length == 0) {
                    this.Map.setCenter(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
                    this.Map.setZoom(5);
                } else {
                    const bounds = new google.maps.LatLngBounds();
                    this.jobLocationData.forEach((x: JobLocationDetailsModal) => {
                        bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
                    });
                    if (this.Map && bounds) {
                        this.Map.fitBounds(bounds);
                        this.Map.setCenter(bounds.getCenter());
                        this.Map.setZoom(5);
                    }

                }
            }, 500);
        }
    }

    private setChartOptions(country: string) {
        this.FuelUnit = (country === 'USA') ? 'Gallons' : 'Litres';
        return {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: `NetVolume ( Fuels Per ${this.FuelUnit})`
                    },
                    ticks: {
                        callback: label => { return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","); }
                    }
                }],
                xAxes: [{
                    type: 'time',
                    time: {
                        displayFormats: {
                            'millisecond': 'MMM DD',
                            'second': 'MMM DD',
                            'minute': 'MMM DD',
                            'hour': 'MMM DD',
                            'day': 'MMM DD',
                            'week': 'MMM DD',
                            'month': 'MMM DD',
                            'quarter': 'MMM DD',
                            'year': 'MMM DD',
                        }
                    }
                }
                ]
            }
        };
    }

    private getDispatcherLocation(): void {
        this.dispatcherService.getDispatcherCountry().subscribe(data => {
            this.UserCountry = data;
            this.FuelUnit = (this.UserCountry === 'USA') ? 'Gallons' : 'Litres';
        });
    }
    public setCenterMap($event): void {
        if (this.UserCountry && !this.jobLocationData.length) {
            this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
            this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
            if (this.Map) {
                this.Map.setCenter({ lat: this.centerLocationLat, lng: this.centerLocationLog });
                this.Map.setZoom(5);
            }
        }
    }
    public downloadDipChart(event: any, assetNumber: string) {
        let anchor = event.target;
        let orignalCanvas = document.getElementsByTagName('canvas')[0];
        var resizedCanvas = document.createElement("canvas");
        var resizedContext = resizedCanvas.getContext("2d");
        resizedCanvas.height = 500;
        resizedCanvas.width = 800;
        var context = orignalCanvas.getContext("2d");
        resizedContext.drawImage(orignalCanvas, 0, 0, 800, 500);
        anchor.href = resizedCanvas.toDataURL('image/jpeg', 1.0);
        anchor.download = `${this.clickedAssetsDetails[assetNumber].AssetName}.png`;
    }

    private calculateMinMAx(selectedTank: JobTankAdditionalDetails): void {
        this.selectedTankMinMax.MaxFill = selectedTank.MaxFill;
        this.selectedTankMinMax.MinFill = selectedTank.MinFill;
        this.selectedTankMinMax.MaxFillPercent = selectedTank.MaxFillPercent;
        this.selectedTankMinMax.MinFillPercent = selectedTank.MinFillPercent;
        let ci_percent = ((this.latestReading.NetVolume || 0) / selectedTank.FuelCapacity) * 100;
        ci_percent = ci_percent > selectedTank.MaxFillPercent ? selectedTank.MaxFillPercent : ci_percent;
        ci_percent = ci_percent < 0 ? 0 : ci_percent;

        let sbf_percent = (selectedTank.MaxFillPercent - ci_percent);
        sbf_percent = sbf_percent > 100 ? 100 : sbf_percent;
        sbf_percent = sbf_percent < 0 ? 0 : sbf_percent;

        this.fillTankDiagram(sbf_percent, ci_percent);
    }

    private fillTankDiagram(sbf_percent: number, ci_percent: number): void {
        this.selectedTankHeight.sbf_percent = sbf_percent;
        this.selectedTankHeight.ci_percent = ci_percent;
        let min_ShouldBeEmptyPercent = (125 - ((sbf_percent * 1.25) + (ci_percent * 1.25)));
        let min_ShouldBeFilledPercent = (sbf_percent * 1.25);
        let min_CurrentInventoryPercent = (ci_percent * 1.25);
        //need of cal
        if (min_ShouldBeFilledPercent < 16 || min_CurrentInventoryPercent < 16) {
            //dont remove from emtty
            if (min_ShouldBeEmptyPercent < 16) {

                if (min_ShouldBeFilledPercent < 16) {
                    min_ShouldBeFilledPercent = min_ShouldBeFilledPercent + 16;
                    min_CurrentInventoryPercent = min_CurrentInventoryPercent - 16;
                }
                if (min_CurrentInventoryPercent < 16) {
                    min_CurrentInventoryPercent = min_CurrentInventoryPercent + 16;
                    min_ShouldBeFilledPercent = min_ShouldBeFilledPercent - 16;
                }
            }
            //remove from empty
            else {
                if (min_ShouldBeFilledPercent < 16) {
                    min_ShouldBeFilledPercent = min_ShouldBeFilledPercent + 16;
                    min_ShouldBeEmptyPercent = min_ShouldBeEmptyPercent - 16;
                }
                if (min_CurrentInventoryPercent < 16) {
                    min_CurrentInventoryPercent = min_CurrentInventoryPercent + 16;
                    min_ShouldBeEmptyPercent = min_ShouldBeEmptyPercent - 16;
                }
            }
        }
        this.selectedTankHeight.CurrentInventoryPercent = min_CurrentInventoryPercent;
        this.selectedTankHeight.ShouldBeFilledPercent = min_ShouldBeFilledPercent;
        this.selectedTankHeight.ShouldBeEmptyPercent = min_ShouldBeEmptyPercent;
    }

    public ShowCarrierMangedData() {
        this.getAllCustomers();
    }

    public carrierChanged() {
        var ids = [];
        this.selectedCarrierIds = '';
        this.SelectedCarrierList.forEach(res => { ids.push(res.Id) });
        this.selectedCarrierIds = ids.join();
        this.getAllCustomers();
    }

    public onCustomerChanged() {
        if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
            this.locationList = [];
            var customers = this.customerList.filter(t => { return this.SelectedCustomerList.filter(el => el.CompanyId == t.CompanyId).length > 0; });
            customers.forEach(res => {
                if (!this.locationList.find(t => t.Id == res.Id)) {
                    this.locationList = this.locationList.concat(res.Jobs);
                }
            });
            this.locationList = this.GetUniqueItems(this.locationList.reduce((p, n) => p.concat(n), []));
            if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
                this.SelectedlocationList = this.SelectedlocationList.filter(t => { return this.locationList.filter(el => el.Id == t.Id).length > 0 });
            }
        }
        else {
            this.initAllLocation();
        }
    }

    public getRegions() {
        this.dispatcherService.GetDispatcherRegions().subscribe(data => {
            this.Regions = data;
            if (this.Regions && this.Regions.length > 0) {
                this.SelectedRegions = [];
                this.SelectedRegions.push(this.Regions[0]);
                var ids = [];
                this.SelectedRegions.forEach(res => { ids.push(res.Id) });
                this.SelectedRegionId = ids.join();
            }
            this.GetFilters();
        });
    }

    private getCustomerListByRegionId(SelectedRegionId) {
        this.carrierService.getJobListForCarrier(SelectedRegionId, this.isShowCarrierManaged, this.selectedCarrierIds).subscribe(t2 => {
            this.customerList = t2;
            if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
                this.SelectedCustomerList = this.SelectedCustomerList.filter(t => { return this.customerList.filter(el => el.CompanyId == t.CompanyId).length > 0 });
            }
            this.initAllLocation();
        });
    }

    private initAllLocation() {
        this.locationList = [];

        if (this.SelectedRegions && this.SelectedRegions.length > 0) {
            this.customerList.forEach(res => {
                this.locationList = this.locationList.concat(res.Jobs.filter(t => this.SelectedRegions.some(el => el.Id == t.RegionId)));
            });
        }
        else {
            this.customerList.forEach(res => {
                if (!this.locationList.find(t => t.Id == res.Id)) {
                    this.locationList = this.locationList.concat(res.Jobs);
                }
            });
        }
        this.locationList = this.GetUniqueItems(this.locationList.reduce((p, n) => p.concat(n), []));
        if (this.SelectedlocationList && this.SelectedlocationList.length > 0) {
            this.SelectedlocationList = this.SelectedlocationList.filter(t => { return this.locationList.filter(el => el.Id == t.Id).length > 0 });
        }
    }

    GetUniqueItems(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
    }

    public onRegionChanged() {
        this.setCustomerAndLocations();
    }

    setCustomerAndLocations() {
        if (this.SelectedRegions && this.SelectedRegions.length > 0) {
            this.customerList = this.UnchangedCustomerList.filter(t => { return this.SelectedRegions.filter(el => t.RegionIds.some(r => el.Id == r)).length > 0; });
        }
        else {
            this.customerList = this.UnchangedCustomerList;
        }
        if (this.SelectedCustomerList && this.SelectedCustomerList.length > 0) {
            this.SelectedCustomerList = this.SelectedCustomerList.filter(t => { return this.customerList.filter(el => el.CompanyId == t.CompanyId).length > 0 });
        }
        this.initAllLocation();
    }

    getAllCustomers() {
        var ids = [];
        this.Regions.forEach(res => { ids.push(res.Id) });
        var selectedRegionId = ids.join();
        this.carrierService.getJobListForCarrier(selectedRegionId, this.isShowCarrierManaged, this.selectedCarrierIds).subscribe(t2 => {
            this.UnchangedCustomerList = t2;
            this.setCustomerAndLocations();
        });
    }

    public onLocationTypeChange($event) {

    }
    public ResetFilters() {
        this.SelectedRegions = [];
        this.SelectedCustomerList = [];
        this.SelectedlocationList = [];
        this.SelectedPriorityList = [];
        this.SelectedStatusList = [];
        this.selectedLocAttributeList = [];
        this.ApplyFilters("reset");

    }
    public ApplyFilters(msg?) {
        this.SaveFilters(false);
        this.count = 0;

        var Regionids = [];
        this.SelectedRegions.forEach(res => {
            this.count++;
            Regionids.push(res.Id)
        });
        this.locationFilterModal.SelectedRegionId = Regionids.join();

        var Customerids = [];
        this.SelectedCustomerList.forEach(res => {
            this.count++;
            Customerids.push(res.CompanyId)
        });
        this.locationFilterModal.SelectedCustomerId = Customerids.join();

        var Locationids = [];
        this.SelectedlocationList.forEach(res => {
            this.count++;
            Locationids.push(res.Id)
        });
        this.locationFilterModal.SelectedLocationId = Locationids.join();

        var Statusids = [];
        this.SelectedStatusList.forEach(res => {
            this.count++;
            Statusids.push(res.Id)
        });
        this.locationFilterModal.SelectedStatusId = Statusids.join();

        var Prioritiesids = [];
        this.SelectedPriorityList.forEach(res => {
            this.count++;
            Prioritiesids.push(res.Id)
        });
        this.locationFilterModal.SelectedPrioritiesId = Prioritiesids.join();

        var LocAttributeids = [];
        if (this.selectedLocAttributeList.length != 0) {
            this.selectedLocAttributeList.forEach(res => {
                this.count++;
                LocAttributeids.push(res.Id)
            });
        }
        else {
            // LocAttributeids.push(0);
            LocAttributeids = [0, 1, 2, 3];
        }

        this.locationFilterModal.selectedLocAttributeId = LocAttributeids.join();

        this.locationGridView.applyFilters(this.locationFilterModal);
        if (msg == "set") {
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        } else if (msg == "reset") {
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }

    }

    async SaveFilters(isTopFilter?: boolean) {
        var data = {};
        this.dispatcherService.getFilters(TfxModule.SupplierWallyboardLocation).subscribe(res => {
            this.IsFilterLoaded = true;
            var input: any
            if (res || res == "") {
                if (res != "") {
                    input = JSON.parse(res);
                    data = input;
                }
                if (isTopFilter) {
                    data['IsShowCarrierManaged'] = this.isShowCarrierManaged;
                    data['Carrier'] = this.SelectedCarrierList;
                    data['toogleMap'] = this.toogleMap;
                    data['isShowNonRetailJobs'] = this.isShowNonRetailJobs;
                    data['singleMulti'] = this.singleMulti;
                } else {

                    data['Regions'] = this.SelectedRegions;
                    data['Customer'] = this.SelectedCustomerList;
                    data['Location'] = this.SelectedlocationList;
                    data['Priority'] = this.SelectedPriorityList;
                    data['Status'] = this.SelectedStatusList;
                    data['selectedLocAttributeList'] = this.selectedLocAttributeList;
                    data['IsShowCarrierManaged'] = this.isShowCarrierManaged;
                    data['Carrier'] = this.SelectedCarrierList;
                    data['toogleMap'] = this.toogleMap;
                    data['isShowNonRetailJobs'] = this.isShowNonRetailJobs;
                    data['singleMulti'] = this.singleMulti;
                }
                this.dispatcherService.postFiltersData(TfxModule.SupplierWallyboardLocation, JSON.stringify(data)).subscribe();

            }
        });

    }
    public GetFilters() {
        this.isLoading = true;
        this.dispatcherService.getFilters(TfxModule.SupplierWallyboardLocation).subscribe(res => {
            this.IsFilterLoaded = true;
            if (res) {
                this.SetFilters(JSON.parse(res));
            }
            else {
                this.getAllCustomers();
                this.toogleMap = true;
            }
            this.isLoading = false;
        });
    }

    public SetFilters(input: any) {
        this.singleMulti = input.singleMulti == "" ? 1 : input.singleMulti;
        if (this.isShowCarrierManaged != input.IsShowCarrierManaged) {
            this.isShowCarrierManaged = input.IsShowCarrierManaged;
        }
        if (this.isShowNonRetailJobs != input.isShowNonRetailJobs) {
            this.isShowNonRetailJobs = input.isShowNonRetailJobs;
        }
        if (this.toogleMap != input.toogleMap) {
            this.toogleMap = input.toogleMap;
        }
        if (input.Carrier && input.Carrier.length > 0) {
            this.SelectedCarrierList = input.Carrier as SelectedItem[] || [];
        }
        if (input.Regions && input.Regions.length > 0) {
            this.SelectedRegions = input.Regions as SelectedItem[] || [];
            // var ids = [];
            // this.SelectedRegions.forEach(res => { ids.push(res.Id) });
            // this.SelectedRegionId = ids.join();
        }
        if (input.Customer && input.Customer.length > 0) {
            this.SelectedCustomerList = input.Customer as Customer[] || [];
        }
        if (input.Location && input.Location.length > 0) {
            this.SelectedlocationList = input.Location as Location[] || [];
        }
        if (input.Priority && input.Priority.length > 0) {
            this.SelectedPriorityList = input.Priority as Priority[] || [];
        }
        if (input.Status && input.Status.length > 0) {
            this.SelectedStatusList = input.Status as SelectedItem[] || [];
        }
        if (input.selectedLocAttributeList && input.selectedLocAttributeList.length > 0) {
            this.selectedLocAttributeList = input.selectedLocAttributeList as SelectedItem[] || [];
        }
        this.ApplyFilters();
        this.getAllCustomers();
    }

    getJobIdsForMapEventHandler(valueEmitted) {

        this.jobIdsEmittedFromSalesComponent = valueEmitted;
        if (this.toogleMap) {
            this.fetchJobLocationData();
        }
    }

}