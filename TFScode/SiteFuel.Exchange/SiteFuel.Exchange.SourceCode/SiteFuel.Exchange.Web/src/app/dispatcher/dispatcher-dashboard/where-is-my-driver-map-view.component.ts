import { Component, OnInit, ViewChildren, QueryList,  ViewChild, SimpleChanges, Input } from '@angular/core';
import { Subject,  Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { SendbirdComponent } from 'src/app/shared-components/sendbird/sendbird.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { WhereIsMyDriverModel, DriverAdditionalDetails, DistatcherRegionModel, routesColor } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { FormGroup } from '@angular/forms';
import { LoadPriorities } from 'src/app/app.constants';
declare function IsUserActive(): boolean;
export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver-map-view',
    templateUrl: './where-is-my-driver-map-view.component.html',
    styleUrls: ['./where-is-my-driver-map-view.component.css']
})
export class WhereIsMyDriverMapViewComponent implements OnInit {
    @Input() singleMulti: number;
   
    @Input() FilterForm: FormGroup;
    subscriptions: Subscription = new Subscription();   
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

    public SelectedCustomerId: string;
    public SelectedStates :[]; 
    public SelectedPriorities :[];
    public SelectedDispachers :[];
    public FromDate :"";
    public ToDate :"";
    public toogleMap :true

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

    public Regions: DistatcherRegionModel[] = [];
    public RegionStates: any = [];
    public RegionDispachers: any = [];
    public LoadPriorities: any[] = LoadPriorities;
    public StateDdlSettings: any = {};
    public PriorityDdlSettings: any = {};
    public RegionDdlSettings: any = {};
    public SelectedRegionId: string;
   
    public SearchedKeyword: string;
 
    public SelectedPrioritiesId: any = [];
 
    public toogleFilter: Boolean = false;
    public toogleDriver: Boolean = false;
    public toogleGrid: Boolean = false;
    public toogleExpandMap: Boolean = false;
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
    public currentOngoingLoadDetails : WhereIsMyDriverModel[] = [];
    @ViewChild(SendbirdComponent) sendbirdComponent: SendbirdComponent;

    constructor(private dispatcherService: DispatcherService, private chatService: chatService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.readOnlyModeSelection();
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
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.singleMulti && change.singleMulti.currentValue) {
           
        }
        if (change.SelectedRegions && change.SelectedRegions.currentValue)
            this.onRegionSelect();
        // if (change.SelectedCustomerId && change.SelectedCustomerId.currentValue)
        //     this.customerChanged();
        // if ((change.FromDate && change.FromDate.currentValue) || (change.ToDate && change.ToDate.currentValue))
        //     this.filterDriverData();
        // if (change.SelectedStates && change.SelectedStates.currentValue)
        //     this.filterDriverData();
        // if (change.SelectedPriorities && change.SelectedPriorities.currentValue)
        //     this.filterDriverData();
        // if (change.SelectedDispachers && change.SelectedDispachers.currentValue)
        //     this.filterDriverData();
        // if (change.SelectedCarriers && change.SelectedCarriers.currentValue)
        // this.filterDriverData();
        // if (change.IsShowCarrierManaged)
        // this.filterDriverData();
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

    // subscribeFormChanges() {
    //     this.subscriptions.add(this.FilterForm.valueChanges
    //         .subscribe(change => {
    //             var ids = [];
    //             var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
    //             selectedRegions.forEach(res => { ids.push(res.Id) });
    //             var selectedRegionId = ids.join();
    //             this.SelectedRegionId = selectedRegionId;
    //             this.filterDriverData()
    //             //this.SaveFilters();
    //         }))
    // }

