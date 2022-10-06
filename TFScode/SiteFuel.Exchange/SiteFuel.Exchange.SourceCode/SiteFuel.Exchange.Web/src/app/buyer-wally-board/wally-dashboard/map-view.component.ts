import { Component, OnInit, AfterViewInit, ViewChildren, QueryList, OnDestroy, ViewChild, TemplateRef, SimpleChanges, OnChanges, Input } from '@angular/core';
import { Subject, from, Subscription, Observable } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { BuyerSendbirdComponent } from 'src/app/shared-components/sendbird/buyer-sendbird/buyer-sendbird.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { WhereIsMyDriverModel, DriverAdditionalDetails, DistatcherRegionModel, routesColor } from 'src/app/carrier/models/DispatchSchedulerModels';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { LoadFilterModel } from '../Models/BuyerWallyBoard';
import { BuyerwallyboardService } from '../services/buyerwallyboard.service';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { LoadPriorities } from 'src/app/app.constants';
declare function IsUserActive(): boolean;

export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver-map-view',
    templateUrl: './map-view.component.html',
    styleUrls: ['./map-view.component.css']
})
export class WhereIsMyDriverMapViewComponent implements OnInit {
    @Input() singleMulti: number;
    @Input() FilterForm: FormGroup;

    public selectedMaplable: any;
    public previousInfowindow: any = null;
    public previousInfowindowIndex: number = null
    public FuelUnit: string;
    public googleMap: any;
    public zoomLevel = 5;
    public centerLoactionLat = 39.1175;
    public centerLoactionLng = -103.8784;
    public MaxInputDate: Date = moment().add(1, 'year').toDate();
    public TodaysDate: string = moment().format('MM/DD/YYYY');

    private AUTO_REFRESH_TIME: number = 300; // seconds
    public autoRefreshTicks: number = this.AUTO_REFRESH_TIME;

