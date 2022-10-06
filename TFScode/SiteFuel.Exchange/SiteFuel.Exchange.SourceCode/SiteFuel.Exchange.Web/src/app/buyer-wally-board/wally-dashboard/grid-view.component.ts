import { Component, OnInit, AfterViewInit, ViewChildren, QueryList, OnDestroy, ViewChild, TemplateRef, SimpleChanges, OnChanges, Input } from '@angular/core';
import { Subject, from, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DataTablesResponse } from '../../shared/models/DataTable-models';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { WhereIsMyDriverModel, DriverAdditionalDetails } from 'src/app/carrier/models/DispatchSchedulerModels';
import { LoadFilterModel, TfxModule } from '../Models/BuyerWallyBoard';
import { BuyerwallyboardService } from '../services/buyerwallyboard.service';
import { FormGroup } from '@angular/forms';
import { DeliveryReqPriority, SelectedMapLabelEnum } from 'src/app/app.enum';
import { LoadPriorities } from 'src/app/app.constants';
declare function IsUserActive(): boolean;
export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver-grid-view',
    templateUrl: './grid-view.component.html',
    styleUrls: ['./grid-view.component.css']
})
export class WhereIsMyDriverGridViewComponent implements OnInit {

    @Input() singleMulti: number;
    @Input() FilterForm: FormGroup;
    public SelectedMapLabelEnum = SelectedMapLabelEnum;
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

    private AUTO_REFRESH_TIME: number = 300; // seconds
    public autoRefreshTicks: number = this.AUTO_REFRESH_TIME;

