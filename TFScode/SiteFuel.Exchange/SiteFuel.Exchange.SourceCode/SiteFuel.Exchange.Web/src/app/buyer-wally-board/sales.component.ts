import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { LoadFilterModel, SalesFilterModal, SalesTankFilterModal, TfxModule } from './Models/BuyerWallyBoard';
import { BuyerwallyboardService } from './services/buyerwallyboard.service';
import { Declarations } from 'src/app/declarations.module';
import { FormGroup } from '@angular/forms';
import { WallyUtilService } from '../carrier/service/wally-utility.service';
import { InventoryDataCaptureList, LoadPriorities } from '../app.constants';
import { SalesTabViewType } from '../app.enum';

@Component({
    selector: 'app-sales',
    templateUrl: './sales.component.html',
    styleUrls: ['./sales.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class SalesComponent implements OnInit {
    public filterCount: number = 0;

    public CarrierDdlSettings: any = {};
    public LocationDdlSettings: any = {};
    public toogleFilter: Boolean = false;
    public Locations: LoadFilterModel[] = [];
    public Carriers: any = [];

    public LoadPriorities: any = LoadPriorities;
    public LocationAttributeList = InventoryDataCaptureList;
    public salesTabFilterForm: FormGroup;
    
    constructor(private dispatcherService: BuyerwallyboardService, private wallyUtilService: WallyUtilService) {
        this.salesTabFilterForm = this.wallyUtilService.getSalesTabFilterForm();
    }

    ngOnInit() {
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
        this.GetFilters();
    }

    getCarrierLocations() {
        this.dispatcherService.GetFilterData(this.salesTabFilterForm.get('IsShowCarrierManaged').value).subscribe(data => {
            this.Locations = data;
            this.Carriers = this.GetUniqueItems(data.map(t => t.Carriers).reduce((p, n) => p.concat(n), []));
        });
    }

    public toggleFilterView() {
        this.toogleFilter = !this.toogleFilter;
    }

    locationChanged(event?: any): void {
        this.salesTabFilterForm.get('SelectedCarriers').setValue([])
        this.setJobSuppliers();
    }

    setJobSuppliers(): void {
        this.Carriers = [];
        let SelectedLocations = this.salesTabFilterForm.get('SelectedlocationList').value as any[];
        this.Locations.map(m => {
            if (SelectedLocations.find(f => f.Id == m.Id) || SelectedLocations.length == 0) {
                if (m && m.Carriers && m.Carriers.length > 0) {
                    this.Carriers = this.Carriers.concat(m.Carriers);
                }
            }
        })
        this.Carriers = this.GetUniqueItems(this.Carriers.reduce((p, n) => p.concat(n), []));
    }

    GetUniqueItems(items: any[]) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
    }
    public ShowCarrierMangedData() {
        this.salesTabFilterForm.get('SelectedCarriers').setValue([]);
        this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
        
        this.getCarrierLocations();
        this.ApplyFilters();
        this.SaveFilters(true);
    }

    public GetFilters() {
        this.dispatcherService.getFilters(TfxModule.BuyerWallyboardSales).subscribe(res => {
            if (res && res.length > 0) {
                this.SetFilters(res);
            }
            else {
                this.getCarrierLocations();
                this.ApplyFilters();
            }
        });
    }

    public SaveFilters(isTopFilter: boolean) {
        
        this.dispatcherService.getFilters(TfxModule.BuyerWallyboardSales).subscribe(res => {
            if (res) {
                
                let filterData = {};

                var input = JSON.parse(res);
                if (isTopFilter) {
                    filterData['isShowCarrierManaged'] = this.salesTabFilterForm.get('IsShowCarrierManaged').value;
                    filterData['SelectedCarriers'] = this.salesTabFilterForm.get('SelectedCarriers').value;
                    if (input.SelectedLocations && input.SelectedLocations.length > 0) {
                        filterData['SelectedLocations'] = input.SelectedLocations || [];
                    }
                    if (input.selectedLocAttributeList && input.selectedLocAttributeList.length > 0) {
                        filterData['selectedLocAttributeList'] = input.selectedLocAttributeList || [];
                    }
                } else {

                    filterData['SelectedLocations'] = this.salesTabFilterForm.get('SelectedlocationList').value as any[] || [];
                    filterData['selectedLocAttributeList'] = this.salesTabFilterForm.get('SelectedLocAttributeList').value as any[] || [];
                    filterData['IsShowCarrierManaged'] = this.salesTabFilterForm.get('IsShowCarrierManaged').value;
                    if (input.SelectedCarriers && input.SelectedCarriers.length > 0) {
                        filterData['SelectedCarriers'] = input.SelectedCarriers || [];
                    }
                }
                this.dispatcherService.saveFilters(TfxModule.BuyerWallyboardSales, JSON.stringify(filterData)).subscribe();
            }
        });
    }

    public SetFilters(input: any) {
        let filter = JSON.parse(input);
        this.salesTabFilterForm.get('IsShowCarrierManaged').setValue(filter.isShowCarrierManaged);
        this.salesTabFilterForm.get('SelectedCarriers').setValue(filter.SelectedCarriers);
        this.salesTabFilterForm.get('SelectedlocationList').setValue(filter.SelectedLocations);
        this.salesTabFilterForm.get('SelectedLocAttributeList').setValue(filter.selectedLocAttributeList);
        
        this.getCarrierLocations();
        this.ApplyFilters();
    }

    public ResetFilters() {
        this.salesTabFilterForm.get('SelectedlocationList').setValue([]);
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

    setFilterCount(){

        this.filterCount = 0;

        let SelectedLocations = this.salesTabFilterForm.get('SelectedlocationList').value as any[];
        let selectedLocAttributeList = this.salesTabFilterForm.get('SelectedLocAttributeList').value as any[];

        if (this.salesTabFilterForm.get('SalesViewType').value == SalesTabViewType.PriorityTab) {
            this.filterCount += SelectedLocations.length;
            this.filterCount += selectedLocAttributeList.length;
        }
        else if (this.salesTabFilterForm.get('SalesViewType').value == SalesTabViewType.TanksTab) {
            this.filterCount += selectedLocAttributeList.length;
        }
    }

    triggerFilter($event: any){
        if($event)
            this.GetFilters();
    }

    salesViewTypeChanged(type: SalesTabViewType){
        this.salesTabFilterForm.get('SalesViewType').setValue(type);
        this.salesTabFilterForm.get('IsApplyFilterOnPageLoad').setValue(true);
        this.setFilterCount();
    }
}
