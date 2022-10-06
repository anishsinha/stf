import { Component, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DistatcherRegionModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { TfxModule } from '../../../buyer-wally-board/Models/BuyerWallyBoard';
import { GridViewComponent } from './grid-view.component';
import { TankViewMasterComponent } from './tank-view-master.component';
import { Declarations } from 'src/app/declarations.module';
import { FormGroup } from '@angular/forms';
import { WallyUtilService } from 'src/app/carrier/service/wally-utility.service';
import { InventoryDataCaptureList, LoadPriorities } from 'src/app/app.constants';
import { SalesTabViewType } from 'src/app/app.enum';


@Component({
    selector: 'app-sales-data',
    templateUrl: './sales-data.component.html',
    styleUrls: ['./sales-data.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class SalesDataComponent implements OnInit {
    public toogleMap: Boolean = true;
    public toogleFilter: Boolean = false;
    public toogleDriver: Boolean = false;
    public toogleGrid: Boolean = false;
    public toogleExpandMap: Boolean = false;
    public RegionDdlSettings: any = {};
    public CustomerDdlSettings: any = {};
    public Regions: DistatcherRegionModel[] = [];
    public customerList = [];
    public locationList = [];
    public loadingData: boolean = true;
    public PriorityDdlSettings: any = {};
    public CarrierDdlSettings: any = {};
    public LoadPriorities: any = LoadPriorities;
    public carrierList: any[] = [];

    LocationAttributeList = InventoryDataCaptureList;
    public filterCount: number = 0;

    //filter form
    salesTabFilterForm: FormGroup;

    get _selectedlocationList() { return this.salesTabFilterForm.get('SelectedlocationList').value as any[]; }
    get _selectedCustomerList() { return this.salesTabFilterForm.get('SelectedCustomerList').value as any[]; }
    get _selectedRegions() { return this.salesTabFilterForm.get('SelectedRegions').value as any[]; }
    get _selectedPriorities() { return this.salesTabFilterForm.get('SelectedPriorities').value as any[]; }
    get _selectedLocAttributeList() { return this.salesTabFilterForm.get('SelectedLocAttributeList').value as any[]; }
    get _selectedCarriers() { return this.salesTabFilterForm.get('SelectedCarriers').value as any[]; }
    
    constructor(private dispatcherService: DispatcherService, private carrierService: CarrierService, private wallyUtilService: WallyUtilService) { 
        this.salesTabFilterForm = this.wallyUtilService.getSalesTabFilterForm();
    }

    ngOnInit() {
        this.init();       
    }

    init() {
        this.RegionDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        }
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
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
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
        this.getRegions();
        this.getCarriers();
        this.GetFilters();
    }
    clickOutsideDropdown() {
        if (this.toogleFilter) {
            this.toogleFilter = false;
        }
    }
    public toggleFilterView() {
        this.toogleFilter = !this.toogleFilter;
    }

    regionChanged(): void {
        let _rgnIds = this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value)
        if (_rgnIds)
            this.getCustomerListByRegionId(_rgnIds);
        else
            this.initAllCustomers();
    }

    private getCustomerListByRegionId(SelectedRegionId: string, isShowAllLoc?: boolean) {

        this.loadingData = true;
        this.carrierService.getJobListForCarrier(SelectedRegionId, this.salesTabFilterForm.get('IsShowCarrierManaged').value, this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedCarriers').value)).subscribe(response => {
            this.loadingData = false;
            this.customerList = response;
            let SelectedCustomerList = this._selectedCustomerList.filter(t => { return this.customerList.filter(el => el.CompanyId == t.CompanyId).length > 0 });
            this.salesTabFilterForm.get('SelectedCustomerList').setValue(SelectedCustomerList || []);
            if (isShowAllLoc)
                this.initAllLocation()
        });
    }

    getCarriers() {
        this.dispatcherService.GetCarriersForSupplier().subscribe(data => {
            this.carrierList = data;
        });
    }
    public getRegions() {
        this.dispatcherService.GetDispatcherRegions().subscribe(data => {
            this.Regions = data;
            this.initAllCustomers();
        });
    }

    private initAllCustomers() {
        this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);
    }
    private initAllLocation() {

        this.locationList = [];
        this.customerList.forEach(res => {
            this.locationList = this.locationList.concat(res.Jobs);
        });

        let SelectedlocationList = this._selectedlocationList.filter(t => { return this.locationList.filter(el => el.JobID == t.JobID).length > 0 });
        this.salesTabFilterForm.get('SelectedlocationList').setValue(SelectedlocationList || []);
    }

    public onCustomerSelect() {

        if (this._selectedCustomerList && this._selectedCustomerList.length > 0) {
            this._selectedCustomerList.forEach(res => {

                let _cust = this.customerList.find(f => f.CompanyId == res.CompanyId);
                if(_cust && _cust.Jobs){
                    this.locationList = this.locationList.concat(_cust.Jobs);
                }
            });

            let SelectedlocationList = this._selectedlocationList.filter(t => { return this.locationList.filter(el => el.JobID == t.JobID).length > 0 });
            this.salesTabFilterForm.get('SelectedlocationList').setValue(SelectedlocationList || []);
        }
        else {
            this.initAllLocation();
        }
    }

    public onCarrierChange() {
        this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);
        this.ApplyFilters();
        this.SaveFilters(true);
    }
    public ShowCarrierMangedData() {
        this.salesTabFilterForm.get('SelectedCarriers').setValue([]);
        this.salesTabFilterForm.get('SelectedRegions').setValue([]);
        this.getCustomerListByRegionId(this.wallyUtilService.getIdsByList(this.salesTabFilterForm.get('SelectedRegions').value), true);      
         this.ApplyFilters();
         this.SaveFilters(true);
    }


    public SaveFilters(isTopFilter?:boolean) {
        var filterData = {};
        this.dispatcherService.getFilters(TfxModule.SupplierWallyboardSales).subscribe(res => {
            var input : any
            if (res || res=="") {
                if(res !="")
                {
                    input= JSON.parse(res);   
                    filterData= input;     
                }
                                
                if(isTopFilter){ 
                    filterData['isShowCarrierManaged'] = this.salesTabFilterForm.get('IsShowCarrierManaged').value;
                    filterData['SelectedCarriers'] = this._selectedCarriers;                  
                }else{

                    filterData['SelectedRegions'] = this._selectedRegions || [];
                    filterData['SelectedCustomerList'] = this._selectedCustomerList || [];
                    filterData['SelectedlocationList'] = this._selectedlocationList || [];
                    filterData['SelectedPriorities'] = this._selectedPriorities || [];
                    filterData['selectedLocAttributeList'] = this._selectedLocAttributeList || [];                    
                    filterData['isShowCarrierManaged'] = this.salesTabFilterForm.get('IsShowCarrierManaged').value;
                    filterData['SelectedCarriers'] = this._selectedCarriers;
                }
                this.dispatcherService.postFiltersData(TfxModule.SupplierWallyboardSales, JSON.stringify(filterData)).subscribe();
            }            
        });
    }

    public GetFilters() {
        this.dispatcherService.getFilters(TfxModule.SupplierWallyboardSales).subscribe(res => {
            if (res && res.length > 0) {
                this.SetFilters(res);
            }
        });
    }

    public SetFilters(input: any) {

        var filterData = JSON.parse(input);
        this.salesTabFilterForm.get('SelectedlocationList').setValue(filterData.SelectedlocationList || []);
        this.salesTabFilterForm.get('SelectedCustomerList').setValue(filterData.SelectedCustomerList || []);
        this.salesTabFilterForm.get('SelectedRegions').setValue(filterData.SelectedRegions || []);
        this.salesTabFilterForm.get('SelectedPriorities').setValue(filterData.SelectedPriorities || []);
        this.salesTabFilterForm.get('SelectedLocAttributeList').setValue(filterData.selectedLocAttributeList || []);
        this.salesTabFilterForm.get('SelectedCarriers').setValue(filterData.SelectedCarriers || []);
        this.salesTabFilterForm.get('IsShowCarrierManaged').setValue(filterData.isShowCarrierManaged);

        this.ApplyFilters();

        if (this._selectedRegions && this._selectedRegions.length > 0) {
            this.regionChanged();
        }
        else if (this._selectedCustomerList && this._selectedCustomerList.length > 0) {
            this.onCustomerSelect();
        }
    }

    public ResetFilters(){
        this.salesTabFilterForm.get('SelectedRegions').setValue([]);
        this.salesTabFilterForm.get('SelectedCustomerList').setValue([]);
        this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
        this.salesTabFilterForm.get('SelectedPriorities').setValue([]);
        this.salesTabFilterForm.get('SelectedLocAttributeList').setValue([]);

        this.ApplyFilters('reset');
    }

    public ApplyFilters(msg?: string) {

        this.salesTabFilterForm.get('IsApplyFilter').setValue(true);

        if (msg == "set") {
            Declarations.msgsuccess("Filter applied successfully", undefined, undefined);
        } else if (msg == "reset") {
            Declarations.msginfo("Filter reset successfully", undefined, undefined);
        }

        this.setFilterCount();
    }

    setFilterCount() {
        this.filterCount = 0;

        if (this.salesTabFilterForm.get('SalesViewType').value == SalesTabViewType.PriorityTab) {

            this.filterCount += this._selectedRegions.length;
            this.filterCount += this._selectedCustomerList.length;
            this.filterCount += this._selectedlocationList.length;
            this.filterCount += this._selectedPriorities.length;
            this.filterCount += this._selectedLocAttributeList.length;
        }
        else if (this.salesTabFilterForm.get('SalesViewType').value == SalesTabViewType.TanksTab) {

            this.filterCount += this._selectedRegions.length;
            this.filterCount += this._selectedCustomerList.length;
            this.filterCount += this._selectedLocAttributeList.length;
        }

    }
    salesViewTypeChanged(type: SalesTabViewType) {
        this.salesTabFilterForm.get('SalesViewType').setValue(type);
        this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(true);
        this.setFilterCount();
    }
}