    public driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };

    private UserCountry = "";
    private CountryCentre = {
        USA: { lat: 39.11757961, lng: -103.8784 },
        CAN: { lat: 57.88251631, lng: -98.54842922 }
    };
    public screenOptions = {
        position: 6
    };

    private searchLoadInterval: any;
    private autoRefreshInterval: any;
    private autoRefreshTimerInterval: any;
    private setCountryCenterInterval: any;
    subscriptions: Subscription = new Subscription();
    public Drivers: WhereIsMyDriverModel[] = [];
    public OfflineDrivers: WhereIsMyDriverModel[] = [];
    public allLoads: WhereIsMyDriverModel[] = [];
    public OnGoingLoads: WhereIsMyDriverModel[] = [];
    public CloneOnGoingLoads = [];
    public MustGoSchedules: WhereIsMyDriverModel[] = [];

    public ShouldGoSchedules: WhereIsMyDriverModel[] = [];

    public CouldGoSchedules: WhereIsMyDriverModel[] = [];

    public selectedDriverLoads: WhereIsMyDriverModel[] = [];
    public selectedDriverDetails: DriverAdditionalDetails = new DriverAdditionalDetails();

    public Locations: LoadFilterModel[] = [];
    public States: any = [];
    public Suppliers: any = [];
    public Carriers: any = [];
    public LoadPriorities: any[] = LoadPriorities;
    public StateDdlSettings: any = {};
    public PriorityDdlSettings: any = {};
    public LocationDdlSettings: any = {};
    public SelectedLocationId: string;
    public SearchedKeyword: string;
    public SelectedPrioritiesId: any = [];
    public toogleFilter: Boolean = false;
    public customerList = [];
    public dtMustGoOptions: any = {};
    public dtShouldGoOptions: any = {};
    public dtCouldGoOptions: any = {};
    public selectedDriverLoadsdtOptions: any = {};
    public selectedDriverLoadsdtTrigger: Subject<any> = new Subject();
    public dtMustGoTrigger: Subject<any> = new Subject();
    public dtShouldGoTrigger: Subject<any> = new Subject();
    public dtCouldGoTrigger: Subject<any> = new Subject();
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    @ViewChild('SelectedDriverLoad', { read: DataTableDirective, static: false }) selectedDriverLoad: DataTableDirective;
    public loadingData: boolean = true;
    public modalData: boolean = true;
    public IsShouldGoLoading: boolean;
    public IsCouldGoLoading: boolean;
    public IsMustGoLoading: boolean;
    public backgroudchatDefault = [];
    public memberInfo: MemberInfo[] = [];
    public disableControl: boolean = false;
    changeFilterValueIntervalForMultiWindow: any;

    @ViewChild(BuyerSendbirdComponent) sendbirdComponent: BuyerSendbirdComponent;

    constructor(private dispatcherService: BuyerwallyboardService, private chatService: chatService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.readOnlyModeSelection();
        this.setSelectedLocationId();
        this.subscribeFormChanges();
        this.dispatcherService.getDispatcherCountry().subscribe(data => {
            this.UserCountry = data;
            this.FuelUnit = (this.UserCountry === 'USA') ? 'Gallons' : 'Litres';
            this.setMapCenter();
        });
        this.chatService.loaderDetails.subscribe((data) => {
            if (data != undefined) {
                this.loadingData = data;
            }
        });
        this.chatService.memberInfoDetails.subscribe((data) => {
            if (data != undefined) {
                this.memberInfo = data as MemberInfo[];
                this.loadingData = false;
                jQuery('#btnconfirm-memberInfo').click();
            }
        });
        this.changeFilterValueIntervalForMultiWindow = setInterval(() => {
            if (IsUserActive()) {
                this.checkFilterValueChange();
            }
        }, 10000);
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }

    setSelectedLocationId() {
        var ids = [];
        var selectedLocationIds = this.FilterForm.get('SelectedLocations').value.forEach(res => { ids.push(res.Id) });
        selectedLocationIds = ids.join();
        this.SelectedLocationId = selectedLocationIds;
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.singleMulti && change.singleMulti.currentValue) {
            //alert(change.singleMulti.currentValue)
        }
    }

    ngAfterViewInit(): void {
        this.getDispatcherLoads();
        this.autoRefreshLoads();
    }

    ngOnDestroy(): void {
        this.clearAllIntervals();
        this.unSubscribeFormChanges();
        if (this.changeFilterValueIntervalForMultiWindow)
            clearInterval(this.changeFilterValueIntervalForMultiWindow);
        this.dtCouldGoTrigger.unsubscribe();
        this.dtShouldGoTrigger.unsubscribe();
        this.dtMustGoTrigger.unsubscribe();
    }

    subscribeFormChanges() {
        // this.subscriptions.add(this.FilterForm.valueChanges
        //     .subscribe(change => {
        //         if (change.FromDate && change.ToDate) {
        //             var ids = [];
        //             change.SelectedLocations.forEach(res => { ids.push(res.Id) });
        //             var selectedLocationIds = ids.join();
        //             if (selectedLocationIds != this.SelectedLocationId) {
        //                 this.CloneOnGoingLoads = [];
        //                 this.SelectedLocationId = selectedLocationIds;
        //             }
        //             this.filterDriverData();
        //         }
        //     }))

        this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(change => {
            // this.loadingData = true;    
            this.filterDriverData();
        }));
        this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(change => {
            // this.loadingData = true;       
            this.filterDriverData();
        }));

    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }

    async checkFilterValueChange() {
        if (this.singleMulti == 2) {
            let frmDate = MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY);
            let toDate = MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY);
            //let selectedLocations = MyLocalStorage.getData(MyLocalStorage.WBF_LOCATION_KEY);
            //selectedLocations == "" ? selectedLocations = [] : '';
            // let selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
            //selectedStates == "" ? selectedStates = [] : '';
            if (frmDate != '' && toDate != '' && (!(+ moment(frmDate) === + moment(this.FilterForm.get('FromDate').value)) || !(+ moment(toDate) === + moment(this.FilterForm.get('ToDate').value)))) {
                this.FilterForm.get('FromDate').patchValue(frmDate);
                this.initializeFilterChange();
            }
            //else if (!this.isArrayEqual(selectedLocations, this.FilterForm.get('SelectedLocations').value)) {
            //    this.FilterForm.get('SelectedLocations').patchValue(selectedLocations);
            //    this.initializeFilterChange();
            //}
        }
    }
    initializeFilterChange() {
        localStorage.setItem("filterChange", '1');
        window.location.reload();
    }

    setMapCenter(): void {
        if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(() => {
                this.centerLoactionLat = this.CountryCentre[this.UserCountry].lat;
                this.centerLoactionLng = this.CountryCentre[this.UserCountry].lng;
                if (this.googleMap && this.OnGoingLoads.length == 0) {
                    const bounds = new google.maps.LatLngBounds();
                    bounds.extend(new google.maps.LatLng(this.centerLoactionLat, this.centerLoactionLng));
                    this.googleMap.fitBounds(bounds);
                    this.googleMap.setZoom(5);
                } else {
                    const bounds = new google.maps.LatLngBounds();
                    this.OnGoingLoads.filter(t => t.Lat != null && t.Lng != null).forEach(x => {
                        x.statusColor = routesColor[x.SttsId];
                        bounds.extend(new google.maps.LatLng(x.Lat, x.Lng));
                    });
                    this.googleMap.fitBounds(bounds);

                    const locationbounds = new google.maps.LatLngBounds();
                    this.OnGoingLoads.forEach(x => {
                        locationbounds.extend(new google.maps.LatLng(x.dLat, x.dLng));
                    });
                    if (this.googleMap && locationbounds) {
                        this.googleMap.setCenter(locationbounds.getCenter());         
                    }
                    this.googleMap.setZoom(5);
                }
            }, 500);
        }
    }

    searchDrivers(event: any): void {
        this.SearchedKeyword = event.target.value;
        this.filterDriverData();
    }

    filterDriverData(): void {
        this.clearAllIntervals();
        this.loadingData = true;
        this.searchLoadInterval = window.setTimeout(() => {
            this.getDispatcherLoads();
            this.autoRefreshLoads();
        }, 2000);
    }

    clearAllIntervals(): void {
        if (this.searchLoadInterval) {
            clearInterval(this.searchLoadInterval);
        }
        if (this.autoRefreshInterval) {
            clearInterval(this.autoRefreshInterval);
        }
        if (this.setCountryCenterInterval) {
            clearInterval(this.setCountryCenterInterval);
        }
        if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
        }
    }

    getDispatcherLoads(statusId?): void {
        if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
        }
        let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Id));
        let _priorities = []; this.FilterForm.get('SelectedPriorities').value.forEach(x => _priorities.push(x.Id));
        this.SelectedPrioritiesId = _priorities;
        let selectedSupplierIds = '';
        this.FilterForm.get('SelectedSuppliers').value.map(m => {
            if (selectedSupplierIds == '') selectedSupplierIds = m.Id
            else selectedSupplierIds += ',' + m.Id
        })
        let selectedCarrierIds = '';
        this.FilterForm.get('SelectedCarriers').value.map(m => {
            if (selectedCarrierIds == '') selectedCarrierIds = m.Id
            else selectedCarrierIds += ',' + m.Id
        })
        var ids = [];
        this.SelectedLocationId = '';
        this.FilterForm.get('SelectedLocations').value.forEach(res => { ids.push(res.Id) });
        this.SelectedLocationId = ids.join();

        let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
        let _locAttributeIds = _locAttribute.join();

        let inputs = {
            LocationIds: this.SelectedLocationId == null ? '' : this.SelectedLocationId,
            States: _states,
            Priorities: _priorities,
            FromDate: this.FilterForm.get('FromDate').value,
            ToDate: this.FilterForm.get('ToDate').value,
            DriverSearch: this.SearchedKeyword,
            SupplierCompanyIds: selectedSupplierIds,
            CarrierCompanyIds: selectedCarrierIds,
            IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
            InventoryCaptureType:_locAttributeIds
        };
        this.loadingData = true;
        var data = this.CloneOnGoingLoads;
        var isFilter = false;

        if (statusId && this.CloneOnGoingLoads && this.CloneOnGoingLoads.length > 0) {
            data = data.filter(f => f.SttsId == statusId);
            isFilter = true;
        }
        if (!isFilter) {
            this.dispatcherService.getOnGoingLoadsForMap(inputs).subscribe((data) => {
                this.CloneOnGoingLoads = data;
                this.initailizeOnGoingLoad(data);

            });
        } else
            this.initailizeOnGoingLoad(data);
    }

    private initailizeOnGoingLoad(data) {
        this.OnGoingLoads = data;
        this.Drivers = this.OnGoingLoads.filter((thing, i, arr) => {
            return arr.indexOf(arr.find(t => t.Id === thing.Id)) === i;
        });
        this.Drivers = this.Drivers.filter(x => x.Name != null && x.Name != undefined && x.Name.trim() != '');
        //last location not available
        this.OfflineDrivers = [];
        var driverFilter = [];
        data && data.map(m => {
            if (!driverFilter.find(f => f && f.Name == m.Name)) {
                driverFilter.push(m);
                if (m.Lat == null && m.Lng == null && m.Name != null && m.Name != undefined && m.Name.trim() != '')
                    (this.Drivers && this.Drivers.filter(f => f.Name == m.Name).length > 0) ? '' : this.OfflineDrivers.push(m);
            }
        })
        //this.OfflineDrivers = data.filter(x => x.Lat == null && x.Lng == null && x.Name != null && x.Name != undefined && x.Name.trim() != '');
        this.setMapCenter();
        this.startAutoRefreshTimer();
        this.loadingData = false;
        this.addDrivertoBackground();
    }

    autoRefreshLoads(): void {
        this.autoRefreshInterval = window.setInterval(() => {
            if (IsUserActive()) {
                this.getDispatcherLoads();
            }
        }, this.AUTO_REFRESH_TIME * 1000);
    }

    startAutoRefreshTimer(): void {
        this.stopAutoRefreshTimer();
        this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
        this.autoRefreshTimerInterval = window.setInterval(() => {
            if (IsUserActive()) {
                if (this.autoRefreshTicks == 0) {
                    this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
                    this.stopAutoRefreshTimer();
                } else {
                    this.autoRefreshTicks--;
                }
            }
        }, 1000);
    }

    stopAutoRefreshTimer(): void {
        if (this.autoRefreshTimerInterval) {
            clearInterval(this.autoRefreshTimerInterval);
        }
    }

    mapReady(map: any): void {
        this.googleMap = map;
        this.setMapCenter();
    }

    setZoomLevel(): void {
        if (this.OnGoingLoads.length == 0) {
            this.setMapCenter();
        } else {
            this.zoomLevel = 8; // default zoom level
        }
    }

    public toggleExpandMapView() {
        var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
        this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
    }

    public toggleDriverView() {
        var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
        this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
    }

    public addDrivertoBackground() {
        this.Drivers.forEach(xItem => {
            this.backgroudchatDefault.push(xItem.Id);
        })
        this.sendbirdComponent.IntializeChatDefault(this.backgroudchatDefault, "");
    }
    public doChat(driverId: number) {
        this.sendbirdComponent.IntializeDriverChat(driverId, "");
    }

    public mouseHoverMarker(infoWindow, event: MouseEvent): void {
        if (this.previousInfowindow && this.previousInfowindow.isOpen) {
            this.previousInfowindow.close();
        }
        if (infoWindow) {
            this.previousInfowindow = infoWindow;
            this.previousInfowindow.isOpen = true;
            infoWindow.open();
        }
    }

    public mouseHoveOutMarker(infoWindow, event: MouseEvent, index: number = null): void {
        if (this.previousInfowindow && this.previousInfowindow.isOpen && infoWindow) {
            this.previousInfowindow.close();
            this.previousInfowindow.isOpen = false;
        }
        if (infoWindow) {
            infoWindow.close();
        }
    }

    public showDriverDetails(driver: WhereIsMyDriverModel, infoWindow: any = null): void {
        window.scrollTo(0, 0);
        this.driverModal = { modalDetails: { display: 'block', data: driver } };
        if (infoWindow && infoWindow.isOpen) {
            infoWindow.close();
        }
        this.selectedDriverDetails = new DriverAdditionalDetails();
        this.modalData = true;
        this.dispatcherService.getDriverAdditionalDetails(driver.Id).subscribe(data => {
            if (data) {
                this.selectedDriverDetails = new DriverAdditionalDetails(data);
                this.modalData = false;
            }
            else {
                this.selectedDriverDetails = new DriverAdditionalDetails();
                Declarations.msgwarning('Please try again later.', 'Something Went Wrong', 3000);
                this.modalData = false;
            }
        });
    }

    public modalClose(): void {
        this.driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };
    }

    private closePreviousWindow(index: number): void {
        if (this.previousInfowindowIndex != null && this.previousInfowindow != null) {
            this.OnGoingLoads[this.previousInfowindowIndex].routeShow = false;
            if (this.previousInfowindow && this.previousInfowindow.isOpen)
                this.previousInfowindow.close();
            this.setMapCenter();
        }
    }
    public showHideRoutes(index: number): void {
        if (index == this.previousInfowindowIndex || this.previousInfowindowIndex == null) {
            this.OnGoingLoads[index].routeShow = !this.OnGoingLoads[index].routeShow;
            if (!this.OnGoingLoads[index].routeShow)
                this.setMapCenter();
        } else {
            this.closePreviousWindow(index);
        }
        this.previousInfowindowIndex = index;
    }

    private readOnlyModeSelection(): void {
        let readonlyKey = MyLocalStorage.getData(MyLocalStorage.DSB_READONLY_KEY);
        if (readonlyKey == '') {
            this.disableControl = false;
        }
        else {
            this.disableControl = readonlyKey;
        }
        if (this.disableControl === true) {
            this.FilterForm.get('ToggleMap').patchValue(false);
        }
    }

    filterMapByStatus(statusId) {
        this.selectedMaplable = statusId;
        this.getDispatcherLoads(statusId)
    }

    isArrayEqual(value, other) {
        var type = Object.prototype.toString.call(value);
        if (type !== Object.prototype.toString.call(other)) return false;
        if (['[object Array]', '[object Object]'].indexOf(type) < 0) return false;
        var valueLen = type === '[object Array]' ? value.length : Object.keys(value).length;
        var otherLen = type === '[object Array]' ? other.length : Object.keys(other).length;
        if (valueLen !== otherLen) return false;
        var compare = function (item1, item2) {
        };
        var match;
        if (type === '[object Array]') {
            for (var i = 0; i < valueLen; i++) {
                compare(value[i], other[i]);
            }
        } else {
            for (var key in value) {
                if (value.hasOwnProperty(key)) {
                    compare(value[key], other[key]);
                }
            }
        }
        return true;

    }
    public applyLoadsFilters(filterForm:FormGroup){
        this.FilterForm = filterForm;
        this.filterDriverData();
    }
}

