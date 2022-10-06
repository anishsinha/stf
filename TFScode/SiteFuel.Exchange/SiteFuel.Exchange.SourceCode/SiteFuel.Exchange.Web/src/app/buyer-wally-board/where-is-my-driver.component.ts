import { Component, OnInit, AfterViewInit, EventEmitter, ViewChildren, QueryList, OnDestroy, ViewChild, TemplateRef, SimpleChanges, OnChanges, Input, Output, ViewEncapsulation } from '@angular/core';
import { Subject, from, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { SendbirdComponent } from 'src/app/shared-components/sendbird/sendbird.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { WhereIsMyDriverModel, DriverAdditionalDetails, DistatcherRegionModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { LoadFilterModel, TfxModule } from './Models/BuyerWallyBoard';
import { BuyerwallyboardService } from './services/buyerwallyboard.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { WhereIsMyDriverMapViewComponent } from './wally-dashboard/map-view.component';
import { WhereIsMyDriverGridViewComponent } from './wally-dashboard/grid-view.component';
import { InventoryDataCaptureList, LoadPriorities, RegExConstants } from '../app.constants';

export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver',
    templateUrl: './where-is-my-driver.component.html',
    styleUrls: ['./where-is-my-driver.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class WhereIsMyDriverComponent implements OnInit, AfterViewInit, OnChanges, OnDestroy {
    @Input() singleMulti: number;
    // @Output() singleMultiOutput = new EventEmitter<number>();

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
    private AUTO_REFRESH_TIME: number = 300; // seconds
    public autoRefreshTicks: number = this.AUTO_REFRESH_TIME;
    LocationAttributeList = InventoryDataCaptureList;
    selectedLocAttributeList=[];
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

    public Locations: LoadFilterModel[] = [];
    public DefaultLocations: LoadFilterModel[] = [];
    public States: any = [];
    public Suppliers: any = [];
    public Carriers: any = [];
    public LoadPriorities: any[] = LoadPriorities;
    public StateDdlSettings: any = {};
    public PriorityDdlSettings: any = {};
    public LocationDdlSettings: any = {};
    public SelectedLocationId: string;
    public SelectedStateId: string;
    public SelectedSupplierId: string;
    public SearchedKeyword: string;
    public SelectedPrioritiesId: any = [];
    public toogleFilter: Boolean = false;
    public dtMustGoOptions: any = {};
    public dtShouldGoOptions: any = {};
    public dtCouldGoOptions: any = {};
    public selectedDriverLoadsdtOptions: any = {};
    public selectedDriverLoadsdtTrigger: Subject<any> = new Subject();
    public dtMustGoTrigger: Subject<any> = new Subject();
    public dtShouldGoTrigger: Subject<any> = new Subject();
    public dtCouldGoTrigger: Subject<any> = new Subject();
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    //@ViewChild('SelectedDriverLoad', { read: DataTableDirective, static: false }) selectedDriverLoad: DataTableDirective;
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
    subscription: Subscription;
    public carrierList: any[] = [];
    public IsDataLoaded: boolean = false;
    public filterCount: number;

    @ViewChild(SendbirdComponent) sendbirdComponent: SendbirdComponent;

    constructor(private fb: FormBuilder, private dispatcherService: BuyerwallyboardService, private chatService: chatService, private carrierService: CarrierService) {
        this.singleMulti = (localStorage.getItem('singleMulti')) ? +(localStorage.getItem('singleMulti')) : 1;
        if (this.singleMulti == 1)
            this.loadScreenType = 'All';

        var _this = this;
        window.addEventListener("beforeunload", function (e) {
            _this.SaveFilters(true);
            return;
        });
        this.setFilterForm();
    }

    ngOnInit() {
        this.readOnlyModeSelection();
        this.StateDdlSettings = {
            singleSelection: false,
            idField: 'Id',
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
        this.LocationDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: true
        }
        this.restoreFilterStates();
        this.subscribeFormChanges();
        this.GetFilters();
        //this.getFilterData();
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
            SelectedSuppliers: this.fb.control([]),
            SelectedCarriers: this.fb.control([]),
            SelectedLocations: this.fb.control([]),
            SelectedStates: this.fb.control([]),
            SelectedPriorities: this.fb.control([]),
            singleMulti: this.fb.control(this.singleMulti),
            FromDate: this.fb.control(fromDate),
            ToDate: this.fb.control(toDate),
            selectedLocAttributeList:this.fb.control([])
        });
    }

    subscribeFormChanges() {
        // this.subscriptions.add(this.FilterForm.get('SelectedLocations').valueChanges
        //     .subscribe(change => {
        //         var selectedLocation = this.setSelectedLocationId();
        //         if (this.SelectedLocationId != selectedLocation) {
        //             this.locationChanged();
        //         }
        //     }))
        this.subscriptions.add(this.FilterForm.get('SelectedStates').valueChanges
            .subscribe(change => {
                var selectedStates = this.setSelectedStateId();
                if (this.SelectedStateId != selectedStates) {
                    this.stateChanged();
                }
            }));
            this.subscriptions.add(this.FilterForm.get('SelectedSuppliers').valueChanges
            .subscribe(change => {
                var selectedSupplier = this.setSelectedSupplierId();
                if (this.SelectedSupplierId != selectedSupplier) {
                    this.suppplierChanged();
                }
            }));
        this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges
            .subscribe(change => {
                this.ShowCarrierMangedData();
            }))
    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }

    getFilterData() {
        var isCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
        this.IsDataLoaded = false;
        this.dispatcherService.GetFilterData(isCarrierManaged).subscribe(data => {
            // this.IsDataLoaded = true;
            this.Locations = data;
            this.DefaultLocations=data;
            this.States = this.GetUniqueItems(data.map(t => t.States).reduce((p, n) => p.concat(n), []));
            this.Suppliers = this.GetUniqueItems(data.map(t => t.Suppliers).reduce((p, n) => p.concat(n), []));
            this.Carriers = this.GetUniqueItems(data.map(t => t.Carriers).reduce((p, n) => p.concat(n), []));
            var selectedLocations = this.FilterForm.get('SelectedLocations').value;
            selectedLocations = selectedLocations.filter(t => { return this.Locations.filter(el => t.Id == el.Id).length > 0 });
            this.FilterForm.get('SelectedLocations').patchValue(selectedLocations);
            this.SelectedLocationId = this.setSelectedLocationId();
            this.setSelectedFilters();
        });
    }
    GetUniqueItems(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
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
                //this.loadScreenType == "Grid" ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";
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
                //  type == 'Grid' ? this.loadScreenType = "Map" : this.loadScreenType = "Grid";
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

    locationChanged(): void {
        this.SelectedLocationId = this.setSelectedLocationId();
        this.setJobSuppliers();

        //MyLocalStorage.setData(MyLocalStorage.WBF_LOCATION_KEY, this.SelectedLocations);
        //MyLocalStorage.setData(MyLocalStorage.WBF_SELECTEDSTATES_KEY, this.SelectedStates);
    }

    stateChanged(): void {
        this.SelectedStateId = this.setSelectedStateId();
        this.onSelectStates();
    }

    suppplierChanged(): void {
        this.SelectedStateId = this.setSelectedSupplierId();
        this.onSelectSupplier();
    }

    setSelectedLocationId() {
        var ids = [];
        var selectedLocations = this.FilterForm.get('SelectedLocations').value;
        selectedLocations.forEach(res => { ids.push(res.Id) });
        return ids.join();
    }


    setSelectedStateId() {
        var ids = [];
        var selectedStates = this.FilterForm.get('SelectedStates').value;
        selectedStates.forEach(res => { ids.push(res.Id) });
        return ids.join();
    }

    setSelectedSupplierId() {
        var ids = [];
        var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value;
        selectedSuppliers.forEach(res => { ids.push(res.Id) });
        return ids.join();
    }

    setJobSuppliers(): void {
        this.Suppliers = [];
        this.Carriers = [];
        this.States = [];
        this.Locations.map(m => {
            var selectedLocations = this.FilterForm.get('SelectedLocations').value;
            if (selectedLocations.find(f => f.Id == m.Id) || selectedLocations.length == 0) {
                if (m && m.Suppliers && m.Suppliers.length > 0) {
                    this.Suppliers = this.Suppliers.concat(m.Suppliers);
                }
                if (m && m.Carriers && m.Carriers.length > 0) {
                    this.Carriers = this.Carriers.concat(m.Carriers);
                }
                if (m && m.States && m.States.length > 0) {
                    this.States = this.States.concat(m.States);
                }
            }
        })
        this.States = this.GetUniqueItems(this.States.reduce((p, n) => p.concat(n), []));
        this.Suppliers = this.GetUniqueItems(this.Suppliers.reduce((p, n) => p.concat(n), []));
        this.Carriers = this.GetUniqueItems(this.Carriers.reduce((p, n) => p.concat(n), []));
        if (this.IsDataLoaded) {
            this.setSelectedFilters();
        }
    }


    onSelectStates(): void {
        this.Suppliers = [];
        this.Locations=[]
        var selectedStates = this.FilterForm.get('SelectedStates').value;
        this.Locations=this.DefaultLocations.filter(t=>selectedStates.some(t1=>t1.Id==t.States[0].Id));
        if(!selectedStates || !selectedStates.length){
            this.Locations=this.DefaultLocations;
        }
        this.Suppliers=this.Locations.map(t=>{
            return t.Suppliers;
        });
      
        this.Locations = this.GetUniqueItems(this.Locations.reduce((p, n) => p.concat(n), []));
        this.Suppliers = this.GetUniqueItems(this.Suppliers.reduce((p, n) => p.concat(n), []));
        if (this.IsDataLoaded) {
            this.setSelectedFilters();
        }
    }

    onSelectSupplier(): void {
        var selectedStates = this.FilterForm.get('SelectedStates').value;
        var selectedSupplier = this.FilterForm.get('SelectedSuppliers').value;
        selectedStates.forEach(element => {
        this.Locations=this.DefaultLocations.filter(t=>t.States.filter(t1 => t1.Id == element.Id).length > 0);
        });
        selectedSupplier.forEach(element => {
            this.Locations=this.Locations.filter(t=>t.Suppliers.filter(t1=>t1.Id == element.Id).length > 0);
        });
        this.Locations = this.GetUniqueItems(this.Locations.reduce((p, n) => p.concat(n), []));
        if (this.IsDataLoaded) {
            this.setSelectedFilters();
        }
    }

    setSelectedFilters() {
        var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value;
        var selectedCarriers = this.FilterForm.get('SelectedCarriers').value;
        var selectedStates = this.FilterForm.get('SelectedStates').value;
        selectedSuppliers = selectedSuppliers.filter(t => { return this.Suppliers.filter(el => t.Id == el.Id).length > 0 });
        selectedCarriers = selectedCarriers.filter(t => { return this.Carriers.filter(el => t.Id == el.Id).length > 0 });
        selectedStates = selectedStates.filter(t => { return this.States.filter(el => t.Id == el.Id).length > 0 });

        this.FilterForm.get('SelectedSuppliers').patchValue(selectedSuppliers);
        this.FilterForm.get('SelectedCarriers').patchValue(selectedCarriers);
        this.FilterForm.get('SelectedStates').patchValue(selectedStates);

        this.ApplyLoadsFilters();
    }

    setFromDate(event: any): void {
        if (event != '') {
            this.FilterForm.get('FromDate').patchValue(event);
        }
        var toDate = this.FilterForm.get('ToDate').value;
        var fromDate = this.FilterForm.get('FromDate').value;
        if (fromDate != '' && toDate != '' && RegExConstants.DateFormat.test(fromDate) && RegExConstants.DateFormat.test(toDate)) {
            let _fromDate = moment(fromDate).toDate();
            let _toDate = moment(toDate).toDate();
            if (_toDate < _fromDate) {
                this.FilterForm.get('ToDate').patchValue(event);
            }
            MyLocalStorage.setData(MyLocalStorage.WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            MyLocalStorage.setData(MyLocalStorage.WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
        }
    }

    setToDate(event: any): void {
        if (event != '') {
            this.FilterForm.get('ToDate').patchValue(event);
        }
        this.FilterForm.get('ToDate').patchValue(event);
        var toDate = this.FilterForm.get('ToDate').value;
        var fromDate = this.FilterForm.get('FromDate').value;
        if (fromDate != '' && toDate != '' && RegExConstants.DateFormat.test(fromDate) && RegExConstants.DateFormat.test(toDate)) {
            let _fromDate = moment(fromDate).toDate();
            let _toDate = moment(toDate).toDate();
            if (_fromDate > _toDate) {
                this.FilterForm.get('FromDate').patchValue(event);
            }
            MyLocalStorage.setData(MyLocalStorage.WBF_FROMDATE_KEY, this.FilterForm.get('FromDate').value);
            MyLocalStorage.setData(MyLocalStorage.WBF_TODATE_KEY, this.FilterForm.get('ToDate').value);
        }
    }

    public toggleMapView() {
        var expandMapView = this.FilterForm.get('ToggleMap').value;
        this.FilterForm.get('ToggleMap').patchValue(!expandMapView);
    }

    public toggleFilterView() {
        this.toogleFilter = !this.toogleFilter;
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

    public GetFilters() {
        this.dispatcherService.getFilters(TfxModule.BuyerWallyboardLoads).subscribe(res => {
            if (res && res.length > 0) {
                this.SetFilters(res);
            }
            else {
                this.getFilterData();
            }
        });
    }

    public SaveFilters(isTopFilter) {
        if(isTopFilter){
            this.dispatcherService.getFilters(TfxModule.BuyerWallyboardLoads).subscribe(res => {
                if (res && Object.keys(res).length > 0) {
                    var IsShowCarrierManaged = this.FilterForm.get("IsShowCarrierManaged").value;
                    var  SelectedCarriers=  this.FilterForm.get("SelectedCarriers").value || [];                  
                    let jsonFilterForm : FormGroup = null;                
                    jsonFilterForm = JSON.parse(res);
                    jsonFilterForm["IsShowCarrierManaged"]= IsShowCarrierManaged;
                    jsonFilterForm["SelectedCarriers"] =SelectedCarriers;
                    var filterModel = JSON.stringify(jsonFilterForm);                   
                    this.dispatcherService.saveFilters(TfxModule.BuyerWallyboardLoads, filterModel).subscribe(res => {
                        if (res) {
            
                        }
                    });
                }
            });
        }else{
            var filterModel = JSON.stringify(this.FilterForm.value);
            this.dispatcherService.saveFilters(TfxModule.BuyerWallyboardLoads, filterModel).subscribe(res => {
                if (res) {
    
                }
            });
        }
        // var filterData = this.FilterForm.value;
        // delete filterData["FromDate"];
        // delete filterData["ToDate"];
        // var filterModel = JSON.stringify(filterData);
        // this.dispatcherService.saveFilters(TfxModule.BuyerWallyboardLoads, filterModel).subscribe(res => {
        //     if (res) {
        //     }
        // });
    }

    public SetFilters(input: any) {
        var filterData = JSON.parse(input);
        this.FilterForm.patchValue(filterData);
        this.getFilterData(); 
        this.ApplyLoadsFilters();
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
            //this.toogleDriver = true;
        }

    }

    public ShowCarrierMangedData() {
        this.FilterForm.get('SelectedCarriers').patchValue([]);
        this.getFilterData();
        this.loadsGridView.applyLoadsFilters(this.FilterForm);
        this.loadsMapView.applyLoadsFilters(this.FilterForm);
    }

    public ResetLoadsFilters() {
        var toDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
        var fromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : this.TodaysDate;
        this.FilterForm.get('SelectedStates').patchValue([]);
        this.FilterForm.get('SelectedPriorities').patchValue([]);
        this.FilterForm.get('SelectedSuppliers').patchValue([]);
        this.FilterForm.get('SelectedLocations').patchValue([]);
        this.FilterForm.get('FromDate').patchValue(fromDate);
        this.FilterForm.get('ToDate').patchValue(toDate);
        this.FilterForm.get('selectedLocAttributeList').patchValue([]);
        this.ApplyLoadsFilters('reset');
    }

    public ApplyLoadsFilters(msg?) {
        this.SaveFilters(false);
        this.filterCount = 0;

        if (this.FilterForm) {

            var selectedStates = this.FilterForm.get('SelectedStates').value || [];
            this.filterCount += selectedStates.length;

            var selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value || [];
            this.filterCount += selectedSuppliers.length;

            var selectedLocations = this.FilterForm.get('SelectedLocations').value || [];
            this.filterCount += selectedLocations.length;

            var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
            this.filterCount += selectedPriorities.length;

            var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
            this.filterCount += selectedLocAttributeList.length;
        }

        this.loadsGridView.applyLoadsFilters(this.FilterForm);
        this.loadsMapView.applyLoadsFilters(this.FilterForm);

        if (msg == "set") {
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        } else if (msg == "reset") {
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }
    }
}

