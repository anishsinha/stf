import { Component, OnInit, ViewChildren, QueryList, ViewChild, SimpleChanges, Input } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { Declarations } from '../../declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DataTablesResponse } from '../../shared/models/DataTable-models';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { WhereIsMyDriverModel, DriverAdditionalDetails, DistatcherRegionModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { FormGroup } from '@angular/forms';
import { DeliveryReqPriority, SelectedMapLabelEnum } from 'src/app/app.enum';
import { LoadPriorities } from 'src/app/app.constants';
declare function IsUserActive(): boolean;
export declare var google: any;

@Component({
    selector: 'app-where-is-my-driver-grid-view',
    templateUrl: './where-is-my-driver-grid-view.component.html',
    styleUrls: ['./where-is-my-driver-grid-view.component.css']
})
export class WhereIsMyDriverGridViewComponent implements OnInit {

    @Input() singleMulti: number;
    @Input() FilterForm: FormGroup;
    @Input() IsFiltersLoaded: boolean = false;
    public activePriorityTab = 1;
    public DeliveryReqPriority = DeliveryReqPriority;
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

    public toogleMap: Boolean = true;
    public toogleFilter: Boolean = false;
    public toogleDriver: Boolean = false;
    public toogleGrid: Boolean = false;
    public toogleExpandMap: Boolean = false;
    public customerList = [];

    public IsShowCarrierManaged: boolean = false;
    public SelectedCarrierIds: string = '';
    public SelectedStateIds: string = '';
    public SelectedPriorityIds: string = '';
    public SelectedDispacherIds: string = '';
    public SelectedCustomerId: string = '';
    public selectedLocAttributeIds: string = '';
    public selectedStartDate: string;
    public selectedEndDate: string;
    public IsDataLoaded: boolean = false;
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
    // public IsShouldGoLoading: boolean;
    // public IsCouldGoLoading: boolean;
    // public IsMustGoLoading: boolean;
    public backgroudchatDefault = [];
    public memberInfo: MemberInfo[] = [];
    public disableControl: boolean = false;
    changeFilterValueIntervalForMultiWindow: any;


    @Input() toogleMapFromParent: boolean = true;
    constructor(private dispatcherService: DispatcherService, private chatService: chatService, private carrierService: CarrierService) {
    }

