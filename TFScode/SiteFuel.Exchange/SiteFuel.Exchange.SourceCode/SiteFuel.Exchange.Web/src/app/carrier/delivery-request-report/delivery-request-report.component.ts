import { Component, OnInit, ViewChildren, QueryList, ViewChild } from '@angular/core';
import { DistatcherRegionModel, DRReportFilterViewModel, DeliveryRequestReportGridModel, DRReportFilterInputViewModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DataTableDirective } from 'angular-datatables';
import { CarrierService} from 'src/app/carrier/service/carrier.service';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { DataTablesResponse } from 'src/app/shared/models/DataTable-models';
import { Subject } from 'rxjs';
@Component({
  selector: 'app-delivery-request-report',
  templateUrl: './delivery-request-report.component.html',
  styleUrls: ['./delivery-request-report.component.css']
})
export class DeliveryRequestReportComponent implements OnInit {

    //filter and dropdown variables
    public LocationDdlSettings: any = {};
    public RegionDdlSettings: any = {};
    public Regions: DropdownItemExt[] = [];
    public Locations:  DropdownItem[] = [];
    public SelectedRegions = [];
    public SelectedLocations = [];

    // data binding variables
    public DRReportsData: DeliveryRequestReportGridModel[] = [];
    public unchangedDRReportsData: DeliveryRequestReportGridModel[] = [];

    public SelectedRegionId: string;
    public SelectedLocationId: string;

    public IsLoading: boolean = false;

    // using these two values in deselect as ngModel not updating after deselect event
    public SelectedRegionsForFilter: DropdownItemExt[] = []; 
    public SelectedLocationsForFilter: DropdownItemExt[] = [];


  
    //grid config varibales 
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    public dtDRGridOptions: any = {};
    public dtDRReportTrigger: Subject<any> = new Subject();
    

    constructor(private carrierServ: CarrierService) { }