    public driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };

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
    public MustGoSchedules: WhereIsMyDriverModel[] = [];

    public ShouldGoSchedules: WhereIsMyDriverModel[] = [];

    public CouldGoSchedules: WhereIsMyDriverModel[] = [];

    public selectedDriverLoads: WhereIsMyDriverModel[] = [];
    public selectedDriverDetails: DriverAdditionalDetails = new DriverAdditionalDetails();

    public Locations: LoadFilterModel[] = [];
    public States: any = [];
    public Suppliers: any = [];
    public LoadPriorities: any[] = LoadPriorities;
    public StateDdlSettings: any = {};
    public PriorityDdlSettings: any = {};
    public RegionDdlSettings: any = {};
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
    public activePriorityTab = 1;
    public DeliveryReqPriority = DeliveryReqPriority;
    changeFilterValueIntervalForMultiWindow: any;
    showMustGo = false;
    showShouldGo = false;
    showCouldGo = false;

    constructor(private dispatcherService: BuyerwallyboardService, private chatService: chatService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.readOnlyModeSelection();
        this.subscribeFormChanges();
        let exportColumns = { columns: ':visible' };
        let mustGocolumnsDetails = [];
        let shouldGocolumnsDetails = [];
        let couldGocolumnsDetails = [];

        mustGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
        { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
        { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
        { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
        { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
        { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
        { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
        { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
        { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
        { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
        { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }];

        this.dtMustGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 200,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Id));
                let inputs = {
                    LocationIds: this.getLocationIds(),
                    States: _states,
                    Priority: DeliveryReqPriority.MustGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    SupplierCompanyIds: this.getSupplierIds(),
                    CarrierCompanyIds: this.getCarrierIds(),
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: this.getselectedLocAttributeIds()
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.IsMustGoLoading = true;
                this.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe((resp: DataTablesResponse) => {
                    this.MustGoSchedules = resp.data;
                    this.IsMustGoLoading = false;
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        data: resp.data
                    });
                });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [
                { extend: 'colvis', exportOptions: exportColumns },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Dispatcher Dashboard - Must Go', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Must Go', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: mustGocolumnsDetails
        };


        shouldGocolumnsDetails = [
            { title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }];

        this.dtShouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 200,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Id));
                let inputs = {
                    LocationIds: this.getLocationIds(),
                    States: _states,
                    Priority: DeliveryReqPriority.ShouldGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    SupplierCompanyIds: this.getSupplierIds(),
                    CarrierCompanyIds: this.getCarrierIds(),
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: this.getselectedLocAttributeIds()
                };

                let inputData = Object.assign(dataTablesParameters, inputs);
                this.IsShouldGoLoading = true;
                this.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe((resp: DataTablesResponse) => {
                    this.ShouldGoSchedules = resp.data;
                    this.IsShouldGoLoading = false;
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        data: resp.data
                    });
                });

            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [
                { extend: 'colvis', exportOptions: exportColumns },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Dispatcher Dashboard - Should Go', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Should Go', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: shouldGocolumnsDetails
        };


        couldGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
        { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
        { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
        { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
        { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
        { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
        { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
        { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
        { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
        { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
        { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }];

        this.dtCouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 200,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Id));
                let inputs = {
                    LocationIds: this.getLocationIds(),
                    States: _states,
                    Priority: DeliveryReqPriority.CouldGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    SupplierCompanyIds: this.getSupplierIds(),
                    CarrierCompanyIds: this.getCarrierIds(),
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: this.getselectedLocAttributeIds()
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.IsCouldGoLoading = true;
                this.dispatcherService.getBuyerLoadsForGrid(inputData).subscribe((resp: DataTablesResponse) => {
                    this.CouldGoSchedules = resp.data;
                    this.IsCouldGoLoading = false;
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        data: resp.data
                    });
                });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [
                { extend: 'colvis', exportOptions: exportColumns },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Dispatcher Dashboard - Could Go', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Could Go', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: couldGocolumnsDetails
        };
        this.selectedDriverLoadsdtOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis', exportOptions: exportColumns },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Dispatcher Dashboard - Selected Driver Loads', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Selected Driver Loads', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
        };

        // this.changeFilterValueIntervalForMultiWindow = setInterval(() => {
        //this.checkFilterValueChange();
        // }, 10000);

    }

    getLocationIds() {
        let locations = [];
        let selectedLocationIds = '';
        this.FilterForm.get('SelectedLocations').value.forEach(res => { locations.push(res.Id) });
        selectedLocationIds = locations.join();
        return selectedLocationIds == null ? '' : selectedLocationIds;
    }

    private getSupplierIds() {
        let selectedSupplierIds = '';
        let selectedSuppliers = this.FilterForm.get('SelectedSuppliers').value || [];
        selectedSuppliers.map(m => {
            if (selectedSupplierIds == '')
                selectedSupplierIds = m.Id;
            else
                selectedSupplierIds += ',' + m.Id;
        });
        return selectedSupplierIds;
    }

    private getCarrierIds() {
        let selectedCarrierIds = '';
        let selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
        selectedCarriers.map(m => {
            if (selectedCarrierIds == '')
                selectedCarrierIds = m.Id;
            else
                selectedCarrierIds += ',' + m.Id;
        });
        return selectedCarrierIds;
    }
    private getselectedLocAttributeIds() {
        let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
        let _locAttributeIds = _locAttribute.join();
        return _locAttributeIds;
    }

    public changeActiveTab(priority) {
        this.activePriorityTab = priority;
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }

    subscribeFormChanges() {
        // this.subscriptions.add(this.FilterForm.valueChanges
        //     .subscribe(change => {
        //         if (change.FromDate && change.ToDate) {
        //             this.filterDriverData();
        //         }
        //     }));
        this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges.subscribe(change => {
            this.loadingData = true;
            this.filterDriverData();
        }));
        this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges.subscribe(change => {
            this.loadingData = true;
            this.filterDriverData();
        }));
    }

    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.singleMulti && change.singleMulti.currentValue) {
            //alert(change.singleMulti.currentValue)
        }
    }

    ngAfterViewInit(): void {
        this.getBuyerLoads();
        this.autoRefreshLoads();
        this.dtCouldGoTrigger.next();
        this.dtShouldGoTrigger.next();
        this.dtMustGoTrigger.next();
    }

    ngOnDestroy(): void {
        this.clearAllIntervals();
        this.unSubscribeFormChanges();
        this.dtCouldGoTrigger.unsubscribe();
        this.dtShouldGoTrigger.unsubscribe();
        this.dtMustGoTrigger.unsubscribe();
    }

    async checkFilterValueChange() {
        if (this.singleMulti == 2) {
            let frmDate = MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY);
            let toDate = MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY);
            let selectedLocations = MyLocalStorage.getData(MyLocalStorage.WBF_LOCATION_KEY);
            selectedLocations == "" ? selectedLocations = [] : '';
            let selectedStates = MyLocalStorage.getData(MyLocalStorage.WBF_SELECTEDSTATES_KEY);
            selectedStates == "" ? selectedStates = [] : '';
            if (frmDate != '' && toDate != '' && (!(+ moment(frmDate) === + moment(this.FilterForm.get('FromDate').value)) || !(+ moment(toDate) === + moment(this.FilterForm.get('ToDate').value)))) {
                this.FilterForm.get('FromDate').patchValue(frmDate);
                this.initializeFilterChange();
            } else if (!this.isArrayEqual(selectedLocations, this.FilterForm.get('SelectedLocations').value)) {
                this.FilterForm.get('SelectedLocations').patchValue(selectedLocations);
                this.initializeFilterChange();
            }
        }
    }

    initializeFilterChange() {
        localStorage.setItem("filterChange", '1');
        window.location.reload();
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
        if (this.driverModal.modalDetails.display === "block") {
            this.showDriverDetails(this.driverModal.modalDetails.data);
        }
    }

    filterDriverData(): void {
        this.clearAllIntervals();
        this.searchLoadInterval = window.setTimeout(() => {
            this.getBuyerLoads();
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

    getBuyerLoads(statusId?): void {

        if (this.FilterForm.get('FromDate').value == '' || this.FilterForm.get('ToDate').value == '') {
            return;
        }
        let _priorities = []; this.FilterForm.get('SelectedPriorities').value.forEach(x => _priorities.push(x.Id));
        this.SelectedPrioritiesId = _priorities;
        if (this.SelectedPrioritiesId.length > 0) {
            this.showMustGo = this.SelectedPrioritiesId.filter(f => f == 1).length == 1 ? true : false;
            this.showShouldGo = this.SelectedPrioritiesId.filter(f => f == 2).length == 1 ? true : false;
            this.showCouldGo = this.SelectedPrioritiesId.filter(f => f == 3).length == 1 ? true : false;
        } else {
            this.showMustGo = true; this.showShouldGo = true; this.showCouldGo = true;
        }
        this.startAutoRefreshTimer();
        this.loadingData = false;
        this.refreshDatatable();
    }

    autoRefreshLoads(): void {
        this.autoRefreshInterval = window.setInterval(() => {
            if (IsUserActive()) {
                this.getBuyerLoads();
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

    public toggleGrids() {
        var toggleGrid = this.FilterForm.get('ToggleGrids').value;
        this.FilterForm.get('ToggleGrids').patchValue(!toggleGrid);
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
        this.getBuyerLoads(statusId);
    }

    arraysEqual(a, b) {
        if (a === b) return true;
        if (a == null || b == null) return false;
        if (a.length != b.length) return false;
        for (var i = 0; i < a.length; ++i) {
            if (a[i] !== b[i]) return false;
        }
        return true;
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

    public applyLoadsFilters(filterForm: FormGroup) {
        this.FilterForm = filterForm;
        this.filterDriverData();
    }
}
