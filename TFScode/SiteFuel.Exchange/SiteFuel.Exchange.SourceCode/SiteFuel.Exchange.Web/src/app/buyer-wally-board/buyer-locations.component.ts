import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, Input, ChangeDetectorRef } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Subject, Subscription, BehaviorSubject, from } from 'rxjs';
import * as moment from 'moment';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { JobLocationDetailsModal, TankMinMaxFill, TankChartHeight, DipTest, JobTankAdditionalDetails, JobAssetDetail, Country, Priority, Supplier, Carrier, TfxModule } from './Models/BuyerWallyBoard';
import { BuyerwallyboardService } from './services/buyerwallyboard.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Declarations } from 'src/app/declarations.module';
import { LocationViewComponent } from './sales-data/location-view.component';
import { InventoryDataCaptureList } from '../app.constants';
export declare var google: any;
declare var currentCompanyId: any;
declare var jQuery: any;

@Component({
    selector: 'app-buyer-locations',
    templateUrl: './buyer-locations.component.html',
    styleUrls: ['./buyer-locations.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class BuyerLocationsComponent implements OnInit {
    public Map: any;
    public isLoading = false;
    public zoomLevel = 5;
    public jobLocationDataForMap: JobLocationDetailsModal[] = [];
    public opendedJobDetails: JobLocationDetailsModal;
    public clickedAssetsDetails: JobAssetDetail[] = [];
    public countryList: Country[] = [];
    public priorityList: Priority[] = [];
    public supplierList: Supplier[] = [];
    public carrierList: Carrier[] = [];
    public isShowCarrierManaged: boolean = false;
    public selectedCarrierIds: string = '';
    public latestReading: DipTest = new DipTest();
    public chartData = [];
    public demandChartData: any;
    public chartLabels = [];
    public IsFiltersLoaded: boolean = false;
    private setCountryCenterInterval: any;
    public FuelUnit: string; // Used only in demant chart 
    public UoM: string = '';
    public chartOptions: ChartOptions;
    public selectedTankMinMax: TankMinMaxFill = new TankMinMaxFill();
    public selectedTankHeight: TankChartHeight = new TankChartHeight();

    public multiselectSettingsById: IDropdownSettings;
    public jobMultiselectSettingsById: IDropdownSettings;
    public multiDropdownSettings: IDropdownSettings;//Added for allowing select all for carrier and supplier
    public priorityselectSettingsById: IDropdownSettings;
    public filteredJobLocationData: JobLocationDetailsModal[] = [];
    public unchangedJobLocationData: JobLocationDetailsModal[] = [];
    public FilterForm: FormGroup;

    public assetDetails = { assetIndex: 0 };
    public assetsModal = { modalDetails: { display: 'none', data: 'Modal Show' } };

    public dtTrigger: Subject<any> = new Subject();
    private locationSubscription = new Subscription();

    public clickViewActive: Boolean = false;
    public clickAssetsPanel: Boolean = false;
    public clickChartsPanel: Boolean = false;

    public toogleMap: Boolean = true;
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
    subscriptions: Subscription = new Subscription();
    public CurrentCompanyId: any;
    @Input() singleMulti: number;
    SelectedPriorityList = [];
    public SelectedSupplierIds: string;
    SelectedSupplierList = [];
    public SelectedPriorityIds: string;
    public statusList = [];
    SelectedStatusList = [];
    public SelectedStatusesId: string;
    public locationList = [];
    public DefaultLocationList = [];
    SelectedlocationList = [];
    public SelectedLocationIds: string;
    SelectedCarrierList = [];
    public isShowNonRetailJobs: boolean = false;
    public jobIdsEmittedFromSalesComponent: any = [];
    public IsLoadSalesData = true;
    public filterCount : number;

    LocationAttributeList = InventoryDataCaptureList;
    @ViewChild(LocationViewComponent)  locationView : LocationViewComponent;

    constructor(private buyerwallyboardservice: BuyerwallyboardService, private fb: FormBuilder, private changeDetectorRef: ChangeDetectorRef) {
        this.isChartDataExistSubject = new BehaviorSubject(false);
        this.initializeFilterForm();
    }

    ngOnInit() {
        this.SelectedPriorityList.push({ Id: 1, Name: 'Must Go' });
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
        this.jobMultiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.priorityselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.getFilterData();

        // var _this = this;
        // window.addEventListener("beforeunload", function (e) {
        //     _this.SaveFilters();
        //     return;
        // });
    }

    private fetchJobLocationData(): void {
        this.isLoading = true;
        if (this.jobIdsEmittedFromSalesComponent && this.jobIdsEmittedFromSalesComponent.length) {
            var ids =  []
            this.jobIdsEmittedFromSalesComponent.forEach(res => { ids.push(res.TfxJobId) });
            var jobids = "";
            jobids = ids.join();
            //Location Type FIlter
            var selectedLocAttributeId="";
            if( this.FilterForm.controls.selectedLocAttributeList.value &&   this.FilterForm.controls.selectedLocAttributeList.value.length>0){
                var ids = [];
                this.FilterForm.controls.selectedLocAttributeList.value.forEach(res => { ids.push(res.Id) });
                selectedLocAttributeId= ids.join();
            }

            this.locationSubscription.add(this.buyerwallyboardservice.getJobLocationDetails(jobids,selectedLocAttributeId).subscribe(res => {
                if (res) {
                    this.unchangedJobLocationData = this.jobLocationDataForMap = this.addJobPriority(res['Data']['jobLocationDetails']);
                    // this.locationList = this.unchangedJobLocationData;
                    this.fillSupplierCarrierDropdowns();
                    // this.jobLocationDataForMap = this.applyFilter();
                }
                this.setCountryCentre();
                this.isLoading = false;
            }));
        } else {
            this.unchangedJobLocationData = this.jobLocationDataForMap = [];
            this.setCountryCentre();
            this.isLoading = false;
        }

    }

    ngOnDestroy(): void {
        this.SaveFilters();
    }



    private fillSupplierCarrierDropdowns() {
        this.CurrentCompanyId = Number(currentCompanyId);
        var jobIds = [];
        this.jobLocationDataForMap.map(item => jobIds.push(item.JobID));
        if (jobIds && jobIds.length) {
            var selectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;
            selectedSuppliers = selectedSuppliers.filter(t => { return this.supplierList.filter(el => el.Id == t.Id).length > 0 })
            this.FilterForm.get('SelectedSupplierList').patchValue(selectedSuppliers);
            var selectedCarriers = this.FilterForm.get('SelectedCarrierList').value;
            selectedCarriers = selectedCarriers.filter(t => { return this.carrierList.filter(el => el.Id == t.Id).length > 0 })
            this.FilterForm.get('SelectedCarrierList').patchValue(selectedCarriers);
            this.onSupplierSelect();
            this.changeDetectorRef.detectChanges();
        }
    }

    private initializeFilterForm() {
        this.FilterForm = this.fb.group({
            IsShowAssetJobs: this.fb.control(false),
            IsShowCarrierManaged: this.fb.control(false),
            SelectedCarrierList: this.fb.control([]),
            SelectedSupplierList: this.fb.control([]),
            SelectedlocationList: this.fb.control([]),
            SelectedPriorityList: this.fb.control([]),
            SelectedStatusList: this.fb.control([]),
            ToggleMap: this.fb.control(true),
            singleMulti: this.fb.control(this.singleMulti),
            selectedLocAttributeList:this.fb.control([])
        });
    }
    private addJobPriority(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
        if (jobLocationData && jobLocationData.length) {
            jobLocationData.forEach(element => {
                var obj =  this.jobIdsEmittedFromSalesComponent.find(t => t.TfxJobId == element.JobID);
                if(obj)
                {
                    if (obj.Priority == 1) {
                       element.highestPriority = 1;
                       element.iconUrl = this.mustGoUrl;
                    }
                    else if (obj.Priority == 2) {
                        element.highestPriority = 2;
                        element.iconUrl  = this.shouldGoUrl;
                    }
                    else if (obj.Priority == 3) {
                        element.highestPriority = 3;
                        element.iconUrl  =  this.couldGoUrl;
                    }
                    else {
                        element.highestPriority = 4;
                        element.iconUrl  = this.noDlrUrl;
                    }
                }else{
                    element.highestPriority = 4;
                    element.iconUrl  = this.noDlrUrl;
                }                        
            });
        }
        return jobLocationData;
    }
    // private checkMostPriorityJob(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
    //     const jobLocationLength = jobLocationData.length;
    //     for (let i = 0; i < jobLocationLength; i++) {
    //         let deliveryRequests = jobLocationData[i].jobDeliveryRequests;
    //         if (deliveryRequests.length) {
    //             let filteredMustGoDRs = deliveryRequests.filter((data) => data.Priority === 1);
    //             let filteredShoudGoDRs = deliveryRequests.filter((data) => data.Priority === 2);
    //             if (filteredMustGoDRs.length > 0) {
    //                 jobLocationData[i].highestPriority = 1;
    //                 jobLocationData[i].iconUrl = this.mustGoUrl;
    //             }
    //             else if (filteredShoudGoDRs.length > 0) {
    //                 jobLocationData[i].highestPriority = 2;
    //                 jobLocationData[i].iconUrl = this.shouldGoUrl;
    //             }
    //             else {
    //                 jobLocationData[i].highestPriority = 3;
    //                 jobLocationData[i].iconUrl = this.couldGoUrl;
    //             }
    //         } else {
    //             jobLocationData[i].highestPriority = 4;
    //             jobLocationData[i].iconUrl = this.noDlrUrl;
    //         }
    //     }
    //     return jobLocationData;
    // }

    // private convertToObjectArray(data: string[]): SelectedItem[] {
    //     let modifiedItemArray: SelectedItem[] = [];
    //     data.forEach((item, index) => {
    //         let Item: SelectedItem = { 'Id': 0, 'Name': '' };
    //         Item.Id = index;
    //         Item.Name = item;
    //         modifiedItemArray.push(Item);
    //     })
    //     return modifiedItemArray;
    // }

    private setCountryCentre(): void {
        if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(() => {
                this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
                this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
                if (this.Map && (!this.jobLocationDataForMap || this.jobLocationDataForMap.length == 0)) {
                    this.Map.setCenter(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
                    this.Map.setZoom(5);
                } else {
                    const bounds = new google.maps.LatLngBounds();
                    this.jobLocationDataForMap.forEach((x: JobLocationDetailsModal) => {
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

    private setZoomLevel(): void {
        if (this.jobLocationDataForMap && this.jobLocationDataForMap.length == 0) {
            this.setCountryCentre();
        } else {
            //this.zoomLevel = 10;
        }
    }


    public mouseHoverMarker(infoWindow, event: MouseEvent): void {
        infoWindow.open();
    }
    public mouseHoveOutMarker(infoWindow, event: MouseEvent): void {
        infoWindow.close();
    }
    public closeAssetsClicked(): void {
        this.clickAssetsPanel = false;

    }
    public closeViewClicked(): void {
        this.clickViewActive = false;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
    }

    public modalOpen(jobLocation: JobLocationDetailsModal): void {
        this.opendedJobDetails = jobLocation;
        if ((this.opendedJobDetails.CountryCode === 'USA') || this.opendedJobDetails.CountryCode === 'US') {
            this.UoM = 'Gallons';
        }
        else {
            this.UoM = 'Litres';
        }
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
    public toggleMapView(): void {
        var toggleMap = this.FilterForm.get('ToggleMap').value;
        this.FilterForm.get('ToggleMap').patchValue(!toggleMap);
        this.toogleMap = !toggleMap;
    }
    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }
    public toggleFilterView(): void {
        this.toogleFilter = !this.toogleFilter;
    }
    // private applySubFilter(locationData: any): JobLocationDetailsModal[] {
    //     let location = true, priority = true, status = true;

    //     let filterData = locationData.filter((data: JobLocationDetailsModal) => {
    //         if (this.SelectedlocationList.length) {
    //             var ids = [];
    //             this.SelectedLocationIds = '';
    //             this.SelectedlocationList.forEach(res => { ids.push(res.Id) });
    //             this.SelectedLocationIds = ids.join();
    //             location = this.SelectedLocationIds.includes(String(data.JobID));
    //         }
    //         if (this.SelectedPriorityList.length) {
    //             var ids = [];
    //             this.SelectedPriorityIds = '';
    //             this.SelectedPriorityList.forEach(res => { ids.push(res.Id) });
    //             this.SelectedPriorityIds = ids.join();
    //             priority = this.SelectedPriorityIds.includes(String(data.highestPriority))
    //         }
    //         if (this.SelectedStatusList.length) {
    //             var ids = [];
    //             this.SelectedStatusesId = '';
    //             this.SelectedStatusList.forEach(res => { ids.push(res.Id) });
    //             this.SelectedStatusesId = ids.join();
    //             status = this.SelectedStatusesId.includes(String(data.ScheduleStatus))
    //         }
    //         return (location && priority && status);
    //     })

    //     return filterData;
    // }

    // private applyFilter() {

    //     if (this.IsCarrierChkChanged()) {
    //         return;
    //     }
    //     if (this.IsAssetChkChanged()) {
    //         return;
    //     }
    //     this.jobLocationDataForMap = this.unchangedJobLocationData;
    //     let filteredJobData = [];
    //     let filter = this.FilterForm.value;
    //     if (this.SelectedSupplierList.length) {
    //         var objSup = this.SelectedSupplierList.reduce((a, c) => Object.assign(a, { [c.Id]: c.Id }), {});
    //         filteredJobData = this.jobLocationDataForMap.filter(f => f.supplierDetails.some(o => objSup[o.Id] === o.Id));
    //     }

    //     if (filter.SelectedCarrierList.length) {
    //         var obj = filter.SelectedCarrierList.reduce((a, c) => Object.assign(a, { [c.Id]: c.Id }), {});
    //         if (filteredJobData.length) {
    //             filteredJobData = filteredJobData.filter(f => f.carrierDetails.some(o => obj[o.Id] === o.Id));
    //         }
    //         else {
    //             filteredJobData = this.jobLocationDataForMap.filter(f => f.carrierDetails.some(o => obj[o.Id] === o.Id));
    //         }
    //     }
    //     if (filter.SelectedSupplierList.length == 0 && filter.SelectedCarrierList.length == 0) {
    //         filteredJobData = this.unchangedJobLocationData;
    //     }
    //     return this.applySubFilter(filteredJobData);
    // }

    public closeChartsClicked(): void {
        this.clickChartsPanel = false;
    }

    private getDipTestDetails(siteId: string, tankId: string, noOfDays: number): void {
        this.isLoading = true;
        this.chartData = [];
        this.chartOptions = {};
        this.chartOptions = this.setChartOptions(this.UserCountry);
        this.chartLabels = [];
        this.latestReading = new DipTest();
        this.calculateMinMAx(this.clickedAssetsDetails[this.assetDetails.assetIndex].jobTankAdditionalDetails[0]);
        this.locationSubscription.add(this.buyerwallyboardservice.getDipTestDetails(siteId, tankId, noOfDays).subscribe((data) => {
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
            this.isLoading = false;
        }));
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
                        labelString: `NetVolume ( Fuels Per ${this.UoM})`
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

    public onInfoViewClick(jobLocation: JobLocationDetailsModal): void {
        window.scrollTo(0, 0);
        this.opendedJobDetails = jobLocation;
        if ((this.opendedJobDetails.CountryCode === 'USA') || ((this.opendedJobDetails.CountryCode === 'US'))) {
            this.UoM = 'Gallons';
        }
        else {
            this.UoM = 'Litres';
        }
        this.assetDetails.assetIndex = 0;
        this.clickViewActive = true;
        this.clickAssetsPanel = false;
        this.clickChartsPanel = false;
        this.FilterForm.get('ToggleMap').patchValue(true);
        this.toogleMap = true;
        this.closeAssetsClicked();
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
    public onChartsViewClick(assets: JobAssetDetail[]): void {
        this.clickChartsPanel = true;
        this.isChartDataExistSubject.next(false);
        let tanks: JobAssetDetail[] = [];
        if (assets.length) {
            tanks = assets.filter(t => t.AssetType == 2);
        }

        if (tanks.length && tanks[0].jobTankAdditionalDetails.length) {
            this.getDemandCaptureChart(tanks[0].jobTankAdditionalDetails[0]['SiteId'], 3, tanks[0].JobId);
        }
        else {
            this.isChartDataExistSubject.next(false);
            this.demandChartData = null;
        }
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
    public mapReady(map: any): void {
        this.Map = map;
        this.setCountryCentre();
    }
    public getDemandCaptureChart(siteId: string, noOfDays: number, tfxJobId: number): void {
        this.demandChartData = { siteId: siteId, noOfDays: noOfDays, tfxJobId: tfxJobId };
        this.isChartDataExistSubject.next(true);
    }
    public setCenterMap($event): void {
        if (this.UserCountry && (!this.jobLocationDataForMap || !this.jobLocationDataForMap.length)) {
            this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
            this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
            if (this.Map) {
                this.Map.setCenter({ lat: this.centerLocationLat, lng: this.centerLocationLog });
                this.Map.setZoom(5);
            }
        }
    }


    //Will this work?
    public openDRPanel(event: any) {
        this.modalClose();
        jQuery('#jobIdForDr').val(event.JobId);// set the html hidden field value i9n shared layout.cshtml
        jQuery('#demandCaptureButton').click(); // trigger click of demand capture button in layout.cshtml
    }


    // public IsCarrierChkChanged() {
    //     var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
    //     if (this.isShowCarrierManaged != isCarrierManaged) {
    //         this.selectedCarrierIds = '';
    //         this.FilterForm.get('SelectedCarrierList').patchValue([]);
    //         this.FilterForm.get('SelectedSupplierList').patchValue([]);
    //         this.isShowCarrierManaged = isCarrierManaged;
    //         // this.fetchJobLocationData();
    //         return true;
    //     }
    //     return false;
    // }

    // public IsAssetChkChanged() {
    //     var isAssetChkManaged = this.FilterForm.get('IsShowAssetJobs').value;
    //     if (this.isShowNonRetailJobs != isAssetChkManaged) {
    //         this.isShowNonRetailJobs = isAssetChkManaged;
    //         // this.fetchJobLocationData();
    //         return true;
    //     }
    //     return false;
    // }

    public SaveFilters() {
        var filterData = this.FilterForm.value;
        var filterModel = JSON.stringify(filterData);
        this.buyerwallyboardservice.saveFilters(TfxModule.BuyerWallyboardLocation, filterModel).subscribe(res => {
            if (res) {

            }
        });
    }

    public GetFilters() {
        this.buyerwallyboardservice.getFilters(TfxModule.BuyerWallyboardLocation).subscribe(res => {
            if (res && res.length > 0) {
                this.SetFilters(res);
            }
            else {
                this.IsFiltersLoaded = true; 
                this.changeDetectorRef.detectChanges();
                this.FilterForm.get('SelectedPriorityList').patchValue([{ Id: 1, Name: 'Must Go' }]);
                this.locationView.getSalesData();
            }
        });
    }

    public SetFilters(input: any) {
        this.IsFiltersLoaded = true;
        this.changeDetectorRef.detectChanges();
        var filterData = JSON.parse(input);
        if (!filterData.SelectedPriorityList || !filterData.SelectedPriorityList.length) {
            filterData.SelectedPriorityList = [{ Id: 1, Name: 'Must Go' }];
        }
        this.FilterForm.patchValue(filterData);
        this.toogleMap = filterData.ToggleMap;
        this.locationView.getSalesData();
        this.onSupplierSelect();
        this.ApplyFilters();
    }

    getFilterData() {
        var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
        this.buyerwallyboardservice.GetFilterData(isCarrierManaged).subscribe(data => {
            // this.locationList = data.map(t => {return {JobID: t.Id, JobName : t.Name}});
            this.locationList = data;

            this.DefaultLocationList=data;
            this.supplierList = this.GetUniqueItems(data.map(t => t.Suppliers).reduce((p, n) => p.concat(n), []));
            this.carrierList = this.GetUniqueItems(data.map(t => t.Carriers).reduce((p, n) => p.concat(n), []));
            this.GetFilters();
        });
        // this.ApplyFilters();
    }

    onSupplierSelect(){
        var selectedSuppliers = this.FilterForm.get('SelectedSupplierList').value;
        if(selectedSuppliers != undefined && selectedSuppliers.length > 0)
        {
            // this.locationList=this.DefaultLocationList.filter(t=>t.Suppliers.filter(t1=> selectedSuppliers.some(t2=> t2.Id == t1.Id) ).length > 0 ).map(t => {return {JobID: t.Id, JobName : t.Name}});
            this.locationList=this.DefaultLocationList.filter(t=>t.Suppliers.filter(t1=> selectedSuppliers.some(t2=> t2.Id == t1.Id) ).length > 0 );
            
        }else{
            // this.locationList= this.DefaultLocationList.map(t => {return {JobID: t.Id, JobName : t.Name}});
            this.locationList= this.DefaultLocationList;
        }
        this.locationList = this.GetUniqueItems(this.locationList.reduce((p, n) => p.concat(n), []));
    }
    GetUniqueItems(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
    }
    getJobIdsForMapEventHandler(valueEmitted) {
        this.jobIdsEmittedFromSalesComponent = valueEmitted;
        this.fetchJobLocationData();
    }

    public ResetFilters() {
        this.FilterForm.get('SelectedSupplierList').patchValue([]);
        this.FilterForm.get('SelectedlocationList').patchValue([]);
        this.FilterForm.get('SelectedPriorityList').patchValue([]);
        this.FilterForm.get('SelectedStatusList').patchValue([]);
        this.FilterForm.get('selectedLocAttributeList').patchValue([]);
        this.ApplyFilters('reset');
    }
    public ApplyFilters(msg?) {
        // this.SaveFilters();
        this.filterCount = 0;

        if (this.FilterForm) {

            var selectedSupplierList = this.FilterForm.get('SelectedSupplierList').value || [];
            this.filterCount += selectedSupplierList.length;

            var selectedlocationList = this.FilterForm.get('SelectedlocationList').value || [];
            this.filterCount += selectedlocationList.length;

            var selectedPriorityList = this.FilterForm.get('SelectedPriorityList').value || [];
            this.filterCount += selectedPriorityList.length;

            var selectedStatusList = this.FilterForm.get('SelectedStatusList').value || [];
            this.filterCount += selectedStatusList.length;

            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
            this.filterCount += selectedLocAttributeList.length;
        }
       
        if (msg == "set") {
            this.locationView.applyLoadsFilters(this.FilterForm);
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        } else if (msg == "reset") {
            this.locationView.applyLoadsFilters(this.FilterForm);
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }
        this.SaveFilters();
    }
}