    ngOnInit() {
        this.readOnlyModeSelection();
        this.subscribeFormChanges();

        let exportColumns = { columns: ':visible' };
        let mustGocolumnsDetails = [];
        let shouldGocolumnsDetails = [];
        let couldGocolumnsDetails = [];
        if (this.disableControl) {
            mustGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true },
            { title: 'Drop Ticket', name: 'DROPTicketNum', data: 'DROPTicketNum', "autoWidth": true }

            ];
        }
        else {
            mustGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }

            ];
        }
        this.dtMustGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 163.44,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Code));
                let selectedDispacherIds = '';
                this.FilterForm.get('SelectedDispachers').value.map(m => {
                    if (selectedDispacherIds == '') selectedDispacherIds = m.Id
                    else selectedDispacherIds += ',' + m.Id
                })
                let _carriers = []; this.FilterForm.get('SelectedCarriers').value.forEach(x => _carriers.push(x.Id));

                let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
                let _locAttributeIds = _locAttribute.join();
                let inputs = {
                    RegionId: this.SelectedRegionId,
                    States: _states,
                    Priority: DeliveryReqPriority.MustGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                    CustomerId: this.FilterForm.get('SelectedCustomerId').value,
                    Carriers: _carriers,
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: _locAttributeIds
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.loadingData = true;
                this.dispatcherService.getDispatcherLoads(inputData).subscribe((resp: DataTablesResponse) => {
                    this.MustGoSchedules = resp.data;
                    this.loadingData = false;
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

        if (this.disableControl) {
            shouldGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true },
            { title: 'Drop Ticket', name: 'DROPTicketNum', data: 'DROPTicketNum', "autoWidth": true }
            ];
        }
        else {
            shouldGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }
            ];
        }
        this.dtShouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 163.44,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Code));
                let selectedDispacherIds = '';
                this.FilterForm.get('SelectedDispachers').value.map(m => {
                    if (selectedDispacherIds == '') selectedDispacherIds = m.Id
                    else selectedDispacherIds += ',' + m.Id
                })
                let selectedCarrierIds = '';
                let selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
                selectedCarriers.map(m => {
                    if (selectedCarrierIds == '') selectedCarrierIds = m.Id
                    else selectedCarrierIds += ',' + m.Id
                })
                let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
                let _locAttributeIds = _locAttribute.join();
                let inputs = {
                    RegionId: this.SelectedRegionId,
                    States: _states,
                    Priority: DeliveryReqPriority.ShouldGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                    CustomerId: this.FilterForm.get('SelectedCustomerId').value,
                    Carriers: selectedCarrierIds,
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: _locAttributeIds
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.loadingData = true;
                this.dispatcherService.getDispatcherLoads(inputData).subscribe((resp: DataTablesResponse) => {
                    this.ShouldGoSchedules = resp.data;
                    this.loadingData = false;
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

        if (this.disableControl) {
            couldGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true },
            { title: 'Drop Ticket', name: 'DROPTicketNum', data: 'DROPTicketNum', "autoWidth": true },
            ];
        }
        else {
            couldGocolumnsDetails = [{ title: 'PO Number', name: 'PoNum', data: 'PoNum', "autoWidth": true },
            { title: 'DR ID', name: 'DR_ID', data: 'UniqueOrderNo', "autoWidth": true },
            { title: 'Driver', name: 'Name', data: 'Name', "autoWidth": true },
            { title: 'Dispatcher', name: 'DName', data: 'DName', "autoWidth": true },
            { title: 'Customer', name: 'CName', data: 'CName', "autoWidth": true },
            { title: 'Pickup', name: 'Pckup', data: 'Pckup', "autoWidth": true },
            { title: 'Location', name: 'Loc', data: 'Loc', "autoWidth": true },
            { title: 'Inventory Capture Method', name: 'LT', data: 'InventoryDataCaptureTypeName', "autoWidth": true },
            { title: 'Product Name', name: 'PrdtNm', data: 'PrdtNm', "autoWidth": true },
            { title: 'Ordered Quantity', name: 'Qty', data: 'Qty', "autoWidth": true },
            { title: 'Date', name: 'LdDate', data: 'LdDate', "autoWidth": true },
            { title: 'Status', name: 'Status', data: 'Status', "autoWidth": true }
            ];
        }
        this.dtCouldGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 163.44,
            },
            ajax: (dataTablesParameters: any, callback) => {
                let _states = []; this.FilterForm.get('SelectedStates').value.forEach(x => _states.push(x.Code));
                let selectedDispacherIds = '';
                this.FilterForm.get('SelectedDispachers').value.map(m => {
                    if (selectedDispacherIds == '') selectedDispacherIds = m.Id
                    else selectedDispacherIds += ',' + m.Id
                })
                let selectedCarrierIds = '';
                let selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
                selectedCarriers.map(m => {
                    if (selectedCarrierIds == '') selectedCarrierIds = m.Id
                    else selectedCarrierIds += ',' + m.Id
                })
                let _locAttribute = []; this.FilterForm.get('selectedLocAttributeList').value.forEach(x => _locAttribute.push(x.Id));
                let _locAttributeIds = _locAttribute.join();
                let inputs = {
                    RegionId: this.SelectedRegionId,
                    States: _states,
                    Priority: DeliveryReqPriority.CouldGo,
                    FromDate: this.FilterForm.get('FromDate').value,
                    ToDate: this.FilterForm.get('ToDate').value,
                    DriverSearch: this.SearchedKeyword,
                    DispacherId: selectedDispacherIds == '' ? null : selectedDispacherIds,
                    CustomerId: this.FilterForm.get('SelectedCustomerId').value,
                    Carriers: selectedCarrierIds,
                    IsShowCarrierManaged: this.FilterForm.get('IsShowCarrierManaged').value,
                    InventoryCaptureType: _locAttributeIds
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.loadingData = true;
                this.dispatcherService.getDispatcherLoads(inputData).subscribe((resp: DataTablesResponse) => {
                    this.CouldGoSchedules = resp.data;
                    this.loadingData = false;
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
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Dispatcher Dashboard - Selected Driver Loads', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Selected Driver Loads', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
        };
    }

    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }



    // subscribeFormChanges() {
    //     this.subscriptions.add(this.FilterForm.valueChanges
    //         .subscribe(change => {
    //             var isFilterChanged = this.IsFilterChanged();
    //             if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
    //                 this.IsDataLoaded = true;
    //                 this.filterDriverData();
    //             }
    //         }))
    // }

    subscribeFormChanges() {
        this.subscriptions.add(this.FilterForm.get('IsShowCarrierManaged').valueChanges
            .subscribe(change => {
                var isFilterChanged = this.IsFilterChanged();
                if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
                    this.IsDataLoaded = true;
                    this.filterDriverData();
                }
            }));

        this.subscriptions.add(this.FilterForm.get('SelectedCarriers').valueChanges
            .subscribe(change => {
                var isFilterChanged = this.IsFilterChanged();
                if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
                    this.IsDataLoaded = true;
                    this.filterDriverData();
                }
            }));
    }



    unSubscribeFormChanges() {
        if (this.subscriptions) {
            this.subscriptions.unsubscribe();
        }
    }

    IsFilterChanged() {
        var isFilterChanged = false;
        var isShowCarrierManaged = this.FilterForm.get('IsShowCarrierManaged').value;
        if (this.IsShowCarrierManaged != isShowCarrierManaged) {
            this.IsShowCarrierManaged = isShowCarrierManaged;
            isFilterChanged = true;
        }
        var ids = [];
        var selectedCarriers = this.FilterForm.get('SelectedCarriers').value || [];
        selectedCarriers.forEach(res => { ids.push(res.Id) });
        var selectedCarrierId = ids.join();
        if (this.SelectedCarrierIds != selectedCarrierId) {
            this.SelectedCarrierIds = selectedCarrierId;
            isFilterChanged = true;
        }

        ids = [];
        var selectedRegions = this.FilterForm.get('SelectedRegions').value || [];
        selectedRegions.forEach(res => { ids.push(res.Id) });
        var selectedRegionId = ids.join();
        if (this.SelectedRegionId != selectedRegionId) {
            this.SelectedRegionId = selectedRegionId;
            isFilterChanged = true;
        }
        ids = [];
        var selectedStates = this.FilterForm.get('SelectedStates').value || [];
        selectedStates.forEach(res => { ids.push(res.Id) });
        var selectedStateIds = ids.join();
        if (this.SelectedStateIds != selectedStateIds) {
            this.SelectedStateIds = selectedStateIds;
            isFilterChanged = true;
        }

        ids = [];
        var selectedPriorities = this.FilterForm.get('SelectedPriorities').value || [];
        selectedPriorities.forEach(res => { ids.push(res.Id) });
        var selectedPriorityIds = ids.join();
        if (this.SelectedPriorityIds != selectedPriorityIds) {
            this.SelectedPriorityIds = selectedPriorityIds;
            isFilterChanged = true;
        }
        ids = [];
        var selectedDispachers = this.FilterForm.get('SelectedDispachers').value || [];
        selectedDispachers.forEach(res => { ids.push(res.Id) });
        var selectedDispacherIds = ids.join();
        if (this.SelectedDispacherIds != selectedDispacherIds) {
            this.SelectedDispacherIds = selectedDispacherIds;
            isFilterChanged = true;
        }
        var selectedCustomerId = this.FilterForm.get('SelectedCustomerId').value;
        if (this.SelectedCustomerId != selectedCustomerId) {
            this.SelectedCustomerId = selectedCustomerId;
            isFilterChanged = true;
        }
        ids = [];
        var selectedLocAttributeList = this.FilterForm.get('selectedLocAttributeList').value || [];
        selectedLocAttributeList.forEach(res => { ids.push(res.Id) });
        var selectedLocAttributeIds = ids.join();
        if (this.selectedLocAttributeIds != selectedLocAttributeIds) {
            this.selectedLocAttributeIds = selectedLocAttributeIds;
            isFilterChanged = true;
        }
        var fromdate = this.FilterForm.get('FromDate').value;
        if (this.selectedStartDate != fromdate) {
            this.selectedStartDate = fromdate;
            isFilterChanged = true;
        }
        var todate = this.FilterForm.get('ToDate').value;
        if (this.selectedEndDate != todate) {
            this.selectedEndDate = todate;
            isFilterChanged = true;
        }
        return isFilterChanged;
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.IsFiltersLoaded && change.IsFiltersLoaded.currentValue) {
            this.IsFiltersLoaded = change.IsFiltersLoaded.currentValue;
        }
        // if (change.SelectedRegions && change.SelectedRegions.currentValue)
        //     this.onRegionSelect();
        // if (change.SelectedCustomerId && change.SelectedCustomerId.currentValue)
        //     this.customerChanged();
        // if ((change.FromDate && change.FromDate.currentValue) || (change.ToDate && change.ToDate.currentValue)) {
        //     this.filterDriverData();
        // }
        // if (change.SelectedStates && change.SelectedStates.currentValue)
        //     this.filterDriverData();
        // if (change.SelectedPriorities && change.SelectedPriorities.currentValue)
        //     this.filterDriverData();
        // if (change.SelectedDispachers && change.SelectedDispachers.currentValue)
        //     this.filterDriverData();
        if (change.toogleMapFromParent) {
            this.toogleMapFromParent = change.toogleMapFromParent.currentValue;
        }
        if (change.SelectedCarriers && change.SelectedCarriers.currentValue)
            this.filterDriverData();
        // if (change.selectedLocAttributeList && change.selectedLocAttributeList.currentValue)
        //     this.filterDriverData();
        if (change.IsShowCarrierManaged)
            this.filterDriverData();

    }
    ngAfterViewInit(): void {
        //this.getDispatcherLoads();
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

    public changeActiveTab(priority) {
        this.activePriorityTab = priority;
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
                this.initializeFilterChange();
            } else if (!this.isArrayEqual(selectedRegions, this.FilterForm.get('SelectedRegions').value)) {
                this.FilterForm.get('SelectedRegions').patchValue(selectedRegions);
                this.initializeFilterChange();
            } else if (cid && cid != this.FilterForm.get('SelectedCustomerId').value) {
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
        this.filterDriverData();
    }

    public onRegionSelect() {
        var ids = [];
        this.SelectedRegionId = '';
        this.SelectedRegionId = ids.join();
        this.regionChanged();

    }


    customerChanged(): void {
        this.filterDriverData();
    }



    setFromDate(event: any): void {
        this.filterDriverData();
    }

    setToDate(event: any): void {
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
        if (this.driverModal.modalDetails.display === "block") {
            this.showDriverDetails(this.driverModal.modalDetails.data);
        }
    }

    filterDriverData(): void {
        this.clearAllIntervals();
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
        let _priorities = []; this.FilterForm.get('SelectedPriorities').value.forEach(x => _priorities.push(x.Id));
        this.SelectedPrioritiesId = _priorities;
        this.startAutoRefreshTimer();
        this.loadingData = false;
        this.refreshDatatable();
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

    public toggleExpandMapView() {
        //this.toogleExpandMap = !this.toogleExpandMap;
        var expandMapView = this.FilterForm.get('ToggleExpandMapView').value;
        this.FilterForm.get('ToggleExpandMapView').patchValue(!expandMapView);
    }

    public toggleMapView() {
        // this.toogleMap = !this.toogleMap;
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
        //this.toogleDriver = !this.toogleDriver;
        var toggleDriverView = this.FilterForm.get('ToggleDriverView').value;
        this.FilterForm.get('ToggleDriverView').patchValue(!toggleDriverView);
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
    }

    private getCustomerListByRegionId(SelectedRegionId) {
        this.loadingData = true;
        this.carrierService.getJobListForCarrier(SelectedRegionId).subscribe(t2 => {
            this.loadingData = false;
            this.customerList = t2;
        });
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
        var isFilterChanged = this.IsFilterChanged();
        if (this.IsFiltersLoaded && (isFilterChanged || !this.IsDataLoaded)) {
            this.IsDataLoaded = true;
            this.filterDriverData();
        }
    }
}