    subscribeFormChanges() {
        this.subscriptions.add( this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(change =>{
                this.filterDriverData();
        }));
        this.subscriptions.add( this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(change => {
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
            let selectedRegions = MyLocalStorage.getData(MyLocalStorage.WBF_REGION_KEY);
            selectedRegions == "" ? selectedRegions = [] : '';
            let selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
            selectedStates == "" ? selectedStates = [] : '';
            let selectedDispachers = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDDISPACHER_KEY);
            let cid = MyLocalStorage.getData(MyLocalStorage.WBF_CUSTOMER_KEY);
            if (frmDate != '' && toDate != '' && (!(+ moment(frmDate) === + moment(this.FilterForm.get('FromDate').value)) || !(+ moment(toDate) === + moment(this.FilterForm.get('ToDate').value)))) {
               this.FilterForm.get('FromDate').patchValue(frmDate);
               // this.ToDate = this.ToDate;
                this.initializeFilterChange();
            } else if (!this.isArrayEqual(selectedRegions, this.FilterForm.get('selectedRegions').value)) {
                this.FilterForm.get('selectedRegions').patchValue(selectedRegions);
                this.initializeFilterChange();
            }
            else if (cid && cid != this.FilterForm.get('SelectedCustomerId')) {
                this.FilterForm.get('SelectedCustomerId').patchValue(cid);
                this.initializeFilterChange();
            }
        }
    }
    initializeFilterChange() {
        localStorage.setItem("filterChange", '1');
        window.location.reload();
    }
    regionChanged(event?: any): void {
        this.CloneOnGoingLoads = [];
        this.filterDriverData();
    }

    public onRegionSelect() {
        var ids = [];
        this.SelectedRegionId = '';
        this.FilterForm.get('SelectedRegions').value.forEach(res => { ids.push(res.Id) });
        this.SelectedRegionId = ids.join();
        this.regionChanged();
    }

    customerChanged(event): void {
        this.filterDriverData();
    }

    setRegionStates(): void {
        this.RegionStates = [];        
        this.Regions.map(m => {
            if (this.FilterForm.get('SelectedRegions').value.find(f => f.Id == m.Id)) {
                if (m && m.States && m.States.length > 0) {
                    this.RegionStates = this.RegionStates.concat(m.States);
                }
            }
        })        
    }
    setRegionDispachers(): void {
        this.Regions.map(m => {
            if (this.FilterForm.get('SelectedRegions').value.find(f => f.Id == m.Id)) {
                if (m && m.Dispatchers && m.Dispatchers.length > 0) {
                    this.RegionDispachers = this.RegionDispachers.concat(m.Dispatchers);
                }
            }
        })
    }

    setFromDate(event: any): void {
        this.filterDriverData();
    }

    setToDate(event: any): void {     
        this.filterDriverData();
    }

    onStateSelect(event: any): void {
        this.filterDriverData();
    }

    onStateUnselect(event: any): void {
        this.filterDriverData();
    }

    onPrioritySelect(event: any): void {
        this.filterDriverData();
    }

    onPriorityUnselect(event: any): void {
        this.filterDriverData();
    }
    onDispacherSelect(event: any): void {
        this.filterDriverData();
    }

    onDispacherUnselect(event: any): void {
        this.filterDriverData();
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
                    this.OnGoingLoads.forEach(x => {
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

    setDatatableData(data: WhereIsMyDriverModel[]): void {
        this.MustGoSchedules = data.filter(x => x.LdPri == 1 || x.LdPri == 0).slice();
        this.ShouldGoSchedules = data.filter(x => x.LdPri == 2).slice();
        this.CouldGoSchedules = data.filter(x => x.LdPri == 3).slice();
    }

    refreshDatatable(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                    dtInstance.draw();
                });
            }
        });


