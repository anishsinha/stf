import { Component, OnInit, AfterViewInit, ViewChildren, QueryList, OnDestroy, ViewChild, SimpleChanges, OnChanges, Input, ViewEncapsulation, ChangeDetectorRef, ElementRef } from '@angular/core';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { Subject, from, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { SendbirdComponent } from 'src/app/shared-components/sendbird/sendbird.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { WhereIsMyDriverModel, DriverAdditionalDetails, DistatcherRegionModel, SelectedItem } from 'src/app/carrier/models/DispatchSchedulerModels';
import { FormBuilder, FormGroup } from '@angular/forms';
import { WhereIsMyDriverMapViewComponent } from './where-is-my-driver-map-view.component';
import { WhereIsMyDriverGridViewComponent } from './where-is-my-driver-grid-view.component';
import { TfxModule } from 'src/app/app.enum';
import { InventoryDataCaptureList, LoadPriorities, RegExConstants } from 'src/app/app.constants';
export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver',
    templateUrl: './where-is-my-driver.component.html',
    styleUrls: ['./where-is-my-driver.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class WhereIsMyDriverComponent implements OnInit, AfterViewInit, OnChanges, OnDestroy {
    @Input() singleMulti: number;
    @ViewChild('myDiv') myDiv: ElementRef<HTMLElement>;

    @ViewChild(WhereIsMyDriverMapViewComponent)  loadsMapView : WhereIsMyDriverMapViewComponent;
    @ViewChild(WhereIsMyDriverGridViewComponent) loadsGridView: WhereIsMyDriverGridViewComponent;

    public selectedMaplable: any;
    public previousInfowindow: any = null;
    public previousInfowindowIndex: number = null
    public FuelUnit: string;
    public googleMap: any;
    public zoomLevel = 4;
    public centerLoactionLat = 39.1175;
    public centerLoactionLng = -103.8784;
    public MaxInputDate: Date = moment().add(1, 'year').toDate();
    public TodaysDate: string = moment().format('MM/DD/YYYY');
    public FilterForm: FormGroup;
    public IsFiltersLoaded: boolean = false;
    private AUTO_REFRESH_TIME: number = 300; // seconds
    public autoRefreshTicks: number = this.AUTO_REFRESH_TIME;
    public driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };


    public screenOptions = {
        position: 6
    };
   
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
    public Regions: DistatcherRegionModel[] = [];
    LocationAttributeList = InventoryDataCaptureList;
    selectedLocAttributeList = [];
    public RegionStates: any = [];
    public RegionDispachers: any = [];
    public LoadPriorities: any[] = LoadPriorities;
    public StateDdlSettings: any = {};
    public PriorityDdlSettings: any = {};
    public RegionDdlSettings: any = {};
    public CarrierDdlSettings: any = {};
    public SelectedRegionId: string;
    public SelectedCustomerId: string;
    public SearchedKeyword: string;
    public SelectedPrioritiesId: any = [];
    public toogleFilter: Boolean = false;
    public toogleGrid: Boolean = false;
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
    public viewLoadType = 1;
    public loadScreenType = 'All';
    public isShowCarrierManaged: boolean = false;
    public carrierList: any[] = [];
    public SelectedCarriers: any = [];
    public count: number = 0;

    @ViewChild(SendbirdComponent) sendbirdComponent: SendbirdComponent;

    constructor(private fb: FormBuilder, private dispatcherService: DispatcherService, private chatService: chatService, private carrierService: CarrierService, private cdr: ChangeDetectorRef) {
        this.singleMulti = (localStorage.getItem('singleMulti')) ? +(localStorage.getItem('singleMulti')) : 1;
        if (this.singleMulti == 1)
            this.loadScreenType = 'All';

        var _this = this;
        window.addEventListener("beforeunload", function (e) {
            _this.SaveFilters(true);
            return;
        });
    }

    ngOnInit() {
        this.setFilterForm();
        this.readOnlyModeSelection();
        this.StateDdlSettings = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.PriorityDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        }
        this.RegionDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: true
        }
        this.CarrierDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        }
        this.subscribeFormChanges();
        this.restoreFilterStates();
        this.dispatcherService.GetDispatcherRegions().subscribe(data => {
            this.Regions = data;
            this.GetFilters();
        });
        this.getCarriers();
    }

    public setFilterForm() {
        var toDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
        var fromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : this.TodaysDate;

        this.FilterForm = this.fb.group({
            IsShowCarrierManaged: this.fb.control(false),
            ToggleMap: this.fb.control(true),
            ToggleExpandMapView: this.fb.control(false),
            ToggleGrids: this.fb.control(false),
            ToggleDriverView: this.fb.control(false),
            SelectedCarriers: this.fb.control([]),
            SelectedRegions: this.fb.control([]),
            SelectedStates: this.fb.control([]),
            SelectedPriorities: this.fb.control([]),
            SelectedDispachers: this.fb.control([]),
            SelectedCustomerId: this.fb.control(null),
            singleMulti: this.fb.control(this.singleMulti),
            FromDate: this.fb.control(fromDate),
            ToDate: this.fb.control(toDate),
            selectedLocAttributeList: this.fb.control([])
        });
    }

    subscribeFormChanges() {
        this.subscriptions.add(this.FilterForm.get('SelectedRegions').valueChanges
            .subscribe(change => {
                if (change) {
                    var ids = [];
                    var SelectedRegions = change;
                    SelectedRegions.forEach(res => { ids.push(res.Id) });
                    var selectedRegionId = ids.join();
                    if (this.SelectedRegionId != selectedRegionId) {
                        this.SelectedRegionId = selectedRegionId;
                        this.regionChanged();
                    }
                }
            }))
        this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges
            .subscribe(change => {
                this.FilterForm.get('SelectedRegions').patchValue([]);
            }));
        this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges
            .subscribe(change => {
                this.FilterForm.get('SelectedCarriers').patchValue([]);
                this.FilterForm.get('SelectedRegions').patchValue([]);
                this.ApplyLoadFilters();
            }));
    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }
    getCarriers() {
        this.dispatcherService.GetCarriersForSupplier().subscribe(data => {
            this.carrierList = data;
        });
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }

    ngOnChanges(change: SimpleChanges) {
        let filterChange = localStorage.getItem("filterChange") ? localStorage.getItem("filterChange") : 0;
        if (change.singleMulti && change.singleMulti.currentValue) {
            this.viewLoadType = (localStorage.getItem('viewLoadType')) ? +(localStorage.getItem('viewLoadType')) : 1;
            if (this.singleMulti == 1) {
                this.viewLoadType = 1
                localStorage.setItem('viewLoadType', this.viewLoadType.toString());
                this.loadScreenType = "All"
                sessionStorage.setItem('loadScreenType', this.loadScreenType);
            } else if (this.singleMulti == 2 && this.viewLoadType == 0 && filterChange == 0) {
                this.viewLoadType = 2;
                localStorage.setItem('viewLoadType', this.viewLoadType.toString());
                this.loadScreenType == "Grid" ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";
                sessionStorage.setItem('loadScreenType', this.loadScreenType);
            } else if (this.viewLoadType == 2 && this.singleMulti == 2 && filterChange == 0) {
                this.loadScreenType = sessionStorage.getItem('loadScreenType');
                sessionStorage.setItem('loadScreenType', this.loadScreenType);
                this.viewLoadType = 0;
                localStorage.setItem('viewLoadType', '0');
            } else if (this.singleMulti == 2 && this.viewLoadType == 1 && filterChange == 0) {
                this.viewLoadType == 1 ? this.loadScreenType = "Map" : '';
                sessionStorage.setItem('loadScreenType', this.loadScreenType);
                this.viewLoadType = 2;
                localStorage.setItem('viewLoadType', this.viewLoadType.toString());

                this.viewLoadType = 0;
                localStorage.setItem('viewLoadType', '0');
            } else if (filterChange == 1 && this.singleMulti == 2) {
                sessionStorage.getItem('loadScreenType') ? this.loadScreenType = sessionStorage.getItem('loadScreenType') : 'All';
            }
            if (this.loadScreenType == null && this.singleMulti == 2) { this.loadScreenType = 'Map' }

        }
        filterChange = 0;
        localStorage.setItem('filterChange', filterChange.toString());
    }

    ngAfterViewInit(): void {
    }

    ngOnDestroy(): void {
        this.unSubscribeFormChanges();
        this.SaveFilters(true);
    }

    regionChanged(event?: any): void {
        this.setRegionStates();
        this.setRegionDispachers();
        if (this.SelectedRegionId != "") {
            this.getCustomerListByRegionId(this.SelectedRegionId);
        }
    }

    customerChanged(event: any): void {
        this.SelectedCustomerId = (event.target.value == "null" || event.target.value == null) ? null : event.target.value;
        //MyLocalStorage.setData(MyLocalStorage.WBF_CUSTOMER_KEY, this.SelectedCustomerId);
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
        var selectedStates = this.FilterForm.get('SelectedStates').value || [];
        selectedStates = selectedStates.filter(t => { return this.RegionStates.some(el => el.Code == t.Code); });
        this.FilterForm.get('SelectedStates').patchValue(selectedStates);
    }

    setRegionDispachers(): void {
        this.RegionDispachers = [];
        this.Regions.map(m => {
            if (this.FilterForm.get('SelectedRegions').value.find(f => f.Id == m.Id)) {
                if (m && m.Dispatchers && m.Dispatchers.length > 0) {
                    this.RegionDispachers = this.RegionDispachers.concat(m.Dispatchers);
                }
            }
        })
        this.RegionDispachers = this.GetUniqueItems(this.RegionDispachers.reduce((p, n) => p.concat(n), []));

        var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
        selectedDispachers = selectedDispachers.filter(t => { return this.RegionDispachers.some(el => el.Id == t.Id); });
        this.FilterForm.get('SelectedDispachers').patchValue(selectedDispachers);
    }

    setFromDate(event: any): void {
        if (event != '') {
            this.FilterForm.get('FromDate').patchValue(event);
        }
        var toDate = this.FilterForm.get('ToDate').value;
        var fromDate = this.FilterForm.get('FromDate').value;
        if (fromDate != '' && toDate != '' &&
            RegExConstants.DateFormat.test(fromDate) && RegExConstants.DateFormat.test(toDate)) {
            let _fromDate = moment(fromDate).toDate();
            let _toDate = moment(toDate).toDate();
            if (_toDate < _fromDate) {
                this.FilterForm.get('ToDate').patchValue(event)
            }
            MyLocalStorage.setData(MyLocalStorage.WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            MyLocalStorage.setData(MyLocalStorage.WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
        }
    }

    setToDate(event: any): void {
        if (event != '') {
            this.FilterForm.get('ToDate').patchValue(event);
        }
        var toDate = this.FilterForm.get('ToDate').value;
        var fromDate = this.FilterForm.get('FromDate').value;
        if (fromDate != '' && toDate != '' &&
            RegExConstants.DateFormat.test(fromDate) && RegExConstants.DateFormat.test(toDate)) {
            let _fromDate = moment(fromDate).toDate();
            let _toDate = moment(toDate).toDate();
            if (_fromDate > _toDate) {
                this.FilterForm.get('FromDate').patchValue(event);
            }
            MyLocalStorage.setData(MyLocalStorage.WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            MyLocalStorage.setData(MyLocalStorage.WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
        }
    }

    public toggleExpandMapView() {
        var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
        this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
    }

    public toggleMapView() {
        var expandMapView = this.FilterForm.get('ToggleMap').value;
        this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
    }

    public toggleGrids() {
        this.toogleGrid = !this.toogleGrid;
    }

    public toggleFilterView() {
        this.toogleFilter = !this.toogleFilter;
    }
    public toggleDriverView() {
        var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
        this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
    }
    public modalClose(): void {
        this.driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };
    }

    private restoreFilterStates(): void {
        if (this.disableControl == true) {
            let _selectedDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
            if (_selectedDate != '') {
                this.FilterForm.get('FromDate').patchValue(_selectedDate);
                this.FilterForm.get('ToDate').patchValue(_selectedDate);
            }
        }
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
    private getCustomerListByRegionId(SelectedRegionId) {
        this.loadingData = true;
        this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(t2 => {
            this.loadingData = false;
            this.customerList = t2;
            var selectedCustomerId = this.FilterForm.get('SelectedCustomerId').value;
            this.SelectedCustomerId = this.customerList.filter(t => t.CompanyId == selectedCustomerId).length > 0 ? selectedCustomerId : null;
            if (this.SelectedCustomerId != selectedCustomerId) {
                this.FilterForm.get('SelectedCustomerId').patchValue(this.SelectedCustomerId);
            }
        });
    }


    async SaveFilters(isTopFilter?:boolean) {
        if(isTopFilter){
            this.dispatcherService.getFilters(TfxModule.SupplierWallyboardLoads).subscribe(res => {              
                if (res && Object.keys(res).length > 0) {

                    var IsShowCarrierManaged = this.FilterForm.get("IsShowCarrierManaged").value;
                    var  SelectedCarriers=  this.FilterForm.get("SelectedCarriers").value || [];                  
                    let jsonFilterForm : FormGroup = null;                
                    jsonFilterForm = JSON.parse(res);
                    jsonFilterForm["IsShowCarrierManaged"]= IsShowCarrierManaged;
                    jsonFilterForm["SelectedCarriers"] =SelectedCarriers;
                    var filterModel = JSON.stringify(jsonFilterForm);                   
                    this.dispatcherService.postFiltersData(TfxModule.SupplierWallyboardLoads, filterModel).subscribe();
                }
            });
        }else{
            var filterModel = JSON.stringify(this.FilterForm.value);
            this.dispatcherService.postFiltersData(TfxModule.SupplierWallyboardLoads, filterModel).subscribe();
        }

    }

    getFilterData() {
        this.dispatcherService.getFilters(this.FilterForm.get('IsShowCarrierManaged').value).subscribe(data => {
            this.Regions = data;
            this.RegionStates = this.GetUniqueItems(data.map(t => t.States).reduce((p, n) => p.concat(n), []));
            this.LoadPriorities = this.GetUniqueItems(data.map(t => t.Priority).reduce((p, n) => p.concat(n), []));
            this.RegionDispachers = this.GetUniqueItems(data.map(t => t.Dispachers).reduce((p, n) => p.concat(n), []));
        });
    }
    public GetFilters() {
        this.dispatcherService.getFilters(TfxModule.SupplierWallyboardLoads).subscribe(res => {
            if (res && Object.keys(res).length > 0) {
                this.SetFilters(res);
            }
            else {
                if (this.Regions && this.Regions.length > 0) {
                    var lstRegion = [this.Regions[0]];
                    this.FilterForm.get('SelectedRegions').patchValue(lstRegion);
                }
                this.setRegionStates();
                this.setRegionDispachers();
                this.IsFiltersLoaded = true;
            }
        });
    }

    GetUniqueItems(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.some(t => t == item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
    }

    public SetFilters(input: any) {
        if (input && Object.keys(input).length > 0) {

            // setTimeout(() => {
                var jsonFilterForm = JSON.parse(input);
              
                delete jsonFilterForm["FromDate"];
                delete jsonFilterForm["ToDate"];
                this.FilterForm.patchValue(jsonFilterForm);
                if(!this.FilterForm.get('SelectedRegions').value || !this.FilterForm.get('SelectedRegions').value.length){
                    if( this.Regions && this.Regions.length)
                    {
                        var lstRegion = [this.Regions[0]];
                        this.FilterForm.get('SelectedRegions').patchValue(lstRegion);
                    }
                }
                if (jsonFilterForm.SelectedCustomerId == "") {
                    this.FilterForm.get('SelectedCustomerId').patchValue(null);
                    this.SelectedCustomerId = this.FilterForm.get('SelectedCustomerId').value;
                }
                this.IsFiltersLoaded = true;
                this.cdr.detectChanges();
                let el: HTMLElement = this.myDiv.nativeElement;
                el.click();
                this.setRegionStates();
                this.setRegionDispachers();
                let that= this;
                setTimeout(function () {
                    that.ApplyLoadFilters();
                 }, 1000);              
            // }, 1500);
        }
    }

    public ApplyLoadFilters(msg?) {
        this.SaveFilters(false);
        this.count = 0;
        if (this.FilterForm) {

            var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
            selectedRegions.forEach(res => {
                this.count++;
            });
            var selectedStates = this.FilterForm.get('SelectedStates').value || [];
            selectedStates.forEach(res => {
                this.count++;
            });
            var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
            selectedPriorities.forEach(res => {
                this.count++;
            });
            var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
            selectedDispachers.forEach(res => {
                this.count++;
            });
            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
            if (selectedLocAttributeList != null || selectedLocAttributeList != 'undefined') {
                selectedLocAttributeList.forEach(res => {
                    this.count++;
                });
            }
            else {
                this.FilterForm.get('selectedLocAttributeList').patchValue(0);
            }
            
        }

        this.loadsGridView.applyLoadsFilters(this.FilterForm);
        this.loadsMapView.applyLoadsFilters(this.FilterForm);

        if(msg=="set"){
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        }else if(msg == "reset"){
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }
    }

    public ResetLoadFilters(){
        var toDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
        var fromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : this.TodaysDate;
        this.FilterForm.get('SelectedRegions').patchValue([]);
        this.FilterForm.get('SelectedStates').patchValue([]);
        this.FilterForm.get('SelectedPriorities').patchValue([]);
        this.FilterForm.get('SelectedDispachers').patchValue([]);
        this.FilterForm.get('SelectedCustomerId').patchValue([]);
        this.FilterForm.get('FromDate').patchValue(fromDate);
        this.FilterForm.get('ToDate').patchValue(toDate);
        this.FilterForm.get('selectedLocAttributeList').patchValue([]);
       
        this.ApplyLoadFilters('reset');
    }
}

export interface SelectedState {
    Code: string;
    Name: string;
}