    ngOnInit() {

        // this.ToDate = this.TodaysDate;
        // this.FromDate = moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');

        //this.ToDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_TODATE_KEY) : this.TodaysDate;
        //this.FromDate = this.singleMulti == 2 && MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) ? MyLocalStorage.getData(MyLocalStorage.WBF_FROMDATE_KEY) : moment(this.TodaysDate).add('day', -1).format('MM/DD/YYYY');
        let exportColumns = { columns: ':visible' };
        let DRcolumnsDetails = [];

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
        DRcolumnsDetails = [
            { title: 'Location', name: 'Location', data: 'Location', "autoWidth": true },
            { title: 'Region', name: 'RegionName', data: 'RegionName', "autoWidth": true },
            { title: 'Customer', name: 'CustomerName', data: 'CustomerName', "autoWidth": true },
            { title: 'Customer BrandID', name: 'CustomerBrandID', data: 'Customer BrandID', "autoWidth": true },
            { title: 'Product', name: 'ProductType', data: 'ProductType', "autoWidth": true },
            { title: 'Requested Qty', name: 'RequestedQty', data: 'RequestedQuantity', "autoWidth": true },
            { title: 'LocationId', name: 'LocationId', data: 'LocationId', "autoWidth": true },
            { title: 'Order', name: 'Order', data: 'PoNumber', "autoWidth": true }
        ];

        this.dtDRGridOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            //searching: true,
            dom: '<"html5buttons"B>lTfgitp',
            autoWidth: true,
            fixedHeader: true,
            //ordering: false,
            search: true,
            destroy: true,
            order: [],
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Delivery Request Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Delivery Request Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ]
        };

        this.IsLoading = true;
        this.carrierServ.getDRReportFilters().subscribe((data: DRReportFilterViewModel) => {
            if (data != null && data != undefined) {
                
                let regionsIds = [];
                let locationIds = [];
                this.Regions = data.Regions;
                this.Locations = data.Locations;

                this.SelectedRegionId = '';
                this.Regions.forEach(res => { regionsIds.push(res.Id) });
                this.SelectedRegionId = regionsIds.join();
                
                this.SelectedLocationId = '';
                this.Locations.forEach(res => { locationIds.push(res.Id) });
                this.SelectedLocationId = locationIds.join();
                this.getDRReportGridData();
                
            }
        });
        this.IsLoading = false;
        
    }


    ngAfterViewInit(): void {
        this.dtDRReportTrigger.next();
        
    }

    ngOnDestroy(): void {
        this.dtDRReportTrigger.unsubscribe();
    }

    public OnFilterSelect(event: any, filterType: string) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {

            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id) });
            this.SelectedRegionId = regionsIds.join();           
        }
        if (filterType === 'location') {

            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id) });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
        

    }

    public onFilterSelectAll(event: any, filterType: string) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id) });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id) });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }

    public onFilterDeselect(event: any, filterType: string) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id) });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {
            this.SelectedLocationId = '';
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id) });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }

    public onFilterDeselectAll(event: any, filterType: string) {
        let regionsIds = [];
        let locationIds = [];
        if (filterType === 'region') {
            this.SelectedRegionId = '';
            this.SelectedRegions = this.Regions;
            this.SelectedRegions.forEach(res => { regionsIds.push(res.Id) });
            this.SelectedRegionId = regionsIds.join();
        }
        if (filterType === 'location') {

            this.SelectedLocationId = '';
            this.SelectedLocations = this.Locations;
            this.SelectedLocations.forEach(res => { locationIds.push(res.Id) });
            this.SelectedLocationId = locationIds.join();
        }
        this.filterDRReportData(this.SelectedRegions, this.SelectedLocations);
    }


    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtDRReportTrigger.next();
            });
        }
    }


    public getDRReportGridData() {
        this.IsLoading = true;
        let inputData = new DRReportFilterInputViewModel;
        inputData.RegionIds = this.SelectedRegionId;
        inputData.LocationIds = this.SelectedLocationId;
        inputData.FromDate = '';
        inputData.ToDate = '';
        this.carrierServ.getDRReportGridData(inputData).subscribe((data: DeliveryRequestReportGridModel[]) => {
            this.unchangedDRReportsData = data;
            this.DRReportsData = data;
            this.IsLoading = false;
            this.datatableRerender();
        });
    }

    public filterDRReportData(selectedRegions: any[], selectedLocations: any[]) {
        this.IsLoading = true;
        let filteredData = [] as DeliveryRequestReportGridModel[];
        let filteredDataByRegions = [] as DeliveryRequestReportGridModel[];
         let filteredDataByLocations = [] as DeliveryRequestReportGridModel[];
        if (selectedRegions == null || selectedRegions == undefined || selectedRegions.length == 0) {
            selectedRegions = this.Regions;
        }
        if (selectedLocations == null || selectedLocations == undefined || selectedLocations.length == 0) {
            selectedLocations = this.Locations;
        }

        this.unchangedDRReportsData.forEach(function (data) {
            if (selectedRegions != null && selectedRegions != undefined && selectedRegions.length > 0) {
                for (var i = 0; i < selectedRegions.length; i++) {
                    if (selectedRegions[i].Id == data.RegionId) {
                        filteredDataByRegions.push(data);

                    }
                }
            }
        });

        if (filteredDataByRegions != null && filteredDataByRegions != undefined && filteredDataByRegions.length > 0) {
           
            filteredDataByRegions.forEach(function (data) {
                 if (selectedLocations != null && selectedLocations != undefined && selectedLocations.length > 0) {
                     for (var i = 0; i < selectedLocations.length; i++) {
                         if (selectedLocations[i].Id == data.TfxJobId) {
                             filteredDataByLocations.push(data);
                         }
                     }
                     
                 }               
            });
            this.DRReportsData = filteredDataByLocations;
            
        }
        else {
            this.DRReportsData = filteredDataByRegions;
        }

        this.datatableRerender();
        this.IsLoading = false;      
    }
    
}