        //this.dtMustGoTrigger.next();
        //this.dtShouldGoTrigger.next();
        //this.dtCouldGoTrigger.next();
        if (this.driverModal.modalDetails.display === "block") {
            this.showDriverDetails(this.driverModal.modalDetails.data);
        }
    }

    filterDriverData(): void {
        this.clearAllIntervals();
        this.loadingData = true;
        this.searchLoadInterval = window.setTimeout(() => {
            this.getDispatcherLoads();
            this.autoRefreshLoads();
        }, 1000);
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
        let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Code));
        let _priorities = []; this.FilterForm.get('SelectedPriorities').value.forEach(x => _priorities.push(x.Id));
        this.SelectedPrioritiesId = _priorities;
        let selectedDispacherIds = '';
        this.FilterForm.get('SelectedDispachers').value.map(m => {
            if (selectedDispacherIds == '') selectedDispacherIds = m.Id
            else selectedDispacherIds += ',' + m.Id
        })
        let _carriers = []; this.FilterForm.get('SelectedCarriers').value.forEach(x => _carriers.push(x.Id));
        let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
        let _locAttributeIds = _locAttribute.join();
        let _selectedRegion = []; this.FilterForm.get('SelectedRegions').value.forEach(x => _selectedRegion.push(x.Id));
        let _selectedRegionIds = _selectedRegion.join();
        let inputs = {
            RegionId: _selectedRegionIds == null ? '' : _selectedRegionIds,
            States: _states,
            Priorities: _priorities,
            FromDate: this.FilterForm.get('FromDate').value,
            ToDate: this.FilterForm.get('ToDate').value,
            DriverSearch: this.SearchedKeyword,
            DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
            CustomerId: this.FilterForm.get('SelectedCustomerId').value,
            Carriers: _carriers,
            IsShowCarrierManaged:this.FilterForm.get('IsShowCarrierManaged').value,
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
            this.dispatcherService.getOnGoingLoads(inputs).subscribe((data) => {
                this.CloneOnGoingLoads = data;
                this.initailizeOnGoingLoad(data);

            });
        } else
            this.initailizeOnGoingLoad(data);
        //this.refreshDatatable();
        //MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDSTATES_KEY, this.SelectedStates);
       // MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDPRIORY_KEY, this.SelectedPriorities);
      //  MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDDISPACHER_KEY, this.SelectedDispachers);
     
    }

    private initailizeOnGoingLoad(data) {
        this.OnGoingLoads = data;      
        this.currentOngoingLoadDetails = this.OnGoingLoads.filter(t=>t.SttsId != null && (t.SttsId==1 ||t.SttsId==11 ||
            t.SttsId==12 ||t.SttsId==18 ||t.SttsId==20));
        // this.OnGoingLoads = data.filter(x => x.Lat != null && x.Lng != null);
        //   .map(m => {
        //       if (m.AppLastUpdatedDate)
        //           var date = new Date(m.lastUpdateTimeDiff + ' UTC');
        //       m.lastUpdateTimeDiff = date.toString();
        //       return m;
        //   } 
        //);
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

    //private  getMinutesBetweenDates(startDate) {
    //      var endDate = new Date();
    //      startDate = new Date(startDate);
    //    var diff = endDate.getTime() - startDate.getTime();
    //    return (diff / 60000);
    //  }

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
       /// this.toogleExpandMap = !this.toogleExpandMap;
        var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
        this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
    }

    public toggleMapView() {
        //this.toogleMap = !this.toogleMap;
        var expandMapView = this.FilterForm.get('ToggleMap').value;
        this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
    }

    public toggleGrids() {
        //this.toogleGrid = !this.toogleGrid;
        var toggleGrid = this.FilterForm.get('ToggleGrids').value;
        this.FilterForm.get('ToggleGrids').patchValue(!toggleGrid);
    }

    public toggleFilterView() {
        this.toogleFilter = !this.toogleFilter;
    }
    public toggleDriverView() {
       // this.toogleDriver = !this.toogleDriver;
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
        if (this.previousInfowindow !== null && this.previousInfowindow.isOpen) {
            this.previousInfowindow.close();
        }
        if (infoWindow) {
            this.previousInfowindow = infoWindow;
            this.previousInfowindow.isOpen = true;
            infoWindow.open();
        }
    }

    public mouseHoveOutMarker(infoWindow, event: MouseEvent, index: number = null): void {
        if (this.previousInfowindow !== null && this.previousInfowindow.isOpen && infoWindow !== null) {
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
                if(this.selectedDriverDetails.Trailers.length > 0)
                {
                    this.selectedDriverDetails.Trailers.forEach(t=>{
                        t.OngoingData = this.currentOngoingLoadDetails.filter(res=>res.TrailerDisplayId.split(',').indexOf(t.TruckId));                       
                    })
                }
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

    //private restoreFilterStates(): void {
    //    let _wbfRegionId = MyLocalStorage.getData(MyLocalStorage.WBF_REGION_KEY);
    //    if (_wbfRegionId != null && _wbfRegionId != "") {
    //        // this.SelectedRegionId = _wbfRegionId;
    //        this.SelectedRegions = _wbfRegionId;
    //        var ids = [];
    //        this.SelectedRegionId = '';
    //        this.SelectedRegions.forEach(res => { ids.push(res.Id) });
    //        this.SelectedRegionId = ids.join();
    //        this.getCustomerListByRegionId(this.SelectedRegionId);
    //        let _wbfCustomerName = MyLocalStorage.getData(MyLocalStorage.WBF_CUSTOMER_KEY);
    //        _wbfCustomerName ? this.SelectedCustomerId = _wbfCustomerName : '';
    //    } else {
    //        let _dsbRegionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
    //        if (_dsbRegionId != '') {
    //            //   this.SelectedRegionId = _dsbRegionId;
    //        }
    //    }
    //    let _searchKeyword = MyLocalStorage.getData(MyLocalStorage.WBF_SEARCHEDKEYWORD_KEY);
    //    if (_searchKeyword != '') {
    //        this.SearchedKeyword = _searchKeyword;
    //    }
    //    let _selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
    //    if (_selectedStates != '') {
    //        this.SelectedStates = _selectedStates;
    //    }
    //    let _selectedPriorities = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDPRIORY_KEY);
    //    if (_selectedPriorities != '') {
    //        this.SelectedPriorities = _selectedPriorities;
    //    }
    //    let _selectedDispacher = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDDISPACHER_KEY);
    //    if (_selectedDispacher != '') {
    //        this.SelectedDispachers = _selectedDispacher;
    //    }
    //    if (this.disableControl == true) {
    //        let _selectedDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
    //        if (_selectedDate != '') {
    //            this.FromDate = _selectedDate;
    //            this.ToDate = _selectedDate;
    //        }
    //    }
    //    //let _fromDate = MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY);
    //    //if (_fromDate != '') {
    //    //    this.FromDate = _fromDate;
    //    //}
    //    //let _toDate = MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY);
    //    //if (_toDate != '') {
    //    //    this.ToDate = _toDate;
    //    //}
    //}

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
            this.FilterForm.get('ToggleMap').patchValue(false)
            //this.toogleDriver = true;
        }

    }
    public loadDropTicketDetails(invoiceHeaderId: number) {
        Declarations.showSliderPanel();
        this.dispatcherService.GetDropTicketDetails(invoiceHeaderId).subscribe(data => {
            if (data != '') {
                $("#invoice").html('');
                $("#invoice").html(data);
            }
            else {
                $("#invoice").html('');
                Declarations.msgwarning('No Drop ticket details found.', null, 3000);
            }

            Declarations.appendHTMLSliderContent("#invoice");
            Declarations.hideSliderLoader();
        });
    }


    filterMapByStatus(statusId) {
        this.selectedMaplable = statusId;
        this.getDispatcherLoads(statusId)
        //this.autoRefreshLoads(statusId);
    }

    private getCustomerListByRegionId(SelectedRegionId) {
        this.loadingData = true;
        this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(t2 => {
            this.loadingData = false;
            this.customerList = t2;
        });
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
                }  }     }
        return true;

    }

    public applyLoadsFilters(filterForm:FormGroup){
        this.FilterForm = filterForm;
        this.filterDriverData();
    }
}

